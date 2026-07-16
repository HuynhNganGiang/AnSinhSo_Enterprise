# 04_CODING_STANDARDS.md

| Item | Value |
|------|-------|
| Project | AnSinhSo Enterprise |
| Document Type | AI Operating System |
| Version | 1.0.0 |
| Status | FROZEN |
| Owner | Huỳnh Ngân Giang |
| Architecture | Clean Architecture |
| Target Framework | .NET 8 |
| Database | SQL Server 2022 |
| Last Updated | 2026-07-10 |

---

# 1. Purpose

Tài liệu này định nghĩa toàn bộ tiêu chuẩn lập trình (Coding Standards) bắt buộc áp dụng cho dự án AnSinhSo Enterprise.

Mục tiêu:

- Đồng nhất mã nguồn.
- Dễ bảo trì.
- Dễ mở rộng.
- Tăng tính bảo mật.
- Tăng hiệu năng.
- Giảm Technical Debt.
- Đảm bảo AI và lập trình viên luôn sinh mã theo cùng một tiêu chuẩn.

Mọi mã nguồn mới phải tuân thủ tài liệu này.

---

# 2. Core Principles

Toàn bộ dự án phải tuân thủ các nguyên tắc sau:

- SOLID
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple, Stupid)
- YAGNI (You Aren't Gonna Need It)
- Separation of Concerns
- Clean Architecture
- Dependency Injection
- Repository Pattern
- Unit of Work (khi nghiệp vụ yêu cầu)
- Async/Await
- Fail Fast
- Secure by Default

---

# 3. Project Structure

```

src/

Domain/

Application/

Infrastructure/

API/

tests/

docs/

database/

scripts/

deployment/

```

Không được đặt Business Logic ngoài tầng Application.

Không được truy cập Database trực tiếp từ Controller.

---

# 4. Naming Convention

## 4.1 Solution

AnSinhSo.sln

## 4.2 Project

AnSinhSo.Domain

AnSinhSo.Application

AnSinhSo.Infrastructure

AnSinhSo.API

AnSinhSo.Tests

## 4.3 Class

PascalCase

Ví dụ

PersonService

PaymentRepository

DashboardController

## 4.4 Interface

Tiền tố I

IPersonService

IUserRepository

## 4.5 Method

PascalCase

GetPersonAsync()

CreatePaymentAsync()

## 4.6 Variable

camelCase

person

currentUser

paymentAmount

## 4.7 Constant

UPPER_CASE

MAX_RETRY

DEFAULT_TIMEOUT

---

# 5. Folder Convention

Không được tạo folder theo chức năng ngẫu nhiên.

Ví dụ chuẩn:

```

Application/

DTOs/

Interfaces/

Validators/

Services/

Mappings/

Features/

```

Infrastructure

```

Repositories/

Security/

Persistence/

Configurations/

ExternalServices/

```

API

```

Controllers/

Middlewares/

Filters/

Extensions/

Configurations/

```

---

# 6. C# Coding Rules

## Bắt buộc

Nullable Reference Types

Implicit Usings

File Scoped Namespace

Expression Body khi phù hợp

Async/Await

CancellationToken

ConfigureAwait(false) cho Library

Không dùng var khi gây khó đọc.

---

# 7. Entity Rules

Mọi Entity

Phải kế thừa BaseEntity

Ví dụ

```

Person : BaseEntity

```

Không viết Business Logic trong Entity.

Không viết Validation trong Entity.

---

# 8. DTO Rules

Không trả Entity trực tiếp ra API.

API

↓

DTO

↓

JSON

Không expose:

Password

Hash

Access Token

Secret

AppSecret

Refresh Token

Internal Id nếu không cần.

---

# 9. Repository Rules

Repository chỉ thao tác dữ liệu.

Không chứa Business Logic.

Bắt buộc

Async

Pagination

Filter

Search

Sorting

NoTracking cho dữ liệu chỉ đọc

CancellationToken

---

# 10. Service Rules

Business Logic chỉ nằm ở Service.

Controller không xử lý nghiệp vụ.

Service không được gọi HttpContext.

---

# 11. Controller Rules

Controller chỉ:

Receive Request

Validate Model

Call Service

Return Response

Không được:

Truy vấn SQL

Business Logic

Tính toán nghiệp vụ

---

# 12. API Response Standard

Mọi API phải trả về cùng định dạng:

```json
{
  "success": true,
  "message": "",
  "data": {},
  "errors": []
}
```

Không trả dữ liệu không có cấu trúc.

---

# 13. Exception Handling

Chỉ sử dụng Global Exception Middleware.

Không dùng try/catch tràn lan.

Business Exception

↓

Custom Exception

↓

Middleware

↓

JSON Response

---

# 14. Logging

Sử dụng Serilog.

Bắt buộc log:

Authentication

Authorization

Create

Update

Delete

Import

Export

Exception

Performance

Không log:

Password

Token

Secret

CCCD đầy đủ

Số điện thoại đầy đủ

---

# 15. SQL Standards

Không dùng

SELECT *

Luôn chỉ định tên cột.

Mọi Stored Procedure:

BEGIN TRY

BEGIN TRANSACTION

...

COMMIT

END TRY

BEGIN CATCH

ROLLBACK

THROW

END CATCH

---

# 16. Performance Standards

Async toàn bộ I/O.

Có phân trang.

Có Index.

Không N+1 Query.

Không Query dư.

Không Include không cần thiết.

Ưu tiên Projection.

---

# 17. Security Standards

JWT

RBAC

Permission

HTTPS

Input Validation

Output Encoding

Soft Delete

Audit Log

Data Masking

Encryption

Không Hard Delete.

---

# 18. Documentation Standards

Mọi API

↓

Swagger

Mọi Module

↓

Markdown

Mọi thay đổi lớn

↓

Change Log

---

# 19. Git Standards

Branch

main

develop

feature/*

bugfix/*

hotfix/*

Commit Message

feat:

fix:

refactor:

docs:

test:

perf:

chore:

Ví dụ

feat(person): thêm API tạo công dân

---

# 20. Code Review Checklist

- Kiến trúc đúng Clean Architecture.
- Không duplicate code.
- Đã có Validation.
- Đã có Authorization.
- Đã có Logging.
- Đã có Exception Handling.
- Đã có Unit Test (nếu áp dụng).
- Đã có Swagger.
- Đã có XML Comment (nếu cần).
- Không Hardcode.
- Không lộ dữ liệu nhạy cảm.
- Đúng Naming Convention.
- Đúng Coding Standards.

---

# 21. AI Mandatory Rules

Mọi AI (ChatGPT, AntiGravity AI, Gemini, Claude, Cursor...)

Phải:

- Đọc PROJECT_CONTEXT.md.
- Đọc AI_MEMORY.md.
- Đọc AI_SYSTEM_RULES.md.
- Tuân thủ tài liệu này trước khi sinh mã nguồn.

Không được sinh mã trái với tài liệu này.

---

# 22. Definition of Compliance

Một đoạn mã chỉ được xem là đạt chuẩn khi:

- Biên dịch thành công.
- Tuân thủ Coding Standards.
- Không vi phạm Security Rules.
- Không vi phạm Database Rules.
- Không vi phạm API Standards.
- Không vi phạm AI System Rules.

---

# Change Log

| Version | Date | Author | Description |
|----------|------|--------|-------------|
| 1.0.0 | 2026-07-10 | Huỳnh Ngân Giang | Khởi tạo tiêu chuẩn lập trình Enterprise cho dự án AnSinhSo. |