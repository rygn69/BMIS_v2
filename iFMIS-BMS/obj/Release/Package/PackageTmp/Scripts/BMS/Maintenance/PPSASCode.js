function ChangeTransactionYear() {
    $("#grPPSASCode").data("kendoGrid").dataSource.read();
}

function YearParam() {
    var Year = $("#Years").val();
    return {
        Year: Year
    }
}

function BuildUpPPSASCode() {
    var url = BuildUpPPSASCodeURL();
    var TransactionYear = $("#Years").val();
    if (TransactionYear == '') {
        swal("Please select your Transaction Year", "", "warning");
    } else {
        $.get(url, { TransactionYear: TransactionYear }, function (e) {
            $(window).scrollTop(0);
            $("#PPSASCode").html(e);
            $("#PPSASCode").data('kendoWindow').title("<i class='fa fa-navicon'> </i> Build-up PPSAS Code").center().open();
        });
    }
}

function onGridEdit(arg) {

    if (arg.container.find("input[name=PPSASCode]").length <= 0) {
        $("#grPPSASCode").data("kendoGrid").closeCell(arg.container)
    }

        var gridName = $('#grPPSASCode').data('kendoGrid');
        var ds = gridName.dataSource.data();
        var dataRows = gridName.items();
        var AccountCode = "";
        var Year = $("#Years").val();

        var input = arg.container.find("input[name=PPSASCode]");
        var value = input.val();
        var OldValue = input.val();

        input.keyup(function () {
            value = input.val();
        });
        input.blur(function () {
            var rowIndex = dataRows.index(gridName.select());
            AccountCode = ds[rowIndex].AccountCode;
            if (value != OldValue) {
                var url = UpdateDataURL();
                $.post(url, { AccountCode: AccountCode, PPSASCode: value, TransactionYear: Year }, function (d) {
                    if (d == "1") {
                        $("#grPPSASCode").data("kendoGrid").dataSource.read();
                        //var grid = $("#grPPSASCode").data("kendoGrid")
                        //var ds = grid.dataSource.view();
                        //for (var i = 0; i < ds.length; i++) {
                        //    var row = grid.table.find("tr[data-uid='" + ds[i].PPSASCode + "']");
                        //    var checkbox = $(row).find(".checkbox");
                        //    //if (AccountID == ds[i].PPSASCode) {
                        //        var SelectedAccount = $('#grPPSASCode').data().kendoGrid.dataSource.data()[i];
                        //        SelectedAccount.set('PPSASCode', d);
                        //        break;
                            //}
                        //}
                    }
                    else {
                        swal({
                            title: "Error!",
                            text: d,
                            type: "error",
                            showConfirmButton: true
                        });
                    }
                })
            }
        });
}

function sync() {
    var PPSASAccount = $("#Accounts").val();
    var PPSASSeries = $("#PPSASSeries").val();
    var str  = "";
    if (PPSASSeries.length == 0) {
        str = PPSASAccount;        
    } else {
        str = PPSASAccount + "-" + PPSASSeries;
    }
    
    $("#ChildAccountCode").val(str);
}

function SetPPSASCode() {
    var PPSASAccount = $("#Accounts").val();
    $("#AccountCode").val(PPSASAccount);
    sync();
    //var Year = $("#Years").val();
    //var url = SetPPSASCodeURL();

    //$.get(url, { PPSASAccount: PPSASAccount, Year: Year }, function (e) {
    //    $("#AccountCode").val(e);
    //    $("#ChildAccountCode").val(e);
    //});
}

function SavePPSASCode() {
    var PPSASCode = $("#Accounts").val();
    var PPSASSeries = $("#PPSASSeries").val();
    var FMISAccount = $("#FMISAccount").val();
    var ChildPPSASCode = $("#ChildAccountCode").val();
    var YearOf = $("#Years").val();
    var url = SavePPSASCodeURL();
    $.get(url, { PPSASSeries: PPSASSeries, PPSASCode: PPSASCode, ChildPPSASCode: ChildPPSASCode, FMISAccount: FMISAccount, YearOf: YearOf }, function (e) {
        if (e == '1') {
            swal("PPSASCode Successfully saved!", "", "success")
            $("#grPPSASCode").data("kendoGrid").dataSource.read();
        }

    });
}

function checkIfIxist() {
    var _PPSASCode = $("#_PPSASCode").val();
    var _PPASeriesCodee = $("#_PPASeriesCodee").val();
    $("#Accounts").data("kendoComboBox").value(_PPSASCode);
    $("#PPSASSeries").val(_PPASeriesCodee);
    SetPPSASCode();
}