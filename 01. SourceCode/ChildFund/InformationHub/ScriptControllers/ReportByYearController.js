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
    $.post("/StatisticHome/GetReportByYear", modelSearch, function (result) {
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
    var trContent = '<td >' + GetNotifyByKey("Number_case")+'</td>';
    var trHeader = '<th>' + GetNotifyByKey("keyYear") +'</th>';
    for (var j = 0; j < tabledata.length; j++) {
        trHeader += ' <th class="text-center">' + tabledata[j].Year + '</ths>';
        trContent += ' <td class="text-center">' + tabledata[j].CountAll + '</td>';
    }
    $('#trContent').html(trContent);
    $('#trHeader').html(trHeader);
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

            }
            ]
        });
    }
}


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
