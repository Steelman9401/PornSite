$(document).ready(function () {
    $('.loader').fadeOut();
    $("#holdTest").bind("taphold", function () {
        // Actions
        alert("sup");
    });

    // or

    $("#holdTest").on("taphold", function () {
        // Actions
        alert("sup");
    });
    //kliknuti na nahled videa
    $(document).on("click", ".video", function () {
        $("#modal-video").css('display', 'flex');
        $("#modal-video").removeClass("animated slideOutUp");
        $("#backgroundO").fadeIn();
        $("#modal-video").addClass("animated slideInDown");
        $("body").addClass("modal-on");
        $(".comments").hide();
        $(".related-videos-container").hide();
    });

    //kliknuti na krizek
    $(document).on("click", "#close", function () {
        
       // $("#modal-video").hide();
        $("#backgroundO").fadeOut(500);
        $("#modal-video").addClass("animated slideOutUp");
        document.getElementById("closeButton").click();
        $("body").removeClass("modal-on");
    });

    //kliknuti mimo modal
    $(document).on("click", "#modal-video", function (e) {
        if (!$(e.target).parents("#modal-video").length) {
            document.getElementById("closeButton").click();
            e.stopPropagation();
            //window.history.pushState('main', null);
            //$("#modal-video").hide();
            $("#modal-video").addClass("animated slideOutUp");
            $("#backgroundO").fadeOut(500);
            $("body").removeClass("modal-on");
        }
    });
});
