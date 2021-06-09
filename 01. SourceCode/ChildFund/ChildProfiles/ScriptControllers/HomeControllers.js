// danh sách hồ sơ mới cấp trung ương
var modelSearch =
{
    Year: '',
    DistrictId: '',
    ProvinceId: '',
    Export: 0
};

function GetModelSearch() {
    modelSearch.Year = $('#yearSearch').val();
    modelSearch.DistrictId = $('#districtIdSearch').val();
    modelSearch.ProvinceId = $('#provinceIdSearch').val();
}
var colorsc = ['#6EEDC9', '#0B7AD9'];
var colorsLearning = ['#0B7AD9', '#85D75C', '#EA7E01', '#B64AE2', '#6CECC7', '#A53711', '#419249'];
var colorsNation = ['#0B7AD9', '#85D75C', '#EA7E01', '#B64AE2', '#6CECC7', '#A53711', '#419249'];
function Search(vl) {
    modelSearch.Export = vl;
    $('#titleProfiles').html('Child profiles in ' + $('#yearSearch').val());
    $('#titleProfilesChart').html(' Child profile data in ' + $('#yearSearch').val());
    GetModelSearch();
    $('#loader_id').addClass("loader");
    document.getElementById("overlay").style.display = "block";
    $.post("/Homes/GetData", modelSearch)
        .done(function (result) {
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
            if (result.ok === true) {
                GenTable(result.dataLearning, result.itemGender);
                GenLearning(result.dataLearning);
                GenAge(result.lstAgegModel);
                GenNation(result.dataNation);
                GenGender(result.dataGender);
                GenChartRight(result.lstprofile, result.lstprofileConfim, result.lstprofileUnConfim);
                $('#number_counter_new').html(result.dataCount.countProfile);
                $('#number_counter_unconfim').html(result.dataCount.countUnConfim);
                $('#number_counter_confim').html(result.dataCount.countConfim);
                if (modelSearch.Export !== 0 && result.fileUrl !== '') {
                    modelSearch.Export = 0;
                    var link = document.getElementById('linkDowload');
                    link.href = result.fileUrl;
                    link.download = 'CHILD-PROFILE-DATA.xlsx';
                    link.focus();
                    link.click();
                }
            }
        })
        .fail(function (xhr, status, error) {
            $('#loader_id').removeClass("loader");
            document.getElementById("overlay").style.display = "none";
        });
}

function GenChartRight(lstprofile, lstprofileConfim, lstprofileUnConfim) {
    var title = 'Thống kê hồ sơ trẻ năm ' + $('#yearSearch').val() + '/Record statistic in ' + $('#yearSearch').val();
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
            text: ''
        },
        xAxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Number of child profiles'
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }
        },
        colors: colorsc,
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
                },
            }
        },
        series: [{
            name: 'Đã duyệt/Approved',
            data: lstprofileConfim
        }, {
            name: 'Chờ duyệt/Waiting for approval',
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
            text: ''
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
                    return '<b>' + this.point.name + ':</b> ' +
                        this.y + '%';
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
    var title = 'Tỉ lệ hồ sơ theo xã';
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
            text: ''
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
function GenGender(list) {
    var title = 'Tỉ lệ hồ sơ theo giới tính';
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
    Highcharts.chart('chartGender', {
        chart: {
            type: 'pie'
        },
        title: {
            text: ''
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
function GenAge(list) {

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
    Highcharts.chart('chartAge', {
        chart: {
            type: 'pie'
        },
        title: {
            text: ''
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
                    return '<b>' + this.point.name + ':</b> ' +
                        this.y + '%';
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
        $("#districtIdSearch").html('<option value="">Tất cả/All</option>' + result);
        Search(0);
    });
}
function ChangeProvince() {
    $.post("/Combobox/GetAreaDistrictHome?Id=" + $('#provinceIdSearch').val(), function (result) {
        $("#districtIdSearch").html('<option value="">Tất cả/All</option>' + result);
    });
}
function ChangeProvinceByUser() {
    $.post("/Combobox/DistrictByUser?Id=" + $('#provinceIdSearch').val(), function (result) {
        $("#districtIdSearch").html('<option value="">Tất cả/All</option>' + result);
    });
}
function GenTable(list, Gender) {
    $('#tr-giaoduc').attr("colspan", list.length);
    var htmlHeader = '';
    var htmlRow = ' <td>Hồ sơ/Profile</td>';
    for (var i = 0; i < list.length; i++) {
        htmlHeader += '<th width="130px" class="text-center">' + list[i].Name + '</th>';
        htmlRow += '<td class="text-center">' + list[i].Count + '</td>';
    }
    htmlHeader += '<th width="120px" class="text-center">Nam/Male</th>';
    htmlHeader += '<th width="120px" class="text-center">Nữ/Female</th>';
    htmlRow += '<td class="text-center">' + Gender.Male + '</td>';
    htmlRow += '<td class="text-center">' + Gender.FeMale + '</td>';
    $('#dataHeader').html(htmlHeader);
    $('#dataRow').html(htmlRow);
}