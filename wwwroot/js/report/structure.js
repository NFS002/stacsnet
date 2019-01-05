$(() => {
var tabs = $('<div></div'),
    tablist = $('<ul></ul>'),
    histogram_options = {
        legend: { position: 'none' },
        height: '100%',
        width: '100%',
        colors: ['#e7711c'],
        hAxis: {
            type: 'category',
            title: 'Grade'
        },
        vAxis: {
            title: 'Frequency'
        }
    }
    $('#content').append(tabs)
    tabs.append(tablist)
    tabs.attr('id','tabs')
    tablist.addClass('tab-list')

    var range = (start, end) => {
        return Array(end - start + 1).fill().map((item, index) => start + index);
    };


    var writeHistogramChartInfo = (data, _data, info) => {
        var info_title = $('<div></div>')
        info_content = info_title.clone(),
        count = +data['count'],
        min = +data['min'],
        info_title.addClass('chart-info-title')
        info_content.addClass('chart-info-content')
        info.append(info_title) 
        info.append(info_content)
        if (count < min) {
            var plural = 's'
            if (count === 1)
                plural = ''
            info.addClass('nodata')
            info_title.html(
                count 
                + ' submission' + plural + ' received.'
                + '<br>'
                + 'To preserve anonymity, no data will be displayed until '
                + min
                + ' submissions' )
        }
        else if (count === 1)
            info_title.text('Displaying 1 submssion');
        else {
            var n = _data.getNumberOfRows(),
            mean, variance, sqdiff = 0,
            datarange = _data.getColumnRange(1),
            mean = (datarange.max + datarange.min) / 2
            range(1, n - 1).forEach(i => {
                sqdiff += Math.pow(_data.getValue(i, 1) - mean, 2)
            })
            var variance = sqdiff / n;
            info_title.text('Displaying ' + n + ' submssions');
            info_content.html(
                'Mean grade: ' + mean
                + '<br>'
                + "Highest grade: " + datarange.max 
                + '&emsp;' 
                + "Lowest grade: " + datarange.min 
                + '<br>'
                + "Variance: " +  variance.toFixed(1)
                + '&emsp;'
                + "σ: " + Math.sqrt(variance).toFixed(1))
        }
    }

    types.forEach(function(type){
        /* Create tabs */
        var tab = $('<div></div>'),
        canvas = $('<div></div'),
        loading_div = $('<div></div'),
        info = $('<div></div>'),
        tab_link = $('<a></a>'),
        tab_id = 'tab-' + type
        list_item = $('<li></li>')
        tab_link.text(type)
        tablist.append(list_item)
        canvas.addClass('canvas')
        info.addClass('chart-info')
        loading_div.addClass('loader')
        list_item.append(tab_link);
        tab.attr('id',tab_id)
        tab_link.attr('href','#' + tab_id)
        tab.addClass('tab')
        tab.append(canvas)
        tab.append(info)
        tabs.append(tab)
        canvas.append(loading_div)
    })

    function drawChart (type, data, canvas, info) {
        histogram_options.title = type + ' grades for ' + module + ' in ' + year
        var _data = google.visualization.arrayToDataTable(data['reports'], true),
        chart = new google.visualization.Histogram(canvas)
        chart.draw(_data, histogram_options)
        writeHistogramChartInfo(data, _data, info)
    }

    function loadTab(event, ui) {
        var tab = ui.newPanel,
        type = tab.attr('id').split('-')[1]
        if (tab.find('svg').length)
            return
        canvas = tab[0].firstChild
        loading_div = canvas.firstChild
        info = canvas.nextSibling
        $.when( drawChart(type, data[type], canvas, $(info) ))
            .done( () => loading_div.remove()) 
    }

    google.charts.load('current', { packages: ['corechart']});

    google.charts.setOnLoadCallback(function(){
        fillData()
        tabs.tabs({
            activate: loadTab
        })
    })
    tabs.tabs({ activate: loadTab })
    setTimeout(() => {
        $("a[href^='#tab']")[1].click()
        $("a[href^='#tab']")[0].click()    
    }, 1000);
})