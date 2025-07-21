<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="EmpSelfAppraisal.aspx.cs" Inherits="Member_EmpSelfAppraisal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/EmployeeAppraisal.js" type="text/javascript"></script>

    <script type="text/javascript">

        function CheckInsert() {
            SaveData();
            return false;
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
        .stylelink {
            font-size: 17px;
            color: black;
        }

            .stylelink:hover {
                font-size: 14px;
                color: #F60;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                     <span style="font-size: Medium; font-weight: bold;">Self Performance Review</span><br />
                 <br />
                </div>
            </div>           
            <div id="gridProject"></div>
        </div>
    </div>

    <div id="divOverlay"></div>
    <div id="divAddPopupOverlay"></div>
    <div class="a_popbox" id="divAddPopup" style="display: none;">
        <div class="popup_wrap" style="top: 10%; left: 50%; margin-top: 1%; height: auto; width: 600px; margin-left: -300px;">

            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP()" />

            <style>
                select   { max-width:350px; width:100%; }
                select option  { overflow:hidden; text-overflow:ellipsis; color:#2e2e2e;background:#cbc8c8 url('images/bullte.png')  5px center no-repeat; height:5px; padding: 0.4em 0.6em 0.4em 20px;background-size: 9px; }
                select option:nth-child(even) { background-color:#eceaea;}
            </style>

            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlAddProfile" runat="server">
                            <div id="example" class="k-content">
                                <div id="tickets">
                                    <table>
                                        <tr>
                                            <td>
                                                <br />
                                                <div id="gridKras"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="center">
                                                <br />
                                                <asp:Button ID="btnAddProjects" runat="server" CssClass="small_button red_button open" CausesValidation="false" OnClientClick="javascript:return CheckInsert() " Text="Save" />
                                                <%--<asp:Button ID="btnAddKRA" runat="server" CssClass="small_button white_button open" CausesValidation="false" OnClientClick="javascript:return CheckInsert()" OnClick="btnAddKRA_Click" Width="35%" Text="Save"  />--%>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div id="divOverlay2"></div>
    <div id="divAddPopupOverlay2"></div>

    <div class="a_popbox" id="divAddPopup2" style="display: none;">
        <div class="popup_wrap" style="top: 10%; left: 50%; margin-top: 1%; height: 315px; width: 900px; margin-left: -450px;">

            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeReportPopup()" />

            <%-- <style>
                select   { max-width:350px; width:100%; }
                select option  { overflow:hidden; text-overflow:ellipsis; color:#2e2e2e;background:#cbc8c8 url('images/bullte.png')  5px center no-repeat; height:5px; padding: 0.4em 0.6em 0.4em 20px;background-size: 9px; }
                select option:nth-child(even) { background-color:#eceaea;}
            </style>--%>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server">
                            <div id="example2" class="k-content">
                                <div id="tickets2">
                                    <table>
                                        <tr>
                                            <td>
                                                <strong style="font-size: 14px;">Quarter: </strong>
                                                <label for="QuarterDate" style="vertical-align: middle; font-size: 14px;"></label>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong style="font-size: 14px;">Employee Name: </strong>
                                                <label for="EmpName" style="vertical-align: middle; font-size: 13px;"></label>
                                                &nbsp;&nbsp;
                                                <strong style="font-size: 14px;">Project Name: </strong>
                                                <label for="ProjName" style="vertical-align: middle; font-size: 13px;"></label>
                                               <%-- &nbsp;&nbsp;
                                                <strong style="font-size: 14px;">Authority Manager Name: </strong>
                                                <label for="AuthorityName" style="vertical-align: middle; font-size: 13px;"></label>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <div id="GetEmpSelfAprReport"></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </div>



    <div>
        <table id="tbladdProj">
            <tr>
                <td>
                    <br />
                </td>

            </tr>
        </table>
    </div>

    <asp:HiddenField ID="hdnProjectId" runat="server" />

    <%--<asp:HiddenField ID="hdnLoginID " runat="server"/>--%>
</asp:Content>
