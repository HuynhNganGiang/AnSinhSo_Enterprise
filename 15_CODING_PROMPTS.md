````md id="hx9m3a"
# ============================================================================
# 15_CODING_PROMPTS.md
# ============================================================================
#
# Project         : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
# Document Type   : Enterprise Prompt Library
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

Tài liệu này quy định bộ Prompt chuẩn (Enterprise Prompt Library) sử dụng trong toàn bộ vòng đời phát triển dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa Prompt.
- Chuẩn hóa cách làm việc với AI.
- Đảm bảo mọi AI sinh cùng một kết quả.
- Giảm Prompt trùng lặp.
- Tăng chất lượng mã nguồn.
- Tăng khả năng tái sử dụng.

---

# 2. SCOPE

Áp dụng cho:

- ChatGPT
- Gemini
- Cursor
- GitHub Copilot
- Cline
- Continue
- AntiGravity AI

---

# 3. PROMPT DESIGN PRINCIPLES

Prompt phải đảm bảo:

- Có Context.
- Có Scope.
- Có Standards.
- Có Constraints.
- Có Expected Output.
- Có Related Documents.

Không sử dụng Prompt quá ngắn hoặc thiếu ngữ cảnh.

---

# 4. STANDARD PROMPT STRUCTURE

Mọi Prompt nên tuân theo cấu trúc:

```text
Project Context

↓

Current Sprint

↓

Task Description

↓

Requirements

↓

Constraints

↓

Expected Output

↓

Related Standards

↓

Output Format
```

---

# 5. BOOTSTRAP PROMPT

Đây là Prompt chuẩn để khởi động một cuộc trò chuyện mới.

```text
Đây là bộ tài liệu khởi động của dự án AnSinhSo.

Hãy đọc theo đúng thứ tự:

00_PROJECT_BOOTSTRAP.md

↓

00_PROJECT_PROGRESS.md

↓

00_PROJECT_INDEX.md

↓

00_AI_HANDOVER.md

Sau đó:

- Không thay đổi Architecture.
- Không thay đổi Standards.
- Không thay đổi Business Rules.
- Tiếp tục Sprint hiện tại.
- Chỉ thực hiện đúng nhiệm vụ tôi yêu cầu.
```

---

# 6. PROMPT USAGE RULES

AI phải:

- Đọc Bootstrap trước.
- Đọc Standards liên quan.
- Đọc Documentation liên quan.
- Xác định Sprint.
- Xác định Module.

Sau đó mới sinh nội dung.

---

# 7. PROMPT CATEGORIES

Prompt được chia thành:

- Documentation
- Database
- Backend
- Frontend
- API
- Security
- DevOps
- Testing
- Deployment
- AI
- Review
- Refactoring

---

# 8. PHASE 1 CHECKLIST

Trước khi chuyển sang Phase 2A cần xác nhận:

- Enterprise Prompt Library được khởi tạo.
- Prompt Structure được thống nhất.
- Bootstrap Prompt được chuẩn hóa.
- Prompt Categories được xác định.
- Prompt Design Principles được hoàn thiện.

---

# End of Phase 1

Phase tiếp theo:

- Documentation Prompts
- Documentation Workflow
- Documentation Review Prompts
- Documentation Checklist
````
````md
# ============================================================================
# 9. DOCUMENTATION PROMPTS
# ============================================================================

## 9.1 Purpose

Nhóm Prompt này được sử dụng để tạo, cập nhật và chuẩn hóa toàn bộ tài liệu của dự án AnSinhSo.

Mọi Prompt phải:

- Tuân thủ Bootstrap.
- Tuân thủ Standards.
- Không thay đổi kiến trúc.
- Đồng bộ với tài liệu hiện có.

---

## 9.2 New Documentation Prompt

Sử dụng khi tạo tài liệu mới.

```text
Đây là bộ tài liệu của dự án AnSinhSo.

Hãy đọc:

00_PROJECT_BOOTSTRAP.md

↓

00_PROJECT_PROGRESS.md

↓

00_PROJECT_INDEX.md

↓

00_AI_HANDOVER.md

↓

Đọc toàn bộ Standards liên quan.

Sau đó tạo tài liệu mới:

[Tên tài liệu]

Yêu cầu:

- Markdown (.md)
- Theo đúng cấu trúc Enterprise Documentation
- Không thay đổi kiến trúc
- Có Checklist
- Có Related Documents
- Có Change Log
```

---

## 9.3 Continue Documentation Prompt

Sử dụng khi tiếp tục tài liệu đang viết.

```text
Đây là tài liệu chưa hoàn thiện.

Hãy:

Đọc Bootstrap

↓

Đọc Progress

↓

Đọc Index

↓

Đọc AI Handover

↓

Tiếp tục đúng Phase hiện tại.

Không viết lại phần đã hoàn thành.

Không thay đổi cấu trúc tài liệu.

Tiếp tục đến hết Phase.
```

---

## 9.4 Documentation Update Prompt

Sử dụng khi cập nhật tài liệu.

```text
Hãy cập nhật tài liệu sau:

[Tên tài liệu]

Yêu cầu:

- Giữ nguyên kiến trúc.
- Chỉ cập nhật phần liên quan.
- Không thay đổi nội dung đã Approved.
- Đồng bộ với Standards mới nhất.
- Cập nhật Change Log.
```

---

## 9.5 Documentation Review Prompt

```text
Hãy Review tài liệu sau.

Kiểm tra:

- Structure
- Standards
- Consistency
- Completeness
- Maintainability

Đề xuất các điểm cần cải thiện.

Không tự ý sửa nội dung.
```

---

## 9.6 Documentation Refactoring Prompt

```text
Hãy Refactor tài liệu.

Yêu cầu:

- Không thay đổi nội dung nghiệp vụ.
- Không thay đổi cấu trúc chính.
- Chỉ tối ưu cách trình bày.
- Chuẩn hóa tiêu đề.
- Chuẩn hóa Markdown.
```

---

# ============================================================================
# 10. DOCUMENTATION WORKFLOW PROMPTS
# ============================================================================

## 10.1 Documentation Planning Prompt

```text
Hãy lập kế hoạch xây dựng tài liệu:

[Tên tài liệu]

Bao gồm:

- Phase
- Nội dung từng Phase
- Checklist
- Phụ thuộc tài liệu khác
```

---

## 10.2 Documentation Completion Prompt

```text
Hãy kiểm tra tài liệu đã hoàn chỉnh hay chưa.

Kiểm tra:

- Có thiếu Phase nào không.
- Có thiếu Checklist không.
- Có thiếu Change Log không.
- Có thiếu Related Documents không.
```

---

## 10.3 Documentation Synchronization Prompt

```text
Hãy đồng bộ tài liệu:

[Tài liệu A]

với

[Tài liệu B]

Đảm bảo:

- Không mâu thuẫn.
- Không trùng lặp.
- Không mất dữ liệu.
```

---

# ============================================================================
# 11. DOCUMENTATION REVIEW PROMPTS
# ============================================================================

## 11.1 Enterprise Review Prompt

```text
Review tài liệu theo tiêu chuẩn Enterprise.

Đánh giá:

- Architecture
- Standards
- Security
- Maintainability
- Documentation Quality
- Versioning

Xuất báo cáo Review.
```

---

## 11.2 Consistency Review Prompt

```text
So sánh các tài liệu.

Kiểm tra:

- Tên Module.
- Tên API.
- Database.
- Standards.
- Quy trình.

Liệt kê các điểm chưa đồng bộ.
```

---

## 11.3 Markdown Quality Prompt

```text
Kiểm tra chất lượng Markdown.

Đánh giá:

- Heading.
- Table.
- Code Block.
- Checklist.
- Readability.
```

---

# ============================================================================
# 12. DOCUMENTATION CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2B cần xác nhận:

□ Prompt tạo tài liệu hoàn chỉnh.

□ Prompt cập nhật tài liệu hoàn chỉnh.

□ Prompt Review hoàn chỉnh.

□ Prompt Refactor hoàn chỉnh.

□ Prompt đồng bộ tài liệu hoàn chỉnh.

□ Prompt kiểm tra Markdown hoàn chỉnh.

□ Documentation Workflow được chuẩn hóa.

□ Prompt tuân thủ Bootstrap.

□ Prompt tuân thủ Standards.

---

# ============================================================================
# 13. PHASE 2A CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2B cần xác nhận:

- Documentation Prompt Library hoàn chỉnh.
- Documentation Workflow Prompt hoàn chỉnh.
- Documentation Review Prompt hoàn chỉnh.
- Documentation Checklist hoàn chỉnh.
- Prompt Structure đồng bộ với Enterprise Standards.

---

# End of Phase 2A

Phase tiếp theo:

- Database Development Prompts
- SQL Generation Prompts
- API Development Prompts
- Backend Development Prompts
- Repository Pattern Prompts
- CQRS Prompts
- Entity Framework Prompts
- Code Generation Templates
````
````md id="n7kb4x"
# ============================================================================
# 14. DATABASE DEVELOPMENT PROMPTS
# ============================================================================

## 14.1 Purpose

Nhóm Prompt này được sử dụng để phát triển Database của dự án AnSinhSo.

Mọi Prompt phải tuân thủ:

- DATABASE_DESIGN.md
- BUSINESS_RULES.md
- DATABASE_RULES.md
- PROJECT_BOOTSTRAP.md

Không được tự thay đổi Database Architecture.

---

## 14.2 Database Design Prompt

```text
Hãy đọc:

00_PROJECT_BOOTSTRAP.md

↓

00_PROJECT_PROGRESS.md

↓

DATABASE_DESIGN.md

↓

BUSINESS_RULES.md

↓

DATABASE_RULES.md

Thiết kế Database cho Module:

[Tên Module]

Yêu cầu:

- SQL Server 2022
- Chuẩn hóa 3NF
- Có PK
- Có FK
- Có Index
- Có Comment
- Không thay đổi Schema hiện có.
```

---

## 14.3 Create Table Prompt

```text
Tạo bảng mới:

[Tên bảng]

Yêu cầu:

- Theo DATABASE_DESIGN.md
- Có PK
- Có FK
- Có Constraint
- Có Index
- Có Default Value
- Không trùng với bảng hiện có.
```

---

## 14.4 Migration Prompt

```text
Sinh Migration SQL.

Yêu cầu:

- Có Version.
- Có Rollback.
- Không ảnh hưởng dữ liệu Production.
- Có Comment.
```

---

## 14.5 Seed Data Prompt

```text
Sinh Seed Data.

Yêu cầu:

- Không tạo dữ liệu trùng.
- Có thể chạy nhiều lần.
- Có dữ liệu Demo.
- Có dữ liệu Development.
```

---

# ============================================================================
# 15. SQL GENERATION PROMPTS
# ============================================================================

## 15.1 CRUD SQL Prompt

```text
Sinh SQL CRUD cho Module:

[Tên Module]

Bao gồm:

- INSERT
- UPDATE
- DELETE
- SELECT
- SEARCH
- PAGINATION

Có Transaction khi cần.
```

---

## 15.2 Stored Procedure Prompt

```text
Sinh Stored Procedure.

Yêu cầu:

- TRY...CATCH
- Transaction
- Error Handling
- Comment
- Chuẩn SQL Server
```

---

## 15.3 View Prompt

```text
Sinh View phục vụ Dashboard.

Không chứa Business Logic.

Có Comment.
```

---

## 15.4 Function Prompt

```text
Sinh SQL Function.

Ưu tiên:

- Tái sử dụng.
- Hiệu năng.
- Không xử lý phức tạp.
```

---

# ============================================================================
# 16. API DEVELOPMENT PROMPTS
# ============================================================================

## 16.1 REST API Prompt

```text
Đọc:

API_SPEC.md

↓

API_STANDARDS.md

↓

BUSINESS_RULES.md

Sinh REST API.

Yêu cầu:

- ASP.NET Core 8
- RESTful
- JWT
- Validation
- Swagger
- Logging
- Error Handling
```

---

## 16.2 Controller Prompt

```text
Sinh Controller.

Bao gồm:

- CRUD
- Authorization
- Validation
- Exception Handling
- Standard Response
```

---

## 16.3 DTO Prompt

```text
Sinh:

- Request DTO
- Response DTO
- Mapping

Không sinh Entity trong API Layer.
```

---

## 16.4 Authentication Prompt

```text
Sinh Authentication.

Bao gồm:

- Login
- Refresh Token
- JWT
- Role
- Permission
- Authorization Policy
```

---

# ============================================================================
# 17. BACKEND DEVELOPMENT PROMPTS
# ============================================================================

## 17.1 Service Prompt

```text
Sinh Service Layer.

Tuân thủ:

- SOLID
- Clean Architecture
- Dependency Injection
- Async/Await
- Logging
```

---

## 17.2 Repository Prompt

```text
Sinh Repository Pattern.

Bao gồm:

- Interface
- Implementation
- Async Methods
- Pagination
- Filtering
```

---

## 17.3 CQRS Prompt

```text
Sinh CQRS.

Bao gồm:

- Command
- Query
- Handler
- Validator
- Mapping
```

---

## 17.4 Validation Prompt

```text
Sinh Validation.

Bao gồm:

- Required
- Length
- Format
- Business Rules
```

---

## 17.5 Exception Prompt

```text
Sinh Global Exception Handling.

Bao gồm:

- Middleware
- Error Code
- Logging
- Standard Response
```

---

# ============================================================================
# 18. ENTITY FRAMEWORK PROMPTS
# ============================================================================

## 18.1 Entity Prompt

```text
Sinh Entity Framework Entity.

Theo:

DATABASE_DESIGN.md

Có:

- Navigation Property
- Data Annotation (nếu cần)
- Fluent API tương ứng
```

---

## 18.2 DbContext Prompt

```text
Sinh DbContext.

Bao gồm:

- DbSet
- Fluent API
- Seed Configuration
- Audit Fields
```

---

## 18.3 Fluent API Prompt

```text
Sinh Fluent Configuration.

Bao gồm:

- PK
- FK
- Index
- Constraint
- Relationship
```

---

## 18.4 Repository Unit Test Prompt

```text
Sinh Unit Test cho Repository.

Bao gồm:

- Success
- Validation
- Exception
- Edge Cases
```

---

# ============================================================================
# 19. CODE GENERATION TEMPLATES
# ============================================================================

## 19.1 Enterprise Generation Prompt

```text
Đọc Bootstrap.

↓

Đọc Standards.

↓

Đọc Database.

↓

Đọc API.

↓

Sinh Code Enterprise.

Bao gồm:

- Documentation
- SQL
- Entity
- DTO
- Repository
- Service
- Controller
- Unit Test

Không tạo Placeholder.
```

---

## 19.2 Enterprise Refactoring Prompt

```text
Refactor Module.

Yêu cầu:

- Không đổi API.
- Không đổi Database.
- Không đổi Business Rules.
- Tăng Maintainability.
```

---

## 19.3 Enterprise Debug Prompt

```text
Phân tích lỗi.

Bao gồm:

- Root Cause
- File liên quan
- Giải pháp
- Kiểm thử
- Rủi ro
```

---

# ============================================================================
# 20. PHASE 2B CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2C cần xác nhận:

□ Database Prompt Library hoàn chỉnh.

□ SQL Prompt Library hoàn chỉnh.

□ API Prompt Library hoàn chỉnh.

□ Backend Prompt Library hoàn chỉnh.

□ Entity Framework Prompt Library hoàn chỉnh.

□ Enterprise Code Generation Prompt hoàn chỉnh.

□ Enterprise Refactoring Prompt hoàn chỉnh.

□ Enterprise Debug Prompt hoàn chỉnh.

□ Tất cả Prompt tuân thủ Bootstrap.

□ Tất cả Prompt tuân thủ Enterprise Standards.

---

# End of Phase 2B

Phase tiếp theo:

- Frontend Development Prompts
- UI/UX Generation Prompts
- GIS Development Prompts
- AI Development Prompts
- Zalo OA Development Prompts
- Dashboard Development Prompts
- Report Generation Prompts
- JavaScript Templates
- Frontend Checklist
````
````md id="v3r9fk"
# ============================================================================
# 21. FRONTEND DEVELOPMENT PROMPTS
# ============================================================================

## 21.1 Purpose

Nhóm Prompt này được sử dụng để phát triển Frontend của dự án AnSinhSo.

Mọi Prompt phải tuân thủ:

- 07_FRONTEND_STANDARDS.md
- 14_PROJECT_STRUCTURE.md
- UI_REQUIREMENTS.md
- PROJECT_BOOTSTRAP.md

Không được tự ý thay đổi UI Architecture.

---

## 21.2 UI Page Generation Prompt

```text
Đọc:

00_PROJECT_BOOTSTRAP.md

↓

00_PROJECT_PROGRESS.md

↓

07_FRONTEND_STANDARDS.md

↓

UI_REQUIREMENTS.md

↓

14_PROJECT_STRUCTURE.md

Sinh giao diện:

[Tên Module]

Yêu cầu:

- Responsive
- Accessibility
- Clean UI
- Enterprise Design
- Không Hard-code dữ liệu
- Đồng bộ Design System
```

---

## 21.3 Component Generation Prompt

```text
Sinh Component Frontend.

Yêu cầu:

- Reusable
- Responsive
- Có Loading State
- Có Error State
- Có Empty State
- Có Comment khi cần
```

---

## 21.4 Layout Prompt

```text
Sinh Layout.

Bao gồm:

- Header
- Sidebar
- Footer
- Breadcrumb
- Notification Area
- User Profile
```

---

## 21.5 Form Prompt

```text
Sinh Form.

Bao gồm:

- Validation
- Error Message
- Required Field
- Loading
- Submit State
- Reset
```

---

# ============================================================================
# 22. UI / UX PROMPTS
# ============================================================================

## 22.1 Dashboard UI Prompt

```text
Thiết kế Dashboard Enterprise.

Bao gồm:

- KPI Cards
- Statistics
- Charts
- Quick Actions
- Recent Activities
- Notification Panel

Ưu tiên:

- Dễ đọc
- Dễ sử dụng
- Responsive
```

---

## 22.2 Data Table Prompt

```text
Sinh Data Table.

Bao gồm:

- Search
- Filter
- Pagination
- Sorting
- Export
- Row Action
```

---

## 22.3 Modal Prompt

```text
Sinh Modal.

Bao gồm:

- Confirm
- Delete
- Edit
- View Detail
- Responsive
```

---

## 22.4 Notification Prompt

```text
Sinh Notification UI.

Bao gồm:

- Success
- Error
- Warning
- Information

Không chặn thao tác người dùng.
```

---

# ============================================================================
# 23. GIS DEVELOPMENT PROMPTS
# ============================================================================

## 23.1 GIS Map Prompt

```text
Đọc:

BUSINESS_RULES.md

↓

DATABASE_DESIGN.md

↓

UI_REQUIREMENTS.md

Sinh Module GIS.

Bao gồm:

- Digital Map
- Marker
- Layer
- Legend
- Filter
- Popup
```

---

## 23.2 Household Map Prompt

```text
Sinh Bản đồ Hộ gia đình.

Hiển thị:

- Chủ hộ
- Thôn
- Tọa độ
- Nhóm đối tượng
- Trạng thái hỗ trợ
```

---

## 23.3 GIS Dashboard Prompt

```text
Sinh Dashboard GIS.

Bao gồm:

- Heatmap
- Cluster
- Boundary
- Statistics
- Layer Control
```

---

# ============================================================================
# 24. AI DEVELOPMENT PROMPTS
# ============================================================================

## 24.1 AI Assistant Prompt

```text
Sinh AI Assistant.

Bao gồm:

- Chat
- Q&A
- Search
- Suggestion
- Context Memory
- Logging
```

---

## 24.2 AI Analytics Prompt

```text
Sinh Module AI Analytics.

Bao gồm:

- Prediction
- Trend Analysis
- Risk Detection
- Recommendation
- Explain Result
```

---

## 24.3 AI Report Prompt

```text
Sinh AI Report.

Bao gồm:

- Summary
- Statistics
- Insight
- Recommendation
- Risk Analysis
```

---

# ============================================================================
# 25. ZALO OA DEVELOPMENT PROMPTS
# ============================================================================

## 25.1 Zalo OA Integration Prompt

```text
Sinh Module Zalo OA.

Bao gồm:

- Official Account
- Webhook
- Access Token
- Refresh Token
- Logging
- Retry Policy
```

---

## 25.2 Notification Prompt

```text
Sinh Notification Service.

Bao gồm:

- Broadcast
- Individual Message
- Reminder
- Payment Notice
- Emergency Notice
```

---

## 25.3 Citizen Interaction Prompt

```text
Sinh Module tương tác người dân.

Bao gồm:

- Phản ánh
- Tra cứu
- Đăng ký
- Xác nhận
- Lịch sử
```

---

# ============================================================================
# 26. DASHBOARD DEVELOPMENT PROMPTS
# ============================================================================

## 26.1 Dashboard Prompt

```text
Sinh Dashboard.

Bao gồm:

- KPI
- Charts
- Cards
- Tables
- Map
- AI Summary
```

---

## 26.2 Executive Dashboard Prompt

```text
Sinh Dashboard cho Lãnh đạo.

Bao gồm:

- Tổng quan
- Cảnh báo
- Phân tích AI
- Báo cáo nhanh
- Biểu đồ
```

---

## 26.3 Operational Dashboard Prompt

```text
Sinh Dashboard cho Cán bộ.

Bao gồm:

- Danh sách xử lý
- Tiến độ
- Thông báo
- Công việc hôm nay
```

---

# ============================================================================
# 27. REPORT GENERATION PROMPTS
# ============================================================================

## 27.1 Report Prompt

```text
Sinh Module Báo cáo.

Bao gồm:

- PDF
- Excel
- CSV
- Print
- Preview
```

---

## 27.2 Statistical Report Prompt

```text
Sinh Báo cáo thống kê.

Bao gồm:

- Theo thời gian
- Theo địa bàn
- Theo nhóm đối tượng
- Theo chính sách
```

---

## 27.3 Export Prompt

```text
Sinh chức năng Export.

Hỗ trợ:

- Excel
- PDF
- CSV

Có Logging.
```

---

# ============================================================================
# 28. FRONTEND CODE GENERATION TEMPLATES
# ============================================================================

## 28.1 Enterprise Frontend Prompt

```text
Đọc Bootstrap.

↓

Đọc UI Standards.

↓

Đọc Frontend Standards.

↓

Sinh Frontend Enterprise.

Bao gồm:

- Layout
- Component
- Service
- Routing
- Responsive
- API Integration

Không Hard-code.
```

---

## 28.2 JavaScript Prompt

```text
Sinh JavaScript Module.

Bao gồm:

- Async API
- Validation
- Error Handling
- Loading
- Reusable Function
```

---

## 28.3 CSS Prompt

```text
Sinh CSS.

Ưu tiên:

- Responsive
- Modular
- Reusable
- Accessible

Không trùng lặp Style.
```

---

# ============================================================================
# 29. PHASE 2C CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2D cần xác nhận:

□ Frontend Prompt Library hoàn chỉnh.

□ UI/UX Prompt Library hoàn chỉnh.

□ GIS Prompt Library hoàn chỉnh.

□ AI Prompt Library hoàn chỉnh.

□ Zalo OA Prompt Library hoàn chỉnh.

□ Dashboard Prompt Library hoàn chỉnh.

□ Report Prompt Library hoàn chỉnh.

□ JavaScript Prompt Library hoàn chỉnh.

□ CSS Prompt Library hoàn chỉnh.

□ Tất cả Prompt tuân thủ Bootstrap và Enterprise Standards.

---

# End of Phase 2C

Phase tiếp theo:

- Testing Prompt Library
- Security Review Prompt Library
- Performance Optimization Prompt Library
- DevOps Prompt Library
- Deployment Prompt Library
- Docker Prompt Library
- CI/CD Prompt Library
- Code Review Prompt Library
- Enterprise QA Prompt Library
- Release Management Prompt Library
````
````md id="t4m8qa"
# ============================================================================
# 30. TESTING PROMPT LIBRARY
# ============================================================================

## 30.1 Purpose

Nhóm Prompt này được sử dụng để xây dựng toàn bộ hoạt động kiểm thử của dự án AnSinhSo.

Mọi Prompt phải tuân thủ:

- 11_TESTING_STANDARDS.md
- BUSINESS_RULES.md
- API_SPEC.md
- DATABASE_DESIGN.md

Không tạo Test Case trái với Business Rules.

---

## 30.2 Test Case Generation Prompt

```text
Đọc:

00_PROJECT_BOOTSTRAP.md

↓

11_TESTING_STANDARDS.md

↓

BUSINESS_RULES.md

↓

API_SPEC.md

Sinh Test Case cho Module:

[Tên Module]

Bao gồm:

- Test ID
- Preconditions
- Test Steps
- Expected Result
- Priority
- Test Type
```

---

## 30.3 Unit Test Prompt

```text
Sinh Unit Test.

Bao gồm:

- Arrange
- Act
- Assert

Kiểm thử:

- Success
- Validation
- Exception
- Boundary Value
- Null Value
```

---

## 30.4 Integration Test Prompt

```text
Sinh Integration Test.

Kiểm tra:

- API
- Database
- Authentication
- Transaction
- Logging
```

---

## 30.5 Regression Test Prompt

```text
Sinh Regression Test.

Đảm bảo:

- Không làm hỏng chức năng cũ.
- Kiểm tra toàn bộ Module liên quan.
```

---

# ============================================================================
# 31. SECURITY REVIEW PROMPTS
# ============================================================================

## 31.1 Security Audit Prompt

```text
Review Module theo tiêu chuẩn OWASP.

Kiểm tra:

- SQL Injection
- XSS
- CSRF
- Broken Authentication
- Sensitive Data Exposure
- Security Headers
```

---

## 31.2 Authentication Review Prompt

```text
Review Authentication.

Kiểm tra:

- JWT
- Refresh Token
- Password Hash
- Authorization
- Session
- Role
```

---

## 31.3 Secure Coding Prompt

```text
Kiểm tra Secure Coding.

Đánh giá:

- Input Validation
- Output Encoding
- Exception Handling
- Logging
- Secret Management
```

---

# ============================================================================
# 32. PERFORMANCE OPTIMIZATION PROMPTS
# ============================================================================

## 32.1 Backend Performance Prompt

```text
Phân tích hiệu năng Backend.

Đánh giá:

- SQL Query
- Memory
- CPU
- Async
- Cache
- Logging
```

---

## 32.2 Database Optimization Prompt

```text
Tối ưu Database.

Kiểm tra:

- Index
- Execution Plan
- Slow Query
- Deadlock
- Fragmentation
```

---

## 32.3 Frontend Optimization Prompt

```text
Tối ưu Frontend.

Kiểm tra:

- Bundle Size
- Lazy Loading
- Rendering
- Images
- JavaScript
```

---

# ============================================================================
# 33. DEVOPS PROMPT LIBRARY
# ============================================================================

## 33.1 CI/CD Prompt

```text
Sinh Pipeline CI/CD.

Bao gồm:

- Build
- Test
- Security Scan
- Package
- Deploy
- Notification
```

---

## 33.2 Infrastructure Prompt

```text
Sinh Infrastructure Deployment.

Bao gồm:

- IIS
- SQL Server
- Redis
- Reverse Proxy
- SSL
- Firewall
```

---

## 33.3 Monitoring Prompt

```text
Sinh Monitoring.

Bao gồm:

- Health Check
- Logging
- Metrics
- Alert
- Dashboard
```

---

# ============================================================================
# 34. DEPLOYMENT PROMPTS
# ============================================================================

## 34.1 Production Deployment Prompt

```text
Sinh hướng dẫn Production Deployment.

Bao gồm:

- Publish
- Backup
- Migration
- Configuration
- Verification
- Rollback
```

---

## 34.2 Docker Prompt

```text
Sinh Docker.

Bao gồm:

- Dockerfile
- docker-compose
- Environment
- Network
- Volume
```

---

## 34.3 Release Prompt

```text
Sinh Release Checklist.

Bao gồm:

- Build
- Test
- Backup
- Deploy
- Verify
- Monitor
```

---

# ============================================================================
# 35. CODE REVIEW PROMPTS
# ============================================================================

## 35.1 Enterprise Code Review Prompt

```text
Review Source Code.

Đánh giá:

- Clean Architecture
- SOLID
- Naming
- Maintainability
- Security
- Performance
- Readability

Không tự sửa Code.
```

---

## 35.2 Refactoring Review Prompt

```text
Đánh giá Refactoring.

Kiểm tra:

- Có thay đổi Business Logic không.
- Có thay đổi API không.
- Có thay đổi Database không.
- Có tăng Maintainability không.
```

---

## 35.3 Pull Request Review Prompt

```text
Review Pull Request.

Kiểm tra:

- Coding Standards
- Security
- Testing
- Documentation
- Performance
- Breaking Changes
```

---

# ============================================================================
# 36. ENTERPRISE QA PROMPTS
# ============================================================================

## 36.1 Quality Assessment Prompt

```text
Đánh giá chất lượng Module.

Bao gồm:

- Functionality
- Reliability
- Performance
- Security
- Maintainability
- Scalability
```

---

## 36.2 Release Readiness Prompt

```text
Đánh giá Module trước Release.

Kiểm tra:

- Testing
- Documentation
- Security
- Performance
- Deployment
```

---

## 36.3 Risk Assessment Prompt

```text
Phân tích rủi ro.

Bao gồm:

- Technical Risk
- Business Risk
- Security Risk
- Operational Risk

Đưa ra mức độ ưu tiên xử lý.
```

---

# ============================================================================
# 37. RELEASE MANAGEMENT PROMPTS
# ============================================================================

## 37.1 Release Planning Prompt

```text
Lập kế hoạch Release.

Bao gồm:

- Scope
- Timeline
- Resources
- Risks
- Rollback Plan
```

---

## 37.2 Sprint Closing Prompt

```text
Đánh giá Sprint.

Bao gồm:

- Completed Tasks
- Pending Tasks
- Issues
- Lessons Learned
- Next Sprint
```

---

## 37.3 Project Health Prompt

```text
Đánh giá sức khỏe dự án.

Kiểm tra:

- Documentation
- Code Quality
- Testing
- Technical Debt
- Security
- Deployment
- Overall Progress
```

---

# ============================================================================
# 38. PHASE 2D CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2E cần xác nhận:

□ Testing Prompt Library hoàn chỉnh.

□ Security Prompt Library hoàn chỉnh.

□ Performance Prompt Library hoàn chỉnh.

□ DevOps Prompt Library hoàn chỉnh.

□ Deployment Prompt Library hoàn chỉnh.

□ Docker Prompt Library hoàn chỉnh.

□ Code Review Prompt Library hoàn chỉnh.

□ Enterprise QA Prompt Library hoàn chỉnh.

□ Release Management Prompt Library hoàn chỉnh.

□ Tất cả Prompt tuân thủ Enterprise Standards.

---

# End of Phase 2D

Phase tiếp theo:

- Enterprise Prompt Governance
- Prompt Lifecycle
- Prompt Versioning
- Prompt Quality Standards
- Prompt Naming Convention
- Prompt Repository Management
- AI Collaboration Rules
- Prompt Audit
- Change Log
- End of Document
````
````md id="c4x9ep"
# ============================================================================
# 39. ENTERPRISE PROMPT GOVERNANCE
# ============================================================================

## 39.1 Purpose

Phần này quy định các nguyên tắc quản trị (Governance) đối với toàn bộ Prompt được sử dụng trong dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa Prompt.
- Đảm bảo tính nhất quán.
- Tăng khả năng tái sử dụng.
- Giảm Prompt trùng lặp.
- Dễ bảo trì.
- Dễ kiểm toán.

Prompt được xem là một tài sản (Project Asset) và phải được quản lý tương tự như Source Code.

---

## 39.2 Governance Principles

Mọi Prompt phải tuân thủ các nguyên tắc:

- Có Context đầy đủ.
- Có Scope rõ ràng.
- Có Expected Output.
- Có Standards tham chiếu.
- Có Constraints.
- Có khả năng tái sử dụng.
- Có Version.
- Có Change Log.

---

## 39.3 AI First Principles

Khi làm việc với AI:

AI phải:

- Đọc Bootstrap.
- Đọc Progress.
- Đọc Index.
- Đọc AI Handover.
- Đọc Standards liên quan.
- Đọc tài liệu Module liên quan.

Sau đó mới sinh nội dung.

Không được bỏ qua bước Bootstrap.

---

# ============================================================================
# 40. PROMPT LIFECYCLE
# ============================================================================

## 40.1 Prompt Lifecycle

```text
Create

↓

Review

↓

Approve

↓

Use

↓

Improve

↓

Version

↓

Archive
```

---

## 40.2 Lifecycle Rules

Prompt mới phải:

- Được Review.
- Được thử nghiệm.
- Được chuẩn hóa.
- Được lưu vào Prompt Library.

Không sử dụng Prompt chưa được đánh giá cho Production.

---

## 40.3 Prompt Maintenance

Định kỳ:

- Loại bỏ Prompt trùng.
- Cập nhật Prompt lỗi thời.
- Đồng bộ Standards mới.
- Bổ sung ví dụ thực tế.
- Đánh giá hiệu quả Prompt.

---

# ============================================================================
# 41. PROMPT VERSIONING
# ============================================================================

## 41.1 Version Format

Prompt sử dụng Semantic Versioning:

```text
MAJOR.MINOR.PATCH
```

Ví dụ:

```text
1.0.0

1.1.0

1.2.1

2.0.0
```

---

## 41.2 Version Rules

| Thay đổi | Version |
|----------|----------|
| Sửa lỗi Prompt | PATCH |
| Thêm Prompt mới | MINOR |
| Thay đổi cấu trúc Prompt | MAJOR |

---

## 41.3 Change Management

Mỗi lần cập nhật Prompt cần:

- Ghi Version.
- Ghi ngày cập nhật.
- Ghi lý do thay đổi.
- Ghi tài liệu liên quan.

---

# ============================================================================
# 42. PROMPT QUALITY STANDARDS
# ============================================================================

## 42.1 Quality Criteria

Prompt đạt chuẩn khi:

- Rõ ràng.
- Không mơ hồ.
- Có đầu vào xác định.
- Có đầu ra mong đợi.
- Có khả năng tái sử dụng.
- Có khả năng mở rộng.

---

## 42.2 Quality Checklist

Kiểm tra:

□ Có Context.

□ Có Scope.

□ Có Standards.

□ Có Constraints.

□ Có Output Format.

□ Không mâu thuẫn với Bootstrap.

□ Không mâu thuẫn với Standards.

□ Không mâu thuẫn với Business Rules.

---

## 42.3 Anti-Pattern

Không sử dụng Prompt:

- Thiếu Context.
- Quá ngắn.
- Không có mục tiêu.
- Mâu thuẫn Standards.
- Tự thay đổi Architecture.
- Tự thay đổi Business Rules.

---

# ============================================================================
# 43. PROMPT NAMING CONVENTION
# ============================================================================

## 43.1 Naming Format

Tên Prompt sử dụng định dạng:

```text
<Category>_<Purpose>_<Version>
```

Ví dụ:

```text
Database_CreateTable_v1

Backend_GenerateAPI_v1

Frontend_CreateDashboard_v1

Testing_GenerateTestCase_v1
```

---

## 43.2 Category

Các Category:

- Documentation
- Database
- API
- Backend
- Frontend
- AI
- GIS
- Zalo
- Security
- Testing
- DevOps
- Deployment

---

# ============================================================================
# 44. PROMPT REPOSITORY MANAGEMENT
# ============================================================================

## 44.1 Prompt Library Structure

```text
PromptLibrary/

│

├── Documentation/

├── Database/

├── API/

├── Backend/

├── Frontend/

├── AI/

├── GIS/

├── Zalo/

├── Testing/

├── Security/

├── DevOps/

└── Deployment/
```

---

## 44.2 Storage Rules

Prompt phải:

- Có phân loại.
- Có Version.
- Có Owner.
- Có Change Log.
- Có ví dụ sử dụng.

---

## 44.3 Prompt Reuse

Ưu tiên:

- Tái sử dụng Prompt hiện có.
- Không tạo Prompt trùng lặp.
- Chuẩn hóa Prompt dùng chung.

---

# ============================================================================
# 45. AI COLLABORATION RULES
# ============================================================================

## 45.1 Supported AI Platforms

Prompt Library được thiết kế để sử dụng với:

- ChatGPT
- Gemini
- GitHub Copilot
- Cursor
- Cline
- Continue
- AntiGravity AI

---

## 45.2 Collaboration Principles

Khi chuyển đổi giữa các AI:

- Luôn bắt đầu bằng Bootstrap Prompt.
- Không thay đổi Standards.
- Không thay đổi Architecture.
- Tiếp tục đúng Sprint hiện tại.
- Đồng bộ Documentation.

---

## 45.3 Handover Rules

Khi chuyển sang cuộc trò chuyện mới:

AI phải đọc:

```text
00_PROJECT_BOOTSTRAP.md

↓

00_PROJECT_PROGRESS.md

↓

00_PROJECT_INDEX.md

↓

00_AI_HANDOVER.md
```

Sau đó tiếp tục công việc mà không làm mất ngữ cảnh.

---

# ============================================================================
# 46. PROMPT AUDIT
# ============================================================================

## 46.1 Audit Scope

Định kỳ đánh giá Prompt theo các tiêu chí:

- Tính chính xác.
- Tính đầy đủ.
- Khả năng tái sử dụng.
- Mức độ nhất quán.
- Tuân thủ Standards.
- Hiệu quả sử dụng.

---

## 46.2 Audit Checklist

□ Prompt đúng Bootstrap.

□ Prompt đúng Standards.

□ Prompt đúng Business Rules.

□ Prompt đúng Architecture.

□ Prompt có Version.

□ Prompt có Change Log.

□ Prompt có ví dụ sử dụng.

□ Prompt không trùng lặp.

---

# ============================================================================
# 47. ENTERPRISE PROMPT CHECKLIST
# ============================================================================

Trước khi phát hành Prompt Library cần xác nhận:

□ Enterprise Prompt Governance hoàn chỉnh.

□ Prompt Lifecycle được chuẩn hóa.

□ Prompt Versioning được áp dụng.

□ Prompt Quality Standards hoàn chỉnh.

□ Prompt Naming Convention hoàn chỉnh.

□ Prompt Repository Management hoàn chỉnh.

□ AI Collaboration Rules hoàn chỉnh.

□ Prompt Audit hoàn chỉnh.

□ Prompt Library đồng bộ với toàn bộ Standards.

---

# ============================================================================
# 48. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------------|-------------------------------------------|
| 1.0.0 | 2026-07-12 | Initial Enterprise Prompt Library |

---

# ============================================================================
# 49. RELATED DOCUMENTS
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
- 14_PROJECT_STRUCTURE.md

---

## Core Documents

- BUSINESS_RULES.md
- DATABASE_DESIGN.md
- API_SPEC.md
- UI_REQUIREMENTS.md
- DEPLOYMENT_GUIDE.md

---

# ============================================================================
# 50. END OF DOCUMENT
# ============================================================================

Tài liệu **15_CODING_PROMPTS.md** là thư viện Prompt chính thức của dự án AnSinhSo.

Tất cả Prompt được sử dụng trong quá trình:

- Phân tích nghiệp vụ.
- Thiết kế hệ thống.
- Thiết kế cơ sở dữ liệu.
- Phát triển Backend.
- Phát triển Frontend.
- Tích hợp GIS.
- Tích hợp AI.
- Tích hợp Zalo Official Account.
- Kiểm thử.
- Triển khai.
- Bảo trì.

đều phải tuân thủ tài liệu này.

Mọi thay đổi đối với Prompt Library phải:

- Được đánh giá tác động.
- Được cập nhật Change Log.
- Được đồng bộ với Bootstrap.
- Được Project Owner hoặc Technical Lead phê duyệt.

---

# END OF FILE
````
