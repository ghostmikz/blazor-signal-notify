// Listen for the push event from the server
self.addEventListener('push', event => {
    const payload = event.data ? event.data.text() : 'No payload';
    
    event.waitUntil(
        self.registration.showNotification('Server Counter', {
            body: payload,
            icon: 'favicon.png',
            vibrate: [100, 50, 100],
            data: { url: '/' }
        })
    );
});

self.addEventListener('notificationclick', event => {
    event.notification.close();
    event.waitUntil(clients.openWindow(event.notification.data.url));
});