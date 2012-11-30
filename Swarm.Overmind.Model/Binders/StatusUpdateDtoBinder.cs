using Swarm.Common.Mvc.IoC.Mvc;
using Swarm.Contracts.DTO;

namespace Swarm.Overmind.Model.Binders
{
	[ModelType(typeof(StatusUpdateDto))]
	public class StatusUpdateDtoBinder : JsonStreamBinder<StatusUpdateDto>
	{
	}
}