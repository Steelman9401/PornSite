$(document).ready(function () {

    //kliknuti na "prihlasit se"
    $(document).on("click", ".login, .menu-login", function () {
        $("#modal-login").css('display', 'flex');
        $("body").addClass("modal-on");
    });

    //kliknuti na krizek
    $(document).on("click", "#close-log", function () {
        $("#modal-login").hide();
        $("body").removeClass("modal-on");
    });

    //kliknuti mimo modal
    $(document).on("click", "#modal-login", function (e) {
        if (!$(e.target).parents("#modal-login").length) {
            e.stopPropagation();
            $("#modal-login").hide();
            $("body").removeClass("modal-on");
        }
    });
});