import type {RouteObject} from "react-router";
import {atom} from "jotai";
import Sidebar from "./Sidebar.tsx";
import Create from "./Create.tsx";
import Authors from "./Authors.tsx";
import Books from "./Books.tsx";
import Genres from "./Genres.tsx";
import Home from "./Home.tsx";
import EditAuthor from "./EditAuthor.tsx";
import EditBook from "./EditBook.tsx";
import EditGenre from "./EditGenre.tsx";

export const routesAtom = atom<RouteObject[]>([
    {
        path: '/',
        element: <Sidebar />,
        children: [
            {
                index: true,
                element: <Home />
            },
            {
                path: 'create',
                element: <Create />
            },
            {
                path: 'authors',
                element: <Authors />
            },
            {
                path: 'books',
                element: <Books />
            },
            {
                path: 'genres',
                element: <Genres />
            },
            {
                path: 'authors/:AuthorId/edit',
                element: <EditAuthor />
            },
            {
                path: 'books/:BookId/edit',
                element: <EditBook />
            },
            {
                path: 'genres/:GenreId/edit',
                element: <EditGenre />
            }

        ]
    }
])