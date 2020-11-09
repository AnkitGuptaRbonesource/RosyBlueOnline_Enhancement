var CtrlMarketingJamesAllen = function () {
    var objSvc = null; $frmJamesAllen = null, dtMarketing = null;
    var valJamesAllen = null;

    var OnLoad = function () {
        objSvc = new MarketingService();
        dtMarketing = new Datatable();
        SetValidation();
        LoadGrid();
    }

    var RegisterEvent = function () {
        $(document).on('click', '#btnSave', function (e) {
            e.preventDefault();
            if ($frmJamesAllen.valid()) {
                var pData = ReadForm();
                objSvc.CreateJamesAllen(pData).then(function (data) {
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





        $(document).on('click', 'a.btnEditDetails', function (e) {
            e.preventDefault();
            ClearForm();
            var id = $(e.target).data('id');
            objSvc.EditJamesAllen(id).then(function (data) {
                if (data != null) {
                    $('#hfJamesAllen').val(data.SrNo);
                    $('#txtCaratRange1Disc').val(data.caratRange1Disc);
                    $('#txtCaratRange2Disc').val(data.caratRange2Disc);
                    $('#txtCaratRange3Disc').val(data.caratRange3Disc);
                    $('#txtCaratRange4Disc').val(data.caratRange4Disc);
                    $('#txtCaratRange5Disc').val(data.caratRange5Disc);
                    $('#txtCaratRange6Disc').val(data.caratRange6Disc);
                    $('#txtCaratRange7Disc').val(data.caratRange7Disc);
                    $('#txtCaratRange8Disc').val(data.caratRange8Disc);
                    $('#txtCaratRange9Disc').val(data.caratRange9Disc);
                    $('#txtCaratRange10Disc').val(data.caratRange10Disc);
                    $('#txtCaratRange11Disc').val(data.caratRange11Disc);
                    $('#txtCaratRange12Disc').val(data.caratRange12Disc);
                    $('#txtCaratRange13Disc').val(data.caratRange13Disc);
                    $('#txtCaratRange14Disc').val(data.caratRange14Disc);
                    $('#txtCaratRange15Disc').val(data.caratRange15Disc);
                    $('#txtCaratRange16Disc').val(data.caratRange16Disc);
                    $('#txtCaratRange17Disc').val(data.caratRange17Disc);
                    $('#txtCaratRange18Disc').val(data.caratRange18Disc);
                    $('#txtCaratRange19Disc').val(data.caratRange19Disc);
                    $('#txtCaratRange20Disc').val(data.caratRange20Disc);
                    $('#txtCaratRange21Disc').val(data.caratRange21Disc);
                    $('#txtCaratRange22Disc').val(data.caratRange22Disc);
                    $('#txtCaratRange23Disc').val(data.caratRange23Disc);
                    $('#txtCaratRange24Disc').val(data.caratRange24Disc);
                    $('#txtCaratRange25Disc').val(data.caratRange25Disc);
                    $('#txtCNRDisc').val(data.CNRDisc);
                    $('#txtHaExDisc').val(data.haExDisc);
                    $('#txtHaVgDisc').val(data.haVgDisc);
                    $('#txtCNRDiscHA').val(data.cnrDiscHA);


                    $('input[name=JamesAllen_Status][value=' + data.Isactive + ']').prop('checked', true);


                    $('#btnUpdate').show();
                    $('#btnSave').hide();
                    $('#JamesAllenStatus').show();


                } else {
                    $('#btnUpdate').hide();
                    $('#btnSave').show();
                    $('#JamesAllenStatus').hide();
                    uiApp.Alert({ container: '#uiPanel1', message: "No record found", type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Try agaiin later !", type: "danger" });
            });

        });



        $(document).on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            if ($frmJamesAllen.valid()) {
                var pData = UpdateReadForm();
                objSvc.UpdateJamesAllen(pData).then(function (data) {
                    if (data.IsSuccess && data.Result > 0) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored updated successfully", type: "success" });
                        ClearForm();
                        LoadGrid();
                        $('#btnUpdate').hide();
                        $('#btnSave').show();
                        $('#JamesAllenStatus').hide();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored not added", type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding recored", type: "danger" });
                });
            }
        });

        $(document).on('click', 'a.btnDelete', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            uiApp.Confirm('Confirm to delete this record ?', function (resp) {
                if (resp) {
                    objSvc.DeleteJamesAllen(id).then(function (data) {
                        if (data != null) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Recored deleted successfully", type: "success" });
                            LoadGrid();
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Recored not added", type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Try again later !", type: "danger" });
                    });
                }
            });

        });

    };

    var ClearForm = function () {
        $('#frmJamesAllen').find("input[type=text], textarea").val("");
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
            CNRDisc: $('#txtCNRDisc').val(),
            haExDisc: $('#txtHaExDisc').val(),
            haVgDisc: $('#txtHaVgDisc').val(),
            CNRDiscHA: $('#txtCNRDiscHA').val()
        }
    };


    var UpdateReadForm = function () {
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
            CNRDisc: $('#txtCNRDisc').val(),
            haExDisc: $('#txtHaExDisc').val(),
            haVgDisc: $('#txtHaVgDisc').val(),
            CNRDiscHA: $('#txtCNRDiscHA').val(),
            SrNo: $('#hfJamesAllen').val(),
            Isactive: $('input:radio[name=JamesAllen_Status]:checked').val()

        }
    };

    var LoadGrid = function () {
        
        if (dtMarketing.getDataTable() == null || dtMarketing.getDataTable() == undefined) {
            dtMarketing.init({
                src: '#tblMarketing',
                dataTable: {
                    //deferLoading: 0,
                    scrollX: "300px",
                    scrollY: true,
                    paging: true,
                    order: [[1, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Marketing/GridJamesAllen',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "SrNo", class: 'whspace'},
                        { data: "caratRange1Disc", class: 'whspace'},
                        { data: "caratRange2Disc", class: 'whspace'},
                        { data: "caratRange3Disc", class: 'whspace'},
                        { data: "caratRange4Disc", class: 'whspace'},
                        { data: "caratRange5Disc", class: 'whspace'},
                        { data: "caratRange6Disc", class: 'whspace'},
                        { data: "caratRange7Disc", class: 'whspace'},
                        { data: "caratRange8Disc", class: 'whspace'},
                        { data: "caratRange9Disc", class: 'whspace'},
                        { data: "caratRange10Disc", class: 'whspace'},
                        { data: "caratRange11Disc", class: 'whspace'},
                        { data: "caratRange12Disc", class: 'whspace'},
                        { data: "caratRange13Disc", class: 'whspace'},
                        { data: "caratRange14Disc", class: 'whspace'},
                        { data: "caratRange15Disc", class: 'whspace'},
                        { data: "caratRange16Disc", class: 'whspace'},
                        { data: "caratRange17Disc", class: 'whspace'},
                        { data: "caratRange18Disc", class: 'whspace'},
                        { data: "caratRange19Disc", class: 'whspace'},
                        { data: "caratRange20Disc", class: 'whspace'},
                        { data: "caratRange21Disc", class: 'whspace'},
                        { data: "caratRange22Disc", class: 'whspace'},
                        { data: "caratRange23Disc", class: 'whspace'},
                        { data: "caratRange24Disc", class: 'whspace'},
                        { data: "caratRange25Disc", class: 'whspace'},
                        { data: "CNRDisc", class: 'whspace'},
                        { data: "haExDisc", class: 'whspace'},
                        { data: "haVgDisc", class: 'whspace'},
                        { data: "cnrDiscHA", class: 'whspace'},
                        { data: "Isactive", class: 'whspace'},
                        { data: "createdOn", class: 'whspace'},
                        { data: "UpdatedOn", class: 'whspace'},
                        { data: null, class: 'whspace'},
                        { data: null, class: 'whspace'} 
                    ],
                    columnDefs: [
                        {
                            targets: [30],
                            render: function (data, type, row) {

                                return row.Isactive == 1 ? "Active" : "In Active";

                            },
                            orderable: true
                        },
                        {
                            targets: [31],
                            render: function (data, type, row) {
                                return moment(row.createdOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        },
                        {
                            targets: [32],
                            render: function (data, type, row) {
                                return row.UpdatedOn == null ? "" : moment(row.UpdatedOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        },
                        {
                            targets: [33],
                            render: function (data, type, row) {
                                return '<a class="btnEditDetails" data-id="' + row.SrNo + '"    href="#">Edit</a>';
                            },
                            orderable: true
                        },
                        {
                            targets: [34],
                            render: function (data, type, row) {
                                return '<a class="btnDelete" data-id="' + row.SrNo + '"    href="#">Delete</a>';
                            },
                            orderable: true
                        }

                    ]
                }
            });
        } else {
            dtMarketing.getDataTable().draw();
        }

    };

    var SetValidation = function () {
        $frmJamesAllen = $('#frmJamesAllen');
        var rPattern = /^100(\.0{0,2}?)?$|^\d{0,2}(\.\d{0,2})?$/;
        valJamesAllen = $frmJamesAllen.validate({
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
                CNRDisc: {
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
                },
                CNRDiscHA: {
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
    CtrlMarketingJamesAllen.init();
});
