$(function(){

    function param(name) {
        return (location.search.split(name + '=')[1] || '').split('&')[0];
    }


    var textarea = $( "#textarea" ),
        titlein = $('#titlein'),
        submitbtn = $( "#submitbtn" ),
        tagsin = $( '#tagsin'),
        replybtn = $( ".reply-btn" ),
        open = param('open')
        dialog = $( "#commentform" ).dialog({
            autoOpen: false,
            width: 600,
            height:500,
            resizable: false,
            modal: true
          });

    replybtn.click(() => dialog.dialog( "open" ))


    tagsin.on( 'input selectionchange propertychange', function(e) {
        if (realtags.length > 19) {
            $('#tagserr').text('Limit of 20 tags exceeded')    
        }
        else {
            var val = $(this).val()
            if (val.endsWith(',')) {
                var regex = /[`~!@#$%^&*()_|+=?;:'",.<>\{\}\[\]\\\/]/gi,
                tag = val.slice(0, -1).trim().replace(regex, '')
                if (tag && tag.length) {
                    if (realtags.includes(tag))
                        $('#tagserr').text('Duplicate tags detected') 
                    
                    else {
                        $('#tagserr').empty()
                        var button = $('<div>'),
                        icon = $(" <i class='fa fa-lg fa-hashtag' style='font-size:18px'></i> ")
                        closeicon = $(" <i class='fa fa-times fa-xs' style='padding-left:5px;color:#911'></i> ")
                        button.text(tag)
                        button.addClass('btn-tag')
                        $('#tags-display-box').append(button)
                        button.prepend(icon)
                        button.append(closeicon)
                        $(this).val('')
                        realtags.push(tag)
                        closeicon.on('click', e => {
                            var txt = button.text()
                            button.remove()
                            realtags = realtags.filter(e => e !== txt);
                            if (realtags.length < 20) {
                                $('#tagserr').empty()
                            }
                        })
                    }
                }
            }
        }
    })

    textarea.on( 'input selectionchange propertychange', function() {
        if ( textarea.val().trim().length && titlein.val() && titlein.val().trim().length ) {
            submitbtn.removeClass('disabled')
        }
        else {
            submitbtn.addClass('disabled')
        }
    });

    titlein.on( 'input selectionchange propertychange', function() {
        if ( titlein.val().trim().length && textarea.val() && textarea.val().trim().length )
            submitbtn.removeClass('disabled')
        else 
            submitbtn.addClass('disabled')
    });

    if (open && open.length)
        replybtn.click()

});