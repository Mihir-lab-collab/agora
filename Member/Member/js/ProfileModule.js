
function CheckBoxOperations(MethodName, ProfileID, ModuleID, IsAdmin) {
    $.ajax({
        url: "ProfileModule.aspx/" + MethodName + "?ProfileID=" + ProfileID + "&Moduleid=" + ModuleID + "&IsAdmin=" + IsAdmin,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        success: function (data) {
            //called when successful

        },
        error: function (e) {
            //called when there is an error
            //console.log(e.message);
            alert('Error' + e.message);


        }
    });
}

function UpdateProfile(MethodName, ProfileID, Name, IsAdmin, LocationID, reportingmanagerID) {

    if (ProfileID == "")
        ProfileID = "0"
    $.ajax({
        url: "ProfileModule.aspx/" + MethodName + "?ProfileID='" + ProfileID + "'&Name='" + Name + "'&IsAdmin='" + IsAdmin + "'&LocationID='" + LocationID + "'&reportingmanagerID='" + reportingmanagerID + "'",
        //url: "ProfileModule.aspx/" + MethodName + "?ProfileID='" + ProfileID + "'&Name='" + Name + "'&IsAdmin='" + IsAdmin + "'&LocationID='" + LocationID + "'",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        success: function (data) {
            if (data.d == 0) {
                alert('Duplicate Profile Module Name Not Allowed');
            }


        },
        error: function (e) {

        }
    });
}

$(document).ready(function () {

    ReportingmanagerData();

    $(document).on('click', '#BtnAddProfile', function (e) {
        $('#AddProfilePopUp').show();
        $('[id$="BtnUpdate"]').hide();
    });

    $(document).on('click', '#gridProfile .k-grid-KRA', function (e) {
        e.preventDefault();
        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();
        FillKRANames(ProfileID);
        $('#divKRANames').css('display', '');
        $('#divAddPopupOverlay').addClass('k-overlay');
    });


    $(document).on('click', '#gridProfile .k-grid-edit', function (e) {
        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();
        $('#TxtProfileID').val(ProfileID);

        //Added by AP
        var Name = $(this).parents('tr:first').find('span.ProfileName').text();
        $('[id$="TxtProfileName"]').val(Name);

        var IsAdmin = $(this).parents('tr:first').find('input.ProfileAdmin');
        if (IsAdmin[0].checked) {

            $('[id$="ChkIsAdmin"]').attr("checked", true);
        }
        else {
            $('[id$="ChkIsAdmin"]').attr("checked", false);

        }
        var Location = $(this).parents('tr:first').find('span.ProfileLocation').text();
        var ddlLocation = $("#Locations").data("kendoDropDownList")
        ddlLocation.search(Location);

        //End by AP
        var Reportingmanager = $(this).parents('tr:first').find('span.Profilereportingmanager').text();
        var ddlReportingmanager = $("#reportingmanager").data("kendoDropDownList")
        ddlReportingmanager.search(Reportingmanager);
        $('#AddProfilePopUp').show();
        $('[id$="BtnAdd"]').hide();

    });



    $(document).on('click', '[id$="BtnAdd"]', function (e) {

        var valid = GetDataOnInsert();

        if (valid == false)
            return;
        var ProfileID = $('#TxtProfileID').val();
        var Name = $('#TxtProfileName').val();
        var IsAdmin = $('#ChkIsAdmin').is(':checked');
        var dropdownlist = $("#Locations").data("kendoDropDownList");

        var LocationID = dropdownlist.value();

        var ddlreportingmanager = $("#reportingmanager").data("kendoDropDownList");
        var reportingmanagerID = ddlreportingmanager.value();
        //alert("sd");
        UpdateProfile("InsertProfile", ProfileID, Name, IsAdmin, LocationID, reportingmanagerID);
        closeAddPopUP();

    });

    $(document).on('click', '[id$="BtnUpdate"]', function (e) {
        var valid = GetDataOnInsert();

        if (valid == false)
            return;
        var ProfileID = $('#TxtProfileID').val();
        var Name = $('#TxtProfileName').val();
        var IsAdmin = $('#ChkIsAdmin').is(':checked');
        var dropdownlist = $("#Locations").data("kendoDropDownList");
        var LocationID = dropdownlist.value();
        var ddlreportingmanager = $("#reportingmanager").data("kendoDropDownList");
        var reportingmanagerID = ddlreportingmanager.value();

        UpdateProfile("UpdateProfile", ProfileID, Name, IsAdmin, LocationID, reportingmanagerID);
        closeAddPopUP();
    });

    $(document).on('click', '[id$="BtnClose"]', function (e) {
        //'#BtnClose'
        closeAddPopUP();
        //$("#AddProfilePopuUp").hide();
    });

    var LocationData = new kendo.data.DataSource({
        transport:
            {
                read:
                    {
                        url: "ProfileModule.aspx/GetLocations",
                        contentType: "application/json; charset=utf-8",
                        type: "GET"

                    }
            },
        schema:
            {
                data: "d",
                total: function (response) { // For grid item count botttom right of grid
                    return $(response.d).length;
                }
            },
        pageSize: 50
    });
    $("#Locations").kendoDropDownList({
        dataTextField: "LocationName",
        dataValueField: "LocationID",
        dataSource: LocationData
    });

    function ReportingmanagerData() {
        $.ajax(
              {
                  type: "GET",
                  url: "ProfileModule.aspx/GetReportingManagers",
                  contentType: "application/json;charset=utf-8",
                  data: "{}",
                  dataType: "json",
                  success: function (msg) {
                      $("#reportingmanager").kendoDropDownList({
                          optionLabel: "Select Account Manager",
                          dataTextField: "empName",
                          dataValueField: "empid",
                          dataSource: jQuery.parseJSON(msg.d),
                      }).data("kendoDropDownList");
                  },
                  error: function (x, e) {
                      alert("The call to the server side failed. "
                            + x.responseText);
                  }
              }
        );
    }

    //var ReportingmanagerData = new kendo.data.DataSource({
    //    transport:
    //        {
    //            read:
    //                {
    //                    url: "ProfileModule.aspx/GetReportingManagers",
    //                    contentType: "application/json; charset=utf-8",
    //                    type: "GET"

    //                }
    //        },
    //    schema:
    //        {
    //            data: "d",
    //            total: function (response) { // For grid item count botttom right of grid
    //                return $(response.d).length;
    //            }
    //        },
    //    pageSize: 50
    //});


    //$("#reportingmanager").kendoDropDownList({
    //    dataTextField: "empName",
    //    dataValueField: "empid",
    //    dataSource: ReportingmanagerData
    //});

    $(document).on('change', '.ProfileAdmin', function (e) {
        var IsChecked = $(this).is(":checked");
        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();
        var ModuleID = $(this).parents('tr:first').find('span.ModuleSpan').text();
        var IsAdmin = $(this).parents('tr:first').find('.ModuleAdminChecked').is(':checked');
        if (IsChecked) {
            CheckBoxOperations("UpdateAdminProfile", ProfileID, ModuleID, IsAdmin);
        }
        else {
            CheckBoxOperations("UpdateAdminProfile", ProfileID, ModuleID, IsAdmin);
        }
    });
    $(document).on('change', '.ModuleChecked', function (e) {
        var IsChecked = $(this).is(":checked");

        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();
        var ModuleID = $(this).parents('tr:first').find('span.ModuleSpan').text();
        var IsAdmin = $(this).parents('tr:first').find('.ModuleAdminChecked').is(':checked');

        if (IsChecked) {
            $(this).parents('tr:first').next().find('.ModuleDetailChecked').prop('checked', true);
            CheckBoxOperations("InsertProfileModule", ProfileID, ModuleID, IsAdmin);
        }
        else {

            $(this).parents('tr:first').find('.ModuleAdminChecked').prop('checked', false);
            $(this).parents('tr:first').next().find('.ModuleDetailChecked').prop('checked', false);
            CheckBoxOperations("DeleteProfileModule", ProfileID, ModuleID, IsAdmin);
        }
    });
    $(document).on('change', '.ModuleAdminChecked', function (e) {

        var IsChecked = $(this).is(":checked");
        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();

        var ModuleID = $(this).parents('tr:first').find('span.ModuleSpan').text();
        var IsAdmin = $(this).parents('tr:first').find('.ModuleAdminChecked').is(':checked');
        if (IsChecked) {
            $(this).parents('tr:first').find('.ModuleChecked').prop('checked', true);
            $(this).parents('tr:first').next().find('.ModuleDetailChecked').prop('checked', true);
            CheckBoxOperations("UpdateAdminProfileModule", ProfileID, ModuleID, IsAdmin);
        }
        else {

            CheckBoxOperations("UpdateAdminProfileModule", ProfileID, ModuleID, IsAdmin);
        }
    });
    $(document).on('change', '.ModuleDetailChecked', function (e) {
        var IsChecked = $(this).is(":checked");

        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();
        var ModuleID = $(this).parents('tr:first').find('span.ModuleSpan').text();
        var IsAdmin = $(this).parents('tr:first').find('.ModuleDetailAdminChecked').is(':checked');



        if (IsChecked) {
            var CheckedCount = $(this).parents('.Timesheet:first').find('.ModuleDetailChecked:checked').length;
            var TotalCount = $(this).parents('.Timesheet:first').find('.ModuleDetailChecked').length;
            if (CheckedCount == TotalCount) {
                $(this).parents('tr.k-detail-row:first').prev().find('.ModuleChecked').prop('checked', true);
            }
            else {
                $(this).parents('tr.k-detail-row:first').prev().find('.ModuleChecked').prop('checked', false);
            }
            CheckBoxOperations("InsertProfileModule", ProfileID, ModuleID, IsAdmin);
        }
        else {
            var CheckedCount = $(this).parents('.Timesheet:first').find('.ModuleDetailChecked:checked').length;
            var TotalCount = $(this).parents('.Timesheet:first').find('.ModuleDetailChecked').length;
            if (CheckedCount == 0) {

                $(this).parents('tr.k-detail-row:first').prev().find('.ModuleChecked').prop('checked', false);
                $(this).parents('tr.k-detail-row:first').prev().find('.ModuleChecked').change();

            }
            $(this).parents('tr:first').find('input')[1].checked = false;
            CheckBoxOperations("DeleteProfileModule", ProfileID, ModuleID, IsAdmin);
        }
    });
    $(document).on('change', '.ModuleDetailAdminChecked', function (e) {

        var IsChecked = $(this).is(":checked");
        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();
        $(this).parents('tr:first').find('input:first')[0].checked = true;
        var ModuleID = $(this).parents('tr:first').find('span.ModuleSpan').text();
        var IsAdmin = $(this).parents('tr:first').find('.ModuleDetailAdminChecked').is(':checked');
        if (IsChecked) {
            CheckBoxOperations("UpdateAdminProfileModule", ProfileID, ModuleID, IsAdmin);
        }
        else {

            CheckBoxOperations("UpdateAdminProfileModule", ProfileID, ModuleID, IsAdmin);
        }
    });
    $(document).on('click', '.k-grid-Detail', function (e) {
        var ProfileID = $(this).parents('tr:first').find('span.ProfileSpan').text();
        BindProfileModuleGrid(ProfileID);
        $("#PorfileModulePopuUp").show();
    });


    var ProfileData = new kendo.data.DataSource({
        transport:
            {
                read:
                    {
                        url: "ProfileModule.aspx/GetProfile?ProfileID=''",
                        contentType: "application/json; charset=utf-8",
                        type: "GET"

                    }
            },
        schema:
            {
                data: "d",
                total: function (response) { // For grid item count botttom right of grid
                    return $(response.d).length;
                }
            },
        pageSize: 50
    }); //----> End Data store <----//



    var element = $("#gridProfile").kendoGrid({
        dataSource: ProfileData,
        width: 100,
        //height: 900,
        // sortable: true,
        // pageable: false,
        // detailTemplate: kendo.template($("#template").html()),
        // detailInit: detailInit,
        //dataBound: function () {
        //    this.expandRow(this.tbody.find("tr.k-master-row").first());
        //},
        selectable: 'row',
        //  toolbar: ["create"],
        columns: [
            {
                field: "ProfileID",

                title: "ID",
                width: "8px",
                template: '<span class="ProfileSpan">#= ProfileID #<span>'
            },
            {
                field: "Name",
                title: "Name",
                width: "20px",
                template: '<span class="ProfileName">#= Name #<span>'
            },
            {
                field: "IsAdmin",
                title: "Admin Access",
                width: "15px",
                template: '<input class="ProfileAdmin" type="checkbox" #= IsAdmin ? "checked=checked" : "" # ></input>'

            },
            //{
            //    field: "LocationID",
            //    title: "Location",
            //    width: "180px",
            //    hidden: true,
            //    template: '<span class="ProfileLocationID">#= LocationID #<span>'
            //   // editor: categoryDropDownEditor
            //},
            {
                field: "LocationName",
                title: "Location",
                width: "20px",
                template: '<span class="ProfileLocation">#= LocationName #<span>'
            },
            {
                field: "empName",
                title: "Reporting Manager",
                width: "20px",
                template: '<span class="Profilereportingmanager">#= empName #<span>'
            },
            {
                command: ["Detail", "edit", "KRA"], title: "&nbsp;", width: "15px"
            },

        ],
        editable: false,
        cancel: function (e) {
            e.preventDefault()

            ClosingRateWindow(e);
        },
        change: function (arg) {


            //var gview = $("#gridProfile").data("kendoGrid");
            //var selectedItem = gview.dataSource(gview.selectedItem());//.dataItem(gview.select());
            //alert(selectedItem.ID);



            //var Name = selectedItem.Name;
            //$('[id$="TxtProfileName"]').html(Name);

        },
        //Apurva
        // edit: function (e) 
        // {
        //// var editWindow = e.container.data("kendoWindow");
        //// $('#divEdit').show();

        //     if (e.model.isNew()) 
        //     {
        //         var ProfileId = e.model.ProfileID;
        //         $('[id$="hdnProfileID"]').val(ProfileId);
        //         var Name = e.model.Name;
        //         $('[id$="TxtProfileName"]').val(Name);
        //         var Location = $("#Locations").data("kendoDropDownList");
        //         Location.value(e.model.LocationName);

        //         //var ProfileAdmin=


        // }
        // },
        // save: function (e) {
        //     if (e.model.isNew()) {
        //         var ProfileIdEdit = $('[id$="hdnProfileID"]').val();


        //         var LocationIdEdit = $('[id$="DDLocations"]').val();

        //         var LocationEdit = "Mumbai";



        //         var Name = $('[id$="TxtProfileName"]').val();
        //         //var verror = $("#lblError");

        //         if (LocationIdEdit == '') {

        //         }
        //         else {

        //             UpdateProfile("UpdateProfile", ProfileID, Name, IsAdmin, LocationID);
        //            // RedirectPage();
        //         }
        //     }
        //     else {
        //         id = "0";

        //     }
        // },
        //end Ap
    });
    function categoryDropDownEditor(container, options) {
        var ddd = [
             { LocationName: "Mumbai", LocationID: "10" },
             { LocationName: "Chandigarh", LocationID: "11" },
             { LocationName: "Test", LocationID: "12" }
        ];
        $('<input required data-text-field="LocationName" data-value-field="LocationName" data-bind="value:' + options.field + '"/>')
              .appendTo(container)
              .kendoDropDownList({
                  autoBind: false,
                  dataSource: ddd
              });
    }

});

function BindProfileModuleGrid(ProfileID) {

    //var ProjectCount = selectedItem.ProjectCounts;
    $.ajax({
        url: "ProfileModule.aspx/GetParentModules?ProfileID=" + ProfileID,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        //data: "{ EmpId:'" + selectedItem.empid + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            
            var GetProfileName = "";
            //var obj = $.parseJSON(msg.d); 

            $.each(msg.d, function myfunction(i, obj) {
                if (obj.ProfileName != "") {
                    GetProfileName = obj.ProfileName;
                }
            })

            if (GetProfileName != "") {
                jQuery("label[for='ProfileName']").html(GetProfileName);
            }
            GetParentProfileModule(msg);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });

    function GetParentProfileModule(Tdata) {

        //var ctkGridData = Tdata;
        //var ctkGridData = new kendo.data.DataSource({
        //    transport:
        //        {
        //            read:
        //                {
        //                    url: "ProfileModule.aspx/GetParentModules?ProfileID=" + ProfileID,
        //                    contentType: "application/json; charset=utf-8",
        //                    type: "GET"

        //                }
        //        },
        //    schema:
        //        {
        //            data: "d",
        //            total: function (response) { // For grid item count botttom right of grid
        //                return $(response.d).length;
        //                
        //            }
        //        },
        //    pageSize: 10
        //}); //----> End Data store <----//


        var element = $("#grid").kendoGrid({
            dataSource: new kendo.data.DataSource({
                data: Tdata,
                schema: {
                    data: "d",
                    total: function (Tdata) { // For grid item count botttom right of grid
                        return $(Tdata.d).length;
                    }
                },
                pageSize: 20,
            }),

            //dataSource: Tdata,

            //// height: 900,
            //// sortable: true,
            //// pageable: false,
            //schema:
            //    {
            //        data: "d",
            //        total: function (Tdata) { // For grid item count botttom right of grid
            //            return $(Tdata.d).length;
            //            
            //        }
            //    },

            columns: [
                        {
                            field: "ProfileID",
                            hidden: true,
                            title: "ProfileID",
                            //width: "100px",
                            template: '<span class="ProfileSpan">#= ProfileID #<span>'

                        },
                        //{
                        //    field: "ProfileName",
                        //    //hidden: true,
                        //    title: "ProfileName",
                        //    width: "100px",
                        //    template: '<span class="ProfileSpan">#= ProfileName #<span>'

                        //},
                        {
                            field: "ModuleID",
                            hidden: true,
                            title: "ModuleID",
                            template: '<span class="ModuleSpan">#= ModuleID #<span>'

                        },
                {
                    field: "Name",
                    title: "Name",
                    //width: "50px"
                },
                   {
                       field: "IsChecked",
                       title: "Module Access",
                       template: '<input class="ModuleChecked" type="checkbox" #= IsChecked ? "checked=checked" : "" #  ></input>',
                       //width: "40px"
                   }
                   ,
                   {
                       field: "IsAdmin",
                       title: "Admin Access",
                       template: '<input class="ModuleAdminChecked" type="checkbox" #= IsAdmin ? "checked=checked" : "" # ></input>',
                       //width: "50px"
                   }
            ],
            detailTemplate: kendo.template($("#template").html()),
            detailInit: detailInit,
            dataBound: function () {
                this.expandRow(this.tbody.find("tr.k-master-row").first());
                //this.expandRow(this.tbody.find("tr.k-master-row").width(percentage));
            },
        });
        // $("#grid").find('.k-grid-header').hide();

    }


}

function detailInit(e) {
    var detailRow = e.detailRow;
    options:
        {
            ProfileID: e.data.ProfileID;//1
            ModuleID: e.data.ModuleID;//1
        }
    var ctkGridData_1 = new kendo.data.DataSource({
        transport:
            {
                read:
                    {
                        url: "ProfileModule.aspx/GetModules?ProfileID=" + e.data.ProfileID + "&ModuleID=" + e.data.ModuleID,
                        contentType: "application/json; charset=utf-8",
                        type: "GET"

                    }
            },
        schema:
            {
                data: "d",
                total: function (response) { // For grid item count botttom right of grid
                    return $(response.d).length;
                }
            },
        //pageSize: 10
    });//----> End Data store <----//

    detailRow.find(".Timesheet").kendoGrid({
        dataSource: ctkGridData_1,
        scrollable: false,
        // sortable: true,
        // pageable: true,
        //added new
        // detailTemplate: kendo.template($("#template1").html()),
        //detailInit: detailInit1,
        dataBound: function () {
            //this.expandRow(this.tbody.find("tr.k-master-row").first());
        },
        //

        columns: [

                                        {
                                            field: "ProfileID",
                                            hidden: true,
                                            title: "ProfileID",
                                            width: "100px",
                                            template: '<span class="ProfileSpan">#= ProfileID #<span>'

                                        },
                                        {
                                            field: "ModuleID",
                                            hidden: true,
                                            title: "ModuleID",
                                            template: '<span class="ModuleSpan">#= ModuleID #<span>'

                                        },
            {
                field: "Name",
                title: "Name",
                //width: "40px"
            },
               {
                   field: "IsChecked",
                   title: "IsChecked",
                   template: '<input  class="ModuleDetailChecked" type="checkbox" #= IsChecked ? "checked=checked" : "" #  ></input>',
                   //width: "40px"

               }
               ,
               {
                   field: "IsAdmin",
                   title: "IsAdmin",
                   template: '<input  class="ModuleDetailAdminChecked" type="checkbox" #= IsAdmin ? "checked=checked" : "" # ></input>',
                   //width: "50px"

               }
        ]
    });
    $(".Timesheet").find('.k-grid-header').hide();
}
function closePorfileModulePopuUp() {
    $("#PorfileModulePopuUp").hide();
    $('#grid').html("");
}


function closeAddPopUP() {
    $('#PorfileModulePopuUp').css('display', 'none');
    $('#AddProfilePopUp').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    window.location.reload();
}
function ClosingRateWindow(e) {

    var grid = $(gridProfile).data("kendoGrid");
    grid.refresh();

}


////////////////////
function closePopUP() {
    // $('#txtDevelopmentTeam').html('');
    $('#divKRANames').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function openPopup(e) {

}

function FillKRANames(ProfileID) {
    

    $.ajax(
          {
              type: "POST",
              async: true,
              url: "ProfileModule.aspx/BindKRAS",
              contentType: "application/json;charset=utf-8",
              data: "{'prfileid':'" + ProfileID + "'}",
              dataType: "json",
              success: function (msg) {
                  
                  var dvalueobj = jQuery.parseJSON(msg.d);
                  var jsonData = msg.d;

                  var listItems = [];
                  for (var i = 0; i < dvalueobj.length; i++) {
                      //  alert(dvalueobj.length);
                      if (i % 2 == 0) {
                          listItems.push('<option value="' + dvalueobj[i].KRAID + '" title="' + dvalueobj[i].Title + '">' + dvalueobj[i].KRANames + '</option>');
                          //StyleSheet = "color:#2e2e2e; background:#cbc8c8;";
                      }
                      else {
                          listItems.push('<option value="' + dvalueobj[i].KRAID + '" title="' + dvalueobj[i].Title + '">' + dvalueobj[i].KRANames + '</option>');

                          //StyleSheet = "color:#2e2e2e; background:#eceaea;";
                      }


                  }
                  $('[id$="lstAddKRA"]').empty();

                  $('[id$="lstAddKRA"]').append(listItems.join(''));

              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }

          }

    );
}




