import express from 'express';
import accountRouter from './routes/account.js';
import publiclibraryRouter from './routes/publiclibrary.js';
import privatelibraryRouter from './routes/privatelibrary.js';
import streamingRouter from './routes/streaming.js';
import cors from 'cors';
import fileupload from "express-fileupload";

const app = express();

const PORT = 4000;

const allowedOrigins = [
    'http://localhost:8080',
    'http://localhost:8081',
    'http://localhost:8082',
    'http://localhost:8083'
];

var corsOptionsDelegate = function (req, callback) {
    var corsOptions;
    if (allowedOrigins.indexOf(req.header('Origin')) !== -1) {
        corsOptions = { origin: true } // reflect (enable) the requested origin in the CORS response
    } else {
        corsOptions = { origin: false } // disable CORS for this request
    }
    callback(null, corsOptions) // callback expects two parameters: error and options
}
 
app.use(cors());
app.use(fileupload());
app.use(express.json({ limit: '50mb' }));
app.use(express.urlencoded({ limit: '50mb', extended: true, parameterLimit: 50000 }));

app.use("/account", cors(corsOptionsDelegate), accountRouter);
app.use("/publiclibrary", cors(corsOptionsDelegate), publiclibraryRouter);
app.use("/privatelibrary", cors(corsOptionsDelegate), privatelibraryRouter);
app.use("/streaming", cors(corsOptionsDelegate), streamingRouter);


app.all("*", cors(corsOptionsDelegate), (req, res) => res.status(404).send({
    success: false,
    msg: "This route does not exist"}));

app.listen(PORT, () => console.log(`Server running in port ${PORT}`));

