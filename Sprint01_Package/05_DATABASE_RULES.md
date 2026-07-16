---
Project: AnSinhSo Enterprise

Document ID: DOC-DB-001
Document Name: Database Rules
File Name: 05_DATABASE_RULES.md

Version: 1.0
Status: FROZEN
Baseline: Sprint 00

Owner: Solution Architecture Team

Parent Document:
00_PROJECT_BOOTSTRAP.md

Related Documents:
04_CODING_STANDARDS.md
06_API_STANDARDS.md
08_BACKEND_STANDARDS.md
20_SECURITY_ARCHITECTURE.md

Classification: Internal
---

# =============================================================================
# DATABASE RULES
# Enterprise Documentation Suite
# =============================================================================

# Purpose

Tài liệu này định nghĩa toàn bộ tiêu chuẩn thiết kế, xây dựng, quản trị và vận hành cơ sở dữ liệu của dự án **AnSinhSo Enterprise**.

Đây là tài liệu chuẩn để tất cả Developer, Database Engineer, AI Coding Agent và Technical Reviewer sử dụng khi thiết kế hoặc thay đổi cơ sở dữ liệu.

Mọi thiết kế Database phải tuân thủ các quy định trong tài liệu này.

---

# Scope

Tài liệu áp dụng cho toàn bộ Database Layer của hệ thống, bao gồm:

- SQL Server Database
- Entity Framework Core
- Database Schema
- Tables
- Views
- Stored Procedures
- Functions
- Triggers
- Indexes
- Constraints
- Migrations
- Seed Data
- GIS Data
- AI Data
- Zalo OA Data

---

# Audience

Tài liệu dành cho:

- Solution Architect
- Backend Developer
- Database Engineer
- AI Coding Agent
- Technical Reviewer
- DevOps Engineer

---

# Reading Guide

Để hiểu đầy đủ tài liệu này, nên đọc theo thứ tự:

1. 00_PROJECT_BOOTSTRAP.md
2. 01_PROJECT_CONTEXT.md
3. 03_AI_SYSTEM_RULES.md
4. 04_CODING_STANDARDS.md
5. 05_DATABASE_RULES.md (tài liệu này)
6. 06_API_STANDARDS.md
7. 08_BACKEND_STANDARDS.md

---

# =============================================================================
# PHASE 1 – DATABASE PHILOSOPHY
# =============================================================================

# 1.1 Purpose

Phase này định nghĩa triết lý thiết kế cơ sở dữ liệu cho dự án AnSinhSo Enterprise.

Thay vì chỉ lưu trữ dữ liệu, cơ sở dữ liệu của hệ thống được xem là nền tảng trung tâm (Core Business Asset), phục vụ:

- Quản lý dữ liệu an sinh xã hội.
- Hỗ trợ ra quyết định.
- Phân tích dữ liệu.
- Tích hợp AI.
- Tích hợp GIS.
- Tích hợp Zalo Official Account.
- Báo cáo và thống kê.
- Mở rộng hệ thống trong tương lai.

Database phải được thiết kế với mục tiêu sử dụng lâu dài, ổn định và có khả năng mở rộng.

---

# 1.2 Core Principles

Toàn bộ Database phải tuân thủ các nguyên tắc sau.

## Principle 1 – Business First

Database phản ánh đúng nghiệp vụ.

Không thiết kế Database chỉ để thuận tiện cho lập trình.

---

## Principle 2 – Single Source of Truth

Mỗi dữ liệu chỉ có một nơi lưu trữ chính thức.

Không lưu trùng dữ liệu nếu không có lý do rõ ràng.

---

## Principle 3 – Normalization First

Ưu tiên chuẩn hóa dữ liệu.

Chỉ Denormalize khi có phân tích hiệu năng và được phê duyệt.

---

## Principle 4 – Integrity First

Mọi dữ liệu phải đảm bảo:

- Chính xác
- Nhất quán
- Có khả năng kiểm chứng
- Có thể truy vết

---

## Principle 5 – Security by Design

Bảo mật được tích hợp ngay từ giai đoạn thiết kế Database.

Không xem Security là bước bổ sung sau cùng.

---

## Principle 6 – Scalability

Database phải hỗ trợ mở rộng:

- nhiều xã
- nhiều huyện
- nhiều tỉnh
- nhiều chương trình an sinh
- nhiều loại trợ cấp

không cần thay đổi kiến trúc.

---

## Principle 7 – AI Ready

Database phải đủ cấu trúc để:

- Machine Learning
- AI Prediction
- Recommendation
- Analytics
- Data Warehouse

không cần thiết kế lại.

---

## Principle 8 – GIS Ready

Database phải hỗ trợ:

- Latitude
- Longitude
- Geometry
- Spatial Query
- Spatial Index

để tích hợp bản đồ số.

---

## Principle 9 – Auditability

Mọi thay đổi dữ liệu quan trọng phải:

- lưu lịch sử
- ghi nhận người thực hiện
- ghi nhận thời gian
- có khả năng phục hồi

---

## Principle 10 – Enterprise Quality

Database phải đáp ứng các tiêu chí:

- dễ bảo trì
- dễ mở rộng
- dễ kiểm thử
- dễ sao lưu
- dễ phục hồi
- hiệu năng cao

---

# 1.3 Database Objectives

Hệ thống Database hướng tới các mục tiêu:

✓ Đảm bảo tính toàn vẹn dữ liệu.

✓ Đảm bảo hiệu năng truy vấn.

✓ Hỗ trợ Dashboard thời gian thực.

✓ Hỗ trợ AI.

✓ Hỗ trợ GIS.

✓ Hỗ trợ Zalo OA.

✓ Hỗ trợ tích hợp API.

✓ Hỗ trợ báo cáo thống kê.

✓ Hỗ trợ mở rộng trong tương lai.

---

# 1.4 Database Quality Attributes

Database phải đáp ứng các thuộc tính chất lượng sau:

| Attribute | Description |
|-----------|-------------|
| Availability | Hoạt động ổn định |
| Reliability | Dữ liệu đáng tin cậy |
| Consistency | Nhất quán |
| Integrity | Toàn vẹn |
| Scalability | Dễ mở rộng |
| Security | Bảo mật |
| Maintainability | Dễ bảo trì |
| Performance | Hiệu năng cao |
| Auditability | Có thể truy vết |
| Recoverability | Có thể phục hồi |

---

# 1.5 Enterprise Database Goals

Cơ sở dữ liệu của AnSinhSo Enterprise không chỉ phục vụ vận hành hiện tại mà còn là nền tảng dữ liệu cho các giai đoạn tiếp theo:

- Digital Government
- Smart Commune
- Smart District
- Smart Province
- AI Analytics
- GIS Analytics
- Decision Support System
- Open Data Integration

---

# 1.6 Design Philosophy

Khi thiết kế Database, ưu tiên theo thứ tự:

1. Đúng nghiệp vụ.
2. Đúng kiến trúc.
3. Đúng chuẩn hóa dữ liệu.
4. Đảm bảo toàn vẹn dữ liệu.
5. Đảm bảo bảo mật.
6. Tối ưu hiệu năng.
7. Hỗ trợ mở rộng.

Không đánh đổi tính đúng đắn của dữ liệu để lấy sự đơn giản trong lập trình.

---

# 1.7 AI Database Design Principles

AI Coding Agent khi sinh Database phải:

- Tuân thủ tài liệu này.
- Không tự ý thêm bảng ngoài thiết kế.
- Không tự ý đổi kiểu dữ liệu.
- Không tự ý bỏ khóa ngoại.
- Không tự ý tạo dữ liệu mẫu không đúng nghiệp vụ.
- Báo cáo khi phát hiện mâu thuẫn trong Documentation Suite.

---

# 1.8 Governance Rules

Mọi thay đổi Database phải:

- Được phân tích tác động.
- Được cập nhật Documentation.
- Được tạo Migration.
- Được Review.
- Được Baseline.

Không sửa trực tiếp Database Production.

---

# 1.9 Success Criteria

Phase 1 được xem là hoàn thành khi:

- Triết lý thiết kế Database được xác lập.
- Các nguyên tắc cốt lõi được thống nhất.
- AI và Developer hiểu vai trò của Database trong kiến trúc hệ thống.
- Database được định hướng theo chuẩn Enterprise.

---

# 1.10 Phase Summary

Phase 1 xác lập nền tảng tư duy cho toàn bộ Database của dự án AnSinhSo Enterprise.

Từ Phase 2 trở đi, mọi quy tắc về kiến trúc, schema, bảng dữ liệu, khóa chính, khóa ngoại, chỉ mục, migration và quản trị cơ sở dữ liệu đều phải tuân thủ các nguyên tắc đã được xác lập trong Phase này.
# =============================================================================
# PHASE 2 – ENTERPRISE DATABASE ARCHITECTURE
# =============================================================================

# 2.1 Purpose

Phase này định nghĩa kiến trúc tổng thể (Enterprise Database Architecture) của hệ thống AnSinhSo Enterprise.

Mục tiêu là thiết lập một kiến trúc cơ sở dữ liệu:

- Chuẩn hóa.
- Dễ mở rộng.
- Dễ bảo trì.
- Hỗ trợ hiệu năng cao.
- Hỗ trợ AI.
- Hỗ trợ GIS.
- Hỗ trợ Zalo Official Account.
- Phù hợp với Clean Architecture.

Kiến trúc này là nền tảng cho mọi Database Schema, Entity Framework Core Model, Migration và SQL Script của dự án.

---

# 2.2 Database Architecture Principles

Kiến trúc cơ sở dữ liệu phải tuân thủ các nguyên tắc sau.

## Principle 1 – Domain Driven

Database được tổ chức theo Domain nghiệp vụ.

Ví dụ:

- Identity
- Household
- Social Welfare
- Payment
- GIS
- Notification
- AI
- Audit

Không tổ chức theo màn hình giao diện.

---

## Principle 2 – Modular

Mỗi Domain là một Module độc lập.

Ví dụ:

Identity Module

↓

Users

Roles

Permissions

UserTokens

AuditLog

Household Module

↓

Households

HouseholdMembers

Addresses

Citizen Module

↓

Citizens

CitizenDocuments

CitizenPhotos

---

## Principle 3 – Separation of Concerns

Không trộn dữ liệu giữa các Domain.

Ví dụ:

Thông tin đăng nhập

không lưu

trong bảng HoGiaDinh.

---

## Principle 4 – Shared Reference Data

Các bảng dùng chung phải tách riêng.

Ví dụ

Roles

AdministrativeUnits

EthnicGroups

PolicyCategories

BenefitTypes

StatusCodes

Reference Data không được nhân bản.

---

# 2.3 Enterprise Database Layers

Database được chia thành các lớp logic sau.

## Layer 1

Master Data

Ví dụ

Roles

Users

AdministrativeUnits

Villages

PolicyTypes

BenefitTypes

Reference Tables

---

## Layer 2

Business Data

Ví dụ

Households

Citizens

SocialBeneficiaries

BenefitPayments

SupportPrograms

Applications

---

## Layer 3

Integration Data

Ví dụ

ZaloOA

WebhookLogs

NotificationQueue

SMSQueue

APIIntegration

GISImport

ExternalSync

---

## Layer 4

Analytics Data

Ví dụ

DashboardCache

AIRecommendation

PredictionResults

Statistics

AggregatedData

ReportSnapshots

---

## Layer 5

System Data

Ví dụ

AuditLogs

SystemLogs

UserSessions

RefreshTokens

MigrationHistory

Configurations

---

# 2.4 Domain Boundaries

Toàn bộ Database được chia theo Domain.

| Domain | Responsibility |
|---------|----------------|
| Identity | Xác thực và phân quyền |
| Household | Quản lý hộ gia đình |
| Citizen | Quản lý công dân |
| Social Welfare | Quản lý đối tượng an sinh |
| Payment | Quản lý chi trả |
| GIS | Dữ liệu bản đồ |
| AI | Dữ liệu AI và dự đoán |
| Notification | Zalo OA, Email, SMS |
| Audit | Nhật ký hệ thống |
| Administration | Danh mục và cấu hình |

Không được để một bảng thuộc nhiều Domain cùng lúc.

---

# 2.5 Logical Architecture

```text
Application Layer
        │
        ▼
Entity Framework Core
        │
        ▼
Database Layer
        │
        ├───────────────┐
        │               │
        ▼               ▼
Business Data     Reference Data
        │
        ▼
Integration Data
        │
        ▼
Analytics Data
        │
        ▼
Audit Data
```

Database phải độc lập với Presentation Layer.

---

# 2.6 Database Schema Strategy

SQL Server sử dụng nhiều Schema nhằm tách biệt trách nhiệm.

| Schema | Purpose |
|---------|----------|
| dbo | Core Business |
| auth | Authentication & Authorization |
| ref | Reference Data |
| gis | GIS Data |
| ai | AI Data |
| zalo | Zalo Official Account |
| audit | Audit & Logging |
| report | Reporting |
| config | Configuration |

Không lưu toàn bộ bảng trong schema `dbo` nếu đã có schema chuyên biệt.

---

# 2.7 Module Dependency Rules

Các Module chỉ được phụ thuộc theo chiều sau:

```text
Reference Data
        │
        ▼
Identity
        │
        ▼
Household
        │
        ▼
Citizen
        │
        ▼
Social Welfare
        │
        ▼
Payment
        │
        ▼
Notification
        │
        ▼
Analytics
```

Module dưới không được tham chiếu ngược lên Module trên nếu không thực sự cần thiết.

---

# 2.8 Database Scalability Strategy

Kiến trúc phải hỗ trợ mở rộng:

- Nhiều xã.
- Nhiều huyện.
- Nhiều tỉnh.
- Nhiều chương trình an sinh.
- Nhiều năm ngân sách.
- Nhiều loại trợ cấp.
- Nhiều nguồn dữ liệu tích hợp.

Không được thiết kế chỉ phục vụ dữ liệu của một địa phương.

---

# 2.9 AI & GIS Architecture Integration

Database phải được chuẩn bị sẵn cho các thành phần mở rộng.

## AI

- Prediction Models
- Recommendation Results
- Prompt Logs
- AI Feedback
- AI Metrics

## GIS

- Coordinates
- Spatial Objects
- Administrative Boundaries
- Spatial Index
- GeoJSON Import/Export

## Zalo OA

- OA Users
- Templates
- Notification Queue
- Delivery Logs
- Webhook Events

Các thành phần này phải có Domain riêng và không làm ảnh hưởng đến dữ liệu nghiệp vụ cốt lõi.

---

# 2.10 Phase Summary

Phase 2 xác lập kiến trúc tổng thể của Database AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Database được tổ chức theo Domain nghiệp vụ.
- Các Layer dữ liệu được phân tách rõ ràng.
- Schema Strategy được xác định.
- AI, GIS và Zalo OA có không gian mở rộng riêng.
- Kiến trúc sẵn sàng cho các Phase tiếp theo về Naming Convention, Schema Design, Relationship Rules và Entity Framework Core.

Từ Phase 3 trở đi, mọi bảng, cột, khóa chính, khóa ngoại và đối tượng cơ sở dữ liệu phải tuân thủ kiến trúc được xác lập trong Phase này.
# =============================================================================
# PHASE 3 – NAMING CONVENTION STANDARDS
# =============================================================================

# 3.1 Purpose

Phase này định nghĩa tiêu chuẩn đặt tên (Naming Convention) cho toàn bộ đối tượng trong cơ sở dữ liệu của dự án AnSinhSo Enterprise.

Mục tiêu:

- Đảm bảo tính nhất quán.
- Dễ đọc.
- Dễ bảo trì.
- Dễ mở rộng.
- Hỗ trợ Entity Framework Core.
- Hỗ trợ AI Coding Agent sinh mã chính xác.

Mọi đối tượng Database phải tuân thủ quy tắc trong Phase này.

---

# 3.2 General Naming Principles

Toàn bộ Database phải tuân thủ các nguyên tắc sau.

## Principle 1 – English Only

Tên Database Object sử dụng tiếng Anh.

✔ Users

✔ Households

✔ BenefitPayments

✘ NguoiDung

✘ HoGiaDinh

---

## Principle 2 – PascalCase

Tên Table, View và Entity sử dụng PascalCase.

Ví dụ:

Users

Households

CitizenDocuments

BenefitPayments

---

## Principle 3 – Meaningful Names

Tên phải phản ánh đúng nghiệp vụ.

✔ SocialBeneficiaries

✔ HouseholdMembers

✔ BenefitApplications

✘ Table1

✘ DataTemp

✘ Info

---

## Principle 4 – No Abbreviations

Không viết tắt nếu không phải chuẩn chung.

✔ AdministrativeUnit

✔ NotificationTemplate

✘ AdminUnit

✘ NotiTemp

Ngoại lệ:

API

GIS

AI

JWT

OA

URL

ID

OTP

---

## Principle 5 – Singular vs Plural

Tên bảng sử dụng **danh từ số nhiều (Plural)**.

Ví dụ:

Users

Roles

Permissions

Households

Citizens

BenefitPayments

BenefitPrograms

---

# 3.3 Database Naming Standards

| Object | Convention | Example |
|----------|------------|----------|
| Database | PascalCase | AnSinhSoDB |
| Schema | lowercase | auth |
| Table | PascalCase (Plural) | Households |
| View | vw_PascalCase | vw_HouseholdSummary |
| Stored Procedure | usp_PascalCase | usp_CreatePayment |
| Function | fn_PascalCase | fn_CalculateBenefit |
| Trigger | trg_PascalCase | trg_AuditHousehold |
| Sequence | seq_PascalCase | seq_PaymentNumber |

---

# 3.4 Table Naming Rules

Tên bảng phải:

- là danh từ
- số nhiều
- phản ánh nghiệp vụ

Ví dụ:

Users

Roles

Permissions

Households

HouseholdMembers

Citizens

BenefitPrograms

BenefitPayments

AuditLogs

NotificationQueues

PredictionResults

---

Không sử dụng:

tblUsers

tblCitizen

tbPayment

Data1

MasterData

---

# 3.5 Column Naming Rules

Tên cột sử dụng PascalCase.

Ví dụ:

FirstName

LastName

DateOfBirth

PhoneNumber

IdentityNumber

BenefitAmount

CreatedDate

UpdatedDate

---

Không sử dụng:

first_name

firstname

FIRST_NAME

phone_no

dob

---

# 3.6 Primary Key Naming

Khóa chính luôn có tên:

Id

Ví dụ:

Users

Id

Households

Id

BenefitPayments

Id

Không sử dụng:

UserId

HouseholdId

PaymentId

làm Primary Key.

Các tên này chỉ dùng làm Foreign Key.

---

# 3.7 Foreign Key Naming

Tên Foreign Key theo mẫu:

<TableName>Id

Ví dụ:

UserId

RoleId

HouseholdId

CitizenId

VillageId

ProgramId

PaymentId

---

Ví dụ

HouseholdMembers

Id

HouseholdId

CitizenId

RelationshipTypeId

---

# 3.8 Audit Column Naming

Mọi bảng nghiệp vụ đều phải có các cột chuẩn.

CreatedDate

CreatedBy

UpdatedDate

UpdatedBy

DeletedDate

DeletedBy

IsDeleted

RowVersion

Không đổi tên các cột này.

---

# 3.9 Boolean Naming

Boolean luôn bắt đầu bằng:

Is

Has

Can

Requires

Ví dụ

IsActive

IsDeleted

HasChildren

CanReceiveBenefit

RequiresVerification

Không dùng:

Active

Deleted

StatusFlag

---

# 3.10 DateTime Naming

Tên DateTime phải kết thúc bằng:

Date

Time

At

Ví dụ

BirthDate

CreatedDate

UpdatedDate

ApprovedDate

SubmittedAt

ProcessedAt

Không dùng:

NgayTao

Date1

Time2

---

# 3.11 Enum Naming

Tên bảng danh mục sử dụng:

<Type>

Ví dụ

BenefitType

CitizenStatus

Gender

EducationLevel

RelationshipType

---

Tên cột FK:

BenefitTypeId

GenderId

RelationshipTypeId

---

# 3.12 Index Naming

Tên Index theo mẫu:

IX_Table_Column

Ví dụ

IX_Users_UserName

IX_Citizens_IdentityNumber

IX_Households_VillageId

IX_BenefitPayments_PaymentDate

Composite Index

IX_BenefitPayments_ProgramId_PaymentDate

---

# 3.13 Constraint Naming

Primary Key

PK_Table

Ví dụ

PK_Users

PK_Households

---

Foreign Key

FK_Table_ReferencedTable

Ví dụ

FK_HouseholdMembers_Households

FK_HouseholdMembers_Citizens

---

Unique

UQ_Table_Column

Ví dụ

UQ_Users_UserName

UQ_Citizens_IdentityNumber

---

Check Constraint

CK_Table_Rule

Ví dụ

CK_Citizens_Gender

CK_Payments_Amount

---

Default Constraint

DF_Table_Column

Ví dụ

DF_Users_IsActive

DF_Households_CreatedDate

---

# 3.14 Stored Procedure Naming

Mẫu:

usp_ActionEntity

Ví dụ

usp_CreateHousehold

usp_UpdateCitizen

usp_DeleteBenefit

usp_GeneratePayment

usp_GetDashboard

Không dùng:

spTest

spData

Procedure1

---

# 3.15 Function Naming

Tên Function:

fn_Action

Ví dụ

fn_CalculateAge

fn_GetBenefitLevel

fn_FormatAddress

fn_GetVillageName

---

# 3.16 View Naming

Tên View:

vw_Entity

Ví dụ

vw_HouseholdSummary

vw_CitizenProfile

vw_PaymentStatistics

vw_DashboardOverview

---

# 3.17 AI & GIS Naming

AI

AIModels

AIPredictions

AIFeedbacks

AIPromptLogs

GIS

MapLayers

AdministrativeBoundaries

SpatialObjects

GeoLocations

SpatialIndexes

Zalo OA

OAUsers

OAMessages

NotificationTemplates

WebhookEvents

MessageQueues

---

# 3.18 Reserved Words

Không sử dụng các từ khóa SQL làm tên bảng hoặc cột.

Ví dụ:

User

Order

Group

Key

Value

Table

Index

Transaction

Nếu cần, đổi thành:

Users

BenefitOrders

PermissionGroups

ConfigKeys

SystemValues

---

# 3.19 AI Naming Compliance

AI Coding Agent phải:

- Không tự ý đổi tên bảng.
- Không tự ý viết tắt.
- Không tạo tên không đúng chuẩn.
- Tuân thủ tuyệt đối Naming Convention.
- Báo lỗi nếu phát hiện Naming không thống nhất.

---

# 3.20 Phase Summary

Phase 3 thiết lập bộ tiêu chuẩn đặt tên thống nhất cho toàn bộ Database của AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Tất cả Database Objects đều có quy tắc đặt tên rõ ràng.
- Entity Framework Core có thể ánh xạ (mapping) nhất quán.
- AI Coding Agent có cơ sở để sinh mã chính xác.
- Hệ thống dễ bảo trì, mở rộng và kiểm tra trong suốt vòng đời dự án.

Từ Phase 4 trở đi, mọi Schema, Table, Column, Constraint, Index và Database Object phải tuân thủ Naming Convention được định nghĩa trong Phase này.
# =============================================================================
# PHASE 4 – SCHEMA DESIGN STANDARDS
# =============================================================================

# 4.1 Purpose

Phase này định nghĩa tiêu chuẩn thiết kế Schema cho cơ sở dữ liệu AnSinhSo Enterprise.

Schema được sử dụng để:

- Phân tách Domain nghiệp vụ.
- Tăng khả năng bảo mật.
- Dễ quản lý quyền truy cập.
- Dễ mở rộng hệ thống.
- Giảm xung đột tên đối tượng.
- Hỗ trợ triển khai Enterprise Database.

Schema không chỉ là công cụ phân loại bảng mà còn là một phần của kiến trúc hệ thống.

---

# 4.2 Design Principles

Việc thiết kế Schema phải tuân thủ các nguyên tắc sau.

## Principle 1 – Domain Separation

Mỗi Schema chỉ đại diện cho một Domain nghiệp vụ.

Ví dụ:

auth

↓

Authentication

gis

↓

Geographic Information

audit

↓

Audit Logging

ai

↓

Artificial Intelligence

Không được trộn nhiều Domain vào cùng một Schema.

---

## Principle 2 – Business First

Schema phải phản ánh nghiệp vụ.

Không tạo Schema theo Developer.

✔ auth

✔ audit

✔ report

✘ giang

✘ backend

✘ module1

---

## Principle 3 – Scalability

Schema phải hỗ trợ mở rộng.

Ví dụ

Hiện tại

ai

Sau này

ai_training

ai_prediction

ai_models

Không ảnh hưởng Business Schema.

---

## Principle 4 – Security Isolation

Các Schema nhạy cảm phải tách riêng.

Ví dụ

auth

audit

config

Việc cấp quyền có thể thực hiện theo Schema.

---

# 4.3 Standard Schemas

AnSinhSo Enterprise sử dụng các Schema chuẩn sau.

| Schema | Responsibility |
|---------|----------------|
| dbo | Core Business Data |
| auth | Authentication & Authorization |
| ref | Reference Data |
| audit | Audit Logs |
| gis | GIS & Spatial Data |
| ai | AI & Analytics |
| zalo | Zalo Official Account |
| report | Reporting |
| config | System Configuration |

Không tạo Schema mới nếu chưa được Architecture Review.

---

# 4.4 dbo Schema

dbo chỉ chứa dữ liệu nghiệp vụ cốt lõi.

Ví dụ

Households

HouseholdMembers

Citizens

SocialBeneficiaries

BenefitPrograms

BenefitPayments

Applications

Policies

Không lưu dữ liệu hệ thống trong dbo.

---

# 4.5 auth Schema

Schema auth quản lý xác thực và phân quyền.

Ví dụ

Users

Roles

Permissions

UserRoles

RolePermissions

RefreshTokens

UserSessions

OTPRequests

Không lưu thông tin nghiệp vụ trong auth.

---

# 4.6 ref Schema

Schema ref chứa dữ liệu dùng chung.

Ví dụ

AdministrativeUnits

Villages

EthnicGroups

Religions

Occupations

EducationLevels

BenefitTypes

RelationshipTypes

Reference Data không được lưu trùng trong các bảng nghiệp vụ.

---

# 4.7 audit Schema

Schema audit lưu lịch sử hệ thống.

Ví dụ

AuditLogs

EntityChanges

LoginHistory

APIAccessLogs

SecurityEvents

DataChangeHistory

Không sử dụng audit để lưu dữ liệu nghiệp vụ.

---

# 4.8 gis Schema

Schema gis phục vụ dữ liệu không gian.

Ví dụ

AdministrativeBoundaries

MapLayers

GeoLocations

SpatialObjects

Coordinates

SpatialIndexes

SpatialQueries

Các bảng trong Schema này ưu tiên sử dụng kiểu dữ liệu:

geometry

geography

---

# 4.9 ai Schema

Schema ai lưu dữ liệu phục vụ AI.

Ví dụ

PredictionResults

RecommendationResults

PromptLogs

EmbeddingVectors

TrainingDatasets

InferenceHistory

AIFeedbacks

Schema này phải độc lập với Business Data.

---

# 4.10 zalo Schema

Schema zalo phục vụ tích hợp Zalo Official Account.

Ví dụ

OAUsers

NotificationTemplates

MessageQueues

WebhookEvents

DeliveryLogs

BroadcastHistory

Không lưu Notification trực tiếp trong Business Tables.

---

# 4.11 report Schema

Schema report phục vụ Dashboard và Báo cáo.

Ví dụ

DashboardStatistics

MonthlyReports

AnnualReports

PaymentSummaries

GISStatistics

AIStatistics

Các bảng trong report ưu tiên dữ liệu tổng hợp (Aggregated Data).

---

# 4.12 config Schema

Schema config lưu cấu hình hệ thống.

Ví dụ

SystemSettings

FeatureFlags

ApplicationConfigurations

NotificationConfigurations

GISConfigurations

AIConfigurations

Không Hard-code cấu hình trong Source Code nếu có thể lưu trong config.

---

# 4.13 Schema Dependency Rules

Các Schema phải phụ thuộc theo chiều sau.

```text
ref
 │
 ▼
auth
 │
 ▼
dbo
 │
 ├───────────────┐
 ▼               ▼
gis             zalo
 │               │
 └───────┬───────┘
         ▼
        ai
         │
         ▼
      report
         │
         ▼
       audit
```

Không được tạo Circular Dependency giữa các Schema.

---

# 4.14 Security Rules

Mỗi Schema có thể được cấp quyền riêng.

Ví dụ

Application User

SELECT

INSERT

UPDATE

trên dbo

Nhưng

không có quyền

ALTER

DROP

trên auth.

Quyền phải được cấp theo nguyên tắc Least Privilege.

---

# 4.15 Performance Considerations

Schema phải hỗ trợ:

- Partitioning.
- Indexing.
- Backup theo nhóm dữ liệu.
- Restore từng phần.
- Data Archiving.

Không thiết kế Schema gây cản trở tối ưu hiệu năng.

---

# 4.16 AI Compliance

AI Coding Agent phải:

- Sinh đúng Schema.
- Không tạo bảng sai Schema.
- Không đưa bảng AI vào dbo.
- Không đưa bảng Audit vào Business Schema.
- Tuân thủ đúng kiến trúc Database.

---

# 4.17 Future Expansion

Kiến trúc Schema phải hỗ trợ mở rộng.

Ví dụ

iot

mobile

documents

workflow

payments

integration

mà không làm thay đổi Schema hiện tại.

---

# 4.18 Governance Rules

Schema mới chỉ được tạo khi:

- Có Domain mới.
- Có ADR được phê duyệt.
- Có Documentation Update.
- Có Review bởi Solution Architect.

Không tạo Schema tùy ý.

---

# 4.19 Acceptance Criteria

Phase 4 được xem là hoàn thành khi:

- Danh sách Schema được xác định.
- Trách nhiệm từng Schema rõ ràng.
- Không có chồng chéo Domain.
- Có khả năng mở rộng.
- Đáp ứng yêu cầu bảo mật và hiệu năng.

---

# 4.20 Phase Summary

Phase 4 thiết lập tiêu chuẩn thiết kế Schema cho AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Database được tổ chức theo Domain.
- Các Schema có trách nhiệm rõ ràng.
- Quyền truy cập có thể quản lý theo từng Schema.
- AI có cơ sở để sinh đúng vị trí của từng bảng dữ liệu.
- Hệ thống sẵn sàng mở rộng mà không ảnh hưởng kiến trúc hiện có.

Từ Phase 5 trở đi, mọi bảng dữ liệu sẽ tuân thủ cấu trúc Schema đã được xác lập trong Phase này.
# =============================================================================
# PHASE 5 – PRIMARY KEY STANDARDS
# =============================================================================

# 5.1 Purpose

Phase này định nghĩa tiêu chuẩn sử dụng Primary Key (PK) trong toàn bộ cơ sở dữ liệu của dự án AnSinhSo Enterprise.

Primary Key là định danh duy nhất của mỗi bản ghi và là nền tảng cho:

- Data Integrity
- Entity Framework Mapping
- Relationship Management
- Performance Optimization
- Data Synchronization
- AI Analytics
- GIS Integration

Mọi bảng dữ liệu đều phải có Primary Key.

---

# 5.2 Design Principles

Primary Key phải tuân thủ các nguyên tắc sau:

## Principle 1 – Uniqueness

Mỗi bản ghi chỉ có một Primary Key duy nhất.

Không được phép tồn tại hai bản ghi có cùng giá trị Primary Key.

---

## Principle 2 – Immutability

Primary Key không được thay đổi sau khi bản ghi được tạo.

Không sử dụng dữ liệu nghiệp vụ làm Primary Key.

Ví dụ:

❌ CCCD

❌ Số điện thoại

❌ Mã hộ gia đình

✔ Id

---

## Principle 3 – Simplicity

Primary Key phải đơn giản.

Không sử dụng Composite Primary Key nếu không có lý do đặc biệt.

Ưu tiên:

Một cột

↓

Một khóa chính

---

## Principle 4 – Independence

Primary Key không phụ thuộc vào nghiệp vụ.

Nếu nghiệp vụ thay đổi thì Primary Key vẫn giữ nguyên.

---

# 5.3 Standard Primary Key

Toàn bộ bảng nghiệp vụ sử dụng:

```sql
Id
```

Ví dụ:

```text
Users
--------
Id

Households
--------
Id

Citizens
--------
Id

BenefitPayments
--------
Id
```

Không sử dụng:

```
UserId
CitizenId
PaymentId
```

làm Primary Key.

Các tên này chỉ dành cho Foreign Key.

---

# 5.4 Data Type Strategy

AnSinhSo sử dụng chiến lược sau:

## Business Tables

INT IDENTITY

```sql
Id INT IDENTITY(1,1)
```

Ưu điểm

- Nhanh
- Index nhỏ
- EF Core tối ưu
- SQL Server tối ưu

---

## Distributed Tables (Future)

Khi cần đồng bộ nhiều địa phương:

GUID

```sql
UNIQUEIDENTIFIER
```

hoặc

Sequential GUID

Không sử dụng GUID ở Sprint 01 nếu chưa có yêu cầu phân tán dữ liệu.

---

# 5.5 Identity Strategy

Mặc định:

```sql
IDENTITY(1,1)
```

Ví dụ:

```sql
Id INT IDENTITY(1,1)
PRIMARY KEY
```

Không tự sinh Id từ Source Code.

SQL Server chịu trách nhiệm sinh Identity.

---

# 5.6 Natural Key vs Surrogate Key

AnSinhSo sử dụng:

Surrogate Key

↓

Id

Natural Key (CCCD, Mã hộ, Số hồ sơ...) chỉ sử dụng:

- Business Validation
- Unique Constraint
- Tìm kiếm

Không sử dụng làm Primary Key.

---

# 5.7 Composite Primary Key

Không khuyến khích.

Chỉ sử dụng khi:

- Bảng liên kết nhiều-nhiều (Many-to-Many).
- Không có Entity riêng.

Ví dụ:

UserRoles

```
UserId
RoleId
```

Tuy nhiên, với EF Core 8, ưu tiên tạo:

```text
UserRoles
---------
Id
UserId
RoleId
```

để thuận tiện mở rộng thêm thuộc tính như:

- AssignedDate
- AssignedBy
- IsActive

---

# 5.8 Clustered Index

Primary Key mặc định là:

Clustered Index

Nếu có yêu cầu đặc biệt (Partitioning, Data Warehouse...), cần có Architecture Review trước khi thay đổi.

---

# 5.9 Entity Framework Core Rules

Entity phải khai báo:

```csharp
public int Id { get; set; }
```

Không sử dụng tên khác như:

```csharp
public int UserId { get; set; }      ❌
public int HouseholdID { get; set; } ❌
```

EF Core sẽ tự nhận diện `Id` là khóa chính theo quy ước.

---

# 5.10 Reserved Keys

Các bảng danh mục (Reference Data) vẫn sử dụng:

```sql
Id INT IDENTITY
```

Không hard-code giá trị:

```
Id = 1
Id = 2
Id = 3
```

trong Source Code.

Nếu cần giá trị cố định cho Seed Data, phải được quản lý trong Migration hoặc Seed Script.

---

# 5.11 Performance Considerations

Primary Key phải:

- Có kích thước nhỏ.
- Không thay đổi.
- Được lập chỉ mục.
- Hỗ trợ Join hiệu quả.
- Tối ưu Insert và Update.

Không sử dụng kiểu dữ liệu lớn (NVARCHAR, TEXT...) làm Primary Key.

---

# 5.12 AI Compliance

AI Coding Agent phải:

- Tạo đúng cột `Id`.
- Không tự đổi kiểu dữ liệu.
- Không sử dụng Natural Key làm PK.
- Tuân thủ Identity Strategy.
- Ánh xạ đúng với Entity Framework Core.

---

# 5.13 Governance Rules

Mọi thay đổi Primary Key phải:

- Được cập nhật Documentation.
- Được tạo Migration.
- Được kiểm thử trên môi trường Development.
- Được phê duyệt bởi Solution Architect trước khi áp dụng Production.

Không thay đổi Primary Key trực tiếp trên Production Database.

---

# 5.14 Acceptance Criteria

Phase 5 được xem là hoàn thành khi:

- Tất cả các bảng có Primary Key.
- Primary Key tuân thủ chuẩn `Id`.
- Kiểu dữ liệu được thống nhất.
- Không sử dụng dữ liệu nghiệp vụ làm khóa chính.
- Entity Framework Core ánh xạ chính xác.

---

# 5.15 Phase Summary

Phase 5 thiết lập tiêu chuẩn Primary Key cho toàn bộ Database của AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Mọi bảng đều có định danh thống nhất.
- Entity Framework Core hoạt động theo Convention.
- SQL Server được tối ưu về hiệu năng.
- Kiến trúc dữ liệu sẵn sàng mở rộng lên nhiều địa phương.
- AI Coding Agent có quy tắc rõ ràng để sinh Entity và Migration.

Từ Phase 6 trở đi, mọi quan hệ giữa các bảng sẽ được xây dựng dựa trên chuẩn Primary Key đã được xác lập trong Phase này.
# =============================================================================
# PHASE 6 – FOREIGN KEY STANDARDS
# =============================================================================

# 6.1 Purpose

Phase này định nghĩa tiêu chuẩn thiết kế Foreign Key (FK) cho toàn bộ cơ sở dữ liệu của dự án AnSinhSo Enterprise.

Foreign Key là nền tảng để:

- Đảm bảo Referential Integrity.
- Thiết lập mối quan hệ giữa các Entity.
- Hỗ trợ Entity Framework Core Mapping.
- Tối ưu hóa JOIN.
- Hỗ trợ AI Analytics.
- Hỗ trợ GIS Data Linking.
- Hỗ trợ Reporting.

Mọi quan hệ giữa các bảng phải được thể hiện bằng Foreign Key rõ ràng.

---

# 6.2 Design Principles

Foreign Key phải tuân thủ các nguyên tắc sau.

## Principle 1 – Integrity First

Không được lưu dữ liệu "mồ côi" (Orphan Data).

Ví dụ:

HouseholdMembers.HouseholdId

phải tồn tại trong

Households.Id

---

## Principle 2 – Explicit Relationship

Mọi quan hệ giữa các bảng phải được khai báo rõ ràng.

Không sử dụng quy ước ngầm hoặc chỉ lưu giá trị mà không có ràng buộc.

✔ Có Foreign Key

✘ Chỉ lưu số Id nhưng không tạo FK

---

## Principle 3 – Business Driven

Foreign Key phản ánh đúng quy tắc nghiệp vụ.

Ví dụ:

Citizen

thuộc

Household

Program

có nhiều

BenefitPayments

Không tạo quan hệ chỉ để thuận tiện cho lập trình.

---

## Principle 4 – Stable Reference

Foreign Key luôn tham chiếu đến Primary Key (`Id`) của bảng cha.

Không tham chiếu đến các cột nghiệp vụ như:

- IdentityNumber
- PhoneNumber
- HouseholdCode

---

# 6.3 Naming Convention

Foreign Key sử dụng mẫu:

<EntityName>Id

Ví dụ:

RoleId

UserId

HouseholdId

CitizenId

VillageId

BenefitProgramId

PaymentId

ApplicationId

---

# 6.4 Relationship Types

Database hỗ trợ các loại quan hệ sau.

## One-to-One (1:1)

Ví dụ:

Citizen

↓

CitizenProfile

Mỗi công dân có một hồ sơ chi tiết.

---

## One-to-Many (1:N)

Ví dụ:

Household

↓

HouseholdMembers

Một hộ có nhiều thành viên.

Đây là loại quan hệ phổ biến nhất.

---

## Many-to-Many (N:N)

Ví dụ:

Users

↓

UserRoles

↓

Roles

Bảng trung gian phải là Entity riêng nếu có thêm thuộc tính nghiệp vụ.

---

# 6.5 Optional Relationship

Nếu quan hệ không bắt buộc:

Foreign Key được phép NULL.

Ví dụ:

ApprovedByUserId

Một hồ sơ chưa được duyệt sẽ chưa có người duyệt.

---

# 6.6 Required Relationship

Nếu nghiệp vụ bắt buộc:

Foreign Key phải NOT NULL.

Ví dụ:

Citizen

luôn thuộc

Household

HouseholdId

NOT NULL

---

# 6.7 Delete Behavior

Delete Behavior phải được quy định rõ.

## Restrict (Mặc định)

Ưu tiên sử dụng.

Ví dụ:

Không được xóa Household nếu vẫn còn HouseholdMembers.

---

## Cascade

Chỉ sử dụng khi dữ liệu phụ thuộc hoàn toàn.

Ví dụ:

RefreshTokens

↓

User

Xóa User có thể xóa RefreshToken.

---

## Set Null

Áp dụng cho quan hệ tùy chọn.

Ví dụ:

ApprovedByUserId

Khi User nghỉ việc:

ApprovedByUserId

↓

NULL

---

Không sử dụng Cascade Delete tràn lan.

---

# 6.8 Update Behavior

Primary Key không được cập nhật.

Do đó:

ON UPDATE CASCADE

không được sử dụng trong thiết kế chuẩn.

---

# 6.9 Cross-Schema Relationship

Foreign Key được phép tham chiếu giữa các Schema nếu:

- Có quan hệ nghiệp vụ rõ ràng.
- Được Architecture Review.

Ví dụ:

dbo.BenefitPayments

↓

auth.Users

↓

ApprovedByUserId

---

# 6.10 Entity Framework Core Rules

Entity phải khai báo đầy đủ:

```csharp
public int HouseholdId { get; set; }

public Household Household { get; set; }
```

Navigation Property phải đồng bộ với Foreign Key.

---

# 6.11 Index Strategy

Mọi Foreign Key phải có Index.

Ví dụ:

IX_HouseholdMembers_HouseholdId

IX_BenefitPayments_CitizenId

IX_Applications_BenefitProgramId

Điều này giúp tối ưu JOIN và truy vấn.

---

# 6.12 Circular Dependency

Không được tạo Circular Dependency.

Ví dụ:

A

↓

B

↓

C

↓

A

Đây là thiết kế không hợp lệ.

---

# 6.13 Soft Delete Consideration

Nếu bảng cha sử dụng Soft Delete:

Không tự động Soft Delete bảng con.

Việc xử lý phụ thuộc vào Business Rules.

---

# 6.14 AI Compliance

AI Coding Agent phải:

- Sinh đầy đủ Foreign Key.
- Sinh Navigation Property tương ứng.
- Không bỏ qua Referential Integrity.
- Không tạo FK dư thừa.
- Tuân thủ Delete Behavior.

---

# 6.15 Governance Rules

Mọi thay đổi Foreign Key phải:

- Cập nhật Documentation.
- Tạo Migration.
- Kiểm thử Migration.
- Đánh giá ảnh hưởng đến dữ liệu.
- Được Review trước khi triển khai.

Không thay đổi Foreign Key trực tiếp trên Production Database.

---

# 6.16 Acceptance Criteria

Phase 6 được xem là hoàn thành khi:

- Mọi quan hệ đều có Foreign Key.
- Naming Convention thống nhất.
- Delete Behavior rõ ràng.
- Không có Circular Dependency.
- Entity Framework Core Mapping chính xác.

---

# 6.17 Best Practices

Luôn ưu tiên:

✔ Restrict Delete

✔ Explicit Foreign Key

✔ Navigation Property đầy đủ

✔ Index cho mọi Foreign Key

✔ Business Rule rõ ràng

Tránh:

✘ Cascade Delete không kiểm soát

✘ Thiếu Foreign Key

✘ Quan hệ mơ hồ

✘ Duplicate Relationship

---

# 6.18 Enterprise Examples

Ví dụ quan hệ chính trong AnSinhSo:

AdministrativeUnit
    │
    ▼
Village
    │
    ▼
Household
    │
    ▼
HouseholdMember
    │
    ▼
Citizen
    │
    ▼
SocialBeneficiary
    │
    ▼
BenefitPayment

Authentication:

Role
    │
    ▼
User
    │
    ▼
RefreshToken

Notification:

Citizen
    │
    ▼
OAUser
    │
    ▼
NotificationQueue
    │
    ▼
DeliveryLog

---

# 6.19 Phase Summary

Phase 6 thiết lập tiêu chuẩn Foreign Key cho toàn bộ cơ sở dữ liệu AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Quan hệ giữa các bảng được chuẩn hóa.
- SQL Server đảm bảo Referential Integrity.
- Entity Framework Core sinh Navigation Property chính xác.
- AI Coding Agent có quy tắc rõ ràng để tạo Relationship.
- Database sẵn sàng cho các Phase tiếp theo về Relationship Rules, Data Types và Audit Standards.
# =============================================================================
# PHASE 7 – RELATIONSHIP DESIGN STANDARDS
# =============================================================================

# 7.1 Concept

## Purpose

Phase này định nghĩa tiêu chuẩn thiết kế mối quan hệ (Relationship Design) giữa các Entity trong cơ sở dữ liệu của dự án AnSinhSo Enterprise.

Relationship Design quyết định:

- Tính toàn vẹn dữ liệu.
- Hiệu năng truy vấn.
- Khả năng mở rộng.
- Kiến trúc Domain.
- Entity Framework Core Mapping.
- AI Code Generation.

Một Relationship tốt sẽ giúp hệ thống dễ bảo trì trong nhiều năm mà không cần thay đổi kiến trúc.

---

## Objectives

Relationship phải:

- phản ánh đúng nghiệp vụ
- đơn giản
- dễ hiểu
- dễ mở rộng
- không dư thừa
- không tạo vòng lặp

---

# 7.2 Relationship Standards

## Standard 1 – Business First

Relationship được tạo dựa trên nghiệp vụ.

Không tạo Relationship chỉ để thuận tiện cho Query.

Ví dụ

✔

Household

↓

HouseholdMember

↓

Citizen

✘

Citizen

↓

BenefitProgram

(nếu nghiệp vụ không tồn tại)

---

## Standard 2 – Aggregate Root

Mỗi Domain phải có Aggregate Root.

Ví dụ

Household

↓

HouseholdMembers

↓

Citizen

Aggregate Root

↓

Household

Mọi thao tác cập nhật phải bắt đầu từ Aggregate Root.

---

## Standard 3 – Navigation Property

Entity Framework phải có đầy đủ Navigation Property.

Ví dụ

```csharp
public class Household
{
    public int Id { get; set; }

    public ICollection<HouseholdMember> Members { get; set; }
}
```

---

Child Entity

```csharp
public class HouseholdMember
{
    public int Id { get; set; }

    public int HouseholdId { get; set; }

    public Household Household { get; set; }
}
```

---

## Standard 4 – Explicit Relationship

Không sử dụng Shadow Property.

Foreign Key phải khai báo rõ.

✔

```csharp
public int HouseholdId { get; set; }
```

✘

để EF tự sinh FK.

---

## Standard 5 – Maximum Relationship Depth

Không thiết kế chuỗi Relationship quá sâu.

Khuyến nghị:

```
A

↓

B

↓

C

↓

D
```

Tối đa

4 cấp.

Nếu sâu hơn phải đánh giá lại Domain Model.

---

# 7.3 Relationship Patterns

## One-to-One

Ví dụ

Citizen

↓

CitizenProfile

---

## One-to-Many

Ví dụ

BenefitProgram

↓

BenefitPayments

---

## Many-to-Many

Ví dụ

User

↓

UserRole

↓

Role

Không sử dụng Many-to-Many trực tiếp nếu bảng trung gian có thêm thuộc tính nghiệp vụ.

---

Self Reference

Ví dụ

AdministrativeUnit

↓

ParentAdministrativeUnit

Áp dụng cho:

- xã
- huyện
- tỉnh

---

Hierarchical Relationship

Ví dụ

Village

↓

Household

↓

Citizen

↓

Beneficiary

---

# 7.4 SQL Server Implementation

Ví dụ

```sql
CREATE TABLE Households
(
    Id INT IDENTITY PRIMARY KEY
);

CREATE TABLE HouseholdMembers
(
    Id INT IDENTITY PRIMARY KEY,

    HouseholdId INT NOT NULL,

    CONSTRAINT FK_HouseholdMembers_Households

    FOREIGN KEY (HouseholdId)

    REFERENCES Households(Id)
);
```

---

Không tạo Foreign Key sau khi đã đưa Database vào Production nếu không có Migration.

---

# 7.5 Entity Framework Core Implementation

Sử dụng Fluent API.

```csharp
builder.Entity<HouseholdMember>()

.HasOne(x => x.Household)

.WithMany(x => x.Members)

.HasForeignKey(x => x.HouseholdId)

.OnDelete(DeleteBehavior.Restrict);
```

Ưu tiên Fluent API hơn Data Annotation đối với các Relationship quan trọng.

---

# 7.6 Delete Strategy

Mặc định

```
DeleteBehavior.Restrict
```

Áp dụng cho

Household

Citizen

Beneficiary

Payment

---

Cascade chỉ dùng khi dữ liệu không có ý nghĩa độc lập.

Ví dụ

RefreshToken

↓

User

---

SetNull

chỉ dùng cho

Optional Relationship

Ví dụ

ApprovedByUserId

---

# 7.7 Performance Guidelines

Relationship phải hỗ trợ:

✔ JOIN nhanh

✔ Index đầy đủ

✔ Query tối ưu

✔ Include hợp lý

Không tạo Navigation Property không sử dụng.

---

# 7.8 Anti-Patterns

Không được:

Multiple Cascade Delete

Circular Relationship

Duplicate Relationship

Cross Domain Relationship không cần thiết

Relationship không có Foreign Key

Business Logic phụ thuộc Relationship sai

---

# 7.9 AI Compliance

AI Coding Agent phải:

- Sinh đầy đủ Navigation Property.
- Sinh Fluent API.
- Sinh Foreign Key.
- Không tạo Circular Relationship.
- Không tạo Shadow Property.
- Không sinh Cascade Delete mặc định.
- Tuân thủ Aggregate Root.

---

# 7.10 Governance Rules

Relationship mới chỉ được thêm khi:

- Có Business Requirement.
- Có Documentation Update.
- Có Architecture Review.
- Có Migration.
- Có Unit Test.
- Có Integration Test.

---

# 7.11 Acceptance Criteria

Phase được xem là hoàn thành khi:

✓ Relationship phản ánh đúng nghiệp vụ.

✓ Navigation Property đầy đủ.

✓ Fluent API đầy đủ.

✓ Không có Circular Dependency.

✓ Delete Strategy rõ ràng.

✓ Aggregate Root xác định.

✓ AI có thể sinh Entity chính xác.

---

# 7.12 Phase Summary

Phase 7 xác lập tiêu chuẩn thiết kế Relationship cho toàn bộ Database của AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Các Domain được liên kết theo đúng nghiệp vụ.
- Entity Framework Core có thể sinh Mapping nhất quán.
- SQL Server đảm bảo Referential Integrity.
- AI Coding Agent có quy tắc rõ ràng để tạo Entity Relationship.
- Kiến trúc dữ liệu sẵn sàng cho các Phase tiếp theo về Data Types, Audit Fields và Soft Delete.
# =============================================================================
# PHASE 8 – DATA TYPE STANDARDS
# =============================================================================

# 8.1 Concept

## Purpose

Phase này định nghĩa tiêu chuẩn lựa chọn kiểu dữ liệu (Data Type Standards) cho toàn bộ cơ sở dữ liệu của dự án AnSinhSo Enterprise.

Mục tiêu:

- Đảm bảo tính nhất quán.
- Tối ưu dung lượng lưu trữ.
- Tăng hiệu năng truy vấn.
- Hỗ trợ Entity Framework Core Mapping.
- Hỗ trợ AI Coding Agent sinh Entity chính xác.
- Giảm rủi ro chuyển đổi kiểu dữ liệu.

Mọi bảng dữ liệu trong hệ thống phải tuân thủ các tiêu chuẩn của Phase này.

---

## Objectives

Data Type phải đáp ứng:

- Chính xác.
- Tiết kiệm dung lượng.
- Dễ mở rộng.
- Dễ Mapping với EF Core.
- Tối ưu SQL Server.
- Dễ bảo trì.

---

# 8.2 General Standards

## Standard 1 – Use Native SQL Server Data Types

Chỉ sử dụng kiểu dữ liệu chuẩn của SQL Server.

Ví dụ:

✔ INT

✔ BIGINT

✔ BIT

✔ DECIMAL

✔ NVARCHAR

✔ DATETIME2

✔ UNIQUEIDENTIFIER

Không sử dụng các kiểu dữ liệu đã lỗi thời.

---

## Standard 2 – Smallest Suitable Type

Luôn chọn kiểu dữ liệu nhỏ nhất nhưng vẫn đáp ứng yêu cầu.

Ví dụ:

Tuổi

→ TINYINT

Năm

→ SMALLINT

Số lượng

→ INT

Dung lượng lớn

→ BIGINT

---

## Standard 3 – Unicode First

Toàn bộ dữ liệu văn bản sử dụng:

```sql
NVARCHAR
```

Không sử dụng:

```sql
VARCHAR
```

đối với dữ liệu tiếng Việt.

---

## Standard 4 – Avoid MAX

Không sử dụng:

```
NVARCHAR(MAX)
```

trừ khi thực sự cần.

Ví dụ:

✔ Description

NVARCHAR(1000)

✔ Notes

NVARCHAR(2000)

✘ NVARCHAR(MAX)

---

# 8.3 Numeric Standards

| Business Data | SQL Type |
|--------------|----------|
| Primary Key | INT |
| Large Counter | BIGINT |
| Percentage | DECIMAL(5,2) |
| Money | DECIMAL(18,2) |
| Quantity | INT |
| Status | TINYINT |
| Boolean | BIT |

---

Không sử dụng:

FLOAT

REAL

để lưu tiền.

---

# 8.4 Text Standards

| Data | SQL Type |
|------|----------|
| Full Name | NVARCHAR(200) |
| Address | NVARCHAR(500) |
| Phone Number | NVARCHAR(20) |
| Identity Number | NVARCHAR(20) |
| Email | NVARCHAR(255) |
| Username | NVARCHAR(100) |
| Password Hash | NVARCHAR(500) |
| URL | NVARCHAR(500) |
| Description | NVARCHAR(1000) |

---

Không sử dụng:

CHAR

trừ khi độ dài luôn cố định.

---

# 8.5 Date & Time Standards

Toàn bộ hệ thống sử dụng:

```sql
DATETIME2
```

Ví dụ:

CreatedDate

UpdatedDate

DeletedDate

PaymentDate

ApprovedDate

SubmittedDate

Không sử dụng:

```
DATETIME
```

```
SMALLDATETIME
```

---

## Time Zone

Tất cả thời gian lưu theo:

UTC

Khi hiển thị:

Chuyển đổi sang múi giờ người dùng.

---

# 8.6 Boolean Standards

Boolean sử dụng:

```sql
BIT
```

Ví dụ:

IsActive

IsDeleted

IsApproved

HasChildren

CanReceiveBenefit

Không sử dụng:

```
INT
```

để biểu diễn giá trị Boolean.

---

# 8.7 Decimal Standards

Tiền:

```sql
DECIMAL(18,2)
```

Tỷ lệ:

```sql
DECIMAL(5,2)
```

Điểm đánh giá:

```sql
DECIMAL(4,2)
```

Không sử dụng:

```
FLOAT
DOUBLE
REAL
```

---

# 8.8 Identifier Standards

Primary Key

↓

INT IDENTITY

GUID khi cần:

```sql
UNIQUEIDENTIFIER
```

Token:

```sql
UNIQUEIDENTIFIER
```

CorrelationId:

```sql
UNIQUEIDENTIFIER
```

---

# 8.9 Spatial Data Standards

GIS sử dụng:

```sql
geometry
```

hoặc

```sql
geography
```

Latitude:

```sql
DECIMAL(10,7)
```

Longitude:

```sql
DECIMAL(10,7)
```

GeoJSON:

```sql
NVARCHAR(MAX)
```

(chỉ khi cần lưu dữ liệu thô)

---

# 8.10 AI Data Standards

Prompt:

```sql
NVARCHAR(MAX)
```

Embedding Vector:

Lưu ngoài Database hoặc dưới dạng VARBINARY(MAX) nếu có yêu cầu đặc biệt.

Prediction Score:

```sql
DECIMAL(5,4)
```

Confidence:

```sql
DECIMAL(5,4)
```

Model Version:

```sql
NVARCHAR(100)
```

---

# 8.11 File Standards

Tên File

↓

NVARCHAR(255)

Đường dẫn

↓

NVARCHAR(1000)

Hash

↓

NVARCHAR(256)

Dung lượng

↓

BIGINT

MIME Type

↓

NVARCHAR(100)

Không lưu File Binary trực tiếp trong SQL Server nếu không có yêu cầu đặc biệt.

---

# 8.12 Entity Framework Core Mapping

| SQL Server | EF Core |
|------------|---------|
| INT | int |
| BIGINT | long |
| BIT | bool |
| DECIMAL(18,2) | decimal |
| DATETIME2 | DateTime |
| UNIQUEIDENTIFIER | Guid |
| NVARCHAR | string |

Entity Framework phải ánh xạ đúng kiểu dữ liệu.

---

# 8.13 Best Practices

✔ Sử dụng NVARCHAR cho dữ liệu tiếng Việt.

✔ Sử dụng DECIMAL cho tiền.

✔ Sử dụng DATETIME2 cho thời gian.

✔ Sử dụng BIT cho Boolean.

✔ Chọn độ dài phù hợp.

✔ Tránh NVARCHAR(MAX) khi không cần thiết.

✔ Thiết kế theo nhu cầu thực tế.

---

# 8.14 AI Compliance

AI Coding Agent phải:

- Không tự ý thay đổi Data Type.
- Tuân thủ Mapping SQL Server ↔ EF Core.
- Không sử dụng kiểu dữ liệu lỗi thời.
- Không sử dụng FLOAT cho dữ liệu tài chính.
- Không sử dụng VARCHAR cho dữ liệu Unicode.
- Không sinh NVARCHAR(MAX) nếu không có yêu cầu.

---

# 8.15 Governance Rules

Mọi thay đổi Data Type phải:

- Được phân tích tác động.
- Được cập nhật Documentation.
- Có Migration tương ứng.
- Được kiểm thử trên môi trường Development.
- Được phê duyệt trước khi triển khai Production.

---

# 8.16 Acceptance Criteria

Phase 8 được xem là hoàn thành khi:

✓ Kiểu dữ liệu được chuẩn hóa.

✓ SQL Server và EF Core Mapping nhất quán.

✓ Không sử dụng kiểu dữ liệu lỗi thời.

✓ Dữ liệu Unicode được hỗ trợ đầy đủ.

✓ Thiết kế tối ưu cho hiệu năng và khả năng mở rộng.

---

# 8.17 Phase Summary

Phase 8 thiết lập tiêu chuẩn lựa chọn kiểu dữ liệu cho toàn bộ cơ sở dữ liệu AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Tất cả Entity sử dụng kiểu dữ liệu thống nhất.
- SQL Server được tối ưu về lưu trữ và hiệu năng.
- Entity Framework Core ánh xạ chính xác.
- AI Coding Agent có quy tắc rõ ràng để sinh Entity và Migration.
- Hệ thống sẵn sàng cho các Phase tiếp theo về Audit Fields, Soft Delete và Index Standards.
# =============================================================================
# PHASE 9 – AUDIT FIELDS STANDARDS
# =============================================================================

# 9.1 Concept

## Purpose

Phase này định nghĩa tiêu chuẩn quản lý Audit Fields cho toàn bộ cơ sở dữ liệu của dự án AnSinhSo Enterprise.

Audit Fields giúp hệ thống:

- Theo dõi lịch sử thay đổi dữ liệu.
- Xác định người tạo và người cập nhật.
- Hỗ trợ kiểm toán.
- Hỗ trợ truy vết sự cố.
- Đáp ứng yêu cầu bảo mật.
- Hỗ trợ AI Analytics.
- Đáp ứng yêu cầu chuyển đổi số trong cơ quan nhà nước.

Audit không phải là tính năng tùy chọn.

Mọi bảng nghiệp vụ đều phải hỗ trợ Audit.

---

## Objectives

Audit phải bảo đảm:

- Truy vết đầy đủ.
- Không làm ảnh hưởng hiệu năng.
- Dễ mở rộng.
- Dễ tích hợp với Logging.
- Dễ tích hợp với Security Monitoring.

---

# 9.2 Audit Standards

## Standard 1 – Required Audit Fields

Mọi Business Table bắt buộc có các trường sau:

| Field | Required |
|---------|----------|
| CreatedDate | Yes |
| CreatedBy | Yes |
| UpdatedDate | Yes |
| UpdatedBy | Yes |
| DeletedDate | Soft Delete |
| DeletedBy | Soft Delete |
| IsDeleted | Yes |
| RowVersion | Yes |

---

## Standard 2 – Immutable Creation Information

Các trường:

CreatedDate

CreatedBy

không được cập nhật sau khi bản ghi được tạo.

---

## Standard 3 – Update Tracking

Mỗi lần UPDATE phải ghi nhận:

UpdatedDate

UpdatedBy

---

## Standard 4 – Soft Delete Tracking

Nếu bản ghi bị xóa logic:

IsDeleted = 1

DeletedDate = UTC Now

DeletedBy = Current User

---

## Standard 5 – Concurrency Control

Mọi bảng nghiệp vụ phải có:

RowVersion

để hỗ trợ Optimistic Concurrency.

---

# 9.3 Standard Audit Columns

```sql
CreatedDate
DATETIME2

CreatedBy
INT

UpdatedDate
DATETIME2

UpdatedBy
INT

DeletedDate
DATETIME2 NULL

DeletedBy
INT NULL

IsDeleted
BIT

RowVersion
ROWVERSION
```

---

Không đổi tên các trường này.

---

# 9.4 SQL Server Implementation

Ví dụ:

```sql
CreatedDate DATETIME2 NOT NULL,

CreatedBy INT NOT NULL,

UpdatedDate DATETIME2 NULL,

UpdatedBy INT NULL,

DeletedDate DATETIME2 NULL,

DeletedBy INT NULL,

IsDeleted BIT NOT NULL DEFAULT(0),

RowVersion ROWVERSION
```

Không sử dụng TIMESTAMP.

ROWVERSION là chuẩn của SQL Server.

---

# 9.5 Entity Framework Core Implementation

Base Entity:

```csharp
public abstract class BaseAuditableEntity
{
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }

    public byte[] RowVersion { get; set; } = default!;
}
```

Fluent API:

```csharp
builder.Property(x => x.RowVersion)
       .IsRowVersion();
```

---

# 9.6 Automatic Audit Population

Audit Fields phải được tự động gán thông qua:

- SaveChangesInterceptor
- DbContext
- Middleware
- CurrentUserService

Không cho phép Controller tự gán Audit Fields.

---

# 9.7 Best Practices

✔ Tự động ghi nhận thời gian UTC.

✔ Không cho phép Client gửi CreatedDate.

✔ Không cho phép Client sửa CreatedBy.

✔ Không cho phép Client sửa RowVersion.

✔ Audit được xử lý tại Infrastructure Layer.

---

# 9.8 AI Compliance

AI Coding Agent phải:

- Sinh BaseAuditableEntity.
- Sinh RowVersion.
- Sinh SaveChangesInterceptor.
- Không sinh Audit trong Controller.
- Không bỏ thiếu trường Audit.
- Tuân thủ UTC.

---

# 9.9 Governance Rules

Mọi thay đổi liên quan đến Audit Fields phải:

- Được cập nhật tài liệu.
- Có Migration.
- Được kiểm thử Concurrency.
- Được Review bởi Solution Architect.

---

# 9.10 Acceptance Criteria

Phase được xem là hoàn thành khi:

✓ Tất cả Business Tables có Audit Fields.

✓ EF Core hỗ trợ Concurrency.

✓ Audit tự động hoạt động.

✓ Không có Controller thao tác Audit.

✓ Hệ thống hỗ trợ truy vết đầy đủ.

---

# 9.11 Phase Summary

Phase 9 thiết lập tiêu chuẩn Audit Fields cho toàn bộ Database của AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Mọi thay đổi dữ liệu đều có thể truy vết.
- SQL Server và EF Core hỗ trợ Optimistic Concurrency.
- AI Coding Agent có quy tắc thống nhất để sinh Base Entity.
- Hệ thống đáp ứng yêu cầu kiểm toán, bảo mật và quản trị dữ liệu.
# =============================================================================
# PHASE 10 – SOFT DELETE STANDARDS
# =============================================================================

# 10.1 Concept

## Purpose

Phase này định nghĩa tiêu chuẩn triển khai Soft Delete cho toàn bộ hệ thống AnSinhSo Enterprise.

Soft Delete là cơ chế đánh dấu bản ghi đã bị xóa thay vì xóa vật lý (Physical Delete).

Mục tiêu:

- Bảo vệ dữ liệu.
- Cho phép khôi phục dữ liệu.
- Hỗ trợ Audit.
- Hỗ trợ điều tra sự cố.
- Đáp ứng yêu cầu kiểm toán.
- Đảm bảo an toàn khi vận hành Production.

Trong AnSinhSo Enterprise:

**Business Data không được phép xóa vật lý mặc định.**

---

## Objectives

Soft Delete phải đảm bảo:

- Không mất dữ liệu.
- Không ảnh hưởng Integrity.
- Có thể Restore.
- Có thể Purge theo chính sách.
- Không làm thay đổi Business Logic.
- Hoạt động thống nhất trên toàn hệ thống.

---

# 10.2 Soft Delete Principles

## Principle 1 – Logical Delete First

Mặc định:

DELETE

↓

Soft Delete

Không sử dụng:

```sql
DELETE FROM Households
```

trên dữ liệu nghiệp vụ.

---

## Principle 2 – Recoverability

Dữ liệu phải có khả năng phục hồi.

Ví dụ

Citizen

↓

Delete

↓

Restore

↓

Tiếp tục sử dụng

---

## Principle 3 – Auditability

Khi Soft Delete phải ghi nhận:

DeletedDate

DeletedBy

Reason (nếu có)

---

## Principle 4 – Business Safety

Không cho phép xóa dữ liệu nghiệp vụ nếu chưa được xác nhận.

Ví dụ

Không được xóa:

Household

nếu còn:

Citizen

BenefitPayment

Application

---

# 10.3 Standard Soft Delete Fields

Mọi Business Entity sử dụng:

```sql
IsDeleted BIT NOT NULL DEFAULT(0)

DeletedDate DATETIME2 NULL

DeletedBy INT NULL
```

Không đổi tên.

---

# 10.4 SQL Server Implementation

Ví dụ:

```sql
ALTER TABLE Households

ADD

IsDeleted BIT NOT NULL DEFAULT(0),

DeletedDate DATETIME2 NULL,

DeletedBy INT NULL;
```

Khi Soft Delete:

```sql
UPDATE Households

SET

IsDeleted = 1,

DeletedDate = SYSUTCDATETIME(),

DeletedBy = @CurrentUserId

WHERE Id=@Id;
```

Không sử dụng DELETE trực tiếp.

---

# 10.5 Entity Framework Core Implementation

BaseAuditableEntity:

```csharp
public bool IsDeleted { get; set; }

public DateTime? DeletedDate { get; set; }

public int? DeletedBy { get; set; }
```

---

Global Query Filter:

```csharp
builder.Entity<Household>()

.HasQueryFilter(x => !x.IsDeleted);
```

Hoặc:

```csharp
modelBuilder.Entity<TEntity>()

.HasQueryFilter(x => !x.IsDeleted);
```

áp dụng cho toàn bộ Entity.

---

# 10.6 SaveChanges Interceptor

Khi EF Core nhận:

```csharp
context.Remove(entity);
```

Interceptor phải chuyển thành:

```csharp
entity.IsDeleted = true;

entity.DeletedDate = DateTime.UtcNow;

entity.DeletedBy = CurrentUserId;

entry.State = EntityState.Modified;
```

Application không được phép gọi DELETE vật lý.

---

# 10.7 Restore Strategy

Khi Restore:

```sql
IsDeleted = 0

DeletedDate = NULL

DeletedBy = NULL
```

Lịch sử Audit vẫn được giữ nguyên.

---

# 10.8 Purge Strategy

Purge là xóa vật lý.

Chỉ được thực hiện khi:

- Hết thời gian lưu trữ.
- Có phê duyệt.
- Có Backup.
- Có Audit.

Ví dụ

90 ngày

180 ngày

5 năm

theo chính sách lưu trữ.

---

# 10.9 Cascade Delete Policy

Không Cascade Soft Delete mặc định.

Ví dụ

Household

↓

HouseholdMembers

↓

Citizen

↓

Beneficiary

↓

Payment

Business Service quyết định Entity nào cần Soft Delete.

Không để SQL Server tự Cascade.

---

# 10.10 Query Standards

Mặc định:

```sql
WHERE

IsDeleted = 0
```

Không viết Query bỏ qua Soft Delete nếu không có yêu cầu.

---

Trong EF Core:

Global Query Filter xử lý tự động.

---

# 10.11 Administration Features

Hệ thống phải hỗ trợ:

✔ Xem Recycle Bin

✔ Restore

✔ Purge

✔ Xem Deleted By

✔ Xem Deleted Date

---

# 10.12 Performance Considerations

Tạo Index:

```sql
IX_Table_IsDeleted
```

Nếu bảng lớn:

Composite Index

```sql
IX_Table_IsDeleted_Status
```

để tối ưu Query.

---

# 10.13 Security Rules

Chỉ:

Administrator

hoặc

System Manager

được Purge dữ liệu.

Người dùng thông thường:

Không có quyền Purge.

---

# 10.14 AI Compliance

AI Coding Agent phải:

- Sinh Global Query Filter.
- Sinh Soft Delete Interceptor.
- Không sinh DELETE vật lý mặc định.
- Sinh API Restore.
- Sinh API Recycle Bin.
- Sinh API Purge theo Role.

---

# 10.15 Governance Rules

Mọi thay đổi liên quan Soft Delete phải:

- Có ADR.
- Có Documentation Update.
- Có Migration.
- Có Unit Test.
- Có Integration Test.
- Có Security Review.

---

# 10.16 Acceptance Criteria

Phase được xem là hoàn thành khi:

✓ Không có Business Table sử dụng DELETE vật lý.

✓ Global Query Filter hoạt động.

✓ Restore hoạt động.

✓ Purge được phân quyền.

✓ Audit đầy đủ.

✓ Không mất dữ liệu.

---

# 10.17 Best Practices

✔ Soft Delete cho Business Data.

✔ Physical Delete cho Temporary Data.

✔ Purge theo lịch.

✔ Có Recycle Bin.

✔ Có Audit.

✔ Có Restore.

---

# 10.18 Enterprise Example

Business Flow

```text
User

↓

Delete Household

↓

Business Validation

↓

Soft Delete

↓

Audit Log

↓

Recycle Bin

↓

Restore (optional)

↓

Purge (Administrator only)
```

---

# 10.19 Integration with Other Components

Soft Delete phải tích hợp với:

- Audit Logging
- Security Monitoring
- Notification
- Dashboard
- AI Analytics
- GIS Data
- Reporting

Mọi thành phần phải hiểu trạng thái `IsDeleted`.

---

# 10.20 Phase Summary

Phase 10 thiết lập tiêu chuẩn Soft Delete cho toàn bộ cơ sở dữ liệu AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Business Data được bảo vệ khỏi việc xóa nhầm.
- EF Core sử dụng Global Query Filter thống nhất.
- SQL Server chỉ thực hiện xóa vật lý theo chính sách.
- AI Coding Agent có quy tắc rõ ràng để sinh Repository, Service và API.
- Hệ thống đáp ứng yêu cầu Audit, Compliance và Production Safety.
# =============================================================================
# PHASE 11 – INDEX STANDARDS
# =============================================================================

# 11.1 Concept

## Purpose

Phase này định nghĩa tiêu chuẩn thiết kế Index cho toàn bộ cơ sở dữ liệu của dự án AnSinhSo Enterprise.

Index là thành phần quyết định hiệu năng của hệ thống.

Một thiết kế Index tốt sẽ:

- Giảm thời gian truy vấn.
- Giảm CPU.
- Giảm IO.
- Tăng tốc Dashboard.
- Tăng tốc AI Analytics.
- Tăng tốc GIS Query.
- Tăng tốc Reporting.

Ngược lại, Index sai sẽ làm:

- INSERT chậm.
- UPDATE chậm.
- DELETE chậm.
- Database phình to.
- Fragmentation cao.

Do đó Index phải được thiết kế có chủ đích.

---

# 11.2 Objectives

Index phải:

- phục vụ nghiệp vụ
- tối ưu Query
- hạn chế dư thừa
- dễ bảo trì
- dễ mở rộng
- hỗ trợ SQL Server Optimizer

---

# 11.3 Index Principles

## Principle 1 – Business Driven

Không tạo Index theo cảm tính.

Index chỉ được tạo khi:

✔ Query thường xuyên

✔ JOIN thường xuyên

✔ Search

✔ Sort

✔ Filter

✔ Dashboard

---

## Principle 2 – Minimal Indexes

Không tạo quá nhiều Index.

Mỗi Index đều làm tăng:

- INSERT Cost
- UPDATE Cost
- Storage

---

## Principle 3 – Cover Important Queries

Index phải phục vụ:

- Search

- Paging

- Dashboard

- Reporting

- API

---

## Principle 4 – Review Periodically

Index phải được đánh giá định kỳ.

Không tồn tại "Index tạo một lần dùng mãi mãi".

---

# 11.4 Clustered Index Standards

Mặc định:

Primary Key

↓

Clustered Index

Ví dụ

```sql
PRIMARY KEY CLUSTERED (Id)
```

Không thay đổi nếu chưa có Architecture Review.

---

# 11.5 Nonclustered Index Standards

Tạo Nonclustered Index cho:

Foreign Key

Search Field

Business Code

Status

CreatedDate

UpdatedDate

IsDeleted

Ví dụ

```sql
CREATE INDEX IX_Citizens_IdentityNumber
ON Citizens(IdentityNumber);
```

---

# 11.6 Composite Index

Composite Index được tạo khi Query sử dụng nhiều điều kiện.

Ví dụ

```sql
WHERE

VillageId

AND

Status

AND

IsDeleted
```

↓

```sql
IX_Citizens_VillageId_Status_IsDeleted
```

Thứ tự cột phải theo tần suất lọc.

---

# 11.7 Unique Index

Dùng cho dữ liệu duy nhất.

Ví dụ

CCCD

Email

Username

HouseholdCode

```sql
CREATE UNIQUE INDEX
```

Không dùng Constraint và Unique Index đồng thời cho cùng mục đích.

---

# 11.8 Filtered Index

Áp dụng cho dữ liệu lớn.

Ví dụ

```sql
WHERE

IsDeleted = 0
```

hoặc

```sql
Status = 1
```

Giúp giảm kích thước Index.

---

# 11.9 Covering Index

Nếu Query thường xuyên:

SELECT

Name

Phone

Status

CreatedDate

thì sử dụng:

```sql
INCLUDE(...)
```

Ví dụ

```sql
CREATE INDEX IX_Citizens_Search

ON Citizens(Name)

INCLUDE

(

PhoneNumber,

Status,

VillageId

);
```

---

# 11.10 SQL Server Implementation

Tên Index:

```
IX_Table_Column
```

Ví dụ

IX_Users_UserName

IX_Households_VillageId

IX_Payments_PaymentDate

IX_Citizens_IdentityNumber

---

# 11.11 Entity Framework Core Implementation

Fluent API

```csharp
builder.HasIndex(x => x.IdentityNumber)

.IsUnique();
```

Composite

```csharp
builder.HasIndex(x => new
{
    x.VillageId,
    x.Status
});
```

Không tạo Index bằng Migration thủ công nếu có thể cấu hình trong Model.

---

# 11.12 GIS Index

Spatial Table phải có:

Spatial Index

Ví dụ

```sql
CREATE SPATIAL INDEX
```

Áp dụng:

geometry

geography

---

# 11.13 AI Tables

AI Tables ưu tiên Index cho:

PredictionDate

ModelVersion

PromptType

CreatedDate

Không Index Embedding Vector.

---

# 11.14 Audit Tables

AuditLogs

ưu tiên:

CreatedDate

UserId

ActionType

EntityName

---

# 11.15 Performance Best Practices

✔ Không Index cột có Selectivity thấp.

✔ Không Index cột BIT đơn lẻ.

✔ Không Index NVARCHAR(MAX).

✔ Không Index TEXT.

✔ Không Index XML khi chưa cần.

---

# 11.16 Index Maintenance

Định kỳ:

- Rebuild

- Reorganize

- Update Statistics

Theo kế hoạch bảo trì.

---

# 11.17 AI Compliance

AI Coding Agent phải:

- Sinh Index đúng Naming Convention.
- Không sinh Index dư thừa.
- Sinh Composite Index đúng thứ tự.
- Sinh Unique Index khi cần.
- Sinh Spatial Index cho GIS.

---

# 11.18 Governance Rules

Mọi Index mới phải:

- Có Business Justification.
- Có Benchmark.
- Có Review.
- Có Migration.
- Có Documentation Update.

---

# 11.19 Acceptance Criteria

Phase hoàn thành khi:

✓ Index phản ánh Query thực tế.

✓ Không dư thừa.

✓ FK đều có Index.

✓ Dashboard được tối ưu.

✓ SQL Server Execution Plan hợp lý.

---

# 11.20 Phase Summary

Phase 11 thiết lập tiêu chuẩn Index cho toàn bộ cơ sở dữ liệu AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- Database đạt hiệu năng cao.
- SQL Server Optimizer hoạt động hiệu quả.
- Entity Framework Core sinh Migration đúng chuẩn.
- AI Coding Agent có quy tắc tạo Index thống nhất.
- Hệ thống sẵn sàng cho khối lượng dữ liệu lớn và mở rộng trong tương lai.
# =============================================================================
# PHASE 12 – DATABASE CONSTRAINT STANDARDS
# =============================================================================

# 12.1 Concept

## Purpose

Phase này định nghĩa tiêu chuẩn sử dụng Database Constraints trong toàn bộ hệ thống AnSinhSo Enterprise.

Constraint là tuyến phòng thủ đầu tiên của Database nhằm đảm bảo:

- Data Integrity
- Business Integrity
- Referential Integrity
- Data Quality
- Security

Nguyên tắc của AnSinhSo Enterprise:

Business Validation thực hiện tại Application Layer.

Data Integrity luôn được bảo vệ tại Database Layer.

Hai lớp này không thay thế nhau mà bổ sung cho nhau.

---

## Objectives

Constraint phải:

- ngăn dữ liệu sai
- dễ hiểu
- dễ bảo trì
- dễ mở rộng
- không gây ảnh hưởng lớn đến hiệu năng
- hỗ trợ Entity Framework Core Migration

---

# 12.2 Constraint Standards

Database sử dụng các loại Constraint sau:

✔ Primary Key

✔ Foreign Key

✔ Unique Constraint

✔ Check Constraint

✔ Default Constraint

Không sử dụng Trigger để thay thế Constraint nếu SQL Server đã hỗ trợ.

---

# 12.3 Primary Key Constraint

Mỗi bảng chỉ có một Primary Key.

Tên:

```text
PK_<TableName>
```

Ví dụ

```text
PK_Users

PK_Citizens

PK_Households
```

---

# 12.4 Foreign Key Constraint

Tên:

```text
FK_<ChildTable>_<ParentTable>
```

Ví dụ

```text
FK_Citizens_Households

FK_Payments_Citizens

FK_UserRoles_Users
```

Delete Behavior mặc định:

```text
Restrict
```

---

# 12.5 Unique Constraint

Unique Constraint dùng cho dữ liệu bắt buộc duy nhất.

Ví dụ

CCCD

Email

Username

HouseholdCode

Tên:

```text
UQ_<Table>_<Column>
```

Ví dụ

```text
UQ_Users_UserName

UQ_Citizens_IdentityNumber
```

---

# 12.6 Check Constraint

Check Constraint bảo vệ dữ liệu hợp lệ.

Ví dụ

Giới tính

```sql
Gender IN ('M','F','O')
```

Trợ cấp

```sql
BenefitAmount >= 0
```

Tuổi

```sql
Age >=0
```

Tên:

```text
CK_<Table>_<Rule>
```

---

# 12.7 Default Constraint

Tên

```text
DF_<Table>_<Column>
```

Ví dụ

```sql
IsDeleted = 0

CreatedDate = SYSUTCDATETIME()

IsActive = 1
```

Không Hard-code giá trị mặc định trong Source Code nếu Database có thể xử lý.

---

# 12.8 SQL Server Implementation

Ví dụ

```sql
CONSTRAINT

PK_Users

PRIMARY KEY(Id)

CONSTRAINT

UQ_Users_UserName

UNIQUE(UserName)

CONSTRAINT

CK_Payment_Amount

CHECK

(

Amount>=0

)

CONSTRAINT

DF_Users_IsActive

DEFAULT(1)
```

---

# 12.9 Entity Framework Core Implementation

Fluent API

```csharp
builder

.HasIndex(x=>x.UserName)

.IsUnique();
```

Default Value

```csharp
builder

.Property(x=>x.IsActive)

.HasDefaultValue(true);
```

Check Constraint

```csharp
builder

.ToTable(t=>t.HasCheckConstraint(

"CK_Payment_Amount",

"[Amount]>=0"

));
```

Ưu tiên Fluent API thay vì Data Annotation.

---

# 12.10 Best Practices

✔ Constraint đặt tại Database.

✔ Validation đặt tại Application.

✔ Constraint có tên rõ ràng.

✔ Không phụ thuộc vào Exception Message.

✔ Không dùng Trigger để kiểm tra dữ liệu đơn giản.

---

# 12.11 Common Mistakes

✘ Không đặt tên Constraint.

✘ Dùng Trigger thay Check Constraint.

✘ Duplicate Validation.

✘ Không tạo Unique cho CCCD.

✘ Không có Default Value.

✘ Dùng Cascade Delete không kiểm soát.

---

# 12.12 Performance Impact

Primary Key

★★★★★

Foreign Key

★★★★☆

Unique

★★★★☆

Check

★★★☆☆

Default

★★★★★

Check Constraint quá phức tạp có thể ảnh hưởng tốc độ INSERT.

---

# 12.13 Production Checklist

□ Constraint đã đặt tên đúng chuẩn.

□ Có Migration.

□ Có Unit Test.

□ Có Integration Test.

□ Có Benchmark.

□ Không xung đột Constraint.

□ Đã Review.

---

# 12.14 AI Compliance

AI Coding Agent phải:

- Sinh đầy đủ Constraint.
- Không bỏ Unique Constraint.
- Không bỏ Check Constraint.
- Đặt tên đúng Convention.
- Không tạo Constraint dư thừa.

---

# 12.15 AI Self Validation Checklist

AI phải tự kiểm tra:

□ PK đã tồn tại?

□ FK đã đúng?

□ Unique Constraint đã có?

□ Check Constraint hợp lý?

□ Default Value đã khai báo?

□ Tên Constraint đúng Convention?

□ EF Core Mapping khớp Database?

---

# 12.16 Governance Rules

Mọi Constraint mới phải:

- Có Business Requirement.
- Có Documentation.
- Có Migration.
- Có Review.
- Có Test.

---

# 12.17 Acceptance Criteria

Phase hoàn thành khi:

✓ Database Integrity được đảm bảo.

✓ Không có dữ liệu sai.

✓ EF Core Migration sinh đúng.

✓ Constraint đặt tên chuẩn.

✓ AI có thể sinh chính xác.

---

# 12.18 Enterprise Example

Citizen

↓

IdentityNumber

↓

Unique Constraint

↓

BenefitPayment

↓

Amount

↓

Check Constraint

↓

CreatedDate

↓

Default Constraint

↓

HouseholdId

↓

Foreign Key

↓

Households

---

# 12.19 Phase Summary

Phase 12 chuẩn hóa toàn bộ Constraint của Database AnSinhSo Enterprise.

Sau khi hoàn thành:

- Database có khả năng tự bảo vệ dữ liệu.
- SQL Server và EF Core hoạt động nhất quán.
- AI Coding Agent có đầy đủ quy tắc để sinh Migration và Entity.
- Hệ thống đáp ứng tiêu chuẩn Enterprise về Data Integrity và Data Governance.
# =============================================================================
# PHASE 13 – ENTITY FRAMEWORK CORE STANDARDS
# =============================================================================

# =============================================================================
# PART I – ENTITY FRAMEWORK CORE ARCHITECTURE
# =============================================================================

# =============================================================================
# CHAPTER 1 – DBCONTEXT STANDARDS
# =============================================================================

# 13.1 Purpose

DbContext là trung tâm của tầng Persistence trong kiến trúc Clean Architecture.

Đây là thành phần chịu trách nhiệm kết nối giữa Domain Model và SQL Server thông qua Entity Framework Core.

Trong AnSinhSo Enterprise, DbContext được xem là Infrastructure Component, không phải Business Component.

Mọi quy tắc trong chương này đều bắt buộc đối với toàn bộ dự án.

---

# 13.2 Architecture Position

DbContext nằm tại:

API

↓

Application

↓

Infrastructure

↓

Persistence

↓

AnSinhSoDbContext

↓

SQL Server

DbContext không được tham chiếu trực tiếp từ Presentation Layer.

---

# 13.3 Responsibilities

DbContext chỉ chịu trách nhiệm:

✓ Entity Tracking

✓ Change Tracking

✓ Transaction Management

✓ SaveChanges

✓ Database Connection

✓ Mapping Entity

✓ Query Execution

✓ Concurrency

✓ Query Filter

✓ Interceptor

DbContext KHÔNG chịu trách nhiệm:

✘ Business Logic

✘ Authorization

✘ Validation

✘ Notification

✘ Email

✘ AI Processing

✘ GIS Processing

✘ Report Generation

---

# 13.4 Standard Architecture

Toàn bộ Solution chỉ sử dụng:

AnSinhSoDbContext

Không tạo:

CitizenDbContext

PaymentDbContext

UserDbContext

BenefitDbContext

Mọi Entity đều thuộc cùng một DbContext.

---

# 13.5 Namespace Standards

DbContext phải nằm tại:

AnSinhSo.Infrastructure.Persistence

Không đặt DbContext trong:

Application

Domain

API

Shared

---

# 13.6 Naming Standards

Tên DbContext:

AnSinhSoDbContext

Không sử dụng:

AppDbContext

DefaultDbContext

MainDbContext

DatabaseContext

Tên phải phản ánh đúng Solution.

---

# 13.7 Folder Structure

Infrastructure/

Persistence/

AnSinhSoDbContext.cs

Configurations/

Interceptors/

Migrations/

Repositories/

Seed/

Extensions/

---

# 13.8 Constructor Standards

DbContext chỉ có một Constructor.

Ví dụ:

public AnSinhSoDbContext(
DbContextOptions<AnSinhSoDbContext> options)
: base(options)
{
}

Không Inject:

ILogger

IMapper

IEmailService

IAIService

IGISService

---

# 13.9 DbContext Lifetime

Đăng ký:

AddDbContext()

Lifetime:

Scoped

Không sử dụng:

Singleton

Transient

---

# 13.10 Dependency Injection Rules

Chỉ Infrastructure được phép đăng ký DbContext.

Program.cs

↓

Infrastructure Extension

↓

AddInfrastructure()

↓

AddDbContext()

Controller không được new DbContext.

---

# 13.11 DbContext Configuration

Connection String

Command Timeout

Retry Policy

Migration Assembly

Enable Sensitive Logging

Environment Based Configuration

được cấu hình trong Infrastructure.

Không Hard-code.

---

# 13.12 Transaction Strategy

DbContext hỗ trợ:

Implicit Transaction

Explicit Transaction

TransactionScope

Execution Strategy

Không Transaction trong Controller.

---

# 13.13 SaveChanges Strategy

Toàn bộ SaveChanges:

↓

Interceptor

↓

Audit

↓

Soft Delete

↓

Concurrency

↓

Save

Controller không gọi Business Logic trước SaveChanges.

---

# 13.14 Query Strategy

Ưu tiên:

AsNoTracking()

đối với Query đọc.

Tracking chỉ dùng khi Update.

---

# 13.15 Performance Rules

DbContext phải:

✓ Pooling

✓ Compiled Query

✓ Batch Save

✓ Split Query khi cần

✓ CancellationToken

✓ Async

---

# 13.16 Security Rules

Không ghi Password.

Không ghi Token.

Không ghi Secret.

Không ghi Connection String.

Không Enable Sensitive Data Logging trên Production.

---

# 13.17 Enterprise Checklist

□ Một DbContext duy nhất?

□ Đúng namespace?

□ Đúng folder?

□ Không Business Logic?

□ Scoped Lifetime?

□ Có Transaction?

□ Có Query Filter?

□ Có Interceptor?

□ Có Audit?

□ Có Soft Delete?

□ Có Concurrency?

□ Có Async?

□ Có CancellationToken?

---

# 13.18 AI Coding Rules

AI Coding Agent phải:

- Sinh đúng tên DbContext.
- Không sinh nhiều DbContext.
- Không thêm Business Logic.
- Không thêm Service Injection.
- Tự động ApplyConfigurationsFromAssembly().
- Tự động đăng ký Interceptor.
- Tự động bật Global Query Filter.
- Tuân thủ Clean Architecture.

---

# 13.19 Common Mistakes

Sai:

DbContext gọi Email.

DbContext gọi AI.

DbContext Validate DTO.

DbContext xử lý Upload File.

DbContext xử lý GIS.

DbContext chứa Stored Procedure Script.

Đúng:

DbContext chỉ quản lý Persistence.

---

# 13.20 Chapter Summary

Sau khi hoàn thành Chapter này:

- Kiến trúc DbContext được chuẩn hóa.
- Toàn bộ Solution chỉ có một DbContext.
- EF Core hoạt động theo đúng Clean Architecture.
- Antigravity IDE có đầy đủ quy tắc để sinh DbContext chuẩn Enterprise.
- Tầng Persistence sẵn sàng cho các chương tiếp theo về DbSet, Configuration và Fluent API.
# =============================================================================
# CHAPTER 2 – DBSET STANDARDS
# =============================================================================

# 13.21 Purpose

DbSet là đại diện cho một Entity Collection trong Entity Framework Core.

Mỗi DbSet tương ứng với một Aggregate Root hoặc một Entity được quản lý bởi Persistence Layer.

DbSet là điểm truy cập duy nhất để Entity Framework thực hiện:

- Query
- Insert
- Update
- Delete (Soft Delete)
- Tracking
- Change Detection

Trong AnSinhSo Enterprise, DbSet chỉ đại diện cho dữ liệu.

DbSet không chứa Business Logic.

---

# 13.22 Architecture Position

API

↓

Application

↓

Infrastructure

↓

Persistence

↓

AnSinhSoDbContext

↓

DbSet<TEntity>

↓

SQL Server Table

DbSet chỉ tồn tại trong DbContext.

Không khai báo DbSet ở Repository hoặc Service.

---

# 13.23 Naming Standards

Tên DbSet phải sử dụng:

Danh từ số nhiều (Plural Form).

Ví dụ:

```csharp
public DbSet<User> Users { get; set; }

public DbSet<Citizen> Citizens { get; set; }

public DbSet<Household> Households { get; set; }

public DbSet<BenefitPayment> BenefitPayments { get; set; }
```

Không sử dụng:

```csharp
DbSet<User> User

DbSet<Citizen> Citizen

DbSet<tblCitizen>

DbSet<TBL_USER>
```

---

# 13.24 Aggregate Root Rules

Chỉ Aggregate Root mới được khai báo DbSet.

Ví dụ:

```text
Household
 ├── HouseholdMember
 └── Citizen

BenefitProgram
 └── BenefitPayment

User
 └── RefreshToken
```

Entity con chỉ được truy cập thông qua Aggregate Root khi phù hợp với Domain.

---

# 13.25 Entity Registration Standards

Mọi Entity phải được đăng ký trong DbContext.

Ví dụ:

```csharp
public DbSet<Role> Roles => Set<Role>();

public DbSet<User> Users => Set<User>();

public DbSet<Household> Households => Set<Household>();

public DbSet<Citizen> Citizens => Set<Citizen>();

public DbSet<BenefitProgram> BenefitPrograms => Set<BenefitProgram>();

public DbSet<BenefitPayment> BenefitPayments => Set<BenefitPayment>();
```

Không để thiếu Entity.

Không đăng ký Entity trùng lặp.

---

# 13.26 Table Mapping

Tên DbSet không quyết định tên bảng.

Tên bảng được cấu hình trong Entity Configuration.

Ví dụ:

```csharp
builder.ToTable("Citizens");
```

Không sử dụng tên bảng mặc định nếu đã có Naming Convention.

---

# 13.27 Query Rules

Query đọc:

```csharp
.AsNoTracking()
```

Query cập nhật:

Tracking mặc định.

Không bật Tracking cho Dashboard hoặc Reporting.

---

# 13.28 Lazy Loading Policy

Mặc định:

Không sử dụng Lazy Loading.

Ưu tiên:

- Explicit Loading
- Eager Loading (`Include`)
- Projection (`Select`)

Lý do:

- Dễ kiểm soát hiệu năng.
- Tránh N+1 Query.
- Dễ tối ưu.

---

# 13.29 Split Query Policy

Khi Include nhiều Collection:

```csharp
.AsSplitQuery()
```

được ưu tiên để giảm Cartesian Explosion.

Ví dụ:

```csharp
context.Households
    .Include(x => x.Members)
    .Include(x => x.BenefitPayments)
    .AsSplitQuery();
```

---

# 13.30 Performance Standards

DbSet phải hỗ trợ:

- Async Query
- Pagination
- Projection
- Filter
- Ordering
- CancellationToken

Không tải toàn bộ dữ liệu nếu không cần.

---

# 13.31 Security Standards

Không trả Entity trực tiếp ra API.

Quy trình chuẩn:

```text
DbSet
   ↓
Entity
   ↓
Repository
   ↓
Application
   ↓
DTO
   ↓
API Response
```

Entity không được serialize trực tiếp.

---

# 13.32 Best Practices

✔ Một DbSet cho mỗi Entity.

✔ Sử dụng tên số nhiều.

✔ Truy vấn bất đồng bộ (`async/await`).

✔ Dùng `AsNoTracking()` cho đọc.

✔ Chỉ `Include()` khi cần.

✔ Dùng Projection để giảm dữ liệu trả về.

---

# 13.33 Common Mistakes

✘ DbSet đặt tên số ít.

✘ DbSet chứa Business Logic.

✘ Trả Entity trực tiếp qua API.

✘ Lạm dụng Include.

✘ Lazy Loading mặc định.

✘ Không phân trang.

---

# 13.34 AI Coding Rules

AI Coding Agent phải:

- Sinh DbSet cho mọi Entity.
- Đặt tên DbSet ở dạng số nhiều.
- Không sinh DbSet trùng lặp.
- Không sinh DbSet cho DTO.
- Không sinh DbSet cho ViewModel.
- Không sinh DbSet cho Request/Response Model.

---

# 13.35 AI Self Validation Checklist

AI phải tự kiểm tra:

□ Mọi Entity đã có DbSet?

□ Tên DbSet đúng dạng số nhiều?

□ Có Aggregate Root rõ ràng?

□ Có Entity nào bị đăng ký hai lần?

□ Có DbSet cho DTO không?

□ Có dùng Lazy Loading mặc định không?

□ Query đọc có AsNoTracking() không?

□ Có hỗ trợ Async không?

---

# 13.36 Enterprise Example

```text
AnSinhSoDbContext
│
├── Roles
├── Users
├── RefreshTokens
├── AdministrativeUnits
├── Villages
├── Households
├── HouseholdMembers
├── Citizens
├── BenefitPrograms
├── BenefitPayments
├── Applications
├── Notifications
├── AuditLogs
└── SystemSettings
```

---

# 13.37 Production Checklist

□ Không thiếu DbSet.

□ Không trùng DbSet.

□ Không expose Entity trực tiếp.

□ Đã kiểm tra Aggregate Root.

□ Đã kiểm tra Navigation Property.

□ Đã tối ưu Query.

□ Đã dùng Async.

□ Đã có CancellationToken.

---

# 13.38 Chapter Summary

Chapter 2 chuẩn hóa toàn bộ quy tắc sử dụng DbSet trong AnSinhSo Enterprise.

Sau khi hoàn thành chương này:

- Mọi Entity đều được đăng ký nhất quán.
- DbContext trở thành điểm truy cập dữ liệu duy nhất.
- Query được tối ưu theo chuẩn EF Core.
- Antigravity IDE có đầy đủ quy tắc để sinh DbSet chính xác.
- Kiến trúc Persistence sẵn sàng cho Chapter 3 – Entity Configuration Standards.
# =============================================================================
# CHAPTER 3 – ENTITY CONFIGURATION STANDARDS
# =============================================================================

# 13.39 Purpose

Entity Configuration định nghĩa toàn bộ quy tắc ánh xạ (Mapping) giữa Domain Entity và Database.

Đây là nơi xác định:

- Table Mapping
- Column Mapping
- Primary Key
- Foreign Key
- Relationship
- Constraint
- Index
- Default Value
- Data Type
- Query Filter

Entity Configuration là lớp duy nhất chịu trách nhiệm cấu hình Entity Framework Core Mapping.

Mọi Mapping phải được tách khỏi Entity và DbContext.

---

# 13.40 Architecture Position

Clean Architecture

API

↓

Application

↓

Infrastructure

↓

Persistence

↓

Configurations

↓

Entity Framework Core

↓

SQL Server

Configuration chỉ thuộc Infrastructure.

Không được đặt trong:

Domain

Application

API

---

# 13.41 Folder Structure

Infrastructure/

Persistence/

Configurations/

```
RoleConfiguration.cs

UserConfiguration.cs

HouseholdConfiguration.cs

CitizenConfiguration.cs

BenefitProgramConfiguration.cs

BenefitPaymentConfiguration.cs

AuditLogConfiguration.cs
```

Mỗi Entity có đúng một Configuration.

---

# 13.42 Naming Standards

Tên:

```
<EntityName>Configuration
```

Ví dụ

```
CitizenConfiguration

HouseholdConfiguration

UserConfiguration
```

Không sử dụng

```
ConfigCitizen

CitizenMap

TblCitizen

CitizenEF
```

---

# 13.43 Interface Standards

Mọi Configuration đều phải implement

```csharp
IEntityTypeConfiguration<TEntity>
```

Ví dụ

```csharp
public sealed class CitizenConfiguration
    : IEntityTypeConfiguration<Citizen>
{
}
```

Không cấu hình trực tiếp trong DbContext.

---

# 13.44 Mapping Responsibilities

Configuration chịu trách nhiệm:

✓ Table

✓ Column

✓ Length

✓ Required

✓ Nullable

✓ Primary Key

✓ Foreign Key

✓ Relationship

✓ Index

✓ Check Constraint

✓ Default Value

✓ Query Filter

✓ RowVersion

Không chứa:

Business Logic

Validation

Authorization

Service

Repository

---

# 13.45 Table Mapping

Ví dụ

```csharp
builder.ToTable("Citizens");
```

Không sử dụng tên mặc định khi hệ thống đã có Naming Convention.

---

# 13.46 Primary Key Mapping

```csharp
builder.HasKey(x => x.Id);
```

Tên Constraint:

```
PK_Citizens
```

Không tạo Composite Key nếu không có Business Requirement.

---

# 13.47 Property Mapping

Ví dụ

```csharp
builder.Property(x => x.FullName)

.HasMaxLength(200)

.IsRequired();
```

Đối với CCCD

```csharp
builder.Property(x => x.IdentityNumber)

.HasMaxLength(20)

.IsRequired();
```

Không Hard-code kiểu dữ liệu trong nhiều nơi.

---

# 13.48 Relationship Mapping

Ví dụ

```csharp
builder

.HasOne(x => x.Household)

.WithMany(x => x.Members)

.HasForeignKey(x => x.HouseholdId)

.OnDelete(DeleteBehavior.Restrict);
```

Không sử dụng Cascade Delete mặc định.

---

# 13.49 Index Mapping

Ví dụ

```csharp
builder

.HasIndex(x => x.IdentityNumber)

.IsUnique();
```

Composite Index

```csharp
builder

.HasIndex(x => new
{
    x.VillageId,
    x.Status
});
```

---

# 13.50 Default Values

Ví dụ

```csharp
builder

.Property(x => x.IsDeleted)

.HasDefaultValue(false);
```

```csharp
builder

.Property(x => x.CreatedDate)

.HasDefaultValueSql("SYSUTCDATETIME()");
```

---

# 13.51 Query Filter

Ví dụ

```csharp
builder

.HasQueryFilter(x => !x.IsDeleted);
```

Không lặp lại Query Filter trong Repository.

---

# 13.52 RowVersion

```csharp
builder

.Property(x => x.RowVersion)

.IsRowVersion();
```

Bắt buộc với Business Entity.

---

# 13.53 ApplyConfigurations

DbContext chỉ có:

```csharp
modelBuilder

.ApplyConfigurationsFromAssembly(

typeof(AnSinhSoDbContext).Assembly);
```

Không gọi từng Configuration bằng tay.

---

# 13.54 SQL Server Alignment

Configuration phải khớp:

- Data Type
- Constraint
- Index
- Foreign Key
- Delete Behavior

với SQL Server.

Không để EF Core và SQL Server khác nhau.

---

# 13.55 Best Practices

✔ Một Entity → Một Configuration.

✔ Không Mapping trong Entity.

✔ Không Mapping trong DbContext.

✔ Fluent API là chuẩn.

✔ Có cấu trúc thư mục rõ ràng.

✔ Mọi thay đổi đều qua Migration.

---

# 13.56 Common Mistakes

✘ Mapping trong DbContext.

✘ Data Annotation và Fluent API xung đột.

✘ Thiếu Index.

✘ Thiếu Query Filter.

✘ Thiếu RowVersion.

✘ Thiếu Default Value.

---

# 13.57 AI Coding Rules

AI Coding Agent phải:

- Sinh đúng lớp Configuration.
- Không Mapping trong Entity.
- Không Mapping trong DbContext.
- Sinh Fluent API đầy đủ.
- Tuân thủ Naming Convention.
- Tự động tạo Index.
- Tự động tạo Query Filter.
- Tự động cấu hình RowVersion.

---

# 13.58 AI Self Validation Checklist

AI phải tự kiểm tra:

□ Entity đã có Configuration?

□ Có ApplyConfigurationsFromAssembly()?

□ Có Mapping Table?

□ Có Mapping Property?

□ Có Foreign Key?

□ Có Relationship?

□ Có Index?

□ Có Default Value?

□ Có Query Filter?

□ Có RowVersion?

---

# 13.59 Enterprise Template

Mỗi Configuration phải theo cấu trúc:

```text
Configuration

↓

Table

↓

Primary Key

↓

Properties

↓

Relationships

↓

Indexes

↓

Constraints

↓

Query Filters

↓

Concurrency

↓

Seed (nếu có)
```

---

# 13.60 Chapter Summary

Chapter 3 chuẩn hóa toàn bộ Entity Configuration của AnSinhSo Enterprise.

Sau khi hoàn thành chương này:

- Toàn bộ Mapping được tách khỏi Entity và DbContext.
- SQL Server và Entity Framework Core luôn đồng bộ.
- Fluent API trở thành tiêu chuẩn duy nhất cho cấu hình Entity.
- Antigravity IDE có thể tự động sinh các lớp Configuration nhất quán.
- Tầng Persistence sẵn sàng cho Chapter 4 – Fluent API Standards.
# =============================================================================
# CHAPTER 4 – FLUENT API STANDARDS
# =============================================================================

# 13.61 Purpose

Fluent API là tiêu chuẩn cấu hình chính thức của Entity Framework Core trong dự án AnSinhSo Enterprise.

Mọi quy tắc ánh xạ giữa Domain Model và SQL Server phải được thực hiện thông qua Fluent API.

Data Annotation chỉ được sử dụng trong các trường hợp đơn giản và không được phép thay thế Fluent API đối với các cấu hình quan trọng.

Fluent API phải là nguồn cấu hình duy nhất (Single Source of Configuration).

---

# 13.62 Objectives

Fluent API phải đảm bảo:

- Mapping rõ ràng.
- Không mơ hồ.
- Dễ bảo trì.
- Dễ mở rộng.
- Đồng nhất giữa các Entity.
- Tương thích với SQL Server.
- Tương thích với Migration.

---

# 13.63 Architecture Position

Clean Architecture

```text
Domain Entity
        │
        ▼
Entity Configuration
        │
        ▼
Fluent API
        │
        ▼
EF Core Model
        │
        ▼
SQL Server
```

Business Layer không được phụ thuộc vào Fluent API.

---

# 13.64 General Rules

Mỗi Entity chỉ có một lớp Configuration.

Toàn bộ Fluent API nằm trong lớp Configuration.

Không cấu hình Fluent API trong:

- Entity
- Repository
- Service
- Controller
- DbContext (ngoại trừ ApplyConfigurationsFromAssembly)

---

# 13.65 Primary Key Configuration

Ví dụ:

```csharp
builder.HasKey(x => x.Id);

builder.Property(x => x.Id)
       .ValueGeneratedOnAdd();
```

Quy tắc:

- Chỉ một Primary Key.
- Không thay đổi khóa chính sau khi tạo.
- Không dùng Composite Key nếu không có yêu cầu nghiệp vụ rõ ràng.

---

# 13.66 Property Configuration

Ví dụ:

```csharp
builder.Property(x => x.FullName)
       .HasMaxLength(200)
       .IsRequired();

builder.Property(x => x.PhoneNumber)
       .HasMaxLength(20);

builder.Property(x => x.Email)
       .HasMaxLength(255);
```

Quy tắc:

- Luôn cấu hình độ dài.
- Luôn xác định Required/Optional.
- Không để EF Core tự suy diễn khi đã có tiêu chuẩn.

---

# 13.67 Table Configuration

```csharp
builder.ToTable("Citizens");
```

Quy tắc:

- Tên bảng theo Naming Convention.
- Không phụ thuộc vào tên lớp Entity.

---

# 13.68 One-to-One Mapping

```csharp
builder.HasOne(x => x.Profile)
       .WithOne(x => x.Citizen)
       .HasForeignKey<CitizenProfile>(x => x.CitizenId)
       .OnDelete(DeleteBehavior.Restrict);
```

Áp dụng khi quan hệ thực sự là 1–1.

---

# 13.69 One-to-Many Mapping

```csharp
builder.HasOne(x => x.Household)
       .WithMany(x => x.Members)
       .HasForeignKey(x => x.HouseholdId)
       .OnDelete(DeleteBehavior.Restrict);
```

Đây là kiểu quan hệ được sử dụng nhiều nhất trong dự án.

---

# 13.70 Many-to-Many Mapping

Sử dụng Entity trung gian khi quan hệ có dữ liệu nghiệp vụ.

Ví dụ:

```text
User
   │
UserRole
   │
Role
```

Không sử dụng Many-to-Many trực tiếp nếu bảng trung gian có thêm thuộc tính.

---

# 13.71 Delete Behavior

Mặc định:

```csharp
.OnDelete(DeleteBehavior.Restrict)
```

Chỉ sử dụng:

```csharp
DeleteBehavior.Cascade
```

khi dữ liệu con không có giá trị tồn tại độc lập.

---

# 13.72 Index Configuration

```csharp
builder.HasIndex(x => x.IdentityNumber)
       .IsUnique();

builder.HasIndex(x => new
{
    x.VillageId,
    x.Status
});
```

Index phải đồng bộ với tài liệu Phase 11.

---

# 13.73 Default Value Configuration

```csharp
builder.Property(x => x.IsDeleted)
       .HasDefaultValue(false);

builder.Property(x => x.CreatedDate)
       .HasDefaultValueSql("SYSUTCDATETIME()");
```

Không gán giá trị mặc định trong nhiều tầng.

---

# 13.74 Query Filter Configuration

```csharp
builder.HasQueryFilter(x => !x.IsDeleted);
```

Áp dụng thống nhất cho mọi Business Entity.

---

# 13.75 Concurrency Configuration

```csharp
builder.Property(x => x.RowVersion)
       .IsRowVersion();
```

Bắt buộc đối với các bảng nghiệp vụ.

---

# 13.76 Value Conversion

Ví dụ:

```csharp
builder.Property(x => x.Status)
       .HasConversion<int>();
```

Hoặc:

```csharp
builder.Property(x => x.IsActive)
       .HasConversion<bool>();
```

Mọi Value Converter phải được tài liệu hóa.

---

# 13.77 SQL Server Alignment

Fluent API phải phản ánh chính xác:

- Data Type
- Index
- Constraint
- Delete Behavior
- Default Value
- Check Constraint

Không để EF Core và SQL Server khác nhau.

---

# 13.78 Best Practices

✔ Fluent API là chuẩn chính.

✔ Một Configuration cho mỗi Entity.

✔ Chỉ dùng Data Annotation khi thật cần thiết.

✔ Mọi Mapping đều được Review.

✔ Mọi thay đổi đều thông qua Migration.

---

# 13.79 Common Mistakes

✘ Cấu hình vừa bằng Data Annotation vừa bằng Fluent API.

✘ Thiếu DeleteBehavior.

✘ Không cấu hình độ dài chuỗi.

✘ Không cấu hình Index.

✘ Không cấu hình Query Filter.

✘ Không cấu hình RowVersion.

---

# 13.80 Performance Impact

Fluent API đúng chuẩn sẽ:

- Giảm lỗi Migration.
- Tối ưu SQL được sinh.
- Giảm thời gian khởi động mô hình.
- Hạn chế lỗi khi nâng cấp EF Core.

---

# 13.81 Production Checklist

□ Mỗi Entity có một Configuration.

□ Không cấu hình trong DbContext.

□ Có Primary Key.

□ Có Foreign Key.

□ Có DeleteBehavior.

□ Có Index.

□ Có Query Filter.

□ Có RowVersion.

□ Có Default Value.

□ Đã Review.

---

# 13.82 AI Coding Rules

AI Coding Agent phải:

- Sinh đầy đủ Fluent API.
- Không dùng Data Annotation thay cho Fluent API.
- Tuân thủ Naming Convention.
- Sinh đúng DeleteBehavior.
- Sinh Index theo Phase 11.
- Sinh Query Filter theo Phase 10.
- Sinh Concurrency theo Phase 9.

---

# 13.83 AI Self Validation Checklist

AI phải tự kiểm tra:

□ Entity đã có Configuration?

□ Có Primary Key?

□ Có Foreign Key?

□ Có DeleteBehavior?

□ Có Query Filter?

□ Có RowVersion?

□ Có Index?

□ Có Default Value?

□ Có Mapping Table?

□ Có Mapping Property?

□ Có xung đột giữa Fluent API và Data Annotation không?

---

# 13.84 Enterprise Template

Mọi lớp Configuration phải tuân theo trình tự:

```text
Table
    ↓
Primary Key
    ↓
Properties
    ↓
Relationships
    ↓
Indexes
    ↓
Constraints
    ↓
Default Values
    ↓
Query Filters
    ↓
Concurrency
```

---

# 13.85 Chapter Summary

Chapter 4 thiết lập tiêu chuẩn Fluent API cho toàn bộ dự án AnSinhSo Enterprise.

Sau khi hoàn thành chương này:

- Mọi Entity được cấu hình thống nhất.
- SQL Server và EF Core luôn đồng bộ.
- Kiến trúc Mapping rõ ràng, dễ bảo trì.
- Antigravity IDE có thể sinh Fluent API theo đúng chuẩn Enterprise.
# =============================================================================
# CHAPTER 5 – RELATIONSHIP MAPPING STANDARDS
# =============================================================================

# 13.86 Purpose

Relationship Mapping định nghĩa cách các Entity liên kết với nhau trong toàn bộ hệ thống AnSinhSo Enterprise.

Đây là nền tảng của:

- Domain Model
- Entity Framework Core
- SQL Server
- Business Rules
- Aggregate Design

Relationship phải phản ánh đúng nghiệp vụ, không chỉ phản ánh cấu trúc Database.

---

# 13.87 Objectives

Relationship phải bảo đảm:

- Chính xác theo nghiệp vụ.
- Không dư thừa.
- Không tạo vòng tham chiếu.
- Dễ mở rộng.
- Dễ kiểm thử.
- Tối ưu hiệu năng.
- Hỗ trợ Domain-Driven Design.

---

# 13.88 Relationship Principles

## Principle 1 – Business First

Quan hệ phải xuất phát từ nghiệp vụ.

Không tạo Relationship chỉ vì Database cho phép.

---

## Principle 2 – Aggregate Boundary

Relationship không được phá vỡ ranh giới Aggregate.

Aggregate Root là điểm truy cập duy nhất tới Aggregate.

Ví dụ:

Household
│
├── HouseholdMember
└── Citizen

Mọi thao tác với HouseholdMember phải đi qua Household.

---

## Principle 3 – Explicit Relationship

Không sử dụng Shadow Foreign Key.

Foreign Key phải được khai báo rõ ràng.

Ví dụ:

```csharp
public int HouseholdId { get; set; }

public Household Household { get; set; } = default!;
```

---

## Principle 4 – Restrict by Default

DeleteBehavior mặc định:

```csharp
DeleteBehavior.Restrict
```

Cascade chỉ dùng khi có Business Requirement.

---

# 13.89 One-to-One Standards

Chỉ sử dụng khi:

- Hai Entity luôn tồn tại cùng nhau.
- Một Entity không thể có nhiều bản ghi liên quan.

Ví dụ:

Citizen
↔ CitizenProfile

Fluent API:

```csharp
builder.HasOne(x => x.Profile)
       .WithOne(x => x.Citizen)
       .HasForeignKey<CitizenProfile>(x => x.CitizenId)
       .OnDelete(DeleteBehavior.Restrict);
```

---

# 13.90 One-to-Many Standards

Đây là quan hệ mặc định của hệ thống.

Ví dụ:

Village
│
└── Households

Household
│
└── Citizens

BenefitProgram
│
└── BenefitPayments

Fluent API:

```csharp
builder.HasOne(x => x.Household)
       .WithMany(x => x.Citizens)
       .HasForeignKey(x => x.HouseholdId)
       .OnDelete(DeleteBehavior.Restrict);
```

---

# 13.91 Many-to-Many Standards

Không sử dụng Many-to-Many trực tiếp nếu bảng trung gian có dữ liệu nghiệp vụ.

Ví dụ:

User
│
└── UserRole
│
Role

UserRole là Entity độc lập.

Có thể mở rộng:

AssignedDate

AssignedBy

ExpiredDate

Status

---

# 13.92 Navigation Property Standards

Navigation phải phản ánh Domain.

Ví dụ:

```csharp
public ICollection<Citizen> Citizens { get; private set; }
    = new List<Citizen>();
```

Không sử dụng setter public nếu Aggregate quản lý tập hợp.

---

# 13.93 Foreign Key Standards

Foreign Key phải được khai báo rõ ràng.

Ví dụ:

```csharp
public int VillageId { get; set; }
```

Không phụ thuộc vào Shadow Property.

---

# 13.94 Optional Relationship

Ví dụ:

Citizen

↓

DeathCertificate

Có thể chưa tồn tại.

```csharp
public int? DeathCertificateId { get; set; }
```

Relationship phải thể hiện đúng tính tùy chọn của nghiệp vụ.

---

# 13.95 Required Relationship

Ví dụ:

Citizen

↓

Household

Bắt buộc.

```csharp
public int HouseholdId { get; set; }
```

Không để nullable nếu nghiệp vụ yêu cầu bắt buộc.

---

# 13.96 Circular Reference Prevention

Không thiết kế Navigation gây vòng tham chiếu.

Sai:

A → B

B → C

C → A

Điều này gây khó khăn cho:

- Serialization
- Mapping
- API Response

---

# 13.97 SQL Server Alignment

Relationship trong EF Core phải khớp với:

- Foreign Key
- Delete Behavior
- Constraint
- Index

Không để SQL Server và EF Core khác nhau.

---

# 13.98 Performance Best Practices

✔ Chỉ Include khi cần.

✔ Dùng Projection.

✔ Dùng Split Query với nhiều Collection.

✔ Không tải Navigation quá sâu.

✔ Tránh N+1 Query.

---

# 13.99 Common Mistakes

✘ Cascade Delete mặc định.

✘ Shadow Foreign Key.

✘ Navigation dư thừa.

✘ Aggregate không rõ ràng.

✘ Circular Reference.

✘ Many-to-Many không qua Entity trung gian khi có nghiệp vụ.

---

# 13.100 AI Coding Rules

AI Coding Agent phải:

- Sinh Relationship đúng Domain.
- Không sử dụng Shadow FK.
- Mặc định DeleteBehavior.Restrict.
- Sinh Navigation Property hợp lý.
- Không tạo Circular Reference.
- Tôn trọng Aggregate Root.

---

# 13.101 AI Self Validation Checklist

AI phải tự kiểm tra:

□ Quan hệ phản ánh đúng nghiệp vụ?

□ Aggregate Root rõ ràng?

□ Foreign Key đã khai báo?

□ DeleteBehavior đúng?

□ Có Circular Reference?

□ Navigation hợp lý?

□ SQL Server và EF Core đồng bộ?

---

# 13.102 Enterprise Checklist

□ Mỗi Relationship có Business Justification.

□ Foreign Key có Index.

□ DeleteBehavior được Review.

□ Aggregate Root không bị phá vỡ.

□ Navigation không dư thừa.

□ Serialization an toàn.

---

# 13.103 Chapter Summary

Chapter 5 chuẩn hóa toàn bộ Relationship Mapping trong AnSinhSo Enterprise.

Sau khi hoàn thành:

- Domain Model và Database thống nhất.
- EF Core Mapping phản ánh đúng nghiệp vụ.
- SQL Server được tối ưu về Integrity và Performance.
- Antigravity IDE có đầy đủ quy tắc để sinh Relationship theo chuẩn Enterprise.
# =============================================================================
# CHAPTER 6 – OWNED ENTITY STANDARDS
# =============================================================================

# 13.104 Purpose

Owned Entity là một kiểu Value Object trong Entity Framework Core.

Owned Entity không có vòng đời độc lập.

Nó luôn thuộc về một Owner Entity.

Trong AnSinhSo Enterprise, Owned Entity được sử dụng để:

- Giảm số lượng bảng.
- Mô hình hóa Value Object.
- Tăng tính nhất quán.
- Giảm JOIN.
- Đơn giản hóa Domain Model.

Owned Entity phải phản ánh đúng Domain-Driven Design (DDD).

---

# 13.105 Objectives

Owned Entity phải:

- Không có Identity riêng.
- Không tồn tại độc lập.
- Không có DbSet.
- Không có Repository.
- Không có Service.
- Luôn thuộc về một Aggregate Root.

---

# 13.106 Architecture Position

```text
Aggregate Root
        │
        ▼
Owned Entity
        │
        ▼
Same Database Table
```

Owned Entity không ánh xạ sang bảng riêng (trừ khi có yêu cầu đặc biệt).

---

# 13.107 When to Use Owned Entity

Sử dụng cho:

✔ Address

✔ ContactInfo

✔ GeoLocation

✔ Money

✔ Coordinates

✔ FullName

✔ IdentityDocument

✔ AuditInfo

✔ Period

✔ DateRange

---

Không sử dụng cho:

Citizen

Household

BenefitProgram

Payment

Application

Role

User

---

# 13.108 Naming Standards

Tên phải là danh từ.

Ví dụ:

```text
Address

ContactInfo

GeoLocation

Money

Coordinate

IdentityCard

DateRange
```

Không sử dụng:

```
AddressEntity

MoneyEntity

GeoEntity

TblAddress
```

---

# 13.109 Folder Structure

```text
Domain/

ValueObjects/

Address.cs

ContactInfo.cs

GeoLocation.cs

Money.cs

Coordinate.cs
```

Không đặt trong Infrastructure.

---

# 13.110 Entity Design

Ví dụ:

```csharp
public sealed class Address
{
    public string Province { get; private set; } = default!;

    public string District { get; private set; } = default!;

    public string Commune { get; private set; } = default!;

    public string Village { get; private set; } = default!;

    public string Street { get; private set; } = default!;
}
```

Không có:

Id

CreatedDate

UpdatedDate

Repository

DbSet

---

# 13.111 EF Core Mapping

```csharp
builder.OwnsOne(
    x => x.Address,
    owned =>
    {
        owned.Property(p => p.Province)
             .HasMaxLength(100);

        owned.Property(p => p.District)
             .HasMaxLength(100);

        owned.Property(p => p.Commune)
             .HasMaxLength(100);

        owned.Property(p => p.Village)
             .HasMaxLength(100);

        owned.Property(p => p.Street)
             .HasMaxLength(200);
    });
```

Không tạo bảng Address riêng.

---

# 13.112 Database Mapping

Ví dụ:

```text
Citizens

Id

FullName

Province

District

Commune

Village

Street
```

Address được "flatten" vào bảng Citizens.

---

# 13.113 Aggregate Rules

Owned Entity chỉ được truy cập thông qua Owner.

Ví dụ:

```text
Citizen

↓

Address
```

Không truy vấn Address trực tiếp.

---

# 13.114 Equality Rules

Owned Entity phải so sánh theo giá trị (Value Equality).

Ví dụ:

Hai Address có cùng:

Province

District

Commune

Village

Street

↓

Được xem là bằng nhau.

Không so sánh theo Identity.

---

# 13.115 Performance Best Practices

✔ Giảm JOIN.

✔ Không cần Foreign Key.

✔ Không cần Repository.

✔ Không cần DbSet.

✔ Dữ liệu đọc nhanh hơn.

---

# 13.116 Common Mistakes

✘ Tạo DbSet<Address>.

✘ Tạo Repository<Address>.

✘ Thêm Id vào Address.

✘ Mapping Address thành bảng riêng khi không cần.

✘ Đưa Business Logic vào Value Object.

---

# 13.117 Enterprise Reference Implementation

Cấu trúc chuẩn:

```text
Domain/
└── ValueObjects/
    ├── Address.cs
    ├── ContactInfo.cs
    ├── GeoLocation.cs
    ├── Money.cs
    └── DateRange.cs

Infrastructure/
└── Persistence/
    └── Configurations/
        └── CitizenConfiguration.cs
```

Ví dụ trong `CitizenConfiguration`:

```csharp
builder.OwnsOne(x => x.Address);
builder.OwnsOne(x => x.ContactInfo);
```

---

# 13.118 Production Example

Citizen

```text
Citizen
│
├── FullName
├── DateOfBirth
├── Address
│   ├── Province
│   ├── District
│   ├── Commune
│   ├── Village
│   └── Street
│
└── ContactInfo
    ├── PhoneNumber
    └── Email
```

Không tạo bảng `Addresses`.

---

# 13.119 AI Coding Rules

AI Coding Agent phải:

- Nhận diện Value Object.
- Không sinh DbSet cho Owned Entity.
- Không sinh Repository.
- Không sinh Controller.
- Mapping bằng `OwnsOne()`.
- Không thêm khóa chính (`Id`).

---

# 13.120 AI Prompt Template

Khi sinh Entity:

```text
Nếu Entity không có vòng đời độc lập,
không có Identity,
không cần Repository,
không cần DbSet,

=> Hãy thiết kế dưới dạng Owned Entity
và cấu hình bằng OwnsOne() hoặc OwnsMany().
```

---

# 13.121 AI Self Validation Checklist

AI phải tự kiểm tra:

□ Có thêm Id không?

□ Có DbSet không?

□ Có Repository không?

□ Có Controller không?

□ Có OwnsOne() không?

□ Có Value Equality không?

□ Có Aggregate Root quản lý không?

---

# 13.122 Production Checklist

□ Owned Entity không có Identity.

□ Không có DbSet.

□ Không có Repository.

□ Không có Migration tạo bảng riêng.

□ Mapping đúng bằng OwnsOne().

□ Domain Model phản ánh đúng Value Object.

---

# 13.123 Chapter Summary

Chapter 6 chuẩn hóa việc sử dụng Owned Entity trong AnSinhSo Enterprise.

Sau khi hoàn thành:

- Domain Model phản ánh đúng Value Object.
- SQL Server giảm số lượng bảng và JOIN không cần thiết.
- EF Core sử dụng OwnsOne()/OwnsMany() theo chuẩn Microsoft.
- Antigravity IDE có thể tự động nhận diện và sinh Value Object đúng chuẩn DDD.
# =============================================================================
# CHAPTER 7 – VALUE CONVERTER STANDARDS
# =============================================================================

# 13.124 Purpose

Value Converter là cơ chế chuyển đổi giữa Domain Model và Database Model.

Trong AnSinhSo Enterprise, Domain phải phản ánh đúng Business.

Database phải phản ánh đúng khả năng lưu trữ của SQL Server.

Hai mô hình này không phải lúc nào cũng giống nhau.

Value Converter là cầu nối giữa hai mô hình.

---

# 13.125 Objectives

Value Converter phải:

✓ Không làm thay đổi Business Model.

✓ Không ảnh hưởng Domain.

✓ Hỗ trợ Migration.

✓ Hỗ trợ LINQ Query.

✓ Có hiệu năng cao.

✓ Có khả năng mở rộng.

---

# 13.126 Architecture Position

Domain

↓

Entity

↓

Value Converter

↓

EF Core

↓

SQL Server

Value Converter chỉ tồn tại tại Infrastructure Layer.

---

# 13.127 Standard Converter Types

AnSinhSo Enterprise chuẩn hóa các Converter sau:

✔ Enum

✔ DateOnly

✔ TimeOnly

✔ Boolean

✔ Money

✔ Decimal

✔ Json

✔ GeoLocation

✔ Coordinate

✔ ValueObject

---

# 13.128 Enum Converter

Business

```csharp
public CitizenStatus Status
```

Database

```sql
INT
```

Configuration

```csharp
builder

.Property(x=>x.Status)

.HasConversion<int>();
```

Không lưu Enum dạng chuỗi nếu không có yêu cầu.

---

# 13.129 Boolean Converter

Nếu Database yêu cầu

Y/N

```csharp
.HasConversion(

v=>v?"Y":"N",

v=>v=="Y"

);
```

Nếu SQL Server dùng BIT

↓

Không cần Converter.

---

# 13.130 DateOnly Converter

Domain

```csharp
DateOnly
```

Database

```sql
date
```

Ví dụ

```csharp
.HasConversion(

v=>v.ToDateTime(TimeOnly.MinValue),

v=>DateOnly.FromDateTime(v)

);
```

---

# 13.131 TimeOnly Converter

Domain

```csharp
TimeOnly
```

Database

```sql
time
```

---

# 13.132 Decimal Converter

Tiền trợ cấp

```sql
decimal(18,2)
```

Không dùng

float

double

---

# 13.133 Json Converter

Ví dụ

Preferences

Settings

Metadata

```csharp
.HasConversion(

JsonSerializer.Serialize,

JsonSerializer.Deserialize

);
```

---

# 13.134 Geo Converter

GIS Coordinate

↓

geometry

↓

GeoLocation

↓

Converter

Không để Domain phụ thuộc SQL Server Geometry.

---

# 13.135 Value Object Converter

Ví dụ

Money

↓

decimal

Coordinate

↓

string

GeoLocation

↓

json

---

# 13.136 SQL Server Alignment

Converter phải tương thích:

SQL Server

EF Core

Migration

Không sinh kiểu dữ liệu khác nhau.

---

# 13.137 Performance Rules

Converter phải:

✓ Stateless

✓ Reusable

✓ Không Reflection

✓ Không IO

✓ Không HTTP

✓ Không Database Access

---

# 13.138 Best Practices

✔ Một Converter cho một Value.

✔ Không Business Logic.

✔ Không Inject Service.

✔ Có Unit Test.

✔ Có Integration Test.

---

# 13.139 Common Mistakes

✘ Converter gọi Database.

✘ Converter gọi API.

✘ Converter đọc File.

✘ Converter có Logging.

✘ Converter phụ thuộc DI.

---

# 13.140 Enterprise Reference Implementation

Infrastructure

Persistence

Converters

```text
CitizenStatusConverter.cs

MoneyConverter.cs

DateOnlyConverter.cs

TimeOnlyConverter.cs

GeoLocationConverter.cs

JsonConverter.cs
```

---

# 13.141 Production Example

Citizen

↓

CitizenStatus

↓

Converter

↓

INT

↓

SQL Server

Ngược lại

INT

↓

Converter

↓

CitizenStatus Enum

---

# 13.142 AI Coding Rules

AI Coding Agent phải:

✓ Sinh Converter riêng.

✓ Không để Business biết Converter.

✓ Không dùng Reflection.

✓ Không Inject Service.

✓ Có thể tái sử dụng.

---

# 13.143 AI Self Validation Checklist

□ Có Converter riêng?

□ Có Unit Test?

□ Có Mapping?

□ Có Migration?

□ Có SQL Alignment?

□ Có Stateless?

---

# 13.144 Production Checklist

□ Enum Mapping đúng.

□ DateOnly đúng.

□ TimeOnly đúng.

□ Decimal đúng.

□ Json đúng.

□ Không Business Logic.

□ Có Test.

---

# 13.145 Chapter Summary

Chapter 7 chuẩn hóa toàn bộ Value Converter của AnSinhSo Enterprise.

Sau khi hoàn thành:

- Domain không phụ thuộc Database.
- SQL Server và Domain được tách biệt.
- EF Core Mapping rõ ràng.
- AI Coding Agent có quy tắc chuẩn để sinh Converter.
# =============================================================================
# CHAPTER 8 – GLOBAL QUERY FILTER STANDARDS
# =============================================================================

# 13.146 Purpose

Global Query Filter là cơ chế áp dụng điều kiện truy vấn mặc định cho toàn bộ Entity Framework Core.

Trong AnSinhSo Enterprise, Query Filter không chỉ phục vụ Soft Delete mà còn là nền tảng của:

- Data Security
- Data Isolation
- Administrative Scope
- Organization Boundary
- Multi-Tenant Architecture
- Compliance

Mục tiêu là đảm bảo mọi truy vấn đều tuân thủ phạm vi dữ liệu mà người dùng được phép truy cập.

---

# 13.147 Objectives

Global Query Filter phải:

✓ Tự động áp dụng.

✓ Không lặp lại điều kiện WHERE.

✓ Đảm bảo Data Security.

✓ Đảm bảo Data Integrity.

✓ Hỗ trợ Multi Tenant.

✓ Hỗ trợ Role Based Access.

✓ Dễ bảo trì.

---

# 13.148 Architecture Position

```text
Controller
      │
Application
      │
Repository
      │
DbContext
      │
Global Query Filter
      │
SQL Server
```

Repository không cần tự viết điều kiện lặp lại nếu đã có Query Filter.

---

# 13.149 Standard Filter Types

AnSinhSo Enterprise chuẩn hóa các nhóm Query Filter:

✔ Soft Delete

✔ IsActive

✔ Organization

✔ Province

✔ Commune

✔ Village

✔ Effective Date

✔ Expired Date

✔ Tenant

✔ Data Permission

---

# 13.150 Soft Delete Filter

Ví dụ:

```csharp
builder.HasQueryFilter(x => !x.IsDeleted);
```

Đây là Filter mặc định cho mọi Business Entity.

---

# 13.151 Active Filter

Ví dụ:

```csharp
builder.HasQueryFilter(x => x.IsActive);
```

Không trả về dữ liệu đã ngừng sử dụng nếu nghiệp vụ không yêu cầu.

---

# 13.152 Administrative Scope Filter

Ví dụ:

```text
Province
    ↓
District
    ↓
Commune
    ↓
Village
```

Người dùng cấp xã chỉ nhìn thấy dữ liệu thuộc xã của mình.

---

# 13.153 Organization Filter

Ví dụ:

```text
DepartmentId
```

Người dùng chỉ được truy cập dữ liệu thuộc đơn vị được phân công.

---

# 13.154 Effective Date Filter

Ví dụ:

```text
EffectiveDate <= Today

AND

ExpiredDate >= Today
```

Chỉ lấy dữ liệu còn hiệu lực.

---

# 13.155 Multi-Tenant Filter

Nếu hệ thống triển khai đa đơn vị:

```csharp
builder.HasQueryFilter(x => x.TenantId == _currentTenant.Id);
```

Tenant phải được lấy từ ngữ cảnh (context), không được hard-code.

---

# 13.156 IgnoreQueryFilters Policy

Chỉ các tác vụ đặc biệt mới được phép bỏ qua Query Filter.

Ví dụ:

```csharp
context.Citizens
       .IgnoreQueryFilters();
```

Áp dụng cho:

- Recycle Bin.
- Khôi phục dữ liệu.
- Báo cáo quản trị.
- Kiểm toán.

Không sử dụng trong các API thông thường.

---

# 13.157 SQL Server Alignment

Mọi Query Filter phải tương ứng với:

- Chỉ mục (Index).
- Constraint.
- Thiết kế bảo mật.

Ví dụ:

Filter theo `VillageId` thì `VillageId` phải có Index.

---

# 13.158 Performance Rules

✓ Query Filter phải đơn giản.

✓ Tránh gọi phương thức phức tạp.

✓ Không truy cập dịch vụ bên ngoài.

✓ Không thực hiện I/O.

✓ Không gọi HTTP.

✓ Không truy cập Database khác.

---

# 13.159 Best Practices

✔ Kết hợp Query Filter với Soft Delete.

✔ Kết hợp với Role-Based Security.

✔ Kết hợp với Administrative Scope.

✔ Luôn kiểm thử hiệu năng.

---

# 13.160 Common Mistakes

✘ Lặp lại điều kiện WHERE ở Repository.

✘ Bỏ qua Query Filter không kiểm soát.

✘ Query Filter quá phức tạp.

✘ Thiếu Index cho cột lọc.

✘ Hard-code TenantId.

---

# 13.161 Enterprise Reference Implementation

Infrastructure/

Persistence/

Filters/

```text
SoftDeleteFilter

OrganizationFilter

VillageFilter

TenantFilter

ActiveFilter
```

Các Filter được cấu hình tập trung trong `DbContext`.

---

# 13.162 Production Example

Người dùng:

```text
Cán bộ xã Sông Lũy
```

Khi truy vấn:

```text
Citizens
```

Hệ thống tự động bổ sung:

```sql
WHERE

IsDeleted = 0

AND CommuneId = CurrentUser.CommuneId
```

Không cần viết lại trong từng Repository.

---

# 13.163 AI Coding Rules

AI Coding Agent phải:

- Tự động áp dụng Soft Delete Filter.
- Hỗ trợ Multi-Tenant Filter.
- Hỗ trợ Administrative Scope.
- Không hard-code giá trị.
- Không lặp điều kiện WHERE.

---

# 13.164 AI Prompt Template

```text
Nếu Entity có IsDeleted,
hãy tạo Global Query Filter.

Nếu Entity thuộc phạm vi hành chính,
hãy áp dụng Query Filter theo phạm vi người dùng hiện tại.

Không viết điều kiện WHERE lặp lại trong Repository nếu đã có Query Filter.
```

---

# 13.165 AI Self Validation Checklist

□ Có Soft Delete Filter?

□ Có Filter theo phạm vi dữ liệu?

□ Có sử dụng IgnoreQueryFilters đúng nơi?

□ Có Index cho các cột lọc?

□ Có tránh hard-code?

□ Có kiểm thử hiệu năng?

---

# 13.166 Production Checklist

□ Soft Delete hoạt động.

□ Administrative Scope đúng.

□ Multi-Tenant đúng (nếu áp dụng).

□ Role-Based Access đúng.

□ IgnoreQueryFilters được kiểm soát.

□ Execution Plan được kiểm tra.

---

# 13.167 Chapter Summary

Chapter 8 chuẩn hóa toàn bộ Global Query Filter trong AnSinhSo Enterprise.

Sau khi hoàn thành:

- Bảo mật dữ liệu được áp dụng ngay tại tầng Persistence.
- Repository đơn giản hơn do không phải lặp điều kiện lọc.
- EF Core và SQL Server hoạt động nhất quán.
- Antigravity IDE có thể tự động sinh Query Filter phù hợp với phạm vi dữ liệu và chính sách bảo mật của hệ thống.
# =============================================================================
# CHAPTER 9 – SAVECHANGES INTERCEPTOR STANDARDS
# =============================================================================

# 13.168 Purpose

SaveChanges Interceptor là thành phần trung tâm của Persistence Pipeline trong Entity Framework Core.

Interceptor cho phép thực thi các quy tắc chung trước và sau khi dữ liệu được ghi xuống SQL Server mà không làm thay đổi Business Logic.

Trong AnSinhSo Enterprise, mọi xử lý dùng chung liên quan đến Persistence phải ưu tiên triển khai thông qua Interceptor thay vì ghi trực tiếp trong DbContext hoặc Repository.

---

# 13.169 Objectives

SaveChanges Interceptor phải:

✓ Tách biệt Business Logic khỏi Persistence.

✓ Đảm bảo Audit tự động.

✓ Hỗ trợ Soft Delete.

✓ Hỗ trợ Concurrency.

✓ Hỗ trợ Domain Event.

✓ Hỗ trợ Outbox Pattern.

✓ Hỗ trợ Logging.

✓ Có khả năng mở rộng.

---

# 13.170 Architecture Position

```text
Controller
        │
Application Service
        │
Repository
        │
DbContext
        │
SaveChangesInterceptor
        │
SQL Server
```

Interceptor là bước cuối cùng trước khi Entity Framework Core thực hiện INSERT, UPDATE hoặc DELETE.

---

# 13.171 Standard Responsibilities

Interceptor chịu trách nhiệm:

- Thiết lập CreatedDate.
- Thiết lập UpdatedDate.
- Thiết lập CreatedBy.
- Thiết lập UpdatedBy.
- Chuyển Delete thành Soft Delete.
- Kiểm tra Concurrency Token.
- Ghi Audit Log.
- Phát Domain Event.
- Chuẩn bị Outbox Message.

Interceptor không chịu trách nhiệm:

- Validation.
- Authorization.
- Gửi Email.
- Gửi Zalo.
- AI Processing.
- GIS Processing.
- Business Workflow.

---

# 13.172 Audit Standards

Mọi Business Entity phải tự động cập nhật:

```text
CreatedDate

CreatedBy

UpdatedDate

UpdatedBy
```

Các giá trị này không được gán thủ công trong Controller.

---

# 13.173 Soft Delete Standards

Nếu Entity hỗ trợ Soft Delete:

```text
Delete()

↓

Interceptor

↓

IsDeleted = true

DeletedDate = UtcNow

DeletedBy = CurrentUser
```

Không sử dụng DELETE vật lý trừ khi có yêu cầu đặc biệt.

---

# 13.174 Domain Event Integration

Interceptor có thể thu thập Domain Events sau khi SaveChanges thành công.

Quy trình:

```text
Aggregate Root
        │
Raise Domain Event
        │
SaveChanges
        │
Interceptor
        │
Publish Event
```

Domain Event không được Publish trước khi Transaction thành công.

---

# 13.175 Outbox Pattern

Nếu hệ thống tích hợp:

- Zalo OA
- Email
- SMS
- Notification
- Message Queue

Interceptor ghi Outbox Message trong cùng Transaction.

Publisher sẽ xử lý sau.

Điều này đảm bảo tính nhất quán dữ liệu.

---

# 13.176 Logging Standards

Interceptor chỉ ghi:

- Entity Name.
- Action.
- Execution Time.
- UserId.
- CorrelationId.

Không ghi:

- Password.
- Access Token.
- Refresh Token.
- Connection String.
- Thông tin nhạy cảm.

---

# 13.177 Performance Rules

Interceptor phải:

✓ Stateless.

✓ Async.

✓ Không Reflection.

✓ Không HTTP Call.

✓ Không File I/O.

✓ Không Database ngoài Transaction hiện tại.

---

# 13.178 Enterprise Reference Implementation

```text
Infrastructure/

Persistence/

Interceptors/

AuditInterceptor.cs

SoftDeleteInterceptor.cs

DomainEventInterceptor.cs

OutboxInterceptor.cs

ConcurrencyInterceptor.cs
```

Đăng ký:

```csharp
services.AddScoped<AuditInterceptor>();
services.AddScoped<SoftDeleteInterceptor>();
```

---

# 13.179 Production Example

Citizen

↓

Update Address

↓

SaveChanges()

↓

AuditInterceptor

↓

UpdatedDate

UpdatedBy

↓

SoftDeleteInterceptor (nếu có)

↓

Transaction Commit

↓

Outbox

↓

Zalo Notification Worker

---

# 13.180 AI Coding Rules

AI Coding Agent phải:

- Không Override SaveChanges nếu có thể dùng Interceptor.
- Tách từng trách nhiệm thành Interceptor riêng.
- Không đưa Business Logic vào Interceptor.
- Đăng ký Interceptor qua Dependency Injection.
- Tuân thủ thứ tự thực thi.

---

# 13.181 AI Prompt Template

```text
Nếu cần xử lý chung trước hoặc sau SaveChanges,
hãy triển khai bằng SaveChangesInterceptor.

Không đặt logic trong Controller hoặc Repository.

Mỗi Interceptor chỉ đảm nhận một trách nhiệm.
```

---

# 13.182 AI Self Validation Checklist

□ Có Interceptor riêng cho Audit?

□ Có Soft Delete Interceptor?

□ Có Domain Event Interceptor?

□ Có Outbox Interceptor?

□ Có Async?

□ Có Stateless?

□ Có Dependency Injection?

□ Không chứa Business Logic?

---

# 13.183 Production Checklist

□ Audit hoạt động.

□ Soft Delete hoạt động.

□ Outbox hoạt động.

□ Domain Event hoạt động.

□ Logging đúng chuẩn.

□ Không có dữ liệu nhạy cảm trong Log.

□ Transaction nhất quán.

---

# 13.184 Chapter Summary

Chapter 9 chuẩn hóa toàn bộ SaveChanges Interceptor trong AnSinhSo Enterprise.

Sau khi hoàn thành:

- Persistence Pipeline được chuẩn hóa.
- Audit và Soft Delete hoạt động tự động.
- Domain Event và Outbox Pattern sẵn sàng tích hợp.
- Antigravity IDE có thể sinh Interceptor theo đúng kiến trúc Enterprise và nguyên tắc Single Responsibility.
# =============================================================================
# CHAPTER 10 – MIGRATION STANDARDS
# =============================================================================

# 13.185 Purpose

Migration là cơ chế quản lý thay đổi cấu trúc Database theo thời gian.

Trong AnSinhSo Enterprise, Migration là thành phần bắt buộc để đảm bảo:

- Database Versioning
- Deployment Consistency
- Source Control Integration
- CI/CD Automation
- Rollback Capability
- Team Collaboration

Mọi thay đổi Schema đều phải được quản lý bằng Migration.

Không thay đổi trực tiếp Database Production.

---

# 13.186 Objectives

Migration phải:

✓ Có thể lặp lại.

✓ Có thể rollback.

✓ Có thể review.

✓ Có thể triển khai tự động.

✓ Đồng bộ Source Code và Database.

✓ Không gây mất dữ liệu ngoài ý muốn.

---

# 13.187 Architecture Position

```text
Developer
      │
      ▼
Domain Model
      │
      ▼
Entity Configuration
      │
      ▼
Migration
      │
      ▼
SQL Server
```

Migration là cầu nối giữa mã nguồn và cơ sở dữ liệu.

---

# 13.188 Migration Folder Structure

```text
Infrastructure/

Persistence/

Migrations/

20260714120000_InitialCreate.cs

20260715103000_AddCitizenAddress.cs

20260716091500_AddBenefitProgram.cs

MigrationExtensions.cs
```

Không đặt Migration trong:

- Domain
- Application
- API

---

# 13.189 Naming Standards

Tên Migration phải phản ánh đúng mục đích.

Ví dụ:

```text
InitialCreate

AddCitizenTable

AddBenefitProgram

UpdateCitizenAddress

CreateAuditLog

AddVillageIndexes

RemoveUnusedColumns

RenameBenefitStatus
```

Không sử dụng:

```text
Update1

Fix

Test

ABC

Migration123
```

---

# 13.190 Migration Rules

Mỗi Migration chỉ thực hiện **một thay đổi logic**.

Ví dụ:

✔ Thêm bảng.

✔ Thêm cột.

✔ Thêm Index.

✔ Thêm Constraint.

Không gộp nhiều thay đổi không liên quan vào một Migration.

---

# 13.191 Database Versioning

Mỗi Migration tương ứng với một phiên bản Database.

Ví dụ:

```text
v1.0.0

↓

InitialCreate

v1.1.0

↓

AddCitizenModule

v1.2.0

↓

BenefitProgram

v1.3.0

↓

GISIntegration
```

Version Database phải đồng bộ với Release Notes.

---

# 13.192 Seed Data Standards

Seed Data chỉ dùng cho:

- Roles
- Permissions
- Administrative Units
- System Settings
- Lookup Tables

Không Seed:

- Dữ liệu nghiệp vụ thực tế.
- Hồ sơ công dân.
- Chi trả trợ cấp.

---

# 13.193 Rollback Strategy

Rollback phải được kiểm thử trước khi triển khai Production.

Mỗi Migration phải hỗ trợ:

```text
Up()

Down()
```

Không để phương thức `Down()` rỗng.

---

# 13.194 Environment Strategy

Development

↓

Migration tự do.

Staging

↓

Kiểm thử Migration.

Production

↓

Migration được phê duyệt.

Không chạy Migration trực tiếp trên Production nếu chưa kiểm thử.

---

# 13.195 CI/CD Integration

Pipeline phải:

- Build Solution.
- Chạy Unit Test.
- Chạy Integration Test.
- Kiểm tra Migration.
- Tạo SQL Script.
- Phê duyệt trước khi triển khai.

---

# 13.196 SQL Script Generation

Triển khai Production ưu tiên:

```bash
dotnet ef migrations script
```

Thay vì:

```bash
dotnet ef database update
```

Script SQL phải được DBA hoặc Technical Lead xem xét.

---

# 13.197 Performance Rules

Migration phải:

✓ Chạy theo lô (batch).

✓ Tránh khóa bảng lâu.

✓ Tạo Index hợp lý.

✓ Không xóa dữ liệu ngoài ý muốn.

✓ Có kế hoạch cho bảng lớn.

---

# 13.198 Common Mistakes

✘ Sửa trực tiếp Database.

✘ Không commit Migration.

✘ Gộp quá nhiều thay đổi.

✘ Không kiểm thử Rollback.

✘ Không cập nhật Seed Data.

✘ Đổi tên Migration sau khi đã phát hành.

---

# 13.199 Enterprise Reference Implementation

```text
Infrastructure/
└── Persistence/
    ├── Migrations/
    │   ├── 20260714120000_InitialCreate.cs
    │   ├── 20260715103000_AddCitizenModule.cs
    │   ├── 20260716091500_AddBenefitProgram.cs
    │   └── AnSinhSoDbContextModelSnapshot.cs
    └── Seed/
        ├── RoleSeed.cs
        ├── PermissionSeed.cs
        └── AdministrativeUnitSeed.cs
```

---

# 13.200 Production Example

Sprint 01

↓

InitialCreate

↓

Sprint 02

↓

Citizen Module

↓

Sprint 03

↓

Benefit Program

↓

Sprint 04

↓

GIS Module

↓

Sprint 05

↓

Zalo OA Integration

Mỗi Sprint có tập Migration riêng và được kiểm thử độc lập.

---

# 13.201 AI Coding Rules

AI Coding Agent phải:

- Sinh Migration theo từng thay đổi.
- Không gộp nhiều chức năng.
- Sinh `Up()` và `Down()` đầy đủ.
- Đặt tên Migration rõ ràng.
- Không chỉnh sửa Migration đã phát hành.
- Đồng bộ Model Snapshot.

---

# 13.202 AI Prompt Template

```text
Nếu thay đổi Entity hoặc Configuration,
hãy tạo Migration mới.

Không chỉnh sửa Migration cũ đã phát hành.

Mỗi Migration chỉ chứa một thay đổi logic.

Luôn sinh đầy đủ Up() và Down().
```

---

# 13.203 AI Self Validation Checklist

□ Migration có tên đúng?

□ Có Up()?

□ Có Down()?

□ Có Rollback?

□ Có Seed Data phù hợp?

□ Có cập nhật Snapshot?

□ Có kiểm thử trên Development?

□ Có SQL Script để triển khai?

---

# 13.204 Production Checklist

□ Migration đã Review.

□ Đã chạy Unit Test.

□ Đã chạy Integration Test.

□ Đã kiểm thử Rollback.

□ Đã tạo SQL Script.

□ Đã sao lưu Database.

□ Đã có kế hoạch khôi phục.

□ Đã phê duyệt triển khai.

---

# 13.205 Chapter Summary

Chapter 10 chuẩn hóa toàn bộ quy trình Migration của AnSinhSo Enterprise.

Sau khi hoàn thành:

- Database được quản lý theo phiên bản.
- Mọi thay đổi Schema đều có thể theo dõi và kiểm soát.
- Quy trình triển khai Development, Staging và Production được thống nhất.
- Antigravity IDE có đầy đủ quy tắc để sinh Migration theo chuẩn Enterprise, hỗ trợ CI/CD và giảm rủi ro khi triển khai.
