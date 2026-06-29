-- Добавление новой миграции на основании изменений в сущностях в проекте Domain
dotnet ef migrations add AddNameToCategory --project CatalogService.Persistence --startup-project CatalogService.Api

-- Обновление БД
dotnet ef database update --project CatalogService.Persistence --startup-project CatalogService.Api
