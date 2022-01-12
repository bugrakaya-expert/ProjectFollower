(function ($) {
    $('#mission-add').click(function() {
        var listvalue = $('#mission-textbox').val();
        var playervalue = $('#mission-players').val();
        if (!playervalue.length || !listvalue.length) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Görev tanımında boş alan bırakamazsın!',
                confirmButtonText: 'Tamam'
            })
        }else{    
            $('#mission-list').append('<tr>' + '<td><p class="list-item-heading">' + listvalue + '</p></td>' + '<td><p class="text-muted">' + playervalue + '</p></td>' + '<td class="d-flex justify-content-end"><button type="button" class="btn btn-danger mb-1 deleteli"><i class="simple-icon-trash mr-2"></i>Sil</button><td></tr>');
            $("#mission-textbox").val("");
        }
    });  
    $('#mission-list').on('click', '.deleteli', function(e) {
        $(this).closest('tr').remove();
    });
    $( ".delete-project" ).click(function() {
        Swal.fire({
            title: 'Bu projeyi kaldırmak istediğinize emin misiniz?',
            showCancelButton: true,
            icon: 'info',
            confirmButtonText: 'Kaldır',
            cancelButtonText: `İptal Et`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Başarıyla silindi',
                    icon: 'success',
                    confirmButtonText: 'Tamam',
                })
            }
        })
    });
    $( ".upgrade-user" ).click(function() {
        Swal.fire({
            title: 'Bu kişiyi yönetici yapmak ister misiniz?',
            showCancelButton: true,
            icon: 'info',
            confirmButtonText: 'Yükselt',
            cancelButtonText: `İptal Et`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Kişinin rütbesi başarıyla yükseltildi',
                    icon: 'success',
                    confirmButtonText: 'Tamam',
                })
            }
        })
    });
    $( ".delete-user" ).click(function() {
        Swal.fire({
            title: 'Bu kişiyi silmek ister misiniz?',
            showCancelButton: true,
            icon: 'info',
            confirmButtonText: 'Sil',
            cancelButtonText: `İptal Et`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Kişi başarıyla silindi',
                    icon: 'success',
                    confirmButtonText: 'Tamam',
                })
            }
        })
    });
    $( ".fall-user" ).click(function() {
        Swal.fire({
            title: 'Bu kişiyi sadece kullanıcı yapmak ister misiniz?',
            showCancelButton: true,
            icon: 'info',
            confirmButtonText: 'Düşür',
            cancelButtonText: `İptal Et`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Kişinin rütbesi başarıyla düşürüldü',
                    icon: 'success',
                    confirmButtonText: 'Tamam',
                })
            }
        })
    });
    $('.file-avatar').each(function(){
        var label = $('.custom-select-label');
        var labelValue = $(label).html();
        var fileInput = $('input[type="file"]', this);
        $(fileInput).on('change', function(){
            var fileName = $(this).val().split('\\').pop();
                if (fileName) { 
                    $(label).html(fileName);
                } 
                else { 
                    $(label).html(labelValue);
                }
            });
        });
})(jQuery);