import './ListPage.css'
import type {BookDto} from "./generated-ts-client.ts";
import {useEffect, useState} from "react";
import {bookClient} from "./baseUrl.ts";
import {useNavigate} from "react-router";

type SortField = 'title' | 'createdat' | 'pages' | 'genre' | 'authors';
type SortDirection = 'asc' | 'desc';

function Books() {
    const [books, setBooks] = useState<BookDto[]>([])
    const [sortField, setSortField] = useState<SortField>('title')
    const [sortDirection, setSortDirection] = useState<SortDirection>('asc')
    const navigate = useNavigate();

    useEffect( () => {
        bookClient.getAllBooks().then(res => {
            setBooks(res)
        })
    }, [])

    const handleSort = (field: SortField) => {
        if (sortField === field) {
            setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc')
        } else {
            setSortField(field)
            setSortDirection('asc')
        }
    }

    const sortedBooks = [...books].sort((a, b) => {
        let compareValue = 0
        
        switch (sortField) {
            case 'title':
                compareValue = (a.title || '').localeCompare(b.title || '')
                break
            case 'createdat':
                compareValue = (a.createdat || '').localeCompare(b.createdat || '')
                break
            case 'pages':
                compareValue = (a.pages || 0) - (b.pages || 0)
                break
            case 'genre':
                compareValue = (a.genre || '').localeCompare(b.genre || '')
                break
            case 'authors':
                compareValue = (a.authorIDs?.length || 0) - (b.authorIDs?.length || 0)
                break
        }
        
        return sortDirection === 'asc' ? compareValue : -compareValue
    })

    const SortIcon = ({ field }: { field: SortField }) => {
        if (sortField !== field) return <span className="opacity-30">⇅</span>
        return sortDirection === 'asc' ? <span>↑</span> : <span>↓</span>
    }
    
    return (
        <div className="overflow-x-auto">
            <table className="table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('title')}
                        >
                            Title <SortIcon field="title" />
                        </th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('pages')}
                        >
                            Pages <SortIcon field="pages" />
                        </th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('genre')}
                        >
                            Genre <SortIcon field="genre" />
                        </th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('authors')}
                        >
                            Authors <SortIcon field="authors" />
                        </th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('createdat')}
                        >
                            Created At <SortIcon field="createdat" />
                        </th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {sortedBooks.map((book, index) => (
                        <tr key={book.id} className="hover:bg-base-300">
                            <th>{index + 1}</th>
                            <td>{book.title}</td>
                            <td>{book.pages}</td>
                            <td>
                                {book.genre ? (
                                    <span className="badge badge-sm badge-primary">
                                        {book.genre}
                                    </span>
                                ) : (
                                    <span className="text-gray-400">No genre</span>
                                )}
                            </td>
                            <td>
                                {book.authorIDs && book.authorIDs.length > 0 ? (
                                    <div className="flex flex-wrap gap-1">
                                        {book.authorIDs.map(author => (
                                            <span key={author.id} className="badge badge-sm">
                                                {author.name}
                                            </span>
                                        ))}
                                    </div>
                                ) : (
                                    <span className="text-gray-400">No authors</span>
                                )}
                            </td>
                            <td>
                                {book.createdat
                                    ? new Date(book.createdat).toLocaleDateString()
                                    : 'N/A'
                                }
                            </td>
                            <td>
                                <div className="flex gap-2">
                                    <button className="btn btn-outline btn-warning hover:!bg-warning" onClick={() => navigate(`${book.id}/edit`)}>Edit</button>
                                <button className="btn btn-outline btn-error hover:!bg-error">Delete</button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

export default Books