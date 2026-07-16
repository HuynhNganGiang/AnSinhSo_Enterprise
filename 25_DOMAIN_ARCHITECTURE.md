# 25_DOMAIN_ARCHITECTURE.md
## SPRINT 02 – DOMAIN ARCHITECTURE & MODELING
Version: 1.3.0
Status: Draft
Revision: Final Review Candidate
Project: AnSinhSo Enterprise
Last Updated: 2026-07-16

---

# 1. Architecture Goals

Định nghĩa kiến trúc tầng Domain (Domain Layer) của hệ thống AnSinhSo Enterprise theo phương pháp tiếp cận Domain-Driven Design (DDD) và Clean Architecture. Tài liệu này thiết lập các ngôn ngữ chung (Ubiquitous Language), ranh giới bối cảnh (Bounded Context) và các nguyên tắc thiết kế nhằm đảm bảo tầng Domain giữ vai trò trung tâm, lưu trữ trọn vẹn nghiệp vụ an sinh xã hội, hoàn toàn độc lập với các công nghệ bên ngoài (Database, API, Message Broker).

---

# 2. Domain Layer Overview

Domain Layer là trái tim của hệ thống AnSinhSo Enterprise, chứa toàn bộ logic nghiệp vụ cốt lõi và các quy tắc bất biến (Invariants) của tổ chức. 

Trong kiến trúc Enterprise Clean Architecture được quy định tại `08_BACKEND_STANDARDS.md`, tầng Domain nằm ở vị trí trung tâm nhất. Nó tuyệt đối không phụ thuộc vào bất kỳ tầng nào khác (Infrastructure, Application hay Presentation). Tất cả các module, thư viện và framework bên ngoài đều phải hướng sự phụ thuộc vào tầng Domain thông qua Dependency Inversion.

---

# 3. Ubiquitous Language

Ngôn ngữ chung được sử dụng xuyên suốt giữa đội ngũ phát triển, chuyên gia nghiệp vụ và được phản ánh trực tiếp vào kiến trúc:

- **Công dân (Citizen):** Cá nhân sinh sống trên địa bàn.
- **Hộ gia đình (Household):** Tập hợp các công dân có quan hệ hộ tịch, cư trú cùng nhau.
- **Địa bàn (Location/Area):** Đơn vị hành chính quản lý (Thôn, Xóm, Xã).
- **Nhóm đối tượng (Beneficiary Group):** Phân loại các đối tượng được hưởng chính sách an sinh (ví dụ: hộ nghèo, người có công, người cao tuổi...).
- **Chính sách (Policy):** Các quy định, mức hưởng trợ cấp áp dụng cho các nhóm đối tượng.
- **Chi trả (Payment/Disbursement):** Hành động/kỳ giải ngân tiền trợ cấp cho đối tượng an sinh.
- **Bản đồ số (GIS Map):** Hệ thống thông tin địa lý thể hiện sự phân bố của đối tượng an sinh (áp dụng Latitude, Longitude, Geometry).

---

# 4. Bounded Context

Hệ thống được quy hoạch thành các Bounded Context độc lập để quản lý độ phức tạp và đảm bảo khả năng mở rộng (Scalability):

1. **Identity & Access Management (IAM) Context:** Quản lý người dùng, phân quyền (Role, Permission), xác thực (Authentication).
2. **Demographic Context (Core Context):** Quản lý thông tin nền tảng về con người: Công dân, Hộ gia đình, Địa bàn. Đóng vai trò Single Source of Truth cho các context khác.
3. **Social Security Context (Core Context):** Chuyên trách quản lý Chính sách an sinh và Nhóm đối tượng hưởng lợi. Chứa các quy tắc nghiệp vụ phức tạp nhất để quyết định tính hợp lệ của đối tượng.
4. **Disbursement Context (Core Context):** Quản lý nghiệp vụ tài chính, các đợt chi trả, lịch sử nhận tiền, Transaction và đối soát ngân sách.
5. **GIS Context:** Quản lý không gian, vị trí địa lý của Hộ gia đình/Công dân hỗ trợ Spatial Query theo tiêu chuẩn quy định.
6. **Communication Context:** Đảm nhiệm kênh giao tiếp ra bên ngoài: Tích hợp Zalo OA, thông báo (Notification).
7. **Analytics Context:** Xử lý các truy vấn AI Assistant, tổng hợp dữ liệu, xuất Báo cáo, Thống kê.

---

# 5. Context Map

Mô tả mối quan hệ và luồng giao tiếp giữa các Bounded Context:

- **Demographic Context** đóng vai trò là **Upstream** cho **Social Security Context** và **Disbursement Context** (Downstream). Các hệ thống downstream không sao chép dữ liệu mà chỉ tham chiếu định danh công dân/hộ gia đình.
- **Social Security Context** cung cấp danh sách đối tượng hợp lệ cho **Disbursement Context** để tiến hành các kỳ chi trả thông qua cơ chế tích hợp nội bộ.
- Sự kiện thay đổi trạng thái từ các Core Contexts sẽ được publish để **GIS Context** và **Analytics Context** (Downstream Consumers) cập nhật dữ liệu của mình một cách bất đồng bộ.

---

# 6. Layer Responsibilities

- **Domain Layer:** Định nghĩa các mô hình cốt lõi, quy tắc bất biến, và hợp đồng giao tiếp (Interfaces). Chứa 100% Business Logic. Không giao tiếp trực tiếp với SQL Server hay bất kỳ thiết bị I/O nào.
- **Application Layer:** Triển khai các Use Case (Commands/Queries), điều phối các đối tượng Domain.
- **Infrastructure Layer:** Cung cấp implementation cụ thể cho các hợp đồng giao tiếp (ví dụ: DB Context, Repositories, External APIs).
- **Presentation Layer (API):** Tiếp nhận REST/HTTP Request, xác thực, gọi vào Application Layer và trả về HTTP Response.

---

# 7. Dependency Rule

Tuân thủ nghiêm ngặt nguyên lý phụ thuộc của Clean Architecture:

- Chiều mũi tên phụ thuộc: `Infrastructure -> Application -> Domain <- Presentation`
- **Tầng Domain KHÔNG CÓ bất kỳ reference nào đến các tầng khác.**
- Giao tiếp với ngoại vi được thực hiện qua mô hình Đảo ngược phụ thuộc (Dependency Inversion).

---

# 8. Allowed Dependencies

Tại tầng Domain, CHỈ được phép tham chiếu tới:

- Cấu trúc dữ liệu và logic toán học cơ bản.
- Các khái niệm ngôn ngữ lập trình thuần túy.
- Các Interfaces chuẩn mức độ cơ sở.

*(Ghi chú: Tầng Domain phải đảm bảo 100% Persistence Ignorance, hoàn toàn vô cảm trước các framework bên ngoài).*

---

# 9. Forbidden Dependencies

Tầng Domain BỊ CẤM TUYỆT ĐỐI tham chiếu tới:

- Bất kỳ framework ORM nào.
- Bất kỳ công nghệ truy cập cơ sở dữ liệu nào.
- Bất kỳ Web Framework nào.
- Bất kỳ thư viện liên quan đến I/O, File System, Network, hay External APIs.

---

# 10. Domain Package Structure

Cấu trúc Package (Logical) bên trong tầng Domain được phân chia theo chiều dọc (Vertical Slices) tương ứng với các Bounded Context, sau đó mới chia theo chiều ngang (Building Blocks). Điều này đảm bảo tính đóng gói (High Cohesion) cho từng cụm nghiệp vụ.

---

# 11. Folder Structure

Cấu trúc thư mục định hướng mức kiến trúc cho dự án `AnSinhSo.Domain`:

```text
AnSinhSo.Domain/
├── Common/
├── Abstractions/
├── Exceptions/
├── Interfaces/
├── Modules/
│   ├── Demographic/
│   ├── SocialSecurity/
│   ├── Disbursement/
│   ├── GIS/
│   ├── Communication/
│   └── Analytics/
└── Shared/
```

---

# 12. Architecture Principles

1. **Persistence Ignorance:** Các cấu trúc Domain hoàn toàn không biết cách chúng được lưu trữ hay truy vấn (không có Data Annotation, không có mapping Database bên trong Domain).
2. **Always Valid State:** Một thành phần Domain chỉ có thể được khởi tạo khi nó ở trạng thái hợp lệ. Các Invariants phải được validate ngay lập tức.
3. **Rich Domain Model:** Business logic phải nằm trọn vẹn bên trong các cấu trúc Domain thay vì biến chúng thành các thực thể chỉ chứa Data (Anemic Domain Model).
4. **Aggregate Roots as Consistency Boundaries:** Mọi thao tác thay đổi trạng thái nội bộ đều phải được thực hiện thông qua ranh giới quản lý để đảm bảo tính toàn vẹn dữ liệu (Transaction Consistency).
5. **Business First:** Mô hình hóa nghiệp vụ ưu tiên trước, thiết kế cấu trúc dữ liệu sẽ được xử lý riêng rẽ ở tầng Infrastructure.

---

# 13. Architecture Decision Record (ADR)

Giải thích các quyết định kiến trúc cốt lõi của dự án AnSinhSo Enterprise:

- **Clean Architecture:** Được chọn để cô lập tầng Domain khỏi các thay đổi về công nghệ (như thay đổi Framework UI, thay đổi CSDL), giúp dự án dễ bảo trì và mở rộng trong dài hạn.
- **Domain Driven Design (DDD):** Được chọn do sự phức tạp của nghiệp vụ an sinh xã hội. DDD giúp đồng nhất ngôn ngữ giữa chuyên gia nghiệp vụ và kỹ sư phần mềm, chia nhỏ hệ thống thành các Bounded Context dễ quản lý.
- **Vertical Slice:** Được chọn để tổ chức kiến trúc theo từng Feature (chức năng nghiệp vụ) thay vì theo Technical Layer đơn thuần, giúp tăng độ kết dính (Cohesion) và dễ dàng tái cấu trúc (Refactoring).
- **CQRS Ready:** Kiến trúc được chuẩn bị sẵn sàng cho Command Query Responsibility Segregation để tách biệt luồng ghi (Command - thay đổi trạng thái phức tạp) và luồng đọc (Query - tối ưu hóa tốc độ báo cáo/bản đồ).
- **Persistence Ignorance:** Quyết định không phụ thuộc vào hạ tầng lưu trữ trong tầng Domain để bảo vệ các logic kinh doanh cốt lõi không bị "ô nhiễm" bởi các thuộc tính của CSDL.

---

# 14. Cross-cutting Concerns

Mô tả cách tầng Domain xử lý các vấn đề cắt ngang ở mức kiến trúc:

- **Validation:** Chỉ xử lý *Domain Validation* (các quy tắc bất biến nghiệp vụ - Invariants). Các validation về form/input data không nằm ở đây.
- **Domain Exception:** Sử dụng các ngoại lệ riêng đại diện cho các lỗi vi phạm quy tắc nghiệp vụ.
- **Domain Event:** Được sử dụng để giao tiếp giữa các cụm nghiệp vụ hoặc giữa các Bounded Context mà không tạo ra sự ràng buộc chặt chẽ (Coupling).
- **Business Rule:** Mọi Invariants và Logic tính toán bắt buộc phải được đóng gói vào trong các mô hình Domain, không được để lộ ra ngoài.
- **Audit Metadata Strategy:** Định nghĩa cơ chế theo dõi sự thay đổi của dữ liệu thông qua các khái niệm trừu tượng ở mức kiến trúc. Không gắn chặt vào bất kỳ cấu trúc cụ thể nào, đảm bảo tính nhất quán trong việc ghi vết lịch sử trên toàn hệ thống mà không làm ô nhiễm logic nghiệp vụ.
- **Time:** Sử dụng nhà cung cấp thời gian trừu tượng thay vì gọi trực tiếp thời gian hệ thống để hỗ trợ Unit Testing.
- **Identity:** Tầng Domain không phụ thuộc vào framework Identity. Việc trích xuất User Identity được xử lý ở Application Layer và truyền vào Domain thông qua tham số.

---

# 15. Module Dependency Matrix

Ma trận thể hiện chiều phụ thuộc giữa các tầng kiến trúc:

| Tầng (Layer)   | Được phép phụ thuộc (Allowed) | Bị cấm phụ thuộc (Forbidden) |
|----------------|-------------------------------|------------------------------|
| **Domain**     | None                          | Application, Infrastructure, API |
| **Application**| Domain                        | Infrastructure, API |
| **Infrastructure**| Domain, Application         | API |
| **API**        | Application, Infrastructure   | Domain (không gọi trực tiếp logic) |

**Dependency Flow**

Presentation(API)
↓
Application
↓
Domain
↑
Infrastructure

- **Infrastructure implements Domain contracts:** Tầng Infrastructure chịu trách nhiệm cung cấp sự triển khai cụ thể cho các hợp đồng (Interfaces) đã được định nghĩa tại Domain Layer.
- **Application depends on Domain:** Tầng Application đóng vai trò điều phối, sử dụng các khái niệm và logic nghiệp vụ từ Domain Layer để thực thi Use Case.
- **API never bypasses Application:** Tầng API không bao giờ được phép đi tắt (bypass) để gọi trực tiếp vào Domain Layer hoặc Infrastructure Layer nhằm xử lý logic; mọi yêu cầu phải thông qua Application Layer.

---

# 16. Architecture Constraints

Các giới hạn ràng buộc (Constraints) tuyệt đối của tầng Domain:

Tầng Domain **must never**:
- reference EF Core
- reference SQL
- reference ASP.NET Core
- reference IConfiguration
- reference ILogger
- reference HttpContext
- reference JsonSerializer
- reference Zalo SDK

---

# 17. Architecture Quality Attributes

Đánh giá chất lượng của kiến trúc Domain đang thiết kế:

- **Maintainability:** Cao. Việc cô lập hoàn toàn business logic giúp mã nguồn dễ đọc và nâng cấp.
- **Testability:** Rất cao. Có thể viết Unit Test cho 100% logic nghiệp vụ mà không phụ thuộc I/O.
- **Scalability:** Cao. Thiết kế theo Bounded Context hỗ trợ tách thành các service độc lập trong tương lai.
- **Performance:** Tối ưu thông qua cơ chế CQRS (tách biệt Command/Query).
- **Extensibility:** Cao. Dễ dàng thêm tính năng mới mà không sửa đổi tính năng cũ (Open/Closed Principle).
- **Security:** Tầng Domain mặc định an toàn do được bao bọc bởi Application Layer và API Layer.

---

# 18. Architecture Review Checklist

Checklist nghiệm thu Sprint 02:

- [ ] Tài liệu kiến trúc có tuân thủ hoàn toàn Clean Architecture không?
- [ ] Có xuất hiện bất kỳ công nghệ lưu trữ/Database nào trong Domain Layer không?
- [ ] Bounded Context đã phủ kín các nghiệp vụ cốt lõi chưa?
- [ ] Context Map có xác định đúng Upstream/Downstream không?
- [ ] Allowed/Forbidden Dependencies có rõ ràng không?
- [ ] Không có file code, hay thiết kế chi tiết nào được sinh ra sai quy định.

---

# 19. AI Implementation Notes

Quy định hướng dẫn AI Coding Agent thực hiện triển khai qua các Sprint kế tiếp:

### Sprint 03
| Thuộc tính | Chi tiết |
|------------|----------|
| **Objective** | Khởi tạo cấu trúc dự án Domain theo chuẩn kiến trúc. |
| **Deliverables** | Các định nghĩa hợp đồng và cấu trúc thư mục. |
| **Allowed Scope** | Thiết lập cấu trúc thư mục, các định nghĩa trừu tượng, và Domain Exceptions. |
| **Out of Scope** | Không triển khai logic Application, không đụng chạm đến cơ sở dữ liệu. |
| **Acceptance Criteria** | Cấu trúc phân chia đúng Bounded Context và không chứa dependency bên ngoài. |

### Sprint 04
| Thuộc tính | Chi tiết |
|------------|----------|
| **Objective** | Xây dựng Application Layer phục vụ Use Case. |
| **Deliverables** | Kiến trúc CQRS (Command/Query/Handler) và các thành phần xác thực dữ liệu. |
| **Allowed Scope** | Định nghĩa các luồng xử lý ứng dụng, cơ chế Validator và Application Interfaces. |
| **Out of Scope** | Không can thiệp vào cách truy xuất dữ liệu vật lý (Infrastructure). |
| **Acceptance Criteria** | Mọi yêu cầu nghiệp vụ đều có Handler tương ứng, tuân thủ nguyên tắc không chứa logic cốt lõi. |

### Sprint 05
| Thuộc tính | Chi tiết |
|------------|----------|
| **Objective** | Xây dựng lớp hạ tầng lưu trữ và tích hợp dịch vụ bên ngoài. |
| **Deliverables** | Cấu hình lưu trữ dữ liệu, hệ thống migration và hiện thực hóa các Repository. |
| **Allowed Scope** | Triển khai truy xuất CSDL, giao tiếp API bên ngoài, định nghĩa các bản đồ dữ liệu (Mapping). |
| **Out of Scope** | Không chứa logic nghiệp vụ thuần túy, không xử lý xác thực API trực tiếp. |
| **Acceptance Criteria** | Tuân thủ tuyệt đối quy tắc Dependency Inversion đối với tầng Domain. |

### Sprint 06
| Thuộc tính | Chi tiết |
|------------|----------|
| **Objective** | Mở cổng giao tiếp thông qua Web API. |
| **Deliverables** | Các endpoints giao tiếp, Swagger, và luồng bảo mật. |
| **Allowed Scope** | Định tuyến (Routing), Middleware, Authentication, Dependency Injection Configuration. |
| **Out of Scope** | Không xử lý Use Case logic, không truy cập thẳng Database. |
| **Acceptance Criteria** | Phơi bày đầy đủ các API hợp lệ, có bảo mật và tài liệu OpenAPI rõ ràng. |
