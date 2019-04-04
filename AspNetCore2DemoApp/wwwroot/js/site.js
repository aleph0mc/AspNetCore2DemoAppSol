var AjaxHelper;
(function (AjaxHelper) {
    var Utils = (function () {
        function Utils() {
        }
        Utils.performLoadContent = function (containerSelector, action, withResize, callback) {
            if (withResize === void 0) { withResize = false; }
            if (callback === void 0) { callback = null; }
            var request = $.ajax({
                type: "GET",
                url: action,
                success: function (response, textStatus, xhr) {
                    if (status == 'error') {
                        var msg = 'Sorry but there was an error: ';
                        $(containerSelector).html(msg + xhr.status + ' ' + xhr.statusText);
                    }
                    else {
                        $(containerSelector).html(response);
                        if (null != callback)
                            callback();
                    }
                },
                error: function (xhr, status, error) {
                    var resptxt = xhr.responseText;
                    console.log('responseText: ' + resptxt);
                }
            });
            Utils.requestList.push(request);
        };
        Utils.loadContent = function (containerSelector, action, withResponse, withResize, withClean, callback) {
            if (withResponse === void 0) { withResponse = false; }
            if (withResize === void 0) { withResize = false; }
            if (withClean === void 0) { withClean = false; }
            if (callback === void 0) { callback = null; }
            Utils.performLoadContent(containerSelector, action, withResize, callback);
        };
        Utils.loadContentHistory = function (containerSelector, action, withResponse, withResize, withClean, callback) {
            if (withResponse === void 0) { withResponse = false; }
            if (withResize === void 0) { withResize = false; }
            if (withClean === void 0) { withClean = false; }
            if (callback === void 0) { callback = null; }
            Utils.performLoadContent(containerSelector, action, withResize, callback);
        };
        Utils.cancelRequests = function () {
            while (Utils.requestList.length) {
                var currReq = Utils.requestList.pop();
                currReq.abort();
            }
        };
        Utils.requestList = [];
        return Utils;
    }());
    AjaxHelper.Utils = Utils;
})(AjaxHelper || (AjaxHelper = {}));
var AppStart;
(function (AppStart) {
    var Utils = (function () {
        function Utils() {
        }
        Utils.Init = function () {
            console.log('App started... MLK');
        };
        Utils.SaveSucceeded = function (msg) {
            if ('' == msg)
                return;
            var $divMsg = $("#divMsg");
            if ($divMsg.is(":visible")) {
                return;
            }
            $divMsg.slideDown(1000);
            setTimeout(function () {
                $divMsg.slideUp(1000);
            }, 5000);
        };
        Utils.LoadPageAsync = function () {
            AjaxHelper.Utils.loadContent('#PersonFormDiv', '/Home/PersonInfo');
        };
        Utils.ShowMessageTest = function () {
            console.log('Welcome in this test website!');
        };
        return Utils;
    }());
    AppStart.Utils = Utils;
    window.onload = function (evt) {
    };
})(AppStart || (AppStart = {}));
//# sourceMappingURL=site.js.map