var CtrlGIAUpload = function () {
    var $frmGIAInventory = null;
    var objSF = new SearchFilter();
    var ValGIA = null;
    var dtTable = null;
    var allData = [];
    var OnLoad = function () {
        SetValidation();
        dtTable = new Datatable();
    }

    var RegisterEvents = function () {

        $('#btnSubmit').click(function (e) {
            e.preventDefault();
            if ($frmGIAInventory.valid()) {
                var fd = new FormData();
                fd.append("File", $('#fuExcel').get(0).files[0]);
                uiApp.BlockUI();
                objSF.GetGIADataFromExcel(fd).then(function (data) {
                    if (data.IsSuccess) {
                      //GetDataFromApi(data.Result);
                         TestApi();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "warning" });
                        uiApp.UnBlockUI();
                    }
                }, function () {
                    uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "warning" });
                    uiApp.UnBlockUI();
                });
            }
        });

        //$('#btnDownload').click(function (e) {
        //    e.preventDefault();
        //    if (dtTable.getDataTable() != null && dtTable.getDataTable() != undefined) {
        //        doit();
        //    }
        //});
    }

    var TestApi = function () {


        var url = "https://api.reportresults.gia.edu";

        var xhr = new XMLHttpRequest();
        xhr.open("POST", url);

        xhr.setRequestHeader("Authorization", "68bd3cfb-f113-41e1-896b-fb98527f8742");
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.setRequestHeader("Access-Control-Allow-Origin", "*");
        xhr.setRequestHeader("Access-Control-Allow-Methods", "GET,PUT,POST,DELETE,PATCH,OPTION");
        xhr.setRequestHeader("Access-Control-Allow-Headers", "origin, x-requested-with, accept, x-api-key");
         
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
                console.log(xhr.status);
                console.log(xhr.responseText);
            }
        };

        var data = ' {"query":" query ReportQuery($ReportNumber: String!) {     getReport(report_number: $ReportNumber){ report_number  report_date   report_type results { __typename  ... on DiamondGradingReportResults { shape_and_cutting_style  measurements  carat_weight  color_grade color_origin color_distribution  clarity_grade cut_grade  polish  symmetry  fluorescence  clarity_characteristics  inscriptions   report_comments      proportions {  depth_pct   table_pct  crown_angle  crown_height   pavilion_angle  pavilion_depth  star_length lower_half  girdle culet        }  }   }    quota {   remaining   }   } }","variables":{"ReportNumber":"2141438171"}}';

        xhr.send(data);
        uiApp.UnBlockUI();
    }

    var GetDataFromApi = function (pData) {
        var ListOfData = [];
        var data = [];
        //var idx = 0;

        for (var i = 0; i < pData.length; i++) {
            data.push({
                reportNo: pData[i].Certificate,
                //reportWeight: pData[i].Weight,
            });

            objSF.GetDataFromGiaApi($('#hfIP').val(), data[i]).then(function (rData) {
                var idx = 0;
                for (var k = 0; k < pData.length; k++) {
                    if (pData[k].Certificate == rData.reportDtls.reportDtl[0]["reportNo"]) {
                        idx = k;
                        break;
                    }
                }
                if (rData.status == "SUCCESS") {
                    ListOfData.push(rData.reportDtls.reportDtl[0]);
                    ListOfData[ListOfData.length - 1]["lotno"] = pData[idx].Lotnumber;
                    ListOfData[ListOfData.length - 1]["lotno"] = pData[idx].Lotnumber;
                    ListOfData[ListOfData.length - 1]["certificate"] = pData[idx].Certificate;
                    //ListOfData[ListOfData.length - 1]["crnHt"] = (ListOfData[ListOfData.length - 1]["crnHt"] * 100) + '%';
                    //ListOfData[ListOfData.length - 1]["pavDp"] = (ListOfData[ListOfData.length - 1]["pavDp"] * 100) + '%';
                    //ListOfData[ListOfData.length - 1]["starLn"] = (ListOfData[ListOfData.length - 1]["starLn"] * 100) + '%';
                    //ListOfData[ListOfData.length - 1]["lrHalf"] = (ListOfData[ListOfData.length - 1]["lrHalf"] * 100) + '%';
                    //ListOfData[ListOfData.length - 1]["girdlePct"] = (ListOfData[ListOfData.length - 1]["girdlePct"] * 100) + '%';
                    ListOfData[ListOfData.length - 1]["status"] = "Success";
                    ListOfData[ListOfData.length - 1]["lab"] = "GIA";
                    idx++;
                } else {
                    ListOfData[ListOfData.length - 1]["lotno"] = pData[idx].Lotnumber;
                    ListOfData[ListOfData.length - 1]["certificate"] = pData[idx].Certificate;
                    ListOfData[ListOfData.length - 1]["lab"] = "";
                    ListOfData[ListOfData.length - 1]["status"] = "Not Found";
                    idx++;
                }
                console.log(ListOfData);
                if (pData.length == ListOfData.length) {
                    renderData(ListOfData)
                }
            }, function () {
                uiApp.UnBlockUI();
            });
        }
    }

    var renderData = function (data) {
        allData = data;
        //for (var i = 0; i < data.length; i++) {
        //    data[i].certificate = data[i].inscription.replace('GIA ', '').replace(' ', '');
        //}
        uiApp.UnBlockUI();
        $('#pnlTable').show();
        if (dtTable.getDataTable() == null || dtTable.getDataTable() == undefined) {
            dtTable.init({
                src: '#myTable',
                dataTable: {
                    dom: "<'row'<'col-md-6 col-sm-12'Bli><'col-md-6 col-sm-12'fp<'table-group-actions pull-right'>_TOTAL_>r><'table-responsive't><'row'<'col-md-6 col-sm-12'li><'col-md-6 col-sm-12' p>>",
                    buttons: [{
                        extend: 'excel',
                        title: '',
                        className: 'btn btn-primary',
                        filename: 'GIA',
                        extension: '.xlsx',
                        header: true,
                        sheetName: 'GIA Export',
                        text:'<i class="fa fa-download"></i> Download'
                    }],
                    paging: false,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: [
                        { data: "lotno" },
                        { data: "certificate" },
                        { data: "shape" },
                        { data: "weight" },
                        { data: "color" },
                        { data: "clarity" },
                        { data: "length" },
                        { data: "width" },
                        { data: "depth" },
                        { data: "finalCut" },
                        { data: "lab" },
                        { data: "depthPct" },
                        { data: "tablePct" },
                        { data: "girdle" },
                        { data: "polish" },
                        { data: "symmetry" },
                        { data: "fluorescenceIntensity" },
                        { data: "crnHt" },
                        { data: "crnAg" },
                        { data: "pavDp" },
                        { data: "pavAg" },
                        { data: "starLn" },
                        { data: "lrHalf" },
                        { data: "girdlePct" },
                        { data: "reportDt" },
                        { data: "culetSize" },
                        { data: "inscription" },
                        { data: "keyToSymbols" },
                        { data: "reportComments" },
                        { data: "status" }
                    ],
                    columnDefs: [
                        {
                            targets: 'CrownHeight',
                            render: function (data, type, row) {
                                //adviced by sudhir
                                //var val = data.replace('%', '');
                                //return (val / 100).toFixed(2);
                                return data;
                            }
                        },
                        {
                            targets: 'PavilionDepth',
                            render: function (data, type, row) {
                                //var val = data.replace('%', '');
                                //return (val / 100).toFixed(2);
                                return data;
                            }
                        },
                        {
                            targets: ['pAngle', 'cAngle'],
                            render: function (data, type, row) {
                                var val = data.replace('°', '');
                                return val;
                            }
                        },
                        {
                            targets: 'ReportDate',
                            render: function (data, type, row) {
                                var val = moment(data, "MM/DD/YYYY").format("DD-MM-YYYY");
                                return val;
                            }
                        }
                    ],
                }
            });
        } else {
            dtTable.clearSelection();
            var table = dtTable.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            table.draw(false);
        }

    }

    var SetValidation = function () {
        $frmGIAInventory = $('#frmGIAInventory');
        ValGIA = $frmGIAInventory.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                uploadFile: {
                    required: true,
                    extension: 'xlsx'
                }
            }
        });
    };

    ////Created using Sheet JS
    //function doit(fn, dl) {
    //    var table = TableExport($("#myTable").get(0), {
    //        formats: ['xls'],
    //        exportButtons: false,
    //        bootstrap: true
    //    });
    //    var exportData = table.getExportData()['myTable']['xls'];
    //    table.export2file(exportData.data, exportData.mimeType, "GIAExport", exportData.fileExtension, exportData.merges, exportData.RTL, "GIA");
    //    //{
    //    //    filename: 'GIAExport.xlsx',
    //    //        sheetname: 'GIA',
    //    //            formats: ["xlsx"],
    //    //                exportButtons: true
    //    //}
    //    //$("#myTable").tableExport();
    //    //var type = 'xlsx'
    //    //var elt = document.getElementById('myTable');
    //    //var wb = XLSX.utils.table_to_book(elt, { sheet: "GIA" });
    //    //return dl ?
    //    //    XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
    //    //    XLSX.writeFile(wb, fn || ('GIAExport.' + (type || 'xlsx')));
    //}


    return {
        init: function () {
            OnLoad();
            RegisterEvents();
        }
    }
}();
$(document).ready(function () {
    CtrlGIAUpload.init();
});