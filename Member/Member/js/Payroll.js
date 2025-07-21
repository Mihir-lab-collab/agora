$(document).ready(function () {
    var isactive;
    isactive = true;
    GetPayrollDetails(isactive);
});

var rowNumber = 0;

function resetRowNumber(e) {
    rowNumber = 0;
}

function renderNumber(data) {
    return ++rowNumber;
}

function fnincludeInactive(checked) {
    if (checked) {
        isactive = false;
        GetPayrollDetails(isactive);
    }
    else {
        isactive = true;
        GetPayrollDetails(isactive);
    }


}

function fncallGetSalaryDataForEmp(id) {
    if (id.toString().indexOf('lnkPrev') > 0) {
        var prevyear = parseInt($('[id$="hdnYear"]').val()) - 1;
        GetSalaryDataForEmp($('[id$="hdnEmpId"]').val(), prevyear);
    }
    else {
        var nxtyear = parseInt($('[id$="hdnYear"]').val()) + 1;
        GetSalaryDataForEmp($('[id$="hdnEmpId"]').val(), nxtyear);
    }


}
function CalculateTotalExp1(birthday) {

    var re = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/;

    if (birthday.value != '') {

        if (re.test(birthday.value)) {
            birthdayDate = new Date(birthday.value);
            dateNow = new Date();

            var years = dateNow.getFullYear() - birthdayDate.getFullYear();
            var months = dateNow.getMonth() - birthdayDate.getMonth();
            //var days = dateNow.getDate() - birthdayDate.getDate();
            if (isNaN(years)) {

                document.getElementById('lblAge').innerHTML = '';
                document.getElementById('lblError').innerHTML = 'Input date is incorrect!';
                return false;

            }

            else {
                document.getElementById('lblError').innerHTML = '';

                if (months < 0 || (months == 0 && days < 0)) {
                    years = parseInt(years) - 1;
                    document.getElementById('lblAge').innerHTML = years + ' Years '
                }
                else {
                    document.getElementById('lblAge').innerHTML = years + ' Years '
                }
            }
        }
        else {
            document.getElementById('lblError').innerHTML = 'Date must be mm/dd/yyyy format';
            return false;
        }
    }
}

//////////////////////////////     Payroll Details ///////////////////////////
function GetPayrollDetails(isactive) {

    var LocationId = $('[id$="DDLocations"]').val();

    var EmpId = 0;

    $.ajax({
        type: "POST",
        url: "PayMaster.aspx/BindPayrollDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'isActive':'" + isactive + "','locationId':'" + LocationId + "','EmpId':'" + EmpId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetPayrollData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetPayrollData(Tdata) {
    $("#grdPayroll").html(""); // added for the issue of inactive emps on page change showing active emp
    $("#grdPayroll").kendoGrid({
        height: 300,
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        EmpID: {
                            type: "number",
                            editable: false
                        },
                        EmpName: {
                            type: "string"
                        },
                        EmpJoiningDate: {
                            type: "date"
                        },
                        PastExperince: {
                            type: "number"
                        },
                        TotalExperince: {
                            type: "number"
                        },
                        RevisionDate: {
                            type: "date"
                        },
                        Gross: {
                            type: "number"
                        },
                        Bonus: {
                            type: "number"
                        },
                        MonthlyCTC: {
                            type: "number"
                        },
                        AnnualCTC: {
                            type: "number"
                        },
                        Net: {
                            type: "number"
                        },
                        PBB: {
                            type: "boolean"
                        },
                    }
                }
            },
            //pageSize: 100,
        },

        scrollable: true,
        sortable: true,
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        dataBound: resetRowNumber,
        selectable: 'row',  //selects a row on click
        columns: [{
            title: "Sr No",
            width: "40px",
            template: "#= renderNumber(data) #"
        }, {
            field: "EmpID",
            title: "ID",
            width: "50px",
        }, {
            field: "EmpName",
            title: "Name",
            width: "100px",
        }, {
            field: "EmpJoiningDate",
            title: "Joining Date",
            width: "100px",
            format: "{0:dd-MMM-yyyy}",
        }, {
            field: "TotalExperince",
            title: "Total Experience",
            width: "80px",
            //template: '#= CalculateTotalExp1("10/01/2013")#'
            template: '<div style="text-align:center;">#= TotalExperince # Years</div>'
        }, {
            field: "RevisionDate",
            title: "Revision Month",
            width: "100px",
            template: '<div style="text-align:center;">#= kendo.toString(kendo.parseDate(RevisionDate, "yyyy-MM-dd"), "MMM-yyyy") #',
            //format: "{0:MMM-yyyy}"
        }, {
            field: "AnnualCTC",
            title: "Annual CTC (INR)",
            width: "100px",
            template: '<div class="ra">#= kendo.toString(AnnualCTC,"n0") #</div>'
        }, {
            field: "MonthlyCTC",
            title: "CTC (INR)",
            width: "100px",
            template: '<div class="ra">#= kendo.toString(MonthlyCTC,"n0") #</div>'
        }, {
            field: "Bonus",
            title: "Annual Bonus",
            width: "110px",
            //template: '<div class="ra">#= kendo.toString(Bonus,"n0") #</div>'
            template: '#if(PBB) {#<div class="ra">#= kendo.toString(Bonus,"n0") # (Fixed) #} else {#<div class="ra">#= kendo.toString(Bonus,"n0") # (Performance Based) #} #</div>'
        }, {
            field: "Gross",
            title: "Gross Salary (INR)",
            width: "100px",
            template: '<div class="ra">#= kendo.toString(Gross,"n0") #</div>'
        },
        {
            field: "Net",
            title: "Net Salary (INR)",
            width: "100px",
            template: '<div class="ra">#= kendo.toString(Net,"n0") #</div>'
        },
        ],

        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    contains: "Contains",
                    eq: "Is equal to"
                }
            }
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        change: function (arg) {
            var window = $("#divSalDetails");
            openPopUP();

            var gview = $("#grdPayroll").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());
            $('[id$="hdnEmpId"]').val(selectedItem.EmpID);
            //alert($('[id$="hdnEmpId"]').val());

            var EmpName = selectedItem.EmpName;
            $('[id$="lblEmpNameID"]').html(EmpName + ' (' + selectedItem.EmpID + ')');


            GetSalDetails($('[id$="hdnEmpId"]').val());
            GetSalaryDataForEmp($('[id$="hdnEmpId"]').val(), '0');
            //showCursorPointerForSalDetails();
        },
    });
}

//////////////////////////////     Salary details (1st Popup - 1st grid) ///////////////////////////

function GetSalDetails(EmpId) {

    $.ajax({
        type: "POST",
        url: "PayMaster.aspx/BindPayrollDetailsForEmp",
        contentType: "application/json;charset=utf-8",
        //data: "{'isActive':'" + null + "','locationId':'" + null + "','EmpId':'" + EmpId + "'}",
        data: "{'EmpId':'" + EmpId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetSalaryData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetSalaryData(Tdata) {
    $("#grdSalDetails").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        EmpID: {
                            type: "number",
                            editable: false
                        },
                        RevisionDate: {
                            type: "date"
                        },
                        MonthlyCTC: {
                            type: "number"
                        },
                        Basic: {
                            type: "number"
                        },
                        HRA: {
                            type: "number"
                        },
                        Conveyance: {
                            type: "number"
                        },
                        Medical: {
                            type: "number"
                        },
                        Food: {
                            type: "number"
                        },
                        Special: {
                            type: "number"
                        },
                        PF: {
                            type: "number"
                        },
                        LTA: {
                            type: "number"
                        },
                        PT: {
                            type: "number"
                        },
                        Insurance: {
                            type: "number"
                        },
                        Bonus: {
                            type: "number"
                        },
                        PBB: {
                            type: "boolean"
                        },
                    }
                }
            },
            //pageSize: 100,
        },
        scrollable: true,
        //sortable: true,
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        //dataBound: resetRowNumber,
        //selectable: 'row',  //selects a row on click
        columns: [{
            field: "EmpID",
            title: "ID",
            width: "50px",
            hidden: true
        }, {
            field: "RevisionDate",
            title: "Revision Month",
            width: "100px",
            template: '<div style="text-align:center;">#= kendo.toString(kendo.parseDate(RevisionDate, "yyyy-MM-dd"), "MMM-yyyy") #',
        }, {
            field: "MonthlyCTC",
            title: "CTC (INR)",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(MonthlyCTC,"n0") #</div>'
        }, {
            field: "Basic",
            title: "Basic",
            width: "100px",
            template: '<div class="ra">#= kendo.toString(Basic,"n0") #</div>'
        }, {
            field: "HRA",
            title: "HRA",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(HRA,"n0") #</div>'
        }, {
            field: "Conveyance",
            title: "Conveyance",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(Conveyance,"n0") #</div>'
        },
        {
            field: "Medical",
            title: "Medical",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(Medical,"n0") #</div>'
        }, {
            field: "Food",
            title: "Food",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(Food,"n0") #</div>'
        }, {
            field: "Special",
            title: "Special",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(Special,"n0") #</div>'
        }, {
            field: "PF",
            title: "PF",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>'
        }, {
            field: "PF",
            title: "EPF",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>'
        }, {
            field: "LTA",
            title: "LTA",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(LTA,"n0") #</div>'
        }, {
            field: "PT",
            title: "PT",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(PT,"n0") #</div>'
        }, {
            field: "Insurance",
            title: "Insurance",
            width: "120px",
            template: '<div class="ra">#= kendo.toString(Insurance,"n0") #</div>'
        }, {
            field: "Bonus",
            title: "Bonus",
            width: "120px",
            //template: '<div class="ra">#= kendo.toString(Bonus,"n0") #</div>'
            template: '#if(PBB) {#<div class="ra">#= kendo.toString(Bonus,"n0") # (Fixed) #} else {#<div class="ra">#= kendo.toString(Bonus,"n0") # (Performance Based) #} #</div>'

        },
        ],

        //filterable: {
        //    extra: false,
        //    operators: {
        //        string: {
        //            startswith: "Starts with",
        //            contains: "Contains",
        //            eq: "Is equal to"
        //        }
        //    }
        //},
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        //change: function (arg) {

        //    var window = $("#divSalDetails");
        //    openPopUP();

        //    var gview = $("#grdPayroll").data("kendoGrid");
        //    var selectedItem = gview.dataItem(gview.select());
        //    $('[id$="hdnEmpId"]').val(selectedItem.ID);
        //    alert($('[id$="hdnEmpId"]').val());

        //    GetSalDetails($('[id$="hdnEmpId"]').val());
        //    showCursorPointerForSalDetails();
        //},
    });
}

//////////////////////////////     Salary Details All (1st Popup -2nd grid) ///////////////////////////

function GetSalaryDataForEmp(EmpId, yr) {

    if (yr == 0) {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var year = today.getFullYear();

        //alert(dd + '/' + mm + '/' + year);

        if (mm < 4)
            year = year - 1;
        else
            year = year;
    }
    else {
        year = yr;
    }

    $('[id$="lblyear"]').html('Apr-' + year + ' To ' + 'Mar-' + parseInt(year + 1));

    $('[id$="hdnYear"]').val(year);

    $.ajax({
        type: "POST",
        url: "PayMaster.aspx/BindSalaryDetailsForEmp",
        contentType: "application/json;charset=utf-8",
        data: "{'EmpId':'" + EmpId + "','year':'" + year + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetSalaryDetailsForEmp(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetSalaryDetailsForEmp(Tdata) {
    $("#grdSaldetailsAll").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        PayDate: {
                            type: "date"
                        },
                        Basic: {
                            type: "number"
                        },
                        HRA: {
                            type: "number"
                        },
                        Conveyance: {
                            type: "number"
                        },
                        Medical: {
                            type: "number"
                        },
                        Food: {
                            type: "number"
                        },
                        Special: {
                            type: "number"
                        },
                        LTA: {
                            type: "number"
                        },
                        PF: {
                            type: "number"
                        },
                        PT: {
                            type: "number"
                        },
                        Insurance: {
                            type: "number"
                        },
                        Loan: {
                            type: "number"
                        },
                        Advance: {
                            type: "number"
                        },
                        Leaves: {
                            type: "number"
                        },
                        Presents: {
                            type: "number"
                        },
                        Tax: {
                            type: "number"
                        },
                        Deduction: {
                            type: "number"
                        },
                        Bonus: {
                            type: "number"
                        },
                        Addition: {
                            type: "number"
                        },
                        Remark: {
                            type: "string"
                        },
                        CTC: {
                            type: "number"
                        },
                        Gross: {
                            type: "number"
                        },
                        Net: {
                            type: "number"
                        },
                        TotalAddition: {
                            type: "number"
                        },
                        TotalDeduction: {
                            type: "number"
                        },
                        Ded_Leave: {
                            type: "number"
                        },
                    }
                }
            },
            //pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        //selectable: 'row',  //selects a row on click
        columns: [{
            field: "PayDate",
            title: "Date",
            width: "100px",
            template: '<div style="text-align:center;">#= kendo.toString(kendo.parseDate(PayDate, "yyyy-MM-dd"), "MMM-yyyy") #',
        }, {
            field: "CTC",
            title: "CTC",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(CTC,"n0") #</div>'
        }, {
            field: "Gross",
            title: "Gross",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Gross,"n0") #</div>'
        }, {
            field: "Net",
            title: "Net Salary",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Net,"n0") #</div>'
        },
        {
            field: "Remark",
            title: "Remark",
            width: "100px",
        }, {
            field: "TotalAddition",
            title: "Total Addition",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(TotalAddition,"n0") #</div>'
        }, {
            field: "TotalDeduction",
            title: "Total Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(TotalDeduction,"n0") #</div>'
        }, {
            field: "Basic",
            title: "Basic",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Basic,"n0") #</div>'
        }, {
            field: "HRA",
            title: "HRA",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(HRA,"n0") #</div>'
        }, {
            field: "Conveyance",
            title: "Conveyance",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Conveyance,"n0") #</div>'
        },
        {
            field: "Medical",
            title: "Medical",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Medical,"n0") #</div>'
        }, {
            field: "Food",
            title: "Food",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Food,"n0") #</div>'
        }, {
            field: "Special",
            title: "Special",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Special,"n0") #</div>'
        }, {
            field: "LTA",
            title: "LTA",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(LTA,"n0") #</div>'
        }, {
            field: "PF",
            title: "EPF",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>'
        }, {
            field: "PF",
            title: "PF",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>'
        }, {
            field: "PT",
            title: "PT",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(PT,"n0") #</div>'
        }, {
            field: "Insurance",
            title: "Insurance",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Insurance,"n0") #</div>'
        }, {
            field: "Ded_Leave",
            title: "Leaves Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Ded_Leave,"n0") #</div>'
        },
        {
            field: "Loan",
            title: "Loan",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Loan,"n0") #</div>'
        }, {
            field: "Advance",
            title: "Advance",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Advance,"n0") #</div>'
        }, {
            field: "Tax",
            title: "Income Tax Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Tax,"n0") #</div>'
        }, {
            field: "Deduction",
            title: "Other Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Deduction,"n0") #</div>'
        }, 
       
        {
            field: "Leaves",
            title: "Leaves",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Leaves,"n0") #</div>'
        }, {
            field: "Presents",
            title: "Presents",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Presents,"n0") #</div>'
        }, {
            field: "Bonus",
            title: "Bonus",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Bonus,"n0") #</div>'
        }, {
            field: "Addition",
            title: "Addition",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Addition,"n0") #</div>'
        },
        ],

        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    contains: "Contains",
                    eq: "Is equal to"
                }
            }
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        //change: function (arg) {

        //    var window = $("#TSdetails");
        //    openTSPopUP();

        //    var gview = $("#grdTSDetails").data("kendoGrid");
        //    var selectedItem = gview.dataItem(gview.select());

        //    var gview1 = $("#grdProjectDetails").data("kendoGrid");
        //    var selectedItem1 = gview1.dataItem(gview1.select());

        //    $('[id$="hdnProjectId"]').val(selectedItem1.ID);
        //    $('[id$="hdnTSYear"]').val(selectedItem.TSYear);
        //    $('[id$="hdnTSMonth"]').val(selectedItem.TSMonth);
        //    //alert($('[id$="hdnProjectId"]').val() + " "+ $('[id$="hdnTSYear"]').val()+" "+ $('[id$="hdnTSMonth"]').val());

        //    GetTimesheetDetailsWorkwise($('[id$="hdnProjectId"]').val(), $('[id$="hdnTSYear"]').val(), $('[id$="hdnTSMonth"]').val());
        //    showCursorPointerForMonthTSDetails()
        //},
    });
}

function openPopUP() {
    $('#divSalDetails').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}

function closePopUP() {
    $('#divSalDetails').css('display', 'none');
    $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}