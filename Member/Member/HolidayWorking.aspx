<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="HolidayWorking.aspx.cs" Inherits="Member_HolidayWorking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />   
    
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
     
        
    <script type="text/javascript" language="javascript">

        var Grid = "#gridHolidayWorking";



    </script>

    <script type="text/javascript">
        function checkLength() {
            var sender = document.getElementById('<%=txtReason.ClientID %>');
                if (sender.value.length > 1000) {
                    sender.value = sender.value.substr(0, 1000);
                }
        }

        function CheckHolidayDate() {
            
            var holidayWdate = $('#ddlHolidayDt').val();

            var newdate = holidayWdate.split("/").reverse().join("/");
            var date = new Date();
            var dt = new Date(newdate);
             var errHWDate = $("#lblerrmsgHolidayWDt");
            if (dt > date) {
               errHWDate.html("Holiday Date should not be future date");
                    return false;
            }
             else {
                    errHWDate.html("");
                }
               
            
        }
    </script>
    <script type="text/javascript" src="js/HolidayWorking.js"></script>
    <script type="text/javascript">
        
        //debugger;
        //function CheckHDate() {

        //    var dateSpan = $("#lblDateError");
        //    var offSt = $("#ddlHolidayDt").val();
        //    var CurrentDate = new Date();

        //    if (offSt != '') {//&& offEn != '') {

        //        dateFirst = offSt.split('/');
        //        dateSecond = CurrentDate.split('/');
        //        var startDate = new Date(dateFirst[2], dateFirst[1] - 1, dateFirst[0]); //Year, Month, Date
        //        var endDate = new Date(dateSecond);
        //        if (startDate > endDate) {
        //           dateSpan.html("From date should be less than or equal to Current date.");
        //        //    $('[id$="lblLeaveCount"]').text('');
        //            return false;
        //        }
        //        else {
        //            //showDays();
        //            dateSpan.html("");
        //            return true;
        //        }
        //    }
        //    debugger;
       
            function GetDataOnInsert(Buttonid) {

                var ProjectId = $("[id$=DDProjects]").val();
                var holidayWdate = $('#ddlHolidayDt').val();
                var exphours = $('#ddlHours').val();
                var reason = $("[id$=txtReason]").val();

                //HW Date
                var errHWDate = $("#lblerrmsgHolidayWDt");
                if (holidayWdate == "") {
                    errHWDate.html("Please Select Holiday Date.");
                    return false;
                }
                else {
                    errHWDate.html("");
                }
               
                //Exp Hours
                var errHours = $("#lblerrmsgHours");
                if (exphours == "") {
                    errHours.html("Please Select Expected Hours.");
                    return false;
                }
                else {
                    errHours.html("");
                }
                //Reason
                var errReason = $("#lblerrmsgReason");
                if (reason == "") {
                    errReason.html("Please Enter Reason.");
                    return false;
                }
                else {
                    errReason.html("");
                }

                if (holidayWdate != "") {
                    document.getElementById("<%=hdnHWDate.ClientID%>").value = holidayWdate;

                }
                if (exphours != "") {
                    document.getElementById("<%=hdnExpHours.ClientID%>").value = exphours;

                }
                if (reason != "") {
                    document.getElementById("<%=hdnReason.ClientID%>").value = reason;

                }
            }
            
    </script>
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

        .k-grid tbody .k-button, .k-ie8 .k-grid tbody button.k-button {
            min-width: 0px;
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

        /*#grdPayProcess {
            cursor: pointer;
        }*/
        .popup_head {
            background: url("images/table-title.png") repeat scroll left -1px rgba(0, 0, 0, 0) !important;
            margin-bottom: 0;
            border: 1px solid #ccc;
            border-top: none;
            border-bottom: none;
            padding-left: 12px;
        }

            .popup_head a {
                cursor: pointer;
            }

        .k-grid-content-locked {
            float: left;
            clear: both;
        }

        .k-grid-header-locked {
            float: left;
            clear: both;
        }

        /*div.k-grid-header .k-header {
            text-align: center;
            border-bottom: #c5c5c5 1px solid;
        }

       .k-grid-header{
            margin-bottom: -3px;
        }*/
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="temp"></div>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblHolidayWorking" Text="Holiday Working" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                <span id="spn" runat="server" onclick="ShowAddPopup();" class="small_button white_button open">Add Holiday Working Request</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="gridHolidayWorking"></div>
            </div>
        </div>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 370px; min-height: 50px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Holiday Working Request</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <td>
                        <b>Holiday Date</b>
                    </td>
                    <td>
                       <%--<input id="txtStartDate" name="txtFromDate" onkeyup="return false" style="border-radius: 0;" onkeypress="return false" />--%>
                        <input id="ddlHolidayDt" name="txtddlHolidayDt" onkeyup="return false" onchange="CheckHolidayDate()" style="border-radius: 0;width:250px;" onkeypress="return false" />
                       <%-- <input id="ddlHolidayDt" type="text" rows="5" style="width: 250px;" class="k-textbox" required validationmessage="Please Select Holiday Date."></input>
                         <asp:DropDownList ID="ddlHolidayDt1" runat="server" AutoPostBack="false" Width="250"> </asp:DropDownList>--%>
                        <%--<span id="lblDateError" style="color: Red;"></span>--%>
                        <span style="color: Red;">*</span><br />
                         <span id="lblerrmsgHolidayWDt" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Expected Hours</b>
                    </td>
                    <td>
                        <input id="ddlHours" type="text" rows="5" style="width: 250px;" class="k-textbox" required validationmessage="Please Select Expected Hours."></input>
                          <span style="color: Red;">*</span><br />
                         <span id="lblerrmsgHours" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Reason</b>
                    </td>
                    <td>
                        <textarea id="txtReason"  runat="server" style="width: 275px; height: 75px;" class="k-textbox" rows="20" cols="70"  maxlength="1000" onkeyup="checkLength();" required validationmessage="Please Enter Reason."></textarea>
                         <br /><span style="color: Red;">*</span>
                        <span id="lblerrmsgReason" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveHolidayWorkingReq" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveHolidayWorkingReq_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
            <div>&nbsp;</div>
        </div>
    </div>


    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr id="trConfig" class="manage_bg">
            </tr>
         <tr>
                    <td>
                        <b>Holiday Date</b>
                    </td>
                    <td>
                            <input id="ddlEditHolidayDt" type="text" rows="5" style="width: 300px;" class="k-textbox" readonly></input>
                    </td>
                   <span id="lblError style="color: Red;"></span>
            </tr>
         <tr>
                    <td>
                        <b>Expected Hours</b>
                    </td>
                    <td>
                         <input id="ddlEditHours" type="text" rows="5" style="width: 300px;" class="k-textbox"></input>
                       </td>
                   <span id="lblError style="color: Red;"></span>
            </tr>
         <tr>
                    <td>
                        <b>Reason</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditReason" runat="server" data-bind="value:UserReason" MaxLength="1000" TextMode="MultiLine" required validationmessage=" Please Enter Reason."
                            Height="75px" Width="275px" onkeyup="checkLength();"></asp:TextBox>
                    </td>
                   <span id="lblError style="color: Red;"></span>
            </tr>
            <tr>
                <th></th>
                <td>
                    <div id="tdUpdate"></div>
                </td>
            </tr>
        </table>

    </div>
    </script>


    <asp:HiddenField ID="hdnHolidayWorkingID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnProjectID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnHWDate" runat="server" />
    <asp:HiddenField ID="hdnExpHours" runat="server" />
    <asp:HiddenField ID="hdnReason" runat="server" />
    <asp:HiddenField ID="hdnEmpID" runat="server" />



    </asp:Content>