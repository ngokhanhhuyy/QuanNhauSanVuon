import express from "express";
import { MenuItemController } from "./controllers";
import dotenv from "dotenv";
dotenv.config();
const app = express();
const router = express.Router();
const PORT = process.env.PORT;
app.use(express.json());
app.use((request, response, next) => {
    response.append("Content-Type", "application/json");
    next();
    const statusCodePrefix = Math.floor(response.statusCode / 100);
    const colorsByStatusCodePrefix = {
        [2]: "\x1b[42m",
        [3]: "\x1b[43m",
        [4]: "\x1b[41m",
        [5]: "\x1b[45m",
    };
    const key = statusCodePrefix;
    const color = colorsByStatusCodePrefix[key];
    console.log(`${color} ${response.statusCode} \x1b[0m ${request.url}`);
});
app.get("/", (_, response) => {
    response.status(200).send(JSON.stringify("Hello World"));
});
// Registry controllers.
MenuItemController.registryController(router);
app.listen(PORT, () => {
    console.log(`\x1b[32m Server running at: \x1b[42mhttp://localhost:${PORT}\x1b[0m`);
}).on("error", (error) => {
    throw new Error(error.message);
});
