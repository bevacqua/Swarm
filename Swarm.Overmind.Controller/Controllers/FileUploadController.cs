using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Service;
using Swarm.Overmind.Model.ViewModels;
using log4net;

namespace Swarm.Overmind.Controller.Controllers
{
	public class FileUploadController : ExtendedController
	{
		private readonly IFileUploadService fileUploadService;

		public FileUploadController(IFileUploadService fileUploadService)
		{
			this.fileUploadService = fileUploadService;
		}

		public ActionResult Add()
		{
			var listFiles = fileUploadService.GetAllFiles();
			IList<FileUploadModel> model = listFiles.Select(item => new FileUploadModel()
			{
				Filename = Path.GetFileName(item.Path),
				Code = item.Code
			}).ToList();

			return View(model);
		}
		
		public ActionResult RemoveFile(string code)
		{
			fileUploadService.DeleteByCode(Guid.Parse(code));
			return RedirectToAction("Add");
		}

		[HttpPost]
		public ActionResult UploadFile(FileUpload fileUpload)
		{
			fileUploadService.UploadFile(fileUpload);
			return RedirectToAction("Add");
		}
		
	}
}



