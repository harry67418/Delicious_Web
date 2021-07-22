var AdritextForComboBox = "<select id='SearchOptionCategory' name='sfsname' class='hero__search__form' style='width:10%;height: 45.49px;'>" + "<option value='全部'>全部</option>";$.ajax({
    url: "/HomePage/getCategory",
    type: "POST",
    success: function (data) {
        for (var i = 0; i < data.length; i++) {
            var catName = data[i].replace(/\s*/g, "");
            AdritextForComboBox += '<option value="' + catName + '">' + catName + '</option>';
        }
        AdritextForComboBox +='</select>';
        $("div.hero__search__categories").prepend(AdritextForComboBox);    }
});