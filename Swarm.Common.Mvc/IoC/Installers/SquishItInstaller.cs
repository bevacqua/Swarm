using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SquishIt.Less;
using Yahoo.Yui.Compressor;

namespace Swarm.Common.Mvc.IoC.Installers
{
    /// <summary>
    /// Registers SquishIt components, such as preprocessors.
    /// </summary>
    internal class SquishItInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<LessPreprocessor>()
                    .ImplementedBy<LessPreprocessor>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<CssCompressor>()
                    .ImplementedBy<CssCompressor>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<JavaScriptCompressor>()
                    .ImplementedBy<JavaScriptCompressor>()
                    .LifestyleTransient()
                );
        }
    }
}
