var ModuleSearchFilter = function (options) {
    var CurrentCount = 0;
    var fQuery = "";
    var selShapesCount = 1;
    var ValFrmSize = null, ValAdvSearch = null,
        $s = null, objSF = null;
   


    var OnLoad = function () {
        objSF = new SearchFilter();
        $s = myApp.defScope($(options.parentID));
        RegisterEvents();
        SetValidation();
        $('.datePicker').datepicker({
            format: 'dd-mm-yyyy'
        });
        ReadLotNos(true);
        //ReadForm(true);

        ToggleFColor(); //Added by Ankit 23JUn2020
        ToggleFClarity(); //Added by Ankit 29JUn2020
        ToggleFCertificateMore();
        ToggleFCaratSize();
        GetSizePermisiondDetails();
    }

    var RegisterEvents = function () {

        function checkButton() {
            $hfSelectedShape = $s('#hfSelectedShape');

            if (selShapesCount == $s('.diamond-box > input[type="button"][class!="all_shape"]').length) {
                $s('.diamond-box > input.all_shape[type="button"]').addClass('active');
            } else {
                $s('.diamond-box > input.all_shape[type="button"]').removeClass('active');
            }
            $hfSelectedShape.val('');
            if ($s('.diamond-box > input.all_shape[type="button"]').hasClass('active')) {
                $hfSelectedShape.val($s('.diamond-box > input.all_shape[type="button"]').val());
            } else {
                var lstOfShape = $s('.diamond-box > input[type="button"][class!="all_shape"].active');
                for (var i = 0; i < lstOfShape.length; i++) {
                    $hfSelectedShape.val($hfSelectedShape.val() == "" ? $(lstOfShape[i]).val() : $hfSelectedShape.val() + ',' + $(lstOfShape[i]).val());
                }
            }
            ReadLotNos(true);
            //ReadForm(true);
        }

        $s('.diamond-box > input[type="button"][class!="all_shape"]').click(function (e) {
            e.preventDefault();
            if ($s(this).hasClass('active')) {
                $s(this).removeClass('active');
                selShapesCount--;
            } else {
                $s(this).addClass('active');
                selShapesCount++;
            }
            checkButton();
        });

        $s('.diamond-box > input.all_shape[type="button"]').click(function (e) {
            e.preventDefault();
            var IsActive = $s(this).hasClass('active');
            $s('.diamond-box > input[class!="all_shape"][type="button"]').each(function (idx, ele) {
                if (IsActive == true) {
                    $s(ele).removeClass('active');
                    selShapesCount = 0;
                } else {
                    $s(ele).addClass('active');
                    selShapesCount = $s('.diamond-box > input[class!="all_shape"][type="button"]').length;
                }
            });
            if (IsActive == true) {
                $s('#hfSelectedShape').val('');
            } else {
                $s('#hfSelectedShape').val($(this).val());
            }
            checkButton();
        });

        $s('input[type="radio"][name="Size"]').click(function (e) {
            //if ($s(this).val() == "SingleSize") {
            //    $s('#FquickSize').hide();
            //    $s('#frmSize').show();
            //    $s('#btnAddSizes').hide();
            //    $s('#divMultipleSizes').hide();
            //} else


            if ($s(this).val() == "MultipleSize") {
                $s('#FquickSize').hide();
                //$s('#frmSize').show();
                $s('#btnAddSizes').show();
                $s('#divMultipleSizes').show();
            } else {

                $s('#FquickSize').show();
                //$s('#frmSize').hide();
                $s('#btnAddSizes').hide();
                $s('#divMultipleSizes').hide();
            }

            var countF_Certificate = $("form.F_CaratsSize > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'CaratSize' + i;
                $('#' + fieldId).prop("checked", false);

            }
            $s('#txtSizefrom').val('');
            $s('#txtSizeto').val('');
            $s('#divMultipleSizes').empty();
            ReadLotNos(true);

        });

        $s('#btnAddSizes').click(function (e) {
            e.preventDefault();
            if ($s('#frmSize').valid()) {
                var id = uiApp.getUniqueID('Size');
                var Sizefrom = $s('#txtSizefrom').val().trim();
                var Sizeto = $s('#txtSizeto').val().trim();

                var $Span = $('<span></span>').attr('id', id).attr('name', 'multiplecarat').addClass('multiplecarat').data('FromSize', Sizefrom).data('ToSize', Sizeto);
                var $anc = $('<a></a>').attr('href', '#' + id).addClass('fa fa-times');

                $Span.html(Sizefrom + " - " + Sizeto);
                $Span.append($anc);
                $($anc).click(function (e) {
                    e.preventDefault();
                    $s($s(this).attr('href')).remove();
                    ReadLotNos(true);
                    //ReadForm(true);
                });
                $s('#divMultipleSizes').append($Span);
                $s('#txtSizefrom').val('');
                $s('#txtSizeto').val('');
                ReadLotNos(true);
                //ReadForm(true);
            }
        });

        //$s('#btnAddSizes').click(function (e) { 
        //    e.preventDefault();

        //        ReadLotNos(true); 

        //});

        $s('input[type=checkbox].design-checkbox').change(function (e) {
            ReadLotNos(true);
            //ReadForm(true);
        });

        $s('input[type="radio"][name=r1]').change(function (e) {
            ReadLotNos(true);
            //ReadForm(true);
        });

        $s('#collapseExample input[type=text]').blur(function () {
            if ($s('#collapseExample').valid()) {
                ReadLotNos(true);
                //ReadForm(true);
            }
        });

        $s('#txtSizefrom, #txtSizeto').keyup(function () {
            if ($s('#frmSize').valid()) {
                ReadLotNos(true);
                //ReadForm(true);
            }
        });

        $s('#ancEx').click(function (e) {
            e.preventDefault();
            var isSelected = $(this).data('select');
            if (isSelected == true) {
                $('input[type=checkbox].exe').prop('checked', true);
                $(this).data('select', false);
            } else {
                $('input[type=checkbox].exe').prop('checked', false);
                $(this).data('select', true);
            }
            ReadLotNos(true);
            //ReadForm(true);
        });

        $s('#ancVEx').click(function (e) {
            e.preventDefault();
            var isSelected = $(this).data('select');
            if (isSelected == true) {
                $('input[type=checkbox].exe,.vGood').prop('checked', true);
                $(this).data('select', false);
                $('#ancEx').data('select', true);
            } else {
                $('input[type=checkbox].exe,.vGood').prop('checked', false);
                $(this).data('select', true);
                $('#ancEx').data('select', false);
            }
            ReadLotNos(true);
            //ReadForm(true);
        });

        $s('#NOBGM').change(function (e) {
            ReadLotNos(true);
            //ReadForm(true);
        });

        $s('#dydEdit').click(function (e) {
            e.preventDefault();
            var query = '';
            var ss = $('#collapse5').attr('aria-expanded');
            var ln = $('#collapse3').attr('aria-expanded');

            if (ss == "true") {

                //Commented By Ankit 30JUn2020
                //if ($s('#singleSize').prop('checked') == true) {
                //    if (!$s('#frmSize').valid()) {
                //        alert('Please enter sizes.');
                //        return;
                //    }
                //} else {
                //    if ($('#divMultipleSizes > span').length == 0) {
                //        if (!$s('#frmSize').valid()) {
                //            alert('Please enter sizes.');
                //            return;
                //        }
                //    }
                //}
                query = ReadLotNos(false);
                //query = ReadForm(false);
            }
            else {
                query = ReadLotNos(false);
            }

            if (options.getRequestString) {
                options.onSearched(query);
            } else {
                objSF.StockList(query).then(function (data) {
                    options.onSearched(data);
                }, function (error) {
                });
            }

        });

        $s('#ancSSCollapse').click(function (e) {
            e.preventDefault();
            $('#collapse5').collapse('show');
            $('#collapse3').collapse('hide');
        });

        $s('#ancLNCollapse').click(function (e) {
            e.preventDefault();
            $('#collapse5').collapse('hide');
            $('#collapse3').collapse('show');
            uiApp.scrollTo($('body'));
        });

        $s('#lotnumber').focus(function () {
            $s('#certNo').val('');
            $s('#fileUpload').val('');
            $s('#fileUploadCert').val('');
        });

        $s('#lotnumber').on('keyup', function () {
            Comma($(this).val(), this);
            ReadLotNos(true);
        });

        $s('#certNo').focus(function () {
            $s('#lotnumber').val('');
            $s('#fileUploadCert').val('');
            $s('#fileUpload').val('');
        });

        $s('#certNo').on('keyup', function () {
            Comma($(this).val(), this);
            ReadLotNos(true);
        });

        $s('#fileUpload').change(function () {
            $s('#fileUploadCert').val('');
            $s('#lotnumber').val('');
            $s('#certNo').val('');;
        });

        $s('#fileUploadCert').change(function () {
            $s('#fileUpload').val('');
            $s('#lotnumber').val('');
            $s('#certNo').val('');;
        });

        $s('#dydReset').click(function () {
            $s('.diamond-box > input[class!="all_shape"][type="button"]').each(function (idx, ele) {
                $s(ele).removeClass('active');
                selShapesCount = 0;
            });
            $('#certNo').val('');
            $('#lotnumber').val('');
            $('#fileUpload').val('');
            $('#fileUploadCert').val('');
        });




        //Added by Ankit 23Jun2020
        $('#FColorMore').click(function (e) {
            e.preventDefault();
            ToggleFColor();
            ReadLotNos(true);
        });

        $('#btnF_Color').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_Color > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'color' + i;
                $('#' + fieldId).prop("checked", false);

            }

            var countF_Certificate = $("form.F_FancyColor > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'fancy' + i;
                $('#' + fieldId).prop("checked", false);

            }
            // $('#fancy-colorshow').hide();

            $('#fancy-colorshow').removeClass('show-div');
            $('#chkfancy_colorshow').prop("checked", false);

            ReadLotNos(true);

        });

        $('#btnF_Certificate').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_Certificate > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'lab' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });
        $('#btnF_Clarity').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_Clarity > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'clarity' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });


        $('#btnF_Fluorescence').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_Fluorescence > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'fluor' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });

        $('#btnF_Cut').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("Div.F_Cut > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'cut' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });

        $('#btnF_Polished').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("Div.F_Polished > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'Polished' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });

        $('#btnF_Symmerty').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("Div.F_Symmerty > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'Symmerty' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });


        $('#btnF_Hearts').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_Hearts > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'HA' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });
        $('#btnF_SalesL').click(function (e) {
            e.preventDefault();
            $('#SalesLocation1').prop("checked", false);
            $('#SalesLocation10').prop("checked", false);
            $('#SalesLocation11').prop("checked", false);

            ReadLotNos(true);
        });

        $('#btnF_Origin').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_Origin > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'Origin' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });

        $('#btnF_Keytosymbol').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("Div.F_Keytosymbol > .key-rep > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'KTS' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });

        $('#btnF_FancyColor').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_FancyColor > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'fancy' + i;
                $('#' + fieldId).prop("checked", false);

            }
            ReadLotNos(true);
        });

        //Added by Ankit 29JUn2020
        $('#FClarityMore').click(function (e) {
            e.preventDefault();
            ToggleFClarity();
            ReadLotNos(true);
        });

        $('#btnF_CaratSize').click(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_CaratsSize > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'CaratSize' + i;
                $('#' + fieldId).prop("checked", false);

            }
            $s('#txtSizefrom').val('');
            $s('#txtSizeto').val('');
            $s('#divMultipleSizes').empty();

            ReadLotNos(true);
        });

        //Added by ankit 08July2020

        $s('input[type=checkbox].design-checkbox').change(function (e) {
            e.preventDefault();
            if (e.currentTarget.name == "CaratSize") {
                $s('#txtSizefrom').val('');
                $s('#txtSizeto').val('');
              //  alert(e.currentTarget.name);

                ReadLotNos(true);
            }
        });
      
   

        $('#txtSizefrom').keyup(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_CaratsSize > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'CaratSize' + i;
                $('#' + fieldId).prop("checked", false);

            }
              
            ReadLotNos(true);
        });


        $('#txtSizeto').keyup(function (e) {
            e.preventDefault();
            var countF_Certificate = $("form.F_CaratsSize > label").length;
            var fieldId = "";
            for (i = 1; i <= countF_Certificate; i++) {
                fieldId = 'CaratSize' + i;
                $('#' + fieldId).prop("checked", false);

            }
           //// alert($('#hfCaratsizepermission').val());
           // var val1 = parseFloat($s('#txtSizefrom').val());
           // var val2 = 100;
           // if ($('#hfCaratsizepermission').val()!= "") {
           //     val2 = parseFloat($('#hfCaratsizepermission').val()); 
           // }
           //// alert(val2);
           // if(val1 > val2) {
           //     alert("From unit should not be greater than " + val2 + " unit");
           //     $s('#txtSizefrom').val('');
           // }
 

            ReadLotNos(true);
        });

        $('#FCaratSize').click(function (e) {
            e.preventDefault();
            ToggleFCaratSize();
            ReadLotNos(true);
        });

        $('#FCertificateMore').click(function (e) {
            e.preventDefault();
            ToggleFCertificateMore(); 
            ReadLotNos(true);
        });
    };

    var SetValidation = function () {
        ValFrmSize = $s('#frmSize').validate({
            keypress: true,
            onfocusout: false,
            rules: {
                Sizefrom: {
                    required: true,
                    number: true,
                    accesssize: true

                },
                Sizeto: {
                    required: true,
                    notGreaterThan: true, 
                    number: true 
                }
            },
            messages: {
                Sizefrom: {
                    required: '*'
                },
                Sizeto: {
                    required: '*'
                }
            }
        });

        ValAdvSearch = $s('#collapseExample').validate({
            keypress: true,
            onfocusout: false,
            rules: {
                GRDLPERFROM: {
                    number: true
                },
                GRDLPERTO: {
                    notGreaterThan: true,
                    number: true
                },
                TBLPERFROM: {
                    number: true
                },
                TBLPERTO: {
                    notGreaterThan: true,
                    number: true
                },
                DEPTHPERFROM: {
                    number: true
                },
                DEPTHPERTO: {
                    notGreaterThan: true,
                    number: true
                },
                DISCPERFROM: {
                    number: true
                },
                DISCPERTO: {
                    notGreaterThan: true,
                    number: true
                },
                PRICEPERCTFROM: {
                    number: true
                },
                PRICEPERCTTO: {
                    notGreaterThan: true,
                    number: true
                },
                TOTAMOUNTFROM: {
                    number: true
                },
                TOTAMOUNTTO: {
                    notGreaterThan: true,
                    number: true
                },
                LENFROM: {
                    number: true
                },
                LENTO: {
                    notGreaterThan: true,
                    number: true
                },
                WIDTHFROM: {
                    number: true
                },
                WIDTHTO: {
                    notGreaterThan: true,
                    number: true
                },
                DPTHFROM: {
                    number: true
                },
                DPTHTO: {
                    notGreaterThan: true,
                    number: true
                },
                CRWNAGLFROM: {
                    number: true
                },
                CRWNAGLTO: {
                    notGreaterThan: true,
                    number: true
                },
                CRWNHGHTFROM: {
                    number: true
                },
                CRWNHGHTTO: {
                    notGreaterThan: true,
                    number: true
                },
                PVNLAGLFROM: {
                    number: true
                },
                PVNLAGLTO: {
                    notGreaterThan: true,
                    number: true
                },
                PVNLDPTHFROM: {
                    number: true
                },
                PVNLDPTHTO: {
                    notGreaterThan: true,
                    number: true
                },
                STRLENFROM: {
                    number: true
                },
                STRLENTO: {
                    notGreaterThan: true,
                    number: true
                },
                LWRHLFFROM: {
                    number: true
                },
                LWRHLFTO: {
                    notGreaterThan: true,
                    number: true
                },
                RPTDTFROM: {
                    validateDate: true,
                },
                RPTDTTO: {
                    validateDate: true,
                    dateNotGreaterThan: true
                }
            },
        });

        $.validator.addMethod('notGreaterThan', function (value, ele) {
            var compareTo = $(ele).attr('compareTo');
            var ToSize = parseFloat(value == "" ? "0" : value);
            var FromSize = parseFloat($s(compareTo).val().trim() == "" ? "0" : $s(compareTo).val().trim());
            return FromSize <= ToSize;
        }, "To unit should not be greater than From unit");

        $.validator.addMethod('validateDate', function (value, ele) {
            if (value != "") {
                var aDate = moment(value, myApp.dateFormat.Client, true);
                return aDate.isValid();
            } else {
                return true;
            }
        }, "Invalid Date");

        $.validator.addMethod('dateNotGreaterThan', function (value, ele) {
            var compareTo = $(ele).attr('compareTo');
            var ToDate = moment(value, myApp.dateFormat.Client);
            var FromDate = moment($s(compareTo).val(), myApp.dateFormat.Client);
            var DaysCount = ToDate.diff(FromDate, 'days') // 1
            if (ToDate.isValid() && FromDate.isValid()) {
                return DaysCount >= 0;
            }
            return true;
        }, "To date should be greater than From date");

       // var msg = "From unit should be greater than " + $s('#txtSizefrom').val();
       // var msg = "From unit should be greater ";

        $.validator.addMethod('accesssize', function (value, ele) {
           
            var FromSize = parseFloat($s('#txtSizefrom').val());
             
            var ToSize = 0;
            if ($('#hfCaratsizepermission').val()!= "") {
                ToSize = parseFloat($('#hfCaratsizepermission').val()); 
            }
            
            if (FromSize >= ToSize) {
                $('#SizeMsg').text("");
               // $('#btnSubmit').removeAttr("disabled");
                $('#dydEdit').attr("disabled", false); 


            } else {
                $('#SizeMsg').text("From unit should be Equal or greater than " + $('#hfCaratsizepermission').val());
                $('#dydEdit').attr("disabled", true);
            }
           // $('#SizeMsg').val("From unit should be Equal or greater than " + $('#hfCaratsizepermission').val());
               // alert("From unit should be greater than " + val2 + " unit"); 
            return FromSize >= ToSize;
        },"");
    };
    
    var ReadForm = function (getCount) {
         //Added By Ankit 30JUn2020 --CaratSize
        var SelectedShapes = $s('#hfSelectedShape').val();
        var ArrayID = ["color", "fancy", "clarity", "fluor", "lab", "girdle", "mlky", "cb", "tb", "ec", "HA", "OPN", "SHADE", "KTS", "cut", "Polished", "Symmerty", "SLocation", "CaratSize","Origin"];
        var KeyID = ["CLR", "FNC", "CTY", "FLORA", "CERT", "GRDL", "MILKY", "CB", "TB", "EYECLN", "HA", "OPN", "SHADE", "KTS", "CUT", "POL", "SYMT", "SLOCATION", "MULTISIZE","ORGN"];


        var CheckBox = "";
        var displayCheckBox = "";
        // Read All Checkboxes for filter --------Start-------------
        for (var k = 0; k < ArrayID.length; k++) {
            var innerStr = "";
            var labelCheckBox = "";
            //NameID = "input[type=checkbox][name=" + ArrayID[k] + "]:checked";
            NameID = "input[type=checkbox][name=" + ArrayID[k] + "]";
            countchk = $s(NameID).length;
            for (var ele of $s(NameID)) {
                var forLabel = ele.id;
                if ($s('#KTSNotcontains').prop('checked') && ArrayID[k] == "KTS") {
                    if (!$(ele).prop('checked')) {
                        innerStr = (innerStr == "" ? $(ele).val() : innerStr + "," + $(ele).val());
                        labelCheckBox = (labelCheckBox == "" ? $('label[for=' + forLabel + ']').html() : innerStr + "," + $('label[for=' + forLabel + ']').html());
                    }
                } else {
                    if ($(ele).prop('checked')) {
                        innerStr = (innerStr == "" ? $(ele).val() : innerStr + "," + $(ele).val());
                        labelCheckBox = (labelCheckBox == "" ? $('label[for=' + forLabel + ']').html() : labelCheckBox + "," + $('label[for=' + forLabel + ']').html());
                    }
                }
            }
            if (innerStr != "") {
                CheckBox += KeyID[k] + "~" + innerStr + "|";
                displayCheckBox += "~" + labelCheckBox;
            }

        }

        // Read All Checkboxes for filter --------End---------------


        // Read Carat for filter --------Start-------------
        var Size = "";
        var displaySize = "";
        var SelectedSize = $s('input[type=radio][name=Size]:checked').val();
        if (SelectedSize == "SingleSize") {
            if ($s('#txtSizefrom').val().trim() != "" && $s('#txtSizeto').val().trim() != "") {
                Size = "CRTFRM~" + $s('#txtSizefrom').val().trim() + "|CRTTO~" + $s('#txtSizeto').val().trim();
                displaySize = $s('#txtSizefrom').val().trim() + '-' + $s('#txtSizeto').val().trim()
            }
        } else if (SelectedSize == "MultipleSize")  {
            var listOfCarats = $s('#divMultipleSizes > span.multiplecarat').get();
            for (var i = 0; i < listOfCarats.length; i++) {
                Size = (Size == "" ?
                    ($(listOfCarats[i]).data('FromSize') + '-' + $(listOfCarats[i]).data('ToSize'))
                    : (Size + "," + $(listOfCarats[i]).data('FromSize') + '-' + $(listOfCarats[i]).data('ToSize')));
                displaySize = (displaySize == "" ?
                    ($(listOfCarats[i]).data('FromSize') + '-' + $(listOfCarats[i]).data('ToSize'))
                    : (displaySize + "," + $(listOfCarats[i]).data('FromSize') + '-' + $(listOfCarats[i]).data('ToSize')));

            }
            if (Size != "") {
                Size = "MULTISIZE~" + Size;
            }
        }
        //New added by Ankit 08July2020
        if ($s('#txtSizefrom').val().trim() != "" && $s('#txtSizeto').val().trim() != "") {
            Size = "CRTFRM~" + $s('#txtSizefrom').val().trim() + "|CRTTO~" + $s('#txtSizeto').val().trim();
            displaySize = $s('#txtSizefrom').val().trim() + '-' + $s('#txtSizeto').val().trim()
        }


        // Read Carat for filter --------End---------------

        // Read All TextBox for filter in Advance Search --------Start-------------
        var listOfTextBox = $s('#collapseExample input[type=text]').get();
        var Textinputs = "";
        for (var i = 0; i < listOfTextBox.length; i++) {
            if (listOfTextBox[i].value.trim() != '')
                Textinputs = (Textinputs == "" ?
                    listOfTextBox[i].name + '~' + listOfTextBox[i].value.trim() :
                    Textinputs + '|' + listOfTextBox[i].name + '~' + listOfTextBox[i].value.trim());
        }

        // Read All TextBox for filter in Advance Search --------End-------------
        var NoBGM = "";
        if ($s('#NOBGM').prop('checked')) {
            NoBGM = 'NOBGM~1|';
        }


        //// Read CUT - POLISH - SYMMETRY for filter ------Start-------
        //"input[type=checkbox][name=cut]:checked";
        //// Read CUT - POLISH - SYMMETRY for filter ------End-------
        var FinalQuery = (SelectedShapes == "" ? "" : "SHP~" + SelectedShapes + "|")
            + (Size == "" ? Size : Size + "|")
            + (CheckBox)
            + (NoBGM)
            + (Textinputs)
        //console.log(SelectedShapes + '|' + Size + '|' + dynamicstring + '|' + Textinputs);
        console.log(FinalQuery);

        if (getCount == true) {
            GetCount(FinalQuery);
        }

        fQuery = FinalQuery;

        return {
            count: CurrentCount,
            query: FinalQuery,
            displayQuery: (displaySize + displayCheckBox)
        };
    };

    var ReadLotNos = function (getCount) {
        var FinalQuery = '';
        var SSForm = ReadForm(false);
        if (SSForm != null && SSForm != undefined) {
            FinalQuery = SSForm.query;
        }

        if ($s('#lotnumber').val() != "") {
            FinalQuery = FinalQuery == "" ? 'LOTNO~' + $s('#lotnumber').val() : FinalQuery + '|LOTNO~' + $s('#lotnumber').val();
        }

        if ($s('#certNo').val() != "") {
            FinalQuery = FinalQuery == "" ? 'CERTNO~' + $s('#certNo').val() : FinalQuery + '|CERTNO~' + $s('#certNo').val();
        }

        if (getCount == true) {
            GetCount(FinalQuery);
        }

        console.log(FinalQuery);

        fQuery = FinalQuery;

        return {
            count: CurrentCount,
            query: FinalQuery,
            displayQuery: ''
        };
    }

    function GetCount(FinalQuery, newArrival, callback) {
        $('#dydEdit').attr('disable', true);
        objSF.StockCount(FinalQuery, newArrival).then(function (data) {
            console.log(data);
            if (data.IsSuccess) {
                CurrentCount = data.Result;
                $s('#dydEdit > span.badge').html(CurrentCount[0]["ResultCount"]);
                if (callback != undefined) {
                    callback({
                        count: CurrentCount,
                        query: FinalQuery,
                        displayQuery: ""
                    });
                }
            } else {
                $s('#dydEdit > span.badge').html("0");
            }
            $('#dydEdit').attr('disable', false);
        }, function (error) {
            $('#dydEdit').attr('disable', false);
        });
    };

    function Comma(Num, ele) { //function to add commas to textboxes
        var x1 = Num.split(' ').join(',');
        $(ele).val(x1);
    }

    var setCount = function (count) {
        //ChangeCount(count);
        $s('#dydEdit > span.badge').html(count);
    };


    //Added by Ankit 23JUn2020
    function ToggleFColor() {
        var countcolor = $("form.F_Color > label").length;

        var fieldId = "";
        for (i = 11; i <= countcolor; i++) {
            fieldId = 'color' + i;
            $("label[for='" + fieldId + "']").toggle();
            $('#' + fieldId).prop("checked", false);

        }
        if ($("#FColorMore").val() == 'More') {
            $("#FColorMore").prop('value', 'Less');
        } else {
            $("#FColorMore").prop('value', 'More');

        }


    }
    function ToggleFClarity() {
        var countcolor = $("form.F_Clarity > label").length;

        var fieldId = "";
        for (i = 9; i <= countcolor; i++) {
            fieldId = 'clarity' + i;
            $("label[for='" + fieldId + "']").toggle();
            $('#' + fieldId).prop("checked", false);

        }
        if ($("#FClarityMore").val() == 'More') {
            $("#FClarityMore").prop('value', 'Less');
        } else {
            $("#FClarityMore").prop('value', 'More');

        }


    }

    function ToggleFCertificateMore() {
        var countcolor = $("form.F_Certificate > label").length;

        var fieldId = "";
        for (i = 5; i <= countcolor; i++) {
            fieldId = 'lab' + i;
            $("label[for='" + fieldId + "']").toggle();
            $('#' + fieldId).prop("checked", false);

        }
        if ($("#FCertificateMore").val() == 'More') {
            $("#FCertificateMore").prop('value', 'Less');
        } else {
            $("#FCertificateMore").prop('value', 'More');

        }


    }


    function ToggleFCaratSize() {
        var countcolor = $("form.F_CaratsSize > label").length;

        var fieldId = "";
        for (i = 15; i <= countcolor; i++) {
            fieldId = 'CaratSize' + i;
            $("label[for='" + fieldId + "']").toggle();
            $('#' + fieldId).prop("checked", false);

        }
        if ($("#FCaratSize").val() == 'More') {
            $("#FCaratSize").prop('value', 'Less');
        } else {
            $("#FCaratSize").prop('value', 'More');

        }


    }
    function GetSizePermisiondDetails() {
        objSF.GetSizePermision().then(function (data) { 
            $('#hfCaratsizepermission').val(data.startSizePermitted);
               
        }, function (error) {
        });
    }


    return {
        init: function () {
            OnLoad();
        },
        setCount: setCount,
        setQuery: function (query, newArrival, callback) {
            GetCount(query, newArrival, callback);
        },
        getQuery: function () {
            return ReadForm(false);
        }
    }
};