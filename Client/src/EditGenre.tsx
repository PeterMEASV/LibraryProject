import {useParams} from "react-router";
import {genreClient} from "./baseUrl.ts";
import {useState} from "react";
import {useEffect} from "react";
import type {UpdateGenreDTO} from "./generated-ts-client.ts";

function EditGenre() {
    const { GenreId } = useParams();

    const [name, setName] = useState('');

    useEffect(() => {
        if(GenreId) {
            genreClient.getGenreById(GenreId).then(genreToEdit =>
            {setName(genreToEdit.name || '')}).catch(err => console.error(err))

        }
    },[GenreId])

    const handleUpdate = () => {
        const updateDto: UpdateGenreDTO = {id: GenreId, name: name};

        genreClient.updateGenre(updateDto).then(res => {
            console.log("Genre updated successfully: ", res)

            // Remember to add Toastify here
        })
    }

    return (
        <div>
            <h1>Edit Genre</h1>
            <p>Name:</p>
            <input type="text" value={name} onChange={(e) => setName(e.target.value)} />

            <button onClick={handleUpdate}> Update Genre</button>
        </div>
    )
}
export default EditGenre