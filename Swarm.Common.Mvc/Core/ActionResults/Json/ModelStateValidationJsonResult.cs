using System.Linq;
using System.Web.Mvc;

namespace Swarm.Common.Mvc.Core.ActionResults.Json
{
    /// <summary>
    /// Returns a JSON result that contains model errors.
    /// </summary>
    public sealed class ModelStateValidationJsonResult : JsonResult
    {
        /// <summary>
        /// Returns a JSON result that contains model errors.
        /// </summary>
        public ModelStateValidationJsonResult(ModelStateDictionary state)
        {
            Data = new
            {
                Errors = state.Select(model => new
                {
                    model.Key,
                    Messages = model.Value.Errors.Select(e => e.ErrorMessage)
                })
            };
        }
    }
}
