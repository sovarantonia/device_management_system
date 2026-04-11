# Device management system

A full-stack application for managing company devices and their assignments to users.

The repository contains:
- `backend/` - ASP.NET Core Web API
- `frontend/` - Angular application
- `backendTests/`- Integration tests for backend application
- schema.sql - SQL file containing the schema structure (database creations for application and integration tests)
- table_data.sql - SQL file containing the data for running the application

## Features

- User authentication
- Device creation, update, deletion, and viewing
- Device assignment and unassignment
- Device description generation
- Responsive frontend with form validation
- Device search
- SQL scripts for database creation and data initialization

## Project structure

```
root/
├── backend/
├── frontend/
├── backendTests/
├── schema.sql 
├── table_data.sql  
└── README.md
```

## Prerequisites

Before running the project, make sure you have installed:

- .NET SDK 8.0

- Node.js and npm

- Angular CLI

- SQL Server or SQL Server Express

## Database setup

Before running the application, you must create and populate the database.

- Run SQL scripts

- Configure connection string

Update the connection string in the backend configuration (`appsettings.json`):

```
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=device_management;Trusted_Connection=True;TrustServerCertificate=True"
}
```

## AI description generator setup

The application includes a feature that generates device descriptions using an AI service.

Important

For security reasons, the API key is not included in this repository.

- Configure API key

In `appsettings.json`, the key is intentionally empty:

```
"AIApi": {
  "ApiKey": ""
}
```
Set your API key locally using an environment variable.

For generating an API key (free):
- Go to ```https://openrouter.ai/nvidia/nemotron-3-super-120b-a12b:free/api```
- Create API key (registration is required)
- Go to settings
- Generate key

Set it locally as mentioned above.

If no API key is configured, a fallback description is returned instead.

## Running the backend

- Navigate to the backend folder:

   - cd backend

- Run:

  - dotnet restore
  - dotnet build
  - dotnet run

- The backend will start on a local URL:

  - ```https://localhost:7064```

## Running the frontend

 - Navigate to the frontend folder:

    - cd frontend

 - Install dependencies:

   - npm install

 - Start the app:

   - ng serve

- Open in browser:

  - ```http://localhost:4200```
 
## Running the tests
- Navigate to the backendTests folder:

   - cd backendTests

- In ```CustomWebApplicationFactory.cs``` change the connection string similarly to application connection string
    - ```"Server=YOUR_SERVER;Database=device_management_test;Trusted_Connection=True;TrustServerCertificate=True"```

- Run:

  - dotnet test
