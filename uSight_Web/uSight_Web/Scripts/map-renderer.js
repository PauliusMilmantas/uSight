
var infoWindow = new google.maps.InfoWindow();

function renderMap(map) {
    var markers = JSON.parse(map.dataset.markers);
    var mapObj = new google.maps.Map(map, { zoom: 2, center: { lat: 0, lng: 0 } });
    for (var i = 0; i < markers.length; i++) {
        var m = new google.maps.Marker({ position: { lat: markers[i].lat, lng: markers[i].lng }, map: mapObj });
        m.addListener("click", (function () {
            var dsc = markers[i].desc;
            var mk = m;
            function onclick() {
                infoWindow.setContent(dsc);
                infoWindow.open(mapObj, mk);
            }
            return onclick;
        })());
    }
};

$(document).ready(function () {
    var mapList = document.getElementsByClassName("map");
    for (var i = 0; i < mapList.length; i++) {
        renderMap(mapList[i]);
    }
});