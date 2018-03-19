$(document).ready(function () {
    $("#menu-icon").click(function () {
        $("#form").slideToggle(200);
        $("#menu").slideToggle(200);
    });

    var active = 0;
    $(window).on('resize', function () {
        if ($(window).width() > 744) {
            $("#menu").hide();
            $("#form").show();
            active = 0;
        } else {
            if (!active) {
                $("#form").hide();
                active = 1;
            }
        }
    });

});