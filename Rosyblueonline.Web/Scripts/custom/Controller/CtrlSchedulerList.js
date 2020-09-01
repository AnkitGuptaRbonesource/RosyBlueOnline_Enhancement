var CtrlSchedulerList = function () {
    var  dtScheduler = null;
    var $frmSchedulerDetail = null;
    var ValUserDetail = null;
    var OnLoad = function () {
       
        dtScheduler = new Datatable();
        LoadGrid();
    }

    var LoadGrid = function () {

        if (dtScheduler.getDataTable() == null || dtScheduler.getDataTable() == undefined) {
            dtScheduler.init({
                src: '#tblDetails',
                dataTable: {
                    //deferLoading: 0,
                    paging: true,
                    order: [[0, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/WS_Scheduler/GetList',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            return request;
                        }
                    },
                    columns: [
                        { data: "WSID" },
                        { data: "Name" },
                        { data: "Frequency" },
                        { data: "FrequencyInterval" },
                        { data: "Status" },
                        { data: "LastRun" },
                        { data: "NextRun" },
                        { data: "modifiedBy" },
                        { data: "modifiedOn" }
                    ],
                    columnDefs: [{
                        targets: [0],
                        render: function (data, type, row) {
                            return row.WSID;
                        },
                        orderable: true
                    },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            return row.Name;
                        },
                        orderable: true
                    },
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            return row.Frequency;
                        },
                        orderable: false
                    },
                    {
                        targets: [3],
                        render: function (data, type, row) {
                            return row.FrequencyInterval;
                        },
                        orderable: false
                    },
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            return row.Status;
                        },
                        orderable: false
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            //return row.LastRun;
                            return moment(row.LastRun).format(myApp.dateFormat.Client);
                        },
                        orderable: false
                    },
                    {
                        targets: [6],
                        render: function (data, type, row) {
                            //return row.NextRun;
                            return moment(row.NextRun).format(myApp.dateFormat.Client);
                        },
                        orderable: false
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            return row.modifiedBy;
                        },
                        orderable: false
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            //return row.modifiedOn;
                            return moment(row.modifiedOn).format(myApp.dateFormat.Client);
                        },
                        orderable: false
                        }, {
                            targets: [9],
                            orderable: false,
                            render: function (data, type, row) {
                                //  return "<a id='btnEditDetails' href='/WS_Scheduler/Edit/" + row.WSID + "'>Edit</a>";
                                // return '<a class="ancShowItems" data-id="' + row.orderDetailsId + '" " data-id="' + row.orderDetailsId + '" data-disc="' + row.orderAvgRapOff + '" data-name="' + (row.firstName + " " + row.lastName + " | " + row.companyName) + '" href="#" >' + data + '</a>';

                                return '<a class="btnEditDetails" data-id="' + row.WSID + '"  data-Name="' + row.Name + '"  data-Frequency="' + row.Frequency + '"  data-FrequencyInterval="' + row.FrequencyInterval + '"  data-status="' + row.Status + '"  href="#">Edit</a>';
                            }
                    }]
                }
            });
        } else {
            dtScheduler.getDataTable().draw();
        }

    };

    var RegisterEvents = function () {
        $(document).on('click', 'a.btnEditDetails', function (e) {
            e.preventDefault();
            var id = $(e.target).data('id');
            var Name = $(e.target).data('name');
            var Frequency = $(e.target).data('frequency');
            var FrequencyInterval = $(e.target).data('frequencyinterval');
            var Status = $(e.target).data('status');

            $('#txtScheduler_Name').val($(e.target).data('name'));
            $('#txtScheduler_Frequency').val($(e.target).data('frequency'));
            $('#txtScheduler_FrequencyInt').val($(e.target).data('frequencyinterval'));            
           // $('#txtScheduler_Status').val($(e.target).data('status'));

             
            $('input[name=Scheduler_Status][value='+$(e.target).data("status")+']').prop('checked', true);

            $('#hfWSID').val(id);
            $('#frmSchedulerDetail').show();

            //var index = $(e.target).data('index');
            //  var oData = dtOrder.getDataTable().rows().data()[index];
            //  console.log(oData);
            //  uiApp.BlockUI();
            // Refname = (oData.companyName + '|' + oData.firstName + ' ' + oData.lastName);
          //  AllowEditCustomer(id).then(function (rData) {
             
                //var newOption = new Option((oData.companyName + ' | ' + oData.firstName + ' ' + oData.lastName), oData.loginID, false, false);
                //$('#Modal-EditMemo-Message').html('<b>Ref : ' + (oData.companyName + '|' + oData.firstName + ' ' + oData.lastName) + '</b>');
                //Refname = (oData.companyName + '|' + oData.firstName + ' ' + oData.lastName);
                //$('#ddlCustomer2').append(newOption).trigger('change');
                //$('#hfOrderID').val(id);
                //$('#txtEditMemoRemark').val(oData.remark);
                //$('#Modal-EditMemo').modal('show');

                // uiApp.UnBlockUI();
          // }, function () {
                //  uiApp.UnBlockUI();
           // });

        });


        
    }

    $('#btnUpdateDetails').click(function (e) {
        e.preventDefault();
        var Name = $('#txtScheduler_Name').val().trim();
        var Frequency = $('#txtScheduler_Frequency').val().trim();
        var FrequencyInt = $('#txtScheduler_FrequencyInt').val().trim();  
        var Status = $('input:radio[name=Scheduler_Status]:checked').val().trim();
        var WSID = $('#hfWSID').val();

        if ($frmSchedulerDetail.valid()) {
            uiApp.Confirm('Confirm Update for <b>' + Name + '</b>?', function (resp) {
                if (resp) {
                    UpdateMemo(WSID, Name, Frequency, FrequencyInt, Status).then(function (data) {
                        if (data.IsSuccess == true && data.Result > 0) {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo updated successfully", type: "success" });
                            dtScheduler.getDataTable().ajax.reload();
                            //dtOrder.clearSelection();
                            //$('#Modal-EditMemo').modal('hide');
                            clearForm();
                            $('#frmSchedulerDetail').hide();
                        } else {
                            uiApp.Alert({ container: '#uiPanel1', message: "Memo not updated", type: "error" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
                    });
                }
            });
        }
        //else {
        //    uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });

        //}
       
    });

    var UpdateMemo = function (WSID,Name, Frequency, FrequencyInt, Status) {
        return myApp.http({
            method: 'post',
            url: '/WS_Scheduler/UpdateScheduler',
            data: {
                WSID: WSID,
                Name: Name,
                Frequency: Frequency,
                FrequencyInt: FrequencyInt,
                Status: Status
            }
        });
    };

    $('#btnCancel').click(function (e) {
        e.preventDefault();
        clearForm();
        $('#frmSchedulerDetail').hide();


    });

    function clearForm() { 
        $('#txtScheduler_Name').val('');
        $('#txtScheduler_Frequency').val('');
        $('#txtScheduler_FrequencyInt').val('');
        $('#hfWSID').val(''); 
        $('#rbtnActiveS').prop('checked', false);
        $('#rbtnInActiveS').prop('checked', false); 
      //  ClearValidation();
        ValUserDetail.resetForm();
    }
   
    var SetValidation = function () {
        $frmSchedulerDetail = $('#frmSchedulerDetail');
        ValUserDetail = $frmSchedulerDetail.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                Scheduler_Name: {
                    required: true 
                },
                Scheduler_Frequency: {
                    required: true
                },
                Scheduler_FrequencyInt: {
                    required: true 
                    
                }
            }
        });
    };

    var ClearValidation = function () {
        $frmSchedulerDetail = $('#frmSchedulerDetail');
        ValUserDetail = $frmSchedulerDetail.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                Scheduler_Name: {
                    required: false
                },
                Scheduler_Frequency: {
                    required: false
                },
                Scheduler_FrequencyInt: {
                    required: false

                }
            }
        });
    };

    return {
        init: function () {
            OnLoad();
            RegisterEvents();
            SetValidation();
            
        }
    }
}();
$(document).ready(function () {
    CtrlSchedulerList.init();
});