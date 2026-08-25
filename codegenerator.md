dotnet tool install -g dotnet-aspnet-codegenerator --version 8.0.23

dotnet-aspnet-codegenerator controller
-name [ControllerName]
-api
-m [ModelName]
-dc [DbContextName]



dotnet-aspnet-codegenerator controller -name PeopleController -api -m People -dc HotelListingDbContext -outDir Controllers




dotnet ef migrations add InitialCreate
dotnet ef database update