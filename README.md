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
* **Product Core (Done):** Multi-language support (`ProductTranslations`), Variant Management, Pagination, and Filtering.
* **Security (In Progress):** JWT Authentication & Role-based Authorization.
* **Shopping (Roadmap):** Cart system, Order processing, and Inventory sync.
* **Infrastructure (Roadmap):** Cloud Media Storage, Serilog, and xUnit Testing.

---

## 🤖 AI Agent Roadmap

### 🌟 Phase 1: Smart Consultant (Intent-Based Search)
* **Method:** **Structured RAG** (AI Intent Parsing + SQL Filtering).
* **Workflow:**
    * **Intent Extraction:** Utilize **OpenRouter** to parse user natural language queries into a structured `AiSearchCriteria` (JSON) object.
    * **Metadata Search:** Execute the `SearchByAiCriteriaAsync` method to filter real-time products from SQL Server (Category, Price, Tags, Colors, Materials).
    * **Response Synthesis:** The AI combines the retrieved search results to generate a personalized, human-like recommendation.
* **Status:** [In Progress] Implementing Intent Parsing logic and Database Querying.

### 🛠️ Phase 2: Admin Copilot (Management Assistant)
* **Method:** **Function Calling** via MediatR.
* **Workflow:**
    * Define a Tool Schema (JSON) for administrative reports.
    * The AI automatically triggers existing `Queries/Commands` to fetch real-time inventory, sales reports, or order statuses.
* **Goal:** Empower Admins to manage the system using natural language commands.

### 📈 Phase 3: Hybrid Search & Optimization
* **Hybrid Search:** Combine **Structured SQL Filters** with **Vector Search** (Semantic Search) to handle complex queries (e.g., "Looking for a cozy and elegant gift").
* **Resilience:** Integrate **Polly** for Retry/Circuit Breaker patterns when calling OpenRouter API.
* **Memory:** Implement **Redis** to store Chat History for seamless multi-turn conversations.
---

## 🛠 Setup & Installation

### 1. Prerequisites
* **SDK:** [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* **Database:** SQL Server (LocalDB/Express).
* **AI Service:** OpenRouter API Key.

### 2. Core Packages Integration
```bash
# Data & Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Logic & Design Patterns
dotnet add package MediatR
dotnet add package AutoMapper
dotnet add package FluentValidation.AspNetCore

# AI & Resilience
dotnet add package OpenAI
dotnet add package Polly
```

---


---
