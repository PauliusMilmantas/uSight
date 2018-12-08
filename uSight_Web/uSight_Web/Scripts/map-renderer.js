
function renderMap(map) {
    var markers = JSON.parse(map.dataset.markers);
    var mapObj = new google.maps.Map(map, { zoom: 2, center: { lat: 0, lng: 0 } });
    for (var i = 0; i < markers.length; i++) {
        var m = new google.maps.Marker({ position: { lat: markers[i].lat, lng: markers[i].lng }, map: mapObj });
        var iw = new new google.maps.InfoWindow({ content: markers[i].desc });
        m.addListener("click", function () { iw.open(mapObj, m);});
    }
};

$(document).ready(function () {
    var mapList = document.getElementsByClassName("map");
    for (var i = 0; i < mapList.length; i++) {
        renderMap(mapList[i]);
    }
});