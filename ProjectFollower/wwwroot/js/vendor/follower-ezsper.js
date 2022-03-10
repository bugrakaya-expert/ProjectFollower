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
            })
        } else {
            console.log("eklenenler: "+playervalue);
            $('#mission-list').append('<tr>' + '<td><p class="list-item-heading get-task-item">' + listvalue + '</p></td>' + '<td class="d-none"><p class="list-item-heading get-task-item">' + playerid + '</p></td>' + '<td><p class="">' + playervalue + '</p></td>' + '<td class="d-flex justify-content-end"><button type="button" class="btn btn-danger mb-1 deleteli"><i class="simple-icon-trash mr-2"></i>Sil</button><td></tr>');
            $("#mission-textbox").val("");
        }
    });
    $('#mission-list').on('click', '.deleteli', function (e) {
        $(this).closest('tr').remove();
    });

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

})(jQuery);