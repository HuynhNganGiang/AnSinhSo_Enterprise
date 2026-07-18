import re

filepath = r'D:\AnSinhSo_Enterprise\50_DOCUMENTATION_LANGUAGE_STANDARD.md'

with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# Fix formatting
content = re.sub(r'---## ', '---\n\n## ', content)

# Fix control characters
content = content.replace(chr(9) + 'ests', 'tests')
content = content.replace(chr(9) + 'ry-catch', 'try-catch')
content = content.replace(chr(12) + 'eature', 'feature')
content = content.replace(chr(8) + 'ugfix', 'bugfix')
content = content.replace(chr(12) + 'eat', 'feat')
content = content.replace(chr(12) + 'ix', 'fix')

new_sections = """## 20. Versioning Policy

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
Được phép sửa chữ "feat" bị sai chính tả. Không được phép thêm "MongoDB" vào danh sách Dependency cho phép nếu chưa họp Architecture Board.

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
4. `31_SPRINT_03_BASE_CLASSES` -> `50_DOCUMENTATION_LANGUAGE_STANDARD`
5. `Current Sprint`

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

"""

content = content.replace('## 20. Final Declaration', new_sections + '## 25. Final Declaration')

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

print("Update successful.")
