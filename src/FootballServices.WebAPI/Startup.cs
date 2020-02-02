using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FootballServices.BackgroundJob;
using FootballServices.Configurations;
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
using FootballServices.Domain.Models;
using FootballServices.SqlDataAccess;
using FootballServices.Domain;
using System.IO;
using FluentValidation.AspNetCore;
using FootballServices.Domain.Validation;
using FluentValidation;

namespace FootballServices
{
    public class Startup
    {
        public readonly IConfiguration configuration;
        private readonly bool isTest = false;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();

            this.configuration = builder.Build();

            isTest = env.IsEnvironment("Test");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connConfig = configuration.GetSection("ConnectionStrings").Get<ConnectionConfiguration>();
            var jobConfig = configuration.GetSection("Job").Get<JobConfiguration>();
            if (isTest)
            {
                var guid = Guid.NewGuid().ToString();
                File.Copy("football.db", $"{guid}.db");
                connConfig.DatabaseConnection = $"Data Source={guid}.db";
            }

            services.AddMvc()
                    .AddFluentValidation()
                    .AddNewtonsoftJson();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            services.AddApiVersioning();

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
                                         s.WithInterval(TimeSpan.FromMinutes(jobConfig.MinutesBetweenExecution))
                                          .RepeatForever())
                                     .Build();
            });
            services.AddSingleton(provider =>
            {
                var prop = new NameValueCollection { { "quartz.threadPool.threadCount", "1" } };
                var schedulerFactory = new StdSchedulerFactory(prop);
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });

            services.AddSingleton(jobConfig);

            services.AddTransient<IRepository<Manager>, EFRepository<Manager>>();
            services.AddTransient<IRepository<Player>, EFRepository<Player>>();
            services.AddTransient<IRepository<Referee>, EFRepository<Referee>>();
            services.AddTransient<IRepository<Match>, EFRepository<Match>>();
            services.AddTransient<IRepository<Statistic>, EFRepository<Statistic>>();
            services.AddTransient<IRepository<NotValidBeforeStart>, EFRepository<NotValidBeforeStart>>();

            services.AddTransient<IManagerService, ManagerService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IRefereeService, RefereeService>();
            services.AddTransient<IMatchService, MatchService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<IIncorrectAligmentEndPoint, IncorrectAligmentEndPoint>();

            services.AddTransient<IValidator<Referee>, RefereeValidator>();
            services.AddTransient<IValidator<Player>, PlayerValidator>();
            services.AddTransient<IValidator<Manager>, ManagerValidator>();
            services.AddTransient<IValidator<Match>, MatchValidator>();
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILogger<Startup> logger)
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

            if (!env.IsEnvironment("Test"))
            {
                var jobDetail = app.ApplicationServices.GetService<IJobDetail>();
                var trigger = app.ApplicationServices.GetService<ITrigger>();
                var scheduler = app.ApplicationServices.GetService<IScheduler>();
                scheduler.ScheduleJob(jobDetail, trigger);
            }
        }
    }
}
