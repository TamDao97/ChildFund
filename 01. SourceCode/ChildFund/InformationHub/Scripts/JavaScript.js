$(function () {
    "use strict";
    var colorsHS = ['#8085e9', '#8085e9', '#8085e9', '#8085e9', '#e4d354', '#e4d354', '#f15c80', '#f15c80'];
    if ($('#thongkeHS').length > 0) {


        Highcharts.chart('thongkeHS', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Thống kê hồ sơ trẻ năm 2018 - Trùng Khánh,Cao Bằng'
            },
            xAxis: {
                categories: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Tổng số hồ sơ'
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
                name: 'Tạo mới',
                data: [5, 3, 4, 7, 2, 1, 7, 5, 8, 4, 2, 2]
            }, {
                name: 'Cập nhật',
                data: [2, 2, 3, 2, 1, 1, 7, 5, 8, 4, 2, 2]
            }, {
                name: 'Đã duyệt',
                data: [3, 4, 4, 2, 5, 1, 7, 5, 8, 4, 2, 2]
            }]
        });
    }

    var colors = Highcharts.getOptions().colors,
    categories = [
      "Không đi học",
      "Trẻ mầm non",
      "Học tiểu học",
      "Học trung học" 
    ],
    data = [
      {
          "y": 60,
          "color": colors[2]
      },
      {
          "y": 10,
          "color": colors[1]
      },
      {
          "y": 7.23,
          "color": colors[0]
      },
      {
          "y": 5.58,
          "color": colors[3]
      } 
    ],
    browserData = [],
    i,
    j,
    dataLen = data.length,
    drillDataLen,
    brightness;


    // Build the data arrays
    for (i = 0; i < dataLen; i += 1) {

        // add browser data
        browserData.push({
            name: categories[i],
            y: data[i].y,
            color: data[i].color
        });
    }

});