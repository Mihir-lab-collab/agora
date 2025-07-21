$(document).ready(function () {



    $("#txtFatherName").keypress(function (event) {
        var inputValue = event.which;
        // allow letters and whitespaces only.
        if (!(inputValue >= 65 && inputValue <= 122) && (inputValue != 32 && inputValue != 0)) {
            event.preventDefault();
        }
    });
});



//-------------------------------AdminTDS------------------------------

function DateSelect() {
    var data = $("#txtStartDate").val();
    if (data == '') {
        data = getCurrentFiscalYear();
        $("#txtStartDate").val(data);
    }
    makeFinancialDate(data);
    GetTdsEmpList(data);
}

function GetTdsEmpList(data) {
    $.ajax(
        {
            type: "POST",
            url: "TDSAdmin.aspx/getEmpList",
            data: "{'Year':'" + data + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: true,
            success: function (msg) {
                if (msg.d.length != 0) {
                    GridTdsEmpList(msg.d);
                }
                else {
                    alert("No Records Found");
                    GridTdsEmpList(msg.d);
                }

            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        });

}

function GridTdsEmpList(Data) {
    $("#gridTDSEmpList").kendoGrid({
        height: 500,
        dataSource: {
            data: Data,
            schema: {
                model: {
                    fields: {
                        EmpId: { type: "string" },
                        EmpName: { type: "string" },
                        ModifiedOn: { type: "string" },
                    }
                }
            },
            pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        selectable: true,
        pageable: {
            input: true,
            numeric: false
        },

        selectable: 'row',//Select row on click on row
        columns: [

            { field: "EmpId", title: "Id", width: 120, },
            {
                field: "EmpName", title: "Employee Name", width: 120,

            },
            {
                field: "ModifiedOn", title: "Modified On", width: 120,

            },
        ],
        change: function (arg) {
            var gview = $("#gridTDSEmpList").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());
            var EmpId = selectedItem.EmpId;
            var SelectedYear = $("#txtStartDate").val();
            $('[id$="txtEmpName"]').val(selectedItem.EmpName)
            $('[id$="hdnEmpId"]').val(EmpId);
            getSelectedEmpTds(EmpId, SelectedYear);
        },

    });
}

function getSelectedEmpTds(EmpId, Year) {

    $.ajax(
        {
            type: "POST",
            url: "TDSAdmin.aspx/getSelectedTdsData",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ EmpId: EmpId, Year: Year }),
            dataType: "json",
            async: true,
            success: function (msg) {
                var listFirst = [];
                var listSecond = [];
                jQuery.each(msg.d, function (index, item) {
                    $("#txtFatherName").val($(item)[0]["FatherName"]);
                    $("#txtPanNo").val($(item)[0]["PanNo"]);
                    listFirst.push($(item)[0]);
                    if ($(item)[0]["Deslaimer"]) {
                        $('#ctl00_ContentPlaceHolder1_chkDec').prop('checked', true).prop('disabled', true);
                        $('#ctl00_ContentPlaceHolder1_chkDec').prop('disabled', true);
                    }
                    else {
                        $('#ctl00_ContentPlaceHolder1_chkDec').prop('checked', false);
                    }
                    if ($(item)[0]["RegimeStatus"] == "Not Exsit") {
                        var selectedRegimeType = $('#ctl00_ContentPlaceHolder1_hdnRegime').val();
                        if (selectedRegimeType == '') {

                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('checked', true)
                            $('#ctl00_ContentPlaceHolder1_hdnRegime').val('true');
                        }
                        else if (selectedRegimeType == 'true') {
                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('checked', true)
                        }
                        else {
                            $('#ctl00_ContentPlaceHolder1_rdOldRegime').prop('checked', true)
                        }
                    }
                    else {
                        if ($(item)[0]["Regime"]) {

                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('checked', true).prop('disabled', true);
                            $('#ctl00_ContentPlaceHolder1_rdOldRegime').prop('disabled', true);
                            $('#ctl00_ContentPlaceHolder1_hdnRegime').val('true');
                        }
                        else {
                            $('#ctl00_ContentPlaceHolder1_rdOldRegime').prop('checked', true).prop('disabled', true);
                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('disabled', true);
                        }
                    }

                });
                readOnly();
                GetDataTdsAdmin((listFirst));
                //GetDataSecondListTdsAdmin((listSecond));
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        }
    );
}

function GetDataTdsAdmin(Tdata) {
    $("#divAddPopup").css('display', 'block');
    AddDataInTemplete(Tdata);

    $("#gridProject").kendoGrid({
        autoBind: true,
        scrollable: false,
        selectable: true,
        editable: false,
        dataSource: {
            data: Tdata,
            group: { field: "Type" },
            schema: {
                model: {
                    fields: {
                        ID: { type: "int" },
                        TdsName: { type: "string", editable: false },
                        Amount: {
                            type: "number", editable: true,
                            validation: { required: true }
                        },
                        Comment: { type: "string", editable: true },
                        InsertedOn: { type: "string", editable: false },
                        Type: { type: "string", editable: false }
                    }
                }
            }
        },
        columns: [
            { field: "ID", title: "ID", hidden: true },
            { field: "TdsName", title: "Description", width: 190 },
            {
                field: "Amount", title: "Amount", width: 60,
                template: '<div style="text-align:right;">#= kendo.toString(Amount,"n") #</div>'
            },
            { field: "Comment", title: "Comment", width: 190 },
            { field: "InsertedOn", title: "InsertedOn", hidden: true }
        ],
        groupHeaderTemplate:
            '<tr><td colspan="3" style="background-color:#ffc000; font-weight:bold; text-decoration:underline;">#= value #</td></tr>'
    });
}

function GetDataSecondListTdsAdmin(Tdata) {

    $("#divAddPopup").css('display', 'block');
    AddDataInTempleteSecondList(Tdata);

    $("#gridSecondList").kendoGrid({
        autoBind: true,
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        ID: { type: "int" },
                        TdsName: { type: "string", editable: false },
                        Amount: {
                            type: "number", editable: true, validation: {
                                required: true
                            }
                        },
                        Comment: { type: "string", editable: true, },
                        InsertedOn: { type: "string", editable: false, },
                    }
                }
            },
        },
        scrollable: false,
        sortable: true,
        selectable: true,
        columns: [

            { field: "ID", title: "ID", width: 120, hidden: true },
            { field: "TdsName", title: "Description", width: 190, },
            {
                field: "Amount", title: "Amount", width: 120, template: '<div class="ra" style="text-align:right;">#= kendo.toString(Amount,"n") #</div>'
            },
            {
                field: "Comment", title: "Comment", width: 120,

            },
            {
                field: "InsertedOn", title: "InsertedOn", width: 120, hidden: true

            },
        ],
        editable: false,

    });

}

function readOnly() {
    document.getElementById("txtFatherName").readOnly = true;
    document.getElementById("txtPanNo").readOnly = true;
}

//-------------------------------EndAdminTDS------------------------------



//-------------------------------EmployeeTDS------------------------------
function IsEditGrid(TdsEmpYear) {
    var CurrYear = getCurrentFiscalYear();
    if (TdsEmpYear === CurrYear.toString()) {
        CurrYear = "true";
    }
    else {
        CurrYear = "false";
    }
    return CurrYear;
}


function GetTDS(Year) {
    var Edit = null;
    var CurrYear = getCurrentFiscalYear();

    $("#btnSaveChanges").css('display', 'block');
    $("#divAddPopup").css('display', 'block');
    if (typeof (Year) === null || Year === '' || Year === undefined) {

        Edit = IsEditGrid(CurrYear)
        Year = CurrYear;
    }
    else {
        Edit = IsEditGrid(Year)
        if (CurrYear === Year) {
            $("#btnSaveChanges").css('display', 'block');
            $("#btnExportToPdf").css('display', 'block');
            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('checked', true)
        }
        else {
            $("#btnSaveChanges").css('display', 'none');
            $("#btnExportToPdf").css('display', 'block');
        }
    }
    makeFinancialDate(Year);
    $.ajax(
        {
            type: "POST",
            url: "TdsEmployee.aspx/GetTds",
            contentType: "application/json;charset=utf-8",
            data: "{'Year':'" + Year + "'}",
            dataType: "json",
            async: true,
            success: function (msg) {
                var listFirst = [];
                var listSecond = [];
                jQuery.each(msg.d, function (index, item) {
                    if ($(item)[0]["FatherName"] == "") {
                        $('#txtFatherName').prop('readonly', false);
                    }
                    else {
                        $('#txtFatherName').prop('readonly', true);
                    }

                    $("#txtFatherName").val($(item)[0]["FatherName"]);

                    if ($(item)[0]["PanNo"] == "") {
                        $('#ctl00_ContentPlaceHolder1_txtPanNo').prop('readonly', false);
                    }
                    else {
                        $('#ctl00_ContentPlaceHolder1_txtPanNo').prop('readonly', true);
                    }
                    if ($(item)[0]["Deslaimer"]) {
                        $('#ctl00_ContentPlaceHolder1_chkDec').prop('checked', true).prop('disabled', true);
                        $('#ctl00_ContentPlaceHolder1_chkDec').prop('disabled', true);
                    }
                    else {
                        $('#ctl00_ContentPlaceHolder1_chkDec').prop('checked', false);
                    }
                    if ($(item)[0]["RegimeStatus"] == "Not Exsit") {
                        var selectedRegimeType = $('#ctl00_ContentPlaceHolder1_hdnRegime').val();
                        if (selectedRegimeType == '') {

                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('checked', true)
                            $('#ctl00_ContentPlaceHolder1_hdnRegime').val('true');
                        }
                        else if (selectedRegimeType == 'true') {
                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('checked', true)
                        }
                        else {
                            $('#ctl00_ContentPlaceHolder1_rdOldRegime').prop('checked', true)
                        }
                    }
                    else {
                        if ($(item)[0]["Regime"]) {

                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('checked', true).prop('disabled', true);
                            $('#ctl00_ContentPlaceHolder1_rdOldRegime').prop('disabled', true);
                            $('#ctl00_ContentPlaceHolder1_hdnRegime').val('true');
                        }
                        else {
                            $('#ctl00_ContentPlaceHolder1_rdOldRegime').prop('checked', true).prop('disabled', true);
                            $('#ctl00_ContentPlaceHolder1_rdNewRegime').prop('disabled', true);
                        }
                    }


                    $('[id$="txtPanNo"]').val($(item)[0]["PanNo"]);

                    listFirst.push($(item)[0]);
                });
                /*console.log(listFirst);*/
                var isNewRegimeSelected = $('#ctl00_ContentPlaceHolder1_hdnRegime').val();
                if (isNewRegimeSelected == 'true') {

                    GetDataForNewRegime((listFirst), Edit);
                }
                else {

                    GetData((listFirst), Edit);
                }

                /*GetDataSecondList((listSecond), Edit);*/
            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        }
    );
}

function GetFilledTdsEmployee(Tdata) {

    $("#gridFilledTds").kendoGrid({

        height: 500,
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Year: { type: "string" },
                        InsertedOn: { type: "string", editable: false, },
                    }
                }
            },
            pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        selectable: true,
        pageable: {
            input: true,
            numeric: false
        },

        selectable: 'row',//Select row on click on row
        columns: [

            { field: "Year", title: "Financial Year (Start)", width: 120, },
            {
                field: "InsertedOn", title: "Submitted On", width: 120,

            },
        ],
        change: function (arg) {
            var gview = $("#gridFilledTds").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());
            var TdsEmpYear = selectedItem.Year;

            GetTDS(TdsEmpYear);
        },

    });

}

//this function is use for tranfer data in templete format
function AddDataInTemplete(data) {
    // Group the data manually by 'Type'
    const groupedData = groupBy(data, "Type");

    let html = "";

    const customGroupOrder = [
        "House Property",
        "Deduction U/S 80C",
        "Deduction U/S 80CCD",
        "Deductions U/S 80D",
        "Deductions U/S 80DDB (Specified Disease)",
        "Deductions U/S 80E",
        "Deductions U/S 80EE",
        "Deductions U/S 80G",
        "Deductions U/S 80U"
    ];

    const template = kendo.template($("#template").html());

    // Loop through groups in defined order
    for (let group of customGroupOrder) {
        if (groupedData[group]) {
            html += `
                <tr>
                    <td colspan="5" style="background-color:#ffc000; font-weight:bold;">
                        ${group}
                    </td>
                </tr>`;
            html += kendo.render(template, groupedData[group]);
        }
    }

    $("#Temp tbody").html(html);
}

// Utility function to group an array of objects by a key
function groupBy(array, key) {
    return array.reduce((result, item) => {
        (result[item[key]] = result[item[key]] || []).push(item);
        return result;
    }, {});
}


//this function is use for tranfer data in templete format
function AddDataInTempleteSecondList(Data) {
    var template = kendo.template($("#template1").html());
    var dataSource = new kendo.data.DataSource({
        data: Data,
        change: function () { // subscribe to the CHANGE event of the data source
            $("#Temp1 tbody").html(kendo.render(template, this.view())); // populate the table
        }
    });
    dataSource.read();
}

function GetData(Tdata, CurrYear) {
    var isEdit = true;
    if (CurrYear != "true") {
        isEdit = false;
    }
    AddDataInTemplete(Tdata);

    $("#gridProject").kendoGrid({
        autoBind: true,
        scrollable: false,
        selectable: true,
        editable: isEdit,
        dataSource: {
            data: Tdata,
            group: { field: "Type" },
            schema: {
                model: {
                    fields: {
                        ID: { type: "int" },
                        TdsName: { type: "string", editable: false },
                        Amount: {
                            type: "number", editable: true,
                            validation: { required: true }
                        },
                        Comment: { type: "string", editable: true },
                        InsertedOn: { type: "string", editable: false },
                        Type: { type: "string", editable: false }
                    }
                }
            }
        },
        columns: [
            { field: "ID", title: "ID", hidden: true },
            { field: "TdsName", title: "Description", width: 190 },
            {
                field: "Amount", title: "Amount", width: 60,
                template: '<div style="text-align:right;">#= kendo.toString(Amount,"n") #</div>'
            },
            { field: "Comment", title: "Comment", width: 190 },
            { field: "InsertedOn", title: "InsertedOn", hidden: true }
        ],
        groupHeaderTemplate:
            '<tr><td colspan="3" style="background-color:#ffc000; font-weight:bold; text-decoration:underline;">#= value #</td></tr>'
    });

}
function GetDataForNewRegime(Tdata, CurrYear) {
    var isEdit = true;
    if (CurrYear != "true") {
        isEdit = false;
    }
    AddDataInTemplete(Tdata);

    $("#gridProject").kendoGrid({
        autoBind: true,
        scrollable: false,
        selectable: true,
        editable: isEdit,
        dataSource: {
            data: Tdata,
            group: { field: "Type" },
            schema: {
                model: {
                    fields: {
                        ID: { type: "int" },
                        TdsName: { type: "string", editable: false },
                        Amount: {
                            type: "number",
                            editable: false, // ← Disabled here
                            validation: { required: true }
                        },
                        Comment: { type: "string", editable: true },
                        InsertedOn: { type: "string", editable: false },
                        Type: { type: "string", editable: false }
                    }
                }
            }
        },
        columns: [
            { field: "ID", title: "ID", hidden: true },
            { field: "TdsName", title: "Description", width: 190 },
            {
                field: "Amount", title: "Amount", width: 60,
                template: '<div style="text-align:right;">#= kendo.toString(Amount,"n") #</div>'
            },
            { field: "Comment", title: "Comment", width: 190 },
            { field: "InsertedOn", title: "InsertedOn", hidden: true }
        ],
        groupHeaderTemplate:
            '<tr><td colspan="3" style="background-color:#ffc000; font-weight:bold; text-decoration:underline;">#= value #</td></tr>'
    });
}


$(document).ready(function () {
    // Bind to radio button change event
    $("input[name='ctl00$ContentPlaceHolder1$Regime']").on("change", function () {
        var isNewRegime = $('#ctl00_ContentPlaceHolder1_rdNewRegime').is(':checked');

        if (isNewRegime) {
            $('#ctl00_ContentPlaceHolder1_hdnRegime').val('true');
            /* $('.declarationCheck').css('display', '');*/
        } else {
            $('#ctl00_ContentPlaceHolder1_hdnRegime').val('false');
            /*$('.declarationCheck').css('display', 'none');*/
        }
        GetTDS();
    });
});


function GetDataSecondList(Tdata, CurrYear) {

    var isEdit = true;
    if (CurrYear != "true") {
        isEdit = false;
    }
    AddDataInTempleteSecondList(Tdata);

    $("#gridSecondList").kendoGrid({
        autoBind: true,
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        ID: { type: "int" },
                        TdsName: { type: "string", editable: false },
                        Amount: {
                            type: "number", editable: true, validation: {
                                required: true
                            }
                        },
                        Comment: { type: "string", editable: true, },
                        InsertedOn: { type: "string", editable: false, },
                    }
                }
            },
            //pageSize: 50,
        },
        scrollable: false,
        sortable: true,
        selectable: true,
        //Use for move from one page to another
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [

            { field: "ID", title: "ID", width: 120, hidden: true },
            { field: "TdsName", title: "Description", width: 190, },
            {
                field: "Amount", title: "Amount", width: 120, template: '<div class="ra" style="text-align:right;">#= kendo.toString(Amount,"n") #</div>'
            },
            {
                field: "Comment", title: "Comment", width: 120,

            },
            {
                field: "InsertedOn", title: "InsertedOn", width: 120, hidden: true

            },
        ],
        editable: isEdit,

    });

}

function SaveChangesTDS() {

    if (!$('#ctl00_ContentPlaceHolder1_chkDec').is(':checked')) {
        alert("Please check the declaration checkbox before proceeding.");
        return;
    }
    Validate();

    var gridData = $("#gridProject").data("kendoGrid").dataSource.data();
    //var gridData1 = $("#gridSecondList").data("kendoGrid").dataSource.data();
    var FatherName = $("#txtFatherName").val();
    var PanNo = $('[id$="txtPanNo"]').val();
    var InsertedBy = $('[id$="hdnEmpId"]').val();
    var Regime = $("input[name='ctl00$ContentPlaceHolder1$Regime']:checked").val();
    var Disclaimer = $('[id$="chkDec"]').is(":checked");
    if (FatherName != "" && PanNo != "") {
        closePopUP();
        $.ajax(
            {
                type: "POST",
                url: "TdsEmployee.aspx/SaveTDS",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({ GridData: gridData, FatherName: FatherName, PanNo: PanNo, IsRegime: Regime, IsDeclaimer: Disclaimer }),//, InsertedBy: InsertedBy
                dataType: "json",
                async: true,
                success: function (msg) {
                    CheckExistingTdsEmp();
                    alert("TDS Updated Sucessfully...");

                },
                error: function (x, e) {
                    alert("The call to the server side failed. "
                        + x.responseText);
                }
            }
        );
    }
    else {

        // document.getElementById('dvfile').style.display = "block";
        //  alert("Value should not be empty");
    }

}

function CheckExistingTdsEmp() {

    var Year = getCurrentFiscalYear();
    $.ajax(
        {
            type: "POST",
            url: "TdsEmployee.aspx/CheckExistingTdsEmployee",
            contentType: "application/json;charset=utf-8",
            data: "{'Year':'" + Year + "'}",
            dataType: "json",
            async: true,
            success: function (msg) {
                $("#btnGetTds").css('display', 'block');
                if (msg.d.length !== 0) {
                    GetFilledTdsEmployee((msg.d));
                }

            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        });
}

function closePopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");

    document.getElementById('dvErr').style.display = "none";
    document.getElementById('dvMsg').style.display = "none";
    $('#ctl00_ContentPlaceHolder1_hdnRegime').val('');
}

function getCurrentFiscalYear() {

    var today = new Date();

    var curMonth = parseInt(today.getMonth()) + 1;

    var fiscalYr = "";
    var FullYear = "";
    if (curMonth < 4) {
        FullYear = (today.getFullYear() - 1).toString();

    } else {
        FullYear = today.getFullYear().toString();
    }
    return FullYear;
}

function makeFinancialDate(Selectedyear) {
    var date = parseInt(Selectedyear) + 1;
    date = Selectedyear + "-" + date;
    $("#txtFinancialYear").val(date);
}

function Validate() {
    if ($("#txtFatherName").val() == "") {
        document.getElementById('dvMsg').style.display = "block";
        document.getElementById('dvErr').style.display = "none";
        return false;
        //alert("Please Enter Father's Name!");

    }
    if ($('[id$="txtPanNo"]').val() == "") {
        document.getElementById('dvErr').style.display = "block";
        document.getElementById('dvMsg').style.display = "none";
        return false;

        // alert("Please Enter PAN No!");
    }

}


function getPDF(selector) {

    if ($("#txtFatherName").val() != "" && $('[id$="txtPanNo"]').val() != "") {
        Getvalue();
        $("#dv1stTable").css("display", "block")
        $("#dv1stDumyDisplayTable").css("display", "none")
        $("#gridProject").css("display", "none")
        $("#dvTemp").css("display", "block")
        $("#gridSecondList").css("display", "none")
        $("#dvTemp1").css("display", "block")
        var selectedValue = $("input[name='ctl00$ContentPlaceHolder1$Regime']:checked").val();

        if (selectedValue === 'rdNewRegime') {
            $("#tickNew").html(`
                <div style="
                    position: absolute;
                    top: 2px;
                    left: 4px;
                    width: 6px;
                    height: 12px;
                    border-right: 2px solid green;
                    border-bottom: 2px solid green;
                    transform: rotate(45deg);
                "></div>
            `);


            $("#tickOld").html(`
                <div style="position:absolute; width:12px; height:2px; background:red; transform:rotate(45deg); top:7px; left:3px;"></div>
                <div style="position:absolute; width:12px; height:2px; background:red; transform:rotate(-45deg); top:7px; left:3px;"></div>
            `);
        } else {

            $("#tickNew").html(`
                <div style="position:absolute; width:12px; height:2px; background:red; transform:rotate(45deg); top:7px; left:3px;"></div>
                <div style="position:absolute; width:12px; height:2px; background:red; transform:rotate(-45deg); top:7px; left:3px;"></div>
            `);


            $("#tickOld").html(`
                <div style="
                    position: absolute;
                    top: 2px;
                    left: 4px;
                    width: 6px;
                    height: 12px;
                    border-right: 2px solid green;
                    border-bottom: 2px solid green;
                    transform: rotate(45deg);
                "></div>
            `);
        }
        kendo.drawing.drawDOM($(selector)).then(function (group) {
            var Data = $('[id$="txtEmpName"]').val();

            var Year = $("#txtFinancialYear").val();

            kendo.drawing.pdf.saveAs(group, "TDS_" + Data + "_" + Year + ".pdf");

            $("#dv1stTable").css("display", "none")
            $("#dv1stDumyDisplayTable").css("display", "block")
            $("#gridProject").css("display", "block")
            $("#dvTemp").css("display", "none")
            $("#gridSecondList").css("display", "block")
            $("#dvTemp1").css("display", "none")
            alert("PDF Generated Sucessfully...");
        });
    }
    else {
        //  alert("Value should not be empty");
    }
}

function Getvalue() {
    $("#lblFinancialYear").text($("#txtFinancialYear").val());
    $("#lblEmpName1").text($('[id$="txtEmpName"]').val());
    $("#lblFatherName").text($("#txtFatherName").val());
    $("#lblPanNo").text($('[id$="txtPanNo"]').val());
}

function fnValidatePAN(Obj) {
    if (Obj == null) Obj = window.event.srcElement;
    if (Obj.value != "") {
        ObjVal = Obj.value;
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = ObjVal.substring(3, 4);
        if (ObjVal.search(panPat) == -1) {
            alert("Invalid Pan No");
            Obj.focus();
            return false;
        }
        if (code.test(code_chk) == false) {
            alert("Invaild PAN Card No.");
            return false;
        }
    }
}

//-------------------------------EndEmployeeTDS------------------------------

