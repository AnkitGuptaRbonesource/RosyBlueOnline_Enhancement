var CtrlAdminDashboard = function () {
    var dtNewCustomers = null, dtStockDetails = null;
    var objDashSvc = null;
    var objLoginRegistrationSvc = null, objModuleStockSummary = null;
    var OnLoad = function () {
        objDashSvc = new DashboardService();
        objLoginRegistrationSvc = new LoginRegistrationService();
        objModuleStockSummary = new ModuleStockSummary({
            instanceID: 1
        });
        objModuleStockSummary.init();
        dtRecentSearch = new Datatable();
        dtNewCustomers = new Datatable();
        dtStockDetails = new Datatable();
        //tblNewCustomers
        RegisterEvent();
        LoadData();
        BindCustomers();
    };

    var RegisterEvent = function () {

        $(document).on('click', '.removeSearch', function (e) {
            var rID = $(e.target.parentElement).attr('data-id');
            uiApp.Confirm('Confirm Delete?', function (resp) {
                if (resp) {
                    objDashSvc.RemoveRecent(rID).then(function (data) {
                        if (data.IsSuccess == true && data.Result > 0) {
                            LoadData();
                            uiApp.Alert({ container: '#uiPanel1', message: "Data removed successfully", type: "success" });
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Data not removed", type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                    });
                }
            });
        });

        $(document).on('click', '.loadData', function (e) {
            var criteria = $(e.target.parentElement).attr('data-criteria');
            $('#hfQuery').val(criteria);
            $('#frmPostSearch').submit();
            //location.href = '/Inventory/SpecificSearchPost?c=' + criteria;
        });

        $('.info-boxtxt').click(function (e) {
            e.preventDefault();
            var criteria = $(this).data('criteria');
            if (criteria != undefined && criteria != "") {
                $('#hfQuery').val(criteria);
                $('#frmPostSearch').submit();
                //location.href = '/Inventory/SpecificSearchPost?c=' + criteria;
            }
        });

        $(document).on('click', '.custSearch', function (e) {
            e.preventDefault();
            var idx = $(e.target).attr('data-row');
            var custData = dtNewCustomers.getDataTable().rows().data()[idx];
            $('#pnlViewCustomer').html('');
            $('#tmpCustomer').tmpl(custData).appendTo('#pnlViewCustomer');
            $('#dashboard-ViewCustomer').modal('show');

            $('#hfCustomerID').val(custData.loginID);
        });

        $(document).on('click', '.custApprove', function (e) {
            e.preventDefault();
            var idx = $(e.target).attr('data-row');
            var custData = dtNewCustomers.getDataTable().rows().data()[idx];
            uiApp.Confirm('Comfirm Approve?', function (resp) {
                if (resp) {
                    ApproveCustomer(custData.loginID, 1, 'accepted');
                }
            });
        });

        $(document).on('click', '.custReject', function (e) {
            e.preventDefault();
            var idx = $(e.target).attr('data-row');
            var custData = dtNewCustomers.getDataTable().rows().data()[idx];
            uiApp.Confirm('Comfirm Reject?', function (resp) {
                if (resp) {
                    ApproveCustomer(custData.loginID, 2, 'rejected');
                }
            });
        });

        $('#btnPopUpApprove').click(function (e) {
            e.preventDefault();
            var CustomerID = $('#hfCustomerID').val();
            if (CustomerID != null && CustomerID != undefined) {
                uiApp.Confirm('Comfirm Approve?', function (resp) {
                    if (resp) {
                        ApproveCustomer(CustomerID, 1, 'approved');
                    }
                });
            }
        });

        $('#btnPopUpReject').click(function (e) {
            e.preventDefault();
            var CustomerID = $('#hfCustomerID').val();
            if (CustomerID != null && CustomerID != undefined) {
                uiApp.Confirm('Comfirm Reject?', function (resp) {
                    if (resp) {
                        ApproveCustomer(CustomerID, 2, 'rejected');
                    }
                });
            }
        });



        function ApproveCustomer(loginID, type, flag) {
            objLoginRegistrationSvc.ApproveCustomer(loginID, type).then(function (data) {
                if (data.IsSuccess == true && data.Result > 0) {
                    BindCustomers();
                    $('#dashboard-ViewCustomer').modal('hide');
                    uiApp.Alert({ container: '#uiPanel1', message: "User " + flag, type: "success" });
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "User not " + flag, type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
            });
        }
    };

    var LoadData = function () {
        objDashSvc.GetDashboard().then(function (data) {

            BindCounts(data);
            BindRecentStock(data);
        }, function (error) {
        });
    }

    var BindCounts = function (data) {
        if (data.counts != undefined) {
            $('#ancMemoCount span.info-box-number').html(data.counts.OnMemo);
            $('#ancRoundCount span.info-box-number').html(data.counts.RoundShape);
            $('#ancFancyColorCount span.info-box-number').html(data.counts.FancyCount);
            $('#ancFancyShapeCount span.info-box-number').html(data.counts.FancyShape);
            $('#ancNewArrivalsCount span.info-box-number').html(data.counts.NewArrival);
            $('#ancRevisedStonesCount span.info-box-number').html(0);
            $('#ancBestDealCount span.info-box-number').html(data.counts.BestDeal);
        }
        if (data.criteria != undefined) {
            $('#ancRoundCount').data('criteria', data.criteria.RoundShapeCriteria);
            $('#ancFancyColorCount').data('criteria', data.criteria.FancyCountCriteria);
            $('#ancFancyShapeCount').data('criteria', data.criteria.FancyShapeCriteria);
        }
    }

    var BindCustomers = function () {
        if (dtNewCustomers.getDataTable() == null || dtNewCustomers.getDataTable() == undefined) {
            dtNewCustomers.init({
                src: '#tblNewCustomers',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    lengthChange: false,
                    pageLength: 10,
                    order: [[1, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Dashboard/CustomerListingPendingApproval',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "createdOn" },
                        { data: "firstName" },
                        { data: "companyName" },
                        { data: "emailId" },
                        { data: null },
                        { data: null },
                        { data: null }
                    ],
                    columnDefs: [
                        {
                            targets: [0],
                            render: function (data, type, row) {
                                return moment(row.orderCreatedOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        }, {
                            targets: [1],
                            render: function (data, type, row) {
                                return row.firstName + ' ' + row.lastName;
                            },
                            orderable: true
                        }, {
                            targets: [2],
                            render: function (data, type, row) {
                                return row.companyName;
                            },
                            orderable: true
                        }, {
                            targets: [4],
                            orderable: false,
                            render: function (data, type, row, meta) {
                                return "<a href='#' class='custSearch' data-row='" + meta.row + "' ><i data-row='" + meta.row + "' class='fa fa-search'></i></a>";
                            }
                        }, {
                            targets: [5],
                            orderable: false,
                            render: function (data, type, row, meta) {
                                return "<a href='#' class='custApprove' data-row='" + meta.row + "'><i data-row='" + meta.row + "' class='fa fa-check'></i></a>";
                            }
                        }, {
                            targets: [6],
                            orderable: false,
                            render: function (data, type, row, meta) {
                                return "<a href='#' class='custReject' data-row='" + meta.row + "'><i data-row='" + meta.row + "' class='fa fa-times'></i></a>";
                            }
                        }]
                },
                onCheckboxChange: function (obj) {
                    ListOfChecks(obj);
                }
            });
        } else {
            dtNewCustomers.getDataTable().draw();
        }
    }

    var BindRecentStock = function (data) {
        if (dtStockDetails.getDataTable() == null || dtStockDetails.getDataTable() == undefined) {
            dtStockDetails.init({
                src: '#tblStockDetails',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data.stock,
                    columns: [
                        { data: "createdOn" },
                        { data: "openingstock" },
                        { data: "newStock" },
                        { data: "memostock" },
                        { data: "orderPending" },
                        { data: "sold" },
                        { data: "closingStock" }
                    ],
                    columnDefs: [{
                        targets: [0, 1, 2, 3, 4],
                        className: "rsearch"
                    }, {
                        targets: [0],
                        render: function (data, type, row) {
                            return moment(row.Createdon).format(myApp.dateFormat.Client);
                        },
                        orderable: false
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListMemo = obj;
                }
            });
        } else {
            dtStockDetails.clearSelection();
            var table = dtStockDetails.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.DemandSearch.length; i++) {
                table.row.add(data.DemandSearch[i]);
            }
            table.draw(false);
        }
    }

    return {
        init: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlAdminDashboard.init();
});