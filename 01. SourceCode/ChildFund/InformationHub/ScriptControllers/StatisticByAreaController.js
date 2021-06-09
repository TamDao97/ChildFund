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
    ProvinceId: ''
};

function Refresh(DistrictId, ProvinceId) {
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');
    $('#warId').val('');
    $('#districtId').val('');
    $('#provinceId').val('');

    SearchInit(DistrictId, ProvinceId);
}
function GetModelSearch() {
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
    modelSearch.ProvinceId = $('#provinceId').val();
    modelSearch.DistrictId = $('#districtId').val();
    modelSearch.WardId = $('#wardId').val();
}
function SearchInit(DistrictId, ProvinceId) {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    modelSearch.ProvinceId = ProvinceId;
    modelSearch.DistrictId = DistrictId;
    OpenWaiting();
    $.post("/StatisticByArea/ListReportProfileByArea", modelSearch, function (result) {
        $("#data_table").html(result);
        CloseWaiting();
        GenChart(result.lstTable, result.lstChart, result.lstArea);
    });
}
function Search() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    OpenWaiting();
    $.post("/StatisticByArea/ListReportProfileByArea", modelSearch, function (result) {
        $("#data_table").html(result);
        CloseWaiting();
        GenChart(result.lstTable, result.lstChart, result.lstArea);
    });
}

function GetDistrict() {
    modelSearch.ProvinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
    });
}
function GetWard() {
    modelSearch.DistrictId = $('#districtId').val();
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
    });
}
var colors = ['#26C6DA', 'orange', '#a09e9e', '#00797c', '#84bd00', '#26C6DA'];
function GenChart(lstTable, lstChart, lstArea) {
    var table = '';
    for (var j = 0; j < lstTable.length; j++) {
        table += ' <tr>';
        table += ' <td>' + lstTable[j].Name + '</td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT01' + kytu + ',' + kytu + lstTable[j].AreaId + kytu + ')">' + lstTable[j].Count1 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT02' + kytu + ',' + kytu + lstTable[j].AreaId + kytu + ')">' + lstTable[j].Count2 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT03' + kytu + ',' + kytu + lstTable[j].AreaId + kytu + ')">' + lstTable[j].Count3 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAbuse(' + kytu + 'AT04' + kytu + ',' + kytu + lstTable[j].AreaId + kytu + ')">' + lstTable[j].Count4 + '</a></td>';
        table += ' <td class="text-center">' + lstTable[j].Total + '</td>';
        table += ' </tr>';
    }
    $('#data_table').html(table);
    if ($('#areaChart').length > 0) {
        var data = [];
        for (var k = 0; k < lstArea.length; k++) {
            data.push(lstArea[k]);
        };
        Highcharts.chart('areaChart', {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
           
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: data,
                crosshair: true
            },
            yAxis: {
                allowDecimals: false,
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
                },
                series: {
                    maxPointWidth: 50
                }
            },
            series: [{
                name: lstChart[0].Lable,
                data: lstChart[0].Count

            }, {
                name: lstChart[1].Lable,
                data: lstChart[1].Count

            }, {
                name: lstChart[2].Lable,
                data: lstChart[2].Count
            }, {
                name: lstChart[3].Lable,
                data: lstChart[3].Count
            }]
        });
    }
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