var CtrlRFIDMaster = function () {
    var objSvc = null;

    var LoadForm = function () {
        objSvc = new RFIDService();
    };

    var RegisterEvent = function () {

        $('#mstRFIDUpload').click(function (e) {
            e.preventDefault();
            var fd = new FormData();
            fd.append("File", $('#fileUpload').get(0).files[0]);
            objSvc.AddRFIDViaExcel(fd).then(function (data) {
                if (data.IsSuccess && data.Result > 0) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Rfids added successfully", type: "success" });
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding data", type: "danger" });
            });

        });

        $('input[type="radio"][data-rfid]').click(function () {
            var Status = $(this).val() == "1" ? false : true;
            var rfid = $(this).data('rfid');
            uiApp.BlockUI();
            objSvc.UpdateRFIDStatus(rfid, Status).then(function (data) {
                if (data.IsSuccess == true) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Status updated successfully", type: "success" });
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Problem in updating status", type: "danger" });
                uiApp.UnBlockUI();
            });
        });

    };


    return {
        init: function () {
            LoadForm();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlRFIDMaster.init();
});