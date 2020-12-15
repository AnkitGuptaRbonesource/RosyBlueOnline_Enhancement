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
        //BindPendingOrderList("Pending");
        //BindPendingOrderList("Complete");
        
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
           // var criteria = $(e.target.parentElement).attr('data-Criteria');
            var criteria = e.target.dataset.criteria;
            
          //  alert(criteria);
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

        //$(document).ready(function () {
        //    //Get Widget Counts
        //    $.ajax({
        //        url: "Dashboard/GetWidgetCounts",
        //        type: 'GET',
        //        async:true,
        //        dataType: 'JSON',
        //        data: {
        //            fromDate: startDate,
        //            toDate: endDate
        //        },
        //        beforeSend: function () {
        //            $("#widgetsLoader").fadeToggle('fast');
        //        },
        //        complete: function () {
        //            $("#widgetsLoader").fadeToggle('fast');
        //        },
        //        success: function (result) {
        //            if (result != null) {

        //                if (result.status === 1) {

        //                    var data = result.data;
        //                    for (i = 0; i < data.length; i++) {
        //                        $(`#${data[i].key}`).html(data[i].value); //TotalRoundShapes
        //                    }
        //                }

        //            }
        //        }
        //    });

            //Get Saved Searches
            //$.ajax({
            //    url: "Dashboard/GetDashboardRecentSearches",
            //    async:true,
            //    beforeSend: function () {
            //       //Show Loader
            //    },
            //    complete: function () {
            //        //Hide Loader
            //    },
            //    success: function (result) {
            //        $('#s_search').html(result);
            //    }
            //});

            //$.ajax({
            //    url: "Dashboard/GetDashboardRecentSearches",
            //    async: true,
            //    beforeSend: function () {
            //        //Show Loader
            //    },
            //    complete: function () {
            //        //Hide Loader
            //    },
            //    success: function (result) {
            //        $('#d_search').html(result);
            //    }
            //});
            //$.ajax({
            //    url: "Dashboard/GetDashboardRecentSearches",
            //    async: true,
            //    beforeSend: function () {
            //        //Show Loader
            //    },
            //    complete: function () {
            //        //Hide Loader
            //    },
            //    success: function (result) {
            //        $('#r_search').html(result);
            //    }
            //});
      //  })

    };

    var LoadData = function () {
        uiApp.BlockUI();
        objDashSvc.GetDashboard().then(function (data) {
            BindRecentSearch(data);
            BindSavedSearch(data);
            BindDemandSearch(data);
            BindCounts(data);
            BindRecentStock(data);
            //BindCustomerDemandDetail(data);
            //BindCustomerRecentDetail(data);
            uiApp.UnBlockUI();
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
                        { data: "Createdon"  },
                        { data: "Createdon" },
                        { data: "TotalFound" },
                        { data: "displayCriteria" },
                        { data: "searchCriteria" },
                        { data: "recentSearchID" }
                    ],
                    columnDefs: [{
                        targets: [0, 1, 2, 3, 4,5],
                        className: "rsearch"
                    },
                        {
                            "targets": [0],
                            "visible": false
                        },{
                        targets: [1],
                            render: function (data, type, row) {
                               // return row.Createdon;
                                return moment(row.Createdon).fromNow(true) + ' ago';
                           // return moment(row.Createdon).format(myApp.dateFormat.Client);
                        },
                        orderable: false
                    }, {
                        targets: [4],
                        render: function (data, type, row) {
                            return '<a class="loadData show-selink" data-Criteria="' + row.searchCriteria + '" href="#"><span class="se-bx" data-Criteria="' + row.searchCriteria + '"><i class="fa fa-search" data-Criteria="' + row.searchCriteria + '"  aria-hidden="true"></i></span> Show Results</a>';
                        },
                        orderable: false
                    }, {
                        targets: [5],
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
       // uiApp.BlockUI();
        $("#tblSavedSearch").html("");
        if (data.SavedSearch.length > 0) { 
            $.each(data.SavedSearch, function (i, item) {
                var  newListItem =  '<li class="table-li">' +
                    '<div class="se-info">' +
                    '<div class="seinfo-l1"><span class="sespce sefound">' + item.TotalFound + '</span><span class="sespce sedate">' + moment(item.Createdon).fromNow(true)+' ago </span><span class="sespce sename">' + item.searchCriteriaName + '</span></div>' +
                    '<div class="seinfo-l2">' + item.displayCriteria + '</div>' +
                    '</div>' +
                    '<div class="se-info-action">' +
                    '<a class="act-link al1 loadData" href="#" data-Criteria=' + item.searchCriteria + ' ><span class="se-bx" data-Criteria=' + item.searchCriteria + '><i class="fa fa-search"  data-Criteria=' + item.searchCriteria +' aria-hidden="true"></i></span></a>' + 
                    '<a class="act-link al2 removeSearch" href="#"  data-id=' + item.recentSearchID + '><span class="se-bx de-bx" data-id=' + item.recentSearchID + ' ><i class="fa fa-trash" data-id=' + item.recentSearchID + ' aria-hidden="true"></i></span></a>' +
                    '</div>' +
                    '</li > ';
                $("#tblSavedSearch").append(newListItem);
            });
              

          //  uiApp.UnBlockUI();
        } else {

            var NodataFound = '<div class="table-no-results text-center">' +
                '<div class="table-nrimg-box" > <img src="../Content/img/no_data_icon.svg" alt="" /></div >' +
                ' <div class="table-nr-text">You’ve not saved any search yet!</div>' +
                '<div class="table-btn-box">' +
                ' <a href="/Inventory/SpecificSearch" class="btn btn-primary"><i class="fa fa-search" aria-hidden="true"></i> Search Now</a>' +
                '</div> </div >';
            $("#tblSavedSearch").append(NodataFound);
           // uiApp.UnBlockUI();
        }



        //if (dtSavedSearch.getDataTable() == null || dtSavedSearch.getDataTable() == undefined) {
        //    dtSavedSearch.init({
        //        src: '#tblSavedSearch',
        //        dataTable: {
        //            paging: false,
        //            order: [[1, "desc"]],
        //            processing: false,
        //            serverSide: false,
        //            data: data.SavedSearch,
        //            columns: [
        //                { data: "Createdon" },
        //                { data: "TotalFound" },
        //                { data: "displayCriteria" },
        //                { data: "searchCriteria" },
        //                { data: "recentSearchID" }
        //            ],
        //            columnDefs: [{
        //                targets: [0, 1, 2, 3, 4],
        //                className: "rsearch"
        //            }, {
        //                targets: [0],
        //                render: function (data, type, row) {
        //                    return moment(row.Createdon).format(myApp.dateFormat.Client);
        //                },
        //                orderable: false
        //            }, {
        //                targets: [3],
        //                render: function (data, type, row) {
        //                    return '<a class="loadData" data-Criteria="' + row.searchCriteria + '" href="#"><span class="se-bx"><i class="fa fa-search" aria-hidden="true"></i></span></a>';
        //                },
        //                orderable: false
        //            }, {
        //                targets: [4],
        //                render: function (data, type, row) {
        //                    return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><span class="se-bx de-bx"><i class="fa fa-trash" aria-hidden="true"></i></span></a>';
        //                },
        //                orderable: false
        //            }]
        //        },
        //        onCheckboxChange: function (obj) {
        //            ListMemo = obj;
        //        }
        //    });
        //} else {
        //    dtSavedSearch.clearSelection();
        //    var table = dtSavedSearch.getDataTable();
        //    table.clear().draw();
        //    for (var i = 0; i < data.SavedSearch.length; i++) {
        //        table.row.add(data.SavedSearch[i]);
        //    }
        //    table.draw(false);
        //}
    }    
    
    var BindDemandSearch = function (data) { 
        $("#tblDemandSearch").html("");
        if (data.DemandSearch.length > 0) {
            $.each(data.DemandSearch, function (i, item) {
                var newListItem = '<li class="table-li">' +
                    '<div class="se-info">' +
                    '<div class="seinfo-l1"><span class="sespce sefound">' + item.TotalFound + '</span><span class="sespce sedate">' + moment(item.Createdon).fromNow(true) +' ago </span><span class="sespce sename">' + item.searchCriteriaName + '</span></div>' +
                    '<div class="seinfo-l2">' + item.displayCriteria + '</div>' +
                    '</div>' +
                    '<div class="se-info-action">' +
                    '<a class="act-link al1 loadData" href="#" data-Criteria=' + item.searchCriteria + ' ><span class="se-bx" data-Criteria=' + item.searchCriteria + ' ><i class="fa fa-search" data-Criteria=' + item.searchCriteria + '  aria-hidden="true"></i></span></a>' +
                    '<a class="act-link al2 removeSearch" href="#"  data-id=' + item.recentSearchID + '><span class="se-bx de-bx" data-id=' + item.recentSearchID + ' ><i class="fa fa-trash" data-id=' + item.recentSearchID + ' aria-hidden="true"></i></span></a>' +

                    '</div>' +
                    '</li > ';
                $("#tblDemandSearch").append(newListItem);
            });

             
        } else {

            var NodataFound = '<div class="table-no-results text-center">' +
                '<div class="table-nrimg-box" > <img src="../Content/img/no_data_icon.svg" alt="" /></div >' +
                ' <div class="table-nr-text">You’ve not saved any search yet!</div>' +
                '<div class="table-btn-box">' +
                ' <a href="/Inventory/SpecificSearch" class="btn btn-primary"><i class="fa fa-search" aria-hidden="true"></i> Search Now</a>' +
                '</div> </div >';
            $("#tblDemandSearch").append(NodataFound); 
        }
        //if (dtDemandSearch.getDataTable() == null || dtDemandSearch.getDataTable() == undefined) {
        //    dtDemandSearch.init({
        //        src: '#tblDemandSearch',
        //        dataTable: {
        //            paging: false,
        //            order: [[1, "desc"]],
        //            processing: false,
        //            serverSide: false,
        //            data: data.DemandSearch,
        //            columns: [
        //                { data: "Createdon" },
        //                { data: "TotalFound" },
        //                { data: "searchCriteria" },
        //                { data: "recentSearchID" },
        //                { data: "recentSearchID" }
        //            ],
        //            columnDefs: [{
        //                targets: [0, 1, 2, 3, 4],
        //                className: "rsearch"
        //            }, {
        //                targets: [0],
        //                render: function (data, type, row) {
        //                    return moment(row.Createdon).format(myApp.dateFormat.Client);
        //                },
        //                orderable: false
        //            }, {
        //                targets: [3],
        //                render: function (data, type, row) {
        //                    return '<a class="loadData" data-Criteria="' + row.searchCriteria + '" href="#"><span class="se-bx"><i class="fa fa-search" aria-hidden="true"></i></span></a>';
        //                },
        //                orderable: false
        //            }, {
        //                targets: [4],
        //                render: function (data, type, row) {
        //                    return '<a class="removeSearch" data-id="' + row.recentSearchID + '" href="#"><span class="se-bx de-bx"><i class="fa fa-trash" aria-hidden="true"></i></span></a>';
        //                },
        //                orderable: false
        //            }]
        //        },
        //        onCheckboxChange: function (obj) {
        //            ListMemo = obj;
        //        }
        //    });
        //} else {
        //    dtDemandSearch.clearSelection();
        //    var table = dtDemandSearch.getDataTable();
        //    table.clear().draw();
        //    for (var i = 0; i < data.DemandSearch.length; i++) {
        //        table.row.add(data.DemandSearch[i]);
        //    }
        //    table.draw(false);
        //}
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



     
    //function BindPendingOrderList(OType) {
    //    objDashSvc.GetOrderDetails(OType).then(function (data) {
    //        if (data.IsSuccess == true) {
    //            if (data.Result.length > 0) {
    //                $.each(data.Result, function (i, item) { 
    //                    var newListItem = '<div class="order-data">' +
    //                        '<div class="order-cnt">' +
    //                        '<h5>#' + item.orderDetailsId + '</h5>' +
    //                        '<p> ' + item.firstName + ' ' + item.lastName + '<span>(' + item.companyName + ')</span></p>' +
    //                        '</div>' +
    //                        '<div class="order-nmbr">' +
    //                        '<p>' + moment(item.orderCreatedOn).format("DD-MM-YY") + '</p>' +
    //                        '<h5></h5>' +
    //                        '<h5>' + item.orderPayableAmount + '</h5>' +
    //                        '</div></div>';

                        
    //                    if (OType == "Pending") {
    //                        $("#PendingOrderList").append(newListItem);
    //                    } else {
    //                        $("#CompleteOrderList").append(newListItem);
    //                    }
    //                });


    //            } else {

    //                var NodataFound = '<div class="table-no-results text-center"> No Record Found</div >';
    //                if (OType == "Pending") {
    //                    $("#PendingOrderList").append(NodataFound);
    //                } else {
    //                    $("#CompleteOrderList").append(NodataFound);
    //                }
                    
    //            }

    //        }

    //        else {
    //            uiApp.Alert({ container: '#uiPanel1', message: "No Record Found.", type: "danger" });
    //        }
    //    }, function (error) {
    //        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured.", type: "danger" });
    //    });

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