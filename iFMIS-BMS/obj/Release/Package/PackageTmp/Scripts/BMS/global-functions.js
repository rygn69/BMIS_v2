/// OUTPUT : 10 -> 00010 (DIFFERS FROM THE GIVEN LENGTH)
function ZeroPrefixFormat(str, len) {
    str = str.toString();
    return str.length < len ? ZeroPrefixFormat("0" + str, len) : str;
    
}

/// RETURN the Current Date example. 11/29/2013 10:57:56 AM
function CurrentDate() {
    var url = CurrentDateUrl();
    var result;

    $.ajax({
        url: url,
        type: 'get',
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
}


function CurrentYear() {
    var url = CurrentYearUrl();
    var result;

    $.ajax({
        url: url,
        type: 'get',
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
}

function NextYear() {
    var url = NextYearUrl();
    var result;

    $.ajax({
        url: url,
        type: 'get',
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
}
/// RETURN raw number to money format example. 123456789.10 -> 123,456,789.10
function MoneyFormat(amount) {
    amount = amount.toString();
    return Number(amount).toLocaleString('en');
    
}

function MoneyFormatWithDecimal(n) {
    var num = n;
    return  parseFloat(num).toFixed(2).replace(/./g, function (c, i, a) {
        return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
    });
}

function addZeroes(num) {
    var value = Number(num);
    var res = num.split(".");
    if (num.indexOf('.') === -1) {
        value = value.toFixed(2);
        num = value.toString();
    } else if (res[1].length < 3) {
        value = value.toFixed(2);
        num = value.toString();
        num.toLocaleString('en');
    }
    return num
}

