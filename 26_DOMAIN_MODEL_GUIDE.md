# 26_DOMAIN_MODEL_GUIDE.md
## SPRINT 02 – DOMAIN ARCHITECTURE & MODELING
Version: 1.1.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 26_DOMAIN_MODEL_GUIDE
- **Title:** Enterprise Domain Model Guide
- **Phase:** Sprint 02
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Software Architect, Backend Developer

---

# 2. Purpose
Tài liệu này cung cấp các hướng dẫn tiêu chuẩn cấp doanh nghiệp (Enterprise Specification) về cách thức mô hình hóa tầng Domain (Domain Layer) trong dự án AnSinhSo Enterprise. Mục tiêu là định hướng tư duy thiết kế theo phương pháp Domain-Driven Design (DDD), Clean Architecture và Vertical Slice Architecture để xây dựng một lõi nghiệp vụ mạnh mẽ, độc lập và dễ bảo trì.

Tài liệu này là lý thuyết nền tảng, hoàn toàn không chứa mã nguồn, sơ đồ kỹ thuật hay đặc tả cấu trúc dữ liệu.

---

# 3. Consistency Rules
Quy định sự nhất quán bắt buộc giữa tài liệu `25_DOMAIN_ARCHITECTURE.md` và `26_DOMAIN_MODEL_GUIDE.md`:
- **Kiến trúc định hướng mô hình:** Bất kỳ quy tắc mô hình hóa nào trong `26_DOMAIN_MODEL_GUIDE.md` đều phải tuân thủ nghiêm ngặt các ranh giới của các Bounded Context và Dependency Rule được định nghĩa trong `25_DOMAIN_ARCHITECTURE.md`.
- **Ngôn ngữ chung (Ubiquitous Language):** Các thuật ngữ nghiệp vụ được liệt kê trong `25_DOMAIN_ARCHITECTURE.md` phải được phản ánh nhất quán trong các quy tắc đặt tên và hướng dẫn mô hình hóa tại tài liệu này.
- **Bất biến hạ tầng (Persistence Ignorance):** Cả hai tài liệu đều phải khẳng định nguyên tắc tầng Domain tuyệt đối không có liên kết vật lý tới bất kỳ công nghệ lưu trữ hay framework hạ tầng nào.

---

# 4. Domain Modeling Philosophy
Quá trình mô hình hóa Domain phải tuân thủ nghiêm ngặt các triết lý sau:

- **Rich Domain Model:** Các đối tượng Domain không chỉ chứa dữ liệu mà phải bao bọc trọn vẹn cả các quy tắc nghiệp vụ và logic xử lý liên quan.
- **Behavior over Data:** Tập trung mô hình hóa các hành vi (Behavior) của nghiệp vụ thay vì cấu trúc dữ liệu. Dữ liệu chỉ là kết quả của hành vi.
- **Persistence Ignorance:** Domain Model hoàn toàn không biết và không quan tâm đến việc dữ liệu của nó được lưu trữ như thế nào. Không có ORM, SQL hay Database Mapping ở tầng này.
- **Encapsulation:** Đóng gói chặt chẽ trạng thái bên trong. Không để lộ các thuộc tính ra ngoài cho phép thay đổi tự do (No public setters). Mọi thay đổi trạng thái phải đi qua các phương thức biểu diễn hành vi.
- **Always Valid State:** Đối tượng Domain chỉ tồn tại khi nó ở trạng thái hợp lệ. Không cho phép khởi tạo một đối tượng hỏng hoặc thiếu thông tin nghiệp vụ thiết yếu.
- **Explicit Business Rules:** Mọi quy tắc nghiệp vụ, dù nhỏ nhất, phải được mô hình hóa một cách rõ ràng thành các khái niệm trong Domain, không được ẩn giấu trong Application Layer hay Infrastructure Layer.

---

# 5. Domain Building Blocks
Chỉ định nghĩa các khối xây dựng cơ bản (Building Blocks) cấp Enterprise sử dụng trong dự án:

- **Entity:** Là một đối tượng có định danh duy nhất (Identity) không thay đổi theo thời gian, dù các thuộc tính khác có thay đổi.
- **Aggregate:** Là một cụm các Entity và Value Object có liên quan logic chặt chẽ với nhau, được xử lý như một đơn vị duy nhất trong giao dịch dữ liệu.
- **Aggregate Root:** Là Entity đại diện cho toàn bộ Aggregate. Nó là điểm giao tiếp duy nhất từ bên ngoài vào Aggregate.
- **Value Object:** Là đối tượng được định nghĩa bằng các thuộc tính của nó thay vì định danh. Value Object là bất biến (Immutable).
- **Repository:** Là khái niệm trừu tượng (Interface) định nghĩa cách thức truy xuất và lưu trữ toàn bộ một Aggregate.
- **Domain Service:** Chứa các logic nghiệp vụ không thuộc về bất kỳ Entity hay Value Object cụ thể nào, hoặc logic liên quan đến nhiều Aggregate khác nhau.
- **Factory:** Chịu trách nhiệm tạo lập các Aggregate hoặc Entity phức tạp, đảm bảo Always Valid State ngay từ lúc khởi tạo.
- **Specification:** Là mẫu thiết kế đóng gói một quy tắc nghiệp vụ, điều kiện lọc hoặc validation thành một đối tượng độc lập, có thể tái sử dụng.
- **Domain Event:** Là sự kiện mang ý nghĩa nghiệp vụ, thông báo rằng một điều gì đó quan trọng vừa xảy ra trong Domain, giúp tách biệt các quy trình phụ trợ.

---

# 6. Modeling Decision Guidelines
Hướng dẫn cách ra quyết định khi nào nên sử dụng khối xây dựng (Building Block) nào:

- **Entity:** Sử dụng khi một khái niệm nghiệp vụ có sự sống kéo dài theo thời gian, cần theo dõi sự thay đổi trạng thái và buộc phải có định danh duy nhất không đổi.
- **Value Object:** Sử dụng khi một khái niệm nghiệp vụ chỉ mang ý nghĩa thông qua các thuộc tính của nó, không có định danh duy nhất và có tính bất biến. *Khuyến nghị: Ưu tiên sử dụng Value Object hơn Entity bất cứ khi nào có thể để giảm sự phức tạp.*
- **Aggregate:** Sử dụng khi cần gom nhóm các Entity và Value Object có liên quan chặt chẽ với nhau để bảo vệ một giao dịch nhất quán (Transaction Consistency) và áp đặt các quy tắc bất biến chung.
- **Domain Service:** Sử dụng khi một hành động hoặc logic nghiệp vụ liên quan đến nhiều Aggregate khác nhau, hoặc nó không tự nhiên thuộc về bất kỳ một Entity hay Value Object nào. Tránh lạm dụng để không tạo ra Anemic Domain Model.
- **Specification:** Sử dụng khi cần đóng gói các quy tắc nghiệp vụ, điều kiện xác thực (validation) hoặc tiêu chí lọc phức tạp để có thể tái sử dụng, kiểm thử độc lập và kết hợp (chaining) ở nhiều nơi.
- **Factory:** Sử dụng khi quá trình khởi tạo một Aggregate hoặc Entity rất phức tạp, đòi hỏi phải thiết lập nhiều tham số và kiểm tra các điều kiện để đảm bảo đối tượng luôn ở trạng thái hợp lệ ngay từ ban đầu.
- **Domain Event:** Sử dụng khi một sự thay đổi trạng thái ở một phần của hệ thống cần thông báo cho các phần khác (trong cùng hoặc khác Bounded Context) để thực thi các tác vụ liên đới mà không tạo ra sự ràng buộc chặt chẽ (Decoupling).

---

# 7. Naming Convention
Quy tắc đặt tên chuẩn mực cho các khái niệm Domain theo ngôn ngữ chung (Ubiquitous Language):

- **Entity / Aggregate Root:** Sử dụng danh từ đếm được, viết hoa chữ cái đầu (PascalCase), mô tả chính xác thực thể nghiệp vụ. Tuyệt đối không dùng hậu tố `Entity` hay `Model`.
- **Value Object:** Sử dụng danh từ hoặc cụm danh từ mô tả tính chất, giá trị (PascalCase).
- **Domain Event:** Sử dụng cấu trúc: Tên đối tượng + Động từ ở thì quá khứ phân từ (ví dụ: `HouseholdCreated`, `PaymentCompleted`). Luôn mô tả sự kiện đã xảy ra.
- **Domain Service:** Sử dụng danh từ hoặc cụm danh từ chỉ rõ nghiệp vụ + hậu tố `Service` (chỉ giới hạn việc sử dụng Service ở Domain Layer).
- **Repository Interface:** Bắt đầu bằng chữ `I` + Tên Aggregate Root + hậu tố `Repository` (ví dụ: `IHouseholdRepository`).
- **Specification:** Tên mô tả tiêu chí lọc hoặc quy tắc + hậu tố `Specification`.
- **Factory:** Tên Aggregate/Entity + hậu tố `Factory`.
- **Phương thức (Methods):** Bắt đầu bằng động từ thể hiện rõ hành vi nghiệp vụ (Ví dụ: `Activate()`, `ChangeAddress()`, `CalculateTotal()`). Không sử dụng tiền tố `Set` hoặc `Get` mang tính kỹ thuật.

---

# 8. Aggregate Modeling Guidelines
- Aggregate phải đại diện cho một ranh giới nhất quán về giao dịch (Transaction Boundary). Một Database Transaction lý tưởng chỉ thay đổi một Aggregate.
- Mọi thao tác cập nhật dữ liệu trong Aggregate phải được thực hiện thông qua Aggregate Root.
- Giữ Aggregate nhỏ gọn để tối ưu hiệu suất và giảm thiểu xung đột giao dịch (Concurrency Conflict).
- Các Aggregate chỉ tham chiếu đến nhau thông qua định danh (ID), tuyệt đối không tham chiếu qua object reference để tránh lock dữ liệu thừa.

---

# 9. Entity Modeling Guidelines
- Entity phải bảo vệ dữ liệu của mình bằng cách đóng gói các thuộc tính.
- Chỉ cung cấp các phương thức thay đổi trạng thái (methods) phản ánh đúng ngôn ngữ nghiệp vụ.
- Định danh (Identity) của Entity phải được thiết lập từ khi khởi tạo và không bao giờ được phép thay đổi.

---

# 10. Value Object Guidelines
- Ưu tiên sử dụng Value Object thay vì các kiểu dữ liệu nguyên thủy (Primitive Types) để biểu diễn các khái niệm nghiệp vụ (ví dụ: dùng Value Object thay vì dùng `string` cho số điện thoại).
- Value Object phải hoàn toàn bất biến (Immutable). Khi cần thay đổi giá trị, phải tạo ra một thể hiện (instance) Value Object mới.
- Hai Value Object được coi là bằng nhau nếu và chỉ nếu mọi thuộc tính bên trong của chúng đều giống nhau.

---

# 11. Domain Event Guidelines
- Tên Domain Event phải thể hiện rõ điều gì đó đã xảy ra trong quá khứ.
- Domain Event chứa các thông tin cần thiết nhất liên quan đến sự kiện, tối thiểu là ID của Aggregate và các dữ liệu liên đới phục vụ tác vụ khác.
- Sử dụng Domain Event để xử lý các logic phụ (Side Effects) một cách bất đồng bộ, giúp cho quy trình chính không bị nghẽn (Blocking).

---

# 12. Repository Guidelines
- Chỉ thiết kế Interface Repository cho Aggregate Root. Không được thiết kế Repository cho các Entity con bên trong Aggregate.
- Repository Interface chỉ định nghĩa các phương thức nghiệp vụ cần thiết, không bắt buộc phải tuân theo chuẩn CRUD (Create, Read, Update, Delete) một cách máy móc.
- Repository hoàn toàn không chứa Business Logic; nó chỉ đóng vai trò như một bộ sưu tập (Collection) các Aggregate trong bộ nhớ trừu tượng.

---

# 13. Domain Service Guidelines
- Chỉ sử dụng Domain Service khi logic nghiệp vụ không thể gán tự nhiên vào một Entity hay Value Object.
- Domain Service là Stateless (Không lưu trạng thái).
- Phải đề phòng việc lạm dụng Domain Service để chứa logic của Entity, tránh biến Entity thành Anemic Domain Model.

---

# 14. Domain Modeling Rules
1. **No External Dependencies:** Domain Model không được phép reference đến bất kỳ framework hạ tầng, database provider hay thư viện UI/API nào.
2. **Business Language First:** Mọi tên gọi của Entity, phương thức, sự kiện phải lấy trực tiếp từ Ubiquitous Language của dự án.
3. **One Transaction per Aggregate:** Một giao dịch thay đổi trạng thái chỉ nên áp dụng trên một Aggregate duy nhất. Các thay đổi xuyên Aggregate phải được xử lý bằng Eventual Consistency thông qua Domain Events.
4. **Validation Placement:** Domain validation (kiểm tra Invariants nghiệp vụ) phải nằm trong Domain Model. Data validation (như format email, chuỗi rỗng) sẽ nằm ở Application Layer.

---

# 15. Domain Modeling Anti-patterns
Các mẫu thiết kế lỗi (Anti-patterns) tuyệt đối tránh:
- **Anemic Domain Model:** Các Entity chỉ chứa thuộc tính với các hàm Getter/Setter công khai mà không có hành vi (behavior) hay logic nghiệp vụ, đẩy toàn bộ logic ra Application Service.
- **God Entity:** Một Entity ôm đồm quá nhiều trách nhiệm, chứa một lượng khổng lồ các thuộc tính và luồng nghiệp vụ của toàn bộ hệ thống.
- **Large Aggregate:** Aggregate chứa quá nhiều Entity con, dẫn đến lock dữ liệu lâu, giảm hiệu suất đáng kể và dễ sinh lỗi đồng thời (Concurrency Exception).
- **Fat Repository:** Repository chứa các truy vấn phức tạp mang tính chất báo cáo hoặc thực hiện các logic nghiệp vụ ngay bên trong phương thức truy xuất dữ liệu.
- **Primitive Obsession:** Lạm dụng các kiểu dữ liệu nguyên thủy (int, string, bool) để biểu diễn các khái niệm nghiệp vụ thay vì đóng gói chúng vào Value Object.
- **Transaction Script:** Viết logic nghiệp vụ theo dạng mã lệnh tuần tự (procedural) kéo dài trong một Service từ đầu đến cuối mà không tuân theo thiết kế hướng đối tượng của DDD.

---

# 16. Domain Model Lifecycle
Mô tả sự tiến hóa của Domain Model qua các giai đoạn triển khai (Sprint 03–06):

- **Sprint 03 (Domain Initialization):** Chuyển đổi các hướng dẫn lý thuyết thành cấu trúc lớp (Class) và Interface ban đầu. Thiết lập cấu trúc thư mục, các lớp cơ sở (Base Classes), Exception nghiệp vụ. Chưa có bất kỳ logic xử lý Application hay truy xuất CSDL nào.
- **Sprint 04 (Application Layer Integration):** Các Domain Model bắt đầu được Application Layer triệu gọi thông qua các Use Case sử dụng kiến trúc CQRS. Domain Model thực hiện nhiệm vụ xác thực Invariants trong khi Application Layer điều phối luồng thực thi.
- **Sprint 05 (Infrastructure & Persistence):** Các Repository Interfaces được triển khai chi tiết tại Infrastructure Layer. Domain Model được ánh xạ xuống CSDL thông qua EF Core Configurations (Entity Type Configurations chỉ nằm ở Infrastructure Layer). Bản thân Domain Model không thay đổi gì để tương thích với hạ tầng.
- **Sprint 06 (API Exposure):** API Layer bọc bên ngoài Application Layer. Domain Model không bị phơi bày trực tiếp ra ngoài (thông qua DTOs), nhưng toàn bộ các quy tắc nghiệp vụ của nó sẽ định hình luồng kiểm soát và phản hồi của API.

---

# 17. Dependency Validation Rules
Quy tắc kiểm tra sự phụ thuộc bắt buộc dành cho AI Coding Agent trước khi sinh mã nguồn liên quan đến Domain trong các Sprint tương lai:

- **Rule 1 (No external references):** Hệ thống sẽ vi phạm kiến trúc nếu Domain layer tham chiếu đến thư viện I/O, Database, Framework ngoài (trừ .NET Core cơ bản). AI phải tự động kiểm tra tính hợp lệ của file `.csproj` thuộc project Domain.
- **Rule 2 (No UI/API concerns):** Nếu có yêu cầu sinh mã DTO, cấu hình JSON Serialization, hay xử lý HttpContext bên trong Domain, AI bắt buộc phải từ chối thi hành và cảnh báo việc vi phạm ranh giới Layer.
- **Rule 3 (Interface-Driven Storage):** Domain layer chỉ được chứa giao diện truy xuất (`IRepository`). Bất kỳ yêu cầu nào cố gắng đưa `DbContext` hay các truy vấn LINQ to SQL vào Domain phải bị chặn đứng.
- **Rule 4 (No Circular Dependencies / No Direct Object Reference Across Aggregates):** Aggregate này tuyệt đối không được chứa tham chiếu đối tượng (object reference) sang Aggregate khác, mà chỉ được phép lưu giữ ID của Aggregate đó. AI phải quét mã nguồn để đảm bảo không vi phạm.

---

# 18. Cross-cutting Modeling Rules
- **Concurrency:** Thiết kế Domain phải tính đến khả năng xảy ra tranh chấp dữ liệu và giải quyết thông qua cơ chế Optimistic Concurrency (sử dụng Version control/RowVersion trên Aggregate Root).
- **Audit & Tracking:** Không đưa các thông tin kiểm toán kỹ thuật thuần túy (CreatedBy, UpdatedDate) vào trực tiếp business logic của Domain, trừ khi đó là yêu cầu cốt lõi. Ưu tiên xử lý qua Base Classes để hạ tầng tự động xử lý.
- **Security:** Các quy tắc bảo mật, phân quyền truy cập người dùng được kiểm tra ở Application Layer. Domain Layer giả định mọi lời gọi đến nó đã được Application Layer xác thực và cấp quyền hợp lệ.

---

# 19. AI Review Checklist
Checklist bắt buộc dành cho AI Coding Agent trước khi coi tài liệu này (hoặc các quy trình tạo tài liệu Domain liên quan) là hoàn tất:

- [ ] Tôi có tuân thủ hoàn toàn các nguyên tắc về Clean Architecture và DDD không?
- [ ] Tôi có chắc chắn 100% không sinh ra mã C#, câu lệnh SQL hay biểu đồ kỹ thuật trong tài liệu này không?
- [ ] Tất cả các định nghĩa, quy tắc đặt tên có nhất quán với Ubiquitous Language và `25_DOMAIN_ARCHITECTURE.md` không?
- [ ] Không có bất kỳ thuật ngữ liên quan đến công nghệ (Database Mapping, Foreign Key) rò rỉ vào trong các quy tắc của Domain Layer?
- [ ] Tôi đã mô tả rõ ràng sự tiến hóa của Domain qua các Sprint và các quy tắc kiểm tra ràng buộc (Dependency Validation) chưa?

---

# 20. Architecture Review Checklist
Checklist cho Project Owner và Solution Architecture Team đánh giá tài liệu:

- [ ] Tài liệu có cung cấp các hướng dẫn ra quyết định thiết kế rành mạch và thực tế không?
- [ ] Các Anti-patterns đã được giải thích rõ để Developer tránh mắc phải chưa?
- [ ] Sự nhất quán giữa tầng Kiến trúc và tầng Mô hình hóa đã được giữ vững?
- [ ] Tài liệu hoàn toàn KHÔNG chứa C#, SQL, UML hay bất kỳ mã nguồn nào, đúng như yêu cầu khắt khe của quy trình?

---

# 21. References

### Architecture
- [25_DOMAIN_ARCHITECTURE.md](file:///d:/AnSinhSo_Enterprise/25_DOMAIN_ARCHITECTURE.md)

### Standards
- [05_DATABASE_RULES.md](file:///d:/AnSinhSo_Enterprise/Sprint01_Package/05_DATABASE_RULES.md)
- [08_BACKEND_STANDARDS.md](file:///d:/AnSinhSo_Enterprise/Sprint01_Package/08_BACKEND_STANDARDS.md)

### Sprint Documents
- [00_PROJECT_BOOTSTRAP.md](file:///d:/AnSinhSo_Enterprise/00_PROJECT_BOOTSTRAP.md)
- [14_PROJECT_STRUCTURE.md](file:///d:/AnSinhSo_Enterprise/Sprint01_Package/14_PROJECT_STRUCTURE.md)
- [17_DEVELOPMENT_SPRINTS.md](file:///d:/AnSinhSo_Enterprise/Sprint01_Package/17_DEVELOPMENT_SPRINTS.md)
- [24_SPRINT_02_IMPLEMENTATION_PLAN.md](file:///d:/AnSinhSo_Enterprise/24_SPRINT_02_IMPLEMENTATION_PLAN.md)
