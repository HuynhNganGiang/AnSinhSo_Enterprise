# 06_API_STANDARDS.md

> **Project:** AnSinhSo Enterprise\
> **Document:** API Standards\
> **Version:** 1.0.0\
> **Status:** FROZEN\
> **Owner:** Huỳnh Ngân Giang\
> **Last Updated:** 2026-07-10

------------------------------------------------------------------------

# 1. Purpose

Tài liệu này quy định **tiêu chuẩn bắt buộc** cho toàn bộ API của hệ
thống AnSinhSo. Tài liệu **không mô tả API cụ thể**, mà định nghĩa quy
tắc thiết kế, phát triển, kiểm thử và vận hành.

# 2. Scope

Áp dụng cho:

-   ASP.NET Core 8 Web API
-   Swagger / OpenAPI
-   REST API
-   JWT Authentication
-   RBAC Dynamic
-   AI API
-   GIS API
-   Zalo OA API

# 3. Design Principles

-   RESTful
-   Stateless
-   JSON UTF-8
-   HTTPS only
-   Versioned API
-   Least Privilege
-   Audit by default
-   Idempotent where required

# 4. URL Convention

Base URL:

``` text
/api/v1/
```

Ví dụ:

``` text
GET    /api/v1/persons
GET    /api/v1/persons/{id}
POST   /api/v1/persons
PUT    /api/v1/persons/{id}
PATCH  /api/v1/persons/{id}
DELETE /api/v1/persons/{id}
```

Không sử dụng động từ trong URL.

# 5. HTTP Methods

  Method   Mục đích
  -------- -------------------
  GET      Đọc dữ liệu
  POST     Tạo mới
  PUT      Thay thế toàn bộ
  PATCH    Cập nhật một phần
  DELETE   Soft Delete

# 6. Versioning

-   Bắt buộc dùng `/api/v1`
-   Không phá vỡ API cũ
-   API mới tạo ở `/api/v2`

# 7. Request Standard

-   Content-Type: application/json
-   UTF-8
-   camelCase

Ví dụ:

``` json
{
  "cccd":"060180000001",
  "hoTen":"Nguyễn Văn A",
  "diaBanId":5
}
```

# 8. Response Standard

``` json
{
  "success": true,
  "message": "Success",
  "data": {},
  "errors": [],
  "traceId": "",
  "timestamp": "2026-07-10T10:00:00Z"
}
```

# 9. Error Standard

``` json
{
  "success": false,
  "message": "Validation Error",
  "errors":[
    {
      "field":"HoTen",
      "message":"Không được để trống"
    }
  ],
  "traceId":"..."
}
```

Không trả StackTrace cho client.

# 10. Validation

-   FluentValidation
-   Không validate trong Controller
-   Không validate trong Repository

# 11. Authentication

-   JWT Bearer
-   HTTPS bắt buộc
-   Token có thời hạn
-   Refresh Token

# 12. Authorization

Chỉ kiểm tra Permission.

Ví dụ:

-   person.view
-   person.create
-   person.edit
-   payment.approve

# 13. Pagination

Query:

``` text
?page=1&pageSize=20
```

Response phải có:

-   page
-   pageSize
-   totalRecords
-   totalPages
-   items

# 14. Filtering

Ví dụ:

``` text
?status=Active
?gender=Nam
?district=SongLuy
```

# 15. Sorting

``` text
sortBy=CreatedAt
sortDirection=desc
```

# 16. Searching

``` text
?keyword=Nguyen
```

# 17. DateTime

Chuẩn ISO-8601 UTC.

# 18. Status Codes

-   200
-   201
-   204
-   400
-   401
-   403
-   404
-   409
-   422
-   500

# 19. Idempotency

Các API:

-   Payment
-   Import
-   Batch Processing

phải hỗ trợ `Idempotency-Key`.

# 20. Upload

Cho phép:

-   csv
-   xlsx
-   pdf
-   jpg
-   png

Giới hạn mặc định: 100 MB.

# 21. GIS API

Ví dụ:

-   GET /api/v1/gis/heatmap
-   GET /api/v1/gis/clusters

# 22. AI API

Ví dụ:

-   POST /api/v1/ai/chat
-   POST /api/v1/ai/predict

# 23. Zalo OA API

Ví dụ:

-   POST /api/v1/zalo/send
-   POST /api/v1/zalo/webhook

# 24. Audit Logging

Mọi POST/PUT/PATCH/DELETE đều ghi AuditLogs.

# 25. Logging

Serilog:

-   Request
-   Response
-   Exception
-   Performance

Không ghi:

-   Password
-   JWT
-   Secret

# 26. Security

Bắt buộc:

-   HTTPS
-   JWT
-   CORS
-   CSP
-   HSTS
-   SQL Injection Protection
-   XSS Protection

# 27. Swagger

Mọi endpoint phải có:

-   Summary
-   Description
-   Request Example
-   Response Example
-   Security Requirement

# 28. Performance

-   CRUD \< 500ms
-   Search \< 1s
-   Dashboard \< 2s

# 29. Checklist

-   RESTful
-   JWT
-   Permission
-   Validation
-   Audit
-   Logging
-   Swagger
-   Versioning
-   Pagination
-   Error Standard

# Change Log

  Version   Date         Description
  --------- ------------ ----------------------------
  1.0.0     2026-07-10   Initial production version
