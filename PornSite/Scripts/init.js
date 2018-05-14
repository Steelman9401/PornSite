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

    if (!iOS && isChrome && $(window).width() < 960) {
        $(".tip").css("display", "block");
    }
    if (!isChrome) {
        iOS = true;
    }
});