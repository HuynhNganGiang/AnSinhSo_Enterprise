```md
# 18_ZALO_OA_INTEGRATION_GUIDE.md

> **Enterprise Zalo OA Integration Guide**
>
> **Project:** AnSinhSo – Digital Social Welfare System
>
> **Version:** 1.0
>
> **Status:** Production Design
>
> **Architecture:** Enterprise Messaging Platform
>
> **Target Environment:** Production

---

# PHASE 1 – OVERVIEW

---

# 1.1 Purpose

Tài liệu này mô tả toàn bộ kiến trúc tích hợp giữa hệ thống AnSinhSo và Zalo Official Account (OA) nhằm phục vụ vận hành thực tế tại UBND xã Sông Lũy.

Đây không phải tài liệu hướng dẫn sử dụng API đơn thuần mà là tài liệu thiết kế kiến trúc (Architecture Specification) dành cho việc triển khai Production.

---

# 1.2 Objectives

Triển khai một nền tảng giao tiếp thống nhất giữa:

- Người dân
- Cán bộ xã
- Lãnh đạo
- AI Assistant
- Hệ thống AnSinhSo
- Zalo Official Account

đảm bảo:

- Chính xác
- Bảo mật
- Mở rộng
- Dễ bảo trì
- Có khả năng mở rộng sang nhiều kênh khác

---

# 1.3 Scope

Bao gồm

✓ Đồng bộ Zalo OA

✓ Webhook

✓ Messaging Gateway

✓ Notification

✓ AI Assistant

✓ Citizen Services

✓ Delivery Log

✓ Retry

✓ Monitoring

✓ Security

✓ Production Deployment

---

Không bao gồm

✗ Mobile App

✗ SMS Gateway

✗ Email Gateway

✗ Voice AI

Các nội dung trên được thiết kế để có thể bổ sung trong tương lai.

---

# 1.4 Business Goals

Mục tiêu của việc tích hợp Zalo OA

- Người dân không cần cài đặt ứng dụng riêng.
- Người dân sử dụng chính tài khoản Zalo đang có.
- Cán bộ xã gửi thông báo trực tiếp.
- Người dân tra cứu thông tin.
- AI hỗ trợ giải đáp.
- Tự động hóa quy trình truyền thông.

---

# 1.5 Stakeholders

## Internal

Administrator

Cán bộ Lao động – Thương binh và Xã hội

Lãnh đạo UBND xã

Văn phòng

Kế toán

---

## External

Người dân

Zalo Official Account

AI Service

---

# 1.6 Enterprise Architecture

                +----------------------+
                |     Người dân        |
                +----------+-----------+
                           |
                           |
                     Zalo Official Account
                           |
                    HTTPS Webhook
                           |
              +------------+------------+
              | Messaging Gateway       |
              +------------+------------+
                           |
        +------------------+------------------+
        |                  |                  |
 Notification      Citizen Service      AI Bridge
        |                  |                  |
        +---------+--------+------------------+
                  |
            Business Services
                  |
        SQL Server / File Storage
                  |
              Audit Logging

---

# 1.7 Design Principles

Enterprise First

Production Ready

High Availability

Loose Coupling

Event Driven

API First

Security By Design

Audit By Default

Scalable Architecture

---

# 1.8 Functional Overview

Hệ thống phải hỗ trợ:

- Nhận tin nhắn từ người dân
- Gửi thông báo
- Tra cứu hồ sơ
- Tra cứu lịch sử trợ cấp
- Tiếp nhận phản ánh
- AI Chat
- Broadcast
- Theo dõi trạng thái gửi

---

# 1.9 Non Functional Requirements

Response Time

<3 giây

---

Webhook Availability

99.9%

---

Retry

Có

---

Logging

Có

---

Audit

Có

---

Encryption

Có

---

Monitoring

Có

---

Horizontal Scaling

Có

---

# 1.10 Production Requirements

Bắt buộc triển khai:

HTTPS

SSL

Firewall

Reverse Proxy

Health Check

Centralized Logging

Daily Backup

Monitoring Dashboard

Secret Management

Disaster Recovery Plan

---

# 1.11 Integration Principles

Mọi giao tiếp với Zalo OA phải thông qua:

Messaging Gateway

Không cho phép Business Layer gọi trực tiếp Zalo API.

Điều này giúp:

- Thay đổi nhà cung cấp dễ dàng.
- Retry.
- Queue.
- Theo dõi trạng thái.
- Dễ kiểm thử.
- Dễ mở rộng.

---

# 1.12 Expected Outcomes

Sau khi hoàn thành tài liệu này, hệ thống sẽ có khả năng:

✓ Gửi thông báo tự động

✓ Tiếp nhận phản ánh

✓ Tra cứu hồ sơ

✓ AI Chat

✓ Theo dõi Delivery

✓ Retry

✓ Monitoring

✓ Production Deployment

---

# END OF PHASE 1
```
```md
# ============================================================================
# PHASE 2 – ENTERPRISE ARCHITECTURE
# ============================================================================

---

# 2.1 Architecture Overview

AnSinhSo không tích hợp trực tiếp với Zalo Official Account từ Business Layer.

Thay vào đó, mọi giao tiếp phải đi qua một tầng trung gian gọi là:

**Messaging Gateway**

Kiến trúc này giúp:

- Giảm phụ thuộc vào Zalo API
- Dễ mở rộng
- Retry khi gửi lỗi
- Queue Processing
- Logging
- Monitoring
- Testing
- Production Ready

---

# 2.2 High Level Architecture

                         +----------------------+
                         |      Người dân       |
                         +----------+-----------+
                                    |
                                    |
                             Zalo Official Account
                                    |
                             HTTPS Webhook
                                    |
                        +-----------+-----------+
                        |   Zalo OA Provider    |
                        +-----------+-----------+
                                    |
                        +-----------+-----------+
                        | Messaging Gateway     |
                        +-----------+-----------+
                                    |
          +-------------------------+-------------------------+
          |                         |                         |
 Notification Service      Citizen Service            AI Bridge
          |                         |                         |
          +------------+------------+------------+------------+
                       |                         |
                Business Services          AI Assistant
                       |                         |
                SQL Server Database      Knowledge Base
                       |
                 Audit Logging

---

# 2.3 Core Components

## Presentation Layer

- Zalo OA
- Admin Portal
- Dashboard
- AI Chat

---

## Messaging Gateway

Chịu trách nhiệm:

- Receive Message
- Send Message
- Retry
- Queue
- Delivery Status
- Routing

---

## Citizen Service

Các nghiệp vụ:

- Tra cứu hồ sơ
- Tra cứu trợ cấp
- Tra cứu lịch chi trả
- Phản ánh
- Tra cứu thông báo

---

## Notification Service

- Gửi thông báo
- Broadcast
- Template
- Schedule
- Retry

---

## AI Bridge

Kết nối giữa

Zalo OA

↓

RAG Assistant

↓

Knowledge Base

↓

Database

---

## Business Layer

Bao gồm:

- Welfare
- Payment
- Household
- Citizen
- Reports
- Dashboard

---

## Infrastructure Layer

- SQL Server
- Redis Cache
- File Storage
- Logging
- Monitoring

---

# 2.4 Messaging Gateway Responsibilities

Gateway KHÔNG xử lý nghiệp vụ.

Gateway chỉ làm:

✓ Receive

✓ Authenticate

✓ Validate

✓ Route

✓ Queue

✓ Retry

✓ Logging

✓ Monitoring

---

# 2.5 Message Flow

Citizen

↓

Zalo OA

↓

Webhook

↓

Messaging Gateway

↓

Router

↓

Business Service

↓

Database

↓

Response Builder

↓

Gateway

↓

Zalo OA

↓

Citizen

---

# 2.6 Notification Flow

Business Event

↓

Notification Queue

↓

Notification Worker

↓

Template Engine

↓

Zalo OA Provider

↓

Delivery Log

↓

Citizen

---

# 2.7 AI Flow

Citizen

↓

Zalo OA

↓

Webhook

↓

AI Bridge

↓

Retriever

↓

Knowledge Base

↓

LLM

↓

Response Generator

↓

Messaging Gateway

↓

Citizen

---

# 2.8 Queue Architecture

Notification Queue

↓

Retry Queue

↓

Dead Letter Queue

↓

Audit Log

---

Queue giúp:

- Không mất dữ liệu
- Retry
- Scale Out
- Monitoring

---

# 2.9 Provider Pattern

Business Layer

↓

IZaloOAProvider

↓

ZaloOAProvider

↓

Zalo Official Account API

Business Layer KHÔNG gọi API trực tiếp.

---

# 2.10 Adapter Pattern

Interface

IZaloOAProvider

Triển khai

ZaloOAProvider

Sau này có thể thêm:

EmailProvider

SMSProvider

MobilePushProvider

Không ảnh hưởng Business Layer.

---

# 2.11 Dependency Rule

Business Layer

↓

Interfaces

↓

Infrastructure

Không cho phép:

Infrastructure

↓

Business

---

# 2.12 AI Bridge Architecture

Message

↓

Intent Detection

↓

Permission Check

↓

Retriever

↓

Knowledge Base

↓

LLM

↓

Answer

↓

Citation

↓

Gateway

---

# 2.13 Retry Strategy

Retry lần 1

30 giây

Retry lần 2

2 phút

Retry lần 3

10 phút

Retry lần 4

30 phút

Sau đó

Dead Letter Queue

---

# 2.14 Delivery Status

Pending

Queued

Sending

Delivered

Failed

Expired

Cancelled

---

# 2.15 Logging

Message Log

Webhook Log

AI Log

Notification Log

Delivery Log

Security Log

Audit Log

---

# 2.16 Monitoring

Theo dõi:

- Queue Length
- Delivery Success Rate
- AI Response Time
- API Response Time
- Webhook Errors
- Retry Count
- Failed Messages

---

# 2.17 Security Boundary

Mọi request phải đi qua:

Authentication

↓

Authorization

↓

Rate Limiting

↓

Validation

↓

Business

↓

Logging

---

# 2.18 Enterprise Design Principles

- API First
- Event Driven
- Queue Based
- Retry Safe
- Stateless Services
- Dependency Injection
- Provider Pattern
- Adapter Pattern
- SOLID
- Clean Architecture

---

# 2.19 Expected Deliverables

Sau Phase 2, hệ thống có kiến trúc đủ để:

✓ Tích hợp Zalo OA thật

✓ AI Assistant

✓ Notification Service

✓ Broadcast

✓ Queue

✓ Retry

✓ Logging

✓ Monitoring

✓ Production Deployment

---

# END OF PHASE 2
```
```md id="x7nq2p"
# ============================================================================
# PHASE 3 – NOTIFICATION CENTER ARCHITECTURE
# ============================================================================

---

# 3.1 Overview

Notification Center là hệ thống trung tâm chịu trách nhiệm quản lý toàn bộ việc gửi thông báo trong AnSinhSo.

Business Services KHÔNG gửi trực tiếp tới Zalo OA.

Mọi thông báo đều phải đi qua Notification Center.

---

# 3.2 Objectives

Notification Center phải hỗ trợ:

✓ Event Driven Notification

✓ Message Queue

✓ Scheduled Notification

✓ Broadcast

✓ Retry

✓ Delivery Tracking

✓ Audit Logging

✓ Multi Channel Ready

---

# 3.3 Enterprise Architecture

Business Event
        │
        ▼
Notification Center
        │
 ┌──────┼───────────────┐
 │      │               │
 ▼      ▼               ▼
Queue Template Engine Delivery Tracker
        │
        ▼
Channel Provider
        │
 ┌──────┼───────────────┐
 ▼      ▼               ▼
Zalo   Email          SMS
 OA   (Future)      (Future)

---

# 3.4 Business Events

Notification chỉ được sinh từ Business Event.

Ví dụ:

HouseholdCreated

CitizenApproved

CitizenRejected

PaymentApproved

PaymentCompleted

NewPolicyPublished

EmergencyNotice

DisasterWarning

CitizenFeedbackReceived

---

# 3.5 Event Flow

Business Service

↓

Domain Event

↓

Notification Center

↓

Queue

↓

Template Engine

↓

Provider

↓

Citizen

---

# 3.6 Notification Types

System Notification

Business Notification

Emergency Notification

Broadcast Notification

Reminder Notification

AI Notification

---

# 3.7 Message Priority

Critical

High

Normal

Low

Background

Queue sẽ xử lý theo mức ưu tiên.

---

# 3.8 Queue Design

Notification Queue

↓

Processing Queue

↓

Retry Queue

↓

Dead Letter Queue

↓

Archive

---

# 3.9 Notification Lifecycle

Created

↓

Queued

↓

Processing

↓

Sent

↓

Delivered

↓

Read (nếu nền tảng hỗ trợ)

↓

Archived

Nếu lỗi:

↓

Retry

↓

Dead Letter Queue

---

# 3.10 Template Engine

Template sử dụng Placeholder.

Ví dụ:

Xin chào {{CitizenName}}

Đợt trợ cấp {{PolicyName}}

đã được phê duyệt.

Số tiền:

{{Amount}}

Ngày chi trả:

{{PaymentDate}}

---

# 3.11 Template Categories

Payment

Policy

Emergency

Reminder

Appointment

Broadcast

AI Answer

Citizen Feedback

---

# 3.12 Channel Provider

Interface

INotificationProvider

Triển khai

ZaloProvider

Sau này mở rộng:

EmailProvider

SMSProvider

PushNotificationProvider

---

# 3.13 Retry Policy

Retry 1

30 giây

Retry 2

2 phút

Retry 3

10 phút

Retry 4

30 phút

Retry 5

1 giờ

Sau đó:

Dead Letter Queue

---

# 3.14 Delivery Tracking

Theo dõi:

Message ID

Receiver

Template

Channel

Created Time

Sent Time

Delivery Status

Retry Count

Error Message

---

# 3.15 Database Design

NotificationTemplates

Notifications

NotificationRecipients

NotificationDeliveryLogs

NotificationQueues

NotificationAttachments

---

# 3.16 API Contract

POST

/api/notifications/send

POST

/api/notifications/broadcast

GET

/api/notifications/history

GET

/api/notifications/status/{id}

POST

/api/notifications/retry/{id}

GET

/api/notifications/templates

POST

/api/notifications/templates

PUT

/api/notifications/templates/{id}

---

# 3.17 Security

Roles

Admin

LanhDao

CanBoXa

Permissions

Notification.Send

Notification.Broadcast

Notification.Template

Notification.View

Notification.Retry

JWT Required

Audit Required

---

# 3.18 Monitoring

Theo dõi:

Total Notifications

Success Rate

Failure Rate

Retry Count

Average Delivery Time

Queue Length

Template Usage

Top Errors

---

# 3.19 Performance Requirements

10.000 Notification

<5 phút

Queue Response

<500 ms

API Response

<2 giây

Retry Delay

<30 giây

---

# 3.20 Coding Tasks

Notification Center

Notification Queue

Template Engine

Provider Pattern

REST API

Logging

Swagger

Health Check

---

# 3.21 Testing Tasks

Unit Test

Template Engine

Retry

Priority Queue

Provider

Integration Test

Notification API

Queue

Delivery

Performance Test

10.000 Notifications

Concurrent Sending

---

# 3.22 Documentation

Cập nhật:

API_SPEC.md

DATABASE_DESIGN.md

AI_HANDOVER.md

PROJECT_PROGRESS.md

---

# 3.23 Enterprise Best Practices

Không cho phép Business Layer gọi trực tiếp Provider.

Notification Center phải là tầng trung gian duy nhất.

Provider phải được triển khai theo Interface.

Queue phải hỗ trợ Retry.

Template phải quản lý tập trung.

Delivery phải ghi Log.

Audit phải lưu toàn bộ lịch sử.

---

# 3.24 Expected Deliverables

Sau Phase 3, hệ thống có:

✓ Notification Center

✓ Queue

✓ Retry

✓ Template Engine

✓ Delivery Tracking

✓ Multi-channel Architecture

✓ Production Ready

# ============================================================================
# END OF PHASE 3
# ============================================================================
```
# ============================================================================

# PHASE 4 – AI INTEGRATION HUB ARCHITECTURE

# ============================================================================

---

# 4.1 Overview

AI Integration Hub là tầng trung gian chịu trách nhiệm quản lý toàn bộ việc tích hợp Trí tuệ nhân tạo trong hệ thống AnSinhSo.

Mọi yêu cầu AI phải đi qua AI Integration Hub.

Business Layer, Notification Center và Zalo OA KHÔNG được gọi trực tiếp đến nhà cung cấp AI.

---

# 4.2 Objectives

AI Hub phải hỗ trợ:

✓ AI Provider Management

✓ Prompt Management

✓ RAG Integration

✓ Knowledge Retrieval

✓ Conversation Context

✓ AI Logging

✓ Cost Tracking

✓ AI Monitoring

✓ Multi-Provider Ready

---

# 4.3 Enterprise Architecture

Citizen

↓

Zalo Official Account

↓

Messaging Gateway

↓

AI Integration Hub

↓

┌──────────────────────────────────────┐

│ Authentication                       │

│ Authorization                        │

│ Prompt Manager                       │

│ Context Builder                      │

│ Retriever                            │

│ Knowledge Base                       │

│ AI Provider Manager                  │

│ Response Formatter                   │

│ Citation Engine                      │

│ Conversation Manager                 │

│ AI Audit Log                         │

└──────────────────────────────────────┘

↓

AI Provider

↓

OpenAI

Gemini

Azure OpenAI

Local LLM (Future)

---

# 4.4 AI Provider Pattern

Interface

IAIProvider

Triển khai

OpenAIProvider

GeminiProvider

AzureOpenAIProvider

Future:

LocalLLMProvider

Business Layer chỉ làm việc với:

IAIProvider

---

# 4.5 AI Request Flow

User Request

↓

Permission Validation

↓

Conversation Context

↓

Prompt Builder

↓

Retriever

↓

Knowledge Base

↓

Prompt Assembly

↓

AI Provider

↓

Answer

↓

Citation

↓

Response Formatter

↓

Audit Log

↓

Citizen

---

# 4.6 Prompt Management

Prompt được quản lý tập trung.

Phân loại:

* Citizen Prompt
* Officer Prompt
* Leader Prompt
* AI Analytics Prompt
* GIS Prompt
* Report Prompt
* Zalo OA Prompt

Mỗi Prompt có:

* Version
* Status
* Created By
* Updated At
* Approval

---

# 4.7 Knowledge Retrieval

Nguồn dữ liệu:

* BUSINESS_RULES.md
* DATABASE_DESIGN.md
* API_SPEC.md
* USER_GUIDE.md
* FAQ.md
* Chính sách trợ cấp
* Văn bản pháp luật
* Dữ liệu nghiệp vụ được phân quyền

Retriever chỉ lấy dữ liệu phù hợp với quyền của người dùng.

---

# 4.8 Conversation Management

Mỗi phiên hội thoại lưu:

* SessionId
* UserId
* Role
* Timestamp
* Context
* Prompt Version
* AI Provider
* Citation
* Feedback

Hỗ trợ tiếp tục hội thoại nhưng không vượt phạm vi phân quyền.

---

# 4.9 Citation Engine

Mọi câu trả lời AI phải có khả năng hiển thị:

* Nguồn dữ liệu
* Tài liệu tham chiếu
* Phiên bản tài liệu
* Thời điểm truy xuất

Nếu không tìm thấy dữ liệu phù hợp:

AI phải trả lời:

"Tôi không tìm thấy thông tin phù hợp trong cơ sở tri thức của hệ thống."

Không được tự suy diễn.

---

# 4.10 AI Monitoring

Theo dõi:

* Tổng số yêu cầu
* Tỷ lệ thành công
* Thời gian phản hồi
* Token sử dụng
* Chi phí ước tính
* Nhà cung cấp AI
* Lỗi phổ biến

---

# 4.11 AI Security

Mọi yêu cầu phải trải qua:

Authentication

↓

Authorization

↓

Rate Limiting

↓

Prompt Validation

↓

Retriever

↓

AI Provider

↓

Response Filter

↓

Audit Log

---

# 4.12 Cost Management

Theo dõi:

* Token Input
* Token Output
* Cost / Request
* Cost / User
* Cost / Day
* Cost / Month

Thiết lập:

* Daily Limit
* Monthly Budget
* Alert Threshold

---

# 4.13 API Contract

POST

/api/ai/chat

POST

/api/ai/search

POST

/api/ai/analyze

GET

/api/ai/history

GET

/api/ai/providers

GET

/api/ai/prompts

POST

/api/ai/feedback

---

# 4.14 Database Design

Tables

* AIProviders
* AIPrompts
* AIConversations
* AIConversationMessages
* AIRequestLogs
* AIUsageStatistics
* AIKnowledgeIndexes
* AICitations

---

# 4.15 Coding Tasks

* AI Integration Hub
* Provider Manager
* Prompt Manager
* Citation Engine
* Conversation Manager
* AI REST API
* Logging
* Monitoring
* Health Check

---

# 4.16 Testing Tasks

Unit Test

* Provider Switching
* Prompt Builder
* Citation Engine
* Conversation Context

Integration Test

* AI API
* RAG Pipeline
* Authorization

Performance Test

* 1.000 Concurrent Requests
* Large Knowledge Base
* Provider Failover

---

# 4.17 Documentation

Cập nhật:

* AI_DEVELOPMENT_GUIDE.md
* API_SPEC.md
* PROJECT_PROGRESS.md
* AI_HANDOVER.md
* SECURITY_GUIDE.md

---

# 4.18 Enterprise Best Practices

* Không phụ thuộc vào một nhà cung cấp AI.
* Prompt phải được quản lý phiên bản.
* Mọi phản hồi phải có khả năng truy xuất nguồn.
* Không cho phép AI thay đổi dữ liệu nghiệp vụ.
* Ghi đầy đủ nhật ký để phục vụ kiểm toán và cải tiến.

---

# 4.19 Expected Deliverables

Sau Phase 4, hệ thống có:

✓ AI Integration Hub

✓ Multi-Provider Architecture

✓ Prompt Management

✓ Knowledge Retrieval

✓ Citation Engine

✓ Conversation Management

✓ Cost Monitoring

✓ Enterprise AI Governance

# ============================================================================

# END OF PHASE 4

# =========================================================================###
