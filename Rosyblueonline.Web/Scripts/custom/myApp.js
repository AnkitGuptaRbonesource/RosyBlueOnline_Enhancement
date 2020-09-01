var myApp = function () {
    var isIE8 = false;
    var isIE9 = false;
    var isIE10 = false;
    var doc = null;

    var http = function (options) {
        
        var defaultOptions = {
            method: 'get',
        }
        defaultOptions = $.extend(defaultOptions, options);
        var TokenID = getToken();
        if (TokenID != undefined) {
            defaultOptions["headers"] = {
                TokenID: TokenID
            }
        }

        return new Promise(function (fulfilled, reject) {
            $.ajax(defaultOptions).then(function (result) {
                fulfilled(result);
            }, function (error) {
                if (error.status == 440) {
                    //uiApp.Alert({ container: '#uiPanel1', message: "Session Timeout", type: "error" })
                    alert('Session Timeout');
                } else {
                    reject(error);
                }
            });
        });
        //return $.ajax(defaultOptions);
    }

    var getToken = function () {
        return $.cookie("TokenID");
    }

    var setToken = function (Token) {
        $.cookie("TokenID", Token, { expires: 1, path: '/' });
    }

    var defScope = function ($scope) {
        return function (selector) {
            return $scope.find(selector);
        }
    }

    var getUrlParameter = function (sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    };

    var checkSession = function () {
        setInterval(function myfunction() {
            var token = getToken();
            if (token != undefined && token != null) {
                http({
                    method: 'get',
                    url: '/Home/CheckSession'
                }).then(function (data) {
                    if (data == false) {
                        $.removeCookie("TokenID", { path: '/' });
                        //location.href = '/Home?isTo=1';
                    }
                });
            }
        }, 300000);
    }

    checkSession();

    return {
        init: function () {
        },
        http: function (options) {
            return http(options);
        },
        token: function () {
            return {
                get: getToken,
                set: setToken,
            }
        },
        defScope: defScope,
        dateFormat: {
            Client: 'DD-MM-YYYY',
            Server: 'YYYY-MM-DD'
        },
        dateTimeFormat: {
            Client: 'DD-MM-YYYY HH:mm:ss',
            Server: 'YYYY-MM-DD HH:mm:ss'
        },
        getUrlData: getUrlParameter
    }
}();