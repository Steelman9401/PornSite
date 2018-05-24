var loaded;
$(document).ready(function () {
    var touchmoved;
    var lastValueScroll = 0;
    var previewStarted;
    //kliknuti na nahled videa
    $(document).on("touchend click", ".video", function (e) {
        if (touchmoved != true) {
            if (previewStarted) {
                hideVideo(e.currentTarget.getElementsByTagName("video")[0]);
                previewStarted = false;
            }
            else {
                disableLoader = true;
                $(".video-load").css("display", "none");
                e.currentTarget.getElementsByTagName("button")[0].click();
                loaded = true;
                $("#modal-video").css('display', 'flex');
                $("#modal-video").removeClass("animated fadeOutUp");
                if ($(window).width() < 760) {
                    $("header").css('display', 'none');
                }
                setTimeout(function () { $("#modal-video").removeClass("animated fadeInDown"); }, 501);
                $("#modal-video").addClass("animated fadeInDown");
                $("#background-modal").fadeIn();
                $(".comments").hide();
                $(".related-videos-container").hide();
            }
        }
    }).on('touchmove', ".video", function (e) {
        touchmoved = true;
        if (previewStarted && !iOS) {
            var scrollFromTop = $(document).scrollTop();
            var difference = Math.abs(scrollFromTop - lastValueScroll);
            if (difference > 100) {
                lastValueScroll = scrollFromTop;
                hideVideo(e.currentTarget.getElementsByTagName("video")[0]);
                previewStarted = false;
            }
        }
    }).on('touchstart', ".video", function (e) {
        touchmoved = false;
        if (!iOS) {
            var scrollFromTop = $(document).scrollTop();
            $(".video-load").css("display", "block");
            setTimeout(function () {
                var difference = Math.abs(scrollFromTop - $(document).scrollTop());
                if (!loaded && difference < 50) {
                    hoverVideo(e.currentTarget.getElementsByTagName("video")[0]);
                    previewStarted = true;
                }
            }, 1000);
        }
        });

    //kliknuti na krizek
    $(document).on("click", "#close", function () {
        $("body").removeClass("modal-on");
        $("#close-btn").click();
        $("header").css('display', 'block');
        $("#loader-modal").css("display", "none");
        $("body").removeClass("modal-on");
        $("#modal-video").addClass("animated fadeOutUp");
        $("#background-modal").fadeOut();
        setTimeout(function () { loaded = false; }, 501);
        disableLoader = false;
    });

    //kliknuti mimo modal
    $(document).on("click", "#modal-video", function (e) {
        if (!$(e.target).parents("#modal-video").length) {
            e.stopPropagation();
            $("#close-btn").click();
            $("header").css('display', 'block');
            $("#loader-modal").css("display", "none");
            $("body").removeClass("modal-on");
            $("#modal-video").addClass("animated fadeOutUp");
            $("#background-modal").fadeOut();
            setTimeout(function () { loaded = false; }, 501);
            disableLoader = false;
        }
    });
});