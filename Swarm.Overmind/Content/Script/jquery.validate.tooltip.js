(function ($, l) {
    function convertValidationMessagesToTooltips(form, isRenderTimeValidation) {
        var fields = $("fieldset .field-validation-valid, fieldset .field-validation-error", form);
        fields.each(function() {
            var self = $(this);

            if (!!isRenderTimeValidation) {
                self.addClass("model-validation");
            } else {
                self.removeClass("model-validation");
            }

            self.addClass("tooltip-icon");
            self.attr("rel", "tooltip");
            if (self.hasClass("field-validation-error")) {
                self.attr("title", self.text());
            } else {
                self.attr("title", l.Common.Valid);
            }
            var span = self.find("span");
            if (span.length) {
                span.text("");
            } else {
                self.text("");
            }
            self.tooltip();
        });
    }

    function initializeTooltip() {
        var form = $(this);
        var settings = form.data("validator").settings;

        // update error message:
        var oldErrorPlacement = settings.errorPlacement;
        var newErrorPlacement = function () {
            (oldErrorPlacement || $.noop).apply(this, arguments);
            convertValidationMessagesToTooltips(form);
        };
        settings.errorPlacement = newErrorPlacement;

        // update success message:
        var oldSuccess = settings.success;
        var newSuccess = function () {
            (oldSuccess || $.noop).apply(this, arguments);
            convertValidationMessagesToTooltips(form);
        };
        settings.success = newSuccess;

        convertValidationMessagesToTooltips(form, true); // initialize in case of model-drawn validation messages at page render time.
    }

    $(function () {
        $("form").each(initializeTooltip);
    });
})(jQuery, localization);