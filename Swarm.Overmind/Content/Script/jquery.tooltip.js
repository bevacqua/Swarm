(function ($, window) {
    $.fn.tooltip = function (){
		var classes = {
			tooltip: "tooltip",
			top: "tooltip-top",
			left: "tooltip-left",
			right: "tooltip-right"
		};
		
		function init(self, tooltip) {
			if ($(window).width() < tooltip.outerWidth() * 1.5) {
				tooltip.css("max-width", $(window).width() / 2);
			} else {
				tooltip.css("max-width", 340);
			}

			var pos = {
				x: self.offset().left + (self.outerWidth() / 2) - (tooltip.outerWidth() / 2),
				y: self.offset().top - tooltip.outerHeight() - 20
			};

			if (pos.x < 0) {
				pos.x = self.offset().left + self.outerWidth() / 2 - 20;
				tooltip.addClass(classes.left);
			} else {
				tooltip.removeClass(classes.left);
			}

			if (pos.x + tooltip.outerWidth() > $(window).width()) {
				pos.x = self.offset().left - tooltip.outerWidth() + self.outerWidth() / 2 + 20;
				tooltip.addClass(classes.right);
			} else {
				tooltip.removeClass(classes.right);
			}

			if (pos.y < 0) {
				pos.y = self.offset().top + self.outerHeight();
				tooltip.addClass(classes.top);
			} else {
				tooltip.removeClass(classes.top);
			}

			tooltip.css({
				left: pos.x,
				top: pos.y
			}).animate({
				top: "+=10",
				opacity: 1
			}, 50);
		};

		function activate() {
			var self = $(this);
			var message = self.attr("title");
			var tooltip = $("<div class='{0}'></div>".format(classes.tooltip));

			if (!message) {
				return;
			}
			self.removeAttr("title");
			tooltip.css("opacity", 0).html(message).appendTo("body");

			var reload = function() { // respec tooltip's size and position.
				init(self, tooltip);
			};
			reload();
			$(window).resize(reload);

			var remove = function () {
				tooltip.animate({
					top: "-=10",
					opacity: 0
				}, 50, function() {
					$(this).remove();
				});

				self.attr("title", message);
			};
			
			self.unbind("mouseleave.tooltip");
			self.bind("mouseleave.tooltip", remove);
			self.unbind("click.tooltip");
			tooltip.bind("click.tooltip", remove);
		};

        return this.each(function () {
            var self = $(this);
			self.unbind("mouseenter.tooltip");
			self.bind("mouseenter.tooltip", activate);
        });
    };

	$.tooltip = function() {
		return $("[rel~=tooltip]").tooltip();
	};
})(jQuery, window);