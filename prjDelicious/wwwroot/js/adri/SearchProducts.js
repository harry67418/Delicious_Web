////let member_No = $("#user_member").val();
////let users_cart_No = "cart" + member_No;   //依登入的會員改變localstorage的Key值
var cart = [];
//判斷購物車內是否有商品
function isExist(id) {
    cart = JSON.parse(localStorage.getItem(users_cart_No));
    for (let i = 0; i < cart.length; i++) {
        if (cart[i].cProductId === id) {
            return i;
        }
    }
    return -1;
}

var pdtItem = null;
        //將商品加入到購物車內
function addCart(obj_product) {
    //如果沒有登入則跳到登入畫面
    if (member_No == "") {
        window.location.assign("/ShoppingCart/CurrentCartItem");
        return;
    }
    if (localStorage.getItem(users_cart_No)) {
        let pdtindex = isExist(obj_product.cProductId);
        if (pdtindex != -1) {
            window.alert("此商品已在購物車中");
        } else {
            addItemInCart(obj_product);
        }
    } else {
        addItemInCart(obj_product);
    }
}
//若購物車內無該商品則加入
function addItemInCart(obj_product) {
    let cartItem = {
        cProductId: obj_product.cProductId,
        cProductName: obj_product.cProductName,
        cPrice: obj_product.cPrice,
        cPicture: obj_product.cPicture,
        QuantityInCart: 1,
        ProductAmount: obj_product.cPrice * 1
    };
    cart.push(cartItem);
    window.alert("已加入購物車");
    localStorage.setItem(users_cart_No, JSON.stringify(cart));
}

$("#searchBtn").on("click", function () {
    $("#sideBar").css("display", "none");

    $("#FrameOfSlider").css("display", "none");
    $("#slider_homepage").css("display", "none");
    $("#addsome").css("display", "none");


    $("#SearchChange").css({ left: 285.551 });

    
    //商品foreach
    $.ajax({
        url: "/HomePage/getSearchKey",
        data: {
            "key": $("#SearchKeyInput").val(), "Category": $("#SearchOptionCategory option:selected").val(), "Min": $("#SearchKeyInputMinPrice").val(), "Max": $("#SearchKeyInputMaxPrice").val(), "BreakFast": $('#CheckboxForBreakFast:checked').val(), "Lunch": $('#CheckboxForLunch:checked').val(), "Dinner": $('#CheckboxForDinner:checked').val()
        },
        type: "POST",
        success: function (data) {
            

            var itemPerPage = 8;
            var HowManyPages = (data.length / itemPerPage);//40/8 =5頁;
            //1~8 9~16 17~24 25~32 33~40 41~x
            //0~7 8~15 16~23 24~31 32~39 40~x-1
            var showItems = 0;
            if (data.length < itemPerPage)
            {
                showItems = data.length;
            }
            else
            {
                showItems = (data.length / HowManyPages);
            }
             pdtItem = data;
            var label = `<section class='featured spad'><p>搜尋結果：共有 ${data.length} 筆商品</p><div class='container'><div>`;
            label += "<div class='row featured__filter' id='pagesForForLoop'>";
            for (let i = 0; i < showItems; i++) {
                

                label += `
                                        <div class="col-lg-3 col-md-4 col-sm-6 mix oranges fresh-meat">
                                        <div class="featured__item">
                                        <div class="featured__item__pic set-bg" style="background-image: url(${data[i].cPicture.replace("~", "")})">
                                        <ul class="featured__item__pic__hover">
                                         <li><a href = "#" ><i class="fa fa-heart"></i></a></li>
                                         <li><a href = "#" ><i class="fa fa-retweet"></i></a></li>
                                         <li><a href="javascript: void(0)" onclick="addCart(pdtItem[${i}])"><i class="fa fa-shopping-cart"></i></a></li>
                                         </ul>
                                         </div>
                                         <div class="featured__item__text">
                                         <h6><a href="/ProductDetail/ProductData?id=${data[i].cProductId}">${data[i].cProductName}</a></h6>
                                         <h5>$${data[i].cPrice}</h5>
                                         </div>
                                         </div>
                                         </div>`;
            }
            label += "</ div>";
            label += "</ div>";
            label += "</div>";

            label += `<nav aria-label="Page navigation example">
                                    <ul class="pagination ">`;
            for (var i = 0; i < HowManyPages; i++)
            {
                label += `<li class="page-item"><button type="button" class="page-link" onclick="pagesForSearch(${i})">${i + 1}</button></li>`;
            }


            label += `</ul>
                              </nav>`;
            label += "</ section>";

            /*$("#showProduct1").css('background-image', 'url("img/featured/feature-1.jpg")');*/
            $("#divforSearch").html(label);

        }

    })

})
