import axios from 'axios';
import { response } from 'express';

const urlMS = process.env.MS_STREAMING_CONST;
class MicroservicioStreaming
{

    async Save(save)
    {
        let url = urlMS + "/file/save";
        return axios.post(url,save)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}
let microservicioStreaming = new MicroservicioStreaming();
export { microservicioStreaming }