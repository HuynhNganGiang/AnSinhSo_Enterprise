import {
  useEffect,
  useMemo,
  useState,
} from 'react'

import './AuditLogsPanel.css'


type AuditCategory =
  | 'login'
  | 'security'
  | 'history'
  | 'other'


type AuditStatus =
  | 'success'
  | 'failed'
  | 'warning'
  | 'info'


type AuditSource =
  | 'REAL SNAPSHOT'
  | 'DEMO LOCAL'


type AuditEvent = {
  id: string
  category: AuditCategory
  title: string
  description: string
  userId: string
  username: string
  ipAddress: string
  userAgent: string
  status: AuditStatus
  occurredAt: string | null
  source: AuditSource
  table: string
  raw: Record<string, string | null>
}


type AuditApiResponse = {
  success?: boolean

  data?: {
    source?: string
    snapshotAt?: string
    counts?: Record<string, number>
    items?: AuditEvent[]
  }
}


type DateFilter =
  | 'all'
  | 'today'
  | '7d'
  | '30d'


const DEMO_KEY =
  'ansinhso.demo.system-audit.v1'


const PAGE_SIZE =
  10


const T = {
  eyebrow:
    'H\u1EC6 TH\u1ED0NG 03',

  title:
    'Nh\u1EADt k\u00FD h\u1EC7 th\u1ED1ng',

  subtitle:
    'Theo d\u00F5i \u0111\u0103ng nh\u1EADp, b\u1EA3o m\u1EADt v\u00E0 c\u00E1c s\u1EF1 ki\u1EC7n c\u1EA7n truy v\u1EBFt.',

  noticeReal:
    'D\u1EEF li\u1EC7u REAL \u0111\u01B0\u1EE3c \u0111\u1ECDc t\u1EEB AnSinhSoRealDb qua API ch\u1EC9 \u0111\u1ECDc.',

  noticeDemo:
    'Production ch\u01B0a c\u00F3 nh\u1EADt k\u00FD ho\u1EB7c API kh\u00F4ng kh\u1EA3 d\u1EE5ng. H\u1EC7 th\u1ED1ng \u0111ang hi\u1EC3n th\u1ECB DEMO LOCAL, kh\u00F4ng ghi v\u00E0o SQL production.',

  refresh:
    'L\u00E0m m\u1EDBi',

  exportCsv:
    'Xu\u1EA5t CSV',

  resetDemo:
    'T\u1EA1o l\u1EA1i DEMO',

  production:
    'S\u1EF1 ki\u1EC7n production',

  displayed:
    'S\u1EF1 ki\u1EC7n hi\u1EC3n th\u1ECB',

  failed:
    'C\u1EA7n ch\u00FA \u00FD',

  security:
    'S\u1EF1 ki\u1EC7n b\u1EA3o m\u1EADt',

  login:
    '\u0110\u0103ng nh\u1EADp',

  securityTab:
    'B\u1EA3o m\u1EADt',

  history:
    'L\u1ECBch s\u1EED',

  all:
    'T\u1EA5t c\u1EA3',

  search:
    'T\u00ECm s\u1EF1 ki\u1EC7n, ng\u01B0\u1EDDi d\u00F9ng, IP...',

  allStatuses:
    'T\u1EA5t c\u1EA3 tr\u1EA1ng th\u00E1i',

  allSources:
    'T\u1EA5t c\u1EA3 ngu\u1ED3n',

  allTime:
    'To\u00E0n b\u1ED9 th\u1EDDi gian',

  today:
    'H\u00F4m nay',

  sevenDays:
    '7 ng\u00E0y g\u1EA7n \u0111\u00E2y',

  thirtyDays:
    '30 ng\u00E0y g\u1EA7n \u0111\u00E2y',

  time:
    'Th\u1EDDi gian',

  event:
    'S\u1EF1 ki\u1EC7n',

  user:
    'Ng\u01B0\u1EDDi d\u00F9ng',

  ip:
    '\u0110\u1ECBa ch\u1EC9 IP',

  status:
    'Tr\u1EA1ng th\u00E1i',

  source:
    'Ngu\u1ED3n',

  action:
    'Thao t\u00E1c',

  detail:
    'Chi ti\u1EBFt',

  noData:
    'Kh\u00F4ng c\u00F3 d\u1EEF li\u1EC7u ph\u00F9 h\u1EE3p.',

  previous:
    'Tr\u01B0\u1EDBc',

  next:
    'Sau',

  page:
    'Trang',

  records:
    'b\u1EA3n ghi',

  success:
    'Th\u00E0nh c\u00F4ng',

  failedStatus:
    'Th\u1EA5t b\u1EA1i',

  warning:
    'C\u1EA3nh b\u00E1o',

  info:
    'Th\u00F4ng tin',

  unknownTime:
    'Ch\u01B0a c\u1EADp nh\u1EADt',

  close:
    '\u0110\u00F3ng',

  raw:
    'D\u1EEF li\u1EC7u truy v\u1EBFt',

  userAgent:
    'Thi\u1EBFt b\u1ECB / User Agent',

  loading:
    '\u0110ang t\u1EA3i nh\u1EADt k\u00FD...',

  realReadonly:
    'REAL READ ONLY',

  demo:
    'DEMO LOCAL',
}


function categoryLabel(
  category:
    AuditCategory,
) {
  switch (category) {
    case 'login':
      return T.login

    case 'security':
      return T.securityTab

    case 'history':
      return T.history

    default:
      return T.info
  }
}


function statusLabel(
  status:
    AuditStatus,
) {
  switch (status) {
    case 'success':
      return T.success

    case 'failed':
      return T.failedStatus

    case 'warning':
      return T.warning

    default:
      return T.info
  }
}


function formatDateTime(
  value:
    string | null,
) {
  if (!value) {
    return T.unknownTime
  }

  const date =
    new Date(value)

  if (
    Number.isNaN(
      date.getTime(),
    ) ||
    date.getFullYear() <= 1
  ) {
    return T.unknownTime
  }

  return date.toLocaleString(
    'vi-VN',
  )
}


function createDemoEvents():
  AuditEvent[] {

  const now =
    Date.now()

  const categories:
    AuditCategory[] =
      [
        'login',
        'security',
        'history',
      ]

  const users =
    [
      'ngangiang',
      'canbo01',
      'lanhdao01',
      'system',
    ]

  const ips =
    [
      '10.10.1.12',
      '10.10.1.18',
      '192.168.1.25',
      '172.16.0.11',
    ]

  const rows:
    AuditEvent[] =
      []

  for (
    let index = 0;
    index < 18;
    index++
  ) {

    const category =
      categories[
        index %
        categories.length
      ]

    const isFailed =
      index === 4 ||
      index === 11

    const status:
      AuditStatus =
        isFailed
          ? 'failed'
          : index % 7 === 0
            ? 'warning'
            : 'success'

    const title =
      category === 'login'
        ? (
            isFailed
              ? '\u0110\u0103ng nh\u1EADp kh\u00F4ng th\u00E0nh c\u00F4ng'
              : '\u0110\u0103ng nh\u1EADp th\u00E0nh c\u00F4ng'
          )
        : category === 'security'
          ? (
              status === 'warning'
                ? 'Ph\u00E1t hi\u1EC7n s\u1EF1 ki\u1EC7n b\u1EA3o m\u1EADt c\u1EA7n theo d\u00F5i'
                : 'Ki\u1EC3m tra b\u1EA3o m\u1EADt h\u1EC7 th\u1ED1ng'
            )
          : 'Ghi nh\u1EADn l\u1ECBch s\u1EED truy c\u1EADp'

    const description =
      category === 'login'
        ? 'Nh\u1EADt k\u00FD x\u00E1c th\u1EF1c t\u00E0i kho\u1EA3n ph\u1EE5c v\u1EE5 tr\u00ECnh di\u1EC5n.'
        : category === 'security'
          ? 'S\u1EF1 ki\u1EC7n b\u1EA3o m\u1EADt m\u00F4 ph\u1ECFng ph\u1EE5c v\u1EE5 gi\u00E1m s\u00E1t.'
          : 'L\u1ECBch s\u1EED ho\u1EA1t \u0111\u1ED9ng m\u00F4 ph\u1ECFng ph\u1EE5c v\u1EE5 truy v\u1EBFt.'

    const username =
      users[
        index %
        users.length
      ]

    const ip =
      ips[
        index %
        ips.length
      ]

    rows.push({
      id:
        `demo-audit-${index + 1}`,

      category,

      title,

      description,

      userId:
        `demo-user-${index % users.length + 1}`,

      username,

      ipAddress:
        ip,

      userAgent:
        index % 2 === 0
          ? 'Microsoft Edge / Windows'
          : 'Chrome / Windows',

      status,

      occurredAt:
        new Date(
          now -
          index *
          47 *
          60 *
          1000,
        ).toISOString(),

      source:
        'DEMO LOCAL',

      table:
        category === 'login'
          ? 'AuditLogins'
          : category === 'security'
            ? 'SecurityLogs'
            : 'LoginHistories',

      raw: {
        Demo:
          'true',

        Sequence:
          String(
            index + 1,
          ),

        Username:
          username,

        IpAddress:
          ip,
      },
    })
  }

  return rows
}


function readDemoEvents():
  AuditEvent[] {

  try {

    const raw =
      localStorage.getItem(
        DEMO_KEY,
      )

    if (raw) {

      const parsed =
        JSON.parse(raw)

      if (
        Array.isArray(
          parsed,
        )
      ) {
        return parsed as AuditEvent[]
      }
    }

  } catch {
    // Fall through.
  }


  const created =
    createDemoEvents()

  localStorage.setItem(
    DEMO_KEY,
    JSON.stringify(
      created,
    ),
  )

  return created
}


function AuditLogsPanel() {

  const [
    rows,
    setRows,
  ] =
    useState<AuditEvent[]>(
      [],
    )


  const [
    productionCounts,
    setProductionCounts,
  ] =
    useState<
      Record<string, number>
    >({})


  const [
    demoMode,
    setDemoMode,
  ] =
    useState(false)


  const [
    loading,
    setLoading,
  ] =
    useState(true)


  const [
    apiMessage,
    setApiMessage,
  ] =
    useState('')


  const [
    activeCategory,
    setActiveCategory,
  ] =
    useState<
      'all' |
      AuditCategory
    >('all')


  const [
    keyword,
    setKeyword,
  ] =
    useState('')


  const [
    statusFilter,
    setStatusFilter,
  ] =
    useState<
      'all' |
      AuditStatus
    >('all')


  const [
    sourceFilter,
    setSourceFilter,
  ] =
    useState<
      'all' |
      'real' |
      'demo'
    >('all')


  const [
    dateFilter,
    setDateFilter,
  ] =
    useState<DateFilter>(
      'all',
    )


  const [
    page,
    setPage,
  ] =
    useState(1)


  const [
    selected,
    setSelected,
  ] =
    useState<
      AuditEvent |
      null
    >(null)


  const loadData =
    async () => {

      setLoading(true)

      try {

        const token =
          localStorage.getItem(
            'accessToken',
          )


        const response =
          await fetch(
            '/api/v1/system/audit-logs',
            {
              headers:
                token
                  ? {
                      Authorization:
                        `Bearer ${token}`,
                    }
                  : {},
            },
          )


        if (!response.ok) {

          throw new Error(
            `HTTP ${response.status}`,
          )
        }


        const body =
          (
            await response.json()
          ) as AuditApiResponse


        const data =
          body.data


        const realRows =
          Array.isArray(
            data?.items,
          )
            ? data?.items ?? []
            : []


        setProductionCounts(
          data?.counts ?? {},
        )


        if (
          realRows.length > 0
        ) {

          setRows(
            realRows.map(
              row => ({
                ...row,
                source:
                  'REAL SNAPSHOT',
              }),
            ),
          )

          setDemoMode(false)

          setApiMessage(
            T.noticeReal,
          )

        } else {

          const demo =
            readDemoEvents()

          setRows(demo)

          setDemoMode(true)

          setApiMessage(
            T.noticeDemo,
          )
        }

      } catch {

        const demo =
          readDemoEvents()

        setRows(demo)

        setDemoMode(true)

        setApiMessage(
          T.noticeDemo,
        )
      }
      finally {

        setLoading(false)
        setPage(1)
      }
    }


  useEffect(
    () => {
      void loadData()
    },
    [],
  )


  const productionTotal =
    useMemo(
      () =>
        Object.values(
          productionCounts,
        ).reduce(
          (
            total,
            value,
          ) =>
            total +
            Number(value || 0),
          0,
        ),
      [
        productionCounts,
      ],
    )


  const filteredRows =
    useMemo(
      () => {

        const q =
          keyword
            .trim()
            .toLowerCase()

        const now =
          Date.now()


        return rows.filter(
          row => {

            const categoryOk =
              activeCategory ===
                'all' ||
              row.category ===
                activeCategory


            const statusOk =
              statusFilter ===
                'all' ||
              row.status ===
                statusFilter


            const sourceOk =
              sourceFilter ===
                'all' ||
              (
                sourceFilter ===
                  'real' &&
                row.source ===
                  'REAL SNAPSHOT'
              ) ||
              (
                sourceFilter ===
                  'demo' &&
                row.source ===
                  'DEMO LOCAL'
              )


            const searchable =
              [
                row.title,
                row.description,
                row.username,
                row.userId,
                row.ipAddress,
                row.table,
              ]
                .join(' ')
                .toLowerCase()


            const keywordOk =
              !q ||
              searchable.includes(
                q,
              )


            let dateOk =
              true


            if (
              dateFilter !==
              'all'
            ) {

              if (
                !row.occurredAt
              ) {

                dateOk = false

              } else {

                const time =
                  new Date(
                    row.occurredAt,
                  ).getTime()


                if (
                  Number.isNaN(
                    time,
                  )
                ) {

                  dateOk = false

                } else if (
                  dateFilter ===
                  'today'
                ) {

                  const current =
                    new Date()

                  const eventDate =
                    new Date(time)

                  dateOk =
                    current.getFullYear() ===
                      eventDate.getFullYear() &&
                    current.getMonth() ===
                      eventDate.getMonth() &&
                    current.getDate() ===
                      eventDate.getDate()

                } else {

                  const days =
                    dateFilter ===
                      '7d'
                      ? 7
                      : 30

                  dateOk =
                    now - time <=
                    days *
                    24 *
                    60 *
                    60 *
                    1000
                }
              }
            }


            return (
              categoryOk &&
              statusOk &&
              sourceOk &&
              keywordOk &&
              dateOk
            )
          },
        )
      },
      [
        rows,
        activeCategory,
        statusFilter,
        sourceFilter,
        keyword,
        dateFilter,
      ],
    )


  const pageCount =
    Math.max(
      1,
      Math.ceil(
        filteredRows.length /
          PAGE_SIZE,
      ),
    )


  const currentPage =
    Math.min(
      page,
      pageCount,
    )


  const visibleRows =
    filteredRows.slice(
      (currentPage - 1) *
        PAGE_SIZE,
      currentPage *
        PAGE_SIZE,
    )


  const failedCount =
    rows.filter(
      row =>
        row.status ===
          'failed' ||
        row.status ===
          'warning',
    ).length


  const securityCount =
    rows.filter(
      row =>
        row.category ===
        'security',
    ).length


  const resetDemo =
    () => {

      localStorage.removeItem(
        DEMO_KEY,
      )

      const demo =
        createDemoEvents()

      localStorage.setItem(
        DEMO_KEY,
        JSON.stringify(
          demo,
        ),
      )

      setRows(demo)
      setDemoMode(true)
      setPage(1)
    }


  const exportCsv =
    () => {

      const header =
        [
          'Time',
          'Category',
          'Event',
          'User',
          'IP',
          'Status',
          'Source',
        ]


      const csvCell =
        (
          value:
            unknown,
        ) => {

          const text =
            String(
              value ?? '',
            ).replace(
              /"/g,
              '""',
            )

          return `"${text}"`
        }


      const lines =
        [
          header
            .map(csvCell)
            .join(','),

          ...filteredRows.map(
            row =>
              [
                row.occurredAt ?? '',
                categoryLabel(
                  row.category,
                ),
                row.title,
                row.username ||
                  row.userId,
                row.ipAddress,
                statusLabel(
                  row.status,
                ),
                row.source,
              ]
                .map(csvCell)
                .join(','),
          ),
        ]


      const blob =
        new Blob(
          [
            '\uFEFF' +
            lines.join('\n'),
          ],
          {
            type:
              'text/csv;charset=utf-8',
          },
        )


      const url =
        URL.createObjectURL(
          blob,
        )


      const anchor =
        document.createElement(
          'a',
        )

      anchor.href =
        url

      anchor.download =
        'ansinhso-system-audit.csv'

      document.body.appendChild(
        anchor,
      )

      anchor.click()
      anchor.remove()

      URL.revokeObjectURL(
        url,
      )
    }


  return (
    <section className="audit-panel">

      <div className="audit-heading">

        <div>

          <div className="audit-eyebrow">
            {T.eyebrow}
          </div>

          <h2>
            {T.title}
          </h2>

          <p>
            {T.subtitle}
          </p>

        </div>


        <div className="audit-actions">

          <button
            type="button"
            className="audit-button secondary"
            onClick={() =>
              void loadData()
            }
          >
            ↻ {T.refresh}
          </button>


          <button
            type="button"
            className="audit-button secondary"
            onClick={exportCsv}
          >
            ↓ {T.exportCsv}
          </button>


          {demoMode && (

            <button
              type="button"
              className="audit-button primary"
              onClick={resetDemo}
            >
              {T.resetDemo}
            </button>
          )}

        </div>

      </div>


      <div
        className={
          `audit-notice ${
            demoMode
              ? 'demo'
              : 'real'
          }`
        }
      >

        <strong>
          {demoMode
            ? T.demo
            : T.realReadonly}
        </strong>

        <span>
          {apiMessage}
        </span>

      </div>


      <div className="audit-summary">

        <article>

          <span>
            {T.production}
          </span>

          <strong>
            {productionTotal}
          </strong>

          <small>
            REAL DB
          </small>

        </article>


        <article>

          <span>
            {T.displayed}
          </span>

          <strong>
            {rows.length}
          </strong>

          <small>
            {demoMode
              ? T.demo
              : 'REAL SNAPSHOT'}
          </small>

        </article>


        <article>

          <span>
            {T.failed}
          </span>

          <strong>
            {failedCount}
          </strong>

          <small>
            failed + warning
          </small>

        </article>


        <article>

          <span>
            {T.security}
          </span>

          <strong>
            {securityCount}
          </strong>

          <small>
            SecurityLogs
          </small>

        </article>

      </div>


      <div className="audit-source-counters">

        <div>
          <span>
            AuditLogins
          </span>
          <strong>
            {productionCounts
              .AuditLogins ?? 0}
          </strong>
        </div>

        <div>
          <span>
            SecurityLogs
          </span>
          <strong>
            {productionCounts
              .SecurityLogs ?? 0}
          </strong>
        </div>

        <div>
          <span>
            LoginHistories
          </span>
          <strong>
            {productionCounts
              .LoginHistories ?? 0}
          </strong>
        </div>

      </div>


      <div className="audit-tabs">

        {[
          {
            key:
              'all' as const,
            label:
              T.all,
          },
          {
            key:
              'login' as const,
            label:
              T.login,
          },
          {
            key:
              'security' as const,
            label:
              T.securityTab,
          },
          {
            key:
              'history' as const,
            label:
              T.history,
          },
        ].map(
          item => (

            <button
              type="button"
              key={item.key}
              className={
                activeCategory ===
                item.key
                  ? 'active'
                  : ''
              }
              onClick={() => {
                setActiveCategory(
                  item.key,
                )
                setPage(1)
              }}
            >
              {item.label}
            </button>
          ),
        )}

      </div>


      <div className="audit-toolbar">

        <input
          value={keyword}
          placeholder={T.search}
          onChange={event => {
            setKeyword(
              event.target.value,
            )
            setPage(1)
          }}
        />


        <select
          value={statusFilter}
          onChange={event => {
            setStatusFilter(
              event.target.value as
                'all' |
                AuditStatus,
            )
            setPage(1)
          }}
        >

          <option value="all">
            {T.allStatuses}
          </option>

          <option value="success">
            {T.success}
          </option>

          <option value="failed">
            {T.failedStatus}
          </option>

          <option value="warning">
            {T.warning}
          </option>

          <option value="info">
            {T.info}
          </option>

        </select>


        <select
          value={sourceFilter}
          onChange={event => {
            setSourceFilter(
              event.target.value as
                'all' |
                'real' |
                'demo',
            )
            setPage(1)
          }}
        >

          <option value="all">
            {T.allSources}
          </option>

          <option value="real">
            REAL SNAPSHOT
          </option>

          <option value="demo">
            DEMO LOCAL
          </option>

        </select>


        <select
          value={dateFilter}
          onChange={event => {
            setDateFilter(
              event.target.value as
                DateFilter,
            )
            setPage(1)
          }}
        >

          <option value="all">
            {T.allTime}
          </option>

          <option value="today">
            {T.today}
          </option>

          <option value="7d">
            {T.sevenDays}
          </option>

          <option value="30d">
            {T.thirtyDays}
          </option>

        </select>

      </div>


      {loading ? (

        <div className="audit-empty">
          {T.loading}
        </div>

      ) : (

        <>

          <div className="audit-table-wrap">

            <table className="audit-table">

              <thead>
                <tr>
                  <th>STT</th>
                  <th>{T.time}</th>
                  <th>{T.event}</th>
                  <th>{T.user}</th>
                  <th>{T.ip}</th>
                  <th>{T.status}</th>
                  <th>{T.source}</th>
                  <th>{T.action}</th>
                </tr>
              </thead>


              <tbody>

                {visibleRows.length ===
                0 ? (

                  <tr>
                    <td
                      colSpan={8}
                      className="audit-empty-cell"
                    >
                      {T.noData}
                    </td>
                  </tr>

                ) : (

                  visibleRows.map(
                    (
                      row,
                      index,
                    ) => (

                      <tr
                        key={row.id}
                      >

                        <td>
                          {(currentPage - 1) *
                            PAGE_SIZE +
                            index +
                            1}
                        </td>


                        <td className="audit-time">
                          {formatDateTime(
                            row.occurredAt,
                          )}
                        </td>


                        <td>

                          <div className="audit-event">

                            <span
                              className={
                                `audit-category ${row.category}`
                              }
                            >
                              {categoryLabel(
                                row.category,
                              )}
                            </span>

                            <strong>
                              {row.title}
                            </strong>

                            <small>
                              {row.description ||
                                row.table}
                            </small>

                          </div>

                        </td>


                        <td>

                          <strong>
                            {row.username ||
                              row.userId ||
                              '—'}
                          </strong>

                        </td>


                        <td>
                          {row.ipAddress ||
                            '—'}
                        </td>


                        <td>

                          <span
                            className={
                              `audit-status ${row.status}`
                            }
                          >
                            {statusLabel(
                              row.status,
                            )}
                          </span>

                        </td>


                        <td>

                          <span
                            className={
                              `audit-source ${
                                row.source ===
                                'REAL SNAPSHOT'
                                  ? 'real'
                                  : 'demo'
                              }`
                            }
                          >
                            {row.source}
                          </span>

                        </td>


                        <td>

                          <button
                            type="button"
                            className="audit-detail-button"
                            onClick={() =>
                              setSelected(
                                row,
                              )
                            }
                          >
                            {T.detail}
                          </button>

                        </td>

                      </tr>
                    ),
                  )
                )}

              </tbody>

            </table>

          </div>


          <div className="audit-pagination">

            <span>
              {filteredRows.length} {T.records}
            </span>

            <div>

              <button
                type="button"
                disabled={
                  currentPage <=
                  1
                }
                onClick={() =>
                  setPage(
                    Math.max(
                      1,
                      currentPage -
                        1,
                    ),
                  )
                }
              >
                ← {T.previous}
              </button>


              <span>
                {T.page} {currentPage}/{pageCount}
              </span>


              <button
                type="button"
                disabled={
                  currentPage >=
                  pageCount
                }
                onClick={() =>
                  setPage(
                    Math.min(
                      pageCount,
                      currentPage +
                        1,
                    ),
                  )
                }
              >
                {T.next} →
              </button>

            </div>

          </div>

        </>
      )}


      {selected && (

        <div className="audit-modal-overlay">

          <div className="audit-modal">

            <div className="audit-modal-heading">

              <div>

                <span
                  className={
                    `audit-source ${
                      selected.source ===
                      'REAL SNAPSHOT'
                        ? 'real'
                        : 'demo'
                    }`
                  }
                >
                  {selected.source}
                </span>

                <h3>
                  {selected.title}
                </h3>

                <p>
                  {formatDateTime(
                    selected.occurredAt,
                  )}
                </p>

              </div>


              <button
                type="button"
                className="audit-modal-close"
                onClick={() =>
                  setSelected(
                    null,
                  )
                }
              >
                ×
              </button>

            </div>


            <div className="audit-detail-grid">

              <div>
                <span>
                  {T.user}
                </span>
                <strong>
                  {selected.username ||
                    selected.userId ||
                    '—'}
                </strong>
              </div>

              <div>
                <span>
                  {T.ip}
                </span>
                <strong>
                  {selected.ipAddress ||
                    '—'}
                </strong>
              </div>

              <div>
                <span>
                  {T.status}
                </span>
                <strong>
                  {statusLabel(
                    selected.status,
                  )}
                </strong>
              </div>

              <div>
                <span>
                  Table
                </span>
                <strong>
                  {selected.table}
                </strong>
              </div>

            </div>


            <div className="audit-detail-section">

              <h4>
                {T.userAgent}
              </h4>

              <p>
                {selected.userAgent ||
                  '—'}
              </p>

            </div>


            <div className="audit-detail-section">

              <h4>
                {T.raw}
              </h4>

              <div className="audit-raw-table">

                {Object.entries(
                  selected.raw ?? {},
                ).map(
                  (
                    [
                      key,
                      value,
                    ],
                  ) => (

                    <div
                      key={key}
                    >

                      <span>
                        {key}
                      </span>

                      <strong>
                        {value ||
                          '—'}
                      </strong>

                    </div>
                  ),
                )}

              </div>

            </div>


            <div className="audit-modal-actions">

              <button
                type="button"
                className="audit-button secondary"
                onClick={() =>
                  setSelected(
                    null,
                  )
                }
              >
                {T.close}
              </button>

            </div>

          </div>

        </div>
      )}

    </section>
  )
}


export default AuditLogsPanel