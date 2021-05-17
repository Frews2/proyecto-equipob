import express from 'express';
import { microservicioAccount } from '../clients/http/account.js';

const router = express.Router();

router.get("/getAccount", async (req, res) => {
    const { username, id } = req.query;

    microservicioAccount.GetAccount(username, id)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Account/getAccount", error);
    })
})

router.get("/login", async (req, res) => {
    const { username, password } = req.query;

    microservicioAccount.Login(username, password)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Account/login", error);
    })
})

router.post("/registerNewAccount", async(req, res) =>{
    microservicioAccount.RegisterNewAccount(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Account/register",error);
    })
})

router.put("/updateAccount", async(req, res) =>{
    microservicioAccount.UpdateAccount(newAccount)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Account/updateAccount",error);
    })
})

router.put("/banAccount", async(req, res) =>{
    const { accountId } = req.query;
    microservicioAccount.BanAccount(accountId)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Account/banAccount",error);
    })
})

export default router;