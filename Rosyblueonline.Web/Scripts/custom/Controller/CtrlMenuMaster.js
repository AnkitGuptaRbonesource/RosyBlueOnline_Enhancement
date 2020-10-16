var CtrlMenuMaster = function () {
    var $frmUserDetail = null;
    var ValUserDetail = null;
    var objLRS = null;
    var dtOrder = null;
    var OnlyAddCustomer = null;
    var OnLoad = function () {

        dtOrder = new Datatable();
        objLRS = new LoginRegistrationService();
        ResetMenu();
        LoadGrid();
    };

    var RegisterEvent = function () {
         
         
        $('#ddlRoleFilter').change(function (e) {
            e.preventDefault();
            var objOrderDT = dtOrder.getDataTable();
            if (objOrderDT != null && objOrderDT != undefined) {
                dtOrder.setAjaxParam('RoleID', $('#ddlRoleFilter').val());
                objOrderDT.ajax.reload();
            }
        });

        $('#btnCancel').click(function (e) {
            e.preventDefault();
            $('#frmUserMenuAccess').hide();
            $('#frmUserDetailGrid').show();
            ResetMenu();
        });

        $(document).on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
           // clearForm();
            $('#hfUserId').val(id); 
            $('#frmUserMenuAccess').show();
            $('#frmUserDetailGrid').hide();
            ResetMenu();


            objLRS.GetMenuAccessdata(id).then(function (data) {
                if (data.IsSuccess) { 
                    data.Result[0].MenuId

                    for (var i = 0; i < data.Result.length; i++) {
                        
                        $("#" + data.Result[i].MenuId).prop("checked", true);
                    }

                } 
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel', message: "Some error occured", type: "error" });
            });
 

        });

        $('input[type=checkbox].design-checkbox').change(function (e) {
            e.preventDefault();  
            var id = e.currentTarget.id;
            if (e.currentTarget.dataset.mainmenuid == 0) {

                $('input[type=checkbox].design-checkbox').each(function () {
                    var sid = $(this).attr("id");
                    var mid = $(this).attr("data-MainMenuId")
                    if (mid == id) {
                        if ($("#" + id).is(':checked')) {
                            $("#" + sid).prop("checked", true);

                        } else {
                            $("#" + sid).prop("checked", false);
                        }
                    }

                });

            }
            else {
                var mMenuid = e.currentTarget.dataset.mainmenuid;
                $('input[type=checkbox].design-checkbox').each(function () {
                    var sid = $(this).attr("id");
                    if (sid == mMenuid) {
                        if ($("#" + id).is(':checked')) {
                            $("#" + sid).prop("checked", true);

                        }
                        //else { 
                        //    $("#" + sid).prop("checked", false);
                        //}
                    }

                });
            }
           
        }); 


        $('#btnUpdate').click(function (e) {
            e.preventDefault(); 
          var  NameID = "input[type=checkbox][name=Rights]";
            var countchk = $(NameID).length;
            var labelCheckBox = "";
            for (var ele of $(NameID)) {
                var forLabel = ele.id;
                if ($(ele).prop('checked')) {

                    labelCheckBox = (labelCheckBox == "" ? forLabel : labelCheckBox + "," + + forLabel);
                }
            }
            
            objLRS.SaveMenuAccess($('#hfUserId').val(),labelCheckBox).then(function (data) {
                if (data.IsSuccess) { 

                    $('#frmUserMenuAccess').hide();
                    $('#frmUserDetailGrid').show();
                    uiApp.Alert({ container: '#uiPanel', message: "Menu saved successfully", type: "success" });

                } else {
                    uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "error" });
                }
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel', message: "Some error occured", type: "error" });
            });


          //  alert(labelCheckBox);
              
        });

         



    };

    var ResetMenu = function () {
        $('input[type=checkbox].design-checkbox').each(function () {
            $(this).prop("checked", false);
             
        }); 

    };
      
    var LoadGrid = function () { 
            dtOrder.setAjaxParam('RoleID', $('#ddlRoleFilter').val());
            if (dtOrder.getDataTable() == null || dtOrder.getDataTable() == undefined) {
                dtOrder.init({
                    src: '#tblUserDetail',
                    searching: true,
                    dataTable: {
                        //deferLoading: 0,
                        scrollY: "400px",
                        scrollX: true,
                        paging: true,
                        order: [[1, "desc"]],
                        ajax: {
                            type: 'Post',
                            url: '/User/UserMenuDetailForGrid',
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
                            { data: "username" },
                            { data: "countryName" },
                            { data: null } 
                        ],
                        columnDefs: [{
                            targets: [0],
                            render: function (data, type, row) {
                                return row.firstName + ' ' + row.lastName;
                            }
                        }, {
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
                              //  if (row.roleID == 3 || row.roleID == 8 || row.roleID == 9) {
                                    return "<a href='#' data-id='" + row.loginID + "' class='btn-edit btn-link'>Modify</a>";
                              //  }
                               // return "";
                            },
                            orderable: true
                        } ]
                    },
                    onCheckboxChange: function (obj) {
                        CheckOrder(obj)
                    }
                });
            } else {
                dtOrder.getDataTable().draw();
            }
         
    }; 
      
    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlMenuMaster.init();
});