var CtrlRFIDHistory = function () {
    var objSvc = null, dtRfidHistory = null;

    var LoadForm = function () {
        objSvc = new RFIDService();
        dtRfidHistory = new Datatable();
        if (dtRfidHistory.getDataTable() == null || dtRfidHistory.getDataTable() == undefined) {
            dtRfidHistory.init({
                src: '#tblRFID',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    order: [[2, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/RFID/GridHistory',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "RFIDno" },
                        { data: "CertificateNO" },
                        { data: "CreatedOn" },
                        { data: "CreatedByName" }
                    ],
                    columnDefs: [{
                        targets: [2],
                        render: function (data, type, row) {
                            return moment(data).format(myApp.dateFormat.Client);
                        },
                        orderable: true
                    }]
                }
            });
        } else {
            dtRfidHistory.getDataTable().draw();
        }
    };

    var RegisterEvent = function () {


    };

    return {
        init: function () {
            LoadForm();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlRFIDHistory.init();
});