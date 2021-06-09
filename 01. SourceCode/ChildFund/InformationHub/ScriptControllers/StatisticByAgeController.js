$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var kytu = "'";
var dateNow = new Date();
var yearNow = dateNow.getFullYear();
var fromYear = yearNow - 9;
var type = '';
var province = '';
var district = '';
var ward = '';

var modelSearch =
{
    FromYear: '',
    ToYear: '',
    ClickYear: '',
    WardId: '',
    DistrictId: '',
    ProvinceId: '',
    Export: 0
};

function Refresh(WardId, DistrictId, ProvinceId) {
    $('#provinceId').val('');
    $('#districtId').val('');
    $('#wardId').val('');
    $('#fromYear').val() = fromYear;
    $('#toYear').val() = yearNow;
    $('#clickYear').val() = yearNow;
    SearchInit(WardId, DistrictId, ProvinceId, type);
}
function GetModelSearch() {
    modelSearch.FromYear = $('#fromYear').val();
    modelSearch.ToYear = $('#toYear').val();
    modelSearch.WardId = $('#wardId').val();
    modelSearch.DistrictId = $('#districtId').val();
    modelSearch.ProvinceId = $('#provinceId').val();
    modelSearch.ClickYear = $('#clickYear').val();
}
function InitModel() {
    $('#fromYear').val(fromYear.toString());
    $('#toYear').val(yearNow.toString());
}
InitModel();
function SearchInit(WardId, DistrictId, ProvinceId, Type) {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    type = Type;
    province = ProvinceId;
    district = DistrictId;
    ward = WardId;
    modelSearch.ProvinceId = ProvinceId;
    modelSearch.DistrictId = DistrictId;
    modelSearch.WardId = WardId;
    modelSearch.FromYear = fromYear;
    modelSearch.ToYear = yearNow;
    modelSearch.ClickYear = yearNow;
    $("#titleLeftChart").html(GetNotifyByKey('StatisticByYear') + ' ' + fromYear + ' - ' + yearNow);
    $("#titleRightChart").html(GetNotifyByKey('StatisticByYear') + ' ' + modelSearch.ClickYear);
    OpenWaiting();
    $.post("/StatisticByAge/ListReportProfileByAge", modelSearch, function (result) {
        $("#data_table").html(result);
        CloseWaiting();
        GenChart(result.listTable, result.ageValue, result.chartData);
        GetDistrict(modelSearch.ProvinceId);
        GetWard(modelSearch.DistrictId);
        GenChartRight(result.chartRightData);
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
    $("#titleLeftChart").html(GetNotifyByKey('StatisticByYear') + ' ' + fromYear + ' - ' + yearNow);
    $("#titleRightChart").html(GetNotifyByKey('StatisticByYear') + ' ' + modelSearch.ClickYear);
    OpenWaiting();
    $.post("/StatisticByAge/ListReportProfileByAge", modelSearch, function (result) {
        CloseWaiting();
        GenChart(result.listTable, result.ageValue, result.chartData);
        GenChartRight(result.chartRightData);
        if (Export !== 0) {
            var link = document.getElementById('linkDowload');
            link.href = result.PathFile;
            link.download = GetNotifyByKey('Statistic_age') + (Export === 1 ? '.xlsx' : '.pdf');
            link.focus();
            link.click();
        }
        modelSearch.Export = 0;
    });
}

function GetDistrict(ProvinceId) {
    modelSearch.ProvinceId = ProvinceId;
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>' + result);
    });
}
function GetWard(DistrictId) {
    modelSearch.DistrictId = DistrictId;
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>' + result);
    });
}

function GetDistrictByUser() {
    modelSearch.ProvinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>' + result);
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
    });
}
function GetWardByUser() {
    modelSearch.DistrictId = $('#districtId').val();
    $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>' + result);
    });
}

var colors = ['#8085e9', 'orange', '#a09e9e', 'gold', '#84bd00', '#26C6DA', 'pink', '#8FBC8F', '#F08080'];
function GenChart(listTable, ageValue, chartData) {
    var table = '';
    for (var j = 0; j < listTable.length; j++) {
        table += ' <tr>';
        table += ' <td><a href="javascript:void(0)" onclick="LoadRightChart(' + kytu + listTable[j].LableName + kytu + ')">' + listTable[j].LableName + '</td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[0].AgeValue + kytu + ')">' + listTable[j].Count1 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[1].AgeValue + kytu + ')">' + listTable[j].Count2 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[2].AgeValue + kytu + ')">' + listTable[j].Count3 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[3].AgeValue + kytu + ')">' + listTable[j].Count4 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[4].AgeValue + kytu + ')">' + listTable[j].Count5 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[5].AgeValue + kytu + ')">' + listTable[j].Count6 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[6].AgeValue + kytu + ')">' + listTable[j].Count7 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByAge(' + kytu + ageValue[7].AgeValue + kytu + ')">' + listTable[j].Count8 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="javascript:void">' + listTable[j].Count9 + '</a></td>';
        table += ' </tr>';
    }
    $('#data_table').html(table);

    var mang = [];
    for (var i = 0; i < chartData.length; i++) {
        mang.push([chartData[i].Age, chartData[i].Percent]);
    }
    if ($('#ageLeftChart').length > 0) {
        Highcharts.chart('ageLeftChart', {
            chart: {
                type: 'pie',
                style: {
                    fontFamily: "'Source Sans Pro', Arial, Helvetica, sans-serif"
                }
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            legend: {
                itemWidth: 150,
                align: "center",
            },
            title: {
                text: ''
            },
            colors: colors,
            series: [{
                name: GetNotifyByKey('Percentage'),
                data: mang
            }]
        });
    }
}
function ViewReportByAge(value) {
    var provinceCheck = $('#provinceId').val();
    var districtCheck = $('#districtId').val();
    var wardCheck = $('#wardId').val();

    sessionStorage.setItem("keyAge", value);

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

function LoadRightChart(year) {
    $('#clickYear').val(year);
    $("#titleRightChart").html(GetNotifyByKey('StatisticByYear') + ' ' + modelSearch.ClickYear);
    GetModelSearch();
    if (type == 2) {
        modelSearch.DistrictId = district;
    }
    if (type == 1) {
        modelSearch.ProvinceId = province;
    }
    if (type == 3) {
        modelSearch.WardId = ward;
    }
    modelSearch.ClickYear = year;
    $.post("/StatisticByAge/GetRighChart", modelSearch, function (result) {
        GenChartRight(result.chartRightData);
    });
}

function GenChartRight(chartRightData) {
    var mang = [];
    for (var i = 0; i < chartRightData.length; i++) {
        mang.push([chartRightData[i].Age, chartRightData[i].Percent]);
    }
    if ($('#ageRightChart').length > 0) {
        Highcharts.chart('ageRightChart', {
            chart: {
                type: 'pie',
                style: {
                    fontFamily: "'Source Sans Pro', Arial, Helvetica, sans-serif"
                }
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            title: {
                text: ''
            },
            legend: {
                itemWidth: 150,
                align: "center",
            },
            colors: colors,
            series: [{
                name: GetNotifyByKey('Percentage'),
                data: mang
            }]
        });
    }
}
