$(function () {
    $.ajax({
        url: "/HomePage/getBottomList/",
        type: "get",
        success: function (data) {

            console.log(data);

            var textForReviewsP = `<div class="col-lg-4 col-md-6">
                                                        <div class="latest-product__text">
                                                        <h4>熱評商品</h4>
                                                        <div class="latest-product__slider owl-carousel">`
            var countForReview = 1;
            for (let a = 0; a < 2; a++) {
                textForReviewsP += `<div class="latest-prdouct__slider__item">`;
                for (let b = (3 * (countForReview - 1)); b < (3 * countForReview); b++) {
                    textForReviewsP += ` <a href="/ProductDetail/ProductData?id=${data.reviewProducts[b].cProductId}" class="latest-product__item">
                                                                    <div class="latest-product__item__pic">
                                                                        <img src="${data.reviewProducts[b].cPicture.replace("~", "")}" style="width:100px;height:105.86px;" alt="">
                                                                    </div>
                                                                    <div class="latest-product__item__text">
                                                                        <h6>${data.reviewProducts[b].cProductName}</h6>
                                                                        <span>$${data.reviewProducts[b].cPrice}</span>
                                                                    </div>
                                                                </a>`;
                }
                countForReview++;
                textForReviewsP += `</div>`;
            }
            textForReviewsP += `            
                                </div>
                                        </div>
                                    </div>`;

            $("#ProductSliderForAnime").append(textForReviewsP);

            var textForTops = `<div class="col-lg-4 col-md-6">
                                                    <div class="latest-product__text">
                                                        <h4>好評商品</h4>
                                                        <div class="latest-product__slider owl-carousel">`
            var countForTops = 1;
            for (let a = 0; a < 2; a++) {
                textForTops += `<div class="latest-prdouct__slider__item">`;

                for (let b = (3 * (countForTops - 1)); b < (3 * countForTops); b++) {
                    textForTops += `<a href="/ProductDetail/ProductData?id=${data.topProducts[b].cProductId}" class="latest-product__item">
                                                            <div class="latest-product__item__pic">
                                                                <img src="${data.topProducts[b].cPicture.replace("~", "")}" style="width:100px;height:105.86px;" alt="">
                                                                            </div>
                                                                <div class="latest-product__item__text">
                                                                    <h6>${data.topProducts[b].cProductName}</h6>
                                                                    <span>$${data.topProducts[b].cPrice}</span>
                                                                </div>
                                                                        </a>`
                }
                textForTops += `</div>`;
                countForTops++
            }
            textForTops += `</div>
                                            </div>
                                            </div>`

            $("#ProductSliderForAnime").append(textForTops);

            var txtLastProducts = ` <div class="col-lg-4 col-md-6">
                                                                                        <div class="latest-product__text">
                                                                                            <h4>最新商品</h4>
                                                                                            <div class="latest-product__slider owl-carousel" >`;
            var countForLasts = 1;
            for (let a = 0; a < 2; a++) {

                txtLastProducts += `<div class="latest-prdouct__slider__item">`;

                for (let b = (3 * (countForLasts - 1)); b < (3 * countForLasts); b++) {

                    txtLastProducts += ` <a href="/ProductDetail/ProductData?id=${data.lastProducts[b].cProductId}" class="latest-product__item">
                                                                                            <div class="latest-product__item__pic">
                                                                                                <img src="${data.lastProducts[b].cPicture.replace("~", "")}" style="width:100px;height:105.86px;" alt="">
                                                                                            </div>
                                                                                            <div class="latest-product__item__text">
                                                                                                <h6>${data.lastProducts[b].cProductName}</h6>
                                                                                                <span>$${data.lastProducts[b].cPrice}</span>
                                                                                            </div>
                                                                                        </a>`;

                }
                countForLasts++;
                txtLastProducts += `</div>`;
            }
            txtLastProducts += ` </div>
                                                        </div>
                                                    </div>`;

            $("#ProductSliderForAnime").append(txtLastProducts);





            /*--------------------------
           Latest Product Slider
           ----------------------------*/
            $(".latest-product__slider").owlCarousel({
                loop: true,
                margin: 0,
                items: 1,
                dots: false,
                nav: true,
                navText: ["<span class='fa fa-angle-left'><span/>", "<span class='fa fa-angle-right'><span/>"],
                smartSpeed: 1200,
                autoHeight: false,
                autoplay: true
            });


        }
    })
})