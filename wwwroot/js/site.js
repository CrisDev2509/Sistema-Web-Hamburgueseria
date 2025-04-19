document.addEventListener("DOMContentLoaded", function () {
    //Charts
    var chartDom = document.getElementById('chart');
    if (chartDom != null) {
        var myChart = echarts.init(chartDom);
        var option;

        fetch('/Home/getData')
            .then(response => response.json())
            .then(data => {
                // Configuración de datos y opciones para el gráfico
                let dates = data.map(item => item.date);
                let values = data.map(item => item.value);

                option = {
                    tooltip: {
                        trigger: 'axis',
                        position: function (pt) {
                            return [pt[0], '10%'];
                        }
                    },
                    title: {
                        left: 'center',
                        text: 'Estdisticas de ventas'
                    },
                    toolbox: {
                        feature: {
                            dataZoom: {
                                yAxisIndex: 'none'
                            },
                            restore: {},
                            saveAsImage: {}
                        }
                    },
                    xAxis: {
                        type: 'category',
                        boundaryGap: false,
                        data: dates
                    },
                    yAxis: {
                        type: 'value',
                        boundaryGap: [0, '100%']
                    },
                    dataZoom: [
                        {
                            type: 'inside',
                            start: 75,
                            end: 100
                        },
                        {
                            start: 0,
                            end: 10
                        }
                    ],
                    series: [
                        {
                            name: 'Total ventas',
                            type: 'line',
                            symbol: 'none',
                            sampling: 'lttb',
                            itemStyle: {
                                color: 'rgb(255, 70, 131)'
                            },
                            areaStyle: {
                                color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                                    {
                                        offset: 0,
                                        color: 'rgb(255, 158, 68)'
                                    },
                                    {
                                        offset: 1,
                                        color: 'rgb(255, 70, 131)'
                                    }
                                ])
                            },
                            data: values
                        }
                    ]
                };

                myChart.setOption(option);
        })
        .catch(error => {
            console.log(error);
        });
    }
});
