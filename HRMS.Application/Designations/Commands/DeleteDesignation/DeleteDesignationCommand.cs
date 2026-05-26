using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Designations.Commands.DeleteDesignation;

public record DeleteDesignationCommand(Guid Id) : IRequest<ApiResponse<Guid>>;
