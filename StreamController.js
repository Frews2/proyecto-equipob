/**
 * NPM Module dependencies.
 */
 const express = require('express');
 const trackRoute = express.Router();
 const multer = require('multer');
 
 const mongodb = require('mongodb');
 const MongoClient = require('mongodb').MongoClient;
 const ObjectID = require('mongodb').ObjectID;
 
 /**
  * NodeJS Module dependencies.
  */
 const { Readable } = require('stream');
 
 /**
  * Create Express server && Express Router configuration.
  */
 const app = express();
 app.use('/tracks', trackRoute);
 
 /**
  * Connect Mongo Driver to MongoDB.
  */
 let db;
 MongoClient.connect('mongodb://spotymeAdmin:proyectoredes@mongo_public_library', (err, database) => {
   if (err) {
     console.log('MongoDB Connection Error. Please make sure that MongoDB is running.');
     process.exit(1);
   }
   db = database;
 });