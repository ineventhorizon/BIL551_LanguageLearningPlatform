

1. Make sure you have the following installed:
   - Visual Studio 2022 or newer
   - .NET SDK (.NET 7 or .NET 6 depending on the project)
   - SQL Server (Express or Developer)
   - SQL Server Management Studio (SSMS)

2. Open the solution file (.sln) in Visual Studio.

3. Install NuGet packages if not done automatically.

4. Update the connection string in `appsettings.json` to point to your local SQL Server instance.

5. Open SQL Server Management Studio and run `database_dump.sql` to create the database.

6. Build and run the project.

7. The web application will open in your browser. You can register as a student or teacher to test the system.
