var modelSearch = {
    WardId: ""
};


function SearchChart() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/StatisticHome/GetReportWard", modelSearch, function (result) {
        $('#loader_id').removeClass("loader");
        document.getElementById("overlay").style.display = "none";
        GenChartLeft(result.listCount);
        GenChartRight(result.listChartCount, result.listLable);
    });
}

function GenChartRight(listChartCount, listLable) {
    var tableHeaderRight = '<th >Đối tượng</th>';
    for (var i = 0; i < listLable.length; i++) {
        tableHeaderRight += '<th  class="text-center">' + listLable[i] + '</th>';
    }
    $('#tableHeaderRight').html(tableHeaderRight);

    var tableRight = '';
    for (var j = 0; j < listChartCount.length; j++) {
        tableRight += ' <tr>';
        tableRight += ' <td>' + listChartCount[j].LableName + '</td>';
        for (var h = 0; h < listChartCount[j].ListCount.length; h++) {
            tableRight += ' <td  class="text-center">' + listChartCount[j].ListCount[h] + '</td>';
        }
        tableRight += ' </tr>';
    }
    $('#tableRight').html(tableRight);
    if ($('#containerXuLy').length > 0) {
        Highcharts.chart('containerXuLy', {
            chart: {
                type: 'line'
            },
            title: {
                text: ''
            },
            colors: colors,
            xAxis: {
                categories: listLable,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: GetNotifyByKey('Number_case')
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{
                name: listChartCount[0].LableName,
                data: listChartCount[0].ListCount

            }, {
                name: listChartCount[1].LableName,
                data: listChartCount[1].ListCount

            }, {
                name: listChartCount[2].LableName,
                data: listChartCount[2].ListCount

            }, {
                name: listChartCount[3].LableName,
                data: listChartCount[3].ListCount

            }, {
                name: listChartCount[4].LableName,
                data: listChartCount[4].ListCount

            }]
        });
    }
}