using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUsername
{
    public class GetStreamerListQuery(string username) : IRequest<List<StreamersVm>>
    {
        public string? Username { get; set; } = username ?? throw new ArgumentNullException(nameof(username));
    }
}
