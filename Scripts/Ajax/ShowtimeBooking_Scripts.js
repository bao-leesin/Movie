$('.Paracetamol').on('click', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();

    const chosenId = $(this).attr('id');
    const chosenElement = document.getElementById(chosenId);
    const parentList = chosenElement.parentElement;

    const currentElement = parentList.getElementsByClassName('current')[0]

    !!currentElement && currentElement.classList.remove('current')
    
    chosenElement.classList.add("current")

    let currentElements = document.getElementsByClassName('current')

    var currentDay = currentElements[0].getAttribute('data-day')
    var currentCity = currentElements[1].getAttribute('data-city')
    var currentType = currentElements[2].getAttribute('data-type')

    const paramurl = window.location.search.split('=');

    if (currentElements.length == 3) {
        const $showtimeDiv = $('.container_cinema--list')
        $(document).ready(function () {
            $(".container_cinema").load("/Home/filterShowTime", { cityName: currentCity, showDayInput: currentDay, type: currentType, IdFilm: paramurl[1] });
        }
        )}
  
        
        
});

