import axios from 'axios';


const urlMS = process.env.MS_PRIVATE_LIBRARY_CONST;
class MicroservicioPrivatelibrary
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
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UploadMusic(newSong)
    {
        let url = urlMS + "/Song/UploadSong";
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

    async RequestSong(songId){
        let url = urlMS + "/Song/RequestSong";
        return axios.put(url, {
            params:{
                songId : songId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UpdateSong(update){
        let url = urlMS + "/Song/UpdateSong";
        return axios.put(url, update)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async DeleteSong(id){
        let url = urlMS + "/Song/DeleteSong";
        return axios.delete(url, {
            params:{
                id : id
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async SearchMusic(address, id)
    {
        let url = urlMS + "/Music/SearchMusic";
        return axios.get(url, {
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
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UpdateMusic(update){
        let url = urlMS + "/Music/UpdateMusic";
        return axios.put(url, update)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async DeleteMusic(id){
        let url = urlMS + "/Music/DeleteMusic";
        return axios.delete(url, {
            params:{
                id : id
            }
        })
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

    async UpdateGenre(update){
        let url = urlMS + "/Genre/UpdateGenre";
        return axios.put(url, update)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async DeleteGenre(id){
        let url = urlMS + "/Genre/DeleteGenre";
        return axios.delete(url, {
            params:{
                id : id
            }
        })
        .then(response => {return response.data})
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

    async UpdateAlbum(update){
        let url = urlMS + "/Album/UpdateAlbum";
        return axios.put(url, update)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async DeleteAlbum(id){
        let url = urlMS + "/Album/DeleteAlbum";
        return axios.delete(url, {
            params:{
                id : id
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}
let microservicioPrivatelibrary = new MicroservicioPrivatelibrary();
export { microservicioPrivatelibrary }