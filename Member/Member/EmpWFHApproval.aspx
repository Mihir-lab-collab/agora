<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="EmpWFHApproval.aspx.cs" Inherits="Member_EmpWFHApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="js/EmpWFHApproval.js" type="text/javascript"></script>
    <style type="text/css">
        .btnHide {
            display: none;
        }
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
           #loading-overlay {
      display: none;
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(0, 0, 0, 0.5);
      justify-content: center;
      align-items: center;
      z-index: 9999;
                  }

    #loading {
      border: 5px solid #f3f3f3;
            border-top: 5px solid #3498db;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
     }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        .overlay {
            position: fixed;
            background-color: rgba(0, 0, 0,0.5 );
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 9999;
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="temp"></div>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lbl" Text="Work From Home" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td class="generate_btn" style="padding-right: 1px; /*float: left*/">
                                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnShowList" class="small_button white_button open" Text="Generate Report" runat="server" OnClick="btnShowList_Click" />
                                                        <div class="k-widget k-windowAdd" id="divEmpPopup" runat="server" visible="false" style="padding-top: 10px; padding-right: 10px; min-width: 240px; min-height: 275px; top: 1%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

                                                            <div id="emp">

                                                                <div class="popup_head">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td colspan="2" align="center">
                                                                                <span id="span2" style="font-size: large; font-weight: 100">Select Employees</span>
                                                                                <asp:ImageButton ImageUrl="Images/delete_ic.png" ID="imgClose" runat="server" class="close-button" alt="Close" OnClick="imgClose_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>

                                                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%" class="manage_form">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div id="divMsg" runat="server" visible="false" style="color: red;">please select employee</div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="200px">
                                                                            <input style="width: 25px;" type="checkbox" id="chkAll" />Select All                                                                            
                                                                            <asp:CheckBoxList ID="chkEmplist" runat="server"></asp:CheckBoxList>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnOK" Width="80" class="small_button white_button open" Text="Export" runat="server" OnClick="btnOK_Click" />
                                                                            <asp:Button ID="btnClear" Width="80" class="small_button white_button open" Text="Clear" runat="server" OnClick="btnClear_Click" />
                                                                        </td>


                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnOK" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td class="generate_btn" style="padding-right: 1px;">
                                                <asp:Button ID="btnBulkAppliedWFH" class="small_button white_button open" Text="Bulk Apply WFH" runat="server" OnClick="btnBulkAppliedWFH_Click" />
                                                <div class="k-widget k-windowAdd" id="showPopUp" runat="server" visible="false" style="padding-top: 10px; padding-right: 10px; min-width: 240px; min-height: 275px; top: 1%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

                                                    <div id="employee">

                                                        <div class="popup_head">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="2" align="center">
                                                                        <span id="empSpan" style="font-size: large; font-weight: 100">Select Employees</span>
                                                                        <asp:ImageButton ImageUrl="Images/delete_ic.png" ID="imgClosePopUp" runat="server" class="close-button" alt="Close" OnClick="imgClosePopUp_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <div class="clear">
                                                            </div>
                                                        </div>

                                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%" class="manage_form">
                                                            <tr>
                                                                <%--<td colspan="2">--%>
                                                                <td>From date:
                                                                                <input id="txtFromDateWFH" onkeyup="return false" style="border-radius: 0; width: 125px" onkeypress="return false" />
                                                                    <span style="color: Red;">*</span><br />
                                                                    <span id="lblFromDateError" style="color: Red;"></span>
                                                                    <%-- <asp:TextBox ID="txtFromDateWFH" runat="server" TextMode="Date" ClientIDMode="Static"></asp:TextBox>--%>
                                                                </td>
                                                                <td>To Date:
                                                                                    <input id="txtToDateWFH" onkeyup="return false" style="border-radius: 0; width: 125px" onkeypress="return false" />
                                                                    <span style="color: Red;">*</span><br />
                                                                    <span id="lblToDateError" style="color: Red;"></span>
                                                                </td>
                                                                <td>Reason:
                                                                                    <input type="text" runat="server" id="txtReasonForWFH" style="width: 125px" />
                                                                    <span style="color: Red;">*</span><br />
                                                                    <span id="lblReasonError" style="color: Red;"></span>
                                                                </td>
                                                                <td>Search:
                                                                                    <input type="text" runat="server" id="txtSearch" style="width: 125px" />
                                                                    <asp:Button runat="server" class="small_button white_button open" OnClick="btnBulkAppliedWFH_Click" ID="btnSearch" Text="Search" />
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td width="200px">
                                                                    <input style="width: 25px;" onclick="CheckAllEmployee()" type="checkbox" id="chkAllEmployee" />Select All                                                                            
                                                                            <asp:CheckBoxList ID="cblEmployeeWFH" runat="server"></asp:CheckBoxList>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnBulkWFHOk" Width="80" class="small_button white_button open" Text="Submit" runat="server" OnClick="btnBulkWFHOk_Click" OnClientClick="if(CheckDate()==true){javascript:return true;}else{return false;}" />
                                                                    <asp:Button ID="btnBulkWFHClear" Width="80" class="small_button white_button open" Text="Clear" runat="server" OnClick="btnBulkWFHClear_Click" />
                                                                </td>


                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <%--<td style="/*display: block; */">
                                                <input style="width: 25px;" type="checkbox" id="chkInclude" onclick="Search();" />Include Archive&nbsp;&nbsp;
                                            </td>--%>
                                            <td style="/*float: left*/">Employee Name:
                                                <input type="text" id="txtName" style="width: 130px" />
                                            </td>
                                            <td>From date:
                                                <input type="text" id="txtFromDate" onkeypress="return false;" style="width: 125px" />
                                            </td>
                                            <td>To Date:
                                                <input type="text" id="txtFromTo" onkeypress="return false;" style="width: 125px" />
                                            </td>
                                            <td>
                                                <label>Status: </label>
                                                <asp:DropDownList ID="ddlStatus" AppendDataBoundItems="true" runat="server" Width="100">
                                                    <asp:ListItem Value="0" Text="All" Selected="True" />
                                                    <asp:ListItem Value="a" Text="Approved" />
                                                    <asp:ListItem Value="r" Text="Rejected" />
                                                    <asp:ListItem Value="p" Text="Pending" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span id="spn" runat="server" onclick="Search();" class="small_button white_button">Search</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="gridWFH"></div>
            </div>
        </div>
    </div>
    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 740px; min-height: 50px; top: 1%; left: 350px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">
        <div id="loading-overlay">
            <div id="loading">
            </div>
        </div>
        <div id="Invoice_Form">

            <div class="popup_head">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <span id="spanWFHDetails" style="font-size: large; font-weight: 100">Work From Home Details</span>
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
                    <th>Work From Home Status:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlWFHStatus" AppendDataBoundItems="true" runat="server" Width="100">
                            <asp:ListItem Value="a" Text="Approved" Selected="True" />
                            <asp:ListItem Value="r" Text="Rejected" />
                            <asp:ListItem Value="p" Text="Pending" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align: middle;">Employee ID: 
                    </th>
                    <td>
                        <label id="lblEmpID" title="" style="width: 250px" runat="server"></label>
                    </td>
                    <th>Admin Comments: 
                    </th>
                    <td>
                        <textarea id="txtAdminComment" runat="server" class="k-textbox" style="width: 100%; height: 80px; float: left;"></textarea>
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align: middle;">WFH From:
                        <br />
                        <br />
                        <br />
                        WFH To: 
                    </th>
                    <td>
                        <input type="text" id="txtFDate" onkeypress="return false;" style="width: 140px" />
                        <br />
                        <br />
                        <input type="text" id="txtTDate" onkeypress="return false;" style="width: 140px" />
                    </td>
                    <th style="vertical-align: middle;">Attendance Time In: 
                         <br />
                        <br />
                        Attendance Time Out:
                    </th>
                    <td>
                        <label id="lblAttendanceIn" title="" style="width: auto" runat="server"></label>
                        <br />
                        <br />
                        <label id="lblAttendanceOut" title="" style="width: auto" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <th>Reason of Work From Home:
                    </th>
                    <td>
                        <textarea id="txtReason" runat="server" class="k-textbox" disabled="disabled" style="width: 100%; height: 80px; float: left;"></textarea>
                    </td>
                    <th style="vertical-align: middle;">WFH Sanctioned By:
                    </th>
                    <td>
                        <label id="lblSanctionedBy" title="" style="width: auto" runat="server"></label>

                    </td>
                </tr>
                <tr>
                    <th>WFH Applied On:  
                    </th>
                    <td>
                        <label id="lblWFHEntry" title="" style="width: auto" runat="server"></label>
                    </td>
                    <th>WFH Sanctioned Date:
                    </th>
                    <td>
                        <label id="lblWHFSanctionedDate" title="" style="width: auto" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div style="padding-left: 306px;">
                            <input type="button" id="btnSaveStatus" onclick="SaveStatus()" value="Submit" style="width: 100px; height: 30px;" />
                        </div>
                    </td>
                </tr>

            </table>
        </div>
    </div>
      <div id="divOverlay" class="overlay"></div>
    <asp:HiddenField ID="hdnEmpID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnLocationID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmpWFHID" runat="server" />
    <asp:HiddenField ID="hdnLoginID" runat="server" />
    <asp:HiddenField ID="hdnEmpName" runat="server" />
    <asp:HiddenField ID="hdnHrProfileStatus" runat="server" />
    <asp:HiddenField ID="hdnTime" runat="server" />
    <asp:HiddenField ID="hdnFromDateWFH" runat="server" />
    <asp:HiddenField ID="hdnToDateWFH" runat="server" />
</asp:Content>
