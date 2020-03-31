// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Get the container element


//FAILED
/*var actualLinks = document.querySelectorAll("a.nav-link");

actualLinks.forEach(button => {
    actualLinks.addEventListener("click",
        function () {
            actualLinks.forEach(link => link.classList.remove("active"));
            this.classList.add("active");
        });
});*/

//FAILED
/*var btnContainer = document.getElementById("test");

// Get all buttons with class="btn" inside the container
var btns = btnContainer.getElementsByClassName("nav-link");

// Loop through the buttons and add the active class to the current/clicked button
for (var i = 0; i < btns.length; i++) {
  btns[i].addEventListener("click", function() {
    var current = document.getElementsByClassName("active");
    current[0].className = current[0].className.replace(" active", "");
    this.className += " active";
  });
}*/




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
    setTimeout(carousel, 7000); // Change image every 5 seconds
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
    setTimeout(carouselCaption, 7000); // Change image every 5 seconds
}