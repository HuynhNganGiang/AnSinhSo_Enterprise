# 27_AGGREGATE_DESIGN.md
## SPRINT 02 – DOMAIN ARCHITECTURE & MODELING
Version: 1.1.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 27_AGGREGATE_DESIGN
- **Title:** Enterprise Aggregate Design Specification
- **Phase:** Sprint 02
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Software Architect, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft |
| 1.1.0 | 2026-07-17 | Solution Architecture Team | Added Decision Matrix, Evolution Strategy, Dependency Matrix, Quality Attributes, AI Checklist |

---

# 3. Purpose
Tài liệu này định nghĩa các nguyên tắc và triết lý thiết kế Aggregate ở mức kiến trúc (Architecture Specification) cho dự án AnSinhSo Enterprise theo phương pháp Domain-Driven Design (DDD).

Tài liệu này tuyệt đối không chứa cấu trúc dữ liệu cụ thể, không mô hình hóa các nghiệp vụ chi tiết, không sinh mã C#/SQL/UML và không định nghĩa các Entity hay Value Object vật lý. 

---

# 4. Aggregate Design Philosophy
- **Consistency over Flexibility:** Đảm bảo tính nhất quán của dữ liệu nghiệp vụ quan trọng hơn sự linh hoạt trong truy vấn. Aggregate được sinh ra để duy trì tính toàn vẹn này.
- **True Invariants Protector:** Aggregate tồn tại duy nhất để bảo vệ các quy tắc bất biến (Invariants) cốt lõi của nghiệp vụ. Nếu không có Invariants cần bảo vệ giữa các phần tử dữ liệu, không cần tạo Aggregate.
- **Decoupled by Design:** Các Aggregate phải hoạt động độc lập và hoàn toàn không dính líu chặt chẽ đến nhau về mặt bộ nhớ hay giao dịch cơ sở dữ liệu.

---

# 5. Aggregate Responsibilities
- **Transaction Manager:** Chịu trách nhiệm bao bọc toàn bộ trạng thái của một đối tượng nghiệp vụ phức tạp trong một giao dịch duy nhất.
- **Rule Enforcer:** Đảm bảo rằng mọi thay đổi trạng thái của bất kỳ Entity hay Value Object nào bên trong nó đều tuân thủ và không phá vỡ quy tắc nghiệp vụ.
- **Event Publisher:** Khởi tạo và quản lý các Domain Event để thông báo cho hệ thống biết về các thay đổi quan trọng vừa diễn ra bên trong ranh giới của nó.

---

# 6. Aggregate Boundary
- Ranh giới của Aggregate (Aggregate Boundary) xác định phạm vi mà tại đó sự nhất quán tuyệt đối (Immediate Consistency) phải được đảm bảo.
- Bất cứ Entity hay Value Object nào nằm trong ranh giới này đều phải cùng tồn tại, cùng thay đổi và cùng bị hủy diệt với Aggregate Root.
- Bất cứ thứ gì nằm ngoài ranh giới này chỉ được phép duy trì tính nhất quán cuối cùng (Eventual Consistency).

---

# 7. Aggregate Consistency Rules
- Mọi thao tác thay đổi trạng thái (Create, Update, Delete) bên trong Aggregate phải luôn đảm bảo trạng thái toàn cục của hệ thống là hợp lệ sau khi thao tác hoàn tất.
- Không được phép lưu trữ một Aggregate vào hạ tầng (Infrastructure) nếu nó đang trong trạng thái vi phạm Invariants.

---

# 8. Aggregate Transaction Boundary
- Một giao dịch thay đổi trạng thái (Transaction) chỉ được phép tác động đến **MỘT và CHỈ MỘT** Aggregate tại một thời điểm.
- Việc cập nhật nhiều Aggregate trong cùng một giao dịch đồng bộ (Multi-Aggregate Transaction) là một hành vi vi phạm kiến trúc nghiêm trọng, dẫn đến thắt cổ chai hiệu suất.
- Nếu một hành động nghiệp vụ cần cập nhật nhiều Aggregate, phải sử dụng Domain Events để thực thi việc cập nhật các Aggregate khác ở các giao dịch độc lập (Eventual Consistency).

---

# 9. Aggregate Root Responsibilities
- **Aggregate Root (AR)** là Entity duy nhất đóng vai trò cửa ngõ giao tiếp của toàn bộ Aggregate.
- Mọi nỗ lực truy xuất hoặc cập nhật các Entity con/Value Object bên trong đều bắt buộc phải gọi thông qua phương thức (method) của Aggregate Root.
- AR chịu trách nhiệm khởi tạo trạng thái ban đầu của toàn bộ Aggregate thông qua Factory Method hoặc Constructor an toàn.

---

# 10. Aggregate Collaboration
- Các Aggregate tương tác với nhau chủ yếu bằng cách truyền tin thông qua Domain Events.
- Nếu Aggregate A cần thông tin từ Aggregate B để xử lý logic, thông tin đó nên được truy vấn riêng biệt tại Application Layer và truyền vào phương thức của Aggregate A như một tham số (dưới dạng Value Object nguyên thủy), thay vì Aggregate A trực tiếp giữ tham chiếu và gọi hàm của Aggregate B.

---

# 11. Aggregate Size Guidelines
- **Small is Better:** Ưu tiên thiết kế Aggregate càng nhỏ càng tốt. Một Aggregate lý tưởng thường chỉ chứa duy nhất Aggregate Root và một vài Value Object.
- Tránh đưa quá nhiều Entity con vào một Aggregate trừ khi chúng thực sự phải chia sẻ chung một quy tắc Invariant nghiêm ngặt không thể tách rời.
- Aggregate quá lớn gây ra các vấn đề nghiêm trọng về hiệu suất (vì phải tải toàn bộ cây đối tượng lên bộ nhớ) và dễ dẫn đến xung đột giao dịch (nhiều người dùng cùng sửa một phần của Aggregate).

---

# 12. Aggregate Reference Rules
- Aggregate **KHÔNG ĐƯỢC PHÉP** giữ tham chiếu đối tượng (object reference) trực tiếp đến một Aggregate khác.
- Aggregate chỉ được phép giữ **Định danh (ID)** của Aggregate khác (ID Reference).
- Quy tắc này giúp đảm bảo sự rạch ròi về ranh giới giao dịch và ngăn chặn các ORM vô tình tải kèm dữ liệu thừa hoặc khóa (lock) nhiều bảng cùng lúc.

---

# 13. Aggregate Lifecycle
- **Creation:** Aggregate chỉ được tạo ra thông qua Factory (hoặc các phương thức khởi tạo do Aggregate Root cung cấp), đảm bảo Invariants được kiểm tra nghiêm ngặt ngay từ đầu.
- **Reconstitution:** Quá trình tải Aggregate từ cơ sở dữ liệu lên bộ nhớ thông qua Repository không được kích hoạt các logic nghiệp vụ (ví dụ: không phát sinh lại Domain Event).
- **Modification:** Chỉ diễn ra thông qua các hành vi (Behavior) rõ ràng của Aggregate Root.
- **Deletion:** Xử lý cẩn thận việc xóa. Thường ưu tiên cờ xóa mềm (Soft Delete) đối với Aggregate Root và phát sinh Domain Event để dọn dẹp dữ liệu liên đới ở các Bounded Context khác.

---

# 14. Aggregate Invariants
- Invariants là các quy tắc kinh doanh cốt lõi phải luôn luôn đúng tại bất kỳ thời điểm nào đối với một Aggregate (ví dụ: "Tổng chi trả không bao giờ vượt ngân sách", "Ngày kết thúc phải lớn hơn ngày bắt đầu").
- Invariants phải được kiểm tra trước khi trạng thái của Aggregate thay đổi. Nếu kiểm tra thất bại, hệ thống phải ném ra ngoại lệ (Domain Exception) và hủy giao dịch.

---

# 15. Aggregate Validation Rules
- **Domain Validation (Business Rule Validation):** Là việc kiểm tra các Invariants. Nó phải nằm rải rác bên trong chính các phương thức nghiệp vụ của Aggregate Root. 
- **Application Validation (Data Format Validation):** Là việc kiểm tra định dạng dữ liệu (Email hợp lệ, chuỗi không rỗng, v.v.). Phần này nằm ngoài Aggregate (thường ở Application Layer thông qua thư viện validator) và phải được xác thực trước khi dữ liệu chạm tới Aggregate.

---

# 16. Aggregate Performance Considerations
- Aggregate Root không nên chứa các collection (danh sách Entity con) quá lớn. Việc lấy toàn bộ collection lên bộ nhớ mỗi lần thay đổi trạng thái sẽ làm sập hiệu suất hệ thống.
- Trong trường hợp buộc phải xử lý collection cực lớn, cân nhắc tách Entity con đó thành một phần của Aggregate khác.
- Tuyệt đối cấm sử dụng Lazy Loading bên trong Aggregate vì nó phá vỡ tính bao gói và gây khó khăn lớn cho việc kiểm soát I/O. Eager Loading toàn bộ Aggregate một lần thông qua Repository là bắt buộc.

---

# 17. Aggregate Anti-patterns
Các mẫu lỗi cần tránh khi thiết kế Aggregate:
- **The Blob Aggregate:** Một Aggregate "khổng lồ" chứa hàng tá Entity và Value Object. Dẫn đến xung đột cập nhật (Concurrency) và hiệu suất tồi tệ.
- **Anemic Aggregate:** Aggregate Root không chứa logic bảo vệ Invariants, chỉ phơi bày setter public để Application Layer tự do thay đổi.
- **Cross-Aggregate Updates:** Một thao tác nghiệp vụ cố gắng thay đổi trực tiếp hai Aggregate khác nhau trong cùng một hàm và lưu lại bằng chung một giao dịch.
- **UI-Driven Boundaries:** Thiết kế ranh giới Aggregate dựa trên thiết kế giao diện màn hình (gom hết những gì cần hiển thị vào một Aggregate) thay vì phân tích các Invariants nghiệp vụ.

---

# 18. Aggregate Decision Matrix
Bảng quyết định để đánh giá xem một nhóm Entity/Value Object có nên trở thành một Aggregate hay không:

| Tiêu chí | Điểm trọng số | Hành động nếu ĐẠT | Hành động nếu KHÔNG ĐẠT |
|---|---|---|---|
| Có quy tắc nghiệp vụ bất biến (Invariant) chung không? | Cao | Tiến hành nhóm thành Aggregate | Tách ra thành các Entity/Aggregate độc lập |
| Có yêu cầu tính nhất quán tức thời (Immediate Consistency) không? | Cao | Gộp vào cùng một Aggregate | Cân nhắc sử dụng Eventual Consistency |
| Có khả năng bị nhiều User cập nhật đồng thời không (Concurrency)? | Trung bình | Giữ Aggregate thật nhỏ (chỉ chứa AR) | Có thể để Aggregate lớn hơn một chút nếu rủi ro thấp |
| Kích thước dữ liệu khi truy xuất (Load Size) có quá lớn không? | Cao | TÁCH thành nhiều Aggregate độc lập | Giữ nguyên giới hạn Aggregate |

---

# 19. Aggregate Evolution Strategy (Sprint 03–06)
Chiến lược tiến hóa của cấu trúc Aggregate qua các giai đoạn triển khai mã nguồn:

- **Sprint 03 (Domain Classes & Interfaces):** 
  - Tạo class cho Aggregate Root kế thừa từ lớp `Entity` cơ sở và thực thi Interface `IAggregateRoot`.
  - Khai báo các thuộc tính (ưu tiên sử dụng Value Object) và sử dụng `private set` để chặn sửa đổi từ bên ngoài.
  - Định nghĩa các Domain Exceptions tương ứng với các vi phạm Invariants.
- **Sprint 04 (Behaviors & CQRS Integration):** 
  - Bổ sung các phương thức nghiệp vụ (Behaviors) vào Aggregate Root thay vì chỉ dùng getter/setter.
  - Cài đặt logic kiểm tra Invariants nghiêm ngặt bên trong các phương thức này.
  - Lớp Application (Command Handlers) bắt đầu triệu gọi các phương thức này để thay đổi trạng thái đối tượng.
- **Sprint 05 (Persistence & EF Core Mapping):** 
  - Định nghĩa các lớp `IEntityTypeConfiguration` ở tầng Infrastructure để ánh xạ Aggregate xuống Table.
  - Ánh xạ các Value Object bằng Owned Types (hoặc Complex Types trong EF 8).
  - TUYỆT ĐỐI không sửa đổi mã nguồn Aggregate ở tầng Domain để phục vụ cấu trúc Database.
- **Sprint 06 (API Exposure):** 
  - Aggregate không bao giờ được trả trực tiếp qua API.
  - Ánh xạ trạng thái của Aggregate sang Response DTO thông qua lớp Application.

---

# 20. Aggregate Dependency Matrix
Các ràng buộc về sự phụ thuộc đối với Aggregate ở mức mã nguồn:

| Thành phần tương tác | Trạng thái cho phép | Mô tả chi tiết |
|---|---|---|
| Tham chiếu chéo giữa các Aggregate | Chỉ sử dụng Định danh (Id) | Tuyệt đối không dùng Object Reference (Navigation Property). |
| Phụ thuộc vào Infrastructure | Cấm tuyệt đối | Aggregate không chứa Entity Framework, SQL, hay bất kỳ logic lưu trữ nào. |
| Phụ thuộc vào Application/UI | Cấm tuyệt đối | Aggregate không chứa DTOs, ViewModels, HTTP Context hay API attributes. |
| Phụ thuộc vào Domain Events | Cho phép | Aggregate có quyền khởi tạo và đưa Domain Event vào danh sách nội bộ để chờ xử lý. |

---

# 21. Aggregate Quality Attributes
Các tiêu chí cốt lõi để đánh giá chất lượng thiết kế của một Aggregate:

- **Cohesion (Độ kết dính):** Rất cao. Mọi thành phần bên trong Aggregate phải luôn thay đổi cùng nhau vì cùng một lý do nghiệp vụ.
- **Coupling (Độ phụ thuộc):** Rất thấp. Aggregate không được phụ thuộc trực tiếp vào trạng thái bộ nhớ của các Aggregate khác.
- **Testability (Tính kiểm thử):** Xuất sắc. 100% logic bên trong Aggregate phải có thể viết Unit Test độc lập mà không cần Mock Database hay bất kỳ thành phần I/O nào.
- **Performance (Hiệu suất):** Tốt. Kích thước Aggregate phải đủ nhỏ để tải nhanh từ CSDL mà không kéo theo dữ liệu không cần thiết.

---

# 22. Aggregate Review Checklist
Checklist dành cho các thành viên trong đội ngũ kiến trúc khi đánh giá Aggregate Design:
- [ ] Aggregate có thực sự bảo vệ một Invariant nghiệp vụ nào không? (Nếu không, nó chỉ nên là cấu trúc CRUD thông thường).
- [ ] Aggregate có dính lỗi tham chiếu Object đến Aggregate khác không? (Nếu có, phải thay thế bằng tham chiếu ID).
- [ ] Có đảm bảo mọi thay đổi trạng thái đều phải gọi qua Aggregate Root?
- [ ] Aggregate có tuân thủ quy tắc "1 Transaction cho 1 Aggregate" không?
- [ ] Aggregate có nguy cơ phình to (chứa hàng ngàn Entity con) khi hệ thống chạy thực tế không?

---

# 23. AI Review Checklist
Checklist tự động dành cho AI Coding Agent trước khi tạo hoặc thay đổi mã nguồn Aggregate ở các Sprint sau:

- [ ] Aggregate Root đã chặn đứng toàn bộ việc thay đổi trạng thái tự do (sử dụng `private set` chưa)?
- [ ] Aggregate này có đang cố tình bao bọc nhiều hơn một ranh giới giao dịch (Transaction Boundary) không?
- [ ] Các tham chiếu ngoại lai đã được chuyển hoàn toàn sang ID (Guid/int) thay vì Object chưa?
- [ ] Các tham số truyền vào phương thức của Aggregate Root đã được thiết kế thành Value Object ở những chỗ cần thiết chưa?
- [ ] Aggregate đã được thiết kế để hoàn toàn vô cảm trước cơ sở dữ liệu (Persistence Ignorant) chưa?

---

# 24. AI Implementation Notes
- Khi AI Coding Agent nhận yêu cầu sinh mã nguồn trong tương lai, AI phải tự động kiểm tra xem mã thiết kế Aggregate có vi phạm các nguyên tắc: Kích thước, Tham chiếu chéo, Ranh giới giao dịch hay không.
- Bất kỳ yêu cầu nào (dù từ phía người dùng) đòi hỏi AI sinh mã để cập nhật 2 Aggregate trong cùng 1 Request/Transaction đều phải bị AI đưa ra cảnh báo và từ chối.
- AI tuyệt đối không tự suy diễn cấu trúc Aggregate chi tiết cho nghiệp vụ cụ thể của hệ thống nếu chưa có tài liệu đặc tả thiết kế rõ ràng.

---

# 25. References
- [24_SPRINT_02_IMPLEMENTATION_PLAN.md](file:///d:/AnSinhSo_Enterprise/24_SPRINT_02_IMPLEMENTATION_PLAN.md)
- [25_DOMAIN_ARCHITECTURE.md](file:///d:/AnSinhSo_Enterprise/25_DOMAIN_ARCHITECTURE.md)
- [26_DOMAIN_MODEL_GUIDE.md](file:///d:/AnSinhSo_Enterprise/26_DOMAIN_MODEL_GUIDE.md)
