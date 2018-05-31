self.addEventListener('fetch', function (event) {
    if (event.request.url == 'https://vymasti.si/') {
        console.info('responding to dragon-server fetch with Service Worker! 🤓');
        event.respondWith(fetch(event.request).catch(function (e) {
            return caches.match('game.html');
        }));
        return;
    }
    event.respondWith(
        caches.match(event.request).then(function (response) {
            return response || fetch(event.request);
        })
    );
});
self.addEventListener('install', function (e) {
    e.waitUntil(
        caches.open('the-magic-cache').then(function (cache) {
            return cache.addAll([
                '/',
                '/Scripts/2048.js',
                '/Content/2048.css',
                '/Content/img/kara.jpg',
                '/Content/img/prcarna.mp3',
                '/game.html'
            ]);
        })
    );
});