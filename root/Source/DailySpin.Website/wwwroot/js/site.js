"use strict";
var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/roulette").build();
// accept data from server
hubConnection.on("PlaceBet", function (viewModel, color) {
    let betContainer = document.createElement("div");
    let betElement = document.createElement("p");
    let betElement2 = document.createElement("p");
    betElement.className = "betglass";
    let imgp = document.createElement("IMG");
    imgp.src = `data:image/png;base64,${viewModel.userImage}`;
    imgp.className = "betglass";
    betElement.textContent = viewModel.userName;
    betElement2.textContent = viewModel.userBet;
    betElement2.className = "bet-right-side";
    betContainer.append(imgp, betElement, betElement2);
    betContainer.className = "betglass"
    if (color == "blue") {
        document.getElementById("Blue").appendChild(betContainer);
    }
    else if (color == "green") {
        document.getElementById("Green").appendChild(betContainer);
    }
    else {
        document.getElementById("Yellow").appendChild(betContainer);
    }
});
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
    hubConnection.invoke("PlaceBetf", "blue", +bet).catch(function (err) {
            return console.error(err.toString());
        });
    return true;
});
document.getElementById("Green b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", "green", +bet).catch(function (err) {
            return console.error(err.toString());
        });
    return true;
});
document.getElementById("Yellow b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", "yellow", +bet).catch(function (err) {
            return console.error(err.toString());
        });
    return true;
});


