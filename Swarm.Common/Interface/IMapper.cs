using System;
using AutoMapper;

namespace Swarm.Common.Interface
{
    public interface IMapper
    {
        void AssertConfigurationIsValid();
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
        object Map(object source, object destination, Type sourceType, Type destinationType);
        IMappingExpression CreateMap(Type sourceType, Type destinationType);
        IMappingExpression CreateMap(Type sourceType, Type destinationType, MemberList source);
        IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>();
        IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(MemberList source);
    }
}
