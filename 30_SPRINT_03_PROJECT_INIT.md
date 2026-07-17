# 30_SPRINT_03_PROJECT_INIT.md
## SPRINT 03 – DOMAIN PROJECT INITIALIZATION
Version: 1.1.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 30_SPRINT_03_PROJECT_INIT
- **Title:** Domain Project Initialization Specification
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Software Architect, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft |
| 1.1.0 | 2026-07-17 | Solution Architecture Team | Added Architecture Freeze Rule, Project Creation Order, Namespace Validation Rules, Folder Evolution Strategy, Definition of Done, Validation Matrix, AI Constraints |

---

# 3. Architecture Freeze Rule
Quy tắc đóng băng kiến trúc (Freeze Rule) cho quá trình khởi tạo dự án:
- Sau khi tài liệu này được Project Owner phê duyệt (Freeze), **KHÔNG ĐƯỢC** thay đổi Project Structure.
- KHÔNG ĐƯỢC thay đổi Namespace Strategy.
- KHÔNG ĐƯỢC thay đổi Dependency Constraints.
- Mọi thay đổi phát sinh bắt buộc phải thông qua một Revision mới của tài liệu và phải được phê duyệt lại.

---

# 4. Purpose
Tài liệu này đóng vai trò là đặc tả (Architecture Specification) định hướng việc khởi tạo dự án vật lý `AnSinhSo.Domain` dưới dạng Class Library. Mục tiêu là thiết lập các quy tắc nền tảng về cấu trúc, định danh (Namespace) và các ràng buộc biên dịch (Build Configuration) cho thư mục Domain trước khi bất kỳ mã nguồn chức năng nào được viết.

---

# 5. Scope
**Trong phạm vi (In-Scope):**
- Định nghĩa chiến lược khởi tạo dự án `AnSinhSo.Domain`.
- Thiết lập quy tắc phân cấp thư mục.
- Cấu hình nguyên tắc chuẩn cho biên dịch (.NET 8).
- Định nghĩa các ràng buộc về Dependency và Namespace.

**Ngoài phạm vi (Out of Scope):**
- Không sinh mã nguồn của tệp `.csproj` (chỉ đưa ra nguyên tắc cấu hình).
- Không mô tả chi tiết thiết kế của các Base Classes (Entity, ValueObject, AggregateRoot).
- Không mô tả chi tiết thiết kế các Interfaces (IRepository, IUnitOfWork).
- Không mô tả chi tiết Domain Exceptions hay Domain Events.
- Không mô tả bất kỳ Entity hay Aggregate nghiệp vụ nào cụ thể.

---

# 6. Objectives
- Thiết lập thành công nền móng thư mục và không gian tên (Namespace) cho tầng Domain.
- Đảm bảo 100% ranh giới kiến trúc độc lập (Persistence Ignorance) được bảo vệ ngay từ pha khởi tạo.
- Tạo sự đồng thuận tuyệt đối trong AI và đội ngũ về điểm bắt đầu của mã nguồn.

---

# 7. Project Initialization Principles
- **Nguyên tắc "Thuần túy" (Purity Principle):** Dự án bắt buộc phải là một Class Library thuần túy, không chứa mã khởi chạy (No Entry Point), không giao tiếp I/O, không gọi cơ sở dữ liệu.
- **Nguyên tắc "Định hướng cắt dọc" (Vertical Slice Alignment):** Cấu trúc thư mục ngay từ đầu phải dự phòng cho việc phát triển theo chiều dọc thông qua các thư mục `Modules` phân tách độc lập các Bounded Context.
- **Nguyên tắc "Nghiêm ngặt" (Strictness):** Sử dụng các tính năng mới của C# 12 / .NET 8 để ép buộc tính an toàn kiểu dữ liệu (Type Safety) và nguyên tắc thiết kế ngay từ file cấu hình.

---

# 8. Project Creation Order
Trình tự thực thi khởi tạo dự án được chuẩn hóa như sau:
- **Step 1:** Create Project (Tạo .NET Class Library thuần túy).
- **Step 2:** Create Folder Structure (Tạo cây thư mục chuẩn).
- **Step 3:** Configure Namespace Strategy (Khai báo các định dạng chuẩn cho namespace).
- **Step 4:** Apply Build Principles (Khai báo ràng buộc Nullable, TreatWarningsAsErrors).
- **Step 5:** Validate Architecture (Kiểm tra Dependencies).
- **Step 6:** Freeze (Đóng băng cấu trúc dự án).
- **Step 7:** Coding (Chỉ bắt đầu code khi các bước trên đã hoàn tất).

---

# 9. Project Structure Overview
Cấu trúc tổ chức thư mục nội bộ tuân thủ thiết kế từ `25_DOMAIN_ARCHITECTURE.md`:
- **Common:** Lớp chia sẻ chứa các Building Blocks nền tảng nhất dùng chung cho mọi Bounded Context.
- **Exceptions:** Khu vực định nghĩa các lỗi nghiệp vụ cốt lõi.
- **Modules:** Không gian phân rã dành cho các Bounded Context (Demographic, SocialSecurity, Disbursement, v.v.).

---

# 10. Folder Initialization Strategy
Chiến lược khởi tạo thư mục vật lý (chỉ rỗng, chưa sinh tệp mã nguồn):
1. Thư mục gốc `AnSinhSo.Domain`.
2. Khởi tạo thư mục `Common/`
   - Khởi tạo `Common/Models/`
   - Khởi tạo `Common/Interfaces/`
   - Khởi tạo `Common/Events/`
3. Khởi tạo thư mục `Exceptions/`.
4. Khởi tạo thư mục `Modules/` với các thư mục con dự phòng:
   - `Modules/Demographic/`
   - `Modules/SocialSecurity/`
   - `Modules/Disbursement/`
   - `Modules/GIS/`
   - `Modules/Communication/`
   - `Modules/Analytics/`

---

# 11. Folder Evolution Strategy
Lộ trình tiến hóa của cấu trúc thư mục qua các Sprint:
- **Sprint 03:** Folders chỉ khởi tạo, nằm dưới dạng cấu trúc rỗng hoặc chứa Base Classes.
- **Sprint 04:** Modules bắt đầu chứa Aggregate và các Entity nghiệp vụ thực tế.
- **Sprint 05:** Infrastructure Mapping (Thư mục được tham chiếu từ tầng hạ tầng để ánh xạ EF Core).
- **Sprint 06:** API Exposure (Thư mục được tham chiếu từ tầng Application/API để đóng gói DTO/Endpoints).

---

# 12. Namespace Strategy
Chiến lược định danh (Namespace) bắt buộc cho tất cả mã nguồn tương lai:
- Root namespace phải luôn luôn là: `AnSinhSo.Domain`.
- Namespace phải mô phỏng chính xác cấu trúc thư mục vật lý (Folder-Namespace Matching 1-1).
- **Ví dụ đúng:**
  - `AnSinhSo.Domain.Common.Models`
  - `AnSinhSo.Domain.Modules.SocialSecurity`
- Không sử dụng tên viết tắt hay lược bỏ cấp thư mục trong khai báo namespace.

---

# 13. Namespace Validation Rules
Các quy tắc kiểm tra tính hợp lệ của Namespace:
- Namespace phải trùng với Folder vật lý 100%.
- KHÔNG cho phép sử dụng namespace rút gọn (ví dụ: `AnSinhSo.Models` thay vì `AnSinhSo.Domain.Common.Models`).
- KHÔNG cho phép namespace chéo (đặt file ở `Modules` nhưng khai báo namespace thuộc `Common`).
- KHÔNG cho phép file định nghĩa partial namespace sai cấu trúc.

---

# 14. Dependency Constraints
Ràng buộc phụ thuộc ở mức kiến trúc vật lý:
- **Dự án cô lập (Isolated Project):** `AnSinhSo.Domain` không được Reference tới bất kỳ Project nào khác trong solution AnSinhSo. Nó phải là đáy của đồ thị phụ thuộc (Dependency Graph).
- **Nuget Packages:** Zero Third-Party Packages.
  - Cấm cấu hình tải: `Microsoft.EntityFrameworkCore`
  - Cấm cấu hình tải: `Dapper`
  - Cấm cấu hình tải: `MediatR`
  - Cấm cấu hình tải: `Newtonsoft.Json` (nếu cần serialize nội bộ, dùng `System.Text.Json` hoặc đẩy việc này lên tầng cao hơn).

---

# 15. Build Configuration Principles
Các quy tắc cấu hình biên dịch (dự kiến áp dụng vào `.csproj` sau này):
- **Target Framework:** `net8.0`
- **Nullable Context:** `<Nullable>enable</Nullable>` (Bắt buộc để bảo vệ trạng thái Invariants không bị null đột ngột).
- **Implicit Usings:** `<ImplicitUsings>enable</ImplicitUsings>` (Giúp mã nguồn sạch sẽ, tránh khai báo thừa).
- **Treat Warnings As Errors:** `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` (Khuyến nghị, đặc biệt đối với Nullable warnings, nhằm buộc lập trình viên xử lý dứt điểm rủi ro tham chiếu null).

---

# 16. Coding Standards
Quy chuẩn mã hóa được nhắc lại để AI áp dụng ở các bước tiếp theo:
- **Đóng gói (Encapsulation):** Không phơi bày (expose) public setters. Trạng thái chỉ thay đổi qua methods hoặc constructor/init.
- **Tài liệu hóa (XML Docs):** Mọi Interface và Base Class bắt buộc phải có `/// <summary>` mô tả rõ trách nhiệm.

---

# 17. AI Constraints
Các giới hạn bắt buộc đối với AI Coding Agent trong việc xử lý Init:
- KHÔNG ĐƯỢC tạo Aggregate.
- KHÔNG ĐƯỢC tạo Entity nghiệp vụ.
- KHÔNG ĐƯỢC tạo DbContext.
- KHÔNG ĐƯỢC tạo Migration.
- KHÔNG ĐƯỢC tạo Repository Implementation.
- KHÔNG ĐƯỢC tạo EF Mapping.

---

# 18. Project Validation Rules
Một cấu trúc Project Init được xem là đạt chuẩn nếu:
- Toàn bộ cây thư mục được khởi tạo chính xác.
- File dự án nếu sinh ra không chứa bất kỳ dòng `<PackageReference>` sai quy định.
- Quá trình biên dịch dự án rỗng không phát sinh bất kỳ lỗi hay cảnh báo nào.

---

# 19. Definition of Done
Dự án được đánh giá là hoàn thành khởi tạo (DoD) khi đạt các tiêu chuẩn:
- **Project Structure hoàn chỉnh:** 100% các thư mục đã được tạo ra.
- **Namespace đúng:** Mọi file cấu trúc ban đầu đều có Namespace khớp với đường dẫn.
- **Dependency sạch:** Không trỏ tới bất kỳ project nào.
- **Zero Third-party Package:** `.csproj` hoàn toàn không có `<PackageReference>`.
- **Compile Ready:** `dotnet build` trả về 0 Errors, 0 Warnings.
- **Architecture Ready:** Sẵn sàng cho việc tạo Base Classes trong bước tiếp theo.

---

# 20. Project Validation Matrix
Bảng kiểm tra tính tuân thủ tổng thể:

| Hạng mục | Quy tắc kiểm tra | Trạng thái / Hành động |
|---|---|---|
| **Architecture** | Không có Entry Point, thuần Class Library | Xóa/Tạo lại dự án nếu phát hiện Executable |
| **Namespace** | 100% khớp với đường dẫn vật lý | AI phải tự động rename namespace sai lệch |
| **Dependency** | 0 Third-party, 0 Project Reference | Xóa ngay lập tức `<PackageReference>` và `<ProjectReference>` |
| **Folder** | Đầy đủ `Common`, `Exceptions`, `Modules` | Bổ sung thư mục nếu thiếu |
| **Build** | `net8.0`, `Nullable=enable` | Thêm flag nếu thiếu, sửa warning thành error |
| **DDD Compliance**| Không có thành phần của Data Layer / UI | Re-architect ngay lập tức |

---

# 21. Risks
- **Lạm dụng thư viện tiện ích:** Một số AI hoặc lập trình viên quen thói quen tự động cài các gói tiện ích (như AutoMapper, FluentValidation) vào Domain Layer.
- **Khắc phục:** Sự hiện diện của danh sách cấm tại mục 14 và Validation Rules sẽ buộc hệ thống phải từ chối sinh mã/hủy bỏ commit nếu phát hiện vi phạm.

---

# 22. AI Review Checklist
AI tự đánh giá sau khi hoàn thành tài liệu này:
- [x] Đã từ chối sinh nội dung mã C# cụ thể?
- [x] Đã từ chối sinh nội dung XML cụ thể cho `.csproj`?
- [x] Đã từ chối định nghĩa chi tiết các Building Blocks (Entity, ValueObject, v.v.)?
- [x] Đã liệt kê chi tiết chiến lược thư mục và Namespace?
- [x] Đã tích hợp các quy tắc từ `29_SPRINT_03_IMPLEMENTATION_PLAN.md`?
- [x] Đã tuân thủ AI Constraints (Không Entity, DbContext...)?
- [x] Đã hoàn thiện Architecture Freeze Rule và Validation Matrix?

---

# 23. References
Tài liệu kế thừa bắt buộc:
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN.md
- 28_SPRINT_02_REVIEW.md
- 29_SPRINT_03_IMPLEMENTATION_PLAN.md

---
# END OF SPECIFICATION
