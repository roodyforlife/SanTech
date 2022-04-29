
var from = 20;
let count = 10;
$(window).resize(function () {
    firstLoads();
    $(window).resize(function () { });
});
$(document).ready(function(){
    firstLoads();
});
function firstLoads() {
    let blockHeight = document.querySelector('.ads__content').offsetHeight;
    let windowHeight = window.innerHeight;
    if (blockHeight < windowHeight) {
        GetAdditionalProducts(from, count);
        from += count;
    }
}
window.addEventListener('scroll', onScroll)

function onScroll() {
    let nowScroll = window.scrollY
    let maxScroll = document.documentElement.scrollHeight - window.innerHeight
    if (maxScroll - nowScroll < 200) {
        //alert("scrolled")
        window.removeEventListener('scroll', onScroll)
        GetAdditionalProducts(from, count)
        from += count
    }
}
function GetAdditionalProducts(from, count) {
    var formdata = new FormData()
    formdata.append('from', from)
    formdata.append('count', count)

    $.ajax({
        url: "/Home/GetAdditionalProducts",
        data: formdata,
        cache: false,
        contentType: false,
        processData: false,
        type: "POST",
        success: function (response) {
            if (response.length < 200) {
                return;
            }
            firstLoads();
            $(window).resize(function () {
                firstLoads();
                $(window).resize(function () { });
            });
            $(".ads__content").html($(".ads__content").html() + response)
            window.addEventListener('scroll', onScroll)
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    })
}
