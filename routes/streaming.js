import express from "express";
import fileSystem from 'fs';

const router = express.Router();

router.get("/streaming", async (req, res) => {
    try{
        const {address} = req.query;
        console.log("directorio de canción: ", address);

        var filePath = address;
        var stat = fileSystem.statSync(filePath);

        res.writeHead(200, {
        'Content-Type': 'audio/mp3',
        'Content-Length': stat.size });

        var readStream = fileSystem.createReadStream(filePath);
        readStream.pipe(res); 
    }
    catch (error) {
        console.error("Error en Stream Audio", error);
        return res.status(400).json({
        success: false,
        origin: "audio_streaming_service",
        data: {
        message: "Could not play audio",
        result: null} }); 
    }
});

export default router;