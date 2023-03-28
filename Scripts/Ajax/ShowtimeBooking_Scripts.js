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

    let currentDay = currentElements[0].getAttribute('data-day')
    let currentCity = currentElements[1].getAttribute('data-city')
    let currentType = currentElements[2].getAttribute('data-type')

    const $showtimeDiv = $('.container_cinema--list')
    let url
    if (currentElements.length == 3) {
        url = `https://localhost:44355/Home/filterShowTime/${currentDay}/${currentCity}/${currentType} `
    }

    $.get(url, function (data) {
        $showtimeDiv.replaceWith(data);
    });
        
        

  
});

