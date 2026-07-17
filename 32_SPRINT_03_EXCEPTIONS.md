# 32_SPRINT_03_EXCEPTIONS.md
## SPRINT 03 – DOMAIN EXCEPTIONS SPECIFICATION
Version: 1.1.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 32_SPRINT_03_EXCEPTIONS
- **Title:** Domain Exceptions Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Software Architect, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft |
| 1.1.0 | 2026-07-17 | Solution Architecture Team | Added Exception Lifecycle, Decision Matrix, Naming Convention, Folder Strategy, Dependency Rules, AI Review Matrix, Expanded Validation. |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Exception Architecture.
- KHÔNG ĐƯỢC thay đổi Exception Hierarchy Strategy.
- KHÔNG ĐƯỢC thay đổi Exception Propagation Strategy.
- Mọi thay đổi về đặc tả Exception bắt buộc phải thông qua một Revision mới của tài liệu và được phê duyệt lại.

---

# 4. Purpose
Tài liệu này là đặc tả kiến trúc (Architecture Specification) xác định chiến lược thiết kế và quản lý ngoại lệ (Exceptions) riêng biệt cho tầng Domain. Mục tiêu là định hình một hệ thống kiểm soát vi phạm nghiệp vụ (Business Rule Violations) thuần túy, loại bỏ hoàn toàn sự phụ thuộc vào hạ tầng, giao diện lập trình (API), hoặc cơ sở dữ liệu.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa kiến trúc cho Domain Exception.
- Phân loại và xây dựng phân cấp ngoại lệ (Exception Hierarchy).
- Xây dựng chiến lược bắt lỗi nhanh (Fail Fast).
- Quy định về ngôn ngữ và thông điệp ngoại lệ (Exception Message).
- Giới hạn ranh giới xử lý ngoại lệ (Exception Handling Boundary).

**Out of Scope (Ngoài phạm vi):**
- KHÔNG định nghĩa HTTP Status Codes (thuộc tầng API).
- KHÔNG định nghĩa định dạng Response Model hoặc Error Codes của API.
- KHÔNG mô tả việc ghi log (Logging) hay theo dõi lỗi (Stack Traces).
- KHÔNG sinh bất kỳ mã nguồn C#, XML, JSON hay UML nào.

---

# 6. Objectives
- Bảo vệ sự tinh khiết của tầng Domain: Không để các công nghệ ngoài xâm nhập vào logic nghiệp vụ.
- Đảm bảo tính "Fail Fast": Phát hiện và ngăn chặn trạng thái đối tượng không hợp lệ ngay từ lúc khởi tạo hoặc thay đổi.
- Thống nhất ngôn ngữ thiết kế: Ngoại lệ phải thể hiện ngôn ngữ chung (Ubiquitous Language) của miền nghiệp vụ.
- Đơn giản hóa việc bảo trì: Cho phép tầng Application và API dễ dàng nhận diện và chuyển đổi Domain Exception thành mã lỗi phù hợp cho người dùng cuối.

---

# 7. Exception Architecture
Kiến trúc Ngoại lệ của tầng Domain được thiết kế theo hướng trừu tượng, xoay quanh một Lớp cơ sở (Base Exception) chung nhất. Mọi ngoại lệ nghiệp vụ cụ thể sẽ kế thừa từ Lớp cơ sở này. Kiến trúc này hoàn toàn khép kín, nó độc lập với hệ thống báo lỗi của C# cơ bản, độc lập với thư viện bên thứ ba và chỉ được sinh ra bởi chính quyền miền (Domain Authority). Tầng Domain chỉ chịu trách nhiệm **Throw**, không chịu trách nhiệm **Catch** (trừ trường hợp kiểm soát luồng nội bộ cực kỳ hãn hữu).

---

# 8. Exception Creation Order
Trình tự sinh mã kiến trúc Ngoại lệ (chỉ áp dụng khi vào giai đoạn Coding):
- **Step 1:** Khởi tạo `DomainException` (Lớp trừu tượng cốt lõi).
- **Step 2:** Phân loại ngoại lệ cơ bản (Validation, NotFound, BusinessRuleViolation).
- **Step 3:** Khởi tạo các Lớp ngoại lệ theo từng Domain Module cụ thể.
- **Step 4:** Tích hợp kiểm tra ngoại lệ vào Base Classes (Entity, ValueObject).
- **Step 5:** Rà soát kiến trúc (Validation).
- **Step 6:** Đóng băng.
- **Step 7:** Coding.

---

# 9. Exception Evolution Strategy
Lộ trình phát triển hệ thống Ngoại lệ qua các Sprint:
- **Sprint 03:** Khởi tạo Base DomainException, định nghĩa luật chơi.
- **Sprint 04:** Thiết lập các ngoại lệ cụ thể theo nghiệp vụ của Bounded Context.
- **Sprint 05:** Ánh xạ ngoại lệ cơ sở dữ liệu (từ tầng Infrastructure) thành DomainException (hoặc InfrastructureException tương đương).
- **Sprint 06:** Thiết lập ExceptionFilter / Middleware tại tầng API để dịch DomainException thành mã HTTP phù hợp (Global Exception Handling).

---

# 10. Domain Exception Principles
- **No Infrastructure Dependency:** Không tham chiếu Entity Framework, Dapper, SQL.
- **No HTTP Knowledge:** Không dùng `404`, `400`, `StatusCode`. Không liên kết với `Microsoft.AspNetCore`.
- **No Logging:** Tầng Domain không ghi log lỗi. Việc log lỗi thuộc trách nhiệm của tầng Application/Infrastructure.
- **No Presentation Detail:** Không trả về DTO hay Response Model.
- **Explicit Intent:** Ngoại lệ phải thể hiện rõ quy tắc nghiệp vụ nào bị vi phạm.

---

# 11. Exception Lifecycle
Vòng đời của một ngoại lệ trong hệ thống:
1. **Business Rule Detection:** Phát hiện trạng thái hoặc thao tác không hợp lệ ngay tại phương thức nghiệp vụ của Entity.
2. **Throw DomainException:** Tầng Domain lập tức ném ngoại lệ (Fail Fast) chặn tiến trình.
3. **Bubble Up:** Ngoại lệ nảy lên qua các lớp call stack mà không bị Catch bởi Domain.
4. **Application Translation:** Tầng Application có thể Catch để Rollback UnitOfWork nếu cần, nhưng thường nhường cho API.
5. **API Mapping:** Tầng API (qua ExceptionFilter/Middleware) Catch ngoại lệ và Mapping sang ProblemDetails/HTTP Status tương ứng.
6. **Client Response:** Trả kết quả (Response) cho Client.
7. **Infrastructure Logging:** Tầng hạ tầng song song tiến hành ghi log lỗi vào hệ thống (Seq, Application Insights).

**Nhấn mạnh:**
- Domain chỉ **Throw**.
- Application **Translate**.
- API **Handle**.
- Infrastructure **Log**.

---

# 12. Exception Decision Matrix
Ma trận quyết định khi thiết kế hoặc ném ngoại lệ:

| Loại lỗi | Trách nhiệm xử lý | Kết quả |
|---|---|---|
| **Business Rule Violation** (VD: Rút tiền quá số dư) | Domain ném `DomainException` | Application hủy giao dịch, API trả về 400. |
| **State Transition Violation** (VD: Hủy hồ sơ đã hoàn thành) | Domain ném `DomainException` | Application hủy giao dịch, API trả về 409. |
| **Validation Failure** (VD: Định dạng email sai) | Domain ném `DomainException` | API trả về 400 Bad Request. |
| **Not Found** (Thực thể không tồn tại trong DB) | Application ném `NotFoundException` | API trả về 404 Not Found. |
| **Concurrency** (Lỗi xung đột dữ liệu) | Infrastructure ném `ConcurrencyException` | API trả về 409 Conflict hoặc yêu cầu retry. |
| **Infrastructure Failure** (VD: Mất kết nối DB) | Infrastructure ném Exception mặc định | API trả về 500 Internal Server Error. |

---

# 13. Exception Naming Convention
Quy tắc đặt tên cho Domain Exception:

**Allowed (Cho phép):**
- `CitizenAlreadyExistsException`
- `InvalidCitizenStatusException`
- `BenefitExpiredException`
- `HouseholdClosedException`
(Phản ánh chính xác đối tượng, trạng thái hoặc quy tắc bị vi phạm)

**Not Allowed (Tuyệt đối cấm):**
- `BusinessException1` (Quá chung chung, không có ý nghĩa)
- `DataError` (Lẫn lộn với tầng Data/Infrastructure)
- `CitizenError` (Không nêu rõ nguyên nhân)
- `InvalidData` (Thiếu ngữ cảnh)
- `Exception123` (Vi phạm Clean Code)

**Lý do:** Tên của ngoại lệ chính là một phần của Ubiquitous Language, nó phải truyền đạt được ý định nghiệp vụ ngay từ tên lớp mà chưa cần đọc nội dung Exception Message.

---

# 14. Exception Folder Strategy
Chiến lược tổ chức thư mục ngoại lệ trong dự án:
- `Exceptions/`
  - `Base/` (Chứa `DomainException`)
  - `Validation/` (Chứa các lỗi chung về ValueObject)
  - `Business/` (Chứa các quy tắc nghiệp vụ cốt lõi)
  - `State/` (Chứa lỗi vòng đời trạng thái)
  - `Modules/`
    - `Demographic/` (Lỗi nghiệp vụ module Dân cư)
    - `SocialSecurity/` (Lỗi nghiệp vụ module An sinh xã hội)
    - `Disbursement/` (Lỗi nghiệp vụ module Giải ngân)
    - `Communication/`
    - `Analytics/`

*(Lưu ý: Chỉ thiết lập quy tắc, không sinh code tại tài liệu này).*

---

# 15. Dependency Rules
Ràng buộc phụ thuộc cứng cho việc khai báo Exception:

**Allowed Dependencies:**
- `System.*` (Các thư viện core của C#)

**Forbidden Dependencies (CẤM tuyệt đối):**
- `EntityFrameworkCore`
- `Dapper`
- `MediatR`
- `ASP.NET`
- `Newtonsoft.Json`
- `Logging Framework` (Serilog, NLog)
- `SignalR`
- `Redis`
- `Database Provider` (Npgsql, SqlClient)
- `HTTP Library` (HttpClient, Flurl)

**Nhấn mạnh:** 100% Framework Ignorance.

---

# 16. Exception Classification Strategy
Phân loại ngoại lệ trong Domain thành các nhóm chính:
1. **Business Rule Violations:** Xảy ra khi một hành động vi phạm quy định nghiệp vụ cốt lõi.
2. **Domain Validation Errors:** Xảy ra khi dữ liệu khởi tạo không hợp lệ tại mức Value Object.
3. **State Transition Errors:** Xảy ra khi đối tượng chuyển đổi trạng thái không hợp lệ theo máy trạng thái.
4. **Not Found Constraints:** Khi một Aggregate con không tồn tại trong một Aggregate Root.

---

# 17. Exception Hierarchy Strategy
Cấu trúc phân cấp ngoại lệ:
- Khởi điểm là Lớp `DomainException` (Kế thừa từ `System.Exception` mặc định nhưng bị cô lập logic).
- Lớp này phải là `abstract`.
- Mọi ngoại lệ cụ thể phải kế thừa từ `DomainException`.

---

# 18. Business Rule Violation Strategy
- Áp dụng nguyên tắc **Fail Fast**: Ngay khi phát hiện vi phạm nghiệp vụ, ngoại lệ phải được ném ra ngay lập tức, chặn đứng tiến trình.
- Không sử dụng Return Codes để xử lý luồng nghiệp vụ lỗi trong Domain Model.

---

# 19. Exception Message Guidelines
- **Business Language (Ngôn ngữ nghiệp vụ):** Thông điệp phải phản ánh ngữ cảnh nghiệp vụ.
- **Không chứa dữ liệu nhạy cảm:** Không in mật khẩu, khóa bí mật, PII trực tiếp vào Exception.
- **Đủ chi tiết cho nhà phát triển:** Cung cấp đủ ngữ cảnh để truy vết lỗi.
- **Không định dạng UI:** Không dùng HTML hay ký tự hiển thị.

---

# 20. Exception Propagation Strategy
- **Ném (Throw):** Chỉ ném ngoại lệ khi có sự cố vi phạm.
- **Lan truyền (Propagate):** Ngoại lệ từ Domain Model sẽ nổi lên trên, thoát ra khỏi tầng Domain.
- **Không ngăn chặn nội bộ (No Swallow):** Không được sử dụng try-catch bên trong Domain Model để "nuốt" lỗi.

---

# 21. Exception Handling Boundary
Quy định ranh giới xử lý:
- **Tầng Domain:** Chuyên môn là tạo (Create) và ném (Throw) ngoại lệ. Không bao giờ Catch.
- **Tầng Application:** Có thể Catch để Rollback UnitOfWork, nhưng không chuyển đổi thành HTTP.
- **Tầng Presentation / API:** Là chốt chặn cuối cùng nơi Catch `DomainException` và dịch nó thành cấu trúc lỗi phù hợp (`ProblemDetails`, v.v.).

---

# 22. Expanded Validation Rules
Kiến trúc Exception được xem là hợp lệ khi vượt qua tất cả các chốt kiểm tra sau:
- **No HTTP:** Không chứa bất kỳ Status Code nào (404, 400).
- **No EF:** Không rò rỉ ngoại lệ `DbUpdateException`.
- **No Database:** Không liên kết với cấu trúc bảng hoặc SQL.
- **No Logging:** Không gọi `ILogger` bên trong Exception.
- **No API:** Không chứa `ProblemDetails` hoặc `ResponseModel`.
- **No Infrastructure:** Không phụ thuộc I/O.
- **No Serialization:** Không gắn các attributes như `[JsonIgnore]`.
- **No ORM:** Không dùng thuộc tính của Dapper hay EF.
- **No MediatR:** Không trộn lẫn Exception với `IRequest` hay `INotification`.
- **Fail Fast:** Bắt lỗi ngay tại cửa ngõ của Constructor/Method.
- **Business Language:** Tên và Message phản ánh nghiệp vụ thực tế.
- **Throw Only:** Chỉ Throw, cấm Catch nội bộ trong Domain.

---

# 23. AI Coding Constraints
- **CẤM** sinh mã nguồn C# dưới bất kỳ hình thức nào.
- **CẤM** sinh Pseudo Code để minh họa cách Throw hoặc Catch.
- **CẤM** tham chiếu `Microsoft.AspNetCore.Http` hoặc bất kỳ Namespace nào ngoài `System`.
- **CẤM** sử dụng các cấu trúc dữ liệu JSON.
- **CẤM** thiết kế ErrorCode tĩnh phục vụ API.

---

# 24. Definition of Done
Tài liệu đạt tiêu chuẩn hoàn thành khi:
- Bao quát mọi khía cạnh vòng đời của Domain Exception (từ Raise đến Clear).
- Tích hợp chi tiết Folder Strategy, Naming Convention, và Decision Matrix.
- Phân định ranh giới trách nhiệm rõ ràng với Application, API, và Infrastructure.
- Duy trì 100% Persistence Ignorance và Framework Ignorance.
- Sẵn sàng đóng vai trò tiền đề vững chắc cho việc thiết lập Domain Events (tài liệu 33).

---

# 25. AI Review Matrix
Bảng ma trận tự đánh giá của AI:

| Tiêu chí rà soát | Mô tả kiểm tra |
|---|---|
| **Architecture Boundary** | Tầng Domain chỉ Throw, không Catch, không Handle HTTP. |
| **Persistence Ignorance** | Hoàn toàn tách biệt khỏi DB, EF Core, SQL. |
| **Framework Independence** | Không dính líu đến ASP.NET, MediatR, Logging Framework. |
| **Business Language** | Tên Exception và Message tuân thủ Ubiquitous Language. |
| **Exception Hierarchy** | Kế thừa chặt chẽ từ `DomainException` abstract. |
| **Fail Fast** | Áp dụng Guard Clauses, văng lỗi ngay lập tức. |
| **No Catch** | Tuyệt đối không dùng try-catch bên trong Entity/VO. |
| **Compile Ready** | Sẵn sàng để triển khai thành C# ở Sprint tiếp theo mà không sinh lỗi dependencies. |

---

# 26. Risks
- **Exception được dùng làm Control Flow:** Một số luồng code lạm dụng ném ngoại lệ thay cho luồng if-else thông thường (ví dụ: dùng ngoại lệ để check trùng lặp khi chưa cần thiết).
  - *Hướng giảm thiểu:* Chỉ ném ngoại lệ khi có vi phạm nghiệp vụ (Business Rule Violation) hoặc Invariant bị phá vỡ.
- **Lạm dụng Generic Exception:** Ném ra `System.Exception` hoặc `DomainException` trực tiếp thay vì các ngoại lệ cụ thể (như `CitizenNotFoundException`).
  - *Hướng giảm thiểu:* Bắt buộc `DomainException` phải là abstract, chỉ cho khởi tạo các lớp kế thừa cụ thể.
- **Exception chứa thông tin nhạy cảm:** Developer đưa cả ConnectionString hoặc PII (như CCCD, Password) vào Exception Message.
  - *Hướng giảm thiểu:* Review nghiêm ngặt mã nguồn, chỉ đưa thông tin mô tả ngữ cảnh vi phạm, ẩn các thông tin định danh cá nhân.
- **Exception phụ thuộc API:** Developer lén lút gài mã HTTP 400 vào trong Constructor của Exception để API xử lý cho nhanh.
  - *Hướng giảm thiểu:* Application/API Layer phải dùng Exception Filter/Middleware để ánh xạ Type của Exception sang HTTP Status, tuyệt đối cấm mang Status vào Domain.

---

# 27. AI Review Checklist
- [x] Đã hoàn toàn tuân thủ không sinh C#, Pseudo Code, XML, UML?
- [x] Đã cập nhật Version History lên 1.1.0?
- [x] Đã bổ sung Exception Lifecycle, Decision Matrix, Naming Convention?
- [x] Đã thiết lập Exception Folder Strategy và Dependency Rules?
- [x] Đã mở rộng Validation Rules với 12 tiêu chí nghiêm ngặt?
- [x] Đã bổ sung AI Review Matrix và các rủi ro cụ thể?
- [x] Đã duy trì 100% Persistence Ignorance và Framework Ignorance?
- [x] Tài liệu đã tương thích hoàn toàn với các tài liệu 25 đến 31 và làm tiền đề cho tài liệu 33?

---

# 28. References
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
- 28_SPRINT_02_REVIEW.md
- 29_SPRINT_03_IMPLEMENTATION_PLAN.md
- 30_SPRINT_03_PROJECT_INIT.md
- 31_SPRINT_03_BASE_CLASSES.md

**Lưu ý:** Document này là tiền đề vững chắc chuẩn bị cho việc đặc tả sự kiện miền ở tài liệu tiếp theo: **33_SPRINT_03_EVENTS.md**

---
# END OF SPECIFICATION
