# 37_SPRINT_03_REPOSITORIES.md
## SPRINT 03 – REPOSITORY PATTERN ARCHITECTURE
Version: 1.1.0
Status: Approved (Architecture Freeze)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 37_SPRINT_03_REPOSITORIES
- **Title:** Domain Repositories Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Repositories |
| 1.1.0 | 2026-07-17 | Solution Architecture Team | Expanded sections for Quality, Anti-Patterns, Dependencies, and Performance based on Enterprise standards. |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Repository Pattern Architecture.
- **KHÔNG ĐƯỢC** vi phạm nguyên tắc "1 Aggregate = 1 Repository".
- **KHÔNG ĐƯỢC** rò rỉ bất kỳ công nghệ lưu trữ nào (SQL, EF Core, NoSQL) vào tầng Domain.
- Mọi sự thay đổi về triết lý thiết kế Repository bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là Đặc tả Kiến trúc (Architecture Specification) toàn diện xác định chiến lược thiết kế mẫu Repository (Kho lưu trữ) trong kiến trúc Domain-Driven Design (DDD). Mục tiêu cốt lõi là định nghĩa Repository như một hợp đồng (Contract) nằm tại tầng Domain để cô lập hoàn toàn logic nghiệp vụ khỏi cơ chế lưu trữ (Persistence Mechanism), đảm bảo tính Persistence Ignorance và duy trì sự toàn vẹn của Aggregate.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất của Repository trong DDD và sự khác biệt với các mẫu truy cập dữ liệu truyền thống.
- Thiết lập ranh giới trách nhiệm, quy tắc tương tác với Aggregate, Specification và Unit of Work.
- Định hình cấu trúc thư mục, quy tắc đặt tên, và chiến lược tiến hóa của Repository.
- Xác định các rủi ro kiến trúc và Anti-Patterns.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C# (như `public interface`, `class`, `methods`).
- KHÔNG sinh Interface chung (Generic Repository Interface).
- KHÔNG sinh DbContext, Entity Framework configurations, hay Dapper mappings.
- KHÔNG sinh SQL, UML, XML, JSON hay Pseudo Code.
- KHÔNG giải thích chi tiết tầng Infrastructure.

---

# 6. Objectives
- **Bảo vệ Trọng tâm Miền (Domain Centricity):** Đảm bảo tầng Domain không có bất kỳ kiến thức nào về cơ sở dữ liệu.
- **Mô phỏng Bộ nhớ (In-Memory Illusion):** Tạo ra ảo giác cho tầng Domain rằng toàn bộ Aggregate đang được lưu trữ trong một Collection trên bộ nhớ (In-Memory Collection).
- **Ngăn chặn Rò rỉ Logic (Prevent Logic Leakage):** Đảm bảo Repository hoàn toàn KHÔNG chứa Business Rules, Validation hay Transaction Logic.
- **Quản lý Vòng đời (Lifecycle Management):** Đảm bảo Repository chỉ chịu trách nhiệm cung cấp và lưu giữ các Aggregate Root ở trạng thái toàn vẹn nhất.

---

# 7. Repository Pattern Architecture
**Bản chất Kiến trúc (Architectural Essence):**
- **Repository trong DDD là gì:** Repository là một ranh giới kiến trúc đóng vai trò như một bộ sưu tập (Collection) chứa các đối tượng Aggregate Root. Đối với tầng Domain, Repository trông giống như một danh sách (List) lưu trên RAM, nơi có thể lấy ra (Find), thêm vào (Add) hoặc gỡ bỏ (Remove) Aggregate Root.
- **Repository KHÔNG PHẢI là DAO (Data Access Object):** DAO tập trung vào việc map trực tiếp 1-1 với các bảng trong Database (Table-driven). Repository tập trung vào việc quản lý vòng đời của một Aggregate (Domain-driven).
- **Repository KHÔNG PHẢI là DbContext:** DbContext là công cụ của hạ tầng (Infrastructure) đảm nhận kết nối CSDL và Change Tracking. Repository là ngôn ngữ của Domain (Ubiquitous Language).
- **Repository KHÔNG PHẢI là Generic CRUD Service:** Repository không phải là công cụ cung cấp đủ 4 hàm Create, Read, Update, Delete cho mọi thực thể. Nó chỉ cung cấp các phương thức truy xuất thực sự mang ý nghĩa nghiệp vụ.

**Kiến trúc Phối hợp và Dòng chảy (Coordination Flow):**

1. **Luồng Cấp trên (Top-Down Flow):**
   `Application` -> `Repository` -> `Domain`
   - Tầng **Application** nhận yêu cầu từ người dùng, gọi **Repository** để lấy ra một Aggregate Root (thuộc tầng **Domain**).
   - Tầng Application gọi các phương thức nghiệp vụ trên Aggregate Root đó.
   - Trạng thái của Aggregate Root thay đổi hoàn toàn in-memory.

2. **Luồng Cấp dưới (Bottom-Up Flow):**
   `Repository` -> `Specification` -> `Unit Of Work` -> `Infrastructure`
   - Tầng Application truyền một **Specification** vào **Repository** để tìm kiếm.
   - Nhờ Dependency Inversion, bản cài đặt (Implementation) của Repository tại tầng **Infrastructure** sẽ nhận Specification, dịch nó ra SQL, truy xuất DB, và tái tạo Aggregate Root.
   - Khi Aggregate Root thay đổi, tầng Application báo cho **Unit Of Work** (nằm ở Infrastructure) để commit toàn bộ transaction cùng một lúc. Repository không tự động `Save()`.

---

# 8. Repository Responsibilities
Những trách nhiệm TUYỆT ĐỐI của Repository:
- **Tái tạo (Reconstitute):** Đưa một Aggregate từ trạng thái lưu trữ (Database) trở lại thành một đối tượng sống trong bộ nhớ với toàn vẹn trạng thái.
- **Thêm/Bớt (Collection Simulation):** Cung cấp các thao tác `Add`, `Remove` để báo hiệu rằng một Aggregate mới được tạo ra hoặc một Aggregate cũ không còn cần thiết.
- **Truy xuất theo Tiêu chí (Find by Specification):** Nhận vào một Specification Pattern và trả về các Aggregate thỏa mãn tiêu chí đó.

Những thứ Repository **TUYỆT ĐỐI KHÔNG LÀM:**
- Không chứa Business Rules.
- Không chứa Domain Validation (như độ tuổi, định dạng email).
- Không chứa Domain Logic (không tự động tính toán hay thay đổi trạng thái Aggregate).
- Không chứa Transaction Logic (Không có `BeginTransaction`, `Commit`, `Rollback`).
- Không gọi HTTP, không biết UI, không biết API, không biết Controller.

---

# 9. Relationship With Other Domain Building Blocks
Mối quan hệ và sự phối hợp giữa Repository với các khối kiến trúc khác:
- **Aggregate Root:** Repository chỉ quản lý Aggregate Root, chịu trách nhiệm lưu trữ và phục hồi nó. Nó KHÔNG thay thế Aggregate Root và không chứa trạng thái của Aggregate.
- **Entity:** Repository không quản lý Entity độc lập. Mọi thao tác lưu/xóa Entity phải thông qua Aggregate Root.
- **Value Object:** Repository nhận Value Object làm tham số điều kiện, nhưng không quản lý lưu trữ độc lập cho chúng.
- **Domain Primitive:** Được sử dụng làm tham số định danh hoặc thuộc tính cốt lõi khi Repository truy vấn.
- **Domain Service:** Domain Service có thể gọi nhiều Repository để điều phối nghiệp vụ phức tạp. Tuy nhiên, Repository KHÔNG thay thế Domain Service.
- **Specification:** Repository nhận Specification để lọc dữ liệu. Nó KHÔNG chứa logic điều kiện nội tại, do đó KHÔNG thay thế Specification.
- **Unit Of Work:** Repository không tự lưu dữ liệu (không SaveChanges). Unit Of Work quản lý Transaction. Repository KHÔNG thay thế Unit Of Work.
- **Application Layer:** Gọi Repository để lấy Aggregate, thay đổi Aggregate, sau đó gọi Unit Of Work để lưu.
- **Query Model (CQRS):** Repository phục vụ mảng Command (ghi). Nó KHÔNG thay thế Query Model (đọc dữ liệu dẹt).

---

# 10. Repository Creation Order
Trình tự thiết kế kiến trúc và cài đặt Repository (Khi triển khai Coding):
- **Step 1:** Identify Aggregate (Chỉ xác định tạo Repo cho Aggregate Root thực thụ).
- **Step 2:** Define Repository Contract (Tạo interface tại tầng Domain).
- **Step 3:** Define Specification Integration (Định nghĩa hàm nhận ISpecification).
- **Step 4:** Define Repository Methods (Thêm các hàm như Add, Remove cơ bản).
- **Step 5:** Validation (Kiểm tra lại thiết kế theo Validation Rules).
- **Step 6:** Freeze (Chốt hợp đồng kiến trúc).

---

# 11. Repository Evolution Strategy
Lộ trình kiến trúc của Repository qua các Sprint:
- **Sprint 03:** Thiết lập Architecture Specification cho Repository. Chốt chặn các nguyên tắc bất di bất dịch của DDD.
- **Sprint 04:** Thiết kế các Contract (Interface) của Repository tại tầng Domain. Không có bất kỳ cài đặt (Implementation) nào.
- **Sprint 05:** Thiết kế tầng Infrastructure. Xây dựng các lớp triển khai cụ thể bằng Entity Framework Core. Dịch Specification thành LINQ/SQL.
- **Sprint 06:** Thiết kế Unit of Work tại Infrastructure và cách Application Layer điều phối Unit of Work.
- **Sprint 07:**
  - *Performance Optimization:* Tối ưu hóa hiệu năng tổng thể.
  - *Repository Decorator:* Áp dụng mẫu Decorator để linh hoạt hóa chức năng.
  - *Caching Strategy:* Thiết lập kiến trúc bộ nhớ đệm (Cache).
  - *Read Replica:* Kiến trúc kết nối tới bản sao đọc (Replica).
  - *Distributed Repository:* Kho lưu trữ phân tán.
  - *Observability:* Theo dõi tính minh bạch.
  - *Metrics:* Đo lường các chỉ số.
  - *Telemetry:* Ghi nhận giám sát từ xa.
  - *Repository Diagnostics:* Phân tích chuyên sâu Repository.
  - *Repository Benchmark:* Đánh giá và đo tải hiệu suất.

---

# 12. Repository Principles
Các nguyên lý thiết kế tối thượng của Repository:
- **Persistence Ignorance (Mù tịt về Lưu trữ):** Interface của Repository ở tầng Domain không được chứa bất kỳ từ khóa, kiểu dữ liệu hay exception nào liên quan đến SQL, ADO.NET, EF Core.
- **Collection-Oriented Illusion (Ảo giác Bộ sưu tập):** Repository phải được thiết kế sao cho Client (Application Layer) có cảm giác đang tương tác với một `ICollection<T>` thuần túy trên bộ nhớ.
- **Aggregate Root Only (Chỉ dành cho Aggregate Root):** Repository chỉ phục vụ đối tượng gốc (Root). Mọi thành phần con bên trong Aggregate phải được lưu trữ và truy xuất thông qua Root.
- **No Partial Loading (Không tải từng phần):** Repository luôn luôn trả về một Aggregate Root hoàn chỉnh (Fully loaded) để đảm bảo Invariants (bất biến) được bảo vệ. Không có chuyện trả về Aggregate thiếu dữ liệu con.

---

# 13. Repository Classification Strategy
Phân loại Repository trong kiến trúc Enterprise:
- **Command Repository (Domain Repository):** Được định nghĩa tại tầng Domain, trả về Aggregate Root. Dùng riêng cho luồng Ghi (Command) hoặc xử lý nghiệp vụ phức tạp. Luôn load full Aggregate.
- **Query/Read Model Repository:** (Thường nằm ngoài Domain, thuộc thẳng Application hoặc Infrastructure trong CQRS). Dùng cho luồng Đọc. Trả về DTOs, Flat Data, không trả về Aggregate. (Domain Repository không quan tâm đến nhánh này).

---

# 14. Aggregate Repository Rules
Quy tắc tương quan 1-1 không thể phá vỡ:
- **Một Aggregate = Một Repository:** Nếu có Aggregate `Citizen`, chỉ có duy nhất `CitizenRepository`.
- **Thực thể con không có Repository:** Nếu `Address` là một Entity con nằm trong Aggregate `Citizen`, tuyệt đối KHÔNG ĐƯỢC phép có `AddressRepository`. Để sửa Address, phải thông qua `CitizenRepository.GetById()` -> `citizen.UpdateAddress()`.

---

# 15. Repository Collaboration
Sự phối hợp giữa Repository và các khối khác:
- **Với Domain Service:** Domain Service có thể nhận Repository thông qua Constructor Injection (dưới dạng Interface) để truy xuất dữ liệu cần thiết phục vụ cho một nghiệp vụ liên-Aggregate.
- **Với Unit of Work:** Repository chỉ ghi nhận sự thay đổi (Tracking) hoặc thu thập đối tượng. Unit of Work là người duy nhất nắm giữ lệnh `Commit()` hoặc `SaveChangesAsync()`. Repository không tự quyết định thời điểm lưu dữ liệu.
- **Với Specification:** Repository nhận Specification làm tham số lọc. Thay vì có hàng chục hàm như `GetActiveCitizens()`, `GetCitizensByProvince()`, Repository chỉ cần một hàm `Find(ISpecification spec)`.

---

# 16. Naming Convention
Quy tắc đặt tên hợp đồng (Contract) tại tầng Domain:
- Tên Repository phải gắn liền với tên của Aggregate Root.
- Bắt buộc phải có hậu tố `Repository`.
- Tên phải phản ánh tính chất của một Collection.

**Allowed (Cho phép):**
- Tên hợp đồng: `ICitizenRepository`, `IHouseholdRepository`, `IPensionPolicyRepository`.
- Tên hàm (nếu có): `Add`, `Remove`, `FindById`, `FindBySpecification`.

**Not Allowed (Tuyệt đối cấm):**
- Tên theo DB Table: `ITbl_CitizenDao`, `ICitizenDataStore`.
- Đặt tên cho thực thể con: `IAddressRepository`.
- Tên hàm rò rỉ hạ tầng: `SaveToDb`, `UpdateTable`, `ExecuteSqlQuery`.

---

# 17. Folder Strategy
Chiến lược tổ chức cấu trúc thư mục của Repository tại tầng Domain:
- `Repositories/`
  - `SeedWork/` (Chứa định nghĩa Base Interface trừu tượng nếu cần).
  - `Modules/`
    - `Demographic/` (Hợp đồng lưu trữ Công dân, Hộ khẩu).
    - `SocialSecurity/` (Hợp đồng lưu trữ Chính sách, Trợ cấp).
    - `Disbursement/` (Hợp đồng giải ngân).

*(Lưu ý: Chỉ thiết lập định hướng thư mục, không sinh source code tại đây).*

---

# 18. Dependency Rules
Ràng buộc phụ thuộc cứng cho Repository tại tầng Domain:

**Allowed Dependencies:**
- `System.*` (Thư viện cốt lõi C#).
- Nội bộ tầng `Domain` (Aggregate Roots, Value Objects, Domain Exceptions, Specifications).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `EF Core` / `Microsoft.Data.SqlClient`
- `Dapper`
- `MediatR`
- `Logging` (Serilog, ILogger)
- `SignalR`
- `ASP.NET` (Web, MVC, API)
- `HttpClient`
- `Caching Framework` (Redis, MemoryCache)
- Các thư viện `ORM` bất kỳ.
- `Reflection` lạm dụng.
- `Dynamic Proxy`.

---

# 19. Validation Rules
Một Repository Interface được coi là hợp lệ (Valid) trong Domain khi nó tuân thủ:
- **Stateless:** Không giữ trạng thái dữ liệu (Session Data).
- **Thread Safety:** Responsibility belongs to Infrastructure implementations and is outside the Repository contract.
- **Technology Independent:** Độc lập với mọi công nghệ lưu trữ.
- **Aggregate Consistency:** Giữ vững sự toàn vẹn của Aggregate khi trả về dữ liệu.
- **Persistence Ignorance:** Mù tịt hoàn toàn về Database.
- **Specification Ready:** Sẵn sàng tích hợp Specification Pattern.
- **Unit Of Work Ready:** Không phá vỡ luồng Commit của Unit Of Work.
- Chỉ làm việc độc quyền với một Aggregate Root.
- Không có hàm mang nghĩa ép buộc ghi cơ sở dữ liệu (`Save`, `UpdateDb`).

---

# 20. Repository Performance Strategy
Các chiến lược xử lý kiến trúc về mặt hiệu suất:
- **Large Aggregate:** Hạn chế nhồi nhét quá nhiều vào một Aggregate. Nếu Aggregate quá lớn, Repository khi load lên RAM sẽ gây cạn kiệt tài nguyên.
- **Batch Loading:** Repository phải được thiết kế để hỗ trợ tải nhiều dữ liệu cùng lúc khi cần (nhận List các Spec hoặc ID).
- **Identity Map:** Cơ chế (thường có sẵn trong EF) để Repository không load lại một Aggregate đã nằm trong bộ nhớ của Request hiện tại.
- **Caching Layer:** Repository Decorator có thể được bọc bên ngoài để thực hiện Cache In-Memory trước khi thực sự xuống DB.
- **Read Through Cache:** Thiết lập chiến lược nếu cache miss, Repository sẽ tự động lấy DB và điền vào Cache.
- **Second Level Cache:** Áp dụng cho các Aggregate ít biến động.
- **Repository Decorator:** Là nơi lý tưởng để chèn Logic Logging, Caching, Retry mà không làm bẩn implementation gốc.
- **Async IO:** Repository 100% sử dụng luồng I/O không chặn (Asynchronous I/O) khi tương tác với mạng/ổ đĩa.
- **Database Round Trip:** Giảm thiểu số vòng lặp xuống CSDL bằng cách áp dụng Specification chuẩn xác.
- **Memory Consumption:** Tránh việc dùng Repository Command lấy ra danh sách hàng triệu dòng dữ liệu chỉ để đọc.

---

# 21. AI Coding Constraints
Giới hạn hành vi bắt buộc của AI Coding Agent khi đọc hiểu và triển khai tài liệu này:
- KHÔNG tạo bất kỳ file C# nào triển khai (`class`) cho các Repository này.
- KHÔNG tạo Generic Repository (`IRepository<T>`). Việc thiết kế SeedWork (nếu có) do con người quyết định ở tầng mã nguồn.
- KHÔNG sinh mã JSON, XML, UML, hay giả mã (Pseudo Code).
- Tuyệt đối không gắn các Attribute như `[Table]`, `[Column]` vào mô hình Domain để tiện cho Repository.
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 22. Definition Of Done
Tài liệu được định nghĩa là hoàn tất (Done) khi:
- **Repository Architecture hoàn chỉnh:** Đặc tả đủ cấu trúc và nhiệm vụ.
- **DDD Compliance:** Tuân thủ chuẩn chỉ thiết kế Aggregate Root.
- **Clean Architecture Compliance:** Domain độc lập hạ tầng.
- **Enterprise Ready:** Sẵn sàng mở rộng quy mô lớn.
- **AI Ready:** Prompt rõ ràng cho tác vụ Coding.
- **Coding Ready:** Sẵn sàng chuyển giao Sprint sau.
- **Freeze Ready:** Đạt chuẩn để khóa kiến trúc.

---

# 23. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **Persistence Ignorance** | Repository Interface không chứa Exception hay Type của CSDL. | Bắt buộc (Mandatory) |
| **Aggregate Root Exclusive**| Chỉ Aggregate Root mới có Repository Interface. | Bắt buộc (Mandatory) |
| **No Business Logic** | Không chứa If-Else nghiệp vụ, chỉ có logic truy xuất. | Bắt buộc (Mandatory) |
| **No UI/API Knowledge** | Không có Request/Response HTTP trong tham số/kết quả. | Bắt buộc (Mandatory) |
| **No DTO Return** | Luôn trả về Aggregate Root nguyên bản. | Bắt buộc (Mandatory) |
| **Unit Of Work Separation**| Không tự động Save hay Commit, nhường quyền cho UoW. | Bắt buộc (Mandatory) |
| **Thread Safety** | Responsibility belongs to Infrastructure implementations. | Bắt buộc (Mandatory) |
| **Specification Ready** | Hỗ trợ nhúng chuẩn lọc miền thay vì filter linh tinh. | Bắt buộc (Mandatory) |
| **Aggregate Integrity** | Trả về thực thể nguyên vẹn, cấm partial load. | Bắt buộc (Mandatory) |
| **No ORM / No SQL** | Không dùng Annotation, IQueryable của ORM. | Bắt buộc (Mandatory) |
| **No Query Builder** | Không nhúng thư viện SQL Builder vào Domain. | Bắt buộc (Mandatory) |
| **No Infra Leakage** | Bảo vệ tuyệt đối ranh giới kiến trúc. | Bắt buộc (Mandatory) |
| **Repository per Aggregate**| Ánh xạ 1-1 nghiêm ngặt với gốc miền. | Bắt buộc (Mandatory) |

---

# 24. Risks
Cảnh báo các rủi ro kiến trúc (Architectural Risks) thường gặp:
- **Repository Explosion:** Khởi tạo Repository vô tội vạ cho mọi Entity con, phá vỡ luồng quản lý.
- **Over Fetching:** Load quá nhiều dữ liệu không cần thiết vào Aggregate.
- **Under Fetching:** Load thiếu dữ liệu (Entity con bị null) gây lỗi NullReference khi xử lý nghiệp vụ.
- **Chatty Repository:** Giao tiếp lặp đi lặp lại với DB trong một vòng lặp (N+1 query problem).
- **Leaky Repository:** Rò rỉ các thư viện hạ tầng như DbConnection vào Interface.
- **Shared Repository:** Một Repository cố gắng ôm show quản lý nhiều loại Aggregate khác nhau.
- **Transaction Leakage:** Tự động mở đóng `BeginTransaction` ở Repo làm loạn hệ thống.
- **Generic Repository Abuse:** Lạm dụng `IRepository<T>` biến toàn bộ hệ thống thành vỏ bọc yếu ớt của ORM.

---

# 25. Architectural Anti Patterns
Các mẫu phản kiến trúc (Anti-Patterns) thường gặp khi áp dụng Repository và cách xử lý:

- **God Repository:**
  - Nhồi nhét quá nhiều phương thức (hàng trăm hàm FindBy...) vào một giao diện.
- **Chatty Repository:**
  - Thiết kế hàm lấy từng Object nhỏ thay vì cung cấp hàm lấy theo mẻ (Batch), làm nghẽn cổ chai mạng.
- **Repository per Table:**
  - Khớp mọi Table trong Database với một Repository thay vì căn cứ vào Aggregate Root (sai bét triết lý DDD).
- **Infrastructure Leakage:**
  - Thêm `IQueryable` vào kiểu trả về của Repository, khiến Application Layer bị dính chặt với EF Core.
- **Specification Leakage:**
  - Đẩy thẳng Expression Tree thuần túy của EF vào tham số thay vì dùng Domain Specification.
- **Query Logic Inside Repository:**
  - Dùng Repository để tính tổng, group by, sum, avg phục vụ vẽ biểu đồ (Đây là nhiệm vụ của Read Model).
- **Returning IQueryable:**
  - Như đã nói, bộc lộ toàn bộ cây truy vấn ORM ra ngoài, phá vỡ tính Encapsulation.
- **Returning DbSet:**
  - Lỗi nghiêm trọng nhất, trả thẳng Object của EF ra Domain/Application.
- **Returning ORM Entity:**
  - Tách bạch giữa Domain Model và Data Model kém, dẫn tới trả về Data Entity.
- **Returning Lazy Proxy:**
  - Trả về proxy của ORM khiến cho Exception văng ra bất thình lình ở View nếu Lazy Loading tự kích hoạt.
- **Generic CRUD Service Anti-Pattern:**
  - Biến toàn bộ Use Case thành Create/Update/Delete tẻ nhạt.
- **Unit of Work Inside Repository:**
  - Tự gọi `SaveChanges()` giấu diếm sau các hàm Add/Update.

---

# 26. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã giải thích cặn kẽ khái niệm Repository theo chuẩn DDD, khác biệt với DAO?
- [x] Đã quy định rõ nguyên tắc "1 Aggregate = 1 Repository"?
- [x] Đã mô tả rõ ràng sự phối hợp Repository - Specification - UoW - Infrastructure?
- [x] Đã liệt kê chi tiết các rủi ro và Architectural Anti-Patterns?
- [x] Đã thiết lập các rào cản ngăn chặn rò rỉ Infrastructure vào Domain?
- [x] Repository không sinh code, Không Generic Repository, Không DbContext, Không SQL, Không Dapper, Không EF, Không Transaction, Không SaveChanges, Không CRUD Service? (CÓ).
- [x] Tài liệu tuân thủ chuẩn Enterprise Documentation Suite cao hơn tài liệu số 36? (CÓ).

---

# 27. References
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
- 31_SPRINT_03_BASE_CLASSES.md
- 32_SPRINT_03_EXCEPTIONS.md
- 33_SPRINT_03_EVENTS.md
- 34_SPRINT_03_DOMAIN_PRIMITIVES.md
- 35_SPRINT_03_DOMAIN_SERVICES.md
- 36_SPRINT_03_SPECIFICATIONS.md

---
# END OF SPECIFICATION
