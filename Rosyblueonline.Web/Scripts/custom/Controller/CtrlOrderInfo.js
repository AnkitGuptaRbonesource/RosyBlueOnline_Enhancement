var CtrlOrderInfo = function () {
    var dtOrderItems = null;
    var objOrdSvc = new OrderService();
    var listOfItems = [];
    var OrderID = 0;
    var OnLoad = function () {
        OrderID = $('#hfOrderID').val();
        dtOrderItems = new Datatable();
        LoadForm();
    }

    var LoadForm = function () {
        objOrdSvc.Info(OrderID).then(function (response) {
            console.log(response);
            renderGrid(response.Result.OrderItemDetail);
            bindForm(response.Result.BillingAddress, response.Result.UserDetail, response.Result.OrderDetail, response.Result.Charges);
            if (response.Result.OrderDetail.orderStatus == 11 || response.Result.OrderDetail.orderStatus == 23) {
                $('#btnRemoveItem').prop('disabled', 'disabled');
                $('#btnCancel').prop('disabled', 'disabled');
                $('#btnExecute').prop('disabled', 'disabled');
            } else {
                $('#btnRemoveItem').prop('disabled', '');
                $('#btnCancel').prop('disabled', '');
                $('#btnExecute').prop('disabled', '');
            }
        }, function (error) {
        });
        renderGrid([]);
    }

    var RegisterEvent = function () {
        $('#btnRemoveItem').click(function (e) {
            e.preventDefault();
            if (listOfItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "No items selected", type: "warning" });
                return;
            }
            uiApp.Confirm('Confirm Delete?', function (resp) {
                if (resp) {
                    objOrdSvc.RemoveItemFromOrder(OrderID, listOfItems.join(',')).then(function (data) {
                        if (data.IsSuccess == true) {
                            if (data.Result > 0) {
                                LoadForm();
                                uiApp.Alert({ container: '#uiPanel1', message: "Items removed from order", type: "success" });
                            } else {
                                uiApp.Alert({ container: '#uiPanel1', message: "Items not removed from order", type: "danger" });
                            }
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                    });
                }
            });
        });

        $('#btnCancel').click(function (e) {
            e.preventDefault();
            uiApp.Confirm('Confirm Cancel?', function (resp) {
                if (resp) {
                    objOrdSvc.CancelOrder(OrderID).then(function (data) {
                        if (data.IsSuccess == true) {
                            if (data.Result > 0) {
                                uiApp.AlertPopup('Order Removed Successfully', function () {
                                    location.href = '/Order/List'
                                });
                            } else {
                                uiApp.Alert({ container: '#uiPanel1', message: "Order not removed", type: "danger" });
                            }
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                    });
                }
            });
        });

        $('#btnExecute').click(function (e) {
            e.preventDefault();
            uiApp.Confirm('Confirm Order?', function (resp) {
                if (resp) {
                    uiApp.BlockUI();
                    objOrdSvc.CompleteOrder(OrderID, $('#txtShippingCompany').val().trim(), $('#txtTrackingNumber').val().trim()).then(function (data) {
                        if (data.IsSuccess == true) {
                            if (data.Result > 0) {
                                uiApp.AlertPopup('Order Confirmed Successfully', function () {
                                    location.href = '/Order/List'
                                });
                                uiApp.UnBlockUI();
                            } else {
                                uiApp.UnBlockUI();
                                uiApp.Alert({ container: '#uiPanel1', message: "Order not confirmed", type: "danger" });
                            }
                        } else {
                            uiApp.UnBlockUI();
                            uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                        }
                    }, function (error) {
                            uiApp.UnBlockUI();
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                    });
                }
            });
        });

        $('#btnExcelDownload').click(function (e) {
            e.preventDefault();
            var OrderID = $('#hfOrderID').val();
            uiApp.BlockUI();
            objOrdSvc.GetOrderItemForExcel(OrderID).then(function (data) {
                if (data.IsSuccess) {
                    $('#ancInventoryDownload').get(0).click();
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function () {
                uiApp.UnBlockUI();
            });
        });
    }

    var renderGrid = function (data) {
        if (dtOrderItems.getDataTable() == null || dtOrderItems.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            dtOrderItems.init({
                src: '#SearchTablePost',
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
                    columnDefs: colStruct.SpecificSearch.columnDefs,
                },
                onCheckboxChange: function (obj) {
                    listOfItems = obj;
                }
            });
        } else {
            dtOrderItems.clearSelection();
            var table = dtOrderItems.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }

    var bindForm = function (objBA, objUD, objOD, objCH) {
        $('#spnFirstName').html(objBA.firstName);
        $('#spnLastName').html(objBA.lastName);
        $('#spnCompanyName').html(objBA.companyName);
        $('#spnAddress01').html(objBA.address01);
        $('#spnAddress02').html(objBA.address02);
        $('#spnCityName').html(objBA.cityName);
        $('#spnZipCode').html(objBA.zipCode);
        $('#spnStateName').html(objBA.stateName);
        $('#spnCountryName').html(objBA.countryName);

        $('#spnPhone').html('<b>' + objUD.phoneCode01 + '-' + objUD.phone01 + '</b>');
        $('#spnMobile').html('<b>' + objUD.mobile + '</b>');
        $('#spnFax').html('<b>' + objUD.fax01 + '</b>');
        $('#spnBankName').html('<b>' + objUD.bankName + '</b>');
        $('#spnBranchName').html('<b>' + objUD.branchName + '</b>');
        $('#spnBranchAddress').html('<b>' + objUD.branchAddress + '</b>');

        $('#spnShippingCompany').html(objOD.ShippingCompany == null ? '' : objOD.ShippingCompany);
        $('#spnTrackingNumber').html(objOD.TrackingNumber == null ? '' : objOD.TrackingNumber);
        $('#txtShippingCompany').val(objOD.ShippingCompany == null ? '' : objOD.ShippingCompany);
        $('#txtTrackingNumber').val(objOD.TrackingNumber == null ? '' : objOD.TrackingNumber);
        $('#spnShippingMode').html(objOD.ShipmentMode == 1 ? 'Direct Shipping' : 'Self Pickup');
        $('#spnOrderDate').html(moment(objOD.orderCreatedOn).format(myApp.dateFormat.Client));

        $("#tblOrder").html('');
        $('#tmplOrderDetail').tmpl(objCH).appendTo("#tblOrder");
        $("#tblOrder").append('<tr>\
            <td colspan = "3" align="center" style="text-align: center !important;"> Note: All Orders are in USD.</td>\
        </tr>');
    }

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlOrderInfo.init();
});
