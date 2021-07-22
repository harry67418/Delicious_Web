function ListOrderJS(memberName) {


    var AdricountLO = 1;
    var AdritextForLO = "";


    $.ajax({
        url: "/Order/ListOrder",
        type: "POST",
        async: false,
        data: { MemberName: memberName },
        success: function (data) {

            $.ajax({
                url: "/Order/OrderStatusDate",
                type: "POST",
                data: { MemberName: memberName },
                success: function (response) {

                    console.log(response);
                    for (let i = 0; i < response.length; i++) {
                        /*                    console.log(data[i].cOrderID + data[i].cTotalPrice + response[i].cOrderStatus + response[i].cOrderDate)*/
                        AdritextForLO += ` <tr>
                                                   <th scope="row">${AdricountLO}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                        AdricountLO++;
                    }
                    $("#OrderForList").html(AdritextForLO);

                }
            });

        }

    });

    $("#pills-profile-tab").on("click", function () {
        var textOnGoing = "";
        var countOnGoing = 1;
        $.ajax({
            url: "/Order/ListOrderOnGoing",
            type: "POST",
            data: { MemberName: memberName },
            success: function (data) {
                $.ajax({
                    url: "/Order/OrderStatusDateOnGoing",
                    type: "POST",
                    data: { MemberName: memberName },
                    success: function (response) {
                        for (let i = 0; i < response.length; i++) {
                            textOnGoing += ` <tr>
                                                   <th scope="row">${countOnGoing}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                            countOnGoing++;
                        }
                        $("#OrderForList").html(textOnGoing);
                    }
                });
            }
        });
    });

    //按鈕觸發全部訂單

    $("#pills-home-tab").on("click", function () {

        var AdricountAll = 1;
        var AdritextAll = "";
        $.ajax({
            url: "/Order/ListOrder",
            type: "POST",
            data: { MemberName: memberName },
            success: function (data) {
                $.ajax({
                    url: "/Order/OrderStatusDate",
                    type: "POST",
                    data: { MemberName: memberName },
                    success: function (response) {
                        console.log(response);
                        for (let i = 0; i < response.length; i++) {
                            console.log(data[i].cOrderID + data[i].cTotalPrice + response[i].cOrderStatus + response[i].cOrderDate)
                            AdritextAll += ` <tr>
                                                   <th scope="row">${AdricountAll}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                            AdricountAll++;
                        }
                        $("#OrderForList").html(AdritextAll);
                    }
                });
            }
        });
    });

    //按鈕觸發已完成訂單

    $("#pills-contact-tab").on("click", function () {
        var AdritextFinished = "";
        var AdricountFinished = 1;
        $.ajax({
            url: "/Order/ListOrderFinished",
            type: "POST",
            data: { MemberName: memberName },
            success: function (data) {

                $.ajax({
                    url: "/Order/OrderStatusDateFinished",
                    type: "POST",
                    data: { MemberName: memberName },
                    success: function (response) {

                        for (let i = 0; i < response.length; i++) {
                            AdritextFinished += ` <tr>
                                                   <th scope="row">${AdricountFinished}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                            AdricountFinished++;
                        }
                        $("#OrderForList").html(AdritextFinished);
                    }
                });
            }
        });
    });
}
