
var primaryColor;
var chartColors;
var halfChartColors;

//Charts use HTML attributes data-sets='[{"label":"str", "data":[1,...]},...]' data-labels='["str",...]'
function renderLineChart(canvas) {
    var sets = JSON.parse(canvas.dataset.sets);
    var labels = JSON.parse(canvas.dataset.labels);
    var datasets = [];
    for (var i = 0; i < sets.length && i < 5; i++) {
        var dset = sets[i];
        datasets.push({
            label: dset.label,
            data: dset.data,
            backgroundColor: halfChartColors[i],
            borderColor: chartColors[i],
            borderWidth: 2
        });
    }
    var chart = new Chart(canvas, {
            type: 'line',
            data: {
                labels: labels,
                datasets: datasets
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        }
    );
};

function renderBarChart(canvas) {
    var sets = JSON.parse(canvas.dataset.sets);
    var labels = JSON.parse(canvas.dataset.labels);
    var datasets = [];
    for (var i = 0; i < sets.length && i < 5; i++) {
        var dset = sets[i];
        datasets.push({
            label: dset.label,
            data: dset.data,
            backgroundColor: halfChartColors[i],
            borderColor: chartColors[i],
            borderWidth: 2
        });
    }
    var chart = new Chart(canvas, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: datasets
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    }
    );
};

function renderRadarChart(canvas) {
    var sets = JSON.parse(canvas.dataset.sets);
    var labels = JSON.parse(canvas.dataset.labels);
    var datasets = [];
    for (var i = 0; i < sets.length && i < 5; i++) {
        var dset = sets[i];
        datasets.push({
            label: dset.label,
            data: dset.data,
            backgroundColor: halfChartColors[i],
            borderColor: chartColors[i],
            borderWidth: 2
        });
    }
    var chart = new Chart(canvas, {
        type: 'radar',
        data: {
            labels: labels,
            datasets: datasets
        }
    }
    );
};

function parseColor(color) {
    var m = color.match(/^rgb\s*\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)$/i);
    return [m[1], m[2], m[3]];
}

function halfAlpha(color) {
    var m = parseColor(color)
    return "rgba(" + m[0] + ", " + m[1] + ", " + m[2] + ", " + 0.5 + ")";
}

$(document).ready(function () {
    chartColors = [];
    halfChartColors = [];

    var primaryColorDiv = document.createElement("div");
    primaryColorDiv.style.display = "none";
    primaryColorDiv.className = "text-primary";
    document.body.appendChild(primaryColorDiv);
    primaryColor = getComputedStyle(primaryColorDiv).getPropertyValue("color");
    document.body.removeChild(primaryColorDiv);

    var colorDiv = document.createElement("div");
    colorDiv.style.display = "none";
    document.body.appendChild(colorDiv);

    colorDiv.className = "text-success";
    chartColors.push(getComputedStyle(colorDiv).getPropertyValue("color"));
    halfChartColors.push(halfAlpha(getComputedStyle(colorDiv).getPropertyValue("color")));

    colorDiv.className = "text-info";
    chartColors.push(getComputedStyle(colorDiv).getPropertyValue("color"));
    halfChartColors.push(halfAlpha(getComputedStyle(colorDiv).getPropertyValue("color")));

    colorDiv.className = "text-warning";
    chartColors.push(getComputedStyle(colorDiv).getPropertyValue("color"));
    halfChartColors.push(halfAlpha(getComputedStyle(colorDiv).getPropertyValue("color")));

    colorDiv.className = "text-danger";
    chartColors.push(getComputedStyle(colorDiv).getPropertyValue("color"));
    halfChartColors.push(halfAlpha(getComputedStyle(colorDiv).getPropertyValue("color")));

    colorDiv.className = "text-primary";
    chartColors.push(getComputedStyle(colorDiv).getPropertyValue("color"));
    halfChartColors.push(halfAlpha(getComputedStyle(colorDiv).getPropertyValue("color")));

    document.body.removeChild(colorDiv);

    var lineCanvasList = document.getElementsByClassName("chart-line");
    for (var i = 0; i < lineCanvasList.length; i++) {
        renderLineChart(lineCanvasList[i]);
    }

    var barCanvasList = document.getElementsByClassName("chart-bar");
    for (var i = 0; i < barCanvasList.length; i++) {
        renderBarChart(barCanvasList[i]);
    }

    var radarCanvasList = document.getElementsByClassName("chart-radar");
    for (var i = 0; i < radarCanvasList.length; i++) {
        renderRadarChart(radarCanvasList[i]);
    }
});
