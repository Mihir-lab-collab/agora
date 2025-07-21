<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Notes.ascx.cs" Inherits="Controls_Notes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script >
    function closeNote() {
        document.getElementById('ctl00_ContentPlaceHolder1_Notes1_NotesDiv').style.display = "none"
    }
</script>
<asp:LinkButton ID="NotesLink" runat="server" OnClick="NotesLink_Click" 
    Visible="false" CausesValidation="false">Notes</asp:LinkButton>
 
<%--  <asp:ScriptManager ID="scriptmanagerEmployee" runat="server"></asp:ScriptManager>--%>

<%--<asp:UpdatePanel ID="updatenotes"  runat="server" >
    <ContentTemplate>--%>



        <div id="NotesDiv" runat="server" visible="false" style="top: 0px; right: 0px; width: 320px;
            position: fixed; background-color:#F2F2F2; z-index: 1000; overflow:auto; height:500px; border:1px solid #dbdada;">
            <div id="AddNotesDiv" runat="server" 
                style="background-color: #ebe9e9; padding: 5px" >
                Note
                <div style="float:right">
                    <%--<asp:LinkButton ID="NotesHide" runat="server" CausesValidation="false"
                        onclick="NotesClose_Click" ForeColor="black">Close</asp:LinkButton>--%>
                     <img src="../images/delete_ic.png" alt="Close"  style="cursor:pointer;" onclick="return closeNote();"/>
                    </div>
               
                </div>
            <div style="padding:3px">
            <asp:TextBox ID="NotesTxt" runat="server" Height="60px" TextMode="MultiLine" Width="290px"></asp:TextBox>
                <span style="color:red;">*</span>
            <br />
                <asp:HiddenField ID="hdnempid" runat="server" />
            <asp:Button ID="AddBtn" runat="server" OnClick="AddBtn_Click" Text="Add Note"  CausesValidation="false" />&nbsp;&nbsp;
                
            <br /> 
              <span style="color:green;" runat="server" id="spndisplaymsg" visible="false">Note added successfully.</span>   
             <br />                               
            <asp:GridView ID="NotesGrid" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="2" CellSpacing ="2" CssClass="manage-table" DataKeyNames="empid"
                    onrowcommand="NotesGrid_RowCommand" OnRowDataBound="NotesGrid_RowDataBound">
                <Columns>

                   <asp:TemplateField AccessibleHeaderText="Note" HeaderText="Notes">
                        <ItemTemplate>
                                <asp:Label ID="lblnotese" Style="font-weight: 300;" runat="server" Text='<%# Eval("empNotes").ToString().Replace("\n","<br>") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate >
                       <asp:Label ID="lblnotese" runat="server" Text="No records found."></asp:Label>
                </EmptyDataTemplate>
            <HeaderStyle BackColor="#808080" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#E5E5E5" />
            </asp:GridView>            
            </div>
        </div>

<%--            </ContentTemplate>
</asp:UpdatePanel> --%>
