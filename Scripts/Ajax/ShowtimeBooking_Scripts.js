$('.Paracetamol').on('click', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();

    var chosenId = $(this).attr('id');
    var chosenElement = document.getElementById(chosenId);
    chosenElement.classList.add("current")
    console.log(chosenId);
});

