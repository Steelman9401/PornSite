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
    $(document).on("click", "#install", function (e) {
        // hide our user interface that shows our A2HS button
        $("#pwa").css("display", "none");
        // Show the prompt
        deferredPrompt.prompt();
        // Wait for the user to respond to the prompt
        deferredPrompt.userChoice
            .then((choiceResult) => {
                if (choiceResult.outcome === 'accepted') {
                    console.log('User accepted the A2HS prompt');
                } else {
                    console.log('User dismissed the A2HS prompt');
                }
                deferredPrompt = null;
            });
    });
    $(document).keypress(function (e) {
        if (!$("#search-textbox").is(':focus')) {
            if ($('#frame').length) {
                if (document.getElementById("frame").src != "about:blank")
                    url = document.getElementById("frame").src;
                document.getElementById("frame").src = "https://vymasti.si/excelFull.html"
            }
            $("body").addClass("modal-on");
            goFullScreen($("#hide-page").get(0));
            document.getElementById("hide-page").style.display = "block";
        }
    });

    $(document).on("click", ".click-link", function (e) {
        $("body").removeClass("modal-on");
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
        $("body").removeClass("modal-on");
        $("#hide-page").css("display", "none");
        document.getElementById("frame").src = url;
        if (document.exitFullscreen)
            document.exitFullscreen();
        else if (document.mozCancelFullScreen)
            document.mozCancelFullScreen();
        else if (document.webkitExitFullscreen)
            document.webkitExitFullscreen();
        else if (document.msExitFullscreen)
            document.msExitFullscreen();
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
            goFullScreen($("#hide-page").get(0));
            url = document.getElementById("frame").src;
            document.getElementById("frame").src = "";
        }
        $("body").addClass("modal-on");
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
        $('html,body').animate({ scrollTop: 0 }, 'fast');
        if ($(window).width() < 744) {
            $("#form").slideToggle()
            $("#menu").slideToggle();
        }
        $("#search-dotvvm").click();
    });

    function goFullScreen(element) {
          if (element.requestFullscreen)
                element.requestFullscreen();
            else if (element.mozRequestFullScreen)
                element.mozRequestFullScreen();
            else if (element.webkitRequestFullscreen)
                element.webkitRequestFullscreen();
            else if (element.msRequestFullscreen)
                element.msRequestFullscreen();
    }
});


