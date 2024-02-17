using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList
{
    // in order to make possible the communication between this query and the query handler we need to implement the IRequest
    // from MediatR
    // VideosVm -> Videos view model -> List<VideosVm> returned type
    public class GetVideosListQuery : IRequest<List<VideosVm>>
    {
        public string UserName { get; set; } = string.Empty;

        public GetVideosListQuery(string userName)
        {
            this.UserName = userName ?? throw new ArgumentException(nameof(userName));
        }
    }
}
