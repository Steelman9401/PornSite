$(document).ready(function () {

    $(document).on("click", ".categories", function () {
        $("#categories-preview").slideToggle();
    });

    $(document).on("click", "body, html", function (e) {
        if ((!$(e.target).parents("#categories-preview").length) && (e.target.id != "categories-preview") && (e.target.id != "category")) {
            e.stopPropagation();
            $("#categories-preview").hide();
        }
    });
});