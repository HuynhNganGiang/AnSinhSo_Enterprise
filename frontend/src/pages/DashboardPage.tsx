import { useEffect, useState } from 'react'
import AppLayout from '../layouts/AppLayout'
import './DashboardPage.css'

type Tone = 'blue' | 'green' | 'violet' | 'orange' | 'cyan'

type BusinessIcon =
  | 'household'
  | 'citizen'
  | 'welfare'
  | 'payment'
  | 'map'
  | 'report'
  | 'calendar'

type BusinessIconProps = {
  name: BusinessIcon
}

function BusinessIconSvg({ name }: BusinessIconProps) {
  const paths = {
    household: (
      <>
        <circle cx="8.5" cy="8" r="2.8" />
        <circle cx="15.8" cy="8.8" r="2.2" />
        <path d="M3.5 19.5c.5-3.8 2.6-5.7 6.1-5.7s5.5 1.9 6 5.7" />
        <path d="M14.5 14.6c3 .1 4.8 1.7 5.4 4.9" />
      </>
    ),
    citizen: (
      <>
        <circle cx="12" cy="8" r="4" />
        <path d="M4.5 21c.6-4.2 3.2-6.2 7.5-6.2s6.9 2 7.5 6.2" />
      </>
    ),
    welfare: (
      <>
        <path d="M12 3 20 6v5c0 5-3.5 8.5-8 10-4.5-1.5-8-5-8-10V6Z" />
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
    calendar: (
      <>
        <rect x="3" y="5" width="18" height="16" rx="2" />
        <path d="M8 3v4M16 3v4M3 10h18" />
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
      {paths[name]}
    </svg>
  )
}

type KpiCard = {
  label: string
  description: string
  tone: Exclude<Tone, 'cyan'>
  icon: Exclude<BusinessIcon, 'map' | 'report' | 'calendar'>
}

type HouseholdPagedResponse = {
  data?: {
    totalCount?: number
  }
  success?: boolean
}

const kpis: KpiCard[] = [
  { label: 'Hộ gia đình', description: 'Tổng số hộ', tone: 'blue', icon: 'household' },
  { label: 'Người dân', description: 'Tổng số nhân khẩu', tone: 'green', icon: 'citizen' },
  { label: 'Đối tượng an sinh', description: 'Đang hưởng chính sách', tone: 'violet', icon: 'welfare' },
  { label: 'Chi trả trợ cấp', description: 'Lượt chi trả', tone: 'orange', icon: 'payment' },
]

const quickAccessItems = [
  { eyebrow: 'Quản lý', label: 'Hộ gia đình', tone: 'blue' as const, icon: 'household' as const },
  { eyebrow: 'Tra cứu', label: 'Người dân', tone: 'green' as const, icon: 'citizen' as const },
  { eyebrow: 'Quản lý', label: 'An sinh xã hội', tone: 'violet' as const, icon: 'welfare' as const },
  { eyebrow: 'Chi trả', label: 'Trợ cấp', tone: 'orange' as const, icon: 'payment' as const },
  { eyebrow: 'Xem', label: 'Bản đồ số', tone: 'blue' as const, icon: 'map' as const },
  { eyebrow: 'Báo cáo', label: 'Thống kê', tone: 'cyan' as const, icon: 'report' as const },
]

function DashboardPage() {
  const [householdCount, setHouseholdCount] = useState<number | null>(null)
  const [citizenCount, setCitizenCount] = useState<number | null>(null)

  useEffect(() => {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      return
    }

    const loadHouseholdCount = async () => {
      try {
        const response = await fetch('/api/v1/households?page=1&pageSize=1', {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        })

        if (!response.ok) {
          return
        }

        const body = (await response.json()) as HouseholdPagedResponse
        const totalCount = body.data?.totalCount

        if (typeof totalCount === 'number') {
          setHouseholdCount(totalCount)
        }
      } catch {
        // Giữ trạng thái chưa có dữ liệu khi API không khả dụng.
      }
    }
    const loadCitizenCount = async () => {
      try {
        const response = await fetch('/api/v1/citizens?page=1&pageSize=1', {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        })

        if (!response.ok) {
          return
        }

        const body = (await response.json()) as HouseholdPagedResponse
        const totalCount = body.data?.totalCount

        if (typeof totalCount === 'number') {
          setCitizenCount(totalCount)
        }
      } catch {
        // Giữ trạng thái chưa có dữ liệu khi API không khả dụng.
      }
    }

    void loadHouseholdCount()
    void loadCitizenCount()
  }, [])

  return (
    <AppLayout>
      <div className="dashboard-page">
        <section className="dashboard-heading">
          <div>
            <h1>Dashboard</h1>
            <p>Tổng quan hoạt động an sinh xã hội tại xã Sông Lũy</p>
          </div>

          <button className="dashboard-date" type="button">
            <BusinessIconSvg name="calendar" />
            <span>Hôm nay</span>
            <b>⌄</b>
          </button>
        </section>

        <section className="kpi-grid">
          {kpis.map((item) => (
            <article className="kpi-card" key={item.label}>
              <div className={`kpi-icon ${item.tone}`}>
                <BusinessIconSvg name={item.icon} />
              </div>
              <div className="kpi-main">
                <strong>{item.label}</strong>
                <span>
                  {item.icon === 'household'
                    ? householdCount?.toLocaleString('vi-VN') ?? '—'
                    : item.icon === 'citizen'
                      ? citizenCount?.toLocaleString('vi-VN') ?? '—'
                      : '—'}
                </span>
                <p>{item.description}</p>
              </div>
              <div className={`kpi-change ${item.tone}`}>↗ —%</div>
            </article>
          ))}
        </section>

        <section className="analytics-grid">
          <article className="dashboard-card overview-chart-card">
            <div className="card-heading">
              <div>
                <h2>Biểu đồ tổng quan</h2>
                <p>Thống kê hoạt động 7 ngày gần nhất</p>
              </div>
              <button type="button">7 ngày qua <span>⌄</span></button>
            </div>

            <div className="overview-chart">
              <div className="chart-numbers">
                <span>4</span><span>3</span><span>2</span><span>1</span><span>0</span>
              </div>
              <div className="chart-body">
                <i /><i /><i /><i /><i />
                <div className="placeholder-series blue">
                  {Array.from({ length: 7 }).map((_, index) => <span key={index} />)}
                </div>
                <div className="chart-days">
                  <span>T2</span><span>T3</span><span>T4</span><span>T5</span><span>T6</span><span>T7</span><span>CN</span>
                </div>
              </div>
            </div>

            <div className="chart-legend">
              <span className="blue"><i />Hộ gia đình</span>
              <span className="green"><i />Người dân</span>
              <span className="violet"><i />Đối tượng an sinh</span>
              <span className="orange"><i />Chi trả trợ cấp</span>
            </div>
          </article>

          <article className="dashboard-card distribution-card">
            <div className="card-heading">
              <div>
                <h2>Phân bố đối tượng an sinh</h2>
                <p>Theo nhóm đối tượng</p>
              </div>
            </div>

            <div className="distribution-content">
              <div className="donut-placeholder">
                <div><strong>—</strong><span>Tổng</span></div>
              </div>
              <div className="distribution-list">
                <div><span><i className="blue" />Bảo trợ xã hội</span><b>—</b></div>
                <div><span><i className="green" />Người cao tuổi</span><b>—</b></div>
                <div><span><i className="pink" />Trẻ em</span><b>—</b></div>
                <div><span><i className="orange" />Người khuyết tật</span><b>—</b></div>
                <div><span><i className="yellow" />Khác</span><b>—</b></div>
              </div>
            </div>

            <button className="detail-button" type="button">Xem chi tiết <span>→</span></button>
          </article>
        </section>

        <section className="bottom-grid">
          <article className="dashboard-card quick-access-card">
            <div className="card-heading">
              <div><h2>Truy cập nhanh</h2><p>Các chức năng thường xuyên sử dụng</p></div>
            </div>
            <div className="quick-access-grid">
              {quickAccessItems.map((item) => (
                <button className={`quick-access ${item.tone}`} type="button" key={item.label}>
                  <span className="quick-icon"><BusinessIconSvg name={item.icon} /></span>
                  <div><small>{item.eyebrow}</small><strong>{item.label}</strong></div>
                  <b>→</b>
                </button>
              ))}
            </div>
          </article>

          <article className="dashboard-card activity-card">
            <div className="card-heading">
              <div><h2>Hoạt động gần đây</h2><p>Nhật ký nghiệp vụ mới nhất</p></div>
              <button className="link-button" type="button">Xem tất cả</button>
            </div>
            <div className="activity-list">
              {[
                ['green', 'household'],
                ['blue', 'citizen'],
                ['violet', 'welfare'],
                ['orange', 'payment'],
              ].map(([tone, icon]) => (
                <div className="activity-row" key={`${tone}-${icon}`}>
                  <span className={`activity-icon ${tone}`}><BusinessIconSvg name={icon as BusinessIcon} /></span>
                  <div><strong>Chưa có dữ liệu</strong><p>Dữ liệu hoạt động sẽ hiển thị tại đây</p></div>
                  <time>—</time>
                </div>
              ))}
            </div>
          </article>

          <article className="dashboard-card notification-card">
            <div className="card-heading">
              <div><h2>Thông báo hệ thống</h2><p>Cảnh báo và thông tin vận hành</p></div>
              <button className="link-button" type="button">Xem tất cả</button>
            </div>
            <div className="system-notices">
              <div><span className="notice-icon blue">i</span><div><strong>Hệ thống sẵn sàng</strong><p>Chưa ghi nhận cảnh báo hệ thống.</p></div></div>
              <div><span className="notice-icon orange">!</span><div><strong>Dữ liệu Dashboard</strong><p>Sẽ được đồng bộ từ API ở bước tiếp theo.</p></div></div>
              <div><span className="notice-icon green">✓</span><div><strong>Kết nối hệ thống</strong><p>Frontend đang hoạt động bình thường.</p></div></div>
            </div>
          </article>
        </section>
      </div>
    </AppLayout>
  )
}

export default DashboardPage
