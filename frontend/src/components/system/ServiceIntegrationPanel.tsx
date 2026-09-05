import {
  useCallback,
  useEffect,
  useState,
} from 'react'
import './ServiceIntegrationPanel.css'

type IntegrationStatus = {
  source: string
  generatedAt: string

  database: {
    runtimeDatabase: string
    productionDatabase: string
    isProductionRuntime: boolean
  }

  zaloOA: {
    implementation: string
    liveConnection: string
    liveConnectionVerified: boolean
    sectionName: string
    sectionPresent: boolean
    configuredEntries: number
    expectedEntries: number
    appIdConfigured: boolean
    appSecretConfigured: boolean
    accessTokenConfigured: boolean
    refreshTokenConfigured: boolean
    externalNetworkCallPerformed: boolean
  }

  notificationDelivery: {
    implementation: string
    externalZaloDelivery: boolean
    simulatedSuccess: boolean
    warning: string
  }

  webhook: {
    implementation: string
    route: string
    signatureValidation: boolean
    liveWebhookVerified: boolean
  }

  smsOtp: {
    implementation: string
    sectionName: string
    sectionPresent: boolean
    configuredEntries: number
    expectedEntries: number
    configured: boolean
    liveDeliveryVerified: boolean
  }

  productionData: {
    zaloUsers: number
    notifications: number
    notificationHistories: number
  }

  security: {
    secretsReturned: boolean
    tokensReturned: boolean
    credentialsReturned: boolean
    externalCallsPerformed: boolean
    readOnlyEndpoint: boolean
  }
}

type Envelope = {
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

function isStatus(
  value: unknown,
): value is IntegrationStatus {
  if (!isRecord(value)) {
    return false
  }

  return (
    typeof value.source === 'string' &&
    isRecord(value.database) &&
    isRecord(value.zaloOA) &&
    isRecord(value.notificationDelivery) &&
    isRecord(value.webhook) &&
    isRecord(value.smsOtp) &&
    isRecord(value.productionData) &&
    isRecord(value.security)
  )
}

function unwrap(
  payload: unknown,
): IntegrationStatus | null {
  let current: unknown =
    payload

  for (
    let depth = 0;
    depth < 4;
    depth += 1
  ) {
    if (isStatus(current)) {
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

  return isStatus(current)
    ? current
    : null
}

function yesNo(
  value: boolean,
) {
  return value
    ? 'Có'
    : 'Không'
}

function ServiceIntegrationPanel() {
  const [status, setStatus] =
    useState<IntegrationStatus | null>(
      null,
    )

  const [loading, setLoading] =
    useState(true)

  const [error, setError] =
    useState<string | null>(
      null,
    )

  const load =
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
              '/api/v1/system/integration-status',
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
            unwrap(body)

          if (!data) {
            throw new Error(
              'Invalid integration status response.',
            )
          }

          setStatus(data)
        } catch (requestError) {
          setError(
            requestError instanceof Error
              ? requestError.message
              : 'Không đọc được trạng thái tích hợp.',
          )
        } finally {
          setLoading(false)
        }
      },
      [],
    )

  useEffect(
    () => {
      void load()
    },
    [load],
  )

  return (
    <section className="service-integration-panel">
      <header className="service-integration-header">
        <div>
          <div className="service-eyebrow">
            HỆ THỐNG 06
          </div>

          <h2>
            Tích hợp dịch vụ
          </h2>

          <p>
            Trạng thái thật của Zalo OA,
            webhook, luồng thông báo và SMS OTP.
          </p>
        </div>

        <div className="service-actions">
          <span className="service-source">
            {status?.source ??
              'RUNTIME STATUS'}
          </span>

          <button
            type="button"
            disabled={loading}
            onClick={() =>
              void load()
            }
          >
            ↻ Làm mới
          </button>
        </div>
      </header>

      <div className="service-secret-banner">
        <strong>
          NO SECRET VALUES
        </strong>

        <span>
          Secret, token, API key, OTP và
          credential không được trả về UI.
        </span>
      </div>

      {error && (
        <div className="service-error">
          {error}
        </div>
      )}

      {loading && !status && (
        <div className="service-loading">
          Đang đọc trạng thái runtime...
        </div>
      )}

      {status && (
        <>
          <div className="service-summary">
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
              <span>Zalo OA core</span>

              <strong>
                REAL API
              </strong>

              <small>
                {status.zaloOA.configuredEntries}/
                {status.zaloOA.expectedEntries}
                {' '}configured
              </small>
            </article>

            <article className="service-warn">
              <span>Notification → Zalo</span>

              <strong>
                MOCK
              </strong>

              <small>
                Chưa gửi Zalo thật
              </small>
            </article>

            <article>
              <span>SMS OTP</span>

              <strong>
                {status.smsOtp.configured
                  ? 'CONFIGURED'
                  : 'NOT CONFIGURED'}
              </strong>

              <small>
                HTTP provider implemented
              </small>
            </article>
          </div>

          <div className="service-grid">
            <article className="service-card">
              <header>
                <div>
                  <h3>Zalo Official Account</h3>
                  <p>OAuth / OpenAPI implementation</p>
                </div>

                <b className="tag real">
                  REAL API IMPLEMENTATION
                </b>
              </header>

              <dl>
                <div>
                  <dt>Config section</dt>
                  <dd>{status.zaloOA.sectionName}</dd>
                </div>

                <div>
                  <dt>Metadata</dt>
                  <dd>
                    {status.zaloOA.configuredEntries}/
                    {status.zaloOA.expectedEntries}
                  </dd>
                </div>

                <div>
                  <dt>App ID</dt>
                  <dd>
                    {status.zaloOA.appIdConfigured
                      ? 'Đã cấu hình'
                      : 'Chưa cấu hình'}
                  </dd>
                </div>

                <div>
                  <dt>AppSecret</dt>
                  <dd>
                    {status.zaloOA.appSecretConfigured
                      ? 'Đã cấu hình · hidden'
                      : 'Chưa cấu hình'}
                  </dd>
                </div>

                <div>
                  <dt>Access token</dt>
                  <dd>
                    {status.zaloOA.accessTokenConfigured
                      ? 'Đã cấu hình · hidden'
                      : 'Chưa cấu hình'}
                  </dd>
                </div>

                <div>
                  <dt>Refresh token</dt>
                  <dd>
                    {status.zaloOA.refreshTokenConfigured
                      ? 'Đã cấu hình · hidden'
                      : 'Chưa cấu hình'}
                  </dd>
                </div>

                <div>
                  <dt>Live connection</dt>
                  <dd>
                    <b className="tag pending">
                      NOT VERIFIED
                    </b>
                  </dd>
                </div>
              </dl>
            </article>

            <article className="service-card mock-card">
              <header>
                <div>
                  <h3>Notification → Zalo</h3>
                  <p>Luồng gửi thông báo hiện tại</p>
                </div>

                <b className="tag mock">
                  MOCK
                </b>
              </header>

              <div className="mock-warning">
                <strong>
                  Chưa gửi thông báo Zalo thật
                </strong>

                <p>
                  {status.notificationDelivery.warning}
                </p>

                <span>
                  External delivery:
                  {' '}
                  {yesNo(
                    status.notificationDelivery
                      .externalZaloDelivery,
                  )}
                </span>

                <span>
                  Simulated success:
                  {' '}
                  {yesNo(
                    status.notificationDelivery
                      .simulatedSuccess,
                  )}
                </span>
              </div>
            </article>

            <article className="service-card">
              <header>
                <div>
                  <h3>Zalo Webhook</h3>
                  <p>Nhận và xác thực callback</p>
                </div>

                <b className="tag implemented">
                  IMPLEMENTED
                </b>
              </header>

              <dl>
                <div>
                  <dt>Route</dt>
                  <dd>{status.webhook.route}</dd>
                </div>

                <div>
                  <dt>Signature validation</dt>
                  <dd>
                    {yesNo(
                      status.webhook.signatureValidation,
                    )}
                  </dd>
                </div>

                <div>
                  <dt>Live webhook</dt>
                  <dd>
                    <b className="tag pending">
                      NOT VERIFIED
                    </b>
                  </dd>
                </div>
              </dl>
            </article>

            <article className="service-card">
              <header>
                <div>
                  <h3>SMS OTP Provider</h3>
                  <p>HTTP OTP provider</p>
                </div>

                <b
                  className={
                    status.smsOtp.configured
                      ? 'tag implemented'
                      : 'tag disabled'
                  }
                >
                  {status.smsOtp.configured
                    ? 'CONFIGURED'
                    : 'NOT CONFIGURED'}
                </b>
              </header>

              <dl>
                <div>
                  <dt>Implementation</dt>
                  <dd>
                    {status.smsOtp.implementation}
                  </dd>
                </div>

                <div>
                  <dt>Config section</dt>
                  <dd>{status.smsOtp.sectionName}</dd>
                </div>

                <div>
                  <dt>Section present</dt>
                  <dd>
                    {yesNo(
                      status.smsOtp.sectionPresent,
                    )}
                  </dd>
                </div>

                <div>
                  <dt>Metadata</dt>
                  <dd>
                    {status.smsOtp.configuredEntries}/
                    {status.smsOtp.expectedEntries}
                  </dd>
                </div>

                <div>
                  <dt>Live delivery</dt>
                  <dd>
                    <b className="tag pending">
                      NOT VERIFIED
                    </b>
                  </dd>
                </div>
              </dl>
            </article>
          </div>

          <article className="production-integration-card">
            <header>
              <div>
                <h3>
                  Dữ liệu tích hợp production
                </h3>

                <p>
                  REAL READ ONLY từ
                  AnSinhSoRealDb
                </p>
              </div>

              <b className="tag real">
                REAL DATA
              </b>
            </header>

            <div className="production-counts">
              <div>
                <span>ZaloUsers</span>
                <strong>
                  {status.productionData.zaloUsers}
                </strong>
              </div>

              <div>
                <span>Notifications</span>
                <strong>
                  {status.productionData.notifications}
                </strong>
              </div>

              <div>
                <span>NotificationHistories</span>
                <strong>
                  {status.productionData.notificationHistories}
                </strong>
              </div>
            </div>

            <p className="service-note">
              Giá trị 0 là dữ liệu production thật.
              Không tự tạo dữ liệu Zalo hoặc lịch sử
              gửi giả để làm đẹp màn hình.
            </p>
          </article>
        </>
      )}
    </section>
  )
}

export default ServiceIntegrationPanel