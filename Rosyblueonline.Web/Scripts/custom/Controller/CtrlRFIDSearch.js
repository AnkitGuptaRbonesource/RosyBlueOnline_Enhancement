var CtrlRFIDSearch = function () {
    var objSvc = null;

    var lastInventory = null;
    var taglist = [];
    var uniquetaglist = [];

    var LoadForm = function () {
        objSvc = new RFIDService();
    };

    var RegisterEvent = function () {

        $('#btnScanRF').click(function () {
            var device = new Spacecode.Device($('#ddlipaddress').val());
            $('#rfidtotalCount').hide();
            $('#count').show();
            var count = 0;
            /// device code starts
            device.on('connected', function () {
                if (device.isInitialized()) {
                    device.requestScan();
                    // Connection with the remote device established
                    // console.log('Device Ip from Dropdownlist : ' + $('#ddlipaddress').val());
                    //console.log('Device Ip : ' + document.getElementById("deviceip").value.toString());
                    console.log('Connected to Device: ' + device.getDeviceType() + ' (' + device.getSerialNumber() + ')');
                }
            });

            device.on('disconnected', function () {
                if (device.isInitialized()) {
                    // The connection to the remote device has been lost
                    console.log(device.getSerialNumber() + ' - Disconnected');
                    return;
                }
            });

            device.on('scanstarted', function () {
                count = 0;
                taglist = [];
                console.log("Scan started.");
                console.log("taglist.length." + taglist.length);
                $("#list").html('');
            });

            device.on('scancompleted', function () {
                device.getLastInventory(function (inventory) {
                    lastInventory = inventory;
                    $("notify").html(inventory.getNumberTotal() + " tags have been scanned.");

                    // Note: with remote devices, getLastInventory can return 'null' in case of communication errors
                    console.log(inventory.getNumberTotal() + " tags have been scanned.");
                    // prints: "X tags have been scanned." (with X the result of getNumberTotal)
                    //count = 0;
                    //taglist = [];
                    //uniquetaglist = [];
                });
                //document.getElementById("list").innerHTML =taglist;
                $.each(taglist, function (i, el) {
                    if ($.inArray(el, uniquetaglist) === -1) uniquetaglist.push(el);
                });

                $('#count').hide();
                $('#rfidtotalCount').html(uniquetaglist.length.toString());
                $('#rfidtotalCount').show();
                $('#list').val(uniquetaglist);
                //document.getElementById('count').style.display = 'none';
                //document.getElementById('rfidtotalCount').innerHTML = ;
                //document.getElementById('rfidtotalCount').style.display = 'block';
                //document.getElementById("list").value = uniquetaglist;
                //the inventory (resulting from the scan) is ready
                taglist = [];
                console.log('uniquetaglist length : ' + uniquetaglist.length.toString());
                console.log('taglist length : ' + taglist.length.toString());
                //count = 0;
                //summaryElem1.innerHTML = 0;
                console.log("Scan completed.");
            });

            device.on('tagadded', function (tagUid) {
                summaryElem1 = document.getElementById("count");
                if (tagUid) {
                    count++;
                    //AddItems(list, tag,tag)
                    //console.log("tagadded test : " + count);
                }

                //summaryElem1.innerHTML = uniquetaglist.length.toString() + "</br>";
                summaryElem1.innerHTML = count + "</br>";
                // a tag has been detected during the scan
                console.log("Tag scanned: " + tagUid);

                taglist[count - 1] = tagUid;

            });

            /// Device code ends 

            count = 0;
            cons = console.log(device.isConnected().toString());
            if (device.isConnected() == false) {
                //var device1 = new Spacecode.Device(document.getElementById("deviceip").value);
                //device = device1;
                device.connect();
                return false;
            }
            else {
                device.requestScan();
                return false;
            }
        });

        $('#btnSearch').click(function (e) {
            e.preventDefault();
            $("#hdnrfID").val("RFID~" + $("#list").val());
            $('#btnFormSubmit').trigger('click');
        });

        $('#btnReset').click(function (e) {
            e.preventDefault();
            $('#list').val('');
            $('#ddlipaddress').val('192.168.20.121');
            $('#hdnrfID').val(0);
            $('#summary').html('');
            $('#notify').html('');
        });

        $('#btnRecycle').click(function () {
            //console.log('getStockDetails_started');
            //console.log(stockid + ',' + boxid + ',' + lotstatus);
            var Rfidno = $("#list").val();
            $("#list").value = '';
            if (Rfidno != '') {
                objSvc.RecycleRFIds(Rfidno).then(function (data) {
                    if (data.IsSuccess) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Rfid recycled successfully", type: "success" });
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in recycling rfid", type: "error" });
                });
            }
            else {
                alert('please scan first and recycle');
                return false;
            }
        });

        $('#list').keyup(function (e) {
            Comma($(this).val(), this);
        });

    };

    function Comma(Num, ele) { //function to add commas to textboxes
        var x1 = Num.split(' ').join(',');
        x1 = x1.split('\n').join(',');
        var lastChar = x1.substring(x1.length - 1, x1.length);
        if (lastChar == ',') {
            x1 = x1.substring(0, x1.length - 1);
        }
        $(ele).val(x1);
    }


    return {
        init: function () {
            LoadForm();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlRFIDSearch.init();
});