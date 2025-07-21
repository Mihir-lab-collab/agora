


var columnSchema = [];
columnSchema = [{
    command: [
        {
            name: "edit", click: EditEmployee, text: ""
        },
        {
            name: "note", click: Note, text: "", title: "note", className: "ViewNote", imageClass: "ViewNote"
        },
    ], width: "36px"
},
//{ command: [{ name: "note", click: Note, text: "Note" }], width: "25px" },
{ field: "empid", title: "ID", width: "30px" },
{ field: "empName", title: "Name", width: "50px" },
{ field: "empContact", title: "ContactNo", width: "40px" },
{ field: "Designation", title: "Designation", width: "50px" },
{ field: "PrimarySkillDesc", title: "PrimarySkill", width: "50px" },
{ field: "SecSkills", title: "SecondarySkill", width: "50px" },
{ field: "skillid", title: "SkillId", width: "50px", hidden: true },
{ field: "empExperince", title: "Experience (In months)", width: "50px", hidden: true },
{ field: "Type", title: "Experience", width: "50px" },
{ field: "intelegainExperince", title: "Intelegain Experience (In months)", width: "50px", hidden: true },
{ field: "IType", title: "Intelegain Experience", width: "50px" },
{ field: "empJoiningDate", title: "JoiningDate", width: "50px", format: "{0:dd-MMM-yyyy}" },
{ field: "empExpectedLWD", title: " Exptd LWD", width: "50px", format: "{0:dd-MMM-yyyy}" },//LWD
{
    field: "AnnualCTC", title: "Annual CTC", width: "50px", attributes: { title: "Actc" }
    , template: '<div class="ra" style="text-align:right;">#= kendo.toString(AnnualCTC,"n0") #</div>'
},
{ field: "QualificationId", title: "QualificationId", width: "50px", hidden: true },
{ field: "SecSkillsId", title: "SecSkillsId", width: "50px", hidden: true },
{ field: "SecSkillsId", title: "SecSkillsId", width: "50px", hidden: true },
{ field: "EmpPAN", title: "EmpPAN", width: "50px", hidden: true },
{ field: "EmpUAN", title: "EmpUAN", width: "50px", hidden: true },
{ field: "EmpEPF", title: "EmpEPF", width: "50px", hidden: true },


];

$(document).ready(function () {
    FillSecondarySkills();
    FillQualification();
    GetEmployeeDetails();

    var a = $('#hdfProfile').val();
    //alert(a);
    $('#ctl00_ContentPlaceHolder1_txtExpectedLWD').on("change", function (e) {

        var spanJoiningDate = $('#ctl00_ContentPlaceHolder1_txtJoiningDate').val();
        var ExpectedLWD = $('#ctl00_ContentPlaceHolder1_txtExpectedLWD').val();


        var d = new Date(spanJoiningDate.split("/").reverse().join("-"));
        var dd = d.getDate();
        var mm = d.getMonth() + 1;
        var yy = d.getFullYear();
        var Finaljoiningdate = yy + "/" + mm + "/" + dd;


        var d2 = new Date(ExpectedLWD.split("/").reverse().join("-"));
        var dd2 = d2.getDate();
        var mm2 = d2.getMonth() + 1;
        var yy2 = d2.getFullYear();
        var FinalExpectedLWD = yy2 + "/" + mm2 + "/" + dd2;

        if (new Date(Finaljoiningdate) < new Date(FinalExpectedLWD)) {

        }
        else {
            alert("Expected LWD date should be greater than Joining date");
            $('#ctl00_ContentPlaceHolder1_txtExpectedLWD').val('');
        }
    });


});
function FillSecondarySkills() {
    $.ajax(
        {
            type: "POST",
            url: "Employee.aspx/BindSecondarySkills",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            dataType: "json",
            success: function (msg) {
                $('[id$="lstSecondarySkill"]').kendoMultiSelect({
                    optionLabel: "Select Secondary Skill",
                    dataTextField: "skillDesc",
                    dataValueField: "techId",
                    dataSource: jQuery.parseJSON(msg.d),

                }).data("kendoMultiSelect");
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        }
    );
}

function FillQualification() {
    $.ajax(
        {
            type: "POST",
            url: "Employee.aspx/BindQualification",
            contentType: "application/json;charset=utf-8",
            data: "{}",
            dataType: "json",
            success: function (msg) {
                $('[id$="lstempQual"]').kendoMultiSelect({
                    optionLabel: "Select Qualification",
                    dataTextField: "QualDesc",
                    dataValueField: "QID",
                    dataSource: jQuery.parseJSON(msg.d),

                }).data("kendoMultiSelect");
            },
            error: function (x, e) {
                alert("The call to the server side failed. "
                    + x.responseText);
            }
        }
    );
}


function GetEmployeeDetails() {
    var leavingstatus = $('#ctl00_ContentPlaceHolder1_dlstLeavingDate').val();

    $.ajax({
        type: "POST",
        url: "Employee.aspx/BindEmployee",
        contentType: "application/json;charset=utf-8",
        data: "{leavingstatus:'" + leavingstatus + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            //var EmpData = GetExpYearWise(jQuery.parseJSON(msg.d));
            var EmpData = jQuery.parseJSON(msg.d);

            EmpData = GetExpYearWise(EmpData);
            EmpData = GetIExpYearWise(EmpData);

            GetEmployeeData(EmpData);
            //GetEmployeeData(jQuery.parseJSON(msg.d));

        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetExpYearWise(empData) {
    empData = empData || [];
    for (var i = 0; i < empData.length; i++) {
        var exp = empData[i].empExperince;

        //var jDate = empData[i].Event;
        //var d1 = new Date(Date.parse(jDate));
        //var d2 = new Date();
        //var months = '';
        //months = (d2.getFullYear() - d1.getFullYear()) * 12;
        //months -= d1.getMonth() + 1;
        //months += d2.getMonth();
        //months = months <= 0 ? 0 : months + 1;
        //exp = exp + months;

        empData[i].Type = (Math.floor(exp / 12)).toString() + " yrs - " + (exp % 12).toString() + " mnths";

    }
    return empData;
}

function GetIExpYearWise(empData) {
    empData = empData || [];
    for (var i = 0; i < empData.length; i++) {
        var exp = empData[i].intelegainExperince;

        empData[i].IType = (Math.floor(exp / 12)).toString() + " yrs - " + (exp % 12).toString() + " mnths";
    }
    return empData;
}

function FillProfileData() {
    var LocationId = $('#ctl00_ContentPlaceHolder1_ddlLocation').val();

    $.ajax({
        type: "POST",
        url: "Employee.aspx/BindProfile",
        contentType: "application/json;charset=utf-8",
        data: "{'LocationId':' " + LocationId + "'}",
        cache: false,
        async: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var Pdata = JSON.parse(response.d);
            $('#ctl00_ContentPlaceHolder1_ddlProfile').empty();
            $("#ctl00_ContentPlaceHolder1_ddlProfile").append($("<option/>").val("0").text("--Select Profile--"));
            for (var i = 0; i < Pdata.length; i++) {
                $("#ctl00_ContentPlaceHolder1_ddlProfile").append($("<option/>").val(Pdata[i].ProfileId).text(Pdata[i].ProfileName));
            }

        },

        error: function (response) {
            alert("The call to the server side failed."
                + data.responseText);
        }
    });
}


function editBindProfile(locationID) {
    $.ajax({
        type: "POST",
        url: "Employee.aspx/BindProfile",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: "{'LocationId':' " + locationID + "'}",
        cache: false,
        async: false,
        success: function (data) {
            var Pdata = JSON.parse(data.d);
            $('#ctl00_ContentPlaceHolder1_ddlProfile').empty();
            $("#ctl00_ContentPlaceHolder1_ddlProfile").append($("<option/>").val("0").text("--Select Profile--"));
            for (var i = 0; i < Pdata.length; i++) {
                $("#ctl00_ContentPlaceHolder1_ddlProfile").append($("<option/>").val(Pdata[i].ProfileId).text(Pdata[i].ProfileName));
            }
        }

    });

}

function getValfromDDL(text, strSelector) {
    var ddl = document.getElementById(strSelector);
    //var ddl = strSelector;  
    for (var i = 0; i <= ddl.options.length - 1; i++) {
        ddl.options[i].innerHTML = ddl.options[i].innerHTML.replace(/^\s+|\s+$/g, "");
        text = text.replace(/^\s+|\s+$/g, "");
        if (ddl.options[i].innerHTML.toLowerCase() == text.toLowerCase())
            //  alert(ddl.options[i].innerHTML);
            return i;
    }
}


function GetEmployeeData(EmployeeData) {
    $(gridEmployee).empty();
    $(gridEmployee).kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "Kendo UI Grid Export.xlsx",
            allPages: true
        },
        dataSource: {
            data: EmployeeData,
            schema: {
                model: {
                    fields: {
                        empid: { type: "number" },
                        Photo: { type: "string" },
                        empName: { type: "string" },
                        empContact: { type: "string" },
                        empEmail: { type: "string" },
                        empAddress: { type: "string" },
                        empAccountNo: { type: "string" },
                        EmpPAN: { type: "string" },
                        EmpUAN: { type: "string" },
                        EmpEPF: { type: "string" },
                        empJoiningDate: { type: "date" },
                        empExpectedLWD: { type: "date" },//LWD
                        empBDate: { type: "date", format: "{dd/MMM/yyyy}" },
                        empExperince: { type: "number", hidden: true },
                        Type: { type: "string" },                           // <-- to show exp in yrs n mnths
                        intelegainExperince: { type: "number", hidden: true },
                        IType: { type: "string" },
                        empADate: { type: "date", format: "{dd/MMM/yyyy}" },
                        LocationFKID: { type: "number" },
                        empNotes: { type: "string" },
                        empPrevEmployer: { type: "string" },
                        PrimarySkill: { type: "number" },
                        Designation: { type: "string" },
                        skillid: { type: "number" },
                        empProbationPeriod: { type: "number" },
                        empLeavingDate: { type: "date", format: "{dd/MMM/yyyy}" },
                        IsSuperAdmin: { type: "boolean" },
                        IsAccountAdmin: { type: "boolean" },
                        IsPayrollAdmin: { type: "boolean" },
                        IsPM: { type: "boolean" },
                        IsProjectReport: { type: "boolean" },
                        IsProjectStatus: { type: "boolean" },
                        IsLeaveAdmin: { type: "boolean" },
                        IsActive: { type: "boolean" },
                        Tester: { type: "boolean" },
                        Resume: { type: "string" },
                        EmpStatus: { type: "string" },
                        ProjectID: { type: "number" },
                        Net: { type: "number" },
                        CTC: { type: "number" },
                        AnnualCTC: { type: "number" },
                        Gross: { type: "number" },
                        Skill: { type: "string" },
                        SecurityLevel: { type: "number" },
                        PrimarySkillDesc: { type: "string" },
                        Qualification: { type: "string" },
                        QualificationId: { type: "string", hidden: true },
                        SecSkills: { type: "string" },
                        SecSkillsId: { type: "string", hidden: true },
                        ProfileID: { type: "number" },
                        Type: { type: "string" },
                        ADUserName: { type: "string" },
                        MSTeam: { type: "string" },
                        IsRemoteEmployee: { type: "boolean"}
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable:
        {
            input: true,
            numeric: false
        },
        columns: columnSchema,

        filterable:
        {
            extra: false,
            operators:
            {
                string:
                {
                    startswith: "Starts with",
                    contains: "Contains",
                    eq: "Is equal to"
                }
            }
        },

        editable: false,

        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });
    ///////////////////////////////////////////////// tooltip
    $(".k-grid-content").kendoTooltip({
        filter: "td[title]",
        content: toolTip,
        width: 160,
        height: 30,
        position: "top"
    });

    $(".k-grid-content").click(toolTip);

    function toolTip(e) {
        var target = $(e.target);
        var dataItem = $(gridEmployee).data("kendoGrid").dataItem(e.target.closest("tr"));

        return kendo.template($("#template").html())({
            ctcText: dataItem.CTC.toLocaleString(),
            GrossText: dataItem.Gross.toLocaleString()
        });
    }

    ///////////////////////////

    detailsTemplate = kendo.template($("#popup-editor").html());
    function showDetails(e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        wnd.content(detailsTemplate(dataItem));
        wnd.center().open();
    }
}


function EditEmployee(e) {


    $('#btnTimeSheet').css('display', '');
    $('#btnEmpHistory').css('display', '');
    $('[id$="btnSave"]').val('UPDATE');
    $('#ctl00_ContentPlaceHolder1_lblUploadedName').html("");
    $('#ctl00_ContentPlaceHolder1_Appointment').html("");
   
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    openAddPopUP();

    $('#ctl00_ContentPlaceHolder1_btnSendMail').show();

    $('#ctl00_ContentPlaceHolder1_lblPExp').show();
    $('#ctl00_ContentPlaceHolder1_hdnPrevExp').val(data.empExperince);
    var empId = data.empid
    $('[id$="hdnempId"]').val(empId);

    var empstatus = "";
    if (data.empLeavingDate != null) {
        empstatus = "InActive"
        $('#ctl00_ContentPlaceHolder1_ddlEmpStatus').val(empstatus);
    }
    else {
        empstatus = "Active"
        $('#ctl00_ContentPlaceHolder1_ddlEmpStatus').val(empstatus);
    }

    var empname = data.empName
    $('#ctl00_ContentPlaceHolder1_txtEmpName').val(empname);

    if (data.LocationFKID != "") {
        $('#ctl00_ContentPlaceHolder1_ddlLocation').val(data.LocationFKID);
    }


    if (data.Designation != "") {
        $('#ctl00_ContentPlaceHolder1_empSkill').val(data.skillid);
    }

    if (data.PrimarySkill != "") {

        $('#ctl00_ContentPlaceHolder1_ddlPrimarySkill').val(data.PrimarySkill);
    }

    //if (data.empProbationPeriod != "") {

    //    $('#ctl00_ContentPlaceHolder1_empProbationPeriod').val(data.empProbationPeriod);
    //}
    $('#ctl00_ContentPlaceHolder1_empProbationPeriod').val(data.empProbationPeriod);
    $('#ctl00_ContentPlaceHolder1_txtEmail').val(data.empEmail);

    $('#ctl00_ContentPlaceHolder1_txtAddress').val(data.empAddress);
    $('#ctl00_ContentPlaceHolder1_txtCAddress').val(data.CAddress);
    $('#ctl00_ContentPlaceHolder1_txtIFSCCode').val(data.IFSCCode); /*IFSCCode*/
    //$('#ctl00_ContentPlaceHolder1_txtMSTeamID').val(data.MSTeam); /*IFSCCode*/

    if (data.empGender != '') {
        $('input[name="ctl00$ContentPlaceHolder1$rbtnGender"][value="' + data.empGender + '"]').prop('checked', true);
    }
    else {
        $('input[name="ctl00$ContentPlaceHolder1$rbtnGender"][value="Male"]').prop('checked', false);
        $('input[name="ctl00$ContentPlaceHolder1$rbtnGender"][value="Female"]').prop('checked', false);
    }

    $('#ctl00_ContentPlaceHolder1_txtContact').val(data.empContact);

    $('#ctl00_ContentPlaceHolder1_txtAccountNo').val(data.empAccountNo);

    $('#ctl00_ContentPlaceHolder1_txtPan').val(data.EmpPAN);
    $('#ctl00_ContentPlaceHolder1_txtUan').val(data.EmpUAN);
    $('#ctl00_ContentPlaceHolder1_txtEpfacno').val(data.EmpEPF);

    $('#ctl00_ContentPlaceHolder1_txtPrevEmployer').val(data.empPrevEmployer);

    $('#ctl00_ContentPlaceHolder1_txtNotes').val(data.empNotes);
    //LWD
    if (data.empExpectedLWD != "") {
        var empExpectedLWD = data.empExpectedLWD;
        var jdate = new Date(Date.parse(empExpectedLWD));
        var day = jdate.getDate();
        if (day.toString().length <= 1) {
            day = "0" + day;
        }
        var month = jdate.getMonth() + 1;
        if (month.toString().length <= 1) {
            month = "0" + month;
        }
        var year = jdate.getFullYear();
        var empExpectedLWD = day + '/' + month + '/' + year;
        if (empExpectedLWD == "NaN/NaN/NaN") {
            $('#ctl00_ContentPlaceHolder1_txtExpectedLWD').val('');

        } else {
            $('#ctl00_ContentPlaceHolder1_txtExpectedLWD').val(empExpectedLWD);
        }

    } else {
        alert("expected date empty");
        $('#ctl00_ContentPlaceHolder1_txtExpectedLWD').val('');
    }

    if (data.empJoiningDate != "") {
        var JoiningDate = data.empJoiningDate;
        var jdate = new Date(Date.parse(JoiningDate));
        var day = jdate.getDate();
        if (day.toString().length <= 1) {
            day = "0" + day;
        }
        var month = jdate.getMonth() + 1;
        if (month.toString().length <= 1) {
            month = "0" + month;
        }
        var year = jdate.getFullYear();
        var joiningdate = day + '/' + month + '/' + year;

        $('#ctl00_ContentPlaceHolder1_txtJoiningDate').val(joiningdate);

        var JoiningDate = $('[id$="txtJoiningDate"]').data("kendoDatePicker");
        var LeavingDate = $('[id$="txtLeavingDate"]').data("kendoDatePicker");
        LeavingDate.min(jdate);
    }

    if (data.empLeavingDate != null) {

        var LeavingDate = data.empLeavingDate;
        var ldate = new Date(Date.parse(LeavingDate));
        var lday = ldate.getDate();
        if (lday.toString().length <= 1) {
            lday = "0" + lday;
        }
        var lmonth = ldate.getMonth() + 1;
        if (lmonth.toString().length <= 1) {
            lmonth = "0" + lmonth;
        }
        var lyear = ldate.getFullYear();
        var leavingdate = lday + '/' + lmonth + '/' + lyear;

        $('#ctl00_ContentPlaceHolder1_txtLeavingDate').val(leavingdate);
    }
    else {
        $('#ctl00_ContentPlaceHolder1_txtLeavingDate').val('');
    }

    if (data.empBDate != null) {
        var BirthDate = data.empBDate;
        var bdate = new Date(Date.parse(BirthDate));
        var bday = bdate.getDate();
        if (bday.toString().length <= 1) {
            bday = "0" + bday;
        }
        var bmonth = bdate.getMonth() + 1;
        if (bmonth.toString().length <= 1) {
            bmonth = "0" + bmonth;
        }
        var byear = bdate.getFullYear();
        var birthdate = bday + '/' + bmonth + '/' + byear;

        $('#ctl00_ContentPlaceHolder1_txtBirthDate').val(birthdate);
    }
    else {
        $('#ctl00_ContentPlaceHolder1_txtBirthDate').val('');
    }

    if (data.empADate != null) {
        var AnniversaryDate = data.empADate;
        var adate = new Date(Date.parse(AnniversaryDate));
        var aday = adate.getDate();
        if (aday.toString().length <= 1) {
            aday = "0" + aday;
        }
        var amonth = adate.getMonth() + 1;
        if (amonth.toString().length <= 1) {
            amonth = "0" + amonth;
        }
        var ayear = adate.getFullYear();
        var anniversarydate = aday + '/' + amonth + '/' + ayear;

        $('#ctl00_ContentPlaceHolder1_txtAnniversaryDate').val(anniversarydate);
    }
    else {
        $('#ctl00_ContentPlaceHolder1_txtAnniversaryDate').val('');
    }


    var SecSkills = $("#ctl00_ContentPlaceHolder1_lstSecondarySkill").data("kendoMultiSelect");
    SecSkills.dataSource.filter({});
    SecSkills.value(data.SecSkillsId.split(","));



    var Qualification = $("#ctl00_ContentPlaceHolder1_lstempQual").data("kendoMultiSelect");
    Qualification.dataSource.filter({});
    Qualification.value(data.QualificationId.split(","));

    $("#ctl00_ContentPlaceHolder1_photoImage").attr('src', "/Member/Services/ViewImage.ashx?id=" + empId);


    var cExp = 0;
    //if (data.empExperince > 0)
    //    cExp = data.empExperince;

    if (data.intelegainExperince > 0)
        cExp = data.intelegainExperince;

    //Previous Experience in months
    var pExp = data.empExperince;

    //Get current exp in months after joining Date.

    //var jDate = data.empJoiningDate
    //var d1 = new Date(Date.parse(jDate));
    //var d2 = new Date();
    //var months = '';
    //months = (d2.getFullYear() - d1.getFullYear()) * 12;
    //months -= d1.getMonth() + 1;
    //months += d2.getMonth();
    //months = months <= 0 ? 0 : months + 1;    ///added +1


   // cExp = cExp + months;

    var cYears = Math.floor(cExp / 12);
    var cMonths = cExp % 12;
    $('#ctl00_ContentPlaceHolder1_dropempExpyears').val(cYears);
    $('#ctl00_ContentPlaceHolder1_dropempExpmonths').val(cMonths);

    //Pervious Experience
    var pYears = Math.floor(pExp / 12);
    var pMonths = pExp % 12;
    var pTotalExp = pYears.toString() + '.' + pMonths.toString();

    $('#ctl00_ContentPlaceHolder1_lblPreExp').text(pTotalExp);


    if (data.Resume != '')
    {
        $('#ctl00_ContentPlaceHolder1_lblUploadedName').attr('visible', true);
        var extension = data.Resume.substr((data.Resume.lastIndexOf('.') + 1));
        var strFileName;
        if (extension == "docx")
        {
            extension = "doc";
            strFileName = empId.toString() + '_' + empname + '.' + extension;
        }
        else {
            strFileName = empId.toString() + '_' + empname + '.' + extension;
        }
        $('#ctl00_ContentPlaceHolder1_lblUploadedName').append("<a href='../Common/Download.aspx?m=CV&f=" + strFileName + "'>" + "Download Attachment</a>");
    }
    if (data.Resume != '')
    {
        $('#ctl00_ContentPlaceHolder1_Appointment').attr('visible', true);
       // var extension = data.Resume.substr((data.Resume.lastIndexOf('.') + 1));//docx
        var extension="pdf"
       
        var strFileName1;
        if (extension == "pdf")
        {
            //extension = "doc";
            //strFileName1 = empId.toString() + '_' + empname + '.' + extension;
            strFileName1 = "Employee_Appointment" + "_" + empId.toString() + '.' + extension;
            $('#ctl00_ContentPlaceHolder1_Appointment').append("<a href='../Common/Download.aspx?m=AL&f=" + strFileName1 + "'>" + "Download Appointment</a>");
        }
        else {
           // strFileName1 = empId.toString() + '_' + empname + '.' + extension;
            strFileName1 = "Employee_Appointment" + "_" + empId.toString() + '.'+ extension;
        }
        //"Employee_Appointment" + "_" + intEmpID + fileExtension1
       
    }



    if (data.ProfileID > 0) {
        editBindProfile(data.LocationFKID)
        $('#ctl00_ContentPlaceHolder1_ddlProfile').val(data.ProfileID);
        $('#ctl00_ContentPlaceHolder1_hfProfileID').val(data.ProfileID);
    }
    else {
        editBindProfile(data.LocationFKID)
        $('#ctl00_ContentPlaceHolder1_ddlProfile').val('');
        $('#ctl00_ContentPlaceHolder1_hfProfileID').val('');
    }

    $('#ctl00_ContentPlaceHolder1_txtADUserName').val(data.ADUserName);

    if ($('[id$="hdfProfile"]').val() == "False") {
        $('#ctl00_ContentPlaceHolder1_ddlProfile').prop("disabled", true);
        $("#ctl00_ContentPlaceHolder1_txtADUserName").prop("readonly", true);
    }
    if (data.MSTeam != "") {
        $('#ctl00_ContentPlaceHolder1_txtMSTeamID').val(data.MSTeam);
    }
    $('#ctl00_ContentPlaceHolder1_chkRemote').prop('checked', data.IsRemoteEmployee == 1);
}

function Note(e) {


    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var noteType = "Employee"
    var mode = "GetNoteTypeId";


    var refId = data.empid;

    $.ajax({
        type: "POST",
        url: "Employee.aspx/GetNoteTypeId",
        contentType: "application/json;charset=utf-8",
        data: "{mode:'" + mode + "',noteType:'" + noteType + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            var id = data.d;
            Redirect(id, refId)
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });

}




function Redirect(id, refId) {
    window.location = "../Member/Notes.aspx?TypeId=" + id + "&RefID=" + refId;
}

function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    $('#ctl00_ContentPlaceHolder1_btnSendMail').hide();
    $('#ctl00_ContentPlaceHolder1_lblPExp').hide();
    BindAllDateCalender();
}
function CloseTimeSheet() {
    //$('#divTimeSheet').css('display', 'none');
    $('#divTimeSheet').hide(500);
}

function closedoInactiveUsers() {
    $('#divdoInactiveUsers').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function closeAddPopUP() {
    $('#divEmpHistory').css('display', 'none');
    $('#divTimeSheet').css('display', 'none');
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    clearFields();
    //window.location.reload();


}
function ClosingRateWindow(e) {

    var grid = $(gridEmployee).data("kendoGrid");
    grid.refresh();

}

function Refresh() {
    window.location.reload();
}

function BindAllDateCalender() {
    $('[id$="txtJoiningDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtLeavingDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtAnniversaryDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtBirthDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    $('[id$="txtExpectedLWD"]').kendoDatePicker({ format: "dd/MM/yyyy" });

}


function ShowAddPopup() {
    openAddPopUP();
    clearFields();
    $("#ctl00_ContentPlaceHolder1_ddlEmpStatus").prop("disabled", true);
    $('[id$="txtLeavingDate"]').kendoDatePicker("enable", false);
    $('#btnTimeSheet').css('display', 'none');
    $('#btnEmpHistory').css('display', 'none');
}

function openReportPopup() {
    $('#divGenerateReport').css('display', '');
    $('#divReportPopupOverlay').addClass('k-overlay');
}

function clearFields() {
    $('[id$="txtEmpName"]').val('');
    $('[id$="txtAddress"]').val('');
    $('[id$="txtEmail"]').val('');
    $('[id$="txtContact"]').val('');
    $('[id$="txtAccountNo"]').val('');
    $('[id$="txtPan"]').val('');
    $('[id$="txtUan"]').val('');
    $('[id$="txtEpfacno"]').val('');
    $('[id$="txtPrevEmployer"]').val('');
    $('[id$="txtNotes"]').val('');
    $('[id$="txtJoiningDate"]').val('');
    $('[id$="ddlEmpStatus"]').val('Active');
    $('[id$="txtExpectedLWD"]').val('');
    $('[id$="txtJoiningDate"]').val('');
    $('[id$="txtIFSCCode"]').val(''); /*ifsc*/
    $('[id$="txtMSTeamID"]').val('');

    $("table[id$=rbtnGender] input:radio:checked").removeAttr("checked");

    $('[id$="txtLeavingDate"]').val('');
    $('[id$="txtBirthDate"]').val('');
    $('[id$="txtAnniversaryDate"]').val('');
    $('[id$="txtExpectedLWD"]').val('');
    $("#ctl00_ContentPlaceHolder1_ddlLocation").val("0");
    $('[id$="empSkill"]').val('100');
    $('[id$="ddlPrimarySkill"]').val('0');
    $('[id$="empProbationPeriod"]').val("");

    $('[id$="dropempExpyears"]').val('');
    $('[id$="dropempExpmonths"]').val('');
    $('[id$="lblPreExp"]').html('');

    $('[id$="photoImage"]').attr('src', '');


    $("#ctl00_ContentPlaceHolder1_lstSecondarySkill").data("kendoMultiSelect").value([]);
    $("#ctl00_ContentPlaceHolder1_lstempQual").data("kendoMultiSelect").value([]);
    $('[id$="hdnDocName"]').val('');
    

    $('[id$="hdnempId"]').val('');
    $('[id$="hndExperienceStore"]').val('');
    $('[id$="hfSecondarySkills"]').val('');
    $('[id$="hfQualification"]').val('');

    $('[id$="btnSave"]').val('SAVE');


}

function FillEmployeeData() {
    columnSchema = [];

    columnSchema.push({ field: "SkillId", title: "SkillId", width: "50px", hidden: true });

    $('input[class=chkbox]').each(function () {
        if ($(this).is(":checked")) {
            var stField = $(this).val();
            var stTitle = $(this).attr('title');
            if (stTitle.toLowerCase().indexOf("date") >= 0)

                columnSchema.push({ field: stField, title: stTitle, format: "{0:dd-MMM-yyyy}", width: "50px" });
            else
                columnSchema.push({ field: stField, title: stTitle, width: "50px" });

        }
    });
    GetEmployeeDetails();

    closeReportPopUP();

    $('[id$="btnRefresh"]').css('display', 'block');

}

function ResetEmployeeData() {
    columnSchema = [];
    columnSchema = [{
        command: [
            {
                name: "edit", click: EditEmployee, text: ""
            },
            {
                name: "note", click: Note, text: "", title: "note", className: "ViewNote", imageClass: "ViewNote"
            },
        ], width: "32px"
    },

    //{ command: [{ name: "note", click: Note, text: "Note" }], width: "25px" },
    { field: "empid", title: "ID", width: "30px" },
    { field: "empName", title: "Name", width: "50px" },
    { field: "empContact", title: "ContactNo", width: "50px" },
    { field: "Designation", title: "Designation", width: "50px" },
    { field: "PrimarySkillDesc", title: "PrimarySkill", width: "50px" },
    { field: "skillid", title: "SkillId", width: "50px", hidden: true },
    { field: "empExperince", title: "Experience", width: "50px", hidden: true },
    { field: "Type", title: "Experience", width: "50px" },
    { field: "empJoiningDate", title: "JoiningDate", width: "50px", format: "{0:dd-MMM-yyyy}" },
    { field: "empExpectedLWD", title: "empExpectedLWD", width: "50px", format: "{0:dd-MMM-yyyy}" },//LWD
    { field: "AnnualCTC", title: "Annual CTC", width: "50px" },
    { field: "QualificationId", title: "QualificationId", width: "50px", hidden: true },
    { field: "SecSkillsId", title: "SecSkillsId", width: "50px", hidden: true },
    { field: "EmpPAN", title: "EmpPAN", width: "50px", hidden: true },
    { field: "EmpUAN", title: "EmpUAN", width: "50px", hidden: true },
    { field: "EmpEPF", title: "EmpEPF", width: "50px", hidden: true },


    ];
    GetEmployeeDetails();
}

function closeReportPopUP() {
    $('#divGenerateReport').css('display', 'none');
    $('#divReportPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

// --------------------------------------------------------------------------------------EMPLOYEE TIMESHEET BTNTIMESHEET

function opentimesheet() {
    GetTimeSheet();
    $('[id$="divTimeSheet"]').toggle(500);

}
var eventData = "";
function GetTimeSheet() {
    var empID = $('[id$="hdnempId"]').val();
    $.ajax({
        type: "POST",
        url: "Employee.aspx/GetTimeSheet",
        contentType: "application/json;charset=utf-8",
        data: "{empID:'" + parseInt(empID) + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            eventData = jQuery.parseJSON(msg.d);
            BindTimeSheet(eventData);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function BindTimeSheet(EventData) {

    $(gridTimeSheet).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        empName: { type: "string" },
                        empNotes: { type: "string" },
                        Net: { type: "number" }
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
        //change: onChange,
        columns: [
            {
                field: "empName", title: "Date", width: "80px"

            },
            {
                field: "empNotes", title: "Hours", width: "80px"
                , attributes: { class: "#=Net == 1 ? 'red' : 'black' #" }
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
        editable: false,

    });

}

//-------------------------------------------------------------------------------------- EMPLOYEE ADDRESS HISTORY
var approvalStatus = 0;
var HID = 0;
function ApproveReject(status, hid, cAddress) {
    if (status == "Approve") {
        approvalStatus = 1; //Approve
        HID = hid;
    }
    else if (status == "Reject") {
        approvalStatus = 2; //Reject
        HID = hid;
    }
}

function CopyAddress() {
    if ($('[id$="chkCopyAdd"]').prop("checked"))
        $('[id$="txtCAddress"]').val($('[id$="txtAddress"]').val());
    else
        $('[id$="txtCAddress"]').val('');
}
function CloseEmpHistory() {
    $('#divEmpHistory').css('display', 'none');
}

function openEmpHistory() {
    GetEmpHistory();
    //$("#divEmpHistory").Show();
    $('[id$="divEmpHistory"]').css('display', '');
}

function GetEmpHistory() {
    var empID = $('[id$="hdnempId"]').val();
    $.ajax({
        type: "POST",
        url: "Employee.aspx/GetEmpHistory",
        contentType: "application/json;charset=utf-8",
        data: "{empID:'" + parseInt(empID) + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            eventData = jQuery.parseJSON(msg.d);
            BindEmpHistory(eventData);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function BindEmpHistory(EventData) {

    $(gridEmpHistory).kendoGrid({
        dataSource: {
            data: EventData,
            schema: {
                model: {
                    fields: {
                        HistoryID: { type: "number" },
                        CAddress: { type: "string" },
                        empContact: { type: "string" },
                        InsertedIP: { type: "string" },
                        //empADate: { type: "date" },
                        empName: { type: "string" },
                        ModifiedIP: { type: "string" },
                        EmpStatus: { type: "string" }
                    }

                }
            },
            pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        selectable: true,
        resizable: true,
        pageable: {
            input: true,
            numeric: false
        },
        //change: onChange,
        columns: [
            //{ empADate: "CAddress", title: "empADate", format: "{0:dd/MM/yyyy}",hidden:true },
            {
                field: "CAddress", title: "Current Address", width: "180px"
            },
            {
                field: "empContact", title: "Contact", width: "50px"
            },
            {
                field: "InsertedIP", title: "Anniversary Date", width: "60px"
            },
            {
                field: "empName", title: "Modified By", width: "50px"
            },
            {
                field: "ModifiedIP", title: "Modified On", width: "50px"
            },
            {
                field: "EmpStatus", width: "80px", click: ApproveReject,
                template: "<label>Approved<input id='Approve' name='group'  type='radio' onclick='ApproveReject(id,#: HistoryID #)' value='#: EmpStatus #' #= EmpStatus== '1' ? 'checked' : ''# >" +
                    "<label>Rejected<input id='Reject' name='group'  type='radio' onclick='ApproveReject(id,#: HistoryID #)' value='#: EmpStatus #' #= EmpStatus== '2' ? 'checked' : ''# >"
            },
            //{
            //    field: "empNotes", title: "Hours", width: "80px"
            //    , attributes: { class: "#=Net == 1 ? 'red' : 'black' #" }
            //},

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
        editable: false,

    });

}

function SaveApprovedData() {
    if (!confirm("Are you sure ,you want to save the changes?"))
        return;

    var empID = $('[id$="hdnempId"]').val();
    $.ajax({
        type: "POST",
        url: "Employee.aspx/SaveApprovedData",
        contentType: "application/json;charset=utf-8",
        data: "{empID:'" + parseInt(empID) + "',HID:'" + parseInt(HID) + "',approvalStatus:'" + parseInt(approvalStatus) + " '}",
        dataType: "json",
        async: false,
        success: function (msg) {
            //alert("save successfully");
            if (approvalStatus == 1) {
                for (var i = 0; i < eventData.length; i++) {
                    if (eventData[i].HistoryID == HID) {
                        $('[id$="txtCAddress"]').val(eventData[i].CAddress);
                        $('[id$="txtContact"]').val(eventData[i].empContact);

                        $('[id$="txtAnniversaryDate"]').val(eventData[i].empAccountNo);
                    }
                }
            }
            $('[id$="divEmpHistory"]').css('display', 'none');
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}


