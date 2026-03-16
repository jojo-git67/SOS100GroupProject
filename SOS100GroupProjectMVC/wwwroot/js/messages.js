const messages = document.getElementsByClassName("messages-preview");

for(let i = 0; i < messages.length; i++){
    messages[i].addEventListener("click", async function(){
        //console.log(messages[i].id);
        //TODO: Switch to AJAX
        let inputString = "http://localhost:5282/api/Kommunication/" + messages[i].id + "/read";
        await fetch(inputString, {
            method: "PATCH"
        });
    })
}
