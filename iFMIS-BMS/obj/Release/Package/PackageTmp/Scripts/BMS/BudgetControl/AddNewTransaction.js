
var AddedItem = [];

$(function () {
    //$("#batchno").keypress(function (e) {
    //    if (e.keyCode == 13) {
    //        // $('#Display').click(function ()
  
    //        View()
    //    }
    //});
    var url = SearchTransaction()// "@Url.Action('SearchTransno', 'BudgetControl')";
   // var url = "@Url.Action('GenerateOBRData', 'BudgetControl')";
    $("#cttsEntry").keyup(function (event) {
        $.get(url, { cttsqr: $("#cttsEntry").val() }, function (e) {
            $("#OBRNoEntry").val(e);
            if (e != 0) {
                searchOBRNo()
            }
            })
        });
});

function covert_empty(str) {
    str = str.toString().replace(/,/g, "");
    parseInt(str, 20);
    return str;
}

function Transaction() {
    swal({
        title: "Proceed to next Transaction?",
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
                $("#cttsEntry").focus();
            });
        } else {
            setTimeout(function () {
                $("#AddNewTransaction").closest(".k-window-content").data("kendoWindow").close();
            }, 1000);
        }
    });
}


$(function () {
    // Updated
    $("#OBRNoEntry").change(function (e) {
        searchOBRNo();
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            searchOBRNo();
        }
    });

    $("#TEVControlNoText").focus(function () {
        // searchOBRNo();
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            refreshTEV();
        }
    });

});

function searchOBRNo() {
    // Updated
    var getControlNo = $("#OBRNoEntry").val();
    if (getControlNo == '' || getControlNo == null) {
        getControlNo = 0;
        if ($("#yearendtrans").val() == 1) {
            $("#Save").show();
            $("#Add").show();
        }
        $("#Clear").show();
        $("#Update").hide();
        $("#Cancel").hide();
        $("#Return").hide();
    }
    var urltrans = checktransyear()
    var transyear = $("#Years").val();
    if (getControlNo != "") {
        $.get(urltrans, { ControlNo: getControlNo, transyear: transyear }, function (e) {
            if (e == 1) {
                if (getControlNo != '') {

                    var res = getControlNo.substring(0, 3);
                    var Refval = getControlNo.split("-");

                    if (res == "20-") {
                        swal("Error", "Can't proceed please use the form for PPA. ", "error")
                    } else if (Refval[0] == "118") {
                        swal("Error", "Can't proceed please use the form for PPA. ", "error")
                    }
                    else if (Refval[0].toString().toUpperCase() == "REF") {
                        var url = CheckIfAirmarkExistURL();
                        $.get(url, { ControlNo: getControlNo }, function (e) {
                            if (e.ActionCode == 4) {
                                swal(e.MTitle, e.MBody, e.MType);
                            } else {
                                searchAirMark().done(function () {
                                    console.log("Done Searching Airmark....");
                                    ComputeAppropriation().done(function () {
                                        console.log("Done Computing Allotement");
                                        sync();
                                    });
                                });
                            }
                        });
                    }
                    else if (Refval[0].toUpperCase() == "COM") {
                        searchCommitment()
                    } else {
                        searchOBRTrrno().done(function () {
                            console.log("Done Searching trnnoID");
                            setTrnno();
                        });

                    }
                }
            }
            else {
                swal("Warning", "Transaction year does not match! Kindly check the year of the transaction number generated in the logger and the current control transaction year.", "warning")
            }
        })
    }
}

function searchOBRTrrno() {
    // Updated
    var getControlNo = $("#OBRNoEntry").val();
    var dfrdTrnno = $.Deferred();
    var trnnoUrl = trnnoURL();
    var Year = $("#Years").val();
    $.get(trnnoUrl, { getControlNo: getControlNo,  tyear : Year }, function (a) {
        if (a == 'Null') {
            swal("Warning", "OBRNo. Not Found!", "error");
        } else {
            $("#OBRNo").val(a);
            if (getControlNo == 0) {
                a = 0;
            }
            $("#AccntLabel").html("Account Charged Available Allotment :");          
        }
        dfrdTrnno.resolve();
    });
    return dfrdTrnno.promise();
}

function searchCommitment() {
    var office=$("#Office").val();
    var program=$("#Program").val();
    var account=$("#ObjOfExpenditure").val();
    var comctrl=$("#OBRNoEntry").val();
    var url = comurl();
    $.get(url, { comctrl:comctrl }, function (e) {
        if (e.type == "success") {
            $("#Office").data("kendoComboBox").value(e.OfficeID);
            refreshProgram();
            $("#Program").data("kendoComboBox").dataSource.read();
            $("#Program").data("kendoComboBox").value(e.ProgramID);
            $("#ObjectExpenditure").prop("checked", true);
            refreshExpenditure();
            $("#ObjOfExpenditure").data("kendoComboBox").dataSource.read();
            $("#ObjOfExpenditure").data("kendoComboBox").value(e.AccountID);
            checkEnabled();
            $("#ExpenseDescription").val(e.particular)
            $("#AmountInputed").val(e.Amount)
            $("#AmountInputedValue").val(e.Amount)
            sync()
            tmpobrno()
        
        }
        else {
            swal("Error",e.message,e.type)
        }
    })
}

function tmpobrno() {
    var UserTypeID = TypeID();
    var url = TempcomURL();
    var Year = $("#Years").val();
    if (UserTypeID == 1) {
        // $("#OBRNoEntry").attr("readOnly", true);
        $("#OBRLabel").html("Temp OBR No.");
        $.get(url, { Year: Year, param: 4 }, function (e) {
            $("#OBRNoCenter").val(e);
            $("#TransOBRNo").val(e);
            var fnccode = $("#TransOBRNo").val().split('-')
            $("#functioncode").val(fnccode[1])
        });
    }
}
function searchAirMark() {
    // Updated
    var getControlNo = $("#OBRNoEntry").val();
    var Refval = getControlNo.split("-");
    var CountUserTypeID = getOfficeID();
    var dfrdAirMark = $.Deferred();
    var getControlNo = $("#OBRNoEntry").val();
    var AirMark = AirMarkURL();
    $.get(AirMark, { getControlNo: getControlNo }, function (e) {
        if (e.ExcessAppropriationID == 0) {
            if (CountUserTypeID == 2 || CountUserTypeID == 4 || CountUserTypeID == 5) {
                
                $("#OBRNo").val(e.AROBRNo);
                $("#TransOBRNo").val(e.AROBRSeries);
                $("#OBRNoCenter").val(e.AROBRSeries);
            }
            if (e.ARIfControlExist == 0) {
                if (e.AROBRNo != 0 && e.AROBRSeries != 0) {
                    
                    $("#OBRNo").val(e.AROBRNo);
                    $("#TransOBRNo").val(e.AROBRSeries);
                    $("#OBRNoCenter").val(e.AROBRSeries);
                }
                $("#Office").data("kendoComboBox").value(e.AROffice);
                $("#OfficeID").val(e.AROffice); 
                $("#Program").data("kendoComboBox").dataSource.read();
                $("#Program").data("kendoComboBox").value(e.ARProgram);


                $("#FundType").data("kendoComboBox").value(Refval[1]);
                $("#FundTypeValue").val(Refval[1]);
                $("#FundType").data("kendoComboBox").enable(false);
                if (Refval[1] == '119') {
                    if (e.ARIncome == 0) {
                        $("#EconomicSubsidy").prop("checked", true);
                        $("#EconomicSubsidy").attr("disabled", false);
                    } else if (e.ARIncome == 1) {
                        $("#EconomicIncome").prop("checked", true);
                        $("#EconomicIncome").attr("disabled", false);
                    }
                }
                $("#ObjectExpenditure").prop("checked", true);
                $("#ExpenseClass").attr("disabled", true);

                $("#ObjOfExpenditure").data("kendoComboBox").enable(true);
                $("#ObjOfExpenditure").data("kendoComboBox").dataSource.read();
                $("#ObjOfExpenditure").data("kendoComboBox").value(e.ARAccount);

                //add on 6/28/2021 - xXx
                $("#PPANonOffice").data("kendoComboBox").dataSource.read();
                $("#PPANonOffice").data("kendoComboBox").value(e.subppa);
                PromptOfficeRemainingBalance()
                var url = NonOfficeSubAccount();
                var OfficeID = $("#Office").val();
                var ProgramID = $("#Program").val();
                var AccountID = $("#ObjOfExpenditure").val();
                var TransYear = $("#Years").val()

                //determine the account has sub-ppa
                $.get(url, { ProgramID: ProgramID, AccountID: AccountID, TransYear: TransYear, status: 0 }, function (e) {
                    
                    $("#NonOfficeCode").val(e);
                });

                //add on 6/28/2021 - xXx

                $("#OOE").data("kendoComboBox").value(e.AROOE);

                $("#AccountCharged").data("kendoComboBox").enable(true);
                $("#AccountCharged").data("kendoComboBox").value(e.ARAccount);
                setTimeout(function () {
                    $("#AccountCharged").data("kendoComboBox").dataSource.read();
                }, 500);

                $("#ActualAmountValue").val(e.ARAccount);
                $("#AccntChargeValue").val(e.ARAccount);
                $("#ActualAccount").data("kendoComboBox").enable(true);
                $("#ActualAccount").data("kendoComboBox").dataSource.read();
                $("#ActualAccount").data("kendoComboBox").value(e.ARAccount);

                if (e.AROffice == 43) {
                    $("#PPANonOffice").data("kendoComboBox").enable(true);
                    $("#PPANonOffice").data("kendoComboBox").dataSource.read();
                    $("#PPANonOffice").data("kendoComboBox").value(e.ARActualOffice);
                }
                $("#AccntLabel").html("AR Amount:");
                $("#AccntCharge").val(MoneyFormat(e.ARAmount));
                $("#AccntChargeValue").val(e.ARAmount);
                $("#AmountInputed").val(e.ARAmount); //can't formatted to MoneyFormat, conflict in Balance Allotment(BalanceAllotmentAppointment)
                console.log($("#AmountInputed").val());
                $("#AmountInputedValue").val(e.ARAccount);

                //$("#Add").attr("class", "k-button k-state-disabled");
                //$('#Add').prop('onclick', null).off('click');
                $("#Save").attr("class", "k-button");
                $("#Save").attr("onclick", "Save()");

            } else {
                $("#grOBR1").data("kendoGrid").dataSource.read();
            }
       
            //var fnccode = $("#TransOBRNo").val().split('-')
            //$("#functioncode").val(fnccode[1])

            var urlfunction = officefunctioncode();
            $.get(urlfunction, { OfficeID: $("#Office").val(), ProgramID: $("#Program").val(), AccountID: $("#ObjOfExpenditure").val(), TransYear: $("#Years").val() }, function (e) {
                $("#functioncode").val(e);
            });

        } else {
            swal("Error", "This Reference has Excess ID please use the Excess Form.", "error");
        }

        dfrdAirMark.resolve();
    });
    return dfrdAirMark.promise();

}


function setTrnno() {
    // Updated
    
    var ControlNo = $("#OBRNo").val();
    var url = SearchOBRURL();
    $.get(url, { ControlNo: ControlNo }, function (e) {
        var getControlNo = $("#OBRNoEntry").val();
        var YearMonth = YearMonthURL();
        var YearMonthString = "";
        var res = "";
       
        if (e.OBRNo.length == 19) {
            swal("Warning", "Please be advised that this transaction number has already been used in the obligation. Any changes to the details will result in an update of the transactions. Thank you!", "warning")
        }
        $.get(YearMonth, { ControlNo: getControlNo }, function (x) {
            YearMonthString = x;
            //console.log(x.resultNonOfficeTransNo);
            //console.log(x.resultOBRSeries);
            //console.log(x.resultTransactionNo);
            //console.log(x.resultOBRNo);
            
            if (x.resultNonOfficeTransNo == 1) {
                var OBRNoFromTempOBR = OBRNoFromTempOBRURL();
           
                $.get(OBRNoFromTempOBR, { ControlNo: getControlNo }, function (a) {
                    $("#TransOBRNo").val(a);
                    $("#OBRNoCenter").val(a);
                    
                    var fnccode = $("#TransOBRNo").val().split('-')
                    $("#functioncode").val(fnccode[1])
                    e = a;
                    res = e.OBRNo.split("-");
                    // Start - Set FundType Value
                    $("#FundType").data("kendoComboBox").value(res[0]);
                    $("#FundTypeValue").val(res[0]);
                    $("#FundType").data("kendoComboBox").enable(true);
                    $("#_RefIdentifer").val(1);
                    
                })

            } else if (x.resultOBRSeries == 1 || x.resultTransactionNo == 1 || x.resultOBRNo == 1) {
          
                $("#TransOBRNo").val(e.OBRNo);
                $("#OBRNoCenter").val(e.OBRNo);
                    
                    var fnccode = $("#TransOBRNo").val().split('-')
                    $("#functioncode").val(fnccode[1])
                    res = e.OBRNo.split("-");
                    // Start - Set FundType Value
                    $("#FundType").data("kendoComboBox").value(res[0]);
                    $("#FundTypeValue").val(res[0]);
                    $("#FundType").data("kendoComboBox").enable(true);
                    $("#_RefIdentifer").val(0);
            }
        });
         
        // Display : Search Account if Obligated || Search Raw Data if not yet Saved
        $("#grOBR1").data("kendoGrid").dataSource.read();
        var dataSource2 = $("#grOBR1").data("kendoGrid").dataSource;
        dataSource2.fetch(function () {
            var countGrid = dataSource2.total();
            // Count return row data if 0 = New else EDIT
            if (countGrid >= 2) {
                $("#ExpenseClass").prop("checked", true);
            //  $("#ObjectExpenditure").attr("disabled", true);
            }
        });

        //var Claim = ClaimUrl();
        //$.get(Claim, { ControlNo: ControlNo }, function (e) {
        //    if (e == 0) {
        //        $("#ClaimValue").val(0)
        //    } else {
        //        $("#ClaimValue").val(e)
        //        $("#Claim").val(MoneyFormat(e));
        //        //var Value = Number(e).toLocaleString('en');
        //    }
        //});
    });

    ////Add by xXx- 4/19/2018 
    //var obrno = $("#TransOBRNo").val();
  
    ////var GrossAmountUrl = GrossAmountUrl();
    //$.get(GrossAmountUrl(), { obrno: obrno }, function (b) {
    //    $("#grossAmount").val(b);
 
    //    //$("#grossAmount").val(b);
    //});

    $("#Return").hide();
    $("#Update").hide();
    $("#Cancel").hide();
    if ($("#yearendtrans").val() == 1) {
        $("#Save").show();
        $("#Add").show();
        $("#Clear").show();
    }
    $("#Save").attr("class", "k-button");
    $("#Save").attr("onclick", "Save()");
    $("#Add").attr("class", "k-button");
    $("#Add").attr("onclick", "Add()");

    var CheckIfExist = CheckIfExistUrl();
    $.get(CheckIfExist, { ControlNo: ControlNo }, function (z) {

        if (z != "0") {
            $("#Save").hide();
            if ($("#yearendtrans").val() == 1) {
                $("#Cancel").show();
            }
        }
    });
}

function EditSetAppropriation(grAmount, total, grRefNo) {
    EditSearchAllocatedAmount(grAmount, total, grRefNo);
}

function PromptIfNonOffice() {
    var Program = $("#Program").val();
    if (Program == 43) {
        PromptOfficeRemainingBalance();
    }
}

function GridClickSetGenInfo() {
    // Updated
    var dfrdGridClickSetGenInfo = $.Deferred();
    var entityGrid = $("#grOBR1").data("kendoGrid");
    var selectedItem = entityGrid.dataItem(entityGrid.select());
    $("#Office").data("kendoComboBox").value(selectedItem.grOffice);
    $("#OfficeID").val(selectedItem.grOffice)
    $("#Office").data("kendoComboBox").enable(true);
    $("#TransactionType").data("kendoDropDownList").value(selectedItem.grTransType);
    $("#TransactionType").data("kendoDropDownList").enable(true);
    $("#ModeOfExpense").data("kendoDropDownList").value(selectedItem.grModeOfExpense);
    $("#ModeOfExpense").data("kendoDropDownList").enable(true);
    $("#FundType").data("kendoComboBox").value(selectedItem.grFundCode);
    $("#FundType").data("kendoComboBox").enable(true);
    $("#FundTypeValue").val(selectedItem.grFundCode);
    $("#grtrnnoID").val(selectedItem.grtrnnoID);
    $("#ModeIndicator").val(1);
    $("#CheckRefNo").val(selectedItem.grRefNo);
    $("#SelAccountCharged").val(selectedItem.grAcctCharge)
    console.log("Check RefNo: " + $("#CheckRefNo").val());

    dfrdGridClickSetGenInfo.resolve();
    return dfrdGridClickSetGenInfo.promise();
}

function grsetProgram() {
    // Updated
    var dfrdgrsetProgram = $.Deferred();
    var entityGrid = $("#grOBR1").data("kendoGrid");
    var selectedItem = entityGrid.dataItem(entityGrid.select());

    $("#Program").data("kendoComboBox").dataSource.read();
    $("#Program").data("kendoComboBox").value(selectedItem.grProgramID);
    var total = 0;
    var data = $("#grOBR1").data("kendoGrid").dataSource._data;
    for (i = 0; i < data.length; i++) {

        total = total + Number(data[i].grAmount);
    }

    $("#OOE").data("kendoComboBox").value(selectedItem.grOOEID);
    $("#OOEText").val(selectedItem.grOOEID);
    $("#AmountInputed").val(selectedItem.grAmount);
    $("#ExpenseDescription").val(selectedItem.grDescription);

    var OfficeID = selectedItem.grOffice;
    var SubsidyIncome = selectedItem.grSubsidyIncome;
    if (OfficeID == 37 || OfficeID == 38 || OfficeID == 41) {
        if (SubsidyIncome == 0) {
            $("#EconomicSubsidy").prop("checked", true);
        } else if (SubsidyIncome == 1) {
            $("#EconomicIncome").prop("checked", true);
        }
    }

    $("#TEVControlNoText").val(selectedItem.grTEVControlNo);
    $("#TEVControlNo").data("kendoComboBox").dataSource.read();
    $("#TEVControlNo").data("kendoComboBox").value(selectedItem.greID);
    $("#PPANonOffice").data("kendoComboBox").value(selectedItem.grNonOffice);
    
  
    if ($("#PPANonOffice").val() > 0) {
        PromptOfficeRemainingBalance();
 
    }

    var dataSource2 = $("#grOBR1").data("kendoGrid").dataSource;
    dataSource2.fetch(function () {
        var countGrid = dataSource2.total();
        console.log("selectedItem.grModePayment: " + selectedItem.grModePayment);
        if (selectedItem.grModePayment == 0) {
            if (countGrid == 1) {
                $("#ObjectExpenditure").prop("checked", true);
            } else {
                $("#ExpenseClass").prop("checked", true);
                $("#ActualAccount").data("kendoComboBox").enable(true);
            }
        } else if (selectedItem.grModePayment == 1) {
            //$("#ExpenseClass").prop("checked", true);
            //$("#ObjectExpenditure").prop("checked", false);
            if (countGrid == 1) {
                $("#ObjectExpenditure").prop("checked", true);
            } else {
                $("#ExpenseClass").prop("checked", true);
            }

        } else if (selectedItem.grModePayment == 2) {

            //$("#ObjectExpenditure").prop("checked", true);
            //$("#ExpenseClass").prop("checked", false);
            $("#Add").attr("class", "k-button k-state-disabled");
            $('#Add').prop('onclick', null).off('click');

            if (countGrid == 1) {
                $("#ObjectExpenditure").prop("checked", true);
            } else {
                $("#ExpenseClass").prop("checked", true);
            }
        }

    });
    //$("#ClaimValue").val(total);
    $("#ClaimValue").val(selectedItem.grAmount);
    $("#AmountInputed").val(selectedItem.grAmount);
    $("#AmountInputedValue").val(selectedItem.grAmount);
    $("#AccntChargeValue").val(selectedItem.RefAmount);
   // $("#AccntChargeValue").val(selectedItem.grAmount);
    $("#SelectedAccount").val(selectedItem.grAcctCharge);
    $("#PastAmount").val(selectedItem.grAcctCharge);

    grsetAccount(selectedItem.grAcctCharge, selectedItem.grAcctCode, selectedItem.grModePayment, selectedItem.grRefNo, selectedItem.grAmount, selectedItem.grAmount, total).done(function () {
        console.log("Done Setting up Account Info");
        checkCurrentAllotment();
        dfrdgrsetProgram.resolve();
    });
  
    var AcctCheck = selectedItem.grAcctChecker;
    var ifExist = selectedItem.grifExist;
    var UserTypeID = TypeID();
  
    if (AcctCheck == 1 && (UserTypeID != 4 || $("#lguid").val() == 0)) {
    //if (AcctCheck == 1) {
     
        $("#Remarks").val(selectedItem.grRemarks);
     //   $("#ORNumber").val(selectedItem.grORNumber);
        $("#Claimant").val(selectedItem.grClaimant);
        $("#TransactionType").data("kendoDropDownList").enable(false);
        $("#ModeOfExpense").data("kendoDropDownList").enable(false);
        $("#Office").data("kendoComboBox").enable(false);
        $("#FundType").data("kendoComboBox").enable(false);
        $("#Program").data("kendoComboBox").enable(false);
        $("#ObjOfExpenditure").data("kendoComboBox").enable(false);
        $("#OOE").data("kendoComboBox").enable(false);
        $("#ExpenseDescription").attr("disabled", true);
        $("#AccountCharged").data("kendoComboBox").enable(true);
        $("#ActualAccount").data("kendoComboBox").enable(true);
        $("#TEVControlNo").data("kendoComboBox").enable(false);
        $("#TEVControlNoText").attr("readOnly", true);
     //   $("#Remarks").attr("disabled", true);
     //   $("#ORNumber").attr("readOnly", true);
        $("#ReturnID").val("1");

     
        $("#mode-status").html("UPDATE");
        $("#mode-status").css({ "color": "red", "text-decoration": "line-through" });
        if ($("#yearendtrans").val() == 1) {
            $("#Return").show();
            $("#Cancel").show();
            $("#Clear").show();
        }
        $("#Add").hide();
        $("#Save").hide();
        $("#Update").hide();

        //var grOBRNo = selectedItem.grOBRNo;
        //var url = CheckStatusURL();
        //$.get(url, { OBRNO: grOBRNo }, function (e) {
        swal("Transaction details can't be edited", selectedItem.grRemarks, "warning");
     
        //});
        //swal("Warning", "The OBR you entered is already processed in Accounting Office. Details can't be edited!", "warning");
    } else if (AcctCheck == 0 || (UserTypeID == 4 && $("#lguid").val() == 1)) {
        //$("#Program").data("kendoComboBox").enable(true);
        //$("#OOE").data("kendoComboBox").enable(true);
        //$("#ExpenseDescription").attr("disabled", false);
        $("#AmountInputed").attr("readOnly", false);
        //$("#AccountCharged").data("kendoComboBox").enable(true);
        $("#mode-status").html("UPDATE");
        $("#mode-status").css({ "color": "red", "text-decoration": "none" });


        var RefIdentifer = $("#_RefIdentifer").val();
        if (RefIdentifer == 1 && UserTypeID != 4) {
           
            $("#AccountCharged").data("kendoComboBox").enable(true);
            $("#Clear").hide();
            $("#Add").hide();
            if ($("#yearendtrans").val() == 1) {
                $("#Save").show();
                $("#Update").show();
                $("#Cancel").show();
            }

        } else if (RefIdentifer == 0) {
          
            $("#btnid").css({ "float": "left", "margin-left": "95px", "width": "350px", "margin-top": "-15px" });
            if ($("#yearendtrans").val() == 1) {
                $("#Save").show();
                $("#Update").show();
                $("#Cancel").show();
                $("#Clear").show();
                $("#Add").show();
            }
            if (UserTypeID == 4 && $("#lguid").val() == 1 && AcctCheck == 1) {
                $("#Remarks").val(selectedItem.grRemarks);
                swal("The transaction has already been forwarded to accounting and can only be edited by a specific user.", selectedItem.grRemarks, "warning");
            }
        }

    }

    if (selectedItem.grOffice == 43) {
        $("#PPANonOffice").data("kendoComboBox").enable(true);
        $("#ExpenseClass").attr("disabled", true);
    }
    return dfrdgrsetProgram.promise();
}

function grsetAccount(grAcctCharge, grAcctCode, grModeOfPAyment, grRefNo, grAmount, grAmount, total) {
    // Updated
    var dfrdgrsetAccount = $.Deferred();
    var entityGrid = $("#grOBR1").data("kendoGrid");
    var selectedItem = entityGrid.dataItem(entityGrid.select());

    var checked = $('input[name=ModePayment]:checked').val();
    //console.log("checked: " + checked);
    $("#PastAccount").val(grAcctCharge);
    $("#ObjOfExpenditure").data("kendoComboBox").enable(true);
    $("#ObjOfExpenditure").data("kendoComboBox").dataSource.read();
    $("#ObjOfExpenditure").data("kendoComboBox").value(grAcctCharge);

    $("#AccountCharged").data("kendoComboBox").dataSource.read();
    $("#AccountCharged").data("kendoComboBox").value(grAcctCharge);
    $("#ActualAccount").data("kendoComboBox").dataSource.read();
    $("#ActualAccount").data("kendoComboBox").value(grAcctCode);

    $("#PPANonOffice").data("kendoComboBox").dataSource.read();
    $("#ActualAmountValue").val(grAcctCode);
    if (grAcctCharge == 332 || grAcctCharge == 865) {
        $("#ExpenseClass").attr("disabled", true)
        $("#TEVControlNoText").attr("readOnly", false);
        $("#TEVControlNo").data("kendoComboBox").enable(true);

    } else if (grAcctCharge == 36241) {
        $("#SusProgram").data("kendoComboBox").enable(true);
    }

    
    /////
    EditCurrentComputation().done(function () {
        console.log("Done Edit Current Computation.");
        EditComputation().done(function () {
            console.log("Done Edit Computation.");
            dfrdgrsetAccount.resolve();
        });
        
    });
    return dfrdgrsetAccount.promise();

    //setTimeout(function () {
        
    //    //EditSetAppropriation(grAmount, total, grRefNo);
    //}, 1000)

    

}

function ObligateParam() {
    var CheckTransNo = $("#OBRNoEntry").val();
    var OBRNO = "";
    var Year = $("#Years").val();
    //console.log(OBRNO);
    var type = "";
    var param = 0;
   
    //console.log("CheckTransNo:" + CheckTransNo)
    var prefix = CheckTransNo.split('-');
    if (CheckTransNo == "" || prefix[0].toString().toUpperCase() == "REF") {
        OBRNO = $("#OBRNoCenter").val();
        
    } else {

        OBRNO = $("#OBRNo").val();
    
        //OBRNO = $("#TransOBRNo").val();
    }
    var n = OBRNO.includes("-");
    
    if (n == true) {
        type = "2";
        if (CheckTransNo == "") {
            param = 2;
            //console.log("OBRNo: " + OBRNO);
            //console.log("param: " + param);
        }else {
            param = 1;
            //console.log("OBRNo: " + OBRNO);
            //console.log("param: " + param);
        }
        
    } else {
        // false
        //console.log("OBRNo: " + OBRNO);
        type = "1";
        param = 1;
        //console.log("else param: " + param);
    }
    return {
        OBRNo: OBRNO,
        type: type,
        Year: Year,
        param: param
    }
}


function TransactionType() {
    // UPDATED
    var TransTypeID = $("#TransactionType").val();
    return {
        TransTypeID: TransTypeID
    }
}

function refreshProgram() {
    // UPDATED
    
    var OfficeID = $("#Office").val();
    var url = GetFundCodeURL();
    $.get(url, { OfficeID: OfficeID }, function (e) {
        $("#FundType").data("kendoComboBox").value(e);
        $("#FundTypeValue").val(e);
    });

    $("#OfficeID").val(OfficeID);
    var TempOBRNo = $("#OBRNoCenter").val();
   
    if (OfficeID == 43) {
        $("#econOption").hide();
        document.getElementById("TEVControlNoText").readOnly = true;
        $("#TEVControlNo").data("kendoComboBox").enable(false);
        $("#PPANonOffice").data("kendoComboBox").enable(true);
        $("#SusProgram").data("kendoComboBox").enable(false);
        $("#ObjectExpenditure").prop("checked", true);
        $("#Add").attr("class", "k-button k-state-disabled");
        $("#TransOBRNo").val(TempOBRNo);
        ClearForm(2);
    } else if (OfficeID == 37 || OfficeID == 38 || OfficeID == 41) {
     
        $("#econOption").attr("display", "inline-block");   
        $("#econOption").show();//.attr("display", "inline-block");
        //document.getElementById("econOption").dis = true;
        document.getElementById("EconomicSubsidy").disabled = false;
        document.getElementById("EconomicIncome").disabled = false;

    } else {
        $("#econOption").hide();
        $("#ObjectExpenditure").prop("checked", false);
        $("#ExpenseClass").attr("disabled", true);
        $("#Add").attr("class", "k-button");      
        ClearForm();
    }

    $("#Program").data("kendoComboBox").value("");
    $("#Program").data("kendoComboBox").dataSource.read();
}

$('input:radio[name="ModePayment"]').change(
function () {
    if (this.checked && this.value == 1) {

        $("#ObjOfExpenditure").data("kendoComboBox").value("");
        $("#ObjOfExpenditure").data("kendoComboBox").enable(true);
        refreshForm();
        $("#Add").attr("class", "k-button");
        $("#OOE").data("kendoComboBox").enable(true);
       // $("#ObjOfExpenditure").data("kendoComboBox").enable(false);
        $("#AccountCharged").data("kendoComboBox").enable(true);
        $("#ActualAccount").data("kendoComboBox").enable(true);

    } else if (this.checked && this.value == 2) {

        $("#ooename").html("Obj of Expenditure:")
        $("#ObjOfExpenditure").data("kendoComboBox").enable(true);
        $("#ObjOfExpenditure").data("kendoComboBox").dataSource.read();
        refreshForm();
        $("#Add").attr("class", "k-button k-state-disabled")
        $('#Add').prop('onclick', null).off('click');
        $("#OOE").data("kendoComboBox").enable(true);
        //$("#ObjOfExpenditure").data("kendoComboBox").enable(true);
        $("#AccountCharged").data("kendoComboBox").enable(true);
        $("#ActualAccount").data("kendoComboBox").enable(true);
    }
})

$('input:radio[name="SubsidyIncome"]').change(
    // UPDATED
    function () {
        
        if (this.checked && this.value == 1) {
            ComputeAppropriation();
        } else if (this.checked && this.value == 0) {
            ComputeAppropriation();
        }
    })
 
function getProgram() {
    // UPDATED
    var OfficeID = $("#Office").val();
    var Year = $("#Years").val();
    return {
        OfficeID: OfficeID,
        TransactionYear: Year
    }
}

function checkChecked() {
    // UPDATED
    var OfficeID = $("#Office").val();
    var Program = $("#Program").val();
    var Year = $("#Years").val();
    return {
        OfficeID: OfficeID,
        Program: Program,
        TransactionYear: Year
    }
}

function refreshForm() {
    $("#OOE").data("kendoComboBox").value("");
    $("#Appropriation").val(0);
    $("#AllotedAmount").val(0);
    $("#Obligate").val(0);
    $("#DiffObliandAllocated").val(0);
    $("#AccountCharged").data("kendoComboBox").value("");
    $("#ActualAccount").data("kendoComboBox").value("");
    console.log("Obligate: 1");
}

function configureSpecialCases() {
    // UPDATED
    var dfrdconfigureSpecialCases = $.Deferred();
    var data = $("#ObjOfExpenditure").val();
    var OfficeID = $("#OfficeID").val();
   
    //if (data == 332 || data == 865) {
    //    $("#TEVControlNoText").attr("readOnly", false);
    //    $("#TEVControlNo").data("kendoComboBox").enable(true);
    //    $("#PPANonOffice").data("kendoComboBox").enable(false);
    //    $("#SusProgram").data("kendoComboBox").enable(false);
    //} else
    if (data == 36241) {
        $("#TEVControlNoText").attr("readOnly", true);
        $("#TEVControlNo").data("kendoComboBox").enable(false);
        $("#PPANonOffice").data("kendoComboBox").enable(false);
        $("#SusProgram").data("kendoComboBox").enable(true);

    } else if (OfficeID == 43 || data == 2862) {
        $("#TEVControlNoText").attr("readOnly", true);
        $("#TEVControlNo").data("kendoComboBox").enable(false);
        $("#PPANonOffice").data("kendoComboBox").enable(true);
        $("#PPANonOffice").data("kendoComboBox").dataSource.read();
        $("#SusProgram").data("kendoComboBox").enable(false);
    }
    else {
        $("#PPANonOffice").data("kendoComboBox").dataSource.read();
    }
    dfrdconfigureSpecialCases.resolve();
    return dfrdconfigureSpecialCases.promise();
}

function ChargeAllotmentAvailable() {
    // UPDATED
    var AccountID = $("#AccountCharged").val();
    
    $("#ObjOfExpenditure").data("kendoComboBox").value(AccountID);
    $("#ActualAccount").data("kendoComboBox").value(AccountID);
    $("#ActualAmountValue").val(AccountID);
    $("#AmountInputed").val(0);
    configureSpecialCases().done(function () {
        console.log("Done Setting up Special Cases");
        changeExpenseClass().done(function () {
            console.log("Done Chaging Expense Class");
            ComputeAcctChargeAvailable().done(function () {
                console.log("Done Computing AcctCharge Available.")
            });
        });
    });
}

function ComputeAcctChargeAvailable() {
    // UPDATED
    var dfrdComputeAcctChargeAvailable = $.Deferred();
    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#AccountCharged").val();
    var OOE = $("#OOE").val();
    var SubsidyIncome = $('input[name=SubsidyIncome]:checked').val();
    var Year = $("#Years").val();
    var url = ChargeAllotmentAvailableURL();
    $.get(url, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, OOE: OOE, SubsidyIncome: SubsidyIncome, Year: Year }, function (e) {
        var AmountInputed = $("#AmountInputed").val();
        var acctbal = Number(AmountInputed) + Number(e)
        $("#AccntCharge").val(MoneyFormatWithDecimal(acctbal));
        $("#AccntChargeValue").val(acctbal);
        $("#AccntChargeValueOrig").val(acctbal);
        $("#CurrentAllotment").val(MoneyFormatWithDecimal(acctbal));
        $("#CurrentAllotmentValue").val(acctbal);
        $("#CurrentAllotmentBalance").val(acctbal);

        //if (e == 0) {
        //    $("#AmountInputed").attr("readOnly", false);
        //} else {
            $("#AmountInputed").attr("readOnly", false);
        //}
     //   var AmountInputed = $("#AmountInputed").val();
        var CurrentAccountBalance = Number($("#AccntChargeValue").val()) - Number(AmountInputed);
        $("#BalanceAllotmentAppointment").val(MoneyFormat(CurrentAccountBalance));
        $("#BalanceAllotmentAppointmentValue").val(CurrentAccountBalance);

        dfrdComputeAcctChargeAvailable.resolve();
    });
    var urlfunction = officefunctioncode();
    $.get(urlfunction, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, TransYear: Year }, function (e) {
        $("#functioncode").val(e);
    });

    var url = NonOfficeSubAccount();
    $.get(url, { ProgramID: ProgramID, AccountID: AccountID, TransYear: Year, status: 0 }, function (e) {
        $("#NonOfficeCode").val(e);
    });

    return dfrdComputeAcctChargeAvailable.promise();
   // sync();
}

function changeExpenseClass() {
    var dfrdchangeExpenseClass = $.Deferred();
    var item = $("#ObjOfExpenditure").val();
    var Program = $("#Program").val();
    var param = $('input[name=ModePayment]:checked').val();
    var Year = $("#Years").val();
    var url = changeExpenseClassURL();

    $.get(url, { item: item, param: param, Program: Program, TransactionYear: Year }, function (e) {
        
        $("#OOE").data("kendoComboBox").value(e);
        $("OOEText").val(e);
        $("#OOE").data("kendoComboBox").trigger("change");
        dfrdchangeExpenseClass.resolve();

    });
    return dfrdchangeExpenseClass.promise();
}

function ComputeAppropriation() {
    // UPDATED
    $("#Claim").val("");
    $("#BalanceAllotment").val("");
    $("#BalanceAllotmentAppointment").val("");

    var ooeid = $("#OOE").val();
    $("#OOEText").val(ooeid);

    var dfrdComputeAppropriation = $.Deferred();
    CurrentComputation().done(function () {
        var getControlNo = $("#OBRNoEntry").val();
        var Refval = getControlNo.split("-");
        if (Refval[0].toString().toUpperCase() != "REF") {
      //      $("#AmountInputed").val(""); //hide by xXx - 4/17/2018 
        }
        dfrdComputeAppropriation.resolve();
    });

    return dfrdComputeAppropriation.promise();
}

function CurrentComputation() {
    // Updated
    var dfrdCurrentComputation = $.Deferred();
    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#ObjOfExpenditure").val();
    var OOE = $("#OOE").val();
    var param = $('input[name=ModePayment]:checked').val();
    var WithSubsidiaryFlag = $('input[name=SubsidyIncome]:checked').val();
    var YearOf = $("#Years").val();
    var refno = $("#OBRNoEntry").val();
    var url = CurrentComputationURL();
    
    $.get(url, { OfficeID: OfficeID, AccountID: AccountID, ProgramID: ProgramID, YearOf: YearOf, OOE: OOE, WithSubsidiaryFlag: WithSubsidiaryFlag, param: param, refno: refno }, function (e) {
        if (e.ConnectionStatus == 0) {
            swal(e.Mtitle, e.MBody, e.MType);
        } else {
         
            $("#AllotedAmount").val(MoneyFormat(e.AllotedAmount));
            $("#AllotedAmountValue").val(e.AllotedAmount);
            $("#Appropriation").val(MoneyFormat(e.Appropriation));
            var ControlNo = $("#OBRNoEntry").val();
            if (ControlNo != '') {
                var Refval = ControlNo.split("-");
               
                if (Refval[0].toString().toUpperCase() == "REF") {
                    var AccountCharged = $("#AccntChargeValue").val();
                  
                    var RefObligate = Number(e.ObligatedAmount) - Number(AccountCharged);
                    var RefDiffObligate = Number(e.BalanceAllotment) + Number(AccountCharged);
                
                    $("#Obligate").val(MoneyFormat(RefObligate));
                    $("#ObligateValue").val(RefObligate);
                  
                    $("#DiffObliandAllocated").val(MoneyFormat(RefDiffObligate));
                    $("#DiffObliandAllocatedValue").val(RefDiffObligate);

                } else {
                    $("#Obligate").val(MoneyFormat(e.ObligatedAmount));
                    $("#ObligateValue").val(e.ObligatedAmount);
                    $("#DiffObliandAllocated").val(MoneyFormat(e.BalanceAllotment));
                    $("#DiffObliandAllocatedValue").val(e.BalanceAllotment);
                }
            } else {
                $("#Obligate").val(MoneyFormat(e.ObligatedAmount));
                $("#ObligateValue").val(e.ObligatedAmount);
                $("#DiffObliandAllocated").val(MoneyFormat(e.BalanceAllotment));
                $("#DiffObliandAllocatedValue").val(e.BalanceAllotment);
            }
        }
        dfrdCurrentComputation.resolve();
    });
    return dfrdCurrentComputation.promise();
}

function EditCurrentComputation() {
    // Updated
    var dfrdEditCurrentComputation = $.Deferred();
    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#ObjOfExpenditure").val();
    var OOE = $("#OOE").val();
    var param = $('input[name=ModePayment]:checked').val();
    var WithSubsidiaryFlag = $('input[name=SubsidyIncome]:checked').val();
    var YearOf = $("#Years").val();

    var url = CurrentComputationURL();
    $.get(url, { OfficeID: OfficeID, AccountID: AccountID, ProgramID: ProgramID, YearOf: YearOf, OOE: OOE, WithSubsidiaryFlag: WithSubsidiaryFlag, param: param }, function (e) {
        if (e.ConnectionStatus == 0) {
            swal(e.Mtitle, e.MBody, e.MType);
        } else {
            
            $("#AllotedAmount").val(MoneyFormat(e.AllotedAmount));
            $("#AllotedAmountValue").val(e.AllotedAmount);
            $("#ObligateParamValue").val(e.ObligatedAmount);
            $("#Appropriation").val(MoneyFormat(e.Appropriation));

             if (OfficeID == 37 || OfficeID == 38 || OfficeID == 41) {
     
                $("#econOption").attr("display", "inline-block");   
                $("#econOption").show();//.attr("display", "inline-block");
                document.getElementById("EconomicSubsidy").disabled = false;
                document.getElementById("EconomicIncome").disabled = false;

             }
             else {
                $("#econOption").hide();
                $("#ObjectExpenditure").prop("checked", false);
                $("#ExpenseClass").attr("disabled", true);
                $("#Add").attr("class", "k-button");      
              //  ClearForm();
            }


        }
        dfrdEditCurrentComputation.resolve();
    });
    return dfrdEditCurrentComputation.promise();
}

function setAccount() {
    // UPDATED
    var dfrdsetAccount = $.Deferred();
    $("#AccountCharged").data("kendoComboBox").dataSource.read();
    $("#ActualAccount").data("kendoComboBox").dataSource.read();
    var data = $("#ObjOfExpenditure").val();
    $("#AccountCharged").data("kendoComboBox").value(data);
    $("#ActualAccount").data("kendoComboBox").value(data);
    document.getElementById("ActualAmountValue").value = data;

    dfrdsetAccount.resolve();
    return dfrdsetAccount.promise();
 
}

function checkCurrentAllotment() {

    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#AccountCharged").val();
    var OOE = $("#OOE").val();
    var SubsidyIncome = $('input[name=SubsidyIncome]:checked').val();
    var Year = $("#Years").val();
    var url = ChargeAllotmentAvailableURL();
    $.get(url, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, OOE: OOE, SubsidyIncome: SubsidyIncome, Year: Year }, function (e) {
        $("#CurrentAllotment").val(MoneyFormat(e));
        $("#CurrentAllotmentValue").val(e);
        $("#CurrentAllotmentBalance").val(e);
    });
}

function setAppropriation() {
    SearchAllocatedAmount(); //
    SearchObligated(); //
    //DiffObliandAllocated(); //
}


function TEVparam() {
    // UPDATED
    var ControlNo = $("#TEVControlNoText").val();
    var trnnoID = $("#OBRNo").val();
    return {
        ControlNo: ControlNo,
        trnnoID: trnnoID
    }
}

function refreshTEV() {

    $("#TEVControlNo").data("kendoComboBox").value(" ");
    $("#TEVControlNo").data("kendoComboBox").dataSource.read();

    var Employee = $("#TEVControlNo").data("kendoComboBox").dataSource;
    Employee.fetch(function () {
        var countComboBox = Employee.total();
        if (countComboBox == 0) {
            swal("Warning", "No employee found in the given TO Number", "warning");
        }
    });
}

function AccountParam() {
    var ProgramID = $("#Program").val();
    var OOE = $("#OOE").val();
    var Year = $("#Years").val();
    return {
        ProgramID: ProgramID,
        OOE: OOE,
        TYear: Year
    }
}

function AccountChargedParam() {

    var ProgramID = $("#Program").val();
    var OOE = $("#OOE").val();
    var trnnoID = $("#grtrnnoID").val();
    var ModeIndicator = $("#ModeIndicator").val();
    var OBRNo = $("#OBRNoEntry").val();
    return {
        ProgramID: ProgramID,
        OOE: OOE,
        trnnoID: trnnoID,
        ModeIndicator: ModeIndicator,
        OBRNo: OBRNo
    }
}

function SearchAllocatedAmount() {
    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#ObjOfExpenditure").val();
    //console.log("AccountID: "+AccountID);
    var OOE = $("#OOE").val();
    var param = $('input[name=ModePayment]:checked').val();
    var SubsidyIncome = $('input[name=SubsidyIncome]:checked').val();
    var Year = $("#Years").val();
    var url = SearchAllocatedAmountURL();

    $.get(url, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, param: param, OOE: OOE, SubsidyIncome: SubsidyIncome, Year: Year }, function (e) {
        $("#AllotedAmount").val(MoneyFormat(e));
        $("#AllotedAmountValue").val(e);

    });
}

function EditSearchAllocatedAmount(grAmount, total, grRefNo) {
    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#ObjOfExpenditure").val();
    //console.log("AccountID: "+AccountID);
    var OOE = $("#OOE").val();
    var param = $('input[name=ModePayment]:checked').val();
    var SubsidyIncome = $('input[name=SubsidyIncome]:checked').val();
    var Year = $("#Years").val();
    var url = SearchAllocatedAmountURL();
   
    $.get(url, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, param: param, OOE: OOE, SubsidyIncome: SubsidyIncome, Year: Year }, function (e) {
        $("#AllotedAmount").val(MoneyFormat(e));
        $("#AllotedAmountValue").val(e);
        EditSearchObligated(grAmount, total, grRefNo);

    });
}

function SearchObligated() {
    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#AccountCharged").val();
    var param = $("input[name=ModePayment]:checked").val();
    var SubsidyIncome = $("input[name=SubsidyIncome]:checked").val();
    var OOE = $("#OOE").val();
    var Year = $("#Years").val();
    var url = SearchObligatedURL();
    $.get(url, { ProgramID: ProgramID, AccountID: AccountID, param: param, OOE: OOE, SubsidyIncome: SubsidyIncome, OfficeID: OfficeID, Year: Year }, function (e) {
      
        $("#Obligate").val(MoneyFormat(e));
        $("#ObligateValue").val(e);

        var ControlNo = $("#OBRNoEntry").val();
        if(ControlNo != ''){
            var Refval = ControlNo.split("-");

            if (Refval[0].toString().toUpperCase() == "REF") {
                var AccountCharged = $("#AccntChargeValue").val();
                var RefObligate = Number(e) - Number(AccountCharged);
                $("#Obligate").val(MoneyFormat(RefObligate));
                console.log("Obligate: 2");
                $("#ObligateValue").val(RefObligate);
            }
        }
        DiffObliandAllocated();
    });
}

function EditSearchObligated(grAmount, total, grRefNo) {
    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#AccountCharged").val();
    var param = $("input[name=ModePayment]:checked").val();
    var SubsidyIncome = $("input[name=SubsidyIncome]:checked").val();
    var OOE = $("#OOE").val();
    var url = SearchObligatedURL();
    var Year = $("#Years").val();
    $.get(url, { ProgramID: ProgramID, AccountID: AccountID, param: param, OOE: OOE, SubsidyIncome: SubsidyIncome, OfficeID: OfficeID, Year: Year }, function (e) {
        $("#ObligateParamValue").val(e);
        EditCompute(grRefNo);
    });
}

function DiffObliandAllocated() {

    var OfficeID = $("#OfficeID").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#AccountCharged").val();
    var param = $("input[name=ModePayment]:checked").val();
    var SubsidyIncome = $("input[name=SubsidyIncome]:checked").val();
    var OOE = $("#OOE").val();
    var Year = $("#Years").val();
    var url = DiffObliandAllocatedURL();
    $.get(url, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, param: param, OOE: OOE, SubsidyIncome: SubsidyIncome, Year: Year }, function (e) {
        $("#DiffObliandAllocated").val(MoneyFormat(e));
        $("#DiffObliandAllocatedValue").val(e);

        var ControlNo = $("#OBRNoEntry").val();
        if (ControlNo != '') {
            var Refval = ControlNo.split("-");

            if (Refval[0].toString().toUpperCase() == "REF") {
                var AccountCharged = $("#AccntChargeValue").val();
                var RefObligate = Number(e) + Number(AccountCharged);
                $("#DiffObliandAllocated").val(MoneyFormat(RefObligate));
                $("#DiffObliandAllocatedValue").val(RefObligate);
            }
        }
    });
}

function Add(type) {
    
    var url = AddRawDataURL();
    var formData = $('#ControlTransaction').serialize();
    var ooeid = $("#OOE").val()
    var AcctCharge = $("#ObjOfExpenditure").val();
    var ppanonoffice = $("#PPANonOffice").val();
        var data = $("#grOBR1").data("kendoGrid").dataSource;
        if ($("#AmountInputed").val() == 0)
        {
            swal("Warning", "Please enter the amount!", "warning")
            $("#AmountInputed").focus()
        }
        else if ($("#ExpenseDescription").val().trim() == "") {
            swal("Warning", "Please enter expense description!", "warning")
            $("#ExpenseDescription").focus()
        }
        else {
           
            if (AddedItem.indexOf(AcctCharge) == -1 || AddedItem.indexOf(ppanonoffice) == -1) {
                //AddedItem.push(AcctCharge);
                //AddedItem.push(ppanonoffice);
           
                var AmountInputed = $("#AmountInputed").val();
                var ClaimValue = $("#ClaimValue").val();
                var availapp = covert_empty($("#AccntCharge").val());
                
                if (ooeid != 1) {
                    if (document.getElementById("ObjectExpenditure").checked) {
                        ClaimValue = Number(ClaimValue) + Number(AmountInputed);
                        var balallotment = parseFloat(Number(availapp) - Number(ClaimValue)).toFixed(2)
                    }
                    else {
                        //ClaimValue = Number(ClaimValue) + Number(AmountInputed);
                        var balallotment = parseFloat(Number(availapp) - Number(AmountInputed)).toFixed(2)
                    }
                }
                else {
                    var balallotment = parseFloat(Number(availapp) - Number(AmountInputed)).toFixed(2)
                }
                
                if (balallotment >= 0) {
                  
                    AddedItem.push(AcctCharge);
                    AddedItem.push(ppanonoffice);

                    $.get(url, formData, function (e) {
                        $("#OBRNoCenter").val(e.OBRNo);

                        if ($("#yearendtrans").val() == 1) {
                            $("#Save").show();
                            $("#Add").show();
                        }
                        $("#Clear").show();
                        $("#Update").hide();
                        $("#Return").hide();
                        $("#Cancel").hide();


                        $("#AccountCharged").data("kendoComboBox").dataSource.read();
                        //var AmountInputed = $("#AmountInputed").val();
                        //var ClaimValue = $("#ClaimValue").val();
                        //ClaimValue = Number(ClaimValue) + Number(AmountInputed);
                        $("#ClaimValue").val(ClaimValue);

                        // Append on Grid
                        var AccountName = $("#ObjOfExpenditure").data("kendoComboBox").text();

                        var ProgramID = $("#Program").val();
                        var FundType = $("#FundType").val();
                        var TransType = $("#TransactionType").val();
                        var ModeOfExpense = $("#ModeOfExpense").val();
                        var OBRNo = $("#TransOBRNo").val();
                        var OOE = $("#OOE").val();
                        var Office = $("#Office").val();
                        var ExpenseDescription = $("#ExpenseDescription").val();
                        var ModePayment = $("input[name=ModePayment]:checked").val();
                        var TEV = $("#TEVControlNoText").val();
                        var eID = $("#TEVControlNo").val();
                        var NonOffice = $("#PPANonOffice").val();
                        var Remarks = "";
                        var ORNumber = 0;
                        var Claimant = "";
                        var SubsidyIncome = $("input[name=SubsidyIncome]:checked").val();
                        var RefNo = 0;
                     

                        var grid = $('#grOBR1').data('kendoGrid');
                        grid.dataSource.add({
                            grAccountName: AccountName, grAmountDummy: "₱" + MoneyFormatWithDecimal(AmountInputed), grAmount: AmountInputed, grDateTimeEntered: e.DateTimeEntered, grProgramID: ProgramID, grAcctCharge: AcctCharge, grAcctCode: AcctCharge,
                            grFundCode: FundType, grTransType: TransType, grModeOfExpense: ModeOfExpense, grOBRNo: OBRNo, grOOEID: OOE, grOffice: Office, grDescription: ExpenseDescription,
                            grAcctChecker: 0, grModePayment: ModePayment, grifExist: 0, grtrnnoID: e.trnno_id, grTEVControlNo: TEV, greID: eID, grNonOffice: NonOffice,
                            grRemarks: "Easy", grORNumber: 1, grClaimant: "Easy", grSubsidyIncome: 1, grRefNo: ""
                        });
                        $("#SumAmount").text("₱" + MoneyFormatWithDecimal(e.Amount));
                        $("#_RefIdentifer").val(0);

                    });
                }
                else {
                    swal("Insufficient balance, please check the amount you've entered!", "", "warning");
                }
            } else {
                swal("Account already exist in the list", "", "error");
            }
        }
        //(item in data) {
        //    if (data[item].grAcctCharge == AcctCharge) {
        //        swal("Account already exist in Grid", "", "error")
        //    }
        //}
        //$("#TransOBRNo").val(e.OBRNo);
        $("#btnid").css({ "float": "left", "margin-left": "123px", "width": "220px", "margin-top": "-15px" });
    
}

function Save(type) {

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

        
        $("#Save").attr("class", "k-button k-state-disabled");
        $("#Add").attr("class", "k-button k-state-disabled");
        $('#Save').prop('onclick', null).off('click');
        $('#Add').prop('onclick', null).off('click');
       // BalanceAllotment
        var Amount = $("#AmountInputed").val();
        var BalOfAllot =  covert_empty($("#BalanceAllotment").val());
        var asd = $("#BalanceAllotment").val();
      
        var ClaimValue = $("#ClaimValue").val() == "" || $("#ClaimValue").val() == "0" ? Amount : $("#ClaimValue").val();
        var controlno = $("#OBRNoEntry").val().split("-")
        //var NonOfficeTransaction = parseFloat((Number($("#_NonOfficeTransaction").val()) + Number(ClaimValue)) - Number(Amount)).toFixed(2);
        if (controlno[0].toString().toUpperCase() == "REF" || controlno[0].length == 4 || controlno[0].length == 3) {
            var NonOfficeTransaction = parseFloat((Number($("#_NonOfficeTransaction").val()) + Number(ClaimValue)) - Number(Amount)).toFixed(2);
        }
        else {
            var NonOfficeTransaction = parseFloat((Number($("#_NonOfficeTransaction").val())) - Number(Amount)).toFixed(2);
        }
        console.log("Amount Length" + Amount.length);
        var ppaNOffice = $("#PPANonOffice").val() == "" ? 0 : $("#PPANonOffice").val();
   
        console.log(Amount.length);
        console.log($("#batchno2").val());
        console.log($("#TransactionType").val());
        console.log($("#NonOfficeCode").val());
        console.log(ppaNOffice);
        console.log(BalOfAllot);
        if (Amount.length == 0 || Amount == 0) {
          
            swal({
                title: "W A R N I N G",
                text: "Please enter amount!",
                type: "warning",
                showCancelButton: false,
                timer: 1500,
                closeOnConfirm: true,
                showLoaderOnConfirm: false
            });
            $("#Add").attr("class", "k-button");
            $("#Add").attr("onclick", "Add()");
            $("#Save").attr("class", "k-button");
            $("#Save").attr("onclick", "Save()");
        }

        else if (($("#batchno2").val() == 0 || $("#batchno2").val() == " " || $("#batchno2").val() == "") && $("#TransactionType").val() == 2 && $("#PayrollAcctID").val() != 1 && $("#lguid").val() == 0)//Payroll and no batch
        {
           
                swal({
                    title: "W A R N I N G",
                    text: "Please enter PAYROLL BATCH NO.!",
                    type: "warning",
                    showCancelButton: false,
                    timer: 1500,
                    closeOnConfirm: true,
                    showLoaderOnConfirm: false
                });
                $("#Add").attr("class", "k-button");
                $("#Add").attr("onclick", "Add()");
                $("#Save").attr("class", "k-button");
                $("#Save").attr("onclick", "Save()");
           
        }
        else if ($("#NonOfficeCode").val() > ppaNOffice)//no selected sub-ppa
        {
           
            swal({
                title: "W A R N I N G",
                text: "Please select sub-ppa!",
                type: "warning",
                showCancelButton: false,
                timer: 1500,
                closeOnConfirm: true,
                showLoaderOnConfirm: false
            });
            $("#Add").attr("class", "k-button");
            $("#Add").attr("onclick", "Add()");
            $("#Save").attr("class", "k-button");
            $("#Save").attr("onclick", "Save()");
        }
        
        else if ((BalOfAllot < "0" || BalOfAllot < 0 || (Number(NonOfficeTransaction) < 0 && ppaNOffice > 0))  && $("#lguid").val() == 0)
        {
           
            swal({
                title: "Warning",
                text: "Insufficient balance for this PPA/Accounts!",// + NonOfficeTransaction,
                type: "warning",
                showCancelButton: false,
                timer:2000,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ok!",
                closeOnConfirm: true
            });
        }
        else if ($("#functioncode").val().length != 4) {
            swal({
                title: "Warning",
                text: "Please review your entry or check the function code!",
                type: "warning",
                showCancelButton: false,
                timer: 2500,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ok!",
                closeOnConfirm: true
            });
        }
        else if ($("#yearendtrans").val() == 0) {
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
            //alert('save')
            var url = SaveControlURL();
            var urltracking=TrackingUrl()
            var formData = $("#ControlTransaction").serialize();
            $.get(url, formData, function (e) {
                if (e.message == "success") {
                    //$("#Update").css({ "padding": "5px", "width": "99px", "margin-top": "-10px", "display": "none" });
                    //$("#Clear").css({ "padding": "5px", "width": "99px", "margin-top": "-10px" });
                    $("#TransOBRNo").val(e.OBRNo);
                    $("#OBRNoCenter").val(e.OBRNo);
                    var fnccode = $("#TransOBRNo").val().split('-')
                    $("#functioncode").val(fnccode[1])

                    $("#grOBR1").data("kendoGrid").dataSource.read();

                    $("#Save").attr("class", "k-button k-state-disabled");
                    $("#Add").attr("class", "k-button k-state-disabled");
                    $('#Save').prop('onclick', null).off('click');
                    $('#Add').prop('onclick', null).off('click');

                    $("#Update").hide();
                    $("#Clear").show();
                    swal("Success", "The transaction has been saved!", "success");

                    var strobr= e.OBRNo.split("-")
                    if (e.OBRNo.length == 19 && strobr[0] != "20") {
                        $.post(urltracking, { OBRNo: e.OBRNo }, function (e) {
                        })
                    }

                } else {
                    swal("Error", e, "error");
                    $("#Add").attr("class", "k-button");
                    $("#Add").attr("onclick", "Add()");
                    $("#Save").attr("class", "k-button");
                    $("#Save").attr("onclick", "Save()");
                }
            });
        }
        } else {
            $("#Add").attr("class", "k-button");
            $("#Add").attr("onclick", "Add()");
            $("#Save").attr("class", "k-button");
            $("#Save").attr("onclick", "Save()");
        }
    });

}

function Update() {
    var url = UpdateControlURL();
    var formData = $("#ControlTransaction").serialize();
    var Amount = $("#AmountInputed").val();
    var ClaimValue = $("#ClaimValue").val();
    var ppaNOffice = $("#PPANonOffice").val() == "" ? 0 : $("#PPANonOffice").val();
    var nonofficebal = $("#_NonOfficeTransaction").val() == "" ? 0 : $("#_NonOfficeTransaction").val()
    var NonOfficeTransaction = (Number(nonofficebal) + Number(ClaimValue)) - Number(Amount);
    var BalOfAllot = $("#BalanceAllotment").val();
   
    if (ppaNOffice > 0 && parseFloat(NonOfficeTransaction).toFixed(2) < 0) {
        swal("Warning", "Insufficient balance for this PPAs/Office!", "warning");
    }
    else if ($("#yearendtrans").val() == 0) {
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
                $("#grOBR1").data("kendoGrid").dataSource.read();
                swal("Success", "Transaction Successfully Updated!", "success");
                searchOBRNo();
            } else {
                swal("Error", e, "error");
            }
        });
    }
}

function Return() {
    var url = ReturnControlURL();
    var formData = $("#ControlTransaction").serialize();
    swal({
        title: "Are you sure?",
        text: "This will return the transaction! Do you want to proceed?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes",
        closeOnConfirm: false,
        html: true
    },
    function () {
        if ($("#yearendtrans").val() == 0) {
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
                if (e == "1") {
                    $("#grOBR1").data("kendoGrid").dataSource.read();
                    swal("Success", "Transaction Successfully Returned!", "success");
                    //     ClearForm(3);
                } else if (e == "0") {
                    swal("Notice", "Invalid O.R. NUMBER!!!", "warning");
                } else {
                    swal("Error", e, "error");
                }
            });
        }
    })
}

function Cancel() {
    var url = CancelControlURL();
    var formData = $("#ControlTransaction").serialize();
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
            if ($("#yearendtrans").val() == 0) {
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
                    if (e == "Success") {
                        swal("Success", "Transaction has been Canceled!", "success");
                        $("#grOBR1").data("kendoGrid").dataSource.read();
                        ClearForm(4);
                    } else {
                        swal("Error", e, "error");
                    }
                });
            }
        });
}

function Delete(TrnnoID, Amount, AcctCharge) {
    var url = DeleteControlURL();
    var OBRNo = $("#TransOBRNo").val();
    var ControlNo = $("#OBRNo").val();

    swal({
        title: "Are you sure?",
        text: "This account will be deleted from the list and it can't be undone!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: false,
        html: true
    },
        function () {
            $.get(url, { TrnnoID: TrnnoID, OBRNo: OBRNo, ControlNo: ControlNo }, function (e) {
                if (e == "success") {
                    swal("Success", "Item has been Deleted!", "success");

                    AddedItem = jQuery.grep(AddedItem, function (a) {
                        return a != AcctCharge;
                    });

                    
                    var ClaimValue = $("#ClaimValue").val();
                    var NewValue = Number(ClaimValue) - Number(Amount);
                    //console.log("New Value " + NewValue + " = " + ClaimValue + " - " + Amount);
                    $("#ClaimValue").val(NewValue);
                    $("#Claim").val(MoneyFormat(NewValue)); // minus current "Less: This claim"
                    var AllotedAmount = $("#AllotedAmount").val();


                    var AmountInputted = $("#AmountInputedValue").val();

                    var DiffAmount = Number(AmountInputted) - Number(Amount);

                    $("#AmountInputedValue").val(DiffAmount);
                    $("#Amount").val(0);
                    $("#grOBR1").data("kendoGrid").dataSource.read();
                    $("#TransactionType").data("kendoDropDownList").enable(true);
                    $("#ModeOfExpense").data("kendoDropDownList").enable(true);
                    $("#Office").data("kendoComboBox").enable(true);
                    $("#ActualAccount").data("kendoComboBox").enable(true);

                    $("#AccountCharged").data("kendoComboBox").value("");
                    $("#ActualAccount").data("kendoComboBox").value("");
                    $("#AmountInputed").val("");
                    $("#AccntCharge").val("0.00");
                    
                    if ($("#yearendtrans").val() == 1) {
                        $("#Save").show();
                        $("#Add").show();
                        $("#Add").attr("class", "k-button");
                        $("#Add").attr("onclick", "Add()");
                    }
                    $("#Clear").show();
                    $("#Update").hide();
                    $("#Cancel").hide();
                    EditCurrentComputation()
                    sync();
                    $("#BalanceAllotmentAppointment").val("0.00");
                } else {
                    swal("Error", e, "error");
                }
            });
        });
}

function ClearForm(ClearParam) {


    var check = $("#lock").is(":checked");
    $("#ExpenseClass").attr("disabled", false);
    $("#ObjectExpenditure").attr("disalbed", false);
    $("#ModeIndicator").val(0);
    $("#ObjectExpenditure").prop("checked", false);
    $("#ExpenseClass").prop("checked", false);
    $("#ObjOfExpenditure").data("kendoComboBox").value("");
    $("#TEVControlNo").data("kendoComboBox").value("");
    $("#TEVControlNo").data("kendoComboBox").enable(false);
    $("#PPANonOffice").data("kendoComboBox").value("");
    $("#SusProgram").data("kendoComboBox").value("");
    $("#OOE").data("kendoComboBox").value("");
    $("#AccountCharged").data("kendoComboBox").value("");
    $("#ActualAccount").data("kendoComboBox").value("");
    //$("#ExpenseDescription").val("");

    $("#ExpenseDescription").attr("disabled", false);
    $("#grtrnnoID").val("");
    $("#TEVControlNoText").val("");
    $("#TEVControlNoText").attr("readOnly", true);
    $("#AllotedAmount").val("");
    $("#AllotedAmountValue").val("");
    $("#DiffObliandAllocated").val("");
    $("#DiffObliandAllocatedValue").val("");
    $("#Obligate").val("");
    $("#ObligateValue").val("");
    $("#Claim").val("");
    $("#BalanceAllotment").val("");
    $("#BalanceAllotmentValue").val("");
    $("#AccntCharge").val("");
    //$("#AmountInputed").val("");
    $("#BalanceAllotmentAppointment").val("");
    $("#BalanceAllotmentAppointmentValue").val("");
    $("#CurrentAllotment").val("");
    $("#CurrentAllotmentValue").val("");
    $("#grOBR1").data("kendoGrid").dataSource.read();
    $("#Remarks").val("");
    $("#ORNumber").val("");
    $("#Claimant").val("");
    $("#Appropriation").val("");
    $("#TransOBRNo").val("");

    $("#Update").hide();
    $("#Return").hide();
    $("#Cancel").hide();
    if ($("#yearendtrans").val() == 1) {
        $("#Save").show();
        $("#Add").show();
        $("#Clear").show();
    }
    $("#Add").attr("class", "k-button");
    $("#Add").attr("onclick", "Add()");
    $("#Save").attr("class", "k-button");
    $("#Save").attr("onclick", "Save()");
    $("#mode-status").html("NEW");
    $("#mode-status").css({ "color": "green", "text-decoration": "none" });
    $("#cttsEntry").val("");
    $("#cttsEntry").focus();

    if (ClearParam == 1) {
        
        $("#TransactionType").data("kendoDropDownList").value("");
        $("#ModeOfExpense").data("kendoDropDownList").value("");
        $("#Office").data("kendoComboBox").value("");
        $("#Program").data("kendoComboBox").value("");

        $("#TransactionType").data("kendoDropDownList").enable(true);
        $("#ModeOfExpense").data("kendoDropDownList").enable(true);
        $("#Office").data("kendoComboBox").enable(true);
        $("#Program").data("kendoComboBox").enable(true);
    }
    if (ClearParam == 3) {
        $("#btnid").css({ "float": "left", "margin-left": "123px", "width": "220px", "margin-top": "-15px" });
        if (check == false) {
            
            $("#TransactionType").data("kendoDropDownList").value("");
            $("#ModeOfExpense").data("kendoDropDownList").value("");
            $("#Office").data("kendoComboBox").value("");
            $("#Program").data("kendoComboBox").value("");

            $("#TransactionType").data("kendoDropDownList").enable(true);
            $("#ModeOfExpense").data("kendoDropDownList").enable(true);
            $("#Office").data("kendoComboBox").enable(true);
            $("#Program").data("kendoComboBox").enable(true);
            searchOBRNo();

        }
        $("#OBRNoEntry").val("");
        $("#grOBR1").data("kendoGrid").dataSource.read();
        CheckStatus();
        Transaction();
        $("#ExpenseDescription").val("");
        $("#AmountInputed").val("");
        $("#batchno2").val("");
        $("#loop").val(0);

    }
    if (ClearParam == 2) {
        $("#ExpenseClass").attr("disabled", true);
    }
    if (ClearParam == 4) {
        
        $("#TransactionType").data("kendoDropDownList").value("");
        $("#ModeOfExpense").data("kendoDropDownList").value("");
        $("#Office").data("kendoComboBox").value("");
        $("#Program").data("kendoComboBox").value("");

        $("#TransactionType").data("kendoDropDownList").enable(true);
        $("#ModeOfExpense").data("kendoDropDownList").enable(true);
        $("#Office").data("kendoComboBox").enable(true);
        $("#Program").data("kendoComboBox").enable(true);

        $("#Add").attr("class", "k-button k-state-disabled");
        $('#Add').prop('onclick', null).off('click');
        $("#Save").attr("class", "k-button k-state-disabled");
        $('#Save').prop('onclick', null).off('click');
    }

}

function getAccount() {
    // UPDATED
    var ProgramID = $("#Program").val()
    var AccountID = $("#ObjOfExpenditure").val();
    var Year = $("#Years").val();
    return {
        ProgramID:ProgramID,
        AccountID: AccountID,
        Year: Year,
        Excessid:0
    }
}

function ChangeActualAmountValue() {
    var ActualAccount = $("#ActualAccount").val();
    $("#ActualAmountValue").val(ActualAccount);
}

function refreshExpenditure() {
    // UPDATED
    $("#ObjOfExpenditure").data("kendoComboBox").dataSource.read();
    $("#AccountCharged").data("kendoComboBox").dataSource.read();
    $("#ActualAccount").data("kendoComboBox").dataSource.read();
    $("#loop").val(0);

}

function checkEnabled() {
    // UPDATED
    configureSpecialCases().done(function () {
        console.log("Done Setting up Special Cases");
        changeExpenseClass().done(function () {
            console.log("Done Chaging Expense Class");
            setAccount().done(function () {
                console.log("Done Setting up Account Charge");
                ComputeAcctChargeAvailable().done(function () {
                    console.log("Done Computing AcctCharge Available.")
                });
            });

        });
    });
    
    var OfficeID = $("#Office").val();
    var ProgramID = $("#Program").val();
    var AccountID = $("#ObjOfExpenditure").val();
    
    if ($("#loop").val() == 0)
    {
        $("#loop").val(1);
        
        var url = GetExemptPayroll();

        $.get(url, { accntcode: AccountID }, function (e) {
            $("#PayrollAcctID").val(e);
        });
    }

    if (AccountID == 332 || AccountID == 865) {
        $("#ExpenseClass").attr("disabled", true)
        $("#TEVControlNoText").attr("readOnly", false);
        $("#TEVControlNo").data("kendoComboBox").enable(true);
    }
    else {
        $("#ExpenseClass").attr("disabled", false)
        $("#TEVControlNoText").attr("readOnly", true);
        $("#TEVControlNo").data("kendoComboBox").enable(false);
    }
    
    var TempOBRNo = $("#OBRNoCenter").val();
    //var url = NonOfficeFunctionCodeURL();
    $("#PPANonOffice").data("kendoComboBox").value("");
    
    var urlfunction = officefunctioncode();
    $.get(urlfunction, { OfficeID: OfficeID, ProgramID: ProgramID, AccountID: AccountID, TransYear: TransYear }, function (e) {
        $("#functioncode").val(e);
    });

    //if (OfficeID == 43) {//PromptOfficeRemainingBalance
   
        if ($("#PPANonOffice").val() != '') {
            PromptOfficeRemainingBalance();
        }

        var url = NonOfficeSubAccount();
        var TransYear = $("#Years").val()
    
        $.get(url, { ProgramID: ProgramID, AccountID: AccountID, TransYear: TransYear, status:0 }, function (e) {
            $("#NonOfficeCode").val(e);
        });

    //}
    
}

//function ExemptPayroll() {
   
//    var accntcode = $("#PayrollAcctID").val()
//    var url = GetExemptPayroll();

//    $.get(url, { accntcode: accntcode }, function (e) {

//        $("#PayrollAcctID").val(e);
//    });
    
//}
function formatnum() {
    const number = $("#OBRNoEntry").val();
 
    const formatted = `000${number}`.slice(-5); // Adds leading zeros and takes the last 3 characters

    $("#OBRNoEntry").val(formatted)
}
function sync() {

    var AccntChargeOrig = $("#AccntChargeValueOrig").val();
    var AmountInputed = $("#AmountInputed").val();
    var AmountInputedValue = $("#AmountInputedValue").val();
    var DiffValue = $("#DiffObliandAllocatedValue").val();
    var Balance = $("#BalanceAllotmentAppointment").val();
    var BalanceAllotmentAppointmentValue = $("#BalanceAllotmentAppointmentValue").val();
    var Claim = $("#Claim").val();
    var ClaimValue = $("#ClaimValue").val();
    var BalanceAllotment = $("#BalanceAllotment").val();
    var CurrentValue = $("#CurrentAllotmentBalance").val();
    var DiffAmountInputed = 0;
    var DiffCurrentAllotment = 0;
    var ModeIndicator = $("#ModeIndicator").val();
    var ProgramID = $("#Program").val();
    var PPANonOffice = $("#PPANonOffice").val();
    var AccntCharge = $("#AccntChargeValue").val();

    if (ModeIndicator == 0) { //ADd new
        var ans = parseFloat(AccntCharge).toFixed(2) - parseFloat(AmountInputed).toFixed(2);
        if (parseFloat(ans).toFixed(2) < 0) {
            //console.log(ans);
            if (Number(ans) != "-2.546585164964199e-11") {
                $("#BalanceAllotmentAppointment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#ff0000" });

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
        } else {
            //console.log("blah1");
            var OfficeID = $("#Office").val();
            var NonOfficeAccount = $("#ObjOfExpenditure").val();
            //console.log("NonOfficeAccount: " + NonOfficeAccount);
            if (OfficeID == 43) {
             
                if ($("#NonOfficeCode").val() != 0) {
                    if (NonOfficeAccount == 334 || NonOfficeAccount == 336 || NonOfficeAccount == 342 || ProgramID == 53) {
                        if (PPANonOffice != 0 || PPANonOffice != ''){
                            var TotalAllotment = $("#_NonOfficeTransaction").val();
                            //console.log(TotalAllotment);
                            var NonOfficeAns = Number(TotalAllotment) - Number(AmountInputed);
                            //console.log(NonOfficeAns);
                            if (Number(NonOfficeAns) < 0) {
                                if (Number(NonOfficeAns) < "-2.546585164964199e-11") {
                                    swal({
                                        title: "Warning",
                                        text: "Allotment not enough for this Office!",
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
            $("#BalanceAllotmentAppointment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#ff0000" });
        }
        $("#BalanceAllotmentAppointment").val(MoneyFormat(ans));
        $("#BalanceAllotmentAppointmentValue").val(ans);

        // Change Current Alssslotment 
        DiffCurrentAllotment = Number(CurrentValue) - Number(AmountInputed);
        $("#CurrentAllotment").val(MoneyFormat(DiffCurrentAllotment));
        $("#CurrentAllotmentValue").val(DiffCurrentAllotment);
        // End of Change Current Allotment

        // Change Less Claim 
        var TotalClaim = Number(ClaimValue) + Number(AmountInputed)
        $("#Claim").val(MoneyFormat(TotalClaim));

        //add xXX- 4/19/2018 - Payroll gross amount control
        
        if ($("#TransactionType").val() == 2 && $("#PayrollAcctID").val() != 1) { //payroll only / Salaries and Wages - casual and others...
            var gamount = $("#grossAmount").val();
            var difAmount = Number(gamount) - Number(TotalClaim)
    
            if (parseFloat(difAmount).toFixed(2) < 0 && Account.UserInfo.lgu == 0)//(MoneyFormat(gamount) < MoneyFormat(TotalClaim)) 
            {
                swal({
                    title: "Payroll",
                    text: "Total claim is greater than the gross amount of " + MoneyFormat($("#grossAmount").val()) + "!",
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
        //add xXX- 4/19/2018 - Payroll gross amount control

        // End Change Less Claim

        // Change Balance of Allotment
        var BalanceAllotmentValue = Number(DiffValue) - Number(TotalClaim);
        $("#BalanceAllotment").val(MoneyFormat(BalanceAllotmentValue))
        $("#BalanceAllotmentValue").val(BalanceAllotmentValue);
        // End Balance of Allotment

    }
    if (ModeIndicator == 1) {//Update
        var SelectedAccount = $("#SelectedAccount").val();
        var CurrentAccount = $("#AccountCharged").val();
        
        if (SelectedAccount != CurrentAccount) {
            
            var ans1 = parseFloat(Number(AccntCharge) - Number(AmountInputed)).toFixed(2);
            var ans = parseFloat(ans1).toFixed(2)
            if (parseFloat(ans).toFixed(2) < 0) {
              
                if (parseFloat(ans).toFixed(2)  < 0) {
                    $("#BalanceAllotmentAppointment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#ff0000" });

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
                //else if (Number(ans) == "-2.546585164964199e-11") {
                //    $("#BalanceAllotmentAppointment").val("0");
                //    $("#BalanceAllotmentAppointment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#00ff21" });
                //}

            } else {
             
                //console.log("blah");
                var OfficeID = $("#Office").val();
                var NonOfficeAccount = $("#ObjOfExpenditure").val();
                console.log("NonOfficeAccount: " + NonOfficeAccount);
                if (OfficeID == 43) {
          
                    if ($("#NonOfficeCode").val() != 0) {
                        if (NonOfficeAccount == 334 || NonOfficeAccount == 336 || NonOfficeAccount == 342 || ProgramID == 53) {
                            if (PPANonOffice != 0 || PPANonOffice != '') {
                                var TotalAllotment = $("#_NonOfficeTransaction").val();
                                var NonOfficeAns = Number(TotalAllotment) - Number(AmountInputed);
                                if (Number(NonOfficeAns) < 0) {
                                    if (Number(NonOfficeAns) < "-2.546585164964199e-11") {
                                        swal({
                                            title: "Warning",
                                            text: "Allotment not enough for this Office!",
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
                $("#BalanceAllotmentAppointment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#ff0000" });
            }
           
            $("#BalanceAllotmentAppointment").val(MoneyFormat(ans));
            $("#BalanceAllotmentAppointmentValue").val(ans);

            // Change Current Allotment 
            DiffCurrentAllotment = Number(CurrentValue) - Number(AmountInputed);
            $("#CurrentAllotment").val(MoneyFormat(DiffCurrentAllotment));
            $("#CurrentAllotmentValue").val(DiffCurrentAllotment);
            // End of Change Current Allotment

            // Change Less Claim 
            var TotalClaim = Number(ClaimValue) + Number(AmountInputed)
            $("#Claim").val(MoneyFormat(TotalClaim));

            // End Change Less Claim

            // Change Balance of Allotment
            var BalanceAllotmentValue = Number(DiffValue) - Number(TotalClaim);
            $("#BalanceAllotment").val(MoneyFormat(BalanceAllotmentValue))
            $("#BalanceAllotmentValue").val(BalanceAllotmentValue);
            // End Balance of Allotment

            //add xXX- 4/20/2018 - Payroll gross amount control
       
            if ($("#TransactionType").val() == 2 && $("#PayrollAcctID").val() != 1) { //payroll only and Salaries and Wages - casual
                var DiffClaim = Number($("#grossAmount").val() - Number(TotalClaim));

                if (Number(DiffClaim) < 0) {
                    swal({
                        title: "Payroll",
                        text: "Total claim is greater than the gross amount of " + MoneyFormat($("#grossAmount").val()) + "!",
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
            //add xXX- 4/20/2018 - Payroll gross amount control

        } else {
            //console.log("equal!");

            //EditSetAppropriation(0, 0, 0); //hide by xXx on 4/23/2018 - Cause of negative total obligation incurred
            setTimeout(function () {

                // Change the Obligate Value
            
                var CurrentObligate = $("#ObligateParamValue").val();
                
                var DiffCurrentObligate = Number(CurrentObligate) - Number(ClaimValue);
         
                $("#Obligate").val(MoneyFormat(DiffCurrentObligate));
                console.log("Obligate: 4");
                $("#ObligateValue").val(DiffCurrentObligate);
                // End of Change the Obligate Value

                // Allotment Available
                var AllotedAmount = $("#AllotedAmountValue").val();
                var CurrentAllotmentAmount = Number(AllotedAmount) - Number(DiffCurrentObligate);
                $("#DiffObliandAllocated").val(MoneyFormat(CurrentAllotmentAmount));
                $("#DiffObliandAllocatedValue").val(CurrentAllotmentAmount);
                // End of Allotment Available

                // Change Less Claim
           
                DiffAmountInputed = Number(AmountInputedValue) - Number(AmountInputed); //update on 7/9/2020 - xXx
             
                //DiffAmountInputed = parseFloat(Number(AccntChargeOrig).toFixed(2)) - Number(AmountInputed);
        
            
                var CurrentClaim = Number(ClaimValue) - Number(DiffAmountInputed);
               
                $("#Claim").val(MoneyFormat(CurrentClaim));
                // End Change Less Claim
                // Balance Of Allotment
             
                var CurrentBalance = parseFloat(Number(CurrentAllotmentAmount).toFixed(2)) - Number(CurrentClaim);
             
                $("#BalanceAllotment").val(MoneyFormat(CurrentBalance));
                $("#BalanceAllotmentValue").val(CurrentBalance);
                // End of Balance Allotment

                // Account Charged Available Allotment -- UPDATE
                var CheckRefNo = $("#CheckRefNo").val();
                //console.log("CheckRefNo: " + CheckRefNo);
                if (CheckRefNo == 0) {
                    var ChargeValue = $("#CurrentAllotmentBalance").val();
                    //console.log("AccntChargeValue: " + ChargeValue);
                    //console.log("BalanceAllotmentAppointmentValue: " + $("#BalanceAllotmentAppointmentValue").val());
                    var CurrentChargeValue = parseFloat(Number(AccntChargeOrig).toFixed(2)); //+ Number(AmountInputedValue); //temporary hide on 10/15/2021 -xxx - edit amount is already added in the balance of allotment
             //       $("#AccntCharge").val(MoneyFormat(CurrentChargeValue));
                    $("#AccntChargeValue").val(CurrentChargeValue);
                } else {
                    var CurrentChargeValue = $("#AccntChargeValue").val();
                    $("#AccntCharge").val(MoneyFormat(CurrentChargeValue));
                    $("#AccntChargeValue").val(CurrentChargeValue);
                }
                //console.log(CurrentChargeValue);
                // End of Charged Available Allotment

                // Account Balance Allotment
                var CurrentAccountBalance = Number(CurrentChargeValue) - Number(AmountInputed);
           
                //console.log("CurrentAccountBalance: " + CurrentAccountBalance);
                
                $("#BalanceAllotmentAppointment").val(MoneyFormat(CurrentAccountBalance));
                $("#BalanceAllotmentAppointmentValue").val(CurrentAccountBalance);
                // End of Account Balance Allotment

                //add xXX- 4/20/2018 - Payroll gross amount control
                if ($("#TransactionType").val() == 2 && $("#PayrollAcctID").val() != 1) { //payroll only and Salaries and Wages - casual
                    if (Number($("#grossAmount").val()) > 0) //exclude prior payroll trasactions...
                    {
                        var DiffClaim = Number($("#grossAmount").val() - Number(CurrentClaim));

                        if (Number(DiffClaim) < 0) {
                            swal({
                                title: "Payroll",
                                text: "Total claim is greater than the gross amount of " + MoneyFormat($("#grossAmount").val()) + "!",
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
                //add xXX- 4/20/2018 - Payroll gross amount control

                // IF Return 
                var ReturnID = $("ReturnID").val();
                if (ReturnID == 1) {
                    if (Number(AmountInputed) > Number(AmountInputedValue)) {
                        swal({
                            title: "Warning",
                            text: "Amount cannot exceeds from OBLIGATED AMOUNT!",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Ok!",
                            closeOnConfirm: true
                        },
                             function () {
                                 $("#AmountInputed").val(
                                    function (index, value) {
                                        return value.substr(0, value.length - 1);
                                    })
                                 sync();
                             });
                    }
                }

                // Validating Balance if < 0
                var OfficeID = $("#Office").val();
                var Progcode = $("#Program").val();
                var NonOfficeAccount = $("#ObjOfExpenditure").val();
                console.log("NonOfficeAccount: " + NonOfficeAccount); 
                if (OfficeID == 43) {
                
                    if ($("#NonOfficeCode").val() != 0) {
                        if (NonOfficeAccount == 334 || NonOfficeAccount == 336 || NonOfficeAccount == 342 || ProgramID == 53) {
                         
                            if (PPANonOffice != 0 || PPANonOffice != '') {
                                var TotalAllotment = $("#_NonOfficeTransaction").val();
                                var PastAmount = Number(TotalAllotment) + Number(Claim);
                                var NonOfficeAns = Number(PastAmount) - Number(AmountInputed);
                              
                                if (Number(NonOfficeAns) < 0) {
                                    if (Number(NonOfficeAns) < "-2.546585164964199e-11") {
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
                    else if (CurrentAccountBalance < 0) //Add by xXx on 2/23/2018
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
                                    $("#AmountInputed").val(
                                        function (index, value) {
                                            return 0;
                                        })
                                    sync();
                                });
                    }
                } else
                    if (CurrentAccountBalance < 0) {
                    if (Number(ans) != "-2.546585164964199e-11") {
                        $("#CurrentAllotment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#ff0000" });
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
                } else {
                    $("#CurrentAllotment").css({ "width": "272px", "background-color": "#ededed", "text-align": "right", "font-weight": "700", "color": "#00ff21" });
                }

                // End of Validation Balance
            }, 1000);
        }
       
    }
}

function EditComputation() {
    // Updated
    var dfrdEditComputation = $.Deferred();
    var ClaimValue = $("#ClaimValue").val();
    var AmountInputed = $("#AmountInputed").val();
    var AmountInputedValue = $("#AmountInputedValue").val();
  
    var AccntCharge = $("#AccntChargeValue").val();

    // Start - Total Current Obligation - Charge Amount
    var CurrentObligate = $("#ObligateParamValue").val();
    var DiffCurrentObligate = Number(CurrentObligate) - Number(ClaimValue);
    console.log("Obligate " + DiffCurrentObligate + " = " + CurrentObligate + " - " + ClaimValue);
    $("#Obligate").val(MoneyFormat(DiffCurrentObligate));
    console.log("Obligate: 3");
    $("#ObligateValue").val(DiffCurrentObligate);
    // End - Total Current Obligation - Charge Amount

    // Start - Current Remaining Balance
    var AllotedAmount = $("#AllotedAmountValue").val();
    var CurrentAllotmentAmount = Number(AllotedAmount) - Number(DiffCurrentObligate);
    $("#DiffObliandAllocated").val(MoneyFormat(CurrentAllotmentAmount));
    $("#DiffObliandAllocatedValue").val(CurrentAllotmentAmount);
    // End - Current Remaining Balance

    // Start Claim
    $("#Claim").val(MoneyFormat(ClaimValue));
  
    // End Claim

    // Start - Change Less Claim
    DiffAmountInputed = Number(AccntCharge) - Number(AmountInputedValue);
    var CurrentClaim = Number(ClaimValue) - Number(DiffAmountInputed);
    // End - Change Less Claim

    // Start - Total Remaining Allotment
    var CurrentBalance = Number(CurrentAllotmentAmount) - Number(CurrentClaim);
    $("#BalanceAllotment").val(MoneyFormat(CurrentBalance));
    $("#BalanceAllotmentValue").val(CurrentBalance);
    // End - Total Remaining Allotment

    var grRefNo = $("#CheckRefNo").val();
    
    if (grRefNo == 0) {
        ComputeAcctChargeAvailable()
       
        ////transfer to -function -ComputeAcctChargeAvailable - xXx- 9/3/2020 -START
        //var ChargeValue = $("#BalanceAllotmentValue").val();
        //var CurrentChargeValue = Number(Math.abs(ChargeValue)) + Number(AmountInputedValue);
        //$("#AccntCharge").val(MoneyFormat(CurrentChargeValue));
        //$("#AccntChargeValue").val(CurrentChargeValue);
        //$("#AccntChargeValueOrig").val(CurrentChargeValue);
        ////transfer to -function -ComputeAcctChargeAvailable - xXx- 9/3/2020 - END
    } else {
        var grAmount = $("#AccntChargeValue").val();
        $("#AccntCharge").val(MoneyFormat(grAmount));
        var CurrentChargeValue = grAmount;
        $("#AccntChargeValue").val(grAmount);

    }
    // End of Charged Available Allotment

    // Start - Account Balance Allotment
   
    ////transfer to -function -ComputeAcctChargeAvailable - xXx- 9/3/2020 - START
   // var CurrentAccountBalance = Number(CurrentChargeValue) - Number(AmountInputed);
    //$("#BalanceAllotmentAppointment").val(MoneyFormat(CurrentAccountBalance));
    //$("#BalanceAllotmentAppointmentValue").val(CurrentAccountBalance);
    ////transfer to -function -ComputeAcctChargeAvailable  - xXx- 9/3/2020 - END

    // End - Account Balance Allotment 

    dfrdEditComputation.resolve();
    return dfrdEditComputation.promise();
}

function EditCompute(xxxx) {
    var ClaimValue = $("#ClaimValue").val();
    var AmountInputed = $("#AmountInputed").val();
    var AmountInputedValue = $("#AmountInputedValue").val();
    var AccntCharge = $("#AccntChargeValue").val();
 
    // Change the Obligate Value
    var CurrentObligate = $("#ObligateParamValue").val();
    var DiffCurrentObligate = Number(CurrentObligate) - Number(ClaimValue);
    console.log("Obligate " + DiffCurrentObligate + " = " + CurrentObligate + " - " + ClaimValue);
   
    $("#Obligate").val(MoneyFormat(DiffCurrentObligate));
    console.log("Obligate: 3");
    $("#ObligateValue").val(DiffCurrentObligate);
    // End of Change the Obligate Value

    // Allotment Available
    var AllotedAmount = $("#AllotedAmountValue").val();
    var CurrentAllotmentAmount = Number(AllotedAmount) - Number(DiffCurrentObligate);
    //console.log("DiffObliandAllocated " + CurrentAllotmentAmount + " = " + AllotedAmount + " - " + DiffCurrentObligate);
    $("#DiffObliandAllocated").val(MoneyFormat(CurrentAllotmentAmount));
    $("#DiffObliandAllocatedValue").val(CurrentAllotmentAmount);
    // End of Allotment Available

    var total = $("#ClaimValue").val();
    $("#Claim").val(MoneyFormat(total));

    // Change Less Claim
    //DiffAmountInputed = Number(AmountInputed) - Number(AmountInputedValue); //update on 7/9/2020 - xXx
    DiffAmountInputed = Number(AccntCharge) - Number(AmountInputedValue);
    //console.log("DiffAmountInputed " + DiffAmountInputed + " = " + AmountInputedValue + " - " + AmountInputed);
    var CurrentClaim = Number(ClaimValue) - Number(DiffAmountInputed);
    //console.log("Claim " + CurrentClaim + " = " + ClaimValue + " - " + DiffAmountInputed);
    //$("#Claim").val(MoneyFormat(CurrentClaim));
    // End Change Less Claim

    // Balance Of Allotment
    var CurrentBalance = Number(CurrentAllotmentAmount) - Number(CurrentClaim);
    //console.log("BalanceAllotment " + CurrentBalance + " = " + CurrentAllotmentAmount + " - " + CurrentClaim);
    $("#BalanceAllotment").val(MoneyFormat(CurrentBalance));
    $("#BalanceAllotmentValue").val(CurrentBalance);
    // End of Balance Allotment

    // Account Charged Available Allotment -- UPDATE
    //console.log("grRefNo: " + grRefNo);
    //console.log("CheckRefNo: " + $("#CheckRefNo").val());
    var grRefNo = $("#CheckRefNo").val();
    if (grRefNo == 0) {

        var ChargeValue = $("#CurrentAllotmentBalance").val();
        var CurrentChargeValue = Number(Math.abs(ChargeValue)) + Number(AmountInputedValue);
        //console.log("AccntCharge " + CurrentChargeValue + " = " + ChargeValue + " + " + AmountInputedValue);
        $("#AccntCharge").val(MoneyFormat(CurrentChargeValue));
        $("#AccntChargeValue").val(CurrentChargeValue);

    } else {
        var grAmount = $("#AccntChargeValue").val();
        $("#AccntCharge").val(MoneyFormat(grAmount));
        var CurrentChargeValue = grAmount;
        $("#AccntChargeValue").val(grAmount);

    }


    // End of Charged Available Allotment

    // Account Balance Allotment
    var CurrentAccountBalance = Number(CurrentChargeValue) - Number(AmountInputed);
    //console.log("BalanceAllotmentAppointment " + CurrentAccountBalance + " = " + CurrentChargeValue + " - " + AmountInputed);
    
    $("#BalanceAllotmentAppointment").val(MoneyFormat(CurrentAccountBalance));
    $("#BalanceAllotmentAppointmentValue").val(CurrentAccountBalance);
}

function CheckStatus()
{
    var UserTypeID = TypeID();
    var url = SetTempOBRURL();
    var Year = $("#Years").val();
    
    if (UserTypeID == 1) {
        // $("#OBRNoEntry").attr("readOnly", true);
        
        $("#OBRLabel").html("Temp OBR No.");
        $.get(url, { Year: Year }, function (e) {
            $("#OBRNoCenter").val(e);
            $("#TransOBRNo").val(e);
            var fnccode = $("#TransOBRNo").val().split('-')
            $("#functioncode").val(fnccode[1])
        });
    }
}

function setFundType() {
    // UPDATED
    var Fund = $("#FundType").val();
    $("#FundTypeValue").val(Fund);
}

function PromptOfficeRemainingBalance() {
    // UPDATED

    var fromOffice = $("#Office").val();
    var Office = $("#PPANonOffice").val();
    var Account = $("#ObjOfExpenditure").val();
    var Program = $("#Program").val();
    var Year = $("#Years").val();
    var PPANonOffice = $("#PPANonOffice").val() == "" ? 0 : $("#PPANonOffice").val();
    var url = OfficeRemainingBalanceURL();
    var urlsub = SubAccountRemainingBal();
    var OBRNoEntry=$("#OBRNoEntry").val();
    if ($("#enableFunction").val() == "true" && $("#beginyear").val() <= Year) {
        if (PPANonOffice != 0) {
            $.get(urlsub, { Office: fromOffice, Account: Account, Program: Program, TransactionYear: Year, PPANonOffice: PPANonOffice, Excess: 0, controlno: OBRNoEntry }, function (e) {
                $("#_NonOfficeTransaction").val(e.Amount);
                swal(e.message, '', e.type)
            });
        }
    }
    else {
        if (Program == 53) {
            ComputeSPO();
        } else if (Account == 2862) {
            ComputeLDRRMF();
        } else {
            
            $.get(url, { Office: Office, Account: Account, Program: Program, TransactionYear: Year }, function (e) {
                $("#_NonOfficeTransaction").val(e.Amount);
                swal(e.message, '', e.type)
            });
        }
    }

}

function ComputeSPO() {
    var Office = $("#PPANonOffice").val();
    var Account = $("#ObjOfExpenditure").val();
    var Program = $("#Program").val();
    var Year = $("#Years").val();
    var IsIncome = $('input[name=SubsidyIncome]:checked').val();
    var SPO_ID = $("#PPANonOffice").val();
    var url = ComputeSPOURL();

    $.get(url, { ProgramID: Program, AccountID: Account, YearOF: Year, IsIncome: IsIncome, SPO_ID: SPO_ID }, function (e) {

        $("#_NonOfficeTransaction").val(e.Amount);
        swal(e.MTitle, '', e.MType)

    });
}

function ComputeLDRRMF() {
    var PPANonOffice = $("#PPANonOffice").val();
    var Year = $("#Years").val();
    var url = ComputeLDRRMFURL();
    $.get(url, { PPANonOffice: PPANonOffice, Year: Year }, function (e) {

        var AllottedAmount = e.AllotedAmount
        var ObligatedAmount = e.ObligatedAmount;
      
        $("#AllotedAmount").val(MoneyFormat(e.AllotedAmount));
        $("#AllotedAmountValue").val(e.AllotedAmount);
        $("#Obligate").val(MoneyFormat(e.ObligatedAmount));
        $("#ObligateValue").val(e.ObligatedAmount);

        var Difference = Number(AllottedAmount) - Number(ObligatedAmount);
        $("#DiffObliandAllocated").val(MoneyFormat(Difference));
        $("#DiffObliandAllocatedValue").val(Difference)

        $("#AccntCharge").val(MoneyFormat(Difference));
        $("#AccntChargeValue").val(Difference);
        $("#CurrentAllotment").val(MoneyFormat(Difference));
        $("#CurrentAllotmentValue").val(Difference);
        $("#CurrentAllotmentBalance").val(Difference);

        $("#AmountInputed").attr("readOnly", false);
    });
}

function GridClick() {
    // Updated
    GridClickSetGenInfo().done(function () {
        console.log("Done Setting up General Info");
        grsetProgram().done(function () {
            PromptIfNonOffice();
        });
    });
    //Add by xXx- 4/19/2018 
    var obrno = $("#TransOBRNo").val();
    $.get(GrossAmountUrl(), { obrno: obrno }, function (a) {
       
        $("#grossAmount").val(a);
    });

    $.get(GetPayrollBatchNo(), { obrno: obrno, tempObrno: $("#OBRNoEntry").val() }, function (a) {

        $("#batchno2").val(a);
    });

    if ($("#loop").val() == 0) {
        
        $("#loop").val(1);
        var url = GetExemptPayroll();
        
        $.get(url, { accntcode: $("#SelAccountCharged").val() }, function (e) {
            $("#PayrollAcctID").val(e);
        });
    }

}

function displayDetails(str1, str2, str3)
{
    //$("#AmountInputed").val(str1);
   
    //if (str1 == '999999') {
    if (str1.length >= 16) {
        swal({
            title: "Batch No.",
            text: "Batch number has already been used with control number " + str1 + "! Please check your entry.",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok!",
            closeOnConfirm: true
        })
        $("#grossAmount").val(0);
        $("#ExpenseDescription").val("");
        $("#batchno2").val(0);
    }
    else {
        swal({
            title: "Payroll",
            text: str2 +". Gross Amount: " + MoneyFormat(str1),
            type: "info",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok!",
            closeOnConfirm: true
        })
        $("#grossAmount").val(str1);
        $("#ExpenseDescription").val(str2);
        $("#batchno2").val(str3);
    }
}
