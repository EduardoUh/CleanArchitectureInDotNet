using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    // telling the validator that the validations will be performed over StreamerCommand class
    public class CreateStreamerCommandValidator : AbstractValidator<CreateStreamerCommand>
    {
        public CreateStreamerCommandValidator()
        {
            // by default the RuleFor lambda expression will be aplying the validation over the properties of the
            // given class in the type of the AbstractValidator class, above in the heritage statement
            RuleFor(streamer => streamer.Name)
                .NotEmpty().WithMessage("{Name} can't be empty")
                .NotNull().WithMessage("{Name} can't be null")
                .MaximumLength(50).WithMessage("{Name} can't exceed the 50 characters");

            RuleFor(streamer => streamer.Url)
                .NotEmpty().WithMessage("{Url} can't be empty")
                .NotNull().WithMessage("{Url} can´t be null");
        }
    }
}
