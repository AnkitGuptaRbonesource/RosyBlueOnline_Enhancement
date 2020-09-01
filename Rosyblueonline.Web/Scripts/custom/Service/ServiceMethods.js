var LoginRegistrationService = function () {
    var obj = {};
   
    obj.GenerateOtp = function (EmailID) {
        return myApp.http({
            method: 'post',
            url: '/Home/GenerateOtp',
            data: {
                EmailID: EmailID
            }
        });
    };

    obj.VerifyOtp = function (EmailID, OTP) {
        return myApp.http({
            method: 'post',
            url: '/Home/VerifyOtp',
            data: {
                EmailID: EmailID,
                OTP: OTP
            }
        });
    };

    obj.Register = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/Home/Register',
            data: pData,
            contentType: false,
            processData: false,
        });
    };

    obj.RegisterUser = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/User/Registration',
            data: pData,
        });
    };

    obj.RegistrationViaMemo = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/User/RegistrationViaMemo',
            data: pData,
        });
    };

    obj.UpdateRegisterUser = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/User/UpdateRegistration',
            data: pData,
        });
    };

    obj.CheckUserName = function (UserName, loginID) {
        return myApp.http({
            method: 'post',
            url: '/Home/CheckUserName',
            data: {
                UserName: UserName,
                loginID: loginID
            }
        });
    };

    obj.Login = function (pObj) {
        return myApp.http({
            method: 'post',
            url: '/Home/Login',
            data: pObj
        });
    };

    obj.ApproveCustomer = function (LoginID, isApproved) {
        return myApp.http({
            method: 'post',
            url: '/Home/ApproveCustomer',
            data: {
                LoginID: LoginID,
                isApproved: isApproved
            }
        });
    };

    obj.ResetPassword = function (oldpassword, password) {
        return myApp.http({
            method: 'post',
            url: '/User/ResetPassword',
            data: {
                oldpassword: oldpassword,
                password: password
            }
        });
    };

    obj.GetUserDetail = function (LoginID) {
        if (LoginID == undefined || LoginID == null) {
            LoginID = 0;
        }
        return myApp.http({
            method: 'get',
            url: ('/User/GetUserDetail?qLoginID=' + LoginID.toString())
        });
    }

    obj.BlockSite = function (password) {
        return myApp.http({
            method: 'post',
            url: '/User/UpdateBlockSite',
            data: {
                password: password
            }
        });
    }

    obj.ForgetPassword = function (EmailID) {
        return myApp.http({
            method: 'post',
            url: '/Home/ForgetPassword',
            data: {
                EmailID: EmailID
            }
        });
    }

    obj.ResetForgetPassword = function (Code, ID, Password) {
        return myApp.http({
            method: 'post',
            url: '/Home/ResetForgetPassword',
            data: {
                ID: ID,
                Code: Code,
                Password: Password
            }
        });
    }

    //Added by Ankit 01July2020
    obj.UploadMultiDoc = function (DocData) {
        return myApp.http({
            method: 'post',
            url: '/Home/MultipleDocUpload',
            data: DocData ,
            contentType: false,
            processData: false,
        });
    };
    //Added by Ankit 09July2020
    obj.DeleteUserDoc = function (UserDocId, DocRandomID) {
        return myApp.http({
            method: 'post',
            url: '/Home/DeleteUserDoc',
            data: {
                UserDocId: UserDocId,
                DocRandomID: DocRandomID

            } 
        });
    };
    
    obj.GenerateRandomId = function () {
        return myApp.http({
            method: 'post',
            url: '/Home/GenerateRandomId',
            data: null,
            contentType: false,
            processData: false,
        });
    };

    obj.SaveMenuAccess = function (UserId,labelCheckBox) {
        return myApp.http({
            method: 'post',
            url: '/MenuPermissionMaster/SaveMenuAccess',
            data: {
                UserId: UserId,
                MenuIds: labelCheckBox 

            } 
        });
    };



    obj.GetMenuAccessdata = function (UserId) {
        return myApp.http({
            method: 'post',
            url: '/MenuPermissionMaster/GetMenuAccessdata',
            data: {
                UserId: UserId 

            } 
        });
    };


    obj.AddUpdateSearchPermission = function (startSizePermitted, rowDownloadPermitted, SPLoginId) {
        return myApp.http({
            method: 'post',
            url: '/MenuPermissionMaster/AddUpdateSearchPermission',
            data: {
                startSizePermitted: startSizePermitted,
                rowDownloadPermitted: rowDownloadPermitted,
                SPLoginId: SPLoginId

            }
        });
    };
    
    



    return obj;
};

var SearchFilter = function () {
    var obj = {};

    obj.StockCount = function (data, NewArrival) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/StockCount',
            data: {
                filterText: data,
                NewArrival: NewArrival
            }
        });
    };

    obj.StockList = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/StockList',
            data: {
                filterText: data
            }
        });
    };

    obj.SummaryData = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/SummaryData',
            data: {
                lotnumber: data.join()
            }
        });
    };

    obj.GetInventory = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetInventory',
            data: {
                LotIDs: data.join()
            }
        });
    };

    obj.GetInventoriesByLotID = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetInventoriesByLotID',
            data: {
                LotIDs: data.join()
            }
        });
    };

    obj.AddRecent = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/AddRecent',
            data: {
                obj: data
            }
        });
    };

    obj.GetRecentForOptions = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetRecentForOptions',
            data: {
                SearchType: data
            }
        });
    };

    obj.UploadInventory = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/InventoryUpload',
            data: pData,
            processData: false,
            contentType: false
        });
    };

    obj.UploadStoneStatus = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/UploadStoneStatus',
            data: pData,
            processData: false,
            contentType: false
        });
    };

    obj.ExportToExcelStoneStatus = function (LotNos, FileName, Type) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/ExportToExcelStoneStatus',
            data: {
                LotNos: LotNos,
                FileName: FileName,
                Type: Type
            }
        });
    };

    obj.GetGIADataFromExcel = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetGIADataFromExcel',
            data: pData,
            processData: false,
            contentType: false
        });
    };

    obj.GetDataFromGiaApi = function (ip, pData) {

        var data = [];
        data.push(pData);
        //for (var i = 0; i < pData.length; i++) {
        //    data.push({
        //        reportNo: pData[i].Certificate,
        //        reportWeight: pData[i].Weight,
        //    });
        //}
        var jStr = JSON.stringify({
            header: {
                ipAddress: ip,
            },
            body: {
                reportDtls: {
                    reportDtl: data
                }
            }
        });
        return $.ajax({
            type: 'POST',
            url: 'https://labws.gia.edu/ReportCheck/processrequest.do',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            data: jStr
        });
    };

    obj.ExportToExcelInventory = function (fText, newArrival, IsSpecialSearch, IsOnlyMemo) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/ExportToExcelInventory',
            data: {
                filterText: fText,
                NewArrival: newArrival,
                IsSpecialSearch: IsSpecialSearch,
                IsOnlyMemo: IsOnlyMemo
            }
        });
    };

    obj.SpecificSearchListView = function (fText) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/SpecificSearchListView',
            data: {
                Query: fText 
            }
        });
    };

    obj.AddBestDealViaForm = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/AddBestDealViaForm',
            data: pData
        });
    };

    obj.RemoveBestDealViaForm = function (InventoryIDs) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/RemoveBestDealViaForm',
            data: {
                InventoryIDs: InventoryIDs
            }
        });
    };

    obj.SendEmail = function (FromEmailID, Subject, Message, LotNo) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/SendEmail',
            data: {
                FromEmailID: FromEmailID,
                Subject: Subject,
                Message: Message,
                LotNo: LotNo
            }
        });
    }

    obj.InventorySendMail = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/InventorySendMail',
            data: pData
        });
    }

    obj.GetStockForPrint = function (text) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetStockForPrint',
            data: {
                filterText: text
            }
        });
    }

    obj.GetMemoDetailsInventoryid = function (Inventoryid) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetMemoDetailsInventoryid',
            data: {
                Inventoryid: Inventoryid
            }
        });
    };

    obj.GetCustomerDetailsByID = function (ID) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetOrderDtl',
            data: {
                ID: ID
            }
        });
    };

    obj.GetSizePermision = function () {
        return myApp.http({
            method: 'get',
            url: '/Inventory/GetSizePermision' 
            
        });
    };

    

    return obj;
};

var CartService = function () {
    var obj = {};

    obj.AddtoCart = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/AddtoCart',
            data: {
                filterText: data.join()
            }
        });
    };

    obj.GetCart = function (data) {
        return myApp.http({
            method: 'get',
            url: '/Inventory/GetCart'
        });
    };

    obj.RemoveCart = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/RemoveCart',
            data: {
                LotNos: data.join()
            }
        });
    };

    return obj;
};

var WatchListService = function () {
    var obj = {};

    obj.AddtoWatchList = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/AddtoWatchList',
            data: {
                filterText: data.join()
            }
        });
    };

    obj.GetWatchList = function (data) {
        return myApp.http({
            method: 'get',
            url: '/Inventory/GetWatchList'
        });
    };

    obj.RemoveWatchList = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/RemoveWatchList',
            data: {
                filterText: data.join()
            }
        });
    };

    return obj;
};

var OrderService = function () {
    var obj = {};

    obj.PreBookOrder = function (data, ShipID) {
        return myApp.http({
            method: 'get',
            url: '/Order/PreBookOrder?LotNos=' + data + '&ShippingMode=' + ShipID
        });
    };

    obj.BookOrder = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/Order/BookOrder',
            data: pData
        });
    };

    obj.Info = function (OrderID) {
        return myApp.http({
            method: 'post',
            url: '/Order/GetInfo',
            data: {
                OrderID: OrderID
            }
        });
    };

    obj.RemoveItemFromOrder = function (OrderID, LotNos) {
        return myApp.http({
            method: 'post',
            url: '/Order/RemoveItemsFormOrder',
            data: {
                OrderID: OrderID,
                LotNos: LotNos
            }
        });
    };

    obj.CancelOrder = function (OrderID) {
        return myApp.http({
            method: 'post',
            url: '/Order/CancelOrder',
            data: {
                OrderID: OrderID
            }
        });
    };

    obj.CompleteOrder = function (OrderID, ShippingCompany, TrackingNumber) {
        return myApp.http({
            method: 'post',
            url: '/Order/CompleteOrder',
            data: {
                OrderID: OrderID,
                ShippingCompany: ShippingCompany,
                TrackingNumber: TrackingNumber
            }
        });
    };

    obj.GetOrderItemsByOrderID = function (OrderIDs) {
        return myApp.http({
            method: 'post',
            url: '/Order/GetOrderItemsByOrderID',
            data: {
                OrderID: OrderIDs
            }
        });
    };

    obj.MergeOrder = function (OrderIDs) {
        return myApp.http({
            method: 'post',
            url: '/Order/MergeOrder',
            data: {
                MergeOrderList: OrderIDs.join(',')
            }
        });
    };

    obj.AllowEditCustomer = function (UserID) {
        return myApp.http({
            method: 'post',
            url: '/Order/AllowEditCustomer',
            data: {
                OrderLoginID: UserID
            }
        });
    };

    obj.GetOrderItemForExcel = function (OrderID) {
        return myApp.http({
            method: 'post',
            url: '/Order/GetOrderItemForExcel',
            data: {
                OrderID: OrderID
            }
        });
    };

    obj.GetMemoItemForExcel = function (OrderID, OrderStatus) {
        return myApp.http({
            method: 'post',
            url: '/Order/GetMemoItemForExcel',
            data: {
                OrderID: OrderID,
                OrderStatus: OrderStatus
            }
        });
    };

    obj.GetMultipleInfo = function (OrderIDs) {
        return myApp.http({
            method: 'post',
            url: '/Order/GetMultipleInfo',
            data: {
                OrderIDs: OrderIDs
            }
        });

    }

    return obj;
}

var AddressService = function (type) {
    var obj = {};

    obj.Get = function (data) {
        if (type == 'billing') {
            return myApp.http({
                method: 'get',
                url: '/Home/GetBillingAddresses'
            });
        } else {
            return myApp.http({
                method: 'get',
                url: '/Home/GetShippingAddresses'
            });
        }
    };

    obj.Add = function (data) {
        return myApp.http({
            method: 'post',
            url: '/Home/AddAddress',
            data: data
        });
    };

    obj.Delete = function (AddressID, Type) {
        return myApp.http({
            method: 'post',
            url: '/Home/DeleteAddress',
            data: {
                AddressID: AddressID,
                Type: Type
            }
        });
    };

    obj.GetByAddressID = function (AddressID, Type) {
        return myApp.http({
            method: 'get',
            url: '/Home/GetByAddressID?AddressID=' + AddressID + '&Type=' + Type
        });
    };

    obj.GetCountries = function () {
        return myApp.http({
            method: 'get',
            url: '/Home/GetCountries'
        });
    };

    obj.GetStates = function (CountryID) {
        return myApp.http({
            method: 'get',
            url: '/Home/GetState?CountryID=' + CountryID
        });
    };

    return obj;
};

var MemoService = function () {
    var obj = {};

    obj.Book = function (LotNos, CustomerID, isConfirmed, isSellDirect, Remark, FileNo) {
        return myApp.http({
            method: 'post',
            url: '/Memo/Book',
            data: {
                LotNos: LotNos.join(','),
                CustomerID: CustomerID,
                isConfirmed: isConfirmed,
                isSellDirect: isSellDirect,
                Remark: Remark,
                FileNo: (FileNo == undefined || FileNo == null) ? 0 : FileNo
            }
        });
    };

    obj.PartialCancel = function (OrderID, LotNos, FileNo) {
        return myApp.http({
            method: 'post',
            url: '/Memo/PartialCancel',
            data: {
                OrderID: OrderID,
                LotNos: LotNos.join(','),
                FileNo: (FileNo == undefined || FileNo == null) ? 0 : FileNo
            }
        });
    };

    obj.SplitMemo = function (OrderID, LotNos, CustomerID, isConfirmed, isSellDirect, Remark, FileNo) {
        return myApp.http({
            method: 'post',
            url: '/Memo/SplitMemo',
            data: {
                OrderID: OrderID,
                LotNos: LotNos.join(','),
                CustomerID: CustomerID,
                isConfirmed: isConfirmed,
                isSellDirect: isSellDirect,
                Remark: Remark,
                FileNo: (FileNo == undefined || FileNo == null) ? 0 : FileNo
            }
        });
    };

    obj.SellFullMemo = function (OrderID, MemoMode, salesAvgDiscount) {
        return myApp.http({
            method: 'post',
            url: '/Memo/SellFullMemo',
            data: {
                OrderID: OrderID,
                MemoMode: MemoMode,
                salesAvgDiscount: salesAvgDiscount
            }
        });
    };

    obj.CancelFullMemo = function (OrderID) {
        return myApp.http({
            method: 'post',
            url: '/Memo/CancelFullMemo',
            data: {
                OrderID: OrderID
            }
        });
    };

    obj.MergeMemo = function (CustomerID, isConfirmed, isSellDirect, Remark, MergeOrderList) {
        return myApp.http({
            method: 'post',
            url: '/Memo/MergeMemo',
            data: {
                CustomerID: CustomerID,
                isConfirmed: isConfirmed,
                isSellDirect: isSellDirect,
                Remark: Remark,
                MergeOrderList: MergeOrderList.join(',')
            }
        });
    };

    obj.UpdateMemo = function (OrderID, CustomerID, Remark) {
        return myApp.http({
            method: 'post',
            url: '/Memo/UpdateMemo',
            data: {
                OrderID: OrderID,
                CustomerID: CustomerID,
                Remark: Remark
            }
        });
    };

    obj.Return = function (OrderIDs) {
        return myApp.http({
            method: 'post',
            url: '/Memo/ReturnMemo',
            data: {
                OrderIDs: OrderIDs
            }
        });
    }

    obj.MemoTallyStockByRfid = function () {
        return myApp.http({
            method: 'post',
            url: '/Memo/MemoTallyStockByRfid',
            data: {
                OrderIDs: OrderIDs
            }
        });
    }

    obj.PartialSellMemo = function (OrderID, LotNos, MemoMode, salesAvgDiscount) {
        return myApp.http({
            method: 'post',
            url: '/Memo/PartialSellMemo',
            data: {
                OrderID: OrderID,
                LotNos: LotNos.join(','),
                MemoMode: MemoMode,
                salesAvgDiscount: salesAvgDiscount
            }
        });
    }

    obj.ReturnPartailMemo = function (LotNos) {
        return myApp.http({
            method: 'post',
            url: '/Memo/ReturnPartailMemo',
            data: {
                LotNos: LotNos.join(',')
            }
        });
    }

    obj.GetStockForPrintMemo = function (OrderID) {
        return myApp.http({
            method: 'post',
            url: '/Inventory/GetStockForPrintMemo',
            data: {
                OrderID: OrderID
            }
        });
    }

    


  
    return obj;
};

var DataTableColumnStruct = function (mode) {
    var SpecificSearch = {};
    var RoleID = $('#hfRoleID').val();

    SpecificSearch.columns = [
        { data: "inventoryID" },
        { data: "Stock" }, 
        { data: "CertificateNo" },
        { data: "v360url" },
        { data: "Shape" },
        { data: "Weight" },
        { data: "Color" },
        { data: "Clarity" },
        { data: "Measurement" },
        { data: "TablePerc" },
        { data: "DepthPerc" },
        { data: "Girdle" },
        { data: "Cut" },
        { data: "Polish" },
        { data: "Symmetry" },
        { data: "Fluorescence" },
        { data: "Certificate" },
        { data: "RapnetPrice" },
        { data: "Discount" },
        { data: "Price" },
        { data: "Amount" },
        { data: "D_R" },
        { data: "CrownHeight" },
        { data: "CrownAngle" },
        { data: "PavilionDepth" },
        { data: "PavilionAngle" },
        { data: "StarLength" },
        { data: "LowerHalf" },
        { data: "GirdlePerc" },
        { data: "Laserinscribe" },
        { data: "EyeClean" },
        { data: "Shade" },
        { data: "Milky" },
        { data: "TableBlack" },
        { data: "SideBlack" },
        { data: "HeartAndArrow" },
        { data: "OpensName" },
        { data: "Keytosymbol" },
        { data: "giaComments", class: 'whspace' },
        { data: "Reportdate" },
        { data: "SalesLocation" },
       { data: "refdata" }, /*Added by Ankit 24JUn2020*/
        { data: "refdata" }, /*Added by Ankit 24JUn2020*/
        { data: "Origin" }, /*Added by Ankit 20July2020*/
    ];

    SpecificSearch.columnDefs = [
       
        {
            targets: 'dt-checkbox',
            render: function (data, type, row, ele) {
                //BGN
                if (mode == "SpecificSearch") {
                    return '<label class="checkbox"><input type="checkbox" value="' + row.Stock + '" ' + (row.stockStatusID == 23 ? 'disabled' : '') + '></label>';
                } else {
                    return '<label class="checkbox"><input type="checkbox" value="' + row.Stock + '"></label>';
                }

            },
            orderable: false
        },
        {
            targets: 'stock',
            render: function (data, type, row, ele) {
                return '<a target="_blank" href="/DiamondSearch/DiamondView?id=' + row.inventoryID + '">' + row.Stock + '<a>'
            },
            orderable: false
        },
        //Added by Ankit 24Jun2020
       
        {
           
        
            targets: 'Status', 
            render: function (data, type, row, ele) {
                if (row.refdata != "Available") {
                    return "On Memo";   
                } else {
                    return row.refdata;
                }
               
               
            },
            orderable: false 
            
        },
        {


            targets: 'CompanyName',
            render: function (data, type, row, ele) {
                if (row.refdata != "Available") {
                    //// return '<a target="_blank" href="/Inventory/OrderInfoDetails?inventoryID=' + row.inventoryID + '">' + row.refdata + '<a>'
                    // return '<a class="ancMemoItems" data-id="' + row.inventoryID + '"  href="#" >' + row.refdata  + '</a>';
                   // return '<a   href="/Inventory/MemoInfoDetails?inventoryID=' + row.inventoryID + '">' + row.refdata + '<a>'
                     
                    return '<a   href="/Memo/Filter/' + row.inventoryID + '">' + row.refdata + '<a>'
                    //return '<a class="loadData123" data-Criteria="' + row.inventoryID + '" href="#">' + row.refdata + +'</a>';


                } else {
                    return "";  
                }


            },
            orderable: false

        },
     
        {
            targets: 'dt-V360',
            render: function (data, type, row) {
                if (data != null && data != '' && data != undefined) {
                    return '<a href="' + data + '" target="_blank"><i class="fa fa-video-camera" aria-hidden="true"></i></a>';
                }
                return "";
            },
            orderable: false
        },
        {
            targets: 'lab',
            render: function (data, type, row) {
                var link = "";
                if (row.Certificate == "GIA" || row.Certificate == "EGL" || row.Certificate == "IGI") {
                    switch (row.Certificate) {
                        case "GIA":
                            link = "http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + row.CertificateNo;
                            break;
                        case "EGL":
                            link = "http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + row.CertificateNo;
                            break;
                        case "IGI":
                            link = "http://www.igiworldwide.com/verify.php?r=" + row.CertificateNo;
                            break;
                        default:
                            link = "#" + row.CertificateNo;
                            break;
                    }
                }
                return '<a href="' + link + '" target="_blank">' + row.Certificate + '</a>';
            },
            orderable: false
        },
        {
            targets: 'ha',
            render: function (data, type, row) {
                if (data == "HA-VG") {
                    return '<img src="/Content/img/HA_icon.png" title="' + data + '" />';
                }
                else if (data == "HA-EX") {
                    return '<img src="/Content/img/HA_ex.png" title="' + data + '" />';
                }
                else if (data == "NO") {
                    return '';
                }
                return data;
            },
            orderable: false
        },
        {
            targets: 'laser',
            render: function (data, type, row) {
                if (data == null) {
                    return 'No';
                }
                return 'Yes';
            },
            orderable: false
        },
        
        { className: "dt_col_hide", "targets": (RoleID == "3") ? [42] : null },
           //{ className: "dt_col_hide", "targets": [2] },
    ];

    SpecificSearch.printColumns = [
        { data: "RowNum" },
        { data: "Stock" },
        { data: "Shape" },
        { data: "Weight" },
        { data: "Sizes" },
        { data: "Color" },
        { data: "Clarity" },
        { data: "Length" },
        { data: "Width" },
        { data: "Depth" },
        { data: "HeartAndArrow" },
        { data: "Cut" },
        { data: "Polish" },
        { data: "Symmetry" },
        { data: "Fluorescence" },
        { data: "Lab" },
        { data: "RapnetPrice" },
        { data: "RapAmount" },
        { data: "Discount" },
        { data: "Price" },
        { data: "CertificateNo" },
        { data: "DepthPerc" },
        { data: "TablePerc" },
        { data: "Girdle" },
        { data: "CrownAngle" },
        { data: "CrownHeight" },
        { data: "PavilionAngle" },
        { data: "PavilionDepth" },
        { data: "StarLength" },
        { data: "LowerHalf" },
        { data: "GirdlePerc" },
        { data: "Keytosymbol" }
    ];

    return {
        SpecificSearch: SpecificSearch
    }
};

var DashboardService = function () {
    var obj = {};

    obj.GetDashboard = function () {
        return myApp.http({
            method: 'get',
            url: '/Dashboard/GetDashboard'
        });
    };

    obj.RemoveRecent = function (recentSearchID) {
        return myApp.http({
            method: 'post',
            url: '/Dashboard/RemoveRecent',
            data: {
                recentSearchID: recentSearchID
            }
        });
    };

    obj.Count = function () {
        return myApp.http({
            method: 'get',
            url: '/Dashboard/Count'
        });
    };

    obj.GetStockSummary = function (Stone, LocationID) {
        return myApp.http({
            method: 'post',
            url: '/Dashboard/GetStockSummary',
            data: {
                Stone: Stone,
                LocationID: LocationID
            }
        });
    };

    //added by ankit 11July2020

    obj.GetStoneDetailsStockSummary = function (StartRange, EndRange, Shape, Location, Mode, Event) {
        return myApp.http({
            method: 'post',
            url: '/Dashboard/GetStoneDetailsStockSummary',
            data: {
                
                StartRange: StartRange,
                EndRange: EndRange,
                Shape: Shape,
                Location: Location,
                Mode: Mode,
                Event: Event
            }
        });
    };

    obj.GetSalesLocation = function (Stone) {
        return myApp.http({
            method: 'get',
            url: '/Inventory/GetSalesLocation'
        });
    };

    obj.GetRecentSearchView = function (UserID) {
        return myApp.http({
            method: 'get',
            url: '/Dashboard/GetRecentSearchView?UserID=' + UserID
        });
    }

    obj.GetCustomerLogDetails = function (UserID) {
        return myApp.http({
            method: 'get',
            url: '/Dashboard/GetCustomerLogDetails?UserID=' + UserID
        });
    }

    return obj;
}

var MarketingService = function () {
    var obj = {};

    obj.CreateBlueNile = function (obj) {
        return myApp.http({
            method: 'post',
            url: '/Marketing/CreateBlueNile',
            data: obj
        });
    };

    obj.CreateJamesAllen = function (obj) {
        return myApp.http({
            method: 'post',
            url: '/Marketing/CreateJamesAllen',
            data: obj
        });
    };

    obj.CreateJamesAllenHK = function (obj) {
        return myApp.http({
            method: 'post',
            url: '/Marketing/CreateJamesAllenHK',
            data: obj
        });
    };

    return obj;
}

var ChargesService = function () {
    var obj = {};

    obj.Update = function (obj) {
        return myApp.http({
            method: 'post',
            url: '/Charges/Update',
            data: {
                obj: obj
            }
        });
    };

    return obj;
}

var RFIDService = function () {

    var obj = {};

    obj.AddRFIDViaExcel = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/RFID/AddNewRfId',
            data: pData,
            processData: false,
            contentType: false
        });
    }

    obj.RecycleRFIds = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/RFID/RecycleRFId',
            data: {
                'Rfid': pData
            }
        });
    }

    obj.UpdateRfId = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/RFID/UpdateRfId',
            data: pData,
            processData: false,
            contentType: false
        });
    }

    obj.UpdateRFIDStatus = function (rfid, Status) {
        return myApp.http({
            method: 'post',
            url: '/RFID/InventoryUpdateRFIDStatus',
            data: {
                'rfid': rfid,
                'Status': Status
            }
        });
    }

    obj.TallyStockByRFID = function (RFIDs) {
        return myApp.http({
            method: 'post',
            url: '/RFID/TallyStockByRFID',
            data: {
                'RFIDs': RFIDs
            }
        });
    }

    obj.TallyMemoByRFID = function (OrderID, RFIDs) {
        return myApp.http({
            method: 'post',
            url: '/RFID/TallyMemoByRfid',
            data: {
                'OrderID': parseInt(OrderID),
                'RFIDs': RFIDs
            }
        });
    }

    return obj;
}

var DownloadRightsService = function () {

    var obj = {};

    obj.GetRightForUser = function (ID) {
        return myApp.http({
            method: 'get',
            url: '/User/GetRightForUser?LoginID=' + ID
        });
    }

    obj.PutRightForUser = function (pData) {
        return myApp.http({
            method: 'post',
            url: '/User/PutRightForUser',
            data: {
                objLst: pData
            }
        });
    }

    return obj;
}

