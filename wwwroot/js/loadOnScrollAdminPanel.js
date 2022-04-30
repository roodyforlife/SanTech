
var adminPanelProductsFrom = 20;
var adminPanelProdutsCount = 10;
$(window).resize(function () {
    firstLoads();
    $(window).resize(function () { });
});
$(document).ready(function () {
    firstLoads();
});
function firstLoads() {
    let blockHeight = document.querySelector('.adminPanel__products').offsetHeight;
    let windowHeight = window.innerHeight;
    if (blockHeight < windowHeight) {
        GetAdditionalProducts(adminPanelProductsFrom, adminPanelProdutsCount);
        adminPanelProductsFrom += adminPanelProdutsCount;
    }
}
window.addEventListener('scroll', onScroll)

function onScroll() {
    if ($('#Products').hasClass('_active__adminPanel')) {
        let nowScroll = window.scrollY
        let maxScroll = document.documentElement.scrollHeight - window.innerHeight
        if (maxScroll - nowScroll < 200) {
            console.log("scrolled")
            window.removeEventListener('scroll', onScroll)
            GetAdditionalProducts(adminPanelProductsFrom, adminPanelProdutsCount)
            adminPanelProductsFrom += adminPanelProdutsCount
        }
    }
}
function GetAdditionalProducts(adminPanelProductsFrom, adminPanelProdutsCount) {
    var formdata = new FormData()
    formdata.append('from', adminPanelProductsFrom)
    formdata.append('count', adminPanelProdutsCount)

    $.ajax({
        url: "/AdminPanel/GetAdditionalProducts",
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
            $(".adminPanel__products").html($(".adminPanel__products").html() + response)
            window.addEventListener('scroll', onScroll)
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    })
}
