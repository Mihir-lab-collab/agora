$(document).ready(function () {
    GetAppraisalData();
});

function GetAppraisalData() {
    $.ajax({
        type: "POST",
        url: "EmployeeCVUpload.aspx/GeEmployeeData",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: true,
        success: function (msg) {
            var EmpData = GetExpYearWise(jQuery.parseJSON(msg.d));

            GetEmployeeGridBind(EmpData);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetEmployeeGridBind(Tdata) {
    var SelectedIdList = "";
    var SelectAllIdList = "";
    var SelectAllId = "";
    var IsStatusPending = "";

    var grid = $("#GetApprGrid").kendoGrid({
        dataSource: new kendo.data.DataSource({
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        empid: { type: "number", editable: false },
                        empName: { type: "string", editable: false },
                        SkillDesc: { type: "string", editable: false },
                        PrimarySkill: { type: "string", editable: false },
                        empExperince: { type: "number", hidden: true },
                        Type: { type: "string" },

                        //LastUploadedDate: { type: "date", editable: false },
                        //LastUploadedBy: { type: "string", editable: false },

                        LastUploadedDate: { type: "date", hidden: true },
                        LastUploadedBy: { type: "string", hidden: true },
                        empAddress: { type: "string", hidden: true },
                        projectsWorkingOn: { type: "string", hidden: true },
                        skillMatrixs: { type: "string", hidden: true },


                    }
                },

            },
            pageSize: 50, //[*Note :- If you going to change page size you need also change in "$("#IsallCheck").change()" in same file.]
        }),

        scrollable: true,
        sortable: true,
        selectable: "multiple",
        pageable: { input: true, numeric: false },
        columns: [

                    { field: "empid", width: 100, title: "ID" },
                    { field: "empName", width: 150, title: "Employee Name" },
                    { field: "SkillDesc", width: 150, title: "Designation" },
                   { field: "PrimarySkill", width: 150, title: "Primary Skill " },
                    { field: "empExperince", title: "Experience (In months)", hidden: true },
                    { field: "Type", width: 120, title: "Experience" },
                     { field: "projectsWorkingOn", width: 200, title: "Projects Working On " },
                    { field: "Employee", title: "Download CV", width: 120, template: '<a href="../Common/Download.aspx?m=CVCollection&f=#=Employee#">#= Employee #</a>' },
                    { field: "ResumePath", hidden: true },

                    //{ field: "LastUploadedDate", width: 120, title: "Last Uploaded Date", format: "{0:dd-MMM-yyyy}" },
                    //{ field: "LastUploadedBy", width: 120, title: "Last UploadedBy" },
                   { field: "LastUploadedDate", hidden: true },
                   { field: "LastUploadedBy", hidden: true },
                   { field: "empAddress", hidden: true },

                   { field: "skillMatrixs", hidden: true },
            {
                command: [
                               {
                                   name: "edit", click: EditEmployee, text: ""
                               }
                ],

                width: "100px", attributes: { style: "text-align:center;" }
            }

        ],

        change: function (arg) {
            $("#lblAddress").html('');
            $("#lblSkillMatrix").html('');
            $("#lblProjectWorkingOn").html('');
            $("#lblLastUpdatedate").html('');
            $("#lblLadtUpdateBy").html('');
            var gview = $("#GetApprGrid").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());
            var empAddress = selectedItem.empAddress;
            if (empAddress != "") {
                $("#lblAddress").html(selectedItem.empAddress);
            }
            else {
                $("#lblAddress").html('');
            }
            var line = '-';
            var skillMatrixs = selectedItem.skillMatrixs;
            if (skillMatrixs != "") {
                var res = skillMatrixs;
                res = res.replace(/\-/g, "<br/>" + line + "  ")
                var projskillMatrixs = res.replace("<br/>", "");
                $("#lblSkillMatrix").html(projskillMatrixs);
            }
            else {
                $("#lblSkillMatrix").html('')
            }
            var projectsWorkingOn = selectedItem.projectsWorkingOn;
            if (projectsWorkingOn != "") {
                var res = projectsWorkingOn;
                res = res.replace(/\-/g, "<br/>" + line + "  ")
                var projWorkingOn = res.replace("<br/>", "");
                $("#lblProjectWorkingOn").html(projWorkingOn);
            }
            else {
                $("#lblProjectWorkingOn").html('')
            }
            var LastUploadedDate = selectedItem.LastUploadedDate;
            if (LastUploadedDate == null || LastUploadedDate == "") {
                LastUploadedDate = "";
            }

            if (LastUploadedDate != "") {
                var LastUploadedDate = new Date(selectedItem.LastUploadedDate);
                var year = LastUploadedDate.getFullYear();
                var day = LastUploadedDate.getDate();
                var monthName = new Array();
                monthName[0] = "Jan";
                monthName[1] = "Feb";
                monthName[2] = "Mar";
                monthName[3] = "Apr";
                monthName[4] = "May";
                monthName[5] = "Jun";
                monthName[6] = "Jul";
                monthName[7] = "Aug";
                monthName[8] = "Sep";
                monthName[9] = "Oct";
                monthName[10] = "Nov";
                monthName[11] = "Dec";
                var month = monthName[LastUploadedDate.getMonth()];
                LastUploadedDate = day + "-" + month + "-" + year;
                $("#lblLastUpdatedate").html(LastUploadedDate);
            }
            else {

                $("#lblLastUpdatedate").html('');

            }

            var LastUploadedBy = selectedItem.LastUploadedBy;
            if (LastUploadedBy != "") {
                $("#lblLadtUpdateBy").html(selectedItem.LastUploadedBy);
            }
            else {
                $("#lblLadtUpdateBy").html('');
            }
            openAdddPopUP();
        },
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

    }).data("kendoGrid");

    dataSource.read();
}

function GetExpYearWise(empData) {
    for (var i = 0; i < empData.length ; i++) {
        var exp = empData[i].empExperince;

        var resume = empData[i].ResumePath;

        var jDate = empData[i].empJoiningDate;
        var d1 = new Date(Date.parse(jDate));
        var d2 = new Date();
        var months = '';
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth() + 1;
        months += d2.getMonth();
        months = months <= 0 ? 0 : months + 1;
        exp = exp + months;

        empData[i].Type = (Math.floor(exp / 12)).toString() + " yrs - " + (exp % 12).toString() + " mnths";

        empData[i].extension = resume.substr((resume.lastIndexOf('.') + 1));

        empData[i].extensionnew = ('.') + empData[i].extension;

        if (resume != "") {

            empData[i].Employee = empData[i].empid + empData[i].extensionnew
        }
        else {
            empData[i].Employee = "";
        }
    }
    return empData;
}

function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function EditEmployee(e) {
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);
    var empId = data.empid
    var empname = data.empName
    $('[id$="hdnempId"]').val(empId);
    $('#ctl00_ContentPlaceHolder1_lblempName').text(empname);
    openAddPopUP();

    if (data.ResumePath != '') {
        $('#ctl00_ContentPlaceHolder1_lblUploadedName').attr('visible', true);
        var extension = data.ResumePath.substr((data.ResumePath.lastIndexOf('.') + 1));
        var strFileName = empId.toString() + '.' + extension;
        $('#ctl00_ContentPlaceHolder1_lblUploadedName').append("<a href='../Common/Download.aspx?m=CVCollection&f=" + strFileName + "'>" + "Download Attachment</a>");
    }
 

}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function closeAdddPopUP() {
    $('#divPopup').css('display', 'none');
}

function openAdddPopUP() {
    $('#divPopup').css('display', '');

}

