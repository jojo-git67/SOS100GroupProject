// Hämta cookie värde med namn

function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        const [key, value] = cookie.trim().split('=');
        if (key === name) return value;
    }
    return null;
}

// Läs roll från cookie, standard är student
const role = getCookie("role") || "student";

// Visa/göm admin-knappar baserat på roll
function applyRoleAccess() {
    const adminButtons = document.querySelectorAll(".admin-only");
    adminButtons.forEach(btn => {
        if (role === "courseAdmin" || role === "IT-admin") {
            btn.style.display = "inline-block";
        } else {
            btn.style.display = "none";
        }
    });
}

// Sökfunktion
function searchCourses() {
    const input = document.getElementById("searchInput").value.toLowerCase();
    const cards = document.querySelectorAll(".course-card");
    cards.forEach(card => {
        const title = card.querySelector("h2").textContent.toLowerCase();
        card.style.display = title.includes(input) ? "block" : "none";
    });
}

applyRoleAccess();