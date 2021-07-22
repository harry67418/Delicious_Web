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
    img.src = "/chatimg/" + message;
    img.setAttribute("width", "200px");
    img.setAttribute("height", "200px");   
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
    
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendimgButton").addEventListener("click", function (event) {
     
    const formData = new FormData(document.memberInfo)
    
    fetch('/ChatApi/ImageInput', {
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
});
