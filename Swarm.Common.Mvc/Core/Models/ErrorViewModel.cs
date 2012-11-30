using System;
using System.Web;
using System.Web.Mvc;
using Swarm.Common.Mvc.Extensions;

namespace Swarm.Common.Mvc.Core.Models
{
    public class ErrorViewModel : HandleErrorInfo
    {
        private readonly HttpContextBase context;

        public bool NotFound
        {
            get { return Exception.IsHttpNotFound(); }
        }

        public bool DisplayException
        {
            get { return context.Request.CanDisplayDebuggingDetails(); }
        }

        public string Message { get; private set; }

        public ErrorViewModel(HttpContextBase context, Exception exception, string controllerName, string actionName, string message = null)
            : base(exception, controllerName, actionName)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
            Message = message;
        }
    }
}
