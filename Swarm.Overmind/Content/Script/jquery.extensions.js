(function($) {
	$.fn.fadeOutAndRemove = function() {
		return this.each(function() {
			var self = $(this);
			self.fadeOut("fast", function() {
				self.remove();
			});
		});
	};

	$.fn.flash = function(opts) {
		if (typeof opts === "string") {
			opts = {
				color: opts
			};
		}
		var defaults = {
			color: "#000",
			duration: 500
		};
		var settings = $.extend({}, defaults, opts);
		var half = settings.duration / 2;

		return this.each(function() {
			var self = $(this);
			var flash = function() {
				var original = self.css("backgroundColor");
				self.animate({ backgroundColor: settings.color }, half)
					.animate({ backgroundColor: original }, half);
			};
			if (!!opts.wait) {
				flash();
			} else {
				setTimeout(flash, 0);
			}
		});
	};

	/* the element requires the following CSS in order to be properly centered using this method.
        left: 50%;
        top: 50%;
        position: absolute;
     */
	$.fn.center = function() {
		return this.each(function() {
			var self = $(this);
			self.css("marginLeft", -self.width() / 2);
			self.css("marginTop", -self.height() / 2);
		});
	};

	$.fn.enable = function(b) {
		return this.each(function() {
			this.disabled = !b;
		});
	};

	$.script = function(url, opts) {
		var options = $.extend(opts || { }, {
			dataType: "script",
			url: url
		});
		return $.ajax(options);
	};
})(jQuery);