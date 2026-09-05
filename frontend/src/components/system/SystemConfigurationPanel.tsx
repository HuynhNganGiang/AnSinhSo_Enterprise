import {
  useEffect,
  useMemo,
  useState,
} from 'react'

import './SystemConfigurationPanel.css'


type RuntimeConfig = {
  source: 'REAL RUNTIME' | 'DEMO LOCAL'
  application: string
  api: string
  environment: string
  generatedAt: string | null

  database: {
    provider: string
    name: string
    isRealDatabase: boolean | null
    configured: boolean | null
  }

  demoData: {
    configured: boolean
    enabled: boolean | null
  }

  jwt: {
    sectionPresent: boolean
    configured: boolean
    signingMaterialConfigured: boolean
    configuredEntries: number
  }

  zaloOA: {
    sectionPresent: boolean
    configured: boolean
    appIdConfigured: boolean
    appSecretConfigured: boolean
    accessTokenConfigured: boolean
    refreshTokenConfigured: boolean
    configuredEntries: number
  }

  sms: {
    sectionPresent: boolean
    configured: boolean
    configuredEntries: number
  }

  allowedHostsConfigured: boolean

  security: {
    secretsReturned: boolean
    connectionStringReturned: boolean
    tokensReturned: boolean
    readOnlyEndpoint: boolean
  }
}


type RuntimeResponse = {
  success?: boolean
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


function isRuntimeConfig(
  value: unknown,
): value is RuntimeConfig {
  if (!isRecord(value)) {
    return false
  }

  return (
    (
      value.source === 'REAL RUNTIME' ||
      value.source === 'DEMO LOCAL'
    ) &&
    typeof value.application === 'string' &&
    typeof value.api === 'string' &&
    typeof value.environment === 'string' &&
    isRecord(value.database) &&
    isRecord(value.demoData) &&
    isRecord(value.jwt) &&
    isRecord(value.zaloOA) &&
    isRecord(value.sms) &&
    typeof value.allowedHostsConfigured === 'boolean' &&
    isRecord(value.security)
  )
}


function extractRuntimeConfig(
  payload: unknown,
): RuntimeConfig | null {
  let current: unknown = payload

  for (let depth = 0; depth < 4; depth += 1) {
    if (isRuntimeConfig(current)) {
      return current
    }

    if (!isRecord(current)) {
      return null
    }

    if (!('data' in current)) {
      return null
    }

    current = current.data
  }

  return isRuntimeConfig(current)
    ? current
    : null
}


type LocalConfig = {
  pageSize: 10 | 20 | 50
  autoRefresh: boolean
  refreshSeconds: 30 | 60 | 120
  compactMode: boolean
  showSourceLabels: boolean
  presentationFallback: boolean
}


const STORAGE_KEY =
  'ansinhso.local.system-config.v1'


const DEFAULT_LOCAL:
  LocalConfig = {
    pageSize: 20,
    autoRefresh: true,
    refreshSeconds: 60,
    compactMode: false,
    showSourceLabels: true,
    presentationFallback: true,
  }


const T = {
  eyebrow:
    'H\u1EC6 TH\u1ED0NG 04',

  title:
    'C\u1EA5u h\u00ECnh h\u1EC7 th\u1ED1ng',

  subtitle:
    'Theo d\u00F5i c\u1EA5u h\u00ECnh runtime th\u1EADt v\u00E0 qu\u1EA3n l\u00FD c\u00E1c t\u00F9y ch\u1ECDn giao di\u1EC7n an to\u00E0n.',

  safe:
    'API ch\u1EC9 hi\u1EC3n th\u1ECB metadata an to\u00E0n. Connection string, JWT secret, AppSecret, token, m\u1EADt kh\u1EA9u v\u00E0 OTP kh\u00F4ng bao gi\u1EDD \u0111\u01B0\u1EE3c tr\u1EA3 v\u1EC1 giao di\u1EC7n.',

  fallback:
    'Kh\u00F4ng \u0111\u1ECDc \u0111\u01B0\u1EE3c runtime API. Giao di\u1EC7n chuy\u1EC3n sang DEMO LOCAL v\u00E0 kh\u00F4ng gi\u1EA3 \u0111\u1ECBnh c\u00E1c gi\u00E1 tr\u1ECB b\u1EA3o m\u1EADt.',

  runtime:
    'Tr\u1EA1ng th\u00E1i runtime',

  app:
    '\u1EE8ng d\u1EE5ng',

  environment:
    'M\u00F4i tr\u01B0\u1EDDng',

  database:
    'C\u01A1 s\u1EDF d\u1EEF li\u1EC7u',

  security:
    'B\u1EA3o m\u1EADt & JWT',

  integrations:
    'T\u00EDch h\u1EE3p d\u1ECBch v\u1EE5',

  flags:
    'C\u1EDD v\u1EADn h\u00E0nh',

  configured:
    '\u0110\u00E3 c\u1EA5u h\u00ECnh',

  notConfigured:
    'Ch\u01B0a c\u1EA5u h\u00ECnh',

  active:
    '\u0110ang ho\u1EA1t \u0111\u1ED9ng',

  inactive:
    'Ch\u01B0a ho\u1EA1t \u0111\u1ED9ng',

  unknown:
    'Ch\u01B0a x\u00E1c \u0111\u1ECBnh',

  refresh:
    'L\u00E0m m\u1EDBi runtime',

  localTitle:
    'Thi\u1EBFt l\u1EADp giao di\u1EC7n c\u1EE5c b\u1ED9',

  localDesc:
    'C\u00E1c t\u00F9y ch\u1ECDn n\u00E0y ch\u1EC9 l\u01B0u trong tr\u00ECnh duy\u1EC7t, kh\u00F4ng thay \u0111\u1ED5i SQL production hay appsettings.',

  pageSize:
    'S\u1ED1 b\u1EA3n ghi m\u1ED7i trang',

  autoRefresh:
    'T\u1EF1 \u0111\u1ED9ng l\u00E0m m\u1EDBi',

  interval:
    'Chu k\u1EF3 l\u00E0m m\u1EDBi',

  compact:
    'Ch\u1EBF \u0111\u1ED9 giao di\u1EC7n thu g\u1ECDn',

  sourceLabels:
    'Hi\u1EC7n nh\u00E3n REAL / DEMO',

  fallbackSetting:
    'Cho ph\u00E9p fallback tr\u00ECnh di\u1EC5n',

  save:
    'L\u01B0u c\u1EE5c b\u1ED9',

  reset:
    'Kh\u00F4i ph\u1EE5c m\u1EB7c \u0111\u1ECBnh',

  saved:
    '\u0110\u00E3 l\u01B0u c\u1EA5u h\u00ECnh c\u1EE5c b\u1ED9.',

  policy:
    'Nguy\u00EAn t\u1EAFc ngu\u1ED3n d\u1EEF li\u1EC7u',

  policyText:
    'Production lu\u00F4n \u0111\u01B0\u1EE3c \u01B0u ti\u00EAn. Khi module thi\u1EBFu d\u1EEF li\u1EC7u, fallback tr\u00ECnh di\u1EC5n ph\u1EA3i mang nh\u00E3n DEMO LOCAL v\u00E0 kh\u00F4ng \u0111\u01B0\u1EE3c tr\u1ED9n v\u00E0o KPI production.',

  noSecrets:
    'KH\u00D4NG HI\u1EC2N TH\u1ECA SECRET',

  loading:
    '\u0110ang \u0111\u1ECDc c\u1EA5u h\u00ECnh runtime...',

  seconds:
    'gi\u00E2y',
}


function readLocalConfig():
  LocalConfig {

  try {

    const raw =
      localStorage.getItem(
        STORAGE_KEY,
      )

    if (raw) {

      return {
        ...DEFAULT_LOCAL,
        ...JSON.parse(raw),
      }
    }

  } catch {
    // Use safe defaults.
  }

  return DEFAULT_LOCAL
}


function fallbackRuntime():
  RuntimeConfig {

  return {
    source:
      'DEMO LOCAL',

    application:
      'AnSinhSo Enterprise',

    api:
      '.NET 8 / ASP.NET Core Web API',

    environment:
      'Unavailable',

    generatedAt:
      null,

    database: {
      provider:
        'Unavailable',

      name:
        'Unavailable',

      isRealDatabase:
        null,

      configured:
        null,
    },

    demoData: {
      configured:
        false,

      enabled:
        null,
    },

    jwt: {
      sectionPresent:
        false,

      configured:
        false,

      signingMaterialConfigured:
        false,

      configuredEntries:
        0,
    },

    zaloOA: {
      sectionPresent:
        false,

      configured:
        false,

      appIdConfigured:
        false,

      appSecretConfigured:
        false,

      accessTokenConfigured:
        false,

      refreshTokenConfigured:
        false,

      configuredEntries:
        0,
    },

    sms: {
      sectionPresent:
        false,

      configured:
        false,

      configuredEntries:
        0,
    },

    allowedHostsConfigured:
      false,

    security: {
      secretsReturned:
        false,

      connectionStringReturned:
        false,

      tokensReturned:
        false,

      readOnlyEndpoint:
        true,
    },
  }
}


function displayBoolean(
  value:
    boolean | null,
) {

  if (value === null) {
    return T.unknown
  }

  return value
    ? T.configured
    : T.notConfigured
}


function statusClass(
  value:
    boolean | null,
) {

  if (value === null) {
    return 'unknown'
  }

  return value
    ? 'good'
    : 'warn'
}


function formatDate(
  value:
    string | null,
) {

  if (!value) {
    return T.unknown
  }

  const date =
    new Date(value)

  if (
    Number.isNaN(
      date.getTime(),
    )
  ) {
    return T.unknown
  }

  return date.toLocaleString(
    'vi-VN',
  )
}


function StatusLine({
  label,
  value,
  note,
}: {
  label: string
  value: boolean | null
  note?: string
}) {

  return (
    <div className="system-config-status-line">

      <div>
        <strong>
          {label}
        </strong>

        {note && (
          <small>
            {note}
          </small>
        )}
      </div>


      <span
        className={
          `system-config-status ${statusClass(value)}`
        }
      >
        {displayBoolean(value)}
      </span>

    </div>
  )
}


function SystemConfigurationPanel() {

  const [
    runtime,
    setRuntime,
  ] =
    useState<RuntimeConfig>(
      fallbackRuntime(),
    )


  const [
    loading,
    setLoading,
  ] =
    useState(true)


  const [
    localConfig,
    setLocalConfig,
  ] =
    useState<LocalConfig>(
      readLocalConfig(),
    )


  const [
    saved,
    setSaved,
  ] =
    useState('')


  const loadRuntime =
    async () => {

      setLoading(true)

      try {

        const token =
          localStorage.getItem(
            'accessToken',
          )


        const response =
          await fetch(
            '/api/v1/system/configuration-status',
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
          ) as RuntimeResponse


        const runtimeData =
          extractRuntimeConfig(
            body,
          )


        if (!runtimeData) {

          throw new Error(
            'Runtime response shape is invalid.',
          )
        }


        setRuntime(
          runtimeData,
        )

      } catch {

        setRuntime(
          fallbackRuntime(),
        )
      }
      finally {

        setLoading(false)
      }
    }


  useEffect(
    () => {
      void loadRuntime()
    },
    [],
  )


  const isReal =
    runtime.source ===
    'REAL RUNTIME'


  const runtimeChecks =
    useMemo(
      () => {

        const checks =
          [
            runtime.database
              .isRealDatabase,

            runtime.database
              .configured,

            runtime.jwt
              .configured,

            runtime.allowedHostsConfigured,
          ]


        return checks.filter(
          value =>
            value === true,
        ).length
      },
      [
        runtime,
      ],
    )


  const saveLocal =
    () => {

      localStorage.setItem(
        STORAGE_KEY,
        JSON.stringify(
          localConfig,
        ),
      )

      setSaved(
        T.saved,
      )

      window.setTimeout(
        () => {
          setSaved('')
        },
        2400,
      )
    }


  const resetLocal =
    () => {

      localStorage.removeItem(
        STORAGE_KEY,
      )

      setLocalConfig(
        DEFAULT_LOCAL,
      )

      setSaved(
        T.saved,
      )
    }


  return (
    <section className="system-config-panel">

      <div className="system-config-heading">

        <div>

          <div className="system-config-eyebrow">
            {T.eyebrow}
          </div>

          <h2>
            {T.title}
          </h2>

          <p>
            {T.subtitle}
          </p>

        </div>


        <div className="system-config-heading-actions">

          <span
            className={
              `system-config-source ${
                isReal
                  ? 'real'
                  : 'demo'
              }`
            }
          >
            {runtime.source}
          </span>


          <button
            type="button"
            className="system-config-button secondary"
            onClick={() =>
              void loadRuntime()
            }
          >
            ↻ {T.refresh}
          </button>

        </div>

      </div>


      <div
        className={
          `system-config-notice ${
            isReal
              ? 'real'
              : 'demo'
          }`
        }
      >

        <strong>
          {isReal
            ? T.noSecrets
            : 'DEMO LOCAL'}
        </strong>

        <span>
          {isReal
            ? T.safe
            : T.fallback}
        </span>

      </div>


      {loading ? (

        <div className="system-config-loading">
          {T.loading}
        </div>

      ) : (

        <>

          <div className="system-config-summary">

            <article>
              <span>
                {T.app}
              </span>

              <strong>
                {runtime.application}
              </strong>

              <small>
                {runtime.api}
              </small>
            </article>


            <article>
              <span>
                {T.environment}
              </span>

              <strong>
                {runtime.environment}
              </strong>

              <small>
                {formatDate(
                  runtime.generatedAt,
                )}
              </small>
            </article>


            <article>
              <span>
                {T.database}
              </span>

              <strong>
                {runtime.database.name}
              </strong>

              <small>
                {runtime.database.provider}
              </small>
            </article>


            <article>
              <span>
                Runtime checks
              </span>

              <strong>
                {runtimeChecks}/4
              </strong>

              <small>
                READ ONLY
              </small>
            </article>

          </div>


          <div className="system-config-card-grid">

            <article className="system-config-card">

              <div className="system-config-card-title">
                <span className="blue">
                  DB
                </span>

                <div>
                  <h3>
                    {T.database}
                  </h3>

                  <p>
                    Production runtime
                  </p>
                </div>
              </div>


              <StatusLine
                label="AnSinhSoRealDb"
                value={
                  runtime.database
                    .isRealDatabase
                }
                note={
                  runtime.database.name
                }
              />


              <StatusLine
                label="ConnectionStrings"
                value={
                  runtime.database
                    .configured
                }
                note={
                  'Gi\u00E1 tr\u1ECB k\u1EBFt n\u1ED1i \u0111\u01B0\u1EE3c che ho\u00E0n to\u00E0n.'
                }
              />

            </article>


            <article className="system-config-card">

              <div className="system-config-card-title">
                <span className="green">
                  JWT
                </span>

                <div>
                  <h3>
                    {T.security}
                  </h3>

                  <p>
                    Authentication metadata
                  </p>
                </div>
              </div>


              <StatusLine
                label="JWT"
                value={
                  runtime.jwt
                    .configured
                }
                note={
                  `${runtime.jwt.configuredEntries} configured entries`
                }
              />


              <StatusLine
                label="Signing material"
                value={
                  runtime.jwt
                    .signingMaterialConfigured
                }
                note={
                  T.noSecrets
                }
              />


              <StatusLine
                label="AllowedHosts"
                value={
                  runtime
                    .allowedHostsConfigured
                }
              />

            </article>


            <article className="system-config-card">

              <div className="system-config-card-title">
                <span className="violet">
                  ZA
                </span>

                <div>
                  <h3>
                    {T.integrations}
                  </h3>

                  <p>
                    Zalo OA / SMS
                  </p>
                </div>
              </div>


              <StatusLine
                label="Zalo OA"
                value={
                  runtime.zaloOA
                    .configured
                }
                note={
                  `${runtime.zaloOA.configuredEntries} configured entries`
                }
              />


              <StatusLine
                label="Zalo App ID"
                value={
                  runtime.zaloOA
                    .appIdConfigured
                }
              />


              <StatusLine
                label="Zalo AppSecret"
                value={
                  runtime.zaloOA
                    .appSecretConfigured
                }
                note={
                  T.noSecrets
                }
              />


              <StatusLine
                label="Access Token"
                value={
                  runtime.zaloOA
                    .accessTokenConfigured
                }
                note={
                  T.noSecrets
                }
              />


              <StatusLine
                label="Refresh Token"
                value={
                  runtime.zaloOA
                    .refreshTokenConfigured
                }
                note={
                  T.noSecrets
                }
              />


              <StatusLine
                label="SMS"
                value={
                  runtime.sms
                    .configured
                }
              />

            </article>


            <article className="system-config-card">

              <div className="system-config-card-title">
                <span className="orange">
                  CFG
                </span>

                <div>
                  <h3>
                    {T.flags}
                  </h3>

                  <p>
                    Safe runtime flags
                  </p>
                </div>
              </div>


              <StatusLine
                label="DemoData"
                value={
                  runtime.demoData
                    .configured
                }
                note={
                  runtime.demoData.enabled ===
                  null
                    ? T.unknown
                    : runtime.demoData.enabled
                      ? '\u0110ang b\u1EADt'
                      : '\u0110ang t\u1EAFt'
                }
              />


              <StatusLine
                label="Read-only config endpoint"
                value={
                  runtime.security
                    .readOnlyEndpoint
                }
              />


              <StatusLine
                label="Secrets returned"
                value={
                  !runtime.security
                    .secretsReturned
                }
                note={
                  'NO'
                }
              />


              <StatusLine
                label="Tokens returned"
                value={
                  !runtime.security
                    .tokensReturned
                }
                note={
                  'NO'
                }
              />

            </article>

          </div>

        </>
      )}


      <div className="system-config-local">

        <div className="system-config-local-heading">

          <div>
            <h3>
              {T.localTitle}
            </h3>

            <p>
              {T.localDesc}
            </p>
          </div>

          <span className="system-config-source local">
            LOCAL ONLY
          </span>

        </div>


        <div className="system-config-form-grid">

          <label>
            <span>
              {T.pageSize}
            </span>

            <select
              value={
                localConfig.pageSize
              }
              onChange={event =>
                setLocalConfig(
                  current => ({
                    ...current,

                    pageSize:
                      Number(
                        event.target.value,
                      ) as 10 | 20 | 50,
                  }),
                )
              }
            >
              <option value={10}>
                10
              </option>

              <option value={20}>
                20
              </option>

              <option value={50}>
                50
              </option>
            </select>
          </label>


          <label>
            <span>
              {T.interval}
            </span>

            <select
              disabled={
                !localConfig
                  .autoRefresh
              }
              value={
                localConfig
                  .refreshSeconds
              }
              onChange={event =>
                setLocalConfig(
                  current => ({
                    ...current,

                    refreshSeconds:
                      Number(
                        event.target.value,
                      ) as 30 | 60 | 120,
                  }),
                )
              }
            >
              <option value={30}>
                30 {T.seconds}
              </option>

              <option value={60}>
                60 {T.seconds}
              </option>

              <option value={120}>
                120 {T.seconds}
              </option>
            </select>
          </label>

        </div>


        <div className="system-config-switch-grid">

          {[
            {
              key:
                'autoRefresh' as const,

              label:
                T.autoRefresh,

              value:
                localConfig
                  .autoRefresh,
            },

            {
              key:
                'compactMode' as const,

              label:
                T.compact,

              value:
                localConfig
                  .compactMode,
            },

            {
              key:
                'showSourceLabels' as const,

              label:
                T.sourceLabels,

              value:
                localConfig
                  .showSourceLabels,
            },

            {
              key:
                'presentationFallback' as const,

              label:
                T.fallbackSetting,

              value:
                localConfig
                  .presentationFallback,
            },
          ].map(
            item => (

              <button
                type="button"
                key={item.key}
                className={
                  `system-config-switch ${
                    item.value
                      ? 'on'
                      : 'off'
                  }`
                }
                onClick={() =>
                  setLocalConfig(
                    current => ({
                      ...current,

                      [item.key]:
                        !current[
                          item.key
                        ],
                    }),
                  )
                }
              >
                <span>
                  {item.label}
                </span>

                <strong>
                  {item.value
                    ? 'ON'
                    : 'OFF'}
                </strong>
              </button>
            ),
          )}

        </div>


        <div className="system-config-local-actions">

          <div>
            {saved}
          </div>


          <button
            type="button"
            className="system-config-button secondary"
            onClick={resetLocal}
          >
            {T.reset}
          </button>


          <button
            type="button"
            className="system-config-button primary"
            onClick={saveLocal}
          >
            {T.save}
          </button>

        </div>

      </div>


      <div className="system-config-policy">

        <strong>
          {T.policy}
        </strong>

        <p>
          {T.policyText}
        </p>

      </div>

    </section>
  )
}


export default SystemConfigurationPanel