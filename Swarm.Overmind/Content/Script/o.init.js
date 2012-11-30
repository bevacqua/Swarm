(function ($, o) {
	o.container = "#layout-content";

	$.ajaxSetup({
		type: "POST",
		success: function(result) { // default ajax success, if it's overriden, o.ajax.success should still be invoked.
			o.ajax.success(result); // prevent jQuery from passing an invalid viewResultContainer.
		}
	});

	$("#menu-icon").click(function(e) {
		o.scrollTo($("#menu"));
        e.preventDefault();
        return false;
	});

	$("li.nav-top a").click(function(e) {
		o.scrollTo($("#top"));
        e.preventDefault();
        return false;
	});
})(jQuery, overmind);