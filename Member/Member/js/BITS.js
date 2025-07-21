$(document).ready(function () {
    HideLoading()//vnw 09-Jan-2023
    
    $("#grdProjectDetails  tr:has(td)").hover(function () {
        $(this).css("cursor", "pointer");
    });

    FillCheckbox();



});

function FillCheckbox() {
    $.ajax({
        type: "POST",
        url: "BITSManage.aspx/BindProjectStatusDetails",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var showOverHeads;
            var Added;
            var Initiated;
            var InProgress;
            var UnderUAT;
            var OnHold;
            var CompletedClosed;
            var Cancelled;
            var UnderWarranty;
            var TNM;
            var FixedCost;

            BindCheckBoxValue(jQuery.parseJSON(msg.d));

            var pathname = window.location.pathname; // Returns path only
             
            showOverHeads = true;
            Added = true;
            Initiated = true;
            InProgress = true;
            UnderUAT = true;
            OnHold = true;
            CompletedClosed = false;
            Cancelled = false;
            UnderWarranty = true;
            TNM = true;            
            FixedCost = true;

            //$('#chkFixedCost').removeAttr('checked');
            //FixedCost = false;

            //showOverHeads = false;
            //Added = false;
            //Initiated = false;
            //InProgress = false;
            //UnderUAT = false;
            //OnHold = false;
            //CompletedClosed = false;
            //Cancelled = false;
            //UnderWarranty = false;
            //TNM = true;
            //FixedCost = false;

            //$('[id$="tdOverHead"]').hide();
            //$('#chkIsOverHeads').removeAttr('checked');
            ////$('#chkTNM').removeAttr('checked');// default set to true and checked
            //$('#chkFixedCost').removeAttr('checked');
            //$('#checkbox0').removeAttr('checked');
            //$('#checkbox1').removeAttr('checked');
            //$('#checkbox2').removeAttr('checked');
            //$('#checkbox3').removeAttr('checked');
            //$('#checkbox4').removeAttr('checked');
            //$('#checkbox7').removeAttr('checked');

            if (pathname.toString().indexOf('BITSManage') > 0) {
                //alert("GetProjectDetails_Manage : " + GetProjectDetails_Manage);
                //GetProjectDetails_Manage(null, null, null, null, null, null, null, null, null, TNM, null);
                GetProjectDetails_Manage(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
            }
            else {                
                GetProjectDetails(false, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
            }

           
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });

}

//function openLoading() {
//    $('#divLoading').css('display', '');
//    $('#divAddLoadingOverlay').removeClass("k-overlayDisplaynone"); 
//    $('#divAddLoadingOverlay').addClass('overlyload');
//    $('#divAddLoadingOverlay').css('display', '');
//    setTimeout("closeLoading()", 3000);

//}
//function closeLoading()
//{

//    $('#divLoading').css('display', 'none');
//    $('#divAddLoadingOverlay').removeClass("overlyload").addClass("k-overlayDisplaynone");
//    $('#divAddLoadingOverlay').css('display', 'none');
//}



function showCursorPointerForTSDetails() {
    $("#grdTSDetails  tr:has(td)").hover(function () {
        $(this).css("cursor", "pointer");
    });
}


//function showCursorPointerForMonthTSDetails() {
//    $("#grdTSModuleDetails  tr:has(td)").hover(function () {
//        $(this).css("cursor", "pointer");
//    });
//}

//////////////////////////////     Project Details ///////////////////////////
function GetProjectDetails(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost) {    
    var PMID = $('[id$="hdnAdmin"]').val();
    $.ajax({
        type: "POST",
        url: "BITS.aspx/BindProjectDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'PMID':'" + PMID + "','showOverHeads':'" + showOverHeads + "','Added':'" + Added + "','Initiated':'" + Initiated + "','InProgress':'" + InProgress + "','UnderUAT':'" + UnderUAT + "','OnHold':'" + OnHold + "','CompletedClosed':'" + CompletedClosed + "','Cancelled':'" + Cancelled + "','UnderWarranty':'" + UnderWarranty + "','TNM':'" + TNM + "','FixedCost':'" + FixedCost + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {           
            GetProjectData(jQuery.parseJSON(msg.d));            
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
function BindCheckBoxValue(Tdata) {
    (function ($, kendo) {
        var NS = ".kendoExtCheckBoxList";
        var ExtCheckBoxList = kendo.ui.Widget.extend({
            /// <summary>Displays checkboxes from a kendo.data.DataSource.</summary>

            options: {
                // <summary>The data source of the widget which is used to display a list of checkboxes.</summary>
                dataSource: null,

                /// <summary>The field of the data item that provides the text content of the checkboxes.</summary>
                dataTextField: "",

                /// <summary>The field of the data item that provides the value of the widget.</summary>
                dataValueField: "",

                /// <summary>Name of the widget.</summary>
                name: "ExtCheckBoxList",

                /// <summary>Specifies the orientation of the widget. Supported values are "horizontal" and "vertical".</summary>
                orientation: "vertical"
            },

            events: [
                /// <summary>Fired when the widget is bound to data from its data source.</summary>
                "dataBound",

                /// <summary>Fired when the user selects a checkbox.</summary>
                "select"
            ],

            /// <summary>Data source for the widget.</summary>
            dataSource: null,

            init: function (element, options) {
                /// <summary>Initialize the widget.</summary>

                kendo.ui.Widget.fn.init.call(this, element, options);

                this._dataSource();

                // Read the data from the data source.
                this.dataSource.fetch();

                // Attach an event handler to the selection of a checkbox.
                this.element.on("click" + NS, ".k-checkbox-label", { sender: this }, this._onCheckBoxSelected);

                //this.element.css({ "text-align": "center;","display": "block;"});
            },

            destroy: function () {
                /// <summary>Destroy the widget.</summary>

                $(this.element).off(NS);

                kendo.ui.Widget.fn.destroy.call(this);
            },

            _dataSource: function () {
                /// <summary>Initialize the data source.</summary>

                var dataSource = this.options.dataSource;

                // If the data source is an array, then define an object and set the array to the data attribute.
                dataSource = $.isArray(dataSource) ? { data: dataSource } : dataSource;

                // If there is a data source defined already. 
                if (this.dataSource && this._refreshHandler) {
                    // Unbind from the change event.
                    this.dataSource.unbind("change", this._refreshHandler);
                } else {
                    // Create the refresh event handler for the data source change event.
                    this._refreshHandler = $.proxy(this.refresh, this);
                }

                // Initialize the data source.
                this.dataSource = kendo.data.DataSource.create(dataSource).bind("change", this._refreshHandler);
            },

            _template: function () {
                /// <summary>Get the template for a checkbox.</summary>
                //if ('#: { 1 } #' == 'Added') {
                //    var html = kendo.format("<div class='k-ext-checkbox-item' data-uid='#: uid #' data-value='#: {0} #' data-text='#: {1} #' style='margin: 5px 10px 5px 0px;display:{2};'><input type='checkbox' value='#: {0} #' class='k-checkbox' id='checkbox#: {0} #' onclick='checkVal(chkAdded)' checked/><span class='k-checkbox-label'>#: {1} #</span></div>",
                //        this.options.dataValueField, this.options.dataTextField,
                //        this.options.orientation === "vertical" ? "block" : "inline-block");
                //    return kendo.template(html);
                //}
                //if {
                var html = kendo.format("<div class='k-ext-checkbox-item' data-uid='#: uid #' data-value='#: {0} #' data-text='#: {1} #' style='margin: 5px 10px 5px 0px;display:{2};'><input type='checkbox' value='#: {0} #' class='k-checkbox' id='checkbox#:{0}#' onclick='checkVal(checkbox#:{0}#)' checked/><span class='k-checkbox-label' id='span#:{1}##:{0}#'>#: {1} #</span></div>",
                    this.options.dataValueField, this.options.dataTextField,
                    this.options.orientation === "vertical" ? "block" : "inline-block");
                return kendo.template(html);
                //}

            },

            _onCheckBoxSelected: function (e) {
                /// <summary>Handle the selection of a checkbox.</summary>

                var $target = $(this),
                    $checkBoxItem = $target.closest(".k-ext-checkbox-item"),
                    that = e.data.sender,
                    isChecked = $checkBoxItem.find(".k-checkbox").is(":checked");

                $target.prev(".k-checkbox").prop("checked", !isChecked).addClass("k-state-selected");

                var selectedUid = $checkBoxItem.attr("data-uid");
                that.trigger("select", { item: that.dataSource.getByUid(selectedUid), checked: !isChecked });
            },

            setDataSource: function (dataSource) {
                /// <summary>Sets the data source of the widget.</summary>

                this.options.dataSource = dataSource;
                this._dataSource();
                this.dataSource.fetch();
            },

            refresh: function (e) {
                /// <summary>Renders all checkboxes using the current data items.</summary>

                var template = this._template();

                // Remove all the existing items.
                this.element.empty();

                // Add each of the radio buttons.
                for (var idx = 0; idx < e.items.length; idx++) {
                    this.element.append(template(e.items[idx]));
                }

                // Fire the dataBound event.
                this.trigger("dataBound");
            },

            dataItems: function () {
                /// <summary>Gets the dataItems for the selected checkboxes.</summary>

                var dataSource = this.dataSource,
                    list = [],
                    $items = this.element.find(".k-checkbox:checked").closest(".k-ext-checkbox-item");

                $.each($items, function () {
                    var uid = $(this).attr("data-uid");
                    list.push(dataSource.getByUid(uid));
                });

                return list;
            },

            value: function () {
                /// <summary>Gets or sets the value of the checkboxlist.</summary>

                if (arguments.length === 0) {
                    var list = [];
                    var $items = this.element.find(".k-checkbox:checked").closest(".k-ext-checkbox-item");

                    // Get the value of all selected checkboxes.
                    $.each($items, function () {
                        var value = $(this).attr("data-value");
                        list.push(value);
                    });
                    return list;
                } else {
                    var that = this,
                        list = $.isArray(arguments[0]) ? arguments[0] : (typeof arguments[0] === "string" ? [arguments[0]] : []);

                    // Clear any selected checkboxes.
                    this.element.find(".k-checkbox").prop("checked", false).removeClass("k-state-selected");

                    // Check each checkbox.
                    $.each(list, function () {
                        var value = this;
                        that.element.find(kendo.format(".k-ext-checkbox-item[data-value='{0}'] .k-checkbox", value)).click();
                    });
                }
            }
        });
        kendo.ui.plugin(ExtCheckBoxList);
    })(window.kendo.jQuery, window.kendo);
    var i;
    var data = [];
    for (i = 0; i < Tdata.length; i++) {
        data.push({ id: Tdata[i].projStatusTId, name: Tdata[i].projStatusTDesc })
    }
    function appendConsole() {
        /// <summary>Add messages to a div to demonstrate the event handlers.</summary>

        var $console = $("#console"),
            html = "";

        for (var idx = 0; idx < arguments.length; idx++) {
            html += kendo.format("<div>{0}</div>", arguments[idx]);
        }

        $console.append(kendo.format("<div>{0}</div>", html));
    }
    // Initialize the kendoExtRadioButtonGroup.
    var checkBoxList = $("#checkBoxList").kendoExtCheckBoxList({
        dataSource: data,
        dataValueField: "id",
        dataTextField: "name",
        orientation: "horizontal",
        dataBound: function () {
            //appendConsole("Event: dataBound");
        },
        select: function (e) {
            //appendConsole("Event: select", kendo.format("id: {0}, value: {1}, is checked: {2}<br/>", e.item.id, e.item.name, e.checked));
        }
    }).data("kendoExtCheckBoxList");
    // Set the selected checkboxes.
    checkBoxList.value(["0", "1", "2", "3", "4", "7"]);
}

function GetProjectData(Tdata) {
    //ShowLoading();
    $('#grdProjectDetails').data().kendoGrid.destroy();
    $('#grdProjectDetails').empty();
    if ($("#grdProjectDetails").data("kendoGrid") != undefined) {
        if ($("#grdProjectDetails").data("kendoGrid").dataSource._filter != undefined) {
            var filters = $("#grdProjectDetails").data("kendoGrid").dataSource._filter;
        }
    }
    $("#grdProjectDetails").kendoGrid({
        height: 500,
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectID: {
                            type: "number",
                            editable: false
                        },
                        Name: {
                            type: "string"
                        },
                        PM: {
                            type: "string"
                        },
                        BA: {
                            type: "string"
                        },
                        AccManager: {
                            type: "string"
                        },
                        Duration: {
                            type: "string"
                        },
                        BudgetedHour: {
                            type: "number"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        UnApprovedHours: {
                            type: "number"
                        },
                        strUnApprovedHours: {
                            type: "string"
                        },
                        status: {
                            type: "string"
                        },
                        BudgetedCost: {
                            type: "number"
                        },
                        ActualCost: {
                            type: "number"
                        },
                        ActualPayment: {
                            type: "number"
                        },
                        PaymentRatio: {
                            type: "number"
                        },
                        ProjectHealth_Effort: {
                            type: "number"
                        },
                        Reportdate: {
                            type: "date"
                        },
                        Status: {
                            type: "string"
                        },
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
        //columnMenu: true,
        selectable: 'row',  //selects a row on click
        columns: [{
            field: "projectID",
            title: "ProjectID",
            width: "10px",
            hidden: true
        }, {
            field: "Name",
            title: "Project",
            width: "120px",
        }, {
            field: "PM",
            title: "PM",
            width: "80px",
        }, {
            field: "BA",
            title: "BA",
            width: "80px",
        }, {
            field: "AccManager",
            title: "A/C Manager",
            width: "80px",
        }, {

            field: "Duration",
            title: "Duration",
            width: "100px"
        }, {
            field: "BudgetedHour",
            title: "Budgeted Hours",
            width: "80px",
            //template: '<div style="text-align:right;">#= BudgetedHour#</div>'
            template: '<div class="ra">#= kendo.toString(BudgetedHour,"n0") #</div>' //added css for this in aspx page
        }, {
            field: "ActualHour",
            title: "Actual Hours",
            width: "80px",
            //template: "<div class='ra'>#= ActualHour #</div>"
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'

        },
        {
            field: "UnApprovedHours",
            title: "Unapproved Hours",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
        },
        {
            field: "strUnApprovedHours",
            title: "strUnApproved Hours",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(strUnApprovedHours,"n0") #</div>'
        },
        {
            field: "BudgetedCost",
            title: "Budgeted Cost",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(BudgetedCost,"n0") #</div>'
        },
        {
            field: "ActualCost",
            title: "Actual Cost",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(ActualCost,"n0") #</div>'

        },
        {
            field: "ActualPayment",
            title: "Payment Received",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(ActualPayment,"n0") #</div>'
        },
        {
            field: "Status",
            title: "Project Status",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Status,"n0") #</div>'
        },
        {
            field: "PaymentRatio",
            title: "Payment Ratio",
            width: "80px;",
            display: "inline-block;",
            template: '#if(ActualCost > 0 && ActualPayment>0 && ActualPayment>ActualCost && BudgetedCost>0 && (((ActualPayment/BudgetedCost)*100)<=50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;" title="Payment Received">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:#=kendo.parseInt(((ActualCost/ActualPayment)*40),"n0")#px;" title="Actual Cost">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualPayment>ActualCost && BudgetedCost > 0 && (((ActualPayment/BudgetedCost)*100)>50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;" title="Payment Received" class="line-in-middle">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:#=kendo.parseInt(((ActualCost/ActualPayment)*40),"n0")#px;" title="Actual Cost">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost>ActualPayment && BudgetedCost > 0 && (((ActualPayment/BudgetedCost)*100)<=50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:#=kendo.parseInt(((ActualPayment/ActualCost)*40),"n0")#px;margin: 4px 0 0;display:inline-block;" title="Payment Received">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:red;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost>ActualPayment && BudgetedCost > 0  && (((ActualPayment/BudgetedCost)*100)>50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:#=kendo.parseInt(((ActualPayment/ActualCost)*40),"n0")#px;margin: 4px 0 0;display:inline-block;" title="Payment Received" class="line-in-middle">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:red;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost==ActualPayment && BudgetedCost > 0  && (((ActualPayment/BudgetedCost)*100)<=50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Payment Received">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost==ActualPayment && BudgetedCost > 0 && (((ActualPayment/BudgetedCost)*100)>50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div><div><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Payment Received" class="line-in-middle">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div></div>#}#'
        },
        {
            field: "ProjectHealth_Effort",
            title: "Project Health",
            width: "80px",
            hidden: true,
            //template: '#:ProjectHealth_disp(ProjectHealth_Effort)# <div class="ra">#= kendo.toString(ProjectHealth_Effort,"n0") #%</div>'
            template: '#if(ProjectHealth_Effort >= "0" && ProjectHealth_Effort  <="70" ) {#<div class="btnColor blue"></div>#} ' +
                'else if (ProjectHealth_Effort > "70" && ProjectHealth_Effort  <="100" ) {#<div class="btnColor green"></div>#}' +
                'else if (ProjectHealth_Effort > "100" && ProjectHealth_Effort  <="130" ) {#<div class="btnColor yellow"></div>#}' +
                'else if (ProjectHealth_Effort > "130" && ProjectHealth_Effort  <="150" ) {#<div class="btnColor orange"></div>#} ' +
                'else {#<div class="btnColor red"></div>#}' +
                '#<div class="ra">#= kendo.toString(ProjectHealth_Effort,"n0") #%</div>'

        },
        {
            field: "Reportdate",
            title: "Report Date",
            width: "100px",
            hidden: true,
            template: "#= kendo.toString(kendo.parseDate(Reportdate, 'yyyy-MM-dd'), 'dd-MMM-yyyy') #"
            },
        {
            field: "DevelopmentTeam",
            title: "Allocated Development Team",
            width: "100px",
            hidden: true,
            template: '<div class="ra">#= kendo.toString(DevelopmentTeam,"n0") #</div>'
        }
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
        //editable: "popup",
        //editable: true,
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        change: function (arg) {
            var window = $("#details");
            openPopUP();

            var gview = $("#grdProjectDetails").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());
            //alert(selectedItem.ID);
            $('[id$="hdnProjectId"]').val(selectedItem.ID);
            //$('[id$="lblProjectId"]').html(selectedItem.ID + "," + selectedItem.Name);
            //alert($('[id$="hdnProjectId"]').val());

            var projectName = selectedItem.Name;
            $('[id$="lblProjectName"]').html(projectName);

            var PM = selectedItem.PM;
            $('[id$="lblPM"]').html(PM);
            var BA = selectedItem.BA;
            $('[id$="lblBA"]').html(BA);
            var AccManager = selectedItem.AccManager;
            $('[id$="lblAccManager"]').html(AccManager);
            var Duration = selectedItem.Duration;
            $('[id$="lblDuration"]').html(Duration);

            var BudgetedHour = selectedItem.BudgetedHour;
            $('[id$="lblBudgetedHrs"]').html(BudgetedHour);

            var status = selectedItem.Status;
            $('[id$="lblStatus"]').html(status);

            var BudgetedCost = selectedItem.BudgetedCost;
            $('[id$="lblBudgetedCost"]').html(kendo.toString(BudgetedCost, "n0"));

            var UnApprovedHours = selectedItem.UnApprovedHours;
            $('[id$="lblUnApprovedHours"]').html(kendo.toString(UnApprovedHours, "n0"));

            var strUnApprovedHours = selectedItem.strUnApprovedHours;
            $('[id$="lblstrUnApprovedHours"]').html(kendo.toString(strUnApprovedHours, "n0"));

            var ActualCost = selectedItem.ActualCost;
            $('[id$="lblActualCost"]').html(kendo.toString(ActualCost / 1.5, "n0"));

            var PaymentRec = selectedItem.PaymentRec;
            $('[id$="lblPaymentRec"]').html(kendo.toString(PaymentRec, "n0"));

            //var PaymentRec = selectedItem.ActualPayment;
            //$('[id$="lblPaymentRec"]').html(kendo.toString(ActualPayment, "n0"));

            var Status = selectedItem.Status;
            $('[id$="lblStatusCompletion"]').html(kendo.toString(Status, "n0"));

            var rptdate = selectedItem.Reportdate;
            $('[id$="lblRptdate"]').html(convert(rptdate));

            var DevelopmentTeam = selectedItem.DevelopmentTeam;
            $('[id$="lblDevelopmentTeam"]').html(DevelopmentTeam);


            if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(PaymentRec, "n0") > kendo.parseInt(ActualCost, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") <= 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:block;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    //var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost1"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH1"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                    //$("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;

                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }

                }
                else {

                    //var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost1"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH1"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost1']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;

                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }
                }

            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(PaymentRec, "n0") > kendo.parseInt(ActualCost, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") > 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:block;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    //var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost2"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH2"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost2']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));

                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;
                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }


                }
                else {

                    //var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost2"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH2"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost2']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;
                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }


                }


            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") > kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") <= 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:block;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    //var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //$('[id$="paymentrecd3"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //$('[id$="actualcostOH3"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost3']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;


                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }


                }
                else {
                    //var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    //$('[id$="paymentrecd3"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //$('[id$="actualcostOH3"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost3']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;


                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }

                }



            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") > kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") > 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:block;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    //var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //if (valueOH >= 50) {
                    //    valueOH = valueOH + 2;
                    //}
                    //if (kendo.parseInt((ActualCost / 1.5), "n0") < kendo.parseInt(PaymentRec, "n0")) {
                    //    $('[id$="paymentrecd4"]').attr("style", "width:" + 50 + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcost4"]').attr("style", "width:" + value.toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcostOH4"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //    $("label[for='lblActualCost4']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //    $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    //}
                    //else if (kendo.parseInt((ActualCost / 1.5), "n0") > kendo.parseInt(PaymentRec, "n0")) {
                    //    $('[id$="actualcost4"]').attr("style", "width:" + 50 + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="paymentrecd4"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcostOH4"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //    $("label[for='lblActualCost4']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //    $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    //}
                    var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;
                    if (valueOH >= 50) {
                        valueOH = valueOH + 2;
                    }
                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH4"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd4"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    }


                }
                else {
                    //var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    //if (valueOH >= 50) {
                    //    valueOH = valueOH + 2;
                    //}
                    //if (kendo.parseInt((ActualCost * 1.5), "n0") > kendo.parseInt((PaymentRec), "n0")) {
                    //    $('[id$="paymentrecd4"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcost4"]').attr("style", "width:" + 50 + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcostOH4"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //    $("label[for='lblActualCost4']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //    $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    //}
                    //else {
                    //    $('[id$="paymentrecd4"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcost4"]').attr("style", "width:" + 50 + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcostOH4"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //    $("label[for='lblActualCost4']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //    $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    //}
                    var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;
                    if (valueOH >= 50) {
                        valueOH = valueOH + 2;
                    }
                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH4"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd4"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));

                    }

                }

            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") == kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") <= 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:block;");
                $('[id$="Block6"]').attr("style", "display:none;");
            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") == kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") > 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:block;");
            }
            else {
                $('[id$="showdiv"]').attr("style", "display:none;");
            }
            GetTSBreakup($('[id$="hdnProjectId"]').val());
            GetTimesheetDetails($('[id$="hdnProjectId"]').val());
            showCursorPointerForTSDetails();
        },

    });
    if ($("#grdProjectDetails").data("kendoGrid") != undefined) {
        $("#grdProjectDetails").data("kendoGrid").refresh();
        if (filters != undefined) {
            $("#grdProjectDetails").data("kendoGrid").dataSource.filter(filters);
        }

    }
    //HideLoading();
}

//function ProjectHealth_disp(ProjectHealth) {
//if (ProjectHealth >= "0" && ProjectHealth <= "70")
//{
//    '#<div class="btnColor blue"></div>#'
//}
//else if (ProjectHealth > "70" && ProjectHealth <= "100")
//{
//    '#<div class="btnColor green"></div>#'
//}
//else if (ProjectHealth > "100" && ProjectHealth <= "130")
//{
//    '#<div class="btnColor yellow"></div>#'
//}
//else if (ProjectHealth > "130" && ProjectHealth <= "150")
//{
//    '#<div class="btnColor orange"></div>#'
//}
//else { '#<div class="btnColor red"></div>#' }

//}



//////////////////////////////     TimeSheet Breakup Section (2nd Popup) ///////////////////////////


$(function () {

    $("#chkTNM").click(function () {
        var showOverHeads;
        var Added;
        var Initiated;
        var InProgress;
        var UnderUAT;
        var OnHold;
        var CompletedClosed;
        var Cancelled;
        var UnderWarranty;
        var TNM;
        var FixedCost;        

        if ($(this).is(":checked")) {
            TNM = true;
            $('#chkTNM').attr('checked', 'checked');

        } else {
            TNM = false;
            $('#chkTNM').removeAttr('checked');
        }

         //---- set both selectio T&M & fixedCost start --------

        FixedCost = $('#chkFixedCost').attr('checked') ? true : false;

        if (TNM == false && FixedCost == false) {
            alert("Please select at least one project type.");
            $('#chkTNM').prop('checked', true);
            $('#chkTNM').attr('checked', 'checked');
            TNM = true;
        }
        //---- set both selectio T&M & fixedCost end  ---------

        ////---- set both selectio T&M & fixedCost start --------
        
        //FixedCost = $('#chkFixedCost').attr('checked') ? true : false;

        //if (TNM == true && FixedCost == true) {
        //    $('#chkFixedCost').removeAttr('checked');
        //    FixedCost = false;
        //}

        //if (TNM == false && FixedCost == false) {
        //    alert("please select project type T&M or Fixed Cost ");
        //    $('#checkbox0').removeAttr('checked');
        //    $('#checkbox1').removeAttr('checked');
        //    $('#checkbox2').removeAttr('checked');
        //    $('#checkbox3').removeAttr('checked');
        //    $('#checkbox4').removeAttr('checked');
        //    $('#checkbox7').removeAttr('checked');
        //    //$('#chkTNM').prop('checked', true);
        //    //TNM = true;
        //}
        ////---- set both selectio T&M & fixedCost end  ---------

        Added = $('#checkbox0').attr('checked') ? true : false;
        Initiated = $('#checkbox1').attr('checked') ? true : false;
        InProgress = $('#checkbox2').attr('checked') ? true : false;
        UnderUAT = $('#checkbox3').attr('checked') ? true : false;
        OnHold = $('#checkbox4').attr('checked') ? true : false;
        UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
        FixedCost = $('#chkFixedCost').attr('checked') ? true : false;

        CompletedClosed = false;
        Cancelled = false;
        var isCheckedOverhead = $('#chkIsOverHeads').attr('checked') ? true : false;

        if (isCheckedOverhead == true) {
            showOverHeads = true;
            GetProjectDetails_Manage(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }
        else {
            showOverHeads = false;
            GetProjectDetails(false, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }


    });

});


$(function () {

    $("#chkFixedCost").click(function () {
        var showOverHeads;
        var Added;
        var Initiated;
        var InProgress;
        var UnderUAT;
        var OnHold;
        var CompletedClosed;
        var Cancelled;
        var UnderWarranty;
        var TNM;
        var FixedCost;

        if ($(this).is(":checked")) {
            FixedCost = true;
            $('#chkFixedCost').attr('checked', 'checked');

        } else {
            FixedCost = false;
            $('#chkFixedCost').removeAttr('checked');
        }

        //---- set both selectio T&M & fixedCost start --------

        TNM = $('#chkTNM').attr('checked') ? true : false;

        if (TNM == false && FixedCost == false) {
            alert("Please select at least one project type.");
            $('#chkFixedCost').prop('checked', true);
            $('#chkFixedCost').attr('checked', 'checked');
            FixedCost = true;            
        }
        //---- set both selectio T&M & fixedCost end  ---------

        ////---- set both selectio T&M & fixedCost start --------

        //TNM = $('#chkTNM').attr('checked') ? true : false;

        //if (TNM == true && FixedCost == true) {
        //    $('#chkTNM').removeAttr('checked');
        //    TNM = false;
        //}

        //if (TNM == false && FixedCost == false) {
        //    alert("please select project type T&M or Fixed Cost.");
        //    $('#checkbox0').removeAttr('checked');
        //    $('#checkbox1').removeAttr('checked');
        //    $('#checkbox2').removeAttr('checked');
        //    $('#checkbox3').removeAttr('checked');
        //    $('#checkbox4').removeAttr('checked');
        //    $('#checkbox7').removeAttr('checked');
        //    //$('#chkTNM').prop('checked', true);
        //    //TNM = true;
        //}
        ////---- set both selectio T&M & fixedCost end  ---------

        Added = $('#checkbox0').attr('checked') ? true : false;
        Initiated = $('#checkbox1').attr('checked') ? true : false;
        InProgress = $('#checkbox2').attr('checked') ? true : false;
        UnderUAT = $('#checkbox3').attr('checked') ? true : false;
        OnHold = $('#checkbox4').attr('checked') ? true : false;
        UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
        TNM = $('#chkTNM').attr('checked') ? true : false;        

        CompletedClosed = false;
        Cancelled = false;
        var isCheckedOverhead = $('#chkIsOverHeads').attr('checked') ? true : false;

        if (isCheckedOverhead == true) {
            showOverHeads = true;
            GetProjectDetails_Manage(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }
        else {
            showOverHeads = false;
            GetProjectDetails(false, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }

    });

});


function checkVal(Id) {    
    document.getElementById(Id.id).onclick = function () {

        var showOverHeads;
        var Added;
        var Initiated;
        var InProgress;
        var UnderUAT;
        var OnHold;
        var CompletedClosed;
        var Cancelled;
        var UnderWarranty;
        var TNM;
        var FixedCost;
        var IdChk = Id.id;

        ////---- set both selectio T&M & fixedCost start --------

        //TNM = $('#chkTNM').attr('checked') ? true : false;
        //FixedCost = $('#chkFixedCost').attr('checked') ? true : false;

        //if (TNM == true && FixedCost == true) {
        //    $('#chkFixedCost').removeAttr('checked');
        //    FixedCost = false;
        //}
        //if (TNM == false && FixedCost == false) {
        //    alert("please select project type T&M or Fixed Cost ");
        //    $('#checkbox0').removeAttr('checked');
        //    $('#checkbox1').removeAttr('checked');
        //    $('#checkbox2').removeAttr('checked');
        //    $('#checkbox3').removeAttr('checked');
        //    $('#checkbox4').removeAttr('checked');
        //    $('#checkbox7').removeAttr('checked');
        //    //$('#chkTNM').prop('checked', true);
        //    //TNM = true;
        //}
        ////---- set both selectio T&M & fixedCost end  ---------        


        if ('#' + Id.id == '#checkbox0') {
            chkIsAdded = $('#checkbox0').attr('checked') ? true : false;
            if (chkIsAdded == true) {
                $('#checkbox0').removeAttr('checked');
                Added = false;
            }
            else {
                $('#checkbox0').attr('checked', 'checked');
                Added = true;
            }
            Initiated = $('#checkbox1').attr('checked') ? true : false;
            InProgress = $('#checkbox2').attr('checked') ? true : false;
            UnderUAT = $('#checkbox3').attr('checked') ? true : false;
            OnHold = $('#checkbox4').attr('checked') ? true : false;
            UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
            TNM = $('#chkTNM').attr('checked') ? true : false;
            FixedCost = $('#chkFixedCost').attr('checked') ? true : false;
        }
        else if ('#' + Id.id == '#checkbox1') {
            chkIsStarted = $('#checkbox1').attr('checked') ? true : false;
            if (chkIsStarted == true) {
                $('#checkbox1').removeAttr('checked');
                Initiated = false;
            }
            else {
                $('#checkbox1').attr('checked', 'checked');
                Initiated = true;
            }
            Added = $('#checkbox0').attr('checked') ? true : false;
            InProgress = $('#checkbox2').attr('checked') ? true : false;
            UnderUAT = $('#checkbox3').attr('checked') ? true : false;
            OnHold = $('#checkbox4').attr('checked') ? true : false;
            UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
            TNM = $('#chkTNM').attr('checked') ? true : false;
            FixedCost = $('#chkFixedCost').attr('checked') ? true : false;
        }
        else if ('#' + Id.id == '#checkbox2') {
            chkIsInProgress = $('#checkbox2').attr('checked') ? true : false;
            if (chkIsInProgress == true) {
                $('#checkbox2').removeAttr('checked');
                InProgress = false;
            }
            else {
                $('#checkbox2').attr('checked', 'checked');
                InProgress = true;
            }
            Added = $('#checkbox0').attr('checked') ? true : false;
            Initiated = $('#checkbox1').attr('checked') ? true : false;
            UnderUAT = $('#checkbox3').attr('checked') ? true : false;
            OnHold = $('#checkbox4').attr('checked') ? true : false;
            UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
            TNM = $('#chkTNM').attr('checked') ? true : false;
            FixedCost = $('#chkFixedCost').attr('checked') ? true : false;
        }
        else if ('#' + Id.id == '#checkbox3') {
            chkIsToBeStarted = $('#checkbox3').attr('checked') ? true : false;
            if (chkIsToBeStarted == true) {
                $('#checkbox3').removeAttr('checked');
                UnderUAT = false;
            }
            else {
                $('#checkbox3').attr('checked', 'checked');
                UnderUAT = true;
            }
            Added = $('#checkbox0').attr('checked') ? true : false;
            Initiated = $('#checkbox1').attr('checked') ? true : false;
            InProgress = $('#checkbox2').attr('checked') ? true : false;
            OnHold = $('#checkbox4').attr('checked') ? true : false;
            UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
            TNM = $('#chkTNM').attr('checked') ? true : false;
            FixedCost = $('#chkFixedCost').attr('checked') ? true : false;

        }
        else if ('#' + Id.id == '#checkbox4') {
            chkHold = $('#checkbox4').attr('checked') ? true : false;
            if (chkHold == true) {
                $('#checkbox4').removeAttr('checked');
                OnHold = false;
            }
            else {
                $('#checkbox4').attr('checked', 'checked');
                OnHold = true;
            }
            Added = $('#checkbox0').attr('checked') ? true : false;
            Initiated = $('#checkbox1').attr('checked') ? true : false;
            InProgress = $('#checkbox2').attr('checked') ? true : false;
            UnderUAT = $('#checkbox3').attr('checked') ? true : false;
            UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
            TNM = $('#chkTNM').attr('checked') ? true : false;
            FixedCost = $('#chkFixedCost').attr('checked') ? true : false;

        }
        else if ('#' + Id.id == '#checkbox7') {
            chkIsUnderWarranty = $('#checkbox7').attr('checked') ? true : false;
            if (chkIsUnderWarranty == true) {
                $('#checkbox7').removeAttr('checked');
                UnderWarranty = false;
            }
            else {
                $('#checkbox7').attr('checked', 'checked');
                UnderWarranty = true;
            }
            Added = $('#checkbox0').attr('checked') ? true : false;
            Initiated = $('#checkbox1').attr('checked') ? true : false;
            InProgress = $('#checkbox2').attr('checked') ? true : false;
            UnderUAT = $('#checkbox3').attr('checked') ? true : false;
            OnHold = $('#checkbox4').attr('checked') ? true : false;
            TNM = $('#chkTNM').attr('checked') ? true : false;
            FixedCost = $('#chkFixedCost').attr('checked') ? true : false;

            //alert("Added : " + Added + "\n" + "Initiated : " + Initiated + "\n" + "InProgress : " + InProgress + "\n" + "UnderUAT : " + UnderUAT + "\n" + "OnHold : " + OnHold + "\n" + "TNM : " + TNM + "\n" + "FixedCost : " + FixedCost + "\n");
        } 

        else if ('#' + Id.id == '#chkTNM') {
            TNM = $('#chkTNM').attr('checked') ? true : false;
            if (TNM == true) {
                $('#chkTNM').removeAttr('checked');
                TNM = false;
            }
            else {
                $('#chkTNM').attr('checked', 'checked');
                TNM = true;
            }
            Added = $('#checkbox0').attr('checked') ? true : false;
            Initiated = $('#checkbox1').attr('checked') ? true : false;
            InProgress = $('#checkbox2').attr('checked') ? true : false;
            UnderUAT = $('#checkbox3').attr('checked') ? true : false;
            OnHold = $('#checkbox4').attr('checked') ? true : false;
            UnderWarranty = $('#checkbox7').attr('checked') ? true : false;

            //alert(TNM);
        }


        CompletedClosed = false;
        Cancelled = false;
        var isCheckedOverhead = $('#chkIsOverHeads').attr('checked') ? true : false;
        if (isCheckedOverhead == true) {
            showOverHeads = true;
            GetProjectDetails_Manage(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);            
        }
        else {
            showOverHeads = false;
            GetProjectDetails(false, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }       
    }
}
function GetTSBreakup(prjId) {
    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTSBreakupDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTSBreakupData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetTSBreakupData(Tdata) {
    if ($('#grdTSBreakup').data().kendoGrid != undefined) {
        $('#grdTSBreakup').data().kendoGrid.destroy();
        $('#grdTSBreakup').empty();
    }
    $("#grdTSBreakup").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Module: {
                            type: "string"
                        },
                        Percentage_Effort: {
                            type: "string"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        //UnApprovedHours: {
                        //    type: "number"
                        //},
                        Cost: {
                            type: "number"
                        },
                    }
                }
            },
            //pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [{
            field: "Module",
            title: "Work",
            width: "80px",
        },
        {
            field: "Percentage_Effort",
            title: "Percentage Effort %",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(Percentage_Effort,"n0") #</div>'
        },

        {
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'
        },
            //    {
            //    field: "UnApprovedHours",
            //    title: "Unapproved Hours",
            //    width: "50px",
            //    template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
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
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });

}


//////////////////////////////     TimeSheet Details Section (2nd Popup) ///////////////////////////

function GetTimesheetDetails(prjId) {

    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTimesheetDetailsMonthwise",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTimesheetData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetTimesheetData(Tdata) {
    if ($('#grdTSDetails').data().kendoGrid != undefined) {
        $('#grdTSDetails').data().kendoGrid.destroy();
        $('#grdTSDetails').empty();
    }
    $("#grdTSDetails").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Module: {
                            type: "string"
                        },
                        TSYear: {
                            type: "string"
                        },
                        TSMonth: {
                            type: "string"
                        },
                        Percentage_Effort: {
                            type: "string"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        UnApprovedHours: {
                            type: "number"
                        },
                        Cost: {
                            type: "number"
                        },
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        selectable: 'row',  //selects a row on click
        columns: [{
            field: "Module",
            title: "Month",
            width: "80px",
        },
        {
            field: "TSYear",
            title: "TSYear",
            width: "20px",
            hidden: true,
        },
        {
            field: "TSMonth",
            title: "TSMonth",
            width: "20px",
            hidden: true,
        }, {
            field: "Percentage_Effort",
            title: "Percentage Effort %",
            width: "50px",
            //format: "{0:n0}",
            template: '<div class="ra">#= kendo.toString(Percentage_Effort,"n0") #</div>'
        },
        {
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            //format: "{0:n0}",
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'
        },
            //    {
            //    field: "UnApprovedHours",
            //    title: "Unapproved Hours",
            //    width: "50px",
            //    template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
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
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        change: function (arg) {

            var window = $("#TSdetails");
            openTSPopUP();

            var gview = $("#grdTSDetails").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());

            var gview1 = $("#grdProjectDetails").data("kendoGrid");
            var selectedItem1 = gview1.dataItem(gview1.select());

            $('[id$="hdnProjectId"]').val(selectedItem1.ID);
            $('[id$="hdnTSYear"]').val(selectedItem.TSYear);
            $('[id$="hdnTSMonth"]').val(selectedItem.TSMonth);
            //alert($('[id$="hdnProjectId"]').val() + " "+ $('[id$="hdnTSYear"]').val()+" "+ $('[id$="hdnTSMonth"]').val());

            GetTimesheetDetailsWorkwise($('[id$="hdnProjectId"]').val(), $('[id$="hdnTSYear"]').val(), $('[id$="hdnTSMonth"]').val());
            //showCursorPointerForMonthTSDetails()
        },
    });
}



//////////////////////////////     TimeSheet Details WorkWise (3rd Popup)///////////////////////////

function GetTimesheetDetailsWorkwise(prjId, year, month) {
    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTimesheetDetailsWorkwise",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "','year':'" + year + "','month':'" + month + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTimesheetWorkWiseData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetTimesheetWorkWiseData(Tdata) {
    $("#grdTSModuleDetails").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Module: {
                            type: "string"
                        },
                        Percentage_Effort: {
                            type: "string"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        //UnApprovedHours: {
                        //    type: "number"
                        //},
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        //selectable: 'row',  //selects a row on click
        columns: [{
            field: "Module",
            title: "Work",
            width: "80px",
            //template: '<a href="ManageTimesheet.aspx>#= Module #</a>' //PrjId=" + projId=#=projId#""
        },
        {
            field: "Percentage_Effort",
            title: "Percentage Effort %",
            width: "50px",
            //template: '<div class="ra">#= kendo.toString(Percentage_Effort,"n0") #</div>'
        },
        {
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'
        },
            //    {
            //    field: "UnApprovedHours",
            //    title: "Unapproved Hours",
            //    width: "50px",
            //    template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
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
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        ////added on 11/05/2015
        //change: function (e) {

        //    //var window = $("#grdTSModuleDetails");
        //    //openTSPopUP();

        //    var gview = $("#grdTSModuleDetails").data("kendoGrid");
        //    var selectedItem = gview.dataItem(gview.select());

        //    var gview1 = $("#grdProjectDetails").data("kendoGrid");
        //    var selectedItem1 = gview1.dataItem(gview1.select());


        //    //customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
        //    var projId = selectedItem1.ID;
        //    //var projId = selectedItem.Module;

        //    //alert(projId);
        //    window.location.assign("ManageTimesheet.aspx?PrjId=" + projId);
        //},
        ////end added on 11/05/2015
    });
}


///////////////////////////////////////////////////////////////////////////////////////////////////

function openPopUP() {
    $('#details').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}

function closePopUP() {
    $('#details').css('display', 'none');
    $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}


function openTSPopUP() {
    $('#TSdetails').css('display', '');
    $('#divOverlay').addClass('k-overlay');
}

function closeTSPopUP() {
    $('#TSdetails').css('display', 'none');
    $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}
//function redirectToTimesheet(e) {
//    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
//    var data = this.dataItem(customRowDataItem);
//    var projId = customRowDataItem.projId;
//    window.location.assign("Invoices.aspx?p=" + projId);
//}



//////////////////////////////     Project Details Manage (Show Cost related to data)  ///////////////////////////

function fnshowOverHeads(checked) {

    var Added;
    var Initiated;
    var InProgress;
    var UnderUAT;
    var OnHold;
    var CompletedClosed;
    var Cancelled;
    var UnderWarranty;
    var TNM;
    var FixedCost;
    Added = $('#checkbox0').attr('checked') ? true : false;
    Initiated = $('#checkbox1').attr('checked') ? true : false;
    InProgress = $('#checkbox2').attr('checked') ? true : false;
    UnderUAT = $('#checkbox3').attr('checked') ? true : false;
    OnHold = $('#checkbox4').attr('checked') ? true : false;
    UnderWarranty = $('#checkbox7').attr('checked') ? true : false;
    TNM = $('#chkTNM').attr('checked') ? true : false;
    FixedCost = $('#chkFixedCost').attr('checked') ? true : false;
    CompletedClosed = false;
    Cancelled = false;
    if (checked) {
        showOverHeads = true;
        $('#chkIsOverHeads').attr('checked', true);
        GetProjectDetails_Manage(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
    }
    else {
        showOverHeads = false;
        $('#chkIsOverHeads').removeAttr('checked');
        GetProjectDetails(false, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
    }


}
function ProjectStatusType() {
    $.ajax({
        type: "GET",
        url: "BITS.aspx/BindProjectStatusTypeDetails",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        contentType: "json",
        async: false,
        success: function (msg) {
            alert(msg);
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}
function GetProjectDetails_Manage(showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost) {    
       if ($('[id$="hdnAdmin"]').val() != 0)
        $('[id$="tdOverHead"]').hide();
    else
        $('[id$="tdOverHead"]').show();
    var PMID = $('[id$="hdnAdmin"]').val();
    $.ajax({
        type: "POST",
        url: "BITS.aspx/BindProjectDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'PMID':'" + PMID + "','showOverHeads':'" + showOverHeads + "','Added':'" + Added + "','Initiated':'" + Initiated + "','InProgress':'" + InProgress + "','UnderUAT':'" + UnderUAT + "','OnHold':'" + OnHold + "','CompletedClosed':'" + CompletedClosed + "','Cancelled':'" + Cancelled + "','UnderWarranty':'" + UnderWarranty + "','TNM':'" + TNM + "','FixedCost':'" + FixedCost + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {            
            GetProjectDataManage(jQuery.parseJSON(msg.d));            
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetProjectDataManage(Tdata) {
    ShowLoading();
    if ($('#grdProjectDetails').data().kendoGrid != undefined) {
        $('#grdProjectDetails').data().kendoGrid.destroy();
        $('#grdProjectDetails').empty();
    }
    if ($("#grdProjectDetails").data("kendoGrid") != undefined) {
        var filters = $("#grdProjectDetails").data("kendoGrid").dataSource._filter;
    }
    $("#grdProjectDetails").kendoGrid({
        height: 500,
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectID: {
                            type: "number",
                            editable: false
                        },
                        Name: {
                            type: "string"
                        },
                        PM: {
                            type: "string"
                        },
                        BA: {
                            type: "string"
                        },
                        AccManager: {
                            type: "string"
                        },
                        Duration: {
                            type: "string"
                        },
                        BudgetedHour: {
                            type: "number"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        status: {
                            type: "string"
                        },
                        //strUnApprovedHours: {
                        //    type: "string"
                        //},
                        BudgetedCost: {
                            type: "number"
                        },
                        ActualCost: {
                            type: "number"
                        },
                        ActualPayment: {
                            type: "number"
                        },
                        PaymentRatio: {
                            type: "number"
                        },
                        CostAgainstReceived: {
                            type: "number"
                        },
                        ReportDate: {
                            type: "string"
                        },
                        ProjectHealth_Cost: {
                            type: "number"
                        },
                        Reportdate: {
                            type: "date"
                        },
                        Status: {
                            type: "string"
                        }
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
        //columnMenu: true,
        selectable: 'row',  //selects a row on click
        columns: [{
            field: "projectID",
            title: "ProjectID",
            width: "10px",
            hidden: true
        }, {
            field: "Name",
            title: "Project (Type)",
            width: "120px",
        }, {
            field: "PM",
            title: "PM",
            width: "80px",
        }, {
            field: "BA",
            title: "BA",
            width: "80px",
        }, {
            field: "AccManager",
            title: "A/C Manager",
            width: "80px",
        }, {
            field: "Duration",
            title: "Duration",
            width: "100px"
        }, {
            field: "BudgetedHour",
            title: "Budgeted Hours",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(BudgetedHour,"n0") #</div>' //added css for this in aspx page
        }, {
            field: "ActualHour",
            title: "Actual Hours",
            width: "80px",
            //template: "<div class='ra'>#= ActualHour #</div>"
            template: '<div class="ra">#= kendo.parseInt(ActualHour,"n0") #</div>'

        },// {
        //    field: "UnApprovedHours",
        //    title: "Unapproved Hours",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
        //},
        //{
        //    field: "strUnApprovedHours",
        //    title: "strUnApproved Hours",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(strUnApprovedHours,"n0") #</div>'
        //},
        {
            field: "BudgetedCost",
            title: "Budgeted Cost",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(BudgetedCost,"n0") #</div>'
        }, {
            field: "ActualCost",
            title: "Actual Cost",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(ActualCost,"n0") #</div>'

        }, {
            field: "ActualPayment",
            title: "Payment Received",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(ActualPayment,"n0") #</div>'
        },
        {
            field: "Status",
            title: "Project Status",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Status,"n0") #</div>'
        },


        {
            field: "PaymentRatio",
            title: "Payment Ratio",
            width: "80px;",
            display: "inline-block;",
            template: '#if(ActualCost > 0 && ActualPayment>0 && ActualPayment>ActualCost && BudgetedCost>0 && (((ActualPayment/BudgetedCost)*100)<=50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;" title="Payment Received">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:#=kendo.parseInt(((ActualCost/ActualPayment)*40),"n0")#px;" title="Actual Cost">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualPayment>ActualCost && BudgetedCost > 0 && (((ActualPayment/BudgetedCost)*100)>50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;" title="Payment Received" class="line-in-middle">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:#=kendo.parseInt(((ActualCost/ActualPayment)*40),"n0")#px;" title="Actual Cost">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost>ActualPayment && BudgetedCost > 0 && (((ActualPayment/BudgetedCost)*100)<=50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:#=kendo.parseInt(((ActualPayment/ActualCost)*40),"n0")#px;margin: 4px 0 0;display:inline-block;" title="Payment Received">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:red;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost>ActualPayment && BudgetedCost > 0  && (((ActualPayment/BudgetedCost)*100)>50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:#=kendo.parseInt(((ActualPayment/ActualCost)*40),"n0")#px;margin: 4px 0 0;display:inline-block;" title="Payment Received" class="line-in-middle">&nbsp;</div><span style="display:inline-block;width:40px;">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:red;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost==ActualPayment && BudgetedCost > 0  && (((ActualPayment/BudgetedCost)*100)<=50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Payment Received">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div><div><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div></div>#}' +
                'else if(ActualCost > 0 && ActualPayment>0 && ActualCost==ActualPayment && BudgetedCost > 0 && (((ActualPayment/BudgetedCost)*100)>50)){#<div style="width:80px;"><div style="margin-bottom:5px;"><div id="actualcost" style="background-color:green;color:white;display:inline-block;width:40px;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Actual Cost">&nbsp;&nbsp;#=kendo.toString(ActualCost, "n0")#</span></div><div><div id="paymentrecd" style="background-color:Orange;color:red;width:40px;margin: 4px 0 0;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:40px;" title="Payment Received" class="line-in-middle">&nbsp;&nbsp;#=kendo.toString(ActualPayment,"n0")#</span></div></div>#}#'
        },
        {
            field: "ProjectHealth_Cost",
            title: "Project Health",
            hidden: true,
            width: "80px",
            //template: '#:ProjectHealth_disp(ProjectHealth_Effort)# <div class="ra">#= kendo.toString(ProjectHealth_Effort,"n0") #%</div>'
            template: '#if(ProjectHealth_Cost >= "0" && ProjectHealth_Cost  <="70" ) {#<div class="btnColor blue"></div>#} ' +////#123#
                'else if (ProjectHealth_Cost > "70" && ProjectHealth_Cost  <="100" ) {#<div class="btnColor green"></div>#}' +
                //'else if (ProjectHealth_Cost > "70" && ProjectHealth_Cost  <="100" ){#<button class="btnColor green" ></button>#}' +
                'else if (ProjectHealth_Cost > "100" && ProjectHealth_Cost  <="130" ) {#<div class="btnColor yellow"></div>#}' +
                'else if (ProjectHealth_Cost > "130" && ProjectHealth_Cost  <="150" ) {#<div class="btnColor orange"></div>#} ' +
                'else {#<div class="btnColor red"></div>#}' +
                //'#<div class="ra">#= kendo.toString(ProjectHealth_Cost,"n0") #%</div>' 
                'if(ProjectHealth_Cost == "0" ){#     NA #} else {#<div class="ra"> #=kendo.toString(ProjectHealth_Cost,"n0") #%</div>#}#'
        },
        //{
        //    field: "Status",
        //    title: "Status",
        //    width: "100px"
        //},
        {
            field: "Reportdate",
            title: "Report Date",
            width: "100px",
            hidden: true,
            template: "#= kendo.toString(kendo.parseDate(ReportDate, 'yyyy-MM-dd'), 'dd-MMM-yyyy') #"
            }
            ,
            {
                field: "DevelopmentTeam",
                title: "Allocated Development Team",
                width: "100px",
                hidden: true,
                template: '<div class="ra">#= kendo.toString(DevelopmentTeam,"n0") #</div>'
            }
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
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        change: function (arg) {

            var window = $("#details");
            openPopUP();

            var gview = $("#grdProjectDetails").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());
            $('[id$="hdnProjectId"]').val(selectedItem.ID);

            var projectName = selectedItem.Name;
            $('[id$="lblProjectName"]').html(projectName);

            var PM = selectedItem.PM;
            $('[id$="lblPM"]').html(PM);

            var BA = selectedItem.BA;
            $('[id$="lblBA"]').html(BA);

            var AccManager = selectedItem.AccManager;
            $('[id$="lblAccManager"]').html(AccManager);

            var Duration = selectedItem.Duration;
            $('[id$="lblDuration"]').html(Duration);

            var BudgetedHour = selectedItem.BudgetedHour;
            $('[id$="lblBudgetedHrs"]').html(BudgetedHour);

            var strUnApprovedHours = selectedItem.strUnApprovedHours;
            $('[id$="lblstrUnApprovedHours"]').html(strUnApprovedHours);

            var status = selectedItem.Status;
            $('[id$="lblStatus"]').html(status);

            var BudgetedCost = selectedItem.BudgetedCost;
            $('[id$="lblBudgetedCost"]').html(kendo.toString(BudgetedCost, "n0"));

            var ActualCost = selectedItem.ActualCost;
            $('[id$="lblActualCost"]').html(kendo.toString(ActualCost / 1.5, "n0"));

            var PaymentRec = selectedItem.ActualPayment;
            $('[id$="lblPaymentRec"]').html(kendo.toString(PaymentRec, "n0"));

            var Status = selectedItem.Status;
            $('[id$="lblStatusCompletion"]').html(kendo.toString(Status, "n0"));

            var ProjReportdate = selectedItem.Reportdate;
            $('[id$="lblRptdate"]').html(convert(ProjReportdate));

            var DevelopmentTeam = selectedItem.DevelopmentTeam;
            $('[id$="lblDevelopmentTeam"]').html(DevelopmentTeam);

            if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(PaymentRec, "n0") > kendo.parseInt(ActualCost, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") <= 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:block;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    //var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost1"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH1"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                    //$("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;

                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }

                }
                else {

                    //var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost1"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH1"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost1']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;

                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd1"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH1"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost1']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH1']").text(kendo.toString((ActualCost), "n0"));
                    }


                }

            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(PaymentRec, "n0") > kendo.parseInt(ActualCost, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") > 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:block;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    //var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost2"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH2"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost2']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));

                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;


                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }


                }
                else {

                    //var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    //$('[id$="actualcost2"]').attr("style", "width:" + value.toString() + "px;background-color:green;display:inline-block;");
                    //$('[id$="actualcostOH2"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost2']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;


                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd2"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH2"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost2']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH2']").text(kendo.toString((ActualCost), "n0"));
                    }

                }


            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") > kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") <= 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:block;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    //var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //$('[id$="paymentrecd3"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //$('[id$="actualcostOH3"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost3']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;


                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }


                }
                else {
                    //var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    //var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    //$('[id$="paymentrecd3"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //$('[id$="actualcostOH3"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //$("label[for='lblActualCost3']").text(kendo.toString((ActualCost/1.5), "n0"));
                    //$("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;


                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd3"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH3"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost3']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH3']").text(kendo.toString((ActualCost), "n0"));
                    }

                }



            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") > kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") > 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:block;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:none;");
                var isChecked = $('#chkIsOverHeads').attr('checked') ? true : false;
                if (isChecked == true) {
                    var value = kendo.parseInt((((ActualCost / 1.5) / PaymentRec) * 50), "n0");
                    var valueOH = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;
                    if (valueOH >= 50) {
                        valueOH = valueOH + 2;
                    }
                    //if (kendo.parseInt((ActualCost / 1.5), "n0") < kendo.parseInt(PaymentRec, "n0")) {
                    //    $('[id$="paymentrecd4"]').attr("style", "width:" + 50 + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcost4"]').attr("style", "width:" + value.toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcostOH4"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //    $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                    //    $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    //}
                    //else if (kendo.parseInt((ActualCost / 1.5), "n0") > kendo.parseInt(PaymentRec, "n0")) {
                    //    $('[id$="actualcost4"]').attr("style", "width:" + 50 + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="paymentrecd4"]').attr("style", "width:" + value.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                    //    $('[id$="actualcostOH4"]').attr("style", "width:" + valueOH.toString() + "px;background-color:blue;display:inline-block;");
                    //    $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                    //    $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    //}

                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {

                        $('[id$="actualcostOH4"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:red;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd4"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / PaymentRec) * 100;
                        $('[id$="actualcost4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (ActualCost / PaymentRec) * 100;
                        $('[id$="actualcostOH4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:blue;display:inline-block;");

                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    }



                }


                else {
                    var value = kendo.parseInt((((ActualCost) / PaymentRec) * 50), "n0");
                    var valueOH = kendo.parseInt((((ActualCost * 1.5) / PaymentRec) * 50), "n0");
                    var width = 0;
                    var _actualCost = ActualCost / 1.5;
                    var maxWidth = 200;
                    if (valueOH >= 50) {
                        valueOH = valueOH + 2;
                    }

                    if (kendo.parseInt((ActualCost), "n0") > kendo.parseInt((PaymentRec), "n0")) {



                        $('[id$="actualcostOH4"]').attr("style", "width:" + maxWidth.toString() + "px;background-color:blue;display:inline-block;");
                        width = (PaymentRec / ActualCost) * 100;
                        $('[id$="paymentrecd4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        width = (_actualCost / ActualCost) * 100;
                        $('[id$="actualcost4"]').attr("style", "width:" + ((width / 100) * maxWidth).toString() + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    }
                    else {
                        $('[id$="paymentrecd4"]').attr("style", "width:" + valueOH.toString() + "px;background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;");
                        $('[id$="actualcost4"]').attr("style", "width:" + 50 + "px;background-color:Green;color:red;margin: 4px 0 0;display:inline-block;");
                        $('[id$="actualcostOH4"]').attr("style", "width:" + value.toString() + "px;background-color:blue;display:inline-block;");
                        $("label[for='lblActualCost4']").text(kendo.toString((ActualCost / 1.5), "n0"));
                        $("label[for='lblActualCostOH4']").text(kendo.toString((ActualCost), "n0"));
                    }


                }



            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") == kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") <= 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:block;");
                $('[id$="Block6"]').attr("style", "display:none;");
            }
            else if ((kendo.parseInt(ActualCost, "n0") > 0 && kendo.parseInt(ActualCost, "n0") != 0) && (kendo.parseInt(PaymentRec, "n0") > 0 && kendo.parseInt(PaymentRec, "n0") != 0) && (kendo.parseInt(BudgetedCost, "n0") > 0 && kendo.parseInt(BudgetedCost, "n0") != 0) && kendo.parseInt(ActualCost, "n0") == kendo.parseInt(PaymentRec, "n0") && kendo.parseInt(((PaymentRec / BudgetedCost) * 100), "n0") > 50) {
                $('[id$="showdiv"]').attr("style", "display:block;float:right;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block1"]').attr("style", "display:none;");
                $('[id$="Block2"]').attr("style", "display:none;");
                $('[id$="Block3"]').attr("style", "display:none;");
                $('[id$="Block4"]').attr("style", "display:none;");
                $('[id$="Block5"]').attr("style", "display:none;");
                $('[id$="Block6"]').attr("style", "display:block;");
            }
            else {
                $('[id$="showdiv"]').attr("style", "display:none;");
            }
            $('[id$="lblPaymentRec"]').html(kendo.toString(PaymentRec, "n0"));
            GetTSBreakupManage($('[id$="hdnProjectId"]').val());
            GetTimesheetDetailsManage($('[id$="hdnProjectId"]').val());
            showCursorPointerForTSDetails();
        },

    });
    if ($("#grdProjectDetails").data("kendoGrid") != undefined) {
        $("#grdProjectDetails").data("kendoGrid").refresh();
        $("#grdProjectDetails").data("kendoGrid").dataSource.filter(filters);



    }
    HideLoading();
    ////
    //var IsModuleAdmin = Boolean($('[id$="hdnIsModuleAdmin"]').val());
    ////alert(IsModuleAdmin);
    //if (IsModuleAdmin == false) {
    //    $("#grdProjectDetails").data("kendoGrid").showColumn("BudgetedCost");
    //    $("#grdProjectDetails").data("kendoGrid").showColumn("ActualCost");
    //    $("#grdProjectDetails").data("kendoGrid").showColumn("ActualPayment");


    //}
    //else {
    //    $("#grdProjectDetails").data("kendoGrid").hideColumn("BudgetedCost");
    //    $("#grdProjectDetails").data("kendoGrid").hideColumn("ActualCost");
    //    $("#grdProjectDetails").data("kendoGrid").hideColumn("ActualPayment");

    //    //$('#lblBudgetedCost').hide();
    //    //$('#lblActualCost').hide();
    //    //$('#lblPaymentRec').hide();
    //    $('#trBudgetdCost').hide();
    //    $('#thPayRec').hide();
    //    $('#tdPayRec').hide();
    //}
    ////
}
function convert(str) {
    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];

    var newDate = new Date(str);
    var formattedDate = (newDate.getDate() < 10 ? '0' : '') + newDate.getDate() + '/' + monthNames[newDate.getMonth()] + '/' + newDate.getFullYear();

    return formattedDate;
}
//////////////////////////////     TimeSheet Breakup Section (2nd Popup) ///////////////////////////

function GetTSBreakupManage(prjId) {
    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTSBreakupDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTSBreakupDataManage(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetTSBreakupDataManage(Tdata) {
    $("#grdTSBreakup").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Module: {
                            type: "string"
                        },
                        Percentage_Effort: {
                            type: "string"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        //UnApprovedHours: {
                        //    type: "number"
                        //},
                        Cost: {
                            type: "number"
                        },
                    }
                }
            },
            //pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [{
            field: "Module",
            title: "Work",
            width: "80px",
        },
        {
            field: "Percentage_Effort",
            title: "Percentage Effort %",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(Percentage_Effort,"n0") #</div>'
        },
        {
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'
        },
        //    {
        //    field: "UnApprovedHours",
        //    title: "Unapproved Hours",
        //    width: "50px",
        //    template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
        //},
        {
            field: "Cost",
            title: "Cost",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(Cost,"n0") #</div>'
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
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
    });

    //var IsModuleAdmin = Boolean($('[id$="hdnIsModuleAdmin"]').val());
    //if (IsModuleAdmin == false) {
    //    $("#grdTSBreakup").data("kendoGrid").showColumn("Cost");
    //}
    //else {
    //    $("#grdTSBreakup").data("kendoGrid").hideColumn("Cost");
    //}
}


//////////////////////////////     TimeSheet Details Section (2nd Popup) ///////////////////////////

function GetTimesheetDetailsManage(prjId) {
    $.ajax({
        type: "POST",
        url: "BITS.aspx/GetTimesheetDetailsMonthwise",
        contentType: "application/json;charset=utf-8",
        data: "{'prjId':'" + prjId + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetTimesheetDataManage(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                + x.responseText);
        }
    });
}

function GetTimesheetDataManage(Tdata) {
    $("#grdTSDetails").kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        Module: {
                            type: "string"
                        },
                        TSYear: {
                            type: "string"
                        },
                        TSMonth: {
                            type: "string"
                        },
                        Percentage_Effort: {
                            type: "string"
                        },
                        ActualHour: {
                            type: "number"
                        },
                        //UnApprovedHours: {
                        //    type: "number"
                        //},
                        Cost: {
                            type: "number"
                        },
                    }
                }
            },
            pageSize: 25,
        },
        scrollable: true,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        selectable: 'row',  //selects a row on click
        columns: [{
            field: "Module",
            title: "Month",
            width: "80px",
        },
        {
            field: "TSYear",
            title: "TSYear",
            width: "20px",
            hidden: true,
        },
        {
            field: "TSMonth",
            title: "TSMonth",
            width: "20px",
            hidden: true,
        },
        {
            field: "Percentage_Effort",
            title: "Percentage Effort %",
            width: "50px",
            //format: "{0:n0}",
            template: '<div class="ra">#= kendo.toString(Percentage_Effort,"n0") #</div>'
        },
        {
            field: "ActualHour",
            title: "Hours",
            width: "50px",
            //format: "{0:n0}",
            template: '<div class="ra">#= kendo.toString(ActualHour,"n0") #</div>'
        },
        //    {
        //    field: "UnApprovedHours",
        //    title: "Unapproved Hours",
        //    width: "50px",
        //    template: '<div class="ra">#= kendo.toString(UnApprovedHours,"n0") #</div>'
        //},
        {
            field: "Cost",
            title: "Cost",
            width: "50px",
            template: '<div class="ra">#= kendo.toString(Cost,"n0") #</div>'
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
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
        change: function (arg) {
            var window = $("#TSdetails");
            openTSPopUP();

            var gview = $("#grdTSDetails").data("kendoGrid");
            var selectedItem = gview.dataItem(gview.select());

            var gview1 = $("#grdProjectDetails").data("kendoGrid");
            var selectedItem1 = gview1.dataItem(gview1.select());

            $('[id$="hdnProjectId"]').val(selectedItem1.ID);
            $('[id$="hdnTSYear"]').val(selectedItem.TSYear);
            $('[id$="hdnTSMonth"]').val(selectedItem.TSMonth);

            GetTimesheetDetailsWorkwise($('[id$="hdnProjectId"]').val(), $('[id$="hdnTSYear"]').val(), $('[id$="hdnTSMonth"]').val());
            //showCursorPointerForMonthTSDetails()
        },
    });
    //var IsModuleAdmin = Boolean($('[id$="hdnIsModuleAdmin"]').val());
    //if (IsModuleAdmin == false) {
    //    $("#grdTSDetails").data("kendoGrid").showColumn("Cost");
    //}
    //else {
    //    $("#grdTSDetails").data("kendoGrid").hideColumn("Cost");
    //}

}

//added by trupti for Status Button click
function openAddPopUP() {

    clearText();
    FillProjectStatus();
    GetProjStatusById($('[id$="hdnProjectId"]').val());
    GetProjectStatusDetails($('[id$="hdnProjectId"]').val());
    BindAllDateCalender();
    GetProjUpdateStatus($('[id$="hdnProjectId"]').val());

    setTimeout(openStatusPopUP, 500);

}

function clearText() {

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
function CancelAddPopUP() {
    //  e.preventDefault();
    $('#divStatusPopUP').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    //  location.reload(false);
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

function GetProjStatusById(projId) {

    $.ajax({

        type: "POST",
        url: "MyProjects.aspx/GetProjStatusByProjId",
        contentType: "application/json;charset=utf-8",
        data: "{'projid':'" + projId + "'}",
        cache: false,
        async: false,
        dataType: "json",
        success: function (data) {
            // var da = $.parseJSON(msg.d);
            var obj = jQuery.parseJSON(data.d);

            $("#lblStatus1").html(obj[0].projStatus);


            var dropdownlist = $("#drpstatus").data("kendoDropDownList");
            dropdownlist.value(obj[0].projStatusTId);

            //$('[id$="drpstatus"]').val(obj[0].projStatusTId);
            $("#txtcomplete").html(obj[0].Status);
            var s = obj[0].projStatus;
            s = s.substring(0, s.indexOf('%'));
            if (s.length != 0) {
                $('#txtcomplete').val(s);
            }
            else {
                $('#txtcomplete').val(0);
            }
            $('#errormessage').html('');

            // $("#txtremark").html(obj[0].projRemark);

            //  $('[id$="drpstatus"]').html(obj[0].);
            //$('[id$="drpstatus"]').html(obj[0].projStatusTId);//.val(da.projStatusTId);

        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
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

            $("#lblProjectname1").html(obj[0].projectname);
            // $("#lblstatus1").html(obj[0].projectstatus);
            $("#lblprojid").html(obj[0].projid);
            $('#lblprojid').hide();


        },
        error: function (result) {
            alert("Error");
        }
    });
}
function openStatusPopUP() {

    $('#divStatusPopUP').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
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
function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
    window.location.reload();
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
    $("#lblProjectname1").html(ProjName);

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
/* CheckBox List Widget */

/// <summary>Kendo UI CheckBox List Widget.</summary>
/// <description>Kendo UI  widget that displays a list of checkboxes.</description>
/// <version>1.0</version>
/// <author>John DeVight</author>
/// <license>
/// Licensed under the MIT License (MIT)
/// You may obtain a copy of the License at
/// http://opensource.org/licenses/mit-license.html
/// </license>

function ShowLoading() {
    //alert("in ShowLoading");
    var h = $(document).height();
    $('.ModalLoad').css('margin-top', (h - 100) / 2 + 'px');
    $('.ModalPopUp').height(h);
    $('.ModalPopUp').show();
    return true;
}

function HideLoading() {
    //alert("in HideLoading");
    $('.ModalPopUp').hide();
}