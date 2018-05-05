$(document).ready(function () {

    $(document).on("click", ".categories", function () {
        $("#categories-preview").slideToggle();
        var x = ($(".img-category"));
        for (var i = 0; i < 4; i++) {
            x[i].src = x[i].getAttribute("data-src");
        }
    });

    $(document).on("click", "body, html", function (e) {
        if ((!$(e.target).parents("#categories-preview").length) && (e.target.id != "categories-preview") && (e.target.id != "category")) {
            e.stopPropagation();
            $("#categories-preview").hide();
        }
    });
});