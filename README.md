# DotnetFoundation

> A powerful, scalable boilerplate for building robust **.NET 8** server applications.

DotnetFoundation provides a production-ready foundation with clean architecture, authentication, logging, API documentation, and health monitoring — all pre-configured to help you build enterprise-grade backend services faster.

---

## 🚀 Features

- ✅ **.NET 8** based modern architecture
- ✅ **JWT Authentication** with secure token handling
- ✅ **Entity Framework Core** integration
- ✅ **Swagger/OpenAPI** for API documentation
- ✅ **Serilog**-based structured **Logging**
- ✅ **Health Checks** and Health Checks UI
- ✅ **AutoMapper** for object mapping
- ✅ **Secure Password Hashing** (BCrypt)
- ✅ **MySQL** (Pomelo) database support
- ✅ **Soft Deletion**, **Global Exception Handling**, and **Pagination** utilities
- ✅ Clean, extendable **Repository Pattern**

---

## Technologies & Packages 📦

- **ASP.NET Core 8**
- **Entity Framework Core (EF Core 8)** with **Pomelo.EntityFrameworkCore.MySql**
- **JWT Authentication** (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- **Swagger & OpenAPI** (`Swashbuckle.AspNetCore`)
- **Serilog Logging** (Console, File, Filters)
- **AutoMapper**
- **BCrypt.Net** (for password hashing)
- **DotNetEnv** (for environment variable management)
- **Health Checks** (with UI monitoring)

---

## Getting Started 🚀

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) running (or compatible MariaDB server)

### Installation Steps

1. **Clone the Repository**

```bash
 git clone https://github.com/OsmosysSoftware/dotnet-foundation-v2.git
 cd dotnet-foundation-v2
```

2. **Navigate to the API Project**

```bash
 cd src/Api
```

3. **Restore Dependencies**

```bash
 dotnet restore
```

4. **Apply Database Migrations**

```bash
 dotnet ef database update --context DatabaseContext
```

5. **Run the Project**

```bash
 dotnet run
```

The API will be available at: `https://localhost:5000` (Swagger UI will be available at `/swagger`).

---

## Project Structure 📂

```bash
src/
├── Api/          # ASP.NET Core Web API
├── Core/         # Domain Models, DTOs, Interfaces, Exceptions
```

- **Api**
  - Configures authentication, logging, exception handling, Swagger, and dependency injection.
- **Core**
  - Contains Entities, Repository Interfaces, DTOs, Enums, and Custom Exceptions.


---

## Configuration ⚙️

The project uses **DotNetEnv** to manage environment variables from a `.env` file.
> Make sure to configure your **database** and **JWT settings** before running.


---

## API Documentation 📖

Once you run the project, visit:

- Swagger UI: `https://localhost:5000/swagger`

You can **test all APIs directly** from Swagger.


---

## Logging 📝

The project uses **Serilog** for structured logging:

- Logs are written to console and also to a **file** (`Logs/log-.txt`)
- Logs include enriched information for easier debugging.


---

## Health Checks ✅

Health checks are integrated with:

- Database Connectivity Check
- Health Check UI available at: `https://localhost:5000/health`


---

## Contributing 🤝

Contributions, issues, and feature requests are welcome!

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/yourFeature`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/yourFeature`)
5. Open a Pull Request


---

## License 📄

This project is licensed under the **MIT License**.


---
