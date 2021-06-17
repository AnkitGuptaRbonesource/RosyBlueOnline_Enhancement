var CtrlLogin = function () {
    var ValLogin = null, ValForgetpassword = null;
    var $pnlLogin = null, $pnlForgetpassword = null;

    var objLogSvc = null;
    var latitude = null;
    var longitude = null; 

    var LocalityGeo = null;
    var CityGeo = null;
    var StateGeo = null;
    var CountryGeo = null; 

    var OnLoad = function () {
        objLogSvc = new LoginRegistrationService();
        SetValidation();
        getLocation();
         
    };

    var RegisterEvent = function () {
        $(document).on('click', '.btn-login', function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            if ($pnlLogin.valid()) {
                objLogSvc.Login(ReadForm()).then(function (data) {
                    if (data.IsSuccess == true) {  
                        myApp.token().set(data.Result.tokenID);

                        if (data.Result.RoleID == 0 && data.Result.tokenID != null) {
                            location.href = data.Result.tokenID;
                        }
                        if (data.Result.RoleID == 3) {
                            if (data.Result.IsSiteBlocked == true) { 
                                location.href = '/Home/BlockedSite'; 
                            } else {
                                location.href = '/Dashboard/Customer';
                            }
                        }
                        if (data.Result.RoleID == 2) {
                            location.href = '/Dashboard/Admin';
                        }
                        if (data.Result.RoleID == 8) {
                            location.href = '/Dashboard';
                        }
                        if (data.Result.RoleID == 9) {
                            location.href = '/Dashboard';
                            } 
                        
                    } else {
                        uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
                        uiApp.UnBlockUI();
                    }
                }, function (error) {
                        uiApp.Alert({ container: '#uiPanel', message: "Some error occured", type: "danger" });
                        uiApp.UnBlockUI();
                });
              
            }
        });

        $('#ancFP').click(function (e) {
            e.preventDefault();
            $('#Modal-Forgetpassword').modal('show');
            $('#txtForgetpasswordEmail').val('');
        });

        $('#btnForgetPassword').click(function (e) {
            e.preventDefault();
            if ($pnlForgetpassword.valid()) {
                objLogSvc.ForgetPassword($('#txtForgetpasswordEmail').val().trim()).then(function (data) {
                    if (data.IsSuccess && data.Result) {
                        uiApp.Alert({ container: '#uiPanel', message: "Please check your inbox for reset password link", type: "success" });
                    } else {
                        uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
                    }
                    $('#Modal-Forgetpassword').modal('hide');
                }, function (error) {
                    $('#Modal-Forgetpassword').modal('hide');
                    uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
                });
            }
        });
    }

    var SetValidation = function () {
        $pnlLogin = $('.login-box');
        ValLogin = $pnlLogin.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                UserName: {
                    required: true
                },
                Password: {
                    required: true
                }
            }
        });

        $pnlForgetpassword = $('#Modal-Forgetpassword');
        ValForgetpassword = $pnlForgetpassword.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                ForgetpasswordEmail: {
                    required: true,
                    regex: /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i,
                }
            }
        });
    };

    var ReadForm = function () {
        return {
            Username: $('[name=Username].login-input').val().trim(),
            Password: $('[name=Password].login-input').val().trim(),
            DeviceName: "WEB",
            Latitude: latitude,
            Longitude: longitude,
            Locality :  LocalityGeo,
            City :  CityGeo,
            State : StateGeo,
            Country : CountryGeo 
        };
    }
    //Added by Ankit 17Jun2020--Start
    var getLocation=  function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition, showError, {
                enableHighAccuracy: true,
                timeout: 5000,
                maximumAge: 0
            });
        }
        else { x.innerHTML = "Geolocation is not supported by this browser."; }
    }
     


    var showPosition = function (position) {
        latitude=  position.coords.latitude;
        longitude= position.coords.longitude;
        var GeoUrl = "https://api.bigdatacloud.net/data/reverse-geocode-client?latitude=" + position.coords.latitude + "&longitude=" + position.coords.longitude + "&localityLanguage=en";
         
        $.getJSON(GeoUrl, function (jd) {
            LocalityGeo = jd.locality;
            CityGeo = jd.city;
            StateGeo = jd.principalSubdivision;
            CountryGeo = jd.countryName; 
            
        });

       // var latlondata = position.coords.latitude + "," + position.coords.longitude;
       // var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:=" + position.coords.longitude;
       // alert(latlon)

    }

    var showError= function (error) {
        if (error.code == 1) {
           // alert("User denied the request for Geolocation.");
        }
        else if (err.code == 2) {
            alert("Location information is unavailable.");
        }
        else if (err.code == 3) {
            alert("The request to get user location timed out.");
        }
        else {
            alert("An unknown error occurred.");
        }
    }


    //Added by Ankit 17Jun2020--End
    return {
        init: function () {
            OnLoad();
            RegisterEvent();

        }
    }
}();

$(document).ready(function () {
    CtrlLogin.init();
});
