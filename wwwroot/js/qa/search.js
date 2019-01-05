$(function(){

    var displaycount = (i,j) => {
        if (i === 0) 
            $('#commentcount').text('No ' + comment_text)
        else
            $('#commentcount').text('Showing ' + i + ' of ' + j + ' ' + comment_text)
    }

    var datesort = (a,b) => {
        var texta = $(a).find('.comment-date').text(),
        textb = $(b).find('.comment-date').text(),
        datea = new Date(texta),
        dateb = new Date(textb)
        return datea - dateb
    }


    var redisplay = (arr) => {
        $(arr).each( (i,e) => {
            content.append(e)
        });
    }

    function array_contains(arr1, arr2){
        for(var i = 0; i < arr1.length; i++){
          if(arr2.indexOf(arr1[i]) === -1)
             return false;
        }
        return true;
    }

    content = $('#allcomments')
    allcomments = content.find('.comment').toArray()
    displaycount(allcomments.length, allcomments.length)

    $('#searchbtn').click(function(){
        var order = $('input[name=Date]:checked').val(),
            tags = $('#search_tags_in').val()
            text = $('#search_tt_in').val(),
            commencountbox = $('.commentcount-box'),
            uname = $('#search_uname_in').val(),
            filteredcomments = allcomments
            content.empty()
            allcomments.sort(datesort)
            

            if (order === "ascending") 
                allcomments.reverse()

                        
            if (tags && tags.length) {
                tags = tags.toLowerCase().split(',')
                filteredcomments = allcomments.filter( c => array_contains(tags, $(c).attr('data-tags').split(',')))
            }
            

            if (filteredcomments && filteredcomments.length && text && text.length) 
                filteredcomments = filteredcomments.filter(c => { return ($(c).attr('data-title') + $(c).find('.comment-text').text()).includes(text) })

            if (filteredcomments && filteredcomments.length && uname && uname.length) 
                filteredcomments = filteredcomments.filter(c => $(c).find('.comment-uname').text() === uname)
            

            redisplay(filteredcomments)
            displaycount(filteredcomments.length, allcomments.length)
    })
})