{
    var chart;
    function createChart(prices, idElement) {
        prices = JSON.parse(prices);
        var options = {
            series: [{
                data: prices
            }],
            chart: {
                type: 'candlestick',
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
        chart = new ApexCharts(document.querySelector(idElement), options);
        
        chart.render();
        console.log("Chart rendered");
    }
}