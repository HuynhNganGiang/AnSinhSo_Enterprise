import { useState } from 'react'
import type { FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import '../App.css'

const API_BASE_URL = ''

interface AuthenticationResult {
  accessToken: string
  refreshToken: string
  expiresInSeconds: number
  userId: string
}

interface ApiResponse<T> {
  data: T
  success: boolean
  message: string
  errors: unknown[]
}

interface ApiError {
  message?: string
  title?: string
  detail?: string
}

function LoginPage() {
  const navigate = useNavigate()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState(false)

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    if (!username.trim() || !password) {
      setError('Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.')
      return
    }

    setIsLoading(true)
    setError('')
    setSuccess(false)

    try {
      const response = await fetch(
        `${API_BASE_URL}/api/v1/auth/backoffice/login`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            username: username.trim(),
            password,
            deviceName: 'AnSinhSo Web',
          }),
        },
      )

      if (!response.ok) {
        let message = 'Đăng nhập không thành công.'

        try {
          const errorBody = (await response.json()) as ApiError
          message =
            errorBody.message ??
            errorBody.detail ??
            errorBody.title ??
            message
        } catch {
          // Response không có JSON hợp lệ.
        }

        if (response.status === 401) {
          message = 'Tên đăng nhập hoặc mật khẩu không đúng.'
        } else if (response.status === 403) {
          message = 'Tài khoản hiện không được phép đăng nhập.'
        } else if (response.status === 429) {
          message = 'Bạn đăng nhập quá nhiều lần. Vui lòng thử lại sau.'
        }

        throw new Error(message)
      }

      const responseBody =
        (await response.json()) as ApiResponse<AuthenticationResult>

      const result = responseBody.data

      if (
        !responseBody.success ||
        !result?.accessToken ||
        !result.refreshToken ||
        !result.userId
      ) {
        throw new Error(
          responseBody.message || 'Phản hồi đăng nhập từ hệ thống không hợp lệ.',
        )
      }

      localStorage.setItem('accessToken', result.accessToken)
      localStorage.setItem('refreshToken', result.refreshToken)
      localStorage.setItem('userId', result.userId)
      localStorage.setItem(
        'expiresInSeconds',
        result.expiresInSeconds.toString(),
      )

      setSuccess(true)
      setPassword('')
     navigate('/dashboard', { replace: true })
    } catch (err) {
      if (err instanceof TypeError) {
        setError(
          'Không thể kết nối đến máy chủ. Vui lòng kiểm tra API đang hoạt động.',
        )
      } else {
        setError(
          err instanceof Error
            ? err.message
            : 'Đã xảy ra lỗi trong quá trình đăng nhập.',
        )
      }
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <main className="login-page">
      <section className="brand-panel">
        <div className="brand-content">
          <div className="brand-mark" aria-hidden="true">
            AS
          </div>

          <p className="eyebrow">NỀN TẢNG QUẢN LÝ AN SINH XÃ HỘI</p>

          <h1>An Sinh Số</h1>

          <p className="brand-description">
            Hệ thống hỗ trợ quản lý, theo dõi và điều phối công tác an sinh
            xã hội tại xã Sông Lũy.
          </p>

          <div className="brand-features">
            <span>Quản lý tập trung</span>
            <span>Bản đồ số</span>
            <span>Hỗ trợ ra quyết định</span>
          </div>
        </div>

        <p className="brand-footer">
          Hệ thống thông tin phục vụ cán bộ và lãnh đạo địa phương
        </p>
      </section>

      <section className="login-panel">
        <div className="login-card">
          <div className="mobile-brand">
            <div className="brand-mark small" aria-hidden="true">
              AS
            </div>
            <span>An Sinh Số</span>
          </div>

          <div className="login-heading">
            <p className="eyebrow blue">CỔNG QUẢN TRỊ</p>
            <h2>Đăng nhập hệ thống</h2>
            <p>
              Sử dụng tài khoản được cấp để truy cập hệ thống quản lý.
            </p>
          </div>

          <form onSubmit={handleSubmit} className="login-form">
            <div className="form-field">
              <label htmlFor="username">Tên đăng nhập</label>
              <input
                id="username"
                name="username"
                type="text"
                autoComplete="username"
                value={username}
                onChange={(event) => setUsername(event.target.value)}
                placeholder="Nhập tên đăng nhập"
                disabled={isLoading}
              />
            </div>

            <div className="form-field">
              <label htmlFor="password">Mật khẩu</label>
              <input
                id="password"
                name="password"
                type="password"
                autoComplete="current-password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="Nhập mật khẩu"
                disabled={isLoading}
              />
            </div>

            {error && (
              <div className="message error-message" role="alert">
                {error}
              </div>
            )}

            {success && (
              <div className="message success-message" role="status">
                Đăng nhập thành công.
              </div>
            )}

            <button
              type="submit"
              className="login-button"
              disabled={isLoading}
            >
              {isLoading ? 'Đang đăng nhập...' : 'Đăng nhập'}
            </button>
          </form>

          <p className="support-text">
            Nếu không thể đăng nhập, vui lòng liên hệ quản trị viên hệ thống.
          </p>
        </div>
      </section>
    </main>
  )
}

export default LoginPage