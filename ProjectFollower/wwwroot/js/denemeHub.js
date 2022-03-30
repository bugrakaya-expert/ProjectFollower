"use strict";

var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.Error).withUrl("./homeHub").build();
//var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.None).withUrl("/homeHub").build();
//connection.start();


connection.on("ReceiveMessage", function (message) {

    $("#messageTxt").append(message);
});

connection.start().then(function () {
    /*document.getElementById("sendButton").disabled = false;*/
}).catch(function (err) {
    //return console.error(err.toString());
});


document.getElementById("btnSend").addEventListener("click", function (event) {
    var message = document.getElementById("messageTextbox").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        //return console.error(err.toString());
    });
    event.preventDefault();
});