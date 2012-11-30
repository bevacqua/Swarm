using System.Web;

namespace Swarm.Common.Mvc.Interface
{
    public interface IDebugDetailsRoleAccesor
    {
        string[] GetAuthorizedRoles(HttpRequestBase request);
    }
}