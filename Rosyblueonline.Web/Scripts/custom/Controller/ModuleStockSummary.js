var ModuleStockSummary = function (options) {

    var objDasSvc = null,
        dtStockSummary = null;;
    var $scope = null,
        $ddlShape = null,
        $ddlSalesLocation = null,

        $ddlSType = null;

    var OnLoad = function () {
        $scope = myApp.defScope($('#StockSummaryModule_' + options.instanceID));
        $ddlShape = $scope('#ddlShape_' + options.instanceID);
        $ddlSalesLocation = $scope('#ddlSalesLocation_' + options.instanceID);
        $ddlSType = $scope('#ddlSType_' + options.instanceID);

        objDasSvc = new DashboardService();
        dtStockSummary = new Datatable();
        registerEvents();
        
        objDasSvc.GetSalesLocation().then(function (data) {
            var AllOpt = '', Options = '';
            for (var i = 0; i < data.length; i++) {
                AllOpt = AllOpt == '' ? data[i].salesLocationID : AllOpt + ',' + data[i].salesLocationID
                Options = Options + '<option value="' + data[i].salesLocationID + '">' + data[i].locationName + '</option>';
            }
            $ddlSalesLocation.html('<option value="' + AllOpt + '" selected>ALL</option>' + Options);
            loadStockSummary();
        }, function (error) {
        });

    }

    var registerEvents = function () {
        $ddlShape.change(function () {
            loadStockSummary();
        });
        $ddlSalesLocation.change(function () {
            loadStockSummary();
        });

        $ddlSType.change(function () {
            ToggleStockSummaryColumns();
        });


        //$('#tblStockStatus_1').on('click', '#expandCollapse_level1', function () {
        //    var span = $($($(this).closest("tr")).find('td')).find("[id*=expandCollapse_level]");
        //   // $($(thisval).closest("tr")).find('td').eq(1).text()
        //    objDasSvc.GetStoneDetailsStockSummary('0.1').then(function (response) {
              
        //        Bindhtml(response[0].Color, span); 
        //    }, function (error) {
        //        alert(error);
        //    });
        //});


      
        $(document).on('click', '.list', function (e) { 
            uiApp.BlockUI({ container: '#StockSummaryModule_' + options.instanceID });
            var size = $(this).closest("a")[0].name; 
            var sizearr = size.split('-');
            var span = $(this);
            var name = $(this).text();
            objDasSvc.GetStoneDetailsStockSummary(sizearr[0].trim(), sizearr[1].trim(), $ddlShape.val().trim(), $ddlSalesLocation.val().trim(), $ddlSType.val().trim(), name.trim()).then(function (response) {

                Bindhtml(response[0].Color, span); 
                uiApp.UnBlockUI('#StockSummaryModule_' + options.instanceID);
            }, function (error) {
                alert(error);
            });

            //var html = html + '<tr><td colspan="' + 4 + '"> <ul class="nav nav-tabs">'
            //html = html + '   <li class="active"><a class="list" data-toggle="tab"  >Total Inventory</a></li>'
            //html = html + '  <li><a class="list" data-toggle="tab"  >On Memo</a></li>'
            //html = html + '   <li><a class="list" data-toggle="tab"  >Website Order Pending</a></li> '
            //html = html + '   <li><a  class="list" data-toggle="tab" >Available</a></li>  </ul> '

            //html = html + '</td ></tr > '
            //$('#tblStockStatus_1>tbody>tr').eq($(this).closest('tr').index()).after(html);

        });

        $('#tblStockStatus_1').on('click', '#expandCollapse_level1', function () {
            var span = $($($(this).closest("tr")).find('td')).find("[id*=expandCollapse_level]");
            // $($(thisval).closest("tr")).find('td').eq(1).text()
            var size = $($(this).closest("tr")).find('td').eq(1).text();
            var span = $(this);// $($($(thisval).closest("tr")).find('td')).find("[id*=expandCollapse_level]");

            var rowIndex = $($($(this)).closest("tr")).index();

            var ColumnCount = $($($(this).closest("tr")).find('td')).length;
             
            if ($(span).attr("collapse") == "true") {
                // var AutoId = $($(this).closest("tr")).find('td').eq(2).text().trim();
                $(span).attr("collapse", "false");
                $(span).removeClass("btn btn-info btn-sm glyphicon glyphicon-plus");
                $(span).addClass("btn btn-info btn-sm glyphicon glyphicon-minus");
                 
                var html = html + '<tr><td colspan="' + ColumnCount + '"> <ul class="nav nav-tabs">'
                html = html + '   <li ><a class="list" data-toggle="tab" href="#' + size + '" name="' + size +'">Total Inventory</a></li>'
                html = html + '  <li><a class="list" data-toggle="tab" href="#' + size + '" name="' + size +'">On Memo</a></li>'
                html = html + '   <li><a class="list" data-toggle="tab" href="#' + size + '" name="' + size +'">Website Order Pending</a></li> '
                html = html + '   <li><a  class="list" data-toggle="tab" href="#' + size + '" name="' + size +'">Available</a></li>  </ul> '
               
                html = html + '</td ></tr > ' 
                $('#tblStockStatus_1>tbody>tr').eq($(this).closest('tr').index()).after(html);

                //uiApp.BlockUI({ container: '#StockSummaryModule_' + options.instanceID }); 
                //var sizearr = size.split('-');
                //var span = $(this);
                //var name = 'Total Inventory';
                //objDasSvc.GetStoneDetailsStockSummary(sizearr[0].trim(), sizearr[1].trim(), $ddlShape.val().trim(), $ddlSalesLocation.val().trim(), $ddlSType.val().trim(), name.trim()).then(function (response) {

                //    Bindhtml(response[0].Color, span);
                //    uiApp.UnBlockUI('#StockSummaryModule_' + options.instanceID);
                //}, function (error) {
                //    alert(error);
                //});
                 
            }
            else {
                $('#tblStockStatus_1>tbody>tr').eq($(this).closest('tr').index() + 1).html(null);

                $('#tblStockStatus_1>tbody>tr').eq($(this).closest('tr').index() + 2).html(null);
                $(span).attr("collapse", "true");
                $(span).removeClass("btn btn-info btn-sm glyphicon glyphicon-minus");
                $(span).addClass("btn btn-info btn-sm glyphicon glyphicon-plus");
            }
             
        });

        $(document).on('click', '.loadData', function (e) {
         //   var criteria = $(e.target.parentElement).attr('data-criteria');
           // window.location.href  = '/Inventory/SpecificSearchPost';
            var criteria = e.target.dataset.criteria;
            $('#hfQuery').val(criteria);
            $('#frmPostSearch').submit();
        });
    };

    var loadStockSummary = function () {
        uiApp.BlockUI({ container: '#StockSummaryModule_' + options.instanceID });
        objDasSvc.GetStockSummary($ddlShape.val(), $ddlSalesLocation.val()).then(function (response) {
            renderData(response)
            uiApp.UnBlockUI('#StockSummaryModule_' + options.instanceID);
        }, function (error) {
            uiApp.UnBlockUI('#StockSummaryModule_' + options.instanceID);
        });
    };



    var ToggleStockSummaryColumns = function () {
        //uiApp.BlockUI({ container: '#StockSummaryModule_' + options.instanceID });
        //alert($('#hfStockS').val());
        //alert($ddlSType.val());
        if ($ddlSType.val() == "ByValue") {

            $('#hfStockS').val('ByValue');
            // dtStockSummary.setAjaxParam('StockSStatus', 'ByValue');
        } else {

            $('#hfStockS').val('ByStone');
            //  dtStockSummary.setAjaxParam('StockSStatus', 'ByStone');
        }
        // dtStockSummary.getDataTable().draw();

        loadStockSummary();


    };



    var renderData = function (data) {
        if (dtStockSummary.getDataTable() == null || dtStockSummary.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();

            dtStockSummary.init({
                src: ('#tblStockStatus_' + options.instanceID),
                dataTable: {
                    //deferLoading: 0,
                    paging: false,
                    ordering: false,
                    //order: [[1, "desc"]],

                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: [{ data: "Sizes" },
                    { data: "Sizes" },
                    //{ data: "TotInv" },
                    //{ data: 'TotOnMemo' },
                    //{ data: 'TotOrdPending' },
                    //{ data: 'TotAvailbale' } 

                    { data: 'TotInvValue' },
                    { data: 'TotOnMemoValue' },
                    { data: 'TotOrdPendingValue' },
                    { data: 'TotAvailbaleValue' }

                    ],
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            render: function (data, type, row) {

                                return '<span id="expandCollapse_level1" collapse="true" level="1" class="btn stock-add  glyphicon glyphicon-plus"></span>';

                            }
                        },
                        {
                            targets: [1],
                            orderable: false,
                            render: function (data, type, row) {

                                return row.Sizes;

                               // return data.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                              //  return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.Sizes + '</a>';

                            }
                        },
                        {
                            targets: [2],
                            orderable: false,
                            render: function (data, type, row) {
                                if ($('#hfStockS').val() == "ByValue") {


                                    // return row.TotInvValue.toFixed(2);
                                  //  return row.TotInvValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                     
                                    return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes+'|" href="#">' + row.TotInvValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")+'</a>';
                                    //return Number(row.TotInvValue.toFixed(2)).toLocaleString('en-IN', {
                                    //    maximumFractionDigits: 2,
                                    //    style: 'currency',
                                    //    currency: 'INR'
                                    //});


                                }
                              //  return row.TotInv;
                                return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.TotInv + '</a>';

                            }
                        },
                        {
                            targets: [3],
                            orderable: false,
                            render: function (data, type, row) {
                                if ($('#hfStockS').val() == "ByValue") {

                                    // return row.TotOnMemoValue.toFixed(2);
                                  //  return row.TotOnMemoValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                    return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.TotOnMemoValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</a>';

                                }
                             //   return row.TotOnMemo;
                                return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.TotOnMemo + '</a>';

                            }
                        },
                        {
                            targets: [4],
                            orderable: false,
                            render: function (data, type, row) {
                                if ($('#hfStockS').val() == "ByValue") {

                                    //  return row.TotOrdPendingValue.toFixed(2);
                                    //return row.TotOrdPendingValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                    return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.TotOrdPendingValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</a>';


                                }
                                return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.TotOrdPending + '</a>';

                              //  return row.TotOrdPending;
                            }
                        },
                        {
                            targets: [5],
                            orderable: false,
                            render: function (data, type, row) {
                                if ($('#hfStockS').val() == "ByValue") {

                                    // return row.TotAvailbaleValue.toFixed(2);
                                   // return row.TotAvailbaleValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                    return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.TotAvailbaleValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</a>';


                                }
                                return ' <a class="loadData" data-criteria="SLOCATION~' + $ddlSalesLocation.val().trim() + '|MULTISIZE~' + row.Sizes + '|" href="#">' + row.TotAvailbale + '</a>';

                               // return row.TotAvailbale;
                            }
                        }

                        //, { "visible": false, "targets": [3] }
                    ],
                    footerCallback: function (row, data, start, end, display) {
                        //console.log(row, data, start, end, display);
                        var TotInv = 0, TotOnMemo = 0, TotOrdPending = 0, TotAvailbale = 0, TotInvValue = 0, TotOnMemoValue = 0, TotOrdPendingValue = 0, TotAvailbaleValue = 0;
                        for (var i = 0; i < data.length; i++) {
                            if ($('#hfStockS').val() == "ByStone") {
                                TotInv += data[i].TotInv;
                                TotOnMemo += data[i].TotOnMemo;
                                TotOrdPending += data[i].TotOrdPending;
                                TotAvailbale += data[i].TotAvailbale;
                            } else
                                if ($('#hfStockS').val() == "ByValue") {
                                    TotInvValue += data[i].TotInvValue;
                                    TotOnMemoValue += data[i].TotOnMemoValue;
                                    TotOrdPendingValue += data[i].TotOrdPendingValue;
                                    TotAvailbaleValue += data[i].TotAvailbaleValue;
                                }
                        }
                        if ($('#hfStockS').val() == "ByStone") {
                            $(row).find('th:eq(2)').html(TotInv);
                            $(row).find('th:eq(3)').html(TotOnMemo);
                            $(row).find('th:eq(4)').html(TotOrdPending);
                            $(row).find('th:eq(5)').html(TotAvailbale);
                        } else
                            if ($('#hfStockS').val() == "ByValue") {
                                $(row).find('th:eq(2)').html(TotInvValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                $(row).find('th:eq(3)').html(TotOnMemoValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                $(row).find('th:eq(4)').html(TotOrdPendingValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                $(row).find('th:eq(5)').html(TotAvailbaleValue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            }

                    }

                },
                onCheckboxChange: function (obj) {

                }
            });
        } else {
            dtStockSummary.clearSelection();
            var table = dtStockSummary.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }


    var Bindhtml = function (data, thisval) {

        debugger;
        var cnt = 0;
        cnt = cnt + 1;
        // var AutoId = $($(this).closest("tr")).find('td').eq(2).text().trim();

        var span = thisval;// $($($(thisval).closest("tr")).find('td')).find("[id*=expandCollapse_level]");

        var rowIndex = $($(thisval).closest("tr")).index();

        var ColumnCount = $($($(thisval).closest("tr")).find('td')).length;
        $('#tblStockStatus_1>tbody>tr').eq($(thisval).closest('tr').index() + 1).html(null);
             
            var tableid = 'tblStockStatus_Sub_' + rowIndex;
             
            var html = '<tr><td colspan="' + 8 + '">'

            html = html + '<table id="' + tableid + '" class="table table-fixed dataTable no-footer clsSub"><thead>';
            html = html + data;
            html = html + ' </table ></td ></tr > '
 
            $('#tblStockStatus_1>tbody>tr').eq($(thisval).closest('tr').index()).after(html);

 

    };

   

    //var Bindhtml = function (data,thisval) {

    //        debugger;
    //        var cnt = 0;
    //        cnt = cnt + 1;
    //        // var AutoId = $($(this).closest("tr")).find('td').eq(2).text().trim();

    //    var span = thisval;// $($($(thisval).closest("tr")).find('td')).find("[id*=expandCollapse_level]");

    //    var rowIndex = $($(thisval).closest("tr")).index();

    //    var ColumnCount = $($($(thisval).closest("tr")).find('td')).length;



    //        if ($(span).attr("collapse") == "true") {

    //            //   alert(response);
                 
    //           // var AutoId = $($(this).closest("tr")).find('td').eq(2).text().trim();
    //            $(span).attr("collapse", "false");
    //            $(span).removeClass("btn btn-info btn-sm glyphicon glyphicon-plus");
    //            $(span).addClass("btn btn-info btn-sm glyphicon glyphicon-minus");

    //            var tableid = 'tblStockStatus_Sub_' + rowIndex;

                
    //            var html = '<tr><td colspan="' + ColumnCount + '">'
                 
    //            html=html + '<table id="' + tableid + '" class="table table-fixed dataTable no-footer clsSub"><thead>';
    //            html = html + data;
    //            html = html + ' </table ></td ></tr > '

    //            //html = html + '<tr>'

    //            //////$.each(response, function (i, item) {
    //            //////    response[i]
    //            //////});
    //            //html = html + '<th>Color</th> '
    //            //html = html + '<th>IF</th> '
    //            //html = html + '<th>VVS1</th> '
    //            //html = html + '<th>VVS2</th> '
    //            //html = html + '<th>Vs1</th> '
    //            //html = html + '<th>Vs2</th> '
    //            //html = html + '<th>SI1</th> '
    //            //html = html + '<th>SI2</th> '
    //            //html = html + '<th>SI3</th> '
    //            //html = html + '<th>I1</th> '
    //            //html = html + '<th>I2</th> '
    //            //html = html + '<th>I3</th> '
    //            //html = html + '<th>I4</th> '
    //            //html = html + '<th>FL</th> '
    //            //html = html + '</tr></thead>'

    //            //html = html + '<tbody>'
    //            //html = html + '<tr>'
    //            //html = html + '<td>D</td>'
    //            //html = html + '<td>1</td>'
    //            //html = html + '<td>2</td>'
    //            //html = html + '<td>3</td>'
    //            //html = html + '<td>4</td>'
    //            //html = html + '<td>5</td>'
    //            //html = html + '<td>6</td>'
    //            //html = html + '<td>7</td>'
    //            //html = html + '<td>8</td>'
    //            //html = html + '<td>9</td>'
    //            //html = html + '<td>10</td>'
    //            //html = html + '<td>11</td>'
    //            //html = html + '<td>12</td>'
    //            //html = html + '<td>13</td>'
    //            //html = html + '</tr>'
    //            //html = html + '<tr>'
    //            //html = html + '<td>E</td>'
    //            //html = html + '<td>1</td>'
    //            //html = html + '<td>2</td>'
    //            //html = html + '<td>3</td>'
    //            //html = html + '<td>4</td>'
    //            //html = html + '<td>5</td>'
    //            //html = html + '<td>6</td>'
    //            //html = html + '<td>7</td>'
    //            //html = html + '<td>8</td>'
    //            //html = html + '<td>9</td>'
    //            //html = html + '<td>10</td>'
    //            //html = html + '<td>11</td>'
    //            //html = html + '<td>12</td>'
    //            //html = html + '<td>13</td>'
    //            //html = html + '</tr>'
    //            //html = html + '<tr>'
    //            //html = html + '<td>F</td>'
    //            //html = html + '<td>1</td>'
    //            //html = html + '<td>2</td>'
    //            //html = html + '<td>3</td>'
    //            //html = html + '<td>4</td>'
    //            //html = html + '<td>5</td>'
    //            //html = html + '<td>6</td>'
    //            //html = html + '<td>7</td>'
    //            //html = html + '<td>8</td>'
    //            //html = html + '<td>9</td>'
    //            //html = html + '<td>10</td>'
    //            //html = html + '<td>11</td>'
    //            //html = html + '<td>12</td>'
    //            //html = html + '<td>13</td>'
    //            //html = html + '</tr>'
    //            //html = html + '</tbody >'

    //            //html = html + ' </table ></td ></tr > '


    //            $('#tblStockStatus_1>tbody>tr').eq($(thisval).closest('tr').index()).after(html);




    //        }
    //        else {
    //            $('#tblStockStatus_1>tbody>tr').eq($(thisval).closest('tr').index() + 1).html(null);
    //            $(span).attr("collapse", "true");
    //            $(span).removeClass("btn btn-info btn-sm glyphicon glyphicon-minus");
    //            $(span).addClass("btn btn-info btn-sm glyphicon glyphicon-plus");
    //        }
         

    //};

    return {
        init: function () {
            OnLoad();
        }
    };

}