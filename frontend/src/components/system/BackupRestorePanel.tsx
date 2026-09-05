import {
  useCallback,
  useEffect,
  useMemo,
  useState,
} from 'react'
import './BackupRestorePanel.css'

const CONFIRM_TEXT =
  'BACKUP AnSinhSoRealDb'

type BackupItem = {
  backupSetId: number
  backupType: string
  backupStart: string
  backupFinish: string
  backupSizeMB: number
  physicalDevice: string | null
  copyOnly: boolean
}

type BackupStatus = {
  source: string
  generatedAt: string

  database: {
    name: string
    runtimeDatabase: string
    isProductionRuntime: boolean
    state: string
    recoveryModel: string
    userAccess: string
    isReadOnly: boolean
    approxSizeMB: number
  }

  backup: {
    targetDatabase: string
    defaultPath: string | null
    pathAvailable: boolean
    isSysAdmin: boolean
    isDbOwner: boolean
    canBackupDatabase: boolean
    canBackupLog: boolean
    createEnabled: boolean
    policy: string
  }

  restore: {
    mode: string
    productionRestoreEnabled: boolean
    reason: string
  }

  history: BackupItem[]
}

type Envelope = {
  message?: string
  data?: unknown
}

function isRecord(
  value: unknown,
): value is Record<string, unknown> {
  return (
    typeof value === 'object' &&
    value !== null
  )
}

function isBackupStatus(
  value: unknown,
): value is BackupStatus {
  if (!isRecord(value)) {
    return false
  }

  return (
    typeof value.source === 'string' &&
    isRecord(value.database) &&
    isRecord(value.backup) &&
    isRecord(value.restore) &&
    Array.isArray(value.history)
  )
}

function extractStatus(
  payload: unknown,
): BackupStatus | null {
  let current: unknown =
    payload

  for (
    let depth = 0;
    depth < 4;
    depth += 1
  ) {
    if (isBackupStatus(current)) {
      return current
    }

    if (
      !isRecord(current) ||
      !('data' in current)
    ) {
      return null
    }

    current =
      current.data
  }

  return isBackupStatus(current)
    ? current
    : null
}

function fileName(
  path: string | null,
) {
  if (!path) {
    return 'Không xác định'
  }

  const parts =
    path.split(/[\\/]/)

  return (
    parts[parts.length - 1] ||
    path
  )
}

function formatDate(
  value: string,
) {
  const date =
    new Date(value)

  return Number.isNaN(
    date.getTime(),
  )
    ? value
    : date.toLocaleString(
        'vi-VN',
      )
}

function flag(
  value: boolean,
) {
  return value
    ? 'Có'
    : 'Không'
}

function BackupRestorePanel() {
  const [status, setStatus] =
    useState<BackupStatus | null>(
      null,
    )

  const [loading, setLoading] =
    useState(true)

  const [creating, setCreating] =
    useState(false)

  const [
    confirmation,
    setConfirmation,
  ] =
    useState('')

  const [message, setMessage] =
    useState<string | null>(
      null,
    )

  const [error, setError] =
    useState<string | null>(
      null,
    )

  const loadStatus =
    useCallback(
      async () => {
        setLoading(true)
        setError(null)

        try {
          const token =
            localStorage.getItem(
              'accessToken',
            )

          const response =
            await fetch(
              '/api/v1/system/backup-status',
              {
                headers: token
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
            (await response.json()) as
              Envelope

          const data =
            extractStatus(body)

          if (!data) {
            throw new Error(
              'Backup status response shape is invalid.',
            )
          }

          setStatus(data)
        } catch (requestError) {
          setError(
            requestError instanceof Error
              ? requestError.message
              : 'Không đọc được trạng thái backup.',
          )
        } finally {
          setLoading(false)
        }
      },
      [],
    )

  useEffect(
    () => {
      void loadStatus()
    },
    [loadStatus],
  )

  const canCreate =
    useMemo(
      () =>
        status?.backup.createEnabled ===
          true &&
        confirmation ===
          CONFIRM_TEXT &&
        !creating,
      [
        status,
        confirmation,
        creating,
      ],
    )

  async function createBackup() {
    if (!canCreate) {
      return
    }

    setCreating(true)
    setError(null)
    setMessage(null)

    try {
      const token =
        localStorage.getItem(
          'accessToken',
        )

      const response =
        await fetch(
          '/api/v1/system/backups',
          {
            method: 'POST',

            headers: token
              ? {
                  Authorization:
                    `Bearer ${token}`,
                }
              : {},
          },
        )

      const body =
        (await response.json()) as
          Envelope

      if (!response.ok) {
        throw new Error(
          typeof body.message ===
            'string'
            ? body.message
            : `HTTP ${response.status}`,
        )
      }

      let output =
        'Backup REAL và VERIFY thành công.'

      if (
        isRecord(body.data) &&
        typeof body.data.fileName ===
          'string'
      ) {
        output =
          `Backup REAL và VERIFY thành công: ${body.data.fileName}`
      }

      setMessage(output)
      setConfirmation('')

      await loadStatus()
    } catch (requestError) {
      setError(
        requestError instanceof Error
          ? requestError.message
          : 'Tạo backup thất bại.',
      )
    } finally {
      setCreating(false)
    }
  }

  return (
    <section className="backup-panel">
      <header className="backup-header">
        <div>
          <div className="backup-eyebrow">
            HỆ THỐNG 05
          </div>

          <h2>
            Sao lưu &amp; phục hồi
          </h2>

          <p>
            Backup SQL Server thật cho
            AnSinhSoRealDb. Restore production
            đang khóa an toàn.
          </p>
        </div>

        <div className="backup-header-actions">
          <span className="backup-source">
            {status?.source ??
              'REAL SQL SERVER'}
          </span>

          <button
            type="button"
            className="backup-button secondary"
            onClick={() =>
              void loadStatus()
            }
            disabled={loading}
          >
            ↻ Làm mới
          </button>
        </div>
      </header>

      {error && (
        <div className="backup-alert error">
          {error}
        </div>
      )}

      {message && (
        <div className="backup-alert success">
          {message}
        </div>
      )}

      {loading && !status && (
        <div className="backup-loading">
          Đang đọc SQL Server...
        </div>
      )}

      {status && (
        <>
          <div className="backup-summary-grid">
            <article>
              <span>Database</span>

              <strong>
                {status.database.name}
              </strong>

              <small>
                {status.database.state}
                {' · '}
                {status.database.recoveryModel}
              </small>
            </article>

            <article>
              <span>Runtime DB</span>

              <strong>
                {status.database.runtimeDatabase}
              </strong>

              <small>
                {status.database.isProductionRuntime
                  ? 'Production runtime'
                  : 'Không phải production'}
              </small>
            </article>

            <article>
              <span>Dung lượng</span>

              <strong>
                {status.database.approxSizeMB.toLocaleString(
                  'vi-VN',
                )}{' '}
                MB
              </strong>

              <small>
                {status.database.userAccess}
              </small>
            </article>

            <article className="locked-card">
              <span>
                Production restore
              </span>

              <strong>
                {status.restore.mode}
              </strong>

              <small>
                Không có API restore
              </small>
            </article>
          </div>

          <div className="backup-main-grid">
            <article className="backup-card">
              <div className="backup-card-heading">
                <div>
                  <h3>
                    FULL backup REAL
                  </h3>

                  <p>
                    {status.backup.policy}
                  </p>
                </div>

                <span
                  className={
                    status.backup.createEnabled
                      ? 'backup-tag ready'
                      : 'backup-tag blocked'
                  }
                >
                  {status.backup.createEnabled
                    ? 'READY'
                    : 'BLOCKED'}
                </span>
              </div>

              <dl className="backup-details">
                <div>
                  <dt>Target DB</dt>
                  <dd>
                    {status.backup.targetDatabase}
                  </dd>
                </div>

                <div>
                  <dt>Backup path</dt>
                  <dd>
                    {status.backup.defaultPath ??
                      'Không xác định'}
                  </dd>
                </div>

                <div>
                  <dt>Path available</dt>
                  <dd>
                    {flag(
                      status.backup.pathAvailable,
                    )}
                  </dd>
                </div>

                <div>
                  <dt>sysadmin</dt>
                  <dd>
                    {flag(
                      status.backup.isSysAdmin,
                    )}
                  </dd>
                </div>

                <div>
                  <dt>db_owner</dt>
                  <dd>
                    {flag(
                      status.backup.isDbOwner,
                    )}
                  </dd>
                </div>

                <div>
                  <dt>BACKUP DATABASE</dt>
                  <dd>
                    {flag(
                      status.backup.canBackupDatabase,
                    )}
                  </dd>
                </div>
              </dl>

              <div className="backup-confirm">
                <label
                  htmlFor="backup-confirm"
                >
                  Để tạo file .bak thật, nhập:
                </label>

                <code>
                  {CONFIRM_TEXT}
                </code>

                <input
                  id="backup-confirm"
                  value={confirmation}
                  onChange={event =>
                    setConfirmation(
                      event.target.value,
                    )
                  }
                  placeholder={CONFIRM_TEXT}
                  autoComplete="off"
                  spellCheck={false}
                />

                <button
                  type="button"
                  className="backup-button primary"
                  disabled={!canCreate}
                  onClick={() =>
                    void createBackup()
                  }
                >
                  {creating
                    ? 'Đang backup + VERIFY...'
                    : 'Tạo FULL backup REAL'}
                </button>

                <small>
                  Ở bước kiểm tra UI hiện tại,
                  chưa nhập chuỗi xác nhận.
                </small>
              </div>
            </article>

            <article className="backup-card restore-card">
              <div className="backup-card-heading">
                <div>
                  <h3>
                    Phục hồi production
                  </h3>

                  <p>
                    Chưa mở thao tác phá hủy.
                  </p>
                </div>

                <span className="backup-tag locked">
                  LOCKED
                </span>
              </div>

              <div className="restore-lock">
                <strong>
                  RESTORE DATABASE bị khóa
                </strong>

                <p>
                  {status.restore.reason}
                </p>

                <span>
                  ✓ Không có API restore
                </span>

                <span>
                  ✓ Không tự SINGLE_USER
                </span>

                <span>
                  ✓ Không ghi đè production
                </span>

                <span>
                  ✓ Cần maintenance workflow riêng
                </span>
              </div>
            </article>
          </div>

          <article className="backup-history">
            <div className="backup-card-heading">
              <div>
                <h3>
                  Lịch sử backup SQL
                </h3>

                <p>
                  REAL READ ONLY từ msdb.
                </p>
              </div>

              <span className="backup-tag">
                {status.history.length}{' '}
                bản ghi
              </span>
            </div>

            {status.history.length === 0 ? (
              <div className="backup-empty">
                Chưa có backup nào của
                AnSinhSoRealDb.
              </div>
            ) : (
              <div className="backup-table-wrap">
                <table>
                  <thead>
                    <tr>
                      <th>File</th>
                      <th>Loại</th>
                      <th>Hoàn tất</th>
                      <th>Dung lượng</th>
                      <th>COPY_ONLY</th>
                    </tr>
                  </thead>

                  <tbody>
                    {status.history.map(
                      item => (
                        <tr
                          key={
                            item.backupSetId
                          }
                        >
                          <td>
                            {fileName(
                              item.physicalDevice,
                            )}
                          </td>

                          <td>
                            {item.backupType}
                          </td>

                          <td>
                            {formatDate(
                              item.backupFinish,
                            )}
                          </td>

                          <td>
                            {item.backupSizeMB.toLocaleString(
                              'vi-VN',
                            )}{' '}
                            MB
                          </td>

                          <td>
                            {item.copyOnly
                              ? 'Có'
                              : 'Không'}
                          </td>
                        </tr>
                      ),
                    )}
                  </tbody>
                </table>
              </div>
            )}
          </article>
        </>
      )}
    </section>
  )
}

export default BackupRestorePanel