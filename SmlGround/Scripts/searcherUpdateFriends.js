﻿document.querySelector("button#search").addEventListener("click", function () {
    var input = document.querySelector("input#forSearch");
    if (input.value != "") {
        $("#forUl").load('@Url.Action("FindFriends","Social")' + '?text=' + input.value);
    }
}, true);