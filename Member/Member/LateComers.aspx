<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LateComers.aspx.cs" MasterPageFile="~/Member/Admin.master" Inherits="Member_LateComers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/LateComers.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SearchText() {
            SearchLateComing();
            return false;
        }

        function ResetGrid() {
            Reset();
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

        .DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }
        /*for history grid [e]*/
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">

                    <asp:Label ID="lbl" Text="Late Coming" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <%--<asp:Label ID="lblLocation" Text="" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>--%>
                    <div style="float: right">


                        <div id="ddlStatus" align="right">
                            <select name="Status">
                                <option value="0">Pending </option>
                                <option value="1">Approved </option>
                                <option value="2">Rejected </option>
                                <option value="3">Admin Cancel </option>
                            </select>
                        </div>

                    </div>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td style='width: 140px'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>

                                    </tr>



                                </table>
                            </td>
                        </tr>
                    </table>
                    <table id="Table1" cellspacing="0" cellpadding="4" border="0" width="55%">
                        <tbody>

                            <tr>
                                <td align="left" width="25%">
                                    <b><span id="lblLocation">Location:</span></b>
                                </td>
                                <td colspan="3">

                                    <b><span id="lblLocationId" runat="server"></span></b>
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="25%">
                                    <b>Employee Name:</b>
                                </td>
                                <td width="75%" height="40px" align="left" colspan="3">

                                    <input type="text" id="ddlEmp" name="ddlEmp" />
                                    <%--    <asp:DropDownList ID="ddlEmp" Width="150px" runat="server" DataTextField="empName"
                                        DataValueField="empid" AutoPostBack="true" OnSelectedIndexChanged="ddlEmp_SelectedIndexChanged">
                                    </asp:DropDownList> --%> &nbsp;&nbsp;<span style="color: Red;">*</span>
                                    <%-- <span id="reqproj" style="color:Red;display:none;">Please Select Employee.</span>--%>
                                    <span id="lblerrmsgEmp" style="color: Red;"></span>
                                </td>
                            </tr>


                            <tr>
                                <td class="textcolumn" align="left">
                                    <b>Date:</b>
                                </td>
                                <td align="left">
                                    <strong>From</strong>

                                    <%--  <input id="txtFromDate" name="txtFromDate" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)" style="width: 150px" required validationmessage="Please Select Date" class="k-textbox" />
                                                  &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>--%>

                                    <input id="txtFromDate" name="txtFromDate" onkeyup="return false" style="border-radius: 0;" onkeypress="return false" />
                                    &nbsp;&nbsp;<span style="color: Red;">*</span>

                                    <%-- <input name="txtFromDate" type="text" id="txtFromDate" size="7" onkeypress="return false;" style="width:75px;">--%>
                                    <strong>To</strong>
                                    <%-- <input name="txtToDate" type="text" id="txtToDate" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)"  size="7" onkeypress="return false;" style="width:150px;" class="k-textbox">--%>
                                    <input id="txtToDate" name="txtToDate" onkeyup="return false" style="border-radius: 0;" onkeypress="return false" />
                                    <%--  <input id="txtToDate" name="txtToDate" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)" style="width: 150px" required validationmessage="Please Select Date" class="k-textbox" />--%>
                                                  &nbsp;&nbsp;<span style="color: Red;">*</span>
                                    <%--  <span id="lblerrmsgdate" style="color: Red;"></span>--%>
                                    <span id="lblDateError" style="color: Red;"></span>
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="75%" colspan="4" height="30" style="padding-left: 180px">
                                    <asp:Button Text="Search" ID="btnsearch" OnClientClick="javaScript:return SearchText();" runat="server" />
                                    <%-- <input type="submit" name="btnsearch" value="Search"  id="btnsearch" onclick="javaScript:return Search();" >--%>
                                                &nbsp;&nbsp;
                                           <%--     <input type="submit" name="btnReset" value="Reset" id="btnReset" onclick="javascript:ResetGrid();">--%>

                                    <asp:Button Text="Reset" ID="Button1" OnClientClick="javaScript:return ResetGrid();" runat="server" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="gridLateComer"></div>
                <div id="gridLateComingApproved" style="display: none;"></div>
                <div id="gridLateComingRejected" style="display: none;"></div>
                <div id="gridLateComingAdminCancel" style="display: none;"></div>
            </div>
        </div>
    </div>

    <%--<div id="divAddPopupOverlay" runat="server"></div>--%>

    <asp:HiddenField ID="hdnKEID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnLocationID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEventDate" runat="server" />
    <asp:HiddenField ID="hdnDescription" runat="server" />
    <asp:HiddenField ID="hdnTime" runat="server" />
    <asp:HiddenField ID="hdnLoginID" runat="server" Value="0" />

</asp:Content>
