var CtrlMarketingBlueNile = function () {
    var objSvc = null; $frmBlueNile = null, dtMarketing = null, dtMarketingsummary = null;
    var valBlueNile = null;
    var dtSearchPanel = null;
    var objOSvc = null;

    var OnLoad = function () {
        objSvc = new MarketingService();
        dtMarketing = new Datatable();
        dtMarketingsummary = new Datatable();
        dtSearchPanel = new Datatable();
        objOSvc = new OrderService();

        SetValidation();
        LoadGrid();

        $('#ddlCustlist').select2({
            ajax: {
                delay: 500,
                url: '/Home/GetListOfMarketingCustomer',
                data: function (params) {
                    var query = {
                        search: params.term,
                    }
                    return query;
                },
                processResults: function (data) {
                    return {
                        results: data
                    };
                },
            },
            dropdownParent: $('#Modal-FormMemo'),
            width: 'resolve'
        });
        LoadFilterData();
    };

    var RegisterEvent = function () {
        $(document).on('click', '#btnSave', function (e) {
            e.preventDefault();
            if ($frmBlueNile.valid()) {
                var pData = ReadForm();
                objSvc.CreateBlueNile(pData).then(function (data) {
                    if (data.IsSuccess && data.Result > 0) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored added successfully", type: "success" });
                        ClearForm();
                        LoadGrid();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored not added", type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding recored", type: "danger" });
                });
            }
        });


        $(document).on('click', 'a.btnEditDetails', function (e) {
            e.preventDefault();
            ClearForm();
            var id = $(e.target).data('id');
            objSvc.EditBlueNile(id).then(function (data) {
                if (data != null) {
                    $('#hfBlueNillId').val(data.SrNo);
                    $('#txtCaratRange1Disc').val(data.caratRange1Disc);
                    $('#txtCaratRange2Disc').val(data.caratRange2Disc);
                    $('#txtCaratRange3Disc').val(data.caratRange3Disc);
                    $('#txtCaratRange4Disc').val(data.caratRange4Disc);
                    $('#txtCaratRange5Disc').val(data.caratRange5Disc);
                    $('#txtCaratRange6Disc').val(data.caratRange6Disc);
                    $('#txtCaratRange7Disc').val(data.caratRange7Disc);
                    $('#txtCaratRange8Disc').val(data.caratRange8Disc);
                    $('#txtCaratRange9Disc').val(data.caratRange9Disc);
                    $('#txtCaratRange10Disc').val(data.caratRange10Disc);
                    $('#txtCaratRange11Disc').val(data.caratRange11Disc);
                    $('#txtCaratRange12Disc').val(data.caratRange12Disc);
                    $('#txtCaratRange13Disc').val(data.caratRange13Disc);
                    $('#txtCaratRange14Disc').val(data.caratRange14Disc);
                    $('#txtCaratRange15Disc').val(data.caratRange15Disc);
                    $('#txtCaratRange16Disc').val(data.caratRange16Disc);
                    $('#txtCaratRange17Disc').val(data.caratRange17Disc);
                    $('#txtCaratRange18Disc').val(data.caratRange18Disc);
                    $('#txtCaratRange19Disc').val(data.caratRange19Disc);
                    $('#txtCaratRange20Disc').val(data.caratRange20Disc);
                    $('#txtCaratRange21Disc').val(data.caratRange21Disc);
                    $('#txtCaratRange22Disc').val(data.caratRange22Disc);
                    $('#txtCaratRange23Disc').val(data.caratRange23Disc);
                    $('#txtCaratRange24Disc').val(data.caratRange24Disc);
                    $('#txtCaratRange25Disc').val(data.caratRange25Disc);
                    $('#txtHaExDisc').val(data.haExDisc);
                    $('#txtHaVgDisc').val(data.haVgDisc);
                    $('input[name=BlueNill_Status][value=' + data.Isactive + ']').prop('checked', true);


                    $('#btnUpdate').show();
                    $('#btnSave').hide();
                    $('#BlueNilllStatus').show();


                } else {
                    $('#btnUpdate').hide();
                    $('#btnSave').show();
                    $('#BlueNilllStatus').hide();
                    uiApp.Alert({ container: '#uiPanel1', message: "No record found", type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Try agaiin later !", type: "danger" });
            });

        });



        $(document).on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            if ($frmBlueNile.valid()) {
                var pData = UpdateReadForm();
                objSvc.UpdateBlueNile(pData).then(function (data) {
                    if (data.IsSuccess && data.Result > 0) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored updated successfully", type: "success" });
                        ClearForm();
                        LoadGrid();
                        $('#btnUpdate').hide();
                        $('#btnSave').show();
                        $('#BlueNilllStatus').hide();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Recored not added", type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding recored", type: "danger" });
                });
            }
        });

        $(document).on('click', 'a.btnDelete', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            uiApp.Confirm('Confirm to delete this record ?', function (resp) {
                if (resp) {
                    objSvc.DeleteBlueNile(id).then(function (data) {
                        if (data != null) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Recored deleted successfully", type: "success" });
                            LoadGrid();
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Recored not added", type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Try again later !", type: "danger" });
                    });
                }
            });

        });


        $(document).on('click', '#btnFilter', function (e) {
            e.preventDefault();
           
            LoadFilterData();
           

        });

        $(document).on('click', 'a.btnSearch', function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            var LotNos = $(e.target).attr('data-id');

            //LoadOrdersDetails(LotNos);
            objSvc.LoadOrdersSummaryDetails(LotNos).then(function (data) {
                if (data != null) {
                    LoadOrdersDetails(data);
                    var table = $('#SearchTablePost1').DataTable();
                    table.column(0).visible(false);  
                    uiApp.UnBlockUI();
                    $('body,html').animate({
                        scrollTop: $(document).height()             
                    }, 500);

                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "Details not found", type: "danger" });
                    uiApp.UnBlockUI();
                }
            }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Try again later !", type: "danger" });
                    uiApp.UnBlockUI();
            });
        });



        $(document).on('click', 'a.btnExportMemo', function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            var LotNos = $(e.target).attr('data-id'); 
            objOSvc.GetOrderSummaryItemForExcel(LotNos).then(function (data) {
                if (data.IsSuccess) {
                    $('#ancInventoryDownload').get(0).click();
                    uiApp.UnBlockUI();
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (error) {
                uiApp.UnBlockUI();
            });

        });

        $('#SMonth0').click(function (e) {
            var isSelected = $(this).prop("checked");
            if (isSelected == true) {
                $('input[type=checkbox].SMonth').prop('checked', true); 
            } else {
                $('input[type=checkbox].SMonth').prop('checked', false); 
            } 
        });
        $('.SMonth').click(function (e) { 
            var NameID = "input[type=checkbox][name=SMonth]";
            var countchk = $(NameID).length; 
            var checkedcount = 0;
            for (var ele of $(NameID)) { 
                if ($(ele).prop('checked')) {
                    checkedcount = checkedcount + 1;
                }
            }
            if (countchk == checkedcount) {
                $('input[type=checkbox]#SMonth0').prop('checked', true); 
            } else {
                $('input[type=checkbox]#SMonth0').prop('checked', false); 
            }
        });





        $(function () {
            $('#myMonths').multiselect({
                includeSelectAllOption: true,
                selectAllValue: 'multiselect-all',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: '300',
                buttonWidth: '235',
               
            });
        });



        $(function () {
            $('#mySLocation').multiselect({
                includeSelectAllOption: true,
                selectAllValue: 'multiselect-all',
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: '300',
                buttonWidth: '235',
            });
        });

      


    };
    function LoadOrdersDetails(data) {

        var colStruct = new DataTableColumnStruct('SpecificSearch');
        dtSearchPanel = new Datatable();
        if (dtSearchPanel.getDataTable() == null || dtSearchPanel.getDataTable() == undefined) {
            dtSearchPanel.init({
                src: '#SearchTablePost1',
                dataTable: {
                    scrollY: "485px",
                    scrollX: true,
                    scrollCollapse: true,
                    paging: true,
                    destroy: true,
                    serverSide: false,
                    pageLength: 200,
                    data: data,
                    createdRow: function (row, data, dataIndex) {

                    },
                    lengthMenu: [[50, 100, 200, 500000], [50, 100, 200, "All"]],
                    order: [[1, "desc"]],
                    fixedColumns: {
                        leftColumns: 2
                    },
                    //ajax: {
                    //    type: 'Post',
                    //    url: '/Marketing/StockList2?LotNos=' + LotNos,
                    //    beforeSend: function (request) {
                    //        var TokenID = myApp.token().get();
                    //        request.setRequestHeader("TokenID", TokenID);
                    //        uiApp.BlockUI();
                    //        return request;
                    //    },
                    //    dataFilter: function (data) {
                    //        uiApp.UnBlockUI();
                    //        return data;
                    //    }
                    //    , dataSrc: function (json) {
                    //        console.log(json);
                    //        return json.data;
                    //    }
                    //},
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colStruct.SpecificSearch.columnDefs,
                    rowCallback: function (row, data, index) {


                    }
                },
                onCheckboxChange: function (obj, objInv) {

                }
            });
            // console.log(dtSearchPanel.getDataTable());
        } else {
            dtSearchPanel.clearSelection();
            dtSearchPanel.getDataTable().draw();
        }


    }

    var LoadFilterData = function () {
        uiApp.BlockUI();
        var ddlSYear = $('#ddlSYear').val();
        var SaleValues = $('#ddlCustlist').val() == null ? 0 : $('#ddlCustlist').val();
        var MonthValues = ReadFormFilter('SMonth');
        var LocationValues = ReadFormFilter('SLocation');

        objSvc.GetStockSummaryDetails(SaleValues, ddlSYear, MonthValues, LocationValues).then(function (data) {
            if (data != null) {
                LoadStockSummary(data);
                uiApp.UnBlockUI();
            } else {
                uiApp.Alert({ container: '#uiPanel1', message: "Details not found", type: "danger" });
                uiApp.UnBlockUI();
            }
        }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Try again later !", type: "danger" });
                uiApp.UnBlockUI();
        });
    }

    var ReadFormFilter = function (FilterName) {
        //var innerStr = "";
        //var labelCheckBox = "";
        //NameID = "input[type=checkbox][name=" + FilterName + "]";
        //var countchk = $(NameID).length;
        //for (var ele of $(NameID)) {
        //    var forLabel = ele.id;
        //    if ($(ele).prop('checked')) {
        //        innerStr = (innerStr == "" ? $(ele).val() : innerStr + "," + $(ele).val());
        //        labelCheckBox = (labelCheckBox == "" ? $('label[for=' + forLabel + ']').html() : labelCheckBox + "," + $('label[for=' + forLabel + ']').html());
        //    }
        //}
        //return innerStr;

         
        var selected = $("#" + FilterName+" option:selected");
        var labelCheckBox = "";
            var arrSelected = [];
            selected.each(function () {
                arrSelected.push($(this).val());
                labelCheckBox += $(this).val() == '' ? $(this).val() : $(this).val()+',';
            });
        
        return labelCheckBox;
    }
    var ClearForm = function () {
        $('#frmBlueNile').find("input[type=text], textarea").val("");
    }

    var ReadForm = function () {
        return {
            caratRange1Disc: $('#txtCaratRange1Disc').val(),
            caratRange2Disc: $('#txtCaratRange2Disc').val(),
            caratRange3Disc: $('#txtCaratRange3Disc').val(),
            caratRange4Disc: $('#txtCaratRange4Disc').val(),
            caratRange5Disc: $('#txtCaratRange5Disc').val(),
            caratRange6Disc: $('#txtCaratRange6Disc').val(),
            caratRange7Disc: $('#txtCaratRange7Disc').val(),
            caratRange8Disc: $('#txtCaratRange8Disc').val(),
            caratRange9Disc: $('#txtCaratRange9Disc').val(),
            caratRange10Disc: $('#txtCaratRange10Disc').val(),
            caratRange11Disc: $('#txtCaratRange11Disc').val(),
            caratRange12Disc: $('#txtCaratRange12Disc').val(),
            caratRange13Disc: $('#txtCaratRange13Disc').val(),
            caratRange14Disc: $('#txtCaratRange14Disc').val(),
            caratRange15Disc: $('#txtCaratRange15Disc').val(),
            caratRange16Disc: $('#txtCaratRange16Disc').val(),
            caratRange17Disc: $('#txtCaratRange17Disc').val(),
            caratRange18Disc: $('#txtCaratRange18Disc').val(),
            caratRange19Disc: $('#txtCaratRange19Disc').val(),
            caratRange20Disc: $('#txtCaratRange20Disc').val(),
            caratRange21Disc: $('#txtCaratRange21Disc').val(),
            caratRange22Disc: $('#txtCaratRange22Disc').val(),
            caratRange23Disc: $('#txtCaratRange23Disc').val(),
            caratRange24Disc: $('#txtCaratRange24Disc').val(),
            caratRange25Disc: $('#txtCaratRange25Disc').val(),
            haExDisc: $('#txtHaExDisc').val(),
            haVgDisc: $('#txtHaVgDisc').val()
        }
    };

    var UpdateReadForm = function () {
        return {
            caratRange1Disc: $('#txtCaratRange1Disc').val(),
            caratRange2Disc: $('#txtCaratRange2Disc').val(),
            caratRange3Disc: $('#txtCaratRange3Disc').val(),
            caratRange4Disc: $('#txtCaratRange4Disc').val(),
            caratRange5Disc: $('#txtCaratRange5Disc').val(),
            caratRange6Disc: $('#txtCaratRange6Disc').val(),
            caratRange7Disc: $('#txtCaratRange7Disc').val(),
            caratRange8Disc: $('#txtCaratRange8Disc').val(),
            caratRange9Disc: $('#txtCaratRange9Disc').val(),
            caratRange10Disc: $('#txtCaratRange10Disc').val(),
            caratRange11Disc: $('#txtCaratRange11Disc').val(),
            caratRange12Disc: $('#txtCaratRange12Disc').val(),
            caratRange13Disc: $('#txtCaratRange13Disc').val(),
            caratRange14Disc: $('#txtCaratRange14Disc').val(),
            caratRange15Disc: $('#txtCaratRange15Disc').val(),
            caratRange16Disc: $('#txtCaratRange16Disc').val(),
            caratRange17Disc: $('#txtCaratRange17Disc').val(),
            caratRange18Disc: $('#txtCaratRange18Disc').val(),
            caratRange19Disc: $('#txtCaratRange19Disc').val(),
            caratRange20Disc: $('#txtCaratRange20Disc').val(),
            caratRange21Disc: $('#txtCaratRange21Disc').val(),
            caratRange22Disc: $('#txtCaratRange22Disc').val(),
            caratRange23Disc: $('#txtCaratRange23Disc').val(),
            caratRange24Disc: $('#txtCaratRange24Disc').val(),
            caratRange25Disc: $('#txtCaratRange25Disc').val(),
            haExDisc: $('#txtHaExDisc').val(),
            haVgDisc: $('#txtHaVgDisc').val(),
            SrNo: $('#hfBlueNillId').val(),
            Isactive: $('input:radio[name=BlueNill_Status]:checked').val()

        }
    };
    var LoadGrid = function () {

        if (dtMarketing.getDataTable() == null || dtMarketing.getDataTable() == undefined) {
            dtMarketing.init({
                src: '#tblMarketing',
                dataTable: {
                    //deferLoading: 0,
                    scrollX: "300px",
                    scrollY: true,
                    paging: true,
                    order: [[0, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Marketing/GridBlueNile',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "SrNo", class: 'whspace' },
                        { data: "caratRange1Disc", class: 'whspace' },
                        { data: "caratRange2Disc", class: 'whspace' },
                        { data: "caratRange3Disc", class: 'whspace' },
                        { data: "caratRange4Disc", class: 'whspace' },
                        { data: "caratRange5Disc", class: 'whspace' },
                        { data: "caratRange6Disc", class: 'whspace' },
                        { data: "caratRange7Disc", class: 'whspace' },
                        { data: "caratRange8Disc", class: 'whspace' },
                        { data: "caratRange9Disc", class: 'whspace' },
                        { data: "caratRange10Disc", class: 'whspace' },
                        { data: "caratRange11Disc", class: 'whspace' },
                        { data: "caratRange12Disc", class: 'whspace' },
                        { data: "caratRange13Disc", class: 'whspace' },
                        { data: "caratRange14Disc", class: 'whspace' },
                        { data: "caratRange15Disc", class: 'whspace' },
                        { data: "caratRange16Disc", class: 'whspace' },
                        { data: "caratRange17Disc", class: 'whspace' },
                        { data: "caratRange18Disc", class: 'whspace' },
                        { data: "caratRange19Disc", class: 'whspace' },
                        { data: "caratRange20Disc", class: 'whspace' },
                        { data: "caratRange21Disc", class: 'whspace' },
                        { data: "caratRange22Disc", class: 'whspace' },
                        { data: "caratRange23Disc", class: 'whspace' },
                        { data: "caratRange24Disc", class: 'whspace' },
                        { data: "caratRange25Disc", class: 'whspace' },
                        { data: "haExDisc", class: 'whspace' },
                        { data: "haVgDisc", class: 'whspace' },
                        { data: "Isactive", class: 'whspace' },
                        { data: "createdOn", class: 'whspace' },
                        { data: "UpdatedOn", class: 'whspace' },
                        { data: null, class: 'whspace' },
                        { data: null, class: 'whspace' }
                    ],
                    columnDefs: [
                        {
                            targets: [28],
                            render: function (data, type, row) {

                                return row.Isactive == 1 ? "Active" : "In Active";

                            },
                            orderable: true
                        },
                        {
                            targets: [29],
                            render: function (data, type, row) {
                                return moment(row.createdOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        },
                        {
                            targets: [30],
                            render: function (data, type, row) {
                                return row.UpdatedOn == null ? "" : moment(row.UpdatedOn).format(myApp.dateFormat.Client);
                            },
                            orderable: true
                        },
                        {
                            targets: [31],
                            render: function (data, type, row) {
                                return '<a class="btnEditDetails" data-id="' + row.SrNo + '"    href="#">Edit</a>';
                            },
                            orderable: true
                        },
                        {
                            targets: [32],
                            render: function (data, type, row) {
                                return '<a class="btnDelete" data-id="' + row.SrNo + '"    href="#">Delete</a>';
                            },
                            orderable: true
                        }

                    ]
                }
            });
        } else {
            dtMarketing.getDataTable().draw();
        }

    };

    var SetValidation = function () {
        $frmBlueNile = $('#frmBlueNile');
        var rPattern = /^100(\.0{0,2}?)?$|^\d{0,2}(\.\d{0,2})?$/;
        valBlueNile = $frmBlueNile.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                caratRange1Disc: {
                    required: true,
                    number: true
                },
                caratRange2Disc: {
                    required: true,
                    number: true
                },
                caratRange3Disc: {
                    required: true,
                    number: true
                },
                caratRange4Disc: {
                    required: true,
                    number: true
                },
                caratRange5Disc: {
                    required: true,
                    number: true
                },
                caratRange6Disc: {
                    required: true,
                    number: true
                },
                caratRange7Disc: {
                    required: true,
                    number: true
                },
                caratRange8Disc: {
                    required: true,
                    number: true
                },
                caratRange9Disc: {
                    required: true,
                    number: true
                },
                caratRange10Disc: {
                    required: true,
                    number: true
                },
                caratRange11Disc: {
                    required: true,
                    number: true,
                },
                caratRange12Disc: {
                    required: true,
                    number: true
                },
                caratRange13Disc: {
                    required: true,
                    number: true
                },
                caratRange14Disc: {
                    required: true,
                    number: true
                },
                caratRange15Disc: {
                    required: true,
                    number: true
                },
                caratRange16Disc: {
                    required: true,
                    number: true
                },
                caratRange17Disc: {
                    required: true,
                    number: true
                },
                caratRange18Disc: {
                    required: true,
                    number: true
                },
                caratRange19Disc: {
                    required: true,
                    number: true
                },
                caratRange20Disc: {
                    required: true,
                    number: true
                },
                caratRange21Disc: {
                    required: true,
                    number: true
                },
                caratRange22Disc: {
                    required: true,
                    number: true
                },
                caratRange23Disc: {
                    required: true,
                    number: true
                },
                caratRange24Disc: {
                    required: true,
                    number: true
                },
                caratRange25Disc: {
                    required: true,
                    number: true
                },
                haExDisc: {
                    required: true,
                    number: true
                },
                haVgDisc: {
                    required: true,
                    number: true
                }
            }
        });
    };





    var LoadStockSummary = function (data) {
        dtMarketingsummary = new Datatable();
        if (dtMarketingsummary.getDataTable() == null || dtMarketingsummary.getDataTable() == undefined) {
            dtMarketingsummary.init({
                src: '#tblMarketingSummary',
                dataTable: {
                    
                    scrollY: "305px",
                    scrollX: true,
                    scrollCollapse: true,
                    paging: true,
                    destroy: true,
                    serverSide: false,
                    pageLength: 500, 
                    data: data,
                    columns: [
                        { data: "companyName", class: 'whspace' },
                        { data: "sellMonth", class: 'whspace' },
                        { data: "NoOfStone", class: 'whspace' },
                        { data: "TotalWeight", class: 'whspace' },
                        { data: "TotalValue", class: 'whspace' },
                        { data: "LotNoList", class: 'whspace' },
                        { data: "LotNoList", class: 'whspace' }

                    ],
                    columnDefs: [
                        {
                            targets: [5],
                            render: function (data, type, row) {
                                if (row.NoOfStone > 0) {
                                    return '<a class="btnSearch" data-id="' + row.LotNoList + '"    href="#">Search</a>';
                                } else { return '';}
                            },
                            orderable: true
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {
                                if (row.NoOfStone > 0) {
                                return '<a class="btnExportMemo" data-id="' + row.LotNoList + '"    href="#">Download</a>';
                                } else { return ''; }
                            },
                            orderable: true
                        }
                    ],
                    footerCallback: function (row, data, start, end, display) {
                        var TotNoofstone = 0, TotWeight = 0, TotValue = 0, TotLotnos = '';
                        for (var i = 0; i < data.length; i++) {
                            TotNoofstone += data[i].NoOfStone;
                            TotWeight += data[i].TotalWeight;
                            TotValue += data[i].TotalValue;
                            TotLotnos += data[i].LotNoList + ',';

                        }

                        $(row).find('th:eq(2)').html(TotNoofstone);
                        $(row).find('th:eq(3)').html(TotWeight.toFixed(2));
                        $(row).find('th:eq(4)').html(TotValue.toFixed(2));
                        if (TotNoofstone > 0) {
                        $(row).find('th:eq(5)').html('<a class="btnSearch" data-id="' + TotLotnos+ '"    href="#">Search</a>');
                        } else { $(row).find('th:eq(5)').html(''); }
                        if (TotNoofstone > 0) {

                        $(row).find('th:eq(6)').html('<a class="btnExportMemo" data-id="' + TotLotnos + '"    href="#">Download</a>');
                        } else { $(row).find('th:eq(6)').html(''); }


                    }
                }
            });
        } else {
            dtMarketingsummary.getDataTable().draw();
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
    CtrlMarketingBlueNile.init();
});
