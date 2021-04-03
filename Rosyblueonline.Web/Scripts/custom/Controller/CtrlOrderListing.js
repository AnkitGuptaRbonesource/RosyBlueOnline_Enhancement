var CtrlOrderListing = function () {
    var dtOrder = null;
    var objOSvc = null;
    var ListOrders = [];
    var $ddlCustomer = null;

    var OnLoad = function () {
        dtOrder = new Datatable();
        objOSvc = new OrderService();
        //dtOrder.setAjaxParam('CustomerID', data.query);
        if (dtOrder.getDataTable() == null || dtOrder.getDataTable() == undefined) {
            dtOrder.setAjaxParam('OStatus', $('#ddlStatus').val());
            dtOrder.init({
                src: '#tblOrders',
                dataTable: {
                    //deferLoading: 0,
                    scrollY: "485px",
                    scrollX: true,
                    paging: true,
                    order: [[1, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Order/OrderListing',
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
                        { data: "orderStatus" },
                        { data: "orderPayableAmount" },
                        { data: "firstName" },
                        { data: "companyName" },
                        { data: null }
                    ],
                    columnDefs: [{
                        targets: [0],
                        orderable: false,
                        render: function (data, type, row) {
                            return '<label class="checkbox"><input type="checkbox" value="' + row.orderDetailsId + '"></label>';
                        }
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
                            return data == 10 ? "Pending" : "Approved";
                        },
                        orderable: true
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) { 
                                return data.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            },
                            orderable: true
                        },

                        {
                        targets: [5],
                        orderable: false,
                        render: function (data, type, row) {
                            return row.firstName + ' ' + row.lastName;
                        }
                    }, {
                        targets: [7],
                        orderable: false,
                        render: function (data, type, row) {
                            return "<a href='/order/info/" + row.orderDetailsId + "'>Detail</a>";
                        }
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListOfChecks(obj);
                },
                onLoadDataCompleted: function () {
                    if ($('#ddlStatus').val() == "Completed") {
                        dtOrder.disableCheckbox(false);
                    } else {
                        dtOrder.disableCheckbox(true);
                    }
                }
            });
            console.log(dtOrder.getDataTable());
        } else {
            dtOrder.getDataTable().draw();
        }

        $ddlCustomer = $('#ddlCustomer');
        if ($ddlCustomer.length > 0) {
            $ddlCustomer.select2({
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
                width: 'resolve'
            });
        }


    };

    var RegisterEvents = function () {
        $('#btnMergerOrder').click(function (e) {
            e.preventDefault();
            uiApp.Confirm('Confirm merge?', function (resp) {
                if (resp) {
                    objOSvc.MergeOrder(ListOrders).then(function (data) {
                        if (data.IsSuccess == true && data.Result > 0) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Order merged successfully", type: "success" });
                            dtOrder.getDataTable().draw();
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                }
            });
        });

        $('#ddlStatus').change(function (e) {
            e.preventDefault();
            dtOrder.clearSelection();
            dtOrder.setAjaxParam('OStatus', $(this).val());
            dtOrder.getDataTable().ajax.reload();

        });

        $ddlCustomer.change(function (e) {
            e.preventDefault();
            if ($ddlCustomer.val() != '') {
                dtOrder.setAjaxParam('FilterCustomerID', $($ddlCustomer).val());
            }
            dtOrder.getDataTable().ajax.reload();
        });

        $('#btnClearCustomer').click(function (e) {
            $(this).attr('disable', true);
            e.preventDefault();
            e.stopPropagation();
            dtOrder.setAjaxParam('FilterCustomerID', 0);
            $ddlCustomer.val(null).trigger('change');
            $(this).removeAttr('disable');
        });
    }

    function ListOfChecks(SelectedItems) {
        ListOrders = SelectedItems;
        if (SelectedItems.length > 1) {
            $('#btnMergerOrder').show();
        } else {
            $('#btnMergerOrder').hide();
        }
    }

    return {
        init: function () {
            OnLoad();
            RegisterEvents();
        }
    }
}();
$(document).ready(function () {
    CtrlOrderListing.init();
});