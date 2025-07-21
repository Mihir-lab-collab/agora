<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="PayMaster.aspx.cs" Inherits="Member_PayMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        var GridReport = "#grdPayroll";



    </script>

    <script type="text/javascript" src="js/Payroll.js"></script>

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

        /*for displaying text right aligned*/
        .k-grid .ra,
        .k-numerictextbox .k-input {
            text-align: right;
        }
        /*end*/

        /*for wrapping column header text*/
        .k-grid .k-grid-header .k-header .k-link {
            height: auto;
        }

        .k-grid .k-grid-header .k-header {
            white-space: normal;
        }
        /*end*/
        .grid_head table tr td span.active-set {
            margin-top: 10px;
        }

        #grdPayroll {
            cursor: pointer;
        }


    </style>

    <script type="text/javascript">

        function CalculateTotalExp1(birthday) {

            var re = /^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/;

            if (birthday.value != '') {

                if (re.test(birthday.value)) {
                    birthdayDate = new Date(birthday.value);
                    dateNow = new Date();

                    var years = dateNow.getFullYear() - birthdayDate.getFullYear();
                    var months = dateNow.getMonth() - birthdayDate.getMonth();
                    //var days = dateNow.getDate() - birthdayDate.getDate();
                    if (isNaN(years)) {

                        document.getElementById('lblAge').innerHTML = '';
                        document.getElementById('lblError').innerHTML = 'Input date is incorrect!';
                        return false;

                    }

                    else {
                        document.getElementById('lblError').innerHTML = '';

                        if (months < 0 || (months == 0 && days < 0)) {
                            years = parseInt(years) - 1;
                            document.getElementById('lblAge').innerHTML = years + ' Years '
                        }
                        else {
                            document.getElementById('lblAge').innerHTML = years + ' Years '
                        }
                    }
                }
                else {
                    document.getElementById('lblError').innerHTML = 'Date must be mm/dd/yyyy format';
                    return false;
                }
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head" style="padding: 10px 10px 15px 10px;">
                    <table width="100%">
                        <tr>
                            <td width="38%">
                                <asp:Label ID="lblPayroll" Text="Payroll" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <input type="checkbox" id="chkIsActive" onclick="fnincludeInactive(this.checked);" />
                                <span class="active-set" id="Span1" style="font-size: small; text-align: left;">Show Inactive</span> &nbsp&nbsp
                            </td>
                            <%--<td align="right">Location:
                                    <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                                        OnSelectedIndexChanged="dlLocation_SelectedIndexChanged" Visible="true" AppendDataBoundItems="true" Width="200px">
                                    </asp:DropDownList>
                            </td>--%>

                        </tr>
                    </table>
                </div>

                <!--1st Grid -->
                <div id="grdPayroll"></div>
                <div id="divAddPopupOverlay" runat="server"></div>
                <div id="divOverlay"></div>

                <!--2nd Grid -->
                <div class="a_popbox" id="divSalDetails" style="display: none;">
                    <div class="popup_wrap" style="width: 835px; top: 5%; left: 20%; margin-top: 1%; height: auto; max-height: 650px; min-height: auto; overflow: auto;">
                        <div class="popup_head">
                            <h3>Salary Details:
                            <asp:Label ID="lblEmpNameID" runat="server"> </asp:Label></h3>
                            <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
                            <div class="clear">
                            </div>
                        </div>
                        <div id="grdSalDetails" style="align-content: center;"></div>
                        <div class="clear">
                        </div>
                        <br />

                        <%-- Section 2- Salary Details All --%>
                        <div class="popup_head">
                            <a runat="server" id="lnkPrev" onclick="fncallGetSalaryDataForEmp(this.id);"><b><<</b></a>
                            <asp:Label ID="lblyear" runat="server" Font-Names="Arial"></asp:Label>
                            <a runat="server" id="lnkNext" onclick="fncallGetSalaryDataForEmp(this.id);"><b>>></b></a>
                            <div class="clear">
                            </div>
                        </div>
                        <div id="grdSaldetailsAll" style="align-content: center;"></div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmpId" runat="server" />
    <asp:HiddenField ID="hdnYear" runat="server" />

</asp:Content>

