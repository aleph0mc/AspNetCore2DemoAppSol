/// <reference path="References/jquery/jquery.d.ts"/>
var AppStart;
(function (AppStart) {
    var Utils = /** @class */ (function () {
        function Utils() {
        }
        Utils.Init = function () {
            //Utils.cnt = Math.floor((Math.random() * 100) + 1);
            console.log('App started...');
        };
        Utils.myHistory = [];
        Utils.cnt = 0;
        return Utils;
    }());
    AppStart.Utils = Utils;
    // OnLoad event - invoke init.
    window.onload = function (evt) {
        AppStart.Utils.Init();
        //for (var i = 0; i < 20; i++) {
        //    Utils.cnt = Math.floor((Math.random() * 100) + 1);
        //    AppStart.Utils.myHistory.push({ container: "all" + AppStart.Utils.cnt, action: "controller/action" + AppStart.Utils.cnt });
        //    window.history.pushState({ container: "all" + AppStart.Utils.cnt, action: "controller/action" + AppStart.Utils.cnt }, "");
        //}
    };
    //window.onpopstate = (evt) => {
    //    alert('pop');
    //    var x: any;
    //    while ((x = AppStart.Utils.myHistory.pop()) != null) {
    //        console.log(x.container + ' - ' + x.action);
    //    }
    //}
})(AppStart || (AppStart = {}));
//# sourceMappingURL=app_start.js.map