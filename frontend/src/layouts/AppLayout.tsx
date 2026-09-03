import type { ReactNode } from 'react'
import './AppLayout.css'

type AppLayoutProps = {
  children: ReactNode
}

type IconName =
  | 'menu'
  | 'dashboard'
  | 'household'
  | 'citizen'
  | 'welfare'
  | 'payment'
  | 'map'
  | 'report'
  | 'category'
  | 'settings'
  | 'search'
  | 'bell'
  | 'chevron'

type IconProps = {
  name: IconName
  className?: string
}

function Icon({ name, className = '' }: IconProps) {
  const content = {
    menu: (
      <>
        <path d="M4 7h16" />
        <path d="M4 12h16" />
        <path d="M4 17h16" />
      </>
    ),
    dashboard: (
      <>
        <rect x="3" y="3" width="7" height="7" rx="1.5" />
        <rect x="14" y="3" width="7" height="7" rx="1.5" />
        <rect x="3" y="14" width="7" height="7" rx="1.5" />
        <rect x="14" y="14" width="7" height="7" rx="1.5" />
      </>
    ),
    household: (
      <>
        <path d="M3 11.5 12 4l9 7.5" />
        <path d="M5.5 10.5V20h13v-9.5" />
        <path d="M9.5 20v-6h5v6" />
      </>
    ),
    citizen: (
      <>
        <circle cx="12" cy="8" r="4" />
        <path d="M4.5 21c.6-4.1 3.2-6 7.5-6s6.9 1.9 7.5 6" />
      </>
    ),
    welfare: (
      <>
        <path d="M12 21s-7-4.4-7-10a4 4 0 0 1 7-2.8A4 4 0 0 1 19 11c0 5.6-7 10-7 10Z" />
        <path d="M9 12h6" />
        <path d="M12 9v6" />
      </>
    ),
    payment: (
      <>
        <rect x="3" y="5" width="18" height="14" rx="2" />
        <path d="M3 10h18" />
        <path d="M7 15h3" />
      </>
    ),
    map: (
      <>
        <path d="m3 6 6-3 6 3 6-3v15l-6 3-6-3-6 3Z" />
        <path d="M9 3v15" />
        <path d="M15 6v15" />
      </>
    ),
    report: (
      <>
        <path d="M5 20V11" />
        <path d="M10 20V5" />
        <path d="M15 20v-8" />
        <path d="M20 20V8" />
      </>
    ),
    category: (
      <>
        <path d="M4 7h16" />
        <path d="M5 7v12h14V7" />
        <path d="M9 11h6" />
      </>
    ),
    settings: (
      <>
        <circle cx="12" cy="12" r="3" />
        <path d="M19.4 15a1.7 1.7 0 0 0 .3 1.9l.1.1-2.8 2.8-.1-.1a1.7 1.7 0 0 0-1.9-.3 1.7 1.7 0 0 0-1 1.6V21h-4v-.1a1.7 1.7 0 0 0-1-1.6 1.7 1.7 0 0 0-1.9.3l-.1.1L4.2 17l.1-.1a1.7 1.7 0 0 0 .3-1.9A1.7 1.7 0 0 0 3 14H3v-4h.1a1.7 1.7 0 0 0 1.6-1 1.7 1.7 0 0 0-.3-1.9L4.2 7 7 4.2l.1.1a1.7 1.7 0 0 0 1.9.3A1.7 1.7 0 0 0 10 3V3h4v.1a1.7 1.7 0 0 0 1 1.6 1.7 1.7 0 0 0 1.9-.3l.1-.1L19.8 7l-.1.1a1.7 1.7 0 0 0-.3 1.9 1.7 1.7 0 0 0 1.6 1h.1v4H21a1.7 1.7 0 0 0-1.6 1Z" />
      </>
    ),
    search: (
      <>
        <circle cx="11" cy="11" r="6" />
        <path d="m16 16 4 4" />
      </>
    ),
    bell: (
      <>
        <path d="M18 8a6 6 0 0 0-12 0c0 7-3 7-3 9h18c0-2-3-2-3-9Z" />
        <path d="M10 21h4" />
      </>
    ),
    chevron: <path d="m8 10 4 4 4-4" />,
  }

  return (
    <svg
      className={className}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.9"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      {content[name]}
    </svg>
  )
}

function AppLayout({ children }: AppLayoutProps) {
  return (
    <div className="workspace">
      <header className="workspace-header">
        <div className="workspace-brand">
          <button className="menu-button" type="button" aria-label="Mở menu">
            <Icon name="menu" />
          </button>

          <div className="workspace-logo" aria-hidden="true">AS</div>

          <div className="workspace-brand-text">
            <span>CỔNG QUẢN TRỊ AN SINH</span>
            <strong>Xã Sông Lũy</strong>
          </div>
        </div>

        <div className="workspace-search">
          <Icon name="search" />
          <input type="search" placeholder="Tìm kiếm nhanh..." aria-label="Tìm kiếm nhanh" />
          <kbd>Ctrl + K</kbd>
        </div>

        <div className="workspace-user">
          <button className="notification-button" type="button" aria-label="Thông báo">
            <Icon name="bell" />
            <b>3</b>
          </button>

          <div className="user-avatar">QT</div>

          <div className="user-info">
            <strong>Quản trị viên</strong>
            <span>Administrator</span>
          </div>

          <button className="user-chevron" type="button" aria-label="Mở menu tài khoản">
            <Icon name="chevron" />
          </button>
        </div>
      </header>

      <div className="workspace-body">
        <aside className="app-sidebar">
          <nav className="sidebar-nav" aria-label="Điều hướng chính">
            <a className="sidebar-link active" href="/dashboard"><Icon name="dashboard" /><span>Tổng quan</span></a>
            <a className="sidebar-link" href="/households"><Icon name="household" /><span>Hộ gia đình</span></a>
            <a className="sidebar-link" href="/citizens"><Icon name="citizen" /><span>Người dân</span></a>
            <a className="sidebar-link" href="/welfare"><Icon name="welfare" /><span>An sinh xã hội</span></a>
            <span className="sidebar-link"><Icon name="payment" /><span>Chi trả trợ cấp</span></span>
            <span className="sidebar-link"><Icon name="map" /><span>Bản đồ số</span></span>
            <span className="sidebar-link"><Icon name="report" /><span>Báo cáo thống kê</span></span>
            <span className="sidebar-link"><Icon name="category" /><span>Danh mục</span></span>
            <span className="sidebar-link"><Icon name="settings" /><span>Hệ thống</span></span>
          </nav>

          <div className="sidebar-bottom">
            <div className="sidebar-status">
              <i />
              <div><span>Trạng thái hệ thống</span><strong>Đang hoạt động</strong></div>
            </div>
            <span className="sidebar-version">Phiên bản 1.0.0</span>
          </div>
        </aside>

        <main className="app-content">{children}</main>
      </div>
    </div>
  )
}

export default AppLayout
