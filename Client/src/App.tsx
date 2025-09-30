import './App.css'
import {authorClient, finalUrl} from './baseUrl.ts'
import {useEffect, useState} from "react";
import type {Author, CreateAuthorDTO} from "./generated-ts-client.ts";
function App() {

    const [authors, setAuthors] = useState<Author[]>([])
    const [myForm, setMyForm] = useState<CreateAuthorDTO>({name: ""})

    useEffect( () => {
    authorClient.getAllAuthors().then(res => {
        setAuthors(res)
    })
    }, [])


  return (
    <>
        <input value={myForm.name} onChange= {e=> setMyForm({name: e.target.value})} placeholder={"Name here"} />
        <button onClick={() => {
            authorClient.createAuthor(myForm).then(res => {
                console.log("wat")
                setAuthors([...authors, res])
            })

        }}>Create stuff</button>
        <hr />
        {
            authors.map(author => {
                return <div key={author.id}>
                    {JSON.stringify(author)}
                </div>
            })
        }
    </>
  )
}

export default App
