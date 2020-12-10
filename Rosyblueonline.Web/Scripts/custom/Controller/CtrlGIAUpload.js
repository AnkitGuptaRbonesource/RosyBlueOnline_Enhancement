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
                        GetDataFromApi(data.Result);

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


    var GetDataFromApi = function (pData) {
        var ListOfData = [];
        var data = [];
        var idx2 = 0;
        var idx = 0;
        for (var i = 0; i < pData.length; i++) {
            data.push({
                reportNo: pData[i].Certificate,
                //reportWeight: pData[i].Weight,
            });

            objSF.NewGetDataFromGiaApi(pData[i].Certificate).then(function (rData) {
                 
                var FinalJson = JSON.parse(rData.Result);

                //for (var k = 0; k < pData.length; k++) {
                //    if (pData[k].Certificate == FinalJson.data.getReport.report_number) {
                //        idx2 = k;
                //        break;
                //    }
                //}

                
                    if (rData.IsSuccess == true && FinalJson.data.getReport != null) {
                        ListOfData.push(FinalJson.data.getReport.results); 
                        //ListOfData[ListOfData.length - 1]["lotno"] = pData[idx].Lotnumber;
                        //ListOfData[ListOfData.length - 1]["certificate"] = pData[idx].Certificate;
                         

                        ListOfData[ListOfData.length - 1]["lotno"] = "";
                        ListOfData[ListOfData.length - 1]["certificate"] = FinalJson.data.getReport.report_number;
                        ListOfData[ListOfData.length - 1]["shape"] = FinalJson.data.getReport.results.data.shape.shape_group;
                        ListOfData[ListOfData.length - 1]["weight"] = FinalJson.data.getReport.results.data.weight.weight;
                        ListOfData[ListOfData.length - 1]["color"] = FinalJson.data.getReport.results.data.color.color_grade_code;
                        ListOfData[ListOfData.length - 1]["clarity"] = FinalJson.data.getReport.results.clarity_grade;
                        var width = FinalJson.data.getReport.results.measurements.split(" - ");
                        var length = width[1].split(" x ");
                        var depth = length[1].split(" mm");
                        ListOfData[ListOfData.length - 1]["length"] = length[0];
                        ListOfData[ListOfData.length - 1]["width"] = width[0];
                        ListOfData[ListOfData.length - 1]["depth"] = depth[0];
                        ListOfData[ListOfData.length - 1]["finalCut"] = FinalJson.data.getReport.results.data.cut;
                        ListOfData[ListOfData.length - 1]["lab"] = "GIA";
                        ListOfData[ListOfData.length - 1]["depthPct"] = FinalJson.data.getReport.results.proportions.depth_pct;
                        ListOfData[ListOfData.length - 1]["tablePct"] = FinalJson.data.getReport.results.proportions.table_pct;
                        ListOfData[ListOfData.length - 1]["girdle"] = FinalJson.data.getReport.results.data.girdle.girdle_size_code;
                        ListOfData[ListOfData.length - 1]["polish"] = FinalJson.data.getReport.results.data.polish;
                        ListOfData[ListOfData.length - 1]["symmetry"] = FinalJson.data.getReport.results.data.symmetry;
                        ListOfData[ListOfData.length - 1]["fluorescenceIntensity"] = FinalJson.data.getReport.results.fluorescence;
                        ListOfData[ListOfData.length - 1]["crnHt"] = FinalJson.data.getReport.results.proportions.crown_height + "%";
                        ListOfData[ListOfData.length - 1]["crnAg"] = FinalJson.data.getReport.results.proportions.crown_angle;
                        ListOfData[ListOfData.length - 1]["pavDp"] = FinalJson.data.getReport.results.proportions.pavilion_depth + "%";
                        ListOfData[ListOfData.length - 1]["pavAg"] = FinalJson.data.getReport.results.proportions.pavilion_angle + "%";
                        ListOfData[ListOfData.length - 1]["starLn"] = FinalJson.data.getReport.results.proportions.star_length + "%";
                        ListOfData[ListOfData.length - 1]["lrHalf"] = FinalJson.data.getReport.results.proportions.lower_half + "%";
                        ListOfData[ListOfData.length - 1]["girdlePct"] = FinalJson.data.getReport.results.data.girdle.girdle_pct + "%";
                        ListOfData[ListOfData.length - 1]["reportDt"] = FinalJson.data.getReport.report_date_iso;
                        ListOfData[ListOfData.length - 1]["culetSize"] = FinalJson.data.getReport.results.proportions.culet;
                        ListOfData[ListOfData.length - 1]["inscription"] = FinalJson.data.getReport.results.inscriptions;
                        ListOfData[ListOfData.length - 1]["keyToSymbols"] = FinalJson.data.getReport.results.clarity_characteristics;
                        ListOfData[ListOfData.length - 1]["reportComments"] = FinalJson.data.getReport.results.report_comments;
                        ListOfData[ListOfData.length - 1]["status"] = "Success";
                         
                       idx++;
                    } else {
                        ListOfData.push(FinalJson.errors[0]);
                         //ListOfData[ListOfData.length - 1]["lotno"] = pData[idx].Lotnumber;
                        //ListOfData[ListOfData.length - 1]["certificate"] = pData[idx].Certificate;
                        ListOfData[ListOfData.length - 1]["lotno"] = "";
                        ListOfData[ListOfData.length - 1]["certificate"] = "";
                        ListOfData[ListOfData.length - 1]["shape"] = "";
                        ListOfData[ListOfData.length - 1]["weight"] = "";
                        ListOfData[ListOfData.length - 1]["color"] = "";
                        ListOfData[ListOfData.length - 1]["clarity"] = ""; 
                        ListOfData[ListOfData.length - 1]["length"] = "";
                        ListOfData[ListOfData.length - 1]["width"] = "";
                        ListOfData[ListOfData.length - 1]["depth"] = "";
                        ListOfData[ListOfData.length - 1]["finalCut"] = "";
                        ListOfData[ListOfData.length - 1]["lab"] = "";
                        ListOfData[ListOfData.length - 1]["depthPct"] = "";
                        ListOfData[ListOfData.length - 1]["tablePct"] = "";
                        ListOfData[ListOfData.length - 1]["girdle"] = "";
                        ListOfData[ListOfData.length - 1]["polish"] = "";
                        ListOfData[ListOfData.length - 1]["symmetry"] = "";
                        ListOfData[ListOfData.length - 1]["fluorescenceIntensity"] = "";
                        ListOfData[ListOfData.length - 1]["crnHt"] = "";
                        ListOfData[ListOfData.length - 1]["crnAg"] = "";
                        ListOfData[ListOfData.length - 1]["pavDp"] = "";
                        ListOfData[ListOfData.length - 1]["pavAg"] = "";
                        ListOfData[ListOfData.length - 1]["starLn"] = "";
                        ListOfData[ListOfData.length - 1]["lrHalf"] = "";
                        ListOfData[ListOfData.length - 1]["girdlePct"] = "";
                        ListOfData[ListOfData.length - 1]["reportDt"] = "";
                        ListOfData[ListOfData.length - 1]["culetSize"] = "";
                        ListOfData[ListOfData.length - 1]["inscription"] = "";
                        ListOfData[ListOfData.length - 1]["keyToSymbols"] = "";
                        ListOfData[ListOfData.length - 1]["reportComments"] = "";
                        ListOfData[ListOfData.length - 1]["status"] = FinalJson.errors[0].errorType;


                        idx++;
                    } 
                console.log(ListOfData);
                console.log(idx);
                if (pData.length == idx) {
                    console.log("Final");
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
                        text: '<i class="fa fa-download"></i> Download'
                    }],
                    paging: false,
                    order: [[2, "desc"]],
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
                                if (data == "" || data == null) {
                                    return "";
                                } else {
                                    var val = moment(data).format("DD-MM-YYYY");
                                    return val;
                                }
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