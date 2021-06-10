/*
 Date: 13/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
import express from "express";
import audioRouter from "./routes/streaming.js";
import fileupload from "express-fileupload";
const app = express();
const PORT = 8083;


app.use(express.json({ limit: '50mb' }));
app.use(express.urlencoded({ limit: '50mb', extended: true, parameterLimit: 50000 }));
app.use(fileupload());
app.use("/File", audioRouter);

app.all("*", (req, res) => res.status(400).json({
    success: false,
    origin: "streaming_service",
    data: {
    message: "This route does not exist",
    result: null} }));

app.listen(PORT, () => console.log(`Server running on port ${PORT}`));