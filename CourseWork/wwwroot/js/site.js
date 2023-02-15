const storedTheme = localStorage.getItem('theme');

const getPreferredTheme = () => {
    if (storedTheme) {
        return storedTheme
    }

    const theme = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    localStorage.setItem("theme", theme);
    return theme;
}

const setTheme = function (theme) {
    if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        document.documentElement.setAttribute('data-bs-theme', 'dark');
    } else {
        document.documentElement.setAttribute('data-bs-theme', theme);
    }
}

setTheme(getPreferredTheme());

const showActiveTheme = theme => {
    if (theme == "dark") {
        document.querySelector("body").classList.add("dark-mode");
        document.getElementById("theme").className = "fa-solid fa-sun icon-white";
        document.querySelectorAll("i").forEach(x => {
            x.classList.add("icon-white");
        });
    } else {
        document.querySelector("body").classList.remove("dark-mode");
        document.getElementById("theme").className = "fa-solid fa-moon";
        document.querySelectorAll("i").forEach(x => {
            x.classList.remove("icon-white");
        });
    }
}

window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
    if (storedTheme !== 'light' || storedTheme !== 'dark') {
        setTheme(getPreferredTheme())
    }
})

window.addEventListener('DOMContentLoaded', () => {
    showActiveTheme(getPreferredTheme());
    document.getElementById("theme").onclick = (e) => {
        const theme = localStorage.getItem("theme") == "dark" ? "light" : "dark";
        localStorage.setItem("theme", theme);
        setTheme(theme);
        showActiveTheme(theme);
    }
})
