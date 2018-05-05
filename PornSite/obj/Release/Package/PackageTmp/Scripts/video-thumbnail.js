var check = 0;
function hoverVideo(e) {
    if ($(window).width() > 1149) {
        e.src = e.getAttribute("data-src");
        $(e).get(0).load();
        $(e).get(0).play(); 
        e.parentElement.getElementsByClassName("preview-text")[0].style.display = "none";
        var x = e.parentElement.getElementsByClassName("today")[0];
        if (x.style.display == "none") {
            check = 1;
        }
        else {
            x.style.display = "none";
        }
    }
    }

function hideVideo(e) {
    if ($(window).width() > 1149) {
        e.pause();
        e.load();
        e.src = "";
        e.parentElement.getElementsByClassName("preview-text")[0].style.display = "flex";
        if (check == 0) {
            e.parentElement.getElementsByClassName("today")[0].style.display = "flex";
        }
        else {
            check = 0;
        }
    }
    }