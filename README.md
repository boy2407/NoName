# 📦 Product Management System (.NET 8)

A high-performance backend system built with **.NET 8**, adhering to **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

---

## 🚀 Technical Stack
* **Framework:** .NET 8 (LTS)
* **Architecture:** Clean Architecture (Domain, Application, Infrastructure, API)
* **Patterns:** CQRS with **MediatR**, Repository & Unit of Work.
* **Data:** SQL Server & Entity Framework Core 8.
* **Validation & Mapping:** FluentValidation & AutoMapper.
* **Security:** JWT Authentication & ASP.NET Core Identity.

---

## ✨ Key Features
* **Product Core (Done):** Multi-language (`ProductTranslations`), Pagination, and Filtering.
* **Security (In Progress):** JWT Authentication & Role-based Authorization.
* **Shopping (Roadmap):** Cart system, Order processing, and Inventory sync.
* **Infrastructure (Roadmap):** Cloud Media Storage, Serilog, and xUnit Testing.

---

## 🛠 Setup & Installation

### 1. Prerequisites
* **SDK:** [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* **Database:** SQL Server (LocalDB or Express).

### 2. Core Packages Integration
If you are setting up the project from scratch, ensure these core packages are installed:

```bash
# Data & Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

# Logic & Design Patterns
dotnet add package MediatR
dotnet add package AutoMapper
dotnet add package FluentValidation.AspNetCore

# Security & Authentication
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer