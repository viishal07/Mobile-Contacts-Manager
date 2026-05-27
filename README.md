# 📇 Contact Manager — Full-Stack Application

A modern, Google Contacts-style contact management app built with **Angular 20** (frontend) and **ASP.NET Core 8** (backend), connected to **PostgreSQL via Supabase**.

---

## 🏗️ Project Structure

```
ContactManager/
├── backend/
│   └── ContactManager.API/
│       ├── Controllers/          # HTTP API endpoints
│       ├── Data/                 # EF Core DbContext
│       ├── DTOs/                 # Data Transfer Objects
│       ├── Interfaces/           # Service & Repository contracts
│       ├── Mappings/             # AutoMapper profiles
│       ├── Middleware/           # Global exception handler
│       ├── Migrations/           # EF Core migrations
│       ├── Models/               # Entity models
│       ├── Repositories/         # Data access layer
│       ├── Services/             # Business logic layer
│       ├── appsettings.json
│       ├── Program.cs
│       └── ContactManager.API.csproj
│
├── frontend/
│   └── src/
│       ├── app/
│       │   ├── components/
│       │   │   ├── sidebar/            # App navigation sidebar
│       │   │   ├── contact-list/       # Scrollable contact list
│       │   │   ├── contact-detail/     # Contact detail panel
│       │   │   ├── contact-form/       # Add/Edit dialog
│       │   │   └── confirm-dialog/     # Delete confirmation
│       │   ├── layouts/
│       │   │   └── main-layout/        # Shell layout with dark mode
│       │   ├── models/                 # TypeScript interfaces
│       │   ├── pages/
│       │   │   └── contacts-page/      # Main page (list + detail)
│       │   ├── services/               # API service (HttpClient)
│       │   ├── app.component.ts
│       │   ├── app.config.ts
│       │   └── app.routes.ts
│       ├── environments/
│       │   ├── environment.ts
│       │   └── environment.development.ts
│       ├── index.html
│       ├── main.ts
│       └── styles.scss
│
├── docker-compose.yml
└── README.md
```

---

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Node.js 20+](https://nodejs.org/)
- [Angular CLI 20](https://angular.io/cli): `npm install -g @angular/cli`

---

### Backend Setup

```bash
# Navigate to backend project
cd backend/ContactManager.API

# Restore NuGet packages
dotnet restore

# Apply EF Core migrations to Supabase PostgreSQL
dotnet ef database update

# Run the API (available at http://localhost:5000)
dotnet run
```

> ✅ Swagger UI: http://localhost:5000/swagger

---

### Frontend Setup

```bash
# Navigate to frontend
cd frontend

# Install npm packages
npm install --legacy-peer-deps

# Start Angular dev server (available at http://localhost:4200)
ng serve
```

> ✅ App: http://localhost:4200

---

## 🐳 Docker Deployment

Run both frontend and backend with a single command:

```bash
# From the project root (ContactManager/)
docker-compose up --build
```

| Service  | URL                       |
|----------|---------------------------|
| Frontend | http://localhost:4200      |
| Backend  | http://localhost:5000      |
| Swagger  | http://localhost:5000/swagger |

---

## 🗄️ Database Schema

```sql
CREATE TABLE "Contacts" (
    "Id"          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "FirstName"   VARCHAR(100) NOT NULL,
    "LastName"    VARCHAR(100),
    "Email"       VARCHAR(200),
    "PhoneNumber" VARCHAR(20),
    "Company"     VARCHAR(200),
    "Address"     VARCHAR(500),
    "Favorite"    BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt"   TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UpdatedAt"   TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IX_Contacts_Email     ON "Contacts"("Email");
CREATE INDEX IX_Contacts_FirstName ON "Contacts"("FirstName");
CREATE INDEX IX_Contacts_Favorite  ON "Contacts"("Favorite");
```

---

## 📡 API Endpoints

| Method | Endpoint                | Description                          |
|--------|-------------------------|--------------------------------------|
| GET    | `/api/contacts`         | List all contacts (search, filter, paginate) |
| GET    | `/api/contacts/{id}`    | Get single contact                   |
| POST   | `/api/contacts`         | Create new contact                   |
| PUT    | `/api/contacts/{id}`    | Update contact                       |
| DELETE | `/api/contacts/{id}`    | Delete contact                       |

### Query Parameters (GET /api/contacts)

| Param      | Type    | Description                   |
|------------|---------|-------------------------------|
| `search`   | string  | Search by name/email/phone/company |
| `favorite` | bool    | Filter favorites only         |
| `page`     | int     | Page number (default: 1)      |
| `pageSize` | int     | Items per page (default: 20)  |

---

## ✨ Features

- ✅ Full CRUD — Create, Read, Update, Delete contacts
- ✅ Real-time search with debounce (350ms)
- ✅ Favorites filter via sidebar
- ✅ Dark mode toggle (persisted in localStorage)
- ✅ Pagination support
- ✅ Contact avatar with initials + color
- ✅ Skeleton loading state
- ✅ Snackbar notifications (success/error)
- ✅ Confirm delete dialog
- ✅ Responsive design (mobile + desktop)
- ✅ Global exception middleware
- ✅ Swagger / OpenAPI docs
- ✅ Repository + Service pattern (Clean Architecture)
- ✅ AutoMapper for DTO mapping
- ✅ EF Core with PostgreSQL (Supabase)
- ✅ Docker support

---

## 🧪 Manual EF Core Commands

```bash
# Add a new migration
dotnet ef migrations add <MigrationName>

# Apply migrations
dotnet ef database update

# Revert last migration
dotnet ef migrations remove

# List migrations
dotnet ef migrations list
```

---

## 🛠️ Tech Stack

| Layer      | Technology                          |
|------------|-------------------------------------|
| Frontend   | Angular 20, Angular Material, RxJS  |
| Backend    | ASP.NET Core 8, EF Core 8           |
| Database   | PostgreSQL (Supabase)               |
| ORM        | Entity Framework Core + Npgsql      |
| Mapping    | AutoMapper                          |
| Docs       | Swagger / Swashbuckle               |
| Container  | Docker + Docker Compose             |
