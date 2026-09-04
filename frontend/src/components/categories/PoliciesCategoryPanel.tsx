import { useEffect, useMemo, useState } from 'react'

type PolicySource =
  | 'REAL API'
  | 'DEMO LOCAL'

type PolicyStatus =
  | 'Active'
  | 'Closed'

type PolicyItem = {
  id: string
  name: string
  description: string
  amount: number
  beneficiaryGroup: string
  paymentCycle: string
  legalBasis: string
  status: PolicyStatus
  source: PolicySource
}

type PolicyForm = {
  name: string
  description: string
  amount: string
  beneficiaryGroup: string
  paymentCycle: string
  legalBasis: string
  active: boolean
}

type UnknownRecord =
  Record<string, unknown>

const STORAGE_KEY =
  'ansinhso.demo.policies.v1'

const PAGE_SIZE =
  7

const defaultDemoPolicies:
  PolicyItem[] =
[
  {
    id: 'demo-policy-elderly',
    name: 'Trợ cấp người cao tuổi',
    description:
      'Chính sách mô phỏng hỗ trợ người cao tuổi thuộc diện an sinh.',
    amount: 500000,
    beneficiaryGroup: 'Người cao tuổi',
    paymentCycle: 'Hàng tháng',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Active',
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-policy-disability',
    name: 'Trợ cấp người khuyết tật',
    description:
      'Chính sách mô phỏng hỗ trợ người khuyết tật cần bảo trợ.',
    amount: 750000,
    beneficiaryGroup: 'Người khuyết tật',
    paymentCycle: 'Hàng tháng',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Active',
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-policy-children',
    name: 'Hỗ trợ trẻ em hoàn cảnh khó khăn',
    description:
      'Hỗ trợ mô phỏng dành cho trẻ em có hoàn cảnh đặc biệt hoặc khó khăn.',
    amount: 600000,
    beneficiaryGroup: 'Trẻ em có hoàn cảnh khó khăn',
    paymentCycle: 'Hàng tháng',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Active',
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-policy-poor-household',
    name: 'Hỗ trợ hộ nghèo',
    description:
      'Chính sách mô phỏng hỗ trợ hộ thuộc phân loại nghèo.',
    amount: 1000000,
    beneficiaryGroup: 'Hộ nghèo',
    paymentCycle: 'Theo đợt',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Active',
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-policy-near-poor',
    name: 'Hỗ trợ hộ cận nghèo',
    description:
      'Chính sách mô phỏng hỗ trợ hộ thuộc phân loại cận nghèo.',
    amount: 750000,
    beneficiaryGroup: 'Hộ cận nghèo',
    paymentCycle: 'Theo đợt',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Active',
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-policy-emergency',
    name: 'Trợ giúp xã hội khẩn cấp',
    description:
      'Hỗ trợ mô phỏng cho trường hợp phát sinh khó khăn đột xuất.',
    amount: 1500000,
    beneficiaryGroup: 'Đối tượng khó khăn khác',
    paymentCycle: 'Một lần',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Active',
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-policy-care',
    name: 'Hỗ trợ chăm sóc tại cộng đồng',
    description:
      'Chính sách mô phỏng phục vụ chăm sóc đối tượng bảo trợ tại cộng đồng.',
    amount: 800000,
    beneficiaryGroup: 'Bảo trợ xã hội',
    paymentCycle: 'Hàng tháng',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Active',
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-policy-funeral',
    name: 'Hỗ trợ mai táng',
    description:
      'Khoản hỗ trợ mô phỏng cho tình huống đủ điều kiện hưởng hỗ trợ mai táng.',
    amount: 3000000,
    beneficiaryGroup: 'Bảo trợ xã hội',
    paymentCycle: 'Một lần',
    legalBasis: 'Mô phỏng phục vụ demo',
    status: 'Closed',
    source: 'DEMO LOCAL',
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

function readStoredDemo():
  PolicyItem[] | null {
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

    return parsed as PolicyItem[]
  } catch {
    return null
  }
}

function persistDemo(
  items: PolicyItem[],
) {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify(items),
  )
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

    if (
      typeof value === 'string'
    ) {
      const parsed =
        Number(value)

      if (Number.isFinite(parsed))
        return parsed
    }

    const nested =
      asRecord(value)

    if (nested) {
      const amount =
        nested.amount ??
        nested.Amount

      if (
        typeof amount === 'number' &&
        Number.isFinite(amount)
      ) {
        return amount
      }

      if (
        typeof amount === 'string'
      ) {
        const parsed =
          Number(amount)

        if (Number.isFinite(parsed))
          return parsed
      }
    }
  }

  return 0
}

function readStatus(
  row: UnknownRecord,
): PolicyStatus {
  const raw =
    row.status ??
    row.Status ??
    row.statusId ??
    row.StatusId

  if (typeof raw === 'number') {
    return raw === 2
      ? 'Closed'
      : 'Active'
  }

  if (typeof raw === 'string') {
    const normalized =
      raw.toLowerCase()

    if (
      normalized.includes('closed') ||
      normalized.includes('inactive') ||
      normalized === '2'
    ) {
      return 'Closed'
    }
  }

  const nested =
    asRecord(raw)

  if (nested) {
    const id =
      nested.id ??
      nested.Id

    const name =
      nested.name ??
      nested.Name

    if (
      id === 2 ||
      (
        typeof name === 'string' &&
        name.toLowerCase()
          .includes('closed')
      )
    ) {
      return 'Closed'
    }
  }

  return 'Active'
}

function extractRows(
  body: unknown,
): unknown[] {
  if (Array.isArray(body))
    return body

  const root =
    asRecord(body)

  if (!root)
    return []

  if (Array.isArray(root.data))
    return root.data

  if (Array.isArray(root.items))
    return root.items

  const level1 =
    asRecord(root.data)

  if (!level1)
    return []

  if (Array.isArray(level1.data))
    return level1.data

  if (Array.isArray(level1.items))
    return level1.items

  const level2 =
    asRecord(level1.data)

  if (!level2)
    return []

  if (Array.isArray(level2.items))
    return level2.items

  if (Array.isArray(level2.data))
    return level2.data

  return []
}

function mapRealPolicy(
  value: unknown,
  index: number,
): PolicyItem | null {
  const row =
    asRecord(value)

  if (!row)
    return null

  const name =
    readText(
      row,
      'name',
      'Name',
    )

  if (!name)
    return null

  return {
    id:
      readText(
        row,
        'id',
        'Id',
      ) ||
      `real-policy-${index}`,

    name,

    description:
      readText(
        row,
        'description',
        'Description',
      ),

    amount:
      readNumber(
        row,
        'amount',
        'Amount',
      ),

    beneficiaryGroup: '',
    paymentCycle: '',
    legalBasis: '',
    status:
      readStatus(row),
    source:
      'REAL API',
  }
}

function formatMoney(
  amount: number,
) {
  return amount.toLocaleString(
    'vi-VN',
  ) + ' ₫'
}

function PoliciesCategoryPanel() {
  const [
    realPolicies,
    setRealPolicies,
  ] =
    useState<PolicyItem[]>([])

  const [
    demoPolicies,
    setDemoPolicies,
  ] =
    useState<PolicyItem[]>(
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
      'Đang kiểm tra chính sách production...',
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
    useState<PolicyItem | null>(
      null,
    )

  const [
    form,
    setForm,
  ] =
    useState<PolicyForm>({
      name: '',
      description: '',
      amount: '500000',
      beneficiaryGroup: '',
      paymentCycle: 'Hàng tháng',
      legalBasis:
        'Mô phỏng phục vụ demo',
      active: true,
    })


  const useDemoFallback =
    () => {
      setRealPolicies([])

      const stored =
        readStoredDemo()

      if (stored === null) {
        persistDemo(
          defaultDemoPolicies,
        )

        setDemoPolicies(
          defaultDemoPolicies,
        )
      } else {
        setDemoPolicies(
          stored,
        )
      }

      setMessage(
        'Danh sách Policy production hiện chưa trả dữ liệu sử dụng được. Đang dùng DEMO LOCAL, không ghi SQL production.',
      )
    }


  const load =
    async () => {
      setLoading(true)

      const token =
        localStorage.getItem(
          'accessToken',
        )

      const endpoints = [
        '/api/policies?pageNumber=1&pageSize=200',
        '/api/v1/policies?pageNumber=1&pageSize=200',
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
                .map(mapRealPolicy)
                .filter(
                  (
                    item,
                  ): item is PolicyItem =>
                    item !== null,
                )

            if (mapped.length > 0) {
              setRealPolicies(
                mapped,
              )

              const stored =
                readStoredDemo()

              if (stored !== null) {
                setDemoPolicies(
                  stored,
                )
              }

              setMessage(
                `${mapped.length} chính sách đang lấy trực tiếp từ API production.`,
              )

              return
            }
          } catch {
            // Try next endpoint.
          }
        }

        useDemoFallback()
      } finally {
        setLoading(false)
      }
    }


  useEffect(() => {
    void load()
  }, [])


  const allPolicies =
    useMemo(
      () => [
        ...realPolicies,
        ...demoPolicies,
      ],
      [
        realPolicies,
        demoPolicies,
      ],
    )


  const filtered =
    useMemo(
      () => {
        const q =
          keyword
            .trim()
            .toLowerCase()

        return allPolicies.filter(
          item => {
            const keywordOk =
              !q ||
              item.name
                .toLowerCase()
                .includes(q) ||
              item.description
                .toLowerCase()
                .includes(q) ||
              item.beneficiaryGroup
                .toLowerCase()
                .includes(q) ||
              item.legalBasis
                .toLowerCase()
                .includes(q)

            const sourceOk =
              sourceFilter === 'all' ||
              (
                sourceFilter === 'real' &&
                item.source ===
                  'REAL API'
              ) ||
              (
                sourceFilter === 'demo' &&
                item.source ===
                  'DEMO LOCAL'
              )

            const statusOk =
              statusFilter === 'all' ||
              (
                statusFilter === 'active' &&
                item.status ===
                  'Active'
              ) ||
              (
                statusFilter === 'closed' &&
                item.status ===
                  'Closed'
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
        allPolicies,
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


  const totalAmount =
    allPolicies.reduce(
      (
        sum,
        item,
      ) =>
        sum +
        item.amount,
      0,
    )


  const activeCount =
    allPolicies.filter(
      item =>
        item.status ===
          'Active',
    ).length


  const updateDemo =
    (
      next:
        PolicyItem[],
    ) => {
      setDemoPolicies(next)
      persistDemo(next)
    }


  const openCreate =
    () => {
      setSelected(null)
      setCreating(true)

      setForm({
        name: '',
        description:
          'Chính sách giả lập phục vụ trình diễn.',
        amount: '500000',
        beneficiaryGroup:
          'Bảo trợ xã hội',
        paymentCycle:
          'Hàng tháng',
        legalBasis:
          'Mô phỏng phục vụ demo',
        active: true,
      })
    }


  const openDetail =
    (
      item: PolicyItem,
    ) => {
      setCreating(false)
      setSelected(item)

      setForm({
        name:
          item.name,
        description:
          item.description,
        amount:
          String(
            item.amount,
          ),
        beneficiaryGroup:
          item.beneficiaryGroup,
        paymentCycle:
          item.paymentCycle,
        legalBasis:
          item.legalBasis,
        active:
          item.status ===
          'Active',
      })
    }


  const closeModal =
    () => {
      setCreating(false)
      setSelected(null)
    }


  const saveDemo =
    () => {
      const name =
        form.name.trim()

      const amount =
        Number(
          form.amount,
        )

      if (!name) {
        window.alert(
          'Vui lòng nhập tên chính sách.',
        )
        return
      }

      if (
        !Number.isFinite(amount) ||
        amount < 0
      ) {
        window.alert(
          'Mức trợ cấp không hợp lệ.',
        )
        return
      }

      const duplicate =
        allPolicies.some(
          item =>
            item.name
              .trim()
              .toLowerCase() ===
              name.toLowerCase() &&
            item.id !==
              selected?.id,
        )

      if (duplicate) {
        window.alert(
          'Tên chính sách đã tồn tại.',
        )
        return
      }

      if (creating) {
        const item:
          PolicyItem =
        {
          id:
            `demo-policy-${Date.now()}`,
          name,
          description:
            form.description.trim(),
          amount,
          beneficiaryGroup:
            form.beneficiaryGroup.trim(),
          paymentCycle:
            form.paymentCycle.trim(),
          legalBasis:
            form.legalBasis.trim(),
          status:
            form.active
              ? 'Active'
              : 'Closed',
          source:
            'DEMO LOCAL',
        }

        updateDemo([
          item,
          ...demoPolicies,
        ])
      } else if (
        selected?.source ===
        'DEMO LOCAL'
      ) {
        updateDemo(
          demoPolicies.map(
            item =>
              item.id ===
              selected.id
                ? {
                    ...item,
                    name,
                    description:
                      form.description.trim(),
                    amount,
                    beneficiaryGroup:
                      form.beneficiaryGroup.trim(),
                    paymentCycle:
                      form.paymentCycle.trim(),
                    legalBasis:
                      form.legalBasis.trim(),
                    status:
                      form.active
                        ? 'Active'
                        : 'Closed',
                  }
                : item,
          ),
        )
      }

      closeModal()
    }


  const toggleDemo =
    (
      item: PolicyItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      updateDemo(
        demoPolicies.map(
          row =>
            row.id ===
            item.id
              ? {
                  ...row,
                  status:
                    row.status ===
                    'Active'
                      ? 'Closed'
                      : 'Active',
                }
              : row,
        ),
      )
    }


  const deleteDemo =
    (
      item: PolicyItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa chính sách giả lập "${item.name}"?`,
        )
      ) {
        return
      }

      updateDemo(
        demoPolicies.filter(
          row =>
            row.id !==
            item.id,
        ),
      )
    }


  const copyRealToDemo =
    () => {
      if (
        !selected ||
        selected.source !==
          'REAL API'
      ) {
        return
      }

      const stamp =
        Date.now()

      updateDemo([
        {
          ...selected,
          id:
            `demo-policy-copy-${stamp}`,
          name:
            `${selected.name} (bản demo)`,
          source:
            'DEMO LOCAL',
          beneficiaryGroup:
            selected.beneficiaryGroup ||
            'Bảo trợ xã hội',
          paymentCycle:
            selected.paymentCycle ||
            'Theo đợt',
          legalBasis:
            'Bản sao mô phỏng từ dữ liệu REAL',
        },
        ...demoPolicies,
      ])

      closeModal()
    }


  const realOnly =
    selected?.source ===
    'REAL API'


  return (
    <section className="category-workbench">

      <div className="category-workbench-heading">
        <div className="category-title-line">

          <span className="admin-feature-icon green small">
            <svg
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.8"
              strokeLinecap="round"
              strokeLinejoin="round"
              aria-hidden="true"
            >
              <path d="M7 3h10l3 3v15H7z" />
              <path d="M17 3v4h4" />
              <path d="M10 11h7" />
              <path d="M10 15h7" />
            </svg>
          </span>

          <div>
            <h2>
              Chính sách trợ cấp
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
            className="category-button primary"
            onClick={openCreate}
          >
            + Thêm chính sách DEMO
          </button>

        </div>
      </div>


      <div className="category-stat-grid">

        <article>
          <span>
            Chính sách REAL
          </span>

          <strong>
            {realPolicies.length}
          </strong>

          <small>
            REAL API
          </small>
        </article>

        <article>
          <span>
            Chính sách giả lập
          </span>

          <strong>
            {demoPolicies.length}
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
            Tổng mức hỗ trợ cấu hình
          </span>

          <strong
            style={{
              fontSize: '16px',
            }}
          >
            {formatMoney(
              totalAmount,
            )}
          </strong>

          <small>
            Không phải tổng đã chi
          </small>
        </article>

      </div>


      <div className="category-toolbar">

        <input
          value={keyword}
          placeholder="Tìm tên, nhóm hưởng, mô tả, căn cứ..."
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
            REAL API
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

          <option value="closed">
            Đã đóng
          </option>
        </select>

      </div>


      <div className="category-table-wrap">

        <table className="category-table">
          <thead>
            <tr>
              <th>STT</th>
              <th>Chính sách</th>
              <th>Nhóm hưởng</th>
              <th>Mức hỗ trợ</th>
              <th>Chu kỳ</th>
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
                      {item.name}
                    </strong>

                    <div
                      style={{
                        marginTop: '3px',
                        fontSize: '9px',
                        color: '#94a3b8',
                      }}
                    >
                      {item.description ||
                        'Không có mô tả'}
                    </div>
                  </td>

                  <td>
                    {item.beneficiaryGroup ||
                      '—'}
                  </td>

                  <td>
                    <strong>
                      {formatMoney(
                        item.amount,
                      )}
                    </strong>
                  </td>

                  <td>
                    {item.paymentCycle ||
                      '—'}
                  </td>

                  <td>
                    <span
                      className={
                        `category-source-badge ${
                          item.source ===
                          'REAL API'
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
                          item.status ===
                          'Active'
                            ? 'active'
                            : 'inactive'
                        }`
                      }
                    >
                      {item.status ===
                      'Active'
                        ? 'Hoạt động'
                        : 'Đã đóng'}
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
                            {item.status ===
                            'Active'
                              ? 'Đóng'
                              : 'Kích hoạt'}
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
                    colSpan={8}
                    className="category-empty"
                  >
                    Không có chính sách phù hợp
                    với bộ lọc.
                  </td>
                </tr>
              )}


            {loading && (
              <tr>
                <td
                  colSpan={8}
                  className="category-empty"
                >
                  Đang tải dữ liệu...
                </td>
              </tr>
            )}

          </tbody>
        </table>

      </div>


      <div className="category-pagination">

        <span>
          {filtered.length} bản ghi
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
          Bảo vệ dữ liệu production:
        </strong>

        <span>
          API REAL được ưu tiên để đọc. Do query
          Policy hiện chưa hoàn thiện đầy đủ,
          thao tác thêm/sửa/đóng/kích hoạt/xóa
          ở màn hình này chỉ áp dụng cho
          DEMO LOCAL và không ghi SQL Server.
        </span>

      </div>


      {(creating || selected) && (

        <div className="category-modal-overlay">

          <div className="category-modal">

            <div className="category-modal-heading">

              <div>
                <h2>
                  {creating
                    ? 'Thêm chính sách DEMO'
                    : realOnly
                      ? 'Chi tiết chính sách REAL'
                      : 'Chỉnh sửa chính sách DEMO'}
                </h2>

                <p>
                  {realOnly
                    ? 'Chính sách production đang ở chế độ chỉ đọc.'
                    : 'Thay đổi chỉ lưu localStorage và không ghi production.'}
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

              <label className="full">
                <span>
                  Tên chính sách
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


              <label>
                <span>
                  Mức hỗ trợ (VND)
                </span>

                <input
                  type="number"
                  min="0"
                  disabled={realOnly}
                  value={form.amount}
                  onChange={event =>
                    setForm({
                      ...form,
                      amount:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Nhóm hưởng
                </span>

                <input
                  disabled={realOnly}
                  value={
                    form.beneficiaryGroup
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      beneficiaryGroup:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Chu kỳ chi trả
                </span>

                <input
                  disabled={realOnly}
                  value={
                    form.paymentCycle
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      paymentCycle:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Căn cứ / ghi chú
                </span>

                <input
                  disabled={realOnly}
                  value={
                    form.legalBasis
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      legalBasis:
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


              <label className="category-switch-field full">

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
                  Chính sách đang hoạt động
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


              {realOnly && (
                <button
                  type="button"
                  className="category-button primary"
                  onClick={
                    copyRealToDemo
                  }
                >
                  Tạo bản sao DEMO để chỉnh sửa
                </button>
              )}


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

export default PoliciesCategoryPanel