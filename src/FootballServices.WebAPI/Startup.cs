using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using FootballServices.BackgroundJob;
using FootballServices.Configurations;
using FootballServices.WebAPI.AutofacModule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.EntityFrameworkCore;
using FoorballServices.SqlDataAccess;

namespace FootballServices
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
#if DEBUG
                         .AddJsonFile($"appsettings.Debug.json", optional: true, reloadOnChange: true)
#endif
            .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connConfig = Configuration.GetSection("ConnectionStrings").Get<ConnectionConfiguration>();
            var jobConfig = Configuration.GetSection("Job").Get<JobConfiguration>();

            services.AddMvc()
                    .AddNewtonsoftJson()
                    .AddAutoMapper();

            services.AddControllers();

            services.AddDbContext<FootballServicesContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlite(connConfig.DatabaseConnection);
            }, ServiceLifetime.Transient);

            services.Add(new ServiceDescriptor(typeof(IJob), typeof(MatchesJob), ServiceLifetime.Transient));
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();
            services.AddSingleton(provider =>
            {
                return JobBuilder.Create<MatchesJob>()
                  .WithIdentity("Match.job", "group1")
                  .Build();
            });
            services.AddSingleton(provider =>
            {
                return TriggerBuilder.Create()
                .WithIdentity($"Match.trigger", "group1")
                .StartNow()
                .WithSimpleSchedule(s =>
                    s.WithInterval(TimeSpan.FromSeconds(5))
                     .RepeatForever())
                 .Build();
            });

            services.AddSingleton(provider =>
            {
                var prop = new NameValueCollection { { "quartz.threadPool.threadCount", jobConfig.SimultaneousJobs.ToString() } };
                var schedulerFactory = new StdSchedulerFactory(prop);
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new Services());
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILogger<Startup> logger,
                              IScheduler scheduler)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    string message = $"{context.Request.Path} {context.Request.QueryString} {context.Request.Method}";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.Log(LogLevel.Error, contextFeature.Error, message);

                        await context.Response.WriteAsync(new
                        {
                            context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });

            app.UseRouting();
            app.UseEndpoints(e => e.MapControllers());

            scheduler.ScheduleJob(app.ApplicationServices.GetService<IJobDetail>(), app.ApplicationServices.GetService<ITrigger>());
        }
    }
}
