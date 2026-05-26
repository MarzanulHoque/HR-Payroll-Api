using HRMS.Application.Common.Models;
using HRMS.Application.Designations.Queries.GetDesignationById;
using MediatR;

namespace HRMS.Application.Designations.Queries.GetAllDesignations;

public record GetAllDesignationsQuery : IRequest<ApiResponse<List<DesignationDto>>>;
