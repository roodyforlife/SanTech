$(document).ready(function () {
  //Открытие блока корзины
  $("#backet, #exit__basket").click(function () {
    if (!$('.basket').hasClass('_active')) {
      LoadBasket();
    }
    $(".basket").toggleClass("_active");
    $("body").toggleClass("_lock");
  });
  //нажатие за блоком корзины
  $(".basket").mouseup(function (e) {
    var div = $(".basket__content");
    if (!div.is(e.target) && div.has(e.target).length === 0) {
      $(".basket").toggleClass("_active");
      $("body").toggleClass("_lock");
    }
  });
  //нажатие на кнопку профиля
  $("#Profile").click(function () {
    $(".profile__pull__menu").toggleClass("_active__profile");
  });
  //Развернуть список категорий
  $('.content__search__category__button').click(function () {
    $('.content__search__category__list').toggleClass('_active__category');
  });
  //Разверзнуть список сортировки
  $('.content__search__sort__button').click(function () {
    $('.content__search__sort__list').toggleClass('_active__sort');
  });
  //Выбор категории
  $('.content__search__category__list__li').click(function () {
    $('.content__search__category__number').val(this.id);
    $('.content__search__category__list').toggleClass('_active__category');
    $('.content__search__category__button').html($(this).children('.category__list__li__text').html());
  });
  //Выбор сортировки
  $('.content__search__sort__list__li').click(function () {
    $('.content__search__sort__number').val(this.id);
    $('.content__search__sort__list').toggleClass('_active__sort');
    $('.content__search__sort__button').html($(this).children('.sort__list__li__text').html());
  });
  // Переключатель в админ панеле
  $('.adminPanel__header__menu__item').click(function () {
    $('.admin__toggleContent').removeClass('_active__adminPanel');
    $('.adminPanel__header__menu__item').removeClass('_active__adminMenuItem');
    $(this).addClass('_active__adminMenuItem');
    $(`#${$(this).attr("admin-type")}`).addClass('_active__adminPanel');
  });
  // input file
  $('input[type="file"]').change(function () {
    if ($('#myfile').val() != '') $('#myfile').prev().text('Выбрано фотографий: ' + $('#myfile')[0].files.length);
    else $('#myfile').prev().text('Выберите фотографию');
  });
});
function choice(evt, choice) {
  let tabcontent = document.querySelectorAll(".tabcontent");
  let tablinks = $(".tablinks");
  for (let i = 0; i < tabcontent.length; i++) {
    tabcontent[i].style.display = "none";
  }
  for (i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(
      "_active__button",
      ""
    );
  }
  document.getElementById(choice).style.display = "block";
  $(`#${evt}`).addClass("_active__button");
}
function copyTelephoneToClipboard(element) {
  var $temp = $("<input>");
  $("body").append($temp);
  $temp.val($(element).text()).select();
  document.execCommand("copy");
  $temp.remove();
}
function CountButton(int, id) {
  let value = +($(`#input__count${id}`).val());
  if (value == 1 && int > 0 || value > 1) {
    value += int;
    $(`#input__count${id}`).val(value);
  }
}
function LoadBasket()
{
  var formData = new FormData();
      $.ajax({
        url: "/Home/LoadBasket",
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
          $('.basket__content__orders').html(response);
        }
      });
}