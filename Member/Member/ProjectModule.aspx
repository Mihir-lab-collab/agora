<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectModule.aspx.cs" Inherits="Member_ProjectModule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <link href="../Member/css/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <style>
        .demo-section {
            width: 460px;
            padding: 30px;
        }

            .demo-section h2 {
                text-transform: uppercase;
                font-size: 1.2em;
                margin-bottom: 30px;
            }

            .demo-section label {
                display: inline-block;
                width: 120px;
                padding-right: 5px;
                text-align: right;
            }

            .demo-section .k-button {
                margin: 20px 0 0 125px;
            }

        .k-readonly {
            color: gray;
        }

        .manage_pop {
            border-collapse: collapse;
            border: 1px solid #e7e7e7;
            background: #EEC;
            width: 450px;
        }

            .manage_pop th {
                text-align: right;
                border-bottom: 1px solid #e7e7e7;
                border-right: 1px solid #e7e7e7;
                padding: 6px 10px;
                vertical-align: top;
            }

            .manage_pop td {
                text-align: left;
                border-bottom: 1px solid #e7e7e7;
                border-right: 1px solid #e7e7e7;
                padding: 7px 4px !important;
            }

            .manage_pop .th_head {
                padding: 0 5px 0 15px;
                font-weight: bold;
            }

            .manage_pop h2 {
                font-size: 18px;
            }

            .manage_pop .tab-heading {
                background-color: #f9f9f9;
            }

                .manage_pop .tab-heading h2 {
                    font-size: 14px;
                    font-weight: bold;
                    color: #88a245;
                }

        .treeNode {
            color: black;
            font: 12px Arial, Sans-Serif;
        }

        .rootNode {
            color: black;
            width: 30px;
        }

        .leafNode {
            color: black;
        }

        .selectNode {
            font-weight: bold;
            color: black;
        }
    </style>
    <script type="text/javascript">

        var ProjName = ('<%= Session["ProjectName"]%>');
        var projId = ('<%= Session["ProjectId"]%>');

        $(document).ready(function () {
            GetData();
            GetProIDforTree();
            document.getElementById("<%=RequiredFieldValidatorpop %>").disabled = false;

        });
        function GetData() {
            $.ajax(
                  {
                      type: "POST",
                      url: "Filter.aspx/FilterProject",
                      contentType: "application/json;charset=utf-8",
                      data: "{}",
                      dataType: "json",
                      success: function (msg) {
                          InitialiseControls(jQuery.parseJSON(msg.d));
                      },
                      error: function (x, e) {
                          alert("The call to the server side failed. " + x.responseText);
                      }
                  }
      );
        }
        function InitialiseControls(projectData) {
            var projectDt = $("#project").kendoDropDownList({
                optionLabel: "Select Project",
                dataTextField: "projName",
                dataValueField: "projid",
                dataSource: projectData

            }).data("kendoDropDownList");


        }

        function SetDataInPopup() {


            $('#<%=ModalPopupExtender1.ClientID %>').show();
            $('#<%=txtmodnamepopup.ClientID %>').hide();
            $('#<%=txtestimate.ClientID %>').hide();
            $('#<%=txtdesc.ClientID %>').hide();
            $('#<%=btndadds.ClientID%>').hide();
            $('#<%=btneditPop.ClientID%>').show();
            $('#<%=txtModEditPop.ClientID%>').show();
            $('#<%=txtEstEditPop.ClientID%>').show();
            $('#<%=txtDescEditPop.ClientID%>').show();
            $('#<%=ddlModuleType.ClientID%>').val($('#<%=hdnModuletype.ClientID%>').val());
            document.getElementById("<%=RequiredFieldValidatorpop %>").disabled = true;
        }

        function hideforEdit() {
            $(":checkbox[name='chk[]']").hide();
            $("#spnedit").hide();
        }

        function UserDeleteConfirmation() {
            var txtalert = $('#<%=lblNamepop.ClientID%>').html();
            return confirm("Are you sure you want to delete all modules of " + txtalert + "?");
        }

        function GetProIDforTree() {
            //var ProjectID = $("#project").val();
            document.getElementById("<%=hfProjID.ClientID%>").value = projId;
            document.getElementById("<%=hfdProjName.ClientID%>").value = ProjName;
            document.getElementById("<%=btnShowtree.ClientID%>").click();
        }

        function RemoveSpecialChar(txtModuleName) {
            if (txtModuleName.value != '' && txtModuleName.value.match(/^[\w ]+$/) == null) {
                txtModuleName.value = txtModuleName.value.replace(/[\W]/g, '');
            }
        }
        function isNumberKey(evt) {

            var charCode = (evt.which) ? evt.which : evt.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />--%>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblProjectModule" Text="Project Module" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>

                </div>
                <%--  <asp:Panel ID="pnlProjModule" runat="server">
                    <table class="manage_form" width="100%">
                        <tr>
                            <th>Projects</th>
                            <td>
                                <input id="project" style="width: 300px" onchange="return GetProIDforTree();" />
                            </td>
                        </tr>

                    </table>
                </asp:Panel>--%>
                <asp:UpdatePanel ID="Mainpanel" runat="server">
                    <ContentTemplate>

                        <asp:Panel ID="pnlTree" runat="server" Width="100%" HorizontalAlign="Center" Style="display: none; background-color: white;">
                            <table width="80%" style="margin-left: 186px;">
                                <tr>
                                    <td align="left" height="15px">
                                        <asp:Label ID="lblmsgpop" runat="server" Font-Bold="true" Font-Size="13px" ForeColor="red"></asp:Label>

                                    </td>
                                </tr>

                                <tr>
                                    <td align="left">
                                        <b>
                                            <asp:LinkButton ID="lnkprojname" runat="server" ToolTip="Select for Add,edit,delete modules" OnClick="lnkprojname_Click" Font-Size="13px" ForeColor="Black">

                                            </asp:LinkButton></b>
                                    </td>
                                </tr>

                                <tr>

                                    <td align="left">
                                        <asp:Button ID="btnShowtree" runat="server" Style="display: none" OnClick="btnShowtree_Click" />

                                        <asp:TreeView ID="TreeViewProjectModeles" runat="server" ToolTip="Select for Add,edit,delete modules" ShowLines="true" Width="100" OnSelectedNodeChanged="TreeViewProjectModeles_SelectedNodeChanged">
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <RootNodeStyle CssClass="rootNode" />
                                            <SelectedNodeStyle CssClass="selectNode" />
                                        </asp:TreeView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:LinkButton ID="btnModalTarget" runat="server" Style="display: none;" />
                        <asp:Panel ID="Panel1" runat="server" Style="position: fixed; z-index: 100001; left: 643.5px; top: 900px; width: 405px;">
                            <table width="100%" class="manage_pop">

                                <tr>
                                    <td>
                                        <b>Module: 
                                        </b>
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lblNamepop" runat="server" Font-Bold="true" Text=""></asp:Label>


                                        <input type="button" id="btnselectforedit" onclick="SetDataInPopup()" title="Click to edit" value="Edit"
                                            validationgroup="validatepopup" class="small_button white_button open"
                                            width="130px" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <b>Module Type:
                                        </b>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlModuleType" runat="server" Width="277px">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnModuletype" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td><b>Module Name:</b></td>
                                    <td>
                                        <asp:TextBox ID="txtmodnamepopup" runat="server" Width="266px"></asp:TextBox>
                                        <asp:TextBox ID="txtModEditPop" runat="server" Width="266px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorpop" runat="server" ControlToValidate="txtmodnamepopup"
                                            ErrorMessage="*" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"
                                            ValidationGroup="validatepopup"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatoreditpop" runat="server" ControlToValidate="txtModEditPop"
                                            ErrorMessage="*" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"
                                            ValidationGroup="validatepopupeditpop"></asp:RequiredFieldValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <td><b>Module Estimate:</b></td>
                                    <td>
                                        <asp:TextBox ID="txtestimate" runat="server" onkeypress="return isNumberKey(event)" Width="266px"></asp:TextBox>
                                        <asp:TextBox ID="txtEstEditPop" runat="server" onkeypress="return isNumberKey(event)" Width="266px"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>

                                    <td><b>Module Description:</b>
                                        <br />
                                        <em>(maximum 255 characters)</em>
                                    </td>
                                    <td align="right" valign="top">
                                        <asp:TextBox ID="txtdesc" runat="server" TextMode="MultiLine" Width="266px" Height="40px"></asp:TextBox>
                                        <asp:TextBox ID="txtDescEditPop" runat="server" TextMode="MultiLine" Width="266px" Height="40px"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td valign="top">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btndadds" runat="server" Text="Add" OnClick="btndadds_Click" CssClass="small_button white_button open" ValidationGroup="validatepopup" />
                                                    <asp:Button ID="btneditPop" runat="server" Text="Edit" OnClick="btneditPop_Click" CssClass="small_button white_button open" ValidationGroup="validatepopupeditpop" />
                                                </td>
                                                <td valign="top">
                                                    <asp:Button ID="btndelpop" runat="server" CssClass="small_button white_button open" OnClick="btndelpop_Click" OnClientClick="if ( ! UserDeleteConfirmation()) return false;" Text="Delete" />
                                                </td>
                                                <td valign="top">
                                                    <asp:Button ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" CssClass="small_button white_button open" />
                                                </td>
                                                <td valign="top">
                                                    <asp:Button ID="btnInactive" runat="server" Text="Inactive" OnClick="btnInactive_Click" CssClass="small_button white_button open" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnModalTarget" BehaviorID="mpe" PopupControlID="Panel1"></ajax:ModalPopupExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfProjID" runat="server" />
    <asp:HiddenField ID="hfdProjName" runat="server" />
</asp:Content>

