$(function () {
    var counter = 1;
    $("#addrow").on("click", function () {
        counter++;

        var newRow = $("<tr>");
        var cols = "";

        cols += '<td>' + counter +'</td>';
        cols += '<td><input type="text" class="form-control" name="code' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="name' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="mail' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="phone' + counter + '"/></td>';

        cols += '<td><button type="button" class="btn btn-xs btn-danger ibtnDel"><i class="material-icons">delete</i></button></td>';
        newRow.append(cols);
        $("table#tableActivity").append(newRow);
    });



    $("table#tableActivity").on("click", ".ibtnDel", function (event) {
        $(this).closest("tr").remove();
        counter -= 1
    });

    "use strict";
    var colors = ['#8085e9', '#e4d354', '#f15c80', '#84bd00','#26C6DA'];
    if ($('#containerNow').length > 0) {
        Highcharts.chart('containerNow', {
            chart: {
                type: 'pie',
                style: {
                    fontFamily: "'Source Sans Pro', Arial, Helvetica, sans-serif"
                }
            },
            title: {
                text: 'Tỷ lệ theo loại hình xâm hại'
            },
            //plotOptions: {
            //    pie: {
            //        innerSize: 100,
            //        depth: 45
            //    }
            //},
            colors: colors,
            series: [{
                name: 'Delivered amount',
                data: [
                    ['Thể chất', 8],
                    ['Tình dục', 3],
                    ['Tinh thần', 3],
                    ['Xao nhãng', 1]
                ]
            }]
        });
    }

    if ($('#containerLevelNow').length > 0) {
        Highcharts.chart('containerLevelNow', {
            chart: {
                type: 'pie',
                style: {
                    fontFamily: "'Source Sans Pro', Arial, Helvetica, sans-serif"
                }
            },
            colors: ['#ef5350', '#fdb45d','#9E9E9E'],
            title: {
                text: 'Tỷ lệ theo mức độ khẩn cấp'
            },
            series: [{
                name: 'Delivered amount',
                data: [
                    ['Rất khẩn cấp', 1],
                    ['Khẩn cấp', 4],
                    ['Cần lưu ý', 2]
                ]
            }]
        });
    }

    if ($('#containerXLNow').length > 0) {
        Highcharts.chart('containerXLNow', {
            chart: {
                type: 'pie',
                style: {
                    fontFamily: "'Source Sans Pro', Arial, Helvetica, sans-serif"
                }
            },
            colors: colors,
            title: {
                text: 'Tỷ lệ tiến độ xử lý'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: false,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            series: [{
                name: 'Delivered amount',
                data: [
                    ['Chưa xử lý', 1],
                    ['Đang điều tra xác minh', 4],
                    ['Đã xử lý', 2],
                    ['Cần theo dõi thêm', 2],
                    ['Đóng trường hợp', 2]
                ]
            }]
        });
    }


    if ($('#container').length > 0) {
        Highcharts.chart('container', {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            colors: colors,
            xAxis: {
                categories: [
                    '17/11',
                    '18/11',
                    '19/11',
                    '20/11',
                    '21/11',
                    '22/11',
                    '23/11',
                    '24/11',
                    '25/11',
                    '26/11',
                    '27/11',
                    '28/11'
                ],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Sự vụ (ca)'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
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
                name: 'Thể chất',
                data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

            }, {
                name: 'Tình dục',
                data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

            }, {
                name: 'Tinh thần',
                data: [38.9, 28.8, 19.3, 11.4, 37.0, 28.3, 39.0, 29.6, 42.4, 35.2, 29.3, 11.2]

                }, {
                    name: 'Xao nhãng',
                    data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3, 51.2]

                }]
        });
    }

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
                categories: [
                    'Tháng 1',
                    'Tháng 2',
                    'Tháng 3',
                    'Tháng 4',
                    'Tháng 5',
                    'Tháng 6',
                    'Tháng 7',
                    'Tháng 8',
                    'Tháng 9',
                    'Tháng 10',
                    'Tháng 11',
                    'Tháng 12'
                ],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Sự vụ (ca)'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
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
                name: 'Chưa xử lý',
                data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

            }, {
                name: 'Đang điều tra xác minh',
                data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

            }, {
                name: 'Đã xử lý',
                data: [38.9, 28.8, 19.3, 11.4, 37.0, 28.3, 39.0, 29.6, 42.4, 35.2, 29.3, 11.2]

            }, {
                name: 'Cần theo dõi thêm',
                data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3, 51.2]

                }, {
                    name: 'Đóng sự vụ',
                    data: [18.9, 28.8, 19.3, 21.4, 17.0, 28.3, 39.0, 29.6, 42.4, 35.2, 39.3, 21.2]

                }]
        });
    }

    if ($('#thongkeCapTren').length > 0) {
        Highcharts.chart('thongkeCapTren', {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            colors: colors,
            xAxis: {
                categories: [
                    'Xã Phú Minh',
                    'Xã Phú An',
                    'Xã Phú Hương',
                    'Xã Phú Liên',
                    'Xã Phú Triều',
                    'Xã Phú 1',
                    'Xã Phú 2',
                    'Xã Phú 3',
                    'Xã Phú 4',
                    'Xã Phú 5',
                    'Xã Phú 6',
                    'Xã Phú 7'
                ],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Sự vụ (ca)'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
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
                name: 'Bạo lực',
                data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

            }, {
                name: 'Xâm hại tình dục',
                data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

            }, {
                name: 'Bỏ đói',
                data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3, 51.2]

            }]
        });
    }

    if ($('#baocaoQuanLy').length > 0) {

        Highcharts.chart('baocaoQuanLy', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Biểu Đồ Thống Kê'
            },
            colors: colors,
            xAxis: {
                categories: ['Chưa xử lý', 'Đang xử lý', 'Theo dõi sau đóng ca', 'Đóng ca']
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Ca sựu vụ'
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
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
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
            series: [{
                name: 'Bạo lực',
                data: [5, 3, 4, 2]
            }, {
                name: 'Xâm hại tình dục',
                data: [2, 2, 3, 1]
            }, {
                name: 'Bỏ đói',
                data: [3, 4, 4, 5]
            }]
        });
    }

    if ($('#thongkeViTri').length > 0) {

        Highcharts.chart('thongkeViTri', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Biểu Đồ Thống Kê'
            },
            colors: colors,
            xAxis: {
                categories: ['TP.Hà Nội', 'TP.Đà Nẵng', 'TP.Hồ Chí Minh', 'Hòa Bình','Sơn La','Cao Bằng','...','Cà Mau']
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Ca sựu vụ'
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
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
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
            series: [{
                name: 'Bạo lực',
                data: [500, 312, 412, 250,357,267,432,610]
            }, {
                name: 'Xâm hại tình dục',
                data: [211, 222, 350, 170,120,215,610,704]
            }, {
                name: 'Bỏ đói',
                data: [351, 420, 410, 512,321,519,609,901]
            }]
        });
    }

    var colorsHS = ['#8085e9', '#8085e9', '#8085e9', '#8085e9', '#e4d354', '#e4d354', '#f15c80', '#f15c80'];
    if ($('#thongkeHS').length > 0) {

        // Create the chart
        Highcharts.chart('thongkeHS', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Hồ sơ hỗ trợ trẻ em năm 2018'
            },
            colors: colorsHS,
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'Tổng hồ sơ trẻ'
                }

            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.1f}%'
                    }
                }
            },

            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
            },

            "series": [
                {
                    "name": "Browsers",
                    "colorByPoint": true,
                    "data": [
                        {
                            "name": "Trẻ không đi học",
                            "y": 62.74
                        },
                        {
                            "name": "Trẻ mầm non",
                            "y": 10.57
                        },
                        {
                            "name": "Trẻ học tiểu học",
                            "y": 7.23
                        },
                        {
                            "name": "Trẻ học trung học",
                            "y": 5.58
                        },
                        {
                            "name": "Giới tính Nam",
                            "y": 14.02
                        },
                        {
                            "name": "Giới tính Nữ",
                            "y": 21.92
                        },
                        {
                            "name": "Dân tộc Kinh",
                            "y": 7.62
                        },
                        {
                            "name": "Dân tộc tiểu số",
                            "y": 30.62
                        }
                    ]
                }
            ]
        });
    }
});