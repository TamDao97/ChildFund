$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});
var _Type = '0';
var kytu = "'";
var modelSearch =
{
    DateFrom: '',
    DateTo: '',
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    Export: 0
};

function Refresh() {
    if (_Type === '2') {
        $('#wardId').val('');
    } else
    if (_Type === '1') {
        $('#districtId').val('');
        $('#wardId').val('');
    } else
    if (_Type === '0') {
        $('#districtId').val('');
        $('#provinceId').val('');
        $('#wardId').val('');
    }
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');
    
    Search(3);
}
function GetModelSearch() {
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
    modelSearch.ProvinceId = $('#provinceId').val();
    modelSearch.DistrictId = $('#districtId').val();
    modelSearch.WardId = $('#wardId').val()
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
    $.post("/StatisticByProcessing/GetReportByAbuse", modelSearch, function (result) {
        if (Export !== 0 && Export !== 3) {
            var link = document.getElementById('linkDowload');
            link.href = result.PathFile;
            link.download = GetNotifyByKey('Statistic_abuse') + (Export === 1 ? '.xlsx' : '.pdf');
            link.focus();
            link.click();
        }
        if (Export === 3) {
            $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
            $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') + '</option>');
        }
        modelSearch.Export = 0;
        CloseWaiting();
        GenChart(result.lstTable, result.lstChart);
    });
}
function GetDistrictInit(ProvinceId, DistrictId) {
    $.post("/Combobox/DistrictCBB?Id=" + ProvinceId, function (result) {
        $("#districtId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
        $('#districtId').val(DistrictId);
        Search(0);
        GetWard();
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
    if (modelSearch.DistrictId !== '') {
        $.post("/Combobox/WardCBB?Id=" + modelSearch.DistrictId, function (result) {
            $("#wardId").html('<option selected value="">' + GetNotifyByKey('All_Title') +'</option>' + result);
        });
    }
}

function GenChart(lstTable, lstChart) {
    var colors = ['#26C6DA', 'orange', '#a09e9e', '#00797c', '#84bd00', '#26C6DA'];
    var cateName = [];
    var lstData = [];
    var table = '';
    for (var j = 0; j < lstTable.length; j++) {
        cateName.push(lstTable[j].AbuseTypeName);
        table += ' <tr>';
        table += ' <td>' + lstTable[j].AbuseTypeName + '</td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '0' + kytu + ',' + kytu + lstTable[j].AbuseId + kytu + ')">' + lstTable[j].Count1 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '1' + kytu + ',' + kytu + lstTable[j].AbuseId + kytu + ')">' + lstTable[j].Count2 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '2' + kytu + ',' + kytu + lstTable[j].AbuseId + kytu + ')">' + lstTable[j].Count3 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '3' + kytu + ',' + kytu + lstTable[j].AbuseId + kytu + ')">' + lstTable[j].Count4 + '</a></td>';
        table += ' <td class="text-center"><a href="javascript:void(0)" onclick="ViewReportByStatus(' + kytu + '4' + kytu + ',' + kytu + lstTable[j].AbuseId + kytu + ')">' + lstTable[j].Count5 + '</a></td>';

        table += ' </tr>';
    }
    for (var i = 0; i < lstChart.length; i++) {
        lstData.push({ name: lstChart[i].Lable, data: lstChart[i].Count });
    }
    $('#data_table').html(table);
    if ($('#processingChart').length > 0) {
        Highcharts.chart('processingChart', {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            colors: colors,
            xAxis: {
                categories: cateName
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
            series: lstData
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

function ViewReportByStatus(valueStatus, valueAbuse) {
    var provinceCheck = $('#provinceId').val();
    var districtCheck = $('#districtId').val();
    var wardCheck = $('#wardId').val();

    sessionStorage.setItem("keyStatus", valueStatus);
    sessionStorage.setItem("keyAbuse", valueAbuse);
    sessionStorage.setItem("keyDate", $("#dateFromSearch").val() + ';' + $("#dateToSearch").val());
    if (provinceCheck !== '') {
        if (districtCheck !== '') {
            if (wardCheck !== '') {
                sessionStorage.setItem("keyAddressByUser", wardCheck + ";" + 3);
            } else {
                sessionStorage.setItem("keyAddressByUser", districtCheck + ";" + 2);
            }
        } else {
            sessionStorage.setItem("keyAddressByUser", provinceCheck + ";" + 1);
        }
    }
    window.location = '/ReportProfile/Index';
}