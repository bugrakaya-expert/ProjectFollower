"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();
//var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.None).withUrl("/homeHub").build();
//connection.start();
console.log("Web Socket Connected. - Custom Message -");

connection.on("SendData", function (projectList) {
    console.log("before each: "+projectList);
    $("#tbody").text("");
    $.each(projectList.projects, function (i, item) {
        var name = item.name;
        var id = item.id;
        var status = "";
        var isDelayed = item.isDelayed;
        var endingData = item.endingDate;
        var custImage = item.customers.imageUrl;
        var custmail = item.customers.email + '/img/';
        var img = "/assets/customers/" + custmail + custImage;
        var delayedspan = '';
        if (isDelayed == true) {
            delayedspan = `<span class="badge badge-pill badge-danger ml-3">Proje Gecikmiş!</span>`
        }

        var custName = item.customers.name;
        var sequence = item.projectSequence;
        var dateSequence = item.sequanceDate;
        //var comptype = item.companyType.name;
        if (item.status == 0)
            status = `<span class="badge badge-pill badge-info">YENİ</span>` + delayedspan;
        if (item.status == 1)
            status = `<span class="badge badge-pill badge-warning">YAPIM AŞAMASINDA</span>` + delayedspan;
        if (item.status == 2)
            status = `<span class="badge badge-pill badge-secondary">MÜŞTERİ ONAYINDA</span>` + delayedspan;
        if (item.status == 3)
            status = `<span class="badge badge-pill badge-primary">TAMAMLANDI</span>`;

        var htmlstr = `
                                    <tr class="cardd" id="`+ id +`">
                                        <td>
                                            <img src="`+ img + `" alt="` + name + `" class="list-thumbnail responsive border-0 card-img-left" />
                                        </td>
                                        <td>
                                            <p class="d-none">`+ dateSequence + `</p> <p>` + custName + `</p>
                                        </td>
                                        <td>
                                            <p class="list-item-heading">`+ name + `</p>
                                        </td>
                                        <td>
                                            <p class="d-none">`+ dateSequence + `</p> <p>` + endingData + `</p>
                                        </td>
                                        <td>
                                            <p class="d-none">`+ sequence + `</p>` + status + `
                                        </td>
                                        <td class="text-center">
                                            <a href="/proje-detaylari/`+ item.id + `" type="button" class="btn btn-secondary btn-sm mb-1"><i class="glyph-icon simple-icon-info mr-1"></i>Detaylar</a>
                                        </td>
                                    </tr>
`;

        $("#tbody").append(htmlstr);

        console.log("each looping Index: " + i);
    });


    var alertstr = `
        <div class="alert alert-danger alert-dismissible fade show rounded mb-4" role="alert"><strong>Dikkat!</strong> Termini geçmiş `+ data.delayedProjects + ` adet projeniz bulunmakta. <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button></div>
`;
    if (data.delayedProjects > 0)
        $("#mp-attention").html(alertstr);












    console.log("after each: "+projectList);



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
