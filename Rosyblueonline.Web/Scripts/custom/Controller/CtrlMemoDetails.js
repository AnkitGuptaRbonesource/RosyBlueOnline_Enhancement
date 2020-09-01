
var CtrlMemoDetails = function () { 
    var objOSvc = null;
    var dtOrderItem = null;
    var ListMemo = [];


    var OnLoad = function () {
        objOSvc = new OrderService();
        dtOrderItem = new Datatable();
        RegisterEvent();
        geturl();
    };

    var RegisterEvent = function () {
        $(document).on('click', 'a.ancMemoItems', function (e) {
            e.preventDefault();
           // var OrderID = $(e.target).attr('data-id');
            var OrderID = '81187';
            $('#hfInventoryID12').val(OrderID);
            $('#frmMemoDetails').submit();

            alert(OrderID);
            alert($('#hfInventoryID12').val());
            var name = "";
          //  var name = $(e.target).data('name');
            var disc = $(e.target).data('disc');
            $('#hfIsSellPartial').val('false');
            $('#Modal-SellFullMemo-Message').html('<b>Ref : ' + name + '</b>');
            Refname = name;
            //$('#txtMemoAvgDiscount').val(disc);
            $('#hfOrderID').val(OrderID);
            LoadOrderItems(OrderID);

            //location.href = '/Inventory/SpecificSearchPost?c=' + criteria;
        });

         

    };

    var bindMemo = function () {
        var OrderID = '81187'; 

        alert(OrderID); 
        var name = "";
        //  var name = $(e.target).data('name');
        //var disc = $(e.target).data('disc');
        $('#hfIsSellPartial').val('false');
        $('#Modal-SellFullMemo-Message').html('<b>Ref : ' + name + '</b>');
        Refname = name;
        //$('#txtMemoAvgDiscount').val(disc);
        $('#hfOrderID').val(OrderID);
        LoadOrderItems(OrderID);
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
                    data: data,
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colStruct.SpecificSearch.columnDefs,
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



    var geturl = function () {
        var sPageURL = window.location.href;

        // alert(sPageURL.substring(sPageURL.lastIndexOf('/') + 1));

        var OrderID = sPageURL.substring(sPageURL.lastIndexOf('/') + 1);

        alert(OrderID);
    };


    return {
        init: function () {
            OnLoad();
        }
    };
}();

$(document).ready(function () {
    CtrlMemoDetails.init();
});