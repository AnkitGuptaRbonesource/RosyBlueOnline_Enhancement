var CtrlMarketInventoryUpload = function () {
    var objSF = null;
    var $frmMktInventorUpload = null;
    var dtMrkupload = null;
    var dtQCMrkupload = null;
    var OnLoad = function () {
        dtMrkupload = new Datatable();
        dtQCMrkupload = new Datatable();
        objSF = new SearchFilter();
        RegisterEvents();
        SetValidation();
        FileUploadGrid();
        QCFileUploadGrid();
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd-mm-yy' 
              
        });
        $('#txtUploadDate').datepicker('setDate', 'today');
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

        $('#btnQCUpload').click(function (e) {
            e.preventDefault(); 
            if ($frmMktInventorUpload.valid() == false) {
                return;
            }
            var fData = new FormData();

            if ($('#QCfuUpload').get(0).files.length === 0) {
                console.log("File Not Attached");
                return;
            }

            fData.append("File", $('#QCfuUpload').get(0).files[0]);

            uiApp.BlockUI();
            objSF.QCUploadInventory(fData).then(function (data) {
                uiApp.UnBlockUI();
                if (data.IsSuccess) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Excel uploaded", type: "success" });
                    location.href = '/Marketing/UploadQC';

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
            var FileId = $(e.target).data('id');
            var FileName = $(e.target).data('name');

            var UploadDate = "";
            var VendorName = "";
            var CertNos = "";

            objSF.MarketdownloadForExcel(FileId, FileName, UploadDate, VendorName, CertNos).then(function (data) {
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



        $(document).on('click', '#btnDownload', function (e) {
            e.preventDefault();
            uiApp.BlockUI(); 
            var FileId = "";
            var FileName = "FilterDownload";
            var UploadDate = $('#txtUploadDate').val().trim();
            var VendorName = $('#ddlVenderName').val().trim();
            var CertNos = $('#txtCertificateNos').val().trim();

            objSF.MarketdownloadForExcel(FileId, FileName, UploadDate, VendorName, CertNos).then(function (data) {
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


    var FileUploadGrid = function () {
        uiApp.BlockUI(); 

        if (dtMrkupload.getDataTable() == null || dtMrkupload.getDataTable() == undefined) {
            dtMrkupload.init({
                src: '#tblMarketingInvUpload',
                searching: true,
                dataTable: {
                    //deferLoading: 0,
                    scrollY: "400px",
                    scrollX: true,
                    paging: true,
                    order: [[0, "desc"]], 
                    language: {
                        "search": "All Filters :"
                    },

                    ajax: {
                        type: 'Post',
                        url: '/Marketing/FileDetailForGrid',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "fileId" }, 
                        { data: "fileName" },
                        { data: "uploadStatus" },
                        { data: "createdOn" },
                        { data: "TotalInv" }, 
                        { data: "InvalidInv" },
                        { data: "validInv" },
                        { data: "QCDone" },
                        { data: "QCPending" } 
                    ],
                    columnDefs: [
                        {
                            targets: [1],
                            render: function (data, type, row) {
                                return '<a href="/Content/INV/' + row.fileName + '">' + row.fileName + '</a>';

                            } 
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                return moment(row.createdOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        },
                         
                        {
                            targets: [6],
                            render: function (data, type, row) {
                               
                                return ' <a class="MarketDownload" data-id="' + row.fileId + '" data-name="Valid" href="#">' + row.validInv + '</a>';

                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                return ' <a class="MarketDownload" data-id="' + row.fileId + '" data-name="QCDone" href="#">' + row.QCDone + '</a>';

                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                return ' <a class="MarketDownload" data-id="' + row.fileId + '" data-name="QCPending" href="#">' + row.QCPending + '</a>';

                            }
                        }
                    ]
                },
                onCheckboxChange: function (obj) {
                    CheckOrder(obj)
                }
            });
            uiApp.UnBlockUI();
        } else {
            dtMrkupload.getDataTable().draw();
            uiApp.UnBlockUI();
        }
        uiApp.UnBlockUI();
    };


    var QCFileUploadGrid = function () {
        uiApp.BlockUI();

        if (dtQCMrkupload.getDataTable() == null || dtQCMrkupload.getDataTable() == undefined) {
            dtQCMrkupload.init({
                src: '#tblQCInvUpload',
                searching: true,
                dataTable: {
                    //deferLoading: 0,
                    scrollY: "400px",
                    scrollX: true,
                    paging: true,
                    order: [[0, "desc"]],
                    language: {
                        "search": "All Filters :"
                    },

                    ajax: {
                        type: 'Post',
                        url: '/Marketing/QCFileDetailForGrid',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "fileId" },
                        { data: "fileName" },
                        { data: "uploadStatus" },
                        { data: "createdOn" },
                        { data: "validInv" }  
                    ],
                    columnDefs: [
                        {
                            targets: [1],
                            render: function (data, type, row) {
                                return '<a href="/Content/INV/' + row.fileName + '">' + row.fileName + '</a>';

                            }
                        },
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                return moment(row.createdOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        }  
                    ]
                },
                onCheckboxChange: function (obj) {
                    CheckOrder(obj)
                }
            });
            uiApp.UnBlockUI();
        } else {
            dtQCMrkupload.getDataTable().draw();
            uiApp.UnBlockUI();
        }
        uiApp.UnBlockUI();
    };




    return {
        int: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlMarketInventoryUpload.int();
});