var CtrlInventoryUploadHistory = function () {
    var dtFileUpload = null;

    var OnLoad = function () {
        dtFileUpload = new Datatable();
        RegisterEvents();
        SetValidation();

        if (dtFileUpload.getDataTable() == null || dtFileUpload.getDataTable() == undefined) {
            dtFileUpload.init({
                src: '#tblUploadHistory',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    order: [[0, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Inventory/UploadHistoryForGrid?Type=Rfid',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "fileId" },
                        { data: "uploadFullName" },
                        //{ data: null },
                        { data: null },
                        { data: "createdOn" },
                        { data: "validInv" },
                        { data: "invalidInv" }
                    ],
                    columnDefs: [{
                        targets: [2],
                        render: function (data, type, row) {
                            return '<a href="/Content/INV/' + row.fileId + '.xlsx">' + row.fileName+'</a>';
                        },
                        orderable: false
                    }, {
                        targets: [3],
                        render: function (data, type, row) {
                            return moment(row.createdOn).format(myApp.dateFormat.Client);
                        }
                    }, {
                        targets: [4],
                        render: function (data, type, row) {
                            if (data != 0) {
                                return '<a href="/Content/INV/' + row.fileId + '_Valid.xlsx' + '">' + data + '</a>';
                            } else {
                                return 0;
                            }
                        },
                        orderable: false
                    }, {
                        targets: [5],
                        render: function (data, type, row) {
                            if (data != 0) {
                                return '<a href="/Content/INV/' + row.fileId + '_InValid.xlsx' + '">' + data + '</a>';
                            } else {
                                return 0;
                            }
                        },
                        orderable: false
                    }]
                }
            });

        } else {
            dtFileUpload.getDataTable().draw();
        }
    }

    var RegisterEvents = function () {

    }

    var SetValidation = function () {

    }

    return {
        int: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlInventoryUploadHistory.int();
});