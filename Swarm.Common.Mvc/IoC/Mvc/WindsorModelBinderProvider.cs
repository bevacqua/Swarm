using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Castle.MicroKernel;

namespace Swarm.Common.Mvc.IoC.Mvc
{
    internal sealed class WindsorModelBinderProvider : IModelBinderProvider
    {
        private readonly IKernel kernel;
        private readonly IDictionary<Type, Type> modelBinderTypes;

        public WindsorModelBinderProvider(IKernel kernel, IDictionary<Type, Type> modelBinderTypes)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            if (modelBinderTypes == null)
            {
                throw new ArgumentNullException("modelBinderTypes");
            }
            this.kernel = kernel;
            this.modelBinderTypes = modelBinderTypes;
        }

        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (modelBinderTypes.ContainsKey(modelType))
            {
                Type modelBinder = modelBinderTypes[modelType];
                return (IModelBinder)kernel.Resolve(modelBinder);
            }
            return null;
        }
    }
}
