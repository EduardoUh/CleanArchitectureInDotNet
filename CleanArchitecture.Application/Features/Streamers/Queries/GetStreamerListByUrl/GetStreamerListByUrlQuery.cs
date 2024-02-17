using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUrl
{
    public class GetStreamerListByUrlQuery(string url) : IRequest<List<StreamersVm>>
    {
        public string? Url { get; set; } = url ?? throw new ArgumentNullException(nameof(url));
    }
}
