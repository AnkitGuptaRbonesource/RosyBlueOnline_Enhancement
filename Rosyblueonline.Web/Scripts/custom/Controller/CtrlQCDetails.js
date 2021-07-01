var CtrlQCDetails = function () {
    var objSF = null;
    var $frmQCDetails = null;
    var dtQCDetailsload = null;
    var OnLoad = function () {
        dtQCDetailsload = new Datatable();
        objSF = new SearchFilter();
        RegisterEvents();
        SetValidation();
        QCDetailsGrid();
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd-mm-yy'

        });
        $('#txtUploadDate').datepicker('setDate', 'today');
    }

    var RegisterEvents = function () {

        $('#btnFilter').click(function (e) {
            e.preventDefault();

            QCDetailsGrid();

        });



        $(document).on('click', '#EditDetails', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var CertNo = $(this).data('certno');


            $('#lblCertNo').html(CertNo);

            $('#hfQCID').val(id);

            $('#ddlTableBlack').val("");
            $('#ddlSideBlack').val("");
            $('#ddlOpensName').val("");
            $('#ddlMilky').val("");
            $('#ddlShade').val("");
            $('#txtRemark').val("");


            uiApp.BlockUI();
            objSF.QCDetailsEdit(id).then(function (data) {
                uiApp.UnBlockUI();
                if (data.IsSuccess == true && data.Result != null) {

                    if (data.Result.tableblackinclusion != "") {
                     // $("#ddlTableBlack option:selected").text(data.Result.tableblackinclusion);
                        //$('#ddlTableBlack option').map(function () {
                        //    if ($(this).text() == data.Result.tableblackinclusion) return this;
                        //}).attr('selected', 'selected');
                        $('#ddlTableBlack').val(data.Result.tableblackinclusion);

                    }
                    if (data.Result.sideBlackInclusion != "") {
                        $('#ddlSideBlack').val(data.Result.sideBlackInclusion);

                       // $("#ddlSideBlack option:selected").text(data.Result.sideBlackInclusion);
                    }
                    if (data.Result.opensId != "") {
                        $('#ddlOpensName').val(data.Result.opensId);

                       // $("#ddlOpensName option:selected").text(data.Result.opensId);
                    }
                    if (data.Result.milkyInclusion != "") {
                        $('#ddlMilky').val(data.Result.milkyInclusion);

                       // $("#ddlMilky option:selected").text(data.Result.milkyInclusion);
                    }
                    if (data.Result.shapeID != "") {
                        $('#ddlShade').val(data.Result.shapeID);

                        //$("#ddlShade option:selected").text(data.Result.shapeID);
                    }
                    if (data.Result.Remark != "") {

                        $('#txtRemark').val(data.Result.Remark);
                    }

                    if ($frmQCDetails.valid() == false) {
                        return;
                    }
                    $('#Modal-SearchPermission').modal('show');

                } else {

                    uiApp.Alert({ container: '#uiPanel1', message: "Please try agaiin later !", type: "danger" });
                }
            }, function (error) {
                uiApp.UnBlockUI();
                uiApp.Alert({ container: '#uiPanel1', message: "Some Error Occured.", type: "danger" });
            });



        });
        $('#btnQCUpdate').click(function (e) {
            e.preventDefault();
            var QCID = $('#hfQCID').val();
            //var TableBlack = $("#ddlTableBlack option:selected").text().trim();
            //var SideBlack = $("#ddlSideBlack option:selected").text().trim();
            //var OpensName = $("#ddlOpensName option:selected").text().trim();
            //var Milky = $("#ddlMilky option:selected").text().trim();
            //var Shade = $("#ddlShade option:selected").text().trim();

            var TableBlack = $("#ddlTableBlack").val().trim();
            var SideBlack = $("#ddlSideBlack").val().trim();
            var OpensName = $("#ddlOpensName").val().trim();
            var Milky = $("#ddlMilky").val().trim();
            var Shade = $("#ddlShade").val().trim();
            var Remark = $('#txtRemark').val();


            if ($frmQCDetails.valid() == false) {
                return;
            }
            uiApp.BlockUI();
            objSF.QCDetailsUpdate(QCID, TableBlack, SideBlack, OpensName, Milky, Shade, Remark).then(function (data) {
                uiApp.UnBlockUI();
                if (data.IsSuccess == true) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Done successfully !", type: "success" });

                    $('#Modal-SearchPermission').modal('hide');

                } else {

                    uiApp.Alert({ container: '#uiPanel1', message: "Please try agaiin later !", type: "danger" });
                }
            }, function (error) {
                uiApp.UnBlockUI();
                uiApp.Alert({ container: '#uiPanel1', message: "Some Error Occured.", type: "danger" });
            });

        });

    }


    var SetValidation = function () {
        $frmQCDetails = $('#frmQCDetails');
        $frmQCDetails.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                TableBlack: {
                    required: true
                },
                SideBlack: {
                    required: true
                },
                OpensName: {
                    required: true
                },
                Milky: {
                    required: true
                },
                Shade: {
                    required: true
                }
            }
        });

        $.validator.addMethod("extension", function (value, element, param) {
            param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
            return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
        }, "Please enter a value with a valid extension.");
    }



    var QCDetailsGrid = function () {
        uiApp.BlockUI();
        dtQCDetailsload.setAjaxParam('CreatedDate', $('#txtUploadDate').val());
        dtQCDetailsload.setAjaxParam('QCStatus', $('input[type="radio"][name="QCStatus"]:checked').val());
        dtQCDetailsload.setAjaxParam('VenderName', $('#ddlFileids').val());

        if (dtQCDetailsload.getDataTable() == null || dtQCDetailsload.getDataTable() == undefined) {
            dtQCDetailsload.init({
                src: '#tblQCDetails',
                searching: true,
                dataTable: {
                    //deferLoading: 0,
                    scrollY: "400px",
                    scrollX: true,
                    paging: true,
                    order: [[0, "desc"]],
                    language: {
                        "search": "All Filters :"
                    },

                    ajax: {
                        type: 'Post',
                        url: '/Marketing/QCDetailForGrid',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "inventoryID" },
                        { data: "certificateNo" },
                        { data: "lotNumber" },
                        { data: "tableblackinclusion" },
                        { data: "sideBlackInclusion" },
                        { data: "opensId" },
                        { data: "milkyInclusion" },
                        { data: "shapeID" },
                        { data: "Remark" },
                        { data: "Status" },
                        { data: "createdOn" },
                        { data: "fileId" }
                    ],
                    columnDefs: [

                        {
                            targets: [10],
                            render: function (data, type, row) {
                                return moment(row.createdOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        },
                        {
                            targets: [11],
                            render: function (data, type, row) {
                                return "<button id='EditDetails'   class='btn btn-success' data-id='" + row.inventoryID + "' data-CertNo='" + row.certificateNo + "'><span>  <i class='fa fa-edit'></i></span> </button>";

                            },
                            orderable: true
                        }


                    ]
                },
                onCheckboxChange: function (obj) {

                }
            });
            uiApp.UnBlockUI();
        } else {
            dtQCDetailsload.getDataTable().draw();
            uiApp.UnBlockUI();
        }
        uiApp.UnBlockUI();
    };





    return {
        int: function () {
            OnLoad();
        }
    }
}();
$(document).ready(function () {
    CtrlQCDetails.int();
});