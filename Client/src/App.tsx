import {routesAtom} from "./Atoms.tsx";
import {createBrowserRouter, RouterProvider} from "react-router";
import {useMemo} from "react";
import {useAtomValue} from "jotai";
function App() {

    const routes = useAtomValue(routesAtom);
    const router = useMemo(() => createBrowserRouter(routes), [routes]);
    return <RouterProvider router={router} />;
}

export default App
