var folder_check = $(".form-check-input"); //找到所有收藏夾的checkbox

//編輯收藏夾按鈕
$(document).on("click", ".editfolder", function (evt) { //點下編輯收藏事件
    let recipeid = $(this).data("recipe")
    let loginid = $(this).data("loginid")
    if (loginid == 0) {
        alert("請登入使用收藏功能");
        return;
    }
    $("#folder-list").modal("show");
    $("#folder-list").modal("toggle"); //modal彈出
    //設定按鈕的recipeid以便使用
    folder_check.each(function () {
        $(this).attr("checked", false)
        $(this).prop("checked", false)
        $(this).data("recipe", recipeid);
    })
    //判斷該食譜收藏在哪個資料夾方法
    getRecipeFolder(loginid, recipeid);
});
//食譜收藏夾選單,移入移出
$(document).on("change", ".form-check-input", function (evt) {
    if (this.checked) {//加入收藏夾
        addFolder($(this).data("recipe"), $(this).data("folder"));//加入方法
    } else {//滾出收藏夾
        removeFolder($(this).data("recipe"), $(this).data("folder"));//移出方法
    }
});
//-------------------------------------------------------
//彈出編輯收藏夾視窗
$(".edit-foldername").on("click", function () {
    $("#edit-folder").modal("show");
    $("#edit-folder").modal("toggle"); //modal彈出
})
//彈出建立收藏夾按鈕
$(".create-folder").click(function () {
    $("#create-folder").modal("show");
    $("#create-folder").modal("toggle"); //modal彈出
})
//彈出刪除收藏夾視窗
$(".delete-folder").on("click", function () {
    $(".delete-title").text(`(${$(".folder-title").text()})`)
    $("#delete-folder").modal("show");
    $("#delete-folder").modal("toggle"); //modal彈出
})
//-------------------------------------------------------
//刪除收藏夾按鈕
$(".delete-send").click(function () {
    let foldername = $(".txt-delete-folder").val();
    let foldertitle = $(".folder-title").text().trim(" ");
    let memberid = $(this).data('memberid');
    console.log(foldername)
    console.log(foldertitle)
    if (foldername != foldertitle) {
        $(".delete-error").text("名稱不一致")
    }
    else {
        if (confirm("確定刪除嗎?")) {
            $.ajax({
                beforeSend: function () {

                },
                url: '/Folder/DeleteFolder',
                type: "post",
                data: { "folderId": $(this).data("folderid") },
                success: function (data) {
                    if (data) {
                        window.location.href = `/Folder/Index?memberId=${memberid}`
                    }
                    else {
                        alert("失敗")
                    }
                },
                complete: function () {

                },
                error: function (data) {
                    alert("失敗")
                    $(this).attr("checked", "checked");
                }
            })
        }
    }
})

//編輯確認按鈕
$(".edit-send").click(function () {
    let folderName = $(".txt-edit-folder").val().trim(" ");
    $.ajax({
        beforeSend: function () {

        },
        url: '/Folder/EditFolderName',
        type: "post",
        data: { "folderName": folderName , "folderId": $(this).data("folderid") },
        success: function (data) {
            if (data) {
                $("#edit-folder").modal("hide");
                $(".list-group>.active").text(folderName)//改變選單收藏夾名字
                $(".folder-title").text(folderName)//改變編輯區收藏夾名字
            }
            else {
                alert("失敗")
            }
        },
        complete: function () {

        },
        error: function (data) {
            alert("失敗")
            $(this).attr("checked", "checked");
        }
    })
})
//建立收藏夾按鈕
$(".create-send").click(function () {
    let folderName = $(".txt-create-folder").val().trim(" ");
    if (!$.trim(folderName)) {
        $(".create-error").text("名稱不可為空")
    }
    else {
        $.ajax({
            beforeSend: function () {

            },
            url: '/Folder/CreateFolder',
            type: "post",
            data: { "folderName": folderName, "memberId": $(this).data("memberid") },
            success: function (data) {
                if (data) {
                    window.location.reload();
                }
                else {
                    alert("失敗")
                }
            },
            complete: function () {

            },
            error: function (data) {
                alert("失敗")
                $(this).attr("checked", "checked");
            }
        })
    }
})
//-------------------------------------------------------
//編輯收藏夾輸入框事件
$(".txt-edit-folder").keyup(function () {
    let txt = $(this).val();
    let count = $(this).val().length
    let error = $(".edit-error")

    $(".count").text(count + "/20");

    folderJudgement(txt, error)
})
//新增收藏夾輸入框事件
$(".txt-create-folder").keyup(function () {
    let txt = $(this).val();
    let count = $(this).val().length
    let error = $(".create-error")

    $(".count").text(count + "/20");

    folderJudgement(txt, error)
})
//刪除收藏夾輸入框事件
$(".txt-delete-folder").keyup(function () {
    let txt = $(this).val();
    let foldertitle = $(".folder-title").text().trim(" ");
    if (txt == foldertitle) {
        $(".delete-error").text("");
    }
    else {
        $(".delete-error").text("名稱不一致");
    }
})
//-------------------------------------------------------
//收藏夾名稱判斷
function folderJudgement(txt,error) {
    if (!$.trim(txt)) {
        error.text("名稱不可為空")
    }
    else {
        error.text("")
    }
}
//取得食譜收藏列表
function getRecipeFolder(memberid, recipeid) {
    $.ajax({
        beforeSend: function () {

        },
        url: '/Folder/FolderModal',
        type: "post",
        data: { "memberId": memberid, "recipeId": recipeid },
        success: function (datas) {
            datas.forEach(function (data) {//外層迴圈此食譜收藏過哪個收藏夾
                const { collectionFolder, collectionFolderId, collectionId } = data;
                folder_check.each(function () {//內層迴圈所有checkbox判斷收藏夾id有無匹配
                    if ($(this).data("folder") == collectionFolderId) {
                        $(this).attr("checked", true)
                        $(this).prop("checked", true)
                    }
                })
            })
        },
        complete: function () {

        },
        error: function (data) {
            alert(data);
        }
    })
}
//加入收藏夾
function addFolder(recipeid, folderid) {
    $.ajax({
        beforeSend: function () {

        },
        url: '/Folder/AddFolder',
        type: "post",
        data: { "recipeId": recipeid, "folderId": folderid },
        success: function (data) {
            if (data) {

            }
            else {
                alert("失敗")
                $(this).attr("checked", false);
            }
        },
        complete: function () {

        },
        error: function (data) {
            alert("失敗")
            $(this).attr("checked", "checked");
        }
    })
}
//移出收藏夾
function removeFolder(recipeid, folderid) {
    $.ajax({
        beforeSend: function () {

        },
        url: '/Folder/RemoveFolder',
        type: "post",
        data: { "recipeId": recipeid, "folderId": folderid },
        success: function (data) {
            if (data) {

            }
            else {
                alert("失敗");
                $(this).attr("checked", "checked");
            }
        },
        complete: function () {

        },
        error: function (data) {
            alert("失敗");
            $(this).attr("checked", "checked");
        }
    })
}
//食譜卡連結食譜頁面
$(".recipe img").on("click", function () {
    window.location.href = "/ForRecipe/Recipe?id=" + $(this).data("id");
})

