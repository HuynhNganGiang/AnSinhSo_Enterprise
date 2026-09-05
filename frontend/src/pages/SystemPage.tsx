import UserAccountsPanel from '../components/system/UserAccountsPanel'
import RolePermissionsPanel from '../components/system/RolePermissionsPanel'
import AuditLogsPanel from '../components/system/AuditLogsPanel'
import SystemConfigurationPanel from '../components/system/SystemConfigurationPanel'
import BackupRestorePanel from '../components/system/BackupRestorePanel'
import { useState } from 'react'
import AppLayout from '../layouts/AppLayout'
import './AdminManagementPages.css'

type SystemIcon =
  | 'users'
  | 'shield'
  | 'log'
  | 'config'
  | 'backup'
  | 'integration'

function SystemSvg({ name }: { name: SystemIcon }) {
  const icons = {
    users: (
      <>
        <circle cx="9" cy="8" r="3" />
        <circle cx="17" cy="9" r="2" />
        <path d="M3 20c.5-4 2.6-6 6.1-6 3.4 0 5.5 2 6 6" />
        <path d="M15 15c3 .1 4.8 1.7 5.4 5" />
      </>
    ),
    shield: (
      <>
        <path d="M12 3 20 6v5c0 5-3.5 8.5-8 10-4.5-1.5-8-5-8-10V6Z" />
        <path d="m9 12 2 2 4-5" />
      </>
    ),
    log: (
      <>
        <rect x="4" y="3" width="16" height="18" rx="2" />
        <path d="M8 8h8M8 12h8M8 16h5" />
      </>
    ),
    config: (
      <>
        <circle cx="12" cy="12" r="3" />
        <path d="M19 12a7 7 0 0 0-.1-1l2-1.5-2-3.4-2.5 1a7 7 0 0 0-1.7-1L14.4 3h-4.8l-.4 3.1a7 7 0 0 0-1.7 1l-2.5-1-2 3.4L5 11a7 7 0 0 0 0 2l-2 1.5 2 3.4 2.5-1a7 7 0 0 0 1.7 1l.4 3.1h4.8l.4-3.1a7 7 0 0 0 1.7-1l2.5 1 2-3.4L19 13a7 7 0 0 0 .1-1Z" />
      </>
    ),
    backup: (
      <>
        <path d="M5 7a8 8 0 1 1-1 8" />
        <path d="M5 3v4H1M12 7v5l3 2" />
      </>
    ),
    integration: (
      <>
        <path d="M8 12a4 4 0 0 1 4-4h4" />
        <path d="m14 5 3 3-3 3" />
        <path d="M16 12a4 4 0 0 1-4 4H8" />
        <path d="m10 19-3-3 3-3" />
      </>
    ),
  }

  return (
    <svg
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.9"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      {icons[name]}
    </svg>
  )
}

const systemCards = [
  {
    name: 'Người dùng & tài khoản',
    description:
      'Quản lý tài khoản quản trị, cán bộ xã, lãnh đạo và người dùng hệ thống.',
    icon: 'users' as const,
    tone: 'blue',
  },
  {
    name: 'Vai trò & phân quyền',
    description:
      'Cấu hình role, permission và policy kiểm soát truy cập các chức năng nghiệp vụ.',
    icon: 'shield' as const,
    tone: 'green',
  },
  {
    name: 'Nhật ký hệ thống',
    description:
      'Theo dõi hoạt động đăng nhập, thao tác quản trị và các sự kiện cần truy vết.',
    icon: 'log' as const,
    tone: 'violet',
  },
  {
    name: 'Cấu hình hệ thống',
    description:
      'Quản lý các tham số vận hành, thiết lập giao diện và cấu hình ứng dụng.',
    icon: 'config' as const,
    tone: 'orange',
  },
  {
    name: 'Sao lưu & phục hồi',
    description:
      'Khu vực chuẩn bị cho chức năng sao lưu, phục hồi và kiểm tra dữ liệu hệ thống.',
    icon: 'backup' as const,
    tone: 'cyan',
  },
  {
    name: 'Tích hợp dịch vụ',
    description:
      'Theo dõi cấu hình kết nối API, Zalo OA và các dịch vụ tích hợp bên ngoài.',
    icon: 'integration' as const,
    tone: 'slate',
  },
]

function SystemPage() {
  const [
    selectedSystem,
    setSelectedSystem,
  ] = useState<'users' | 'roles' | 'logs' | 'config' | 'backup'>('users')

  return (
    <AppLayout>
      <div className="admin-management-page">
        <section className="admin-page-heading">
          <div>
            <div className="admin-page-eyebrow">
              <SystemSvg name="config" />
              <span>QUẢN TRỊ HỆ THỐNG</span>
            </div>

            <h1>Hệ thống</h1>

            <p>
              Trung tâm cấu hình, bảo mật và giám sát vận hành
              hệ thống AnSinhSo.
            </p>
          </div>

          <div className="admin-heading-badge">
            <i />
            Hệ thống đang hoạt động
          </div>
        </section>

        <section className="system-summary-grid">
          <article className="system-summary-card">
            <span>Ứng dụng</span>
            <strong>AnSinhSo Enterprise</strong>
            <small>Cổng quản trị an sinh số</small>
          </article>

          <article className="system-summary-card">
            <span>Phiên bản giao diện</span>
            <strong>1.0.0</strong>
            <small>Frontend quản trị</small>
          </article>

          <article className="system-summary-card">
            <span>Kiến trúc</span>
            <strong>Web API</strong>
            <small>Frontend + ASP.NET Core API</small>
          </article>

          <article className="system-summary-card">
            <span>Trạng thái</span>
            <strong>Đang hoạt động</strong>
            <small>Môi trường demo phát triển</small>
          </article>
        </section>

        <section className="admin-notice">
          <span className="admin-notice-icon">i</span>

          <div>
            <strong>Quản trị hệ thống</strong>
            <p>
              Giao diện các chức năng quản trị đã được chuẩn bị.
              Các thao tác thay đổi tài khoản, quyền, cấu hình
              và dữ liệu sẽ chỉ được kích hoạt khi Backend/API
              tương ứng được kết nối.
            </p>
          </div>
        </section>

        <section className="admin-card-grid">
          {systemCards.map((item) => (
            <article
              className="admin-feature-card"
              key={item.name}
            >
              <div className="admin-feature-top">
                <span
                  className={`admin-feature-icon ${item.tone}`}
                >
                  <SystemSvg name={item.icon} />
                </span>

                <span className="admin-status-tag">
                  QUẢN TRỊ
                </span>
              </div>

              <h2>{item.name}</h2>
              <p>{item.description}</p>

              <div className="admin-feature-footer">
                <span>
                  {item.icon === 'users' ||
                item.icon === 'shield' ||
                item.icon === 'log' ||
                item.icon === 'config' ||
                item.icon === 'backup'
                    ? 'Đang hoạt động'
                    : 'Chế độ an toàn'}
                </span>

                <button
                  type="button"
                  onClick={() => {
                    if (
                      item.icon ===
                      'backup'
                    ) {
                      setSelectedSystem('backup')
                      return
                    }

                    if (
                      item.icon ===
                      'config'
                    ) {
                      setSelectedSystem('config')
                      return
                    }

                    if (
                      item.icon ===
                      'log'
                    ) {
                      setSelectedSystem('logs')
                      return
                    }
                    if (
                      item.name ===
                      'Người dùng & tài khoản'
                    ) {
                      setSelectedSystem('users')
                    } else if (
                      item.name ===
                      'Vai trò & phân quyền'
                    ) {
                      setSelectedSystem('roles')
                    }
                  }}
                >
                  {item.icon === 'users' ||
                item.icon === 'shield' ||
                item.icon === 'log' ||
                item.icon === 'config' ||
                item.icon === 'backup'
                    ? 'Mở quản lý →'
                    : 'Chờ tích hợp →'}
                </button>
              </div>
            </article>
          ))}
        </section>

        {selectedSystem ===
        'users' && (
          <UserAccountsPanel />
        )}

        {selectedSystem ===
        'roles' && (
          <RolePermissionsPanel />
        )}

        {selectedSystem ===
        'logs' && (
          <AuditLogsPanel />
        )}

        {selectedSystem ===
        'config' && (
          <SystemConfigurationPanel />
        )}

        {selectedSystem ===
        'backup' && (
          <BackupRestorePanel />
        )}

        <section className="admin-panel">
          <div className="admin-panel-heading">
            <div>
              <h2>Thông tin cấu hình hiện tại</h2>
              <p>
                Thông tin mô tả lớp giao diện, không thay đổi
                cấu hình Backend.
              </p>
            </div>

            <span className="admin-panel-badge">
              READ ONLY
            </span>
          </div>

          <div style={{ overflowX: 'auto' }}>
            <table className="system-config-table">
              <thead>
                <tr>
                  <th>Thành phần</th>
                  <th>Trạng thái</th>
                  <th>Phạm vi</th>
                  <th>Ghi chú</th>
                </tr>
              </thead>

              <tbody>
                <tr>
                  <td><strong>Frontend</strong></td>
                  <td>
                    <span className="system-config-value">
                      Hoạt động
                    </span>
                  </td>
                  <td>React / Vite</td>
                  <td>Giao diện quản trị</td>
                </tr>

                <tr>
                  <td><strong>Backend API</strong></td>
                  <td>
                    <span className="system-config-value">
                      Tích hợp
                    </span>
                  </td>
                  <td>ASP.NET Core Web API</td>
                  <td>Không thay đổi trong bước này</td>
                </tr>

                <tr>
                  <td><strong>Database</strong></td>
                  <td>
                    <span className="system-config-value">
                      Để sau
                    </span>
                  </td>
                  <td>SQL Server</td>
                  <td>Không thao tác dữ liệu</td>
                </tr>

                <tr>
                  <td><strong>Zalo OA</strong></td>
                  <td>
                    <span className="system-config-value">
                      Adapter / MVP
                    </span>
                  </td>
                  <td>Tích hợp ngoài</td>
                  <td>Không cấu hình token tại giao diện</td>
                </tr>
              </tbody>
            </table>
          </div>
        </section>
      </div>
    </AppLayout>
  )
}

export default SystemPage
