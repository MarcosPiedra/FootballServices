using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballServices.WebAPI.AutofacModule
{
    public class Services : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<PermissionsInterceptor>()
            //       .SingleInstance()
            //       .Named<IInterceptor>("Interceptors");

            //builder.RegisterWithInterceptor<AGVsService, IAGVsService>();
            //builder.RegisterWithInterceptor<FleetService, IFleetService>();
            //builder.RegisterWithInterceptor<EnvironmentService, IEnvironmentService>();
            //builder.RegisterWithInterceptor<TreeService, ITreeService>();
            //builder.RegisterWithInterceptor<SimulationService, ISimulationService>();
            //builder.RegisterWithInterceptor<PayloadService, IPayloadService>();
            //builder.RegisterWithInterceptor<ChargerService, IChargerService>();
            //builder.RegisterWithInterceptor<ViewHelpersService, IViewHelpersService>();
            //builder.RegisterWithInterceptor<UserService, IUserService>();
            //builder.RegisterWithInterceptor<ViewRenderService, IViewRenderService>();
            //builder.RegisterWithInterceptor<ProjectsService, IProjectsService>();

            //builder.RegisterType<PermissionsService>().As<IPermissionsService>();

            //builder.RegisterType<PermissionsCached>().As<IPermissionsCached>();
        }
    }
}
