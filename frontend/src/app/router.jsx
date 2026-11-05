import { createBrowserRouter, Navigate} from 'react-router-dom'

import AuthPage from '../features/auth/pages/AuthPage'
import DashboardPage from '../features/dashboard/pages/DashboardPage'

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
    }
])

export default router