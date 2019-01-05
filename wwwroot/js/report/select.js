$(function(){
    select_module = $('#select-module'),
    select_year = $('#select-year'),
    btn_select = $('.header-box a')
    $('.alert').fadeOut(3500)
    modules.forEach(m => {
        var option = $('<option></option>')
        option.attr('value', m)
        option.text(m)
        select_module.append(option)
    })

    years.forEach(y => {
        var option = $('<option></option>')
        option.attr('value', y)
        option.text(y)
        select_year.append(option)
    })

    select_module.find("option[value="  + "'" + module + "'" + "]").attr('selected', true);
    select_year.find("option[value=" + "'" + year + "'" + "]").attr('selected', true);

    btn_select.click(e => {
        var modulechoice = select_module.val();
        var yearchoice = select_year.val();
        btn_select.attr('href','/Report/' + modulechoice + '/' + yearchoice)
        return true;
    })
})