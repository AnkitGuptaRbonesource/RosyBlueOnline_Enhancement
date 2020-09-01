var CtrlMarketingBlueNile = function () {
    var objSvc = null; $frmBlueNile = null, dtMarketing = null;
    var valBlueNile = null;

    var OnLoad = function () {
        objSvc = new MarketingService();
        dtMarketing = new Datatable();
        SetValidation();
        LoadGrid();
    }

    var RegisterEvent = function () {
        $(document).on('click', '#btnSave', function (e) {
            e.preventDefault();
            if ($frmBlueNile.valid()) {
                var pData = ReadForm();
                objSvc.CreateBlueNile(pData).then(function (data) {
                    if (data.IsSuccess && data.Result > 0) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored added successfully", type: "success" });
                        ClearForm();
                        LoadGrid();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored not added", type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding recored", type: "danger" });
                });
            }
        });
    };

    var ClearForm = function () {
        $('#frmBlueNile').find("input[type=text], textarea").val("");
    }

    var ReadForm = function () {
        return {
            caratRange1Disc: $('#txtCaratRange1Disc').val(),
            caratRange2Disc: $('#txtCaratRange2Disc').val(),
            caratRange3Disc: $('#txtCaratRange3Disc').val(),
            caratRange4Disc: $('#txtCaratRange4Disc').val(),
            caratRange5Disc: $('#txtCaratRange5Disc').val(),
            caratRange6Disc: $('#txtCaratRange6Disc').val(),
            caratRange7Disc: $('#txtCaratRange7Disc').val(),
            caratRange8Disc: $('#txtCaratRange8Disc').val(),
            caratRange9Disc: $('#txtCaratRange9Disc').val(),
            caratRange10Disc: $('#txtCaratRange10Disc').val(),
            caratRange11Disc: $('#txtCaratRange11Disc').val(),
            caratRange12Disc: $('#txtCaratRange12Disc').val(),
            caratRange13Disc: $('#txtCaratRange13Disc').val(),
            caratRange14Disc: $('#txtCaratRange14Disc').val(),
            caratRange15Disc: $('#txtCaratRange15Disc').val(),
            caratRange16Disc: $('#txtCaratRange16Disc').val(),
            caratRange17Disc: $('#txtCaratRange17Disc').val(),
            caratRange18Disc: $('#txtCaratRange18Disc').val(),
            caratRange19Disc: $('#txtCaratRange19Disc').val(),
            caratRange20Disc: $('#txtCaratRange20Disc').val(),
            caratRange21Disc: $('#txtCaratRange21Disc').val(),
            caratRange22Disc: $('#txtCaratRange22Disc').val(),
            caratRange23Disc: $('#txtCaratRange23Disc').val(),
            caratRange24Disc: $('#txtCaratRange24Disc').val(),
            caratRange25Disc: $('#txtCaratRange25Disc').val(),
            haExDisc: $('#txtHaExDisc').val(),
            haVgDisc: $('#txtHaVgDisc').val()
        }
    };

    var LoadGrid = function () {
        
        if (dtMarketing.getDataTable() == null || dtMarketing.getDataTable() == undefined) {
            dtMarketing.init({
                src: '#tblMarketing',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    order: [[0, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Marketing/GridBlueNile',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "SrNo" },
                        { data: "caratRange1Disc" },
                        { data: "caratRange2Disc" },
                        { data: "caratRange3Disc" },
                        { data: "caratRange4Disc" },
                        { data: "caratRange5Disc" },
                        { data: "caratRange6Disc" },
                        { data: "caratRange7Disc" },
                        { data: "caratRange8Disc" },
                        { data: "caratRange9Disc" },
                        { data: "caratRange10Disc" },
                        { data: "caratRange11Disc" },
                        { data: "caratRange12Disc" },
                        { data: "caratRange13Disc" },
                        { data: "caratRange14Disc" },
                        { data: "caratRange15Disc" },
                        { data: "caratRange16Disc" },
                        { data: "caratRange17Disc" },
                        { data: "caratRange18Disc" },
                        { data: "caratRange19Disc" },
                        { data: "caratRange20Disc" },
                        { data: "caratRange21Disc" },
                        { data: "caratRange22Disc" },
                        { data: "caratRange23Disc" },
                        { data: "caratRange24Disc" },
                        { data: "caratRange25Disc" },
                        { data: "haExDisc" },
                        { data: "haVgDisc" },
                        { data: "createdOn" }
                    ],
                    columnDefs: [{
                        targets: [28],
                        render: function (data, type, row) {
                            return moment(row.orderCreatedOn).format(myApp.dateFormat.Client);
                        },
                        orderable: true
                    }]
                }
            });
        } else {
            dtMarketing.getDataTable().draw();
        }

    };

    var SetValidation = function () {
        $frmBlueNile = $('#frmBlueNile');
        var rPattern = /^100(\.0{0,2}?)?$|^\d{0,2}(\.\d{0,2})?$/;
        valBlueNile = $frmBlueNile.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                caratRange1Disc: {
                    required: true,
                    number: true
                },
                caratRange2Disc: {
                    required: true,
                    number: true
                },
                caratRange3Disc: {
                    required: true,
                    number: true
                },
                caratRange4Disc: {
                    required: true,
                    number: true
                },
                caratRange5Disc: {
                    required: true,
                    number: true
                },
                caratRange6Disc: {
                    required: true,
                    number: true
                },
                caratRange7Disc: {
                    required: true,
                    number: true
                },
                caratRange8Disc: {
                    required: true,
                    number: true
                },
                caratRange9Disc: {
                    required: true,
                    number: true
                },
                caratRange10Disc: {
                    required: true,
                    number: true
                },
                caratRange11Disc: {
                    required: true,
                    number: true,
                },
                caratRange12Disc: {
                    required: true,
                    number: true
                },
                caratRange13Disc: {
                    required: true,
                    number: true
                },
                caratRange14Disc: {
                    required: true,
                    number: true
                },
                caratRange15Disc: {
                    required: true,
                    number: true
                },
                caratRange16Disc: {
                    required: true,
                    number: true
                },
                caratRange17Disc: {
                    required: true,
                    number: true
                },
                caratRange18Disc: {
                    required: true,
                    number: true
                },
                caratRange19Disc: {
                    required: true,
                    number: true
                },
                caratRange20Disc: {
                    required: true,
                    number: true
                },
                caratRange21Disc: {
                    required: true,
                    number: true
                },
                caratRange22Disc: {
                    required: true,
                    number: true
                },
                caratRange23Disc: {
                    required: true,
                    number: true
                },
                caratRange24Disc: {
                    required: true,
                    number: true
                },
                caratRange25Disc: {
                    required: true,
                    number: true
                },
                haExDisc: {
                    required: true,
                    number: true
                },
                haVgDisc: {
                    required: true,
                    number: true
                }
            }
        });
    };

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlMarketingBlueNile.init();
});
