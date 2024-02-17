using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using CleanArchitecture.Application.Specifications.Streamers;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUrl
{
    public class GetStreamerListByUrlQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetStreamerListByUrlQuery, List<StreamersVm>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<StreamersVm>> Handle(GetStreamerListByUrlQuery request, CancellationToken cancellationToken)
        {
            // we gonna implement the specification design pattern, so the logic should be there and not here
            var spec = new StreamersWithVideosSpecification(request.Url!);
            var streamerList = await _unitOfWork.Repository<Streamer>().GetAllWithSpec(spec);

            return _mapper.Map<List<StreamersVm>>(streamerList);
        }

    }
}
