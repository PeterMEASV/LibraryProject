import './EditPage.css';
import {useParams} from "react-router";
import {bookClient, genreClient, authorClient} from "./baseUrl.ts";
import {useState, useEffect} from "react";
import type {UpdateBookDto, GenreDTO, AuthorDto} from "./generated-ts-client.ts";

function EditBook() {
    const { BookId } = useParams();

    const [title, setTitle] = useState('');
    const [pages, setPages] = useState(0);
    const [genreId, setGenreId] = useState('');
    const [authorIDs, setAuthorIDs] = useState<string[]>([]);
    
    const [availableGenres, setAvailableGenres] = useState<GenreDTO[]>([]);
    const [availableAuthors, setAvailableAuthors] = useState<AuthorDto[]>([]);
    
    const [genreSearchTerm, setGenreSearchTerm] = useState('');
    const [showGenreDropdown, setShowGenreDropdown] = useState(false);
    
    const [authorSearchTerm, setAuthorSearchTerm] = useState('');
    const [showAuthorDropdown, setShowAuthorDropdown] = useState(false);

    useEffect(() => {
        if(BookId) {
            bookClient.getBookById(BookId).then(bookToEdit => {
                setTitle(bookToEdit.title || '');
                setPages(bookToEdit.pages || 0);
                setGenreId(bookToEdit.genre || '');
                setAuthorIDs(bookToEdit.authorIDs?.map(a => a.id).filter((id): id is string => id !== undefined) || []); //This is the only way it works. I don't know why.
            }).catch(err => console.error(err));
        }



        // Should fetch all genres and authors.
        genreClient.getAllGenres().then(genres => {
            setAvailableGenres(genres);
        }).catch(err => console.error(err));


        authorClient.getAllAuthors().then(authors => {
            setAvailableAuthors(authors);
        }).catch(err => console.error(err));
    }, [BookId]);




    // Update genre search term when genreId changes and genres are loaded
    useEffect(() => {
        if (genreId && availableGenres.length > 0) {
            const selectedGenre = availableGenres.find(g => g.id === genreId);
            if (selectedGenre) {
                setGenreSearchTerm(selectedGenre.name || '');
            }
        }
    }, [genreId, availableGenres]);

    const filteredGenres = availableGenres.filter(genre =>
        genre.name?.toLowerCase().includes(genreSearchTerm.toLowerCase())
    );



    const filteredAuthors = availableAuthors.filter(author =>
        author.name?.toLowerCase().includes(authorSearchTerm.toLowerCase()) &&
        !authorIDs.includes(author.id || '')
    );

    const selectedAuthors = availableAuthors.filter(author => 
        authorIDs.includes(author.id || '')
    );

    const handleGenreSelect = (genre: GenreDTO) => {
        setGenreId(genre.id || '');
        setGenreSearchTerm(genre.name || '');
        setShowGenreDropdown(false);
    };

    const handleAuthorAdd = (author: AuthorDto) => {
        if (author.id && !authorIDs.includes(author.id)) {
            setAuthorIDs([...authorIDs, author.id]);
        }
        setAuthorSearchTerm('');
        setShowAuthorDropdown(false);
    };

    const handleAuthorRemove = (authorId: string) => {
        setAuthorIDs(authorIDs.filter(id => id !== authorId));
    };

    const handleUpdate = () => {
        if (!BookId) return;

        const updateDto: UpdateBookDto = {
            id: BookId,
            title: title,
            pages: pages,
            genreId: genreId,
            authorIDs: authorIDs
        };

        bookClient.updateBook(updateDto).then(res => {
            console.log("Book updated successfully: ", res);
            // Remember to add Toastify here
        }).catch(err => console.error(err));
    };

    return (
        <div className="edit-container">
            <h1>Edit Book</h1>
            
            <div className="edit-form">
                <p>Title:</p>
                <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} className="edit-input"/>

                <p>Pages:</p>
                <input type="number" value={pages} onChange={(e) => setPages(Number(e.target.value))} className="edit-input"/>

                <p>Genre:</p>
                <div className="dropdown-container">
                    <input type="text" value={genreSearchTerm} onChange={(e) => {setGenreSearchTerm(e.target.value); setShowGenreDropdown(true);}}

                        //TODO: Should probably fix not being able to click out :P
                        onFocus={() => setShowGenreDropdown(true)} placeholder="Search for a genre..." className="edit-input"/>



                    {showGenreDropdown && filteredGenres.length > 0 && (
                        <div className="dropdown-menu">
                            {filteredGenres.map(genre => (
                                <div
                                    key={genre.id}
                                    onClick={() => handleGenreSelect(genre)}
                                    className={`dropdown-item ${genreId === genre.id ? 'selected' : ''}`}>{genre.name}</div>
                            ))}
                        </div>
                    )}
                </div>

                <p>Authors:</p>
                <div className="dropdown-container">
                    <input type="text" value={authorSearchTerm} onChange={(e) => {setAuthorSearchTerm(e.target.value);setShowAuthorDropdown(true);}}

                           //TODO: Same problem here as the genre dropdown
                        onFocus={() => setShowAuthorDropdown(true)} placeholder="Search for an author..." className="edit-input"/>

                    {showAuthorDropdown && filteredAuthors.length > 0 && (
                        <div className="dropdown-menu">
                            {filteredAuthors.map(author => (
                                <div
                                    key={author.id}
                                    onClick={() => handleAuthorAdd(author)}
                                    className="dropdown-item">{author.name}</div>
                            ))}
                        </div>
                    )}
                </div>

                {selectedAuthors.length > 0 && (
                    <div className="selected-items-container">
                        <p className="selected-items-label">Selected Authors:</p>
                        <div className="selected-items-list">
                            {selectedAuthors.map(author => (
                                <div key={author.id} className="selected-item-tag">
                                    <span>{author.name}</span>
                                    <button
                                        onClick={() => handleAuthorRemove(author.id || '')}
                                        className="remove-button">x</button>
                                </div>
                            ))}
                        </div>
                    </div>
                )}

                <button onClick={handleUpdate} className="update-button">Update Book</button>
            </div>
        </div>
    );
}

export default EditBook;