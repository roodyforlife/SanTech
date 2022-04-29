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
                setTimeout(function () {
                    $('.exit__alert').removeClass('_active__exit__alert');
                }, 3000);
            }
        });
    });
});
// Add New Product
function CreateProduct()
{
let uploadedFile = $('#myfile');
var formData = new FormData();
formData.append('UploadedFile', uploadedFile[0].files[0])
formData.append("Title", $('#Title').val());
formData.append("Desc", $('#Desc').val());
formData.append("SaleProcent", $('#SaleProcent').val());
formData.append("BonusNumber", $('#BonusNumber').val());
formData.append("Cost", $('#Cost').val());
$.ajax({
    url: "../AdminPanel/CreateNewProduct",
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
            console.log($('.admin__create__input').val(null));
            $('#myfile').prev().text('Выберите фотографию');
            alert("Товар добавлен");
        }
    }
});
}