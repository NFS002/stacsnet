/* var histogram_practical_cols = { cols : [
    { label: 'Type', type: 'string' },
    { label : 'Grade', type: 'number' },
    { type: 'string', role: 'style'}]},

histogram_cols = { cols : [
    { label: 'Type', type: 'string' },
    { label : 'Grade', type: 'number' }]},

getColor = (week) => {
    var color = '#717'
    switch(week) {
        case(0): color = '#101'; break
        case(1): color = '#211'; break
        case(2): color = '#265'; break
        case(3): color = '#363'; break
        case(4): color = '#413'; break
        case(5): color = '#252'; break
        case(6): color = '#501'; break
        case(7): color = '#135'; break
        case(8): color = '#814'; break
        case(9): color = '#184'; break
        case(10): color = '#109'; break
        case(11): color = '#999'; break
        case(12): color = '#911'; break;
        case(13): color = '#919'; break;
        default: color = '#777';
    }
    return color;
}, */


var fillData = () => {
    Object.keys(data).forEach(function(key) { 
        if (!data[key]['reports'].length) {
            data[key]['reports'].push(['_', '_'])
        }
    })
}