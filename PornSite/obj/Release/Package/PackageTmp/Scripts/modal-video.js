$(document).ready(function () {

    //kliknuti na btn komentare
    $(document).on("click", "#comments", function () {
        $(".related-videos-container").hide();
        $(".comments").show();
        $('.modal-body').animate({
            scrollTop: $(".comments").offset().top
        }, 1000)
    });

    //kliknuti na btn podobna videa
    $(document).on("click", "#related", function () {
        $(".comments").hide();
        $(".related-videos-container").show();
        $('.modal-body').animate({
            scrollTop: $(".related-videos-container").offset().top
        }, 1000)
    });
});