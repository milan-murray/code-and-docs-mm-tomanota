const form = document.getElementById('prompt_area');

function handleSubmit(event) {
    event.preventDefault();

    const data = new FormData(event.target);

    const value = Object.fromEntries(data.entries());

    console.log({ value });
}

var num_prompts = 1;

function addPropmt() {
    num_prompts += 1;
    document.getElementById('prompt_area').innerHTML += `<h4>Prompt ${num_prompts}</h4><hr><label for='header_${num_prompts}'>Heading</label><input autocomplete='off' type='text' name='header_${num_prompts}' id='header_${num_prompts}'><br><label for='body_${num_prompts}'>Body</label><input autocomplete='off' type='text' name='body_${num_prompts}' id='body_${num_prompts}'><br>`;
}

document.getElementById("add_prompts").addEventListener('click', addPropmt);

form.addEventListener('submit', handleSubmit);