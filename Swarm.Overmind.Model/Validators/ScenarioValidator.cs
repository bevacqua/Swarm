using System;
using FluentValidation;
using Swarm.Overmind.Model.ViewModels;

namespace Swarm.Overmind.Model.Validators
{
	public class ScenarioValidator : AbstractValidator<ScenarioModel>
	{
		public ScenarioValidator()
		{
			RuleFor(m => m.VirtualUsers)
				.GreaterThan(0).WithMessage("# of Virtual users should be greater than zero.");

			RuleFor(m => m.SleepTime)
				.NotEmpty().WithMessage("Sleep time is required.");

			RuleFor(m => m.SleepTime)
			    .LessThan(TimeSpan.FromDays(1)).WithMessage("Sleep time should not exceed one day.");

			RuleFor(m => m.RampUpTime)
				.NotEmpty().WithMessage("Ramp up time is required.");

			RuleFor(m => m.RampUpTime)
				.LessThan(TimeSpan.FromDays(1)).WithMessage("Ramp up time should not exceed one day.");

			RuleFor(m => m.SamplingInterval)
			    .NotEmpty().WithMessage("Sampling Interval is required.");

			RuleFor(m => m.SamplingInterval)
				.LessThan(TimeSpan.FromDays(1)).WithMessage("Sampling Interval should not exceed one day.");

			RuleFor(m => m.RequestTimeout)
				.NotEmpty().WithMessage("Request timeout is required.");

			RuleFor(m => m.RequestTimeout)
				.LessThan(TimeSpan.FromDays(1)).WithMessage("Request timeout should not exceed one day.");

			RuleFor(m => m.LogLevel)
				.NotEmpty().WithMessage("Log level is required.");

			RuleFor(m => m.Endpoint)
				.NotEmpty().WithMessage("Endpoint is required.");
		}
	}
}
