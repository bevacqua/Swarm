using RestSharp;

namespace Swarm.Drone.Domain.Logic.Models
{
	public class Header
	{
		public string Name { get; set; }
		public ParameterType Type { get; set; }
	}
}