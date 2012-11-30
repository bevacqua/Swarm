using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace Swarm.Common.Helpers
{
    public static class AutoMapperHelpers
    {
        public static IMappingExpression<TSource, TDestination> Ignoring<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression,
            Expression<Func<TDestination, object>> destination)
        {
            return expression.ForMember(
                destination,
                x => x.Ignore()
                );
        }

        public static IMappingExpression<TSource, TDestination> Ignoring<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression,
            params Expression<Func<TDestination, object>>[] destinations)
        {
            return destinations.Aggregate(expression, (current, destination) => current.Ignoring(destination));
        }
    }
}
