$(document).ready(function () {
    //kliknuti na nahled videa
    $(document).on("click", ".video", function () {

        $("#normalVideo").css('display', 'flex');
        $("#normalVideo").removeClass("animated slideOutUp");
        $("#backgroundO").fadeIn();
        $("#normalVideo").addClass("animated slideInDown");
        $("body").addClass("modal-on");
        $(".comments").hide();
        $(".related-videos-container").hide();
    });

    $(document).on("click", ".admin", function () {
        $("#myModal").modal();
    });

    //kliknuti na krizek
    $(document).on("click", "#close", function () {
        
       // $("#normalVideo").hide();
        $("#backgroundO").fadeOut(500);
        $("#normalVideo").addClass("animated slideOutUp");
        document.getElementById("closeButton").click();
        $("body").removeClass("modal-on");
    });

    //kliknuti mimo modal
    $(document).on("click", ".modal-video", function (e) {
        if (!$(e.target).parents(".modal-video").length) {
            e.target.getElementsByClassName("close-button")[0].click();
            e.stopPropagation();
            //window.history.pushState('main', null);
            //$("#normalVideo").hide();
            $(".modal-video").addClass("animated slideOutUp");
            $("#backgroundO").fadeOut(500);
            $("body").removeClass("modal-on");
        }
    });
});
