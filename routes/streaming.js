import express from "express";
import fileSystem, { fstat } from 'fs';

const router = express.Router();

router.get("/Stream", async (req, res) => {
    try{
        
        const {address} = req.query;
        var filePath = process.cwd() + "/spotyme/" + address;
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
        origin: "streaming_service",
        data: {
        message: "No se pudo reproducir la canción",
        result: null} }); 
    }
});

router.post("/Save", async (req, res) => {
    
    try{
        const {album} = req.query;
        const {archivo} = req.files;

        var folderPath = process.cwd() + "/spotyme/" + album + "/";
        var filePath = folderPath + archivo.name;
        
        let arregloBytes = archivo;

        fileSystem.mkdir(folderPath, null, function (err) {
            if (err) {
                console.log('AVISO: Guardando en directorio de album existente');
            };
        })

        fileSystem.writeFile(filePath, arregloBytes.data, function (err) {
            return res.status(200).json({
                success: true,
                origin: "streaming_service",
                data: {
                message: "Se guardo el archivo en directorio ",
                result: filePath} }); 
        })
    }
    catch (error) {
        console.error("Error en Guardar Audio", error);
        return res.status(400).json({
        success: false,
        origin: "streaming_service",
        data: {
        message: "No se pudo guardar el archivo de audio",
        result: null} }); 
    }
});

router.get("/View", async (req, res) => {
    try{
        const {path} = req.query;
        const {type} = req.query;
        var filePath = process.cwd() + "/spotyme/" + path + type;

        var contentType = null;

        if ( type === ".png") {
            contentType = "image/png";
        } 
        else if ( type === ".jpeg") {
            contentType = "image/jpeg";
        } 
        else if ( type === ".jpg") {
            contentType = "image/jpg";
        }

        fileSystem.readFile(filePath, function(err, content)
        {
            if(err) {return res.status(400).json({
                success: false,
                origin: "audio_streaming_service",
                data: {
                message: "No se pudo encontrar la imagen en ruta" + filePath,
                result: null} });
            }

            res.writeHead(200, { 
                "Content-Type": contentType });
            res.end(content, 'utf-8');
        })
    }
    catch (error) {
        console.error("Error en Streaming", error);
        return res.status(400).json({
        success: false,
        origin: "audio_streaming_service",
        data: {
        message: "No se pudo mostrar la imagen",
        result: null} }); 
    }
});

export default router;