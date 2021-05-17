import axios from 'axios';

class MicroservicioAccount
{
    
    async RegisterNewAccount(newAccount){
        let url = "http://localhost:8080/Account/RegisterAccount";
        return axios.post(url, newAccount)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async GetAccount(username, id){
        let url = "http://localhost:8080/Account/SearchAccount";
        return axios.get(url, {
            params: {
                username: username,
                id: id
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async Login(username, password){
        let url = "http://localhost:8080/Account/UserLogin";
        return axios.get(url, {
            params: {
                username: username,
                password: password
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UpdateAccount(newAccount){
        let url = "http://localhost:8080/Account/UpdateAccount";
        return axios.put(url, newAccount)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
     }

     async BanAccount(accountId){
        let url = "http://localhost:8080/Account/BanAccount";
        return axios.put(url, {
            params:{
                accountId : accountId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
     }

}

let microservicioAccount = new MicroservicioAccount();
export { microservicioAccount }