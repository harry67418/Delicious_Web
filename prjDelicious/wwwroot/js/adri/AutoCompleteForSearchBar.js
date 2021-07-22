var availableTags;
$.ajax({
    url: "/HomePage/getSearchBar",
    type: "GET",
    success: function (data) {
        availableTags = data.split(",");
        $("#SearchKeyInput").autocomplete({
            source: availableTags
        });
    }
});
