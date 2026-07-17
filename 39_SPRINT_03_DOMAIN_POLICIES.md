# 39_SPRINT_03_DOMAIN_POLICIES.md
## SPRINT 03 – DOMAIN POLICY ARCHITECTURE
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 39_SPRINT_03_DOMAIN_POLICIES
- **Title:** Domain Policies Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Policies |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Domain Policy Architecture.
- **KHÔNG ĐƯỢC** vi phạm nguyên tắc "Policy là đối tượng thuần túy của Domain, không chứa hạ tầng".
- **KHÔNG ĐƯỢC** rò rỉ bất kỳ công nghệ lưu trữ nào (SQL, EF Core) vào Domain Policy.
- Mọi sự thay đổi về triết lý thiết kế Policy bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là Đặc tả Kiến trúc (Architecture Specification) toàn diện xác định chiến lược thiết kế mẫu Domain Policy (Chính sách Miền) trong kiến trúc Domain-Driven Design (DDD). Mục tiêu cốt lõi là định nghĩa cách thức tách biệt và đóng gói các quy tắc nghiệp vụ phức tạp, hay biến đổi thành các đối tượng Policy, giúp giữ cho Aggregate Root và Domain Service không bị phình to bởi các logic tính toán cồng kềnh.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất của Domain Policy trong hệ sinh thái DDD.
- Sự phân biệt rõ ràng giữa Policy và Domain Service, Specification, Factory.
- Thiết lập ranh giới trách nhiệm, quy tắc tương tác với các Building Blocks khác.
- Định hình cấu trúc thư mục, quy tắc đặt tên, và chiến lược phân loại Policy.
- Xác định các rủi ro kiến trúc và Anti-Patterns.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C# (như `class`, `interface`).
- KHÔNG sinh mã logic thuật toán.
- KHÔNG sinh SQL, UML, XML, JSON hay Pseudo Code.
- KHÔNG giải thích chi tiết tầng Infrastructure hay Application.

---

# 6. Objectives
- **Đóng gói Quy tắc Chuyên biệt (Encapsulate Rules):** Đưa các quy tắc tính toán phức tạp, dễ thay đổi (như tính thuế, tính lương hưu) ra khỏi Entity.
- **Tăng tính Mở rộng (Open/Closed Principle):** Cho phép dễ dàng thêm các phiên bản Policy mới (như `NewYearDiscountPolicy`) mà không phải sửa đổi mã nguồn của Aggregate.
- **Tái sử dụng (Reusability):** Một Policy có thể được gọi từ nhiều Domain Service, Aggregate, hoặc Application Service khác nhau.
- **Mù tịt về Lưu trữ (Persistence Ignorance):** Policy là thuật toán bộ nhớ thuần túy (In-Memory Algorithm), không dính líu đến CSDL.

---

# 7. Domain Policy Architecture
**Bản chất Kiến trúc (Architectural Essence):**
- **Domain Policy trong DDD là gì:** Policy (thường được thiết kế theo Strategy Pattern) là một khái niệm miền dùng để đóng gói một "quyết định nghiệp vụ" (Business Decision) hoặc "thuật toán nghiệp vụ" (Business Algorithm) phức tạp. Nó nhận đầu vào là các tham số, Value Object, hoặc Entity, và trả về một kết quả (số tiền, điểm số, mức ưu tiên, hành động tiếp theo).
- **Policy KHÔNG PHẢI là Domain Service:** Domain Service dùng để điều phối (Orchestrate) một quy trình nghiệp vụ tác động lên nhiều Aggregate. Policy chỉ thực hiện tính toán tĩnh (Calculations/Decisions) và không thay đổi trạng thái hệ thống.
- **Policy KHÔNG PHẢI là Specification:** Specification chỉ trả về `true/false` (Yes/No) để lọc hoặc kiểm tra điều kiện. Policy có thể trả về một đối tượng, một con số, hoặc một cấu trúc dữ liệu mô tả kết quả của việc ra quyết định.
- **Policy KHÔNG PHẢI là Factory:** Factory sinh ra một Object mới. Policy phân tích trạng thái và đưa ra quyết định hoặc tính toán.
- **Policy KHÔNG PHẢI là Business Rule gốc rễ:** Business Rule là khái niệm trên giấy (Ubiquitous Language). Domain Policy là việc hiện thực hóa (Implementation) của một Business Rule phức tạp, có tính chất dễ thay đổi (Volatile). Các rule đơn giản (VD: Name không được null) nằm thẳng trong Entity.

---

# 8. Responsibilities
Những trách nhiệm TUYỆT ĐỐI của Domain Policy:
- **Thực thi Thuật toán Miền (Execute Domain Algorithm):** Tính toán ra các giá trị mang ý nghĩa nghiệp vụ (ví dụ: `CalculatePensionAmount()`).
- **Ra Quyết định (Make Decision):** Xác định chiến lược áp dụng (ví dụ: `DetermineEligibilityLevel()`).
- **Độc lập Trạng thái (Stateless):** Nhận tham số đầu vào, trả về kết quả. Không lưu giữ trạng thái nội tại (No side-effects).

Những thứ Domain Policy **TUYỆT ĐỐI KHÔNG LÀM:**
- Không truy xuất CSDL (Không Database, Không Repository).
- Không gọi HTTP/API ngoài.
- Không thay đổi (Mutate) trạng thái của đối tượng truyền vào. Nó chỉ trả về kết quả tính toán.
- Không phát ra Domain Events (Nhiệm vụ này của Aggregate hoặc Domain Service).
- Không quản lý Transaction hay Unit Of Work.

---

# 9. Relationship With Aggregate
- **Người phục vụ:** Aggregate có thể nhận Policy làm tham số trong hàm của nó để tự tính toán (Double Dispatch). VD: `citizen.CalculatePension(IPensionPolicy policy)`.
- **Giảm tải:** Policy giúp Aggregate (Root) không trở thành God Class bằng cách bóc tách hàng nghìn dòng code if/else của các chính sách ra khỏi Aggregate.
- **Tính trong suốt:** Aggregate không cần biết chi tiết Policy chạy thế nào, chỉ quan tâm đến Interface của Policy.

---

# 10. Relationship With Repository
- **Phân định rạch ròi:** Policy và Repository hoàn toàn tách biệt. Policy tính toán logic trên bộ nhớ. Repository kéo dữ liệu từ ổ cứng.
- **Không bao giờ Inject:** CẤM TUYỆT ĐỐI việc tiêm (inject) Repository vào Policy. Nếu Policy cần dữ liệu, Application Layer phải dùng Repository lấy dữ liệu đó ra trước, rồi truyền vào cho Policy.

---

# 11. Relationship With Factory
- **Khác biệt hoàn toàn:** Factory tạo đối tượng. Policy đưa ra quyết định.
- **Phối hợp:** Trong một số trường hợp, Factory có thể sử dụng Policy để quyết định xem đối tượng được khởi tạo với trạng thái nào, hoặc Policy có thể yêu cầu Factory tạo ra một đối tượng kết quả. Tuy nhiên, chúng là hai khái niệm song song.

---

# 12. Relationship With Domain Service
- **Bộ máy tính toán của Service:** Domain Service thường là nơi điều phối quy trình. Khi gặp một quyết định phức tạp, Domain Service sẽ gọi đến Domain Policy để xin kết quả, sau đó áp dụng kết quả này cho quy trình của mình.
- **Ví dụ:** Domain Service chịu trách nhiệm cấp phát trợ cấp (Disbursement). Nó sẽ gọi `EligibilityPolicy` để xem công dân có thỏa mãn không, sau đó gọi `CalculateAmountPolicy` để lấy số tiền, rồi tiến hành các bước cập nhật Aggregate.

---

# 13. Relationship With Specification
- **So sánh trực diện:** Specification là tập con về mặt ý nghĩa của Policy. Specification trả về Boolean (Predicate). Policy trả về Data/Action.
- **Kết hợp:** Một Policy phức tạp có thể tái sử dụng nhiều Specification bên trong nó để đưa ra kết luận cuối cùng.

---

# 14. Relationship With Domain Events
- **Không gửi Event:** Policy hoàn toàn tĩnh và Functional (Hàm thuần túy). Nó không kích hoạt sự kiện.
- **Làm cơ sở tạo Event:** Kết quả từ Policy sẽ được Aggregate hoặc Domain Service sử dụng. Nếu kết quả làm thay đổi trạng thái, Aggregate sẽ là người kích hoạt Domain Event.

---

# 15. Creation Strategy
Trình tự chiến lược tạo Policy trong quá trình phát triển (Coding):
- **Step 1:** Nhận diện một quy tắc nghiệp vụ phức tạp, dễ thay đổi theo thời gian (ví dụ: Thuế, Lương hưu, Khuyến mãi, Phân cấp).
- **Step 2:** Định nghĩa Interface (hợp đồng) của Policy tại tầng Domain (VD: `ITaxPolicy`).
- **Step 3:** Thiết kế các Input (tham số, Value Object) và Output (thường là một Value Object chứa kết quả).
- **Step 4:** Cài đặt các class thực thi cụ thể (VD: `StandardTaxPolicy`, `VIPTaxPolicy`).
- **Step 5:** Validation (Kiểm tra lại xem Policy có vô tình dính đến thư viện ngoài hay không).

---

# 16. Evolution Strategy
Lộ trình kiến trúc của Domain Policy qua các Sprint:
- **Sprint 03:** Thiết lập Architecture Specification, phân rõ giới tuyến với Specification và Domain Service.
- **Sprint 04:** Định hình các giao diện cơ bản của Policy cho các nghiệp vụ lõi (VD: Hộ khẩu, Trợ cấp).
- **Sprint 05:** Bổ sung Policy Factory hoặc Strategy Resolver tại Application Layer để tiêm Policy động vào Domain.
- **Sprint 06:** Ứng dụng Unit Testing diện rộng. Policy là đối tượng dễ test nhất vì nó Stateless và Pure.
- **Sprint 07:** Tích hợp Rules Engine (nếu Policy thay đổi quá nhanh chóng, có thể cần đẩy logic ra ngoài Engine, Domain Policy sẽ hoạt động như adapter chuẩn hóa).

---

# 17. Principles
Các nguyên lý thiết kế tối thượng của Domain Policy:
- **Pure Function (Hàm Tinh Khiết):** Kết quả của Policy chỉ phụ thuộc duy nhất vào tham số đầu vào. Không phụ thuộc vào trạng thái ẩn bên ngoài.
- **Open/Closed Principle (OCP):** Kiến trúc phải cho phép bổ sung Policy mới (class mới) mà không phải sửa các Policy cũ hay Aggregate.
- **Strategy Pattern Base:** Đại đa số các Domain Policy được cài đặt thông qua Strategy Pattern, giúp thay đổi thuật toán tại Runtime.

---

# 18. Classification
Phân loại kiến trúc Policy trong Enterprise DDD:
- **Calculation Policy:** Tính toán và trả về một giá trị tài chính, điểm số, hoặc thời gian. (VD: `OvertimeCalculationPolicy`).
- **Decision/Routing Policy:** Đưa ra quyết định điều hướng nghiệp vụ. (VD: `ApprovalWorkflowRoutingPolicy`).
- **Classification Policy:** Phân loại một đối tượng vào một nhóm nhất định. (VD: `CitizenCategoryClassificationPolicy`).

---

# 19. Collaboration
Sự phối hợp kiến trúc:
- **Application Layer:** Khởi tạo hoặc Resolve Policy thích hợp từ DI, truyền nó vào Domain Service hoặc Aggregate Root.
- **Domain Layer:** Chứa Interface và Implementation cốt lõi của các Policy.
- **Infrastructure:** Mù hoàn toàn với Policy. Có thể chứa cấu hình để ánh xạ Policy nào được dùng (qua DI), nhưng logic không nằm ở đây.

---

# 20. Naming Convention
Quy tắc đặt tên hợp đồng và class tại tầng Domain:
- **Interface:** Bắt buộc có hậu tố `Policy` hoặc `Strategy`. VD: `IDiscountPolicy`, `IRetirementEligibilityPolicy`.
- **Implementation:** Chứa từ khóa mô tả ngữ cảnh hoặc phiên bản + `Policy`. VD: `Year2026RetirementPolicy`, `VeteransDiscountPolicy`.
- **Phương thức:** Dùng các động từ chỉ tính toán hoặc quyết định. VD: `Calculate(...)`, `Evaluate(...)`, `Determine(...)`.

---

# 21. Folder Strategy
Chiến lược tổ chức cấu trúc thư mục của Policy tại tầng Domain:
- `Policies/`
  - `SeedWork/` (Các Base Policy Interface nếu cần).
  - `Modules/`
    - `Demographic/` (VD: `IdentityVerificationPolicy`).
    - `SocialSecurity/` (VD: `PensionCalculationPolicy`).
    - `Disbursement/` (VD: `PayoutRoutingPolicy`).
  - `Shared/` (Các Policy dùng chéo giữa các Module).

*(Lưu ý: Chỉ thiết lập định hướng thư mục, không sinh source code tại đây).*

---

# 22. Dependency Rules
Ràng buộc phụ thuộc cứng cho Policy tại tầng Domain:

**Allowed Dependencies:**
- Tầng Domain nội tại (Entities, Value Objects, Domain Primitives, Specifications).
- `System.*` (Thư viện cốt lõi).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `Repositories` (Policy không được phép truy xuất kho lưu trữ).
- `EF Core` / `Dapper` / `SQL`.
- `ASP.NET` / `HttpClient` / `SignalR`.
- `Logging Frameworks` (Tránh làm đục Domain, trừ khi trừu tượng qua interface rất mỏng, nhưng tốt nhất là không dùng).
- `Third-party API clients` (Việc gọi API ngoài thuộc về Infrastructure/Application).

---

# 23. Validation Rules
Một Domain Policy được coi là hợp lệ (Valid) trong Domain khi nó tuân thủ:
- **Stateless:** Không lưu giữ trạng thái giữa các lần gọi (No private fields with mutated state).
- **Side-Effect Free:** Cấm tuyệt đối thao tác lưu DB, gọi I/O, thay đổi đối tượng truyền vào (Nên truyền đối tượng dạng ReadOnly nếu có thể).
- **Domain Speaking:** Tham số truyền vào và kết quả trả về phải là Value Object hoặc Enum của Domain, không dùng `JObject` hay chuỗi vô nghĩa.

---

# 24. Performance Strategy
Các chiến lược hiệu suất cho Policy:
- **Micro-Optimization:** Vì Policy thường xuyên được gọi hàng vạn lần trong các vòng lặp xử lý (Batch Processing), thuật toán bên trong Policy phải tối ưu, sử dụng cấu trúc dữ liệu nhẹ.
- **No External Latency:** Vì cấm gọi I/O, Policy không bị ảnh hưởng bởi độ trễ mạng.
- **Flyweight Pattern:** Các đối tượng Policy thường Stateless, do đó có thể sử dụng cơ chế Singleton (hoặc tiêm DI as Singleton) để tái sử dụng instance, tiết kiệm bộ nhớ thay vì new liên tục.

---

# 25. AI Coding Constraints
Giới hạn hành vi bắt buộc của AI Coding Agent khi đọc hiểu tài liệu này:
- KHÔNG tạo bất kỳ file C# nào triển khai (`class`, `interface`) cho các Policy tại đây.
- KHÔNG sinh mã JSON, XML, UML, hay Pseudo Code minh họa.
- CẤM tiêm Repository vào Policy.
- CẤM sinh code cho thấy Policy có hành vi lưu dữ liệu (`Save`).
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 26. Definition Of Done
Tài liệu được định nghĩa là hoàn tất (Done) khi:
- **Policy Architecture hoàn chỉnh:** Đặc tả sắc bén sự khác biệt giữa Policy, Service, Specification.
- **DDD Compliance:** Triết lý Pure Function và OCP được bảo đảm.
- **Clean Architecture Compliance:** Ranh giới không lấn chiếm Infrastructure.
- **Enterprise Ready:** Phân loại và cấu trúc thư mục rõ ràng.
- **AI Ready:** Prompt rõ ràng, có constraints mạnh mẽ.
- **Coding Ready:** Sẵn sàng làm hướng dẫn cho Sprint Implementation.
- **Freeze Ready:** Đạt chuẩn để Project Owner khóa kiến trúc.

---

# 27. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **No Persistence** | Không gọi Save, Insert, Update, Delete. | Bắt buộc (Mandatory) |
| **No I/O Operations** | Không gọi File System, Mạng, HTTP. | Bắt buộc (Mandatory) |
| **Stateless** | Không giữ trạng thái private thay đổi được. | Bắt buộc (Mandatory) |
| **Side Effect Free** | Không sửa đổi tham số đầu vào. | Bắt buộc (Mandatory) |
| **No Repository Inject**| Policy không nhận Interface của Repository. | Bắt buộc (Mandatory) |
| **Domain speaking I/O** | Input/Output phải là thuật ngữ Domain. | Bắt buộc (Mandatory) |

---

# 28. Risks
Cảnh báo các rủi ro kiến trúc (Architectural Risks):
- **Overkill Policy:** Lạm dụng Policy cho những logic if/else quá đơn giản (như Name có trống không), dẫn đến Class Explosion.
- **Policy Dependencies Leak:** Truyền thẳng DTO của tầng Application hoặc Presentation vào Policy thay vì truyền Domain Entity/Value Object.
- **Mutable State Leak:** Thiết kế Policy dạng Stateful, khiến ứng dụng chạy đa luồng bị sai kết quả (Thread-safety issues).

---

# 29. Architectural Anti Patterns
Các mẫu phản kiến trúc (Anti-Patterns) thường gặp:

- **The Database-Polled Policy:**
  - Tiêm `IRepository` vào Policy để policy tự chui vào CSDL truy vấn mức thuế. Điều này khóa cứng Policy vào DB và giết chết hiệu năng tính toán batch.
- **The Mutating Policy:**
  - Hàm `Calculate()` của Policy không những tính ra số tiền mà còn tự động gán luôn `citizen.Balance = newBalance`. Policy đang lấn quyền quản lý trạng thái của Aggregate Root.
- **The Service Masquerading as Policy:**
  - Đặt tên là `ApproveLoanPolicy` nhưng bên trong lại gọi API của ngân hàng để check tín dụng và gửi email báo cáo (Đây phải là Domain Service hoặc Application Service).
- **The Swiss Army Knife Policy:**
  - Một file `GeneralBusinessPolicy` chứa đến 5000 dòng code if/else cho đủ mọi loại nghiệp vụ, vi phạm nghiêm trọng Single Responsibility Principle.
- **The Primitive Obsessed Policy:**
  - Hàm Policy nhận vào 15 tham số kiểu `int`, `string` thay vì gom chúng thành một `TaxCalculationContext` (Value Object).

---

# 30. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã giải thích cặn kẽ khái niệm Domain Policy, so sánh với Service, Specification, Factory?
- [x] Đã thiết lập bức tường lửa chặn Policy truy xuất CSDL (No Repository)?
- [x] Đã làm rõ tính Pure Function và Stateless của Policy?
- [x] Đã liệt kê chi tiết các rủi ro và Architectural Anti-Patterns?
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

---
# END OF SPECIFICATION
