// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function refreshToken() {
    fetch('/api/auth/refresh', {
        method: 'POST',
        credentials: 'include'
    })
        .then(response => response.json())
        .then(data => {
            document.cookie = `AccessToken=${data.AccessToken}; path=/; HttpOnly; Secure; SameSite=Strict; Expires=${new Date(Date.now() + 3600000)}`;
        })
        .catch(error => console.error('Refresh failed:', error));
}

function checkAndRefreshToken() {
    const token = document.cookie.split('; ').find(row => row.startsWith('AccessToken='))?.split('=')[1];
    if (token) {
        const payload = JSON.parse(atob(token.split('.')[1]));
        if (payload.exp < Date.now() / 1000) {
            refreshToken();
        }
    }
}