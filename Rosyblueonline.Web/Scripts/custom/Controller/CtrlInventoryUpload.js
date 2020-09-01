var CtrlInventoryUpload = function () {
    var objSF = null, objMSvc = null, objOSvc = null;
    var dtMemo = null;
    var $frmInventorUpload = null;
    var FileID = 0;
    var LstOfCheckItems = [];
    var OrderID = null;
    var dtOrderInfoForPopup = null;

    var OnLoad = function () {
        objSF = new SearchFilter();
        objOSvc = new OrderService();
        objMSvc = new MemoService();
        dtOrderInfoForPopup = new Datatable();
        RegisterEvents();
        SetValidation();
        $('#ddlCustomer').select2({
            ajax: {
                delay: 500,
                url: '/Home/GetListOfCustomer',
                data: function (params) {
                    var query = {
                        search: params.term,
                    }
                    return query;
                },
                processResults: function (data) {
                    return {
                        results: data
                    };
                },
            },
            dropdownParent: $('#Modal-FormMemo'),
            width: 'resolve'
        });

    }

    var RegisterEvents = function () {

        $('#ddlDownloadType').change(function () {
            var dType = $(this).val();
            $('#btnDeals').attr('href', dType);
        });

        $('#btnUpload').click(function (e) {
            e.preventDefault();
            if ($frmInventorUpload.valid() == false) {
                return;
            }
            var fData = new FormData();
            fData.append('uploadFormatId', $('#ddlInventoryType').val());
            if ($('#fuUpload').get(0).files.length === 0 && demandID == 0) {
                console.log("File Not Attached");
                return;
            }
            $('#btnSaveMemo').data('Action', '');
            fData.append("File", $('#fuUpload').get(0).files[0]);

            uiApp.BlockUI();
            objSF.UploadInventory(fData).then(function (data) {
                uiApp.UnBlockUI();
                if (data.IsSuccess) {
                    if (data.Message == "MEMO_UPLOAD") {
                        GetDataForMemo(data.Result);
                        $('#btnSaveMemo').data('Action', 'CreateMemo');
                    } else if (data.Message == "MEMO_CANCEL") {
                        uiApp.Alert({ container: '#uiPanel1', message: "Memo partially cancelled", type: "success" });
                        location.href = '/Memo';
                    } else if (data.Message == "ADD_LAB" || data.Message == "REMOVE_LAB") {
                        uiApp.Alert({ container: '#uiPanel1', message: "Labs updated", type: "success" });
                    } else if (data.Message == "V360VIACERTNO") {
                        uiApp.Alert({ container: '#uiPanel1', message: "V360 URL Updated", type: "success" });
                        location.href = '/Inventory/UploadHistory';
                    } else if (data.Message == "SPLIT_MEMO") {
                        GetDataForMemo(data.Result);
                        if (data.Result.List.length > 0) {
                            OrderID = data.Result.List[0]["OrderID"];
                            $('#MemoInfoOperation').html('Memo split from following order');
                            GetMultipleInfo(OrderID);
                        }
                        $('#btnSaveMemo').data('Action', 'SplitMemo');

                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Excel uploaded", type: "success" });
                        location.href = '/Inventory/UploadHistory';
                    }
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                }
                console.log(data);
            }, function (error) {
                uiApp.UnBlockUI();
            });
        });

        $('#btnSaveMemo').click(function (e) {
            e.preventDefault();
            var actionType = $('#btnSaveMemo').data('Action');

            switch (actionType) {
                case "CreateMemo":
                    objMSvc.Book(LstOfCheckItems,
                        $('#ddlCustomer').val(),
                        $('#chkConfirmMemo').prop('checked') == true ? 1 : 0,
                        $('#chkSellDirectly').prop('checked') == true ? 1 : 0,
                        $('#txtMemoRemark').val().trim(),
                        FileID).then(function (data) {
                            if (data.IsSuccess) {
                                $('#Modal-FormMemo').modal('hide');
                                var strMsg = data.Result.validCount + " items added to memo. " + (data.Result.InvalidCount != 0 ? data.Result.InvalidCount + " items already added to memo or order" : "");
                                uiApp.Alert({ container: '#uiPanel1', message: strMsg, type: "success" });
                                //TotalCount = TotalCount - data.Result.RecentCartCount;
                                LstOfCheckItems = [];
                                ClearMemo();
                                //location.href = '/Inventory/UploadHistory';
                                location.href = '/Memo';
                            } else {
                                uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding to memo", type: "error" })
                            }
                        }, function (error) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Some Error Occured.", type: "danger" });
                        });
                    break;
                case "SplitMemo":
                    if (OrderID != 0 && OrderID != undefined && OrderID != null) {
                        objMSvc.SplitMemo(OrderID,
                            LstOfCheckItems,
                            $('#ddlCustomer').val(),
                            $('#chkConfirmMemo').prop('checked') == true ? 1 : 0,
                            $('#chkSellDirectly').prop('checked') == true ? 1 : 0,
                            $('#txtMemoRemark').val().trim(),
                            FileID).then(function (data) {
                                if (data.IsSuccess) {
                                    $('#Modal-FormMemo').modal('hide');
                                    var strMsg = "Memo Split successfully.";
                                    uiApp.Alert({ container: '#uiPanel1', message: strMsg, type: "success" });
                                    LstOfCheckItems = [];
                                    ClearMemo();
                                    //location.href = '/Inventory/UploadHistory';
                                    location.href = '/Memo';
                                } else {
                                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in spliting to memo", type: "error" })
                                }
                            }, function (error) {
                                uiApp.Alert({ container: '#uiPanel1', message: "Some Error Occured.", type: "danger" });
                            });
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Order ID is not present", type: "error" });
                    }

                    break;
            }


        });

    }

    var ClearMemo = function () {
        //Clear and reset form -- start --
        $('#ddlCustomer').val('');
        $('#ddlCustomer').trigger('change');
        $('#chkConfirmMemo').prop('checked', false);
        $('#chkSellDirectly').prop('checked', false);
        $('#txtMemoRemark').val('');
        //Clear and reset form -- end --
    }

    var SetValidation = function () {
        $frmInventorUpload = $('#frmInventorUpload');
        $frmInventorUpload.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                uploadFormatId: {
                    required: true
                },
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

    function GetDataForMemo(fileData) {
        FileID = fileData.FileID;
        LstOfCheckItems = [];
        for (var i = 0; i < fileData.List.length; i++) {
            LstOfCheckItems.push(fileData.List[i].Stock);
        }
        uiApp.BlockUI();
        objSF.GetInventoriesByLotID(LstOfCheckItems).then(function (data) {
            uiApp.UnBlockUI();
            $('#Modal-FormMemo').modal('show');
            if (data.IsSuccess == true) {
                $('#ddlCustomer').val('');
                $('#ddlCustomer').trigger('change');
                $('#chkConfirmMemo').prop('checked', false);
                $('#chkSellDirectly').prop('checked', false);
                $('#txtMemoRemark').val('');
                BindMemoTable(data.Result);
            } else {
                uiApp.Alert({ container: '#uiPanel1', message: "Problem in fetching data.", type: "danger" });
            }
        }, function (error) {
            uiApp.UnBlockUI();
            uiApp.Alert({ container: '#uiPanel1', message: "Some Error Occured.", type: "danger" });
        });
    }

    function BindMemoTable(data) {
        if (dtMemo == null && dtMemo == undefined) {
            dtMemo = new Datatable();
        }

        if (dtMemo.getDataTable() == null || dtMemo.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            var colDef = colStruct.SpecificSearch.columnDefs;
            colDef[0]["visible"] = false;
            dtMemo.init({
                src: '#MemoTable',
                dataTable: {
                    //deferLoading: 0,
                    paging: false,
                    scrollY: "270px",
                    scrollX: true,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colDef,
                },
                onCheckboxChange: function (obj) {

                }
            });
        } else {
            dtMemo.clearSelection();
            var table = dtMemo.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }

    }

    var RenderOrderInfoForMemo = function (data) {
        if (dtOrderInfoForPopup.getDataTable() == null || dtOrderInfoForPopup.getDataTable() == undefined) {
            dtOrderInfoForPopup.init({
                src: '#MemoInfoTable',
                dataTable: {
                    scrollY: "70px",
                    scrollCollapse: true,
                    paging: false,
                    order: [],
                    processing: false,
                    serverSide: false,
                    data: data,
                    language: {
                        info: ''
                    },
                    columns: [
                        { data: 'orderDetailsId', sorting: false },
                        { data: 'orderCreatedOn', sorting: false },
                        { data: 'companyName', sorting: false }
                    ],
                    columnDefs: [{
                        targets: [1],
                        render: function (data, type, row) {
                            return moment(data).format("YYYY-MM-DD");
                        }
                    }, {
                        targets: [2],
                        render: function (data, type, row) {
                            return row.companyName + ' (' + row.firstName + ' ' + row.lastName + ')';
                        }
                    }],
                },
                onCheckboxChange: function (obj) {
                    //ListMemo = obj;
                }
            });
        } else {
            dtOrderInfoForPopup.clearSelection();
            var table = dtOrderInfoForPopup.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }

    var clearOrderInfoForMemo = function () {
        $('#pnlMemoInfoWindow').hide();
        $('#MemoInfoOperation').html('');
        RenderOrderInfoForMemo([]);
    }

    var GetMultipleInfo = function (OrderIDs) {
        $('#pnlMemoInfoWindow').show();
        objOSvc.GetMultipleInfo(OrderIDs).then(function (data) {
            if (data.IsSuccess) {
                RenderOrderInfoForMemo(data.Result);
            }
        }, function (error) {
        });
    }

    return {
        int: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlInventoryUpload.int();
});