$(document).ready(function () {

    GetCSProjectDetails();
    var grid = $('#projectID').data("kendoGrid");
});

function GetCSProjectDetails() {
    $.ajax({
        type: "POST",
        url: "ProposalCS.aspx/BindCSProjects",
        contentType: "application/json;charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetProjectCSData(jQuery.parseJSON(msg.d));
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}



function GetProjectCSData(Tdata) {
    $(GridProposalCSProjects).kendoGrid({
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        ProposalCSDefaultID: { type: "number", editable: false },
                        ProjectTitle: { type: "string" },
                        ProjectUrl: { type: "string" },
                        ProjectDesc: { type: "string" },
                        Image: { type: "string" }
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
        columns: [
                    { field: "ProposalCSDefaultID", title: "Project Id", width: "50px", hidden: true },
                    { field: "ProjectTitle", title: "Project Title", width: "50px" },
                    { field: "ProjectUrl", title: "Project Url", width: "55px" },
                    { field: "ImageName", title: "Image", width: "70px", template: '<img src="../Member/CMSImages/#= ImageName #" alt="image" width="42" height="42"/>',hidden:true},
                   // { field: "ImageName", title: "Image", width: "70px", template: ' <img src="data:image/png;base64,#= ImageName #"/>' },
                  // { field: "ImageName", title: "Image", width: "1024px", height: "768px" },
                  // { field: "Image", title: "Image", width: "70px", template: '<img  src="../Member/ViewImage.ashx?ImageName=#=ImageName#" alt="image" width="42" height="42"/>' },
                 
                   // { field: "ProjectDesc", title: "Project Description", width: "100px" },
                   { command: [{ name: "edit" }], width: "60px" }],

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

                var projectTitle = e.model.ProjectTitle;
                $('[id$="txtEditProjectCSTitle"]').val(projectTitle);

                var projectUrl = e.model.ProjectUrl;
                $('[id$="txtEditProjectCSUrl"]').val(projectUrl);
                var projectDesc = e.model.ProjectDesc;

                $('[id$="txtEditProjectCSDesc"]').val(projectDesc);


                e.container.data("kendoWindow").title('Project Details');
                var width = $("#trProject").width();
                $(".k-edit-form-container").parent().width(width + 20).data("kendoWindow").center();
                updateButton = e.container.find(".k-grid-update");
                cancelbutton = e.container.find(".k-grid-cancel");
                $("#tdUpdate").append(updateButton);
                $("#tdUpdate").append(cancelbutton);

                //$("#Editfiles").kendoUpload({
                //    async: {

                //        saveUrl: "TaskManager.aspx",
                //        removeUrl: "TaskManager.aspx",
                //        autoUpload: false
                //    },


                //});

            }
            else {
                e.container.data("kendoWindow").title('Project Details');
            }
        },
        save: function (e) {
            if (e.model.isNew()) {
                id = e.model.ProposalCSDefaultID;
                var projectEditCSTitle = $('[id$="txtEditProjectCSTitle"]').val();
                var projectEditCSUrl = $('[id$="txtEditProjectCSUrl"]').val();
                var projectEditCSDesc = $('[id$="txtEditProjectCSDesc"]').val();
                var ImageName = $('[id$="FileEditUpload"]').val();
                ImageName = ImageName.match(/[-_\w]+[.][\w]+$/i)[0];
                var filext = ImageName.substring(ImageName.lastIndexOf(".") + 1);
                if (filext == "jpg" || filext == "jpeg" || filext == "gif" || filext == "bmp") {
                    var contentype = "image" + "/" + filext;
                }
                var str = String.fromCharCode.apply(ImageName);
                var imagedata = str.replace(/.{76}(?=.)/g, '$&\n');
                UpdateProposalCSProjects(id, projectEditCSTitle, projectEditCSUrl, projectEditCSDesc, ImageName, contentype);
                RedirectPage();
            }
            else {
                id = "0";

            }
        }
    });

}

function newPopup(url) {
    popupWindow = window.open(
        url, 'popUpWindow', 'height=700,width=800,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')
}

function getId(id) {
    var abc = id;
}

function UpdateProposalCSProjects(id, projectEditCSTitle, projectEditCSUrl, projectEditCSDesc, ImageName, contentype) {

    $.ajax(
           {

               type: "POST",
               url: "ProposalCS.aspx/UpdateProposalCSProjects",
               contentType: "application/json;charset=utf-8",
               data: "{'projectCSID':'" + id + "','projectCSTitle':'" + projectEditCSTitle + "','projCSUrl':'" + projectEditCSUrl + "','projectEditCSDesc':'" + projectEditCSDesc + "','ImageName':'" + ImageName + "','contentType':'" + contentype + "'}",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   alert('Project updated successfully.');
                   $('body').css('cursor', 'default');

               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );
}

function ClosingRateWindow(e) {

    var grid = $("#gridProposalCSPojects").data("kendoGrid");
    grid.refresh();

}

function RedirectPage() {
    window.location.assign("ProposalCS.aspx");
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
    $("#txtProjectCSTitle").val('');
    $("#txtProjectCSDesc").val('');
    $("#lblerrmsgProjectTitle").html("");
    $("#lblerrmsgProjectDesc").html("");
}