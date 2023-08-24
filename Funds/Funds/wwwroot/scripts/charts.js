function createChart(prices, idElement) {
    var chartToDelete = document.getElementById("apexchartstock");
    if (!(chartToDelete == null && chartToDelete == undefined))
        chartToDelete.remove();
    console.log(prices);
    try {
        prices = JSON.parse(prices);
        var options = {
            series: [{
                data: prices
            }],
            chart: {
                type: 'candlestick',
                id: 'tock'
            },
            xaxis: {
                type: 'datetime'
            },
            yaxis: {
                tooltip: {
                    enabled: true
                }
            }
        };
        var chart = new ApexCharts(document.getElementById(idElement), options);

        chart.render();
        console.log("Chart rendered");
    } catch (exception) {}
    }
    
    