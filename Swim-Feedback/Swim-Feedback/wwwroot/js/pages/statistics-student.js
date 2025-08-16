var studentStatistics = {};

studentStatistics.init = function (scoreChartData) {
    studentStatistics.createScoreChart(scoreChartData);
};

studentStatistics.createScoreChart = function (scoreChartData) {
    const ctx = document.getElementById('student-statistics');

    const config = {
        type: 'bar',
        data: {
            labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10'],
            datasets: [{
                label: '# of feedback',
                data: scoreChartData,
                backgroundColor: [
                    'rgb(255, 0, 0, 0.7)',    
                    'rgb(254, 68, 0, 0.7)',
                    'rgb(249, 102, 0, 0.7)',
                    'rgb(249, 102, 0, 0.7)',
                    'rgb(240, 129, 0, 0.7)',
                    'rgb(227, 154, 0, 0.7)',
                    'rgb(210, 177, 0, 0.7)',
                    'rgb(189, 199, 0, 0.7)',
                    'rgb(129, 237, 0, 0.7)',
                    'rgb(72, 255, 0, 0.7)',
                ],
                borderColor: [
                    'rgb(255, 0, 0, 1)',
                    'rgb(254, 68, 0, 1)',
                    'rgb(249, 102, 0, 1)',
                    'rgb(249, 102, 0, 1)',
                    'rgb(240, 129, 0, 1)',
                    'rgb(227, 154, 0, 1)',
                    'rgb(210, 177, 0, 1)',
                    'rgb(189, 199, 0, 1)',
                    'rgb(129, 237, 0, 1)',
                    'rgb(72, 255, 0, 1)',
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: 'Feedback'
                }
            }
        }
    };

    studentStatistics.scoreChart = new Chart(ctx, config);
};

studentStatistics.updateScoreChart = function (scoreChartData) {
    studentStatistics.scoreChart.data.datasets[0].data = scoreChartData;
    studentStatistics.scoreChart.update();
};