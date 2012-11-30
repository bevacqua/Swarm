using System.Web;
using Swarm.Overmind.Domain.Entity.ViewModels;

namespace Swarm.Overmind.Model.ViewModels
{
	public class ReportViewModel
	{
		public ReportModel Report { get; set; }
		public IHtmlString Json { get; set; }
	}
}