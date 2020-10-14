var CtrlWatchList = function () {
    var objCartSvc = null, objWLSvc = null, objSFSvc = null;
    var dtWL = null;
    var watchListData = null;
    var LstOfCheckItems = [];
    var SummeryTemp = '<tr class="rsearchnew" style="height: 25px;">\
        <td class="rsearchnew"><b><span id="lblcountsel">${TotalPcs}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblTCt1">${TotalCarat}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblNetORapValue">${AvgRapPerCT}</span></b></td>\
        <td class="rsearchnew"><b><span id="lbltotRap">${TotalRap}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblNetRapPer">${AvgRapoff}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblavgpriceafterdis">${PricePerct}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblfinalamoutpayment">${PayableAmount}</span></b></td>\
        </tr>';

    var OnLoad = function () {
        objCartSvc = new CartService();
        objWLSvc = new WatchListService();
        objSFSvc = new SearchFilter();
        dtWL = new Datatable();
        LoadData();
    }


    function LoadData() {
        objWLSvc.GetWatchList().then(function (data) {
            renderData(data.Result);
        }, function (error) {
        });
    }

    var renderData = function (data) {
        watchListData = data;
        if (dtWL.getDataTable() == null || dtWL.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            dtWL.init({
                src: '#SearchTablePost',
                dataTable: {
                    //deferLoading: 0,
                    scrollY: "300px",
                    scrollX: true,
                    paging: true,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colStruct.SpecificSearch.columnDefs,
                },
                onCheckboxChange: function (obj) {
                    ListOfChecks(obj);
                }
            });
        } else {
            dtWL.clearSelection();
            var table = dtWL.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }

    var ListOfChecks = function (obj) {
        LstOfCheckItems = obj;
        if (obj.length > 0) {
            objSFSvc.SummaryData(obj).then(function (data) {
                $("#tblBodySummary").html('');
                data.Result.TotalPcs = obj.length;
                $.tmpl(SummeryTemp, data.Result).appendTo("#tblBodySummary");
            }, function (error) {
            });
        } else {
            $("#tblBodySummary").html('');
            var data = {
                Result: {
                    TotalPcs: obj.length
                }
            };
            $.tmpl(SummeryTemp, data.Result).appendTo("#tblBodySummary");
        }
    }

    var RegisterEvent = function () {
        $('#btnRemove').click(function (e) {
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "No items selected", type: "warning" });
                return;
            }
            uiApp.Confirm('Confirm Delete?', function (resp) {
                if (resp) {
                    objWLSvc.RemoveWatchList(LstOfCheckItems).then(function (data) {
                        if (data.IsSuccess == true) {
                            uiApp.Alert({ container: '#uiPanel1', message: data.Result.RecentWatchCount + " Item(s) removed", type: "success" });
                            LoadData();
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Items not removed", type: "error" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Problem in removing items", type: "error" });
                    });
                }
            });
        });

        $('#btnAddToCart').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "No items selected", type: "warning" });
                return;
            }
            uiApp.Confirm('Do you want to add the selected items to cart?', function (resp) {
                if (resp) {
                    objCartSvc.AddtoCart(LstOfCheckItems).then(function (data) {
                        if (data.IsSuccess) {
                            var strMsg = data.Result.RecentCartCount + " items to cart. " + (data.Result.CartExist != 0 ? data.Result.CartExist + " items already added to cart" : "");
                            uiApp.Alert({ container: '#uiPanel1', message: strMsg, type: "success" });
                            LoadData();
                            CtrlMasterLayout.LoadCounts();
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding to cart", type: "error" })
                        }
                    }, function (error) {
                    });
                }
            });
        });

        $('#btnCompare').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length <= 1) {
                uiApp.Alert({ container: '#uiPanel1', message: "Need to select at least 2 items to compare", type: "warning" });
                return;
            }
            if (LstOfCheckItems.length > 5) {
                uiApp.Alert({ container: '#uiPanel1', message: "Can show only 5 items.", type: "warning" });
                return;
            }

            objSFSvc.GetInventory(LstOfCheckItems).then(function (data) {
                if (data.IsSuccess == true) {
                    $('#divCompareModal').modal('show');
                    $('#tblCompare').html('');
                    //$.tmpl(CompareTemp, data.Result).appendTo('#tblCompare');
                    $('#tmpCompare').tmpl(data.Result).appendTo('#tblCompare');
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in fetching data.", type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some Error Occured.", type: "danger" });
            });
        });

        $('#btnExcel').click(function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            objSFSvc.ExportToExcelInventory('LOTNO~' + LstOfCheckItems.join(','), false).then(function (data) {
                if (data.IsSuccess) {
                    //window.open('/Inventory/ExportToExcelInventoryDownload');
                    var locItem = window.location.href.split('/');
                    locItem.splice(locItem.length - 1, 1);
                    locItem.splice(locItem.length - 1, 1);
                    var loc = locItem.join('/') + "/Inventory/ExportToExcelInventoryDownload";
                    window.open(
                        loc,
                        "Download File",
                        "resizable,scrollbars,status"
                    );
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                uiApp.UnBlockUI();
            });
        });

        $('#btnAllExcel').click(function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            var lotNos = "";
            for (var i = 0; i < watchListData.length; i++) {
                lotNos = lotNos == "" ? watchListData[i].LotNumber : lotNos + "," + watchListData[i].LotNumber;
            }
            objSFSvc.ExportToExcelInventory('LOTNO~' + lotNos, false).then(function (data) {
                if (data.IsSuccess) {
                    //window.open('/Inventory/ExportToExcelInventoryDownload');
                    var locItem = window.location.href.split('/');
                    locItem.splice(locItem.length - 1, 1);
                    locItem.splice(locItem.length - 1, 1);
                    var loc = locItem.join('/') + "/Inventory/ExportToExcelInventoryDownload";
                    window.open(
                        loc,
                        "Download File",
                        "resizable,scrollbars,status"
                    );
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                uiApp.UnBlockUI();
            });
        });
    }

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlWatchList.init();
});