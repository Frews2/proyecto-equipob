import axios from 'axios';

const urlMS = process.env.MS_STREAMING_CONST;
class MicroservicioStreaming
{
    async Streaming(address)
    {
        let url = urlMs + "/audio/Streaming";
        return axios.get(url, {
            params: {
                address: address
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async Save(save)
    {
        let url = "http://localhost:8081/audio/Save";
        return axios.post(url, save)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}
let microservicioStreaming = new MicroservicioStreaming();
export { microservicioStreaming }