var loaded;
$(document).ready(function () {
    //kliknuti na nahled videa
    $(document).on("click", ".video", function () {
        loaded = true;
        $("#modal-video").css('display', 'flex');
        $("#modal-video").removeClass("animated fadeOutUp");
        if ($(window).width() < 760) {
            $("header").css('display', 'none');
        }
        setTimeout(function () { $("#modal-video").removeClass("animated fadeInDown"); }, 501);
        $("#modal-video").addClass("animated fadeInDown");
        $("#background-modal").fadeIn();
        //$("body").addClass("modal-on");
        $(".comments").hide();
        $(".related-videos-container").hide();
    });

    //kliknuti na krizek
    $(document).on("click", "#close", function () {
        $("body").removeClass("modal-on");
        //$("#background-modal").fadeOut();
        $("#close-btn").click();
        $("header").css('display', 'block');
        $("#loader-main").css("display", "none");
        $("#loader-modal").css("display", "none");
        $("body").removeClass("modal-on");
        $("#modal-video").addClass("animated fadeOutUp");
        $("#background-modal").fadeOut();
        setTimeout(function () { loaded = false; }, 501);
    });

    //kliknuti mimo modal
    $(document).on("click", "#modal-video", function (e) {
        if (!$(e.target).parents("#modal-video").length) {
            e.stopPropagation();
            $("#close-btn").click();
            $("#loader-main").css("display", "none");
            $("header").css('display', 'block');
            $("#loader-modal").css("display", "none");
            $("body").removeClass("modal-on");
            $("#modal-video").addClass("animated fadeOutUp");
            $("#background-modal").fadeOut();
            setTimeout(function () { loaded = false; }, 501);
        }
    });
});