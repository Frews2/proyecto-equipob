import express from 'express';
import accountRouter from './routes/account.js';
import cors from 'cors';

const app = express();

const PORT = 4000;

const allowedOrigins = [
    'http://localhost:8080'
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
app.use(express.urlencoded({extended: true})); 
app.use(express.json());

app.use("/account", cors(corsOptionsDelegate), accountRouter);

app.all("*", cors(corsOptionsDelegate), (req, res) => res.status(404).send({
    success: false,
    msg: "This route does not exist"}));

app.listen(PORT, () => console.log(`Server running in port ${PORT}`));

