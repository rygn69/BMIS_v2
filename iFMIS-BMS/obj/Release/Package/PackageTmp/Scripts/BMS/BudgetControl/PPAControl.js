var GridClickIdentifier=0;
function Transaction() {
    swal({
        title: "Proceed to next Transactions?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#007f00",
        confirmButtonText: "Proceed",
        closeOnConfirm: true,
        closeOnCancel: true
    },
    function (isConfirm) {
        if (isConfirm) {
            var url = TransactionURL();
            $.get(url, function (e) {
                $("#UserTransactionID").val(e);
                $("#cttsppa").focus();
            });
        } else {
            setTimeout(function () {
                $("#PPAControl").closest(".k-window-content").data("kendoWindow").close();
            }, 1000);            
        }
        
    });
}
function MainPPAParam() {
    var TransactionYear = $("#Years").val();
    return {
        TransactionYear: TransactionYear
    }
}

$(function () {

    $("#TransactionNo").change(function (e) {
        searchOBRDetails();
    }).keypress(function (e) {
        if (e.keyCode == 13) {
           searchOBRDetails();
        }
    });

    var url = SearchPPATransaction()// "@Url.Action('SearchTransno', 'BudgetControl')";
    $("#cttsppa").keyup(function (event) {
        $.get(url, { cttsqr: $("#cttsppa").val() }, function (e) {
            $("#TransactionNo").val(e);
            if (e != 0) {
                searchOBRDetails()
            }
        })
    });
    //$("#TransactionNo").keypress(function (e) {
    //    if (e.keyCode == 13) {
    //        searchOBRDetails();
    //    }
    //});
});

function searchOBRDetails() {
    var TransactionNo = $("#TransactionNo").val() == ""? 0  :$("#TransactionNo").val() ;
    var PPATransOBRNo = $("#PPATransOBRNo").val();
    $("#PPAOBRNo").val(TransactionNo)
    $("#_onGridClickIdentifier").val(0);
    if (TransactionNo == '' || TransactionNo == null) {
        TransactionNo = 0;
    }

    var str = TransactionNo.split("-");

    if (str[0] == "Ref") {
        
        SetRefInfo().done(function () {
            console.log("Done Setting up Ref Info");
            xPPAComputation().done(function () {
                sync();
            });
        });
        

    } else {

        SetPPATransInfo().done(function () {
            setTransDetails();
        });
    }
}

function SetPPATransInfo() {

    var dfrdSetPPATransInfo = $.Deferred();
    var TransactionNo = $("#TransactionNo").val();
    var trnnoUrl = trnnoURL();
    var Year = $("#Years").val();
    $.get(trnnoUrl, { getControlNo: TransactionNo, tyear: Year }, function (a) {

        $("#PPAOBRNo").val(a);
        if (TransactionNo == 0) {
            a = 0;
        }
        if (a == '' && str[0] != "20") {
            swal("Warning", "OBRNo. Not Found!", "error");
        }
        dfrdSetPPATransInfo.resolve();
    });

    return dfrdSetPPATransInfo.promise();
}

function SetRefInfo() {
    var dfrdSetRefInfo = $.Deferred();
    var TransactionNo = $("#TransactionNo").val();
    var PPATransOBRNo = $("#PPATransOBRNo").val();
    var CountUserTypeID = TypeID();
  
    var CountOfficeRef = 0;
    
    var AirMark = PPAAirMarkURL();
    $.get(AirMark, { getControlNo: TransactionNo, PPATransOBRNo: PPATransOBRNo }, function (e) {

        if (CountUserTypeID == 2 || CountUserTypeID == 4 || CountUserTypeID == 5) {
            $("#PPAOBRNo").val(e.AROBRNo);
            $("#PPATransOBRNo").val(e.AROBRSeries);
            $("#OBRNoCenter").val(e.AROBRSeries);

        }
        $("#_ChangePPAChecker").val(0);

        $("#PPAOffice").data("kendoComboBox").value(e.AROffice);
        $("#OfficeID").val(e.AROffice);
        $("#PPAProgram").data("kendoComboBox").dataSource.read();
        $("#PPAProgram").data("kendoComboBox").value(e.ARProgram);

        $("#ObjectExpenditure").prop("checked", true);


        $("#PPAObjOfExpenditure").data("kendoComboBox").enable(true);
        $("#PPAObjOfExpenditure").data("kendoComboBox").dataSource.read();
        $("#PPAObjOfExpenditure").data("kendoComboBox").value(e.ARAccount);

        $("#PPAOOE").data("kendoComboBox").value(e.AROOE);
        $("#RootPPA").data("kendoComboBox").enable(true);

        $("#RootPPA").data("kendoComboBox").value(e.RootPPAID);

        $("#PPA").data("kendoComboBox").enable(true);
        $("#PPA").data("kendoComboBox").dataSource.read();
        $("#PPA").data("kendoComboBox").value(e.PPAID);
        //setPPAID();

        

        $("#PPAAccountCharged").data("kendoComboBox").enable(true);
        $("#PPAAccountCharged").data("kendoComboBox").dataSource.read();
        $("#PPAAccountCharged").data("kendoComboBox").value(e.ARAccount);
        //setTimeout(function () {
            
        //}, 500);

        $("#ActualAmountValue").val(e.ARAccount);
        $("#AccntChargeValue").val(e.ARAccount);
        $("#PPAActualAccount").data("kendoComboBox").enable(true);
        $("#PPAActualAccount").data("kendoComboBox").dataSource.read();
        $("#PPAActualAccount").data("kendoComboBox").value(e.ARAccount);

        $("#PPAvailable").val(MoneyFormatWithDecimal(e.ARAmount));
        $("#_PPAvailable").val(e.ARAmount);
        $("#AmountInputed").val(e.ARAmount);
        $("#AccntChargeValue").val(e.ARAmount);
        $("#_Amount").val(e.ARAmount);

        $("#PPAAdd").attr("class", "k-button k-state-disabled");
        $('#PPAAdd').prop('onclick', null).off('click');
        $("#PPASave").attr("class", "k-button");
        $("#PPASave").attr("onclick", "PPASave()");

        dfrdSetRefInfo.resolve();
    });

    return dfrdSetRefInfo.promise();

}

function setTransDetails() {
    var ControlNo = $("#PPAOBRNo").val();
    var url = SearchOBRURL();
    $.get(url, { ControlNo: ControlNo }, function (e) {
       
        var TransactionNo = $("#TransactionNo").val();
        var TransRes = TransactionNo.split("-");
        if(TransRes[0] == "20"){

            var urlGetOBR = OBRNo20FromTempOBRURL();
            $.get(urlGetOBR, { ControlNo: TransactionNo }, function (d) {
                
                $("#PPATransOBRNo").val(d.OBRNo);
                $("#OBRNoCenter").val(d.OBRNo);
                $("#PPAOffice").data("kendoComboBox").value(e.OfficeID);
                $("#PPAProgram").data("kendoComboBox").dataSource.read();
                $("#PPAProgram").data("kendoComboBox").value(e.ProgramID);
                $("#PPAObjOfExpenditure").data("kendoComboBox").dataSource.read();
                $("#PPAObjOfExpenditure").data("kendoComboBox").value(2861);
                var resultOBR = d.OBRNo.split("-");
                $("#PPAFundType").data("kendoComboBox").value(118);
                $("#PPAFundType").data("kendoComboBox").enable(true);
                $("#PPAFundTypeValue").val(resultOBR[0]);
                $("#_RefIdentifier").val(1);
             
                if (d.obrseries != 0 && d.functioncode == 0) {
                    $("#fcode_temp").val(1)
                }
                else {
                    $("#fcode_temp").val(0)
                }
            });

        } else {
            var res = e.OBRNo.split("-");
            $("#PPATransOBRNo").val(e.OBRNo);

            var fcode = e.OBRNo.split('-')
            $("#PPAFuntionCode").val(fcode[1])

            if (fcode[0] != '118')
            {
                swal("Warning", "Fund code doesn't match for 20% Control. Please check the transaction number in the logger.", "warning")
                $("#TransactionNo").val(0)
                searchOBRDetails();
            }

            $("#OBRNoCenter").val(e.OBRNo);
            $("#PPAOffice").data("kendoComboBox").value(e.OfficeID);
            $("#PPAProgram").data("kendoComboBox").dataSource.read();
            $("#PPAProgram").data("kendoComboBox").value(e.ProgramID);
            $("#PPAObjOfExpenditure").data("kendoComboBox").dataSource.read();
            $("#PPAObjOfExpenditure").data("kendoComboBox").value(2861);
            $("#PPAFundType").data("kendoComboBox").value(res[0]);
            $("#PPAFundType").data("kendoComboBox").enable(true);
            $("#PPAFundTypeValue").val(res[0]);
            $("#_RefIdentifier").val(0);
        }

        // Display : Search Account if Obligated || Search Raw Data if not yet Saved
        $("#grPPAName").data("kendoGrid").dataSource.read();
       // $("#grAccountName").data("kendoGrid").dataSource.read();
        var dataSource2 = $("#grPPAName").data("kendoGrid").dataSource;
        dataSource2.fetch(function () {
            var countGrid = dataSource2.total();

            var CheckIfCanceled = CheckIfCanceledURL();

            $.get(CheckIfCanceled, { ControlNo: ControlNo }, function (z) {
                if (z != '' && countGrid == 0) {
                    swal("Warning!", "The OBR you've entered has already been canceled or Invalid!", "error");
                } else if (countGrid == 0) {
                    $("#mode-status").html("NEW")
                    $("#PPAExpenseClass").attr("disabled", false);
                    $("#PPAObjectExpenditure").attr("disabled", false);
                    $("#PPAExpenseDescription").attr("disabled", false);
                } else {
                    $("#mode-status").html("EDIT");
                    $("#mode-status").css({ "color": "green", "text-decoration": "none" });
                }
            });

            if (countGrid >= 2) {
                $("#PPAExpenseClass").prop("checked", true);
                $("#PPAObjectExpenditure").attr("disabled", true);
            }
        });
    });
 
    $("#Return").hide();
    $("#Update").hide();
    $("#Save").show();
    $("#Add").show();
    $("#Clear").show();
    $("#Save").attr("class", "k-button");
    $("#Save").attr("onclick", "PPASave()");
    $("#Add").attr("class", "k-button");
    $("#Add").attr("onclick", "PPAAdd()");

    //ClearForm(1);

    var CheckIfExist = CheckIfExistUrl();
    $.get(CheckIfExist, { ControlNo: ControlNo }, function (z) {
        if (z != "0") {
            $("#Save").hide();
            $("#Cancel").show();
            $("#Add").attr("class", "k-button k-state-disabled");
            $('#Add').prop('onclick', null).off('click');
        }
    });
}

function GridClick() {
    
    GridClickIdentifier = 1;
    var entityGrid = $("#grPPAName").data("kendoGrid");

    var selectedItem = entityGrid.dataItem(entityGrid.select());
    $("#_onGridClickIdentifier").val(1);
    $("#PPATransactionType").data("kendoDropDownList").value(selectedItem.grPPATransactionType);
    $("#PPAModeOfExpense").data("kendoDropDownList").value(selectedItem.grPPAModeOfExpense);
    $("#RootPPA").data("kendoComboBox").value(selectedItem.grRootPPA);
    $("#PPA").data("kendoComboBox").value(selectedItem.grPPA);
    $("#PPAOOE").data("kendoComboBox").value(selectedItem.grPPAOOE);
    $("#PPADescription").val(selectedItem.grPPADescription);
    $("#Remarks").val(selectedItem.grRemarks);
    //setTimeout(function () {
    // Mode Indicator 1 means Edit Mode 
        
        //setPPAID();
        $("#AmountInputed").val(selectedItem.grPPAClaim);
        $("#_Amount").val(selectedItem.grPPAClaim);
        $("#_Claim").val(selectedItem.grPPATotalClaim);
        $("#Claim").val(MoneyFormatWithDecimal(selectedItem.grPPATotalClaim));
        $("#ModeIndicator").val(1);
    //console.log("GridClick");
  
        xEditPPAComputation().done(function () {
            sync();
        });
        //xPPAComputation().done(function () {

        //    sync();
        //})
        //console.log("AmountInputed: " + selectedItem.grPPAClaim);
        //setTimeout(function () {

            
            
        //    var AmountInputted = $("#AmountInputed").val();
        //    var _PPAvailable = $("#_PPAvailable").val();
        //    var PPABalanceAllotment = Number(_PPAvailable) - Number(AmountInputted);
        //    $("#PPABalanceAllotment").val(MoneyFormatWithDecimal(PPABalanceAllotment));
        //    $("#_PPABalanceAllotment").val(PPABalanceAllotment);
            

        //}, 1000);
        
    //}, 500);
    $("#_SubsidyID").val(selectedItem.grSubsidyID);
    $("#_PPAID").val(selectedItem.grPPAID);
    $("#_AcctChecker").val(selectedItem.grPPAAcctChecker);
    var AcctChecker = selectedItem.grPPAAcctChecker;
    //console.log("AcctChecker: " + AcctChecker)
    if (AcctChecker == 0) {
        var RefIdentifier = $("#_RefIdentifier").val();
        //console.log("_RefIdentifier" + RefIdentifier);
        if (RefIdentifier == 1) {
            // $("#AccountCharged").data("kendoComboBox").enable(true);
            if ($("#yearendtransppa").val() == 1) {
                $("#PPAUpdate").show();
                $("#PPACancel").show();
                $("#PPASave").show();
            }
            $("#PPAClear").hide();
            $("#PPAAdd").hide();
            
            $("#PPASave").attr("class", "k-button");
            $("#PPASave").attr("onclick", "PPASave()");
            

        } else if (RefIdentifier == 0) {
            $("#PPAAdd").hide();
            $("#PPASave").hide();
            if ($("#yearendtransppa").val() == 1) {
                $("#PPAClear").show();
                $("#PPAUpdate").show();
                $("#PPACancel").show();
            }
        }

    } else if (AcctChecker == 1) {
        if ($("#yearendtransppa").val() == 1) {
            $("#PPAUpdate").show();
            $("#PPAReturn").show();
        }
        $("#PPAUpdate").attr("class", "k-button k-state-disabled");
        $("#PPAUpdate").prop('onclick', null).off('click');
        $("#PPAAdd").hide();
        $("#PPASave").hide();

        swal(selectedItem.grRemarks, '', "warning");
    }
   // GridClickIdentifier = 0;
}

function xEditPPAComputation() {
    var dfrdPPAComputation = $.Deferred();
    var ControlNo = $("#TransactionNo").val();
    var str = ControlNo.split('-');
    var Year = $("#Years").val();
    var AccountID = $("#PPAObjOfExpenditure").val();
    var RootPPA = $("#RootPPA").val();
    var PPAID = $("#PPA").val();
    var AmountInputted = $("#_Amount").val();
 //   console.log("xEditPPAComputation");
    var Obligationp20P = 0;
    var Difference20P = 0;
    var PPAObligation = 0;
    var PPADifference = 0;
    var PPAAvailable = 0;
    var ProgramID=$("#PPAProgram").val()
    var url = PPAComputationURL();
    $.get(url, { Year: Year, AccountID: AccountID, RootPPA: RootPPA, PPAID: PPAID, ControlNo: ControlNo, ProgramID: ProgramID }, function (e) {
        $("#AcctRelease").val(MoneyFormatWithDecimal(e.AcctRelease));
        $("#_AcctRelease").val(e.AcctRelease);
        console.log("23")
        console.log(e.AcctRelease)

        Obligationp20P = Number(e.AcctObligation) - Number(AmountInputted);
        Difference20P = Number(e.AcctDifference) + Number(AmountInputted);


        PPAObligation = Number(e.RootPPAObligation) - Number(AmountInputted);
        PPADifference = Number(e.RootPPADifference) + Number(AmountInputted)
        $("#AcctObligation").val(MoneyFormatWithDecimal(Obligationp20P));
        $("#_AcctObligation").val(Obligationp20P);
        $("#_AcctDifference").val(Difference20P);
        $("#PPARelease").val(MoneyFormatWithDecimal(e.RootPPARelease));
        $("#_PPARelease").val(e.RootPPARelease);
        $("#PPAObligate").val(MoneyFormatWithDecimal(PPAObligation));
        $("#_PPAObligate").val(PPAObligation);
        $("#_PPADifference").val(PPADifference);

        if (str[0] != "Ref" && GridClickIdentifier == 0) {
            $("#PPAvailable").val(MoneyFormatWithDecimal(e.PPADifference));
            $("#_PPAvailable").val(e.PPADifference);
        }

        PPAAvailable = Number(e.PPADifference) + Number(AmountInputted);

        $("#PPAvailable").val(MoneyFormatWithDecimal(PPAAvailable));
        $("#_PPAvailable").val(PPAAvailable);
       
        console.log("Set Allotment");
        dfrdPPAComputation.resolve();

        var TransactionNo = $("#TransactionNo").val();
        var url2 = CheckIFOBRExistInAirMarkURL();
        $.get(url2, { OBRNO: TransactionNo }, function (a) {
            console.log("a: " + a.Count);
            if (a.Count >= 1) {
                $("#PPAvailable").val(MoneyFormatWithDecimal(a.Amount));
                $("#_PPAvailable").val(a.Amount);
                dfrdPPAComputation.resolve();
            } else {
                // PPAAvailable = Number(e.PPADifference) + Number(AmountInputted);
               
                //$("#PPAvailable").val(MoneyFormatWithDecimal(PPAAvailable));
                //$("#_PPAvailable").val(PPAAvailable);
                //console.log("Set Allotment");
                //// dfrdEditExcessComputation.resolve();
                //dfrdPPAComputation.resolve();
            }

        });

    });
    return dfrdPPAComputation.promise();
}

function PPATransactionType() {
    var TransTypeID = $("#PPATransactionType").val();
    return {
        TransTypeID: TransTypeID
    }
}

function refreshProgram() {

    //var officeid = $("#ppaoffice").val();
    //$("#ppaoffice").val(officeid);

    //var tempobrno = $("#obrnocenter").val();
    
    //    var url = changeobrurl();
    //    $.get(url, { tempobrno: tempobrno, officeid: officeid }, function (e) {
    //        $("#ppatransobrno").val(e);
    //        document.getelementbyid("ppatransobrno").value = e;
    //        //clearform();
    //    });

    $("#PPAProgram").data("kendoComboBox").dataSource.read()

}

function refreshExpenditure() {
    $("#PPAObjOfExpenditure").data("kendoComboBox").dataSource.read()
}
function PPAProgram() {
    var OfficeID = $("#PPAOffice").val();
    var d = new Date();
    var Year2 = d.getFullYear();
    return {
        OfficeID: OfficeID,
        TransactionYear: Year2
    }
}

function AccountParam() {
    var ProgramID = $("#PPAProgram").val();
    var OOE = $("#PPAOOE").val();
    var TYear = $("#Years").val();
    return {
        ProgramID: ProgramID,
        OOE: OOE,
        TYear: TYear
    }
}

function PPAParam() {
    var TransactionYear = $("#Years").val();
    var AccountID = $("#RootPPA").val();
    return {
        TransactionYear: TransactionYear,
        AccountID: AccountID
    }
}

function ComputePPA() {
    xPPAComputation().done(function () {
        sync();
    });
}

function xPPAComputation() {
    var dfrdPPAComputation = $.Deferred();
    var ControlNo = $("#TransactionNo").val() == "" ? $("#PPATransOBRNo").val() : $("#TransactionNo").val();
    var str = ControlNo.split('-');
    var Year = $("#Years").val();
    var AccountID = $("#PPAObjOfExpenditure").val();
    var RootPPA = $("#RootPPA").val();
    var PPAID = $("#PPA").val();
    var url = PPAComputationURL();
    var functioncode = "";
    var ProgramID = $("#PPAProgram").val();
    
    $.get(url, { Year: Year, AccountID: AccountID, RootPPA: RootPPA, PPAID: PPAID, ControlNo: ControlNo, ProgramID: ProgramID }, function (e) {
        $("#AcctRelease").val(MoneyFormatWithDecimal(e.AcctRelease));
        console.log("24")
        console.log(e.AcctRelease)
        $("#_AcctRelease").val(e.AcctRelease);
        $("#AcctObligation").val(MoneyFormatWithDecimal(e.AcctObligation));
        $("#_AcctObligation").val(e.AcctObligation);
        $("#_AcctDifference").val(e.AcctDifference);
        $("#PPARelease").val(MoneyFormatWithDecimal(e.RootPPARelease));
        $("#_PPARelease").val(e.RootPPARelease);
        
        if (str[0] != "REF" && str[0] != "Ref") {
            $("#PPATransOBRNo").val(e.obrno);
        }

        functioncode = $("#PPATransOBRNo").val().split('-')
        $("#PPAFuntionCode").val(functioncode[1])
     

       // console.log("GridClickIdentifier:" + GridClickIdentifier)
        //var fnccode = $("#TransOBRNo").val().split('-')
        //$("#functioncode").val(fnccode[1])


        if (str[0] != "Ref" && GridClickIdentifier == 0) {
            $("#PPAvailable").val(MoneyFormatWithDecimal(e.PPADifference));
            $("#_PPAvailable").val(e.PPADifference);
        } else if (str[0] != "Ref" && GridClickIdentifier == 1) {
            console.log("GridClickIdentifier 1");
            var _Amount = $("#_Amount").val();
            var PPAObligation = e.RootPPAObligation;
            var PPARelease = e.RootPPARelease;
            var PPATrimObligation = Number(PPAObligation) - Number(_Amount);
            var PPADifference = Number(PPARelease) - Number(PPATrimObligation);
            console.log("PPADifference: " + PPADifference);
            $("#PPAObligate").val(MoneyFormatWithDecimal(PPATrimObligation));
            $("#_PPAObligate").val(PPATrimObligation);
            $("#PPAvailable").val(MoneyFormatWithDecimal(PPADifference));
            
            $("#_PPADifference").val(PPADifference);
        }

        if (str[0] == "Ref") {
            var _Amount = $("#_Amount").val();
            var PPAObligation = e.RootPPAObligation;
            var PPARelease = e.RootPPARelease;
            var PPATrimObligation = Number(PPAObligation) - Number(_Amount);
            var PPADifference = Number(PPARelease) - Number(PPATrimObligation);
            $("#PPAObligate").val(MoneyFormatWithDecimal(PPATrimObligation));
            $("#_PPAObligate").val(PPATrimObligation);
            
            $("#_PPADifference").val(PPADifference);

        } else if(GridClickIdentifier == 0){
            $("#PPAObligate").val(MoneyFormatWithDecimal(e.RootPPAObligation));
            $("#_PPAObligate").val(e.RootPPAObligation);
            
            $("#_PPADifference").val(e.RootPPADifference);
        }

        GridClickIdentifier = 0;
        dfrdPPAComputation.resolve();
    });
    return dfrdPPAComputation.promise();
}

function setPPAID() {
    
    var PPAID = $("#PPA").val();
    console.log("PPA: " + PPAID);
    var OBRNo = $("#PPATransOBRNo").val();
    var AccountID = 2861;
    var TransactionYear = $("#Years").val();
    var url = setPPAOBRURL();
    var UserTypeID = TypeID();
    console.log("UserTypeID:"+UserTypeID)
    var PPASetterID = $("#_ChangePPAChecker").val();
    console.log("PPASetterID: "+PPASetterID)
    if (PPASetterID == 0) {
        if (UserTypeID != 1) {

            var TransactionNo = $("#TransactionNo").val();
            var str = TransactionNo.split("-");
            
            if(str[0] != "Ref"){
                if (AccountID == 2861) {
                    $.get(url, { PPAID: PPAID, OBRNo: OBRNo, TransactionYear: TransactionYear, AccountID: AccountID }, function (e) {
                        //console.log("OBRNo: " + e);
                        //if (e == null || e == '') {
                        //    swal("Warning", "PPA Account doesn't have any Function Code assign. Cannot proceed to transaction.", "warning")
                        //} else {
                            
                            if ($("#TransactionNo").val() != "") {
                                $("#PPATransOBRNo").val(e);
                            }
                            setAllotment();
                            $("#_ChangePPAChecker").val(1);
                        //}

                    });
                }
                console.log(1);
            } else {
                setAllotment();
                console.log(2);
               // $("#_ChangePPAChecker").val(1); 
            }
        } else {
            console.log(3);
            setAllotment();
            $("#_ChangePPAChecker").val(1);
        }
    }else if(PPASetterID == 1){
        console.log(4);
        setAllotment();
    }
    
}

function setAllotment() {
    var RootPPA = $("#RootPPA").val();
    var Year = $("#Years").val();
    //var Year = 2015; // !-- Change this one
    var url = setAllotmentURL();
    $.get(url, { Year: Year, RootPPA: RootPPA }, function (e) {
        console.log("AllotedAmount: " + e);
        $("#AllotedAmount").val(MoneyFormatWithDecimal(e));
        $("#_AllotedAmount").val(e);       
        setTimeout(function () {
            setObligation();
        }, 300);
        
    });
}

function setObligation() {
    var RootPPA = $("#RootPPA").val();
    var Year = $("#Years").val();
    var url = setObligationURL();
    $.get(url, { Year: Year, RootPPA: RootPPA }, function (e) {
        $("#Obligate").val(MoneyFormatWithDecimal(e));
        $("#_Obligate").val(e);
        setTimeout(function () {
            setRootPPARelease();
        }, 400);
        
    });
}

function setRootPPARelease() { 
    var Year = $("#Years").val();
    var RootPPA = $("#RootPPA").val();
    console.log("RootPPA: "+$("#RootPPA").val());
    console.log("PPA: "+$("#PPA").val());
    var url = setPPAReleaseURL();
    $.get(url, { Year: Year, RootPPA: RootPPA }, function (e) {
        $("#PPARelease").val(MoneyFormatWithDecimal(e));
        $("#_PPARelease").val(e);
        setTimeout(function () {
            setRootPPAObligation();
        },500);
        
    });
}

function setRootPPAObligation() {
    var Year = $("#Years").val();
    var RootPPA = $("#RootPPA").val();
    var url = getPPAObligationURL();
    $.get(url, { Year: Year, RootPPA: RootPPA }, function (e) {
        $("#PPAObligate").val(MoneyFormatWithDecimal(e));
        $("#_PPAObligate").val(e);
        setTimeout(function () {
            setPPARelease();
        },600);
        
    });

}

function setPPARelease() {
    var Year = $("#Years").val();
    var RootPPA = $("#PPA").val();
    var url = getPPAReleaseURL();
    $.get(url, { Year: Year, RootPPA: RootPPA }, function (e) {
        $("#_PPAAllotment").val(e);
        setTimeout(function () {
            setPPAObligation();
        }, 700);
        
    });
}

function setPPAObligation() {
    var Year = $("#Years").val();
    var RootPPA = $("#PPA").val();
    var url = getPPAObligateURL();
    $.get(url, { Year: Year, RootPPA: RootPPA }, function (e) {
        $("#_PPAObligation").val(e);
        setTimeout(function () {
            PPAComputation();
        },800);
        
    });
}

function PPAComputation() {

    var PPARelease = $("#_PPARelease").val();
    var PPAObligate = $("#_PPAObligate").val();
    var PPAAllotment = $("#_PPAAllotment").val();
    var PPAObligation = $("#_PPAObligation").val();

    var PPADifference = Number(PPARelease) - Number(PPAObligate);
    
    $("#_PPADifference").val(PPADifference);

    var PPAAvailable = Number(PPAAllotment) - Number(PPAObligation);
    var TransactionNo = $("#TransactionNo").val();;
    console.log("TransactionNo: "+TransactionNo.length);
    if (TransactionNo.length != 0) {
        var TransactionValue = $("#TransactionNo").val();
        var str = TransactionValue.split("-");
        var _onGridClickIdentifier = $("#_onGridClickIdentifier").val();
       // console.log("str[0]: " + str[0]);
       // console.log("_ChangePPAChecker: " + _onGridClickIdentifier);

        var url = CheckIfAirMarkURL();
        $.get(url, { ControlNo: TransactionValue }, function (e) {
            console.log("FromAirMark: " + e.FromAirMark);
            console.log("FromOBR: " + e.FromOBR);
            if (e.FromAirMark == 0) {
                if (e.FromOBR == 0) {
                    var str = TransactionNo.split("-");
                    if (_onGridClickIdentifier == 1 && str[0] != "20") {
                        var AmountInputted = $("#AmountInputed").val();
                        PPAAvailable = Number(PPAAvailable) + Number(AmountInputted);
                        $("#PPAvailable").val(MoneyFormatWithDecimal(PPAAvailable));
                        
                        $("#_PPAvailable").val(PPAAvailable);
                    } else if(str[0] != 'Ref'){
                        $("#PPAvailable").val(MoneyFormatWithDecimal(PPAAvailable));
                        $("#_PPAvailable").val(PPAAvailable);
                       
                    }
                    
                    sync();
                } else {
                    console.log("blah3");
                    var _Amount = $("#_Amount").val();
                    $("#PPAvailable").val(_Amount);
                    
                    $("#_PPAvailable").val(_Amount);
                    
                    sync();
                }
            } else {
                console.log("blah4");
                    var _Amount = $("#_Amount").val();
                    $("#PPAvailable").val(_Amount);
                    
                    $("#_PPAvailable").val(_Amount);
                    console.log("_PPAvailable0: " + _Amount);
                    
                    sync();
                
            }
        });

        //if (str[0] != "Ref" && _onGridClickIdentifier == 1) {
        //    //var url = CheckIfRef 
        //    $("#PPAvailable").val(MoneyFormat(PPAAvailable));
        //    $("#_PPAvailable").val(PPAAvailable);
        //} else if (str[0] == "Ref" && _onGridClickIdentifier == 1) {
        //    var _Amount = $("#_Amount").val();
        //    $("#PPAvailable").val(_Amount);
        //    $("#_PPAvailable").val(_Amount);
        //} 
    } else {
        $("#PPAvailable").val(MoneyFormatWithDecimal(PPAAvailable));
        $("#_PPAvailable").val(PPAAvailable);
        sync();
    }
    
    
}

function SetPPA()
{
    console.log("RootPPA :"+$("#RootPPA").val());
}

function sync() {
    //console.log("ModeIndicator: " + $("#ModeIndicator").val());
    var _PPAvailable = $("#_PPAvailable").val();
    console.log("_PPAvailable1: " + _PPAvailable);
    var _DiffAllotment = $("#_DiffAllotment").val();
    var AmountInputted = $("#AmountInputed").val();
    var _Claim = $("#_Claim").val();
    var _Amount = $("#_Amount").val();
    var ModeIndicator = $("#ModeIndicator").val();
    
    var PPADifference = $("#_PPADifference").val();
    console.log("PPA Difference: "+PPADifference);
    var _PPARelease = $("#_PPARelease").val();
  
    if (AmountInputted != null) {
        
        if (Number(PPADifference) < 0 || Number(PPADifference) == "-2.546585164964199e-11")
        {
            swal({
                title: "Warning",
                text: "PPA Release is not enough!",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ok!",
                closeOnConfirm: true
            },
                function () {
                    $("#AmountInputed").val(
                        function (index, value) {
                            return 0;
                        })
                    sync();
                    $("#AmountInputed").blur();
                });

        }
        else
        {
            if (ModeIndicator == 0){
                var Claim = Number(_Claim) + Number(AmountInputted);
                $("#Claim").val(MoneyFormatWithDecimal(Claim));
                console.log("Com1");
                var BalanceAllotment = Number(_DiffAllotment) - Number(AmountInputted);
                $("#BalanceAllotment").val(MoneyFormatWithDecimal(BalanceAllotment));
                $("#_BalanceAllotment").val(BalanceAllotment);
                console.log("_PPAvailable: " + _PPAvailable);
                var PPABalanceAllotment = Number(_PPAvailable) - Number(AmountInputted);
                $("#PPABalanceAllotment").val(MoneyFormatWithDecimal(PPABalanceAllotment));
                $("#_PPABalanceAllotment").val(PPABalanceAllotment);

                if (Number(PPABalanceAllotment) < 0) {
                    $("#PPABalanceAllotment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#ff0000" });

                    if (PPABalanceAllotment != "-2.546585164964199e-11") {
                
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
                                            $("#AmountInputed").val(
                                               function (index, value) {
                                                   return 0;
                                               })
                                            sync();
                                        });
                    }
               }
            } else if (ModeIndicator == 1) {
                
                var CurrentClaim = Number(AmountInputted) - Number(_Amount);
                
                var TotalClaim = Number(_Claim) + Number(CurrentClaim);
                $("#Claim").val(MoneyFormatWithDecimal(TotalClaim));
                
                var PPABalanceAllotment = parseFloat(Number(_PPAvailable) - Number(TotalClaim)).toFixed(2);
                $("#PPABalanceAllotment").val(MoneyFormatWithDecimal(PPABalanceAllotment));
                $("#_PPABalanceAllotment").val(PPABalanceAllotment);
                //   $("#_Claim").val(TotalClaim);
                
                
                if (Number(PPABalanceAllotment) < 0) {
                    $("#PPABalanceAllotment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#ff0000" });
                    console.log("Com2");
                    
                    if (PPABalanceAllotment != "-2.546585164964199e-11") {
                        
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
                                            $("#AmountInputed").val(
                                               function (index, value) {
                                                   return 0;
                                               })
                                            sync();
                                        });
                    }  
                }
            }
        }
    }
}

function PPAAdd() {
    var url = AddPPAControlURL();
    var formData = $("#PPACurrentControl").serialize();
    if ($("#yearendtransppa").val() == 0) {
        swal({
            title: "Warning",
            text: "The financial transaction for C.Y. " + $("#Years").val() + " was closed!",
            type: "warning",
            showCancelButton: false,
            timer: 2500,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok!",
            closeOnConfirm: true
        });
    }
    else {
        $.get(url, formData, function (e) {
            if (e.message == "success") {

                $("#PPATransOBRNo").val(e.OBRNo);
                $("#grPPAName").data("kendoGrid").dataSource.read();
                // $("#grAccountName").data("kendoGrid").dataSource.read();

                var Claim = $("#_Claim").val();
                var AmountInputed = $("#AmountInputed").val();
                var TotalClaim = Number(Claim) + Number(AmountInputed);
                $("#Claim").val(MoneyFormatWithDecimal(TotalClaim));
                $("#_Claim").val(TotalClaim);
            }
        });
    }
}

function PPASave() {
    var url = SavePPAControlURL();
    var formData = $("#PPACurrentControl").serialize();
    var Years = $("#Years").val();
    var formData_Serialize = formData + "&TransactionYear=" + Years;
   
   
    if ($("#PPAFuntionCode").val().length != 4 )
    {
        swal("Warning", "Please review your entry or check the function code!", "warning");
    }
    else if ($("#PPAOOE").val() == 0) {
        swal("Warning", "Please select an expense class!", "warning");
    }
    else {
        if (parseFloat($("#AmountInputed").val()).toFixed(2) == 0 || $("#AmountInputed").val() == null || $("#AmountInputed").val() == "" || $("#AmountInputed").val() < 0) {
            swal("Warning", "Obligation amount must not be lesser than or equal to zero!", "warning");
        }
        else if ($("#yearendtransppa").val() == 0) {
            swal({
                title: "Warning",
                text: "The financial transaction for C.Y. " + $("#Years").val() + " was closed!",
                type: "warning",
                showCancelButton: false,
                timer: 2500,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ok!",
                closeOnConfirm: true
            });
        }
        else if ($("#fcode_temp").val() == 1) {
            swal("Warning", "Please build-up function code for this ppa!", "warning");
        }
        else {
            //alert('save')
            $.get(url, formData_Serialize, function (e) {
                if (e.ReturnStatus == 1) {

                    $("#PPATransOBRNo").val(e.OBRNo);
                    $("#grPPAName").data("kendoGrid").dataSource.read();

                    $("#PPASave").attr("class", "k-button k-state-disabled");
                    $("#PPAAdd").attr("class", "k-button k-state-disabled");
                    $('#PPASave').prop('onclick', null).off('click');
                    $('#PPAAdd').prop('onclick', null).off('click');

                    $("#PPAUpdate").hide();
                    if ($("#yearendtransppa").val() == 1) {
                        $("#PPAClear").show();
                    }
                    ClearForm(1);
                }

                swal(e.MTitle, e.MBody, e.MType);
            });
        }
    }
}

function PPAUpdate() {
    var url = UpdatePPAControlURL();
    var formData = $("#PPACurrentControl").serialize();
    if ($("#yearendtransppa").val() == 0) {
        swal({
            title: "Warning",
            text: "The financial transaction for C.Y. " + $("#Years").val() + " was closed!",
            type: "warning",
            showCancelButton: false,
            timer: 2500,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok!",
            closeOnConfirm: true
        });
    }
    else if (Number($("#AmountInputed").val()) == 0)
        swal("Warning", "Sorry! Transaction can't be updated. The obligation amount must not be zero!", "warning")
    else if ($("#fcode_temp").val() == 1) {
        swal("Warning", "Please build-up function code for this ppa!", "warning");
    }
    else {
        $.get(url, formData, function (e) {
            if (e == "success") {

                swal("Success", "The Control has been Updated!", "success");

                $("#grPPAName").data("kendoGrid").dataSource.read();
                // $("#grAccountName").data("kendoGrid").dataSource.read();

                var ModeIndicator = $("#ModeIndicator").val();
                var AcctChecker = $("#_AcctChecker").val();

                //$("#PPAAdd").show();
                //$("#PPASave").show();
                //$("#PPASave").attr("class", "k-button k-state-disabled");
                //$("#PPAAdd").attr("class", "k-button k-state-disabled");
                //$('#PPASave').prop('onclick', null).off('click');
                //$('#PPAAdd').prop('onclick', null).off('click');
                //$("#PPAUpdate").hide();
                //$("#PPAClear").show();
                //$("#PPACancel").hide();
            }
        });
    }
}

function PPANameParam() {
    var TransactionNo = $("#TransactionNo").val();
    var OBRNo = "";
    var param = 0;
    var Year = $("#Years").val();
    var TransRes = TransactionNo.split("-");
    console.log(TransRes[0])
    if(TransRes[0] == "Ref"){
        OBRNo = $("#PPATransOBRNo").val();
        param = 2;
        console.log("> 1");
    }
    if(TransRes[0] == "20"){
        OBRNo = $("#TransactionNo").val();
        param = 2;
        console.log("> 2");
    } else 
    if (TransactionNo == "") {
        OBRNo = $("#PPATransOBRNo").val();
        param = 2;
        console.log("> 3");
    } else if (TransactionNo != "") {
        OBRNo = $("#PPAOBRNo").val();
        param = 1;
        console.log("> 4");
    }
    console.log("OBRNo: " + OBRNo);
    console.log("param: " + param);
    return {
        OBRNo: OBRNo,
        param: param

    }
}

function AccountNameParam() {
    var TransactionNo = $("#TransactionNo").val();
    var OBRNo = "";
    var param = 0;
    var Year = $("#Years").val();
    var TransRes = TransactionNo.split("-");
    console.log(TransRes[0])
    if (TransRes[0] == "20") {
        OBRNo = $("#TransactionNo").val();
        param = 2;
    } else
        if (TransactionNo == "") {
            OBRNo = $("#PPATransOBRNo").val();
            param = 2;
        } else if (TransactionNo != "") {
            OBRNo = $("#PPAOBRNo").val();
            param = 1;
        }
    return {
        OBRNo: OBRNo,
        param: param

    }
}
function ClearForm(Param) {



    if (Param == 1) {

    }

    if (Param == 2) {

    }

    if (Param == 3) {
        if ($("#yearendtransppa").val() == 1) {
            $("#PPAAdd").show();
            $("#PPASave").show();
            $("#PPAClear").show();
        }
        $("#PPAUpdate").hide();
        $("#PPAReturn").hide();
        $("#PPACancel").hide();

        //searchOBRDetails();
        CheckStatus();
        Transaction();
        $("#PPATransactionType").data("kendoDropDownList").value("");
        $("#PPAModeOfExpense").data("kendoDropDownList").value("");
        $("#PPAOffice").data("kendoComboBox").value("");
        $("#PPAFundType").data("kendoComboBox").value("");
        $("#PPAProgram").data("kendoComboBox").value("");
        $("#PPAObjOfExpenditure").data("kendoComboBox").value("");
        $("#RootPPA").data("kendoComboBox").value("");
        $("#PPA").data("kendoComboBox").value("");
        $("#PPAOOE").data("kendoComboBox").value("");
        $("#PPADescription").val("");
        $("#AllotedAmount").val("");
        $("#Obligate").val("");
        $("#PPARelease").val("");
        $("#PPAObligate").val("");
        $("#PPAAccountCharged").data("kendoComboBox").value("");
        $("#PPAActualAccount").data("kendoComboBox").value("");

        $("#PPAvailable").val("");
        $("#AmountInputed").val("");
        $("#PPABalanceAllotment").val("");
        $("#Claim").val("");
        $("#Remarks").val("");
        $("#CurrentAllotment").val("");
        $("#ObjectExpenditure").prop("checked", false);

    }
    $("#cttsppa").val("");
    $("#cttsppa").focus();
    $("#fcode_temp").val(0)

}

function ChangePPAsetter() {
    $("#_ChangePPAChecker").val(0);
}

function DeletePPA(SubsidyID, PPAID, PPAClaim) {
    var url = DeletePPAURL();

    swal({
        title: "Are you sure?",
        text: "This item will be deleted!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: false,
        html: true
    }, function () {
        $.get(url, { SubsidyID: SubsidyID, PPAID: PPAID }, function (e) {

            if (e == "success") {

                swal("Success", "The Item has been Deleted!", "success");
                $("#grPPAName").data("kendoGrid").dataSource.read();
             //   $("#grAccountName").data("kendoGrid").dataSource.read();

                

                $("#AmountInputed").val(0);
                var Claim = $("#_Claim").val();
                var CurrentClaim = Number(Claim) - Number(PPAClaim);
                $("#Claim").val(MoneyFormatWithDecimal(CurrentClaim));
                $("#_Claim").val(CurrentClaim);

                var PPAvailable = $("#_PPAvailable").val();
                $("#PPABalanceAllotment").val(MoneyFormatWithDecimal(PPAvailable));
                $("#_PPABalanceAllotment").val(PPAvailable);

                $("#PPA").data("kendoComboBox").value(-1);
                if ($("#yearendtransppa").val() == 1) {
                    $("#PPAAdd").show();
                    $("#PPASave").show();
                    $("#Clear").show();
                }
                
                $("#PPASave").attr("class", "k-button k-state-disabled");
                $("#PPAAdd").attr("class", "k-button k-state-disabled");
                $('#PPASave').prop('onclick', null).off('click');
                $('#PPAAdd').prop('onclick', null).off('click');
                $("#PPAUpdate").hide();
                $("#PPACancel").hide();
                $("#PPAReturn").hide();
                
            }
    });
   

    });
}

function PPAReturn()
{
    
    var url = PPAReturnURL();
    var formData = $("#PPACurrentControl").serialize();
    if ($("#yearendtransppa").val() == 0) {
        swal({
            title: "Warning",
            text: "The financial transaction for C.Y. " + $("#Years").val() + " was closed!",
            type: "warning",
            showCancelButton: false,
            timer: 2500,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok!",
            closeOnConfirm: true
        });
    }
    else {
        $.get(url, formData, function (e) {
            if (e == "success") {

                swal("Success", "Transaction Successfully Returned & Updated!", "success");
                $("#grPPAName").data("kendoGrid").dataSource.read();
                // $("#grAccountName").data("kendoGrid").dataSource.read();
                if ($("#yearendtransppa").val() == 1) {
                    $("#PPAAdd").show();
                    $("#PPASave").show();
                }

                $("#PPASave").attr("class", "k-button k-state-disabled");
                $("#PPAAdd").attr("class", "k-button k-state-disabled");
                $('#PPASave').prop('onclick', null).off('click');
                $('#PPAAdd').prop('onclick', null).off('click');
            }
        });
    }
}

function PPACancel()
{
    var url = PPACancelURL();
    var OBRNo = $("#PPAOBRNo").val();

    swal({
        title: "Are you sure?",
        text: "This transaction will be <span style='color:#ff0000'>CANCELED</span> and all the Amounts will be returned!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: false,
        html: true
    },
        function () {
            if ($("#yearendtransppa").val() == 0) {
                swal({
                    title: "Warning",
                    text: "The financial transaction for C.Y. " + $("#Years").val() + " was closed!",
                    type: "warning",
                    showCancelButton: false,
                    timer: 2500,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Ok!",
                    closeOnConfirm: true
                });
            }
            else {
                $.get(url, { OBRNo: OBRNo }, function (e) {
                    if (e == "success") {
                        swal("Success", "Transaction has been Canceled!", "success");
                        $("#grPPAName").data("kendoGrid").dataSource.read();
                        ClearForm(4);
                    } else {
                        swal("Error", e, "error");
                    }
                });
            }
        });
}

function CheckStatus() {
    var UserTypeID = TypeID();
    var GetUserID = getOfficeID();
    var Year = $("#Years").val();
    var url = SetTempOBRURL();
    
   // if (UserTypeID == 1 || GetUserID == 1 || GetUserID == 63) {
        // $("#OBRNoEntry").attr("readOnly", true);
        $("#OBRLabel").html("Temp OBR No.");
        var Year = $("#Years").val();
        $.get(url, { Year: Year }, function (e) {
            $("#OBRNoCenter").val(e);
            $("#PPATransOBRNo").val(e);
        });
        //if (GetUserID == 1 || GetUserID == 63) {
        //    //$("#Office").val(getOfficeID());
        //    $("#PPAOffice").data("kendoComboBox").value(getOfficeID());
        //    $("#OfficeID").val(getOfficeID());
        //    //$("#Office").data("kendoComboBox").enable(false);
        //}
       
  //  }
}