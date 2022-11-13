let form = document.getElementById("upload");
let file = document.getElementById("file");
let quiz_area = document.getElementById("quiz-container");
let upload_area = document.getElementById("upload-container");
let submit_btn = document.getElementById("submit-answer");
let docTitle = document.getElementById("doc-title");
let userResponse = document.getElementById("responseText");
let guideText = document.getElementById("incorrect-guide");

form.addEventListener("submit", handleSubmit);

function handleSubmit(e) {
    e.preventDefault();

    if (!file.value.length) {
        console.log("No file uploaded");
        return;
    }

    quiz_area.style.display = "";
    upload_area.style.display = "none";

    let reader = new FileReader();

    reader.onload = logFile;

    reader.readAsText(file.files[0]);
}

function logFile(e) {
    let str = e.target.result;
    json = JSON.parse(str);
    // console.log('string', str);
    console.log('json', json);
    beginQuiz();
}

function checkCorrectResponse() {
    if (userResponse.value == json.prompts[currentPrompt-1].body) {
        userResponse.value = "";
        return true;
    }
    console.log("Incorrect response");
    return false;
}

function firstPrompt() {
    if (currentPrompt < json.prompts.length) {
        diplayPrompt(json.prompts);
        currentPrompt++;
    } else {
        docTitle.innerText = "Quiz complete!"
    }
}

function nextPrompt() {
    if (currentPrompt < json.prompts.length) {
        if (checkCorrectResponse()) {
            diplayPrompt(json.prompts);
            currentPrompt++;
            guideText.innerText = "";
        } else {
            guideText.innerText = json.prompts[currentPrompt-1].body;
        }
    } else {
        docTitle.innerText = "Quiz complete!"
    }
}

function diplayPrompt(promptIn) {
    console.log("displaying prompt");
    document.getElementById("header").innerText = promptIn[currentPrompt].heading;
}

let currentPrompt = 0;

function beginQuiz() {
    console.log("Quiz started");

    docTitle.innerText = json.title;

    firstPrompt();
}

submit_btn.addEventListener('click', nextPrompt);
