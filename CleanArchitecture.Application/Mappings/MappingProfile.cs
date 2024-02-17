using AutoMapper;
using CleanArchitecture.Application.Features.Directors.Commands.CreateDirector;
using CleanArchitecture.Application.Features.Directors.Queries.Vms;
using CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer;
using CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer;
using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // here we are setting up the config for the mapping operation used in GetVideosListQueryHandler
            CreateMap<Video, VideosVm>();
            // here we are setting up the config for the mapping operation to map a StreamerCommand object to a Streamer object
            CreateMap<CreateStreamerCommand, Streamer>();

            CreateMap<UpdateStreamerCommand, Streamer>();
            CreateMap<CreateDirectorCommand, Director>();
            CreateMap<Streamer, StreamersVm>();
            CreateMap<Director, DirectorVm>();
        }
    }
}
