# 00_GIT_WORKFLOW.md

# GIT WORKFLOW
## Enterprise Git Strategy
Version: 1.0.0
Status: Active
Project: AnSinhSo Enterprise
Last Updated: 2026-07-17

---

# 1. Purpose

Tài liệu này quy định quy trình làm việc với Git và GitHub cho toàn bộ dự án AnSinhSo Enterprise.

Mục tiêu:

- Chuẩn hóa quy trình làm việc của tất cả thành viên.
- Đảm bảo lịch sử Git sạch (Clean Git History).
- Hạn chế Merge Conflict.
- Tăng khả năng mở rộng khi dự án có nhiều lập trình viên.
- Hỗ trợ AI Coding Agent làm việc thống nhất.

---

# 2. Branch Strategy

Repository sử dụng Git Flow rút gọn.

```
main
│
develop
│
├── feature/documentation
├── feature/backend
├── feature/frontend
└── hotfix/*
```

---

# 3. Branch Responsibilities

## main

Production Branch.

Chỉ chứa:

- Stable Release
- Production Ready

Không commit trực tiếp.

---

## develop

Integration Branch.

Tất cả Feature sau khi hoàn thành đều Merge vào đây.

Không phát triển trực tiếp.

---

## feature/documentation

Dùng để:

- Enterprise Documentation
- Architecture Documents
- AI Specifications
- Markdown Documents

Ví dụ:

31_SPRINT_03_BASE_CLASSES.md

32_SPRINT_03_EXCEPTIONS.md

...

---

## feature/backend

Chỉ phát triển:

- Domain
- Application
- Infrastructure
- API
- Database

Không chứa Documentation.

---

## feature/frontend

Chỉ phát triển:

- React
- UI
- Pages
- Components
- TailwindCSS

Không chứa Backend.

---

## hotfix/*

Ví dụ:

```
hotfix/login-bug

hotfix/security

hotfix/payment
```

Chỉ dùng khi Production gặp lỗi.

---

# 4. Standard Workflow

Bước 1

Checkout feature branch.

```
git checkout feature/documentation
```

Bước 2

Làm việc.

Bước 3

Commit.

```
git add .

git commit -m "docs(sprint03): complete specification architecture"
```

Bước 4

Push.

```
git push
```

Bước 5

Merge vào develop.

```
git checkout develop

git pull origin develop

git merge feature/documentation

git push origin develop
```

Bước 6

Quay lại feature.

```
git checkout feature/documentation

git merge develop
```

---

# 5. Commit Convention

Sử dụng Conventional Commits.

## Documentation

```
docs:
```

Ví dụ:

```
docs(sprint03): complete specification architecture
```

---

## Feature

```
feat:
```

Ví dụ

```
feat(domain): add citizen aggregate
```

---

## Fix

```
fix:
```

Ví dụ

```
fix(authentication): resolve jwt validation
```

---

## Refactor

```
refactor:
```

---

## Test

```
test:
```

---

## Chore

```
chore:
```

---

# 6. Merge Rules

Được phép:

```
feature/*
↓

develop
↓

main
```

Không được phép:

```
feature
↓

main
```

---

# 7. Pull Request Rules

Mọi Feature phải được Merge vào develop trước.

main chỉ nhận Merge từ develop.

---

# 8. Conflict Resolution

Nếu xảy ra Conflict:

1.

```
git pull origin develop
```

2.

Resolve Conflict.

3.

```
git add .
```

4.

```
git commit
```

5.

```
git push
```

---

# 9. Release Strategy

Ví dụ:

Sprint 01

↓

develop

Sprint 02

↓

develop

Sprint 03

↓

develop

↓

Release

↓

main

---

# 10. Version Tag

Ví dụ:

```
v0.1.0

v0.2.0

v0.3.0

v1.0.0
```

---

# 11. Protected Branches

Protected:

- main
- develop

Không commit trực tiếp.

---

# 12. AI Coding Rules

AI Coding Agent phải:

- Không commit trực tiếp vào main.
- Không commit trực tiếp vào develop.
- Luôn làm việc trên feature branch.
- Tuân thủ Conventional Commits.
- Không tự ý tạo Branch mới nếu chưa được Project Owner chấp thuận.

---

# 13. Repository Structure

```
main

↓

develop

↓

feature/*
```

Đây là mô hình chuẩn của toàn bộ dự án.

---

# 14. Definition Of Done

Git Workflow được xem là hoàn thành khi:

- Branch Strategy được định nghĩa.
- Merge Strategy được định nghĩa.
- Commit Convention được chuẩn hóa.
- Release Strategy được chuẩn hóa.
- AI Coding Rules được xác định.

---

# END OF DOCUMENT