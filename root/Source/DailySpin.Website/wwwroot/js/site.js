"use strict";
var hubConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/roulette").build();
// accept data from server
hubConnection.on("PlaceBet", function (viewModel, color) {
    let betContainer = document.createElement("div");
    let betElement = document.createElement("p");
    let betElement2 = document.createElement("p");
    betElement.className = "bet-list";
    let imgp = document.createElement("IMG");

    imgp.src = `data:image/png;base64,${viewModel.userImage}`;
    imgp.className = "betglass";

    betElement.textContent = viewModel.userName;
    betElement2.textContent = viewModel.userBet;
    betElement2.className = "bet-right-side";

    betContainer.append(imgp, betElement, betElement2);
    betContainer.className = "bet-list"

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
hubConnection.on("Spin", function () {
    let countB = document.getElementById("Blue").childNodes.length;
    let countG = document.getElementById("Green").childNodes.length;
    let countY = document.getElementById("Yellow").childNodes.length;
    while (countB > 0) {
        document.getElementById("Blue").firstChild.remove();
        countB--;
    }
    while (countG > 0) {
        document.getElementById("Green").firstChild.remove();
        countG--;
    }
    while (countY > 0) {
        document.getElementById("Yellow").firstChild.remove();
        countY--;
    }
})
hubConnection.on("ReturnError", function (text) {
    document.getElementById("errorlbl").innerText = `Error: ${text}`;
    displayError(true);
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
    displayError(false);
    return true;
});
document.getElementById("Green b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", "green", +bet).catch(function (err) {
            return console.error(err.toString());
        });
    displayError(false);
    return true;
});
document.getElementById("Yellow b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", "yellow", +bet).catch(function (err) {
            return console.error(err.toString());
        });
    displayError(false);
    return true;
});
//vanila functions
document.getElementById("Clear").addEventListener("click", function clear() {
    document.getElementById("bet").value = 0;
});
document.getElementById("+1").addEventListener("click", function plus1() {
    plus(1);
});
document.getElementById("+10").addEventListener("click", function plus10() {
    plus(10);
});
document.getElementById("+100").addEventListener("click", function plus100() {
    plus(100);
});
document.getElementById("+1000").addEventListener("click", function plus1000() {
    plus(1000);
});
document.getElementById("MAX").addEventListener("click", function plusMax() {
    plus(0);
});
function plus(value) {
    let number = parseInt(document.getElementById("bet").value);
    if (isNaN(number)) {
        number = 0;
    }
    number += value;
    document.getElementById("bet").value = number;
}
function displayError(enable) {
    var x = document.getElementById("errorlbl");
    if (enable === true) {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}