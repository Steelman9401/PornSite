$(document).ready(function () {

    //kliknuti na btn podobna videa
    $(document).on("click", ".related", function () {
        $(".related-videos-container").show();
        $('.modal-body').animate({
            scrollTop: $(".related-videos-container").offset().top
        }, 1000)
    });

    //kliknuti na podobne video
    $(document).on("click", ".related-video", function () {
        $(".related-videos-container").hide();
        $('.modal-body').animate({
            scrollTop: $(".modal-body").offset().top
        }, 1000)
    });
});