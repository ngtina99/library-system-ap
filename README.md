

# Library System API

Small ASP.NET Core Web API for managing books, users, and loans in a simple library system.

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core InMemory
- OpenAPI / Swagger
- xUnit unit tests
- Dependency Injection
- DTO-based API contracts

## Features

### Books

- Create book
- Update book
- Delete book if not currently on loan
- List books
- Filter books by availability and author

### Users

- Register user
- List users

### Loans

- Borrow a book
- Return a book
- List active loans

## Running the Application
bash dotnet restore dotnet run --project src/LibrarySystem.Api


Swagger UI is available in development mode.

## Running Tests
bash dotnet test


## Architecture

The project uses a simple layered structure:

- `Controllers` handle HTTP requests and responses
- `Services` contain business logic
- `DTOs` define API request and response contracts
- `Models` represent persistence entities
- `Data` contains the EF Core `DbContext`
- `Validation` contains custom validation attributes
- `tests` contains unit tests for service and validation logic

## Key Decisions

- EF Core InMemory is used because persistence is not required for the homework assignment.
- DTOs are used to avoid exposing internal entity models directly.
- Service classes keep business rules outside controllers.
- Conflict responses are returned for business-rule violations such as borrowing an unavailable book.
- `DateTimeOffset.UtcNow` is used for loan timestamps to avoid local time ambiguity.

## Possible Improvements

- Replace InMemory database with SQL Server or PostgreSQL.
- Add authentication and authorization.
- Add global exception handling middleware with structured logging.
- Add integration tests for API endpoints.
- Add CI pipeline for build and test validation.

## Operational Considerations

Although this is a small homework project, the following production-readiness aspects were considered:

- The API returns meaningful HTTP status codes.
- Business conflicts are represented with `409 Conflict`.
- Unexpected exceptions are handled through ASP.NET Core problem details.
- DTOs protect the API contract from internal entity changes.
- Service classes isolate business logic from HTTP concerns.
- Unit tests cover important loan and validation scenarios.
- The project can be extended with persistent storage, authentication, structured logging, and monitoring.