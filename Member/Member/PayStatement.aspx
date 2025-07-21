<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="PayStatement.aspx.cs" Inherits="Member_PayStatement" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <%--Bellow links are for ng-grid --%>

    <link type="text/css" href="css/InvoiceValidation.css" rel="stylesheet" />
    <link href="css/ng-grid.css" rel="stylesheet" />


    <%--Bellow links are for kendo controls (do not change sequence)--%>

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script type="text/javascript" src="js/jquery.datetimepicker.js"></script>
    <script language="JavaScript" src="../Includes/CalendarControl.js" type="text/javascript">
    </script>
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />

    <link href="css/jquery.datetimepicker.css" rel="stylesheet" />

    <script src="../Member/js/PayStatement.js" type="text/javascript"></script>


    <style type="text/css">
        .k-grid tbody .k-button, .k-ie8 .k-grid tbody button.k-button {
            min-width: 40px;
            min-height: 30px;
        }

        .ViewPDF, .ViewPDF:hover {
            background-image: url('../images/icon-pdf.gif');
            min-width: 10px;
            width: 20px;
            height: 30px;
            background-size: 20px;
            background-repeat: no-repeat;
        }

        .mail, .mail:hover {
            background-image: url('images/send.png');
            min-width: 10px;
            width: 20px;
            height: 30px;
            background-size: 20px;
            background-repeat: no-repeat;
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
        /*for history grid [e]*/


        /*ng-grid*/
        .gridStyle {
            border: 1px solid rgb(212,212,212);
            /*width: 770px;*/
            width: 900px;
            height: 150px;
        }

        .buttondel {
            background-image: url('images/delete.png');
        }

        .buttonadd {
            background: url('images/addbtn_small.jpg') center center no-repeat;
        }

        /*input.ng-dirty.ng-invalid {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }*/

        /*input.ng-invalid-required {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }*/

        input.ng-invalid-pattern {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }
        /*added by Dipti to wrap column header*/
        .ngHeaderText {
            text-overflow: clip;
            white-space: normal;
        }
        /*end added by Dipti to wrap column header*/
        /*end added by Dipti to wrap table column */
        .thclass {
            width: 100px;
        }

        .tdclass {
            width: 180px;
        }

        .manage_form th {
            text-align: right;
        }
        /*end added by Dipti to wrap table column */
    </style>

    <%--  div confirm style --%>
    <style>
        .popup {
            background: none repeat scroll 0 0 #fff;
            box-shadow: 0 0 5px #3d3d4f;
            position: fixed;
            z-index: 9999;
            display: none;
            width: 550px;
            top: 20%;
            left: 50%;
            margin-left: -185px;
            box-shadow: 0 7px 20px rgba(0, 0, 0, 0.45);
            font-family: Arial, Helvetica, sans-serif;
        }

        .overlay {
            position: fixed;
            background-color: rgba(0, 0, 0,0.5 );
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 8888;
            display: none;
        }

        .popupCont {
            background-color: #f0f0f0;
            border-top: 1px solid #CCC;
        }

        .popup_head {
            padding: 0 15px;
        }

            .popup_head h2 {
                font-size: 40px;
                padding: 30px 15px;
            }

        .close {
            background: #2b2d2d;
            color: #fff;
            display: block;
            float: right;
            padding: 9px 12px;
            text-align: center;
        }

        .loginfoot {
            border-top: 1px solid #ccc; /*padding:20px 16px;*/
            text-align: center;
            margin-top: 35px;
            padding: 25px 0;
            background: #fff;
            position: relative;
        }

        .or {
            margin-left: -24.5px;
            left: 50%;
            position: absolute;
            top: -24.5px;
        }

        .popupCont .row {
            overflow: visible;
        }

            .popupCont .row .col1 {
                float: none;
            }

        .border {
            border-top: 1px solid #ccc;
            padding: 10px 0;
            background: #fff;
            margin: 0 !important;
        }

        .popupCont .gray {
            padding: 12px 44px;
            behavior: url(../pie/PIE.htc);
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -moz-border-radius: 5px;
            position: relative;
        }

            .popupCont .gray:hover {
                background: #7f7d7d;
            }



        /*-- ========================== subscription popup 
===============================================-- */
        /*model_popup*/
        .model_popup {
            background: #fff;
            border-radius: 5px 5px 0 0;
            overflow: hidden;
            border-radius: 5px 5px 0 0;
            position: relative;
            behavior: url(../pie/PIE.htc);
            -webkit-border-radius: 5px 5px 0 0;
            -moz-border-radius: 5px 5px 0 0;
            margin-bottom: 5%;
            left: 43%;
            position: absolute;
        }

        .sub_popup .left_panel {
            min-height: 630px;
        }

        .pricing th, .subp_cont th {
            position: relative;
        }

        .subp_cont .price {
            margin-top: 0;
        }

        .closePopup_btn {
            color: #fff;
            text-decoration: none;
            display: block;
            float: right;
            font-family: Arial, Helvetica, sans-serif;
            position: absolute;
            right: 0;
            top: 0;
            padding: 5px 10px;
            background: rgba(0,0,0,0.5);
        }

        /*-------------------------------start css here
------------------------------------------*/
        .box_SC {
            border-top: 3px solid #e69503;
            font-weight: 600;
            padding: 22px;
            margin: 30px 0 0 0;
            overflow: hidden;
            text-align: center;
        }

        .window_content {
            padding: 55px 30px 20px;
            font-weight: 300;
            font-size: 1.2em;
        }

        .remind_later {
            padding: 20px 30px;
            border-top: #d4d4d4 1px solid;
            text-align: right;
        }

        .selectS1 {
            height: 30px;
            line-height: 30px;
            padding: 5px;
            width: 100px;
        }

        .btn.close {
            color: whitesmoke;
            background: #252e34;
            text-align: center;
            width: 100%;
            transition: all .3s ease;
            font-size: 24px;
            font-weight: 500;
            padding: 10px 0px;
            border-radius: 3px;
            cursor: pointer;
            transition: all .3s ease;
            z-index: 5;
            margin-bottom: 3px;
        }

            .btn.close:hover {
                margin-top: 3px;
                margin-bottom: 0;
            }

            .btn.close:active {
                margin-top: 8px;
                margin-bottom: 0;
            }

        .submit_btn {
            background: #2f3840 !important;
            box-shadow: 0 8px 0 0 #000000;
        }

        .cancel_btn {
            background: #2f3840 !important;
            box-shadow: 0 8px 0 0 #000000;
        }

        .submit_btn:hover {
            box-shadow: 0 5px 0 0 #518324;
            background: #252e34 !important;
        }

        .cancel_btn:hover {
            box-shadow: 0 5px 0 0 #cb3939;
            background: #252e34 !important;
        }

        .col2 {
            padding: 15px 20px;
            width: 42%;
            float: left;
        }
    </style>
    <%-- div confirm style end --%>


    <%-- div confirm style end --%>
    <style>
        table tr td {
            padding: 1px 5px;
        }

        #table1 tr td {
            font-family: Arial;
            font-size: 14px;
        }

        input[type="radio"], input[type="checkbox"] {
            margin: 0 auto;
            float: none;
            display: block;
        }

        .white_button {
            border-radius: 4px;
        }

        .white_button {
            height: 30px;
            line-height: 28px;
        }
    </style>



    <script type="text/javascript">
        $(document).ready(function () {
            $("[id$=txtcalender]").click(function () {
                popupCalender('txtcalender');
            });

            $('[id$="txtEmail"]').kendoEditor();
        });
        function CheckSelectValidation() {
            if ($('input:checkbox[id^="chkSelect"]:checked').length < 1) {
                alert("Please select at least one Employee.");
                return false;
            }
        }
        function printdiv(printpage) {
            var firstDivContent = document.getElementById('mydiv1');
            $("[id$=chequeno],[id$=txtcalender]").replaceWith(function () {
                return "<label>" + $(this).val() + "</label>";
            });
            var headstr = "<html><head><title></title></head><body>";
            var footstr = "</body>";
            var newstr = $("[id$=divInBankStatement]").html();
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr + newstr + footstr;
            window.print();
            document.body.innerHTML = oldstr;
            return false;
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divPayStmtDetail" style="padding: 20px;" runat="server">
        <table height="0" width="90%" border="0" style="border-collapse: collapse"
            bordercolor="#111111" align="center">
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>
                    <table border="0" width="100%">
                        <tr>
                            <td align="center" style="font-size: 16px; padding-bottom: 15px;">
                                <b>Bank Statment</b></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr id="rowhide" runat="server">
                            <td align="center">
                                <font face="Verdana"></font>
                            </td>
                        </tr>
                        <tr>
                            <td width="25%" colspan="4" style="padding-bottom: 10px;">
                                <font face="Verdana" size="2"><b>SALARY SLIP FOR THE MONTH :
                                    <asp:Label ID="lbtnpre" runat="server"></asp:Label>

                                    <%=Date%>
                                   
                                </b></td>
                        </tr>
                    </table>
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgrdCLetter" runat="server" BorderColor="Black" Font-Size="10pt"
                                Font-Name="Verdana" BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"
                                FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10pt" ShowFooter="True"
                                OnItemDataBound="dgrdCLetter_ItemDataBound" Width="90%" AllowSorting="True">
                                <%--   <ItemStyle VerticalAlign="Top" ></ItemStyle>--%>
                                <HeaderStyle Font-Bold="True" Width="70px" Height="30px"></HeaderStyle>
                                <FooterStyle></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Select" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <input id="chkSelect" type="checkbox" value='<%# Eval("EmpID")%>' name="chkSelect" checked="checked" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Sr." HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <%# Container.ItemIndex+1%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn ItemStyle-Wrap="true">
                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Wrap="true"></ItemStyle>
                                        <HeaderTemplate>
                                            Employee Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txtempname" readonly="true" runat="server" Width="90px" Text='<%# Eval("EmpName")%>'
                                                textmode="singleline"> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="Empolyee Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        DataField="EmpID">
                                        <HeaderStyle Width="12%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Account Number" DataField="AccountNo" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                     <asp:BoundColumn HeaderText="IFSC Code" DataField="IFSCCode" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Net Salary(Rs.)">
                                        <ItemStyle Width="17%" HorizontalAlign="Right"></ItemStyle>
                                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                        <ItemTemplate>
                                            <input id="lblTotal" style="text-align: right; width: 72px; border-top-style: none; border-right-style: none; border-left-style: none; background-color: #FFFFFF; border-bottom-style: none"
                                                readonly type="text" size="15" value='<%# CSCode.Global.GetCurrencyFormat(Convert.ToDouble(Eval("Net"))) %>'
                                                name="lblTotal">
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <input name="text" type="text" id="lblGrandTotal" style="text-align: right; font-weight: bold; width: 78px; border-top-style: none; border-right-style: none; border-left-style: none; height: 18px; background-color: #FFFFFF; border-bottom-style: none"
                                                size="10"
                                                readonly>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>

                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                            <asp:Button ID="btnSubmit" Width="85px" runat="server" CssClass="small_button white_button open" Text="Proceed" OnClick="btnSubmit_Click" OnClientClick="return CheckSelectValidation();"></asp:Button>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
            </tr>
            <asp:HiddenField ID="hdnMonth" runat="server" />
            <asp:HiddenField ID="hdnYear" runat="server" />
            <asp:HiddenField ID="hdnLocId" runat="server" />
            <asp:HiddenField ID="hdnEmpIds" runat="server" />
            <asp:HiddenField ID="hdnSumValue" runat="server" />
            <asp:HiddenField ID="hdnCheckno" runat="server" />
            <asp:HiddenField ID="hdntxtDate" runat="server" />
        </table>
    </div>

    <div id="divBankStatement" runat="server" style="display: none; padding: 10px; width: 64%; margin: 10px auto; padding: 15px; background-color: #fff; border: 1px solid #b9b9b9;">
        <div id="divInBankStatement" runat="server">
        </div>
        <div id="divbtnClick" style="text-align: center">
            <input type="button" name="btnPtint" onclick="printdiv()" value="Print" class="small_button white_button open" />
            &nbsp;&nbsp;&nbsp;
          <asp:Button ID="btnExport" Width="150px" runat="server" CssClass="small_button white_button open" Text="Export to Excel" OnClick="btnExport_Click"
              OnClientClick="$('[id$=hdnCheckno]').val($('[id$=chequeno]').val());$('[id$=hdntxtDate]').val($('[id$=txtcalender]').val());"></asp:Button>
            <%-- <asp:Button ID="btnSendMail" Width="150px" runat="server" OnClick="btnSendMail_Click" CssClass="small_button white_button open" Text="Send Mail"  
              OnClientClick="BindMail();"></asp:Button>--%>
            <input type="button" name="btnSendMail" onclick="BindMail();" value="Send Mail" class="small_button white_button open" />
        </div>
    </div>

    <div id="divMail" class="k-widget k-windowAdd" style="display: none; padding: 10px 10px 20px; min-width: 300px; min-height: 50px; top: 50%; left: 486px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">
        <div class="popup_head">
            <table width="100%">
                <tr>
                    <td colspan="2" align="center">
                        <span id="span2" style="font-size: large; font-weight: 100">Bank Statement Mail</span>
                        <img src="Images/delete_ic.png" class="close-button" alt="Close" onclick="CloseMailBox()" />
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
        </div>
        <table id="tblInvMail1" align="center" height="100" border="0" cellpadding="3"
            cellspacing="3" runat="server">
            <tr>
                <td>To:
                </td>
                <td>
                    <%--<asp:Label ID="lblTo" runat="server" Text=""></asp:Label>--%>
                    <asp:TextBox ID="txtTo" size="60" runat="server"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td>Cc:
                </td>
                <td>
                    <asp:TextBox ID="txtCc" size="60" runat="server" Text="accounts@intelgain.com"></asp:TextBox>
                </td>

                <td style="visibility: visible">Attached Documents:
                    <div id="dvAttachnemt" runat="server"></div>

                </td>
            </tr>
            <tr>
                <td>Bcc:
                </td>
                <td>
                    <asp:TextBox ID="txtBcc" size="60" runat="server" Text=""></asp:TextBox>

                </td>
                <td></td>
            </tr>
            <tr>
                <td>Subject:
                </td>
                <td>
                    <asp:TextBox ID="txtSubject" size="60" runat="server"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td valign="top" colspan="3">
                    <%--<asp:TextBox ID="txtEmail" runat="server" Rows="15" Width="400" TextMode="MultiLine"></asp:TextBox>--%>
                    <textarea id="txtEmail" runat="server" style="width: 515px; height: 250px;" rows="15" cols="70" class="k-textbox"></textarea>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="custbutton" colspan="2">
                    <center>
                        <asp:Button ID="btnSendReceipt" runat="server" CssClass="small_button white_button open" Text="Send Receipt" OnClientClick=" return sendmail();" />
                    </center>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>

