self.addEventListener('push', event => {
    const payload = event.data ? event.data.text() : 'New Request Received';
    
    const options = {
        body: payload,
        icon: 'icon-192.png', // Ensure this file exists in wwwroot
        vibrate: [100, 50, 100],
        data: {
            dateOfArrival: Date.now(),
            primaryKey: 1
        },
        actions: [
            { action: 'view', title: 'Open App' },
            { action: 'close', title: 'Dismiss' }
        ]
    };

    event.waitUntil(
        self.registration.showNotification('Manager Alert', options)
    );
});