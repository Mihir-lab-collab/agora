<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="UploadPolicy.aspx.cs" Inherits="Member_UploadPolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%-- <script type="text/javascript">
        $(document).ready(function () {
            alert("hi");            

            //$("#btnTest").click(function () {
            //    $("#fuTest").trigger("click");
            //});
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">      
     <br />
     <ContentTemplate>
    <div class="content_wrap">
                <div class="gride_table">
                    <div class="box_border">
                    <div class="grid_head">
    <div style="padding-top:10px;padding-left:20px;"><h1 align="left"> Upload HR Policies</h1></div>   
    <br />
                       <%--  <div class="accord">--%>
<table id="tdchpwd" width="100%" runat="server" class="manage_form">
    <tr>
                                                    <th style="width: 15%;">File Type</th>
        <td>

    <div runat="server" align="left" id="DivFileType" style="padding-left:20px;">
                                    <%--<asp:Label ID="lblfiletype" runat="server" Text="FileType" CssClass="head" ForeColor="Black" Font-Size="Medium"></asp:Label>--%>
                                    <asp:DropDownList ID="ddlfiletype" Width="150px" runat="server" DataTextField="FileType"
                                        DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlfiletype_SelectedIndexChanged">
                                    </asp:DropDownList> <span style="color:red"> * </span>
                                </div> <br /><br>
            </td>
        </tr>
    <tr>
         <th style="width: 15%;"> Browse File </th>
        <td>
    <div id="div1" align="left" style="padding-left:20px;">
        <asp:FileUpload ID="FileUpload1" class="small_button white_button" accept=".pdf,.PDF" runat="server" onchange="Filevalidation()" /><span style="color:red"> * </span><br /><br />
            <asp:Button ID="Button1" class="small_button white_button" runat="server" onclick="Button1_Click" Text="Upload" />&nbsp;
        <asp:Button ID="Button2" class="small_button white_button" runat="server" onclick="Button2_Click" Text="Reset" />  
            <div align="left" style="padding-left:20px;padding-top:10px;padding-bottom:10px;">           
                    <asp:Label ID="lblMsg" ForeColor="Red" runat="server" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" />  
                    <asp:Label ID="lblSelected" runat="server" ForeColor="Green" /> 
                   <asp:Label ID="lblSize" runat="server" ForeColor="Green" />         
              <%-- <hr />--%>
            </div> 
        

            <%-- <asp:Button ID="Button2" class="small_button white_button" runat="server" onclick="Button2_Click" Text="Reset" />   --%>
            
    </div>
            </td>
        </tr>
    </table>
    </div>
                    </div>
        </div>
         </div>
         </ContentTemplate>
<script type="text/javascript">
    document.getElementById('<%=Button1.ClientID %>').style.visibility = "visible";
    function Filevalidation(e) {
       /* e.preventDefault();*/
        var lblFile = document.getElementById("<%=lblSelected.ClientID %>");
        lblFile.innerHTML = "";
        var lblError = document.getElementById("<%=lblError.ClientID %>");
        lblError.innerHTML = ""; 
        var lblMsg = document.getElementById("<%=lblMsg.ClientID %>");
        lblMsg.innerHTML = "";
        var fileUpload = document.getElementById("<%=FileUpload1.ClientID %>");
        //alert(fileUpload.files.item(0).name);
        var fileUploadName = fileUpload.files.item(0).name;
        /*var filetypeid= fileUpload.files.item(0).ddlfiletype;*/
        var allowedFiles = ["pdf"];
        var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
        if (!regex.test(fileUpload.value.toLowerCase())) {
            lblError.innerHTML = "Only   <b>" + allowedFiles.join(', ') + "</b> Files Can Be Uploaded.";
            document.getElementById('<%=Button1.ClientID %>').style.visibility = "visible";
            return false;
        }
        <%--else
        {
        
            var allowedFilesnames = ["HR_Policy.pdf", " Hr_Medical.pdf", " Hr_ASHP.pdf"];
            if (fileUploadName == "HR_Policy.pdf")
            {                
                document.getElementById('<%=Button1.ClientID %>').style.visibility = "visible";
            }
            else if (fileUploadName == "Mediclaim_Policy.pdf")
            {
                document.getElementById('<%=Button1.ClientID %>').style.visibility = "visible";
            }
            else if (fileUploadName == "ASH_Policy.pdf")
            {
                document.getElementById('<%=Button1.ClientID %>').style.visibility = "visible";
            }
            else
            {
                //alert("else");
                document.getElementById('<%=Button1.ClientID %>').style.visibility = "hidden";
                lblError.innerHTML = "Upload Policy File Name Should be : <b>" + allowedFilesnames + "</b> only.";
            }--%>
           <%--if (fileUpload.files.length > 0)
            {
                for (var i = 0; i <= fileUpload.files.length - 1; i++) {
                    var fsize = fileUpload.files.item(i).size;
                    var file = Math.round((fsize / 1024));
                    // The size of the file.
                    if (file >= 4096) {
                        alert("File too Big, please select a file less than 4mb");
                    } else {
                        document.getElementById('<%=lblSize.ClientID %>').innerHTML = '<b>' + file + '</b> KB Size File Selected ';
                    }
                }
            }--%>
           // lblFile.innerHTML = fileUpload.files.item(0).name.files;
       /* }*/
    }
</script>

</asp:Content>