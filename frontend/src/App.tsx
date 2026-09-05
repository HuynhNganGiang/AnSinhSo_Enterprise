import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from 'react-router-dom'
import ProtectedRoute from './components/ProtectedRoute'
import { hasValidAccessToken } from './utils/authSession'
import DashboardPage from './pages/DashboardPage'
import ReportsPage from './pages/ReportsPage'
import CitizensPage from './pages/CitizensPage'
import HouseholdsPage from './pages/HouseholdsPage'
import WelfareCasesPage from './pages/WelfareCasesPage'
import PaymentsPage from './pages/PaymentsPage'
import MapPage from './pages/MapPage'
import AiPage from './pages/AiPage'
import NotificationsPage from './pages/NotificationsPage'
import CategoriesPage from './pages/CategoriesPage'
import SystemPage from './pages/SystemPage'
import LoginPage from './pages/LoginPage'

function App() {
  const isAuthenticated = hasValidAccessToken()

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
          path="/welfare"
          element={
            <ProtectedRoute>
              <WelfareCasesPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/payments"
          element={
            <ProtectedRoute>
              <PaymentsPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/map"
          element={
            <ProtectedRoute>
              <MapPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/ai"
          element={
            <ProtectedRoute>
              <AiPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/notifications"
          element={
            <ProtectedRoute>
              <NotificationsPage />
            </ProtectedRoute>
          }
        />


        <Route
          path="/categories"
          element={
            <ProtectedRoute>
              <CategoriesPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/system"
          element={
            <ProtectedRoute>
              <SystemPage />
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

        <Route
          path="/reports"
          element={
            <ProtectedRoute>
              <ReportsPage />
            </ProtectedRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  )
}

export default App
