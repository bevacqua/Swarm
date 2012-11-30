using System;
using System.ComponentModel.DataAnnotations;
using Swarm.Contracts.Enum;

namespace Swarm.Overmind.Model.ViewModels
{
    public class ScenarioExecutionModel
    {
        public long Id { get; set; }
        public long ScenarioId { get; set; }

		[Display(Name="Scenario")]
		public ScenarioModel Scenario { get; set; }

        [Display(Name = "Started")]
        public DateTime Started { get; set; }

        [Display(Name = "Finished")]
        public DateTime? Finished { get; set; }

        [Display(Name = "Status")]
        public ExecutionStatus Status { get; set; }
    }
}
