

    $(function () {
        $("#TransactionNo").keypress(function (e) {
            if (e.keyCode == 13) {
                searchOBRDetails();
            }
        });
        $("#RefNo").keypress(function (e) {
            if (e.keyCode == 13) {
                SearchIfReferenceExist();
            }
        });
      
        $("#cttsno").keyup(function (event) {
            var str = $("#cttsno").val();// "aoms=4920dfesfddg"
            var str2 = str.substring(str.indexOf('=') + 1, str.length)
            $("#cttsno").val(str2);
            var url = OutgoingQR();
            $.get(url, { qr: str2 }, function (e) {
                if (e.transmode == 1) {
                    $("#radioIn").prop("checked", true)
                    $("#radiOut").prop("checked", false)
                }
                else {
                    $("#radioIn").prop("checked", false)
                    $("#radiOut").prop("checked", true)
                }
                
                if (e.type == "success") {
                    outgoing()
                    CheckOutEvent()
                    $("#TransactionModeIN").prop("checked",false)
                    $("#TransactionModeOUT").prop("checked", true)
                    $("#TransactionModeIN").val(0)
                    $("#TransactionModeOUT").val(2)
                    $("#TransactionNo").val(e.transno)
                    $("#OBRNo").val(e.OBRNo)
                    $("#OBRNowithFnCode").val(e.OBRNo)
                    $("#Amount").val((e.Amount))
                    $("#Particular").val(e.particular)
                    $("#UserInTimeStamp").val(e.datetimein)
                    $("#UserOutTimeStamp").val(e.datetimeout)
                    $("#VerifiedTimeStamp").val(e.datetimeverified)
                    $("#trnno").val(e.trnno_id)
                    $("#chkcafoa").val(1)
                    document.getElementById("chkcafoa").checked = true;
                    
                }
                else {
                    incoming()
                    CheckInEvent()
                    $("#TransactionModeIN").prop("checked", true)
                    $("#TransactionModeOUT").prop("checked", false)
                    $("#TransactionModeIN").val(1)
                    $("#TransactionModeOUT").val(0)
                   
                }
            })
        });
    });
    
    //function unlockctts() {
    //    $("#cttsno").val("");
    //    $("#cttsno").removeAttr("disabled");
    //    $("#cttsno").focus();
//}
   
    function incoming() {
        if ($("#cttsaccessid").val() == "True") {
          //  $("#cttsno").removeAttr("disabled");
            $("#cttsno").focus();
        }
        else {
            $("#RefNo").focus();
        }
        $("#TransactionModeOUT").val(0)
        $("#Save").html("<span class='fa fa-save' style='color:black'> </span> SAVE")
        ////TEMPORARY HIDE-USE only for DEMO to get reference and LINK ----START
        //if ($("#cttsenabled").val() == 1) {
        //    //generate hash code - 8 digits
        //    if ($("#TransactionModeIN").val() == 1 && $("#cttsno").val() == "") {
        //        var url = gethascode();
        //        $.get(url, function (e) {
        //            $("#cttsno").val(e);
        //            var refno_url = GetCTTSRefNo();
        //            $.get(refno_url, { control: $("#cttsno").val(), isid: 14, iskeycode: 'FD09', Particular: '', amount: '0' }, function (e) {
        //                $("#cttsno").val(e);
        //            });
        //        });
        //    }
        //}
        ////TEMPORARY HIDE-USE only for DEMO----END
    }
    function outgoing() {
        if ($("#cttsaccessid").val() == "True") {
      //      $("#cttsno").removeAttr("disabled");
            $("#cttsno").focus();
        }
        else {
            $("#RefNo").focus();
        }
        $("#TransactionModeIN").val(0)
    }
    function searchOBRDetails() {
        var TransactionNo = $("#TransactionNo").val();
        var tyear = $("#Years").val()
        var url = searchOBRData();
        $.get(url, { TransactionNo: TransactionNo,tyear:tyear}, function (e) {
            CheckOutEvent();
            $("#RefNo").val(e.ReferenceNo);
            $("#FundType").data("kendoComboBox").value(e.FundID);
            $("#OBRNo").val(e.OBRNo);
            $("#Particular").val(e.Description);
            $("#Amount").val(accounting.formatMoney(e.Amount));
            $("#Claimant").val(e.ClaimantEmployee);
            $("#trnno").val(e.id);
            $("#UserIDOut").val(e.UserIDOut);
            $("#OBRNowithFnCode").val(e.OBRNowithFnCode);
            $("#TransactionModeIN").attr('disabled', true)
            $("#TransactionModeOUT").prop("checked", true);
            $("#Account").val(e.account);
            
            $("#UserInTimeStamp").val(e.DateTimeIN);
            $("#VerifiedTimeStamp").val(e.datetimeverified);
            if (e.DateTimeOut != '') {
                $("#UserOutTimeStamp").val(e.DateTimeOut);
            }

        });

    }


    $('input:radio[name="TransactionMode"]').change(
    function () {
        if (this.checked && this.value == 1) {
            
            CheckInEvent();
            
        } else if (this.checked && this.value == 2) {
            CheckOutEvent();

        }
    })

    function CheckInEvent() {

        document.getElementById("Particular").readOnly = false;
        document.getElementById("RefNo").readOnly = false;
        document.getElementById("OBRNo").readOnly = false;
        document.getElementById("TransactionNo").readOnly = true;
        document.getElementById("mode-status").innerHTML = "IN";
        document.getElementById("mode-status").style = "color:green";
        $("#FundType").data("kendoComboBox").enable(true);
        $("#UserOutTimeStamp").val("");

        //var url = OBRUserTime();
        //$.get(url, function (e) {
        //    $("#UserInTimeStamp").val(e);
        //});

    }

    function CheckOutEvent() {
        document.getElementById("Particular").readOnly = false;
        document.getElementById("OBRNo").readOnly = false;
        document.getElementById("RefNo").readOnly = false;
        document.getElementById("TransactionNo").readOnly = false;
        document.getElementById("mode-status").innerHTML = "OUT";
        // document.getElementById("mode-status").style = "color:red";
        $("#mode-status").css("color", "red");
        $("#Save").html("<span class='fa fa-check-circle-o' style='color:green'> </span> Check Out");
        $("#FundType").data("kendoComboBox").enable(true);

        $("#RefNo").val("");
        $("#FundType").data("kendoComboBox").value("");
        $("#UserInTimeStamp").val("");

        //var url = OBRUserTime();
        //$.get(url, function (e) {
        //    $("#UserOutTimeStamp").val(e);

        //});
    }




    function GenerateOBR() {

        var FundID = $("#FundType").val();
        console.log(FundID);
        var url = "@Url.Action('GenerateOBRData', 'BudgetControl')";
        $.get(url, { FundID: FundID }, function (e) {
            console.log(e);
            console.log(e.OBRNo);
            $("#OBRNo").val(e.OBRNo);
            $("#TransactionNo").val(e.TransactionNo);
        });
    }
    function translink() {
        var logtransLink_url = LogTransLink();
        $.get(logtransLink_url, { uniquerefno: $("#cttsno").val() }, function (e) {
         window.open(e) ;
        });
    }

    function Save(id) {
     
        var stat_code = document.querySelector('input[name="TransactionMode"]:checked').value == 1 ? 77 : 80;
        var chkcafoa = $("#chkcafoa").val();
        var OfficeForward = $("#OfficeForward").val() == "" ? 0 : $("#OfficeForward").val();
        var EmployeeForward = $("#EmployeeForward").val() == "" ? 0 : $("#EmployeeForward").val()
        var otherindiv_id = $("#otherindiv_id").val()
        var approveby = $("#approveby").val() == null? 0 : $("#approveby").val()

        //if ($("#cttsaccessid").val() == "True" && $("#cttsno").val() == "" && stat_code == 77 && chkcafoa==1) {
        //    swal("Something went wrong!!!", "Please re-scan the QR CODE!", "warning")
        //    $("#cttsno").focus();
        //}
        if ($("#cttsaccessid").val() == "True" && stat_code == 77 && chkcafoa == 1) {
            swal("Something went wrong!!!", "Please re-scan the QR CODE!", "warning")
          //  $("#cttsno").focus();
        }
        else if ($("#OBRNo").val().length != 19 && $("#TransactionModeOUT").val() == 2) {
            swal("System Notice", "No OBR No. found!", "warning");
        }
        else if ($("#FundType").val() == -1 || $("#FundType").val() == "" || $("#FundType").val() == "0") {
            swal("System Notice", "Please select fund type!", "warning");
        }
        else if (OfficeForward == 0 && otherindiv_id == "" && $("#lguid").val() == 1 && document.getElementById("Save").innerText == " Check Out") {
            swal("Warning", "Before checking out and forwarding the transaction, please select an office from the list or enter the name of another individual.", "warning");
        }
        else if (approveby == 0 && document.getElementById("Save").innerText == " Check Out"){
            swal("Information","Please select an employee from the Approve By dropdown.","info")
        }
        else {
         
                $("#Save").attr("class", "k-button k-state-disabled");
                $('#Save').prop('onclick', null).off('click');

                var TransactionMode = document.querySelector('input[name="TransactionMode"]:checked').value;
                if (TransactionMode == 1) { //incoming

                    var RefNo = $("#RefNo").val();
                    if (RefNo != '') {
                        var url = SearchIfReferenceExistURL();
                        $.get(url, { RefNo: RefNo }, function (e) {
                            console.log("IfExist" + e.IfExist);
                            if (e.IfExist == 0) {

                                swal(e.type, e.message, "error")
                            } else if (e.IfExist = 1) {
                                if (e.CountRow == 1) {
                                    swal(e.type, e.message, "warning");
                                } else {
                                    SaveOBR(chkcafoa,id,EmployeeForward,otherindiv_id,approveby);
                                }
                            }

                        });
                    } else {
                        SaveOBR(chkcafoa,id,EmployeeForward,otherindiv_id,approveby);
                    }     
                } else if (TransactionMode == 2) { //outgoing
                    SaveOBR(chkcafoa,id,EmployeeForward,otherindiv_id,approveby);
                }
            
        }
    }
    
    function SaveOBR(chkcafoa, id,EmployeeForward,otherindiv_id,approveby) {
     
        var TransactionMode = document.querySelector('input[name="TransactionMode"]:checked').value;
        
      
        if (TransactionMode == 1) {


            var FundID = $("#FundType").val() == "" ? 0 : $("#FundType").val();
            var UserInTimeStamp = $("#UserInTimeStamp").val();
            var UserID = $("#UserID").val();
            var RefNo = $("#RefNo").val();
            var cttsno = $("#cttsno").val() == "" ? 0 : $("#cttsno").val();
            var TransactionNo = $("#TransactionNo").val();
            var isPastYear = 0;
            var obrno = "101-1123-20-09-0069"// $("#OBRNo").val();
            var tyear = $("#Years").val()
            var officeassign = $("#OfficeForward").val() == null || $("#OfficeForward").val() ==""? 0 : $("#OfficeForward").val();
           
            if ($("#isPastYear").is(":checked")) {
                isPastYear = 1
            } else {
                isPastYear = 0;
            }
            //if ($("#lguid").val() == 0 && cttsno == 0) {
            //    swal("Warning", "Please input the QR number in the field!", "warning")
            //}
            //else {
                var url = CheckInOBR();
                $.get(url, { FundID: FundID, UserInTimeStamp: UserInTimeStamp, UserID: UserID, RefNo: RefNo, isPastYear: isPastYear, cttsno: cttsno, obrno: obrno, chkcafoa: chkcafoa, tyear: tyear, officeassign: officeassign }, function (e) {

                    console.log(e.OBRNo);

                    if ((e.OBRNo != "" || e.TransactionNo != "") && e.TransactionNo != "-1") {
                        $("#UserInTimeStamp").val(e.UserINTimeStamp);
                        $("#OBRNo").val(e.OBRNo);
                        $("#TransactionNo").val(e.TransactionNo);
                        var data = e.TransactionNo;
                        $("#grOBR").data("kendoGrid").dataSource.read();
                        //swal({
                        //    title: "Your Transaction No is : <span style='color:green'>" + data.toString() + "</span>",
                        //    text: "OBR Series : <b>" + e.OBRNo + "</b>",
                        //    html: true,
                        //    type: "success"
                        //});

                        swal({
                            title: "Your Transaction No is : " + data.toString() + "",
                            text: "OBR Series : " + e.OBRNo + "",
                            html: true,
                            type: "success",
                            showCancelButton: false,
                            confirmButtonColor: "#007f00",
                            confirmButtonText: "Ok",
                            closeOnConfirm: true
                        },
                           function (isConfirm) {
                               if (isConfirm) {
                                   $("#Save").attr("class", "k-button");
                                   $("#Save").attr("onclick", "Save()");
                               }
                           });



                    }
                    else {
                        swal("Warning", e.OBRNo, "warning")
                    }

                });
            //}
        } else if (TransactionMode == 2) {
            if ($("#UserOutTimeStamp").val() == "") {
                var trnno = $("#trnno").val();
                var TransactionNum = $("#TransactionNo").val();
                var UserIDOut = $("#UserIDOut").val();
                var OBRNowithFnCode = $("#OBRNowithFnCode").val();
                var ObrNoASs = $("#OBRNo").val();
                EaseCheckOut(TransactionNum, UserIDOut, OBRNowithFnCode, trnno, ObrNoASs, '', id, EmployeeForward, otherindiv_id,approveby);
            }
            else {
                swal("Warning","Transaction is already checked out","warning")
            }
        }
    }

    function EaseCheckOut(TransactionNum, UserIDOut, OBRNowithFnCode, grtrnno, ObrNoASs, cttsno, id, EmployeeForward, otherindiv_id,approveby)
    {

        var str = grtrnno;
        var TransactionNo = ZeroPrefixFormat(str, 5);
        var url = CheckOutOBR();
        var urlv2 = CheckOutOBRv2();
        console.log(id)
        if(OBRNowithFnCode == 0){
            swal("System Notice", "No OBR found! Please make sure that the OBR is already proccesed and already assigned with function code", "warning");
        } else if (UserIDOut == 0) {

            swal({
                title: "Are you sure?",
                text: "Transaction <span style='color:green'> <b> #" + TransactionNum + " </b> </span> will be checked out.",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Proceed",
                closeOnConfirm: true,
                html: true
            },
            function () {
                if ($("#lguid").val() == 0) { // PGAS
             
                    if (cttsno != '') {
                      
                        $.get(url, { TransactionNo: grtrnno, OBRNowithFnCode: OBRNowithFnCode, ObrNoASs: ObrNoASs,approveby:approveby }, function (e) {
                            if (e.type == "success") {
                                $("#grOBR").data("kendoGrid").dataSource.read();
                                $("#UserOutTimeStamp").val(e.UserOutTimeStamp);
                            }
                            swal(e.title, e.message, e.type);
                        });
                    }
                    else {
                        $.get(url, { TransactionNo: grtrnno, OBRNowithFnCode: OBRNowithFnCode, ObrNoASs: ObrNoASs,approveby:approveby }, function (e) {
                            if (e.type == "success") {
                                $("#grOBR").data("kendoGrid").dataSource.read();
                                $("#UserOutTimeStamp").val(e.UserOutTimeStamp);
                            }
                            swal(e.title, e.message, e.type);
                        });
                    }
                }
                else { //other lgu's
                console.log("23")
                console.log(id)
                    if (id == 2) { //view the details
                        $.get(urlv2, { TransactionNo: grtrnno, OBRNowithFnCode: OBRNowithFnCode, ObrNoASs: ObrNoASs, EmployeeForward: EmployeeForward, otherindiv_id: otherindiv_id }, function (e) {
                            if (e.type == "success") {
                                $("#grOBR").data("kendoGrid").dataSource.read();
                                $("#UserOutTimeStamp").val(e.UserOutTimeStamp);
                            }
                            swal(e.title, e.message, e.type);
                        });
                    }
                    else { // direct checkout
                        var url2 = ViewFowardOffice();
                        $.get(url2, { trnno: grtrnno, OBRNowithFnCode: OBRNowithFnCode, ObrNoASs: ObrNoASs, EmployeeForward: EmployeeForward, otherindiv_id: otherindiv_id }, function (e) {
                            $(window).scrollTop(0);
                            $("#ForwardOffice").html(e);
                            $('#ForwardOffice').data('kendoWindow').title("<i class='fa fa-clock-o'> </i> Documents Send To").center().open();
                        })
                    }
                }
            });
        }  
        else {
            swal("Error", "Already checked out!", "error");
        }
        
    }

    function ViewDetails(TransactionNo, trnno, cttsno, office, verify_tag, officeassign, program, employeeassign, otherindividual, approveby) {
      
        var str = TransactionNo;
        var TransactionNum = ZeroPrefixFormat(str, 5); 
        var url = ViewDetailsUrl();
        console.log(TransactionNum);
        $.get(url, { Transaction: TransactionNum, trnno: trnno, cttsno: cttsno, office: office, officeassign: officeassign, program: program, verify_tag: verify_tag, employeeassign, otherindividual, approveby: approveby }, function (e) {
            $(window).scrollTop(0);
            $("#AddNewOBR").html(e);
            $('#AddNewOBR').data('kendoWindow').title("<i class='fa fa-clock-o'> </i> OBR Logger").center().open();
           // $("").disabled = true;
            $("#TransactionModeIN").attr('disabled', true)
            $("#TransactionModeOUT").prop("checked", true);
            //var url = OBRUserTime();
            //$.get(url, function (e) {
            //    $("#UserOutTimeStamp").val(e);
            //});
            $("#mode-status").text("OUT");
            $("#mode-status").css("color", "red");
            $("#Update").show();
            $("#Save").html("<span class='fa fa-check-circle-o' style='color:green'> </span> Check Out")
            $("#Clear").hide();
            searchOBRDetails();
        })
      
    }
    function Close() {
        $("#grOBR").data("kendoGrid").dataSource.read();
        $('#AddNewOBR').data('kendoWindow').close();
    }
    function ChangeTransactionYear()
    {
        $("#grOBR").data("kendoGrid").dataSource.read();
    }

    function YearParam()
    {
        var Year = $("#Years").val();
        return {
            Year: Year
        }
    }

    function Clear() {

        $("input[type=text]").not("#Years").val("");
        $("input[type=radio]").attr('checked', false);
        $("#linkbelowid").css("display", "none");
        $("#mode-status").text("IN");
        $("#Save").html("<span class='fa fa-save' style='color:black'> </span> SAVE")
        $("#radioIn").prop("checked", true)
        $("#FundType").data("kendoComboBox").value("")

    }

    function Update() {
        var TransactionNo = $("#TransactionNo").val();
        var RefNo = $("#RefNo").val();
        var FundType = $("#FundType").val();
        var Particular = $("#Particular").val();
        var Year = $("#Years").val();
        var officeassign = $("#OfficeForward").val() == null ? 0 : $("#OfficeForward").val();
        var EmployeeForward = $("#EmployeeForward").val() == null || $("#EmployeeForward").val() == "" ? 0 : $("#EmployeeForward").val()
        var otherindiv_id = $("#otherindiv_id").val()
        var approveby = $("#approveby").val() == ""? 0 : $("#approveby").val()
        //if ($("#lguid").val() == 0) { //pgas
            if (approveby != 0) {
                var url = UpdateOBRURL();
                $.get(url, { TransactionNo: TransactionNo, RefNo: RefNo, Particular: Particular, Year: Year, officeassign: officeassign, EmployeeForward: EmployeeForward, otherindiv_id: otherindiv_id, approveby: approveby }, function (e) {
                    swal(e.title, e.message, e.type);
                    $("#grOBR").data("kendoGrid").dataSource.read();
                });
            }
            else {
                swal("Information", "Please choose an employee from the Approve By dropdown.", "info")
            }
        //}
        //else {
        //    var url = UpdateOBRURL();
        //    $.get(url, { TransactionNo: TransactionNo, RefNo: RefNo, Particular: Particular, Year: Year, officeassign: officeassign, EmployeeForward: EmployeeForward, otherindiv_id: otherindiv_id, approveby: approveby }, function (e) {
        //        swal(e.title, e.message, e.type);
        //        $("#grOBR").data("kendoGrid").dataSource.read();
        //    });
        //}

    }

    function SearchIfReferenceExist()
    {
        var RefNo = $("#RefNo").val();
        var url = SearchIfReferenceExistURL();
        $.get(url, { RefNo: RefNo }, function (e) {
            console.log("IfExist"+e.IfExist);
            if (e.IfExist == 0) {                
                swal(e.type, e.message, "error")
            } else if(e.IfExist = 1){
                if (e.CountRow == 1) {
                    swal(e.type, e.message, "warning");
                } else {
                    $("#FundType").data("kendoComboBox").value(e.FundTypeID);
                    $("#FundType").data("kendoComboBox").enable(true);
                    $("#Particular").val(e.Description);
                    $("#Amount").val(accounting.formatMoney(e.Amount));
                    // Do something
                }
            }            
        });
    }

    function getoffice() {
        return {
            officeid: $("#OfficeForward").val() == "" ? 0 : $("#OfficeForward").val()
        }
    }
    function getoffice_v2() {
        return {
            officeid: $("#OfficeForwardv2").val() == "" ? 0 : $("#OfficeForwardv2").val()
        }
    }
    function lguclick() {
        if (document.getElementById("radiolgu").checked == true) {
            $("#radiolgu").val(1)
            $("#radiotherindiv").prop("checked", false)
            $("#radiotherindiv").val(0)
            $("#OfficeForward").data("kendoComboBox").value($("#OfficeForward_tmp").val())
            $("#EmployeeForward").data("kendoComboBox").value(0)
            $("#OfficeForward").data("kendoComboBox").enable(true);
            $("#EmployeeForward").data("kendoComboBox").enable(true);
            $("#otherindiv_id").val("")
            $("#otherindiv_id").prop("readonly", true)
            $("#EmployeeForward").focus()
        }
    }
    function otherclick() {
        if (document.getElementById("radiotherindiv").checked == true) {
            $("#radiotherindiv").val(1)
            $("#radiolgu").prop("checked", false)
            $("#radiolgu").val(0)
            $("#OfficeForward").data("kendoComboBox").value(0)
            $("#EmployeeForward").data("kendoComboBox").value(0)
            $("#OfficeForward").data("kendoComboBox").enable(false);
            $("#EmployeeForward").data("kendoComboBox").enable(false);
            $("#otherindiv_id").val("")
            $("#otherindiv_id").prop("readonly", false)
            $("#otherindiv_id").focus()
        }
    }
    function lguclick_v1() {
        if (document.getElementById("radiolgu_v1").checked == true) {
            $("#radiolgu_v1").val(1)
            $("#radiotherindiv_v1").prop("checked", false)
            $("#radiotherindiv_v1").val(0)
            $("#OfficeForwardv2").data("kendoComboBox").value($("#OfficeForward_tmp").val())
            $("#EmployeeForwardv2").data("kendoComboBox").value(0)
            $("#OfficeForwardv2").data("kendoComboBox").enable(true);
            $("#EmployeeForwardv2").data("kendoComboBox").enable(true);
            $("#otherindiv_idv2").val("")
            $("#otherindiv_idv2").prop("readonly", true)
            $("#EmployeeForwardv2").focus()
        }
    }
    function otherclick_v1() {
        if (document.getElementById("radiotherindiv_v1").checked == true) {
            $("#radiotherindiv_v1").val(1)
            $("#radiolgu_v1").prop("checked", false)
            $("#radiolgu_v1").val(0)
            $("#OfficeForwardv2").data("kendoComboBox").value(0)
            $("#EmployeeForwardv2").data("kendoComboBox").value(0)
            $("#OfficeForwardv2").data("kendoComboBox").enable(false);
            $("#EmployeeForwardv2").data("kendoComboBox").enable(false);
            $("#otherindiv_idv2").val("")
            $("#otherindiv_idv2").prop("readonly", false)
            $("#otherindiv_idv2").focus()
        }
    }
   

    