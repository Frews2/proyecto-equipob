import express from 'express';
import got from 'got';
import axios from 'axios';
import FormData from 'form-data';
import fs from 'fs';
import fileSystem, { fstat } from 'fs';

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
    console.log(req);
    const {album} = req.query;
    var config ={
        method:'post',
        url: urlMS + "/file/save?album="+album,
        headers: {
        },
        maxContentLength: Infinity,
        maxBodyLength: Infinity,
        
        data : req.files
    };
    axios(config)
    .then(resp => {
        res.send(resp.data);
    })
    .catch(error => {
        res.send(error);
    });
})

router.get("/view", async(req, res) =>{
    const {type} = req.query;
    const {path} = req.query;
    console.log(type);
    console.log(path);

    var config ={
        method:'get',
        url: urlMS + "/file/view?type="+type+"&path="+path,
        headers: {
        },
        maxContentLength: Infinity,
        maxBodyLength: Infinity,
    };
    axios(config)
    .then(resp => {
        var binary = Buffer.from(resp.data,'utf-8');
            res.writeHead(200, { 
                "Content-Type": "image/jpg" });
            res.end(binary, 'utf-8');
    })
    .catch(error => {
        res.send(error);
    });
})


export default router;