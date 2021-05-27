import express from 'express';
import { microservicioStreaming } from '../clients/http/streaming.js';
import got from 'got';

const router = express.Router();
const urlMS = process.env.MS_STREAMING_CONST;

router.get("/streaming", async (req, res) => {
    try{ 
        const {address} = req.query;
        let url = urlMS + "/file/stream"+"?address="+address;
        console.log(url);

        got.stream(url).pipe(res);
    }
    catch (error) {
        console.error("Error en Streaming", error);
        return res.status(400).json({
        success: false,
        origin: "streaming_service",
        data: {
        message: "No se pudo reproducir la canción",
        result: null} }); 
    }
    finally
    {
        console.log("AAAAAAAAAAAAA");
    }
})

router.post("/save", async(req, res) =>{
    microservicioStreaming.Save(req)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("/save",error);
    })
})
export default router;