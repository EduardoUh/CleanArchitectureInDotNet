using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Domain;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;

namespace CleanArchitecture.Application.Features.Streamers.Commands.DeleteStreamer
{
    public class DeleteStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,ILogger<DeleteStreamerCommandHandler> logger) : IRequestHandler<DeleteStreamerCommand, Unit>
    {
        // private readonly IStreamerRepository _streamerRepository = streamerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DeleteStreamerCommandHandler> _logger = logger;

        public async Task<Unit> Handle(DeleteStreamerCommand request, CancellationToken cancellationToken)
        {
            // var streamerToDelete = await _streamerRepository.GetByIdAsync(request.Id);

            var streamerToDelete = await _unitOfWork.StreamerRepository.GetByIdAsync(request.Id);

            if(streamerToDelete == null)
            {
                _logger.LogError($"Streamer with the id {request.Id} not found");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            // await _streamerRepository.DeleteAsync(streamerToDelete);

            _unitOfWork.StreamerRepository.DeleteEntity(streamerToDelete);

            await _unitOfWork.Complete();

            _logger.LogInformation($"Streamer with the id {request.Id} removed successfully");

            return Unit.Value;
        }
    }
}
