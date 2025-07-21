$(document).ready(function () {

    GetComplaintDetails();

});
function GetComplaintDetails() {
    $.ajax({
        type: "POST",
        url: "Complaint.aspx/BindComplaints",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetAllCompaints(jQuery.parseJSON(msg.d));

        }
    });

}
function GetAllCompaints(Tdata) {
    var projname = [
            { field: "projName", title: "Project Name", width: "130px" },
            { field: "custCompany", title: "Customer Company", width: "130px" },
    ];
    $("#grid").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        compId: { type: "number" },//, editable: false
                        projName: { type: "string" },
                        compDate: { type: "date", format: "{0:dd-MMM-yyyy}" },
                        compTitle: { type: "string" },
                        compDesc: { type: "string" },
                        compFeedback: { type: "string" },
                        compResolved: { type: "binary" },//, operator: "eq"
                        compProjctId: { type: "number" },
                        compCategory: { type: "string" },
                        custName: { type: "string" },
                        custRegDate: { type: "string" },
                        custAddress: { type: "string" },
                        custEmail: { type: "string" },
                        custCompany: { type: "string" },
                        projId: { type: "number" },
                    }
                }
            },

            pageSize: 20,
            //serverPaging: true,
            //serverFiltering: true,
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
                },
                binary: {                   
                    eq: "Is equal to"
                }
            }
        },
        edit: function (e) {
         
                var editWindow = e.container.data("kendoWindow");
                if (e.model.isNew())
                {
                    var compId = e.model.compId;
                    var compDate = e.model.compDate;
                    var cDate = new Date(Date.parse(compDate));
                    var day = cDate.getDate();
                    var compFeedback = e.model.compFeedback;
                    var compTitle = e.model.compTitle;
                    var compResolved = e.model.compResolved;

                    if (day.toString().length <= 1)
                    {
                        day = "0" + day;
                    }
                    var month = cDate.getMonth() + 1;
                    if (month.toString().length <= 1) {
                        month = "0" + month;
                    }
                    var year = cDate.getFullYear();
                    var compDt = day + '/' + month + '/' + year;

                    //BindAllEditDateCalender();
                    $('[id$="hdncompId"]').val(compId);

                    e.container.data("kendoWindow").title('Complaints Details');
                    var width = $("#trConfig").width();

                    $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();

                    updateButton = e.container.find(".k-grid-update");
                    cancelbutton = e.container.find(".k-grid-cancel");
                    $("#tdUpdate").append(updateButton);
                    $("#tdUpdate").append(cancelbutton);
                }
          
                else {
                    e.container.data("kendoWindow").title('Complaint Details');
                }
           


        },
        save: function (e) {
            if (e.model.isNew()) {
                e.preventDefault();
               var compId = $('[id$="hdncompId"]').val();                       
                var compResolved = $('[id$="Editresolve"]').val();
                var compFeedback = $('[id$="EditFeedback"]').val();
                var verror = $("#lblError");

                if (compId == '' ) {
                    
                }
                else {
                   
                    UpdateComplaint(compId,compResolved,compFeedback);                  
                }
            }
            else {
                id = "0";

            }
        },

        remove: function (e) {
            e.preventDefault();
            if (confirm("Are you sure you want to delete?")) {
                DeleteComplaint(e.model.compId)//(e.model.compId);
            } else {

                e.preventDefault();
                ClosingRateWindow(e);
            }
        },

        cancel: function (e) {
            e.preventDefault()
          
            ClosingRateWindow(e);
            //window.location.reload();
        },
        

        pageable: true,
        columns: [
             { command: [{ name: "edit", text: "" }, { name: "destroy", text: "" }], width: "100px" },
             { field: "compCategory", title: "Category", width: "100px" },
             { field: "compDate", title: "Date", width: "50px", format: "{0:dd-MMM-yyyy}", width: "130px" },
             { field: "projName", title: "Project Name", width: "90px", template: "#= projName + ' (' + custCompany #" + ')' },
             { field: "compTitle", title: "Title", width: "90px" },
             { field: "compResolved", title: "Resolved", width: "90px" },
             { field: "custName", title: "Customer Name", width: "90px", hidden: true },
             { field: "custCompany", title: "Customer Company", width: "90px" },
             { field: "custEmail", title: "Customer Email", width: "90px", hidden: true },
             { field: "custAddress", title: "Customer Address", width: "90px", hidden: true },
             { field: "compId", title: "Id", width: "100px", hidden: true },
             { field: "compDesc", title: "Description", width: "200px", height: "10px" },
             { field: "compFeedback", title: "Feedback", width: "200px", hidden: true },
        ],

    });
}

function DeleteComplaint(compId) {
    $.ajax(
           {

               type: "POST",
               url: "Complaint.aspx/DeleteComplaint",
               contentType: "application/json;charset=utf-8",
               data: "{'compId':'" + compId + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert('complaint deleted successfully.');
                   $('body').css('cursor', 'default');
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                          + msg.responseText);
               }
           }
        );
}

function closeAddPopUP() {

    $('#divAddPopup').css('display', 'none');
    window.location.reload();
}

function UpdateComplaint(compId,compResolved,compFeedback) {

    $.ajax(
           {

               type: "POST",
               url: "Complaint.aspx/UpdateComplaint",
               contentType: "application/json;charset=utf-8",
               data: "{'compId':'" + compId + "','compResolved':'" + compResolved + "','compFeedback':'" + compFeedback + "'}",
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

function BindAllEditDateCalender() {
    $("#EditcompDate").kendoDatePicker({ format: "dd/MM/yyyy" });
}


function ClosingRateWindow(e) {
    $('#grid').data('kendoGrid').refresh();

}


function BindAllEditDateCalender() {
    $("#EditcompDate").kendoDatePicker({ format: "dd/MM/yyyy" });
}
function InitialiseControlsAs(projId) {

    var Priority = $("#EditprojName").kendoDropDownList({        
        dataTextField: "projName",
        dataValueField: "projId",
        dataSource: projId,
        
    }).data("kendoDropDownList");
}

function InitialisecategoryControlsAs(ID) {

   $('[id$="Editcategory"]').kendoDropDownList({        
        dataTextField: "compCategory",
        dataValueField: "ID",
        dataSource: ID,
    }).data("kendoDropDownList");
   
}

function getbindprojdetails() {
    $.ajax(
          {

              type: "POST",
              url: "Complaint.aspx/GetProjectNameByProjId",
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


function getbindcatdetails() {
    $.ajax(
          {

              type: "POST",
              url: "Complaint.aspx/BindCategoryDetails",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  InitialisecategoryControlsAs(jQuery.parseJSON(msg.d));
              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
              }
          }
       );

}

//function RedirectPage() {
//    window.location.assign("Complaint.aspx");
//}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    BindAllEditDateCalender();
}



