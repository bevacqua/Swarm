using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Swarm.Common.Extensions;

namespace Swarm.Overmind.Model.Binders
{
	public abstract class JsonStreamBinder<TModel> : IModelBinder
	{
		protected TModel ParseModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			HttpContextBase context = controllerContext.HttpContext;
			Stream stream = context.Request.InputStream;
			stream.Position = 0;
			string json = stream.ReadFully();

			TModel model = JsonConvert.DeserializeObject<TModel>(json);
			return model;
		}

		public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			TModel model = ParseModel(controllerContext, bindingContext);
			return model;
		}
	}
}