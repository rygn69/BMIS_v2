function searchAccount() {
    // UPDATED
    var Data = $("#ExcessData").val();
    $("#ExcessAppropriation").data("kendoGrid").dataSource.filter([
        {
            field: "grAccount",
            operator: "contains",
            value: Data
        }
    ]);
}
function YearParam() {
    // UPDATED
    var Year = $("#Years").val();
    return {
        Year: Year
    }
}

function enableControl() {
    $("#ControlNo").prop('disabled', false);
    $("#controlnew").prop('disabled', false);
    $("#controlnew").focus();
}

$(function () {

    $("#ExcessData").keypress(function (e) {
        if (e.keyCode == 13) {
            searchAccount();
        }
    });
    $("#ControlNo").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchOBR()
        }
    });

   
});

function RefreshGrid() {
    // UPDATED
    $("#grPastObligated").data("kendoGrid").dataSource.read();
}

$('input:radio[name="FundType"]').change(
    function () {
        if (this.checked && this.value == 0) {

            $("#Office").data("kendoComboBox").value(43);
            setTimeout(function () {
 
                $("#Program").data("kendoComboBox").dataSource.read();
                $("#Program").data("kendoComboBox").value(55);

            }, 500);
           
            setTimeout(function () {

                $("#Account").data("kendoComboBox").dataSource.read();
                $("#Account").data("kendoComboBox").value(2861);
            }, 500);

            $("#PPAID").data("kendoComboBox").enable(true);
            $("#ExcessAccount").data("kendoComboBox").dataSource.read();
        
        } 
        //hide on 2/21/2018-xXx
        //else if (this.checked && this.value == 1) {

        //    $("#Office").data("kendoComboBox").value(43);
        //    setTimeout(function () {

        //        $("#Program").data("kendoComboBox").dataSource.read();
        //        $("#Program").data("kendoComboBox").value(53);
        //    }, 500);

        //    setTimeout(function () {

        //        $("#Account").data("kendoComboBox").dataSource.read();
        //        $("#Account").data("kendoComboBox").value(2628);
        //    }, 500);

        //    $("#PPAID").data("kendoComboBox").enable(true);
        //    $("#ExcessAccount").data("kendoComboBox").dataSource.read();
        //Update on 2/21/2018-xXx 
        //}
        else if ((this.checked && this.value == 1) || (this.checked && this.value == 2) || (this.checked && this.value == 3)) {
            $("#Office").data("kendoComboBox").value("");
            $("#Program").data("kendoComboBox").value("");
            $("#Account").data("kendoComboBox").value("");

            $("#Program").data("kendoComboBox").enable(true);
            $("#Account").data("kendoComboBox").enable(true);
            $("#PPAID").data("kendoComboBox").enable(true);
            $("#ExcessAccount").data("kendoComboBox").dataSource.read();
        } 
        //else if (this.checked && this.value == 3) {
        //    $("#Office").data("kendoComboBox").value("");
        //    $("#Program").data("kendoComboBox").value("");
        //    $("#Account").data("kendoComboBox").value("");

        //    $("#Program").data("kendoComboBox").enable(true);
        //    $("#Account").data("kendoComboBox").enable(true);
        //    $("#PPAID").data("kendoComboBox").enable(true);
        //    $("#ExcessAccount").data("kendoComboBox").dataSource.read();
        //}
    })

function SearchOBR() {
   
    var dfrdsetsearchOBR = $.Deferred();
    var ControlNum = $("#ControlNo").val();
    var YearOf = TransYear();
    var url = SearchOBRURL();

    var str = ControlNum.split("-");
    //alert(str[0].toUpperCase())
    if(str[0].toUpperCase() == "REF"){
        // UPDATED
        var RefUrl = SearchExcessAirMarkURL();
        $.get(RefUrl, { ControlNo: ControlNum }, function (e) {
        //    alert(3)
            $("#_TempIndicator").val(0);
            $("#_OBRSeries").val(ControlNum);
            console.log(e.FundFlag);
            if (e.FundFlag == 101 || e.FundFlag == 127) {
                console.log('P1');
                $("#GeneralFund").prop("checked", true);
            } else if (e.FundFlag == 201) {
                $("#SEFFund").prop("checked", true);
                console.log('P2');
            } else if (e.FundFlag == 118) {
                $("#20PercentFund").prop("checked", true);
                console.log('P3');
            } else if (e.FundFlag == 119) {
                $("#EcoFund").prop("checked", true);
                console.log('P4');
            }

            $("#Office").data("kendoComboBox").value(e.Office);

            if ($("#userOffficeid").val() == 1 || $("#userOffficeid").val() == 43 || $("#userOffficeid").val() == 19)
            {
                $("#Program").data("kendoComboBox").enable(false);
                $("#Account").data("kendoComboBox").enable(false);
            }
            else {
                $("#Program").data("kendoComboBox").enable(true);
                $("#Program").data("kendoComboBox").value(e.ProgramID);
                $("#Program").data("kendoComboBox").dataSource.read();

                $("#Account").data("kendoComboBox").enable(true);
                $("#Account").data("kendoComboBox").value(e.AccountID);
                $("#Account").data("kendoComboBox").dataSource.read();
            }
            $("#ExcessAccount").data("kendoComboBox").value(e.ExcessAppropriationID);

            $("#PPAID").data("kendoComboBox").enable(true);
            $("#PPAID").data("kendoComboBox").value(e.ppaid);
            $("#SubAccount").data("kendoComboBox").enable(true);
            $("#SubAccount").data("kendoComboBox").value(e.ppaid);
            $("#ExcessDescription").val(e.Description)
          //  if (e.ppaid != 0) {
          ////      GetSubAccountBalance()
          //  }
            //setAppropriation();
            //setObligation();
            ExcessComputation().done(function () {
                console.log("Done Computing in Excess");
                var x = $("#_ExcessObligation").val();
                var y = $("#_ExcessAllotment").val();

                var z = 0;
                var a = 0;

                z = Number(x) - Number(e.Amount);
                y = Number(y) + Number(e.Amount);

                $("#ExcessObligation").val((z));
                $("#_ExcessObligation").val(z);
                $("#ExcessAllotment").val(MoneyFormat(y));
                $("#_ExcessAllotment").val(e.Amount);
               

                //$("#ExcessAllotment").val(MoneyFormat(e.Amount));
                //$("#_ExcessAllotment").val(e.Amount);
                $("#ShowPOAmount").show();
                $("#POAmount").text(MoneyFormat(e.Amount));
                $("#OBRNo").val(e.OBRNo);
                $("#_OBRSeries").val(e.OBRSeries);

                $("#AmountExcess").val(e.Amount);
                $("#Claim").val(MoneyFormat(e.Amount));
                $("#_Claim").val(e.Amount);
                $("#_OrigClaim").val(e.Amount);
                $("#_OrigExcessObligation").val(z);
                
                sync();
            });
        });

    }
    else if (str[0] == "Com" || str[0] == "com") {
        var url = exceescomURL();
        var comctrl = ControlNum;
        var Office = $("#Office").val();
        var Year = $("#Years").val();
        $.get(url, { Office: Office, comctrl: comctrl, Year: Year }, function (e) {
            if (e.type == "success") {
                $("#Office").data("kendoComboBox").value(e.OfficeID);
                SetPrograms();
                $("#Program").data("kendoComboBox").dataSource.read();
                $("#Program").data("kendoComboBox").value(e.ProgramID);
                SetAccounts();
                $("#Account").data("kendoComboBox").dataSource.read();
                $("#Account").data("kendoComboBox").value(0);
                SetPPA();
                $("#ExcessAccount").data("kendoComboBox").dataSource.read();
                $("#ExcessAccount").data("kendoComboBox").value(e.AccountID);
                $("#ExcessDescription").val(e.particular)
                ExcessComputation().done(function () {

                    var x = $("#_ExcessObligation").val();
                    var y = $("#_ExcessAllotment").val();

                    var z = 0;
                    var a = 0;

                    z = Number(x) - Number(e.Amount);
                    y = Number(y) + Number(e.Amount);

                    $("#ExcessObligation").val(MoneyFormat(z));
                    $("#_ExcessObligation").val(z);
                    $("#ExcessAllotment").val(MoneyFormat(y));
                    $("#_ExcessAllotment").val(e.Amount);

                    $("#AmountExcess").val(e.Amount)
                    $("#Claim").val(MoneyFormat(e.Amount));
                    $("#_Claim").val(e.Amount);
                    $("#_OrigClaim").val(e.Amount);
                    sync();
                })
                $("#OBRNo").val(e.OBRNo);
            }
            else {
                swal("Error", e.message, e.type)
            }
        })
    }
    else if (str[0] == "20") {
        // UPDATED
        
        var RefUrl = SearchTempExcessAirMarkURL();
        $.get(RefUrl, { ControlNo: ControlNum }, function (e) {
            $("#_TempIndicator").val(2);
            $("#OBRNo").val(e.OBRNo);
            $("#_OBRSeries").val(e.OBRNo);
            $("#20PercentFund").prop("checked", true);
            $("#Office").data("kendoComboBox").value(e.Office);

            $("#Program").data("kendoComboBox").value(e.ProgramID);
            $("#Program").data("kendoComboBox").dataSource.read();

            $("#Account").data("kendoComboBox").value(e.accountid);
            $("#Account").data("kendoComboBox").dataSource.read();

            $("#ExcessAccount").data("kendoComboBox").value(e.ExcessAppropriationID);
            $("#ExcessDescription").val(e.Description);
            $("#PPAID").data("kendoComboBox").enable(true);
            $("#PPAID").data("kendoComboBox").value(e.ppaid);
            $("#SubAccount").data("kendoComboBox").value(e.ppaid)
            //setAppropriation();
            //setObligation();
            EditExcessComputation().done(function () {
                console.log("Done Computing in Excess");
                $("#ShowPOAmount").hide();
                $("#AmountExcess").val(e.Amount);
                $("#Claim").val(MoneyFormat(e.Amount));
                $("#_Claim").val(e.Amount);
                $("#_OrigClaim").val(e.Amount);

                sync();
            });
            if ($("#ControlNo").val() == $("#OBRNo").val()) {
                $("#SaveExcessControl").hide();
                $("#ClearExcessControl").hide();
                $("#DeleteExcessControl").show();
                $("#UpdateExcessControl").show();
            }
        });
    }
    else {

        $.get(url, { ControlNum: ControlNum, YearOf: YearOf }, function (e) {
            $("#ShowPOAmount").hide();
            if (e.OBRSeries == '' && e.OBRNo == '') {
                swal("Warning", "No OBR Found! Please enter the logger series number into the transaction number field and hit Enter to generate the OBR number. Then, choose an office and the corresponding account charge from the list.", "warning");
            } else if (e.OBRNo != '') {
                $("#OBRNo").val(e.OBRNo);
                var functcode = $("#OBRNo").val().split("-")
                $("#funcNo").val(functcode[1]);

                //$("#funcNo").val(e.OBRNo)
                var OBR = e.OBRNo;
                //console.log("Your OBR is :" +OBR);
                var url2 = SearchTrnnoURL();
                //swal("Warning", "The OBR you entered is already used.", "warning")
                $.get(url2, { OBR: OBR }, function (e) {
                    if (e == 0) {
                        swal("This OBR has already been used in current control. Can't Proceed!", "", "error");
                    } else {

                        SearchExcessControl(e, $("#controlnew").val());
                    }

                    // Search Excess Details
                });

            } else {
               // alert(e.OBRSeries)
                $("#_TempIndicator").val(0);
                
                $("#OBRNo").val(e.OBRSeries);
                $("#_OBRSeries").val(e.OBRSeries);

                var functcode = $("#OBRNo").val().split("-")
                $("#funcNo").val(functcode[1]);
                //$("#funcNo").val(e.OBRSeries)
            }
        });
        $("#OBRLabel").text("OBRNo:")
    }
    dfrdsetsearchOBR.resolve();
    return dfrdsetsearchOBR.promise();
}

function SearchExcessControl(TrnnoID, transno) {
    // UPDATED

    setExcessData(TrnnoID, transno).done(function () {
        EditExcessComputation().done(function () {
            sync();
        });
    });

     
}

function setExcessData(TrnnoID, transno) {
    // UPDATED

    var dfrdsetExcessData = $.Deferred();
    var url = SearchExcessControlURL();
    var tempExcessubaccountbal = 0;
    $.get(url, { TrnnoID: TrnnoID }, function (e) {

        $("#mode-status").text("EDIT");
        document.getElementById("ControlNo").readOnly = false;
        $("input[name=FundType][value=" + e.FundFlag + "]").attr('checked', true);
        $("#PPAID").data("kendoComboBox").enable(true);
        //alert(e.obrnotemp)
        $("#controlnew").val(transno);
        $("#ControlNo").val(e.obrnotemp);
        $("#Office").data("kendoComboBox").value(e.Office);
        $("#Program").data("kendoComboBox").dataSource.read();
        $("#Program").data("kendoComboBox").value(e.Program);
        $("#Account").data("kendoComboBox").dataSource.read();
        $("#Account").data("kendoComboBox").value(e.NonOfficeAccount);
        $("#ExcessAccount").data("kendoComboBox").dataSource.read();
        $("#ExcessAccount").data("kendoComboBox").value(e.ExcessAppropriationNo);
        $("#_TempExcessAppID").val(e.ExcessAppropriationNo)
        $("#_AcctCharge").val(e.ExcessAppropriationNo);
        $("#PPAID").data("kendoComboBox").dataSource.read();
        $("#PPAID").data("kendoComboBox").value(e.ppaid);
        $("textarea#ExcessDescription").val(e.Description);
        $("#AmountExcess").val(e.Amount);
        $("#OBRNo").val(e.OBRNo);
        var functcode = $("#OBRNo").val().split("-")
        $("#funcNo").val(functcode[1]);

        $("#AmountExcess").val(e.Amount)
        $("#Claim").val(MoneyFormat(e.Amount));
        $("#_Claim").val(e.Amount);
        $("#_ClaimValue").val(e.Amount);
        $("#_ppabalance").val(e.Amount);
        $("#_ModeIndicator").val(1);
        $("#_trnno").val(e.trnno);
        $("#SubAccount").data("kendoComboBox").dataSource.read();
     
        if (e.FundFlag == 0) { //20% pdf 
            $("#SubAccount").data("kendoComboBox").value(e.ppaid);
        } else {
            $("#SubAccount").data("kendoComboBox").value(e.spoid);
        }
        
        if ($("#SubAccount").val() != 0 ){
            GetSubAccountBalance(1);
            //tempExcessubaccountbal = Number($("#_TempExcessBal").val())+ Number($("#_Claim").val());
            //$("#_TempExcessBal").val(tempExcessubaccountbal);
            //alert($("#_TempExcessBal").val());
        }

        $("#SaveExcessControl").hide();
        $("#ClearExcessControl").hide();
        $("#DeleteExcessControl").show();
        $("#UpdateExcessControl").show();
        console.log(e.AcctChecker);
        if (e.AcctChecker >= 1) {
            swal("Error", "The following transaction can't be edited cause it is already processed in Accounting.", "error")
            DisableFields();
        }
        //checkCurrentAllotmentBalance(e.ExcessAppropriationNo, e.FundFlag);
        dfrdsetExcessData.resolve();
    });
    return dfrdsetExcessData.promise();
}

function EditExcessComputation() {

    // UPDATED
    var dfrdEditExcessComputation = $.Deferred();
    var FundType = $('input[name=FundType]:checked').val();
    var AccountID = $("#ExcessAccount").val();
    var Claim = $("#_Claim").val();
    var Year = $("#Years").val();
    var Obligation = 0;
    var Difference = 0;
    var url = ExcessComputationURL();
    $.get(url, { FundID: FundType, AccountID: AccountID, Year: Year }, function (e) {

        if (e.ConnectionStatus == 0) {
            swal(e.MTitle, e.MBody, e.MType);
        } else {
            $("#ExcessTotalAppropriation").val(MoneyFormat(e.Appropriation));
            $("#_ExcessTotalAppropriation").val(e.Appropriation);
            
            console.log("!!");
            var ControlNum = $("#ControlNo").val();
            var str = ControlNum.split("-");
            //if (str[0] != "Ref") {

                var OBRNO = $("#OBRNo").val();
                var url2 = CheckIFOBRExistInAirMarkURL();
                $.get(url2, { OBRNO: OBRNO }, function (a) {
                    console.log("a: " + a.Count);
                    if (a.Count >= 1) {
                       // alert(1)
                        Obligation = Number(e.Obligation) - Number(Claim);
                        $("#ExcessObligation").val(MoneyFormat(Obligation));
                        $("#_ExcessObligation").val(Obligation);
                        $("#_OrigExcessObligation").val(Obligation);
                        
                        $("#ExcessAllotment").val(MoneyFormat(a.Amount));
                        $("#_ExcessAllotment").val(a.Amount) ;
                        dfrdEditExcessComputation.resolve();
                    } else {
                        //alert(2)
                        Obligation = Number(e.Obligation) - Number(Claim);
                        Difference = Number(e.Difference) + Number(Claim);
                       
                        $("#ExcessObligation").val(MoneyFormat(Obligation));
                        $("#_ExcessObligation").val(Obligation);
                        $("#_OrigExcessObligation").val(Obligation);

                        $("#ExcessAllotment").val(MoneyFormat(Difference));
                        $("#_ExcessAllotment").val(Difference);
                        console.log("Set Allotment");
                        dfrdEditExcessComputation.resolve();
                    }

                    
                });

            //}
                
                
        }
        
        
    });
    return dfrdEditExcessComputation.promise();
}

function ExcessComputation() {
    // UPDATED
    var dfrdExcessComputation = $.Deferred();
    var FundType = $('input[name=FundType]:checked').val();
    var AccountID = $("#ExcessAccount").val();
    var Year = $("#Years").val();
    var url = ExcessComputationURL();
    $.get(url, { FundID: FundType, AccountID: AccountID, Year: Year }, function (e) {

        if (e.ConnectionStatus == 0) {
            swal(e.MTitle, e.MBody, e.MType);
        } else {
            $("#ExcessTotalAppropriation").val(MoneyFormat(e.Appropriation));
            $("#_ExcessTotalAppropriation").val(e.Appropriation);
            $("#ExcessObligation").val(MoneyFormat(e.Obligation));
            $("#_ExcessObligation").val(e.Obligation);
            $("#ExcessAllotment").val(MoneyFormat(e.Difference));
            $("#_ExcessAllotment").val(e.Difference);

            //var ControlNum = $("#ControlNo").val();
            //var str = ControlNum.split("-");
            //if (str[0] != "Ref") {

            //    var OBRNO = $("#OBRNo").val();
            //    var url2 = CheckIFOBRExistInAirMarkURL();
            //    $.get(url2, { OBRNO: OBRNO }, function (a) {
            //        console.log("a: " + a.Count);
            //        if (a.Count == 0) {
            //            $("#ExcessAllotment").val(MoneyFormat(e.Difference));
            //            $("#_ExcessAllotment").val(e.Difference);
            //        }
            //    });

            //}
            dfrdExcessComputation.resolve();
        }

        
    });
    return dfrdExcessComputation.promise();
}

function ExcessRegistry()
{
    // UPDATED
    //var selyr = ExSelYear();
    var url = ExcessRegistryUrl();
    
    $.get(url, { YearOf: $("#Years").val() }, function (e) {
        $(window).scrollTop(0);
        $("#ExcessRegistry").html(e);
        $('#ExcessRegistry').data('kendoWindow').title("<i class='fa fa-navicon'> </i> Excess Registry").center().open();
        // $("").disabled = true;
    })
}

function ExcessControl()
{
    // UPDATED
    var url = ExcessControlURL();
    var TransactionYear = $("#Years").val();
    if (TransactionYear == '') {
        swal("Please select your Transaction Year", "", "warning");
    } else {
        $.get(url, { TransactionYear: TransactionYear }, function (e) {
            $(window).scrollTop(0);
            $("#ExcessControl").html(e);
            $("#ExcessControl").data('kendoWindow').title("<i class='fa fa-navicon'> </i> Excess Control").center().open();
        });
    }
    
}

function SaveAppropriation()
{
    var url = SaveAppropriationURL();
    var Amount = $("#AmountAppropriation").val();
    var Account = $("#AccountNameAppropriation").val();
    var Year = $("#YearOf").val();
    var amount = $("#AmountAppropriation").val() == "" ? 0 : $("#AmountAppropriation").val()
    //var FundType = document.querySelector('input[name="FundType"]:checked').value;
    var FundType = $('input[name=FundType]:checked').val();
    if (amount == 0) {
        swal("Warning","Please enter an amount!","warning")
    }
    else {
        $.get(url, { Amount: Amount, AccountName: Account, Year: Year, FundType: FundType }, function (e) {
            if (e == "success") {
                swal("Success", "Excess Appropriation has been Added", "success");
                $("#ExcessAppropriation").data("kendoGrid").dataSource.read();
                ClearAppropriation()
            } else {
                swal("Error", e, "error");
            }
        });
    }
}

function onGridClickAppropriation()
{
    var entityGrid = $("#ExcessAppropriation").data("kendoGrid");
    var selectedItem = entityGrid.dataItem(entityGrid.select());
    $("#AmountAppropriation").val(selectedItem.grAmount);
    $("#AccountNameAppropriation").val(selectedItem.grAccount);
    $("#_PastAccount").val(selectedItem.grAccount);
    $("#YearOf").data("kendoDropDownList").value(selectedItem.grYearOf);
    $("#ExcessID").val(selectedItem.grExcessID);
    console.log(selectedItem.grFundFlag);
    if(selectedItem.grFundFlag == 0)
    {
        $("#20PercentFund").prop("checked", true);
    } else if (selectedItem.grFundFlag == 1) {
        $("#SEFFund").prop("checked", true);
    } else if (selectedItem.grFundFlag == 2) {
        $("#GeneralFund").prop("checked", true);
    } else if (selectedItem.grFundFlag == 3) {
        $("#EcoFund").prop("checked", true);
    }
    $("#SaveAppropriation").hide();
    $("#CloseAppropriation").hide();

    $("#DeleteAppropriation").show();
    $("#UpdateAppropriation").show();
}

function UpdateAppropriation()
{
    var YearOf = $("#YearOf").val();
    var AmountAppropriation = $("#AmountAppropriation").val();
    var AccountNameAppropriation = $("#AccountNameAppropriation").val();
    var FundType = $('input[name=FundType]:checked').val();
    var ExcessID = $("#ExcessID").val();
    var PastAccount = $("#_PastAccount").val();
    var url = UpdateAppropriationURL();

    $.get(url, {
        YearOf: YearOf, AmountAppropriation: AmountAppropriation,
        AccountNameAppropriation: AccountNameAppropriation, FundType: FundType, ExcessID: ExcessID,
        PastAccount: PastAccount
    }, function (e) {
        if (e == "success") {
            swal("Success", "Excess Appropriation has been updated!", "success");
            $("#ExcessAppropriation").data("kendoGrid").dataSource.read();
        } else {
            swal("Error", e, "error");
        }
    });
}

function DeleteAppropriation()
{
    var ExcessID = $("#ExcessID").val();
    var url = DeleteAppropriationURL();

    swal({
        title: "Are you sure?",
        text: "This Excess Appropriation will be Inactive.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Delete",
        closeOnConfirm: false,
        html: true
    },
            function () {
                $.get(url, { ExcessID: ExcessID }, function (e) {
                    if (e == "success") {
                        swal("Success", "Excess Appropriation has been Deleted!", "success");
                        $("#ExcessAppropriation").data("kendoGrid").dataSource.read();
                        ClearForm();
                    } else {
                        swal("System Notice", e, "warning")
                    }
                });
            });   
}

function SetPrograms() {
    
    var Office = $("#Office").val();
    var OBRNo = $("#OBRNo").val();
    var ControlNo = $("#ControlNo").val();
    var url = setProgramOBRURL();
    var contrlsplit = $("#ControlNo").val().split("-")
    if (Office == 43 || Office == 1) {
        $("#Program").data("kendoComboBox").enable(true);
        $("#Program").data("kendoComboBox").dataSource.read();
    } else {
        if (ControlNo != '' && contrlsplit[0] != "Com" && contrlsplit[0] != "com") {
            $.get(url, { Office: Office, OBRNo: OBRNo }, function (e) {
                $("#OBRNo").val(e);
                var functcode = $("#OBRNo").val().split("-")
                $("#funcNo").val(functcode[1]);
            });
        }
    }
}
function SetPPA() {
    var ControlNo = $("#ControlNo").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#Account").val();
    var OBRNo = $("#OBRNo").val();
    var url = setAccountURL();
    var TransactionYear = TransYear();
    var contrlsplit = $("#ControlNo").val().split("-")
  
    if (AccountID == 2861) {
        $("#PPAID").data("kendoComboBox").enable(true);
    } else if (AccountID == 2862) {
        
        $("#PPAID").data("kendoComboBox").enable(true);
        $("#PPAID").data("kendoComboBox").dataSource.read();
        if (ControlNo != '') {
            $.get(url, { ProgramID: ProgramID, AccountID: AccountID, OBRNo: OBRNo, TransactionYear: TransactionYear }, function (e) {
                $("#OBRNo").val(e);
                var functcode = $("#OBRNo").val().split("-")
                $("#funcNo").val(functcode[1]);
            });
        }

    } else {
     
        $("#PPAID").data("kendoComboBox").enable(false);
        if (ControlNo != '' && contrlsplit[0] != "Com" && contrlsplit[0] != "com") {
            $.get(url, { ProgramID: ProgramID, AccountID: AccountID, OBRNo: OBRNo, TransactionYear: TransactionYear }, function (e) {
                $("#OBRNo").val(e);
                var functcode = $("#OBRNo").val().split("-")
                $("#funcNo").val(functcode[1]);
            });
        }

    }
 
}
function setPPAID() {
    // UPDATED
    var PPAID = $("#PPAID").val();
    var OBRNo = $("#OBRNo").val();
    var AccountID = $("#Account").val();
    var ControlNo = $("#ControlNo").val();
    
    //console.log(AccountID);
    var TransactionYear = TransYear();
    var ControlNo = $("#ControlNo").val();

    var url = setPPAOBRURL();
    if (AccountID == 2861) {
        if (ControlNo != '') {
            if (UserType() != 1) {
                $.get(url, { PPAID: PPAID, OBRNo: OBRNo, TransactionYear: TransactionYear, AccountID: AccountID }, function (e) {
                    if (e.length > 0) {
                    $("#OBRNo").val(e);
                    var functcode = $("#OBRNo").val().split("-")
                    $("#funcNo").val(functcode[1]);
                    }
                });
            }
        }
    }
    //else {
    //  //  alert(123)
    //    $.get(urlsub, { Office: 0, Account: 0, Program: 0, TransactionYear: TransactionYear, PPANonOffice: PPAID, Excess: 1 }, function (e) {
    //        $("#_ppabalance").val(e.Amount);
    //        swal(e.message, '', e.type)
    //    });
    //}
   
}
function SetAccounts() {
    // UPDATED
    $("#Account").data("kendoComboBox").enable(true);
    $("#Account").data("kendoComboBox").dataSource.read();
}

function ClearForm()
{
    $("#AmountAppropriation").val("");
    $("#AccountNameAppropriation").val("");
    $("#YearOf").data("kendoDropDownList").select(0);
    $("#20PercentFund").prop("checked", false);
    $("#SEFFund").prop("checked", false);
    $("#GeneralFund").prop("checked", false);
    $("#SaveAppropriation").show();
    $("#ClearAppropriation").show();
    $("#DeleteAppropriation").hide();
    $("#UpdateAppropriation").hide();
}

function getProgram() {
    var OfficeID = $("#Office").val();
    var TransactionYear = TransYear();
    // alert(Year);
    return {
        OfficeID: OfficeID,
        TransactionYear: TransactionYear
    }
}
function getAccount()
{
    var ProgramID = $("#Program").val();
    var TransactionYear = TransYear();
    return {
        ProgramID: ProgramID,
        TransactionYear: TransactionYear
    }
}
function getSubAccount() {
    var ProgramID = $("#Program").val();
    var AccountID = $("#Account").val();
    var Year = TransYear(); //get current year SPO accounts
    var excessid = $("#ExcessAccount").val();
   
    return {
        ProgramID: ProgramID,
        AccountID: AccountID,
        Year: Year,
        excessid: excessid
    }
}
function getExcessAccount()
{
    var FundType = $('input[name=FundType]:checked').val();
    var Years = $("#Years").val();
    return {
        FundType: FundType,
        Years: Years
    }
}
function getPPAID()
{
    var Account = $("#Account").val();
    var TransactionYear = TransYear();
    return {
        TransactionYear: TransactionYear,
        AccountID: Account
    }
}
function setExcessControl() {
    // UPDATED
    var Office = $("#Office").val() == "" ? 0 : $("#Office").val();
    var Program = $("#Program").val() == "" ? 0 : $("#Program").val();
    var Account = $("#Account").val() == "" ? 0 : $("#Account").val();
    var ExcessAccount = $("#ExcessAccount").val() == "" ? 0 : $("#ExcessAccount").val();

    ExcessComputation().done(function () {
        console.log("Done Computing in Excess");

        $("#AmountExcess").val("")
        $("#Claim").val(0);
        $("#_Claim").val(0);
        $("#ExcessBalance").val(0);
        $("#_ExcessBalance").val(0);
    });
    $("#_TempExcessAppID").val(ExcessAccount)
    $("#SubAccount").data("kendoComboBox").dataSource.read();

    var url = NonOfficeSubAccountExcess();
    var TransYear = $("#Years").val()
  //  alert(1);
    $.get(url, { ProgramID: Program, AccountID: Account, TransYear: TransYear, ExcessAccount: ExcessAccount }, function (e) {
        $("#NonOfficeCode").val(e);
      //  alert($("#NonOfficeCode").val())
    });
    //SearchOBR()
}



function setAppropriation()
{
    var FundType = $('input[name=FundType]:checked').val();
    //console.log(FundType);
    var AccountID = $("#ExcessAccount").val();
   // console.log(AccountID);
    var url = setAppropriationURL();
    $.get(url, { AccountID: AccountID, FundType: FundType }, function (e) {
        $("#ExcessTotalAppropriation").val(MoneyFormat(e));
        $("#_ExcessTotalAppropriation").val(e);
    });
}

function setObligation()
{
    var FundType = $('input[name=FundType]:checked').val();
    //console.log(FundType);
    var AccountID = $("#ExcessAccount").val();
    //console.log(AccountID);
    var url = setObligationURL();
    $.get(url, { AccountID: AccountID, FundType: FundType }, function (e) {
        $("#ExcessObligation").val(MoneyFormat(e));
        $("#_ExcessObligation").val(e);

        setTimeout(setAllotmentAvailable(), 1500);
    });
}

function setAllotmentAvailable() {

    var Appropriations = $("#_ExcessTotalAppropriation").val();
    var Obligation = $("#_ExcessObligation").val();
    var Allotment = Number(Appropriations) - Number(Obligation);

    $("#ExcessAllotment").val(MoneyFormat(Allotment));
    $("#_ExcessAllotment").val(Allotment);
    setTimeout(function () {
        sync();
    }, 1000);

    var ControlNo = $("#ControlNo").val();
    var str = ControlNo.split("-");
    if (str[0] == "Ref") {
        var _Amount = $("#_Claim").val();
        $("#ExcessAllotment").val(MoneyFormat(_Amount));
        $("#_ExcessAllotment").val(_Amount);
        sync();
    } else if (str[0] == "20") {
        var _Amount = $("#_Claim").val();
        var ControlNo = $("#ControlNo").val();
        var url = CheckIfRefURL();
        $.get(url, { ControlNo: ControlNo }, function (e) {
            if (e >= 1) {
                $("#ExcessAllotment").val(MoneyFormat(_Amount));
                $("#_ExcessAllotment").val(_Amount);
            } else if(e == 0){
                $("#ExcessAllotment").val(MoneyFormat(Allotment));
                $("#_ExcessAllotment").val(Allotment);
            }
        });
        $("#ExcessAllotment").val(MoneyFormat(Allotment));
        $("#_ExcessAllotment").val(Allotment);
        setTimeout(function () {
            sync();
        }, 1000);
        
    }else {
        var url = CheckIfAirMarkURL();
        $.get(url, { ControlNo: ControlNo }, function (e) {
            console.log("FromAirMark: " + e.FromAirMark);
            console.log("FromOBR: " + e.FromOBR);
            if (e.FromAirMark == 0) {
                if (e.FromOBR == 0) {
                    $("#ExcessAllotment").val(MoneyFormat(Allotment));
                    $("#_ExcessAllotment").val(Allotment);
                } else {
                    var _Amount = $("#_Amount").val();
                    $("#ExcessAllotment").val(MoneyFormat(Allotment));
                    $("#_ExcessAllotment").val(Allotment);
                }
            } else {

                var _Amount = $("#_Claim").val();
                $("#ExcessAllotment").val(_Amount);
                $("#_ExcessAllotment").val(_Amount);


            }
        });

    }
}

function sync() {
        
    var ModeIndicator = $("#_ModeIndicator").val();
    var _AcctCharge = $("#_AcctCharge").val();
    var ExcessAccount = $("#ExcessAccount").val();
    var SPOExcessBal = $("#_TempExcessBal").val();
    var spoid = $("#SubAccount").val();
    var origclaim_amount = $("#_Claim").val();
    var _TempExcessBal = $("#_TempExcessBal").val() == '' ? 0:$("#_TempExcessBal").val();
   // alert(_TempExcessBal);
    
    if (ModeIndicator == 1) {//Update transactions
    //    alert(1);
        if (_AcctCharge != ExcessAccount) {
            console.log("C1");
            var AmountExcess = $("#AmountExcess").val();
           
            var _AmountExcess = $("#_AmountExcess").val();
            var ClaimValue = Number(_AmountExcess) + Number(AmountExcess);
            $("#Claim").val(MoneyFormat(ClaimValue));
            $("#_Claim").val(ClaimValue);

            var _ExcessAllotment = $("#_ExcessAllotment").val();
            var BalanceValue = Number(_ExcessAllotment) - Number(ClaimValue);
            $("#ExcessBalance").val(MoneyFormat(BalanceValue));
            $("#_ExcessBalance").val(BalanceValue);
          
            if (Number(BalanceValue) < 0) {
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
                                                 $("#AmountExcess").val(
                                                    function (index, value) {
                                                        return 0;
                                                    })
                                                 sync();
                                             });
            }
            console.log("!=");
        } else if (ExcessAccount == _AcctCharge) {
            console.log("C2");
            //setAppropriation();
            //ExcessComputation().done(function () {

                //var _ExcessTotalAppropriation = $("#_ExcessTotalAppropriation").val();
                ////var _CurrentObligation = $("#_CurrentObligation").val();
                //var _CurrentObligation = $("#_ExcessObligation").val();
                //var _ClaimValue = $("#_ClaimValue").val();
                

                //var Allotment = Number(_CurrentObligation) - Number(_ClaimValue);
                //$("#ExcessObligation").val(MoneyFormat(Allotment));
                //$("#_ExcessObligation").val(Allotment);
                //var _ExcessObligation = Allotment;

                //var BalanceAllotment = Number(_ExcessTotalAppropriation) - Number(_ExcessObligation);
                //$("#ExcessAllotment").val(MoneyFormat(BalanceAllotment));
                //$("#_ExcessAllotment").val(BalanceAllotment);
                //var _ExcessAllotment = BalanceAllotment;

            var AmountExcess = $("#AmountExcess").val();
            var _ExcessAllotment = $("#_ExcessAllotment").val();
            $("#Claim").val(MoneyFormat(AmountExcess));
            $("#_Claim").val(AmountExcess);
            console.log(_ExcessAllotment);
                var BalanceValue = Number(_ExcessAllotment) - Number(AmountExcess);
                $("#ExcessBalance").val(MoneyFormat(BalanceValue));
                $("#_ExcessBalance").val(BalanceValue);
            //});

            
            console.log("==");
        }
        if (spoid != 0)
        {
            var origclaim_amount = $("#_ppabalance").val();
            var _TempExcessBal = $("#_TempExcessBal").val() == '' ? 0 : $("#_TempExcessBal").val();
            var AmountExcess = $("#AmountExcess").val();
            var tempAmount = Number(origclaim_amount) + Number(_TempExcessBal)
            var diff = parseFloat(Number(tempAmount)).toFixed(2) - parseFloat(Number(AmountExcess)).toFixed(2);
           
            if (parseFloat(diff).toFixed(2) < 0)
            {
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
                            $("#AmountExcess").val(
                            function (index, value) {
                                return 0;
                            })
                            sync();
                        });
            }
        }
    } else if (ModeIndicator == 0) {//New transactions
        console.log("C3");
        var AmountExcess = $("#AmountExcess").val();
        var _AmountExcess = $("#_AmountExcess").val();
        var ClaimValue = Number(_AmountExcess) + Number(AmountExcess);
        var appropriation = $("#ExcessTotalAppropriation").val() //excess appropriation
        var appropriation2 = $("#_ExcessTotalAppropriation").val()
        var totobligation = $("#ExcessObligation").val();
        var excess = $("#_ExcessObligation").val();//total obligation
        var claim = $("#_Claim").val();
        var cntrlno=$("#ControlNo").val().split("-");
        var _OrigClaim = $("#_OrigClaim").val();//Original Claim amount
        var _OrigExcessObligation = $("#_OrigExcessObligation").val();

        $("#Claim").val(MoneyFormat(ClaimValue));
        $("#_Claim").val(ClaimValue);
        
        //Update on 3/12/2019 -xXx - Add filter
        if (cntrlno[0] == "20" || cntrlno[0] == "101" || cntrlno[0] == "201" || cntrlno[0] == "127" || cntrlno[0] == "118" || cntrlno[0] == "119")
        {
            if (Number(ClaimValue) <= Number(_OrigClaim)) {
                var bal1 = parseFloat(Number(excess)).toFixed(2) - parseFloat(Number(ClaimValue)).toFixed(2)
                var bal2 = parseFloat(Number(parseFloat(appropriation2).toFixed(2)) - Number(parseFloat(bal1).toFixed(2))).toFixed(2)
                $("#ExcessObligation").val(MoneyFormat(bal1))
                $("#ExcessAllotment").val(MoneyFormat(bal2))
                $("#_ExcessAllotment").val(bal2)
                var _ExcessAllotment = $("#_ExcessAllotment").val();
            }
            else {
                var bal1 = parseFloat(Number(_OrigExcessObligation)).toFixed(2) - parseFloat(Number(_OrigClaim)).toFixed(2)
                var bal2 = parseFloat(Number(parseFloat(appropriation2).toFixed(2)) - Number(parseFloat(bal1).toFixed(2))).toFixed(2)
                $("#ExcessObligation").val(MoneyFormat(bal1))
                $("#ExcessAllotment").val(MoneyFormat(bal2))
                $("#_ExcessAllotment").val(bal2)
                var _ExcessAllotment = $("#_ExcessAllotment").val();
            }
        }
        //Update on 3/12/2019 -xXx - Add filter
   
        var _ExcessAllotment = $("#_ExcessAllotment").val();
        var BalanceValue = Number(parseFloat(_ExcessAllotment).toFixed(2)) - Number(parseFloat(ClaimValue).toFixed(2));
       
        $("#ExcessBalance").val(MoneyFormat(BalanceValue));
        $("#_ExcessBalance").val(BalanceValue);
     
      //  alert(BalanceValue)
        if (Number(BalanceValue) < 0) {
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
                    $("#AmountExcess").val(
                    function (index, value) {
                        return 0;
                    })
                    sync();
                });
        }
        if (spoid != 0) {
            var AmountExcess = $("#AmountExcess").val();
            var diff = parseFloat(SPOExcessBal).toFixed(2) - parseFloat(AmountExcess).toFixed(2)
            if (parseFloat(diff).toFixed(2) < 0) {
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
                            $("#AmountExcess").val(
                            function (index, value) {
                                return 0;
                            })
                            sync();
                        });
            }
        }
        
    }
}

function SaveExcessControl() {

    var OBRNo = $("#OBRNo").val();
    var FundType = $('input[name=FundType]:checked').val();
    var ExcessAccount = $("#ExcessAccount").val();
    var ExcessDescription = $('textarea#ExcessDescription').val();
    var Amount = $("#AmountExcess").val();
    var PPAID = $("#PPAID").val();
    var Office = $("#Office").val();
    var Program = $("#Program").val();
    var NonOfficeAccount = $("#Account").val();

    var TransactionYear = $("#_TransactionYear").val();
    var Appropriation = $("#_ExcessTotalAppropriation").val();
    var Obligation = $("#_ExcessObligation").val();
    var Allotment = $("#_ExcessAllotment").val();
    var Balance = $("#_ExcessBalance").val();
    var OBRSeries = $("#_OBRSeries").val();
    var TempIndicator = $("#_TempIndicator").val();
    var ControlNo = $("#ControlNo").val();
    var Account = $("#Account").val();
    var SubAccount = $("#SubAccount").val() == "" ? 0 : $("#SubAccount").val();
    var TransYearex = $("#_TransactionYear").val();
    //var PPAID = $("#PPAID").val();
    var cntrlno = $("#ControlNo").val().split("-")
    var userOffficeid = $("#userOffficeid").val()
    var funcNo = $("#funcNo").val();
    var OBRNo = $("#OBRNo").val();
    
    if ((ControlNo == '' || cntrlno[0] == 'Ref' || cntrlno[0] == 'REF' || cntrlno[0] == 'Com' || cntrlno[0] == 'com') && (userOffficeid == 19 || userOffficeid == 1 || userOffficeid == 43))
    {
        TempIndicator = 1;
    }
  
    if (Account == 2628 && (SubAccount == 0 || SubAccount == '')) {
        swal("Warning", "Please Select Sub-ppa!", "warning");
    }
    //else if (Account == 2861 && (PPAID == 0 || PPAID == ''))
    //{
    //    swal("Error", "Please Select PPA's Account!", "error");
        //}
    else if (ExcessAccount == 0) {
        swal("Warning", "Please select account charge!", "warning");
    }
    else if ($("#NonOfficeCode").val() > SubAccount) {
        swal("Warning", "Please select sub-ppa!", "warning");
    }
    else if (Office == 43 && Account == 0) {
        swal("Warning", "No function code assigned! Please select an account from the dropdown list.", "warning");
        $("#Account").focus;
    }
    else if (funcNo.length != 4) { //function code / responsibility center
        swal("Warning", "Please review your entry or check the function code!", "warning");
    }
    else if( Office <= 0 ){
        swal("Warning", "Please select an office!", "warning");
        $("#Office").focus();
    }
    else {
        var url = SaveExcessControlURL();
        $.get(url, {
            OBRNo: OBRNo, FundType: FundType, ExcessAccount: ExcessAccount,
            ExcessDescription: ExcessDescription, Amount: Amount, PPAID: PPAID, TransactionYear: TransactionYear,
            Appropriation: Appropriation, Obligation: Obligation, Allotment: Allotment, Balance: Balance,
            Office: Office, Program: Program, NonOfficeAccount: NonOfficeAccount, OBRSeries: OBRSeries, TempIndicator: TempIndicator, ControlNo: ControlNo,
            SubAccount: SubAccount
        }, function (e) {
            if (e.message == "success") {
                swal("Success", "Transaction sucessfully saved.", "success");
                $("#grPastObligated").data("kendoGrid").dataSource.read();
                $("#OBRNo").val(e.OBRNo);
            } else {
               swal("Error", e.message, "error");
            }
       
        });
    }
}

function GridParam()
{
    var TransactionYear = $("#Years").val();
    return {
        TransactionYear: TransactionYear
    }
}

function ExcessEdit(TrnnoID, transno) {
    var TrnnoID = TrnnoID;
    var url = ExcessEditURL();
    var TransactionYear = $("#Years").val();
    //console.log(TrnnoID);
    //console.log(TransactionYear);
    $.get(url, { TransactionYear: TransactionYear }, function (e) {
            $(window).scrollTop(0);
            $("#ExcessControl").html(e);
            $("#ExcessControl").data('kendoWindow').title("<i class='fa fa-navicon'> </i> Excess Control").center().open();
            setTimeout(SearchExcessControl(TrnnoID, transno), 500);
        });
}

function checkCurrentAllotmentBalance(ExcessAccount, Fundflag) {

    var url = setObligationURL();
    $.get(url, { AccountID: ExcessAccount, FundType: Fundflag }, function (e) {
        $("#_CurrentObligation").val(e);
    });
}

function UpdateExcessControl() {

    var OBRNo = $("#OBRNo").val();
    var FundType = $('input[name=FundType]:checked').val();
    var ExcessAccount = $("#ExcessAccount").val();
    var ExcessDescription = $('#ExcessDescription').val();
    var Amount = $("#AmountExcess").val();
    var PPAID = $("#PPAID").val();
    var Office = $("#Office").val();
    var Program = $("#Program").val();
    var NonOfficeAccount = $("#Account").val();

    var TransactionYear = $("#_TransactionYear").val();
    var Appropriation = $("#_ExcessTotalAppropriation").val();
    var Obligation = $("#_ExcessObligation").val();
    var Allotment = $("#_ExcessAllotment").val();
    var Balance = $("#_ExcessBalance").val();
    var trnno = $("#_trnno").val() == "" ? 0 : $("#_trnno").val();
    var SubAccount = $("#SubAccount").val();
    var url = UpdateExcessControlURL();
    var Account = $("#Account").val();
    var SubAccount = $("#SubAccount").val();
    var OBRNoTemp = $("#ControlNo").val();
    //alert(SubAccount);
    var PPAID = $("#PPAID").val();
    if (Account == 2628 && (SubAccount == 0 || SubAccount == '')) {
        swal("Error", "Please Select Sub-account!", "error");
    }
    else if (Account == 2861 && (PPAID == 0 || PPAID == '')) {
        swal("Error", "Please Select PPA's Account!", "error");
    }
    else if ($("#NonOfficeCode").val() > SubAccount) {
        swal("Error", "Please select sub-ppa!", "error");
    }
    else {
        $.get(url, {
            OBRNo: OBRNo, FundType: FundType, ExcessAccount: ExcessAccount,
            ExcessDescription: ExcessDescription, Amount: Amount, PPAID: PPAID, TransactionYear: TransactionYear,
            Appropriation: Appropriation, Obligation: Obligation, Allotment: Allotment, Balance: Balance,
            Office: Office, Program: Program, NonOfficeAccount: NonOfficeAccount, trnno: trnno, SubAccount: SubAccount, OBRNoTemp: OBRNoTemp
        }, function (e) {
            swal("Success", "Transaction Sucessfully Updated.", "success");
            $("#grPastObligated").data("kendoGrid").dataSource.read();
        });
    }
}
function ExcessDelete(TrnnoID, grOBRNo) {
    var url = SearchOBRNoURL();
    var url2 = DeleteExcessControlURL();
   
    var str = grOBRNo.split("-");
    //if(str[0] != "20" && Account.UserInfo.Department == 1)
    //{
    //    swal("Error", "Sorry! Your're not allowed to delete this particular obligation!", "error")
    //}
    //else {
        $.get(url, { TrnnoID: TrnnoID }, function (data) {
            swal({
                title: "Are you sure?",
                text: "This Excess Control will be Inactive and you will not be able to recover it",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Delete",
                closeOnConfirm: false,
                html: true
            },
               function () {
                   var OBRNo = data;
                   $.get(url2, { OBRNo: OBRNo }, function (e) {
                       if (e == 'success') {
                           swal("Success", "Transaction has been deleted!", "success");
                           $("#grPastObligated").data("kendoGrid").dataSource.read();
                       } else if (e == 'existed') {
                           swal("Error", "The following transaction can't be deleted cause it is already processed in Accounting.", "error")
                       }
                       else {
                           swal("Error", "Something went wrong.", "error");
                       }
                   });
               });
        });
    //}
}
function DeleteExcessControl()
{
    var OBRNo = $("#OBRNo").val();
    var url = DeleteExcessControlURL();
    console.log("OBRNo : " + OBRNo);
    swal({
        title: "Are you sure?",
        text: "This Excess Control will be Inactive and you will not be able to recover it",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Delete",
        closeOnConfirm: false,
        html: true
    },
           function () {
               console.log("OBRNo2 : " + OBRNo);
               $.get(url, { OBRNo: OBRNo }, function (e) {
                   if (e == 'success') {
                       swal("Success", "Transaction has been deleted!", "success");
                       $("#grPastObligated").data("kendoGrid").dataSource.read();
                       ClearExcessControl();
                   } else if (e == 'existed') {
                       swal("Error", "The following transaction can't be edited cause it is already processed in Accounting.", "error")
                   }
                   else {
                       swal("Error", "Something went wrong.", "error");
                   }
               });
           });

   
}



function DisableFields()
{
   
        $("#Program").data("kendoComboBox").enable(false);
        $("#Account").data("kendoComboBox").enable(false);
        $("#PPAID").data("kendoComboBox").enable(false);
        $("#Office").data("kendoComboBox").enable(false);
        $("#ExcessAccount").data("kendoComboBox").enable(false);
        $('#ExcessDescription').prop('readonly', true);
        document.getElementById("AmountExcess").readOnly = true;
        $('input[name=FundType]').prop('disabled', true);
        $('#DeleteExcessControl').prop('onclick', null).off('click');
        $('#UpdateExcessControl').prop('onclick', null).off('click');

        $("#DeleteExcessControl").attr('class', 'k-button k-state-disabled');
        $("#UpdateExcessControl").attr('class', 'k-button k-state-disabled');
}

function ClearAppropriation() {

    $("#YearOf").data("kendoDropDownList").value(0);
    $("#AmountAppropriation").val("");
    $("#AccountNameAppropriation").val("");
    $("input[name=FundType]").attr('checked', false);

    $("#SaveAppropriation").show();
    $("#CloseAppropriation").show();
    $("#DeleteAppropriation").hide();
    $("#UpdateAppropriation").hide();

}

function ClearExcessControl() {

    $("input[type=number]").val("");
    $("input[type=text]").not("#Years").val("");
    $("textarea#ExcessDescription").val("");
    $("input[type=radio]").attr('checked', false);
    $("#ExcessAccount").data("kendoComboBox").value("");
}

function CloseAppropriation() {
    $("#ExcessRegistry").data("kendoWindow").close();
}

function CloseExcessControl() {
    $("#ExcessControl").data("kendoWindow").close();
}

function CheckStatus() {
    var UserTypeID = TypeID();
    var GetUserID = getOfficeID();
    var Years = $("#Years").val();
    var url = SetTempOBRURL();
  //  alert(Years);
    // if (UserTypeID == 1 || GetUserID == 1 || GetUserID == 63) {
    // $("#OBRNoEntry").attr("readOnly", true);
    $("#OBRLabel").html("Temp OBR No.");
    $.get(url, { Years: Years }, function (e) {
        $("#OBRNo").val(e);
        var fncode = $("#OBRNo").val().split('-');
        $("#funcNo").val(fncode[1])

    });
    //if (GetUserID == 1 || GetUserID == 63) {
    //    //$("#Office").val(getOfficeID());
    //    $("#PPAOffice").data("kendoComboBox").value(getOfficeID());
    //    $("#OfficeID").val(getOfficeID());
    //    //$("#Office").data("kendoComboBox").enable(false);
    //}

    //  }
    
}

function GetSubAccountBalance(id) {
    var TYear = $("#Years").val();
    var spoid = $("#SubAccount").val();
    var accountid = $("#Account").val();
    var SubAccount = $("#SubAccount").val();

    //var url0 = GetExcessAccountIDURL();
    //$.get(url0, { TYear: TYear }, function (e) {
    //    $("#_TempExcessAppID").val(e);
   
    //});
    var tempExcessid = $("#_TempExcessAppID").val() == "" ? $("#ExcessAccount").val() : $("#_TempExcessAppID").val();
    var url = GetSubAccountBalanceURL();
    //alert(tempExcessid)
    //alert($("#ExcessAccount").val())
    $.get(url, { TYear: TYear, spoid: spoid, tempExcessid: tempExcessid, SubAccount: SubAccount }, function (e) {
        swal({
            title: "Excess Appropriation Balance",
            text: "Remaining Balance : " + MoneyFormat(e),
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok!",
            closeOnConfirm: true
        })
        $("#_TempExcessBal").val(e);
        
        if ($("#20PercentFund").val() == 0)
        {
            $("#PPAID").data("kendoComboBox").dataSource.read();
            $("#PPAID").data("kendoComboBox").value(SubAccount);
            setPPAID();
        }
        //alert(456)
        //setPPAID();
    });
}