# 35_SPRINT_03_DOMAIN_SERVICES.md
## SPRINT 03 – DOMAIN SERVICES SPECIFICATION
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 35_SPRINT_03_DOMAIN_SERVICES
- **Title:** Domain Services Architecture Specification
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
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Domain Service Architecture.
- KHÔNG ĐƯỢC nới lỏng Dependency Rules (Cấm mọi ngoại lệ phụ thuộc hạ tầng).
- KHÔNG ĐƯỢC phá vỡ nguyên tắc Stateless.
- Mọi thay đổi phát sinh bắt buộc phải thông qua một Revision mới và được phê duyệt lại trước khi áp dụng.

---

# 4. Purpose
Tài liệu này là đặc tả kiến trúc (Architecture Specification) xác định chiến lược thiết kế và sử dụng Domain Services (Dịch vụ Miền). Mục tiêu là định hình một lớp đối tượng đặc biệt để xử lý các nghiệp vụ cốt lõi phức tạp liên quan đến nhiều Aggregate, mà nếu nhồi nhét vào một Entity đơn lẻ sẽ gây mất tự nhiên và phá vỡ nguyên tắc Trách nhiệm duy nhất (Single Responsibility Principle).

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa kiến trúc và vai trò của Domain Service.
- Phân biệt Domain Service với các Building Blocks khác.
- Thiết lập quy tắc giao tiếp (Collaboration) và điều phối Aggregate.
- Quy định nguyên tắc Stateless, Persistence Ignorance và Naming Convention.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C#, Pseudo Code, XML, JSON hay UML.
- KHÔNG mô tả Application Service (thuộc tầng Use Cases).
- KHÔNG mô tả thiết kế Database, ORM hay API.
- KHÔNG sử dụng các kỹ thuật hạ tầng (như Dependency Injection Framework).

---

# 6. Objectives
- Cung cấp một nơi an toàn để thực thi logic miền liên quan đến nhiều đối tượng mà không ép các đối tượng này phụ thuộc cứng vào nhau.
- Triệt tiêu "Anemic Domain Model" bằng cách giữ lại logic miền thực thụ, nhưng cũng tránh "Bloated Entity" (thực thể quá phình to).
- Đảm bảo tính độc lập hoàn toàn với Framework, bảo vệ lõi nghiệp vụ.

---

# 7. Domain Service Architecture
**Giải thích khái niệm và sự khác biệt:**
- **Domain Service:** Là một hoạt động hoặc hành động trong thế giới thực mà không thuộc về tự nhiên của bất kỳ Entity hay Value Object nào. Nó không có trạng thái (Stateless) và chỉ chứa hành vi nghiệp vụ (Behavior).
- **Khác với Entity:** Entity có Định danh (Identity) và Trạng thái (State). Domain Service KHÔNG CÓ cả hai.
- **Khác với Value Object / Domain Primitive:** Value Object là dữ liệu cấu trúc, Primitive là dữ liệu đơn. Domain Service là HÀNH ĐỘNG xử lý dữ liệu đó.
- **Khác với Application Service:** Application Service làm nhiệm vụ Orchestration (mở giao dịch DB, gọi Repository, gọi API ngoài, gửi Email). Domain Service chỉ thuần tính toán và thay đổi trạng thái của các tham số đầu vào thuộc phạm vi Domain, nó không biết DB hay Email là gì.

---

# 8. Service Creation Order
Quy trình và thứ tự thiết kế (chỉ áp dụng khi vào giai đoạn Coding):
- **Step 1:** Phân tích nhu cầu nghiệp vụ, xác nhận logic không thể nhét vào Entity.
- **Step 2:** Định nghĩa Interface (nếu cần thiết cho việc Mock/Testing) hoặc trực tiếp định nghĩa Class thuần.
- **Step 3:** Triển khai phương thức nghiệp vụ với tham số đầu vào là các Entity/Value Object.
- **Step 4:** Áp dụng Guard Clauses kiểm tra hợp lệ.
- **Step 5:** Thực thi logic thay đổi trạng thái của các tham số đầu vào.
- **Step 6:** Validation & Freeze.

---

# 9. Service Evolution Strategy
Lộ trình phát triển của Domain Services qua các Sprint:
- **Sprint 03:** Thiết lập nguyên lý và quy tắc kiến trúc.
- **Sprint 04:** Thiết kế các Domain Service thực tế để điều phối giao dịch giữa các Entities cốt lõi (ví dụ: Tính toán bảo hiểm xã hội).
- **Sprint 05:** Không có sự thay đổi tại Domain Service, nhưng Infrastructure sẽ cung cấp Repository để Application Layer truyền dữ liệu vào Service.
- **Sprint 06:** Application Service gọi Domain Service thông qua tham chiếu thuần túy.

---

# 10. Domain Service Principles
- **Stateless (Phi trạng thái):** Không lưu trữ bất kỳ dữ liệu nào giữa các lần gọi hàm. Mọi dữ liệu cần thiết phải được truyền qua tham số.
- **Persistence Ignorance:** Không truy cập cơ sở dữ liệu. Không tiêm (inject) Repository Implementation. (Chỉ cho phép truyền Repository Interface qua tham số hàm nếu cực kỳ cần thiết cho Validation, nhưng ưu tiên truy xuất sẵn data từ Application Layer).
- **No Side Effects Outside Domain:** Không gọi API HTTP, không gửi message ra RabbitMQ/MediatR, không lưu File.

---

# 11. Service Classification Strategy
Phân loại các Domain Service thường gặp:
1. **Calculation Services (Tính toán):** Thực hiện các phép toán nghiệp vụ phức tạp dựa trên nhiều thông số từ các thực thể khác nhau (ví dụ: `PensionCalculationService`).
2. **Policy Enforcement Services (Thực thi chính sách):** Kiểm tra xem một tập hợp các điều kiện có thỏa mãn một quy định của tổ chức hay không (ví dụ: `DisbursementEligibilityService`).
3. **Cross-Aggregate Coordination Services (Điều phối đa Aggregate):** Xử lý luồng nghiệp vụ liên quan đến hai hay nhiều Aggregate Root độc lập (ví dụ: `FundsTransferService` giữa hai tài khoản).

---

# 12. Service Responsibilities
- **Khi nào DÙNG Domain Service:**
  - Một hành động nghiệp vụ liên quan đến nhiều Aggregate Root.
  - Phép tính toán phức tạp cần truy cập vào thông tin cấu hình miền hoặc nhiều Value Object mà Entity không nên tự gánh vác.
  - Các quy tắc xác thực (Business Rules) cần tra cứu chéo (cross-reference).
- **Khi nào KHÔNG DÙNG Domain Service:**
  - Hành động chỉ làm thay đổi trạng thái của một Entity duy nhất -> Đưa vào Entity.
  - Hành động thực hiện CRUD cơ bản -> Đưa vào Application Service / Repository.
  - Hành động gọi API bên ngoài (Payment Gateway) -> Thuộc về Infrastructure Service.

---

# 13. Service Collaboration Rules
- Domain Service hoạt động như một cỗ máy tính toán.
- Nó nhận đầu vào là các Aggregate Root, Value Object hoặc Domain Primitive.
- Nó thực thi logic và gọi trực tiếp các phương thức (Methods) của các Aggregate Root được truyền vào để cập nhật trạng thái của chúng.
- Nó có thể trả về một Domain Exception nếu quy tắc bị vi phạm (Fail Fast).

---

# 14. Aggregate Boundary Rules
- Domain Service là cầu nối hợp pháp duy nhất giữa các Aggregate Root độc lập bên trong tầng Domain.
- Tuy nhiên, Domain Service không được phép lưu (Save) trạng thái. Nó chỉ thay đổi trạng thái trong bộ nhớ (In-Memory State). Việc lưu vào cơ sở dữ liệu (UnitOfWork.Commit) là trách nhiệm của Application Layer, diễn ra SAU KHI Domain Service trả về kết quả.

---

# 15. Dependency Rules
Ràng buộc phụ thuộc cứng:

**Allowed Dependencies:**
- `System.*` (Các thư viện core C#).
- Các Building Blocks nội bộ: `Entity`, `Value Object`, `Domain Primitive`, `Domain Exception`.

**Forbidden Dependencies (Tuyệt đối cấm):**
- `EntityFrameworkCore`
- `Dapper`
- `MediatR`
- `ASP.NET`
- `ServiceLocator` hoặc `IServiceCollection` (DI Container).
- `Logging` (`ILogger`).
- Bất kỳ API HTTP hay CSDL nào.

---

# 16. Naming Convention
Quy tắc đặt tên bắt buộc:
- **Hậu tố:** Phải luôn kết thúc bằng từ `Service` hoặc một danh từ chỉ hành động rõ ràng.
- **Động từ/Danh từ nghiệp vụ:** Phản ánh đúng chức năng.

**Allowed (Cho phép):**
- `PensionCalculationService`
- `HouseholdTransferService`
- `FundAllocationManager` (Nếu phù hợp ngữ cảnh, dù `Service` ưu tiên hơn).

**Not Allowed (Tuyệt đối cấm):**
- `CitizenService` (Tên quá chung chung, giống CRUD Application Service).
- `DataProcessingService` (Tên mang tính kỹ thuật).
- Tên chứa hậu tố `Helper`, `Utility`.

---

# 17. Stateless Design Strategy
- Mọi biến thành viên (fields) của lớp Domain Service (nếu có) chỉ được phép là các thiết lập cấu hình bất biến (Configuration Parameters) hoặc các Interface hợp lệ (như `IRepository` - nếu có quy định tiêm qua constructor, tuy nhiên ưu tiên tiêm qua tham số Method để giữ Service 100% thuần túy).
- Không được phép khai báo bất kỳ biến trạng thái nào thay đổi giá trị trong quá trình thực thi hàm.

---

# 18. Validation Strategy
- Các tham số truyền vào Domain Service phải được làm sạch từ trước (Sử dụng Domain Primitives).
- Trong trường hợp nhận vào các Entity, Service phải áp dụng Guard Clauses để kiểm tra null.
- Service chịu trách nhiệm thực thi các Validation liên đới (Cross-entity Validation) mà một Entity không thể tự biết. Mọi sự vi phạm phải dẫn đến `DomainException`.

---

# 19. Transaction Boundary Rules
- Domain Service hoàn toàn mù tịt (Ignorant) về Giao dịch cơ sở dữ liệu (Database Transaction).
- Nó không được mở, đóng, commit hay rollback transaction. Trách nhiệm đó nằm ngoài ranh giới của nó (ở Application Layer).

---

# 20. Domain Service Lifecycle
- Khởi tạo (Instantiation) bởi Application Layer hoặc DI Container.
- Gọi hàm thực thi (Execution) từ Application Service.
- Thay đổi trạng thái các Aggregate truyền vào.
- Trả về kết quả (hoặc void) và kết thúc nhiệm vụ. Thu gom rác (Garbage Collection) sẽ xử lý.

---

# 21. AI Coding Constraints
- **CẤM** sinh mã C# dưới bất kỳ hình thức nào.
- **CẤM** sinh Pseudo Code hay JSON/XML/UML.
- **CẤM** định nghĩa các thao tác I/O.
- **CẤM** thiết kế các hàm trả về HTTP Response.
- **CẤM** khai báo DbContext hay bất kỳ thư viện bên thứ 3 nào.

---

# 22. Definition of Done
Tài liệu đạt tiêu chuẩn hoàn thành khi:
- Làm rõ ranh giới, trách nhiệm và thời điểm sử dụng Domain Service.
- Giải thích triệt để sự khác biệt với Entity, Value Object, Primitive và Application Service.
- Đảm bảo 100% Stateless và Persistence Ignorance.
- Sẵn sàng làm kim chỉ nam để AI sinh code đúng kiến trúc trong các Sprint sau.

---

# 23. Validation Matrix
Bảng ma trận tự kiểm tra sự tuân thủ kiến trúc:

| Tiêu chí | Nội dung kiểm tra | Đánh giá |
|---|---|---|
| **Statelessness** | Không giữ bất kỳ State nào sau khi method return. | Bắt buộc. |
| **No Infrastructure** | Tuyệt đối không Logging, DB, HTTP, MediatR. | Bắt buộc. |
| **No CRUD** | Không làm nhiệm vụ Lưu/Xóa (Save/Delete). | Bắt buộc. |
| **Business Logic** | Xử lý logic thuần túy giữa nhiều Aggregate. | Bắt buộc. |
| **Fail Fast** | Văng `DomainException` nếu nghiệp vụ lỗi. | Bắt buộc. |

---

# 24. Risks
- **Lạm dụng Domain Service (Anemic Domain Model):** Gom tất cả logic vào Service và biến Entity thành một tập hợp Getter/Setter vô hồn.
  - *Giải pháp:* Chỉ dùng Domain Service khi logic liên quan đến đa Aggregate hoặc các tính toán quá phức tạp. Hãy cố gắng đẩy hành vi về lại Entity nếu có thể.
- **Trộn lẫn với Application Service:** Developer nhét logic gửi Email hoặc commit UnitOfWork vào Domain Service.
  - *Giải pháp:* Giới hạn Dependency cấm tuyệt đối các thư viện I/O.
- **Phụ thuộc Service Locator:** Sử dụng DI container bừa bãi trong tầng Domain để resolve các service.
  - *Giải pháp:* Tiêm (Inject) qua tham số Method hoặc Constructor một cách tường minh, cấm dùng `IServiceProvider`.

---

# 25. AI Review Checklist
- [x] Không sinh C#, Pseudo Code, UML, XML, JSON?
- [x] Đã thiết lập Architecture Freeze Rule?
- [x] Đã phân biệt rõ ràng Domain Service với Entity, VO, Primitive, App Service?
- [x] Đã thiết lập Naming Convention và Stateless Design Strategy?
- [x] Đã tuân thủ nghiêm ngặt Dependency Rules (Cấm EF Core, HTTP, DI Container)?
- [x] Đã quy định rõ Khi nào DÙNG và Khi nào KHÔNG DÙNG?
- [x] Tài liệu tuân thủ chuẩn Enterprise Documentation Suite?

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
- 34_SPRINT_03_DOMAIN_PRIMITIVES.md

---
# END OF SPECIFICATION
