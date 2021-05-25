import express from 'express';
import { microservicioPrivatelibrary } from '../clients/http/privatelibrary.js';

const router = express.Router();

router.get("/searchSong", async (req, res) => {
    const { title, id } = req.query;

    microservicioPrivatelibrary.SearchSong(title, id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/searchSong", error);
    })
})

router.post("/uploadSong", async(req, res) =>{
    microservicioPrivatelibrary.UploadSong(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/uploadSong",error);
    })
})

router.get("/queryGenreSongs", async (req, res) => {
    const { genreId } = req.query;

    microservicioPrivatelibrary.QueryGenreSongs(genreId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/queryGenreSongs", error);
    })
})

router.get("/queryArtistSongs", async (req, res) => {
    const { artistId } = req.query;

    microservicioPrivatelibrary.QueryArtistSongs(artistId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/queryArtistSongs", error);
    })
})

router.get("/queryAlbumSongs", async (req, res) => {
    const { albumId } = req.query;

    microservicioPrivatelibrary.QueryAlbumSongs(albumId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/queryAlbumSongs", error);
    })
})

router.put("/requestSong", async(req, res) =>{
    const { songId } = req.query;
    microservicioPrivatelibrary.RequestSong(songId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/requestSong",error);
    })
})

router.put("/updateSong", async(req, res) =>{
    microservicioPrivatelibrary.UpdateSong(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/updateSong",error);
    })
})

router.delete("/deleteSong", async(req,res) =>{
    const { id } = req.query;
    microservicioPrivatelibrary.DeleteSong(id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/deleteSong",error);
    })
})

router.get("/searchMusic", async (req, res) => {
    const {address , id } = req.query;

    microservicioPrivatelibrary.SearchMusic(address , id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Music/searchMusic", error);
    })
})

router.post("/uploadMusic", async(req, res) =>{
    microservicioPrivatelibrary.UploadMusic(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Music/uploadMusic",error);
    })
})

router.put("/updateMusic", async(req, res) =>{
    microservicioPrivatelibrary.UpdateMusic(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/updateMusic",error);
    })
})

router.delete("/deleteMusic", async(req,res) =>{
    const { id } = req.query;
    microservicioPrivatelibrary.DeleteMusic(id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/deleteMusic",error);
    })
})

router.get("/searchGenre", async (req, res) => {
    const { name, id } = req.query;

    microservicioPrivatelibrary.SearchGenre(name, id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Genre/searchGenre", error);
    })
})

router.post("/uploadGenre", async(req, res) =>{
    microservicioPrivatelibrary.UploadGenre(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Genre/uploadGenre",error);
    })
})

router.put("/updateGenre", async(req, res) =>{
    microservicioPrivatelibrary.UpdateGenre(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Genre/updateGenre",error);
    })
})

router.delete("/deleteGenre", async(req,res) =>{
    const { id } = req.query;
    microservicioPrivatelibrary.DeleteGenre(id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Genre/deleteGenre",error);
    })
})

router.get("/searchAlbum", async (req, res) => {
    const { name, id } = req.query;

    microservicioPrivatelibrary.SearchAlbum(name, id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Album/searchAlbum", error);
    })
})

router.post("/uploadAlbum", async(req, res) =>{
    microservicioPrivatelibrary.UploadAlbum(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Album/uploadAlbum",error);
    })
})

router.put("/updateAlbum", async(req, res) =>{
    microservicioPrivatelibrary.UpdateAlbum(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Album/updateAlbum",error);
    })
})

router.delete("/deleteAlbum", async(req,res) =>{
    const { id } = req.query;
    microservicioPrivatelibrary.DeleteAlbum(id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Genre/deleteAlbum",error);
    })
})
export default router;