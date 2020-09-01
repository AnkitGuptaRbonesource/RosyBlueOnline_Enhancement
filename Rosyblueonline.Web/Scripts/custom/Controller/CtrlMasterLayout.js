var CtrlMasterLayout = function () {
    var objDS = null;

    var OnLoad = function () {
        objDS = new DashboardService();
        LoadCounts();
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