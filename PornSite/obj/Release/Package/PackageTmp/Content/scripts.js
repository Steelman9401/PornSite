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

$(document).ready(function () {
    $("#menu-icon").click(function () {
        $("#form").slideToggle(200);
        $("#menu").slideToggle(200);
    });
});