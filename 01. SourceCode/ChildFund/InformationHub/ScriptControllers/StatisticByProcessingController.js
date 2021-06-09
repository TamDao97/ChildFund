$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var kytu = "'";
var _Type = '0';
var modelSearch =
{
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    Export: 0

};

function Refresh() {
    if (_Type === '2') {
        $('#wardId').val('');
    }
    if (_Type === '1') {
        $('#districtId').val('');
        $('#wardId').val('');
    }
    if (_Type === '0') {
        $('#districtId').val('');
        $('#provinceId').val('');
        $('#wardId').val('');
    }

    Search(3);
}
function GetModelSearch() {
    modelSearch.ProvinceId = $('#provinceId').val();
    modelSearch.DistrictId = $('#districtId').val();
    modelSearch.WardId = $('#wardId').val();
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
    $.post("/StatisticByProcessing/ListReportProfileByProcessing", modelSearch, function (result) {
        if (Export !== 0 && Export!==3) {
            var link = document.getElementById('linkDowload');
            link.href = result.PathFile;
            link.download = GetNotifyByKey('Statistic_process') + (Export === 1 ? '.xlsx' : '.pdf');
            link.focus();
            link.click();
        }
        if (Export === 3) {
            $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
            $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
        }
        modelSearch.Export = 0;
        CloseWaiting();
        GenChart(result.lstChart, result.lstLocation, result.lstTableLeft, result.lstTableRight);
    });
}
function GetDistrictInit(ProvinceId, DistrictId) {
    $.post("/Combobox/DistrictCBB?Id=" + ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>' + result);
        $('#districtId').val(DistrictId);
        Search(0);
        GetWard();
    });
}

function GetDistrict() {
    modelSearch.ProvinceId = $('#provinceId').val();
    $.post("/Combobox/DistrictCBB?Id=" + modelSearch.ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>' + result);
        $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
    });
}
function GetWard() {
    modelSearch.DistrictId = $('#districtId').val();
    if (modelSearch.DistrictId !== '') {
        $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
            $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>' + result);
        });
    }
}

var colors = ['#26C6DA', 'orange', '#a09e9e', '#00797c', '#84bd00', '#26C6DA'];
function GenChart(lstChart, lstLocation, lstTableLeft, lstTableRight) {
    GenTableLeft(lstTableLeft);
    GenTableRight(lstTableRight);

    if ($('#processingStatusChart').length > 0) {
        var data = [];
        for (var k = 0; k < lstLocation.length; k++) {
            data.push(lstLocation[k]);
        };
        Highcharts.chart('processingStatusChart', {
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
function InitFuntion(Type, ProvinceId, DistrictId) {
    _Type = Type;
    if (Type === '1' || Type === '2') {
        $('#provinceId').val(ProvinceId);
        GetDistrictInit(ProvinceId, DistrictId);
        if (Type === '1') {
            document.getElementById("provinceId").disabled = true;
        } else if (Type === '2') {
            document.getElementById("provinceId").disabled = true;
            document.getElementById("districtId").disabled = true;
        }

    } else {
        Search(0);
    }
}

function GenTableLeft(lstTableLeft) {
    var tableLeft = '';
    for (var j = 0; j < lstTableLeft.length; j++) {
        tableLeft += ' <tr>';
        tableLeft += ' <td>' + lstTableLeft[j].LableName + '</td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '0' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count1 + '</a></td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '1' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count2 + '</a></td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '2' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count3 + '</a></td>';
        tableLeft += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '3' + kytu + ',' + kytu + lstTableLeft[j].AreaId + kytu + ')">' + lstTableLeft[j].Count4 + '</a></td>';
        tableLeft += ' </tr>';
    }
    $('#data_table_left').html(tableLeft);
}
function GenTableRight(lstTableRight) {
    var tableRight = '';
    for (var j = 0; j < lstTableRight.length; j++) {
        tableRight += ' <tr>';
        tableRight += ' <td>' + lstTableRight[j].LableName + '</td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '0' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count1 + '</a></td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '1' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count2 + '</a></td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '2' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count3 + '</a></td>';
        tableRight += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '3' + kytu + ',' + kytu + lstTableRight[j].AreaId + kytu + ')">' + lstTableRight[j].Count4 + '</a></td>';
        tableRight += ' </tr>';
    }
    $('#data_table_right').html(tableRight);
}

function ViewReportByStatus(value, AreaId) {

    var provinceCheck = $('#provinceId').val();
    var districtCheck = $('#districtId').val();
    var wardCheck = $('#wardId').val();

    sessionStorage.setItem("keyStatus", value);
    sessionStorage.setItem("keyDate", ';');

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