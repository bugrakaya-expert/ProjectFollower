"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();
//var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.Error).withUrl("./homeHub").build();
var cookie = $.cookie('_nb59x45k9');
function showNotification(placementFrom, placementAlign, type, message, title, projectId) {
    alert("func: "+projectId);
    $.notify(
        {
            title: title,
            message: message,
            url: "/proje-detaylari/"+projectId,
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
}

connection.on("SendNotification", function (notifyData) {
    var dataStr = JSON.stringify(notifyData);

    if (notifyData.userId == cookie) {
        showNotification("top", "right", "primary", notifyData.message, notifyData.title, notifyData.projectId);

    }
});

connection.start().then(function () {
}).catch(function (err) {
});