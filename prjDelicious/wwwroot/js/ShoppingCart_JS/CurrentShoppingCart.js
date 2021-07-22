let member_NoCSC = $("#user_member").val();
let users_cart_NoCSC = "cart" + member_NoCSC;   //依登入的會員改變localstorage的Key值
let pdtcart = JSON.parse(localStorage.getItem(users_cart_NoCSC));  //從localStorage讀取購物車內的資料
let pdtcart_quantity = [];     //用來存放各個商品的庫存量及製作時間
//載入購物車時抓取現有庫存量
$(function () {
    if (pdtcart == null || pdtcart.length == 0)
        return;
    let pdtIDquantity = [];   //放入的屬性為ID和數量
    for (let i = 0; i < pdtcart.length; i++) {
        let pdtInCart = parseInt(pdtcart[i].cProductId);
        pdtIDquantity.push(pdtInCart);
        //console.log(pdtInCart);
    }
    $.ajax({
        url: "/ShoppingCart/CurrentItemCount",
        data: JSON.stringify(pdtIDquantity),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            pdtcart_quantity = data;
            //console.log(data);
        }
    });
})

//console.log(pdtcart);
//建立動態的購物車介面
$(function () {
    let text = "";
    //判斷購物車內使否有商品
    if (pdtcart == null || pdtcart.length == 0) {
        $('#cart_total_price').text("$0");
        return;
    }
    let item_index = 0;  //為產品在購物車的排序編號
    for (let i = 0; i < pdtcart.length; i++) {
        let No = pdtcart[i].cProductId;  //No為產品的ID編號
        let pdt_price = "price" + No;    //產品價格的html標籤id
        let id_product = "product" + No; //產品編號的html標籤id
        let total_price = "total" + No;  //產品總計的html標籤id
        let pdt_quan = "quantity" + No;  //產品數量的html標籤id
        text += `<tr id="${item_index}">
                            <td class="shoping__cart__item">
                                <a href="/ProductDetail/ProductData/${pdtcart[i].cProductId}">
                                <img width="100" height="100" src="${pdtcart[i].cPicture.replace('~', '')}" alt="">
                                <h5>${pdtcart[i].cProductName}</h5>
                                </a>
                            </td>
                            <td class="shoping__cart__price" id="${pdt_price}">
                                $${Math.round(pdtcart[i].cPrice)}
                            </td>
                            <td class="shoping__cart__quantity">
                                <div class="quantity">
                                    <div class="pro-qty">
                                        <span class="dec qtybtn" onclick="decbtn(${pdt_quan})">-</span>
                                        <input id="${id_product}" type="hidden" value="${pdtcart[i].cProductId}">
                                        <input oninput="this.value = this.value.replace(/[^0-9]/g, '');" onchange="change_quan(${pdt_quan})"
                                        id="${pdt_quan}" type="text" value="${pdtcart[i].QuantityInCart}">
                                        <span class="inc qtybtn" onclick="incbtn(${pdt_quan})">+</span>
                                    </div>
                                </div>
                            </td>
                            <td class="shoping__cart__total cart_totalprice" id="${total_price}">
                                $${pdtcart[i].ProductAmount}
                            </td>
                            <td class="shoping__cart__item__close">
                                <span onclick = "DeleteProduct(${item_index},'${pdtcart[i].cProductName}')" class="icon_close" style="background-color:orangered;border-radius: 5px;"></span>
                            </td>
                        </tr>`;
        item_index += 1;
    }
    $("#show_cart").html(text);
    CartAmount();   //此方法執行的結果會改變 id="cart_total_price" 的標籤內容，故需放在最後執行
})

//手動輸入數量
function change_quan(quantityID) {
    let productID = $('#' + quantityID.id).prev().val();
    let count = $('#' + quantityID.id).val();
    let index = $(quantityID).closest("tr").attr("id");
    if (count == null || count == 0) {
        count = 1;
    }
    //檢查產品現有可製作的份量
    if (count > pdtcart_quantity[index].quantityInCart) {
        window.alert("已超過現有可製作的份量");
        count = pdtcart_quantity[index].quantityInCart;
    }
    $('#' + quantityID.id).val(count);
    GetTotalPrice(productID, count); //計算購物車內某商品的總和
    CartAmount();   //計算購物車內商品總額
}
//按下-按鈕數量-1
function decbtn(htmlID) {
    let quantity = $('#' + htmlID.id).val();
    let productID = $('#' + htmlID.id).prev().val();
    if (quantity > 1)
        var count = parseFloat(quantity) - 1;
    else
        var count = 1;
    $('#' + htmlID.id).val(count);
    GetTotalPrice(productID, count);
    CartAmount();   //計算購物車內商品總額
}
//按下+按鈕數量+1
function incbtn(htmlID) {
    let quantity = $('#' + htmlID.id).val();
    let productID = $('#' + htmlID.id).prev().val();
    let count = parseFloat(quantity) + 1;
    let index = $(htmlID).closest("tr").attr("id");
    if (count > pdtcart_quantity[index].quantityInCart) {
        window.alert("已超過現有可製作的份量");
        count -= 1;
    }
    $('#' + htmlID.id).val(count);
    GetTotalPrice(productID, count);
    CartAmount();   //計算購物車內商品總額
}
//計算購物車內某商品的總和
function GetTotalPrice(id, count) {
    let price = $('#price' + id).text().replace('$', "");
    let totalprice = parseFloat(price) * count;
    $('#total' + id).html('$' + totalprice);
}

//刪除購物車的商品
function DeleteProduct(row_NO, pdtname) {
    let cart_index = pdtcart.findIndex(x => x.cProductName == pdtname);
    pdtcart.splice(cart_index, 1);    //刪除pdtcart中該商品
    localStorage.setItem(users_cart_NoCSC, JSON.stringify(pdtcart));
    $('tr#' + row_NO).remove();       //移除該商品的html標籤
    CartAmount();
}

//購物車總金額計算
function CartAmount() {
    let cart_total = 0;
    let product_total = document.querySelectorAll('.cart_totalprice');
    for (let i = 0; i < product_total.length; i++) {
        cart_total += parseInt(product_total[i].textContent.replace('$', ""));
    }
    $("#cart_total_price").text('$' + cart_total);  //顯示當前折價後的金額
    $("#origin_price").html('$' + cart_total);      //顯示當前未折價的總金額
    GetDiscountTotalPrice();
}
let discount_price = 0;   //要折扣的金額
let total_price = 0;      //未含折扣的金額
let sum_price = 0;        //折扣後的總計
let couponId = 1;         //折價券未指定時，使用ID為1的無折扣折價券
//檢查優惠券是否可使用
$(function () {
    $("#check_coupon").click(function () {
        let coupon_code = $("#input_coupon_code").val();
        console.log(coupon_code);
        $.ajax({
            url: "/ShoppingCart/CheckCouponCode",
            data: JSON.stringify(coupon_code),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "0") {
                    window.alert("輸入的代碼有誤或此優惠券已被使用");

                } else {
                    discount_price = data.cCutPrice;
                    couponId = data.cCuponId;
                    let text = "目前優惠券的折價金額為: $" + discount_price;
                    $("#coupon_discount").html(text);
                    GetDiscountTotalPrice();
                    window.alert("此優惠券可使用");
                }
            }
        })
    })
})
//檢查優惠券checkbox是否有勾選
$(function () {
    $("#check_is_used").change(function () {
        //console.log($("#check_is_used").prop('checked'));
        if ($("#check_is_used").prop('checked')) {
            $("#input_coupon_code").prop('disabled', false);
        } else {
            discount_price = 0;
            couponId = 1;
            $("#input_coupon_code").val("");
            $("#coupon_discount").html("目前優惠券的折價金額為: 無");
            $("#input_coupon_code").prop('disabled', true);
            GetDiscountTotalPrice();
        }
    })

})
//取得折價券的面額
function GetDiscountTotalPrice() {
    total_price = parseInt($("#origin_price").text().replace('$', ""));
    sum_price = total_price - discount_price;
    $("#off_price").html('$' + discount_price);
    $("#cart_total_price").html('$' + sum_price);
}

//跳轉至最後確認結帳頁面
$(function () {
    $("#send_order").click(function () {
        if (pdtcart == null || pdtcart.length == 0) {
            window.alert("購物車內尚無任何商品");
            return;
        }
        let cart_total_price = parseInt($("#cart_total_price").text().replace('$', ''));
        if (cart_total_price <= 0) {
            window.alert("購物車總計不可低於折扣金額");
            return;
        }
        window.location.assign("/CheckCart/OrderData");
    })
})

//離開頁面時儲存當前的商品及數量
$(window).bind('beforeunload', function () {
    let pdt_finishtime = 0;    //改訂單需要製作的時間
    let longer_time = 0;       //該商品的製作時間
    let pdt_count = 0;
    for (let i = 0; i < pdtcart.length; i++) {
        pdtcart[i].QuantityInCart = parseInt($('#quantity' + pdtcart[i].cProductId).val());
        pdtcart[i].ProductAmount = parseInt($('#total' + pdtcart[i].cProductId).text().replace('$', ''));
        //簡易自訂計算商品製作的時間
        if (longer_time < pdtcart_quantity[i].finishTime) {
            longer_time = pdtcart_quantity[i].finishTime;
            pdt_count = pdtcart[i].QuantityInCart;
        }
        pdt_finishtime = Math.round(((pdt_count / 4) + 1) * longer_time)   //取得製作時間最長的商品*數量
    }
    let cart_price_obj = {
        origin: total_price,
        discount: discount_price,
        total: sum_price,
        couponId: couponId,
        finishTime: pdt_finishtime
    };
    localStorage.setItem("cart_price", JSON.stringify(cart_price_obj));
    localStorage.setItem(users_cart_NoCSC, JSON.stringify(pdtcart));
}
);
