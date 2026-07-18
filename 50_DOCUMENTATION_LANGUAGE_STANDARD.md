# 50_DOCUMENTATION_LANGUAGE_STANDARD.md

## DEVELOPMENT CONSTITUTION

**Version:** 1.0.0
**Status:** APPROVED
**Project:** AnSinhSo Enterprise
**Document Type:** Development Constitution

---

## Document Metadata

| Item | Value |
|------|-------|
| Document Name | 50_DOCUMENTATION_LANGUAGE_STANDARD.md |
| Document Type | Development Constitution |
| Project | AnSinhSo Enterprise |
| Version | 1.0.0 |
| Status | APPROVED |
| Owner | Architecture Board |
| Author | Huỳnh Ngân Giang |
| Created Date | 2026-07-18 |
| Last Updated | 2026-07-18 |
| Next Review | Only when Architecture changes |

---

## Revision History

| Version | Date | Author | Description |
|----------|------------|-----------------|------------------------------|
| 1.0.0 | 2026-07-18 | Huỳnh Ngân Giang | Initial Development Constitution |

---

## Related Documents

Tài liệu này có quan hệ trực tiếp với các tài liệu sau:

### Project Foundation

- 00_PROJECT_INDEX.md
- 00_PROJECT_BOOTSTRAP.md
- 14_PROJECT_STRUCTURE.md

### Sprint 02

- 24_SPRINT_02_IMPLEMENTATION_PLAN.md
- 25_DOMAIN_ARCHITECTURE.md
- 26_DOMAIN_MODEL_GUIDE.md
- 27_AGGREGATE_DESIGN_GUIDE.md
- 28_SPRINT_02_REVIEW.md

### Sprint 03

- 29_SPRINT_03_IMPLEMENTATION_PLAN.md
- 30_SPRINT_03_PROJECT_INIT.md
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
- 43_SPRINT_03_DOMAIN_NOTIFICATIONS.md
- 44_SPRINT_03_ARCHITECTURE_REVIEW.md
- 45_SPRINT_03_IMPLEMENTATION_GUIDELINES.md
- 46_SPRINT_03_AI_IMPLEMENTATION_RULEBOOK.md
- 47_SPRINT_03_DEFINITION_OF_READY.md
- 48_SPRINT_03_DEFINITION_OF_DONE.md
- 49_SPRINT_03_RETROSPECTIVE.md

---

> **WARNING**: ĐÂY LÀ HIẾN PHÁP PHÁT TRIỂN (DEVELOPMENT CONSTITUTION) CỦA TOÀN BỘ DỰ ÁN ANSINHSO ENTERPRISE. KHÔNG AI ĐƯỢC PHÉP THAY ĐỔI, VI PHẠM HOẶC BỎ QUA CÁC QUY TẮC TRONG TÀI LIỆU NÀY. AI CODING AGENT VÀ DEVELOPER PHẢI TUÂN THỦ TUYỆT ĐỐI.

## 1. Purpose

Tài liệu này được thiết lập nhằm mục đích chuẩn hóa toàn bộ ngôn ngữ giao tiếp, quy ước đặt tên, cấu trúc thư mục, quy tắc kiến trúc và phong cách lập trình cho dự án AnSinhSo Enterprise. Đây là bản Hiến pháp tối cao đóng băng toàn bộ các quyết định về kiến trúc và quy trình từ Sprint 01 đến Sprint 03. Việc ban hành tài liệu này nhằm đảm bảo tính nhất quán tuyệt đối, loại bỏ sự mơ hồ trong giao tiếp giữa con người và AI Coding Agent, đồng thời ngăn chặn Technical Debt trước khi giai đoạn Development (từ Sprint 04) chính thức bắt đầu.

---

## 2. Scope

Phạm vi áp dụng của tài liệu này bao trùm toàn bộ hệ sinh thái phát triển của dự án AnSinhSo Enterprise, cụ thể bắt buộc đối với:
- **Developer:** Kỹ sư phần mềm tham gia viết, sửa, hoặc đánh giá mã nguồn.
- **AI Coding Agent:** Bất kỳ công cụ AI nào (Copilot, Gemini, ChatGPT, Cursor...) tham gia sinh mã nguồn hoặc review mã.
- **Code Reviewer:** Những cá nhân hoặc hệ thống tự động chịu trách nhiệm kiểm duyệt Pull Request.
- **QA:** Đội ngũ đảm bảo chất lượng, sử dụng tài liệu này làm cơ sở kiểm thử tiêu chuẩn.
- **Architecture Board:** Hội đồng Kiến trúc sử dụng làm thước đo để giám sát tính tuân thủ.

---

## 3. Language Standard

Quy chuẩn ngôn ngữ của AnSinhSo Enterprise được quy định như sau:
- **Ngôn ngữ diễn giải chính:** Tiếng Việt.
- **Thuật ngữ kỹ thuật:** Phải giữ nguyên 100% Tiếng Anh, tuyệt đối không được dịch sang Tiếng Việt.
- **Tên Design Pattern:** Giữ nguyên (VD: Repository Pattern, Factory Pattern, Result Pattern, Unit of Work...).
- **Tên Framework & Library:** Giữ nguyên (VD: ASP.NET Core, Entity Framework Core, MediatR, FluentValidation, Serilog...).
- **Cấu trúc kỹ thuật:** Giữ nguyên tên Namespace, Project, Folder, Interface, Class, Method.
- **Nguyên lý kiến trúc:** Giữ nguyên Clean Architecture, Domain-Driven Design (DDD), CQRS, SOLID.

---

## 4. Naming Convention

### Why
Tại sao Naming Convention lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Quy định cách đặt tên cho Solution, Project, Namespace, Folder, File, Class, Interface, Abstract Class, Record, Enum, Method, Property, Variable, Constant, Readonly, Private Field, Event, Command, Query, Handler, Validator, Specification, Repository, Factory, Policy, DTO, ViewModel, Request, Response, Mapping, Extension, Middleware, và Configuration.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Interface bắt buộc bắt đầu bằng chữ I (VD: IUserRepository). Private Field bắt buộc bắt đầu bằng _ (VD: _dbContext). Command kết thúc bằng Command (VD: CreateUserCommand).

### Anti-pattern
Đặt tên không nhất quán (VD: UserRepositoryInterface thay vì IUserRepository), sử dụng tiếng Việt trong tên biến (VD: _nguoiDung).

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 5. Folder Structure Standard

### Why
Tại sao Folder Structure Standard lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Quy chuẩn phân cấp thư mục chặt chẽ dựa trên Clean Architecture. Bao gồm các thư mục chính: Domain, Application, Infrastructure, Presentation, Shared, và Tests.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Solution được chia thành `src/` và `tests/`. Trong `src/`, các Project phải được đặt tên theo định dạng Company.Project.Layer (VD: AnSinhSo.Domain).

### Anti-pattern
Trộn lẫn các class của Application vào thư mục Infrastructure. Không chia tách rõ ràng giữa Commands và Queries trong Application layer.

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 6. Clean Architecture Rules

### Why
Tại sao Clean Architecture Rules lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Quy định bất di bất dịch về Dependency Rule, Layer Boundary, Reference Direction, Allowed Dependency và Forbidden Dependency.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Reference Direction chỉ đi vào trong: Presentation -> Infrastructure -> Application -> Domain. Domain không reference đến bất kỳ Project nào khác.

### Anti-pattern
Domain Project tham chiếu đến Entity Framework Core hoặc bất kỳ thư viện UI nào (Forbidden Dependency).

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 7. Domain-Driven Design Rules

### Why
Tại sao Domain-Driven Design Rules lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Cấu trúc hạt nhân của hệ thống. Định nghĩa rõ ranh giới của Aggregate Root, Entity, Value Object, Domain Event, Domain Service, Repository, Factory, Policy, Specification, và Invariant.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Sử dụng Factory để khởi tạo Aggregate Root khi logic phức tạp. Mọi Invariant (điều kiện bất biến) phải được validate ngay bên trong Aggregate Root.

### Anti-pattern
Anemic Domain (Entity chỉ có getter/setter mà không có business logic). Thực hiện lưu database trực tiếp từ Domain Service.

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 8. CQRS Standard

### Why
Tại sao CQRS Standard lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Phân tách rõ ràng giữa Command (thay đổi trạng thái) và Query (lấy dữ liệu), sử dụng MediatR, Handler, Validation và Pipeline Behavior.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Mỗi Command có một Handler riêng biệt. Validation được thực hiện qua Pipeline Behavior bằng FluentValidation trước khi Handler thực thi.

### Anti-pattern
Sử dụng chung một DTO cho cả Command và Query. Gọi trực tiếp Repository cập nhật dữ liệu bên trong Query Handler.

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 9. Coding Standard

### Why
Tại sao Coding Standard lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Tiêu chuẩn viết code C#, sử dụng XML Documentation, Comment, Formatting, Nullable, Async/Await, Exception, Logging, Result Pattern, Error Catalog, và Notification Pattern.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Mọi API Controller bắt buộc phải trả về kiểu dữ liệu đóng gói bằng Result Pattern. Mọi lỗi nghiệp vụ phải được ánh xạ vào Error Catalog.

### Anti-pattern
Sử dụng try-catch lồng nhau vô tội vạ để điều khiển luồng nghiệp vụ thay vì dùng Result Pattern. Trả về Exception trực tiếp cho Client.

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 10. Git Standard

### Why
Tại sao Git Standard lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Quy định vòng đời Git: Branch, Commit, Pull Request, Merge, Tag, Release.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Tên Branch phải tuân theo chuẩn `feature/ticket-name`, `bugfix/ticket-name`. Thông điệp Commit phải có tiền tố (VD: `feat:`, `fix:`, `docs:`).

### Anti-pattern
Commit trực tiếp lên nhánh main. Thông điệp Commit mơ hồ như fixed bug hoặc update code.

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 11. AI Coding Standard

### Why
Tại sao AI Coding Standard lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Quy tắc bắt buộc cho AI: Không Hallucination, Không tự ý tạo Architecture, Không thay đổi Domain, Namespace, Folder, Aggregate Root, Không bỏ Validation, Notification, Result Pattern.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
AI sinh code bám sát 100% vào Context Window. Khi không chắc chắn, AI phải hỏi (Prompt for clarification) thay vì tự biên dịch.

### Anti-pattern
AI tự động sinh ra một Design Pattern mới (VD: Singleton) trong khi dự án đã quy định dùng Dependency Injection.

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 12. Prompt Writing Standard

### Why
Tại sao Prompt Writing Standard lại là yếu tố sống còn? Trong kiến trúc quy mô Enterprise, sự thiếu chuẩn mực ở khía cạnh này sẽ dẫn đến sự hỗn loạn về mã nguồn, gây khó khăn cho việc bảo trì, làm giảm khả năng đọc hiểu của Developer và khiến AI Coding Agent dễ rơi vào trạng thái Hallucination. Việc thiết lập quy chuẩn này giúp mã nguồn có tính tự giải thích (self-documenting) cao nhất.

### What
Quy chuẩn viết Prompt dành cho Developer khi giao tiếp với AI: Ngắn gọn, rõ ràng, đầy đủ Context và tuyệt đối không mâu thuẫn với Rulebook.

### How
Cách thực hiện:
- Phải áp dụng đúng quy chuẩn được mô tả trong tài liệu này cho mọi artifact mới.
- Không được phép linh động hoặc tạo ra ngoại lệ trừ khi có sự phê duyệt từ Architecture Board.
- AI Coding Agent phải kiểm tra ngữ cảnh và đảm bảo kết quả sinh ra khớp hoàn toàn với cấu trúc chuẩn.

### Best Practice
- Luôn kiểm tra chéo (Cross-check) với các tệp tin đã tồn tại trong cùng một Layer.
- Sử dụng các công cụ phân tích tĩnh (Static Analysis) và EditorConfig để tự động hóa việc bắt lỗi.

### Example
Bao gồm nội dung của `31_SPRINT_03_BASE_CLASSES.md` vào Context Window trước khi yêu cầu AI viết một Entity mới.

### Anti-pattern
Cung cấp Prompt chung chung như 'Viết chức năng đăng nhập' mà không đính kèm Context về CQRS và Clean Architecture.

### Checklist
- [ ] Tuân thủ tuyệt đối quy ước được định nghĩa.
- [ ] Giữ nguyên thuật ngữ tiếng Anh.
- [ ] Được Review tự động bởi AI Coding Agent mà không sinh ra cảnh báo.

---

## 13. Review Checklist

Checklist bắt buộc dành cho Developer khi Review Pull Request:
- [ ] Mã nguồn đã tuân thủ chặt chẽ Clean Architecture Rules hay chưa?
- [ ] Result Pattern đã được áp dụng một cách triệt để chưa?
- [ ] Error Catalog có bị bỏ qua trong quá trình xử lý lỗi không?
- [ ] Đảm bảo không có bất kỳ Business Logic nào bị rò rỉ ra khỏi Domain Layer.
- [ ] Đảm bảo không có bất kỳ Infrastructure Logic nào bị lẫn lộn vào Application Layer.
- [ ] XML Documentation đã được viết đầy đủ và rõ ràng chưa?

---

## 14. AI Review Checklist

Checklist bắt buộc dành cho AI Coding Agent khi tiến hành Review mã nguồn:
- [ ] Static Analysis hoàn toàn không xuất hiện lỗi.
- [ ] Naming Convention khớp 100% với tài liệu quy chuẩn.
- [ ] Mọi dữ liệu đầu vào (Input) đều phải được chạy qua FluentValidation.
- [ ] Tất cả các phương thức bất đồng bộ (Async Method) đều bắt buộc nhận CancellationToken.
- [ ] Tuyệt đối không xảy ra hiện tượng Hallucination tự phát sinh ra các Interface hoặc Namespace không tồn tại trong dự án.

---

## 15. Developer Checklist

Danh sách kiểm tra bắt buộc trước khi thực hiện Commit (Pre-commit):
- [ ] Đảm bảo mã nguồn đã Build thành công mà không gặp lỗi.
- [ ] Tất cả các Unit Test đều được thực thi và Pass hoàn toàn.
- [ ] Mã nguồn đã được Format tuân thủ đúng tiêu chuẩn của EditorConfig.
- [ ] Không còn bất kỳ Warning nào từ Roslyn Analyzer.
- [ ] Đã dọn dẹp sạch sẽ các thư viện (using) không còn được sử dụng.

---

## 16. Definition of Coding Ready

Điều kiện tiên quyết để một tính năng chính thức được phép bước vào giai đoạn lập trình (Coding):
1. Yêu cầu nghiệp vụ (Requirement) đã được phân tích và làm rõ hoàn toàn.
2. Thiết kế kiến trúc (Architecture) dành riêng cho tính năng đó đã được đánh giá và phê duyệt.
3. Các tài liệu nền tảng như Rulebook và Constitution (từ Sprint 01 đến Sprint 03) đã được nạp đầy đủ vào Context Window của AI.
4. Nhánh phát triển (Branch) đã được tạo đúng theo quy chuẩn từ nhánh `develop`.

---

## 17. Development Workflow

Quy trình phát triển tiêu chuẩn (Standard Workflow) mang tính chất bắt buộc:
1. **Requirement:** Nắm bắt và thấu hiểu rõ ràng các yêu cầu nghiệp vụ.
2. **Architecture:** Tham chiếu nghiêm ngặt các tài liệu kiến trúc từ Sprint 01 đến Sprint 03.
3. **Prompt:** Xây dựng Prompt đúng quy chuẩn, đảm bảo cung cấp đầy đủ Context.
4. **AI Coding:** AI Coding Agent tiến hành sinh mã nguồn tuân thủ tuyệt đối Rulebook.
5. **Review:** AI và Developer thực hiện tự đánh giá lại chất lượng mã nguồn (Self-review).
6. **Refactor:** Tiến hành tối ưu hóa và tinh chỉnh mã nguồn nếu cần thiết.
7. **Commit:** Tạo các Commit tuân theo tiêu chuẩn Semantic Versioning.
8. **Pull Request:** Khởi tạo Pull Request để tiến hành đánh giá chéo (Cross-review).
9. **Merge:** Thực hiện hợp nhất mã nguồn an toàn vào nhánh chính.

---

## 18. Coding Freeze Rules

Các quy định khắt khe liên quan đến việc thay đổi kiến trúc:
- Toàn bộ kiến trúc được thiết lập từ Sprint 01 đến Sprint 03 chính thức được coi là ĐÓNG BĂNG HOÀN TOÀN (Frozen).
- Cả Developer lẫn AI Coding Agent tuyệt đối **KHÔNG ĐƯỢC** phép tự ý chỉnh sửa các Interface lõi, các Base Classes, hoặc thay đổi các quy tắc phụ thuộc (Dependency Rule).
- Bất kỳ yêu cầu thay đổi nào có khả năng tác động đến Architecture đều phải được lập thành văn bản đệ trình lên Architecture Board. Việc sửa đổi chỉ được phép tiến hành sau khi có chữ ký phê duyệt chính thức (Sign-off).

---

## 19. Golden Rules

20 Nguyên tắc vàng (Golden Rules) mang tính chất sống còn của dự án:
1. Architecture First (Kiến trúc luôn đi đầu).
2. Domain First (Nghiệp vụ phải được ưu tiên số 1).
3. Business Rule First (Luật nghiệp vụ không bao giờ bị nhân nhượng).
4. No Business Logic in Controller (Không rò rỉ nghiệp vụ ra Controller).
5. No Business Logic in Repository (Không rò rỉ nghiệp vụ vào Repository).
6. No Direct Database Access (Không truy cập Database trực tiếp mà bỏ qua Unit of Work).
7. Always Use Result Pattern (Luôn trả về kết quả qua Result Pattern).
8. Always Use Error Catalog (Luôn quản lý lỗi qua Error Catalog).
9. Always Validate Input (Luôn kiểm duyệt dữ liệu đầu vào qua Pipeline).
10. Always Review AI Generated Code (Luôn soát xét kỹ mã nguồn do AI sinh ra).
11. Dependency Inversion is Mandatory (Luôn đảo ngược sự phụ thuộc).
12. Fail Fast (Chặn lỗi ngay lập tức ở rìa hệ thống).
13. Immutable Value Objects (Value Object bắt buộc phải bất biến).
14. Single Responsibility (Một class chỉ làm một việc).
15. Do Not Repeat Yourself (Loại bỏ code trùng lặp nhưng không lạm dụng over-engineering).
16. Async All The Way (Tận dụng sức mạnh bất đồng bộ).
17. Secure By Design (Bảo mật ngay từ khâu thiết kế).
18. Log Meaningfully (Log có cấu trúc và có ý nghĩa).
19. Test The Behavior, Not The Implementation (Test hành vi, không test cách triển khai).
20. The Constitution is Absolute (Tài liệu này là tối thượng).

---

## 20. Versioning Policy

### Why
Tại sao cần quy định Versioning Policy? Trong một dự án Enterprise, việc quản lý phiên bản (Version) rất quan trọng để đảm bảo tính nhất quán của mã nguồn và tài liệu. Việc tuân thủ chuẩn Semantic Versioning giúp tất cả Developer, QA, và Hệ thống CI/CD hiểu rõ tính chất của sự thay đổi.

### What
Bổ sung quy định Versioning bao gồm Semantic Versioning (Major.Minor.Patch).

### How
Cách thực hiện:
- **Major**: Phản ánh những thay đổi lớn về kiến trúc hoặc phá vỡ cấu trúc hiện tại (Breaking Changes). Major Version chỉ được thay đổi khi có sự phê duyệt trực tiếp bằng văn bản từ Architecture Board.
- **Minor**: Bổ sung tính năng mới, tương thích ngược (Backward Compatible).
- **Patch**: Sửa lỗi (Bugfix), cập nhật nhỏ không ảnh hưởng đến tính năng.

### Best Practice
- Gắn thẻ (Tag) trên Git repository tự động theo Semantic Versioning.

### Example
Thay đổi từ `1.0.0` lên `2.0.0` (Cần Architecture Board phê duyệt). Thay đổi từ `1.1.0` lên `1.2.0` (Thêm tính năng).

### Anti-pattern
Tự ý tăng Major version khi chỉ fix một bug nhỏ. 

### Checklist
- [ ] Thay đổi phiên bản tuân thủ Semantic Versioning.
- [ ] Thay đổi Major Version đã có phê duyệt từ Architecture Board.

---

## 21. Documentation Change Policy

### Why
Tài liệu định hình nên luật lệ của dự án. Không thể tự ý thay đổi luật mà không có sự kiểm soát chặt chẽ.

### What
Bổ sung chính sách thay đổi tài liệu. Quy định rõ những hành vi được phép và không được phép khi chỉnh sửa tài liệu.

### How
Cách thực hiện:
**Được phép:**
- Sửa lỗi chính tả.
- Sửa ví dụ (Example).
- Sửa đường dẫn liên kết (Link).

**Không được (Trừ khi có Architecture Board phê duyệt):**
- Sửa Rule (Quy tắc).
- Sửa Architecture (Kiến trúc).
- Sửa Domain (Nghiệp vụ).
- Sửa Dependency (Phụ thuộc).

### Best Practice
- Review tài liệu giống như Review code (Document as Code).

### Example
Được phép sửa lỗi chính tả. Không được phép thêm thư viện MongoDB vào danh sách Dependency cho phép nếu chưa họp Architecture Board.

### Anti-pattern
Một Developer tự ý cập nhật Rulebook để cho phép gọi trực tiếp Database từ API Controller.

### Checklist
- [ ] Thay đổi không vi phạm chính sách cấm (Không sửa Rule, Architecture, Domain, Dependency).
- [ ] Thay đổi vi phạm đã được Architecture Board phê duyệt.

---

## 22. AI Context Loading Order

### Why
AI cần ngữ cảnh (Context) để làm việc. Đưa Context sai thứ tự hoặc thiếu Context sẽ gây ra hiện tượng Hallucination và làm hỏng kiến trúc.

### What
Bổ sung thứ tự AI phải đọc tài liệu.

### How
Cách thực hiện: AI phải load theo đúng trình tự sau để hiểu từ tổng quan đến chi tiết, từ quy trình đến nghiệp vụ:
1. `00_PROJECT_INDEX`
2. `00_PROJECT_BOOTSTRAP`
3. `14_PROJECT_STRUCTURE`
4. Sprint 03 Documents (31 → 49)
5. `50_DOCUMENTATION_LANGUAGE_STANDARD`
6. `Current Sprint`

### Best Practice
- Tạo các Script hoặc Alias để tự động hóa việc gộp Context cho AI.

### Example
Chạy lệnh gộp tài liệu theo thứ tự trên trước khi bắt đầu đặt câu hỏi cho AI Coding Agent.

### Anti-pattern
Đưa ngay một Task Code cho AI mà không cho AI đọc tài liệu Rule.

### Checklist
- [ ] Đã load tài liệu đúng thứ tự quy định.

---

## 23. Definition of Code Done

### Why
Giúp các Developer và AI biết chính xác khi nào một tính năng thực sự "Hoàn thành", tránh tình trạng Done giả (Fake Done).

### What
Bổ sung Definition of Code Done. Một Feature chỉ được xem là DONE khi thỏa mãn toàn bộ các điều kiện khắt khe nhất.

### How
Cách thực hiện: Một Feature chỉ được xem là DONE khi:
- [x] Build thành công.
- [x] Unit Test Pass.
- [x] Integration Test Pass (nếu có).
- [x] XML Documentation đã được viết.
- [x] Áp dụng chuẩn Result Pattern.
- [x] Áp dụng chuẩn FluentValidation.
- [x] Có cơ chế Logging.
- [x] Exception Mapping đúng chuẩn.
- [x] Đã cập nhật Swagger.
- [x] Đã tạo file Migration (nếu có thay đổi Database).
- [x] Tạo Pull Request.
- [x] Pass Code Review.
- [x] Merge thành công.

### Best Practice
- Gắn Checklist này vào Pull Request Template.

### Example
Hoàn thành một API không chỉ là code xong API, mà còn phải viết Unit Test, khai báo Error Catalog, validate dữ liệu, log request, và tạo Migration.

### Anti-pattern
Báo "Done" trong buổi Daily Meeting khi code mới chỉ chạy được trên máy Local (Works on my machine).

### Checklist
- [ ] Vượt qua toàn bộ tiêu chí của Definition of Code Done.

---

## 24. AI Response Rules

### Why
AI có xu hướng "chiều lòng" người dùng và tự ý sáng tạo. Điều này cực kỳ nguy hiểm trong dự án Enterprise Architecture. Chúng ta cần thiết lập ranh giới tuyệt đối cho AI.

### What
Bổ sung Rule bắt buộc cho AI Coding Agent. Khi nào AI được làm và khi nào AI phải dừng lại.

### How
Cách thực hiện: AI **KHÔNG ĐƯỢC**:
- Tự tạo Architecture.
- Tự tạo Folder ngoài quy chuẩn.
- Tự tạo Namespace sai quy ước.
- Tự tạo Design Pattern mới.
- Tự đổi Aggregate Root.
- Tự bỏ Validation.
- Tự bỏ Result Pattern.
- Tự bỏ Notification Pattern.
- Tự thêm Package mà không hỏi ý kiến.

Nếu thiếu Context, AI **PHẢI DỪNG LẠI** và đặt câu hỏi cho Developer.

### Best Practice
- Luôn kiểm tra kỹ các đề xuất của AI trước khi chèn vào mã nguồn (Accept Code).

### Example
Thay vì sinh code trả về Exception, AI nhận ra thiếu Error Catalog, AI dừng lại và hỏi: "Vui lòng cung cấp mã lỗi tương ứng trong Error Catalog để tôi sử dụng Result Pattern".

### Anti-pattern
AI tự ý cài thư viện `Newtonsoft.Json` vào dự án khi framework mặc định đã là `System.Text.Json`.

### Checklist
- [ ] AI không thực hiện bất kỳ hành động nào trong danh sách bị cấm.
- [ ] AI có dừng lại và hỏi khi thiếu Context.

---

## Glossary

| Term | Meaning |
|------|---------|
| DDD | Domain-Driven Design |
| CQRS | Command Query Responsibility Segregation |
| DTO | Data Transfer Object |
| DI | Dependency Injection |
| Aggregate Root | Root Entity của Aggregate |
| Value Object | Immutable Domain Object |
| Repository | Domain Repository Pattern |
| Unit of Work | Transaction Boundary |
| Result Pattern | Standard Response Pattern |
| Specification Pattern | Business Rule Encapsulation |
| FluentValidation | Validation Framework |
| MediatR | CQRS Mediator Library |
| Serilog | Structured Logging Framework |
| ASP.NET Core | Backend Framework |

---

## Compliance Level

Toàn bộ nội dung của tài liệu này được phân thành ba cấp độ tuân thủ:

### Level 1 – Mandatory (MUST)

Bắt buộc tuân thủ.

Không được phép vi phạm.

Ví dụ:

- Clean Architecture
- DDD
- CQRS
- Naming Convention
- Result Pattern
- Dependency Rule

---

### Level 2 – Recommended (SHOULD)

Khuyến khích áp dụng.

Có thể điều chỉnh nếu có lý do hợp lý.

Ví dụ:

- Logging Format
- XML Documentation
- Folder Grouping

---

### Level 3 – Optional (MAY)

Có thể áp dụng khi phù hợp.

Không ảnh hưởng đến kiến trúc.

Ví dụ:

- Coding Style Preferences
- Comment Style
- File Ordering

---

## Constitution Priority

Quy định thứ tự ưu tiên khi nhiều tài liệu mâu thuẫn.

Ví dụ:

**Priority Order:**

1. Development Constitution
2. Architecture Rulebook
3. Sprint Documentation
4. Source Code Comments

Nếu có mâu thuẫn giữa các tài liệu thì tài liệu có Priority cao hơn sẽ được ưu tiên.

---

## 25. Final Declaration

**Tuyên bố chính thức:**
Kể từ thời điểm tài liệu `50_DOCUMENTATION_LANGUAGE_STANDARD.md` này chính thức được ban hành và phê duyệt:
- Toàn bộ các tài liệu và kết quả đạt được trong **Sprint 01**, **Sprint 02**, và **Sprint 03** được xác nhận là **ĐÓNG BĂNG HOÀN TOÀN (FROZEN)**.
- Bắt đầu từ **Sprint 04** trở đi, dự án sẽ dồn **100% tài nguyên và mức độ tập trung cho giai đoạn Development (Phát triển tính năng)**.
- Bất kỳ hành động nào nhằm quay lại chỉnh sửa Architecture, Base Classes hay Rulebook đều bị nghiêm cấm tuyệt đối, trừ khi có quyết định phê duyệt chính thức từ Architecture Board.

**YÊU CẦU TUÂN THỦ TUYỆT ĐỐI.**

## 26. Architecture Decision Records (ADR)

### Why
Trong các dự án Enterprise, các quyết định kiến trúc quan trọng cần được lưu vết rõ ràng để giải thích lý do vì sao một sự lựa chọn lại được đưa ra tại một thời điểm cụ thể. Điều này giúp các thành viên mới và AI Coding Agent thấu hiểu ngữ cảnh lịch sử thay vì mù quáng làm theo hoặc cố gắng thay đổi các quyết định đã chốt.

### What
Bắt buộc sử dụng Architecture Decision Records (ADR) để ghi lại mọi thay đổi đáng kể về thiết kế, thư viện cốt lõi, hoặc kiến trúc hệ thống.

### How
Cách thực hiện:
- Mọi quyết định thay đổi hệ thống phải được soạn thảo thành một tài liệu ADR.
- ADR phải tuân thủ format tiêu chuẩn bao gồm: Context, Decision, Status, và Consequences.
- ADR phải được phê duyệt bởi Architecture Board trước khi áp dụng.

### Best Practice
- Lưu trữ các ADR trong thư mục `docs/adr/` và đánh số thứ tự tịnh tiến (VD: `001_Use_MediatR_For_CQRS.md`).

### Example
Tạo một ADR ghi chú lý do chọn `FluentValidation` thay vì Data Annotations để đảm bảo nguyên lý Separation of Concerns trong Clean Architecture.

### Anti-pattern
Đưa ra quyết định thay đổi thư viện cốt lõi qua tin nhắn Slack mà không có tài liệu lưu vết chính thức.

### Checklist
- [ ] Bất kỳ quyết định kiến trúc mới nào đều được ghi nhận bằng một ADR.
- [ ] ADR đã được Architecture Board phê duyệt.

---

## 27. Single Source of Truth

### Why
Khi thông tin bị phân mảnh ở nhiều nơi (Wiki, Slack, Word, Markdown), Developer và AI Coding Agent sẽ dễ bị nhầm lẫn và đưa ra quyết định sai lệch. Dự án Enterprise cần một nguồn chân lý duy nhất.

### What
Quy định kho lưu trữ mã nguồn (Git Repository) và các tài liệu Markdown đi kèm là Single Source of Truth (SSOT) duy nhất của dự án.

### How
Cách thực hiện:
- Bất kỳ quy định, thiết kế, hay yêu cầu nào không có mặt trong thư mục tài liệu chính thức (`.md`) của Repository đều được xem là không hợp lệ.
- Nếu có sự sai lệch giữa tài liệu ngoài (Jira, Confluence) và tài liệu trong Repository, tài liệu trong Repository luôn đúng.

### Best Practice
- Cập nhật tài liệu song song với quá trình cập nhật mã nguồn (Document as Code).

### Example
Thay vì tìm kiếm quy định về Exception Mapping trên nhóm chat, Developer chỉ được phép tham chiếu đến các tài liệu thuộc Rulebook đã được quy định trong Source Code.

### Anti-pattern
Triển khai tính năng dựa trên lời nói trong một cuộc họp mà không yêu cầu cập nhật lại tài liệu yêu cầu trong mã nguồn.

### Checklist
- [ ] Mọi nghiệp vụ và quy tắc đều được tài liệu hóa thành văn bản Markdown trong Git.

---

## 28. Conflict Resolution Policy

### Why
Mâu thuẫn giữa các tài liệu, mâu thuẫn giữa AI và Developer, hoặc mâu thuẫn giữa các Developer là điều khó tránh. Việc có một chính sách giải quyết rõ ràng giúp dự án không bị đình trệ.

### What
Chính sách định tuyến và giải quyết mọi xung đột liên quan đến kiến trúc và nghiệp vụ.

### How
Cách thực hiện:
- Khi có xung đột giữa mã nguồn và tài liệu Rulebook: Tài liệu Rulebook thắng.
- Khi có xung đột giữa hai tài liệu ngang cấp: Đệ trình lên Architecture Board để đưa ra phán quyết.
- Khi có xung đột giữa AI Coding Agent và Developer: Developer phải tạm dừng AI, kiểm tra lại Prompt và tài liệu, sau đó cung cấp ngữ cảnh chuẩn xác hơn.

### Best Practice
- Không bao giờ cố tình vi phạm Rulebook để "vượt rào" giải quyết xung đột ngắn hạn.

### Example
Nếu AI khăng khăng tạo một class Application Service nhưng tài liệu quy định dùng Domain Service, Developer phải tuân thủ tài liệu và điều chỉnh lại Prompt cho AI.

### Anti-pattern
Tự ý thay đổi Rulebook để làm cho mã nguồn sai trở thành mã nguồn hợp lệ.

### Checklist
- [ ] Mọi xung đột kiến trúc đều được báo cáo và xử lý dựa trên Constitution.

---

## 29. AI Decision Matrix

### Why
AI Coding Agent hoạt động hiệu quả nhất khi được cung cấp một bộ định tuyến quyết định (Decision Matrix) rõ ràng. Giúp AI biết chính xác khi nào được tự động hóa và khi nào phải dừng lại để xin phép.

### What
Ma trận phân quyền tự chủ của AI Coding Agent trong việc xử lý mã nguồn.

### How
Cách thực hiện:
- **Tự động 100%:** Sinh Boilerplate code, viết XML Documentation, sinh Unit Test cho logic đã có, Refactor theo EditorConfig.
- **Phải xin phép trước khi làm:** Khởi tạo Aggregate Root mới, thay đổi cấu trúc của Entity, cập nhật Policy.
- **Cấm tuyệt đối:** Xóa các file kiến trúc cốt lõi, thay đổi cơ chế Dependency Injection, chỉnh sửa Base Classes.

### Best Practice
- Thiết lập các Custom Instructions trong IDE để tự động ép AI tuân thủ Decision Matrix này.

### Example
AI được phép tự động sinh 10 class DTO tương ứng với một Aggregate mà không cần hỏi thêm, nhưng phải hỏi trước khi thêm một Domain Event mới.

### Anti-pattern
Cho phép AI tự động chèn một Design Pattern phức tạp vào hệ thống chỉ để giải quyết một logic nghiệp vụ đơn giản.

### Checklist
- [ ] Hành động của AI nằm trong giới hạn tự chủ cho phép.
- [ ] AI có dừng lại xin phép ở các vùng cấm.

---

## 30. Forbidden Actions

### Why
Đôi khi những quy định "Nên làm" chưa đủ mạnh mẽ, hệ thống Enterprise cần những quy định "Tuyệt đối không được làm" để vạch ra ranh giới tử thần.

### What
Danh sách các hành động bị cấm vĩnh viễn và không có ngoại lệ.

### How
Cách thực hiện: Tuyệt đối KHÔNG:
- Đặt câu truy vấn (SQL/LINQ) bên ngoài thư mục Infrastructure.
- Khởi tạo trực tiếp (từ khóa `new`) các Dependency cốt lõi mà không qua Dependency Injection.
- Sử dụng `Thread.Sleep()` hoặc các lệnh chặn luồng (blocking calls) trong môi trường Async.
- Commit thông tin nhạy cảm (Passwords, API Keys, Secrets) vào mã nguồn.
- Bỏ qua cơ chế Result Pattern để throw trực tiếp `Exception` ra Presentation Layer nhằm quản lý luồng chạy nghiệp vụ.

### Best Practice
- Cấu hình CI/CD Pipeline để tự động đánh rớt (fail) bất kỳ Pull Request nào chứa các Forbidden Actions.

### Example
Sử dụng `Task.Delay()` thay vì `Thread.Sleep()`, hoặc inject `IDateTimeProvider` thay vì dùng tĩnh `DateTime.Now`.

### Anti-pattern
Bỏ qua cảnh báo của linter và force commit mã nguồn vi phạm Forbidden Actions.

### Checklist
- [ ] Mã nguồn không chứa bất kỳ hành động nào trong danh sách bị cấm.

---

## 31. Technical Debt Policy

### Why
Nợ kỹ thuật (Technical Debt) là không thể tránh khỏi khi phải cân bằng giữa thời gian và chất lượng. Tuy nhiên, việc không kiểm soát và không trả nợ sẽ dẫn đến sự sụp đổ của kiến trúc.

### What
Quy định về việc ghi nhận, đánh giá và giải quyết Nợ kỹ thuật.

### How
Cách thực hiện:
- Mọi đoạn code được xác định là Nợ kỹ thuật phải được đánh dấu rõ ràng bằng `// TODO: [TechDebt]` kèm theo ID Ticket.
- Không cho phép tồn tại Nợ kỹ thuật liên quan đến Clean Architecture Boundaries (VD: Application không được phụ thuộc Infrastructure). Nợ kiến trúc phải được giải quyết trong cùng Sprint.
- Các Nợ kỹ thuật cấp thấp phải được đưa vào Backlog và xử lý định kỳ.

### Best Practice
- Đặt giới hạn thời gian cho mỗi thẻ TODO, nếu quá hạn, CI/CD sẽ hiển thị Warning.

### Example
`// TODO: [TechDebt][ASS-1024] Refactor logic tính toán sang Strategy Pattern trong Sprint tiếp theo.`

### Anti-pattern
Tạo Nợ kỹ thuật âm thầm bằng cách comment `// TODO: fix this later` mà không lưu lại ticket theo dõi.

### Checklist
- [ ] Bất kỳ Nợ kỹ thuật nào được đưa vào mã nguồn đều có Ticket đi kèm.

---

## 32. Exception Approval Process

### Why
Trong một số trường hợp cực kỳ đặc thù, dự án có thể cần phải phá vỡ một Rule. Nếu không có quy trình rõ ràng, các Developer sẽ tự ý "bẻ luật".

### What
Quy trình xin cấp phép ngoại lệ (Exception) cho một Rule trong tài liệu Constitution.

### How
Cách thực hiện:
- Bước 1: Developer tạo một Request Document mô tả rõ Rule muốn phá vỡ và lý do nghiệp vụ.
- Bước 2: Trình bày phương án thay thế và rủi ro nếu phá vỡ Rule.
- Bước 3: Đệ trình lên Architecture Board.
- Bước 4: Chỉ khi có văn bản phê duyệt, Rule mới được phép bị phá vỡ cục bộ cho trường hợp đó.

### Best Practice
- Giữ tỷ lệ ngoại lệ ở mức tối thiểu cho toàn bộ dự án.

### Example
Xin phép viết một Raw SQL Query trực tiếp bằng Dapper thay vì Entity Framework Core để tối ưu hóa một báo cáo xử lý dữ liệu khổng lồ.

### Anti-pattern
Bẻ luật trước, xin phép sau (Shoot first, ask questions later).

### Checklist
- [ ] Có đầy đủ tài liệu giải trình và được phê duyệt từ Architecture Board trước khi triển khai ngoại lệ.

---

## 33. AI Output Standard

### Why
Bản thân AI Coding Agent sinh ra nội dung (Output) rất khác nhau tùy theo mô hình. Việc chuẩn hóa Output giúp đảm bảo mã nguồn và tài liệu đồng nhất bất kể AI nào được sử dụng.

### What
Tiêu chuẩn đầu ra bắt buộc mà mọi AI Coding Agent phải định dạng trước khi trả kết quả cho Developer.

### How
Cách thực hiện:
- Mã nguồn sinh ra phải chứa đủ các khối cốt lõi thay vì lược bỏ tùy tiện bằng các comment như `// ... existing code ...` trừ khi được yêu cầu rõ ràng.
- Phải áp dụng đúng định dạng cấu trúc, Naming Convention, và Result Pattern.
- Không tự ý thêm thư viện mới (Packages) nếu không được chỉ thị.

### Best Practice
- Đưa Output Standard này vào System Prompt mặc định của toàn bộ các AI Assistant dùng trong dự án.

### Example
Khi refactor một class lớn, AI phải đưa ra khối code trọn vẹn để Developer dễ dàng đối chiếu và tích hợp.

### Anti-pattern
AI tự ý thu gọn các property quan trọng trong Entity chỉ để kết xuất ra Output nhanh hơn.

### Checklist
- [ ] Output của AI có đầy đủ định dạng và đúng tiêu chuẩn cấu trúc của dự án.

---

## 34. Future Sprint Protection

### Why
Khi dự án đóng băng thiết kế và tiến sang các Sprint mới, rủi ro vô tình làm hỏng các nền tảng cũ rất cao. Cần có quy định bảo vệ mã nguồn cốt lõi.

### What
Các cơ chế kỹ thuật và quy tắc hành xử nhằm bảo vệ tuyệt đối các Artifact đã hoàn thành trong các Sprint trước.

### How
Cách thực hiện:
- Các Base Classes, Interfaces cốt lõi từ Sprint 01-03 được quản lý chặt chẽ và yêu cầu nhiều hơn 2 Approver trong quá trình Pull Request nếu có thay đổi.
- Nghiêm cấm việc tái cấu trúc (Refactor) các Aggregates của Sprint cũ trong lúc thực hiện Task của Sprint mới nếu không có Ticket độc lập.
- Bất kỳ thay đổi nào phá vỡ sự tương thích ngược (Backward Incompatibility) phải bị từ chối ngay ở vòng Code Review.

### Best Practice
- Sử dụng các Unit Tests và Integration Tests hiện có làm mạng lưới an toàn (Safety Net) nhằm đảm bảo hệ thống cũ không bị phá vỡ.

### Example
Khi bổ sung tính năng ở Sprint 04, không được phép thay đổi cấu trúc của class `User` đã hoàn thành, thay vào đó dùng cơ chế Extension hoặc thiết kế Aggregate mới độc lập.

### Anti-pattern
Tùy tiện sửa logic của các Domain Services lõi chỉ để "thuận tiện" cho một tính năng nhỏ ở Sprint hiện tại.

### Checklist
- [ ] Tính năng mới không gây thay đổi phá vỡ (Breaking Change) đối với nền tảng hiện hữu.

---

## 35. Constitution Change Procedure

### Why
Hiến pháp là tài liệu tối cao, đóng vai trò định hình dự án. Tuy nhiên, nếu thực tiễn buộc phải thay đổi Hiến pháp, quá trình này cần diễn ra cực kỳ cẩn trọng để bảo vệ tính nhất quán của hệ thống.

### What
Quy trình tuân thủ khi có nhu cầu thay đổi, cập nhật hoặc bổ sung các điều luật trong tài liệu Constitution.

### How
Cách thực hiện:
- Bất kỳ yêu cầu thay đổi nào đối với tài liệu `50_DOCUMENTATION_LANGUAGE_STANDARD.md` phải được khởi tạo dưới dạng một yêu cầu chính thức.
- Hội đồng kiến trúc (Architecture Board) phải họp bàn và đánh giá tác động của sự thay đổi lên toàn bộ hệ thống.
- Yêu cầu sự biểu quyết đồng thuận từ Architecture Board.
- Nếu được thông qua, phải ban hành thông báo toàn dự án và tiến hành tăng Version của tài liệu.

### Best Practice
- Thực hiện định kỳ việc rà soát Hiến pháp thay vì sửa đổi nhỏ lẻ liên tục.

### Example
Quy trình đệ trình một phê duyệt để bổ sung hoặc thay đổi một Design Pattern cốt lõi áp dụng trên toàn dự án.

### Anti-pattern
Sửa trực tiếp file Constitution trên nhánh `main` mà không thông qua Architecture Board và không có Pull Request Review.

### Checklist
- [ ] Có tài liệu đánh giá tác động.
- [ ] Có sự biểu quyết đồng thuận của toàn bộ Architecture Board.

---

# END OF SPECIFICATION
