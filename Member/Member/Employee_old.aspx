<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Employee_old.aspx.cs"
    Inherits="Member_Default" EnableEventValidation="false" ValidateRequest="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <span style="color: rgb(34, 34, 34); font-family: Consolas, 'Lucida Console', monospace; font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: pre-wrap; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; display: inline !important; float: none; background-color: rgb(255, 255, 255);"></span>
   
    <style type="text/css">
        div.DialogueBackground {
            position: fixed;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            background-color: #777;
            opacity: 0.9;
            filter: alpha(opacity=50);
            text-align: center;
        }

            div.DialogueBackground div.Dialogue {
                width: 300px;
                height: 400px;
                overflow: auto;
                position: absolute;
                left: 50%;
                top: 20%;
                margin-left: -150px;
                margin-top: -50px;
                padding: 10px;
                background-color: #fff;
                border: solid 2px #000;
                z-index: 10090;
            }


        div.close-button {
            float: right;
            margin: -10px -10px 0 0;
        }

        div.k-overlayDisplaynone {
            display: none;
        }

        div.k-overlay {
            display: block;
            z-index: 10002;
            opacity: 0.5;
        }

        div.a_popbox {
            position: absolute;
            padding: 25px;
            z-index: 10050;
            background-color: white;
        }

        div.popup_wrap {
            border: 6px solid #252e34;
            background-color: #FFF;
            position: absolute;
            padding: 15px;
            z-index: 2;
            overflow-x: scroll;
        }

            div.popup_wrap h1 {
                padding: 0 0 10px 0;
                margin-bottom: 15px;
                border-bottom: 1px solid #e5e7e4;
                font-size: 22px;
                color: #728c3a;
            }

        div.popup_msg {
            border: 6px solid #252e34;
            background-color: #FFF;
            position: absolute;
            padding: 15px;
            top: 20%;
            left: 25%;
            right: 25%;
            z-index: 1;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#A1').click(function () {
                $('#hdSize').val('10');
                Loadpage();
            });
            $('#A2').click(function () {
                $('#hdSize').val('15');
                Loadpage();
            });
        });
        function Loadpage() {
            try {
                //reload page using new page size
                var url = $(location).attr('href');
                var index = "Index=1";
                if (url.indexOf('Index') > 0) {
                    index = url.split('?')[1];
                    index = index.split('&')[0];
                }
                url = url.split('?')[0] + "?" + index + "&Size=" + $('#hdSize').val() + "";
                window.location.href = url;
            } catch (e) {
                alert(e);
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


        function CheckExistsEmailid() {
            var EmpID = 0;
            if ($('#ctl00_ContentPlaceHolder1_btnSave').val() != 'SAVE')
                EmpID = $('#ctl00_ContentPlaceHolder1_txtempid').val();

            if ($('#ctl00_ContentPlaceHolder1_empEmail').val() != '') {
                var Email = $('#ctl00_ContentPlaceHolder1_empEmail').val()
                $.ajax({
                    type: "POST",
                    url: "Employee.aspx/GetExistsClient",
                    contentType: "application/json;charset=utf-8",
                    data: "{'EmailID':'" + Email + "','EmpID':' " + EmpID + "'}",
                    cache: false,
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        if (data.d !== "0") {
                            alert('Emailid already exist.');
                            $('#ctl00_ContentPlaceHolder1_empEmail').focus();
                            return false;
                        }
                        else
                            return true;

                    },
                    error: function (data) {
                        alert("The call to the server side failed."
                              + data.responseText);
                    }
                });
            }
            else
                return false;
        }



    </script>
   
        <%-- With update panel--%>
        <div class="content_wrap">
            <div class="gride_table">
                <div class="box_border">
                    <div class="grid_head">

                        <asp:Label ID="lblInventoryDetails" Text="Employee Master" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        <div style="float: right">
                            <asp:Button ID="btnAdd" Text="ADD" runat="server" OnClick="btnAdd_Click" CssClass="small_button white_button open" CausesValidation="false" />
                            <asp:Button ID="btnView" Text="VIEW" runat="server" OnClick="btnView_Click" CssClass="small_button white_button open" CausesValidation="false" />
                        </div>
                    </div>

                    <asp:Panel ID="pnlEmployeeAdd" runat="server" ClientIDMode="Static">

                        <%-- <asp:ScriptManager ID="scriptmanagerEmployee" runat="server"></asp:ScriptManager>      
    
 
            <asp:UpdatePanel ID="updatepanelEmployee" runat="server">
             
        <ContentTemplate>    --%>
                        <table class="manage_form">
                        </table>
                        <asp:ScriptManager ID="scriptmanagerEmployee" runat="server"></asp:ScriptManager>


                   <%--   <asp:UpdatePanel ID="updatepanelEmployee" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                         
                           <ContentTemplate>--%>
                                <table id="tdgrid" runat="server" width="100%" class="manage_form">
                                    <tr>
                                        <th style="width: 141px;">Attachment:</th>
                                        <td>
                                            <asp:FileUpload ID="fileUploadImage" runat="server"></asp:FileUpload>
                                            <%--  <asp:Button ID="btnProcessData" runat="server" Text="Process Data" CausesValidation="false" OnClick="btnProcessData_Click" />--%>
                                            <asp:Label ID="lblMessage" runat="server" Text=" Upload Only /Doc/Docx/PDF formats."></asp:Label><br />
                                            <br />
                                            <asp:Label ID="lblUploadedName" Visible="true" runat="server">Display Name</asp:Label>
                                        </td>
                                        <th>Photo:</th>
                                        <td>
                                            <div id="PhotoAttch">
                                                <img alt="" id="photoImage" src="" runat="server" />
                                            </div>
                                            <br />
                                            <asp:FileUpload ID="filePhotoUpload" runat="server"></asp:FileUpload>
                                        </td>

                                    </tr>
                                    <tr>
                                        <th>Location
                                        </th>
                                        <td>

                                            <asp:DropDownList ID="ddlLocationList" runat="server" CssClass="b_dropdown">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                                                ControlToValidate="ddlLocationList" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>

                                        </td>
                                        <th>
                                            <span runat="server" id="Editempid" visible="false">EmpID:</span>
                                        </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtempid" CssClass="a_textbox" Visible="false"></asp:TextBox>

                                        </td>

                                    </tr>

                                    <tr>
                                        <th>Employee Name </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="empName" CssClass="a_textbox"></asp:TextBox><span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="empName" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                        </td>
                                        <th>Joining Date </th>
                                        <td>
                                            <asp:TextBox ID="empJoiningDate" runat="server" CssClass="a_textbox"></asp:TextBox><span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="empJoiningDate" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="empJoiningDate" CssClass="cal_Theme1" Format="dd/MM/yyyy"></ajax:CalendarExtender>

                                        </td>
                                    </tr>

                                    <tr>
                                        <th>Address</th>
                                        <td>
                                            <asp:TextBox runat="server" ID="empAddress" TextMode="MultiLine" Height="57px" Width="250px"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="empAddress" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <th>Leaving Date</th>
                                        <td>
                                            <asp:TextBox ID="empLeavingDate" runat="server" CssClass="a_textbox"></asp:TextBox>
                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="empLeavingDate" CssClass="cal_Theme1" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Contact Number</th>
                                        <td>
                                            <asp:TextBox runat="server" ID="empContact" CssClass="a_textbox" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="empContact" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                        </td>
                                        <th>Probation Period</th>
                                        <td>
                                            <div class="tdbox">
                                                <asp:DropDownList runat="server" ID="empProbationPeriod">
                                                    <asp:ListItem Value="0">0</asp:ListItem>
                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                    <asp:ListItem Value="3" Selected="True">3</asp:ListItem>
                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Email Id </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="empEmail" CssClass="a_textbox" onblur="return CheckExistsEmailid();"></asp:TextBox>
                                            <span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="empEmail" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email." ValidationGroup="validate" ControlToValidate="empEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                                            <asp:Button ID="btnSendMail" Text="Send Mail" OnClick="btnSendMail_Click" runat="server" Visible="false" CssClass="small_button white_button open" />
                                        </td>

                                        <th>Designation</th>
                                        <td>
                                            <asp:DropDownList ID="empSkill" runat="server" AutoPostBack="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvempSkill" ControlToValidate="empSkill" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Rights</th>
                                        <td>
                                            <asp:CheckBox ID="right2" runat="server" Text="Tester"></asp:CheckBox>
                                        </td>
                                        <th>Account No.</th>
                                        <td>
                                            <asp:TextBox runat="server" ID="empAccountno" CssClass="a_textbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Employee BirthDate</th>
                                        <td>
                                            <asp:TextBox ID="empBDate" runat="server" CssClass="a_textbox"></asp:TextBox>
                                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="empBDate" CssClass="cal_Theme1" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                            <%--<asp:RequiredFieldValidator ID="rfvEmpBDate" ControlToValidate="empBDate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <th>Anniversary Date</th>
                                        <td>
                                            <asp:TextBox ID="empADate" runat="server" CssClass="a_textbox"></asp:TextBox>
                                            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="empADate" CssClass="cal_Theme1" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Previous Employer</th>
                                        <td>
                                            <asp:TextBox ID="empPrevEmployer" runat="server" TextMode="MultiLine" Height="56px" Width="248px"></asp:TextBox>
                                        </td>
                                        <th>Qualification</th>
                                        <td>
                                            <asp:ListBox ID="lstempQual" runat="server" Rows="4" cols="40" Name="empQualification"
                                                SelectionMode="Multiple" EnableViewState="true" Width="263px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Primary Skill</th>
                                        <td>
                                            <asp:DropDownList ID="ddlPrimarySkill" runat="server" CssClass="b_dropdown" Style="margin-bottom: 30px;" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <th>Secondary Skill
                                        </th>
                                        <td>
                                            <asp:ListBox ID="lstSecondarySkill" runat="server" Rows="4" cols="40"
                                                SelectionMode="Multiple" EnableViewState="true" Width="263px"></asp:ListBox></td>
                                    </tr>
                                    <tr>
                                        <th>Experience</th>
                                        <td>
                                           <%-- <asp:UpdatePanel ID="updExp" runat="server">
                                                <ContentTemplate>--%>
                                                    <div class="tdbox">
                                                        <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="dropempExpyears" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropempExpyears_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="tdbox">
                                                        <asp:Label ID="lblMonth" runat="server" Text="Month"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="dropempExpmonths" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropempExpyears_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div style="margin: 22px 0 0 8px; float: left;">
                                                        <asp:Label ID="lblExp" runat="server" Text="0 month"></asp:Label>
                                                        <asp:HiddenField ID="hndExperienceStore" runat="server" />
                                                    </div>
                                               <%-- </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                        <th>Notes</th>
                                        <td>
                                            <asp:TextBox ID="empNotes" runat="server" TextMode="MultiLine" Height="57px" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div id="Div1" runat="server" style="text-align: right">
                                                <span style="color: red; float: left;">Fields marked with * are mandatory</span><span class="clear"></span>
                                                <asp:Label ID="lblResult" runat="server" ForeColor="Red" Visible="true"></asp:Label>
                                                <asp:Button runat="server" ID="btnSave" ValidationGroup="validate" Text="SAVE" OnClick="btnSave_Click" CommandArgument='<%#Eval("empid") %>' CssClass="small_button white_button open" OnClientClick="CheckExistsEmailid();" />
                                                <asp:Button runat="server" ID="btnCancel" Text="CANCEL" OnClick="btnCancel_Click" Visible="false" CssClass="small_button white_button open" CausesValidation="false" />
                                            </div>
                                        </td>
                                    </tr>

                                </table>
                           <%-- </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                                <asp:PostBackTrigger ControlID="btnCancel" />
                                <asp:PostBackTrigger ControlID="gvEmployeeView" />
                            </Triggers>
                  </asp:UpdatePanel>--%>
                    </asp:Panel>
                    <asp:Panel ID="pnlEmployeeView" runat="server">
                        <table width="100%" class="manage_grid_a">
                            <tr>
                                <th style="border-bottom: none!important;">
                                    <div style="text-align: left">
                                    </div>
                                    <div style="text-align: Left">
                                        <asp:Label ID="lblTotalEmployee" runat="server" />
                                        <asp:DropDownList ID="dlstLeavingDate" runat="server" AutoPostBack="true" Width="160"
                                            OnSelectedIndexChanged="drpListJoiningDate_SelectedIndexChanged" CssClass="c_dropdown">
                                            <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                            <asp:ListItem Text="Current" Value="Current" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocation" Text="Location:" runat="server" Visible="true" />
                                        <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" 
                                            Width="160" CssClass="b_dropdown" OnSelectedIndexChanged="dlLocation_SelectedIndexChanged"
                                            Visible="false" AppendDataBoundItems="true">
                                            <asp:ListItem Text="ALL" Value="0"></asp:ListItem>

                                        </asp:DropDownList>

                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="b_dropdown" Width="160">
                                        </asp:TextBox>
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="small_button white_button open" OnClick="btnSearch_Click" />
                                        <%--<asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" class="small_button white_button open" OnClick="btnGenerateReport_Click" />--%>
                                        <asp:Button ID="btnColumnsMapping" runat="server" Text="Generate Report" class="small_button white_button open" OnClick="ShowHideDialogue" CommandArgument="show" />
                                    </div>
                                </th>
                                <%--  <td align="right">
                                 <asp:DropDownList ID="ddlPageCounts" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageCounts_SelectedIndexChanged" CssClass="a_dropdown">
                                    <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="50" Value="50" ></asp:ListItem>
                                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                            </tr>

                        </table>
                        <div style="overflow-x: scroll; height: 100%; width: 100%">
                            <asp:GridView ID="gvEmployeeView" DataKeyNames="empid" runat="server" CssClass="manage_grid_a mange_lsttd" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true" PageSize="150" Width="100%" OnPageIndexChanging="gvEmployeeView_PageIndexChanging" OnRowDataBound="gvEmployeeView_RowDataBound" OnRowCommand="gvEmployeeView_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="50">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnView" CommandName="View" runat="server" ImageUrl="~/Member/images/edit.png" ToolTip="View & Edit" Height="20" Width="20" CausesValidation="false" OnClick="imgbtnEdit_Click" CommandArgument='<%#Eval("empid") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="nowrap" HeaderText="Resume">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkattach" runat="server" OnCommand="ViewResume" CommandArgument='<%#Eval("empid")+"#"+Eval("Resume")+"#"+Eval("empName")%>'>
                                           <%# (Eval("Resume").ToString()!="" ? " <img id='Img1' src='/images/icon-pdf.gif' alt='Download' runat='server'/>" : "") %>
                                            </asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employee ID">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnSecurityLevel" runat="server" Value='<%#Eval("SecurityLevel") %>' />
                                            <asp:Label ID="lblEmployeeId" runat="server" Text='<%#Eval("empid") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("empName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contact No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContact" runat="server" Text='<%#Eval("empContact") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblskillid" runat="server" Text='<%#Eval("Skill") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Experience">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExperince" runat="server" Text='<%# CalculateTotalExp(Eval("empJoiningDate") == DBNull.Value ? "" : Eval("empJoiningDate","{0:dd/MM/yyyy}"), Eval("empExperince") == DBNull.Value ? 0 : Convert.ToInt32(Eval("empExperince")), true) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Joining Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJoiningDate" runat="server" Text='<%# Eval("empJoiningDate","{0:dd-MMM-yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leaving Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeavingDate" runat="server" Text='<%# Eval("empLeavingDate") == DBNull.Value ? "" : Eval("empLeavingDate").ToString().Trim() == "" ? "" : ((DateTime)Eval("empLeavingDate")).ToString("dd-MMM-yyyy") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Birth Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBirthDate" runat="server" Text='<%# Eval("empBDate","{0:dd-MMM-yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle />
                                <PagerTemplate>
                                    <table id="pagerInnerTable" runat="server" class="pageing">
                                        <tr align="center">
                                            <td>
                                                <asp:LinkButton ID="lnkPrevPage" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Prev" runat="server">Prev</asp:LinkButton>
                                            </td>
                                            <td>
                                                <div id="divListBottons" runat="server"></div>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkNextPage" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Next" runat="server">Next</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <asp:Panel runat="server" ID="pnlColumns" Visible="false" CssClass="DialogueBackground">
            <div class="Dialogue" id="divcolumnslistPopup">
                <p>Generate Report</p>
                <table id="tbColumnsList" width="100%" class="manage_form">
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="chkColumnsList" RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true" TextAlign="Right" runat="server" Height="100%"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAddColumns" Text="OK" runat="server" OnClick="btnAddColumns_Click" CssClass="small_button white_button open" CausesValidation="false" />
                            <asp:Button ID="btnCancelAddCoumns" Text="Cancel" runat="server" OnClick="btnCancelAddCoumns_Click" CssClass="small_button white_button open" CausesValidation="false" />
                        </td>
                    </tr>

                </table>

            </div>
        </asp:Panel>


        <%--   </ContentTemplate>
    </asp:UpdatePanel>
        --%>
   

    <asp:HiddenField ID="hdnDocName" runat="server" />
    <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hdHasAllLocationAcess" runat="server" Value="0" />
</asp:Content>
