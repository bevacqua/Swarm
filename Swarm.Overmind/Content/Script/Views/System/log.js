(function ($, o) {
	function filterTable() {
		var tableRows = $('.filterableTable tbody tr');
		var filterColumns = $('.filter td input');
		var showRow;

		for (var j = 0; j < tableRows.length; j++) {
			showRow = true;
			var columns = tableRows[j].cells.length;
			for (var i = 0; i < columns; i++) {

				if (hasFilter(filterColumns[i])) {
					var filter = filterColumns[i].value;
					if (shouldBeShowing(tableRows[j].cells[i].innerHTML, filter)) {
						showRow = true;
					} else {
						showRow = false;
						break;
					}
				}
			}
			applyFilter(tableRows[j], showRow);
		}
	}

	function applyFilter(row, show) {
		row.style.display = (show) ? "" : "none";
	}

	function shouldBeShowing(data, filter) {
		return data.toLowerCase().indexOf(filter.toLowerCase()) != -1;
	}

	function hasFilter(column) {
		return column.value != "";
	}

    function addButtonHideColumn() {
        var html = '<button class="hideColumn">X</button>';
        var th = $('#syslogs th');
        $.each(th, function () {
            $(this).append(html);
        });
    }
    
    function isVisibleColumn(index) {
        return $("#syslogs td:nth-child("+ index +")").css('display') != 'none';
    }

    function setVisibility(index) {
        if (!isVisibleColumn(index)) {
            return 'none';
        }
        return 'table-cell';
    }

    function hideColumn(index) {
        $("#syslogs td:nth-child("+ index +")").hide();
        $("#syslogs thead th:nth-child("+ index +")").hide();
    }

    function init(hub) {
        $(function () {
            o.realtime.subscribe("logs", {
                update: function(entry) {
                    var syslogs = $("#syslogs tbody");
                    var build = o.tag;
                    var row = build("tr");

                    build("td").appendTo(row).text(entry.date).css('display',setVisibility(1));
                    build("td").appendTo(row).text(entry.level).css('display',setVisibility(2));
                    build("td").appendTo(row).text(entry.message).css('display',setVisibility(3));
                    build("td").appendTo(row).text(entry.thread).css('display',setVisibility(4));
                    if (entry.exception == null) {
                        build("td").appendTo(row).text('').css('display',setVisibility(5));
                    } else {
                        build("td").appendTo(row).text(entry.exception.message).css('display',setVisibility(5));    
                    }
                    build("td").appendTo(row).text(entry.logger).css('display',setVisibility(6));
                    build("td").appendTo(row).text(entry.requestUrl).css('display',setVisibility(7));
                    
                    row.prependTo(syslogs).flash("#b1f7ed");
                    syslogs.find("tr").slice(9).slideUp().remove();
                    filterTable();
                }
            });
            addButtonHideColumn();
            
            $('.filterText').on("keyup", function () {
                filterTable();
            });
            
            $('.filterableTable').on("click", ".hideColumn", function () {
                var column = $(this).parent();
                var index = $(column).index() + 1;
                hideColumn(index);
            });
            
            o.realtime.listen(hub);
        });
    };

    o.views.system = o.views.system || {};
    o.views.system.log = {
        init: init,
    };
})(jQuery, overmind);