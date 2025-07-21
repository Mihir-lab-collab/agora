<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectDashboard.aspx.cs" Inherits="Member_ProjectDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../js/kendo.web.min.js"></script>

    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <link href="../Member/css/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <style>
        .Customtd {
            padding: 0 5px 0 15px;
            font-weight: bold;
        }

        .ProjDiv {
            padding: 10px;
            border-bottom: none !important;
            height: auto;
            overflow: hidden;
            background: url(../images/table-title.png) repeat-x left top !important;
            border: 1px solid #ccc;
            text-align: center;
            vertical-align: text-top;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            ////  GetData();
        });
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="ProjDiv">
                    <asp:Label ID="lblProjectName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>

                </div>
                <asp:Panel ID="pnlProjModule" runat="server">
                </asp:Panel>

                <asp:UpdatePanel ID="Mainpanel" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlTree" runat="server" Width="100%" HorizontalAlign="Center" Style="display: none; background-color: white;">
                            <table width="80%" style="margin-left: 186px;">
                                <tr>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjectManager" runat="server" Text="Project Manager Name:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjectManagerFor" runat="server"></asp:Label>
                                    </td>

                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblCustomername" runat="server" Text="Customer Name:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblCustomernameFor" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>

                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblDeveloperTeam" runat="server" Text="project Development Team:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblDeveloperTeamFor" runat="server"></asp:Label>
                                    </td>


                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjectDuration" runat="server" Text="Project Duration:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjectDurationFor" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>

                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblprojectSpendTime" runat="server" Text="Project Spend Time:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblprojectSpendTimeFor" runat="server"></asp:Label>
                                    </td>

                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjStatus" runat="server" Text="Project Status and Health:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjStatusFor" runat="server"></asp:Label>
                                    </td>

                                </tr>
                                <tr>

                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjBudgetedHours" runat="server" Text="Project Budgeted Hours:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjBudgetedHoursFor" runat="server"></asp:Label>
                                    </td>

                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjUnapprovedHours" runat="server" Text="Project Unapproved Hours:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjUnapprovedHoursFor" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjestStartdate" runat="server" Text="Project Start Date:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblProjestStartdateFor" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Project End Date:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd" valign="top">
                                        <asp:Label ID="lblEndDatefor" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left" height="30px" class="Customtd">
                                        <asp:Label ID="lblProjBugCost" runat="server" Text="Budged Cost:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd">
                                        <asp:Label ID="lblProjBugCostFor" runat="server"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd">
                                        <asp:Label ID="lblActualCust" runat="server" Text="Actual Cost:"></asp:Label>
                                    </td>
                                    <td align="left" height="30px" class="Customtd">
                                        <asp:Label ID="lblActualCustFor" runat="server"></asp:Label>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                        <asp:LinkButton ID="btnModalTarget" runat="server" Style="display: none;" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>

