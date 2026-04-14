# Product Management System (.NET 8)

A high-performance backend system built with **.NET 8**, adhering to **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

---

## Technical Stack
* **Framework:** .NET 8 (LTS)
* **Architecture:** Clean Architecture (Domain, Application, Infrastructure, API)
* **Patterns:** CQRS with **MediatR**, Repository & Unit of Work.
* **Distributed Systems:** **MassTransit** Redis for Caching and distributed locking.
* **Data:** SQL Server & Entity Framework Core 8.
* **Validation & Mapping:** FluentValidation & AutoMapper.
* **Security:** JWT Authentication & ASP.NET Core Identity.

---

## Key Features

### ✅ Completed
* **Product Core:** Multi-language support (`ProductTranslations`), Variant Management, Pagination & Filtering, Category hierarchy.
* **User Authentication:** JWT Authentication, Email confirmation (ASP.NET Core Identity), Role-based Authorization.
* **Shopping:** Cart system, Order processing, Order tracking, and Inventory sync.
* **Payment Integration (MoMo):**
  - CQRS-based Payment Commands (`CreatePaymentCommand`, `UpdatePaymentStatusCommand`).
  - **MoMo e-Wallet Integration:** HMAC-SHA256 signature generation, Payment URL creation, IPN webhook handling.
  - **Idempotency & Race Condition Handling:** Redis RedLock for distributed locking, callback validation, transaction logging.
  - **Real-time Updates:** SignalR Hub for payment status notifications (`PaymentStatusUpdated` event).
  - **Database Persistence:** Transaction entity, Order status updates, audit logging.

### 🔄 In Progress
* **Concurrency & Resilience:** Redis RedLock implementation for race condition prevention, Polly retry policies for external API calls.
* **AI Agent - Smart Consultant:** Intent-based product search using Semantic Kernel + OpenRouter (RAG pattern).

### 🗺️ Roadmap
* **Additional Payment Providers:** VNPay, ZaloPay (extensible via `PaymentProviderBase` template).
* **Infrastructure:** Cloud Media Storage (Azure Blob), Serilog structured logging, xUnit comprehensive test suite.
* **Admin Copilot:** AI-powered admin assistant using Function Calling via MediatR.
* **Hybrid Search:** Vector search (Semantic) combined with SQL filtering for advanced product discovery.

---

## Payment System Architecture

### MoMo Integration (e-Wallet Payment)
**Status:** ✅ In Production (MVP)

#### Key Components:
1. **Application Layer (`Features/Payments`)**
   - `CreatePaymentCommand` → Generate Momo payment URL
   - `UpdatePaymentStatusCommand` → Handle IPN webhook callbacks from Momo
   - FluentValidation validators for request validation

2. **Infrastructure Layer (`Services/MomoPaymentService`)**
   - Extends `PaymentProviderBase` using Template Method pattern
   - HMAC-SHA256 signature generation for API request authentication
   - Payment URL creation via Momo API endpoint
   - IPN callback validation (currently basic; signature validation in TODO)

3. **Presentation Layer (SignalR Hub + REST API)**
   - `POST /api/payments/create/{orderId}?provider=MoMo` → Create payment
   - `GET /api/payments/momo-callback` → Client redirect endpoint (UI notification)
   - `POST /api/payments/momo-ipn` → Momo server-side IPN notification
   - `SignalR Hub: /hubs/payment-status` → Real-time payment updates to clients

#### Security & Reliability:
- **Idempotency:** Prevents duplicate payment processing via order ID locking
- **Race Condition Prevention:** Redis RedLock distributed locking
- **Transaction Logging:** All payment attempts logged for reconciliation
- **Order Status Updates:** Automatic sync between Transaction and Order entities

#### Configuration:
```json
{
  "PaymentSettings": {
    "Momo": {
      "PartnerCode": "YOUR_PARTNER_CODE",
      "AccessKey": "YOUR_ACCESS_KEY",
      "SecretKey": "YOUR_SECRET_KEY",
      "Endpoint": "https://test-payment.momo.vn/v3/gateway/api/create",
      "ReturnUrl": "https://yourdomain.com/payments/momo-callback",
      "IpnUrl": "https://yourdomain.com/api/payments/momo-ipn"
    }
  }
}
```

#### Flow Diagram:
```
1. CREATE PAYMENT:
   Client → POST /api/payments/create/123?provider=MoMo
   → CreatePaymentCommand → MomoPaymentService.BuildProviderSpecificUrl()
   → Call MoMo API with HMAC-SHA256 signature
   → Save Transaction (Pending) → Return PayUrl

2. CALLBACK (Client Redirect):
   Momo → GET /api/payments/momo-callback?orderId=123&resultCode=0
   → Controller returns waiting page + SignalR connection

3. IPN WEBHOOK (Server-to-Server):
   Momo → POST /api/payments/momo-ipn (with signature)
   → UpdatePaymentStatusCommand (with Redis lock)
   → Update Transaction & Order status
   → Publish SignalR event PaymentStatusUpdated
   → Client receives real-time update
```

---

## AI Agent Roadmap

### Phase 1: Smart Consultant (Intent-Based Product Search) 
**Status:** 🔄 In Progress

**Method:** Structured RAG (Retrieval-Augmented Generation) with Intent Parsing
* **Step 1:** Parse user natural language query via **OpenRouter API** → Extract structured `SearchCriteria` (JSON)
* **Step 2:** Execute SQL filtering on real-time products (Category, Price, Tags, Colors, Materials)
* **Step 3:** Synthesize AI response combining database results with natural language generation

**Example:**
```
User: "Show me a cozy blue jacket under 500k"
→ AI Intent: { Category: "Jackets", Color: "Blue", MaxPrice: 500000, Mood: "cozy" }
→ SQL Query: SELECT * FROM Products WHERE Category='Jackets' AND Color='Blue' AND Price <= 500000
→ AI Response: "Found 3 elegant jackets perfect for your style..."
```

### Phase 2: Admin Copilot (Function Calling Assistant)
**Status:** 🗓️ Planned (Q2 2026)

**Method:** Function Calling via MediatR
* Define Tool Schema (JSON) for admin operations (sales reports, inventory status, order management)
* AI automatically maps user commands to MediatR Commands/Queries
* Execute and synthesize results

**Example:**
```
Admin: "What's our top-selling product this month?"
→ AI triggers: GetMonthlySalesReportQuery
→ Database returns: [ProductId: 123, Sales: 5000, Revenue: 250M]
→ AI Response: "Your top product is XYZ Jacket (5000 units, 250M VND)"
```

---

## Setup & Installation

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


# Redis 
dotnet add package RedisLock.net
dotnet add package StackExchange.Redis

# AI & Resilience
dotnet add package OpenAI
dotnet add package Polly
```

---


---
