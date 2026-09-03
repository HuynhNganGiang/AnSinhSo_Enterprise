import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from 'react-router-dom'
import ProtectedRoute from './components/ProtectedRoute'
import DashboardPage from './pages/DashboardPage'
import CitizensPage from './pages/CitizensPage'
import HouseholdsPage from './pages/HouseholdsPage'
import LoginPage from './pages/LoginPage'

function App() {
  const isAuthenticated = Boolean(localStorage.getItem('accessToken'))

  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/login"
          element={
            isAuthenticated ? (
              <Navigate to="/dashboard" replace />
            ) : (
              <LoginPage />
            )
          }
        />

        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <DashboardPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/citizens"
          element={
            <ProtectedRoute>
              <CitizensPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/households"
          element={
            <ProtectedRoute>
              <HouseholdsPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="*"
          element={
            <Navigate
              to={isAuthenticated ? '/dashboard' : '/login'}
              replace
            />
          }
        />
      </Routes>
    </BrowserRouter>
  )
}

export default App