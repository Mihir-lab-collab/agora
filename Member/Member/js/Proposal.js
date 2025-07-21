$(document).ready(function () {

    GetProjectDetails();
    var grid = $('#projectID').data("kendoGrid");
});

function GetProjectDetails() {
    $.ajax({
        type: "POST",
        url: "Proposal.aspx/BindProjects",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetProjectData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. " + x.responseText);
        }
    });
}

var StatusData = [{
    text: "Draft",
    value: "d"
}, {
    text: "Published",
    value: "p"
}, {
    text: "Acquired",
    value: "a"
}, {
    text: "Deleted",
    value: "r"
},

];

function FillStatus() {
    var Status = $("#drpStatusedit").kendoDropDownList({
        dataSource: [{
            text: "Draft",
            value: "d"
        }, {
            text: "Published",
            value: "p"
        }, {
            text: "Acquired",
            value: "a"
        }, {
            text: "Deleted",
            value: "r"
        },

        ],
        dataTextField: "text",
        dataValueField: "value",
    }).data("kendoDropDownList");
}


function GetProjectData(Tdata) {
    $(GridProposalProjects).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        projectID: {
                            type: "number",
                            editable: false
                        },
                        projectTitle: {
                            type: "string"
                        },
                        projDesc: {
                            type: "string"
                        },
                        accessCode: {
                            type: "string"
                        },
                        createdBy: {
                            type: "string"
                        },
                        createdOn: {
                            type: "date"
                        },
                        modifiedBy: {
                            type: "string"
                        },
                        modifiedOn: {
                            type: "date"
                        },
                        clientMail: {
                            type: "string"
                        },
                        status: {
                            type: "string"
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
        columns: [{
            field: "projectID",
            title: "Project Id",
            width: "50px",
            hidden: true
        }, {
            field: "projectTitle",
            title: "Name",
            width: "80px",
            template: '<a href="Redirect.aspx?p=#=projectID#" target="_Blank">#= projectTitle #</a>'
        }, {
            field: "clientMail",
            title: "Email ID",
            width: "150px"
        }, {
            field: "accessCode",
            title: "Access Code",
            width: "150px"
        }, {
            field: "createdBy",
            title: "Created By",
            width: "80px"
        }, {
            field: "createdOn",
            format: "{0:dd-MMM-yyyy}",
            title: "Created On",
            width: "100px"
        }, {
            field: "modifiedBy",
            title: "Modified By",
            width: "80px"
        }, {
            field: "modifiedOn",
            format: "{0:dd-MMM-yyyy}",
            title: "Modified On",
            width: "100px"
        }, {
            field: "status",
            title: "Status",
            width: "80px"
        }, {
            command: [{
                name: "edit"
            }, {
                name: "copy",
                click: addNewCustomRow

            }],
            width: "60px"
        }],

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
        edit: function (e) {
            var editWindow = e.container.data("kendoWindow");
            if (e.model.isNew()) {
                FillStatus();
                var projectTitle = e.model.projectTitle;
                $('[id$="txtEditProjectTitle"]').val(projectTitle);



                var projectDesc = e.model.projDesc;

                $('[id$="txtEditProjectDesc"]').val(projectDesc);

                var clientMail = e.model.clientMail;
                $('[id$="txtEditClientMaildId"]').val(clientMail);
                var dropdownlist = $("#drpStatusedit").data("kendoDropDownList");
                dropdownlist.text(e.model.status);
                e.container.data("kendoWindow").title('Project Details');
                var width = $("#trProject").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);
            } else {
                e.container.data("kendoWindow").title('Project Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                id = e.model.projectID;
                var projectEditTitle = $('[id$="txtEditProjectTitle"]').val();
                var projectEditDesc = $('[id$="txtEditProjectDesc"]').val();
                var clientEditMail = $('[id$="txtEditClientMaildId"]').val();
                var status = $('[id$="drpStatusedit"]').val();

                UpdateProjectDetails(id, projectEditTitle, projectEditDesc, clientEditMail, status);
                RedirectPage();
            } else {
                id = "0";

            }
        }

    });

}

function addNewCustomRow(e) {
    e.preventDefault();

    customRowDataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var data = this.dataItem(customRowDataItem);
    var projectTitle = customRowDataItem.projectTitle;
    projectTitle = projectTitle + "_copy";
    var projectDesc = customRowDataItem.projDesc;
    var clientMail = customRowDataItem.clientMail;


    var window = $("#window").kendoWindow({
        title: "Are you sure want to copy this record?",
        visible: false, //the window will not appear before its .open method is called
        width: "300px",
        height: "80px",
    }).data("kendoWindow");


    var windowTemplate = kendo.template($("#windowTemplate").html());
    window.content(windowTemplate); //send the row data object to the template and render it
    window.open().center();
    // SaveProjectDetails(projectTitle, projectDesc, clientMail);
    // RedirectPage();
    // $('#divProposal').css('display','');

    //  $("#yesButton").click(function () {
    $("#yesButton").click(function () {
        SaveProjectDetails(projectTitle, projectDesc, clientMail);
        RedirectPage();
        window.close();
    })
    $("#noButton").click(function () {
        window.close();
    })
    //isCustomAdd = true;
    //this.addRow();
}




function newPopup(url) {
    popupWindow = window.open(
        url, 'popUpWindow', 'height=700,width=800,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')
}

function getId(id) {
    var abc = id;
}



function UpdateProjectDetails(id, projectEditTitle, projectEditDesc, clientEditMail, status) {

    $.ajax({

        type: "POST",
        url: "Proposal.aspx/UpdateProposalProjects",
        contentType: "application/json;charset=utf-8",
        data: "{'projectID':'" + id + "','projectTitle':'" + projectEditTitle + "','projDesc':'" + projectEditDesc + "','clientMail':'" + clientEditMail + "','status':'" + status + "'}",
        cache: false,
        async: false,
        dataType: "json",
        success: function (msg) {
            var message = jQuery.parseJSON(msg.d);
            alert('Project updated successfully.');
            $('body').css('cursor', 'default');

        },
        error: function (msg) {
            alert("The call to the server side failed." + msg.responseText);
        }
    });
}


function SaveProjectDetails(projectEditTitle, projectEditDesc, clientEditMail) {

    $.ajax({

        type: "POST",
        url: "Proposal.aspx/SaveProjectDetails",
        contentType: "application/json;charset=utf-8",
        data: "{'projectTitle':'" + projectEditTitle + "','projDesc':'" + projectEditDesc + "','clientMail':'" + clientEditMail + "'}",
        cache: false,
        async: false,
        dataType: "json",
        success: function (msg) {
            var message = jQuery.parseJSON(msg.d);
            alert('Project copied successfully.');
            $('body').css('cursor', 'default');

        },
        error: function (msg) {
            alert("The call to the server side failed." + msg.responseText);
        }
    });
}

function ClosingRateWindow(e) {

    var grid = $("#gridProposalPojects").data("kendoGrid");
    grid.refresh();

}

function RedirectPage() {
    window.location.assign("Proposal.aspx");
}

function openAddPopUP() {

    $('#divAddPopup').css('display', '');
    $('#divAddPopupOverlay').addClass('k-overlay');
}

function closeAddPopUP() {
    $('#divAddPopup').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function closeTeamPopUP() {
    //$('#divTeamMembers').html('');
    // $('#txtDevelopmentTeam').html('');
    $('#divProposal').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
}

function ShowAddPopup() {
    openAddPopUP();
    $("#txtProjectTitle").val('');
    $("#txtProjectDesc").val('');
    $("#lblerrmsgProjectTitle").html("");
    $("#lblerrmsgProjectDesc").html("");
}