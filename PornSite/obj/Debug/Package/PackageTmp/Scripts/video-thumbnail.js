var check = 0;
var video = null;
var videoPlaying;
function hoverVideo(e) {
    if (!videoPlaying) {
        e.src = e.getAttribute("data-src");
        videoPlaying = true;
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
        e.pause();
        e.load();
        e.src = "";
        videoPlaying = false;
        e.parentElement.getElementsByClassName("preview-text")[0].style.display = "flex";
        if (check == 0) {
            e.parentElement.getElementsByClassName("today")[0].style.display = "flex";
        }
        else {
            check = 0;
        }
}

$(document).ready(function () {
    $("#category").on("taphold", function () {
        alert("hello");
    });                       
});


