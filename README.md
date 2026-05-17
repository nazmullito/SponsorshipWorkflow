# Sponsorship Workflow System

Enterprise workflow-based sponsorship management platform built with:

* ASP.NET Core 10 Web API
* Clean Architecture
* PostgreSQL (Neon)
* React + Vite + TypeScript
* JWT Authentication
* Role-Based Access Control (RBAC)
* Docker
* Render (Backend Hosting)
* Vercel (Frontend Hosting)
* Neon PostgreSQL (Database)

---

# Live Application

## Frontend URL

[https://sponsorship-workflow-frontend.vercel.app](https://sponsorship-workflow-frontend.vercel.app)

## Backend API Swagger Documentation URL

[https://sponsorshipworkflow.onrender.com/swagger/index.html](https://sponsorshipworkflow.onrender.com/swagger/index.html)

---

# Git Repositories

## Backend Repository

[https://github.com/nazmullito/SponsorshipWorkflow](https://github.com/nazmullito/SponsorshipWorkflow)

## Frontend Repository

[https://github.com/nazmullito/SponsorshipWorkflowFrontend](https://github.com/nazmullito/SponsorshipWorkflowFrontend)

---

# Test Accounts

## Admin

Email:

```text
admin@test.com
```

Password:

```text
Test123!
```

## Finance

Email:

```text
finance@test.com
```

Password:

```text
Test123!
```

## Manager

Email:

```text
manager@test.com
```

Password:

```text
Test123!
```

## Requestor

Email:

```text
requestor@test.com
```

Password:

```text
Test123!
```

> Update credentials if your seeded data differs.

---

# Project Overview

The Sponsorship Workflow System is a workflow-driven enterprise application designed to manage sponsorship requests, approvals, and role-based processing.

The system demonstrates:

* Clean architecture principles
* Enterprise workflow handling
* JWT authentication and authorization
* Role-based access control
* API-driven frontend/backend integration
* Cloud deployment
* Docker-based hosting

The solution is intentionally simplified to focus on:

* maintainable architecture
* workflow logic
* backend/frontend integration
* RBAC implementation
* deployment readiness

---

# Backend

# Technology Stack

* ASP.NET Core 10
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* Docker
* Swagger/OpenAPI
* Clean Architecture

---

# Backend Architecture

The backend follows Clean Architecture principles.

## Layers

### Sponsorship.API

Responsibilities:

* Controllers
* Middleware
* Dependency Injection
* Authentication setup
* Swagger configuration
* Startup configuration

### Sponsorship.Application

Responsibilities:

* Business logic
* DTOs
* Services
* Validation
* Interfaces
* Workflow processing

### Sponsorship.Domain

Responsibilities:

* Core entities
* Enums
* Domain models
* Business rules

### Sponsorship.Infrastructure

Responsibilities:

* Database access
* EF Core configuration
* Repositories
* Authentication implementation
* External integrations

---

# Backend Workflow Logic

The application implements a sponsorship request workflow.

Typical workflow:

1. User submits sponsorship request
2. Manager reviews request
3. Admin approves/rejects request
4. Status transitions are persisted
5. RBAC determines access rights

Workflow state is tracked through:

* request status
* approval state
* role permissions

---

# RBAC (Role-Based Access Control)

The application implements role-based authorization using JWT authentication.

Roles include:

* Admin
* Finance
* Manager
* Requestor

Examples:

| Role          | Permissions                                                                                                          |   |
| ------------- | -------------------------------------------------------------------------------------------------------------------- | - |
| System Admin  | View all requests, view workflow history and manage sponsorship types.                                               |   |
| Finance Admin | View requests pending finance review, approve final sponsorship request, reject request and remarks.                 |   |
| Manager       | View pending manager approvals, approve request, reject request add approval remarks.                                |   |
| Requestor     | Create sponsorship request, save as draft, submit request, view own requests and cancel request if not yet approved. |   |
|               |                                                                                                                      |   |

Authorization is enforced using:

* JWT claims
* ASP.NET Core authorization policies
* role-based controller access

---

# Database Design

Database provider:

* PostgreSQL (Neon)

Main entities include:

* Users
* Roles
* SponsorshipRequests
* WorkflowStatuses

Relationships:

* One user can create many requests
* One role can belong to many users
* Workflow status tracks request lifecycle

Entity Framework Core migrations are used for schema management.

---

# Backend Setup Guide

## Prerequisites

Install:

* .NET SDK 10
* PostgreSQL
* Visual Studio / VS Code
* Docker (optional)

---

# Clone Repository

```bash
git clone https://github.com/nazmullito/SponsorshipWorkflow.git
```

---

# Configure Database

Update:

```text
Sponsorship.API/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=sponsorship_db;Username=postgres;Password=yourpassword"
  }
}
```

---

# Apply Migrations

Run:

```bash
dotnet ef database update --project Sponsorship.Infrastructure --startup-project Sponsorship.API
```

---

# Run Backend

```bash
cd Sponsorship.API

dotnet run
```

Backend runs at:

```text
https://localhost:5001
```

Swagger:

```text
https://localhost:5001/swagger
```

---

# Docker Support

Run using Docker:

```bash
docker build -t sponsorship-api .

docker run -p 8080:8080 sponsorship-api
```

---

# Production Hosting

Backend hosted on:

* Render

Database hosted on:

* Neon PostgreSQL

---

# Frontend

# Technology Stack

* React
* TypeScript
* Vite
* Axios
* React Router
* JWT Authentication
* CSS / UI Components

---

# Frontend Structure

The frontend is component-driven.

Typical structure:

```text
src/
 ├── components/
 ├── pages/
 ├── services/
 ├── routes/
 ├── hooks/
 ├── context/
 ├── layouts/
 └── utils/
```

Responsibilities:

* pages → feature screens
* components → reusable UI
* services → API communication
* routes → navigation and protected routes
* context → authentication/session handling

---

# Frontend Authentication Flow

1. User logs in
2. Backend returns JWT token
3. Token stored in browser storage
4. Axios attaches token to requests
5. Protected routes validate authentication

---

# Frontend Setup Guide

## Prerequisites

Install:

* Node.js
* npm

---

# Clone Repository

```bash
git clone https://github.com/nazmullito/SponsorshipWorkflowFrontend.git
```

---

# Install Dependencies

```bash
npm install
```

---

# Configure Environment Variables

Create:

```text
.env.development
```

Example:

```env
VITE_API_URL=https://localhost:5001/api
```

---

# Run Frontend

```bash
npm run dev
```

Frontend runs at:

```text
http://localhost:5173
```

---

# Production Environment

Create:

```text
.env.production
```

Example:

```env
VITE_API_URL=https://sponsorshipworkflow.onrender.com/api
```

---

# Build Frontend

```bash
npm run build
```

---

# Production Hosting

Frontend hosted on:

* Vercel

---

# Deployment Notes

## Backend Deployment

Backend deployment steps:

1. Push source code to GitHub
2. Connect repository to Render
3. Configure Docker deployment
4. Configure environment variables
5. Connect Neon PostgreSQL
6. Apply migrations automatically

---

## Frontend Deployment

Frontend deployment steps:

1. Push frontend code to GitHub
2. Import project into Vercel
3. Configure Vite settings
4. Add VITE_API_URL environment variable
5. Deploy production build

---

# Assumptions and Tradeoffs

This project was intentionally simplified for assessment purposes.

Tradeoffs include:

* simplified workflow implementation
* simplified UI styling
* limited audit logging
* limited validation coverage
* no advanced caching layer
* no distributed infrastructure
* minimal monitoring/logging

Focus areas prioritized:

* clean architecture
* maintainability
* RBAC implementation
* workflow handling
* deployment readiness
* code organization

---

# Future Improvements

Potential improvements:

* refresh token support
* notification system
* advanced workflow engine
* file uploads
* reporting
* unit/integration testing
* CI/CD pipelines
* Redis caching
* container orchestration
* observability and monitoring

---

# Conclusion

This project demonstrates:

* enterprise-oriented backend architecture
* frontend/backend integration
* RBAC authorization
* workflow-driven design
* PostgreSQL integration
* Docker deployment
* cloud hosting setup
* production-ready development workflow
