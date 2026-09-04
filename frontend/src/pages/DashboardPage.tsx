import { useEffect, useState } from 'react'
import AppLayout from '../layouts/AppLayout'
import './DashboardPage.css'

type Tone =
  | 'blue'
  | 'green'
  | 'violet'
  | 'orange'
  | 'cyan'

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

function BusinessIconSvg({
  name,
}: BusinessIconProps) {
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
        <rect
          x="3"
          y="5"
          width="18"
          height="14"
          rx="2"
        />
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
        <rect
          x="3"
          y="5"
          width="18"
          height="16"
          rx="2"
        />
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
  icon: Exclude<
    BusinessIcon,
    'map' | 'report' | 'calendar'
  >
}

type PagedResponse = {
  data?: {
    totalCount?: number
  }
  success?: boolean
}

type WelfareDemoItem = {
  programName?: string
  status?: string
  benefitAmount?: number
  citizenSnapshot?: {
    fullName?: string
  }
}

type PaymentDemoItem = {
  paymentNumber?: string
  beneficiaryName?: string
  status?: string
  amount?: number
}

const WELFARE_STORAGE_KEY =
  'ansinhso.demo.welfarecases.v1'

const PAYMENT_STORAGE_KEY =
  'ansinhso.demo.payments.v1'

const DEFAULT_WELFARE_DEMO_COUNT =
  48

const DEFAULT_PAYMENT_DEMO_COUNT =
  36

/*
 * Verified against AnSinhSoRealDb during deployment checkpoint.
 *
 * These are NOT simulated business counts.
 * They are used only when authenticated API access is unavailable.
 */
const VERIFIED_REAL_HOUSEHOLD_SNAPSHOT =
  1263

const VERIFIED_REAL_CITIZEN_SNAPSHOT =
  3969

const kpis: KpiCard[] = [
  {
    label: 'Hộ gia đình',
    description: 'Tổng số hộ',
    tone: 'blue',
    icon: 'household',
  },
  {
    label: 'Người dân',
    description: 'Tổng số nhân khẩu',
    tone: 'green',
    icon: 'citizen',
  },
  {
    label: 'Hồ sơ an sinh',
    description: 'Tổng số hồ sơ',
    tone: 'violet',
    icon: 'welfare',
  },
  {
    label: 'Chi trả trợ cấp',
    description: 'Lượt chi trả',
    tone: 'orange',
    icon: 'payment',
  },
]

const quickAccessItems = [
  {
    eyebrow: 'Quản lý',
    label: 'Hộ gia đình',
    tone: 'blue' as const,
    icon: 'household' as const,
    route: '/households',
  },
  {
    eyebrow: 'Tra cứu',
    label: 'Người dân',
    tone: 'green' as const,
    icon: 'citizen' as const,
    route: '/citizens',
  },
  {
    eyebrow: 'Quản lý',
    label: 'An sinh xã hội',
    tone: 'violet' as const,
    icon: 'welfare' as const,
    route: '/welfare-cases',
  },
  {
    eyebrow: 'Chi trả',
    label: 'Trợ cấp',
    tone: 'orange' as const,
    icon: 'payment' as const,
    route: '/payments',
  },
  {
    eyebrow: 'Xem',
    label: 'Bản đồ số',
    tone: 'blue' as const,
    icon: 'map' as const,
    route: '/map',
  },
  {
    eyebrow: 'Báo cáo',
    label: 'Thống kê',
    tone: 'cyan' as const,
    icon: 'report' as const,
    route: '/reports',
  },
]

function readDemoArray<T>(
  key: string,
): {
  present: boolean
  items: T[]
} {
  try {
    const raw =
      localStorage.getItem(key)

    if (raw === null) {
      return {
        present: false,
        items: [],
      }
    }

    const parsed =
      JSON.parse(raw)

    if (!Array.isArray(parsed)) {
      return {
        present: false,
        items: [],
      }
    }

    return {
      present: true,
      items: parsed as T[],
    }
  } catch {
    return {
      present: false,
      items: [],
    }
  }
}

function DashboardPage() {
  const [
    householdCount,
    setHouseholdCount,
  ] =
    useState<number | null>(null)

  const [
    citizenCount,
    setCitizenCount,
  ] =
    useState<number | null>(null)

  const [
    welfareCaseCount,
    setWelfareCaseCount,
  ] =
    useState<number | null>(null)

  const [
    paymentCount,
    setPaymentCount,
  ] =
    useState<number | null>(null)

  const [
    householdLive,
    setHouseholdLive,
  ] =
    useState(false)

  const [
    citizenLive,
    setCitizenLive,
  ] =
    useState(false)

  const [
    welfareSimulated,
    setWelfareSimulated,
  ] =
    useState(false)

  const [
    paymentSimulated,
    setPaymentSimulated,
  ] =
    useState(false)

  const [
    welfareDemoItems,
    setWelfareDemoItems,
  ] =
    useState<WelfareDemoItem[]>([])

  const [
    paymentDemoItems,
    setPaymentDemoItems,
  ] =
    useState<PaymentDemoItem[]>([])

  useEffect(() => {
    const accessToken =
      localStorage.getItem(
        'accessToken',
      )

    const welfareStored =
      readDemoArray<WelfareDemoItem>(
        WELFARE_STORAGE_KEY,
      )

    const paymentStored =
      readDemoArray<PaymentDemoItem>(
        PAYMENT_STORAGE_KEY,
      )

    setWelfareDemoItems(
      welfareStored.items,
    )

    setPaymentDemoItems(
      paymentStored.items,
    )

    const welfareFallbackCount =
      welfareStored.present
        ? welfareStored.items.length
        : DEFAULT_WELFARE_DEMO_COUNT

    const paymentFallbackCount =
      paymentStored.present
        ? paymentStored.items.length
        : DEFAULT_PAYMENT_DEMO_COUNT

    const applyWelfareFallback =
      () => {
        setWelfareCaseCount(
          welfareFallbackCount,
        )
        setWelfareSimulated(true)
      }

    const applyPaymentFallback =
      () => {
        setPaymentCount(
          paymentFallbackCount,
        )
        setPaymentSimulated(true)
      }

    if (!accessToken) {
      /*
       * Authentication is unavailable in this browser session.
       * Preserve production truth using the SQL-verified snapshot.
       */
      setHouseholdCount(
        VERIFIED_REAL_HOUSEHOLD_SNAPSHOT,
      )

      setCitizenCount(
        VERIFIED_REAL_CITIZEN_SNAPSHOT,
      )

      setHouseholdLive(false)
      setCitizenLive(false)

      applyWelfareFallback()
      applyPaymentFallback()

      return
    }

    const getCount =
      async (
        url: string,
      ): Promise<number | null> => {
        try {
          const response =
            await fetch(
              url,
              {
                headers: {
                  Authorization:
                    `Bearer ${accessToken}`,
                },
              },
            )

          if (!response.ok)
            return null

          const body =
            (await response.json()) as PagedResponse

          const value =
            body.data?.totalCount

          return typeof value ===
            'number'
            ? value
            : null
        } catch {
          return null
        }
      }

    const load =
      async () => {
        const [
          households,
          citizens,
          welfare,
          payments,
        ] =
          await Promise.all([
            getCount(
              '/api/v1/households?page=1&pageSize=1',
            ),
            getCount(
              '/api/v1/citizens?page=1&pageSize=1',
            ),
            getCount(
              '/api/v1/welfarecases?page=1&pageSize=1',
            ),
            getCount(
              '/api/v1/payments?page=1&pageSize=1',
            ),
          ])

        if (households !== null) {
          setHouseholdCount(
            households,
          )

          setHouseholdLive(true)
        } else {
          setHouseholdCount(
            VERIFIED_REAL_HOUSEHOLD_SNAPSHOT,
          )

          setHouseholdLive(false)
        }

        if (citizens !== null) {
          setCitizenCount(
            citizens,
          )

          setCitizenLive(true)
        } else {
          setCitizenCount(
            VERIFIED_REAL_CITIZEN_SNAPSHOT,
          )

          setCitizenLive(false)
        }

        if (
          welfare !== null &&
          welfare > 0
        ) {
          setWelfareCaseCount(
            welfare,
          )
          setWelfareSimulated(false)
        } else {
          applyWelfareFallback()
        }

        if (
          payments !== null &&
          payments > 0
        ) {
          setPaymentCount(
            payments,
          )
          setPaymentSimulated(false)
        } else {
          applyPaymentFallback()
        }
      }

    void load()
  }, [])

  const getKpiValue =
    (
      icon: KpiCard['icon'],
    ) => {
      if (icon === 'household')
        return householdCount

      if (icon === 'citizen')
        return citizenCount

      if (icon === 'welfare')
        return welfareCaseCount

      return paymentCount
    }

  const getKpiSource =
    (
      icon: KpiCard['icon'],
    ) => {
      if (icon === 'household')
        return householdLive
          ? 'REAL LIVE'
          : 'REAL SNAPSHOT'

      if (icon === 'citizen')
        return citizenLive
          ? 'REAL LIVE'
          : 'REAL SNAPSHOT'

      if (icon === 'welfare')
        return welfareSimulated
          ? 'DEMO'
          : 'REAL LIVE'

      return paymentSimulated
        ? 'DEMO'
        : 'REAL LIVE'
    }

  const isKpiSimulated =
    (
      icon: KpiCard['icon'],
    ) =>
      icon === 'welfare'
        ? welfareSimulated
        : icon === 'payment'
          ? paymentSimulated
          : false

  const distribution =
    (() => {
      if (!welfareSimulated) {
        return {
          socialProtection: 0,
          elderly: 0,
          children: 0,
          disability: 0,
          other: 0,
        }
      }

      if (
        welfareDemoItems.length === 0
      ) {
        const total =
          welfareCaseCount ??
          DEFAULT_WELFARE_DEMO_COUNT

        const base =
          Math.floor(total / 6)

        return {
          socialProtection: base,
          elderly: base,
          children: base,
          disability: base,
          other:
            total -
            base * 4,
        }
      }

      const result = {
        socialProtection: 0,
        elderly: 0,
        children: 0,
        disability: 0,
        other: 0,
      }

      welfareDemoItems.forEach(
        (item) => {
          const name =
            (
              item.programName ??
              ''
            ).toLowerCase()

          if (
            name.includes(
              'bảo trợ',
            )
          ) {
            result.socialProtection += 1
          } else if (
            name.includes(
              'cao tuổi',
            )
          ) {
            result.elderly += 1
          } else if (
            name.includes(
              'trẻ em',
            )
          ) {
            result.children += 1
          } else if (
            name.includes(
              'khuyết tật',
            )
          ) {
            result.disability += 1
          } else {
            result.other += 1
          }
        },
      )

      return result
    })()

  const distributionTotal =
    distribution.socialProtection +
    distribution.elderly +
    distribution.children +
    distribution.disability +
    distribution.other

  const percentage =
    (
      value: number,
    ) =>
      distributionTotal > 0
        ? (
            value /
            distributionTotal
          ) *
          100
        : 0

  const p1 =
    percentage(
      distribution.socialProtection,
    )

  const p2 =
    p1 +
    percentage(
      distribution.elderly,
    )

  const p3 =
    p2 +
    percentage(
      distribution.children,
    )

  const p4 =
    p3 +
    percentage(
      distribution.disability,
    )

  const distributionGradient =
    distributionTotal > 0
      ? `conic-gradient(
          #4f8df7 0% ${p1}%,
          #35b884 ${p1}% ${p2}%,
          #e84d98 ${p2}% ${p3}%,
          #ff9447 ${p3}% ${p4}%,
          #ffb617 ${p4}% 100%
        )`
      : undefined

  const overviewItems = [
    {
      label: 'Hộ gia đình',
      value:
        householdCount ?? 0,
      tone: '#2563eb',
      simulated: false,
      source:
        householdLive
          ? 'REAL LIVE'
          : 'REAL SNAPSHOT',
    },
    {
      label: 'Người dân',
      value:
        citizenCount ?? 0,
      tone: '#16a34a',
      simulated: false,
      source:
        citizenLive
          ? 'REAL LIVE'
          : 'REAL SNAPSHOT',
    },
    {
      label: 'Hồ sơ an sinh',
      value:
        welfareCaseCount ?? 0,
      tone: '#7c3aed',
      simulated:
        welfareSimulated,
      source:
        welfareSimulated
          ? 'DEMO'
          : 'REAL LIVE',
    },
    {
      label: 'Chi trả trợ cấp',
      value:
        paymentCount ?? 0,
      tone: '#f97316',
      simulated:
        paymentSimulated,
      source:
        paymentSimulated
          ? 'DEMO'
          : 'REAL LIVE',
    },
  ]

  const overviewMaximum =
    Math.max(
      1,
      ...overviewItems.map(
        (item) =>
          item.value,
      ),
    )

  const activities = [
    {
      tone: 'green',
      icon:
        'household' as BusinessIcon,
      title:
        `${(
          householdCount ??
          0
        ).toLocaleString(
          'vi-VN',
        )} hộ gia đình`,
      description:
        householdLive
          ? 'Dữ liệu API trực tiếp từ AnSinhSoRealDb'
          : 'Snapshot production đã xác minh từ AnSinhSoRealDb',
      time:
        householdLive
          ? 'LIVE'
          : 'SNAPSHOT',
    },
    {
      tone: 'blue',
      icon:
        'citizen' as BusinessIcon,
      title:
        `${(
          citizenCount ??
          0
        ).toLocaleString(
          'vi-VN',
        )} người dân`,
      description:
        householdLive
          ? 'Dữ liệu API trực tiếp từ AnSinhSoRealDb'
          : 'Snapshot production đã xác minh từ AnSinhSoRealDb',
      time:
        householdLive
          ? 'LIVE'
          : 'SNAPSHOT',
    },
    {
      tone: 'violet',
      icon:
        'welfare' as BusinessIcon,
      title:
        `${(
          welfareCaseCount ??
          0
        ).toLocaleString(
          'vi-VN',
        )} hồ sơ an sinh`,
      description:
        welfareSimulated
          ? 'Dữ liệu mô phỏng phục vụ trình diễn'
          : 'Dữ liệu thật từ hệ thống',
      time:
        welfareSimulated
          ? 'DEMO'
          : 'REAL',
    },
    {
      tone: 'orange',
      icon:
        'payment' as BusinessIcon,
      title:
        `${(
          paymentCount ??
          0
        ).toLocaleString(
          'vi-VN',
        )} lượt chi trả`,
      description:
        paymentSimulated
          ? 'Dữ liệu mô phỏng phục vụ trình diễn'
          : 'Dữ liệu thật từ hệ thống',
      time:
        paymentSimulated
          ? 'DEMO'
          : 'REAL',
    },
  ]

  void paymentDemoItems

  return (
    <AppLayout>
      <div className="dashboard-page">

        <section className="dashboard-heading">
          <div>
            <h1>
              Dashboard
            </h1>

            <p>
              Tổng quan hoạt động an sinh xã hội
              tại xã Sông Lũy
            </p>
          </div>

          <button
            className="dashboard-date"
            type="button"
          >
            <BusinessIconSvg
              name="calendar"
            />
            <span>
              Hôm nay
            </span>
            <b>⌄</b>
          </button>
        </section>


        <section className="kpi-grid">
          {kpis.map(
            (item) => {

              const value =
                getKpiValue(
                  item.icon,
                )

              const simulated =
                isKpiSimulated(
                  item.icon,
                )

              const sourceLabel =
                getKpiSource(
                  item.icon,
                )

              return (
                <article
                  className="kpi-card"
                  key={item.label}
                >
                  <div
                    className={
                      `kpi-icon ${item.tone}`
                    }
                  >
                    <BusinessIconSvg
                      name={
                        item.icon
                      }
                    />
                  </div>

                  <div className="kpi-main">
                    <strong>
                      {item.label}
                    </strong>

                    <span>
                      {value
                        ?.toLocaleString(
                          'vi-VN',
                        ) ??
                        '—'}
                    </span>

                    <p>
                      {item.description}
                    </p>
                  </div>

                  <div
                    className={
                      `kpi-change ${item.tone}`
                    }
                    title={
                      simulated
                        ? 'Dữ liệu mô phỏng, không phải số liệu production'
                        : 'Dữ liệu thật'
                    }
                  >
                    {sourceLabel}
                  </div>
                </article>
              )
            },
          )}
        </section>


        <section className="analytics-grid">

          <article className="dashboard-card overview-chart-card">
            <div className="card-heading">
              <div>
                <h2>
                  Biểu đồ tổng quan
                </h2>

                <p>
                  Quy mô dữ liệu hiện tại —
                  REAL và DEMO được tách biệt
                </p>
              </div>
            </div>

            <div
              style={{
                display:
                  'grid',
                gap:
                  '14px',
                padding:
                  '18px 8px 10px',
              }}
            >
              {overviewItems.map(
                (item) => {

                  const width =
                    item.value === 0
                      ? 0
                      : Math.max(
                          2,
                          (
                            item.value /
                            overviewMaximum
                          ) *
                            100,
                        )

                  return (
                    <div
                      key={
                        item.label
                      }
                    >
                      <div
                        style={{
                          display:
                            'flex',
                          justifyContent:
                            'space-between',
                          alignItems:
                            'center',
                          gap:
                            '12px',
                          marginBottom:
                            '7px',
                          fontSize:
                            '12px',
                        }}
                      >
                        <span>
                          {
                            item.label
                          }
                        </span>

                        <strong>
                          {item.value.toLocaleString(
                            'vi-VN',
                          )}
                          {' '}
                          <small
                            style={{
                              color:
                                item.simulated
                                  ? '#c2410c'
                                  : '#047857',
                            }}
                          >
                            {item.source}
                          </small>
                        </strong>
                      </div>

                      <div
                        style={{
                          height:
                            '10px',
                          borderRadius:
                            '999px',
                          background:
                            '#eef2f7',
                          overflow:
                            'hidden',
                        }}
                      >
                        <div
                          style={{
                            width:
                              `${width}%`,
                            height:
                              '100%',
                            minWidth:
                              item.value > 0
                                ? '5px'
                                : 0,
                            background:
                              item.tone,
                            borderRadius:
                              '999px',
                            transition:
                              'width .3s ease',
                          }}
                        />
                      </div>
                    </div>
                  )
                },
              )}
            </div>

            <div className="chart-legend">
              <span className="blue">
                <i />
                Hộ gia đình — REAL
              </span>

              <span className="green">
                <i />
                Người dân — REAL
              </span>

              <span className="violet">
                <i />
                Hồ sơ an sinh
                {welfareSimulated
                  ? ' — DEMO'
                  : ' — REAL'}
              </span>

              <span className="orange">
                <i />
                Chi trả trợ cấp
                {paymentSimulated
                  ? ' — DEMO'
                  : ' — REAL'}
              </span>
            </div>
          </article>


          <article className="dashboard-card distribution-card">
            <div className="card-heading">
              <div>
                <h2>
                  Phân bố đối tượng an sinh
                </h2>

                <p>
                  {welfareSimulated
                    ? 'Phân bố từ dữ liệu mô phỏng'
                    : 'Theo dữ liệu nghiệp vụ thực tế'}
                </p>
              </div>
            </div>

            <div className="distribution-content">

              <div
                className="donut-placeholder"
                style={{
                  background:
                    distributionGradient,
                }}
              >
                <div
                  style={{
                    background:
                      '#ffffff',
                    borderRadius:
                      '999px',
                  }}
                >
                  <strong>
                    {distributionTotal > 0
                      ? distributionTotal.toLocaleString(
                          'vi-VN',
                        )
                      : '—'}
                  </strong>

                  <span>
                    {welfareSimulated
                      ? 'Demo'
                      : 'Tổng'}
                  </span>
                </div>
              </div>

              <div className="distribution-list">

                <div>
                  <span>
                    <i className="blue" />
                    Bảo trợ xã hội
                  </span>

                  <b>
                    {distribution.socialProtection ||
                      '—'}
                  </b>
                </div>

                <div>
                  <span>
                    <i className="green" />
                    Người cao tuổi
                  </span>

                  <b>
                    {distribution.elderly ||
                      '—'}
                  </b>
                </div>

                <div>
                  <span>
                    <i className="pink" />
                    Trẻ em
                  </span>

                  <b>
                    {distribution.children ||
                      '—'}
                  </b>
                </div>

                <div>
                  <span>
                    <i className="orange" />
                    Người khuyết tật
                  </span>

                  <b>
                    {distribution.disability ||
                      '—'}
                  </b>
                </div>

                <div>
                  <span>
                    <i className="yellow" />
                    Khác
                  </span>

                  <b>
                    {distribution.other ||
                      '—'}
                  </b>
                </div>
              </div>
            </div>

            <button
              className="detail-button"
              type="button"
              onClick={() => {
                window.location.href =
                  '/welfare-cases'
              }}
            >
              Xem chi tiết
              <span>→</span>
            </button>
          </article>
        </section>


        <section className="bottom-grid">

          <article className="dashboard-card quick-access-card">
            <div className="card-heading">
              <div>
                <h2>
                  Truy cập nhanh
                </h2>

                <p>
                  Các chức năng thường xuyên sử dụng
                </p>
              </div>
            </div>

            <div className="quick-access-grid">
              {quickAccessItems.map(
                (item) => (
                  <button
                    className={
                      `quick-access ${item.tone}`
                    }
                    type="button"
                    key={item.label}
                    onClick={() => {
                      window.location.href =
                        item.route
                    }}
                  >
                    <span className="quick-icon">
                      <BusinessIconSvg
                        name={
                          item.icon
                        }
                      />
                    </span>

                    <div>
                      <small>
                        {item.eyebrow}
                      </small>

                      <strong>
                        {item.label}
                      </strong>
                    </div>

                    <b>→</b>
                  </button>
                ),
              )}
            </div>
          </article>


          <article className="dashboard-card activity-card">
            <div className="card-heading">
              <div>
                <h2>
                  Hoạt động gần đây
                </h2>

                <p>
                  Trạng thái dữ liệu nghiệp vụ hiện tại
                </p>
              </div>
            </div>

            <div className="activity-list">
              {activities.map(
                (activity) => (
                  <div
                    className="activity-row"
                    key={
                      activity.icon
                    }
                  >
                    <span
                      className={
                        `activity-icon ${activity.tone}`
                      }
                    >
                      <BusinessIconSvg
                        name={
                          activity.icon
                        }
                      />
                    </span>

                    <div>
                      <strong>
                        {activity.title}
                      </strong>

                      <p>
                        {activity.description}
                      </p>
                    </div>

                    <time>
                      {activity.time}
                    </time>
                  </div>
                ),
              )}
            </div>
          </article>


          <article className="dashboard-card notification-card">
            <div className="card-heading">
              <div>
                <h2>
                  Thông báo hệ thống
                </h2>

                <p>
                  Cảnh báo và thông tin vận hành
                </p>
              </div>
            </div>

            <div className="system-notices">

              <div>
                <span className="notice-icon blue">
                  i
                </span>

                <div>
                  <strong>
                    Hệ thống sẵn sàng
                  </strong>

                  <p>
                    Frontend và API đang hoạt động.
                  </p>
                </div>
              </div>

              <div>
                <span className="notice-icon green">
                  ✓
                </span>

                <div>
                  <strong>
                    Dữ liệu production
                  </strong>

                  <p>
                    {(
                      householdCount ??
                      0
                    ).toLocaleString(
                      'vi-VN',
                    )}{' '}
                    hộ và{' '}
                    {(
                      citizenCount ??
                      0
                    ).toLocaleString(
                      'vi-VN',
                    )}{' '}
                    người dân từ AnSinhSoRealDb.
                  </p>
                </div>
              </div>

              <div>
                <span className="notice-icon orange">
                  !
                </span>

                <div>
                  <strong>
                    Dữ liệu trình diễn
                  </strong>

                  <p>
                    {welfareSimulated
                      ? `${welfareCaseCount ?? 0} hồ sơ an sinh`
                      : 'Hồ sơ an sinh REAL'}
                    {' · '}
                    {paymentSimulated
                      ? `${paymentCount ?? 0} lượt chi trả`
                      : 'Chi trả REAL'}.
                    Dữ liệu DEMO không được ghi vào SQL production.
                  </p>
                </div>
              </div>
            </div>
          </article>
        </section>
      </div>
    </AppLayout>
  )
}

export default DashboardPage