"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();
//var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.None).withUrl("/homeHub").build();
//connection.start();
console.clear();

connection.on("SendData", function (projectList) {
    window.location.reload(true)


    //$("#messageTxt").append(projectList);
});

connection.start().then(function () {
    /*document.getElementById("sendButton").disabled = false;*/
}).catch(function (err) {
    //return console.error(err.toString());
});


/*
document.getElementById("btnSend").addEventListener("click", function (event) {

    event.preventDefault();
});*/
/*
connection.invoke("SendData", dataTableVal).catch(function (err) {
    return console.error(err.toString());
});*/
