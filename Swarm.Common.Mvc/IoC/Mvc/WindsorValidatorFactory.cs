using System;
using System.Linq;
using Castle.MicroKernel;
using FluentValidation;
using FluentValidation.Attributes;
using Swarm.Common.Helpers;

namespace Swarm.Common.Mvc.IoC.Mvc
{
    internal sealed class WindsorValidatorFactory : ValidatorFactoryBase
    {
        private readonly IKernel kernel;

        public WindsorValidatorFactory(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            this.kernel = kernel;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (validatorType == null)
            {
                throw new ArgumentNullException("validatorType");
            }
            if (!validatorType.IsGenericType || validatorType.GetGenericTypeDefinition() != typeof (IValidator<>))
            {
                throw new ArgumentException("validatorType must implement IValidator<>");
            }
            Type modelType = validatorType.GetGenericArguments().Single();
            ValidatorAttribute validatorAttribute = modelType.GetAttribute<ValidatorAttribute>();
            if (validatorAttribute == null) // if a model doesn't have a validator attribute, that model type shouldn't be validated.
            {
                return kernel.Resolve<IValidator<dynamic>>(); // we resolve a generic null validator in this case.
            }
            else
            {
                return (IValidator)kernel.Resolve(validatorType); // resolve the validator implementation.
            }
        }
    }
}
