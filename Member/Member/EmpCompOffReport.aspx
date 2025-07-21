<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpCompOffReport.aspx.cs" Inherits="Member_EmpCompOffReport" MasterPageFile="~/Member/Admin.master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/empCompOffDetails.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
           
            $("#txtFromDate").kendoDatePicker({ format: "dd/MM/yyyy" });
            $("#txtToDate").kendoDatePicker({ format: "dd/MM/yyyy" });
           
        });
        
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
                    <asp:Label ID="lbl" Text="Comp Off Report" runat="server" Font-Bold="true" Font-Size="Medium" style="display:inline-block"></asp:Label>
                    
                        </div>
            

                    <table id="Table1" cellspacing="0" cellpadding="4" border="0" width="55%">
                        <tbody>
                            <tr>
                                <td>&nbsp;</td>
                           </tr>
                            <tr>

                                <td align="left" width="25%">
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
                            <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" width="75%" colspan="4" height="30" style="padding-left: 180px">
                                 <asp:Button class="k-button k-button-icontext k-grid-excel" Text="Export To Excel" ID="btnExport"  runat="server" OnClientClick="javaScript:return Search();return false;" OnClick="btnExport_Click"/>
                               </td>
                            </tr>
                        </tbody>
                    </table>
                    <div style="text-align: right;"> <span id="msg" style="color:red;font-size:16px"></span></div>
                   
               

                </div>
            </div>
            <%-- <div id="gridLeaves"></div> content goes here--%>
        </div>
    </div>


    <div id="divAddPopupOverlay" runat="server"></div>
   
</asp:Content>
