const container = document.querySelector(".container");
const seats = document.querySelectorAll(".row .seat");
const count = document.getElementById("count");
const total = document.getElementById("total");


$.get('/Home/distributeChairTier',
    {},
    (jsonData) => {
        jsonData.map(chairs => {
            chairs.Chairs.map(chair => {
                const seat = Array.from(seats).find(seat => seat.id === chair)
                seat.classList.add(chairs.Tier)
            })
        })
    }
)

populateUI();


function updateSelectedCount() {
    const selectedSeats = document.querySelectorAll(".row .seat.selected");

    const seatsIndex = [...selectedSeats].map((seat) => [...seats].indexOf(seat)+1);

    localStorage.setItem("selectedSeats", JSON.stringify(seatsIndex));

    $.post(
        "/Home/calculatePrice",
        { selectedChairs: seatsIndex },
        (total) => {
            console.log(total)
        }
    )
    
}


function populateUI() {
    const selectedSeats = JSON.parse(localStorage.getItem("selectedSeats"));

    if (selectedSeats !== null && selectedSeats.length > 0) {
        seats.forEach((seat, index) => {
            if (selectedSeats.indexOf(index) > -1) {
                seat.classList.add("selected");
            }
        });
    }
}

container.addEventListener("click", (e) => {
    if (
        e.target.classList.contains("seat") &&
        !e.target.classList.contains("sold")
    ) {
        e.target.classList.toggle("selected");
        updateSelectedCount();
    }
});

updateSelectedCount();