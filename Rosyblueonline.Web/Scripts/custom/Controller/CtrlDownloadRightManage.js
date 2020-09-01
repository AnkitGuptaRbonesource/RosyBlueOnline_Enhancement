var CtrlDownloadRightManage = function () {
    var objSvc = null, dtRfidHistory = null;

    var LoadForm = function () {
        objSvc = new DownloadRightsService();
        //dtRfidHistory = new Datatable();
        //if (dtRfidHistory.getDataTable() == null || dtRfidHistory.getDataTable() == undefined) {
        //    dtRfidHistory.init({
        //        src: '#tblRFID',
        //        dataTable: {
        //            //deferLoading: 0,
        //            paging: true,
        //            order: [[2, "desc"]],
        //            ajax: {
        //                type: 'Post',
        //                url: '/RFID/GridHistory',
        //                beforeSend: function (request) {
        //                    var TokenID = myApp.token().get();
        //                    request.setRequestHeader("TokenID", TokenID);
        //                    return request;
        //                }
        //            },
        //            columns: [
        //                { data: "RFIDno" },
        //                { data: "CertificateNO" },
        //                { data: "CreatedOn" },
        //                { data: "Createdyby" }
        //            ],
        //            columnDefs: [{
        //                targets: [2],
        //                render: function (data, type, row) {
        //                    return moment(data).format(myApp.dateFormat.Client);
        //                },
        //                orderable: true
        //            }]
        //        }
        //    });
        //} else {
        //    dtRfidHistory.getDataTable().draw();
        //}
    };

    var RegisterEvent = function () {
        $('#ddlUser').change(function (e) {
            if ($(this).val() != "") {
                objSvc.GetRightForUser($(this).val()).then(function (data) {
                    BindCheckBox(data);
                }, function (error) {
                });
            } else {
                BindCheckBox([]);
            }
        });

        $('#btnUpdate').click(function (e) {
            e.preventDefault();
            if ($('#ddlUser').val() != "") {
                var ListOfItem = [];
                var lstChk = $('input[type="checkbox"][name="Rights"]:checked').get();
                for (var i = 0; i < lstChk.length; i++) {
                    ListOfItem.push({ LoginID: $('#ddlUser').val(), DownloadID: $(lstChk[i]).val() });
                }
                objSvc.PutRightForUser(ListOfItem).then(function (data) {
                    if (data.IsSuccess) {
                        alert('Updated rights');
                        location.reload();
                    } else {
                        alert('Problem in updating');
                    }
                }, function (error) {
                    alert('Error in updating');
                });
            }
        });
    };

    var BindCheckBox = function (data) {
        var lstChk = $('input[type="checkbox"][name="Rights"]').get();
        for (var i = 0; i < lstChk.length; i++) {
            $(lstChk[i]).prop('checked', false);
        }
        for (var i = 0; i < data.length; i++) {
            $('#chkRight' + data[i].DownloadID).prop('checked', true);
        }
    }

    return {
        init: function () {
            LoadForm();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlDownloadRightManage.init();
});