$(document).ready(function () {

    var validator = $("#tickets").kendoValidator().data("kendoValidator"),
           status = $(".status");
    //if ($('[id$="hdSessionProjId"]').val() != '') {
    GetPagesize();
    BindProjectbyId();
    $('[id$="hdSessionProjId"]').val('');
    //}
    //else {
    //GetPagesize();
    //  GetData();
    //}


    //GetData();

    //BindAllDateCalender();
});

function BindProjectbyId() {

    $.ajax(
          {
              type: "POST",
              url: "ProjectTeam.aspx/BindProjectList",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: false,
              success: function (msg) {
                  GetData(jQuery.parseJSON(msg.d));
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function FillDiscountDropDown() {

    $.ajax(
          {
              type: "POST",
              url: "ProjectTeam.aspx/FillDiscountDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: false,
              success: function (msg) {
                  $("#cboDiscount").kendoDropDownList({
                      optionLabel: "Select Discount",
                      //dataTextField: data.Discount,
                      //dataValueField: data.Discount,
                      change: OnDiscountChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function OnEmployeeChange() {
    $('[id$="hdempId"]').val($("#txtEmployee").val());
}
function OnProjectChange() {
    $('[id$="hdprojId"]').val($("#txtProjectTile").val());
}


function OnDiscountChange() {
    $('[id$="hdDiscount"]').val($("#cboDiscount").val());
}

function GetPagesize() {
    $("#comboBox").width(70).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: [
            { text: 50 },
            { text: 100 },
            { text: 200 },
            { text: 300 },
            { text: 500 }
        ],
        change: function (e) {
            var grid = $("#gridProject").data("kendoGrid");
            grid.dataSource.pageSize(this.value());
        }
    });
}

function BindAllDateCalender() {
    //$("#ModifiedOn").kendoDatePicker({ format: "dd/MMM/yyyy" });
    //$("#gridProject").data("kendoGrid").columns[4].field.kendoDatePicker({ format: "dd-MMM-yyyy" });
}

function ShowAddPopup() {
    $('[id$="chkIsActive"]').prop("checked", false);
    openAddPopUP();
}

function openAddPopUP() {
    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');

    FillProjectDropDown();
    FillEmployeeDropDown();
    FillDiscountDropDown();
    //SetDefualtCustomer();
    //$('#hdDiscount').val = '';

}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    clearFields();
}

function FillEmployeeDropDown() {
    $.ajax({
              type: "POST",
              url: "ProjectTeam.aspx/FillEmployeeDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: false,
              success: function (msg) {
                  $("#txtEmployee").kendoDropDownList({
                      optionLabel: "Select Employee",
                      dataTextField: "empName",
                      dataValueField: "empId",
                      change: OnEmployeeChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");
              },
              error: function (x, e) {
                  alert("The call to the server side failed. "
                        + x.responseText);
              }
          }
    );
}

function FillProjectDropDown() {
    $.ajax(
          {

              type: "POST",
              url: "ProjectTeam.aspx/FillProjectDropDown",
              contentType: "application/json;charset=utf-8",
              data: "{}",
              dataType: "json",
              async: false,
              success: function (msg) {
                  //
                  $("#txtProjectTile").kendoDropDownList({
                      optionLabel: "Select Project",
                      dataTextField: "projName",
                      dataValueField: "projId",
                      change: OnProjectChange,
                      dataSource: jQuery.parseJSON(msg.d)
                  }).data("kendoDropDownList");
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
        height: 500,
        dataSource: {
            data: Tdata,//jQuery.parseJSON(msg.d),
            schema: {
                model: {
                    fields: {
                        projName: { type: "string" },
                        projId: { type: "number" },
                        empId: { type: "number" },
                        empName: { type: "string" },
                        Discount: { type: "number" },
                        ModifiedOn: { type: "date", format: "{0:dd-MMM-yyyy}" },
                        MemberId: { type: "number" },
                        Isactive: { type: "number" },
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
        {
            command: [
                            {
                                name: "edit", click: EditProject
                            },
            ],
            width: "45px", attributes: { style: "text-align:center;" }
        },
        { field: "projName", title: "Project Title", width: 120, },
        { field: "empName", title: "Employee Name", width: 120, },
        { field: "Discount", title: "Discount", width: 120, },
        { field: "ModifiedOn", title: "Modified Date", width: 50, template: "#= kendo.toString(ModifiedOn, 'dd-MMM-yyyy')#" }
        //{ field: "Isactive", title: "Active", width: 120, },
        ],
        filterable:
        {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    contains: "Contains",
                    eq: "Is equal to"
                }
            }
            //ui:function(element){
            //    element.kendoDatePicker({
            //        format: "{0:dd-MMM-yyyy}"
            //    });
            //}
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
                    //id, $("#txtEditCustomer").val(), e.model.projName, e.model.projDesc, $("#txtEditProjectManager").val(), (e.model.projStartDate == '') ? '' : kendo.toString(e.model.projStartDate, 'dd/MM/yyyy'), $("#txtEditProjectDuration").val(),
                    // (e.model.projActComp == '') ? '' : kendo.toString(e.model.projActComp, 'dd/MM/yyyy'), $("#txtEditPaymentCurrency").val(), e.model.currExRate, e.model.projCost, $("#txtEditProcurementMonth").val(), $("#txtEditLastPaymentMonth").val(),
                    //$("#txtEditCodeReviewTeam").data("kendoMultiSelect").value().toString(), e.model.OtherEmailId, $("#txtEditDevelopmentTeam").data("kendoMultiSelect").value().toString(), $("input[name=chkModuleIDEdit]:checked").map(function () { return this.value; }).get().join(","));

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
    //alert("sucess");
    //    },
    //    error: function (x, e) {
    //        alert("The call to the server side failed. "
    //              + x.responseText);
    //    }
    //});
}

function EditProject(e) {

    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    openAddPopUP();
    //  $('#ctl00_ContentPlaceHolder1_btnAddMilstone').attr("style", 'display:block')
    $("#ctl00_ContentPlaceHolder1_btnAddMilstone").removeClass("displayNone");
    $("#ctl00_ContentPlaceHolder1_btnAddMilstone").addClass("small_button white_button open");

    //var Id = data.Id;
    //$('[id$="hdId"]').val(Id);

    var projectId = data.projId;
    $('[id$="hdprojId"]').val(projectId);

    var empId = data.empId;
    $('[id$="hdempId"]').val(empId);

    var discount = data.Discount;
    $('[id$="hdDiscount"]').val(discount);

    var discount = "UPDATE";
    $('[id$="hdMode"]').val(discount);

    var memberid = data.MemberId;
    $('[id$="hdMemberId"]').val(memberid);




    if (data.projId != "") {
        $("#txtProjectTile").data("kendoDropDownList").value(data.projId);

    }
    if (data.empId != "") {
        $("#txtEmployee").data("kendoDropDownList").value(data.empId);
    }

    $("#cboDiscount").data("kendoDropDownList").value(data.Discount);

    $('[id$="chkIsActive"]').prop("checked", (true));
}

function SetDefualtEmployee() {
    EmployeeID = window.location.search.substring(1);
    var n = EmployeeID.split("=");
    ID = CustomerID.split("=");
    if (ID[1] != null) {

        var dropdownlist = $("#txtEmployee").data("kendoDropDownList");
        var value = dropdownlist.value();
        dropdownlist.value(ID[1]);
        dropdownlist.enable(false);
    }
}

function closePopUP() {
    $('#divPopUP').css('display', 'None');
    $('#divOverlay').removeClass('k-overlay').addClass("k-overlayDisplaynone");
}

function clearFields() {

    $('[id$="hdprojId"]').val('');
    $('[id$="hdempId"]').val('');
    $('[id$="hdDiscount"]').val('');
    $('[id$="hdMode"]').val('');
    $('[id$="hdMemberId"]').val('1');
    $('[id$="txtProjectTile"]').val('');
    $('[id$="txtEmployee"]').val('');
    $('[id$="cboDiscount"]').val('');
}


