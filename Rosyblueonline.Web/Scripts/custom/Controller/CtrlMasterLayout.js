var CtrlMasterLayout = function () {
    var objDS = null, objSF = null;

    var OnLoad = function () {
        objDS = new DashboardService();
        objSF = new SearchFilter();
        
        LoadCounts();
        BindPendingOrderList("Pending");
        BindPendingOrderList("Complete");
    }

    var LoadCounts = function () {
        objDS.Count().then(function (data) {
            if (data.IsSuccess) {
                $('#spanCartCount').html(data.Result.Cart);
                if (data.Result.Cart && data.Result.Cart > 0) {
                    $('#spanCartCount').addClass("active-watchlist");
                }

                $('#spanOrderCount').html(data.Result.Orders);
                if (data.Result.WatchList && data.Result.WatchList > 0) {
                    $('#spanWatchlistCount').addClass("active-watchlist");
                }
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

                        var newListItem = '<div class="order-data-row">' +
                            '<a  class="order-data" target="_blank" href="/order/info/' + item.orderDetailsId + '"><div class="order-cnt">' +
                            '<h5>#' + item.orderDetailsId + '</h5>' +
                            '<p> ' + item.firstName + ' ' + item.lastName + '<span>(' + item.companyName + ')</span></p>' +
                            '</div>' +
                            '<div class="order-nmbr">' +
                            '<p>' + moment(item.orderCreatedOn).format("DD-MM-YY") + '</p>' +
                            '<h5></h5>' +
                            '<h5>' + item.orderPayableAmount + '</h5>' +
                            '</div></a></div>';


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


    //$('#SearchId').click(function (e) {
    //    e.preventDefault();
    //    var query = '';
    //    var ss = $('#collapse5').attr('aria-expanded');
    //    var ln = $('#collapse3').attr('aria-expanded');

       
    //    if ($("#SearchInput").val() == "" || $("#SearchInput").val() == undefined) {
    //        alert('Please enter valid input !');
    //    } else {

    //        query = ReadLotNos(false);
    //       // query = "LOTNO~" + $("#SearchInput").val() + "|CERTNO~" + $("#SearchInput").val();
    //        query = "LOTNO~" + $("#SearchInput").val();
    //        options.onSearched(query);
            
    //    }

    //});



    $('#SearchId').click(function (e) {
        e.preventDefault();
        var criteria = '';
        if ($("#LotCertSearchInput").val() == "" || $("#LotCertSearchInput").val() == undefined) {
            alert('Please enter valid input !');
        } else {

            criteria = 'LOTNO~' + $("#LotCertSearchInput").val() + '|CERTNO~' + $("#LotCertSearchInput").val();

            //alert(criteria);
            $('#hfQuery111').val(criteria);
            $('#frmPostSearch111').submit();
        }
    });

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