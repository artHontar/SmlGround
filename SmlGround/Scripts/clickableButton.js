var label = document.querySelectorAll(".pull-right");
for (let i = 0; i < label.length; i++) {
    label[i].addEventListener("click", function (event) { WorkWithFriend(event.target); }, true);
}