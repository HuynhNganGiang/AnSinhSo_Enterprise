```md
# 20_SECURITY_ARCHITECTURE.md

> Enterprise Security Architecture
>
> Project: AnSinhSo – Digital Social Welfare System
>
> Version: 1.0
>
> Status: Production Design

------------------------------------------------------------------------------

# PHASE 1 – SECURITY OVERVIEW

------------------------------------------------------------------------------

# 1.1 Purpose

Tài liệu này mô tả kiến trúc bảo mật tổng thể của hệ thống AnSinhSo.

Mục tiêu:

- Bảo vệ dữ liệu công dân
- Bảo vệ dữ liệu trợ cấp
- Bảo vệ API
- Bảo vệ AI
- Bảo vệ GIS
- Bảo vệ Zalo OA
- Đáp ứng yêu cầu triển khai Production

------------------------------------------------------------------------------

# 1.2 Security Objectives

Hệ thống phải đảm bảo:

✓ Confidentiality

✓ Integrity

✓ Availability

✓ Authentication

✓ Authorization

✓ Accountability

✓ Auditability

------------------------------------------------------------------------------

# 1.3 Security Principles

Zero Trust

Least Privilege

Defense in Depth

Secure by Default

Fail Secure

Security by Design

Audit by Default

Encryption Everywhere

------------------------------------------------------------------------------

# 1.4 Security Domains

Identity Security

API Security

Database Security

AI Security

GIS Security

Notification Security

Infrastructure Security

Application Security

------------------------------------------------------------------------------

# 1.5 Security Layers

                 Internet
                     │
             Reverse Proxy
                     │
             Web Application Firewall
                     │
              ASP.NET Core API
                     │
      Authentication Middleware
                     │
      Authorization Middleware
                     │
        Business Services Layer
                     │
        Repository Layer
                     │
             SQL Server

------------------------------------------------------------------------------

# 1.6 Security Components

Authentication

Authorization

JWT

Refresh Token

Role-Based Access Control

Permission-Based Access Control

Audit Logging

Rate Limiting

Encryption

Secret Management

------------------------------------------------------------------------------

# 1.7 Authentication

Phương thức:

- JWT Access Token
- Refresh Token

Hỗ trợ:

- Token Rotation
- Token Revocation
- Session Expiration

------------------------------------------------------------------------------

# 1.8 Authorization Model

Role

↓

Permission

↓

Policy

↓

Endpoint

Không kiểm tra quyền trực tiếp trong Controller.

Sử dụng Policy-Based Authorization.

------------------------------------------------------------------------------

# 1.9 Roles

Administrator

Lãnh đạo

Cán bộ xã

Kế toán

Người dân

AI Service

System Worker

------------------------------------------------------------------------------

# 1.10 Permission Groups

User.*

Household.*

Citizen.*

Policy.*

Payment.*

Dashboard.*

GIS.*

AI.*

Notification.*

Audit.*

Administration.*

------------------------------------------------------------------------------

# 1.11 API Security

Mọi API phải:

✓ HTTPS

✓ JWT

✓ Permission Check

✓ Validation

✓ Audit

✓ Logging

✓ Rate Limiting

------------------------------------------------------------------------------

# 1.12 Database Security

Không cho phép:

Business Layer

↓

Raw SQL

Chỉ được thông qua:

Repository

↓

EF Core

↓

SQL Server

------------------------------------------------------------------------------

# 1.13 Secret Management

Không lưu:

API Key

Connection String

JWT Secret

OA Secret

trong source code.

Sử dụng:

Environment Variables

Secret Store

------------------------------------------------------------------------------

# 1.14 Encryption

Dữ liệu truyền

TLS 1.3

Dữ liệu nhạy cảm

AES-256

Password

Argon2id hoặc BCrypt

------------------------------------------------------------------------------

# 1.15 Audit Logging

Lưu:

Ai

Làm gì

Lúc nào

Ở đâu

IP

Thiết bị

Request

Response Code

------------------------------------------------------------------------------

# END OF PHASE 1
```
```md
# =============================================================================
# PHASE 2 – IDENTITY & ACCESS MANAGEMENT (IAM)
# =============================================================================

---

# 2.1 Overview

Identity & Access Management (IAM) chịu trách nhiệm xác thực (Authentication),
phân quyền (Authorization) và quản lý danh tính của tất cả người dùng trong hệ
thống AnSinhSo.

IAM là nền tảng bảo mật cốt lõi của toàn bộ hệ thống.

---

# 2.2 Security Objectives

IAM phải đảm bảo:

✓ Người dùng được xác thực trước khi truy cập

✓ Người dùng chỉ truy cập đúng phạm vi dữ liệu

✓ Hỗ trợ phân quyền linh hoạt

✓ Có khả năng mở rộng

✓ Theo dõi toàn bộ lịch sử đăng nhập

✓ Hỗ trợ tích hợp AI và Zalo OA

---

# 2.3 Identity Types

Hệ thống hỗ trợ các loại định danh sau:

## Internal Users

- Administrator
- Lãnh đạo
- Cán bộ xã
- Kế toán

---

## External Users

- Người dân

---

## System Accounts

- AI Worker
- Notification Worker
- Scheduler
- Background Service

---

## Third-party Services

- Zalo OA
- AI Provider
- GIS Service

---

# 2.4 Authentication Architecture

User

↓

Login API

↓

Authentication Service

↓

JWT Generator

↓

Access Token

↓

Refresh Token

↓

Protected APIs

---

# 2.5 Authentication Methods

Internal Users

Username + Password

JWT

Refresh Token

---

Citizens

CCCD/Số định danh + OTP (nếu triển khai)

Hoặc

Zalo OA Identity Mapping

---

System Services

Service Account

API Key

Mutual Authentication (Future)

---

# 2.6 Authorization Model

Role

↓

Permission

↓

Policy

↓

Endpoint

↓

Business Rule

Không kiểm tra quyền bằng if(Role == ...).

Toàn bộ sử dụng Policy-Based Authorization.

---

# 2.7 Roles

Administrator

Lãnh đạo

Cán bộ xã

Kế toán

Người dân

AI Service

Notification Service

System Worker

---

# 2.8 Permission Matrix

User.*

Role.*

Permission.*

Household.*

Citizen.*

Policy.*

Payment.*

Dashboard.*

GIS.*

AI.*

Notification.*

Audit.*

Report.*

Configuration.*

---

# 2.9 Resource-Level Authorization

Ngoài Role và Permission, hệ thống phải kiểm tra phạm vi dữ liệu.

Ví dụ:

- Cán bộ chỉ xem dữ liệu thuộc địa bàn được phân công.
- Lãnh đạo xem toàn xã.
- Người dân chỉ xem hồ sơ của chính mình.

---

# 2.10 Token Strategy

Access Token

- Thời gian sống: 15–30 phút

Refresh Token

- Thời gian sống: 7–30 ngày (cấu hình)

Yêu cầu:

- Rotation
- Revocation
- Blacklist khi cần

---

# 2.11 Session Management

Lưu:

- SessionId
- UserId
- LoginTime
- ExpiredTime
- Device
- Browser
- IP
- LastActivity

Cho phép quản trị viên thu hồi phiên đăng nhập.

---

# 2.12 Password Policy

- Độ dài tối thiểu: 10 ký tự
- Có chữ hoa
- Có chữ thường
- Có số
- Có ký tự đặc biệt

Mật khẩu lưu bằng:

Argon2id

Hoặc

BCrypt

Không lưu mật khẩu thuần văn bản.

---

# 2.13 Account Lockout

Ví dụ:

Sai mật khẩu liên tiếp:

5 lần

↓

Khóa 15 phút

Có thể cấu hình.

---

# 2.14 Multi-Factor Authentication (Future)

Có thể mở rộng:

- Email OTP
- SMS OTP
- Zalo OTP
- Authenticator App

---

# 2.15 Audit Requirements

Ghi nhận:

- Login
- Logout
- Login Failed
- Password Changed
- Role Changed
- Permission Changed
- Token Revoked

---

# 2.16 Security Events

Các sự kiện cần theo dõi:

- Nhiều lần đăng nhập thất bại
- Đăng nhập từ thiết bị mới
- Đăng nhập từ IP bất thường
- Token bị thu hồi
- Quyền truy cập bị từ chối

---

# 2.17 API Requirements

POST /api/auth/login

POST /api/auth/refresh

POST /api/auth/logout

POST /api/auth/change-password

POST /api/auth/revoke

GET  /api/auth/profile

GET  /api/auth/sessions

---

# 2.18 Database Mapping

Tables

Users

Roles

Permissions

RolePermissions

UserRoles

RefreshTokens

UserSessions

AuditLogs

---

# 2.19 Enterprise Best Practices

✓ JWT ngắn hạn

✓ Refresh Token Rotation

✓ Policy-Based Authorization

✓ Permission-Based Access Control

✓ Audit đầy đủ

✓ Session Tracking

✓ Zero Trust

✓ Least Privilege

---

# 2.20 Expected Deliverables

Sau Phase 2, hệ thống có:

✓ Enterprise Authentication

✓ Enterprise Authorization

✓ Session Management

✓ Token Management

✓ Permission Framework

✓ Audit Framework

✓ Production Ready IAM

# =============================================================================
# END OF PHASE 2
# =============================================================================
```
```md
# =============================================================================
# PHASE 3 – API SECURITY & ZERO TRUST ARCHITECTURE
# =============================================================================

---

# 3.1 Overview

AnSinhSo được xây dựng theo mô hình **API First**.

Mọi thành phần của hệ thống đều giao tiếp thông qua API:

- Web Dashboard
- AI Integration Hub
- GIS Services
- Zalo OA Integration
- Notification Center
- Background Workers

Do đó, API Security là tuyến phòng thủ quan trọng nhất của hệ thống.

Kiến trúc bảo mật áp dụng mô hình **Zero Trust**:

> Không có request nào được mặc định tin cậy, kể cả request đến từ mạng nội bộ.

---

# 3.2 Zero Trust Principles

Hệ thống áp dụng các nguyên tắc:

- Never Trust, Always Verify
- Least Privilege Access
- Verify Explicitly
- Assume Breach
- Continuous Validation
- Secure by Default

Mỗi request phải được xác minh độc lập.

---

# 3.3 API Request Pipeline

Client

↓

HTTPS

↓

Reverse Proxy

↓

Rate Limiter

↓

Authentication Middleware

↓

Authorization Middleware

↓

Request Validation

↓

Business Service

↓

Repository

↓

Database

↓

Audit Log

↓

Response

---

# 3.4 API Security Layers

Layer 1

Network Security

- HTTPS
- TLS
- Reverse Proxy
- Firewall

---

Layer 2

Identity Security

- JWT
- Refresh Token
- Session Validation

---

Layer 3

Authorization

- Role
- Permission
- Policy

---

Layer 4

Input Validation

- DTO Validation
- FluentValidation
- Request Size Limit
- File Validation

---

Layer 5

Business Rule Validation

Ví dụ:

- Không chi trả hai lần cho cùng một kỳ.
- Không sửa hồ sơ đã khóa.
- Không truy cập dữ liệu ngoài phạm vi được phân quyền.

---

Layer 6

Audit Logging

Lưu toàn bộ lịch sử thao tác phục vụ kiểm tra và truy vết.

---

# 3.5 HTTPS Requirements

Bắt buộc:

- HTTPS Only
- TLS 1.3 (ưu tiên)
- HSTS
- Secure Cookies
- HTTP → HTTPS Redirect

Không cho phép API hoạt động qua HTTP trong môi trường Production.

---

# 3.6 JWT Validation

Mỗi Access Token phải kiểm tra:

- Issuer
- Audience
- Signature
- Expiration
- Not Before
- Token Revocation

Token không hợp lệ phải bị từ chối ngay.

---

# 3.7 Policy-Based Authorization

Không sử dụng:

if (Role == "Admin")

Thay vào đó:

- Authorization Policies
- Permission Handlers
- Custom Authorization Requirements

Điều này giúp mở rộng dễ dàng khi bổ sung vai trò hoặc quyền mới.

---

# 3.8 Input Validation

Mọi API phải:

- Validate Model
- Validate Business Rules
- Chuẩn hóa dữ liệu đầu vào
- Từ chối dữ liệu không hợp lệ

Không xử lý trực tiếp dữ liệu chưa được kiểm tra.

---

# 3.9 Output Security

Không trả về:

- Stack Trace
- Connection String
- Exception nội bộ
- SQL Error
- Secret Key
- Token nội bộ

Chỉ trả về mã lỗi và thông báo phù hợp.

---

# 3.10 Rate Limiting

Áp dụng theo:

- User
- IP
- API Endpoint
- Client Application

Ví dụ:

Login API

- 5 yêu cầu/phút/IP

Search API

- 60 yêu cầu/phút/người dùng

AI Chat API

- Theo quota được cấu hình

---

# 3.11 CORS Policy

Chỉ cho phép các Origin đã được cấu hình.

Không sử dụng:

AllowAnyOrigin()

trong môi trường Production.

---

# 3.12 Security Headers

Thiết lập:

- HSTS
- X-Content-Type-Options
- X-Frame-Options
- Referrer-Policy
- Content-Security-Policy (nếu phù hợp)
- Permissions-Policy

---

# 3.13 File Upload Security

Kiểm tra:

- Loại tệp
- Kích thước
- Tên tệp
- Phần mở rộng
- MIME Type

Lưu tệp ngoài thư mục thực thi của ứng dụng.

---

# 3.14 API Versioning

Sử dụng:

/api/v1/...

Chuẩn bị khả năng mở rộng:

/api/v2/...

Không thay đổi hành vi của phiên bản cũ khi phát hành phiên bản mới.

---

# 3.15 Audit Requirements

Mỗi request cần lưu:

- RequestId
- UserId
- Endpoint
- Method
- StatusCode
- Duration
- IP
- User Agent
- CorrelationId

---

# 3.16 Security Monitoring

Theo dõi:

- Failed Login
- Unauthorized Access
- Forbidden Requests
- High Error Rate
- Rate Limit Violations
- Token Validation Failures

---

# 3.17 Incident Response

Khi phát hiện bất thường:

- Ghi Audit
- Gửi cảnh báo
- Có thể khóa tài khoản (theo cấu hình)
- Thu hồi Token (nếu cần)
- Hỗ trợ điều tra sau sự cố

---

# 3.18 Integration Security

Các hệ thống tích hợp như:

- Zalo OA
- AI Provider
- GIS Services

phải sử dụng:

- HTTPS
- API Key hoặc OAuth (tùy dịch vụ)
- Secret Management
- Logging
- Timeout
- Retry có kiểm soát

---

# 3.19 Testing Requirements

Kiểm thử:

- Authentication
- Authorization
- Rate Limiting
- JWT Validation
- CORS
- Security Headers
- File Upload
- API Versioning

Bao gồm cả kiểm thử trường hợp thành công và thất bại.

---

# 3.20 Expected Deliverables

Sau Phase 3:

✓ Zero Trust API Architecture

✓ API Security Standards

✓ Rate Limiting Strategy

✓ Secure Request Pipeline

✓ Enterprise Audit Logging

✓ Production-Ready API Security

# =============================================================================
# END OF PHASE 3
# =============================================================================
```
# =============================================================================

# PHASE 4 – DATA SECURITY ARCHITECTURE

# =============================================================================

---

# 4.1 Overview

Dữ liệu của AnSinhSo bao gồm nhiều thông tin quan trọng như:

* Thông tin hộ gia đình
* Thông tin công dân
* Thông tin trợ cấp
* Hồ sơ chi trả
* Nhật ký hệ thống
* Dữ liệu AI
* Dữ liệu GIS

Do đó, mọi dữ liệu phải được phân loại, bảo vệ và kiểm soát truy cập theo mức độ nhạy cảm.

---

# 4.2 Data Classification

Hệ thống phân loại dữ liệu thành bốn mức:

### Public

Thông tin có thể công khai.

Ví dụ:

* Tin tức
* Chính sách đã công bố
* Hướng dẫn sử dụng

---

### Internal

Thông tin chỉ dành cho cán bộ.

Ví dụ:

* Dashboard nội bộ
* Báo cáo tổng hợp

---

### Confidential

Thông tin nghiệp vụ.

Ví dụ:

* Hồ sơ hộ gia đình
* Danh sách đối tượng an sinh
* Lịch sử chi trả

---

### Restricted

Thông tin nhạy cảm.

Ví dụ:

* CCCD
* Số điện thoại
* Thông tin tài khoản
* Access Token
* Refresh Token
* Secret cấu hình

---

# 4.3 Data Encryption

Dữ liệu khi truyền:

* HTTPS
* TLS 1.3

Dữ liệu lưu trữ:

* AES-256 (đối với trường cần mã hóa)

Mật khẩu:

* Argon2id (ưu tiên)
* BCrypt (phương án thay thế)

---

# 4.4 Personally Identifiable Information (PII)

Các trường sau được xem là dữ liệu định danh cá nhân:

* Họ và tên
* CCCD/Số định danh
* Ngày sinh
* Địa chỉ
* Số điện thoại

Chỉ người có quyền phù hợp mới được xem đầy đủ các trường này.

---

# 4.5 Data Access Rules

Mọi truy cập dữ liệu phải đáp ứng đồng thời:

* Đã xác thực
* Có quyền phù hợp
* Thuộc phạm vi địa bàn được phân công (nếu áp dụng)
* Được ghi Audit Log

---

# 4.6 Backup Protection

Bản sao lưu phải:

* Được mã hóa
* Kiểm tra khả năng khôi phục định kỳ
* Lưu theo chính sách lưu giữ
* Hạn chế quyền truy cập

---

# 4.7 Data Retention

Xác định thời gian lưu trữ cho từng loại dữ liệu theo quy định của cơ quan quản lý và nhu cầu vận hành.

Không xóa dữ liệu khi chưa hết thời gian lưu giữ được quy định.

---

# 4.8 Expected Deliverables

Sau Phase 4:

✓ Data Classification

✓ Encryption Strategy

✓ PII Protection

✓ Backup Security

✓ Data Retention Policy

# =============================================================================

# END OF PHASE 4

# =============================================================================
```md
# =============================================================================
# PHASE 5 – INFRASTRUCTURE SECURITY
# =============================================================================

## 5.1 Overview

Infrastructure Security bảo vệ toàn bộ hạ tầng vận hành AnSinhSo, bao gồm:

- Máy chủ ứng dụng
- SQL Server
- Redis Cache (nếu sử dụng)
- File Storage
- Reverse Proxy
- IIS
- Mạng nội bộ

Mục tiêu là đảm bảo tính bảo mật, ổn định và khả năng phục hồi của hệ thống.

---

## 5.2 Security Objectives

Infrastructure phải đảm bảo:

✓ High Availability

✓ Secure Configuration

✓ Least Privilege

✓ Network Isolation

✓ Backup Protection

✓ Continuous Monitoring

---

## 5.3 Network Security

- Chỉ mở các cổng dịch vụ cần thiết.
- Chặn toàn bộ lưu lượng không hợp lệ bằng Firewall.
- Phân tách vùng mạng cho Web, Database và các dịch vụ nội bộ khi có điều kiện triển khai.
- Không công khai SQL Server trực tiếp ra Internet.

---

## 5.4 Reverse Proxy

Reverse Proxy phải hỗ trợ:

- HTTPS Termination
- Request Forwarding
- Header Validation
- Rate Limiting
- Compression (nếu phù hợp)
- Logging

---

## 5.5 IIS Security

- Chỉ chạy Application Pool chuyên biệt cho AnSinhSo.
- Tắt các module không sử dụng.
- Không hiển thị thông tin phiên bản.
- Cấu hình HTTPS bắt buộc.

---

## 5.6 Database Infrastructure Security

- Chỉ Web API được phép truy cập SQL Server.
- Không cho phép truy cập trực tiếp từ Internet.
- Tài khoản kết nối CSDL có quyền tối thiểu.

---

## 5.7 Backup Security

- Sao lưu định kỳ.
- Mã hóa bản sao lưu.
- Kiểm tra khả năng khôi phục.
- Giới hạn quyền truy cập file backup.

---

## 5.8 Infrastructure Checklist

✓ Firewall

✓ HTTPS

✓ Reverse Proxy

✓ SQL Server Protection

✓ Backup

✓ Health Check

✓ Monitoring

---

# =============================================================================
# END OF PHASE 5
# =============================================================================

# =============================================================================
# PHASE 6 – AI SECURITY
# =============================================================================

## 6.1 Overview

AI Security đảm bảo việc tích hợp AI không làm rò rỉ dữ liệu hoặc vượt quá phạm vi được phân quyền.

---

## 6.2 Security Objectives

✓ Chỉ truy xuất dữ liệu được phép.

✓ Không cho AI tự ý suy diễn thông tin nội bộ.

✓ Có khả năng truy xuất nguồn trả lời.

✓ Ghi đầy đủ nhật ký yêu cầu AI.

---

## 6.3 AI Request Flow

User

↓

Authentication

↓

Authorization

↓

Retriever

↓

Knowledge Base

↓

AI Provider

↓

Response Filter

↓

Audit Log

↓

User

---

## 6.4 Prompt Security

- Prompt được quản lý tập trung.
- Có phiên bản.
- Chỉ người có quyền mới được chỉnh sửa Prompt hệ thống.

---

## 6.5 Knowledge Base Security

Nguồn dữ liệu chỉ được lập chỉ mục từ:

- BUSINESS_RULES.md
- DATABASE_DESIGN.md
- API_SPEC.md
- USER_GUIDE.md
- FAQ
- Văn bản được phê duyệt

Không lập chỉ mục dữ liệu chưa được kiểm duyệt.

---

## 6.6 AI Output Filtering

Kiểm tra phản hồi AI để:

- Loại bỏ dữ liệu nhạy cảm.
- Ngăn lộ thông tin ngoài phạm vi quyền.
- Bổ sung nguồn tham chiếu nếu có.

---

## 6.7 AI Audit

Lưu:

- UserId
- Prompt
- Provider
- Response Time
- Citation
- Feedback

---

## 6.8 AI Security Checklist

✓ Prompt Versioning

✓ Citation

✓ Permission Check

✓ Audit

✓ Response Filtering

---

# =============================================================================
# END OF PHASE 6
# =============================================================================

# =============================================================================
# PHASE 7 – GIS SECURITY
# =============================================================================

## 7.1 Overview

GIS Security bảo vệ dữ liệu bản đồ và thông tin địa lý của hệ thống.

---

## 7.2 Security Objectives

✓ Chỉ hiển thị dữ liệu theo quyền.

✓ Không công khai tọa độ hoặc lớp dữ liệu nhạy cảm nếu không cần thiết.

✓ Theo dõi mọi truy cập GIS.

---

## 7.3 GIS Access Control

Phân quyền theo:

- Vai trò
- Địa bàn phụ trách
- Chức năng
- Lớp dữ liệu (Layer)

---

## 7.4 Layer Security

Ví dụ:

Public Layer

- Ranh giới hành chính
- Thông tin công khai

Restricted Layer

- Dữ liệu hộ gia đình
- Điểm chi trả
- Thông tin nghiệp vụ

---

## 7.5 GIS API Security

- HTTPS
- JWT
- Permission Check
- Rate Limiting
- Audit Logging

---

## 7.6 GIS Data Integrity

Không cho phép chỉnh sửa dữ liệu bản đồ nếu:

- Không có quyền.
- Dữ liệu đã được khóa.
- Không có lịch sử thay đổi.

---

## 7.7 GIS Audit

Lưu:

- UserId
- Layer
- Thời gian
- Hành động
- Địa chỉ IP

---

## 7.8 GIS Security Checklist

✓ Layer Security

✓ Access Control

✓ API Security

✓ Audit

✓ Data Integrity

# =============================================================================
# END OF PHASE 7
# =============================================================================
```
```md id="ansinhso-security-phase8-10"
# =============================================================================
# PHASE 8 – ZALO OFFICIAL ACCOUNT SECURITY
# =============================================================================

---

# 8.1 Overview

Zalo Official Account (OA) là kênh giao tiếp chính giữa hệ thống AnSinhSo và người dân.

Do đó, toàn bộ việc tích hợp Zalo OA phải được bảo vệ theo nguyên tắc:

- Secure Integration
- Least Privilege
- Zero Trust
- Audit by Default

Business Layer KHÔNG được phép gọi trực tiếp Zalo API.

Mọi kết nối phải đi qua:

Messaging Gateway

↓

Zalo Provider

↓

Zalo Official API

---

# 8.2 Security Objectives

Hệ thống phải đảm bảo:

✓ OA Token Protection

✓ Webhook Verification

✓ Signature Validation

✓ Request Validation

✓ Rate Limiting

✓ Audit Logging

✓ Retry Protection

---

# 8.3 Webhook Security

Webhook phải kiểm tra:

- HTTPS
- Request Method
- Signature
- Timestamp
- Replay Protection

Không xử lý Request không hợp lệ.

---

# 8.4 Webhook Validation Pipeline

Incoming Request

↓

HTTPS

↓

Signature Validation

↓

Timestamp Validation

↓

Payload Validation

↓

Authentication

↓

Business Processing

↓

Audit Log

---

# 8.5 Replay Attack Protection

Mỗi Request phải có:

- Unique Message ID
- Timestamp
- Expiration Window

Không xử lý lại Request đã nhận.

---

# 8.6 OA Token Security

Không lưu:

Access Token

Refresh Token

OA Secret

trong Source Code.

Sử dụng:

Environment Variables

Secret Store

Key Vault (Future)

---

# 8.7 Message Validation

Kiểm tra:

- Message Type
- Sender
- Receiver
- Payload Size
- Encoding

---

# 8.8 Delivery Security

Theo dõi:

Pending

Queued

Sending

Delivered

Failed

Retry

Dead Letter

---

# 8.9 Zalo Security Checklist

✓ HTTPS

✓ Webhook Validation

✓ Signature Validation

✓ Replay Protection

✓ Secret Management

✓ Audit Logging

✓ Retry

---

# =============================================================================
# END OF PHASE 8
# =============================================================================



# =============================================================================
# PHASE 9 – SECRET MANAGEMENT
# =============================================================================

---

# 9.1 Overview

Secret Management đảm bảo mọi thông tin nhạy cảm được lưu trữ và sử dụng an toàn.

---

# 9.2 Managed Secrets

Bao gồm:

- JWT Secret
- Database Connection String
- Redis Password
- Zalo OA Secret
- AI API Key
- GIS API Key
- SMTP Password
- Encryption Keys

---

# 9.3 Secret Storage

Production

↓

Environment Variables

↓

Secret Store

↓

Key Vault (Future)

Không lưu Secret trong:

- Source Code
- Git Repository
- appsettings.json (Production)

---

# 9.4 Secret Rotation

Định kỳ thay đổi:

- JWT Secret
- API Key
- OA Token
- Service Account Password

Hỗ trợ Rotation mà không cần thay đổi Business Layer.

---

# 9.5 Secret Access Policy

Chỉ:

- Web API
- Worker Services
- Infrastructure Services

được truy cập Secret theo đúng phạm vi.

---

# 9.6 Encryption Keys

Khóa mã hóa phải:

- Được lưu riêng
- Có phiên bản
- Có chính sách thay đổi
- Có quy trình khôi phục

---

# 9.7 Configuration Hierarchy

Development

↓

User Secrets

Testing

↓

Environment Variables

Production

↓

Secret Store

↓

Key Vault (Future)

---

# 9.8 Secret Audit

Theo dõi:

- Secret Access
- Secret Rotation
- Secret Expiration
- Failed Access

---

# 9.9 Secret Management Checklist

✓ Environment Variables

✓ Secret Store

✓ Rotation

✓ Encryption

✓ Access Control

✓ Audit

---

# =============================================================================
# END OF PHASE 9
# =============================================================================



# =============================================================================
# PHASE 10 – AUDIT & COMPLIANCE
# =============================================================================

---

# 10.1 Overview

Audit giúp theo dõi toàn bộ hoạt động của hệ thống để:

- Kiểm tra
- Điều tra sự cố
- Đảm bảo tính minh bạch
- Hỗ trợ vận hành

---

# 10.2 Audit Principles

Audit by Default

Immutable Logs

Time Synchronization

Traceability

Least Privilege

---

# 10.3 Audit Scope

Ghi nhận:

Authentication

Authorization

Data Changes

Payment Processing

Notification

AI Requests

GIS Operations

System Configuration

---

# 10.4 Audit Information

Mỗi bản ghi bao gồm:

- AuditId
- Timestamp
- UserId
- Role
- Action
- Entity
- EntityId
- Result
- IP Address
- User Agent
- CorrelationId

---

# 10.5 Audit Categories

Authentication

Authorization

Business

Database

AI

GIS

Notification

Security

Infrastructure

Administration

---

# 10.6 Compliance Requirements

Đảm bảo:

- Dữ liệu có thể truy vết
- Không sửa trực tiếp Audit Log
- Đồng bộ thời gian hệ thống
- Nhật ký có khả năng tìm kiếm

---

# 10.7 Log Retention

Audit Log được lưu theo chính sách của đơn vị quản lý.

Không xóa hoặc chỉnh sửa bản ghi Audit ngoài quy trình được phê duyệt.

---

# 10.8 Audit Monitoring

Theo dõi:

- Failed Login
- Permission Denied
- Mass Update
- Data Export
- AI Usage
- Notification Failure
- Security Events

---

# 10.9 Audit API

GET /api/audit/logs

GET /api/audit/events

GET /api/audit/security

GET /api/audit/user/{id}

GET /api/audit/export

---

# 10.10 Audit Best Practices

✓ Immutable Logs

✓ Centralized Logging

✓ Correlation ID

✓ Searchable Logs

✓ Time Synchronization

✓ Role-Based Access

---

# 10.11 Expected Deliverables

Sau Phase 10:

✓ Enterprise Audit Framework

✓ Compliance Guidelines

✓ Centralized Audit Logging

✓ Traceability

✓ Security Investigation Support

# =============================================================================
# END OF PHASE 10
# =============================================================================
```
```md id="ansinhso-security-phase11-13"
# =============================================================================
# PHASE 11 – SECURITY MONITORING
# =============================================================================

---

# 11.1 Overview

Security Monitoring chịu trách nhiệm giám sát liên tục trạng thái bảo mật của hệ thống AnSinhSo nhằm phát hiện sớm các hành vi bất thường, hỗ trợ điều tra và nâng cao khả năng phản ứng.

Monitoring phải hoạt động theo thời gian thực đối với các thành phần quan trọng.

---

# 11.2 Objectives

Hệ thống phải có khả năng:

✓ Phát hiện truy cập trái phép

✓ Theo dõi đăng nhập thất bại

✓ Giám sát API

✓ Giám sát AI

✓ Giám sát Zalo OA

✓ Giám sát Database

✓ Giám sát Infrastructure

✓ Cảnh báo sự cố

---

# 11.3 Monitoring Scope

Bao gồm:

Authentication

Authorization

API

Database

Background Workers

AI Integration Hub

Notification Center

Messaging Gateway

GIS

Infrastructure

---

# 11.4 Security Metrics

Theo dõi:

Failed Login Count

Permission Denied

Token Validation Failure

Rate Limit Violations

Database Access Errors

Webhook Validation Errors

Retry Queue Size

Dead Letter Queue Size

AI Provider Failures

Notification Failure Rate

---

# 11.5 Alert Levels

Information

↓

Warning

↓

Error

↓

Critical

Mỗi mức có quy trình xử lý và người chịu trách nhiệm phù hợp.

---

# 11.6 Monitoring Dashboard

Dashboard hiển thị:

- Tổng số sự kiện bảo mật
- API Health
- Worker Status
- Queue Status
- AI Requests
- Notification Success Rate
- Database Connectivity
- Infrastructure Health

---

# 11.7 Alert Channels

Có thể cấu hình:

- Dashboard
- Email
- Zalo OA (Thông báo nội bộ)
- Nhật ký hệ thống

---

# 11.8 Monitoring Best Practices

✓ Health Check

✓ Metrics Collection

✓ Centralized Logging

✓ Correlation ID

✓ Alert Threshold

✓ Continuous Monitoring

---

# 11.9 Expected Deliverables

✓ Security Dashboard

✓ Alert Framework

✓ Security Metrics

✓ Health Monitoring

# =============================================================================
# END OF PHASE 11
# =============================================================================



# =============================================================================
# PHASE 12 – INCIDENT RESPONSE
# =============================================================================

---

# 12.1 Overview

Incident Response mô tả quy trình xử lý khi xảy ra sự cố bảo mật hoặc sự cố hệ thống.

Mục tiêu là giảm thiểu tác động và khôi phục dịch vụ nhanh nhất có thể.

---

# 12.2 Incident Classification

Low

Medium

High

Critical

---

# 12.3 Incident Lifecycle

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

Post Incident Review

---

# 12.4 Common Security Incidents

- Đăng nhập bất thường
- Tấn công Brute Force
- Token bị lộ
- Truy cập trái phép
- AI Provider lỗi
- Zalo OA Webhook lỗi
- SQL Server mất kết nối
- Queue quá tải

---

# 12.5 Response Actions

Ví dụ:

Brute Force

↓

Khóa tài khoản tạm thời

↓

Ghi Audit

↓

Thông báo quản trị viên

---

Token Compromised

↓

Thu hồi Refresh Token

↓

Yêu cầu đăng nhập lại

↓

Ghi Audit

---

Webhook Attack

↓

Từ chối Request

↓

Ghi Log

↓

Tăng cảnh báo

---

# 12.6 Communication Plan

Thông báo cho:

- Quản trị viên
- Lãnh đạo (nếu cần)
- Đơn vị vận hành

Không công bố thông tin nhạy cảm trong quá trình xử lý.

---

# 12.7 Post Incident Review

Sau mỗi sự cố phải đánh giá:

- Nguyên nhân gốc
- Ảnh hưởng
- Thời gian xử lý
- Bài học kinh nghiệm
- Hành động phòng ngừa

---

# 12.8 Incident Documentation

Mỗi sự cố cần lưu:

- Incident ID
- Severity
- Timeline
- Root Cause
- Resolution
- Preventive Action

---

# 12.9 Expected Deliverables

✓ Incident Response Procedure

✓ Incident Classification

✓ Recovery Workflow

✓ Post Incident Review

# =============================================================================
# END OF PHASE 12
# =============================================================================



# =============================================================================
# PHASE 13 – DISASTER RECOVERY SECURITY
# =============================================================================

---

# 13.1 Overview

Disaster Recovery Security đảm bảo khả năng phục hồi của hệ thống sau các sự cố nghiêm trọng như mất dữ liệu, hỏng máy chủ hoặc gián đoạn dịch vụ.

---

# 13.2 Objectives

✓ Data Protection

✓ Service Continuity

✓ Backup Recovery

✓ Infrastructure Recovery

✓ Configuration Recovery

---

# 13.3 Disaster Scenarios

- Hỏng máy chủ
- Mất điện kéo dài
- SQL Server lỗi
- Mất dữ liệu
- Mã độc tống tiền (Ransomware)
- Lỗi cấu hình
- Sự cố mạng

---

# 13.4 Recovery Strategy

Application

↓

Infrastructure

↓

Database

↓

Configuration

↓

Services

↓

Verification

↓

Go Live

---

# 13.5 Backup Strategy

Thực hiện:

- Full Backup
- Differential Backup (nếu áp dụng)
- Transaction Log Backup (đối với SQL Server khi phù hợp)

Định kỳ kiểm tra khả năng khôi phục.

---

# 13.6 Recovery Validation

Sau khi khôi phục phải kiểm tra:

- Database Integrity
- API Health
- AI Services
- Notification Center
- Zalo OA Integration
- GIS Services

---

# 13.7 Recovery Documentation

Lưu:

- Backup Version
- Recovery Time
- Recovery Result
- Validation Report

---

# 13.8 Business Continuity Principles

- Ưu tiên khôi phục dịch vụ cốt lõi.
- Khôi phục theo mức độ ưu tiên nghiệp vụ.
- Đảm bảo dữ liệu nhất quán trước khi mở lại hệ thống.

---

# 13.9 Expected Deliverables

✓ Disaster Recovery Plan

✓ Backup Strategy

✓ Recovery Procedure

✓ Business Continuity Guidelines

# =============================================================================
# END OF PHASE 13
# =============================================================================
```
```md
# =============================================================================
# PHASE 14 – SECURITY TESTING
# =============================================================================

---

# 14.1 Overview

Security Testing nhằm xác minh toàn bộ kiến trúc bảo mật của hệ thống AnSinhSo hoạt động đúng theo thiết kế trước khi triển khai Production.

Mọi thành phần bảo mật phải được kiểm thử độc lập và kiểm thử tích hợp.

---

# 14.2 Security Testing Objectives

Mục tiêu:

✓ Xác thực đúng

✓ Phân quyền đúng

✓ Không rò rỉ dữ liệu

✓ API an toàn

✓ AI an toàn

✓ GIS an toàn

✓ Zalo OA an toàn

✓ Infrastructure an toàn

---

# 14.3 Testing Scope

Authentication

Authorization

JWT

Refresh Token

RBAC

Permissions

API Security

Database Security

AI Security

GIS Security

Zalo OA

Infrastructure

Audit

Monitoring

---

# 14.4 Authentication Testing

Kiểm thử:

✓ Đăng nhập thành công

✓ Sai mật khẩu

✓ Sai tài khoản

✓ Tài khoản bị khóa

✓ Token hết hạn

✓ Refresh Token

✓ Logout

✓ Session Revocation

---

# 14.5 Authorization Testing

Kiểm tra:

Administrator

Lãnh đạo

Cán bộ xã

Kế toán

Người dân

AI Service

Notification Worker

System Worker

Đảm bảo mỗi vai trò chỉ truy cập đúng chức năng và dữ liệu được phép.

---

# 14.6 API Security Testing

Kiểm thử:

Unauthorized

Forbidden

Rate Limit

JWT Validation

Input Validation

Output Validation

HTTPS

CORS

Security Headers

---

# 14.7 Database Security Testing

Kiểm tra:

Repository Only

SQL Injection

Permission

Backup Restore

Encryption

Audit

---

# 14.8 AI Security Testing

Kiểm thử:

Prompt Injection

Unauthorized Query

Citation

Permission Check

Context Isolation

Prompt Version

Audit

---

# 14.9 GIS Security Testing

Kiểm tra:

Layer Permission

Map Access

API Permission

Data Integrity

Audit

---

# 14.10 Zalo OA Security Testing

Kiểm thử:

Webhook Validation

Signature

Replay Protection

Retry

Provider

Audit

Delivery

---

# 14.11 Infrastructure Security Testing

Firewall

HTTPS

Reverse Proxy

Health Check

Backup

Monitoring

Logging

Configuration

---

# 14.12 Performance & Stress Security Testing

Kiểm thử:

Concurrent Login

Concurrent API

Notification Queue

AI Requests

Webhook Burst

Large Dataset

---

# 14.13 Penetration Testing

Thực hiện (nếu có điều kiện):

- Authentication Bypass
- Authorization Bypass
- Broken Access Control
- Injection
- XSS
- CSRF (nếu có giao diện web phù hợp)
- File Upload
- Sensitive Data Exposure

---

# 14.14 Security Test Deliverables

✓ Security Test Cases

✓ Test Reports

✓ Penetration Report

✓ Risk Assessment

✓ Remediation Report

---

# 14.15 Exit Criteria

Chỉ triển khai Production khi:

✓ Không còn lỗi bảo mật mức Critical

✓ Không còn lỗi bảo mật mức High chưa được chấp nhận rủi ro

✓ Toàn bộ kiểm thử bắt buộc đạt yêu cầu

---

# =============================================================================
# END OF PHASE 14
# =============================================================================



# =============================================================================
# PHASE 15 – PRODUCTION SECURITY CHECKLIST
# =============================================================================

---

# 15.1 Overview

Checklist này phải được xác nhận trước mỗi lần triển khai Production.

---

# 15.2 Authentication

□ JWT hoạt động

□ Refresh Token hoạt động

□ Password Hash đúng

□ Session Management

□ Role Mapping

□ Permission Mapping

---

# 15.3 API Security

□ HTTPS

□ Rate Limiting

□ Validation

□ Error Handling

□ Versioning

□ Security Headers

---

# 15.4 Database

□ Backup

□ Restore Test

□ Encryption

□ Migration

□ Least Privilege

---

# 15.5 AI

□ Provider

□ Prompt Version

□ Citation

□ Permission

□ Audit

---

# 15.6 GIS

□ Layer Permission

□ API

□ Audit

□ Data Integrity

---

# 15.7 Zalo OA

□ Access Token

□ Secret

□ Webhook

□ Retry

□ Delivery

□ Monitoring

---

# 15.8 Infrastructure

□ Firewall

□ Reverse Proxy

□ IIS

□ Health Check

□ SSL

□ Monitoring

□ Logging

---

# 15.9 Monitoring

□ Dashboard

□ Alerts

□ Metrics

□ Health Check

□ Queue Status

---

# 15.10 Audit

□ Audit Enabled

□ Log Rotation

□ Correlation ID

□ Search

□ Export

---

# 15.11 Backup & Recovery

□ Full Backup

□ Restore Tested

□ Disaster Recovery

□ Recovery Documentation

---

# 15.12 Documentation

Đã cập nhật:

□ API_SPEC.md

□ DATABASE_DESIGN.md

□ AI_DEVELOPMENT_GUIDE.md

□ ZALO_OA_INTEGRATION_GUIDE.md

□ SECURITY_ARCHITECTURE.md

□ DEVELOPMENT_SPRINTS.md

---

# 15.13 Deployment Approval

Trước khi Production cần có xác nhận:

□ Technical Lead

□ Project Owner

□ System Administrator

---

# 15.14 Final Acceptance Criteria

Hệ thống chỉ được phép triển khai Production khi:

✓ Security Review hoàn tất

✓ Security Testing đạt

✓ Backup xác nhận

✓ Monitoring hoạt động

✓ Audit hoạt động

✓ Rollback Plan sẵn sàng

✓ Tài liệu cập nhật

---

# =============================================================================
# SECURITY ARCHITECTURE BASELINE v1.0
# =============================================================================

Version

1.0

Status

Production Ready

Architecture

Enterprise Security Reference

Target Framework

ASP.NET Core 8

Database

SQL Server

Authentication

JWT + Refresh Token

Authorization

RBAC + Policy-Based Authorization

AI

Secure AI Integration Hub

GIS

Secure GIS Platform

Citizen Communication

Secure Messaging Gateway + Zalo OA

Deployment

Production

# =============================================================================
# END OF DOCUMENT
# =============================================================================
```
