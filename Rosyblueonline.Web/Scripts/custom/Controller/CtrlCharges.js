var CtrlCharges = function () {
    var objSvc = null; $frmCharges = null;
    var valCharges = null;

    var OnLoad = function () {
        SetValidation();
        objSvc = new ChargesService();
    }

    var RegisterEvent = function () {
        $(document).on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            if ($frmCharges.valid()) {
                var pData = ReadForm();
                uiApp.BlockUI();
                objSvc.Update(pData).then(function (data) {
                    if (data.IsSuccess) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Charges updated successfully", type: "success" });
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Charges not updated", type: "warning" });
                    }
                    uiApp.UnBlockUI();
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in updating charges", type: "danger" });
                    uiApp.UnBlockUI();
                });
            }
        });
    };

    var ClearForm = function () {
        $('#frmBlueNile').find("input[type=text], textarea").val("");
    }

    var ReadForm = function () {
        var lstInputs = $('input[type="text"]', $frmCharges).get();
        var pData = [];
        for (var i = 0; i < lstInputs.length; i++) {
            var valName = $(lstInputs[i]).attr('name');
            var value = $(lstInputs[i]).val();
            pData.push({ Key: valName, Value: value });
        }
        return pData;
    };

    var SetValidation = function () {
        var jTmpl = '"{0}":{"required":true,"percentNumber":{1},"fixedNumber":{2}}'
        var fulTmpl = '';
        $frmCharges = $('#frmCharges');
        var lstInputs = $('input[type="text"]', $frmCharges).get();
        var validationRules = [];
        for (var i = 0; i < lstInputs.length; i++) {
            var valType = $(lstInputs[i]).data('validation');
            var valName = $(lstInputs[i]).attr('name');
            var cTemp = jTmpl;
            cTemp = cTemp.replace('{0}', valName);
            cTemp = cTemp.replace('{1}', valType == "Percent" ? true : false);
            cTemp = cTemp.replace('{2}', valType == "Fixed" ? true : false);
            fulTmpl = (fulTmpl == "" ? cTemp : fulTmpl + "," + cTemp);
        }
        console.log(fulTmpl);
        validationRules = JSON.parse("{" + fulTmpl + "}");
        console.log(validationRules);

        valCharges = $frmCharges.validate({
            keypress: true,
            onfocusout: false,
            rules: validationRules
        });

        $.validator.addMethod("percentNumber", function (value, element, param) {
            return (value >= 0 && value <= 100);
        }, "Please enter valid percentage i.e min value 0 to max value 100.");

        $.validator.addMethod('fixedNumber', function (value, element, param) {
            return value.match(/^[0-9]\d*(\.\d+)?$/);
        }, "Please enter valid amount.");
    };


    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlCharges.init();
});
