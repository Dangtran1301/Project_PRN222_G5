async function fetchWithAutoRefresh(url, options = {}, retry = true) {
    const response = await fetch(url, {
        ...options,
        credentials: 'include'
    });

    if (response.status === 401 && retry) {
        const refreshResult = await tryRefreshToken();

        if (refreshResult) {
            return fetchWithAutoRefresh(url, options, false);
        } else {
            window.location.href = '/Auth/Login';
            return null;
        }
    }

    return response;
}

async function tryRefreshToken() {
    try {
        const response = await fetch('/Auth/RefreshToken', {
            method: 'POST',
            credentials: 'include'
        });

        if (!response.ok) throw new Error("Refresh failed");
        return true;
    } catch (e) {
        console.error("Refresh token failed", e);
        return false;
    }
}