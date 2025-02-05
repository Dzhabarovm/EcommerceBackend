using Domain.Enums;
using MediatR;

namespace Application.Sellers.Queries
{
    public record GetApplicationsQuery(ApplicationStatus? Status) : IRequest<List<ApplicationDto>>;
}
