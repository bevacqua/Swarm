using System;
using AutoMapper;
using Castle.MicroKernel;
using Swarm.Common.Interface;

namespace Swarm.Common.AutoMapper
{
    public class Mapper : IMapper
    {
        private readonly IMappingEngine engine;

        public Mapper(IKernel kernel, IMappingEngine engine, IMapperConfigurator[] configurators)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            if (configurators == null)
            {
                throw new ArgumentNullException("configurators");
            }
            this.engine = engine;

            ConfigureEngine(kernel, configurators);
        }

        internal void ConfigureEngine(IKernel kernel, IMapperConfigurator[] configurators)
        {
            IConfiguration configuration = GetConfiguration();
            configuration.ConstructServicesUsing(kernel.Resolve);

            foreach (IMapperConfigurator configurator in configurators)
            {
                configurator.CreateMaps(this);
            }
        }

        private IConfigurationProvider GetConfigurationProvider()
        {
            IMappingEngineRunner runner = engine as IMappingEngineRunner;
            if (runner == null)
            {
                throw new ArgumentException(Resources.Error.AutoMapperInvalidEngine);
            }
            return runner.ConfigurationProvider;
        }

        private IConfiguration GetConfiguration()
        {
            IConfigurationProvider provider = GetConfigurationProvider();
            IConfiguration configuration = provider as IConfiguration;
            if (configuration == null)
            {
                throw new ArgumentException(Resources.Error.AutoMapperInvalidProvider);
            }
            return configuration;
        }

        public void AssertConfigurationIsValid()
        {
            IConfigurationProvider provider = GetConfigurationProvider();
            provider.AssertConfigurationIsValid();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return engine.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return engine.Map(source, destination);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return engine.Map(source, destination, sourceType, destinationType);
        }

        public IMappingExpression CreateMap(Type sourceType, Type destinationType)
        {
            IConfiguration configuration = GetConfiguration();
            return configuration.CreateMap(sourceType, destinationType);
        }

        public IMappingExpression CreateMap(Type sourceType, Type destinationType, MemberList source)
        {
            IConfiguration configuration = GetConfiguration();
            return configuration.CreateMap(sourceType, destinationType, source);
        }

        public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            IConfiguration configuration = GetConfiguration();
            return configuration.CreateMap<TSource, TDestination>();
        }

        public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(MemberList source)
        {
            IConfiguration configuration = GetConfiguration();
            return configuration.CreateMap<TSource, TDestination>(source);
        }
    }
}
