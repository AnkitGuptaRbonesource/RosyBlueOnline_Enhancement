var CtrlQCFinalDownload = function () {
    var objSF = null;
    var $frmFileids = null;
    var $frmFileids2 = null;
    var OnLoad = function () {
        objSF = new SearchFilter();
        RegisterEvents();
        SetValidation();
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd-mm-yy'

        });
        $('#txtFromDate').datepicker('setDate', 'today');

        $('#txtToDate').datepicker('setDate', 'today');


       GetFinalListOfFileids($('#txtFromDate').val(), $('#txtToDate').val())

       
    };

    var RegisterEvents = function () {

         


        $(document).on('click', '#btnUpdate', function (e) {
            e.preventDefault();


            if ($frmFileids.valid() == false) {
                return;
            }
            uiApp.BlockUI();
            var FileId = ReadFormFilter('Fileids');

            objSF.MInventory_QCPriceUpdate(FileId).then(function (data) {
                if (data.IsSuccess) {
                    uiApp.UnBlockUI();
                } else {
                    uiApp.UnBlockUI();
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }

            }, function (error) {
                uiApp.UnBlockUI();
            });

        });


        $(document).on('click', '#btnDownload', function (e) {
            e.preventDefault();
            if ($frmFileids2.valid() == false) {
                return;
            }
            uiApp.BlockUI();

            var FileId = ReadFormFilter('Fileids2');

            // var FileId = $('#ddlFileids2').val().trim(); 

            objSF.DownloadQCFinalDetails(FileId).then(function (data) {
                if (data.IsSuccess) {
                    $('#ancMktInventoryDownload1').get(0).click();
                    uiApp.UnBlockUI();
                } else {
                    uiApp.UnBlockUI();
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }

            }, function (error) {
                uiApp.UnBlockUI();
            });

        });

        $("#txtFromDate").change(function () {
              
            GetFinalListOfFileids2($('#txtFromDate').val(), $('#txtToDate').val())
          

        });
        $("#txtToDate").change(function () {
          
            GetFinalListOfFileids2($('#txtFromDate').val(), $('#txtToDate').val())

           
        });


        



    };


    var SetValidation = function () {
        $frmFileids = $('#frmFileids');
        $frmFileids.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                Fileids: {
                    required: true
                }
            }
        });

        $frmFileids2 = $('#frmFileids2');
        $frmFileids2.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                Fileids2: {
                    required: true
                }
            }
        });


    };



    var ReadFormFilter = function (FilterName) {


        var selected = $("#" + FilterName + " option:selected");
        var labelCheckBox = "";
        var arrSelected = [];
        selected.each(function () {
            arrSelected.push($(this).val());
            labelCheckBox += $(this).val() == '' ? $(this).val() : $(this).val() + ',';
        });

        return labelCheckBox;
    }

    function GetFinalListOfFileids(Fromdate, Todate) {
         
        objSF.GetFinalListOfFileids(Fromdate, Todate).then(function (data) {
            if (data.IsSuccess) { 
            //    $("#ddlFileids").find('option').remove();
              //$("#ddlFileids2").find('option').remove();
               //$("#ddlFileids2").empty();
              //  $("#ddlFileids").find('option').remove();

                $.each(data.Result.Fileids, function (key, value) {
                   // $("#ddlFileids").multiselect('rebuild'); 

                    $("#ddlFileids").append($("<option></option>").val(value.ID).html(value.Name));
                });
               
                $('#ddlFileids').multiselect({
                    includeSelectAllOption: true,
                    selectAllValue: 'multiselect-all',
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    maxHeight: '300',
                    buttonWidth: '235',

                }); 

                //if (data.Result.Fileids.length == 0) {
                //    $("#ddlFileids").multiselect('rebuild');

                //} 

                $('#ddlFileids2').multiselect({
                    includeSelectAllOption: true,
                    selectAllValue: 'multiselect-all',
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    maxHeight: '300',
                    buttonWidth: '235',
                });
               

                uiApp.UnBlockUI();
            } else {
                uiApp.UnBlockUI();
                uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
            }


        }, function (error) {
            uiApp.UnBlockUI();

        });


    }



    function GetFinalListOfFileids2(Fromdate, Todate) {

        objSF.GetFinalListOfFileids(Fromdate, Todate).then(function (data) {
            if (data.IsSuccess) { 
                $("#ddlFileids2").find('option').remove(); 
                $.each(data.Result.Fileids2, function (key, value) {

                    $("#ddlFileids2").append($("<option></option>").val(value.ID).html(value.Name));
                    $("#ddlFileids2").multiselect('rebuild');


                });

                $('#ddlFileids2').multiselect({
                    includeSelectAllOption: true,
                    selectAllValue: 'multiselect-all',
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    maxHeight: '300',
                    buttonWidth: '235',
                });

                if (data.Result.Fileids2.length == 0) {
                    $("#ddlFileids2").multiselect('rebuild');

                }



                uiApp.UnBlockUI();
            } else {
                uiApp.UnBlockUI();
                uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
            }


        }, function (error) {
            uiApp.UnBlockUI();

        });


    }



    return {
        int: function () {
            OnLoad();

        }
    }
}();
$(document).ready(function () {
    CtrlQCFinalDownload.int();
});