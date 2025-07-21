<%@ Page Language="C#" AutoEventWireup="true" CodeFile="empHolidayWorkDetails.aspx.cs" Inherits="Member_empHolidayWorkDetails" MasterPageFile="~/Member/Admin.master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/empHolidayWorkDetails.js" type="text/javascript"></script>

    <script type="text/javascript">
        function SearchText() {
            Search();
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
                    <div style="overflow:hidden;padding:10PX 0 15PX">
                    <asp:Label ID="lbl" Text="Holiday Working" runat="server" Font-Bold="true" Font-Size="Medium" style="display:inline-block"></asp:Label>
                    <div style="float:right;">

                        <div id="ddlStatus" align="right">
                            <select name="Status">
                                <option value="0">Pending </option>
                                <option value="1">Approved </option>
                                <option value="2">Rejected </option>
                                <option value="3">Admin Cancel </option>
                            </select>
                        </div>

                    </div>
                        </div>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
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
                    <div style="text-align: right;"> <span id="msg" style="color:red;font-size:16px"></span></div>
                   
                    <div class="grid_head">
                        <div id="gridHolidayWokingPending"></div>
                        <div id="gridHolidayWokingApproved" style="display: none;"></div>
                        <div id="gridHolidayWokingRejected" style="display: none;"></div>
                        <div id="gridHolidayWokingAdminCalcel" style="display: none;"></div>
                    </div>


                </div>
            </div>
            <%-- <div id="gridLeaves"></div> content goes here--%>
        </div>
    </div>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px;height: 294px;width: 442px;top: 1%; left: 350px; z-index: 10003; opacity: 1; transform: scale(1); border: solid; margin-top: 235px;margin-left: 259px;">

        <div id="Invoice_Form">

            <div class="popup_head">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <span id="span2" style="font-size: large; font-weight: 100">Comp Off Details</span>
                            <img src="Images/delete_ic.png" class="close-button" alt="Close" onclick="ClosePopUp()" />
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th style="vertical-align: middle;">Employee Name:   
                    </th>
                    <td width="200px">
                        <label id="lblName" title="" style="width: 250px" runat="server"></label>
                    </td>

                </tr>
                <tr>
                    <th style="vertical-align: middle;">CompOff Date: 
                    </th>
                    <td>
                        <label id="lblCompOffDate" title="" style="width: 250px" runat="server"></label>
                    </td>

                </tr>
                <tr>
                    <th style="vertical-align: middle;">Project:
                       
                    </th>
                    <td>
                        <label id="lblProjects" title="" style="width: 250px" runat="server"></label>

                    </td>

                </tr>
                <tr>
                    <th>CompOff Comment:
                    </th>
                    <td>
                        <textarea id="txtCompOffComment" runat="server" class="k-textbox" style="width: 100%; height: 80px; float: left;"></textarea>
                    </td>

                </tr>

                <tr>
                    <td colspan="4">
                        <div style="padding-left: 306px;">
                            <input type="button" id="btnSaveCompOff" onclick="SaveCompOff()" value="Submit" style="width: 100px; height: 30px;" />
                        </div>
                    </td>
                </tr>

            </table>
        </div>
    </div>

    <asp:HiddenField ID="hdnEmpID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnStaus" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSelectEmp" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLocationID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCompOFfEmpID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnholidayDate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnProjectName" runat="server" ClientIDMode="Static" />
</asp:Content>
