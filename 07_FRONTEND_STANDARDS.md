# ============================================================================
# 07_FRONTEND_STANDARDS.md
# ============================================================================
# Project       : AnSinhSo - Hệ thống An Sinh Số xã Sông Lũy
# Document Type : Enterprise Frontend Standards
# Version       : 1.0.0
# Status        : FROZEN
# Owner         : Project Architecture Team
# Architecture  : Clean Architecture + Enterprise Architecture
# Frontend      : ASP.NET Core MVC / Razor / HTML5 / CSS3 / JavaScript ES2022
# UI Framework  : Bootstrap 5.x
# GIS           : Leaflet.js
# AI            : OpenAI / Gemini / AntiGravity AI
# Integration   : REST API / JWT / Zalo Official Account
# Last Updated  : 2026-07-10
# ============================================================================

# 07. FRONTEND STANDARDS

---

# 1. Purpose

Tài liệu này quy định toàn bộ tiêu chuẩn phát triển Frontend của dự án **AnSinhSo**.

Mục tiêu:

- Chuẩn hóa toàn bộ giao diện người dùng.
- Đảm bảo tất cả module có cùng kiến trúc.
- Giảm Technical Debt.
- Tăng khả năng bảo trì lâu dài.
- Hỗ trợ AI Coding sinh mã nguồn đồng nhất.
- Đảm bảo khả năng mở rộng nhiều năm mà không phải thay đổi kiến trúc.

Tài liệu này là tiêu chuẩn bắt buộc đối với:

- Developer
- AI Coding Assistant
- Reviewer
- Tester
- DevOps
- Technical Lead

---

# 2. Scope

Áp dụng cho toàn bộ Frontend của hệ thống.

Bao gồm:

- Login
- Dashboard
- Người dùng
- Phân quyền
- Địa bàn
- Công dân
- Hộ gia đình
- Chính sách
- Đợt chi trả
- GIS Map
- AI Assistant
- Zalo OA
- Báo cáo
- Thống kê
- Thiết lập hệ thống

Không áp dụng cho:

- Backend API
- Database
- Infrastructure
- DevOps Pipeline

Các nội dung trên được quy định trong các tài liệu tương ứng.

---

# 3. Objectives

Frontend phải đáp ứng các tiêu chí sau.

## 3.1 Enterprise Ready

Có thể sử dụng cho hệ thống cấp xã, huyện, tỉnh và mở rộng lên cấp quốc gia.

---

## 3.2 Production Ready

Có thể triển khai trực tiếp trên môi trường Production.

Không sử dụng:

- Demo Code
- Fake Data
- Hard Code
- Temporary Fix
- Sample Component

---

## 3.3 Maintainability

Mọi module phải:

- độc lập
- dễ đọc
- dễ mở rộng
- dễ bảo trì
- dễ thay thế

---

## 3.4 Reusability

Component phải được thiết kế để tái sử dụng.

Không copy cùng một đoạn HTML nhiều lần.

Ưu tiên:

- Partial View
- Shared Component
- Layout Component
- JavaScript Module

---

## 3.5 Scalability

Frontend phải có khả năng mở rộng:

- thêm module mới
- thêm dashboard mới
- thêm báo cáo
- thêm AI
- thêm GIS
- thêm Notification

mà không phải sửa kiến trúc cũ.

---

# 4. Audience

Đối tượng sử dụng tài liệu.

| Vai trò | Mục đích |
|----------|----------|
| Frontend Developer | Xây dựng giao diện |
| Backend Developer | Đồng bộ API |
| Tester | Kiểm thử giao diện |
| Technical Lead | Review |
| Solution Architect | Thiết kế |
| DevOps | Triển khai |
| AI Coding | Sinh mã nguồn |

---

# 5. Frontend Principles

Toàn bộ giao diện phải tuân thủ các nguyên tắc sau.

## Principle 01

Single Responsibility

Mỗi View chỉ thực hiện một chức năng.

---

## Principle 02

Separation of Concerns

Tách riêng:

- HTML
- CSS
- JavaScript
- API

Không viết JavaScript trực tiếp trong View nếu không thật sự cần thiết.

---

## Principle 03

No Business Logic

Frontend không xử lý nghiệp vụ.

Toàn bộ nghiệp vụ xử lý tại Backend.

Frontend chỉ:

- hiển thị dữ liệu
- gọi API
- validate cơ bản
- hiển thị thông báo

---

## Principle 04

Stateless UI

Không lưu dữ liệu nghiệp vụ trong Browser.

Nguồn dữ liệu duy nhất là Backend API.

---

## Principle 05

API First

Mọi dữ liệu phải lấy thông qua REST API.

Không truy cập trực tiếp Database.

---

## Principle 06

Security First

Không:

- hard-code token
- hard-code password
- hard-code URL Production
- hard-code Role

---

## Principle 07

Accessibility

Mọi màn hình phải hỗ trợ:

- Keyboard Navigation
- Screen Reader
- Focus State
- Contrast
- Responsive

---

## Principle 08

Consistency

Toàn bộ hệ thống phải có cùng:

- màu sắc
- font chữ
- spacing
- button
- icon
- animation
- dialog
- loading
- toast

---

# 6. Frontend Architecture

Frontend được tổ chức theo kiến trúc phân lớp.

```text
Presentation Layer
│
├── Layout
├── Pages
├── Components
├── Partial Views
│
Application Layer
│
├── API Client
├── Authentication
├── Authorization
├── State
├── Cache
│
Infrastructure Layer
│
├── REST API
├── JWT
├── GIS
├── AI
├── Zalo OA
```

Frontend không được phép gọi Database.

Frontend chỉ giao tiếp với Backend thông qua HTTP REST API.

---

# 7. High-Level Architecture

```text
Browser
      │
      ▼

Frontend (MVC / Razor)

      │

REST API

      │

ASP.NET Core Backend

      │

Application Layer

      │

Domain Layer

      │

Infrastructure

      │

SQL Server
```

Không được phép:

Browser

↓

Database

Hoặc

Browser

↓

Stored Procedure

Mọi truy cập dữ liệu phải đi qua Backend API.

---

# 8. Architectural Rules

Bắt buộc tuân thủ.

| Rule | Required |
|-------|----------|
| Responsive | Yes |
| Mobile First | Yes |
| API First | Yes |
| JWT Authentication | Yes |
| RBAC Authorization | Yes |
| Clean Code | Yes |
| Reusable Components | Yes |
| Lazy Loading | Yes |
| Error Boundary | Yes |
| Centralized API | Yes |
| Logging | Yes |
| Audit Friendly | Yes |

---

# 9. Definition of Frontend

Frontend trong dự án AnSinhSo bao gồm:

- User Interface
- User Experience
- Client-side Validation
- Authentication UI
- Authorization UI
- Dashboard
- Charts
- GIS
- AI Chat
- Notification
- Reports
- Search
- Filter
- Export
- Responsive Layout

Không bao gồm:

- Business Logic
- SQL
- Authentication Logic
- Payment Logic
- Security Logic
- Database Processing

---

# End of Phase 1

Phase tiếp theo:

- Folder Structure
- Naming Convention
- Coding Standards
# ============================================================================
# 10. FOLDER STRUCTURE
# ============================================================================

## 10.1 Objective

Frontend phải được tổ chức theo cấu trúc thư mục thống nhất nhằm:

- Dễ tìm kiếm.
- Dễ mở rộng.
- Dễ bảo trì.
- Giảm phụ thuộc giữa các module.
- Hỗ trợ AI Coding sinh mã chính xác.

Mọi dự án AnSinhSo phải sử dụng cùng một cấu trúc thư mục.

---

## 10.2 Standard Folder Structure

```text
src/

│
├── assets/
│   ├── css/
│   ├── js/
│   ├── images/
│   ├── fonts/
│   ├── icons/
│   └── vendor/
│
├── layouts/
│
├── pages/
│
├── components/
│
├── services/
│
├── api/
│
├── auth/
│
├── state/
│
├── utils/
│
├── constants/
│
├── config/
│
├── hooks/
│
├── middleware/
│
├── gis/
│
├── ai/
│
├── zalo/
│
├── dashboard/
│
├── reports/
│
├── shared/
│
└── tests/
```

Không được tạo thư mục ngoài chuẩn nếu chưa được Technical Lead phê duyệt.

---

## 10.3 Folder Responsibilities

| Folder | Responsibility |
|----------|----------------|
| assets | Static Resources |
| layouts | Layout Templates |
| pages | Business Screens |
| components | Reusable Components |
| api | REST API Client |
| services | Business Services |
| auth | Authentication |
| state | State Management |
| utils | Helper Functions |
| constants | Global Constants |
| config | Configuration |
| hooks | Custom Hooks |
| middleware | Client Middleware |
| dashboard | Dashboard Components |
| gis | GIS Components |
| ai | AI Chat Components |
| zalo | Zalo OA Components |
| reports | Report Pages |
| shared | Shared Libraries |
| tests | Frontend Testing |

---

## 10.4 Forbidden Folder Structure

Không được phép:

```text
Pages/

Pages2/

Pages_New/

Components_Copy/

Components_Final/

TestPage/

Old/

Backup/

New Folder/
```

Không lưu:

- File backup
- File tạm
- File copy
- File thử nghiệm

trong Source Code.

---

# ============================================================================
# 11. MODULE STRUCTURE
# ============================================================================

## 11.1 Module Principles

Mỗi module phải độc lập.

Không phụ thuộc trực tiếp vào module khác.

Mọi giao tiếp thông qua:

- API
- Shared Component
- Shared Service

---

## 11.2 Standard Module

Ví dụ module Person.

```text
Person/

│
├── PersonList
├── PersonDetail
├── PersonCreate
├── PersonEdit
├── PersonDelete
├── PersonSearch
├── PersonFilter
├── PersonService
├── PersonApi
└── index
```

Không được đặt toàn bộ logic trong một file.

---

## 11.3 Module Size

Một module không nên vượt quá:

- 20 components
- 20 API
- 20 Views

Nếu vượt:

=> phải tách module.

---

## 11.4 Module Isolation

Module A

không được gọi trực tiếp

Module B.

Ví dụ:

```text
Citizen

↓

Shared Component

↓

Household
```

Không được:

```text
Citizen

↓

Household

↓

Policy

↓

Payment
```

Chuỗi phụ thuộc nhiều tầng bị cấm.

---

## 11.5 Module Communication

Cho phép:

```text
UI

↓

API

↓

Backend
```

Không cho phép:

```text
UI

↓

UI

↓

UI
```

---

# ============================================================================
# 12. COMPONENT STRUCTURE
# ============================================================================

## 12.1 Component Principles

Component phải:

- độc lập
- tái sử dụng
- không chứa Business Logic
- dễ kiểm thử

---

## 12.2 Component Types

### Layout Components

Ví dụ:

- Header
- Sidebar
- Footer
- Navbar

---

### Business Components

Ví dụ:

- PersonCard
- HouseholdCard
- PaymentCard

---

### Shared Components

Ví dụ:

- Button
- Modal
- Table
- Pagination
- SearchBox
- Loading
- EmptyState

---

### Dashboard Components

Ví dụ:

- KPI Card
- Line Chart
- Pie Chart
- HeatMap

---

### GIS Components

Ví dụ:

- LeafletMap
- Marker
- Popup
- Cluster
- LayerControl

---

### AI Components

Ví dụ:

- AIChat
- PromptInput
- ChatHistory
- SuggestionCard

---

### Zalo Components

Ví dụ:

- NotificationCard
- OAStatus
- BroadcastPanel

---

## 12.3 Component Size

Một component không nên vượt:

- 300 dòng

Nếu vượt:

=> tách component.

---

## 12.4 Component Responsibilities

Một component chỉ thực hiện:

- một nhiệm vụ

Ví dụ:

PersonTable

Chỉ hiển thị bảng.

Không:

- gọi API
- validate
- export
- upload

---

## 12.5 Component Communication

Cho phép:

```text
Parent

↓

Child
```

Không:

```text
Child

↓

Parent

↓

Child
```

Không thao tác trực tiếp DOM của component khác.

---

## 12.6 Shared Components

Các thành phần sau bắt buộc dùng chung toàn hệ thống:

- Button
- Card
- Badge
- Alert
- Toast
- Modal
- Spinner
- Pagination
- Search Box
- Date Picker
- Confirm Dialog
- Empty State
- Loading Skeleton
- Breadcrumb
- Tabs
- Table
- Export Button

Không tạo lại nếu đã tồn tại.

---

## 12.7 Component Lifecycle

Component phải có đầy đủ:

- Initialization
- Loading
- Success
- Empty
- Error
- Dispose

Không để component ở trạng thái không xác định.

---

## 12.8 AI Coding Rules

AI khi sinh Component phải tuân thủ:

- Single Responsibility Principle
- Reusable
- Responsive
- Không Hard-code dữ liệu
- Không gọi SQL
- Không gọi Stored Procedure
- Chỉ gọi REST API
- Không chứa Business Logic
- Không duplicate code
- Có khả năng Unit Test

Đây là quy tắc bắt buộc đối với:

- AntiGravity AI
- GitHub Copilot
- Cursor
- Continue
- Cline
- ChatGPT
# ============================================================================
# 13. NAMING CONVENTIONS
# ============================================================================

## 13.1 Purpose

Toàn bộ Frontend phải sử dụng quy tắc đặt tên thống nhất nhằm:

- Tăng khả năng đọc hiểu.
- Giảm lỗi khi bảo trì.
- Hỗ trợ AI Coding.
- Đồng nhất giữa Frontend và Backend.
- Dễ tìm kiếm trong toàn bộ Solution.

---

## 13.2 General Principles

Tên phải:

- rõ nghĩa
- ngắn gọn
- không viết tắt khó hiểu
- phản ánh đúng chức năng

Không sử dụng:

```
abc
test
temp
demo
new
old
page1
button2
```

---

## 13.3 Language

Toàn bộ Source Code sử dụng:

English

Không sử dụng tiếng Việt trong:

- Variable
- Function
- Class
- Component
- CSS
- Folder
- File

Cho phép tiếng Việt chỉ trong:

- Nội dung hiển thị
- Resource
- Localization

---

## 13.4 File Naming

Sử dụng:

PascalCase

Ví dụ:

```
CitizenList.cshtml

CitizenDetail.cshtml

PaymentHistory.cshtml

DashboardIndex.cshtml

MapDashboard.cshtml
```

Không:

```
citizenlist

citizen_List

citizen-list

Citizen_List
```

---

## 13.5 Folder Naming

Sử dụng:

PascalCase

Ví dụ:

```
Citizen

Household

Dashboard

Shared

Components

Layouts
```

---

## 13.6 JavaScript Naming

File JavaScript:

camelCase hoặc kebab-case thống nhất toàn dự án.

Ví dụ:

```
citizenService.js

dashboardApi.js

mapHelper.js

notificationService.js
```

---

## 13.7 CSS Naming

CSS Class sử dụng:

BEM hoặc kebab-case.

Ví dụ:

```
dashboard-card

dashboard-card__title

dashboard-card--warning
```

Không:

```
redBox

style2

newCard

card123
```

---

## 13.8 Variable Naming

Sử dụng camelCase.

Ví dụ:

```
citizenName

householdCount

selectedPolicy

currentUser
```

Boolean phải bắt đầu bằng:

```
is

has

can

should
```

Ví dụ:

```
isAdmin

hasPermission

canEdit

shouldReload
```

---

## 13.9 Constant Naming

Sử dụng:

UPPER_SNAKE_CASE

Ví dụ:

```
API_TIMEOUT

DEFAULT_PAGE_SIZE

TOKEN_KEY

MAX_UPLOAD_SIZE
```

---

## 13.10 Function Naming

Function phải bắt đầu bằng động từ.

Ví dụ:

```
loadCitizen()

saveCitizen()

deleteCitizen()

searchCitizen()

exportExcel()

showLoading()
```

Không:

```
citizen()

button()

data()

abc()
```

---

## 13.11 Event Naming

Quy tắc:

```
onClick

onSubmit

onSave

onDelete

onSearch

onFilter
```

---

## 13.12 API Naming

REST Endpoint đồng nhất với Backend.

Ví dụ:

```
GET

/api/citizens

POST

/api/citizens

PUT

/api/citizens/{id}

DELETE

/api/citizens/{id}
```

Frontend không tự đặt URL khác Backend.

---

## 13.13 Component Naming

Component sử dụng PascalCase.

Ví dụ:

```
CitizenTable

CitizenCard

DashboardCard

SearchBox

Pagination

LoadingSpinner
```

---

## 13.14 ID Naming

HTML ID phải duy nhất.

Ví dụ:

```
txtCitizenName

ddlPolicy

btnSave

tblCitizen

dlgDelete
```

---

## 13.15 Data Attribute

Ưu tiên:

```
data-id

data-role

data-action

data-status
```

Không sử dụng attribute tự định nghĩa không có ý nghĩa.

---

# ============================================================================
# 14. HTML STANDARDS
# ============================================================================

## 14.1 HTML Version

Toàn bộ dự án sử dụng:

HTML5

---

## 14.2 Semantic HTML

Ưu tiên sử dụng:

```
header

main

section

article

aside

footer

nav

figure
```

Không lạm dụng:

```
div
```

---

## 14.3 Accessibility

Mọi input phải có:

```
label
```

Ví dụ:

```
<label for="txtCitizenName">

<input id="txtCitizenName">
```

---

## 14.4 Form Structure

Thứ tự chuẩn:

```
Label

Input

Validation Message

Help Text
```

---

## 14.5 Button

Button phải khai báo rõ:

```
type="button"

type="submit"

type="reset"
```

Không bỏ trống thuộc tính type.

---

## 14.6 Image Standards

Mọi hình ảnh phải có:

```
alt
```

Ví dụ:

```
<img
    src="/images/logo.png"
    alt="AnSinhSo Logo">
```

---

## 14.7 Table Standards

Table phải gồm:

```
thead

tbody

tfoot (nếu cần)
```

Không hiển thị dữ liệu trực tiếp ngoài tbody.

---

## 14.8 Form Validation

Validation Message hiển thị ngay dưới Control.

Không sử dụng Alert Browser.

---

## 14.9 DOM Rules

Không thao tác DOM trực tiếp khi có Component tương ứng.

Không:

```
document.write()

innerHTML = ...
```

Ưu tiên:

```
createElement()

append()

replaceChildren()
```

---

## 14.10 Forbidden HTML

Không sử dụng:

```
<font>

<center>

<marquee>

<blink>
```

Các thẻ đã lỗi thời hoàn toàn bị cấm.

---

# ============================================================================
# 15. CSS STANDARDS
# ============================================================================

## 15.1 CSS Principles

CSS phải:

- Module hóa.
- Có khả năng tái sử dụng.
- Không trùng lặp.
- Không ghi đè tùy tiện Bootstrap.

---

## 15.2 File Organization

```
base.css

layout.css

components.css

utilities.css

dashboard.css

gis.css

ai.css
```

---

## 15.3 Design Tokens

Toàn bộ màu sắc, spacing, typography phải khai báo thông qua Design Tokens.

Ví dụ:

```
--primary-color

--secondary-color

--border-radius

--spacing-md

--font-size-base
```

Không hard-code giá trị lặp lại trong nhiều file.

---

## 15.4 Responsive Breakpoints

Chuẩn Bootstrap 5:

```
xs

sm

md

lg

xl

xxl
```

Không tạo breakpoint tùy ý nếu không có phê duyệt kiến trúc.

---

## 15.5 Z-Index Policy

Sử dụng thang chuẩn:

```
Dropdown      : 1000
Sticky Header : 1020
Modal         : 1055
Toast         : 1080
Loading       : 1090
```

Không sử dụng các giá trị cực lớn như `999999`.

---

## 15.6 Animation

Chỉ dùng animation phục vụ trải nghiệm người dùng.

Thời gian khuyến nghị:

```
150ms
250ms
300ms
```

Không sử dụng animation gây ảnh hưởng hiệu năng hoặc làm phân tán sự chú ý.

---

## 15.7 Print Style

Các trang báo cáo phải hỗ trợ chế độ in.

Ẩn:

- Sidebar
- Menu
- Chat AI
- Notification

Hiển thị đầy đủ nội dung báo cáo và thông tin nhận diện hệ thống.

---

## 15.8 CSS Quality Checklist

Trước khi hoàn thành giao diện cần kiểm tra:

- Không trùng selector.
- Không dùng `!important` nếu không thật sự cần.
- Không có CSS không sử dụng.
- Responsive trên mọi breakpoint.
- Đảm bảo tương phản màu theo WCAG.
- Không làm thay đổi hành vi mặc định của Bootstrap ngoài phạm vi cho phép.
# ============================================================================
# 16. JAVASCRIPT STANDARDS
# ============================================================================

## 16.1 Purpose

Toàn bộ mã JavaScript trong dự án AnSinhSo phải thống nhất nhằm:

- Dễ đọc.
- Dễ bảo trì.
- Dễ kiểm thử.
- Hỗ trợ AI Coding.
- Đồng bộ với Clean Architecture.

JavaScript chỉ đảm nhiệm xử lý giao diện và giao tiếp với Backend API.

---

## 16.2 JavaScript Version

Toàn bộ dự án sử dụng:

ES2022

Không sử dụng cú pháp đã lỗi thời (Legacy JavaScript).

---

## 16.3 General Principles

JavaScript phải tuân thủ:

- Single Responsibility Principle
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple)
- SOLID (ở mức phù hợp Frontend)
- Không chứa Business Logic

---

## 16.4 Module Organization

Mỗi module gồm:

```text
citizenApi.js
citizenService.js
citizenValidation.js
citizenEvent.js
citizenPage.js
```

Không viết toàn bộ logic trong một file.

---

## 16.5 API Access

Toàn bộ request phải thông qua API Client.

Không được gọi trực tiếp:

```javascript
fetch(...)
```

ở nhiều nơi.

Ví dụ:

```
ApiClient.get()

ApiClient.post()

ApiClient.put()

ApiClient.delete()
```

---

## 16.6 Async Rules

Bắt buộc sử dụng:

```
async / await
```

Không lạm dụng Promise Chain.

Không sử dụng callback lồng nhau.

---

## 16.7 Error Handling

Mọi API phải có:

```
try

catch

finally
```

Không bỏ qua Exception.

---

## 16.8 DOM Manipulation

Ưu tiên:

- Event Delegation
- Query Selector
- Component Update

Không thao tác DOM lặp nhiều lần.

---

## 16.9 Global Variables

Không sử dụng Global Variable.

Cho phép:

```
const AppConfig

const ApiClient
```

Không tạo biến toàn cục ngoài phạm vi cần thiết.

---

## 16.10 Logging

Production:

Không sử dụng

```
console.log()

console.table()

console.dir()
```

Chỉ sử dụng Logger chuẩn nếu cần.

---

## 16.11 Code Quality

Một Function:

- dưới 50 dòng
- tối đa 3 mức lồng nhau
- tên rõ nghĩa
- không thực hiện nhiều nhiệm vụ

---

## 16.12 Security

Không:

- lưu Password
- lưu JWT vĩnh viễn
- lưu thông tin nhạy cảm vào Local Storage nếu không cần thiết
- hard-code API Key

---

# ============================================================================
# 17. RAZOR VIEW STANDARDS
# ============================================================================

## 17.1 Purpose

Chuẩn hóa Razor View trong ASP.NET Core MVC.

---

## 17.2 View Responsibilities

View chỉ thực hiện:

- Hiển thị dữ liệu
- Binding
- Render Component

Không xử lý nghiệp vụ.

---

## 17.3 ViewModel

Mỗi View sử dụng ViewModel riêng.

Không truyền Domain Entity trực tiếp lên giao diện.

---

## 17.4 Layout

Tất cả View phải sử dụng Layout thống nhất.

Ví dụ:

```
_Layout.cshtml
```

Không tạo nhiều Layout nếu không có nhu cầu rõ ràng.

---

## 17.5 Partial View

Các thành phần dùng lại phải tách thành Partial View.

Ví dụ:

```
_Header

_Footer

_Sidebar

_Breadcrumb

_SearchBox

_Pagination
```

---

## 17.6 Tag Helpers

Ưu tiên Tag Helper của ASP.NET Core.

Không lạm dụng HTML thuần nếu đã có Tag Helper tương ứng.

---

## 17.7 Validation

Validation Message hiển thị ngay dưới Control.

Không dùng Alert Browser.

---

## 17.8 Scripts

Không viết JavaScript dài trong View.

Sử dụng:

```
@section Scripts
```

để nạp file JavaScript riêng.

---

# ============================================================================
# 18. LAYOUT STANDARDS
# ============================================================================

## 18.1 Standard Layout

Layout chuẩn gồm:

```text
Header

Sidebar

Breadcrumb

Content

Footer

Toast

Modal

Loading Overlay
```

---

## 18.2 Header

Header hiển thị:

- Logo
- Tên hệ thống
- Người dùng đăng nhập
- Thông báo
- Đăng xuất

---

## 18.3 Sidebar

Sidebar phải hỗ trợ:

- Collapse
- Expand
- Active Menu
- Permission Menu

---

## 18.4 Breadcrumb

Mọi màn hình nghiệp vụ phải có Breadcrumb.

Ví dụ:

```
Trang chủ

>

Hộ gia đình

>

Chi tiết
```

---

## 18.5 Footer

Footer hiển thị:

- Phiên bản
- Copyright
- Đơn vị phát triển
- Năm phát hành

---

## 18.6 Loading Overlay

Loading phải thống nhất toàn hệ thống.

Không mỗi module một kiểu Loading.

---

## 18.7 Empty State

Khi không có dữ liệu phải hiển thị:

- Icon
- Tiêu đề
- Mô tả
- Nút thao tác

---

## 18.8 Error Screen

Có màn hình chuẩn cho:

- 401
- 403
- 404
- 500

---

# ============================================================================
# 19. RESPONSIVE DESIGN STANDARDS
# ============================================================================

## 19.1 Responsive Principles

Thiết kế theo:

Mobile First

Responsive First

---

## 19.2 Supported Devices

Bắt buộc hỗ trợ:

- Mobile
- Tablet
- Laptop
- Desktop
- Ultra Wide

---

## 19.3 Minimum Resolution

Thiết kế tối thiểu:

```
360px
```

---

## 19.4 Grid System

Sử dụng Bootstrap Grid.

Không tự xây dựng Grid mới nếu không cần.

---

## 19.5 Tables

Table lớn phải:

- Responsive
- Horizontal Scroll
- Sticky Header (khi cần)

---

## 19.6 Forms

Form phải tự co giãn.

Không đặt chiều rộng cố định.

---

## 19.7 Cards

Card hiển thị đẹp ở mọi kích thước màn hình.

---

## 19.8 Charts

Biểu đồ phải tự thay đổi kích thước theo Container.

---

## 19.9 GIS

Bản đồ phải:

- Responsive
- Zoom tốt trên Mobile
- Hỗ trợ Touch

---

## 19.10 AI Chat

AI Chat phải hỗ trợ:

- Desktop
- Tablet
- Mobile

Không che nội dung chính.

---

# ============================================================================
# 20. DESIGN SYSTEM
# ============================================================================

## 20.1 Design Philosophy

Thiết kế theo hướng:

- Government
- Enterprise
- Professional
- Clean
- Modern

---

## 20.2 Color Principles

Màu sắc phải:

- Nhất quán
- Dễ đọc
- Độ tương phản cao
- Thân thiện với người dùng

---

## 20.3 Primary Color

Ưu tiên:

Government Blue

---

## 20.4 Secondary Color

Dùng cho:

- Information
- Navigation
- Background

---

## 20.5 Status Colors

Chuẩn hóa:

Success

Warning

Danger

Info

Neutral

---

## 20.6 Typography

Font chính:

Inter

Fallback:

Segoe UI

Arial

sans-serif

---

## 20.7 Border Radius

Sử dụng thống nhất:

```
4px

8px

12px
```

---

## 20.8 Shadows

Chỉ sử dụng:

- Small
- Medium
- Large

Không tạo Shadow tùy ý.

---

## 20.9 Icons

Chuẩn:

Bootstrap Icons

Không trộn nhiều bộ Icon trong cùng hệ thống.

---

## 20.10 Spacing

Sử dụng Scale thống nhất:

```
4

8

12

16

24

32

48

64
```

Không đặt Margin/Padding ngẫu nhiên.

---

## 20.11 Buttons

Mọi Button dùng chung Component.

Các loại:

- Primary
- Secondary
- Success
- Danger
- Warning
- Outline
- Icon Button

---

## 20.12 Cards

Card phải có:

- Header
- Body
- Footer (nếu cần)

Không đặt quá nhiều thông tin trên một Card.

---

## 20.13 Forms

Tất cả Form phải thống nhất:

- Label
- Placeholder
- Validation
- Required Indicator
- Help Text

---

## 20.14 Tables

Bảng dữ liệu phải hỗ trợ:

- Sort
- Filter
- Search
- Pagination
- Export
- Responsive

---

## 20.15 Dialogs

Dialog chuẩn gồm:

- Title
- Content
- Action Buttons
- Close Button

---

## 20.16 Notifications

Thông báo sử dụng Toast chuẩn.

Không sử dụng alert() của trình duyệt.

---

## 20.17 Loading

Loading sử dụng Spinner hoặc Skeleton Screen.

Không để màn hình trắng khi chờ dữ liệu.

---

## 20.18 Design Review Checklist

Trước khi nghiệm thu giao diện cần kiểm tra:

- Đồng bộ màu sắc.
- Đồng bộ khoảng cách.
- Responsive đầy đủ.
- Hỗ trợ truy cập (Accessibility).
- Không Hard-code dữ liệu.
- Không vi phạm Naming Convention.
- Đúng Design System.
- Đúng Bootstrap 5.
- Đúng Clean Architecture.
- Đạt Definition of Done.
# ============================================================================
# 21. FORM STANDARDS
# ============================================================================

## 21.1 Purpose

Toàn bộ Form trong hệ thống AnSinhSo phải thống nhất về:

- Giao diện
- Hành vi
- Validation
- Trải nghiệm người dùng
- Khả năng mở rộng

---

## 21.2 Standard Form Layout

Mọi Form phải theo thứ tự:

```
Title

Description (Optional)

Input Controls

Validation Messages

Action Buttons
```

---

## 21.3 Input Controls

Cho phép:

- TextBox
- TextArea
- Number
- DatePicker
- DateTimePicker
- Dropdown
- MultiSelect
- Checkbox
- Radio
- Switch
- File Upload

Không tự tạo Control mới khi chưa có yêu cầu kiến trúc.

---

## 21.4 Required Fields

Trường bắt buộc phải có:

- Dấu *
- Validation
- Tooltip (nếu cần)

Ví dụ:

```
Họ và tên *
```

---

## 21.5 Readonly Fields

Readonly phải phân biệt rõ với Editable.

Không sử dụng Disabled nếu vẫn cần Submit dữ liệu.

---

## 21.6 Action Buttons

Thứ tự chuẩn:

```
Lưu

Lưu & Tiếp tục

Làm mới

Hủy
```

---

## 21.7 Form Submission

Khi Submit:

- Disable Button
- Hiển thị Loading
- Chống Submit nhiều lần
- Hiển thị kết quả

---

## 21.8 Unsaved Changes

Nếu người dùng rời Form khi chưa lưu:

Hiển thị Confirm Dialog.

---

# ============================================================================
# 22. VALIDATION STANDARDS
# ============================================================================

## 22.1 Validation Layers

Validation gồm:

- HTML Validation
- Client Validation
- API Validation
- Backend Validation

Frontend không thay thế Backend Validation.

---

## 22.2 Validation Rules

Mọi Input phải kiểm tra:

- Required
- Max Length
- Min Length
- Format
- Range
- Special Characters (nếu cần)

---

## 22.3 Validation Messages

Thông báo:

- Ngắn gọn
- Dễ hiểu
- Theo tiếng Việt chuẩn

Ví dụ:

```
CCCD không hợp lệ.

Ngày sinh không được lớn hơn ngày hiện tại.

Số điện thoại đã tồn tại.
```

---

## 22.4 Validation Display

Hiển thị ngay dưới Control.

Không dùng Alert Browser.

---

## 22.5 Server Validation

Nếu Backend trả lỗi:

Frontend phải hiển thị đúng thông báo từ API.

Không thay đổi nội dung lỗi nghiệp vụ.

---

# ============================================================================
# 23. DASHBOARD STANDARDS
# ============================================================================

## 23.1 Dashboard Principles

Dashboard phải:

- Nhanh
- Trực quan
- Ít thao tác
- Có khả năng mở rộng

---

## 23.2 Standard Layout

Dashboard gồm:

```
Header

Filter

KPI Cards

Charts

Map

Recent Activities

Quick Actions
```

---

## 23.3 KPI Cards

Hiển thị:

- Tổng hộ
- Tổng đối tượng
- Tổng chi trả
- Hồ sơ chờ xử lý
- Phản ánh mới

---

## 23.4 Refresh

Dashboard hỗ trợ:

- Manual Refresh
- Auto Refresh (tùy cấu hình)

---

## 23.5 Drill Down

KPI phải có khả năng mở màn hình chi tiết.

---

# ============================================================================
# 24. CHART STANDARDS
# ============================================================================

## 24.1 Supported Charts

Cho phép:

- Bar
- Line
- Pie
- Doughnut
- Area
- Heatmap
- Timeline

---

## 24.2 Chart Rules

Mỗi Chart phải có:

- Title
- Legend
- Tooltip
- Empty State
- Loading

---

## 24.3 Responsive

Chart tự thay đổi theo kích thước màn hình.

---

## 24.4 Export

Cho phép xuất:

- PNG
- PDF
- Excel (nếu phù hợp)

---

# ============================================================================
# 25. GIS UI STANDARDS
# ============================================================================

## 25.1 GIS Principles

Bản đồ là thành phần cốt lõi của AnSinhSo.

Phải đảm bảo:

- Hiệu năng
- Dễ sử dụng
- Chính xác
- Trực quan

---

## 25.2 GIS Components

Bao gồm:

- Base Map
- Marker
- Cluster
- Popup
- Layer Control
- Search
- Legend

---

## 25.3 Marker Colors

Màu Marker phải thống nhất theo trạng thái nghiệp vụ.

Ví dụ:

- Xanh: Bình thường
- Vàng: Cần theo dõi
- Đỏ: Khẩn cấp
- Xám: Ngừng hoạt động

---

## 25.4 Popup

Popup hiển thị:

- Thông tin hộ
- Đối tượng
- Chính sách
- Liên kết chi tiết

---

## 25.5 GIS Performance

Không tải toàn bộ Marker cùng lúc.

Bắt buộc:

- Lazy Loading
- Marker Cluster
- Tile Cache

---

# ============================================================================
# 26. AI ASSISTANT UI STANDARDS
# ============================================================================

## 26.1 AI Principles

AI chỉ hỗ trợ.

Không thay thế quyết định của cán bộ.

---

## 26.2 AI Components

Bao gồm:

- Chat Window
- Prompt Input
- Suggested Questions
- Conversation History

---

## 26.3 AI Responses

Mọi phản hồi AI phải:

- Có thời gian
- Có trạng thái xử lý
- Có khả năng sao chép
- Có khả năng phản hồi

---

## 26.4 Disclaimer

Luôn hiển thị thông báo:

"Kết quả AI chỉ mang tính hỗ trợ."

---

# ============================================================================
# 27. ZALO OA UI STANDARDS
# ============================================================================

## 27.1 OA Dashboard

Hiển thị:

- Trạng thái OA
- Token
- Số người theo dõi
- Tin nhắn đã gửi
- Tin nhắn lỗi

---

## 27.2 Broadcast

Hiển thị:

- Tiến trình gửi
- Thành công
- Thất bại
- Retry

---

## 27.3 Notification History

Cho phép:

- Tìm kiếm
- Lọc
- Xem chi tiết
- Gửi lại

---

# ============================================================================
# 28. REPORT UI STANDARDS
# ============================================================================

## 28.1 Reports

Báo cáo phải:

- Có Filter
- Có Preview
- Có Export
- Có Print

---

## 28.2 Report Layout

Gồm:

- Tiêu đề
- Điều kiện lọc
- Nội dung
- Footer

---

## 28.3 Export Formats

Chuẩn hỗ trợ:

- PDF
- Excel
- CSV

---

# ============================================================================
# 29. SEARCH & FILTER STANDARDS
# ============================================================================

## 29.1 Search

Tìm kiếm phải hỗ trợ:

- Từ khóa
- Nhiều trường
- Không phân biệt hoa thường

---

## 29.2 Filter

Filter phải:

- Có Reset
- Có Apply
- Có Clear All

---

## 29.3 Pagination

Mặc định:

20 dòng/trang.

Cho phép:

- 20
- 50
- 100

---

# ============================================================================
# 30. EXPORT STANDARDS
# ============================================================================

## 30.1 Export Formats

Cho phép:

- Excel
- PDF
- CSV

---

## 30.2 Export Rules

Tên file:

```
Module_yyyyMMdd_HHmmss
```

Ví dụ:

```
Citizen_20260712_143000.xlsx
```

---

## 30.3 Export Security

Chỉ người có quyền mới được Export.

Export phải ghi Audit Log.

Không xuất dữ liệu vượt phạm vi phân quyền.

---

# End of Phase 2D
# ============================================================================
# 31. AUTHENTICATION UI STANDARDS
# ============================================================================

## 31.1 Purpose

Giao diện xác thực phải:

- Đơn giản
- Bảo mật
- Dễ sử dụng
- Đồng bộ với JWT Authentication của Backend

---

## 31.2 Login Screen

Màn hình đăng nhập bao gồm:

- Logo hệ thống
- Tên hệ thống
- Tài khoản
- Mật khẩu
- Hiện/Ẩn mật khẩu
- Ghi nhớ đăng nhập (nếu được cấu hình)
- Nút Đăng nhập
- Thông báo lỗi

---

## 31.3 Session Expiration

Khi JWT hết hạn:

- Hiển thị thông báo
- Chuyển về màn hình đăng nhập
- Không mất dữ liệu chưa lưu (nếu có thể khôi phục)

---

## 31.4 Logout

Đăng xuất phải:

- Xóa Token
- Xóa Session Client
- Chuyển về Login
- Không lưu Cache nhạy cảm

---

# ============================================================================
# 32. AUTHORIZATION UI STANDARDS
# ============================================================================

## 32.1 RBAC

Frontend chỉ hiển thị chức năng phù hợp với Role.

Ví dụ:

- Admin
- Lãnh đạo
- Cán bộ xã
- Người dân

---

## 32.2 Permission Rendering

Không hiển thị Menu mà người dùng không có quyền truy cập.

Không chỉ Disable Menu.

---

## 32.3 Route Protection

Mọi màn hình phải kiểm tra quyền trước khi hiển thị.

Nếu không đủ quyền:

Hiển thị trang 403.

---

# ============================================================================
# 33. ERROR HANDLING UI STANDARDS
# ============================================================================

## 33.1 Error Categories

Chuẩn hóa các nhóm lỗi:

- Validation
- Business
- Authentication
- Authorization
- Network
- Server
- Unknown

---

## 33.2 HTTP Status Pages

Chuẩn giao diện cho:

- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 409 Conflict
- 500 Internal Server Error

---

## 33.3 Error Messages

Thông báo lỗi phải:

- Rõ ràng
- Không lộ thông tin hệ thống
- Có hướng dẫn xử lý nếu phù hợp

---

# ============================================================================
# 34. LOADING STANDARDS
# ============================================================================

## 34.1 Loading Types

Cho phép:

- Spinner
- Skeleton Screen
- Progress Bar

---

## 34.2 Global Loading

Toàn bộ request bất đồng bộ phải hỗ trợ trạng thái Loading.

---

## 34.3 Timeout

Nếu vượt thời gian cấu hình:

- Dừng Loading
- Thông báo lỗi
- Cho phép thử lại

---

# ============================================================================
# 35. TOAST & NOTIFICATION STANDARDS
# ============================================================================

## 35.1 Toast Types

Chuẩn hóa:

- Success
- Information
- Warning
- Error

---

## 35.2 Notification Rules

Thông báo phải:

- Không che nội dung chính
- Tự ẩn sau thời gian cấu hình (trừ lỗi nghiêm trọng)
- Có thể đóng thủ công

---

# ============================================================================
# 36. FRONTEND PERFORMANCE STANDARDS
# ============================================================================

## 36.1 Performance Objectives

Frontend phải:

- Khởi tạo nhanh
- Phản hồi nhanh
- Giảm số lượng HTTP Request
- Giảm kích thước tài nguyên tải xuống

---

## 36.2 Optimization

Áp dụng:

- Lazy Loading
- Minification
- Bundling
- Image Optimization
- Browser Cache

---

## 36.3 DOM Performance

Không render dữ liệu quá lớn trong một lần.

Ưu tiên:

- Pagination
- Infinite Scroll (khi phù hợp)
- Virtualization (đối với danh sách lớn)

---

# ============================================================================
# 37. FRONTEND SECURITY STANDARDS
# ============================================================================

## 37.1 Security Principles

Frontend không phải nơi thực thi bảo mật nghiệp vụ.

Mọi quyết định bảo mật thuộc Backend.

---

## 37.2 Client Storage

Không lưu:

- Password
- Refresh Token dạng rõ
- API Secret
- Thông tin nhạy cảm

---

## 37.3 XSS Prevention

Không render HTML không kiểm soát.

Ưu tiên:

- Encode Output
- Sanitize Input
- CSP (Content Security Policy)

---

## 37.4 CSRF

Mọi biểu mẫu và yêu cầu thay đổi dữ liệu phải tuân thủ cơ chế chống CSRF theo kiến trúc Backend.

---

# ============================================================================
# 38. BROWSER COMPATIBILITY
# ============================================================================

## 38.1 Supported Browsers

Hỗ trợ:

- Google Chrome (2 phiên bản ổn định gần nhất)
- Microsoft Edge (2 phiên bản ổn định gần nhất)
- Mozilla Firefox (2 phiên bản ổn định gần nhất)
- Safari (phiên bản được hỗ trợ trên macOS/iOS)

Không hỗ trợ Internet Explorer.

---

## 38.2 Responsive Devices

Kiểm thử tối thiểu trên:

- Mobile
- Tablet
- Laptop
- Desktop

---

# ============================================================================
# 39. ACCESSIBILITY STANDARDS
# ============================================================================

## 39.1 Standard

Tuân thủ:

WCAG 2.1 Level AA

---

## 39.2 Accessibility Requirements

Bao gồm:

- Keyboard Navigation
- Screen Reader
- Focus Indicator
- Color Contrast
- Alt Text
- ARIA Attributes (khi cần)

---

## 39.3 Accessibility Testing

Kiểm tra trước khi phát hành:

- Điều hướng bằng bàn phím
- Độ tương phản
- Nội dung thay thế cho hình ảnh
- Khả năng sử dụng trên thiết bị hỗ trợ

---

# ============================================================================
# 40. LOGGING & MONITORING
# ============================================================================

## 40.1 Client Logging

Frontend chỉ ghi các sự kiện cần thiết.

Không ghi dữ liệu cá nhân hoặc thông tin nhạy cảm.

---

## 40.2 Monitoring

Theo dõi:

- JavaScript Error
- API Failure
- Performance Metrics
- User Interaction (theo chính sách bảo mật)

---

# ============================================================================
# 41. FRONTEND TESTING STANDARDS
# ============================================================================

## 41.1 Testing Scope

Bao gồm:

- UI Testing
- Responsive Testing
- Accessibility Testing
- Integration Testing
- End-to-End Testing

---

## 41.2 Acceptance Criteria

Một chức năng chỉ được nghiệm thu khi:

- Đúng nghiệp vụ
- Đúng giao diện
- Responsive
- Không có lỗi JavaScript
- Không vi phạm Security Standards

---

# ============================================================================
# 42. DEFINITION OF DONE
# ============================================================================

Một chức năng Frontend được xem là hoàn thành khi:

- Hoàn thành theo yêu cầu nghiệp vụ
- Tuân thủ Frontend Standards
- Responsive đầy đủ
- Không Hard-code
- Không chứa Business Logic
- Gọi API đúng chuẩn
- Được kiểm thử
- Được Code Review
- Đạt Security Checklist
- Đạt Performance Checklist

---

# ============================================================================
# 43. FRONTEND CODE REVIEW CHECKLIST
# ============================================================================

Reviewer phải kiểm tra:

- Naming Convention
- Component Structure
- Reusability
- Responsive
- Accessibility
- Security
- Performance
- Error Handling
- API Integration
- Clean Code
- Không có mã chết (Dead Code)
- Không có ghi chú tạm thời (TODO/FIXME) trước khi phát hành

---

# ============================================================================
# 44. AI CODING RULES (ENTERPRISE)
# ============================================================================

Mọi AI Coding Assistant phải:

- Đọc Bootstrap trước khi sinh mã
- Đọc Progress để xác định Sprint
- Tuân thủ Clean Architecture
- Không thay đổi kiến trúc đã thống nhất
- Không sinh Demo Code trong Production
- Không Hard-code dữ liệu
- Không sinh mã trùng lặp
- Ưu tiên Component tái sử dụng
- Tuân thủ toàn bộ Frontend Standards
- Tự kiểm tra Definition of Done trước khi hoàn thành

Áp dụng cho:

- ChatGPT
- GitHub Copilot
- Cursor
- Cline
- Continue
- Gemini
- AntiGravity AI

---

# ============================================================================
# 45. CHANGE LOG
# ============================================================================

| Version | Date | Description |
|----------|------|-------------|
| 1.0.0 | 2026-07-10 | Initial Enterprise Frontend Standards |

---

# ============================================================================
# 46. RELATED DOCUMENTS
# ============================================================================

- 00_PROJECT_BOOTSTRAP.md
- 00_PROJECT_PROGRESS.md
- 00_PROJECT_INDEX.md
- 04_CODING_STANDARDS.md
- 05_DATABASE_RULES.md
- 06_API_STANDARDS.md
- 08_BACKEND_STANDARDS.md
- 09_SECURITY_STANDARDS.md
- CLEAN_ARCHITECTURE.md
- UI_REQUIREMENTS.md
- DESIGN_SYSTEM.md
- COMPONENT_LIBRARY.md

---

# END OF DOCUMENT