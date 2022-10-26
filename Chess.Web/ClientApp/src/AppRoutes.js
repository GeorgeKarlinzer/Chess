import ApplicationPaths from './ApplicationPaths'
import Login from './components/account/Login'
import Logout from './components/account/Logout'
import Register from './components/account/Register'
import Home from './components/Home'

const AppRoutes = [
    {
        index: true,
        element: <Home/>
    },
    {
        path: `/${ApplicationPaths.registerRoute}`,
        element: <Register/>
    },
    {
        path: `/${ApplicationPaths.loginRoute}`,
        element: <Login/>
    },
    {
        path: `/${ApplicationPaths.logoutRoute}`,
        element: <Logout/>
    }
];

export default AppRoutes;
