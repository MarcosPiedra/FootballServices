using Autofac;
using FootballServices.Domain;
using FootballServices.Domain.Models;
using FootballServices.SqlDataAccess;
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
            builder.RegisterGeneric(typeof(EFRepository<>))
                   .As(typeof(IRepository<>))
                   .InstancePerDependency();

            builder.RegisterType<ManagerService>().As<IManagerService>();
            //builder.RegisterType<EFRepository<Manager>>().As<IRepository<Manager>>();
        }
    }
}
