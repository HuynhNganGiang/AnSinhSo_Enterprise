# PROJECT_CONTEXT.md
Version: 1.0
Project: AnSinhSo Enterprise Edition
Status: Production Architecture
Author: Huỳnh Ngân Giang

---

# 1. Tổng quan

AnSinhSo là hệ thống quản lý An sinh xã hội cấp xã được xây dựng dành cho UBND xã Sông Lũy.

Mục tiêu của hệ thống là số hóa toàn bộ dữ liệu an sinh xã hội, thay thế việc quản lý Excel truyền thống.

Hệ thống hướng tới:

- Quản lý hộ gia đình
- Quản lý đối tượng an sinh
- Quản lý chi trả
- GIS
- AI Assistant
- Zalo Official Account
- Dashboard lãnh đạo

---

# 2. Kiến trúc

Architecture

Clean Architecture

Presentation

↓

API

↓

Application

↓

Domain

↓

Infrastructure

↓

SQL Server

---

# 3. Công nghệ

ASP.NET Core 8

Entity Framework Core

SQL Server 2022

JWT Authentication

Swagger

FluentValidation

Serilog

Leaflet GIS

SignalR

Zalo Official Account API

OpenAI API

Redis Cache

Background Service

Hangfire

Docker Ready

IIS Ready

---

# 4. Database

Database Name

AnSinhSoDB

Chuẩn hóa

3NF

Soft Delete

Có

Audit Log

Có

RBAC

Dynamic

Temporal Data

Có

GIS

Geography

Hierarchy

HierarchyID

---

# 5. Các Module

Authentication

Dashboard

Người dùng

Role

Permission

Địa bàn

Hộ gia đình

Công dân

Nhóm đối tượng

Chính sách

Chi trả

Bản đồ số

AI Assistant

Zalo OA

Báo cáo

Thống kê

Import Excel

Export Excel

Audit Log

System Setting

---

# 6. Người sử dụng

Admin

Cán bộ xã

Lãnh đạo

Người dân

---

# 7. Mục tiêu chất lượng

Code First Enterprise

Không duplicate

SOLID

DRY

KISS

Clean Code

Clean Architecture

DDD Ready

CQRS Ready

Repository Pattern

Unit Of Work

Dependency Injection

Async Await

REST API

OpenAPI

Swagger

---

# 8. Tiêu chuẩn

Enterprise

Production Ready

Maintainable

Scalable

Secure

High Performance

Cloud Ready

Docker Ready

CI/CD Ready

---

# 9. Quy tắc

Mọi thay đổi phải tương thích toàn bộ hệ thống.

Không sửa phá vỡ Database.

Không sinh code demo.

Không sinh code giả.

Chỉ sinh code Production Ready.

---

# 10. Kết quả cuối cùng

Hệ thống hoàn chỉnh có thể triển khai thực tế tại UBND xã Sông Lũy.