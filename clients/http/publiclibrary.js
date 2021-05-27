import axios from 'axios';

const urlMS = process.env.MS_PUBLIC_LIBRARY_CONST;
class MicroservicioPubliclibrary
{
    async SearchSong(title, id)
    {
        let url = urlMS + "/Song/SearchSong";
        return axios.get(url, {
            params: {
                title: title,
                id: id
            }
        })
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async ShowPendingSongs()
    {
        let url = urlMS + "/Song/ShowPendingSongs";
        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ShowRejectedSongs()
    {
        let url = urlMS + "/Song/ShowRejectedSongs";
        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UploadSongRequest(newSong)
    {
        let url = urlMS + "/Song/UploadSongRequest";
        return axios.post(url, newSong)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async QueryGenreSongs(genreId)
    {
        let url = urlMS + "/Song/QueryGenreSongs";
        return axios.get(url, {
            params: {
                genreId: genreId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async QueryArtistSongs(artistId)
    {
        let url = urlMS + "/Song/QueryArtistSongs";
        return axios.get(url, {
            params: {
                artistId: artistId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async QueryAlbumSongs(albumId)
    {
        let url = urlMS + "/Song/QueryAlbumSongs";
        return axios.get(url, {
            params: {
                albumId: albumId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async RejectSong(songId){
        let url = urlMS + "/Song/RejectSong";
        return axios.put(url, {
            params:{
                songId : songId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ApproveSong(songId){
        let url = urlMS + "/Song/ApproveSong";
        return axios.put(url, {
            params:{
                songId : songId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async SearchMusic(address, id)
    {
        let url = urlMS + "/Music/SearchMusic";
        return axios.post(url, {
            params: {
                address: address,
                id: id
            }
        })
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async UploadMusic(newMusic)
    {
        let url = urlMS + "/Music/UploadMusic";
        return axios.post(url, newMusic)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async SearchAlbum(name, id)
    {
        let url = urlMS + "/Album/SearchAlbum";
        return axios.get(url, {
            params: {
                name: name,
                id: id
            }
        })
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async UploadAlbum(newAlbum)
    {
        let url = urlMS + "/Album/UploadAlbum";
        return axios.post(url, newAlbum)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async SearchGenre(name, id)
    {
        let url = urlMS + "/Genre/SearchGenre";
        return axios.get(url, {
            params: {
                name: name,
                id: id
            }
        })
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async UploadGenre(newGenre)
    {
        let url = urlMS + "/Genre/UploadGenre";
        return axios.post(url, newGenre)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}

let microservicioPubliclibrary = new MicroservicioPubliclibrary();
export { microservicioPubliclibrary }