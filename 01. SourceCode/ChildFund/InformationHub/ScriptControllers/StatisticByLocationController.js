$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var kytu = "'";

var modelSearch =
{
    DateFrom: '',
    DateTo: '',
    WardId: '',
    DistrictId: '',
    ProvinceId: '',
    AbuseId:'',
    Export: 0
};

function Refresh() {
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');
    $('#wardId').val('');
    $('#districtId').val('');
    $('#provinceId').val('');

    Search(3);
}
function GetModelSearch() {
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
    modelSearch.ProvinceId = $('#provinceId').val();
    modelSearch.DistrictId = $('#districtId').val();
    modelSearch.WardId = $('#wardId').val();
    modelSearch.AbuseId = $('input[name=AbuseId]:checked').val();
}
function Search(Export) {
    modelSearch.Export = Export;
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    OpenWaiting();
    $.post("/StatisticByLocation/ListReportProfileByLocation", modelSearch, function (result) {
        CloseWaiting();
        GenChart(result.lstTableLeft, result.lstTableRight, result.lstChart, result.lstLocation, result.lstType);
        if (Export !== 0 && Export !== 3) {
            var link = document.getElementById('linkDowload');
            link.href = result.PathFile;
            link.download = GetNotifyByKey('Statistic_location') + (Export === 1 ? '.xlsx' : '.pdf');
            link.focus();
            link.click();
        }
        if (Export === 3) {
            $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
            $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
        }
        modelSearch.Export = 0;
    });
}

function GetDistrict() {
    modelSearch.ProvinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
    });
}
function GetWard() {
    modelSearch.DistrictId = $('#districtId').val();
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
    });
}

var colors = ['#26C6DA', 'orange', '#a09e9e', '#00797c', '#84bd00', '#26C6DA'];
function GenChart(lstTableLeft, lstTableRight, lstChart, lstLocation, lstType) {
    GenTableLeft(lstTableLeft, lstType);
    GenTableRight(lstTableRight, lstType);
    if ($('#locationChart').length > 0) {
        var data = [];
        for (var k = 0; k < lstLocation.length; k++) {
            data.push(lstLocation[k]);
        }
        Highcharts.chart('locationChart', {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            xAxis: {
                categories: data
            },
            yAxis: {
                allowDecimals: false,
                min: 0,
                title: {
                    text: GetNotifyByKey('Number_case')
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            colors: colors,
            legend: {
                align: 'right',
                x: -10,
                verticalAlign: 'top',
                y: 10,
                floating: false,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: false,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                },
                series: {
                    maxPointWidth: 50
                }
            },
            series: [{
                name: lstChart[0].TypeName,
                data: lstChart[0].Count
            },
            //    {
            //    name: lstChart[1].TypeName,
            //    data: lstChart[1].Count
            //}, {
            //    name: lstChart[2].TypeName,
            //    data: lstChart[2].Count
            //}, {
            //    name: lstChart[3].TypeName,
            //    data: lstChart[3].Count
            //    }
            ]
        });
    }
}

function GenTableLeft(lstTableLeft, lstType) {
    var tableLeft = '';
    for (var j = 0; j < lstTableLeft.length; j++) {
        tableLeft += ' <tr>';
        tableLeft += ' <td>' + lstTableLeft[j].LableName + '</td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT01' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count1 + '</a></td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT02' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count2 + '</a></td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT03' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count3 + '</a></td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT04' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count4 + '</a></td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT05' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count5 + '</a></td>';
        tableLeft += ' </tr>';
    }
    $('#data_table_left').html(tableLeft);
}
function GenTableRight(lstTableRight, lstType) {
    var tableRight = '';
    for (var j = 0; j < lstTableRight.length; j++) {
        tableRight += ' <tr>';
        tableRight += ' <td>' + lstTableRight[j].LableName + '</td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT01' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count1 + '</a></td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT02' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count2 + '</a></td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT03' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count3 + '</a></td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT04' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count4 + '</a></td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT05' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count5 + '</a></td>';
        tableRight += ' </tr>';
    }
    $('#data_table_right').html(tableRight);
}

function ViewReportByAbuse(value, AreaId) {
    var provinceCheck = $('#provinceId').val();
    var districtCheck = $('#districtId').val();
    var wardCheck = $('#wardId').val();

    sessionStorage.setItem("keyAbuse", value);
    sessionStorage.setItem("keyDate", $("#dateFromSearch").val() + ';' + $("#dateToSearch").val());

    if (provinceCheck !== '') {
        if (districtCheck !== '') {
            sessionStorage.setItem("keyAddressByUser", AreaId + ";" + 3);
        } else {
            sessionStorage.setItem("keyAddressByUser", AreaId + ";" + 2);
        }
    }
    else {
        sessionStorage.setItem("keyAddressByUser", AreaId + ";" + 1);
    }
    window.location = '/ReportProfile/Index';
}