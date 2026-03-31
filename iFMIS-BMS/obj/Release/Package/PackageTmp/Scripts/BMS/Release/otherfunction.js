

function commaSeparateNumberRealignment(val) { //convert from number to string

    val = val.toString().replace(/,/g, ''); //remove existing commas first

    var valSplit = val.split('.'); //then separate decimals
    while (/(\d+)(\d{3})/.test(valSplit[0].toString())) {
        valSplit[0] = valSplit[0].toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }

    if (valSplit.length == 2) { //if there were decimals
        val = valSplit[0] + "." + valSplit[1]; //add decimals back
    } else {
        val = valSplit[0];
    }
    return val;
}
function covert_empty(str) { //convert from string to number
    str = str.toString().replace(/,/g, "");
    parseInt(str, 20);
    return str;
}
function commaSeparateNumberversion2(val) {

    var ReturnValue = $('#' + val + '').val();

    ReturnValue = ReturnValue.toString().replace(/,/g, ''); //remove existing commas first
    // alert(ReturnValue);
    var valSplit = ReturnValue.split('.'); //then separate decimals

    while (/(\d+)(\d{3})/.test(valSplit[0].toString())) {
        valSplit[0] = valSplit[0].toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }

    if (valSplit.length == 2) { //if there were decimals
        ReturnValue = valSplit[0] + "." + valSplit[1]; //add decimals back
    } else {
        ReturnValue = valSplit[0];
    }

    $('#' + val + '').val(ReturnValue);
    sync()
}

function commaSeparateNumberversion_realignapproval(val) {


    var ReturnValue = $('#' + val + '').val();



    ReturnValue = ReturnValue.toString().replace(/,/g, ''); //remove existing commas first
    // alert(ReturnValue);
    var valSplit = ReturnValue.split('.'); //then separate decimals

    while (/(\d+)(\d{3})/.test(valSplit[0].toString())) {
        valSplit[0] = valSplit[0].toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }

    if (valSplit.length == 2) { //if there were decimals
        ReturnValue = valSplit[0] + "." + valSplit[1]; //add decimals back
    } else {
        ReturnValue = valSplit[0];
    }

    $('#' + val + '').val(ReturnValue);
   // sync()
    sync2()
}

function commaSeparateNumberOnly(val) {


    var ReturnValue = $('#' + val + '').val();



    ReturnValue = ReturnValue.toString().replace(/,/g, ''); //remove existing commas first
    // alert(ReturnValue);
    var valSplit = ReturnValue.split('.'); //then separate decimals

    while (/(\d+)(\d{3})/.test(valSplit[0].toString())) {
        valSplit[0] = valSplit[0].toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }

    if (valSplit.length == 2) { //if there were decimals
        ReturnValue = valSplit[0] + "." + valSplit[1]; //add decimals back
    } else {
        ReturnValue = valSplit[0];
    }

    $('#' + val + '').val(ReturnValue);
}