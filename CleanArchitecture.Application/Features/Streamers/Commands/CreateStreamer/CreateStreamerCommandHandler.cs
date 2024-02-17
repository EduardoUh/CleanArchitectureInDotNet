using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastucture;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    public class CreateStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, ILogger<CreateStreamerCommandHandler> logger) : IRequestHandler<CreateStreamerCommand, int>
    {
        // private readonly IStreamerRepository _streamerRepository = streamerRepository;
        // using unit of work instad
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<CreateStreamerCommandHandler> _logger = logger;

        public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
        {
            var streamerEntity = _mapper.Map<Streamer>(request);

            // var newStreamer = await _streamerRepository.AddAsync(streamerEntity);
            // it is recommended to use this method instad of the async methods, because the operation confirmation should
            // be done separetly
            _unitOfWork.StreamerRepository.AddEntity(streamerEntity);

            var result = await _unitOfWork.Complete();

            if(result <= 0)
            {
                throw new Exception("Streamer creation failed");
            }

            /*
            if (streamerEntity == null)
            {
                _logger.LogError($"Streamer creation task failed");
                return 0;
            }
            */

            _logger.LogInformation($"Streaming company {streamerEntity!.Name} created successfully");

            await SendEmail(streamerEntity);

            return streamerEntity!.Id;
        }

        public async Task SendEmail(Streamer streamer)
        {
            var email = new Email()
            {
                To = "eduardoivanuhgamino@gmail.com",
                Body = $"Streaming company {streamer.Name} created successfully",
                Subject = "Notification message"
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception)
            {
                _logger.LogError($"Couldn't send the email to {streamer.Name}");
            }
        }
    }
}
