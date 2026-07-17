# 36_SPRINT_03_SPECIFICATIONS.md
## SPRINT 03 – SPECIFICATIONS PATTERN ARCHITECTURE
Version: 1.1.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 36_SPRINT_03_SPECIFICATIONS
- **Title:** Domain Specifications Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Principal Software Architect, Domain Expert, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft - Enterprise Architecture Specification for Domain Specifications |
| 1.1.0 | 2026-07-17 | Solution Architecture Team | Added Relationship with Building Blocks, Anti-Patterns, Sprint07 Evolution, and expanded Performance & Folder strategies. |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Specification Pattern Architecture.
- KHÔNG ĐƯỢC nới lỏng Dependency Rules (Cấm mọi ngoại lệ phụ thuộc hạ tầng như Entity Framework hoặc Dapper).
- KHÔNG ĐƯỢC phá vỡ tính chất Read-Only và Immutability của Specification.
- Mọi thay đổi phát sinh bắt buộc phải thông qua một Revision mới của tài liệu và được phê duyệt lại bởi hội đồng kiến trúc trước khi áp dụng.

---

# 4. Purpose
Tài liệu này là đặc tả kiến trúc (Architecture Specification) toàn diện xác định chiến lược thiết kế và sử dụng Specification Pattern (Mẫu Đặc tả) bên trong tầng Domain. Mục tiêu cốt lõi là định hình một phương pháp chuẩn hóa, mạnh mẽ để đóng gói các quy tắc nghiệp vụ (Business Rules), điều kiện (Conditions) và tiêu chí lựa chọn (Selection Criteria) thành các đối tượng độc lập. Việc này giúp ngăn ngừa sự phân mảnh của logic nghiệp vụ rải rác khắp hệ thống, đồng thời đảm bảo tính tái sử dụng, khả năng kết hợp và tính tường minh trong thiết kế Domain-Driven Design (DDD).

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa khái niệm, bản chất và cấu trúc kiến trúc của Specification Pattern.
- Thiết lập ranh giới rõ ràng giữa Specification và các mô hình kiến trúc khác (Validator, Repository, Query Object).
- Quy định các nguyên tắc thiết kế bất biến (Immutability), tính kết hợp (Composability) và quy tắc đánh giá (Evaluation Strategy).
- Phân định chiến lược phân loại, đặt tên, và chiến lược phối hợp với Aggregate.

**Out of Scope (Ngoài phạm vi):**
- KHÔNG sinh mã nguồn C#, Pseudo Code, XML, JSON hay UML dưới mọi hình thức.
- KHÔNG mô tả cách chuyển đổi Specification thành Expression Trees hay LINQ để truy vấn Database.
- KHÔNG mô tả chi tiết Implementation của Entity Framework, Dapper, hay bất kỳ ORM nào.
- KHÔNG mô tả Command Query Responsibility Segregation (CQRS) hay MediatR.
- KHÔNG cung cấp các ví dụ lập trình cụ thể (Interfaces, Classes, Methods).

---

# 6. Objectives
- **Đóng gói Logic Nghiệp vụ:** Chuyển đổi các quy tắc nghiệp vụ ẩn giấu trong các khối lệnh `if-else` phức tạp thành các đối tượng có tên gọi mang ý nghĩa (Ubiquitous Language).
- **Tính Tái sử dụng (Reusability):** Viết quy tắc một lần và sử dụng lại ở nhiều nơi (Validation, Selection, Building to Order) mà không bị trùng lặp mã.
- **Tính Kết hợp (Composability):** Cho phép kết hợp các quy tắc đơn giản thành các quy tắc phức tạp thông qua các phép toán logic (AND, OR, NOT) một cách tự nhiên.
- **Bảo vệ Trọng tâm Miền (Domain Centricity):** Đảm bảo 100% Persistence Ignorance, ngăn chặn hoàn toàn sự rò rỉ của truy vấn dữ liệu hạ tầng (Infrastructure Leakage) vào tầng Domain.

---

# 7. Specification Pattern Architecture
**Bản chất Kiến trúc (Architectural Essence):**
- **Specification là gì:** Là một đối tượng miền (Domain Object) chứa một quy tắc nghiệp vụ (Business Rule) duy nhất, được sử dụng để kiểm tra xem một đối tượng khác (thường là Entity hoặc Aggregate) có thỏa mãn quy tắc đó hay không.
- **Business Rule là gì:** Là những chính sách, điều kiện, rào cản mang tính quyết định đến trạng thái hoặc luồng công việc của hệ thống (Ví dụ: "Công dân phải trên 18 tuổi để nhận trợ cấp").
- **Rule Object:** Specification biến một quy tắc nghiệp vụ vô hình thành một thực thể hữu hình (First-class citizen) trong kiến trúc.
- **Composable Rules (Khả năng kết hợp):** Nhiều Specification độc lập có thể được móc nối với nhau tạo thành một mạng lưới các quy tắc phức tạp.
- **Reusable Rules (Khả năng tái sử dụng):** Một Specification duy nhất có thể được sử dụng để: (1) Kiểm tra tính hợp lệ của một đối tượng trong bộ nhớ, (2) Lọc danh sách các đối tượng từ bộ nhớ, (3) Định nghĩa tiêu chí để lấy dữ liệu.
- **Business Predicate (Vị từ nghiệp vụ):** Trung tâm của Specification là một phép thử (Predicate) trả về đúng (Satisfied) hoặc sai (Not Satisfied).
- **Business Expression:** Sự biểu đạt của quy tắc phải hoàn toàn bằng ngôn ngữ miền, không phải bằng ngôn ngữ cơ sở dữ liệu.

**Phân định ranh giới (Boundary Clarification):**
- **Specification không phải Validator:** Validator (như FluentValidation) tập trung vào việc kiểm tra tính đúng đắn của dữ liệu đầu vào (DTOs, Form data) thường ở tầng Application. Specification đánh giá các điều kiện nghiệp vụ cốt lõi dựa trên trạng thái của đối tượng miền (Domain Object).
- **Specification không phải Repository:** Specification KHÔNG thực thi việc lấy dữ liệu. Nó chỉ là *tiêu chí*. Repository là đối tượng nhận Specification và thực thi việc lấy dữ liệu dựa trên tiêu chí đó.
- **Specification không phải Query Object:** Query Object (trong CQRS) đại diện cho một thao tác đọc toàn diện, trả về DTO cho Presentation. Specification nằm gọn trong Domain, xử lý trên Entity/Aggregate.

---

# 8. Specification Creation Order
Quy trình và thứ tự thiết kế kiến trúc Specification (chỉ áp dụng khi vào giai đoạn Coding):
- **Step 1:** Nhận diện các quy tắc nghiệp vụ cốt lõi (Business Rules) từ yêu cầu hệ thống.
- **Step 2:** Trừu tượng hóa quy tắc thành lớp cơ sở Specification.
- **Step 3:** Thiết kế cơ chế Composite (AND, OR, NOT).
- **Step 4:** Tạo các Specific Specifications cho từng Bounded Context/Module.
- **Step 5:** Định nghĩa ranh giới tương tác giữa Specification và Aggregate.
- **Step 6:** Validation toàn bộ kiến trúc.
- **Step 7:** Freeze (Đóng băng đặc tả) và chuyển sang Coding.

---

# 9. Specification Evolution Strategy
Lộ trình tiến hóa của Specification Pattern qua các giai đoạn Sprint:
- **Sprint 03:** Thiết lập Seed Work đặc tả, định hình nguyên lý cốt lõi, cơ chế Composite và Evaluation Strategy tại tầng Domain. KHÔNG có bất kỳ liên kết hạ tầng nào.
- **Sprint 04:** Xây dựng hàng loạt các Specification phục vụ cho Validation và Business Rules của các Entity cụ thể thuộc Bounded Context Dân cư và An sinh xã hội.
- **Sprint 05:** Tại tầng Infrastructure, xây dựng cơ chế Evaluator/Translator để biên dịch (translate) các Domain Specification thuần túy thành các truy vấn cơ sở dữ liệu (Database Queries) mà không làm ô nhiễm tầng Domain.
- **Sprint 06:** Tầng Application sẽ khởi tạo Specification và truyền xuống Repository để truy xuất dữ liệu một cách linh hoạt.
- **Sprint 07:**
  - *Performance Optimization:* Tối ưu hóa cấu trúc cây quy tắc, hạn chế overhead khi đánh giá hàng loạt in-memory.
  - *Compiled Specification:* Biên dịch các quy tắc logic thành các function delegates (`Func<T, bool>`) được cache lại để đánh giá nhanh hơn.
  - *Caching Strategy:* Lưu trữ kết quả đánh giá của các Specification tốn kém nếu tham số và trạng thái Aggregate không đổi.
  - *Advanced Composite Rules:* Hỗ trợ các luật gom nhóm phức tạp hơn (ví dụ: AtLeastOne, Majority).
  - *Large-scale Business Rule Library:* Đóng gói các đặc tả thành một thư viện quy tắc nghiệp vụ quy mô lớn cho toàn bộ Enterprise.

---

# 10. Specification Principles
Các nguyên tắc thiết kế bất di bất dịch đối với một Domain Specification:
- **Single Responsibility (Trách nhiệm duy nhất):** Mỗi Specification chỉ được phép biểu diễn MỘT quy tắc nghiệp vụ hoặc MỘT tiêu chí phân loại duy nhất.
- **Reusable (Tái sử dụng):** Kiến trúc phải cho phép Specification được khởi tạo một lần và gọi đánh giá ở nhiều vị trí (trong Aggregate, trong Domain Service).
- **Composable (Có khả năng kết hợp):** Phải tuân thủ mẫu Composite, cho phép tạo ra quy tắc lớn từ nhiều quy tắc nhỏ.
- **Immutable (Bất biến):** Một Specification, sau khi được khởi tạo với các tham số điều kiện, không bao giờ thay đổi trạng thái. Nó là một đối tượng Read-Only hoàn toàn.
- **No Infrastructure:** Tuyệt đối không chứa cấu trúc dữ liệu hạ tầng, không chứa khóa ngoại (Foreign Keys) liên quan đến Database schema.
- **No Database:** Không quan tâm dữ liệu được lưu trữ ở đâu, dùng SQL hay NoSQL.
- **No HTTP / No API:** Không có kiến thức về Web context, Request, Response, Status Code.
- **Persistence Ignorance:** Mù tịt hoàn toàn về sự tồn tại của hệ thống lưu trữ lâu dài.
- **Framework Ignorance:** Không sử dụng thư viện bên ngoài để định nghĩa quy tắc (chỉ sử dụng C# thuần túy).

---

# 11. Classification Strategy
Chiến lược phân loại Specification dựa trên mục đích sử dụng trong Domain:
- **Validation Specification:** Đánh giá xem một thực thể có đang ở trạng thái hợp lệ để tiếp tục tồn tại hoặc lưu trữ hay không. (Ví dụ: `CitizenMustHaveValidIdentitySpecification`).
- **Eligibility Specification (Đặc tả điều kiện):** Đánh giá xem một thực thể có đủ điều kiện để tham gia một quy trình nghiệp vụ cụ thể hay không. (Ví dụ: `EligibleForPensionSpecification`).
- **Authorization/Permission Specification:** (Hiếm gặp hơn nhưng khả thi) Đánh giá xem một đối tượng có quyền hạn nhất định dựa trên quy tắc miền hay không. (Ví dụ: `HouseholdHeadCanModifyMembersSpecification`).
- **Business Rule Specification:** Đại diện cho một quy tắc nghiệp vụ cốt lõi, thường dùng trong Domain Services. (Ví dụ: `DisbursementLimitNotExceededSpecification`).
- **State Specification:** Kiểm tra trạng thái vòng đời của một thực thể. (Ví dụ: `PolicyIsActiveSpecification`).
- **Cross Aggregate Specification:** Đánh giá các điều kiện liên đới giữa hai hay nhiều Aggregate (thường được điều phối bởi Domain Service).

---

# 12. Composite Specification
Cơ chế kết hợp (Composability) là sức mạnh cốt lõi của kiến trúc này, bao gồm các phép toán:
- **AND (Giao):** Kết hợp hai Specification, chỉ trả về Satisfied khi CẢ HAI đều Satisfied.
- **OR (Hợp):** Kết hợp hai Specification, trả về Satisfied khi ÍT NHẤT MỘT Specification Satisfied.
- **NOT (Phủ định):** Đảo ngược kết quả đánh giá của một Specification.
- **Nested Specification (Lồng ghép):** Cho phép nhóm các phép AND, OR thành các khối logic phức tạp (tương đương với dấu ngoặc đơn trong biểu thức toán học).
- **Short Circuit Evaluation (Đánh giá đoản mạch):** Kiến trúc phải yêu cầu cơ chế đánh giá dừng lại ngay lập tức khi kết quả logic đã chắc chắn (Ví dụ: phép AND dừng ngay nếu vế đầu tiên Not Satisfied) nhằm tối ưu hiệu suất In-Memory.

---

# 13. Evaluation Strategy
Chiến lược thực thi và đánh giá (Evaluation) của Specification:
- **Satisfied:** Kết quả trả về khi đối tượng mục tiêu vượt qua hoàn toàn quy tắc kiểm tra.
- **Not Satisfied:** Kết quả trả về khi đối tượng mục tiêu vi phạm quy tắc.
- **Evaluation Result:** Không chỉ trả về boolean (true/false), kiến trúc hướng tới việc cung cấp nguyên nhân gốc rễ (Root Cause) hoặc thông báo lỗi nghiệp vụ chi tiết (Business Error Message) khi Not Satisfied, nhằm hỗ trợ tầng trên ném `DomainException` chính xác.
- **Business Predicate:** Logic đánh giá (Evaluation Logic) phải được cô lập thành một Predicate thuần túy, hoạt động hoàn toàn trên bộ nhớ (In-Memory).

---

# 14. Aggregate Collaboration Rules
Quy tắc tương tác giữa Specification và Aggregate:
- **Specification chỉ đọc (Read-Only):** Specification nhận Aggregate vào để phân tích, nó tuyệt đối không bao giờ thay đổi (mutate) trạng thái của Aggregate đó.
- **Không sửa Entity:** Nghiêm cấm mọi hành vi thao tác vào các phương thức hay thuộc tính có tính chất thay đổi dữ liệu của Entity.
- **Không Save:** Specification không có khái niệm lưu trữ hay ghi nhận thay đổi (No Commit, No SaveChanges).
- **Không gọi Repository trực tiếp:** Specification là một tiêu chí độc lập, nó KHÔNG ĐƯỢC ôm trong mình Repository để tự đi tìm dữ liệu khác. Mọi dữ liệu cần thiết để Specification đánh giá phải được cung cấp sẵn (qua tham số constructor) hoặc nằm ngay trong cấu trúc của Aggregate được đánh giá.

---

# 15. Relationship With Other Domain Building Blocks
Mối quan hệ và sự phối hợp giữa Specification với các khối kiến trúc miền khác:
- **Entity & Aggregate Root:** Specification không thay thế Entity hay Aggregate Root. Nó đứng bên ngoài để đánh giá (evaluate) trạng thái của các đối tượng này. Aggregate giữ trạng thái, Specification giữ quy tắc.
- **Value Object & Domain Primitive:** Specification sử dụng Value Object và Domain Primitive làm tham số điều kiện (parameters). Ví dụ: `AgeGreaterThanSpecification(Age limit)`. Specification không lưu trữ chúng mà chỉ dùng để so sánh.
- **Domain Service:** Domain Service thường điều phối nhiều Aggregate và có thể gọi nhiều Specification để đưa ra quyết định nghiệp vụ phức tạp. Specification làm cho Domain Service trở nên gọn gàng hơn vì các `if-else` phức tạp đã bị ẩn đi.
- **Repository:** Specification định nghĩa "Cái gì cần lấy", còn Repository định nghĩa "Lấy như thế nào". Specification hoàn toàn mù tịt về CSDL, nó chỉ được truyền vào Repository để tầng hạ tầng dịch thành SQL.
- **Application Layer:** Application Layer khởi tạo Specification và truyền nó xuống Repository hoặc Domain Service.

*Nhấn mạnh:* Specification KHÔNG thay thế bất kỳ thành phần nào trên đây. Nó chỉ đóng vai trò thuần túy là **mô tả Business Rules** một cách độc lập và tái sử dụng.

---

# 16. Naming Convention
Quy tắc đặt tên bắt buộc cho Specification:
- Tên phải phản ánh rõ quy tắc nghiệp vụ đang được kiểm tra.
- Tên kết thúc bằng hậu tố `Specification`.
- Tên phải là một câu khẳng định hoặc biểu thức điều kiện rõ ràng.

**Allowed (Cho phép):**
- `CitizenIsEligibleForPensionSpecification`
- `HouseholdHasActiveStatusSpecification`
- `BenefitDisbursementNotExceededSpecification`
- `ActivePolicySpecification`

**Not Allowed (Tuyệt đối cấm):**
- `CitizenSpec` (Tên quá ngắn, không có ý nghĩa nghiệp vụ).
- `CheckCitizenSpecification` (Dùng động từ hành động, Specification là trạng thái/quy tắc, không phải là Hàm thực thi).
- `ValidateDataSpecification` (Tên chung chung mang tính kỹ thuật).
- `GetCitizenByIdSpecification` (Đây là Query, không phải Business Rule).

---

# 17. Folder Strategy
Chiến lược tổ chức cấu trúc thư mục đặc tả:
- `Specifications/`
  - `Common/`: Chứa các Specification dùng chung cho toàn hệ thống (VD: `ActiveEntitySpecification`).
  - `Composite/`: Chứa định nghĩa kiến trúc lõi của các phép toán `AndSpecification`, `OrSpecification`, `NotSpecification`.
  - `Internal/`: Chứa các Specification chỉ được dùng nội bộ trong một Aggregate cụ thể, không phơi bày ra ngoài.
  - `Modules/`:
    - `Demographic/`: Quy tắc thuộc module Dân cư (VD: `CitizenAgeSpecification`).
    - `SocialSecurity/`: Quy tắc thuộc module An sinh xã hội (VD: `EligibleForPensionSpecification`).
    - `Disbursement/`: Quy tắc thuộc module Giải ngân.
    - `Communication/`: Quy tắc truyền thông.
    - `Analytics/`: Quy tắc phân tích dữ liệu.

*(Lưu ý: Chỉ thiết lập quy tắc thư mục định hướng kiến trúc, không sinh source code tại đây).*

---

# 18. Dependency Rules
Ràng buộc phụ thuộc cứng cho Specification:

**Allowed Dependencies:**
- `System.*` (Các thư viện core C# phục vụ logic và cấu trúc dữ liệu cơ bản).
- Các thành phần nội bộ của tầng `Domain` (Entities, Value Objects, Primitives).

**Forbidden Dependencies (Tuyệt đối cấm):**
- `EntityFrameworkCore` (Tuyệt đối cấm. Mọi translation sang SQL phải nằm ở Infrastructure).
- `Dapper`
- `MediatR`
- `Logging Framework` (Serilog, NLog, ILogger).
- `HTTP Library`
- `ASP.NET` (Core, MVC, API).
- `SignalR`
- Các thư viện ORM, Caching, EventBus bên ngoài.

---

# 19. Validation Rules
Kiến trúc Specification được xem là hợp lệ (Valid) khi vượt qua các chốt chặn sau:
- **Pure Logic:** Chỉ chứa toán tử so sánh (>, <, ==, !=) và các hàm thuần túy.
- **Stateless:** Không giữ trạng thái liên phiên (Session State), chỉ giữ tham số quy tắc (Thresholds, Limits) được truyền vào một lần duy nhất lúc khởi tạo.
- **In-Memory Ready:** Bất kỳ Specification nào cũng phải có khả năng thực thi thành công đối với một đối tượng được tạo trực tiếp bằng từ khóa `new` trên bộ nhớ (không cần DB).
- **Expression Translation Ready:** Dù không chứa công nghệ EF Core, nhưng cấu trúc logic bên trong (Predicate) phải đủ chuẩn mực (không dùng các hàm phức tạp không thể dịch được) để tầng hạ tầng sau này có thể dễ dàng map sang Expression Tree.

---

# 20. Performance Considerations
Nhận định về hiệu suất và rủi ro In-Memory:
- **Cost of Composite Specification:** Xây dựng cây AND/OR sâu gây tốn bộ nhớ khởi tạo các lớp bao bọc. Cần thiết kế nhẹ (lightweight).
- **Expression Translation Cost:** Việc dịch từ Expression Tree sang SQL tại hạ tầng cần được biên dịch và cache lại, không dịch lại từ đầu mỗi lần gọi.
- **Memory Evaluation:** Đánh giá in-memory cực nhanh nhưng phải đảm bảo Entity đã nạp đủ (Eager Loading) để tránh Exception hoặc sai logic.
- **Short Circuit Optimization:** Tối ưu hóa đoản mạch. Cấu trúc AND/OR phải dừng đánh giá ngay khi vế đầu xác định được kết quả cuối cùng.
- **Specification Reuse:** Singleton hoặc Caching các Specification không có tham số thay đổi để giảm chi phí cấp phát bộ nhớ.
- **Stateless Benefits:** Do không mang trạng thái giao dịch, Specification an toàn với đa luồng (Thread-safe) trong đánh giá in-memory.
- **Đánh giá trên bộ nhớ so với CSDL:** Do Specification Pattern hoạt động trên bộ nhớ tại tầng Domain, việc lạm dụng kéo hàng triệu dòng dữ liệu lên RAM sau đó mới chạy Specification để lọc là tối kỵ. Specification được tạo ở Domain, nhưng tầng Application sẽ truyền nó xuống Repository để Infrastructure chuyển dịch thành truy vấn Database (SQL Where clause).
- **Tránh tính toán phức tạp (Heavy Computation):** Specification nên tập trung vào quy tắc logic bool (True/False). Nếu quy tắc đòi hỏi các thuật toán AI, Machine Learning, hay các phép tính ma trận nặng nề, nó không nên nằm trong Specification cơ bản mà phải được cô lập sang các Calculation Services chuyên dụng.

---

# 21. AI Coding Constraints
Giới hạn hành vi bắt buộc của AI Coding Agent khi đọc hiểu và triển khai tài liệu này:
- KHÔNG tạo Interface (`ISpecification<T>`).
- KHÔNG tạo Class (`BaseSpecification<T>`).
- KHÔNG sinh mã C# khai báo các Expression Trees hay Predicates.
- KHÔNG sinh mã JSON, XML, UML, hay giả mã (Pseudo Code).
- KHÔNG mô tả cách thiết lập MediatR Pipeline hay DbContext.
- Nhiệm vụ duy nhất là duy trì bản chất văn bản kiến trúc (Architecture Text).

---

# 22. Definition Of Done
Tài liệu được định nghĩa là hoàn tất (Done) khi:
- Đã thiết lập rõ ràng triết lý Specification Pattern trong DDD.
- Đã phân định ranh giới cấm vượt qua giữa Specification và Infrastructure/Application.
- Đã quy định rõ cách thức kết hợp (Composite) và đánh giá (Evaluation).
- Đã bao hàm các cảnh báo rủi ro (Risks) và chiến lược phòng ngừa đầy đủ.
- Đồng bộ hoàn hảo với các định dạng và tiêu chuẩn của tài liệu 31 đến 35.

---

# 23. Validation Matrix
Ma trận tự kiểm định kiến trúc (Architecture Validation Matrix):

| Yếu tố Kiến trúc | Yêu cầu Kỹ thuật khắt khe | Trạng thái Đánh giá |
|---|---|---|
| **Business Purity** | Đảm bảo tính tinh khiết của logic nghiệp vụ, không vướng bận IO. | Bắt buộc (Mandatory) |
| **Read-only** | Specification chỉ được quyền truy xuất (đọc) thuộc tính. | Bắt buộc (Mandatory) |
| **No Mutation** | Tuyệt đối cấm thao tác gán lại hoặc làm thay đổi State của Entity. | Bắt buộc (Mandatory) |
| **Composite Ready** | Phải kết hợp được qua AND, OR, NOT một cách tự nhiên. | Bắt buộc (Mandatory) |
| **Framework Independent** | Không tham chiếu Entity Framework, MediatR hay ASP.NET. | Bắt buộc (Mandatory) |
| **DDD Compliance** | Phản ánh chính xác Ubiquitous Language trong Naming & Logic. | Bắt buộc (Mandatory) |
| **Persistence Ignorance** | Specification mù tịt về CSDL, Table, Query, SQL. | Bắt buộc (Mandatory) |
| **Side-effect Free** | Specification chỉ "Hỏi", không bao giờ "Làm thay đổi". | Bắt buộc (Mandatory) |

---

# 24. Risks
Cảnh báo các rủi ro kiến trúc (Architectural Risks) thường gặp và cách giảm thiểu:
- **God Specification (Đặc tả Thượng đế):** Một Specification khổng lồ chứa hàng chục mệnh đề AND, OR, kiểm tra hàng chục quy tắc khác nhau, phá vỡ Single Responsibility.
  - *Mitigation:* Bắt buộc tách nhỏ thành nhiều Specification nguyên thủy (Primitive Specifications) và dùng Composite (AND/OR) để nối lại.
- **Too Generic Specification (Đặc tả quá chung chung):** Tạo các quy tắc chung chung như `ValueGreaterThanSpecification` thay vì gắn với nghiệp vụ `AgeGreaterThanMajoritySpecification`.
  - *Mitigation:* Tuân thủ Naming Convention bằng Ubiquitous Language.
- **Repository Leakage (Rò rỉ Repository):** Truyền `IRepository` vào trong Constructor của Specification để nó tự đi truy vấn thêm dữ liệu kiểm tra.
  - *Mitigation:* Cấm tuyệt đối. Mọi dữ liệu phụ thuộc phải được tính toán trước và truyền vào Specification dưới dạng tham số cơ bản (Primitive Values) hoặc Value Objects.
- **Infrastructure Leakage (Rò rỉ Hạ tầng):** Cố gắng sử dụng các phương thức dành riêng cho Database (như `EF.Functions.Like`) ngay bên trong lớp Specification ở tầng Domain.
  - *Mitigation:* Tầng Domain chỉ dùng các thao tác bộ nhớ chuẩn của ngôn ngữ lập trình. Việc mapping sang SQL thuộc về Infrastructure Evaluators.
- **Business Rule Duplication (Trùng lặp Quy tắc):** Developer lười biếng viết cứng (hard-code) logic `if (age < 18)` vào trực tiếp Service thay vì dùng `IsAdultSpecification` đã tồn tại.
  - *Mitigation:* Quy định chặt chẽ trong Code Review: Mọi Business Rule tái sử dụng bắt buộc phải đóng gói thành Specification.

---

# 25. Architectural Anti-Patterns
Các mẫu phản kiến trúc (Anti-Patterns) thường gặp khi áp dụng Specification và cách xử lý triệt để:

- **God Specification:**
  - *Nguyên nhân:* Nhồi nhét hàng chục quy tắc kiểm tra (AND, OR hỗn loạn) vào chung một class để "tiết kiệm file".
  - *Hậu quả:* Mất khả năng tái sử dụng, không thể debug vế nào bị sai, vi phạm SRP.
  - *Cách phòng tránh:* Chia nhỏ thành các Primitive Specifications độc lập và nối chúng lại.
- **Fat Specification:**
  - *Nguyên nhân:* Specification chứa quá nhiều tham số khởi tạo cấu hình phức tạp.
  - *Hậu quả:* Khó khởi tạo, dễ truyền nhầm tham số, giảm tính minh bạch.
  - *Cách phòng tránh:* Giới hạn số lượng tham số. Gom nhóm tham số thành Value Object nếu cần.
- **Repository Injection:**
  - *Nguyên nhân:* Bơm thẳng `IRepository` vào Specification để tra cứu CSDL trong lúc đánh giá.
  - *Hậu quả:* Gây Hidden I/O, chặn quá trình Unit Test in-memory, biến Specification thành một Service lậu.
  - *Cách phòng tránh:* Aggregate/Domain Service phải lấy đủ Data và truyền vào cho Specification.
- **Database Query Inside Specification:**
  - *Nguyên nhân:* Đính kèm các chuỗi raw SQL hoặc thuộc tính đặc tả riêng của ORM.
  - *Hậu quả:* Domain Layer bị ô nhiễm bởi Infrastructure Layer.
  - *Cách phòng tránh:* Sử dụng chuẩn LINQ Expressions thuần C# để hạ tầng tự động translate.
- **HTTP Logic:**
  - *Nguyên nhân:* Tham chiếu `HttpContext` để kiểm tra quyền người dùng ngay trong Specification.
  - *Hậu quả:* Domain Layer bị dính cứng vào Môi trường Web, không dùng được ở Background Job.
  - *Cách phòng tránh:* Identity và Claims phải được Application Layer chuyển đổi thành tham số cơ bản và đưa vào Domain.
- **UI Validation Logic:**
  - *Nguyên nhân:* Kiểm tra độ dài MaxLength, Regular Expressions cho giao diện người dùng bên trong Specification.
  - *Hậu quả:* Trộn lẫn trách nhiệm Validation đầu vào (thuộc Application/FluentValidation) và Business Rule cốt lõi.
  - *Cách phòng tránh:* Tách bạch rõ rệt Input Validation và Domain Logic Evaluation.
- **Infrastructure Leakage:**
  - *Nguyên nhân:* Kế thừa Specification từ một class của thư viện NuGet bên ngoài chưa qua kiểm duyệt.
  - *Hậu quả:* Toàn bộ Domain bị khóa chết vào chu kỳ cập nhật của thư viện đó.
  - *Cách phòng tránh:* Xây dựng lớp Base nội bộ (In-house) hoặc cẩn trọng tuyệt đối khi chọn thư viện DDD base.
- **Technical Naming:**
  - *Nguyên nhân:* Đặt tên theo phong cách CRUD (`CheckUpdateStatusSpec`, `FilterActiveRecordsSpec`).
  - *Hậu quả:* Mất kết nối ngôn ngữ với Domain Expert.
  - *Cách phòng tránh:* Đặt tên bằng câu khẳng định theo quy trình nghiệp vụ (VD: `AccountIsReadyForSettlementSpecification`).

---

# 26. AI Review Checklist
- [x] Có sinh C#, Pseudo Code, UML, XML, JSON hay SQL không? (KHÔNG).
- [x] Đã giải thích cặn kẽ khái niệm Specification và sự khác biệt với Validator, Repo?
- [x] Đã mô tả cơ chế Composite (AND, OR, NOT) một cách khái quát kiến trúc?
- [x] Đã thiết lập các nguyên tắc bất di bất dịch: Single Responsibility, Immutability, No Infrastructure?
- [x] Đã định nghĩa ranh giới tương tác với Aggregate (chỉ đọc, không sửa, không lưu)?
- [x] Đã tuân thủ nghiêm ngặt Dependency Rules (Cấm EF Core, MediatR, Logging)?
- [x] Tài liệu tuân thủ chuẩn Enterprise Documentation Suite với chất lượng cao nhất?

---

# 27. References
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
- 28_SPRINT_02_REVIEW.md
- 29_SPRINT_03_IMPLEMENTATION_PLAN.md
- 30_SPRINT_03_PROJECT_INIT.md
- 31_SPRINT_03_BASE_CLASSES.md
- 32_SPRINT_03_EXCEPTIONS.md
- 33_SPRINT_03_EVENTS.md
- 34_SPRINT_03_DOMAIN_PRIMITIVES.md
- 35_SPRINT_03_DOMAIN_SERVICES.md

---
# END OF SPECIFICATION
