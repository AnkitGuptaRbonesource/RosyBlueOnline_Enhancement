var CrtlStoneHistory = function () {
    var dtSearchTable = null;


    var OnLoad = function () {
        dtSearchTable = new Datatable();
        LoadGrid();
    }

    var RegisterEvent = function () {
        $('#btnShow').click(function (e) {
            e.preventDefault();
            LoadGrid();
        });

        $('#btnFileUpload').click(function (e) {
            e.preventDefault();
        });

        $('#fuStoneIDs').change(function () {
            LoadFile(this);
        });
    }

    var LoadFile = function (ele) {
        var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.csv)$/;
        if (regex.test(ele.value.toLowerCase())) {
            if (typeof (FileReader) != "undefined") {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var rows = e.target.result.split("\n");
                    console.log(e.target.result);
                    $('#txtStoneIDs').val(rows);
                }
                reader.readAsText(ele.files[0]);
            } else {
                uiApp.Alert({ container: '#uiPanel1', message: "This browser does not support HTML5.", type: "error" });
            }
        }
    }

    var LoadGrid = function () {
        if (dtSearchTable.getDataTable() == null || dtSearchTable.getDataTable() == undefined) {
            dtSearchTable.setAjaxParam('StoneIDs', $('#txtStoneIDs').val());
            dtSearchTable.init({
                src: '#tblSearchTable',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    order: [10],
                    ajax: {
                        type: 'Post',
                        url: '/Inventory/GetStoneHistory',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "Lotnumber", orderable: false },
                        { data: "shapeName", orderable: false },
                        { data: "diaWeight", orderable: false },
                        { data: "colorName", orderable: false },
                        { data: "clarityName", orderable: false },
                        { data: "cutGradeName", orderable: false },
                        { data: "pricePerCT", orderable: false },
                        { data: "discount", orderable: false },
                        { data: "FlagDescription", orderable: false },
                        { data: "CompanyName", orderable: false }, 
                        { data: "CustFirstName", orderable: false }, 
                        { data: "CreatedOn", class: '.dt-date', orderable: true },
                        { data: "discount", orderable: false },
                        { data: "newDiscount", orderable: false },
                        { data: "rapPrice", orderable: false },
                        { data: "newRapPrice", orderable: false }
                    ],
                    columnDefs: [{
                        targets: 'dt-date',
                        render: function (data, type, row) {
                            return moment(row.CreatedOn).format(myApp.dateFormat.Client);
                        },
                        orderable: true
                    },
                        {
                            targets: 'CustomerName',
                            render: function (data, type, row) {
                                return row.CustFirstName + " " + row.CustLastName;
                            },
                            orderable: false
                        }

                    ]
                },
                onCheckboxChange: function (obj) {
                },
                onLoadDataCompleted: function () {
                }
            });
        } else {
            dtSearchTable.setAjaxParam('StoneIDs', $('#txtStoneIDs').val());
            dtSearchTable.getDataTable().draw();
        }
    }

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CrtlStoneHistory.init();
});
