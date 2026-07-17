# 40_SPRINT_03_DOMAIN_VALIDATORS.md
## SPRINT 03 – DOMAIN VALIDATORS ARCHITECTURE
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 40_SPRINT_03_DOMAIN_VALIDATORS
- **Title:** Domain Validators Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Validators |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Domain Validator Architecture.
- **KHÔNG ĐƯỢC** vi phạm nguyên tắc "Validator là đối tượng thuần túy kiểm tra dữ liệu, không chứa nghiệp vụ, không thay đổi trạng thái hệ thống".
- **KHÔNG ĐƯỢC** rò rỉ bất kỳ công nghệ lưu trữ, HTTP, hay Infrastructure nào vào Domain Validator.
- Mọi sự thay đổi về triết lý thiết kế Validator bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là Đặc tả Kiến trúc (Architecture Specification) toàn diện xác định chiến lược thiết kế các Validator tại tầng Domain trong kiến trúc Domain-Driven Design (DDD). Mục tiêu cốt lõi là thiết lập ranh giới cực kỳ nghiêm ngặt để đảm bảo Validator chỉ làm một việc duy nhất: Kiểm tra tính hợp lệ của dữ liệu Miền (Domain Data), giúp bảo vệ Aggregate Root và các Building Blocks khác khỏi những dữ liệu không hợp lệ mà không lấn sân sang luồng xử lý nghiệp vụ.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất, mục đích của Domain Validator trong DDD.
- Phân tách rõ ràng Validator với Application Validation, Entity Validation, Specification, và FluentValidation.
- Thiết lập các giới hạn cứng: Validator được làm gì và cấm làm gì.
- Phân tích chi tiết các rủi ro và Anti-Patterns cụ thể.
- Định hình cấu trúc phân loại, chiến lược khởi tạo và quy tắc phụ thuộc.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C# (như `class`, `interface`).
- KHÔNG sinh cấu hình FluentValidation cụ thể.
- KHÔNG sinh SQL, UML, XML, JSON hay Pseudo Code.
- KHÔNG giải thích chi tiết tầng Infrastructure hay giao diện UI.

---

# 6. Objectives
- **Phòng vệ Từ Xa (Defensive Programming):** Ngăn chặn các dữ liệu sai lệch, không thỏa mãn quy tắc cấu trúc miền xâm nhập vào Aggregate.
- **Tách bạch Trách nhiệm (Separation of Concerns):** Gỡ bỏ logic kiểm tra dữ liệu đầu vào cồng kềnh ra khỏi Constructor và phương thức của Entity.
- **Đảm bảo Thuần khiết (Purity):** Đảm bảo Validator là các hàm tinh khiết (Pure Functions), không phụ thuộc vào I/O hay trạng thái hệ thống.
- **Bảo vệ Trọng tâm Miền (Domain Centricity):** Định hình Validator như những người gác cổng mù tịt về công nghệ lưu trữ (Persistence Ignorance).

---

# 7. Domain Validator Architecture
**Bản chất Kiến trúc (Architectural Essence):**
- **Domain Validator là gì:** Là một thành phần thuộc tầng Domain, đóng gói các bộ quy tắc kiểm tra tính toàn vẹn và hợp lệ của cấu trúc dữ liệu (như độ dài, định dạng, giá trị biên của Value Object hoặc các Entity).
- **Phân tách Ranh giới Rõ ràng:** Domain Validator chỉ nhận dữ liệu đầu vào, kiểm tra đối chiếu với các quy tắc miền, và trả về danh sách lỗi (Errors/Exceptions) nếu vi phạm. Nó không tham gia vào luồng quyết định nghiệp vụ (Business Flow).
- **Stateless & Side Effect Free:** Kiến trúc của Domain Validator là kiến trúc tĩnh. Nó không lưu trữ trạng thái, không thay đổi (mutate) đối tượng được truyền vào.

---

# 8. Responsibilities
Những trách nhiệm TUYỆT ĐỐI của Domain Validator:
- **Kiểm tra Tính Hợp lệ (Validation):** Đánh giá các thuộc tính, thực thể, hoặc Value Object để đảm bảo chúng tuân thủ các Invariants cấu trúc (VD: Tuổi phải lớn hơn 0, CMND phải đủ 12 số).
- **Gom nhóm Lỗi (Error Aggregation):** Tập hợp tất cả các lỗi Validation thay vì văng Exception ngay ở lỗi đầu tiên (Fail Fast vs Fail Safe tùy chiến lược, nhưng tập trung vào cung cấp thông tin lỗi Domain).

Những thứ Domain Validator **TUYỆT ĐỐI KHÔNG LÀM:**
- Không xử lý nghiệp vụ (No Business Logic).
- Không truy cập Repository để tra cứu (No Repository Call).
- Không truy cập Database (No SQL, No EF Core).
- Không gọi HTTP hay API ngoài.
- Không lưu dữ liệu (No Save).
- Không phát Domain Event.
- Không thay đổi trạng thái Aggregate (No Mutation).

---

# 9. Relationship With Aggregate
- **Không thay thế Entity Validation cốt lõi:** Aggregate Root vẫn phải bảo vệ Invariants nội tại của nó. Domain Validator được sử dụng như một lớp bảo vệ bên ngoài hoặc hỗ trợ cho Aggregate khi cấu trúc validation trở nên quá phức tạp.
- **Không xâm phạm (Non-invasive):** Validator không sửa đổi dữ liệu bên trong Aggregate. Aggregate gọi Validator hoặc Validator kiểm tra Aggregate từ bên ngoài và trả về kết quả `IsValid/Errors`.

---

# 10. Relationship With Value Objects
- **Validation tại nguồn:** Đa số các quy tắc validation cơ bản (như định dạng Email, độ dài chuỗi) nên nằm ngay trong constructor của Value Object.
- **Complex Validation:** Domain Validator được sử dụng khi cần kiểm duyệt sự kết hợp của nhiều Value Object lại với nhau thành một thực thể phức tạp.

---

# 11. Relationship With Domain Service
- **Công cụ hỗ trợ:** Domain Service có thể gọi Domain Validator để kiểm tra tính hợp lệ của dữ liệu trước khi tiến hành một quy trình nghiệp vụ phức tạp.
- **Khác biệt cốt lõi:** Validator chỉ trả về kết quả Đúng/Sai/Lỗi dựa trên luật kiểm tra tĩnh. Domain Service điều phối luồng làm việc và thay đổi trạng thái hệ thống.

---

# 12. Relationship With Specification
- **Khác biệt Mục đích:**
  - `Specification` thường được dùng để lập bộ lọc truy vấn dữ liệu (Querying/Filtering) hoặc định nghĩa điều kiện nghiệp vụ để duyệt (Satisfies).
  - `Domain Validator` dùng để xác minh tính đúng đắn cấu trúc và định dạng dữ liệu (Data Integrity).
- **Phân biệt:** Specification có thể được truyền vào Repository. Validator tuyệt đối KHÔNG.

---

# 13. Relationship With Repository
- **Mù tịt hoàn toàn (Total Ignorance):** Validator KHÔNG bao giờ biết đến sự tồn tại của Repository.
- **Uniqueness Check:** Nếu cần kiểm tra "Email đã tồn tại chưa", đây là trách nhiệm của Domain Service (gọi Repository), KHÔNG PHẢI của Validator.

---

# 14. Relationship With Factory
- **Tiền xử lý (Pre-condition):** Factory có thể sử dụng Validator để kiểm tra toàn bộ tập dữ liệu đầu vào trước khi quyết định gọi Constructor khởi tạo Aggregate.
- **Bảo vệ tính Atomic:** Giúp Factory tuân thủ nguyên tắc "Chỉ trả về Aggregate hợp lệ 100%".

---

# 15. Relationship With Domain Policy
- **Policy vs Validator:** Policy đưa ra quyết định hoặc tính toán kết quả (VD: Mức thuế là bao nhiêu). Validator chỉ xác định dữ liệu đầu vào (VD: Thuế suất nhập vào có lớn hơn 0 và nhỏ hơn 100 không).
- **Tính kết hợp:** Policy giả định dữ liệu đầu vào đã được Validator làm sạch.

---

# 16. Validator Creation Strategy
Trình tự chiến lược thiết kế Validator:
- **Step 1:** Ưu tiên đưa Validation vào trong Value Object trước.
- **Step 2:** Đưa Invariants vào trong Aggregate Root.
- **Step 3:** Nếu các quy tắc trên làm phình to Entity/VO, tạo một lớp Domain Validator độc lập tại tầng Domain.
- **Step 4:** Định hình tập hợp các rule thuần túy tĩnh (độ dài, định dạng, dải giá trị).
- **Step 5:** Rà soát loại bỏ hoàn toàn các rule yêu cầu kiểm tra Database (đẩy các rule này sang Domain Service).

---

# 17. Validator Principles
- **Pure Function (Hàm tinh khiết):** Input giống nhau phải luôn trả ra kết quả Validator giống nhau trong mọi bối cảnh.
- **Statelessness (Phi trạng thái):** Validator không lưu trữ biến private giữ trạng thái giữa các lần gọi.
- **No Side Effects (Không tác dụng phụ):** Validator kiểm tra dữ liệu trong chế độ Read-Only.
- **Fail Completeness:** Ưu tiên thu thập mọi lỗi thay vì văng Exception ngay lập tức (nếu áp dụng mô hình Error List/Notification Pattern).

---

# 18. Validator Classification
Phân loại Validator:
- **Application Validation (Ngoài Domain):** Kiểm tra cấu trúc DTO (như Null, MaxLength, Regex) ngay tại biên (API/Controller). (Thường dùng FluentValidation tại đây).
- **Domain Validation (Trong Domain):** Kiểm tra tính hợp lệ của Invariants, các sự kết hợp giữa các thuộc tính tạo thành Business Validity.
- **Phân biệt với FluentValidation:** FluentValidation là thư viện (Library). Domain Validation là Khái niệm (Concept). Trong Domain, có thể dùng FluentValidation nhưng tuyệt đối CẤM tiêm DI của hạ tầng (như `DbContext`) vào các Validator rules.

---

# 19. Collaboration
- **Application Layer:** Gọi Validator trên DTO trước khi gọi Domain.
- **Domain Layer:** Gọi Domain Validator ngay trước khi thực thi thay đổi trạng thái (Mutations) trên Aggregate.
- **Infrastructure:** Mù với Domain Validator.

---

# 20. Naming Convention
- Bắt buộc có hậu tố `Validator`.
- Tên Validator phải đi kèm với tên Entity hoặc Concept nó kiểm tra.
- VD: `CitizenAgeValidator`, `PensionClaimValidator`.
- KHÔNG đặt tên gây nhầm lẫn nghiệp vụ như `CheckCitizenEligibility` (nghe giống Domain Service hoặc Policy).

---

# 21. Folder Strategy
Cấu trúc tổ chức thư mục của Validator tại tầng Domain:
- `Validators/`
  - `SeedWork/` (Base Validator Interface).
  - `Modules/`
    - `Demographic/`
    - `SocialSecurity/`
  - `Shared/` (Các Validator dùng chung như `EmailDomainValidator`).

*(Lưu ý: Chỉ thiết lập định hướng thư mục, không sinh source code tại đây).*

---

# 22. Dependency Rules
Ràng buộc phụ thuộc cứng cho Domain Validator:

**Allowed Dependencies:**
- Tầng Domain (Entities, Value Objects, Domain Exceptions).
- `System.*` (Thư viện cốt lõi).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `Repositories` / `DbContext` / `EF Core` / `Dapper` / `SQL`.
- `HttpClient` / `API Clients`.
- `ASP.NET` / `MVC` / `SignalR`.
- `Logging` (Serilog, ILogger).
- Bất kỳ Infrastructure Layer component nào.

---

# 23. Validation Rules
Một Domain Validator hợp lệ khi:
- **Stateless:** Được khởi tạo 1 lần và dùng lại cho nhiều request an toàn.
- **Side Effect Free:** Không thay đổi (mutate) object.
- **No Persistence:** Cấm mọi thao tác Save/Read từ ổ cứng.
- **No Infrastructure:** Không bắt Try-Catch các lỗi `SqlException` hay `HttpRequestException`.
- **Pure Domain:** Viết bằng ngôn ngữ của Ubiquitous Language.

---

# 24. Performance Strategy
- **In-Memory Speed:** Validator phải thực thi cực nhanh (O(1) hoặc O(N) trong bộ nhớ).
- **Fast Path Rejection:** Kiểm tra các rule nhẹ (như null, định dạng) trước khi chuyển sang các rule phức tạp (tính toán toán học nội bộ).
- **Singleton Lifecycle:** Vì Validator là Stateless, chúng nên được đăng ký dưới dạng Singleton trong DI Container để tiết kiệm bộ nhớ cấp phát.

---

# 25. AI Coding Constraints
Giới hạn hành vi bắt buộc của AI Coding Agent:
- KHÔNG tạo bất kỳ file C# nào triển khai (`class`, `interface`) cho các Validator tại đây.
- KHÔNG sinh mã JSON, XML, UML, hay Pseudo Code.
- CẤM tiêm Repository vào Validator. Nếu phát hiện yêu cầu "Validator kiểm tra trùng lặp DB", AI phải từ chối và hướng dẫn dùng Domain Service.
- CẤM sinh code cho thấy Validator có hành vi lưu dữ liệu (`Save`) hoặc phát `Domain Event`.
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 26. Definition Of Done
Tài liệu được coi là hoàn tất khi:
- Ranh giới của Domain Validator được cô lập hoàn toàn khỏi Database và Business Logic.
- Mọi Anti-Patterns được chỉ rõ và phân tích sâu.
- Các nguyên tắc "Thuần khiết miền" (Pure Domain) được bảo vệ bằng Checklist và Validation Matrix.
- Văn bản đạt chuẩn mực Enterprise Architecture, tương đương hoặc cao cấp hơn tài liệu 37, 38, 39.

---

# 27. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **Stateless** | Không giữ trạng thái dữ liệu (Session Data) giữa các lần gọi. | Bắt buộc (Mandatory) |
| **Side Effect Free** | Không sửa đổi dữ liệu (Mutate) Aggregate truyền vào. | Bắt buộc (Mandatory) |
| **No Persistence** | Không gọi Save, Commit, Rollback. | Bắt buộc (Mandatory) |
| **No Repository** | CẤM tiêm IRepository vào bên trong Validator. | Bắt buộc (Mandatory) |
| **No I/O Operations**| Cấm mọi thao tác File, HTTP, Network. | Bắt buộc (Mandatory) |
| **No Infrastructure**| Mù hoàn toàn về EF Core, SQL, Dapper, ASP.NET. | Bắt buộc (Mandatory) |
| **No Domain Events** | Validator không được tự ý Publish/Dispatch Events. | Bắt buộc (Mandatory) |
| **Pure Domain** | Logic kiểm tra phản ánh chính xác Ubiquitous Language. | Bắt buộc (Mandatory) |

---

# 28. Risks
- **Fat Validator:** Validator phình to chứa toàn bộ logic nghiệp vụ (if/else lồng nhau) thay vì phân tán về Value Objects.
- **Bypass Validation:** Aggregate cung cấp Setter công khai, khiến mã bên ngoài sửa trực tiếp dữ liệu mà quên gọi qua Validator.
- **Duplication of Validation:** Lặp lại y hệt logic validation của DTO (Application) xuống Domain Validator mà không mang thêm ý nghĩa nghiệp vụ miền.

---

# 29. Architectural Anti Patterns
Các mẫu phản kiến trúc (Anti-Patterns) nghiêm trọng cần tránh:

- **Fat Validator:**
  - Nhồi nhét hàng chục quy tắc phức tạp, thậm chí cả quy tắc phân luồng (Routing Rules) vào trong Validator, biến nó thành một God Class.
- **Validator Calling Repository (Lỗi tử huyệt):**
  - Validator tiêm (inject) `IRepository` để thực hiện câu query "Tìm xem công dân có CMND này tồn tại chưa". Phá vỡ tính Pure, biến Validator thành một Infrastructure Wrapper. (Trách nhiệm này thuộc về Domain Service).
- **Validator Calling API:**
  - Validator gọi sang cổng API bên thứ 3 (ví dụ Hệ thống Thuế Quốc gia) để xác thực mã số thuế. Validator sẽ bị treo nếu API sập.
- **Validator Saving Database:**
  - Nếu dữ liệu sai, Validator ghi log thẳng xuống Database thông qua `DbContext`. Phá vỡ SRP và Persistence Ignorance.
- **Validator Mutating Aggregate:**
  - Validator có hàm `Sanitize()` tự động viết hoa tên hoặc cắt tỉa khoảng trắng (Trim) của đối tượng truyền vào. Validator chỉ Kiểm Tra (Read-Only), không Cắt Gọt (Mutate).
- **Validator Throwing Infrastructure Exception:**
  - Nếu có lỗi, Validator văng `SqlException` hoặc `HttpException` thay vì `DomainValidationException`.
- **Validator Doing Business Logic:**
  - Dùng Validator để quyết định xem công dân có đủ điều kiện nhận lương hưu không. (Đây là Domain Policy, không phải cấu trúc Validation).
- **Generic Validator Abuse:**
  - Lạm dụng một Generic `Validator<T>` duy nhất chạy Reflection để kiểm tra Annotation cho toàn bộ các thực thể, khiến mã siêu chậm và lỏng lẻo.

---

# 30. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã giải thích cặn kẽ khái niệm Domain Validator và so sánh với Entity Validation, Specification, Policy?
- [x] Đã thiết lập bức tường lửa chặn Validator truy xuất CSDL (No Repository, No Persistence)?
- [x] Đã phân tích chi tiết 8 Anti-Patterns, đặc biệt là Validator Calling Repository và Validator Mutating Aggregate?
- [x] Đã bảo đảm tính Stateless, Side-Effect Free, Pure Domain của Validator?
- [x] Đã tuân thủ chuẩn format 31 mục khắt khe của Enterprise Documentation Suite? (CÓ).

---

# 31. References
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

---
# END OF SPECIFICATION
