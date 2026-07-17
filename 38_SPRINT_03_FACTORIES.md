# 38_SPRINT_03_FACTORIES.md
## SPRINT 03 – FACTORY PATTERN ARCHITECTURE
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 38_SPRINT_03_FACTORIES
- **Title:** Domain Factories Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Factories |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Factory Pattern Architecture.
- **KHÔNG ĐƯỢC** vi phạm nguyên tắc "Factory chỉ tạo Aggregate hợp lệ".
- **KHÔNG ĐƯỢC** nhúng bất kỳ logic hạ tầng (Infrastructure) hay truy xuất dữ liệu nào vào Factory.
- Mọi sự thay đổi về triết lý thiết kế Factory bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là Đặc tả Kiến trúc (Architecture Specification) toàn diện xác định chiến lược thiết kế mẫu Factory (Nhà máy) trong kiến trúc Domain-Driven Design (DDD). Mục tiêu cốt lõi là định nghĩa Factory như một cơ chế khởi tạo phức tạp để đảm bảo mọi Aggregate Root được sinh ra đều thỏa mãn trọn vẹn các Invariants (bất biến) của hệ thống ngay từ giây phút đầu tiên.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất của Factory trong DDD.
- Quy định các giới hạn cứng: Factory không làm gì, được làm gì.
- Thiết lập ranh giới trách nhiệm, quy tắc tương tác với các Building Blocks khác.
- Định hình cấu trúc thư mục, quy tắc đặt tên, và chiến lược phân loại Factory.
- Xác định các rủi ro kiến trúc và Anti-Patterns.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C# (như `public class`, `new()`).
- KHÔNG sinh mã IoC/DI container setup.
- KHÔNG sinh SQL, UML, XML, JSON hay Pseudo Code.
- KHÔNG giải thích chi tiết tầng Infrastructure hay Application.

---

# 6. Objectives
- **Bảo vệ Invariants (Bảo vệ tính bất biến):** Không cho phép tạo ra các đối tượng (Aggregate) ở trạng thái rác (Invalid State).
- **Đóng gói Khởi tạo (Encapsulate Creation):** Giấu đi sự phức tạp của việc lắp ráp các Entity và Value Object lại thành một Aggregate Root hoàn chỉnh.
- **Tách biệt Trách nhiệm (Separation of Concerns):** Aggregate tập trung vào việc quản lý vòng đời và business rules, trong khi Factory gánh vác việc "sinh ra" nó.
- **Mù tịt về Lưu trữ (Persistence Ignorance):** Factory chỉ tạo đối tượng trên RAM, không dính líu đến CSDL.

---

# 7. Factory Pattern Architecture
**Bản chất Kiến trúc (Architectural Essence):**
- **Factory trong DDD là gì:** Factory là một khái niệm miền (Domain Concept) được trừu tượng hóa để xử lý việc lắp ráp một đối tượng hoặc một Aggregate phức tạp. Khi quá trình tạo mới một Aggregate đòi hỏi quá nhiều quy tắc, dữ liệu, hoặc sự liên kết giữa các Entity con, Factory sẽ đứng ra chịu trách nhiệm thay cho một hàm Constructor cồng kềnh.
- **Factory KHÔNG PHẢI là Constructor:** Constructor (Hàm tạo) thường chỉ nên thực hiện gán các thuộc tính đơn giản. Factory thực thi cả một quy trình lắp ráp (assembly process).
- **Factory KHÔNG PHẢI là Builder:** Builder Pattern dùng để tạo đối tượng theo từng bước (Step-by-step). Factory Pattern trong DDD tạo đối tượng trong một thao tác duy nhất mang tính nguyên tử (Atomic creation).
- **Factory KHÔNG PHẢI là Repository:** Factory tạo ra một Aggregate *mới tinh*. Repository lấy ra một Aggregate *đã tồn tại* từ ổ cứng.
- **Factory KHÔNG PHẢI là Service:** Domain Service chứa Business Rules vận hành. Factory không thực thi quy trình nghiệp vụ (Business Process), nó chỉ thực thi quy trình khởi tạo (Creation Process).
- **Factory KHÔNG PHẢI là Dependency Injection (DI):** Không nhầm lẫn Domain Factory với Service Locator hay AbstractFactory của IoC Container.

---

# 8. Factory Responsibilities
Những trách nhiệm TUYỆT ĐỐI của Factory:
- **Tạo Aggregate Hợp lệ:** Đảm bảo đối tượng được trả về từ Factory luôn luôn thỏa mãn mọi Domain Invariants (Tính nhất quán).
- **Khởi tạo Thực thể Con (Child Entities):** Tạo và gắn kết chính xác các Entity con hoặc Value Object vào Aggregate Root.
- **Cấp phát Định danh (Identity Generation):** Có thể phối hợp với các thuật toán sinh ID để định danh đối tượng (nếu không dùng cơ chế ID của DB).

Những thứ Factory **TUYỆT ĐỐI KHÔNG LÀM:**
- Không Validate Business Rules phức tạp vượt quá phạm vi khởi tạo.
- Không Save Database (Lưu dữ liệu).
- Không gọi Repository (VD: Không kiểm tra Email trùng lặp bằng cách gọi DB, đó là việc của Domain Service).
- Không gọi Commit hay quản lý Transaction.
- Không gửi Request HTTP.
- Không phụ thuộc Entity Framework Core.
- Không phụ thuộc Infrastructure hay ASP.NET.
- Không phụ thuộc SQL hay bất kỳ ORM nào.

---

# 9. Relationship With Aggregate Root
- **Độc quyền Khởi tạo:** Factory tồn tại duy nhất để phục vụ Aggregate Root. Nếu quá trình tạo Aggregate phức tạp, nó sẽ được ủy quyền cho Factory.
- **Bảo vệ Trạng thái:** Factory đảm bảo Aggregate Root không bao giờ bị lộ các hàm Setters công khai chỉ để phục vụ việc khởi tạo. Mọi trạng thái hợp lệ phải được đưa vào từ Factory.
- **Tính trọn vẹn (Wholeness):** Factory luôn trả về một Aggregate Root hoàn chỉnh, cấm trả về một phần của Aggregate.

---

# 10. Relationship With Repository
- **Phân định rạch ròi:**
  - Factory: Tạo một Aggregate từ HƯ KHÔNG (New up).
  - Repository: Tái tạo (Reconstitute) một Aggregate từ CƠ SỞ DỮ LIỆU.
- **Không chồng chéo:** Factory tuyệt đối không tiêm (inject) Repository vào bên trong nó. Việc kiểm tra tính duy nhất (ví dụ: Citizen ID đã tồn tại chưa) phải được thực hiện ở Domain Service trước khi gọi Factory, hoặc ở Application Service.

---

# 11. Relationship With Domain Service
- **Nguồn cung cấp:** Domain Service có thể gọi Factory để lấy ra một Aggregate mới sau khi đã hoàn tất các quy trình nghiệp vụ kiểm duyệt.
- **Không thay thế nhau:** Domain Service chứa luồng nghiệp vụ. Factory chỉ có luồng lắp ráp bộ phận. Chúng là hai Building Blocks hoàn toàn riêng biệt.

---

# 12. Relationship With Value Object
- **Lắp ghép tĩnh:** Factory nhận các primitive types (chuỗi, số) hoặc chính các Value Object từ bên ngoài và lắp ghép chúng vào bên trong Aggregate Root.
- **Creation Rule:** Value Object thường quá đơn giản để cần đến một Factory riêng. Factory chủ yếu sinh ra để phục vụ Aggregate.

---

# 13. Relationship With Domain Events
- **Kích hoạt Sự kiện:** Khi Factory tạo ra một Aggregate Root thành công, bản thân Aggregate Root (ngay trong hàm tạo hoặc từ Factory) có thể ghi nhận một `DomainEvent` (ví dụ: `CitizenCreatedEvent`).
- Factory không tự mình Publish/Dispatch Event. Nó chỉ gắn Event vào danh sách sự kiện nội bộ của Aggregate Root.

---

# 14. Factory Creation Strategy
Trình tự chiến lược tạo Factory trong quá trình phát triển (Coding):
- **Step 1:** Xác định mức độ phức tạp của Aggregate. Nếu Constructor vượt quá 5 tham số hoặc cần logic chằng chịt, hãy cân nhắc Factory.
- **Step 2:** Quyết định phân loại Factory (Factory Method trên chính Aggregate, hay một Class Factory độc lập).
- **Step 3:** Khai báo Factory Contract (nếu cần DI) hoặc Static Method.
- **Step 4:** Cài đặt logic lắp ráp, khởi tạo các Value Object, List Entity con.
- **Step 5:** Rà soát lại với Validation Rules để đảm bảo không rò rỉ cơ sở dữ liệu.

---

# 15. Factory Evolution Strategy
Lộ trình kiến trúc của Factory qua các Sprint:
- **Sprint 03:** Thiết lập Architecture Specification. Định hình tư duy khởi tạo.
- **Sprint 04:** Thiết kế các Factory Methods cơ bản nằm tĩnh bên trong các Aggregate Root.
- **Sprint 05:** Khai báo các Factory Classes phức tạp (nếu hệ thống cần tạo đối tượng với đồ thị sâu).
- **Sprint 06:** Ứng dụng Unit Testing nghiêm ngặt để kiểm định tính trọn vẹn của Aggregate sinh ra từ Factory.
- **Sprint 07:** Tối ưu hóa bộ nhớ, hạn chế cấp phát rác (Garbage Collection) khi Factory chạy ở cường độ cao.

---

# 16. Factory Principles
Các nguyên lý thiết kế tối thượng của Factory:
- **Atomic Creation:** Quá trình tạo đối tượng phải là một khối nguyên tử: Hoặc là có một Aggregate hợp lệ hoàn toàn, hoặc ném ra Exception (không có trạng thái nửa vời).
- **Intention-Revealing:** Tên của Factory hoặc phương thức Factory phải nói rõ được BỐI CẢNH (Context) của việc tạo mới.
- **Side-Effect Free:** Factory cấm tuyệt đối việc gây ra các phản ứng phụ ra ngoài hệ thống (như ghi file, ghi log xuống DB, gửi email). Nó là một hàm tinh khiết (Pure) về mặt kiến trúc.

---

# 17. Factory Classification
Phân loại kiến trúc Factory trong Enterprise DDD:
- **Factory Method (Tĩnh):** Nằm ngay bên trong Aggregate Root. Được ưu tiên dùng cho hầu hết các Use Case. (VD: `Citizen.CreateNew()`).
- **Factory Class (Độc lập):** Một class riêng biệt tại tầng Domain (VD: `CitizenFactory`). Dùng khi quá trình tạo yêu cầu phải khởi tạo hàng chục Value Object phức tạp và việc để trong Aggregate Root sẽ vi phạm Single Responsibility Principle (SRP).
- **Reconstitution Factory:** Một dạng Factory đặc biệt dùng cho Repository để tái tạo Aggregate từ dữ liệu DB (thường được ORM như EF Core tự xử lý, Domain hiếm khi phải tự viết, ngoại trừ trường hợp ánh xạ quá dị biệt).

---

# 18. Aggregate Creation Rules
Các quy định cứng về việc "Sinh Thành" (Creation):
- **Quy tắc Tự Tôn:** Không được phép new `Entity` con từ bên ngoài và tiêm (inject) vào Aggregate Root bằng properties.
- **Lắp ráp từ Gốc:** Factory của Aggregate Root phải là người duy nhất nắm quyền gọi constructor của các Entity con (hoặc ủy quyền ngầm).
- **Trạng thái Mặc định:** Factory phải thiết lập trạng thái khởi điểm của một Aggregate theo đúng quy trình nghiệp vụ (ví dụ: `Status = Pending`).

---

# 19. Collaboration
Sự phối hợp kiến trúc:
- **Application Layer:** Application Layer gọi Factory với các DTO parameters nguyên thủy, nhận lại một Aggregate Root, và quăng nó cho Repository để lưu trữ.
- **Specification:** Factory hoàn toàn KHÔNG biết đến Specification. Specification dùng để lọc, Factory dùng để sinh.
- **Infrastructure:** Factory mù hạ tầng. Không có DbContext hay HttpClient lảng vảng quanh Factory.

---

# 20. Naming Convention
Quy tắc đặt tên hợp đồng và class tại tầng Domain:
- **Class Factory độc lập:** Bắt buộc có hậu tố `Factory` (VD: `CitizenFactory`, `PensionPolicyFactory`).
- **Factory Method trên Aggregate:** Bắt buộc sử dụng các động từ chỉ rõ ý định.
  - Được dùng: `Create()`, `CreateNew()`, `Register()`, `Issue()`.
  - KHÔNG dùng: `Build()`, `Init()`, `New()`.

---

# 21. Folder Strategy
Chiến lược tổ chức cấu trúc thư mục của Factory tại tầng Domain:
- `Factories/` (Chỉ chứa các Factory Class độc lập phức tạp).
  - `Modules/`
    - `Demographic/`
    - `SocialSecurity/`
- *(Lưu ý 1: Nếu dùng Factory Method, chúng sẽ nằm ngay trong file của Aggregate Root).*
- *(Lưu ý 2: Chỉ thiết lập định hướng thư mục, không sinh source code tại đây).*

---

# 22. Dependency Rules
Ràng buộc phụ thuộc cứng cho Factory tại tầng Domain:

**Allowed Dependencies:**
- Tầng Domain nội tại (Aggregate Roots, Entities, Value Objects, Domain Exceptions, Domain Events).
- `System.*` (Thư viện cốt lõi).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `Repositories` (Factory không được phép truy xuất kho lưu trữ).
- `EF Core` / `Dapper` / `SQL`.
- `ASP.NET` / `HttpClient` / `SignalR`.
- `Logging Frameworks` (Tránh làm đục Domain).
- Bất kỳ thứ gì thuộc về `Infrastructure` hay `Application`.

---

# 23. Validation Rules
Một Factory được coi là hợp lệ (Valid) trong Domain khi nó tuân thủ:
- **Invariant Guarantee:** Luôn luôn đảm bảo đối tượng tạo ra đúng quy tắc miền.
- **No Persistence:** Cấm tuyệt đối thao tác lưu DB.
- **No External Call:** Cấm tuyệt đối thao tác gọi API ngoài.
- **Encapsulated Construction:** Không rò rỉ các thành phần "đang xây dựng dở dang" (half-baked state) ra ngoài.

---

# 24. Performance Strategy
Các chiến lược hiệu suất cho Factory:
- **Minimal Allocation:** Tránh tạo rác bộ nhớ (Memory Allocation) trong quá trình parse tham số ở Factory.
- **No I/O Blocking:** Vì Factory không gọi DB hay API, nó phải hoạt động hoàn toàn trên RAM và phản hồi trong thời gian microsecond.
- **Lazy Initialization Guard:** Factory không được dùng Lazy Load hay Proxy. Nó phải trả về Aggregate được tạo nguyên khối.

---

# 25. AI Coding Constraints
Giới hạn hành vi bắt buộc của AI Coding Agent khi đọc hiểu tài liệu này:
- KHÔNG tạo bất kỳ file C# nào triển khai (`class`) cho các Factory tại đây.
- KHÔNG sinh mã JSON, XML, UML, hay Pseudo Code minh họa.
- Không gài bẫy AI bằng cách chèn Repository vào Factory trong tương lai.
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 26. Definition Of Done
Tài liệu được định nghĩa là hoàn tất (Done) khi:
- **Factory Architecture hoàn chỉnh:** Ranh giới giữa Factory, Repository và Constructor đã được phân định sắc nét.
- **DDD Compliance:** Triết lý tạo Invariants được đề cao tuyệt đối.
- **Clean Architecture Compliance:** Không rò rỉ Infrastructure.
- **Enterprise Ready:** Phân loại rõ Factory Class và Factory Method.
- **AI Ready:** Giới hạn cực rõ những gì cấm làm.
- **Freeze Ready:** Đạt chuẩn để Project Owner khóa kiến trúc.

---

# 27. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **No Persistence** | Không gọi Save, Insert, Update, Delete. | Bắt buộc (Mandatory) |
| **No I/O Operations** | Không gọi File System, Mạng, HTTP. | Bắt buộc (Mandatory) |
| **Invariant Compliance**| Đảm bảo Aggregate tạo ra là toàn vẹn logic. | Bắt buộc (Mandatory) |
| **Atomic Creation** | Tạo thành công 100% hoặc văng Domain Exception. | Bắt buộc (Mandatory) |
| **No Repository Inject**| Factory không nhận Interface của Repository. | Bắt buộc (Mandatory) |
| **Infrastructure Free** | Không phụ thuộc EF, SQL, hay Framework ngoài. | Bắt buộc (Mandatory) |

---

# 28. Risks
Cảnh báo các rủi ro kiến trúc (Architectural Risks):
- **Complex Creation Leakage:** Đẩy logic tạo Aggregate phức tạp ra Application Layer khiến Application dính chặt vào chi tiết của Domain.
- **Anaemic Factory:** Factory chỉ gán thuộc tính như 1 Constructor tẻ nhạt mà không thắt chặt Invariants.
- **Coupled Factory:** Khóa cứng Factory vào Database bằng cách cho Factory đi kiểm tra tính duy nhất (Uniqueness Check).

---

# 29. Architectural Anti Patterns
Các mẫu phản kiến trúc (Anti-Patterns) thường gặp:

- **The Database-Aware Factory:**
  - Tiêm thẳng `DbContext` hoặc `IRepository` vào Factory để kiểm tra trùng lặp email/username trước khi tạo đối tượng. Bóp chết tính In-memory của Domain.
- **The Half-Baked Aggregate:**
  - Factory trả về một Aggregate thiếu thuộc tính quan trọng và yêu cầu Application Layer phải gọi hàm `Init()` hoặc gán Setter ngay sau đó.
- **The Application Layer Factory:**
  - Tầng Application tự new Aggregate, tự new List Entity con, tự ép kiểu rồi gán. Phá vỡ hoàn toàn Encapsulation.
- **The Service Locator Factory:**
  - Factory tự động resolve các thư viện ngoài bằng Service Locator (`IServiceProvider`) làm hỏng thiết kế Testability.
- **The Over-Engineered Abstract Factory:**
  - Tạo ra hàng loạt `IFactory`, `AbstractFactory` rườm rà như Java EE cũ trong khi chỉ cần một Static Method `Create()` trên Aggregate là đủ.

---

# 30. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã giải thích cặn kẽ khái niệm Factory theo chuẩn DDD?
- [x] Đã thiết lập bức tường lửa chặn Factory truy xuất DB (No Repository)?
- [x] Đã mô tả rõ ràng sự phối hợp và khác biệt giữa Factory, Constructor, Builder, Service?
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

---
# END OF SPECIFICATION
