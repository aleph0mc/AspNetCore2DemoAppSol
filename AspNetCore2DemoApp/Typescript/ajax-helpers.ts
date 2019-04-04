module AjaxHelper {
    export class Utils {
        private static requestList = [];


        private static performLoadContent(containerSelector: string, action: string, withResize: boolean = false, callback: any = null) {
            var request = $.ajax({
                type: "GET",
                url: action,
                success: function (response, textStatus, xhr) {
                    if (status == 'error') {
                        var msg = 'Sorry but there was an error: ';
                        $(containerSelector).html(msg + xhr.status + ' ' + xhr.statusText);
                    } else {
                        $(containerSelector).html(response);

                        if (null != callback)
                            callback();
                    }
                },
                error: function (xhr, status, error) {
                    //console.log('error: ' + error);
                    //console.log('status: ' + status);
                    var resptxt: any = xhr.responseText;
                    console.log('responseText: ' + resptxt);
                    //// In case of session timeout redirect to login view
                    //if (resptxt.includes('SessionTimeout'))
                    //    location.href = 'Account/Login';
                }
            });


            Utils.requestList.push(request);
        }

        public static loadContent(containerSelector: string, action: string, withResponse: boolean = false, withResize: boolean = false, withClean: boolean = false, callback: any = null) {
            // save data for histoty
            //window.history.pushState(-1, null);
            //siteHistory.push({ container: containerSelector, url: action, resize: withResize, clean: withClean, callback: callback });

            Utils.performLoadContent(containerSelector, action, withResize, callback);
        }

        public static loadContentHistory(containerSelector: string, action: string, withResponse: boolean = false, withResize: boolean = false, withClean: boolean = false, callback: any = null) {
            Utils.performLoadContent(containerSelector, action, withResize, callback);
        }


        //call this before menu calls... cancels ajax requests
        public static cancelRequests() {
            while (Utils.requestList.length) {
                var currReq = Utils.requestList.pop();
                //console.log(currReq);
                currReq.abort();
            }
        }
    }
}