function lazyLoader() {
    let options = {
        root: null, // relative to document viewport 
        rootMargin: '0px', // margin around root. Values are similar to css property. Unitless values not allowed
        threshold: 0.3 // visible amount of item shown in relation to root
    };
    let observer = new IntersectionObserver(onChange, options);
    let images = document.querySelectorAll('video');
    images.forEach(img => observer.observe(img));
}
function onChange(changes, observer) {
    changes.forEach(change => {
        if (change.intersectionRatio > 0) {
            if (change.target.getAttribute("poster") == "../Content/img/video.jpg") {
                var img = new Image();
                img.src = change.target.getAttribute('data-img');
                img.onload = function () {
                    $(change.target).fadeOut(250, function () {
                        change.target.setAttribute("poster", img.src);
                        $(change.target).fadeIn(250);
                    });
                }
            }
        }
    });
}
var iOS = !!navigator.platform && /iPad|iPhone|iPod/.test(navigator.platform);
var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
$(document).ready(function () {
    navigator.serviceWorker && navigator.serviceWorker.register('./scripts/sw.js').then(function (registration) {
        console.log('Excellent, registered with scope: ', registration.scope);
    });
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
        if ($(window).width() > 760) {
            var x = ($(".img-category"));
            for (var i = 0; i < 4; i++) {
                x[i].src = x[i].getAttribute("data-src");
            }
        }
        if ($(window).width() > 1149) {
            document.getElementById("excel").src = "../Content/img/excel.png";
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