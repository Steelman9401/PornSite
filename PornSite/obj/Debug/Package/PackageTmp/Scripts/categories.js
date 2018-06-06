function lazyLoadCats() {
    let options = {
        root: null, // relative to document viewport 
        rootMargin: '0px', // margin around root. Values are similar to css property. Unitless values not allowed
        threshold: 0.3 // visible amount of item shown in relation to root
    };
    let observer = new IntersectionObserver(onChangeC, options);
    let images = document.querySelectorAll('.category-img');
    images.forEach(img => observer.observe(img));
}
function onChangeC(changes, observer) {
    changes.forEach(change => {
        if (change.intersectionRatio > 0) {
            if (change.target.getAttribute("src") == "../Content/placeholder.jpg") {
                var img = new Image();
                img.src = change.target.getAttribute('data-img');
                img.onload = function () {
                    $(change.target).fadeOut(250, function () {
                        change.target.setAttribute("src", img.src);
                        $(change.target).fadeIn(250);
                    });
                }
            }
        }
    });
}
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