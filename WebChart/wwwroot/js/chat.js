"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
document.getElementById("sendimgButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveImgMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);   
    li.textContent = `${user} says `;
    var img = document.createElement("img");
    document.getElementById("messagesList").appendChild(img);
    //https://localhost:44380/uploads/
    img.src = "/uploads/" + message;
     
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
    
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendimgButton").addEventListener("click", function (event) {
     
    const formData = new FormData(document.memberInfo)
    
    fetch('/api/ImageInput', {
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
