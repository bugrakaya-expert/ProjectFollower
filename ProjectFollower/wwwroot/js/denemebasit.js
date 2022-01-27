"use strict";
//var homeHub = $.connection.homeHub;
var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();

$.connection.hub.start().done(function () {
    $("btnSend").click(function () {
        var message = $("messageTextbox").val();
        connection.server.send(message);
    });
    connection.client.SendAsync = function (message) {
        $("#messageTxt").append(message);
    }
})