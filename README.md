# Project Assessment

This assessment includes two projects: `GrpcService` and `HttpService`. Both are built using **.NET Core 8.0**.

---

## Project Structure

### GrpcService
This is a **gRPC** project that defines services for:

- **Organization**
- **Users**
- **User-Organization Association**

Each service includes full **CRUD operations**.

### HttpService
This is an **ASP.NET Core Web API** project that **consumes the GrpcService**. It exposes all the CRUD operations from the gRPC services over HTTP.

---

## Project Setup

### Docker Setup (SQL Server)

1. **Install Docker**  
   Download and install Docker from: [https://docs.docker.com/get-docker](https://docs.docker.com/get-docker)

2. **Download and Run SQL Server Docker Image**

   Run below script in terminal
   
   docker pull mcr.microsoft.com/mssql/server:2022-latest

   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Ap@12345" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest

3. **Check if the container is running**

4. **Start SQL Server Management Studio (SSMS)**

   Connect to the SQL Server instance running in Docker to verify the connection.

**Note** : There's no need to manually run any migration scripts. The database and tables will be created automatically when you start the GrpcService application.
   
## Technologies Used

- .NET Core 8.0
- gRPC
- ASP.NET Core Web API
- Docker
- SQL Server 2022