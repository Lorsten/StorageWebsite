document.getElementById("Hambugermenu").addEventListener("click", function () {
    let menu = document.querySelector("header nav");
    menu.classList.remove("Slide-in");
    menu.classList.toggle("is-active");
});



document.getElementById("Hambugermenu-open").addEventListener("click", function () {
    let menu = document.querySelector("header nav");
    menu.classList.toggle("is-active");
    menu.style.animationPlayState = "running";
    menu.classList.add("Slide-in");
    let count = 0;
    // Create a countdown for 20 frames than remove the class again 
    const Time = () => {
        if (count == 20) {
            window.cancelAnimationFrame(Time);
            menu.classList.remove("Slide-in");
        } else {
            count++;
            window.requestAnimationFrame(Time);
        }
    }
    window.requestAnimationFrame(Time);
})