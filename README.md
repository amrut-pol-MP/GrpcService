Project Setup

    Install Docker 
    Execute the following commands in your terminal:

    Download and Run SQL Server Docker Image
    
    docker pull mcr.microsoft.com/mssql/server:2022-latest

    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Ap@12345" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest

Check if the container is running:
	docker ps -a

    Start SQL Server Management Studio (SSMS)
    Connect to the SQL Server instance running in Docker to verify the connection.

    Database Initialization
    No manual migration is required — the database and tables will be created automatically when you run GrpcService.

Endpoints

Organization Operations
    Create Organization
    Get Organization by ID
    Query Organizations (Paginated)
    Update Organization
    Delete Organization