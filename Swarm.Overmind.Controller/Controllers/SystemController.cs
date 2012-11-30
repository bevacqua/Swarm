using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Swarm.Common.Configuration;
using Swarm.Common.Mvc.Core.Attributes;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Service;
using Swarm.Overmind.Model.ViewModels;

namespace Swarm.Overmind.Controller.Controllers
{
    public class SystemController : ExtendedController
    {
        private readonly ILogService logService;

        public SystemController(ILogService logService)
        {
            if (logService == null)
            {
                throw new ArgumentNullException("logService");
            }
            this.logService = logService;
        }

        [HttpGet]
        [NotAjax]
        public ActionResult Log()
        {
            IList<Log> logs = logService.GetLast(10);
            IList<LogModel> model = mapper.Map<IList<Log>, IList<LogModel>>(logs);
            return View(model);
        }

        [HttpGet]
        [NotAjax]
        public ActionResult Environment()
        {
            IList<KeyValuePair<string, string>> model = Config.AsKeyValuePairs();
            return View(model);
        }
    }
}
