using System.Collections.Generic;
using System.Web.Mvc;

namespace Swarm.Common.Mvc.Core.ActionResults.Json
{
    /// <summary>
    /// Returns a JSON result that contains exception messages.
    /// </summary>
    public sealed class ExceptionJsonResult : JsonResult
    {
        /// <summary>
        /// Returns a JSON result that contains exception messages.
        /// </summary>
        public ExceptionJsonResult(IEnumerable<string> exceptions)
        {
            Data = new
            {
                Exceptions = exceptions
            };
        }
    }
}
