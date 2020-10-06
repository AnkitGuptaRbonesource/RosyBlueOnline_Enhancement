/***
Wrapper/Helper Class for datagrid based on jQuery Datatable Plugin
***/
var Datatable = function () {

    var tableOptions; // main options
    var dataTable; // datatable object
    var table; // actual table jquery object
    var tableContainer; // actual table container object
    var tableWrapper; // actual table wrapper jquery object
    var tableInitialized = false;
    var ajaxParams = {}; // set filter mode
    var the;
    var CheckedRows = [];
    var CheckedRowsInventoryID = [];

    var countSelectedRecords = function () {
        //var selected = $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).length;
        var selected = CheckedRows.length;
        var text = tableOptions.dataTable.language.metronicGroupActions;
        if (selected > 0) {
            $('.table-group-actions', tableWrapper).text(text.replace("_TOTAL_", selected));
        } else {
            $('.table-group-actions', tableWrapper).text("");
        }
        tableOptions.onCheckboxChange(CheckedRows, CheckedRowsInventoryID);

    };

    var AddSelectedRecords = function (data, inventoryID) {
        var idx = CheckedRows.length;
        CheckedRows.splice(idx, 0, data);
        CheckedRowsInventoryID.splice(idx, 0, inventoryID);
    }

    var RemoveSelectedRecords = function (data) {
        var idx = 0;
        for (var i in CheckedRows) {
            if (CheckedRows[i] == data) {
                idx = i;
                break;
            }
        }
        CheckedRows.splice(idx, 1);
        CheckedRowsInventoryID.splice(idx, 1);
    }

    return {

        //main function to initiate the module
        init: function (options) {

            if (!$().dataTable) {
                return;
            }

            the = this;

            // default settings
            options = $.extend(true, {
                searching: false,
                src: "", // actual table  
                filterApplyAction: "filter",
                filterCancelAction: "filter_cancel",
                resetGroupActionInputOnSuccess: true,
                loadingMessage: 'Loading...',
                dataTable: {
                    //dom: "<'top'lp><'clear'>",
                    dom: "<'row'<'top'fpl><'clear'>>" +
                        "<'row'<'col-sm-12'tr>>",
                    //"dom": options.searching == true ?
                    //    "<'row'<'col-md-6 col-sm-12'li><'col-md-6 col-sm-12'fp<'table-group-actions pull-right'>_TOTAL_>r><'table-responsive't><'row'<'col-md-6 col-sm-12'li><'col-md-6 col-sm-12'>>" :
                    //    "<'row'<'col-md-6 col-sm-12'li><'col-md-6 col-sm-12'p<'table-group-actions pull-right'>_TOTAL_>r><'table-responsive't><'row'<'col-md-6 col-sm-12'li><'col-md-6 col-sm-12' p>>", // datatable layout
                    "pageLength": 50, // default records per page
                    "language": { // language settings
                        // metronic spesific
                        "metronicGroupActions": "_TOTAL_ records selected:  ",
                        "metronicAjaxRequestGeneralError": "Could not complete request. Please check your internet connection",
                        // data tables spesific
                        //"lengthMenu": "<span class='seperator'>|</span>View _MENU_ records",
                        "lengthMenu": "Show up to Record : _MENU_",
                        //"info": "<span class='seperator'>|</span>Found total _TOTAL_ records",
                        "info": "Found total _TOTAL_ records",
                        "infoEmpty": "No records found to show",
                        "emptyTable": "No data available in table",
                        "zeroRecords": "No matching records found",
                        "paginate": {
                            "previous": "Prev",
                            "next": "Next",
                            "last": "Last",
                            "first": "First",
                            "page": "Page",
                            "pageOf": "of"
                        },
                        "search": ""
                    },
                  "fixedColumns": { "leftColumns": 2},
                    "orderCellsTop": true,
                    "columnDefs": [{ // define columns sorting options(by default all columns are sortable extept the first checkbox column)
                        'orderable': false,
                        'targets': [0]
                    }],

                    "pagingType": "simple_numbers", // pagination type(bootstrap, bootstrap_full_number or bootstrap_extended)
                    "autoWidth": false, // disable fixed width and enable fluid table
                    "processing": false, // enable/disable display message box on record load
                    "serverSide": true, // enable/disable server side ajax loading

                    "ajax": { // define ajax settings
                        "url": "", // ajax URL
                        "type": "POST", // request type
                        "timeout": 20000,
                        "data": function (data) { // add request parameters before submit
                            $.each(ajaxParams, function (key, value) {
                                data[key] = value;
                            });
                        },
                        "dataSrc": function (res) { // Manipulate the data returned from the server
                            if (res.customActionMessage) {
                            }
                            if (tableOptions.onSuccess) {
                                tableOptions.onSuccess.call(undefined, the, res);
                            }

                            return res.data;
                        },
                        "error": function () { // handle general connection errors
                            if (tableOptions.onError) {
                                tableOptions.onError.call(undefined, the);
                            }
                        }
                    },

                    "drawCallback": function (oSettings) { // run some code on table redraw
                        if (tableInitialized === false) { // check if table has been initialized
                            tableInitialized = true; // set table initialized
                            table.show(); // display table
                        }
                        //countSelectedRecords(); // reset selected records indicator

                        // callback for ajax data load
                        if (tableOptions.onDataLoad) {
                            tableOptions.onDataLoad.call(undefined, the);
                        }


                    }
                },

            }, options);

            tableOptions = options;


            
            //var $label = document.getElementsByTagName("INPUT")[0].closest("label");
            //$label.replaceWith(document.getElementsByTagName("INPUT")[0]);

            // create table's jquery object
            table = $(options.src);
            tableContainer = table.parents(".table-container");

            // apply the special class that used to restyle the default datatable
            var tmp = $.fn.dataTableExt.oStdClasses;

            //$.fn.dataTableExt.oStdClasses.sWrapper = $.fn.dataTableExt.oStdClasses.sWrapper + " dataTables_extended_wrapper";
            //$.fn.dataTableExt.oStdClasses.sFilterInput = "form-control input-xs input-sm input-inline";
            //$.fn.dataTableExt.oStdClasses.sLengthSelect = "form-control input-xs input-sm input-inline";

            // initialize a datatable
            dataTable = table.DataTable(options.dataTable);

            // revert back to default
            //$.fn.dataTableExt.oStdClasses.sWrapper = tmp.sWrapper;
            //$.fn.dataTableExt.oStdClasses.sFilterInput = tmp.sFilterInput;
            //$.fn.dataTableExt.oStdClasses.sLengthSelect = tmp.sLengthSelect;

            // get table wrapper
            tableWrapper = table.parents('.dataTables_wrapper');

            // build table group actions panel
            //if ($('.table-actions-wrapper', tableContainer).size() === 1) {
            //    $('.table-group-actions', tableWrapper).html($('.table-actions-wrapper', tableContainer).html()); // place the panel inside the wrapper
            //    $('.table-actions-wrapper', tableContainer).remove(); // remove the template container
            //}
            // handle group checkboxes check/uncheck
            tableWrapper.on('change', 'input[type=checkbox].group-checkable', function (e) {
                var set = table.find('tbody > tr > td:nth-child(1) input[type="checkbox"]:enabled');
                var checked = $(this).prop("checked");
                for (var i = 0; i < set.length; i++) {
                    if (checked != $(set[i]).prop("checked")) {
                        $(set[i]).prop("checked", checked);
                        var rowIdx = $(set[i]).parent().parent().parent().index();
                        var rowData = dataTable.rows(rowIdx).data()[0];
                        if (checked == true) {
                            AddSelectedRecords($(set[i]).val(), rowData);
                        } else {
                            //CheckedRows.pop();
                            RemoveSelectedRecords($(set[i]).val());
                        }
                    }
                }
                countSelectedRecords();
            });

            //$('.group-checkable').change(function () {
            //    //CheckedRows = [];
            //});

            //// handle row's checkbox click
            //table.on('change', '#checkAll', function (e) {
            //    var set = table.find('tbody > tr > td:nth-child(1) input[type="checkbox"]');
            //    if ($(this).prop('checked') == true) {
            //        for (var i = 0; i < set.length; i++) {
            //            CheckedRows.push($(set).val());
            //        }
            //    } else {
            //        for (var i = 0; i < set.length; i++) {
            //            CheckedRows.pop($(set).val());
            //        }
            //    }
            //    countSelectedRecords();
            //});

            //// handle row's checkbox click
            table.on('change', 'tbody > tr > td:nth-child(1) input[type="checkbox"]', function (e) {
                // checkbox -> label -> td -> tr
                var rowIdx = $(this).parent().parent().parent().index();
                var rowData = dataTable.rows(rowIdx).data()[0];
                if ($(this).prop('checked') == true) {
                    AddSelectedRecords($(this).val(), rowData.inventoryID);
                } else {
                    //CheckedRows.pop($(this).val());
                    RemoveSelectedRecords($(this).val(), rowData.inventoryID);
                }
                countSelectedRecords();
            });

            //// handle xhr on complete
            table.on('draw.dt', function (data) {
                if (CheckedRows.length > 0) {
                    $('tbody > tr > td:nth-child(1) input[type="checkbox"]', table).each(function (idx, ele) {
                        for (var i = 0; i < CheckedRows.length; i++) {
                            if ($(ele).val() == CheckedRows[i]) {
                                $(ele).prop('checked', true)
                            }
                        }
                    });
                }
                if (tableOptions["onLoadDataCompleted"] != null) {
                    tableOptions.onLoadDataCompleted();
                }

                var chkLen = $('tbody > tr > td:nth-child(1) input[type="checkbox"]', table).length;
                var chkedLen = $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).length;
                if (chkLen == chkedLen) {
                    $('.group-checkable', table).prop('checked', true);
                } else {
                    $('.group-checkable', table).prop('checked', false);
                }
            });

            table.on('page.dt', function () {
                $('.group-checkable', table).prop('checked', false);
            });

            //// handle filter submit button click
            //table.on('click', '.filter-submit', function (e) {
            //    e.preventDefault();
            //    the.submitFilter();
            //});

            //// handle filter cancel button click
            //table.on('click', '.filter-cancel', function (e) {
            //    e.preventDefault();
            //    the.resetFilter();
            //});

        },

        submitFilter: function () {
            the.setAjaxParam("action", tableOptions.filterApplyAction);

            // get all typeable inputs
            $('textarea.form-filter, select.form-filter, input.form-filter:not([type="radio"],[type="checkbox"])', table).each(function () {
                the.setAjaxParam($(this).attr("name"), $(this).val());
            });

            // get all checkboxes
            $('input.form-filter[type="checkbox"]:checked', table).each(function () {
                the.addAjaxParam($(this).attr("name"), $(this).val());
            });

            // get all radio buttons
            $('input.form-filter[type="radio"]:checked', table).each(function () {
                the.setAjaxParam($(this).attr("name"), $(this).val());
            });

            dataTable.ajax.reload();
        },

        resetFilter: function () {
            $('textarea.form-filter, select.form-filter, input.form-filter', table).each(function () {
                $(this).val("");
            });
            $('input.form-filter[type="checkbox"]', table).each(function () {
                $(this).attr("checked", false);
            });
            the.clearAjaxParams();
            the.addAjaxParam("action", tableOptions.filterCancelAction);
            dataTable.ajax.reload();
        },

        getSelectedRowsCount: function () {
            return CheckedRows.length;
        },

        getSelectedRows: function () {
            return CheckedRows;
        },

        clearSelection: function () {
            CheckedRows = [];
            $('.group-checkable', table).prop('checked', false);
            countSelectedRecords();
        },

        setAjaxParam: function (name, value) {
            ajaxParams[name] = value;
        },

        addAjaxParam: function (name, value) {
            if (!ajaxParams[name]) {
                ajaxParams[name] = [];
            }

            skip = false;
            for (var i = 0; i < (ajaxParams[name]).length; i++) { // check for duplicates
                if (ajaxParams[name][i] === value) {
                    skip = true;
                }
            }

            if (skip === false) {
                ajaxParams[name].push(value);
            }
        },

        clearAjaxParams: function () {
            ajaxParams = {};
        },

        getDataTable: function () {
            return dataTable;
        },

        getTableWrapper: function () {
            return tableWrapper;
        },

        getTableContainer: function () {
            return tableContainer;
        },

        getTable: function () {
            return table;
        },

        disableCheckbox: function (disabled) {
            $('input[type="checkbox"]', table).each(function (idx, ele) {
                $(ele).prop("disabled", disabled ? '' : 'disabled');
            });
        },

        getData: function () {
            return dataTable.rows().data();
        }
    };

};