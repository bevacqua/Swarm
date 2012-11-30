(function ($) {
	$.placeholder = {
		placeholderClass: null,

		hidePlaceholder: function () {
			var self = $(this);
			if (self.val() == self.attr('placeholder')) {
				self.val("").removeClass($.placeholder.placeholderClass);
			}
		},

		showPlaceholder: function () {
			var self = $(this);
			if (self.val() == "") {
				self.val(self.attr('placeholder')).addClass($.placeholder.placeholderClass);
			}
		},

		preventPlaceholderSubmit: function () {
			$(this).find(".placeholder").each(function () {
				var self = $(this);
				if (self.val() == self.attr('placeholder')) {
					self.val('');
				}
			});
			return true;
		}
	};

	$.fn.placeholder = function (options) {
		if (document.createElement('input').placeholder == undefined) {
			var config = {
				placeholderClass: 'placeholding'
			};

			if (options) $.extend(config, options);
			$.placeholder.placeholderClass = config.placeholderClass;

			this.each(function () {
				var self = $(this);
				if (self.is(":password")) { // fallback to a fixed placeholder, it will be masked by the input anyways.
					self.attr("placeholder", "password");
				}
				self.focus($.placeholder.hidePlaceholder);
				self.blur($.placeholder.showPlaceholder);
				if (self.val() == '') {
					self.val(self.attr("placeholder"));
					self.addClass($.placeholder.placeholderClass);
				}
				self.addClass("placeholder");
				$(this.form).submit($.placeholder.preventPlaceholderSubmit);
			});
		}
		return this;
	};
})(jQuery);