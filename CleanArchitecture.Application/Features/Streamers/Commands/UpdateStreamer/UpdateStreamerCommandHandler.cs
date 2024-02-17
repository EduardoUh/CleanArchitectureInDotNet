using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer
{
    public class UpdateStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateStreamerCommandHandler> logger) : IRequestHandler<UpdateStreamerCommand, Unit>
    {
        //private readonly IStreamerRepository _streamerRepository = streamerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateStreamerCommandHandler> _logger = logger;

        public async Task<Unit> Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
        {
            // var streamerToUpdate = await _streamerRepository.GetByIdAsync(request.Id);

            var streamerToUpdate = await _unitOfWork.StreamerRepository.GetByIdAsync(request.Id);

            if (streamerToUpdate == null)
            {
                _logger.LogError($"Streamer with id {request.Id} not found");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            // request -> streamerToUpdate - id, name, url
            // source of the data, destination of the data, type of the source, type of the destination

            Console.WriteLine(streamerToUpdate.Url);

            _mapper.Map(request, streamerToUpdate, typeof(UpdateStreamerCommand), typeof(Streamer));

            // await _streamerRepository.UpdateAsync(streamerToUpdate);

            _unitOfWork.StreamerRepository.UpdateEntity(streamerToUpdate);

            await _unitOfWork.Complete();

            _logger.LogInformation($"Streamer updated succesfully");

            return Unit.Value;
        }
    }
}
