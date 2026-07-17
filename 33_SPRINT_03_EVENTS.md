# 33_SPRINT_03_EVENTS.md
## SPRINT 03 – DOMAIN EVENTS SPECIFICATION
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 33_SPRINT_03_EVENTS
- **Title:** Domain Events Architecture Specification
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
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Domain Event Architecture.
- KHÔNG ĐƯỢC thay đổi Event Collection Strategy.
- KHÔNG ĐƯỢC thay đổi Dependency Rules của Event.
- Mọi thay đổi phát sinh bắt buộc phải thông qua một Revision mới và được phê duyệt lại trước khi áp dụng.

---

# 4. Purpose
Tài liệu này là đặc tả kiến trúc (Architecture Specification) xác định chiến lược thiết kế, lưu trữ và luân chuyển Sự kiện miền (Domain Events) bên trong tầng Domain. Mục tiêu là định hình một cơ chế giao tiếp gián tiếp (decoupled) giữa các Aggregate, giúp các phần của hệ thống phản ứng với các thay đổi trạng thái mà không cần liên kết cứng (tight coupling) với nhau.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất kiến trúc của Domain Event.
- Quy định vòng đời, cách đặt tên và chiến lược lưu trữ Event bên trong Entity.
- Phân định ranh giới và trách nhiệm Dispatch Event.
- Định nghĩa nguyên tắc bất biến (Immutability) và độc lập Framework.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C#, Pseudo Code, XML, JSON hay UML.
- KHÔNG định nghĩa Event Handlers (thuộc tầng Application).
- KHÔNG cấu hình MediatR, RabbitMQ, Kafka hay Outbox Pattern.
- KHÔNG mô tả việc lưu Event vào Database (Event Sourcing).

---

# 6. Objectives
- Đảm bảo tính Persistence Ignorance và Framework Ignorance tuyệt đối cho mọi Sự kiện miền.
- Thiết lập cơ sở vững chắc cho Event-Driven Architecture nội bộ (In-Process Messaging).
- Ngăn chặn việc lạm dụng Event để xử lý logic nghiệp vụ tuần tự thay vì dùng luồng chính.
- Chuẩn hóa quá trình Raise, Store, và Clear Event an toàn, không rò rỉ bộ nhớ.

---

# 7. Domain Event Architecture
**Giải thích cốt lõi:**
- **Domain Event là gì:** Là một bản ghi bất biến (Immutable Record) mô tả một sự thật đã xảy ra trong quá khứ liên quan tới nghiệp vụ cốt lõi.
- **Tính phản ánh (Reflection):** Sự kiện chỉ phản ánh điều đã xảy ra (ví dụ: Công dân đã được đăng ký), nó KHÔNG phải là một lệnh (Command) yêu cầu hệ thống làm gì đó.
- **Không điều khiển nghiệp vụ (No Control Flow):** Không dùng Event để thực hiện các luồng if-else cốt lõi hay tính toán nghiệp vụ trực tiếp bên trong Aggregate.
- **Persistence & Framework Ignorance:** Base Event của Domain không được kế thừa từ `INotification` (MediatR), không mang Attributes của ORM, hoàn toàn thuần túy C#.

---

# 8. Event Creation Order
Quy trình và thứ tự sinh mã (chỉ áp dụng khi vào giai đoạn Coding):
- **Step 1: Base Event:** Khởi tạo lớp/interface cơ sở (`IDomainEvent`).
- **Step 2: Concrete Event:** Khởi tạo các sự kiện cụ thể (`CitizenRegisteredEvent`).
- **Step 3: Raise:** Triển khai logic tạo sự kiện trong các phương thức của Entity.
- **Step 4: Collect:** Cấu hình Collection lưu trữ sự kiện trong Base Entity.
- **Step 5: Dispatch:** (Application Layer thu thập từ UnitOfWork và gửi đi).
- **Step 6: Clear:** Xóa sự kiện sau khi dispatch thành công.
- **Step 7: Freeze:** Đóng băng bộ khung.

---

# 9. Event Evolution Strategy
Lộ trình phát triển của Event qua các Sprint:
- **Sprint 03:** Khởi tạo Seed Work (Interface cơ bản và cơ chế Collection trong Entity).
- **Sprint 04:** Business Events (Tạo các sự kiện nghiệp vụ cụ thể gắn liền với Aggregate).
- **Sprint 05:** Dispatching (Tích hợp Outbox Pattern hoặc Dispatcher tại tầng Infrastructure/Application).
- **Sprint 06:** Event Handling (Xây dựng các Handlers để phản ứng chéo giữa các Modules/Bounded Contexts).

---

# 10. Event Principles
Nguyên tắc bắt buộc cho mọi Event:
- **Immutable:** Không thể thay đổi nội dung sau khi khởi tạo (ưu tiên dùng `record` hoặc `IReadOnly` properties).
- **Past Tense Naming:** Luôn dùng thì quá khứ phân từ.
- **No Business Logic:** Event không chứa bất kỳ logic tính toán hay phương thức xử lý nào.
- **No Database:** Không có Foreign Keys, không liên kết DB.
- **No API:** Không dùng để render trực tiếp ra UI hoặc API Response.
- **No Infrastructure:** Không phụ thuộc I/O.
- **No HTTP:** Không mang thông tin Session, Token.
- **No Logging:** Tầng Domain không nhúng Logger vào Event.

---

# 11. Event Lifecycle
Vòng đời của một Domain Event:
1. **Raise:** Một hành động nghiệp vụ (Method) trên Entity kết thúc thành công, tạo ra một Event instance.
2. **Store:** Event được thêm vào danh sách nội bộ của Base Entity.
3. **Collect:** Tầng Application (UnitOfWork hoặc Interceptor) quét tất cả các Entities đang bị thay đổi (Tracked) để lấy danh sách Events.
4. **Dispatch:** Tầng Application phát (Publish) các Events này ra In-memory Bus (như MediatR).
5. **Handle:** Các Event Handlers (nằm ngoài Domain) tiếp nhận và thi hành side-effects (Gửi Email, Cập nhật Aggregate khác).
6. **Clear:** Danh sách Events trong Entity được xóa sạch để tránh phát trùng lặp ở lần lưu tiếp theo.

---

# 12. Event Collection Strategy
- **Entity giữ danh sách Event:** Mọi Domain Event phát sinh phải được lưu trữ tạm thời trong chính Entity đã tạo ra nó (cụ thể là Base Entity).
- **ReadOnly Collection:** Danh sách này phơi bày ra ngoài dưới dạng `IReadOnlyCollection` để bảo vệ tính đóng gói.
- **Add:** Entity cung cấp phương thức nội bộ `protected void AddDomainEvent(IDomainEvent eventItem)`.
- **Clear:** Cung cấp phương thức `public void ClearDomainEvents()` chỉ để gọi từ Infrastructure/Application sau khi Dispatch.
- **Không Publish trực tiếp:** Domain Layer tuyệt đối KHÔNG gọi trực tiếp các Bus/Broker (như `_mediator.Publish()`).

---

# 13. Event Dispatch Strategy
Phân định rõ trách nhiệm khi Dispatch:
- **Domain:** Chỉ chịu trách nhiệm Tạo (Raise) và Giữ (Store). Không biết Event sẽ đi đâu.
- **Application:** Chịu trách nhiệm Thu thập (Collect) và Phát (Dispatch) thông qua các Mediator nội bộ (In-process).
- **Infrastructure:** Chịu trách nhiệm Lưu trữ bền vững (Outbox Pattern) nếu cần đảm bảo Transactional Outbox, hoặc đưa ra Message Broker ngoại vi (Kafka/RabbitMQ).
- **Presentation:** Phản ứng lại thông qua SignalR/WebSockets nếu cần cập nhật giao diện thời gian thực (Real-time).

---

# 14. Naming Convention
Quy tắc đặt tên bắt buộc (Thì quá khứ - Past Tense):

**Allowed (Cho phép):**
- `CitizenRegisteredEvent`
- `BenefitApprovedEvent`
- `PaymentCompletedEvent`
- `HouseholdCreatedEvent`

**Not Allowed (Tuyệt đối cấm):**
- `CitizenEvent` (Quá chung chung, không rõ trạng thái).
- `Event1` (Vi phạm Clean Code).
- `BusinessEvent` (Tên không phản ánh nghiệp vụ cụ thể).
- `DataEvent` (Nghe giống lỗi hạ tầng hoặc CRUD).
- `RegisterCitizenEvent` (Động từ nguyên thể, đây là Tên của Command, không phải Event).

---

# 15. Folder Strategy
Cấu trúc thư mục định hướng cho các Domain Events:
- `Events/`
  - `Common/` (Các events chung nếu có)
  - `Modules/`
    - `Demographic/` (VD: `CitizenRegisteredEvent`)
    - `SocialSecurity/` (VD: `BenefitApprovedEvent`)
    - `Disbursement/` (VD: `PaymentCompletedEvent`)
    - `Communication/`
    - `Analytics/`

---

# 16. Dependency Rules
Ràng buộc phụ thuộc cứng:

**Allowed Dependencies:**
- `System.*` (Cốt lõi .NET, `System.DateTime`, `System.Guid`, v.v.).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `EntityFrameworkCore`
- `Dapper`
- `MediatR` (Không dùng `INotification` tại Domain Layer).
- `ASP.NET`
- `SignalR`
- `Redis`
- `Newtonsoft.Json`
- `Logging` (Serilog, NLog)
- `Database Provider`

---

# 17. Event Ordering Strategy
- **Theo thứ tự (Sequential):** Các sự kiện được `Add` vào danh sách của Entity theo đúng thứ tự lịch sử phát sinh.
- **Không Publish đảo thứ tự:** Khi Application thu thập và Dispatch, bắt buộc phải duy trì thứ tự FIFO (First-In-First-Out).
- **Không chạy song song trong Domain:** Tầng Domain là đơn luồng (Single-threaded) trong phạm vi của một Request/Command, không có khái niệm Raise event đa luồng để tránh Race Condition trên danh sách nội bộ.

---

# 18. Validation Rules
Kiến trúc Event được xem là hợp lệ khi:
- **No HTTP, No API:** Không phụ thuộc Web context.
- **No EF, No ORM:** Không dùng tính năng tracking của DB.
- **No Logging, No Infrastructure:** Thuần túy logic.
- **Immutable:** Không thể bị sửa đổi (set) sau khi sinh ra.
- **Past Tense:** Đặt tên đúng ngữ pháp.
- **No Business Logic:** Không chứa methods như `Calculate()` hay `Validate()` bên trong Event.

---

# 19. AI Constraints
Giới hạn bắt buộc của AI Coding Agent khi tạo tài liệu này:
- KHÔNG sinh mã C# dưới bất kỳ hình thức nào.
- KHÔNG sinh Pseudo Code hay JSON.
- KHÔNG vẽ UML.
- KHÔNG định nghĩa Event Handler (như `IRequestHandler` hay `INotificationHandler`).
- KHÔNG nhúng Interface của thư viện MediatR vào.

---

# 20. Definition of Done
Tài liệu đạt tiêu chuẩn hoàn thành khi:
- Mô tả toàn diện và khép kín vòng đời của Sự kiện miền.
- Cô lập hoàn toàn Domain Layer khỏi trách nhiệm Dispatching và Message Broker.
- Chuẩn hóa chiến lược đặt tên và lưu trữ cục bộ tại Aggregate.
- Tương thích 100% với nguyên tắc Persistence Ignorance và Framework Ignorance.

---

# 21. Validation Matrix
Bảng ma trận tự kiểm tra sự tuân thủ:

| Tiêu chí | Nội dung kiểm tra | Đánh giá |
|---|---|---|
| **Architecture** | Domain chỉ Raise/Store, Application mới Dispatch. | Bắt buộc. |
| **DDD** | Phản ánh chân thực điều đã xảy ra (Past Tense). | Bắt buộc. |
| **Dependency** | Loại trừ triệt để MediatR và ORM khỏi Domain. | Bắt buộc. |
| **Immutability** | Đảm bảo tính Read-only của Object và Collection. | Bắt buộc. |
| **Compile Ready** | (Kiểm tra ở Phase Coding) Cấu trúc hợp lệ cho C# record/class. | Bắt buộc. |

---

# 22. Risks
- **Duplicate Events:** Nếu không `Clear` danh sách sau khi Dispatch, sự kiện có thể bị bắn đi nhiều lần trong cùng một phiên sống của đối tượng.
  - *Giải pháp:* Cấu trúc nghiêm ngặt hàm `ClearDomainEvents()` tại Application.
- **Missing Events:** Entity ném Event nhưng Application Layer quên Collect.
  - *Giải pháp:* Cấu hình Interceptor trong EF Core (tại tầng Infrastructure) để quét tự động trước khi `SaveChanges`.
- **Out of Order:** Dispatch Event không đúng thứ tự gây sai lệch State ở các hệ thống downstream.
  - *Giải pháp:* Đảm bảo cấu trúc danh sách (List) bảo toàn thứ tự FIFO.
- **Event Storm:** Một Aggregate sinh ra quá nhiều Event vô nghĩa hoặc lặp lặp cho những thay đổi nhỏ.
  - *Giải pháp:* Gộp trạng thái và chỉ phát Event mang ý nghĩa nghiệp vụ lớn.
- **Framework Leakage:** Developer vô tình cho Domain Event kế thừa `MediatR.INotification` để tiện lợi.
  - *Giải pháp:* Tạo một base Wrapper/Adapter ở tầng Application, tuyệt đối không ô nhiễm Domain.

---

# 23. AI Review Checklist
- [x] Không sinh C#, Pseudo Code, UML, XML, JSON?
- [x] Đã thiết lập Architecture Freeze Rule?
- [x] Đã mô tả Event Architecture, Creation Order, Evolution Strategy?
- [x] Đã thiết lập Naming Convention (thì quá khứ)?
- [x] Đã tuân thủ nghiêm ngặt Dependency Rules (Cấm MediatR, EF Core)?
- [x] Đã có Event Lifecycle, Dispatch Strategy và Collection Strategy?
- [x] Tài liệu tuân thủ Enterprise Documentation Suite?

---

# 24. References
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
- 28_SPRINT_02_REVIEW.md
- 29_SPRINT_03_IMPLEMENTATION_PLAN.md
- 30_SPRINT_03_PROJECT_INIT.md
- 31_SPRINT_03_BASE_CLASSES.md
- 32_SPRINT_03_EXCEPTIONS.md

---
# END OF SPECIFICATION
