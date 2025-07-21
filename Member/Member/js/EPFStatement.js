
$(document).ready(function () {

    $("#txtStartDate").kendoDatePicker({
        depth: "year",
        start: "year", format: "MM/yyyy", change: DateSelect
    });

    var d = new Date();

    $("#txtStartDate").val(d.getMonth() + "/" + d.getFullYear());

    if ($("#txtStartDate").val() == '') {
        $('#dvall').css('display', 'block');
        $('#dvbg').css('display', 'block');
    }
    else {
        $('#dvall').css('display', 'none');
        $('#dvbg').css('display', 'none');
        DataBind($("#txtStartDate").val());
    }
});

function DateSelect() {

    var data = $("#txtStartDate").val();

    if ($("#txtStartDate").val() == '') {
        $('#dvall').css('display', 'block');
        $('#dvbg').css('display', 'block');
    }
    else {
        $('#dvall').css('display', 'none');
        $('#dvbg').css('display', 'none');
        DataBind(data);
    }

}


function DataBind(data) {
    $.ajax(
           {
               type: "POST",
               url: "EPF_Statement.aspx/GetEpfStatement",
               contentType: "application/json;charset=utf-8",
               data: "{'data':'" + data + "'}",
               dataType: "json",
               async: true,
               success: function (msg) {
                   if (msg.d == "") {
                       alert("No record found");
                   }
                   GetData((msg.d));
               },
               error: function (x, e) {
                   alert("The call to the server side failed. "
                         + x.responseText);
               }
           }
     );
}

function GetData(Tdata) {
    $("#gridProject").kendoGrid({
        //toolbar: ["excel"],
        //excel: {
        //    fileName: "EPFStatement_Report.xlsx"
        //},
        height: 500,
        dataSource: {
            data: Tdata,//jQuery.parseJSON(msg.d),
            aggregate: [{ field: "Basic", aggregate: "sum" },
                { field: "EPSWages", aggregate: "sum" },
                { field: "PF", aggregate: "sum" },
                { field: "EPSContribution", aggregate: "sum" },
                { field: "BalenceER", aggregate: "sum" }

            ],
            schema: {
                model: {
                    fields: {
                        empId: { type: "string" },
                        EmployeeName: { type: "string" },
                        Basic: { type: "int" },
                        EPSWages: { type: "int" },
                        PF: { type: "int" },
                        EPSContribution: { type: "int" },
                        BalenceER: { type: "int" },
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
        columns: [
        { field: "empId", title: "ID", width: 50, },

        { field: "EmployeeName", title: "Name", width: 120, },// footerTemplate: "Total" },
        {
            field: "Basic", title: "Basic", width: 120,
            template: '<div class="ra">#= kendo.toString(Basic,"n0") #</div>'
        },
        {
            field: "EPSWages", title: "EPS Wages", width: 120,
            template: '<div class="ra">#= kendo.toString(EPSWages,"n0") #</div>'
        },
        {
            field: "PF", title: "PF", width: 120,
            template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>'
        },
        {
            field: "EPSContribution", title: "EPS Contribution", width: 120,
            template: '<div class="ra">#= kendo.toString(EPSContribution,"n0") #</div>'
        },
        {
            field: "BalenceER", title: "Balence ER", width: 120,
            template: '<div class="ra">#= kendo.toString(BalenceER,"n0") #</div>'
        },
        ],
        dataBound: function () {
            var grid = this;
            var model;
            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);
                if (model.EmployeeName == "TOTAL") {
                    $(this).css('font-weight', 'bold');
                }

            });
        },
        editable: false,

        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },

        filterMenuInit: function (e) {
            if (e.field == "name") {
                var firstValueDropDown = e.container.find("select:eq(0)").data("kendoDropDownList");
                firstValueDropDown.value("contains");
                var logicDropDown = e.container.find("select:eq(1)").data("kendoDropDownList");
                logicDropDown.value("or");
                var secondValueDropDown = e.container.find("select:eq(2)").data("kendoDropDownList");
                secondValueDropDown.value("contains");
            }
        },

        // Add &  edit
        save: function (e) {
            //
            var id = "";
            if (e.model.isNew()) {
                id = e.model.projId;

                if ($('#txtEditProjectTile').val().length == 0) {
                    alert('Please fill Project Title');
                    e.preventDefault();
                }
                else {
                    UpdateProjectProjTeam(id, e.model.projName, e.model.empId, e.model.empName, $('#cboDiscount').data("kendoMultiSelect").value().toString());
                    ClosingRateWindow(e);
                    window.location.href = window.location.href;
                }
            }
            else {
                id = "0";
            }
        },
    });
    detailsTemplate = kendo.template($("#popup-editor").html());
    function showDetails(e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        wnd.content(detailsTemplate(dataItem));
        wnd.center().open();
    }
}