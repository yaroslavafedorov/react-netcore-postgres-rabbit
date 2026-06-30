-- Добавление новой миграции на основании изменений в сущностях в проекте Domain
dotnet ef migrations add ИМЯ_МИГРАЦИИ --project OrderService.Persistence --startup-project OrderService.Api

-- Обновление БД
dotnet ef database update --project OrderService.Persistence --startup-project OrderService.Api
