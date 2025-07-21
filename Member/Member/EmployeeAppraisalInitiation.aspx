<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="EmployeeAppraisalInitiation.aspx.cs" Inherits="EmployeeAppraisalInitiation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/Appraisal.js"></script>

    <script type="text/javascript" language="javascript">

        function CheckInsert() {
            var GetProfileValues = $("#txtRoles").data("kendoMultiSelect").value().toString();
            $('#<% =hndEditRole.ClientID %>').attr('value', '');

            if (GetProfileValues != "") {
                document.getElementById("<%=hftxtAppraisalAuthority.ClientID%>").value = $("#txtRoles").data("kendoMultiSelect").value().toString();


                //SaveKRAdata();
                //return false;
            }
            else {
                alert("Please select roles");
                return false;
            }
        }

        function CheckEditRole() {
            var GetProfileValues = $("#txtEditRoles").data("kendoMultiSelect").value().toString();
            if (GetProfileValues != "") {
                document.getElementById("<%=hftxtAppraisalAuthority.ClientID%>").value = $("#txtEditRoles").data("kendoMultiSelect").value().toString();
                $('#<% =hndEditRole.ClientID %>').attr('value', 'DELETEROLE');

                //SaveKRAdata();
                 //return false;
             }
             else {
                 alert("Please select roles");
                 return false;
             }
        }

        function PreLinkBtnClick() {
            PreLinkClick();
            return false;
        }

        function NextLinkBtnClick() {
            NextLinkClick();
            return false;
        }

        function CheckManagerInsert() {
            SaveData();
            return false;
        }

    </script>

    <style type="text/css">
        .checkbox {
            width: 20px !important;
        }

        .displayNone {
            display: none;
        }
    </style>

    <style type="text/css">
        .Detailslbl {
            width: 300px;
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
        .stylelink {
            font-size: 17px;
            color: black;
        }

            .stylelink:hover {
                font-size: 14px;
                color: #F60;
            }

        .lnknexticon {
            margin-left: 60px;
        }

        .lnknextlable {
            margin-left: 30px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <%--            <style>
                select   { max-width:350px; width:100%; }
                select option  { overflow:hidden; text-overflow:ellipsis; color:#2e2e2e;background:#cbc8c8 url('images/bullte.png')  5px center no-repeat; height:5px; padding: 0.4em 0.6em 0.4em 20px;background-size: 9px; }
                select option:nth-child(even) { background-color:#eceaea;}
            </style>--%>
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">

                    <table width="100%">
                        <tr>
                            <td>
                                <span style="font-size: Medium; font-weight: bold;">Employee Performance Review</span><br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong style="font-size: 14px;">Quarter: </strong>
                                <label for="QuarterWiseDate" style="vertical-align: middle; font-size: 13px;"></label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td runat="server">
                                <asp:LinkButton ID="LnkbPrev" CssClass="ClcLnkbPrev" runat="Server" OnClientClick="JavaScript: return PreLinkBtnClick();" Text="Previous">
                                   <img class="LNKPRE_CLICK" src="images/button-previous.png" />
                                </asp:LinkButton>

                                <asp:LinkButton ID="LnkbNext" CssClass="ClsLnkbNext lnknexticon" runat="Server" OnClientClick="JavaScript: return NextLinkBtnClick();" Text="Next"> 
                                    <img id="NEXT_ICON" class="LNKNEXT_CLICK" src="images/button-next.png" />
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Previous</label>
                                <label class="lnknextlable">Next</label>
                            </td>
                        </tr>
                    </table>

                    <div id="msgBox"></div>
                    <br />
                </div>
                <div id="GetApprGrid"></div>
            </div>
        </div>

    </div>

    <%--Popup for assigning the roles --%>
    <div id="divOverlay"></div>
    <div id="divAddPopupOverlay"></div>
    <div class="a_popbox" id="divAddPopup" style="display: none;">
        <div class="popup_wrap" style="width: 360px; margin-left: 175px; top: -30%; left: 20%;">
            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP()" />

            <table width="100%">
                <tr>
                    <td colspan="2" align="center">
                        <span id="Span1" style="font-size: large; font-weight: 100">Add Profiles</span>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" border="0" width="104%" class="manage_form">
                <tr>
                    <td>
                        <asp:Panel ID="pnlAddProfile" runat="server">
                            <div id="example" class="k-content">
                                <div id="tickets">

                                    <table class="manage_form" width="70%">
                                        <tr>
                                            <th>
                                                <br />
                                                Profiles</th>
                                            <td style="width: 300px;">
                                                <br />
                                                <input id="txtRoles" multiple="multiple" data-placeholder="Select Roles" name="txtRoles" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="center">
                                                <br />
                                                <asp:Button ID="btnAddKRA" runat="server" CssClass="small_button white_button open" CausesValidation="false" OnClientClick="javascript:return CheckInsert()" OnClick="btnAddKRA_Click" Width="35%" Text="Save" />
                                                <%--<asp:Button ID="btnAddKRA" runat="server" CssClass="small_button white_button open" CausesValidation="false" OnClientClick="javascript:return CheckInsert()" Width="35%" Text="Save"  />--%>
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
    <input type="hidden" id="EmpId" value='<%= Session["EmpID"] %>' />


    <%--Popup for getting Employee Report--%>
    <div id="divOverlay2"></div>
    <div id="divAddPopupOverlay2"></div>
    <div class="a_popbox" id="divAddPopup2" style="display: none;">
        <div class="popup_wrap" style="top: 10%; left: 50%; margin-top: 1%; height: 400px; width: 900px; margin-left: -450px;">

            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP2()" />

            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server">
                            <div id="example2" class="k-content">
                                <div id="tickets2">
                                    <table width="100%">
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
                                                &nbsp;&nbsp;
                                                <strong style="font-size: 14px;">Authority Manager Name: </strong>
                                                <label for="AuthorityName" style="vertical-align: middle; font-size: 13px;"></label>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <div id="GetEmpAprReport"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <textarea id="txtcom" readonly="readonly" name="txtcom" cols="40" rows="5" style="width: 275px; height: 75px;" placeholder="Please Enter Comments Here"></textarea>
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


    <%--Popup for getting Employee ratings and for providing Manager ratings--%>
    <div id="divOverlay3"></div>
    <div id="divAddPopupOverlay3"></div>
    <div class="a_popbox" id="divAddPopup3" style="display: none;">
        <div class="popup_wrap" style="top: 10%; left: 50%; margin-top: 1%; height: auto; width: 600px; margin-left: -300px;">

            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP3()" />

            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server">
                            <div id="example3" class="k-content">
                                <div id="tickets3">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <br />
                                                <div id="gridmgr"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <textarea id="txtcomMngr" name="txtcom" cols="40" rows="5" style="width: 275px; height: 75px;" placeholder="Please Enter Comments Here"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="center">
                                                <br />
                                                <asp:Button ID="btnAddProjects" runat="server" CssClass="small_button red_button open" CausesValidation="false" OnClientClick="javascript:return CheckManagerInsert() " Text="Save" />
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


    <%--Popup for getting Employee project KRA'S--%>
    <div id="divOverlay4"></div>
    <div id="divAddPopupOverlay4"></div>
    <div class="a_popbox" id="divAddPopup4" style="display: none;">
        <div class="popup_wrap" style="top: 10%; left: 50%; margin-top: 1%; height: 60%; width: 600px; margin-left: -300px;">

            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP4()" />

            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="Panel3" runat="server">
                            <div id="example4" class="k-content">
                                <div id="tickets4">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <table width="70%">
                                                    <tr>
                                                        <td width="100px">Profiles : </td>
                                                        <td><input id="txtEditRoles" multiple="multiple" data-placeholder="Select Roles" name="txtEditRoles" />                                                       
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td><br />
                                                            <asp:Button ID="btnAddKRAUpdate" runat="server" CssClass="small_button white_button open" CausesValidation="false" OnClientClick="javascript:return CheckEditRole()" OnClick="btnAddKRA_Click" Width="35%" Text="Update" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                            </td>                                            
                                           
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <div id="gridEmpKra"></div>
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



    <%--<button id="BtnUpdateId" class="small_button white_button" onclick="UpdateInitiationRecord()" style="width: 101px; margin-left: 1093px; margin-top: 11px;">Update</button>--%>
    <asp:HiddenField ID="hftxtAppraisalAuthority" runat="server" />
    <asp:HiddenField ID="hndQuarterDate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hndAppraisalID" runat="server" />
    <asp:HiddenField ID="hdnProjectId" runat="server" />
    <asp:HiddenField ID="hdnEmpId" runat="server" />
    <asp:HiddenField ID="hndStatusId" runat="server" />
    <asp:HiddenField ID="hdnAppraisalStatus" runat="server" />
    <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnQuarterCounter" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hndEditRole" runat="server" ClientIDMode="Static" />
    <input type="hidden" id="hdnProjID" runat="server" />

</asp:Content>


