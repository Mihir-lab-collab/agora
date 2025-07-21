$(document).ready(function () {
    var inccompleted;
    var str = document.getElementsByClassName('hdnval')[0].value;
    inccompleted = '';
    GetTaskDetails(inccompleted);


    GetPagesize();
    $(document).on("click", ".k-grid-edit", function (event) {
        var BugID = $(this).parents('tr:first').find('td:first').text();
        GetAttachment(BugID);
    });
    $("#files").kendoUpload({
        async: {
            saveUrl: "MyProjects.aspx",
            removeUrl: "MyProjects.aspx",
            //removeField : fileNames,
            autoUpload: false
        },
        //cancel: onCancel,
        //complete: onComplete,
        error: onError,
        //progress: onProgress,
        //remove: onRemove,
        select: onSelect,
        //success: onSuccess,
        // upload: onUpload       
    });


});


function fnincludeall(checked) {

    if (checked) {
        inccompleted = 'Completed';
        GetTaskDetails(inccompleted);
    }
    else {
        inccompleted = '';
        GetTaskDetails(inccompleted);
    }


}
///////////////////////////////////// Start Bind Grid////////////////////////////////////////////////////////
function GetDeveloperDetails(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/BindTeam",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (msg) {
            GetTeamMember(jQuery.parseJSON(msg.d));

        },
        error: function (result) {
            alert("Error");
        }
    });
}


function GetDevelopmentTeam(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/BindAddTeam",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (msg) {


        },
        error: function (result) {
            alert("Error");
        }
    });
}

function FillDeveloperDropDown(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/BindTeam",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (msg) {
            GetTeamMember(jQuery.parseJSON(msg.d));

        },
        error: function (result) {
            alert("Error");
        }
    });
}

function GetProjectDetails(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/BindProjectDetails",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (data) {
            var obj = jQuery.parseJSON(data.d);

            $("#lblprjName").html(obj[0].projectname);
            $("#lblprojStatus").html(obj[0].projectstatus);
            $("#lblCustName").html(obj[0].customername);
            $("#lblExpProjStatus").html(obj[0].expprojectstatus);
            $("#lblCustAddress").html(obj[0].customeraddress);
            $("#lblStartDate").html(formatJSONDate(obj[0].startdate));
            $("#lblProjDurat").html(obj[0].projectduration);
            $("#lblExpDate").html(formatJSONDate(obj[0].expcompletiondate));
            $("#lblProjMang").html(obj[0].projectmanager);
            $("#lblActCompDate").html(formatJSONDate(obj[0].actcompletiondate));
        },
        error: function (result) {
            alert("Error");
        }
    });
}

function GetProjectStatus(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/projectStatus",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (msg) {
            GetEmployeeStatus(jQuery.parseJSON(msg.d));

        },
        error: function (result) {
            alert("Error");
        }
    });
}

function GetProjectStatusDetails(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/BindProjectDetails",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (data) {
            var obj = jQuery.parseJSON(data.d);

            $("#lblProjectname").html(obj[0].projectname);
            $("#lblstatus").html(obj[0].projectstatus);
            $("#lblprojid").html(obj[0].projid);
            $('#lblprojid').hide();


        },
        error: function (result) {
            alert("Error");
        }
    });
}

function formatJSONDate(jsonDate) {
    var newDate;
    if (jsonDate.length > 0) {
        var parsedDate = new Date(parseInt(jsonDate.substr(6)));
        var jsDate = new Date(parsedDate);
        // var d = new Date(jsonDate);
        newDate = jsDate.getDate() + "/" + (jsDate.getMonth() + 1) + "/" + jsDate.getFullYear();
        // var newDate = new Date(parseInt(jsonDate.substr(6)));
        //return new Date(parseInt(jsonDate.replace('/Date(', '')));
    }
    else {
        newDate = '';
    }
    return newDate;
}


function FillDevelopmentTeam(dataitem) {


    $.ajax(
          {
              type: "POST",
              async: true,
              url: "MyProjects.aspx/BindTeam",
              contentType: "application/json;charset=utf-8",
              data: "{'proid':'" + dataitem.projId + "'}",
              dataType: "json",
              success: function (msg) {
                  var dvalueobj = jQuery.parseJSON(msg.d);
                  var jsonData = msg.d;

                  var listItems = [];
                  for (var i = 0; i < dvalueobj.length; i++) {
                      //  alert(dvalueobj.length);
                      listItems.push('<option value="' +
                    dvalueobj[i].empid + '">' + dvalueobj[i].empName
                      + '</option>');
                  }
                  $('[id$="lstAddTeam"]').empty();

                  $('[id$="lstAddTeam"]').append(listItems.join(''));

              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }

          }

    );
}

function FillCodeReviewTeam(dataitem) {
    var revarray = [];
    $.ajax({
        type: "POST",
        async: false,
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/CodeReviewTeam",
        data: "{'projid':'" + dataitem.projId + "'}",
        dataType: "json",
        success: function (msg) {
            var dvalueobj = jQuery.parseJSON(msg.d);
            for (var i = 0; i < dvalueobj.length; i++) {
                revarray.push(dvalueobj[i].empName);
            }
        },
    });

    $.ajax(
          {
              type: "POST",
              async: false,
              url: "MyProjects.aspx/BindProjectManagerDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              success: function (msg) {
                  $("#txtCodeReviewTeam").kendoMultiSelect({
                      optionLabel: "Select Code Review team",
                      dataTextField: "empName",
                      dataValueField: "empId",
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoMultiSelect");
                  var multi = $("#txtCodeReviewTeam").data("kendoMultiSelect");
                  multi.value(revarray);
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );

}

function GetTaskDetails(val) {

    $.ajax({

        type: "POST",
        url: "MyProjects.aspx/BindMyProjId",
        contentType: "application/json;charset=utf-8",
        data: "{'EmpId':'" + CustId + "','include':'" + val + "','projId':'" + projId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {

            GetTaskData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetTaskDetailsForEmp() {
    $.ajax({
        type: "POST",
        url: "TaskManager.aspx/BindBugsByProjId",
        contentType: "application/json;charset=utf-8",
        data: "{'projId':'" + projId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTaskDataForEmp(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetEmployeeStatus(Tdata) {
    $(GridEmployeeStatus).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectstartdate: { type: "date" },
                        expcompleted: { type: "string" },
                        actualcompleted: { type: "string" },
                        remarks: { type: "string" }
                    }
                }
            },
            // pageSize: 5,
        },
        scrollable: false,
        sortable: true,
        //height: 600,
        //toolbar: ["create"],
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [
                    { field: "projectstartdate", format: "{0:dd-MMM-yyyy}", title: "Status Date", width: "100px" },
                    { field: "expcompleted", title: "Status", width: "100px" },
                    { field: "actualcompleted", title: "Actual Completed", width: "100px" },
                    { field: "remarks", title: "Remarks", width: "100px" },
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
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });

}

function GetTeamMember(Tdata) {
    $(GridTeamMember).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectstartdate: { type: "date" },
                        expcompleted: { type: "string" },
                        actualcompleted: { type: "string" },
                        remarks: { type: "string" }
                    }
                }
            },
            paging: false,
            // pageSize: 5,
        },
        scrollable: false,
        sortable: true,
        //height: 600,
        //toolbar: ["create"],
        //pageable: {
        //    input: false,
        //    numeric: false
        //},
        columns: [

                    { field: "empId", title: "Id", width: "100px" },
                    { field: "empName", title: "Name", width: "150px" },
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
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });

}

function GetTaskData(Tdata) {
    
    $('#gridTaskMang').empty();
    $("#divloading").show();
    var grid = $("#gridTaskMang").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projId: { type: "number", editable: false },
                        custId: { type: "number" },
                        projName: { type: "string" },
                        projDesc: { type: "string" },
                        projManager: { type: "number" },
                        projStartDate: { type: "date" },
                        custName: { type: "string" },
                        empName: { type: "string" },
                        AccountManager: { type: "string" },
                        projExpComp: { type: "date" },
                        //projActComp: { type: "date" },
                        projRemark: { type: "string" },
                        projStatusTDesc: { type: "string" },
                        ExpCompleted: { type: "string" },
                        codeDevTeam: { type: "string" },
                        codeRevTeam: { type: "string" },
                        projStatus: { type: "string" },
                        proStatusDate: { type: "date" }
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        sort: { field: "projName", dir: "desc" },
        //height: 600,
        //toolbar: ["create"],
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                     //editor: projectNameEditor, min: 1
                    //{ field: "projId", title: "Project ID", width: "80px" },
                    { field: "projId", title: "Project Id", width: "100px" },
                    { field: "projName", title: "Project Title", width: "180px" },
                    { field: "projStartDate", format: "{0:dd-MMM-yyyy}", title: "Start Date", width: "100px" },
                    { field: "projExpComp", format: "{0:dd-MMM-yyyy}", title: "Exp Comp Date", width: "100px" },
                    //{ field: "projActComp", format: "{0:dd-MMM-yyyy}", title: "Act Comp Date", width: "100px" },
                    { field: "projStatusTDesc", title: "Status", width: "100px" },
                    { field: "projStatus", title: "Actual Completion", width: "100px" },
                    { field: "projRemark", title: "Project Status Remark", width: "120px" },
                    //{ field: "ExpCompleted", title: "Expected Completion", width: "100px" },
                    { field: "codedevname", title: "Code Developer", width: "100px" },

                    { field: "empName", title: "Project Manager", width: "110px" },
                    { field: "AccountManager", title: "Account Manager", width: "110px" },


                    //{ field: "coderevname", title: "Code Review", width: "100px" },
                 //  { command: [{ name: "View", click: FetchTeamMember }, { name: "Status", click: openAddPopUP }], width: "140px" },
                    { command: [{ name: "View", click: FetchTeamMember }, { name: "Status", click: openAddPopUP }, { name: "Team", click: openTeamPopup }], style: "margin-bottom:20px;", width: "100px" },

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
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        // add & edit popup
        edit: function (e) {
            var editWindow = e.container.data("kendoWindow");
            if (e.model.isNew()) {
                $('[id$="Comment"]').val('');

                // GetStatus();
                InitialiseControlsTypeEdit();
                // BindAllbugsResolutionByBugId(e.model.bug_id, "#grdbugsResolution");
                ///////////This is to get drop down value on Edit/////////////////
                var dropdownlist = $("#drpStatus").data("kendoDropDownList");
                dropdownlist.value(e.model.status_id);

                var dropdownPriority = $("#drpPriorityedit").data("kendoDropDownList");
                dropdownPriority.value(e.model.priority_id);
                ///////////This is to get drop down value on Edit/////////////////
                //var rdotype = kendo.observable({
                //    selectedType: e.model.isdefect
                //});
                //kendo.bind($("input"), rdotype);
                var bugdesc = e.model.bug_desc;
                bugdesc = bugdesc.replace(/\<br[\/]*\>/g, "\n");
                $('[id$="bugDesc"]').val(bugdesc);

                var dropdownType = $("#drpTypeedit").data("kendoDropDownList");
                dropdownType.value(e.model.IsTypeID);

                $("#Editfiles").kendoUpload({
                    async: {

                        saveUrl: "Myprojects.aspx",
                        removeUrl: "Myprojects.aspx",
                        autoUpload: false
                    },

                    error: onError,
                    select: onSelect
                });

                //$(".k-upload-files.k-reset").find("li").remove();
                //$("#Editfiles").parents(".t-upload").find(".t-upload-files").remove();

                e.container.data("kendoWindow").title('Project Status');
                // $(".k-grid-update").text = "Save";
                ClearTempFilesandSession();
                var width = $("#trTask").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();


                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Task Details');
            }
        },

        save: function (e) {

            var id = "";
            if (e.model.isNew()) {
                var filespendingtouload = $(".k-widget.k-upload").find(".k-button.k-upload-selected").is(':visible');
                if (filespendingtouload == true) {
                    e.preventDefault();
                    alert("Please upload the selected files first");

                }
                else {
                    id = e.model.bug_id;
                    // alert(id);
                    var statusID = $('[id$="drpStatus"]').val();
                    var priorityID = $("#drpPriorityedit").val();
                    var Comment = $("#Comment").val().replace(new RegExp("\\n", "g"), "<br/>");
                    //var isDefect = $('input[name="rdoUpdateType"]:checked').val();
                    var IsTypeId = $("#drpTypeedit").val();
                    //UpdateTask(id, priorityID, statusID, e.model.resolution, e.model.Comment);
                    //if (Comment.replace(new RegExp("<br/>", "g"), "").trim() == "") {
                    //    e.preventDefault();
                    //    alert("Please Put Comments");
                    //}
                    //else {                      
                    //UpdateTask(id, priorityID, statusID, e.model.empid, e.model.resolution, Comment, IsTypeId);
                    RedirectPage();
                    //}
                    //UpdateAttachmentBybugID(id);
                    //ClosingRateWindow(e);

                }
            }
            else {
                id = "0";

            }
        },

        view: function (e) {
            var Projectid = e.projId;
            GetDeveloperDetails(Projectid);
        },

        // delete record
        remove: function (e) {

            if (confirm("Are you sure you want to delete?")) {
                DeleteBugsById(e.model.bug_id);
                //alert('Task Deleted')
                // custom actions here
            } else {

                e.preventDefault()
                ClosingRateWindow(e);

            }



        },
    }).data("kendoGrid");
    $("#divloading").hide();
}

//.......
function GetTaskDataForEmp(Tdata) {
    $(GridTaskManager).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        bug_id: { type: "number", editable: false },
                        bug_name: { type: "string" },
                        date_assigned: { type: "date" },
                        bug_lastModified: { type: "date" },
                        priority: { type: "string" },
                        status: { type: "string" },
                        status_id: { type: "number" },
                        priority_id: { type: "number" },
                        empName: { type: "string" },
                        empid: { type: "number" },
                        bug_desc: { type: "string" },
                        resolution: { type: "string" },
                        IsType: { type: "string" },
                        IsTypeID: { type: "number" }
                    }
                }
            },
            pageSize: 25,
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
                    { field: "bug_id", title: "Task ID", width: "80px" },
                    { field: "bug_name", title: "Task Name", width: "150px" },
                    { field: "IsType", title: "Type" },
                    { field: "date_assigned", format: "{0:dd-MMM-yyyy}", title: "Posted On", width: "100px" },
                    { field: "bug_lastModified", format: "{0:dd-MMM-yyyy}", title: "Last Modified", width: "100px" },
                    { field: "priority", title: "priority", width: "100px" },
                    { field: "status", title: "Status", width: "100px" },
                    { field: "empName", title: "Assigned To" },
                    //{ command: [{  text: "Editar", click: editFunction }, { text: "Eliminar", click: deleteFunction }]},
                    { command: [{ name: "edit" }], width: "155px" }],
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
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        // add & edit popup
        edit: function (e) {
            var editWindow = e.container.data("kendoWindow");
            if (e.model.isNew()) {
                $('[id$="Comment"]').val('');
                GetStatus();
                InitialiseControlsTypeEdit();
                BindAllbugsResolutionByBugId(e.model.bug_id, "#grdbugsResolution");
                ///////////This is to get drop down value on Edit/////////////////
                var dropdownlist = $("#drpStatus").data("kendoDropDownList");
                dropdownlist.value(e.model.status_id);

                var dropdownPriority = $("#drpPriorityedit").data("kendoDropDownList");
                dropdownPriority.value(e.model.priority_id);

                ///////////This is to get drop down value on Edit/////////////////

                var dropdownType = $("#drpTypeedit").data("kendoDropDownList");
                dropdownType.value(e.model.IsTypeID);

                //var rdotype = kendo.observable({
                //    selectedType: e.model.isdefect
                //});
                //kendo.bind($("input"), rdotype);
                var bugdesc = e.model.bug_desc;
                bugdesc = bugdesc.replace(/\<br[\/]*\>/g, "\n");
                $('[id$="bugDesc"]').val(bugdesc);

                $("#Editfiles").kendoUpload({
                    async: {

                        saveUrl: "MyProjects.aspx",
                        removeUrl: "MyProjects.aspx",
                        autoUpload: false
                    },

                    error: onError,
                    select: onSelect
                });

                //$(".k-upload-files.k-reset").find("li").remove();
                //$("#Editfiles").parents(".t-upload").find(".t-upload-files").remove();

                e.container.data("kendoWindow").title('Task Details');
                // $(".k-grid-update").text = "Save";
                ClearTempFilesandSession();
                var width = $("#trTask").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();


                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").before(updateButton);
                $("#tdUpdate").before(cancelbutton);
            }
            else {
                e.container.data("kendoWindow").title('Task Details');
            }
        },

        save: function (e) {

            var id = "";
            if (e.model.isNew()) {
                var filespendingtouload = $(".k-widget.k-upload").find(".k-button.k-upload-selected").is(':visible');
                if (filespendingtouload == true) {
                    e.preventDefault();
                    alert("Please upload the selected files first");

                }
                else {
                    id = e.model.bug_id;
                    // alert(id);
                    var statusID = $('[id$="drpStatus"]').val();
                    var priorityID = $("#drpPriorityedit").val();
                    var Comment = $("#Comment").val().replace(new RegExp("\\n", "g"), "<br/>");
                    //var isDefect = $('input[name="rdoUpdateType"]:checked').val();
                    var IsTypeId = $("#drpTypeedit").val();
                    //UpdateTask(id, priorityID, statusID, e.model.resolution, e.model.Comment);
                    //if (Comment.replace(new RegExp("<br/>", "g"), "").trim() == "") {
                    //    e.preventDefault();
                    //    alert("Please Put Comments");
                    //}
                    //else {
                    // UpdateTask(id, priorityID, statusID, e.model.empid, e.model.resolution, Comment, IsTypeId);
                    RedirectPage();
                    //}
                    //UpdateAttachmentBybugID(id);
                    //ClosingRateWindow(e);

                }
            }
            else {
                id = "0";

            }
        },



        // delete record
        remove: function (e) {

            if (confirm("Are you sure you want to delete")) {
                DeleteBugsById(e.model.bug_id);
                //alert('Task Deleted')
                // custom actions here
            } else {

                e.preventDefault()
                ClosingRateWindow(e);

            }



        },
    });

}

function ClosingRateWindow(e) {

    //$(document).on("click", ".k-i-close", function () { $(this).parents('.k-window:first').hide(); $('.k-overlay').hide(); });
    var grid = $("#gridTaskMang").data("kendoGrid");
    grid.refresh();



}

/////////////////////////////////////////////////////////////// End Bind Grid////////////////////////////////////////////////////////

function FetchTeamMember(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");   
    var grid = $("#gridTaskMang").data("kendoGrid");
    var dataItem = grid.dataItem($(event.target).closest("tr"));
    cleartext();
    GetDeveloperDetails(dataItem.projId);
    GetProjectStatus(dataItem.projId);
    GetProjectDetails(dataItem.projId);
    GetReportData(dataItem.projId);
    openPopUP();
    setTimeout(openPopUP, 1000);
}



function openPopUP() {

    $('#divPopUP').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}

function closePopUP() {
    $('#divPopUP').css('display', 'None');

    $('#divOverlay').removeClass('k-overlay').addClass("k-overlayDisplaynone");
}
function GetReport() {
    //  openPopUP();
    //$(document).ready(function () {
    //GetReportData();
    //});
}
function GetReportData(projId) {
    $.ajax(
          {
              type: "POST",
              url: "TaskManager.aspx/GetAllStatusReportByProjId",
              contentType: "application/json;charset=utf-8",
              data: "{'projId':'" + projId + "'}",
              dataType: "json",
              success: function (msg) {
                  var detailObj = jQuery.parseJSON(msg.d)
                  //alert(msg.d);
                  $.each(detailObj, function (key, output) {
                      var keywords = output.key;
                      var count = output.value;
                      if (keywords == 'TotalBugsCount')
                          $("#lblTotal").text(count);
                      else if (keywords == 'Terminated')
                          $("#lblresolved").text(count);
                      else if (keywords == 'Showstopper')
                          $("#lblshowstoper").text(count);
                      else if (keywords == 'Major')
                          $("#lblmajor").text(count);
                      else if (keywords == 'Cosmetic')
                          $("#lblCosmetic").text(count);
                      else if (keywords == 'Minor')
                          $("#lblminor").text(count);
                      else if (keywords == 'Open')
                          $("#lblOpen").text(count);
                      else if (keywords == 'In progress')
                          $("#lblInProgress").text(count);
                      else if (keywords == 'Completed')
                          $("#lblCompleted").text(count);
                      else if (keywords == 'On hold')
                          $("#lblOnHold").text(count);
                      else if (keywords == 'Ready for QA')
                          $("#lblReadyQA").text(count);

                  });
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
    // 
}
//Add Taskmanager functionality starts here

function DeleteBugAttachmentByFileID(id, name, curBugID) {
    //alert( document.getElementById("<%=hfbugFileId.ClientID%>").value );
    //alert($("#txtFileID").val());

    //alert(id);
    $.ajax(
       {
           type: "POST",
           url: "TaskManager.aspx/DeletebugAttachmentsByFileId",
           contentType: "application/json;charset=utf-8",
           data: "{'bugFileId':'" + id + "','bugFileName':'" + name + "'}",
           dataType: "json",
           cache: false,
           async: false,
           success: function (msg) {
               // RedirectPage();
               var detailObj = jQuery.parseJSON(msg.d)
               $.each(detailObj, function (key, output) {
                   if (output == "false") {
                       alert('You just added this file.Wait for 1 minute');
                       return false;
                   }
                   else if (output == "true") {
                       alert('File deleted successfully.');
                       //ClosingRateWindow(e);
                       // RedirectPage();
                       GetAttachment(curBugID);
                       return false;
                   }

               });
           },
           error: function (x, e) {
               alert("The call to the server side failed. "
                     + x.responseText);
           }
       }
);


}
function GetAttachment(BugID) {

    $.ajax(
          {
              type: "POST",
              url: "TaskManager.aspx/GetFirstbugAttachmentsByBugId",
              contentType: "application/json;charset=utf-8",
              data: "{'BugId':'" + BugID + "'}",
              dataType: "json",
              async: false,
              success: function (msg) {

                  var data = jQuery.parseJSON(msg.d);
                  $('[id$="attachment"]').html("");
                  if (data.length > 0) {
                      for (i = 0; i < data.length; i++) {
                          var button;

                          var Anchor = $('<a target="_blank" href="../Common/DownloadAttachments.aspx?DocName=' + data[i].bugFilePath + '">' + data[i].bugFilePath + '</a>');
                          $('[id$="attachment"]').append(Anchor);
                          //var FileID=$('<input type="text" id="txtFileID" Style="display:none;"  value="'+data[i].bugFileId+'" />');
                          // $('[id$="attachment"]').append(FileID);
                          var strval = document.getElementsByClassName('hdnval')[0].value;
                          if (strval == 'True')
                              button = $('<input type="button" id="' + data[i].bugFileId + '" name="' + data[i].bugFilePath + '"  title="' + BugID + '" value="Delete" onClick="var confirmmessage =\'Do you really want to delete file?\';if(confirm(confirmmessage))DeleteBugAttachmentByFileID(this.id,this.name,this.title);else return false;"/>');
                          else
                              button = $('<input type="button" id="' + data[i].bugFileId + '" name="' + data[i].bugFilePath + '" style=display:none title="' + BugID + '" value="Delete" onClick="var confirmmessage =\'Do you really want to delete file?\';if(confirm(confirmmessage))DeleteBugAttachmentByFileID(this.id,this.name,this.title);else return false;"/>');



                          $('[id$="attachment"]').append(button);

                          $('[id$="attachment"]').append('<br/>');

                      }
                  }
                  else {
                      var Anchor = "No Attachment."
                      $('[id$="attachment"]').append(Anchor);
                      $('[id$="attachment"]').addClass("ForeColor");


                  }
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);
}
function onSelect(e) {
    //  kendoConsole.log("Select :: " + getFileInfo(e));
    //alert(getFileInfo(e));
    return $.map(e.files, function (file) {
        if (file.size <= 10485760) {
            //UploadFile();
            return true;
        }
        else {
            
            alert("File size must less than 10 MB !");
            return false;
        }

    }).join(", ");
}
function onError(e) {
    kendoConsole.log("Error (" + e.operation + ") :: " + getFileInfo(e));
}
function GetData() {
    $.ajax(
          {
              type: "POST",
              url: "TaskManager.aspx/FilterModule",
              contentType: "application/json;charset=utf-8",
              data: "{ '_parentId': '" + projId + "'}",
              dataType: "json",
              success: function (msg) {
                  InitialiseControlsModule(jQuery.parseJSON(msg.d));
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
);

    $.ajax(
             {
                 type: "POST",
                 url: "TaskManager.aspx/GetAllbugPriorities",
                 contentType: "application/json;charset=utf-8",
                 data: "{}",
                 dataType: "json",
                 success: function (msg) {
                     InitialiseControlsPriority(jQuery.parseJSON(msg.d));
                 },
                 error: function (x, e) {
                     alert("The call to the server side failed. "
                           + x.responseText);
                 }
             }
  );
    $.ajax(

            {
                type: "POST",
                url: "TaskManager.aspx/GetProjectEmployeesByProjId",
                contentType: "application/json;charset=utf-8",
                data: "{ 'projId': '" + projId + "'}",
                dataType: "json",
                success: function (msg) {

                    InitialiseControls(jQuery.parseJSON(msg.d));

                },
                error: function (x, e) {
                    alert("The call to the server side failed. "
                          + x.responseText);
                }
            }
);


}
function InitialiseControlsModule(ModuleData) {

    var Module = $("#drpmodule").kendoDropDownList({
        optionLabel: "Select Module",
        dataTextField: "moduleName",
        dataValueField: "moduleId",
        dataSource: ModuleData,
    }).data("kendoDropDownList");
}
function InitialiseControlsPriority(PriorityData) {

    var Priority = $("#drpPriority").kendoDropDownList({
        optionLabel: "Select Priority",
        dataTextField: "priority_desc",
        dataValueField: "priority_id",
        dataSource: PriorityData,
    }).data("kendoDropDownList");

}
function InitialiseControls(AssignedToData) {

    var Priority = $("#drpAssignedTo").kendoDropDownList({
        optionLabel: "Select",
        dataTextField: "empName",
        dataValueField: "empid",
        dataSource: AssignedToData,
    }).data("kendoDropDownList");

}

function InitialiseControlsStatus(StatusData) {
    var Status = $('[id$="drpStatus"]').kendoDropDownList({
        //optionLabel: "Select",
        dataTextField: "status",
        dataValueField: "status_id",
        dataSource: StatusData,
    }).data("kendoDropDownList");
}
function InitialiseControlsPr(PriorityDataEdit) {

    var Priority = $("#drpPriorityedit").kendoDropDownList({
        //optionLabel: "Select Priority",
        dataTextField: "priority_desc",
        dataValueField: "priority_id",
        dataSource: PriorityDataEdit,
    }).data("kendoDropDownList");
}

function InitialiseControdrpStatus() {
    var Type = $("#drpstatus").kendoDropDownList({
        optionLabel: "Select Status",
        dataSource: [
            { IsType: "Started", IsTypeID: 1 },
            { IsType: "In Progress", IsTypeID: 2 },
            { IsType: "To Be Started", IsTypeID: 3 },
            { IsType: "On Hold", IsTypeID: 4 },
            { IsType: "Completed", IsTypeID: 5 },
            { IsType: "Cancelled", IsTypeID: 6 },
        ],
        dataTextField: "IsType",
        dataValueField: "IsTypeID",
    }).data("kendoDropDownList");
}

function InitialiseControlsType() {
    var Type = $("#drpType").kendoDropDownList({
        optionLabel: "Select Type",
        dataSource: [
            { IsType: "Functional Defect (FD)", IsTypeID: 1 },
            { IsType: "Technical Defect (TD)", IsTypeID: 2 },
            { IsType: "Change", IsTypeID: 3 }
        ],
        dataTextField: "IsType",
        dataValueField: "IsTypeID",
    }).data("kendoDropDownList");
}
function InitialiseControlsTypeEdit() {
    var Type = $("#drpTypeedit").kendoDropDownList({
        //optionLabel: "Select Type",
        dataSource: [
            { IsType: "Functional Defect (FD)", IsTypeID: 1 },
            { IsType: "Technical Defect (TD)", IsTypeID: 2 },
            { IsType: "Change", IsTypeID: 3 }
        ],
        dataTextField: "IsType",
        dataValueField: "IsTypeID",
    }).data("kendoDropDownList");
}

function GetStatus() {
    $.ajax(

           {
               type: "POST",
               url: "MyProjects.aspx/GetAllbugStatuses",
               contentType: "application/json;charset=utf-8",
               data: "{}",
               dataType: "json",
               async: false,
               success: function (msg) {
                   //alert();
                   InitialiseControlsStatus(jQuery.parseJSON(msg.d));

               },
               error: function (x, e) {
                   alert("The call to the server side failed. "
                         + x.responseText);
               }
           }
 );
    $.ajax(
                   {
                       type: "POST",
                       url: "TaskManager.aspx/GetAllbugPriorities",
                       contentType: "application/json;charset=utf-8",
                       data: "{}",
                       dataType: "json",
                       async: false,
                       success: function (msg) {
                           //alert('Priority');
                           InitialiseControlsPr(jQuery.parseJSON(msg.d));
                       },
                       error: function (x, e) {
                           alert("The call to the server side failed. "
                                 + x.responseText);
                       }
                   }
        );


}
function UpdateTask(BugID, priorityid, statusId, empId, CommentHistroy, Resolution, IsTypeId) {
    //alert(BugID +","+priorityid+","+","+statusId+","+CommentHistroy+","+Resolution);

    $.ajax(
           {

               type: "POST",
               url: "TaskManager.aspx/UpdatebugByBugId",
               contentType: "application/json;charset=utf-8",
               data: "{'bug_Id':'" + BugID + "','priority_id':'" + priorityid + "','statusId':'" + statusId + "','empId':'" + empId + "','CommentHistory':'" + CommentHistroy + "','Resolution':'" + Resolution + "','IsTypeId':'" + IsTypeId + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert('Task updated successfully.');
                   //if (message.contains("Sucesses")) {
                   //    alert(message);
                   //    RedirectPage();
                   //}
                   //else {
                   //    e.preventDefault();
                   //    alert(message);
                   //}
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );

}
function UpdateAttachmentBybugID(BugID, allfilenames) {

    //alert(BugID);
    $.ajax(
           {

               type: "POST",
               url: "TaskManager.aspx/UpdatebugAttachment",
               contentType: "application/json;charset=utf-8",
               data: "{'bug_Id':'" + BugID + "','allfiles':'" + allfilenames + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   //RedirectPage();
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );

}
function RedirectPage() {
    window.location.assign("Myprojects.aspx");
}
function isChar(evt, field) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    return ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode == 8 || charCode == 9 || charCode == 32 || (charCode >= 48 && charCode <= 57)) ? true : false;
}




function openTeamPopup(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");   
    var grid = $("#gridTaskMang").data("kendoGrid");
    var dataItem = grid.dataItem($(event.target).closest("tr"));
    // var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    cleartext();
    //  GetDevelopmentTeam(dataItem.projId)

    FillDevelopmentTeam(dataItem);
    //$('#divTeamMembers').html('');
    // $('#divTeamMembers').empty();
    $('#divTeamMembers').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function closeTeamPopUP() {
    //$('#divTeamMembers').html('');
    $('#txtDevelopmentTeam').html('');
    $('#divTeamMembers').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function openAddPopUP(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    
    var grid = $("#gridTaskMang").data("kendoGrid");
    var dataItem = grid.dataItem($(event.target).closest("tr"));
    cleartext();
    e.preventDefault();
    FillProjectStatus();
    
    $('[id$="drpstatus"]').val(dataItem.projStatusTId);

    GetProjectStatusDetails(dataItem.projId);
    BindAllDateCalender();
    
    var s = dataItem.projStatus;
    s = s.substring(0, s.indexOf('%'));

    GetProjUpdateStatus(dataItem.projId);
    if (s.length != 0) {
        $('#txtcomplete').val(s);
    }
    else {
        $('#txtcomplete').val(0);
    }
    $('#errormessage').html('');

    setTimeout(openStatusPopUP, 500);
}

function openStatusPopUP() {

    $('#divStatusPopUP').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function GetProjUpdateStatus(projId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "MyProjects.aspx/projectStatus",
        data: "{'proid':'" + projId + "'}",
        dataType: "json",
        success: function (msg) {
            GetEmployeeUpdateStatus(jQuery.parseJSON(msg.d));

        },
        error: function (result) {
            alert("Error");
        }
    });
}

function GetEmployeeUpdateStatus(Tdata) {
    $(GridProjectStaus).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectstartdate: { type: "date" },
                        expcompleted: { type: "string" },
                        actualcompleted: { type: "string" },
                        remarks: { type: "string" },
                        PostedBy: { type: "string" }
                    }
                }
            },
            // pageSize: 5,
        },
        scrollable: false,
        sortable: true,
        //height: 600,
        //toolbar: ["create"],
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [
                    { field: "projectstartdate", format: "{0:dd-MMM-yyyy}", title: "Status Date", width: "100px" },
                    { field: "expcompleted", title: "Status", width: "100px" },
                    { field: "actualcompleted", title: "Actual Completed", width: "100px" },
                    { field: "remarks", title: "Remarks", width: "100px" },
                    { field: "PostedBy", title: "Posted By", width: "100px" },

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
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });

}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    window.location.reload();
}

function CancelAddPopUP() {
    //  e.preventDefault();
    $('#divStatusPopUP').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    //  location.reload(false);
}
function cleartext() {
    $("#txtTaskName").val('');
    $("#txtTaskDesc").val('');
    $("#lblerrmsgModule").html("");
    $("#lblerrmsgTaskName").html("");
    $("#lblerrmsgTaskDesc").html("");
    $("#lblerrmsgPriority").html("");
    $("#lblerrmsgAssgnTo").html("");
    $("#lblmsgType").html("");
    $("#lblProjectname").html("");
    $("#lblprjName").html("");
    $("#lblprojStatus").html("");
    $("#lblCustName").html("");
    $("#lblExpProjStatus").html("");
    $("#lblCustAddress").html("");
    $("#lblStartDate").html("");
    $("#lblProjDurat").html("");
    $("#lblExpDate").html("");
    $("#lblProjMang").html("");
    $("#lblActCompDate").html("");
}

function ShowAddPopup() {

    openAddPopUP();
    $("#txtTaskName").val('');
    $("#txtTaskDesc").val('');
    $("#lblerrmsgModule").html("");
    $("#lblerrmsgTaskName").html("");
    $("#lblerrmsgTaskDesc").html("");
    $("#lblerrmsgPriority").html("");
    $("#lblerrmsgAssgnTo").html("");
    $("#lblmsgType").html("");
    $("#lblProjectname").html(ProjName);

    //$(".k-upload-files.k-reset").find("li").remove();
    //$("#files").parents(".t-upload").find(".t-upload-files").remove();

    GetData();
    InitialiseControlsType();
    InitialiseControdrpStatus();
    var drpmodule = $("#drpmodule").data("kendoDropDownList");
    drpmodule.text(drpmodule.options.optionLabel);
    drpmodule.element.val("");
    drpmodule.selectedIndex = -1;
    
    var drpstatus = $("#drpstatus").data("kendoDropDownList");
    drpstatus.text(drpstatus.options.optionLabel);
    drpstatus.element.val("");
    drpstatus.selectedIndex = -1;

    var drpPriority = $("#drpPriority").data("kendoDropDownList");
    drpPriority.text(drpPriority.options.optionLabel);
    drpPriority.element.val("");
    drpPriority.selectedIndex = -1;

    var drpAssignedTo = $("#drpAssignedTo").data("kendoDropDownList");
    drpAssignedTo.text(drpAssignedTo.options.optionLabel);
    drpAssignedTo.element.val("");
    drpAssignedTo.selectedIndex = -1;

    var drpType = $("#drpType").data("kendoDropDownList");
    drpType.text(drpType.options.optionLabel);
    drpType.element.val("");
    drpType.selectedIndex = -1

    $(".k-widget.k-upload").find("ul").remove();
    $(".k-widget.k-upload").find(".k-button.k-upload-selected").remove();
    ClearTempFilesandSession();

}
function ClearTempFilesandSession() {
    $.ajax(
            {

                type: "POST",
                url: "Myprojects.aspx/ClearTempFilesandSession",
                contentType: "application/json;charset=utf-8",
                data: "{}",
                cache: false,
                async: false,
                dataType: "json",
                success: function (msg) {
                    RedirectPage();
                },
                error: function (msg) {
                    alert("The call to the server side failed."
                          + msg.responseText);
                }
            }
         );
}
// Bind BugsResolution details for Bugid
function BindAllbugsResolutionByBugId(BugId, ResolutionGrid) {

    $.ajax({
        type: "POST",
        url: "TaskManager.aspx/GetAllbugsResolutionByBugId",
        contentType: "application/json;charset=utf-8",
        data: "{'BugId':'" + BugId + "'}",
        dataType: "json",
        success: function (msg) {
            var Result = jQuery.parseJSON(msg.d);
            if (Result != "No records") {
                GetBugsResolutionData(Result, ResolutionGrid);
                // $("#divOldbugsResolution").css('display', 'none');
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}
function GetBugsResolutionData(Tdata, ResolutionGrid) {
    $(ResolutionGrid).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        bugsResolutionId: { type: "number" },
                        bug_id: { type: "number" },
                        status: { type: "string" },
                        priority: { type: "string" },
                        resolution: { type: "string" },
                        resolutionBy: { type: "string" },
                        resolutionDate: { type: "date" },
                        ResolutionFiles: { type: "string" },
                    }
                }
            }
        },
        scrollable: false,
        sortable: false,
        pageable: false,
        columns: [
                    { field: "resolutionBy", title: "Commented By" },
                    { field: "resolutionDate", format: "{0:dd-MMM-yyyy}", title: "On Date", width: "100px" },
                    { field: "status", title: "Status" },
                    { field: "priority", title: "Priority" },
                    //{ field: "resolution", title: "Comments" },
                    { title: "Comments", template: "#= GetComments(data) #" },
                    //{ field: "ResolutionFiles", title: "Attachments" },                 
                    { title: "Attachments", template: "#= GetResolutionFiles(data) #" },

        ],
        filterable: false,
        editable: false,
    });
}
function DeleteBugsById(BugID) {
    //alert(BugID +","+priorityid+","+","+statusId+","+CommentHistroy+","+Resolution);

    $.ajax(
           {

               type: "POST",
               url: "TaskManager.aspx/DeletebugById",
               contentType: "application/json;charset=utf-8",
               data: "{'bug_Id':'" + BugID + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   //alert('Task Deleted');
                   //if (message.contains("Sucesses")) {
                   //    alert(message);
                   //    RedirectPage();
                   //}
                   //else {
                   //    e.preventDefault();
                   //    alert(message);
                   //}
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );

}
function GetComments(data) {
    if ($("#grdbugsResolution").data("kendoGrid").dataSource.total() > 0) {
        var output = "";
        var resolution = data.resolution;
        output = resolution;
        return output;
    }
}
function GetResolutionFiles(data) {
    if ($("#grdbugsResolution").data("kendoGrid").dataSource.total() > 0) {
        var output = "";
        var strFileNames = data.ResolutionFiles;
        var CurFile = strFileNames.split('\n');
        for (var i = 0; i < CurFile.length; i++) {
            output = '<a target="_blank" href="../Common/DownloadAttachments.aspx?DocName=' + CurFile[i] + '">' + CurFile[i] + '</a> <br>' + output;
        }
        return output;
    }
}
function GetPagesize() {
    var grid = $(GridTaskManager).data("kendoGrid");
    $("#comboBox").width(120).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: [
            { text: 'All' },
            { text: 'Current' },
            { text: 'Completed' },
            { text: 'Cancelled' },
        ],
        index: 0,
        change: function (e) {
            if (this.value() == 'All')
                grid.dataSource.pageSize(grid.dataSource.total());
            else
                grid.dataSource.pageSize(this.value());
        }
    });
    //grid.dataSource.pageSize(grid.dataSource.total());
}
function BindAllDateCalender() {

    $('[id$="txtStartDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
    var todayDate = getCurrentDate();
    $("#txtStartDate").data("kendoDatePicker").value(todayDate);
}

function getCurrentDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '/' + mm + '/' + yyyy;
    return today;
}
function FillProjectStatus() {
    $.ajax(
         {
             type: "POST",
             url: "MyProjects.aspx/BindProjectStatusId",
             contentType: "application/json;charset=utf-8",
             data: "{}",
             dataType: "json",
             //async: false,
             success: function (msg) {
                 $("#drpstatus").kendoDropDownList({
                     dataTextField: "projStatusTDesc",
                     dataValueField: "projStatusTId",
                     dataSource: jQuery.parseJSON(msg.d),

                 }).data("kendoDropDownList");
                 //$('[id$="drpstatus"]').val("InProgreess");
             },
             error: function (x, e) {
                 alert("The call to the server side failed. "
                       + x.responseText);
             }
         }
   );
}
