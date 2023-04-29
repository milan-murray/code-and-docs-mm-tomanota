
const escapeHtml = require('escape-html');

exports.checkPromptsHttp = (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Allow-Methods", "GET");
    res.setHeader("Access-Control-Allow-Headers", "Content-Type");

    const prompt = req.query.q ? req.query.q.normalize("NFC") : undefined;
    const answer = req.query.a ? req.query.a.normalize("NFC") : undefined;

    if (!prompt || !answer) {
        res.status(200).json({
            areEqual: false,
            index: -1,
            correctAnswer: answer,
            userAttempt: prompt
        });
        return;
    }

    if (prompt === answer) {
        res.status(200).json({
            areEqual: true,
            index: -1,
            correctAnswer: answer,
            userAttempt: prompt
        });
        return;
    }

    let i = 0
    for (i; i < Math.min(prompt.length, answer.length); i++) {
        if (prompt[i] !== answer[i]) {
            res.status(200).json({
                areEqual: false,
                index: i,
                correctAnswer: answer,
                userAttempt: prompt
            });
            return;
        }
    }

    res.status(200).json({
        areEqual: false,
        index: i,
        correctAnswer: answer,
        userAttempt: prompt
    });
};

// EN -> IE
exports.ENTitlesIEHttp = (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Allow-Methods", "GET");
    res.setHeader("Access-Control-Allow-Headers", "Content-Type");

    const irishTitles = [
        "IE-Travel-1"
    ]

    res.json(irishTitles);
}

// EN -> DE
exports.ENTitlesDEHttp = (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Allow-Methods", "GET");
    res.setHeader("Access-Control-Allow-Headers", "Content-Type");

    const germanTitles = [
        "DE-Travel-1"
    ]

    res.json(germanTitles);
}

// EN -> ES
exports.ENTitlesESHttp = (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Allow-Methods", "GET");
    res.setHeader("Access-Control-Allow-Headers", "Content-Type");

    const spanishTitles = [
        "ES-Travel-1",
        "ES-Food-1",
        "ES-Indefinite-Past-1"
    ]

    res.json(spanishTitles);
}

// ES -> EN
exports.ESTitlesENHttp = (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Allow-Methods", "GET");
    res.setHeader("Access-Control-Allow-Headers", "Content-Type");

    const englishTitles = [
        "EN-Viajes-1",
        "EN-Comida-1",
        "EN-Pasado-Indefinido-1"
    ]

    res.json(englishTitles);
}

// DE -> EN
exports.DETitlesENHttp = (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Allow-Methods", "GET");
    res.setHeader("Access-Control-Allow-Headers", "Content-Type");

    const englishTitles = [
        "EN-Reise-1"
    ]

    res.json(englishTitles);
}

exports.getPromptsHttp = (req, res) => {
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Allow-Methods", "GET");
    res.setHeader("Access-Control-Allow-Headers", "Content-Type");

    const requestedTitle = escapeHtml(req.query.q);
    var indexNotFound = true;

    // Notes
    const allNotes = [
        // EN -> ES
        [
            "ES-Travel-1",
            ["My passport is in the suitcase", "Mi pasaporte esta en la maleta", "^.*pasaporte.*maleta.*$"],
            ["I got up at 3AM!", "¡Me he levantado a las 3 de la madrugada!", "^.*levantado.*madrugada.*$"],
            ["Did you have breakfast?", "¿Has desayunado?", "^.*desayunad[oa].*$"],
            ["The train has been delayed", "El tren tiene un retraso", "^.*tren.*un retraso.*$"],
            ["They will arrive in 20 minutes", "Llegaran en 20 minutos", "^.*Llegaran.*minutos.*$"],
            ["I'm lost, can you help me?", "Estoy perdido, ¿puedes ayudarme?", "^.*perdid[oa].*ayudarme$"],
            ["Our bus is about to leave, quick!", "Nuestro autobús esta al punto de salir, ¡Rápido!", "^.*al punto de salir.*$"],
            ["Where is the bathroom?", "¿Donde están los servicios?", "^(.*servicios.*)|(.*baño.*)$"],
            ["How long is the journey?", "¿Cuanto tardará el viaje?", "^.*tardar[áa].*viaje.*$"],
            ["Suitcase", "Maleta", "^.*maleta.*$"]
        ],
        [
            "ES-Food-1",
            ["The bread", "El pan", "^.*pan.*$"],
            ["The cheese", "El queso", "^.*queso.*$"],
            ["The tuna", "El atun", "^.*atun.*$"],
            ["The noodles", "Los fideos", "^.*fideo.*$"],
            ["The vegetables", "Las verduras", "^.*verdura.*$"],
            ["The chocolate", "El chocolate", "^.*chocolate.*$"],
            ["The tea", "El té", "^.*t[eé].*$"],
            ["The milk", "La leche", "^.*leche.*$"],
            ["The yogurt", "El yogur", "^.*yogur.*$"],
            ["The soya", "La soja", "^.*soja.*$"]
        ],
        [
            "ES-Indefinite-Past-1",
            ["I often used to", "Solía"],
            ["I used to talk with him", "Yo hablaba con él"],
            ["I used to eat too much", "Yo comía demasiado"],
            ["I didn't used to read so much", "Yo no leía mucho"]
        ],
        // ES -> EN
        [
            "EN-Viajes-1",
            ["Mi pasaporte esta en la maleta", "My passport is in the suitcase", "^.*passport.*suitcase.*$"],
            ["¡Me he levantado a las 3 de la madrugada!", "I got up at 3AM!", "^.*got up.*AM.*$"],
            ["¿Has desayunado?", "Did you have breakfast?", "^.*breakfast.*$"],
            ["El tren tiene un retraso", "The train has been delayed", "^.*train.*delay.*$"],
            ["Llegaran en 20 minutos", "They will arrive in 20 minutes", "^.*arrive.*minutes.*$"],
            ["Estoy perdido, ¿puedes ayudarme?", "I'm lost, can you help me?", "^.*lost.*help$"],
            ["Nuestro autobús esta al punto de salir, ¡Rápido!", "Our bus is about to leave, quick!", "^.*about to leave.*$"],
            ["¿Donde están los servicios?", "Where is the bathroom?", "^(.*toilet.*)|(.*bathroom.*)$"],
            ["¿Cuanto tardará el viaje?", "How long is the journey?", "^.*long.*journey.*$"],
            ["Maleta", "Suitcase", "^.*suitcase.*$"]
        ],
        [
            "EN-Comida-1",
            ["El pan", "The bread", "^.*bread.*$"],
            ["El queso", "The cheese", "^.*cheese.*$"],
            ["El atun", "The tuna", "^.*tuna.*$"],
            ["Los fideos", "The noodles", "^.*noodle.*$"],
            ["Las verduras", "The vegetables", "^.*vegtable.*$"],
            ["El chocolate", "The chocolate", "^.*chocolate.*$"],
            ["El té", "The tea", "^.*tea.*$"],
            ["La leche", "The milk", "^.*milk.*$"],
            ["El yogur", "The yogurt", "^.*yogurt.*$"],
            ["La soja", "The soya", "^.*soya.*$"]
        ],
        [
            "EN-Pasado-Indefinido-1",
            ["Solía", "I often used to"],
            ["Yo hablaba con él", "I used to talk with him"],
            ["Yo comía demasiado", "I used to eat too much"],
            ["Yo no leía mucho", "I didn't used to read so much"]
        ],
        // EN -> DE
        [
            "DE-Travel-1",
            ["My passport is in the suitcase","Mein Pass ist im Koffer"],
            ["I got up at 3AM!","Ich bin um 3 Uhr morgens aufgestanden!"],
            ["Did you have breakfast?","Hast du gefrühstückt?"],
            ["The train has been delayed","Der Zug hat Verspätung"],
            ["They will arrive in 20 minutes","Sie kommen in 20 Minuten an"],
            ["I'm lost, can you help me?", "Ich habe mich verlaufen, können Sie mir helfen?"], 
            ["Our bus is about to leave, quick!", "Unser Bus fährt gleich ab, schnell! "],
            ["Where is the bathroom?","Wo ist die Toilette?"],
            ["How long is the journey?","Wie lange dauert die Fahrt?"],
            ["Suitcase","Koffer"]
        ],
        // DE -> EN
        [
            "EN-Reise-1",
            ["Mein Pass ist im Koffer", "My passport is in the suitcase"],
            ["Ich bin um 3 Uhr morgens aufgestanden!", "I got up at 3AM!"],
            ["Hast du gefrühstückt?", "Did you have breakfast?"],
            ["Der Zug hat Verspätung", "The train has been delayed"],
            ["Sie kommen in 20 Minuten an", "They will arrive in 20 minutes"],
            ["Ich habe mich verlaufen, können Sie mir helfen?", "I'm lost, can you help me?"], 
            ["Unser Bus fährt gleich ab, schnell! ", "Our bus is about to leave, quick!"],
            ["Wo ist die Toilette?", "Where is the bathroom?"],
            ["Wie lange dauert die Fahrt?", "How long is the journey?"],
            ["Koffer", "Suitcase"]

        ],
        // EN -> IE
        [
            "IE-Travel-1",
            ["My passport is in the suitcase", "Tá mo phas i mo chás"],
            ["I got up at 3AM!", "Dhúisigh mé ag a trí a chlog ar maidin!"],
            ["Did you have breakfast?", "An raibh bricfeasta agat?"],
            ["The train has been delayed", "Cuireadh moill ar an traein"],
            ["They will arrive in 20 minutes", "Beidh siad anseo i fiche nóiméad"],
            ["Our bus is about to leave, quick!", "Tá ár mbus ar tí imeacht, d'imigh leat!"],
            ["Where is the bathroom?", "Cá bhfuil an leithreas?"],
            ["How long is the journey?", "Cé comh fada is atá an turas?"],
            ["Suitcase", "Cás"]
        ]
    ]

    var i = 0;

    while (indexNotFound && i < allNotes.length) {
        if (allNotes[i][0] == requestedTitle) {
            indexNotFound = false;
        }
        else {
            i = i + 1;
        }
    }

    var foundPrompts = [];

    if (!indexNotFound) {
        foundPrompts = allNotes[i];
        foundPrompts.shift();
    }

    res.json(foundPrompts);
}