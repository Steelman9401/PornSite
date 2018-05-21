var showLoader;
var disableLoader;
$(document).ready(function () {
    var url;
    var myVar = setInterval(myTimer, 800);
    function myTimer() {
        if ($(window).width() > 1000) {
            if ($("#modal-login").css("display") == "flex") {
                if ($("#blinking-text").css("display") == "none") {
                    $("#blinking-text").show();
                }
                else {
                    $("#blinking-text").hide();
                }
            }
            if (loaded)
                window.focus();
        }

    }
    $(document).on("click", ".click", function (e) {
        e.currentTarget.getElementsByTagName("button")[0].click();
    });

    $(document).keypress(function (e) {
        if (!$("#search-textbox").is(':focus')) {
            if ($('#frame').length) {
                if (document.getElementById("frame").src != "about:blank")
                    url = document.getElementById("frame").src;
                document.getElementById("frame").src = "about:blank"
            }
            document.getElementById("potter").src = "https://www.youtube.com/embed/mITHDj6-DSc?start=48";
            document.getElementById("hide-page").style.display = "block";
        }
    });

    $(document).on("click", ".click-link", function (e) {
        e.currentTarget.getElementsByTagName("a")[0].click();
        $("#categories-preview").hide();
    });
    $(document).on("click", ".click-link-mobile", function (e) {
        e.currentTarget.getElementsByTagName("a")[0].click();
        $('html,body').animate({ scrollTop: 0 }, 'fast');
        $("#form").hide();
        $("#menu").hide();
    });
    $(document).on("click", ".remove-loader-related", function (e) {
        $("#loader-related").css("display", "none");
    });
    $(document).on("click", "#safety-info", function (e) {
        $("#modal-login").css("display", "flex");
    });
    $(document).on("click", "#safe-button", function (e) {
        $("#hide-page").css("display", "none");
        document.getElementById("potter").src = "";
        document.getElementById("frame").src = url;
    });

    $(document).on("click", ".show-header", function (e) {
        $("header").css("display", "block");
        setTimeout(function () { loaded = false; }, 1000);
    });

    $(document).on("click", ".scroll-top", function (e) {
        $('html,body').animate({ scrollTop: 0 }, 'fast');
    });

    $(document).on("click", ".remove-loader-modal", function (e) {
        $("#loader-modal").css("display", "none");
    });
    $(document).on("click", ".hide-modal", function () {
        $("#loader-modal").css("display", "none");
        $("#modal-video").addClass("animated fadeOutUp");
        $("#background-modal").fadeOut();
    });
    $(document).on("click", "#most-viewed", function (e) {
        $(e.currentTarget).addClass("active-li");
        $("#newest").removeClass("active-li");
    });
    $(document).on("click", "#newest", function (e) {
        $(e.currentTarget).addClass("active-li");
        $("#most-viewed").removeClass("active-li");
    });
    $(document).on("click", "#safety-button", function (e) {
        if ($('#frame').length) {
            url = document.getElementById("frame").src;
            document.getElementById("frame").src = "";
        }
        document.getElementById("potter").src = "https://www.youtube.com/embed/mITHDj6-DSc?start=48"
        document.getElementById("hide-page").style.display = "block";
    });
    $(document).on("click", "li", function (e) {
        if (!$(e.currentTarget).hasClass("no-scroll")) {
            e.currentTarget.getElementsByTagName("a")[0].click();
            setTimeout(function () { page = false; }, 1000);
            $('header').removeClass('animated slideOutUp');
            $('header').addClass("animated slideInDown")
            $('html, body').scrollTop(0);
        }
    });

    $(document).on("click", "#search", function (e) {
        disableLoader = true;
        $('html,body').animate({ scrollTop: 0 }, 'fast');
        if ($(window).width() < 744) {
            $("#form").slideToggle()
            $("#menu").slideToggle();
        }
        $("#search-dotvvm").click();
    });
});


