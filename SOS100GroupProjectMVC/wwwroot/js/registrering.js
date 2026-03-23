// Registrering page - JavaScript logic

// API base URL - change to Azure URL when deploying
const API_BASE_URL = "http://localhost:5041";

// Katalogtjänsten base URL - change to correct port when running locally
const KATALOG_BASE_URL = "http://localhost:5149";

// Get cookie value by name
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        const [key, value] = cookie.trim().split('=');
        if (key === name) return value;
    }
    return null;
}

// Read role and userId from cookie
const userId = getCookie("userId");
const role = getCookie("role");

// Hide all sections first
function hideAll() {
    document.getElementById("searchContainer").style.display = "none";
    document.getElementById("availableCoursesSection").style.display = "none";
    document.getElementById("myCoursesSection").style.display = "none";
    document.getElementById("hanteraSection").style.display = "none";
    document.getElementById("historikSection").style.display = "none";
}

// Show sections based on user role
function showContentByRole(role) {
    hideAll();

    if (role === "student") {
        document.getElementById("searchContainer").style.display = "block";
        document.getElementById("availableCoursesSection").style.display = "block";
        document.getElementById("myCoursesSection").style.display = "block";
        document.getElementById("myCoursesSectionTitle").style.display = "block";
        document.getElementById("adminCoursesSectionTitle").style.display = "none";
    }
    else if (role === "courseAdmin") {
        document.getElementById("myCoursesSection").style.display = "block";
        document.getElementById("hanteraSection").style.display = "block";
        document.getElementById("historikSection").style.display = "block";
        document.getElementById("myCoursesSectionTitle").style.display = "none";
        document.getElementById("adminCoursesSectionTitle").style.display = "block";
    }
    else if (role === "IT-admin") {
        document.getElementById("searchContainer").style.display = "block";
        document.getElementById("availableCoursesSection").style.display = "block";
        document.getElementById("myCoursesSection").style.display = "block";
        document.getElementById("hanteraSection").style.display = "block";
        document.getElementById("historikSection").style.display = "block";
        document.getElementById("myCoursesSectionTitle").style.display = "block";
        document.getElementById("adminCoursesSectionTitle").style.display = "none";
    }
    else if (role === "teacher") {
        document.querySelector(".page-container").innerHTML = `
            <div style="text-align: center; margin-top: 3rem;">
                <i class="fa fa-lock" style="font-size: 3rem;"></i>
                <h2>Ingen behörighet</h2>
                <p>Du har inte tillgång till denna sida.</p>
                <a href="/Home" class="btn">Gå tillbaka till startsidan</a>
            </div>
        `;
    }
}

// Search function - filters courses based on input
document.getElementById("searchInput").addEventListener("input", function() {
    const searchValue = this.value.toLowerCase();
    const courseCards = document.querySelectorAll("#availableCoursesSection .course-card");
    courseCards.forEach(card => {
        const courseName = card.querySelector("p").textContent.toLowerCase();
        if (courseName.includes(searchValue)) {
            card.style.display = "flex";
        } else {
            card.style.display = "none";
        }
    });
});

// Fetch all available courses from Katalogtjänsten
async function fetchAvailableCourses() {
    try {
        const response = await fetch(`${KATALOG_BASE_URL}/api/courses`);
        const courses = await response.json();

        const availableCoursesSection = document.getElementById("availableCoursesSection");
        const existingCards = availableCoursesSection.querySelectorAll(".course-card");
        existingCards.forEach(card => card.remove());

        courses.forEach(course => {
            const card = document.createElement("div");
            card.className = "course-card";
            card.dataset.courseId = course.courseId;
            card.innerHTML = `
                <p>${course.title}</p>
                <div class="card-buttons">
                    <button class="btn btn-register">Registrera dig</button>
                </div>
            `;

            // Add event listener to the register button
            card.querySelector(".btn-register").addEventListener("click", function() {
                registerCourse(course.courseId);
            });

            availableCoursesSection.appendChild(card);
        });

    } catch (error) {
        console.error("Error fetching courses from Katalogtjänsten:", error);
    }
}

// Fetch course title from Katalogtjänsten by courseId
async function fetchCourseTitle(courseId) {
    try {
        const response = await fetch(`${KATALOG_BASE_URL}/api/courses/${courseId}`);
        const course = await response.json();
        return course.title;
    } catch (error) {
        // If Katalogtjänsten is unavailable show courseId as fallback
        return `Kurs ID: ${courseId}`;
    }
}

// Fetch all registrations for the logged in student / IT-admin
async function fetchMyRegistrations() {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Registrering/user/${userId}`);
        const registrations = await response.json();

        const myCoursesSection = document.getElementById("myCoursesSection");
        const existingCards = myCoursesSection.querySelectorAll(".course-card");
        existingCards.forEach(card => card.remove());

        // Use for...of to allow await inside the loop
        for (const reg of registrations) {
            const card = document.createElement("div");
            card.className = "course-card";

            // Fetch course title from Katalogtjänsten
            const courseTitle = await fetchCourseTitle(reg.courseId);

            const button =
                `<button class="btn btn-remove" onclick="deleteRegistration(${reg.registreringId})">Ta bort</button>`;

            card.innerHTML = `
                <p>${courseTitle} - Status: ${reg.status}</p>
                ${button}
            `;
            myCoursesSection.appendChild(card);
        }

    } catch (error) {
        console.error("Error fetching registrations:", error);
    }
}

// Fetch courses that the logged in courseAdmin is responsible for
async function fetchAdminCourses() {
    try {
        const response = await fetch(`${KATALOG_BASE_URL}/api/courses`);
        const courses = await response.json();

        const myCoursesSection = document.getElementById("myCoursesSection");
        const existingCards = myCoursesSection.querySelectorAll(".course-card");
        existingCards.forEach(card => card.remove());

        const myCourses = courses.filter(course => course.teacherId === parseInt(userId));

        for (const course of myCourses) {
            const card = document.createElement("div");
            card.className = "course-card";

            card.innerHTML = `
                <p>${course.title} (Kurs ID: ${course.courseId})</p>
            `;

            myCoursesSection.appendChild(card);
        }

    } catch (error) {
        console.error("Error fetching admin courses:", error);
    }
}

// Fetch pending registrations for a specific course
async function fetchPendingRegistrations(courseId) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Registrering/course/${courseId}`);
        const registrations = await response.json();

        const hanteraSection = document.getElementById("hanteraSection");

        // Filter only pending registrations
        const pendingRegistrations = registrations.filter(r => r.status === "väntande");

        for (const reg of pendingRegistrations) {
            const card = document.createElement("div");
            card.className = "course-card-admin";
            card.dataset.registrationId = reg.registreringId;

            // Fetch course title from Katalogtjänsten
            const courseTitle = await fetchCourseTitle(reg.courseId);

            card.innerHTML = `
                <div class="card-info">
                    <p><strong>Student ID:</strong> ${reg.userId}</p>
                    <p><strong>Kurs:</strong> ${courseTitle} (ID: ${reg.courseId})</p>
                </div>
                <div class="card-buttons">
                    <button class="btn btn-godkann">Godkänn</button>
                    <button class="btn btn-neka">Neka</button>
                </div>
            `;

            card.querySelector(".btn-godkann").addEventListener("click", function() {
                updateStatus(reg.registreringId, "godkänd");
            });

            card.querySelector(".btn-neka").addEventListener("click", function() {
                updateStatus(reg.registreringId, "nekad");
            });

            hanteraSection.appendChild(card);
        }

    } catch (error) {
        console.error("Error fetching pending registrations:", error);
    }
}

// Fetch status history based on role
async function fetchHistorik() {
    try {
        const historikSection = document.getElementById("historikSection");
        const existingCards = historikSection.querySelectorAll(".course-card-admin");
        existingCards.forEach(card => card.remove());

        const existingParagraphs = historikSection.querySelectorAll("p");
        existingParagraphs.forEach(p => {
            if (p.textContent === "Ingen historik hittades.") {
                p.remove();
            }
        });

        let history = [];

        // Student uses history by user
        if (role === "student") {
            const response = await fetch(`${API_BASE_URL}/api/Registrering/user/${userId}/history`);
            history = await response.json();
        }

        // courseAdmin uses history by managed courses
        else if (role === "courseAdmin") {
            const coursesResponse = await fetch(`${KATALOG_BASE_URL}/api/courses`);
            const courses = await coursesResponse.json();

            const myCourses = courses.filter(course => course.teacherId === parseInt(userId));

            for (const course of myCourses) {
                const response = await fetch(`${API_BASE_URL}/api/Registrering/course/${course.courseId}/history`);
                const courseHistory = await response.json();
                history = history.concat(courseHistory);
            }
        }

        // IT-admin uses history for all courses
        else if (role === "IT-admin") {
            const coursesResponse = await fetch(`${KATALOG_BASE_URL}/api/courses`);
            const courses = await coursesResponse.json();

            for (const course of courses) {
                const response = await fetch(`${API_BASE_URL}/api/Registrering/course/${course.courseId}/history`);
                const courseHistory = await response.json();
                history = history.concat(courseHistory);
            }
        }

        if (history.length === 0) {
            const empty = document.createElement("p");
            empty.textContent = "Ingen historik hittades.";
            historikSection.appendChild(empty);
            return;
        }

        history.forEach(h => {
            const card = document.createElement("div");
            card.className = "course-card-admin";
            card.innerHTML = `
                <div class="card-info">
                    <p><strong>Registrering ID:</strong> ${h.registrationId}</p>
                    <p><strong>Från:</strong> ${h.oldStatus}</p>
                    <p><strong>Till:</strong> ${h.newStatus}</p>
                    <p><strong>Datum:</strong> ${new Date(h.changedDate).toLocaleString()}</p>
                </div>
            `;
            historikSection.appendChild(card);
        });

    } catch (error) {
        console.error("Error fetching history:", error);
    }
}

// Register user for a course via POST request
async function registerCourse(courseId) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Registrering`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                userId: parseInt(userId),
                courseId: courseId
            })
        });

        if (response.ok) {
            alert("Du är nu registrerad på kursen!");
            fetchMyRegistrations();
        } else {
            alert("Något gick fel, försök igen!");
        }
    } catch (error) {
        console.error("Error registering for course:", error);
    }
}

// Update status of a registration via PUT request
async function updateStatus(registrationId, newStatus) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Registrering/${registrationId}?newStatus=${newStatus}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.ok) {
            alert(`Registrering ${newStatus}!`);

            if (role === "courseAdmin") {
                initAdmin();
            } else if (role === "IT-admin") {
                initItAdmin();
            }

        } else {
            alert("Något gick fel, försök igen!");
        }
    } catch (error) {
        console.error("Error updating status:", error);
    }
}

// Delete a registration via DELETE request
async function deleteRegistration(registreringId) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Registrering/${registreringId}`, {
            method: "DELETE"
        });

        if (response.ok) {
            fetchMyRegistrations();
        } else {
            console.error("Could not delete registration");
        }
    } catch (error) {
        console.error("Error deleting registration:", error);
    }
}

// Fetch pending registrations and history for courseAdmin
async function initAdmin() {
    try {
        const response = await fetch(`${KATALOG_BASE_URL}/api/courses`);
        const courses = await response.json();

        const myCourses = courses.filter(course => course.teacherId === parseInt(userId));

        const hanteraSection = document.getElementById("hanteraSection");
        const existingCards = hanteraSection.querySelectorAll(".course-card-admin");
        existingCards.forEach(card => card.remove());

        for (const course of myCourses) {
            await fetchPendingRegistrations(course.courseId);
        }

        fetchHistorik();

    } catch (error) {
        console.error("Error loading admin data:", error);
    }
}

// Fetch all pending registrations and history for IT-admin
async function initItAdmin() {
    try {
        const response = await fetch(`${KATALOG_BASE_URL}/api/courses`);
        const courses = await response.json();

        const hanteraSection = document.getElementById("hanteraSection");
        const existingCards = hanteraSection.querySelectorAll(".course-card-admin");
        existingCards.forEach(card => card.remove());

        for (const course of courses) {
            await fetchPendingRegistrations(course.courseId);
        }

        fetchHistorik();

    } catch (error) {
        console.error("Error loading IT-admin data:", error);
    }
}

// Run role logic on page load
showContentByRole(role);

// Fetch data based on role
if (role === "student") {
    fetchMyRegistrations();
    fetchAvailableCourses();
}
else if (role === "courseAdmin") {
    fetchAdminCourses();
    initAdmin();
}
else if (role === "IT-admin") {
    fetchMyRegistrations();
    fetchAvailableCourses();
    initItAdmin();
}