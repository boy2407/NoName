# Project Rules: .NET 8 Clean Architecture

## General Guidelines
- **Architecture**: Strict Clean Architecture (Domain, Application, Infrastructure, WebAPI).
- **Patterns**: Mandatory CQRS with MediatR. No business logic in Controllers.
- **Entity Framework**: Use Fluent API for configurations, not Data Annotations.
- **C# 12 Features**: Use Primary Constructors for Dependency Injection.
- **Naming**: Commands/Queries must end with 'Command' or 'Query'. Handlers must match.
- **Validation**: Use FluentValidation for all Application Requests.
- **Caching**: Separate caching concerns into a dedicated class to avoid fat handlers/controllers.
- **Separation of Concerns**: Separate product-related logic early to reduce heavy classes and improve readability, coding speed, and testability.

## Concurrency Management
- **Inventory Race Conditions**: Handle race conditions using Redis RedLock. The first requester acquires the lock, while subsequent requests wait and retry at short intervals (e.g., 50ms).

## AI Integration
- **AI Agent**: Use models and a framework to build an AI agent into the product.