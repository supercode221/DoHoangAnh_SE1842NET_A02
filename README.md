# 📰 FU News Management System v2

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red?logo=microsoftsqlserver)
![Architecture](https://img.shields.io/badge/Distributed%20Architecture-4%20APIs-success)
![SignalR](https://img.shields.io/badge/SignalR-Real--time-blue?logo=signalr)
![AI](https://img.shields.io/badge/AI-Tag%20Suggestion-orange)
![Status](https://img.shields.io/badge/Status-Active-brightgreen)

A distributed, intelligent news management system for FPT University featuring AI-powered tag suggestions, real-time notifications, analytics dashboards, and offline capabilities.

---

## 🚀 Overview

The **FU News Management System v2** is a complete rewrite of the original system, now implementing a **distributed microservices architecture** with four independent components. This system provides advanced features including AI-based content tagging, real-time notifications via SignalR, comprehensive analytics, and robust offline mode support.

The system includes:

- **Core API** - Main CRUD operations with JWT authentication and audit logging
- **Analytics API** - Dashboard statistics, trending articles, and Excel exports
- **AI API** - Intelligent tag suggestions based on article content
- **Frontend** - Responsive MVC/Razor Pages interface with background workers and caching

---

## 🧩 Main Features

### 🔐 Authentication & Security

- JWT-based authentication with **Refresh Token** flow
- Automatic token refresh before expiration
- **Audit logging** for all CRUD operations (tracking user actions with before/after data)
- Role-based authorization (Admin, Staff)

### 📊 Analytics & Reporting

- Interactive **Chart.js dashboards** (Pie, Bar charts)
- Real-time statistics: articles by category, status, and author
- **Trending articles** based on view counts
- **Excel report export** functionality
- Advanced filtering by date, category, and status

### 🤖 AI-Powered Features

- **Automatic tag suggestions** based on article content
- Learning cache to remember frequently selected tags
- Confidence scores for suggested tags
- Quick-select chips/badges for tag attachment

### 🔔 Real-Time Notifications

- **SignalR Hub** for live notifications
- Toast notifications when new articles are published
- Notification center with recent activity history (10+ notifications)

### 📡 Distributed Architecture

- **HttpClient** communication between Frontend and APIs
- **Polly Retry Policy** for resilient API calls
- **Background Worker** (HostedService) to refresh cached data every 6 hours
- **Offline Mode** - operates with cached data when APIs are unavailable

### 📝 Content Management

- **CRUD operations** for News Articles, Categories, Tags, and Accounts
- Image upload with validation (file type and size)
- Article duplication support
- **Related articles** recommendations based on category/tags
- Bootstrap Modals for create/edit operations

---

## 🧠 Technical Stack

| Component          | Technology                                         |
| ------------------ | -------------------------------------------------- |
| **Frontend**       | ASP.NET MVC / Razor Pages (.NET 8)                 |
| **Core API**       | ASP.NET Web API (.NET 8)                           |
| **Analytics API**  | ASP.NET Web API (.NET 8)                           |
| **AI API**         | ASP.NET Web API (.NET 8) with OpenAI integration   |
| **Database**       | Microsoft SQL Server                               |
| **ORM**            | Entity Framework Core 8                            |
| **Authentication** | JWT with Refresh Tokens                            |
| **Real-time**      | SignalR                                            |
| **Charts**         | Chart.js                                           |
| **Excel Export**   | EPPlus                                             |
| **Architecture**   | Distributed Microservices (4 independent projects) |

---

## 🏗️ System Architecture

```
┌─────────────────────────────────────────────────────────┐
│                      FRONTEND (MVC)                     │
│  • Background Worker (6-hour cache refresh)             │
│  • SignalR Client                                       │
│  • Offline Mode Support                                 │
└───────────┬─────────────┬─────────────┬─────────────────┘
            │             │             │
    ┌───────▼─────┐ ┌────▼──────┐ ┌───▼──────────┐
    │  CORE API   │ │ ANALYTICS │ │   AI API     │
    │             │ │    API    │ │              │
    │ • JWT Auth  │ │ • Stats   │ │ • Tag        │
    │ • CRUD      │ │ • Trends  │ │   Suggestion │
    │ • Audit Log │ │ • Export  │ │ • Learning   │
    │ • SignalR   │ │ • Filters │ │   Cache      │
    └──────┬──────┘ └─────┬─────┘ └──────────────┘
           │              │
           └──────┬───────┘
                  │
         ┌────────▼─────────┐
         │   SQL SERVER     │
         │  • NewsArticle   │
         │  • Category      │
         │  • Tag           │
         │  • Account       │
         │  • AuditLog      │
         └──────────────────┘
```

---

## ⚙️ Setup & Installation

### Prerequisites

- **.NET 8 SDK** or later
- **SQL Server 2019** or later
- **Visual Studio 2022** or Visual Studio Code
- **(Optional)** OpenAI API Key for AI tag suggestions

### 1️⃣ Database Setup

1. Open **SQL Server Management Studio (SSMS)**
2. Run the provided `FUNewsManagement.sql` script to create the database and seed initial data
3. Verify the following tables are created:
   - `Account`, `Category`, `Tag`, `NewsArticle`, `AuditLog`

### 2️⃣ Core API Setup

1. Open `FUNewsManagement_CoreAPI.sln` in Visual Studio
2. Update `appsettings.json` with your connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=FUNewsManagement;Trusted_Connection=True;TrustServerCertificate=True"
     },
     "JwtSettings": {
       "Secret": "your-super-secret-key-min-32-characters",
       "Issuer": "FUNewsCore",
       "Audience": "FUNewsUsers",
       "ExpiryMinutes": 60,
       "RefreshTokenExpiryDays": 7
     }
   }
   ```
3. Run the project (default: `https://localhost:7001`)

### 3️⃣ Analytics API Setup

1. Open `FUNewsManagement_AnalyticsAPI.sln`
2. Update `appsettings.json` with the same connection string
3. Run the project (default: `https://localhost:7002`)

### 4️⃣ AI API Setup

1. Open `FUNewsManagement_AIAPI.sln`
2. Update `appsettings.json`:
   ```json
   {
     "OpenAI": {
       "ApiKey": "your-openai-api-key-here",
       "Model": "gpt-3.5-turbo"
     }
   }
   ```
   _Note: If you don't have an OpenAI key, the system will use simulated keyword extraction_
3. Run the project (default: `https://localhost:7003`)

### 5️⃣ Frontend Setup

1. Open `FUNewsManagement_FE.sln`
2. Update `appsettings.json` with API URLs:
   ```json
   {
     "ApiSettings": {
       "CoreApiUrl": "https://localhost:7001",
       "AnalyticsApiUrl": "https://localhost:7002",
       "AIApiUrl": "https://localhost:7003"
     }
   }
   ```
3. Run the project (default: `https://localhost:7000`)

### 6️⃣ Access the Application

- Navigate to `https://localhost:7000`
- **Default Admin Account:**
  - Email: `admin@fpt.edu.vn`
  - Password: `Admin@123`
- **Default Staff Account:**
  - Email: `staff@fpt.edu.vn`
  - Password: `Staff@123`

---

## 📁 Project Structure

```
FUNewsManagement/
│
├── FUNewsManagement_CoreAPI.sln
│   ├── Controllers/          # API endpoints for CRUD operations
│   ├── Models/               # Entity models (Account, NewsArticle, etc.)
│   ├── Services/             # Business logic, JWT, Audit logging
│   ├── Data/                 # EF Core DbContext
│   ├── Hubs/                 # SignalR notification hub
│   └── Middleware/           # Authentication, error handling
│
├── FUNewsManagement_AnalyticsAPI.sln
│   ├── Controllers/          # Dashboard, trending, export endpoints
│   ├── Services/             # Analytics calculations, Excel generation
│   └── DTOs/                 # Data transfer objects
│
├── FUNewsManagement_AIAPI.sln
│   ├── Controllers/          # AI tag suggestion endpoint
│   ├── Services/             # OpenAI integration, keyword extraction
│   └── Cache/                # Learning cache for tag suggestions
│
├── FUNewsManagement_FE.sln
│   ├── Controllers/          # MVC controllers
│   ├── Views/                # Razor views
│   ├── Services/             # HttpClient services for each API
│   ├── BackgroundServices/   # HostedService for cache refresh
│   ├── wwwroot/              # Static files (JS, CSS, Chart.js)
│   └── SignalR/              # SignalR client configuration
│
└── FUNewsManagement.sql      # Database initialization script
```

---

## 🔌 API Endpoints

### Core API (`https://localhost:7001`)

| Method              | Endpoint            | Description                              |
| ------------------- | ------------------- | ---------------------------------------- |
| POST                | `/api/auth/login`   | User login (returns JWT + refresh token) |
| POST                | `/api/auth/refresh` | Refresh expired JWT                      |
| GET                 | `/api/news`         | List all news articles (with pagination) |
| GET                 | `/api/news/{id}`    | Get article details                      |
| POST                | `/api/news`         | Create new article                       |
| PUT                 | `/api/news/{id}`    | Update article                           |
| DELETE              | `/api/news/{id}`    | Delete article                           |
| GET/POST/PUT/DELETE | `/api/category`     | Category CRUD                            |
| GET/POST/PUT/DELETE | `/api/tag`          | Tag CRUD                                 |
| GET/POST/PUT/DELETE | `/api/account`      | Account CRUD (Admin only)                |
| GET                 | `/api/auditlog`     | View audit history (Admin only)          |

### Analytics API (`https://localhost:7002`)

| Method | Endpoint                        | Description          |
| ------ | ------------------------------- | -------------------- |
| GET    | `/api/analytics/dashboard`      | Dashboard statistics |
| GET    | `/api/analytics/trending`       | Most viewed articles |
| GET    | `/api/analytics/recommend/{id}` | Related articles     |
| GET    | `/api/analytics/export`         | Export Excel report  |

### AI API (`https://localhost:7003`)

| Method | Endpoint               | Description                   |
| ------ | ---------------------- | ----------------------------- |
| POST   | `/api/ai/suggest-tags` | Suggest tags based on content |

---

## 🎯 Key Features Implementation

### 🔄 Refresh Token Flow

- Access tokens expire after 60 minutes
- Refresh tokens valid for 7 days
- Frontend automatically refreshes tokens before expiration
- Manual refresh via `/api/auth/refresh`

### 📋 Audit Logging

- Tracks all CRUD operations
- Records: User, Action, Entity, Timestamp, Before/After JSON
- Admin-only access to audit logs
- Filterable by user and entity type

### 🔔 SignalR Notifications

- Real-time notifications when articles are created
- Toast notifications in UI
- Notification center with history
- Automatic reconnection on disconnect

### 💾 Caching & Offline Mode

- Background Worker refreshes cache every 6 hours
- Offline banner when APIs are unreachable
- Read-only access to cached data
- Local JSON fallback

### 🎨 UI/UX Features

- Responsive design (Bootstrap 5)
- Loading indicators for all API calls
- Toast/Alert messages for success/failure
- Confirmation prompts for CRUD operations
- Keyboard navigation support
- High color contrast for accessibility

---

## 📸 Screenshots

### Dashboard

![Dashboard with Chart.js visualizations showing article statistics]

### AI Tag Suggestion

![AI-powered tag suggestions with confidence scores]

### Real-time Notifications

![SignalR notification toast when new article is published]

### Offline Mode

![Offline mode banner with cached data display]

---

## 🧪 Testing Accounts

| Role  | Email              | Password    | Permissions                                     |
| ----- | ------------------ | ----------- | ----------------------------------------------- |
| Admin | `admin@fpt.edu.vn` | `Admin@123` | Full system access, user management, audit logs |
| Staff | `staff@fpt.edu.vn` | `Staff@123` | CRUD news, categories, tags                     |

---

## 🚀 Advanced Features

### Background Worker

- Runs every 6 hours automatically
- Refreshes cached data from Core API
- Logs execution times and status
- Configurable interval in `appsettings.json`

### Polly Retry Policy

- Automatic retry on transient failures
- 3 retry attempts with exponential backoff
- Circuit breaker pattern for API resilience
- Timeout handling

### Excel Export

- Export dashboard data to `.xlsx` format
- Includes filters applied in UI
- Formatted with headers and styling
- Download via Analytics API

---

## 📊 Performance Metrics

- **API Response Time:** < 1 second (target)
- **Cache Hit Rate:** ~80% during offline mode
- **Background Worker:** Every 6 hours
- **SignalR Latency:** < 100ms for notifications

---

## 🛠️ Troubleshooting

### APIs not connecting

- Verify all 4 projects are running simultaneously
- Check firewall settings for ports 7000-7003
- Ensure `ApiSettings` URLs match in Frontend `appsettings.json`

### Database connection errors

- Confirm SQL Server is running
- Verify connection string in all API projects
- Check database name matches `FUNewsManagement`

### JWT token errors

- Ensure `JwtSettings.Secret` is at least 32 characters
- Verify same secret is used across all APIs
- Check token expiration settings

### SignalR not working

- Confirm Core API SignalR hub is running at `/hubs/notifications`
- Check browser console for connection errors
- Verify CORS policy allows SignalR connections

---

## 📜 License

This project is developed as part of the **PRN232 Assignment** at FPT University.  
All rights reserved © 2025.

---

## 👤 Author

**Student Name:** Đỗ Hoàng Anh
**Student Code:** HE181538
**Course:** PRN232 - Cross-Platform Application Development  
**Institution:** FPT University

---

## 📞 Support

For questions or issues:

- 📧 Email: [anhdhhe181538@fpt.edu.vn]
- 📚 Course Material: PRN232 Assignment 02 Documentation
- 🔗 Repository: [GitHub URL if applicable]

---

## ✅ Submission Checklist

- [ ] All 4 solution files (.sln)
- [ ] Database script (FUNewsManagement.sql)
- [ ] README.md (this file)
- [ ] Screenshots folder with:
  - [ ] API responses (Postman/Swagger)
  - [ ] AI tag suggestion demo
  - [ ] Dashboard with charts
  - [ ] SignalR notifications
  - [ ] Offline mode
  - [ ] Audit log view
- [ ] Test accounts documented
- [ ] All API URLs configured correctly

---

**🎓 Academic Integrity Notice:** This project is submitted for educational purposes as part of PRN232 coursework at FPT University.
