var ModuleAddressManager = function (options) {

    var objMgrSvc = null,
        ValCustAdd = null,
        FirstLoad = false;

    var SelectedItem = null;
    var ListOfItems = [];

    var $scope = null,
        $CustomerAddressFormModal = null,
        $frmCustomerAddressForm = null,
        $btnSubmitAddress = null,
        $ddlCountryID = null,
        $ddlstateID = null,
        $btnAddAddress = null;//data-addid="${AddressID}"

    var optionTemplate = '<option value="${Value}">${Text}</option>';
    var cardTemplate = '<div class="caption">\
                                     <p class="address">\
                                        {{if isDefault == true }}\
                                           \
                                        {{/if}}\
                                        ${firstName} ${lastName} <br /> \
                                        ${companyName} <br/>\
                                        ${address01}, ${address02}, ${cityName}, ${stateName}, ${countryName} - ${zipCode}\
                                        {{if isDefault == true }}\
                                            \
                                        {{/if}}\
                                     </p>\
                                        <input type="radio" class="radioleft" '+ (options.enableSelect == true ? '' : 'style="display:none;"') + ' name="CustomerAddress_' + options.instanceID + '" {{if Type == "shipping"}} value="${shippingId}" {{/if}} {{if Type == "billing"}} value="${billingId}" {{/if}} {{if isDefault == true}} checked {{/if}}  >\
                                 <div class="caption-button">\
                                        {{if Type == "shipping"}}\
                                            <button type="button" data-addid="${shippingId}" id="btn-edit-address_'+ options.instanceID + '" class="btn btn-primary btn-sm btn-edit-address">Edit</button>\
                                            <button type="button" data-addid="${shippingId}" id="btn-delete-address_'+ options.instanceID + '" class="btn btn-danger btn-sm btn-delete-address">Delete</button>\
                                        {{/if}}\
                                        {{if Type == "billing"}}\
                                            <button type="button" data-addid="${billingId}" id="btn-edit-address_'+ options.instanceID + '"  class="btn btn-primary btn-sm btn-edit-address">Edit</button>\
                                            <button type="button" data-addid="${billingId}" id="btn-delete-address_'+ options.instanceID + '" class="btn btn-danger ' + (options.type == "billing" ? 'hide' : '') + ' btn-sm btn-delete-address">Delete</button>\
                                        {{/if}}\
                              </div>\
                            </div>';


    var OnLoad = function () {

        $scope = myApp.defScope($(options.parentID));
        $CustomerAddressFormModal = $scope('#CustomerAddressFormModal_' + options.instanceID);
        $frmCustomerAddressForm = $scope('#frmCustomerAddressForm_' + options.instanceID);
        $btnSubmitAddress = $scope('#btnSubmitAddress_' + options.instanceID);
        $btnAddAddress = $scope('#btnAddAddress_' + options.instanceID);
        $ddlCountryID = $scope('#ddlCountryID_' + options.instanceID);
        $ddlstateID = $scope('#ddlstateID_' + options.instanceID);
        objMgrSvc = new AddressService(options.type);
        if (options.type == "billing") {
            $btnAddAddress.hide();
        }
        LoadAddress();
        BindCountry();
        SetValidation();
        registerEvents();
    }

    var registerEvents = function () {
        $btnAddAddress.click(function (e) {
            e.preventDefault();
            SetForm({});
            $CustomerAddressFormModal.modal('show');
            $CustomerAddressFormModal.addClass('in');
            ValCustAdd.resetForm();
        });

        $btnSubmitAddress.click(function (e) {
            e.preventDefault();
            if ($frmCustomerAddressForm.valid()) {
                var pData = GetForm();
                objMgrSvc.Add(pData).then(function (response) {
                    if (response.Result > 0) {
                        LoadAddress();
                        $CustomerAddressFormModal.modal('hide');
                    }
                }, function (error) {
                    alert('Problem in added address');
                });
            }
        });

        $(document).on('click', 'input[type=radio][name=CustomerAddress_' + options.instanceID + ']', function (e) {
            var addID = $(this).val();
            SelectedItem = addID;
            options.onSelect(e, addID);
        });

        $(document).on('click', '#btn-edit-address_' + options.instanceID, function (e) {
            e.preventDefault();
            var addID = $(this).attr('data-addid');
            objMgrSvc.GetByAddressID(addID, options.type).then(function (data) {
                if (data.IsSuccess == true) {
                    SetForm(data.Result);
                    $CustomerAddressFormModal.modal('show');
                } else {
                    uiApp.Alert({ type: 'error', message: 'Problem in fetching data' });
                }
            }, function (error) {
                uiApp.Alert({ type: 'error', message: 'Some Error Occured' });
            });
        });

        $(document).on('click', '#btn-delete-address_' + options.instanceID, function (e) {
            e.preventDefault();
            var addID = $(this).attr('data-addid');
            objMgrSvc.Delete(addID, options.type).then(function (response) {
                if (response.IsSuccess == true && response.Result > 0) {
                    LoadAddress();
                    uiApp.Alert({ type: 'success', message: 'Address deleted succcessfully' });
                } else {
                    uiApp.Alert({ type: 'error', message: 'Problem in fetching data' });
                }
            }, function (error) {
                uiApp.Alert({ type: 'error', message: 'Some Error Occured' });
            });
        });

        $ddlCountryID.change(function (e) {
            BindState($(this).val(), 0);
        });

    };

    var LoadAddress = function () {
        uiApp.BlockUI({ container: '#pnlCustomerAddressManagerModule_' + options.instanceID });
        objMgrSvc.Get().then(function (response) {

            var $lstAddress = $scope('#lstAddress_' + options.instanceID);
            $lstAddress.html('');
            console.log(response);
            $.tmpl(cardTemplate, response.Result).appendTo($lstAddress);
            options.onReload(new Event('OnReload'), options.instanceID);

            ListOfItems = response.Result;
            if (SelectedItem != null && SelectedItem != undefined) {
                var $chkBox = $scope('#lstAddress_' + options.instanceID).find('input[type=radio][value=' + SelectedItem + ']').get(0);
                if ($chkBox != null && $chkBox != undefined) {
                    $($chkBox).prop('checked', true);
                } else {
                    SelectedItem = null;
                    options.onSelect(new Event("onSelect"), 0);
                }
            } else {
                for (var i = 0; i < response.Result.length; i++) {
                    if (response.Result[i]["isDefault"] == true) {
                        var val = (response.Result[i]["billingId"] == undefined || response.Result[i]["billingId"] == 0) ? response.Result[i]["shippingId"] : response.Result[i]["billingId"];
                        var $chkBox = $scope('#lstAddress_' + options.instanceID).find('input[type=radio][value=' + val + ']').get(0);
                        options.onSelect(new Event("onSelect"), val);
                        SelectedItem = val;
                        break;
                    }
                }
            }
            uiApp.UnBlockUI('#pnlCustomerAddressManagerModule_' + options.instanceID);
        }, function (error) {
            uiApp.UnBlockUI('#pnlCustomerAddressManagerModule_' + options.instanceID);
        });
    };

    var SetValidation = function () {
        ValCustAdd = $frmCustomerAddressForm.validate({
            rules: {
                firstName: {
                    required: true,
                    minlength: 2
                },
                lastName: {
                    required: true,
                    minlength: 2
                },
                companyName: {
                    required: true,
                    minlength: 2
                },
                address01: {
                    required: true
                },
                address02: {
                    required: true
                },
                cityName: {
                    required: true
                },
                stateID: {
                    required: true
                },
                countryID: {
                    required: true
                },
                zipCode: {
                    required: true
                }
            }
        });
    };

    var SetForm = function (data) {
        $scope('#hfbillingId_' + options.instanceID).val(data["billingId"] === undefined ? '0' : data["billingId"]);
        $scope('#hfshippingId_' + options.instanceID).val(data["shippingId"] === undefined ? '0' : data["shippingId"]);
        $scope('#txtlastName_' + options.instanceID).val(data["lastName"] === undefined ? '' : data["lastName"]);
        $scope('#txtfirstName_' + options.instanceID).val(data["firstName"] === undefined ? '' : data["firstName"]);
        $scope('#txtcompanyName_' + options.instanceID).val(data["companyName"] === undefined ? '' : data["companyName"]);
        $scope('#txtaddress01_' + options.instanceID).val(data["address01"] === undefined ? '' : data["address01"]);
        $scope('#txtaddress02_' + options.instanceID).val(data["address02"] === undefined ? '' : data["address02"]);
        $scope('#ddlCountryID_' + options.instanceID).val(data["countryID"] === undefined ? '' : data["countryID"]);
        BindState(data["countryID"], data["stateID"]);
        $scope('#txtcityName_' + options.instanceID).val(data["cityName"] === undefined ? '' : data["cityName"]);
        $scope('#txtzipCode_' + options.instanceID).val(data["zipCode"] === undefined ? '' : data["zipCode"]);
        if (options.type == "billing")
            $scope('#chkDefault_' + options.instanceID).attr('disabled', true);
        else {
            $scope('#chkDefault_' + options.instanceID).attr('disabled', false);
        }
        $scope('#chkDefault_' + options.instanceID).prop('checked', (data["isDefault"] == undefined || data["isDefault"] == null) ? false : data["isDefault"]);

    };

    var GetForm = function () {
        return {
            billingId: $scope('#hfbillingId_' + options.instanceID).val().trim(),
            shippingId: $scope('#hfshippingId_' + options.instanceID).val().trim(),
            lastName: $scope('#txtlastName_' + options.instanceID).val().trim(),
            firstName: $scope('#txtfirstName_' + options.instanceID).val().trim(),
            companyName: $scope('#txtcompanyName_' + options.instanceID).val().trim(),
            address01: $scope('#txtaddress01_' + options.instanceID).val().trim(),
            address02: $scope('#txtaddress02_' + options.instanceID).val().trim(),
            countryID: $scope('#ddlCountryID_' + options.instanceID).val().trim(),
            stateID: $scope('#ddlstateID_' + options.instanceID).val().trim(),
            cityName: $scope('#txtcityName_' + options.instanceID).val().trim(),
            zipCode: $scope('#txtzipCode_' + options.instanceID).val().trim(),
            Type: options.type,
            isDefault: $scope('#chkDefault_' + options.instanceID).prop('checked')
        };
    };

    var BindCountry = function () {
        objMgrSvc.GetCountries().then(function (response) {
            $.tmpl(optionTemplate, response).appendTo($ddlCountryID);
        }, function (error) {
        });
    }

    var BindState = function (CountryID, StateID) {
        objMgrSvc.GetStates(CountryID).then(function (response) {
            $ddlstateID.html('<option value="">Select State</option>');
            $.tmpl(optionTemplate, response).appendTo($ddlstateID);
            if (StateID != 0 && StateID != undefined) {
                $ddlstateID.val(StateID);
            }
        }, function (error) {
        });
    }

    return {
        init: function () {
            OnLoad();
        }
        //getAddress: getAddress,
        //getSelectedAddress: getSelectedAddress,
        //loadAddress: LoadAddressExternal
    };

}