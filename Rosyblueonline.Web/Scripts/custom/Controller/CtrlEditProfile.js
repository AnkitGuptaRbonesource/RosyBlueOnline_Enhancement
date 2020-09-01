var CtrlEditProfile = function () {
    var objLRS = null;
    var OnLoad = function () {
        objLRS = new LoginRegistrationService();
        objModBA = new ModuleAddressManager({
            parentID: '#divBillingAddress',
            type: 'billing',
            instanceID: 1,
            enableSelect: false,
            onSelect: function (ent, val) {
                //OrderDetail["BillingID"] = val;
            },
            onReload: function (ent, val) {

            }
        });
        objModSA = new ModuleAddressManager({
            parentID: '#divShippingAddress',
            type: 'shipping',
            instanceID: 2,
            enableSelect: false,
            onSelect: function (ent, val) {
                //OrderDetail["ShippingID"] = val;
            },
            onReload: function (ent, val) {

            }
        });
        initModuleAddressManager();
        objLRS.GetUserDetail().then(function (data) {
            if (data.IsSuccess == true) {
                BindForm(data.Result.UserDetail);
            }
        }, function (error) {
            console.log(error);
        });
    }

    var RegisterEvent = function () {
        $('#btnUpdate').click(function (e) {
            e.preventDefault();
        });
    }

    var initModuleAddressManager = function () {
        objModBA.init();
        objModSA.init();
    }

    var BindForm = function (data) {
        $('#chkManufacturer').prop('checked', data.isManufacturer);
        $('#chkWholeSaler').prop('checked', data.isWholeSaler);
        $('#chkRetailer').prop('checked', data.isRetailer);
        $('#chkOther').prop('checked', data.isOther);
        $('#txtOtherType').val(data.otherType);
        $('#txtPhoneCode').val(data.phoneCode01);
        $('#txtPhone').val(data.phone01);
        $('#txtFax').val(data.fax01);
        $('#txtMobile').val(data.mobile);
        $('#txtDateOfEstablishment').val(moment(data.dateOfEstablishment).format(myApp.dateFormat.Client));
        $('#txtEmail').val(data.emailId);
        $('#txtWebsite').val(data.website);
        $('#txtAccNumber').val(data.accNumber);
        $('#txtBankName').val(data.bankName);
        $('#txtBranch').val(data.branchName);
        $('#txtBranchAddress').val(data.otherType);
        $('#txtTinNo').val(data.tinNo);
        $('#txtPan').val(data.pan);
        $('#txtGstNo').val(data.gstNo);
    }

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlEditProfile.init();
});