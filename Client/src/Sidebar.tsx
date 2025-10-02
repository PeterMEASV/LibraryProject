import './Sidebar.css'
import {Outlet, useNavigate} from "react-router";
function Sidebar() {

    const navigate = useNavigate();
    
    const toggleNav = () => {
        const panel = document.getElementById("mySidepanel");
        if (panel) {
            if (panel.style.width === "300px") {
                panel.style.width = "50px";
            } else {
                panel.style.width = "300px";
            }
        }
    }

    return (
        <>
            <div id="mySidepanel" className="sidepanel">
                <a href="#" className="closebtn" onClick={toggleNav}>&#9776;</a>
                <div className="menu-items">
                    <a onClick={() => {navigate('')}}>Home</a>
                    <a onClick={() => {navigate('create')}}>Create Entries</a>
                    <a onClick={() => {navigate('authors')}}>Authors</a>
                    <a onClick={() => {navigate('books')}}>Books</a>
                    <a onClick={() => {navigate('genres')}}>Genres</a>
                </div>
            </div>
            <header className="page-header">
                <h1>Peter's Library</h1>
            </header>
            <div className="main-content">
                <Outlet />
            </div>
        </>
    )
}
export default Sidebar