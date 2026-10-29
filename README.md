# PersonalLibraryAPI

A RESTful API built with ASP.NET Core for managing a personal library of books and categories.

The project was developed as a practical exercise in backend development with C# and .NET, focusing on REST APIs, relational data, Entity Framework Core, authentication, authorization, filtering, sorting, and pagination.

## Features

- Create, read, update, and delete books
- Create, read, update, and delete categories
- One-to-many relationship between categories and books
- Search and filter books
- Sort query results
- Pagination
- DTOs for API requests and responses
- Data validation
- Asynchronous database operations
- JWT authentication
- Authorization policies
- Custom middleware
- Entity Framework Core migrations

## Tech Stack

- C#
- .NET / ASP.NET Core Web API
- Entity Framework Core
- LINQ
- SQLite
- JWT Authentication
- Dependency Injection
- REST

## Data Model

The application contains two main entities:

### Category

Represents a book category.

A category can contain multiple books.

### Book

Represents a book stored in the personal library.

Each book belongs to a category.

The relationship can be represented as:

```text
Category
   │
   └── 1 : N
          │
         Book
```

## Querying

The API supports querying book data using filters, sorting, and pagination.

These features were implemented using LINQ and Entity Framework Core queries.

Examples of supported operations include:

- Filtering books by available criteria
- Searching for books
- Sorting results
- Returning paginated results

## Authentication and Authorization

The API uses JWT-based authentication.

Protected operations require a valid JWT token, and authorization policies are used to restrict access where appropriate.

## Database

The project uses SQLite with Entity Framework Core.

The database structure is managed through EF Core migrations.

To apply the migrations:

```bash
dotnet ef database update
```

## Running the Project

Clone the repository:

```bash
git clone https://github.com/lirio22/PersonalLibraryAPI.git
```

Enter the project directory:

```bash
cd PersonalLibraryAPI
```

Restore the dependencies:

```bash
dotnet restore
```

Apply the database migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

## What I Practiced

This project was created to practice backend development concepts including:

- REST API design
- ASP.NET Core controllers
- Services and dependency injection
- DTO design
- Input validation
- Relational database modeling
- Entity Framework Core
- LINQ queries
- Database migrations
- Asynchronous programming
- Filtering, sorting, and pagination
- Authentication and authorization
- Middleware

## Project Status

Completed as a backend development portfolio and study project.

## API Endpoints

### Books

| Method | Endpoint | Description | Authorization |
|---|---|---|---|
| `GET` | `/api/books` | Returns books with optional filtering, sorting, and pagination | Public |
| `GET` | `/api/books/{id}` | Returns a book by ID | Public |
| `POST` | `/api/books` | Creates a new book | Authenticated user |
| `PUT` | `/api/books/{id}` | Updates an existing book | Authenticated user |
| `DELETE` | `/api/books/{id}` | Deletes a book | Admin only |

#### Book Query Parameters

`GET /api/books` supports the following optional query parameters:

| Parameter | Type | Description |
|---|---|---|
| `isRead` | `bool` | Filters books by read status |
| `categoryId` | `int` | Filters books by category |
| `title` | `string` | Searches books by title |
| `sortBy` | `string` | Sorts by `title`, `author`, or `id` |
| `descending` | `bool` | Uses descending sort order when `true` |
| `page` | `int` | Page number. Default: `1` |
| `pageSize` | `int` | Number of results per page. Default: `10` |

Example:

```http
GET /api/books?isRead=true&categoryId=2&sortBy=title&descending=false&page=1&pageSize=10
```

### Categories

| Method | Endpoint | Description | Authorization |
|---|---|---|---|
| `GET` | `/api/categories` | Returns all categories | Public |
| `GET` | `/api/categories/{id}` | Returns a category by ID | Public |
| `POST` | `/api/categories` | Creates a new category | Admin only |
| `PUT` | `/api/categories/{id}` | Updates an existing category | Admin only |
| `DELETE` | `/api/categories/{id}` | Deletes a category | Admin only |

A category cannot be deleted while it still contains books.

### Common Responses

- `200 OK` — Request completed successfully
- `201 Created` — Resource created successfully
- `204 No Content` — Resource deleted successfully
- `400 Bad Request` — Invalid request or business-rule violation
- `401 Unauthorized` — Authentication is required
- `403 Forbidden` — The authenticated user does not have permission
- `404 Not Found` — Requested book, category, or related resource was not found
