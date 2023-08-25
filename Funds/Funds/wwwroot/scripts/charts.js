function createChart(prices, idElement) {
    var chartToDelete = document.getElementById("apexchartstock");
    if (!(chartToDelete == null && chartToDelete == undefined))
        chartToDelete.remove();
    try {
        prices = JSON.parse(prices);
        var options = {
            series: [{
                data: prices
            }],
            chart: {
                type: 'candlestick',
                id: 'tock',
                toolbar: {
                    offsetY: document.body.clientHeight * 0.007
                }
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
    } catch (exception) {}
}

function applyStyle(idElement, style) {
    const el = document.getElementById(idElement);
    el.className = style;
    console.log(idElement + "->" + style);
}

function applyClass(cssClass, style) {
    const elemets = document.querySelector(cssClass);
    for( const e in elements){
        el.className = style;
    }
}
    
    