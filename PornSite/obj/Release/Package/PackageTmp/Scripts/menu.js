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
            active = 1;
        } else {
            if (active == 1) {
                $("#form").hide();
                active = 0;
            }
        }
    });

});