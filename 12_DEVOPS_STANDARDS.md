````md
# ============================================================================
# 12_DEVOPS_STANDARDS.md
# ============================================================================

Project         : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
Document Type   : Enterprise DevOps Standards
Version         : 1.0.0
Status          : FROZEN
Owner           : Project Architecture Team

Architecture    : Enterprise Clean Architecture
Framework        : ASP.NET Core 8
Frontend         : HTML / CSS / JavaScript
Database         : SQL Server 2022
CI/CD            : GitHub Actions
Container        : Docker
Cloud Ready      : Azure / AWS / GCP
Last Updated     : 2026-07-12

============================================================================

# 1. PURPOSE

Tài liệu này quy định tiêu chuẩn DevOps áp dụng cho toàn bộ dự án AnSinhSo.

Mục tiêu:

- Chuẩn hóa quy trình DevOps.
- Tự động hóa Build và Deployment.
- Đảm bảo chất lượng Release.
- Tăng khả năng mở rộng.
- Đảm bảo vận hành ổn định.
- Hỗ trợ Continuous Delivery.
- Hỗ trợ AI Coding Assistant.

---

# 2. SCOPE

Áp dụng cho:

- Source Code
- Git Repository
- CI/CD Pipeline
- Docker
- Infrastructure
- Monitoring
- Logging
- Release
- Production
- AI Services
- GIS Services
- Zalo OA

---

# 3. DEVOPS OBJECTIVES

Mục tiêu của DevOps:

- Continuous Integration
- Continuous Delivery
- Infrastructure Automation
- Automated Testing
- Monitoring
- Security
- High Availability

---

# 4. DEVOPS PRINCIPLES

Áp dụng các nguyên tắc:

- Automation First
- Everything as Code
- Immutable Infrastructure
- Shift Left Security
- Continuous Feedback
- Continuous Improvement
- Observability by Design

---

## Principle 01

Tự động hóa tối đa các tác vụ lặp lại.

---

## Principle 02

Mọi thay đổi đều phải được quản lý bằng Source Control.

---

## Principle 03

Infrastructure phải có khả năng tái tạo.

---

## Principle 04

Bảo mật được tích hợp ngay từ đầu trong Pipeline.

---

## Principle 05

Mọi thành phần phải có khả năng giám sát và truy vết.

---

# 5. DEVOPS LIFECYCLE

```text
Planning

↓

Development

↓

Build

↓

Testing

↓

Security Scan

↓

Package

↓

Deploy

↓

Monitor

↓

Feedback

↓

Continuous Improvement
```

---

# 6. DEVOPS ROLES

| Vai trò | Trách nhiệm |
|----------|-------------|
| Developer | Phát triển tính năng, Unit Test |
| DevOps Engineer | Xây dựng và vận hành Pipeline |
| QA Engineer | Kiểm thử và xác nhận chất lượng |
| Solution Architect | Thiết kế kiến trúc DevOps |
| Project Manager | Quản lý Release |
| System Administrator | Quản trị hạ tầng |
| AI Coding Assistant | Sinh mã theo chuẩn DevOps |

---

# 7. DEVOPS ARCHITECTURE

Chuỗi triển khai chuẩn:

```text
Developer

↓

Git Repository

↓

CI Pipeline

↓

Build

↓

Automated Test

↓

Artifact

↓

Staging

↓

UAT

↓

Production

↓

Monitoring

↓

Feedback
```

---

# 8. DEVOPS SUCCESS METRICS

Theo dõi:

- Deployment Frequency
- Lead Time for Changes
- Change Failure Rate
- Mean Time to Recovery (MTTR)
- Build Success Rate
- Test Success Rate
- Release Success Rate

---

# 9. DEVOPS ARTIFACTS

Bao gồm:

- Source Code
- Build Artifact
- Docker Image
- Release Package
- Deployment Script
- Infrastructure Script
- Release Notes
- Change Log

---

# 10. DEVOPS BASELINE

Trước khi chuyển sang Phase 2A cần xác nhận:

- Repository được chuẩn hóa.
- Branch Strategy được áp dụng.
- Build thành công.
- Testing đạt yêu cầu.
- Security Scan sẵn sàng.
- Pipeline hoạt động.
- Monitoring được tích hợp.

---

# End of Phase 1

Phase tiếp theo:

- GitOps
- Infrastructure as Code (IaC)
- Configuration Management
- Environment Management
- Secret Management
- Artifact Management
- Container Registry
- Repository Standards
````
````md id="devops-phase2a"
# ============================================================================
# 11. GITOPS STANDARDS
# ============================================================================

## 11.1 Purpose

GitOps là phương pháp quản lý hạ tầng và triển khai hệ thống thông qua Git Repository.

Git Repository là nguồn dữ liệu duy nhất (Single Source of Truth) cho:

- Infrastructure
- Configuration
- Deployment
- Kubernetes Manifests
- CI/CD Pipeline

---

## 11.2 GitOps Principles

Áp dụng các nguyên tắc:

- Git là nguồn dữ liệu chính.
- Mọi thay đổi đều thông qua Pull Request.
- Không chỉnh sửa trực tiếp trên Production.
- Có khả năng Audit.
- Có khả năng Rollback.

---

## 11.3 GitOps Workflow

```text
Developer

↓

Git Commit

↓

Pull Request

↓

Code Review

↓

Merge

↓

CI Pipeline

↓

CD Pipeline

↓

Production
```

---

## 11.4 Benefits

GitOps giúp:

- Dễ kiểm soát thay đổi.
- Dễ khôi phục phiên bản.
- Tăng khả năng truy vết.
- Hạn chế lỗi do thao tác thủ công.

---

# ============================================================================
# 12. INFRASTRUCTURE AS CODE (IaC)
# ============================================================================

## 12.1 Purpose

Toàn bộ hạ tầng nên được định nghĩa bằng mã nguồn (Infrastructure as Code).

---

## 12.2 IaC Scope

Bao gồm:

- Virtual Machine
- Network
- Firewall
- Storage
- IIS Configuration
- SQL Server Configuration
- Docker
- Kubernetes

---

## 12.3 IaC Principles

Infrastructure phải:

- Có Version.
- Có Review.
- Có thể tái tạo.
- Có thể tự động triển khai.

---

## 12.4 Suggested Tools

Có thể sử dụng:

- Terraform
- Bicep
- ARM Templates
- Ansible
- PowerShell DSC

Việc lựa chọn công cụ phụ thuộc vào hạ tầng triển khai.

---

# ============================================================================
# 13. CONFIGURATION MANAGEMENT
# ============================================================================

## 13.1 Purpose

Quản lý cấu hình nhất quán giữa các môi trường.

---

## 13.2 Configuration Categories

Bao gồm:

- Application Configuration
- Infrastructure Configuration
- Security Configuration
- Database Configuration
- Logging Configuration

---

## 13.3 Configuration Rules

Không Hard-code:

- URL
- Password
- API Key
- Token
- Connection String

---

## 13.4 Configuration Source

Ưu tiên:

- appsettings.json
- appsettings.{Environment}.json
- Environment Variables
- Secret Store

---

# ============================================================================
# 14. ENVIRONMENT MANAGEMENT
# ============================================================================

## 14.1 Standard Environments

Chuẩn môi trường:

```text
Local

↓

Development

↓

Testing

↓

UAT

↓

Staging

↓

Production
```

---

## 14.2 Environment Isolation

Mỗi môi trường phải:

- Database riêng.
- Configuration riêng.
- Secret riêng.
- Logging riêng.

---

## 14.3 Environment Promotion

Chỉ được Promote khi:

- Testing đạt.
- Security đạt.
- QA phê duyệt.

---

# ============================================================================
# 15. SECRET MANAGEMENT
# ============================================================================

## 15.1 Purpose

Bảo vệ toàn bộ thông tin nhạy cảm.

---

## 15.2 Secret Types

Bao gồm:

- Password
- JWT Secret
- API Key
- Connection String
- OAuth Secret
- AI API Key
- Zalo OA Secret

---

## 15.3 Secret Storage

Khuyến nghị:

- Azure Key Vault
- AWS Secrets Manager
- HashiCorp Vault
- Environment Variables

---

## 15.4 Secret Rotation

Secrets cần:

- Định kỳ thay đổi.
- Thu hồi khi lộ lọt.
- Ghi nhận lịch sử thay đổi.

---

# ============================================================================
# 16. ARTIFACT MANAGEMENT
# ============================================================================

## 16.1 Purpose

Quản lý các Artifact được sinh ra từ quá trình Build.

---

## 16.2 Artifact Types

Bao gồm:

- Build Package
- Docker Image
- SQL Scripts
- Release Package
- Documentation
- Test Report

---

## 16.3 Artifact Principles

Artifact phải:

- Có Version.
- Không bị chỉnh sửa sau Build.
- Có thể tải lại.
- Có khả năng kiểm chứng.

---

## 16.4 Artifact Retention

Chính sách lưu trữ cần xác định:

- Thời gian lưu.
- Phiên bản lưu.
- Quy trình xóa.

---

# ============================================================================
# 17. CONTAINER REGISTRY
# ============================================================================

## 17.1 Purpose

Lưu trữ và quản lý Docker Image.

---

## 17.2 Supported Registry

Có thể sử dụng:

- GitHub Container Registry
- Azure Container Registry
- Docker Hub
- Amazon ECR

---

## 17.3 Image Tagging

Chuẩn đặt Tag:

```text
v1.0.0

v1.1.0

latest (Development only)
```

Không sử dụng `latest` trên Production.

---

## 17.4 Image Security

Image phải:

- Được quét lỗ hổng.
- Có Version rõ ràng.
- Không chứa Secret.

---

# ============================================================================
# 18. REPOSITORY STANDARDS
# ============================================================================

## 18.1 Repository Structure

Khuyến nghị:

```text
src/

tests/

docs/

deployment/

scripts/

database/

.github/

README.md
```

---

## 18.2 Repository Rules

Repository phải:

- Có README.
- Có License (nếu áp dụng).
- Có CHANGELOG.
- Có .gitignore.
- Có CONTRIBUTING (nếu phát triển nhiều thành viên).

---

## 18.3 Branch Protection

Áp dụng cho:

- main
- develop

Yêu cầu:

- Pull Request.
- Code Review.
- CI thành công.
- Không Force Push.

---

# ============================================================================
# 19. DEVOPS GOVERNANCE
# ============================================================================

## 19.1 Governance Principles

Mọi thay đổi phải:

- Có người chịu trách nhiệm.
- Có lịch sử thay đổi.
- Có khả năng kiểm toán.
- Có khả năng phục hồi.

---

## 19.2 Approval Flow

```text
Developer

↓

Pull Request

↓

Reviewer

↓

QA

↓

Project Lead

↓

Merge
```

---

# ============================================================================
# 20. PHASE 2A CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2B cần xác nhận:

- GitOps được áp dụng.
- Infrastructure được quản lý bằng IaC (nếu triển khai).
- Configuration được chuẩn hóa.
- Environment được tách biệt.
- Secrets được quản lý an toàn.
- Artifact được quản lý theo Version.
- Container Registry sẵn sàng.
- Repository tuân thủ tiêu chuẩn.
- Branch Protection được kích hoạt.
- Quy trình Governance được áp dụng.

---

# End of Phase 2A

Phase tiếp theo:

- CI Pipeline Standards
- CD Pipeline Standards
- Build Automation
- Deployment Automation
- Pipeline Security
- Pipeline Versioning
- Release Automation
- Pipeline Monitoring
- Pipeline Failure Recovery
- DevSecOps Integration
````
````md
# ============================================================================
# 21. CONTINUOUS INTEGRATION (CI) PIPELINE
# ============================================================================

## 21.1 Purpose

Continuous Integration (CI) giúp:

- Build tự động.
- Kiểm thử tự động.
- Phát hiện lỗi sớm.
- Chuẩn hóa chất lượng mã nguồn.
- Giảm rủi ro khi tích hợp.

---

## 21.2 CI Principles

Pipeline phải:

- Chạy tự động.
- Có thể lặp lại.
- Có Logging.
- Có Version.
- Có khả năng mở rộng.

---

## 21.3 Standard CI Pipeline

```text
Developer

↓

Git Push

↓

Restore Packages

↓

Build

↓

Unit Test

↓

Integration Test

↓

Static Analysis

↓

Security Scan

↓

Artifact

↓

Publish Artifact
```

---

## 21.4 CI Trigger

Pipeline được kích hoạt khi:

- Push
- Pull Request
- Merge
- Scheduled Build
- Manual Trigger

---

# ============================================================================
# 22. CONTINUOUS DELIVERY (CD) PIPELINE
# ============================================================================

## 22.1 Purpose

Continuous Delivery tự động chuẩn bị phiên bản sẵn sàng triển khai.

---

## 22.2 CD Flow

```text
Artifact

↓

Deploy Development

↓

Integration Test

↓

Deploy Testing

↓

Deploy UAT

↓

Deploy Staging

↓

Release Approval

↓

Production
```

---

## 22.3 Approval Gates

Bắt buộc:

- QA Approval
- Project Lead Approval
- Production Approval

---

## 22.4 Deployment Strategy

Hỗ trợ:

- Rolling Deployment
- Blue-Green Deployment
- Canary Deployment

Việc lựa chọn chiến lược phụ thuộc vào quy mô hạ tầng.

---

# ============================================================================
# 23. BUILD AUTOMATION
# ============================================================================

## 23.1 Purpose

Toàn bộ quá trình Build phải được tự động hóa.

---

## 23.2 Build Steps

- Restore
- Compile
- Unit Test
- Static Analysis
- Package
- Publish Artifact

---

## 23.3 Build Validation

Một Build chỉ thành công khi:

- Không lỗi Compile.
- Không lỗi Critical.
- Test thành công.
- Artifact được tạo.

---

# ============================================================================
# 24. DEPLOYMENT AUTOMATION
# ============================================================================

## 24.1 Principles

Deployment phải:

- Repeatable
- Traceable
- Auditable
- Rollback Ready

---

## 24.2 Deployment Targets

- Development
- Testing
- UAT
- Staging
- Production

---

## 24.3 Deployment Validation

Sau Deployment kiểm tra:

- Health Check
- Smoke Test
- API
- Database
- Monitoring

---

# ============================================================================
# 25. PIPELINE SECURITY
# ============================================================================

## 25.1 Purpose

Pipeline phải được bảo vệ khỏi thay đổi trái phép.

---

## 25.2 Security Controls

Áp dụng:

- MFA
- Branch Protection
- Secret Store
- Signed Commits (khuyến nghị)
- Least Privilege

---

## 25.3 Secret Protection

Không lưu:

- Password
- API Key
- JWT Secret
- Certificate
- Token

trong Repository hoặc Pipeline Script.

---

# ============================================================================
# 26. PIPELINE VERSIONING
# ============================================================================

## 26.1 Version Rules

Pipeline cũng phải có Version.

---

## 26.2 Version History

Theo dõi:

- Người thay đổi.
- Thời gian.
- Nội dung thay đổi.
- Lý do thay đổi.

---

## 26.3 Change Approval

Mọi thay đổi Pipeline phải:

- Review.
- Test.
- Approval.

---

# ============================================================================
# 27. RELEASE AUTOMATION
# ============================================================================

## 27.1 Purpose

Tự động hóa quá trình phát hành.

---

## 27.2 Release Workflow

```text
Build

↓

Package

↓

Tag

↓

Release Notes

↓

Artifact

↓

Deploy
```

---

## 27.3 Release Validation

Kiểm tra:

- Version
- Migration
- Backup
- Smoke Test
- Rollback

---

# ============================================================================
# 28. PIPELINE MONITORING
# ============================================================================

## 28.1 Monitoring Scope

Theo dõi:

- Build Time
- Deployment Time
- Success Rate
- Failure Rate
- Queue Time

---

## 28.2 Alerts

Thông báo khi:

- Build Fail
- Test Fail
- Deployment Fail
- Security Scan Fail

---

## 28.3 Dashboard

Theo dõi:

- Active Builds
- Running Pipelines
- Recent Releases
- Failed Deployments

---

# ============================================================================
# 29. PIPELINE FAILURE RECOVERY
# ============================================================================

## 29.1 Failure Handling

Nếu Pipeline thất bại:

- Dừng Pipeline.
- Ghi Log.
- Gửi Alert.
- Không tiếp tục Deploy.

---

## 29.2 Recovery

Sau khi sửa lỗi:

- Chạy lại Pipeline.
- Kiểm tra Artifact.
- Kiểm tra Version.
- Thực hiện Smoke Test.

---

# ============================================================================
# 30. DEVSECOPS INTEGRATION
# ============================================================================

## 30.1 Purpose

Security được tích hợp xuyên suốt trong Pipeline.

---

## 30.2 Security Checks

Pipeline nên bao gồm:

- SAST
- Dependency Scan
- Secret Scan
- Container Scan
- License Scan

---

## 30.3 Security Gates

Không cho phép Release nếu:

- Có Critical Vulnerability.
- Có Secret bị lộ.
- Có Dependency nguy hiểm.
- Không đạt Security Policy.

---

# ============================================================================
# 31. PHASE 2B CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2C cần xác nhận:

- CI Pipeline hoạt động.
- CD Pipeline hoạt động.
- Build Automation hoàn chỉnh.
- Deployment Automation hoạt động.
- Pipeline Security được áp dụng.
- Pipeline Versioning được quản lý.
- Release Automation hoàn thành.
- Pipeline Monitoring hoạt động.
- Pipeline Failure Recovery được kiểm thử.
- DevSecOps được tích hợp.

---

# End of Phase 2B

Phase tiếp theo:

- Docker Standards
- Docker Compose Standards
- Kubernetes Standards
- Helm Standards
- Container Networking
- Container Security
- Container Scaling
- Service Mesh
- Reverse Proxy
- Cloud Native Architecture
- Container Operations
````
````md
# ============================================================================
# 32. DOCKER STANDARDS
# ============================================================================

## 32.1 Purpose

Docker là nền tảng chuẩn để đóng gói và triển khai ứng dụng AnSinhSo.

Mục tiêu:

- Đồng nhất môi trường phát triển.
- Giảm khác biệt giữa Development và Production.
- Tăng khả năng triển khai.
- Hỗ trợ CI/CD.
- Hỗ trợ mở rộng hệ thống.

---

## 32.2 Standard Images

Khuyến nghị sử dụng:

Backend

- ASP.NET Core 8 Runtime
- ASP.NET Core 8 SDK (Build)

Frontend

- Nginx
- Node.js LTS (Build)

Database

- SQL Server 2022

---

## 32.3 Multi-stage Build

Tất cả Dockerfile của Backend phải sử dụng Multi-stage Build.

Ví dụ:

Build

↓

Publish

↓

Runtime

Lợi ích:

- Giảm kích thước Image.
- Tăng bảo mật.
- Build nhanh hơn.

---

## 32.4 Dockerfile Standards

Dockerfile cần:

- Có comment.
- Không Hard-code Secret.
- Không chạy bằng Root User nếu có thể.
- Sử dụng phiên bản Image cụ thể.
- Có HEALTHCHECK khi phù hợp.

---

## 32.5 Image Tagging

Chuẩn Version:

```text
ansinhso-api:1.0.0

ansinhso-web:1.0.0

ansinhso-api:latest (Development only)
```

Production không sử dụng tag `latest`.

---

# ============================================================================
# 33. DOCKER COMPOSE STANDARDS
# ============================================================================

## 33.1 Purpose

Docker Compose phục vụ môi trường:

- Development
- Testing
- Demo

---

## 33.2 Standard Services

Compose có thể bao gồm:

- API
- Frontend
- SQL Server
- Redis
- Monitoring

---

## 33.3 Network Rules

- Các Container giao tiếp qua Docker Network.
- Không mở cổng không cần thiết.
- Chỉ Publish các cổng phục vụ người dùng hoặc quản trị.

---

## 33.4 Environment Variables

Không lưu Secret trực tiếp trong file Compose.

Ưu tiên:

- `.env`
- Environment Variables
- Secret Store

---

# ============================================================================
# 34. KUBERNETES STANDARDS
# ============================================================================

## 34.1 Purpose

Kubernetes là nền tảng điều phối Container cho môi trường Production có quy mô lớn.

---

## 34.2 Kubernetes Resources

Chuẩn tài nguyên:

- Namespace
- Deployment
- Service
- ConfigMap
- Secret
- Ingress
- Persistent Volume
- Horizontal Pod Autoscaler

---

## 34.3 Namespace Standards

Tối thiểu:

```text
development

testing

staging

production
```

---

## 34.4 Deployment Strategy

Hỗ trợ:

- Rolling Update
- Blue-Green
- Canary

---

## 34.5 Resource Limits

Mỗi Pod nên cấu hình:

- CPU Request
- CPU Limit
- Memory Request
- Memory Limit

Để tránh ảnh hưởng lẫn nhau giữa các dịch vụ.

---

# ============================================================================
# 35. HELM STANDARDS
# ============================================================================

## 35.1 Purpose

Helm dùng để quản lý và triển khai Kubernetes Manifest.

---

## 35.2 Chart Structure

Chuẩn:

```text
Chart.yaml

values.yaml

templates/

charts/
```

---

## 35.3 Versioning

Helm Chart cần:

- Semantic Versioning.
- Change Log.
- Release History.

---

# ============================================================================
# 36. CONTAINER NETWORKING
# ============================================================================

## 36.1 Network Principles

Container chỉ giao tiếp qua:

- Internal Network
- Service Discovery

---

## 36.2 Service Communication

Ưu tiên:

- HTTPS
- TLS
- Internal DNS

---

## 36.3 Network Policies

Áp dụng nguyên tắc:

Least Privilege Network.

Container chỉ được phép giao tiếp với dịch vụ cần thiết.

---

# ============================================================================
# 37. CONTAINER SECURITY
# ============================================================================

## 37.1 Security Principles

Container phải:

- Chạy với quyền tối thiểu.
- Không chứa Secret.
- Không chứa dữ liệu nhạy cảm.
- Không chứa công cụ không cần thiết.

---

## 37.2 Image Security

Image phải:

- Được quét lỗ hổng.
- Được ký số (nếu áp dụng).
- Có nguồn gốc rõ ràng.

---

## 37.3 Runtime Security

Theo dõi:

- Privileged Container.
- Root Container.
- Unexpected Process.
- File Integrity.

---

# ============================================================================
# 38. CONTAINER SCALING
# ============================================================================

## 38.1 Scaling Strategy

Hỗ trợ:

- Horizontal Scaling.
- Vertical Scaling.

---

## 38.2 Auto Scaling

Có thể áp dụng:

- CPU Based.
- Memory Based.
- Custom Metrics.

---

## 38.3 High Availability

Khuyến nghị:

- Nhiều Replica.
- Health Check.
- Readiness Probe.
- Liveness Probe.

---

# ============================================================================
# 39. SERVICE MESH
# ============================================================================

## 39.1 Purpose

Service Mesh hỗ trợ:

- Traffic Management.
- Service Discovery.
- Mutual TLS.
- Observability.

---

## 39.2 Suggested Platforms

Có thể sử dụng:

- Istio
- Linkerd

Việc triển khai phụ thuộc quy mô hệ thống.

---

# ============================================================================
# 40. REVERSE PROXY & CLOUD NATIVE
# ============================================================================

## 40.1 Reverse Proxy

Có thể sử dụng:

- Nginx
- Traefik
- YARP (ASP.NET Core)
- HAProxy

---

## 40.2 Cloud Native Principles

Ứng dụng cần hướng tới:

- Stateless Services.
- Immutable Infrastructure.
- Configuration ngoài ứng dụng.
- Health Check.
- Auto Scaling.

---

## 40.3 Cloud Compatibility

Thiết kế tương thích với:

- Microsoft Azure
- Amazon Web Services (AWS)
- Google Cloud Platform (GCP)
- Hạ tầng On-Premises

---

# ============================================================================
# 41. CONTAINER OPERATIONS
# ============================================================================

## 41.1 Operational Checklist

Theo dõi:

- Image Version.
- Container Status.
- CPU.
- Memory.
- Disk Usage.
- Restart Count.
- Logs.

---

## 41.2 Maintenance

Định kỳ:

- Cập nhật Base Image.
- Quét lỗ hổng.
- Xóa Image không sử dụng.
- Kiểm tra Resource Usage.

---

# ============================================================================
# 42. PHASE 2C CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2D cần xác nhận:

- Dockerfile tuân thủ tiêu chuẩn.
- Docker Compose được chuẩn hóa.
- Kubernetes Manifest được chuẩn hóa.
- Helm Chart có Version.
- Container Networking an toàn.
- Container Security đạt yêu cầu.
- Auto Scaling sẵn sàng.
- Reverse Proxy hoạt động.
- Cloud Native Principles được áp dụng.
- Container Operations được giám sát.

---

# End of Phase 2C

Phase tiếp theo:

- Monitoring Standards
- Logging Standards
- Distributed Tracing
- Observability
- Alert Management
- Incident Management
- Operational Runbook
- Capacity Planning
- SLA / SLO / SLI
- Production Operations
````
````md
# ============================================================================
# 43. MONITORING STANDARDS
# ============================================================================

## 43.1 Purpose

Monitoring giúp theo dõi trạng thái hoạt động của toàn bộ hệ thống AnSinhSo theo thời gian thực.

Mục tiêu:

- Phát hiện sự cố sớm.
- Theo dõi hiệu năng.
- Giảm thời gian gián đoạn.
- Hỗ trợ điều tra sự cố.
- Cung cấp dữ liệu cho cải tiến hệ thống.

---

## 43.2 Monitoring Scope

Bao gồm:

- Backend API
- Frontend
- SQL Server
- Docker
- Kubernetes
- AI Services
- GIS Services
- Zalo OA Integration
- Network
- Operating System

---

## 43.3 Monitoring Principles

Monitoring phải:

- Real-time
- Centralized
- Highly Available
- Auditable
- Historical

---

## 43.4 Suggested Tools

Có thể sử dụng:

- Prometheus
- Grafana
- Azure Monitor
- AWS CloudWatch
- Zabbix
- Datadog

---

# ============================================================================
# 44. LOGGING STANDARDS
# ============================================================================

## 44.1 Purpose

Logging phục vụ:

- Điều tra lỗi.
- Kiểm toán.
- Theo dõi nghiệp vụ.
- Phân tích hiệu năng.

---

## 44.2 Logging Levels

Chuẩn mức Log:

```text
Trace

Debug

Information

Warning

Error

Critical
```

---

## 44.3 Logging Rules

Log phải:

- Có Timestamp.
- Có Correlation ID.
- Có User ID (nếu phù hợp).
- Có Request ID.
- Có Module.
- Có Severity.

---

## 44.4 Không được ghi Log

Không Log:

- Password.
- JWT Token.
- API Key.
- Connection String.
- OTP.
- Thông tin nhạy cảm chưa được che giấu.

---

## 44.5 Suggested Platforms

Có thể sử dụng:

- Serilog
- Seq
- ELK Stack
- OpenSearch
- Azure Log Analytics

---

# ============================================================================
# 45. DISTRIBUTED TRACING
# ============================================================================

## 45.1 Purpose

Distributed Tracing giúp theo dõi một Request xuyên suốt nhiều dịch vụ.

---

## 45.2 Trace Scope

Theo dõi:

- HTTP Request
- Database
- AI Service
- GIS Service
- Zalo OA
- Background Jobs

---

## 45.3 Trace Identifier

Mỗi Request nên có:

- Trace ID
- Span ID
- Correlation ID

---

## 45.4 Suggested Tools

Có thể sử dụng:

- OpenTelemetry
- Jaeger
- Zipkin

---

# ============================================================================
# 46. OBSERVABILITY
# ============================================================================

## 46.1 Purpose

Observability là khả năng hiểu trạng thái hệ thống thông qua:

- Metrics
- Logs
- Traces

---

## 46.2 Three Pillars

```text
Metrics

+

Logs

+

Traces
```

---

## 46.3 Core Metrics

Theo dõi:

- CPU
- Memory
- Disk
- Network
- API Latency
- Error Rate
- Database Connections
- Queue Length

---

## 46.4 Business Metrics

Theo dõi:

- Số hộ gia đình.
- Số đối tượng an sinh.
- Hồ sơ mới.
- Chi trả thành công.
- Tin nhắn Zalo gửi thành công.
- AI Requests.
- GIS Requests.

---

# ============================================================================
# 47. ALERT MANAGEMENT
# ============================================================================

## 47.1 Purpose

Alert giúp phát hiện và phản ứng nhanh với sự cố.

---

## 47.2 Alert Severity

| Level | Ý nghĩa |
|---------|---------|
| Critical | Cần xử lý ngay |
| High | Xử lý trong thời gian ngắn |
| Medium | Theo dõi và xử lý |
| Low | Theo dõi |

---

## 47.3 Alert Rules

Alert cần:

- Có ngưỡng rõ ràng.
- Không tạo cảnh báo trùng lặp.
- Có người chịu trách nhiệm.
- Có lịch sử xử lý.

---

## 47.4 Alert Channels

Có thể gửi qua:

- Email
- Microsoft Teams
- Slack
- SMS
- Zalo OA (nếu phù hợp)

---

# ============================================================================
# 48. INCIDENT MANAGEMENT
# ============================================================================

## 48.1 Purpose

Chuẩn hóa quy trình xử lý sự cố.

---

## 48.2 Incident Workflow

```text
Detection

↓

Alert

↓

Classification

↓

Assignment

↓

Investigation

↓

Resolution

↓

Verification

↓

Closure

↓

Post-Incident Review
```

---

## 48.3 Incident Severity

| Level | Mô tả |
|---------|-------|
| P1 | Toàn bộ hệ thống ngừng hoạt động |
| P2 | Chức năng quan trọng bị ảnh hưởng |
| P3 | Chức năng phụ bị ảnh hưởng |
| P4 | Lỗi nhỏ hoặc cải tiến |

---

# ============================================================================
# 49. OPERATIONAL RUNBOOK
# ============================================================================

## 49.1 Purpose

Runbook mô tả các bước vận hành tiêu chuẩn.

---

## 49.2 Runbook Scope

Bao gồm:

- Deployment
- Rollback
- Backup
- Restore
- Database Migration
- Certificate Renewal
- Secret Rotation
- Service Restart

---

## 49.3 Runbook Requirements

Mỗi Runbook cần:

- Điều kiện thực hiện.
- Các bước chi tiết.
- Kiểm tra sau khi hoàn thành.
- Phương án khôi phục.

---

# ============================================================================
# 50. CAPACITY PLANNING
# ============================================================================

## 50.1 Purpose

Đảm bảo hạ tầng đáp ứng nhu cầu tăng trưởng.

---

## 50.2 Planning Scope

Theo dõi:

- CPU
- RAM
- Storage
- Network
- Database
- API Traffic

---

## 50.3 Capacity Review

Định kỳ đánh giá:

- Hàng tháng.
- Trước mỗi Release lớn.
- Sau các đợt tăng tải.

---

# ============================================================================
# 51. SLA / SLO / SLI
# ============================================================================

## 51.1 Definitions

SLI (Service Level Indicator)

Là chỉ số đo lường chất lượng dịch vụ.

Ví dụ:

- Availability
- Response Time
- Error Rate

---

SLO (Service Level Objective)

Là mục tiêu mong muốn của hệ thống dựa trên SLI.

---

SLA (Service Level Agreement)

Là cam kết chất lượng dịch vụ giữa đơn vị cung cấp và đơn vị sử dụng.

---

## 51.2 Suggested SLI

Theo dõi:

- API Availability
- Login Success Rate
- Database Availability
- AI Response Time
- GIS Availability
- Zalo Notification Success Rate

---

# ============================================================================
# 52. PRODUCTION OPERATIONS
# ============================================================================

## 52.1 Daily Operations

Kiểm tra:

- System Health
- Backup Status
- Error Logs
- Failed Jobs
- Disk Usage
- Security Alerts

---

## 52.2 Weekly Operations

Thực hiện:

- Vulnerability Scan
- Capacity Review
- Log Cleanup
- Patch Review
- Dependency Review

---

## 52.3 Monthly Operations

Thực hiện:

- Disaster Recovery Drill
- Restore Testing
- Security Audit
- Performance Review
- Documentation Review

---

# ============================================================================
# 53. PHASE 2D CHECKLIST
# ============================================================================

Trước khi chuyển sang Phase 2E cần xác nhận:

- Monitoring hoạt động.
- Logging được chuẩn hóa.
- Distributed Tracing được cấu hình.
- Observability đáp ứng yêu cầu.
- Alert Management hoạt động.
- Incident Management được chuẩn hóa.
- Operational Runbook đầy đủ.
- Capacity Planning được thực hiện.
- SLA / SLO / SLI được xác định.
- Production Operations được thiết lập.

---

# End of Phase 2D

Phase tiếp theo:

- Enterprise DevOps Governance
- DevSecOps Governance
- Compliance Standards
- Infrastructure Security
- Cost Management (FinOps)
- AI DevOps Standards
- Business Continuity
- Disaster Recovery Governance
- Enterprise Checklists
- Change Log
- Related Documents
- End of Document
````
```md id="devops-phase2e"
# ============================================================================
# 54. ENTERPRISE DEVOPS GOVERNANCE
# ============================================================================

## 54.1 Purpose

Enterprise DevOps Governance thiết lập các quy tắc quản trị nhằm đảm bảo toàn bộ hoạt động DevOps được thực hiện thống nhất, minh bạch và có khả năng kiểm toán.

---

## 54.2 Governance Objectives

Mục tiêu:

- Chuẩn hóa quy trình DevOps.
- Quản lý rủi ro.
- Đảm bảo tuân thủ.
- Kiểm soát thay đổi.
- Hỗ trợ mở rộng hệ thống.

---

## 54.3 Governance Principles

Mọi thay đổi phải:

- Có yêu cầu thay đổi.
- Có phê duyệt.
- Có lịch sử.
- Có khả năng Rollback.
- Có khả năng Audit.

---

# ============================================================================
# 55. DEVSECOPS GOVERNANCE
# ============================================================================

## 55.1 Purpose

Security được tích hợp xuyên suốt toàn bộ vòng đời DevOps.

---

## 55.2 Security Integration

Security phải xuất hiện tại:

- Planning
- Development
- Build
- Testing
- Deployment
- Monitoring
- Incident Response

---

## 55.3 DevSecOps Controls

Pipeline nên tích hợp:

- Static Application Security Testing (SAST)
- Dynamic Application Security Testing (DAST)
- Software Composition Analysis (SCA)
- Secret Scanning
- Container Image Scanning
- Infrastructure as Code Scanning

---

## 55.4 Security Gates

Không cho phép Release khi:

- Có Blocker Vulnerability.
- Có Critical Vulnerability chưa xử lý.
- Phát hiện Secret trong Repository.
- Không đạt Security Policy.

---

# ============================================================================
# 56. COMPLIANCE STANDARDS
# ============================================================================

## 56.1 Purpose

Đảm bảo hệ thống đáp ứng các quy định nội bộ và yêu cầu pháp lý hiện hành.

---

## 56.2 Compliance Scope

Áp dụng cho:

- Source Code
- Database
- API
- Logging
- Audit
- Deployment
- Backup
- User Management

---

## 56.3 Compliance Principles

Hệ thống cần:

- Có Audit Trail.
- Có Logging.
- Có Version Control.
- Có Data Retention Policy.
- Có Access Control.

---

# ============================================================================
# 57. INFRASTRUCTURE SECURITY
# ============================================================================

## 57.1 Security Scope

Bao gồm:

- Server
- Operating System
- Database
- Network
- Container
- Kubernetes
- Reverse Proxy

---

## 57.2 Hardening Checklist

Thực hiện:

- Cập nhật bản vá bảo mật.
- Tắt dịch vụ không cần thiết.
- Giới hạn cổng mạng.
- Áp dụng Firewall.
- Áp dụng MFA cho tài khoản quản trị.

---

## 57.3 Infrastructure Review

Định kỳ:

- Vulnerability Scan.
- Patch Review.
- Configuration Review.
- Access Review.

---

# ============================================================================
# 58. FINOPS (COST MANAGEMENT)
# ============================================================================

## 58.1 Purpose

Quản lý và tối ưu chi phí vận hành hệ thống.

---

## 58.2 Cost Categories

Theo dõi:

- Compute
- Storage
- Network
- Database
- Monitoring
- AI Services
- GIS Services

---

## 58.3 Cost Optimization

Khuyến nghị:

- Tắt tài nguyên không sử dụng.
- Theo dõi chi phí theo môi trường.
- Tự động mở rộng tài nguyên khi cần.
- Định kỳ rà soát mức sử dụng.

---

# ============================================================================
# 59. AI DEVOPS STANDARDS
# ============================================================================

## 59.1 Purpose

Chuẩn hóa việc sử dụng AI trong quy trình phát triển và vận hành.

---

## 59.2 AI Responsibilities

AI Coding Assistant hỗ trợ:

- Sinh mã nguồn.
- Sinh Unit Test.
- Sinh Documentation.
- Sinh Migration Script.
- Sinh API Documentation.
- Sinh CI/CD Pipeline.
- Sinh Deployment Script.

---

## 59.3 AI Restrictions

AI không được:

- Sinh Secret thật.
- Hard-code Password.
- Hard-code Token.
- Bỏ qua Security Standards.
- Bỏ qua Testing Standards.
- Thay đổi kiến trúc khi chưa được phê duyệt.

---

## 59.4 Approved AI Assistants

Áp dụng cho:

- ChatGPT
- Gemini
- GitHub Copilot
- Cursor
- Cline
- Continue
- AntiGravity AI

Tất cả phải tuân thủ bộ Standards của dự án AnSinhSo.

---

# ============================================================================
# 60. BUSINESS CONTINUITY
# ============================================================================

## 60.1 Purpose

Đảm bảo hoạt động của hệ thống không bị gián đoạn kéo dài.

---

## 60.2 Business Continuity Scope

Bao gồm:

- Application
- Database
- AI Services
- GIS Services
- Zalo OA
- Infrastructure

---

## 60.3 Continuity Measures

Áp dụng:

- Backup.
- Redundancy.
- High Availability.
- Monitoring.
- Disaster Recovery.

---

# ============================================================================
# 61. DISASTER RECOVERY GOVERNANCE
# ============================================================================

## 61.1 Governance Objectives

Đảm bảo khả năng phục hồi sau thảm họa.

---

## 61.2 Disaster Recovery Plan

Bao gồm:

- Incident Response.
- Backup.
- Restore.
- Communication Plan.
- Recovery Validation.

---

## 61.3 Recovery Review

Định kỳ:

- Kiểm thử Restore.
- Kiểm thử Backup.
- Đánh giá RTO.
- Đánh giá RPO.

---

# ============================================================================
# 62. ENTERPRISE DEVOPS CHECKLIST
# ============================================================================

Trước khi Go-Live cần xác nhận:

- GitOps được áp dụng.
- CI/CD Pipeline hoạt động.
- Security Scan đạt yêu cầu.
- Docker Image được kiểm tra.
- Monitoring hoạt động.
- Logging đầy đủ.
- Alerting hoạt động.
- Backup thành công.
- Restore được kiểm thử.
- Disaster Recovery được xác nhận.
- Documentation cập nhật.
- Release Notes hoàn thành.

---

# ============================================================================
# 63. AI CODING RULES (DEVOPS)
# ============================================================================

Mọi AI Coding Assistant phải:

- Tuân thủ `12_DEVOPS_STANDARDS.md`.
- Không thay đổi Pipeline nếu chưa được phê duyệt.
- Không thay đổi Branch Strategy.
- Không tạo Dockerfile vi phạm Security Standards.
- Không sinh Pipeline bỏ qua Unit Test.
- Không bỏ qua Security Scan.
- Không bỏ qua Quality Gates.
- Không Commit Secret.
- Luôn sinh tài liệu tương ứng khi tạo Pipeline hoặc Infrastructure.

---

# ============================================================================
# 64. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------------|--------------------------------|
| 1.0.0 | 2026-07-12 | Initial Enterprise DevOps Standards |

---

# ============================================================================
# 65. RELATED DOCUMENTS
# ============================================================================

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_PROJECT_INDEX.md
- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 07_FRONTEND_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md
- 10_DEPLOYMENT_STANDARDS.md
- 11_TESTING_STANDARDS.md
- 13_AI_DEVELOPMENT_GUIDE.md
- API_SPEC.md
- DATABASE_DESIGN.md
- DEPLOYMENT_GUIDE.md

---

# ============================================================================
# 66. END OF DOCUMENT
# ============================================================================

Tài liệu **12_DEVOPS_STANDARDS.md** là tiêu chuẩn DevOps chính thức của dự án AnSinhSo.

Mọi thành viên dự án, AI Coding Assistant và các quy trình CI/CD phải tuân thủ tài liệu này.

Mọi thay đổi đối với tài liệu phải được:

- Đánh giá tác động.
- Xem xét bởi Solution Architect hoặc Project Lead.
- Cập nhật Change Log.
- Thông báo cho các thành viên liên quan.

---

# END OF FILE
```
