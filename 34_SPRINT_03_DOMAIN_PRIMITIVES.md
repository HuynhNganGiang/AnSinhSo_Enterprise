# 34_SPRINT_03_DOMAIN_PRIMITIVES.md
## SPRINT 03 – DOMAIN PRIMITIVES SPECIFICATION
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 34_SPRINT_03_DOMAIN_PRIMITIVES
- **Title:** Domain Primitives Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Software Architect, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Domain Primitive Architecture.
- KHÔNG ĐƯỢC thay đổi Strongly Typed Primitive Strategy.
- KHÔNG ĐƯỢC thay đổi Validation & Guard Strategy của Primitives.
- Mọi thay đổi phát sinh bắt buộc phải thông qua một Revision mới và được phê duyệt lại trước khi áp dụng.

---

# 4. Purpose
Tài liệu này là đặc tả kiến trúc (Architecture Specification) xác định chiến lược thiết kế cho các Domain Primitives (Nguyên thủy miền) trong tầng Domain. Mục tiêu là định hình một lớp bảo vệ vững chắc để loại bỏ hội chứng ám ảnh kiểu dữ liệu nguyên thủy (Primitive Obsession), đảm bảo rằng mọi dữ liệu đầu vào của hệ thống đều được bọc trong các kiểu dữ liệu có ý nghĩa nghiệp vụ, tự kiểm tra tính hợp lệ và hoàn toàn bất biến.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất kiến trúc của Domain Primitive và phân biệt nó với Value Object, Entity.
- Quy định các nguyên tắc thiết kế: Immutability, Fail Fast, Validation.
- Thiết lập chiến lược tạo các Strongly Typed Primitives.
- Định nghĩa ranh giới phụ thuộc và nguyên tắc so sánh (Equality).

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C#, Pseudo Code, XML, JSON hay UML.
- KHÔNG định nghĩa cách ánh xạ (mapping) Primitive xuống Database bằng EF Core (thuộc tầng Infrastructure).
- KHÔNG mô tả Data Transfer Objects (DTO) hoặc logic tầng API.
- KHÔNG định nghĩa Entity hay Aggregate cụ thể nào.

---

# 6. Objectives
- Triệt tiêu hoàn toàn Primitive Obsession trong hệ thống (không dùng `string`, `int`, `decimal` trực tiếp cho các khái niệm nghiệp vụ).
- Đảm bảo tính Persistence Ignorance và Framework Ignorance tuyệt đối.
- Giảm thiểu số lượng lỗi xác thực dữ liệu (Validation Errors) ở các tầng trên nhờ việc Primitive tự bảo vệ tính đúng đắn ngay từ lúc khởi tạo.
- Nâng cao tính biểu đạt của mã nguồn theo đúng Ubiquitous Language.

---

# 7. Domain Primitive Architecture
**Giải thích cốt lõi:**
- **Domain Primitive là gì:** Là một khái niệm hẹp hơn Value Object. Nó là một đối tượng bao bọc (wrap) ĐÚNG MỘT giá trị nguyên thủy duy nhất (ví dụ: một chuỗi `string` hoặc một số `decimal`), nhưng gắn thêm ý nghĩa nghiệp vụ và các quy tắc xác thực chặt chẽ.
- **Sự khác biệt với Value Object:** Value Object có thể chứa nhiều thuộc tính cấu thành (ví dụ: `Address` gồm `Street`, `City`, `ZipCode`). Domain Primitive CHỈ ĐẠI DIỆN cho một giá trị đơn (ví dụ: `EmailAddress`, `PhoneNumber`, `CitizenId`).
- **Sự khác biệt với Entity:** Domain Primitive hoàn toàn không có Identity (Định danh) và không có vòng đời thay đổi trạng thái (State Lifecycle). Hai Primitive có cùng giá trị thì được coi là một.
- **Tính tự thân (Self-Validation):** Primitive bắt buộc phải Validation ngay khi khởi tạo. Không bao giờ tồn tại một Primitive mang trạng thái không hợp lệ trong bộ nhớ.
- **Framework Ignorance:** Primitive không mang bất kỳ attribute nào của ORM, JSON Serializer, hay Web API.

---

# 8. Primitive Creation Order
Quy trình và thứ tự thiết kế Primitive (chỉ áp dụng khi vào giai đoạn Coding):
- **Step 1:** Xác định nhu cầu nghiệp vụ (nhận diện Primitive Obsession).
- **Step 2:** Khởi tạo cấu trúc bao bọc (Wrapper) cho giá trị đơn lõi.
- **Step 3:** Thiết lập Guard Strategy (Kiểm tra hợp lệ, ném DomainException nếu lỗi).
- **Step 4:** Triển khai Factory/Constructor ẩn (Private/Protected) và Factory Method (Public static).
- **Step 5:** Triển khai Equality Strategy (So sánh giá trị lõi).
- **Step 6:** Validation & Freeze kiến trúc.

---

# 9. Primitive Evolution Strategy
Lộ trình phát triển của Domain Primitives qua các Sprint:
- **Sprint 03:** Thiết lập Seed Work và Đặc tả cho Primitives.
- **Sprint 04:** Triển khai các Domain Primitives thực tế (Ví dụ: `Money`, `Email`, `TaxId`) để sử dụng trong Business Entities.
- **Sprint 05:** Cấu hình Value Converters trong Infrastructure để Entity Framework có thể lưu trữ Primitive vào cột đơn trong Database.
- **Sprint 06:** Cấu hình Type Converters/Model Binders ở tầng Application/API để tự động chuyển đổi từ DTO sang Primitive.

---

# 10. Primitive Principles
Nguyên tắc bắt buộc đối với mọi Domain Primitive:
- **Single Value Wrap:** Chỉ bọc một kiểu dữ liệu nguyên thủy duy nhất.
- **Immutability:** Trạng thái bên trong tuyệt đối không thể thay đổi sau khi khởi tạo (Read-Only).
- **Fail Fast:** Ném ngoại lệ miền (Domain Exception) ngay lập tức nếu dữ liệu đầu vào không hợp lệ.
- **No Business Logic:** Không chứa các tính toán nghiệp vụ phức tạp liên đới đến nhiều đối tượng. Nó chỉ kiểm tra tính toàn vẹn của chính nó (Format, Length, Range).
- **Persistence Ignorance:** Không có thuộc tính khóa ngoại, không biết Database là gì.

---

# 11. Primitive Classification Strategy
Phân loại các Domain Primitive thường gặp trong hệ thống:
1. **Identifiers (Định danh):** `CitizenId`, `HouseholdId`, `PolicyId`. Thay vì dùng Guid/Int thuần, dùng Primitive để chống gán nhầm loại ID.
2. **Quantities & Measurements (Đo lường):** `Money`, `Weight`, `Percentage`. (Mặc dù `Money` đôi khi là Value Object nếu có thêm Currency, nhưng ở mức cơ bản nó được xem là Primitive).
3. **Formatted Strings (Chuỗi định dạng):** `EmailAddress`, `PhoneNumber`, `TaxCode` (Mã số thuế), `IdentityCardNumber` (Số CCCD).
4. **Temporal (Thời gian):** `BirthDate`, `EffectiveDate`.

---

# 12. Strongly Typed Primitive Strategy
- **Type Safety:** Thay thế tất cả các tham số mang tính cấu trúc trong các Method Signature bằng Strongly Typed Primitive. Thay vì `UpdateEmail(string email)`, bắt buộc phải là `UpdateEmail(EmailAddress email)`.
- **Phòng chống hoán đổi vị trí (Positional Swap Bug):** Việc sử dụng Primitive ngăn chặn lỗi truyền sai thứ tự tham số (ví dụ truyền nhầm tham số String `phone` vào chỗ của String `email`).

---

# 13. Primitive Validation Strategy
- **Xác thực tại nguồn:** Toàn bộ quá trình xác thực (Regex, độ dài, giới hạn giá trị) diễn ra ngay tại Constructor hoặc Factory Method của Primitive.
- **Không dùng Validator ngoài:** Không phụ thuộc vào thư viện bên ngoài (như FluentValidation) để kiểm tra các quy tắc bất biến cơ bản của Primitive. FluentValidation chỉ nên dùng ở tầng Application để validate DTO. Tầng Domain phải tự chủ.
- **Exceptions:** Sử dụng hệ thống Domain Exception đã thiết kế ở tài liệu `32_SPRINT_03_EXCEPTIONS.md`.

---

# 14. Guard Strategy
- Sử dụng **Guard Clauses** (Mệnh đề bảo vệ) để Fail Fast.
- Mọi điều kiện đầu vào bất thường (Null, Empty, Out of Range, Regex Mismatch) phải kích hoạt việc ném ngoại lệ lập tức.
- Không sử dụng cấu trúc `if-else` lồng nhau phức tạp. Kiểm tra và chặn ngay từ dòng lệnh đầu tiên của hàm khởi tạo.

---

# 15. Immutability Rules
- Cấm hoàn toàn (100%) việc sử dụng Public Setters.
- Giá trị nội tại phải được đánh dấu bằng các từ khóa cấp ngôn ngữ để không thể thay đổi sau khi khởi tạo (chỉ gán qua Constructor hoặc Init-only properties).
- Bất kỳ thao tác nào muốn thay đổi giá trị (ví dụ: viết hoa chuỗi, cộng trừ số) đều phải tạo ra và trả về một Instance mới của Primitive đó (như cách chuỗi `string` hoạt động trong .NET).

---

# 16. Equality Strategy
- **Value-Based Equality:** Hai Domain Primitive được coi là hoàn toàn bằng nhau nếu và chỉ nếu giá trị nguyên thủy bên trong của chúng bằng nhau.
- Bắt buộc phải ghi đè (Override) phương thức `Equals` và `GetHashCode`.
- Bắt buộc phải nạp chồng (Overload) các toán tử `==` và `!=` để việc so sánh trong mã nguồn trở nên tự nhiên.
- Có thể hỗ trợ interface so sánh như `IEquatable<T>` (sẽ quyết định khi Coding).

---

# 17. Factory Strategy
- Ẩn Constructor: Khuyến nghị (nhưng không bắt buộc ép cứng) việc sử dụng Private/Protected Constructors.
- Cung cấp Factory Method (ví dụ: `Create(string value)`) để thực hiện Validation trước khi quyết định cấp phát bộ nhớ cho đối tượng mới. Điều này làm rõ ý nghĩa "Tạo mới và Xác thực" hơn so với việc gọi hàm `new` trực tiếp.

---

# 18. Naming Convention
Quy tắc đặt tên bắt buộc:
- Tên Primitive phải là danh từ, phản ánh chính xác nghiệp vụ thực thể.

**Allowed (Cho phép):**
- `EmailAddress`
- `TaxId`
- `IdentityCardNumber`
- `MonetaryAmount`

**Not Allowed (Tuyệt đối cấm):**
- Tên gắn hậu tố kiểu dữ liệu: `EmailString`, `AmountDecimal`, `IdGuid`.
- Tên gắn hậu tố chức năng: `EmailValidator`, `AmountWrapper`.
- Tên quá chung chung: `Value1`, `DataString`.

---

# 19. Dependency Rules
Ràng buộc phụ thuộc cứng cho Primitive:

**Allowed Dependencies:**
- `System.*` (Các thư viện lõi: `System.Text.RegularExpressions`, `System.ArgumentException`, v.v.).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `EntityFrameworkCore`
- `Dapper`
- `MediatR`
- `ASP.NET`
- `Newtonsoft.Json` (Không gắn `[JsonProperty]` hay custom converter vào trong Domain).
- Bất kỳ thư viện liên quan đến I/O, Web, hay Logging.

---

# 20. Validation Rules
Kiến trúc Primitive được xem là hợp lệ khi:
- **Immutable:** Không thể bị sửa đổi (set) sau khi sinh ra.
- **Fail Fast:** Kiểm tra dữ liệu tại cửa ngõ.
- **Single Responsibility:** Chỉ quản lý và bảo vệ duy nhất một giá trị nguyên thủy.
- **Equality:** So sánh dựa trên giá trị, không dựa trên tham chiếu bộ nhớ.
- **No HTTP, No EF, No ORM, No Logging, No API, No Infrastructure.**

---

# 21. AI Coding Constraints
Giới hạn bắt buộc của AI Coding Agent khi tạo tài liệu này:
- KHÔNG sinh mã C# dưới bất kỳ hình thức nào.
- KHÔNG sinh Pseudo Code hay JSON.
- KHÔNG vẽ UML.
- KHÔNG sinh các đoạn mã EF Core Value Converter.
- KHÔNG sinh mã tạo đối tượng Primitive cụ thể.

---

# 22. Definition of Done
Tài liệu đạt tiêu chuẩn hoàn thành khi:
- Làm rõ ranh giới khái niệm giữa Domain Primitive, Value Object và Entity.
- Khẳng định tính chất bất biến (Immutability) và khả năng tự kiểm tra (Self-Validation).
- Phân định rõ chiến lược Equality và Factory.
- Tương thích 100% với nguyên tắc Persistence Ignorance và Framework Ignorance.

---

# 23. Validation Matrix
Bảng ma trận tự kiểm tra sự tuân thủ:

| Tiêu chí | Nội dung kiểm tra | Đánh giá |
|---|---|---|
| **Architecture** | Không mang Identity, Không có Lifecycle. | Bắt buộc. |
| **DDD** | Wrap 1 giá trị cốt lõi, tên gọi theo Ubiquitous Language. | Bắt buộc. |
| **Dependency** | Loại trừ EF Core, JSON Serializer, MediatR. | Bắt buộc. |
| **Immutability** | Đảm bảo tính Read-only của giá trị bên trong. | Bắt buộc. |
| **Fail Fast** | Ném ngoại lệ ngay tại Constructor nếu invalid. | Bắt buộc. |

---

# 24. Risks
- **Over-Engineering (Làm quá mức):** Tạo Primitive cho mọi thuộc tính nhỏ nhặt trong hệ thống (như `FirstName`, `LastName` riêng rẽ khi không có rule validation cụ thể) dẫn đến bùng nổ số lượng class.
  - *Giải pháp:* Chỉ bọc những thuộc tính có rule validation thực sự, có ý nghĩa nghiệp vụ cao, hoặc dễ bị hoán đổi nhầm lẫn (Identifiers, Formatted Strings).
- **Framework Leakage:** Developer lười biếng gắn thẳng `[Column("Email")]` của EF Core vào trong Domain Primitive.
  - *Giải pháp:* Cấm tuyệt đối. Mọi cấu hình ánh xạ phải nằm ở tầng Infrastructure bằng Fluent API.
- **Performance Overhead:** Khởi tạo quá nhiều đối tượng nhỏ gây áp lực lên Garbage Collector.
  - *Giải pháp:* Tối ưu cấu trúc dữ liệu nếu cần ở giai đoạn Coding, nhưng không thỏa hiệp tính an toàn nghiệp vụ ở giai đoạn Architecture.

---

# 25. AI Review Checklist
- [x] Không sinh C#, Pseudo Code, UML, XML, JSON?
- [x] Đã thiết lập Architecture Freeze Rule?
- [x] Đã mô tả Primitive Architecture, Creation Order, Evolution Strategy?
- [x] Đã giải thích rõ sự khác biệt giữa Primitive vs Value Object vs Entity?
- [x] Đã thiết lập Guard Strategy, Validation Strategy và Immutability Rules?
- [x] Đã tuân thủ nghiêm ngặt Dependency Rules (Cấm EF Core, HTTP, JSON)?
- [x] Tài liệu tuân thủ chuẩn Enterprise Documentation Suite, đồng bộ với tài liệu 31, 32, 33?

---

# 26. References
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
- 28_SPRINT_02_REVIEW.md
- 29_SPRINT_03_IMPLEMENTATION_PLAN.md
- 30_SPRINT_03_PROJECT_INIT.md
- 31_SPRINT_03_BASE_CLASSES.md
- 32_SPRINT_03_EXCEPTIONS.md
- 33_SPRINT_03_EVENTS.md

---
# END OF SPECIFICATION
