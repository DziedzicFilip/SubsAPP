# SubsAPP

SubsAPP is a workforce management application designed for handling shift substitutions, schedules, and employee groups. It consists of a RESTful backend built with ASP.NET Core and a web frontend built with React and TypeScript.

---

## Project Structure

```
SubsAPP/
├── Backend/
│   ├── Backend.API/        # ASP.NET Core Web API
│   └── Backend.Tests/      # Unit tests
├── FrontendWeb/            # React + TypeScript (Vite)
└── MobileApp/              # Mobile application (planned)
```

---

## Technology Stack

| Layer          | Technology                                           |
| -------------- | ---------------------------------------------------- |
| Backend        | ASP.NET Core 8, Entity Framework Core, MS SQL Server |
| Authentication | ASP.NET Identity, JWT Bearer                         |
| Frontend       | React 19, TypeScript 5, Vite 7                       |
| API Docs       | Swagger / OpenAPI (Swashbuckle)                      |
| Tests          | xUnit                                                |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- MS SQL Server instance

### Backend

1. Fill in the `Backend/Backend.API/.env` file:

   ```env
   DB_CONNECTION_STRING=Server=localhost;Database=SubsAPP;Trusted_Connection=True;
   JWT_SECRET=your_super_secret_key_here
   ```

2. Apply database migrations:

   ```bash
   cd Backend/Backend.API
   dotnet ef database update
   ```

3. Run the API:

   ```bash
   dotnet run --project Backend/Backend.API
   ```

   The API will be available at `https://localhost:5000`.

### Frontend

1. Fill in the `FrontendWeb/.env` file:

   ```env
   VITE_API_URL=https://localhost:5001
   ```

2. Install dependencies and start the dev server:

   ```bash
   cd FrontendWeb
   npm install
   npm run dev
   ```

   The app will be available at `http://localhost:5173`.

---

## Authentication

The API uses JWT Bearer tokens. To access protected endpoints:

1. Call `POST /api/Auth/login` with valid credentials.
2. Copy the `token` field from the response.
3. Include it in subsequent requests:
   ```
   Authorization: Bearer <your_token>
   ```

Tokens expire after **1 hour**.

---

## License

This project is private and not licensed for public distributio for now.
