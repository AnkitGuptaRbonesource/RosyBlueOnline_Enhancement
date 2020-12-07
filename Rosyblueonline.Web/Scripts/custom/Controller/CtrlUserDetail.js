var CtrlUserDetail = function () {
    var $frmUserDetail = null;
    var ValUserDetail = null;
    var objLRS = null;
    var dtOrder = null;
    var OnlyAddCustomer = null;
    var OnLoad = function () {

        dtOrder = new Datatable();
        objLRS = new LoginRegistrationService();

        OnlyAddCustomer = $('#hfOnlyAddCustomer').val();
        ChangesOnlyAddCustomer();
        LoadGrid();
    };

    var RegisterEvent = function () {

        $('#ddlRoleFilter').change(function (e) {
            e.preventDefault();
            var objOrderDT = dtOrder.getDataTable();
            if (objOrderDT != null && objOrderDT != undefined) {
                dtOrder.setAjaxParam('RoleID', $('#ddlRoleFilter').val());
            //  objOrderDT.ajax.reload();
                LoadGrid();  
            }
        });

        $(document).on('change', '#ddlCountry', function (e) {
            e.preventDefault();
            BindState($('#ddlCountry').val());
        });

        $('#btnSave').click(function (e) {
            e.preventDefault();
            $('#txtPassword').rules('add', 'required');
            if ($frmUserDetail.valid()) {
                var pData = {
                    obj: {
                        firstName: $('#txtFirstName').val().trim(),
                        lastName: $('#txtLastName').val().trim(),
                        emailId: $('#txtEmailID').val().trim(),
                        username: $('#txtUserName').val().trim(),
                        password: $('#txtPassword').val().trim(),
                        companyName: $('#txtCompanyName').val().trim(),
                        address01: $('#txtAddress').val().trim(),
                        countryID: $('#ddlCountry').val(),
                        stateID: $('#ddlState').val(),
                        cityName: $('#txtCity').val().trim(),
                        ZIP: $('#txtZip').val().trim(),
                        Mobile: $('#txtMobile').val().trim(),
                        phoneCode01: $('#txtPhoneCode').val().trim(),
                        phone01: $('#txtPhone').val().trim(),
                        accNumber: $('#txtAccountNumber').val().trim(),
                        bankName: $('#txtBankName').val().trim(),
                        branchName: $('#txtBranchName').val().trim(),
                        branchAddress: $('#txtBranchAddress').val().trim()
                    },
                    roles: $('input[type="radio"][name="optradio"]:checked').val()
                }

                if (OnlyAddCustomer == "false") {
                    objLRS.RegisterUser(pData).then(function (data) {
                        console.log(data);
                        if (data.IsSuccess) {
                            LoadGrid();
                            clearForm();
                            $('#frmUserDetail').hide();
                            $('#frmUserDetailGrid').show();
                            uiApp.Alert({ container: '#uiPanel', message: "Record added successfully", type: "success" });
                        } else {
                            uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
                    });
                } else {
                    objLRS.RegistrationViaMemo(pData).then(function (data) {
                        console.log(data);
                        if (data.IsSuccess) {
                            clearForm();
                            window.close();
                            uiApp.Alert({ container: '#uiPanel', message: "Record added successfully", type: "success" });
                        } else {
                            uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
                    });
                }
            }
        });

        $('#btnUpdate').click(function (e) {
            e.preventDefault();
            $('#txtPassword').rules('remove');
            if ($frmUserDetail.valid()) {
                var pData = {
                    obj: {
                        LoginID: $('#hfLoginID').val(),
                        firstName: $('#txtFirstName').val().trim(),
                        lastName: $('#txtLastName').val().trim(),
                        emailId: $('#txtEmailID').val().trim(),
                        username: $('#txtUserName').val().trim(),
                        password: $('#txtPassword').val().trim(),
                        companyName: $('#txtCompanyName').val().trim(),
                        address01: $('#txtAddress').val().trim(),
                        countryID: $('#ddlCountry').val(),
                        stateID: $('#ddlState').val(),
                        cityName: $('#txtCity').val().trim(),
                        ZIP: $('#txtZip').val().trim(),
                        Mobile: $('#txtMobile').val().trim(),
                        phoneCode01: $('#txtPhoneCode').val().trim(),
                        phone01: $('#txtPhone').val().trim(),
                        accNumber: $('#txtAccountNumber').val().trim(),
                        bankName: $('#txtBankName').val().trim(),
                        branchName: $('#txtBranchName').val().trim(),
                        branchAddress: $('#txtBranchAddress').val().trim()
                    },
                    roles: $('input[type="radio"][name="optradio"]:checked').val()
                }
                objLRS.UpdateRegisterUser(pData).then(function (data) {
                    console.log(data);
                    if (data.IsSuccess) {
                        LoadGrid();
                        $('#frmUserDetail').hide();
                        $('#frmUserDetailGrid').show();
                        uiApp.Alert({ container: '#uiPanel', message: "Record updated successfully", type: "success" });
                    } else {
                        uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
                });
            }
        });

        $('#btnAddUser').click(function (e) {
            e.preventDefault();
            clearForm();
            $('#frmUserDetail').show();
            $('#frmUserDetailGrid').hide();
            $('#btnSave').show();
            $('#btnUpdate').hide();
        });

        $('#btnCancel').click(function (e) {
            e.preventDefault();
            $('#frmUserDetail').hide();
            $('#frmUserDetailGrid').show();
        });

        $(document).on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            clearForm();
            objLRS.GetUserDetail(id).then(function (data) {
                if (data.IsSuccess) {
                    console.log(data.Result);
                    BindForm(data.Result);
                    $('#frmUserDetail').show();
                    $('#frmUserDetailGrid').hide();
                    $('#btnSave').hide();
                    $('#btnUpdate').show();
                } else {
                    uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "error" });
                }
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel', message: "Some error occured", type: "error" });
            });

        });

        $(document).on('click', '.btn-resetpassword', function (e) {
            e.preventDefault();
            var email = $(this).data('email');
            if (email != undefined && email != null) {
                objLRS.ForgetPassword(email).then(function (data) {
                    if (data.IsSuccess && data.Result) {
                        uiApp.Alert({ container: '#uiPanel', message: "Reset password link sent", type: "success" });
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


        $(document).on('click', '.btn-permission', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var startsizepermitted = $(this).data('startsizepermitted');
            var rowdownloadpermitted = $(this).data('rowdownloadpermitted'); 
            var isOriginFilterPermitted = $(this).data('isoriginfilterpermitted'); 
            if (startsizepermitted == null) { startsizepermitted = 0;}
            if (rowdownloadpermitted == null) { rowdownloadpermitted = 0 }
            
            $('#lblUserNameD').html($(this).data('lblusernamed'));
            $('#txtstartSizePermitted').val(startsizepermitted);
            $('#txtrowDownloadPermitted').val(rowdownloadpermitted); 
           $('input[name=OriginStatus][value=' + isOriginFilterPermitted + ']').prop('checked', true);


            $('#hfSPLoginId').val(0);
            $('#hfSPLoginId').val(id);

            $('#Modal-SearchPermission').modal('show');
        });



        $('#btnPerupdate').click(function (e) {
            e.preventDefault(); 
            var startSizePermitted=$('#txtstartSizePermitted').val().trim();
            var rowDownloadPermitted = $('#txtrowDownloadPermitted').val().trim();

            var OriginStatus = $('input[type="radio"][name="OriginStatus"]:checked').val()

            var SPLoginId = $('#hfSPLoginId').val().trim();
            if (startSizePermitted == "" || rowDownloadPermitted == "") {
                alert("Please enter details !");

            } else {
                objLRS.AddUpdateSearchPermission(startSizePermitted, rowDownloadPermitted, SPLoginId, OriginStatus).then(function (data) {
                    console.log(data);
                    if (data.IsSuccess) {
                        LoadGrid();
                        $('#Modal-SearchPermission').modal('hide');
                        
                        uiApp.Alert({ container: '#uiPanel', message: "Record save successfully", type: "success" });
                    } else {
                        uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
                });
            }
        });


    };

    var BindState = function (CountryID, StateID) {
        $.ajax({
            url: '/Home/GetState?CountryID=' + CountryID,
            method: 'get'
        }).then(function (data) {
            $('#ddlState').html('');
            console.log(data);
            $.tmpl('<option value="${Value}">${Text}</option>', [{ Value: "", Text: "<-- Select State -->" }]).appendTo('#ddlState');
            $.tmpl('<option value="${Value}">${Text}</option>', data).appendTo('#ddlState');

            if (StateID != undefined && StateID != null) {
                $('#ddlState').val(StateID);
            }
        }, function (error) {
        });
    }

    var SetValidation = function () {
        $frmUserDetail = $('#frmUserDetail');
        ValUserDetail = $frmUserDetail.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                FirstName: {
                    required: true
                },
                LastName: {
                    required: true
                },
                emailId: {
                    required: true,
                    email: true
                },
                UserName: {
                    required: true
                },
                Password: {
                    required: true
                },
                CompanyName: {
                    required: true
                },
                OfficeAddress: {
                    required: true
                },
                countryID: {
                    required: true
                },
                stateID: {
                    required: true
                },
                cityName: {
                    required: true
                },
                ZIP: {
                    required: true,
                    number: true,
                    maxlength: 10,
                    minlength: 6
                },
                phoneCode01: {
                    maxlength: 4,
                    minlength: 3
                },
                phone01: {
                    maxlength: 12,
                    minlength: 7,
                    number: true
                },
                Mobile: {
                    maxlength: 10,
                    minlength: 10,
                    number: true
                },
                AccountNumber: {
                    required: true
                },
                BankName: {
                    required: true
                },
                BranchName: {
                    required: true
                },
                BranchAddress: {
                    required: true
                }
            },
            messages: {
                ZIP: {
                    number: "Please enter valid zip code"
                }
            }
        });
    };

    var SetValidationForMemo = function () {
        $frmUserDetail = $('#frmUserDetail');
        ValUserDetail = $frmUserDetail.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                FirstName: {
                    required: true
                },
                LastName: {
                    required: true
                },
                emailId: {
                    required: true,
                    email: true
                },
                UserName: {
                    required: true
                },
                Password: {
                    required: true
                },
                CompanyName: {
                    required: true
                },
                countryID: {
                    required: true
                },
                stateID: {
                    required: true
                }
            }
        });
    };

    var LoadGrid = function () {
        if (OnlyAddCustomer == "false") {
           // alert($('#ddlRoleFilter').val());
            dtOrder.setAjaxParam('RoleID', $('#ddlRoleFilter').val());
            if (dtOrder.getDataTable() == null || dtOrder.getDataTable() == undefined) {
                dtOrder.init({
                    src: '#tblUserDetail', 
                    searching: true,
                 
                    dataTable: {
                        //deferLoading: 0,
                        scrollY: "485px",
                        scrollX: true,
                        paging: true,
                        order: [[1, "desc"]],
                        ajax: {
                            type: 'Post',
                            url: '/User/UserDetailForGrid',
                            beforeSend: function (request) {
                                var TokenID = myApp.token().get();
                                request.setRequestHeader("TokenID", TokenID);
                                return request;
                            }
                        },
                        columns: [
                            { data: "firstName" },
                            { data: "companyName" },
                            { data: "roleID" },
                            { data: "mobile" },
                            { data: "username" },
                            { data: "emailId" },
                            { data: "countryName" },
                            { data: "startSizePermitted" },
                            { data: "rowDownloadPermitted" },
                            { data: "isOriginFilterPermitted", class: 'whspace' },
                            { data: null, class: 'whspace'},
                            { data: null, class: 'whspace'},                           
                            { data: null, class: 'whspace'},
                        ],

                        columnDefs: [{
                            targets: [0],
                            render: function (data, type, row) {
                                return row.firstName + ' ' + row.lastName;
                            }
                        },
                            //{ className: "dt_col_hide", "targets": ($('#ddlRoleFilter').val().trim() == "3") ? null : [6, 7, 10] },
                            //{ className: "dt_col_show", "targets": ($('#ddlRoleFilter').val().trim() == "3") ? [6, 7, 10] : null},

                            {
                            targets: [2],
                            render: function (data, type, row) {
                                var RoleName = "";
                                switch (row.roleID) {
                                    case 1:
                                        RoleName = "SUPER ADMIN";
                                        break;
                                    case 2:
                                        RoleName = "ADMIN";
                                        break;
                                    case 3:
                                        RoleName = "CUSTOMER";
                                        break;
                                    case 4:
                                        RoleName = "SUBUSER";
                                        break;
                                    case 5:
                                        RoleName = "STOREHEAD";
                                        break;
                                    case 6:
                                        RoleName = "STORE";
                                        break;
                                    case 7:
                                        RoleName = "RESELLER";
                                        break;
                                    case 8:
                                        RoleName = "SALES PERSON";
                                        break;
                                    case 9:
                                        RoleName = "ADMIN SUPPORT";
                                        break;

                                    default:
                                        break;
                                }
                                return RoleName;
                            },
                            orderable: true
                        }, {
                            targets: ["tbl-actions"],
                            render: function (data, type, row) {
                                if (row.roleID == 3 || row.roleID == 8 || row.roleID == 9) {
                                    return "<a href='#' data-id='" + row.loginID + "' class='btn-edit btn-link'>Edit</a>";
                                }
                                return "";
                            },
                            orderable: true
                        }, {
                            targets: ["tbl-resetpassword-actions"],
                            render: function (data, type, row) {
                                return "<a href='#' data-email='" + row.emailId + "' class='btn-resetpassword btn-link'><i class='fa fa-key' aria-hidden='true'></i></a>";
                            },
                            orderable: true
                            },
                            {
                                targets: ["tbl-permission-actions"],
                                render: function (data, type, row) {
                                    if (row.roleID == 3 || row.roleID == 8 || row.roleID == 9) { 
                                        return "<a href='#' data-id='" + row.loginID + "'  data-startSizePermitted='" + row.startSizePermitted + "' data-lblusernamed='" + row.username + "'     data-rowDownloadPermitted='" + row.rowDownloadPermitted + "'  data-isOriginFilterPermitted='" + row.isOriginFilterPermitted + "'  class='btn-permission btn-link'>Add permission</a>";
                                    }
                                    return "";
                                },
                                orderable: true
                            },
                            {
                                targets: [9],
                                render: function (data, type, row) {
                                    if (row.isOriginFilterPermitted == 0) {
                                        return "Not Allowed";
                                    } else { return "Allowed";}
                                }
                            }

                        ]
                    },
                    onCheckboxChange: function (obj) {
                        CheckOrder(obj)
                    }
                });
            } else {
                dtOrder.getDataTable().draw();
            }
        }
    };

    var ChangesOnlyAddCustomer = function () {
        if (OnlyAddCustomer == "false") {
            $('#frmUserDetail').hide();
            $('#frmUserDetailGrid').show();
            $('#btnCancel').show();
            SetValidation();
        } else {
            $('#frmUserDetail').show();
            $('#frmUserDetailGrid').hide();
            $('#btnCancel').hide();
            SetValidationForMemo();
        }
    }

    var BindForm = function (data) {
        if (data.Address != null && data.Address != undefined) {
            $('#txtFirstName').val(data.Address.firstName);
            $('#txtLastName').val(data.Address.lastName);
            $('#txtCompanyName').val(data.Address.companyName);
            $('#txtAddress').val(data.Address.address01);
            $('#ddlCountry').val(data.Address.countryID);
            $('#ddlState').val();
            BindState(data.Address.countryID, data.Address.stateID)
            $('#txtCity').val(data.Address.cityName);
            $('#txtZip').val(data.Address.zipCode);
        }

        $('#txtEmailID').val(data.UserDetail.emailId);
        $('#txtUserName').val(data.Login.username);
        $('#txtPassword').val("");
        $('#txtMobile').val(data.UserDetail.mobile);
        $('#txtPhoneCode').val(data.UserDetail.phoneCode01);
        $('#txtPhone').val(data.UserDetail.phone01);
        $('#txtAccountNumber').val(data.UserDetail.accNumber);
        $('#txtBankName').val(data.UserDetail.bankName);
        $('#txtBranchName').val(data.UserDetail.branchName);
        $('#txtBranchAddress').val(data.UserDetail.branchAddress);
        $('#rbtnSupportAdmin').prop('checked', data.Login.roleID == 8 ? true : false);
        $('#rbtnCustomer').prop('checked', data.Login.roleID == 3 ? true : false);
        $('#hfLoginID').val(data.Login.loginID);
    }

    function clearForm() {
        $('#txtFirstName').val('');
        $('#txtLastName').val('');
        $('#txtCompanyName').val('');
        $('#txtAddress').val('');
        $('#ddlCountry').val('');
        $('#ddlState').val('');
        $('#txtCity').val('');
        $('#txtZip').val('');
        $('#txtEmailID').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val("");
        $('#txtMobile').val('');
        $('#txtPhoneCode').val('');
        $('#txtPhone').val('');
        $('#txtAccountNumber').val('');
        $('#txtBankName').val('');
        $('#txtBranchName').val('');
        $('#txtBranchAddress').val('');
        $('#rbtnSupportAdmin').prop('checked', false);
        $('#rbtnCustomer').prop('checked', true);
        $('#LoginID').val(0);
    }

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlUserDetail.init();
});