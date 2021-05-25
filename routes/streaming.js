import express from 'express';
import { microservicioStreaming } from '../clients/http/streaming.js';

const router = express.Router();

router.get("/streaming", async (req, res) => {
    const { address } = req.query;

    microservicioStreaming.Streaming(address)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("streaming", error);
    })
})

router.post("/save", async(req, res) =>{
    microservicioStreaming.Save(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("/save",error);
    })
})
export default router;