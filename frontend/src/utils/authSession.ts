const AUTH_STORAGE_KEYS = [
  'accessToken',
  'refreshToken',
  'userId',
  'expiresInSeconds',
] as const

type JwtPayload = {
  exp?: number
}

export function clearAuthSession() {
  for (const key of AUTH_STORAGE_KEYS) {
    localStorage.removeItem(key)
  }
}

function decodeJwtPayload(
  token: string,
): JwtPayload | null {
  try {
    const parts =
      token.split('.')

    if (parts.length !== 3) {
      return null
    }

    const base64Url =
      parts[1]

    const base64 =
      base64Url
        .replace(/-/g, '+')
        .replace(/_/g, '/')

    const padded =
      base64.padEnd(
        Math.ceil(base64.length / 4) * 4,
        '=',
      )

    const binary =
      atob(padded)

    const bytes =
      Uint8Array.from(
        binary,
        character =>
          character.charCodeAt(0),
      )

    const json =
      new TextDecoder()
        .decode(bytes)

    return JSON.parse(
      json,
    ) as JwtPayload

  } catch {
    return null
  }
}

export function isAccessTokenExpired(
  token: string,
) {
  const payload =
    decodeJwtPayload(token)

  if (
    !payload ||
    typeof payload.exp !== 'number'
  ) {
    return true
  }

  return (
    Date.now() >=
    payload.exp * 1000
  )
}

export function hasValidAccessToken() {
  const token =
    localStorage.getItem(
      'accessToken',
    )

  if (!token) {
    return false
  }

  if (
    isAccessTokenExpired(
      token,
    )
  ) {
    clearAuthSession()
    return false
  }

  return true
}