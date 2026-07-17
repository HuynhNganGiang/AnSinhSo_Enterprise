# 42_SPRINT_03_DOMAIN_ERROR_CATALOG.md
## SPRINT 03 – DOMAIN ERROR CATALOG ARCHITECTURE
Version: 1.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 42_SPRINT_03_DOMAIN_ERROR_CATALOG
- **Title:** Domain Error Catalog Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Error Catalog |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Domain Error Architecture.
- **KHÔNG ĐƯỢC** vi phạm nguyên tắc "Error thuộc về Domain, không được trộn lẫn HTTP Status hay Infrastructure Details".
- **KHÔNG ĐƯỢC** sử dụng Magic Strings để biểu diễn lỗi. Toàn bộ lỗi phải được định danh qua Catalog.
- Mọi sự thay đổi về cấu trúc phân loại Error bắt buộc phải thông qua một Revision mới và được hội đồng kiến trúc phê duyệt.

---

# 4. Purpose
Tài liệu này là Đặc tả Kiến trúc (Architecture Specification) xác định nền tảng thiết kế Domain Error Catalog. Mục đích cốt lõi là tạo ra một trung tâm đăng ký (Registry) và phân loại lỗi nhất quán cho toàn bộ hệ thống, giúp Result Pattern có một "từ điển" (Dictionary/Catalog) chuẩn mực để giao tiếp các sự cố nghiệp vụ mà không cần lặp lại hoặc hard-code các thông báo lỗi.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa bản chất của Domain Error và sự khác biệt với các loại lỗi khác.
- Thiết lập Error Taxonomy, Hierarchy, Ownership, và Severity.
- Quy định cấu trúc của Error Catalog, Namespace, Code Convention.
- Phân tích chi tiết 17 loại Error Categories (Validation, Conflict, Authorization,...).
- Xác định các Anti-Patterns và rủi ro kiến trúc.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C# (như `class Error`, `enum ErrorType`, `record DomainError`).
- KHÔNG sinh cấu hình JSON, XML hay cơ sở dữ liệu lưu lỗi.
- KHÔNG sinh mã xử lý đa ngôn ngữ (Resource files).
- KHÔNG giải thích chi tiết middleware bắt lỗi ở tầng API.

---

# 6. Objectives
- **Standardization (Chuẩn hóa):** Đảm bảo mọi lỗi phát sinh từ Domain đều tuân theo cùng một cấu trúc (Mã lỗi, Phân loại, Thông điệp).
- **Decoupling (Giảm phụ thuộc):** Tách biệt việc định nghĩa lỗi khỏi việc xử lý hiển thị lỗi (UI/Localization).
- **Observability (Khả năng quan sát):** Cung cấp Meta-Data chính xác để hệ thống Telemetry (Log/Monitor) dễ dàng phân tích xu hướng lỗi nghiệp vụ.
- **Developer Experience (Trải nghiệm Lập trình):** Tránh việc Developer phải tự "bịa" ra câu thông báo lỗi mỗi khi code, thay vào đó chỉ việc truy xuất từ Error Catalog.

---

# 7. Domain Error Architecture
**Bản chất của Domain Error:**
- Là một khái niệm phản ánh sự từ chối của một quy tắc nghiệp vụ (Business Rule) hoặc trạng thái không hợp lệ của Domain Model.
- **Error vs Exception:**
  - `Error` là một kết quả trả về mang tính *dự báo được* (Expected) từ Result Pattern. Hệ thống vẫn an toàn.
  - `Exception` là hiện tượng *đứt gãy hệ thống* (Unexpected).
- **Error vs Validation:** Validation là hành động kiểm tra. Error là *hậu quả/kết quả* của việc Validation thất bại.
- **Error vs Notification:** Notification dùng để thông báo cho hệ thống khác. Error dùng để chặn luồng thực thi hiện tại.
- **Error vs Infrastructure Error:** Domain Error nói rằng "Công dân không đủ tuổi" (Nghiệp vụ). Infrastructure Error nói rằng "Mất kết nối Redis" (Hạ tầng).
- **Error vs HTTP Status:** Domain Error không biết mã 400, 404 hay 500 là gì. Trách nhiệm ánh xạ từ Domain Error sang HTTP Status là của tầng Application/Presentation.

---

# 8. Error Taxonomy & Hierarchy
- **Error Taxonomy (Phân loại học):** Hệ thống phân lớp lỗi từ chung nhất (Global) đến chi tiết nhất (Module/Aggregate specific).
- **Error Hierarchy (Cấp bậc):** Không sử dụng kế thừa class (Inheritance) để định nghĩa Error vì nó tạo ra cây phân cấp cồng kềnh. Error nên được thiết kế dưới dạng cấu trúc dữ liệu phẳng (Flat Data Structure) mang tính chất định danh (Code, Type, Message).
- **Error Namespace:** Các lỗi phải được gom nhóm theo Bounded Context hoặc Aggregate. Ví dụ: `Citizen.InvalidAge`, `Pension.AlreadyClaimed`.

---

# 9. Error Categories
Hệ thống Domain Error được phân loại khắt khe vào các Category sau:
- **Validation Errors:** Dữ liệu đầu vào sai cấu trúc/định dạng (VD: `Email.Empty`).
- **Business Rule Errors:** Vi phạm quy tắc lõi (VD: `Policy.NotEligible`).
- **Conflict Errors:** Xung đột trạng thái (VD: `Citizen.DuplicateId`).
- **Authorization Errors:** Không nhận diện được thực thể nghiệp vụ (VD: `Identity.Unknown`).
- **Permission Errors:** Có nhận diện nhưng không đủ quyền thực thi hành động trong miền (VD: `Disbursement.ApprovalDenied`).
- **State Errors:** Trạng thái Aggregate không hợp lệ cho hành động (VD: `Policy.AlreadyActivated`).
- **Aggregate Errors:** Lỗi tổng hợp cấu trúc của Aggregate (VD: `Citizen.MissingRequiredAddress`).
- **Repository Errors (Domain Meaning):** Lỗi tìm kiếm mang ý nghĩa miền (VD: `Citizen.NotFound`).
- **Identity Errors:** Lỗi sinh ID hoặc đối chiếu ID (VD: `Id.InvalidFormat`).
- **Policy Errors:** Lỗi trả về từ Domain Policy (VD: `Tax.CalculationFailed`).
- **Specification Errors:** Lỗi khi không thỏa mãn Specification (VD: `Criteria.NotMatched`).
- **Factory Errors:** Lỗi khi khởi tạo Aggregate (VD: `Factory.AssemblyFailed`).
- **Domain Service Errors:** Lỗi khi điều phối nhiều Aggregate (VD: `Service.ProcessAborted`).
- **Event Errors:** Lỗi cấu trúc Event (VD: `Event.PayloadInvalid`).
- **Concurrency Errors:** Lỗi cạnh tranh dữ liệu được Domain nhận diện (VD: `Aggregate.VersionMismatch`).
- **Version Errors:** Lỗi không tương thích phiên bản nghiệp vụ (VD: `Policy.DeprecatedVersion`).
- **Operation Errors:** Các lỗi thực thi nghiệp vụ chung không rơi vào các nhóm trên.

---

# 10. Error Ownership & Lifecycle
- **Error Ownership (Quyền sở hữu):** Lỗi thuộc về nơi định nghĩa quy tắc. Aggregate Root sở hữu lỗi của chính nó. Value Object sở hữu lỗi định dạng của nó. Error Catalog đóng vai trò là nơi tập hợp, không tước đi quyền sở hữu của Domain.
- **Error Lifecycle (Vòng đời):**
  1. Định nghĩa tại Catalog.
  2. Khởi tạo/Trả về thông qua Result Pattern.
  3. Truyền tải qua Application Layer.
  4. Ánh xạ thành HTTP Response hoặc Logged tại Presentation Layer. Xóa sổ khỏi bộ nhớ.

---

# 11. Error Severity & Metadata
- **Severity (Mức độ nghiêm trọng):** Error có thể mang các cờ (flag) mô tả mức độ:
  - `Warning`: Không chặn luồng chính, nhưng cần cảnh báo.
  - `Failure`: Chặn luồng chính.
  - `Critical`: Lỗi nghiệp vụ cực kỳ nghiêm trọng, cần kích hoạt cảnh báo hệ thống (Alert).
- **Metadata:** Error có thể chứa Dictionary các thông tin phụ (như `MinLength=5`, `CurrentValue=3`) để Application Layer biết cách hiển thị chi tiết mà không cần parse chuỗi.

---

# 12. Error Localization Strategy
- **Nguyên tắc ngắt ly:** Domain Layer **TUYỆT ĐỐI KHÔNG** chứa mã xử lý đa ngôn ngữ (Localization/I18N).
- **Tiêu chuẩn:** Chuỗi `Message` bên trong Error chỉ là tiếng Anh dành cho Developer (Developer-facing message). Tầng Presentation sẽ sử dụng `ErrorCode` để map sang resource tiếng Việt/Nhật tương ứng đưa cho người dùng (User-facing message).

---

# 13. Error Catalog, Registry & Namespace
- **Error Catalog:** Là một kho chứa tập trung tĩnh (Static Registry) khai báo tất cả các mã lỗi có thể có.
- **Error Module & Prefix:** Sử dụng Prefix để tránh đụng độ (Collision) giữa các Module.
  - VD Prefix: `DEM` (Demographic), `SOC` (Social Security).
- **Error Code Convention:**
  - Định dạng khuyên dùng: `[Context].[Entity].[Issue]` (VD: `Demographic.Citizen.NotFound`).
  - Hoặc định dạng mã: `ERR-[MODULE]-[ID]` (VD: `ERR-DEM-001`). Domain Architecture khuyến nghị dùng định dạng chuỗi đọc được (Human-readable string) hơn là mã số vô hồn.

---

# 14. Relationship With Result Pattern & Exception
- **Result Pattern:** Error là "linh hồn" của phần `Failure` trong Result Pattern. Một `Result` thất bại bắt buộc phải ôm theo ít nhất 1 `Error`.
- **Exception:** Error sinh ra để THAY THẾ Exception trong luồng nghiệp vụ. Exception chỉ tồn tại nếu hạ tầng (Network, Disk) gặp sự cố.

---

# 15. Relationship With Aggregate & Value Object
- Aggregate và Value Object sử dụng các Error được định nghĩa trong Catalog để từ chối khởi tạo (trả về qua Factory/Result) khi Invariants bị xâm phạm.

---

# 16. Relationship With Domain Service & Policy
- Domain Service và Policy sử dụng Error để báo cáo sự thất bại của các quy tắc liên-Aggregate hoặc quy trình tính toán phức tạp.

---

# 17. Relationship With Validator & Specification
- **Validator:** Domain Validator trả về mảng (List) các `Error`. Các lỗi này phải được tham chiếu từ Catalog, không tự gõ tay text (hard-code).
- **Specification:** Specification trả về boolean, nhưng nếu tích hợp vào quá trình xác thực, khi trả về `false`, hệ thống sẽ nạp một `Error` tương ứng vào Result.

---

# 18. Relationship With Repository & Factory
- **Repository:** Trả về `NotFound Error` từ Catalog nếu không tìm thấy dữ liệu. Không ném Exception.
- **Factory:** Trả về `Validation Errors` hoặc `State Errors` nếu lắp ráp Aggregate thất bại.

---

# 19. Relationship With Application Layer
- Application Layer là "người tiêu thụ" (Consumer) của Error. Nó đọc `ErrorCode` và `ErrorCategory` để quyết định ánh xạ sang HTTP 400, 404, 403, 409...

---

# 20. Naming Convention & Error Code Convention
- Tên hằng số/biến định nghĩa lỗi: UpperCamelCase (VD: `InvalidAgeError`).
- Error Code (Mã lỗi văn bản): PascalCase có dấu chấm (VD: `Citizen.InvalidAge`).
- Error Message (Nội dung): Rõ ràng, súc tích, mô tả cái gì sai (VD: "The citizen's age must be over 18.").

---

# 21. Folder Strategy
Cấu trúc thư mục tại tầng Domain:
- `Errors/`
  - `SeedWork/` (Base Error structure concept).
  - `Catalogs/`
    - `DemographicErrors/` (Khai báo lỗi module Demographic).
    - `SocialSecurityErrors/` (Khai báo lỗi module Social Security).
  - `Shared/` (Các lỗi chung như `ValueIsRequired`).

*(Lưu ý: Chỉ thiết lập định hướng thư mục, không sinh source code tại đây).*

---

# 22. Dependency Rules
Ràng buộc phụ thuộc:
- **Allowed Dependencies:** Chỉ phụ thuộc vào các System types (String, Int) cơ bản.
- **Forbidden Dependencies:** TUYỆT ĐỐI CẤM tham chiếu đến `Microsoft.AspNetCore.Mvc`, các thư viện Localization, `EntityFrameworkCore`, hay bất kỳ HTTP Framework nào.

---

# 23. Validation Rules
Kiến trúc Error Catalog được coi là hợp lệ khi:
- Không có lỗi nào bị trùng lặp Mã (Error Code).
- Mỗi lỗi đều được gán đúng Category (Loại lỗi).
- Thông điệp lỗi (Message) không chứa dữ liệu nhạy cảm (như mật khẩu, chuỗi kết nối).
- Lỗi không mang đặc tính của ngôn ngữ hiển thị UI (Không gắn resource file).

---

# 24. Performance Strategy
- Tối ưu hóa khởi tạo: Khai báo toàn bộ các Error dưới dạng hằng số tĩnh (Static ReadOnly / Constants) để không tốn chi phí khởi tạo bộ nhớ (Zero Allocation) mỗi khi văng lỗi.

---

# 25. Evolution Strategy
- **Sprint 03:** Thiết lập Architecture Specification.
- **Sprint 04:** Triển khai khung sườn Catalog tĩnh (Static Error Catalog).
- **Sprint 05:** Mở rộng Catalog với Metadata động.
- **Sprint 06:** Tích hợp với công cụ sinh mã tự động (T4/Source Generator) để quét lỗi toàn miền và xuất file Markdown/Swagger cho Team Frontend.

---

# 26. AI Coding Constraints
- KHÔNG tạo bất kỳ file C# nào triển khai (`class`, `enum`, `record`) cho Error Catalog tại đây.
- KHÔNG sinh mã JSON, XML, UML, hay Pseudo Code.
- CẤM việc nhét `HttpStatusCode` vào bên trong cấu trúc của Error.
- CẤM việc sinh file `.resx` (Localization) ở tầng Domain.
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 27. Definition Of Done
Tài liệu được coi là hoàn tất khi:
- Toàn bộ ma trận phân loại lỗi (Taxonomy, Hierarchy) được làm sáng tỏ.
- 17 Category lỗi được liệt kê rành mạch.
- Bức tường ngăn cách giữa Domain Error và Infrastructure / Localization được thiết lập cứng rắn.
- Đáp ứng chuẩn mực Enterprise Architecture.

---

# 28. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **No HTTP Status** | Lỗi không chứa mã 404, 400, 500. | Bắt buộc (Mandatory) |
| **No Exceptions Wrap** | Cấm dùng Domain Error làm vỏ bọc cho SqlException. | Bắt buộc (Mandatory) |
| **No UI Localization** | Cấm tích hợp ResourceManager/I18N vào Domain. | Bắt buộc (Mandatory) |
| **Unique Code** | Error Code phải là duy nhất trên toàn hệ thống. | Bắt buộc (Mandatory) |
| **Static Allocation** | Tái sử dụng đối tượng lỗi, không new liên tục. | Bắt buộc (Mandatory) |
| **No Magic Strings** | Bắt buộc gọi lỗi từ Catalog, cấm gõ chuỗi tay. | Bắt buộc (Mandatory) |

---

# 29. Architectural Risks
- **Error Explosion:** Không kiểm soát danh mục dẫn đến có quá nhiều lỗi tương đồng (như `FirstNameEmpty`, `LastNameEmpty` thay vì dùng chung `FieldRequired` + Metadata).
- **Vague Errors:** Khai báo các lỗi quá chung chung như `Citizen.Failed`, khiến tầng Application không biết nên xử lý thế nào.

---

# 30. Architectural Anti Patterns
Các mẫu phản kiến trúc (Anti-Patterns) nghiêm trọng cần tránh:

- **Hard-coded Error Message:**
  - Developer viết `Result.Failure(new Error("Code", "Chuỗi gõ tay trong hàm"))` thay vì lấy từ Catalog tĩnh, làm hỏng khả năng quản lý lỗi tập trung.
- **Duplicated Error:**
  - Hai Module khác nhau định nghĩa cùng một mã `Err001` nhưng mang hai ý nghĩa khác nhau.
- **Exception wrapped as Error:**
  - Nhét nguyên một object `Exception` (kèm StackTrace) vào bên trong Domain Error. Điều này gây rò rỉ dữ liệu nhạy cảm và làm phình bộ nhớ.
- **HTTP Status inside Error:**
  - Thêm property `StatusCode = 404` vào Domain Error. Vi phạm tính độc lập miền (Domain Independence).
- **Localized Text inside Domain:**
  - Viết logic kiểm tra ngôn ngữ `if (lang == "vi") return "Lỗi"; else return "Error";` ngay tại Domain. (Đây là thảm họa kiến trúc).
- **Magic String Error:**
  - Dùng chuỗi tự do làm mã lỗi thay vì cấu trúc phân loại.
- **Mixed Infrastructure Error:**
  - Trả về `DomainError` mang nội dung "Kết nối CSDL thất bại ở cổng 1433".
- **Error without Code / without Category:**
  - Lỗi chỉ có Message mà không phân loại (Category) khiến tầng Application không thể map ra mã HTTP phù hợp.
- **Generic Unknown Error:**
  - Lạm dụng một lỗi chung chung `System.Error` cho mọi sự cố nghiệp vụ.

---

# 31. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã phân loại cặn kẽ 17 loại Error Categories?
- [x] Đã giải thích rạch ròi Error với Exception, Validation, HTTP Status?
- [x] Đã thiết lập bức tường lửa chặn Localization và Infrastructure lọt vào Error?
- [x] Đã phân tích chi tiết các Anti-Patterns (nhất là Hard-code, HTTP leak, Localized leak)?
- [x] Đã tuân thủ chuẩn format khắt khe của Enterprise Documentation Suite? (CÓ).

---

# 32. References
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

---
# END OF SPECIFICATION
