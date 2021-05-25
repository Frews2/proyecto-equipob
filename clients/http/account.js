import axios from 'axios';

const urlMS = process.env.MS_ACCOUNT_CONST;
class MicroservicioAccount
{
    async RegisterNewAccount(newAccount){
        let url = urlMS + "/Account/RegisterAccount";
        return axios.post(url, newAccount)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async GetAccount(username, id){
        let url =  urlMS + "/Account/SearchAccount";
        return axios.get(url, {
            params: {
                username: username,
                id: id
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async Login(logAccount){
        let url =  urlMS + "/Account/UserLogin";
        console.log(logAccount);
        console.log(url);
        return axios.post(url,logAccount)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UpdateAccount(account){
        let url =  urlMS + "/Account/UpdateAccount";
        return axios.put(url, account)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async BanAccount(accountId){
        let url =  urlMS + "/Account/BanAccount";
        return axios.put(url, {
            params:{
                accountId : accountId
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async UpdatePassword(password){
        let url =  urlMS + "/Password/UpdatePassword";
        return axios.put(url, password)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

}

let microservicioAccount = new MicroservicioAccount();
export { microservicioAccount }