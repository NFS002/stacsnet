$(function(){

    var formfields = {
        select_module: $('#input-module'),
        select_type: $('#input-gradetype'),
        in_grade:  $('#input-grade'),
        in_week: $('#input-week'),
        in_date: $("form input[name=Year]" ),
        error_box: $('div[data-valmsg-summary] > ul'),
        applydefaults: function() {
            this.select_module.val(module)
            this.select_type.prop('selected-index',0)
            this.in_grade.val(7)
            this.in_week.val(1)
            this.in_date.val(year)
        }
    },

    modal = $( "#modalform" ).dialog({
        autoOpen: false,
        width: 600,
        height: 520,
        resizable: false,
        modal: true,
        open: () => {
            formfields.applydefaults()
            formfields.error_box.empty()
        }   
    })



    $('form').submit(function(e) {
        if (formfields.select_type.find('option:selected').text() === 'Practical' 
            && (!formfields.in_week.val() || (formfields.in_week.val() < 0 && formfields.in_week.val() > 13))) {
            var li = $('<li></li>')
            li.text("Enter a value for 'Week' between 1 and 13")
            formfields.error_box.append(li)
            e.preventDefault()
            return false
        }
    })

    $('#btn-modal').click(function() {
        modal.dialog("open")
    })
})
