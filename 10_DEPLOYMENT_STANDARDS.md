# 10_DEPLOYMENT_STANDARDS.md

## Phase 1 – Deployment Foundation

```
============================================================================
10_DEPLOYMENT_STANDARDS.md
============================================================================

Project         : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
Document Type   : Enterprise Deployment Standards
Version         : 1.0.0
Status          : FROZEN
Owner           : Project Architecture Team

Architecture    : Enterprise Clean Architecture
Framework        : ASP.NET Core 8
Frontend         : HTML / CSS / JavaScript
Database         : SQL Server 2022
Server           : Windows Server 2022
Web Server       : IIS
CI/CD            : GitHub Actions
Source Control   : Git

Deployment Type  : Enterprise Production
Last Updated     : 2026-07-12

============================================================================
```

---

# 1. PURPOSE

Tài liệu này quy định toàn bộ tiêu chuẩn triển khai (Deployment Standards) của hệ thống **AnSinhSo**.

Mục tiêu:

* Chuẩn hóa quy trình triển khai.
* Đảm bảo khả năng mở rộng.
* Giảm rủi ro khi phát hành.
* Hỗ trợ CI/CD.
* Hỗ trợ AI Coding.
* Đảm bảo hệ thống hoạt động ổn định trên Production.

---

# 2. SCOPE

Áp dụng cho toàn bộ hệ thống:

* Backend
* Frontend
* SQL Server
* IIS
* Windows Server
* Docker (Future)
* GitHub
* GitHub Actions
* AI Services
* GIS Services
* Zalo Official Account
* File Storage
* Monitoring
* Backup

---

# 3. DEPLOYMENT OBJECTIVES

Hệ thống phải đảm bảo:

* Reliability
* Availability
* Scalability
* Maintainability
* Recoverability
* Security

---

## 3.1 Reliability

Quá trình triển khai phải ổn định và có khả năng lặp lại.

Không phụ thuộc thao tác thủ công.

---

## 3.2 Availability

Thời gian gián đoạn phải được giảm xuống mức thấp nhất.

---

## 3.3 Scalability

Có thể mở rộng:

* Người dùng
* Dữ liệu
* Server
* Dịch vụ

---

## 3.4 Maintainability

Dễ:

* cập nhật
* nâng cấp
* sửa lỗi
* rollback

---

## 3.5 Recoverability

Có khả năng:

* Backup
* Restore
* Disaster Recovery

---

## 3.6 Security

Deployment phải tuân thủ toàn bộ tài liệu:

* 09_SECURITY_STANDARDS.md

---

# 4. DEPLOYMENT PRINCIPLES

## Principle 01

Automation First

Ưu tiên tự động hóa.

---

## Principle 02

Infrastructure as Code Ready

Thiết kế sẵn sàng áp dụng IaC trong tương lai.

---

## Principle 03

Immutable Deployment

Không chỉnh sửa trực tiếp trên Production.

Mọi thay đổi phải đến từ Source Code.

---

## Principle 04

Configuration Separation

Tách biệt:

* Source Code
* Configuration
* Secret
* Environment

---

## Principle 05

Rollback Ready

Mọi Release đều phải có khả năng Rollback.

---

## Principle 06

Repeatable Deployment

Có thể triển khai nhiều lần với cùng kết quả.

---

## Principle 07

Security by Default

Mọi môi trường Production phải mặc định an toàn.

---

# 5. DEPLOYMENT ARCHITECTURE

```
Developer

↓

Git Repository

↓

Continuous Integration

↓

Build Artifact

↓

Release Pipeline

↓

Testing

↓

Staging

↓

Production
```

---

# 6. ENTERPRISE DEPLOYMENT ARCHITECTURE

```
Developer

        │

        ▼

Visual Studio 2022

        │

        ▼

GitHub Repository

        │

        ▼

GitHub Actions

        │

        ▼

Publish Artifact

        │

        ▼

Windows Server

        │

        ▼

IIS

        │

 ┌──────┼───────────┐

 ▼      ▼           ▼

SQL     AI         GIS

Server Service    Service

        │

        ▼

Zalo Official Account

        │

        ▼

Citizen Portal

Admin Portal

Leadership Portal
```

---

# 7. DEPLOYMENT LIFECYCLE

Một phiên bản phần mềm phải trải qua:

```
Development

↓

Code Review

↓

Build

↓

Unit Test

↓

Integration Test

↓

Publish

↓

Staging

↓

User Acceptance Test

↓

Production

↓

Monitoring

↓

Maintenance
```

---

# 8. DEPLOYMENT ENVIRONMENTS

Hệ thống chuẩn gồm:

| Environment | Mục đích            |
| ----------- | ------------------- |
| Local       | Lập trình           |
| Development | Phát triển          |
| Testing     | Kiểm thử            |
| UAT         | Nghiệm thu          |
| Staging     | Mô phỏng Production |
| Production  | Vận hành chính thức |

---

# 9. DEPLOYMENT RESPONSIBILITIES

| Vai trò             | Trách nhiệm                           |
| ------------------- | ------------------------------------- |
| Solution Architect  | Kiến trúc triển khai                  |
| Backend Developer   | Publish API                           |
| Frontend Developer  | Publish Frontend                      |
| Database Developer  | Migration Database                    |
| DevOps Engineer     | CI/CD                                 |
| QA                  | Deployment Testing                    |
| Project Manager     | Release Approval                      |
| AI Coding Assistant | Sinh mã tuân thủ Deployment Standards |

---

# 10. DEPLOYMENT BASELINE

Toàn bộ hệ thống phải đáp ứng tối thiểu:

* HTTPS
* Version Control
* Git Repository
* CI Ready
* Rollback Ready
* Backup
* Monitoring
* Logging
* Audit
* Environment Separation
* Secret Management
* Security Compliance

---

# End of Phase 1

Phase tiếp theo:

* Development Environment
* Local Development
* Development Server
* Testing Environment
* UAT
* Staging
* Production
* Environment Variables
* Configuration Management
* Secret Management
# ============================================================================

# 11. ENVIRONMENT STANDARDS

# ============================================================================

## 11.1 Purpose

Mọi môi trường triển khai phải được chuẩn hóa nhằm:

* Đảm bảo tính nhất quán.
* Giảm sai sót khi triển khai.
* Dễ dàng kiểm thử.
* Hỗ trợ CI/CD.
* Dễ mở rộng trong tương lai.

---

## 11.2 Standard Environments

Hệ thống AnSinhSo sử dụng các môi trường sau:

| Environment | Mục đích                    |
| ----------- | --------------------------- |
| Local       | Phát triển trên máy cá nhân |
| Development | Môi trường phát triển chung |
| Testing     | Kiểm thử chức năng          |
| UAT         | Nghiệm thu người dùng       |
| Staging     | Mô phỏng Production         |
| Production  | Vận hành chính thức         |

---

## 11.3 Environment Isolation

Mỗi môi trường phải được tách biệt:

* Database
* Configuration
* Secret
* Log
* File Storage
* Domain
* SSL Certificate

Không được sử dụng chung dữ liệu Production cho Development hoặc Testing.

---

# ============================================================================

# 12. LOCAL DEVELOPMENT ENVIRONMENT

# ============================================================================

## 12.1 Purpose

Môi trường Local dành cho lập trình viên phát triển và kiểm thử ban đầu.

---

## 12.2 Required Software

Chuẩn sử dụng:

* Windows 11
* Visual Studio 2022
* Visual Studio Code
* .NET SDK 8
* SQL Server 2022 Developer
* SQL Server Management Studio (SSMS)
* Git
* Postman
* Node.js LTS (nếu sử dụng công cụ Frontend)
* Docker Desktop (khuyến nghị)

---

## 12.3 Local Database

Mỗi lập trình viên sử dụng Database riêng.

Không chia sẻ Database Local.

---

## 12.4 Local Configuration

Sử dụng:

* `appsettings.Development.json`
* .NET User Secrets
* Environment Variables

Không lưu Secret vào Repository.

---

## 12.5 Local Testing

Trước khi Commit:

* Build thành công.
* Không còn lỗi biên dịch.
* Unit Test (nếu có) đạt yêu cầu.
* Không còn Warning nghiêm trọng.

---

# ============================================================================

# 13. DEVELOPMENT ENVIRONMENT

# ============================================================================

## 13.1 Purpose

Development là môi trường tích hợp mã nguồn của nhóm phát triển.

---

## 13.2 Characteristics

* Có dữ liệu mẫu.
* Có Logging.
* Có Authentication.
* Có Monitoring cơ bản.

---

## 13.3 Deployment Rules

Chỉ triển khai từ:

* Nhánh `develop`
* Hoặc nhánh được chỉ định theo quy trình phát triển.

---

## 13.4 Testing Scope

Kiểm tra:

* API
* Frontend
* Database
* AI Integration
* GIS Integration
* Zalo OA Sandbox (nếu có)

---

# ============================================================================

# 14. TESTING ENVIRONMENT

# ============================================================================

## 14.1 Purpose

Dành cho QA kiểm thử chức năng.

---

## 14.2 Test Data

Chỉ sử dụng:

* Dữ liệu giả lập.
* Dữ liệu đã ẩn danh.
* Không sử dụng dữ liệu Production.

---

## 14.3 Validation

Kiểm thử:

* Functional Testing
* Integration Testing
* Regression Testing
* Security Testing cơ bản

---

## 14.4 Stability

Không triển khai tính năng chưa hoàn thiện lên môi trường Testing.

---

# ============================================================================

# 15. USER ACCEPTANCE TEST (UAT)

# ============================================================================

## 15.1 Purpose

Dành cho khách hàng hoặc đơn vị nghiệp vụ nghiệm thu.

---

## 15.2 UAT Data

Ưu tiên sử dụng:

* Dữ liệu mô phỏng sát thực tế.
* Không sử dụng dữ liệu cá nhân thật nếu chưa được phép.

---

## 15.3 Acceptance Criteria

Mọi chức năng phải đáp ứng:

* Business Rules
* UI/UX
* Performance
* Security cơ bản

---

# ============================================================================

# 16. STAGING ENVIRONMENT

# ============================================================================

## 16.1 Purpose

Staging là môi trường mô phỏng Production.

---

## 16.2 Principles

Staging phải gần giống Production về:

* Cấu hình
* Database Schema
* IIS
* Runtime
* Phiên bản .NET
* SSL

---

## 16.3 Deployment

Release Candidate phải được triển khai lên Staging trước Production.

---

## 16.4 Final Validation

Kiểm tra:

* API
* Frontend
* Database
* AI
* GIS
* Zalo OA
* Backup
* Monitoring

---

# ============================================================================

# 17. PRODUCTION ENVIRONMENT

# ============================================================================

## 17.1 Purpose

Production là môi trường phục vụ người dùng cuối.

---

## 17.2 Production Principles

Production phải:

* Ổn định.
* An toàn.
* Có Backup.
* Có Monitoring.
* Có Logging.
* Có Audit.

---

## 17.3 Deployment Rules

Chỉ triển khai:

* Phiên bản đã được phê duyệt.
* Đã kiểm thử trên Staging.
* Đã có kế hoạch Rollback.

---

## 17.4 Production Access

Quyền truy cập phải được giới hạn.

Không cấp quyền quản trị cho người không có trách nhiệm.

---

# ============================================================================

# 18. ENVIRONMENT VARIABLES

# ============================================================================

## 18.1 Purpose

Thông tin cấu hình phải được tách khỏi mã nguồn.

---

## 18.2 Examples

Ví dụ:

* Database Connection String
* JWT Secret
* SMTP Configuration
* AI API Key
* Zalo OA Secret
* Storage Path

---

## 18.3 Rules

Không Hard-code:

* Password
* Secret
* API Key
* Connection String

---

## 18.4 Naming Convention

Sử dụng tên rõ ràng, nhất quán và theo chuẩn của nền tảng triển khai.

---

# ============================================================================

# 19. CONFIGURATION MANAGEMENT

# ============================================================================

## 19.1 Configuration Hierarchy

Thứ tự ưu tiên:

1. Environment Variables
2. Secret Manager
3. appsettings.{Environment}.json
4. appsettings.json

---

## 19.2 Environment Files

Chuẩn:

* appsettings.json
* appsettings.Development.json
* appsettings.Testing.json
* appsettings.Staging.json
* appsettings.Production.json

---

## 19.3 Configuration Reload

Các cấu hình hỗ trợ thay đổi động phải được thiết kế để có thể tải lại mà không cần sửa mã nguồn.

---

# ============================================================================

# 20. SECRET MANAGEMENT

# ============================================================================

## 20.1 Managed Secrets

Bao gồm:

* JWT Secret
* Database Password
* SMTP Password
* AI API Key
* Zalo OA Secret
* Storage Credential

---

## 20.2 Secret Storage

Ưu tiên:

* Environment Variables
* .NET User Secrets (Development)
* Azure Key Vault
* AWS Secrets Manager
* HashiCorp Vault

---

## 20.3 Secret Rotation

Secret phải có khả năng:

* Thay đổi định kỳ.
* Thu hồi.
* Ghi nhận lịch sử thay đổi theo quy trình quản trị.

---

## 20.4 Secret Access

Chỉ các dịch vụ hoặc người dùng được ủy quyền mới được phép truy cập Secret.

---

# ============================================================================

# 21. ENVIRONMENT CHECKLIST

# ============================================================================

Trước khi chuyển sang Phase tiếp theo cần xác nhận:

* Mỗi Environment được tách biệt.
* Database độc lập giữa các môi trường.
* Không sử dụng dữ liệu Production trong Testing.
* Configuration được quản lý tập trung.
* Secret không nằm trong Source Code.
* Environment Variables hoạt động đúng.
* Staging tương đồng Production.
* Production có Backup.
* Production có Monitoring.
* Production có Logging.
* Production có Audit.
* Có kế hoạch Rollback.

---

# End of Phase 2A

Phase tiếp theo:

* Windows Server Standards
* IIS Standards
* SQL Server Deployment
* Docker Standards
* Docker Compose
* Reverse Proxy
* HTTPS
* SSL Certificates
* Domain Management
* Firewall
* Network Security
# ============================================================================

# 22. WINDOWS SERVER STANDARDS

# ============================================================================

## 22.1 Purpose

Windows Server là nền tảng triển khai chính của hệ thống AnSinhSo.

Máy chủ phải được cấu hình theo chuẩn Enterprise nhằm đảm bảo:

* Bảo mật
* Hiệu năng
* Khả năng mở rộng
* Tính ổn định

---

## 22.2 Supported Versions

Khuyến nghị:

* Windows Server 2022 Standard
* Windows Server 2025 (Future Ready)

Không triển khai trên các phiên bản đã hết hỗ trợ.

---

## 22.3 Server Configuration

Máy chủ cần cấu hình:

* Time Zone chính xác
* Đồng bộ thời gian (NTP)
* Windows Update
* Windows Defender
* Firewall
* Event Log

---

## 22.4 Server Roles

Chỉ cài đặt các Role cần thiết.

Không cài đặt dịch vụ không sử dụng.

---

## 22.5 Administrator Account

Không sử dụng tài khoản Administrator mặc định để vận hành ứng dụng.

Khuyến nghị:

* Đổi tên Administrator
* Mật khẩu mạnh
* MFA (nếu có)
* Nhật ký đăng nhập

---

# ============================================================================

# 23. IIS STANDARDS

# ============================================================================

## 23.1 Purpose

IIS là Web Server chuẩn cho Backend ASP.NET Core.

---

## 23.2 IIS Version

Khuyến nghị:

* IIS 10+

---

## 23.3 Application Pool

Mỗi hệ thống sử dụng Application Pool riêng.

Không dùng chung với ứng dụng khác.

---

## 23.4 Identity

Application Pool Identity:

* Least Privilege
* Không dùng Administrator

---

## 23.5 HTTPS Binding

Production chỉ sử dụng:

* HTTPS
* TLS

Không Publish qua HTTP.

---

## 23.6 Logging

Bật:

* IIS Logs
* Failed Request Tracing (khi cần)
* Windows Event Log

---

# ============================================================================

# 24. SQL SERVER DEPLOYMENT

# ============================================================================

## 24.1 Supported Version

Chuẩn:

* SQL Server 2022

Có thể mở rộng:

* Azure SQL
* SQL Server Always On

---

## 24.2 Database Deployment

Triển khai bằng:

* EF Core Migration
* SQL Scripts
* DACPAC (nếu áp dụng)

---

## 24.3 Database Accounts

Tách riêng:

* DBA
* Application Account
* Read Only Account
* Backup Account

---

## 24.4 Connection Security

Bắt buộc:

* Authentication
* Encryption
* Least Privilege

---

## 24.5 Database Maintenance

Thực hiện định kỳ:

* Backup
* Index Maintenance
* Statistics Update
* Integrity Check

---

# ============================================================================

# 25. DOCKER STANDARDS

# ============================================================================

## 25.1 Purpose

Docker chưa phải nền tảng triển khai chính nhưng hệ thống phải sẵn sàng hỗ trợ.

---

## 25.2 Container Principles

Mỗi Container chỉ thực hiện một nhiệm vụ chính.

Ví dụ:

* API
* Database (Development)
* Redis
* AI Gateway

---

## 25.3 Image Rules

Docker Image phải:

* Phiên bản rõ ràng
* Không dùng latest trên Production
* Có khả năng tái tạo

---

## 25.4 Security

Không lưu:

* Secret
* Password
* API Key

bên trong Docker Image.

---

# ============================================================================

# 26. DOCKER COMPOSE STANDARDS

# ============================================================================

## 26.1 Purpose

Docker Compose được sử dụng cho:

* Local Development
* Integration Testing
* Demo Environment

---

## 26.2 Services

Ví dụ:

* API
* SQL Server
* Redis
* AI Service

---

## 26.3 Environment Variables

Secret phải truyền thông qua:

* Environment Variables
* Secret Files

---

# ============================================================================

# 27. REVERSE PROXY STANDARDS

# ============================================================================

## 27.1 Purpose

Reverse Proxy giúp:

* SSL Termination
* Load Balancing
* Security
* Routing

---

## 27.2 Supported Reverse Proxy

Có thể sử dụng:

* IIS
* Nginx
* HAProxy
* YARP

---

## 27.3 Forwarded Headers

Cấu hình Forwarded Headers đúng để ứng dụng nhận biết:

* Client IP
* HTTPS
* Host

---

# ============================================================================

# 28. HTTPS & TLS STANDARDS

# ============================================================================

## 28.1 HTTPS Only

Production bắt buộc:

HTTPS.

---

## 28.2 TLS Version

Khuyến nghị:

* TLS 1.3
* TLS 1.2 (tương thích)

Không sử dụng:

* SSL
* TLS 1.0
* TLS 1.1

---

## 28.3 HTTP Redirect

Toàn bộ HTTP phải chuyển hướng sang HTTPS.

---

## 28.4 HSTS

Khuyến nghị bật:

HTTP Strict Transport Security (HSTS)

cho Production.

---

# ============================================================================

# 29. SSL CERTIFICATE STANDARDS

# ============================================================================

## 29.1 Certificate

Sử dụng chứng chỉ từ nhà cung cấp đáng tin cậy.

Không sử dụng Self-Signed Certificate trên Production.

---

## 29.2 Certificate Renewal

Theo dõi:

* Ngày hết hạn
* Gia hạn trước khi hết hạn

---

## 29.3 Private Key

Private Key phải:

* Được bảo vệ.
* Không chia sẻ.
* Không đưa vào Repository.

---

# ============================================================================

# 30. DOMAIN MANAGEMENT

# ============================================================================

## 30.1 Domain Naming

Ví dụ:

```text
api.ansinhso.gov.vn

admin.ansinhso.gov.vn

citizen.ansinhso.gov.vn
```

Tên miền thực tế do đơn vị triển khai quyết định.

---

## 30.2 DNS

Quản lý:

* A Record
* CNAME
* TXT
* MX (nếu có)

---

## 30.3 Subdomain Isolation

Các dịch vụ nên được tách bằng Subdomain phù hợp.

---

# ============================================================================

# 31. FIREWALL STANDARDS

# ============================================================================

## 31.1 Default Policy

Mặc định:

Deny All.

Chỉ mở các cổng cần thiết.

---

## 31.2 Common Ports

Ví dụ:

* 80 (Redirect)
* 443 (HTTPS)
* 1433 (SQL - chỉ mạng nội bộ nếu cần)
* 22 hoặc 3389 (quản trị, giới hạn truy cập)

---

## 31.3 Remote Access

Remote Desktop hoặc SSH phải:

* Giới hạn IP.
* Có xác thực mạnh.
* Ghi nhật ký.

---

# ============================================================================

# 32. NETWORK SECURITY

# ============================================================================

## 32.1 Network Segmentation

Khuyến nghị tách:

* Web Layer
* Application Layer
* Database Layer

---

## 32.2 Internal Communication

Các dịch vụ nội bộ nên sử dụng mạng riêng hoặc kênh truyền bảo mật.

---

## 32.3 DDoS Readiness

Khuyến nghị sử dụng:

* Reverse Proxy
* WAF
* CDN (nếu triển khai Internet)

---

## 32.4 Secure Ports

Không mở các cổng không sử dụng.

Định kỳ rà soát dịch vụ đang lắng nghe trên máy chủ.

---

# ============================================================================

# 33. INFRASTRUCTURE CHECKLIST

# ============================================================================

Trước khi triển khai cần xác nhận:

* Windows Server được cập nhật.
* IIS cấu hình đúng.
* SQL Server bảo mật.
* HTTPS hoạt động.
* TLS đúng phiên bản.
* SSL hợp lệ.
* Firewall cấu hình đúng.
* Reverse Proxy hoạt động.
* Docker (nếu sử dụng) an toàn.
* Không có Secret trong Image.
* DNS chính xác.
* Chỉ mở các cổng cần thiết.
* Đã kiểm tra Network Security.

---

# End of Phase 2B

Phase tiếp theo:

* Build Standards
* Publish Standards
* Release Process
* Database Migration
* Backup Strategy
* Restore Strategy
* Rollback Strategy
* Versioning
* Release Checklist
* Production Deployment Workflow
# ============================================================================

# 34. BUILD STANDARDS

# ============================================================================

## 34.1 Purpose

Quy trình Build phải đảm bảo:

* Có khả năng lặp lại.
* Có thể tự động hóa.
* Sinh ra Artifact nhất quán.
* Không phụ thuộc máy của lập trình viên.

---

## 34.2 Build Configuration

Chuẩn:

* Debug (Development)
* Release (Production)

Không triển khai Production bằng Debug Build.

---

## 34.3 Build Requirements

Trước khi Build:

* Restore Packages thành công.
* Compile không lỗi.
* Không còn Critical Warning.
* Unit Test đạt yêu cầu.
* Version được cập nhật.

---

## 34.4 Build Output

Artifact phải bao gồm:

* API Publish
* Frontend
* Configuration Template
* Database Migration Scripts (nếu có)
* Release Notes

---

# ============================================================================

# 35. PUBLISH STANDARDS

# ============================================================================

## 35.1 Publish Strategy

Triển khai bằng:

* dotnet publish
* CI/CD Pipeline
* Release Artifact

Không sao chép thủ công từng tệp lên Production.

---

## 35.2 Publish Modes

Hỗ trợ:

* Framework-Dependent Deployment (FDD)
* Self-Contained Deployment (SCD)

Khuyến nghị FDD cho môi trường Windows Server có sẵn .NET Runtime.

---

## 35.3 Publish Folder Structure

Ví dụ:

```text
publish/

├── api/

├── wwwroot/

├── appsettings.json

├── web.config

└── logs/
```

---

## 35.4 Verification

Sau Publish cần kiểm tra:

* Build thành công.
* Đủ tệp.
* Không thiếu DLL.
* Không thiếu Static Files.

---

# ============================================================================

# 36. RELEASE MANAGEMENT

# ============================================================================

## 36.1 Release Principles

Mỗi Release phải:

* Có Version.
* Có Change Log.
* Có Release Note.
* Có Rollback Plan.

---

## 36.2 Release Types

Bao gồm:

* Major
* Minor
* Patch
* Hotfix

---

## 36.3 Release Approval

Release Production phải được phê duyệt theo quy trình quản lý dự án.

---

## 36.4 Release Documentation

Mỗi phiên bản cần lưu:

* Build Number
* Version
* Deployment Date
* Deployed By
* Related Commit

---

# ============================================================================

# 37. DATABASE MIGRATION STANDARDS

# ============================================================================

## 37.1 Migration Strategy

Database chỉ được thay đổi thông qua:

* EF Core Migration
* SQL Migration Script
* Deployment Script đã được kiểm thử

---

## 37.2 Migration Rules

Không chỉnh sửa trực tiếp Database Production.

Mọi thay đổi phải được lưu trong Source Control.

---

## 37.3 Migration Order

Thứ tự:

1. Backup Database
2. Chạy Migration
3. Kiểm tra Schema
4. Kiểm tra dữ liệu
5. Smoke Test

---

## 37.4 Rollback

Mọi Migration phải có phương án khôi phục.

---

# ============================================================================

# 38. BACKUP STRATEGY

# ============================================================================

## 38.1 Backup Scope

Sao lưu:

* Database
* Uploaded Files
* Configuration
* Logs (theo chính sách)
* Deployment Packages

---

## 38.2 Backup Frequency

Khuyến nghị:

* Daily Backup
* Weekly Full Backup
* Monthly Archive

Tần suất thực tế do đơn vị vận hành quy định.

---

## 38.3 Backup Validation

Backup phải được kiểm tra khả năng khôi phục định kỳ.

---

# ============================================================================

# 39. RESTORE STRATEGY

# ============================================================================

## 39.1 Restore Process

Quy trình:

```text
Backup Selection

↓

Integrity Verification

↓

Restore Database

↓

Restore Files

↓

System Validation

↓

Go Live
```

---

## 39.2 Restore Testing

Thực hiện kiểm tra khôi phục theo định kỳ để bảo đảm tính khả dụng của bản sao lưu.

---

# ============================================================================

# 40. ROLLBACK STRATEGY

# ============================================================================

## 40.1 Rollback Conditions

Thực hiện Rollback khi:

* Deployment thất bại.
* Lỗi nghiêm trọng.
* Mất dữ liệu.
* Hiệu năng không đạt yêu cầu.

---

## 40.2 Rollback Scope

Rollback gồm:

* Application
* Database (nếu cần)
* Configuration

---

## 40.3 Rollback Principles

Rollback phải:

* Nhanh.
* An toàn.
* Có kiểm chứng.

---

# ============================================================================

# 41. VERSIONING STANDARDS

# ============================================================================

## 41.1 Version Format

Chuẩn:

```text
Major.Minor.Patch
```

Ví dụ:

```text
1.0.0

1.1.0

1.1.2
```

---

## 41.2 Version Update Rules

* Major: Thay đổi lớn.
* Minor: Thêm tính năng.
* Patch: Sửa lỗi.

---

## 41.3 Version Tag

Mỗi Release phải được gắn Tag trong Git.

---

# ============================================================================

# 42. PRODUCTION DEPLOYMENT WORKFLOW

# ============================================================================

## 42.1 Deployment Flow

```text
Developer

↓

Build

↓

Unit Test

↓

Code Review

↓

CI Build

↓

Integration Test

↓

Staging

↓

UAT

↓

Release Approval

↓

Production Deployment

↓

Smoke Test

↓

Monitoring
```

---

## 42.2 Smoke Test

Sau Deployment cần kiểm tra:

* Login
* Dashboard
* API
* Database
* AI Service
* GIS Service
* Zalo OA
* Logging

---

# ============================================================================

# 43. RELEASE CHECKLIST

# ============================================================================

Trước khi Release cần xác nhận:

* Build thành công.
* Unit Test đạt.
* Integration Test đạt.
* UAT hoàn thành.
* Database Migration sẵn sàng.
* Backup hoàn thành.
* Rollback Plan sẵn sàng.
* Version đã cập nhật.
* Change Log đã cập nhật.
* Release Notes hoàn thành.
* Artifact đã xác minh.
* Smoke Test được chuẩn bị.

---

# End of Phase 2C

Phase tiếp theo:

* Git Workflow Standards
* Branch Strategy
* Pull Request Standards
* GitHub Actions
* Continuous Integration
* Continuous Deployment
* Automated Testing
* Deployment Automation
* Release Automation
* DevOps Best Practices
# ============================================================================

# 44. GIT WORKFLOW STANDARDS

# ============================================================================

## 44.1 Purpose

Git Workflow phải đảm bảo:

* Dễ quản lý mã nguồn.
* Dễ cộng tác.
* Hạn chế xung đột.
* Hỗ trợ CI/CD.
* Hỗ trợ Rollback.

---

## 44.2 Source of Truth

Git Repository là nguồn duy nhất của mã nguồn.

Không chỉnh sửa trực tiếp trên Production Server.

---

## 44.3 Commit Principles

Mỗi Commit phải:

* Chỉ giải quyết một mục đích.
* Có thông điệp rõ ràng.
* Có thể truy vết.

---

## 44.4 Commit Message Convention

Khuyến nghị:

```text
feat: thêm quản lý hộ gia đình

fix: sửa lỗi đăng nhập

docs: cập nhật tài liệu

refactor: tối ưu service

test: bổ sung unit test

chore: cập nhật package
```

---

# ============================================================================

# 45. BRANCH STRATEGY

# ============================================================================

## 45.1 Standard Branches

```text
main

develop

feature/*

release/*

hotfix/*
```

---

## 45.2 Main Branch

Chỉ chứa:

* Production Code
* Stable Release

Không phát triển trực tiếp trên `main`.

---

## 45.3 Develop Branch

Là nhánh tích hợp chính cho quá trình phát triển.

---

## 45.4 Feature Branch

Mỗi tính năng:

* Một Branch riêng.
* Merge sau khi Review.

Ví dụ:

```text
feature/authentication

feature/dashboard

feature/zalo-oa

feature/gis-map
```

---

## 45.5 Release Branch

Sử dụng khi chuẩn bị phát hành.

Chỉ sửa:

* Bug
* Configuration
* Documentation

---

## 45.6 Hotfix Branch

Áp dụng cho lỗi Production.

Sau khi hoàn thành phải Merge về:

* main
* develop

---

# ============================================================================

# 46. PULL REQUEST STANDARDS

# ============================================================================

## 46.1 Pull Request Required

Không Merge trực tiếp vào:

* main
* develop

Mọi thay đổi phải thông qua Pull Request.

---

## 46.2 PR Checklist

Pull Request phải bao gồm:

* Mô tả thay đổi.
* Liên kết Issue (nếu có).
* Kết quả kiểm thử.
* Đánh giá ảnh hưởng.
* Rollback Notes (nếu cần).

---

## 46.3 Reviewer

Ít nhất một Reviewer phải phê duyệt trước khi Merge.

Đối với thay đổi lớn, khuyến nghị có từ hai Reviewer trở lên.

---

# ============================================================================

# 47. CONTINUOUS INTEGRATION (CI)

# ============================================================================

## 47.1 Purpose

CI giúp:

* Build tự động.
* Kiểm thử tự động.
* Phát hiện lỗi sớm.

---

## 47.2 CI Pipeline

```text
Git Push

↓

Restore Packages

↓

Build

↓

Unit Test

↓

Static Analysis

↓

Artifact

↓

Store Artifact
```

---

## 47.3 CI Validation

Pipeline chỉ thành công khi:

* Build thành công.
* Test đạt.
* Không có lỗi nghiêm trọng.
* Artifact được tạo thành công.

---

# ============================================================================

# 48. GITHUB ACTIONS STANDARDS

# ============================================================================

## 48.1 Purpose

GitHub Actions là nền tảng CI mặc định của dự án.

---

## 48.2 Workflow

Chuẩn gồm:

* Restore
* Build
* Test
* Publish
* Upload Artifact

---

## 48.3 Secrets

Secrets sử dụng:

* GitHub Secrets
* Environment Secrets

Không Hard-code trong Workflow.

---

## 48.4 Artifact

Artifact phải:

* Có Version.
* Có Build Number.
* Có thời gian tạo.

---

# ============================================================================

# 49. CONTINUOUS DEPLOYMENT (CD)

# ============================================================================

## 49.1 Deployment Flow

```text
Artifact

↓

Staging

↓

Smoke Test

↓

Approval

↓

Production
```

---

## 49.2 Approval Gate

Production Deployment phải có bước phê duyệt.

Không triển khai tự động lên Production nếu chưa được phê duyệt theo quy trình.

---

## 49.3 Deployment Verification

Sau Deployment:

* Health Check.
* Smoke Test.
* Monitoring.
* Logging.

---

# ============================================================================

# 50. AUTOMATED TESTING

# ============================================================================

## 50.1 Automated Tests

Pipeline nên chạy:

* Unit Test
* Integration Test
* API Test

Có thể mở rộng:

* UI Test
* Performance Test
* Security Scan

---

## 50.2 Test Result

Nếu Test thất bại:

Pipeline phải dừng.

Không tiếp tục triển khai.

---

# ============================================================================

# 51. DEPLOYMENT AUTOMATION

# ============================================================================

## 51.1 Principles

Deployment phải:

* Tự động.
* Có thể lặp lại.
* Có Logging.
* Có Rollback.

---

## 51.2 Manual Steps

Hạn chế tối đa thao tác thủ công.

Nếu bắt buộc:

Phải có hướng dẫn rõ ràng.

---

## 51.3 Verification

Sau mỗi bước:

* Kiểm tra trạng thái.
* Ghi Log.
* Báo lỗi khi thất bại.

---

# ============================================================================

# 52. RELEASE AUTOMATION

# ============================================================================

## 52.1 Automatic Versioning

Khuyến nghị:

* Build Number tự động.
* Version Tag.
* Release Notes tự động sinh.

---

## 52.2 Release Artifact

Artifact phải:

* Đầy đủ.
* Không bị thay đổi sau khi tạo.
* Có Checksum (nếu áp dụng).

---

# ============================================================================

# 53. DEVOPS BEST PRACTICES

# ============================================================================

## 53.1 Infrastructure

Infrastructure phải:

* Có Version.
* Có Backup.
* Có Monitoring.

---

## 53.2 Automation First

Ưu tiên:

* Build Automation.
* Test Automation.
* Deployment Automation.

---

## 53.3 Observability

Hệ thống phải hỗ trợ:

* Logging.
* Monitoring.
* Alerting.
* Tracing.

---

## 53.4 Security

Pipeline phải:

* Quản lý Secret an toàn.
* Không công khai Token.
* Không ghi Password vào Log.

---

# ============================================================================

# 54. DEVOPS CHECKLIST

# ============================================================================

Trước khi triển khai cần xác nhận:

* Git Workflow đúng.
* Branch Strategy đúng.
* Pull Request được Review.
* CI thành công.
* CD sẵn sàng.
* GitHub Actions hoạt động.
* Automated Test đạt.
* Artifact hợp lệ.
* Deployment Automation hoạt động.
* Release Automation hoạt động.
* Monitoring hoạt động.
* Logging hoạt động.
* Rollback sẵn sàng.

---

# End of Phase 2D

Phase tiếp theo:

* Monitoring Standards
* Logging Operations
* Alerting Standards
* High Availability
* Disaster Recovery Operations
* Patch Management
* Maintenance Windows
* AI Service Deployment
* GIS Service Deployment
* Zalo OA Deployment
* Production Operations
* Deployment Definition of Done
* AI Coding Rules
* Final Deployment Checklist
* Change Log
* Related Documents
* End of Document
# ============================================================================

# 55. MONITORING STANDARDS

# ============================================================================

## 55.1 Purpose

Monitoring giúp theo dõi tình trạng hoạt động của toàn bộ hệ thống AnSinhSo theo thời gian thực.

Mục tiêu:

* Phát hiện sự cố sớm.
* Giảm thời gian gián đoạn.
* Đánh giá hiệu năng.
* Hỗ trợ vận hành Production.

---

## 55.2 Monitoring Scope

Theo dõi:

* API
* Frontend
* SQL Server
* IIS
* Windows Server
* AI Service
* GIS Service
* Zalo OA
* Background Jobs
* File Storage

---

## 55.3 Key Metrics

Theo dõi tối thiểu:

* CPU
* Memory
* Disk
* Network
* Response Time
* Error Rate
* Database Connections
* Active Users
* Queue Length

---

## 55.4 Health Check

Mỗi dịch vụ phải có Health Endpoint.

Ví dụ:

```text
/health

/health/live

/health/ready
```

---

# ============================================================================

# 56. LOGGING OPERATIONS

# ============================================================================

## 56.1 Logging Principles

Logging phải:

* Structured
* Searchable
* Traceable
* Consistent

---

## 56.2 Log Categories

Bao gồm:

* Application Log
* Security Log
* Audit Log
* IIS Log
* Windows Event Log
* SQL Log

---

## 56.3 Log Retention

Thời gian lưu Log được xác định theo chính sách của đơn vị triển khai.

Có thể phân loại:

* Daily
* Weekly
* Monthly
* Archive

---

## 56.4 Log Protection

Log phải:

* Không chỉnh sửa.
* Không lưu Password.
* Không lưu Secret.
* Không lưu Token đầy đủ.

---

# ============================================================================

# 57. ALERTING STANDARDS

# ============================================================================

## 57.1 Purpose

Alerting giúp thông báo ngay khi hệ thống gặp bất thường.

---

## 57.2 Alert Levels

Phân loại:

* Information
* Warning
* Critical

---

## 57.3 Alert Conditions

Ví dụ:

* API Down
* Database Down
* IIS Stopped
* CPU quá cao
* Memory quá cao
* Disk gần đầy
* Login Failure tăng bất thường
* AI Service không phản hồi
* Zalo OA lỗi liên tiếp

---

## 57.4 Notification Channels

Có thể sử dụng:

* Email
* Microsoft Teams
* Slack
* SMS
* Zalo OA (nội bộ)

---

# ============================================================================

# 58. HIGH AVAILABILITY (HA)

# ============================================================================

## 58.1 Principles

Production cần hướng tới:

* High Availability
* Fault Tolerance
* Minimal Downtime

---

## 58.2 HA Strategy

Có thể mở rộng:

* Load Balancer
* Multiple IIS Servers
* SQL Server Always On
* Redis Cache
* Reverse Proxy

---

## 58.3 Single Point of Failure

Hạn chế tối đa các điểm lỗi đơn (Single Point of Failure).

---

# ============================================================================

# 59. DISASTER RECOVERY OPERATIONS

# ============================================================================

## 59.1 Disaster Recovery Plan

Chuẩn bị:

* Backup Procedure
* Restore Procedure
* Recovery Checklist
* Incident Contact

---

## 59.2 Recovery Testing

Định kỳ kiểm tra:

* Khôi phục Database
* Khôi phục File
* Khôi phục Application

---

## 59.3 Recovery Objectives

Xác định:

* RTO (Recovery Time Objective)
* RPO (Recovery Point Objective)

Theo chính sách vận hành.

---

# ============================================================================

# 60. PATCH MANAGEMENT

# ============================================================================

## 60.1 Patch Policy

Định kỳ cập nhật:

* Windows Server
* IIS
* .NET Runtime
* SQL Server
* NuGet Packages
* JavaScript Libraries

---

## 60.2 Testing

Mọi bản vá phải được kiểm thử trên:

* Development
* Testing
* Staging

trước khi áp dụng Production.

---

## 60.3 Emergency Patch

Đối với lỗ hổng nghiêm trọng:

* Đánh giá rủi ro.
* Triển khai theo quy trình khẩn cấp.
* Có kế hoạch Rollback.

---

# ============================================================================

# 61. MAINTENANCE WINDOWS

# ============================================================================

## 61.1 Maintenance Policy

Các hoạt động bảo trì nên được thực hiện trong khung thời gian đã thông báo trước.

---

## 61.2 Maintenance Scope

Bao gồm:

* Deployment
* Database Maintenance
* Security Patch
* Backup Validation
* Infrastructure Upgrade

---

## 61.3 Communication

Thông báo tới người dùng và các bên liên quan trước khi bảo trì theo quy trình vận hành.

---

# ============================================================================

# 62. AI SERVICE DEPLOYMENT

# ============================================================================

## 62.1 AI Service

AI Service triển khai độc lập với Backend.

---

## 62.2 AI Configuration

Quản lý:

* API Key
* Model Version
* Timeout
* Retry Policy

---

## 62.3 Monitoring

Theo dõi:

* Response Time
* Error Rate
* Token Usage
* Availability

---

# ============================================================================

# 63. GIS SERVICE DEPLOYMENT

# ============================================================================

## 63.1 GIS Service

GIS Service được triển khai tách biệt để dễ mở rộng.

---

## 63.2 GIS Resources

Quản lý:

* Map Tiles
* Spatial Database
* Layer Configuration
* Cache

---

## 63.3 GIS Monitoring

Theo dõi:

* API
* Tile Server
* Spatial Query
* Cache Hit Rate

---

# ============================================================================

# 64. ZALO OA DEPLOYMENT

# ============================================================================

## 64.1 Configuration

Quản lý:

* OA ID
* Access Token
* Refresh Token
* Webhook URL

---

## 64.2 Deployment Validation

Kiểm tra:

* Webhook
* Token
* Message Sending
* Retry

---

## 64.3 Monitoring

Theo dõi:

* Success Rate
* Failed Requests
* Retry Count

---

# ============================================================================

# 65. PRODUCTION OPERATIONS

# ============================================================================

## 65.1 Daily Operations

Kiểm tra:

* Health Check
* Logs
* Monitoring Dashboard
* Backup Status

---

## 65.2 Weekly Operations

* Kiểm tra hiệu năng.
* Kiểm tra dung lượng.
* Kiểm tra bảo mật.
* Kiểm tra Backup.

---

## 65.3 Monthly Operations

* Đánh giá hạ tầng.
* Kiểm tra Patch.
* Kiểm tra Disaster Recovery.
* Rà soát quyền truy cập.

---

# ============================================================================

# 66. DEPLOYMENT DEFINITION OF DONE

# ============================================================================

Một phiên bản chỉ được xem là triển khai hoàn tất khi:

* Build thành công.
* Publish thành công.
* Deployment thành công.
* Migration hoàn thành.
* Health Check đạt.
* Smoke Test đạt.
* Monitoring hoạt động.
* Logging hoạt động.
* Backup xác nhận.
* Rollback khả dụng.
* Security Review hoàn thành.
* Release Notes được cập nhật.

---

# ============================================================================

# 67. AI CODING RULES (DEPLOYMENT)

# ============================================================================

Mọi AI Coding Assistant phải:

* Tuân thủ `10_DEPLOYMENT_STANDARDS.md`.
* Không Hard-code Environment.
* Không Hard-code Secret.
* Không bỏ qua Health Check.
* Không bỏ qua Backup.
* Không sinh cấu hình Production không an toàn.
* Không vô hiệu hóa HTTPS.
* Không sinh Pipeline bỏ qua Test.
* Sinh cấu hình có khả năng mở rộng và Rollback.

Áp dụng cho:

* ChatGPT
* Gemini
* GitHub Copilot
* Cursor
* Cline
* Continue
* AntiGravity AI

---

# ============================================================================

# 68. FINAL DEPLOYMENT CHECKLIST

# ============================================================================

Trước khi Go-Live cần xác nhận:

* Build thành công.
* CI/CD Pipeline thành công.
* Version chính xác.
* Release Notes hoàn chỉnh.
* Backup hoàn thành.
* Database Migration thành công.
* Health Check đạt.
* Smoke Test đạt.
* Monitoring hoạt động.
* Alerting hoạt động.
* Logging hoạt động.
* HTTPS hoạt động.
* SSL hợp lệ.
* Firewall đúng cấu hình.
* Rollback đã sẵn sàng.
* AI Service hoạt động.
* GIS Service hoạt động.
* Zalo OA hoạt động.
* Security Review hoàn thành.
* Có phê duyệt Go-Live.

---

# ============================================================================

# 69. CHANGE LOG

# ============================================================================

| Version | Date       | Description                             |
| ------- | ---------- | --------------------------------------- |
| 1.0.0   | 2026-07-12 | Initial Enterprise Deployment Standards |

---

# ============================================================================

# 70. RELATED DOCUMENTS

# ============================================================================

* 00_PROJECT_BOOTSTRAP.md
* 00_PROJECT_PROGRESS.md
* 00_PROJECT_INDEX.md
* 04_CODING_STANDARDS.md
* 05_DATABASE_RULES.md
* 06_API_STANDARDS.md
* 07_FRONTEND_STANDARDS.md
* 08_BACKEND_STANDARDS.md
* 09_SECURITY_STANDARDS.md
* 11_TESTING_STANDARDS.md
* 12_DEVOPS_STANDARDS.md
* DEPLOYMENT_GUIDE.md
* CLEAN_ARCHITECTURE.md

---

# ============================================================================

# END OF DOCUMENT

# ============================================================================
