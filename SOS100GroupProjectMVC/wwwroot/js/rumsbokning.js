document.addEventListener("DOMContentLoaded", function () {
    const bookingForm = document.querySelector('form[asp-action="CreateBooking"], form[action*="CreateBooking"]');
    const deleteForms = document.querySelectorAll('form[asp-action="DeleteBooking"], form[action*="DeleteBooking"]');
    const messageBoxes = document.querySelectorAll(".message-box");

    // Dölj meddelanden efter några sekunder
    messageBoxes.forEach(function (messageBox) {
        setTimeout(function () {
            messageBox.style.transition = "opacity 0.5s ease";
            messageBox.style.opacity = "0";

            setTimeout(function () {
                messageBox.remove();
            }, 500);
        }, 4000);
    });

    // Validering för skapa bokning
    if (bookingForm) {
        bookingForm.addEventListener("submit", function (e) {
            const roomField = bookingForm.querySelector('[name="RoomId"]');
            const dateField = bookingForm.querySelector('[name="Date"]');
            const startTimeField = bookingForm.querySelector('[name="StartTime"]');
            const endTimeField = bookingForm.querySelector('[name="EndTime"]');
            const submitButton = bookingForm.querySelector('button[type="submit"]');

            const roomId = roomField ? roomField.value.trim() : "";
            const dateValue = dateField ? dateField.value : "";
            const startTimeValue = startTimeField ? startTimeField.value : "";
            const endTimeValue = endTimeField ? endTimeField.value : "";

            if (!roomId) {
                e.preventDefault();
                alert("Du måste välja ett rum.");
                return;
            }

            if (!dateValue) {
                e.preventDefault();
                alert("Du måste välja ett datum.");
                return;
            }

            if (!startTimeValue || !endTimeValue) {
                e.preventDefault();
                alert("Du måste välja både starttid och sluttid.");
                return;
            }

            if (endTimeValue <= startTimeValue) {
                e.preventDefault();
                alert("Sluttiden måste vara senare än starttiden.");
                return;
            }

            // Hindra dubbelklick
            if (submitButton) {
                submitButton.disabled = true;
                submitButton.textContent = "Bokar...";
            }
        });
    }

    // Bekräftelse vid avbokning
    deleteForms.forEach(function (deleteForm) {
        deleteForm.addEventListener("submit", function (e) {
            const confirmed = confirm("Är du säker på att du vill avboka bokningen?");

            if (!confirmed) {
                e.preventDefault();
                return;
            }

            const deleteButton = deleteForm.querySelector('button[type="submit"]');
            if (deleteButton) {
                deleteButton.disabled = true;
                deleteButton.textContent = "Avbokar...";
            }
        });
    });
});