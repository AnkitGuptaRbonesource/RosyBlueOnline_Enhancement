var CtrlAdminDashboard = function () {
    var dtRecentSearch = null, dtSavedSearch = null, dtDemandSearch = null, dtPendingOrders = null, dtCompletedOrders = null,
        dtNewCustomers = null, dtStockDetails = null, dtCustomerDemandDetail = null, dtCustomerRecentdetail = null, dtCustomerRecentSearchResult = null, dtCustomerRecentActivityDetails = null, dtCustomerActivityLogData = null;
    var objDashSvc = null;
    var objLoginRegistrationSvc = null, objModuleStockSummary = null;
    var OnLoad = function () {
        objDashSvc = new DashboardService();
        objLoginRegistrationSvc = new LoginRegistrationService();
        dtRecentSearch = new Datatable();
        dtSavedSearch = new Datatable();
        dtDemandSearch = new Datatable();
        dtPendingOrders = new Datatable();
        dtCompletedOrders = new Datatable();
        dtNewCustomers = new Datatable();
        dtStockDetails = new Datatable();
        dtCustomerDemandDetail = new Datatable();
        dtCustomerRecentdetail = new Datatable();
        dtCustomerRecentSearchResult = new Datatable();
        dtCustomerRecentActivityDetails = new Datatable();
        dtCustomerActivityLogData = new Datatable();

        //tblNewCustomers
        RegisterEvent();
        LoadData();
        BindPendingOrders();
        BindCompletedOrders();
        BindCustomers();
        //objModuleStockSummary = new ModuleStockSummary({
        //    instanceID: 1
        //});
      //  objModuleStockSummary.init();
        BindUserActivityDetails();
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

        $(document).on('click', '.loadDataRecentSearches', function (e) {
            e.preventDefault();
            var userID = $(e.target.parentElement).attr('data-userid');
            objDashSvc.GetRecentSearchView(userID).then(function (data) {
                $('#dashboard-ViewRecentSearch').modal('show');
                BindCustomerRecentSearch(data);
            }, function (error) {
            });
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

        $(document).on('click', '.custSearch', function (e) {
            e.preventDefault();
            var idx = $(e.target).attr('data-row');
            var custData = dtNewCustomers.getDataTable().rows().data()[idx];
            $('#pnlViewCustomer').html('');
            $('#tmpCustomer').tmpl(custData).appendTo('#pnlViewCustomer');
            $('#dashboard-ViewCustomer').modal('show');

            $('#hfCustomerID').val(custData.loginID);
        });


        $(document).on('click', '.btnViewDetails', function (e) {
            e.preventDefault();
            var userID = $(e.target).data('id');  
            objDashSvc.GetCustomerLogDetails(userID).then(function (data) {
                $('#Modal-UserActivityLog').modal('show');
                BindCustomerLogDetails(data);
            }, function (error) {
            });


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
            BindRecentSearch(data);
            BindSavedSearch(data);
            BindDemandSearch(data);
            BindCounts(data);
            BindRecentStock(data);
            BindCustomerDemandDetail(data);
            BindCustomerRecentDetail(data);
        }, function (error) {
        });
    }

    var BindRecentSearch = function (data) {
        if (dtRecentSearch.getDataTable() == null || dtRecentSearch.getDataTable() == undefined) {
            dtRecentSearch.init({
                src: '#tblRecentSearch',
                dataTable: {
                    paging: false,
                    order: [[0, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data.SpecificSearch,
                    columns: [
                        { data: "Createdon" },
                        { data: "TotalFound" },
                        { data: "displayCriteria" },
                        { data: "searchCriteria" },
                        { data: "recentSearchID" }
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
                    }, {
                        targets: [3],
                        render: function (data, type, row) {
                            return '<a class="loadData show-selink" data-Criteria="' + row.searchCriteria + '" href="#"><span class="se-bx"><i class="fa fa-search" aria-hidden="true"></i></span> Show Results</a>';
                        },
                        orderable: false
                    }, {
                        targets: [4],
                        render: function (data, type, row) {
                            return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><span class="se-bx de-bx"><i class="fa fa-trash" aria-hidden="true"></i></span></a>';
                        },
                        orderable: false,
                        visible: false
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListMemo = obj;
                }
            });
        } else {
            dtRecentSearch.clearSelection();
            var table = dtRecentSearch.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.SpecificSearch.length; i++) {
                table.row.add(data.SpecificSearch[i]);
            }
            table.draw(false);
        }

    }

    var BindSavedSearch = function (data) {
        if (dtSavedSearch.getDataTable() == null || dtSavedSearch.getDataTable() == undefined) {
            dtSavedSearch.init({
                src: '#tblSavedSearch',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data.SavedSearch,
                    columns: [
                        { data: "Createdon" },
                        { data: "TotalFound" },
                        { data: "displayCriteria" },
                        { data: "searchCriteria" },
                        { data: "recentSearchID" }
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
                    }, {
                        targets: [3],
                        render: function (data, type, row) {
                            return '<a class="loadData" data-Criteria="' + row.searchCriteria + '" href="#"><span class="se-bx"><i class="fa fa-search" aria-hidden="true"></i></span></a>';
                        },
                        orderable: false
                    }, {
                        targets: [4],
                        render: function (data, type, row) {
                            return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><span class="se-bx de-bx"><i class="fa fa-times" aria-hidden="true"></i></span></a>';
                        },
                        orderable: false
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListMemo = obj;
                }
            });
        } else {
            dtSavedSearch.clearSelection();
            var table = dtSavedSearch.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.SavedSearch.length; i++) {
                table.row.add(data.SavedSearch[i]);
            }
            table.draw(false);
        }

    }

    var BindDemandSearch = function (data) {
        if (dtDemandSearch.getDataTable() == null || dtDemandSearch.getDataTable() == undefined) {
            dtDemandSearch.init({
                src: '#tblDemandSearch',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data.DemandSearch,
                    columns: [
                        { data: "Createdon" },
                        { data: "TotalFound" },
                        { data: "searchCriteria" },
                        { data: "recentSearchID" },
                        { data: "recentSearchID" }
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
                    }, {
                        targets: [3],
                        render: function (data, type, row) {
                            return '<a class="loadData" data-Criteria="' + row.searchCriteria + '" href="#"><span class="se-bx"><i class="fa fa-search" aria-hidden="true"></i></span></a>';
                        },
                        orderable: false
                    }, {
                        targets: [4],
                        render: function (data, type, row) {
                            return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><span class="se-bx de-bx"><i class="fa fa-times" aria-hidden="true"></i></span></a>';
                        },
                        orderable: false
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListMemo = obj;
                }
            });
        } else {
            dtDemandSearch.clearSelection();
            var table = dtDemandSearch.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.DemandSearch.length; i++) {
                table.row.add(data.DemandSearch[i]);
            }
            table.draw(false);
        }


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

    var BindPendingOrders = function () {
        if (dtPendingOrders.getDataTable() == null || dtPendingOrders.getDataTable() == undefined) {
            dtPendingOrders.setAjaxParam('OStatus', "Pending");
            dtPendingOrders.init({
                src: '#tblPendingOrders',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    lengthChange: false,
                    pageLength: 10,
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
                        targets: [4],
                        orderable: false,
                        render: function (data, type, row) {
                            return row.firstName + ' ' + row.lastName;
                        }
                    }, {
                        targets: [6],
                        orderable: false,
                        render: function (data, type, row) {
                            return "<a target='_blank' href='/order/info/" + row.orderDetailsId + "'>Detail</a>";
                        }
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListOfChecks(obj);
                }
            });
        } else {
            dtPendingOrders.getDataTable().draw();
        }
    }

    var BindCompletedOrders = function () {
        if (dtCompletedOrders.getDataTable() == null || dtCompletedOrders.getDataTable() == undefined) {
            dtCompletedOrders.setAjaxParam('OStatus', "Completed");
            dtCompletedOrders.init({
                src: '#tblCompletedOrders',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    lengthChange: false,
                    pageLength: 10,
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
                        targets: [4],
                        orderable: false,
                        render: function (data, type, row) {
                            return row.firstName + ' ' + row.lastName;
                        }
                    }, {
                        targets: [6],
                        orderable: false,
                        render: function (data, type, row) {
                            return "<a target='_blank' href='/order/info/" + row.orderDetailsId + "'>Detail</a>";
                        }
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListOfChecks(obj);
                }
            });
        } else {
            dtCompletedOrders.getDataTable().draw();
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
                            className: 'text-center',
                            render: function (data, type, row, meta) {
                                return "<a href='#' class='custSearch' data-row='" + meta.row + "' ><i data-row='" + meta.row + "' class='fa fa-search'></i></a>";
                            }
                        }, {
                            targets: [5],
                            orderable: false,
                            className: 'text-center',
                            render: function (data, type, row, meta) {
                                return "<a href='#' class='custApprove ' data-row='" + meta.row + "'><i data-row='" + meta.row + "' class='fa fa-check'></i></a>";
                            }
                        }, {
                            targets: [6],
                            orderable: false,
                            className: 'text-center',
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
                    data: data.stocks,
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
                            return moment(row.createdOn).format(myApp.dateFormat.Client);
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

    var BindCustomerDemandDetail = function (data) {
        if (dtCustomerDemandDetail.getDataTable() == null || dtCustomerDemandDetail.getDataTable() == undefined) {
            dtCustomerDemandDetail.init({
                src: '#tblCustomerDemandDetail',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                   paging: true,
                    lengthChange: false,
                    pageLength: 10,
                    data: data.DemandList,
                    columns: [
                        { data: null },
                        { data: "Createdon" },
                        //{ data: "TotalFound" },
                        { data: "demandName" },
                        { data: "demandCriteria" },
                        { data: null }, 
                    ],
                    columnDefs: [{
                        targets: [0],
                        render: function (data, type, row) {
                            return row.firstName + ' ' + row.lastName;
                        },
                    }, {
                        targets: [1],
                        render: function (data, type, row) {
                            return moment(row.Createdon).format(myApp.dateFormat.Client);
                        },
                        orderable: false
                    }, {
                        targets: [4],
                        render: function (data, type, row) {
                            return '<a class="loadData" data-Criteria="' + row.demandCriteria + '" href="#"><i class="fa fa-search" aria-hidden="true"></i></a>';
                        },
                        orderable: false
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListOfChecks(obj);
                }
            });
        } else {
            dtCustomerDemandDetail.getDataTable().draw();
            dtCustomerDemandDetail.clearSelection();
            var table = dtCustomerDemandDetail.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.DemandList.length; i++) {
                table.row.add(data.DemandList[i]);
            }
            table.draw(false);
        }
    }

    var BindCustomerRecentDetail = function (data) {
        if (dtCustomerRecentdetail.getDataTable() == null || dtCustomerRecentdetail.getDataTable() == undefined) {
            dtCustomerRecentdetail.init({
                src: '#tblCustomerRecentdetail',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                   paging: true,
                    lengthChange: false,
                    pageLength: 10,
                    data: data.RecentSearch,
                    columns: [
                        { data: null },
                        { data: "Date" },
                        { data: null },
                    ],
                    columnDefs: [{
                        targets: [0],
                        render: function (data, type, row) {
                            return row.firstName + ' ' + row.lastName;
                        },
                    }, {
                        targets: [1],
                        render: function (data, type, row) {
                            return moment(row.Date).format(myApp.dateFormat.Client);
                        },
                        orderable: false
                    }, {
                        targets: [2],
                        render: function (data, type, row) {
                            return '<a class="loadDataRecentSearches" data-userid="' + row.loginID + '" href="#"><i class="fa fa-search" aria-hidden="true"></i></a>';
                        },
                        orderable: false
                    }]
                },
                onCheckboxChange: function (obj) {

                }
            });
        } else {
            dtCustomerRecentdetail.getDataTable().draw();
            dtCustomerRecentdetail.clearSelection();
            var table = dtCustomerRecentdetail.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.RecentSearch.length; i++) {
                table.row.add(data.RecentSearch[i]);
            }
            table.draw(false);
        }
    }

    var BindCustomerRecentSearch = function (data) {
        if (dtCustomerRecentSearchResult .getDataTable() == null || dtCustomerRecentSearchResult .getDataTable() == undefined) {
            dtCustomerRecentSearchResult .init({
                src: '#tblCustomerRecentSearchResult',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: [
                        { data: "Createdon" },
                        { data: "TotalFound" },
                        { data: "displayCriteria" },
                        { data: "searchCriteria" },
                        { data: "recentSearchID" }
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
                    }, {
                        targets: [3],
                        render: function (data, type, row) {
                            return '<a class="loadData" data-Criteria="' + row.searchCriteria + '" href="#"><i class="fa fa-search" aria-hidden="true"></i></a>';
                        },
                        orderable: false
                    }, {
                        targets: [4],
                        render: function (data, type, row) {
                            return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><i class="fa fa-times" aria-hidden="true"></i></a>';
                        },
                        orderable: false,
                        visible: false
                    }]
                },
                onCheckboxChange: function (obj) {
                    ListMemo = obj;
                }
            });
        } else {
            dtCustomerRecentSearchResult .clearSelection();
            var table = dtCustomerRecentSearchResult.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }

    }


    //Added by Ankit 19JUn2020
    //BindCompletedOrders
    var BindUserActivityDetails = function () {
        if (dtCustomerRecentActivityDetails.getDataTable() == null || dtCustomerRecentActivityDetails.getDataTable() == undefined) {
          //  dtCustomerRecentActivityDetails.setAjaxParam('OStatus', "Completed");
            dtCustomerRecentActivityDetails.init({
                src: '#tblCustomerActivityDetails',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    lengthChange: false,
                    pageLength: 10,
                    order: [[1, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Dashboard/UserActivityDetails',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [ 
                        { data: "Username" },
                        { data: null}, 
                        { data: "CreatedOn" },
                        { data: null }
                    ],
                    columnDefs: [{
                        targets: [0],
                        orderable: false 
                         
                    },
                       
                        {
                            targets: [1],
                            render: function (data, type, row) {
                                return row.Locality + ', ' + row.City + ', ' + row.State + ', ' + row.Country;
                            } ,
                            orderable: false

                            
                        },
                        {
                        targets: [2],
                        render: function (data, type, row) {
                            return moment(row.CreatedOn).format(myApp.dateTimeFormat.Client);
                        } 
                        },
                         {
                        targets: [3],
                        orderable: false,
                        render: function (data, type, row) {
                            return '<a class="btnViewDetails" data-id="' + row.LoginID + '" href="#">View</a>';
                             
                              //  return '<a class="btnEditDetails" data-id="' + row.WSID + '"  data-Name="' + row.Name + '"  data-Frequency="' + row.Frequency + '"  data-FrequencyInterval="' + row.FrequencyInterval + '"  data-status="' + row.Status + '"  href="#">Edit</a>';
                            
                        }
                    }]
                },
                onCheckboxChange: function (obj) {
                     
                }
            });
        } else {
            dtCustomerRecentActivityDetails.getDataTable().draw();
        }
    }


    var BindCustomerLogDetails = function (data) {
        if (dtCustomerActivityLogData.getDataTable() == null || dtCustomerActivityLogData.getDataTable() == undefined) {
            dtCustomerActivityLogData.init({
                src: '#tblCustomerLogData',
                dataTable: {
                    paging: false,
                    order: [[2, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: [
                        { data: "ActionName" },
                        { data: "ActionDetails" },
                        { data: "CreatedOn" } 
                    ],
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false

                        },
                        {
                            targets: [1],
                            orderable: false

                        },
                        {
                            targets: [2],
                            render: function (data, type, row) {
                                return moment(row.CreatedOn).format(myApp.dateTimeFormat.Client);
                            }
                        }]
                    
                },
                onCheckboxChange: function (obj) {
                   // ListMemo = obj;
                }
            });
        } else {
            dtCustomerActivityLogData.clearSelection();
            var table = dtCustomerActivityLogData.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }

    }

    //$(document).on('click', 'a.btnViewDetails', function (e) {
    //    e.preventDefault();
    //    //BindUserActivityDetails();
    //    //var id = $(e.target).data('id');
    //    //var name = $(e.target).data('name');
    //    //var disc = $(e.target).data('disc'); 
    //    //$('#Modal-UserActivityLog').modal('show');
    //    //BindUserActivityDetails();

    //    var userID = $(e.target.parentElement).attr('data-userid');
    //    objDashSvc.GetCustomerLogDetails(userID).then(function (data) {
    //        $('#Modal-UserActivityLog').modal('show');
    //        BindCustomerLogDetails(data);
    //    }, function (error) {
    //    });

       
    //});

    return {
        init: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlAdminDashboard.init();
});