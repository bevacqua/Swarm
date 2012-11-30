using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Swarm.Common.Interface;
using Swarm.Common.Mvc.IoC.Mvc;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Model.Binders
{
	[ModelType(typeof(FileUpload))]
	public class FileUploadModelBinder : IModelBinder
	{
		private readonly IMapper mapper;

		public FileUploadModelBinder(IMapper mapper)
		{
			if (mapper == null)
			{
				throw new ArgumentNullException("mapper");
			}
			this.mapper = mapper;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			HttpContextBase context = controllerContext.HttpContext;
			
			if (context.Request.ContentLength > 0)
			{
				var fileContents = new byte[context.Request.ContentLength];
				context.Request.InputStream.Read(fileContents, 0, context.Request.ContentLength);
				var filename = context.Request.Headers["X-File-Name"];

				var subPath = Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString("yyyyMMddHHmmsss") + Path.GetExtension(filename);
				
				return new FileUpload()
						   {
							   Code = Guid.NewGuid(),
							   Date = DateTime.Now,
							   Contents = fileContents,
							   Path = subPath
						   };
			}

			return new FileUpload();
		}
	}
}
