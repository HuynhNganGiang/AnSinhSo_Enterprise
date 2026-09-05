import { useEffect, useMemo, useState } from 'react'

type PaymentPointSource =
  | 'REAL MAP'
  | 'DEMO LOCAL'

type CoordinateSource =
  | 'REAL STORED'
  | 'DEMO COORDINATE'

type PaymentPointItem = {
  id: string
  code: string
  name: string
  description: string
  address: string
  latitude: number
  longitude: number
  displayOrder: number
  active: boolean
  source: PaymentPointSource
  coordinateSource: CoordinateSource
}

type PaymentPointForm = {
  code: string
  name: string
  description: string
  address: string
  latitude: string
  longitude: string
  displayOrder: string
  active: boolean
}

type UnknownRecord =
  Record<string, unknown>

const STORAGE_KEY =
  'ansinhso.demo.payment-points.v1'

const PAGE_SIZE =
  8

const defaultDemoPoints:
  PaymentPointItem[] =
[
  {
    id: 'demo-payment-1',
    code: 'DCT-001',
    name: 'Điểm chi trả trung tâm xã',
    description:
      'Điểm chi trả mô phỏng tại khu vực trung tâm xã Sông Lũy.',
    address:
      'Khu vực trung tâm xã Sông Lũy',
    latitude: 11.210113,
    longitude: 108.321720,
    displayOrder: 1,
    active: true,
    source: 'DEMO LOCAL',
    coordinateSource:
      'DEMO COORDINATE',
  },
  {
    id: 'demo-payment-2',
    code: 'DCT-002',
    name: 'Điểm chi trả khu vực Bắc',
    description:
      'Điểm chi trả mô phỏng phục vụ các hộ khu vực phía Bắc.',
    address:
      'Khu vực phía Bắc xã Sông Lũy',
    latitude: 11.224400,
    longitude: 108.318900,
    displayOrder: 2,
    active: true,
    source: 'DEMO LOCAL',
    coordinateSource:
      'DEMO COORDINATE',
  },
  {
    id: 'demo-payment-3',
    code: 'DCT-003',
    name: 'Điểm chi trả khu vực Nam',
    description:
      'Điểm chi trả mô phỏng phục vụ các hộ khu vực phía Nam.',
    address:
      'Khu vực phía Nam xã Sông Lũy',
    latitude: 11.196800,
    longitude: 108.324500,
    displayOrder: 3,
    active: true,
    source: 'DEMO LOCAL',
    coordinateSource:
      'DEMO COORDINATE',
  },
  {
    id: 'demo-payment-4',
    code: 'DCT-004',
    name: 'Điểm chi trả khu vực Đông',
    description:
      'Điểm chi trả mô phỏng phục vụ các hộ khu vực phía Đông.',
    address:
      'Khu vực phía Đông xã Sông Lũy',
    latitude: 11.210900,
    longitude: 108.339500,
    displayOrder: 4,
    active: true,
    source: 'DEMO LOCAL',
    coordinateSource:
      'DEMO COORDINATE',
  },
  {
    id: 'demo-payment-5',
    code: 'DCT-005',
    name: 'Điểm chi trả lưu động',
    description:
      'Điểm chi trả lưu động mô phỏng phục vụ các trường hợp cần hỗ trợ tại địa bàn.',
    address:
      'Địa bàn xã Sông Lũy',
    latitude: 11.207200,
    longitude: 108.306500,
    displayOrder: 5,
    active: true,
    source: 'DEMO LOCAL',
    coordinateSource:
      'DEMO COORDINATE',
  },
]

function asRecord(
  value: unknown,
): UnknownRecord | null {
  if (
    value !== null &&
    typeof value === 'object' &&
    !Array.isArray(value)
  ) {
    return value as UnknownRecord
  }

  return null
}

function readText(
  row: UnknownRecord,
  ...keys: string[]
) {
  for (const key of keys) {
    const value =
      row[key]

    if (
      typeof value === 'string' &&
      value.trim()
    ) {
      return value.trim()
    }
  }

  return ''
}

function readNumber(
  row: UnknownRecord,
  ...keys: string[]
) {
  for (const key of keys) {
    const value =
      row[key]

    if (
      typeof value === 'number' &&
      Number.isFinite(value)
    ) {
      return value
    }

    if (typeof value === 'string') {
      const number =
        Number(value)

      if (Number.isFinite(number))
        return number
    }
  }

  return 0
}

function extractRows(
  value: unknown,
): unknown[] {
  if (Array.isArray(value))
    return value

  const root =
    asRecord(value)

  if (!root)
    return []

  for (
    const key of [
      'items',
      'markers',
      'data',
    ]
  ) {
    const candidate =
      root[key]

    if (Array.isArray(candidate))
      return candidate
  }

  const level1 =
    asRecord(root.data)

  if (level1) {
    for (
      const key of [
        'items',
        'markers',
        'data',
      ]
    ) {
      const candidate =
        level1[key]

      if (Array.isArray(candidate))
        return candidate
    }

    const level2 =
      asRecord(level1.data)

    if (level2) {
      for (
        const key of [
          'items',
          'markers',
          'data',
        ]
      ) {
        const candidate =
          level2[key]

        if (Array.isArray(candidate))
          return candidate
      }
    }
  }

  return []
}

function parsePopup(
  popup: string,
) {
  const lines =
    popup
      .split('\n')
      .map(item => item.trim())

  const addressLine =
    lines.find(
      item =>
        item
          .toLowerCase()
          .startsWith(
            'địa chỉ:',
          ),
    )

  const descriptionLine =
    lines.find(
      item =>
        item
          .toLowerCase()
          .startsWith(
            'mô tả:',
          ),
    )

  return {
    address:
      addressLine
        ?.replace(
          /^địa chỉ:\s*/i,
          '',
        ) ?? '',

    description:
      descriptionLine
        ?.replace(
          /^mô tả:\s*/i,
          '',
        ) ?? '',
  }
}

function toRealPoint(
  value: unknown,
  index: number,
): PaymentPointItem | null {
  const row =
    asRecord(value)

  if (!row)
    return null

  const markerType =
    readText(
      row,
      'markerType',
      'MarkerType',
      'type',
      'Type',
    )

  if (
    markerType &&
    markerType.toLowerCase() !==
      'paymentpoint'
  ) {
    return null
  }

  const name =
    readText(
      row,
      'name',
      'Name',
      'popupTitle',
      'PopupTitle',
    )

  const latitude =
    readNumber(
      row,
      'latitude',
      'Latitude',
      'lat',
      'Lat',
    )

  const longitude =
    readNumber(
      row,
      'longitude',
      'Longitude',
      'lng',
      'Lng',
    )

  if (
    !name ||
    !Number.isFinite(latitude) ||
    !Number.isFinite(longitude) ||
    latitude === 0 ||
    longitude === 0
  ) {
    return null
  }

  const popup =
    readText(
      row,
      'popupContent',
      'PopupContent',
    )

  const parsed =
    parsePopup(popup)

  const status =
    readText(
      row,
      'status',
      'Status',
    )

  return {
    id:
      readText(
        row,
        'id',
        'Id',
      ) ||
      `real-payment-point-${index + 1}`,

    code:
      readText(
        row,
        'code',
        'Code',
      ) ||
      `REAL-${String(
        index + 1,
      ).padStart(3, '0')}`,

    name,

    description:
      parsed.description,

    address:
      parsed.address,

    latitude,
    longitude,

    displayOrder:
      index + 1,

    active:
      !status ||
      status
        .toLowerCase()
        .includes('active'),

    source:
      'REAL MAP',

    coordinateSource:
      'REAL STORED',
  }
}

function readStoredDemo():
  PaymentPointItem[] | null {
  try {
    const raw =
      localStorage.getItem(
        STORAGE_KEY,
      )

    if (raw === null)
      return null

    const parsed =
      JSON.parse(raw)

    if (!Array.isArray(parsed))
      return null

    return parsed as PaymentPointItem[]
  } catch {
    return null
  }
}

function persistDemo(
  rows: PaymentPointItem[],
) {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify(rows),
  )
}

function PaymentPointsCategoryPanel() {
  const [
    realPoints,
    setRealPoints,
  ] =
    useState<PaymentPointItem[]>([])

  const [
    demoPoints,
    setDemoPoints,
  ] =
    useState<PaymentPointItem[]>(
      () =>
        readStoredDemo() ??
        [],
    )

  const [
    loading,
    setLoading,
  ] =
    useState(true)

  const [
    message,
    setMessage,
  ] =
    useState(
      'Đang kiểm tra điểm chi trả production...',
    )

  const [
    keyword,
    setKeyword,
  ] =
    useState('')

  const [
    sourceFilter,
    setSourceFilter,
  ] =
    useState('all')

  const [
    statusFilter,
    setStatusFilter,
  ] =
    useState('all')

  const [
    page,
    setPage,
  ] =
    useState(1)

  const [
    creating,
    setCreating,
  ] =
    useState(false)

  const [
    selected,
    setSelected,
  ] =
    useState<PaymentPointItem | null>(
      null,
    )

  const [
    form,
    setForm,
  ] =
    useState<PaymentPointForm>({
      code: '',
      name: '',
      description: '',
      address: '',
      latitude: '11.210113',
      longitude: '108.321720',
      displayOrder: '1',
      active: true,
    })

  const ensureDemo =
    () => {
      const stored =
        readStoredDemo()

      if (stored === null) {
        persistDemo(
          defaultDemoPoints,
        )

        setDemoPoints(
          defaultDemoPoints,
        )
      } else {
        setDemoPoints(
          stored,
        )
      }
    }

  const load =
    async () => {
      setLoading(true)

      const token =
        localStorage.getItem(
          'accessToken',
        )

      const endpoints = [
        '/api/v1/map/markers?citizens=false&households=false&paymentPoints=true&welfare=false',
        '/api/map/markers?citizens=false&households=false&paymentPoints=true&welfare=false',
        '/api/v1/map?citizens=false&households=false&paymentPoints=true&welfare=false',
        '/api/map?citizens=false&households=false&paymentPoints=true&welfare=false',
      ]

      try {
        for (
          const endpoint
          of endpoints
        ) {
          try {
            const response =
              await fetch(
                endpoint,
                {
                  headers:
                    token
                      ? {
                          Authorization:
                            `Bearer ${token}`,
                        }
                      : undefined,
                },
              )

            if (!response.ok)
              continue

            const body =
              (await response.json()) as unknown

            const mapped =
              extractRows(body)
                .map(
                  toRealPoint,
                )
                .filter(
                  (
                    item,
                  ): item is PaymentPointItem =>
                    item !== null,
                )

            if (
              mapped.length > 0
            ) {
              setRealPoints(
                mapped,
              )

              const stored =
                readStoredDemo()

              if (stored !== null) {
                setDemoPoints(
                  stored,
                )
              }

              setMessage(
                `${mapped.length} điểm chi trả đang lấy từ dữ liệu Map production.`,
              )

              return
            }
          } catch {
            // Try the next known Map endpoint.
          }
        }

        setRealPoints([])
        ensureDemo()

        setMessage(
          'Production chưa có điểm chi trả khả dụng trên bản đồ. Đang dùng 5 điểm DEMO LOCAL; không ghi SQL production.',
        )
      } finally {
        setLoading(false)
      }
    }

  useEffect(() => {
    void load()
  }, [])

  const allPoints =
    useMemo(
      () => [
        ...realPoints,
        ...demoPoints,
      ],
      [
        realPoints,
        demoPoints,
      ],
    )

  const filtered =
    useMemo(
      () => {
        const q =
          keyword
            .trim()
            .toLowerCase()

        return allPoints.filter(
          item => {
            const keywordOk =
              !q ||
              item.code
                .toLowerCase()
                .includes(q) ||
              item.name
                .toLowerCase()
                .includes(q) ||
              item.address
                .toLowerCase()
                .includes(q) ||
              item.description
                .toLowerCase()
                .includes(q)

            const sourceOk =
              sourceFilter ===
                'all' ||
              (
                sourceFilter ===
                  'real' &&
                item.source ===
                  'REAL MAP'
              ) ||
              (
                sourceFilter ===
                  'demo' &&
                item.source ===
                  'DEMO LOCAL'
              )

            const statusOk =
              statusFilter ===
                'all' ||
              (
                statusFilter ===
                  'active' &&
                item.active
              ) ||
              (
                statusFilter ===
                  'inactive' &&
                !item.active
              )

            return (
              keywordOk &&
              sourceOk &&
              statusOk
            )
          },
        )
      },
      [
        allPoints,
        keyword,
        sourceFilter,
        statusFilter,
      ],
    )

  const totalPages =
    Math.max(
      1,
      Math.ceil(
        filtered.length /
        PAGE_SIZE,
      ),
    )

  const visible =
    filtered.slice(
      (page - 1) *
        PAGE_SIZE,
      page *
        PAGE_SIZE,
    )

  const activeCount =
    allPoints.filter(
      item =>
        item.active,
    ).length

  const updateDemo =
    (
      rows:
        PaymentPointItem[],
    ) => {
      setDemoPoints(rows)
      persistDemo(rows)
    }

  const openCreate =
    () => {
      setCreating(true)
      setSelected(null)

      setForm({
        code:
          `DCT-${String(
            demoPoints.length + 1,
          ).padStart(3, '0')}`,

        name: '',
        description:
          'Điểm chi trả giả lập phục vụ trình diễn.',
        address:
          'Xã Sông Lũy',
        latitude:
          '11.210113',
        longitude:
          '108.321720',
        displayOrder:
          String(
            demoPoints.length + 1,
          ),
        active: true,
      })
    }

  const openDetail =
    (
      item:
        PaymentPointItem,
    ) => {
      setCreating(false)
      setSelected(item)

      setForm({
        code:
          item.code,
        name:
          item.name,
        description:
          item.description,
        address:
          item.address,
        latitude:
          String(
            item.latitude,
          ),
        longitude:
          String(
            item.longitude,
          ),
        displayOrder:
          String(
            item.displayOrder,
          ),
        active:
          item.active,
      })
    }

  const closeModal =
    () => {
      setCreating(false)
      setSelected(null)
    }

  const saveDemo =
    () => {
      const code =
        form.code
          .trim()
          .toUpperCase()

      const name =
        form.name.trim()

      const latitude =
        Number(
          form.latitude,
        )

      const longitude =
        Number(
          form.longitude,
        )

      const displayOrder =
        Number(
          form.displayOrder,
        )

      if (!code || !name) {
        window.alert(
          'Vui lòng nhập mã và tên điểm chi trả.',
        )
        return
      }

      if (
        !Number.isFinite(latitude) ||
        latitude < -90 ||
        latitude > 90 ||
        !Number.isFinite(longitude) ||
        longitude < -180 ||
        longitude > 180
      ) {
        window.alert(
          'Tọa độ không hợp lệ.',
        )
        return
      }

      if (
        !Number.isFinite(
          displayOrder,
        ) ||
        displayOrder < 0
      ) {
        window.alert(
          'Thứ tự hiển thị không hợp lệ.',
        )
        return
      }

      const duplicate =
        allPoints.some(
          item =>
            item.code
              .toLowerCase() ===
              code.toLowerCase() &&
            item.id !==
              selected?.id,
        )

      if (duplicate) {
        window.alert(
          'Mã điểm chi trả đã tồn tại.',
        )
        return
      }

      if (creating) {
        const item:
          PaymentPointItem =
        {
          id:
            `demo-payment-${Date.now()}`,
          code,
          name,
          description:
            form.description.trim(),
          address:
            form.address.trim(),
          latitude,
          longitude,
          displayOrder,
          active:
            form.active,
          source:
            'DEMO LOCAL',
          coordinateSource:
            'DEMO COORDINATE',
        }

        updateDemo([
          item,
          ...demoPoints,
        ])
      } else if (
        selected?.source ===
        'DEMO LOCAL'
      ) {
        updateDemo(
          demoPoints.map(
            item =>
              item.id ===
              selected.id
                ? {
                    ...item,
                    code,
                    name,
                    description:
                      form.description.trim(),
                    address:
                      form.address.trim(),
                    latitude,
                    longitude,
                    displayOrder,
                    active:
                      form.active,
                  }
                : item,
          ),
        )
      }

      closeModal()
    }

  const toggleDemo =
    (
      item:
        PaymentPointItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      updateDemo(
        demoPoints.map(
          row =>
            row.id ===
            item.id
              ? {
                  ...row,
                  active:
                    !row.active,
                }
              : row,
        ),
      )
    }

  const deleteDemo =
    (
      item:
        PaymentPointItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa điểm chi trả giả lập "${item.name}"?`,
        )
      ) {
        return
      }

      updateDemo(
        demoPoints.filter(
          row =>
            row.id !==
            item.id,
        ),
      )
    }

  const realOnly =
    selected?.source ===
    'REAL MAP'

  return (
    <section className="category-workbench">

      <div className="category-workbench-heading">

        <div className="category-title-line">

          <span className="admin-feature-icon cyan small">
            <svg
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.8"
              aria-hidden="true"
            >
              <rect
                x="3"
                y="6"
                width="18"
                height="12"
                rx="2"
              />
              <path d="M3 10h18" />
              <path d="M7 14h4" />
            </svg>
          </span>

          <div>
            <h2>
              Điểm chi trả
            </h2>

            <p>
              {message}
            </p>
          </div>

        </div>


        <div className="category-heading-actions">

          <button
            type="button"
            className="category-button secondary"
            disabled={loading}
            onClick={() =>
              void load()
            }
          >
            {loading
              ? 'Đang tải...'
              : '↻ Làm mới REAL'}
          </button>

          <button
            type="button"
            className="category-button secondary"
            onClick={() => {
              window.location.href =
                '/map'
            }}
          >
            Xem trên bản đồ
          </button>

          <button
            type="button"
            className="category-button primary"
            onClick={openCreate}
          >
            + Thêm điểm DEMO
          </button>

        </div>

      </div>


      <div className="category-stat-grid">

        <article>
          <span>
            Điểm production
          </span>

          <strong>
            {realPoints.length}
          </strong>

          <small>
            REAL MAP
          </small>
        </article>

        <article>
          <span>
            Điểm giả lập
          </span>

          <strong>
            {demoPoints.length}
          </strong>

          <small>
            localStorage
          </small>
        </article>

        <article>
          <span>
            Đang hoạt động
          </span>

          <strong>
            {activeCount}
          </strong>

          <small>
            REAL + DEMO
          </small>
        </article>

        <article>
          <span>
            Có tọa độ
          </span>

          <strong>
            {allPoints.filter(
              item =>
                Number.isFinite(
                  item.latitude,
                ) &&
                Number.isFinite(
                  item.longitude,
                ),
            ).length}
          </strong>

          <small>
            Phân biệt nguồn
          </small>
        </article>

      </div>


      <div className="category-toolbar">

        <input
          value={keyword}
          placeholder="Tìm mã, tên, địa chỉ điểm chi trả..."
          onChange={event => {
            setKeyword(
              event.target.value,
            )
            setPage(1)
          }}
        />

        <select
          value={sourceFilter}
          onChange={event => {
            setSourceFilter(
              event.target.value,
            )
            setPage(1)
          }}
        >
          <option value="all">
            Tất cả nguồn
          </option>

          <option value="real">
            REAL MAP
          </option>

          <option value="demo">
            DEMO LOCAL
          </option>
        </select>

        <select
          value={statusFilter}
          onChange={event => {
            setStatusFilter(
              event.target.value,
            )
            setPage(1)
          }}
        >
          <option value="all">
            Tất cả trạng thái
          </option>

          <option value="active">
            Đang hoạt động
          </option>

          <option value="inactive">
            Tạm ngừng
          </option>
        </select>

      </div>


      <div className="category-table-wrap">

        <table className="category-table">

          <thead>
            <tr>
              <th>STT</th>
              <th>Mã điểm</th>
              <th>Tên điểm chi trả</th>
              <th>Địa chỉ</th>
              <th>Tọa độ</th>
              <th>Thứ tự</th>
              <th>Nguồn</th>
              <th>Trạng thái</th>
              <th>Thao tác</th>
            </tr>
          </thead>

          <tbody>

            {visible.map(
              (
                item,
                index,
              ) => (
                <tr key={item.id}>

                  <td>
                    {(page - 1) *
                      PAGE_SIZE +
                      index +
                      1}
                  </td>

                  <td>
                    <strong>
                      {item.code}
                    </strong>
                  </td>

                  <td>
                    <strong>
                      {item.name}
                    </strong>

                    <div
                      style={{
                        marginTop: '3px',
                        color: '#94a3b8',
                        fontSize: '9px',
                      }}
                    >
                      {item.description ||
                        '—'}
                    </div>
                  </td>

                  <td>
                    {item.address ||
                      '—'}
                  </td>

                  <td>
                    <div>
                      {item.latitude.toFixed(6)}
                    </div>

                    <div>
                      {item.longitude.toFixed(6)}
                    </div>

                    <small
                      style={{
                        color:
                          item.coordinateSource ===
                          'REAL STORED'
                            ? '#15803d'
                            : '#c2410c',
                      }}
                    >
                      {item.coordinateSource}
                    </small>
                  </td>

                  <td>
                    {item.displayOrder}
                  </td>

                  <td>
                    <span
                      className={
                        `category-source-badge ${
                          item.source ===
                          'REAL MAP'
                            ? 'real-api'
                            : 'demo'
                        }`
                      }
                    >
                      {item.source}
                    </span>
                  </td>

                  <td>
                    <span
                      className={
                        `category-state-badge ${
                          item.active
                            ? 'active'
                            : 'inactive'
                        }`
                      }
                    >
                      {item.active
                        ? 'Hoạt động'
                        : 'Tạm ngừng'}
                    </span>
                  </td>

                  <td>
                    <div className="category-row-actions">

                      <button
                        type="button"
                        onClick={() =>
                          openDetail(
                            item,
                          )
                        }
                      >
                        {item.source ===
                        'DEMO LOCAL'
                          ? 'Xem / Sửa'
                          : 'Xem'}
                      </button>

                      {item.source ===
                        'DEMO LOCAL' && (
                        <>
                          <button
                            type="button"
                            onClick={() =>
                              toggleDemo(
                                item,
                              )
                            }
                          >
                            {item.active
                              ? 'Tắt'
                              : 'Bật'}
                          </button>

                          <button
                            type="button"
                            className="danger"
                            onClick={() =>
                              deleteDemo(
                                item,
                              )
                            }
                          >
                            Xóa
                          </button>
                        </>
                      )}

                    </div>
                  </td>

                </tr>
              ),
            )}


            {!loading &&
              visible.length ===
                0 && (
                <tr>
                  <td
                    colSpan={9}
                    className="category-empty"
                  >
                    Không có điểm chi trả phù hợp
                    với bộ lọc.
                  </td>
                </tr>
              )}


            {loading && (
              <tr>
                <td
                  colSpan={9}
                  className="category-empty"
                >
                  Đang tải điểm chi trả...
                </td>
              </tr>
            )}

          </tbody>
        </table>

      </div>


      <div className="category-pagination">

        <span>
          {filtered.length} điểm chi trả
        </span>

        <div>

          <button
            type="button"
            disabled={page <= 1}
            onClick={() =>
              setPage(
                Math.max(
                  1,
                  page - 1,
                ),
              )
            }
          >
            ← Trước
          </button>

          <span>
            Trang {page}/{totalPages}
          </span>

          <button
            type="button"
            disabled={
              page >= totalPages
            }
            onClick={() =>
              setPage(
                Math.min(
                  totalPages,
                  page + 1,
                ),
              )
            }
          >
            Sau →
          </button>

        </div>

      </div>


      <div className="category-safety-note">

        <strong>
          Bảo vệ production:
        </strong>

        <span>
          Điểm REAL trên Map chỉ được đọc khi
          backend có Location và trạng thái Active.
          Nếu production chưa có điểm phù hợp,
          hệ thống dùng 5 điểm DEMO LOCAL.
          Thêm/Sửa/Bật-Tắt/Xóa DEMO chỉ lưu
          localStorage và không ghi SQL Server.
        </span>

      </div>


      {(creating || selected) && (

        <div className="category-modal-overlay">

          <div className="category-modal">

            <div className="category-modal-heading">

              <div>
                <h2>
                  {creating
                    ? 'Thêm điểm chi trả DEMO'
                    : realOnly
                      ? 'Chi tiết điểm chi trả REAL'
                      : 'Chỉnh sửa điểm chi trả DEMO'}
                </h2>

                <p>
                  {realOnly
                    ? 'Dữ liệu production ở chế độ chỉ đọc.'
                    : 'Dữ liệu chỉ lưu trong trình duyệt.'}
                </p>
              </div>

              <button
                type="button"
                className="category-modal-close"
                onClick={closeModal}
              >
                ×
              </button>

            </div>


            <div className="category-form-grid">

              <label>
                <span>
                  Mã điểm
                </span>

                <input
                  disabled={realOnly}
                  value={form.code}
                  onChange={event =>
                    setForm({
                      ...form,
                      code:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Tên điểm chi trả
                </span>

                <input
                  disabled={realOnly}
                  value={form.name}
                  onChange={event =>
                    setForm({
                      ...form,
                      name:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label className="full">
                <span>
                  Địa chỉ
                </span>

                <input
                  disabled={realOnly}
                  value={form.address}
                  onChange={event =>
                    setForm({
                      ...form,
                      address:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label className="full">
                <span>
                  Mô tả
                </span>

                <textarea
                  disabled={realOnly}
                  value={
                    form.description
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      description:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Vĩ độ
                </span>

                <input
                  type="number"
                  step="0.000001"
                  disabled={realOnly}
                  value={
                    form.latitude
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      latitude:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Kinh độ
                </span>

                <input
                  type="number"
                  step="0.000001"
                  disabled={realOnly}
                  value={
                    form.longitude
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      longitude:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Thứ tự hiển thị
                </span>

                <input
                  type="number"
                  min="0"
                  disabled={realOnly}
                  value={
                    form.displayOrder
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      displayOrder:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label className="category-switch-field">

                <input
                  type="checkbox"
                  disabled={realOnly}
                  checked={
                    form.active
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      active:
                        event.target.checked,
                    })
                  }
                />

                <span>
                  Đang hoạt động
                </span>

              </label>

            </div>


            <div className="category-modal-actions">

              <button
                type="button"
                className="category-button secondary"
                onClick={closeModal}
              >
                Đóng
              </button>

              <button
                type="button"
                className="category-button secondary"
                onClick={() => {
                  window.location.href =
                    '/map'
                }}
              >
                Xem Bản đồ số
              </button>

              {!realOnly && (
                <button
                  type="button"
                  className="category-button primary"
                  onClick={saveDemo}
                >
                  Lưu DEMO
                </button>
              )}

            </div>

          </div>

        </div>
      )}

    </section>
  )
}

export default PaymentPointsCategoryPanel