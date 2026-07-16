```md
# =============================================================================
# 21_INFRASTRUCTURE_ARCHITECTURE.md
# Enterprise Infrastructure Reference Architecture
#
# Project:
# AnSinhSo – Digital Social Welfare System
#
# Version:
# 1.0
#
# Status:
# Enterprise Design
# =============================================================================

---

# PHASE 1 – INFRASTRUCTURE OVERVIEW

---

## 1.1 Purpose

Tài liệu này mô tả kiến trúc hạ tầng tổng thể của hệ thống AnSinhSo.

Mục tiêu là chuẩn hóa cách tổ chức mã nguồn, môi trường chạy, dịch vụ nền, cấu hình và triển khai để bảo đảm hệ thống có khả năng mở rộng, bảo trì và vận hành ổn định.

Tài liệu là cơ sở để:

- Triển khai Solution ASP.NET Core 8
- Chuẩn hóa Clean Architecture
- Thiết kế hạ tầng Production
- Hướng dẫn Antygravity AI sinh mã nguồn
- Hướng dẫn DevOps và triển khai

---

## 1.2 Infrastructure Objectives

Hệ thống phải đáp ứng:

✓ Modular

✓ Maintainable

✓ Scalable

✓ Secure

✓ Observable

✓ Production Ready

✓ Cloud Ready

✓ On-Premise Ready

---

## 1.3 Design Principles

Infrastructure được xây dựng theo các nguyên tắc:

- Clean Architecture
- SOLID
- Dependency Inversion
- Configuration as Code
- Stateless Services
- API First
- Event Driven
- Fail Fast
- Health Check by Default
- Logging by Default

---

## 1.4 Infrastructure Layers

Presentation Layer

↓

Application Layer

↓

Domain Layer

↓

Infrastructure Layer

↓

External Services

↓

Database

---

## 1.5 Runtime Components

Bao gồm:

- ASP.NET Core Web API
- SQL Server
- Worker Services
- Messaging Gateway
- Notification Center
- AI Integration Hub
- GIS Integration
- Zalo OA Integration

---

## 1.6 Deployment Targets

Hệ thống hỗ trợ:

Development

Testing

Staging

Production

Có thể triển khai:

- Máy chủ Windows với IIS
- Máy chủ Linux (nếu mở rộng trong tương lai)
- Môi trường nội bộ của UBND xã
- Hạ tầng đám mây khi cần

---

## 1.7 Environment Strategy

Mỗi môi trường có:

- Cấu hình riêng
- Chuỗi kết nối riêng
- Secret riêng
- Logging riêng
- Monitoring riêng

Không sử dụng cấu hình Production cho môi trường Development.

---

## 1.8 Infrastructure Standards

Framework

ASP.NET Core 8

ORM

Entity Framework Core

Database

SQL Server

Authentication

JWT

Architecture

Clean Architecture

Logging

Serilog (hoặc giải pháp tương đương)

Health Check

ASP.NET Core Health Checks

---

## 1.9 Non-functional Requirements

Khả năng mở rộng

Khả năng giám sát

Khả năng bảo trì

Khả năng phục hồi

Khả năng kiểm thử

Hiệu năng ổn định

---

## 1.10 Expected Deliverables

Sau Phase 1:

✓ Infrastructure Vision

✓ Design Principles

✓ Runtime Overview

✓ Deployment Targets

✓ Infrastructure Standards

# =============================================================================
# END OF PHASE 1
# =============================================================================
```
```md
# =============================================================================
# PHASE 2 – ENTERPRISE SOLUTION STRUCTURE
# =============================================================================

---

## 2.1 Design Objectives

Solution của AnSinhSo phải:

- Dễ bảo trì.
- Dễ mở rộng.
- Tách biệt trách nhiệm.
- Hỗ trợ kiểm thử.
- Không phụ thuộc trực tiếp vào hạ tầng.
- Có thể phát triển theo từng Sprint.

---

## 2.2 Solution Layout

AnSinhSo/

├── src/

├── tests/

├── docs/

├── deployment/

├── scripts/

├── tools/

├── samples/

├── README.md

├── LICENSE

└── .gitignore

---

## 2.3 Source Structure

src/

├── AnSinhSo.Domain

├── AnSinhSo.Application

├── AnSinhSo.Infrastructure

├── AnSinhSo.WebApi

├── AnSinhSo.Worker

├── AnSinhSo.Contracts

├── AnSinhSo.SharedKernel

└── AnSinhSo.Host

---

## 2.4 Domain Project

Chứa:

- Entities
- Value Objects
- Domain Events
- Aggregates
- Domain Services
- Repository Interfaces
- Specifications
- Business Rules

**Không được tham chiếu đến:**

- Entity Framework Core
- SQL Server
- ASP.NET Core
- HTTP
- Logging
- Zalo OA
- AI Provider

---

## 2.5 Application Project

Chứa:

- Use Cases
- Commands
- Queries
- DTO
- Validators
- Interfaces
- Mapping
- Authorization Policies
- Application Services

Application chỉ phụ thuộc vào Domain.

---

## 2.6 Infrastructure Project

Chứa:

- Entity Framework Core
- SQL Server
- Repository Implementations
- Background Services
- AI Provider
- Zalo OA Provider
- GIS Provider
- Email/SMS Provider (nếu bổ sung)
- Logging
- File Storage

Infrastructure triển khai các interface do Application hoặc Domain định nghĩa.

---

## 2.7 WebApi Project

Bao gồm:

- Controllers
- Middleware
- Authentication
- Authorization
- Swagger
- Health Checks
- Dependency Injection
- Exception Handling

WebApi không chứa Business Logic.

---

## 2.8 Worker Project

Bao gồm:

- Scheduled Jobs
- Queue Processing
- Notification Delivery
- AI Background Tasks
- Retry Processing
- Cleanup Jobs

Worker dùng chung Application Layer, không triển khai nghiệp vụ riêng.

---

## 2.9 Contracts Project

Chứa:

- API Contracts
- Shared DTO
- Request Models
- Response Models
- Event Contracts

Contracts không phụ thuộc Infrastructure.

---

## 2.10 SharedKernel Project

Chứa các thành phần dùng chung:

- Result Pattern
- Base Entity
- Base Auditable Entity
- Error Definitions
- Constants
- Enumerations
- Pagination
- Common Extensions

---

## 2.11 Host Project

Là điểm khởi động của hệ thống.

Nhiệm vụ:

- Nạp cấu hình.
- Đăng ký Dependency Injection.
- Khởi tạo Logging.
- Khởi tạo Monitoring.
- Khởi tạo Health Checks.
- Khởi tạo Middleware.

---

## 2.12 Dependency Rules

Được phép:

WebApi → Application

Worker → Application

Infrastructure → Domain

Infrastructure → Application

Application → Domain

Không được phép:

Domain → Infrastructure

Domain → WebApi

Application → WebApi

Application → SQL Server

Domain → Entity Framework Core

---

## 2.13 Build Order

1. SharedKernel
2. Domain
3. Application
4. Infrastructure
5. Contracts
6. WebApi
7. Worker
8. Host

---

## 2.14 Folder Standards

Mỗi Project phải có cấu trúc thống nhất:

- Abstractions
- Configuration
- Constants
- Entities
- Interfaces
- Services
- Extensions
- Options
- Validators
- Exceptions

Không tạo cấu trúc tùy ý giữa các Sprint.

---

## 2.15 Expected Deliverables

Sau Phase 2:

✓ Enterprise Solution Layout

✓ Project Boundaries

✓ Dependency Rules

✓ Folder Standards

✓ Build Order

# =============================================================================
# END OF PHASE 2
# =============================================================================
```
```md id="ansinhso-infra-phase3"
# =============================================================================
# PHASE 3 – CLEAN ARCHITECTURE & DEPENDENCY RULES
# =============================================================================

---

# 3.1 Overview

AnSinhSo áp dụng mô hình **Clean Architecture** nhằm:

- Tách biệt nghiệp vụ và hạ tầng.
- Giảm phụ thuộc giữa các tầng.
- Dễ kiểm thử.
- Dễ bảo trì.
- Dễ mở rộng.
- Cho phép thay thế công nghệ mà không ảnh hưởng Business Logic.

Nguyên tắc cốt lõi:

> Business không phụ thuộc Framework.

---

# 3.2 Architectural Layers

Core

↓

Application

↓

Infrastructure

↓

Presentation

↓

External Systems

---

## Domain Layer

Chứa:

- Entities
- Value Objects
- Aggregates
- Domain Events
- Domain Services
- Repository Interfaces
- Specifications

Không được phụ thuộc:

- ASP.NET Core
- Entity Framework Core
- SQL Server
- HTTP
- Logging
- AI Provider
- Zalo OA

---

## Application Layer

Chứa:

- Use Cases
- Commands
- Queries
- DTO
- Validators
- Authorization Policies
- Interfaces
- Mapping
- Pipeline Behaviors

Application chỉ phụ thuộc Domain.

---

## Infrastructure Layer

Triển khai:

- Repository
- EF Core
- SQL Server
- AI Provider
- Zalo Provider
- GIS Provider
- File Storage
- Cache
- Email
- Logging

Infrastructure triển khai interface, không chứa Business Rules.

---

## Presentation Layer

Bao gồm:

- Web API
- Swagger
- Middleware
- Authentication
- Authorization
- Health Checks

Presentation chỉ gọi Application.

---

# 3.3 Dependency Rule

Nguyên tắc bắt buộc:

Dependency luôn hướng vào Core.

Presentation
      ↓
Application
      ↓
Domain

Infrastructure
      ↓
Application
      ↓
Domain

Domain không tham chiếu bất kỳ project nào khác.

---

# 3.4 Dependency Matrix

| Source | Target | Allowed |
|---------|--------|---------|
| Domain | Domain | ✅ |
| Domain | Application | ❌ |
| Domain | Infrastructure | ❌ |
| Domain | WebApi | ❌ |
| Application | Domain | ✅ |
| Application | Infrastructure | ❌ |
| Application | WebApi | ❌ |
| Infrastructure | Application | ✅ |
| Infrastructure | Domain | ✅ |
| WebApi | Application | ✅ |
| Worker | Application | ✅ |

---

# 3.5 Dependency Injection

Tất cả dịch vụ phải được đăng ký thông qua DI Container.

Không khởi tạo trực tiếp:

- Repository
- Provider
- Logger
- HTTP Client

Sử dụng constructor injection.

---

# 3.6 Cross-Cutting Concerns

Các chức năng dùng chung phải được tách riêng:

- Logging
- Validation
- Authentication
- Authorization
- Caching
- Audit
- Monitoring
- Exception Handling

Không lặp lại mã ở nhiều module.

---

# 3.7 Result Pattern

Application trả về Result hoặc Result<T> thống nhất.

Không sử dụng Exception để điều khiển luồng nghiệp vụ thông thường.

Exception chỉ dành cho lỗi bất thường.

---

# 3.8 Validation Strategy

Validation được thực hiện trước khi xử lý nghiệp vụ.

Các lớp Validation độc lập với Controller.

Business Rules vẫn phải được kiểm tra trong Domain/Application khi cần.

---

# 3.9 Repository Pattern

Repository chỉ chịu trách nhiệm:

- Truy xuất dữ liệu.
- Lưu dữ liệu.
- Xóa dữ liệu.

Repository không chứa Business Logic.

---

# 3.10 Unit of Work

Nếu sử dụng Unit of Work:

- Quản lý Transaction.
- Commit/Rollback.
- Đảm bảo tính nhất quán dữ liệu.

---

# 3.11 CQRS

Các chức năng nghiệp vụ áp dụng:

Commands

↓

Write Model

Queries

↓

Read Model

CQRS được áp dụng ở mức Application Layer, không tách thành hai hệ thống độc lập.

---

# 3.12 Error Handling

Sử dụng Global Exception Middleware.

Chuẩn hóa:

- Error Code
- Error Message
- Trace Id
- Correlation Id

Không trả Stack Trace trong Production.

---

# 3.13 Coding Principles

Áp dụng:

- SOLID
- DRY
- KISS
- YAGNI
- Explicit Dependencies
- Fail Fast

---

# 3.14 Architecture Validation

Mỗi Sprint phải kiểm tra:

- Dependency Rules
- Layer Boundaries
- Build Success
- Test Success

Không chấp nhận vi phạm kiến trúc để đổi lấy tốc độ phát triển.

---

# 3.15 Expected Deliverables

Sau Phase 3:

✓ Clean Architecture Baseline

✓ Dependency Rules

✓ CQRS Guidelines

✓ Repository Standards

✓ Validation Strategy

✓ Error Handling Standards

# =============================================================================
# END OF PHASE 3
# =============================================================================
```
# =============================================================================

# PHASE 4 – APPROVED TECHNOLOGY STACK & CODING STANDARDS

# =============================================================================

---

# 4.1 Overview

Để đảm bảo tính nhất quán trong toàn bộ vòng đời phát triển, dự án AnSinhSo sử dụng một bộ công nghệ và quy chuẩn lập trình được phê duyệt trước.

Mọi Sprint, mọi module và mọi thành viên phát triển phải tuân thủ tài liệu này.

Không tự ý thay đổi framework hoặc thư viện nếu chưa được phê duyệt.

---

# 4.2 Approved Technology Stack

| Thành phần           | Công nghệ                             |
| -------------------- | ------------------------------------- |
| Framework            | ASP.NET Core 8                        |
| Runtime              | .NET 8 LTS                            |
| Language             | C# 12                                 |
| Architecture         | Clean Architecture + Modular Monolith |
| ORM                  | Entity Framework Core 8               |
| Database             | SQL Server 2022                       |
| Authentication       | JWT + Refresh Token                   |
| Authorization        | Policy-Based Authorization + RBAC     |
| Validation           | FluentValidation                      |
| Logging              | Serilog                               |
| API Documentation    | Swagger / OpenAPI                     |
| Health Check         | ASP.NET Core Health Checks            |
| Background Jobs      | .NET BackgroundService                |
| Configuration        | Microsoft.Extensions.Configuration    |
| Dependency Injection | Microsoft DI                          |
| Testing              | xUnit + FluentAssertions              |
| Source Control       | Git                                   |
| Package Manager      | NuGet                                 |

---

# 4.3 Mapping Strategy

Để giảm phụ thuộc và dễ kiểm soát hiệu năng:

* DTO đơn giản: ánh xạ thủ công (manual mapping).
* Trường hợp có nhiều model lặp lại: có thể sử dụng AutoMapper nếu được phê duyệt.

Không lạm dụng thư viện ánh xạ cho các trường hợp đơn giản.

---

# 4.4 Logging Standards

Sử dụng Serilog.

Yêu cầu:

* Structured Logging.
* CorrelationId.
* RequestId.
* EventId.
* Không ghi mật khẩu, Access Token hoặc Secret vào log.

---

# 4.5 Validation Standards

Toàn bộ Request Model sử dụng FluentValidation.

Nguyên tắc:

* Validate dữ liệu đầu vào.
* Business Rule vẫn kiểm tra tại Application/Domain.
* Không đặt logic kiểm tra nghiệp vụ trong Controller.

---

# 4.6 Configuration Standards

Cấu hình được phân tầng:

* appsettings.json
* appsettings.Development.json
* appsettings.Staging.json
* appsettings.Production.json
* Environment Variables
* Secret Store (Production)

Không ghi thông tin nhạy cảm vào repository.

---

# 4.7 API Standards

Định dạng:

/api/v1/{resource}

Quy ước:

* RESTful.
* JSON.
* Camel Case.
* Chuẩn mã trạng thái HTTP.
* Response thống nhất.

---

# 4.8 Error Response Standards

Mọi API trả lỗi theo định dạng thống nhất:

* ErrorCode
* Message
* TraceId
* CorrelationId
* Timestamp

Không trả Stack Trace trong Production.

---

# 4.9 Naming Conventions

Solution

AnSinhSo

Projects

AnSinhSo.Domain

AnSinhSo.Application

AnSinhSo.Infrastructure

AnSinhSo.WebApi

AnSinhSo.Worker

AnSinhSo.Contracts

AnSinhSo.SharedKernel

Namespaces phải khớp với tên Project.

---

# 4.10 Folder Conventions

Mỗi project sử dụng cấu trúc thống nhất:

* Entities
* Interfaces
* Services
* Configuration
* Validators
* Exceptions
* Extensions
* Options
* Mappings
* Constants

Không tự ý tạo cấu trúc khác khi chưa có thống nhất.

---

# 4.11 Code Quality Standards

Áp dụng:

* SOLID
* DRY
* KISS
* YAGNI
* Explicit Dependencies
* Fail Fast

Không chấp nhận mã nguồn có cảnh báo nghiêm trọng hoặc vi phạm quy tắc kiến trúc.

---

# 4.12 Documentation Standards

Mỗi module phải có:

* README.md
* XML Documentation (cho API công khai khi cần)
* Changelog (khi phát hành)

Tài liệu phải được cập nhật khi thay đổi hành vi hoặc kiến trúc.

---

# 4.13 Review Standards

Mỗi Sprint cần kiểm tra:

* Build thành công.
* Unit Test đạt.
* Không vi phạm Dependency Rules.
* Tuân thủ Coding Standards.
* Tuân thủ Security Standards.

---

# 4.14 Expected Deliverables

Sau Phase 4:

✓ Technology Stack được khóa

✓ Coding Standards

✓ API Standards

✓ Validation Standards

✓ Logging Standards

✓ Naming Conventions

✓ Review Checklist

# =============================================================================

# END OF PHASE 4

# =============================================================================
```md id="ansinhso-infra-phase5"
# =============================================================================
# PHASE 5 – DEVELOPMENT CONVENTIONS
# =============================================================================

---

# 5.1 Purpose

Tài liệu này quy định các quy ước phát triển nhằm đảm bảo:

- Mã nguồn đồng nhất.
- Dễ đọc.
- Dễ review.
- Dễ bảo trì.
- Phù hợp với Clean Architecture.
- Phù hợp với Enterprise Coding Standards.

Mọi Sprint phải tuân thủ các quy ước dưới đây.

---

# 5.2 Naming Conventions

## Projects

AnSinhSo.Domain

AnSinhSo.Application

AnSinhSo.Infrastructure

AnSinhSo.WebApi

AnSinhSo.Worker

AnSinhSo.Contracts

AnSinhSo.SharedKernel

---

## Interfaces

Bắt đầu bằng:

I

Ví dụ:

IUserRepository

INotificationService

IAIProvider

---

## Services

Kết thúc bằng:

Service

Ví dụ:

CitizenService

HouseholdService

PaymentService

---

## Repository

Kết thúc bằng:

Repository

Ví dụ:

CitizenRepository

PolicyRepository

---

## Controllers

Kết thúc bằng:

Controller

Ví dụ:

CitizenController

DashboardController

---

# 5.3 DTO Naming

Command

CreateCitizenCommand

UpdateCitizenCommand

DeleteCitizenCommand

Query

GetCitizenQuery

SearchCitizenQuery

Response

CitizenResponse

DashboardResponse

SummaryResponse

---

# 5.4 Folder Standards

Mỗi module phải có cấu trúc thống nhất:

Entities/

DTOs/

Commands/

Queries/

Validators/

Mappings/

Interfaces/

Services/

Repositories/

Configurations/

Exceptions/

---

# 5.5 Dependency Injection

Mọi service phải đăng ký qua DI.

Không sử dụng:

new Repository()

new Service()

new HttpClient()

Thay vào đó sử dụng constructor injection hoặc IHttpClientFactory.

---

# 5.6 Exception Handling

Không bắt Exception tại Controller nếu chỉ để trả lỗi chung.

Sử dụng Global Exception Middleware.

Chuẩn hóa:

- ErrorCode
- Message
- TraceId
- CorrelationId

---

# 5.7 Result Pattern

Application Layer trả về:

Result

Result<T>

Thay vì:

bool

string

Exception cho các trường hợp nghiệp vụ thông thường.

---

# 5.8 Logging Convention

Mọi log phải bao gồm:

- Timestamp
- EventId
- UserId (nếu có)
- CorrelationId
- Module
- Action

Không ghi:

- Password
- Access Token
- Refresh Token
- Secret
- Dữ liệu nhạy cảm không cần thiết

---

# 5.9 Configuration Convention

Mỗi module có lớp Options riêng.

Ví dụ:

JwtOptions

DatabaseOptions

ZaloOptions

AIOptions

Các lớp Options được bind từ cấu hình và đăng ký thông qua Dependency Injection.

---

# 5.10 Validation Convention

Validation được thực hiện bằng FluentValidation.

Controller không chứa logic kiểm tra dữ liệu.

Business Rules vẫn được xác minh tại Application hoặc Domain khi cần.

---

# 5.11 API Convention

Chuẩn đường dẫn:

/api/v1/{resource}

HTTP Methods:

GET

POST

PUT

PATCH

DELETE

Response sử dụng định dạng thống nhất của hệ thống.

---

# 5.12 Commenting Convention

- Chỉ comment khi cần giải thích quyết định kỹ thuật hoặc nghiệp vụ phức tạp.
- Tránh comment mô tả lại điều mà tên hàm hoặc tên biến đã thể hiện rõ.
- Tài liệu XML chỉ áp dụng cho API công khai hoặc thư viện dùng chung.

---

# 5.13 Git Convention

Định dạng Commit đề xuất:

feat:

fix:

refactor:

docs:

test:

chore:

Ví dụ:

feat: add household management module

fix: resolve JWT refresh token validation

docs: update AI development guide

---

# 5.14 Code Review Checklist

Mỗi Pull Request hoặc Sprint Review cần xác nhận:

□ Build thành công

□ Không vi phạm Clean Architecture

□ Không vi phạm Dependency Rules

□ Đạt Unit Test

□ Đúng Business Rules

□ Đúng Security Standards

□ Đúng Coding Standards

□ Không có TODO hoặc mã thử nghiệm để lại

---

# 5.15 Expected Deliverables

Sau Phase 5:

✓ Development Convention

✓ Coding Convention

✓ Naming Convention

✓ Git Convention

✓ Review Checklist

✓ Technical Baseline được thống nhất

# =============================================================================
# END OF PHASE 5
# =============================================================================
```
```md id="ansinhso-infra-phase6"
# =============================================================================
# PHASE 6 – CONFIGURATION & ENVIRONMENT MANAGEMENT
# =============================================================================

---

# 6.1 Purpose

Thiết lập chiến lược quản lý cấu hình thống nhất cho toàn bộ hệ thống AnSinhSo.

Mục tiêu:

- Tách biệt cấu hình và mã nguồn.
- Hỗ trợ nhiều môi trường triển khai.
- Đảm bảo an toàn thông tin.
- Dễ dàng thay đổi mà không cần biên dịch lại ứng dụng.

---

# 6.2 Environment Strategy

Hệ thống sử dụng các môi trường:

Development

↓

Testing

↓

Staging

↓

Production

Mỗi môi trường có:

- Connection String riêng.
- JWT Secret riêng.
- API Key riêng.
- Logging Level riêng.
- Monitoring riêng.
- Feature Flags (nếu áp dụng).

---

# 6.3 Configuration Hierarchy

Thứ tự ưu tiên:

1. Environment Variables
2. Secret Store / Key Vault (Production)
3. appsettings.{Environment}.json
4. appsettings.json

Không lưu thông tin nhạy cảm trong mã nguồn.

---

# 6.4 Strongly Typed Configuration

Mỗi nhóm cấu hình được ánh xạ vào một lớp Options.

Ví dụ:

- JwtOptions
- DatabaseOptions
- RedisOptions
- ZaloOptions
- AIOptions
- GisOptions
- EmailOptions
- StorageOptions

Đăng ký thông qua Dependency Injection.

---

# 6.5 Feature Flags

Cho phép bật/tắt các chức năng theo môi trường.

Ví dụ:

- EnableAI
- EnableGIS
- EnableZaloOA
- EnableBackgroundJobs
- EnableDetailedLogging

Không cần thay đổi mã nguồn khi bật/tắt các tính năng này.

---

# 6.6 Secret Management Integration

Production:

- Environment Variables
- Secret Store
- Key Vault (khi triển khai)

Không commit Secret lên Git.

Không chia sẻ Secret qua email hoặc công cụ chat.

---

# 6.7 Logging Configuration

Cấu hình theo môi trường:

Development

- Verbose
- Debug

Testing

- Information

Production

- Warning
- Error
- Critical

Giảm log không cần thiết trên Production để tối ưu hiệu năng và bảo mật.

---

# 6.8 Health Check Configuration

Định nghĩa các Health Check có thể cấu hình:

- Database
- Background Worker
- AI Provider
- Zalo OA
- GIS Service
- File Storage

Cho phép bật/tắt từng Health Check theo nhu cầu.

---

# 6.9 Configuration Validation

Khi khởi động ứng dụng:

- Kiểm tra các cấu hình bắt buộc.
- Kiểm tra định dạng.
- Kiểm tra giá trị hợp lệ.
- Ghi log và dừng khởi động nếu cấu hình quan trọng bị thiếu.

---

# 6.10 Environment Variables

Ví dụ:

ASPNETCORE_ENVIRONMENT

ConnectionStrings__DefaultConnection

Jwt__Issuer

Jwt__Audience

Jwt__Secret

AI__ApiKey

Zalo__AccessToken

Storage__RootPath

---

# 6.11 Deployment Profiles

Chuẩn bị các profile:

- Local Development
- Test Server
- Production Server

Mỗi profile có tập cấu hình riêng và được quản lý độc lập.

---

# 6.12 Configuration Review Checklist

Trước khi triển khai:

□ Không còn Secret trong mã nguồn.

□ Environment Variables đầy đủ.

□ Feature Flags đúng.

□ Logging Level đúng.

□ Health Check hoạt động.

□ Các lớp Options bind thành công.

---

# 6.13 Expected Deliverables

Sau Phase 6:

✓ Environment Strategy

✓ Configuration Standards

✓ Options Pattern

✓ Feature Flags

✓ Secret Integration

✓ Startup Validation

# =============================================================================
# END OF PHASE 6
# =============================================================================
```
```md id="ansinhso-infra-phase7"
# =============================================================================
# PHASE 7 – DATA INFRASTRUCTURE
# =============================================================================

---

# 7.1 Purpose

Data Infrastructure định nghĩa kiến trúc lưu trữ dữ liệu của hệ thống AnSinhSo, bao gồm:

- SQL Server
- Entity Framework Core
- Database Migration
- Transaction Management
- Backup & Recovery
- Data Access Standards

Mục tiêu là đảm bảo dữ liệu nhất quán, an toàn và dễ mở rộng.

---

# 7.2 Database Architecture

Hệ thống sử dụng:

- Database Engine: SQL Server 2022
- ORM: Entity Framework Core 8
- Migration: EF Core Migrations
- Transaction: Unit of Work (khi phù hợp)

Mỗi môi trường (Development, Testing, Staging, Production) sử dụng cơ sở dữ liệu độc lập.

---

# 7.3 Data Access Principles

Tuân thủ các nguyên tắc:

- Chỉ Infrastructure được truy cập trực tiếp cơ sở dữ liệu.
- Application truy cập dữ liệu thông qua Repository hoặc Query Service.
- Domain không phụ thuộc Entity Framework Core.
- Không viết SQL trực tiếp trong Controller.

---

# 7.4 Entity Framework Core Standards

Quy định:

- Sử dụng Fluent API để cấu hình mô hình dữ liệu.
- Hạn chế Data Annotations khi cấu hình phức tạp.
- Migration được quản lý tập trung.
- Không chỉnh sửa Migration đã áp dụng trên Production.

---

# 7.5 Repository Standards

Repository chịu trách nhiệm:

- Truy vấn dữ liệu.
- Thêm, cập nhật, xóa dữ liệu.
- Không chứa Business Rules.
- Không trả về DTO.

Repository trả về Entity hoặc Aggregate phù hợp với Domain.

---

# 7.6 Query Strategy

Áp dụng CQRS ở mức Application:

- Query tối ưu cho đọc.
- Command tối ưu cho ghi.

Các truy vấn thống kê lớn có thể sử dụng View hoặc Stored Procedure nếu cần tối ưu hiệu năng.

---

# 7.7 Transaction Management

Transaction được quản lý tại Application Service hoặc Unit of Work.

Nguyên tắc:

- Chỉ mở Transaction khi cần.
- Giữ Transaction ngắn.
- Rollback khi xảy ra lỗi.
- Không lồng nhiều Transaction không cần thiết.

---

# 7.8 Migration Strategy

Migration được tạo và quản lý bằng EF Core.

Quy trình:

1. Thay đổi mô hình dữ liệu.
2. Tạo Migration.
3. Review Migration.
4. Áp dụng trên Development.
5. Kiểm thử.
6. Áp dụng Staging.
7. Áp dụng Production theo kế hoạch triển khai.

---

# 7.9 Indexing Strategy

Thiết kế chỉ mục dựa trên:

- Khóa chính.
- Khóa ngoại.
- Các trường tìm kiếm thường xuyên.
- Các trường lọc và sắp xếp.

Định kỳ rà soát hiệu năng truy vấn và điều chỉnh chỉ mục khi cần.

---

# 7.10 Backup & Restore

Chính sách:

- Sao lưu định kỳ.
- Kiểm tra khả năng khôi phục.
- Mã hóa bản sao lưu khi phù hợp.
- Lưu trữ theo chính sách của đơn vị quản lý.

Khôi phục phải được kiểm thử trước khi sử dụng trong Production.

---

# 7.11 Data Integrity

Đảm bảo:

- Khóa chính và khóa ngoại đầy đủ.
- Ràng buộc dữ liệu phù hợp.
- Không xóa dữ liệu làm mất tính nhất quán.
- Áp dụng Soft Delete khi phù hợp với nghiệp vụ.

---

# 7.12 Performance Guidelines

- Sử dụng AsNoTracking() cho truy vấn chỉ đọc.
- Phân trang cho danh sách lớn.
- Tránh N+1 Query.
- Chỉ Include dữ liệu cần thiết.
- Theo dõi truy vấn chậm để tối ưu.

---

# 7.13 Monitoring

Theo dõi:

- Thời gian truy vấn.
- Số lượng kết nối.
- Tỷ lệ lỗi.
- Deadlock (nếu có).
- Tăng trưởng dữ liệu.

---

# 7.14 Data Infrastructure Checklist

□ Migration được review.

□ Backup hoạt động.

□ Restore đã kiểm thử.

□ Index được tối ưu.

□ Repository tuân thủ chuẩn.

□ Không có Business Logic trong Repository.

---

# 7.15 Expected Deliverables

Sau Phase 7:

✓ Data Architecture

✓ EF Core Standards

✓ Repository Standards

✓ Migration Strategy

✓ Transaction Strategy

✓ Backup & Restore Policy

✓ Performance Guidelines

# =============================================================================
# END OF PHASE 7
# =============================================================================
```
```md id="ansinhso-infra-phase8"
# =============================================================================
# PHASE 8 – CACHING INFRASTRUCTURE
# =============================================================================

---

# 8.1 Purpose

Caching Infrastructure được thiết kế nhằm:

- Giảm tải Database.
- Tăng tốc độ phản hồi.
- Giảm số lượng truy vấn lặp lại.
- Hỗ trợ mở rộng hệ thống.
- Cải thiện trải nghiệm người dùng.

Caching là thành phần hỗ trợ hiệu năng, không thay thế nguồn dữ liệu chính.

---

# 8.2 Caching Strategy

Áp dụng chiến lược nhiều lớp:

Client Cache

↓

API Response Cache

↓

Application Cache

↓

Distributed Cache (Redis)

↓

SQL Server

Trong giai đoạn MVP có thể sử dụng `IMemoryCache`. Khi triển khai Production hoặc mở rộng nhiều máy chủ, chuyển sang Redis mà không thay đổi Business Logic.

---

# 8.3 Cache Principles

Nguyên tắc:

- Cache Aside Pattern.
- Không lưu dữ liệu nhạy cảm trong cache nếu không cần thiết.
- Thiết lập thời gian sống (TTL) phù hợp.
- Xóa hoặc làm mới cache khi dữ liệu nguồn thay đổi.
- Cache phải có khả năng vô hiệu hóa theo cấu hình.

---

# 8.4 Cache Categories

Các nhóm dữ liệu phù hợp để cache:

- Danh mục địa bàn.
- Danh mục nhóm đối tượng.
- Danh mục chính sách.
- Cấu hình hệ thống.
- Quyền và vai trò ít thay đổi.
- Thông tin Dashboard tổng hợp.

Không cache:

- JWT.
- Refresh Token.
- Mật khẩu.
- Secret.
- Dữ liệu cá nhân nhạy cảm nếu không có biện pháp bảo vệ phù hợp.

---

# 8.5 Cache Key Convention

Định dạng:

Module:Entity:Identifier

Ví dụ:

Area:All

Policy:Active

Dashboard:Summary

Citizen:{CitizenId}

Quy tắc đặt tên phải thống nhất để dễ quản lý và xóa cache.

---

# 8.6 Cache Expiration

Đề xuất:

- Danh mục: 12–24 giờ.
- Dashboard tổng hợp: 5–15 phút.
- Cấu hình: đến khi thay đổi.
- Thống kê: theo nhu cầu nghiệp vụ.

Không đặt TTL quá dài cho dữ liệu thay đổi thường xuyên.

---

# 8.7 Cache Invalidation

Khi dữ liệu thay đổi:

1. Cập nhật Database.
2. Hoàn thành Transaction.
3. Xóa hoặc cập nhật Cache liên quan.
4. Ghi Audit nếu cần.

Không cập nhật Cache trước khi dữ liệu được ghi thành công.

---

# 8.8 Distributed Cache

Khi sử dụng Redis:

- Chỉ lưu dữ liệu có thể tái tạo.
- Không xem Redis là nguồn dữ liệu chính.
- Có chính sách kết nối lại (Reconnect).
- Giám sát dung lượng và hiệu năng.

---

# 8.9 AI Cache

Có thể cache:

- Prompt đã chuẩn hóa.
- Kết quả tra cứu tài liệu.
- Metadata của Knowledge Base.

Không cache phản hồi chứa dữ liệu nhạy cảm nếu chưa đánh giá rủi ro.

---

# 8.10 Monitoring

Theo dõi:

- Cache Hit Ratio.
- Cache Miss Ratio.
- Thời gian truy xuất.
- Số lượng khóa.
- Dung lượng bộ nhớ.
- Lỗi kết nối Redis (nếu sử dụng).

---

# 8.11 Testing

Kiểm thử:

- Cache Hit.
- Cache Miss.
- Cache Expiration.
- Cache Invalidation.
- Khôi phục khi cache không khả dụng.

Ứng dụng phải tiếp tục hoạt động nếu lớp cache gặp sự cố.

---

# 8.12 Checklist

□ Cache Strategy được xác định.

□ Key Convention thống nhất.

□ TTL phù hợp.

□ Invalidation đúng.

□ Monitoring hoạt động.

□ Không lưu dữ liệu nhạy cảm không cần thiết.

---

# 8.13 Expected Deliverables

Sau Phase 8:

✓ Cache Architecture

✓ Cache Strategy

✓ Cache Key Convention

✓ Expiration Policy

✓ Invalidation Policy

✓ Monitoring Guidelines

# =============================================================================
# END OF PHASE 8
# =============================================================================
```
```md id="ansinhso-infra-phase9"
# =============================================================================
# PHASE 9 – BACKGROUND PROCESSING ARCHITECTURE
# =============================================================================

---

# 9.1 Purpose

Background Processing chịu trách nhiệm thực hiện các tác vụ không cần phản hồi ngay cho người dùng.

Mục tiêu:

- Tăng khả năng phản hồi của API.
- Tách xử lý dài khỏi HTTP Request.
- Dễ mở rộng.
- Dễ thay thế công nghệ xử lý nền.

---

# 9.2 Design Principles

Background Processing phải:

✓ Asynchronous

✓ Retryable

✓ Observable

✓ Idempotent

✓ Configurable

✓ Auditable

Không chứa Business Logic đặc thù. Nghiệp vụ vẫn thuộc Application Layer.

---

# 9.3 Job Categories

Các nhóm Job:

- Notification Jobs
- Zalo OA Jobs
- AI Processing Jobs
- Import Jobs
- Export Jobs
- Synchronization Jobs
- Cleanup Jobs
- Maintenance Jobs
- Scheduled Jobs

---

# 9.4 Job Processing Flow

Request

↓

Application Layer

↓

Job Dispatcher

↓

Background Queue

↓

Worker

↓

Application Service

↓

Infrastructure

↓

Audit Log

---

# 9.5 Job Dispatcher

Job Dispatcher chịu trách nhiệm:

- Đưa Job vào hàng đợi.
- Gán độ ưu tiên.
- Gán CorrelationId.
- Ghi nhật ký.

Dispatcher không xử lý nghiệp vụ.

---

# 9.6 Worker Responsibilities

Worker chỉ:

- Lấy Job.
- Thực thi Job.
- Gọi Application Service.
- Ghi Log.
- Cập nhật trạng thái.
- Retry khi cần.

Worker không viết Business Rules.

---

# 9.7 Retry Policy

Áp dụng cho lỗi tạm thời:

- Lỗi mạng.
- Dịch vụ ngoài không phản hồi.
- Timeout.

Retry sử dụng chiến lược Backoff tăng dần và giới hạn số lần thử.

Sau khi vượt quá ngưỡng, Job chuyển sang trạng thái thất bại để xử lý tiếp.

---

# 9.8 Job Status

Chu trình trạng thái:

Pending

↓

Queued

↓

Processing

↓

Completed

↓

Failed

↓

Cancelled

Mọi thay đổi trạng thái phải được ghi log.

---

# 9.9 Scheduling

Hỗ trợ:

- One-time Job.
- Scheduled Job.
- Periodic Job.
- Manual Trigger.

Lịch chạy được cấu hình, không mã hóa cứng trong chương trình.

---

# 9.10 Idempotency

Mỗi Job phải có định danh duy nhất.

Nếu cùng một Job được gửi nhiều lần:

- Không thực hiện trùng lặp.
- Có thể nhận diện và bỏ qua bản sao.
- Ghi nhận vào Audit nếu cần.

---

# 9.11 Monitoring

Theo dõi:

- Queue Length.
- Processing Time.
- Retry Count.
- Failure Rate.
- Throughput.
- Long Running Jobs.

---

# 9.12 Future Extensibility

Thiết kế thông qua abstraction để có thể thay thế cơ chế xử lý nền mà không ảnh hưởng Application Layer.

Giai đoạn đầu:

- .NET BackgroundService.

Các giai đoạn sau có thể thay thế bằng:

- Hangfire.
- Quartz.NET.
- Hệ thống hàng đợi hoặc nền tảng xử lý nền khác.

---

# 9.13 Background Processing Checklist

□ Dispatcher tách biệt.

□ Worker không chứa Business Logic.

□ Retry Policy được cấu hình.

□ Job có trạng thái.

□ Job có CorrelationId.

□ Có Monitoring.

□ Có Audit.

---

# 9.14 Expected Deliverables

Sau Phase 9:

✓ Background Processing Architecture

✓ Job Dispatcher Pattern

✓ Worker Standards

✓ Retry Strategy

✓ Scheduling Strategy

✓ Monitoring Guidelines

# =============================================================================
# END OF PHASE 9
# =============================================================================
```
```md
# =============================================================================
# PHASE 10 – INTEGRATION INFRASTRUCTURE
# =============================================================================

---

# 10.1 Purpose

Mọi hệ thống bên ngoài phải được tích hợp thông qua Integration Layer.

Không Business Layer nào được gọi trực tiếp:

- HTTP API
- SDK
- Database khác
- AI Provider
- Zalo OA
- GIS
- SMTP

Mục tiêu:

- Loose Coupling
- Testability
- Maintainability
- Replaceability

---

# 10.2 Integration Principles

Toàn bộ External Systems phải:

↓

Provider

↓

Application

↓

Domain

Không đi ngược chiều.

---

# 10.3 Supported Providers

AI

↓

IAIProvider

↓

OpenAIProvider

GeminiProvider

FutureProvider

---

Zalo

↓

IZaloProvider

↓

OfficialOAProvider

FutureProvider

---

GIS

↓

IGISProvider

↓

LeafletProvider

FutureGISProvider

---

Storage

↓

IStorageProvider

↓

LocalStorage

CloudStorage

---

Notification

↓

INotificationProvider

↓

Zalo

Email

SMS

Push

---

# 10.4 HttpClient Policy

Không sử dụng:

new HttpClient()

Bắt buộc:

IHttpClientFactory

Có cấu hình:

- Timeout

- Retry

- Circuit Breaker

- Logging

- CorrelationId

---

# 10.5 Provider Responsibilities

Provider chỉ:

- Serialize

- Deserialize

- Authentication

- Retry

- Logging

- HTTP Call

Không Business Logic.

---

# 10.6 AI Integration

AI

↓

AI Integration Hub

↓

IAIProvider

↓

Provider

↓

Application

Không Application gọi AI SDK.

---

# 10.7 Zalo Integration

Notification

↓

Messaging Gateway

↓

IZaloProvider

↓

Zalo OA

↓

Response

---

# 10.8 GIS Integration

Map

↓

IGISProvider

↓

Leaflet

↓

GeoJSON

↓

Application

---

# 10.9 File Storage

Abstract:

IStorageProvider

Implementation:

Local

NAS

Cloud

Future

---

# 10.10 Retry Policy

Áp dụng:

AI

Zalo

SMTP

Storage

Không Retry:

Validation Error

Permission Error

Business Error

---

# 10.11 Circuit Breaker

Nếu Provider lỗi liên tục:

↓

Open Circuit

↓

Reject

↓

Cooldown

↓

Half Open

↓

Recover

---

# 10.12 Timeout Strategy

AI

30s

Zalo

15s

Storage

60s

GIS

20s

Có thể cấu hình.

---

# 10.13 Integration Logging

Ghi:

CorrelationId

Provider

Latency

Retry

Result

Error

Không log Secret.

---

# 10.14 Monitoring

Dashboard:

AI

Provider Status

Retry

Latency

Webhook

Storage

Notification

---

# 10.15 Future Extensibility

Có thể thay:

Gemini

↓

OpenAI

Claude

Azure AI

Không sửa Application.

---

Có thể thay:

Zalo

↓

SMS

Email

Push

Không sửa Business Layer.

---

# 10.16 Expected Deliverables

✓ Provider Pattern

✓ HttpClient Strategy

✓ Retry Strategy

✓ Circuit Breaker

✓ Timeout

✓ Monitoring

✓ Integration Hub

# =============================================================================
# END OF PHASE 10
# =============================================================================
```
```md id="ansinhso-infra-phase11"
# =============================================================================
# PHASE 11 – OBSERVABILITY ARCHITECTURE
# =============================================================================

---

# 11.1 Purpose

Observability giúp theo dõi toàn bộ trạng thái của hệ thống AnSinhSo trong quá trình vận hành.

Mục tiêu:

- Phát hiện sự cố sớm.
- Hỗ trợ điều tra.
- Đo lường hiệu năng.
- Theo dõi tích hợp với AI, Zalo OA và GIS.
- Đánh giá chất lượng dịch vụ.

---

# 11.2 Core Components

Observability bao gồm:

- Structured Logging
- Metrics
- Distributed Tracing
- Health Checks
- Audit Correlation
- Monitoring Dashboard
- Alerting

---

# 11.3 Structured Logging

Toàn bộ log phải theo định dạng có cấu trúc.

Mỗi bản ghi nên bao gồm:

- Timestamp
- Level
- CorrelationId
- TraceId
- UserId (nếu có)
- Module
- Action
- Duration
- Result

Không ghi:

- Password
- JWT
- Refresh Token
- API Key
- Secret
- Dữ liệu cá nhân không cần thiết

---

# 11.4 Correlation Strategy

Mỗi Request tạo một CorrelationId.

CorrelationId được truyền xuyên suốt:

Client

↓

Web API

↓

Application

↓

Infrastructure

↓

Worker

↓

AI

↓

Zalo OA

↓

Database Audit

Nhờ đó có thể truy vết toàn bộ vòng đời của một yêu cầu.

---

# 11.5 Metrics

Thu thập các chỉ số:

- Request Count
- Request Duration
- Error Rate
- Database Query Time
- Queue Length
- Cache Hit Ratio
- AI Response Time
- Zalo Delivery Rate
- GIS Response Time

---

# 11.6 Health Checks

Theo dõi:

- SQL Server
- Worker
- AI Provider
- Zalo OA Provider
- GIS Provider
- Storage
- Cache

Health Check phải phản ánh đúng trạng thái của từng thành phần.

---

# 11.7 Monitoring Dashboard

Dashboard hiển thị:

- Tình trạng API.
- Thời gian phản hồi.
- Tỷ lệ lỗi.
- Queue Status.
- AI Requests.
- Notification Delivery.
- Database Health.
- Cache Status.

---

# 11.8 Alerting

Thiết lập ngưỡng cảnh báo cho:

- API Error Rate tăng cao.
- Database không phản hồi.
- Worker dừng hoạt động.
- Queue tồn đọng.
- AI Provider lỗi.
- Zalo OA gửi thất bại liên tiếp.

Cảnh báo có thể gửi qua Dashboard, Email hoặc Zalo OA nội bộ.

---

# 11.9 Distributed Tracing

Mỗi thao tác nghiệp vụ quan trọng cần có TraceId.

TraceId kết hợp với CorrelationId để hỗ trợ điều tra các luồng xử lý kéo dài hoặc liên quan nhiều dịch vụ.

---

# 11.10 Audit Correlation

Audit Log và Application Log phải có khả năng liên kết thông qua:

- CorrelationId
- UserId
- Timestamp

Giúp truy xuất đầy đủ quá trình xử lý của một nghiệp vụ.

---

# 11.11 Retention Policy

Chính sách lưu trữ log và metrics phải được cấu hình theo môi trường.

Production cần có cơ chế xoay vòng (rotation) và lưu trữ phù hợp với quy định của đơn vị vận hành.

---

# 11.12 Observability Checklist

□ Structured Logging hoạt động.

□ CorrelationId được truyền xuyên suốt.

□ Health Checks đầy đủ.

□ Metrics được thu thập.

□ Dashboard hiển thị trạng thái hệ thống.

□ Alerting được cấu hình.

□ Audit có thể đối chiếu với Application Log.

---

# 11.13 Expected Deliverables

Sau Phase 11:

✓ Observability Architecture

✓ Logging Standards

✓ Metrics Standards

✓ Tracing Strategy

✓ Monitoring Dashboard

✓ Alerting Strategy

✓ Health Check Guidelines

# =============================================================================
# END OF PHASE 11
# =============================================================================
```
```md id="ansinhso-infra-phase12"
# =============================================================================
# PHASE 12 – DEPLOYMENT ARCHITECTURE
# =============================================================================

---

# 12.1 Purpose

Deployment Architecture định nghĩa cách triển khai, vận hành và nâng cấp hệ thống AnSinhSo trên các môi trường Development, Testing, Staging và Production.

Mục tiêu:

- Triển khai nhất quán.
- Dễ bảo trì.
- An toàn.
- Hỗ trợ nâng cấp và khôi phục.

---

# 12.2 Deployment Environments

Các môi trường triển khai:

- Development
- Testing
- Staging
- Production

Mỗi môi trường có:

- Cấu hình riêng.
- Cơ sở dữ liệu riêng.
- Secret riêng.
- Logging riêng.
- Monitoring riêng.

---

# 12.3 Logical Deployment Topology

Client (Web / Mobile)

↓

Reverse Proxy (nếu sử dụng)

↓

ASP.NET Core Web API

↓

Background Worker

↓

SQL Server

↓

External Providers

- AI
- Zalo OA
- GIS
- Storage

---

# 12.4 Hosting Strategy

Giai đoạn đầu:

- Windows Server
- IIS
- SQL Server

Có khả năng mở rộng sang:

- Linux
- Reverse Proxy
- Container (nếu cần trong tương lai)

Kiến trúc không phụ thuộc vào nền tảng triển khai cụ thể.

---

# 12.5 Deployment Process

Quy trình triển khai:

1. Build.
2. Chạy kiểm thử tự động.
3. Tạo gói triển khai.
4. Sao lưu cơ sở dữ liệu.
5. Áp dụng Migration.
6. Triển khai ứng dụng.
7. Kiểm tra Health Check.
8. Xác nhận hệ thống hoạt động.

---

# 12.6 Rollback Strategy

Trong trường hợp triển khai thất bại:

- Khôi phục phiên bản ứng dụng trước.
- Khôi phục cơ sở dữ liệu nếu cần.
- Ghi nhận sự cố.
- Thực hiện đánh giá nguyên nhân.

Rollback phải được kiểm thử trước khi Production.

---

# 12.7 HTTPS & Network

Yêu cầu:

- HTTPS bắt buộc.
- Chứng chỉ số hợp lệ.
- Giới hạn cổng dịch vụ.
- Chỉ mở các cổng cần thiết.
- Bảo vệ bằng tường lửa theo chính sách của đơn vị triển khai.

---

# 12.8 Deployment Validation

Sau mỗi lần triển khai cần kiểm tra:

- API.
- Database.
- Worker.
- AI Provider.
- Zalo OA.
- GIS.
- Health Checks.
- Logging.

---

# 12.9 Versioning

Mỗi lần phát hành cần ghi nhận:

- Version.
- Ngày phát hành.
- Nội dung thay đổi.
- Migration đi kèm.
- Người phê duyệt.

---

# 12.10 Deployment Checklist

□ Build thành công.

□ Unit Test đạt.

□ Migration được review.

□ Backup hoàn tất.

□ Health Check đạt.

□ Monitoring hoạt động.

□ Rollback Plan sẵn sàng.

---

# 12.11 Expected Deliverables

Sau Phase 12:

✓ Deployment Architecture

✓ Deployment Workflow

✓ Rollback Strategy

✓ Validation Checklist

✓ Versioning Guidelines

# =============================================================================
# END OF PHASE 12
# =============================================================================
```
```md id="ansinhso-infra-phase13"
# =============================================================================
# PHASE 13 – SCALABILITY & PERFORMANCE ARCHITECTURE
# =============================================================================

---

# 13.1 Purpose

Scalability & Performance Architecture định nghĩa chiến lược mở rộng và tối ưu hiệu năng của hệ thống AnSinhSo.

Mục tiêu:

- Đáp ứng số lượng người dùng tăng dần.
- Duy trì thời gian phản hồi ổn định.
- Giảm điểm nghẽn (bottleneck).
- Cho phép mở rộng mà không thay đổi kiến trúc cốt lõi.

---

# 13.2 Design Principles

Áp dụng các nguyên tắc:

- Stateless Application.
- Horizontal Scalability Ready.
- Vertical Scalability Ready.
- Caching by Design.
- Async Processing.
- Database Optimization.
- Resource Isolation.

---

# 13.3 Scalability Strategy

### Giai đoạn 1 – MVP

- 01 Web API.
- 01 Background Worker.
- 01 SQL Server.
- IMemoryCache.

### Giai đoạn 2 – Production

- Nhiều Web API Instance.
- Redis.
- Reverse Proxy / Load Balancer (khi cần).
- Monitoring tập trung.

### Giai đoạn 3 – Future

- Container hóa.
- Tách Worker theo nhóm tác vụ.
- Mở rộng Storage và AI Provider.

---

# 13.4 Stateless Services

Application không lưu trạng thái phiên làm việc trong bộ nhớ tiến trình.

Các trạng thái cần thiết được quản lý thông qua:

- JWT.
- Database.
- Distributed Cache (khi áp dụng).

Điều này cho phép mở rộng nhiều máy chủ mà không cần thay đổi Business Logic.

---

# 13.5 Performance Guidelines

Ưu tiên:

- Truy vấn tối ưu.
- Phân trang dữ liệu.
- Batch Processing khi phù hợp.
- Xử lý bất đồng bộ cho tác vụ dài.
- Giảm truy vấn lặp.
- Hạn chế tải dữ liệu dư thừa.

---

# 13.6 Database Performance

Khuyến nghị:

- Chỉ mục phù hợp.
- Truy vấn có chọn lọc.
- AsNoTracking() cho dữ liệu chỉ đọc.
- Kiểm soát N+1 Query.
- Theo dõi truy vấn chậm.

---

# 13.7 Background Processing Performance

- Không thực hiện tác vụ dài trong HTTP Request.
- Chuyển các tác vụ nền sang Job Dispatcher.
- Giới hạn số lượng Job đồng thời theo cấu hình.
- Theo dõi thời gian xử lý và tỷ lệ thất bại.

---

# 13.8 Cache Performance

Theo dõi:

- Cache Hit Ratio.
- Cache Miss Ratio.
- TTL.
- Memory Usage.

Điều chỉnh chính sách cache dựa trên số liệu thực tế.

---

# 13.9 External Integration Performance

Giới hạn thời gian chờ (timeout) và số lần thử lại (retry) cho:

- AI Provider.
- Zalo OA.
- GIS.
- File Storage.

Không để một dịch vụ ngoài làm ảnh hưởng toàn bộ hệ thống.

---

# 13.10 Capacity Planning

Định kỳ đánh giá:

- Số lượng người dùng.
- Dung lượng cơ sở dữ liệu.
- Tăng trưởng dữ liệu.
- Tải của Worker.
- Tần suất gửi thông báo.

Các quyết định mở rộng dựa trên số liệu giám sát, không dựa trên giả định.

---

# 13.11 Performance Testing

Thực hiện các loại kiểm thử:

- Load Test.
- Stress Test.
- Endurance Test.
- Spike Test.

Ghi nhận kết quả và so sánh với mục tiêu hiệu năng đã đặt ra.

---

# 13.12 Scalability Checklist

□ Ứng dụng không phụ thuộc trạng thái tiến trình.

□ Có chiến lược cache.

□ Có xử lý nền.

□ Có kế hoạch mở rộng nhiều máy chủ.

□ Có chỉ số theo dõi hiệu năng.

□ Có kiểm thử tải trước Production.

---

# 13.13 Expected Deliverables

Sau Phase 13:

✓ Scalability Strategy

✓ Performance Guidelines

✓ Capacity Planning

✓ Performance Testing Strategy

✓ Future Expansion Roadmap

# =============================================================================
# END OF PHASE 13
# =============================================================================
```
```md id="ansinhso-infra-phase14"
# =============================================================================
# PHASE 14 – PRODUCTION INFRASTRUCTURE CHECKLIST
# =============================================================================

---

# 14.1 Purpose

Checklist này được sử dụng trước mỗi lần triển khai Production nhằm đảm bảo hạ tầng, cấu hình và dịch vụ của hệ thống AnSinhSo đáp ứng đầy đủ các tiêu chí vận hành.

Mọi hạng mục phải được xác nhận trước khi phát hành.

---

# 14.2 Environment

□ Đúng môi trường (Production)

□ appsettings.Production.json được kiểm tra

□ Environment Variables đầy đủ

□ Không còn cấu hình Development

□ Secret được nạp từ Secret Store hoặc Environment Variables

---

# 14.3 Database

□ Sao lưu thành công

□ Khôi phục thử nghiệm thành công

□ Migration đã được review

□ Chỉ mục được kiểm tra

□ Connection String chính xác

□ Người dùng cơ sở dữ liệu có quyền tối thiểu cần thiết

---

# 14.4 Web API

□ Build thành công

□ Không còn lỗi nghiêm trọng

□ Swagger được cấu hình theo chính sách triển khai

□ Health Checks hoạt động

□ HTTPS hoạt động

□ CORS đúng cấu hình

---

# 14.5 Background Worker

□ Worker khởi động thành công

□ Dispatcher hoạt động

□ Retry Policy hoạt động

□ Job Queue hoạt động

□ Logging đầy đủ

---

# 14.6 External Providers

## AI

□ API Key hợp lệ

□ Timeout đúng

□ Retry đúng

□ Logging hoạt động

---

## Zalo OA

□ Access Token hợp lệ

□ OA Secret được cấu hình

□ Webhook xác thực thành công

□ Gửi thông báo thử nghiệm thành công

---

## GIS

□ Dịch vụ bản đồ hoạt động

□ Dữ liệu GeoJSON kiểm tra thành công

---

# 14.7 Security

□ JWT hoạt động

□ Refresh Token hoạt động

□ RBAC kiểm tra thành công

□ Policy-Based Authorization hoạt động

□ Security Headers được bật

---

# 14.8 Observability

□ Structured Logging

□ CorrelationId

□ Metrics

□ Monitoring Dashboard

□ Alerting

□ Audit Logging

---

# 14.9 Performance

□ Cache hoạt động

□ Không có truy vấn chậm nghiêm trọng

□ Background Jobs ổn định

□ Thời gian phản hồi API đạt mục tiêu

---

# 14.10 Backup & Recovery

□ Kế hoạch sao lưu được kích hoạt

□ Kế hoạch khôi phục được kiểm thử

□ Tài liệu Disaster Recovery được cập nhật

---

# 14.11 Documentation

Đã cập nhật:

□ SYSTEM_ARCHITECTURE.md

□ DATABASE_DESIGN.md

□ API_SPEC.md

□ SECURITY_ARCHITECTURE.md

□ INFRASTRUCTURE_ARCHITECTURE.md

□ DEVELOPMENT_SPRINTS.md

---

# 14.12 Deployment Approval

Yêu cầu xác nhận của:

□ Project Owner

□ Technical Reviewer

□ System Administrator

(Trong phạm vi đồ án, các vai trò này có thể do cùng một người đảm nhiệm nhưng vẫn nên được ghi nhận riêng trong tài liệu để phản ánh quy trình quản trị.)

---

# 14.13 Production Acceptance Criteria

Chỉ triển khai khi:

✓ Không còn lỗi mức Critical

✓ Không còn lỗi mức High chưa được chấp nhận

✓ Health Checks đạt

✓ Backup sẵn sàng

✓ Monitoring hoạt động

✓ Rollback Plan khả thi

---

# 14.14 Expected Deliverables

Sau Phase 14:

✓ Production Checklist

✓ Deployment Checklist

✓ Approval Workflow

✓ Production Acceptance Criteria

# =============================================================================
# END OF PHASE 14
# =============================================================================
```
```md id="ansinhso-infra-phase15"
# =============================================================================
# PHASE 15 – INFRASTRUCTURE BASELINE v1.0
# =============================================================================

---

# 15.1 Purpose

Infrastructure Baseline v1.0 là phiên bản kiến trúc hạ tầng chính thức của dự án AnSinhSo.

Tài liệu này tổng hợp các quyết định kỹ thuật đã được phê duyệt và là cơ sở bắt buộc cho mọi Sprint phát triển, kiểm thử và triển khai.

Mọi thay đổi ảnh hưởng đến kiến trúc phải được đánh giá và cập nhật Baseline trước khi áp dụng.

---

# 15.2 Approved Architecture

Kiến trúc chính thức của hệ thống:

- Architecture Style: Modular Monolith
- Design Pattern: Clean Architecture
- Development Model: Sprint-based
- API Style: RESTful API
- Authentication: JWT + Refresh Token
- Authorization: RBAC + Policy-Based Authorization
- Background Processing: Job Dispatcher + Worker
- Integration: Provider Pattern
- Configuration: Options Pattern
- Observability: Structured Logging + Metrics + Health Checks
- Security: Zero Trust Principles + Defense in Depth

---

# 15.3 Approved Technology Stack

| Thành phần | Công nghệ |
|------------|-----------|
| Framework | ASP.NET Core 8 (.NET 8 LTS) |
| Language | C# 12 |
| ORM | Entity Framework Core 8 |
| Database | SQL Server 2022 |
| Logging | Serilog |
| Validation | FluentValidation |
| API Documentation | Swagger / OpenAPI |
| Background Jobs | .NET BackgroundService (MVP) |
| Cache | IMemoryCache (MVP), Redis (Production-ready) |
| Testing | xUnit + FluentAssertions |
| Source Control | Git |
| Hosting | IIS trên Windows Server (giai đoạn đầu) |

---

# 15.4 Mandatory Architecture Principles

Mọi Sprint phải tuân thủ:

✓ Clean Architecture.

✓ Dependency Inversion.

✓ SOLID.

✓ DRY.

✓ KISS.

✓ Fail Fast.

✓ Explicit Dependencies.

✓ Separation of Concerns.

Không chấp nhận thay đổi làm phá vỡ các nguyên tắc trên.

---

# 15.5 Mandatory Development Rules

Bắt buộc:

- Không viết Business Logic trong Controller.
- Không truy cập Database trực tiếp từ Presentation.
- Không gọi trực tiếp AI, Zalo OA hoặc GIS từ Application.
- Không khởi tạo thủ công Repository, Service hoặc HttpClient.
- Không ghi Secret hoặc thông tin nhạy cảm vào mã nguồn hoặc log.

---

# 15.6 Approved Integration Model

Mọi tích hợp phải đi qua abstraction:

Application

↓

Interface

↓

Infrastructure Provider

↓

External System

Các Provider phải có khả năng thay thế mà không ảnh hưởng Domain và Application.

---

# 15.7 Approved Deployment Model

MVP:

- IIS
- Windows Server
- SQL Server
- Background Worker
- IMemoryCache

Production Ready:

- Redis
- Reverse Proxy / Load Balancer (khi cần)
- Monitoring tập trung
- Mở rộng nhiều Web API Instance

---

# 15.8 Architecture Governance

Mọi thay đổi kiến trúc phải:

1. Được đánh giá tác động.
2. Cập nhật tài liệu liên quan.
3. Được review.
4. Được ghi nhận trong Change Log.

Không thay đổi trực tiếp trong mã nguồn mà không phản ánh vào tài liệu.

---

# 15.9 AI Development Governance

Antygravity AI và các AI Coding Agent khác phải:

- Tuân thủ Technical Baseline.
- Tuân thủ Security Architecture.
- Tuân thủ Infrastructure Architecture.
- Tuân thủ Development Sprints.
- Không tự thay đổi Technology Stack.
- Không tự thay đổi kiến trúc.

Nếu phát hiện xung đột giữa tài liệu và mã nguồn, ưu tiên tài liệu đã được Baseline.

---

# 15.10 Baseline Review

Sau mỗi Sprint:

- Review kiến trúc.
- Đánh giá thay đổi.
- Quyết định có cần cập nhật Baseline hay không.

Không cập nhật Baseline nếu thay đổi chỉ ở mức triển khai chi tiết.

---

# 15.11 Versioning

Current Version:

Infrastructure Baseline v1.0

Trạng thái:

Approved

Có hiệu lực từ:

Ngày hoàn thành giai đoạn Architecture & Design.

---

# 15.12 Completion Criteria

Infrastructure Architecture được xem là hoàn thành khi:

✓ 15 Phase đã hoàn tất.

✓ Được Architecture Review.

✓ Được Baseline Freeze.

✓ Đồng bộ với các tài liệu kiến trúc khác.

✓ Sẵn sàng cho Sprint 01.

---

# 15.13 Architecture Trilogy

Ba tài liệu kiến trúc cốt lõi của AnSinhSo:

1. SYSTEM_ARCHITECTURE.md
   - Mô tả cấu trúc và luồng hoạt động của hệ thống.

2. SECURITY_ARCHITECTURE.md
   - Mô tả chiến lược bảo mật và kiểm soát truy cập.

3. INFRASTRUCTURE_ARCHITECTURE.md
   - Mô tả triển khai, vận hành và tiêu chuẩn kỹ thuật.

Ba tài liệu này tạo thành nền tảng kiến trúc thống nhất của dự án.

---

# 15.14 Expected Deliverables

Sau Phase 15:

✓ Infrastructure Baseline v1.0

✓ Technical Baseline

✓ Governance Rules

✓ Approved Technology Stack

✓ Architecture Freeze

✓ Sprint Readiness

---

# =============================================================================
# END OF DOCUMENT
#
# Document:
# 21_INFRASTRUCTURE_ARCHITECTURE.md
#
# Status:
# BASELINE v1.0
#
# Ready for:
# Sprint 01 – Foundation
# =============================================================================
```
