$(document).ready(function ()
{
    var search = $('[id$="searchbox"]').val();
    GetKnowledgeBaseDetails(search);

    $("#txtkbFile").kendoUpload({
        async: {
            saveUrl: "knowledgeBase.aspx",
            removeUrl: "knowledgeBase.aspx",
            autoUpload: false,
        },
        localization: {
            retry: "Done"
        },
        multiple: true
    });
});

function replaceall(str, replace, with_this) {
    var str_hasil = "";
    var temp;

    for (var i = 0; i < str.length; i++) // not need to be equal. it causes the last change: undefined..
    {
        if (str[i] == replace) {
            temp = with_this;
        }
        else {
            temp = str[i];
        }

        str_hasil += temp;
    }

    return str_hasil;
}

function getHistory(Tdata) {
    $('#cmmntgrid').kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        masterId: { type: "string" },
                        empName: { type: "string" },
                        commentHistory: { type: "string" },
                        hDate: { type: "date", format: "{0:dd-MMM-yyyy}" }

                    }

                }
            },
            pageSize: 3,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                    { field: "masterId", title: "ID", width: "50px" },
                    { field: "empName", title: "Name", width: "50px" },
                    { field: "commentHistory", title: "comments", width: "50px" },
                    { field: "hDate", title: "Date", width: "50px", format: "{0:dd-MMM-yyyy}" },

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
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
        },
        cancel: function (e) {
            e.preventDefault()

            ClosingRateWindow(e);
        },
        edit: function (e) {
        },
        save: function (e) {
        },
        remove: function (e) {
        },

    });


}

function convert(str) {

    var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

    var newDate = new Date(str);
    var formattedDate = (newDate.getDate() < 10 ? '0' : '') + newDate.getDate() + '-' + monthNames[newDate.getMonth()] + '-' + newDate.getFullYear();

    return formattedDate;
}
function GetKnowledgeBaseDetails(Techname) {
   
    $("#history").html('');
    $.ajax({
        type: "POST",
        url: "KnowledgeBase.aspx/BindKnowledgeBase",
        contentType: "application/json;charset=utf-8",
        data: "{'techName':'" + Techname + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
         
            GetAll(jQuery.parseJSON(msg.d));         
        }
    });

}
function GetAll(Tdata) {
    //$("#grid").html('');
    $("#grid").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        kbId: { type: "number" },
                        empId: { type: "number" },
                        empName: { type: "string" },
                        kbDate: { type: "date", format: "{0:dd-MMM-yyyy}" },
                        kbDescrptn: { type: "string" },
                        kbComments: { type: "string" },
                        kbFile: { type: "string" },
                        kbTitle: { type: "string" },
                        techId: { type: "number" },
                        techname: { type: "string" },
                        subtechName: { type: "string" },
                        projId: { type: "number" },
                        Url: { type: "string" },
                        projName: { type: "string" }
                    }
                }
            },
            pageSize: 100,
        },
        scrollable: true,
        sortable: true,
        editable: true,
        editable: {
            mode: "popup",
            template: kendo.template($("#popup-editor").html())
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
        edit: function (e) {
            var editWindow = e.container.data("kendoWindow");
            if (e.model.isNew()) {

                var kbId = e.model.kbId;
                var kbTitle = e.model.kbTitle;
                var kbComments = e.model.kbComments;
                var kbDescrptn = e.model.kbDescrptn;
                //var kbFile = e.model.kbFile;
                var empId = e.model.empId;
                var empName = e.model.empName;
                var techId = e.model.techId;
                var subtechName = e.model.subtechName;
                var projId = e.model.projId;
                var Url = e.model.Url;

                $('[id$="hdnempName"]').val(empName);

                $('[id$="attachUrl"]').html("");
                var Anchor = $('<a target="_blank" href="http://' + Url + '">' + Url + '</a>');

                $('[id$="attachUrl"]').append(Anchor);

                $('[id$="attachUrl"]').append('<br/>');

                getbindEmpdetails();
                getbindTech();
                GetDownloadedFile(kbId);

                var dropdownlist = $("#EdittechId").data("kendoDropDownList");
                dropdownlist.value(techId);

                $('[id$="hdnkbId"]').val(kbId);
                $("#EditkbFile").kendoUpload({
                    async: {
                        saveUrl: "knowledgeBase.aspx",
                        removeUrl: "knowledgeBase.aspx",
                        autoUpload: false
                    },
                    localization: {
                        retry: "Upload successfully.........."
                    },
                });
                e.container.data("kendoWindow").title('Knowledge Base');
                var width = $("#trConfig").width();
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
            if (e.model.isNew()) {

                var kbId = $('[id$="hdnkbId"]').val();
                var kbTitle = $('[id$="EditTitle"]').val();
                var kbDescrptn = $('[id$="EditkbDescrptn"]').val();
                var kbComments = $('[id$="EditnewkbComments"]').val();
                var techId = $('[id$="EdittechId"]').val();
                var SearchUrl = $('[id$="EditUrl"]').val();
                var subtechname = $('[id$="Edittags"]').val();
                subtechname = replaceall(subtechname, ',', '$');

                getbindprojdetails();

                var projName = $("#EditprojName").data("kendoDropDownList");
                projName.value(e.model.projName);
                var kbFile = e.model.kbFile;


                if (kbId != '') {
                    UpdateKB(kbId, kbTitle, kbComments, kbDescrptn, techId, SearchUrl, subtechname, kbFile);
                    RedirectPage();
                    // ClosingRateWindow(e);
                }

            }
        },
        remove: function (e) {
            var kbId = e.model.kbId;
            if (confirm("Are you sure you want to delete?")) {
                DeleteKnowledgebase(e.model.kbId);
            } else {

                e.preventDefault();
                ClosingRateWindow(e);
            }
        },
        cancel: function (e) {
          e.preventDefault();
            ClosingRateWindow(e);
            
            

        },
        serverPaging: true,
        pageable:true,
        //pageable: {
        //    //input: true,
        //    numeric: false,
        //    refresh: true,
           
        //},
        //pageable:false,
        columns: [
             {
                 command: [ {
                     name: "view", text: "", imageClass: "ViewData", title: "View Details", click: function (e) {                  
                         var window = $("#KBDetails");
                         var tr = $(e.target).closest("tr");
                         var data = this.dataItem(tr);


                         var kbId = data.kbId;
                         $('[id$="lblId"]').html(kbId);
                         $('[id$="hdnkbId"]').val(kbId);
                        
                         var kbTitle = data.kbTitle;
                         $('[id$="lblTitle"]').html(kbTitle);

                         var empName = data.empName;
                         $('[id$="lblName"]').html(empName);

                         var kbDate = data.kbDate;
                         $('[id$="lblDate"]').html(convert(kbDate));

                         var Url = data.Url;
                         var Anchor = $('<a target="_blank" href="http://' + Url + '">' + Url + '</a>');
                         $('[id$="lblUrl"]').html(Anchor);

                         var kbDescrptn = data.kbDescrptn;
                         $('[id$="lblDescrptn"]').html(kbDescrptn);
                        
                         GetDownloadedFile(kbId);
                     
                         openPopUP();
                     }
                 }, { name: "edit", text: "", width: "110px" }, { name: "destroy", text: ""}], width: "70px"
             },
             { field: "kbId", title: "kbId", width: "100px", hidden: true },
             { field: "empId", title: "empId", width: "50px", hidden: true },
             { field: "kbDate", title: "Date",  format: "{0:dd-MMM-yyyy}", width: "70px" },
             { field: "kbTitle", title: "Title", width: "200px" },
             { field: "projName", title: "Project Name", width: "70px" },
             { field: "empName", title: "Posted By", width: "90px" },
             { field: "kbDescrptn", title: "kbDescrptn", width: "90px", hidden: true },
             { field: "kbComments", title: "kbComments", width: "90px", hidden: true },
             { field: "kbFile", title: "kbFile", width: "90px", hidden: true },
             { field: "techName", title: "Technology", width: "90px" },
             { field: "subtechName", title: "Tags", width: "60px" }
        ],
        dataBound: function () {
            var grid = this;
            var model;

            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);
                var empName = $('[id$="hdnempName"]').val();
                if (model.empName == empName) {

                }
                else {
                    $(this).find(".k-grid-edit").remove();
                }
            });


            grid.tbody.find("tr[role='row']").each(function () {
                model = grid.dataItem(this);                
                var IsAdmin = $('[id$="hdn"]').val();                
                if (IsAdmin == "true") {
                
                }
                else {                   
                    $(this).find(".k-grid-delete").remove();
                }
            });
        }

    });
}


function DeleteKnowledgebase(kbId) {
    $.ajax(
          {

              type: "POST",
              url: "KnowledgeBase.aspx/DeleteKB",
              contentType: "application/json;charset=utf-8",
              data: "{'kbId':'" + kbId + "'}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  var message = jQuery.parseJSON(msg.d);
                  alert('deleted successfully.');
                  $('body').css('cursor', 'default');
              },
             
          }
       );

}
function ClosingRateWindow(e) {

    var grid = $("#grid").data("kendoGrid");
    grid.refresh();


}
function UpdateKB(kbId, kbTitle, kbComments, kbDescrptn, techId, SearchUrl, Subtechname, kbFile) {
    $.ajax(
           {

               type: "POST",
               url: "KnowledgeBase.aspx/UpdateComplaint",
               contentType: "application/json;charset=utf-8",
               data: "{'kbId':'" + kbId + "','kbTitle':'" + kbTitle + "','kbComments':'" + kbComments + "','kbDescrptn':'" + kbDescrptn + "','techId':'" + techId + "','Url':'" + SearchUrl + "','subtechName':'" + Subtechname + "','kbFile':'" + kbFile + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert('updated successfully.');
                   $('body').css('cursor', 'default');
                   closeAddPopUP();
                   
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );
}
function closePopUP() {
    
    $('#KBDetails').css('display', 'None');
    $("#history").html('');
    $('#divOverlay').removeClass('k-overlay').addClass("k-overlayDisplaynone");
    ClosingRateWindow();
   // RedirectPage();
}
function RedirectPage() {
    window.location.assign("KnowledgeBase.aspx");
}

function onSuccess(e) {
    return $.map(e.files, function (file) {
        if (file.size <= 10485760) {
            return true;
        }
        else {
            return false;
            alert("File size must less than 10 MB !");

        }
    }
    );
}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
   
}

function ShowAddPopup() {
    clear();
    openAddPopUP();
    getbindTech();
    getbindprojdetails();
}

function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
 }

function openPopUP() {
    $("#history").html('');
    var kbId = $('[id$="hdnkbId"]').val();
   /// GetClick(kbId);
    $('#KBDetails').css('display', '');
    $('#divOverlay').addClass('k-overlay');
    //GetHistoryDetails(kbId);
   
    GetRepeater(kbId);
  
}
function InitialiseEmployee(empId) {

    var Type = $("#txtkbempName").kendoDropDownList({
        optionLabel: "---Select Name----",
        dataSource: empId,
        dataTextField: "empName",
        dataValueField: "empId",
    }).data("kendoDropDownList");


}
function InitialiseTech(techId) {
    var tech = $("#txttechName").kendoDropDownList({
        optionLabel: "---Select Name----",
        dataSource: techId,
        dataTextField: "techName",
        dataValueField: "techId",

    }).data("kendoDropDownList");


    var tech = $("#EdittechId").kendoDropDownList({
        optionLabel: "---Select Name----",
        dataSource: techId,
        dataTextField: "techName",
        dataValueField: "techId",
    }).data("kendoDropDownList");

}
function getbindEmpdetails() {
    $.ajax(
          {

              type: "POST",
              url: "KnowledgeBase.aspx/BindEmployee",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  InitialiseEmployee(jQuery.parseJSON(msg.d));
              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
              }
          }
       );

}
function getbindTech() {
    $.ajax(
          {
              type: "POST",
              url: "KnowledgeBase.aspx/BindTech",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  InitialiseTech(jQuery.parseJSON(msg.d));
              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
              }
          }
       );

}

function getbindprojdetails() {
    $.ajax(
          {
              type: "POST",
              url: "KnowledgeBase.aspx/GetProjectNameByProjId",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  InitialiseControlsAs(jQuery.parseJSON(msg.d));
              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
              }
          }
       );

}
function InitialiseControlsAs(projId) {
    var Priority = $("#EditprojName").kendoDropDownList({
        optionLabel: "Knowledge Base",
        dataTextField: "projName",
        dataValueField: "projId",
        dataSource: projId,

    }).data("kendoDropDownList");
}

function GetRepeater(kbId) {
    $.ajax({
        type: "POST",
        url: "KnowledgeBase.aspx/getRepater",
        contentType: "application/json;charset=utf-8",
        data: "{'kbId':'" + kbId + "'}",
        dataType: "json",
        async: false,
        success: function (data) {            
            var data = jQuery.parseJSON(data.d)
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {                   
                    $("#history").append("<div class='comment' style='width: 90%;'><b style='color:#373737;'></b>" + data[i].commentHistory + "<div style='overflow:hidden; display:block; padding-top:10px;'><span style='text-align: right; height: 20px; display:inline-block'>Posted By :<b>" + data[i].empName + "</b></span><span style='text-align: right; margin-left: 5px; height: 20px; margin-bottom:20px;'>on <b>" + data[i].hDate + "</b><span></div></div>");
                }  
                }
                       
           
        },
        error: function (result) {
            alert("Error  " + result.responseText);
        }
    });

}

function onUploadRemove(e) {
    var files = e.kbFile;
    for (i = 0; i < files.length; i++) {
        files[i].name = files[i].id;
    }
}

function onUploadSuccess(e) {
    //add the id returned in the json from the upload server script
    e.files[0].id = e.response.id;
}

function GetDownloadedFile(kbId) {
    $.ajax({
        type: "POST",
        url: "KnowledgeBase.aspx/GetReportAttachments",
        contentType: "application/json;charset=utf-8",
        data: "{'kbId':'" + kbId + "'}",
        cache: false,
        async: false,
        dataType: "json",
        success: function (msg) {
            var data = jQuery.parseJSON(msg.d);
            $('[id$="attachment"]').html("");
            if (data.length > 0) {
                for (i = 0; i < data.length; i++) {
                    var Anchor = $('<a target="_blank" href="../Common/Download.aspx?m=KB&f=' + data[i].kbFile + '">' + data[i].kbFile + '</a>');
                    $('[id$="attachment"]').append(Anchor);
                    $('[id$="attachment"]').append('<br/>');
                }
            }
            else {
                var Anchor = "No Attachment."
                $('[id$="attachment"]').append(Anchor);
                $('[id$="attachment"]').addClass("ForeColor");
            }

        },

       
    });
}

function clear() {
    $("#txtkbTitle").val('');
    $("#txtkbDescrptn").html('');
    $("#hdnkbDescrptn").val('');    
    $("#txttechName").val('');
    $("#txttags").val('');
    $("#txtUrl").val('');
    
}

function GetClick(kbId) {
    $.ajax({
        type: "POST",
        url: "KnowledgeBase.aspx/BindKnowledgeBase1",
        contentType: "application/json;charset=utf-8",
        data: "{'kbId':'" + kbId + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            var kbTitle = data.kbTitle;
            $('[id$="lblTitle"]').html(kbTitle);

            var empName = data.empName;
            $('[id$="lblName"]').html(empName);

            var kbDate = data.kbDate;
            $('[id$="lblDate"]').html(convert(kbDate));

            var Url = data.Url;
            var Anchor = $('<a target="_blank" href="http://' + Url + '">' + Url + '</a>');
            $('[id$="lblUrl"]').html(Anchor);

            var kbDescrptn = data.kbDescrptn;
            $('[id$="lblDescrptn"]').html(kbDescrptn);

            GetDownloadedFile(kbId);

        }
    });
}