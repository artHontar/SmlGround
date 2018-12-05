var input = document.querySelector("input#uploadImage");
input.addEventListener("change", function () {

    if (input.value != "") {
        var file = document.querySelector("input[type='file']").files[0];

        var formdata = new FormData();
        formdata.append("uploadImage", file);
        formdata.append("id", document.querySelector("input[name='Id']").value);
        $.ajax({
            url: "/Social/EditAvatar",
            data:
                formdata
            ,
            dataType: "json",
            contentType: false,
            processData: false,
            type: "post",
            success: function ParseResponse(data) {
                var img = document.querySelectorAll("img[name='avatar']");
                var reader = new FileReader();
                var base64Image;
                reader.readAsDataURL(file);
                reader.onload = function () {
                    base64Image = reader.result;
                    for (let i = 0; i < img.length; i++)
                    { 
                        img[i].setAttribute(
                            'src', base64Image
                            );
                    }
                    input.value = "";

                }
            },
            error: function Fail() {
                alert("Что-то не получилось, попробуй ещё");
            }
        });
    };

}, true);