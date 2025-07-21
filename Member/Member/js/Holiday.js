$(document).ready(function () {
    GetHolidayDetails();
  
});

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    window.location.reload();
}

function ShowAddPopup()
{
    openAddPopUP();
   
}
function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
    BindAllDateCalender();
}

function BindAllDateCalender()
{
 $("#txtHolidayDate").kendoDatePicker({ format: "dd/MM/yyyy" });
}



function BindAllEditDateCalender()
{
 $("#txtEditHolidayDate").kendoDatePicker({ format:"dd/MM/yyyy" });
}

function GetHolidayDetails() {
   
    $.ajax({
        type: "POST",
        url: "Holiday.aspx/BindHoliday",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
        
            GetLocationData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function ClosingRateWindow(e)
{

    var grid = $(gridHoliday).data("kendoGrid");
    grid.refresh();

}

function GetLocationData(HolidyaData) {
   
    $(gridHoliday).kendoGrid({
        dataSource: {
            data: HolidyaData,
            schema: {
                model: {
                    fields: {
                        HolidayId: { type: "number" },
                        HolidayDate: { type: "string" },
                        Narration: {type:"string"}
                        }

                }
            },
            pageSize: 50,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
                    { field: "HolidayId", title: "ID", width: "50px", hidden: true },
                    { field: "HolidayDate", title: "Holiday Date", width: "50px" },
                    { field: "Narration", title: "Narration", width: "50px" },
                    { command: [{ name: "edit" }, { name: "destroy" }], width: "20px" }

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
          var editWindow = e.container.data("kendoWindow");
          $('#divEdit').show();

          if (e.model.isNew()) {
              BindAllEditDateCalender();
              // FillTextEdit();
               var HolidayId = e.model.HolidayId;
               $('[id$="hdnHolidayID"]').val(HolidayId);

               var HolidayDate = e.model.HolidayDate;
               var hdate = new Date(Date.parse(HolidayDate));
               var day = hdate.getDate();
               if (day.toString().length <= 1) {
                   day = "0" + day;
               }
               var month = hdate.getMonth() + 1;
               if (month.toString().length <= 1) {
                   month = "0" + month;
               }
               var year = hdate.getFullYear();
               var holidaydt = day + '/' + month + '/' + year;
            
               $('[id$="txtEditHolidayDate"]').val(holidaydt);
              
              var Narration = e.model.Narration;
              $('[id$="txtEditNarration"]').val(Narration);

             e.container.data("kendoWindow").title('Holiday Details');
             var width = $("#trConfig").width();
           $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
             updateButton = e.container.find(".k-grid-update");
             cancelbutton = e.container.find(".k-grid-cancel");
            $("#tdUpdate").append(updateButton);
              $("#tdUpdate").append(cancelbutton);
           }
           else {
               e.container.data("kendoWindow").title('Holiday Details');
           }
       },
        save: function (e)
        {
            if (e.model.isNew())
            {
               var HolidayEditID = $('[id$="hdnHolidayID"]').val();
             

               var LocationIdEdit = $('[id$="DDLocations"]').val();
             
              var LocationEdit = "Mumbai";
              
           
                      var HolidayDateEdit = $('[id$="txtEditHolidayDate"]').val();
                      var Narration = $('[id$="txtEditNarration"]').val();
               var verror = $("#lblError");

               if (LocationIdEdit == '')
               {
                  
               }
               else
               {
             
                   UpdateHoliday(HolidayEditID, LocationIdEdit, HolidayDateEdit, Narration);
                RedirectPage();
               }
          }
            else
            {
             id = "0";

         }
       },
       remove: function (e) {

           if (confirm("Are you sure you want to delete?"))
           {
               DeleteHoliday(e.model.HolidayId);
           } else {

               e.preventDefault()
               ClosingRateWindow(e);

           }



       },

    });

}

function UpdateHoliday(HolidayEditID, LocationIdEdit, HolidayDateEdit, Narration) {

    $.ajax(
           {

             type: "POST",
               url: "Holiday.aspx/UpdateHoliday",
               contentType: "application/json;charset=utf-8",
               data: "{'HolidayId':'" + HolidayEditID + "','LocationId':'" + LocationIdEdit + "','holidaydate':'" + HolidayDateEdit + "','narration':'" + Narration + "'}",
             cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                  alert('updated successfully.');
                  $('body').css('cursor', 'default');

              },
              error: function (msg) {
                  alert("The call to the server side failed."
                        + msg.responseText);
               }
           }
        );
}


function DeleteHoliday(HolidayId) {
   $.ajax(
          {

              type: "POST",
              url: "Holiday.aspx/DeleteHoliday",
             contentType: "application/json;charset=utf-8",
              data: "{'HolidayId':'" + HolidayId + "'}",
              cache: false,
              async: false,
               dataType: "json",
            success: function (msg) {
                  var message = jQuery.parseJSON(msg.d);
                   alert('Holiday deleted successfully.');
                  $('body').css('cursor', 'default');
             },
              error: function (msg) {
                  alert("The call to the server side failed."
                         + msg.responseText);
               }
          }
       );
}
function RedirectPage() {
    window.location.assign("Holiday.aspx");
}