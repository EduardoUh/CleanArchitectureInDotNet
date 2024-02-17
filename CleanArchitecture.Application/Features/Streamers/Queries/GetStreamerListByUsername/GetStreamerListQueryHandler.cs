using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using CleanArchitecture.Domain;
using MediatR;
using System.Linq.Expressions;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUsername
{
    public class GetStreamerListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetStreamerListQuery, List<StreamersVm>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<StreamersVm>> Handle(GetStreamerListQuery request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Streamer, object>>>
            {
                s => s.Videos!
            };

            var streamerList = await _unitOfWork.Repository<Streamer>().GetAsync(
                s => s.CreatedBy == request.Username,// predicate
                s => s.OrderBy(s => s.CreationDate),
                includes,
                true
                );

            // now we gonna map the results to the StreamersVm dot (data transfer object)
            return _mapper.Map<List<StreamersVm>>(streamerList);
        }
    }
}
