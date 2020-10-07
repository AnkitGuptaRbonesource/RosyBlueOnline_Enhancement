var CtrlMasterLayout = function () {
    var objDS = null;

    var OnLoad = function () {
        objDS = new DashboardService();
        LoadCounts();
        BindPendingOrderList("Pending");
        BindPendingOrderList("Complete");
    }

    var LoadCounts = function () {
        objDS.Count().then(function (data) {
            if (data.IsSuccess) {
                $('#spanCartCount').html(data.Result.Cart);
                $('#spanOrderCount').html(data.Result.Orders);
                $('#spanWatchlistCount').html(data.Result.WatchList);
            }
        }, function (error) {

        });
    }

     
    function BindPendingOrderList(OType) {
        objDS.GetOrderDetails(OType).then(function (data) {
            if (data.IsSuccess == true) {
                if (data.Result.length > 0) {
                    $.each(data.Result, function (i, item) {
                        var newListItem = '<div class="order-data">' +
                            '<div class="order-cnt">' +
                            '<h5>#' + item.orderDetailsId + '</h5>' +
                            '<p> ' + item.firstName + ' ' + item.lastName + '<span>(' + item.companyName + ')</span></p>' +
                            '</div>' +
                            '<div class="order-nmbr">' +
                            '<p>' + moment(item.orderCreatedOn).format("DD-MM-YY") + '</p>' +
                            '<h5></h5>' +
                            '<h5>' + item.orderPayableAmount + '</h5>' +
                            '</div></div>';


                        if (OType == "Pending") {
                            $("#PendingOrderList").append(newListItem);
                        } else {
                            $("#CompleteOrderList").append(newListItem);
                        }
                    });


                } else {

                    var NodataFound = '<div class="table-no-results text-center"> No Record Found</div >';
                    if (OType == "Pending") {
                        $("#PendingOrderList").append(NodataFound);
                    } else {
                        $("#CompleteOrderList").append(NodataFound);
                    }

                }

            }

            else {
                uiApp.Alert({ container: '#uiPanel1', message: "No Record Found.", type: "danger" });
            }
        }, function (error) {
            uiApp.Alert({ container: '#uiPanel1', message: "Some error occured.", type: "danger" });
        });

    }

    return {
        init: function () {
            OnLoad();
        },
        LoadCounts: LoadCounts
    }
}();
$(document).ready(function () {
    CtrlMasterLayout.init();
});