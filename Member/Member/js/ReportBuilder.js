function GetLogData(Tdata) {
    $("#grid").kendoGrid({
        dataSource: { data: Tdata, pageSize: 100 },
        scrollable: true,
        pageable: {
            input: true,
            numeric: false
        },
        sortable: true,
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
        editable: false,

    });
}


function GetDetails() {

    $.ajax({
        type: "POST",
        url: "ReportBuilder.aspx/BindReport",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {

            GetData(jQuery.parseJSON(msg.d));
            $('[id$="pnlInput"]').hide();
            $('[id$="pnlQuery"]').hide();
            $('[id$="pnlGrid"]').hide();
            $('[id$="ctl00_ContentPlaceHolder1_pnlRun"]').hide();
            $('[id$="BtnAddReport"]').show();
            $('[id$="BtnCancelReport"]').hide();

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetData(ReportData) {

    $(gridReport).kendoGrid({
        dataSource: {
            data: ReportData,
            schema: {
                model: {
                    fields: {
                        reportId: { type: "number" },
                        name: { type: "string" },
                        Description: { type: "string" },

                        query: { type: "string" },
                        insertedOn: { type: "string" }
                    }
                }
            },
            pageSize: 100,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                    { field: "reportId", title: "ID", hidden: true },
                    { field: "name", title: "Name", width: "20px" },
                    { field: "Description", title: "Description", width: "20px" },

                    { field: "query", title: "Query", width: "50px" },
                     { field: "insertedOn", title: "Created Date", width: "10px" },
                    { command: [{ text: "Edit", click: showDetails }, { text: "Run", click: RunDetails }, { text: "Delete", click: DeleteReport }], width: "25px" }
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
        }
        ,
        editable: false,
    });
}

function showDetails(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var id = dataItem.reportId;
    $('[id$="hdnReportId"]').val(id);
    $('[id$="btnReportDetails"]').click();
}

function RunDetails(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var id = dataItem.reportId;
    $('[id$="hdnReportId"]').val(id);
    $('[id$="btnRunReport"]').click();
}

function DeleteReport(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var id = dataItem.reportId;
    if (confirm("Are you sure you want to delete?")) {
        $('[id$="hdnReportId"]').val(id);
        $('[id$="btnDeleteReport"]').click();
    } else {
    }
}

function CancelReport() {
    $('[id$="ctl00_ContentPlaceHolder1_pnlReport"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_pnlInput"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlQuery"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlGrid"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlRun"]').hide();
    $('[id$="BtnAddReport"]').show();
    $('[id$="BtnCancelReport"]').hide();
    $("#ctl00_ContentPlaceHolder1_lblReportNameBui").text('');
    GetDetails();
}

function AddReport() {
    $('[id$="ctl00_ContentPlaceHolder1_pnlReport"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlInput"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_pnlQuery"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_pnlGrid"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_pnlRun"]').hide();
    $('[id$="BtnAddReport"]').hide();
    $('[id$="BtnCancelReport"]').show();
}

function AddRunReport() {
    $('[id$="ctl00_ContentPlaceHolder1_pnlReport"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlInput"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlQuery"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlGrid"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_pnlRun"]').show();
    $('[id$="BtnAddReport"]').hide();
    $('[id$="BtnCancelReport"]').show();
}

function setSelectedDdlvalue() {
    var id = $('[id$="hdnFieldCount"]').val();
    $("#ddlInputFiled").val(id);
}

function ddlonChange() {
    var selectedval = document.getElementById("ddlInputFiled").value;
    $('[id$="hdnFieldCount"]').val(selectedval);
    if (selectedval == 0) {
        $('[id$="ctl00_ContentPlaceHolder1_ControlsDiv"]').html('');
    }

    else {
        var controlsbody = "<table width=\"82%\" style=\"overflow: hidden;\">";
        for (i = 1; i <= selectedval; i++) {
            var ddlid = "ddltype_" + i;

            controlsbody += "<tr><td>Parameter " + i + ":</td><td><select Id='" + ddlid + "' onchange='ddltypeChange(this);' name='" + ddlid + "'><option value='TEXT'>TextBox</option><option value='SQL'>SQL Statement</option><option value='CSV'>CSV Statement</option></select></td> <td style='float:right;'><textarea id='textarea" + i + "' name='textarea" + i + "' rows='3' cols='40' style='display:none'></textarea></td><td style='float:right;'><input type='text' id='text" + i + "' name='text" + i + "' /></td></tr>"; //style = 'display:none'
        }
        controlsbody += "</table>";
        $('[id$="ctl00_ContentPlaceHolder1_ControlsDiv"]').html(controlsbody);
        if ($('[id$="hdnIsNewAdd"]').val() == 1) {
            $('[id$="ctl00_ContentPlaceHolder1_pnlInput"]').show();
            $('[id$="ctl00_ContentPlaceHolder1_pnlQuery"]').show();
            $('[id$="ctl00_ContentPlaceHolder1_pnlGrid"]').hide();
        }

    }
}

function ddltypeChange(obj) {
    var splitid = obj.id.split('_');

    var DropSelected = splitid[1];

    var txtarea = "textarea" + DropSelected;
    var txtbox = "text" + DropSelected;

    $('[id$="' + txtbox + '"]').val('');
    $('[id$="' + txtarea + '"]').val('');

    if (obj.value == 'SQL') {
        $('[id$="' + txtbox + '"]').hide();
        $('[id$="' + txtarea + '"]').show();
    }
    else if (obj.value == 'CSV') {
        $('[id$="' + txtbox + '"]').hide();
        $('[id$="' + txtarea + '"]').show();
    }
    else {
        $('[id$="' + txtbox + '"]').show();
        $('[id$="' + txtarea + '"]').hide();
    }

}

//function GetQueryResult(obj) {

//    var objbtn = obj.id.split('_');
//    var j = objbtn[1];
//    var ddlTypeId = "ddltype_" + j;

//    if ($('[id$="' + ddlTypeId + '"]').val() == "2") {
//        //SQL DropDown

//        BindSQLDropDown(j)
//    }
//    else if ($('[id$="' + ddlTypeId + '"]').val() == "3") {
//        //CSV DropDown

//        var dlstresult = "ddlresult_" + j;
//        $('[id$="' + dlstresult + '"]').val('');
//        var textId = "text" + j;
//        var txtFieldVal = $('[id$="' + textId + '"]').val();
//        var arrValues = $.unique(txtFieldVal.split(','));
//        for (var i = 0; i < arrValues.length; i++) {
//            $('[id$="' + dlstresult + '"]').append('<option value="' + arrValues[i] + '">' + arrValues[i] + '</option>');
//        }


//    }

//}

function BindSQLDropDown(j, value) {

    var txtAreaId = "textarea" + j;
    var txtAreaVal = $('[id$="' + txtAreaId + '"]').val();

    $.ajax({
        type: "POST",
        url: "ReportBuilder.aspx/BindDropDown",
        contentType: "application/json;charset=utf-8",
        data: "{'Qry':'" + txtAreaVal + "'}",
        dataType: "json",
        async: false,
        success: function (data, status, headers, config) {

            var dlstresult = "ddlresult_" + j;
            $('[id$="' + dlstresult + '"]').empty();

            $.each(data.d, function (key, value) {

                $('[id$="' + dlstresult + '"]').append('<option value="' + value.ID + '">' + value.Name + '</option>');
            });

            $('[id$="' + dlstresult + '"]').val(value);

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                   + x.responseText);
        }
    });
}

function AddNewReport() {
    $('[id$="hdnReportId"]').val("0");
    $('[id$="ctl00_ContentPlaceHolder1_pnlReport"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlInput"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_ChartDiv"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlQuery"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_pnlGrid"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlRun"]').hide();
    $('[id$="BtnAddReport"]').hide();
    $('[id$="BtnCancelReport"]').show();
    $('[id$="hdnIsNewAdd"]').val("1");
    $("#ddlInputFiled").val("Select");
    $("#ctl00_ContentPlaceHolder1_txtName").val("");
    $("#ctl00_ContentPlaceHolder1_txtDescription").val("");
    $("#ctl00_ContentPlaceHolder1_txtQuery").val("");
    //$('input[name="' +rdbMgmtReport + '"][value="no"]').prop('checked', true);
    $('[id$="dlstChartType"]').val('Select');
    var controlsbody = "";
    //$('[id$="ctl00_ContentPlaceHolder1_ControlsDiv"]').html(controlsbody);
    $('[id$="ControlsDiv"]').empty();

}

function AlertMsg() {
    alert("Report have been deleted successfully !");
}

function disableChart() {

    $('[id$="ctl00_ContentPlaceHolder1_ChartDiv"]').hide();
}
function EnableChart() {

    $('[id$="ctl00_ContentPlaceHolder1_ChartDiv"]').show();
}

function disableChartRun() {

    $('[id$="ctl00_ContentPlaceHolder1_ChartDivRun"]').hide();
}
function EnableChartRun() {

    $('[id$="ctl00_ContentPlaceHolder1_ChartDivRun"]').show();
}


function hide() {
    $("#ctl00_ContentPlaceHolder1_RunControlsDiv").hide();
}
function changeImage() {
    var image = document.getElementById('ctl00_ContentPlaceHolder1_myImage');
    if (image.src.match("plus")) {
        image.src = "images/minus_icon.png";
        $("#ctl00_ContentPlaceHolder1_RunControlsDiv").show();
    } else {
        image.src = "images/plus_icon.png";
        $("#ctl00_ContentPlaceHolder1_RunControlsDiv").hide();
    }
}

//27/08/2015

function GenerateControls(data) {

    if (data.length > 0) {

        for (var i = 0; i < data.length; i++) {

            var row = data[i];
            var index = row.Index;
            var type = row.Type;
            var text = row.Text;
            var tarea = row.TextArea;
            var result = row.Result;

            var ddlid = "ddltype_" + index;
            var txtarea = "textarea" + index;
            var txtbox = "text" + index;
            //var dlstresult = "ddlresult_" + index;
            //var btnId = "btnresult_" + index;


            $('[id$="' + ddlid + '"]').val(type);

            if (type == 'SQL') {
                $('[id$="' + txtbox + '"]').hide();
                $('[id$="' + txtbox + '"]').val('');

                $('[id$="' + txtarea + '"]').show();
                $('[id$="' + txtarea + '"]').val(tarea);

                //$('[id$="' + dlstresult + '"]').show();
                //Bind Dropdown using SQL.
                //BindSQLDropDown(index);

                //$('[id$="' + dlstresult + '"]').val(result);

                //$('[id$="' + btnId + '"]').show();
            }
            else if (type == 'CSV') {
                $('[id$="' + txtbox + '"]').hide();
                $('[id$="' + txtbox + '"]').val('');

                $('[id$="' + txtarea + '"]').show();
                $('[id$="' + txtarea + '"]').val(tarea);

                //$('[id$="' + dlstresult + '"]').show();
                //Bind Dropdown using CSV.

                //var arrValues = $.unique(text.split(','));
                //for (var k = 0; k < arrValues.length; k++) {
                //    $('[id$="' + dlstresult + '"]').append('<option value="' + arrValues[k] + '">' + arrValues[k] + '</option>');
                //}
                //$('[id$="' + dlstresult + '"]').val(result);

                //$('[id$="' + btnId + '"]').show();
            }
            else {

                $('[id$="' + txtbox + '"]').show();
                $('[id$="' + txtbox + '"]').val(text);

                $('[id$="' + txtbox + '"]').html(text);


                $('[id$="' + txtarea + '"]').hide();
                $('[id$="' + txtarea + '"]').val('');

                //$('[id$="' + dlstresult + '"]').hide();
                //$('[id$="' + btnId + '"]').hide();
            }
        }

    }
}

function GenerateReportControls(data) {

    if (data.length > 0) {

        for (var i = 0; i < data.length; i++) {

            var row = data[i];
            var index = row.Index;
            var type = row.Type;
            var text = row.Text;
            var tarea = row.TextArea;
            var result = row.Result;

            var ddlid = "ddltype_" + index;
            var txtarea = "textarea" + index;
            var txtbox = "text" + index;
            var dlstresult = "ddlresult_" + index;
            //var btnId = "btnresult_" + index;


            $('[id$="' + ddlid + '"]').val(type);

            if (type == 'SQL') {
                $('[id$="' + txtbox + '"]').hide();
                $('[id$="' + txtbox + '"]').val('');

                $('[id$="' + txtarea + '"]').hide();
                $('[id$="' + txtarea + '"]').val(tarea);

                $('[id$="' + dlstresult + '"]').show();
                //Bind Dropdown using SQL.
                BindSQLDropDown(index, result);

                //$('[id$="' + dlstresult + '"]').val(result);

                //$('[id$="' + btnId + '"]').show();
            }
            else if (type == 'CSV') {
                $('[id$="' + txtbox + '"]').hide();
                $('[id$="' + txtbox + '"]').val('');

                $('[id$="' + txtarea + '"]').hide();
                $('[id$="' + txtarea + '"]').val(tarea);

                $('[id$="' + dlstresult + '"]').show();
                //Bind Dropdown using CSV.

                var arrValues = $.unique(tarea.split(','));
                for (var k = 0; k < arrValues.length; k++) {
                    $('[id$="' + dlstresult + '"]').append('<option value="' + arrValues[k] + '">' + arrValues[k] + '</option>');
                }
                $('[id$="' + dlstresult + '"]').val(result);

                //$('[id$="' + btnId + '"]').show();
            }
            else {

                $('[id$="' + txtbox + '"]').show();
                $('[id$="' + txtbox + '"]').val(text);

                $('[id$="' + txtbox + '"]').html(text);


                $('[id$="' + txtarea + '"]').hide();
                $('[id$="' + txtarea + '"]').val('');

                //$('[id$="' + dlstresult + '"]').hide();
                //$('[id$="' + btnId + '"]').hide();
            }
        }

    }
}
//27/08/2015


//-------------------------- for Report Page------------

function GetReportDetails() {

    $.ajax({
        type: "POST",
        url: "Report.aspx/BindReport",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {

            GetReportData(jQuery.parseJSON(msg.d));
            $('[id$="pnlInput"]').hide();
            $('[id$="pnlQuery"]').hide();
            $('[id$="pnlGrid"]').hide();
            $('[id$="ctl00_ContentPlaceHolder1_pnlRun"]').hide();
            $('[id$="BtnAddReport"]').show();
            $('[id$="BtnCancelReport"]').hide();

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetReportData(ReportData) {

    $(gridReport).kendoGrid({
        dataSource: {
            data: ReportData,
            schema: {
                model: {
                    fields: {
                        reportId: { type: "number" },
                        name: { type: "string" },
                    }
                }
            },
            pageSize: 100,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                    { field: "reportId", title: "ID", hidden: true },
                    { field: "name", title: "Name", width: "20px" },
                    { command: [{ text: "Run", click: RunDetails }], width: "25px" }
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
        }
        ,
        editable: false,
    });
}

function Cancel() {
    $('[id$="ctl00_ContentPlaceHolder1_pnlReport"]').show();
    $('[id$="ctl00_ContentPlaceHolder1_pnlGrid"]').hide();
    $('[id$="ctl00_ContentPlaceHolder1_pnlRun"]').hide();
    $('[id$="BtnCancelReport"]').hide();
    $("#ctl00_ContentPlaceHolder1_lblReportName").text('');

    GetReportDetails();
}

function disableChartRunReport() {

    $('[id$="ctl00_ContentPlaceHolder1_ChartDivRunReport"]').hide();
}

function EnableChartRunReport() {

    $('[id$="ctl00_ContentPlaceHolder1_ChartDivRunReport"]').show();
}



