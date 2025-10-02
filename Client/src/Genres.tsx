import './ListPage.css'
import type {GenreDTO} from "./generated-ts-client.ts";
import {useEffect, useState} from "react";
import {genreClient} from "./baseUrl.ts";
import {useNavigate} from "react-router";

type SortField = 'name' | 'createdat';
type SortDirection = 'asc' | 'desc';

function Genres() {
    const [genres, setGenres] = useState<GenreDTO[]>([])
    const [sortField, setSortField] = useState<SortField>('name')
    const [sortDirection, setSortDirection] = useState<SortDirection>('asc')
    const navigate = useNavigate();

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

export default Genres