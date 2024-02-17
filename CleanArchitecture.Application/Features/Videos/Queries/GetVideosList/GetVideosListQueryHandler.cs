using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList
{
    // Here in the handler we are stating where the request comes from and what will be returned
    public class GetVideosListQueryHandler : IRequestHandler<GetVideosListQuery, List<VideosVm>>
    {
        // the logic to retrieve the videos list is in the IVideoRepository interface
        //private readonly IVideoRepository _videoRepository;

        private readonly IUnitOfWork _unitOfWork;
        // since the IVideoRepository returns a list of Videos and not VideosVm
        // we need to perfom a mapping operation using IMapper
        private readonly IMapper _mapper;
        // also we need to inject those dependencies

        public GetVideosListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            // this._videoRepository = videoRepository;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<List<VideosVm>> Handle(GetVideosListQuery request, CancellationToken cancellationToken)
        {
            // Here goes the logic, remember the logic is in the IVideoRepository
            // request is a reference of the GetVideosListQuery so we can access its properties
            // var videoList = await this._videoRepository.GetVideoByUserName(request.UserName);

            var videoList = await _unitOfWork.VideoRepository.GetVideoByUserName(request.UserName);

            // now we can perform the mapping operation in order to convert the List<Videos> to a List<VideosVm>
            return this._mapper.Map<List<VideosVm>>(videoList);
        }
    }
}
