// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(document).ready(function () {
    //const locImg = "/assets/users/img/";
    $("#photochange").text("");


    $.ajax({
        url: "jsonresult/getphotolinkjson",
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (imglink) {
            $("#photochange").append(`

Mevcut Profil Fotoğrafı<br />
<img src='`+ imglink +`' alt='' class='img-thumbnail border-0 rounded-circle list-thumbnail align-self-center'>



`);
        }
    });






});
function editUserFunc() {
    $("#modalsubmit").prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "jsonresult/edituserpassjson",
        data: {
            editUserPass: {
                currentPassword: $("#modalCurrentPassword").val(),
                newPassword: $("#modalPassword").val(),
                confirmnewPassword: $("#modalPasswordconfirm").val()
            }
        },
        success: function (msg) {

            $('#sifreModal').modal('toggle');
            $("#modalCurrentPassword").val('');
            $("#modalPassword").val('');
            $("#modalPasswordconfirm").val('');
            $("#modalsubmit").prop('disabled', false);

            var msg_code = "";
            var msg_desc = "";
            var msg_icon = "";

            if (msg.length == 0) {
                msg_desc = "Şifreniz başarılı bir şekilde güncelleştirildi.";
                msg_icon = "success";
            }
            else {
                msg_code = msg[0].code;
                msg_desc = msg[0].description;

                if (msg_code == "PasswordMismatch") {
                    msg_desc = "Geçerli şifreniz yalnış";
                    msg_icon = "error";
                }
                if (msg_code == "PasswordTooShort") {
                    msg_desc = "Şifreleriniz en az 6 karakter olmak zorundadır.";
                    msg_icon = "error";
                }
            }

            Swal.fire({
                title: msg_desc,
                showCancelButton: false,
                icon: msg_icon,
                confirmButtonText: 'Tamam',
            })


        }
    });



}

/*
function updatePhotoFunc() {
    $("#modalPhotoSubmit").prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "jsonresult/updateUserPhoto",
        data: {

        },
        success: function (msg) {

            Swal.fire({
                title: "Profil fotoğrafı başarılı bir şekilde güncelleştirildi.",
                showCancelButton: false,
                icon: msg_icon,
                confirmButtonText: 'Tamam',
            })


        }
    });



}

*/