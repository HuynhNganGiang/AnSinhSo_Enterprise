# 45_SPRINT_03_IMPLEMENTATION_GUIDELINES.md
## SPRINT 03 – IMPLEMENTATION GUIDELINES
**Version:** 1.0.0
**Status:** Frozen
**Project:** AnSinhSo Enterprise
**Last Updated:** 2026-07-18

**Architecture State:** FROZEN
**Frozen Date:** 2026-07-18
**Frozen By:** Project Owner

---

# 1. Document Metadata
- **Document ID:** 45_SPRINT_03_IMPLEMENTATION_GUIDELINES
- **Title:** Enterprise Implementation & Coding Guidelines
- **Phase:** Sprint 03 (Bridge to Sprint 04)
- **Owner:** Principal Solution Architect
- **Audience:** AI Coding Agent, Lead Developers, Backend Engineers, QA

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Principal Architect | Initial Draft - Comprehensive Implementation Strategy bridging Architecture to Code |
| 1.0.1 | 2026-07-17 | Principal Architect | Expanded Coding Standards, Git, CI/CD, Metrics, Security, Logging, Migration, and Readiness. |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Workflow, Coding Order hay Dependency Rules đã định nghĩa.
- **KHÔNG ĐƯỢC** bỏ qua bất kỳ bước nào trong quy trình Code Review và Test.
- Mọi sự thay đổi về công cụ, quy trình hay thứ tự code bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là "Chiếc cầu nối" (Bridge) giữa Đặc tả Kiến trúc (Sprint 03) và Triển khai Mã nguồn (Sprint 04). Mục đích là cung cấp một bản đồ định hướng (Implementation Strategy) chi tiết đến từng bước cho Developer và AI Coding Agent biết chính xác phải code cái gì trước, cái gì sau, và code như thế nào để không phá vỡ Clean Architecture và DDD.

---

# 5. Scope
**In Scope:**
- Định nghĩa chiến lược triển khai: Workflow, Order, Structure.
- Cung cấp Guidelines cho 4 tầng: Domain, Application, Infrastructure, Presentation.
- Thiết lập quy tắc Testing, Review, Naming, và AI Prompting.

**Out of Scope:**
- KHÔNG sinh mã nguồn C# thực tế (Không có class, interface, SQL).
- KHÔNG mô tả logic hay thuật toán nghiệp vụ (Business Rules).

---

# 6. Objectives
- **Standardized Output:** Đảm bảo dù AI hay con người code, output sinh ra đều đồng nhất 100% về phong cách.
- **Error Prevention:** Chặn đứng việc code sai thứ tự (Ví dụ: Code Database trước khi code Domain).
- **Efficiency:** Giúp quá trình Pair-Programming giữa Dev và AI đạt tốc độ tối đa nhờ Context Prompt rõ ràng.

---

# 7. Sprint 04 Overview
- **Why:** Sprint 04 là giai đoạn chuyển hóa tài liệu thành mã nguồn thực thi.
- **What:** Khởi tạo Solution, thiết lập Projects, cài đặt Base Classes, và code các Domain Entities đầu tiên.
- **How:** Dựa vào thiết kế từ Sprint 02 và các quy tắc từ Sprint 03 để tiến hành gõ code.
- **Expected Result:** Một Solution .NET 8 có thể biên dịch (build success) với toàn bộ tầng Domain Layer được che phủ bởi Unit Test.

---

# 8. Implementation Philosophy
- **Why:** Một triết lý rõ ràng giúp giải quyết các tranh cãi kỹ thuật (Technical Disputes).
- **What:** Triết lý "Domain First, Database Last".
- **How:** Mọi đoạn code bắt buộc phải bắt đầu từ `Entities` và `Value Objects`. Giao diện (UI) và CSDL (DB) bị cấm xuất hiện cho đến khi Domain đã hoàn tất và pass test.
- **Expected Result:** Kiến trúc Clean Architecture vững chãi, không bị dính chặt vào EF Core.

---

# 9. Development Order
- **Why:** Làm lung tung sẽ gây lỗi Circular Dependency và đập đi viết lại.
- **What:** Trình tự phát triển toàn cục của một Feature.
- **How:** 
  1. Viết Unit Test cho Domain (TDD).
  2. Implement Domain.
  3. Implement Application (CQRS/MediatR).
  4. Implement Infrastructure (EF Core).
  5. Implement API (Endpoints).
- **Expected Result:** Feature hoạt động mượt mà từ lõi ra vỏ.

---

# 10. Layer Implementation Sequence
- **Why:** Đảm bảo tuân thủ "Dependency Rule" của Clean Architecture.
- **What:** Trình tự code các Layer trong Solution.
- **How:** 
  - `AnSinhSo.Domain` (Bắt đầu ở đây)
  - `AnSinhSo.Application` (Phụ thuộc Domain)
  - `AnSinhSo.Infrastructure` (Phụ thuộc Application)
  - `AnSinhSo.Api` (Tham chiếu mọi thứ để chạy)
- **Expected Result:** Không có Layer vòng (Circular Reference).

---

# 11. Project Dependency Order
- **Why:** Tránh việc DLL của Infrastructure bị rò rỉ vào DLL của Domain.
- **What:** Cây phụ thuộc của `.csproj`.
- **How:** 
  - `Domain.csproj` -> Trống (Chỉ có BCL).
  - `Application.csproj` -> Reference `Domain.csproj`.
  - `Infrastructure.csproj` -> Reference `Application.csproj`.
  - `Api.csproj` -> Reference `Infrastructure.csproj`, `Application.csproj`.
- **Expected Result:** Biên dịch thành công với luồng tham chiếu 1 chiều.

---

# 12. Folder Creation Order
- **Why:** Đảm bảo cấu trúc dự án đồng nhất, dễ tìm file.
- **What:** Trình tự tạo thư mục vật lý trước khi tạo class.
- **How:**
  1. `/SeedWork` (Các abstract/base).
  2. `/Modules` (Chứa các thư mục nghiệp vụ như `Demographic`).
  3. `/Modules/Demographic/Entities`
  4. `/Modules/Demographic/ValueObjects`
- **Expected Result:** Solution tree gọn gàng ngay từ Day 1.

---

# 13. Domain Implementation Guidelines
- **Why:** Domain là trái tim, không được phép sai sót.
- **What:** Quy tắc gõ code tầng Domain.
- **How:** Private setters cho toàn bộ Properties. Constructors không được public. Chỉ dùng Factory Methods (`Create`). Không dùng chuỗi/số cho các khái niệm có ý nghĩa (Dùng Value Object). Trả về `Result Pattern`.
- **Expected Result:** Aggregate Roots bất khả xâm phạm.

---

# 14. Application Layer Guidelines
- **Why:** Nơi điều phối luồng người dùng (Use Cases).
- **What:** CQRS và MediatR Pipeline.
- **How:** Tách biệt 100% Command (Thay đổi trạng thái) và Query (Đọc dữ liệu). Command Handlers chỉ lấy Aggregate, gọi hàm thay đổi, lưu lại. Mọi Handler trả về `Result<T>`.
- **Expected Result:** Controller/API trở nên cực mỏng (Thin Controller).

---

# 15. Infrastructure Layer Guidelines
- **Why:** Nơi giao tiếp với thế giới bên ngoài (DB, Network).
- **What:** EF Core, HttpClient, SMTP.
- **How:** Cài đặt các Interface của Application. Sử dụng Entity Framework Core Fluent API (IEntityTypeConfiguration). Không dùng Data Annotations trên Entity.
- **Expected Result:** Hạ tầng có thể thay thế (Ví dụ đổi từ SQL Server sang PostgreSQL) mà không sửa một dòng code Domain nào.

---

# 16. Presentation Layer Guidelines
- **Why:** Điểm chạm của Client (Web/Mobile).
- **What:** Minimal APIs hoặc MVC Controllers.
- **How:** Controller không chứa logic. Chỉ mapping `Result` từ MediatR thành `Http Status Code` (Ví dụ `Result.IsFailure` -> `400 BadRequest`).
- **Expected Result:** API Controller trung bình dưới 10 dòng code/hàm.

---

# 17. API Implementation Guidelines
- **Why:** Đảm bảo Restful và bảo mật.
- **What:** Giao diện HTTP.
- **How:** Bắt buộc có Versioning (v1, v2). Có Rate Limiting. Bọc phản hồi (Response Wrapper) đồng nhất.
- **Expected Result:** API Contract chuyên nghiệp, dễ dàng tích hợp.

---

# 18. Dependency Injection Guidelines
- **Why:** Tránh Tight Coupling (Dính chặt).
- **What:** IoC Container của .NET.
- **How:** Không gộp chung vào `Program.cs`. Phải tạo các `DependencyInjection.cs` static class tại từng Layer (VD: `AddDomain()`, `AddInfrastructure()`).
- **Expected Result:** Setup gọn gàng, tự quản lý theo Layer.

---

# 19. Configuration Guidelines
- **Why:** Bảo mật thông tin (Secrets).
- **What:** `appsettings.json`, Environment Variables.
- **How:** Sử dụng `IOptions<T>` Pattern. Không bao giờ inject thẳng `IConfiguration` vào Service. Không lưu Hard-coded string.
- **Expected Result:** Safe Runtime Configuration.

---

# 20. Testing Strategy
- **Why:** Code không có Test là Legacy Code.
- **What:** Mô hình Tháp Kiểm Thử (Test Pyramid).
- **How:** 70% Unit Test (cho Domain/Application), 20% Integration Test (cho Infrastructure), 10% E2E Test (cho API).
- **Expected Result:** CI/CD Pipeline xanh mượt, tự tự Deploy.

---

# 21. Unit Test Guidelines
- **Why:** Kiểm chứng Logic tĩnh.
- **What:** xUnit, FluentAssertions, NSubstitute.
- **How:** Đặt tên test theo chuẩn `MethodName_StateUnderTest_ExpectedBehavior`. AAA (Arrange - Act - Assert). Không đụng Database.
- **Expected Result:** Test chạy trong micro-seconds.

---

# 22. Integration Test Guidelines
- **Why:** Kiểm chứng liên kết DB/API.
- **What:** Testcontainers (Docker).
- **How:** Spin-up Docker SQL Server, chạy Migration, test Repo/API, Tear-down Docker.
- **Expected Result:** Đảm bảo Query EF Core không bị lỗi cú pháp SQL khi runtime.

---

# 23. Code Review Checklist
- **Why:** Bảo vệ Master Branch.
- **What:** Danh sách rà soát tĩnh.
- **How:**
  - Có trả về Result Pattern không?
  - Có leak DBContext vào Application không?
  - Có dùng Magic String không?
  - Code Coverage > 80% chưa?
- **Expected Result:** Mã nguồn sạch sẽ trước khi Merge.

---

# 24. AI Coding Constraints
- **Why:** Hạn chế AI tự sáng tác sai cấu trúc (AI Hallucinations).
- **What:** Lệnh cấm bắt buộc đối với AI khi sinh code.
- **How:** 
  - AI KHÔNG được sinh Constructor public cho Entity. 
  - AI KHÔNG được dùng AutoMapper trong Domain. 
  - AI KHÔNG được sáng tạo kiến trúc ngoài luồng định sẵn.
  - AI PHẢI tuân thủ toàn bộ hệ thống tài liệu Sprint 03.
  - AI KHÔNG được bỏ qua Result Pattern khi thiết kế trả về.
  - AI KHÔNG được bỏ qua Error Catalog thay vì gõ text lỗi.
  - AI KHÔNG được bỏ qua Notification Pattern để Validate.
- **Expected Result:** AI Coding Agent hoạt động như một Senior Developer bị ràng buộc bởi luật Enterprise nghiêm ngặt.

---

# 25. AI Prompting Guidelines
- **Why:** Dev cần biết cách giao tiếp với AI.
- **What:** Cú pháp nhắc lệnh (Prompting).
- **How:** "Hãy đóng vai Senior .NET Dev. Implement class `Citizen` dựa trên tài liệu 27 và 45. Bắt buộc dùng Result Pattern và ErrorCatalog".
- **Expected Result:** Code sinh ra xài được ngay trong 1 hit (Zero-shot or Few-shot).

---

# 26. Naming Convention
- **Why:** Mã nguồn như một cuốn sách.
- **What:** Quy tắc đặt tên.
- **How:**
  - Class/Method: `PascalCase`
  - Private Field: `_camelCase`
  - Interface: Prefix `I` (`IRepository`)
  - Command: Suffix `Command` (`CreateCitizenCommand`)
  - Handler: Suffix `Handler`
- **Expected Result:** Tính thống nhất cực cao.

---

# 27. File Organization Rules
- **Why:** 1 File = 1 Trách nhiệm.
- **What:** Quy tắc lưu trữ vật lý.
- **How:** 1 Class duy nhất trên 1 file `.cs`. Tên file trùng tên class. Không gom nhiều class vào 1 file (Trừ các class cấu trúc Error rất nhỏ có liên quan mật thiết).
- **Expected Result:** Git diff dễ dàng theo dõi.

---

# 28. Coding Standards
- **Why:** Khóa chặt chất lượng mã nguồn ở cấp độ trình biên dịch.
- **What:** Tiêu chuẩn Code .NET 8.
- **How:**
  - Sử dụng file `.editorconfig` ở root.
  - Bật cờ `Nullable Enable` cho toàn solution.
  - Bật `Implicit Usings`.
  - Cấu hình `TreatWarningsAsErrors = true`.
  - Bất kỳ Warning nào cũng khiến Build Failure.
- **Expected Result:** Mã nguồn an toàn 100% trước NullReference.

---

# 29. Git Workflow
- **Why:** Đảm bảo luồng Code được kiểm soát chặt chẽ.
- **What:** Flow quản lý phiên bản.
- **How:**
  - Mọi thay đổi bắt đầu từ `Feature Branch`.
  - Yêu cầu `Pull Request` (PR) để gộp code.
  - Phải có `Code Review` (Tối thiểu 1 Approve).
  - `Merge Strategy`: Squash and merge.
  - `Branch Protection`: Khóa Push thẳng lên nhánh `main`/`develop`.
  - `Conventional Commit`: Sử dụng chuẩn (feat:, fix:, docs:).
- **Expected Result:** Lịch sử Source Control rõ ràng, dễ Rollback.

---

# 30. CI/CD Guidelines
- **Why:** Tự động hóa kiểm soát rào chắn.
- **What:** Continuous Integration & Deployment.
- **How:**
  - **Build Pipeline**: Tự động Build mỗi khi Push.
  - **Test Pipeline**: Tự động Run mọi Unit Tests.
  - **Code Coverage**: Bắt buộc > 80%.
  - **Static Analysis**: Tích hợp SonarQube Ready.
  - **Security Scan**: Quét nuget vulnerabilities.
- **Expected Result:** Mọi dòng code thối đều bị chặn trước khi merge.

---

# 31. Code Metrics
- **Why:** Ngăn chặn các hàm và class khổng lồ.
- **What:** Số đo độ phức tạp.
- **How:**
  - `Cyclomatic Complexity` < 10 mỗi method.
  - `Method Length` < 50 dòng code.
  - `Class Size` < 500 dòng code.
  - `File Size` < 600 dòng code.
  - `Maintainability Index`: Mức Green (> 60).
- **Expected Result:** Hệ thống không biến thành bãi rác không thể bảo trì.

---

# 32. Security Coding Guidelines
- **Why:** Chống lại các cuộc tấn công mạng.
- **What:** Thực hành Code An Toàn.
- **How:**
  - **Secret Management**: Không Hard-code passwords. Dùng Azure KeyVault.
  - **Input Validation**: Tự động chạy Validation behavior trước khi vào Domain.
  - **SQL Injection Prevention**: Trăm phần trăm dùng Parameterized EF Core.
  - **XSS / CSRF**: Luôn Escape đầu ra.
  - **Authentication / Authorization**: Sử dụng Claims-based hoặc Policy-based, cấm Hard-code Role strings.
- **Expected Result:** Hệ thống đạt chuẩn Enterprise Security.

---

# 33. Logging Guidelines
- **Why:** Để có thể phân tích sự cố (Traceability).
- **What:** Chiến lược ghi log.
- **How:**
  - **No Domain Logging**: Domain KHÔNG biết `ILogger` là gì.
  - **Layer Logging**: Log chỉ nằm ở Application/Infrastructure.
  - **CorrelationId**: Đính kèm CorrelationId xuyên suốt các request.
  - **Structured Logging**: Ghi log chuẩn JSON.
- **Expected Result:** Debugging production nhanh và chính xác.

---

# 34. Migration Guidelines
- **Why:** Nâng cấp CSDL an toàn.
- **What:** Database Evolution.
- **How:**
  - **EF Core Migration Strategy**: Thay đổi cấu trúc Model phải tạo code-first migration.
  - **Database Versioning**: Theo dõi phiên bản DB gắn với Application version.
  - **Rollback Strategy**: Luôn có phương án Down-migration.
- **Expected Result:** Zero-downtime database deployment.

---

# 35. Sprint 04 Entry Checklist
- **Why:** Tránh việc "chạy vội" khi mọi thứ chưa đóng băng.
- **What:** Rà soát bắt buộc trước khi Dev bắt đầu viết code Sprint 04.
- **How:**
  - [x] Domain Freeze (Đóng băng đặc tả nghiệp vụ).
  - [x] Architecture Freeze (Đóng băng tài liệu Kiến trúc 31-45).
  - [x] Review Completed (Ký duyệt xong mọi báo cáo).
  - [x] Unit Test Strategy Approved.
  - [x] Coding Standards Approved.
  - [x] Git Strategy Approved.
  - [x] CI/CD Ready.
- **Expected Result:** Nền tảng vững chắc, không đổi spec giữa chừng.

---

# 36. Definition Of Ready
- **Why:** Rào chắn chính thức của Sprint Planning.
- **What:** Điều kiện Bắt Đầu Sprint 04.
- **How:** Đội ngũ phát triển (hoặc AI Agent) chỉ được phép tạo Solution và gõ những dòng C# đầu tiên khi **toàn bộ 15 tài liệu Sprint 03** đã được Project Owner xác nhận "Freeze". Mọi vi phạm nguyên tắc Result Pattern hay Error Catalog phải bị reject ngay lập tức tại vòng CI.
- **Expected Result:** Sprint 04 chỉ tập trung vào Logic Code, không tranh cãi Architecture.

---

# 37. Architecture Validation Matrix
| Tiêu chí Implementation | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **Domain Purity Check** | Tuyệt đối không `using Microsoft.EntityFrameworkCore` trong Domain. | Bắt buộc (Mandatory) |
| **Result Pattern Enforced**| Mọi hàm nghiệp vụ không trả về `void` hay ném Exception. | Bắt buộc (Mandatory) |
| **Encapsulation Check** | Entity có `private set`, khởi tạo qua `Factory Method`. | Bắt buộc (Mandatory) |
| **Testing Coverage** | Mọi Domain Logic phải có Unit Test. | Bắt buộc (Mandatory) |
| **Strict CI/CD** | Cấm Merge code nếu chưa pass Test và Static Scan. | Bắt buộc (Mandatory) |

---

# 38. Architectural Risks
- **Why:** Cảnh báo trước các điểm rơi thất bại.
- **What:** Lỗi thiết kế triển khai.
- **How:** Rủi ro "Tạo vòng lặp tham chiếu" (Circular Dependencies) nếu chia Project quá vụn. Rủi ro "Mập mờ logic" nếu không kiểm soát chặt sự khác biệt giữa Domain Service và Application Service.
- **Expected Result:** Nâng cao nhận thức phòng thủ kiến trúc.

---

# 39. Architectural Anti Patterns
Phải đặc biệt cảnh giác với các thói quen xấu khi gõ code (Coding Anti-Patterns):
1. **The Extension Method Abuse:** Lạm dụng Extension Method để nhét logic cấu trúc vào mọi nơi.
2. **The Fat Controller:** Code validate và truy vấn thẳng trong `[HttpPost]`.
3. **The Data-Bag Entity:** Khởi tạo `new Citizen() { Id = 1, Name = "A" }` không qua Factory.
4. **The Exception for Flow:** Dùng `try { repo.Find() } catch (NotFoundException)`.
5. **The Magic String Query:** `_dbContext.Set<Citizen>().FromSqlRaw("SELECT * ...")` bừa bãi.

---

# 40. Definition Of Done
Tài liệu Implementation Guidelines được hoàn tất khi:
- Bao quát toàn bộ quy trình từ tạo Folder, chọn Dependency, gõ Code, Test đến Review.
- Cung cấp định hướng cực kỳ sắc bén để chế ngự AI Coding Agent.
- Bao phủ hoàn chỉnh các quy chuẩn Code (Standards), Git, Metrics, Bảo mật, Logging.
- Trở thành Sổ tay Lập trình (Developer Handbook) chính thức cho Sprint 04.

---

# 41. References
- 31_SPRINT_03_BASE_CLASSES.md
- 32_SPRINT_03_EXCEPTIONS.md
- 33_SPRINT_03_EVENTS.md
- 34_SPRINT_03_DOMAIN_PRIMITIVES.md
- 35_SPRINT_03_DOMAIN_SERVICES.md
- 36_SPRINT_03_SPECIFICATIONS.md
- 37_SPRINT_03_REPOSITORIES.md
- 38_SPRINT_03_FACTORIES.md
- 39_SPRINT_03_DOMAIN_POLICIES.md
- 40_SPRINT_03_DOMAIN_VALIDATORS.md
- 41_SPRINT_03_DOMAIN_RESULT_PATTERN.md
- 42_SPRINT_03_DOMAIN_ERROR_CATALOG.md
- 43_SPRINT_03_DOMAIN_NOTIFICATIONS.md
- 44_SPRINT_03_ARCHITECTURE_REVIEW.md

---
---

# Sprint Freeze Record

| Item | Value |
|------|-------|
| Sprint | Sprint 03 |
| Status | Frozen |
| Freeze Tag | sprint-03-freeze |
| Freeze Date | 2026-07-18 |
| Approved By | Architecture Board |
| Next Sprint | Sprint 04 |
# END OF SPECIFICATION
