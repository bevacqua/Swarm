using System.Web;

namespace Swarm.Common.Mvc.HttpModules.Wiring
{
    public interface IApplicationModuleManager
    {
        void Execute(HttpApplication application);
    }
}