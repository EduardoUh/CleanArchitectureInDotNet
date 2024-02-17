using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer
{
    public class UpdateStreamerCommandValidator : AbstractValidator<UpdateStreamerCommand>
    {
        public UpdateStreamerCommandValidator()
        {
            RuleFor(streamer => streamer.Name)
                .NotEmpty().WithMessage("Field name can not be empty")
                .MaximumLength(50).WithMessage("{Name} can't exceed the 50 characters");
            RuleFor(streamer => streamer.Url)
                .NotEmpty().WithMessage("Field url can not be empty");
        }
    }
}
