/// <reference path="References/jquery/jquery.d.ts"/>
/// <reference path="References/kendo-ui/kendo-ui.d.ts"/>

module AppStart {

    export class Utils {
        public static Init() {
            console.log('App started... MLK');
        }

        public static SaveSucceeded(msg) {

            if ('' == msg)
                return;

            var $divMsg = $("#divMsg");
            if ($divMsg.is(":visible")) { return; }
            $divMsg.slideDown(1000);
            setTimeout(function () {
                $divMsg.slideUp(1000);
            }, 5000);

        }

        public static LoadPageAsync() {
            AjaxHelper.Utils.loadContent('#PersonFormDiv', '/Home/PersonInfo');
        }

        public static ShowMessageTest() {
            console.log('Welcome in this test website!');
        }
    }

    window.onload = (evt) => {
        //AppStart.Utils.Init();
    }
}
