# Project Rules: .NET 8 Clean Architecture

## General Guidelines
- **Architecture**: Strict Clean Architecture (Domain, Application, Infrastructure, WebAPI).
- **Patterns**: Mandatory CQRS with MediatR. No business logic in Controllers.
- **Entity Framework**: Use Fluent API for configurations, not Data Annotations.
- **C# 12 Features**: Use Primary Constructors for Dependency Injection.
- **Naming**: Commands/Queries must end with 'Command' or 'Query'. Handlers must match.
- **Validation**: Use FluentValidation for all Application Requests.

## AI Integration
- **AI Agent**: Use models and a framework to build an AI agent into the product.