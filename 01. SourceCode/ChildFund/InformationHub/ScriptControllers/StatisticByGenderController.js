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
    Export: 0
};
function Refresh(WardId, DistrictId, ProvinceId, DateTimeNow, YearNow) {
    $('#provinceId').val('');
    $('#districtId').val('');
    $('#wardId').val('');
    $('#dateFromSearch').val('01/01/' + YearNow);
    $('#dateToSearch').val(DateTimeNow);
    SearchInit(WardId, DistrictId, ProvinceId, DateTimeNow, YearNow);
}
function GetModelSearch() {
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
    modelSearch.WardId = $('#wardId').val();
    modelSearch.DistrictId = $('#districtId').val();
    modelSearch.ProvinceId = $('#provinceId').val();
}
function SearchInit(WardId, DistrictId, ProvinceId, DateTimeNow, YearNow) {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    modelSearch.ProvinceId = ProvinceId;
    modelSearch.DistrictId = DistrictId;
    modelSearch.WardId = WardId;
    modelSearch.DateFrom = '01/01/' + YearNow;
    modelSearch.DateTo = DateTimeNow;
    $("#titleLeftChart").html(GetNotifyByKey('StatisticByTime') + ' ' + modelSearch.DateFrom + ' - ' + modelSearch.DateTo);
    OpenWaiting();
    $.post("/StatisticByGender/ListReportProfileByGender", modelSearch, function (result) {
        $("#data_table").html(result);
        CloseWaiting();
        GenChart(result.lstTable, result.lstChart, result.lstAbuse);
        GetDistrict(ProvinceId);
        GetWard(DistrictId);
    });
}
function Search(Export, ProvinceId, DistrictId, WardId, Type) {
    modelSearch.Export = Export;
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    if (Type == 2) {
        modelSearch.DistrictId = DistrictId;
    }
    if (Type == 1) {
        modelSearch.ProvinceId = ProvinceId;
    }
    if (Type == 3) {
        modelSearch.WardId = WardId;
    }
    $("#titleLeftChart").html(GetNotifyByKey('StatisticByTime') + ' ' + modelSearch.DateFrom + ' - ' + modelSearch.DateTo);
    OpenWaiting();
    $.post("/StatisticByGender/ListReportProfileByGender", modelSearch, function (result) {
        $("#data_table").html(result);
        CloseWaiting();
        GenChart(result.lstTable, result.lstChart, result.lstAbuse);
        if (Export !== 0) {
            var link = document.getElementById('linkDowload');
            link.href = result.PathFile;
            link.download = GetNotifyByKey('Statistic_gender') + (Export === 1 ? '.xlsx' : '.pdf');
            link.focus();
            link.click();
        }
        modelSearch.Export = 0;
    });
}

function GetDistrict(ProvinceId) {
    modelSearch.ProvinceId = ProvinceId;
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
    });
}
function GetWard(DistrictId) {
    modelSearch.DistrictId = DistrictId;
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
    });
}

function GetDistrictByUser() {
    modelSearch.ProvinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>');
    });
}
function GetWardByUser() {
    modelSearch.DistrictId = $('#districtId').val();
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
    });
}

var colors = ['#26C6DA', 'orange', '#a09e9e', '#00797c', '#84bd00', '#26C6DA'];
function GenChart(lstTable, lstChart, lstAbuse) {
    var table = '';
    for (var j = 0; j < lstTable.length; j++) {
        table += ' <tr>';
        table += ' <td>' + lstTable[j].Type + '</td>';
        table += ' <td class="text-center" style="width: 150px;"><a href="javascript:void(0)" onclick="ViewReportByGender(' + kytu + lstTable[j].TypeId + kytu + ',1)">' + lstTable[j].CountNam + '</a></td>';
        table += ' <td class="text-center" style="width: 150px;"><a href="javascript:void(0)" onclick="ViewReportByGender(' + kytu + lstTable[j].TypeId + kytu + ',2)">' + lstTable[j].CountNu + '</a></td>';
        table += ' <td class="text-center" style="width: 150px;"><a href="javascript:void(0)" onclick="ViewReportByGender(' + kytu + lstTable[j].TypeId + kytu + ',0)">' + lstTable[j].CountKhong + '</a></td>';
        table += ' </tr>';
    }
    $('#data_table').html(table);
    if ($('#genderChart').length > 0) {
        Highcharts.chart('genderChart', {
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
                categories: [
                    lstAbuse[0],
                    lstAbuse[1],
                    lstAbuse[2],
                    lstAbuse[3],
                    lstAbuse[4],
                ],
                crosshair: true
            },
            yAxis: {
                allowDecimals: false,
                min: 0,
                title: {
                    text: GetNotifyByKey('Number_case')
                }
            },
            colors: colors,
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
                name: lstChart[0].Label,
                data: lstChart[0].Count

            }, {
                name: lstChart[1].Label,
                data: lstChart[1].Count

            }, {
                name: lstChart[2].Label,
                data: lstChart[2].Count

            }]
        });
    }
}
function ViewReportByGender(value, Gender) {
    var provinceCheck = $('#provinceId').val();
    var districtCheck = $('#districtId').val();
    var wardCheck = $('#wardId').val();

    sessionStorage.setItem("keyAbuse", value);
    sessionStorage.setItem("keyGender", Gender);
    sessionStorage.setItem("keyDate", $("#dateFromSearch").val() + ';' + $("#dateToSearch").val());

    if (provinceCheck !== '') {
        if (districtCheck !== '') {
            if (wardCheck !== '') {
                sessionStorage.setItem("keyAddressByUser", $('#wardId').val() + ";" + 3);
            }
            else {
                sessionStorage.setItem("keyAddressByUser", $('#districtId').val() + ";" + 2);
            }
        }
        else {
            sessionStorage.setItem("keyAddressByUser", $('#provinceId').val() + ";" + 1);
        }
    }
    window.location = '/ReportProfile/Index';
}
