$(document).ready(function () {
    $('.loader').fadeOut();

    //kliknuti na nahled videa
    $(document).on("click", ".video", function () {
        $("#modal-video").css('display', 'flex');
        $("#backgroundO").fadeIn();
        $("#modal-video").addClass("animated bounceInUp");
        $("body").addClass("modal-on");
        $(".comments").hide();
        $(".related-videos-container").hide();
    });

    //kliknuti na krizek
    $(document).on("click", "#close", function () {
        
        $("#modal-video").hide();
        $("#backgroundO").fadeOut(500);
        $("body").removeClass("modal-on");
    });

    //kliknuti mimo modal
    $(document).on("click", "#modal-video", function (e) {
        if (!$(e.target).parents("#modal-video").length) {
            document.getElementById("closeButton").click();
            e.stopPropagation();
            $("#modal-video").hide();
            $("#backgroundO").fadeOut(500);
            $("body").removeClass("modal-on");
        }
    });
});