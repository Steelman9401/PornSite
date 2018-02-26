function playVideo(e) {
    e.play();
}
function stopVideo(e) {
    e.pause();
}
function pressButton(e) {
    e.children[0].click();
    $("#modal").fadeIn(500);
}

function hideVideo(e) {
    e.nextElementSibling.click();
    $("#modal").fadeOut(500);
}
