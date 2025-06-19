"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();
//var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.Error).withUrl("./homeHub").build();
var cookie = $.cookie('_nb59x45k9');
function showNotification(placementFrom, placementAlign, type, message, title, projectId, url) {
    //alert("func: "+projectId);
    $.notify(
        {
            title: title,
            message: message,
            url: url,
            target: "_blank"
        },
        {
            element: "body",
            position: null,
            type: type,
            allow_dismiss: true,
            newest_on_top: false,
            showProgressbar: false,
            placement: {
                from: placementFrom,
                align: placementAlign,
            },
            offset: 20,
            spacing: 10,
            z_index: 1031,
            delay: 4000,
            timer: 6000,
            url_target: "_blank",
            mouse_over: null,
            animate: {
                enter: "animated fadeInDown",
                exit: "animated fadeOutUp"
            },
            onShow: null,
            onShown: null,
            onClose: null,
            onClosed: null,
            icon_type: "class",
            template:
                '<div data-notify="container" class="col-11 col-sm-3 alert  alert-{0} " role="alert">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                "</div>" +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                "</div>"
        }
    );
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
                                <input type="checkbox" class="custom-control-input notifyvalue d-none" data-userid=`+ value.userId + ` data-projectid=` + value.projectId + ` data-title="` + value.title + `" data-date=` + value.date + ` data-message="` + value.message + `" id=` + value.id + ` >
                                <a href="`+ value.url + `">
                                    <img src="/assets/customers/b1ba221f-0aff-466c-a2c7-08d9f60b54f2/img/412a889e-1af4-4a1b-a29a-3e22717a8248.jpg" alt="Notification Image"
                                            class="img-thumbnail list-thumbnail xsmall border-0 rounded-circle" />
                                </a>
                                <div class="pl-3">
                                    <a href="`+ value.url + `">
                                        <p class="font-weight-bold mb-1">`+ value.title + `</p>
                                        <p class="font-weight-medium mb-1">`+ value.message + `</p>
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
}

connection.on("SendNotification", function (notifyData) {
    var dataStr = JSON.stringify(notifyData);

    if (notifyData.userId == cookie) {
        showNotification("top", "right", "primary", notifyData.message, notifyData.title, notifyData.projectId, notifyData.url);

    }
});

connection.start().then(function () {
}).catch(function (err) {
});