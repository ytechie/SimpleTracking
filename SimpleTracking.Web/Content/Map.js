var map;

function GetMap() {
    var mapOptions = 
        {
            credentials: "AlAVTbe0GdeAnzr6Vmxo7N3-uXmOy28QhZ8T2n3F5UHaMFRXpYrnuKcgP4clEe75",
            //center: new Microsoft.Maps.Location(45.5, -122.5),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 7
        }

    map = new Microsoft.Maps.Map(document.getElementById("mapDiv"), mapOptions);

    console.log("Loading Microsoft search module");
    Microsoft.Maps.loadModule("Microsoft.Maps.Search", { callback: searchLoaded });
}

function searchLoaded() {
    console.log("Microsoft search module loaded");

    var searchManager = new Microsoft.Maps.Search.SearchManager(map);

    var searchRequest = { query: "pizza in Seattle, WA", count: 1, callback: searchCallback, errorCallback: searchError };
    searchManager.search(searchRequest);
}

function searchCallback(searchResponse, userData) {
    //alert("The search result is " + searchResponse.searchResults[0].name + ".");
    var location = searchResponse.searchResults[0].location;
}

function searchError(searchRequest) {
    console.error("An error occurred while geocoding an address", searchRequest);
}

function getLatLongFromAddress(address) {

}

function ShowMap() {
    GetMap();
}

$(ShowMap);