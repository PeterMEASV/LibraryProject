import './ListPage.css'
import type {Author} from "./generated-ts-client.ts";
import {useEffect, useState} from "react";
import {authorClient} from "./baseUrl.ts";
import {useNavigate} from "react-router";
import {toast} from "react-toastify";

type SortField = 'name' | 'createdat' | 'books';
type SortDirection = 'asc' | 'desc';

function Authors() {
    const [authors, setAuthors] = useState<Author[]>([])
    const [sortField, setSortField] = useState<SortField>('name')
    const [sortDirection, setSortDirection] = useState<SortDirection>('asc')
    const navigate = useNavigate();
    const [authorToDelete, setAuthorToDelete] = useState<string | undefined>(undefined)
    const [showDeleteModal, setShowDeleteModal] = useState(false)

    useEffect( () => {
        authorClient.getAllAuthors().then(res => {
            setAuthors(res)
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

    const sortedAuthors = [...authors].sort((a, b) => {
        let compareValue = 0
        
        switch (sortField) {
            case 'name':
                compareValue = (a.name || '').localeCompare(b.name || '')
                break
            case 'createdat':
                compareValue = (a.createdat || '').localeCompare(b.createdat || '')
                break
            case 'books':
                compareValue = (a.books?.length || 0) - (b.books?.length || 0)
                break
        }
        
        return sortDirection === 'asc' ? compareValue : -compareValue
    })

    const SortIcon = ({ field }: { field: SortField }) => {
        if (sortField !== field) return <span className="opacity-30">⇅</span>
        return sortDirection === 'asc' ? <span>↑</span> : <span>↓</span>
    }

    const handleDelete = (id: string | undefined) => {
        setAuthorToDelete(id)
        setShowDeleteModal(true)
    }

    const handleDeleteConfirm = () => {
        if (!authorToDelete) return;

        authorClient.deleteAuthor({ id: authorToDelete }).then(() => {
            toast.success("Author deleted successfully!");
            setAuthors(authors.filter(author => author.id !== authorToDelete));
            setShowDeleteModal(false);
            setAuthorToDelete(undefined);
        }).catch(err => {
            console.error(err);
            toast.error("Failed to delete author.");
        })

    }

    const handleDeleteCancel = () => {
        setShowDeleteModal(false)
        setAuthorToDelete(undefined)
    }
    
    return (
        <div className="overflow-x-auto">
            <table className="table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('name')}
                        >
                            Name <SortIcon field="name" />
                        </th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('createdat')}
                        >
                            Created At <SortIcon field="createdat" />
                        </th>
                        <th 
                            className="cursor-pointer hover:bg-base-200"
                            onClick={() => handleSort('books')}
                        >
                            Books <SortIcon field="books" />
                        </th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {sortedAuthors.map((author, index) => (
                        <tr key={author.id} className="hover:bg-base-300">
                            <th>{index + 1}</th>
                            <td>{author.name}</td>
                            <td>
                                {author.createdat
                                    ? new Date(author.createdat).toLocaleDateString()
                                    : 'N/A'
                                }
                            </td>
                            <td>
                                {author.books && author.books.length > 0 ? (
                                    <div className="flex flex-wrap gap-1">
                                        {author.books.map(book => (
                                            <span key={book.id} className="badge badge-sm">
                                                {book.title}
                                            </span>
                                        ))}
                                    </div>
                                ) : (
                                    <span className="text-gray-400">No books</span>
                                )}
                            </td>
                            <td>
                                <div className="flex gap-2">
                                <button className="btn btn-outline btn-warning hover:!bg-warning" onClick={() => navigate(`${author.id}/edit`)}>Edit</button>
                                <button className="btn btn-outline btn-error hover:!bg-error" onClick={() => handleDelete(author.id)}>Delete</button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            {showDeleteModal && (
                <dialog className="modal modal-open">
                    <div className="modal-box">
                        <h3 className="font-bold text-lg">Confirm Delete</h3>
                        <p className="py-4">Are you sure you want to delete this author? This action cannot be undone.</p>
                        <div className="modal-action">
                            <button className="btn" onClick={handleDeleteCancel}>Cancel</button>
                            <button className="btn btn-error" onClick={handleDeleteConfirm}>Delete</button>
                        </div>
                    </div>
                </dialog>
            )}
        </div>
    )
}

export default Authors