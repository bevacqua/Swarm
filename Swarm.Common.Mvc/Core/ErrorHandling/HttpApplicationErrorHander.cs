using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Swarm.Common.Extensions;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Common.Mvc.Core.Models;
using Swarm.Common.Mvc.Extensions;
using Swarm.Common.Mvc.Utility;
using Swarm.Common.Resources;
using log4net;

namespace Swarm.Common.Mvc.Core.ErrorHandling
{
    public class HttpApplicationErrorHander
    {
        private readonly ILog log = LogManager.GetLogger(typeof (HttpApplicationErrorHander));
        private readonly HttpApplication application;
        private readonly ExceptionHelper helper;

        public HttpApplicationErrorHander(HttpApplication application, ExceptionHelper helper)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }
            if (helper == null)
            {
                throw new ArgumentNullException("helper");
            }
            this.application = application;
            this.helper = helper;
        }

        public void HandleApplicationError()
        {
            HttpContextBase context = HttpContext.Current.Wrap();
            Exception exception = application.Server.GetLastError();

            helper.Log(log, exception);

            if (context == null)
            {
                return;
            }

            using (ErrorController controller = ErrorController.Instance(context))
            {
                if (exception == null) // prevent bizarre scenario when handling requests to *.cshtml physical files.
                {
                    return;
                }
                HttpResponseBase response = context.Response;
                response.Clear();
                response.TrySkipIisCustomErrors = true;

                if (exception.IsHttpNotFound())
                {
                    response.Status = Constants.HttpNotFound;
                }
                else
                {
                    response.Status = Constants.HttpServerError;
                }

                try
                {
                    WriteViewResponse(exception, controller);
                }
                catch (Exception exceptionRenderingViewResult) // now we're in trouble. lets be as graceful as possible.
                {
                    Exception concatenated = helper.ConcatExceptions(exception, exceptionRenderingViewResult);
                    WriteGracefulResponse(concatenated, controller);
                }
                finally
                {
                    application.Server.ClearError();
                }
            }
        }

        private bool WriteJsonResponse(HttpRequestBase request, HttpResponseBase response, string json)
        {
            if (request.IsAjaxRequest())
            {
                response.Status = Constants.HttpSuccess;
                response.ContentType = Constants.JsonContentType;
                response.Write(json);
                return true;
            }
            return false;
        }

        private void WriteViewResponse(Exception exception, StringRenderingController controller)
        {
            if (!WriteJsonResponse(controller.Request, controller.Response, User.UnhandledExceptionJson))
            {
                HttpResponseBase response = controller.Response;
                ErrorViewModel model = helper.GetErrorViewModel(controller.RouteData, exception);
                string result = controller.ViewString(Constants.ErrorViewName, model);
                response.ContentType = Constants.HtmlContentType;
                response.Write(result);
            }
        }

        private void WriteGracefulResponse(Exception exception, ErrorController controller)
        {
            try
            {
                // write an HTML response from an embedded resource in the assembly.
                WriteHtmlResponse(exception, controller);
            }
            catch (Exception exceptionRenderingHtml) // we seem to be having a very rough day, lets just call it a day.
            {
                Exception concatenated = helper.ConcatExceptions(exception, exceptionRenderingHtml);
                WritePlainTextResponse(controller.Response, concatenated); // write a plain text response.
            }
        }

        private void WriteHtmlResponse(Exception exceptionWritingView, Controller controller)
        {
            log.Fatal(Error.FatalException, exceptionWritingView);

            HttpResponseBase response = controller.Response;
            ErrorViewModel model = helper.GetErrorViewModel(controller.RouteData, exceptionWritingView);
            string html = GetHtmlResponse(model);

            response.Clear();
            response.ContentType = Constants.HtmlContentType;
            response.Write(html);
        }

        private void WritePlainTextResponse(HttpResponseBase response, Exception exceptionWritingHtml)
        {
            log.Fatal(Error.FatalException, exceptionWritingHtml);

            response.Clear();
            response.ContentType = Constants.PlainTextContentType;
            response.Write(User.FatalException);
        }

        private string GetHtmlResponse(ErrorViewModel model)
        {
            string html = GetEmbeddedHtmlTemplate(Constants.UnrecoverableViewName);
            string htmlForException = model.DisplayException ? GetHtmlException(model.Exception) : string.Empty;
            html = html.Replace(Unrecoverable.ModelTitle, HttpUtility.HtmlEncode(User.FatalException));
            html = html.Replace(Unrecoverable.ModelRefresh, HttpUtility.HtmlEncode(User.Refresh));
            html = html.Replace(Unrecoverable.ModelMessage, HttpUtility.HtmlEncode(model.Message));
            html = html.Replace(Unrecoverable.ModelException, htmlForException);
            return html;
        }

        public string GetHtmlException(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }
            object sqlData = exception.Data["SQL"];
            string sqlHtml = string.Empty;

            if (sqlData != null)
            {
                string sql = sqlData.ToString();
                sqlHtml = Unrecoverable.Sql.FormatWith(HttpUtility.HtmlEncode(sql));
            }
            StringBuilder stackTrace = new StringBuilder();
            string stackTraceHtml = string.Empty;

            if (exception.StackTrace != null)
            {
                string[] lines = exception.StackTrace.SplitOnNewLines();
                foreach (string line in lines)
                {
                    stackTrace.AppendFormat(Unrecoverable.StackTraceLine, HttpUtility.HtmlEncode(line.Trim()));
                }
                stackTraceHtml = Unrecoverable.StackTrace.FormatWith(stackTrace);
            }

            #region Inner Exception recursion

            AggregateException aggregate = exception as AggregateException;
            IEnumerable<Exception> innerExceptions = new[] {exception.InnerException};
            if (aggregate != null)
            {
                innerExceptions = aggregate.InnerExceptions;
            }
            string innerHtml = string.Empty;
            foreach (Exception inner in innerExceptions)
            {
                innerHtml += GetHtmlException(inner);
            }
            if (!innerHtml.NullOrBlank())
            {
                innerHtml = Unrecoverable.InnerException.FormatWith(innerHtml);
            }

            #endregion

            string html = GetEmbeddedHtmlTemplate(Constants.UnrecoverableExceptionViewName);
            html = html.Replace(Unrecoverable.ModelMessage, HttpUtility.HtmlEncode(exception.Message));
            html = html.Replace(Unrecoverable.ModelSql, sqlHtml);
            html = html.Replace(Unrecoverable.ModelStackTrace, stackTraceHtml);
            html = html.Replace(Unrecoverable.ModelInnerException, innerHtml);
            return html;
        }

        private string GetEmbeddedHtmlTemplate(string viewName)
        {
            Type type = typeof (HttpApplicationErrorHander);
            Assembly assembly = type.Assembly;
            Stream stream = assembly.GetManifestResourceStream(type, viewName);
            string html = stream.ReadFully(); // we don't use RazorEngine here, to avoid any complications in case what's faulty is RazorEngine itself.
            return html;
        }
    }
}
