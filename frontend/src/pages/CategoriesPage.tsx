import { useEffect, useMemo, useState } from 'react'
import AppLayout from '../layouts/AppLayout'
import './AdminManagementPages.css'
import WelfareGroupsCategoryPanel from '../components/categories/WelfareGroupsCategoryPanel'
import PoliciesCategoryPanel from '../components/categories/PoliciesCategoryPanel'
import AreasCategoryPanel from '../components/categories/AreasCategoryPanel'
import PaymentPointsCategoryPanel from '../components/categories/PaymentPointsCategoryPanel'

type CategoryKey =
  | 'welfare-groups'
  | 'policies'
  | 'relationship-types'
  | 'areas'
  | 'payment-points'
  | 'household-classifications'

type CategoryIcon =
  | 'group'
  | 'policy'
  | 'relation'
  | 'location'
  | 'payment'
  | 'classification'

type RelationshipSource =
  | 'REAL API'
  | 'REAL SNAPSHOT'
  | 'DEMO LOCAL'

type RelationshipItem = {
  id: string
  code: string
  name: string
  description: string
  isActive: boolean
  source: RelationshipSource
}

type RelationshipApiRow = {
  id?: string
  code?: string
  name?: string
  description?: string
  isActive?: boolean
  source?: string
}

type RelationshipApiPayload = {
  success?: boolean
  data?: RelationshipApiRow[]
  totalCount?: number
}

type RelationshipApiResponse = {
  success?: boolean
  data?:
    | RelationshipApiPayload
    | RelationshipApiRow[]
}

type RelationshipForm = {
  code: string
  name: string
  description: string
  isActive: boolean
}

const RELATIONSHIP_DEMO_KEY =
  'ansinhso.demo.relationship-types.v1'

const PAGE_SIZE = 8

const relationshipSnapshot: RelationshipItem[] = [
  {
    id: 'snapshot-CHU_HO',
    code: 'CHU_HO',
    name: 'Chủ hộ',
    description: 'Quan hệ chủ hộ.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-CON',
    code: 'CON',
    name: 'Con',
    description: 'Quan hệ con trong hộ gia đình.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-CHAU',
    code: 'CHAU',
    name: 'Cháu',
    description: 'Quan hệ cháu trong hộ gia đình.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-VO',
    code: 'VO',
    name: 'Vợ',
    description: 'Quan hệ vợ.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-VO_CHONG',
    code: 'VO_CHONG',
    name: 'Vợ / Chồng',
    description: 'Quan hệ vợ hoặc chồng.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-CHONG',
    code: 'CHONG',
    name: 'Chồng',
    description: 'Quan hệ chồng.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-EM',
    code: 'EM',
    name: 'Em',
    description: 'Quan hệ em.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-CON_DAU',
    code: 'CON_DAU',
    name: 'Con dâu',
    description: 'Quan hệ con dâu.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-KHAC',
    code: 'KHAC',
    name: 'Khác',
    description: 'Quan hệ khác trong hộ.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-ME',
    code: 'ME',
    name: 'Mẹ',
    description: 'Quan hệ mẹ.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-ANH',
    code: 'ANH',
    name: 'Anh',
    description: 'Quan hệ anh.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-CHI',
    code: 'CHI',
    name: 'Chị',
    description: 'Quan hệ chị.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-BO',
    code: 'BO',
    name: 'Bố',
    description: 'Quan hệ bố.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
  {
    id: 'snapshot-BA',
    code: 'BA',
    name: 'Bà',
    description: 'Quan hệ bà.',
    isActive: true,
    source: 'REAL SNAPSHOT',
  },
]

const categoryCards = [
  {
    key: 'welfare-groups' as CategoryKey,
    name: 'Nhóm đối tượng',
    description:
      'Nhóm người cao tuổi, trẻ em, người khuyết tật và các nhóm bảo trợ xã hội.',
    icon: 'group' as CategoryIcon,
    tone: 'blue',
  },
  {
    key: 'policies' as CategoryKey,
    name: 'Chính sách trợ cấp',
    description:
      'Danh mục chính sách, chương trình hỗ trợ và thông tin nghiệp vụ liên quan.',
    icon: 'policy' as CategoryIcon,
    tone: 'green',
  },
  {
    key: 'relationship-types' as CategoryKey,
    name: 'Loại quan hệ hộ',
    description:
      'Quan hệ giữa thành viên và hộ gia đình như chủ hộ, con, vợ, chồng.',
    icon: 'relation' as CategoryIcon,
    tone: 'violet',
  },
  {
    key: 'areas' as CategoryKey,
    name: 'Địa bàn quản lý',
    description:
      'Danh mục thôn, khu vực và địa bàn sử dụng trong tra cứu, thống kê và bản đồ.',
    icon: 'location' as CategoryIcon,
    tone: 'orange',
  },
  {
    key: 'payment-points' as CategoryKey,
    name: 'Điểm chi trả',
    description:
      'Danh mục địa điểm phục vụ các đợt chi trả và điều phối trợ cấp.',
    icon: 'payment' as CategoryIcon,
    tone: 'cyan',
  },
  {
    key: 'household-classifications' as CategoryKey,
    name: 'Phân loại hộ',
    description:
      'Các trạng thái hộ nghèo, cận nghèo, thoát nghèo và nhóm khác.',
    icon: 'classification' as CategoryIcon,
    tone: 'slate',
  },
]

function CategorySvg({
  name,
}: {
  name: CategoryIcon
}) {
  let content

  if (name === 'group') {
    content = (
      <>
        <circle cx="8" cy="8" r="3" />
        <circle cx="16.5" cy="9" r="2.3" />
        <path d="M3 20c.5-4 2.7-6 6.3-6 3.5 0 5.5 2 6 6" />
        <path d="M15 15c3 .1 4.8 1.8 5.4 5" />
      </>
    )
  } else if (name === 'policy') {
    content = (
      <>
        <path d="M7 3h10l3 3v15H7z" />
        <path d="M17 3v4h4M10 11h7M10 15h7" />
      </>
    )
  } else if (name === 'relation') {
    content = (
      <>
        <circle cx="7" cy="7" r="3" />
        <circle cx="17" cy="7" r="3" />
        <path d="M4 20c.5-4 2.5-6 6-6" />
        <path d="M20 20c-.5-4-2.5-6-6-6" />
        <path d="M9 10l6 4M15 10l-6 4" />
      </>
    )
  } else if (name === 'location') {
    content = (
      <>
        <path d="M20 10c0 5-8 11-8 11S4 15 4 10a8 8 0 1 1 16 0Z" />
        <circle cx="12" cy="10" r="2.5" />
      </>
    )
  } else if (name === 'payment') {
    content = (
      <>
        <rect x="3" y="5" width="18" height="14" rx="2" />
        <path d="M3 10h18M7 15h4" />
      </>
    )
  } else {
    content = (
      <>
        <path d="M4 6h16M4 12h16M4 18h16" />
        <circle cx="8" cy="6" r="2" />
        <circle cx="15" cy="12" r="2" />
        <circle cx="10" cy="18" r="2" />
      </>
    )
  }

  return (
    <svg
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.8"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      {content}
    </svg>
  )
}

function loadDemoRelations(): RelationshipItem[] {
  try {
    const raw =
      localStorage.getItem(
        RELATIONSHIP_DEMO_KEY,
      )

    if (!raw)
      return []

    const parsed =
      JSON.parse(raw)

    if (!Array.isArray(parsed))
      return []

    return parsed as RelationshipItem[]
  } catch {
    return []
  }
}

function saveDemoRelations(
  items: RelationshipItem[],
) {
  localStorage.setItem(
    RELATIONSHIP_DEMO_KEY,
    JSON.stringify(items),
  )
}

function sourceClass(
  source: RelationshipSource,
) {
  if (source === 'DEMO LOCAL')
    return 'demo'

  if (source === 'REAL API')
    return 'real-api'

  return 'snapshot'
}

function CategoriesPage() {
  const [
    selectedCategory,
    setSelectedCategory,
  ] =
    useState<CategoryKey>(
      'relationship-types',
    )

  const [
    realRelations,
    setRealRelations,
  ] =
    useState<RelationshipItem[]>([])

  const [
    demoRelations,
    setDemoRelations,
  ] =
    useState<RelationshipItem[]>(
      () => loadDemoRelations(),
    )

  const [
    loading,
    setLoading,
  ] =
    useState(true)

  const [
    loadMessage,
    setLoadMessage,
  ] =
    useState(
      'Đang tải danh mục quan hệ hộ...',
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
    selectedRelation,
    setSelectedRelation,
  ] =
    useState<RelationshipItem | null>(
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
    useState<RelationshipForm>({
      code: '',
      name: '',
      description: '',
      isActive: true,
    })

  const loadRelationships =
    async () => {
      setLoading(true)

      const token =
        localStorage.getItem(
          'accessToken',
        )

      try {
        const response =
          await fetch(
            '/api/v1/relationship-types?activeOnly=false',
            {
              headers: token
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
          (await response.json()) as RelationshipApiResponse

        const relationshipRows =
          Array.isArray(body.data)
            ? body.data
            : Array.isArray(
                body.data?.data,
              )
              ? body.data.data
              : []

        const apiItems =
          relationshipRows
            .map(
              (
                item,
                index,
              ): RelationshipItem => ({
                id:
                  String(
                    item.id ??
                      item.code ??
                      `real-${index}`,
                  ),

                code:
                  String(
                    item.code ??
                      '',
                  ),

                name:
                  String(
                    item.name ??
                      item.code ??
                      '',
                  ),

                description:
                  String(
                    item.description ??
                      '',
                  ),

                isActive:
                  item.isActive ??
                  true,

                source:
                  'REAL API',
              }),
            )
            .filter(
              item =>
                item.code.length > 0 ||
                item.name.length > 0,
            )

        if (apiItems.length === 0) {
          throw new Error(
            'EMPTY_RELATIONSHIP_API',
          )
        }

        setRealRelations(
          apiItems,
        )

        setLoadMessage(
          `${apiItems.length} loại quan hệ đang lấy trực tiếp từ AnSinhSoRealDb.`,
        )
      } catch {
        setRealRelations(
          relationshipSnapshot,
        )

        setLoadMessage(
          'API chưa khả dụng trong phiên hiện tại. Đang dùng snapshot 14 loại quan hệ đã xác minh từ production.',
        )
      } finally {
        setLoading(false)
      }
    }

  useEffect(() => {
    void loadRelationships()
  }, [])

  const allRelations =
    useMemo(
      () => [
        ...realRelations,
        ...demoRelations,
      ],
      [
        realRelations,
        demoRelations,
      ],
    )

  const filteredRelations =
    useMemo(
      () => {
        const q =
          keyword
            .trim()
            .toLowerCase()

        return allRelations.filter(
          item => {
            const keywordMatch =
              !q ||
              item.code
                .toLowerCase()
                .includes(q) ||
              item.name
                .toLowerCase()
                .includes(q) ||
              item.description
                .toLowerCase()
                .includes(q)

            const sourceMatch =
              sourceFilter === 'all' ||
              (
                sourceFilter === 'real' &&
                item.source !==
                  'DEMO LOCAL'
              ) ||
              (
                sourceFilter === 'demo' &&
                item.source ===
                  'DEMO LOCAL'
              )

            const statusMatch =
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
              keywordMatch &&
              sourceMatch &&
              statusMatch
            )
          },
        )
      },
      [
        allRelations,
        keyword,
        sourceFilter,
        statusFilter,
      ],
    )

  const totalPages =
    Math.max(
      1,
      Math.ceil(
        filteredRelations.length /
        PAGE_SIZE,
      ),
    )

  const visibleRelations =
    filteredRelations.slice(
      (page - 1) *
        PAGE_SIZE,
      page *
        PAGE_SIZE,
    )

  const realCount =
    realRelations.length

  const demoCount =
    demoRelations.length

  const activeCount =
    allRelations.filter(
      item =>
        item.isActive,
    ).length

  const updateDemo =
    (
      next:
        RelationshipItem[],
    ) => {
      setDemoRelations(next)
      saveDemoRelations(next)
    }

  const openCreate =
    () => {
      setSelectedRelation(null)
      setCreating(true)

      setForm({
        code: '',
        name: '',
        description:
          'Dữ liệu giả lập phục vụ demo danh mục.',
        isActive: true,
      })
    }

  const openDetail =
    (
      item: RelationshipItem,
    ) => {
      setCreating(false)
      setSelectedRelation(
        item,
      )

      setForm({
        code:
          item.code,

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
      setSelectedRelation(null)
      setCreating(false)
    }

  const saveDemo =
    () => {
      const code =
        form.code
          .trim()
          .toUpperCase()
          .replace(
            /\s+/g,
            '_',
          )

      const name =
        form.name.trim()

      if (!code || !name) {
        window.alert(
          'Vui lòng nhập mã và tên quan hệ.',
        )
        return
      }

      const duplicate =
        allRelations.some(
          item =>
            item.code
              .toUpperCase() ===
              code &&
            item.id !==
              selectedRelation?.id,
        )

      if (duplicate) {
        window.alert(
          'Mã quan hệ đã tồn tại.',
        )
        return
      }

      if (creating) {
        const item:
          RelationshipItem =
        {
          id:
            `demo-relation-${Date.now()}`,

          code,

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
          ...demoRelations,
        ])
      } else if (
        selectedRelation?.source ===
        'DEMO LOCAL'
      ) {
        updateDemo(
          demoRelations.map(
            item =>
              item.id ===
              selectedRelation.id
                ? {
                    ...item,
                    code,
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
        !selectedRelation ||
        selectedRelation.source ===
          'DEMO LOCAL'
      ) {
        return
      }

      const stamp =
        Date.now()

      const copy:
        RelationshipItem =
      {
        ...selectedRelation,

        id:
          `demo-relation-copy-${stamp}`,

        code:
          `${selectedRelation.code}_DEMO_${String(
            stamp,
          ).slice(-4)}`,

        name:
          `${selectedRelation.name} (bản demo)`,

        source:
          'DEMO LOCAL',
      }

      updateDemo([
        copy,
        ...demoRelations,
      ])

      closeModal()
    }

  const toggleDemo =
    (
      item:
        RelationshipItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      updateDemo(
        demoRelations.map(
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

  const removeDemo =
    (
      item:
        RelationshipItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa quan hệ giả lập "${item.name}"?`,
        )
      ) {
        return
      }

      updateDemo(
        demoRelations.filter(
          row =>
            row.id !==
            item.id,
        ),
      )
    }

  const selectedCard =
    categoryCards.find(
      item =>
        item.key ===
        selectedCategory,
    )

  const realOnlyModal =
    selectedRelation !== null &&
    selectedRelation.source !==
      'DEMO LOCAL'

  return (
    <AppLayout>
      <div className="admin-management-page">

        <section className="admin-page-heading">
          <div>
            <div className="admin-page-eyebrow">
              <CategorySvg
                name="classification"
              />

              <span>
                QUẢN TRỊ DANH MỤC
              </span>
            </div>

            <h1>
              Danh mục
            </h1>

            <p>
              Quản lý các danh mục dùng chung
              trong hệ thống AnSinhSo.
            </p>
          </div>

          <div className="admin-heading-badge">
            <i />
            REAL-FIRST
          </div>
        </section>


        <section className="admin-notice">
          <span className="admin-notice-icon">
            i
          </span>

          <div>
            <strong>
              Nguyên tắc dữ liệu
            </strong>

            <p>
              Dữ liệu production luôn được ưu tiên.
              Các thao tác thử nghiệm được ghi rõ
              DEMO LOCAL và chỉ lưu trong trình duyệt,
              không sửa dữ liệu SQL production.
            </p>
          </div>
        </section>


        <section className="admin-card-grid">
          {categoryCards.map(
            item => {
              const implemented =
                item.key ===
                  'relationship-types' ||
                item.key ===
                  'welfare-groups' ||
                item.key ===
                  'policies' ||
                item.key ===
                  'areas' ||
                item.key ===
                  'payment-points'

              const selected =
                selectedCategory ===
                item.key

              return (
                <article
                  className={
                    `admin-feature-card ${
                      selected
                        ? 'category-selected'
                        : ''
                    }`
                  }
                  key={item.key}
                >
                  <div className="admin-feature-top">
                    <span
                      className={
                        `admin-feature-icon ${item.tone}`
                      }
                    >
                      <CategorySvg
                        name={item.icon}
                      />
                    </span>

                    <span
                      className={
                        `admin-status-tag ${
                          implemented
                            ? 'ready'
                            : ''
                        }`
                      }
                    >
                      {implemented
                        ? 'ĐANG HOẠT ĐỘNG'
                        : 'TIẾP THEO'}
                    </span>
                  </div>

                  <h2>
                    {item.name}
                  </h2>

                  <p>
                    {item.description}
                  </p>

                  <div className="admin-feature-footer">
                    <span>
                      {implemented
                        ? item.key ===
                            'relationship-types'
                          ? `${realCount || 14} REAL`
                          : item.key ===
                              'welfare-groups'
                            ? 'DEMO FALLBACK'
                            : 'REAL-FIRST / DEMO'
                        : 'Triển khai lần lượt'}
                    </span>

                    <button
                      type="button"
                      className="category-open-button"
                      onClick={() => {
                        setSelectedCategory(
                          item.key,
                        )
                      }}
                    >
                      {implemented
                        ? 'Mở quản lý →'
                        : 'Xem kế hoạch →'}
                    </button>
                  </div>
                </article>
              )
            },
          )}
        </section>


        {selectedCategory ===
        'relationship-types' ? (
          <section className="category-workbench">

            <div className="category-workbench-heading">
              <div>
                <div className="category-title-line">
                  <span className="admin-feature-icon violet small">
                    <CategorySvg
                      name="relation"
                    />
                  </span>

                  <div>
                    <h2>
                      Loại quan hệ hộ
                    </h2>

                    <p>
                      {loadMessage}
                    </p>
                  </div>
                </div>
              </div>

              <div className="category-heading-actions">
                <button
                  type="button"
                  className="category-button secondary"
                  onClick={() =>
                    void loadRelationships()
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
                  + Thêm quan hệ DEMO
                </button>
              </div>
            </div>


            <div className="category-stat-grid">
              <article>
                <span>
                  Quan hệ production
                </span>

                <strong>
                  {realCount}
                </strong>

                <small>
                  {realRelations.some(
                    item =>
                      item.source ===
                      'REAL API',
                  )
                    ? 'REAL API'
                    : 'REAL SNAPSHOT'}
                </small>
              </article>

              <article>
                <span>
                  Bản ghi giả lập
                </span>

                <strong>
                  {demoCount}
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
                  {allRelations.length}
                </strong>

                <small>
                  Có phân biệt nguồn
                </small>
              </article>
            </div>


            <div className="category-toolbar">
              <input
                value={keyword}
                placeholder="Tìm mã, tên hoặc mô tả quan hệ..."
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
                  REAL
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
                  Ngừng hoạt động
                </option>
              </select>
            </div>


            <div className="category-table-wrap">
              <table className="category-table">
                <thead>
                  <tr>
                    <th>
                      STT
                    </th>

                    <th>
                      Mã
                    </th>

                    <th>
                      Tên quan hệ
                    </th>

                    <th>
                      Mô tả
                    </th>

                    <th>
                      Nguồn
                    </th>

                    <th>
                      Trạng thái
                    </th>

                    <th>
                      Thao tác
                    </th>
                  </tr>
                </thead>

                <tbody>
                  {visibleRelations.map(
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
                          {item.name}
                        </td>

                        <td>
                          {item.description ||
                            '—'}
                        </td>

                        <td>
                          <span
                            className={
                              `category-source-badge ${sourceClass(
                                item.source,
                              )}`
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
                                    removeDemo(
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
                    visibleRelations.length ===
                      0 && (
                      <tr>
                        <td
                          colSpan={7}
                          className="category-empty"
                        >
                          Không có loại quan hệ
                          phù hợp với bộ lọc.
                        </td>
                      </tr>
                    )}

                  {loading && (
                    <tr>
                      <td
                        colSpan={7}
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
                {filteredRelations.length}
                {' '}
                bản ghi
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
                14 loại quan hệ REAL chỉ đọc.
                Thêm/Sửa/Bật-Tắt/Xóa hiện áp dụng
                cho dữ liệu DEMO LOCAL để không làm
                thay đổi cấu trúc hộ đã nhập production.
              </span>
            </div>
          </section>
        ) : selectedCategory ===
        'welfare-groups' ? (
          <WelfareGroupsCategoryPanel />
        ) : selectedCategory ===
        'policies' ? (
          <PoliciesCategoryPanel />
        ) : selectedCategory ===
        'areas' ? (
          <AreasCategoryPanel />
        ) : selectedCategory ===
        'payment-points' ? (
          <PaymentPointsCategoryPanel />
        ) : (
          <section className="category-workbench category-next-panel">
            <span
              className={
                `admin-feature-icon ${
                  selectedCard?.tone ??
                  'blue'
                }`
              }
            >
              <CategorySvg
                name={
                  selectedCard?.icon ??
                  'group'
                }
              />
            </span>

            <div>
              <h2>
                {selectedCard?.name}
              </h2>

              <p>
                Mục này là bước tiếp theo trong
                chuỗi hoàn thiện Danh mục. Sau khi
                khóa Loại quan hệ hộ, hệ thống sẽ
                triển khai đầy đủ dữ liệu, tìm kiếm,
                lọc và các nút nghiệp vụ tại đây.
              </p>

              <button
                type="button"
                className="category-button secondary"
                onClick={() =>
                  setSelectedCategory(
                    'relationship-types',
                  )
                }
              >
                ← Quay lại Loại quan hệ hộ
              </button>
            </div>
          </section>
        )}


        {(creating ||
          selectedRelation) && (
          <div className="category-modal-overlay">
            <div className="category-modal">

              <div className="category-modal-heading">
                <div>
                  <h2>
                    {creating
                      ? 'Thêm loại quan hệ DEMO'
                      : realOnlyModal
                        ? 'Chi tiết loại quan hệ REAL'
                        : 'Chỉnh sửa loại quan hệ DEMO'}
                  </h2>

                  <p>
                    {realOnlyModal
                      ? 'Bản ghi production ở chế độ chỉ đọc.'
                      : 'Dữ liệu chỉ lưu trong localStorage của trình duyệt.'}
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
                    Mã quan hệ
                  </span>

                  <input
                    disabled={realOnlyModal}
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
                    Tên quan hệ
                  </span>

                  <input
                    disabled={realOnlyModal}
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
                    disabled={realOnlyModal}
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
                    disabled={realOnlyModal}
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

                {realOnlyModal &&
                  selectedRelation && (
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

                {!realOnlyModal && (
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

      </div>
    </AppLayout>
  )
}

export default CategoriesPage