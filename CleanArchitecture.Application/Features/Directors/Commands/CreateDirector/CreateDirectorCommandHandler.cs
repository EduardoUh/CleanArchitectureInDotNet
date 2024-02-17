using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Directors.Commands.CreateDirector
{
    public class CreateDirectorCommandHandler(ILogger<CreateDirectorCommandHandler> logger, IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<CreateDirectorCommand, int>
    {
        private readonly ILogger<CreateDirectorCommandHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<int> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
        {
            var directorEntity = _mapper.Map<Director>(request);
            // using unit of work to add the entity to memory (not db)
            _unitOfWork.Repository<Director>().AddEntity(directorEntity);
            // saving changes to db
            var result = await _unitOfWork.Complete();

            if(result <= 0)
            {
                _logger.LogError("Couldn't save the director to db");
                throw new Exception("Couldn't save the director to db");
            }

            return directorEntity.Id;
        }
    }
}
