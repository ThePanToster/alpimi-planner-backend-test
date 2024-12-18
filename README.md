## Alpimi planner backend
A WEP API created in .NET for the Alpimi planner

# Setup for development

- Install [SQL Server Developer](https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads)

- Launch `View > Server Explorer` in Visual Studio, right click `Data Connections` and `Create New SQL Server Database`, provide your hostname as a server name and check `Trust server certificate`

- After a successful connection, right click the database, select `Properties` and copy the connection string

- Create a `.env` file inside the root of the project containing:
```
CONNECTION_STRING="put_connection_string_here"
JWT_KEY="put_key_here"
JWT_EXPIRE=60
JWT_ISSUER="https://example.com/"
HASH_ITERATIONS=10
HASH_ALGORYIHM=SHA1
KEY_SIZE=20
```

- Install Entity Framework tool globally
```
dotnet tool install --global dotnet-ef
```

- Update your database with the latest migration
```
dotnet ef database update
```

# Tips

- Install [CSharpier](https://marketplace.visualstudio.com/items?itemName=csharpier.CSharpier), go to `Tools > Options > CSharpier > General` and change `Reformat with CSharpier on Save` to `True`. It will save you a lot of time

- Remember to add a migration whenever an entity is changed:
```
dotnet ef migrations add MigrationName
dotnet ef database update
```
- How to remove migration:
```
dotnet ef database update PreviousMigrationName
dotnet ef migrations remove
```