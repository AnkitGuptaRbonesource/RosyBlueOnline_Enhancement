var CtrlRFIDStockTally = function () {
    var dtStockTally = null;
    var objSvc = null;
    var InventoryCount = [];
    var Inventory = [];
    var Result = [];
    var inv = []; 
    var LoadForm = function () {
        dtStockTally = new Datatable();
        objSvc = new RFIDService();
        dtStockTally = new Datatable();
        BindStockCount([]);
       // LoadGrid([]);

       // GetStockCount($("#list").val());
    };

    var RegisterEvent = function () {
        //3005555574,3005577253,3006331403,3006573604,3006643415
        $('#btnScanRF').click(function () {
            uiApp.BlockUI();
            if ($('#txtboxname').val() != '') {
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
                    $("#list").val(uniquetaglist);
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
                    uiApp.UnBlockUI();
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
                uiApp.UnBlockUI();
            }
            else {
                uiApp.Alert({ container: '#uiPanel1', message: "Box name is mandatory", type: "error" });
                uiApp.UnBlockUI();
            }
        });

        //$('#btnScanRF').click(function () {
        //    if ($('#txtboxname').val() != '') {
        //        GetStockCount($("#list").val());
        //    }
        //    else {
        //        uiApp.Alert({ container: '#uiPanel1', message: "Box name is mandatory", type: "error" });
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
            InventoryCount = [];
            Inventory = [];
            Result = [];
            $('#list').val('');
            $('#ddlipaddress').val('192.168.20.121');
            $('#txtboxname').val('');
            $('#count').html('');
            $('#rfidtotalCount').html('');
            $('#summary').html('');
            $('#notify').html('');
            $('#tblStockCountDetail').html('');
            BindStockCount([]);
            renderData([]);
            BindTotalStockCount();
        });

        $('#btnexport').click(function (e) {
            e.preventDefault();
            if ($('#tableStockCountDetail > tbody > tr').get().length > 0) {
                $('#tableStockCountDetail').table2excel({
                    exclude: "",
                    name: "Stock Counts",
                    filename: "TallyStockCounts.xls", // do include extension
                    preserveColors: true // set to true if you want background colors and font colors preserved
                });
            }
        });
    };

    var GetStockCount = function (Rfids) {
       // var Rfids = '3004606654,3005175774,3005176224,3005231261,3005512745,3005535546,3005541477,3005570541,3006337337,3006337772,3006340463,3006347247,3006350252,3006474212,3006500210,3006504672,3006540644,3006560271,3006570305,3006576177,3006603667,3006604677,3006605604,3006613231,3006615645,3006622234,3006633121,3006635713,3006643250,3006705164,3005007735';
        if (Rfids.trim() != "") {
            objSvc.TallyStockByRFID(Rfids).then(function (data) {
                if (data.IsSuccess) {
                    Result.push(data.Result);
                    console.log(data.Result);
                    Inventory = data.Result.Inventory;
                    BindStockCount(data.Result.StockCount);
                    
                    BindTotalStockCount();
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
                }
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
            });
        }
    }

    var BindStockCount = function (data) {
        //$("#tblStockCountDetail").html('');
        InventoryCount = data;

        if (data.length > 0) {
            data[0]["BoxName"] = $('#txtboxname').val().trim();
        }
        $('#tmplStockCountDetail').tmpl(data).appendTo("#tblStockCountDetail");


        var invLst = $("#list").val().split(',');
      

        for (var i = 0; i < invLst.length; i++) {
            for (var j = 0; j < Inventory.length; j++) {
                if (invLst[i] != "") {
                    if (invLst[i] == Inventory[j].rfid) {
                        Inventory[j]["bookNo"] = $('#txtboxname').val();
                        inv.push(Inventory[j]);
                    }
                }
            }
        }
                

        renderData(inv);
    };

    var FilerData = function (type, rfid) {
        //var invLst = rfid.split(',');
        //var inv = [];

        //for (var k = 0; k < Result.length; k++) {
        //    var Inventory = Result[k].Inventory;
        //    for (var i = 0; i < invLst.length; i++) {
        //        for (var j = 0; j < Inventory.length; j++) {
        //            if (invLst[i] != "") {
        //                if (invLst[i] == Inventory[j].rfid) {
        //                    Inventory[j]["bookNo"] = $('#txtboxname').val();
        //                    inv.push(Inventory[j]);
        //                }
        //            }
        //        }
        //    }
        //}

        //renderData(inv);
        doit();
        //if (inv.length > 0) {
        //    doit();
        //} else {
        //    uiApp.Alert({ container: '#uiPanel1', message: "No valid inventory", type: "warning" });
        //}
    }

    var renderData = function (data) {
        if (dtStockTally.getDataTable() == null || dtStockTally.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();

            var cols = [
                { data: "bookNo" },
                { data: "rfid" },
                { data: "Stock" },
                { data: "CertificateNo" },
                { data: "Weight" },
                { data: "Shape" },
                { data: "Color" },
                { data: "Clarity" },
                { data: "Cut" },
                { data: "stockStatusID", class: 'ss' },
                { data: "refdata" }
            ];

            var colDefs = [
                {
                    targets: 'ss',
                    render: function (data, type, row) {
                        if (row.stockStatusID == 19) {
                            return "Invalid";
                        } else if (row.stockStatusID == 20) {
                            return "Active";
                        } else if (row.stockStatusID == 21) {
                            return "Order pending";
                        } else if (row.stockStatusID == 22) {
                            return "Sold";
                        } else if (row.stockStatusID == 23) {
                            return "On Memo";
                        } else if (row.stockStatusID == 35) {
                            return "PreOrder Blocking";
                        }
                    },
                    orderable: false
                },
            ];

            dtStockTally.init({
                src: '#SearchTablePost',
                dataTable: {
                    //deferLoading: 0,
                    paging: false,
                    order: [[8, "asc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: cols,
                    columnDefs: colDefs,
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

    var BindTotalStockCount = function () {
        var ps = "", as = "", oms = "", ss = "", ops = "", is = "";
        for (var k = 0; k < Result.length; k++) {
            ps = ps == "" ? Result[k].StockCount[0].Total.trim() : ps + "," + Result[k].StockCount[0].Total.trim();
            as = as == "" ? Result[k].StockCount[0].ActiveCount.trim() : as + "," + Result[k].StockCount[0].ActiveCount.trim();
            oms = oms == "" ? Result[k].StockCount[0].OnMemo.trim() : oms + "," + Result[k].StockCount[0].OnMemo.trim();
            ss = ss == "" ? Result[k].StockCount[0].Sold.trim() : ss + "," + Result[k].StockCount[0].Sold.trim();
            ops = ops == "" ? Result[k].StockCount[0].OrderPending.trim() : ops + "," + Result[k].StockCount[0].OrderPending.trim();
            is = is == "" ? Result[k].StockCount[0].Invalid.trim() : is + "," + Result[k].StockCount[0].Invalid.trim();
        }

        $('#btnPhysicalStock').data('rfid', ps);
        $('#btnPhysicalStock').html(ps.split(',')[0] == "" ? "0" : ps.split(',').length);
        $('#btnActiveStock').data('rfid', as);
        $('#btnActiveStock').html(as.split(',')[0] == "" ? "0" : as.split(',').length);
        $('#btnOnMemoStock').data('rfid', oms);
        $('#btnOnMemoStock').html(oms.split(',')[0] == "" ? "0" : oms.split(',').length);
        $('#btnSoldStock').data('rfid', ss);
        $('#btnSoldStock').html(ss.split(',')[0] == "" ? "0" : ss.split(',').length);
        $('#btnOrderPendingStock').data('rfid', ops);
        $('#btnOrderPendingStock').html(ops.split(',')[0] == "" ? "0" : ops.split(',').length);
        $('#btnInvalidStock').data('rfid', is);
        $('#btnInvalidStock').html(is.split(',')[0] == "" ? "0" : is.split(',').length);
    }

    var LoadGrid = function (data) {

        if (dtStockTally.getDataTable() == null || dtStockTally.getDataTable() == undefined) {
            dtStockTally.init({
                src: '#tblStockTally',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: [
                        { data: "BoxName" },
                        { data: "TotalCount" },
                        { data: "ActiveStockCount" },
                        { data: "OnMemoCount" },
                        { data: "SoldCount" },
                        { data: "OrderPending" },
                        { data: "InvalidCount" },
                        { data: "UnReferencedCount" }
                    ]
                },
                onCheckboxChange: function (obj) {

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

        $('#txtboxname').val('');
        $('#rfidtotalCount').html('');
        $('#rfidtotalCount').html('');
        $('#count').html('');
        $("#notify").html('');
        $('#Divbtnexport').show();
    }

    function doit(fn, dl) {
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
    CtrlRFIDStockTally.init();
});