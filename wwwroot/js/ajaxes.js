$(document).ready(function () {
  $('.exit__button').click(function(){
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
    setTimeout(function(){ 
      $('.exit__alert').removeClass('_active__exit__alert');
    }, 3000);
          }
      });
   });
});