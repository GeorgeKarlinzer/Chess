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
        path: `${ApplicationPaths.register}`,
        element: <Register/>
    },
    {
        path: `${ApplicationPaths.login}`,
        element: <Login/>
    },
    {
        path: `~/${ApplicationPaths.logout}`,
        element: <Logout/>
    }
];

export default AppRoutes;
