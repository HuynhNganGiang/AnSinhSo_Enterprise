# 41_SPRINT_03_DOMAIN_RESULT_PATTERN.md
## SPRINT 03 – DOMAIN RESULT PATTERN ARCHITECTURE
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 41_SPRINT_03_DOMAIN_RESULT_PATTERN
- **Title:** Domain Result Pattern Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Result Pattern |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Domain Result Pattern Architecture.
- **KHÔNG ĐƯỢC** vi phạm nguyên tắc "Exceptions are for Exceptional cases, Results are for Domain Failures".
- **KHÔNG ĐƯỢC** dùng Exception để điều hướng luồng nghiệp vụ (Control Flow).
- Mọi sự thay đổi về triết lý thiết kế Result Pattern bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là Đặc tả Kiến trúc (Architecture Specification) toàn diện xác định chiến lược thiết kế mẫu Result (Result Pattern) tại tầng Domain. Mục tiêu cốt lõi là thiết lập một phương thức tiêu chuẩn, thanh lịch, và hiệu suất cao để giao tiếp kết quả của một hành động nghiệp vụ (thành công hay thất bại) giữa các tầng kiến trúc mà không lạm dụng việc ném Exceptions (ngoại lệ), từ đó bảo vệ tính toàn vẹn và rõ ràng của hệ thống.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất của Result Pattern trong DDD.
- Phân tách rõ ràng khi nào dùng Result, khi nào dùng Exception.
- Quy định các loại Result tiêu chuẩn (Success, Failure, NotFound, Validation...).
- Thiết lập ranh giới trách nhiệm, quy tắc tương tác với các Building Blocks (CQRS, MediatR, UoW...).
- Xác định các rủi ro kiến trúc và Anti-Patterns.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C# (như `class Result<T>`, `interface IResult`).
- KHÔNG sinh mã HTTP Response (như `Ok()`, `BadRequest()`).
- KHÔNG sinh SQL, UML, XML, JSON hay Pseudo Code.
- KHÔNG giải thích chi tiết tầng Infrastructure.

---

# 6. Objectives
- **Explicit Failure Handling:** Buộc lập trình viên phải xử lý một cách tường minh cả hai nhánh Thành công và Thất bại.
- **Performance Optimization:** Loại bỏ chi phí hiệu năng cực kỳ đắt đỏ của thao tác rải (throw) và bắt (catch) Exceptions (call stack unwinding).
- **Expressive Domain:** Làm cho chữ ký (Signature) của phương thức trở nên trung thực. Trả về `Result<Citizen>` cho thấy hành động này có thể thất bại, thay vì trả về `Citizen` và ngầm ném lỗi.
- **Unified Communication:** Cung cấp một ngôn ngữ chung duy nhất để tầng Domain giao tiếp với tầng Application.

---

# 7. Domain Result Pattern Architecture
**Bản chất Kiến trúc (Architectural Essence):**
- **Domain Result Pattern là gì:** Là một Wrapper (vỏ bọc) đóng gói dữ liệu đầu ra của một quá trình kèm theo trạng thái (Thành công/Thất bại) và một danh sách các lỗi (Error) mô tả nguyên nhân thất bại.
- Kiến trúc quy định các loại Result cốt lõi:
  - **Success Result:** Hành động thành công, có thể chứa hoặc không chứa dữ liệu trả về (Data).
  - **Failure Result:** Lỗi chung của miền (Generic Domain Error).
  - **Validation Result:** Lỗi do dữ liệu đầu vào không hợp lệ (thường tập hợp nhiều lỗi cùng lúc từ Domain Validator).
  - **Business Rule Result:** Lỗi vi phạm nghiệp vụ cốt lõi (Domain Policy/Invariants).
  - **Conflict Result:** Lỗi xung đột trạng thái (Ví dụ: Công dân đã được đăng ký, phiên bản dữ liệu bị cũ).
  - **NotFound Result:** Không tìm thấy đối tượng (Aggregate) cần thao tác.
  - **Unauthorized Result:** Lỗi liên quan đến định danh/xác thực tại ngữ cảnh miền.
  - **Forbidden Result:** Lỗi vi phạm phân quyền/truy cập (Người dùng có định danh nhưng không đủ thẩm quyền duyệt quyết định).

---

# 8. Responsibilities
**Result vs Exception:**
- **Result (Expected Failure):** Dùng cho các thất bại **CÓ THỂ DỰ ĐOÁN TRƯỚC** trong luồng nghiệp vụ. (Ví dụ: Khách hàng không đủ tiền, Email sai định dạng, Citizen không tồn tại). Đây là những sự kiện nghiệp vụ bình thường.
- **Exception (Unexpected Failure):** Dùng cho các thất bại **KHÔNG THỂ DỰ ĐOÁN** hoặc **LỖI HỆ THỐNG**. (Ví dụ: Database chết, NullReference, OutOfMemory). Đừng bao giờ dùng Result cho lỗi đứt cáp mạng.

**Quy tắc điều hướng (Control Flow):**
- Tầng Application gọi Domain. Domain trả về `Result`. Tầng Application kiểm tra `if (result.IsFailure)` và rẽ nhánh. Đây là luồng điều hướng hợp lệ. Nếu dùng Exception, ứng dụng sẽ trở thành "Exception-Driven Development".

---

# 9. Relationship With Aggregate
- **Trả về Result từ Hành vi:** Các phương thức của Aggregate Root (ví dụ: `citizen.ChangeAddress()`) ưu tiên trả về `Result` nếu việc thay đổi có khả năng vi phạm quy tắc mà không nghiêm trọng đến mức phải phá sập hệ thống (Throw Exception).
- **Invariants Guard:** Mặc dù Aggregate bảo vệ Invariants, việc trả về `Result.Failure(DomainErrors.Address.Invalid)` giúp tầng gọi biết chính xác lỗi gì mà không cần `try-catch`.

---

# 10. Relationship With Value Objects
- **Instantiation:** Hàm khởi tạo (Factory Method) của Value Object (như `Email.Create()`) luôn luôn trả về `Result<Email>`. Không dùng constructor public để ném Exception nếu chuỗi email sai.

---

# 11. Relationship With Factory
- **Sinh đối tượng an toàn:** Factory Pattern tại Domain chịu trách nhiệm lắp ráp Aggregate. Hàm `Factory.Create()` sẽ trả về `Result<AggregateRoot>`. Nếu thiếu dữ kiện hoặc xung đột, nó trả về `Failure` kèm danh sách Validation Errors.

---

# 12. Relationship With Domain Validator
- **Nguồn cấp Lỗi:** Domain Validator là thành phần tính toán tĩnh. Đầu ra của nó sẽ được đóng gói thành một `ValidationResult` (kế thừa từ `Result`), chứa một mảng các `Error` để cung cấp cho người gọi bức tranh toàn cảnh về lý do dữ liệu bị từ chối.

---

# 13. Relationship With Domain Policy
- **Quyết định có điều kiện:** Domain Policy có thể trả về một `Result` chứa phân loại hoặc số tiền được tính toán. Nếu Policy không thể thực thi do thiếu điều kiện nghiệp vụ, nó trả về `Failure`.

---

# 14. Relationship With Domain Service
- **Orchestrator Return Type:** Mọi phương thức của Domain Service điều phối luồng nghiệp vụ đều phải trả về `Result` hoặc `Result<T>`. Đây là điểm cuối của tầng Domain giao tiếp với Application Layer.

---

# 15. Relationship With Specification
- **Bộ lọc không trả Result:** Specification thực thi hàm `IsSatisfiedBy()` và trả về `bool`. Không dùng Result cho Specification. Result chỉ bọc quá trình thực thi nghiệp vụ, không bọc biểu thức Logic.

---

# 16. Relationship With Repository
- **Tìm kiếm Thất bại:** Thay vì trả về `null` hoặc ném `EntityNotFoundException`, hàm `Repository.GetById()` có thể được kiến trúc để trả về `Result<T>` và nếu không có, nó trả về `NotFoundResult`. (Điều này loại bỏ hoàn toàn bẫy NullReference).

---

# 17. Collaboration (Application, CQRS, MediatR)
- **Application Layer:** Application Service (Use Cases) nhận Request, xử lý, và luôn luôn trả về `Result` cho Presentation Layer (Controller/Minimal API).
- **CQRS & MediatR:** Mọi `IRequestHandler` của MediatR (Command hoặc Query) bắt buộc định nghĩa kiểu trả về là `Result<TResponse>`.
- **Unit Of Work (UoW):** Application Layer chỉ gọi `UoW.CommitAsync()` nếu toàn bộ chuỗi Result trả về từ Domain đều là `Success`. Nếu có `Failure`, luồng dừng lại, không Commit.

---

# 18. Creation Strategy
Trình tự chiến lược tích hợp Result Pattern:
- **Step 1:** Chuẩn hóa kiểu `Error` (Mã lỗi, Thông báo lỗi) cho toàn hệ thống.
- **Step 2:** Định nghĩa Base `Result` và `Result<T>` tại SeedWork của Domain.
- **Step 3:** Mở rộng thành các loại Result cụ thể (ValidationResult, NotFoundResult) để cung cấp Meta-Data.
- **Step 4:** Thay thế toàn bộ các hàm trả về `void` ở Domain/Application thành trả về `Result`.
- **Step 5:** Cấm sử dụng `throw new Exception` cho các luồng nghiệp vụ thông thường.

---

# 19. Evolution Strategy
- **Sprint 03:** Thiết lập Architecture Specification cho Result Pattern.
- **Sprint 04:** Triển khai các Abstract Classes và Error Constants.
- **Sprint 05:** Thay thế Exceptions bằng Result trong các Domain Model.
- **Sprint 06:** Tích hợp Result với MediatR Pipeline Behaviors (tự động Validate và chặn Request nếu ValidationResult có lỗi).
- **Sprint 07:** Tích hợp Result Pattern với API Presentation (tự động map Result thành HTTP Status Code 200, 400, 404, 403, 409).

---

# 20. Principles
- **Honest Signatures (Chữ ký trung thực):** Nhìn vào hàm trả về `Result`, Lập trình viên biết ngay hàm này có thể thất bại. Hàm trả về `void` giả định luôn thành công.
- **Railway Oriented Programming (ROP):** Luồng xử lý như đường ray xe lửa. Nếu một bước trả về Failure, luồng rẽ sang nhánh phụ (chuyển thẳng ra ngoài) và bỏ qua các bước còn lại.
- **Error Centralization:** Gom toàn bộ thông báo lỗi nghiệp vụ thành các Constants (VD: `DomainErrors.Citizen.NotFound`), tránh Hard-code chuỗi chữ ở khắp nơi.

---

# 21. Classification
Các dạng Result trong Enterprise DDD:
- **Void Result:** Đại diện cho hành động không trả về dữ liệu (tương đương `void`).
- **Value Result:** Đại diện cho hành động trả về đối tượng (VD: `Result<Citizen>`).
- **Collection Result:** Trả về một tập hợp (VD: `Result<IReadOnlyList<Citizen>>`).
- **Paged Result:** Trả về tập hợp phân trang.

---

# 22. Naming Convention
- Bắt buộc trả về Interface hoặc Object có từ khóa `Result`.
- Lỗi cụ thể dùng từ khóa `Error` (VD: `Error.Validation(...)`, `Error.NotFound(...)`).
- Tránh đặt tên biến Result là `response`, `output` mà nên gọi là `result`.

---

# 23. Folder Strategy
Cấu trúc tổ chức thư mục tại tầng Domain:
- `SeedWork/`
  - `Results/` (Chứa các cấu trúc nền tảng của Result).
  - `Errors/` (Chứa cấu trúc Error).
- `Modules/`
  - `[Module_Name]/`
    - `Errors/` (Chứa các DomainErrors chuyên biệt cho Module đó).

*(Lưu ý: Chỉ thiết lập định hướng thư mục, không sinh source code tại đây).*

---

# 24. Dependency Rules
Ràng buộc phụ thuộc cứng cho Result Pattern:

**Allowed Dependencies:**
- Không có. Result Pattern là lớp đáy của kiến trúc, chỉ phụ thuộc vào `System.*`.

**Forbidden Dependencies (Tuyệt đối cấm):**
- `Microsoft.AspNetCore.Mvc` (Cấm thiết kế Result trả về `IActionResult` hay `Http Status Code` như 404 bên trong Domain).
- Bất kỳ Infrastructure Framework nào.

---

# 25. Validation Rules
Một Result Pattern hợp lệ khi:
- Dữ liệu lỗi (Error) không mang thông điệp Technical (như StackTrace, tên Cột DB). Thông điệp lỗi phải là Business Message.
- Result không cho phép truy cập thuộc tính `Data` nếu trạng thái là `Failure` (phải văng Exception kỹ thuật nếu Developer cố tình làm vậy).
- Trạng thái `IsSuccess` và `IsFailure` luôn đối nghịch nhau.

---

# 26. Performance Strategy
- **Zero Allocation on Success:** Thành công là phổ biến nhất. Khởi tạo Success Result phải tối ưu bộ nhớ nhất có thể (VD: dùng `struct` hoặc `ValueTask`).
- **No StackTrace:** Vì Result chỉ đóng gói string Error, nó loại bỏ hoàn toàn quá trình sinh StackTrace cực kỳ tốn CPU của Exception. Hệ thống có thể xử lý hàng ngàn Result lỗi trên giây mà không nghẽn.

---

# 27. AI Coding Constraints
Giới hạn hành vi bắt buộc của AI Coding Agent:
- KHÔNG tạo bất kỳ file C# nào triển khai (`class`, `interface`) cho Result Pattern tại đây.
- KHÔNG sinh mã JSON, XML, UML, hay Pseudo Code.
- Tuyệt đối cấm chèn mã nguồn map Result với HTTP Response (như `if (result.Error.Type == ErrorType.NotFound) return NotFound();`). Điều này thuộc về API Layer, không nằm trong tài liệu Domain này.
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 28. Definition Of Done
Tài liệu được coi là hoàn tất khi:
- Triết lý thay thế Exception bằng Result trong luồng Control Flow được làm rõ.
- Danh sách 8 loại Result cốt lõi được liệt kê.
- Ranh giới trách nhiệm của Result được xác lập hoàn toàn sạch bóng khỏi HTTP và SQL.
- Văn bản đạt chuẩn mực Enterprise Architecture của Sprint 03.

---

# 29. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **No Control Flow Exceptions** | Cấm dùng Exception rẽ nhánh nghiệp vụ. | Bắt buộc (Mandatory) |
| **Domain Error Only** | Lỗi phải là ngôn ngữ Ubiquitous Language. | Bắt buộc (Mandatory) |
| **No Http Status Codes** | Result không được chứa mã trạng thái 200/404. | Bắt buộc (Mandatory) |
| **No Stack Traces** | Không bọc Exception hệ thống vào Result. | Bắt buộc (Mandatory) |
| **Strict Data Access** | Cấm đọc dữ liệu `Data` nếu Result thất bại. | Bắt buộc (Mandatory) |

---

# 30. Architectural Anti Patterns
Các mẫu phản kiến trúc (Anti-Patterns) nghiêm trọng cần tránh:

- **The HTTP Result Leakage:**
  - Thiết kế `Result` chứa sẵn thuộc tính `StatusCode = 404`. Điều này khiến tầng Domain bị dính chặt vào giao thức Web (HTTP). Lỡ sau này làm App gRPC hoặc Console thì vứt bỏ.
- **The Wrapper of Exception:**
  - Bắt Exception hệ thống (`SqlException`) và nhét nguyên cục Exception đó vào `Result.Error`. Rò rỉ thông tin bảo mật và phá vỡ cấu trúc lỗi miền.
- **The Ignored Result:**
  - Application Layer gọi Domain Service, nhận về Result nhưng không kiểm tra `result.IsSuccess` mà cứ thế gọi `UoW.Commit()`.
- **Exception for Expected Failure:**
  - Quăng `UserNotFoundException` khi người dùng nhập sai mật khẩu (Đáng lẽ phải là `Result.Failure`).
- **The Empty Failure:**
  - Trả về Result thất bại nhưng không có chuỗi `Error` nào giải thích lý do, làm cho API trả về lỗi nhưng Front-End không biết hiển thị gì cho User.

---

# 31. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã giải thích cặn kẽ khái niệm Result Pattern, 8 loại Result và so sánh với Exception?
- [x] Đã thiết lập bức tường lửa chặn Result chứa Http Status Code?
- [x] Đã phân tích chi tiết các rủi ro, quy tắc CQRS/MediatR/UoW?
- [x] Đã bảo đảm tính tối ưu bộ nhớ và Railway Oriented Programming?
- [x] Đã tuân thủ chuẩn format 31 mục khắt khe của Enterprise Documentation Suite? (CÓ).

---

# 32. References
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
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

---
# END OF SPECIFICATION
