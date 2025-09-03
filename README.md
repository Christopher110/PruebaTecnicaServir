# Prueba Técnica – Starter MVC v3 (Neutral + Alerts)

- .NET 8 + EF Core + SQL Server + Bootstrap

## Run
```bash
cd src/PruebaFullstack
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef migrations add Init
dotnet ef database update
dotnet run
```
