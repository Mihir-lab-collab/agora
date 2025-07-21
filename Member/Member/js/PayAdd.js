var rowNumber = 0;
var calculatedData = '';

function resetRowNumber(e) {
    rowNumber = 0;
}

function renderNumber(data) {
    return ++rowNumber;
}

////$('tr').each(function () {
//$("#grdPayAdd").find('.k-grid-content tr').each(function () {
//    $(this).find('td:eq(1)').addClass('addbgcolor')
//});

function CheckPayProcess() {
    var month = $("[id$=dlMonth] option:selected").val();
    //var loc = $("[id$=DDLocations] option:selected").val(); //$('[id$="hdLocationId"]').val();
    var year = $("#ctl00_ContentPlaceHolder1_txtYear").val();// $('[id$="txtYear"]').val();
    var noOfdays = $('[id$="hdnNoOfDays"]').val();
    $.ajax(
       {
           type: "POST",
           url: "PayAdd.aspx/IFExistsPayProcess",
           contentType: "application/json;charset=utf-8",
           data: "{'month':'" + month + "','year':'" + year + "'}",//,'locationId':'" + loc + "'}",
           dataType: "json",
           cache: false,
           async: false,
           success: function (msg) {
               var detailObj = jQuery.parseJSON(msg.d);

               if (detailObj.value == "false") {
                   if (confirm('Do you want to continue?')) {
                       showDetailsdiv();

                       GetAddPayDetails(year, month, noOfdays);
                   }
               }
               else if (detailObj.value == "true") {
                   alert('PayProcess for this Month is already done.')
               }
           },
           error: function (x, e) {
               alert("The call to the server side failed. "
                     + x.responseText);
           }
       }
);


}


function GetAddPayDetails(year, month, noOfdays) {
    $('[id$="lblMonth"]').html($("[id$=dlMonth] option:selected").text() + " " + year);
    $('[id$="lblLocation"]').html($("[id$=DDLocations] option:selected").text());

    $.ajax({
        type: "POST",
        url: "PayAdd.aspx/BindAddPayDetails",
        contentType: "application/json;charset=utf-8",
        //data: "{'year':'" + year + "','month':'" + month + "','locationId':'" + loc + "','noOfdays':'" + noOfdays + "'}",
        data: "{'year':'" + year + "','month':'" + month + "','noOfdays':'" + noOfdays + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetPayProcessData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });


}
var grid;
function GetPayProcessData(Tdata) {
    grid = $("#grdPayAdd").kendoGrid({
        height: 500,
        //edit: onEdit,
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
                            type: "string", editable: true
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
                        PT: {
                            type: "number"
                        },
                        Insurance: {
                            type: "number"
                        },
                        Ded_Loan: {
                            type: "number",
                            validation: { min: 0 }
                        },
                        Ded_Advance: {
                            type: "number",
                            validation: { min: 0 }
                        },
                        Ded_Tax: {
                            type: "number",
                            validation: { min: 0 }
                        },
                        Ded_Deduction: {
                            type: "number",
                            validation: { min: 0 }
                        },
                        Add_Bonus: {
                            type: "number",
                            validation: { min: 0 }
                        },
                        Add_Addition: {
                            type: "number",
                            validation: { min: 0 }
                        },
                        Gross: {
                            type: "number", editable: false
                        },
                        Days: {
                            type: "number", editable: false
                        },
                        Leaves: {
                            type: "number", editable: false
                        },
                        //
                        Presents: {
                            type: "number", editable: false
                        },
                        //
                        TotalAddition: {
                            type: "number", defaultValue: 0
                        },
                        TotalDeduction: {
                            type: "number", defaultValue: 0
                        },
                        CTC: {
                            type: "number", defaultValue: 0
                        },
                        Net: {
                            type: "number", defaultValue: 0
                        },
                        Ded_Leave: {
                            type: "number", defaultValue: 0
                        },
                        Remark: {
                            type: "string"
                        },
                        CalcGross: {
                            type: "number"
                        },
                        CalcBasic: {
                            type: "number"
                        },
                        PF: {
                            type: "number"
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
        columns: [{
            title: "Sr No",
            width: "40px",
            template: "#= renderNumber(data) #",
            attributes: {
                style: "background-color: lightgray;"
            },
        },

        {
            title: "Select All",
            width: "60px",
            template: '<div class="center"><input id="checkbox#=EmpID#" class="chkbxq" type="checkbox" checked="checked"/></div>',
            headerTemplate: '<div class="center"><label>Select All</label><br /><input id="selectall" class="chkbx" type="checkbox"  onclick="ToggleChkBox(this.checked);" checked="checked"/></div>',
            attributes: {
                style: "background-color: lightgray;"
            },

        }, {
            field: "EmpName",
            title: "Name",
            width: "100px",
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "EmpID",
            title: "ID",
            width: "50px",
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Gross",
            title: "Gross",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Gross,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Days",
            title: "Days",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(Days,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Leaves",
            title: "Leaves",
            width: "55px",
            template: '<div class="ra">#= kendo.toString(Leaves,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            title: "Deductions",
            columns: [
        {
            field: "Ded_Loan",
            title: "Loan Installment",
            width: "80px",
            template: '<div class="ra blue">#= kendo.toString(Ded_Loan,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "PF",
            title: "Provident Fund",
            width: "80px",
            template: '<div class="ra blue">#= kendo.toString(PF,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Ded_Advance",
            title: "Advance",
            width: "80px",
            template: '<div class="ra blue">#= kendo.toString(Ded_Advance,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Ded_Tax",
            title: "Income Tax Deduction",
            width: "80px",
            template: '<div class="ra blue">#= kendo.toString(Ded_Tax,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Ded_Deduction",
            title: "Other Deduction",
            width: "80px",
            template: '<div class="ra blue">#= kendo.toString(Ded_Deduction,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }
            ]
        },
        {
            title: "Additions",
            columns: [

        {
            field: "Add_Bonus",
            title: "Bonus",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Add_Bonus,"n0") #</div>',
            attributes: {
                style: "background-color: pink"
            },
        }, {
            field: "Add_Addition",
            title: "Other Addition",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Add_Addition,"n0") #</div>',
            attributes: {
                style: "background-color: pink"
            },
        }]
        }, {
            field: "Remark",
            title: "Remark",
            width: "80px",
            attributes: {
                style: "background-color: lightgreen;"
            },
        }
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
        editable: true,
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });
}

//function onEdit(editEvent) {
//    // Ignore edits of existing rows.
//    if (!editEvent.model.isNew()) { return; }
//    //editEvent.model.set("Ded_Loan", { name: "" });
//    editEvent.container
//         .find("[name=Ded_Loan]")
//         .val("")
//         .trigger("change");

//    editEvent.container
//         .find("[name=Ded_Advance]")
//         .val("")
//         .trigger("change");

//    editEvent.container
//         .find("[name=Ded_Tax]")
//         .val("")
//         .trigger("change");

//    editEvent.container
//         .find("[name=Ded_Deduction]")
//         .val("")
//         .trigger("change");

//    editEvent.container
//         .find("[name=Add_Bonus]")
//         .val("")
//         .trigger("change");

//    editEvent.container
//         .find("[name=Add_Addition]")
//         .val("")
//         .trigger("change");
//}

//$('tr').each(function () {
$("#grdPayAdd").find('.k-grid-content tr').each(function () {
    $(this).find('td:eq(1)').addClass('addbgcolor')
});

$('#grdPayAdd').on("click", "td", function (e) {
    var selectedTd = $(e.target).closest("td");
    var grdChkBox = selectedTd.parents('tr').find("td:first").next("td").find('input:checkbox');
    grdChkBox.prop('checked', !grdChkBox.prop('checked'));

});
function ToggleChkBox(flag) {
    //code of old version
    //$('.chkbxq').each(function () {
    //    $(this).attr('checked', flag);
    //});
    $('.chkbxq').prop('checked', flag);

}

function getNoofDays() {
    var month = $("[id$=dlMonth] option:selected").val();
    var year = $("#ctl00_ContentPlaceHolder1_txtYear").val(); //$('[id$="txtYear"]').val();

    $.ajax({
        type: "POST",
        url: "PayAdd.aspx/getNoofDays",
        contentType: "application/json;charset=utf-8",
        data: "{'month':'" + month + "','year':'" + year + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            $('[id$="hdnNoOfDays"]').val(msg.d);

        },
        error: function (x, e) {
            alert("The error occured. " + x.responseText);
        }
    });
}
//////////////////////////////     Final PayProcess Details ///////////////////////////

var StrChecked = ''; var checkedEmpID = [];
function GetCalPayDetails() {
    showCalculationdiv();
    var grid = $("#grdPayAdd").data("kendoGrid");
    //var count = grid.tbody.find("input[type='checkbox']").length;

    grid.tbody.find("input[type='checkbox']").each(function (index) {
        if ($(this).is(':checked')) {
            StrChecked = StrChecked + ";" + index;
        }
        else {
            checkedEmpID.push($(this).attr("id").replace('checkbox', ''));
        }
    });

    var ss = StrChecked.split(';');
    //$("#grdPayProcessCal").find('.k-grid-content tr').each(function (index) {
    //    if (jQuery.inArray(index.toString(), ss) != -1) {
    //        $(this).show();
    //    } else {
    //        $(this).hide();
    //    }
    //});
    var displayedData = $("#grdPayAdd").data().kendoGrid.dataSource.view();
    var new_displayedData = displayedData;
    var getds = $("#grdPayAdd").data("kendoGrid").dataSource;

    for (var i = 0; i < displayedData.length; i++) {
        displayedData[i].Remark = displayedData[i].Remark.replace(/'/g, '|');
        for (var j = 0; j < checkedEmpID.length; j++)
        {
            if (eval(displayedData)[i].EmpID == checkedEmpID[j]) {
                new_displayedData.splice(i, 1);
                //break;
            }
        }
    }

    var displayedDataAsJSON = JSON.stringify(new_displayedData);

    if (ss != "") {
        $('[id$="spnSubmitCal"]').show();
    }
    else {
        $('[id$="spnSubmitCal"]').hide();
    }
    $('[id$="hdnJSONData"]').val(displayedDataAsJSON);
    var month = $("[id$=dlMonth] option:selected").val();
    $.ajax({
        type: "POST",
        url: "PayAdd.aspx/CalculateData",
        contentType: "application/json;charset=utf-8",
        data: "{'hdnJSONData':'" + $('[id$="hdnJSONData"]').val() + "','month':'" + month + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {

            var dataG = jQuery.parseJSON(msg.d);

            for (var i = 0; i < dataG.length; i++) {
                dataG[i].Remark = dataG[i].Remark.replace('|', '\'');
            }

            GetCalPayProcessData(dataG);
            calculatedData = JSON.stringify(jQuery.parseJSON(msg.d));
            var ss = StrChecked.split(';');
            //$("#grdPayProcessCal").find('.k-grid-content tr').each(function (index) {
            //    if (jQuery.inArray(index.toString(), ss) != -1) {
            //        $(this).show();
            //    } else {
            //        $(this).hide();
            //    }
            //});

        },
        error: function (x, e) {
            alert("The error occured. "
                  + x.responseText);
        }
    });

}

function GetCalPayProcessData(Tdata) {
    $('[id$="lblPayComment"]').html($('[id$="txtPayComment"]').val());

    $("#grdPayProcessCal").kendoGrid({
        dataSource: Tdata,
        height: 500,
        scrollable: true,
        dataBound: resetRowNumber,
        columns: [
        {
            field: "EmpName",
            title: "Name",
            width: "100px",
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "EmpID",
            title: "ID",
            width: "50px",
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "Gross",
            title: "Gross",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Gross,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "CalcGross",
            title: "Calculated Gross",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(CalcGross,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Net",
            title: "Net",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Net,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "CTC",
            title: "CTC",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(CTC,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },

        {
            field: "TotalAddition",
            title: "Total Addition",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(TotalAddition,"n0") #</div>',
            attributes: {
                style: "background-color: lightpink;"
            },
        },
        {
            field: "TotalDeduction",
            title: "Total Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(TotalDeduction,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        },

        {
            field: "Basic",
            title: "Basic",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Basic,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
         {
             field: "CalcBasic",
             title: "Calculated Basic",
             width: "80px",
             template: '<div class="ra">#= kendo.toString(CalcBasic,"n0") #</div>',
             attributes: {
                 style: "background-color: lightgray;"
             },
         },
        {
            field: "HRA",
            title: "HRA",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(HRA,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Conveyance",
            title: "Conveyance",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Conveyance,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "Medical",
            title: "Medical",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Medical,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Food",
            title: "Food",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Food,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Special",
            title: "Special",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Special,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "LTA",
            title: "LTA",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(LTA,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "Days",
            title: "Days",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Days,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Leaves",
            title: "Leaves",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Leaves,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
         {
            field: "Presents",
            title: "Present",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Presents,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            title: "Deductions",
            columns: [
                {
                    field: "PT",
                    title: "PT",
                    width: "80px",
                    template: '<div class="ra">#= kendo.toString(PT,"n0") #</div>',
                    attributes: {
                        style: "background-color: lightblue;"
                    },
                }, {
                    field: "Insurance",
                    title: "Insurance",
                    width: "80px",
                    template: '<div class="ra">#= kendo.toString(Insurance,"n0") #</div>',
                    attributes: {
                        style: "background-color: lightblue;"
                    },
                }, {
                    field: "PF",
                    title: "PF",
                    width: "80px",
                    template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>',
                    attributes: {
                        style: "background-color: lightblue;"
                    },
                },
                        {
                            field: "Ded_Leave",
                            title: "Leaves Deduction",
                            width: "80px",
                            template: '<div class="ra">#= kendo.toString(Ded_Leave,"n0") #</div>',
                            attributes: {
                                style: "background-color: lightblue;"
                            },
                        },
        {
            field: "Ded_Loan",
            title: "Loan Installment",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Ded_Loan,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Ded_Advance",
            title: "Advance",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Ded_Advance,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Ded_Tax",
            title: "Income Tax Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Ded_Tax,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Ded_Deduction",
            title: "Other Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Ded_Deduction,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }
            ]
        },
        {
            title: "Additions",
            columns: [
        {
            field: "Add_Bonus",
            title: "Bonus",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Add_Bonus,"n0") #</div>',
            attributes: {
                style: "background-color: lightpink;"
            },
        }, {
            field: "Add_Addition",
            title: "Other Addition",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Add_Addition,"n0") #</div>',
            attributes: {
                style: "background-color: lightpink;"
            },
        }]
        },
         {
             field: "Remark",
             title: "Remark",
             width: "80px",
             attributes: {
                 style: "background-color: lightgreen;"
             },
         }
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
    });
}

function SaveData() {
    var month = $("[id$=dlMonth] option:selected").val();
    var loc = $('[id$="hdLocationId"]').val();
    var year = $("#ctl00_ContentPlaceHolder1_txtYear").val();// $('[id$="txtYear"]').val();

    $.ajax({
        type: "POST",
        url: "PayAdd.aspx/SaveSalary",
        contentType: "application/json;charset=utf-8",
        data: "{'month':'" + month + "','year':'" + year + "','locationId':'" + loc + "','PayComment':'" + $('[id$="lblPayComment"]').html() + "','calculatedData':'" + calculatedData + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            if (msg.d == true) {
                alert("Pay process details saved successfully.");
                clearData();
                showInputdiv();
                window.location.assign("/Member/PayStatement.aspx");
            } else { }
        },
        error: function (x, e) {
            alert("The error occured. " + x.responseText);
        }
    });
}

function showInputdiv() {
    $('#details').css('display', 'none');
    $('#divinput').css('display', 'block');
    $('#divCalculation').css('display', 'none');
}
function clearData() {
    $("#grdPayAdd").html("");
    $("#grdPayProcessCal").html("");
    //$('[id$="txtYear"]').val($('[id$="hdnCurrYear"]').val());
    $("#ctl00_ContentPlaceHolder1_txtYear").val($('[id$="hdnCurrYear"]').val());
    $('[id$="dlMonth"]').val($('[id$="hdnCurrMonth"]').val());
    $('[id$="dlLocation"]').val($('[id$="hdnCurrLoc"]').val());
    //$('[id$="hdnCurrYear"]').val('');
    //$('[id$="hdnCurrMonth"]').val('')
    //$('[id$="hdnCurrLoc"]').val('')
    //$('[id$="hdnNoOfDays"]').val('')
    $('[id$="hdLocationId"]').val($('[id$="hdnCurrLoc"]').val());
    $('[id$="hdnJSONData"]').val('')
    $('[id$="lblMonth"]').html('');
    $('[id$="lblLocation"]').html('')
    $('[id$="txtPayComment"]').val('')
    $('[id$="lblPayComment"]').html('');



}
function showDetailsdiv() {
    $('#details').css('display', 'block');
    $('#divinput').css('display', 'none');
}

function showCalculationdiv() {
    $('#divCalculation').css('display', 'block');
    $('#details').css('display', 'none');
    $('#divinput').css('display', 'none');
}

function CancelPayAdd() {
    $('#divCalculation').css('display', 'none');
    $('#details').css('display', 'none');
    $('#divinput').css('display', 'block');
}
function CancelFinal() {
    $('#divCalculation').css('display', 'none');
    $('#details').css('display', 'block');
    $('#divinput').css('display', 'none');
    $("#grdPayAdd").html("");
    GetPayProcessData($("#grdPayAdd").data("kendoGrid").dataSource._data);
    var ss = StrChecked.split(';');
    $("#grdPayAdd").data("kendoGrid").tbody.find("input[type='checkbox']").each(function (index) {
        // whatever you need done.
        if (jQuery.inArray(index.toString(), ss) != -1) {
            // return true;
            $(this).prop('checked', true);
        } else {
            // return false;
            $(this).prop('checked', false);
        }
    });

    StrChecked = '';
    checkedEmpID = [];
}





