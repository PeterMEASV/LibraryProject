import {AuthorClient, BookClient, GenreClient} from "./generated-ts-client.ts";

const isProduction = import.meta.env.PROD;

const prod = "https://peters-library-project-server.fly.dev"
const dev = "http://localhost:5035"

export const finalUrl = isProduction ? prod : dev;

export const authorClient = new AuthorClient(finalUrl);
export const bookClient = new BookClient(finalUrl);
export const genreClient = new GenreClient(finalUrl);