<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectReports.aspx.cs" Inherits="Member_ProjectReports" MasterPageFile="~/Member/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    
    <script type="text/javascript" language="javascript">

       
        var GridReport = "#grdReport";
        

        function getReportDate() {

            var date = $('[id$="txtReportDate"]').val();

            var dateSpan = $("#lblmsgDate");
            if (date == "") {
                dateSpan.html("Please select report date.");
                return false;
            }
            else {
                $('[id*="hdnReportDate"]').val(date);
                dateSpan.html("");
            }
        }

        function clearFields() {
            $('[id*="ddlProj"]').val('0');
            $('[id*="lblCustomerName"]').html('');
            $('[id*="lblEmailTo"]').html('');
            $('[id*="txtCc"]').val('');
            $('[id*="txtReportTitle"]').val('');
            $('[id*="txtReportDate"]').val('');
            $('[id*="txtDescription"]').val('');
            $('[id$="lblmsgTitle"]').html('');
            $('[id*="txtFromDate"]').val('');
            $('[id*="txtToDate"]').val('');
        }

        function checkEmail() {

            var email = $('[id$="txtCc"]').val();
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            var mailSpan = $("#lblmsgMailId");

            if (email != "") {

                var getEmail = email.split(',')

                for (var a in getEmail) {
                    var variable = getEmail[a]
                    if (!filter.test(variable)) {
                        alert('Please enter a valid email address');
                        mailSpan.html("*");
                        return false;
                    }
                    else {
                        mailSpan.html("");
                    }
                }
            }
        }

        function checkTitle() {

            var title = $('[id$="txtReportTitle"]').val();
            if (title == "") {
                $('[id*="lblmsgTitle"]').html('Please enter report title.');
                return false;
            }
            else {

                $('[id*="lblmsgTitle"]').html('');
                return true;
            }
        }

    </script>

    <script type="text/javascript" src="js/ProjectReports.js"></script>

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
                                <asp:Label ID="lblProjectsModule" Text="Project Report" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <div style="float: right;">
                                    <span>From Date</span>
                                    <input id="txtFromDate" name="txtFromDate" onkeyup="return false" style="border-radius: 0;"
                                        onkeypress="return false" />
                                    <span>To Date</span>
                                    <input id="txtToDate" name="txtToDate" onkeyup="return false" style="border-radius: 0;"
                                        onkeypress="return false" />
                                     <span id="spnSearch" runat="server" onclick="GetSearchReportDetails();" class="small_button white_button open">Search</span>
                                </div>
                            </td>
                            <td>
                                <div style="float: right;">
                                    <span id="spn" runat="server" onclick="ShowAddPopup();fileUpload();" class="small_button white_button open">Add New Report</span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="grdReport"></div>
            </div>
        </div>
    </div>

    <%--  PopUP Div Starts --%>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 600px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div>
                    <div class="popup_head">
                        <table width="100%">
                            <tr>

                                <td colspan="2" align="center">
                                    <span id="span2" style="font-size: large; font-weight: 100">Add Report</span>
                                    <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP();clearFields();ClearTempFilesandSession();" alt="Close" />
                                </td>
                            </tr>
                        </table>
                        <div class="clear">
                        </div>
                    </div>

                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">

                        <tr>
                            <th>Project:   
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlProj" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProj_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>

                            </td>
                            <th>Customer Name: 
                            </th>
                            <td>
                                <asp:Label ID="lblCustomerName" name="lblCustomerName" Style="width: 250px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <th rowspan="2">Email To: 
                            </th>
                            <td rowspan="2">
                                <asp:Label ID="lblEmailTo" name="lblEmailTo" Style="width: 250px" runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <th>Cc:  

                            </th>
                            <td>
                                <input id="txtCc" name="txtCc" style="width: 250px" runat="server" onblur="checkEmail();" />
                            </td>
                        </tr>
                        <tr>
                            <th>Report Date: 
                            </th>
                            <td>
                                <input id="txtReportDate" name="txtReportDate" onkeyup="return false" style="border-radius: 0;"
                                    onkeypress="return false" />
                            </td>
                            <span id="lblmsgDate" style="color: Red;"></span>
                        </tr>

                        <tr>
                            <th>Report Title
                            </th>
                            <td colspan="3">
                                <input id="txtReportTitle" name="txtReportTitle" class="k-textbox" onblur="return checkTitle();" style="width: 100%; border-radius: 0;" runat="server" />
                                <span id="lblmsgTitle" style="color: Red;" runat="server"></span>
                            </td>
                        </tr>

                        <tr>
                            <th colspan="1">Report Description: </th>
                            <td colspan="3">
                               <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" class="k-textbox" name="txtDescription" Style="width: 100%; border-radius: 0; height: 400px; padding: 5px 3px;"></asp:TextBox>
                           
                                  </td>
                        </tr>

                        <tr>
                            <th style="width: 20%;">Attachment
                            </th>
                            <td style="padding: 0;" colspan="4">
                                <input name="files" id="files" type="file" />
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4" style="text-align: center">
                                <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" OnClientClick="checkTitle();getReportDate();return checkFileUpload();" CssClass="small_button white_button open">
                         <span>Send</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click" CssClass="small_button white_button open" OnClientClick="closeAddPopUP();">
                         <span>Cancel</span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <div id="divOverlay"></div>

    <div id="ProjectReportDetails" class="k-widget k-windowAdd" style="display: none; padding-left: 10px; padding-right: 10px; padding-top: 12px; padding-bottom: 10px; min-width: 600px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

        <div class="popup_head">
            <h3>Project Report Details</h3>
            <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
            <div class="clear">
            </div>
        </div>

        <table cellpadding="0" cellspacing="0" border="1" width="100%" class="manage_form1">
            <tr id="trProject" class="manage_bg">
            </tr>
            <tr>
                <th>Project
                </th>
                <td>
                    <label id="lblEditProjectTitle" class="Detailslbl" />
                </td>
            </tr>
            <tr>
                <th>Report Date
                </th>
                <td>
                    <label id="lblEditReportDate" class="Detailslbl" />
                </td>
            </tr>
            <tr>
                <th>Report Title
                </th>
                <td>
                    <label id="lblEditReportTitle" class="Detailslbl" />
                </td>
            </tr>
            <tr>
                <th>Inserted Date
                </th>
                <td>
                    <label id="lblEditLastModified" class="Detailslbl" />
                </td>
            </tr>
            <tr>
                <th>Reported By
                </th>
                <td>
                    <label id="lblEditReportedBy" class="Detailslbl"></label>
                </td>
            </tr>
            <tr>
                <th>Report Description
                </th>
                <td>
                    <label id="lblEditReportDesc"></label>
                </td>
            </tr>
            <tr>
                <th>Attachments</th>
                <td>
                    <div id="attachment">
                    </div>
                </td>
            </tr>
        </table>

    </div>

    <input type="hidden" id="hdnReportId" runat="server" />
    <input type="hidden" id="hdnReportDate" runat="server" />
    <input type="hidden" id="hdnProjID" runat="server" />
    <input type="hidden" id="hdnEmpId" runat="server" />
</asp:Content>
