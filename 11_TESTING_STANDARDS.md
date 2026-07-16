````md
# ============================================================================
# 11_TESTING_STANDARDS.md
# ============================================================================

Project         : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
Document Type   : Enterprise Testing Standards
Version         : 1.0.0
Status          : FROZEN
Owner           : Project Architecture Team

Architecture    : Enterprise Clean Architecture
Framework        : ASP.NET Core 8
Frontend         : HTML / CSS / JavaScript
Database         : SQL Server 2022
Testing          : Enterprise Testing Framework
Last Updated     : 2026-07-12

============================================================================

# 1. PURPOSE

Tài liệu này quy định toàn bộ tiêu chuẩn kiểm thử (Testing Standards)
áp dụng cho hệ thống AnSinhSo.

Mục tiêu:

- Chuẩn hóa quy trình kiểm thử.
- Nâng cao chất lượng phần mềm.
- Phát hiện lỗi sớm.
- Giảm rủi ro khi triển khai Production.
- Hỗ trợ Continuous Testing.
- Hỗ trợ AI Coding.

---

# 2. SCOPE

Áp dụng cho toàn bộ hệ thống:

- Backend API
- Frontend
- SQL Server
- AI Services
- GIS Services
- Zalo Official Account
- Authentication
- Authorization
- Database
- Deployment
- Security

---

# 3. TESTING OBJECTIVES

Quy trình kiểm thử hướng đến các mục tiêu sau:

- Functional Correctness
- Reliability
- Security
- Performance
- Compatibility
- Maintainability
- Regression Prevention

---

## 3.1 Functional Correctness

Đảm bảo mọi chức năng hoạt động đúng theo:

- Business Rules
- Functional Specification
- User Requirements

---

## 3.2 Reliability

Hệ thống phải hoạt động ổn định trong nhiều điều kiện sử dụng.

---

## 3.3 Security

Kiểm thử nhằm phát hiện:

- Authentication lỗi
- Authorization lỗi
- Injection
- XSS
- CSRF
- Lộ dữ liệu

---

## 3.4 Performance

Đánh giá:

- Response Time
- Throughput
- Resource Usage

---

## 3.5 Compatibility

Đảm bảo hệ thống hoạt động trên:

- Chrome
- Edge
- Firefox

và các độ phân giải màn hình phổ biến.

---

## 3.6 Maintainability

Test phải:

- Dễ cập nhật
- Dễ mở rộng
- Có thể tái sử dụng

---

# 4. TESTING PRINCIPLES

Áp dụng các nguyên tắc:

- Shift Left Testing
- Early Testing
- Risk-based Testing
- Automation First
- Continuous Testing
- Traceability
- Repeatability

---

## Principle 01

Testing bắt đầu ngay từ giai đoạn thiết kế.

---

## Principle 02

Không chờ hoàn thành toàn bộ hệ thống mới kiểm thử.

---

## Principle 03

Ưu tiên kiểm thử các chức năng có rủi ro cao.

---

## Principle 04

Các bài kiểm thử nên được tự động hóa khi phù hợp.

---

## Principle 05

Mọi lỗi phải có khả năng truy vết đến:

- Requirement
- Source Code
- Test Case

---

# 5. TESTING PYRAMID

Hệ thống áp dụng mô hình:

```
          UI Test

      Integration Test

          Unit Test
```

Ưu tiên:

- Nhiều Unit Test.
- Vừa đủ Integration Test.
- Ít UI Test hơn nhưng tập trung vào luồng nghiệp vụ chính.

---

# 6. TESTING LIFECYCLE

Mỗi Sprint áp dụng quy trình:

```
Requirement

↓

Design Review

↓

Test Planning

↓

Test Case Design

↓

Development

↓

Unit Test

↓

Integration Test

↓

System Test

↓

Regression Test

↓

UAT

↓

Release
```

---

# 7. TESTING ROLES

| Vai trò | Trách nhiệm |
|----------|-------------|
| Developer | Unit Test, sửa lỗi |
| QA Engineer | Thiết kế và thực hiện Test Case |
| Solution Architect | Định nghĩa chiến lược kiểm thử |
| Project Manager | Phê duyệt kế hoạch kiểm thử |
| Product Owner | Nghiệm thu chức năng |
| AI Coding Assistant | Sinh mã và Test Case theo chuẩn |

---

# 8. QUALITY OBJECTIVES

Hệ thống hướng đến:

- Tỷ lệ Build thành công cao.
- Không có lỗi nghiêm trọng trước Production.
- Tỷ lệ Regression thấp.
- Kiểm thử đầy đủ các chức năng trọng yếu.
- Khả năng mở rộng bộ Test trong tương lai.

---

# 9. TEST ARTIFACTS

Các tài liệu kiểm thử bao gồm:

- Test Plan
- Test Strategy
- Test Case
- Test Data
- Test Report
- Defect Report
- Regression Report
- UAT Report

---

# 10. TESTING BASELINE

Mỗi chức năng trước khi chuyển sang QA phải đáp ứng:

- Build thành công.
- Không còn lỗi biên dịch.
- Code Review hoàn thành.
- Unit Test đạt yêu cầu.
- Không còn lỗi mức Critical.
- Đáp ứng Coding Standards.
- Đáp ứng Security Standards.

---

# End of Phase 1

Phase tiếp theo:

- Unit Testing
- Integration Testing
- API Testing
- UI Testing
- End-to-End Testing
- Smoke Testing
- Regression Testing
- User Acceptance Testing
````
````md
# ============================================================================
# 11. UNIT TESTING
# ============================================================================

## 11.1 Purpose

Unit Testing xác minh từng thành phần nhỏ nhất của hệ thống hoạt động chính xác.

Mỗi Unit Test phải:

- Độc lập.
- Có thể lặp lại.
- Thực thi nhanh.
- Không phụ thuộc Database hoặc dịch vụ bên ngoài.

---

## 11.2 Scope

Áp dụng cho:

- Domain Services
- Application Services
- Business Logic
- Helper Classes
- Validators
- Utility Functions

Không áp dụng trực tiếp cho:

- Database
- HTTP API
- File System
- Zalo OA
- AI Service
- GIS Service

Các thành phần này được kiểm thử ở cấp Integration hoặc End-to-End.

---

## 11.3 Unit Test Principles

Một Unit Test phải đảm bảo:

- Một mục tiêu kiểm thử.
- Một kết quả mong đợi.
- Không phụ thuộc thứ tự thực thi.
- Có thể chạy nhiều lần với cùng kết quả.

---

## 11.4 Naming Convention

Khuyến nghị:

```text
MethodName_ShouldExpectedResult_WhenCondition
```

Ví dụ:

```text
CreateHousehold_ShouldReturnSuccess_WhenDataIsValid

Login_ShouldFail_WhenPasswordIncorrect

ApproveSupport_ShouldUpdateStatus_WhenAuthorized
```

---

## 11.5 Mocking

Các phụ thuộc bên ngoài phải được Mock hoặc Stub.

Ví dụ:

- Repository
- Email Service
- SMS Service
- AI Provider
- Zalo OA API
- GIS API

---

## 11.6 Coverage Target

Khuyến nghị:

| Module | Coverage |
|---------|---------:|
| Domain | ≥ 90% |
| Application | ≥ 85% |
| Infrastructure | Theo mức độ cần thiết |

Coverage chỉ là chỉ số tham khảo, không thay thế chất lượng Test Case.

---

# ============================================================================
# 12. INTEGRATION TESTING
# ============================================================================

## 12.1 Purpose

Kiểm tra khả năng phối hợp giữa các thành phần.

---

## 12.2 Scope

Bao gồm:

- API ↔ Application
- Application ↔ Database
- Authentication
- Authorization
- Repository
- Transaction
- File Storage

---

## 12.3 Integration Rules

Không Mock các thành phần đang cần kiểm tra sự tích hợp.

Sử dụng Database riêng cho môi trường kiểm thử.

---

## 12.4 Test Scenarios

Ví dụ:

- Tạo hộ gia đình.
- Cập nhật đối tượng an sinh.
- Ghi nhận chi trả.
- Đồng bộ thông báo.

---

# ============================================================================
# 13. API TESTING
# ============================================================================

## 13.1 Purpose

Kiểm tra toàn bộ REST API.

---

## 13.2 Verification

Mỗi API phải kiểm tra:

- HTTP Method
- URL
- Status Code
- Authentication
- Authorization
- Request Validation
- Response Structure
- Error Response

---

## 13.3 HTTP Status

Ví dụ:

- 200 OK
- 201 Created
- 204 No Content
- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 409 Conflict
- 500 Internal Server Error

---

## 13.4 API Contract

API Response phải đúng theo tài liệu:

- API_SPEC.md
- Swagger/OpenAPI

---

# ============================================================================
# 14. UI TESTING
# ============================================================================

## 14.1 Purpose

Đảm bảo giao diện hoạt động đúng trên trình duyệt được hỗ trợ.

---

## 14.2 Scope

Kiểm tra:

- Navigation
- Form Validation
- Responsive Layout
- Accessibility cơ bản
- Theme
- Localization (nếu có)

---

## 14.3 Supported Browsers

- Google Chrome
- Microsoft Edge
- Mozilla Firefox

---

## 14.4 Responsive Testing

Kiểm tra các kích thước:

- Desktop
- Laptop
- Tablet
- Mobile

---

# ============================================================================
# 15. END-TO-END TESTING (E2E)
# ============================================================================

## 15.1 Purpose

Kiểm tra toàn bộ quy trình nghiệp vụ từ đầu đến cuối.

---

## 15.2 Scope

Ví dụ:

```text
Đăng nhập

↓

Tạo hộ gia đình

↓

Thêm đối tượng

↓

Phê duyệt

↓

Chi trả

↓

Thông báo Zalo

↓

Hoàn thành
```

---

## 15.3 Validation

Kiểm tra:

- Database
- API
- UI
- Logging
- Audit

---

# ============================================================================
# 16. SMOKE TESTING
# ============================================================================

## 16.1 Purpose

Xác minh nhanh hệ thống sau Build hoặc Deployment.

---

## 16.2 Smoke Checklist

Kiểm tra tối thiểu:

- Login
- Dashboard
- API
- Database
- Upload
- Search
- AI Service
- GIS Service
- Zalo OA

---

## 16.3 Execution

Smoke Test phải được thực hiện:

- Sau Build.
- Sau Deployment.
- Sau Rollback.

---

# ============================================================================
# 17. REGRESSION TESTING
# ============================================================================

## 17.1 Purpose

Đảm bảo thay đổi mới không làm hỏng chức năng cũ.

---

## 17.2 Scope

Bao gồm:

- Authentication
- Authorization
- Dashboard
- Household Management
- Welfare Management
- Payment Management
- AI
- GIS
- Zalo OA

---

## 17.3 Automation

Regression Test nên được tự động hóa khi phù hợp để giảm thời gian kiểm thử.

---

# ============================================================================
# 18. USER ACCEPTANCE TESTING (UAT)
# ============================================================================

## 18.1 Purpose

Đánh giá hệ thống từ góc nhìn người sử dụng cuối.

---

## 18.2 Participants

Có thể bao gồm:

- Đại diện UBND xã
- Cán bộ phụ trách an sinh
- Lãnh đạo
- Người quản trị hệ thống

---

## 18.3 Acceptance Criteria

Đánh giá:

- Đúng nghiệp vụ.
- Dễ sử dụng.
- Hiệu năng chấp nhận được.
- Bảo mật đáp ứng yêu cầu.
- Tài liệu đầy đủ.

---

## 18.4 UAT Result

Kết quả gồm:

- Passed
- Passed with Conditions
- Failed

---

# ============================================================================
# 19. TEST LEVEL MAPPING
# ============================================================================

| Test Level | Mục tiêu chính |
|------------|----------------|
| Unit Test | Kiểm tra từng thành phần |
| Integration Test | Kiểm tra sự tích hợp |
| API Test | Kiểm tra giao tiếp dịch vụ |
| UI Test | Kiểm tra giao diện |
| E2E Test | Kiểm tra toàn bộ quy trình |
| Smoke Test | Xác nhận nhanh sau Build/Deploy |
| Regression Test | Đảm bảo không phát sinh lỗi hồi quy |
| UAT | Nghiệm thu bởi người dùng |

---

# ============================================================================
# 20. TEST LEVEL CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2B cần xác nhận:

- Unit Test hoàn thành.
- Integration Test đạt yêu cầu.
- API Test đạt yêu cầu.
- UI Test đạt yêu cầu.
- End-to-End Test thành công.
- Smoke Test thành công.
- Regression Test không phát hiện lỗi nghiêm trọng.
- UAT được thực hiện theo kế hoạch.
- Kết quả kiểm thử được lưu trữ và có thể truy vết.

---

# End of Phase 2A

Phase tiếp theo:

- Test Planning
- Test Strategy
- Test Case Standards
- Test Data Management
- Defect Management
- Bug Severity
- Bug Priority
- Requirement Traceability Matrix (RTM)
- Test Coverage
- Test Reporting
````
````md
# ============================================================================
# 21. TEST PLANNING
# ============================================================================

## 21.1 Purpose

Test Planning xác định phạm vi, phương pháp và nguồn lực cần thiết để thực hiện kiểm thử.

Mỗi Sprint hoặc Release phải có kế hoạch kiểm thử rõ ràng trước khi bắt đầu.

---

## 21.2 Test Plan Contents

Một Test Plan tối thiểu gồm:

- Mục tiêu kiểm thử
- Phạm vi kiểm thử
- Chức năng nằm trong phạm vi
- Chức năng ngoài phạm vi
- Chiến lược kiểm thử
- Môi trường kiểm thử
- Test Data
- Lịch trình
- Nhân sự
- Tiêu chí hoàn thành
- Rủi ro
- Phương án giảm thiểu

---

## 21.3 Test Planning Principles

Kế hoạch kiểm thử phải:

- Thực tế
- Có thể đo lường
- Có thể cập nhật
- Có khả năng truy vết

---

# ============================================================================
# 22. TEST STRATEGY
# ============================================================================

## 22.1 Purpose

Test Strategy mô tả cách tiếp cận kiểm thử của toàn bộ dự án.

---

## 22.2 Strategy Components

Bao gồm:

- Unit Testing
- Integration Testing
- API Testing
- UI Testing
- E2E Testing
- Performance Testing
- Security Testing
- Regression Testing
- UAT

---

## 22.3 Risk-based Testing

Ưu tiên kiểm thử:

- Authentication
- Authorization
- Chi trả trợ cấp
- Dữ liệu đối tượng
- AI
- GIS
- Zalo OA

do có mức độ ảnh hưởng cao.

---

# ============================================================================
# 23. TEST CASE STANDARDS
# ============================================================================

## 23.1 Purpose

Test Case phải thống nhất định dạng trên toàn bộ dự án.

---

## 23.2 Test Case Structure

Mỗi Test Case gồm:

- Test ID
- Module
- Requirement ID
- Test Title
- Preconditions
- Test Steps
- Expected Result
- Actual Result
- Status
- Tester
- Test Date

---

## 23.3 Test Case Naming

Khuyến nghị:

```text
TC-Module-001

TC-AUTH-001

TC-HOUSEHOLD-015

TC-ZALO-008
```

---

## 23.4 Test Case Principles

Một Test Case chỉ nên kiểm tra một mục tiêu chính.

Không kết hợp nhiều nghiệp vụ trong cùng một Test Case.

---

# ============================================================================
# 24. TEST DATA MANAGEMENT
# ============================================================================

## 24.1 Purpose

Quản lý dữ liệu kiểm thử nhằm đảm bảo:

- Chính xác
- Có thể tái sử dụng
- Không ảnh hưởng Production

---

## 24.2 Test Data Types

Bao gồm:

- Valid Data
- Invalid Data
- Boundary Data
- Empty Data
- Large Data
- Special Characters

---

## 24.3 Test Database

Sử dụng Database riêng cho:

- Development
- Testing
- UAT

Không dùng Database Production.

---

## 24.4 Sensitive Data

Nếu sử dụng dữ liệu thật:

- Ẩn danh (Masking)
- Mã hóa (nếu cần)
- Tuân thủ chính sách bảo mật

---

# ============================================================================
# 25. DEFECT MANAGEMENT
# ============================================================================

## 25.1 Purpose

Mọi lỗi phát hiện phải được ghi nhận và theo dõi đến khi đóng.

---

## 25.2 Defect Workflow

```text
New

↓

Assigned

↓

In Progress

↓

Fixed

↓

Retest

↓

Closed
```

Nếu chưa đạt:

```text
Retest

↓

Reopened
```

---

## 25.3 Defect Information

Mỗi Defect gồm:

- Defect ID
- Module
- Description
- Steps to Reproduce
- Expected Result
- Actual Result
- Severity
- Priority
- Reporter
- Assignee
- Status

---

# ============================================================================
# 26. BUG SEVERITY
# ============================================================================

## 26.1 Severity Levels

| Severity | Ý nghĩa |
|----------|---------|
| Critical | Hệ thống không hoạt động |
| High | Chức năng chính bị lỗi |
| Medium | Chức năng phụ bị ảnh hưởng |
| Low | Lỗi nhỏ, không ảnh hưởng nghiệp vụ |
| Cosmetic | Chỉ ảnh hưởng giao diện |

---

## 26.2 Severity Principles

Severity phản ánh mức độ ảnh hưởng kỹ thuật, không phản ánh mức độ ưu tiên sửa.

---

# ============================================================================
# 27. BUG PRIORITY
# ============================================================================

## 27.1 Priority Levels

| Priority | Ý nghĩa |
|----------|---------|
| P1 | Sửa ngay |
| P2 | Sửa trong Sprint hiện tại |
| P3 | Có thể trì hoãn |
| P4 | Backlog hoặc cải tiến |

---

## 27.2 Priority Assignment

Priority được xác định dựa trên:

- Ảnh hưởng nghiệp vụ
- Người sử dụng
- Tiến độ Release

---

# ============================================================================
# 28. REQUIREMENT TRACEABILITY MATRIX (RTM)
# ============================================================================

## 28.1 Purpose

RTM giúp bảo đảm mọi yêu cầu đều được kiểm thử.

---

## 28.2 Mapping

Quan hệ truy vết:

```text
Requirement

↓

Business Rule

↓

Use Case

↓

API

↓

Source Code

↓

Test Case

↓

Bug

↓

Release
```

---

## 28.3 Requirement Coverage

Mỗi Requirement phải có tối thiểu:

- Một Test Case
- Một kết quả kiểm thử

---

# ============================================================================
# 29. TEST COVERAGE
# ============================================================================

## 29.1 Coverage Types

Theo dõi:

- Requirement Coverage
- Code Coverage
- API Coverage
- UI Coverage
- Business Rule Coverage

---

## 29.2 Coverage Targets

Khuyến nghị:

| Loại | Mục tiêu |
|------|----------|
| Requirement Coverage | 100% |
| API Coverage | ≥ 90% |
| Business Rule Coverage | 100% |
| Unit Test Coverage | Theo mục tiêu của từng module |

---

## 29.3 Coverage Review

Coverage cần được rà soát trước mỗi Release.

---

# ============================================================================
# 30. TEST REPORTING
# ============================================================================

## 30.1 Purpose

Báo cáo kiểm thử cung cấp tình trạng chất lượng hiện tại của hệ thống.

---

## 30.2 Test Report Contents

Bao gồm:

- Sprint
- Build Version
- Test Scope
- Test Cases Executed
- Passed
- Failed
- Blocked
- Defects
- Coverage
- Risks
- Recommendation

---

## 30.3 Dashboard Metrics

Theo dõi:

- Test Pass Rate
- Defect Density
- Open Defects
- Critical Bugs
- Test Execution Progress

---

# ============================================================================
# 31. TEST MANAGEMENT CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2C cần xác nhận:

- Test Plan được phê duyệt.
- Test Strategy rõ ràng.
- Test Case được chuẩn hóa.
- Test Data đầy đủ.
- Defect được quản lý.
- Severity được phân loại.
- Priority được xác định.
- RTM hoàn chỉnh.
- Coverage đạt mục tiêu.
- Test Report được cập nhật.

---

# End of Phase 2B

Phase tiếp theo:

- Test Automation Strategy
- CI Testing
- GitHub Actions Testing
- API Automation
- UI Automation
- Performance Testing
- Load Testing
- Stress Testing
- Security Testing
- Continuous Testing
- Quality Gates
````
````md
# ============================================================================
# 32. TEST AUTOMATION STRATEGY
# ============================================================================

## 32.1 Purpose

Test Automation giúp:

- Giảm thời gian kiểm thử.
- Tăng độ ổn định.
- Hỗ trợ Continuous Integration.
- Hỗ trợ Continuous Deployment.
- Giảm Regression Bugs.

---

## 32.2 Automation Principles

Ưu tiên tự động hóa:

- Unit Test
- API Test
- Integration Test
- Smoke Test
- Regression Test

UI Automation chỉ áp dụng cho các luồng nghiệp vụ quan trọng.

---

## 32.3 Automation Goals

Mục tiêu:

- Chạy tự động sau mỗi Pull Request.
- Chạy trên CI Pipeline.
- Kết quả có thể truy vết.
- Dễ bảo trì.
- Có khả năng mở rộng.

---

## 32.4 Automation Pyramid

```text
               UI Automation
            -------------------
          Integration Automation
        -------------------------
         API Automation
      ----------------------------
          Unit Automation
```

Ưu tiên số lượng:

- Nhiều Unit Test.
- Nhiều API Test.
- Ít UI Test.

---

# ============================================================================
# 33. CONTINUOUS INTEGRATION TESTING
# ============================================================================

## 33.1 Purpose

Mỗi lần Push hoặc Pull Request đều phải kích hoạt kiểm thử tự động.

---

## 33.2 CI Test Flow

```text
Developer

↓

Git Push

↓

GitHub Actions

↓

Restore

↓

Build

↓

Unit Test

↓

Integration Test

↓

API Test

↓

Static Analysis

↓

Artifact
```

---

## 33.3 CI Validation

Pipeline chỉ thành công khi:

- Build thành công.
- Test thành công.
- Không có lỗi Critical.
- Không có Security Scan thất bại.

---

# ============================================================================
# 34. GITHUB ACTIONS TESTING
# ============================================================================

## 34.1 Workflow

Pipeline tiêu chuẩn:

- Restore Packages
- Build
- Unit Test
- Integration Test
- API Test
- Static Code Analysis
- Publish Artifact

---

## 34.2 Branch Rules

Workflow tự động chạy khi:

- Pull Request
- Push vào develop
- Push vào release/*
- Push vào hotfix/*

---

## 34.3 Failure Handling

Nếu bất kỳ bước nào thất bại:

- Pipeline dừng.
- Không Publish.
- Không Deploy.

---

# ============================================================================
# 35. API AUTOMATION TESTING
# ============================================================================

## 35.1 Scope

Tự động kiểm thử:

- Authentication
- Authorization
- CRUD APIs
- Search
- Pagination
- Validation
- Error Handling

---

## 35.2 Validation

Mỗi API kiểm tra:

- Status Code
- Headers
- Response Body
- Response Time
- Security
- Schema

---

## 35.3 Automation Tools

Có thể sử dụng:

- xUnit Integration Test
- Postman Collection
- Newman
- REST Client

Lựa chọn công cụ phụ thuộc vào quy trình phát triển của dự án.

---

# ============================================================================
# 36. UI AUTOMATION TESTING
# ============================================================================

## 36.1 Scope

Tự động hóa các luồng quan trọng:

- Login
- Dashboard
- Household Management
- Welfare Management
- Payment Workflow
- User Management

---

## 36.2 UI Principles

Không tự động hóa:

- Giao diện thay đổi thường xuyên.
- Thành phần thử nghiệm tạm thời.

Ưu tiên các chức năng ổn định.

---

## 36.3 Suggested Tools

Có thể sử dụng:

- Playwright
- Selenium
- Cypress

Khuyến nghị Playwright cho các dự án ASP.NET Core hiện đại.

---

# ============================================================================
# 37. PERFORMANCE TESTING
# ============================================================================

## 37.1 Purpose

Đánh giá hiệu năng hệ thống dưới tải làm việc bình thường.

---

## 37.2 Metrics

Theo dõi:

- Response Time
- Throughput
- CPU
- Memory
- Database Connection
- Network

---

## 37.3 Performance Targets

Giá trị mục tiêu do đơn vị triển khai xác định.

Tài liệu này không quy định các ngưỡng cố định để phù hợp nhiều quy mô triển khai.

---

# ============================================================================
# 38. LOAD TESTING
# ============================================================================

## 38.1 Purpose

Đánh giá khả năng phục vụ nhiều người dùng đồng thời.

---

## 38.2 Scope

Kiểm thử:

- Login
- Dashboard
- Search
- Report
- Payment
- AI Request
- GIS Request

---

## 38.3 Suggested Tools

Có thể sử dụng:

- k6
- Apache JMeter
- NBomber

---

# ============================================================================
# 39. STRESS TESTING
# ============================================================================

## 39.1 Purpose

Xác định giới hạn chịu tải của hệ thống.

---

## 39.2 Validation

Kiểm tra:

- Khả năng phục hồi.
- Thời gian khôi phục.
- Ổn định sau khi quá tải.

---

# ============================================================================
# 40. SECURITY TESTING
# ============================================================================

## 40.1 Scope

Bao gồm:

- Authentication
- Authorization
- SQL Injection
- XSS
- CSRF
- File Upload
- JWT
- API Security

---

## 40.2 Security Validation

Đối chiếu với:

- 09_SECURITY_STANDARDS.md
- OWASP Top 10

---

## 40.3 Suggested Tools

Có thể sử dụng:

- OWASP ZAP
- Burp Suite Community Edition
- Microsoft Security Code Analysis

---

# ============================================================================
# 41. CONTINUOUS TESTING
# ============================================================================

## 41.1 Principles

Kiểm thử được thực hiện liên tục trong toàn bộ vòng đời phát triển.

---

## 41.2 Execution Points

Thực hiện:

- Sau Commit.
- Sau Pull Request.
- Sau Merge.
- Sau Build.
- Sau Deployment.

---

## 41.3 Benefits

- Phát hiện lỗi sớm.
- Giảm Regression.
- Tăng chất lượng Release.

---

# ============================================================================
# 42. QUALITY GATES
# ============================================================================

## 42.1 Purpose

Quality Gate là điều kiện bắt buộc trước khi chuyển sang giai đoạn tiếp theo.

---

## 42.2 Required Gates

Một Build chỉ được phép Release khi:

- Build thành công.
- Unit Test đạt.
- Integration Test đạt.
- API Test đạt.
- Security Scan đạt.
- Code Review hoàn thành.
- Không còn lỗi Critical.

---

## 42.3 Release Gate

Production chỉ được triển khai khi:

- Smoke Test đạt.
- Regression Test đạt.
- UAT hoàn thành.
- Deployment Checklist hoàn thành.

---

# ============================================================================
# 43. TEST AUTOMATION CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2D cần xác nhận:

- Automation Strategy được áp dụng.
- CI Testing hoạt động.
- GitHub Actions chạy thành công.
- API Automation đạt yêu cầu.
- UI Automation cho các luồng chính.
- Performance Test được thực hiện.
- Load Test hoàn thành.
- Stress Test được đánh giá.
- Security Test đạt yêu cầu.
- Continuous Testing được tích hợp.
- Quality Gates được thiết lập.

---

# End of Phase 2C

Phase tiếp theo:

- Code Review Standards
- Static Code Analysis
- Secure Code Review
- SonarQube Ready
- Definition of Ready (DoR)
- Definition of Done (DoD)
- Acceptance Criteria
- Quality Assurance Process
- Quality Gates Review
- Release Readiness
````
````md
# ============================================================================
# 44. CODE REVIEW STANDARDS
# ============================================================================

## 44.1 Purpose

Code Review là bước bắt buộc nhằm bảo đảm:

- Chất lượng mã nguồn.
- Tính nhất quán.
- Bảo mật.
- Khả năng bảo trì.
- Tuân thủ Coding Standards.

---

## 44.2 Review Scope

Code Review áp dụng cho:

- Backend
- Frontend
- Database Scripts
- API
- Infrastructure Code
- Configuration
- Migration

---

## 44.3 Review Principles

Reviewer cần kiểm tra:

- Đúng nghiệp vụ.
- Đúng kiến trúc.
- Không sinh lỗi mới.
- Không làm giảm hiệu năng.
- Không vi phạm Security Standards.

---

## 44.4 Review Checklist

Kiểm tra:

- Coding Standards
- Naming Convention
- Exception Handling
- Logging
- Validation
- Performance
- Security
- Documentation
- Test Cases

---

## 44.5 Review Outcome

Kết quả Review:

- Approved
- Approved with Comments
- Changes Requested
- Rejected

---

# ============================================================================
# 45. STATIC CODE ANALYSIS
# ============================================================================

## 45.1 Purpose

Static Code Analysis giúp phát hiện lỗi mà không cần chạy chương trình.

---

## 45.2 Analysis Scope

Kiểm tra:

- Code Smells
- Dead Code
- Duplicate Code
- Complexity
- Security Issues
- Maintainability

---

## 45.3 Static Analysis Rules

Mỗi Pull Request nên được phân tích tự động trước khi Merge.

---

## 45.4 Suggested Tools

Có thể sử dụng:

- Roslyn Analyzer
- SonarQube
- SonarCloud
- ReSharper InspectCode

---

# ============================================================================
# 46. SECURE CODE REVIEW
# ============================================================================

## 46.1 Purpose

Đảm bảo mã nguồn đáp ứng yêu cầu bảo mật trước khi phát hành.

---

## 46.2 Security Review Checklist

Kiểm tra:

- Authentication
- Authorization
- Input Validation
- Output Encoding
- SQL Injection
- XSS
- CSRF
- File Upload
- JWT Handling
- Secret Management

---

## 46.3 Sensitive Information

Không được phép:

- Hard-code Password
- Hard-code API Key
- Hard-code Connection String
- Hard-code Token
- Commit Secret lên Repository

---

# ============================================================================
# 47. SONARQUBE READY STANDARDS
# ============================================================================

## 47.1 Purpose

Mã nguồn phải sẵn sàng tích hợp với SonarQube hoặc các nền tảng phân tích tương đương.

---

## 47.2 Quality Indicators

Theo dõi:

- Bugs
- Vulnerabilities
- Code Smells
- Technical Debt
- Coverage
- Duplication

---

## 47.3 Quality Targets

Khuyến nghị:

- Không có Blocker Issues.
- Không có Critical Issues.
- Coverage đạt mục tiêu dự án.
- Technical Debt được kiểm soát.

---

# ============================================================================
# 48. ACCEPTANCE CRITERIA
# ============================================================================

## 48.1 Purpose

Acceptance Criteria xác định điều kiện để một chức năng được xem là hoàn thành.

---

## 48.2 Acceptance Principles

Mỗi User Story cần có tiêu chí:

- Rõ ràng.
- Có thể kiểm thử.
- Có thể đo lường.
- Không mơ hồ.

---

## 48.3 Typical Criteria

Ví dụ:

- Chức năng hoạt động đúng.
- Không phát sinh lỗi.
- Giao diện đúng thiết kế.
- API đúng tài liệu.
- Test Case đạt.
- Security Review đạt.

---

# ============================================================================
# 49. DEFINITION OF READY (DoR)
# ============================================================================

## 49.1 Purpose

Một công việc chỉ được đưa vào Sprint khi đáp ứng Definition of Ready.

---

## 49.2 Ready Checklist

- Requirement rõ ràng.
- Business Rule đầy đủ.
- UI (nếu có) được phê duyệt.
- API được xác định.
- Database được thiết kế.
- Estimation hoàn thành.
- Acceptance Criteria đầy đủ.

---

# ============================================================================
# 50. DEFINITION OF DONE (DoD)
# ============================================================================

## 50.1 Purpose

Definition of Done xác định điều kiện hoàn thành của một hạng mục.

---

## 50.2 Done Checklist

Một chức năng được xem là hoàn thành khi:

- Code hoàn thành.
- Code Review đạt.
- Unit Test đạt.
- Integration Test đạt.
- API Test đạt.
- Security Review đạt.
- Documentation cập nhật.
- Không còn lỗi Critical.

---

# ============================================================================
# 51. QUALITY ASSURANCE PROCESS
# ============================================================================

## 51.1 QA Workflow

```text
Requirement

↓

Development

↓

Code Review

↓

Unit Test

↓

Integration Test

↓

QA Testing

↓

Bug Fix

↓

Regression Test

↓

UAT

↓

Release
```

---

## 51.2 QA Responsibilities

QA chịu trách nhiệm:

- Thiết kế Test Case.
- Thực hiện kiểm thử.
- Ghi nhận Defect.
- Xác minh bản sửa lỗi.
- Báo cáo chất lượng.

---

# ============================================================================
# 52. QUALITY GATES REVIEW
# ============================================================================

## 52.1 Gate Review

Trước mỗi Release cần xác nhận:

- Build đạt.
- Test đạt.
- Security đạt.
- Coverage đạt.
- Documentation đầy đủ.
- Review hoàn thành.

---

## 52.2 Gate Decision

Kết quả:

- Pass
- Conditional Pass
- Fail

Nếu Fail, Release phải bị dừng cho đến khi các vấn đề được xử lý.

---

# ============================================================================
# 53. RELEASE READINESS
# ============================================================================

## 53.1 Release Validation

Trước khi phát hành cần kiểm tra:

- Build Version
- Migration
- Backup
- Rollback
- Test Report
- Release Notes

---

## 53.2 Go-Live Readiness

Chỉ triển khai Production khi:

- UAT hoàn thành.
- Smoke Test đạt.
- Monitoring hoạt động.
- Deployment Checklist hoàn tất.
- Các bên liên quan đã phê duyệt.

---

# ============================================================================
# 54. QUALITY ASSURANCE CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2E cần xác nhận:

- Code Review hoàn thành.
- Static Code Analysis đạt yêu cầu.
- Secure Code Review hoàn thành.
- SonarQube Quality Gate đạt.
- Acceptance Criteria được đáp ứng.
- Definition of Ready được tuân thủ.
- Definition of Done được xác nhận.
- QA Process hoàn thành.
- Quality Gate được phê duyệt.
- Release Readiness sẵn sàng.

---

# End of Phase 2D

Phase tiếp theo:

- AI Testing Standards
- GIS Testing Standards
- Zalo OA Testing Standards
- Database Testing
- Backup & Restore Testing
- Disaster Recovery Testing
- Production Validation
- Release Validation
- Enterprise Test Metrics
- AI Coding Rules
- Final Testing Checklist
- Change Log
- Related Documents
- End of Document
````
```md
# ============================================================================
# 55. AI TESTING STANDARDS
# ============================================================================

## 55.1 Purpose

AI là một thành phần hỗ trợ ra quyết định trong hệ thống AnSinhSo.

Việc kiểm thử AI nhằm đảm bảo:

- Kết quả ổn định.
- Phản hồi đúng ngữ cảnh.
- Không sinh thông tin nguy hiểm.
- Không vi phạm quy tắc nghiệp vụ.
- Có khả năng truy vết.

---

## 55.2 AI Testing Scope

Bao gồm:

- AI Chat Assistant
- AI Recommendation
- AI Summary
- AI Classification
- AI Prediction
- AI Prompt Processing

---

## 55.3 AI Validation

Kiểm tra:

- Accuracy
- Consistency
- Response Time
- Context Handling
- Error Handling
- Security

---

## 55.4 AI Test Dataset

Chuẩn bị bộ dữ liệu gồm:

- Normal Cases
- Boundary Cases
- Invalid Requests
- Sensitive Requests
- Empty Input
- Large Input

---

# ============================================================================
# 56. GIS TESTING STANDARDS
# ============================================================================

## 56.1 Purpose

Đảm bảo hệ thống bản đồ số hoạt động chính xác.

---

## 56.2 GIS Scope

Kiểm thử:

- Map Rendering
- Marker Display
- Layer Switching
- Polygon
- Search
- Coordinate
- Spatial Query

---

## 56.3 Validation

Kiểm tra:

- Hiển thị đúng vị trí.
- Không sai tọa độ.
- Không mất dữ liệu.
- Hiệu năng hiển thị.

---

## 56.4 GIS Integration

Đảm bảo:

- API hoạt động.
- Layer tải đúng.
- Cache hoạt động.
- Đồng bộ dữ liệu.

---

# ============================================================================
# 57. ZALO OA TESTING STANDARDS
# ============================================================================

## 57.1 Purpose

Đảm bảo tích hợp Zalo Official Account hoạt động ổn định.

---

## 57.2 Scope

Kiểm thử:

- Access Token
- Webhook
- Message Sending
- Notification
- Retry
- Error Handling

---

## 57.3 Test Cases

Ví dụ:

- Gửi thông báo thành công.
- Token hết hạn.
- Người dùng chưa theo dõi OA.
- Lỗi mạng.
- Retry thành công.

---

## 57.4 Validation

Kiểm tra:

- Success Rate
- Retry Logic
- Logging
- Audit Trail

---

# ============================================================================
# 58. DATABASE TESTING
# ============================================================================

## 58.1 Purpose

Đảm bảo dữ liệu chính xác và toàn vẹn.

---

## 58.2 Scope

Kiểm thử:

- CRUD
- Constraints
- Foreign Keys
- Transactions
- Stored Procedures
- Views
- Indexes

---

## 58.3 Validation

Kiểm tra:

- Data Integrity
- Concurrency
- Rollback
- Performance

---

## 58.4 Migration Testing

Sau mỗi Migration cần xác minh:

- Schema đúng.
- Dữ liệu không mất.
- Chỉ mục hoạt động.
- Ứng dụng tương thích.

---

# ============================================================================
# 59. BACKUP & RESTORE TESTING
# ============================================================================

## 59.1 Purpose

Đảm bảo bản sao lưu có thể sử dụng khi cần.

---

## 59.2 Test Scope

Bao gồm:

- Full Backup
- Incremental Backup
- Restore Database
- Restore Files
- Restore Configuration

---

## 59.3 Validation

Kiểm tra:

- Backup thành công.
- Restore thành công.
- Không mất dữ liệu.
- Hệ thống hoạt động bình thường sau khôi phục.

---

# ============================================================================
# 60. DISASTER RECOVERY TESTING
# ============================================================================

## 60.1 Purpose

Đánh giá khả năng phục hồi sau sự cố.

---

## 60.2 Test Scenarios

Ví dụ:

- Server Failure
- Database Failure
- Disk Failure
- Network Failure
- Application Failure

---

## 60.3 Recovery Validation

Đánh giá:

- RTO
- RPO
- Data Integrity
- Service Availability

---

# ============================================================================
# 61. PRODUCTION VALIDATION
# ============================================================================

## 61.1 Production Smoke Test

Sau khi Go-Live kiểm tra:

- Login
- Dashboard
- Household Management
- Welfare Management
- Payment
- AI
- GIS
- Zalo OA

---

## 61.2 Health Validation

Kiểm tra:

- API Health
- Database Health
- Background Jobs
- Monitoring
- Logging

---

# ============================================================================
# 62. RELEASE VALIDATION
# ============================================================================

## 62.1 Release Verification

Đối chiếu:

- Requirement
- Test Result
- Release Notes
- Version
- Deployment Report

---

## 62.2 Final Acceptance

Release chỉ được chấp nhận khi:

- Không còn lỗi Critical.
- UAT đạt.
- QA phê duyệt.
- Product Owner phê duyệt.

---

# ============================================================================
# 63. ENTERPRISE TEST METRICS
# ============================================================================

## 63.1 Quality Metrics

Theo dõi:

- Test Pass Rate
- Test Coverage
- Defect Density
- Regression Rate
- Automation Coverage

---

## 63.2 Performance Metrics

Theo dõi:

- Response Time
- Throughput
- Error Rate
- Availability
- MTTR
- MTBF

---

## 63.3 Security Metrics

Theo dõi:

- Security Defects
- Critical Vulnerabilities
- Authentication Failures
- Authorization Failures

---

# ============================================================================
# 64. AI CODING RULES (TESTING)
# ============================================================================

Mọi AI Coding Assistant phải:

- Tuân thủ 11_TESTING_STANDARDS.md.
- Sinh Unit Test cùng với Business Logic.
- Sinh Integration Test cho API.
- Không bỏ qua Validation Test.
- Không bỏ qua Security Test.
- Không sinh Test Case trùng lặp.
- Không tạo dữ liệu kiểm thử không hợp lệ nếu không có mục đích rõ ràng.
- Luôn sinh Test Case có Expected Result.
- Tuân thủ Definition of Done.

Áp dụng cho:

- ChatGPT
- Gemini
- GitHub Copilot
- Cursor
- Cline
- Continue
- AntiGravity AI

---

# ============================================================================
# 65. FINAL TESTING CHECKLIST
# ============================================================================

Trước khi Release cần xác nhận:

- Unit Test đạt.
- Integration Test đạt.
- API Test đạt.
- UI Test đạt.
- End-to-End Test đạt.
- Smoke Test đạt.
- Regression Test đạt.
- Security Test đạt.
- Performance Test hoàn thành.
- Load Test hoàn thành.
- AI Test đạt.
- GIS Test đạt.
- Zalo OA Test đạt.
- Database Test đạt.
- Backup & Restore Test đạt.
- Disaster Recovery Test hoàn thành.
- UAT được phê duyệt.
- QA phê duyệt.
- Product Owner phê duyệt.

---

# ============================================================================
# 66. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------------|--------------------------------|
| 1.0.0 | 2026-07-12 | Initial Enterprise Testing Standards |

---

# ============================================================================
# 67. RELATED DOCUMENTS
# ============================================================================

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_PROJECT_INDEX.md
- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 07_FRONTEND_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md
- 10_DEPLOYMENT_STANDARDS.md
- 12_DEVOPS_STANDARDS.md
- TEST_PLAN.md
- TEST_CASE_TEMPLATE.md
- API_SPEC.md

---

# ============================================================================
# END OF DOCUMENT
# ============================================================================
```
