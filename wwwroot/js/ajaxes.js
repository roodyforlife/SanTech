// Sign Out from account
$(document).ready(function () {
    $('.exit__button').click(function () {
        var formData = new FormData();
        $.ajax({
            url: "/Home/SignOutAccount",
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                $('.profile__pull__menu__logged').css("display", "none");
                $('.profile__pull__menu__list').html(response);
                $('.exit__alert').addClass('_active__exit__alert');
                $('.menu__item__count').remove();
                setTimeout(function () {
                    $('.exit__alert').removeClass('_active__exit__alert');
                }, 3000);
            }
        });
    });
    // Add New Product
    $(".adminPanel__create__content__input__button").click(function () {
        var formData = new FormData()
        let uploadedFile = $('#myfile');
        //formData.append('UploadedFile', uploadedFile[0].files[0])
        formData.append('UploadedFile', uploadedFile[0].files[0])
        formData.append("Title", $('#Title').val());
        formData.append("Desc", $('#Desc').val());
        formData.append("SaleProcent", $('#SaleProcent').val());
        formData.append("BonusNumber", $('#BonusNumber').val());
        formData.append("Cost", $('#Cost').val());
        formData.append('CategoryId', $('.adminPanel__category__number').val());

        $.ajax({
            url: "/AdminPanel/AddNewProduct",
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response) {
                    alert("Ошибка при введении данных. Возможно вы не заполнили какое-то поле.")
                }
                else {
                    $('.admin__create__input').val(null);
                    $('.adminPanel__category__number').val(0);
                    $('.adminPanel__category__button').html('Категория товара')
                    $('#myfile').prev().text('Выберите фотографию');
                    alert("Товар добавлен");
                }
            }
        });
    });
    $('.basket__content__clear').click(function () {
        $.ajax({
            url: "/Home/DeleteAllBasket",
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: false,
            success: function (response) {
                $('.menu__item__count').html(0);
                LoadBasket();
            }
        });
    });
    $('.favorites__content__clear').click(function () {
        $.ajax({
            url: "/Home/DeleteAllFavorites",
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: false,
            success: function (response) {
                $('.menu__item__count__favorites').html(0);
                LoadFavorites();
            }
        });
    });
});

function AddToBasket(Id, userEmail) {
    if (userEmail.length > 0) {
        var formData = new FormData();
        formData.append("Id", Id);
        $.ajax({
            url: "/Home/AddToBasket",
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response) {
                    $('.menu__item__count').html(parseInt($('.menu__item__count').html()) + 1);
                }
            }
        });
    } else {
        alert('Вы не авторизованы!')
    }
}
function AddToFavourites(Id, userEmail) {
    if (userEmail.length > 0) {
        var formData = new FormData();
        formData.append("Id", Id);
        $.ajax({
            url: "/Home/AddToFavourites",
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response) {
                    $('.menu__item__count__favorites').html(parseInt($('.menu__item__count__favorites').html()) + 1);
                }
            }
        });
    } else {
        alert('Вы не авторизованы!')
    }
}
function DeleteFromBasket(basketId) {
    //$(`#${basketId}`).remove();
    $('.menu__item__count').html(parseInt($('.menu__item__count').html()) - 1);
    var formData = new FormData();
    formData.append("basketId", basketId);
    $.ajax({
        url: "/Home/DeleteFromBasket",
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            LoadBasket();
        }
    });
}
function DeleteFromFavorites(favoriteId) {
    $('.menu__item__count__favorites').html(parseInt($('.menu__item__count__favorites').html()) - 1);
    var formData = new FormData();
    formData.append("favoriteId", favoriteId);
    $.ajax({
        url: "/Home/DeleteFromFavorites",
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            LoadFavorites();
        }
    });
}
function DeleteProduct(productId) {
    var formData = new FormData();
    formData.append("productId", productId);
    $.ajax({
        url: "/AdminPanel/DeleteProduct",
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $(`#productId_${productId}`).css("opacity", "0.3");
            $(`#productId_${productId}`).css("pointer-events", "none");
        }
    });
}