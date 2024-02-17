using FluentValidation;

namespace CleanArchitecture.Application.Features.Directors.Commands.CreateDirector
{
    public class CreateDirectorCommandValidator : AbstractValidator<CreateDirectorCommand>
    {
        public CreateDirectorCommandValidator()
        { 
            RuleFor(director => director.Name)
                .NotEmpty().WithMessage("{Name} is required");
            RuleFor(director => director.LastName)
                .NotEmpty().WithMessage("{Last name} is required");
            RuleFor(director => director.VideoId)
                .NotEmpty().WithMessage("{Video id} is required");
        }
    }
}
