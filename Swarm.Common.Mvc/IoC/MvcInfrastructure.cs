using System.Web.Mvc;
using Castle.MicroKernel;
using FluentValidation.Mvc;
using SquishIt.Framework;
using SquishIt.Less;
using Swarm.Common.Mvc.IoC.Mvc;

namespace Swarm.Common.Mvc.IoC
{
    public static class MvcInfrastructure
    {
        /// <summary>
        /// Initialize Mvc factories.
        /// </summary>
        public static void Initialize(IKernel kernel)
        {
            // inject view engine.
            IViewEngine engine = kernel.Resolve<IViewEngine>();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);

            // inject controllers.
            WindsorControllerFactory controllerFactory = new WindsorControllerFactory(kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // inject model binders.
            WindsorModelBinderProvider binderProvider = kernel.Resolve<WindsorModelBinderProvider>();
            ModelBinderProviders.BinderProviders.Add(binderProvider);

            // inject model validators.
            WindsorValidatorFactory validatorFactory = new WindsorValidatorFactory(kernel);
            FluentValidationModelValidatorProvider validatorProvider = new FluentValidationModelValidatorProvider(validatorFactory);
            ModelValidatorProviders.Providers.Add(validatorProvider);

            // initialize dotless preprocessors.
            LessPreprocessor dotless = kernel.Resolve<LessPreprocessor>();
            Bundle.RegisterStylePreprocessor(dotless);
        }
    }
}
