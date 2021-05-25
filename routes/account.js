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

router.post("/login", async (req, res) => {
    microservicioAccount.Login(req.body)
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
    microservicioAccount.UpdateAccount(req.body)
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


router.put("/updatePassword", async(req, res) =>{
    microservicioAccount.UpdatePassword(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("Password/updatePassword",error);
    })
})

export default router;