$(function() {
    var info = $('.file-info-box')
    var input = $('#files_in')
    input.change(function() {
        info.empty()
        var files = input[0].files;
        if (files.length > 1) {
            for (var i = 0; i < files.length; i++) {
                var el = $('<span/>').text(files[i].name)
                el.addClass('file-info')
                info.append(el)
            }
        }        
    });
}) 