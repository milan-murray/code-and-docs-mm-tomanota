// Import router package
const router = require('express').Router();

/* Hand get requests for '/'
/* this is the index or home page
*/
router.get('/', (req, res) => {
    res.json({content: 'This is the default route.'});
});

// export
module.exports = router;
