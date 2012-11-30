using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Swarm.Common.Mvc.IoC.Mvc
{
    public sealed class ActionInvokerFilters
    {
        private readonly IList<IActionFilter> action;
        private readonly IList<IAuthorizationFilter> authorization;
        private readonly IList<IExceptionFilter> exception;
        private readonly IList<IResultFilter> result;

        public IList<IActionFilter> Action
        {
            get { return action; }
        }

        public IList<IAuthorizationFilter> Authorization
        {
            get { return authorization; }
        }

        public IList<IExceptionFilter> Exception
        {
            get { return exception; }
        }

        public IList<IResultFilter> Result
        {
            get { return result; }
        }

        public ActionInvokerFilters(
            IList<IActionFilter> action = null,
            IList<IAuthorizationFilter> authorization = null,
            IList<IExceptionFilter> exception = null,
            IList<IResultFilter> result = null)
        {
            this.action = action ?? Enumerable.Empty<IActionFilter>().ToList();
            this.authorization = authorization ?? Enumerable.Empty<IAuthorizationFilter>().ToList();
            this.exception = exception ?? Enumerable.Empty<IExceptionFilter>().ToList();
            this.result = result ?? Enumerable.Empty<IResultFilter>().ToList();
        }
    }
}
