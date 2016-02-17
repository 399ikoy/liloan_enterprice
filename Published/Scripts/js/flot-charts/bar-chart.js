$(document).ready(function () {
    var dataurl = 'Get/';
    // setup plot
    var options = {
        legend: {
            show: true,
            margin: 10,
            backgroundOpacity: 0.9
        },
        points: {
            show: true,
            radius: 3
        },
        lines: {
            show: true
        },
        grid: { hoverable: true, clickable: true },
        yaxis: { min: 0, tickFormatter: formatter },
        xaxis: { ticks: [[1, "Jan"], [2, "Feb"], [3, "Mar"], [4, "Apr"], [5, "May"], [6, "Jun"], [7, "Jul"], [8, "Aug"], [9, "Sep"], [10, "Oct"], [11, "Nov"], [12, "Dec"]] }

    };
    function formatter(val, axis) {
        return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    var d = new Date();
    var n = d.getFullYear();
    $.ajax({
        url: dataurl,
        method: 'GET',
        data: { year: n },
        dataType: 'json',
        success: function (data) {
            $.plot($("#placeholder"), data, options);
        }
    });


    var previousPoint = null;
    $("#placeholder").bind("plothover", function (event, pos, item) {
        if (item) {
            if (previousPoint != item.dataIndex) {
                previousPoint = item.dataIndex;

                $("#tooltip").remove();
                var x = item.datapoint[0],
                    y = item.datapoint[1].toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                showTooltip(item.pageX, item.pageY, item.series.label +": " + y);

            }
        }
        else {
            $("#tooltip").remove();
            previousPoint = null;
        }
    });


    function showTooltip(x, y, contents) {
        $('<div id="tooltip">' + contents + '</div>').css({
            position: 'absolute',
            display: 'none',
            top: y + 5,
            left: x + 5,
            border: '1px solid #fdd',
            padding: '2px',
            'background-color': '#fee',
            opacity: 0.80
        }).appendTo("body").fadeIn(200);
    }


    
    ///* Make some random data for Flot Line Chart*/

    //var data1 = [[1, 60], [2, 30], [3, 50], [4, 100], [5, 10], [6, 90], [7, 85], [8, 100], [9, 10], [10, 90], [11, 85],[12,80]];
    ////var data2 = [[1,20], [2,90], [3,60], [4,40], [5,100], [6,25], [7,65]];
    //// var data3 = [[1,100], [2,20], [3,60], [4,90], [5,80], [6,10], [7,5]];

    ///* Create an Array push the data + Draw the bars*/

    //var barData = new Array();

    //barData.push({
    //    data: data1,
    //    label: 'Monthly Total Purchase Reports',
    //    bars: {
    //        show: true,
    //        barWidth: 0.08,
    //        order: 1,
    //        lineWidth: 0,
    //        fillColor: '#8BC34A'
    //    }
    //});

    // //barData.push({
    // //        data : data2,
    // //        label: 'Purchase',
    // //        bars : {
    // //                show : true,
    // //                barWidth : 0.08,
    // //                order : 2,
    // //                lineWidth: 0,
    // //                fillColor: '#00BCD4'
    // //        }
    // //});

    //// barData.push({
    ////         data : data3,
    ////         label: 'Profit',
    ////         bars : {
    ////                 show : true,
    ////                 barWidth : 0.08,
    ////                 order : 3,
    ////                 lineWidth: 0,
    ////                 fillColor: '#FF9800'
    ////         }
    //// });

    ///* Let's create the chart */
    //if ($('#bar-chart')[0]) {
    //    $.plot($("#bar-chart"), barData, {
    //        grid: {
    //            borderWidth: 1,
    //            borderColor: '#eee',
    //            show: true,
    //            hoverable: true,
    //            clickable: true
    //        },

    //        yaxis: {
    //            tickColor: '#eee',
    //            tickDecimals: 0,
    //            font: {
    //                lineHeight: 20,
    //                style: "normal",
    //                color: "#9f9f9f",
    //            },
    //            shadowSize: 0
    //        },

    //        xaxis: {
    //            tickColor: '#fff',
    //            tickDecimals: 0,
    //            font: {
    //                lineHeight: 20,
    //                style: "normal",
    //                color: "#9f9f9f"
    //            },
    //            shadowSize: 0,
    //        },

    //        legend: {
    //            container: '.flc-bar',
    //            backgroundOpacity: 0.5,
    //            noColumns: 0,
    //            backgroundColor: "white",
    //            lineWidth: 0
    //        }
    //    });
    //}

    ///* Tooltips for Flot Charts */

    //if ($(".flot-chart")[0]) {
    //    $(".flot-chart").bind("plothover", function (event, pos, item) {
    //        if (item) {
    //            var x = item.datapoint[0].toFixed(2),
    //                y = item.datapoint[1].toFixed(2);
    //            $(".flot-tooltip").html(item.series.label + " of " + x + " = " + y).css({ top: item.pageY + 5, left: item.pageX + 5 }).show();
    //        }
    //        else {
    //            $(".flot-tooltip").hide();
    //        }
    //    });

    //    $("<div class='flot-tooltip' class='chart-tooltip'></div>").appendTo("body");
    //}
});