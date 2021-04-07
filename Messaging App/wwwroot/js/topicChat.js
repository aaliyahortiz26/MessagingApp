"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

//Disable send button until connection is established
document.getElementById("sendTopicMessageButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var label = document.createElement("label");
    label.textContent = encodedMsg;
    document.getElementById("messagesLabelNotFromDatabase").appendChild(label);
});

connection.start().then(function () {
    document.getElementById("sendTopicMessageButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendTopicMessageButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageTextArea").value;
    connection.invoke("SendMessageTopic", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
