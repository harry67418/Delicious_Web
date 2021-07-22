var textForSideBarCate = "";
$.ajax({
    url: "/HomePage/getCategory",
    type: "POST",
    success: function (data) {
        for (var i = 0; i < data.length; i++) {
            var catName = data[i].replace(/\s*/g, "");
            textForSideBarCate += `<li><a href="#featureProductsForLocation" onclick="sideBarClick(${i})" style="font-size:17px;margin:10px">${catName}</a></li>`;
        }
        $("#sideBarCategory").append(textForSideBarCate);
    }
});

function sideBarClick(num) {

    $("#push_procategory ul li").eq(num).click();

}