# PROJECT INDEX
## Dự án: Hệ thống An Sinh Số xã Sông Lũy (AnSinhSo)
Version: 2.0.0
Status: FROZEN
Architecture: Enterprise Clean Architecture
Framework: ASP.NET Core 8 + SQL Server 2022
Last Updated: 2026-07-14

---

# 1. PURPOSE

Tài liệu này là **bản đồ tổng thể (Master Index)** của toàn bộ hệ thống tài liệu AnSinhSo.

Mục tiêu:

- Giúp AI Coding hiểu nhanh toàn bộ dự án.
- Giúp thành viên mới có thể tham gia dự án.
- Là điểm khởi đầu của mọi Sprint.
- Định vị chính xác tài liệu trong workspace.

Tài liệu này được khóa (FROZEN) sau khi hoàn thành Sprint 00.

---

# 2. STARTUP ORDER

Mọi AI (ChatGPT, AntiGravity AI, Cursor, Cline, Continue, GitHub Copilot...) phải đọc tài liệu theo đúng thứ tự sau:

1. 00_PROJECT_BOOTSTRAP.md
2. 00_PROJECT_PROGRESS.md
3. 00_PROJECT_INDEX.md
4. 00_AI_HANDOVER.md

Sau khi đọc xong bốn tài liệu trên mới tiếp tục đọc các tài liệu chuyên môn theo yêu cầu của từng Sprint.

---

# 3. DOCUMENT STRUCTURE

```
Enterprise Documentation
│
├── 00 Project & Foundation
│   ├── 00_PROJECT_BOOTSTRAP.md
│   ├── 00_PROJECT_PROGRESS.md
│   ├── 00_PROJECT_INDEX.md
│   └── 00_AI_HANDOVER.md
│
├── 01 Context
│   ├── 01_PROJECT_CONTEXT.md
│   ├── 02_AI_MEMORY.md
│   └── 03_AI_SYSTEM_RULES.md
│
├── 02 Development Standards & Architecture
│   ├── 04_CODING_STANDARDS.md (FROZEN)
│   ├── 05_DATABASE_RULES.md (FROZEN)
│   ├── 06_API_STANDARDS.md (FROZEN)
│   ├── 07_FRONTEND_STANDARDS.md (FROZEN)
│   ├── 08_BACKEND_STANDARDS.md (FROZEN)
│   ├── 09_SECURITY_STANDARDS.md (FROZEN)
│   ├── 10_DEPLOYMENT_STANDARDS.md (FROZEN)
│   ├── 11_TESTING_STANDARDS.md (FROZEN)
│   ├── 12_DEVOPS_STANDARDS.md (FROZEN)
│   ├── 20_SECURITY_ARCHITECTURE.md
│   └── 21_INFRASTRUCTURE_ARCHITECTURE.md
│
├── 03 Development Guides
│   ├── 13_AI_DEVELOPMENT_GUIDE.md
│   ├── 14_PROJECT_STRUCTURE.md
│   ├── 15_CODING_PROMPTS.md
│   ├── 16_IMPLEMENTATION_ROADMAP.md
│   ├── 17_DEVELOPMENT_SPRINTS.md
│   └── 18_ZALO_OA_INTEGRATION_GUIDE.md
│
└── 04 Governance
    ├── 19_BASELINE_REVIEW.md
    ├── 22_AI_EXECUTION_GUIDE.md
    └── 23_SPRINT_00_READINESS.md
```

---

# 4. ENTERPRISE CORE FILES

## Foundation
- 00_PROJECT_BOOTSTRAP.md
- 01_PROJECT_CONTEXT.md
- 02_AI_MEMORY.md
- 03_AI_SYSTEM_RULES.md

## Development Standards (FROZEN)
- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md (Chứa quy tắc và thiết kế cơ sở dữ liệu tích hợp)
- 06_API_STANDARDS.md (Chứa quy tắc và đặc tả thiết kế API tích hợp)
- 07_FRONTEND_STANDARDS.md
- 08_BACKEND_STANDARDS.md (Chứa quy tắc nghiệp vụ tích hợp)
- 09_SECURITY_STANDARDS.md
- 10_DEPLOYMENT_STANDARDS.md
- 11_TESTING_STANDARDS.md
- 12_DEVOPS_STANDARDS.md

## Architecture
- 20_SECURITY_ARCHITECTURE.md
- 21_INFRASTRUCTURE_ARCHITECTURE.md

## Development Sprints & Guides
- 13_AI_DEVELOPMENT_GUIDE.md
- 14_PROJECT_STRUCTURE.md
- 15_CODING_PROMPTS.md
- 16_IMPLEMENTATION_ROADMAP.md
- 17_DEVELOPMENT_SPRINTS.md
- 18_ZALO_OA_INTEGRATION_GUIDE.md

## Governance & Readiness
- 19_BASELINE_REVIEW.md
- 22_AI_EXECUTION_GUIDE.md
- 23_SPRINT_00_READINESS.md

---

# 5. DOCUMENT DEPENDENCY

```
00_PROJECT_BOOTSTRAP
      │
      ▼
01_PROJECT_CONTEXT
      │
      ▼
02_AI_MEMORY & 03_AI_SYSTEM_RULES
      │
      ▼
04~12 Development Standards & Architecture (20, 21)
      │
      ▼
13~18 Development Guides & Sprints
      │
      ▼
19~23 Governance & Readiness
```

---

# 6. AI WORKFLOW

Khi sinh mã nguồn, AI phải thực hiện theo quy trình:

1. Đọc Bootstrap.
2. Đọc Progress.
3. Đọc Index.
4. Đọc AI Handover.
5. Xác định Sprint hiện tại.
6. Đọc tài liệu liên quan trong Sprints.
7. Sinh mã nguồn tuân thủ Standards.
8. Tự kiểm tra Coding, Security, Performance theo Quality Gate.
9. Chỉ bàn giao mã nguồn khi đạt Definition of Done.

---

# 7. VERSIONING

Toàn bộ tài liệu sử dụng Semantic Versioning.

Ví dụ:

```
v2.0.0
```

- Major: Thay đổi kiến trúc tài liệu / Hệ thống.
- Minor: Thêm tài liệu mới / Cập nhật nội dung lớn.
- Patch: Sửa lỗi chính tả / Cập nhật nhỏ.

---

# 8. CHANGE MANAGEMENT

Mỗi tài liệu phải có:

- Metadata (Version, Status, Owner, Last Updated)
- Change Log
- Related Documents

Không được thay đổi cấu trúc chung của tài liệu nếu chưa cập nhật Bootstrap.

---

# 9. MAINTENANCE PRINCIPLES

- Một tài liệu chỉ có một trách nhiệm.
- Không trùng lặp nội dung giữa các tài liệu.
- Tất cả tài liệu phải có khả năng mở rộng lâu dài.

---

# 10. CURRENT STATUS

Project Status:
- Sprint 00: COMPLETED
- Sprint 01: READY

Current Focus:
- Sprint 01 – Foundation

Next Action:
- Khởi tạo Solution, cấu trúc thư mục Clean Architecture, thiết lập Logging, Swagger, Health Check theo đặc tả Sprint 01.

---

# END OF DOCUMENT