var uiApp = function () {

    function getUniqueID(prefix) {
        return 'prefix_' + Math.floor(Math.random() * (new Date()).getTime());
    }

    function scrollTo(el, offeset) {
        var pos = (el && el.length > 0) ? el.offset().top : 0;

        if (el) {
            if ($('body').hasClass('page-header-fixed')) {
                pos = pos - $('.page-header').height();
            } else if ($('body').hasClass('page-header-top-fixed')) {
                pos = pos - $('.page-header-top').height();
            } else if ($('body').hasClass('page-header-menu-fixed')) {
                pos = pos - $('.page-header-menu').height();
            }
            pos = pos + (offeset ? offeset : -1 * el.height());
        }

        $('html,body').animate({
            scrollTop: pos
        }, 'slow');
    }

    function Alert(options) {
        options = $.extend(true, {
            container: "", // alerts parent container(by default placed after the page breadcrumbs)
            place: "append", // "append" or "prepend" in container 
            type: 'success', // alert's type
            message: "", // alert's message
            close: true, // make alert closable
            reset: true, // close all previouse alerts first
            focus: true, // auto scroll to the alert after shown
            closeInSeconds: 5, // auto close after defined seconds
            icon: "" // put icon before the message
        }, options);

        var id = getUniqueID("App_alert");

        var html = '<div id="' + id + '" class="custom-alerts alert alert-' + options.type + ' fade in">' + (options.close ? '<button type="button" class="close" data-dismiss="alert" aria-hidden="true"><i class="fa fa-close" aria-hidden="true"></i></button>' : '') + (options.icon !== "" ? '<i class="fa-lg fa fa-' + options.icon + '"></i>  ' : '') + options.message + '</div>';

        if (options.reset) {
            $('.custom-alerts').remove();
        }

        if (!options.container) {
            if ($('.page-fixed-main-content').size() === 1) {
                $('.page-fixed-main-content').prepend(html);
            } else if (($('body').hasClass("page-container-bg-solid") || $('body').hasClass("page-content-white")) && $('.page-head').size() === 0) {
                $('.page-title').after(html);
            } else {
                if ($('.page-bar').size() > 0) {
                    $('.page-bar').after(html);
                } else {
                    $('.page-breadcrumb, .breadcrumbs').after(html);
                }
            }
        } else {
            if (options.place === "append") {
                $(options.container).append(html);
            } else {
                $(options.container).prepend(html);
            }
        }

        if (options.focus) {
            scrollTo($('#' + id), -70);
        }

        if (options.closeInSeconds > 0) {
            setTimeout(function () {
                $('#' + id).remove();
            }, options.closeInSeconds * 1000);
        }

        return id;
    }

    function BlockUI(options) {
        options = $.extend(true, {
            container: "", // block element
            message: "<div class='loader'></div>" // blocking message
        }, options);

        if (options.container !== "" && options.container !== undefined && options.container !== null) {
            $(options.container).block({
                message: options.message
            });
        } else {
            $.blockUI({
                message: options.message
            });
        }
    }

    function UnBlockUI(container) {
        if (container !== "" && container !== undefined && container !== null) {
            $(container).unblock();
        } else {
            $.unblockUI();
        }
    }

    function Confirm(Message, Callback) {
        return bootbox.confirm(Message, function (result) {
            Callback(result);
        });
    }

    function AlertPopup(Message, Callback) {
        return bootbox.alert(Message, function (result) {
            Callback(result);
        });
    }

    function Show(el) {
        //el.show();
        //el.animate({ opacity: "1" }, 'slow', 'linear', function () {
        //    callBack();
        //});
        $(el).css({
            'opacity': 0,
            'width': 0
        });
        $(el).show();
        $(el).animate({ opacity: "1", width: "100%" }, 'slow', 'linear');
    }

    function Hide(el, callBack) {
        el.hide(1000, 'linear', function () {
            callBack();
        });
        el.css({
            'opacity': 0
        });
    }

    function CompareInventory() {

    }

    function BindPrintTable(data) {
        $('#tblPrint').html('');
        var tmpPrintRows = $('#tmpPrintHeaderRow').html();
        $('#tmpPrintRows').tmpl([data[0]]).appendTo('#tblPrint');
        $(tmpPrintRows).appendTo('#tblPrint');
        data.splice(0, 1);
        $('#tmpPrintRows').tmpl(data).appendTo('#tblPrint');

        var innerHtmlHeader = $('#tblPrint').html();

        var newWin = window.open('', 'Print-Window');
        newWin.document.open();
        newWin.document.write('<html><style>table {border-collapse: collapse;} td,th { border:1px solid #000; } th { background-color:#fff700 } </style> <body style="overflow:scroll" onload="window.print()"><table>' + innerHtmlHeader + '</table>\
                                <script>!function(){var e=document.getElementsByTagName("input");console.log(e.length);for(var n=0;n<e.length;n++)e[n].remove(),n--}();</script>\
                               </body></html>');
        newWin.document.close();
    }

    return {
        Alert: Alert,
        AlertPopup: AlertPopup,
        scrollTo: scrollTo,
        BlockUI: BlockUI,
        UnBlockUI: UnBlockUI,
        Confirm: Confirm,
        getUniqueID: getUniqueID,
        Show: Show,
        Hide: Hide,
        BindPrintTable: BindPrintTable
    };

}();
