/// <reference path="jquery-1.8.2.min.js" />
(function ($) {

    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    Date.prototype.format = function (fmt) {
        var o = {
            "M+": this.getMonth() + 1,                 //月份 
            "d+": this.getDate(),                    //日 
            "h+": this.getHours(),                   //小时 
            "m+": this.getMinutes(),                 //分 
            "s+": this.getSeconds(),                 //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds()             //毫秒 
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }

    $.fn.serializeJson = function () {
        var serializeObj = {};
        var array = this.serializeArray();
        var str = this.serialize();
        $(array).each(function () {
            if (serializeObj[this.name]) {
                if ($.isArray(serializeObj[this.name])) {
                    serializeObj[this.name].push(this.value);
                } else {
                    serializeObj[this.name] = [serializeObj[this.name], this.value];
                }
            } else {
                serializeObj[this.name] = this.value;
            }
        });
        return serializeObj;
    };

    Array.prototype.where = function (filter) {

        var collection = this;

        switch (typeof filter) {

            case 'function':
                return $.grep(collection, filter);

            case 'object':
                for (var property in filter) {
                    if (!filter.hasOwnProperty(property))
                        continue; // ignore inherited properties

                    collection = $.grep(collection, function (item) {
                        return item[property] === filter[property];
                    });
                }
                return collection.slice(0); // copy the array 
                // (in case of empty object filter)

            default:
                throw new TypeError('func must be either a' +
                    'function or an object of properties and values to filter by');
        }
    };


    Array.prototype.firstOrDefault = function (func) {
        return this.where(func)[0] || null;
    };

})(jQuery);
$.fn.extend({ 
    doAjax: function (url) {

        var me = this;
        if (me.validate() == false) {
            return false;
        }
        var ajaxType = typeof (this.reqType) === "undefined" ? "GET" : this.reqType.toLocaleUpperCase();
        var actualValue = me.pageValues;
        if (ajaxType !== "GET") {
            if (typeof (actualValue) == "string" && actualValue.lastIndexOf("=") == actualValue.indexOf("=")) {
                actualValue = actualValue.substring(actualValue.indexOf("="));
            }
            if (typeof (actualValue) === "object") {
                var ps = "{";
                for (var p in actualValue) {
                    ps += p += ":\"" + actualValue[p] + "\",";
                }
                actualValue = (ps.length > 0 ? ps.substr(0, ps.length - 1) : "") + "}";
            }
            if (typeof (this.jsonValues) !== "undefined") {
                actualValue = this.jsonValues;
            }
        }
        $.ajax({
            cache: false,
            headers: {
                "Content-Type": typeof (this.requestHeaderType) === "undefined" ? "application/x-www-form-urlencoded" : this.requestHeaderType
            },
            type: ajaxType,
            url: $.rootURL + url + (ajaxType === "GET" ? "?" + actualValue : ""),
            data: ajaxType === "GET" ? "{}" : actualValue,
            dataType: 'json',
            success: function (result) {
                if (me.callBackFunction) {
                    me.callBackFunction(result);
                }
            },
            statusCode: {
                401: function () {
                    window.location = "/";
                }
            },
            error: function (result, status) {
                switch (status) {
                    case "error":
                        if (result.status === 401) {
                            $.messager.alert('登录到期', $.parseJSON(result.responseText).Message, 'info', function () { window.location = "/"; });
                        }
                        else {
                            if (typeof (result) === "string") {
                                $.messager.alert('错误提示', result, 'error');
                            }
                            else {
                                $.messager.alert('错误提示', result.responseText, 'error');
                            }
                        }
                        break;
                    default:
                        $.messager.alert('错误提示', result.responseText, 'error');
                        break;
                }
            }
        });
        return this;
    },
    validate: function () {
        if (this.is("form") === true) {
            return this.form('validate');
        }
        else {
            var isValid = true;
            $(":input", this).each(function () {
                var input = $(this);
                if (input.attr("required") == null && input.attr("data-options") == null) {
                    return;
                }
                if (!input.validatebox("isValid")) {
                    isValid = false;
                    input.focus();
                }
            });
            return isValid;
        }
    },
    setCurrentDate: function () {
        var today = new Date();
        var year = today.getFullYear();
        var month = today.getMonth() + 1;
        if (month < 10) { month = "0" + month.toString(); }
        var date = today.getDate();
        if (date < 10) { date = "0" + date.toString(); }
        this.datebox('setValue', year + "-" + month + "-" + date);
    },
    initRight: function () {
        var ps = this.attr("class");
        if (ps) {
            if (ps.indexOf("easyui-window") >= 0) {
                $.pageFunction.setUserPageFunctions(this);
            }
        }
        else {
            $.pageFunction.setUserPageFunctions(this.parent().parent());
        }
    },
    ajaxLoading: function () {
        var w = this.parent().outerWidth(true);
        var h = this.parent().outerHeight(true);
        this.append("<div class=\"datagrid-mask-side\" style=\"opacity:0.4;position:absolute;top:0px;left:0px;width:" + w + "px;height:" + h + "px;background-color:#cccccc\"></div>");
        this.append("<div class=\"datagrid-mask-msg datagrid-mask-side\" style=\"display: block; left: " + (w / 2 - 90) + "px; top: " + (h / 2) + "px;\">正在处理，请稍候。。。</div>");
    },
    ajaxLoadEnd: function () {
        this.parent().find(".datagrid-mask-side").remove();
    },
    rowspan: function (colIdx) {
        return this.each(function () {
            var that;
            $('tr', this).each(function (row) {
                $('td:eq(' + colIdx + ')', this).filter(':visible').each(function (col) {
                    if (that != null && $(this).html() == $(that).html()) {
                        rowspan = $(that).attr("rowSpan");
                        if (rowspan == undefined) {
                            $(that).attr("rowSpan", 1);
                            rowspan = $(that).attr("rowSpan");
                        }
                        rowspan = Number(rowspan) + 1;
                        $(that).attr("rowSpan", rowspan);
                        $(this).hide();
                    } else {
                        that = this;
                    }
                });
            });
        });
    }
});
//-----------------------------------------------------
//extend the static method for the ajax request.
//-----------------------------------------------------
(function ($) {
    $.extend({
        showError: function (msg) {
            $.messager.alert('错误提示', msg, 'error');
        },
        showInfo: function (msg) {
            $.messager.alert('信息提示', msg, 'info');
        },
        getServerView: function (method, parameter, callbackFun) {
            $.ajax({
                cache: false,
                type: "Get",
                contentType: "application/json; charset=utf-8",
                url: $.rootURL + method,
                data: parameter == null ? "{}" : parameter,
                dataType: 'html',
                success: function (result) {
                    if (callbackFun) {
                        callbackFun(result);
                    }
                },
                error: function (result, status) {
                    if (status === 'error') {
                        if (typeof (result) === "string") {
                            $.messager.alert('错误提示', result, 'error');
                        }
                        else {
                            $.messager.alert('错误提示', result.responseText, 'error');
                        }
                    }
                }
            });
        },
        getServerJsonNoAsync: function (method, parameter, callbackFun, errorcallBackFun) {
            $.ajax({
                cache: false,
                type: "Get",
                async: false,
                contentType: "application/json; charset=utf-8",
                url: $.rootURL + method,
                data: parameter == null ? "{}" : parameter,
                dataType: 'json',
                success: function (result) {
                    if (callbackFun) {
                        callbackFun(result);
                    }
                },
                error: function (result, status) {
                    if (errorcallBackFun) {
                        errorcallBackFun(result, status);
                    }
                    else {
                        switch (status) {
                            case "error":
                                if (result.status === 401) {
                                    $.messager.alert('登录到期', $.parseJSON(result.responseText).Message, 'info', function () { window.location = "/"; });
                                }
                                else {
                                    alert(result.responseText);
                                    if (typeof (result) === "string") {
                                        $.messager.alert('错误提示', result + " 错误代码：" + result.status.toString(), 'error');
                                    }
                                    else if (result.responseText != null && typeof (result.responseText) === "string") {
                                        $.messager.alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                    }
                                    else {
                                        var ps = $.parseJSON(result.responseText);
                                        if (ps != null && ps.ExceptionMessage != null) {
                                            $.messager.alert('错误提示', ps.ExceptionMessage + " 错误代码：" + result.status.toString(), 'error');
                                        }
                                        else {
                                            $.messager.alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                        }
                                    }
                                }
                                break;
                            default:
                                $.messager.alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                break;
                        }
                    }
                }
            });
        },
        getServerJson: function (method, parameter, callbackFun, errorcallBackFun) {
            $.ajax({
                cache: false,
                type: "Get",
                contentType: "application/json; charset=utf-8",
                url: $.rootURL + method,
                data: parameter == null ? "{}" : parameter,
                dataType: 'json',
                success: function (result) {
                    if (callbackFun) {
                        callbackFun(result);
                    }
                },
                error: function (result, status) {
                    if (errorcallBackFun) {
                        errorcallBackFun(result, status);
                    }
                    else {
                        switch (status) {
                            case "error":
                                if (result.status === 401) {
                                    $.messager.alert('登录到期', $.parseJSON(result.responseText).Message, 'info', function () { window.location = "/"; });
                                }
                                else {
                                    if (typeof (result) === "string") {
                                        $.messager.alert('错误提示', result + " 错误代码：" + result.status.toString(), 'error');
                                    }
                                    else if (result.responseText != null && typeof (result.responseText) === "string") {
                                        $.messager.alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                    }
                                    else {
                                        var ps = $.parseJSON(result.responseText);
                                        if (ps != null && ps.ExceptionMessage != null) {
                                            $.messager.alert('错误提示', ps.ExceptionMessage + " 错误代码：" + result.status.toString(), 'error');
                                        }
                                        else {
                                            $.messager.alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                        }
                                    }
                                }
                                break;
                            default:
                                $.messager.alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                break;
                        }
                    }
                }
            });
        },
        postJsontoServer: function (method, parameter, callbackFun, errorcallBackFun) {
            $.ajax({
                cache: false,
                type: "Post",
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                url: $.rootURL + method,
                data: parameter == null ? "{}" : parameter,
                dataType: 'json',
                success: function (result) {
                    if (callbackFun) {
                        callbackFun(result);
                    }
                },
                error: function (result, status) {
                    if (errorcallBackFun) {
                        errorcallBackFun(result, status);
                    }
                    else {
                        switch (status) {
                            case "error":
                                if (result.status === 401) {
                                    alert('登录到期', $.parseJSON(result.responseText).Message, 'info', function () { window.location = "/"; });
                                }
                                else {
                                    if (typeof (result) === "string") {
                                        alert('错误提示', result + " 错误代码：" + result.status.toString(), 'error');
                                    }
                                    else if (result.responseText != null && typeof (result.responseText) === "string") {
                                        alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                    }
                                    else {
                                        var ps = $.parseJSON(result.responseText);
                                        if (ps != null && ps.ExceptionMessage != null) {
                                            alert('错误提示', ps.ExceptionMessage + " 错误代码：" + result.status.toString(), 'error');
                                        }
                                        else {
                                            alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                        }
                                    }
                                }
                                break;
                            default:
                                alert('错误提示', result.responseText + " 错误代码：" + result.status.toString(), 'error');
                                break;
                        }
                    }
                }
            });
        },
        getQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
        rootURL: "/",
        pageSize: 30,
        pageList: [10, 30, 60],
        changeDateFormat: function (jsondate) {
            jsondate = jsondate.replace("/Date(", "").replace(")/", "");
            if (jsondate.indexOf("+") > 0) {
                jsondate = jsondate.substring(0, jsondate.indexOf("+"));
            }
            else if (jsondate.indexOf("-") > 0) {
                jsondate = jsondate.substring(0, jsondate.indexOf("-"));
            }
            var date = new Date(parseInt(jsondate, 10));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            return date.getFullYear() + "-" + month + "-" + currentDate;
        },
        setSysTabsOption: function (res) {
            $("#sysTabCount").val(res);
        },
        //清空指定标签中控件的值
        ResetValue: function (cid) {
            var everyInput = $('#' + cid).find("input[type=text][class!='combo-text validatebox-text'],input[type=radio][checked=true],input[type=checkbox][checked=true],input[type=hidden][class!='combo-value'],select,textarea");
            everyInput.val('');
        },
        /**
         * 根据长度截取先使用字符串，超长部分追加...
         * @param str 对象字符串
         * @param len 目标字节长度
         * @return 处理结果字符串
        */
        cutString: function (str, len) {
            //length属性读出来的汉字长度为1
            if (str.length * 2 <= len) {
                return str;
            }
            var strlen = 0;
            var s = "";
            for (var i = 0; i < str.length; i++) {
                s = s + str.charAt(i);
                if (str.charCodeAt(i) > 128) {
                    strlen = strlen + 2;
                    if (strlen > len) {
                        return s.substring(0, s.length - 1) + "...";
                    }
                } else {
                    strlen = strlen + 1;
                    if (strlen > len) {
                        return s.substring(0, s.length - 1) + "...";
                    }
                }
            }
            return s;
        },
        getExportFile: function (url, options) {
            if ($('#downloadcsv').length == 0) {
                $('body').append("<iframe id=\"downloadcsv\" style=\"display:none\"></iframe>");
            }
            var ps = "";
            if (typeof options === "object") {
                var ps = "?";
                for (var p in options) {
                    ps += p + '=' + options[p].toString() + "&";
                }
                var now = new Date();
                ps += "_=" + now.toString();
            }
            $('#downloadcsv').attr('src', url + ps);
        },
        showWindowMessage: function (result, status) {
            if (result.status && result.status == "401") {
                alert("登录超时，请重新登录。");
                window.close();
            }
            else if (result.responseText) {
                if (result.responseText.ExceptionMessage) {
                    alert(result.responseText.ExceptionMessage);
                }
                else {
                    alert(result.responseText);
                }
            }
        }
    }); //end of function extend.
})(jQuery);

(function ($) {
    $.HR = {
     
        ShowLoading: function () {
            debugger;
            $('#showloadmain').css({ height: $(window).height(), width: $(window).width() }).show();
            $('#showloading').css({
                top: ($(window).height() - 60) * 0.5,
                left: ($(window).width() - 185) * 0.5
            }).show();
            $('#showloadmain').click(function () {
                $('#showloading,#showloadmain').hide();
            });
        },
        CloseLoading: function () {
            $('#showloading,#showloadmain').hide();
        },
        getParameter: function (paraStr, url) {
            var result = "";
            //获取URL中全部参数列表数据    
            var str = "&" + url.split("?")[1];
            var paraName = paraStr + "=";
            //判断要获取的参数是否存在    
            if (str.indexOf("&" + paraName) != -1) {
                //如果要获取的参数到结尾是否还包含“&”        
                if (str.substring(str.indexOf(paraName), str.length).indexOf("&") != -1) {
                    //得到要获取的参数到结尾的字符串            
                    var TmpStr = str.substring(str.indexOf(paraName) + paraName.length, str.length);
                    //截取从参数开始到最近的“&”出现位置间的字符            
                    result = TmpStr.substr(TmpStr.indexOf(paraName) + paraName.length, TmpStr.indexOf("&") - TmpStr.indexOf(paraName));
                }
                else {
                    result = str.substring(str.indexOf(paraName) + paraName.length, str.length);
                }
            }
            else {
                result = "";
            }
            return (result.replace("&", ""));
        }
    };
    window.onresize = function () {
        setTimeout($.HR.doResize, 300);
    };

})(jQuery);




