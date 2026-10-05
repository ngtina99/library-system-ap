# Library System API

Small ASP.NET Core Web API for managing books, users and book loans in a library system.

## Table of Contents

- [Tech Stack](#tech-stack)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [Architecture](#architecture)
- [Features](#features)
  - [Books](#books)
  - [Users](#users)
  - [Loans](#loans)
- [API Endpoints](#api-endpoints)
- [Business Rules](#business-rules)
- [Validation and HTTP Responses](#validation-and-http-responses)
- [Exception Handling](#exception-handling)
- [Key Decisions](#key-decisions)
  - [EF Core InMemory](#ef-core-inmemory)
  - [Service Layer](#service-layer)
  - [DTOs](#dtos)
  - [Async/Await](#asyncawait)
  - [Date and Time](#date-and-time)
- [Operational Considerations](#operational-considerations)

## Tech Stack

- .NET 8
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- EF Core InMemory
- OpenAPI / Swagger
- xUnit

## Running the Application

Requirements:

- .NET 8 SDK

Run the API:

```bash
dotnet run --project src/LibrarySystem.Api
```

Swagger UI is available in development mode at:

```text
/swagger
```

## Running Tests

Run all tests from the repository root:

```bash
dotnet test
```

The test suite covers the followings:

- borrowing an available book
- preventing borrowing of an unavailable book
- returning an active loan
- preventing a loan from being returned twice
- preventing deletion of a book that is currently on loan
- deleting a book that has no active loan
- rejecting a publication year in the future

## Architecture

The application uses a simple layered architecture:

```text
HTTP Request
     ↓
Controller
     ↓
Service
     ↓
EF Core DbContext
     ↓
InMemory Database
```

## API Endpoints

### Books

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/books` | Create a new book |
| `GET` | `/api/books` | List all books |
| `GET` | `/api/books?available=true` | Filter books by availability |
| `GET` | `/api/books?author=Martin` | Filter books by author |
| `GET` | `/api/books/{id}` | Get a book by ID |
| `PUT` | `/api/books/{id}` | Update a book |
| `DELETE` | `/api/books/{id}` | Delete a book if it is not on loan |

### Users

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/users` | Register a new user |
| `GET` | `/api/users` | List all users |

### Loans

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/loans` | Borrow a book |
| `POST` | `/api/loans/{id}/return` | Return a borrowed book |
| `GET` | `/api/loans/active` | List active loans |

## Business Rules

The API implements the following core rules:

- A book can only be borrowed if it is available.
- Borrowing a book marks it as unavailable.
- Returning a book marks it as available again.
- A loan cannot be returned more than once.
- A book cannot be deleted while it has an active loan.
- A loan can only be created for an existing user and book.
- A book's publication year cannot be in the future.

Business-rule conflicts are returned using appropriate HTTP status codes such as `409 Conflict`.

## Validation and HTTP Responses

Request DTOs use Data Annotations for input validation. With ASP.NET Core's `[ApiController]`, invalid requests automatically result in `400 Bad Request`.

The API uses appropriate HTTP status codes, including:

- `200 OK` for successful reads and updates
- `201 Created` when a resource is created
- `204 No Content` after successful deletion
- `400 Bad Request` for invalid input
- `404 Not Found` when a requested resource does not exist
- `409 Conflict` for business-rule conflicts
- `500 Internal Server Error` for unexpected failures

## Exception Handling

Unexpected exceptions are handled globally using ASP.NET Core's built-in exception-handling middleware together with `ProblemDetails`.

Expected business errors are handled explicitly by the application and mapped to appropriate HTTP responses rather than being treated as exceptions.

## Key Decisions

- EF Core InMemory is used because persistence is not required for the homework assignment.
- DTOs are used to avoid exposing internal entity models directly.
- Service classes keep business rules outside controllers.
- Conflict responses are returned for business-rule violations such as borrowing an unavailable book.
- `DateTimeOffset.UtcNow` is used for loan timestamps to avoid local time ambiguity.

Responsibilities are separated as follows:

- `Controllers` handle HTTP requests, responses, and status codes.
- `Services` contain application and business logic.
- `DTOs` define API request and response contracts.
- `Models` represent application entities.
- `Data` contains the EF Core `DbContext`.
- `Validation` contains custom validation attributes.
- `tests` contains automated tests for business and validation logic.

## Key Decisions

### EF Core InMemory

The assignment allows an in-memory database, so EF Core InMemory is used to keep the infrastructure simple and keep the focus on API design and business logic.

For a production application, a relational database such as SQL Server would normally be used together with EF Core migrations.

### Service Layer

Business logic is kept in service classes rather than controllers. This keeps controllers focused on HTTP concerns and makes the business rules easier to understand and test.

A separate repository layer was intentionally not added because EF Core's `DbContext` already provides a data-access abstraction and an additional repository layer would add unnecessary complexity for this small application.

### DTOs

Request and response DTOs are used instead of exposing EF Core entities directly. This keeps the external API contract separate from the internal data model.

### Async/Await

Database operations use asynchronous EF Core APIs such as `ToListAsync`, `FirstOrDefaultAsync`, and `SaveChangesAsync`.

Read-only queries use `AsNoTracking()` where appropriate.

### Date and Time

`DateTimeOffset.UtcNow` is used for registration and loan timestamps to avoid dependence on the server's local time zone.

## Operational Considerations

Although this is a small homework project, the following production-readiness aspects were considered:

- The API returns meaningful HTTP status codes.
- Business conflicts are represented with `409 Conflict`.
- Unexpected exceptions are handled using ASP.NET Core's built-in exception-handling middleware together with `ProblemDetails`, resulting in consistent `500 Internal Server Error` responses.
- Expected business errors are handled explicitly and mapped to appropriate HTTP status codes such as `404 Not Found` and `409 Conflict`.
- DTOs protect the API contract from internal entity changes.
- Service classes isolate business logic from HTTP concerns.
- Unit tests cover important loan and validation scenarios.
- The project can be extended with persistent storage, authentication, structured logging, and monitoring.