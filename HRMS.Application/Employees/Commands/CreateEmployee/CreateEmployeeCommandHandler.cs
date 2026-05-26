using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;

namespace HRMS.Application.Employees.Commands.CreateEmployee;

/// <summary>
/// This handler executes when MediatR receives a CreateEmployeeCommand.
/// </summary>
public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, ApiResponse<Guid>>
{
    // A repository or DbContext would normally be injected here.
    // private readonly IApplicationDbContext _context;
    
    // public CreateEmployeeCommandHandler(IApplicationDbContext context) 
    // { 
    //     _context = context;
    // }

    public async Task<ApiResponse<Guid>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // 1. Map the command to our Domain Entity
        var newEmployee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Department = request.Department,
            Designation = request.Designation,
            JoiningDate = DateTime.UtcNow
        };

        // 2. Add to DbContext and Save (Mocked for now until Interfaces are set)
        // _context.Employees.Add(newEmployee);
        // await _context.SaveChangesAsync(cancellationToken);

        // 3. Return generic success response
        return ApiResponse<Guid>.SuccessResponse(newEmployee.Id, "Employee created successfully.");
    }
}
