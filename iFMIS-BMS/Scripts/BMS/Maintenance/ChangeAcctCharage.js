var AddedItem = [];

$("#OBRNo").change(function (e) {
    searchtrnno();
}).keypress(function (e) {
    if (e.keyCode == 13) {
        searchtrnno();
    }
});

$('input:radio[name="FundType"]').change(
function () {
    if (this.checked && this.value == 201) {
        $("#EconomicSubsidy").prop("disabled", true);
        $("#EconomicIncome").prop("disabled", true);
        $("#NewEconomicSubsidy").prop("disabled", true);
        $("#NewEconomicIncome").prop("disabled", true);


    } else if (this.checked && this.value == 101) {
        $("#EconomicSubsidy").prop("disabled", true);
        $("#EconomicIncome").prop("disabled", true);
        $("#NewEconomicSubsidy").prop("disabled", true);
        $("#NewEconomicIncome").prop("disabled", true);

    } else if (this.checked && this.value == 119) {
        $("#EconomicSubsidy").prop("disabled", false);
        $("#EconomicIncome").prop("disabled", false);
        $("#NewEconomicSubsidy").prop("disabled", false);
        $("#NewEconomicIncome").prop("disabled", false);

    }
})

function searchtrnno() {
    var FundType = $('input[name=FundType]:checked').val();
    var OBRNo = $("#OBRNo").val();
    var Refval = OBRNo.split("-");
    var Refval2 = Refval[0] == 127 || Refval[0] == 109 || Refval[0] == 128 || Refval[0] == 131 || Refval[0] == 129 || Refval[0] == 132 ? 101 : Refval[0];
    //alert(Refval2)
    //alert(FundType)
    if (Refval2 != FundType) {

        swal("FundType and OBR does not match.", "", "error")

    } else {

    searchOBRNo().done(function () {
        searchOBRNoDetails().done(function () {
            //setTimeout(function () {
            //    var OfficeID = $("#Office").val();
            //    console.log("OfficeID: "+OfficeID);
            //    if (OfficeID == 37 || OfficeID == 38 || OfficeID == 41) {
            //        var IsIncome = $("#_IsIncome").val();
            //        if (IsIncome == 0) {
            //            $("#EconomicSubsidy").prop("checked", true);
            //            $("#EconomicIncome").attr("readOnly", true);
            //            console.log("IsIncome: "+IsIncome);
            //        } else if (IsIncome == 1) {
            //            $("#EconomicIncome").prop("checked", true);
            //            $("#EconomicSubsidy").attr("readOnly", true);
            //            console.log("IsIncome: "+IsIncome);
            //        }
            //    }
            //}, 500);            
        });
    });
    }
}

function searchOBRNo() {
    var FundType = $('input[name=FundType]:checked').val();
    if (FundType == 0) {
        swal({
            title: 'Please select Transaction Type <span class="fa fa-smile-o"> </span>',
            text: '',
            type: 'error',
            html: true
         });
    } else {
        var dfrdsearchOBRNo = $.Deferred();
        var OBRNo = $("#OBRNo").val();
        var url = searchOBRNoURL();

        $.get(url, { OBRNo: OBRNo }, function (e) {
            if (e.trnno != 0) {
                $("#_trnno").val(e.trnno);
                $("#_IsIncome").val(e.IsIncome);
                $("#_OriginalAmount").val(e.Amount);
                $("#OriginalAmount").val(MoneyFormatWithDecimal(e.Amount));
                if (e.OfficeID == 37 || e.OfficeID == 38 || e.OfficeID == 41) {
                    var IsIncome = $("#_IsIncome").val();
                    if (IsIncome == 0) {
                        $("#EconomicSubsidy").prop("checked", true);
                        $("#EconomicIncome").prop("disabled", true);
                        console.log("IsIncome: " + IsIncome);
                    } else if (IsIncome == 1) {
                        $("#EconomicIncome").prop("checked", true);
                        $("#EconomicSubsidy").prop("disabled", true);
                        console.log("IsIncome: " + IsIncome);
                    }
                }
            } else {
                swal(e.MTitle, e.MBody, e.MType)
            }
            dfrdsearchOBRNo.resolve();
        });
        return dfrdsearchOBRNo.promise();
    }
}

function searchOBRNoDetails() {
    var dfrdsearchOBRDetails = $.Deferred();
    $("#Office").data("kendoComboBox").dataSource.read();
    $("#Program").data("kendoComboBox").dataSource.read();
    $("#ObjOfExpenditure").data("kendoComboBox").dataSource.read();
    dfrdsearchOBRDetails.resolve();
    return dfrdsearchOBRDetails.promise();
}

function getOffice() {
    var FundType = $('input[name=FundType]:checked').val();
    var Year = $("#Years").val();
    return {
        FundType: FundType,
        YearOF: Year
    }
}

function getProgram() {
    // UPDATED
    var OfficeID = $("#NewOffice").val();
    if (OfficeID == '' || OfficeID == null)
    {
        OfficeID = 0;
    }
    var FundType = $('input[name=FundType]:checked').val();
    var Year = $("#Years").val();
    return {
        FundType: FundType,
        OfficeID: OfficeID,
        YearOF: Year
    }
}
function getAccount() {
    // UPDATED
    var OfficeID = $("#NewOffice").val();
    if (OfficeID == '' || OfficeID == null) {
        OfficeID = 0;
    }
    var Program = $("#NewProgram").val();
    var FundType = $('input[name=FundType]:checked').val();
    var Year = $("#Years").val();
    return {
        FundType: FundType,
        ProgramID: Program,
        YearOF:Year
    }
}

function ParamFrom() {
    var trnno = $("#_trnno").val();
    var Year = $("#Years").val();
    var FundType = $('input[name=FundType]:checked').val();
    return {
        trnno: trnno,
        Year: Year,
        FundType: FundType
    }
}

function CheckIfEdit() {
    //var _Edit = $("#_Edit").val();
    //if (_Edit == 0) {
    //    swal("Please click Edit before proceeding.", "", "warning");
    //    $("#NewOffice").data("kendoComboBox").value("");
    //    $("#NewProgram").data("kendoComboBox").value("");
    //    //$("#NewAccounts").data("kendoComboBox").value("");
    //} else {
        var Office = $("#NewOffice").val();
        $("#_Office").val(Office);
        $("#NewProgram").data("kendoComboBox").value("");
        $("#NewProgram").data("kendoComboBox").dataSource.read();
        //$("#NewAccounts").data("kendoComboBox").value("");

    //}
}

function Edit() {
    $("#_Edit").val(1);
}

function SetProgramID() {
    var Program = $("#NewProgram").val();
    $("#_Program").val(Program);
    $("#NewAccounts").data("kendoComboBox").value("");
    $("#NewAccounts").data("kendoComboBox").dataSource.read();
}

function Update() {
    var OfficeID = $("#Office").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#ObjOfExpenditure").val();
    var trnno = $("#_trnno").val();
    var _Office = $("#_Office").val();
    var _Program = $("#_Program").val();
    var _Accounts = $("#NewAccounts").val();
    var Claim = $("#Claim").val();
    var IsIncome = $('input[name=NewSubsidyIncome]:checked').val();
    var url = UpdateURL();

    var AcctCharge = $("#NewAccounts").val();
    var data = $("#grTransactionCharge").data("kendoGrid").dataSource;
    if (AddedItem.indexOf(AcctCharge) == -1) {
        AddedItem.push(AcctCharge);

        $.get(url, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, trnno: trnno, _Office: _Office, _Program: _Program, _Accounts: _Accounts, IsIncome: IsIncome }, function (e) {

            $("#_finalOBR").val(e.NewOBR);
            var PartialAmount = $("#_PartialAmount").val();
            var Amount = 0;
            Amount = Number(PartialAmount) + Number(e.Amount);
            $("#_PartialAmount").val(Amount);
            $("#_TotalAmount").val(MoneyFormatWithDecimal(Amount));

            var AccountName = $("#NewAccounts").data("kendoComboBox").text();
            var ObjOfExpenditure = $("#ObjOfExpenditure").data("kendoComboBox").text();

            var grid = $('#grTransactionCharge').data('kendoGrid');
            grid.dataSource.add({
                AccountName: AccountName, AmountDummy: "₱" + MoneyFormatWithDecimal(e.Amount), TempID: e.TempID, _OldAcctCharge: ObjOfExpenditure
            });
            $("#SumAmount").text("₱" + MoneyFormatWithDecimal(Amount));

            var _OriginalAmount = $("#_OriginalAmount").val();
            console.log("Original Amount: " + Number(_OriginalAmount));
            console.log("Amount: " + Number(Amount));
            if (Number(_OriginalAmount) == Number(Amount)) {
                
                console.log("Equal");
                $("#Save").attr("class", "k-button");
                $("#Save").attr("onclick", "Save()");
            }
        });

    } else {
        swal("Account already exist on the Grid", "", "error");
    }

   

    // $("#OBRNoEntry").attr("readOnly", true);
}

function TransactionChargeParam() {
    var _trnno = $("#_trnno").val();
    var Year = $("#Years").val();
    return {
        trnno: _trnno,
        Year: Year
    }
}

function SetRemainingBalance() {
    CheckRemainingBalance().done(function (e) {
        sync();
    });
}

function CheckRemainingBalance() {
    var dfrdCheckRemainingBalance = $.Deferred();
    var _Office = $("#_Office").val();
    var _Program = $("#_Program").val();
    var _Accounts = $("#NewAccounts").val();
    var Year = $("#Years").val();
    var IsIncome = $('input[name=NewSubsidyIncome]:checked').val();
    var url = CheckRemainingBalanceURL();

    $.get(url, { OfficeID: _Office, ProgramID: _Program, Account: _Accounts, Year: Year, IsIncome: IsIncome }, function (e) {
        $("#AllotmentAvailable").val(MoneyFormatWithDecimal(e));
        $("#_AllotmentAvailable").val(e);
        dfrdCheckRemainingBalance.resolve();
    });

    return dfrdCheckRemainingBalance.promise();
}

function SetAmountCharge() {

    var _trnno = $("#_trnno").val();
    var AcctCharge = $("#ObjOfExpenditure").val();
    var url = SetAmountChargeURL();

    $.get(url, { _trnno: _trnno, AcctCharge: AcctCharge }, function (e) {
        $("#Claim").val(MoneyFormatWithDecimal(e));
        $("#_Claim").val(e);
    });

}

function sync() {
    var AllotmentAvailable = $("#_AllotmentAvailable").val();
    var Claim = $("#_Claim").val();
    var BalanceAllotment = 0;

    BalanceAllotment = Number(AllotmentAvailable) - Number(Claim);
    $("#BalanceAllotment").val(MoneyFormatWithDecimal(BalanceAllotment));
    
    if(BalanceAllotment < 0)
    {
        $("#BalanceAllotment").css({ "width": "270px", "background-color": "#ededed", "text-align": "right", "font-weight": "bold", "color": "#ff0000" });
        swal({
            title: "Warning",
            text: "Allotment not enough!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok!",
            closeOnConfirm: true
        },
                             function () {
                                 $("#AllotmentAvailable").val(
                                            function (index, value) {
                                                return 0;
                                            })
                                 $("#BalanceAllotment").val(
                                            function (index, value) {
                                                return 0;
                                            })
                                 
                                 $("#NewAccounts").data("kendoComboBox").value(function (index, value) {
                                     return "";
                                 });
                             });
    } else {
        $("#BalanceAllotment").css({ "width": "270px", "background-color": "#ededed", "text-align": "right", "font-weight": "bold", "color": "#00ff21" });
    }
}

function Save() {
    swal({
        title: "Please make sure all the details are correct.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Proceed",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    },
   function (isConfirm) {
       if (isConfirm) {
           var url = SaveURL();
           var OBRNo = $("#OBRNo").val();
           var Year = $("#Years").val();
           $.get(url, { OBRNo: OBRNo, Years: Year }, function (e) {
               //if (e.ReturnStatus == 1) {
               swal(e.MTitle, e.MBody, e.MType);
               //} else {

               //}
           });
       }
   });
}

function Delete(TempID) {

    swal({
        title: "Are you sure you want to Delete this Item ?.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Proceed",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    },
  function (isConfirm) {
      if (isConfirm) {
          var url = DeleteURL();
          $.get(url, { TempID: TempID }, function (e) {

              var dfrdGridClickSetGenInfo = $.Deferred();
              var entityGrid = $("#grTransactionCharge").data("kendoGrid");
              var selectedItem = entityGrid.dataItem(entityGrid.select());

              swal(e.MTitle, e.MBody, e.MType);
              $("#grTransactionCharge").data("kendoGrid").dataSource();
          });
      }
  });

   
}

function onClose() {
  
}

function Clear() {
    // Start Radio Button
    $("#SEFFund").prop("checked", false);
    $("#GeneralFund").prop("checked", false);
    $("#EcoFund").prop("checked", false);
    $("#EconomicSubsidy").prop("checked", false);
    $("#EconomicIncome").prop("checked", false);
    $("#NewEconomicSubsidy").prop("checked", false);
    $("#NewEconomicIncome").prop("checked", false);
    // End Radio Button

    $("#OBRNo").val("");
    $("#_trnno").val("");
    $("#_finalOBR").val("");
    $("#_TotalAmount").val("");
    $("#OriginalAmount").val("");
    $("#Office").data("kendoComboBox").value("");
    $("#Program").data("kendoComboBox").value("");
    $("#ObjOfExpenditure").data("kendoComboBox").value("");
    $("#OfficeID").val("");
    $("#NewOffice").data("kendoComboBox").value("");
    $("#NewProgram").data("kendoComboBox").value("");
    $("#NewAccounts").data("kendoComboBox").value("");
    $("#AllotmentAvailable").val("");
    $("#Claim").val("");
    $("#BalanceAllotment").val("");
    $("#_Edit").val("");
    $("#_Office").val("");
    $("#_Program").val("");
    $("#_Amount").val("");
    $("#_Claim").val("");
    $("#_AllotmentAvailable").val("");
    $("#_BalanceAllotment").val("");
    $("#_OriginalAmount").val("");
    $("#_PartialAmount").val("");
    $("#_IsIncome").val("");

    $("#grTransactionCharge").data("kendoGrid").dataSource();

}