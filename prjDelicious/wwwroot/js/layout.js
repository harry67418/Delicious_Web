//登出
function Logout() {
    $.ajax({
        url: "/HomePage/Logout",
        type: "POST",
        success: function (data) {
            window.alert(data);
            window.location.reload();
        }
    })
}
//搜尋按鈕
$("#btnSearch").click(function () {
    let txtSearch = "?txtSearch=" + $("#txtSearch").val()
    if ($("#selSearch").val() == "recipe") {
    window.open('/Search/RecipeList' + txtSearch)
}
    else {
    window.open('/Search/MemberList' + txtSearch)
}
})