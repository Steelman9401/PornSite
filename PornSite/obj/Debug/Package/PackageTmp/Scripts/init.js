function lazyLoader() {
    let options = {
        root: null, // relative to document viewport 
        rootMargin: '0px', // margin around root. Values are similar to css property. Unitless values not allowed
        threshold: 1.0 // visible amount of item shown in relation to root
    };
    let observer = new IntersectionObserver(onChange, options);
    let images = document.querySelectorAll('video');
    images.forEach(img => observer.observe(img));
}
function onChange(changes, observer) {
    changes.forEach(change => {
        if (change.intersectionRatio > 0) {
            if (change.target.getAttribute("poster") == "../Content/img/video.jpg") {
                $(change.target).fadeTo(200, 0.30, function () {
                    $(change.target).attr("poster", change.target.getAttribute("data-img"));
                }).fadeTo(200, 1);
                setTimeout(function () {
                    $(change.target).removeClass("animated fadeIn");
                }, 501);
            }
        }
    });
}
var iOS = !!navigator.platform && /iPad|iPhone|iPod/.test(navigator.platform);
var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
$(document).ready(function () {
    if (navigator.userAgent.indexOf("Firefox") != -1 && $(window).width() > 960) {
        $("#firefox-error").css("display", "block");
    }
    if (document.cookie.match(/^(.*;)?\s*History\s*=\s*[^;]+(.*)?$/)) {
        $("#cookies").css('display', 'none');
    }
    else {
        $("#cookies").css('display', 'flex');
    }

    if (!isChrome) {
        iOS = true;
    }
    $(document).on("click", "#confirm", function () {
        if ($(window).width() > 960) {
            document.getElementById("excel").src = "../Content/img/excel.png";
            var x = ($(".img-category"));
            for (var i = 0; i < 4; i++) {
                x[i].src = x[i].getAttribute("data-src");
            }
        }
        $('#modal-confirmation').css('display', 'none');
        $('main').addClass('animated fadeInUp');
        if (!iOS && isChrome && $(window).width() < 960) {
            $(".tip").css("display", "block");
        }
        setTimeout(function () {
            $("main").removeClass("animated fadeInUp");
            lazyLoader();
        }, 501);
    });
});