$(() => {
    var alert = $('.flash')
    var close = $('.btn-flash')
    close.click(() => {
        alert.fadeTo(500, 0)
    })
})
