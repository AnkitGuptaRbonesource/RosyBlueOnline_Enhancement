var CtrlRFIDStockTallyMemo = function () {
    var dtStockTally = null;
    var objSvc = null;
    var InventoryCount = [];
    var Inventory = [];
    var LoadForm = function () {
        objSvc = new RFIDService();
        dtStockTally = new Datatable();
        //BindStockCount([]);
        //LoadGrid([]);
    };

    var RegisterEvent = function () {

        $('#btnScanRF').click(function () {
            var lastInventory = [];
            var device = new Spacecode.Device($('#ddlipaddress').val());
            $('rfidtotalCount').hide()
            $('count').show();
            var count = 0;
            var ajaxHit = 0;
            taglist = [];
            uniquetaglist = [];
            $("#notify").html('');
            $("#list").val('');
            $('#rfidtotalCount').html('');
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
                //taglist.length = 0;
                $("list").val('');
            });

            device.on('scancompleted', function () {
                device.getLastInventory(function (inventory) {
                    lastInventory = inventory;
                    //summaryElem2 = $("#notify");
                    $("#notify").html(inventory.getNumberTotal() + " tags have been scanned.");

                    // Note: with remote devices, getLastInventory can return 'null' in case of communication errors
                    console.log(inventory.getNumberTotal() + " tags have been scanned.");
                    // prints: "X tags have been scanned." (with X the result of getNumberTotal)
                    // call ajax function to insert box details                     
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
                if ($('#chkContinueMode').prop('checked') == true) {
                    $("#list").val($("#list").val() + "," + uniquetaglist);
                } else {
                    $("#list").val(uniquetaglist);
                }

                // the inventory (resulting from the scan) is ready
                console.log('uniquetaglist length : ' + uniquetaglist.length.toString());
                taglist = [];
                //uniquetaglist = [];
                console.log('taglist length : ' + taglist.length.toString());

                //count = 0;
                // summaryElem1.innerHTML = 0;
                console.log("Scan completed.");
                //if (ajaxHit == 0) {
                //    ajaxHit = 1;
                //    if ($("#list").val() != '') {
                //        GetStockCount(uniquetaglist);
                //    }
                //    else { ajaxHit = 0; }
                //}
                device = null;
                GetStockCount($("#list").val());
            });

            device.on('tagadded', function (tagUid) {
                if (tagUid) {
                    count++;
                    //AddItems(list, tag,tag)
                    //console.log("tagadded test : " + count);
                }

                //summaryElem1.innerHTML = uniquetaglist.length.toString() + "</br>";
                $("#count").html(count + "</br>");
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

        //$('#btnScanRF').click(function () {
        //    if ($("#list").val() != "") {
        //        GetStockCount($("#list").val());
        //    }
        //});

        $(document).on('click', '.btn-count-click', function (e) {
            e.preventDefault();
            var type = $(e.target).data('type');
            var rfid = $(e.target).data('rfid');
            console.log("type", type);
            console.log("rfid", rfid);
            FilerData(type, rfid);
        });

        $('#btnReset').click(function (e) {
            e.preventDefault();
            $('#list').val('');
            $('#ddlipaddress').val('192.168.20.121');
            $('#txtboxname').val('');
            $('#count').html('');
            $('#rfidtotalCount').html('');
            $('#summary').html('');
            $('#notify').html('');
            //BindStockCount([]);
            renderData([]);
        });

        $('#btnexport').click(function (e) {
            e.preventDefault();
            if ($('#SearchTablePost > tbody > tr').get().length > 0) {
                $('#SearchTablePost').table2excel({
                    exclude: "",
                    name: "Inventory",
                    filename: "MemoTallyStock.xls", // do include extension
                    preserveColors: true // set to true if you want background colors and font colors preserved
                });
            }
        });
    };

    var GetStockCount = function (Rfids) {
        var OrderID = $('#hfMemoID').val();
        objSvc.TallyMemoByRFID(OrderID, Rfids).then(function (data) {
            if (data.IsSuccess) {
                //console.log(data.Result);
                renderData(data.Result.Final);
            } else {
                uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
            }
        }, function (e) {
            uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
        });
    }


    var renderData = function (data) {
        if (dtStockTally.getDataTable() == null || dtStockTally.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            var colDef = colStruct.SpecificSearch.columnDefs;
            colDef[0]["visible"] = false;
            colDef.push({
                targets: 'dt-status',
                render: function (data, type, row) {
                    if (row.stockStatusID == 19) {
                        return 'InActive';
                    } else if (row.stockStatusID == 20) {
                        return 'Active';
                    } else if (row.stockStatusID == 21) {
                        return 'Order pending (order is waiting for admin approval)';
                    } else if (row.stockStatusID == 22) {
                        return 'Sold';
                    } else if (row.stockStatusID == 23) {
                        return 'On Memo';
                    } else if (row.stockStatusID == 24) {
                        return 'On Cart';
                    } else if (row.stockStatusID == 35) {
                        return 'PreOrder Blocking';
                    }
                    return "";
                },
                orderable: false
            });
            dtStockTally.init({
                src: '#SearchTablePost',
                dataTable: {
                    //deferLoading: 0,
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: [
                        { data: "Stock" },
                        { data: "CertificateNo" },
                        { data: "rfid" },
                        { data: "Shape" },
                        { data: "Weight" },
                        { data: "Color" },
                        { data: "Clarity" },
                        { data: "Cut" },
                        { data: "refdata" },
                        { data: "RFIDStatus" },
                        { data: null }
                    ],
                    columnDefs: colDef,
                },
                onCheckboxChange: function (obj) {
                    //ListOfChecks(obj);
                    //$('#pnlOrderConfirmation').hide();
                }
            });
        } else {
            dtStockTally.clearSelection();
            var table = dtStockTally.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }
    }

    var LoadGrid = function (data) {


        $('#rfidtotalCount').html('');
        $('#rfidtotalCount').html('');
        $('#count').html('');
        $("#notify").html('');
        $('#Divbtnexport').show();
    }

    function doit(fn, dl) {
        //var type = 'xlsx'
        //var elt = $('#SearchTablePost').get(0);
        //var wb = XLSX.utils.table_to_book(elt, { sheet: "Stock" });
        // //wb.Sheets.Stock["!ref"]

        //return dl ?
        //    XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        //    XLSX.writeFile(wb, fn || ('TallyStock.' + (type || 'xlsx')));

        $("#SearchTablePost").table2excel({
            exclude: "",
            name: "Stock",
            filename: "TallyStock.xls", // do include extension
            preserveColors: true // set to true if you want background colors and font colors preserved
        });
    }

    return {
        init: function () {
            LoadForm();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlRFIDStockTallyMemo.init();
});