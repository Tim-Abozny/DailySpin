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
hubConnection.on("Spin", function (rl) {
    displayResult(true);
    if (rl === "blue") {
        document.getElementById("result").innerText = "BLUE WON";
}
    else if (rl === "yellow") {
        document.getElementById("result").innerText = "YELLOW WON";
    }
    else if (rl === "green") {
        document.getElementById("result").innerText = "GREEN WON";
    }
    else {
        document.getElementById("result").innerText = "EMPTY SPIN";
    }
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
    startTimer()
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
    let number = parseInt(document.getElementById("bet").value)
    if (!isNaN(number)) {
        let NewBalance = parseInt(document.getElementById("balance").value);
        document.getElementById("balance").value = NewBalance - parseInt(bet);
        document.getElementsByClassName('balance-user')[1].innerHTML = document.getElementById("balance").value;
    }
    displayError(false);
    return true;
});
document.getElementById("Green b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", "green", +bet).catch(function (err) {
            return console.error(err.toString());
    });
    let number = parseInt(document.getElementById("bet").value)
    if (!isNaN(number)) {
        let NewBalance = parseInt(document.getElementById("balance").value);
        document.getElementById("balance").value = NewBalance - parseInt(bet);
        document.getElementsByClassName('balance-user')[1].innerHTML = document.getElementById("balance").value;
    }
    
    displayError(false);
    return true;
});
document.getElementById("Yellow b").addEventListener("click", function () {

    const bet = document.getElementById("bet").value;
    hubConnection.invoke("PlaceBetf", "yellow", +bet).catch(function (err) {
            return console.error(err.toString());
    });
    let number = parseInt(document.getElementById("bet").value)
    if (!isNaN(number)) {
        let NewBalance = parseInt(document.getElementById("balance").value);
        document.getElementById("balance").value = NewBalance - parseInt(bet);
        document.getElementsByClassName('balance-user')[1].innerHTML = document.getElementById("balance").value;
    }
    displayError(false);
    return true;
});
//vanila functions
//bet panel
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
//error
function displayError(enable) {
    var x = document.getElementById("errorlbl");
    if (enable === true) {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}
//result
function displayResult(enable) {
    var x = document.getElementById("result");
    if (enable === true) {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}
//timer
function countdown(endtime) {
    var secondsSpan = document.getElementById("seconds");

    function updateClock() {
        var t = getTimeRemaining(endtime);

        secondsSpan.innerHTML = ("0" + t.seconds).slice(-2);

        if (t.total <= 0) {
            clearInterval(timeinterval);
        }
    }

    updateClock();
    var timeinterval = setInterval(updateClock, 1000);
}

function getTimeRemaining(endtime) {
    var total = Date.parse(endtime) - Date.parse(new Date());
    var seconds = Math.floor((total / 1000) % 60);

    return {
        total: total,
        seconds: seconds,
    };
}

function startTimer() {
    var deadline = new Date(Date.parse(new Date()) + 15 * 1000); // 15 seconds from now
    countdown(deadline);
    start();
}

// ANIMATION JS
const cells = 31

function generateItems() {
    document.querySelector('.list').remove()
    document.querySelector('.scope').innerHTML = `
    <ul class="list"></ul>
  `

    const list = document.querySelector('.list')
    var myResult;
    for (let i = 0; i < cells; i++) {
        hubConnection.invoke("GetItem")
            .then(item => { myResult = item })
            .then(() => console.log(myResult));
        if (myResult !== undefined) {
            const item = myResult;
            const li = document.createElement('li')
            li.setAttribute('data-item', JSON.stringify(item))
            li.classList.add('list__item')
            li.innerHTML = `
          <img src="/${item.image}" alt="" />
        `

            list.append(li)
        }
    }
}

generateItems()

let isStarted = false
let isFirstStart = true

function start() {
    if (isStarted) return
    else isStarted = true

    if (!isFirstStart) generateItems()
    else isFirstStart = false
    const list = document.querySelector('.list')

    setTimeout(() => {
        list.style.left = '50%'
        list.style.transform = 'translate3d(-50%, 0, 0)'
    }, 0)

    const item = list.querySelectorAll('li')[15]
    list.addEventListener('transitionend', () => {
        isStarted = false
        if (item !== undefined) {
            item.classList.add('active')
            const data = JSON.parse(item.getAttribute('data-item'))
            console.log(data);
        }
    }, { once: true })

}