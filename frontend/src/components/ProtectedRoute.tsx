import type { ReactNode } from 'react'
import { Navigate } from 'react-router-dom'
import { hasValidAccessToken } from '../utils/authSession'

type ProtectedRouteProps = {
  children: ReactNode
}

function ProtectedRoute({ children }: ProtectedRouteProps) {
  const isAuthenticated =
    hasValidAccessToken()

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />
  }

  return children
}

export default ProtectedRoute