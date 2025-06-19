"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();
console.clear();
var nowt = new Date();

connection.on("SchedulerQuery", function (id) {
    if (CustomerId == id) {
        var customerNamePublic = "";
        $.ajax({
            type: "GET",
            url: "/getscheduler/" + id,
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                var scheduler = data.scheduler;
                var schedulerPriorityObj = data.schedulerPriority;
                var customersObj = data.customers;
                customerNamePublic = data.customers[0].text;
                $(() => {
                    $('#scheduler').dxScheduler({
                        dataSource: scheduler,
                        type: 'date',
                        views: ['month'],
                        currentView: 'month',
<<<<<<< HEAD
                        //currentDate: new Date($.now()),
                        currentDate: now,
=======
                        currentDate: nowt,
>>>>>>> 908055d269205e5f90321e945e19f6feddfb3e1a
                        showAllDayPanel: false,
                        maxAppointmentsPerCell: 5,
                        allDay: true,
                        FixedWidth: true,
                        crossScrollingEnabled: true,
                        cellDuration: 20,
                        startDayHour: 15,
                        editing: {
                            allowAdding: true,
                            allowDeleting: true,
                            allowUpdating: true,
                            allowResizing: true,
                            allowDragging: true,
                        },
                        onAppointmentAdded(e) {
                            $.ajax({
                                type: "POST",
                                url: "/postscheduler",
                                data: JSON.stringify(e.appointmentData),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    showToast('Oluşturuldu', e.appointmentData.text, 'info');
                                    $.ajax({
                                        type: "GET",
                                        url: "/jsonresult/trigger_WebSocket/" + id,
                                        contentType: "application/json",
                                        dataType: "json",
                                        success: function (data) {
                                            nowt = e.appointmentData.endDate;
                                        }
                                    });
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                }
                            });

                        },
                        onAppointmentUpdated(e) {
                            $.ajax({
                                type: "POST",
                                url: "/updatescheduler",
                                data: JSON.stringify(e.appointmentData),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    showToast('Güncellendi', e.appointmentData.text, 'info');
                                    $.ajax({
                                        type: "GET",
                                        url: "/jsonresult/trigger_WebSocket/" + id,
                                        contentType: "application/json",
                                        dataType: "json",
                                        success: function (data) {
                                            nowt = e.appointmentData.endDate;
                                        }
                                    });
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        },
                        onAppointmentDeleted(e) {
                            $.ajax({
                                type: "GET",
                                url: "/deletescheduler/" + JSON.stringify(e.appointmentData.id),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    nowt = e.appointmentData.endDate;
                                    showToast('Silindi', e.appointmentData.text, 'info');
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        },

                        onAppointmentFormOpening(data) {
                            const { form } = data;
                            console.log(customersObj[0].id);
                            form.option('items', [{
                                label: {
                                    text: 'Adı',
                                },
                                editorType: 'dxTextBox',
                                dataField: 'text',
                                editorOptions: {
                                    width: '100%',
                                    type: 'text',
                                },
                            }, {
                                label: {
                                    text: 'Firma',
                                },
                                editorType: 'dxSelectBox',
                                dataField: 'customersId',
                                editorOptions: {
                                    items: customersObj, placeholder: customersObj[0].text,
                                    displayExpr: 'text',
                                    valueExpr: 'id',
                                    value: customersObj[0].id,
                                    readOnly: true,
                                },
                            }, {
                                label: {
                                    text: 'Durum',
                                },
                                editorType: 'dxSelectBox',
                                dataField: 'priorityId',
                                editorOptions: {
                                    items: schedulerPriorityObj,
                                    displayExpr: 'text',
                                    valueExpr: 'id',
                                },
                            }, {
                                label: {
                                    text: 'Tarih',
                                },
                                dataField: 'startDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    width: '100%',
                                    type: 'date',
                                },
                            }, {
                                colSpan: 2,
                                label: {
                                    text: 'Açıklama',
                                },
                                dataField: 'description',
                                editorType: "dxTextArea",
                                editorOptions: {
                                    width: '100%',
                                },
                            },
                            ]);
                        },
                        resources: [
                            {
                                fieldExpr: 'priorityId',
                                dataSource: schedulerPriorityObj,
                                label: 'Durum',
                                useColorAsDefault: true,
                            }],
                        height: 900,
                    });
                });
            }
        }).done(function (result) {

            $(document).ready(function () {
                $(".dx-toolbar-items-container").append(`
<div class="dx-toolbar-after pr-2"><select class="form-control select-single mt-2" data-width="100%" name="Id" id="customer-scheduler-alt">
<option disabled selected>[ `+ customerNamePublic +` ]</option>
</select></div>

`);

                $.ajax({
                    url: "/getCustomersforScheduler",
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (data) {
                        setToolbarCenter();
                        $.each(data, function (i, item) {

                            $('#customer-scheduler-alt').append($('<option>', {
                                value: item.id,
                                text: item.name
                            }));
                        });
                    }
                }).done(function (result) {
                    $('#customer-scheduler-alt').on('change', function () {

                        window.location.replace("/Scheduler?Id=" + this.value);
                    });
                    
                    console.log("setTopolbarCenter");
                });

            });

        });
    }

    

});

connection.start().then(function () {
}).catch(function (err) {
});