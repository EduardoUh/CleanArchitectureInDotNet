using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Directors.Queries.Vms;
using CleanArchitecture.Application.Features.Shared.Queries;
using CleanArchitecture.Application.Specifications.Directors;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Directors.Queries.PaginationDirector
{
    public class PaginationDirectorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<PaginationDirectorsQuery, PaginationVm<DirectorVm>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginationVm<DirectorVm>> Handle(PaginationDirectorsQuery request, CancellationToken cancellationToken)
        {
            var directorSpecificationParams = new DirectorSpecificationParams
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new DirectorSpecification(directorSpecificationParams);
            var directors = await _unitOfWork.Repository<Director>().GetAllWithSpec(spec);

            var specCount = new DirectorForCountingSpecification(directorSpecificationParams);
            var totalDirectors = await _unitOfWork.Repository<Director>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalDirectors) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Director>, IReadOnlyList<DirectorVm>>(directors);

            var pagination = new PaginationVm<DirectorVm>
            {
                Count = totalDirectors,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }

    }
}
