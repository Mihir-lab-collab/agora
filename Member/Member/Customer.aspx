<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Customer_Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>

    <script src="../Member/js/Customer.js" type="text/javascript"></script>

    <script type="text/javascript">
        function checkemail(ID) {
            //alert(ID);
            //inputEmail = $('[id$="txtEmail"]').val();

            if ($('.red').length != 0) {
                $('.red').html('');
            }
            inputEmail = $('[id$="' + ID + '"]').val();
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            if (ID == 'txtEmailCC') {
                if (inputEmail == "")
                    return true;
            }

            var getEmail = inputEmail.split(',')

            for (var a in getEmail) {
                var variable = getEmail[a]
                if (!filter.test(variable)) {
                     $('[id$="' + ID + '"]').after('<div class="red" style="color:red">Please enter Customer Name</div>');
                   // alert('Please enter a valid email address');
                    //mailSpan.html("*");
                    //$('[id*="txtEmail"]').focus();
                    return false;
                }
                //else {
                //    mailSpan.html("");
                //}
            }
        }
    </script>

    <style>
        .k-textbox {
            width: 11.8em;
        }

        #tickets {
        }

            #tickets h3 {
                font-weight: normal;
                font-size: 1.4em;
                border-bottom: 1px solid #ccc;
            }

            #tickets ul {
                list-style-type: none;
                margin: 0;
                padding: 0;
            }

            #tickets li {
                margin: 10px 0 0 0;
            }

        label {
            display: inline-block;
            width: 90px;
            text-align: right;
        }

        .required {
            font-weight: bold;
        }

        .accept, .status {
            padding-left: 90px;
        }

        .valid {
            color: green;
        }

        .invalid {
            color: red;
        }

        span.k-tooltip {
            margin-left: 6px;
        }

        .note.error span {
            background: transparent url(../images/error.png) 0px 0px no-repeat;
        }

        .note.check span {
            background: transparent url(../images/check.png) 0px 0px no-repeat;
        }
    </style>
    <style type="text/css">
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
    </style>
    <script type="text/javascript">

        function CheckInsert() {
            if ($('.red').length != 0) {
                $('.red').html('');
            }

            if ($('[id$="txtCustomerName"]').val() == "") {
                $('[id$="txtCustomerName"]').after('<div class="red" style="color:red">Please enter Customer Name</div>');
                return false;
            }
            else if ($('[id$="txtCustomerCompany"]').val() == "") {
                $('[id$="txtCustomerCompany"]').after('<div class="red" style="color:red">Please enter Company Name</div>');
                return false;
            }
            else if ($('[id$="txtEmail"]').val() == "") {
                $('[id$="txtEmail"]').after('<div class="red" style="color:red">Please enter Email</div>');
                return false;
            }
            else if ($('[id$="txtEmail"]').val() != "") {
                inputEmail = $('[id$="txtEmail"]').val();
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                var getEmail = inputEmail.split(',')
                for (var a in getEmail) {
                    var variable = getEmail[a]
                    if (!filter.test(variable)) {
                        $('[id$="txtEmail"]').after('<div class="red" style="color:red">Please enter a valid email address</div>');
                        // alert('Please enter a valid email address');
                        //mailSpan.html("*");
                        //$('[id*="txtEmail"]').focus();
                        return false;
                    }
                    else if ($('[id$="txtEmailCC"]').val() != "") {
                        inputEmail = $('[id$="txtEmailCC"]').val();
                        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                        var getEmail = inputEmail.split(',')
                        for (var a in getEmail) {
                            var variable = getEmail[a]
                            if (!filter.test(variable)) {
                                $('[id$="txtEmailCC"]').after('<div class="red" style="color:red">Please enter a valid email address</div>');
                                // alert('Please enter a valid email address');
                                //mailSpan.html("*");
                                //$('[id*="txtEmail"]').focus();
                                return false;
                            }
                            //Added else Condition by Nikhil Shetye for Setting hidden field & check India country
                            else {
                                if ($('[id$="ddlCountry"]').val() == "") {
                                    $('[id$="ddlCountry"]').after('<div class="red" style="color:red">Please enter Country</div>');
                                    return false;
                                }
                                else if ($('[id$="ddlState"]').val() == "" && $('[id$="ddlCountry"]').val() == "India") {
                                    $('[id$="ddlState"]').after('<div class="red" style="color:red">Please enter State</div>');
                                    return false;
                                }

                                else if ($('[id$="txtAddress"]').val() == "") {
                                    $('[id$="txtAddress"]').after('<div class="red" style="color:red">Please enter Address</div>');
                                    return false;
                                }
                                else {
                                    //alert('Hi');
                                    document.getElementById("<%=hftxtCustomerName.ClientID%>").value = $("#txtCustomerName").val();
                            document.getElementById("<%=hftxtCustomerCompany.ClientID%>").value = $("#txtCustomerCompany").val();
                            document.getElementById("<%=hftxtAddress.ClientID%>").value = $("#txtAddress").val();
                            document.getElementById("<%=hftxtNotes.ClientID%>").value = $("#txtNotes").val();
                            document.getElementById("<%=hftxtCustEmail.ClientID%>").value = $("#txtEmail").val();
                            document.getElementById("<%=hftxtCustEmailCC.ClientID%>").value = $("#txtEmailCC").val();
                            document.getElementById("<%=hfIsShowAllTask.ClientID%>").value = $("#chkShowAllTask").is(':checked');

                            document.getElementById("<%=hfddlCountryId.ClientID%>").value = $("#ddlCountry").val();
                            document.getElementById("<%=hfddlStateId.ClientID%>").value = $("#ddlState").val();
                            document.getElementById("<%=hfddlCityId.ClientID%>").value = $("#ddlCity").val();
                            document.getElementById("<%=hftxtGSTIN.ClientID%>").value = $("#txtGSTIN").val();
                            return true;
                        }
                            }
                        }
                    }
                    else {
                        if ($('[id$="ddlCountry"]').val() == "") {
                            $('[id$="ddlCountry"]').after('<div class="red" style="color:red">Please enter Country</div>');
                            return false;
                        }
                        else if ($('[id$="ddlState"]').val() == "" && $('[id$="ddlCountry"]').val()=="India") {  //check India country added by Nikhil Shetye
                            $('[id$="ddlState"]').after('<div class="red" style="color:red">Please enter State</div>');
                            return false;
                        }

                        else if ($('[id$="txtAddress"]').val() == "") {
                            $('[id$="txtAddress"]').after('<div class="red" style="color:red">Please enter Address</div>');
                            return false;
                        }
                        else {
                            //alert('Hi');
                            document.getElementById("<%=hftxtCustomerName.ClientID%>").value = $("#txtCustomerName").val();
                            document.getElementById("<%=hftxtCustomerCompany.ClientID%>").value = $("#txtCustomerCompany").val();
                            document.getElementById("<%=hftxtAddress.ClientID%>").value = $("#txtAddress").val();
                            document.getElementById("<%=hftxtNotes.ClientID%>").value = $("#txtNotes").val();
                            document.getElementById("<%=hftxtCustEmail.ClientID%>").value = $("#txtEmail").val();
                            document.getElementById("<%=hftxtCustEmailCC.ClientID%>").value = $("#txtEmailCC").val();
                            document.getElementById("<%=hfIsShowAllTask.ClientID%>").value = $("#chkShowAllTask").is(':checked');

                            document.getElementById("<%=hfddlCountryId.ClientID%>").value = $("#ddlCountry").val();
                            document.getElementById("<%=hfddlStateId.ClientID%>").value = $("#ddlState").val();
                            document.getElementById("<%=hfddlCityId.ClientID%>").value = $("#ddlCity").val();
                            document.getElementById("<%=hftxtGSTIN.ClientID%>").value = $("#txtGSTIN").val();
                            return true;
                        }
                    }
                        }
                    }

                }


      

    </script>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblCusomerModule" Text="Manage Customer" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>

                                <div style="float: right;">
                                    <span id="spncustomer" onclick="ShowAddPopup()" class="small_button white_button open">Add New CUstomer</span>
                                    <span id="Spnpage1" style="font-size: small;">Show </span>
                                    <input id="comboBox" /><span id="Spnpage2" style="font-size: small;"> Records per Page</span>

                                </div>

                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridCustomer"></div>
            </div>
        </div>
    </div>
    <script id="popup-editor" type="text/x-kendo-template">
    <table width="100%" cellpadding="10" cellspacing="0" class="manage_form">
        <tr>
            <td colspan="2" align="center">
                <span id="Span2" style="font-size: large; font-weight: lighter">Company Details</span>
            </td>
        </tr>
         <tr>
            <th>Name</th>
            <td>
                <input id="txtEditCustomerName" type="text" name="txtEditCustomerName" data-bind="value:custName" style="width: 300px" required validationmessage="Please enter Customer Name" class="k-textbox" />
            </td>
        </tr>
        <tr>
            <th>Company</th>
            <td>
           <%-- <span id="spncustid">#=custId#</span> --%>        
             <input id="txtEditCustomerCompany" type="text" name="txtEditCustomerCompany" data-bind="value:custCompany" onblur="return getCompanyExistsbycustid(this.id,this.value,);" style="width: 300px" required validationmessage="Please enter company Name" class="k-textbox" /><span id="spnEditcomapnyexist" style="color: red; display: none"><b>Company already exists</b></span>
            </td>
        </tr>

          <tr>
              <th>GSTIN</th>
              <td>
              <input id="txtEditGSTIN" type="text" name="txtEditGSTIN" data-bind="value:GSTIN" style="width: 300px" class="k-textbox" />
              </td>
         </tr>

         <tr>
            <th>Email</th>
            <td>
             <textarea id="txtEditEmail" rows="2" cols="40" name="txtEditEmail" data-bind="value:custEmail" style="width: 300px" required validationmessage="Please enter Email" class="k-textbox" />
            </td>
        </tr>
        <tr>
            <th>Email CC</th>
            <td>
             <textarea id="txtEditEmailCC" rows="2" cols="40" name="txtEditEmailCC" data-bind="value:custEmailCC" style="width: 300px" class="k-textbox" />
            </td>
        </tr>
          <tr>
                                            <th>Country</th>
                                            <td>
                                                <input id="ddlEditCountry" name="ddlEditCountry" style="width: 300px" data-bind="value:CountryName" required validationmessage="<br>Please Select Country"/>
                                            </td>
                                        </tr>
                                         <tr>
                                            <th><label id="lblEditState" runat="server">State</label></th>
                                            <td>
                                                <input id="ddlEditState" name="ddlEditState" data-bind="value:StateName" style="width: 300px" />
                                            </td>
                                        </tr>

                                          <tr>
                                            <th><label id="lblEditCity" runat="server">City</label></th>
                                            <td>
                                                <input id="ddlEditCity" name="ddlEditCity" data-bind="value:CityName" style="width: 300px" />
                                            </td>
                                        </tr>

        <tr>
            <th>Address</th>
            <td>
                <textarea id="txtAddress" rows="4" cols="40" name="txtAddress" data-bind="value:custAddress" style="width: 300px; resize: none;" required validationmessage="Please enter Address" class="k-textbox"></textarea>
            </td>
        </tr>
        <tr>
            <th>Notes</th>
            <td>
                <textarea id="txtNotes" rows="4" cols="40" name="txtNotes" data-bind="value:custNotes" style="width: 300px; resize: none;" class="k-textbox"></textarea>
            </td>
        </tr>  
         <tr>
            <th>Show All Task </th>
            <td>
               <input id="chkIsShowAllTask" type="checkbox" name="chkIsShowAllTask" />
            </td>
        </tr>                                    
   </table>
        <br/>         
    </script>
    <%--  PopUP Div Starts --%>
    <div id="divAddPopupOverlay"></div>
    <div class="a_popbox" id="divAddPopup" style="display: none;">
        <div class="popup_wrap" style="width: 600px; top: -30%; left: 30%;">
            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP()" />
            <table width="100%">
                <tr>
                    <td colspan="2" align="center">
                        <span id="Span1" style="font-size: large; font-weight: 100">Add New Customer</span>
                    </td>

                </tr>
            </table>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">

                <tr>

                    <td>

                        <asp:Panel ID="pnlAddCustomer" runat="server">
                            <div id="example" class="k-content">
                                <div id="tickets">
                                    <table class="manage_form" width="100%">

                                        <tr>
                                            <td colspan="2" align="center">
                                                <span id="Span2" style="font-size: large; font-weight: lighter">Company Details</span>
                                            </td>

                                        </tr>
                                        <tr>
                                            <th>Name</th>
                                            <td>
                                                <input id="txtCustomerName" type="text" name="txtCustomerName" style="width: 300px" class="k-textbox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Company</th>
                                            <td>
                                                <input id="txtCustomerCompany" type="text" name="txtCustomerCompany" style="width: 300px" class="k-textbox" /><span id="spncomapnyexist" style="color: red; display: none"><b>Company already exists</b></span>
                                            </td>
                                        </tr>

                                         <tr>
                                            <th>GSTIN</th>
                                            <td>
                                                <input id="txtGSTIN" type="text" name="txtGSTIN" style="width: 300px" class="k-textbox" />
                                                
                                            </td>
                                        </tr>

                                        <tr>
                                            <th>Email</th>
                                            <td>
                                                <textarea id="txtEmail" rows="2" cols="40" name="txtEmail" style="width: 300px" class="k-textbox"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Email CC</th>
                                            <td>
                                                <textarea id="txtEmailCC" rows="2" cols="40" name="txtEmailCC" style="width: 300px" class="k-textbox"></textarea>
                                            </td>
                                        </tr>
<%--Added by Apurva--%>
                                         <tr>
                                            <th>Country</th>
                                            <td>
                                                <input id="ddlCountry" name="ddlCountry" style="width: 300px"/>
                                               <%-- <span id="spnCountry" style="color: red; display: none"><b>Please Select Country</b></span>--%>
                                            </td>
                                        </tr>
                                         <tr>
                                            <th><label id="lblState" runat="server">State</label></th>
                                            <td>
                                                <input id="ddlState" name="ddlState" style="width: 300px" />
                                            </td>
                                        </tr>

                                          <tr>
                                            <th><label id="lblCity" runat="server">City</label></th>
                                            <td>
                                                <input id="ddlCity" name="ddlCity" style="width: 300px" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <th>Address</th>
                                            <td>
                                                <textarea id="txtAddress" rows="4" cols="40" name="txtAddress" style="width: 300px; resize: none;"  class="k-textbox"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Notes</th>
                                            <td>
                                                <textarea id="txtNotes" rows="4" cols="40" name="txtNotes" style="width: 300px; resize: none;" class="k-textbox"></textarea>
                                            </td>
                                        </tr>

                                        <tr>
                                            <th>Show All Task </th>
                                            <td>
                                                <input id="chkShowAllTask" type="checkbox" name="chkShowAllTask" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return CheckInsert();" OnClick="btnAddCustomer_Click" class="small_button white_button open" Text="ADD" />
                                                         <%--    <asp:Button ID="btnAddCustomer" runat="server" CssClass="small_button white_button open" OnClientClick="javascript:return CheckInsert();" Text="Add" OnClick="btnAddCustomer_Click" />--%>
                                                        </td>

                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>

                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hfCustID" runat="server" />
    <asp:HiddenField ID="hftxtCustomerName" runat="server" />
    <asp:HiddenField ID="hftxtCustomerCompany" runat="server" />
    <asp:HiddenField ID="hftxtAddress" runat="server" />
    <asp:HiddenField ID="hftxtNotes" runat="server" />
    <asp:HiddenField ID="hftxtCustEmail" runat="server" />
    <asp:HiddenField ID="hftxtCustEmailCC" runat="server" />
    <asp:HiddenField ID="hfIsShowAllTask" runat="server" />

     <asp:HiddenField ID="hfddlCountryId" runat="server" />
    <asp:HiddenField ID="hfddlStateId" runat="server" />
    <asp:HiddenField ID="hfddlCityId" runat="server" />
    <asp:HiddenField ID="hftxtGSTIN" runat="server" />
</asp:Content>



