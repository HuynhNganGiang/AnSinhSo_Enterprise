# ============================================================================
# 08_BACKEND_STANDARDS.md
# ============================================================================
# Project       : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
# Document Type : Enterprise Backend Standards
# Version       : 1.0.0
# Status        : FROZEN
# Owner         : Project Architecture Team
# Architecture  : Enterprise Clean Architecture
# Backend       : ASP.NET Core 8 Web API
# Framework     : .NET 8
# Database      : SQL Server 2022
# ORM           : Entity Framework Core
# Authentication: JWT Bearer
# Validation    : FluentValidation
# Logging       : Serilog
# Documentation : Swagger / OpenAPI
# GIS           : Leaflet Integration API
# AI            : OpenAI / Gemini / AntiGravity AI
# Integration   : REST API / Zalo Official Account
# Last Updated  : 2026-07-12
# ============================================================================

# 08. BACKEND STANDARDS

---

# 1. PURPOSE

Tài liệu này quy định toàn bộ tiêu chuẩn phát triển Backend của dự án **AnSinhSo**.

Mục tiêu:

- Chuẩn hóa toàn bộ kiến trúc Backend.
- Đảm bảo khả năng mở rộng lâu dài.
- Đồng nhất giữa các module.
- Hỗ trợ AI Coding sinh mã chính xác.
- Giảm Technical Debt.
- Đảm bảo hệ thống sẵn sàng triển khai Production.

Tài liệu là tiêu chuẩn bắt buộc đối với:

- Backend Developer
- Technical Lead
- Solution Architect
- Reviewer
- Tester
- DevOps
- AI Coding Assistant

---

# 2. SCOPE

Áp dụng cho toàn bộ Backend của hệ thống.

Bao gồm:

- REST API
- Business Logic
- Domain Model
- Application Layer
- Infrastructure Layer
- Entity Framework Core
- SQL Server
- Authentication
- Authorization
- Validation
- Logging
- Audit
- GIS Integration
- AI Integration
- Zalo OA Integration
- Background Services
- Caching
- File Storage
- Notification
- Reporting API

Không áp dụng cho:

- Frontend
- Database Design
- CI/CD Pipeline

Các nội dung này được quy định trong tài liệu riêng.

---

# 3. OBJECTIVES

Backend phải đáp ứng các tiêu chí sau.

## 3.1 Enterprise Ready

Có khả năng triển khai từ cấp xã đến cấp tỉnh hoặc mở rộng quy mô toàn quốc.

---

## 3.2 Production Ready

Không sử dụng:

- Demo Code
- Fake Service
- Temporary Logic
- Hard-code
- Mock Data trong Production

---

## 3.3 Maintainability

Mọi module phải:

- Độc lập
- Dễ đọc
- Dễ kiểm thử
- Dễ mở rộng
- Dễ thay thế

---

## 3.4 Scalability

Cho phép mở rộng:

- Module mới
- API mới
- Chính sách an sinh mới
- AI Service
- GIS Service
- Notification Service
- Third-party Integration

mà không thay đổi kiến trúc nền tảng.

---

## 3.5 Reliability

Backend phải:

- Có khả năng phục hồi lỗi.
- Hỗ trợ Transaction.
- Ghi nhận Audit Log.
- Xử lý Exception tập trung.
- Đảm bảo tính nhất quán dữ liệu.

---

# 4. AUDIENCE

| Vai trò | Mục đích |
|----------|----------|
| Backend Developer | Phát triển dịch vụ |
| Frontend Developer | Đồng bộ API |
| Tester | Kiểm thử API |
| Technical Lead | Review |
| Solution Architect | Thiết kế |
| DevOps | Triển khai |
| AI Coding Assistant | Sinh mã nguồn |

---

# 5. BACKEND PRINCIPLES

## Principle 01 – Clean Architecture

Toàn bộ Backend phải tuân thủ Enterprise Clean Architecture.

Không được phụ thuộc trực tiếp giữa các tầng sai quy định.

---

## Principle 02 – Separation of Concerns

Tách biệt rõ:

- Domain
- Application
- Infrastructure
- API

Mỗi tầng chỉ đảm nhiệm đúng trách nhiệm của mình.

---

## Principle 03 – Business Logic First

Toàn bộ nghiệp vụ phải nằm trong Application/Domain.

Controller không chứa Business Logic.

---

## Principle 04 – API First

Mọi giao tiếp với Frontend hoặc hệ thống bên ngoài phải thông qua REST API.

Không cho phép Frontend truy cập trực tiếp Database.

---

## Principle 05 – Dependency Injection

Toàn bộ Service phải được quản lý thông qua Dependency Injection.

Không khởi tạo thủ công (`new`) các Service nghiệp vụ trong Controller.

---

## Principle 06 – Security First

Bắt buộc áp dụng:

- JWT Authentication
- RBAC
- Input Validation
- Output Sanitization
- Audit Log

---

## Principle 07 – Transaction Safety

Các nghiệp vụ thay đổi dữ liệu phải bảo đảm:

- Atomicity
- Consistency
- Isolation
- Durability (ACID)

---

## Principle 08 – Observability

Backend phải hỗ trợ:

- Structured Logging
- Metrics
- Health Check
- Audit Trail
- Trace ID

---

# 6. BACKEND ARCHITECTURE

```text
Client
    │
    ▼

ASP.NET Core Web API

    │

Controllers

    │

Application Layer

    │

Domain Layer

    │

Infrastructure Layer

    │

Entity Framework Core

    │

SQL Server
```

Mọi truy cập dữ liệu đều phải đi qua Application và Infrastructure.

---

# 7. HIGH-LEVEL ARCHITECTURE

```text
Browser / Mobile / Zalo OA

        │

REST API

        │

Controllers

        │

Application Services

        │

Domain

        │

Repositories

        │

Entity Framework Core

        │

SQL Server
```

Không cho phép:

- Controller gọi trực tiếp DbContext.
- Controller chứa SQL.
- Controller chứa Business Logic.

---

# 8. ARCHITECTURAL RULES

| Rule | Required |
|-------|----------|
| Clean Architecture | Yes |
| REST API | Yes |
| Dependency Injection | Yes |
| Repository Pattern | Yes |
| Unit of Work | Yes |
| FluentValidation | Yes |
| JWT Authentication | Yes |
| RBAC Authorization | Yes |
| Structured Logging | Yes |
| Audit Logging | Yes |
| Transaction Support | Yes |
| Global Exception Handling | Yes |
| Swagger/OpenAPI | Yes |
| Health Check | Yes |

---

# 9. DEFINITION OF BACKEND

Backend bao gồm:

- API Controllers
- Application Services
- Domain Models
- Repositories
- Validation
- Authentication
- Authorization
- Logging
- Audit
- Background Jobs
- Integration Services

Không bao gồm:

- UI Rendering
- CSS
- HTML
- JavaScript
- Database Administration
- DevOps Pipeline

---

# End of Phase 1

Phase tiếp theo:

- Solution Structure
- Layer Responsibilities
- Folder Structure
- Project Organization
# ============================================================================

# 10. SOLUTION STRUCTURE

# ============================================================================

## 10.1 Objective

Toàn bộ Backend phải được tổ chức theo Solution thống nhất nhằm:

* Dễ mở rộng.
* Dễ bảo trì.
* Dễ kiểm thử.
* Giảm phụ thuộc giữa các Project.
* Hỗ trợ AI Coding sinh mã chính xác.

Mọi Solution của AnSinhSo phải tuân theo cùng một cấu trúc.

---

## 10.2 Standard Solution Structure

```text
AnSinhSo.sln

│
├── src/
│
│   ├── AnSinhSo.Api/
│   │
│   ├── AnSinhSo.Application/
│   │
│   ├── AnSinhSo.Domain/
│   │
│   ├── AnSinhSo.Infrastructure/
│   │
│   ├── AnSinhSo.Persistence/
│   │
│   ├── AnSinhSo.Shared/
│   │
│   └── AnSinhSo.Contracts/
│
├── tests/
│
│   ├── UnitTests/
│   ├── IntegrationTests/
│   └── PerformanceTests/
│
└── docs/
```

Không được tự ý thêm Project ngoài chuẩn nếu chưa được Technical Lead phê duyệt.

---

## 10.3 Project Responsibilities

| Project                 | Responsibility                                 |
| ----------------------- | ---------------------------------------------- |
| AnSinhSo.Api            | REST API, Controllers, Middleware              |
| AnSinhSo.Application    | Business Use Cases, CQRS, DTO                  |
| AnSinhSo.Domain         | Entity, Value Object, Domain Rules             |
| AnSinhSo.Infrastructure | External Services, Logging, Email, AI, Zalo OA |
| AnSinhSo.Persistence    | EF Core, DbContext, Repository                 |
| AnSinhSo.Shared         | Shared Models, Constants, Helpers              |
| AnSinhSo.Contracts      | Request/Response Contracts                     |

---

## 10.4 Dependency Rules

Phụ thuộc giữa các Project phải theo đúng thứ tự:

```text
Api
 │
 ▼
Application
 │
 ▼
Domain
 ▲
 │
Infrastructure
 │
 ▼
Persistence
```

Domain không được phụ thuộc vào bất kỳ Project nào khác.

---

## 10.5 Forbidden Dependencies

Không cho phép:

```text
Api
 ↓
Persistence
```

```text
Api
 ↓
DbContext
```

```text
Application
 ↓
SQL Server
```

```text
Domain
 ↓
Entity Framework Core
```

Các phụ thuộc trên vi phạm Clean Architecture.

---

# ============================================================================

# 11. LAYER RESPONSIBILITIES

# ============================================================================

## 11.1 API Layer

API Layer chịu trách nhiệm:

* Routing
* Authentication
* Authorization
* Model Binding
* Validation Trigger
* HTTP Response

Không xử lý nghiệp vụ.

---

## 11.2 Application Layer

Application Layer chịu trách nhiệm:

* Use Cases
* Business Workflow
* CQRS
* DTO
* Transaction Coordination
* Mapping

Không truy cập trực tiếp Database.

---

## 11.3 Domain Layer

Domain Layer là trung tâm của hệ thống.

Bao gồm:

* Entity
* Value Object
* Domain Service
* Domain Event
* Business Rule

Không phụ thuộc Framework.

---

## 11.4 Infrastructure Layer

Infrastructure chịu trách nhiệm:

* Logging
* Email
* SMS
* AI
* GIS
* Zalo OA
* Cache
* File Storage
* Third-party Integration

Không chứa nghiệp vụ.

---

## 11.5 Persistence Layer

Persistence chịu trách nhiệm:

* DbContext
* Repository
* Entity Configuration
* Migration
* Database Transaction

Không chứa Business Logic.

---

## 11.6 Shared Layer

Shared Project chứa:

* Constants
* Enumerations
* Shared Exceptions
* Common Helpers
* Base Classes

Không chứa nghiệp vụ riêng của Module.

---

# ============================================================================

# 12. PROJECT ORGANIZATION

# ============================================================================

## 12.1 Project Principles

Mỗi Project phải:

* Có một trách nhiệm duy nhất.
* Có khả năng triển khai độc lập.
* Không phụ thuộc vòng.
* Tuân thủ SOLID.

---

## 12.2 Namespace Standards

Namespace phải đồng nhất với cấu trúc thư mục.

Ví dụ:

```text
AnSinhSo.Application.Citizens.Commands

AnSinhSo.Domain.Entities

AnSinhSo.Infrastructure.Logging
```

---

## 12.3 File Size

Khuyến nghị:

* Class dưới 300 dòng.
* Method dưới 50 dòng.
* Interface dưới 200 dòng.

Nếu vượt quá cần xem xét tách nhỏ.

---

## 12.4 One Class One File

Mỗi Class nằm trong một file riêng.

Không khai báo nhiều Class nghiệp vụ trong cùng một file.

---

## 12.5 Feature-Based Organization

Ưu tiên tổ chức theo tính năng.

Ví dụ:

```text
Application

└── Citizens
    ├── Commands
    ├── Queries
    ├── DTOs
    ├── Validators
    └── Handlers
```

---

# ============================================================================

# 13. BACKEND FOLDER STRUCTURE

# ============================================================================

## 13.1 API Project

```text
Controllers/

Middlewares/

Extensions/

Configurations/

Filters/

Endpoints/

HealthChecks/

Swagger/

wwwroot/
```

---

## 13.2 Application Project

```text
Commands/

Queries/

Handlers/

Validators/

DTOs/

Interfaces/

Mappings/

Behaviors/

Services/
```

---

## 13.3 Domain Project

```text
Entities/

ValueObjects/

Events/

Exceptions/

Specifications/

Repositories/

Enums/
```

---

## 13.4 Infrastructure Project

```text
Logging/

Email/

Sms/

AI/

GIS/

ZaloOA/

Caching/

Storage/

Identity/
```

---

## 13.5 Persistence Project

```text
Configurations/

Repositories/

Contexts/

Seed/

Migrations/

Interceptors/
```

---

# ============================================================================

# 14. BACKEND MODULE STRUCTURE

# ============================================================================

## 14.1 Standard Module

Ví dụ Module Citizen.

```text
Citizen

│
├── Commands
├── Queries
├── DTOs
├── Validators
├── Handlers
├── Services
├── Interfaces
├── Specifications
└── Mapping
```

---

## 14.2 Module Independence

Mỗi Module phải độc lập.

Không được gọi trực tiếp Repository của Module khác.

Mọi giao tiếp phải thông qua:

* Application Service
* Interface
* Domain Event

---

## 14.3 Module Size

Khuyến nghị:

* Không quá 30 Commands.
* Không quá 30 Queries.
* Không quá 30 Handlers.

Nếu vượt quá phải tách Submodule.

---

## 14.4 Module Communication

Cho phép:

```text
Controller

↓

Application

↓

Domain

↓

Repository
```

Không cho phép:

```text
Controller

↓

Repository
```

---

# ============================================================================

# 15. BACKEND ARCHITECTURE CHECKLIST

# ============================================================================

Trước khi tạo Module mới cần kiểm tra:

* Đúng Solution Structure.
* Đúng Layer Responsibilities.
* Không vi phạm Dependency Rules.
* Không chứa Business Logic trong Controller.
* Không truy cập DbContext trực tiếp ngoài Persistence.
* Đúng Feature-Based Organization.
* Đúng Naming Convention.
* Có khả năng Unit Test.
* Có khả năng mở rộng.
* Tuân thủ Clean Architecture.

---

# End of Phase 2A

Phase tiếp theo:

* Domain Standards
* Entity Standards
* Value Object Standards
* Domain Events
* Domain Services
* Aggregate Root
* Repository Contracts
# ============================================================================
# 16. DOMAIN LAYER STANDARDS
# ============================================================================

## 16.1 Purpose

Domain Layer là trung tâm của toàn bộ hệ thống AnSinhSo.

Mọi quy tắc nghiệp vụ (Business Rules) phải được mô hình hóa tại tầng Domain nhằm:

- Đảm bảo tính độc lập với Framework.
- Hạn chế phụ thuộc vào Infrastructure.
- Dễ kiểm thử.
- Dễ mở rộng.
- Đảm bảo Business Logic luôn nhất quán.

Domain Layer không được phụ thuộc vào:

- Entity Framework Core
- SQL Server
- ASP.NET Core
- REST API
- Infrastructure
- Third-party Library

---

## 16.2 Domain Responsibilities

Domain chỉ bao gồm:

- Entities
- Value Objects
- Aggregate Roots
- Domain Events
- Domain Services
- Repository Interfaces
- Specifications
- Enumerations
- Domain Exceptions

Không bao gồm:

- DTO
- Controller
- Middleware
- DbContext
- Repository Implementation
- API
- Logging
- Authentication

---

## 16.3 Domain Principles

Mọi Domain Model phải tuân thủ:

- Rich Domain Model
- Persistence Ignorance
- Encapsulation
- Immutability (khi phù hợp)
- Business First

---

# ============================================================================
# 17. ENTITY STANDARDS
# ============================================================================

## 17.1 Entity Definition

Entity là đối tượng có định danh (Identity) và vòng đời riêng.

Ví dụ:

- Citizen
- Household
- Policy
- Payment
- User

---

## 17.2 Entity Rules

Entity phải:

- Có Id duy nhất.
- Có trạng thái hợp lệ.
- Bảo vệ dữ liệu nội bộ.
- Không cho phép thay đổi trạng thái trái quy tắc nghiệp vụ.

---

## 17.3 Entity Constructors

Ưu tiên sử dụng:

- Constructor đầy đủ
- Factory Method

Không khởi tạo Entity ở trạng thái không hợp lệ.

---

## 17.4 Entity Methods

Entity phải cung cấp hành vi thay vì chỉ chứa dữ liệu.

Ví dụ:

- Activate()
- Deactivate()
- UpdateInformation()
- AssignPolicy()
- RemovePolicy()

Không thao tác trực tiếp vào thuộc tính nếu có thể thông qua hành vi.

---

## 17.5 Entity Invariants

Entity phải luôn duy trì các điều kiện bất biến (Invariant).

Ví dụ:

- Công dân phải thuộc một hộ gia đình hợp lệ.
- Chính sách phải còn hiệu lực trước khi áp dụng.
- Đợt chi trả không được phát sinh sau khi đã khóa.

---

# ============================================================================
# 18. VALUE OBJECT STANDARDS
# ============================================================================

## 18.1 Definition

Value Object không có Identity.

Hai Value Object bằng nhau khi toàn bộ giá trị bằng nhau.

Ví dụ:

- Address
- FullName
- Money
- Coordinate
- PhoneNumber

---

## 18.2 Characteristics

Value Object phải:

- Immutable.
- Không có Id.
- Có khả năng so sánh theo giá trị.

---

## 18.3 Validation

Value Object phải tự kiểm tra tính hợp lệ ngay khi khởi tạo.

Không cho phép tồn tại Value Object ở trạng thái không hợp lệ.

---

# ============================================================================
# 19. AGGREGATE ROOT STANDARDS
# ============================================================================

## 19.1 Aggregate Root

Aggregate Root là điểm truy cập duy nhất vào Aggregate.

Ví dụ:

- Household
- Citizen
- PaymentBatch

---

## 19.2 Rules

Không truy cập trực tiếp Entity con từ bên ngoài Aggregate.

Mọi thay đổi phải thông qua Aggregate Root.

---

## 19.3 Transaction Boundary

Một Aggregate tương ứng với một ranh giới Transaction nghiệp vụ.

Không để Transaction trải rộng qua nhiều Aggregate nếu không cần thiết.

---

# ============================================================================
# 20. DOMAIN EVENTS
# ============================================================================

## 20.1 Purpose

Domain Event dùng để thông báo các sự kiện nghiệp vụ quan trọng.

Ví dụ:

- CitizenCreated
- HouseholdUpdated
- PaymentCompleted
- PolicyAssigned

---

## 20.2 Rules

Domain Event:

- Không chứa Business Logic.
- Chỉ chứa dữ liệu của sự kiện.
- Được xử lý bất đồng bộ khi phù hợp.

---

## 20.3 Benefits

- Giảm phụ thuộc giữa các Module.
- Tăng khả năng mở rộng.
- Hỗ trợ Event-Driven Architecture.

---

# ============================================================================
# 21. DOMAIN SERVICES
# ============================================================================

## 21.1 Purpose

Domain Service xử lý nghiệp vụ không thuộc riêng một Entity.

Ví dụ:

- EligibilityCalculationService
- BenefitCalculationService
- HouseholdClassificationService

---

## 21.2 Rules

Domain Service:

- Stateless.
- Không truy cập Infrastructure.
- Không gọi API ngoài.
- Chỉ xử lý Business Logic.

---

# ============================================================================
# 22. REPOSITORY CONTRACT STANDARDS
# ============================================================================

## 22.1 Repository Interfaces

Repository Interface phải được khai báo trong Domain Layer.

Ví dụ:

- ICitizenRepository
- IHouseholdRepository
- IUserRepository

---

## 22.2 Responsibilities

Repository Interface định nghĩa:

- Add
- Update
- Delete
- Find
- Exists
- Query

Không chứa SQL.

---

## 22.3 Implementation

Repository Implementation phải nằm trong Persistence Layer.

Domain không biết cách Repository được triển khai.

---

# ============================================================================
# 23. SPECIFICATION PATTERN
# ============================================================================

## 23.1 Purpose

Specification dùng để đóng gói điều kiện truy vấn và quy tắc lọc dữ liệu.

Ví dụ:

- ActiveCitizenSpecification
- EligibleHouseholdSpecification
- PaymentPendingSpecification

---

## 23.2 Benefits

- Tái sử dụng điều kiện truy vấn.
- Giảm lặp mã.
- Dễ kiểm thử.

---

# ============================================================================
# 24. DOMAIN EXCEPTIONS
# ============================================================================

## 24.1 Purpose

Mọi lỗi nghiệp vụ phải được biểu diễn bằng Domain Exception.

Ví dụ:

- CitizenAlreadyExistsException
- InvalidPolicyException
- PaymentLockedException

---

## 24.2 Rules

Không sử dụng Exception chung (`Exception`) để biểu diễn lỗi nghiệp vụ.

Domain Exception phải:

- Có tên rõ nghĩa.
- Mô tả đúng nguyên nhân.
- Không chứa thông tin hạ tầng.

---

# ============================================================================
# 25. DOMAIN LAYER CHECKLIST
# ============================================================================

Trước khi hoàn thành Domain Layer cần kiểm tra:

- Domain không phụ thuộc Framework.
- Không phụ thuộc EF Core.
- Không phụ thuộc SQL Server.
- Không chứa DTO.
- Không chứa Controller.
- Entity có hành vi nghiệp vụ.
- Value Object bất biến.
- Aggregate Root đúng chuẩn.
- Repository Interface nằm trong Domain.
- Domain Event đúng quy tắc.
- Domain Service không phụ thuộc Infrastructure.
- Specification được tái sử dụng.
- Domain Exception được định nghĩa đầy đủ.

---

# End of Phase 2B

Phase tiếp theo:

- Application Layer Standards
- CQRS Standards
- MediatR Standards
- DTO Standards
- Mapping Standards
- Validation Standards
- Pipeline Behaviors
# ============================================================================
# 26. APPLICATION LAYER STANDARDS
# ============================================================================

## 26.1 Purpose

Application Layer là tầng điều phối toàn bộ Use Case của hệ thống.

Application Layer chịu trách nhiệm:

- Điều phối Business Workflow.
- Gọi Domain Model.
- Điều phối Transaction.
- Gọi Repository Interface.
- Mapping DTO.
- Validation.
- Authorization (Application Level).

Application Layer không chứa:

- SQL
- DbContext
- Entity Framework Core
- HTTP Context
- Controller Logic

---

## 26.2 Responsibilities

Application Layer bao gồm:

- Commands
- Queries
- Command Handlers
- Query Handlers
- DTO
- Validators
- Interfaces
- Mapping
- Behaviors
- Services

---

## 26.3 Principles

Application Layer phải tuân thủ:

- Use Case First
- CQRS
- Dependency Inversion
- Single Responsibility
- Open/Closed Principle

---

# ============================================================================
# 27. CQRS STANDARDS
# ============================================================================

## 27.1 Purpose

Toàn bộ Use Case phải tổ chức theo mô hình CQRS.

CQRS giúp:

- Tách biệt đọc và ghi.
- Dễ mở rộng.
- Dễ kiểm thử.
- Tăng hiệu năng.

---

## 27.2 Commands

Command dùng cho:

- Create
- Update
- Delete
- Approve
- Reject
- Import
- Export (khi làm thay đổi trạng thái)

Command không trả về danh sách dữ liệu.

---

## 27.3 Queries

Query chỉ dùng để đọc dữ liệu.

Query không được:

- Insert
- Update
- Delete

---

## 27.4 Naming Convention

Command

```text
CreateCitizenCommand

UpdateCitizenCommand

DeleteCitizenCommand
```

Query

```text
GetCitizenByIdQuery

SearchCitizenQuery

GetDashboardQuery
```

---

## 27.5 Handler Naming

```text
CreateCitizenCommandHandler

UpdateCitizenCommandHandler

SearchCitizenQueryHandler
```

---

# ============================================================================
# 28. COMMAND STANDARDS
# ============================================================================

## 28.1 Command Responsibilities

Command chỉ chứa:

- Input Data
- Validation Attributes (nếu có)

Không chứa Business Logic.

---

## 28.2 Command Handler

Command Handler chịu trách nhiệm:

- Validate
- Load Entity
- Thực hiện Use Case
- Gọi Repository
- Commit Transaction

---

## 28.3 Transaction

Mỗi Command tương ứng với một Transaction nghiệp vụ.

Không chia nhỏ Transaction nếu không cần thiết.

---

# ============================================================================
# 29. QUERY STANDARDS
# ============================================================================

## 29.1 Query Responsibilities

Query chỉ phục vụ:

- Đọc dữ liệu
- Tìm kiếm
- Phân trang
- Thống kê
- Dashboard

---

## 29.2 Query Optimization

Cho phép:

- Projection
- Pagination
- Filtering
- Sorting

Không tải dữ liệu dư thừa.

---

## 29.3 Read Model

Read Model độc lập với Domain Entity khi cần tối ưu hiệu năng.

---

# ============================================================================
# 30. DTO STANDARDS
# ============================================================================

## 30.1 Purpose

DTO dùng để trao đổi dữ liệu giữa các tầng.

Không sử dụng Domain Entity trực tiếp.

---

## 30.2 DTO Types

Bao gồm:

- Request DTO
- Response DTO
- Detail DTO
- Summary DTO
- Export DTO

---

## 30.3 DTO Rules

DTO:

- Không chứa Business Logic.
- Không chứa phương thức nghiệp vụ.
- Chỉ chứa dữ liệu.

---

## 30.4 Naming

Ví dụ:

```text
CitizenDto

CitizenDetailDto

CitizenResponse

CitizenRequest
```

---

# ============================================================================
# 31. MAPPING STANDARDS
# ============================================================================

## 31.1 Mapping Purpose

Mapping chuyển đổi:

Entity

↓

DTO

hoặc

DTO

↓

Entity

---

## 31.2 Mapping Rules

Không Mapping trong Controller.

Thực hiện tại:

- Mapping Profile
- Mapper Service

---

## 31.3 AutoMapper

Cho phép sử dụng AutoMapper.

Tuy nhiên:

Mapping phức tạp phải viết thủ công.

---

# ============================================================================
# 32. VALIDATION STANDARDS
# ============================================================================

## 32.1 Validation Framework

Chuẩn sử dụng:

FluentValidation

---

## 32.2 Validation Levels

Bao gồm:

- Request Validation
- Business Validation
- Database Validation

---

## 32.3 Validation Rules

Mọi Request phải kiểm tra:

- Required
- Length
- Format
- Range
- Business Rules

---

## 32.4 Error Messages

Thông báo lỗi phải:

- Rõ ràng
- Dễ hiểu
- Không lộ thông tin hệ thống

---

# ============================================================================
# 33. PIPELINE BEHAVIORS
# ============================================================================

## 33.1 Purpose

Pipeline Behaviors xử lý logic dùng chung trước và sau Handler.

---

## 33.2 Standard Behaviors

Bao gồm:

- Validation Behavior
- Logging Behavior
- Performance Behavior
- Transaction Behavior
- Authorization Behavior

---

## 33.3 Execution Order

```text
Request

↓

Validation

↓

Authorization

↓

Logging

↓

Transaction

↓

Handler

↓

Commit

↓

Response
```

---

# ============================================================================
# 34. APPLICATION SERVICES
# ============================================================================

## 34.1 Purpose

Application Service điều phối nhiều Use Case khi cần.

Không thay thế CQRS.

---

## 34.2 Responsibilities

Application Service:

- Điều phối nhiều Handler.
- Điều phối nhiều Aggregate.
- Tích hợp nhiều Module.

---

## 34.3 Rules

Không:

- Truy cập DbContext.
- Chứa SQL.
- Chứa Controller Logic.

---

# ============================================================================
# 35. APPLICATION CHECKLIST
# ============================================================================

Trước khi hoàn thành Application Layer cần kiểm tra:

- Đúng CQRS.
- Command không đọc dữ liệu.
- Query không ghi dữ liệu.
- DTO không chứa Business Logic.
- Mapping tập trung.
- Validation dùng FluentValidation.
- Handler ngắn gọn.
- Không truy cập DbContext trực tiếp.
- Transaction đúng phạm vi.
- Pipeline Behaviors đầy đủ.
- Có khả năng Unit Test.
- Tuân thủ Clean Architecture.

---

# End of Phase 2C

Phase tiếp theo:

- Repository Pattern Standards
- Unit of Work Standards
- Entity Framework Core Standards
- DbContext Standards
- Transaction Standards
- Database Access Standards
- Performance Optimization
# ============================================================================
# 36. REPOSITORY PATTERN STANDARDS
# ============================================================================

## 36.1 Purpose

Repository Pattern là lớp trung gian giữa Domain và Persistence.

Mục tiêu:

- Tách biệt Business Logic khỏi Database.
- Dễ kiểm thử.
- Dễ thay đổi công nghệ lưu trữ.
- Tuân thủ Dependency Inversion Principle.

---

## 36.2 Responsibilities

Repository chịu trách nhiệm:

- CRUD Operations
- Query Data
- Aggregate Persistence
- Data Retrieval

Repository không chịu trách nhiệm:

- Business Logic
- Validation
- Authorization
- Logging
- Transaction Management

---

## 36.3 Repository Interfaces

Repository Interface phải được khai báo trong Domain Layer.

Ví dụ:

```
ICitizenRepository

IHouseholdRepository

IPaymentRepository

IUserRepository
```

---

## 36.4 Repository Implementation

Repository Implementation chỉ được đặt trong Persistence Layer.

Không được triển khai Repository trong:

- Controller
- Application
- Domain

---

## 36.5 Generic Repository

Chỉ sử dụng Generic Repository cho các thao tác CRUD chung.

Không ép mọi nghiệp vụ sử dụng Generic Repository nếu làm giảm tính rõ ràng của mã nguồn.

---

## 36.6 Custom Repository

Các nghiệp vụ phức tạp nên tạo Repository riêng.

Ví dụ:

```
CitizenRepository

DashboardRepository

StatisticsRepository
```

---

# ============================================================================
# 37. UNIT OF WORK STANDARDS
# ============================================================================

## 37.1 Purpose

Unit of Work quản lý toàn bộ Transaction của một Use Case.

Mục tiêu:

- Đảm bảo tính nhất quán dữ liệu.
- Gom nhiều Repository vào một Transaction.
- Hỗ trợ Rollback.

---

## 37.2 Responsibilities

Unit of Work chịu trách nhiệm:

- Begin Transaction
- Commit
- Rollback
- SaveChanges

---

## 37.3 Rules

Một Command tương ứng với một Unit of Work.

Không Commit nhiều lần trong cùng một Use Case nếu không có lý do đặc biệt.

---

## 37.4 Repository Access

Repositories phải được truy cập thông qua Unit of Work khi nghiệp vụ cần phối hợp nhiều Aggregate.

---

# ============================================================================
# 38. ENTITY FRAMEWORK CORE STANDARDS
# ============================================================================

## 38.1 ORM Standard

Chuẩn ORM của dự án:

- Entity Framework Core

Không sử dụng nhiều ORM trong cùng một hệ thống nếu không có quyết định kiến trúc.

---

## 38.2 DbContext

Mỗi Database chỉ có một DbContext chính.

Ví dụ:

```
AnSinhSoDbContext
```

---

## 38.3 Entity Configuration

Toàn bộ Entity Configuration phải tách riêng.

Không cấu hình Entity trực tiếp trong DbContext.

Ví dụ:

```
CitizenConfiguration

HouseholdConfiguration

PolicyConfiguration
```

---

## 38.4 Fluent API

Ưu tiên Fluent API.

Không lạm dụng Data Annotation.

---

## 38.5 Lazy Loading

Không sử dụng Lazy Loading trong Production.

Ưu tiên:

- Explicit Loading
- Projection
- Include có kiểm soát

---

## 38.6 Tracking

Mặc định:

Query đọc dữ liệu sử dụng:

```
AsNoTracking()
```

Chỉ bật Tracking khi cần cập nhật Entity.

---

# ============================================================================
# 39. DBCONTEXT STANDARDS
# ============================================================================

## 39.1 Responsibilities

DbContext chỉ chịu trách nhiệm:

- Mapping Entity
- Database Connection
- Change Tracking
- SaveChanges

Không chứa:

- Business Logic
- Validation
- Authorization

---

## 39.2 Naming

DbContext phải có tên:

```
AnSinhSoDbContext
```

---

## 39.3 DbSet Naming

Ví dụ:

```
DbSet<Citizen>

DbSet<Household>

DbSet<Policy>
```

Không đặt tên viết tắt.

---

## 39.4 SaveChanges

Không gọi SaveChanges trực tiếp trong Repository nếu sử dụng Unit of Work.

---

# ============================================================================
# 40. DATABASE ACCESS STANDARDS
# ============================================================================

## 40.1 Access Rules

Luồng truy cập dữ liệu:

```
Controller

↓

Application

↓

Repository

↓

DbContext

↓

SQL Server
```

Không được bỏ qua Repository.

---

## 40.2 SQL Rules

Không viết SQL trực tiếp trong:

- Controller
- Service
- Handler

Nếu cần SQL tối ưu:

- Stored Procedure
- Raw SQL
- Dapper

phải được Technical Lead phê duyệt.

---

## 40.3 Pagination

Mọi API trả danh sách lớn phải hỗ trợ:

- Page Number
- Page Size
- Total Records
- Total Pages

---

## 40.4 Filtering

Cho phép:

- Keyword
- Status
- Date Range
- Administrative Unit
- Dynamic Filter

---

## 40.5 Sorting

Cho phép:

- Ascending
- Descending

Không Hard-code thứ tự.

---

# ============================================================================
# 41. TRANSACTION STANDARDS
# ============================================================================

## 41.1 Transaction Scope

Một Transaction chỉ bao phủ đúng một Use Case.

Không kéo dài Transaction qua nhiều Request.

---

## 41.2 Rollback

Rollback bắt buộc khi:

- Validation thất bại sau khi ghi dữ liệu.
- Exception phát sinh.
- Commit thất bại.

---

## 41.3 Nested Transaction

Không sử dụng Nested Transaction nếu không thật sự cần thiết.

---

## 41.4 Distributed Transaction

Không sử dụng Distributed Transaction trong phạm vi dự án hiện tại.

Nếu phát sinh tích hợp liên hệ thống sẽ áp dụng Outbox Pattern hoặc Saga Pattern theo tài liệu kiến trúc riêng.

---

# ============================================================================
# 42. PERFORMANCE STANDARDS FOR DATA ACCESS
# ============================================================================

## 42.1 Query Optimization

Mọi truy vấn phải:

- Chỉ lấy cột cần thiết.
- Không SELECT *.
- Có Index phù hợp.
- Có Pagination nếu dữ liệu lớn.

---

## 42.2 Projection

Ưu tiên:

```
Entity

↓

DTO
```

Không tải toàn bộ Entity nếu chỉ cần vài trường dữ liệu.

---

## 42.3 Batch Processing

Các thao tác Import hoặc đồng bộ dữ liệu phải xử lý theo Batch.

Không xử lý từng bản ghi riêng lẻ nếu dữ liệu lớn.

---

## 42.4 N+1 Query

Không để xảy ra N+1 Query.

Ưu tiên:

- Include
- Projection
- Join tối ưu

---

# ============================================================================
# 43. PERSISTENCE CHECKLIST
# ============================================================================

Trước khi hoàn thành Persistence Layer cần kiểm tra:

- Repository đúng trách nhiệm.
- Interface nằm trong Domain.
- Implementation nằm trong Persistence.
- Có Unit of Work.
- Không gọi DbContext trực tiếp ngoài Persistence.
- Entity Configuration tách riêng.
- Fluent API đầy đủ.
- Query sử dụng AsNoTracking khi phù hợp.
- Có Pagination.
- Có Filtering.
- Có Sorting.
- Không có N+1 Query.
- Không có SQL trong Controller.
- Transaction đúng phạm vi.
- Rollback đầy đủ.
- Tuân thủ Clean Architecture.

---

# End of Phase 2D

Phase tiếp theo:

- Authentication Standards
- Authorization Standards
- JWT Standards
- Logging Standards
- Audit Logging
- Global Exception Handling
- Background Jobs
- Caching
- File Storage
- AI Integration
- GIS Integration
- Zalo OA Integration
- Health Checks
- Performance Monitoring
- Backend Testing
- Definition of Done
# ============================================================================
# 44. AUTHENTICATION STANDARDS
# ============================================================================

## 44.1 Authentication Framework

Hệ thống sử dụng:

- JWT Bearer Authentication
- ASP.NET Core Identity (nếu triển khai)
- Refresh Token
- HTTPS bắt buộc

---

## 44.2 Authentication Principles

Authentication phải:

- Stateless
- Secure
- Scalable
- Auditable

---

## 44.3 JWT Rules

Access Token:

- Thời gian sống ngắn
- Chứa Claims cần thiết
- Không lưu thông tin nhạy cảm

Refresh Token:

- Sinh ngẫu nhiên
- Có thời hạn
- Có thể thu hồi
- Lưu Hash nếu triển khai Production

---

## 44.4 Password Policy

Bắt buộc:

- Hash Password
- Không lưu Plain Text
- Chính sách độ dài và độ phức tạp theo cấu hình hệ thống

---

# ============================================================================
# 45. AUTHORIZATION STANDARDS
# ============================================================================

## 45.1 Authorization Model

Hệ thống sử dụng:

Role-Based Access Control (RBAC)

Các Role mặc định:

- System Administrator
- Lãnh đạo
- Cán bộ xã
- Người dân

---

## 45.2 Permission Principles

Mỗi API phải kiểm tra quyền trước khi thực hiện.

Không dựa vào Frontend để kiểm soát quyền.

---

## 45.3 Resource Authorization

Ngoài Role, các nghiệp vụ phải kiểm tra quyền theo phạm vi dữ liệu.

Ví dụ:

- Theo xã
- Theo thôn
- Theo đơn vị phụ trách
- Theo hồ sơ được phân công

---

# ============================================================================
# 46. GLOBAL EXCEPTION HANDLING
# ============================================================================

## 46.1 Centralized Exception

Toàn bộ Exception phải được xử lý tập trung.

Không xử lý lặp lại trong từng Controller.

---

## 46.2 Exception Categories

Chuẩn hóa:

- ValidationException
- DomainException
- BusinessException
- AuthenticationException
- AuthorizationException
- NotFoundException
- ConflictException
- InfrastructureException
- UnknownException

---

## 46.3 API Response

API trả lỗi phải thống nhất:

- Error Code
- Message
- Trace Id
- Timestamp

Không trả Stack Trace cho Client.

---

# ============================================================================
# 47. LOGGING STANDARDS
# ============================================================================

## 47.1 Logging Framework

Chuẩn sử dụng:

Serilog

Có thể tích hợp:

- Seq
- Elasticsearch
- Azure Monitor
- Application Insights

---

## 47.2 Structured Logging

Mọi Log phải có cấu trúc.

Ví dụ:

- UserId
- RequestId
- CorrelationId
- Module
- Action
- Duration

---

## 47.3 Log Levels

Chuẩn:

- Trace
- Debug
- Information
- Warning
- Error
- Critical

---

## 47.4 Sensitive Data

Không ghi:

- Password
- Token
- CCCD đầy đủ
- Thông tin bí mật

---

# ============================================================================
# 48. AUDIT LOG STANDARDS
# ============================================================================

## 48.1 Audit Scope

Audit ghi nhận:

- Đăng nhập
- Đăng xuất
- Tạo dữ liệu
- Cập nhật
- Xóa
- Phê duyệt
- Từ chối
- Xuất dữ liệu

---

## 48.2 Audit Information

Bao gồm:

- User
- Action
- Entity
- Entity Id
- Time
- IP
- Device (nếu có)

---

## 48.3 Immutable

Audit Log không được sửa trực tiếp.

---

# ============================================================================
# 49. BACKGROUND JOB STANDARDS
# ============================================================================

## 49.1 Purpose

Background Job xử lý:

- Gửi Zalo OA
- Gửi Email
- Đồng bộ dữ liệu
- AI Processing
- GIS Synchronization
- Report Generation

---

## 49.2 Principles

Background Job:

- Retry được
- Có Logging
- Có Monitoring
- Không ảnh hưởng Request chính

---

# ============================================================================
# 50. CACHING STANDARDS
# ============================================================================

## 50.1 Cache Strategy

Áp dụng cho:

- Lookup Data
- Dashboard
- Statistics
- GIS Layers
- Configuration

---

## 50.2 Cache Rules

Không Cache:

- Password
- Token
- Dữ liệu nhạy cảm
- Quyền truy cập

---

## 50.3 Cache Invalidation

Cache phải được làm mới khi dữ liệu thay đổi.

---

# ============================================================================
# 51. FILE STORAGE STANDARDS
# ============================================================================

## 51.1 Supported Files

Cho phép:

- PDF
- Word
- Excel
- CSV
- JPEG
- PNG

---

## 51.2 Upload Rules

Kiểm tra:

- Kích thước
- Định dạng
- Virus (nếu có giải pháp)
- Quyền truy cập

---

## 51.3 Storage

Tách biệt:

- Temporary
- Permanent
- Archive

---

# ============================================================================
# 52. GIS SERVICE STANDARDS
# ============================================================================

## 52.1 Responsibilities

GIS Service xử lý:

- Bản đồ
- Tọa độ
- Marker
- Layer
- Spatial Query

---

## 52.2 Rules

Không xử lý Business Logic.

Chỉ xử lý dữ liệu không gian.

---

# ============================================================================
# 53. AI SERVICE STANDARDS
# ============================================================================

## 53.1 Responsibilities

AI Service hỗ trợ:

- Phân tích dữ liệu
- Gợi ý
- Dự báo
- Chatbot
- Báo cáo

---

## 53.2 Principles

AI:

- Không thay thế quyết định nghiệp vụ.
- Có thể thay thế Provider.
- Có Logging.
- Có Timeout.
- Có Retry.

---

# ============================================================================
# 54. ZALO OA STANDARDS
# ============================================================================

## 54.1 Responsibilities

Quản lý:

- Template
- Broadcast
- Notification
- Token
- Retry

---

## 54.2 Error Handling

Lưu:

- Request
- Response
- Retry Count
- Status

---

# ============================================================================
# 55. HEALTH CHECK STANDARDS
# ============================================================================

## 55.1 Health Checks

Theo dõi:

- Database
- AI Service
- GIS Service
- Zalo OA
- File Storage
- Cache

---

## 55.2 Endpoint

Chuẩn:

```
/health
```

---

# ============================================================================
# 56. PERFORMANCE MONITORING
# ============================================================================

## 56.1 Metrics

Theo dõi:

- Response Time
- Throughput
- Error Rate
- CPU
- Memory
- Database Connections

---

## 56.2 Slow Query

Mọi truy vấn chậm phải được ghi Log để tối ưu.

---

# ============================================================================
# 57. BACKEND TESTING STANDARDS
# ============================================================================

## 57.1 Testing Scope

Bao gồm:

- Unit Test
- Integration Test
- API Test
- Performance Test
- Security Test

---

## 57.2 Coverage

Mục tiêu:

- Business Logic ≥ 80%
- Critical Module ≥ 90%

---

## 57.3 Test Principles

Mỗi Use Case quan trọng phải có:

- Happy Path
- Validation Test
- Exception Test
- Authorization Test

---

# ============================================================================
# 58. BACKEND SECURITY CHECKLIST
# ============================================================================

Trước khi phát hành cần kiểm tra:

- JWT hoạt động.
- RBAC đúng.
- Không lộ Stack Trace.
- Không Hard-code Secret.
- HTTPS bắt buộc.
- Input Validation đầy đủ.
- SQL Injection Prevention.
- XSS Prevention.
- CSRF (nếu áp dụng).
- Audit Log hoạt động.

---

# ============================================================================
# 59. DEFINITION OF DONE
# ============================================================================

Một chức năng Backend được xem là hoàn thành khi:

- Đúng nghiệp vụ.
- Đúng Clean Architecture.
- Đúng CQRS.
- Có Validation.
- Có Authorization.
- Có Logging.
- Có Audit.
- Có Unit Test.
- Có API Documentation.
- Có Error Handling.
- Có Transaction.
- Đạt Code Review.
- Không còn Technical Debt nghiêm trọng.

---

# ============================================================================
# 60. BACKEND CODE REVIEW CHECKLIST
# ============================================================================

Reviewer phải kiểm tra:

- Naming Convention.
- Layer Separation.
- Dependency Rules.
- Business Logic.
- Repository Pattern.
- Unit of Work.
- Transaction.
- Validation.
- Security.
- Logging.
- Audit.
- Performance.
- Testing.
- Clean Code.
- SOLID Principles.
- Không còn Dead Code.
- Không còn TODO/FIXME trước khi phát hành.

---

# ============================================================================
# 61. AI CODING RULES (BACKEND)
# ============================================================================

Mọi AI Coding Assistant phải:

- Đọc 00_PROJECT_BOOTSTRAP.md trước.
- Đọc 00_PROJECT_PROGRESS.md để xác định Sprint.
- Tuân thủ Clean Architecture.
- Không sửa kiến trúc đã thống nhất.
- Không sinh Business Logic trong Controller.
- Không Hard-code dữ liệu.
- Không truy cập DbContext ngoài Persistence.
- Sinh mã có khả năng Unit Test.
- Tuân thủ toàn bộ Backend Standards.
- Tự kiểm tra Definition of Done trước khi hoàn thành.

Áp dụng cho:

- ChatGPT
- GitHub Copilot
- Cursor
- Cline
- Continue
- Gemini
- AntiGravity AI

---

# ============================================================================
# 62. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------------|--------------------------------|
| 1.0.0 | 2026-07-12 | Initial Enterprise Backend Standards |

---

# ============================================================================
# 63. RELATED DOCUMENTS
# ============================================================================

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_PROJECT_INDEX.md
- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 07_FRONTEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md
- CLEAN_ARCHITECTURE.md
- DATABASE_DESIGN.md
- API_SPEC.md
- BUSINESS_RULES.md

---

# END OF DOCUMENT