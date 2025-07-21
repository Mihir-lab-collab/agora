<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="TaskManager.aspx.cs" Inherits="Member_TaskManager" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="..member/js/kendo.web.min.js" type="text/javascript"></script>
    <link href="../styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../js/cultures/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/TaskManager.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function closePopUP() {
            $('#divPopUP').css('display', 'none');
            $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
        }
        function openPopUP() {
            $('#divPopUP').css('display', '');
            $('#divOverlay').addClass('k-overlay');
        }

    </script>

    <script type="text/javascript" language="javascript">
        var ProjName = ('<%= Session["ProjectName"]%>');

        //commented by Pravin on 15 May 2014


        // Added by Pravin on 15 May 2014 : start
        var projId = ('<%= Session["ProjectId"]%>');
        var CustId = ('<%= Session["CustId"]%>');
        var GridTaskManager = "#gridTaskMang";

        // Added by Pravin on 15 May 2014 : end

        function GetDataOnInsert(Buttonid) {


            var filespendingtouload = $(".k-widget.k-upload").find(".k-button.k-upload-selected").is(':visible');
            //alert(filespendingtouload);
            if (Buttonid == "ContentPlaceHolder1_lnkSaveNewTask") {
                if (filespendingtouload == true) {
                    alert("Please upload the selected files first");
                    return false;
                }
            }

            var ModuleID = $("#drpmodule").val();
            var TaskName = $("#txtTaskName").val();
            var TaskDesc = $("#txtTaskDesc").val().replace(new RegExp("\\n", "g"), "<br/>");
            var Priority = $("#drpPriority").val();
            var AssgnTo = $("#drpAssignedTo").val();
            var Type = $("#drpType").val();

            var ModuleSpan = $("#lblerrmsgModule");
            var TaskNameSpan = $("#lblerrmsgTaskName");
            var TaskDescSpan = $("#lblerrmsgTaskDesc");
            var PrioritySpan = $("#lblerrmsgPriority");
            var AssgnToSpan = $("#lblerrmsgAssgnTo");
            var TypeSpan = $("#lblmsgType");

            //alert(ModuleID + ","+ TaskName+","+ TaskDesc+","+Priority+","+AssgnTo);
            //if (ModuleID=="" && TaskName=="" && TaskDesc=="" && Priority=="" && AssgnTo=="") {

            //    ModuleSpan.html("Please select module.");
            //    TaskNameSpan.html("Please enter task name.");
            //    TaskDescSpan.html("Please enter task description.");
            //    PrioritySpan.html("Please select priority.");
            //    AssgnToSpan.html("Please select project member.");
            //    return false;
            //}
            //if (ModuleID=="" && TaskName=="" && TaskDesc=="" && Priority=="") {

            //    ModuleSpan.html("Please select module.");
            //    TaskNameSpan.html("Please enter task name.");
            //    TaskDescSpan.html("Please enter task description.");
            //    PrioritySpan.html("Please select priority.");               
            //    return false;
            //}
            //else{
            //    ModuleSpan.html("");
            //    TaskNameSpan.html("");
            //    TaskDescSpan.html("");
            //    PrioritySpan.html("");
            //    AssgnToSpan.html("");

            //}
            if (ModuleID == "") {
                ModuleSpan.html("Please select module.");
                return false;
            }
            else {
                ModuleSpan.html("");
            }
            if (TaskName == "") {
                TaskNameSpan.html("Please enter task name.");
                return false;
            }
            else {
                TaskNameSpan.html("");
            }
            if (TaskDesc == "") {
                TaskDescSpan.html("Please enter task description.");
                return false;
            }
            else {
                TaskDescSpan.html("");
            }
            if (Type == "") {
                TypeSpan.html("Please select Type.");
                return false;
            }
            else {
                TypeSpan.html("");
            }
            if (Priority == "") {
                PrioritySpan.html("Please select priority.");
                return false;
            }
            else {
                PrioritySpan.html("");
            }
            if (AssgnTo == "") {
                AssgnToSpan.html("Please select project member.");
                return false;
            }
            else {
                AssgnToSpan.html("");
            }



            //if (ModuleID!="" && TaskName!="" && TaskDesc!="" && Priority!="" && AssgnTo!="") {
            if (ModuleID != "" && TaskName != "" && TaskDesc != "" && Priority != "" && Type != "") {
                document.getElementById("<%=hfModuleID.ClientID%>").value = ModuleID;
                document.getElementById("<%=hfTaskName.ClientID%>").value = TaskName;
                document.getElementById("<%=hfTaskDesc.ClientID%>").value = TaskDesc;
                document.getElementById("<%=hfpriority.ClientID%>").value = Priority;
                document.getElementById("<%=hfAssignedTo.ClientID%>").value = AssgnTo;
                document.getElementById("<%=hfType.ClientID%>").value = Type;

                return true;
            }

        }
    </script>
    <style type="text/css">
        .k-edit-form-container {
            width: 100%;
        }

        #details-container {
            padding: 10px;
        }

            #details-container h2 {
                margin: 0;
            }

            #details-container em {
                color: #8c8c8c;
            }

            #details-container dt {
                margin: 0;
                display: inline;
            }

        .ForeColor {
            color: red;
        }
    </style>
    <style type="text/css">
        .k-textbox {
            width: 11.8em;
        }

        #tickets {
            /*width: 510px;
                    height: 323px;
                    margin: 30px auto;
                    padding: 10px 20px 20px 170px;
                    background: url('../../content/web/validator/ticketsOnline.png') transparent no-repeat 0 0;*/
        }

            #tickets h3 {
                font-weight: normal;
                font-size: 1.4em;
                border-bottom: 1px solid #ccc;
            }

            #tickets ul {
                list-style-type: none;
                margin: 0;
                padding: 0;
            }

            #tickets li {
                margin: 10px 0 0 0;
            }

        label {
            display: inline-block;
            width: 90px;
            text-align: right;
        }

        .required {
            font-weight: bold;
        }

        .accept, .status {
            padding-left: 90px;
        }

        .valid {
            color: green;
        }

        .invalid {
            color: red;
        }

        span.k-tooltip {
            margin-left: 6px;
        }

        .note.error span {
            background: transparent url(../images/error.png) 0px 0px no-repeat;
        }

        .note.check span {
            background: transparent url(../images/check.png) 0px 0px no-repeat;
        }
    </style>
    <style type="text/css">
        /*headers*/

        .k-grid th.k-header,
        .k-grid-header {
            background: #252e34;
        }

            .k-grid th.k-header,
            .k-grid th.k-header .k-link {
                color: white;
            }

        /*rows*/

        .k-grid-content > table > tbody > tr {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }


        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }

        /*selection*/

        .k-grid table tr.k-state-selected {
            background: #f99;
            color: #fff;
        }
        /*for history grid [s]*/
        .k-alt {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*for history grid [e]*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td align="left">

                                <asp:Label ID="lblCusomerModule" Text="Task Manager" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td align="right">
                                <table>
                                    <tr>
                                        <%--    <td align="left">
                                            <span id="spnTerminated" style="font-size: small;">
                                                <asp:CheckBox ID="CheckBox1" runat="server" Width="200px" Text="Include Terminated Tasks"  AutoPostBack="true" />
                                            </span>
                                        </td>--%>
                                        <td>
                                            <%--<asp:CheckBox ID="chkTerminated" runat="server" Width="200px" Text="Include Terminated Tasks" onclick="alert(this.checked);" /></td>--%>
                                            <%-- <asp:CheckBox ID="chkTerminatedAsp" runat="server" Checked="true" Width="200px" Text="Include Terminated Tasks" onclick="fnTerminated(this.checked);" />--%>
                                            <input type="checkbox" id="chkTerminated" onclick="fnTerminatedClick(this.checked);" />
                                            <span id="Span1" style="font-size: small;">Include Terminated</span> &nbsp&nbsp
                                        </td>
                                        <td>
                                            <span id="Spnpage1" style="font-size: small;">Show: </span>
                                            <input id="comboBox" /><span id="Spnpage2" style="font-size: small;"> Records per Page</span>
                                        </td>
                                        <td>
                                            <span id="spn" runat="server" onclick="ShowAddPopup()" class="small_button white_button open">Add New Task</span>
                                        </td>
                                        <td>
                                            <span id="PrevMonth" onclick="GetReport()" class="small_button white_button open" runat="server" style="display: none">Report</span>
                                            <%--<span id="PrevMonth" onclick="GetReport()" class="small_button white_button open" style="display:block;">Report</span>--%> <%--added by pravin--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridTaskMang"></div>
            </div>
        </div>
    </div>
    <%--  PopUP Div Starts --%>
    <div id="divAddPopupOverlay" runat="server"></div>
    <%--<div class="a_popbox" id="divAddPopup" style="display: none;">--%>
    <%-- <div class="popup_wrap" style="width: 600px; top: 10%; left: 30%; overflow-y: auto; overflow-x: hidden; position: fixed">--%>
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Task</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Project</th>
                    <td align="left" width="450px">
                        <label id="lblProjectname" style="text-align: left; width: 100%;"></label>
                    </td>
                </tr>
                <tr>
                    <th>Module
                    </th>
                    <td>
                        <input id="drpmodule" style="width: 300px" onblur="return GetDataOnInsert(this.id); " />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgModule" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Task Name
                    </th>
                    <td>
                        <input id="txtTaskName" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return PasswordValidation();return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgTaskName" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Task Description
                    </th>
                    <td>
                        <textarea id="txtTaskDesc" style="width: 290px; height: 100px;" rows="5" cols="70" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgTaskDesc" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Type
                    </th>
                    <td>
                        <input id="drpType" style="width: 300px" onblur="return GetDataOnInsert(this.id);" />
                        <span style="color: Red;">*</span>
                        <span id="lblmsgType" style="color: Red;"></span>

                    </td>
                </tr>
                <tr>
                    <th>Priority
                    </th>
                    <td>
                        <input id="drpPriority" style="width: 300px" onblur="return GetDataOnInsert(this.id); " />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgPriority" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Assigned To
                    </th>
                    <td>
                        <%--<input id="drpAssignedTo" style="width: 300px" onblur="return GetDataOnInsert(this.id); " />--%>
                        <input id="drpAssignedTo" style="width: 300px" onblur="return GetDataOnInsert(this.id); " />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgAssgnTo" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th style="width: 20%;">Attachment
                    </th>
                    <td style="padding: 0;">
                        <input name="files" id="files" type="file" />
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:LinkButton ID="lnkSaveNewTask" runat="server" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveNewTask_Click"><span>Save</span></asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <div id="divOverlay"></div>
    <div class="a_popbox" id="divPopUP" style="display: none;">
        <div class="popup_wrap" style="width: 600px; top: 10%; left: 25%;">
            <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Total Tasks Reported:
                    </th>
                    <td>
                        <span id="lblTotal"></span>
                    </td>
                </tr>
                <tr>
                    <th>Total Tasks Resolved (Terminated):
                    </th>
                    <td>
                        <span id="lblresolved"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <th>Priority Wise</th>
                                <td valign="top">
                                    <table width="100%" style="vertical-align: top">
                                        <tr valign="top">
                                            <th valign="top">Showstopper:</th>
                                            <td>
                                                <span id="lblshowstoper"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Major:</th>
                                            <td>
                                                <span id="lblmajor"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Cosmetic:</th>
                                            <td>
                                                <span id="lblCosmetic"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Minor:</th>
                                            <td>
                                                <span id="lblminor"></span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <th>Status Wise</th>
                                <td valign="top">
                                    <table width="100%" style="vertical-align: top">
                                        <tr valign="top">
                                            <th valign="top">Open:</th>
                                            <td>
                                                <span id="lblOpen"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>In Progress:</th>
                                            <td>
                                                <span id="lblInProgress"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Completed:</th>
                                            <td>
                                                <span id="lblCompleted"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>On Hold:</th>
                                            <td>
                                                <span id="lblOnHold"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Ready for QA:</th>
                                            <td>
                                                <span id="lblReadyQA"></span>
                                            </td>
                                        </tr>
                                    </table>

                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

    </div>
    <%--  PopUP Div Ends --%>

    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table width="100%" cellpadding="0" cellspacing="0" class="manage_form">
        <tr id="trBugID" Style="display:none;"><td>#=bug_id#</td></tr>
         <tr id="trTask" class="manage_bg">
        <th>
                <label>Task No</label>
         </th>
         <td>#=bug_id#</td>
         </tr>
         <tr>
        <th>Status </th>
        <td><input  id="drpStatus" style="width: 300px"  />
         
         </td>        
        </tr>
            <tr>
                <th> Priority</th>
            <td><input  id="drpPriorityedit" style="width: 300px"/></td>
            </tr>
         <tr>
           <th>Assigned To</th>
            <td><input  id="drpAssignedToedit" style="width: 300px"/></td>
            </tr>
            <tr>
                <th >Task Title</th>
            <td  colspan="2">#=bug_name#</td>
            </tr>
            <tr>
                <th >Task Description</th>

              <td><textarea id ="bugDesc" style="width:290px;height:50px;" rows="2" cols="70" readonly></textarea>
                </td>
            </tr>
            <%-- <tr>
                <th >Type</th>
                <td><table><tr>
                  <td><input type="radio" value="true"   name="rdoUpdateType" data-bind="checked: selectedType" >Defect</input></td>
                     <td > <input type="radio" value="false"  name="rdoUpdateType" data-bind="checked:selectedType" >Change</input> </td>
                </tr></table>
                </td>
            </tr>--%>
            <tr>
                <th>Type</th>
                <td><input  id="drpTypeedit" style="width: 300px"/></td>
            </tr>
            <tr>
                <th >Comment</th>
        <td>
          <textarea name="Comment" id="Comment" style="width:290px;height:100px;" rows="5" cols="70"></textarea>
        <%-- <span style="color: Red;">*</span> -- required validationmessage="Please enter Comment."--%>
        </td>
            </tr>
         <tr>
                <th>Attachments</th>
        <td align="left">
        <div id="attachment">
          
       </div>
       <input name="Editfiles" id="Editfiles" type="file" />
        </td>
       </tr>
        <tr>
        <th></th>
          <td ><div id="tdUpdate"></div></td>
        </tr>
            <tr> <th> Comment History</th>           
              <td>             
             <div id="grdbugsResolution" style="max-height: 250px; overflow-y: auto; overflow-x: hidden;width:500px;"></div>
            <div id="divOldbugsResolution" style="max-width: 400px; overflow-x: auto; overflow-y: hidden;"> #=resolution#</div>
         <br>        
        </td>
            </tr>                         
        </table>

    </div>
       
    </script>

    <%-- <input type="hidden" id="uploadfilenames" name="uploadfilenames" value="test.jpg"/>--%>
    <asp:HiddenField ID="hfModuleID" runat="server" />
    <asp:HiddenField ID="hfTaskName" runat="server" />
    <asp:HiddenField ID="hfTaskDesc" runat="server" />
    <asp:HiddenField ID="hfpriority" runat="server" />
    <asp:HiddenField ID="hfAssignedTo" runat="server" />
    <asp:HiddenField ID="hfBugID" runat="server" />
    <asp:HiddenField ID="hfNewFileName" runat="server" />
    <asp:HiddenField ID="hfType" runat="server" />
    <input type="hidden" id="hdn" class="hdnval" runat="server" />
    <asp:HiddenField ID="hfLoginId" runat="server" />
    <asp:HiddenField ID="hfUserName" runat="server" />
    <asp:HiddenField ID="hfProjId" runat="server" />


    <%-- <label id="lblresolutionhistory" width="100%" >--%>
</asp:Content>

