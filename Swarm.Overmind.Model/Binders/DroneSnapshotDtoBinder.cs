using Swarm.Common.Mvc.IoC.Mvc;
using Swarm.Contracts.DTO;

namespace Swarm.Overmind.Model.Binders
{
	[ModelType(typeof(DroneSnapshotDto))]
	public class DroneSnapshotDtoBinder : JsonStreamBinder<DroneSnapshotDto>
	{
	}
}