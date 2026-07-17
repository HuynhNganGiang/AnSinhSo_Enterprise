# 31_SPRINT_03_BASE_CLASSES.md
## SPRINT 03 – DOMAIN BASE CLASSES SPECIFICATION
Version: 1.1.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 31_SPRINT_03_BASE_CLASSES
- **Title:** Domain Base Classes Architecture Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Software Architect, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft |
| 1.1.0 | 2026-07-17 | Solution Architecture Team | Added Freeze Rule, Creation Order, Evolution Strategy, Decision Matrices, and Expanded Validations |

---

# 3. Architecture Freeze Rule
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Base Class Strategy.
- KHÔNG ĐƯỢC thay đổi Equality Strategy.
- KHÔNG ĐƯỢC thay đổi Constructor Strategy.
- KHÔNG ĐƯỢC thay đổi Event Strategy.
- Bất kỳ thay đổi nào cũng phải được thực hiện thông qua một Revision mới.

---

# 4. Purpose
Tài liệu này là đặc tả kiến trúc (Architecture Specification) xác định các nguyên tắc thiết kế cho các Lớp cơ sở (Base Classes) của tầng Domain. Mục tiêu là định hình bộ khung (Seed Work) tiêu chuẩn nhằm chuẩn hóa hành vi, cơ chế định danh, cơ chế so sánh và chiến lược lưu trữ sự kiện miền cho mọi thực thể nghiệp vụ sẽ được tạo ra trong các Sprint tiếp theo.

---

# 5. Scope
**In Scope (Trong phạm vi):**
- Định nghĩa nguyên tắc kiến trúc cho `Entity`, `AggregateRoot`, `ValueObject`.
- Định nghĩa chiến lược so sánh (Equality) và định danh (Identity).
- Định nghĩa chiến lược thu thập sự kiện miền (Domain Event Collection).
- Định nghĩa các quy tắc thiết kế chung (Encapsulation, Constructor, Inheritance, Generic).

**Out of Scope (Ngoài phạm vi):**
- KHÔNG mô tả thiết kế của Interfaces (`IRepository`, `IUnitOfWork`, `IAggregateRoot`).
- KHÔNG mô tả thiết kế của Exceptions hay Domain Events chi tiết.
- KHÔNG mô tả bất kỳ Entity nghiệp vụ nào (như Citizen, Household).
- KHÔNG sinh mã nguồn (Source Code), mã giả (Pseudo Code) hay UML.

---

# 6. Objectives
- Thiết lập một nền tảng hướng đối tượng vững chắc và nhất quán cho hệ thống.
- Đảm bảo tính đóng gói (Encapsulation) cao nhất: Không cho phép thay đổi trạng thái trái phép.
- Đồng nhất hóa cơ chế nhận diện thực thể (Identity) và nhận diện giá trị (Value Equality).
- Cung cấp nền tảng tích hợp Domain Event trực tiếp vào vòng đời của Entity.

---

# 7. Base Class Architecture
Kiến trúc Lớp cơ sở (Base Class) được thiết kế theo mô hình phân tầng trừu tượng (Abstract Hierarchy). Các lớp này đóng vai trò là "Seed Work", định nghĩa sẵn các logic lặp lại (boilerplate) mà mọi mô hình miền (Domain Model) đều phải kế thừa. Cấu trúc này không phụ thuộc vào bất kỳ framework ORM nào, đảm bảo tính Persistence Ignorance tuyệt đối.

---

# 8. Base Class Creation Order
Quy trình và thứ tự sinh mã Lớp cơ sở (chỉ áp dụng khi vào giai đoạn Coding):
- **Step 1:** Khởi tạo `Entity` (Xây dựng Identity, Equality, và Event Holding).
- **Step 2:** Khởi tạo `AggregateRoot` (Kế thừa Entity, đánh dấu ranh giới gốc).
- **Step 3:** Khởi tạo `ValueObject` (Xây dựng Deep Value Equality).
- **Step 4:** Validation (Rà soát kiến trúc).
- **Step 5:** Freeze (Đóng băng bộ khung Base Classes).
- **Step 6:** Coding (Bắt đầu áp dụng vào Entity cụ thể).

---

# 9. Base Class Evolution Strategy
Lộ trình phát triển của Lớp cơ sở:
- **Sprint 03:** Khởi tạo Seed Work (Chỉ có Base Classes rỗng/chuẩn mực).
- **Sprint 04:** Business Entities (Sử dụng Base Classes để mô hình hóa nghiệp vụ cốt lõi).
- **Sprint 05:** Infrastructure Mapping (Ánh xạ Base Classes với Database qua ORM Fluent API).
- **Sprint 06:** Application Integration (Liên kết Domain Events trong Base Classes với MediatR ở tầng Application).

---

# 10. Entity Design Principles
- **Định danh duy nhất (Unique Identity):** Mọi Entity bắt buộc phải có một định danh duy nhất không thể thay đổi sau khi khởi tạo.
- **Tính đột biến có kiểm soát (Controlled Mutability):** Trạng thái (thuộc tính) của Entity có thể thay đổi theo thời gian, nhưng mọi sự thay đổi phải thông qua các phương thức nghiệp vụ thể hiện rõ ý định (Intention-Revealing Methods), không thông qua các bộ gán (setters) công khai.
- **Lưu trữ sự kiện (Event Holding):** Khả năng chứa danh sách các sự kiện miền phát sinh trong suốt quá trình thay đổi trạng thái của chính nó.
- **Lớp trừu tượng (Abstract Definition):** Không được phép khởi tạo trực tiếp Lớp cơ sở Entity, nó phải là abstract.

---

# 11. AggregateRoot Design Principles
- **Mở rộng của Entity (Entity Extension):** Aggregate Root về bản chất là một Entity đặc biệt, do đó nó phải kế thừa mọi đặc tính của Base Entity.
- **Điểm bảo vệ nhất quán (Consistency Boundary):** Đóng vai trò là gốc của một cụm (cluster) đối tượng. Bất kỳ sự thay đổi nào đối với các Entity con (nếu có) đều phải được định tuyến thông qua Aggregate Root.
- **Đánh dấu kiến trúc (Architectural Marker):** Lớp cơ sở này không cần phải chứa thêm nhiều logic so với Base Entity, nhưng nó đóng vai trò phân loại quan trọng ở cấp độ hệ thống để giới hạn việc tạo Repository (chỉ Repository cho AggregateRoot).

---

# 12. ValueObject Design Principles
- **Không có định danh (Identity-less):** Value Object được phân biệt hoàn toàn dựa trên giá trị của các thuộc tính mà nó chứa, không có khái niệm `Id`.
- **Bất biến (Immutability):** Khi đã khởi tạo, trạng thái của Value Object không thể bị thay đổi. Bất kỳ thao tác nào nhằm thay đổi giá trị đều phải trả về một phiên bản (instance) hoàn toàn mới.
- **So sánh giá trị sâu (Deep Value Equality):** Hai Value Object được coi là bằng nhau nếu tất cả các thuộc tính tương ứng của chúng bằng nhau. Việc so sánh này phải được hỗ trợ thông qua cơ chế trừu tượng hóa để lớp dẫn xuất (derived class) cung cấp các thành phần tham gia so sánh.
- **Lớp trừu tượng (Abstract Definition):** Lớp cơ sở ValueObject phải là abstract.

---

# 13. Identity Decision Matrix
So sánh các kiểu định danh cấp kiến trúc (Không quyết định thay đổi Code ở Sprint này):
- **Guid:** Đảm bảo duy nhất toàn cầu, lý tưởng cho phân tán (Distributed Systems) nhưng tốn dung lượng và làm phân mảnh Index trong DB.
- **Int:** Hiệu năng cao nhất, chiếm ít dung lượng, nhưng phụ thuộc vào DB Identity/Sequence sinh mã, không an toàn trong phân tán.
- **Long:** Hiệu năng cao, mở rộng tốt hơn Int, phù hợp hệ thống lớn nhưng vẫn phụ thuộc vào chiến lược cấp phát ID tập trung (như Snowflake).
- **Strongly Typed Id:** An toàn kiểu dữ liệu nhất (Type-safe), chống nhầm lẫn khi truyền tham số (như `UserId` vs `OrderId`), nhưng mất công thiết lập cơ chế chuyển đổi (Converters) ở tầng Infrastructure và API.

---

# 14. Identity Strategy
- Định danh của Entity nên được thiết kế dưới dạng Generic Type (Ví dụ: `TId`) để linh hoạt trong việc sử dụng kiểu dữ liệu đã phân tích ở Decision Matrix.
- Thuộc tính Id chỉ được phép gán giá trị ở thời điểm khởi tạo ban đầu và phải được bảo vệ chặt chẽ.

---

# 15. Equality Validation Rules
Quy tắc rà soát cơ chế so sánh:
- **Equals:** Phải được override ở `Entity` (chỉ so sánh ID) và `ValueObject` (so sánh toàn bộ thuộc tính).
- **GetHashCode:** Phải được override nhất quán với `Equals` (nếu Equals trả về true thì GetHashCode phải giống nhau).
- **Operator == / !=:** Phải được nạp chồng (overload) để hỗ trợ cú pháp so sánh chuẩn C#.
- **Reference Equality:** Vẫn phải được tính đến nếu hai biến trỏ về cùng một vùng nhớ.
- **Identity Equality:** Áp dụng bắt buộc cho `Entity`.
- **Value Equality:** Áp dụng bắt buộc cho `ValueObject`.

---

# 16. Equality Strategy
- **Entity Equality:** Phép so sánh bằng và mã băm chỉ dựa trên thuộc tính định danh (Identity). 
- **Value Object Equality:** Phép so sánh bằng và mã băm phải quét qua tất cả các thuộc tính cấu thành. 

---

# 17. Domain Event Lifecycle
Quy trình vòng đời của sự kiện miền:
- **Raise:** Sự kiện được tạo ra bên trong một phương thức nghiệp vụ của Entity.
- **Store:** Sự kiện được lưu vào danh sách ẩn bên trong Base Entity.
- **Collect:** UnitOfWork hoặc Dispatcher sẽ thu thập tất cả sự kiện trước khi lưu vào DB.
- **Dispatch (Application Layer):** Đẩy sự kiện cho các Handlers xử lý (nằm ngoài phạm vi Domain Layer).
- **Clear:** Làm sạch danh sách sự kiện trong Entity sau khi đã dispatch thành công.

---

# 18. Domain Event Collection Strategy
- **Lưu trữ cục bộ:** Base Entity phải chứa một danh sách (Collection) nội bộ, dùng để lưu trữ các Domain Event phát sinh.
- **Bảo mật danh sách:** Danh sách này chỉ có thể đọc từ bên ngoài (IReadOnlyCollection). Việc thêm sự kiện (Add) hoặc xóa sự kiện (Clear) phải được kiểm soát bằng các phương thức cụ thể bên trong Base Entity.

---

# 19. Constructor Validation Rules
- **Protected Constructor:** Bắt buộc phải có (thường là tham số rỗng) nhằm phục vụ riêng cho ORM khi map data từ DB lên Entity/ValueObject mà không phá ranh giới Đóng gói.
- **Public Constructor:** Hạn chế sử dụng nếu logic phức tạp. Nếu dùng, phải có tham số đầy đủ để đảm bảo Invariants ngay từ lúc khởi tạo.
- **Factory Constructor:** Ưu tiên sử dụng phương thức static (như `Create(...)`) thay cho public constructor để làm rõ ý nghĩa nghiệp vụ.
- **Invariant Enforcement:** Mọi constructor (public/factory) đều phải ném ngoại lệ (`DomainException`) nếu dữ liệu đầu vào không hợp lệ.

---

# 20. Constructor Strategy
- Bắt buộc sử dụng constructors có tham số để đảm bảo đối tượng luôn khởi tạo ở trạng thái hợp lệ.
- Bảo vệ triệt để tính Encapsulation, kể cả khi phải thỏa hiệp với công nghệ (ORM) bằng cách dùng mức truy cập `protected` thay vì `public`.

---

# 21. Encapsulation Rules
- **No Public Setters:** Cấm sử dụng public setters cho tất cả các thuộc tính trạng thái.
- **Protected/Private Init:** Việc gán giá trị thuộc tính chỉ được thực hiện thông qua constructors, phương thức nghiệp vụ, hoặc các từ khóa giới hạn truy cập (như `private set`, `protected set` hoặc `init`).
- **Bảo vệ Collections:** Mọi danh sách đối tượng bên trong Base Classes (như danh sách sự kiện) phải trả về kiểu chỉ đọc.

---

# 22. Inheritance Rules
- Các Base Classes (`Entity`, `ValueObject`, `AggregateRoot`) phải là `abstract`.
- Các mô hình nghiệp vụ khi kế thừa phải tuân thủ nghiêm ngặt các phương thức abstract được Base Class yêu cầu.

---

# 23. Generic Design Guidelines
- Khi sử dụng Generic (ví dụ: `Entity<TId>`), thiết kế phải đảm bảo ràng buộc (constraints) chặt chẽ, ngăn ngừa việc sử dụng kiểu định danh không hợp lệ.
- Ưu tiên tính đơn giản. Việc ra quyết định cụ thể về kiểu Generic sẽ diễn ra ở phase Coding.

---

# 24. AI Coding Constraints
- **CẤM** sinh mã C# dưới mọi hình thức trong tài liệu đặc tả.
- **CẤM** định nghĩa cụ thể một Base Class bằng mã giả (Pseudo Code).
- **CẤM** nhúng các Attributes phụ thuộc ORM (`[Key]`, `[Table]`) vào đặc tả Base Class.
- **CẤM** mô tả logic của Repository hoặc DbContext.
- **CẤM** sinh các lớp nghiệp vụ cụ thể.

---

# 25. Validation Rules
Thiết kế Lớp cơ sở được xem là hợp lệ nếu:
- Đảm bảo tính Persistence Ignorance.
- Định hình rõ ranh giới giữa Entity và Value Object.
- Hoàn toàn tuân thủ các quy định tại mục 24.

---

# 26. Definition of Done
Tiêu chí nghiệm thu (DoD) mở rộng:
- **Project Structure hoàn chỉnh:** Đã mô tả rõ ràng trách nhiệm của từng Base Class.
- **Namespace đúng:** Mọi thiết kế đều tuân thủ namespace đã chốt ở tài liệu 30.
- **Dependency sạch:** Base Classes đặc tả không phụ thuộc framework ngoài.
- **Zero Third-party Package:** Tuân thủ nguyên tắc thuần C#.
- **Architecture Ready:** Sẵn sàng làm đầu vào cho khâu triển khai mã nguồn cụ thể mà không cần hỏi lại cấu trúc.

---

# 27. Validation Matrix
Bảng ma trận kiểm tra tính tuân thủ:

| Tiêu chí kiểm tra | Đánh giá | Trạng thái / Khắc phục |
|---|---|---|
| **DDD Compliance** | Phân rõ Entity, VO, AggregateRoot. | Re-architect nếu nhập nhằng khái niệm. |
| **Encapsulation** | Cấm public setters, dùng Init/Private set. | Sửa lại chuẩn nếu phát hiện. |
| **Persistence Ignorance** | Không chứa DB Attributes hay ORM Logic. | Gỡ bỏ lập tức nếu dính EF Core/SQL. |
| **Event Collection** | Có hỗ trợ Add/Clear/ReadOnlyCollection. | Bổ sung Collection nếu thiếu. |
| **Equality** | Override Equals, GetHashCode, ==, !=. | Bắt buộc triển khai đầy đủ toán tử. |
| **Immutability** | ValueObject tuyệt đối không đổi trạng thái. | Hủy bỏ thay đổi trạng thái nếu có. |
| **Compile Ready** | (Sẽ kiểm tra ở Phase Coding) | Cần vượt qua Build test với No Warnings. |

---

# 28. Risks
- **Rủi ro rò rỉ mã nguồn:** Có khả năng AI vi phạm bằng cách dùng mã giả C# để giải thích các mẫu (Patterns).
  - **Giảm thiểu:** Nhấn mạnh 100% tài liệu chỉ mang tính Đặc tả Kiến trúc (Architecture Specification), cấm tuyệt đối C#.

---

# 29. AI Review Checklist
- [x] Đã hoàn toàn tuân thủ không sinh mã nguồn C# hay Pseudo Code?
- [x] Đã phân định rõ ranh giới giữa Entity và ValueObject?
- [x] Đã thiết lập các quy định Constructor, Encapsulation chi tiết?
- [x] Cấu trúc Lớp cơ sở đã phù hợp để triển khai DDD?
- [x] Đã tích hợp đầy đủ Identity Decision Matrix và Domain Event Lifecycle?
- [x] Đã hoàn thiện Equality Validation Rules và Constructor Validation Rules?
- [x] Đã tuân thủ nguyên tắc kế thừa từ các tài liệu số 25 đến 30?

---

# 30. References
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
- 28_SPRINT_02_REVIEW.md
- 29_SPRINT_03_IMPLEMENTATION_PLAN.md
- 30_SPRINT_03_PROJECT_INIT.md

---
# END OF SPECIFICATION
