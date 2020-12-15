var CtrlSpecificSearch = function () {
    var TotalCount = 0;
    var wSearchPanel = $("#divSearchPanel").width();
    var wSearchResult = $("#divSearchResult").width();
    var objFilter = null, objColStr = null;
    var dtSearchPanel = null;
    var dtMemo = null;
    var dtPrintItems = null;
    var objSF = null, objCart = null, objWL = null;
    var ValSavedSearch = null, ValSaveDemand = null, ValBestDeal = null, ValSendMail = null;
    var $SavedSearch = null; $SaveDemand = null, $BestDeal = null, $ModalFormSendMail = null;
    var LstOfCheckItems = [];
    var LstOfCheckItemsInventoryId = [];
    var page = 1;
    var PageSize = 12;
    var CurrentPageIdx = 1;
    var TotalRecordCount = 0;
    var SummeryTemp = '<tr class="rsearchnew" style="height: 25px;">\
        <td class="rsearchnew"><b><span id="lblcountsel">${TotalPcs}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblTCt1">${TotalCarat}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblNetORapValue">${AvgRapPerCT}</span></b></td>\
        <td class="rsearchnew"><b><span id="lbltotRap">${TotalRap}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblNetRapPer">${AvgRapoff}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblavgpriceafterdis">${PricePerct}</span></b></td>\
        <td class="rsearchnew"><b><span id="lblfinalamoutpayment">${PayableAmount}</span></b></td>\
        </tr>';


    //var ListViewDataTemplate = ' < div  class="col-md-3 col-sm-4 col-xs-6 post" >\
    //                            <div class="align-centerd diamond">\
    //                            <div class="img-container pointer" > <img alt="diamond" src="data:image/png;base64,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"></div>\
    //                            <div class="flex column space-around actions">\
    //                            <div class="flex space-between align-centered">\
    //                            <div class="lab">${Certificate}</div>\
    //                            <div class="lab">${Clarity}</div>\
    //                            <div class="lab add-link"><a href="#"  class="btnlistcart btn-link fa fa-shopping-cart fontsz"  data-id="' + Stock + '"   aria - hidden="true" ></a ></div > ' +
    //                            <div class="lab add-link"><a href="#"  class="btnlistWatchlist btn-link fa fa-list fontsz" data-id="' +  Stock + '"   aria - hidden="true" ></a ></div > ' +
    //                            </div>' +
    //                            <div class="flex align-centered space-between details"><div>' +  Shape + '</div><div>' +  Weight + '</div><div>' + item.Shade + '</div><div>' + item.Color + '</div></div>' +
    //                            <div class="flex space-between align-centered reserve"><div class="flex align-centered price"><div class="">' + item.Amount + '</div></div></div>' +
    //                            </div>' +
    //                            </div ></div > ';

    //var CompareTemp = '{{each(prop, val) Items}}\
    //                    <tr>\
    //                         <td class="rsearch comappop-head">${FHeader}</td>\
    //                         <td class="rsearch">${Field1}</td>\
    //                         <td class="rsearch">${Field2}</td>\
    //                         {{if Count >= 3}}\
    //                             <td class="rsearch">${Field3}</td>\
    //                         {{/if}}\
    //                         {{if Count >= 4}}\
    //                             <td class="rsearch">${Field4}</td>\
    //                         {{/if}}\
    //                         {{if Count >= 5}}\
    //                             <td class="rsearch">${Field5}</td>\
    //                         {{/if}}\
    //                    </tr>\
    //                   {{/each}}';

    var OnLoad = function () {
        objCart = new CartService();
        objWL = new WatchListService();
        objSF = new SearchFilter();
        objMemo = new MemoService();

        objFilter = new ModuleSearchFilter({
            parentID: "#divSpecificSearch",
            getRequestString: true,
            onSearched: function (data) {
                SearchFilterOnSearched(data);
            }
        });
        objColStr = new DataTableColumnStruct('SpecificSearch');

        dtSearchPanel = new Datatable();
        dtPrintItems = new Datatable();
        objFilter.init();

        $('#ddlCustomer').select2({
            ajax: {
                delay: 500,
                url: '/Home/GetListOfCustomer',
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

        RegisterEvent();

        SetValidation();

        bindSavedSearch();

        bindSavedDemand();

        LoadDataFromURL();

        $('[data-tooltip="#foo"]').tooltip();

        //ToggleFColor(); //Added by Ankit 23JUn2020
    }

    var RegisterEvent = function () {

        $('#btnBack').click(function (e) {
            var wSp = $("#divSearchPanel").data('width');
            e.preventDefault();
            $('#divSearchResult').hide();
            $("#divSearchPanel").css({
                'opacity': 0,
                'width': 0
            });
            $("#divSearchPanel").show();
            $("#divSearchPanel").animate({ opacity: "1", width: wSearchPanel }, 'slow', 'linear');
        });

        $('#btnCart').click(function (e) {

            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "warning" });
                return;
            }

            if ($('#hfAddToCartPermittedcount').val() == "" || $('#hfAddToCartPermittedcount').val() == null || $('#hfAddToCartPermittedcount').val() == undefined || $('#hfAddToCartPermittedcount').val() == 0) {
                $('#hfAddToCartPermittedcount').val(1000);
            }
            if (parseInt($('#spanCartCount').html()) + LstOfCheckItems.length > $('#hfAddToCartPermittedcount').val()) {
                uiApp.Alert({ container: '#uiPanel2', message: "You can add only " + $('#hfAddToCartPermittedcount').val() + " items on cart.", type: "warning" });
                return;

            }


            uiApp.Confirm('Do you want to add the selected items to cart?', function (resp) {
                if (resp) {
                    objCart.AddtoCart(LstOfCheckItems).then(function (data) {
                        if (data.IsSuccess) {
                            ReloadNavigationCount();
                            var strMsg = data.Result.RecentCartCount + " items added to cart. " + (data.Result.CartExist != 0 ? data.Result.CartExist + " items already added to cart" : "");
                            uiApp.Alert({ container: '#uiPanel2', message: strMsg, type: "success" });
                            TotalCount = TotalCount - data.Result.RecentCartCount;
                            objFilter.setCount(TotalCount);
                            dtSearchPanel.setAjaxParam('Total', TotalCount);
                            dtSearchPanel.getDataTable().ajax.reload();
                        } else {
                            uiApp.Alert({ container: '#uiPanel2', message: "Problem in adding to cart", type: "error" })
                        }
                        console.log(data);
                    }, function (error) {
                    });
                }
            });
        });

        $('#btnWatchList').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "warning" });
                return;
            }
            uiApp.Confirm('Do you want to add the selected items to watchlist?', function (resp) {
                if (resp) {
                    objWL.AddtoWatchList(LstOfCheckItems).then(function (data) {
                        if (data.IsSuccess) {
                            ReloadNavigationCount();
                            var strMsg = data.Result.RecentWatchCount + " items added to watchlist. " + (data.Result.WatchExist != 0 ? data.Result.WatchExist + " items already added to watchlist" : "");
                            uiApp.Alert({ container: '#uiPanel2', message: strMsg, type: "success" });
                            TotalCount = TotalCount - data.Result.RecentCartCount;
                            objFilter.setCount(TotalCount);
                            dtSearchPanel.setAjaxParam('Total', TotalCount);
                            dtSearchPanel.getDataTable().ajax.reload();
                        } else {
                            uiApp.Alert({ container: '#uiPanel2', message: "Problem in adding to watchlist", type: "error" })
                        }
                        console.log(data);
                    }, function (error) {
                    });
                }
            });
        });

        $('#btnCompare').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length <= 1) {
                uiApp.Alert({ container: '#uiPanel2', message: "Need to select at least 2 items to compare", type: "warning" });
                return;
            }
            if (LstOfCheckItems.length > 5) {
                uiApp.Alert({ container: '#uiPanel2', message: "Can show only 5 items.", type: "warning" });
                return;
            }

            objSF.GetInventory(LstOfCheckItems).then(function (data) {
                if (data.IsSuccess == true) {
                    $('#divCompareModal').modal('show');
                    $('#tblCompare').html('');
                    $('#tmpCompare').tmpl(data.Result).appendTo('#tblCompare');
                    var str = $('#tblCompare').html();
                    var res = str.replace(/&lt;/g, "<");
                    res = res.replace(/&gt;/g, ">");
                    $('#tblCompare').html('');
                    $('#tblCompare').html(res);
                    //$.tmpl(CompareTemp, data.Result).appendTo('#tblCompare');

                } else {
                    uiApp.Alert({ container: '#uiPanel2', message: "Problem in fetching data.", type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel2', message: "Some Error Occured.", type: "danger" });
            });
        });

        $('#btnAddToMemo').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "warning" });
                return;
            }
            $('#chkConfirmMemo').prop('checked', false);
            $('#chkSellDirectly').prop('checked', false);
            $('#txtMemoRemark').val('');

            objSF.GetInventoriesByLotID(LstOfCheckItems).then(function (data) {

                $('#Modal-FormMemo').one('shown.bs.modal', function (e) {
                    if (data.IsSuccess == true) {
                        BindMemoTable(data.Result);
                    } else {
                        uiApp.Alert({ container: '#uiPanel2', message: "Problem in fetching data.", type: "danger" });
                    }
                });
                $("#Modal-FormMemo").modal({
                    backdrop: 'static',
                    keyboard: false
                });
                //setTimeout(function () {

                //}, 1000);


            }, function (error) {
                uiApp.Alert({ container: '#uiPanel2', message: "Some Error Occured.", type: "danger" });
            });

        });

        $('#btnSaveMemo').click(function (e) {
            e.preventDefault();
            objMemo.Book(LstOfCheckItems,
                $('#ddlCustomer').val(),
                $('#chkConfirmMemo').prop('checked') == true ? 1 : 0,
                $('#chkSellDirectly').prop('checked') == true ? 1 : 0,
                $('#txtMemoRemark').val().trim()).then(function (data) {
                    if (data.IsSuccess) {
                        $('#Modal-FormMemo').modal('hide');
                        var strMsg = data.Result.validCount + " items added to memo. " + (data.Result.InvalidCount != 0 ? data.Result.InvalidCount + " items already added to memo or order" : "");
                        uiApp.Alert({ container: '#uiPanel2', message: strMsg, type: "success" });
                        TotalCount = TotalCount - data.Result.RecentCartCount;
                        LstOfCheckItems = [];
                        //Clear and reset form -- start --
                        dtSearchPanel.clearSelection();
                        $('#ddlCustomer').val('');
                        $('#ddlCustomer').trigger('change');
                        $('#chkConfirmMemo').prop('checked', false);
                        $('#chkSellDirectly').prop('checked', false);
                        //Clear and reset form -- end --
                        objFilter.setCount(TotalCount);
                        dtSearchPanel.setAjaxParam('Total', TotalCount);
                        dtSearchPanel.getDataTable().ajax.reload();

                    } else {
                        uiApp.Alert({ container: '#uiPanel2', message: "Problem in adding to memo", type: "error" })
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel2', message: "Some Error Occured.", type: "danger" });
                });
        });

        $('#btnSavedSearch').click(function (e) {
            e.preventDefault();
            var objSFData = objFilter.getQuery();

            if ($SavedSearch.valid() && (objSFData.query != "" && objSFData.query != undefined)) {
                var pData = {
                    recentSearchID: $('#ddlSavedSearch').val() == "" ? "0" : $('#ddlSavedSearch').val(),
                    searchCriteriaName: $('#txtSavedSearch').val().trim(),
                    searchType: 'SavedSearch',
                    searchCriteria: objSFData.query,
                    displayCriteria: objSFData.displayQuery,
                    isactive: true
                }

                objSF.AddRecent(pData).then(function (data) {
                    if (data.IsSuccess == true) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Saved search.", type: "success" });
                        clearSaveSearch();
                        bindSavedSearch();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Problem in saving search.", type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Some error occured.", type: "danger" });
                });
            }
        });

        $('#btnLoadSavedSearch').click(function (e) {
            e.preventDefault();
            var $ddlSavedSearch = $('#ddlSavedSearch').get(0);
            var query = $('option:selected', $ddlSavedSearch).attr('data-query');
            objFilter.setQuery(query, false, function (data) {
                SearchFilterOnSearched(data, false);
                clearSaveSearch();
            });
        });

        $('#btnSaveDemand').click(function (e) {
            e.preventDefault();
            var objSFData = objFilter.getQuery();

            if ($SaveDemand.valid() && (objSFData.query != "" && objSFData.query != undefined)) {
                var pData = {
                    recentSearchID: $('#ddlSaveDemand').val() == "" ? "0" : $('#ddlSaveDemand').val(),
                    searchCriteriaName: $('#txtSaveDemand').val().trim(),
                    searchType: 'DemandSearch',
                    searchCriteria: objSFData.query,
                    displayCriteria: objSFData.displayQuery,
                    isactive: true
                }

                objSF.AddRecent(pData).then(function (data) {
                    if (data.IsSuccess == true) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Saved demand.", type: "success" });
                        clearSaveDemand();
                        bindSavedDemand();
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Problem in saving demand.", type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Some error occured.", type: "danger" });
                });
            }
        });

        $('#btnLoadSavedDemand').click(function (e) {
            e.preventDefault();
            var $ddlSaveDemand = $('#ddlSaveDemand').get(0);
            var query = $('option:selected', $ddlSaveDemand).attr('data-query');
            objFilter.setQuery(query, false, function (data) {
                SearchFilterOnSearched(data, false);
                clearSaveSearch();
            });
        });

        $('#btnPrint').click(function (e) {
            e.preventDefault();

            objSF.GetStockForPrint('LOTNO~' + LstOfCheckItems.join(',')).then(function (data) {
                console.log(data);
                console.log(JSON.parse(data["Result"]));
                $('#tblBodyPrint').html('');
                var pData = JSON.parse(data["Result"]);
                uiApp.BindPrintTable(pData);
            });
        });

        $('#btnExcel').click(function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            objSF.ExportToExcelInventory('LOTNO~' + LstOfCheckItems.join(','), false, true, false).then(function (data) {
                if (data.IsSuccess) {
                    //window.location.href = "data:application/vnd.ms-excel;base64," + data.Result
                    $('#ancDownloadExcel').get(0).click();
                    //$('#ancDownloadExcel').trigger('click');
                    //window.open('/Inventory/ExportToExcelInventoryDownload');
                    //var locItem = window.location.href.split('/');
                    //locItem.splice(locItem.length - 1, 1);
                    //locItem.splice(locItem.length - 1, 1);
                    //var loc = locItem.join('/') + "/Inventory/ExportToExcelInventoryDownload";
                    //window.open(
                    //    loc,
                    //    "Download File",
                    //    "resizable,scrollbars,status"
                    //);
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                uiApp.UnBlockUI();
            });
        });

        $('#btnAllExcel').click(function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            objSF.ExportToExcelInventory($('#hfFilterText').val(), false, false, false).then(function (data) {
                if (data.IsSuccess) {
                    //window.location.href = "data:application/vnd.ms-excel;base64," + data.Result
                    //window.open('/Inventory/ExportToExcelInventoryDownload');
                    $('#ancDownloadExcel').get(0).click();
                    //$('#ancDownloadExcel').trigger('click');
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                uiApp.UnBlockUI();
            });
        });

        $('#btnAddBestDeal').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "danger" });
                return;
            }
            $('#Modal-AddToBestDeal').modal('show');
        });

        $('#btnSaveBestDeal').click(function (e) {
            e.preventDefault();
            if ($BestDeal.valid()) {
                //var rowData = dtSearchPanel.getData();
                //console.log(rowData);
                var pData = {
                    InventoryIDs: LstOfCheckItemsInventoryId.join(','),
                    Discount: $('#txtBestDealDiscount').val().trim(),
                    Remark: $('#txtBestDealRemark').val().trim(),
                };
                objSF.AddBestDealViaForm(pData).then(function (data) {
                    if (data.IsSuccess == true && data.Result > 0) {
                        uiApp.Alert({ container: '#uiPanel2', message: "Best deal added successfully", type: "success" });
                        dtSearchPanel.getDataTable().ajax.reload();
                        $('#Modal-AddToBestDeal').modal('hide');
                    } else {
                        uiApp.Alert({ container: '#uiPanel2', message: data.Message, type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel2', message: "Problem in adding best deal", type: "danger" });
                });
            }
        });

        $('#btnRemoveBestDeal').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItemsInventoryId.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "danger" });
                return;
            }
            uiApp.Confirm('Confirm remove best deal?', function (resp) {
                if (resp) {
                    objSF.RemoveBestDealViaForm(LstOfCheckItemsInventoryId.join(',')).then(function (data) {
                        if (data.IsSuccess == true && data.Result > 0) {
                            uiApp.Alert({ container: '#uiPanel2', message: "Best deal removed successfully", type: "success" });
                            dtSearchPanel.getDataTable().ajax.reload();
                        } else {
                            uiApp.Alert({ container: '#uiPanel2', message: data.Message, type: "danger" });
                        }
                    }, function (error) {
                        uiApp.Alert({ container: '#uiPanel2', message: "Problem in removing best deal", type: "danger" });
                    })
                }
            });
        });

        $('#btnInspectionReport').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "danger" });
                return;
            }
            $('#Modal-FormEmail').modal('show');
        });

        $('#btnSendInspectionReportMail').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "danger" });
                return;
            }
            objSF.SendEmail($('#txtMailFrom').val().trim(), $('#txtSubject').val().trim(), $('#txtMessage').val().trim(), LstOfCheckItems.join(',')).then(function (data) {
                if (data) {
                    uiApp.Alert({ container: '#uiPanel2', message: "Mail Sent", type: "success" });
                    $('#Modal-FormEmail').modal('hide');
                } else {
                    uiApp.Alert({ container: '#uiPanel2', message: "Mail Not Sent", type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel2', message: "Error in sending mail", type: "danger" });
            });
        });

        $('#btnEmail').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "danger" });
                return;
            }
            clearModalSendMail();
            $('#Modal-FormSendMail').modal('show');
        });

        $('#btnSendEmail').click(function (e) {
            e.preventDefault();
            if (LstOfCheckItems.length == 0) {
                uiApp.Alert({ container: '#uiPanel2', message: "No items selected", type: "danger" });
                return;
            }
            if ($('#frmModalFormSendMail').valid()) {
                var pData = {
                    filterText: "LOTNO~" + LstOfCheckItems.join(','),
                    EmailName: $('#txtEmailName').val(),
                    EMailTo: $('#txtEMailTo').val(),
                    EMailCC: $('#txtEMailCC').val(),
                    EMailBCC: $('#txtEMailBCC').val(),
                    Subject: $('#txtEMailSubject').val(),
                    Message: $('#txtEMailMessage').val()
                };
                objSF.InventorySendMail(pData).then(function (data) {
                    if (data.IsSuccess == true) {
                        $('#Modal-FormSendMail').modal('hide');
                        uiApp.Alert({ container: '#uiPanel2', message: "Mail Sent", type: "success" });
                    } else {
                        uiApp.Alert({ container: '#uiPanel2', message: "Mail Not Sent", type: "danger" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel2', message: "Error in sending mail", type: "danger" });
                });
            }
        });

        $('#fileUpload').change(function () {
            LoadFile(this, 'LotNo')
        });

        $('#fileUploadCert').change(function () {
            LoadFile(this, 'CertNo')
        });

        $('#dydReset').click(function (e) {
            e.preventDefault();
            var lstFrms = $('form').get();
            for (var i = 0; i < lstFrms.length; i++) {
                lstFrms[i].reset();
            }
            $('#singleSize').prop('checked', true);
            $('#multiSize').prop('checked', false);
            $('#divMultipleSizes').html('');
            $('#btnAddSizes').hide();
            objFilter.setQuery('', '');
        });

        //Added by Ankit 26Jun2020
        $('#btnMemoExcel').click(function (e) {
            e.preventDefault();
            uiApp.BlockUI();
            objSF.ExportToExcelInventory($('#hfFilterText').val(), false, false, true).then(function (data) {
                if (data.IsSuccess) {
                    //window.location.href = "data:application/vnd.ms-excel;base64," + data.Result
                    //window.open('/Inventory/ExportToExcelInventoryDownload');
                    $('#ancDownloadExcel').get(0).click();
                    //$('#ancDownloadExcel').trigger('click');
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                uiApp.UnBlockUI();
            });
        });
        $('#btnShowListView').click(function (e) {
            e.preventDefault();
            var criteria = $('#hfFilterText').val();
            $("#ListViewData").html("");
            var j = 1;
            var pageno = "page1";
            var k = 2;
            CurrentPageIdx = 1;
            uiApp.BlockUI();
            objSF.SpecificSearchListView(criteria).then(function (data) {
                if (data.IsSuccess) {
                    $.each(data.Result, function (i, item) {
                        if (j == 13) {
                            j = 1;
                            pageno = "page" + k;
                            k++;
                        }
                        var Images = '';
                        if (item.v360url == null || item.v360url == "") {
                            Images = '<img alt="diamond"   src="data:image/png;base64,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">';
                        } else {

                            if (item.v360url.endsWith(".mp4") == true) {
                                Images = '<video width="320" height="240" autoplay controls><source src=' + item.v360url + ' type="video/mp4"></video>';

                            } else {

                                Images = '<iframe  id="iframev360" src=' + item.v360url + ' width="480" height="480"  style="margin-left:65px;width: 275px;margin-left: 0px;" scrolling="no" frameborder="no" allowfullscreen=""></iframe>';
                            }
                        }

                        var newListItem = '<div id="' + pageno + '"    class="col-lg-2 col-md-3 col-sm-4 col-xs-6 post">' +
                            '<div class="align-centerd diamond">' +
                            '<div class="img-container pointer" ><a target="_blank" href="/DiamondSearch/DiamondView?id=' + item.inventoryID + '" class="thumblink"></a>' + Images +
                            //  '<img alt="diamond"   src="data:image/png;base64,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">' +

                            //  '<iframe id="iframev360" src="https://server.v360.in/vision360.html?d=gia-1203403009&amp;surl=https://bluenile.v360.in/12/imaged/gia-1203403009/1" width="480" height="480" style="margin-left:65px;width: 270px;margin-left: -10px;" scrolling="no" frameborder="no" allowfullscreen=""></iframe>'+

                            //'<iframe id="iframev360" src=' + item.v360url + ' width="480" height="480"  style="margin-left:65px;width: 270px;margin-left: -10px;" scrolling="no" frameborder="no" allowfullscreen=""></iframe>' +

                            '</div > ' +
                            '<div class="flex column space-around actions">' +
                            '<div class="flex space-between align-centered">' +
                            '<div class="lab">' + item.Certificate + '</div>' +
                            '<div class="lab">' + item.Clarity + '</div>' +
                            '<div   class="lab add-link"><a href="#"  class="btnlistcart btn-link"  data-id="' + item.Stock + '" data-toggle="tooltip" data-placement="top" title="Cart" ><img src="/Content/img/panel-cart.svg"></a ></div > ' +
                            '<div   class="lab add-link"><a href="#"  class="btnlistWatchlist btn-link" data-id="' + item.Stock + '"  data-toggle="tooltip" data-placement="top" title="Watch-List"><img src="/Content/img/addwatchlist-img.svg"></a ></div > </div>' +


                            '<div class="flex align-centered space-between details"><div>' + item.Shape + '</div><div>' + item.Weight + '</div><div>' + item.Shade + '</div><div>' + item.Color + '</div></div>' +
                            ' <div class="flex space-between align-centered reserve"><div class="flex align-centered price"><div class=""> $ ' + item.Amount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</div></div></div>' +
                            ' </div>' +
                            '</div ></div > ';

                        $("#ListViewData").append(newListItem);
                        j++;
                    });


                    //'<div class="lab add-link"><a href="#"  class="btnlistcart btn-link fa fa-shopping-cart fontsz"  data-id="' + item.Stock + '"   aria - hidden="true" ></a ></div > ' +
                    //    '<div class="lab add-link"><a href="#"  class="btnlistWatchlist btn-link fa fa-list fontsz" data-id="' + item.Stock + '"   aria - hidden="true" ></a ></div > ' +

                    //$('.pagination12 .post:not(#page' + page + ')').hide();
                    //$('.pagination12 .post#page' + page).show();


                    TotalRecordCount = data.Result.length;
                    GeneratePageIndex(data.Result.length, PageSize, CurrentPageIdx);



                    setTimeout(function () {
                        $('.pagination12 .post:not(#page' + page + ')').hide();
                        $('.pagination12 .post#page' + page).show();


                    }, 3000);


                    if ($('#hfRoleID').val() != 3) {
                        $('.btnlistcart').hide();
                        $('.btnlistWatchlist').hide();
                    } else {
                        $('.btnlistcart').show();
                        $('.btnlistWatchlist').show();
                    }

                    uiApp.UnBlockUI();
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: "No data found", type: "danger" });
                }
                uiApp.UnBlockUI();
            }, function (e) {
                uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "danger" });
                uiApp.UnBlockUI();
            });

        });


        function GeneratePageIndex(total, pageSize, pageIdx) {

            var $Page = $('#pnlPagination');
            var $Prev = $('<li><a href="#">Previous</a></li>');
            var $Next = $('<li><a href="#">Next</a></li>');
            var totalNodes = Math.ceil(total / pageSize);
            var visibleNodeQueue = [];
            var endindexss = 0;
            endindexss = totalNodes;

            if (totalNodes > 10) {
                endindexss = totalNodes;
                if (totalNodes > 10) {
                    endindexss = +pageIdx + 10;
                }
                if (endindexss > totalNodes) {
                    endindexss = totalNodes;
                } else { endindexss = endindexss; }

                if (endindexss > 10) {

                    if (pageIdx - endindexss < 10) {
                        if (pageIdx < endindexss) {
                            pageIdx = endindexss - 10;
                        }
                    } else { pageIdx = pageIdx; }

                }
                else { pageIdx = 1; }

            }
            for (var i = pageIdx; i <= endindexss; i++) {
                visibleNodeQueue.push(i);
            }
            $Page.html('');
            if (total <= pageSize) {
                $Page.addClass('hide');
            } else {
                $Page.removeClass('hide');
                if (pageIdx > 1) {
                    $Prev.data('pageIdx', '1')
                    $Page.append($Prev);
                }
                for (var i = 0; i < visibleNodeQueue.length; i++) {
                    if (visibleNodeQueue[i] == pageIdx) {
                        $Page.append('<li class="active"><a data-pageIdx="' + visibleNodeQueue[i] + '" href="#">' + visibleNodeQueue[i] + '</a></li>')
                    } else {

                        $Page.append('<li><a data-pageIdx="' + visibleNodeQueue[i] + '" href="#">' + visibleNodeQueue[i] + '</a></li>')
                    }
                }
                if (pageIdx < totalNodes) {
                    $Next.data('pageIdx', totalNodes);
                    $Page.append($Next);
                }

            }

        }

        $(document).on('click', '#pnlPagination > li > a', function (e) {
            e.preventDefault();
            if ($(this).attr('data-pageidx') != undefined) {
                CurrentPageIdx = $(this).attr('data-pageidx');
            } else {

                if ($(this).html().trim() == "Previous") {
                    CurrentPageIdx = CurrentPageIdx - 1

                } else {
                    CurrentPageIdx = +CurrentPageIdx + 1

                }

            }
            $('.pagination12 .post:not(#page' + CurrentPageIdx + ')').hide();
            $('.pagination12 .post#page' + CurrentPageIdx).show();

            GeneratePageIndex(TotalRecordCount, PageSize, CurrentPageIdx);

        });



        $(document).on('click', '.btnlistcart', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var listofstock = new Array(id);
            uiApp.Confirm('Do you want to add the selected items to cart?', function (resp) {
                if (resp) {
                    objCart.AddtoCart(listofstock).then(function (data) {
                        if (data.IsSuccess) {
                            ReloadNavigationCount();
                            var strMsg = data.Result.RecentCartCount + " items added to cart. " + (data.Result.CartExist != 0 ? data.Result.CartExist + " items already added to cart" : "");
                            uiApp.Alert({ container: '#uiPanel2', message: strMsg, type: "success" });
                            TotalCount = TotalCount - data.Result.RecentCartCount;
                            objFilter.setCount(TotalCount);
                            dtSearchPanel.setAjaxParam('Total', TotalCount);
                            dtSearchPanel.getDataTable().ajax.reload();
                        } else {
                            uiApp.Alert({ container: '#uiPanel2', message: "Problem in adding to cart", type: "error" })
                        }
                        console.log(data);
                    }, function (error) {
                    });
                }
            });
        });


        $(document).on('click', '.btnlistWatchlist', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var listofstock = new Array(id);

            uiApp.Confirm('Do you want to add the selected items to watchlist?', function (resp) {
                if (resp) {
                    objWL.AddtoWatchList(listofstock).then(function (data) {
                        if (data.IsSuccess) {
                            ReloadNavigationCount();
                            var strMsg = data.Result.RecentWatchCount + " items added to watchlist. " + (data.Result.WatchExist != 0 ? data.Result.WatchExist + " items already added to watchlist" : "");
                            uiApp.Alert({ container: '#uiPanel2', message: strMsg, type: "success" });
                            TotalCount = TotalCount - data.Result.RecentCartCount;
                            objFilter.setCount(TotalCount);
                            dtSearchPanel.setAjaxParam('Total', TotalCount);
                            dtSearchPanel.getDataTable().ajax.reload();
                        } else {
                            uiApp.Alert({ container: '#uiPanel2', message: "Problem in adding to watchlist", type: "error" })
                        }
                        console.log(data);
                    }, function (error) {
                    });
                }
            });
        });
        //$(document).on('click', 'a.ancMemoItems', function (e) {
        //    e.preventDefault();
        //    var Inventoryid = $(e.target).attr('data-id');
        //    objSF.GetMemoDetailsInventoryid(Inventoryid).then(function (data) {
        //        if (data.IsSuccess == true) {
        //            window.location.href = '/Memo';
        //          MemoDetailsShow(data.Result.OrderID,"");

        //        } else {
        //            uiApp.Alert({ container: '#uiPanel1', message: "Details not found", type: "error" });
        //        }
        //    }, function (error) {
        //        uiApp.Alert({ container: '#uiPanel1', message: "Some error occured", type: "error" });
        //    });


        //});



        ////Added by Ankit 23Jun2020
        //$('#FColorMore').click(function (e) {
        //    e.preventDefault();
        //    ToggleFColor();
        //});

        //$('#btnF_Color').click(function (e) {
        //    e.preventDefault();
        //    var countF_Certificate = $("form.F_Color > label").length;
        //    var fieldId = "";
        //    for (i = 1; i <= countF_Certificate; i++) {
        //        fieldId = 'color' + i;
        //        $('#' + fieldId).prop("checked", false);

        //    }
        //    ReadLotNos(true);

        //});

        //$('#btnF_Certificate').click(function (e) {
        //    e.preventDefault();
        //    var countF_Certificate = $("form.F_Certificate > label").length;
        //    var fieldId = "";
        //    for (i = 1; i <= countF_Certificate; i++) {
        //        fieldId = 'lab' + i;
        //        $('#' + fieldId).prop("checked", false);

        //    }

        //});
        //$('#btnF_Clarity').click(function (e) {
        //    e.preventDefault();
        //    var countF_Certificate = $("form.F_Clarity > label").length;
        //    var fieldId = "";
        //    for (i = 1; i <= countF_Certificate; i++) {
        //        fieldId = 'clarity' + i;
        //        $('#' + fieldId).prop("checked", false);

        //    }

        //});


        //$('#btnF_Fluorescence').click(function (e) {
        //    e.preventDefault();
        //    var countF_Certificate = $("form.F_Fluorescence > label").length;
        //    var fieldId = "";
        //    for (i = 1; i <= countF_Certificate; i++) {
        //        fieldId = 'fluor' + i;
        //        $('#' + fieldId).prop("checked", false);

        //    }

        //});

        //$('#btnF_Cut').click(function (e) {
        //    e.preventDefault();
        //    var countF_Certificate = $("Div.F_Cut > label").length;
        //    var fieldId = "";
        //    for (i = 1; i <= countF_Certificate; i++) {
        //        fieldId = 'cut' + i;
        //        $('#' + fieldId).prop("checked", false);

        //    }

        //});

        //$('#btnF_Polished').click(function (e) {
        //    e.preventDefault();
        //    var countF_Certificate = $("Div.F_Polished > label").length;
        //    var fieldId = "";
        //    for (i = 1; i <= countF_Certificate; i++) {
        //        fieldId = 'Polished' + i;
        //        $('#' + fieldId).prop("checked", false);

        //    }

        //});

        //$('#btnF_Symmerty').click(function (e) {
        //    e.preventDefault();
        //    var countF_Certificate = $("Div.F_Symmerty > label").length;
        //    var fieldId = "";
        //    for (i = 1; i <= countF_Certificate; i++) {
        //        fieldId = 'Symmerty' + i;
        //        $('#' + fieldId).prop("checked", false);

        //    }

        //});

    }

    var LoadFile = function (ele, type) {
        var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.csv)$/;
        if (regex.testele.value.toLowerCase()) {
            if (typeof (FileReader) != "undefined") {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var rows = e.target.result.split("\n");
                    console.log(e.target.result);
                    if (type == 'LotNo') {
                        $('#lotnumber').val(rows);
                        $('#lotnumber').trigger('keyup');
                    } else if (type == 'CertNo') {
                        $('#certNo').val(rows);
                        $('#certNo').trigger('keyup');
                    }
                }
                reader.readAsText(ele.files[0]);
            } else {
                uiApp.Alert({ container: '#uiPanel1', message: "This browser does not support HTML5.", type: "error" });
            }
        }
    }

    var SetValidation = function () {
        $SavedSearch = $('#frmSavedSearch');
        $SaveDemand = $('#frmSaveDemand');
        $BestDeal = $('#frmBestDeal');
        $ModalFormSendMail = $('#frmModalFormSendMail');

        ValSavedSearch = $SavedSearch.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                SavedSearch: {
                    required: true
                }
            }
        });

        ValSaveDemand = $SaveDemand.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                SaveDemand: {
                    required: true
                }
            }
        });

        ValBestDeal = $BestDeal.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                BestDealDiscount: {
                    required: true,
                    percentNumber: true
                },
                BestDealRemark: {
                    required: true
                }
            }
        });

        ValSendMail = $ModalFormSendMail.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                //EMailFrom: {
                //    required: true,
                //    regex: /\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/,
                //},
                EmailName: {
                    required: true
                },
                EMailTo: {
                    required: true,
                    regex: /\w+[.]?\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/,
                },
                EMailCC: {
                    regex: /\w+[.]?\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/,
                },
                EMailBCC: {
                    regex: /\w+[.]?\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/,
                },
                EMailSubject: {
                    required: true
                }
            }, messages: {
                //EMailFrom: {
                //    regex: "Invalid Email"
                //},
                EMailTo: {
                    regex: "Invalid Email"
                },
                EMailCC: {
                    regex: "Invalid Email"
                },
                EMailBCC: {
                    regex: "Invalid Email"
                }
            }
        });

        $.validator.addMethod("percentNumber", function (value, element, param) {
            return (value >= 0 && value <= 100);
        }, "Please enter valid percentage i.e min value 0 to max value 100.");

    };



    function BindMemoTable(data) {
        if (dtMemo == null && dtMemo == undefined) {
            dtMemo = new Datatable();
        }

        if (dtMemo.getDataTable() == null || dtMemo.getDataTable() == undefined) {
            var colStruct = new DataTableColumnStruct();
            var colDef = colStruct.SpecificSearch.columnDefs;
            colDef[0]["visible"] = false;
            dtMemo.init({
                src: '#MemoTable',
                dataTable: {
                    //deferLoading: 0,
                    paging: false,
                    scrollY: "270px",
                    scrollX: true,
                    order: [[1, "desc"]],
                    processing: false,
                    serverSide: false,
                    data: data,
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colDef,
                },
                onCheckboxChange: function (obj) {

                }
            });
            dtMemo.getDataTable().columns.adjust().draw();
        } else {
            dtMemo.clearSelection();
            var table = dtMemo.getDataTable();
            table.clear().draw();
            for (var i = 0; i < data.length; i++) {
                table.row.add(data[i]);
            }
            //table.draw(false);
            table.columns.adjust().draw(false);
        }

    }

    function SearchFilterOnSearched(data, AddToRecentSearch, NewArrival) {
        TotalCount = data.count;
        console.log(data);
        filterText = data.query;
        $('#hfFilterText').val(data.query);
        $('#hfAddToRecentSearch').val('false');
        dtSearchPanel.setAjaxParam('filterText', data.query);
        dtSearchPanel.setAjaxParam('displayCriteria', data.displayQuery);
        dtSearchPanel.setAjaxParam('Total', data.count[4].ResultCount);
        if (AddToRecentSearch != undefined) {
            dtSearchPanel.setAjaxParam('AddToRecentSearch', AddToRecentSearch);
        }
        $('#hfNewArrival').val('false');
        if (NewArrival != undefined) {
            dtSearchPanel.setAjaxParam('NewArrival', NewArrival);
            $('#hfNewArrival').val('true');
        }
        // $('#tblSoldInDays').html('');
        //$('#tmpSoldInDays').tmpl(data).appendTo("#tblSoldInDays");
        BindSoldIn(data);
        var RoleID = $('#hfRoleID').val();
        var colStruct = new DataTableColumnStruct('SpecificSearch');

        //New Column Added by Ankit 24JUn2020


        if (dtSearchPanel.getDataTable() == null || dtSearchPanel.getDataTable() == undefined) {
            dtSearchPanel.init({
                src: '#SearchTablePost',
                dataTable: {
                    //deferLoading: 0,
                    scrollY: "485px",
                    scrollX: true,
                    //scrollCollapse: true,
                    paging: true,
                    pageLength: 200,
                    // pageLength: (RoleID == "1" || RoleID == "2" || RoleID == "9" || RoleID == "2") ? 500000 : 100,
                    createdRow: function (row, data, dataIndex) {
                        if (data.Milky == "M1" || data.Milky == "M2" || data.Milky == "M3") {
                            $(row).find('td:eq(0)').addClass('milky');
                        }
                        if (data.Shade == "BR1" || data.Shade == "BR2" || data.Shade == "BR3"
                            || data.Shade == "GR1" || data.Shade == "GR2" || data.Shade == "GR3" || data.Shade == "MIX") {
                            $(row).find('td:eq(0)').addClass('milky');
                        }
                    },
                    lengthMenu: [[50, 100, 200, 500000], [50, 100, 200, "All"]],
                    order: [[1, "desc"]],
                    ajax: {
                        type: 'Post',
                        url: '/Inventory/StockList2',
                        beforeSend: function (request) {
                            var TokenID = myApp.token().get();
                            request.setRequestHeader("TokenID", TokenID);
                            uiApp.BlockUI();
                            return request;
                        },
                        dataFilter: function (data) {
                            uiApp.UnBlockUI();
                            return data;
                        }
                        , dataSrc: function (json) {
                            //json = JSON.parse(json);
                            console.log(json);
                            return json.data;
                        }
                    },
                    columns: colStruct.SpecificSearch.columns,
                    columnDefs: colStruct.SpecificSearch.columnDefs,
                    rowCallback: function (row, data, index) {
                        //console.log('test', row, data, index);
                        if (data.IsInBestDeal != "0") {
                            //$(row).find('td:eq(5)').css('background-color', '#61fa90');
                        }
                        if (data.labStatus == true) {
                            //$(row).find('td:eq(5)').css('background-color', '#fff700');
                        }
                        if (data.HeartAndArrow != "" && data.HeartAndArrow != "NO") {
                            //$(row).find('td:eq(5)').css('background-color', '#e600c4');
                            //$(row).find('td:eq(5)').attr('title', data.HeartAndArrow);
                        }
                        if (data.CreatedOn == moment().format("MMM DD YYYY")) {
                            //$(row).find('td:eq(5)').css('background-color', '#fba723');
                        }
                        if (data.stockStatusID == 23) {
                            $(row).css('background-color', '#cccccc');
                            //$(row).find('td:eq(5)').css('background-color', '#fbd0ec');
                            //$(row).find('td:eq(5)').attr('title', data.refdata);
                        }

                    }
                },
                onCheckboxChange: function (obj, objInv) {
                    ListOfChecks(obj, objInv);
                }
            });
            console.log(dtSearchPanel.getDataTable());
        } else {
            dtSearchPanel.clearSelection();
            dtSearchPanel.getDataTable().draw();
        }
        $('#spanTotalCount').html(data.count[0].ResultCount);
        $('#spanAvailableCount').html(data.count[4].ResultCount);
        //$('#spanOrderPendingCount').html(0);
        $('#spanOnMemoCount').html(data.count[3].ResultCount);
        ListOfChecks([]);

        //dt.draw(false);
        console.log(data);
        $("#divSearchPanel").hide();
        $("#divSearchPanel").animate({ opacity: "1" }, 'slow', 'linear');
        $('#divSearchResult').show(1000);

    }

    function ListOfChecks(obj, objInv) {
        LstOfCheckItems = obj;
        LstOfCheckItemsInventoryId = objInv;
        //$('#hfFilterText').val('LOTNO~' + LstOfCheckItems.join(','));
        if (obj.length > 0) {
            objSF.SummaryData(obj).then(function (data) {
                $("#tblBodySummary").html('');
                data.Result.TotalPcs = obj.length;

                data.Result.AvgRapPerCT = data.Result.AvgRapPerCT.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.AvgRapoff = data.Result.AvgRapoff.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.PayableAmount = data.Result.PayableAmount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.PricePerct = data.Result.PricePerct.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.TotalCarat = data.Result.TotalCarat.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                data.Result.TotalRap = data.Result.TotalRap.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");


                $.tmpl(SummeryTemp, data.Result).appendTo("#tblBodySummary");
            }, function (error) {
            });
        } else {
            $("#tblBodySummary").html('');
            var data = {
                Result: {
                    TotalPcs: obj.length
                }
            };
            $.tmpl(SummeryTemp, data.Result).appendTo("#tblBodySummary");
        }
    }

    function clearSaveSearch() {
        $('#txtSavedSearch').val('');
        $('#ddlSavedSearch').val('');
    }

    function clearSaveDemand() {
        $('#txtSaveDemand').val('');
        $('#ddlSaveDemand').val('');
    }

    function clearModalSendMail() {
        $('#txtEmailName').val('');
        $('#txtEMailTo').val('');
        $('#txtEMailCC').val('');
        $('#txtEMailBCC').val('');
        $('#txtEMailSubject').val('');
        $('#txtEMailMessage').val('');
    }

    function bindSavedSearch() {
        $('#ddlSavedSearch').html('');
        objSF.GetRecentForOptions('SavedSearch').then(function (data) {
            if (data.IsSuccess) {
                $.tmpl('<option data-query="${Text2}" value="${Value}">${Text}</option>', [{ Value: "", Text: "<-- Select Search -->", Text2: "" }]).appendTo('#ddlSavedSearch');
                $.tmpl('<option data-query="${Text2}" value="${Value}">${Text}</option>', data.Result).appendTo('#ddlSavedSearch');
            }
        });
    }

    function bindSavedDemand() {
        $('#ddlSaveDemand').html('');
        objSF.GetRecentForOptions('DemandSearch').then(function (data) {
            if (data.IsSuccess) {
                $.tmpl('<option data-query="${Text2}" value="${Value}">${Text}</option>', [{ Value: "", Text: "<-- Select Search -->", Text2: "" }]).appendTo('#ddlSaveDemand');
                $.tmpl('<option data-query="${Text2}" value="${Value}">${Text}</option>', data.Result).appendTo('#ddlSaveDemand');
            }
        });
    }

    function LoadDataFromURL() {
        //var c = myApp.getUrlData('c');
        var c = $('#hfQuery').val();
        if (c != undefined && c != "") {
            var n = false;
            if (c == "NewArrival") {
                c = "";
                n = true;
            }
            objFilter.setQuery(c, n, function (data) {
                SearchFilterOnSearched(data, false, n);
                clearSaveSearch();
            });
        }
    }

    function ReloadNavigationCount() {
        if (CtrlMasterLayout != undefined) {
            CtrlMasterLayout.LoadCounts();
        }
    }




    function BindSoldIn(data11) {
        $('#tmpSoldInDays121').html('');
        var Finalsold = "";
        for (i = 5; i <= data11.count.length-1; i++) {
            Finalsold = '<li>' +
                '<a href="#">' +
                '<span class="summerydays">' +
                '<span class="inday">' + data11.count[i].countName + ' </span>' +
                '<span class="sold"><span style="color:#E9978C;">' + data11.count[i].ResultCount + '</span> Sold</span>' +
                '</span>' +
                '</a>' +
                ' </li> ';
            $('#tmpSoldInDays121').append(Finalsold);

        }



    }

    ////Added by Ankit 23JUn2020
    //function ToggleFColor() {
    //    var countcolor = $("form.F_Color > label").length;

    //    var fieldId = "";
    //    for (i = 11; i <= countcolor; i++) {
    //        fieldId = 'color' + i;
    //        $("label[for='" + fieldId + "']").toggle();
    //        $('#' + fieldId).prop("checked", false);

    //    }
    //    if ($("#FColorMore").val() == 'More') {
    //        $("#FColorMore").prop('value', 'Less');
    //    } else {
    //        $("#FColorMore").prop('value', 'More');

    //    }


    //}
    return {
        init: function () {
            OnLoad();
        }
    }
}();

$(document).ready(function () {
    CtrlSpecificSearch.init();
});
