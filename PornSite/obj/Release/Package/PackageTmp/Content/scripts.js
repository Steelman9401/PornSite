function loadVideo(e) {
    e.play();
}
function playVideo(e) {
    e.play();
}
function stopVideo(e) {
    e.load();
    e.pause();
}
function buttonPress(e) {
    e.nextElementSibling.click();
}
function pressButton(e) {
    e.children[0].click();
    $("#modal-video").css("display", "flex")
        .hide()
        .slideToggle(200);
}

function hideVideo(e) {
    e.nextElementSibling.click();
    $("#modal-video").slideToggle(200);
}

