using System;
using System.Web.Mvc;
using Swarm.Common.Mvc.Exceptions;

namespace Swarm.Common.Mvc.Core.ActionResults
{
    public class NotFoundResult : ActionResult
    {
        public string Message { get; private set; }
        public Exception InnerException { get; private set; }

        /// <summary>
        /// Not found result that throws an exception to be handled through the application pipeline instead of dropping the request to IIS.
        /// </summary>
        public NotFoundResult(string message = null, Exception innerException = null)
        {
            Message = message;
            InnerException = innerException;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            throw new HttpNotFoundException(Message, InnerException);
        }
    }
}
