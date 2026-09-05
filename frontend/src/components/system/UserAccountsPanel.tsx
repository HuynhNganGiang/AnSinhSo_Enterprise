import {
  useEffect,
  useMemo,
  useState,
} from 'react'

import './UserAccountsPanel.css'


type Source =
  | 'REAL SNAPSHOT'
  | 'DEMO LOCAL'


type AccountStatus =
  | 'ACTIVE'
  | 'LOCKED'


type AccountRow = {
  id: string
  username: string
  email: string
  role: string
  source: Source
  status: AccountStatus
  accessFailedCount: number
  lockoutEnd: string | null
  createdAt: string
  updatedAt: string | null
}


type RoleSummary = {
  name: string
  assignedUsers: number
}


type DemoForm = {
  username: string
  email: string
  role: string
  status: AccountStatus
}


const DEMO_STORAGE_KEY =
  'ansinhso.demo.system-users.v1'


const PAGE_SIZE =
  8


const REAL_BACKOFFICE_ACCOUNTS:
  AccountRow[] =
[
  {
    id: 'real-backoffice-1',
    username: 'ngangiang',
    email: 'sonhuynh2014.bt@gmail.com',
    role: 'Backoffice',
    source: 'REAL SNAPSHOT',
    status: 'ACTIVE',
    accessFailedCount: 0,
    lockoutEnd: null,
    createdAt: '0001-01-01T00:00:00',
    updatedAt: null,
  },
]


const REAL_IDENTITY =
{
  total: 1,
  statusCode: 2,
  failedAttemptCount: 0,
  primaryPhone: '0900000001',
  createdAt: '0001-01-01T00:00:00',
}


const ROLE_SUMMARY:
  RoleSummary[] =
[
  {
    name: 'Admin',
    assignedUsers: 1,
  },
  {
    name: 'Citizen',
    assignedUsers: 0,
  },
  {
    name: 'Officer',
    assignedUsers: 0,
  }
]


function extractApiData(
  payload: any,
) {
  if (
    payload &&
    payload.data &&
    payload.data.data !== undefined
  ) {
    return payload.data.data
  }

  if (
    payload &&
    payload.data !== undefined
  ) {
    return payload.data
  }

  return payload
}


function readDemoRows():
  AccountRow[] {
  try {
    const raw =
      localStorage.getItem(
        DEMO_STORAGE_KEY,
      )

    if (!raw)
      return []

    const parsed =
      JSON.parse(raw)

    return Array.isArray(parsed)
      ? parsed as AccountRow[]
      : []
  } catch {
    return []
  }
}


function saveDemoRows(
  rows:
    AccountRow[],
) {
  localStorage.setItem(
    DEMO_STORAGE_KEY,
    JSON.stringify(rows),
  )
}


function formatDateTime(
  value:
    string | null,
) {
  if (!value) {
    return 'Chưa cập nhật'
  }

  const normalized =
    value.trim()

  if (
    !normalized ||
    normalized.startsWith(
      '0001-01-01',
    )
  ) {
    return 'Chưa cập nhật'
  }

  const date =
    new Date(normalized)

  if (
    Number.isNaN(
      date.getTime(),
    ) ||
    date.getFullYear() <= 1
  ) {
    return 'Chưa cập nhật'
  }

  return date.toLocaleString(
    'vi-VN',
  )
}


function maskPhone(
  value:
    string,
) {
  if (!value)
    return 'Chưa có'

  if (value.length <= 6)
    return value

  return `${value.slice(0, 3)}***${value.slice(-3)}`
}


function UserAccountsPanel() {

  const [
    demoRows,
    setDemoRows,
  ] =
    useState<AccountRow[]>(
      () =>
        readDemoRows(),
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
    useState<AccountRow | null>(
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
      username: '',
      email: '',
      role: 'Officer',
      status: 'ACTIVE',
    })

  const [
    currentRoles,
    setCurrentRoles,
  ] =
    useState<string[]>([])

  const [
    currentPermissions,
    setCurrentPermissions,
  ] =
    useState<string[]>([])

  const [
    authState,
    setAuthState,
  ] =
    useState<
      | 'loading'
      | 'ready'
      | 'missing-token'
      | 'error'
    >('loading')


  const loadCurrentAuthorization =
    async () => {

      const token =
        localStorage.getItem(
          'accessToken',
        )

      if (!token) {
        setCurrentRoles([])
        setCurrentPermissions([])
        setAuthState(
          'missing-token',
        )
        return
      }

      setAuthState(
        'loading',
      )

      try {

        const headers =
        {
          Authorization:
            `Bearer ${token}`,
        }

        const [
          roleResponse,
          permissionResponse,
        ] =
          await Promise.all([
            fetch(
              '/api/v1/users/me/roles',
              {
                headers,
              },
            ),

            fetch(
              '/api/v1/users/me/permissions',
              {
                headers,
              },
            ),
          ])

        if (
          !roleResponse.ok ||
          !permissionResponse.ok
        ) {
          throw new Error(
            'Authorization API failed.',
          )
        }

        const rolePayload =
          await roleResponse.json()

        const permissionPayload =
          await permissionResponse.json()

        const roleData =
          extractApiData(
            rolePayload,
          )

        const permissionData =
          extractApiData(
            permissionPayload,
          )

        const roleNames =
          Array.isArray(roleData)
            ? roleData
                .map(
                  item =>
                    typeof item ===
                    'string'
                      ? item
                      : (
                          item?.name ??
                          item?.Name ??
                          ''
                        ),
                )
                .filter(Boolean)
            : []

        const permissionNames =
          Array.isArray(
            permissionData,
          )
            ? permissionData
                .map(
                  item =>
                    String(
                      item,
                    ),
                )
                .filter(Boolean)
            : []

        setCurrentRoles(
          roleNames,
        )

        setCurrentPermissions(
          permissionNames,
        )

        setAuthState(
          'ready',
        )

      } catch {

        setCurrentRoles([])
        setCurrentPermissions([])
        setAuthState(
          'error',
        )
      }
    }


  useEffect(
    () => {
      void loadCurrentAuthorization()
    },
    [],
  )


  const allRows =
    useMemo(
      () => [
        ...REAL_BACKOFFICE_ACCOUNTS,
        ...demoRows,
      ],
      [demoRows],
    )


  const filteredRows =
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
              item.username
                .toLowerCase()
                .includes(q) ||
              item.email
                .toLowerCase()
                .includes(q) ||
              item.role
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
                item.status ===
                  'ACTIVE'
              ) ||
              (
                statusFilter ===
                  'locked' &&
                item.status ===
                  'LOCKED'
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
        filteredRows.length /
          PAGE_SIZE,
      ),
    )


  const currentPage =
    Math.min(
      page,
      totalPages,
    )


  const visibleRows =
    filteredRows.slice(
      (currentPage - 1) *
        PAGE_SIZE,
      currentPage *
        PAGE_SIZE,
    )


  const productionLocked =
    REAL_BACKOFFICE_ACCOUNTS.filter(
      item =>
        item.status ===
        'LOCKED',
    ).length


  const assignedRoleCount =
    ROLE_SUMMARY.reduce(
      (
        total,
        item,
      ) =>
        total +
        item.assignedUsers,
      0,
    )


  const persistDemo =
    (
      rows:
        AccountRow[],
    ) => {

      setDemoRows(
        rows,
      )

      saveDemoRows(
        rows,
      )
    }


  const openCreate =
    () => {

      setCreating(true)
      setSelected(null)

      setForm({
        username:
          `demo.user${demoRows.length + 1}`,
        email:
          `demo.user${demoRows.length + 1}@ansinhso.local`,
        role:
          'Officer',
        status:
          'ACTIVE',
      })
    }


  const openDetail =
    (
      item:
        AccountRow,
    ) => {

      setCreating(false)
      setSelected(
        item,
      )

      setForm({
        username:
          item.username,
        email:
          item.email,
        role:
          item.role,
        status:
          item.status,
      })
    }


  const closeModal =
    () => {

      setCreating(false)
      setSelected(null)
    }


  const saveDemo =
    () => {

      const username =
        form.username.trim()

      const email =
        form.email.trim()

      if (
        !username ||
        !email
      ) {
        window.alert(
          'Vui lòng nhập tên đăng nhập và email.',
        )
        return
      }

      const duplicated =
        allRows.some(
          item =>
            (
              item.username
                .toLowerCase() ===
                username.toLowerCase() ||
              item.email
                .toLowerCase() ===
                email.toLowerCase()
            ) &&
            item.id !==
              selected?.id,
        )

      if (duplicated) {
        window.alert(
          'Tên đăng nhập hoặc email đã tồn tại.',
        )
        return
      }

      if (creating) {

        const row:
          AccountRow =
        {
          id:
            `demo-system-user-${Date.now()}`,
          username,
          email,
          role:
            form.role,
          source:
            'DEMO LOCAL',
          status:
            form.status,
          accessFailedCount: 0,
          lockoutEnd:
            form.status ===
              'LOCKED'
              ? new Date(
                  Date.now() +
                  15 *
                  60 *
                  1000,
                ).toISOString()
              : null,
          createdAt:
            new Date()
              .toISOString(),
          updatedAt: null,
        }

        persistDemo([
          row,
          ...demoRows,
        ])

      } else if (
        selected?.source ===
        'DEMO LOCAL'
      ) {

        persistDemo(
          demoRows.map(
            item =>
              item.id ===
              selected.id
                ? {
                    ...item,
                    username,
                    email,
                    role:
                      form.role,
                    status:
                      form.status,
                    lockoutEnd:
                      form.status ===
                        'LOCKED'
                        ? (
                            item.lockoutEnd ??
                            new Date(
                              Date.now() +
                              15 *
                              60 *
                              1000,
                            ).toISOString()
                          )
                        : null,
                    updatedAt:
                      new Date()
                        .toISOString(),
                  }
                : item,
          ),
        )
      }

      closeModal()
    }


  const toggleDemoLock =
    (
      item:
        AccountRow,
    ) => {

      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      const locking =
        item.status !==
        'LOCKED'

      persistDemo(
        demoRows.map(
          row =>
            row.id ===
            item.id
              ? {
                  ...row,
                  status:
                    locking
                      ? 'LOCKED'
                      : 'ACTIVE',
                  lockoutEnd:
                    locking
                      ? new Date(
                          Date.now() +
                          15 *
                          60 *
                          1000,
                        ).toISOString()
                      : null,
                  updatedAt:
                    new Date()
                      .toISOString(),
                }
              : row,
        ),
      )
    }


  const deleteDemo =
    (
      item:
        AccountRow,
    ) => {

      if (
        item.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa tài khoản DEMO "${item.username}"?`,
        )
      ) {
        return
      }

      persistDemo(
        demoRows.filter(
          row =>
            row.id !==
            item.id,
        ),
      )
    }


  const selectedIsReal =
    selected?.source ===
    'REAL SNAPSHOT'


  return (
    <section className="system-users-panel">

      <div className="system-users-heading">

        <div>
          <div className="system-users-eyebrow">
            HỆ THỐNG 01
          </div>

          <h2>
            Người dùng & tài khoản
          </h2>

          <p>
            Quản lý tài khoản hệ thống theo nguyên tắc
            REAL-first. Dữ liệu production chỉ đọc;
            thao tác thử nghiệm được tách riêng bằng
            DEMO LOCAL.
          </p>
        </div>


        <div className="system-users-heading-actions">

          <button
            type="button"
            className="system-users-button secondary"
            onClick={() =>
              void loadCurrentAuthorization()
            }
          >
            ↻ Làm mới quyền phiên
          </button>

          <button
            type="button"
            className="system-users-button primary"
            onClick={openCreate}
          >
            + Thêm tài khoản DEMO
          </button>

        </div>

      </div>


      <div className="system-users-safety">

        <strong>
          Bảo vệ production
        </strong>

        <span>
          Không hiển thị PasswordHash, SecurityStamp,
          JWT hoặc OTP. Khóa/mở/sửa/xóa ở mục này chỉ
          áp dụng cho DEMO LOCAL. Gán quyền production
          được tách sang “Vai trò & phân quyền”.
        </span>

      </div>


      <div className="system-users-stats">

        <article>
          <span>
            Tài khoản backoffice
          </span>

          <strong>
            {REAL_BACKOFFICE_ACCOUNTS.length}
          </strong>

          <small>
            REAL SNAPSHOT
          </small>
        </article>


        <article>
          <span>
            Định danh công dân
          </span>

          <strong>
            {REAL_IDENTITY.total}
          </strong>

          <small>
            REAL SNAPSHOT
          </small>
        </article>


        <article>
          <span>
            Đang khóa production
          </span>

          <strong>
            {productionLocked}
          </strong>

          <small>
            Theo LockoutEnd
          </small>
        </article>


        <article>
          <span>
            Gán role production
          </span>

          <strong>
            {assignedRoleCount}
          </strong>

          <small>
            UserRoles
          </small>
        </article>

      </div>


      <div className="system-users-two-column">

        <article className="system-users-session-card">

          <div className="system-users-section-title">

            <div>
              <h3>
                Phiên đăng nhập hiện tại
              </h3>

              <p>
                Role và permission được đọc LIVE qua API.
              </p>
            </div>

            <span
              className={
                `system-users-live-badge ${
                  authState ===
                  'ready'
                    ? 'ready'
                    : ''
                }`
              }
            >
              {authState ===
                'loading'
                ? 'ĐANG TẢI'
                : authState ===
                    'ready'
                  ? 'LIVE API'
                  : authState ===
                      'missing-token'
                    ? 'CHƯA CÓ TOKEN'
                    : 'API ERROR'}
            </span>

          </div>


          <div className="system-users-session-row">

            <span>
              Vai trò
            </span>

            <div className="system-users-chip-list">

              {currentRoles.length >
              0 ? (
                currentRoles.map(
                  role => (
                    <span
                      key={role}
                      className="system-users-chip role"
                    >
                      {role}
                    </span>
                  ),
                )
              ) : (
                <em>
                  Chưa đọc được vai trò phiên.
                </em>
              )}

            </div>

          </div>


          <div className="system-users-session-row">

            <span>
              Quyền
            </span>

            <div className="system-users-permission-summary">

              <strong>
                {currentPermissions.length}
              </strong>

              <small>
                permission được cấp cho phiên hiện tại
              </small>

            </div>

          </div>


          {currentPermissions.length >
            0 && (

            <details className="system-users-permission-details">

              <summary>
                Xem danh sách quyền
              </summary>

              <div className="system-users-chip-list compact">

                {currentPermissions.map(
                  permission => (
                    <span
                      key={permission}
                      className="system-users-chip permission"
                    >
                      {permission}
                    </span>
                  ),
                )}

              </div>

            </details>
          )}

        </article>


        <article className="system-users-identity-card">

          <div className="system-users-section-title">

            <div>
              <h3>
                Định danh công dân production
              </h3>

              <p>
                Hiển thị thông tin trạng thái an toàn.
              </p>
            </div>

            <span className="system-users-source-badge real">
              REAL SNAPSHOT
            </span>

          </div>


          <dl>

            <div>
              <dt>
                Status code
              </dt>

              <dd>
                {REAL_IDENTITY.statusCode}
              </dd>
            </div>


            <div>
              <dt>
                Lần xác thực lỗi
              </dt>

              <dd>
                {REAL_IDENTITY.failedAttemptCount}
              </dd>
            </div>


            <div>
              <dt>
                Điện thoại
              </dt>

              <dd>
                {maskPhone(
                  REAL_IDENTITY.primaryPhone,
                )}
              </dd>
            </div>


            <div>
              <dt>
                Ngày tạo
              </dt>

              <dd>
                {formatDateTime(
                  REAL_IDENTITY.createdAt,
                )}
              </dd>
            </div>

          </dl>

        </article>

      </div>


      <div className="system-users-role-summary">

        <div className="system-users-section-title">

          <div>
            <h3>
              Phân bố vai trò production
            </h3>

            <p>
              Thống kê từ Roles và UserRoles.
            </p>
          </div>

        </div>


        <div className="system-users-role-grid">

          {ROLE_SUMMARY.map(
            item => (

              <article key={item.name}>

                <span>
                  {item.name}
                </span>

                <strong>
                  {item.assignedUsers}
                </strong>

                <small>
                  tài khoản được gán
                </small>

              </article>
            ),
          )}

        </div>

      </div>


      <div className="system-users-toolbar">

        <input
          value={keyword}
          placeholder="Tìm tên đăng nhập, email hoặc vai trò..."
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
            Không khóa
          </option>

          <option value="locked">
            Đang khóa
          </option>
        </select>

      </div>


      <div className="system-users-table-wrap">

        <table className="system-users-table">

          <thead>
            <tr>
              <th>STT</th>
              <th>Tên đăng nhập</th>
              <th>Email</th>
              <th>Loại / Vai trò</th>
              <th>Nguồn</th>
              <th>Đăng nhập lỗi</th>
              <th>Trạng thái</th>
              <th>Ngày tạo</th>
              <th>Thao tác</th>
            </tr>
          </thead>


          <tbody>

            {visibleRows.map(
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
                    <strong>
                      {item.username}
                    </strong>
                  </td>

                  <td>
                    {item.email}
                  </td>

                  <td>
                    {item.role}
                  </td>

                  <td>
                    <span
                      className={
                        `system-users-source-badge ${
                          item.source ===
                          'REAL SNAPSHOT'
                            ? 'real'
                            : 'demo'
                        }`
                      }
                    >
                      {item.source}
                    </span>
                  </td>

                  <td>
                    {item.accessFailedCount}
                  </td>

                  <td>
                    <span
                      className={
                        `system-users-status ${
                          item.status ===
                          'ACTIVE'
                            ? 'active'
                            : 'locked'
                        }`
                      }
                    >
                      {item.status ===
                      'ACTIVE'
                        ? 'Không khóa'
                        : 'Đang khóa'}
                    </span>
                  </td>

                  <td>
                    {formatDateTime(
                      item.createdAt,
                    )}
                  </td>

                  <td>

                    <div className="system-users-actions">

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
                              toggleDemoLock(
                                item,
                              )
                            }
                          >
                            {item.status ===
                            'LOCKED'
                              ? 'Mở khóa'
                              : 'Khóa'}
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


            {visibleRows.length ===
              0 && (

              <tr>
                <td
                  colSpan={9}
                  className="system-users-empty"
                >
                  Không tìm thấy tài khoản phù hợp.
                </td>
              </tr>
            )}

          </tbody>

        </table>

      </div>


      <div className="system-users-pagination">

        <span>
          {filteredRows.length} tài khoản hiển thị
        </span>

        <div>

          <button
            type="button"
            disabled={
              currentPage <= 1
            }
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


      {(creating || selected) && (

        <div className="system-users-modal-overlay">

          <div className="system-users-modal">

            <div className="system-users-modal-heading">

              <div>
                <h3>
                  {creating
                    ? 'Thêm tài khoản DEMO'
                    : selectedIsReal
                      ? 'Chi tiết tài khoản REAL'
                      : 'Chỉnh sửa tài khoản DEMO'}
                </h3>

                <p>
                  {selectedIsReal
                    ? 'Production chỉ đọc trong mục này.'
                    : 'Dữ liệu chỉ lưu trong localStorage của trình duyệt.'}
                </p>
              </div>

              <button
                type="button"
                className="system-users-modal-close"
                onClick={closeModal}
              >
                ×
              </button>

            </div>


            <div className="system-users-form">

              <label>
                <span>
                  Tên đăng nhập
                </span>

                <input
                  disabled={
                    selectedIsReal
                  }
                  value={
                    form.username
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      username:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Email
                </span>

                <input
                  disabled={
                    selectedIsReal
                  }
                  value={
                    form.email
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      email:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label>
                <span>
                  Vai trò trình diễn
                </span>

                <select
                  disabled={
                    selectedIsReal
                  }
                  value={
                    form.role
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      role:
                        event.target.value,
                    })
                  }
                >

                  <option value="Admin">
                    Admin
                  </option>

                  <option value="Officer">
                    Officer
                  </option>

                  <option value="Citizen">
                    Citizen
                  </option>

                </select>
              </label>


              <label>
                <span>
                  Trạng thái
                </span>

                <select
                  disabled={
                    selectedIsReal
                  }
                  value={
                    form.status
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      status:
                        event.target
                          .value as
                          AccountStatus,
                    })
                  }
                >

                  <option value="ACTIVE">
                    Không khóa
                  </option>

                  <option value="LOCKED">
                    Đang khóa
                  </option>

                </select>
              </label>

            </div>


            {selectedIsReal &&
              selected && (

              <div className="system-users-real-detail">

                <div>
                  <span>
                    AccessFailedCount
                  </span>

                  <strong>
                    {selected.accessFailedCount}
                  </strong>
                </div>

                <div>
                  <span>
                    LockoutEnd
                  </span>

                  <strong>
                    {formatDateTime(
                      selected.lockoutEnd,
                    )}
                  </strong>
                </div>

                <div>
                  <span>
                    CreatedAt
                  </span>

                  <strong>
                    {formatDateTime(
                      selected.createdAt,
                    )}
                  </strong>
                </div>

                <div>
                  <span>
                    UpdatedAt
                  </span>

                  <strong>
                    {formatDateTime(
                      selected.updatedAt,
                    )}
                  </strong>
                </div>

              </div>
            )}


            <div className="system-users-modal-actions">

              <button
                type="button"
                className="system-users-button secondary"
                onClick={closeModal}
              >
                Đóng
              </button>

              {!selectedIsReal && (

                <button
                  type="button"
                  className="system-users-button primary"
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

export default UserAccountsPanel