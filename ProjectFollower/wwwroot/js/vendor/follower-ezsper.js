(function ($) {
    $('#mission-add').click(function () {
        var listvalue = $('#mission-textbox').val();
        var playerid = $('#mission-players').val();
        var playervalue = new Array();
        $('.gorev-players .select2-selection__choice').each(function () {
            playervalue.push( $(this).attr('title'));
        });
        if (!playervalue.length || !listvalue.length) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Proje görevlerinde boş alan bırakamazsın!',
                confirmButtonText: 'Tamam'
            });
        } else {
            $('#mission-list').append('<tr>' + '<td><p class="list-item-heading get-task-item">' + listvalue + '</p></td>' + '<td class="d-none"><p class="list-item-heading get-task-item">' + playerid + '</p></td>' + '<td><p class="">' + playervalue + '</p></td>' + '<td class="d-flex justify-content-end"><button type="button" class="btn btn-danger mb-1 deleteli"><i class="simple-icon-trash mr-2"></i>Sil</button><td></tr>');
            $("#mission-textbox").val("");
        }
    });
    $('#mission-list').on('click', '.deleteli', function (e) {
        $(this).closest('tr').remove();
    });
    $('#submitForm').on('click', function () {
        if (!$('#mission-list').children('tr').length) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Proje görevlerinde boş alan bırakamazsın!',
                confirmButtonText: 'Tamam'
            })
        } 
    });
    let searchParams = new URLSearchParams(window.location.search);
    let param = searchParams.get('status');
    if (param == "true") {
        Swal.fire({
            icon: 'success',
            title: 'Başarılı...',
            text: 'Proje başarıyla eklendi!',
            confirmButtonText: 'Tamam'
        }).then((result) => {
            window.location.href = 'dashboard';  
        });
    }

    $(".remove-customer-document").click(function () {
        var id = $(this).attr('id');
        Swal.fire({
            title: 'Bu dökümanı kaldırmak istediğinizden emin misiniz?',
            showCancelButton: true,
            icon: 'info',
            confirmButtonText: 'Kaldır',
            cancelButtonText: `İptal Et`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                $.ajax({
                    url: "/jsonresult/removeCustomerDocument/" + id,
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (data) {
                        Swal.fire({
                            title: 'Başarıyla güncellendi',
                            icon: 'success',
                            confirmButtonText: 'Tamam',
                        }).then((result) => {
                            location.reload();
                        });
                    }
                });
            }
        })
    });

    $('.file-avatar').each(function(){
        var label = $('.custom-select-label');
        var labelValue = $(label).html();
        var fileInput = $('input[type="file"]', this);
        $(fileInput).on('change', function(){
            var fileName = $(this).val().split('\\').pop();
            var names = [];
            for (var i = 0; i < $(this).get(0).files.length; ++i) {
                names.push($(this).get(0).files[i].name+"<br>");
            }
            $(label).html(names);
        });
    });

    $("#tableSearch").on("keyup", function() {
    var value = $(this).val().toLowerCase();
        $("#tbody tr").filter(function() {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    var notnews = true;
    $('#notificationButton').on("click", function () {
        $(".notification-unread i").css("animation", "none");
        $(".notification-unread .count").css("animation", "none");
    });
    var isreaded = "";
    var totalnot = 0;
    $.ajax({
        url: '/api/getnotify',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $.each(data, function (key, value) {
                if (totalnot < 5) {
                    if (value.readed == false) {
                        var item = `<div class="d-flex flex-row align-items-center notification-item p-2 border-bottom">
                                <input type="checkbox" class="custom-control-input notifyvalue d-none" data-userid=`+ value.userId + ` data-projectid=` + value.projectId + ` data-title="` + value.title + `" data-date=` + value.date + ` data-message="` + value.message + `" id=` + value.id +` >
                                <a href="`+ value.url + `">
                                    <img src="/assets/customers/b1ba221f-0aff-466c-a2c7-08d9f60b54f2/img/412a889e-1af4-4a1b-a29a-3e22717a8248.jpg" alt="Notification Image"
                                            class="img-thumbnail list-thumbnail xsmall border-0 rounded-circle" />
                                </a>
                                <div class="pl-3">
                                    <a href="`+ value.url + `">
                                        <p class="mb-1">`+ value.title + `</p>
                                        <p class="text-muted mb-0 text-small">`+ value.date + `</p>
                                    </a>
                                </div>
                            </div>`
                        $(".notification-list").append(item);
                        totalnot++;
                    }
                }
                else {
                    totalnot = "5+";
                }
                
            });
            if (totalnot == 0) {
                var item = `<div class="d-flex flex-row align-items-center justify-content-center p-2 border-bottom">
                                <p class="font-weight-bold mb-1">Yeni bir bildirim yok</p>
                            </div>`
                $(".notification-list").append(item);
                $("#notificationButton").removeClass("notification-unread");
            }
            $(".notification-unread-count").text(totalnot);
            
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });

    $(document).on("click", ".notification-item", function () {
        var formdata = [];
        $(this).find('input').each(function () {
            var item = {};
            //here the item name should be the same as your model
            item['id'] = this.id,
            item['date'] = $(this).data("date"),
            item['message'] = $(this).data("message"),
            item['userid'] = $(this).data("userid"),
            item['projectid'] = $(this).data("projectid"),
            item['title'] = $(this).data("title"),
            item['readed'] = true
            formdata.push(item);
        });
        //here the model should be the same as your controller parameter name
        formdata = JSON.stringify(formdata);
        updateOnAction(formdata);
    });
    function updateOnAction(formdata) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',//this statement should be added
            url: "/api/updatenotify",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            traditional: true,
            data: formdata,
            dataType: "json",
            success: function (received) {
                
            }
        });
    };


})(jQuery);