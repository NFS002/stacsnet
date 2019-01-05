$(function(){
    const container = $('#filebox'),
    files = container.find('a.filename').toArray()

    function display(search_key, order) {
        

        var new_files = files.filter(e => $(e).attr('data-name').toUpperCase().includes(search_key.toUpperCase())),
        l = new_files.length,
        txt = l + ' matching files found',
        el = $('<p>')
        br = $('<br>')
        el.text(txt)

        if (order === 'dsc') new_files.sort((file1, file2) =>{
            f1 = $(file1)
            f2 = $(file2)
            var d1_year = f1.attr('data-year'),
            d1_month = f1.attr('data-month'),
            d1_day = f1.attr('data-day'),
            d1_hour = f1.attr('data-hour'),
            d1_minute = f1.attr('data-minute'),
            d1_second = f1.attr('data-second'),
            d2_year = f2.attr('data-year'),
            d2_month = f2.attr('data-month'),
            d2_day = f2.attr('data-day'),
            d2_hour = f2.attr('data-hour'),
            d2_minute = f2.attr('data-minute'),
            d2_second = f2.attr('data-second')
            d1 = new Date(d1_year, d1_month, d1_day, d1_hour, d1_minute, d1_second)
            d2 = new Date(d2_year, d2_month, d2_day, d2_hour, d2_minute, d2_second)
            return d1 - d2
        })
        container.append(el)
        container.append(br)
        new_files.forEach(e => container.append(e))
    }

    $('#filesearch_btn').click(function() {
        var search_key = $('#filesearch_in').val()
        var order = $("input[name='date_in']:checked").val();
        container.empty()
        display(search_key, order)
    })
})