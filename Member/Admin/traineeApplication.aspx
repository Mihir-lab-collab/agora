<%@ Page Language="C#" AutoEventWireup="true" CodeFile="traineeApplication.aspx.cs"
    Inherits="Admin_traineeApplication" %>

<%@ Register Src="../controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dynamic Admin Control :: Trainee Application </title>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="4" width="100%">
        <tr>
            <td colspan="5">
                <uc1:adminMenu ID="AdminMenu1" runat="server" />
            </td>
        </tr>
        <tr bgcolor="#edf2e6">
            <td bgcolor="#edf2e6">
                <b><font face="Verdana" size="2">
                    <p align="left">
                        <a href="traineeApplication.aspx?search=Pending"><font color="#A2921E">Pending</font></a>|<a
                            href="traineeApplication.aspx?search=Confirmed"><font color="#A2921E">Confirmed</font></a>|<a
                                href="traineeApplication.aspx?search=All"><font color="#A2921E">All</font></a>
                    </p></b>
            </td>
        </tr>
        <tr bgcolor="#edf2e6">
            <td bgcolor="#edf2e6" valign="top" align="center">
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr bgcolor="#edf2e6" id="trSearch" runat="server">
            <td bgcolor="#edf2e6" valign="top" align="center">
                <b>Please enter the form no : </b>
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" BackColor=" #C5D5AE" Font-Size="10pt"
                    ForeColor=" #A2921E" Font-Bold="true" OnClick="btnSearch_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="grdTraineeDetails" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    DataKeyNames="RegID" Width="100%" OnRowDataBound="grdTraineeDetails_RowDataBound"
                    OnRowEditing="grdTraineeDetails_RowEditing" OnRowCancelingEdit="grdTraineeDetails_RowCancelingEdit"
                    OnRowUpdating="grdTraineeDetails_RowUpdating" OnRowCommand="grdTraineeDetails_RowCommand"
                    PageSize="50" onrowdeleting="grdTraineeDetails_RowDeleting">
                    <HeaderStyle Font-Size="11pt" BackColor="#edf2e6" ForeColor="#A2921E" Font-Bold="True">
                    </HeaderStyle>
                    <AlternatingRowStyle BackColor="#edf2e6" />
                    <Columns>
                        <asp:TemplateField HeaderText="Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRegID" runat="server" Text='<%#Eval("RegID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblRowIndex" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Form No" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblFormID" runat="server" Text='<%#Eval("FormNo")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DOB" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblDOBirth" runat="server" Text='<%#Eval("DOB")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="State" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Text='<%#Eval("State")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EmailID" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblEmailID" runat="server" Text='<%#Eval("EmailID")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobile No" HeaderStyle-HorizontalAlign="Left">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Eductaion Details" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblEductaionDetails" runat="server" Text='<%#Eval("EductaionDetails")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Intersted Because" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblInterstedBcoz" runat="server" Text='<%#Eval("InterstedBcoz")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%#GetStatus(Convert.ToBoolean(Eval("Status")))%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#Eval("Status")%>' />
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField FooterText="Actions" HeaderText="Actions" ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ToolTip="Click here to Update" Text="Update" ValidationGroup="edit"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ToolTip="Click here to Cancel" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" ValidationGroup="edit" CausesValidation="False"
                                    CommandName="Edit" ToolTip="Click here to Edit." Text="Edit"></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Click here to Delete record" Text="Delete" OnClientClick="javascript:return confirm('Do you want to delete this record');return false;"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReceipt" runat="server" CommandName="CratePdf" CommandArgument='<%# Eval("RegID") %>'
                                    Text="Receipt" ToolTip="Click here to create Pdf.">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
