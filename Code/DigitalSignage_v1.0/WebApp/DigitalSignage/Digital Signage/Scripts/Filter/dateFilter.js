
dsFilters.filter('dateFilter', function () {
    console.log('Inside filter')
    return format = function (time, format) {
        console.log(time,format)
        var regExp = /\(([^)]+)\)/; //getting the milliseconds numbers using regex
        var matches = regExp.exec(time);
        //matches[1] contains the value between the parentheses
        console.log(matches[1]);
        var milliseconds = parseInt(matches[1]);
        var t = new Date(milliseconds);
        var tf = function (i) { return (i < 10 ? '0' : '') + i };
        return format.replace(/yyyy|MM|dd|HH|mm|ss/g, function (a) {
            switch (a) {
                case 'yyyy':
                    return tf(t.getFullYear());
                    break;
                case 'MM':
                    return tf(t.getMonth() + 1);
                    break;
                case 'mm':
                    return tf(t.getMinutes());
                    break;
                case 'dd':
                    return tf(t.getDate());
                    break;
                case 'HH':
                    return tf(t.getHours());
                    break;
                case 'ss':
                    return tf(t.getSeconds());
                    break;
            }
        })
        console.log(t, tf);
    }     
});