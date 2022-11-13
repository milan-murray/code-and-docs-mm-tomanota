function createJson() {
    const inputs = document.getElementsByTagName("input");
    var promptsObj = {};
    promptsObj.prompts = [];

    if (inputs.length > 0) {
        promptsObj.title = inputs[0].value;
        for (let i = 1; i < inputs.length; i = i + 2) {
            promptsObj.prompts.push({ 'heading': inputs[i].value, 'body': inputs[i + 1].value, 'score' : 0 });
        }
        console.log(promptsObj);
    }
}

document.getElementById("create_json").addEventListener('click', createJson);

var num_prompts = 0;

$(function () {
    $("#add_prompts").click(function (e) {
        num_prompts += 1;
        e.preventDefault();
        $("#fieldList").append(`<h3>Prompt ${num_prompts + 1}</h3>`);
        $("#fieldList").append(`<li><label for='header_${num_prompts}'>Heading</label></li>`);
        $("#fieldList").append(`<li><input autocomplete='off' type='text' name='header_${num_prompts}' id='header_${num_prompts}'></li>`);
        $("#fieldList").append(`<li><label for='body_${num_prompts}'>Body</label></li>`);
        $("#fieldList").append(`<li><input autocomplete='off' type='text' name='body_${num_prompts}' id='body_${num_prompts}'><li>`);
        if (num_prompts > 13) {
            document.getElementById("add_prompts").disabled = true;
        }
    });
});

$(document).ready(function () {
    $("#fieldList").append(`<h3>Prompt ${num_prompts + 1}</h3>`);
    $("#fieldList").append(`<li><label for='header_${num_prompts}'>Heading</label></li>`);
    $("#fieldList").append(`<li><input autocomplete='off' type='text' name='header_${num_prompts}' id='header_${num_prompts}'></li>`);
    $("#fieldList").append(`<li><label for='body_${num_prompts}'>Body</label></li>`);
    $("#fieldList").append(`<li><input autocomplete='off' type='text' name='body_${num_prompts}' id='body_${num_prompts}'><li>`);
})

document.getElementById("create_json").disabled = false;
