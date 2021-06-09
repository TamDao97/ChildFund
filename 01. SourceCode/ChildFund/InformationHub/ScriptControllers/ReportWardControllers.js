$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var kytu = "'";
var dateNow = new Date();
var yearNow = dateNow.getFullYear();
var fromYear = yearNow - 9;

var modelSearch = {
    ToYear: '',
    FromYear: '',
    Type: '',
    Export: 0
};
function InitModel() {
    $('#fromYear').val(fromYear.toString());
    $('#toYear').val(yearNow.toString());
}
function GetModelSearch() {
    modelSearch.FromYear = $('#fromYear').val();
    modelSearch.ToYear = $('#toYear').val();
}
function SearchChart(Export) {
    modelSearch.Export = Export;
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    $("#titleLeftChart").html(GetNotifyByKey('StatisticByYear') + ' ' + fromYear + ' - ' + yearNow);
    GetModelSearch();
    OpenWaiting();
    $.post("/StatisticHome/GetReportWard", modelSearch, function (result) {
        if (result.ok === true) {
            GenChart(result.Chart_Title, result.listCount, result.chartData, result.tabledata, result.listYear);
            if (Export !== 0) {
                var link = document.getElementById('linkDowload');
                link.href = result.PathFile;
                link.download = 'Thong-ke-truong-hop' + (Export === 1 ? '.xlsx' : '.pdf');
                link.focus();
                link.click();
            }
        } else {
            toastr.error(result.mess, { timeOut: 5000 });
        }
        CloseWaiting();
        modelSearch.Export = 0;
    });
}

var colors = ['#26C6DA', 'orange', '#a09e9e', '#00797c', '#84bd00', '#26C6DA'];
function GenChart(Chart_Title, listCount, chartData, tabledata, listYear) {
    var table = '';
    for (var j = 0; j < tabledata.length; j++) {
        table += ' <tr>';
        table += ' <td>' + tabledata[j].Year + '</td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT01' + kytu + ')">' + tabledata[j].Count1 + ' (' + tabledata[j].Percent1 + ' %)' + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT02' + kytu + ')">' + tabledata[j].Count2 + ' (' + tabledata[j].Percent2 + ' %)' + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT03' + kytu + ')">' + tabledata[j].Count3 + ' (' + tabledata[j].Percent3 + ' %)' + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT04' + kytu + ')">' + tabledata[j].Count4 + ' (' + tabledata[j].Percent4 + ' %)' + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT05' + kytu + ')">' + tabledata[j].Count5 + ' (' + tabledata[j].Percent5 + ' %)' + '</a></td>';
        table += ' <td class="text-center">' + tabledata[j].CountAll + '</td>';
        table += ' </tr>';
    }
    $('#table').html(table);
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
                categories: listYear,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: Chart_Title
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
                name: chartData[0].AbuseType,
                data: chartData[0].Count

            }, {
                name: chartData[1].AbuseType,
                data: chartData[1].Count

            }, {
                name: chartData[2].AbuseType,
                data: chartData[2].Count

            }, {
                name: chartData[3].AbuseType,
                data: chartData[3].Count

                }
                , {
                    name: chartData[4].AbuseType,
                    data: chartData[4].Count

                }
            ]
        });
    }
}

//function GenChartLeft(listCount, Chart_Title) {
//    var tableLeft = '';
//    var mang = [];
//    for (var i = 0; i < listCount.length; i++) {
//        mang.push([listCount[i].LableName, listCount[i].Count]);
//        tableLeft += '<tr><td>' + listCount[i].LableName + '</td><td><a href="javascript:void(0)"  onclick="ViewReportByAbuse(' + kytu + listCount[i].AbuseId + kytu + ')">' + listCount[i].Count + '</a></td><td>' + listCount[i].Percen + '</td></tr>';
//    }
//    $('#tableLeft').html(tableLeft);
//    if ($('#containerNow').length > 0) {
//        Highcharts.chart('containerNow', {
//            chart: {
//                type: 'pie',
//                //style: {
//                //    fontFamily: "'Source Sans Pro', Arial, Helvetica, sans-serif"
//                //}
//            },
//            plotOptions: {
//                pie: {
//                    allowPointSelect: true,
//                    cursor: 'pointer',
//                    dataLabels: {
//                        enabled: false
//                    },
//                    showInLegend: true
//                }
//            },
//            title: {
//                text: ''
//            },
//            colors: colors,
//            series: [{
//                name: Chart_Title,
//                data: mang
//            }]
//        });
//    }

//}
function ResetModel() {
    InitModel();
    SearchChart(0);
}
ResetModel();
function ViewReportByAbuse(value) {
    var DateSearchFrom = '';
    var DateSearchTo = '';
    var date = dateNow.getDate();
    var month = dateNow.getMonth() + 1;
    var year = dateNow.getFullYear();

    DateSearchFrom = '01/01/' + $('#fromYear').val();
    DateSearchTo = (date < 10 ? '0' + date : date) + '/' + (month < 10 ? '0' + month : month) + '/' + dateNow.getFullYear();
    sessionStorage.setItem("keyDate", DateSearchFrom + ';' + DateSearchTo);
    sessionStorage.setItem("keyAbuse", value);
    window.location = '/ReportProfile/Index';
}
//function ViewReportByStatus(value, DateSearch) {
//    var Type = $('input[name=sgroupType]:checked').val();
//    if (Type === '0' || Type === '1') {
//        sessionStorage.setItem("keyDate", DateSearch + ';' + DateSearch);
//    } else {
//        var Year = $('#sYear').val();
//        var d = new Date(Year, DateSearch, 0);
//        if (DateSearch.length === 1) {
//            DateSearch = '0' + DateSearch;
//        }
//        sessionStorage.setItem("keyDate", '01/' + DateSearch + '/' + Year + ';' + d.getDate() + '/' + DateSearch + '/' + Year);
//    }
//    sessionStorage.setItem("keyStatus", value);
//    //var win = window.open('/ReportProfile/Index', '_blank');
//    //win.focus();
//    window.location = '/ReportProfile/Index';
//}