import './EditPage.css';
import {useParams} from "react-router";
import {authorClient, bookClient} from "./baseUrl.ts";
import {useState, useEffect} from "react";
import type {UpdateAuthorDTO, BookDto} from "./generated-ts-client.ts";
import {toast} from "react-toastify";

function EditAuthor() {
    const { AuthorId } = useParams();

    const [name, setName] = useState('');
    const [bookIds, setBookIds] = useState<string[]>([]);
    
    const [availableBooks, setAvailableBooks] = useState<BookDto[]>([]);
    
    const [bookSearchTerm, setBookSearchTerm] = useState('');
    const [showBookDropdown, setShowBookDropdown] = useState(false);

    useEffect(() => {
        if(AuthorId) {
            authorClient.getAuthorById(AuthorId).then(authorToEdit => {
                setName(authorToEdit.name || '');
                setBookIds(authorToEdit.books?.map(b => b.id).filter((id): id is string => id !== undefined) || []);
            }).catch(err => console.error(err));
        }


        bookClient.getAllBooks().then(books => {
            setAvailableBooks(books);
        }).catch(err => console.error(err));
    }, [AuthorId]);

    const filteredBooks = availableBooks.filter(book =>
        book.title?.toLowerCase().includes(bookSearchTerm.toLowerCase()) &&
        !bookIds.includes(book.id || '')
    );

    const selectedBooks = availableBooks.filter(book => 
        bookIds.includes(book.id || '')
    );

    const handleBookAdd = (book: BookDto) => {
        if (book.id && !bookIds.includes(book.id)) {
            setBookIds([...bookIds, book.id]);
        }
        setBookSearchTerm('');
        setShowBookDropdown(false);
    };

    const handleBookRemove = (bookId: string) => {
        setBookIds(bookIds.filter(id => id !== bookId));
    };

    const handleUpdate = () => {
        if (!AuthorId) return;

        const updateDto: UpdateAuthorDTO = {
            id: AuthorId,
            name: name,
            bookIds: bookIds
        };

        authorClient.updateAuthor(updateDto).then(res => {
            console.log("Author updated successfully: ", res);
            toast.success("Author updated successfully!");
        }).catch(err => {
            console.error(err);
            toast.error("Failed to update author.");
        });
    };

    return (
        <div className="edit-container">
            <h1>Edit Author</h1>
            
            <div className="edit-form">
                <p>Name:</p>
                <input type="text" value={name} onChange={(e) => setName(e.target.value)} className="edit-input"/>

                <p>Books:</p>
                <div className="dropdown-container">
                    <input type="text" value={bookSearchTerm} onChange={(e) => {setBookSearchTerm(e.target.value);setShowBookDropdown(true);}}
                        onFocus={() => setShowBookDropdown(true)} placeholder="Search for a book..." className="edit-input"/>



                    {showBookDropdown && filteredBooks.length > 0 && (
                        <div className="dropdown-menu">
                            {filteredBooks.map(book => (
                                <div key={book.id} onClick={() => handleBookAdd(book)} className="dropdown-item">{book.title}</div>
                            ))}
                        </div>
                    )}
                </div>

                {selectedBooks.length > 0 && (
                    <div className="selected-items-container">
                        <p className="selected-items-label">Selected Books:</p>
                        <div className="selected-items-list">
                            {selectedBooks.map(book => (
                                <div key={book.id} className="selected-item-tag">
                                    <span>{book.title}</span>
                                    <button onClick={() => handleBookRemove(book.id || '')} className="remove-button">x</button>
                                </div>
                            ))}
                        </div>
                    </div>
                )}

                <button onClick={handleUpdate} className="update-button">Update Author</button>
            </div>
        </div>
    );
}

export default EditAuthor;