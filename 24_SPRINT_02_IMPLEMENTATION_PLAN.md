# 24_SPRINT_02_IMPLEMENTATION_PLAN.md
## SPRINT 02 – DOMAIN ARCHITECTURE & MODELING
Version: 2.0.0
Status: Draft (Pending Review)
Project: AnSinhSo Enterprise
Last Updated: 2026-07-14

---

# 1. Objectives

Mục tiêu chính của **Sprint 02 – Domain Architecture & Modeling** là nghiên cứu, phân tích nghiệp vụ và xây dựng kiến trúc thiết kế Domain cốt lõi của dự án AnSinhSo Enterprise. 

Nhiệm vụ trọng tâm của Sprint này tập trung hoàn toàn vào việc thiết lập đặc tả thiết kế hệ thống theo phương pháp **Domain-Driven Design (DDD)** và nguyên tắc **Clean Architecture** dưới dạng tài liệu kỹ thuật chuẩn chỉnh, chuẩn bị đầy đủ cơ sở lý thuyết và bản vẽ thiết kế trước khi tiến hành viết mã nguồn ở các Sprint sau.

---

# 2. Scope

### In-Scope (Nằm trong phạm vi triển khai của Sprint 02)
- Phân tích và xây dựng các tài liệu đặc tả kiến trúc và thiết kế Domain, bao gồm:
  - Phân tích các khối xây dựng cơ bản (DDD Building Blocks) như `Entity`, `AggregateRoot`, `ValueObject`, `DomainEvent`.
  - Thiết lập hướng dẫn mô hình hóa dữ liệu và nghiệp vụ cho dự án AnSinhSo Enterprise.
  - Thiết kế cấu trúc các Aggregate cốt lõi (Hộ gia đình, Công dân, Trợ cấp và Chính sách an sinh).
- Xác định các quy tắc nghiệp vụ bất biến (Invariants) trên mặt lý thuyết và tài liệu thiết kế.
- Định nghĩa kiến trúc giao tiếp giữa các tầng và quy hoạch cấu trúc thư mục của dự án `AnSinhSo.Domain` trên tài liệu đặc tả.

### Out-of-Scope (Không triển khai trong Sprint 02)
- **Tuyệt đối không triển khai mã nguồn (No source code):** Không tạo bất kỳ file C# code nào cho Base classes, Entities, Value Objects, Domain Events hay Repository Interfaces trong dự án `AnSinhSo.Domain`.
- Không thiết lập cấu hình cơ sở dữ liệu, DbContext hay các config ánh xạ EF Core.
- Không xây dựng các API Controllers, Endpoints hay Business Services ở các project khác.

---

# 3. Deliverables

Sản phẩm bàn giao duy nhất của Sprint 02 là bộ tài liệu đặc tả thiết kế (Documentation Only):
1. **[NEW] [25_DOMAIN_ARCHITECTURE.md](file:///d:/AnSinhSo_Enterprise/25_DOMAIN_ARCHITECTURE.md):** Định nghĩa cấu trúc kiến trúc Domain Layer, cách thức phân chia các Bounded Context và nguyên tắc tương tác giữa các Layer.
2. **[NEW] [26_DOMAIN_MODEL_GUIDE.md](file:///d:/AnSinhSo_Enterprise/26_DOMAIN_MODEL_GUIDE.md):** Hướng dẫn mô hình hóa thực thể Domain, định nghĩa các Base Classes của DDD và tiêu chuẩn áp dụng cho dự án AnSinhSo.
3. **[NEW] [27_AGGREGATE_DESIGN.md](file:///d:/AnSinhSo_Enterprise/27_AGGREGATE_DESIGN.md):** Thiết kế chi tiết các Aggregate Roots, các Entities/Value Objects bên trong và danh sách các quy tắc bất biến (Invariants) của từng Aggregate.

---

# 4. Constraints

- **No Domain Design in Implementation Plan:** Không đưa ra bất kỳ quyết định thiết kế Domain cụ thể nào trực tiếp trong tài liệu `24_SPRINT_02_IMPLEMENTATION_PLAN.md` này. Mọi quyết định thiết kế phải được chuyển tiếp sang các tài liệu `25`, `26` và `27`.
- **Standards Compliance:** Thiết kế Domain phải tuân thủ nghiêm ngặt các quy định về kiểu dữ liệu trong [05_DATABASE_RULES.md](file:///d:/AnSinhSo_Enterprise/Sprint01_Package/05_DATABASE_RULES.md) và triết lý DDD quy định tại [08_BACKEND_STANDARDS.md](file:///d:/AnSinhSo_Enterprise/Sprint01_Package/08_BACKEND_STANDARDS.md).
- **Technology Independence:** Các tài liệu thiết kế không được đưa vào các khái niệm phụ thuộc công nghệ hoặc ORM cụ thể (như EF Core, SQL Server) mà chỉ tập trung vào ngôn ngữ nghiệp vụ chung (Ubiquitous Language).

---

# 5. Risks

| Rủi ro | Mức độ | Biện pháp giảm thiểu |
|--------|--------|----------------------|
| **Thiết kế không nhất quán với DB Rules** | Trung bình | Rà soát chéo các kiểu dữ liệu địa lý (GIS) và phân cấp (HierarchyID) trong tài liệu thiết kế Domain so với [05_DATABASE_RULES.md](file:///d:/AnSinhSo_Enterprise/Sprint01_Package/05_DATABASE_RULES.md). |
| **Thiết kế Domain bị phụ thuộc công nghệ** | Thấp | Cấm sử dụng các khái niệm Database hoặc framework ngoài trong tài liệu thiết kế. Chỉ tập trung vào quy tắc nghiệp vụ bất biến. |
| **Mơ hồ trong định nghĩa Aggregate** | Cao | Sử dụng sơ đồ trực quan và định nghĩa rõ ràng ranh giới (Aggregate Boundaries) trong tài liệu `27_AGGREGATE_DESIGN.md`. |

---

# 6. Dependency

- **Sprint 01 Completed:** Giải pháp và cấu trúc các project của giải pháp phải được khởi tạo đúng và build thành công.
- **FROZEN Standards:** Yêu cầu các tài liệu tiêu chuẩn phát triển (`04` đến `12`) đã được đóng băng và tham chiếu chính thức.

---

# 7. Review Strategy

Quy trình đánh giá và nghiệm thu tài liệu thiết kế:
1. **Rà soát kiến trúc (Architecture Review Session):** Solution Architecture Team thực hiện rà soát chéo giữa các tài liệu `25`, `26`, `27` để đảm bảo tính đồng bộ, nhất quán và khả năng mở rộng.
2. **Project Owner Review & Approval:** Project Owner trực tiếp đánh giá chất lượng tài liệu thiết kế về mặt nghiệp vụ, đảm bảo phản ánh chính xác nghiệp vụ thực tế của UBND xã Sông Lũy.
3. **Documentation Freeze:** Sau khi được PO phê duyệt, bộ tài liệu thiết kế Domain (`25`, `26`, `27`) sẽ được đóng băng để làm cơ sở cho việc triển khai code ở Sprint 03.

---

# 8. Acceptance Criteria

Sprint 02 được nghiệm thu khi đáp ứng đầy đủ các tiêu chí sau:

### Về Tài liệu thiết kế (Documentation Criteria)
- [x] Hoàn thành và bàn giao đầy đủ 3 tài liệu thiết kế: `25_DOMAIN_ARCHITECTURE.md`, `26_DOMAIN_MODEL_GUIDE.md`, `27_AGGREGATE_DESIGN.md`.
- [x] Các tài liệu được tổ chức rõ ràng theo đúng template quy định, có Metadata đầy đủ.
- [x] Không tồn tại các mâu thuẫn về mặt thuật ngữ nghiệp vụ (Ubiquitous Language) giữa các tài liệu.

### Về Kiến trúc và Nghiệp vụ (Architecture Review Criteria)
- [x] Sơ đồ phân chia Layer và tương tác giữa các Layer trong `25_DOMAIN_ARCHITECTURE.md` tuân thủ đúng nguyên lý Dependency Rule của Clean Architecture.
- [x] Các định nghĩa thực thể, Value Object trong `26` và `27` được tách biệt hoàn toàn khỏi công nghệ Persistance.
- [x] Xác định rõ ràng các ranh giới giao dịch (Transaction Boundaries) của từng Aggregate trong `27_AGGREGATE_DESIGN.md`.
- [x] Liệt kê đầy đủ các quy tắc bất biến nghiệp vụ (Invariants) của các Aggregate để phục vụ cho viết code kiểm thử và validation sau này.

---

# 9. Verification Plan

### Automated Verification
- Sử dụng các công cụ Markdown Lint để kiểm tra định dạng và tính hợp lệ của các file tài liệu thiết kế.
- Kiểm tra toàn bộ các liên kết chéo (Hyperlinks) trong tài liệu để đảm bảo không có liên kết hỏng.

### Manual Verification
- Solution Architecture Team thực hiện kiểm tra chéo các tài liệu thiết kế để đảm bảo không chứa mã nguồn giả hay các quyết định phụ thuộc công nghệ.
- Phiên họp rà soát kiến trúc (Architecture Review) chính thức giữa Team và Project Owner được thông qua.

---
# END OF PLAN
