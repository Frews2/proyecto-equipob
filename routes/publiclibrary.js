import express from 'express';
import { microservicioPubliclibrary } from '../clients/http/publiclibrary.js';

const router = express.Router();

router.get("/searchSong", async (req, res) => {
    const { title, id } = req.query;

    microservicioPubliclibrary.SearchSong(title, id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/searchSong", error);
    })
})

router.get("/showPendingSongs", async (req, res) => {
    microservicioPubliclibrary.ShowPendingSongs()
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/showPendingSongs", error);
    })
})
router.get("/showRejectedSongs", async (req, res) => {
    microservicioPubliclibrary.ShowRejectedSongs()
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/showRejectedSongs", error);
    })
})

router.post("/uploadSongRequest", async(req, res) =>{
    microservicioPubliclibrary.UploadSongRequest(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Account/uploadSongRequest",error);
    })
})

router.get("/queryGenreSongs", async (req, res) => {
    const { genreId } = req.query;

    microservicioPubliclibrary.QueryGenreSongs(genreId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/queryGenreSongs", error);
    })
})
router.get("/queryArtistSongs", async (req, res) => {
    const { artistId } = req.query;

    microservicioPubliclibrary.QueryArtistSongs(artistId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/queryArtistSongs", error);
    })
})
router.get("/queryAlbumSongs", async (req, res) => {
    const { albumId } = req.query;

    microservicioPubliclibrary.QueryArtistSongs(albumId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/queryAlbumSongs", error);
    })
})

router.put("/rejectSong", async(req, res) =>{
    const { songId } = req.query;
    microservicioPubliclibrary.RejectSong(songId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/rejectSong",error);
    })
})
router.put("/approveSong", async(req, res) =>{
    const { songId } = req.query;
    microservicioPubliclibrary.ApproveSong(songId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Song/approveSong",error);
    })
})

router.post("/searchMusic", async (req, res) => {
    const { address , id } = req.query;

    microservicioPubliclibrary.SearchMusic(address , id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Music/searchMusic", error);
    })
})

router.post("/uploadMusic", async(req, res) =>{
    microservicioPubliclibrary.UploadMusic(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Music/uploadMusic",error);
    })
})

router.get("/searchAlbum", async (req, res) => {
    const { name , id } = req.query;

    microservicioPubliclibrary.SearchAlbum(name , id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Album/searchAlbum", error);
    })
})

router.post("/uploadAlbum", async(req, res) =>{
    microservicioPubliclibrary.UploadAlbum(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Album/uploadAlbum",error);
    })
})

router.get("/searchGenre", async (req, res) => {
    const { name , id } = req.query;

    microservicioPubliclibrary.SearchGenre(name , id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Genre/searchGenre", error);
    })
})

router.post("/uploadGenre", async(req, res) =>{
    microservicioPubliclibrary.UploadGenre(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Genre/uploadGenre",error);
    })
})
export default router;