$(document).ready(function () {
    $('.write__comment, .write__comment__content__exit').click(function () {
      $('.write__comment__block').toggleClass("_active__comment");
      $("body").toggleClass("_lock");
    });
    $(".write__comment__block").mouseup(function (e) {
      var div = $(".write__comment__content");
      if (!div.is(e.target) && div.has(e.target).length === 0) {
        $(".write__comment__block").toggleClass("_active__comment");
        $("body").toggleClass("_lock");
      }
    });
    const ratings = $('.comment__form__rating');
    if (ratings.length > 0) {
      initRating();
    }
    function initRating() {
      let ratingActive, ratingValue, ratingInputValue;
      for (let index = 0; index < ratings.length; index++) {
        const rating = ratings[index];
        initRatings(rating);
      }
    }
    function initRatings(rating) {
      initRatingVars(rating);
      setRatingActiveWidth();
      setRating(rating);
  
    }
    function initRatingVars(rating) {
      ratingActive = $('.rating__active');
      ratingValue = $('.rating__value');
      ratingInputValue = $('.rating__input__value')
    }
    function setRatingActiveWidth(index = ratingValue.html()) {
      const ratingActiveWidth = index / 0.03;
      ratingActive.css("width", `${ratingActiveWidth}`);
    }
    function setRating(rating) {
      const ratingItems = $('.rating__item');
      for (let index = 0; index < ratingItems.length; index++) {
        const ratingItem = ratingItems[index];
        $(ratingItem).mouseenter(function (e) {
          initRatingVars(rating);
          setRatingActiveWidth(ratingItem.value);
        });
        $(ratingItem).mouseleave(function (e) {
          setRatingActiveWidth();
        });
        $(ratingItem).click(function (e) {
          initRatingVars(rating);
          ratingInputValue.val(index + 1);
          ratingValue.html(index + 1);
          setRatingActiveWidth();
        });
      }
    }
    $('.reviews__content__item__answer').click(function(e)
    {
        $(this).parent().children('.reviews__content__item__answer__block').css('display', 'block');
    });
    $('.reviews__content__item__answer__input__cancel').click(function(){
        $(this).parents('.reviews__content__item__answer__block').css('display', 'none');
    });
  });