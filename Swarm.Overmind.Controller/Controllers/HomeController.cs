using System.Web.Mvc;
using Swarm.Common.Mvc.Core.Controllers;

namespace Swarm.Overmind.Controller.Controllers
{
    public class HomeController : ExtendedController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
