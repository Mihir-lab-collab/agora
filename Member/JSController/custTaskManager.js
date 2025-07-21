$(document).ready(function () {
    var includeTerminated;
    var str = document.getElementsByClassName('hdnval')[0].value;
    if (projId == '') {
        projId = 0;
    }
    //--------------------------
    //var chkTerminated = document.getElementById("chkTerminated").checked;
    //if (chkTerminated) {
    //    includeTerminated = "Y";
    //}
    //else {
    //    includeTerminated = "N";
    //}

    //--------------------------

    if (str == 'True') {
        GetTaskDetails();
    }
    else {
        GetTaskDetailsForEmp();
    }

    GetPagesize();
    $(document).on("click", ".k-grid-edit", function (event) {
        var BugID = $(this).parents('tr:first').find('td:first').text();
        GetAttachment(BugID);
    });
    $("#files").kendoUpload({
        async: {
            saveUrl: "TaskManager.aspx",
            removeUrl: "TaskManager.aspx",
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
function fnTerminatedClick(checked) {

    var includeTerminated;
    var str = document.getElementsByClassName('hdnval')[0].value;
    if (projId == '') {
        projId = 0;
    }
    //--------------------------
    //var chkTerminated = document.getElementById("chkTerminated").checked;
    //if (chkTerminated) {
    //    includeTerminated = "Y";
    //}
    //else {
    //    includeTerminated = "N";
    //}
    //--------------------------

    if (str == 'True') {
        GetTaskDetails();
    }
    else {
        GetTaskDetailsForEmp();
    }

    GetPagesize();
    $(document).on("click", ".k-grid-edit", function (event) {
        var BugID = $(this).parents('tr:first').find('td:first').text();
        GetAttachment(BugID);
    });
    $("#files").kendoUpload({
        async: {
            saveUrl: "TaskManager.aspx",
            removeUrl: "TaskManager.aspx",
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

}


///////////////////////////////////// Start Bind Grid////////////////////////////////////////////////////////
function GetTaskDetails() {
    var chkTerminated = document.getElementById("chkTerminated").checked;
    if (chkTerminated) {
        includeTerminated = "Y";
    }
    else {
        includeTerminated = "N";
    }
    //alert('dfdf')
    $.ajax({
        type: "POST",
        url: "TaskManager.aspx/BindBugsByProjId",
        contentType: "application/json;charset=utf-8",
        data: "{'projId':'" + projId + "', 'includeTerminated':'" + includeTerminated + "'}",
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
    var chkTerminated = document.getElementById("chkTerminated").checked;
    if (chkTerminated) {
        includeTerminated = "Y";
    }
    else {
        includeTerminated = "N";
    }

    $.ajax({
        type: "POST",
        url: "TaskManager.aspx/BindBugsByProjId",
        contentType: "application/json;charset=utf-8",
        //data: "{'projId':'" + projId + "'}",
        data: "{'projId':'" + projId + "', 'includeTerminated':'" + includeTerminated + "'}",
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

function GetTaskData(Tdata) {
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
                        IsTypeID: { type: "number" },
                        assigned_by_Name: { type: "string" },
                        ModuleName: { type: "string" }
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
                    { field: "ModuleName", title: "Module" },
                    { field: "IsType", title: "Type" },
                    { field: "date_assigned", format: "{0:dd-MMM-yyyy}", title: "Posted On", width: "100px" },
                    { field: "bug_lastModified", format: "{0:dd-MMM-yyyy}", title: "Last Modified", width: "100px" },
                    { field: "priority", title: "priority", width: "100px" },
                    { field: "status", title: "Status", width: "100px" },
                    { field: "empName", title: "Assigned To" },
                     { field: "assigned_by_Name", title: "Assigned By" },
                   //{ command: [{  text: "Editar", click: editFunction }, { text: "Eliminar", click: deleteFunction }]},
                   { command: [{ name: "edit" }, { name: "destroy" }], width: "180px" }],
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

                var drpAssignedTo = $("#drpAssignedToedit").data("kendoDropDownList");
                drpAssignedTo.value(e.model.empid);

                var dropdownType = $("#drpTypeedit").data("kendoDropDownList");
                dropdownType.value(e.model.IsTypeID);
                //var rdotype = kendo.observable({
                //    selectedType: e.model.isdefect
                //});
                //kendo.bind($("input"), rdotype);

                ///////////This is to get drop down value on Edit/////////////////
                var bugdesc = e.model.bug_desc;
                bugdesc = bugdesc.replace(/\<br[\/]*\>/g, "\n");
                $('[id$="bugDesc"]').val(bugdesc);

                $("#Editfiles").kendoUpload({
                    async: {

                        saveUrl: "TaskManager.aspx",
                        removeUrl: "TaskManager.aspx",
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
                    var empID = $("#drpAssignedToedit").val();
                    var Comment = $("#Comment").val().replace(new RegExp("\\n", "g"), "<br/>");
                    var IsTypeId = $("#drpTypeedit").val();
                    //var isDefect = $('input[name="rdoUpdateType"]:checked').val();
                    //UpdateTask(id, priorityID, statusID, e.model.resolution, e.model.Comment);
                    //if (Comment.replace(new RegExp("<br/>", "g"), "").trim() == "") {
                    //e.preventDefault();
                    //    alert("Please Put Comments");
                    //}

                    //else {
                    $('body').css('cursor', 'progress');
                    //$("#divloading").show();
                    UpdateTask(id, priorityID, statusID, empID, e.model.resolution, Comment, IsTypeId);

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

            if (confirm("Are you sure you want to delete?")) {
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
                        IsTypeID: { type: "number" },
                        assigned_by_Name: { type: "string" },     //Added by Pravin on 23Jul2014
                        ModuleName: { type: "string" }     //Added by Pravin on 23Jul2014
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
                    { field: "bug_id", title: "ID", width: "50px" },   //changed from Task ID to ID by Pravin on 2Sep2014,width: "50px"
                    { field: "bug_name", title: "Task" },   //, width: "150px" (Task Name)
                    { field: "ModuleName", title: "Module" },     //Added by Pravin on 23Jul2014, ("Module Name")
                    { field: "IsType", title: "Type", width: "70px" },
                    { field: "date_assigned", format: "{0:dd-MMM-yyyy}", title: "Posted On" },   //, width: "100px" 
                    { field: "bug_lastModified", format: "{0:dd-MMM-yyyy}", title: "Last Modified" },    // width: "100px" 
                    { field: "priority", title: "Priority" },       //priority Spelling changed by Pravin on 2Sep2014, width: "100px"
                    { field: "status", title: "Status" },    //, width: "100px" 
                    { field: "empName", title: "Assigned To" },
                    { field: "assigned_by_Name", title: "Assigned By" },     //Added by Pravin on 23Jul2014

                    //{ command: [{  text: "Editar", click: editFunction }, { text: "Eliminar", click: deleteFunction }]},
                     //{ command: [{ name: "edit" }] }], 
                    //{ command: [{ name: "edit", width: "20px", text: { edit: "" } }], width: "100px" }],
                    { command: [{ name: "edit", width: "20px" }], width: "100px" }],
        //width changed from 155px to 130px on 23Jul2014, , width: "120px"
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

                var drpAssignedTo = $("#drpAssignedToedit").data("kendoDropDownList");
                drpAssignedTo.value(e.model.empid);


                //var rdotype = kendo.observable({
                //    selectedType: e.model.isdefect
                //});
                //kendo.bind($("input"), rdotype);
                var bugdesc = e.model.bug_desc;
                bugdesc = bugdesc.replace(/\<br[\/]*\>/g, "\n");
                $('[id$="bugDesc"]').val(bugdesc);

                $("#Editfiles").kendoUpload({
                    async: {

                        saveUrl: "TaskManager.aspx",
                        removeUrl: "TaskManager.aspx",
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
                    UpdateTask(id, priorityID, statusID, e.model.empid, e.model.resolution, Comment, IsTypeId);
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
function openPopUP() {
    $('#divPopUP').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}
function GetReport() {
    openPopUP();
    //$(document).ready(function () {
    GetReportData();
    //});
}
function GetReportData() {
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

                          var Anchor = $('<a target="_blank" href="Common/DownloadAttachments.aspx?mode=T&DocName=' + data[i].bugFilePath + '">' + data[i].bugFilePath + '</a>');
                          $('[id$="attachment"]').append(Anchor);
                          //var FileID=$('<input type="text" id="txtFileID" Style="display:none;"  value="'+data[i].bugFileId+'" />');
                          // $('[id$="attachment"]').append(FileID);

                          var button = $('<input type="button" id="' + data[i].bugFileId + '" name="' + data[i].bugFilePath + '" title="' + BugID + '" value="Delete" onClick="var confirmmessage =\'Do you really want to delete file?\';if(confirm(confirmmessage))DeleteBugAttachmentByFileID(this.id,this.name,this.title);else return false;"/>');
                          //alert(button);
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
            return false;
            alert("File size must less than 10 MB !");

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
function InitialiseControlsAs(EmpDataEdit) {

    var Priority = $("#drpAssignedToedit").kendoDropDownList({
        //optionLabel: { empName: "Select", empid: 0 },
        dataTextField: "empName",
        dataValueField: "empid",
        dataSource: EmpDataEdit,
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
               url: "TaskManager.aspx/GetAllbugStatuses",
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
    $.ajax(
                  {
                      type: "POST",
                      url: "TaskManager.aspx/GetProjectEmployeesByProjId",
                      contentType: "application/json;charset=utf-8",
                      data: "{ 'projId': '" + projId + "'}",
                      dataType: "json",
                      async: false,
                      success: function (msg) {
                          //alert('Priority');
                          InitialiseControlsAs(jQuery.parseJSON(msg.d));
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
                   $('body').css('cursor', 'default');
                   //$("#divloading").hide();
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
    window.location.assign("TaskManager.aspx");
}
function isChar(evt, field) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    return ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode == 8 || charCode == 9 || charCode == 32 || (charCode >= 48 && charCode <= 57)) ? true : false;
}
function openAddPopUP() {

    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
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
    var drpmodule = $("#drpmodule").data("kendoDropDownList");
    drpmodule.text(drpmodule.options.optionLabel);
    drpmodule.element.val("");
    drpmodule.selectedIndex = -1;


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
                url: "TaskManager.aspx/ClearTempFilesandSession",
                contentType: "application/json;charset=utf-8",
                data: "{}",
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
                //$("#divOldbugsResolution").css('display', 'none');
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
            output = '<a target="_blank" href="Common/DownloadAttachments.aspx?mode=T&DocName=' + CurFile[i] + '">' + CurFile[i] + '</a> <br>' + output;
            //output = '<a target="_blank" href="/Agora_dev/Member/Attachment/TaskManager/' + CurFile[i] + '">' + CurFile[i] + '</a> <br>' + output;
        }
        return output;
    }
}
function GetPagesize() {
    var grid = $(GridTaskManager).data("kendoGrid");
    $("#comboBox").width(70).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: [
            { text: 'All' },
            { text: 25 },
            { text: 50 },
            { text: 100 },
            { text: 200 },
            { text: 300 },
            { text: 500 }
        ],
        index: 1,
        change: function (e) {

            if (this.value() == 'All')
                grid.dataSource.pageSize(grid.dataSource.total());
            else
                grid.dataSource.pageSize(this.value());
        }
    });

    //grid.dataSource.pageSize(grid.dataSource.total());
}

//bindGrid 
//$(document).ready(function () {
//    $.ajax({
//        type: "POST",
//        url: "TaskManager.aspx/BindBugsByProjId",

//        contentType: "application/json;charset=utf-8",
//        data: "{'projId':'" + projId + "'}",
//        dataType: "json",
//        success: function (msg) {
//            GetTData(jQuery.parseJSON(msg.d));
//        },
//        error: function (x, e) {
//            alert("The call to the server side failed. "
//                  + x.responseText);
//        }
//    });

//});
//function GetTData(Tdata) {
//    $(GridTaskManager).kendoGrid({
//        dataSource: {
//            data: Tdata,
//            pageSize: 10,
//            schema: {
//                model: {
//                    bug_id: "bug_id",
//                    fields: {
//                        bug_id: { type: "number", editable: false },
//                        bug_name: { type: "string", validation: { required: true } },
//                        date_assigned: { type: "date", validation: { required: true } },
//                        bug_lastModified: { type: "date", validation: { required: true } },
//                        priority: { type: "string", validation: { required: true } },                       
//                        status: { type: "string" },
//                        empName: { type: "string" }
//                    }
//                }
//            }
//        },
//        //height: 705,
//        editable: true,
//        editable: {
//            mode: "popup",
//            template: kendo.template($("#popup-editor").html())
//        },

//        //toolbar: ["create"],
//        scrollable: true,
//        sortable: true,
//        pageable: { input: true, numeric: false},
//        columns: [
//                     //editor: projectNameEditor, min: 1
//                    { field: "bug_id", title: "Task ID", width: "100px" },
//                    { field: "bug_name", title: "Task Name" },
//                    { field: "date_assigned", title: "Posted On", format: "{0:dd-MMM-yyyy}", width: "100px" },
//                    { field: "bug_lastModified", title: "Last Modified", format: "{0:dd-MMM-yyyy}", width: "100px" },
//                    { field: "priority", title: "priority" },                   
//                    { field: "status", title: "Status" },
//                     { field: "empName", title: "Assigned To" },
//                    //{ command: [{  text: "Editar", click: editFunction }, { text: "Eliminar", click: deleteFunction }]},
//                    { command: [{ name: "edit" }], width: "155px"}],
//        filterable: {
//            extra: false,
//            operators: {
//                string: {
//                    startswith: "Starts with",
//                    contains: "Contains",
//                    eq: "Is equal to"
//                }
//            }
//        },
//        // delete record
//        remove: function (e) {
//            var jj = e.model.bug_id;
//            alert(jj);

//        },
//    });
//}



