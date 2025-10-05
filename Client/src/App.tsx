import {routesAtom} from "./Atoms.tsx";
import {createBrowserRouter, RouterProvider} from "react-router";
import {useMemo} from "react";
import {useAtomValue} from "jotai";
import {ToastContainer} from "react-toastify";
function App() {

    const routes = useAtomValue(routesAtom);
    const router = useMemo(() => createBrowserRouter(routes), [routes]);
    return <><RouterProvider router={router} /> <ToastContainer /></>;
}

export default App
