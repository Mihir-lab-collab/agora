<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="CIP.aspx.cs" Inherits="Member_Holiday" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/CIP.js" type="text/javascript"></script>

    <script type="text/javascript">

        function call()
        {
            alert("Saved Successfully");
        }
        function GetDataOnInsert(Buttonid)
        {
            var EventDate = $('#txtEventDate').val();
            var narration = $('#txtNarration').val();
            var Time = $('#timepicker').val(); 

         var errHDate = $("#lblerrmsgdate");
         if (EventDate == "")
         {
                errHDate.html("Please Select Event Date.");
                return false;
            }
         else
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
                    <asp:Label ID="lblHoliday" Text="CIP Sessions (Continuous Improvement Program)" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                     <asp:Label ID="lblLocation" Text="" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td style="padding-right:400px;"><input id="chkOLD" type="checkbox" onchange="BindOldCIP();" /><label><b>Past Sessions</b></label></td>
                                            <td>                                                
                                                <span id="spn" runat="server" onclick="ShowAddPopup();" class="small_button white_button open">Add New</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="gridEvents"></div>
            </div>
        </div>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 600px;    border-color: black; border-width: thin; min-height: 400px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Events</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
              
                <tr>
                    <th>Event Date:</th>
                    <td  align="center">
                        <input id="txtEventDate" name="txtEventDate" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)" style="width: 200px;" required validationmessage="Please Select Date" class="k-textbox" />
                         &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                         <span id="lblerrmsgdate" style="color: Red;"></span>
                        <b>Time :</b><input class="k-header" id="timepicker" value="11:00 AM" onkeydown="return false;" style="width: 100px; left:5px;" />
                    </td>
                </tr>
                <tr>
                    <th>Description:</th>
                    <td align="center">
                        <%--<input id="txtNarration" type="text" style="width: 450px;" onkeypress="return isChar(event,this);"  required validationmessage="Please Enter Narration" class="k-textbox" />--%>
                        <textarea id="txtNarration" style="width: 450px; height:300px" required validationmessage="Please Enter Description" class="k-textbox" ></textarea>
                         <span style="color: Red;vertical-align: top;">*</span>
                        <span id="lblerrmsgnarration" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveEvents" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveEvents_Click" />
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
                <th>Holiday Date
                </th>
                <td>
            <input id="txtEditHolidayDate" name="txtEditHolidayDate" data-bind="value:HolidayDate" onkeydown="return false;" onkeyup="dateInput(this)" onkeypress="dateInput(this)" style="width: 300px" class="k-textbox" required validationmessage="Date cannot be blank." />


                </td>
            </tr>
            <tr>
                <td>
                   <span id="lblError style="color: Red;"></span>

                </td>
            </tr>

            <tr>
                <th> Narration
                </th>
                <td>
                    <input id="txtEditNarration" type="text" style="width: 300px;" class="k-textbox" required validationmessage="Narration cannot be blank." />

                </td>
            </tr>
            <tr>
                <td>
                   <span id="lblError style="color: Red;"></span>

                </td>
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


    <asp:HiddenField ID="hdnKEID" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnLocationID" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnEventDate" runat="server" />
    <asp:HiddenField ID="hdnDescription" runat="server" />
    <asp:HiddenField ID="hdnTime" runat="server" />
</asp:Content>




