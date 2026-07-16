# 09_SECURITY_STANDARDS.md

## Phase 1 – Security Foundation

```text
============================================================================
09_SECURITY_STANDARDS.md
============================================================================
Project        : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
Document Type  : Enterprise Security Standards
Version        : 1.0.0
Status         : FROZEN
Owner          : Project Architecture Team
Architecture   : Enterprise Clean Architecture
Framework      : ASP.NET Core 8
Database       : SQL Server 2022
Authentication : JWT Bearer
Authorization  : RBAC
Encryption     : AES-256 / TLS 1.3
Hashing        : PBKDF2 / BCrypt / Argon2 (Theo cấu hình triển khai)
Logging        : Serilog
Audit          : Audit Trail
Compliance     : OWASP ASVS, OWASP Top 10, NIST CSF
Last Updated   : 2026-07-12
============================================================================
```

---

# 09. SECURITY STANDARDS

---

# 1. PURPOSE

Tài liệu này quy định toàn bộ tiêu chuẩn bảo mật của hệ thống **AnSinhSo**.

Mục tiêu:

* Chuẩn hóa các yêu cầu bảo mật.
* Bảo vệ dữ liệu người dân.
* Đảm bảo an toàn hệ thống khi triển khai Production.
* Giảm thiểu rủi ro tấn công.
* Tuân thủ các thông lệ bảo mật quốc tế.
* Hỗ trợ AI Coding sinh mã đúng chuẩn Security.

Toàn bộ thành viên dự án bắt buộc tuân thủ tài liệu này.

---

# 2. SCOPE

Áp dụng cho toàn bộ hệ thống:

* Backend
* Frontend
* Database
* API
* Authentication
* Authorization
* AI Integration
* GIS Integration
* Zalo Official Account
* File Storage
* Logging
* Audit
* Infrastructure

Không giới hạn trong phạm vi mã nguồn.

---

# 3. SECURITY OBJECTIVES

Hệ thống phải đảm bảo:

* Confidentiality
* Integrity
* Availability
* Authenticity
* Accountability
* Traceability

---

## 3.1 Confidentiality

Thông tin người dân chỉ được truy cập bởi người có quyền.

Không được để lộ dữ liệu nhạy cảm.

---

## 3.2 Integrity

Dữ liệu không được thay đổi trái phép.

Mọi thay đổi phải có khả năng kiểm tra.

---

## 3.3 Availability

Hệ thống phải sẵn sàng phục vụ theo SLA đã xác định.

Có cơ chế dự phòng và phục hồi khi xảy ra sự cố.

---

## 3.4 Authenticity

Mọi người dùng đều phải được xác thực trước khi truy cập.

---

## 3.5 Accountability

Mọi thao tác quan trọng phải ghi nhận Audit Log.

---

## 3.6 Traceability

Mỗi yêu cầu phải có khả năng truy vết:

* Người thực hiện
* Thời gian
* Địa chỉ IP (nếu áp dụng)
* Hành động
* Kết quả

---

# 4. SECURITY PRINCIPLES

## Principle 01 – Security by Design

Bảo mật phải được thiết kế ngay từ đầu.

Không bổ sung sau khi hoàn thành hệ thống.

---

## Principle 02 – Least Privilege

Người dùng chỉ được cấp đúng quyền cần thiết.

Không cấp quyền dư thừa.

---

## Principle 03 – Zero Trust

Không mặc định tin cậy:

* Người dùng
* Thiết bị
* Mạng
* Dịch vụ

Mọi truy cập đều phải được xác minh.

---

## Principle 04 – Defense in Depth

Áp dụng nhiều lớp bảo vệ:

* Frontend
* API
* Backend
* Database
* Infrastructure

Không phụ thuộc vào một cơ chế duy nhất.

---

## Principle 05 – Secure by Default

Mọi cấu hình mặc định phải ưu tiên bảo mật.

---

## Principle 06 – Fail Secure

Nếu xảy ra lỗi:

* Không cấp quyền.
* Không để lộ dữ liệu.
* Không bỏ qua xác thực.

---

## Principle 07 – Audit Everything Important

Các thao tác quan trọng phải được ghi nhận đầy đủ.

---

## Principle 08 – Privacy First

Ưu tiên bảo vệ dữ liệu cá nhân của người dân.

---

# 5. CIA TRIAD

## 5.1 Confidentiality

Áp dụng:

* Authentication
* Authorization
* Encryption
* Access Control

---

## 5.2 Integrity

Áp dụng:

* Validation
* Audit Log
* Transaction
* Concurrency Control

---

## 5.3 Availability

Áp dụng:

* Backup
* Monitoring
* Health Check
* Disaster Recovery

---

# 6. SECURITY ARCHITECTURE

```text
User
 │
 ▼
Authentication
 │
 ▼
Authorization
 │
 ▼
API Security
 │
 ▼
Business Layer
 │
 ▼
Database Security
 │
 ▼
Audit & Monitoring
```

Bảo mật được áp dụng ở mọi tầng của hệ thống.

---

# 7. SECURITY LAYERS

Hệ thống gồm các lớp bảo vệ:

Layer 1

Physical Security

↓

Layer 2

Infrastructure Security

↓

Layer 3

Network Security

↓

Layer 4

Application Security

↓

Layer 5

API Security

↓

Layer 6

Database Security

↓

Layer 7

Data Security

---

# 8. SECURITY RESPONSIBILITIES

| Vai trò             | Trách nhiệm                         |
| ------------------- | ----------------------------------- |
| Solution Architect  | Thiết kế kiến trúc bảo mật          |
| Backend Developer   | Áp dụng Security Standards          |
| Frontend Developer  | Bảo vệ phía Client                  |
| Database Developer  | Bảo vệ dữ liệu                      |
| DevOps              | Bảo mật hạ tầng                     |
| Tester              | Security Testing                    |
| Technical Lead      | Security Review                     |
| AI Coding Assistant | Sinh mã tuân thủ Security Standards |

---

# 9. SECURITY TERMINOLOGY

Các thuật ngữ chuẩn sử dụng trong dự án:

| Thuật ngữ      | Ý nghĩa                                     |
| -------------- | ------------------------------------------- |
| Authentication | Xác thực người dùng                         |
| Authorization  | Phân quyền truy cập                         |
| RBAC           | Role-Based Access Control                   |
| JWT            | JSON Web Token                              |
| TLS            | Transport Layer Security                    |
| Encryption     | Mã hóa dữ liệu                              |
| Hashing        | Băm dữ liệu                                 |
| Audit Trail    | Nhật ký kiểm tra                            |
| OWASP          | Open Worldwide Application Security Project |
| Zero Trust     | Mô hình không tin cậy mặc định              |

---

# 10. SECURITY BASELINE

Toàn bộ hệ thống phải đáp ứng tối thiểu:

* HTTPS bắt buộc.
* JWT Authentication.
* RBAC Authorization.
* Input Validation.
* Output Encoding.
* Audit Logging.
* Structured Logging.
* Password Hashing.
* Secret Management.
* Backup định kỳ.
* Monitoring liên tục.
* Không Hard-code Secret.
* Không Hard-code Password.
* Không Hard-code API Key.

---

# End of Phase 1

Phase tiếp theo:

* Authentication Standards
* JWT Standards
* Refresh Token Standards
* Password Policy
* Session Management
* Cookie Security
* MFA Readiness
* Identity Security
# ============================================================================

# 11. AUTHENTICATION STANDARDS

# ============================================================================

## 11.1 Purpose

Authentication là lớp bảo vệ đầu tiên của hệ thống.

Mọi người dùng phải được xác thực trước khi truy cập bất kỳ tài nguyên nào của hệ thống.

Không cho phép Anonymous Access đối với các API nghiệp vụ, ngoại trừ các Endpoint được quy định rõ.

---

## 11.2 Authentication Architecture

Hệ thống sử dụng:

* JWT Bearer Authentication
* Refresh Token
* HTTPS bắt buộc
* ASP.NET Core Authentication Middleware

Luồng xác thực:

```text
Client

↓

Login API

↓

Identity Validation

↓

Generate JWT

↓

Generate Refresh Token

↓

Client

↓

Authorized API
```

---

## 11.3 Authentication Principles

Authentication phải bảo đảm:

* Stateless
* Secure
* Scalable
* Auditable
* Revocable

---

## 11.4 Authentication Flow

Quy trình chuẩn:

1. Người dùng gửi thông tin đăng nhập.
2. Hệ thống xác thực thông tin.
3. Sinh Access Token.
4. Sinh Refresh Token.
5. Ghi Audit Log.
6. Trả Token về Client.
7. Client sử dụng JWT cho các API tiếp theo.

---

## 11.5 Supported Authentication

Phiên bản hiện tại hỗ trợ:

* Username + Password
* JWT Bearer

Có khả năng mở rộng:

* OAuth2
* OpenID Connect
* SSO
* Multi-Factor Authentication (MFA)

---

# ============================================================================

# 12. JWT STANDARDS

# ============================================================================

## 12.1 Purpose

JWT được sử dụng để xác thực các yêu cầu giữa Client và Backend.

---

## 12.2 Access Token

Access Token phải:

* Có thời hạn ngắn.
* Chỉ chứa thông tin cần thiết.
* Không chứa dữ liệu nhạy cảm.

---

## 12.3 Required Claims

Access Token tối thiểu phải bao gồm:

* UserId
* Username
* Role
* TokenId (JTI)
* Issued At
* Expiration

Có thể mở rộng thêm Claims theo nghiệp vụ.

---

## 12.4 Token Lifetime

Khuyến nghị:

* Access Token: 15–30 phút.
* Refresh Token: Theo chính sách triển khai (ví dụ 7–30 ngày).

Giá trị cụ thể được cấu hình trong môi trường triển khai, không được Hard-code.

---

## 12.5 Token Signing

JWT phải:

* Được ký số.
* Kiểm tra Issuer.
* Kiểm tra Audience.
* Kiểm tra Expiration.
* Kiểm tra Signature.

---

## 12.6 Token Revocation

Hệ thống phải hỗ trợ:

* Thu hồi Token.
* Đăng xuất khỏi tất cả thiết bị (nếu triển khai).
* Thu hồi Refresh Token khi phát hiện bất thường.

---

# ============================================================================

# 13. REFRESH TOKEN STANDARDS

# ============================================================================

## 13.1 Purpose

Refresh Token dùng để cấp Access Token mới mà không yêu cầu người dùng đăng nhập lại.

---

## 13.2 Rules

Refresh Token phải:

* Sinh ngẫu nhiên.
* Khó đoán.
* Có thời hạn.
* Có thể thu hồi.
* Chỉ sử dụng một lần nếu áp dụng cơ chế Rotation.

---

## 13.3 Storage

Refresh Token không được lưu dưới dạng Plain Text trong Database Production.

Ưu tiên:

* Hash
* Encrypt

---

## 13.4 Rotation

Khuyến nghị áp dụng:

Refresh Token Rotation.

Sau mỗi lần Refresh:

* Sinh Refresh Token mới.
* Thu hồi Refresh Token cũ.

---

## 13.5 Revocation

Thu hồi Refresh Token khi:

* Người dùng đăng xuất.
* Đổi mật khẩu.
* Khóa tài khoản.
* Phát hiện truy cập bất thường.

---

# ============================================================================

# 14. PASSWORD POLICY

# ============================================================================

## 14.1 Password Requirements

Chính sách mật khẩu được cấu hình theo môi trường triển khai.

Khuyến nghị bao gồm:

* Độ dài tối thiểu.
* Chữ hoa.
* Chữ thường.
* Số.
* Ký tự đặc biệt.

---

## 14.2 Password Storage

Không lưu:

* Plain Text Password.
* Password mã hóa đối xứng.

Chỉ lưu:

* Password Hash.
* Salt (nếu thuật toán yêu cầu).

---

## 14.3 Hash Algorithm

Khuyến nghị:

* Argon2
* BCrypt
* PBKDF2

Không sử dụng:

* MD5
* SHA1

---

## 14.4 Password Change

Khi đổi mật khẩu:

* Kiểm tra mật khẩu hiện tại.
* Ghi Audit Log.
* Thu hồi Refresh Token hiện có.

---

## 14.5 Password Reset

Mật khẩu mới phải được tạo thông qua quy trình xác minh hợp lệ.

Không gửi mật khẩu dạng rõ qua Email hoặc Zalo.

---

# ============================================================================

# 15. SESSION MANAGEMENT

# ============================================================================

## 15.1 Session Strategy

Hệ thống sử dụng:

Stateless Authentication.

Không lưu Session nghiệp vụ trên Server.

---

## 15.2 Session Timeout

Session hết hạn theo Access Token.

Refresh Token được sử dụng để gia hạn khi hợp lệ.

---

## 15.3 Concurrent Sessions

Có thể cấu hình:

* Cho phép nhiều thiết bị.
* Giới hạn số phiên hoạt động.
* Đăng xuất khỏi các thiết bị khác.

---

## 15.4 Session Invalidation

Session phải bị vô hiệu khi:

* Đăng xuất.
* Đổi mật khẩu.
* Khóa tài khoản.
* Thu hồi Token.

---

# ============================================================================

# 16. COOKIE SECURITY

# ============================================================================

## 16.1 Cookie Usage

Nếu hệ thống sử dụng Cookie:

Bắt buộc:

* Secure
* HttpOnly
* SameSite

---

## 16.2 Cookie Rules

Không lưu:

* Password
* Secret
* API Key

---

## 16.3 Cross-Site Protection

Cookie phải được cấu hình phù hợp nhằm giảm thiểu nguy cơ Cross-Site Request Forgery (CSRF).

---

# ============================================================================

# 17. MULTI-FACTOR AUTHENTICATION (MFA)

# ============================================================================

## 17.1 Readiness

Kiến trúc phải sẵn sàng mở rộng MFA.

---

## 17.2 Supported Methods

Có thể hỗ trợ:

* Authenticator App
* OTP
* Email
* SMS
* Hardware Token

---

## 17.3 Sensitive Operations

Khuyến nghị yêu cầu xác thực bổ sung đối với:

* Đổi mật khẩu.
* Đổi thông tin tài khoản.
* Cấp quyền.
* Xuất dữ liệu lớn.

---

# ============================================================================

# 18. IDENTITY SECURITY

# ============================================================================

## 18.1 Identity Principles

Mỗi người dùng phải có:

* Định danh duy nhất.
* Vai trò.
* Trạng thái.
* Nhật ký hoạt động.

---

## 18.2 Account Status

Tài khoản có thể ở các trạng thái:

* Active
* Inactive
* Locked
* Suspended

---

## 18.3 Failed Login

Hệ thống phải theo dõi số lần đăng nhập thất bại.

Có thể áp dụng:

* Delay
* Captcha
* Khóa tạm thời

Theo chính sách triển khai.

---

## 18.4 Account Lockout

Khóa tài khoản phải:

* Có thời gian.
* Có Audit Log.
* Có khả năng mở khóa.

---

# ============================================================================

# 19. AUTHENTICATION CHECKLIST

# ============================================================================

Trước khi phát hành cần kiểm tra:

* JWT hoạt động đúng.
* Refresh Token hợp lệ.
* Password được Hash.
* Không lưu Plain Text Password.
* Không Hard-code Secret.
* HTTPS bắt buộc.
* Token có thời hạn.
* Token có thể thu hồi.
* Có Audit Log.
* Session được quản lý đúng.
* Cookie bảo mật (nếu sử dụng).
* Có khả năng mở rộng MFA.
* Tuân thủ Zero Trust.

---

# End of Phase 2A

Phase tiếp theo:

* Authorization Standards
* RBAC Standards
* Permission Model
* Resource-Based Authorization
* Data Access Security
* API Authorization
* Privilege Management
* Security Context
# ============================================================================

# 20. AUTHORIZATION STANDARDS

# ============================================================================

## 20.1 Purpose

Authorization xác định người dùng được phép thực hiện hành động nào sau khi đã xác thực thành công.

Mọi yêu cầu truy cập tài nguyên phải trải qua bước kiểm tra quyền.

Authentication chỉ trả lời câu hỏi:

> "Bạn là ai?"

Authorization trả lời câu hỏi:

> "Bạn được phép làm gì?"

---

## 20.2 Authorization Model

Hệ thống AnSinhSo áp dụng:

* Role-Based Access Control (RBAC)
* Resource-Based Authorization
* Policy-Based Authorization (khi cần)
* Principle of Least Privilege

---

## 20.3 Authorization Principles

Mọi quyết định phân quyền phải tuân thủ:

* Least Privilege
* Need to Know
* Explicit Allow
* Deny by Default
* Separation of Duties

---

## 20.4 Authorization Flow

```text
Request

↓

JWT Validation

↓

Load User Claims

↓

Check Role

↓

Check Permission

↓

Check Resource Scope

↓

Business Logic

↓

Response
```

---

# ============================================================================

# 21. ROLE-BASED ACCESS CONTROL (RBAC)

# ============================================================================

## 21.1 Purpose

RBAC là cơ chế phân quyền chính của hệ thống.

Người dùng được gán một hoặc nhiều Role.

Role quyết định tập Permission mà người dùng được sử dụng.

---

## 21.2 Standard Roles

Các vai trò mặc định:

| Role                | Mô tả             |
| ------------------- | ----------------- |
| SystemAdministrator | Quản trị hệ thống |
| LanhDao             | Lãnh đạo          |
| CanBoXa             | Cán bộ xã         |
| NguoiDan            | Người dân         |

Có thể mở rộng thêm Role theo yêu cầu triển khai.

---

## 21.3 Role Rules

Role:

* Không chứa Business Logic.
* Không phụ thuộc Frontend.
* Không được Hard-code trong nhiều vị trí.
* Được quản lý tập trung.

---

## 21.4 Multiple Roles

Hệ thống hỗ trợ:

* Một người dùng nhiều Role.
* Một Role nhiều Permission.

Nếu có nhiều Role:

Permission được hợp nhất theo chính sách hệ thống.

---

# ============================================================================

# 22. PERMISSION MODEL

# ============================================================================

## 22.1 Purpose

Permission là đơn vị phân quyền nhỏ nhất.

Không kiểm tra quyền trực tiếp bằng tên Role trong Business Logic nếu có thể sử dụng Permission.

---

## 22.2 Permission Naming

Định dạng:

```text
<Module>.<Action>
```

Ví dụ:

```text
Citizen.View

Citizen.Create

Citizen.Update

Citizen.Delete

Household.View

Household.Update

Policy.Approve

Dashboard.View
```

---

## 22.3 Permission Categories

Bao gồm:

* View
* Create
* Update
* Delete
* Approve
* Reject
* Import
* Export
* Manage
* Audit

---

## 22.4 Permission Management

Permission phải được quản lý tập trung.

Không khai báo Permission rải rác trong mã nguồn.

---

# ============================================================================

# 23. RESOURCE-BASED AUTHORIZATION

# ============================================================================

## 23.1 Purpose

Ngoài Role và Permission, hệ thống phải kiểm tra phạm vi dữ liệu.

Ví dụ:

Một cán bộ xã không được xem dữ liệu của xã khác.

---

## 23.2 Resource Scope

Có thể giới hạn theo:

* Xã
* Thôn
* Hộ gia đình
* Hồ sơ
* Đợt chi trả
* Chính sách
* Người phụ trách

---

## 23.3 Ownership Check

Người dân chỉ được phép:

* Xem hồ sơ của chính mình.
* Gửi phản ánh của chính mình.
* Theo dõi trợ cấp của chính mình.

Không được truy cập dữ liệu của người khác.

---

## 23.4 Administrative Scope

Cán bộ chỉ được thao tác trên phạm vi được phân công.

Mọi yêu cầu vượt phạm vi phải bị từ chối.

---

# ============================================================================

# 24. API AUTHORIZATION

# ============================================================================

## 24.1 API Protection

Tất cả API nghiệp vụ phải yêu cầu Authorization.

Ngoại lệ:

* Login
* Refresh Token
* Health Check (nếu được cấu hình)
* Swagger (chỉ trong môi trường phát triển)

---

## 24.2 Endpoint Security

Mỗi Endpoint phải xác định:

* Anonymous
* Authenticated
* Role
* Permission
* Resource Scope

---

## 24.3 Default Policy

Mặc định:

```text
Deny All
```

Chỉ cho phép truy cập khi đáp ứng đầy đủ điều kiện.

---

# ============================================================================

# 25. DATA ACCESS SECURITY

# ============================================================================

## 25.1 Data Isolation

Người dùng chỉ được truy cập dữ liệu thuộc phạm vi của mình.

Không trả về dữ liệu vượt quyền.

---

## 25.2 Query Filtering

Việc lọc dữ liệu theo quyền phải được thực hiện tại Backend.

Không dựa vào Frontend để ẩn dữ liệu.

---

## 25.3 Export Security

Xuất dữ liệu phải kiểm tra:

* Permission
* Resource Scope
* Audit Log

---

## 25.4 Bulk Operations

Các thao tác hàng loạt phải được kiểm tra quyền cho từng loại dữ liệu liên quan.

---

# ============================================================================

# 26. PRIVILEGE MANAGEMENT

# ============================================================================

## 26.1 Least Privilege

Mọi tài khoản chỉ được cấp quyền tối thiểu cần thiết.

---

## 26.2 Temporary Privileges

Quyền tạm thời phải:

* Có thời hạn.
* Có Audit Log.
* Có khả năng thu hồi.

---

## 26.3 Privilege Escalation

Không cho phép người dùng tự nâng quyền.

Mọi thay đổi quyền phải được ghi nhận.

---

# ============================================================================

# 27. SECURITY CONTEXT

# ============================================================================

## 27.1 User Context

Mỗi Request phải có Security Context bao gồm:

* UserId
* Username
* Roles
* Permissions
* Administrative Scope
* CorrelationId

---

## 27.2 Context Validation

Security Context phải được xác thực trước khi Business Logic được thực thi.

---

## 27.3 Trust Boundary

Không tin tưởng:

* Client Claims bị sửa đổi.
* Request Header không xác thực.
* Dữ liệu do Client tự khai báo nếu chưa kiểm chứng.

---

# ============================================================================

# 28. AUTHORIZATION CHECKLIST

# ============================================================================

Trước khi phát hành cần kiểm tra:

* RBAC hoạt động đúng.
* Permission được kiểm tra đầy đủ.
* Resource Scope chính xác.
* Backend tự kiểm tra quyền.
* Không Hard-code Role trong Business Logic.
* Default Policy là Deny.
* Export có kiểm tra quyền.
* Audit Log ghi nhận thay đổi quyền.
* Người dân không xem được dữ liệu người khác.
* Cán bộ không truy cập vượt phạm vi.
* Security Context được xác thực.
* Tuân thủ Least Privilege và Zero Trust.

---

# End of Phase 2B

Phase tiếp theo:

* OWASP Top 10
* Input Validation Standards
* SQL Injection Prevention
* Cross-Site Scripting (XSS)
* Cross-Site Request Forgery (CSRF)
* File Upload Security
* API Input Security
* Output Encoding
* Secure Coding Guidelines
# ============================================================================

# 29. OWASP TOP 10 COMPLIANCE

# ============================================================================

## 29.1 Purpose

Hệ thống AnSinhSo phải được thiết kế và phát triển nhằm giảm thiểu các rủi ro thuộc **OWASP Top 10**.

Mọi chức năng mới phải được đánh giá theo các nguy cơ bảo mật phổ biến trước khi đưa vào Production.

---

## 29.2 Security Baseline

Backend và Frontend phải đáp ứng tối thiểu:

* Input Validation
* Output Encoding
* Authentication
* Authorization
* Logging
* Audit
* Secure Configuration
* Error Handling
* Dependency Management

---

## 29.3 OWASP Coverage

Hệ thống phải có biện pháp phòng chống:

| OWASP Risk                         | Trạng thái                          |
| ---------------------------------- | ----------------------------------- |
| Broken Access Control              | Required                            |
| Cryptographic Failures             | Required                            |
| Injection                          | Required                            |
| Insecure Design                    | Required                            |
| Security Misconfiguration          | Required                            |
| Vulnerable Components              | Required                            |
| Authentication Failures            | Required                            |
| Software & Data Integrity Failures | Required                            |
| Logging & Monitoring Failures      | Required                            |
| Server-Side Request Forgery (SSRF) | Required khi tích hợp dịch vụ ngoài |

---

# ============================================================================

# 30. INPUT VALIDATION STANDARDS

# ============================================================================

## 30.1 Validation Principles

Mọi dữ liệu từ Client đều được xem là **không đáng tin cậy**.

Tất cả Request phải được kiểm tra trước khi xử lý.

---

## 30.2 Validation Levels

Áp dụng ba cấp:

* Client Validation
* API Validation
* Business Validation

Client Validation chỉ nhằm cải thiện trải nghiệm người dùng.

Backend luôn là nơi quyết định cuối cùng.

---

## 30.3 Validation Rules

Kiểm tra tối thiểu:

* Required
* Data Type
* Length
* Range
* Format
* Enumeration
* Business Rules

---

## 30.4 Validation Framework

Chuẩn sử dụng:

* FluentValidation
* Data Annotation (khi phù hợp)

Không viết Validation phân tán trong Controller.

---

## 30.5 Fail Fast

Khi Validation thất bại:

* Dừng xử lý ngay.
* Không tiếp tục Business Logic.
* Trả về mã lỗi phù hợp.

---

# ============================================================================

# 31. SQL INJECTION PREVENTION

# ============================================================================

## 31.1 Principles

Không cho phép nối chuỗi để tạo câu lệnh SQL.

Luôn sử dụng:

* Entity Framework Core
* Parameterized Query
* Stored Procedure có tham số

---

## 31.2 Forbidden

Không được viết:

```sql
SELECT * FROM Users
WHERE Username = '" + username + "'
```

---

## 31.3 Required

Ưu tiên:

* LINQ
* EF Core
* Parameterized Query

---

## 31.4 Raw SQL

Raw SQL chỉ được sử dụng khi:

* Có yêu cầu tối ưu hiệu năng.
* Đã được Code Review.
* Có Parameter đầy đủ.

---

# ============================================================================

# 32. CROSS-SITE SCRIPTING (XSS)

# ============================================================================

## 32.1 Purpose

Ngăn chặn việc thực thi mã JavaScript độc hại trên trình duyệt người dùng.

---

## 32.2 Prevention

Áp dụng:

* Output Encoding
* HTML Encoding
* JavaScript Encoding
* URL Encoding

Theo ngữ cảnh hiển thị dữ liệu.

---

## 32.3 Rich Text

Nếu hỗ trợ nội dung HTML:

* Chỉ cho phép danh sách thẻ an toàn.
* Loại bỏ Script.
* Loại bỏ Event Handler.
* Loại bỏ Inline JavaScript.

---

## 32.4 Frontend Rules

Không sử dụng:

* innerHTML

Ưu tiên:

* textContent
* Binding an toàn của Framework

---

# ============================================================================

# 33. CROSS-SITE REQUEST FORGERY (CSRF)

# ============================================================================

## 33.1 Scope

Đối với hệ thống sử dụng JWT Bearer qua Authorization Header, nguy cơ CSRF được giảm đáng kể.

Nếu sử dụng Cookie Authentication thì phải triển khai cơ chế chống CSRF.

---

## 33.2 Protection

Áp dụng khi cần:

* Anti-Forgery Token
* SameSite Cookie
* Origin Validation
* Referer Validation (khi phù hợp)

---

## 33.3 State Changing Requests

Các thao tác:

* POST
* PUT
* PATCH
* DELETE

phải được bảo vệ theo cơ chế xác thực tương ứng.

---

# ============================================================================

# 34. FILE UPLOAD SECURITY

# ============================================================================

## 34.1 Allowed Files

Chỉ cho phép các định dạng đã được phê duyệt.

Ví dụ:

* PDF
* DOCX
* XLSX
* CSV
* JPG
* PNG

---

## 34.2 Validation

Kiểm tra:

* MIME Type
* File Extension
* File Size
* File Signature (Magic Number khi cần)

---

## 34.3 Dangerous Files

Không cho phép tải lên:

* EXE
* DLL
* BAT
* CMD
* JS
* VBS
* SCR

hoặc các tệp thực thi khác.

---

## 34.4 File Name

Không sử dụng trực tiếp tên tệp do người dùng cung cấp để lưu trữ.

Ưu tiên:

* UUID
* GUID
* Generated File Name

---

## 34.5 Malware Scanning

Khuyến nghị tích hợp giải pháp quét mã độc trước khi lưu trữ lâu dài.

---

# ============================================================================

# 35. API INPUT SECURITY

# ============================================================================

## 35.1 Request Validation

Mọi API phải:

* Validate Request Model.
* Kiểm tra quyền.
* Kiểm tra Business Rule.

---

## 35.2 Payload Size

Giới hạn kích thước Request theo từng API.

Không cho phép Payload vượt cấu hình.

---

## 35.3 Unknown Fields

Các trường không được định nghĩa trong API Contract nên được bỏ qua hoặc từ chối theo chính sách của hệ thống.

---

## 35.4 Rate Abuse Protection

API nhạy cảm phải có cơ chế hạn chế tần suất truy cập để giảm nguy cơ lạm dụng.

---

# ============================================================================

# 36. OUTPUT ENCODING

# ============================================================================

## 36.1 Principles

Dữ liệu trả về phải được mã hóa phù hợp với ngữ cảnh hiển thị.

---

## 36.2 Encoding Context

Áp dụng:

* HTML Encoding
* JavaScript Encoding
* URL Encoding
* JSON Encoding

---

## 36.3 Error Messages

Không trả về:

* Stack Trace
* SQL Query
* Connection String
* Secret
* API Key

---

# ============================================================================

# 37. SECURE CODING GUIDELINES

# ============================================================================

## 37.1 General Rules

Developer phải:

* Validate mọi Input.
* Encode mọi Output phù hợp.
* Không Hard-code Secret.
* Không Hard-code Password.
* Không bỏ qua Exception.
* Không Disable Security.

---

## 37.2 Code Review

Code Review phải kiểm tra:

* SQL Injection
* XSS
* CSRF
* Broken Access Control
* Sensitive Data Exposure
* Error Handling
* Logging
* Audit

---

## 37.3 Third-Party Libraries

Chỉ sử dụng thư viện:

* Có nguồn gốc rõ ràng.
* Được cập nhật.
* Không có lỗ hổng nghiêm trọng đã biết.

---

# ============================================================================

# 38. APPLICATION SECURITY CHECKLIST

# ============================================================================

Trước khi phát hành cần kiểm tra:

* Tuân thủ OWASP Top 10.
* Input Validation đầy đủ.
* Không có SQL Injection.
* Không có XSS.
* CSRF được xử lý theo cơ chế xác thực.
* Upload File được kiểm tra.
* Không trả về thông tin nhạy cảm.
* Payload được giới hạn.
* Output được Encoding phù hợp.
* Thư viện phụ thuộc đã được rà soát.
* Secure Coding Guidelines được áp dụng.
* Hoàn thành Security Code Review.

---

# End of Phase 2C

Phase tiếp theo:

* Encryption Standards
* Cryptographic Standards
* Secret Management
* API Security
* Rate Limiting
* Logging Security
* Audit Security
* Monitoring & Alerting
* Security Headers
* Secure Configuration
# ============================================================================

# 39. ENCRYPTION STANDARDS

# ============================================================================

## 39.1 Purpose

Encryption bảo vệ dữ liệu khỏi việc truy cập trái phép trong quá trình lưu trữ và truyền tải.

Mọi dữ liệu nhạy cảm phải được đánh giá để xác định có cần mã hóa hay không.

---

## 39.2 Encryption Scope

Áp dụng cho:

* Personal Data
* Authentication Token
* Secret
* Backup
* Configuration
* API Communication

---

## 39.3 Encryption Types

Hệ thống sử dụng:

* Encryption At Rest
* Encryption In Transit

Khuyến nghị bổ sung:

* Encryption In Backup

---

## 39.4 Approved Algorithms

Khuyến nghị:

* AES-256
* RSA-2048 hoặc cao hơn (khi cần bất đối xứng)
* TLS 1.3

Không sử dụng:

* DES
* 3DES
* RC4

---

## 39.5 Key Rotation

Khóa mã hóa phải có khả năng thay đổi định kỳ.

Không sử dụng một khóa cố định trong toàn bộ vòng đời hệ thống.

---

# ============================================================================

# 40. CRYPTOGRAPHIC STANDARDS

# ============================================================================

## 40.1 Password Hashing

Chỉ sử dụng:

* Argon2
* BCrypt
* PBKDF2

Không sử dụng:

* MD5
* SHA1

---

## 40.2 Random Number Generation

Các giá trị sau phải được sinh bằng bộ sinh số ngẫu nhiên bảo mật:

* JWT Secret (khi tạo)
* Refresh Token
* API Key
* Reset Token
* Verification Token

---

## 40.3 Digital Signature

Các Token phải được ký số trước khi phát hành.

---

## 40.4 Cryptographic Agility

Thiết kế phải cho phép thay đổi thuật toán mã hóa mà không ảnh hưởng Business Logic.

---

# ============================================================================

# 41. SECRET MANAGEMENT

# ============================================================================

## 41.1 Principles

Secret bao gồm:

* JWT Secret
* API Key
* Database Password
* SMTP Password
* Zalo OA Secret
* AI Provider Key

Không lưu trực tiếp trong mã nguồn.

---

## 41.2 Secret Storage

Ưu tiên:

* Environment Variables
* Secret Manager
* Azure Key Vault
* AWS Secrets Manager
* HashiCorp Vault

---

## 41.3 Development Environment

Môi trường phát triển có thể sử dụng:

* User Secrets (.NET)
* Local Environment Variables

Không commit Secret lên Git.

---

## 41.4 Rotation

Secret phải có khả năng:

* Thay đổi
* Thu hồi
* Theo dõi lịch sử thay đổi

---

# ============================================================================

# 42. API SECURITY STANDARDS

# ============================================================================

## 42.1 HTTPS

Tất cả API Production phải sử dụng HTTPS.

Không hỗ trợ HTTP trong môi trường Production.

---

## 42.2 API Versioning

API phải hỗ trợ Versioning.

Ví dụ:

```text
/api/v1/citizens

/api/v2/citizens
```

---

## 42.3 Content-Type Validation

API chỉ chấp nhận Content-Type phù hợp.

Ví dụ:

* application/json
* multipart/form-data

---

## 42.4 Response Headers

API phải trả về các Security Header phù hợp.

---

## 42.5 API Documentation

Swagger chỉ nên công khai trong môi trường Development hoặc được bảo vệ bằng cơ chế xác thực phù hợp.

---

# ============================================================================

# 43. RATE LIMITING STANDARDS

# ============================================================================

## 43.1 Purpose

Giảm thiểu:

* Brute Force
* API Abuse
* DoS quy mô nhỏ
* Excessive Requests

---

## 43.2 Protected Endpoints

Áp dụng Rate Limiting cho:

* Login
* Refresh Token
* Password Reset
* OTP
* AI Service
* File Upload

---

## 43.3 Strategies

Có thể áp dụng:

* Fixed Window
* Sliding Window
* Token Bucket

Tùy theo từng Endpoint.

---

## 43.4 Response

Khi vượt giới hạn:

* HTTP 429
* Retry-After Header (nếu áp dụng)

---

# ============================================================================

# 44. SECURITY LOGGING

# ============================================================================

## 44.1 Security Events

Ghi nhận:

* Login Success
* Login Failure
* Password Change
* Permission Change
* Account Lock
* Token Revocation
* Access Denied

---

## 44.2 Log Protection

Log phải:

* Không sửa trực tiếp.
* Có thời gian.
* Có CorrelationId.
* Có UserId (nếu xác định được).

---

## 44.3 Sensitive Data

Không ghi:

* Password
* Secret
* Access Token đầy đủ
* Refresh Token
* API Key

Có thể Mask dữ liệu khi cần.

---

# ============================================================================

# 45. SECURITY AUDIT

# ============================================================================

## 45.1 Audit Scope

Audit các hành động:

* Tạo
* Cập nhật
* Xóa
* Phân quyền
* Đăng nhập
* Đăng xuất
* Xuất dữ liệu

---

## 45.2 Audit Fields

Bao gồm:

* UserId
* Action
* Entity
* EntityId
* Timestamp
* Result
* CorrelationId

---

## 45.3 Audit Integrity

Audit Log phải:

* Không chỉnh sửa.
* Không xóa trực tiếp.
* Có cơ chế lưu trữ lâu dài theo chính sách của hệ thống.

---

# ============================================================================

# 46. SECURITY MONITORING

# ============================================================================

## 46.1 Monitoring Scope

Theo dõi:

* Authentication
* Authorization
* API
* Database
* Background Jobs
* AI Services
* GIS Services
* Zalo OA

---

## 46.2 Alerting

Thiết lập cảnh báo cho:

* Nhiều lần đăng nhập thất bại.
* Tăng đột biến lỗi 401/403.
* Tăng bất thường lỗi 500.
* Tăng bất thường số lượng Request.

---

## 46.3 Incident Correlation

Các Log phải hỗ trợ liên kết theo:

* TraceId
* CorrelationId

để phục vụ điều tra sự cố.

---

# ============================================================================

# 47. SECURITY HEADERS

# ============================================================================

## 47.1 Recommended Headers

Khuyến nghị cấu hình:

* Strict-Transport-Security (HSTS)
* X-Content-Type-Options
* X-Frame-Options
* Referrer-Policy
* Content-Security-Policy
* Permissions-Policy

---

## 47.2 Server Information

Không để lộ:

* Phiên bản ASP.NET
* Phiên bản IIS/Kestrel
* Thông tin hệ điều hành

trong Response Header.

---

# ============================================================================

# 48. SECURE CONFIGURATION

# ============================================================================

## 48.1 Configuration Principles

Mọi cấu hình Production phải:

* Tối thiểu quyền.
* Không dùng giá trị mặc định.
* Không chứa Secret trong Repository.

---

## 48.2 Debug Settings

Production:

* Disable Debug.
* Disable Detailed Errors.
* Disable Developer Exception Page.

---

## 48.3 Environment Separation

Tách biệt cấu hình:

* Development
* Testing
* Staging
* Production

Không dùng chung Secret giữa các môi trường.

---

# ============================================================================

# 49. SECURITY CHECKLIST

# ============================================================================

Trước khi triển khai Production cần kiểm tra:

* HTTPS hoạt động.
* TLS cấu hình đúng.
* Secret không nằm trong Git.
* JWT Secret được quản lý an toàn.
* Password Hash đúng chuẩn.
* API có Rate Limiting.
* Security Header đầy đủ.
* Log không chứa dữ liệu nhạy cảm.
* Audit Log hoạt động.
* Monitoring hoạt động.
* Alerting hoạt động.
* Environment được tách biệt.
* Không bật Debug.
* Không lộ thông tin Server.
* Hoàn thành Security Review.

---

# End of Phase 2D

Phase tiếp theo:

* AI Security Standards
* GIS Security Standards
* Zalo OA Security Standards
* Database Security Standards
* Backup & Disaster Recovery
* Incident Response
* Vulnerability Management
* Penetration Testing
* Security Definition of Done
* Security Code Review Checklist
* AI Coding Security Rules
* Final Security Checklist
* Related Documents
* End of Document
````md
# ============================================================================
# 50. AI SECURITY STANDARDS
# ============================================================================

## 50.1 Purpose

AI là một thành phần hỗ trợ ra quyết định trong hệ thống AnSinhSo.

AI không được phép trở thành nguồn quyết định cuối cùng đối với các nghiệp vụ hành chính.

---

## 50.2 AI Security Principles

AI phải tuân thủ:

- Least Privilege
- Zero Trust
- Human-in-the-loop
- Explainability
- Auditability

---

## 50.3 Prompt Security

Không gửi cho AI:

- Password
- JWT
- Refresh Token
- Connection String
- Secret
- API Key
- Dữ liệu nhạy cảm không cần thiết

Mọi Prompt phải được kiểm soát trước khi gửi.

---

## 50.4 Output Validation

Kết quả AI:

- Không được sử dụng trực tiếp để ghi Database.
- Phải được kiểm tra.
- Phải được xác thực theo Business Rules.

---

## 50.5 AI Logging

Ghi nhận:

- AI Provider
- Model
- Prompt Id
- Request Time
- Response Time
- Token Usage
- Error

Không ghi Prompt chứa dữ liệu nhạy cảm.

---

# ============================================================================
# 51. GIS SECURITY STANDARDS
# ============================================================================

## 51.1 GIS Data Protection

Dữ liệu bản đồ phải được phân quyền theo:

- Vai trò
- Đơn vị hành chính
- Phạm vi quản lý

---

## 51.2 Coordinate Protection

Không công khai tọa độ chính xác nếu dữ liệu thuộc diện hạn chế.

Có thể làm giảm độ chính xác hoặc ẩn thông tin theo chính sách nghiệp vụ.

---

## 51.3 GIS API Security

GIS API phải:

- Authentication
- Authorization
- Rate Limiting
- Audit Logging

---

# ============================================================================
# 52. ZALO OFFICIAL ACCOUNT SECURITY
# ============================================================================

## 52.1 Access Token Management

Token của Zalo OA phải:

- Được lưu an toàn.
- Có cơ chế làm mới.
- Không ghi vào Log.

---

## 52.2 Webhook Validation

Webhook phải:

- Xác minh nguồn gửi.
- Kiểm tra chữ ký (nếu nền tảng hỗ trợ).
- Từ chối Request không hợp lệ.

---

## 52.3 Message Security

Không gửi qua Zalo:

- Password
- JWT
- Refresh Token
- Secret
- Thông tin cá nhân vượt phạm vi thông báo

---

## 52.4 Retry Strategy

Khi gửi thất bại:

- Retry theo chính sách.
- Ghi Log.
- Không gửi lặp vô hạn.

---

# ============================================================================
# 53. DATABASE SECURITY STANDARDS
# ============================================================================

## 53.1 Database Access

Chỉ Backend được phép truy cập Database.

Frontend không truy cập trực tiếp.

---

## 53.2 Database Account

Tài khoản Database:

- Không sử dụng tài khoản quản trị cho ứng dụng.
- Chỉ cấp quyền cần thiết.

---

## 53.3 Sensitive Data

Các trường nhạy cảm phải được đánh giá để:

- Hash
- Encrypt
- Mask

tùy theo mục đích sử dụng.

---

## 53.4 Backup Encryption

Các bản sao lưu chứa dữ liệu nhạy cảm nên được mã hóa và quản lý quyền truy cập.

---

# ============================================================================
# 54. BACKUP & DISASTER RECOVERY
# ============================================================================

## 54.1 Backup Policy

Thực hiện:

- Full Backup
- Differential Backup (nếu áp dụng)
- Transaction Log Backup (nếu áp dụng)

theo chính sách vận hành.

---

## 54.2 Backup Storage

Backup phải:

- Lưu tách biệt hệ thống chính.
- Kiểm soát quyền truy cập.
- Kiểm tra khả năng khôi phục định kỳ.

---

## 54.3 Disaster Recovery

Chuẩn bị:

- Recovery Procedure
- Recovery Time Objective (RTO)
- Recovery Point Objective (RPO)

Giá trị cụ thể do đơn vị vận hành xác định.

---

# ============================================================================
# 55. INCIDENT RESPONSE
# ============================================================================

## 55.1 Security Incident

Ví dụ:

- Rò rỉ dữ liệu.
- Token bị lộ.
- Tấn công Brute Force.
- SQL Injection.
- Malware.
- Ransomware.

---

## 55.2 Response Process

Quy trình:

```text
Detection

↓

Analysis

↓

Containment

↓

Eradication

↓

Recovery

↓

Post-Incident Review
```

---

## 55.3 Incident Logging

Mọi sự cố phải ghi nhận:

- Time
- Severity
- Impact
- Root Cause
- Resolution

---

# ============================================================================
# 56. VULNERABILITY MANAGEMENT
# ============================================================================

## 56.1 Dependency Review

Định kỳ rà soát:

- NuGet Packages
- JavaScript Libraries
- Docker Images (nếu áp dụng)

---

## 56.2 Patch Management

Áp dụng bản vá bảo mật theo quy trình kiểm thử trước khi triển khai Production.

---

## 56.3 Security Scan

Khuyến nghị:

- Static Application Security Testing (SAST)
- Dependency Scanning
- Secret Scanning

---

# ============================================================================
# 57. PENETRATION TESTING
# ============================================================================

## 57.1 Scope

Kiểm thử:

- Authentication
- Authorization
- API
- File Upload
- Database Access
- AI Integration
- GIS Integration
- Zalo OA Integration

---

## 57.2 Frequency

Khuyến nghị:

- Trước Production.
- Sau thay đổi lớn.
- Định kỳ theo chính sách vận hành.

---

# ============================================================================
# 58. SECURITY DEFINITION OF DONE
# ============================================================================

Một Module chỉ được xem là hoàn thành khi:

- Authentication đúng.
- Authorization đúng.
- Input Validation đầy đủ.
- Output Encoding đầy đủ.
- Không còn lỗ hổng nghiêm trọng đã biết.
- Có Logging.
- Có Audit.
- Có Security Review.
- Có Test.
- Đạt Code Review.

---

# ============================================================================
# 59. SECURITY CODE REVIEW CHECKLIST
# ============================================================================

Reviewer phải kiểm tra:

- Broken Access Control.
- Injection.
- XSS.
- CSRF.
- Authentication.
- Authorization.
- Logging.
- Audit.
- Encryption.
- Secret Management.
- API Security.
- Rate Limiting.
- Error Handling.
- Dependency Security.
- Secure Configuration.

---

# ============================================================================
# 60. AI CODING SECURITY RULES
# ============================================================================

Mọi AI Coding Assistant phải:

- Tuân thủ 09_SECURITY_STANDARDS.md.
- Không sinh Hard-code Secret.
- Không sinh Password mẫu trong mã nguồn.
- Không bỏ qua Validation.
- Không bỏ qua Authorization.
- Không sinh SQL Injection.
- Không sử dụng thuật toán Hash lỗi thời.
- Không ghi dữ liệu nhạy cảm vào Log.
- Không tạo API không có Authentication nếu không được yêu cầu rõ ràng.

Áp dụng cho:

- ChatGPT
- Gemini
- GitHub Copilot
- Cursor
- Cline
- Continue
- AntiGravity AI

---

# ============================================================================
# 61. FINAL SECURITY CHECKLIST
# ============================================================================

Trước khi triển khai Production cần xác nhận:

- HTTPS hoạt động.
- JWT hoạt động.
- RBAC hoạt động.
- Secret được quản lý an toàn.
- Password Hash đúng chuẩn.
- Input Validation đầy đủ.
- Output Encoding đầy đủ.
- Không còn SQL Injection.
- Không còn XSS.
- Không còn CSRF (theo cơ chế xác thực áp dụng).
- File Upload an toàn.
- Logging đầy đủ.
- Audit đầy đủ.
- Monitoring hoạt động.
- Backup hoạt động.
- Khả năng Restore đã được kiểm tra.
- AI Security được áp dụng.
- GIS Security được áp dụng.
- Zalo OA Security được áp dụng.
- Hoàn thành Penetration Testing.
- Hoàn thành Security Review.

---

# ============================================================================
# 62. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------------|--------------------------------|
| 1.0.0 | 2026-07-12 | Initial Enterprise Security Standards |

---

# ============================================================================
# 63. RELATED DOCUMENTS
# ============================================================================

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_PROJECT_INDEX.md
- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 07_FRONTEND_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- CLEAN_ARCHITECTURE.md
- DATABASE_DESIGN.md
- API_SPEC.md
- BUSINESS_RULES.md
- DEPLOYMENT_GUIDE.md

---

# ============================================================================
# END OF DOCUMENT
# ============================================================================
````
