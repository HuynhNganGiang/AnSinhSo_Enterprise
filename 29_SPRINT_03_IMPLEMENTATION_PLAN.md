# 29_SPRINT_03_IMPLEMENTATION_PLAN.md
## SPRINT 03 – DOMAIN IMPLEMENTATION PLANNING
Version: 1.2.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Document Metadata
- **Document ID:** 29_SPRINT_03_IMPLEMENTATION_PLAN
- **Title:** Sprint 03 Implementation Plan
- **Phase:** Sprint 03
- **Owner:** Solution Architecture Team
- **Audience:** AI Coding Agent, Software Architect, Backend Developer

---

# 2. Version History
| Version | Date | Author | Description |
|---|---|---|---|
| 1.0.0 | 2026-07-17 | Solution Architecture Team | Initial Draft |
| 1.1.0 | 2026-07-17 | Solution Architecture Team | Added Sprint Dependencies, AI Constraints, Execution Order, Definition of Done, AI Review Matrix |
| 1.2.0 | 2026-07-17 | Solution Architecture Team | Adjusted Execution Order, Added Architecture Freeze Rule, Final Consistency Validation |

---

# 3. Architecture Freeze Rule
Quy tắc đóng băng kiến trúc (Freeze Rule) cho Sprint 03:
- Sau khi Project Owner phê duyệt tài liệu, toàn bộ kế hoạch Sprint 03 được xem là **Freeze** (Đóng băng).
- KHÔNG thay đổi Phạm vi công việc (Scope).
- KHÔNG thay đổi Cấu trúc thư mục (Folder Structure).
- KHÔNG thay đổi Ràng buộc phụ thuộc (Dependency Rules).
- KHÔNG bổ sung Sản phẩm bàn giao (Deliverables) mới.
- KHÔNG mở rộng thời gian hoặc quy mô Sprint.
- Mọi thay đổi phát sinh sau khi Freeze bắt buộc phải thực hiện thông qua việc tạo một Revision mới của tài liệu và được phê duyệt lại từ đầu.

---

# 4. Sprint Dependencies
Các tài liệu liên kết bắt buộc phải tuân thủ trong Sprint 03 (được thừa kế từ Sprint 02):
- **25_DOMAIN_ARCHITECTURE.md**: Định nghĩa ranh giới và cấu trúc tổng thể.
- **26_DOMAIN_MODEL_GUIDE.md**: Hướng dẫn nguyên tắc mô hình hóa.
- **27_AGGREGATE_DESIGN.md**: Định nghĩa thiết kế Aggregate.
- **28_SPRINT_02_REVIEW.md**: Tổng kết Sprint 02 và các bài học kinh nghiệm.

**Quy tắc ưu tiên (Priority Rule):** Trong trường hợp có sự mâu thuẫn giữa kế hoạch triển khai của Sprint 03 và các tài liệu kiến trúc của Sprint 02, tài liệu kiến trúc (25, 26, 27) luôn có quyền ưu tiên cao hơn.

---

# 5. Sprint Goal
Mục tiêu cốt lõi của Sprint 03 là chuyển đổi các nguyên tắc lý thuyết đã được phê duyệt trong Sprint 02 thành cấu trúc mã nguồn (Source Code) nền tảng cho dự án `AnSinhSo.Domain`. Sprint này sẽ xây dựng bộ khung (Seed Work) bao gồm các Base Classes, Interfaces dùng chung, hệ thống Exceptions và các cấu trúc Domain Event gốc, tạo tiền đề vững chắc để mô hình hóa các Aggregate cụ thể trong các giai đoạn tiếp theo.

---

# 6. Scope
**Phạm vi công việc (In-Scope):**
- Khởi tạo dự án `AnSinhSo.Domain` (dưới dạng Class Library thuần túy).
- Xây dựng cấu trúc thư mục chuẩn (Folder Structure) theo định hướng chia nhỏ Bounded Context.
- Phát triển các Lớp cơ sở (Base Classes) cho mô hình DDD: `Entity`, `AggregateRoot`, `ValueObject`.
- Phát triển các giao diện lõi (Interfaces): `IAggregateRoot`, `IRepository`, `IUnitOfWork`.
- Thiết lập cơ sở cho hệ thống sự kiện miền (Domain Events): Giao diện `IDomainEvent`.
- Thiết lập cấu trúc ngoại lệ nghiệp vụ (Domain Exceptions).

---

# 7. Out of Scope
**Ngoài phạm vi (Không thực hiện trong Sprint 03):**
- Tuyệt đối không sinh mã nguồn cho bất kỳ Aggregate, Entity, hay Value Object nghiệp vụ cụ thể nào (ví dụ: Không tạo bảng `Household`, `Citizen`, `Policy`).
- Không tạo các Repository thực thi (Repository Implementation) giao tiếp với Entity Framework Core hoặc cơ sở dữ liệu vật lý.
- Không tạo DbContext, Migrations hay bất kỳ mã SQL nào.
- Không tạo tầng Application (CQRS Handlers) hay Web API (Controllers).
- Không tạo DTOs, ViewModels hay cấu hình AutoMapper.

---

# 8. Deliverables
Sản phẩm dự kiến bàn giao sau Sprint:
- Toàn bộ cấu trúc mã nguồn nền tảng (Seed Work) của tầng Domain.
- Cấu trúc thư mục được thiết lập hoàn chỉnh và sẵn sàng để lập trình viên sử dụng.
- Các Interfaces và Base Classes đã được kiểm tra tính hợp lệ về mặt biên dịch (Compile-ready).

---

# 9. Coding Standards
- Mọi mã nguồn phải tuân thủ chuẩn C# 12 / .NET 8.
- Áp dụng triệt để tính năng Nullable Reference Types (`<Nullable>enable</Nullable>`).
- Không sử dụng public setters cho các thuộc tính trạng thái bên trong Base Classes.
- Mã nguồn phải thuần túy hướng đối tượng, tập trung vào đóng gói (Encapsulation).
- Lớp và phương thức phải được Documented (Summary XML) rõ ràng.

---

# 10. Dependency Rules
Ràng buộc phụ thuộc nghiêm ngặt dành cho dự án `AnSinhSo.Domain`:
- **Chỉ được phép:** Tham chiếu tới các thư viện cốt lõi của .NET.
- **Cấm tuyệt đối:** Không thêm package `Microsoft.EntityFrameworkCore`, `Microsoft.AspNetCore.*`, `Dapper`, `System.Data.SqlClient` hoặc bất kỳ ORM/Web framework nào khác. Tuân thủ 100% Persistence Ignorance.

---

# 11. AI Coding Constraints
Danh sách các giới hạn (cấm đoán) bắt buộc đối với AI Coding Agent khi triển khai Sprint 03:
- **CẤM SINH DB CONTEXT:** Bất kỳ file nào có chứa DbContext hoặc `OnModelCreating`.
- **CẤM SINH DB MIGRATION:** Bất kỳ file nào kế thừa `Migration`.
- **CẤM SINH SQL QUERIES:** Bất kỳ câu lệnh SQL thuần túy hoặc LINQ to Entities nào nhắm vào Database.
- **CẤM SINH BUSINESS ENTITIES:** Bất kỳ class nào đại diện cho nghiệp vụ (như `User`, `Account`, `Household`). Chỉ sinh Base Classes.
- **CẤM GỌI API BÊN NGOÀI:** Không chèn mã gọi API, HTTPClient.
- **CẤM THÊM ATTRIBUTES HẠ TẦNG:** Không sử dụng các DataAnnotations như `[Table]`, `[Column]`, `[Required]` vào trong Domain.

---

# 12. Sprint Execution Order
Lộ trình thực thi chi tiết cho AI (các file sinh ra phải theo đúng tuần tự):
1. **30_SPRINT_03_PROJECT_INIT.md** (Khởi tạo `AnSinhSo.Domain.csproj` và thư mục).
2. **31_SPRINT_03_BASE_CLASSES.md** (Sinh `Entity.cs`, `AggregateRoot.cs`, `ValueObject.cs`).
3. **32_SPRINT_03_EXCEPTIONS.md** (Sinh `DomainException.cs`).
4. **33_SPRINT_03_EVENTS.md** (Sinh `IDomainEvent.cs`, `DomainEventDispatcher` logic nếu có ở cấp Base).
5. **34_SPRINT_03_INTERFACES.md** (Sinh `IAggregateRoot.cs`, `IRepository.cs`, `IUnitOfWork.cs`).
6. **35_SPRINT_03_REVIEW.md** (Đánh giá hoàn thành Sprint).
7. **Freeze Sprint 03** (Đóng băng tài liệu theo quy trình Architecture Freeze Rule).
8. **Coding** (Thực thi sinh mã nguồn vật lý vào giải pháp dựa trên tài liệu đã Freeze).

---

# 13. Folder Structure
Cấu trúc vật lý của dự án `AnSinhSo.Domain` sẽ được tổ chức như sau:
```text
AnSinhSo.Domain/
├── Common/
│   ├── Models/         (Base Classes: Entity, AggregateRoot, ValueObject)
│   ├── Interfaces/     (IAggregateRoot, IRepository, IUnitOfWork)
│   └── Events/         (IDomainEvent)
├── Exceptions/         (DomainException)
├── Modules/            (Sẽ trống trong Sprint 03, chuẩn bị cho Bounded Contexts)
│   ├── Demographic/
│   ├── SocialSecurity/
│   ├── Disbursement/
│   ├── GIS/
│   ├── Communication/
│   └── Analytics/
└── AnSinhSo.Domain.csproj
```

---

# 14. Base Class Roadmap
Lộ trình thiết kế mã cho các Lớp cơ sở (Seed Work Models):
- **`Entity`**: Là lớp trừu tượng (abstract class) chứa thuộc tính định danh `Id`, cơ chế so sánh bằng (Equality) dựa trên `Id`, và danh sách các Domain Events nội bộ chưa được publish.
- **`AggregateRoot`**: Kế thừa `Entity`, đóng vai trò là điểm giao tiếp duy nhất (Root) cho một cụm thực thể.
- **`ValueObject`**: Lớp trừu tượng (abstract class) cung cấp cơ chế so sánh giá trị (Value Equality) cho tất cả các thuộc tính bên trong nó.

---

# 15. Interface Roadmap
Lộ trình thiết kế mã cho các Giao diện lõi:
- **`IAggregateRoot`**: Marker interface (interface trống) để đánh dấu các Aggregate Root.
- **`IRepository<T>`**: Generic interface định nghĩa hợp đồng trừu tượng. Bắt buộc có Generic Constraint `where T : IAggregateRoot` để ngăn chặn việc tạo Repository cho các Entity con.
- **`IUnitOfWork`**: Interface định nghĩa ranh giới lưu trữ giao dịch chung (chứa phương thức `CommitAsync` hoặc `SaveChangesAsync`).

---

# 16. Domain Event Roadmap
- **`IDomainEvent`**: Marker interface cho tất cả các sự kiện. Thường chứa một thuộc tính bắt buộc `OccurredOn` (thời điểm xảy ra sự kiện).
- **Event Collection:** Hệ thống cần hỗ trợ lưu trữ Domain Event tạm thời bên trong `Entity` cơ sở thông qua các phương thức `AddDomainEvent(IDomainEvent eventItem)` và `ClearDomainEvents()`. Việc Dispatch sự kiện sẽ được xử lý ở tầng Infrastructure hoặc Application sau này.

---

# 17. Exception Roadmap
- **`DomainException`**: Kế thừa trực tiếp từ `System.Exception`, đóng vai trò là Exception gốc của mọi ngoại lệ phát sinh khi Invariants nghiệp vụ bị vi phạm.
- Các ngoại lệ này chỉ chứa thông điệp nghiệp vụ (Business Message), tuyệt đối không chứa mã lỗi HTTP (như 400, 404, 500) để đảm bảo không rò rỉ kiến trúc Web xuống tầng Domain.

---

# 18. Repository Interface Roadmap
- Cung cấp các thao tác nền tảng ở dạng bất đồng bộ: `GetByIdAsync`, `AddAsync`, `Update`, `Delete`.
- `IRepository` tuyệt đối không cung cấp các phương thức `Save` hoặc `Commit`. Trách nhiệm ghi dữ liệu xuống CSDL phải được giao phó toàn bộ cho `IUnitOfWork`.

---

# 19. Definition of Done
Tiêu chí hoàn thành (DoD) của Sprint 03:
- [ ] Dự án `AnSinhSo.Domain` tồn tại và Build thành công, không có lỗi (Zero Warnings/Errors).
- [ ] Tệp `.csproj` hoàn toàn sạch sẽ, vô cảm trước hạ tầng (Persistence Ignorance).
- [ ] Toàn bộ Seed Work Classes và Interfaces đã hiện diện đúng theo cấu trúc thư mục quy định.
- [ ] Không có một mã nghiệp vụ cụ thể nào (Aggregate, API, EF Core) bị đưa vào mã nguồn một cách vô ý.
- [ ] Cấu trúc Base classes phải đảm bảo đúng nguyên tắc Đóng gói (Encapsulation).
- [ ] Dự án vượt qua toàn bộ quy tắc kiểm tra trong AI Review Matrix.

---

# 20. AI Review Matrix
Bảng kiểm tra tự động dành cho AI đánh giá sự tuân thủ kiến trúc của mã nguồn sinh ra:

| Tiêu chí | Nội dung kiểm tra | Đạt / Không đạt | Hành động nếu Không Đạt |
|---|---|---|---|
| **DDD Compliance** | Các khái niệm (Entity, VO, AggregateRoot) được thể hiện bằng Base Classes / Interfaces chuẩn xác. | | Hủy bỏ code, sinh lại theo đúng Pattern. |
| **SOLID Compliance** | Các interface (`IRepository`, `IUnitOfWork`) đảm bảo Single Responsibility. | | Rà soát và tách Interface. |
| **Persistence Ignorance** | Domain Model không chứa `[Table]`, `[Key]`, cấu trúc của ORM. | | Xóa các Attributes vi phạm ngay lập tức. |
| **Compile Ready** | Mã nguồn không có lỗi cú pháp, tương thích chuẩn C# 12 / .NET 8. | | Sửa lỗi cú pháp, sử dụng Nullable types. |
| **No Infrastructure** | Tệp `.csproj` không chứa thư viện I/O, DB, Network. | | Gỡ bỏ `<PackageReference>` vi phạm. |
| **No Business Entity** | Không sinh ra thực thể nghiệp vụ cụ thể nào của bài toán AnSinhSo. | | Xóa file entity vừa sinh ra. |

---

# 21. Review Checklist
Checklist dành cho AI và Tech Lead khi rà soát mã nguồn sinh ra chi tiết:
- [ ] Base `Entity` đã cài đặt chuẩn cơ chế so sánh `Equals` và `GetHashCode` dựa trên `Id` chưa?
- [ ] Base `ValueObject` có xử lý đệ quy so sánh giá trị qua `GetEqualityComponents()` chưa?
- [ ] Base `Entity` có hỗ trợ lưu trữ danh sách các Domain Events không?
- [ ] `IRepository<T>` có bị giới hạn bằng constraint `where T : IAggregateRoot` không?
- [ ] Tầng Domain có hoàn toàn cách ly với thư viện ngoại vi không?

---

# 22. Risks
- **Rủi ro vi phạm kiến trúc (Leaky Abstractions):** AI hoặc Lập trình viên có xu hướng reference gói `MediatR` trực tiếp vào Domain layer để cài đặt `INotification` cho Domain Events, làm rò rỉ Application Layer xuống Domain.
  - **Giảm thiểu:** Giải quyết bằng cách định nghĩa `IDomainEvent` độc lập và thuần túy, việc gắn kết (mapping) với `INotification` của MediatR sẽ diễn ra tại Application Layer hoặc sử dụng `MediatR.Contracts` cực mỏng nếu bắt buộc.

---
# END OF PLAN
