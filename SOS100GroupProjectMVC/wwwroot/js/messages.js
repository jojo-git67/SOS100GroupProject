let messages = document.querySelectorAll(".messages-preview");

messages.forEach(message => {
    message.addEventListener("click", async function () {

        let url = `http://localhost:5282/api/Kommunication/${this.id}/read`;

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