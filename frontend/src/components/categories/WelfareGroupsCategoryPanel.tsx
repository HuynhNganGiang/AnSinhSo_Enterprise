import { useEffect, useMemo, useState } from 'react'

type SourceKind =
  | 'REAL API'
  | 'DEMO LOCAL'

type WelfareGroupItem = {
  id: string
  name: string
  description: string
  isActive: boolean
  source: SourceKind
}

type WelfareGroupForm = {
  name: string
  description: string
  isActive: boolean
}

type UnknownRecord =
  Record<string, unknown>

const STORAGE_KEY =
  'ansinhso.demo.welfare-groups.v1'

const PAGE_SIZE =
  8

const defaultDemoGroups:
  WelfareGroupItem[] =
[
  {
    id: 'demo-welfare-group-elderly',
    name: 'Người cao tuổi',
    description:
      'Nhóm người cao tuổi thuộc diện theo dõi và hỗ trợ an sinh.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-welfare-group-disability',
    name: 'Người khuyết tật',
    description:
      'Nhóm người khuyết tật cần theo dõi chính sách bảo trợ và hỗ trợ.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-welfare-group-children',
    name: 'Trẻ em có hoàn cảnh khó khăn',
    description:
      'Nhóm trẻ em có hoàn cảnh khó khăn cần hỗ trợ và theo dõi.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-welfare-group-poor',
    name: 'Hộ nghèo',
    description:
      'Nhóm đối tượng thuộc hộ nghèo theo phân loại đang sử dụng.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-welfare-group-near-poor',
    name: 'Hộ cận nghèo',
    description:
      'Nhóm đối tượng thuộc hộ cận nghèo cần theo dõi chính sách.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-welfare-group-social',
    name: 'Bảo trợ xã hội',
    description:
      'Nhóm đối tượng thuộc diện bảo trợ xã hội và trợ giúp thường xuyên.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-welfare-group-single',
    name: 'Người đơn thân khó khăn',
    description:
      'Nhóm người đơn thân có hoàn cảnh khó khăn cần theo dõi hỗ trợ.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-welfare-group-other',
    name: 'Đối tượng khó khăn khác',
    description:
      'Nhóm mở rộng phục vụ tình huống hỗ trợ an sinh khác.',
    isActive: true,
    source: 'DEMO LOCAL',
  },
]

function readStoredDemo():
  WelfareGroupItem[] | null {
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

    return parsed as WelfareGroupItem[]
  } catch {
    return null
  }
}

function persistDemo(
  items: WelfareGroupItem[],
) {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify(items),
  )
}

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

  const data =
    asRecord(root.data)

  if (!data)
    return []

  if (Array.isArray(data.data))
    return data.data

  if (Array.isArray(data.items))
    return data.items

  const nested =
    asRecord(data.data)

  if (!nested)
    return []

  if (Array.isArray(nested.items))
    return nested.items

  if (Array.isArray(nested.data))
    return nested.data

  return []
}

function readString(
  record: UnknownRecord,
  ...keys: string[]
) {
  for (const key of keys) {
    const value =
      record[key]

    if (
      typeof value === 'string' &&
      value.trim()
    ) {
      return value.trim()
    }
  }

  return ''
}

function readBoolean(
  record: UnknownRecord,
  fallback: boolean,
  ...keys: string[]
) {
  for (const key of keys) {
    const value =
      record[key]

    if (typeof value === 'boolean')
      return value
  }

  return fallback
}

function toRealGroup(
  value: unknown,
  index: number,
): WelfareGroupItem | null {
  const row =
    asRecord(value)

  if (!row)
    return null

  const name =
    readString(
      row,
      'name',
      'Name',
    )

  if (!name)
    return null

  return {
    id:
      readString(
        row,
        'id',
        'Id',
      ) ||
      `real-welfare-group-${index}`,

    name,

    description:
      readString(
        row,
        'description',
        'Description',
      ),

    isActive:
      readBoolean(
        row,
        true,
        'isActive',
        'IsActive',
      ),

    source:
      'REAL API',
  }
}

function WelfareGroupsCategoryPanel() {
  const [
    realGroups,
    setRealGroups,
  ] =
    useState<WelfareGroupItem[]>([])

  const [
    demoGroups,
    setDemoGroups,
  ] =
    useState<WelfareGroupItem[]>(
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
      'Đang kiểm tra dữ liệu nhóm đối tượng...',
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
    selected,
    setSelected,
  ] =
    useState<WelfareGroupItem | null>(
      null,
    )

  const [
    creating,
    setCreating,
  ] =
    useState(false)

  const [
    form,
    setForm,
  ] =
    useState<WelfareGroupForm>({
      name: '',
      description: '',
      isActive: true,
    })

  const load =
    async () => {
      setLoading(true)

      const token =
        localStorage.getItem(
          'accessToken',
        )

      try {
        const response =
          await fetch(
            '/api/welfare-groups?pageNumber=1&pageSize=200',
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

        if (!response.ok) {
          throw new Error(
            `HTTP ${response.status}`,
          )
        }

        const body =
          (await response.json()) as unknown

        const rows =
          extractRows(body)

        const mapped =
          rows
            .map(toRealGroup)
            .filter(
              (
                item,
              ): item is WelfareGroupItem =>
                item !== null,
            )

        if (mapped.length === 0) {
          throw new Error(
            'EMPTY_WELFARE_GROUPS',
          )
        }

        setRealGroups(mapped)

        const stored =
          readStoredDemo()

        if (stored !== null) {
          setDemoGroups(stored)
        }

        setMessage(
          `${mapped.length} nhóm đối tượng đang lấy trực tiếp từ API production.`,
        )
      } catch {
        setRealGroups([])

        const stored =
          readStoredDemo()

        if (stored === null) {
          setDemoGroups(
            defaultDemoGroups,
          )

          persistDemo(
            defaultDemoGroups,
          )
        } else {
          setDemoGroups(
            stored,
          )
        }

        setMessage(
          'Production chưa có dữ liệu nhóm đối tượng hoặc API danh sách chưa khả dụng. Đang dùng DEMO LOCAL.',
        )
      } finally {
        setLoading(false)
      }
    }

  useEffect(() => {
    void load()
  }, [])

  const allGroups =
    useMemo(
      () => [
        ...realGroups,
        ...demoGroups,
      ],
      [
        realGroups,
        demoGroups,
      ],
    )

  const filtered =
    useMemo(
      () => {
        const q =
          keyword
            .trim()
            .toLowerCase()

        return allGroups.filter(
          item => {
            const keywordOk =
              !q ||
              item.name
                .toLowerCase()
                .includes(q) ||
              item.description
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
                item.isActive
              ) ||
              (
                statusFilter === 'inactive' &&
                !item.isActive
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
        allGroups,
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

  const updateDemo =
    (
      next:
        WelfareGroupItem[],
    ) => {
      setDemoGroups(next)
      persistDemo(next)
    }

  const openCreate =
    () => {
      setSelected(null)
      setCreating(true)

      setForm({
        name: '',
        description:
          'Nhóm đối tượng giả lập phục vụ trình diễn.',
        isActive: true,
      })
    }

  const openDetail =
    (
      item: WelfareGroupItem,
    ) => {
      setCreating(false)
      setSelected(item)

      setForm({
        name:
          item.name,

        description:
          item.description,

        isActive:
          item.isActive,
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

      if (!name) {
        window.alert(
          'Vui lòng nhập tên nhóm đối tượng.',
        )
        return
      }

      const duplicate =
        allGroups.some(
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
          'Tên nhóm đối tượng đã tồn tại.',
        )
        return
      }

      if (creating) {
        const item:
          WelfareGroupItem =
        {
          id:
            `demo-welfare-group-${Date.now()}`,

          name,

          description:
            form.description.trim(),

          isActive:
            form.isActive,

          source:
            'DEMO LOCAL',
        }

        updateDemo([
          item,
          ...demoGroups,
        ])
      } else if (
        selected?.source ===
        'DEMO LOCAL'
      ) {
        updateDemo(
          demoGroups.map(
            item =>
              item.id ===
              selected.id
                ? {
                    ...item,
                    name,
                    description:
                      form.description.trim(),
                    isActive:
                      form.isActive,
                  }
                : item,
          ),
        )
      }

      closeModal()
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
            `demo-welfare-group-copy-${stamp}`,
          name:
            `${selected.name} (bản demo)`,
          source:
            'DEMO LOCAL',
        },
        ...demoGroups,
      ])

      closeModal()
    }

  const toggleDemo =
    (
      item: WelfareGroupItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      updateDemo(
        demoGroups.map(
          row =>
            row.id ===
            item.id
              ? {
                  ...row,
                  isActive:
                    !row.isActive,
                }
              : row,
        ),
      )
    }

  const deleteDemo =
    (
      item: WelfareGroupItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa nhóm giả lập "${item.name}"?`,
        )
      ) {
        return
      }

      updateDemo(
        demoGroups.filter(
          row =>
            row.id !==
            item.id,
        ),
      )
    }

  const realOnly =
    selected?.source ===
    'REAL API'

  const activeCount =
    allGroups.filter(
      item =>
        item.isActive,
    ).length

  return (
    <section className="category-workbench">

      <div className="category-workbench-heading">
        <div className="category-title-line">
          <span className="admin-feature-icon blue small">
            <svg
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.8"
              aria-hidden="true"
            >
              <circle
                cx="8"
                cy="8"
                r="3"
              />
              <circle
                cx="16.5"
                cy="9"
                r="2.3"
              />
              <path d="M3 20c.5-4 2.7-6 6.3-6 3.5 0 5.5 2 6 6" />
              <path d="M15 15c3 .1 4.8 1.8 5.4 5" />
            </svg>
          </span>

          <div>
            <h2>
              Nhóm đối tượng
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
            onClick={() =>
              void load()
            }
            disabled={loading}
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
            + Thêm nhóm DEMO
          </button>
        </div>
      </div>


      <div className="category-stat-grid">
        <article>
          <span>
            Nhóm production
          </span>

          <strong>
            {realGroups.length}
          </strong>

          <small>
            REAL API
          </small>
        </article>

        <article>
          <span>
            Nhóm giả lập
          </span>

          <strong>
            {demoGroups.length}
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
            Tổng hiển thị
          </span>

          <strong>
            {allGroups.length}
          </strong>

          <small>
            Có phân biệt nguồn
          </small>
        </article>
      </div>


      <div className="category-toolbar">
        <input
          value={keyword}
          placeholder="Tìm tên hoặc mô tả nhóm đối tượng..."
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
              <th>Tên nhóm</th>
              <th>Mô tả</th>
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
                  </td>

                  <td>
                    {item.description ||
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
                          item.isActive
                            ? 'active'
                            : 'inactive'
                        }`
                      }
                    >
                      {item.isActive
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
                            {item.isActive
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
              visible.length === 0 && (
                <tr>
                  <td
                    colSpan={6}
                    className="category-empty"
                  >
                    Không có nhóm đối tượng
                    phù hợp với bộ lọc.
                  </td>
                </tr>
              )}

            {loading && (
              <tr>
                <td
                  colSpan={6}
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
          Production hiện chưa có WelfareGroups:
        </strong>

        <span>
          Các nhóm đang trình diễn là DEMO LOCAL.
          Thêm/Sửa/Bật-Tắt/Xóa chỉ tác động
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
                    ? 'Thêm nhóm đối tượng DEMO'
                    : realOnly
                      ? 'Chi tiết nhóm đối tượng REAL'
                      : 'Chỉnh sửa nhóm đối tượng DEMO'}
                </h2>

                <p>
                  {realOnly
                    ? 'Bản ghi production đang ở chế độ chỉ đọc.'
                    : 'Thay đổi chỉ lưu cục bộ trong trình duyệt.'}
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
                  Tên nhóm đối tượng
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
                  Mô tả
                </span>

                <textarea
                  disabled={realOnly}
                  value={form.description}
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
                    form.isActive
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      isActive:
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

export default WelfareGroupsCategoryPanel