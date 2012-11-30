(function ($, o, l) {
    var executionId;
    var snapshots = [];
    var maxSnapshots = 9;

    var index = $(".index");
    var status = $(".status");
    var charts = $(".charts");
    var abort = $(".abort");

    var overall;
    var workload;
    var responseTime;
    var cakeChart;

    var resultsAcumulation = { Success: 0, Failed: 0 };

    var statusColorCodes = {
        Created: "#000",
        Preparing: "#FBCE30",
        Synchronizing: "#2E64FE",
        Executing: "#04B45F",
        Completed: "#0F0F8C",
        Aborted: "#FF483D",
        Faulted: "#CE0F0C"
    };

    var abortUrl;

    function init(model, abortEndpoint) {
        executionId = model.ExecutionId;
        abortUrl = abortEndpoint;

        $.each(model.Snapshots, function () {
            addSnapshot(this);
        });
        updateStatus(model);
        bindClicks();

        o.realtime.subscribe("report", {
            update: addSnapshot,
            updateStatus: updateStatus
        });

        createOverallChart();
        createWorkloadChart();
        createResponseTimeChart();
        createResultsCake();
    };

    function addSnapshot(snapshot) {
        snapshot.AliveUsers = snapshot.BusyUsers + snapshot.SleepingUsers;
        if (snapshot.ExecutionId !== executionId) { // we are only interested in updates for this executionId.
            return;
        }
        if (!charts.is(":visible")) {
            charts.fadeIn("slow");
        }
        snapshots.push(snapshot);
        addSnapshotToList(snapshot);
        var bookmarks = index.find(".bookmark");
        bookmarks.slice(maxSnapshots, bookmarks.length).fadeOutAndRemove();
        autoselect(snapshot);
        resultsAcumulation.Success += parseInt(snapshot.Successful);
        resultsAcumulation.Failed += parseInt(snapshot.Failed);
        updateResultsCake();
    }

    function addSnapshotToList(snapshot) {
        var c = $("<div class='bookmark'/>")
			.data("id", snapshot.Id)
			.data("index", snapshots.length - 1)
			.text(snapshot.Name);

        index.prepend(c);
    }

    function autoselect(snapshot) {
        var bookmarks = index.find(".bookmark");
        var selected = bookmarks.filter(".selected");

        // if no selection is made, or the selected element is the latest...
        if (selected.length == 0 || bookmarks.index(selected) == 1) {
            select(bookmarks.first(), false);
            updateOverallChart(snapshot);
            updateWorkloadChart(snapshot);
            updateResponseTimeChart(snapshot);
        }
    }

    function select(bookmark, redraw) {
        var selected = index.find(".bookmark.selected");

        selected.removeClass("selected");
        bookmark.addClass("selected");

        if (redraw) {
            createOverallChart();
            createWorkloadChart();
            createResponseTimeChart();
            createResultsCake();
        }
    }

    function bindClicks() {
        index.on("click", ".bookmark:not(.selected)", function () {
            select($(this), true);
        });

        abort.prop("cursor", "pointer").on("click", function () {
            $.ajax({
                url: abortUrl,
                success: function (data) {
                    if (o.ajax.success(data)) {
                        console.log(data);
                    }
                }
            });
        });
    }

    function createOverallChart() { // redraw the chart from scratch.
        var selected = index.find(".bookmark.selected");
        var slice = snapshots.slice(0, selected.data("index"));
        overall = new Highcharts.Chart({
            chart: {
                renderTo: "overall",
                type: "line"
            },
            title: {
                text: l.Reporting.Overall
            },
            xAxis: {
                title: {
                    text: l.Reporting.OverallX
                },
                type: 'datetime'
            },
            yAxis: {
                title: {
                    text: l.Reporting.OverallY
                }
            },
            series: [{
                name: 'Success',
                data: $.map(slice, function (v) { return [getPoint(v, "Successful")]; })
            }, {
                name: 'Failure',
                data: $.map(slice, function (v) { return [getPoint(v, "Failed")]; })
            }, {
                name: 'Completed',
                data: $.map(slice, function (v) { return [getPoint(v, "Completed")]; })
            }, {
                name: 'Sleeping',
                data: $.map(slice, function (v) { return [getPoint(v, "SleepingUsers")]; })
            }, {
                name: 'Waiting',
                data: $.map(slice, function(v) { return [getPoint(v, "BusyUsers")]; })
            }, {
                name: 'Alive',
                data: $.map(slice, function (v) { return [getPoint(v, "AliveUsers")]; })
            }]
        });
    }



    function updateOverallChart(snapshot) { // just update the chart with the latest snapshot.
        if (!overall) {
            return;
        }
        overall.series[0].addPoint(getPoint(snapshot, "Successful"), true, false);
        overall.series[1].addPoint(getPoint(snapshot, "Failed"), true, false);
        overall.series[2].addPoint(getPoint(snapshot, "Completed"), true, false);
        overall.series[3].addPoint(getPoint(snapshot, "SleepingUsers"), true, false);
        overall.series[4].addPoint(getPoint(snapshot, "BusyUsers"), true, false);
        overall.series[5].addPoint(getPoint(snapshot, "AliveUsers"), true, false);
    }

    function updateWorkloadChart(snapshot) { // just update the chart with the latest snapshot.
        if (!workload) {
            return;
        }
        workload.series[0].addPoint(getPoint(snapshot, "Average"), true, false);
    }


    function updateResponseTimeChart(snapshot) { // just update the chart with the latest snapshot.
        if (!responseTime) {
            return;
        }
        responseTime.series[0].addPoint(getPoint(snapshot, "AverageResponseTime"), true, false);
    }


    function createWorkloadChart() { // redraw the chart from scratch.
        var selected = index.find(".bookmark.selected");
        var slice = snapshots.slice(0, selected.data("index"));
        workload = new Highcharts.Chart({
            chart: {
                renderTo: "workload",
                type: "line"
            },
            title: {
                text: l.Reporting.Workload
            },
            xAxis: {
                title: {
                    text: l.Reporting.WorkloadX
                },
                type: 'datetime'
            },
            yAxis: {
                title: {
                    text: l.Reporting.WorkloadY
                }
            },
            series: [{
                name: 'Workload',
                data: $.map(slice, function (v) { return [getPoint(v, "Average")]; })
            }]
        });
    }


    function createResponseTimeChart() { // redraw the chart from scratch.
        var selected = index.find(".bookmark.selected");
        var slice = snapshots.slice(0, selected.data("index"));
        responseTime = new Highcharts.Chart({
            chart: {
                renderTo: "responseTime",
                type: "line"
            },
            title: {
                text: l.Reporting.ResponseTime
            },
            xAxis: {
                title: {
                    text: l.Reporting.ResponseTimeX
                },
                type: 'datetime'
            },
            yAxis: {
                title: {
                    text: l.Reporting.ResponseTimeY
                }
            },
            series: [{
                name: 'ResponseTime',
                data: $.map(slice, function (v) { return [getPoint(v, "AverageResponseTime")]; })
            }]
        });
    }

    // Build the chart

    function updateResultsCake() {
        if (!cakeChart) {
            createResultsCake();
        }
        cakeChart.series[0].setData([
            ['Success', resultsAcumulation.Success],
            ['Failure', resultsAcumulation.Failed]
        ]);
    }

    function createResultsCake() {
        cakeChart = new Highcharts.Chart({
            chart: {
                renderTo: 'resultsCake',
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            title: {
                text: l.Reporting.ResultsCake
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y} ({point.percentage}%)</b>',
                percentageDecimals: 2
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            return '<b>' + this.point.name + '</b>: ' + this.percentage.toFixed(2) + ' %';
                        }
                    },
                    showInLegend: true
                }
            },
            series: [{
                type: 'pie',
                name: 'Results',
                data: [
                    ['Success', resultsAcumulation.Success],
                    ['Failure', resultsAcumulation.Failed]
                ]
            }]
        });
    }

    function getPoint(snapshot, property) {
        var point = [new Date(snapshot.Started).getTime(), snapshot[property]];
        return point;
    }
    
    function updateStatus(json) {
        if (json.ExecutionId !== executionId) { // we are only interested in updates for this executionId.
            return;
        }
        var s = json.Status;
        status.text(s);
        status.animate({ color: statusColorCodes[s] });

        var abortable = s === "Preparing" || s === "Synchronizing" || s === "Executing";
        abort.toggle(abortable);
    }

    o.views.report = o.views.report || {};
    o.views.report.scenario = {
        init: init
    };

})(jQuery, overmind, localization);