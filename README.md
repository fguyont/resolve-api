# Resolve - Support Ticket Management API

A backend API built for enterprise support ticket management, featuring automated priority triage.

## Tech Stack
- **Framework:** .NET 9 with ASP.NET Core Web API
- **Database:** PostgreSQL with Entity Framework Core
- **AI Integration:** Google Gemini API (`gemini-3.6-flash`) for automated ticket analysis and categorization
- **Documentation:** Swagger / OpenAPI

## Key Features
- **Ticket Lifecycle:** Create, list, and update support tickets with status tracking.
- **AI-Powered Analysis:** Automatically analyzes newly created support tickets using Google Gemini to provide instant categorization, insights, and priority suggestions.
- **Smart Triage Simulation:** Automatically sets ticket priority to `HIGH` based on keyword detection in the description like "urgent", "down", etc..
