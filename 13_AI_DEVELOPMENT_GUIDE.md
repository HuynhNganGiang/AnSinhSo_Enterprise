````md
# ============================================================================
# 13_AI_DEVELOPMENT_GUIDE.md
# ============================================================================
#
# Project         : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
# Document Type   : Enterprise AI Development Guide
# Version         : 1.0.0
# Status          : Approved
#
# Architecture    : Enterprise Clean Architecture
# Framework       : ASP.NET Core 8
# Database        : SQL Server 2022
# Frontend        : HTML / CSS / JavaScript
# AI Integration  : ChatGPT / Gemini / GitHub Copilot / Cursor / Cline / Continue
#
# Last Updated    : 2026-07-12
#
# ============================================================================

# 1. PURPOSE

Tài liệu này quy định cách sử dụng AI trong toàn bộ vòng đời phát triển dự án AnSinhSo.

Đây là tài liệu trung tâm dành cho tất cả AI Coding Assistant tham gia phát triển dự án.

Mục tiêu:

- Chuẩn hóa cách AI làm việc.
- Đảm bảo AI không phá vỡ kiến trúc.
- Tăng năng suất phát triển.
- Đảm bảo tính nhất quán của mã nguồn.
- Hỗ trợ phát triển dài hạn.
- Giảm lỗi do AI sinh mã.

---

# 2. SCOPE

Áp dụng cho toàn bộ hoạt động:

- Phân tích yêu cầu.
- Thiết kế hệ thống.
- Thiết kế Database.
- Thiết kế API.
- Phát triển Backend.
- Phát triển Frontend.
- Viết SQL.
- Viết Documentation.
- Sinh Unit Test.
- Sinh Integration Test.
- Hỗ trợ DevOps.
- Hỗ trợ Deployment.
- Hỗ trợ Review Code.
- Hỗ trợ Refactoring.
- Hỗ trợ Debug.
- Hỗ trợ Viết Báo Cáo.

---

# 3. SUPPORTED AI ASSISTANTS

Tài liệu này áp dụng cho:

- ChatGPT
- Gemini
- GitHub Copilot
- Cursor AI
- Cline
- Continue
- AntiGravity AI
- Các AI Coding Assistant khác

Tất cả đều phải tuân thủ tài liệu này.

---

# 4. AI DEVELOPMENT OBJECTIVES

Mục tiêu của AI:

- Hỗ trợ Developer.
- Không thay thế quyết định kiến trúc.
- Sinh mã theo Standards.
- Sinh tài liệu đồng bộ.
- Sinh Test Case.
- Sinh Documentation.
- Giảm lỗi.
- Tăng khả năng bảo trì.

---

# 5. AI DEVELOPMENT PRINCIPLES

AI phải tuân thủ:

- Architecture First.
- Standards First.
- Security First.
- Documentation First.
- Testing First.
- Maintainability First.
- Consistency First.

Không được ưu tiên tốc độ hơn chất lượng.

---

# 6. AI RESPONSIBILITIES

AI được phép hỗ trợ:

- Thiết kế.
- Viết Code.
- Sinh SQL.
- Sinh API.
- Sinh Unit Test.
- Sinh Documentation.
- Sinh Deployment Script.
- Sinh CI/CD Pipeline.
- Review Code.
- Giải thích mã nguồn.
- Phân tích lỗi.

AI không thay thế vai trò phê duyệt của con người.

---

# 7. AI DEVELOPMENT LIFECYCLE

```text
Requirement

↓

Analysis

↓

Architecture

↓

Database

↓

Backend

↓

Frontend

↓

Testing

↓

Deployment

↓

Documentation

↓

Review

↓

Maintenance
```

---

# 8. AI SUCCESS METRICS

Đánh giá AI dựa trên:

- Code Quality.
- Test Coverage.
- Documentation Coverage.
- Security Compliance.
- Standards Compliance.
- Maintainability.
- Defect Rate.

---

# 9. AI DEVELOPMENT BASELINE

Trước khi AI sinh mã, bắt buộc phải:

- Đọc Bootstrap.
- Đọc Project Progress.
- Đọc Project Index.
- Đọc AI Handover.
- Đọc Standards liên quan.
- Xác định Sprint hiện tại.
- Xác định Module đang phát triển.

Nếu thiếu tài liệu, AI phải yêu cầu người dùng cung cấp.

---

# 10. PHASE 1 CHECKLIST

Trước khi chuyển sang Phase 2A cần xác nhận:

- AI hiểu kiến trúc dự án.
- AI hiểu Standards.
- AI hiểu Sprint hiện tại.
- AI hiểu quy trình làm việc.
- AI sẵn sàng phát triển theo Enterprise Standards.

---

# End of Phase 1

Phase tiếp theo:

- AI Context Management
- Bootstrap Loading
- AI Memory Rules
- Prompt Engineering Standards
- AI Decision Rules
- AI Architecture Rules
- AI Collaboration Workflow
- AI Documentation Workflow
````
````md
# ============================================================================
# 11. AI CONTEXT MANAGEMENT
# ============================================================================

## 11.1 Purpose

AI Context Management quy định cách AI quản lý ngữ cảnh trong suốt vòng đời phát triển dự án.

Mục tiêu:

- Không mất ngữ cảnh khi chuyển Chat.
- Không phá vỡ kiến trúc.
- Duy trì tính nhất quán.
- Tiếp tục đúng Sprint hiện tại.
- Hạn chế AI suy diễn.

---

## 11.2 Context Priority

AI phải ưu tiên ngữ cảnh theo thứ tự sau:

```text
1. User Instructions

↓

2. 00_AI_HANDOVER.md

↓

3. 00_PROJECT_PROGRESS.md

↓

4. 00_PROJECT_BOOTSTRAP.md

↓

5. 00_PROJECT_INDEX.md

↓

6. Standards Documents

↓

7. Business Documents

↓

8. Current Task
```

Không được ưu tiên kiến thức chung hơn tài liệu của dự án.

---

## 11.3 Context Rules

AI phải:

- Đọc đầy đủ Bootstrap.
- Xác định Sprint hiện tại.
- Xác định Module hiện tại.
- Xác định Standards liên quan.
- Kiểm tra tài liệu đã Approved.

Không được bắt đầu viết Code nếu chưa hoàn thành các bước trên.

---

## 11.4 Context Validation

Trước khi thực hiện bất kỳ yêu cầu nào, AI phải tự xác nhận:

- Đã hiểu phạm vi công việc.
- Đã xác định đúng tài liệu.
- Đã xác định đúng Standards.
- Không có xung đột kiến trúc.

---

# ============================================================================
# 12. BOOTSTRAP LOADING STANDARD
# ============================================================================

## 12.1 Mandatory Loading Order

AI phải đọc đúng thứ tự:

```text
00_PROJECT_BOOTSTRAP.md

↓

00_PROJECT_PROGRESS.md

↓

00_PROJECT_INDEX.md

↓

00_AI_HANDOVER.md
```

Sau đó mới được đọc các tài liệu chuyên môn.

---

## 12.2 Project Initialization

Sau khi đọc Bootstrap, AI phải xác định:

- Current Sprint.
- Current Phase.
- Current Module.
- Approved Documents.
- Pending Documents.
- Next Action.

---

## 12.3 Bootstrap Verification

Nếu thiếu:

- Bootstrap
- Progress
- Index
- AI Handover

AI phải yêu cầu người dùng bổ sung.

Không được tự suy diễn.

---

# ============================================================================
# 13. AI MEMORY RULES
# ============================================================================

## 13.1 Principle

AI chỉ sử dụng:

- Bootstrap.
- Documentation.
- Current Conversation.

Không được giả định những thông tin không tồn tại trong tài liệu.

---

## 13.2 Long-term Context

Thông tin cần duy trì:

- Kiến trúc.
- Quy tắc.
- Standards.
- Sprint.
- Roadmap.

---

## 13.3 Short-term Context

Thông tin chỉ dùng trong Sprint hiện tại:

- Task.
- Module.
- Refactor nhỏ.
- Bug Fix.
- Review.

---

## 13.4 Memory Consistency

Nếu có xung đột giữa:

- Conversation

và

- Bootstrap

thì Bootstrap được ưu tiên.

Trừ khi người dùng yêu cầu thay đổi.

---

# ============================================================================
# 14. AI SESSION MANAGEMENT
# ============================================================================

## 14.1 New Chat

Khi chuyển sang Chat mới:

AI phải:

- Đọc Bootstrap.
- Khởi tạo Context.
- Xác nhận Current Sprint.
- Chờ nhiệm vụ tiếp theo.

---

## 14.2 Long Conversation

Nếu cuộc trò chuyện quá dài:

Không được yêu cầu người dùng giải thích lại toàn bộ dự án.

Chỉ yêu cầu:

Bootstrap Version mới nhất.

---

## 14.3 Session Recovery

Nếu mất Context:

AI phải:

- Đọc lại Bootstrap.
- Đọc Progress.
- Đọc Index.
- Đọc AI Handover.

---

# ============================================================================
# 15. PROJECT STATE MANAGEMENT
# ============================================================================

## 15.1 Project State

AI phải luôn biết:

- Sprint.
- Phase.
- Module.
- Milestone.
- Architecture Status.

---

## 15.2 Approved Status

Các trạng thái:

```text
Draft

↓

Review

↓

Approved

↓

Released
```

AI không được sửa tài liệu đã Approved nếu không có yêu cầu.

---

## 15.3 Sprint Tracking

Sau mỗi Sprint cần cập nhật:

- Progress.
- Change Log.
- Roadmap.
- Current Task.

---

# ============================================================================
# 16. AI TASK INITIALIZATION
# ============================================================================

Trước mỗi nhiệm vụ AI phải xác định:

- Mục tiêu.
- Phạm vi.
- Module liên quan.
- Standards liên quan.
- Tài liệu cần cập nhật.

---

## 16.1 Example Workflow

```text
User Request

↓

Read Bootstrap

↓

Read Progress

↓

Locate Module

↓

Read Standards

↓

Generate Solution

↓

Update Documentation
```

---

# ============================================================================
# 17. CONTEXT LOSS PREVENTION
# ============================================================================

Để tránh mất ngữ cảnh:

AI phải:

- Không suy diễn.
- Không tạo lại tài liệu.
- Không thay đổi Architecture.
- Không bỏ qua Bootstrap.
- Không bỏ qua Standards.

---

## 17.1 Recovery Strategy

Nếu không chắc chắn:

- Hỏi người dùng.
- Kiểm tra Bootstrap.
- Kiểm tra Progress.
- Kiểm tra Index.

---

# ============================================================================
# 18. AI CONTEXT CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2B cần xác nhận:

- Bootstrap đã được đọc.
- Context đã được khởi tạo.
- Sprint được xác định.
- Architecture được khóa.
- Standards được xác định.
- Approved Documents được nhận diện.
- Current Task được xác định.
- Không có xung đột ngữ cảnh.

---

# End of Phase 2A

Phase tiếp theo:

- Prompt Engineering Standards
- Prompt Templates
- Prompt Versioning
- AI Workflow Standards
- AI Decision Rules
- AI Communication Rules
- AI Collaboration Standards
- Multi-AI Workflow
- AI Output Standards
- Prompt Quality Control
````
````md
# ============================================================================
# 19. PROMPT ENGINEERING STANDARDS
# ============================================================================

## 19.1 Purpose

Prompt Engineering là tập hợp các tiêu chuẩn giúp AI hiểu đúng yêu cầu, tạo ra kết quả chính xác, nhất quán và phù hợp với kiến trúc của dự án AnSinhSo.

Mọi Prompt sử dụng trong dự án phải hướng đến:

- Chính xác.
- Rõ ràng.
- Có khả năng tái sử dụng.
- Có khả năng mở rộng.
- Không gây hiểu sai ngữ cảnh.

---

## 19.2 Prompt Principles

Prompt phải tuân thủ:

- Context First
- Requirement First
- Standards First
- Security First
- Maintainability First

Không được viết Prompt quá mơ hồ.

---

## 19.3 Prompt Structure

Một Prompt chuẩn nên gồm:

```text
Project Context

↓

Task

↓

Requirements

↓

Constraints

↓

Expected Output

↓

Related Documents
```

---

## 19.4 Prompt Categories

Bao gồm:

- Documentation Prompt
- Coding Prompt
- Database Prompt
- API Prompt
- Frontend Prompt
- Backend Prompt
- Security Prompt
- Testing Prompt
- DevOps Prompt
- Review Prompt

---

# ============================================================================
# 20. PROMPT TEMPLATES
# ============================================================================

## 20.1 Documentation Prompt

Mẫu:

```text
Context

↓

Read Bootstrap

↓

Read Standards

↓

Continue Current Sprint

↓

Output Markdown
```

---

## 20.2 Coding Prompt

Mẫu:

```text
Read Bootstrap

↓

Read Standards

↓

Read Database

↓

Read API

↓

Generate Enterprise Code

↓

Generate Tests

↓

Update Documentation
```

---

## 20.3 Bug Fix Prompt

Mẫu:

```text
Describe Issue

↓

Locate Module

↓

Analyze Root Cause

↓

Propose Solution

↓

Update Tests

↓

Update Documentation
```

---

## 20.4 Refactoring Prompt

AI chỉ được Refactor khi:

- Có yêu cầu.
- Không phá vỡ kiến trúc.
- Không đổi API.
- Không đổi Database.
- Không đổi Business Rules.

---

# ============================================================================
# 21. PROMPT VERSIONING
# ============================================================================

## 21.1 Purpose

Prompt cũng là tài sản của dự án và cần được quản lý phiên bản.

---

## 21.2 Version Rules

Mỗi Prompt nên có:

- Version.
- Date.
- Author.
- Purpose.
- Change Log.

---

## 21.3 Change Management

Nếu Prompt thay đổi:

- Cập nhật Version.
- Ghi Change Log.
- Đánh giá tác động.

---

# ============================================================================
# 22. AI WORKFLOW STANDARDS
# ============================================================================

## 22.1 Standard Workflow

AI phải làm việc theo quy trình:

```text
Read Bootstrap

↓

Read Progress

↓

Read Index

↓

Read Standards

↓

Understand Task

↓

Generate Solution

↓

Validate

↓

Update Documentation
```

---

## 22.2 Documentation First

Nếu nhiệm vụ yêu cầu tạo mới:

- Module
- API
- Database
- Deployment

AI phải kiểm tra tài liệu liên quan trước khi sinh mã.

---

## 22.3 Validation

Sau khi sinh nội dung, AI phải tự đánh giá:

- Có đúng Standards?
- Có đúng Sprint?
- Có đúng Architecture?
- Có phá vỡ tài liệu cũ không?

---

# ============================================================================
# 23. AI DECISION RULES
# ============================================================================

## 23.1 Decision Priority

AI ưu tiên theo thứ tự:

```text
User Request

↓

Bootstrap

↓

AI Handover

↓

Project Progress

↓

Standards

↓

Business Rules

↓

Technical Knowledge
```

---

## 23.2 Decision Restrictions

AI không được tự quyết định:

- Đổi Architecture.
- Đổi Database.
- Đổi API.
- Đổi Security Policy.
- Đổi Deployment Strategy.

---

## 23.3 Conflict Resolution

Nếu có xung đột:

- Hỏi người dùng.
- Không tự lựa chọn.

---

# ============================================================================
# 24. AI COMMUNICATION RULES
# ============================================================================

## 24.1 Communication Principles

AI cần:

- Rõ ràng.
- Chính xác.
- Có cấu trúc.
- Dễ theo dõi.

---

## 24.2 Status Reporting

Sau mỗi nhiệm vụ nên báo:

- Đã hoàn thành.
- Đang thực hiện.
- Bước tiếp theo.

---

## 24.3 Transparency

Nếu AI không chắc chắn:

- Nói rõ.
- Không suy diễn.
- Không tạo thông tin giả.

---

# ============================================================================
# 25. AI COLLABORATION STANDARDS
# ============================================================================

## 25.1 Multi-AI Collaboration

Dự án có thể sử dụng nhiều AI:

- ChatGPT
- Gemini
- Cursor
- Copilot
- Continue
- Cline

Tất cả phải dùng chung Bootstrap.

---

## 25.2 Shared Standards

Mọi AI phải:

- Đọc Bootstrap.
- Đọc Standards.
- Tuân thủ Architecture.

---

## 25.3 Responsibility

AI chỉ chịu trách nhiệm với phần mình tạo.

Việc phê duyệt thuộc về người dùng.

---

# ============================================================================
# 26. AI OUTPUT STANDARDS
# ============================================================================

## 26.1 Output Requirements

Output cần:

- Có cấu trúc.
- Dễ đọc.
- Có thể sử dụng ngay.
- Đồng bộ với dự án.

---

## 26.2 Markdown Standards

Tài liệu phải:

- Có tiêu đề.
- Có đánh số.
- Có Checklist khi cần.
- Có Change Log nếu cập nhật.

---

## 26.3 Code Standards

Nếu sinh Code:

- Tuân thủ Coding Standards.
- Có Comment khi cần.
- Có Error Handling.
- Có Validation.
- Có Logging nếu phù hợp.

---

# ============================================================================
# 27. PROMPT QUALITY CONTROL
# ============================================================================

## 27.1 Quality Checklist

Prompt tốt cần:

- Đủ Context.
- Đủ Constraints.
- Đủ Expected Output.
- Không mơ hồ.
- Không mâu thuẫn.

---

## 27.2 Review Checklist

Trước khi sử dụng Prompt cần kiểm tra:

- Đúng Bootstrap.
- Đúng Sprint.
- Đúng Standards.
- Đúng Module.
- Đúng mục tiêu.

---

# ============================================================================
# 28. PHASE 2B CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2C cần xác nhận:

- Prompt Standards hoàn chỉnh.
- Prompt Templates được chuẩn hóa.
- Prompt Versioning được áp dụng.
- AI Workflow được chuẩn hóa.
- AI Decision Rules rõ ràng.
- AI Communication thống nhất.
- Multi-AI Collaboration được hỗ trợ.
- AI Output Standards được xác định.
- Prompt Quality Control hoàn chỉnh.

---

# End of Phase 2B

Phase tiếp theo:

- AI Coding Standards
- AI Code Generation Rules
- AI Database Development
- AI API Development
- AI Frontend Development
- AI Backend Development
- AI Refactoring Rules
- AI Debugging Standards
- AI Security Development
- AI Performance Optimization
````
````md
# ============================================================================
# 29. AI CODING STANDARDS
# ============================================================================

## 29.1 Purpose

Quy định tiêu chuẩn khi AI sinh mã nguồn cho dự án AnSinhSo.

Mục tiêu:

- Sinh mã nhất quán.
- Tuân thủ Clean Architecture.
- Dễ bảo trì.
- Dễ mở rộng.
- Không phá vỡ kiến trúc.

---

## 29.2 Coding Principles

AI phải tuân thủ:

- SOLID Principles
- Clean Code
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple)
- Separation of Concerns
- Dependency Injection
- Repository Pattern
- Unit of Work (nếu áp dụng)

---

## 29.3 Coding Workflow

```text
Read Bootstrap

↓

Read Standards

↓

Read Module

↓

Analyze Requirement

↓

Generate Code

↓

Generate Tests

↓

Generate Documentation

↓

Validate
```

---

## 29.4 Code Quality Goals

Mã nguồn phải:

- Có khả năng mở rộng.
- Có khả năng kiểm thử.
- Có khả năng bảo trì.
- Có Logging phù hợp.
- Có Error Handling.
- Có Validation.

---

# ============================================================================
# 30. AI CODE GENERATION RULES
# ============================================================================

## 30.1 Before Writing Code

AI phải xác định:

- Module.
- Layer.
- Business Rules.
- Database.
- API.
- Security.
- Testing.

---

## 30.2 Generated Code

Mọi mã sinh ra cần:

- Biên dịch được.
- Không lỗi cú pháp.
- Không sinh mã mẫu (placeholder) nếu chưa được ghi rõ.
- Không tạo TODO không cần thiết.
- Không tạo mã trùng lặp.

---

## 30.3 Naming Convention

Tuân thủ:

- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md

Không tự tạo quy tắc đặt tên mới.

---

## 30.4 File Update Policy

Nếu sửa một Module:

AI chỉ sửa:

- File liên quan.
- Documentation liên quan.

Không sửa các Module khác.

---

# ============================================================================
# 31. AI DATABASE DEVELOPMENT
# ============================================================================

## 31.1 Database Rules

AI phải:

- Đọc DATABASE_DESIGN.md.
- Đọc BUSINESS_RULES.md.
- Đọc DATABASE_RULES.md.

Sau đó mới được sinh SQL.

---

## 31.2 Database Restrictions

Không được:

- Đổi Primary Key.
- Đổi Foreign Key.
- Đổi Relationship.
- Đổi Data Type.
- Đổi Business Rule.

Nếu chưa được phê duyệt.

---

## 31.3 SQL Generation

SQL phải:

- Có Transaction khi cần.
- Có Rollback khi phù hợp.
- Có Comment cho Migration lớn.
- Tuân thủ Naming Convention.

---

# ============================================================================
# 32. AI API DEVELOPMENT
# ============================================================================

## 32.1 API Rules

Trước khi sinh API:

AI phải đọc:

- API_SPEC.md
- API Standards

---

## 32.2 API Requirements

API phải:

- RESTful.
- Có Version.
- Có Validation.
- Có Authentication.
- Có Authorization.
- Có Error Response chuẩn.

---

## 32.3 API Restrictions

Không được:

- Đổi Endpoint.
- Đổi Response Format.
- Đổi DTO.
- Đổi Authentication Flow.

Nếu chưa có yêu cầu.

---

# ============================================================================
# 33. AI BACKEND DEVELOPMENT
# ============================================================================

## 33.1 Backend Standards

Backend phải tuân thủ:

- Clean Architecture.
- Dependency Injection.
- Repository Pattern.
- Service Layer.
- CQRS (nếu áp dụng).

---

## 33.2 Business Logic

Business Logic phải:

- Độc lập.
- Có Validation.
- Có Exception Handling.
- Có Logging.

---

## 33.3 Service Rules

Service không được:

- Truy cập UI.
- Hard-code dữ liệu.
- Bỏ qua Business Rules.

---

# ============================================================================
# 34. AI FRONTEND DEVELOPMENT
# ============================================================================

## 34.1 Frontend Rules

Frontend phải tuân thủ:

- UI Standards.
- UX Standards.
- Responsive Design.
- Accessibility.
- Performance.

---

## 34.2 JavaScript Rules

Không được:

- Hard-code URL.
- Hard-code Token.
- Hard-code API Key.

---

## 34.3 UI Update Policy

Không tự ý:

- Đổi Theme.
- Đổi Layout.
- Đổi Navigation.
- Đổi Component.

Nếu chưa có yêu cầu.

---

# ============================================================================
# 35. AI REFACTORING RULES
# ============================================================================

## 35.1 Refactoring Principles

AI chỉ được Refactor khi:

- Có yêu cầu.
- Không thay đổi chức năng.
- Không thay đổi API.
- Không thay đổi Database.

---

## 35.2 Safe Refactoring

Cho phép:

- Tách Method.
- Đổi tên biến.
- Loại bỏ mã trùng lặp.
- Tối ưu cấu trúc.

---

## 35.3 Forbidden Refactoring

Không được:

- Đổi Architecture.
- Đổi Pattern.
- Đổi Business Rules.
- Đổi Database Schema.

---

# ============================================================================
# 36. AI DEBUGGING STANDARDS
# ============================================================================

## 36.1 Debug Workflow

```text
Analyze

↓

Locate

↓

Root Cause

↓

Fix

↓

Test

↓

Document
```

---

## 36.2 Root Cause Analysis

AI phải ưu tiên:

- Tìm nguyên nhân.
- Không sửa triệu chứng.
- Không sửa nhiều nơi khi chưa xác định nguyên nhân.

---

## 36.3 Debug Report

Sau khi sửa lỗi nên báo:

- Nguyên nhân.
- File thay đổi.
- Giải pháp.
- Rủi ro.
- Kiểm thử.

---

# ============================================================================
# 37. AI SECURITY DEVELOPMENT
# ============================================================================

## 37.1 Security First

Mọi Code phải tuân thủ:

- Authentication.
- Authorization.
- Input Validation.
- Output Encoding.
- Logging.
- Error Handling.

---

## 37.2 Forbidden

Không được sinh:

- Hard-code Password.
- Hard-code Secret.
- SQL Injection.
- XSS.
- CSRF.
- Unsafe Deserialization.

---

## 37.3 Dependency Rules

Ưu tiên:

- Official Library.
- Stable Version.
- Được bảo trì.

---

# ============================================================================
# 38. AI PERFORMANCE OPTIMIZATION
# ============================================================================

## 38.1 Performance Goals

Mã nguồn cần:

- Hiệu quả.
- Dễ mở rộng.
- Tối ưu tài nguyên.
- Không tối ưu quá mức gây khó bảo trì.

---

## 38.2 Optimization Rules

Ưu tiên:

- Giảm truy vấn Database.
- Giảm gọi API dư thừa.
- Tối ưu Cache.
- Tối ưu bất đồng bộ (Async/Await).

---

## 38.3 Performance Restrictions

Không được:

- Hy sinh tính đúng đắn.
- Bỏ Validation.
- Bỏ Logging quan trọng.

Chỉ để tăng tốc độ.

---

# ============================================================================
# 39. PHASE 2C CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2D cần xác nhận:

- AI Coding Standards được áp dụng.
- Code Generation Rules hoàn chỉnh.
- Database Development tuân thủ Standards.
- API Development tuân thủ Standards.
- Backend Development tuân thủ Standards.
- Frontend Development tuân thủ Standards.
- Refactoring Rules được xác định.
- Debugging Workflow được chuẩn hóa.
- Security Development được áp dụng.
- Performance Optimization tuân thủ quy định.

---

# End of Phase 2C

Phase tiếp theo:

- AI Code Review Standards
- AI Testing Standards
- AI Documentation Standards
- AI Change Management
- AI Knowledge Management
- AI Learning Workflow
- AI Release Workflow
- AI Quality Assurance
- AI Risk Management
- AI Validation Framework
````
````md
# ============================================================================
# 40. AI CODE REVIEW STANDARDS
# ============================================================================

## 40.1 Purpose

AI hỗ trợ Review Code nhằm:

- Nâng cao chất lượng mã nguồn.
- Phát hiện lỗi sớm.
- Đảm bảo tuân thủ Standards.
- Đảm bảo tính nhất quán của toàn bộ dự án.

AI Review không thay thế việc phê duyệt của Developer hoặc Project Lead.

---

## 40.2 Review Scope

Bao gồm:

- Source Code
- SQL
- API
- Frontend
- Backend
- Documentation
- Infrastructure
- CI/CD Pipeline

---

## 40.3 Review Workflow

```text
Read Bootstrap

↓

Read Standards

↓

Analyze Code

↓

Identify Issues

↓

Propose Improvements

↓

Validate

↓

Generate Review Report
```

---

## 40.4 Review Checklist

AI cần kiểm tra:

- Coding Standards
- Naming Convention
- Business Rules
- Security
- Performance
- Maintainability
- Test Coverage
- Documentation

---

# ============================================================================
# 41. AI TESTING STANDARDS
# ============================================================================

## 41.1 Testing Principles

AI phải ưu tiên:

- Test First
- Repeatable
- Automated
- Reliable

---

## 41.2 Required Tests

Nếu phù hợp với yêu cầu, AI cần sinh:

- Unit Test
- Integration Test
- API Test
- Validation Test
- Security Test
- Regression Test

---

## 41.3 Test Validation

AI phải xác nhận:

- Test chạy thành công.
- Không bỏ sót Validation.
- Bao phủ các luồng nghiệp vụ chính.
- Có Expected Result rõ ràng.

---

## 41.4 Test Report

Sau khi sinh Test cần ghi rõ:

- Phạm vi kiểm thử.
- Điều kiện kiểm thử.
- Kết quả mong đợi.
- Các trường hợp ngoại lệ.

---

# ============================================================================
# 42. AI DOCUMENTATION STANDARDS
# ============================================================================

## 42.1 Documentation First

Nếu AI tạo mới:

- API
- Database
- Module
- Service
- Deployment
- Security

thì phải cập nhật tài liệu tương ứng.

---

## 42.2 Documentation Rules

Tài liệu cần:

- Có Version.
- Có Mục đích.
- Có Phạm vi.
- Có Checklist.
- Có Change Log.

---

## 42.3 Documentation Synchronization

Không để xảy ra tình trạng:

- Code đã thay đổi nhưng tài liệu chưa cập nhật.
- API thay đổi nhưng API_SPEC.md không thay đổi.
- Database thay đổi nhưng DATABASE_DESIGN.md không thay đổi.

---

# ============================================================================
# 43. AI CHANGE MANAGEMENT
# ============================================================================

## 43.1 Purpose

Mọi thay đổi do AI tạo ra phải có khả năng kiểm soát và truy vết.

---

## 43.2 Change Categories

Bao gồm:

- New Feature
- Enhancement
- Bug Fix
- Refactoring
- Documentation
- Infrastructure

---

## 43.3 Change Rules

AI phải mô tả:

- Mục tiêu thay đổi.
- Phạm vi ảnh hưởng.
- Tài liệu cần cập nhật.
- Rủi ro.

---

## 43.4 Change Validation

Trước khi hoàn thành:

- Kiểm tra Standards.
- Kiểm tra Architecture.
- Kiểm tra Business Rules.
- Kiểm tra Documentation.

---

# ============================================================================
# 44. AI KNOWLEDGE MANAGEMENT
# ============================================================================

## 44.1 Knowledge Sources

AI chỉ sử dụng:

- Bootstrap
- Project Progress
- Project Index
- Approved Documentation
- Current Conversation

---

## 44.2 Knowledge Priority

```text
User Request

↓

Bootstrap

↓

AI Handover

↓

Standards

↓

Project Documentation

↓

General Knowledge
```

---

## 44.3 Knowledge Restrictions

Không được:

- Tự tạo Requirement.
- Tự thay đổi Business Rules.
- Tự bổ sung chức năng chưa được yêu cầu.

---

# ============================================================================
# 45. AI LEARNING WORKFLOW
# ============================================================================

## 45.1 Continuous Improvement

Sau mỗi Sprint AI cần:

- Phân tích lỗi.
- Đề xuất cải tiến.
- Đề xuất chuẩn hóa.
- Cập nhật Documentation nếu được yêu cầu.

---

## 45.2 Lessons Learned

Khuyến nghị lưu lại:

- Lỗi phổ biến.
- Giải pháp.
- Best Practices.
- Kiến thức mới áp dụng cho dự án.

---

# ============================================================================
# 46. AI RELEASE WORKFLOW
# ============================================================================

## 46.1 Before Release

AI cần xác nhận:

- Code hoàn chỉnh.
- Test đạt.
- Documentation cập nhật.
- Version đúng.
- Change Log cập nhật.

---

## 46.2 Release Checklist

- Build thành công.
- Test thành công.
- Security đạt.
- Documentation đầy đủ.
- Deployment sẵn sàng.

---

# ============================================================================
# 47. AI QUALITY ASSURANCE
# ============================================================================

## 47.1 QA Principles

AI phải đảm bảo:

- Correctness
- Consistency
- Reliability
- Maintainability
- Security

---

## 47.2 Quality Gates

Không hoàn thành nhiệm vụ nếu:

- Vi phạm Standards.
- Thiếu Documentation.
- Thiếu Validation.
- Thiếu Testing.

---

# ============================================================================
# 48. AI RISK MANAGEMENT
# ============================================================================

## 48.1 Risk Categories

Theo dõi:

- Architecture Risk
- Security Risk
- Performance Risk
- Documentation Risk
- Deployment Risk

---

## 48.2 Risk Response

Nếu phát hiện rủi ro:

- Thông báo người dùng.
- Giải thích nguyên nhân.
- Đề xuất phương án xử lý.

Không tự ý thay đổi để khắc phục.

---

# ============================================================================
# 49. AI VALIDATION FRAMEWORK
# ============================================================================

## 49.1 Validation Levels

Mỗi đầu ra của AI cần được đánh giá theo:

- Functional Validation
- Architecture Validation
- Standards Validation
- Security Validation
- Documentation Validation

---

## 49.2 Completion Criteria

Một nhiệm vụ chỉ được xem là hoàn thành khi:

- Đúng yêu cầu.
- Tuân thủ Standards.
- Không phá vỡ kiến trúc.
- Có thể bảo trì.
- Có thể mở rộng.

---

# ============================================================================
# 50. PHASE 2D CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2E cần xác nhận:

- AI Code Review hoàn chỉnh.
- AI Testing được chuẩn hóa.
- AI Documentation đồng bộ.
- AI Change Management được áp dụng.
- AI Knowledge Management rõ ràng.
- AI Learning Workflow được xác định.
- AI Release Workflow hoàn chỉnh.
- AI Quality Assurance đạt yêu cầu.
- AI Risk Management được áp dụng.
- AI Validation Framework hoàn chỉnh.

---

# End of Phase 2D

Phase tiếp theo:

- Enterprise AI Governance
- Multi-AI Collaboration Governance
- AI Security Governance
- AI Compliance Standards
- AI Ethics
- AI Audit
- AI Performance Metrics
- AI Coding Rules
- Enterprise Checklists
- Change Log
- Related Documents
- End of Document
````
```md
# ============================================================================
# 51. ENTERPRISE AI GOVERNANCE
# ============================================================================

## 51.1 Purpose

Enterprise AI Governance quy định cơ chế quản trị việc sử dụng AI trong toàn bộ vòng đời phát triển dự án AnSinhSo.

Mục tiêu:

- Đảm bảo AI hoạt động nhất quán.
- Đảm bảo AI tuân thủ Standards.
- Giảm rủi ro.
- Tăng chất lượng sản phẩm.
- Đảm bảo khả năng kiểm toán.
- Đảm bảo khả năng mở rộng.

---

## 51.2 Governance Principles

Toàn bộ AI phải tuân thủ:

- Human-in-the-Loop
- Architecture First
- Documentation First
- Security First
- Testing First
- Traceability
- Accountability
- Continuous Improvement

---

## 51.3 AI Authority

AI có quyền:

- Sinh Code.
- Sinh Documentation.
- Sinh SQL.
- Sinh Unit Test.
- Sinh API.
- Sinh Deployment Script.
- Sinh Pipeline.
- Phân tích lỗi.

AI không có quyền:

- Phê duyệt kiến trúc.
- Thay đổi Business Rules.
- Thay đổi Database.
- Thay đổi Security Policy.
- Thay đổi Standards.

---

# ============================================================================
# 52. MULTI-AI GOVERNANCE
# ============================================================================

## 52.1 Supported AI

Dự án hỗ trợ:

- ChatGPT
- Gemini
- GitHub Copilot
- Cursor
- Cline
- Continue
- AntiGravity AI

---

## 52.2 Collaboration Rules

Mọi AI phải:

- Đọc Bootstrap.
- Đọc Project Progress.
- Đọc Project Index.
- Đọc AI Handover.
- Tuân thủ toàn bộ Standards.

---

## 52.3 Shared Context

Nguồn ngữ cảnh dùng chung:

- Bootstrap v2
- Project Documentation
- Standards
- Current Sprint
- Current Conversation

Không AI nào được sử dụng ngữ cảnh trái với tài liệu chính thức của dự án.

---

# ============================================================================
# 53. AI SECURITY GOVERNANCE
# ============================================================================

## 53.1 Security Objectives

Đảm bảo AI:

- Không làm rò rỉ dữ liệu.
- Không sinh Secret thật.
- Không vi phạm Security Standards.
- Không bỏ qua Authentication.
- Không bỏ qua Authorization.

---

## 53.2 Secure Development

AI phải:

- Kiểm tra Input Validation.
- Kiểm tra Output Encoding.
- Kiểm tra Error Handling.
- Kiểm tra Logging.
- Kiểm tra Dependency.

---

## 53.3 Secret Protection

Không được:

- Hard-code Password.
- Hard-code API Key.
- Hard-code JWT Secret.
- Hard-code Connection String.
- Sinh Credential thật.

---

# ============================================================================
# 54. AI COMPLIANCE STANDARDS
# ============================================================================

## 54.1 Compliance Objectives

AI phải tuân thủ:

- Coding Standards.
- Database Rules.
- API Standards.
- Security Standards.
- Deployment Standards.
- Testing Standards.
- DevOps Standards.

---

## 54.2 Compliance Validation

Mỗi đầu ra cần được kiểm tra:

- Standards Compliance.
- Architecture Compliance.
- Documentation Compliance.
- Security Compliance.

---

# ============================================================================
# 55. AI ETHICS
# ============================================================================

## 55.1 Principles

AI cần:

- Trung thực.
- Minh bạch.
- Không bịa đặt.
- Không suy diễn.
- Không che giấu hạn chế.

---

## 55.2 Transparency

Nếu AI:

- Không chắc chắn.
- Thiếu dữ liệu.
- Thiếu tài liệu.

Phải thông báo cho người dùng.

---

## 55.3 Human Decision

Mọi quyết định cuối cùng thuộc về:

- Product Owner.
- Project Lead.
- Người dùng.

Không thuộc về AI.

---

# ============================================================================
# 56. AI AUDIT & TRACEABILITY
# ============================================================================

## 56.1 Audit Scope

Theo dõi:

- Prompt.
- Output.
- Documentation.
- Code Changes.
- SQL Changes.
- API Changes.

---

## 56.2 Traceability

Mọi thay đổi cần có:

- Nguồn yêu cầu.
- Phạm vi.
- Tài liệu liên quan.
- Change Log.

---

# ============================================================================
# 57. AI PERFORMANCE METRICS
# ============================================================================

## 57.1 KPI

Đánh giá AI theo:

- Accuracy.
- Standards Compliance.
- Documentation Quality.
- Code Quality.
- Test Coverage.
- Security Compliance.

---

## 57.2 Quality Indicators

Theo dõi:

- Tỷ lệ lỗi.
- Tỷ lệ sửa lại.
- Tỷ lệ hoàn thành.
- Tỷ lệ tuân thủ Standards.

---

# ============================================================================
# 58. ENTERPRISE AI CODING RULES
# ============================================================================

AI phải:

- Đọc Bootstrap trước khi làm việc.
- Không phá vỡ kiến trúc.
- Không thay đổi Standards.
- Không thay đổi Business Rules.
- Không tạo tài liệu trùng lặp.
- Đồng bộ Documentation khi thay đổi.
- Sinh Test khi phù hợp.
- Báo rõ phạm vi thay đổi.

---

# ============================================================================
# 59. ENTERPRISE AI CHECKLIST
# ============================================================================

Trước khi hoàn thành một nhiệm vụ, AI cần xác nhận:

□ Đã đọc Bootstrap.

□ Đã đọc Project Progress.

□ Đã đọc Project Index.

□ Đã đọc AI Handover.

□ Đã xác định Sprint.

□ Đã xác định Module.

□ Đã đọc Standards liên quan.

□ Không vi phạm Architecture.

□ Không vi phạm Business Rules.

□ Không vi phạm Security Standards.

□ Documentation đã đồng bộ.

□ Đã đánh giá rủi ro.

□ Đã xác nhận đầu ra có thể bảo trì.

---

# ============================================================================
# 60. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------------|--------------------------------|
| 1.0.0 | 2026-07-12 | Initial Enterprise AI Development Guide |

---

# ============================================================================
# 61. RELATED DOCUMENTS
# ============================================================================

## Bootstrap

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_PROJECT_INDEX.md
- 00_AI_HANDOVER.md

---

## Core Standards

- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 07_FRONTEND_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md
- 10_DEPLOYMENT_STANDARDS.md
- 11_TESTING_STANDARDS.md
- 12_DEVOPS_STANDARDS.md

---

## Core Documentation

- BUSINESS_RULES.md
- DATABASE_DESIGN.md
- API_SPEC.md
- DEPLOYMENT_GUIDE.md

---

# ============================================================================
# 62. END OF DOCUMENT
# ============================================================================

Tài liệu **13_AI_DEVELOPMENT_GUIDE.md** là tiêu chuẩn chính thức quy định cách mọi AI Coding Assistant tham gia phát triển dự án AnSinhSo.

Tất cả AI phải:

- Đọc Bootstrap trước khi làm việc.
- Tuân thủ toàn bộ Standards của dự án.
- Không thay đổi kiến trúc nếu chưa được phê duyệt.
- Không thay đổi Business Rules.
- Không tự suy diễn khi thiếu dữ liệu.
- Đồng bộ Documentation khi có thay đổi.
- Làm việc theo Sprint hiện tại.
- Hỗ trợ Developer, không thay thế vai trò phê duyệt của con người.

Mọi thay đổi đối với tài liệu này phải:

- Được đánh giá tác động.
- Được cập nhật Change Log.
- Được đồng bộ với Bootstrap và Project Progress.
- Được người quản lý dự án hoặc Product Owner phê duyệt.

---

# END OF FILE
```
