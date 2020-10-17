var CtrlMemo = function () {
    var dtOrder = null;
    var dtOrderItem = null;
    var dtOrderItemForPopup = null;
    var dtOrderInfoForPopup = null;
    var objOSvc = null, objMSvc = null, objSFSvc = null;
    var ListMemo = [];
    var intTotalRowCount = 0;
    var ListOrders = [];
    var $SellFullMemo = null;
    var ValSellFullMemo = null;
    var Refname = "";

    var OnLoad = function () {
        dtOrder = new Datatable();
        dtOrderItem = new Datatable();
        dtOrderItemForPopup = new Datatable();
        dtOrderInfoForPopup = new Datatable();
        objOSvc = new OrderService();
        objMSvc = new MemoService();
        objSFSvc = new SearchFilter();

       //  dtOrder.destroy();

      
        ReadUrl();
         
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
        $('#ddlCustomer2').select2({
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
            dropdownParent: $('#Modal-EditMemo'),
            width: 'resolve'
        });

        RegisterEvent();
        SetValidation();
       // GetMemoOrderID();
    };

    var RegisterEvent = function () {
         
         
        $(document).on('click', 'a.ancShowItems', function (e) {
            e.preventDefault();
            var OrderID = $(e.target).attr('data-id');
            //var id = $(e.target).data('id');
            var name = $(e.target).data('name');
            var disc = $(e.target).data('disc');
            $('#hfIsSellPartial').val('false');
            $('#Modal-SellFullMemo-Message').html('<b>Ref : ' + name + '</b>');
            Refname = name;
            //$('#txtMemoAvgDiscount').val(disc);
            $('#hfOrderID').val(OrderID);
            LoadOrderItems(OrderID);
        });

        $('#btnCancelMemo').click(function (e) {
            e.preventDefault();
            var OrderID = $('#hfOrderID').val();
            if (ListMemo.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "Please select any item", type: "warning" });
                return;
            }
            if (ListMemo.length == intTotalRowCount) {
                uiApp.Alert({ container: '#uiPanel1', message: "Cannot select all item", type: "warning" });
                return;
            }
            uiApp.Confirm('Confirm cancellation of ' + ListMemo.length + ' items ?', function (resp) {
                if (resp) {
                    objMSvc.PartialCancel(OrderID, ListMemo).then(function (data) {
                        if (data.IsSuccess == true) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Partial cancel completed", type: "success" });
                            LoadOrderItems(OrderID);
                            dtOrder.getDataTable().draw();
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo not fetched", type: "error" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                }
            });
        });

        $('#btnBackMemo').click(function (e) {
            e.preventDefault();
            clearOnBack();
        });

        $('#btnSplitMemo').click(function (e) {
            e.preventDefault();
            var OrderID = $('#hfOrderID').val();
            if (ListMemo.length == 0) {
                uiApp.Alert({ container: '#uiPanel1', message: "Please select any item", type: "warning" });
                return;
            }
            if (ListMemo.length == intTotalRowCount) {
                uiApp.Alert({ container: '#uiPanel1', message: "Cannot select all item", type: "warning" });
                return;
            }
            uiApp.Confirm('Confirm split of ' + ListMemo.length + ' items ?', function (resp) {
                if (resp) {
                    GetOrderItemsForSplit();
                    $('#MemoInfoOperation').html('Memo split from following order');
                    GetMultipleInfo(OrderID);
                }
            });
        });

        $('#btnSaveMemo').click(function (e) {
            e.preventDefault();
            var OrderID = $('#hfOrderID').val();
            var CustomerID = $('#ddlCustomer').val();
            var Remark = $('#txtMemoRemark').val().trim();
            var isConfirmed = $('#chkConfirmMemo').prop('checked') ? 1 : 0;
            var isSellDirect = $('#chkSellDirectly').prop('checked') ? 1 : 0;
            var actionType = $('#btnSaveMemo').data('Action');

            switch (actionType) {
                case "SplitMemo":
                    objMSvc.SplitMemo(OrderID, ListMemo, CustomerID, isConfirmed, isSellDirect, Remark).then(function (data) {
                        if (data.IsSuccess == true && data.Result > 0) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo split successfully", type: "success" });
                            //LoadOrderItems(OrderID);
                            dtOrder.getDataTable().draw();
                            clearOnBack();
                            $('#Modal-FormMemo').modal('hide');
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo not split", type: "error" });
                        }
                        clearOrderInfo();
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                    break;
                case "MergeMemo":
                    objMSvc.MergeMemo(CustomerID, isConfirmed, isSellDirect, Remark, ListOrders).then(function (data) {
                        if (data.IsSuccess == true && data.Result > 0) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo merged successfully", type: "success" });
                            dtOrder.getDataTable().draw();
                            $('#Modal-FormMemo').modal('hide');
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo not merged", type: "error" });
                        }
                        clearOrderInfo();
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                    break;
                default:
                    break;
            }
        });

        $('#btnMergeMemo').click(function (e) {
            e.preventDefault();
            clearMemo();
            objOSvc.GetOrderItemsByOrderID(ListOrders.join(',')).then(function (data) {
                $('#Modal-FormMemo').modal('show');
                $('#Modal-FormMemo h4.modal-title').html('Merge Memo');
                $('#btnSaveMemo').data('Action', 'MergeMemo');
                $('#Modal-FormMemo').one('shown.bs.modal', function (e) {
                    if (data.IsSuccess == true) {
                        RenderOrderItemsInPopup(data.Result);
                        $('#MemoInfoOperation').html('Memo merged from following order');
                        GetMultipleInfo(ListOrders.join(','));
                    } else {
                        uiApp.Alert({ container: '#uiPanel2', message: "Problem in fetching data.", type: "danger" });
                    }
                });
                $("#Modal-FormMemo").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }, function (error) {
            });
        });

        $('#btnMemoReturn').click(function (e) {
            e.preventDefault();
            if (ListOrders.length > 0) {
                objMSvc.Return(ListOrders.join(',')).then(function (data) {
                    if (data.IsSuccess == true && data.Result > 0) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Sell return successfully", type: "success" });
                        dtOrder.clearSelection();
                        dtOrder.getDataTable().ajax.reload();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                });
            } else {
                uiApp.Alert({ container: '#uiPanel1', message: "No orders selected", type: "error" });
            }
        });

        $(document).on('click', 'a.btnSellFullMemo', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            var name = $(e.target).data('name');
            var disc = $(e.target).data('disc');
            $('#hfIsSellPartial').val('false');
            $('#Modal-SellFullMemo-Message').html('<b>Ref : ' + name + '</b>');
            Refname = name;
            $('#txtMemoAvgDiscount').val(disc);
            $('#hfOrderID').val(id);
            $('#Modal-SellFullMemo').modal('show');

        });

        $(document).on('click', 'a.btnCancelFullMemo', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            var cName = $(e.target).data('cname');

            uiApp.Confirm(('Confirm cancel memo of  <b>' + cName + '</b>?'), function (resp) {
                if (resp) {
                    objMSvc.CancelFullMemo(id).then(function (data) {
                        if (data.IsSuccess && data.Result > 0) {
                            dtOrder.getDataTable().draw();
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo cancelled successfully", type: "success" });
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo not cancelled", type: "error" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                }
            });
        });

        $(document).on('click', 'a.btnEditMemo', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            var index = $(e.target).data('index');
            var oData = dtOrder.getDataTable().rows().data()[index];
            console.log(oData);
            uiApp.BlockUI(); 
            Refname = (oData.companyName + '|' + oData.firstName + ' ' + oData.lastName);
            objOSvc.AllowEditCustomer(oData.createdBy).then(function (rData) { 
                 var  newOption = new Option((oData.companyName + ' | ' + oData.firstName + ' ' + oData.lastName), oData.loginID, false, false);
                $('#Modal-EditMemo-Message').html('<b>Ref : ' + (oData.companyName + '|' + oData.firstName + ' ' + oData.lastName) + '</b>');
                Refname = (oData.companyName + '|' + oData.firstName + ' ' + oData.lastName);
               
                $('#ddlCustomer2').empty();
                $('#ddlCustomer2').append(newOption).trigger('change');
                $('#hfOrderID').val(id);
                $('#txtEditMemoRemark').val(oData.remark);
                $('#Modal-EditMemo').modal('show');
                //$('#ddlCustomer2').prop('disabled', rData ? '' : 'disabled');
                uiApp.UnBlockUI();
            }, function () {
                uiApp.UnBlockUI();
            });

        });

        $(document).on('click', 'a.btnPrintMemoItems', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            objMSvc.GetStockForPrintMemo(id).then(function (data) {
                if (data.IsSuccess == true) {
                    $('#tblBodyPrint').html('');
                    var pData = JSON.parse(data["Result"]);
                    uiApp.BindPrintTable(pData);
                }
            }, function (error) {
            });
        });

        $('#chkWebMode').click(function (e) {
            $('#pnlMemoSales').hide();
            $('#txtMemoSaleDiscount').val('0');
        });

        $('#chkSalesMode').click(function (e) {
            $('#pnlMemoSales').show();
        });

        $('#btnSaleMemo').click(function (e) {
            e.preventDefault();
            var MemoMode = $('input[type=radio][name=MemoMode]:checked').val();
            var OrderID = $('#hfOrderID').val();
            var SaleDiscount = $('#txtMemoSaleDiscount').val();
            if (MemoMode == 2 && $SellFullMemo.valid() == false) {
                return;
            }

            uiApp.Confirm(('Confirm Sale for <b>' + Refname + '</b>?'), function (resp) {
                if (resp) {
                    if ($('#hfIsSellPartial').val() == "false") {
                        objMSvc.SellFullMemo(OrderID, MemoMode, SaleDiscount).then(function (data) {
                            if (data.IsSuccess && data.Result > 0) {
                                dtOrder.getDataTable().draw();
                                uiApp.Alert({ container: '#uiPanel1', message: "Memo sold successfully", type: "success" });
                                clearSaleMemo();
                                $('#Modal-SellFullMemo').modal('hide');
                                clearOnBack();
                            } else {
                                uiApp.Alert({ container: '#uiPanel1', message: "Memo not sold", type: "error" });
                            }
                        }, function (error) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                        });
                    } else {
                        objMSvc.PartialSellMemo(OrderID, ListMemo, MemoMode, SaleDiscount).then(function (data) {
                            if (data.IsSuccess && data.Result > 0) {
                                dtOrder.getDataTable().draw();
                                //LoadOrderItems(OrderID);
                                uiApp.Alert({ container: '#uiPanel1', message: "Partial Memo sold successfully", type: "success" });
                                clearSaleMemo();
                                $('#Modal-SellFullMemo').modal('hide');
                                clearOnBack();
                            } else {
                                uiApp.Alert({ container: '#uiPanel1', message: "Memo not sold", type: "error" });
                            }
                        }, function (error) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                        });
                    }
                }
            });

        });

        $('#btnUpdateMemo').click(function (e) {
            e.preventDefault();
            var CustomerID = $('#ddlCustomer2').val();
            var OrderID = $('#hfOrderID').val();
            var Remark = $('#txtEditMemoRemark').val();
            uiApp.Confirm('Confirm Update for <b>' + Refname + '</b>?', function (resp) {
                if (resp) {
                    objMSvc.UpdateMemo(OrderID, CustomerID, Remark).then(function (data) {
                        if (data.IsSuccess == true && data.Result > 0) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo updated successfully", type: "success" });
                            dtOrder.getDataTable().ajax.reload();
                            dtOrder.clearSelection();
                            $('#Modal-EditMemo').modal('hide');
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo not updated", type: "error" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                }
            });
        });

        $('#btnViewSoldHistory').click(function (e) {
            e.preventDefault();
            if ($(this).data('toggle') == "ViewSoldHistory") {
                $(this).data('toggle', 'HideSoldHistory')
                $(this).html('View Sold History');
                $('#hfOStatus').val('Pending');
                $('#divReturnSale').hide();
                dtOrder.setAjaxParam('OStatus', 'Pending');
                $('#panelHeading').html('Memo Pending');
            } else {
                $(this).data('toggle', 'ViewSoldHistory')
                $(this).html('View Pending Order');
                $('#hfOStatus').val('Completed');
                $('#divReturnSale').show();
                dtOrder.setAjaxParam('OStatus', 'Completed');
                $('#panelHeading').html('Memo Sold');
            }
            dtOrder.getDataTable().draw();
        });

        $(document).on('click', 'a.btnExportMemo', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            var index = $(e.target).data('index');
            var oData = dtOrder.getDataTable().rows().data()[index];
            uiApp.BlockUI();
            objOSvc.GetMemoItemForExcel(id, $('#hfOStatus').val()).then(function (data) {
                if (data.IsSuccess) {
                    $('#ancInventoryDownload').get(0).click();
                    //window.open('/Inventory/ExportToExcelInventoryDownload');
                    //var locItem = window.location.href.split('/');
                    //locItem.splice(locItem.length - 1, 1);
                    //var loc = locItem.join('/') + "/Inventory/ExportToExcelInventoryDownload";
                    //window.open(
                    //    loc,
                    //    "Download File",
                    //    "resizable,scrollbars,status"
                    //);
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (error) {
                uiApp.UnBlockUI();
            });

        });

        $('#btnSellPartialMemo').click(function (e) {
            e.preventDefault();
            $('#hfIsSellPartial').val('true');
            objSFSvc.SummaryData(ListMemo).then(function (data) {
                if (data.IsSuccess) {
                    $('#txtMemoAvgDiscount').val(data.Result.AvgRapoff);
                    $('#Modal-SellFullMemo').modal('show');

                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
            });
        });

        $('#btnReturnPartialSellMemo').click(function (e) {
            e.preventDefault();
            uiApp.Confirm('Confirm Partial Return Sell Update?', function (resp) {
                if (ListMemo.length > 0) {
                    objMSvc.ReturnPartailMemo(ListMemo).then(function (data) {
                        if (data.IsSuccess) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Partial Return Sell Successfully", type: "success" });
                            dtOrder.getDataTable().draw();
                            var OrderID = $('#hfOrderID').val();
                            LoadOrderItems(OrderID);
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
                        }
                    }, function (error) {
                    });
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No memos selected", type: "error" });
                }
            });
        });

        //$(document).on('click', '.loadData123', function (e) {
        //    var criteria = e.target.dataset.criteria;
        //    $('#hfMemoQuery').val(criteria);
        //     $('#frmMomoPage').submit();
        //    //location.href = '/Inventory/SpecificSearchPost?c=' + criteria;
        //});

        //$(document).on('click', 'a.ancMemoItems', function (e) {
        //    e.preventDefault();
        //    var Inventoryid = $(e.target).attr('data-id');
        //    objSFSvc.GetMemoDetailsInventoryid(Inventoryid).then(function (data) {
        //        if (data.IsSuccess == true) {
        //            window.location.href = '/Memo';
        //            MemoDetailsShow(data.Result.orderDetailsId, "");
                  

        //        } else {
        //            uiApp.Alert({ container: '#uiPanel1', message: "Details not found", type: "error" });
        //        }
        //    }, function (error) {
        //        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
        //    });


        //});
    };

    var BindMainTable = function (CustID) {

        //dtOrder.setAjaxParam('CustomerID', data.query);
        if (dtOrder.getDataTable() == null || dtOrder.getDataTable() == undefined) {
            dtOrder.init({
                src: '#tblOrders',
                dataTable: {
                    //deferLoading: 0,
                    //scrollY: "500px",
                    scrollY: "485px",
                    scrollX: true,
                    paging: true,
                    pageLength: 200,
                    lengthMenu: [[10, 25, 50, 100, 200, 500000], [10,25,50, 100, 200, "All"]],
                    order: [[1, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Order/OrderListing?OType=memo&FilterCustomerID=' + CustID,
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: null },
                        { data: "orderDetailsId" },
                        { data: "orderCreatedOn" },
                        { data: "companyName" },
                        { data: "remark" },
                        { data: "OrdCount" },
                        { data: "orderTotalCarat" },
                        { data: "orderAvgRapPerCT" },
                        { data: "orderTotalRap" },
                        //{ data: "orderAvgDiscount" },
                        { data: "orderAvgRapOff" },
                        { data: "orderPricePerCT" },
                        { data: "orderAmount" },
                        { data: "CreatedName" },
                        //{ data: "ModifierName" },
                        { data: null }
                    ],
                    columnDefs: [{
                        targets: [0],
                        render: function (data, type, row) {
                            return '<label class="checkbox"><input type="checkbox" value="' + row.orderDetailsId + '"></label>';
                        },
                        orderable: false
                    }, {
                        targets: [1],
                        orderable: false
                    }, {
                        targets: [2],
                        render: function (data, type, row) {
                            return moment(row.orderCreatedOn).format(myApp.dateFormat.Client);
                        },
                        orderable: true
                    }, {
                        targets: [3],
                        render: function (data, type, row) {
                            return row.companyName + "-" + row.firstName + " " + row.lastName;
                        },
                        orderable: true
                    }, {
                        targets: [4],
                        orderable: false
                    }, {
                        targets: [5],
                        orderable: false,
                        render: function (data, type, row) {
                            return '<a class="ancShowItems" data-id="' + row.orderDetailsId + '" " data-id="' + row.orderDetailsId + '" data-disc="' + row.orderAvgRapOff + '" data-name="' + (row.firstName + " " + row.lastName + " | " + row.companyName) + '" href="#" >' + data + '</a>';
                        }
                    }, {
                        targets: [6],
                        orderable: false,
                        render: function (data, type, row) {
                          //  return data.toFixed(2);
                            return data.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                        }
                    },
                    {
                        targets: [10],
                        orderable: false,
                        render: function (data, type, row) {
                            //return row.orderAvgRapOff.toFixed(2);
                            if ($('#hfOStatus').val() == "Completed") {
                                if (row.isSaleMode == true) {
                                  //  return row.saleAvgRapPerCT.toFixed(2);
                                    return row.saleAvgRapPerCT.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                }
                            }
                         //   return row.orderPricePerCT.toFixed(2);
                            return row.orderPricePerCT.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                        }
                    },
                    {
                        targets: [9],
                        orderable: false,
                        render: function (data, type, row) {
                            if ($('#hfOStatus').val() == "Completed") {
                                if (row.isSaleMode == true) {
                                   // return row.orderAvgDiscount.toFixed(2);
                                    return row.orderAvgDiscount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                                }
                            }
                            //return row.orderAvgRapOff.toFixed(2);
                            return row.orderAvgRapOff.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                        }
                    }, {
                        targets: [11],
                        orderable: false,
                        render: function (data, type, row) {
                            if ($('#hfOStatus').val() == "Completed") {
                                if (row.isSaleMode == true) {
                                    //return row.salesAmount.toFixed(2);
                                    return row.salesAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                }
                            }
                            //return row.orderAmount.toFixed(2);
                            return row.orderAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                        }
                    }, {
                        targets: [13],
                        orderable: false,
                        render: function (data, type, row, meta) {
                            if ($('#hfOStatus').val() == "Pending") {
                                return '<button type="button" class="btn btn-primary btn-sm dropdown-toggle" data-toggle="dropdown"><i class="fa fa-plus fa-lg"></i></button>\
                                        <ul class="dropdown-menu dropdown-menu-add pull-right">\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" data-disc="' + row.orderAvgRapOff + '" data-name="' + (row.companyName + " | " + row.firstName + " " + row.lastName) + '" class="btnSellFullMemo"><i class="fa fa-opencart"></i>Sell Full Memo</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" data-cName="' + (row.companyName + " | " + row.firstName + " " + row.lastName) + '" class="btnCancelFullMemo"><i class="fa fa-times"></i>Cancel Full Memo</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnEditMemo"><i class="fa fa-pencil"></i>Edit</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnExportMemo"><i class="fa fa-file-excel-o" aria-hidden="true"></i>Export to Excel</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnSendMailMemo"><i class="fa fa-envelope-o"></i>Send a Mail</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnPrintMemoItems"><i class="fa fa-print"></i>Print</a></li>\
                                            <li><a href="/RFID/TallyMemo/'+ row.orderDetailsId + '" target="_blank" class="btnRFIDMatching"><i class="fa fa-file-excel-o"></i>RFID Matching</a></li>\
                                        </ul>';
                            } else {
                                return '<button type="button" class="btn btn-primary btn-sm dropdown-toggle" data-toggle="dropdown"><i class="fa fa-plus fa-lg"></i></button>\
                                        <ul class="dropdown-menu dropdown-menu-add pull-right">\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnExportMemo"><i class="fa fa-file-excel-o" aria-hidden="true"></i>Export to Excel</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnSendMailMemo"><i class="fa fa-envelope-o"></i>Send a Mail</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnPrintMemoItems"><i class="fa fa-print"></i>Print</a></li>\
                                            <li><a href="#" data-index="'+ meta.row + '" data-id="' + row.orderDetailsId + '" class="btnRFIDMatching"><i class="fa fa-file-excel-o"></i>RFID Matching</a></li>\
                                        </ul>';
                            }
                        }
                        }],
                    rowCallback: function (row, data, index) { 
                        if (data.orderDetailsId == $('#hfOrdID').val()) {
                            $(row).css('background-color', '#d6d5d5');
                        }
                        
                    }
                },
                onCheckboxChange: function (obj) {
                    CheckOrder(obj)
                },
                onLoadDataCompleted: function () {
                }
            });

        } else {
            dtOrder.getDataTable().draw();
        }

    }


    var ReadUrl = function () {

      //  alert($('#hfMemoQuery').val());
        var sPageURL = window.location.href;

        // alert(sPageURL.substring(sPageURL.lastIndexOf('/') + 1));
        var ID = 0;
        var lastdata = sPageURL.substring(sPageURL.lastIndexOf('/') + 1);
        if (lastdata != 'Memo') {
            ID = lastdata;
            objSFSvc.GetCustomerDetailsByID(ID).then(function (data) {
                if (data.IsSuccess) {
                    
                    $('#hfIntIID').val(ID);
                    $('#hfOrdID').val(data.Result.orderDetailsId);
                     
                    BindMainTable(data.Result.customerId);

                    //alert($('#hfIntIID').val());
                    //alert($('#hfOrdID').val());

                }
            }, function (error) {
            });

        }
        else {
            BindMainTable(0);
        }

    }

    var GetMultipleInfo = function (OrderIDs) {
        $('#pnlMemoInfoWindow').show();
        objOSvc.GetMultipleInfo(OrderIDs).then(function (data) {
            if (data.IsSuccess) {
                RenderOrderInfoInPopup(data.Result);
            }
        }, function (error) {
        });
    }

    var clearSaleMemo = function () {
        $('#txtMemoSaleDiscount').val('0');
        $('#chkWebMode').prop('checked', true);
        $('#chkSalesMode').prop('checked', false);
        $('#pnlMemoSales').hide();
    }

    var clearOnBack = function () {
        $('#secMemoItems').hide();
        $('#secGrid').show();
        $('#hfOrderID').val('');
    }

    var clearOrderInfo = function () {
        $('#pnlMemoInfoWindow').hide();
        $('#MemoInfoOperation').html('');
        RenderOrderInfoInPopup([]);
    }

    var clearMemo = function () {
        $('#ddlCustomer').val('');
        $('#ddlCustomer').trigger('change');
        $('#txtMemoRemark').val('');
        $('#chkConfirmMemo').prop('checked', false);
        $('#chkSellDirectly').prop('checked', false);

    }

    var RenderOrderItems = function (data) {
        intTotalRowCount = data.length;
        if (dtOrderItem.getDataTable() == null || dtOrderItem.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            dtOrderItem.init({
                src: '#SearchTablePost',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    scrollY: "270px",
                    scrollX: true,
                    data: data,
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colStruct.SpecificSearch.columnDefs,
                    rowCallback: function (row, data, index) {
                        if (data.inventoryID == $('#hfIntIID').val()) {
                            $(row).css('background-color', '#d6d5d5');
                        }

                    }
                },
                onCheckboxChange: function (obj) {
                    ListMemo = obj;
                }
            });
        } else {
            dtOrderItem.clearSelection();
            var table = dtOrderItem.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }

    var LoadOrderItems = function (OrderID) {
        objOSvc.GetOrderItemsByOrderID(OrderID).then(function (data) {
            if (data.IsSuccess == true) {
                RenderOrderItems(data.Result);
                $('#secMemoItems').show();
                $('#secGrid').hide();
                if ($('#hfOStatus').val() == "Pending") {
                    $('#btnSplitMemo').show();
                    $('#btnCancelMemo').show();
                    $('#btnSellPartialMemo').show();
                    $('#btnReturnPartialSellMemo').hide();
                } else {
                    $('#btnSplitMemo').hide();
                    $('#btnCancelMemo').hide();
                    $('#btnSellPartialMemo').hide();
                    $('#btnReturnPartialSellMemo').show();
                }
            } else {
                uiApp.Alert({ container: '#uiPanel1', message: "Memo not fetched", type: "error" });
            }
        }, function (error) {
            uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
        });
    }

    var GetOrderItemsForSplit = function () {
        objSFSvc.GetInventoriesByLotID(ListMemo).then(function (data) {
            if (data.IsSuccess == true) {
                $('#Modal-FormMemo').modal('show');
                $('#Modal-FormMemo h4.modal-title').html('Split Memo');
                $('#btnSaveMemo').data('Action', 'SplitMemo');
                $('#Modal-FormMemo').one('shown.bs.modal', function (e) {
                    RenderOrderItemsInPopup(data.Result);
                    clearMemo();
                });
                $("#Modal-FormMemo").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        });
    }

    var RenderOrderItemsInPopup = function (data) {
        if (dtOrderItemForPopup.getDataTable() == null || dtOrderItemForPopup.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            var colDef = colStruct.SpecificSearch.columnDefs;
            colDef[0]["visible"] = false;
            dtOrderItemForPopup.init({
                src: '#MemoTable',
                dataTable: {
                    scrollY: "250px",
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colDef,
                },
                onCheckboxChange: function (obj) {
                    //ListMemo = obj;
                }
            });
        } else {
            dtOrderItemForPopup.clearSelection();
            var table = dtOrderItemForPopup.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }

    var RenderOrderInfoInPopup = function (data) {
        if (dtOrderInfoForPopup.getDataTable() == null || dtOrderInfoForPopup.getDataTable() == undefined) {
            dtOrderInfoForPopup.init({
                src: '#MemoInfoTable',
                dataTable: {
                    scrollY: "70px",
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

    var CheckOrder = function (lst) {
        ListOrders = lst;
        if ($('#btnViewSoldHistory').data('toggle') == "ViewSoldHistory") {
            $('#btnMergeMemo').hide();
            $('#divReturnSale').show();
            return;
        }

        if (ListOrders.length > 1) {
            $('#btnMergeMemo').show();
        } else {
            $('#btnMergeMemo').hide();
        }

    };

    var SetValidation = function () {
        $SellFullMemo = $('#frmSellFullMemo');

        ValSellFullMemo = $SellFullMemo.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                MemoAgvDiscount: {
                    required: true,
                    max: 0,
                    min: -100
                }
            }
        });

    };



    //var GetCustomerDetails = function (ID) {
    //    objMSvc.GetCustomerDetailsByID(ID).then(function (data) {
    //        if (data.IsSuccess == true) {
    //            RenderOrderItems(data.Result);
               
                
    //        } else {
    //            uiApp.Alert({ container: '#uiPanel1', message: "Memo not fetched", type: "error" });
    //        }
    //    }, function (error) {
    //        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
    //    });
    //}

    //var MemoDetailsShow = function (orderid,name) {
        
    //    var OrderID = orderid;
        
    //    var name = name; 
    //    $('#hfIsSellPartial').val('false');
    //    $('#Modal-SellFullMemo-Message').html('<b>Ref : ' + name + '</b>');
    //    Refname = name; 
    //    $('#hfOrderID').val(OrderID);
    //    LoadOrderItems(OrderID);
    //};

    var GetMemoOrderID = function () {
        var sPageURL = window.location.href;
        
       // alert(sPageURL.substring(sPageURL.lastIndexOf('/') + 1));

        var OrderID = sPageURL.substring(sPageURL.lastIndexOf('/') + 1);
        if (OrderID != 'Memo'  ) {

            var name = "";
            $('#hfIsSellPartial').val('false');
            $('#Modal-SellFullMemo-Message').html('<b>Ref : ' + name + '</b>');
            Refname = name;
            $('#hfOrderID').val(OrderID);
            LoadOrderItems(OrderID);
        }
    };


    return {
        init: function () {
            OnLoad();
        }
    };
}();

$(document).ready(function () {
    CtrlMemo.init();
});