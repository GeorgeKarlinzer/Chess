import Home from './components/Home'
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import Register from './components/authorization/Register'
import Login from './components/authorization/Login'

const AppRoutes = [
    {
        index: true,
        element: <Home/>
    },
    {
        path: `${ApplicationPaths.Register}`,
        element: <Register/>
    },
    {
        path: `${ApplicationPaths.Login}`,
        element: <Login/>
    }
];

export default AppRoutes;
