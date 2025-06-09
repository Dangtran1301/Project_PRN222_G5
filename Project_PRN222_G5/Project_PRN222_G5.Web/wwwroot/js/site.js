function refreshToken() {
    fetch('/Auth/Refresh', {
        method: 'POST',
        credentials: 'include'
    })
        .then(response => {
            if (!response.ok) throw new Error('Refresh failed');
            return response.json();
        })
        .then(data => {
            document.cookie = `AccessToken=${data.AccessToken}; path=/; HttpOnly; Secure; SameSite=Strict; Expires=${new Date(Date.now() + 3600000)}`;
        })
        .catch(error => {
            console.error('Refresh failed:', error);
            window.location.href = '/Auth/Login';
        });
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

setInterval(checkAndRefreshToken, 60000 * 15);

function previewImage(input, previewId) {
    const file = input.files[0];
    const img = document.getElementById(previewId);

    if (file && img) {
        const reader = new FileReader();
        reader.onload = function (e) {
            img.src = e.target.result;
            img.style.display = 'block';      // Hiện ảnh
            img.classList.remove('d-none');   // Xóa class ẩn nếu có
        };
        reader.readAsDataURL(file);
    }
}