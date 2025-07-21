<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="EmpSkillMatrix.aspx.cs" Inherits="Member_EmpSkillMatrix" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Member/controls/EmployeeSkill.ascx" TagName="ucEmpSkill" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>--%>
    

    <%--Bellow links are for Kendo --%>    
    <%--<script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>--%>
    
    <%--<script src="js/SkillMatrix.js" type="text/javascript"></script>--%>
    <%--<script src="js/EmployeeSkill.js" type="text/javascript"></script>--%>

    <script type="text/javascript">

        function call() {
            alert("Saved Successfully");
        }
        function CheckSkillOnInsert(Buttonid) {
            var skill = $('[id$="txtSkillName"]').val();
            var errSkill = $("#lblerrmsgSkill");

            if (skill == "") {
                errSkill.html("Please enter Skill.");
                $("#lblerrmsgSkill").css('display', '');
                return false;
            }
        }
        function GetDataOnInsert(Buttonid) {
            //var skill = $('[id$="txtSkillName"]').val();
            var category = $('[id$="txtCategoryName"]').val();
            //var Time = $('#timepicker').val(); 

            var errHDate = $("#lblerrmsgdate");
            //var errSkill = $("#lblerrmsgSkill");
            if (category == "") {
                errHDate.html("Please enter category.");
                $("#lblerrmsgdate").css('display', '');
                return false;
            }
            //if (skill == "") {
            //    errSkill.html("Please enter Skill.");
            //    return false;
            //}
            <%-- else
         {
                errHDate.html("");
            }

            var errNarration = $("#lblerrmsgnarration");
            if (narration == "") {
                errNarration.html("Please Enter Description.");
                return false;
            }
            else {
                errNarration.html("");
            }

            if (EventDate != "") {
                document.getElementById("<%=hdnEventDate.ClientID%>").value = EventDate;                 
            }
            if (narration != "") {
                document.getElementById("<%=hdnDescription.ClientID%>").value = narration;                 
            }
            document.getElementById("<%=hdnTime.ClientID%>").value = Time;
         --%>
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

        .DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }
        /*for history grid [e]*/
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="temp"></div>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblHoliday" Text="Skill Matrix" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblLocation" Text="" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divUC">
                    <UC:ucEmpSkill ID="ucEmpSkills" runat="server"></UC:ucEmpSkill>
                </div>
            </div>
        </div>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>

    

   

    
    <%--<asp:HiddenField ID="hdnTime" runat="server" />--%>
</asp:Content>
