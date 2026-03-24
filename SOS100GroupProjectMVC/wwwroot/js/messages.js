let messages = document.querySelectorAll(".messages-preview");

let titleEl = document.getElementById("selected-message-title");
let bodyEl = document.getElementById("selected-message-body");

messages.forEach(msg => {
    msg.addEventListener("click", async function () {
        
        titleEl.textContent = this.dataset.title;
        bodyEl.textContent = this.dataset.content;
        
        messages.forEach(m => m.classList.remove("active"));
        this.classList.add("active");
        
        let url = `https://app-sos100-kommunication-api.azurewebsites.net/api/Kommunication/${this.id}/read`;

        try {
            let response = await fetch(url, { method: "PATCH" });

            if (response.ok) {
                let icon = this.querySelector(".unread-icon");
                if (icon) {
                    icon.remove();
                }
            }

        } catch (err) {
            console.error(err);
        }
    });
});