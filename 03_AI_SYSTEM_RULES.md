# AI_SYSTEM_RULES.md

Version 1.0

Enterprise Rule Engine

---

# Rule 1

Không được phá vỡ kiến trúc hiện tại.

---

# Rule 2

Không được tạo Duplicate Logic.

---

# Rule 3

Không được tạo Hard Delete.

Bắt buộc Soft Delete.

---

# Rule 4

Mọi INSERT

↓

Audit

↓

Log

↓

Transaction

---

# Rule 5

Mọi UPDATE

↓

Audit

↓

Log

↓

Transaction

---

# Rule 6

Mọi DELETE

↓

Soft Delete

↓

Audit

↓

Transaction

---

# Rule 7

Mọi API

↓

Validation

↓

Permission

↓

Exception

↓

Logging

↓

Response Standard

---

# Rule 8

Mọi Stored Procedure

BEGIN TRY

BEGIN TRANSACTION

...

COMMIT

END TRY

BEGIN CATCH

ROLLBACK

THROW

END CATCH

---

# Rule 9

Mọi Repository

Async

CancellationToken

No Tracking khi chỉ đọc

Pagination

Filter

Search

Sorting

---

# Rule 10

Không SELECT *

---

# Rule 11

Không Hardcode.

---

# Rule 12

Không viết Business Logic trong Controller.

---

# Rule 13

Controller

↓

Service

↓

Repository

↓

Database

---

# Rule 14

Mọi API trả về

{
success

message

data

errors
}

---

# Rule 15

Mọi dữ liệu nhạy cảm

CCCD

Phone

Token

Password

AppSecret

AccessToken

↓

Mask

Encrypt

---

# Rule 16

Không bao giờ trả Password.

---

# Rule 17

Không bao giờ trả Access Token Database.

---

# Rule 18

Không bao giờ ghi Secret vào Log.

---

# Rule 19

Ưu tiên Performance.

---

# Rule 20

Ưu tiên Production.

Không Demo.

Không Fake.

Không Mock nếu không được yêu cầu.

---

# Rule 21

Mọi module mới

↓

Database

↓

Entity

↓

DTO

↓

Repository

↓

Service

↓

Controller

↓

Swagger

↓

Frontend

↓

Testing

↓

Documentation

---

# Rule 22

AI phải tự kiểm tra:

Architecture

Performance

Security

Database

Coding Convention

Trước khi kết thúc.