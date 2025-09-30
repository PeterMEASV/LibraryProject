const isProduction = import.meta.env.PROD;

const prod = "https://peters-library-project-server.fly.dev"
const dev = "http://localhost:5228"
export const finalUrl = isProduction ? prod : dev;