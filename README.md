# Resolve - Support Ticket Management API

A backend API built for enterprise support ticket management, featuring automated priority triage.

## Tech Stack
- **Framework:** .NET 9 with ASP.NET Core Web API
- **Database:** PostgreSQL with Entity Framework Core
- **Documentation:** Swagger / OpenAPI

## Key Features
- **Ticket Lifecycle:** Create, list, and update support tickets with status tracking.
- **Smart Triage Simulation:** Automatically sets ticket priority to `HIGH` based on keyword detection in the description like "urgent", "down", etc..
