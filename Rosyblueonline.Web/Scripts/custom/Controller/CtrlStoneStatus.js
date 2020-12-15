var CtrlStoneStatus = function () {
    var objColStr = null, objSF = null;
    var dtSearchPanel = null;
    var objColumns = null, objColumnDefs = null;

    var OnLoad = function () {
        objSF = new SearchFilter();
        objColStr = new DataTableColumnStruct('SpecificSearch');
        dtSearchPanel = new Datatable();
        objColumns = objColStr.SpecificSearch.columns;
        objColumns.splice(0, 1);

        objColumns.splice(1, 0, { data: "Stockstatus", width: "200px" });
        objColumns.splice(objColumns.length, 0, { data: "solddate" });
        objColumnDefs = objColStr.SpecificSearch.columnDefs;
        objColumnDefs.splice(1, 0, {
            targets: 1,
            render: function (data, type, row) {
                return row.refdata;
                //var Text = "";
                //switch (row.Stockstatus) {
                //    case "Active Inventory":
                //        Text = "Available";
                //        break;
                //    case "Inventory On Memo":
                //        Text = "On Memo : " + row.refdata;
                //        break;
                //    case "Order pending (order is waiting for admin approval)":
                //        Text = "Order pending : " + row.refdata;
                //        break;
                //    case "Sold Inventory":
                //        Text = "Sold : " + row.refdata;
                //        break;
                //    default:
                //        Text = row.Stockstatus;
                //        break;
                //}
                //return Text;
            },
            orderable: true,
            width: "500px"
        });
        objColumnDefs.splice(0, 1);
        objColumns.splice(1, 1);
        objColumns.splice(1, 0, {
            data: "refdata"
        });
        //console.log(objColumns);
        //LoadGrid();
    }

    var RegisterEvent = function () {
        $('#btnSubmit').click(function (e) {
            e.preventDefault();
            var objFD = new FormData();
            if ($('#fuName').get(0).files.length != 0) {
                objFD.append('file', $('#fuName').get(0).files[0]);
                objSF.UploadStoneStatus(objFD).then(function (data) {
                    if (data.IsSuccess == true) {
                        LoadGrid(data.Result, "", $('input[name=optradio]:checked').val());
                    }
                }, function (error) {
                    console.log(error);
                });
            } else {
                LoadGrid("", $('#txtLotNo').val().trim(), $('input[name=optradio]:checked').val());
            }
        });

        $('#txtLotNo').change(function () {
            $('#fuName').val('');
        });

        $('#txtLotNo').on('keyup', function () {
            Comma($(this).val(), this);
        });

        $('#fuName').change(function () {
            $('#txtLotNo').val('');
        });

        $('#btnExcel').click(function (e) {
            e.preventDefault();
            var objFD = new FormData();
            if ($('#fuName').get(0).files.length != 0) {
                objFD.append('file', $('#fuName').get(0).files[0]);
                objSF.UploadStoneStatus(objFD).then(function (data) {
                    if (data.IsSuccess == true) {
                        ExcelDownload(data.Result, "", $('input[name=optradio]:checked').val());
                    }
                }, function (error) {
                    console.log(error);
                });
            } else {
                ExcelDownload("", $('#txtLotNo').val().trim(), $('input[name=optradio]:checked').val());
            }
        });
    }

    function Comma(Num, ele) { //function to add commas to textboxes
        var x1 = Num.split(' ').join(',');
        $(ele).val(x1);
    }

    var LoadGrid = function (FileName, LotNos, Type) {
        dtSearchPanel.setAjaxParam('Type', Type);
        dtSearchPanel.setAjaxParam('LotNos', LotNos);
        dtSearchPanel.setAjaxParam('FileName', FileName);
        if (dtSearchPanel.getDataTable() == null || dtSearchPanel.getDataTable() == undefined) {
            dtSearchPanel.init({
                src: '#SearchTablePost',
                dataTable: {
                    paging: true,
                    order: [[1, "desc"]],
                    scrollY: "485px",
                    scrollX: true,
                    ajax: {
                        type: 'Post',
                        url: '/Inventory/GetStoneStatus',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: objColumns,
                    columnDefs: objColumnDefs,
                },
                onCheckboxChange: function (obj) {
                }
            });
        } else {
            dtSearchPanel.clearSelection();
            dtSearchPanel.getDataTable().draw();
        }
    }

    var ExcelDownload = function (FileName, LotNos, Type) {
        objSF.ExportToExcelStoneStatus(LotNos, FileName, Type).then(function (data) {
            if (!data.IsSuccess) {
                uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
            }
            else {
                $('#ancDownloadExcel').get(0).click();
            }
        }, function () {
            uiApp.Alert({ container: '#uiPanel1', message: "Problem in downloading excel", type: "danger" });
        })
    }

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlStoneStatus.init();
});