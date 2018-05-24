$(document).ready(function () {


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