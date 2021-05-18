import express from "express";
import fileSystem, { fstat } from 'fs';

const router = express.Router();

router.get("/Streaming", async (req, res) => {
    try{
        const {address} = req.query;
        console.log("directorio de canción a reproducir: ", address);

        var filePath = address;
        var stat = fileSystem.statSync(filePath);

        res.writeHead(200, {
        'Content-Type': 'audio/mpeg',
        'Content-Length': stat.size });

        var readStream = fileSystem.createReadStream(filePath);
        readStream.pipe(res); 
    }
    catch (error) {
        console.error("Error en Streaming", error);
        return res.status(400).json({
        success: false,
        origin: "audio_streaming_service",
        data: {
        message: "No se pudo reproducir la canción",
        result: null} }); 
    }
});

router.post("/Save", async (req, res) => {
    try{
        const {address} = req.query;
        var bytes = req.body;
        
        var filePath = address;
        fileSystem.mkdir(filePath, null, function(err){
            if(err){
                console.error("Error en Save", error);
                return res.status(400).json({
                success: false,
                origin: "audio_streaming_service",
                data: {
                message: "Could not save audio",
                result: null} }); 
            } else console.log("Canción guardada en directorio: ", address);
        })

        fileSystem.writeFile(address, bytes, function(err)
        {
            return res.status(200).json({
                success: true,
                origin: "audio_streaming_service",
                data: {
                message: "Audio file saved with address",
                result: address} }); 
        })
    }
    catch (error) {
        console.error("Error en Guardar Audio", error);
        return res.status(400).json({
        success: false,
        origin: "audio_streaming_service",
        data: {
        message: "No se pudo guardar el archivo de audio",
        result: null} }); 
    }
});

export default router;