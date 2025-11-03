# Assignly - Task Management System (Backend)

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)
![License](https://img.shields.io/badge/license-MIT-green)

**Assignly** is a comprehensive task management system built with **.NET** (backend) and **Angular** (frontend). It provides powerful features for organizing teams, managing tasks, and tracking progress with real-time collaboration capabilities.

## Features

### Authentication & Authorization
- **Internal Registration**: Email-based user registration and login
- **External Authentication**: OAuth integration with Google and other providers
- **Role-Based Access Control (RBAC)**: Admin and Member roles with granular permissions

### Organization & Workspace Management
- **Multi-Organization Support**: Create and manage multiple organizations
- **Hierarchical Structure**: Organizations → Workspaces → Modules → Tasks
- **Advanced Search**: Search across workspaces, modules, and tasks

### Task Management
- **Task Assignment**: Assign tasks to team members via email
- **Task Views**: Calendar view and List view for better visualization
- **Task Access Control**: Only admins can edit tasks; members can view and update status
- **Task Organization**: Cards organized in lists within modules

### Real-Time Collaboration
- **Live Comments**: Real-time commenting system on tasks
- **Attachments Support**: Upload images and add hyperlinks to comments
- **Instant Updates**: Real-time notifications using SignalR

### Roadmap Planning
- **Personal Roadmaps**: Users can create and assign roadmaps to themselves
- **Team Roadmaps**: Admins can assign roadmaps to team members
- **Progress Tracking**: Visual representation of roadmap progress

### Notifications & Automation
- **Email Notifications**: Automated email notifications for task assignments, updates, and mentions
- **In-App Notifications**: Real-time notification system

### Customization & Localization
- **Theme Support**: Multiple theme options for personalized user experience
- **Internationalization**: Multi-language support with localization
- **User Guide**: Built-in user guide and documentation

## Entity Relationships

### Core Entities

```
User (1) ──────── (M) Organization
    │
    ├─── Role: Admin | Member
    │
    ├─── (M) Task Assignment (M) ──── Task
    │
    ├─── (1) ────── (M) Comment ──── (1) Task
    │
    ├─── (1) ────── (M) Notification
    │
    └─── (M) ────── (M) Roadmap

Organization (1) ──── (M) Workspace

Workspace (1) ──── (M) Module

Module (1) ──── (M) Task

Task (1) ──── (M) Attachment
    │             ├── File
    │             └── Link
    │
    └─── (M) Comment (M) ──── (M) Attachment
```

### Entity Breakdown

1. **User**: Authentication, roles, profile information
2. **Organization**: Top-level entity for team grouping
3. **Workspace**: Project or department-level organization
4. **Module**: Feature or category grouping (Lists)
5. **Task**: Individual work items (Cards)
6. **Comment**: Real-time discussions on tasks
7. **Attachment**: Files and links attached to tasks/comments
8. **Notification**: User alerts and updates
9. **Roadmap**: Strategic planning and milestone tracking

## Technology Stack

### Core Framework
- **.NET 9.0**: Latest LTS version
- **ASP.NET Core Web API**: RESTful API framework
- **C# 13**: Latest language features

### Database & ORM
- **Entity Framework Core 9**: ORM and database migrations
- **SQL Server**: Primary database (configurable)

### Authentication & Security
- **ASP.NET Core Identity**: User management
- **JWT Bearer Tokens**: Stateless authentication
- **OAuth 2.0**: External authentication (Google, etc.)

### Real-Time Communication
- **SignalR**: WebSocket-based real-time messaging

### File Storage
- **Local File System**: Development storage

### Additional Libraries
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation
- **Swagger/OpenAPI**: API documentation

## 📁 Project Structure (Clean Architecture)

```
Assignly/ (Solution)
│
├── Assignly.API/                       # Presentation layer (Web API)
│   ├── Connected Services/
│   ├── Dependencies/
│   ├── Properties/
│   ├── Controllers/                    # API Controllers
│   ├── appsettings.json               # Configuration
│   └── Program.cs                     # Application entry point
│
├── Assignly.Core/                      # Core business logic
│   ├── Dependencies/
│   ├── DTOs/                          # Data Transfer Objects
│   └── Profiler.cs                    # Performance profiling
│
├── Assignly.Data/                      # Data layer
│   ├── Dependencies/
│   ├── Enums/                         # Enumerations (Roles, etc.)
│   └── Models/                        # Domain entities
│       ├── User.cs
│       ├── Organization.cs
│       ├── Workspace.cs
│       ├── Module.cs
│       ├── Task.cs
│       ├── Comment.cs
│       ├── Attachment.cs
│       ├── Notification.cs
│       └── Roadmap.cs
│
├── Assignly.Infrastructure/            # Infrastructure layer
│   ├── Dependencies/
│   ├── Migrations/                    # EF Core migrations
│   ├── Repositories/                  # Data access repositories
│   └── AppDBContext.cs               # Database context
│
└── Assignly.Service/                   # Service layer (Business logic)
    ├── Dependencies/
    └── Services/                      # Business services
        ├── AuthService.cs
        ├── TaskService.cs
        ├── CommentService.cs
        ├── AttachmentService.cs
        ├── NotificationService.cs
        └── EmailService.cs
```
