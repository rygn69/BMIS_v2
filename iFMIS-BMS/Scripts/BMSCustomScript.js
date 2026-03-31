
function MaximizeDIV(DivName) {
    $('#' + DivName + '').css("position", "fixed");
    $('#' + DivName + '').css("top", "0");
    $('#' + DivName + '').css("bottom", "0");
    $('#' + DivName + '').css("right", "0");
    $('#' + DivName + '').css("left", "0");
    $('#' + DivName + '').css("background", "#FFF");
    $('#' + DivName + '').css("z-index", "10000");
    $('#' + DivName + '').css("padding", "10px 10px 10px 10px");
    $("html").css("overflow-y", "hidden");
    $('#' + DivName + '').css("overflow-y", "auto");
}
function MinimizeDIV(DivName) {
    $('#' + DivName + '').css("position", "");
    $('#' + DivName + '').css("top", "");
    $('#' + DivName + '').css("bottom", "");
    $('#' + DivName + '').css("right", "");
    $('#' + DivName + '').css("left", "");
    $('#' + DivName + '').css("background", "");
    $('#' + DivName + '').css("z-index", "");
    $('#' + DivName + '').css("padding", "");
    $("html").css("overflow-y", "auto");
    $('#' + DivName + '').css("overflow-y", "hidden");
}
function CloseKendoWindow(WindowName) {
    document.documentElement.style.overflow = 'auto';  // firefox, chrome
    document.body.scroll = "yes";
    $('#' + WindowName + '').data("kendoWindow").close();
}
function formatCurrency(total) {
    var neg = false;
    if (total != "") {
        if (total < 0) {
            neg = true;
            total = Math.abs(total);
        }
        return (neg ? "-" : '') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    }
}

function OnKeyUpConvertToCurrency(event, TexboxName) {
    var total = $('#' + TexboxName + '').val();
    str = total.replace(/,/g, "");
    parseInt(str, 20);
    var textboxvalue = (formatCurrency(str));
    var cursorpos = (textboxvalue.length) - $('#' + TexboxName + '').prop("selectionStart")
    if (cursorpos >= 3) {
        cursorpos = 3
    }
    else {
        cursorpos
    }
    if (event.keyCode == 190) {
        setCaretToPos($('#' + TexboxName + '')[0], (textboxvalue.length) - 2);
    }
    else {
        if ($.inArray(event.keyCode, [37, 38, 39, 40, 46, 9, 27, 13, 110, 65, 35, 36, 16, 17, 18, 20]) === -1) {
            $('#' + TexboxName + '').val(textboxvalue);
            setCaretToPos($('#' + TexboxName + '')[0], (textboxvalue.length) - cursorpos);
        }
    }
}

function FilterNumericOnly(event) {
    if ($.inArray(event.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
        // Allow: Ctrl+A, Command+A
       (event.keyCode == 65 && (event.ctrlKey === true || event.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
       (event.keyCode >= 35 && event.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((event.shiftKey || (event.keyCode < 48 || event.keyCode > 57)) && (event.keyCode < 96 || event.keyCode > 105)) {
        event.preventDefault();
    }
}
function setSelectionRange(input, selectionStart, selectionEnd) {
    if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    } else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}
function setCaretToPos(input, pos) {
    setSelectionRange(input, pos, pos);
}

//function ConvertCommasToEmpty(str) {
//    str = str.replace(/,/g, "");
//    parseInt(str, 20);
//    return str;
//}
function DisableOfficeDropDown(UserTypeDesc,OfficeID,DropDownName,Kendo_Type) {
    if (UserTypeDesc == 'Budget In-Charge') {
        $('#' + DropDownName + '').data(Kendo_Type).value(OfficeID);
        $('#' + DropDownName + '').data(Kendo_Type).enable(false);
    }
    else {
        $('#' + DropDownName + '').data(Kendo_Type).enable();
        $('#' + DropDownName + '').data(Kendo_Type).value(OfficeID);
    }
}
function UpdateDocumentTitle(Title) {
    document.getElementById("TabTitle").innerHTML = Title;
}