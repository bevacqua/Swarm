using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using AutoMapper.Mappers;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Interface;
using Mapper = Swarm.Common.AutoMapper.Mapper;

namespace Swarm.Common.IoC.Installers
{
    public sealed class AutoMapperInstaller : IWindsorInstaller
    {
        private readonly Assembly[] mapperAssemblies;

        public AutoMapperInstaller(params Assembly[] mapperAssemblies)
        {
            if (mapperAssemblies == null)
            {
                throw new ArgumentNullException("mapperAssemblies");
            }
            this.mapperAssemblies = mapperAssemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (Assembly assembly in mapperAssemblies)
            {
                container.Register(
                    Classes
                        .FromAssembly(assembly)
                        .BasedOn<IMapperConfigurator>()
                        .WithServiceFromInterface(typeof(IMapperConfigurator))
                        .LifestyleTransient()
                    );
            }
            
            container.Register(
                Component
                    .For<ITypeMapFactory>()
                    .ImplementedBy<TypeMapFactory>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IConfiguration, IConfigurationProvider>()
                    .UsingFactoryMethod(InstanceConfigurationStore)
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IMappingEngine>()
                    .ImplementedBy<MappingEngine>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IMapper>()
                    .ImplementedBy<Mapper>()
                    .DynamicParameters(
                        (k, parameters) => parameters["configurators"] = container.ResolveAll<IMapperConfigurator>()
                    )
                    .LifestyleSingleton()
                );
        }

        private ConfigurationStore InstanceConfigurationStore(IKernel kernel)
        {
            ITypeMapFactory typeMapFactory = kernel.Resolve<ITypeMapFactory>();
            IEnumerable<IObjectMapper> mappers = MapperRegistry.AllMappers();

            return new ConfigurationStore(typeMapFactory, mappers);
        }
    }
}
