(function ($, o, window) {
    o.realtime = (function () {
        var api = {
            url: void 0
        };

        var subscriptions = [];

        function listen(url) {
            api.url = url;

            enqueue(function () {
                extendHubs(); // must extend before the connection is started.

                $.connection.hub.start();
            });
        }

        function enqueue(callback) {
            o.load({
                url: api.url,
                callback: callback
            });
        }

        function extendHubs() {
            $.each(subscriptions, function () {
                this();
            });
        }

        function subscribe(hubName, extensions) {
            var callback = function () {
                var hub = $.connection[hubName];
                $.extend(hub, extensions);
            };
            subscriptions.push(callback);
        }

        return {
            listen: listen,
            subscribe: subscribe
        };
    })();
})(jQuery, overmind, window);