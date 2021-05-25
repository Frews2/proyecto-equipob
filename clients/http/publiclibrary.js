import axios from 'axios';

const urlMS = process.env.MS_PUBLIC_LIBRARY_CONST;
class MicroservicioPubliclibrary
{
    async SearchSong(title, id)
    {
        let url = urlMs + "/Song/SearchSong";
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
        let url = urlMs + "/Song/ShowPendingSongs";
        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ShowRejectedSongs()
    {
        let url = urlMs + "/Song/ShowRejectedSongs";
        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UploadSongRequest(newSong)
    {
        let url = urlMs + "/Song/UploadSongRequest";
        return axios.post(url, newSong)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async QueryGenreSongs(genreId)
    {
        let url = urlMs + "/Song/QueryGenreSongs";
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
        let url = urlMs + "/Song/QueryArtistSongs";
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
        let url = urlMs + "/Song/QueryAlbumSongs";
        return axios.get(url, {
            params: {
                albumId: albumId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async RejectSong(songId){
        let url = urlMs + "/Song/RejectSong";
        return axios.put(url, {
            params:{
                songId : songId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ApproveSong(songId){
        let url = urlMs + "/Song/ApproveSong";
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
        let url = urlMs + "/Music/SearchMusic";
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
        let url = urlMs + "/Music/UploadMusic";
        return axios.post(url, newMusic)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async SearchAlbum(name, id)
    {
        let url = urlMs + "/Album/SearchAlbum";
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
        let url = urlMs + "/Album/UploadAlbum";
        return axios.post(url, newAlbum)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async SearchGenre(name, id)
    {
        let url = urlMs + "/Genre/SearchGenre";
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
        let url = urlMs + "/Genre/UploadGenre";
        return axios.post(url, newGenre)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}

let microservicioPubliclibrary = new MicroservicioPubliclibrary();
export { microservicioPubliclibrary }