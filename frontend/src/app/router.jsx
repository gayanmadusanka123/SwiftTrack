import { createBrowserRouter, Navigate} from 'react-router-dom'

import AuthPage from '../features/auth/pages/AuthPage'
import DashboardPage from '../features/dashboard/pages/DashboardPage'
import AdminDashboardPage from '../features/admin/pages/AdminDashboardPage'

const router = createBrowserRouter([
    {
        path: '/',
        element: <Navigate to="/login" replace />,
    },
    {
        path: '/login',
        element: <AuthPage />,
    },
    {
        path: '/signup',
        element: <AuthPage />,
    },
    {
        path: '/dashboard',
        element: <DashboardPage />
    },
    {
        path: '/admin/dashboard',
        element: <AdminDashboardPage />
    }
])

export default router