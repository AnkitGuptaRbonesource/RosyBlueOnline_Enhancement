var CtrlMarketInventoryUpload = function () {
    var objSF = null;
    var $frmMktInventorUpload = null;

    var OnLoad = function () {
        objSF = new SearchFilter();
        RegisterEvents();
        SetValidation();
    }

    var RegisterEvents = function () {
        $('#btnUpload').click(function (e) {
            e.preventDefault();
            if ($frmMktInventorUpload.valid() == false) {
                return;
            }
            var fData = new FormData();

            if ($('#fuUpload').get(0).files.length === 0) {
                console.log("File Not Attached");
                return;
            }

            fData.append("File", $('#fuUpload').get(0).files[0]);

            uiApp.BlockUI();
            objSF.UploadMarketUploadInventory(fData).then(function (data) {
                uiApp.UnBlockUI();
                if (data.IsSuccess) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Excel uploaded", type: "success" });
                    location.href = '/Marketing/MarketInventoryUpload';

                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                }
                console.log(data);
            }, function (error) {
                uiApp.UnBlockUI();
            });
        });




        $(document).on('click', '.MarketDownload', function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            var id = $(e.target).data('id');
            var FileName = $(e.target).data('name');
            objSF.MarketdownloadForExcel(id, FileName).then(function (data) {
                if (data.IsSuccess) {
                    $('#ancMktInventoryDownload1').get(0).click();
                    uiApp.UnBlockUI();
                } else {
                    uiApp.UnBlockUI();
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }

            }, function (error) {
                uiApp.UnBlockUI();
            });

        });


        $('#btnDelete').click(function (e) {
            e.preventDefault();
          
            var id = 100;
            uiApp.Confirm('Confirm to delete entire data ?', function (resp) {
                if (resp) {
                    uiApp.BlockUI();
                    objSF.DeleteMarketInventory(id).then(function (data) {
                        if (data.IsSuccess) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Data Deleted", type: "danger" });

                            uiApp.UnBlockUI();
                        } else {
                            uiApp.UnBlockUI();
                            uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                        }

                    }, function (error) {
                        uiApp.UnBlockUI();
                    });
                }

                 
            });

        });

    }


    var SetValidation = function () {
        $frmMktInventorUpload = $('#frmMktInventorUpload');
        $frmMktInventorUpload.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                fileUpload: {
                    required: true,
                    extension: 'xls|xlsx'
                }
            }
        });

        $.validator.addMethod("extension", function (value, element, param) {
            param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
            return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
        }, "Please enter a value with a valid extension.");
    }





    return {
        int: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlMarketInventoryUpload.int();
});