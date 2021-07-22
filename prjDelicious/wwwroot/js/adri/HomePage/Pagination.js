////let member_No = $("#user_member").val();
////let users_cart_No = "cart" + member_No;        //依登入的會員改變localstorage的Key值
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
        window.alert("請先登入會員");
        window.location.assign("/ShoppingCart/CurrentCartItem");
        return;
    }
    if (obj_product.cQuantity <=0 || obj_product.cIsOnSaleId != 1) {
        window.alert("此商品目前已售完，請稍後");
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
//分頁function
function pagesForSearch(page) {

    
    $.ajax({
        url: "/HomePage/getSearchKey/",
        type: "POST",
        data: {
            "key": $("#SearchKeyInput").val(), "Category": $("#SearchOptionCategory option:selected").val(), "Min": $("#SearchKeyInputMinPrice").val(), "Max": $("#SearchKeyInputMaxPrice").val(), "BreakFast": $('#CheckboxForBreakFast:checked').val(), "Lunch": $('#CheckboxForLunch:checked').val(), "Dinner": $('#CheckboxForDinner:checked').val()
        },
        success: function (data) {
            pdtItem = data;



            var itemPerPage = 8;
            //var HowManyPages = (data.length / itemPerPage);   //40/8
            var textForLoop = "";
            var items = (page + 1) * itemPerPage;//1*8 2*8
            if (items > data.length) {
                items = data.length;
            }

            for (var i = (page * itemPerPage); i < items; i++) {
                var images = data[i].cPicture;
                var imageSrc = images.replace("~", "");
                console.log(imageSrc);

                textForLoop += `<div class="col-lg-3 col-md-4 col-sm-6 mix oranges fresh-meat">
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
            $("#pagesForForLoop").html(textForLoop);
        }

    });


}
