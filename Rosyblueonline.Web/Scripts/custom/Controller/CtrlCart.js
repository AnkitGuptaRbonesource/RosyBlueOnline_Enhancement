var CtrlCart = function () {
    var objSvc = null, objOrdSvc = null, objWLSvc = null, objSF = null;
    var dtCart = null, dtCartForOrder = null;;
    var cartData = null;
    var LstOfCheckItems = [];
    var objModBA = null, objModSA = null;
    var OrderDetail = {
        LotNos: '',
        CustomerId: 0,
        ShippingMode: 0,
        BillingID: 0,
        ShippingID: 0
    };
    var SummeryTemp = '<tr class="rsearchnew" style="height: 25px;">\
        <td class="rsearchnew"><b><span id="lblcountsel">${TotalPcs}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblTCt1">${TotalCarat}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblNetORapValue">${AvgRapPerCT}</span></b></td>\
        <td class="rsearchnew"><b><span id="lbltotRap">${TotalRap}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblNetRapPer">${AvgRapoff}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblavgpriceafterdis">${PricePerct}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblfinalamoutpayment">${PayableAmount}</span></b></td>\
        </tr>';

    var SummeryTempForOrder = '<tr style="height: 25px;">\
        <td class="whspace"><b><span>${TotalPcs}</span></b></td>\
        <td class="whspace"><b><span>${TotalCarat}</span></b></td>\
        <td class="whspace"><b><span>${TotalRap}</span></b></td>\
        <td class="whspace"><b><span>${AvgRapoff}</span></b></td>\
        <td class="whspace"><b><span>${PricePerct}</span></b></td>\
        <td class="whspace"><b><span>${PayableAmount}</span></b></td>\
        </tr>';

    var OnLoad = function () {

        objOrdSvc = new OrderService();
        objSF = new SearchFilter();
        objSvc = new CartService();
        objWLSvc = new WatchListService();
        dtCartForOrder = new Datatable();
        dtCart = new Datatable();
        objModBA = new ModuleAddressManager({
            parentID: '#divBillingAddress',
            type: 'billing',
            instanceID: 1,
            enableSelect: true,
            onSelect: function (ent, val) {
                OrderDetail["BillingID"] = val;
            },
            onReload: function (ent, val) {

            }
        });
        objModSA = new ModuleAddressManager({
            parentID: '#divShippingAddress',
            type: 'shipping',
            instanceID: 2,
            enableSelect: true,
            onSelect: function (ent, val) {
                OrderDetail["ShippingID"] = val;
            },
            onReload: function (ent, val) {

            }
        });

        LoadData();
        RegisterEvent();
    }

    var initModuleAddressManager = function () {
        objModBA.init();
        objModSA.init();
    }

    var RegisterEvent = function () {
        $('#btnRemove').click(function (e) {
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "No items selected", type: "warning" });
                return;
            }
          
            uiApp.Confirm('Confirm Delete?', function (resp) {
                if (resp) {
                    uiApp.BlockUI();
                    objSvc.RemoveCart(LstOfCheckItems).then(function (data) {
                        if (data.IsSuccess == true) {
                            uiApp.Alert({ container: '#uiPanel1', message: data.Result.RecentCartCount + " Item(s) removed", type: "success" });

                            setTimeout(function () {
                                location.reload();
                            }, 1000);
                            // LoadData();
                            
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Items not removed", type: "error" });
                            uiApp.UnBlockUI();
                        }
                    }, function (error) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Problem in removing items", type: "error" });
                            uiApp.UnBlockUI();
                    });
                }
            });
        });

        $('#btnOrder').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "No items selected", type: "warning" });
                return;
            }
            uiApp.Confirm('Confirm Order?', function (resp) {
                if (resp) {
                    objOrdSvc.PreBookOrder(LstOfCheckItems.join(','), $('#ddlShippingMode').val()).then(function (data) {
                        if (data.IsSuccess == true) {
                            console.log(data.Result);
                            renderBookingData(data.Result.Charges);
                            renderDataForOrder();
                            $('#pnlOrderConfirmation').show();
                            $('#pnlSSResult').hide();
                            uiApp.scrollTo($('#pnlOrderConfirmation'), 10);
                            initModuleAddressManager();

                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Order not fetched", type: "error" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Problem in fetching order", type: "error" });
                    });
                }
            });
        });

        $('#ddlShippingMode').change(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "No items selected", type: "warning" });
                return;
            }
            objOrdSvc.PreBookOrder(LstOfCheckItems.join(','), $('#ddlShippingMode').val()).then(function (data) {
                if (data.IsSuccess == true) {
                    console.log(data.Result);
                    renderBookingData(data.Result.Charges);
                    renderDataForOrder();
                    //ReloadCount();
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "Order not fetched", type: "error" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Problem in fetching order", type: "error" });
            });
        });

        $('#btnConfirmOrder').click(function (e) {
            e.preventDefault();
            uiApp.Confirm('Re-Confirm Order?', function (confirmResp) {
                if (confirmResp) {
                   
                    if (LstOfCheckItems.length == 0) {
                        uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "warning" });
                        return;

                    }
                    if (OrderDetail.BillingID == 0) {
                        uiApp.Alert({ container: '#uiPanel2', message: "Please select any billing address", type: "warning" });
                        return;
                    }
                    if (OrderDetail.ShippingID == 0) {
                        uiApp.Alert({ container: '#uiPanel2', message: "Please select any shipping address", type: "warning" });
                        return;
                    }
                    uiApp.BlockUI();
                    OrderDetail.LotNos = LstOfCheckItems.join(',');
                    OrderDetail.ShippingMode = $('#ddlShippingMode').val();
                    objOrdSvc.BookOrder(OrderDetail).then(function (response) {
                        if (response.IsSuccess) {
                            LoadData();
                            $('#pnlOrderConfirmation').hide();
                            $('#pnlSSResult').show();
                            uiApp.scrollTo($('#pnlSSResult'), 10);
                            //var Message = 'Order created with order # ' + response.Result.OrderId + '.<br/> Valid Count: ' + response.Result.validCount + " Invalid count: " + response.Result.InvalidCount;
                            var Message = 'Order placed successfully. You will get an email shortly';
                            uiApp.Alert({ container: '#uiPanel1', message: Message, type: "success" });
                            uiApp.UnBlockUI();
                            setTimeout(function () {
                                location.reload();
                            }, 1000);
                        }
                        else {
                            uiApp.UnBlockUI();
                            uiApp.Alert({ container: '#uiPanel1', message: "Order not created", type: "error" });
                        }
                    }, function myfunction() {
                            uiApp.UnBlockUI();
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                }
            })
        });

        $('#btnCancelOrder').click(function (e) {
            e.preventDefault();
            $('#pnlOrderConfirmation').hide();
            $('#pnlSSResult').show();
            uiApp.scrollTo($('#pnlSSResult'), 10);
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

            objSF.GetInventory(LstOfCheckItems).then(function (data) {
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
            objSF.ExportToExcelInventory('LOTNO~' + LstOfCheckItems.join(','), $('#hfNewArrival').val()).then(function (data) {
                if (data.IsSuccess) {
                    //window.open('/Inventory/ExportToExcelInventoryDownload');
                    //var locItem = window.location.href.split('/');
                    //locItem.splice(locItem.length - 1, 1);
                    //locItem.splice(locItem.length - 1, 1);
                    //var loc = locItem.join('/') + "/Inventory/ExportToExcelInventoryDownload";
                    //window.open(
                    //    loc,
                    //    "Download File",
                    //    "resizable,scrollbars,status"
                    //);
                    $('#ancDownloadExcel').get(0).click();
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
            for (var i = 0; i < cartData.length; i++) {
                lotNos = lotNos == "" ? cartData[i].LotNumber : lotNos + "," + cartData[i].LotNumber;
            }
            objSF.ExportToExcelInventory('LOTNO~' + lotNos, $('#hfNewArrival').val()).then(function (data) {
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

    var renderData = function (data) {
        cartData = data;
        if (dtCart.getDataTable() == null || dtCart.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();

            dtCart.init({
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
                    $('#pnlOrderConfirmation').hide();
                }
            });
        } else {
            dtCart.clearSelection();
            var table = dtCart.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }

    var renderDataForOrder = function () {
        var chkedData = [];
        for (var i = 0; i < LstOfCheckItems.length; i++) {
            for (var j = 0; j < cartData.length; j++) {
                if (LstOfCheckItems[i] == cartData[j].Stock) {
                    chkedData.push(cartData[j]);
                    break;
                }
            }
        }


        if (dtCartForOrder.getDataTable() == null || dtCartForOrder.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            //var cols =
            colStruct.SpecificSearch.columns.splice(0, 1);
            //var colDefs =
            colStruct.SpecificSearch.columnDefs.splice(0, 1);
            dtCartForOrder.init({
                src: '#SearchTablePostForOrder',
                dataTable: {
                    scrollY: "300px",
                    scrollX: true,
                    paging: false,
                    order: [[0, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: chkedData,
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colStruct.SpecificSearch.columnDefs,

                },
                onCheckboxChange: function () {

                }
            });
        } else {
            dtCartForOrder.clearSelection();
            var table = dtCartForOrder.getDataTable();
            table.clear().draw();
            for (var i = 0; i < chkedData.length; i++) {
                table.row.add(chkedData[i]);
            }
            table.draw(false);
        }
    }

    function ListOfChecks(obj) {
        LstOfCheckItems = obj;
        console.log('Changed LstOfCheckItems', LstOfCheckItems);
        if (obj.length > 0) {
            objSF.SummaryData(obj).then(function (data) {
                $("#tblBodySummary").html('');
                $('#tblBodySummaryForOrder').html('');
                data.Result.TotalPcs = obj.length;

                data.Result.AvgRapPerCT = data.Result.AvgRapPerCT.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.AvgRapoff = data.Result.AvgRapoff.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.PayableAmount = data.Result.PayableAmount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.PricePerct = data.Result.PricePerct.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.TotalCarat = data.Result.TotalCarat.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.TotalRap = data.Result.TotalRap.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                $.tmpl(SummeryTemp, data.Result).appendTo("#tblBodySummary");
                $.tmpl(SummeryTempForOrder, data.Result).appendTo('#tblBodySummaryForOrder');
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
            $.tmpl(SummeryTempForOrder, data.Result).appendTo('#tblBodySummaryForOrder');
        }
    }

    function LoadData() {
        ReloadNavigationCount();
        objSvc.GetCart().then(function (data) {
            renderData(data.Result);

        }, function (error) {
        });
    }

    function renderBookingData(data) {
        $("#tblOrder").html('');
        $('#tmplOrderDetail').tmpl(data).appendTo("#tblOrder");
        $("#tblOrder").append('<tr>\
            <td colspan = "3" align="center"> Note: All Orders are in USD.</td>\
        </tr>')
    }

    function ReloadNavigationCount() {
        if (CtrlMasterLayout != undefined) {
            CtrlMasterLayout.LoadCounts();
        }
    }

    return {
        init: function () {
            OnLoad();
        }
    }
}();

$(document).ready(function () {
    CtrlCart.init();
    //var cart = new CtrlCart();
});