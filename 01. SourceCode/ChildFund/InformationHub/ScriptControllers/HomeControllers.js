// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    Year: '',
    DistrictId: '',
    ProvinceId: ''
};

function GetModelSearch() {
    modelSearch.Year = $('#yearSearch').val();
    modelSearch.DistrictId = $('#districtIdSearch').val();
    modelSearch.ProvinceId = $('#provinceIdSearch').val();
}
var colorsLearning = ['#8085e9', '#e4d354', '#f15c80', '#FFCC66', '#66CC66', '#FF9966'];
var colorsNation = ['#FF9966', '#f15c80', '#e4d354', '#8085e9', '#66CC66', '#FFCC66'];
function Search() {
    $('#titleProfiles').html('Hồ sơ trẻ năm ' + $('#yearSearch').val());
    $('#titleProfilesChart').html('Thống kê hồ sơ trẻ năm ' + $('#yearSearch').val());
    GetModelSearch();
    OpenWaiting();
    $.post("/Homes/GetData", modelSearch, function (result) {
        CloseWaiting();
        if (result.ok === true) {
            GenTable(result.dataLearning, result.itemGender);
            GenLearning(result.dataLearning);
            GenNation(result.dataNation);
            GenChartRight(result.lstprofile, result.lstprofileConfim, result.lstprofileUnConfim);
            $('#number_counter_new').html(result.dataCount.countProfile);
            $('#number_counter_unconfim').html(result.dataCount.countUnConfim);
            $('#number_counter_confim').html(result.dataCount.countConfim);
        }
      
    });
}

function GenChartRight(lstprofile, lstprofileConfim, lstprofileUnConfim) {
    var title = 'Thống kê hồ sơ trẻ năm ' + $('#yearSearch').val();
    if (modelSearch.DistrictId !== '' || modelSearch.ProvinceId !== '') {
        title += '- ';
        if (modelSearch.DistrictId !== '') {
            title += $('#districtIdSearch').find('option:selected').text() + ', ';
        }
        if (modelSearch.ProvinceId !== '') {
            title += $('#provinceIdSearch').find('option:selected').text();
        }
    }
    Highcharts.chart('thongkeHS', {
        chart: {
            type: 'column'
        },
        title: {
            text: title
        },
        xAxis: {
            categories: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Số hồ sơ'
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
            x: -30,
            verticalAlign: 'top',
            y: 25,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        tooltip: {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: '{series.name}: {point.y}<br/>'
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                }
            }
        },
        series: [ {
            name: 'Đã duyệt',
            data: lstprofileConfim
        }, {
            name: 'Chưa duyệt',
            data: lstprofileUnConfim
        }]
    });

}
function GenLearning(list) {
    var title = 'Tỉ lệ hồ sơ trẻ đi học ';
    if (modelSearch.DistrictId !== '' || modelSearch.ProvinceId !== '') {
        title += '- ';
        if (modelSearch.DistrictId !== '') {
            title += $('#districtIdSearch').find('option:selected').text() + ', ';
        }
        if (modelSearch.ProvinceId !== '') {
            title += $('#provinceIdSearch').find('option:selected').text();
        }
    }

    var browserData = [];
    // Build the data arrays
    for (i = 0; i < list.length; i++) {
        // add browser data
        browserData.push({
            name: list[i].Name,
            y: list[i].Percen,
            color: colorsLearning[i]
        });
    }
    // Create the chart
    Highcharts.chart('chartLearning', {
        chart: {
            type: 'pie'
        },
        title: {
            text: title
        },
        subtitle: {
            text: ''
        },
        yAxis: {
            title: {
                text: 'Total percent market share'
            }
        },
        plotOptions: {
            pie: {
                shadow: false,
                center: ['50%', '50%']
            }
        },
        tooltip: {
            valueSuffix: '%'
        },
        series: [{
            name: 'Tỉ lệ',
            data: browserData,
            size: '60%',
            innerSize: '40%',
            dataLabels: {
                formatter: function () {
                    // display only if larger than 1
                    return  '<b>' + this.point.name + ':</b> ' +
                        this.y + '%' ;
                }
            },
            id: 'versions'
        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 400
                },
                chartOptions: {
                    series: [{
                        id: 'versions',
                        dataLabels: {
                            enabled: false
                        }
                    }]
                }
            }]
        }
    });
}
function GenNation(list) {
    var title = 'Tỉ lệ hồ sơ dân tộc trẻ ';
    if (modelSearch.DistrictId !== '' || modelSearch.ProvinceId !== '') {
        title += '- ';
        if (modelSearch.DistrictId !== '') {
            title += $('#districtIdSearch').find('option:selected').text() + ', ';
        }
        if (modelSearch.ProvinceId !== '') {
            title += $('#provinceIdSearch').find('option:selected').text();
        }
    }
    var browserData = [];
    // Build the data arrays
    for (i = 0; i < list.length; i++) {
        // add browser data
        browserData.push({
            name: list[i].Name,
            y: list[i].Percen,
            color: colorsNation[i]
        });
    }
    // Create the chart
    Highcharts.chart('chartNation', {
        chart: {
            type: 'pie'
        },
        title: {
            text: title
        },
        subtitle: {
            text: ''
        },
        yAxis: {
            title: {
                text: 'Total percent market share'
            }
        },
        plotOptions: {
            pie: {
                shadow: false,
                center: ['50%', '50%']
            }
        },
        tooltip: {
            valueSuffix: '%'
        },
        series: [{
            name: 'Tỉ lệ',
            data: browserData,
            size: '60%',
            innerSize: '40%',
            dataLabels: {
                formatter: function () {
                    // display only if larger than 1
                    return this.y > 1 ? '<b>' + this.point.name + ':</b> ' +
                        this.y + '%' : null;
                }
            },
            id: 'versions'
        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 400
                },
                chartOptions: {
                    series: [{
                        id: 'versions',
                        dataLabels: {
                            enabled: false
                        }
                    }]
                }
            }]
        }
    });
}
function SearchByUser() {
    $.post("/Combobox/DistrictByUser?Id=" + $('#provinceIdSearch').val(), function (result) {
        $("#districtIdSearch").html('<option value="">Tất cả</option>' + result);
        Search();
    });
}
function ChangeProvince() {
    $.post("/Combobox/GetAreaDistrictHome?Id=" + $('#provinceIdSearch').val(), function (result) {
        $("#districtIdSearch").html('<option value="">Tất cả</option>' + result);
    });
}
function ChangeProvinceByUser() {
    $.post("/Combobox/DistrictByUser?Id=" + $('#provinceIdSearch').val(), function (result) {
        $("#districtIdSearch").html('<option value="">Tất cả</option>' + result);
    });
}
function GenTable(list, Gender) {
    $('#tr-giaoduc').attr("colspan", list.length);
    var htmlHeader = '';
    var htmlRow = ' <td>Hồ sơ</td>';
    for (var i = 0; i < list.length; i++) {
        htmlHeader += '<th width="130px" class="text-center">' + list[i].Name + '</th>';
        htmlRow += '<td class="text-center">' + list[i].Count + '</td>';
    }
    htmlHeader += '<th width="120px" class="text-center">Nam</th>';
    htmlHeader += '<th width="120px" class="text-center">Nữ</th>';
    htmlRow += '<td class="text-center">' + Gender.Male + '</td>';
    htmlRow += '<td class="text-center">' + Gender.FeMale + '</td>';
    $('#dataHeader').html(htmlHeader);
    $('#dataRow').html(htmlRow);
}