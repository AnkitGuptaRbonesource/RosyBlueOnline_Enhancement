var CtrlCustomerDashboard = function () {
    var dtRecentSearch = null, dtSavedSearch = null, dtDemandSearch = null, dtPendingOrders = null, dtCompletedOrders = null, dtStockDetails=null;
    var objDashSvc = null;
    var OnLoad = function () {
        objDashSvc = new DashboardService();
        dtRecentSearch = new Datatable();
        dtSavedSearch = new Datatable();
        dtDemandSearch = new Datatable();
        dtPendingOrders = new Datatable();
        dtCompletedOrders = new Datatable();
        dtCustomerDemandDetail = new Datatable();
        dtCustomerRecentdetail = new Datatable();
        dtStockDetails = new Datatable();

        RegisterEvent();
        LoadData();
        BindPendingOrders();
        BindCompletedOrders();
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
                            uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
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

        $(document).ready(function () {
            //Get Widget Counts
            $.ajax({
                url: "Dashboard/GetWidgetCounts",
                type: 'GET',
                async:true,
                dataType: 'JSON',
                data: {
                    fromDate: startDate,
                    toDate: endDate
                },
                beforeSend: function () {
                    $("#widgetsLoader").fadeToggle('fast');
                },
                complete: function () {
                    $("#widgetsLoader").fadeToggle('fast');
                },
                success: function (result) {
                    if (result != null) {

                        if (result.status === 1) {

                            var data = result.data;
                            for (i = 0; i < data.length; i++) {
                                $(`#${data[i].key}`).html(data[i].value); //TotalRoundShapes
                            }
                        }

                    }
                }
            });

            //Get Saved Searches
            $.ajax({
                url: "Dashboard/GetDashboardRecentSearches",
                async:true,
                beforeSend: function () {
                   //Show Loader
                },
                complete: function () {
                    //Hide Loader
                },
                success: function (result) {
                    $('#s_search').html(result);
                }
            });

            $.ajax({
                url: "Dashboard/GetDashboardRecentSearches",
                async: true,
                beforeSend: function () {
                    //Show Loader
                },
                complete: function () {
                    //Hide Loader
                },
                success: function (result) {
                    $('#d_search').html(result);
                }
            });
            $.ajax({
                url: "Dashboard/GetDashboardRecentSearches",
                async: true,
                beforeSend: function () {
                    //Show Loader
                },
                complete: function () {
                    //Hide Loader
                },
                success: function (result) {
                    $('#r_search').html(result);
                }
            });
        })

    };

    var LoadData = function () {
        objDashSvc.GetDashboard().then(function (data) {
            BindRecentSearch(data);
            BindSavedSearch(data);
            BindDemandSearch(data);
            BindCounts(data);
            BindRecentStock(data);
            //BindCustomerDemandDetail(data);
            //BindCustomerRecentDetail(data);
        }, function (error) {
        });
    }

    var BindRecentSearch = function (data) {
        if (dtRecentSearch.getDataTable() == null || dtRecentSearch.getDataTable() == undefined) {
            dtRecentSearch.init({
                src: '#tblRecentSearch',
                dataTable: {
                    paging: false,
                    order: [[1, "desc"]],
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
                            return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><span class="se-bx de-bx"><i class="fa fa-trash" aria-hidden="true"></i></span></a>';
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
                            return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><span class="se-bx de-bx"><i class="fa fa-trash" aria-hidden="true"></i></span></a>';
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

    //var BindCustomerDemandDetail = function (data) {
    //    if (dtCustomerDemandDetail.getDataTable() == null || dtCustomerDemandDetail.getDataTable() == undefined) {
    //        dtCustomerDemandDetail.init({
    //            src: '#tblCustomerDemandDetail',
    //            dataTable: {
    //                paging: false,
    //                order: [[1, "desc"]],
    //                processing: false,
    //                serverSide: false,
    //                data: data.DemandList,
    //                columns: [
    //                    { data: null },
    //                    { data: "Createdon" },
    //                    { data: "TotalFound" },
    //                    { data: "demandName" },
    //                    { data: "demandCriteria" },
    //                    { data: null },
    //                    { data: null },
    //                ],
    //                columnDefs: [{
    //                    targets: [0],
    //                    render: function (data, type, row) {
    //                        return row.firstName + ' ' + row.lastName;
    //                    },
    //                }, {
    //                    targets: [1],
    //                    render: function (data, type, row) {
    //                        return moment(row.Createdon).format(myApp.dateFormat.Client);
    //                    },
    //                    orderable: false
    //                }, {
    //                    targets: [5,6],
    //                    render: function (data, type, row) {
    //                        return '<a class="loadData" data-Criteria="' + row.demandCriteria + '" href="#"><i class="fa fa-search" aria-hidden="true"></i></a>';
    //                    },
    //                    orderable: false
    //                }]
    //            },
    //            onCheckboxChange: function (obj) {
    //                ListOfChecks(obj);
    //            }
    //        });
    //    } else {
    //        dtCustomerDemandDetail.getDataTable().draw();
    //        dtCustomerDemandDetail.clearSelection();
    //        var table = dtCustomerDemandDetail.getDataTable();
    //        table.clear().draw();
    //        for (var i = 0; i < data.DemandList.length; i++) {
    //            table.row.add(data.DemandList[i]);
    //        }
    //        table.draw(false);
    //    }
    //}

    //var BindCustomerRecentDetail = function (data) {
    //    if (dtCustomerRecentdetail.getDataTable() == null || dtCustomerRecentdetail.getDataTable() == undefined) {
    //        dtCustomerRecentdetail.init({
    //            src: '#tblCustomerRecentdetail',
    //            dataTable: {
    //                paging: false,
    //                order: [[1, "desc"]],
    //                processing: false,
    //                serverSide: false,
    //                data: data.RecentSearch,
    //                columns: [
    //                    { data: null },
    //                    { data: "Date" },
    //                    { data: null },
    //                ],
    //                columnDefs: [{
    //                    targets: [0],
    //                    render: function (data, type, row) {
    //                        return row.firstName + ' ' + row.lastName;
    //                    },
    //                }, {
    //                    targets: [1],
    //                    render: function (data, type, row) {
    //                        return moment(row.Date).format(myApp.dateFormat.Client);
    //                    },
    //                    orderable: false
    //                }, {
    //                    targets: [2],
    //                    render: function (data, type, row) {
    //                        return '<a class="loadData" href="#"><i class="fa fa-search" aria-hidden="true"></i></a>';
    //                    },
    //                    orderable: false
    //                }]
    //            },
    //            onCheckboxChange: function (obj) {
                    
    //            }
    //        });
    //    } else {
    //        dtCustomerRecentdetail.getDataTable().draw();
    //        dtCustomerRecentdetail.clearSelection();
    //        var table = dtCustomerRecentdetail.getDataTable();
    //        table.clear().draw();
    //        for (var i = 0; i < data.RecentSearch.length; i++) {
    //            table.row.add(data.RecentSearch[i]);
    //        }
    //        table.draw(false);
    //    }
    //}

    return {
        init: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlCustomerDashboard.init();
});