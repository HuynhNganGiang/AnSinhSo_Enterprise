import { useEffect, useMemo, useState } from 'react'

type AreaSource =
  | 'REAL DERIVED'
  | 'DEMO LOCAL'

type CoordinateSource =
  | 'SIMULATED CENTROID'
  | 'DEMO COORDINATE'

type AreaLevel =
  | 'Thôn / khu vực'
  | 'Xã'
  | 'Huyện'
  | 'Tỉnh'
  | 'Khu vực DEMO'

type AreaItem = {
  id: string
  name: string
  level: AreaLevel
  parentName: string
  province: string
  district: string
  ward: string
  householdCount: number
  latitude: number
  longitude: number
  coordinateSource: CoordinateSource
  active: boolean
  source: AreaSource
}

type AreaForm = {
  name: string
  level: AreaLevel
  parentName: string
  latitude: string
  longitude: string
  active: boolean
}

type UnknownRecord =
  Record<string, unknown>

const STORAGE_KEY =
  'ansinhso.demo.areas.v1'

const PAGE_SIZE =
  8

const SONG_LUY_CENTER = {
  latitude: 11.21011269694565,
  longitude: 108.32172004484949,
}

const defaultDemoAreas:
  AreaItem[] =
[
  {
    id: 'demo-area-01',
    name: 'Khu vực DEMO 01',
    level: 'Khu vực DEMO',
    parentName: 'Xã Sông Lũy',
    province: 'Bình Thuận',
    district: '',
    ward: 'Sông Lũy',
    householdCount: 0,
    latitude: 11.218,
    longitude: 108.310,
    coordinateSource: 'DEMO COORDINATE',
    active: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-area-02',
    name: 'Khu vực DEMO 02',
    level: 'Khu vực DEMO',
    parentName: 'Xã Sông Lũy',
    province: 'Bình Thuận',
    district: '',
    ward: 'Sông Lũy',
    householdCount: 0,
    latitude: 11.224,
    longitude: 108.329,
    coordinateSource: 'DEMO COORDINATE',
    active: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-area-03',
    name: 'Khu vực DEMO 03',
    level: 'Khu vực DEMO',
    parentName: 'Xã Sông Lũy',
    province: 'Bình Thuận',
    district: '',
    ward: 'Sông Lũy',
    householdCount: 0,
    latitude: 11.198,
    longitude: 108.314,
    coordinateSource: 'DEMO COORDINATE',
    active: true,
    source: 'DEMO LOCAL',
  },
  {
    id: 'demo-area-04',
    name: 'Khu vực DEMO 04',
    level: 'Khu vực DEMO',
    parentName: 'Xã Sông Lũy',
    province: 'Bình Thuận',
    district: '',
    ward: 'Sông Lũy',
    householdCount: 0,
    latitude: 11.203,
    longitude: 108.337,
    coordinateSource: 'DEMO COORDINATE',
    active: true,
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

function readText(
  row: UnknownRecord,
  ...keys: string[]
) {
  for (const key of keys) {
    const value = row[key]

    if (
      typeof value === 'string' &&
      value.trim()
    ) {
      return value.trim()
    }
  }

  return ''
}

function extractRows(
  body: unknown,
): unknown[] {
  if (Array.isArray(body))
    return body

  const root = asRecord(body)

  if (!root)
    return []

  if (Array.isArray(root.items))
    return root.items

  if (Array.isArray(root.data))
    return root.data

  const level1 =
    asRecord(root.data)

  if (!level1)
    return []

  if (Array.isArray(level1.items))
    return level1.items

  if (Array.isArray(level1.data))
    return level1.data

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

function extractTotalCount(
  body: unknown,
) {
  const root = asRecord(body)

  if (!root)
    return 0

  const candidates: unknown[] = [
    root.totalCount,
    root.total,
    root.count,
  ]

  const level1 =
    asRecord(root.data)

  if (level1) {
    candidates.push(
      level1.totalCount,
      level1.total,
      level1.count,
    )

    const level2 =
      asRecord(level1.data)

    if (level2) {
      candidates.push(
        level2.totalCount,
        level2.total,
        level2.count,
      )
    }
  }

  for (const value of candidates) {
    if (
      typeof value === 'number' &&
      Number.isFinite(value)
    ) {
      return value
    }
  }

  return 0
}

function readAddress(
  value: unknown,
) {
  const row =
    asRecord(value)

  if (!row)
    return null

  const nested =
    asRecord(
      row.address ??
      row.Address,
    )

  const ward =
    nested
      ? readText(
          nested,
          'ward',
          'Ward',
          'commune',
          'Commune',
        )
      : ''

  const district =
    nested
      ? readText(
          nested,
          'district',
          'District',
        )
      : ''

  const province =
    nested
      ? readText(
          nested,
          'province',
          'Province',
        )
      : ''

  return {
    ward:
      ward ||
      readText(
        row,
        'ward',
        'Ward',
        'addressWard',
        'AddressWard',
        'commune',
        'Commune',
      ),

    district:
      district ||
      readText(
        row,
        'district',
        'District',
        'addressDistrict',
        'AddressDistrict',
      ),

    province:
      province ||
      readText(
        row,
        'province',
        'Province',
        'addressProvince',
        'AddressProvince',
      ),
  }
}

function stableHash(
  text: string,
) {
  let hash = 2166136261

  for (
    let index = 0;
    index < text.length;
    index += 1
  ) {
    hash ^= text.charCodeAt(index)

    hash =
      Math.imul(
        hash,
        16777619,
      )
  }

  return hash >>> 0
}

function estimatedCentroid(
  key: string,
) {
  const a =
    stableHash(
      `${key}|a`,
    ) /
    4294967295

  const b =
    stableHash(
      `${key}|b`,
    ) /
    4294967295

  const radiusKm =
    0.45 +
    3.1 *
    Math.sqrt(b)

  const angle =
    Math.PI *
    2 *
    a

  const latitude =
    SONG_LUY_CENTER.latitude +
    (
      radiusKm *
      Math.cos(angle)
    ) /
    111.32

  const longitude =
    SONG_LUY_CENTER.longitude +
    (
      radiusKm *
      Math.sin(angle)
    ) /
    (
      111.32 *
      Math.cos(
        SONG_LUY_CENTER.latitude *
        Math.PI /
        180,
      )
    )

  return {
    latitude,
    longitude,
  }
}

function deriveAreas(
  households: unknown[],
): AreaItem[] {
  const buckets =
    new Map<
      string,
      {
        ward: string
        district: string
        province: string
        count: number
      }
    >()

  households.forEach(
    household => {
      const address =
        readAddress(household)

      if (!address)
        return

      const {
        ward,
        district,
        province,
      } = address

      let key = ''
      let displayName = ''

      if (ward) {
        key =
          `ward|${ward}|${district}|${province}`
        displayName = ward
      } else if (district) {
        key =
          `district|${district}|${province}`
        displayName = district
      } else if (province) {
        key =
          `province|${province}`
        displayName = province
      }

      if (!key || !displayName)
        return

      const current =
        buckets.get(key)

      if (current) {
        current.count += 1
      } else {
        buckets.set(
          key,
          {
            ward,
            district,
            province,
            count: 1,
          },
        )
      }
    },
  )

  return Array.from(
    buckets.entries(),
  )
    .map(
      (
        [key, value],
      ): AreaItem => {
        const name =
          value.ward ||
          value.district ||
          value.province

        const level: AreaLevel =
          value.ward
            ? 'Xã'
            : value.district
              ? 'Huyện'
              : 'Tỉnh'

        const parentName =
          value.ward
            ? value.district ||
              value.province
            : value.district
              ? value.province
              : ''

        const centroid =
          estimatedCentroid(key)

        return {
          id:
            `real-area-${stableHash(key)}`,

          name,
          level,
          parentName,
          province:
            value.province,
          district:
            value.district,
          ward:
            value.ward,
          householdCount:
            value.count,
          latitude:
            centroid.latitude,
          longitude:
            centroid.longitude,
          coordinateSource:
            'SIMULATED CENTROID',
          active: true,
          source:
            'REAL DERIVED',
        }
      },
    )
    .sort(
      (a, b) =>
        b.householdCount -
        a.householdCount ||
        a.name.localeCompare(
          b.name,
          'vi',
        ),
    )
}

function readStoredDemo():
  AreaItem[] | null {
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

    return parsed as AreaItem[]
  } catch {
    return null
  }
}

function persistDemo(
  rows: AreaItem[],
) {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify(rows),
  )
}

function AreasCategoryPanel() {
  const [
    realAreas,
    setRealAreas,
  ] =
    useState<AreaItem[]>([])

  const [
    demoAreas,
    setDemoAreas,
  ] =
    useState<AreaItem[]>(
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
      'Đang tổng hợp địa bàn từ địa chỉ hộ production...',
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
    levelFilter,
    setLevelFilter,
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
    useState<AreaItem | null>(
      null,
    )

  const [
    form,
    setForm,
  ] =
    useState<AreaForm>({
      name: '',
      level:
        'Khu vực DEMO',
      parentName:
        'Xã Sông Lũy',
      latitude:
        String(
          SONG_LUY_CENTER.latitude,
        ),
      longitude:
        String(
          SONG_LUY_CENTER.longitude,
        ),
      active: true,
    })

  const ensureDemo =
    () => {
      const stored =
        readStoredDemo()

      if (stored === null) {
        persistDemo(
          defaultDemoAreas,
        )

        setDemoAreas(
          defaultDemoAreas,
        )
      } else {
        setDemoAreas(stored)
      }
    }

  const load =
    async () => {
      setLoading(true)

      const token =
        localStorage.getItem(
          'accessToken',
        )

      try {
        const firstResponse =
          await fetch(
            '/api/v1/households?page=1&pageSize=200',
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

        if (!firstResponse.ok) {
          throw new Error(
            `HTTP ${firstResponse.status}`,
          )
        }

        const firstBody =
          (await firstResponse.json()) as unknown

        let rows =
          extractRows(firstBody)

        const totalCount =
          extractTotalCount(
            firstBody,
          )

        if (
          totalCount >
          rows.length
        ) {
          const totalPages =
            Math.ceil(
              totalCount /
              200,
            )

          for (
            let currentPage = 2;
            currentPage <=
              totalPages;
            currentPage += 1
          ) {
            const response =
              await fetch(
                `/api/v1/households?page=${currentPage}&pageSize=200`,
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
              break

            const body =
              (await response.json()) as unknown

            rows = [
              ...rows,
              ...extractRows(body),
            ]
          }
        }

        const derived =
          deriveAreas(rows)

        if (derived.length === 0) {
          throw new Error(
            'NO_USABLE_ADDRESS_FIELDS',
          )
        }

        setRealAreas(
          derived,
        )

        ensureDemo()

        setMessage(
          `${derived.length} địa bàn được tổng hợp từ địa chỉ hộ production. Tọa độ tâm khu vực chỉ là SIMULATED và không ghi SQL.`,
        )
      } catch {
        setRealAreas([])

        ensureDemo()

        setMessage(
          'API hộ chưa trả đủ trường địa chỉ để tổng hợp địa bàn. Đang dùng DEMO LOCAL; không ghi SQL production.',
        )
      } finally {
        setLoading(false)
      }
    }

  useEffect(() => {
    void load()
  }, [])

  const allAreas =
    useMemo(
      () => [
        ...realAreas,
        ...demoAreas,
      ],
      [
        realAreas,
        demoAreas,
      ],
    )

  const filtered =
    useMemo(
      () => {
        const q =
          keyword
            .trim()
            .toLowerCase()

        return allAreas.filter(
          item => {
            const keywordOk =
              !q ||
              item.name
                .toLowerCase()
                .includes(q) ||
              item.parentName
                .toLowerCase()
                .includes(q) ||
              item.district
                .toLowerCase()
                .includes(q) ||
              item.province
                .toLowerCase()
                .includes(q)

            const sourceOk =
              sourceFilter ===
                'all' ||
              (
                sourceFilter ===
                  'real' &&
                item.source ===
                  'REAL DERIVED'
              ) ||
              (
                sourceFilter ===
                  'demo' &&
                item.source ===
                  'DEMO LOCAL'
              )

            const levelOk =
              levelFilter ===
                'all' ||
              item.level ===
                levelFilter

            return (
              keywordOk &&
              sourceOk &&
              levelOk
            )
          },
        )
      },
      [
        allAreas,
        keyword,
        sourceFilter,
        levelFilter,
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

  const householdsCovered =
    realAreas.reduce(
      (
        total,
        item,
      ) =>
        total +
        item.householdCount,
      0,
    )

  const updateDemo =
    (
      rows: AreaItem[],
    ) => {
      setDemoAreas(rows)
      persistDemo(rows)
    }

  const openCreate =
    () => {
      setCreating(true)
      setSelected(null)

      setForm({
        name: '',
        level:
          'Khu vực DEMO',
        parentName:
          'Xã Sông Lũy',
        latitude:
          String(
            SONG_LUY_CENTER.latitude,
          ),
        longitude:
          String(
            SONG_LUY_CENTER.longitude,
          ),
        active: true,
      })
    }

  const openDetail =
    (
      item: AreaItem,
    ) => {
      setCreating(false)
      setSelected(item)

      setForm({
        name:
          item.name,
        level:
          item.level,
        parentName:
          item.parentName,
        latitude:
          String(
            item.latitude,
          ),
        longitude:
          String(
            item.longitude,
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

      if (!name) {
        window.alert(
          'Vui lòng nhập tên địa bàn.',
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

      if (creating) {
        const next:
          AreaItem =
        {
          id:
            `demo-area-${Date.now()}`,
          name,
          level:
            form.level,
          parentName:
            form.parentName.trim(),
          province:
            'Bình Thuận',
          district: '',
          ward:
            'Sông Lũy',
          householdCount: 0,
          latitude,
          longitude,
          coordinateSource:
            'DEMO COORDINATE',
          active:
            form.active,
          source:
            'DEMO LOCAL',
        }

        updateDemo([
          next,
          ...demoAreas,
        ])
      } else if (
        selected?.source ===
        'DEMO LOCAL'
      ) {
        updateDemo(
          demoAreas.map(
            item =>
              item.id ===
              selected.id
                ? {
                    ...item,
                    name,
                    level:
                      form.level,
                    parentName:
                      form.parentName.trim(),
                    latitude,
                    longitude,
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
      item: AreaItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      updateDemo(
        demoAreas.map(
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
      item: AreaItem,
    ) => {
      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa địa bàn giả lập "${item.name}"?`,
        )
      ) {
        return
      }

      updateDemo(
        demoAreas.filter(
          row =>
            row.id !==
            item.id,
        ),
      )
    }

  const realOnly =
    selected?.source ===
    'REAL DERIVED'

  return (
    <section className="category-workbench">

      <div className="category-workbench-heading">

        <div className="category-title-line">
          <span className="admin-feature-icon orange small">
            <svg
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.8"
              aria-hidden="true"
            >
              <path d="M12 21s6-5.4 6-11A6 6 0 0 0 6 10c0 5.6 6 11 6 11Z" />
              <circle cx="12" cy="10" r="2" />
            </svg>
          </span>

          <div>
            <h2>
              Địa bàn quản lý
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
            Xem bản đồ
          </button>

          <button
            type="button"
            className="category-button primary"
            onClick={openCreate}
          >
            + Thêm địa bàn DEMO
          </button>

        </div>
      </div>


      <div className="category-stat-grid">

        <article>
          <span>
            Địa bàn từ production
          </span>

          <strong>
            {realAreas.length}
          </strong>

          <small>
            REAL DERIVED
          </small>
        </article>

        <article>
          <span>
            Địa bàn giả lập
          </span>

          <strong>
            {demoAreas.length}
          </strong>

          <small>
            localStorage
          </small>
        </article>

        <article>
          <span>
            Hộ đã tổng hợp
          </span>

          <strong>
            {householdsCovered}
          </strong>

          <small>
            Từ địa chỉ REAL
          </small>
        </article>

        <article>
          <span>
            Tọa độ khu vực
          </span>

          <strong>
            {allAreas.length}
          </strong>

          <small>
            SIMULATED / DEMO
          </small>
        </article>

      </div>


      <div className="category-toolbar">

        <input
          value={keyword}
          placeholder="Tìm địa bàn, huyện, tỉnh..."
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
            REAL DERIVED
          </option>

          <option value="demo">
            DEMO LOCAL
          </option>
        </select>

        <select
          value={levelFilter}
          onChange={event => {
            setLevelFilter(
              event.target.value,
            )
            setPage(1)
          }}
        >
          <option value="all">
            Tất cả cấp
          </option>

          <option value="Xã">
            Xã
          </option>

          <option value="Huyện">
            Huyện
          </option>

          <option value="Tỉnh">
            Tỉnh
          </option>

          <option value="Khu vực DEMO">
            Khu vực DEMO
          </option>
        </select>

      </div>


      <div className="category-table-wrap">

        <table className="category-table">

          <thead>
            <tr>
              <th>STT</th>
              <th>Địa bàn</th>
              <th>Cấp</th>
              <th>Địa bàn cha</th>
              <th>Số hộ</th>
              <th>Tọa độ tâm</th>
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
                    {item.level}
                  </td>

                  <td>
                    {item.parentName ||
                      '—'}
                  </td>

                  <td>
                    {item.householdCount}
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
                        color: '#c2410c',
                      }}
                    >
                      {item.coordinateSource}
                    </small>
                  </td>

                  <td>
                    <span
                      className={
                        `category-source-badge ${
                          item.source ===
                          'REAL DERIVED'
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
              visible.length === 0 && (
                <tr>
                  <td
                    colSpan={9}
                    className="category-empty"
                  >
                    Không có địa bàn phù hợp
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
                  Đang tổng hợp địa bàn...
                </td>
              </tr>
            )}

          </tbody>
        </table>

      </div>


      <div className="category-pagination">

        <span>
          {filtered.length} địa bàn
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
          Quy tắc tọa độ:
        </strong>

        <span>
          Hộ có tọa độ thật tiếp tục dùng tọa độ
          production trên Bản đồ số. Hộ thiếu tọa độ
          được MapQueryService ước tính theo địa chỉ.
          Tọa độ tâm địa bàn tại bảng này chỉ phục vụ
          trình diễn và luôn được đánh dấu SIMULATED;
          không ghi vào SQL production.
        </span>

      </div>


      {(creating || selected) && (

        <div className="category-modal-overlay">

          <div className="category-modal">

            <div className="category-modal-heading">

              <div>
                <h2>
                  {creating
                    ? 'Thêm địa bàn DEMO'
                    : realOnly
                      ? 'Chi tiết địa bàn REAL'
                      : 'Chỉnh sửa địa bàn DEMO'}
                </h2>

                <p>
                  {realOnly
                    ? 'Địa bàn được suy ra từ địa chỉ production và ở chế độ chỉ đọc.'
                    : 'Dữ liệu DEMO chỉ lưu localStorage.'}
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
                  Tên địa bàn
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
                  Cấp địa bàn
                </span>

                <select
                  disabled={realOnly}
                  value={form.level}
                  onChange={event =>
                    setForm({
                      ...form,
                      level:
                        event.target.value as AreaLevel,
                    })
                  }
                  style={{
                    width: '100%',
                    border: '1px solid #dfe6ef',
                    borderRadius: '8px',
                    padding: '9px 10px',
                  }}
                >
                  <option value="Khu vực DEMO">
                    Khu vực DEMO
                  </option>

                  <option value="Xã">
                    Xã
                  </option>

                  <option value="Huyện">
                    Huyện
                  </option>

                  <option value="Tỉnh">
                    Tỉnh
                  </option>
                </select>
              </label>


              <label className="full">
                <span>
                  Địa bàn cha
                </span>

                <input
                  disabled={realOnly}
                  value={
                    form.parentName
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      parentName:
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

              {!realOnly && (
                <button
                  type="button"
                  className="category-button primary"
                  onClick={saveDemo}
                >
                  Lưu DEMO
                </button>
              )}

              {realOnly && (
                <button
                  type="button"
                  className="category-button primary"
                  onClick={() => {
                    window.location.href =
                      '/map'
                  }}
                >
                  Xem Bản đồ số
                </button>
              )}

            </div>

          </div>

        </div>
      )}

    </section>
  )
}

export default AreasCategoryPanel