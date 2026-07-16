# =============================================================================
# 00_PROJECT_BOOTSTRAP.md
# =============================================================================
# Project         : AnSinhSo Enterprise
# Document        : Project Bootstrap
# Version         : 1.0.0
# Status          : BASELINE
# Owner           : Solution Architecture Team
# Last Updated    : YYYY-MM-DD
# =============================================================================

> Đây là tài liệu đầu tiên mà mọi AI Coding Agent, Developer và Technical Reviewer phải đọc trước khi làm việc với dự án.

---

# TABLE OF CONTENTS

- Phase 1. Purpose
- Phase 2. Project Overview
- Phase 3. Enterprise Documentation Suite
- Phase 4. Reading Order
- Phase 5. Architecture Baseline
- Phase 6. Technology Stack
- Phase 7. Development Workflow
- Phase 8. AI Working Rules
- Phase 9. Sprint Status
- Phase 10. Bootstrap Checklist

---
# Phase 1 – Purpose

---

## 1.1 Purpose of This Document

`00_PROJECT_BOOTSTRAP.md` là tài liệu khởi tạo (Bootstrap Document) của toàn bộ dự án **AnSinhSo Enterprise**.

Đây là tài liệu đầu tiên mà:

- AI Coding Agent
- Software Architect
- Backend Developer
- Frontend Developer
- DevOps Engineer
- Technical Reviewer

phải đọc trước khi tham khảo bất kỳ tài liệu kỹ thuật nào khác.

---

## 1.2 Objectives

Tài liệu này có các mục tiêu sau:

### Objective 1 – Establish Project Context

Giúp AI và Developer hiểu:

- Dự án là gì.
- Mục tiêu của hệ thống.
- Phạm vi triển khai.
- Kiến trúc tổng thể.

---

### Objective 2 – Standardize Documentation Reading

Thiết lập thứ tự đọc tài liệu chuẩn nhằm đảm bảo:

- Không bỏ sót tài liệu quan trọng.
- Không hiểu sai kiến trúc.
- Không tạo mã nguồn vượt phạm vi Sprint.

---

### Objective 3 – Bootstrap AI Coding

Mọi AI Coding Agent phải sử dụng tài liệu này làm điểm bắt đầu trước khi:

- Phân tích tài liệu.
- Lập kế hoạch Sprint.
- Sinh mã nguồn.
- Refactor.
- Review.

---

### Objective 4 – Architecture Governance

Đảm bảo toàn bộ Source Code được xây dựng đúng:

- Clean Architecture
- Enterprise Documentation Suite
- Coding Standards
- Security Standards
- Development Roadmap

---

## 1.3 Scope

Bootstrap chỉ quản lý:

- Documentation Suite
- Baseline
- Sprint Workflow
- AI Workflow
- Development Workflow

Bootstrap **không chứa**:

- Business Rules
- Database Design
- API Specification
- Source Code
- SQL Scripts

Các nội dung này được quản lý trong các tài liệu chuyên biệt.

---

## 1.4 Expected Outcomes

Sau khi đọc xong Phase này, AI hoặc Developer phải:

- Hiểu vai trò của Bootstrap.
- Hiểu phạm vi của tài liệu.
- Biết phải đọc tài liệu nào tiếp theo.
- Không tự ý thay đổi kiến trúc dự án.

---

## Phase 1 Summary

Phase 1 xác định vai trò của `00_PROJECT_BOOTSTRAP.md` là **AI Entry Point** và **Project Entry Point** cho toàn bộ hệ thống AnSinhSo Enterprise.
# Phase 2 – Project Overview

## 2.1 Project Information

| Item | Value |
|------|-------|
| Project Name | AnSinhSo Enterprise |
| Full Name | Hệ thống An Sinh Số xã Sông Lũy |
| Project Type | Enterprise Web Application |
| Architecture | Clean Architecture |
| Framework | ASP.NET Core 8 |
| Database | SQL Server 2022 |
| Mapping Platform | OpenStreetMap / Leaflet |
| AI Platform | AI Assistant Integration |
| Messaging Platform | Zalo Official Account |
| Deployment | Windows Server + IIS |
| Documentation Standard | Enterprise Documentation Suite |

---

## 2.2 Project Vision

AnSinhSo Enterprise được xây dựng với mục tiêu trở thành nền tảng quản lý an sinh xã hội hiện đại, tập trung, minh bạch và có khả năng mở rộng cho chính quyền cấp xã.

Hệ thống không chỉ số hóa dữ liệu hộ gia đình và đối tượng an sinh mà còn tích hợp bản đồ số (GIS), trí tuệ nhân tạo (AI) và Zalo Official Account nhằm hỗ trợ quản lý, điều phối và cung cấp dịch vụ công một cách hiệu quả.

Trong tương lai, hệ thống có thể mở rộng để triển khai cho nhiều xã, phường hoặc toàn huyện mà không cần thay đổi kiến trúc lõi.

---

## 2.3 Project Mission

Sứ mệnh của dự án là xây dựng một hệ thống quản lý an sinh xã hội có khả năng:

- Chuẩn hóa dữ liệu an sinh.
- Giảm thao tác thủ công trong quản lý.
- Hỗ trợ lãnh đạo ra quyết định dựa trên dữ liệu.
- Tăng tính minh bạch trong quá trình chi trả trợ cấp.
- Cải thiện khả năng tương tác giữa chính quyền và người dân.
- Từng bước chuyển đổi số công tác an sinh tại địa phương.

---

## 2.4 Business Context

Hiện nay phần lớn dữ liệu an sinh tại cấp xã vẫn được quản lý bằng Excel hoặc hồ sơ giấy.

Điều này dẫn đến nhiều hạn chế:

- Dữ liệu phân tán.
- Khó kiểm tra trùng lặp.
- Khó thống kê.
- Thiếu khả năng theo dõi lịch sử.
- Không hỗ trợ trực quan hóa trên bản đồ.
- Không có cơ chế thông báo tự động.
- Khó mở rộng khi số lượng đối tượng tăng lên.

AnSinhSo Enterprise được xây dựng để giải quyết toàn bộ các vấn đề trên.

---

## 2.5 Project Objectives

Dự án hướng đến các mục tiêu chính sau:

### Objective 1 – Digital Transformation

Số hóa toàn bộ dữ liệu an sinh của xã.

---

### Objective 2 – Centralized Data Management

Xây dựng cơ sở dữ liệu tập trung phục vụ quản lý thống nhất.

---

### Objective 3 – GIS Integration

Hiển thị dữ liệu hộ gia đình và đối tượng an sinh trên bản đồ số.

---

### Objective 4 – AI Integration

Ứng dụng AI để:

- hỗ trợ tìm kiếm thông minh;
- phân tích dữ liệu;
- dự báo nhu cầu an sinh;
- hỗ trợ cán bộ trong quá trình xử lý nghiệp vụ.

---

### Objective 5 – Zalo OA Integration

Tích hợp Zalo Official Account để:

- gửi thông báo;
- gửi lịch chi trả;
- tiếp nhận phản ánh;
- hỗ trợ người dân tra cứu thông tin.

---

### Objective 6 – Decision Support

Cung cấp Dashboard trực quan giúp lãnh đạo theo dõi tình hình an sinh và đưa ra quyết định nhanh chóng.

---

## 2.6 Core Business Modules

Hệ thống bao gồm các nhóm chức năng chính:

### Administrative Management

- Quản lý người dùng.
- Quản lý vai trò.
- Quản lý phân quyền.
- Nhật ký hệ thống.

---

### Household Management

- Quản lý hộ gia đình.
- Quản lý thành viên.
- Quản lý địa bàn.
- Quản lý tọa độ GIS.

---

### Social Welfare Management

- Quản lý đối tượng an sinh.
- Quản lý nhóm đối tượng.
- Quản lý chính sách.
- Quản lý trợ cấp.

---

### Payment Management

- Quản lý đợt chi trả.
- Quản lý lịch sử chi trả.
- Theo dõi trạng thái thanh toán.

---

### GIS Module

- Hiển thị dữ liệu trên bản đồ.
- Phân tích theo khu vực.
- Thống kê theo địa bàn.

---

### AI Module

- AI Assistant.
- Phân tích dữ liệu.
- Hỗ trợ tìm kiếm.
- Hỗ trợ báo cáo.

---

### Zalo OA Module

- Đồng bộ người dùng.
- Gửi thông báo.
- Tiếp nhận phản ánh.
- Tra cứu thông tin.

---

### Reporting Module

- Dashboard.
- Báo cáo thống kê.
- Báo cáo tổng hợp.
- Xuất Excel/PDF.

---

## 2.7 Stakeholders

Các nhóm người sử dụng hệ thống bao gồm:

| Role | Description |
|------|-------------|
| System Administrator | Quản trị toàn bộ hệ thống |
| Commune Officer | Cán bộ xã quản lý dữ liệu |
| Leadership | Lãnh đạo theo dõi Dashboard và báo cáo |
| Citizen | Người dân sử dụng Zalo OA để tra cứu thông tin |
| AI Coding Agent | Hỗ trợ phát triển hệ thống |
| Developer | Triển khai và bảo trì hệ thống |

---

## 2.8 High-Level Architecture

Hệ thống được xây dựng theo mô hình nhiều lớp (Layered Clean Architecture).

```

Presentation Layer

↓

Application Layer

↓

Domain Layer

↓

Infrastructure Layer

↓

SQL Server

```

Các dịch vụ bên ngoài được tích hợp thông qua Infrastructure Layer:

- AI Service.
- Zalo Official Account.
- GIS Service.
- Email Service (tương lai).
- SMS Service (tương lai).

---

## 2.9 Success Metrics

Dự án được xem là thành công khi đáp ứng các tiêu chí sau:

- Chuẩn hóa 100% dữ liệu an sinh.
- Quản lý đầy đủ hộ gia đình và đối tượng.
- Hỗ trợ trực quan hóa trên bản đồ.
- Tích hợp thành công Zalo Official Account.
- AI hỗ trợ tìm kiếm và phân tích dữ liệu.
- Hệ thống hoạt động ổn định theo kiến trúc Clean Architecture.
- Có khả năng mở rộng mà không cần thay đổi kiến trúc lõi.

---

## 2.10 Phase Summary

Phase 2 cung cấp cái nhìn tổng quan về dự án AnSinhSo Enterprise, bao gồm bối cảnh nghiệp vụ, mục tiêu, phạm vi chức năng và kiến trúc ở mức cao.

Sau khi hoàn thành Phase này, AI Coding Agent hoặc Developer phải hiểu rõ:

- Dự án được xây dựng để giải quyết vấn đề gì.
- Những thành phần nào thuộc phạm vi dự án.
- Các nhóm người sử dụng chính.
- Kiến trúc tổng thể của hệ thống.
- Mục tiêu cuối cùng của dự án.

Phase 2 là nền tảng để tiếp tục đọc **Phase 3 – Enterprise Documentation Suite**, nơi mô tả toàn bộ hệ thống tài liệu và thứ tự sử dụng trong quá trình phát triển.
# Phase 3 – Enterprise Documentation Suite

## 3.1 Overview

AnSinhSo Enterprise sử dụng mô hình **Enterprise Documentation Suite (EDS)** để quản lý toàn bộ tài liệu kiến trúc, tiêu chuẩn kỹ thuật, quy trình phát triển và Sprint.

Mọi AI Coding Agent, Developer và Technical Reviewer phải sử dụng Documentation Suite làm **Single Source of Truth (SSOT)**.

Không được suy diễn ngoài tài liệu.

Không được tự ý thay đổi kiến trúc nếu chưa có phê duyệt.

---

## 3.2 Documentation Philosophy

Documentation Suite được xây dựng theo các nguyên tắc sau.

### Single Source of Truth

Mỗi nội dung chỉ được định nghĩa tại **một tài liệu duy nhất**.

Ví dụ:

- Coding Standards chỉ nằm trong `04_CODING_STANDARDS.md`
- Database Rules chỉ nằm trong `05_DATABASE_RULES.md`
- API Standards chỉ nằm trong `06_API_STANDARDS.md`

AI không được tạo thêm quy tắc mới.

---

### Layered Documentation

Toàn bộ tài liệu được chia thành nhiều tầng.

```

Bootstrap

↓

Architecture

↓

Standards

↓

Development

↓

Sprint

↓

Implementation

```

Mỗi tầng có phạm vi và trách nhiệm riêng.

---

### Version Controlled

Mỗi tài liệu phải có:

- Version
- Status
- Last Updated
- Owner

Ví dụ:

```

Version : 1.0

Status : BASELINE

Owner : Solution Architecture Team

```

---

### Incremental Evolution

Documentation được cập nhật theo từng Sprint.

Không viết lại toàn bộ tài liệu nếu không cần thiết.

Mọi thay đổi phải đảm bảo khả năng truy vết.

---

## 3.3 Documentation Categories

Toàn bộ Documentation Suite được chia thành 6 nhóm chính.

| Category | Purpose |
|----------|----------|
| Bootstrap | Khởi tạo dự án |
| Architecture | Kiến trúc tổng thể |
| Standards | Tiêu chuẩn kỹ thuật |
| Development | Hướng dẫn phát triển |
| Sprint | Quản lý tiến độ |
| Governance | Quản trị tài liệu |

---

# 3.4 Enterprise Documentation Catalog

## Bootstrap Documents

| File | Purpose |
|------|----------|
| 00_PROJECT_BOOTSTRAP.md | AI Entry Point |
| 00_AI_HANDOVER.md | Chuyển giao ngữ cảnh giữa AI |
| 00_PROJECT_INDEX.md | Chỉ mục tài liệu |
| 00_PROJECT_PROGRESS.md | Theo dõi tiến độ |

---

## Project Documents

| File | Purpose |
|------|----------|
| 01_PROJECT_CONTEXT.md | Bối cảnh dự án |
| 02_AI_MEMORY.md | Bộ nhớ làm việc của AI |
| 03_AI_SYSTEM_RULES.md | Quy tắc bắt buộc cho AI |

---

## Standards Documents

| File | Purpose |
|------|----------|
| 04_CODING_STANDARDS.md | Coding Convention |
| 05_DATABASE_RULES.md | Database Constitution |
| 06_API_STANDARDS.md | API Design |
| 07_FRONTEND_STANDARDS.md | Frontend Standards |
| 08_BACKEND_STANDARDS.md | Backend Standards |
| 09_SECURITY_STANDARDS.md | Security Standards |
| 10_DEPLOYMENT_STANDARDS.md | Deployment Standards |
| 11_TESTING_STANDARDS.md | Testing Standards |
| 12_DEVOPS_STANDARDS.md | DevOps Standards |

---

## Development Documents

| File | Purpose |
|------|----------|
| 13_AI_DEVELOPMENT_GUIDE.md | AI Development Workflow |
| 14_PROJECT_STRUCTURE.md | Project Structure |
| 15_CODING_PROMPTS.md | Prompt Library |
| 16_IMPLEMENTATION_ROADMAP.md | Roadmap |
| 17_DEVELOPMENT_SPRINTS.md | Sprint Planning |
| 18_ZALO_OA_INTEGRATION_GUIDE.md | Zalo OA Integration |

---

## Governance Documents

| File | Purpose |
|------|----------|
| 19_BASELINE_REVIEW.md | Baseline Review |
| 20_SECURITY_ARCHITECTURE.md | Security Architecture |
| 21_INFRASTRUCTURE_ARCHITECTURE.md | Infrastructure Architecture |
| 22_AI_EXECUTION_GUIDE.md | AI Execution Guide |
| 23_SPRINT_00_READINESS.md | Sprint Readiness |

---

## 3.5 Documentation Dependency

Documentation có mối quan hệ phụ thuộc như sau.

```

00_PROJECT_BOOTSTRAP

↓

01_PROJECT_CONTEXT

↓

03_AI_SYSTEM_RULES

↓

04~12 Standards

↓

13~18 Development

↓

19~23 Governance

↓

Source Code

```

AI phải tuân thủ đúng thứ tự này.

---

## 3.6 Reading Priority

Khi bắt đầu một Sprint mới.

AI phải đọc theo đúng trình tự.

### Priority 1

```

00_PROJECT_BOOTSTRAP

```

---

### Priority 2

```

01_PROJECT_CONTEXT

02_AI_MEMORY

03_AI_SYSTEM_RULES

```

---

### Priority 3

```

04

↓

12

```

---

### Priority 4

```

13

↓

18

```

---

### Priority 5

```

19

↓

23

```

---

Không được bỏ qua Priority.

---

## 3.7 Conflict Resolution

Nếu hai tài liệu có nội dung mâu thuẫn.

AI phải ưu tiên theo thứ tự.

| Priority | Document Type |
|----------|---------------|
| 1 | Bootstrap |
| 2 | Architecture |
| 3 | Standards |
| 4 | Sprint |
| 5 | Source Code |

Nếu vẫn không xác định được.

AI phải:

- dừng sinh mã nguồn;
- yêu cầu Developer xác nhận;
- không tự suy diễn.

---

## 3.8 Documentation Update Policy

Documentation chỉ được cập nhật khi:

- có Sprint mới;
- thay đổi kiến trúc;
- thay đổi nghiệp vụ;
- thay đổi công nghệ;
- thay đổi Security.

Không cập nhật tài liệu chỉ vì thay đổi nhỏ trong Source Code.

---

## 3.9 Definition of Documentation Complete

Một tài liệu được xem là hoàn chỉnh khi đáp ứng đầy đủ:

- Có Version.
- Có Status.
- Có Owner.
- Có Table of Contents.
- Có các Phase.
- Có Checklist (nếu cần).
- Có liên kết logic với các tài liệu khác.
- Không mâu thuẫn với Documentation Suite.

---

## 3.10 Phase Summary

Enterprise Documentation Suite là nền tảng quản trị tri thức của dự án AnSinhSo Enterprise.

Toàn bộ kiến trúc, quy trình phát triển, tiêu chuẩn kỹ thuật và Sprint đều phải được quản lý thông qua hệ thống tài liệu này.

Mọi AI Coding Agent phải coi Documentation Suite là nguồn thông tin chính thức duy nhất trước khi phân tích, thiết kế hoặc sinh mã nguồn.

Sau khi hoàn thành Phase 3, AI phải hiểu:

- Cấu trúc toàn bộ Documentation Suite.
- Vai trò của từng tài liệu.
- Thứ tự đọc tài liệu.
- Quan hệ phụ thuộc giữa các tài liệu.
- Quy tắc xử lý khi xảy ra mâu thuẫn.
# Phase 4 – Documentation Reading Order

## 4.1 Purpose

Enterprise Documentation Suite bao gồm nhiều tài liệu với các mục đích khác nhau. Để đảm bảo AI Coding Agent và Developer luôn hiểu đúng ngữ cảnh của dự án, mọi tài liệu phải được đọc theo một thứ tự thống nhất.

Việc đọc sai thứ tự có thể dẫn đến:

- Hiểu sai phạm vi Sprint.
- Sinh mã nguồn không đúng kiến trúc.
- Vi phạm Coding Standards.
- Thiếu các yêu cầu về Security.
- Không tuân thủ Development Roadmap.

Do đó, mọi AI Coding Agent bắt buộc phải tuân thủ quy trình đọc tài liệu được định nghĩa trong Phase này.

---

# 4.2 Reading Principles

Việc đọc tài liệu phải tuân theo bốn nguyên tắc sau.

## Principle 1 – Context Before Code

AI phải hiểu đầy đủ bối cảnh dự án trước khi sinh bất kỳ dòng mã nào.

Không được tạo:

- Entity
- Controller
- API
- SQL
- Migration
- Service
- UI

nếu chưa đọc đầy đủ tài liệu bắt buộc.

---

## Principle 2 – Architecture Before Implementation

AI phải hiểu kiến trúc hệ thống trước khi triển khai.

Nếu chưa hiểu:

- Clean Architecture
- Project Structure
- Standards

AI không được phép sinh mã.

---

## Principle 3 – Standards Before Development

Mọi Coding Standards phải được đọc trước khi triển khai.

Điều này giúp đảm bảo:

- Naming Convention
- Folder Structure
- API Format
- Exception Handling
- Logging
- Security

được thống nhất trong toàn bộ hệ thống.

---

## Principle 4 – Sprint Before Coding

AI chỉ được triển khai những gì thuộc Sprint hiện tại.

Không được triển khai trước:

- Sprint sau.
- Tính năng tương lai.
- Module chưa được phê duyệt.

---

# 4.3 Standard Reading Workflow

AI phải đọc tài liệu theo đúng trình tự sau.

```text
Project Bootstrap
        │
        ▼
Project Context
        │
        ▼
AI System Rules
        │
        ▼
Architecture Standards
        │
        ▼
Development Standards
        │
        ▼
Sprint Planning
        │
        ▼
Implementation
```

Không được thay đổi thứ tự này.

---

# 4.4 Mandatory Documents

Đây là các tài liệu bắt buộc phải đọc trong mọi Sprint.

| Priority | File | Mandatory |
|----------|------|-----------|
| 1 | 00_PROJECT_BOOTSTRAP.md | ✅ |
| 2 | 01_PROJECT_CONTEXT.md | ✅ |
| 3 | 03_AI_SYSTEM_RULES.md | ✅ |
| 4 | 04_CODING_STANDARDS.md | ✅ |
| 5 | 05_DATABASE_RULES.md | ✅ |
| 6 | 06_API_STANDARDS.md | ✅ |
| 7 | 08_BACKEND_STANDARDS.md | ✅ |
| 8 | 09_SECURITY_STANDARDS.md | ✅ |
| 9 | 14_PROJECT_STRUCTURE.md | ✅ |
| 10 | 17_DEVELOPMENT_SPRINTS.md | ✅ |

Nếu thiếu bất kỳ tài liệu nào ở trên, AI phải dừng quá trình triển khai và yêu cầu bổ sung.

---

# 4.5 Optional Documents

Các tài liệu sau chỉ cần đọc khi có yêu cầu tương ứng.

| File | Khi nào cần đọc |
|------|----------------|
| 07_FRONTEND_STANDARDS.md | Phát triển Frontend |
| 10_DEPLOYMENT_STANDARDS.md | Triển khai hệ thống |
| 11_TESTING_STANDARDS.md | Viết Test |
| 12_DEVOPS_STANDARDS.md | Thiết lập CI/CD |
| 18_ZALO_OA_INTEGRATION_GUIDE.md | Tích hợp Zalo OA |
| 20_SECURITY_ARCHITECTURE.md | Thiết kế bảo mật |
| 21_INFRASTRUCTURE_ARCHITECTURE.md | Thiết kế hạ tầng |

---

# 4.6 AI Reading Checklist

Trước khi sinh mã nguồn, AI phải tự xác nhận:

- [ ] Đã đọc Project Bootstrap.
- [ ] Đã đọc Project Context.
- [ ] Đã đọc AI System Rules.
- [ ] Đã đọc Coding Standards.
- [ ] Đã đọc Database Rules.
- [ ] Đã đọc API Standards.
- [ ] Đã đọc Backend Standards.
- [ ] Đã đọc Security Standards.
- [ ] Đã đọc Project Structure.
- [ ] Đã đọc Sprint Planning.

Nếu bất kỳ mục nào chưa hoàn thành, AI phải dừng lại.

---

# 4.7 Human Review

Developer cần xác nhận:

- Sprint hiện tại.
- Kiến trúc không thay đổi.
- Không có tài liệu mới chưa được AI đọc.
- Baseline vẫn hợp lệ.

Điều này giúp tránh việc AI làm việc với tài liệu lỗi thời.

---

# 4.8 Traceability

Mỗi Sprint phải có khả năng truy vết ngược về:

```
Source Code
        │
        ▼
Sprint
        │
        ▼
Development Guide
        │
        ▼
Standards
        │
        ▼
Architecture
        │
        ▼
Project Bootstrap
```

Điều này đảm bảo mọi dòng mã đều có cơ sở từ tài liệu.

---

# 4.9 Common Mistakes

AI không được:

- Bỏ qua Bootstrap.
- Đọc Sprint trước Standards.
- Sinh Migration khi chưa đọc Database Rules.
- Tự ý thêm kiến trúc mới.
- Triển khai module ngoài Sprint.

Nếu phát hiện các trường hợp trên, AI phải yêu cầu xác nhận từ Developer.

---

# 4.10 Phase Summary

Phase 4 định nghĩa quy trình đọc tài liệu chuẩn cho toàn bộ dự án.

Sau khi hoàn thành Phase này, AI Coding Agent phải:

- Biết chính xác tài liệu nào cần đọc.
- Biết thứ tự đọc.
- Biết tài liệu nào là bắt buộc.
- Biết khi nào phải dừng để yêu cầu xác nhận.
- Đảm bảo mọi hoạt động triển khai đều dựa trên Documentation Suite thay vì suy diễn.

Phase 4 là nền tảng giúp duy trì tính nhất quán giữa tài liệu, kiến trúc và mã nguồn trong toàn bộ vòng đời của dự án.
# Phase 5 – Architecture Baseline

## 5.1 Purpose

Architecture Baseline xác định các quyết định kiến trúc cốt lõi (Core Architecture Decisions) của dự án AnSinhSo Enterprise.

Mọi AI Coding Agent, Developer và Technical Reviewer phải coi các quyết định trong Phase này là **Baseline v1.0**.

Các quyết định này chỉ được thay đổi khi có Architecture Review chính thức.

---

# 5.2 Architecture Vision

AnSinhSo Enterprise được thiết kế theo định hướng:

- Enterprise Ready
- Cloud Ready
- AI Ready
- GIS Ready
- Zalo OA Ready
- Security by Design
- Clean Architecture
- Domain Driven Design (Lite)
- Modular Monolith (giai đoạn hiện tại)

Kiến trúc phải đảm bảo khả năng mở rộng sang Microservices trong tương lai mà không cần viết lại toàn bộ hệ thống.

---

# 5.3 Architecture Principles

Toàn bộ hệ thống phải tuân thủ các nguyên tắc sau.

## Principle 1 – Separation of Concerns

Mỗi tầng chỉ chịu trách nhiệm cho một nhóm chức năng.

Không được trộn:

- UI
- Business Logic
- Database
- Infrastructure

trong cùng một Layer.

---

## Principle 2 – Dependency Inversion

Business Layer không được phụ thuộc vào Infrastructure.

Dependency phải hướng từ ngoài vào trong.

```

Presentation

↓

Application

↓

Domain

↑

Infrastructure

```

Infrastructure phụ thuộc Domain.

Domain không phụ thuộc Infrastructure.

---

## Principle 3 – Single Responsibility

Mỗi Class chỉ có một trách nhiệm.

Ví dụ:

- Controller chỉ xử lý HTTP.
- Service chỉ xử lý Business Logic.
- Repository chỉ truy cập dữ liệu.

---

## Principle 4 – High Cohesion

Các thành phần cùng Module phải có liên quan chặt chẽ.

Ví dụ:

Household Module

- Household Entity
- Household Service
- Household Repository
- Household API

không được phân tán sang Module khác.

---

## Principle 5 – Low Coupling

Các Module giao tiếp thông qua Interface.

Không gọi trực tiếp Implementation.

---

# 5.4 Approved Architecture

Kiến trúc chính thức của dự án.

```

Presentation Layer

↓

Application Layer

↓

Domain Layer

↓

Infrastructure Layer

↓

SQL Server

```

Các hệ thống bên ngoài:

```

AI Service

↓

Infrastructure

↑

Zalo OA

↓

Infrastructure

↑

GIS

↓

Infrastructure

```

Không được gọi trực tiếp từ Controller.

---

# 5.5 Approved Technology Stack

## Backend

- ASP.NET Core 8

---

## ORM

- Entity Framework Core 8

---

## Database

- SQL Server 2022

---

## Authentication

- JWT Authentication

---

## Authorization

- Role Based Authorization

---

## API Documentation

- Swagger / OpenAPI

---

## Logging

- Serilog

---

## Object Mapping

- AutoMapper

---

## Validation

- FluentValidation

---

## Background Processing

- Hosted Services

---

## Cache

- IMemoryCache

Có thể mở rộng Redis trong tương lai.

---

## GIS

- Leaflet
- OpenStreetMap

---

## AI

- AI Gateway
- AI Provider Abstraction

---

## Messaging

- Zalo Official Account API

---

# 5.6 Architecture Constraints

Các giới hạn bắt buộc.

## MUST

- Clean Architecture.
- Repository Pattern.
- Dependency Injection.
- DTO.
- Async Programming.
- REST API.
- HTTPS.

---

## SHOULD

- CQRS Ready.
- Event Driven Ready.
- Redis Ready.
- Docker Ready.

---

## MUST NOT

Không được:

- SQL trong Controller.
- EF Core trong Controller.
- Business Logic trong Controller.
- Static Service.
- Hard Code Connection String.
- Hard Code Secret.

---

# 5.7 Architecture Decision Records (ADR)

## ADR-001

Architecture

Clean Architecture

Status

Approved

---

## ADR-002

Database

SQL Server 2022

Status

Approved

---

## ADR-003

ORM

Entity Framework Core

Status

Approved

---

## ADR-004

Authentication

JWT

Status

Approved

---

## ADR-005

GIS

Leaflet + OpenStreetMap

Status

Approved

---

## ADR-006

Messaging

Zalo Official Account

Status

Approved

---

## ADR-007

AI

AI Provider Abstraction

Status

Approved

---

## ADR-008

Deployment

Windows Server + IIS

Status

Approved

---

# 5.8 Change Management

Mọi thay đổi kiến trúc phải:

1. Phân tích tác động.
2. Cập nhật Documentation Suite.
3. Được Architecture Review.
4. Được phê duyệt.
5. Cập nhật Baseline Version.

Không được thay đổi trực tiếp trong Source Code.

---

# 5.9 Architecture Freeze

Các thành phần sau được đóng băng trong Baseline v1.0.

- Kiến trúc Clean Architecture.
- SQL Server.
- Entity Framework Core.
- JWT Authentication.
- REST API.
- Serilog.
- Leaflet.
- OpenStreetMap.
- Zalo Official Account.
- AI Gateway Pattern.

Không được thay đổi nếu chưa có Architecture Review.

---

# 5.10 Phase Summary

Architecture Baseline là nền tảng kỹ thuật của toàn bộ dự án.

Sau khi hoàn thành Phase này:

- AI phải triển khai đúng kiến trúc đã được phê duyệt.
- Developer không được tự ý thay đổi công nghệ cốt lõi.
- Mọi thay đổi kiến trúc đều phải được quản lý thông qua Architecture Decision Records (ADR).

Phase 5 đánh dấu việc "đóng băng" các quyết định kiến trúc của Baseline v1.0, tạo nền tảng ổn định cho tất cả các Sprint tiếp theo.
# Phase 6 – Technology Stack Governance

## 6.1 Purpose

Technology Stack Governance định nghĩa tập hợp các công nghệ được phê duyệt (Approved Technology Stack) để sử dụng trong dự án AnSinhSo Enterprise.

Mục tiêu của Phase này là:

- Chuẩn hóa công nghệ sử dụng trong toàn bộ dự án.
- Đảm bảo tính nhất quán giữa các Sprint.
- Giảm rủi ro khi tích hợp nhiều thành phần.
- Hỗ trợ AI Coding Agent sinh mã đúng với công nghệ đã được phê duyệt.
- Hạn chế việc sử dụng thư viện hoặc framework không được kiểm soát.

---

# 6.2 Technology Governance Principles

Toàn bộ công nghệ sử dụng trong dự án phải tuân thủ các nguyên tắc sau.

## Principle 1 – Stability First

Ưu tiên sử dụng các công nghệ đã ổn định, có cộng đồng lớn và được hỗ trợ lâu dài (Long-Term Support).

---

## Principle 2 – Enterprise Ready

Chỉ lựa chọn các công nghệ phù hợp với môi trường doanh nghiệp và có khả năng mở rộng.

---

## Principle 3 – Security by Default

Mọi thư viện và framework phải đáp ứng yêu cầu về bảo mật, được cập nhật thường xuyên và không chứa lỗ hổng nghiêm trọng đã biết.

---

## Principle 4 – AI Friendly

Công nghệ được lựa chọn phải có tài liệu đầy đủ, phổ biến và được các AI Coding Agent hỗ trợ tốt.

---

## Principle 5 – Minimal Dependencies

Chỉ cài đặt thư viện khi thật sự cần thiết.

Ưu tiên sử dụng thư viện chính thức của Microsoft hoặc các thư viện đã được cộng đồng kiểm chứng.

---

# 6.3 Approved Technology Matrix

## Backend

| Component | Technology | Status |
|----------|------------|--------|
| Framework | ASP.NET Core 8 | Approved |
| Runtime | .NET 8 LTS | Approved |
| Language | C# 12 | Approved |

---

## Database

| Component | Technology | Status |
|----------|------------|--------|
| Database Engine | SQL Server 2022 | Approved |
| ORM | Entity Framework Core 8 | Approved |
| Migration | EF Core Migration | Approved |

---

## Authentication

| Component | Technology | Status |
|----------|------------|--------|
| Authentication | JWT Bearer | Approved |
| Authorization | Role-Based Authorization | Approved |
| Password Hashing | ASP.NET Identity Password Hasher | Approved |

---

## API

| Component | Technology | Status |
|----------|------------|--------|
| API Style | RESTful API | Approved |
| Documentation | Swagger / OpenAPI | Approved |
| Response Format | Standard Response Wrapper | Approved |

---

## Frontend

| Component | Technology | Status |
|----------|------------|--------|
| HTML | HTML5 | Approved |
| CSS | Tailwind CSS | Approved |
| JavaScript | ES6+ | Approved |
| Map Library | Leaflet | Approved |

---

## GIS

| Component | Technology | Status |
|----------|------------|--------|
| Base Map | OpenStreetMap | Approved |
| GIS Engine | Leaflet | Approved |
| Coordinate System | WGS84 | Approved |

---

## AI

| Component | Technology | Status |
|----------|------------|--------|
| AI Gateway | Provider Abstraction | Approved |
| AI Provider | Configurable | Approved |
| Prompt Management | Prompt Library | Approved |

---

## Messaging

| Component | Technology | Status |
|----------|------------|--------|
| Official Channel | Zalo Official Account | Approved |
| Integration | Zalo OA API | Approved |

---

## Logging

| Component | Technology | Status |
|----------|------------|--------|
| Structured Logging | Serilog | Approved |

---

## Validation

| Component | Technology | Status |
|----------|------------|--------|
| Validation | FluentValidation | Approved |

---

## Object Mapping

| Component | Technology | Status |
|----------|------------|--------|
| Mapper | AutoMapper | Approved |

---

## Testing

| Component | Technology | Status |
|----------|------------|--------|
| Unit Testing | xUnit | Approved |
| Mocking | Moq | Approved |

---

# 6.4 Planned Technologies

Các công nghệ sau chưa triển khai trong Baseline v1.0 nhưng đã được xem xét cho các Sprint tương lai.

| Technology | Planned Sprint |
|------------|----------------|
| Redis Cache | Sprint 04 |
| Docker | Sprint 05 |
| SignalR | Sprint 06 |
| Hangfire | Sprint 06 |
| Elasticsearch | Future |
| Kubernetes | Future |

Không được triển khai các công nghệ này nếu chưa có kế hoạch chính thức.

---

# 6.5 Deprecated Technologies

Các công nghệ sau không được sử dụng trong dự án.

| Technology | Reason |
|------------|--------|
| ASP.NET MVC 5 | Không còn phù hợp với .NET 8 |
| WebForms | Deprecated |
| Entity Framework 6 | Không tương thích với EF Core 8 |
| jQuery cho logic chính | Không cần thiết |
| SOAP Services | Không phù hợp kiến trúc REST |
| FTP Deployment | Không đảm bảo bảo mật |

---

# 6.6 Third-Party Library Policy

Khi cần bổ sung thư viện bên thứ ba, phải đáp ứng các tiêu chí:

- Có giấy phép sử dụng rõ ràng.
- Được cộng đồng sử dụng rộng rãi.
- Có tài liệu chính thức.
- Có lịch sử cập nhật thường xuyên.
- Không có lỗ hổng bảo mật nghiêm trọng.

Mọi thư viện mới phải được xem xét trước khi đưa vào dự án.

---

# 6.7 Version Management

Các phiên bản công nghệ phải được quản lý tập trung.

Ví dụ:

| Component | Version |
|-----------|---------|
| .NET | 8.x |
| EF Core | 8.x |
| SQL Server | 2022 |
| Tailwind CSS | Stable |
| Leaflet | Stable |
| Serilog | Stable |

Không tự ý nâng cấp Major Version trong quá trình phát triển nếu chưa đánh giá tác động.

---

# 6.8 Technology Change Process

Khi cần thay đổi công nghệ:

1. Phân tích nhu cầu.
2. Đánh giá tác động.
3. Cập nhật tài liệu.
4. Thực hiện Proof of Concept (nếu cần).
5. Được phê duyệt bởi Solution Architect.
6. Cập nhật Baseline.

Không thay đổi trực tiếp trong Source Code.

---

# 6.9 AI Coding Requirements

AI Coding Agent phải:

- Chỉ sử dụng công nghệ trong danh sách Approved.
- Không tự động thêm Package hoặc Framework mới.
- Không thay đổi phiên bản công nghệ.
- Báo cáo nếu phát hiện xung đột giữa tài liệu và môi trường triển khai.

---

# 6.10 Phase Summary

Technology Stack Governance đảm bảo toàn bộ dự án AnSinhSo Enterprise sử dụng một tập hợp công nghệ thống nhất, ổn định và có khả năng mở rộng.

Sau khi hoàn thành Phase này, AI và Developer phải:

- Biết công nghệ nào được phép sử dụng.
- Biết công nghệ nào chưa được phép triển khai.
- Tuân thủ quy trình thay đổi công nghệ.
- Đảm bảo mọi Sprint đều sử dụng cùng một Technology Baseline.

Phase 6 là nền tảng giúp duy trì tính nhất quán kỹ thuật và giảm rủi ro trong suốt vòng đời phát triển của dự án.
# Phase 7 – Development Workflow

## 7.1 Purpose

Development Workflow định nghĩa quy trình phát triển chuẩn của dự án AnSinhSo Enterprise.

Mục tiêu của Phase này là:

- Chuẩn hóa cách AI và Developer phối hợp.
- Đảm bảo mọi Sprint đều tuân theo cùng một quy trình.
- Kiểm soát chất lượng mã nguồn.
- Giảm rủi ro phát sinh khi nhiều AI hoặc nhiều Developer cùng tham gia.

Development Workflow áp dụng cho toàn bộ vòng đời của dự án, từ Sprint 00 đến Production.

---

# 7.2 Development Philosophy

AnSinhSo Enterprise được phát triển theo các nguyên tắc sau:

- Documentation First
- Architecture First
- Security by Design
- AI Assisted Development
- Incremental Delivery
- Continuous Validation
- Quality over Speed

Mọi Sprint phải tạo ra giá trị có thể kiểm chứng được thay vì chỉ hoàn thành mã nguồn.

---

# 7.3 Development Lifecycle

Toàn bộ dự án tuân theo quy trình sau:

```text
Planning
      │
      ▼
Documentation Review
      │
      ▼
Sprint Planning
      │
      ▼
Implementation
      │
      ▼
Code Review
      │
      ▼
Testing
      │
      ▼
Acceptance Review
      │
      ▼
Baseline Update
      │
      ▼
Next Sprint
```

Không được bỏ qua bất kỳ bước nào.

---

# 7.4 Sprint Workflow

Mỗi Sprint phải trải qua các giai đoạn sau:

### Step 1 – Sprint Review

- Đọc mục tiêu Sprint.
- Kiểm tra Dependencies.
- Xác nhận Baseline hiện tại.

---

### Step 2 – Documentation Review

AI phải đọc:

- Project Bootstrap
- Project Context
- AI Rules
- Standards
- Sprint Documents

---

### Step 3 – Task Breakdown

Phân rã Sprint thành các Task nhỏ.

Ví dụ:

Sprint 01

↓

Solution

↓

Projects

↓

References

↓

Configuration

↓

Middleware

↓

Swagger

↓

Health Check

---

### Step 4 – Implementation

Triển khai từng Task.

Không triển khai đồng thời nhiều Module lớn.

---

### Step 5 – Validation

Kiểm tra:

- Build thành công.
- Không có Warning nghiêm trọng.
- Coding Standards được tuân thủ.
- Security Standards được tuân thủ.

---

### Step 6 – Review

Developer hoặc AI Reviewer thực hiện:

- Architecture Review
- Code Review
- Dependency Review
- Security Review

---

### Step 7 – Sprint Closure

Sau khi hoàn thành:

- Cập nhật Progress.
- Cập nhật Baseline.
- Chuẩn bị Sprint tiếp theo.

---

# 7.5 AI Collaboration Workflow

AI Coding Agent phải làm việc theo quy trình:

```text
Read Documentation
        │
        ▼
Understand Context
        │
        ▼
Analyze Sprint
        │
        ▼
Generate Plan
        │
        ▼
Wait For Approval
        │
        ▼
Generate Code
        │
        ▼
Self Validation
        │
        ▼
Developer Review
```

AI không được bỏ qua bước **Wait For Approval**.

---

# 7.6 Human Responsibilities

Developer chịu trách nhiệm:

- Phê duyệt Sprint.
- Xác nhận yêu cầu nghiệp vụ.
- Review mã nguồn.
- Merge vào nhánh chính.
- Cập nhật Documentation khi cần.

AI không thay thế vai trò quyết định của Developer.

---

# 7.7 AI Responsibilities

AI Coding Agent chịu trách nhiệm:

- Đọc Documentation Suite.
- Tuân thủ Coding Standards.
- Sinh mã đúng Sprint.
- Không tự ý mở rộng phạm vi.
- Đề xuất cải tiến khi cần.
- Báo cáo các điểm chưa rõ thay vì tự suy diễn.

---

# 7.8 Quality Gates

Một Sprint chỉ được xem là hoàn thành khi vượt qua tất cả các Quality Gate sau:

| Gate | Requirement |
|------|-------------|
| Build | Thành công |
| Architecture | Đúng Clean Architecture |
| Standards | Tuân thủ Standards |
| Security | Không có lỗi nghiêm trọng |
| Testing | Đạt tiêu chí Sprint |
| Documentation | Được cập nhật nếu cần |

Nếu một Quality Gate không đạt, Sprint chưa được đóng.

---

# 7.9 Deliverables

Mỗi Sprint phải tạo ra các Deliverable rõ ràng.

Ví dụ:

| Sprint | Deliverables |
|---------|--------------|
| Sprint 01 | Solution, Projects, Middleware |
| Sprint 02 | Authentication, Authorization |
| Sprint 03 | Household Management |
| Sprint 04 | Social Welfare Management |
| Sprint 05 | GIS Integration |
| Sprint 06 | AI Integration |
| Sprint 07 | Zalo OA Integration |
| Sprint 08 | Dashboard & Reporting |
| Sprint 09 | Testing & Optimization |
| Sprint 10 | Production Deployment |

Mỗi Deliverable phải có khả năng kiểm chứng.

---

# 7.10 Phase Summary

Development Workflow là quy trình vận hành chuẩn của dự án AnSinhSo Enterprise.

Sau khi hoàn thành Phase này, AI và Developer phải:

- Hiểu toàn bộ vòng đời của một Sprint.
- Biết vai trò và trách nhiệm của từng bên.
- Tuân thủ Quality Gates trước khi kết thúc Sprint.
- Đảm bảo mọi thay đổi đều được kiểm soát và có thể truy vết.

Development Workflow là cầu nối giữa Documentation Suite và quá trình triển khai thực tế, giúp dự án phát triển có hệ thống và bền vững.
# Phase 8 – AI Operating Model

## 8.1 Purpose

AnSinhSo Enterprise được phát triển theo mô hình **AI-Assisted Software Engineering**, trong đó AI đóng vai trò là cộng sự kỹ thuật (Engineering Assistant), hỗ trợ Developer trong toàn bộ vòng đời phát triển phần mềm.

AI không thay thế vai trò quyết định của con người. Mọi quyết định về kiến trúc, nghiệp vụ và triển khai cuối cùng đều thuộc về Project Owner hoặc Solution Architect.

Mục tiêu của AI Operating Model là:

- Chuẩn hóa cách AI tham gia vào dự án.
- Đảm bảo nhiều AI có thể phối hợp mà không làm mất ngữ cảnh.
- Kiểm soát chất lượng đầu ra.
- Giảm rủi ro AI tự ý thay đổi kiến trúc hoặc nghiệp vụ.

---

# 8.2 AI Roles

Trong dự án AnSinhSo Enterprise, AI có thể đảm nhiệm nhiều vai trò khác nhau tùy theo giai đoạn phát triển.

| AI Role | Responsibility |
|---------|----------------|
| Solution Architect | Phân tích kiến trúc, đề xuất giải pháp, review thiết kế |
| Backend Developer | Sinh mã nguồn Backend theo Sprint |
| Frontend Developer | Sinh giao diện và tích hợp API |
| Database Engineer | Thiết kế Entity, Migration, SQL, Index |
| Security Reviewer | Kiểm tra yêu cầu bảo mật và đề xuất cải tiến |
| Code Reviewer | Rà soát chất lượng mã nguồn |
| QA Assistant | Hỗ trợ xây dựng Test Case và kiểm thử |
| Technical Writer | Cập nhật Documentation Suite |

Một AI có thể đảm nhiệm nhiều vai trò, nhưng trong từng phiên làm việc chỉ nên tập trung vào **một vai trò chính** để đảm bảo tính nhất quán.

---

# 8.3 AI Collaboration Model

Dự án sử dụng mô hình cộng tác nhiều AI với vai trò bổ trợ cho nhau.

### ChatGPT

Vai trò chính:

- Thiết kế kiến trúc.
- Xây dựng Documentation Suite.
- Phân tích nghiệp vụ.
- Thiết kế Database.
- Thiết kế API.
- Review giải pháp.
- Hỗ trợ ra quyết định kỹ thuật.

---

### Antigravity IDE

Vai trò chính:

- Sinh mã nguồn.
- Refactor.
- Tạo Project.
- Tạo Entity.
- Viết Controller.
- Viết Service.
- Viết Middleware.
- Viết Unit Test.
- Hỗ trợ triển khai Sprint.

---

### Developer

Vai trò chính:

- Xác nhận yêu cầu.
- Phê duyệt thay đổi.
- Review mã nguồn.
- Merge Source Code.
- Quyết định các thay đổi kiến trúc.

---

# 8.4 AI Collaboration Workflow

```text
Documentation Suite
        │
        ▼
ChatGPT
(Analysis & Planning)
        │
        ▼
00_AI_HANDOVER.md
        │
        ▼
Antigravity IDE
(Code Generation)
        │
        ▼
Developer Review
        │
        ▼
Git Commit
        │
        ▼
Next Sprint
```

Mọi quá trình chuyển giao giữa ChatGPT và Antigravity IDE phải thông qua tài liệu `00_AI_HANDOVER.md`.

---

# 8.5 AI Decision Boundaries

AI được phép:

- Phân tích tài liệu.
- Đề xuất kiến trúc.
- Sinh mã nguồn.
- Refactor.
- Tạo Test Case.
- Tối ưu hiệu năng.
- Đề xuất cải tiến.

AI **không được phép**:

- Thay đổi nghiệp vụ đã được phê duyệt.
- Thay đổi Baseline.
- Thay đổi công nghệ cốt lõi.
- Xóa dữ liệu.
- Thực hiện hành động triển khai Production mà không có xác nhận.

---

# 8.6 AI Approval Matrix

| Action | AI Can Execute | Human Approval Required |
|---------|----------------|-------------------------|
| Generate Code | Yes | No |
| Refactor Code | Yes | Recommended |
| Create Database Migration | Yes | Yes |
| Change Database Schema | No | Yes |
| Change Architecture | No | Yes |
| Add New Technology | No | Yes |
| Modify Security Policy | No | Yes |
| Production Deployment | No | Yes |

---

# 8.7 AI Context Management

Trước mỗi Sprint, AI phải xác nhận:

- Đã đọc `00_PROJECT_BOOTSTRAP.md`.
- Đã đọc `00_AI_HANDOVER.md`.
- Đã đọc `17_DEVELOPMENT_SPRINTS.md`.
- Đã đọc tài liệu Standards liên quan.
- Đã xác định đúng Sprint hiện tại.

Nếu thiếu bất kỳ tài liệu nào, AI phải yêu cầu bổ sung trước khi tiếp tục.

---

# 8.8 AI Quality Requirements

Mọi đầu ra do AI tạo phải đáp ứng:

- Tuân thủ Clean Architecture.
- Tuân thủ Coding Standards.
- Không tạo mã ngoài phạm vi Sprint.
- Có khả năng build.
- Có khả năng kiểm thử.
- Có khả năng mở rộng.
- Không chứa thông tin bí mật được mã hóa cứng (hard-coded secrets).

---

# 8.9 AI Escalation Policy

AI phải dừng và yêu cầu Developer xác nhận khi gặp các tình huống sau:

- Thiếu thông tin nghiệp vụ.
- Tài liệu mâu thuẫn.
- Yêu cầu thay đổi kiến trúc.
- Yêu cầu thay đổi Database Schema.
- Thay đổi Security Policy.
- Tích hợp công nghệ chưa được phê duyệt.
- Không xác định được Sprint hoặc phạm vi công việc.

AI không được tự suy diễn để tiếp tục triển khai.

---

# 8.10 Phase Summary

AI Operating Model xác định vai trò, quyền hạn và trách nhiệm của AI trong dự án AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- AI hiểu rõ vai trò của mình trong từng Sprint.
- Developer kiểm soát được các thay đổi quan trọng.
- ChatGPT và Antigravity IDE có cơ chế phối hợp thống nhất.
- Toàn bộ hoạt động của AI đều dựa trên Documentation Suite và quy trình đã được phê duyệt.

Phase 8 là nền tảng giúp dự án áp dụng AI một cách có kiểm soát, minh bạch và phù hợp với quy trình phát triển phần mềm theo chuẩn Enterprise.
# Phase 9 – Project Governance

## 9.1 Purpose

Project Governance định nghĩa mô hình quản trị dự án AnSinhSo Enterprise nhằm đảm bảo mọi hoạt động phát triển đều được thực hiện một cách minh bạch, có thể kiểm soát, có khả năng truy vết và phù hợp với Documentation Suite.

Governance không nhằm tăng thủ tục hành chính mà nhằm:

- Đảm bảo tính nhất quán giữa tài liệu và mã nguồn.
- Kiểm soát thay đổi.
- Hạn chế rủi ro do AI hoặc Developer tự ý thay đổi kiến trúc.
- Thiết lập cơ chế ra quyết định rõ ràng.

---

# 9.2 Governance Principles

Toàn bộ dự án phải tuân thủ các nguyên tắc sau.

## Principle 1 – Documentation First

Documentation là nguồn thông tin chính thức.

Nếu Source Code và Documentation khác nhau thì phải:

- xem xét nguyên nhân;
- cập nhật tài liệu hoặc mã nguồn;
- không tự ý chọn một bên.

---

## Principle 2 – Architecture Stability

Kiến trúc chỉ được thay đổi sau khi:

- đánh giá tác động;
- cập nhật tài liệu;
- được phê duyệt.

---

## Principle 3 – Incremental Delivery

Dự án được phát triển theo Sprint.

Không triển khai toàn bộ hệ thống trong một lần.

---

## Principle 4 – Traceability

Mọi thay đổi đều phải truy vết được.

Ví dụ:

Requirement

↓

Documentation

↓

Sprint

↓

Source Code

↓

Test Case

↓

Release

---

## Principle 5 – Continuous Improvement

Documentation và Source Code được cải tiến liên tục nhưng phải đảm bảo không phá vỡ Baseline hiện tại.

---

# 9.3 Governance Roles

| Role | Responsibility |
|------|----------------|
| Project Owner | Quyết định cuối cùng về nghiệp vụ |
| Solution Architect | Phê duyệt kiến trúc |
| AI Coding Agent | Sinh mã theo tài liệu |
| Developer | Triển khai và Review |
| Technical Reviewer | Kiểm tra chất lượng |
| Tester | Kiểm thử chức năng |

---

# 9.4 RACI Matrix

| Activity | Owner | Architect | AI | Developer | Reviewer |
|-----------|------|-----------|----|------------|----------|
| Documentation | A | R | C | C | C |
| Architecture | C | A | C | R | C |
| Coding | C | C | R | A | C |
| Testing | C | C | C | R | A |
| Deployment | A | C | I | R | C |

Trong đó:

- **R (Responsible):** Thực hiện.
- **A (Accountable):** Chịu trách nhiệm cuối cùng.
- **C (Consulted):** Được tham vấn.
- **I (Informed):** Được thông báo.

---

# 9.5 Change Management

Mọi thay đổi phải đi theo quy trình:

```text
Request
      │
      ▼
Impact Analysis
      │
      ▼
Architecture Review
      │
      ▼
Documentation Update
      │
      ▼
Implementation
      │
      ▼
Testing
      │
      ▼
Baseline Update
```

Không được thay đổi trực tiếp trong Source Code mà không cập nhật Documentation.

---

# 9.6 Configuration Management

Các thành phần sau phải được quản lý tập trung:

- Source Code
- Documentation
- SQL Scripts
- Configuration
- API Contracts
- Environment Settings
- Secrets

Không lưu thông tin nhạy cảm trong Git Repository.

---

# 9.7 Baseline Governance

Dự án sử dụng các mức Baseline sau:

| Baseline | Description |
|-----------|-------------|
| Baseline 0.1 | Draft |
| Baseline 0.5 | Internal Review |
| Baseline 1.0 | Sprint 00 Approved |
| Baseline 1.x | Sprint Updates |
| Baseline 2.0 | Production Release |

Mọi thay đổi phải ghi rõ phiên bản Baseline.

---

# 9.8 Risk Management

Các nhóm rủi ro chính:

### Technical Risks

- Sai kiến trúc.
- Sai Database.
- Sai API.

---

### Business Risks

- Thay đổi nghiệp vụ.
- Thiếu dữ liệu.

---

### AI Risks

- AI suy diễn.
- AI sinh mã vượt Sprint.
- AI thay đổi Baseline.

---

### Operational Risks

- Mất dữ liệu.
- Lỗi triển khai.
- Sai cấu hình.

Mỗi rủi ro cần có kế hoạch giảm thiểu (Mitigation Plan).

---

# 9.9 Governance Checklist

Trước khi kết thúc Sprint cần xác nhận:

- [ ] Documentation đã cập nhật.
- [ ] Source Code đúng Standards.
- [ ] Không thay đổi Architecture.
- [ ] Security Review hoàn thành.
- [ ] Build thành công.
- [ ] Test đạt yêu cầu.
- [ ] Baseline được cập nhật.

---

# 9.10 Phase Summary

Project Governance thiết lập cơ chế quản trị cho toàn bộ dự án AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- AI và Developer hiểu rõ trách nhiệm của mình.
- Mọi thay đổi đều có quy trình kiểm soát.
- Dự án có khả năng truy vết từ Requirement đến Release.
- Documentation và Source Code luôn đồng bộ.

Phase 9 tạo nền tảng quản trị giúp dự án phát triển ổn định và có thể mở rộng trong dài hạn.
# Phase 10 – Bootstrap Readiness Checklist

## 10.1 Purpose

Bootstrap Readiness Checklist xác nhận rằng dự án AnSinhSo Enterprise đã sẵn sàng bước vào giai đoạn triển khai kỹ thuật (Implementation Phase).

Phase này đóng vai trò là **Go / No-Go Decision Gate**, giúp Project Owner, Solution Architect, Developer và AI Coding Agent thống nhất rằng toàn bộ nền tảng của dự án đã được chuẩn bị đầy đủ.

Mọi Sprint chỉ được bắt đầu khi các tiêu chí trong Phase này đạt trạng thái **PASS**.

---

# 10.2 Readiness Categories

Việc đánh giá mức độ sẵn sàng được chia thành các nhóm sau:

- Documentation Readiness
- Architecture Readiness
- Technology Readiness
- Development Readiness
- Infrastructure Readiness
- Security Readiness
- AI Readiness
- Sprint Readiness

Mỗi nhóm phải được đánh giá độc lập.

---

# 10.3 Documentation Readiness

| Checklist | Status |
|-----------|--------|
| Project Bootstrap hoàn chỉnh | ☐ |
| Project Context hoàn chỉnh | ☐ |
| AI System Rules hoàn chỉnh | ☐ |
| Coding Standards hoàn chỉnh | ☐ |
| Database Rules hoàn chỉnh | ☐ |
| API Standards hoàn chỉnh | ☐ |
| Backend Standards hoàn chỉnh | ☐ |
| Security Standards hoàn chỉnh | ☐ |
| Development Guide hoàn chỉnh | ☐ |
| Sprint Planning hoàn chỉnh | ☐ |

**Acceptance Criteria**

- Không còn tài liệu ở trạng thái Placeholder.
- Không còn liên kết sai.
- Không còn tên file không thống nhất.
- Documentation Suite được Baseline.

---

# 10.4 Architecture Readiness

| Checklist | Status |
|-----------|--------|
| Clean Architecture được phê duyệt | ☐ |
| Project Structure được xác nhận | ☐ |
| Dependency Rules được xác nhận | ☐ |
| Layer Responsibilities được xác nhận | ☐ |
| ADR được Baseline | ☐ |

**Acceptance Criteria**

- Không còn quyết định kiến trúc đang chờ xử lý.
- Không còn mâu thuẫn giữa các tài liệu.

---

# 10.5 Technology Readiness

| Checklist | Status |
|-----------|--------|
| .NET 8 | ☐ |
| SQL Server 2022 | ☐ |
| EF Core 8 | ☐ |
| Swagger | ☐ |
| Serilog | ☐ |
| Tailwind CSS | ☐ |
| Leaflet | ☐ |
| Zalo OA Integration Plan | ☐ |

---

# 10.6 Development Readiness

| Checklist | Status |
|-----------|--------|
| Sprint Roadmap được xác nhận | ☐ |
| Coding Workflow được xác nhận | ☐ |
| Git Strategy được xác nhận | ☐ |
| Branch Strategy được xác nhận | ☐ |
| Naming Convention được xác nhận | ☐ |

---

# 10.7 Infrastructure Readiness

| Checklist | Status |
|-----------|--------|
| SQL Server Environment | ☐ |
| Development Environment | ☐ |
| IIS Deployment Plan | ☐ |
| Backup Strategy | ☐ |
| Logging Strategy | ☐ |

---

# 10.8 Security Readiness

| Checklist | Status |
|-----------|--------|
| JWT Strategy | ☐ |
| Secret Management | ☐ |
| Role-Based Authorization | ☐ |
| Audit Logging | ☐ |
| Security Standards | ☐ |

---

# 10.9 AI Readiness

| Checklist | Status |
|-----------|--------|
| AI Handover hoàn chỉnh | ☐ |
| AI Execution Guide hoàn chỉnh | ☐ |
| AI Roles được xác định | ☐ |
| AI Context được đồng bộ | ☐ |
| AI Operating Model hoàn chỉnh | ☐ |

---

# 10.10 Bootstrap Decision

Sau khi hoàn thành toàn bộ Checklist:

| Result | Action |
|---------|--------|
| PASS | Chuyển sang Sprint 01 |
| CONDITIONAL PASS | Hoàn thành các mục còn thiếu trước khi bắt đầu Sprint |
| FAIL | Không được phép triển khai |

Nếu kết quả là **PASS**, Documentation Suite chính thức trở thành nền tảng cho toàn bộ quá trình phát triển.

---

# Phase 10 Summary

Bootstrap Readiness Checklist là bước xác nhận cuối cùng trước khi triển khai dự án.

Sau khi hoàn thành Phase này:

- Documentation đã sẵn sàng.
- Kiến trúc đã được Baseline.
- Công nghệ đã được phê duyệt.
- AI đã có đầy đủ ngữ cảnh.
- Dự án đủ điều kiện chuyển sang Sprint 01.

Phase 10 là cổng kiểm soát chất lượng cuối cùng của giai đoạn khởi tạo (Project Bootstrap).
# Phase 11 – Definition of Ready (DoR) & Definition of Done (DoD)

## 11.1 Purpose

Definition of Ready (DoR) và Definition of Done (DoD) thiết lập các tiêu chí thống nhất để xác định:

- Khi nào một Sprint được phép bắt đầu.
- Khi nào một Sprint được xem là hoàn thành.
- Khi nào AI được phép sinh mã nguồn.
- Khi nào Developer được phép đóng Sprint.

Mục tiêu của Phase này là:

- Giảm hiểu nhầm giữa AI và Developer.
- Chuẩn hóa tiêu chí đánh giá.
- Nâng cao chất lượng sản phẩm.
- Đảm bảo mọi Sprint đều tạo ra giá trị có thể kiểm chứng.

---

# 11.2 Definition of Ready (DoR)

Một Sprint chỉ được phép bắt đầu khi tất cả các tiêu chí sau đều đạt trạng thái **PASS**.

## Business Readiness

- [ ] Mục tiêu Sprint đã được xác định.
- [ ] Phạm vi Sprint rõ ràng.
- [ ] Không còn yêu cầu nghiệp vụ mâu thuẫn.
- [ ] Product Owner đã xác nhận.

---

## Documentation Readiness

- [ ] Documentation Suite đã cập nhật.
- [ ] Project Bootstrap đã Baseline.
- [ ] Sprint Document đã hoàn chỉnh.
- [ ] AI Handover đã cập nhật.
- [ ] Không còn Placeholder.

---

## Architecture Readiness

- [ ] Không có thay đổi kiến trúc đang chờ xử lý.
- [ ] ADR đã được phê duyệt.
- [ ] Dependency Diagram đã xác nhận.
- [ ] Project Structure ổn định.

---

## Technical Readiness

- [ ] Development Environment hoạt động.
- [ ] Solution Build thành công.
- [ ] Database Environment sẵn sàng.
- [ ] Git Repository sẵn sàng.

---

## AI Readiness

- [ ] AI đã đọc Bootstrap.
- [ ] AI đã đọc Sprint.
- [ ] AI đã đọc Standards.
- [ ] AI đã xác định đúng phạm vi Sprint.

---

# 11.3 AI Ready Checklist

Trước khi sinh mã nguồn, AI phải tự xác nhận:

- [ ] Tôi hiểu mục tiêu Sprint.
- [ ] Tôi hiểu kiến trúc hệ thống.
- [ ] Tôi chỉ triển khai trong phạm vi Sprint.
- [ ] Tôi không thay đổi Baseline.
- [ ] Tôi sẽ tuân thủ Documentation Suite.

Nếu bất kỳ câu trả lời nào là **No**, AI phải dừng và yêu cầu xác nhận.

---

# 11.4 Definition of Done (DoD)

Một Sprint chỉ được xem là hoàn thành khi toàn bộ tiêu chí sau đạt trạng thái **PASS**.

## Source Code

- [ ] Build thành công.
- [ ] Không có lỗi biên dịch.
- [ ] Không có Warning nghiêm trọng.
- [ ] Coding Standards được tuân thủ.

---

## Architecture

- [ ] Đúng Clean Architecture.
- [ ] Không vi phạm Dependency Rules.
- [ ] Không phát sinh Technical Debt nghiêm trọng.

---

## Database

- [ ] Migration hoạt động.
- [ ] Không làm mất dữ liệu.
- [ ] Database Rules được tuân thủ.

---

## API

- [ ] API đúng chuẩn REST.
- [ ] Swagger cập nhật.
- [ ] Response đúng Standard Format.
- [ ] HTTP Status Code chính xác.

---

## Security

- [ ] Authentication hoạt động.
- [ ] Authorization hoạt động.
- [ ] Không có Hard-coded Secret.
- [ ] Logging đầy đủ.

---

## Testing

- [ ] Unit Test đạt yêu cầu.
- [ ] Integration Test đạt yêu cầu (nếu áp dụng).
- [ ] Manual Test hoàn thành.
- [ ] Không còn lỗi mức Critical.

---

## Documentation

- [ ] Documentation đã cập nhật.
- [ ] AI Handover đã cập nhật.
- [ ] Sprint Progress đã cập nhật.
- [ ] Baseline Review đã cập nhật (nếu có thay đổi).

---

# 11.5 Sprint Acceptance Criteria

Sprint được chấp nhận khi:

- Mục tiêu Sprint đã hoàn thành.
- Chức năng hoạt động đúng.
- Không có lỗi Critical hoặc High.
- Documentation đồng bộ với Source Code.
- Được Developer hoặc Project Owner xác nhận.

---

# 11.6 Sprint Closure Process

```text
Implementation
        │
        ▼
Build
        │
        ▼
Testing
        │
        ▼
Documentation Update
        │
        ▼
Review
        │
        ▼
Acceptance
        │
        ▼
Baseline Update
        │
        ▼
Sprint Closed
```

Không được đóng Sprint nếu còn tiêu chí DoD chưa đạt.

---

# 11.7 Quality Gates

Mỗi Sprint phải vượt qua các Quality Gate sau:

| Gate | Requirement |
|------|-------------|
| Business | PASS |
| Documentation | PASS |
| Build | PASS |
| Architecture | PASS |
| Database | PASS |
| API | PASS |
| Security | PASS |
| Testing | PASS |
| Review | PASS |

Nếu bất kỳ Gate nào **FAIL**, Sprint chưa được hoàn thành.

---

# 11.8 Continuous Improvement

Sau mỗi Sprint cần thực hiện:

- Sprint Retrospective.
- Lessons Learned.
- Documentation Improvement.
- AI Prompt Improvement.
- Architecture Review (nếu cần).

Các bài học kinh nghiệm phải được ghi nhận để cải thiện các Sprint tiếp theo.

---

# 11.9 Governance Rules

Definition of Ready và Definition of Done chỉ được thay đổi khi:

- Có thay đổi quy trình phát triển.
- Có thay đổi kiến trúc.
- Có quyết định từ Solution Architect hoặc Project Owner.

Không tự ý sửa đổi các tiêu chí trong quá trình thực hiện Sprint.

---

# 11.10 Phase Summary

Definition of Ready và Definition of Done là tiêu chuẩn đánh giá chính thức của dự án AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

- AI biết khi nào được phép bắt đầu Sprint.
- Developer biết khi nào Sprint được xem là hoàn thành.
- Toàn bộ nhóm phát triển có cùng tiêu chuẩn đánh giá.
- Chất lượng sản phẩm được kiểm soát thống nhất qua mọi Sprint.

Phase 11 là cầu nối giữa Project Governance và quá trình triển khai thực tế, đảm bảo mỗi Sprint đều đạt chất lượng trước khi chuyển sang Sprint tiếp theo.
# =============================================================================
# PHASE 12 – PROJECT BASELINE DECLARATION
# =============================================================================

# 12.1 Purpose

Phase này chính thức tuyên bố rằng:

AnSinhSo Enterprise đã hoàn thành giai đoạn Bootstrap.

Toàn bộ Documentation Suite đã được thiết lập làm **Single Source of Truth (SSOT)**.

Kiến trúc, công nghệ, tiêu chuẩn phát triển và quy trình quản trị đã được xác lập thành Baseline v1.0.

Kể từ thời điểm này, mọi AI Coding Agent và Developer phải làm việc dựa trên Baseline này.

---

# 12.2 Baseline Declaration

## Project Status

BASELINE VERSION

1.0

---

PROJECT STATUS

READY FOR IMPLEMENTATION

---

PROJECT PHASE

Sprint 01

---

PROJECT TYPE

Enterprise Software

---

ARCHITECTURE STATUS

Approved

---

DOCUMENTATION STATUS

Baseline Approved

---

AI STATUS

Ready

---

SECURITY STATUS

Approved

---

INFRASTRUCTURE STATUS

Ready

---

## Official Decision

Project Owner xác nhận:

✔ Documentation Suite hoàn chỉnh.

✔ Kiến trúc đã được Baseline.

✔ Công nghệ đã được phê duyệt.

✔ Development Workflow đã xác lập.

✔ AI Operating Model đã xác lập.

✔ Sprint Planning đã hoàn chỉnh.

Dự án chính thức chuyển sang giai đoạn Implementation.

---

# 12.3 Enterprise Documentation Manifest

Documentation Suite chính thức của Baseline v1.0 gồm:

Bootstrap

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_INDEX.md
- 00_PROJECT_PROGRESS.md
- 00_AI_HANDOVER.md

Core

- 01_PROJECT_CONTEXT.md
- 02_AI_MEMORY.md
- 03_AI_SYSTEM_RULES.md

Standards

- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 07_FRONTEND_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md
- 10_DEPLOYMENT_STANDARDS.md
- 11_TESTING_STANDARDS.md
- 12_DEVOPS_STANDARDS.md

Development

- 13_AI_DEVELOPMENT_GUIDE.md
- 14_PROJECT_STRUCTURE.md
- 15_CODING_PROMPTS.md
- 16_IMPLEMENTATION_ROADMAP.md
- 17_DEVELOPMENT_SPRINTS.md
- 18_ZALO_OA_INTEGRATION_GUIDE.md

Governance

- 19_BASELINE_REVIEW.md
- 20_SECURITY_ARCHITECTURE.md
- 21_INFRASTRUCTURE_ARCHITECTURE.md
- 22_AI_EXECUTION_GUIDE.md
- 23_SPRINT_00_READINESS.md

Đây là bộ tài liệu chính thức của Baseline v1.0.

---

# 12.4 AI Startup Procedure

Mỗi AI Coding Agent khi tham gia dự án phải thực hiện theo trình tự sau:

Step 1

Đọc

00_PROJECT_BOOTSTRAP.md

↓

Step 2

Đọc

01_PROJECT_CONTEXT.md

↓

Step 3

Đọc

03_AI_SYSTEM_RULES.md

↓

Step 4

Đọc

Documentation Standards

↓

Step 5

Đọc

17_DEVELOPMENT_SPRINTS.md

↓

Step 6

Đọc

00_AI_HANDOVER.md

↓

Step 7

Sinh Execution Plan

↓

Step 8

Developer Approval

↓

Step 9

Generate Source Code

Không được đảo thứ tự.

---

# 12.5 Project Constitution

AnSinhSo Enterprise hoạt động theo các nguyên tắc sau.

## Principle 1

Documentation là nguồn thông tin chính thức.

---

## Principle 2

Architecture được kiểm soát thông qua ADR.

---

## Principle 3

Mọi Sprint đều phải tuân thủ Documentation Suite.

---

## Principle 4

AI không được tự ý thay đổi Baseline.

---

## Principle 5

Developer chịu trách nhiệm cuối cùng.

---

## Principle 6

Source Code phải phản ánh đúng Documentation.

---

## Principle 7

Documentation luôn được cập nhật trước hoặc đồng thời với các thay đổi lớn.

---

## Principle 8

Security được tích hợp trong mọi Sprint.

---

## Principle 9

AI chỉ triển khai trong phạm vi Sprint hiện tại.

---

## Principle 10

Mọi thay đổi đều phải truy vết được.

---

# 12.6 Next Actions

Sau khi hoàn thành Bootstrap.

Developer thực hiện:

□ Clone Repository

□ Kiểm tra Documentation

□ Xác nhận Sprint

□ Khởi tạo Solution

□ Chuẩn bị Development Environment

□ Chuẩn bị SQL Server

□ Chuẩn bị Git Repository

□ Chuẩn bị Zalo OA

□ Chuẩn bị GIS Environment

□ Chuẩn bị AI Context

---

AI Coding Agent thực hiện:

□ Đọc Bootstrap

□ Đọc Context

□ Đọc Standards

□ Đọc Sprint

□ Đọc AI Handover

□ Sinh Sprint Execution Plan

□ Chờ Developer Approval

□ Generate Code

---

# 12.7 Baseline Governance

Baseline v1.0 chỉ được thay đổi khi:

- Architecture Review.

- Documentation Review.

- Sprint Review.

- Security Review.

- Project Owner Approval.

Không thay đổi trực tiếp.

---

# 12.8 Revision History

| Version | Date | Description |
|----------|------|-------------|
| 0.1 | Draft | Khởi tạo Bootstrap |
| 0.5 | Internal Review | Hoàn thiện Documentation Suite |
| 1.0 | Approved | Sprint 00 Baseline |

Các phiên bản tiếp theo:

1.1

↓

Sprint 01

↓

Sprint 02

↓

...

↓

2.0

Production Release

---

# 12.9 Final Declaration

AnSinhSo Enterprise chính thức hoàn thành giai đoạn Bootstrap.

Documentation Suite được xác lập là nền tảng kỹ thuật và quản trị duy nhất của dự án.

Mọi AI Coding Agent, Developer và Technical Reviewer phải tuân thủ toàn bộ các quy định, tiêu chuẩn và quy trình được định nghĩa trong Documentation Suite.

Từ thời điểm này, dự án chính thức chuyển sang giai đoạn triển khai kỹ thuật (Implementation Phase) theo kế hoạch Sprint đã được phê duyệt.

---

# 12.10 Phase Summary

Phase 12 đánh dấu việc hoàn thành Bootstrap của AnSinhSo Enterprise.

Sau khi hoàn thành Phase này:

✔ Documentation Suite được Baseline.

✔ Architecture được Freeze.

✔ Technology Stack được Freeze.

✔ AI Operating Model được kích hoạt.

✔ Governance có hiệu lực.

✔ Sprint 01 chính thức được phép bắt đầu.

00_PROJECT_BOOTSTRAP.md là tài liệu đầu tiên mà mọi AI Coding Agent và Developer phải đọc trước khi tham gia phát triển dự án.

Đây là "Hiến pháp" (Project Constitution) của toàn bộ hệ thống AnSinhSo Enterprise.