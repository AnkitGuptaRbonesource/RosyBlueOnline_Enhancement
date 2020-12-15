var CtrlRegistration = function () {
    var ValReg = null, ValPD = null;
    var $pnlRegistration = null, $pnlPersonalDetail = null;
    var OtpVerified = false;
    var OldEmail = "";
    var OldUserName = "";

    var objLogSvc = null;
    var regType = "";
    //var dtDocFiles = null;
    var SummeryTemp = '<tr class="rsearchnew" style="height: 25px;">\
           <td class="rsearchnew"><b><span id="lbKycDocName">${KycDocName}</span></b></td>\
        <td class="rsearchnew"><b><span id="lbkycDocNo">${kycDocNo}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblOrgFileName">${OrgFileName}</span></b></td>\
        <td class="rsearchnew"><b><span id="lbKycDocExpiryDate">${KycDocExpiryDate}</span></b></td>\
         <td class="rsearchnew"><b><span id="lblUserDocId" class=${UserDocId} value=${UserDocId} style="cursor: pointer;"><i class="fa fa-trash fontsz"></i></span></b></td>\
        </tr>';

     

    var OnLoad = function () {
        objLogSvc = new LoginRegistrationService();
        SetValidation(); 
       
       // dtDocFiles = new Datatable();


        //$('.datePicker').datepicker({
        //    format: 'dd-mm-yyyy'
        //});

        //Added by Ankit 02July2020
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd-mm-yy',
            minDate: new Date()

        });

        regType = $('#hfRegistrationType').val();
        $('#btnNext').attr('disabled', true);

        //dtDocFiles = new Datatable();

        
            GenerateRandomId();
        
    };

    var RegisterEvent = function () {
        if (regType == 'Self') {
            $(document).on('blur', '#txtEmail', function (e) {
                e.preventDefault();
                var Email = $('#txtEmail').val().trim();

                if (ValReg.element("#txtEmail")) {
                    if (OldEmail != Email) {
                        OldEmail = Email;
                        objLogSvc.GenerateOtp(Email).then(function (data) {
                            if (data.IsSuccess == true) {
                                alert('OTP Generated');
                                $('#otpDiv').show();
                                ValReg.element('#txtEmail').resetForm();
                            } else {
                                ValReg.showErrors({
                                    "emailId": data.Message,
                                });
                                $('#otpDiv').hide();
                            }
                        }, function (error) {
                            alert('Problem in generating otp')
                        });
                    }
                }
            });
        }
        $(document).on('change', '#ddlCountry', function (e) {
            e.preventDefault();
            BindState($('#ddlCountry').val());
        });

        $(document).on('click', '#btnVerifyOtp', function (e) {
            e.preventDefault();
            var Otp = $('#txtOtp').val().trim();
            var Email = $('#txtEmail').val().trim();

            objLogSvc.VerifyOtp(Email, Otp).then(function (data) {
                if (data.IsSuccess == true && data.Result == true) {
                    alert('OTP Verified');
                    $('#btnNext').removeAttr('disabled');
                    ValReg.element('#txtOtp').reset();
                } else {
                    ValReg.showErrors({
                        "otp": "OTP not verified",
                    });
                }
            }, function (error) {
            });
        });

        $(document).on('click', '#btnNext', function (e) {
            e.preventDefault();
            if ($pnlRegistration.valid()) {
                $('#MainPersonalDetail').show(); 
                $('#pnlRegistration').collapse('toggle');
            }
        });

        

        $(document).on('click', '#lblUserDocId', function (e) {
            e.preventDefault();
            //alert(e.currentTarget.innerText)
            var UserDocId = e.currentTarget.className;
            var DocRandomID = $("#hrRandomID").val();
            var text = $(this).text();
            uiApp.Confirm('Confirm Delete?', function (resp) {
                if (resp) {

                    objLogSvc.DeleteUserDoc(UserDocId, DocRandomID).then(function (data) {
                        if (data.length > 0) {
                            NewBindDocFiles(data);
                        } else {
                            $("#DocFileData").hide();
                            $("#tblBodySDocFiles").html('');
                        }
                    });
                }
            });


        });


        $(document).on('click', '#btnPrevious', function (e) {
            e.preventDefault();
            //if ($pnlRegistration.valid()) {
           //$('#pnlPersonalDetail').collapse('toggle');
            $('#MainPersonalDetail').hide();

            $('#pnlRegistration').collapse('toggle');
            //}
        });

        $(document).on('click', '#btnReLoadCaptcha', function (e) {
            e.preventDefault();
            $('#imgCaptcha').attr('src', '/Home/GetCaptchaImage');
        });

        $(document).on('blur', '#txtUsername', function (e) {
            e.preventDefault();
            var UserName = $('#txtUsername').val().trim();
            if (ValReg.element("#txtUsername")) {
                if (OldUserName != UserName) {
                    OldUserName = UserName;
                    objLogSvc.CheckUserName(UserName, 0).then(function (data) {
                        if (data.IsSuccess == true && data.Result == true) {
                            ValPD.element('#txtUsername').resetForm();
                        } else {
                            ValPD.showErrors({
                                "username": data.Message,
                            });
                        }
                    }, function (error) {
                        console.log("Problem in verifing email id")
                    });
                }
            }
        });

        $(document).on('click', '#btnSave', function (e) {
            if ($pnlPersonalDetail.valid() && $pnlRegistration.valid()) {
                var fd = ReadForm();
                objLogSvc.Register(fd).then(function (data) {
                    if (data.IsSuccess == true && data.Result == true) {
                        if (regType == 'Self') {
                            uiApp.AlertPopup('Thank you for registration!! You will get an email with your login credentials', function () {
                                location.href = '/Home';
                            });
                        }
                    } else {
                        if (data.Code == "100") {
                            alert(data.Message);
                        }
                    }
                });
            }
        });

        $(document).on('click', '#btnDocUpload', function (e) {
           // if ($DocUploadFormVal.valid() ) {
            var DocUploadFormValidation = true;

            var KycDocName = $('#ddlKycDocName').val().trim();
            var KycDocNo = $('#txtKycDocNo').val().trim();
            var DateOfDocExpiry = $('#txtDateOfDocExpiry').val().trim(); 

            if (KycDocName == undefined || KycDocName == "") {
                alert("Select Identity");
                DocUploadFormValidation = false;
            } else if (KycDocNo == undefined || KycDocNo== "") {
                alert("Kyc Doc No");
                DocUploadFormValidation = false;
            } else if (DateOfDocExpiry == undefined || DateOfDocExpiry == "") {
              //  alert("Doc Expiry Date");
              //  DocUploadFormValidation= false;
            } else
                if ($('#fuUploadFile').get(0).files.length === 0) {
                    alert("Upload Id-Proof");
                    DocUploadFormValidation = false;
                }
             
            if (DocUploadFormValidation==true) {
                
                var Dfd = ReadDocForm();
                objLogSvc.UploadMultiDoc(Dfd).then(function (data) { 
                    if (data.length> 0) { 
                        NewBindDocFiles(data);
                        DocclearForm();
                        // if (regType == 'Self') {
                        //uiApp.AlertPopup('Thank you for registration!! You will get an email with your login credentials', function () {
                        //    location.href = '/Home';
                        //});
                        // }
                    } else {
                        if (data.Code == "100") {
                            alert(data.Message);
                        }
                    }
                });
            }
            // }
        });

    }

    var BindState = function (CountryID) {
        $.ajax({
            url: '/Home/GetState?CountryID=' + CountryID,
            method: 'get'
        }).then(function (data) {
            $('#ddlState').html('');
            console.log(data);
            $.tmpl('<option value="${Value}">${Text}</option>', [{ Value: "", Text: "<-- Select State -->" }]).appendTo('#ddlState');
            $.tmpl('<option value="${Value}">${Text}</option>', data).appendTo('#ddlState');
        }, function (error) {
        });
    }

    var SetValidation = function () {
        $pnlRegistration = $('#pnlRegistration');
        $pnlPersonalDetail = $('#pnlPersonalDetail');
        ValReg = $pnlRegistration.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                companyName: {
                    required: true
                },
                address01: {
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
                    maxlength: 10 
                },
                emailId: {
                    required: true,
                    regex: /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i,
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
                Fax: {
                    maxlength: 10,
                    minlength: 10,
                    number: true
                },
                Mobile: {
                    maxlength: 10,
                    minlength: 10,
                    number: true
                },
                website: {
                    regex: /^((https?): \/\/)?(www.)?[a-z0-9]+\.[a-z]+(\/[a-zA-Z0-9#]+\/?)*$/
                },
                otp: {
                    required: true
                },
                TypeId: {
                    required: true
                }
            },
            messages: {
                emailId: {
                    regex: "Please enter valid email id",
                },
                website: {
                    regex: "Please enter valid website",
                },
                ZIP: {
                    number: "Please enter valid zip code"
                }
            }
        });

        ValPD = $pnlPersonalDetail.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                firstName: {
                    required: true
                },
                lastName: {
                    required: true
                },
                accNumber: {
                    required: true
                },
                bankName: {
                    required: true
                },
                branchName: {
                    required: true
                },
                branchAddress: {
                    required: true
                },

                //tinNo: {
                //    required: true
                //},
                //pan: {
                //    required: true
                //},
                //gstNo: {
                //    required: true
                //},
                username: {
                    required: true
                },
                password: {
                    required: true,
                    PasswordPattern: true
                },
                confirmPassword: {
                    required: true,
                    ComparePassword: true
                },
                //kycDocName: { //commented by Ankit 02July2020
                //    required: true
                //},
                //kycDocNo: {
                //    required: true
                //},
                uploadFile: {
                    required: true,
                    extension: 'pdf|jpg|jpeg|png'
                },
                Captchacode: {
                    required: true
                }
                //, //commented by Ankit 02July2020
                //dateOfDocExpiry: {
                //    required: true
                //} 
            },
            messages: {
                emailId: {
                    regex: "Please enter valid email id",
                },
                website: {
                    regex: "Please enter valid website",
                },
                ZIP: {
                    number: "Please enter valid zip code"
                }
            }
        });

        $.validator.addMethod("extension", function (value, element, param) {
            param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
            return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
        }, "Please enter a value with a valid extension.");

        $.validator.addMethod('PasswordPattern', function (value) {
            return value.match(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/);
        }, "<p>Password must contain the following:\
            <ul><li>A <b>lowercase</b> letter</li>\
            <li>A <b>capital (uppercase)</b> letter</li>\
            <li>A <b>number</b></li>\
            <li>Minimum <b>8 characters</b></li></ul>\
            </p>");

        $.validator.addMethod('ComparePassword', function (value) {
            return value === $('#txtPassword').val();
        }, "Passwords do not match");
    };

    var ReadForm = function () {
        var fd = new FormData();
        fd.append('companyName', $('#txtCompanyName').val().trim());
        fd.append('address01', $('#txtAddress').val().trim());
        fd.append('countryID', $('#ddlCountry').val());
        fd.append('stateID', $('#ddlState').val());
        fd.append('cityName', $('#txtCity').val().trim());
        fd.append('ZIP', $('#txtCompanyName').val().trim());

        //fd.append('isManufacturer', $('#chkManufacturer').prop('checked'));
        //fd.append('isWholeSaler', $('#chkWholeSaler').prop('checked'));
        //fd.append('isRetailer', $('#chkRetailer').prop('checked'));
        //fd.append('isOther', $('#chkOther').prop('checked'));

        fd.append('otherType', $('#txtOtherType').val().trim());
        fd.append('phoneCode01', $('#txtPhoneCode').val().trim());
        fd.append('phone01', $('#txtPhone').val().trim());
        fd.append('Fax', $('#txtFax').val().trim());
        fd.append('Mobile', $('#txtMobile').val().trim());
        fd.append('dateOfEstablishment', $('#txtDateOfEstablishment').val().trim());
        fd.append('emailId', $('#txtEmail').val().trim());
        fd.append('website', $('#txtWebsite').val().trim());
        fd.append('otp', $('#txtOtp').val().trim());
        fd.append('firstName', $('#txtFirstName').val().trim());
        fd.append('lastName', $('#txtLastName').val().trim());
        fd.append('accNumber', $('#txtAccNumber').val().trim());
        fd.append('bankName', $('#txtBankName').val().trim());
        fd.append('branchName', $('#txtBranchName').val().trim());
        fd.append('branchAddress', $('#txtBranchAddress').val().trim());
        fd.append('tinNo', $('#txtTinNo').val().trim());
        fd.append('pan', $('#txtPan').val().trim());
        fd.append('gstNo', $('#txtGstNo').val().trim());
        fd.append('username', $('#txtUsername').val().trim());
        fd.append('password', $('#txtPassword').val().trim());

        //fd.append('kycDocName', $('#ddlKycDocName').val().trim());  //commented by Ankit 02July2020
        //fd.append('kycDocNo', $('#txtKycDocNo').val().trim()); //commented by Ankit 02July2020

        fd.append('Captchacode', $('#txtCaptchacode').val().trim());

        fd.append('TypeId', $('#ddlRTypes').val());  /*Added By Ankit 26Jun2020*/ 
        //fd.append('dateOfDocExpiry', $('#txtDateOfDocExpiry').val().trim()); //commented by Ankit 02July2020
        fd.append('DocRandomID', $("#hrRandomID").val().trim());

        
        


        //if ($('#fuUploadFile').get(0).files.length === 0) {
        //    //alert('File not attached');
        //    console.log('File not attached');
        //    return;
        //} else {
        //    fd.append("File", $('#fuUploadFile').get(0).files[0]);
        //}
        return fd;
    }


    //Added by Ankit 02July2020

    //  var DocUploadFormValidation   = function () {
    //    if ($('#ddlKycDocName').val().trim() == undefined || $('#ddlKycDocName').val().trim() == "") {
    //        alert("Doc Name");
    //        return false;
    //    } else if ($('#txtKycDocNo').val().trim() == undefined || $('#txtKycDocNo').val().trim() == "") {
    //        alert("Doc On");
    //        return false;
    //    } else if ($('#txtDateOfDocExpiry').val().trim() == undefined || $('#txtDateOfDocExpiry').val().trim() == "") {
    //        alert("Doc Date");
    //        return false;
    //    } else
    //        if ($('#fuUploadFile').get(0).files.length === 0) {
    //            alert("Doc file");
    //            return false;
    //        }
    //    return true;
    //};


    //var DocSetValidation = function () {
    //    $DocUploadFormVal = $('#DocUploadForm'); 
    //    ValReg = $DocUploadFormVal.validate({
    //        keypress: true,
    //        onfocusout: false,
    //        rules: {
    //            KycDocId: {
    //                required: true
    //            },
    //            kycDocNo: {
    //                required: true
    //            },
    //            KycDocExpiryDate: {
    //                required: true
    //            },
    //            kycDocFile: {
    //                required: true
    //            }
                
                
    //        }
    //    });
    //};

    var ReadDocForm = function () {
        var Docfd = new FormData(); 
        Docfd.append('DocId', $('#ddlKycDocName').val().trim());
        Docfd.append('DocNo', $('#txtKycDocNo').val().trim());  
        //Docfd.append('DocExpiryDate', $('#txtDateOfDocExpiry').val().trim());
        Docfd.append('DocRandomID', $("#hrRandomID").val().trim());
       
        if ($('#fuUploadFile').get(0).files.length === 0) {
            //alert('File not attached');
            console.log('File not attached');
            return;
        } else {
            Docfd.append("DocFile", $('#fuUploadFile').get(0).files[0]);
        }
        Docfd.append('DocExpiryDate', $('#txtDateOfDocExpiry').val().trim());

        return Docfd;
    }

    function DocclearForm() {
        $('#ddlKycDocName').val('');
        $('#txtKycDocNo').val('');
        $('#txtDateOfDocExpiry').val('');
        $('#fuUploadFile').val(''); 
    }

    //var BindDocFiles = function (data) {
        
    //    if (dtDocFiles.getDataTable() == null || dtDocFiles.getDataTable() == undefined) {
    //        dtDocFiles.init({
    //            src: '#DocFIleInfoTable',
    //            dataTable: {
    //                scrollY: "70px",
    //                paging: false,
    //                order: [],
    //                processing: false,
    //                serverSide: false,
    //                data: data, 
    //                columns: [
    //                    { data: 'UserDocId', sorting: false },
    //                    { data: 'LoginId', sorting: false },
    //                    { data: 'KycDocId', sorting: false },
    //                      { data: 'kycDocNo', sorting: false },
    //                        { data: 'kycDocFile', sorting: false },
    //                          { data: 'KycDocExpiryDate', sorting: false } 
    //                ],
    //                columnDefs: [
    //                    {
    //                        targets: [0],
    //                        render: function (data, type, row) {
    //                            return row.UserDocId;
    //                        }
    //                    },
    //                    {
    //                        targets: [1],
    //                        render: function (data, type, row) {
    //                            return row.LoginId;
    //                        }
    //                    },
    //                    {
    //                        targets: [2],
    //                        render: function (data, type, row) {
    //                            return row.KycDocId;

    //                        }
    //                    },
    //                    {
    //                        targets: [3],
    //                        render: function (data, type, row) {
    //                            return row.kycDocNo;
    //                        }
    //                    },
    //                    {
    //                        targets: [4],
    //                        render: function (data, type, row) {
    //                            return row.kycDocFile;
    //                        }
    //                    },
    //                    {
    //                        targets: [5],
    //                        render: function (data, type, row) {
    //                            return row.KycDocExpiryDate;
    //                        }
    //                    }
    //                ],
    //            },
    //            onCheckboxChange: function (obj) {
    //                //ListMemo = obj;
    //            }
    //        });
    //    } else {
            
    //        dtDocFiles.getDataTable().draw();
    //    }
    //}

    var NewBindDocFiles = function (data) {
           
        if (data.length > 0) {
            
            $("#DocFileData").show();
            $("#tblBodySDocFiles").html(''); 
            $.tmpl(SummeryTemp, data).appendTo("#tblBodySDocFiles");
                 
            } else {
            $("#tblBodySDocFiles").html('');
                 
            }
        
    }

    var GenerateRandomId = function () {
        if ($("#hrRandomID").val() == "") {
            objLogSvc.GenerateRandomId().then(function (data) {
                if (data.Result != null || data.Result != undefined) {
                    $("#hrRandomID").val(data.Result);
                } else {
                    //if (data.Code == "100") {
                    //    alert(data.Message);
                    //}
                }
            });
        }

    }
    return {
        init: function () {
            OnLoad();
            RegisterEvent();

        }
    }
}();

$(document).ready(function () {
    CtrlRegistration.init();
});
