const router = require('express').Router();
const documents = require('../services/document_service.js');

router.get('/', function (req, res) {
    // Endpoint: /quiz
    res.json({content: 'This is the default for the document quiz'});
});

router.get('/existing_doc', (req, res) => {
    // Endpoint: /quiz/existing_doc

    // Endpoint: /quiz/existing_doc?f=español_viaje_1.md
    const file_name = req.query.f;

    file_data = documents.parseExisting(file_name);

    if (typeof file_data === 'undefined')
    {
        res.statusMessage = "Bad Request - file not found";
        res.status(400).json({content: 'error'});
    }

    res.json(file_data);
});

module.exports = router;
