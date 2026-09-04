import { useEffect, useMemo, useState } from 'react'
import AppLayout from '../layouts/AppLayout'
import './ReportsPage.css'

/* ANSINHSO_REPORT_REAL_DEMO
 *
 * Production/API is always preferred.
 *
 * If authenticated API access for Household/Citizen is unavailable,
 * the report uses the SQL-verified production snapshot.
 *
 * If WelfareCase/Payment production data is empty,
 * the report uses clearly labelled local DEMO data.
 *
 * DEMO values are never persisted to SQL Server by this page.
 */

type ReportCounts = {
  households: number | null
  citizens: number | null
  welfareCases: number | null
  payments: number | null
}

type SourceKind =
  | 'REAL LIVE'
  | 'REAL SNAPSHOT'
  | 'DEMO'

type ReportSources = {
  households: SourceKind
  citizens: SourceKind
  welfareCases: SourceKind
  payments: SourceKind
}

type PagedResponse = {
  data?: {
    totalCount?: number
  }
}

type ReportIconName =
  | 'report'
  | 'refresh'
  | 'print'
  | 'household'
  | 'citizen'
  | 'welfare'
  | 'payment'
  | 'arrow'

const VERIFIED_REAL_HOUSEHOLDS =
  1263

const VERIFIED_REAL_CITIZENS =
  3969

const DEFAULT_WELFARE_DEMO =
  48

const DEFAULT_PAYMENT_DEMO =
  36

const WELFARE_STORAGE_KEY =
  'ansinhso.demo.welfarecases.v1'

const PAYMENT_STORAGE_KEY =
  'ansinhso.demo.payments.v1'

const initialCounts: ReportCounts = {
  households: null,
  citizens: null,
  welfareCases: null,
  payments: null,
}

const initialSources: ReportSources = {
  households: 'REAL SNAPSHOT',
  citizens: 'REAL SNAPSHOT',
  welfareCases: 'DEMO',
  payments: 'DEMO',
}

function ReportIcon({
  name,
}: {
  name: ReportIconName
}) {
  let content

  if (name === 'household') {
    content = (
      <>
        <circle cx="8" cy="8" r="2.6" />
        <circle cx="16" cy="9" r="2.1" />
        <path d="M3 20c.6-4 2.6-6 6-6s5.4 2 6 6" />
        <path d="M14 15c3 .2 4.8 1.8 5.5 5" />
      </>
    )
  } else if (name === 'citizen') {
    content = (
      <>
        <circle cx="12" cy="8" r="4" />
        <path d="M4.5 21c.7-4.2 3.2-6.2 7.5-6.2s6.8 2 7.5 6.2" />
      </>
    )
  } else if (name === 'welfare') {
    content = (
      <>
        <path d="M12 3 20 6v5c0 5-3.5 8.5-8 10-4.5-1.5-8-5-8-10V6Z" />
        <path d="M9 12h6M12 9v6" />
      </>
    )
  } else if (name === 'payment') {
    content = (
      <>
        <rect x="3" y="5" width="18" height="14" rx="2" />
        <path d="M3 10h18M7 15h3" />
      </>
    )
  } else if (name === 'refresh') {
    content = (
      <>
        <path d="M20 7v5h-5" />
        <path d="M19 12a7 7 0 1 1-2-5" />
      </>
    )
  } else if (name === 'print') {
    content = (
      <>
        <path d="M7 9V3h10v6" />
        <rect x="5" y="14" width="14" height="7" />
        <path d="M5 17H3v-7h18v7h-2" />
      </>
    )
  } else if (name === 'arrow') {
    content = (
      <>
        <path d="M5 12h14" />
        <path d="m14 7 5 5-5 5" />
      </>
    )
  } else {
    content = (
      <>
        <path d="M5 20V11M10 20V5M15 20v-8M20 20V8" />
      </>
    )
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
      {content}
    </svg>
  )
}

function numberText(
  value: number | null,
) {
  return value === null
    ? '—'
    : value.toLocaleString(
        'vi-VN',
      )
}

function readDemoCount(
  key: string,
  defaultValue: number,
) {
  try {
    const raw =
      localStorage.getItem(key)

    if (raw === null)
      return defaultValue

    const parsed =
      JSON.parse(raw)

    if (!Array.isArray(parsed))
      return defaultValue

    /*
     * If the user deleted all local demo rows,
     * zero is intentional and must be preserved.
     */
    return parsed.length
  } catch {
    return defaultValue
  }
}

async function loadApiCount(
  url: string,
  accessToken: string | null,
): Promise<number | null> {
  if (!accessToken)
    return null

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

    const total =
      body.data?.totalCount

    return typeof total === 'number'
      ? total
      : null
  } catch {
    return null
  }
}

function sourceClass(
  source: SourceKind,
) {
  if (source === 'DEMO')
    return 'demo'

  if (source === 'REAL LIVE')
    return 'real-live'

  return 'snapshot'
}

function ReportsPage() {
  const [
    counts,
    setCounts,
  ] =
    useState<ReportCounts>(
      initialCounts,
    )

  const [
    sources,
    setSources,
  ] =
    useState<ReportSources>(
      initialSources,
    )

  const [
    loading,
    setLoading,
  ] =
    useState(false)

  const [
    lastUpdated,
    setLastUpdated,
  ] =
    useState<Date | null>(
      null,
    )

  const [
    syncMessage,
    setSyncMessage,
  ] =
    useState(
      'Đang chuẩn bị dữ liệu báo cáo...',
    )

  const loadReports =
    async () => {
      setLoading(true)

      const accessToken =
        localStorage.getItem(
          'accessToken',
        )

      const welfareDemoCount =
        readDemoCount(
          WELFARE_STORAGE_KEY,
          DEFAULT_WELFARE_DEMO,
        )

      const paymentDemoCount =
        readDemoCount(
          PAYMENT_STORAGE_KEY,
          DEFAULT_PAYMENT_DEMO,
        )

      const [
        householdsApi,
        citizensApi,
        welfareApi,
        paymentApi,
      ] =
        await Promise.all([
          loadApiCount(
            '/api/v1/households?page=1&pageSize=1',
            accessToken,
          ),

          loadApiCount(
            '/api/v1/citizens?page=1&pageSize=1',
            accessToken,
          ),

          loadApiCount(
            '/api/v1/welfarecases?page=1&pageSize=1',
            accessToken,
          ),

          loadApiCount(
            '/api/v1/payments?page=1&pageSize=1',
            accessToken,
          ),
        ])

      const households =
        householdsApi !== null &&
        householdsApi > 0
          ? householdsApi
          : VERIFIED_REAL_HOUSEHOLDS

      const citizens =
        citizensApi !== null &&
        citizensApi > 0
          ? citizensApi
          : VERIFIED_REAL_CITIZENS

      const welfareCases =
        welfareApi !== null &&
        welfareApi > 0
          ? welfareApi
          : welfareDemoCount

      const payments =
        paymentApi !== null &&
        paymentApi > 0
          ? paymentApi
          : paymentDemoCount

      const nextSources: ReportSources =
        {
          households:
            householdsApi !== null &&
            householdsApi > 0
              ? 'REAL LIVE'
              : 'REAL SNAPSHOT',

          citizens:
            citizensApi !== null &&
            citizensApi > 0
              ? 'REAL LIVE'
              : 'REAL SNAPSHOT',

          welfareCases:
            welfareApi !== null &&
            welfareApi > 0
              ? 'REAL LIVE'
              : 'DEMO',

          payments:
            paymentApi !== null &&
            paymentApi > 0
              ? 'REAL LIVE'
              : 'DEMO',
        }

      setCounts({
        households,
        citizens,
        welfareCases,
        payments,
      })

      setSources(
        nextSources,
      )

      setLastUpdated(
        new Date(),
      )

      if (
        nextSources.households ===
          'REAL LIVE' &&
        nextSources.citizens ===
          'REAL LIVE' &&
        nextSources.welfareCases ===
          'REAL LIVE' &&
        nextSources.payments ===
          'REAL LIVE'
      ) {
        setSyncMessage(
          'Toàn bộ chỉ tiêu đang lấy trực tiếp từ API production.',
        )
      } else {
        setSyncMessage(
          'Báo cáo đang kết hợp dữ liệu production và dữ liệu trình diễn; nguồn của từng chỉ tiêu được ghi rõ.',
        )
      }

      setLoading(false)
    }

  useEffect(() => {
    void loadReports()
  }, [])

  const personsPerHousehold =
    useMemo(
      () => {
        if (
          counts.citizens === null ||
          counts.households === null ||
          counts.households === 0
        ) {
          return null
        }

        return (
          counts.citizens /
          counts.households
        )
      },
      [counts],
    )

  const maxCount =
    useMemo(
      () => {
        const values = [
          counts.households,
          counts.citizens,
          counts.welfareCases,
          counts.payments,
        ].filter(
          (
            item,
          ): item is number =>
            item !== null,
        )

        return values.length > 0
          ? Math.max(
              ...values,
              1,
            )
          : 1
      },
      [counts],
    )

  const statisticsRows = [
    {
      key: 'households',
      label: 'Hộ gia đình',
      description:
        'Tổng số hộ gia đình được quản lý trong hệ thống',
      value:
        counts.households,
      source:
        sources.households,
      icon:
        'household' as const,
      href:
        '/households',
      tone:
        'blue',
    },

    {
      key: 'citizens',
      label: 'Người dân',
      description:
        'Tổng số công dân được quản lý tập trung',
      value:
        counts.citizens,
      source:
        sources.citizens,
      icon:
        'citizen' as const,
      href:
        '/citizens',
      tone:
        'green',
    },

    {
      key: 'welfare',
      label: 'Hồ sơ an sinh',
      description:
        'Hồ sơ an sinh production hoặc dữ liệu trình diễn khi production chưa có',
      value:
        counts.welfareCases,
      source:
        sources.welfareCases,
      icon:
        'welfare' as const,
      href:
        '/welfare-cases',
      tone:
        'violet',
    },

    {
      key: 'payments',
      label: 'Chi trả trợ cấp',
      description:
        'Lượt chi trả production hoặc dữ liệu trình diễn khi production chưa có',
      value:
        counts.payments,
      source:
        sources.payments,
      icon:
        'payment' as const,
      href:
        '/payments',
      tone:
        'orange',
    },
  ]

  return (
    <AppLayout>
      <div className="reports-page">

        <section className="reports-heading">
          <div>
            <div className="reports-eyebrow">
              <ReportIcon
                name="report"
              />

              <span>
                BÁO CÁO & THỐNG KÊ
              </span>
            </div>

            <h1>
              Báo cáo thống kê
            </h1>

            <p>
              Tổng hợp dữ liệu an sinh xã hội
              tại xã Sông Lũy từ hệ thống
              AnSinhSo.
            </p>
          </div>

          <div className="reports-actions">
            <button
              className="reports-secondary-button"
              type="button"
              onClick={() =>
                void loadReports()
              }
              disabled={loading}
            >
              <ReportIcon
                name="refresh"
              />

              <span>
                {loading
                  ? 'Đang tải...'
                  : 'Làm mới'}
              </span>
            </button>

            <button
              className="reports-primary-button"
              type="button"
              onClick={() =>
                window.print()
              }
            >
              <ReportIcon
                name="print"
              />

              <span>
                In / Lưu PDF
              </span>
            </button>
          </div>
        </section>


        <section className="reports-meta">
          <div>
            <span className="reports-live-dot" />

            <strong>
              AnSinhSoRealDb + DEMO fallback
              có phân biệt nguồn
            </strong>
          </div>

          <span>
            Cập nhật:{' '}
            {lastUpdated
              ? lastUpdated.toLocaleString(
                  'vi-VN',
                )
              : '—'}
          </span>
        </section>


        <section className="reports-source-message">
          <strong>
            Nguồn báo cáo
          </strong>

          <span>
            {syncMessage}
            {' '}
            Dữ liệu DEMO không được ghi
            vào SQL production.
          </span>
        </section>


        <section className="reports-kpi-grid">
          {statisticsRows.map(
            (item) => (
              <article
                className={
                  `reports-kpi-card ${item.tone}`
                }
                key={item.key}
              >
                <div className="reports-kpi-top">
                  <span className="reports-kpi-icon">
                    <ReportIcon
                      name={item.icon}
                    />
                  </span>

                  <span
                    className={
                      `reports-data-badge ${sourceClass(
                        item.source,
                      )}`
                    }
                  >
                    {item.source}
                  </span>
                </div>

                <strong className="reports-kpi-value">
                  {loading
                    ? '...'
                    : numberText(
                        item.value,
                      )}
                </strong>

                <h2>
                  {item.label}
                </h2>

                <p>
                  {item.description}
                </p>

                <a href={item.href}>
                  Xem chi tiết

                  <ReportIcon
                    name="arrow"
                  />
                </a>
              </article>
            ),
          )}
        </section>


        <section className="reports-main-grid">

          <article className="reports-card reports-structure">
            <div className="reports-card-heading">
              <div>
                <h2>
                  Cơ cấu dữ liệu hệ thống
                </h2>

                <p>
                  So sánh quy mô các nhóm dữ liệu;
                  REAL và DEMO được đánh dấu riêng.
                </p>
              </div>

              <span className="reports-period">
                Hiện tại
              </span>
            </div>

            <div className="reports-bars">
              {statisticsRows.map(
                (item) => {
                  const percent =
                    item.value === null
                      ? 0
                      : Math.round(
                          (
                            item.value /
                            maxCount
                          ) *
                            100,
                        )

                  return (
                    <div
                      className="reports-bar-row"
                      key={item.key}
                    >
                      <div className="reports-bar-label">
                        <span>
                          {item.label}
                          {' · '}
                          <b
                            className={
                              `reports-inline-source ${sourceClass(
                                item.source,
                              )}`
                            }
                          >
                            {item.source}
                          </b>
                        </span>

                        <strong>
                          {numberText(
                            item.value,
                          )}
                        </strong>
                      </div>

                      <div className="reports-bar-track">
                        <span
                          className={
                            `reports-bar-value ${item.tone}`
                          }
                          style={{
                            width:
                              `${Math.max(
                                item.value === 0
                                  ? 0
                                  : percent,

                                item.value
                                  ? 2
                                  : 0,
                              )}%`,
                          }}
                        />
                      </div>
                    </div>
                  )
                },
              )}
            </div>

            <div className="reports-ratio">
              <span>
                Bình quân nhân khẩu / hộ
                <small>
                  REAL
                </small>
              </span>

              <strong>
                {personsPerHousehold === null
                  ? '—'
                  : personsPerHousehold
                      .toLocaleString(
                        'vi-VN',
                        {
                          minimumFractionDigits:
                            2,
                          maximumFractionDigits:
                            2,
                        },
                      )}
              </strong>
            </div>
          </article>


          <article className="reports-card reports-summary">
            <div className="reports-card-heading">
              <div>
                <h2>
                  Tóm tắt vận hành
                </h2>

                <p>
                  Các chỉ số phục vụ báo cáo nhanh.
                </p>
              </div>
            </div>

            <div className="reports-summary-list">
              {statisticsRows.map(
                (item) => (
                  <div key={item.key}>
                    <span>
                      <i
                        className={
                          item.tone
                        }
                      />

                      {item.label}
                    </span>

                    <strong>
                      {numberText(
                        item.value,
                      )}
                      {' '}
                      <small
                        className={
                          `reports-inline-source ${sourceClass(
                            item.source,
                          )}`
                        }
                      >
                        {item.source}
                      </small>
                    </strong>
                  </div>
                ),
              )}
            </div>

            <div className="reports-summary-note">
              <strong>
                Nguyên tắc báo cáo
              </strong>

              <p>
                Hộ gia đình và người dân ưu tiên
                API production; khi phiên API không
                khả dụng, hệ thống dùng snapshot
                production đã được xác minh:
                1.263 hộ và 3.969 người dân.
                Hồ sơ an sinh và chi trả ưu tiên
                dữ liệu production; nếu production
                bằng 0 thì dùng dữ liệu DEMO từ
                trình duyệt và luôn ghi rõ nguồn.
              </p>
            </div>
          </article>
        </section>


        <section className="reports-card reports-table-card">
          <div className="reports-card-heading">
            <div>
              <h2>
                Bảng tổng hợp số liệu
              </h2>

              <p>
                Bảng dùng cho trình bày,
                in và lưu PDF.
              </p>
            </div>

            <span className="reports-period">
              Xã Sông Lũy
            </span>
          </div>

          <div className="reports-table-wrap">
            <table className="reports-table">
              <thead>
                <tr>
                  <th>
                    STT
                  </th>

                  <th>
                    Chỉ tiêu
                  </th>

                  <th>
                    Nội dung
                  </th>

                  <th className="number-column">
                    Số lượng
                  </th>

                  <th>
                    Nguồn
                  </th>
                </tr>
              </thead>

              <tbody>
                {statisticsRows.map(
                  (
                    item,
                    index,
                  ) => (
                    <tr key={item.key}>
                      <td>
                        {index + 1}
                      </td>

                      <td>
                        <strong>
                          {item.label}
                        </strong>
                      </td>

                      <td>
                        {item.description}
                      </td>

                      <td className="number-column">
                        <strong>
                          {numberText(
                            item.value,
                          )}
                        </strong>
                      </td>

                      <td>
                        <span
                          className={
                            `reports-table-status ${sourceClass(
                              item.source,
                            )}`
                          }
                        >
                          <i />

                          {item.source}
                        </span>
                      </td>
                    </tr>
                  ),
                )}
              </tbody>
            </table>
          </div>
        </section>


        <section className="reports-footer-note">
          <div>
            <strong>
              AnSinhSo – Hệ thống An sinh số xã hội
            </strong>

            <span>
              Báo cáo tổng hợp • Xã Sông Lũy
            </span>
          </div>

          <span>
            REAL production và DEMO
            được tách biệt rõ ràng.
          </span>
        </section>

      </div>
    </AppLayout>
  )
}

export default ReportsPage