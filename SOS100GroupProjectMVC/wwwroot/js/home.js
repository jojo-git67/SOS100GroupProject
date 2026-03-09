// Visa funktioner beroende på roll

// Hämta cookie värde med namn
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        const [key, value] = cookie.trim().split('=');
        if (key === name) return value;
    }
    return null;
}

// Läs roll från cookie
// OBS: "student" är temporär standard för testning, tas bort när login är klar
const role = getCookie("role") || "student";

// Göm alla sektioner först
function hideAll() {
    document.getElementById("notiserSection").style.display = "none";
    document.getElementById("kursSection").style.display = "none";
    document.getElementById("schemaSection").style.display = "none";
}

// Visa sektioner baserat på roll:
// student: notiser, kurser, schema
// teacher: notiser, schema
// courseAdmin / IT-admin: notiser, kurser, schema
function showContentByRole(role) {
    hideAll();

    if (role === "student") {
        document.getElementById("notiserSection").style.display = "block";
        document.getElementById("kursSection").style.display = "block";
        document.getElementById("schemaSection").style.display = "block";
    }
    else if (role === "teacher") {
        document.getElementById("notiserSection").style.display = "block";
        document.getElementById("schemaSection").style.display = "block";
    }
    else if (role === "courseAdmin" || role === "IT-admin") {
        document.getElementById("notiserSection").style.display = "block";
        document.getElementById("kursSection").style.display = "block";
        document.getElementById("schemaSection").style.display = "block";
    }
}

// TODO: Ersätt cookie med riktig inloggningssession när den är klar
showContentByRole(role);