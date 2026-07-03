# ADR-002: Authentication & Authorization Architecture

**Status:** Accepted

**Date:** 2026-07-03

---

# Context

Integron ERP is a multi-tenant SaaS platform where multiple companies share the same application instance.

The authentication system must provide:

* Secure user authentication
* Tenant isolation
* Flexible authorization
* Enterprise scalability
* Future support for external identity providers

Authentication is considered a core platform capability and will be consumed by every business module.

---

# Decision

The platform will implement a centralized Authentication module responsible for identity and access management.

The Authentication module owns:

* Company (Tenant)
* User
* Role
* Permission
* UserRole
* RolePermission
* User Session
* Refresh Token (if applicable)
* Password Management
* Audit Logging

Business modules must not manage users or permissions directly.

---

# Multi-Tenant Strategy

The system will use a shared PostgreSQL database.

Each tenant is represented by a Company.

Every tenant-owned entity includes a CompanyId.

Authorization rules ensure users can only access data belonging to their own company.

Future migration to database-per-tenant remains possible.

---

# Authentication Strategy

Authentication will be implemented using ASP.NET Core Identity.

Identity will manage:

* Users
* Password hashing
* Password validation
* Security stamps
* Account lockout
* Password reset

The authentication implementation should remain independent from business modules.

---

# Authorization Strategy

The platform uses Role-Based Access Control (RBAC) combined with permission-based authorization.

Users receive one or more roles.

Roles contain one or more permissions.

Permissions determine access to specific business capabilities.

Example:

Inventory.View

Inventory.Create

Inventory.Update

Inventory.Delete

Sales.Create

Reports.View

This approach provides flexibility without creating excessive numbers of roles.

---

# Company Ownership Model

Platform Administrator

↓

Company

↓

Company Administrator

↓

Employees

Only Company Administrators can manage users within their own tenant.

Platform administration remains isolated from tenant administration.

---

# Session Management

User sessions belong to a specific company.

Each authenticated request carries tenant context.

Authentication middleware validates:

* Identity
* Active session
* Tenant membership
* Authorization

---

# Security Principles

The authentication system must support:

* Secure password hashing
* HTTPS-only communication
* Account lockout after repeated failed attempts
* Password complexity validation
* Audit logging for authentication events

Future enhancements include:

* Multi-Factor Authentication (MFA)
* Single Sign-On (SSO)
* OAuth/OpenID Connect integration
* External Identity Providers

---

# Module Boundaries

Only the Authentication module may:

* Create users
* Delete users
* Assign roles
* Assign permissions
* Authenticate users

Other modules interact with Authentication only through defined contracts.

No module may directly manipulate authentication data.

---

# Consequences

Benefits:

* Clear ownership of identity management
* Strong tenant isolation
* Flexible authorization model
* Easier future migration to a dedicated Authentication service
* Consistent security model across the platform

Trade-offs:

* Authentication becomes a foundational dependency for all business modules.
* Authorization design must remain stable as the platform grows.
