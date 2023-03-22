"use strict";
async function startConnection() {
    var hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/roulette")
        .build();
    await hubConnection.start();
    let ItemGlobalName = "";
    if (hubConnection.state === signalR.HubConnectionState.Connected) {
        console.log("SignalR connected.");
    }
    else {
        console.log("Not yet");
    }
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
    hubConnection.on("Spin", async function () {
        //paste here starting timer
        startingTimer();
        /////pasted
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
        await hubConnection.invoke("GetActualBalanceAsync")
            .then(UserBalance => {
                document.getElementsByClassName('balance-user')[1].innerHTML = Number(UserBalance)
            });
    })
    hubConnection.on("ReturnError", function (text) {
        document.getElementById("errorlbl").innerText = `Error: ${text}`;
        displayError(true);
    });

    // send data to server
    document.getElementById("Blue b").addEventListener("click", function () {
        AddBet("blue");
        return true;
    });
    document.getElementById("Green b").addEventListener("click", function () {
        AddBet("green");
        return true;
    });
    document.getElementById("Yellow b").addEventListener("click", function () {
        AddBet("yellow");
        return true;
    });
    //vanila functions
    function AddBet(color) {
        var secondsSpan = document.getElementById("seconds");
        if (!isEmpty(secondsSpan.innerText)) {
            const bet = makeBet();
            hubConnection.invoke("PlaceBetf", color, +bet).catch(function (err) {
                return console.error(err.toString());
            });
        } else {
            alert("Please, wait a few secodns to load  💛");
        }
        displayError(false);
    }
    function isEmpty(value) {
        return (value == null || (typeof value === "string" && value.trim().length === 0));
    }
    function makeBet() {
        const bet = document.getElementById("bet").value;
        if (!isNaN(parseInt(bet))) {
            hubConnection.invoke("GetActualBalanceAsync")
                .then(UserBalance => {
                    document.getElementsByClassName('balance-user')[1].innerHTML = (UserBalance - parseInt(bet)).toString();
                });
        }
        return bet;
    }
    //bet panel
    document.getElementById("reloadBalance").addEventListener("click", async function reload() {
        await hubConnection.invoke("GetActualBalanceAsync")
            .then(UserBalance => {
                document.getElementsByClassName('balance-user')[1].innerHTML = Number(UserBalance)
            });
    });
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
        hubConnection.invoke("GetActualBalanceAsync")
            .then(UserBalance => {
                if (parseInt(document.getElementById("bet").value) !== UserBalance) {
                    plus(UserBalance);
                }
            });
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
            document.getElementById("navigate-panel").setAttribute("style", "height:80px");
        } else {
            x.style.display = "none";
            document.getElementById("navigate-panel").setAttribute("style", "height:50px");
        }
    }
    //timer
    function countdown(endtime, isSeven) {
        var secondsSpan = document.getElementById("seconds");

        function updateClock() {
            var t = getTimeRemaining(endtime);

            secondsSpan.innerHTML = ("0" + t.seconds).slice(-2);

            if (t.total <= 0 && isSeven === true) {
                clearInterval(timeinterval);
                startAnimation();
                startTimer();
            }
            else if (t.total <= 0) {
                clearInterval(timeinterval);
                hubConnection.invoke("RunAsyncServerMethod", ItemGlobalName);
                loadHistory();
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
        var deadline = new Date(Date.parse(new Date()) + 5 * 1000); // 5 seconds from now
        countdown(deadline, false);
        //disable buttons
        document.getElementById("Blue b").disabled = true;
        document.getElementById("Green b").disabled = true;
        document.getElementById("Yellow b").disabled = true;
    }
    function startingTimer() {
        var deadline = new Date(Date.parse(new Date()) + 10 * 1000); // 10 seconds from now
        countdown(deadline, true);
        //enable buttons
        document.getElementById("Blue b").disabled = false;
        document.getElementById("Green b").disabled = false;
        document.getElementById("Yellow b").disabled = false;
    }
    // ANIMATION JS
    const cells = 31

    async function loadHistory() {
        document.querySelector('.history').remove()
        document.querySelector('.history-scope').innerHTML = `
    <ul class="history"></ul>
  `
        const list = document.querySelector('.history')
        var myResult;
        await hubConnection.invoke("GetHistoryList")
            .then(items => { myResult = items });
        for (let i = 0; i < myResult.length; i++) {
            const item = myResult[i];
            const li = document.createElement('li')
            li.classList.add('history__item')
            li.innerHTML = `
          <img src="/${item.image}" alt="" />
        `
            list.append(li)
        }
    }

    async function generateItems() {
        document.querySelector('.list').remove()
        document.querySelector('.scope').innerHTML = `
    <ul class="list"></ul>
  `

        const list = document.querySelector('.list')
        var myResult;
        await hubConnection.invoke("FirstGenerateItems")
            .then(items => { myResult = items });
        for (let i = 0; i < cells; i++) {
            const item = myResult[i];
            if (i == 15) {
                ItemGlobalName = item.name;
            }
            const li = document.createElement('li')
            li.setAttribute('data-item', JSON.stringify(item))
            li.classList.add('list__item')
            li.innerHTML = `
          <img src="/${item.image}" alt="" />
        `

            list.append(li)
        }
    }
    await generateItems();
    await loadHistory();

    let isStarted = false
    let isFirstStart = false

    async function startAnimation() {
        if (isStarted) return
        else isStarted = true

        if (!isFirstStart) await generateItems()
        else isFirstStart = false
        const list = document.querySelector('.list')

        setTimeout(() => {
            list.style.left = '50%'
            list.style.transform = 'translate3d(-50%, 0, 0)'
        }, 0)

        const items = list.querySelectorAll('li')
        const item = items[15]
        list.addEventListener('transitionend', () => {
            isStarted = false
            item.classList.add('active')
            const data = JSON.parse(item.getAttribute('data-item'))
            console.log(data);
        }, { once: true });
    }
}
startConnection();