using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.DeleteStreamer
{
    public class DeleteStreamerCommandValidator : AbstractValidator<DeleteStreamerCommand>
    {
        public DeleteStreamerCommandValidator()
        {
            RuleFor(streamer => streamer.Id)
                .NotNull().WithMessage("The id can't be null");
        }
    }
}
