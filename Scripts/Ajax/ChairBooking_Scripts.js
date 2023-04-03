const container = document.querySelector(".container");
const seats = document.querySelectorAll(".row .seat");
const count = document.getElementById("count");
const total = document.getElementById("total");

localStorage.clear();
populateUI();

function updateSelectedCount() {
    const selectedSeats = document.querySelectorAll(".row .seat.selected");

    const seatsIndex = [...selectedSeats].map((seat) => [...seats].indexOf(seat)+1);

    localStorage.setItem("selectedSeats", JSON.stringify(seatsIndex));

    /* Chuyển từ 1 mảng sang objet theo kiểu {tier: số ghế thuộc tier đó được chọn} */
     function countChairByTier(chairTiers) {
        const listTierOfSelectedChairs = chairTiers.reduce((accumulator, tier) => {
    /* Lọc qua các phần tử, kiểm tra các ghế bằng id lấy từ Storage xem có class là tier đó không. Đếm rồi tạo cặp Key-Value */
            let count = 0
            seatsIndex.forEach(index => {
                if (document.getElementById(index).classList.contains(tier)) {
                    count++
                }  
            })
            return { ...accumulator, [tier]: count } 
        }, {})
        return listTierOfSelectedChairs
    }

    /* Req tới getChairTiers để lấy ra các tier của ghế, sau khi xử lý thành dạng object thì gửi lại về cho server để sv tính tổng giá và gửi lại */

    $.get(
        "/Home/getChairTiers",
        {},
        (listChairTier) => {
            const listTierOfSelectedChairs = countChairByTier(listChairTier)
     
             $.post(     
                 "/Home/calculatePrice",
                 {
                     tierWithChairs: JSON.stringify(listTierOfSelectedChairs),
                     jsonChairIds: JSON.stringify(seatsIndex)
                 },
                (total) => {
                    console.log(total)
                    $(".showtime-info--chair_price").text(total)
                }
            )
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

    $.get('/Home/distributeChairTier',
        {},
        (jsonData) => {
            jsonData.forEach(chairs => {
                chairs.Chairs.forEach(chair => {
                    const seat = Array.from(seats).find(seat => seat.id === chair)
                    seat.classList.add(chairs.Tier)
                })
            })
        }
    )
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

/*updateSelectedCount();*/

//function nextToTicketBooking(idShowtime) {
//     = `@Url.Action(bookTicket,Home,new { idShowtime=${idShowtime}, price = ${$()} })``
//}