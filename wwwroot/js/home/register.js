$(() => {
    var alert = $('.alert')
    alert.effect('highlight', {}, 3000);
    alert.parent().css('min-height', alert.height() + 10)
})