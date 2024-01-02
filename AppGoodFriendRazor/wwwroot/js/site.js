// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    document.addEventListener('DOMContentLoaded', function() {
        var themeButtons = document.querySelectorAll('[data-bs-theme-value]');
    themeButtons.forEach(function(btn) {
        btn.addEventListener('click', function () {
            var theme = btn.getAttribute('data-bs-theme-value');
            switchTheme(theme);
        });
        });
    });

    function switchTheme(theme) {
        if (theme === 'light') {
        // Apply light theme
        document.body.classList.add('light-mode');
    document.body.classList.remove('dark-mode');
        } else if (theme === 'dark') {
        // Apply dark theme
        document.body.classList.add('dark-mode');
    document.body.classList.remove('light-mode');
        } else {
        // Apply auto or default theme
        document.body.classList.remove('dark-mode', 'light-mode');
        }
    }

