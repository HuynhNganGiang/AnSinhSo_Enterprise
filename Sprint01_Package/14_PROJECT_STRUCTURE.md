````md
# ============================================================================
# 14_PROJECT_STRUCTURE.md
# ============================================================================
#
# Project         : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
# Document Type   : Enterprise Project Structure Guide
# Version         : 1.0.0
# Status          : Approved
#
# Architecture    : Enterprise Clean Architecture
# Framework       : ASP.NET Core 8
# Database        : SQL Server 2022
# Frontend        : HTML / CSS / JavaScript
#
# Last Updated    : 2026-07-12
#
# ============================================================================

# 1. PURPOSE

Tài liệu này quy định cấu trúc chuẩn của toàn bộ dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa Repository.
- Chuẩn hóa Solution.
- Chuẩn hóa Project.
- Chuẩn hóa Folder.
- Chuẩn hóa Module.
- Chuẩn hóa Documentation.
- Chuẩn hóa Asset.
- Chuẩn hóa Deployment.

Mọi AI và Developer phải tuân thủ tài liệu này.

---

# 2. SCOPE

Áp dụng cho toàn bộ dự án:

- Repository
- Backend
- Frontend
- Database
- Documentation
- Deployment
- DevOps
- AI
- Test
- Scripts

---

# 3. DESIGN PRINCIPLES

Project Structure phải đảm bảo:

- Clean
- Scalable
- Maintainable
- Modular
- Enterprise Ready
- Cloud Ready
- Docker Ready

---

# 4. ENTERPRISE PROJECT ARCHITECTURE

```text
Enterprise Repository

↓

Solution

↓

Projects

↓

Layers

↓

Modules

↓

Components

↓

Files
```

---

# 5. REPOSITORY STRUCTURE

Repository chỉ có một Root duy nhất.

Ví dụ:

```text
AnSinhSo/

│

├── src/

├── database/

├── docs/

├── deployment/

├── scripts/

├── tests/

├── tools/

├── assets/

├── backups/

└── README.md
```

Không tạo nhiều thư mục Root trùng chức năng.

---

# 6. SOLUTION STRUCTURE

Solution chính:

```text
AnSinhSo.sln
```

Bao gồm:

- API
- Application
- Domain
- Infrastructure
- Shared

Mỗi Project có trách nhiệm riêng.

---

# 7. PROJECT ORGANIZATION

Các Project không được phụ thuộc vòng (Circular Dependency).

Dependency chỉ theo chiều:

```text
Presentation

↓

Application

↓

Domain

↓

Infrastructure
```

---

# 8. LAYER ORGANIZATION

Các Layer:

- Presentation Layer
- Application Layer
- Domain Layer
- Infrastructure Layer
- Shared Layer

Mỗi Layer chỉ thực hiện đúng vai trò của mình.

---

# 9. MODULE ORGANIZATION

Ví dụ:

```text
Authentication

Citizen

Household

SocialSecurity

Payment

Dashboard

Map

AI

Notification

Administration
```

Mỗi Module hoạt động độc lập.

---

# 10. NAMING CONVENTION

Tên:

Project

Folder

Namespace

Class

Method

Property

phải tuân thủ:

04_CODING_STANDARDS.md

Không tạo Naming Convention mới.

---

# 11. PHASE 1 CHECKLIST

Trước khi chuyển sang Phase 2A cần xác nhận:

- Repository Structure được xác định.
- Solution Structure hoàn chỉnh.
- Layer Structure hoàn chỉnh.
- Module Structure được xác định.
- Naming Convention được thống nhất.
- Không có Circular Dependency.
- Enterprise Architecture được giữ nguyên.

---

# End of Phase 1

Phase tiếp theo:

Backend Project Structure
````
````md
# ============================================================================
# 12. BACKEND PROJECT STRUCTURE
# ============================================================================

## 12.1 Purpose

Phần này quy định cấu trúc Backend chuẩn của dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa Solution.
- Chuẩn hóa Project.
- Chuẩn hóa Layer.
- Chuẩn hóa Dependency.
- Chuẩn hóa Module.
- Chuẩn hóa Namespace.
- Chuẩn hóa Folder.

Toàn bộ Backend phải tuân thủ kiến trúc Clean Architecture.

---

## 12.2 Enterprise Backend Architecture

Backend sử dụng kiến trúc:

```text
Presentation Layer

↓

Application Layer

↓

Domain Layer

↓

Infrastructure Layer

↓

Database
```

Mọi Dependency chỉ được phép đi theo chiều từ trên xuống dưới.

Không được phép phụ thuộc ngược.

---

## 12.3 Backend Solution Structure

```text
src/

│

├── AnSinhSo.API/

├── AnSinhSo.Application/

├── AnSinhSo.Domain/

├── AnSinhSo.Infrastructure/

├── AnSinhSo.Shared/

├── AnSinhSo.Contracts/

└── AnSinhSo.Worker/
```

Ý nghĩa:

| Project | Vai trò |
|----------|----------|
| API | REST API |
| Application | Business Use Cases |
| Domain | Entity và Business Rules |
| Infrastructure | Database, External Services |
| Shared | Shared Components |
| Contracts | DTO, Request, Response |
| Worker | Background Jobs |

---

# ============================================================================
# 13. PRESENTATION LAYER
# ============================================================================

## 13.1 API Project

```text
AnSinhSo.API

│

├── Controllers/

├── Middlewares/

├── Filters/

├── Extensions/

├── Configuration/

├── Endpoints/

├── Swagger/

├── wwwroot/

├── appsettings.json

└── Program.cs
```

---

## 13.2 Controllers

Controllers chỉ có nhiệm vụ:

- Nhận Request
- Validate Model
- Gọi Application Layer
- Trả Response

Không chứa Business Logic.

---

## 13.3 Middleware

Bao gồm:

- Exception Middleware
- Authentication Middleware
- Authorization Middleware
- Logging Middleware
- Request Middleware
- Response Middleware

---

## 13.4 Configuration

Lưu:

- JWT
- CORS
- Swagger
- Database
- Logging
- Dependency Injection

---

# ============================================================================
# 14. APPLICATION LAYER
# ============================================================================

## 14.1 Folder Structure

```text
Application/

│

├── Features/

├── Interfaces/

├── Services/

├── Commands/

├── Queries/

├── Validators/

├── Behaviors/

├── DTOs/

├── Mapping/

└── Common/
```

---

## 14.2 Feature Structure

Ví dụ:

```text
Citizen/

Household/

SocialSecurity/

Dashboard/

Authentication/

Notification/

AI/

GIS/
```

Mỗi Feature hoạt động độc lập.

---

## 14.3 CQRS Structure

Mỗi Feature:

```text
Feature

│

├── Commands/

├── Queries/

├── Handlers/

├── Validators/

├── DTO/

└── Mapping/
```

Nếu không sử dụng CQRS thì vẫn giữ cấu trúc Feature để dễ mở rộng.

---

# ============================================================================
# 15. DOMAIN LAYER
# ============================================================================

## 15.1 Domain Structure

```text
Domain/

│

├── Entities/

├── Enums/

├── ValueObjects/

├── Events/

├── Exceptions/

├── Specifications/

├── Interfaces/

└── Constants/
```

---

## 15.2 Domain Rules

Domain chỉ chứa:

- Business Rules
- Entity
- Value Object
- Domain Service
- Domain Event

Không được chứa:

- SQL
- HTTP
- API
- UI
- Infrastructure

---

# ============================================================================
# 16. INFRASTRUCTURE LAYER
# ============================================================================

## 16.1 Folder Structure

```text
Infrastructure/

│

├── Persistence/

├── Repositories/

├── Identity/

├── Authentication/

├── Authorization/

├── Logging/

├── Caching/

├── Storage/

├── Email/

├── Sms/

├── Zalo/

├── AI/

├── GIS/

└── BackgroundJobs/
```

---

## 16.2 Persistence

Bao gồm:

```text
Persistence/

│

├── DbContext/

├── Configurations/

├── Migrations/

├── Seed/

└── UnitOfWork/
```

---

## 16.3 External Services

Tách riêng:

- Zalo OA
- OpenAI
- Gemini
- Maps
- Email
- SMS

Không được viết trực tiếp trong Business Logic.

---

# ============================================================================
# 17. SHARED PROJECT
# ============================================================================

## 17.1 Shared Components

```text
Shared/

│

├── Constants/

├── Helpers/

├── Extensions/

├── Utilities/

├── Exceptions/

├── Models/

├── Responses/

└── Localization/
```

---

## 17.2 Shared Rules

Shared chỉ chứa:

- Thành phần dùng chung.
- Không chứa Business Logic.
- Không phụ thuộc Module.

---

# ============================================================================
# 18. DEPENDENCY RULES
# ============================================================================

## 18.1 Allowed Dependency

```text
API

↓

Application

↓

Domain

↓

Infrastructure
```

---

## 18.2 Forbidden Dependency

Không được:

```text
Domain

↓

API
```

Không được:

```text
Infrastructure

↓

API
```

Không được:

```text
Application

↓

Presentation
```

---

## 18.3 Circular Dependency

Tuyệt đối cấm.

Không Project nào được tham chiếu vòng.

---

# ============================================================================
# 19. NAMESPACE STANDARDS
# ============================================================================

Ví dụ:

```text
AnSinhSo.API.Controllers

AnSinhSo.Application.Features.Citizen

AnSinhSo.Domain.Entities

AnSinhSo.Infrastructure.Persistence

AnSinhSo.Shared.Helpers
```

Namespace phải phản ánh đúng cấu trúc thư mục.

---

# ============================================================================
# 20. BACKEND DEVELOPMENT CHECKLIST
# ============================================================================

Trước khi tạo Project Backend cần xác nhận:

□ Đúng Solution Structure.

□ Đúng Layer Structure.

□ Đúng Folder Structure.

□ Đúng Dependency Rules.

□ Không có Circular Dependency.

□ Namespace đúng chuẩn.

□ Business Logic nằm đúng Layer.

□ Infrastructure được tách riêng.

□ Shared Project không chứa Business Logic.

□ Tuân thủ Clean Architecture.

---

# ============================================================================
# 21. PHASE 2A CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2B cần xác nhận:

- Backend Solution Structure hoàn chỉnh.
- API Project được chuẩn hóa.
- Application Layer được chuẩn hóa.
- Domain Layer được chuẩn hóa.
- Infrastructure Layer được chuẩn hóa.
- Shared Project được chuẩn hóa.
- Dependency Rules được xác định.
- Namespace Standards được thống nhất.
- Backend Checklist hoàn thành.

---

# End of Phase 2A

Phase tiếp theo:

- Frontend Project Structure
- Frontend Folder Standards
- UI Module Structure
- Asset Organization
- JavaScript Module Standards
- CSS Architecture
- Responsive Layout Structure
- Frontend Build Structure
- Static Resource Management
- Frontend Checklist
````
````md
# ============================================================================
# 22. FRONTEND PROJECT STRUCTURE
# ============================================================================

## 22.1 Purpose

Phần này quy định cấu trúc chuẩn của Frontend trong dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa cấu trúc thư mục.
- Chuẩn hóa Component.
- Chuẩn hóa Asset.
- Chuẩn hóa JavaScript.
- Chuẩn hóa CSS.
- Chuẩn hóa Layout.
- Chuẩn hóa Responsive Design.
- Chuẩn hóa khả năng mở rộng.

Frontend phải dễ bảo trì, dễ mở rộng và độc lập với Backend.

---

## 22.2 Frontend Architecture

Frontend được tổ chức theo kiến trúc Module-Based.

```text
Browser

↓

Pages

↓

Layouts

↓

Components

↓

Services

↓

API

↓

Backend
```

Mỗi tầng chỉ thực hiện đúng trách nhiệm của mình.

---

## 22.3 Root Folder Structure

```text
frontend/

│

├── public/

├── src/

├── assets/

├── docs/

├── package.json

├── README.md

└── .env
```

Nếu sử dụng Frontend thuần (HTML/CSS/JavaScript) thì vẫn áp dụng nguyên tắc phân tách module tương tự.

---

# ============================================================================
# 23. SRC STRUCTURE
# ============================================================================

## 23.1 Source Folder

```text
src/

│

├── pages/

├── layouts/

├── components/

├── services/

├── modules/

├── hooks/

├── utils/

├── constants/

├── configs/

├── routes/

├── styles/

├── assets/

└── index.js
```

---

## 23.2 Folder Responsibilities

| Folder | Chức năng |
|----------|-----------|
| pages | Màn hình chính |
| layouts | Khung giao diện |
| components | Thành phần dùng chung |
| services | Giao tiếp API |
| modules | Chức năng nghiệp vụ |
| hooks | Logic tái sử dụng (nếu dùng framework) |
| utils | Hàm tiện ích |
| constants | Hằng số |
| configs | Cấu hình |
| routes | Điều hướng |
| styles | CSS toàn cục |

---

# ============================================================================
# 24. PAGE STRUCTURE
# ============================================================================

## 24.1 Pages

Ví dụ:

```text
pages/

│

├── Login/

├── Dashboard/

├── Household/

├── Citizen/

├── SocialSecurity/

├── Payment/

├── GIS/

├── AI/

├── Reports/

└── Administration/
```

---

## 24.2 Page Rules

Mỗi Page chỉ:

- Hiển thị dữ liệu.
- Điều hướng.
- Gọi Component.

Không chứa Business Logic.

---

# ============================================================================
# 25. COMPONENT STRUCTURE
# ============================================================================

## 25.1 Components

```text
components/

│

├── Common/

├── Forms/

├── Tables/

├── Cards/

├── Charts/

├── Dialogs/

├── Navigation/

├── Sidebar/

├── Header/

├── Footer/

└── Loading/
```

---

## 25.2 Component Rules

Component phải:

- Có khả năng tái sử dụng.
- Độc lập.
- Không phụ thuộc Business Logic.
- Không gọi Database.

---

## 25.3 Naming

Ví dụ:

```text
CitizenCard

HouseholdTable

DashboardChart

SearchBox

NotificationPanel
```

Không dùng tên chung chung như:

```text
Component1

MyTable

Demo
```

---

# ============================================================================
# 26. MODULE STRUCTURE
# ============================================================================

## 26.1 Business Modules

```text
modules/

│

├── Authentication/

├── Citizen/

├── Household/

├── SocialSecurity/

├── Dashboard/

├── GIS/

├── AI/

├── Notification/

└── Reports/
```

---

## 26.2 Module Rules

Mỗi Module:

- Có Service riêng.
- Có Component riêng.
- Có Assets riêng (nếu cần).
- Không phụ thuộc trực tiếp Module khác.

---

# ============================================================================
# 27. SERVICE STRUCTURE
# ============================================================================

## 27.1 API Services

```text
services/

│

├── auth.service.js

├── citizen.service.js

├── household.service.js

├── dashboard.service.js

├── ai.service.js

├── zalo.service.js

└── api.service.js
```

---

## 27.2 Service Rules

Service chỉ:

- Gọi API.
- Xử lý Request.
- Xử lý Response.

Không chứa:

- UI.
- HTML.
- CSS.

---

# ============================================================================
# 28. CONFIGURATION STRUCTURE
# ============================================================================

## 28.1 Config Folder

```text
configs/

│

├── api.config.js

├── auth.config.js

├── map.config.js

├── ai.config.js

└── app.config.js
```

---

## 28.2 Configuration Rules

Không được Hard-code:

- API URL
- Token
- Secret
- Environment

---

# ============================================================================
# 29. ROUTING STRUCTURE
# ============================================================================

## 29.1 Routes

```text
routes/

│

├── public.routes.js

├── private.routes.js

└── index.js
```

---

## 29.2 Route Rules

Phân tách:

- Public Routes
- Protected Routes
- Admin Routes

Kiểm tra quyền truy cập trước khi điều hướng.

---

# ============================================================================
# 30. FRONTEND DEPENDENCY RULES
# ============================================================================

## 30.1 Dependency

```text
Pages

↓

Components

↓

Services

↓

API
```

---

## 30.2 Forbidden

Không được:

```text
Component

↓

Database
```

Không được:

```text
Page

↓

SQL
```

Không được:

```text
UI

↓

Business Logic
```

---

# ============================================================================
# 31. PHASE 2B CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2C cần xác nhận:

- Frontend Structure hoàn chỉnh.
- Pages được chuẩn hóa.
- Components được chuẩn hóa.
- Modules được chuẩn hóa.
- Services được chuẩn hóa.
- Configurations được chuẩn hóa.
- Routing được chuẩn hóa.
- Dependency Rules được xác định.
- Không có Business Logic trong UI.
- Frontend có khả năng mở rộng.

---

# End of Phase 2B

Phase tiếp theo:

- Database Project Structure
- SQL Folder Standards
- Migration Structure
- Seed Data Organization
- Stored Procedure Organization
- View & Function Structure
- Backup Strategy
- Database Versioning
- Database Checklist
````
````md
# ============================================================================
# 32. DATABASE PROJECT STRUCTURE
# ============================================================================

## 32.1 Purpose

Phần này quy định cấu trúc chuẩn cho toàn bộ Database của dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa SQL Scripts.
- Chuẩn hóa Migration.
- Chuẩn hóa Seed Data.
- Chuẩn hóa Stored Procedure.
- Chuẩn hóa Views.
- Chuẩn hóa Functions.
- Chuẩn hóa Backup.
- Chuẩn hóa Version Control.

Database phải đảm bảo:

- Dễ quản lý.
- Dễ mở rộng.
- Dễ triển khai.
- Dễ phục hồi.
- Dễ kiểm thử.

---

## 32.2 Database Architecture

```text
Business Rules

↓

Database Design

↓

Tables

↓

Constraints

↓

Indexes

↓

Views

↓

Stored Procedures

↓

Functions

↓

Seed Data

↓

Backup
```

Database luôn được phát triển dựa trên BUSINESS_RULES.md và DATABASE_DESIGN.md.

---

## 32.3 Database Root Structure

```text
database/

│

├── schema/

├── migrations/

├── seed/

├── procedures/

├── functions/

├── views/

├── triggers/

├── indexes/

├── scripts/

├── backups/

├── releases/

└── README.md
```

---

# ============================================================================
# 33. SCHEMA STRUCTURE
# ============================================================================

## 33.1 Schema Folder

```text
schema/

│

├── 01_CreateDatabase.sql

├── 02_CreateTables.sql

├── 03_CreateConstraints.sql

├── 04_CreateIndexes.sql

├── 05_CreateViews.sql

├── 06_CreateFunctions.sql

├── 07_CreateProcedures.sql

├── 08_CreateTriggers.sql

└── 09_CreatePermissions.sql
```

---

## 33.2 Rules

Schema chỉ chứa:

- Database Definition.
- Table Definition.
- Constraint Definition.

Không chứa:

- Demo Data.
- Test Data.

---

# ============================================================================
# 34. MIGRATION STRUCTURE
# ============================================================================

## 34.1 Migration Folder

```text
migrations/

│

├── 20260712_Initial.sql

├── 20260720_AddCitizenTable.sql

├── 20260725_UpdateDashboard.sql

└── ...
```

---

## 34.2 Migration Rules

Mỗi Migration:

- Chỉ thực hiện một mục tiêu.
- Có Version.
- Có Rollback (nếu cần).
- Không sửa Migration cũ đã phát hành.

---

## 34.3 Naming Convention

```text
YYYYMMDD_Description.sql
```

Ví dụ:

```text
20260712_InitialDatabase.sql

20260715_AddAIConfiguration.sql

20260720_UpdateCitizenTable.sql
```

---

# ============================================================================
# 35. SEED DATA STRUCTURE
# ============================================================================

## 35.1 Seed Folder

```text
seed/

│

├── Roles.sql

├── Users.sql

├── Provinces.sql

├── Districts.sql

├── Communes.sql

├── Household.sql

├── Citizen.sql

└── SocialSecurity.sql
```

---

## 35.2 Seed Rules

Seed Data phải:

- Có thể chạy nhiều lần.
- Không tạo dữ liệu trùng.
- Có khả năng Reset.

---

## 35.3 Environment

Phân tách:

```text
Development

Testing

Production
```

Không dùng dữ liệu Development cho Production.

---

# ============================================================================
# 36. STORED PROCEDURE STRUCTURE
# ============================================================================

## 36.1 Folder Structure

```text
procedures/

│

├── Citizen/

├── Household/

├── Dashboard/

├── AI/

├── GIS/

├── Reports/

└── Payment/
```

---

## 36.2 Procedure Naming

Ví dụ:

```text
sp_GetCitizen

sp_InsertCitizen

sp_UpdateCitizen

sp_DeleteCitizen
```

---

## 36.3 Procedure Rules

Procedure phải:

- Có TRY...CATCH.
- Có Transaction khi cần.
- Có Comment.
- Có Error Handling.

---

# ============================================================================
# 37. VIEW & FUNCTION STRUCTURE
# ============================================================================

## 37.1 Views

```text
views/

│

├── vwCitizenSummary.sql

├── vwDashboardStatistics.sql

├── vwHouseholdReport.sql

└── ...
```

---

## 37.2 Functions

```text
functions/

│

├── fn_GetAge.sql

├── fn_CalculateAllowance.sql

├── fn_FormatAddress.sql

└── ...
```

---

## 37.3 Rules

View:

- Chỉ phục vụ đọc dữ liệu.
- Không thay thế Business Logic.

Function:

- Có thể tái sử dụng.
- Không chứa xử lý phức tạp.

---

# ============================================================================
# 38. BACKUP STRUCTURE
# ============================================================================

## 38.1 Backup Folder

```text
backups/

│

├── Full/

├── Differential/

├── TransactionLog/

└── Archive/
```

---

## 38.2 Backup Policy

Thực hiện:

- Full Backup.
- Differential Backup.
- Transaction Log Backup.

Theo lịch của môi trường Production.

---

## 38.3 Restore Testing

Định kỳ kiểm tra:

- Restore thành công.
- Backup không lỗi.
- Đúng phiên bản.

---

# ============================================================================
# 39. DATABASE VERSIONING
# ============================================================================

## 39.1 Version Rules

Mỗi lần thay đổi Database:

- Tăng Version.
- Ghi Migration.
- Cập nhật Documentation.
- Cập nhật Change Log.

---

## 39.2 Version Format

Ví dụ:

```text
v1.0.0

v1.1.0

v1.2.0

v2.0.0
```

---

## 39.3 Release Rules

Không triển khai Production nếu:

- Thiếu Migration.
- Thiếu Backup.
- Thiếu Rollback Plan.

---

# ============================================================================
# 40. DATABASE PROJECT CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2D cần xác nhận:

□ Database Structure hoàn chỉnh.

□ Schema được chuẩn hóa.

□ Migration được chuẩn hóa.

□ Seed Data được chuẩn hóa.

□ Stored Procedures được chuẩn hóa.

□ Views và Functions được chuẩn hóa.

□ Backup Strategy được xác định.

□ Database Versioning được áp dụng.

□ Không vi phạm DATABASE_RULES.md.

□ Documentation đã được cập nhật.

---

# ============================================================================
# 41. PHASE 2C CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2D cần xác nhận:

- Database Project Structure hoàn chỉnh.
- SQL Folder Standards được chuẩn hóa.
- Migration Structure hoàn chỉnh.
- Seed Data Organization hoàn chỉnh.
- Stored Procedure Organization hoàn chỉnh.
- View & Function Structure hoàn chỉnh.
- Backup Strategy được xác định.
- Database Versioning được chuẩn hóa.
- Database Checklist hoàn thành.

---

# End of Phase 2C

Phase tiếp theo:

- Documentation Project Structure
- Standards Folder Structure
- Architecture Documents
- API Documentation Structure
- AI Documentation Structure
- Version Control for Documents
- Documentation Lifecycle
- Documentation Governance
- Documentation Checklist
````
````md
# ============================================================================
# 42. DOCUMENTATION PROJECT STRUCTURE
# ============================================================================

## 42.1 Purpose

Phần này quy định cấu trúc chuẩn của toàn bộ tài liệu trong dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa Documentation.
- Chuẩn hóa Standards.
- Chuẩn hóa Architecture Documents.
- Chuẩn hóa Technical Documents.
- Chuẩn hóa User Documents.
- Chuẩn hóa AI Documents.
- Chuẩn hóa Version Control.

Tất cả tài liệu phải:

- Dễ tìm kiếm.
- Dễ cập nhật.
- Dễ bảo trì.
- Có khả năng mở rộng.
- Đồng bộ với mã nguồn.

---

## 42.2 Documentation Architecture

```text
Project

↓

Architecture Documents

↓

Technical Documents

↓

Standards

↓

Deployment

↓

Operations

↓

AI Guides

↓

User Guides
```

---

## 42.3 Documentation Root Structure

```text
docs/

│

├── architecture/

├── standards/

├── database/

├── api/

├── frontend/

├── backend/

├── deployment/

├── devops/

├── testing/

├── ai/

├── operations/

├── user-guides/

├── releases/

├── templates/

└── README.md
```

---

# ============================================================================
# 43. ARCHITECTURE DOCUMENTS
# ============================================================================

## 43.1 Folder Structure

```text
architecture/

│

├── PROJECT_OVERVIEW.md

├── SYSTEM_ARCHITECTURE.md

├── CLEAN_ARCHITECTURE.md

├── TECHNOLOGY_STACK.md

├── MODULE_ARCHITECTURE.md

├── DATA_FLOW.md

├── SEQUENCE_DIAGRAM.md

├── DEPLOYMENT_ARCHITECTURE.md

└── DECISION_RECORDS/
```

---

## 43.2 Rules

Architecture Documents chỉ mô tả:

- Kiến trúc.
- Thành phần hệ thống.
- Quyết định thiết kế.
- Luồng dữ liệu.

Không chứa hướng dẫn triển khai chi tiết.

---

# ============================================================================
# 44. STANDARDS STRUCTURE
# ============================================================================

## 44.1 Standards Folder

```text
standards/

│

├── CODING/

├── DATABASE/

├── API/

├── FRONTEND/

├── BACKEND/

├── SECURITY/

├── DEPLOYMENT/

├── TESTING/

├── DEVOPS/

└── AI/
```

---

## 44.2 Rules

Mỗi Standard phải có:

- Purpose.
- Scope.
- Rules.
- Checklist.
- Related Documents.
- Change Log.

---

# ============================================================================
# 45. API DOCUMENTATION STRUCTURE
# ============================================================================

## 45.1 Folder Structure

```text
api/

│

├── API_SPEC.md

├── AUTHENTICATION.md

├── ERROR_CODES.md

├── REQUEST_EXAMPLES/

├── RESPONSE_EXAMPLES/

├── POSTMAN/

└── SWAGGER/
```

---

## 45.2 API Rules

API Documentation phải đồng bộ với:

- Swagger.
- Source Code.
- DTO.
- Response Model.

Không để tài liệu khác với API thực tế.

---

# ============================================================================
# 46. DATABASE DOCUMENTATION STRUCTURE
# ============================================================================

## 46.1 Folder Structure

```text
database/

│

├── DATABASE_DESIGN.md

├── ERD.md

├── TABLES.md

├── INDEXES.md

├── MIGRATIONS.md

├── STORED_PROCEDURES.md

├── FUNCTIONS.md

└── BACKUP_POLICY.md
```

---

## 46.2 Rules

Database Documentation cần mô tả:

- Schema.
- Quan hệ.
- Chuẩn dữ liệu.
- Quy tắc nghiệp vụ.

---

# ============================================================================
# 47. AI DOCUMENTATION STRUCTURE
# ============================================================================

## 47.1 Folder Structure

```text
ai/

│

├── AI_DEVELOPMENT_GUIDE.md

├── AI_PROMPTS.md

├── AI_WORKFLOW.md

├── AI_HANDOVER.md

├── AI_BEST_PRACTICES.md

├── AI_CHANGELOG.md

└── AI_FAQ.md
```

---

## 47.2 AI Rules

Tài liệu AI cần đồng bộ với:

- Bootstrap.
- Project Progress.
- Standards.
- Coding Workflow.

---

# ============================================================================
# 48. VERSION CONTROL FOR DOCUMENTS
# ============================================================================

## 48.1 Version Format

Mỗi tài liệu phải có:

```text
Version

Status

Owner

Created Date

Updated Date

Change Log
```

---

## 48.2 Status

Các trạng thái:

```text
Draft

↓

Review

↓

Approved

↓

Released

↓

Archived
```

---

## 48.3 File Naming

Định dạng:

```text
UPPER_CASE_WITH_UNDERSCORE.md
```

Ví dụ:

```text
API_SPEC.md

DATABASE_DESIGN.md

PROJECT_PROGRESS.md

AI_DEVELOPMENT_GUIDE.md
```

---

# ============================================================================
# 49. DOCUMENTATION LIFECYCLE
# ============================================================================

## 49.1 Lifecycle

```text
Create

↓

Review

↓

Approve

↓

Release

↓

Maintain

↓

Archive
```

---

## 49.2 Maintenance

Sau mỗi Sprint:

- Kiểm tra tài liệu.
- Đồng bộ với mã nguồn.
- Cập nhật Version.
- Cập nhật Change Log.

---

## 49.3 Archive

Không xóa tài liệu cũ.

Chỉ chuyển sang:

```text
archive/
```

để phục vụ truy vết.

---

# ============================================================================
# 50. DOCUMENTATION GOVERNANCE
# ============================================================================

## 50.1 Governance Principles

Documentation phải:

- Chính xác.
- Nhất quán.
- Đồng bộ.
- Có khả năng kiểm toán.
- Có khả năng bảo trì.

---

## 50.2 AI Responsibilities

AI phải:

- Cập nhật Documentation khi thay đổi Code.
- Không tạo tài liệu trùng lặp.
- Không đổi cấu trúc tài liệu đã Approved.
- Không bỏ qua Change Log.

---

## 50.3 Human Responsibilities

Developer hoặc Project Owner chịu trách nhiệm:

- Review.
- Approve.
- Release.

---

# ============================================================================
# 51. DOCUMENTATION CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2E cần xác nhận:

□ Documentation Structure hoàn chỉnh.

□ Standards Structure hoàn chỉnh.

□ Architecture Documents được chuẩn hóa.

□ API Documentation được chuẩn hóa.

□ Database Documentation được chuẩn hóa.

□ AI Documentation được chuẩn hóa.

□ Version Control được áp dụng.

□ Documentation Lifecycle được xác định.

□ Documentation Governance hoàn chỉnh.

□ Documentation đồng bộ với Project.

---

# ============================================================================
# 52. PHASE 2D CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2E cần xác nhận:

- Documentation Project Structure hoàn chỉnh.
- Standards Folder Structure được chuẩn hóa.
- Architecture Documents được chuẩn hóa.
- API Documentation Structure hoàn chỉnh.
- Database Documentation Structure hoàn chỉnh.
- AI Documentation Structure hoàn chỉnh.
- Version Control được áp dụng.
- Documentation Lifecycle hoàn chỉnh.
- Documentation Governance được xác định.
- Documentation Checklist hoàn thành.

---

# End of Phase 2D

Phase tiếp theo:

- Enterprise Repository Governance
- Git Repository Standards
- Branch Strategy
- Repository Security
- Repository Lifecycle
- Folder Governance
- Repository Versioning
- Repository Audit
- Enterprise Checklist
- Change Log
- Related Documents
- End of Document
````
````md
# ============================================================================
# 53. ENTERPRISE REPOSITORY GOVERNANCE
# ============================================================================

## 53.1 Purpose

Phần này quy định các tiêu chuẩn quản trị Repository của dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa Git Repository.
- Chuẩn hóa Branch.
- Chuẩn hóa Version.
- Chuẩn hóa Release.
- Chuẩn hóa Repository Security.
- Chuẩn hóa Repository Lifecycle.

Repository là nguồn dữ liệu chính thức (Single Source of Truth) của toàn bộ dự án.

---

## 53.2 Repository Principles

Repository phải đảm bảo:

- One Repository.
- One Architecture.
- One Standards.
- One Documentation.
- One Version History.

Toàn bộ thành viên và AI đều làm việc trên cùng một tiêu chuẩn.

---

## 53.3 Repository Structure

Repository chỉ chứa:

```text
src/

database/

docs/

deployment/

scripts/

tests/

tools/

assets/

backups/
```

Không tạo thư mục ngoài tiêu chuẩn nếu chưa được phê duyệt.

---

# ============================================================================
# 54. GIT REPOSITORY STANDARDS
# ============================================================================

## 54.1 Git Principles

Git là hệ thống quản lý phiên bản chính thức.

Mọi thay đổi đều phải:

- Có Commit.
- Có lịch sử.
- Có khả năng truy vết.
- Có thể phục hồi.

---

## 54.2 Commit Standards

Commit Message nên theo chuẩn:

```text
<type>: <description>
```

Ví dụ:

```text
feat: add citizen management module

fix: resolve login validation issue

refactor: optimize dashboard service

docs: update API specification

test: add unit tests for payment service

chore: update dependencies
```

---

## 54.3 Commit Types

| Type | Ý nghĩa |
|--------|----------------------------|
| feat | Thêm chức năng |
| fix | Sửa lỗi |
| docs | Cập nhật tài liệu |
| refactor | Tái cấu trúc mã |
| test | Thêm hoặc sửa kiểm thử |
| style | Định dạng mã nguồn |
| perf | Tối ưu hiệu năng |
| build | Build / Dependency |
| ci | CI/CD |
| chore | Công việc bảo trì |

---

# ============================================================================
# 55. BRANCH STRATEGY
# ============================================================================

## 55.1 Branch Structure

```text
main

↓

develop

↓

feature/*

↓

release/*

↓

hotfix/*
```

---

## 55.2 Main Branch

Branch:

```text
main
```

Chỉ chứa:

- Production Code.
- Stable Release.

Không phát triển trực tiếp trên main.

---

## 55.3 Develop Branch

Branch:

```text
develop
```

Là nơi tích hợp toàn bộ chức năng trước khi phát hành.

---

## 55.4 Feature Branch

Ví dụ:

```text
feature/authentication

feature/dashboard

feature/gis

feature/ai

feature/zalo

feature/report
```

Mỗi chức năng một Branch.

---

## 55.5 Release Branch

Ví dụ:

```text
release/v1.0.0

release/v1.1.0
```

Dùng chuẩn bị phát hành.

---

## 55.6 Hotfix Branch

Ví dụ:

```text
hotfix/login-error

hotfix/security-fix
```

Chỉ dùng cho Production.

---

# ============================================================================
# 56. REPOSITORY SECURITY
# ============================================================================

## 56.1 Security Rules

Repository không được chứa:

- Password.
- API Key.
- Secret.
- Token.
- Private Certificate.
- Connection String Production.

---

## 56.2 Sensitive Files

Đưa vào:

```text
.gitignore
```

Ví dụ:

```text
.env

appsettings.Production.json

*.pfx

*.bak

*.mdf

*.ldf

node_modules/

bin/

obj/
```

---

## 56.3 Secret Management

Sử dụng:

- Environment Variables.
- Secret Manager.
- Azure Key Vault (nếu triển khai Azure).
- Docker Secrets (nếu dùng Docker).

Không lưu Secret trong Git.

---

# ============================================================================
# 57. REPOSITORY VERSIONING
# ============================================================================

## 57.1 Semantic Versioning

Áp dụng:

```text
MAJOR.MINOR.PATCH
```

Ví dụ:

```text
1.0.0

1.1.0

1.2.3

2.0.0
```

---

## 57.2 Version Rules

| Thay đổi | Version |
|----------|----------|
| Bug Fix | PATCH |
| New Feature | MINOR |
| Breaking Change | MAJOR |

---

## 57.3 Release Tags

Ví dụ:

```text
v1.0.0

v1.1.0

v2.0.0
```

---

# ============================================================================
# 58. REPOSITORY LIFECYCLE
# ============================================================================

## 58.1 Development Lifecycle

```text
Planning

↓

Development

↓

Testing

↓

Review

↓

Release

↓

Deployment

↓

Maintenance
```

---

## 58.2 Repository Maintenance

Định kỳ:

- Xóa Branch đã Merge.
- Kiểm tra Dependency.
- Kiểm tra Security.
- Đồng bộ Documentation.
- Kiểm tra Build.

---

# ============================================================================
# 59. REPOSITORY AUDIT
# ============================================================================

## 59.1 Audit Scope

Định kỳ kiểm tra:

- Branch.
- Commit.
- Standards.
- Documentation.
- Security.
- Dependency.
- License.
- Backup.

---

## 59.2 Audit Checklist

Kiểm tra:

- Đúng Branch Strategy.
- Không có Secret.
- Không có Binary không cần thiết.
- Không có File tạm.
- Documentation đầy đủ.

---

# ============================================================================
# 60. ENTERPRISE PROJECT CHECKLIST
# ============================================================================

Trước khi kết thúc tài liệu cần xác nhận:

□ Repository Structure chuẩn hóa.

□ Backend Structure chuẩn hóa.

□ Frontend Structure chuẩn hóa.

□ Database Structure chuẩn hóa.

□ Documentation Structure chuẩn hóa.

□ Git Strategy được xác định.

□ Branch Strategy được xác định.

□ Repository Security được áp dụng.

□ Versioning được chuẩn hóa.

□ Repository Lifecycle hoàn chỉnh.

□ Repository Audit được chuẩn hóa.

---

# ============================================================================
# 61. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------------|-------------------------------------------|
| 1.0.0 | 2026-07-12 | Initial Enterprise Project Structure Guide |

---

# ============================================================================
# 62. RELATED DOCUMENTS
# ============================================================================

## Bootstrap

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_PROJECT_INDEX.md
- 00_AI_HANDOVER.md

---

## Standards

- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 07_FRONTEND_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md
- 10_DEPLOYMENT_STANDARDS.md
- 11_TESTING_STANDARDS.md
- 12_DEVOPS_STANDARDS.md
- 13_AI_DEVELOPMENT_GUIDE.md

---

## Core Documents

- BUSINESS_RULES.md
- DATABASE_DESIGN.md
- API_SPEC.md
- DEPLOYMENT_GUIDE.md

---

# ============================================================================
# 63. END OF DOCUMENT
# ============================================================================

Tài liệu **14_PROJECT_STRUCTURE.md** là tiêu chuẩn chính thức quy định cấu trúc Repository, Solution, Project, Folder và Documentation của dự án AnSinhSo.

Tất cả Developer và AI phải:

- Tuân thủ cấu trúc Repository đã quy định.
- Không tự ý thay đổi Clean Architecture.
- Không tạo Project ngoài chuẩn.
- Không tạo Dependency vòng (Circular Dependency).
- Đồng bộ Documentation khi thay đổi cấu trúc.
- Tuân thủ Branch Strategy và Versioning.

Mọi thay đổi đối với tài liệu này phải:

- Được đánh giá tác động.
- Được cập nhật Change Log.
- Được đồng bộ với Bootstrap.
- Được Project Owner hoặc Technical Lead phê duyệt.

---

# END OF FILE
````
