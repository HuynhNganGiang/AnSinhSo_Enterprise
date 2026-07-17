# 43_SPRINT_03_DOMAIN_NOTIFICATIONS.md
## SPRINT 03 – DOMAIN NOTIFICATIONS ARCHITECTURE
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 43_SPRINT_03_DOMAIN_NOTIFICATIONS
- **Title:** Domain Notifications Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Notifications |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi cấu trúc cốt lõi của Domain Notification Pattern.
- **KHÔNG ĐƯỢC** nhầm lẫn Domain Notification với các khái niệm Email, SMS, Push Notification, hay Infrastructure Message Queue.
- **KHÔNG ĐƯỢC** sử dụng Domain Notification để văng Exception kiểm soát luồng.
- Mọi thay đổi về triết lý Notification bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là Đặc tả Kiến trúc (Architecture Specification) chuyên sâu định nghĩa "Domain Notification Pattern" trong hệ sinh thái DDD của dự án AnSinhSo. Mục tiêu lõi là chuẩn hóa một cơ chế thu thập (Collect) mọi lỗi và cảnh báo nghiệp vụ trong suốt quá trình Validation và Business Execution, trước khi đóng gói toàn bộ chúng vào Result Pattern trả về cho Application Layer.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất của Domain Notification.
- Phân tách Notification với Exception, Domain Event, Logging, UI Message.
- Phân tích chi tiết Vòng đời (Lifecycle) và Chiến lược thu thập (Collection Strategy).
- Phân loại (Categories), Mức độ nghiêm trọng (Severity) và Quyền sở hữu (Ownership).
- Liệt kê và phân tích 15 Anti-Patterns kiến trúc phổ biến nhất.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C# (như `class Notification`, `interface INotificationContext`).
- KHÔNG sinh mã triển khai các thư viện như MediatR `INotification`.
- KHÔNG mô tả cách gửi Email, Zalo hay SMS.
- KHÔNG sinh cấu hình JSON, XML, hay SQL.

---

# 6. Objectives
- **Collect Instead of Throw:** Thay đổi tư duy từ "Thấy lỗi đầu tiên là quăng Exception" sang "Gom toàn bộ lỗi vào Notification và trả về một lần".
- **Decoupled Error Transportation:** Đóng vai trò là "chuyến xe" chuyên chở các lỗi từ sâu bên trong Domain (Validator, Policy) ra đến mép ngoài cùng (Result Pattern).
- **Pure Domain Semantics:** Giữ cho Notification hoàn toàn không dính líu đến công nghệ hạ tầng.
- **Enhanced User Experience:** Cung cấp cho Application Layer một danh sách đầy đủ tất cả những gì người dùng làm sai, giúp Frontend hiển thị tất cả các lỗi trong một lần submit (thay vì bắt người dùng sửa từng lỗi một).

---

# 7. Domain Notification Architecture
**Bản chất của Domain Notification:**
- Là một mẫu thiết kế (Pattern) nằm gọn trong tầng Domain, dùng để làm một chiếc "Giỏ chứa" (Container) thu thập kết quả đánh giá nghiệp vụ (Thường là chứa các `Error` từ `ErrorCatalog`).
- **Khác Exception:** Notification KHÔNG cắt đứt call-stack (luồng thực thi). Exception bẻ gãy call-stack ngay lập tức.
- **Khác Domain Event:** Domain Event mô tả "Một thứ nghiệp vụ quan trọng ĐÃ XẢY RA và được phép lưu trữ/công bố". Notification mô tả "Một hoặc nhiều rào cản ngăn không cho nghiệp vụ xảy ra".
- **Khác Logging:** Logging là việc ghi chép kỹ thuật (Technical Tracing) xuống file/console cho SysAdmin. Notification là dữ liệu sống trên RAM phục vụ luồng điều hướng nghiệp vụ.
- **Khác Infrastructure Message:** Notification không chạy qua RabbitMQ hay Kafka. Nó di chuyển qua các tham số hàm (Method Parameters) hoặc Scoped DI trên RAM.
- **Khác UI Message:** Notification không chứa mã HTML, không chứa câu văn tiếng Việt có định dạng hiển thị. Nó chỉ chứa Error Code và Metadata thuần túy.

---

# 8. Notification Lifecycle
Vòng đời bắt buộc của một Domain Notification diễn ra hoàn toàn trong một Request:
1. **Create:** Một Notification Container tĩnh hoặc scoped được khởi tạo.
2. **Collect:** Các Validator hoặc Domain Policy đánh giá trạng thái và "đẩy" (push) các `Error` (lấy từ Error Catalog) vào Notification Container.
3. **Aggregate:** Container tổng hợp, loại bỏ các lỗi trùng lặp (nếu có).
4. **Transfer:** Trạng thái Notification được chuyển giao từ tầng sâu (Entities) ra mép (Domain Service).
5. **Result:** Domain Service đóng gói Notification này vào bên trong `ValidationResult` hoặc `FailureResult`.
6. **Application Layer:** Tầng Application nhận Result, đọc Notification, rẽ nhánh tương ứng (không gọi SaveChanges nếu có lỗi).
7. **Dispose:** Vòng đời kết thúc, Garbage Collector dọn dẹp Notification khỏi bộ nhớ. Hoàn toàn không lưu xuống ổ cứng.

---

# 9. Notification Collection Strategy
Kiến trúc AnSinhSo Enterprise hỗ trợ hai chiến lược, ưu tiên Collect All:
- **Fail Fast (Từ chối sớm):** Dừng ngay khi gặp lỗi đầu tiên. Phù hợp với các lỗi mang tính chất tiên quyết (Prerequisite) như `CitizenNotFound`. Nếu không có Citizen, không cần tính toán các bước tiếp theo.
- **Collect All Errors (Thu thập toàn bộ - ƯU TIÊN DOANH NGHIỆP):** Chạy qua toàn bộ các rule có thể chạy song song (đặc biệt trong Validator). Ví dụ: Kiểm tra một hồ sơ có 20 trường thông tin. Nếu sai 5 trường, Notification phải thu thập đủ 5 lỗi `Error` rồi mới trả về. Điều này tiết kiệm vô số vòng lặp Request/Response giữa UI và Backend.

---

# 10. Notification Categories & Severity
**Categories (Phân loại):**
- **Validation:** Sai định dạng, trống trường bắt buộc.
- **Business:** Vi phạm nguyên tắc miền.
- **Permission / Authorization:** Truy cập vào miền ngoài thẩm quyền xử lý.
- **Policy:** Không thỏa mãn chính sách tính toán (VD: Policy trả về "Không đủ năm đóng BHXH").
- **Conflict:** Xung đột phiên bản dữ liệu Aggregate.
- **Warning:** Các vấn đề không làm sụp đổ tác vụ nhưng cần chú ý.
- **Information:** Thông tin luồng chạy (hiếm dùng, chủ yếu phục vụ Audit Log trong bộ nhớ).

**Severity (Mức độ nghiêm trọng):**
- **Info:** Dữ liệu thông thường.
- **Warning:** Đánh dấu cảnh báo, Result có thể vẫn `Success` kèm Warning Notification.
- **Failure:** Gây ra `FailureResult` chặn luồng.
- **Critical:** Cảnh báo đỏ, kích hoạt luồng xử lý giám sát ngay tại tầng Application.

---

# 11. Notification Ownership
Trách nhiệm thao tác với Notification:
- **Validator:** Chủ sở hữu ghi (Write-Owner). Đẩy phần lớn lỗi vào Notification.
- **Policy / Domain Service:** Bổ sung lỗi nghiệp vụ sâu vào Notification.
- **Aggregate Root / Value Object:** Có thể trả về lỗi để Factory/Domain Service nạp vào Notification.
- **Result Pattern:** Trở thành "Vỏ bọc" cuối cùng (Wrapper) niêm phong Notification.
- **Application Layer:** Chủ sở hữu đọc (Read-Owner). Chuyển hóa Notification thành HTTP Response hoặc DTO cho Presentation.

---

# 12. Relationship With Aggregate & Value Objects
- Khi Factory hoặc Constructor của Aggregate được khởi chạy thông qua Validator, Notification đóng vai trò là "cái túi" nhặt các hạt sạn (Lỗi Value Object, Lỗi Invariants). Nếu cái túi không rỗng, Aggregate từ chối hoàn tất quá trình khởi tạo (trả về Result lỗi).

---

# 13. Relationship With Domain Services & Policies
- Domain Service hoạt động như một nhạc trưởng, có thể tiêm (inject/pass) Notification context qua các Policy khác nhau để thu gom toàn bộ kết quả chẩn đoán trước khi quyết định bước tiếp theo.

---

# 14. Relationship With Validators & Specifications
- **Validator:** Khách hàng lớn nhất của Notification. Validator không văng Exception mà nhét lỗi vào Notification.
- **Specification:** Nếu Specification `IsSatisfiedBy` trả về false, Domain Service sẽ lấy lý do (Reason) từ Specification và nạp vào Notification.

---

# 15. Relationship With Result Pattern & Error Catalog
- **Bộ ba kiến trúc:** `ErrorCatalog` cung cấp từ vựng (Vocabulary). `Notification` gom các từ vựng đó lại thành một bức thư (Letter). `Result Pattern` là chiếc phong bì (Envelope) gửi bức thư đó ra khỏi tầng Domain.

---

# 16. Relationship With Exceptions & Domain Events
- **Exceptions:** Notification sinh ra để TIÊU DIỆT việc sử dụng Exception trong logic miền.
- **Domain Events:** Notification xử lý các lỗi XẢY RA TRƯỚC khi trạng thái thay đổi. Domain Event thông báo sự thành công XẢY RA SAU khi trạng thái đã đổi.

---

# 17. Relationship With Factories & Repositories
- **Factory:** Trả về Notification chứa các lỗi ngăn cản quá trình tạo đối tượng.
- **Repository:** Repository không dùng Notification. Nếu lỗi dữ liệu, Repository ném Infrastructure Exception. Nếu không tìm thấy dữ liệu, trả về Result mang theo Error từ Error Catalog.

---

# 18. Relationship With Application Layer (CQRS & MediatR)
- **CQRS/MediatR Pipeline:** Notification được tích hợp cực kỳ mạnh mẽ tại `IPipelineBehavior` của MediatR. Pipeline chặn Request lại, chạy Validator, thu thập lỗi vào Notification. Nếu có lỗi, Pipeline lập tức trả về `Result` mang theo Notification, không cho Request chạm tới Domain Handler.

---

# 19. Naming Convention
- Tên class thiết kế mẫu (Nếu có): `DomainNotification`, `ValidationNotification`.
- Hành vi: `AddError`, `HasErrors`, `Collect`.
- KHÔNG gọi là `ExceptionBag` hay `ErrorLog`.

---

# 20. Folder Strategy
Cấu trúc tổ chức thư mục tại tầng Domain:
- `SeedWork/`
  - `Notifications/` (Chứa các khái niệm cốt lõi của Notification Pattern).
- (Không chia theo Module vì Notification là một Design Pattern nền tảng, không mang tính module nghiệp vụ cụ thể).

*(Lưu ý: Chỉ thiết lập định hướng thư mục, không sinh source code tại đây).*

---

# 21. Dependency Rules
Ràng buộc phụ thuộc cứng cho Notification:

**Allowed Dependencies:**
- Tầng Domain (`ErrorCatalog`, `ResultPattern`, `System.*`).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `Repositories` / `DbContext` / `EF Core`.
- `Microsoft.AspNetCore.Mvc` (Đặc biệt là `ModelStateDictionary`).
- Giao thức gửi tin: `SMTP`, `Twilio`, `Firebase Push Notification` (Đây là việc của Infrastructure).
- Các hệ thống Message Queue như `RabbitMQ`, `Kafka`, `Azure Service Bus`.

---

# 22. Validation Rules
Một Domain Notification hợp lệ khi:
- Chỉ lưu trữ các cấu trúc `Error` thuần túy thuộc Domain.
- Không chứa tham chiếu vòng (Circular Reference) ngược lại các Aggregate.
- Luôn cung cấp trạng thái `IsSuccess` (nếu không có Error nào) hoặc `HasErrors`.

---

# 23. Performance Strategy
- **Zero Allocation (Tối ưu):** Notification nội bộ có thể được lưu trữ bằng mảng ArrayPool hoặc List với capacity cấp trước, tránh phân bổ lại RAM liên tục khi số lượng lỗi lớn.
- **Immutable Notification:** Cấu trúc tốt nhất là khi đóng gói vào Result, Notification trở thành Read-Only (IReadOnlyCollection) để đảm bảo không ai chọc vào sửa đổi sau khi đã có phán quyết.
- **No Duplicate Notification:** Cần dùng `HashSet` để lọc các mã lỗi trùng lặp sinh ra từ các Validator trùng lặp.

---

# 24. AI Coding Constraints
Giới hạn hành vi bắt buộc của AI Coding Agent:
- KHÔNG tạo bất kỳ file C# nào triển khai (`class`, `record`) cho Notification Pattern tại đây.
- KHÔNG sinh mã JSON, XML, UML, hay Pseudo Code.
- CẤM trộn lẫn khái niệm "Email/SMS" khi xử lý code liên quan đến Notification trong Domain Layer. Nhắc lại: Đây là "Domain Error Notification Pattern".
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 25. Architectural Risks
- **Notification Explosion:** Quá nhiều luật kiểm tra vụn vặt được đẩy vào Notification gây ra một payload khổng lồ trả về cho người dùng (ví dụ: 100 lỗi chính tả).
- **Duplicate Notification:** Không lọc lỗi trùng khiến Notification lặp lại cùng một mã lỗi nhiều lần.
- **Notification Memory Leak:** Cài đặt Notification dưới dạng Singleton dùng chung cho nhiều Request gây rò rỉ bộ nhớ nghiêm trọng và sai lệch nghiệp vụ giữa các user.
- **Shared Notification Context:** Chia sẻ một đối tượng Notification cho nhiều tiến trình bất đồng bộ mà không có Thread-Safety khóa (lock).
- **Notification Hidden State:** Thiết kế Notification phức tạp đến mức nó tự ẩn giấu các trạng thái mà tầng Application không truy xuất được.

---

# 26. Architectural Anti Patterns
15 mẫu phản kiến trúc (Anti-Patterns) tuyệt đối cấm:

1. **Notification Throwing Exception:** Notification tự động ném Exception khi add lỗi thứ 5. Chức năng của nó là gom lỗi, không phải văng lỗi.
2. **Notification Calling Repository:** Cấu trúc Notification chứa `IRepository` để tự tra cứu dữ liệu. Lỗi xâm phạm nghiêm trọng.
3. **Notification Publishing Domain Event:** Khi thêm một lỗi, Notification tự Publish event `ErrorAddedEvent`. Lạm dụng Event quá đà.
4. **Notification Holding HTTP Status:** Notification tự gán `StatusCode = 400` cho chính nó. Vi phạm nguyên lý cách ly miền.
5. **Notification Holding Localization:** Notification tự gọi `Resource.vi_VN` để dịch chuỗi lỗi. Vi phạm nghiêm trọng UI separation.
6. **Notification Logging Directly:** Cấu trúc Notification nhận `ILogger` và tự `Log.Error` mỗi khi có lỗi được đẩy vào.
7. **Notification Saving Database:** Notification gọi lệnh lưu toàn bộ các lỗi vào bảng `ErrorLogs` của DB.
8. **Notification Calling API:** Notification gọi webhook ra ngoài để báo cáo lỗi.
9. **Notification Mutating Aggregate:** Notification không chỉ gom lỗi mà còn gọi lại `citizen.ClearInvalidData()` để sửa Aggregate. Notification chỉ là túi chứa, không phải dao mổ.
10. **Notification Shared Singleton State:** Cấu hình IoC cho Notification là `AddSingleton`, dẫn đến lỗi của User A văng sang màn hình của User B.
11. **Notification Without Error Catalog:** Notification chứa các biến String kiểu `"Lỗi nhập sai"` thay vì mã tham chiếu `"ERR-001"` từ Error Catalog.
12. **Notification Without Result Pattern:** Hàm của Domain Service trả về `Notification` nhưng không bọc trong `Result`, gây bối rối cho Application Layer.
13. **Notification Used As Message Queue:** Hiểu lầm từ "Notification" và đem nó đi cấu hình vào Kafka Producer.
14. **Notification Mixed With Infrastructure:** Cấu trúc Notification bắt các lỗi mất kết nối DB.
15. **Notification Used As UI Model:** Application Layer truyền thẳng cấu trúc nội tại của Notification xuống View Razor hoặc React Frontend mà không map qua Error Response chuẩn.

---

# 27. Definition Of Done
Tài liệu hoàn tất khi:
- Khái niệm "Domain Notification" được vạch rõ ranh giới với các khái niệm "Infrastructure Notification" (Email/SMS).
- Luồng hoạt động cùng Validator và Result Pattern được mô tả sắc nét.
- Toàn bộ 15 Anti-Patterns được chỉ mặt đặt tên và nghiêm cấm.
- Đạt chuẩn Enterprise Documentation Suite không tì vết.

---

# 28. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **No UI Separation Leak** | Cấm dịch ngôn ngữ (I18N) bên trong Notification. | Bắt buộc (Mandatory) |
| **No Transport Infra** | Cấm gửi Email, SMS, Kafka từ Domain Notification. | Bắt buộc (Mandatory) |
| **Stateless Flow** | Dòng đời Notification sống và chết theo Request. | Bắt buộc (Mandatory) |
| **No Exceptions Flow** | Cấm Notification tự ý ném ngoại lệ hệ thống. | Bắt buộc (Mandatory) |
| **Error Catalog Strictly** | Chỉ chấp nhận chứa phần tử lấy từ Error Catalog. | Bắt buộc (Mandatory) |
| **Result Pattern Binding**| Phải nằm trong Result Pattern khi rời Domain. | Bắt buộc (Mandatory) |

---

# 29. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã giải thích rõ Domain Notification KHÔNG PHẢI Email, SMS, Message Queue?
- [x] Đã phân tích đủ 15 Anti-Patterns kiến trúc?
- [x] Đã vạch rõ vòng đời Lifecycle và phương pháp Collect All Errors?
- [x] Đã chỉ rõ ranh giới với Aggregate, Result, Validator?
- [x] Đã tuân thủ chuẩn format khắt khe của Enterprise Documentation Suite? (CÓ).

---

# 30. References
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
- 40_SPRINT_03_DOMAIN_VALIDATORS.md
- 41_SPRINT_03_DOMAIN_RESULT_PATTERN.md
- 42_SPRINT_03_DOMAIN_ERROR_CATALOG.md

---
# END OF SPECIFICATION
