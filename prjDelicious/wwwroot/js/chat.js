"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
document.getElementById("sendimgButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);    
    li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveImgMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} says `;
    var img = document.createElement("img");
    document.getElementById("messagesList").appendChild(img);
    //https://localhost:44380/uploads/
    img.src = "/chatimg/" + message;

    img.setAttribute("width", "200px");
    img.setAttribute("height", "200px");

    //if (img.width > 300) {
    //    var newheight = img.height * (img.width - 300) / img.width

    //    img.setAttribute("width", "90px");
    //    img.setAttribute("height", newheight + "px");
    //}
    //else if (img.height > 200) {
    //    var newwidth = img.width * (img.height - 200) / img.height
    //    img.setAttribute("width", newwidth+"px");
    //    img.setAttribute("height",  "200px");
    //}

    //img.src = "data:image/png;base64," + message;


});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    document.getElementById("sendimgButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    if (message == "") {
        alert("未輸入訊息");
        return;
    }
    else if (user == "") {
        alert("未輸入名子");
        return;
    }
    else {
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});

document.getElementById("sendimgButton").addEventListener("click", function (event) {
    var m_img = document.getElementById("imageInput").value;
    var user = document.getElementById("userInput").value;
    if (m_img == "") {
        alert("未選擇照片");
        return;
    }
    else if (user == "")
    {
        alert("未輸入名子");
        return;
    }
    else {
        const formData = new FormData(document.memberInfo)
        fetch('/ApiChat/ImageInput', {
            method: "POST",
            body: formData
        }).then(response => response.text())
            .then(data => {
                var user = document.getElementById("userInput").value;
                connection.invoke("SendImgMessage", user, data).catch(function (err) {
                    return console.error(err.toString());
                });
            });
        event.preventDefault();
    }
});




////function ChatRoomStart(MemberNameForChatStart) {


////var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//////Disable send button until connection is established
////    document.getElementById("sendButton").disabled = true;
////    document.getElementById("sendimgButton").disabled = true;





////connection.on("ReceiveMessage", function (user, message) {
////    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
////    var when = new Date().toLocaleTimeString();
////    var encodedMsg = /*user + " says " +*/ '<p>'+ msg+'<span class="chat_message_time">' + when +'</span></p>';
////    var li = document.createElement("li");
////    li.innerHTML = encodedMsg;

////    $.ajax({
////        url: "/adChatRoom/getMessage",
////        type: "POST",
////        data: { MemberName: user, Content: message, MessageTime: when },

////    });

////    connection.on("ReceiveImgMessage", function (user, message) {
////        var li = document.createElement("li");
////        document.getElementById("messagesList").appendChild(li);
////        li.textContent = `${user} says `;
////        var img = document.createElement("img");
////        document.getElementById("messagesList").appendChild(img);
////        //https://localhost:44380/uploads/
////        img.src = "https://localhost:44333//chatimg/" + message;

////        img.setAttribute("width", "200px");
////        img.setAttribute("height", "200px");

////        //if (img.width > 300) {
////        //    var newheight = img.height * (img.width - 300) / img.width

////        //    img.setAttribute("width", "90px");
////        //    img.setAttribute("height", newheight + "px");
////        //}
////        //else if (img.height > 200) {
////        //    var newwidth = img.width * (img.height - 200) / img.height
////        //    img.setAttribute("width", newwidth+"px");
////        //    img.setAttribute("height",  "200px");
////        //}

////        //img.src = "data:image/png;base64," + message;


////    });

////    var Picture ="";
////    if (user == '管理員') {
////        Picture = 'src = "img/adrChatRoom/adm2.jpg" class="md-user-image" >';
        
////    }
////    else {
////        Picture = 'src = "img/adrChatRoom/Customer_Icon.jpg" class="md-user-image">';
////    }
////    if (msg == "") { return; }
////    if (user == MemberNameForChatStart)
////    {
////    /*自己都放右邊  */
////        if (自己有沒有連續留言 == 0)
////        {

////            $("#chatStartPoint").append('<div class="chat_message_wrapper chat_message_right">' + '<div class= "chat_user_avatar">' + '<a target="_blank">' + '<img alt="' + user + '" title="' + user + '"' + Picture + '</a>' + '</div>' + '<ul class="chat_message messagesListForClient" >');
////            /*            document.getElementById("messagesListForClient").appendChild(li);*/
////            $('ul.messagesListForClient:eq(-1)').append(li);

////            自己有沒有連續留言 = 1;
////        }
////        else
////        {
////            $('ul.messagesListForClient:eq(-1)').append(li);
/////*            document.getElementById("messagesListForClient").appendChild(li);*/
////        }
////        對方有沒有連續留言 = 0;
////        document.getElementById('messageInput').value = '';
////    }

////    else {
////        if (對方有沒有連續留言 == 0) {
////            $("#chatStartPoint").append('<div class="chat_message_wrapper">' + '<div class="chat_user_avatar">' + '<a target="_blank">' + '<img alt="' + user + '" title="' + user + '"' + Picture + '</a>' + '</div>' + '<ul class="chat_message messagesList">');
////        /*document.getElementById("messagesList").appendChild(li);*/
////            $('ul.messagesList:eq(-1)').append(li);

////            對方有沒有連續留言 = 1;
////        }
////        else
////        {
////            $('ul.messagesList:eq(-1)').append(li);
/////*            document.getElementById("messagesList").appendChild(li);*/
////        }
////        自己有沒有連續留言 = 0;
////        document.getElementById('messageInput').value = '';
////    }
////});

////connection.start().then(function () {
////    document.getElementById("sendButton").disabled = false;
////    document.getElementById("sendimgButton").disabled = false;
////}).catch(function (err) {
////    return console.error(err.toString());
////});

////document.getElementById("sendButton").addEventListener("click", function (event) {
////    var user = MemberNameForChatStart;
////    var message = document.getElementById("messageInput").value;
////    connection.invoke("SendMessage", user, message).catch(function (err) {
////        return console.error(err.toString());
////    });
////    event.preventDefault();
////});

////    document.getElementById("sendimgButton").addEventListener("click", function (event) {

////        const formData = new FormData(document.memberInfo)

////        fetch('/ApiChat/ImageInput', {
////            method: "POST",
////            body: formData
////        }).then(response => response.text())
////            .then(data => {
////                var user = document.getElementById("userInput").value;
////                connection.invoke("SendImgMessage", user, data).catch(function (err) {
////                    return console.error(err.toString());
////                });

////            });

////        event.preventDefault();
////    });

////}

