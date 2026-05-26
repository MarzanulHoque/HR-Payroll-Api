using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Designations.Queries.GetDesignationById;

public record GetDesignationByIdQuery(Guid Id) : IRequest<ApiResponse<DesignationDto>>;
