# 🚀 Resolve - Support Ticket Management API

A robust backend API built for enterprise support ticket management, featuring agent assignment, role-based security, and automated priority triage powered by AI.

---

## 🛠️ Tech Stack

* **Framework:** .NET 9 with ASP.NET Core Web API
* **Database & ORM:** PostgreSQL with Entity Framework Core
* **Security & Auth:** JWT Authentication and Role-based management (Client / Agent)
* **AI Integration:** Google Gemini API for automated ticket analysis and categorization
* **Documentation:** Swagger / OpenAPI

---

## ✨ Key Features

* **Authentication & Role Management:** Secure registration and login endpoints with role distinction (Clients and Agents).
* **Ticket Lifecycle:** Create, list, update, and track support tickets with status updates.
* **Agent Assignment ("Take Charge"):** Dynamic retrieval of available agents and quick ticket assignment workflows.
* **AI-Powered Analysis (`AiAnalysis`):** Automatically processes newly created support tickets using the Google Gemini API to provide instant categorization and recommendations.
* **Smart Triage Simulation:** Automatically adjusts ticket priority based on keyword detection in the description (e.g., "urgent", "down").

---

## ⚙️ Quick Start

### Prerequisites
* .NET 9 SDK
* PostgreSQL

### 1. Database Configuration
Update your connection string in `appsettings.json`.

### 2. Restore and Migrate
\`\`\`bash
dotnet restore
dotnet ef database update
\`\`\`

### 3. Run the API
\`\`\`bash
dotnet run
\`\`\`
*(The API runs by default on `http://localhost:5076` and Swagger documentation is available at `/swagger`)*
