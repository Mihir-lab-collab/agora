$(document).ready(function () {
    //var invoices;
    //if (str == 'True') {
        //    invoices = 'All';
    GetTeamDetails();
    //}


});


function fnCompletedProjects(checked) {
    
    //var includePaidInvoices;
    //var empId = document.getElementsByClassName('hdnval')[0].value;
    //if (str == 'True') {
    GetTeamDetails();
    //}

   

}

function GetTeamDetails() {
    var empId = document.getElementsByClassName('hdnval')[0].value;
    
    var chkCompletedProjects = document.getElementById("chkCompletedProjects").checked;
    if (chkCompletedProjects) {
        includeMyprojects = "Y";
    }
    else {
        includeMyprojects = "N";
    }
    $.ajax({

        type: "POST",
        url: "MyTeam.aspx/BindMyTeam",
        contentType: "application/json;charset=utf-8",
        data: "{'empId':'" + empId + "','includeMyprojects':'" + includeMyprojects + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTeamData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}



function GetTeamData(Tdata) {
    $(GridMyTeam).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        empId: { type: "number", editable: false },
                        empName: { type: "string" },
                        designation: { type: "string" },
                        primarySkill: { type: "string" },
                        SecondarySkill: { type: "string" },
                        experience: { type: "string" },
                        projectsWorkingOn: { type: "string" },
                        
                    }
                }
            },
            pageSize: 500,
        },
        scrollable: true,
        sortable: true,
        //height: 600,
        //toolbar: ["create"],
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                     //editor: projectNameEditor, min: 1
                    //{ field: "projId", title: "Project ID", width: "80px" },
                    { field: "empId", title: "Employee-Id", width: "60px" },
                    { field: "empName", title: "Employee Name", width: "100px" },
                    { field: "designation", title: "Designation", width: "100px" },
                    { field: "primarySkill", title: "Primary Skill", width: "100px" },
                    { field: "SecondarySkill", title: "Secondary Skill", width: "100px" },
                    { field: "experience", title: "Experience", width: "100px" },
                    { field: "projectsWorkingOn", title: "Projects Working On", width: "180px"},
                   

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
       
        
    });

}


