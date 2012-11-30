using FluentValidation;

namespace Swarm.Common.Mvc.FluentValidation
{
    /// <summary>
    /// This view model validator is required for all those models where no validator is explicitly declared.
    /// </summary>
    public class NullModelValidator : AbstractValidator<dynamic>
    {
    }
}
