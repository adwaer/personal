﻿using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using SN.Dal;

namespace SN.WebApi.Config
{
    public class IocConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            //builder
            //    .RegisterType<LocationByIpQuery>()
            //    .AsSelf()
            //    .InstancePerRequest();

            //builder
            //    .RegisterType<LocationByCityQuery>()
            //    .AsSelf()
            //    .InstancePerRequest();

            builder
                .RegisterInstance(EntityDataSet.Instance)
                .As<EntityDataSet>();

            builder.RegisterApiControllers(Assembly.GetCallingAssembly());

            return builder.Build();
        }
    }
}
