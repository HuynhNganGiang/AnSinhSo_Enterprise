import {
  useMemo,
  useState,
} from 'react'

type Source =
  | 'REAL SNAPSHOT'
  | 'DEMO LOCAL'

type ClassificationItem = {
  id: number | string
  code: string
  domainName: string
  name: string
  description: string
  householdCount: number
  active: boolean
  source: Source
}

type DemoForm = {
  code: string
  name: string
  description: string
  active: boolean
}

const STORAGE_KEY =
  'ansinhso.demo.household-classifications.v1'

const PAGE_SIZE =
  8

const REAL_SNAPSHOT:
  ClassificationItem[] =
[
  {
    id: 1,
    code: 'UNKNOWN',
    domainName: 'Unknown',
    name: 'Chưa xác định',
    description: 'Hộ chưa đủ thông tin đáng tin cậy để xác định nhóm an sinh cụ thể.',
    householdCount: 923,
    active: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 2,
    code: 'POOR',
    domainName: 'Poor',
    name: 'Hộ nghèo',
    description: 'Hộ đang thuộc nhóm hộ nghèo theo CurrentClassification production.',
    householdCount: 84,
    active: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 3,
    code: 'NEAR_POOR',
    domainName: 'NearPoor',
    name: 'Hộ cận nghèo',
    description: 'Hộ đang thuộc nhóm hộ cận nghèo theo CurrentClassification production.',
    householdCount: 202,
    active: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 4,
    code: 'EXITED_POOR',
    domainName: 'ExitedPoor',
    name: 'Đã thoát nghèo',
    description: 'Hộ đã thoát khỏi nhóm hộ nghèo theo trạng thái nghiệp vụ hiện hành.',
    householdCount: 20,
    active: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 5,
    code: 'EXITED_NEAR_POOR',
    domainName: 'ExitedNearPoor',
    name: 'Đã thoát cận nghèo',
    description: 'Hộ đã thoát khỏi nhóm hộ cận nghèo theo trạng thái nghiệp vụ hiện hành.',
    householdCount: 32,
    active: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 99,
    code: 'OTHER',
    domainName: 'Other',
    name: 'Khác',
    description: 'Giá trị nguồn chưa đủ chắc chắn để chuẩn hóa vào nhóm nghiệp vụ chính.',
    householdCount: 2,
    active: true,
    source: 'REAL SNAPSHOT',
  }
]

const realHouseholdTotal =
  REAL_SNAPSHOT.reduce(
    (sum, item) =>
      sum +
      item.householdCount,
    0,
  )

function loadDemoRows():
  ClassificationItem[] {
  try {
    const raw =
      localStorage.getItem(
        STORAGE_KEY,
      )

    if (!raw)
      return []

    const parsed =
      JSON.parse(raw)

    return Array.isArray(parsed)
      ? parsed as ClassificationItem[]
      : []
  } catch {
    return []
  }
}

function storeDemoRows(
  rows:
    ClassificationItem[],
) {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify(rows),
  )
}

function HouseholdClassificationsCategoryPanel() {
  const [
    demoRows,
    setDemoRows,
  ] =
    useState<ClassificationItem[]>(
      () =>
        loadDemoRows(),
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
    useState<ClassificationItem | null>(
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
    useState<DemoForm>({
      code: '',
      name: '',
      description: '',
      active: true,
    })

  const allRows =
    useMemo(
      () => [
        ...REAL_SNAPSHOT,
        ...demoRows,
      ],
      [demoRows],
    )

  const filtered =
    useMemo(
      () => {
        const q =
          keyword
            .trim()
            .toLowerCase()

        return allRows.filter(
          item => {
            const keywordOk =
              !q ||
              item.code
                .toLowerCase()
                .includes(q) ||
              item.domainName
                .toLowerCase()
                .includes(q) ||
              item.name
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
                  'REAL SNAPSHOT'
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
        allRows,
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

  const currentPage =
    Math.min(
      page,
      totalPages,
    )

  const visible =
    filtered.slice(
      (currentPage - 1) *
        PAGE_SIZE,
      currentPage *
        PAGE_SIZE,
    )

  const getRealCount =
    (
      domainName:
        string,
    ) =>
      REAL_SNAPSHOT.find(
        item =>
          item.domainName ===
          domainName,
      )?.householdCount ?? 0

  const poorCount =
    getRealCount('Poor')

  const nearPoorCount =
    getRealCount('NearPoor')

  const unknownCount =
    getRealCount('Unknown')

  const saveDemoState =
    (
      rows:
        ClassificationItem[],
    ) => {
      setDemoRows(rows)
      storeDemoRows(rows)
    }

  const openCreate =
    () => {
      setCreating(true)
      setSelected(null)

      setForm({
        code:
          `DEMO_CLASS_${demoRows.length + 1}`,
        name: '',
        description:
          'Phân loại giả lập phục vụ trình diễn.',
        active: true,
      })
    }

  const openDetail =
    (
      item:
        ClassificationItem,
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

      if (!code || !name) {
        window.alert(
          'Vui lòng nhập mã và tên phân loại.',
        )
        return
      }

      const duplicate =
        allRows.some(
          item =>
            item.code
              .toLowerCase() ===
              code.toLowerCase() &&
            item.id !==
              selected?.id,
        )

      if (duplicate) {
        window.alert(
          'Mã phân loại đã tồn tại.',
        )
        return
      }

      if (creating) {
        const item:
          ClassificationItem =
        {
          id:
            `demo-classification-${Date.now()}`,
          code,
          domainName:
            code,
          name,
          description:
            form.description.trim(),
          householdCount: 0,
          active:
            form.active,
          source:
            'DEMO LOCAL',
        }

        saveDemoState([
          item,
          ...demoRows,
        ])
      } else if (
        selected?.source ===
        'DEMO LOCAL'
      ) {
        saveDemoState(
          demoRows.map(
            item =>
              item.id ===
              selected.id
                ? {
                    ...item,
                    code,
                    domainName:
                      code,
                    name,
                    description:
                      form.description.trim(),
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
        ClassificationItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      saveDemoState(
        demoRows.map(
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
        ClassificationItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa phân loại DEMO "${item.name}"?`,
        )
      ) {
        return
      }

      saveDemoState(
        demoRows.filter(
          row =>
            row.id !==
            item.id,
        ),
      )
    }

  const realOnly =
    selected?.source ===
    'REAL SNAPSHOT'

  return (
    <section className="category-workbench">

      <div className="category-workbench-heading">

        <div className="category-title-line">

          <span className="admin-feature-icon slate small">
            <svg
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.8"
              aria-hidden="true"
            >
              <path d="M4 7h10" />
              <path d="M18 7h2" />
              <circle cx="16" cy="7" r="2" />

              <path d="M4 12h2" />
              <path d="M10 12h10" />
              <circle cx="8" cy="12" r="2" />

              <path d="M4 17h7" />
              <path d="M15 17h5" />
              <circle cx="13" cy="17" r="2" />
            </svg>
          </span>

          <div>
            <h2>
              Phân loại hộ
            </h2>

            <p>
              6 định nghĩa phân loại REAL,
              tổng {realHouseholdTotal.toLocaleString('vi-VN')} hộ production.
              CurrentClassification độc lập với trạng thái
              Active / Inactive của hộ.
            </p>
          </div>

        </div>


        <div className="category-heading-actions">

          <button
            type="button"
            className="category-button secondary"
            onClick={() => {
              window.location.href =
                '/households'
            }}
          >
            Xem hộ gia đình
          </button>

          <button
            type="button"
            className="category-button primary"
            onClick={openCreate}
          >
            + Thêm phân loại DEMO
          </button>

        </div>

      </div>


      <div className="category-stat-grid">

        <article>
          <span>
            Tổng hộ production
          </span>

          <strong>
            {realHouseholdTotal.toLocaleString('vi-VN')}
          </strong>

          <small>
            REAL SNAPSHOT
          </small>
        </article>


        <article>
          <span>
            Hộ nghèo
          </span>

          <strong>
            {poorCount.toLocaleString('vi-VN')}
          </strong>

          <small>
            ID = 2
          </small>
        </article>


        <article>
          <span>
            Hộ cận nghèo
          </span>

          <strong>
            {nearPoorCount.toLocaleString('vi-VN')}
          </strong>

          <small>
            ID = 3
          </small>
        </article>


        <article>
          <span>
            Chưa xác định
          </span>

          <strong>
            {unknownCount.toLocaleString('vi-VN')}
          </strong>

          <small>
            ID = 1
          </small>
        </article>

      </div>


      <div className="category-toolbar">

        <input
          value={keyword}
          placeholder="Tìm mã, tên hoặc mô tả phân loại..."
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
            REAL SNAPSHOT
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
              <th>ID</th>
              <th>Mã</th>
              <th>Tên phân loại</th>
              <th>Domain</th>
              <th>Số hộ</th>
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
                    {(currentPage - 1) *
                      PAGE_SIZE +
                      index +
                      1}
                  </td>

                  <td>
                    {item.id}
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
                      {item.description}
                    </div>
                  </td>

                  <td>
                    {item.domainName}
                  </td>

                  <td>
                    <strong>
                      {item.householdCount.toLocaleString('vi-VN')}
                    </strong>
                  </td>

                  <td>
                    <span
                      className={
                        `category-source-badge ${
                          item.source ===
                          'REAL SNAPSHOT'
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
                        'REAL SNAPSHOT'
                          ? 'Xem'
                          : 'Xem / Sửa'}
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


            {visible.length === 0 && (
              <tr>
                <td
                  colSpan={9}
                  className="category-empty"
                >
                  Không có phân loại phù hợp với bộ lọc.
                </td>
              </tr>
            )}

          </tbody>

        </table>

      </div>


      <div className="category-pagination">

        <span>
          {filtered.length} phân loại
        </span>

        <div>

          <button
            type="button"
            disabled={currentPage <= 1}
            onClick={() =>
              setPage(
                Math.max(
                  1,
                  currentPage - 1,
                ),
              )
            }
          >
            ← Trước
          </button>

          <span>
            Trang {currentPage}/{totalPages}
          </span>

          <button
            type="button"
            disabled={
              currentPage >=
              totalPages
            }
            onClick={() =>
              setPage(
                Math.min(
                  totalPages,
                  currentPage + 1,
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
          6 dòng REAL là snapshot được lấy từ
          AnSinhSoRealDb trong quá trình triển khai.
          REAL chỉ xem. Thêm/Sửa/Bật-Tắt/Xóa
          chỉ áp dụng cho DEMO LOCAL và chỉ lưu
          localStorage, không ghi SQL production.
        </span>

      </div>


      {(creating || selected) && (

        <div className="category-modal-overlay">

          <div className="category-modal">

            <div className="category-modal-heading">

              <div>
                <h2>
                  {creating
                    ? 'Thêm phân loại hộ DEMO'
                    : realOnly
                      ? 'Chi tiết phân loại REAL'
                      : 'Chỉnh sửa phân loại DEMO'}
                </h2>

                <p>
                  {realOnly
                    ? `${selected?.householdCount.toLocaleString('vi-VN') ?? 0} hộ production thuộc phân loại này.`
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
                  Mã phân loại
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
                  Tên hiển thị
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


            {realOnly && selected && (
              <div className="category-safety-note">

                <strong>
                  Production:
                </strong>

                <span>
                  ID = {selected.id};
                  Domain = {selected.domainName};
                  số hộ = {selected.householdCount.toLocaleString('vi-VN')}.
                  CurrentClassification không phải
                  lifecycle HouseholdStatus.
                </span>

              </div>
            )}


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
                    '/households'
                }}
              >
                Mở danh sách hộ
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

export default HouseholdClassificationsCategoryPanel