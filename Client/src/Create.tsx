import './ListPage.css'
import './Create.css'
import {useState, useEffect} from "react";
import {authorClient, bookClient, genreClient} from "./baseUrl.ts";
import type {AuthorDto, GenreDTO} from "./generated-ts-client.ts";

function Create() {

    //Author stuff
    const [authorName, setAuthorName] = useState('')
    const handleCreateAuthor = () => {
        if(authorName) {
            authorClient.createAuthor({name: authorName}).then(res => {
                console.log("Author created successfully: ", res)
                setAuthorName('');
                //todo Probably needs a toastify

                refreshAuthorList();
            })
        }
    }

    //Book stuff
    const [bookTitle, setBookTitle] = useState('')
    const [bookPages, setBookPages] = useState(0)
    const [bookGenreId, setBookGenreId] = useState('')
    const [bookAuthorIDs, setBookAuthorIDs] = useState<string[]>([])
    
    const [availableGenres, setAvailableGenres] = useState<GenreDTO[]>([])
    const [availableAuthors, setAvailableAuthors] = useState<AuthorDto[]>([])
    
    const [genreSearchTerm, setGenreSearchTerm] = useState('')
    const [showGenreDropdown, setShowGenreDropdown] = useState(false)
    
    const [authorSearchTerm, setAuthorSearchTerm] = useState('')
    const [showAuthorDropdown, setShowAuthorDropdown] = useState(false)

    useEffect(() => {
        genreClient.getAllGenres().then(genres => {
            setAvailableGenres(genres);
        }).catch(err => console.error(err));

        authorClient.getAllAuthors().then(authors => {
            setAvailableAuthors(authors);
        }).catch(err => console.error(err));
    }, []);

    const filteredGenres = availableGenres.filter(genre =>
        genre.name?.toLowerCase().includes(genreSearchTerm.toLowerCase())
    );

    const filteredAuthors = availableAuthors.filter(author =>
        author.name?.toLowerCase().includes(authorSearchTerm.toLowerCase()) &&
        !bookAuthorIDs.includes(author.id || '')
    );

    const selectedAuthors = availableAuthors.filter(author => 
        bookAuthorIDs.includes(author.id || '')
    );

    const handleGenreSelect = (genre: GenreDTO) => {
        setBookGenreId(genre.id || '');
        setGenreSearchTerm(genre.name || '');
        setShowGenreDropdown(false);
    };

    const handleAuthorAdd = (author: AuthorDto) => {
        if (author.id && !bookAuthorIDs.includes(author.id)) {
            setBookAuthorIDs([...bookAuthorIDs, author.id]);
        }
        setAuthorSearchTerm('');
        setShowAuthorDropdown(false);
    };

    const handleAuthorRemove = (authorId: string) => {
        setBookAuthorIDs(bookAuthorIDs.filter(id => id !== authorId));
    };

    const refreshAuthorList = (() => {
        authorClient.getAllAuthors().then(authors => {
            setAvailableAuthors(authors);
        }).catch(err => console.error(err));
    })

    const refreshGenreList = (() => {
        genreClient.getAllGenres().then(genres => {
            setAvailableGenres(genres);
        }).catch(err => console.error(err));
    })
    
    const handleCreateBook = () => {
        if (bookTitle && bookPages > 0 && bookGenreId) {
            bookClient.createBook({
                title: bookTitle,
                pages: bookPages,
                genreId: bookGenreId,
                authorIDs: bookAuthorIDs
            }).then(res => {
                console.log("Book created successfully: ", res);
                // Reset form
                setBookTitle('');
                setBookPages(0);
                setBookGenreId('');
                setBookAuthorIDs([]);
                setGenreSearchTerm('');
                //todo Probably needs a toastify
            }).catch(err => console.error(err));
        }
    }

    //Genre stuff
    const [genreName, setGenreName] = useState('')

    const handleCreateGenre = () => {
        if(genreName) {
            genreClient.createGenre({name: genreName}).then(res => {
                console.log("Genre created successfully: ", res)
                setGenreName('');
                //todo Probably needs a toastify

                refreshGenreList();
            })
        }
    }

    return (
        <div style={{width: '80%', margin : 'auto'}}>
            <details className="collapse bg-base-100 border-base-300 border">
                <summary className="collapse-title font-semibold">Click to create a new Author.</summary>
                <div className="collapse-content flex flex-col gap-2">
                    <p>Enter the name</p>
                    <input type="text" value={authorName} onChange={(e) => setAuthorName(e.target.value)}/>
                    <button onClick={() => handleCreateAuthor()}>Create Author</button>
                </div>
            </details>

            <details className="collapse bg-base-100 border-base-300 border">
                <summary className="collapse-title font-semibold">Click to create a new Book.</summary>
                <div className="collapse-content flex flex-col gap-2">
                    <p>Title:</p>
                    <input 
                        type="text" 
                        value={bookTitle} 
                        onChange={(e) => setBookTitle(e.target.value)}
                        placeholder="Enter book title"
                    />

                    <p>Pages:</p>
                    <input 
                        type="number" 
                        value={bookPages} 
                        onChange={(e) => setBookPages(Number(e.target.value))}
                        placeholder="Enter number of pages"
                    />

                    <p>Genre:</p>
                    <div className="dropdown-container">
                        <input type="text" value={genreSearchTerm} onChange={(e) => {setGenreSearchTerm(e.target.value);setShowGenreDropdown(true);}}
                            onFocus={() => setShowGenreDropdown(true)} placeholder="Search for a genre..."/>

                        {showGenreDropdown && filteredGenres.length > 0 && (
                            <div className="dropdown-menu">
                                {filteredGenres.map(genre => (
                                    <div
                                        key={genre.id}
                                        onClick={() => handleGenreSelect(genre)}
                                        className={`dropdown-item ${bookGenreId === genre.id ? 'selected' : ''}`}> {genre.name} </div>))}
                            </div>
                        )}
                    </div>

                    <p>Authors:</p>
                    <div className="dropdown-container">
                        <input type="text" value={authorSearchTerm} onChange={(e) => {setAuthorSearchTerm(e.target.value);setShowAuthorDropdown(true);}}
                            onFocus={() => setShowAuthorDropdown(true)} placeholder="Search for an author..."/>

                        {showAuthorDropdown && filteredAuthors.length > 0 && (
                            <div className="dropdown-menu">
                                {filteredAuthors.map(author => (
                                    <div
                                        key={author.id}
                                        onClick={() => handleAuthorAdd(author)}
                                        className="dropdown-item"> {author.name} </div>
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
                                            className="remove-button">x</button></div>
                                ))}
                            </div>
                        </div>
                    )}

                    <button onClick={() => handleCreateBook()}>Create Book</button>
                </div>
            </details>

            <details className="collapse bg-base-100 border-base-300 border">
                <summary className="collapse-title font-semibold">Click to create a new Genre.</summary>
                <div className="collapse-content flex flex-col gap-2">
                    <p>Enter the name</p>
                    <input type="text" value={genreName} onChange={(e) => setGenreName(e.target.value)}/>
                    <button onClick={() => handleCreateGenre()}>Create Genre</button>
                </div>
            </details>
        </div>
    )
}
export default Create