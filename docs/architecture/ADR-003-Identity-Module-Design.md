# ADR-003: Identity Module Architecture

**Status:** Accepted

**Date:** 2026-08-03

---

# Context

The Identity module is the foundational business module of Integron ERP.

Every authenticated request, tenant-aware operation, and authorization decision depends on this module.

The platform requires a centralized identity system that provides:

* Company (Tenant) management
* User management
* Authentication
* Authorization
* Password management
* Session management
* Tenant isolation

The Identity module must remain independent from business modules while exposing a consistent API for identity-related operations.

---

# Decision

The Identity module will be implemented as an independent module following Clean Architecture principles.

The module owns all identity-related business logic, including:

* Company Registration
* Company Management
* User Management
* Authentication
* Authorization
* Role Management
* Password Management
* Refresh Token Management

Business modules must never directly manipulate identity data.

---

# Architectural Style

The Identity module follows Clean Architecture.

The project is divided into four layers:

* Domain
* Application
* Infrastructure
* Presentation

Responsibilities are separated as follows:

**Domain**

Contains:

* Entities
* Repository Interfaces
* Constants
* Domain Rules

The Domain layer has no dependencies on other layers.

---

**Application**

Contains:

* CQRS Commands
* CQRS Queries
* DTOs
* Validators
* Business Rules

Business logic is implemented through MediatR request handlers.

FluentValidation performs request validation through a MediatR pipeline behavior.

---

**Infrastructure**

Contains:

* Entity Framework Core
* ASP.NET Core Identity
* Repository Implementations
* JWT Token Service
* Refresh Token Persistence
* Database Configuration

Infrastructure implements contracts defined by the Domain layer.

---

**Presentation**

Contains:

* API Controllers
* Authorization Attributes
* HTTP Endpoints

Controllers delegate requests to MediatR and contain no business logic.

---

# Authentication Strategy

Authentication is implemented using ASP.NET Core Identity together with JWT authentication.

The system supports:

* Company Registration
* User Login
* JWT Access Tokens
* Refresh Tokens
* Logout
* Password Change
* Password Reset

Refresh tokens are securely stored in the database and can be revoked when required.

Changing or resetting a password revokes all active refresh tokens for the affected user.

---

# Authorization Strategy

Authorization is implemented using Role-Based Access Control (RBAC).

Current system roles include:

* Owner
* Manager

Authorization is enforced through:

* ASP.NET Authorization Attributes
* Role Policies
* Business Rule Validation

Examples include:

* Only Owners can manage company information.
* Only Owners can change user roles.
* Users cannot deactivate their own accounts.
* A company must always have at least one active Owner.

The architecture allows future migration toward permission-based authorization without significant structural changes.

---

# Multi-Tenant Strategy

The Identity module enforces strict tenant isolation.

Each authenticated request carries the current CompanyId.

The CompanyId is resolved through ICurrentUserService rather than directly accessing HTTP claims.

All queries and commands validate tenant ownership before accessing or modifying data.

No tenant can access another tenant's resources.

---

# Security Principles

The Identity module follows the following security principles:

* Password hashing through ASP.NET Core Identity
* JWT authentication
* Refresh token rotation and revocation
* Secure password validation
* Role-based authorization
* Tenant isolation
* Validation through FluentValidation
* Centralized exception handling
* Unauthorized requests return standardized responses

Future enhancements may include:

* Multi-Factor Authentication (MFA)
* OAuth/OpenID Connect
* External Identity Providers
* Audit Logging
* Session Monitoring

---

# Design Patterns

The Identity module adopts the following architectural patterns:

* Clean Architecture
* CQRS
* MediatR
* Repository Pattern
* Unit of Work
* Dependency Injection
* Validation Pipeline
* Global Exception Middleware

These patterns provide strong separation of concerns and improve maintainability and testability.

---

# Module Boundaries

The Identity module exclusively owns:

* Company
* ApplicationUser
* RefreshToken
* Roles
* Authentication
* Authorization

Other business modules may consume identity information but must never directly modify identity entities.

All identity operations must pass through the Identity module.

---

# Consequences

Benefits:

* Clear ownership of identity functionality
* Strong tenant isolation
* Consistent authentication and authorization model
* Highly maintainable architecture
* Improved scalability for future modules
* Reusable architectural patterns across the ERP
* Easier testing through CQRS and dependency injection

Trade-offs:

* Increased number of classes due to CQRS separation.
* Additional infrastructure compared to a traditional CRUD architecture.
* Slightly higher initial development effort in exchange for improved long-term maintainability.