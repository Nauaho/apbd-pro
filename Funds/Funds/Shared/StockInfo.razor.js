
function createChart(prices, idElement){
  var options = {
  chart: {
    type: 'candlestick'
  },
  series: [{
    name: 'sales',
    data: prices
  }],
  plotOptions: {
    candlestick: {
      colors: {
        upward: '#00B746',
        downward: '#DF7D46'
      }
    }
  }
}

var chart = new ApexCharts(document.querySelector("#"+idElement), options);

chart.render();
}
