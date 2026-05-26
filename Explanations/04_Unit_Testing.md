# How We Test Our Code (Unit Testing)

To ensure our application works reliably, we write **Unit Tests**. 

## What is a Unit Test?
A unit test is a small piece of code that tests a specific part of your application in isolation. Instead of running the whole application and clicking buttons, a unit test directly asks your code to run a function and checks if the output is correct.

## Tools We Use:
1. **xUnit**: The testing framework. It finds our tests and runs them. We use the `[Fact]` attribute to tell xUnit "this is a test".
2. **Moq**: A library that lets us "fake" things. Because we only want to test our *business logic* (and not the actual database connection), we use Moq to create a fake database.

## How a test is written (The 3 A's):
- **Arrange**: Set everything up. We create our "fake" database and prepare the data (like creating a `CreateDepartmentCommand` with test data).
- **Act**: Execute the action. We pass the command into the Handler, just like the actual system would.
- **Assert**: Verify the result. We verify that the returned result says `Success = true`, the record was given an `Id`, and that the handler asked the (fake) database to save the changes exactly once.

## Why is this helpful?
Whenever we make future changes to the system, we can just hit "Run Tests" in our terminal (`dotnet test`). If any tests fail, we instantly know that a change accidentally broke existing logic BEFORE we deploy it to production!
