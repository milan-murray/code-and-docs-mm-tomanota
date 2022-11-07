// require imports packages required by the application
const express = require('express');
const cors = require('cors');

const HOST = '127.0.0.1';
const PORT = 5000;


// app is a new instance of express (the web app framework)
let app = express();

// adding Helmet to enhance your API's security
//app.use(helmet());

// Application settings
app.use((req, res, next) => {
	// Globally set Content-Type header for the application
	res.setHeader("Content-Type", "application/json");
	next();
});

// Allow app to support different body content types
app.use(express.text());
// support json encoded bodies
app.use(express.json());
// support url encoded bodies
app.use(express.urlencoded({ extended: true }));

// cors
// https://www.npmjs.com/package/cors
// https://codesquery.com/enable-cors-nodejs-express-app/
// Simple Usage (Enable All CORS Requests)
app.use(cors());
app.options('*', cors()) // include before other routes

/* Configure app Routes to handle requests from browser */
// The home page 
app.use('/', require('./controllers/index'));

// catch 404 and forward to error handler
app.use((req, res, next) => {
	var err = new Error('Not Found: '+ req.method + ":" + req.originalUrl);
	err.status = 404;
	next(err);
});

// Start the HTTP server using HOST address and PORT const's defined above
// Listen for incoming connections
var server = app.listen(PORT, HOST, () => {
	console.log(`Express server listening on http://${HOST}:${PORT}`);
});

// export this as a module, making the app object available when imported.
module.exports = app;
