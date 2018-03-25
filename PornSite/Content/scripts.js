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
    e.getElementsByTagName("button")[0].click();
    //$("#modal-video").css("display", "flex")
    //    .hide()
    //    .slideToggle(300);
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

$('.button').click(function () {
    var buttonId = $(this).attr('id');
    $('#modal-container').removeAttr('class').addClass(buttonId);
    $('body').addClass('modal-active');
})

$('#modal-container').click(function () {
    $(this).addClass('out');
    $('body').removeClass('modal-active');
});
//$("div").mousedown(function () {
//    alert("Handler for .mousedown() called.");
//});

