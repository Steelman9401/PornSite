function playVideo(e) {
    e.play();
}
function stopVideo(e) {
    e.load();
    e.pause();
}
function pressButton(e) {
    e.children[0].click();
}