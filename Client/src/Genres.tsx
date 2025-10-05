import './ListPage.css'
import type {GenreDTO} from "./generated-ts-client.ts";
import {useEffect, useState} from "react";
import {genreClient} from "./baseUrl.ts";
import {useNavigate} from "react-router";
import {toast} from "react-toastify";

type SortField = 'name' | 'createdat';
type SortDirection = 'asc' | 'desc';

function Genres() {
    const [genres, setGenres] = useState<GenreDTO[]>([])
    const [sortField, setSortField] = useState<SortField>('name')
    const [sortDirection, setSortDirection] = useState<SortDirection>('asc')
    const navigate = useNavigate();
    const [genreToDelete, setGenreToDelete] = useState<string | undefined>(undefined)
    const [showDeleteModal, setShowDeleteModal] = useState(false)

    useEffect( () => {
        genreClient.getAllGenres().then(res => {
            setGenres(res)
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

    const sortedGenres = [...genres].sort((a, b) => {
        let compareValue = 0
        
        switch (sortField) {
            case 'name':
                compareValue = (a.name || '').localeCompare(b.name || '')
                break
            case 'createdat':
                compareValue = (a.createdat || '').localeCompare(b.createdat || '')
                break
        }
        
        return sortDirection === 'asc' ? compareValue : -compareValue
    })

    const SortIcon = ({ field }: { field: SortField }) => {
        if (sortField !== field) return <span className="opacity-30">⇅</span>
        return sortDirection === 'asc' ? <span>↑</span> : <span>↓</span>
    }

    const handleDelete = (id: string | undefined) => {
        setGenreToDelete(id)
        setShowDeleteModal(true)
    }

    const handleDeleteConfirm = () => {
        if (!genreToDelete) return;

        genreClient.deleteGenre({ id: genreToDelete }).then(() => {
            toast.success("Genre deleted successfully!");
            setGenres(genres.filter(genre => genre.id !== genreToDelete));
            setShowDeleteModal(false);
            setGenreToDelete(undefined);
        }).catch(err => {
            console.error(err);
            toast.error("Failed to delete genre.");
        })
    }

    const handleDeleteCancel = () => {
        setShowDeleteModal(false)
        setGenreToDelete(undefined)
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
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {sortedGenres.map((genre, index) => (
                        <tr key={genre.id} className="hover:bg-base-300">
                            <th>{index + 1}</th>
                            <td>{genre.name}</td>
                            <td>
                                {genre.createdat
                                    ? new Date(genre.createdat).toLocaleDateString()
                                    : 'N/A'
                                }
                            </td>
                            <td>
                                <div className="flex gap-2">
                                    <button className="btn btn-outline btn-warning hover:!bg-warning" onClick={() => navigate(`${genre.id}/edit`)}>Edit</button>
                                <button className="btn btn-outline btn-error hover:!bg-error" onClick={() => handleDelete(genre.id)}>Delete</button>
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
                        <p className="py-4">Are you sure you want to delete this genre? This action cannot be undone.</p>
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

export default Genres