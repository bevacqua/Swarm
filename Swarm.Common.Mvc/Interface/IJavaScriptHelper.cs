using System;
using System.Web.Mvc;

namespace Swarm.Common.Mvc.Interface
{
    public interface IJavaScriptHelper
    {
        void Register(string path, string source, Guid? guid = null);
        MvcHtmlString Emit(Guid? guid = null);
    }
}