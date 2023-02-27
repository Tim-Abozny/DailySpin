"use strict";
console.log("second line");
var hubConnection = new signalR.HubConnectionBuilder().withUrl("/BetsGlass").build();
console.log("after connect trying");
// accept data from server
hubConnection.on("PlaceBet", function (viewModel) {
    let betElement = document.createElement("p");
    let imgp = document.createElement("IMG");
    let srcString = viewModel.UserImage;
    imgp.src = "data:image/png;base64${srcString}"
    betElement.textContent = viewModel.UserName;
    betElement.textContent = viewModel.UserBet;
    document.getElementById("Blue").appendChild(imgp);
    document.getElementById("Blue").appendChild(betElement);
});
console.log("after on");
hubConnection.on("ReturnError", function (text) {
    document.getElementById("errorlbl").innerText = `Error: ${text}`;
});

hubConnection.start().then(function () {
        console.log("SignalR connected.");
    }).catch(function (err) {
        return console.error(err.toString());
    });

// send data to server
document.getElementById("Blue b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", { "glasscolor": "blue", "bet": parseInt(bet) }).catch(function (err) {
            return console.error(err.toString());
        });
    return true;
});
document.getElementById("Green b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", { "glasscolor": "green", "bet": parseInt(bet) }).catch(function (err) {
            return console.error(err.toString());
        });
    return true;
});
document.getElementById("Yellow b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", { "glasscolor": "yellow", "bet": parseInt(bet) }).catch(function (err) {
            return console.error(err.toString());
        });
    return true;
});


