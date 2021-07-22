function chatsBefore(MemberNameLogin) {
    var 自己有沒有連續留言 = 0;
    var 對方有沒有連續留言 = 0;

    $.ajax({
        url: "/adChatRoom/ListMessages",
        type: "POST",
        data: { MemberName: MemberNameLogin },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var memberName = data[i].cMemberName
                var msg = data[i].cContent;
                var when = data[i].cMessageTime
                var encodedMsg = /*user + " says " +*/ '<p>' + msg + '<span class="chat_message_time">' + when + '</span></p>';
                var li = document.createElement("li");
                li.innerHTML = encodedMsg;


                var Picture = "";
                if (memberName == '管理員') {
                    Picture = 'src = "img/adrChatRoom/adm2.jpg" class="md-user-image" >';
                }
                else {
                    Picture = 'src = "img/adrChatRoom/Customer_Icon.jpg" class="md-user-image">';
                }

                /*如果是自己，放右邊*/
                if (data[i].cMemberName == MemberNameLogin) {
                    if (自己有沒有連續留言 == 0) {
                        $("#chatStartPoint").append('<div class="chat_message_wrapper chat_message_right">' + '<div class= "chat_user_avatar">' + '<a target="_blank">' + '<img alt="' + memberName + '" title="' + memberName + '"' + Picture + '</a>' + '</div>' + '<ul class="chat_message messagesListForClient" >');
                        /*            document.getElementById("messagesListForClient").appendChild(li);*/
                        $('ul.messagesListForClient:eq(-1)').append(li);

                        自己有沒有連續留言 = 1;
                    }
                    else {
                        $('ul.messagesListForClient:eq(-1)').append(li);
                        /*            document.getElementById("messagesListForClient").appendChild(li);*/
                    }
                    對方有沒有連續留言 = 0;
                }
                else {
                    if (對方有沒有連續留言 == 0) {
                        $("#chatStartPoint").append('<div class="chat_message_wrapper">' + '<div class="chat_user_avatar">' + '<a target="_blank">' + '<img alt="' + memberName + '" title="' + memberName + '"' + Picture + '</a>' + '</div>' + '<ul class="chat_message messagesList">');
                        /*document.getElementById("messagesList").appendChild(li);*/
                        $('ul.messagesList:eq(-1)').append(li);

                        對方有沒有連續留言 = 1;
                    }
                    else {
                        $('ul.messagesList:eq(-1)').append(li);
                        /*            document.getElementById("messagesList").appendChild(li);*/
                    }
                    自己有沒有連續留言 = 0;
                }
            }
        }
    });

}