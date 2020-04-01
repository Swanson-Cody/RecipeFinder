// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Get the container element


var slideIndex = 0;
carousel();


function carousel() {
    var i;
    var x = document.getElementsByClassName("mySlides");
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }
    slideIndex++;
    if (slideIndex > x.length) { slideIndex = 1 }
    x[slideIndex - 1].style.display = "block";
    setTimeout(carousel, 5000); // Change image every 5 seconds
}


var slideCaptionIndex = 0;
carouselCaption();

function carouselCaption() {
    var a;
    var q = document.getElementsByClassName("myCaptions");
    for (a = 0; a < q.length; a++) {
        q[a].style.display = "none";
    }
    slideCaptionIndex++;
    if (slideCaptionIndex > q.length) { slideCaptionIndex = 1 }
    q[slideCaptionIndex - 1].style.display = "block";
    setTimeout(carouselCaption, 5000); // Change image every 5 seconds
}