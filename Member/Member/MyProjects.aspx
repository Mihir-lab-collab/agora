<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="MyProjects.aspx.cs" Inherits="Member_MyProjects" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
  <%-- <script src="js/jquery.min.1.9.1.js" type="text/javascript"></script>--%>
  <%--<script src="jquery.min.map" type="text/javascript"></script>--%>
    <script src="../Member/js/MyProjectlist.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript">



        var ProjName = ('<%= Session["ProjectName"]%>');

        //commented by Pravin on 15 May 2014


        // Added by Pravin on 15 May 2014 : start
        var projId = ('<%= Session["ProjectId"]%>');
        var CustId = ('<%= Session["CustId"]%>');

        var GridTaskManager = "#gridTaskMang";
        function GetTextElements()
        {
            //  document.getElementById("<%=hftxtDevelopmentTeam.ClientID%>").value = $("#txtDevelopmentTeam").data("kendoMultiSelect").value().toString();       
            var arr = new Array();
            var lbox = document.getElementById('<%= lstAddTeam.ClientID %>');
            document.getElementById("<%=hftxtDevelopmentTeam.ClientID%>").value = "";
            for (var i = 0; i < lbox.length; i++) {

                document.getElementById("<%=hftxtDevelopmentTeam.ClientID%>").value += document.getElementById("<%=hftxtDevelopmentTeam.ClientID%>").value != "" ? ',' + lbox.options[i].value : lbox.options[i].value;

            }
            return true;

        }

        // Added by Apurva on 9 Feb 2016
        function moveMember() {
            var oSrc = document.getElementById('<%= lstteam.ClientID %>');
            var oDest = document.getElementById('<%= lstAddTeam.ClientID %>');
         
            for (var i = 0; i < oSrc.options.length; i++)
            {
                var contains = false;
                if (oSrc.options[i].selected)
                {
                    for (var j = 0, ceiling = oDest.options.length; j < ceiling; j++)
                    {

                        if (oDest.options[j].value == oSrc.options[i].value)
                        {
                            contains = true;
                            break;
                        }
                    }
                    if (contains)
                    {
                        alert("This member is already added");
                    }
                    else
                    {
                        var option = document.createElement("option");
                        oDest.appendChild(option);
                        option.value = oSrc.options[i].value;
                        option.text = oSrc.options[i].text;
                    }
                }
            }
        }
        
       function removeMember()
        {
            var oSrc = document.getElementById('<%= lstteam.ClientID %>');
            var oDest = document.getElementById('<%= lstAddTeam.ClientID %>');
            var count = oDest.options.length;
            for (var i = 0; i < count; i++)
            {
                if (oDest.options[i].selected == true)
                {
                    try
                    {
                        oDest.remove(i, null);
                        i--;
                    }
                    catch (error)
                    {

                        oDest.remove(i);
                        i--;
                    }
                }
            }
        }


    </script>

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
   
     <script type="text/javascript">

         function CancelPopUP() {
             CancelAddPopUP();
             return false;
         }

        function CheckInsert() {
            document.getElementById("<%=hfProjectStatus.ClientID%>").value = $("#drpstatus").val();
            document.getElementById("<%=hfprojstatusDate.ClientID%>").value = $("#txtStartDate").val();
            document.getElementById("<%=hfprojcompletion.ClientID%>").value = $("#txtcomplete").val();
            document.getElementById("<%=hfRemarks.ClientID%>").value = $("#txtremark").val();
            document.getElementById("<%=hfprojid.ClientID%>").value = $("#lblprojid").html();
            closeAddPopUP();
        }

        function isNumberKeyOrDelete(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 46 || charCode > 57 || charCode == 8))
                return false;
            return true;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content_wrap">

        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td align="left">

                                <asp:Label ID="lblCusomerModule" Text="My Projects" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <input type="checkbox" id="includeall" onclick="fnincludeall(this.checked);" />
                                <span id="Span1" style="font-size: small;">Show All</span> &nbsp&nbsp
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridTaskMang"></div>
            </div>
        </div>
    </div>


    <div id="divAddPopupOverlay" runat="server"></div>
    
    <div class="a_popbox" id="divStatusPopUP" style="display: none;">
    <div class="k-widget k-windowAdd" id="divAddPopup" padding-top: 10px; padding-right: 10px; min-width: 100px; min-height: 80px; top: 10%; left: 500px; z-index: 10003; margin-top: 5%; opacity: 1; transform: scale(1);" data-role="draggable" >
        <div>
            <div class="popup_head">
                <h3>Update Status</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="javascript: return CancelPopUP();"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

          <asp:UpdatePanel ID="pnlUpdateStatus" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form project-tbl">
                       <tr style="display:none">
                            <td colspan="4">
                                <asp:Label ID="lblprojid" runat="server" Style="text-align: left; width: 100%;" ClientIDMode="Static" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>Project</th>
                            <td align="left">
                                <asp:Label ID="lblProjectname" runat="server" ClientIDMode="Static" Style="text-align: left; width: 100%;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>Last Status</th>
                            <td align="left">
                                <asp:Label ID="lblstatus" runat="server" ClientIDMode="Static" Style="text-align: left; width: 100%;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>Current Status</th>
                            <td align="left">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <th>Date</th>
                                        <td>
                                            <input id  ="txtStartDate" name="txtStartDate" onkeyup="dateInput(this)" onkeypress="dateInput(this)" style="width: 230px" class="k-textbox" />
                                            <span style="color: Red;">*</span>
                                            <span id="lblmsgdate" style="color: Red;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Project Status Type</th>
                                        <td>
                                            <input id="drpstatus" type="text" style="width: 230px" />
                                            <span style="color: Red;">*</span>
                                            <span id="lblmsgstatus" style="color: Red;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Completed</th>
                                        <td>
                                            <input id="txtcomplete" style="width: 230px" class="k-textbox" onkeypress="return isNumberKeyOrDelete(event);"/>
                                            <span style="color: Red;">*</span>
                                            <span id="lblmsgcomplete" style="color: Red;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Remark</th>
                                        <td>

                                           <%-- <input id="txtremark" style="width: 230px" class="k-textbox" />--%>
                                           <%--  <asp:TextBox runat="server" ID="txtremark" TextMode="MultiLine" Height="57px" Width="230px"></asp:TextBox>--%>
                                            <textarea cols="60" rows="5" id="txtremark" name="message"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <td>
                                            <asp:Label ID="errormessage" runat="server" ClientIDMode="Static"></asp:Label></td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <th></th>
                            <td>
                                <asp:LinkButton ID="lnkSaveProjectStatus" runat="server" CssClass="small_button red_button open" OnClientClick="CheckInsert()" OnClick="lnkSaveProjectStatus_Click" ><span>Save</span></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;
                                <input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="CancelAddPopUP();" />
                            </td>
                        </tr>
                    </table>
                    <br />
                  <div class="popup_head" style="overflow:hidden; padding-bottom:10px; color:#000"> <h3> Project Status</h3></div>
<div id="GridProjectStaus"></div>    
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </div>

    <div id="divOverlay"></div>
    <div class="a_popbox" id="divPopUP" style="display: none;">
        <div class="popup_wrap" style="width: 900px; top: 1%; left: 25%; margin-top: 1%; height: 700px; overflow: auto;">

            <div class="popup_head">
                <h3>Project Details</h3>
                <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
                <div class="clear">
                </div>
            </div>

            <table cellspacing="1" cellpadding="4" width="100%" border="0" class="manage_form">

                <tr>
                    <td width="15%" valign="top">
                        <b>Project Name:</b></td>
                    <td width="35%" valign="top"><b>
                        <asp:Label ID="lblprjName" runat="server" ClientIDMode="Static"></asp:Label></b></td>
                    <td width="15%" valign="top"><b>Project Status:</b></td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblprojStatus" runat="server" ClientIDMode="Static"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top" width="15%">
                        <b>Customer Name:</b></td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblCustName" runat="server" ClientIDMode="Static"></asp:Label></td>
                    <td width="15%" valign="top"><b>Exp Project Status:</b></td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblExpProjStatus" runat="server" ClientIDMode="Static"></asp:Label></td>

                </tr>
                <tr>
                    <td align="left" width="15%"><b>Customer Address:</b></td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblCustAddress" runat="server" ClientIDMode="Static"></asp:Label></td>
                    <td valign="top" width="15%"><b>Start Date:</b></td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblStartDate" runat="server" ClientIDMode="Static"></asp:Label></td>

                </tr>
                <tr>
                    <td align="left" width="15%"><b>Project Duration:</b>
                    </td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblProjDurat" runat="server" ClientIDMode="Static"></asp:Label></td>
                    <td valign="top" width="15%"><b>Exp Comp Date:</b></td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblExpDate" runat="server" ClientIDMode="Static"></asp:Label></td>

                </tr>

                <tr>
                    <td valign="top" align="left" width="15%">
                        <b>Project Manager:</b>
                    </td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblProjMang" runat="server" ClientIDMode="Static"></asp:Label></td>
                    <td valign="top" nowrap="nowrap" width="15%"><b>Act Comp Date
                    </b>
                    </td>
                    <td width="35%" valign="top">
                        <asp:Label ID="lblActCompDate" runat="server" ClientIDMode="Static"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left"><b>Project Team Member</b><b>
                    </b>
                    </td>
                    <td colspan="1">
                        <div id="GridTeamMember"></div>
                    </td>
                    <td colspan="3">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                            <tr>
                                <th>Total Tasks Reported:
                                </th>
                                <td>
                                    <span id="lblTotal">0</span>
                                </td>
                            </tr>
                            <tr>
                                <th>Total Tasks Resolved (Terminated):
                                </th>
                                <td>
                                    <span id="lblresolved">0</span>
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
                                                            <span id="lblshowstoper">0</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Major:</th>
                                                        <td>
                                                            <span id="lblmajor">0</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Cosmetic:</th>
                                                        <td>
                                                            <span id="lblCosmetic">0</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Minor:</th>
                                                        <td>
                                                            <span id="lblminor">0</span>
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
                                            <th valign="middle">Status Wise</th>
                                            <td valign="top">
                                                <table width="100%" style="vertical-align: top">
                                                    <tr valign="top">
                                                        <th valign="top">Open:</th>
                                                        <td>
                                                            <span id="lblOpen">0</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>In Progress:</th>
                                                        <td>
                                                            <span id="lblInProgress">0</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Completed:</th>
                                                        <td>
                                                            <span id="lblCompleted">0</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>On Hold:</th>
                                                        <td>
                                                            <span id="lblOnHold">0</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Ready for QA:</th>
                                                        <td>
                                                            <span id="lblReadyQA">0</span>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table cellspacing="1" cellpadding="4" width="100%" border="0" class="manage_form">
                <tr>
                    <td>
                        <h2>Project Status</h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="GridEmployeeStatus"></div>
                    </td>
                </tr>
            </table>

        </div>

    </div>

    <div class="a_popbox" id="divTeamMembers" style="display: none;">
        <div class="popup_wrap" style="width: 500px; top: 5%; left: 40%; margin-top: 15%;">
            <div class="popup_head">
                <h3>Team Members</h3>
                <input type="hidden" id="testd" name="testd" />
                <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closeTeamPopUP()" />
                <div class="clear">
                </div>
            </div>
            <table>
                <tr>
                    <td>
                        <asp:ListBox ID="lstteam" runat="server" SelectionMode="Multiple" Height="200"></asp:ListBox>
                    </td>
                    <td align="center">
                        <input type="button" id="btnTadd" runat="server" value="Add" class="small_button red_button open" onclick="moveMember();" />
                        <br />
                        <br />
                        <input type="button" id="btnTremove" runat="server" value="Remove" class="small_button red_button open" onclick="removeMember();" />
                    </td>
                    <td>
                        <asp:ListBox ID="lstAddTeam" size="4" runat="server" SelectionMode="Multiple" Height="200"></asp:ListBox>
                    </td>
                </tr>

                <tr>
                    <th></th>
                    <td style="width: 300px">
                        <asp:LinkButton ID="lnkSaveTeam" runat="server" CssClass="small_button red_button open" OnClientClick="javascript:return GetTextElements();javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveTeam_Click"><span>Save</span></asp:LinkButton>
                    </td>

                    <td style="width: 300px">
                        <input type="button" class="small_button red_button open" value="Cancel" id="Button1" onclick="closeTeamPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
     <div id="divloading" class="black_coverLoading" style="display: none">
        <center>
            <img alt="" src="images/loading-image.gif" /><br />
        </center>
    </div>
    <script type="text/x-kendo-template" id="popup-editor">
   
       
    </script>

    <asp:HiddenField ID="hfModuleID" runat="server" />
    <asp:HiddenField ID="hfProjectStatus" runat="server" />
    <asp:HiddenField ID="hfprojstatusDate" runat="server" />
    <asp:HiddenField ID="hfprojcompletion" runat="server" />
    <asp:HiddenField ID="hfRemarks" runat="server" />

    <asp:HiddenField ID="hfprojid" runat="server" />
    <asp:HiddenField ID="hfTaskName" runat="server" />
    <asp:HiddenField ID="hfTaskDesc" runat="server" />
    <asp:HiddenField ID="hfpriority" runat="server" />
    <asp:HiddenField ID="hfAssignedTo" runat="server" />
    <asp:HiddenField ID="hfBugID" runat="server" />
    <asp:HiddenField ID="hfNewFileName" runat="server" />
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hftxtDevelopmentTeam" runat="server" />



    <input type="hidden" id="hdn" class="hdnval" runat="server" />

</asp:Content>



