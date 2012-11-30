using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Routing;
using Swarm.Common.Extensions;
using Swarm.Common.Mvc.Core.Models;
using Swarm.Common.Mvc.Extensions;
using Swarm.Common.Resources;
using log4net;

namespace Swarm.Common.Mvc.Utility
{
    public class ExceptionHelper
    {
        private readonly HttpContextBase context;

        public ExceptionHelper(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
        }

        /// <summary>
        /// Gets all inner exceptions for the current exception, not including itself.
        /// </summary>
        internal Stack<Exception> GetExceptionStack(Exception exception)
        {
            Stack<Exception> stack = new Stack<Exception>();
            Exception inner = exception.InnerException;
            while (inner != null)
            {
                stack.Push(inner);
                inner = inner.InnerException;
            }
            return stack;
        }

        /// <summary>
        /// Examines the exception inside out (innermost-first) and returns the most reasonable explanation about what is going on to the user,
        /// without actually disclosing any sensitive information about the error that was raised.
        /// </summary>
        public string GetMessage(Exception exception, bool ajax)
        {
            Stack<Exception> stack = GetExceptionStack(exception);
            while (exception != null)
            {
                string specificMessage = GetSpecificExceptionMessage(exception, ajax);
                if (!specificMessage.NullOrEmpty())
                {
                    return specificMessage;
                }
                if (stack.Count == 0)
                {
                    break;
                }
                exception = stack.Pop();
            }

            string genericMessage = User.UnhandledException; // generic default exception response
            if (ajax)
            {
                genericMessage = User.UnhandledAjaxException;
            }

            return genericMessage;
        }

        internal string GetSpecificExceptionMessage(Exception exception, bool ajax)
        {
            if (exception == null)
            {
                return null;
            }
            string errorMessage = null;

            if (exception is SqlException)
            {
                errorMessage = User.DatabaseError;
            }
            else if (exception.IsHttpNotFound())
            {
                errorMessage = User.WebResourceNotFound;
            }
            return errorMessage;
        }

        public ErrorViewModel GetErrorViewModel(RouteData data, Exception exception, bool ajax = false)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            string controllerName = data.GetControllerString(Error.EmptyController);
            string actionName = data.GetActionString(Error.EmptyAction);
            string errorMessage = GetMessage(exception, ajax);
            ErrorViewModel model = new ErrorViewModel(context, exception, controllerName, actionName, errorMessage);
            return model;
        }

        /// <summary>
        /// Concatenate two exceptions where the latter is thrown while addressing the former.
        /// </summary>
        /// <param name="source">The exception that originated the event.</param>
        /// <param name="child">The exception raised when trying to handle the original exception.</param>
        /// <returns>An exception that contains both exceptions.</returns>
        public Exception ConcatExceptions(Exception source, Exception child)
        {
            return new AggregateException(Error.AggregateException, child, source);
        }

        public void Log(ILog log, Exception exception, string message = null)
        {
            if (exception.IsHttpNotFound())
            {
                log.Info(message ?? Error.WebResourceNotFound, exception);
            }
            else
            {
                log.Error(message ?? Error.UnhandledException, exception);
            }
        }
    }
}
