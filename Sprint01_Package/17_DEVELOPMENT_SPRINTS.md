````md
# ============================================================================
# 17_DEVELOPMENT_SPRINTS.md
# ============================================================================
#
# Project         : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
# Document Type   : Enterprise Sprint Management
# Version         : 1.0.0
# Status          : Approved
#
# Architecture    : Enterprise Clean Architecture
# Framework       : ASP.NET Core 8
# Database        : SQL Server 2022
#
# Last Updated    : 2026-07-12
#
# ============================================================================

# 1. PURPOSE

Tài liệu này quy định toàn bộ kế hoạch triển khai Sprint của dự án AnSinhSo.

Đây là tài liệu điều hành (Execution Document) được sử dụng trong suốt quá trình phát triển hệ thống.

Sau khi tài liệu này được phê duyệt, mọi hoạt động lập trình đều phải thực hiện theo Sprint được định nghĩa tại đây.

---

# 2. OBJECTIVES

Mục tiêu của tài liệu:

- Chuẩn hóa Development Sprint.
- Chuẩn hóa Task Breakdown.
- Chuẩn hóa Deliverables.
- Chuẩn hóa Acceptance Criteria.
- Đồng bộ giữa Documentation và Source Code.
- Đồng bộ giữa AI và Developer.

---

# 3. DEVELOPMENT PRINCIPLES

Mọi Sprint phải:

- Tuân thủ Bootstrap.
- Tuân thủ Enterprise Standards.
- Tuân thủ Business Rules.
- Tuân thủ Database Design.
- Tuân thủ API Standards.
- Tuân thủ Implementation Roadmap.

Không được thay đổi kiến trúc hệ thống trong quá trình thực hiện Sprint.

---

# 4. DEVELOPMENT LIFECYCLE

```text
Planning

↓

Analysis

↓

Database

↓

Backend

↓

Frontend

↓

Integration

↓

Testing

↓

Documentation

↓

Review

↓

Sprint Closing
```

---

# 5. SPRINT OVERVIEW

| Sprint | Mục tiêu chính | Trạng thái |
|----------|----------------|------------|
| Sprint 01 | Foundation | Planned |
| Sprint 02 | Authentication | Planned |
| Sprint 03 | User & Role | Planned |
| Sprint 04 | Master Data | Planned |
| Sprint 05 | Household | Planned |
| Sprint 06 | Citizen | Planned |
| Sprint 07 | Social Welfare | Planned |
| Sprint 08 | Payment | Planned |
| Sprint 09 | Frontend Foundation | Planned |
| Sprint 10 | Dashboard | Planned |
| Sprint 11 | GIS | Planned |
| Sprint 12 | AI | Planned |
| Sprint 13 | Zalo OA | Planned |
| Sprint 14 | Testing | Planned |
| Sprint 15 | Deployment | Planned |
| Sprint 16 | Production | Planned |

---

# 6. SPRINT EXECUTION MODEL

Mỗi Sprint đều tuân theo mô hình:

```text
Sprint Goal

↓

User Stories

↓

Task Breakdown

↓

Implementation

↓

Testing

↓

Documentation

↓

Review

↓

Acceptance

↓

Sprint Close
```

---

# 7. SPRINT TEMPLATE

Mỗi Sprint phải được mô tả theo cấu trúc sau:

## Sprint Information

- Sprint ID
- Sprint Name
- Sprint Goal
- Duration
- Priority

---

## User Stories

- User Story 01
- User Story 02
- ...

---

## Database Tasks

- Thiết kế bảng
- Migration
- Seed Data

---

## Backend Tasks

- API
- Repository
- Service
- Authentication
- Validation

---

## Frontend Tasks

- Layout
- Components
- Forms
- Dashboard
- Responsive

---

## Integration Tasks

- API Integration
- GIS
- AI
- Zalo OA

---

## Testing Tasks

- Unit Test
- Integration Test
- Regression Test

---

## Documentation Tasks

- Progress Update
- Change Log
- AI Handover

---

## Deliverables

- Source Code
- SQL Scripts
- API
- Documentation
- Test Cases

---

## Acceptance Criteria

- Functional
- Security
- Performance
- Documentation

---

## Definition of Done

- Review
- Test Pass
- Documentation Updated
- Approved

---

# 8. SPRINT MANAGEMENT RULES

## Planning

Sprint chỉ được bắt đầu khi:

□ Sprint trước đã hoàn thành.

□ Documentation được cập nhật.

□ Business Rules không còn thay đổi.

---

## During Sprint

Trong quá trình Sprint:

- Không đổi Architecture.
- Không đổi Database tùy ý.
- Không bỏ qua Testing.
- Không bỏ qua Documentation.

---

## Sprint Closing

Sprint chỉ được đóng khi:

□ Tất cả Task hoàn thành.

□ Acceptance Criteria đạt.

□ Documentation cập nhật.

□ Project Owner phê duyệt.

---

# 9. PHASE 1 CHECKLIST

Trước khi chuyển sang Phase 2A cần xác nhận:

□ Sprint Framework được chuẩn hóa.

□ Sprint Lifecycle được xác định.

□ Sprint Template hoàn chỉnh.

□ Sprint Rules hoàn chỉnh.

□ Sprint Overview được xác định.

---

# End of Phase 1

Phase tiếp theo:

- Sprint 01 – Foundation
- Infrastructure
- Solution Structure
- Coding Environment
- Development Environment
- CI Foundation
- Initial Deliverables
````
````md
# ============================================================================
# 10. SPRINT 01 – FOUNDATION
# ============================================================================

## 10.1 Sprint Information

| Thuộc tính | Giá trị |
|------------|----------|
| Sprint ID | Sprint 01 |
| Sprint Name | Foundation |
| Priority | Critical |
| Duration | 2 Weeks |
| Status | Planned |
| Milestone | Enterprise Foundation |

---

# 10.2 Sprint Goal

Mục tiêu của Sprint 01 là xây dựng nền tảng kỹ thuật (Technical Foundation) cho toàn bộ dự án AnSinhSo.

Sau khi Sprint này hoàn thành, hệ thống phải sẵn sàng để phát triển các chức năng nghiệp vụ mà không cần thay đổi kiến trúc.

Sprint này không tập trung vào nghiệp vụ (Business Logic), mà tập trung xây dựng một nền tảng ổn định, dễ mở rộng và tuân thủ các tiêu chuẩn Enterprise.

---

# 10.3 Sprint Objectives

Hoàn thành:

- Enterprise Solution Structure
- Clean Architecture
- Coding Convention
- Dependency Injection
- Configuration Management
- Logging Framework
- Exception Handling
- API Standard Response
- Swagger
- Health Check
- Git Repository
- CI Foundation
- Development Environment

---

# 10.4 User Stories

### User Story 01

**As a** Developer

**I want** một Solution được tổ chức theo Clean Architecture

**So that** việc phát triển và bảo trì hệ thống trở nên dễ dàng.

---

### User Story 02

**As a** Developer

**I want** cấu hình Dependency Injection hoàn chỉnh

**So that** các Service có thể mở rộng và kiểm thử độc lập.

---

### User Story 03

**As a** Developer

**I want** Logging và Exception Handling hoạt động

**So that** việc giám sát và xử lý lỗi được thống nhất.

---

### User Story 04

**As a** System Administrator

**I want** Health Check API

**So that** có thể theo dõi trạng thái hoạt động của hệ thống.

---

# 10.5 Architecture Tasks

## Solution Structure

Tạo Solution:

```text
AnSinhSo.sln
```

---

## Projects

```text
src/

AnSinhSo.API

AnSinhSo.Application

AnSinhSo.Domain

AnSinhSo.Infrastructure

AnSinhSo.Shared

tests/

AnSinhSo.UnitTests

AnSinhSo.IntegrationTests
```

---

## Folder Structure

```text
src/

API/

Controllers

Middlewares

Filters

Configurations

Application/

Interfaces

Services

DTOs

Validators

Mappings

Domain/

Entities

Enums

ValueObjects

Events

Infrastructure/

Persistence

Repositories

Identity

Logging

ExternalServices

Shared/

Constants

Exceptions

Responses

Utilities
```

---

# 10.6 Environment Setup Tasks

Chuẩn bị môi trường:

- Visual Studio 2022
- .NET SDK 8
- SQL Server 2022
- SQL Server Management Studio
- Git
- GitHub
- Postman
- Docker Desktop (Optional)

---

# 10.7 Configuration Tasks

Thiết lập:

- appsettings.json
- appsettings.Development.json
- appsettings.Production.json
- launchSettings.json
- User Secrets
- Environment Variables

---

# 10.8 Dependency Injection Tasks

Đăng ký:

- Services
- Repositories
- AutoMapper
- Validators
- FluentValidation
- Logging
- Authentication (placeholder)
- Authorization (placeholder)

---

# 10.9 Middleware Tasks

Xây dựng:

- Global Exception Middleware
- Request Logging Middleware
- Response Wrapper Middleware
- Correlation ID Middleware
- Request Timing Middleware

---

# 10.10 API Tasks

Khởi tạo:

- Swagger
- API Versioning
- Health Check API
- Base Controller
- Standard Response
- Error Response

---

# 10.11 Infrastructure Tasks

Triển khai:

- Logging
- Configuration
- Repository Pattern
- Unit of Work (Skeleton)
- Dependency Registration

---

# 10.12 Testing Tasks

Chuẩn bị:

- xUnit
- FluentAssertions
- Test Project
- Test Configuration

Sprint này chưa yêu cầu Unit Test nghiệp vụ.

---

# 10.13 Documentation Tasks

Cập nhật:

- PROJECT_PROGRESS.md
- AI_HANDOVER.md
- CHANGELOG
- README.md

---

# 10.14 Deliverables

Sau Sprint 01 phải có:

### Source Code

□ Solution

□ Projects

□ Folder Structure

□ Middleware

□ Logging

□ Swagger

□ Health Check

---

### Configuration

□ appsettings

□ launchSettings

□ Environment

---

### Documentation

□ README

□ Bootstrap Update

□ Progress Update

---

# 10.15 Acceptance Criteria

Sprint được xem là hoàn thành khi:

### Architecture

□ Clean Architecture đúng.

□ Layer không phụ thuộc sai chiều.

---

### API

□ Swagger hoạt động.

□ Health Check hoạt động.

□ Standard Response hoạt động.

---

### Middleware

□ Global Exception hoạt động.

□ Logging hoạt động.

□ Correlation ID hoạt động.

---

### Configuration

□ Development chạy.

□ Production cấu hình sẵn.

---

### Documentation

□ Documentation cập nhật.

□ Sprint Report hoàn chỉnh.

---

# 10.16 Risks

Các rủi ro cần kiểm soát:

- Sai cấu trúc Solution.
- Sai Dependency Injection.
- Middleware xung đột.
- Logging không thống nhất.
- Thiếu chuẩn Response.
- Thiếu tài liệu.

---

# 10.17 Definition of Done

Sprint 01 chỉ được đóng khi:

□ Solution Build thành công.

□ Không còn Compile Error.

□ Swagger hoạt động.

□ Health Check trả về HTTP 200.

□ Middleware hoạt động đúng.

□ Logging ghi nhận đầy đủ.

□ Cấu trúc thư mục đúng chuẩn.

□ Documentation cập nhật đầy đủ.

□ Được Project Owner phê duyệt.

---

# 10.18 Sprint Output

Sau Sprint 01, dự án sẽ có:

- Enterprise Solution.
- Clean Architecture.
- Chuẩn Coding.
- Chuẩn Logging.
- Chuẩn Exception.
- Chuẩn Response.
- Chuẩn Middleware.
- Chuẩn Dependency Injection.
- Chuẩn Testing.
- Chuẩn Documentation.

Đây sẽ là nền tảng cho toàn bộ các Sprint tiếp theo.

---

# 10.19 Phase 2A Checklist

Trước khi chuyển sang Sprint 02 cần xác nhận:

□ Enterprise Solution hoàn chỉnh.

□ Build thành công.

□ Swagger hoạt động.

□ Health Check hoạt động.

□ Logging hoạt động.

□ Middleware hoàn chỉnh.

□ Documentation cập nhật.

□ Sprint 01 được nghiệm thu.

---

# End of Sprint 01

Tiếp theo:

- Sprint 02 – Authentication & Authorization
- JWT Authentication
- Identity Management
- Refresh Token
- Role Management
- Permission Management
- Security Foundation
- User Authentication API
````
````md
# ============================================================================
# 11. SPRINT 02 – AUTHENTICATION & AUTHORIZATION
# ============================================================================

## 11.1 Sprint Information

| Thuộc tính | Giá trị |
|------------|----------|
| Sprint ID | Sprint 02 |
| Sprint Name | Authentication & Authorization |
| Priority | Critical |
| Duration | 2 Weeks |
| Status | Planned |
| Depends On | Sprint 01 |

---

# 11.2 Sprint Goal

Xây dựng toàn bộ nền tảng xác thực và phân quyền cho hệ thống AnSinhSo.

Sau Sprint này, hệ thống phải có khả năng:

- Đăng nhập
- Xác thực JWT
- Refresh Token
- Phân quyền theo Role
- Phân quyền theo Permission
- Quản lý phiên đăng nhập

Sprint này là nền tảng cho tất cả các Sprint nghiệp vụ phía sau.

---

# 11.3 Business Objectives

Hoàn thành:

- Authentication
- Authorization
- Identity Management
- JWT Security
- Refresh Token
- User Profile
- Password Management

---

# 11.4 User Stories

### User Story 01

As a User

I want to login securely

So that I can access the system.

---

### User Story 02

As an Administrator

I want to assign roles

So that permissions are managed correctly.

---

### User Story 03

As a User

I want to refresh my access token

So that I do not need to login repeatedly.

---

### User Story 04

As a Security Administrator

I want every request authenticated

So that unauthorized access is prevented.

---

# 11.5 Database Tasks

Hoàn thiện các bảng:

- Users
- Roles
- Permissions
- RolePermissions
- UserRoles
- RefreshTokens

Kiểm tra:

□ Primary Key

□ Foreign Key

□ Index

□ Seed Data

□ Audit Fields

---

# 11.6 Backend Tasks

Triển khai:

Authentication Module

Bao gồm:

- Login API
- Logout API
- Refresh Token API
- Current User API
- Change Password API

---

Application Layer

- DTOs
- Validators
- Interfaces
- Services

---

Infrastructure Layer

- JWT Provider
- Password Hasher
- Identity Service
- Refresh Token Service

---

API Layer

- AuthController
- UserController

---

# 11.7 Security Tasks

Áp dụng:

- JWT Bearer
- Password Hashing
- Role Authorization
- Policy Authorization
- Claims
- Token Expiration
- Refresh Token Rotation

---

# 11.8 Configuration Tasks

Hoàn thiện:

appsettings.json

```json
JWT

Issuer

Audience

SecretKey

AccessTokenExpiration

RefreshTokenExpiration
```

---

# 11.9 API Endpoints

POST

/api/auth/login

---

POST

/api/auth/refresh

---

POST

/api/auth/logout

---

POST

/api/auth/change-password

---

GET

/api/auth/profile

---

GET

/api/auth/me

---

# 11.10 Validation Rules

Kiểm tra:

Email

Username

Password

Phone

Role

Permission

Token

Refresh Token

---

# 11.11 Testing Tasks

Viết:

Unit Test

- Login

- JWT

- Refresh Token

- Password

---

Integration Test

- Authentication

- Authorization

- Security

---

# 11.12 Documentation Tasks

Cập nhật:

PROJECT_PROGRESS.md

AI_HANDOVER.md

CHANGELOG

API_SPEC.md

SECURITY_STANDARDS.md

---

# 11.13 Deliverables

Source Code

□ Authentication Module

□ JWT Module

□ Identity Module

□ Refresh Token Module

---

Database

□ Users

□ Roles

□ Permissions

□ Refresh Tokens

---

Documentation

□ API Update

□ Sprint Report

---

# 11.14 Acceptance Criteria

Authentication

□ Login thành công.

□ Logout thành công.

□ JWT hợp lệ.

□ Refresh Token hoạt động.

---

Authorization

□ Role hoạt động.

□ Permission hoạt động.

□ Policy hoạt động.

---

Security

□ Password Hash đúng.

□ Token không bị giả mạo.

□ Unauthorized trả về HTTP 401.

□ Forbidden trả về HTTP 403.

---

Documentation

□ Swagger cập nhật.

□ API Spec cập nhật.

□ Sprint Report hoàn chỉnh.

---

# 11.15 Risks

Các rủi ro:

- JWT cấu hình sai.

- Secret Key không an toàn.

- Refresh Token hết hạn.

- Permission sai.

- Authentication bypass.

---

# 11.16 Definition of Done

Sprint chỉ được đóng khi:

□ Login thành công.

□ JWT hoạt động.

□ Refresh Token hoạt động.

□ Role Authorization hoạt động.

□ Permission hoạt động.

□ Swagger cập nhật.

□ Unit Test Pass.

□ Integration Test Pass.

□ Documentation cập nhật.

□ Được Project Owner nghiệm thu.

---

# 11.17 Sprint Output

Sau Sprint 02, hệ thống sẽ có:

- Authentication Framework
- Authorization Framework
- Identity Management
- JWT Security
- Refresh Token
- Role-Based Access Control (RBAC)
- Security Foundation

Đây sẽ là nền tảng cho toàn bộ các Sprint nghiệp vụ tiếp theo.

---

# 11.18 Sprint Review Checklist

□ Authentication đúng Standards.

□ Authorization đúng Business Rules.

□ Security đạt yêu cầu.

□ API đúng REST Standards.

□ Test đạt 100%.

□ Documentation đồng bộ.

---

# End of Sprint 02

Sprint tiếp theo:

Sprint 03 – User & Role Management
````
````md
# ============================================================================
# 12. SPRINT 03 – USER & ROLE MANAGEMENT
# ============================================================================

## 12.1 Sprint Information

| Thuộc tính | Giá trị |
|------------|----------|
| Sprint ID | Sprint 03 |
| Sprint Name | User & Role Management |
| Priority | Critical |
| Duration | 2 Weeks |
| Status | Planned |
| Depends On | Sprint 02 |
| Milestone | Identity Management |

---

# 12.2 Sprint Goal

Triển khai toàn bộ hệ thống quản lý người dùng và phân quyền cho AnSinhSo.

Sau Sprint này, hệ thống phải quản lý được:

- Người dùng
- Vai trò
- Quyền hạn
- Phân quyền theo chức năng
- Hồ sơ người dùng
- Nhật ký hoạt động

Đây là Sprint hoàn thiện nền tảng quản trị hệ thống trước khi bước sang các module nghiệp vụ.

---

# 12.3 Business Modules

Sprint này bao gồm:

- User Management
- Role Management
- Permission Management
- User Profile
- User Status
- Audit Log

---

# 12.4 Business Flow

```text
Administrator

↓

Đăng nhập

↓

Quản lý User

↓

Gán Role

↓

Role nhận Permission

↓

User sử dụng chức năng

↓

Ghi Audit Log

↓

Dashboard
```

---

# 12.5 User Stories

### User Story 01

As an Administrator

I want to create new users

So that commune staff can access the system.

---

### User Story 02

As an Administrator

I want to assign roles

So that users only access authorized functions.

---

### User Story 03

As an Administrator

I want to deactivate accounts

So that inactive users cannot access the system.

---

### User Story 04

As an Auditor

I want to view user activities

So that system usage can be monitored.

---

# 12.6 Database Mapping

Các bảng sử dụng:

```text
Users

↓

Roles

↓

Permissions

↓

RolePermissions

↓

UserRoles

↓

AuditLogs
```

---

## AuditLogs

Lưu:

- User
- Action
- Module
- IP
- Browser
- Time
- Result

---

# 12.7 Backend Modules

Triển khai:

```text
Application

UserService

RoleService

PermissionService

AuditService

↓

Infrastructure

UserRepository

RoleRepository

PermissionRepository

AuditRepository

↓

API

UsersController

RolesController

PermissionsController
```

---

# 12.8 API Contract

## User API

GET

/api/users

---

GET

/api/users/{id}

---

POST

/api/users

---

PUT

/api/users/{id}

---

DELETE

/api/users/{id}

---

PATCH

/api/users/{id}/status

---

## Role API

GET

/api/roles

POST

/api/roles

PUT

/api/roles/{id}

DELETE

/api/roles/{id}

---

## Permission API

GET

/api/permissions

POST

/api/permissions

PUT

/api/permissions/{id}

DELETE

/api/permissions/{id}

---

# 12.9 Validation Rules

Users

□ Username duy nhất

□ CCCD không trùng

□ Email hợp lệ

□ Số điện thoại hợp lệ

□ Vai trò bắt buộc

---

Roles

□ Role Name duy nhất

□ Không được xóa Role đang sử dụng

---

Permissions

□ Permission Key duy nhất

□ Không được trùng Route

---

# 12.10 Folder Structure

```text
Application/

Users/

Roles/

Permissions/

Audit/

Infrastructure/

Repositories/

Identity/

Persistence/

API/

Controllers/

UsersController

RolesController

PermissionsController
```

---

# 12.11 Coding Tasks

Triển khai:

- DTO
- Validator
- AutoMapper
- Repository
- Service
- Controller
- Exception
- Logging
- Swagger

---

# 12.12 Testing Tasks

Unit Test

□ UserService

□ RoleService

□ PermissionService

---

Integration Test

□ User CRUD

□ Role CRUD

□ Permission CRUD

□ Authorization

---

Security Test

□ Role Access

□ Permission Access

□ Unauthorized Access

---

# 12.13 Documentation Tasks

Cập nhật:

- PROJECT_PROGRESS.md
- AI_HANDOVER.md
- API_SPEC.md
- CHANGELOG
- USER_GUIDE.md

---

# 12.14 Deliverables

Backend

□ User Module

□ Role Module

□ Permission Module

□ Audit Module

---

Database

□ AuditLogs

□ UserRoles

□ RolePermissions

---

API

□ User API

□ Role API

□ Permission API

---

Documentation

□ Swagger

□ Sprint Report

---

# 12.15 Sequence Flow

```text
Administrator

↓

Create User

↓

Assign Role

↓

Save Database

↓

Audit Log

↓

Response

↓

Dashboard Refresh
```

---

# 12.16 Acceptance Criteria

Users

□ CRUD hoạt động.

□ Search hoạt động.

□ Filter hoạt động.

□ Pagination hoạt động.

---

Roles

□ CRUD hoạt động.

□ Role Assignment đúng.

---

Permissions

□ Permission Mapping đúng.

□ Authorization đúng.

---

Audit

□ Ghi Log đầy đủ.

---

Documentation

□ Swagger cập nhật.

□ API cập nhật.

□ Sprint Report hoàn chỉnh.

---

# 12.17 Risks

Các rủi ro:

- Trùng Username.

- Sai Permission.

- Sai Authorization.

- Thiếu Audit Log.

- Role Mapping sai.

---

# 12.18 Definition of Done

□ User CRUD hoàn chỉnh.

□ Role CRUD hoàn chỉnh.

□ Permission CRUD hoàn chỉnh.

□ Audit Log hoạt động.

□ Unit Test Pass.

□ Integration Test Pass.

□ Swagger hoàn chỉnh.

□ Documentation cập nhật.

□ Được Project Owner nghiệm thu.

---

# 12.19 Sprint Output

Sau Sprint 03, hệ thống sẽ có:

- User Management
- Role Management
- Permission Management
- Audit Logging
- Authorization Matrix
- User Profile Management

Sprint này hoàn thiện nền tảng quản trị trước khi bước sang các module nghiệp vụ của AnSinhSo.

---

# 12.20 AI Development Package

## AI Input Documents

Antygravity AI chỉ cần đọc các tài liệu sau để triển khai Sprint 03:

### Bootstrap

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_AI_HANDOVER.md

### Enterprise Standards

- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md

### Core Documents

- DATABASE_DESIGN.md
- API_SPEC.md
- BUSINESS_RULES.md

### Sprint Documents

- Sprint 01
- Sprint 02
- Sprint 03

---

## AI Expected Output

AI phải sinh:

□ SQL Scripts

□ Entity Classes

□ Repository Pattern

□ Services

□ DTOs

□ Validators

□ Controllers

□ Swagger

□ Unit Tests

□ Integration Tests

□ Documentation Update

Không được tạo thêm chức năng ngoài phạm vi Sprint 03.

---

# End of Sprint 03

Sprint tiếp theo:

Sprint 04 – Master Data Management
````
````md
# ============================================================================
# 13. SPRINT 04 – MASTER DATA MANAGEMENT
# ============================================================================

## 13.1 Sprint Information

| Thuộc tính | Giá trị |
|------------|----------|
| Sprint ID | Sprint 04 |
| Sprint Name | Master Data Management |
| Priority | Critical |
| Duration | 2 Weeks |
| Status | Planned |
| Depends On | Sprint 03 |
| Milestone | Business Foundation |

---

# 13.2 Sprint Goal

Xây dựng toàn bộ hệ thống quản lý dữ liệu danh mục (Master Data) làm nền tảng cho các module nghiệp vụ của AnSinhSo.

Toàn bộ dữ liệu dùng chung trong hệ thống phải được chuẩn hóa tại Sprint này.

Sau khi hoàn thành Sprint 04, các Sprint nghiệp vụ sẽ chỉ sử dụng dữ liệu từ Master Data, không được tự tạo dữ liệu danh mục riêng.

---

# 13.3 Business Context

Master Data là dữ liệu ít thay đổi nhưng được sử dụng xuyên suốt hệ thống.

Bao gồm:

- Địa bàn hành chính
- Thôn/Khu phố
- Nhóm đối tượng an sinh
- Chính sách trợ cấp
- Loại hộ gia đình
- Trạng thái hồ sơ
- Đơn vị chi trả
- Danh mục cấu hình

Đây là nền tảng cho các Sprint 05 → Sprint 13.

---

# 13.4 Business Flow

```text
Administrator

↓

Đăng nhập

↓

Quản lý Master Data

↓

Kiểm tra hợp lệ

↓

Lưu Database

↓

Audit Log

↓

Các Module nghiệp vụ sử dụng dữ liệu
```

---

# 13.5 User Stories

### User Story 01

As an Administrator

I want to manage administrative areas

So that households are assigned correctly.

---

### User Story 02

As an Administrator

I want to manage welfare categories

So that beneficiaries are classified consistently.

---

### User Story 03

As an Administrator

I want to manage allowance policies

So that benefit calculations follow regulations.

---

### User Story 04

As a System

I want every business module to reuse Master Data

So that duplicated information is eliminated.

---

# 13.6 Business Modules

Sprint này triển khai:

- Administrative Area Management
- Village Management
- Welfare Category Management
- Policy Management
- Household Type Management
- Status Management
- Configuration Management

---

# 13.7 Database Mapping

Các bảng:

```text
DiaBan

↓

Thon

↓

NhomDoiTuong

↓

ChinhSachTroCap

↓

LoaiHoGiaDinh

↓

TrangThaiHoSo

↓

SystemConfigurations
```

---

# 13.8 Entity Design

## DiaBan

- Id
- MaDiaBan
- TenDiaBan
- CapHanhChinh
- ParentId
- IsActive

---

## Thon

- Id
- MaThon
- TenThon
- DiaBanId
- IsActive

---

## NhomDoiTuong

- Id
- MaNhom
- TenNhom
- MoTa

---

## ChinhSachTroCap

- Id
- MaChinhSach
- TenChinhSach
- MucTroCap
- ChuKyChiTra
- IsActive

---

# 13.9 API Contract

## Administrative Area

GET     /api/administrative-areas

GET     /api/administrative-areas/{id}

POST    /api/administrative-areas

PUT     /api/administrative-areas/{id}

DELETE  /api/administrative-areas/{id}

---

## Villages

GET

POST

PUT

DELETE

---

## Welfare Categories

GET

POST

PUT

DELETE

---

## Policies

GET

POST

PUT

DELETE

---

# 13.10 Validation Rules

Administrative Area

□ Mã không được trùng.

□ Tên bắt buộc.

□ Không được xóa khi đang sử dụng.

---

Village

□ Thuộc đúng địa bàn.

□ Không trùng tên trong cùng địa bàn.

---

Policy

□ Mức trợ cấp > 0.

□ Chu kỳ chi trả hợp lệ.

---

# 13.11 Folder Structure

Application/

    MasterData/

        AdministrativeAreas/

        Villages/

        WelfareCategories/

        Policies/

Infrastructure/

    Repositories/

        MasterData/

API/

    Controllers/

        AdministrativeAreasController

        VillagesController

        PoliciesController

---

# 13.12 Coding Tasks

Triển khai:

- Entity
- DTO
- Repository
- Service
- Validator
- AutoMapper
- Controller
- Swagger
- Exception Handling
- Logging

---

# 13.13 Testing Tasks

Unit Test

□ CRUD Administrative Area

□ CRUD Village

□ CRUD Welfare Category

□ CRUD Policy

---

Integration Test

□ API

□ Validation

□ Authorization

---

# 13.14 Documentation Tasks

Cập nhật:

- DATABASE_DESIGN.md
- API_SPEC.md
- PROJECT_PROGRESS.md
- AI_HANDOVER.md
- CHANGELOG

---

# 13.15 Deliverables

Database

□ Migration

□ Seed Data

□ Index

---

Backend

□ CRUD APIs

□ Services

□ Repository

---

Documentation

□ Swagger

□ Sprint Report

---

# 13.16 AI Execution Package

## Required Input

AI phải đọc:

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_AI_HANDOVER.md
- DATABASE_DESIGN.md
- BUSINESS_RULES.md
- API_SPEC.md
- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- Sprint 01
- Sprint 02
- Sprint 03
- Sprint 04

---

## Expected Output

AI phải sinh:

□ SQL Migration

□ Entity Classes

□ Repository Pattern

□ Service Layer

□ DTOs

□ Validators

□ Controllers

□ Swagger

□ Unit Tests

□ Integration Tests

---

## Out Of Scope

AI KHÔNG được:

- Tạo Household Module.
- Tạo Citizen Module.
- Tạo Payment Module.
- Tạo Dashboard.
- Tạo GIS.
- Tạo AI Assistant.
- Tạo Zalo OA.

Chỉ triển khai Master Data.

---

# 13.17 Review Checklist

Project Owner kiểm tra:

□ Cấu trúc Entity đúng.

□ Quan hệ Database đúng.

□ API đúng REST Standards.

□ Validation đầy đủ.

□ Logging hoạt động.

□ Swagger đầy đủ.

□ Unit Test Pass.

□ Documentation đồng bộ.

---

# 13.18 Definition of Done

□ Master Data CRUD hoàn chỉnh.

□ Database Migration thành công.

□ Seed Data hoạt động.

□ Swagger cập nhật.

□ Unit Test Pass.

□ Integration Test Pass.

□ Documentation cập nhật.

□ Được Project Owner phê duyệt.

---

# 13.19 Sprint Output

Sau Sprint 04, hệ thống có:

- Master Data Framework.
- Administrative Area Management.
- Village Management.
- Welfare Category Management.
- Policy Management.
- Configuration Foundation.

Đây là nền tảng bắt buộc cho mọi Sprint nghiệp vụ phía sau.

---

# End of Sprint 04

Tiếp theo:

Sprint 05 – Household Management
````
````md
# ============================================================================
# 14. SPRINT 05 – HOUSEHOLD MANAGEMENT
# ============================================================================

## 14.1 Sprint Information

| Thuộc tính | Giá trị |
|------------|----------|
| Sprint ID | Sprint 05 |
| Sprint Name | Household Management |
| Priority | Critical |
| Duration | 2 Weeks |
| Status | Planned |
| Depends On | Sprint 04 |
| Milestone | Core Business Module |

---

# 14.2 Sprint Goal

Triển khai hoàn chỉnh Module Quản lý Hộ gia đình.

Đây là module nghiệp vụ cốt lõi của hệ thống AnSinhSo.

Mọi dữ liệu trợ cấp, đối tượng an sinh, thống kê, bản đồ số, AI và Zalo OA đều tham chiếu đến Hộ gia đình.

Sprint này phải đảm bảo:

- Quản lý hộ gia đình.
- Quản lý chủ hộ.
- Quản lý địa chỉ.
- Liên kết địa bàn.
- Phân loại hộ.
- Trạng thái hộ.
- Tìm kiếm nhanh.
- Nhật ký thay đổi.

---

# 14.3 Business Context

Trong AnSinhSo, Hộ gia đình là thực thể trung tâm.

Quan hệ nghiệp vụ:

DiaBan
    ↓
Thon
    ↓
HoGiaDinh
    ↓
ThanhVien
    ↓
DoiTuongAnSinh
    ↓
ChiTraTroCap

Không được tạo dữ liệu đối tượng an sinh nếu chưa tồn tại Hộ gia đình.

---

# 14.4 Business Flow

```text
Cán bộ xã

↓

Tạo Hộ gia đình

↓

Kiểm tra địa bàn

↓

Kiểm tra chủ hộ

↓

Lưu dữ liệu

↓

Sinh Audit Log

↓

Sẵn sàng cho Sprint 06
```

---

# 14.5 Business Rules

BR-HH-001

Mỗi hộ có đúng một chủ hộ.

---

BR-HH-002

Mỗi hộ thuộc đúng một thôn.

---

BR-HH-003

Mỗi hộ thuộc đúng một địa bàn.

---

BR-HH-004

Không được xóa hộ đã phát sinh dữ liệu chi trả.

---

BR-HH-005

Không được trùng Mã hộ.

---

BR-HH-006

Không được trùng CCCD chủ hộ trong cùng thời điểm.

---

# 14.6 Database Mapping

Bảng chính:

HoGiaDinh

Quan hệ:

DiaBan (1)

↓

Thon (1)

↓

HoGiaDinh (N)

↓

ThanhVien (N)

---

# 14.7 Entity Design

Entity:

HoGiaDinh

Thuộc tính:

- Id
- MaHo
- TenChuHo
- CCCDChuHo
- SoDienThoai
- DiaChi
- DiaBanId
- ThonId
- LoaiHoId
- TrangThai
- GhiChu
- CreatedAt
- UpdatedAt
- CreatedBy
- UpdatedBy

---

# 14.8 DTO Design

CreateHouseholdDto

UpdateHouseholdDto

HouseholdResponseDto

HouseholdSummaryDto

HouseholdSearchDto

---

# 14.9 Repository Design

Interface

IHouseholdRepository

Các chức năng:

- GetAllAsync()
- GetByIdAsync()
- SearchAsync()
- ExistsAsync()
- CreateAsync()
- UpdateAsync()
- DeleteAsync()

---

# 14.10 Service Design

IHouseholdService

Bao gồm:

- Create Household
- Update Household
- Delete Household
- Search Household
- Get Detail
- Validate Business Rules

---

# 14.11 API Contract

GET

/api/households

---

GET

/api/households/{id}

---

POST

/api/households

---

PUT

/api/households/{id}

---

DELETE

/api/households/{id}

---

GET

/api/households/search

---

GET

/api/households/statistics

---

# 14.12 Validation Rules

□ Mã hộ bắt buộc.

□ Mã hộ không trùng.

□ Chủ hộ bắt buộc.

□ CCCD hợp lệ.

□ Điện thoại hợp lệ.

□ Địa bàn bắt buộc.

□ Thôn bắt buộc.

□ Loại hộ bắt buộc.

---

# 14.13 Folder Structure

Application/

Households/

Commands/

Queries/

Validators/

DTOs/

Infrastructure/

Repositories/

Households/

API/

Controllers/

HouseholdsController

---

# 14.14 Coding Tasks

Triển khai:

□ Entity

□ EF Configuration

□ Repository

□ Service

□ DTO

□ Validator

□ AutoMapper

□ Controller

□ Swagger

□ Logging

□ Exception Handling

---

# 14.15 Testing Tasks

Unit Test

□ Create

□ Update

□ Delete

□ Search

□ Validation

---

Integration Test

□ CRUD

□ API

□ Authorization

---

Performance Test

□ Search

□ Pagination

---

# 14.16 Documentation Tasks

Cập nhật:

- DATABASE_DESIGN.md
- API_SPEC.md
- BUSINESS_RULES.md
- PROJECT_PROGRESS.md
- AI_HANDOVER.md

---

# 14.17 AI Execution Package

## Required Input

AI phải đọc:

- Bootstrap Documents
- Database Design
- Business Rules
- API Specification
- Coding Standards
- Sprint 01 → Sprint 05

---

## Expected Output

AI phải sinh:

□ SQL Migration

□ Entity

□ Repository

□ Service

□ DTO

□ Validator

□ AutoMapper

□ Controller

□ Swagger

□ Unit Test

□ Integration Test

---

## Out Of Scope

AI KHÔNG được tạo:

- Thành viên hộ.
- Đối tượng an sinh.
- GIS.
- Dashboard.
- AI.
- Zalo OA.

Chỉ triển khai Household Management.

---

# 14.18 Quality Checklist

□ Build thành công.

□ Không có Compile Error.

□ Migration chạy thành công.

□ API đúng REST.

□ Validation đầy đủ.

□ Logging hoạt động.

□ Swagger đầy đủ.

□ Unit Test Pass.

□ Integration Test Pass.

---

# 14.19 Sprint Deliverables

Sau Sprint 05 phải có:

- Household Module.
- CRUD API.
- Search API.
- Statistics API.
- Migration.
- Documentation Update.
- Test Report.

---

# 14.20 Definition of Done

Sprint chỉ được đóng khi:

□ Toàn bộ Business Rules được hiện thực.

□ CRUD hoạt động.

□ Search hoạt động.

□ Migration thành công.

□ Documentation cập nhật.

□ Được Project Owner nghiệm thu.

---

# End of Sprint 05

Tiếp theo:

Sprint 06 – Citizen & Household Member Management
````
```md
# ============================================================================
# 15. SPRINT 06 – HOUSEHOLD MEMBER MANAGEMENT
# ============================================================================
#
# Module            : Household Member Management
# Sprint            : Sprint 06
# Priority          : Critical
# Duration          : 2 Weeks
# Depends On        : Sprint 05
# Architecture      : Clean Architecture
# Framework         : ASP.NET Core 8
# Database          : SQL Server 2022
#
# ============================================================================

# 15.1 Sprint Overview

## Sprint Goal

Triển khai Module Quản lý Thành viên Hộ gia đình.

Module này là nền tảng cho:

- Đối tượng an sinh
- Chính sách trợ cấp
- AI Prediction
- GIS
- Dashboard
- Báo cáo thống kê

Sau Sprint này hệ thống phải quản lý được toàn bộ thành viên thuộc từng hộ.

---

# 15.2 Business Context

Quan hệ nghiệp vụ:

DiaBan
    ↓
Thon
    ↓
HoGiaDinh
    ↓
ThanhVienHoGiaDinh
    ↓
DoiTuongAnSinh
    ↓
HoSoTroCap

Một thành viên chỉ thuộc một hộ tại một thời điểm.

Lịch sử chuyển hộ phải được lưu.

---

# 15.3 Business Objectives

Hoàn thành:

- Quản lý thành viên
- Quan hệ với chủ hộ
- Quan hệ nhân khẩu
- Thông tin CCCD
- Bảo hiểm y tế
- Bảo hiểm xã hội
- Trạng thái cư trú
- Lịch sử chuyển hộ

---

# 15.4 Use Cases

### UC01

Thêm thành viên.

---

### UC02

Cập nhật thông tin.

---

### UC03

Chuyển hộ.

---

### UC04

Ngừng cư trú.

---

### UC05

Tra cứu thành viên.

---

### UC06

Xem lịch sử.

---

# 15.5 User Stories

As Commune Officer

I want to manage household members

So that welfare beneficiaries are managed accurately.

---

As Administrator

I want to track household history

So that data integrity is maintained.

---

# 15.6 Functional Requirements

FR-001

CRUD Household Members.

---

FR-002

Search.

---

FR-003

Filter.

---

FR-004

Pagination.

---

FR-005

Export Excel.

---

FR-006

Import Excel.

---

FR-007

Audit Logging.

---

FR-008

Soft Delete.

---

# 15.7 Non-functional Requirements

- Response <2s

- UTF-8

- Unicode

- Audit

- Logging

- Security

- Backup

---

# 15.8 Business Rules

BR-HM-001

Một thành viên chỉ thuộc một hộ.

---

BR-HM-002

Một hộ có đúng một chủ hộ.

---

BR-HM-003

Không trùng CCCD.

---

BR-HM-004

Ngày sinh hợp lệ.

---

BR-HM-005

Không xóa dữ liệu nếu đã phát sinh chi trả.

---

# 15.9 Database Mapping

Tables

HoGiaDinh

↓

ThanhVien

↓

QuanHeChuHo

↓

NgheNghiep

↓

BaoHiem

↓

LichSuHoKhau

---

# 15.10 Entity Design

ThanhVien

- Id
- HoGiaDinhId
- HoTen
- CCCD
- GioiTinh
- NgaySinh
- QuanHeChuHo
- SoDienThoai
- DiaChi
- NgheNghiep
- BHYT
- BHXH
- TrangThai
- IsDeleted
- CreatedAt
- UpdatedAt

---

# 15.11 DTO Design

CreateMemberDto

UpdateMemberDto

MemberDto

MemberSearchDto

MemberSummaryDto

---

# 15.12 Repository Design

IHouseholdMemberRepository

CreateAsync()

UpdateAsync()

DeleteAsync()

SearchAsync()

ExistsAsync()

GetHistoryAsync()

---

# 15.13 Service Design

IHouseholdMemberService

Create

Update

Delete

Transfer Household

Search

Statistics

Validation

---

# 15.14 Controller Design

HouseholdMembersController

GET

POST

PUT

DELETE

SEARCH

EXPORT

IMPORT

HISTORY

---

# 15.15 API Contract

GET

/api/household-members

---

GET

/api/household-members/{id}

---

POST

/api/household-members

---

PUT

/api/household-members/{id}

---

DELETE

/api/household-members/{id}

---

POST

/api/household-members/import

---

GET

/api/household-members/export

---

GET

/api/household-members/history/{id}

---

# 15.16 Validation Rules

□ CCCD không trùng.

□ Họ tên bắt buộc.

□ Ngày sinh hợp lệ.

□ Quan hệ chủ hộ hợp lệ.

□ Hộ tồn tại.

□ Không vượt giới hạn dữ liệu.

---

# 15.17 Security Requirements

Role:

Admin

CanBoXa

---

Permission

Member.Read

Member.Create

Member.Update

Member.Delete

Member.Export

Member.Import

---

JWT Required

Authorization Required

Audit Required

---

# 15.18 Performance Requirements

Search

<2 giây

---

Import

10.000 dòng

<60 giây

---

Export

10.000 dòng

<30 giây

---

# 15.19 Coding Tasks

Entity

Repository

Service

DTO

Validator

Controller

Swagger

Logging

Migration

Seed

---

# 15.20 Testing Tasks

Unit Test

CRUD

Validation

Business Rules

---

Integration Test

API

Authorization

Migration

---

Performance Test

Import

Export

Search

---

# 15.21 Documentation Tasks

Cập nhật:

DATABASE_DESIGN.md

API_SPEC.md

BUSINESS_RULES.md

PROJECT_PROGRESS.md

AI_HANDOVER.md

---

# 15.22 AI Execution Package

## AI Required Input

- Bootstrap Documents
- Standards
- Sprint 01 → Sprint 06

---

## AI Output

SQL

Entity

Repository

Service

Controller

Swagger

Migration

Unit Test

Integration Test

README Update

---

## AI Scope Boundary

Không triển khai:

GIS

Dashboard

AI

Payment

Notification

Zalo OA

---

# 15.23 Review Checklist

□ Build Success

□ Migration Success

□ Swagger OK

□ API OK

□ Logging OK

□ Validation OK

□ Business Rules OK

□ Documentation Updated

---

# 15.24 Definition of Done

□ CRUD hoàn chỉnh

□ Search hoạt động

□ Import/Export hoạt động

□ Unit Test Pass

□ Integration Test Pass

□ Documentation hoàn chỉnh

□ Sprint được nghiệm thu

---

# 15.25 Sprint Deliverables

Backend Module

Database Migration

REST API

Swagger

Test Report

Documentation Update

---

# 15.26 Git Commit Convention

feature/sprint-06-household-members

Commit Message:

feat(member): implement household member management module

---

# 15.27 Sprint Close Checklist

□ Code Review

□ AI Review

□ Security Review

□ Documentation Review

□ Project Owner Approval

---

# Sprint Output

Sau Sprint 06 hệ thống có khả năng quản lý đầy đủ thành viên hộ gia đình, làm nền tảng trực tiếp cho Sprint 07 – Social Welfare Beneficiary Management.

# ============================================================================
# END OF SPRINT 06
# ============================================================================
```
```md
# ============================================================================
# 16. SPRINT 07 – SOCIAL WELFARE BENEFICIARY MANAGEMENT
# ============================================================================
#
# Module            : Social Welfare Beneficiary Management
# Sprint            : Sprint 07
# Priority          : Critical
# Duration          : 2 Weeks
# Depends On        : Sprint 06
# Architecture      : Clean Architecture
# Framework         : ASP.NET Core 8
# Database          : SQL Server 2022
#
# ============================================================================

# 16.1 Sprint Overview

## Sprint Goal

Triển khai Module Quản lý Đối tượng An sinh xã hội.

Đây là module nghiệp vụ trung tâm của toàn bộ hệ thống AnSinhSo.

Sau Sprint này, hệ thống phải quản lý được:

- Hồ sơ đối tượng an sinh
- Phân loại nhóm đối tượng
- Chính sách đang hưởng
- Trạng thái hưởng
- Lịch sử thay đổi
- Theo dõi hồ sơ

---

# 16.2 Business Context

Quan hệ nghiệp vụ:

DiaBan
    ↓
Thon
    ↓
HoGiaDinh
    ↓
ThanhVien
    ↓
DoiTuongAnSinh
    ↓
ChinhSachTroCap
    ↓
ChiTraTroCap

Mỗi đối tượng an sinh phải gắn với một thành viên hộ gia đình hợp lệ.

---

# 16.3 Business Objectives

Hoàn thành:

- Quản lý hồ sơ đối tượng
- Phân loại nhóm đối tượng
- Gán chính sách trợ cấp
- Theo dõi trạng thái
- Quản lý lịch sử thay đổi
- Tra cứu hồ sơ

---

# 16.4 Scope

Bao gồm:

- CRUD Đối tượng an sinh
- Tìm kiếm
- Bộ lọc
- Phân loại
- Theo dõi trạng thái
- Nhật ký thay đổi

---

# 16.5 Out Of Scope

Không triển khai:

- Chi trả trợ cấp
- Dashboard
- GIS
- AI Analytics
- AI Assistant
- Zalo OA

Các chức năng này sẽ được triển khai ở các Sprint tiếp theo.

---

# 16.6 Stakeholders

- Quản trị hệ thống
- Cán bộ Lao động - Thương binh và Xã hội
- Lãnh đạo UBND xã
- Người dân (chỉ được xem thông tin của bản thân)

---

# 16.7 Use Cases

UC01 - Tạo hồ sơ đối tượng.

UC02 - Cập nhật hồ sơ.

UC03 - Chuyển trạng thái.

UC04 - Gán chính sách trợ cấp.

UC05 - Tra cứu hồ sơ.

UC06 - Xem lịch sử thay đổi.

---

# 16.8 User Stories

As Commune Officer

I want to manage beneficiary profiles

So that welfare information is accurate and up to date.

---

As Leader

I want to monitor beneficiary statistics

So that decisions can be made based on reliable data.

---

# 16.9 Functional Requirements

FR-001 CRUD hồ sơ.

FR-002 Quản lý trạng thái.

FR-003 Gán nhóm đối tượng.

FR-004 Gán chính sách trợ cấp.

FR-005 Tìm kiếm.

FR-006 Lọc dữ liệu.

FR-007 Nhật ký thay đổi.

FR-008 Soft Delete.

---

# 16.10 Non-functional Requirements

- Response < 2 giây
- Audit Logging
- Unicode UTF-8
- Soft Delete
- Transaction Safety
- Backup Compatibility

---

# 16.11 Business Rules

BR-BEN-001

Một thành viên chỉ có một hồ sơ đối tượng đang hoạt động.

BR-BEN-002

Không được hưởng đồng thời các chính sách xung đột (quy tắc chi tiết sẽ lấy từ BUSINESS_RULES.md).

BR-BEN-003

Mọi thay đổi trạng thái phải ghi Audit Log.

BR-BEN-004

Không được xóa hồ sơ đã phát sinh chi trả.

---

# 16.12 Database Mapping

Tables:

- DoiTuongAnSinh
- NhomDoiTuong
- ChinhSachTroCap
- HoGiaDinh
- ThanhVien
- AuditLogs

---

# 16.13 Entity Design

Entity: DoiTuongAnSinh

Thuộc tính chính:

- Id
- ThanhVienId
- NhomDoiTuongId
- ChinhSachTroCapId
- NgayBatDauHuong
- NgayKetThucHuong
- TrangThai
- GhiChu
- CreatedAt
- UpdatedAt
- CreatedBy
- UpdatedBy
- IsDeleted

---

# 16.14 DTO Design

- CreateBeneficiaryDto
- UpdateBeneficiaryDto
- BeneficiaryDetailDto
- BeneficiarySummaryDto
- BeneficiarySearchDto

---

# 16.15 Repository Design

IBeneficiaryRepository

- CreateAsync()
- UpdateAsync()
- DeleteAsync()
- SearchAsync()
- GetByIdAsync()
- ExistsAsync()

---

# 16.16 Service Design

IBeneficiaryService

- Create
- Update
- ChangeStatus
- AssignPolicy
- Search
- ValidateBusinessRules

---

# 16.17 Controller Design

BeneficiariesController

REST Endpoints

CRUD

Search

Status

Policy Assignment

History

---

# 16.18 API Contract

GET    /api/beneficiaries

GET    /api/beneficiaries/{id}

POST   /api/beneficiaries

PUT    /api/beneficiaries/{id}

PATCH  /api/beneficiaries/{id}/status

PATCH  /api/beneficiaries/{id}/policy

GET    /api/beneficiaries/search

GET    /api/beneficiaries/history/{id}

---

# 16.19 Sprint Output

Sau Sprint 07, hệ thống có:

- Module quản lý đối tượng an sinh hoàn chỉnh.
- API REST đầy đủ.
- Entity, Repository, Service, DTO, Controller.
- Migration và Seed Data.
- Swagger cập nhật.
- Unit Test và Integration Test.
- Tài liệu đồng bộ.

Sprint tiếp theo:

Sprint 08 – Welfare Policy & Payment Management.

# ============================================================================
# END OF SPRINT 07
# ============================================================================
```
```md
# ============================================================================
# 17. SPRINT 08 – WELFARE POLICY & PAYMENT MANAGEMENT
# ============================================================================
#
# Module            : Welfare Policy & Payment Management
# Sprint            : Sprint 08
# Priority          : Critical
# Duration          : 2 Weeks
# Depends On        : Sprint 07
# Architecture      : Clean Architecture
# Framework         : ASP.NET Core 8
# Database          : SQL Server 2022
#
# ============================================================================

# 17.1 Sprint Overview

## Sprint Goal

Triển khai Module Quản lý Chính sách và Chi trả Trợ cấp.

Sau Sprint này, hệ thống phải quản lý được:

- Chính sách trợ cấp
- Đợt chi trả
- Danh sách chi trả
- Lịch sử chi trả
- Trạng thái thanh toán
- Báo cáo chi trả

Sprint này là trung tâm của toàn bộ quy trình nghiệp vụ an sinh.

---

# 17.2 Business Context

Quan hệ nghiệp vụ

NhomDoiTuong
        ↓
ChinhSachTroCap
        ↓
DoiTuongAnSinh
        ↓
DotChiTra
        ↓
ChiTraTroCap
        ↓
BaoCaoThongKe

---

# 17.3 Business Objectives

Hoàn thành:

- Quản lý chính sách
- Quản lý đợt chi trả
- Lập danh sách chi trả
- Duyệt chi trả
- Ghi nhận kết quả
- Thống kê

---

# 17.4 Scope

Bao gồm

- Policy CRUD
- Payment Period CRUD
- Payment Processing
- Payment Approval
- Payment History
- Search
- Statistics

---

# 17.5 Out Of Scope

Không triển khai

- GIS
- Dashboard
- AI Analytics
- AI Assistant
- Zalo OA
- SMS Notification

---

# 17.6 Stakeholders

- Administrator
- Cán bộ Lao động - Thương binh và Xã hội
- Kế toán
- Lãnh đạo UBND
- Người dân (chỉ xem lịch sử của bản thân)

---

# 17.7 Use Cases

UC01 Quản lý chính sách.

UC02 Tạo đợt chi trả.

UC03 Sinh danh sách chi trả.

UC04 Duyệt danh sách.

UC05 Ghi nhận kết quả.

UC06 Tra cứu lịch sử.

UC07 Thống kê.

---

# 17.8 User Stories

As Welfare Officer

I want to create payment periods

So that beneficiaries receive support on schedule.

---

As Accountant

I want to confirm payment status

So that financial records are accurate.

---

# 17.9 Functional Requirements

FR-001 CRUD Policy

FR-002 CRUD Payment Period

FR-003 Generate Payment List

FR-004 Payment Approval

FR-005 Payment Confirmation

FR-006 Search

FR-007 Statistics

FR-008 Export Excel

---

# 17.10 Non-functional Requirements

- Transaction Safety
- Audit Logging
- Soft Delete
- Response < 3 giây
- Export tối đa 50.000 bản ghi

---

# 17.11 Business Rules

BR-PAY-001

Một đối tượng chỉ được nhận một khoản chi trả cho cùng một chính sách trong cùng một kỳ.

---

BR-PAY-002

Không được chỉnh sửa đợt chi trả sau khi đã duyệt.

---

BR-PAY-003

Mọi thao tác duyệt phải lưu Audit Log.

---

BR-PAY-004

Chỉ người có quyền mới được xác nhận hoàn tất chi trả.

---

# 17.12 Database Mapping

Tables

- ChinhSachTroCap
- DotChiTra
- ChiTraTroCap
- DoiTuongAnSinh
- AuditLogs

---

# 17.13 Entity Design

Entity: ChiTraTroCap

Thuộc tính

- Id
- DotChiTraId
- DoiTuongAnSinhId
- SoTien
- NgayChiTra
- TrangThai
- HinhThucChiTra
- NguoiXacNhan
- GhiChu
- CreatedAt
- UpdatedAt

---

# 17.14 DTO Design

- CreatePaymentDto
- UpdatePaymentDto
- PaymentDetailDto
- PaymentSummaryDto
- PaymentSearchDto

---

# 17.15 Repository Design

IPaymentRepository

- CreateAsync()
- UpdateAsync()
- ApproveAsync()
- SearchAsync()
- ExportAsync()

---

# 17.16 Service Design

IPaymentService

- GeneratePaymentList()
- ApprovePayment()
- ConfirmPayment()
- Search()
- Statistics()

---

# 17.17 Controller Design

PaymentsController

PoliciesController

PaymentPeriodsController

---

# 17.18 API Contract

GET    /api/payments

POST   /api/payments

PUT    /api/payments/{id}

PATCH  /api/payments/{id}/approve

PATCH  /api/payments/{id}/confirm

GET    /api/payments/history

GET    /api/payment-periods

POST   /api/payment-periods

---

# 17.19 Security Requirements

Roles

- Admin
- CanBoXa
- KeToan
- LanhDao

Permissions

- Payment.Read
- Payment.Create
- Payment.Approve
- Payment.Confirm
- Payment.Export

JWT Required

Audit Required

---

# 17.20 Performance Requirements

- Sinh danh sách 10.000 hồ sơ < 60 giây
- Tìm kiếm < 2 giây
- Xuất Excel 50.000 bản ghi < 90 giây

---

# 17.21 Coding Tasks

- Entity
- Repository
- Service
- DTO
- Validator
- Controller
- Swagger
- Logging
- Migration
- Seed Data

---

# 17.22 Testing Tasks

Unit Test

- Payment Generation
- Approval
- Confirmation
- Validation

Integration Test

- CRUD
- Approval Workflow
- Authorization

Performance Test

- Generate Payment
- Export Excel

---

# 17.23 Documentation Tasks

Cập nhật

- DATABASE_DESIGN.md
- BUSINESS_RULES.md
- API_SPEC.md
- PROJECT_PROGRESS.md
- AI_HANDOVER.md

---

# 17.24 AI Execution Package

## AI Required Input

- Bootstrap Documents
- Standards
- Sprint 01 → Sprint 08

---

## AI Expected Output

- SQL Migration
- Entity
- Repository
- Service
- Controller
- Swagger
- Unit Test
- Integration Test
- API Documentation

---

## Scope Boundary

Không triển khai:

- GIS
- Dashboard
- AI
- Zalo OA
- Notification Center

---

# 17.25 Review Checklist

□ Build thành công

□ Migration thành công

□ Approval Workflow đúng

□ Audit Log đầy đủ

□ Swagger hoàn chỉnh

□ Unit Test Pass

□ Integration Test Pass

---

# 17.26 Definition of Done

□ CRUD hoàn chỉnh

□ Sinh danh sách chi trả

□ Duyệt chi trả

□ Xác nhận chi trả

□ Báo cáo hoạt động

□ Documentation cập nhật

□ Sprint được nghiệm thu

---

# 17.27 Sprint Deliverables

- Welfare Policy Module
- Payment Management Module
- Approval Workflow
- REST API
- Database Migration
- Swagger
- Test Report

---

# 17.28 Git Commit Convention

Branch

feature/sprint-08-payment-management

Commit

feat(payment): implement welfare payment management module

---

# 17.29 Sprint Close Checklist

□ Code Review

□ AI Review

□ Security Review

□ Documentation Review

□ Product Owner Approval

---

# Sprint Output

Sau Sprint 08, hệ thống có khả năng quản lý đầy đủ:

- Chính sách trợ cấp
- Đợt chi trả
- Danh sách chi trả
- Quy trình duyệt
- Lịch sử chi trả
- Báo cáo thống kê

Đây là nền tảng cho Sprint 09 – Reports & Dashboard.

# ============================================================================
# END OF SPRINT 08
# ============================================================================
```
```md
# ============================================================================
# 18. SPRINT 09 – REPORTS & DASHBOARD
# ============================================================================
#
# Module            : Reports & Dashboard
# Sprint            : Sprint 09
# Priority          : High
# Duration          : 2 Weeks
# Depends On        : Sprint 08
# Architecture      : Clean Architecture
# Framework         : ASP.NET Core 8
# Database          : SQL Server 2022
#
# ============================================================================

# 18.1 Sprint Overview

## Sprint Goal

Triển khai hệ thống Dashboard và Báo cáo phục vụ công tác quản lý an sinh xã hội.

Sprint này giúp lãnh đạo và cán bộ xã có thể theo dõi dữ liệu theo thời gian thực, thống kê, phân tích và xuất báo cáo phục vụ điều hành.

Sau Sprint này hệ thống phải hỗ trợ:

- Dashboard tổng quan
- Dashboard theo địa bàn
- Dashboard theo nhóm đối tượng
- Dashboard theo chính sách
- Dashboard chi trả
- Dashboard thời gian
- Báo cáo Excel
- Báo cáo PDF

---

# 18.2 Business Context

Nguồn dữ liệu

HoGiaDinh
        ↓
ThanhVien
        ↓
DoiTuongAnSinh
        ↓
ChinhSachTroCap
        ↓
ChiTraTroCap
        ↓
Dashboard
        ↓
Reports

Dashboard chỉ đọc dữ liệu, không chỉnh sửa dữ liệu nghiệp vụ.

---

# 18.3 Business Objectives

Hoàn thành:

- Executive Dashboard
- Operational Dashboard
- Statistical Reports
- KPI Dashboard
- Export Reports
- Printable Reports

---

# 18.4 Scope

Bao gồm:

- Dashboard tổng quan
- Dashboard địa bàn
- Dashboard trợ cấp
- Dashboard chi trả
- Dashboard dân số
- Dashboard hộ gia đình
- Export Excel
- Export PDF

---

# 18.5 Out Of Scope

Không triển khai:

- AI Analytics
- GIS
- AI Assistant
- Zalo OA Notification

---

# 18.6 Stakeholders

- Chủ tịch UBND xã
- Phó Chủ tịch
- Cán bộ Lao động - TB&XH
- Kế toán
- Văn phòng UBND

---

# 18.7 Use Cases

UC01 Xem Dashboard tổng quan.

UC02 Lọc Dashboard.

UC03 Xem Dashboard theo địa bàn.

UC04 Xuất Excel.

UC05 Xuất PDF.

UC06 In báo cáo.

UC07 So sánh theo thời gian.

---

# 18.8 User Stories

As Commune Leader

I want to view welfare statistics

So that I can make informed decisions.

---

As Welfare Officer

I want to export reports

So that monthly reporting is simplified.

---

# 18.9 Functional Requirements

FR-001 Dashboard Overview

FR-002 Dashboard by Administrative Area

FR-003 Dashboard by Village

FR-004 Dashboard by Beneficiary Group

FR-005 Dashboard by Policy

FR-006 Dashboard by Payment

FR-007 Export Excel

FR-008 Export PDF

FR-009 KPI Cards

FR-010 Charts

---

# 18.10 Non-functional Requirements

- Dashboard tải <3 giây
- Hỗ trợ tối thiểu 100 người dùng đồng thời
- Dữ liệu cập nhật theo thời gian thực hoặc theo chu kỳ cấu hình
- Tối ưu truy vấn SQL

---

# 18.11 Business Rules

BR-DASH-001

Chỉ hiển thị dữ liệu theo phạm vi quyền truy cập.

---

BR-DASH-002

Lãnh đạo xem toàn xã.

---

BR-DASH-003

Cán bộ chỉ xem dữ liệu được phân quyền.

---

BR-DASH-004

Mọi báo cáo phải lấy từ dữ liệu đã được phê duyệt.

---

# 18.12 Database Mapping

Đọc dữ liệu từ:

- HoGiaDinh
- ThanhVien
- DoiTuongAnSinh
- ChinhSachTroCap
- DotChiTra
- ChiTraTroCap

Không tạo bảng nghiệp vụ mới.

Có thể sử dụng:

- SQL View
- Materialized Summary Table (nếu cần tối ưu)
- Stored Procedure cho báo cáo lớn

---

# 18.13 Dashboard Components

Executive KPI

- Tổng số hộ
- Tổng số nhân khẩu
- Tổng số đối tượng
- Tổng số tiền trợ cấp
- Số hồ sơ mới
- Hồ sơ chờ duyệt

---

Biểu đồ

- Cột
- Đường
- Tròn
- Khu vực
- Heat Map (chuẩn bị cho Sprint GIS)

---

# 18.14 API Contract

GET

/api/dashboard/overview

GET

/api/dashboard/beneficiaries

GET

/api/dashboard/payments

GET

/api/dashboard/geography

GET

/api/dashboard/statistics

GET

/api/reports/excel

GET

/api/reports/pdf

---

# 18.15 Service Design

IDashboardService

IReportService

Bao gồm:

- Dashboard Summary
- Dashboard KPI
- Statistics
- Export Excel
- Export PDF
- Trend Analysis

---

# 18.16 Security Requirements

Role:

Admin

LanhDao

CanBoXa

Permission

Dashboard.Read

Report.Export

Report.Print

JWT Required

Audit Required

---

# 18.17 Performance Requirements

Dashboard

<3 giây

---

Export Excel

100.000 dòng

<120 giây

---

Export PDF

<30 giây

---

# 18.18 Coding Tasks

- Dashboard API
- Report API
- SQL Views
- Stored Procedures
- DTO
- Services
- Export Engine
- Swagger
- Logging

---

# 18.19 Testing Tasks

Unit Test

- Dashboard
- KPI
- Reports

Integration Test

- API
- Authorization
- Export

Performance Test

- Dashboard Load
- Large Report Export

---

# 18.20 Documentation Tasks

Cập nhật:

- API_SPEC.md
- DATABASE_DESIGN.md
- BUSINESS_RULES.md
- UI_REQUIREMENTS.md
- PROJECT_PROGRESS.md

---

# 18.21 AI Execution Package

## AI Required Input

- Bootstrap Documents
- Standards
- Sprint 01 → Sprint 09

---

## AI Expected Output

□ Dashboard API

□ Report API

□ KPI Engine

□ SQL Views

□ Export Excel

□ Export PDF

□ Swagger

□ Unit Tests

---

## Scope Boundary

Không triển khai:

- GIS
- AI
- Zalo OA
- Notification Center

---

# 18.22 Review Checklist

□ Dashboard đúng KPI

□ Báo cáo đúng số liệu

□ Export thành công

□ Performance đạt yêu cầu

□ Swagger đầy đủ

□ Unit Test Pass

□ Documentation cập nhật

---

# 18.23 Definition of Done

□ Dashboard hoàn chỉnh

□ Báo cáo hoạt động

□ Export Excel/PDF thành công

□ API được kiểm thử

□ Documentation cập nhật

□ Sprint được nghiệm thu

---

# 18.24 Sprint Deliverables

- Dashboard Module
- Reporting Module
- KPI Engine
- Export Engine
- REST API
- Swagger
- Test Report

---

# 18.25 Git Commit Convention

Branch

feature/sprint-09-dashboard

Commit

feat(dashboard): implement reports and dashboard module

---

# 18.26 Sprint Close Checklist

□ Code Review

□ AI Review

□ Security Review

□ Performance Review

□ Product Owner Approval

---

# Sprint Output

Sau Sprint 09, hệ thống có:

- Dashboard điều hành
- Dashboard nghiệp vụ
- Hệ thống KPI
- Báo cáo thống kê
- Xuất Excel/PDF
- Nền tảng dữ liệu cho Sprint 10 – GIS Digital Map

# ============================================================================
# END OF SPRINT 09
# ============================================================================
```
```md
# ============================================================================
# 19. SPRINT 10 – GIS DIGITAL MAP INTEGRATION
# ============================================================================
#
# Module            : GIS Digital Map Integration
# Sprint            : Sprint 10
# Priority          : Very High
# Duration          : 3 Weeks
# Depends On        : Sprint 09
# Architecture      : Clean Architecture
# Framework         : ASP.NET Core 8 + LeafletJS
# Database          : SQL Server 2022
#
# ============================================================================

# 19.1 Sprint Overview

## Sprint Goal

Triển khai hệ thống Bản đồ số (GIS) phục vụ quản lý an sinh xã hội.

Sau Sprint này, hệ thống phải hiển thị trực quan:

- Địa bàn hành chính
- Thôn/Khu phố
- Hộ gia đình
- Đối tượng an sinh
- Điểm chi trả
- Thống kê theo bản đồ

---

# 19.2 Business Context

GIS là trung tâm trực quan hóa dữ liệu của AnSinhSo.

Quan hệ dữ liệu:

DiaBan
        ↓
Thon
        ↓
HoGiaDinh
        ↓
ToaDoGPS
        ↓
BanDoSo
        ↓
Dashboard
        ↓
AI Analytics

---

# 19.3 Business Objectives

Hoàn thành:

- Digital Map
- GIS Layer Management
- Marker Management
- Spatial Search
- Geographic Statistics
- Heatmap Foundation

---

# 19.4 Scope

Bao gồm:

- Hiển thị bản đồ
- Marker hộ gia đình
- Marker đối tượng an sinh
- Marker điểm chi trả
- Tìm kiếm trên bản đồ
- Lọc dữ liệu theo địa bàn
- Popup thông tin
- Legend bản đồ

---

# 19.5 Out Of Scope

Không triển khai:

- AI Prediction
- Chatbot AI
- Zalo OA
- Drone Mapping
- 3D GIS

---

# 19.6 Stakeholders

- Chủ tịch UBND xã
- Cán bộ Lao động - TB&XH
- Cán bộ Địa chính
- Cán bộ Văn phòng
- Quản trị hệ thống

---

# 19.7 Use Cases

UC01 Xem bản đồ toàn xã.

UC02 Xem theo thôn.

UC03 Tìm hộ trên bản đồ.

UC04 Tìm đối tượng an sinh.

UC05 Xem chi tiết Marker.

UC06 Thống kê theo vùng.

UC07 Bật/Tắt lớp dữ liệu.

---

# 19.8 User Stories

As Commune Officer

I want to locate households on a digital map

So that field verification becomes easier.

---

As Commune Leader

I want to visualize welfare distribution

So that decisions are based on geographic information.

---

# 19.9 Functional Requirements

FR-001 Display Administrative Map

FR-002 Household Markers

FR-003 Beneficiary Markers

FR-004 Payment Location Markers

FR-005 Layer Control

FR-006 Search by Location

FR-007 Filter by Village

FR-008 Popup Information

FR-009 Marker Clustering

FR-010 GIS Statistics

---

# 19.10 Non-functional Requirements

- Map tải <5 giây
- Marker cập nhật <2 giây
- Hỗ trợ tối thiểu 20.000 Marker
- Responsive Desktop/Mobile
- Cache Tile Map

---

# 19.11 Business Rules

BR-GIS-001

Mỗi hộ có tối đa một tọa độ GPS chính.

---

BR-GIS-002

Marker chỉ hiển thị với dữ liệu hợp lệ.

---

BR-GIS-003

Người dùng chỉ xem dữ liệu trong phạm vi được phân quyền.

---

BR-GIS-004

Thay đổi tọa độ phải ghi Audit Log.

---

# 19.12 Database Mapping

Tables

- DiaBan
- Thon
- HoGiaDinh
- DoiTuongAnSinh
- GPSCoordinates
- AuditLogs

---

# 19.13 GIS Layer Design

Administrative Layer

Village Layer

Household Layer

Beneficiary Layer

Payment Layer

Heatmap Layer (Reserved)

AI Prediction Layer (Reserved)

---

# 19.14 API Contract

GET

/api/gis/map

GET

/api/gis/markers

GET

/api/gis/layers

GET

/api/gis/statistics

POST

/api/gis/coordinates

PUT

/api/gis/coordinates/{id}

---

# 19.15 Service Design

IGisService

IGeoLocationService

IGisStatisticsService

Bao gồm:

- Marker Management
- Layer Management
- Coordinate Validation
- Spatial Search
- Geographic Statistics

---

# 19.16 UI Components

- Interactive Map
- Layer Switcher
- Marker Popup
- Search Box
- Filter Panel
- Legend
- Zoom Control
- Scale Control

---

# 19.17 Security Requirements

Roles

- Admin
- LanhDao
- CanBoXa

Permissions

- GIS.Read
- GIS.UpdateCoordinate
- GIS.Export

JWT Required

Audit Required

---

# 19.18 Performance Requirements

- 20.000 Marker
- Cluster Rendering <5 giây
- Spatial Search <2 giây
- API Response <1 giây

---

# 19.19 Coding Tasks

- GIS Module
- Coordinate Entity
- GIS Services
- Leaflet Integration
- REST API
- Marker Cluster
- Spatial Search
- Swagger

---

# 19.20 Testing Tasks

Unit Test

- Coordinate Validation
- Marker Service
- Layer Service

Integration Test

- GIS API
- Security
- Search

Performance Test

- Marker Load
- Cluster
- Rendering

---

# 19.21 Documentation Tasks

Cập nhật:

- DATABASE_DESIGN.md
- API_SPEC.md
- UI_REQUIREMENTS.md
- PROJECT_PROGRESS.md
- AI_HANDOVER.md

---

# 19.22 AI Execution Package

## AI Required Input

- Bootstrap Documents
- Standards
- Sprint 01 → Sprint 10

---

## AI Expected Output

□ GIS Module

□ Leaflet Integration

□ Marker API

□ Spatial Search

□ Layer Control

□ Swagger

□ Unit Test

---

## Scope Boundary

Không triển khai:

- AI Prediction
- AI Chatbot
- Zalo OA
- Notification

---

# 19.23 Review Checklist

□ Bản đồ hiển thị đúng

□ Marker đúng dữ liệu

□ Layer hoạt động

□ Popup chính xác

□ Search hoạt động

□ Swagger đầy đủ

□ Unit Test Pass

---

# 19.24 Definition of Done

□ GIS hoạt động

□ Marker hiển thị đúng

□ Spatial Search hoàn chỉnh

□ API kiểm thử thành công

□ Documentation cập nhật

□ Sprint được nghiệm thu

---

# 19.25 Sprint Deliverables

- GIS Module
- Interactive Digital Map
- Marker Management
- Spatial Search
- REST API
- Swagger
- Test Report

---

# 19.26 Git Commit Convention

Branch

feature/sprint-10-gis

Commit

feat(gis): implement digital map integration

---

# 19.27 Sprint Close Checklist

□ Code Review

□ GIS Review

□ Security Review

□ Performance Review

□ Product Owner Approval

---

# Sprint Output

Sau Sprint 10, hệ thống có khả năng:

- Hiển thị bản đồ số toàn xã
- Quản lý vị trí hộ gia đình
- Quản lý vị trí đối tượng an sinh
- Thống kê theo không gian
- Chuẩn bị dữ liệu cho AI Analytics (Sprint 11)

# ============================================================================
# END OF SPRINT 10
# ============================================================================
```
```md
# ============================================================================
# 20. SPRINT 11 – AI ANALYTICS & PREDICTION
# ============================================================================
#
# Module            : AI Analytics & Prediction
# Sprint            : Sprint 11
# Priority          : Very High
# Duration          : 3 Weeks
# Depends On        : Sprint 10
# Architecture      : Clean Architecture + AI Service Layer
# Framework         : ASP.NET Core 8
# Database          : SQL Server 2022
#
# ============================================================================

# 20.1 Sprint Overview

## Sprint Goal

Triển khai nền tảng AI Analytics phục vụ phân tích dữ liệu an sinh xã hội.

AI đóng vai trò:

- Phân tích dữ liệu
- Phát hiện bất thường
- Dự báo xu hướng
- Hỗ trợ ra quyết định
- Sinh KPI thông minh
- Đề xuất ưu tiên xử lý

AI KHÔNG tự động ra quyết định hành chính.

---

# 20.2 Business Context

Nguồn dữ liệu AI

DiaBan
      ↓
HoGiaDinh
      ↓
ThanhVien
      ↓
DoiTuongAnSinh
      ↓
ChinhSachTroCap
      ↓
ChiTraTroCap
      ↓
Dashboard
      ↓
AI Analytics

---

# 20.3 Business Objectives

Hoàn thành

- AI Dashboard
- Trend Analysis
- Prediction Engine
- Risk Detection
- Smart Recommendation
- AI KPI

---

# 20.4 Scope

Bao gồm

- Phân tích dữ liệu
- Dự báo xu hướng
- Phát hiện bất thường
- Đề xuất ưu tiên
- Phân tích địa bàn
- Phân tích chi trả
- AI Summary

---

# 20.5 Out Of Scope

Không triển khai

- Chatbot
- Zalo OA
- OCR
- Voice AI
- Image Recognition

---

# 20.6 Stakeholders

- Chủ tịch UBND xã
- Lãnh đạo
- Cán bộ Lao động - TB&XH
- Quản trị hệ thống

---

# 20.7 Use Cases

UC01 Xem phân tích AI.

UC02 Xem dự báo.

UC03 Xem cảnh báo.

UC04 Xem đề xuất.

UC05 So sánh xu hướng.

UC06 Phân tích theo địa bàn.

UC07 Sinh báo cáo AI.

---

# 20.8 User Stories

As Commune Leader

I want AI to identify welfare trends

So that I can allocate resources proactively.

---

As Welfare Officer

I want AI to detect unusual records

So that data quality is improved.

---

# 20.9 Functional Requirements

FR-001 Trend Analysis

FR-002 Prediction

FR-003 Risk Detection

FR-004 Duplicate Detection

FR-005 Geographic Analysis

FR-006 AI Summary

FR-007 Recommendation Engine

FR-008 AI KPI

---

# 20.10 Non-functional Requirements

- AI Response <10 giây
- Kết quả có thể giải thích
- Có khả năng mở rộng mô hình AI
- Lưu lịch sử phân tích

---

# 20.11 Business Rules

BR-AI-001

AI chỉ đóng vai trò hỗ trợ.

---

BR-AI-002

Mọi khuyến nghị đều phải ghi rõ cơ sở dữ liệu sử dụng.

---

BR-AI-003

Không tự động thay đổi dữ liệu nghiệp vụ.

---

BR-AI-004

Mọi lần chạy AI phải được ghi nhật ký.

---

# 20.12 AI Analysis Modules

- Trend Analysis
- Population Analysis
- Welfare Analysis
- Budget Analysis
- Geographic Analysis
- Payment Analysis
- Risk Detection
- Recommendation Engine

---

# 20.13 AI Services

IAIAnalyticsService

IPredictionService

IRecommendationService

IRiskDetectionService

IKPIService

---

# 20.14 API Contract

GET /api/ai/dashboard

GET /api/ai/trends

GET /api/ai/predictions

GET /api/ai/recommendations

GET /api/ai/risk-analysis

GET /api/ai/kpis

POST /api/ai/run-analysis

---

# 20.15 Security Requirements

Roles

- Admin
- LanhDao

Permissions

- AI.Read
- AI.Run
- AI.Export

JWT Required

Audit Required

---

# 20.16 Performance Requirements

- Phân tích 100.000 bản ghi <60 giây
- Sinh Dashboard AI <10 giây
- API <3 giây (khi đọc kết quả đã tính toán)

---

# 20.17 Coding Tasks

- AI Service Layer
- AI Scheduler
- AI REST API
- Recommendation Engine
- Risk Engine
- Dashboard Integration
- Swagger
- Logging

---

# 20.18 Testing Tasks

Unit Test

- Recommendation
- Prediction
- Risk Detection

Integration Test

- AI API
- Dashboard

Performance Test

- Large Dataset Analysis
- Concurrent Requests

---

# 20.19 Documentation Tasks

Cập nhật:

- AI_DEVELOPMENT_GUIDE.md
- API_SPEC.md
- BUSINESS_RULES.md
- PROJECT_PROGRESS.md
- AI_HANDOVER.md

---

# 20.20 AI Execution Package

## AI Required Input

- Bootstrap Documents
- Standards
- Sprint 01 → Sprint 11

## AI Expected Output

□ AI Service Layer

□ Analytics API

□ Recommendation Engine

□ Risk Detection

□ Dashboard Integration

□ Swagger

□ Unit Tests

---

## Scope Boundary

Không triển khai:

- Chatbot
- Zalo OA
- OCR
- Voice AI

---

# 20.21 Review Checklist

□ AI phân tích đúng

□ Khuyến nghị hợp lý

□ Không ghi sai dữ liệu

□ API hoạt động

□ Dashboard hiển thị

□ Documentation cập nhật

---

# 20.22 Definition of Done

□ AI Analytics hoàn chỉnh

□ Prediction hoạt động

□ Recommendation hoạt động

□ Dashboard AI hoàn chỉnh

□ Unit Test Pass

□ Documentation hoàn chỉnh

---

# 20.23 Sprint Deliverables

- AI Analytics Module
- Prediction Engine
- Recommendation Engine
- AI Dashboard
- REST API
- Swagger
- Test Report

---

# 20.24 Git Commit Convention

Branch

feature/sprint-11-ai-analytics

Commit

feat(ai): implement analytics and prediction module

---

# 20.25 Sprint Close Checklist

□ AI Review

□ Security Review

□ Performance Review

□ Product Owner Approval

---

# Sprint Output

Sau Sprint 11, hệ thống có khả năng:

- Phân tích dữ liệu an sinh bằng AI
- Dự báo xu hướng
- Phát hiện bất thường
- Đưa ra khuyến nghị có giải thích
- Hỗ trợ lãnh đạo ra quyết định

Sprint tiếp theo:

Sprint 12 – AI Assistant
# ============================================================================
# END OF SPRINT 11
# ============================================================================
```
```md id="rag4n2"
# ============================================================================
# 21. SPRINT 12 – AI ASSISTANT (ENTERPRISE RAG)
# ============================================================================
#
# Module            : AI Assistant
# Sprint            : Sprint 12
# Priority          : Very High
# Duration          : 3 Weeks
# Depends On        : Sprint 11
# Architecture      : Clean Architecture + RAG
# Framework         : ASP.NET Core 8
# Database          : SQL Server 2022
#
# ============================================================================

# 21.1 Sprint Overview

## Sprint Goal

Triển khai Trợ lý AI của AnSinhSo theo kiến trúc Enterprise RAG.

AI Assistant hỗ trợ:

- Cán bộ xã
- Lãnh đạo
- Người dân (theo phạm vi phân quyền)

AI chỉ trả lời dựa trên dữ liệu đã được phép truy cập và cơ sở tri thức của hệ thống.

---

# 21.2 Business Context

Nguồn tri thức

BUSINESS_RULES
        ↓
DATABASE_DESIGN
        ↓
API_SPEC
        ↓
USER_GUIDE
        ↓
FAQ
        ↓
Policies
        ↓
Knowledge Base
        ↓
Retriever
        ↓
LLM
        ↓
AI Assistant

---

# 21.3 Business Objectives

Hoàn thành:

- AI Chat
- Knowledge Search
- Context Retrieval
- Answer Generation
- Citation
- Conversation History
- Suggested Questions

---

# 21.4 Scope

Bao gồm

- Chat AI
- RAG
- Semantic Search
- Conversation History
- Suggested Prompt
- AI Citation
- AI Feedback

---

# 21.5 Out Of Scope

Không triển khai

- Voice AI
- OCR
- Video AI
- Image Recognition
- AI Agent Workflow

---

# 21.6 Stakeholders

- Admin
- Lãnh đạo
- Cán bộ xã
- Người dân

---

# 21.7 Use Cases

UC01 Hỏi chính sách.

UC02 Tra cứu quy trình.

UC03 Tra cứu hồ sơ (theo quyền).

UC04 Hỏi hướng dẫn sử dụng.

UC05 Hỏi số liệu tổng hợp được phép truy cập.

UC06 Gửi phản hồi về câu trả lời.

---

# 21.8 User Stories

As Welfare Officer

I want AI to answer procedural questions

So that my daily work becomes faster.

---

As Citizen

I want AI to explain welfare policies

So that I understand my rights and obligations.

---

# 21.9 Functional Requirements

FR-001 AI Chat

FR-002 Semantic Search

FR-003 Citation

FR-004 Conversation History

FR-005 Suggested Questions

FR-006 User Feedback

FR-007 Session Context

FR-008 Audit Log

---

# 21.10 Non-functional Requirements

- AI phản hồi <10 giây
- Hỗ trợ nhiều phiên đồng thời
- Câu trả lời có nguồn tham chiếu
- Không truy xuất dữ liệu vượt quyền

---

# 21.11 Business Rules

BR-AST-001

AI chỉ trả lời dựa trên Knowledge Base và dữ liệu được phép truy cập.

---

BR-AST-002

Mọi câu trả lời phải kèm nguồn tham chiếu nếu sử dụng tài liệu nội bộ.

---

BR-AST-003

Không tiết lộ dữ liệu của người dùng khác.

---

BR-AST-004

Lưu nhật ký hội thoại để phục vụ kiểm tra và cải tiến.

---

# 21.12 RAG Architecture

User Question
      ↓
Authentication
      ↓
Authorization
      ↓
Retriever
      ↓
Knowledge Base
      ↓
Relevant Context
      ↓
LLM
      ↓
Response Generator
      ↓
Citation Builder
      ↓
Audit Log

---

# 21.13 Knowledge Sources

- BUSINESS_RULES.md
- DATABASE_DESIGN.md
- API_SPEC.md
- USER_GUIDE.md
- FAQ.md
- Chính sách trợ cấp
- Văn bản hướng dẫn

---

# 21.14 AI Services

IAssistantService

IRetrieverService

IKnowledgeBaseService

IConversationService

ICitationService

---

# 21.15 API Contract

POST /api/ai/chat

POST /api/ai/search

GET /api/ai/history

GET /api/ai/suggestions

POST /api/ai/feedback

DELETE /api/ai/history/{id}

---

# 21.16 Security Requirements

Roles

- Admin
- LanhDao
- CanBoXa
- NguoiDan

Permissions

- AI.Chat
- AI.Search
- AI.History

JWT Required

Audit Required

Conversation Logging Required

---

# 21.17 Performance Requirements

- Trả lời <10 giây
- Truy xuất tri thức <2 giây
- Hỗ trợ 500 phiên đồng thời
- Lưu lịch sử hội thoại an toàn

---

# 21.18 Coding Tasks

- RAG Pipeline
- Retriever
- Knowledge Index
- Conversation Module
- Citation Engine
- AI API
- Swagger
- Logging

---

# 21.19 Testing Tasks

Unit Test

- Retriever
- Citation
- Session Context

Integration Test

- AI API
- Authorization
- Knowledge Retrieval

Performance Test

- Concurrent Chat
- Retrieval Speed

---

# 21.20 Documentation Tasks

Cập nhật:

- AI_DEVELOPMENT_GUIDE.md
- API_SPEC.md
- PROJECT_PROGRESS.md
- AI_HANDOVER.md
- USER_GUIDE.md

---

# 21.21 AI Execution Package

## AI Required Input

- Bootstrap Documents
- Standards
- Sprint 01 → Sprint 12

## AI Expected Output

□ AI Assistant Module

□ RAG Pipeline

□ Knowledge Retriever

□ Citation Engine

□ Conversation History

□ REST API

□ Swagger

□ Unit Tests

---

## Scope Boundary

Không triển khai:

- Zalo OA
- Voice AI
- OCR
- AI Workflow Automation

---

# 21.22 Review Checklist

□ AI trả lời đúng ngữ cảnh

□ Có nguồn tham chiếu

□ Không vượt quyền truy cập

□ API hoạt động

□ Lưu lịch sử hội thoại

□ Documentation cập nhật

---

# 21.23 Definition of Done

□ AI Assistant hoạt động

□ RAG hoàn chỉnh

□ Conversation History hoạt động

□ Citation Engine hoạt động

□ API kiểm thử thành công

□ Sprint được nghiệm thu

---

# 21.24 Sprint Deliverables

- AI Assistant Module
- RAG Engine
- Knowledge Base Integration
- Citation Engine
- Conversation History
- REST API
- Swagger
- Test Report

---

# 21.25 Git Commit Convention

Branch

feature/sprint-12-ai-assistant

Commit

feat(ai): implement enterprise RAG assistant

---

# 21.26 Sprint Close Checklist

□ AI Review

□ Security Review

□ Knowledge Review

□ Performance Review

□ Product Owner Approval

---

# Sprint Output

Sau Sprint 12, hệ thống có:

- Trợ lý AI theo kiến trúc Enterprise RAG
- Truy xuất tri thức có kiểm soát
- Trả lời có nguồn tham chiếu
- Lưu lịch sử hội thoại
- Sẵn sàng tích hợp Zalo OA ở Sprint 13

# ============================================================================
# END OF SPRINT 12
# ============================================================================
```
