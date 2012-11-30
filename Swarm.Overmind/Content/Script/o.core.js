(function (window, $, l) {
    var o = (function () {
        function createTag(tagName, css) {
            return $("<" + tagName + ">").addClass(css);
        }

        function createDialog() {
            var dialog = createTag("section", "dialog");
            var title = createTag("header").appendTo(dialog);
            var content = createTag("article").appendTo(dialog);
            var self = {
                object: dialog,
                title: title,
                content: content
            };
            self.decays = function () {
                var remove = function () {
                    dialog.fadeOutAndRemove();
                };
                dialog.on("click.decay", remove);
                setTimeout(remove, 30 * 1000);
                return self;
            };
            return self;
        }

        function initDialog(opts) {
            var dialog = createDialog();
            dialog.title.text(opts.title);
            if (typeof opts.message === "string") {
                dialog.content.text(opts.message);
            } else {
                dialog.content.append(opts.message);
            }
            if (opts.width) {
                dialog.object.css("width", opts.width);
            }
            if (opts.maxWidth) {
                dialog.object.css("max-width", opts.maxWidth);
            }
            dialog.object.addClass(opts.css).appendTo("body").center();
            if (opts.decays) {
                dialog.decays();
            }
            if (opts.fadeIn) {
                dialog.object.fadeIn();
            }
            return dialog;
        }

        function notificationDialog(title, message) {
            var opts;
            if (typeof title === "object") {
                opts = title;
            } else {
                opts = {
                    title: title,
                    message: message
                };
            }
            var dialog = initDialog($.extend(opts, {
                css: "notification notification-message",
                decays: true,
                fadeIn: true
            }));
            return dialog;
        }

        function actionDialog(opts) {
            var contents = createTag("section", "dialog-contents");

            if (typeof opts.message === "string") {
                contents.text(opts.message);
            } else {
                contents.append(opts.message);
            }

            var buttons = createTag("section", "dialog-buttons");
            var closeButtons = [];

            $.each(opts.buttons, function () {
                var buttonInfo = this;
                var button = createTag("button", "dialog-button");
                button.addClass(buttonInfo.css);
                button.text(buttonInfo.caption);
                if (buttonInfo.click) {
                    button.on("click.dialog-button", buttonInfo.click);
                } else if (buttonInfo.href) {
                    button.on("click.dialog-button", function () {
                        window.location.href = buttonInfo.href;
                    });
                } else if (buttonInfo.close) {
                    button.addClass("dialog-close-button");
                    button.text(buttonInfo.caption || l.Common.Close);
                    closeButtons.push(button);
                }
                buttons.append(button);
            });
            var elements = contents.add(buttons);
            var dialog = initDialog($.extend(opts, {
                css: "action-dialog",
                message: elements,
                fadeIn: true
            }));

            $.each(closeButtons, function () {
                this.on("click.dialog-button", function () {
                    dialog.object.fadeOut("fast");
                });
            });
            return dialog;
        }

        function displayAjaxExceptionsInDialog(exceptions) {
            if (!!exceptions) {
                var title;
                if (exceptions.length > 1) {
                    title = l.Common.Errors;
                } else {
                    title = l.Common.SingleError;
                }
                var section = createTag("section", "dialog-contents");
                var dialog = initDialog({
                    title: title,
                    css: "notification notification-error",
                    message: exceptions.wrap(section)
                });
                dialog.decays().object.fadeIn();
                return dialog.object;
            }
            return false;
        }

        function parseExceptionsInAjaxResult(result) {
            if (!!result.Exceptions && !!result.Exceptions.length) {
                if (result.Exceptions.length === 1) {
                    var errorText = result.Exceptions[0].toString();
                    var paragraph = createTag("p", "error").text(errorText);
                    return paragraph;
                } else {
                    var list = createTag("ul", "errors");

                    $.each(result.Exceptions, function () {
                        var item = createTag("li", "error").text(this.toString());
                        list.append(item);
                    });

                    return list;
                }
            }
            return false;
        }

        function displayAjaxExceptions(result, asDialog) {
            var errors = parseExceptionsInAjaxResult(result);

            if (!!asDialog) {
                return displayAjaxExceptionsInDialog(errors);
            } else if (!!errors) {
                return errors;
            }

            return false;
        }

        function displayModelValidationWarnings(form, result) {
            if (!result.Errors) {
                return false;
            }
            $.each(result.Errors, function () {
                var field = $("span[data-valmsg-for={0}]".format(this.Key));
                var message;

                $.each(this.Messages, function () {
                    var text = this.toString();
                    if (!message) { // typically, there will be a single validation error message in each case.
                        message = text;
                    } else { // in case there's more, each goes in an individual span element.
                        if (typeof message === "string") {
                            message = createTag("span").text(message);
                        }
                        var next = createTag("span").text(text);
                        message.add(next);
                    }
                });

                if (typeof message === "string") {
                    field.text(message);
                } else {
                    field.append(message);
                }
            });
            return !!result.Errors.length;
        }

        function redirectResult(result) {
            if (result.redirect === true) {
                window.location.href = result.href;
                return true;
            }
            return false;
        }

        function viewResult(result, viewResultContainer) {
            if (result.partial === true) {
                if (!!result.title) {
                    $("title").text(result.title);
                }
                if (!!result.html) {
                    /*
                    *	The container can be specified in three ways.
                    *	- Directly being passed to the viewResult function, in which case we use that one.
                    *	- Provided by the response as a "preferred container".
                    *	- Or, lastly, just using the default container (which is tipically the layout body).
                    */
                    var container = viewResultContainer || result.container || b.container;
                    if (typeof container === "string") {
                        container = $(container);
                    }
                    container.hide().html(result.html);
                    container.fadeIn("fast");
                }
                if (!!result.script) {
                    var tag = createTag("script");
                    tag.html(result.script);
                    $("body").append(tag);
                }
                return true;
            }
            return false;
        }

        function replaceSubmitButton(element) {
            var button = createTag("button", "submit");
            var caption = createTag("span");
            caption.text(element.val());
            button.append(caption);
            button.on("click.submit", function (e) {
                e.preventDefault();
                button.submit();
            });
            element.replaceWith(button);
        }

        function disableElementBeforeSend(element) {
            var loading = l.Common.Loading;
            var caption;
            if (element.is("form")) {
                element = element.find("button.submit");
                caption = element.find("span");
            } else {
                caption = element;
            }
            caption.data("caption", element.text());
            caption.text(loading);
            element.addClass("disabled").enable(false);
        }

        function enableElementOnCompletion(element) {
            var caption;
            if (element.is("form")) {
                element = element.find("button.submit");
                caption = element.find("span");
            } else {
                caption = element;
            }
            caption.text(caption.data("caption"));
            element.removeClass("disabled").enable(true);
        }

        function disableElementDuringAjaxRequests(element, ajaxOptions) {
            // update form status before and after ajax calls
            var oldBeforeSend = ajaxOptions.beforeSend;
            var newBeforeSend = function () {
                (oldBeforeSend || $.noop).apply(this, arguments);
                disableElementBeforeSend(element);
            };
            ajaxOptions.beforeSend = newBeforeSend;

            var oldComplete = ajaxOptions.complete;
            var newComplete = function () {
                (oldComplete || $.noop).apply(this, arguments);
                enableElementOnCompletion(element);
            };
            ajaxOptions.complete = newComplete;
            return ajaxOptions;
        }

        function isAjaxFormDisabled(form) {
            var button = form.find("button.submit");
            return button.prop("disabled");
        }

        function ajaxSuccess(result, viewResultContainer) {
            if (result === null || result === void 0) {
                return true;
            }
            if (!displayAjaxExceptions(result, true)) { // this is true when Exceptions are raised, model state errors are verified elsewhere.
                if (!redirectResult(result)) { // redirect results terminate response handling right here.
                    if (!viewResult(result, viewResultContainer)) { // view results (or partial view results) get rendered here.
                        return true;
                    }
                }
            }
            return false;
        }

        function ajaxify(opts) {
            if (opts instanceof $) {
                opts = { element: opts };
            }
            if (opts.element.is("form")) {
                ajaxifyForm(opts);
            } else if (opts.element.is("a")) {
                ajaxifyAnchor(opts);
            }
        }

        function ajaxifyAnchor(opts) {
            var element = opts.element;
            element.on("click.ajax", function (e) {
                var ajaxOptions = {
                    url: opts.element.attr("href"),
                    success: function (result) {
                        if (ajaxSuccess(result, opts.viewResultContainer)) {
                            (opts.success || $.noop)(result);
                        }
                    }
                };
                ajaxOptions = disableElementDuringAjaxRequests(opts.element, ajaxOptions);
                $.ajax(ajaxOptions);
                e.preventDefault();
            });
        }

        function ajaxifyForm(opts) {
            var form = opts.element;
            form.find("[placeholder]").placeholder();
            var submit = form.find(":submit");
            replaceSubmitButton(submit);

            form.submit(function (e) {
                if (!isAjaxFormDisabled(form) && form.valid()) {
                    var ajaxOptions = {
                        success: function (result) {
                            if (ajaxSuccess(result, opts.viewResultContainer)) {
                                if (!displayModelValidationWarnings(form, result)) {
                                    (opts.success || $.noop)(result);
                                }
                            }
                        }
                    };
                    ajaxOptions = disableElementDuringAjaxRequests(form, ajaxOptions);
                    form.ajaxSubmit(ajaxOptions);
                }
                e.preventDefault();
            });
        }

        function scrollTo(element) {
            $('html, body').animate({
                scrollTop: element.offset().top
            }, "slow");
        }

        function setCookie(key, value, expires) {
            var defaults = {
                path: "/",
                expires: 1000 * 60 * 60 * 6 * 24 * 30 // 6 months
            };
            var date = new Date();
            var offset = expires || defaults.expires;
            date.setTime(date.getTime() + offset);
            var json = window.JSON.stringify(value);
            var cookie = "{0}={1}; expires={2}; path={3}".format(key, json, date.toGMTString(), defaults.path);
            document.cookie = cookie;
        }

        function getCookie(key, defaultValue) {
            var identifier = "{0}=".format(key);
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = cookies[i];
                while (cookie.charAt(0) == ' ') {
                    cookie = cookie.substring(1, cookie.length);
                }
                if (cookie.indexOf(identifier) == 0) {
                    var value = cookie.substring(identifier.length, cookie.length);
                    return $.parseJSON(value);
                }
            }
            return defaultValue || null;
        }

        function setStorageItem(key, value) {
            var localStorage = window.localStorage;
            if (localStorage) {
                try {
                    localStorage.setItem(key, window.JSON.stringify(value));
                } catch (e) {
                }
            }
            return !!localStorage;
        }

        function getStorageItem(key, defaultValue) {
            var localStorage = window.localStorage;
            if (localStorage) {
                var value = localStorage.getItem(key);
                if (value) {
                    return $.parseJSON(value);
                }
            }
            return defaultValue || null;
        }

        function setPreference(key, value, expires) {
            if (!setStorageItem(key, value)) {
                setCookie(key, value, expires);
            }
        }

        function getPreference(key, defaultValue) {
            var fromStorage = getStorageItem(key, defaultValue);
            if (fromStorage === defaultValue || fromStorage === null) {
                var fromCookie = getCookie(key, defaultValue);
                return fromCookie;
            } else {
                return fromStorage;
            }
        }

        var scriptLoader = [];

        /*
        * loads a script and defers a callback for when the script finishes loading.
        * you can also just stack callbacks on the script load by invoking this method repeatedly.
        *
        * opts format:	{
        *		url: the url of the target script resource,
        *		timeout: a timeout in milliseconds after which any callbacks on the script will be dropped, and the script element removed.
        *		callbacks: an optional array of callbacks to execute after the script completes loading.
        *		callback: an optional callback to execute after the script completes loading.
        *		before: an optional callback to execute before the script is loaded, only intended to be ran prior to requesting the script, not multiple times.
        *		success: an optional callback to execute when the script successfully loads, always remember to call script.complete at the end.
        *		error: an optional callback to execute when and if the script request fails.
        *	}
        */
        function loadScript(opts) {
            if (typeof opts === "string") {
                opts = {
                    url: opts
                };
            }
            var script = scriptLoader[opts.url];
            if (script === void 0) {
                var complete = function (s) {
                    s.status = "loaded";
                    s.executeCallbacks();
                };

                script = scriptLoader[opts.url] = {
                    url: opts.url,
                    status: "loading",
                    requested: new Date(),
                    timeout: opts.timeout || 10000,
                    callbacks: opts.callbacks || [opts.callback || $.noop],
                    addCallback: function (callback) {
                        if (!!callback) {
                            if (script.status !== "loaded") {
                                script.callbacks.push(callback);
                            } else {
                                callback();
                            }
                        }
                    },
                    executeCallbacks: function () {
                        $.each(script.callbacks, function () {
                            this();
                        });
                        script.callbacks = [];
                    },
                    before: opts.before || $.noop,
                    success: opts.success || complete,
                    complete: complete,
                    error: opts.error || $.noop
                };

                script.before();

                $.script(script.url, {
                    timeout: script.timeout,
                    success: function () {
                        script.success(script);
                    },
                    error: function () {
                        script.error(); // .error should remove anything added by .before
                        scriptLoader[script.url] = void 0; // dereference, no callbacks were executed, no harm is done.
                    }
                });
            } else {
                script.addCallback(opts.callback);
            }
        }

        return {
            ajax: {
                success: ajaxSuccess,
                disables: disableElementDuringAjaxRequests,
                disabled: isAjaxFormDisabled
            },
            ajaxify: ajaxify,
            notification: notificationDialog,
            dialog: actionDialog,
            tag: createTag,
            scrollTo: scrollTo,
            set: setPreference,
            get: getPreference,
            load: loadScript,
            views: {}
        };
    })();

    // expose the o object to the global namespace.
    window.overmind = o;
})(window, jQuery, localization);