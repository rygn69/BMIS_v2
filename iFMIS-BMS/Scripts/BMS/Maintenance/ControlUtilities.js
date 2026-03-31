function searchAccount() {

    var Data = $("#SearchData").val();
    $("#grNonOfficeFunctionCode").data("kendoGrid").dataSource.filter([
        {
            field: "grNonOfficeAcctName",
            operator: "contains",
            value: Data
        }
    ]);
}

$(function () {
    $("#SearchData").keypress(function (e) {
        if (e.keyCode == 13) {
            searchAccount();
        }
    });
});

function GridParam() {
    var TransactionYear = $("#Years").val();
    return {
        Year: TransactionYear
    }
}

function RefreshGrid() {
    $("#grNonOfficeFunctionCode").data("kendoGrid").dataSource.read();
}

function NonOfficeControl() {
    var url = NonOfficeControlURL();
    var TransactionYear = $("#Years").val();
    if (TransactionYear == '') {
        swal("Please select your Transaction Year", "", "warning");
    } else {
        $.get(url, { TransactionYear: TransactionYear }, function (e) {
            $(window).scrollTop(0);
            $("#NonOfficeCode").html(e);
            $("#NonOfficeCode").data('kendoWindow').title("<i class='fa fa-navicon'> </i> Add Non-Office Function Code").center().open();
        });
    }
}

function SetPrograms() {
    $("#Program").data("kendoComboBox").value("");
    $("#Program").data("kendoComboBox").dataSource.read();
}
function ProgramParam() {
    var OfficeID = $("#Office").val();
    var TransactionYear = $("#Years").val();
    return {
        OfficeID: OfficeID,
        TransactionYear: TransactionYear
    }
}

function AccountParam() {
    var ProgramID = $("#Program").val();
    var TransactionYear = $("#Years").val();
    return {
        ProgramID: ProgramID,
        TransactionYear: TransactionYear
    }
}

function SetAccount() {
    $("#Account").data("kendoComboBox").value("");
    $("#Account").data("kendoComboBox").dataSource.read();
    $("#FunctionCodeNum").val("");
}
function SetMainPPA() {
    var AccountID = $("#Account").val();
    console.log(AccountID);
    if(AccountID == 2861){
        $("#MainPPA").data("kendoComboBox").enable(true);
        $("#PPA").data("kendoComboBox").enable(true);
        $("#MainPPA").data("kendoComboBox").dataSource.read();
        $("#FunctionCodeNum").val("");
    }else 
    {
        $("#MainPPA").data("kendoComboBox").enable(false);
        $("#PPA").data("kendoComboBox").enable(false);
        $("#MainPPA").data("kendoComboBox").value("");
        $("#PPA").data("kendoComboBox").value("");
        $("#FunctionCodeNum").val("");
        SetFunctionCode();
    }
}

function MainPPAParam() {
    var TransactionYear = $("#Years").val();
    return {
        TransactionYear: TransactionYear
    }
}
function PPAsParam() {
    var MainPPA = $("#MainPPA").val();
    var TransactionYear = $("#Years").val();
    return {
        TransactionYear: TransactionYear,
        AccountID: MainPPA
    }
}
function SetPPA() {
    $("#PPA").data("kendoComboBox").value("");
    $("#PPA").data("kendoComboBox").dataSource.read();
    $("#FunctionCodeNum").val("");
}
function SetFunctionCode() {
    var MainPPA = $("#MainPPA").val();
    var TransactionYear = $("#Years").val();
    var PPA = $("#PPA").val();
    var Program = $("#Program").val();
    var Account = $("#Account").val();
    var url = FunctionCodeURL();
    $.get(url, { MainPPA: MainPPA, TransactionYear: TransactionYear, PPA: PPA, Program: Program, Account: Account}, function (e) {
        if (e == 0) {
            $("#FunctionCodeNum").val(e);
            $("#FunctionCodeNum").attr("readOnly", false);
            $("#SaveNonOfficeCode").attr("class", "k-button");
            $("#SaveNonOfficeCode").attr("onclick", "SaveNonOfficeCode()");
            
        } else {
            $("#FunctionCodeNum").val(e);
            $("#FunctionCodeNum").attr("readOnly", true);
            $('#SaveNonOfficeCode').prop('onclick', null).off('click');
            $("#SaveNonOfficeCode").attr('class', 'k-button k-state-disabled');
            
        }
        
    });

}

function SaveNonOfficeCode() {
    var MainPPA = $("#MainPPA").val();
    var TransactionYear = $("#Years").val();
    var PPA = $("#PPA").val();
    var OfficeID = $("#Office").val();
    var Program = $("#Program").val();
    var Account = $("#Account").val();
    var FunctionCodeNum = $("#FunctionCodeNum").val();
    var url = SaveNonOfficeCodeURL();
    $.get(url, { MainPPA: MainPPA, TransactionYear: TransactionYear, PPA: PPA, Program: Program, Account: Account, FunctionCodeNum: FunctionCodeNum, OfficeID: OfficeID }, function (e) {
        if (e == "success") {
            swal("Success", "Function Code has been Save!", "success");
            $("#FunctionCodeNum").attr("readOnly", true);
        } else {
            swal("Error", "Something went wrong!", "error");
        }
    });
}

