function loadVideo(e) {
    e.load();
    e.play();
}
function playVideo(e) {
    e.load();
    e.play();
}
function stopVideo(e) {
    e.load();
    e.pause();
}

function buttonPress(e) {
    e.getElementsByTagName("button")[0].click();
    //$("#normalVideo").css("display", "flex")
    //    .hide()
    //    .slideToggle(300);
}
function universalPress(e)
{
    e.parentElement.getElementsByTagName("button")[0].click();
    e.style.fontWeight = "bold";
    if (e === document.getElementById("mostViewedLinkSearch")) {
        document.getElementById("newestLinkSearch").style.fontWeight = "normal";
    }
    else
    {
        document.getElementById("mostViewedLinkSearch").style.fontWeight = "normal";
    }

}
function pressButton(e) {
    e.children[0].click();
    $("#normalVideo").css("display", "flex")
        .hide()
        .slideToggle(200);
}

function hideVideo(e) {
    e.nextElementSibling.click();
    $("#normalVideo").slideToggle(200);
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

$("video").on("hover", function (e) {
    alert("sup");
});


