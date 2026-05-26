# CQRS & REST API Flow (How an API call works)

Let's trace how saving a new Employee works from start to finish:

1. **The Request**: 
   - Angular or React sends a `POST` request to `http://localhost:5005/api/v1/employees` containing JSON data `{ "firstName": "John", "lastName": "Doe" ... }`.

2. **The Controller** (`EmployeesController.cs`):
   - Receives the request in `CreateEmployee(CreateEmployeeCommand command)`.
   - Sends the command into the MediatR pipeline: `await _mediator.Send(command);`

3. **The Handler** (`CreateEmployeeCommandHandler.cs`):
   - MediatR automatically finds this file because it handles `CreateEmployeeCommand`.
   - It takes the incoming data, creates a new `Employee` object (from Domain layer), and tells the database to save it using `_context.Employees.Add()` and `await _context.SaveChangesAsync()`.
   - Returns our standardized `ApiResponse<Guid>` back to the controller.

4. **The Response**:
   - The Controller receives the result. If successful, it returns HTTP 200 OK along with our standard JSON format containing the new Employee's ID.

## Code Examples

Here is how the above flow looks in code:

**1. The Command (Data Container)**
```csharp
public record CreateDepartmentCommand(
    string Name,
    string? Description,
    Guid? ParentDepartmentId,
    Guid? ManagerId
) : IRequest<ApiResponse<Guid>>;
```

**2. The Handler (Business Logic)**
```csharp
public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;
    
    public CreateDepartmentCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<ApiResponse<Guid>> Handle(CreateDepartmentCommand request, CancellationToken ct)
    {
        var department = new Department { Name = request.Name, Description = request.Description };
        _context.Departments.Add(department);
        await _context.SaveChangesAsync(ct);
        
        return ApiResponse<Guid>.SuccessResponse(department.Id, "Created");
    }
}
```

**3. The Controller (API Entry Point)**
```csharp
[HttpPost]
public async Task<ActionResult<ApiResponse<Guid>>> CreateDepartment(CreateDepartmentCommand command)
{
    var result = await _mediator.Send(command); // Magic happens here
    return Ok(result);
}
```
