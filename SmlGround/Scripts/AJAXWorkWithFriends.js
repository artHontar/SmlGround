function WorkWithFriend(target) {
    var hrefClasses = target.classList;
    var getOperation = "";
    for (let i = 0; i < hrefClasses.length; i++) {
        if (hrefClasses[i] == "add-friend") {
            getOperation = hrefClasses[i];
            break;
        }
        else if (hrefClasses[i] == "approve-friend") {
            getOperation = hrefClasses[i];
            break;
        }
        else if (hrefClasses[i] == "reject-friend") {
            getOperation = hrefClasses[i];
            break;
        }
        else if (hrefClasses[i] == "delete-friend") {
            getOperation = hrefClasses[i];
            break;
        }
        else if (hrefClasses[i] == "send-message") {
            getOperation = hrefClasses[i];
            break;
        }
    }
    if (getOperation == "add-friend") {
        $.ajax({
            url: "/Social/AddFriend",
            data: {
                id: target.id
            },
            dataType: "json",
            type: "post",
            success: function Response(data) {
                target.classList.remove("add-friend");
                target.classList.add("reject-friend");
                target.title = "Отменить заявку";
                target.innerText = "Отменить заявку";
                target.classList.remove("btn-success");
                target.classList.add("btn-warning");
            },
            error: function Response(data) {
                alert("Ну, попробуй ещё раз");
            }
        });
    }
    if (getOperation == "approve-friend") {
        $.ajax({
            url: "/Social/ApproveFriend",
            data: {
                id: target.id
            },
            dataType: "json",
            type: "post",
            success: function Response(data) {
                target.classList.remove("approve-friend");
                target.classList.add("delete-friend");
                target.title = "Удалить из друзей";
                target.innerText = "Удалить из друзей";
                target.classList.remove("btn-success");
                target.classList.add("btn-danger");
            },
            error: function Response(data) {
                alert("Ну, попробуй ещё раз");
            }
        });
    }
    if (getOperation == "reject-friend") {
        $.ajax({
            url: "/Social/RejectFriend",
            data: {
                id: target.id
            },
            dataType: "json",
            type: "post",
            success: function Response(data) {
                target.classList.remove("reject-friend");
                target.classList.add("add-friend");
                target.title = "Добавить в друзья";
                target.innerText = "Добавить в друзья";
                target.classList.remove("btn-warning");
                target.classList.add("btn-primary");

            },
            error: function Response(data) {
                alert("Ну, попробуй ещё раз");
            }
        });
    }
    if (getOperation == "delete-friend") {
        $.ajax({
            url: "/Social/DeleteFriend",
            data: {
                id: target.id
            },
            dataType: "json",
            type: "post",
            success: function Response(data) {
                target.classList.remove("delete-friend");
                target.classList.add("approve-friend");
                target.title = "Принять заявку";
                target.innerText = "Принять заявку";
                target.classList.remove("btn-danger");
                target.classList.add("btn-success");
            },
            error: function Response(data) {
                alert("Ну, попробуй ещё раз");
            }
        });
    }
}