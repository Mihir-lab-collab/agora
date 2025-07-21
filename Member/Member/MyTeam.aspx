<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="MyTeam.aspx.cs" Inherits="Member_MyTeam" EnableEventValidation="false" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

   
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>

   <%-- <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../Member/Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../Member/css/layout.css" rel="stylesheet" type="text/css" />--%>

    <%--Bellow links are for kendo controles (do not change sequence)--%>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />


    <%--<link href="../Member/css/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/css/kendo.default.min.css" rel="stylesheet" />--%>
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="../Member/js/MyTeamlist.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        var GridMyTeam = "#gridMyTeam";
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
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }


        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0!important;
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
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*for history grid [e]*/
    </style>
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
                                <asp:Label ID="lblCusomerModule" Text="My Team" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                 <input type="checkbox" id="chkCompletedProjects" onclick="fnCompletedProjects(this.checked);"/>
                                 <span id="Span1" style="font-size: small;">My Projects</span> &nbsp&nbsp
                            </td>
                        </tr>
                    </table>
                </div>
                 <div id="gridMyTeam"></div>

            </div>
        </div>
    </div>
        <input type="hidden" id="hdn" class="hdnval" runat="server"/>

</asp:Content>



