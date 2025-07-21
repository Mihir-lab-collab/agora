<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>change request</title>
     <link href="../includes/CalendarControl.css" rel="stylesheet" type="text/css" />
    <script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js"> </script>
</head>

<script language="JavaScript" type="text/javascript">
    function popupProjectDetail(Id, isInvgen) {
        var win = window.open('view_changeRequest.aspx?id=' + Id + '&isInvgen=' + isInvgen, 'winWatch', 'scrollbars=yes,resizable=1,toolbar=no,menubar=no,location=right,width=700,height=550,left=150,top=50');
        win.focus();
    }
    function redirectProjectSummary(prgid) {
        alert("Invoice generated already.")
        window.location = 'paymentSummary.aspx?projId=' + prgid;
    }
	function popupChangeRequest(Id)
	{  
		if (Id==-1)
		{
			alert('select Project........');
			return false;
		}
		if(Id!=-1)
		{
			 window.open("adminAddchangeRequest.aspx?projId="+ Id,null, "height=430,width=800,left=100,top=50,status=yes,toolbar=no,menubar=no,location=no");
		}
	} 
</script>

<script language="VB" runat="server">
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim Conn As SqlConnection = New SqlConnection(dsn)
    Dim Cmd As New SqlCommand()
    Dim SortField As String
    Dim cust_name, cust_add, comp_name, cust_email, cust_prjname As String
    Dim cust_regdate As Date
    Dim gf As New generalFunction
    
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        gf.checkEmpLogin()
        Dim FromDate As Date = (DateAndTime.Today).AddYears(-1)
        Dim currentDate As Date = DateAndTime.Today
        btnAdd.Attributes.Add("onclick", "javascript:popupChangeRequest(dd_prj_name.value); return false;")
 
        Dim sql As String
        Conn.Open()
        Dim search As Integer
 	 
	
        If dd_prj_name.SelectedValue <> "" Then
            search = dd_prj_name.SelectedValue
        ElseIf ((Request.QueryString("projId") <> Nothing Or Request.QueryString("projId") <> "")) Then   ' add code by satya 11 may 2012
            search = Convert.ToInt32(Request.QueryString("projId"))
        End If
            
        
	
        If search <> -1 Then
            Dim cmdProject As SqlCommand
            Dim dtrProject As SqlDataReader
            sql = "SELECT * FROM Customermaster ,projectMaster WHERE Customermaster.custid=projectMaster.custid AND projectMaster.projid=" & search
            cmdProject = New SqlCommand(sql, Conn)
            dtrProject = cmdProject.ExecuteReader()
	
            Do While (dtrProject.Read())
                cust_name = dtrProject("custname").ToString()
                cust_add = dtrProject("custAddress").ToString()
                comp_name = dtrProject("custCompany").ToString()
                cust_email = dtrProject("custemail").ToString()
                cust_regdate = dtrProject("custregdate")
                cust_prjname = dtrProject("projname").ToString()
            Loop
            dtrProject.Close()
        End If
        Conn.Close()
        If Page.IsPostBack = "False" Then
		    txtFromDate.Value = FromDate.ToString("dd-MMM-yyyy")
            txtToDate.Value = currentDate.ToString("dd-MMM-yyyy")
            fillProject()
            BindGrid()
            fillDDL()     
        End If
    End Sub
    
    Sub MyDataGrid_Sort(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
	
    End Sub
    Sub fillProject()
        Dim Con As SqlConnection
        Dim cmdProject As SqlCommand
        Dim strProject As String
        Dim dtrProject As SqlDataReader
        Con = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        strProject = "SELECT projId , projName FROM projectMaster order by projName"
        Con.Open()
        cmdProject = New SqlCommand(strProject, Con)
        dtrProject = cmdProject.ExecuteReader()
        If dd_prj_name.Items.Count = 0 Then
            dd_prj_name.Items.Add(New ListItem("All", -1))
            Do While (dtrProject.Read())
                dd_prj_name.Items.Add(New ListItem(dtrProject("projName").ToString(), dtrProject("projId").ToString()))
            Loop
        End If
        dtrProject.Close()
	
        cmdProject.Dispose()
        Con.Close()
        If Request.QueryString("projId") <> "" Then
            dd_prj_name.SelectedValue = Request.QueryString("projId")
        End If
	
	
    End Sub
       Sub fillDDL()
        ddlStatus.Items.Add(New ListItem("All", "All"))
        ddlStatus.Items.Add(New ListItem("Approved", "A"))
        ddlStatus.Items.Add(New ListItem("Pending", "P"))
        ddlStatus.Items.Add(New ListItem("Rejected", "R"))
        ddlStatus.Items.Add(New ListItem("Hold", "H"))
    End Sub
     Public Sub BindGrid()
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Cmd.Connection = Conn
        Dim strSQL As String = ""
        Conn.Open()
        Dim search As Integer
    
        strSQL = "SELECT *,isnull(paymentmaster.crId,0) as paymentCRID FROM changeRequest "
        strSQL = strSQL & " inner join projectMaster on changeRequest.chgProjId=projectMaster.projId "
        strSQL = strSQL & "left join paymentmaster on paymentmaster.crId = changerequest.chgId where 1=1 "
        If (dd_prj_name.SelectedValue <> "-1") Then
            strSQL = strSQL & " and projectMaster.projId =" & dd_prj_name.SelectedValue
        End If
        If (ddlStatus.SelectedValue <> "All" And ddlStatus.SelectedValue <> "") Then
            strSQL = strSQL & " and changerequest.chgApproved='" & ddlStatus.SelectedValue & "'"
        End If
        If (txtFromDate.Value <> "" And txtToDate.Value <> "") Then
            strSQL = strSQL & " and chgdate Between '" & Convert.ToDateTime(txtFromDate.Value + " 00:00:00") & "' And '" & Convert.ToDateTime(txtToDate.Value + " 23:59:00") & "' "
        End If
        strSQL = strSQL & " order by  changeRequest.insertedOn desc"
   'Response.Write(strSQL)
        Cmd.CommandText = strSQL
        da = New SqlDataAdapter(strSQL, Conn)
        ds = New DataSet()
        HV1.Value = "0"
        da.Fill(ds)
        MyDataGrid.DataSource = ds
        MyDataGrid.DataBind()
        Conn.Close()
    End Sub



    Sub ddcust_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGrid()
    End Sub
    Sub dd_prj_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGrid()
    End Sub
     Sub ddlStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
       BindGrid()
    End Sub
    Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        If e.CommandName = "delete" Then
            Dim id As Integer
            id = Int32.Parse(e.Item.Cells(0).Text)
            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += " val= confirm('Do you really want to delete this record?');"
            sp += " if (val==true)  "
            sp += " window.location.href = 'delete_changeRequest.aspx?ID=" & id & "';"
            sp += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType, "script123", sp)
        End If
    End Sub

    Private Sub ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
         If (e.Item.Cells(6).Text = "P") Then
            e.Item.Cells(6).Text = "Pending"
            e.Item.Cells(6).ForeColor = System.Drawing.Color.Blue
        End If
        If (e.Item.Cells(6).Text = "R") Then
            e.Item.Cells(6).Text = "Rejected"
            e.Item.Cells(6).ForeColor = System.Drawing.Color.Red
        End If
        If (e.Item.Cells(6).Text = "A") Then
            e.Item.Cells(6).Text = "Accepted"
            e.Item.Cells(6).ForeColor = System.Drawing.Color.Green
        End If
        If (e.Item.Cells(6).Text = "H") Then
            e.Item.Cells(6).Text = "On hold"
            e.Item.Cells(6).ForeColor = System.Drawing.Color.OrangeRed 
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim view As Button = CType(e.Item.Cells(9).FindControl("view"), Button)
    		Dim paymentCrID  as string
            paymentCrID = e.Item.Cells(2).Text 
            If (paymentCrID = "0") Then
                view.Attributes.Add("onclick", "popupProjectDetail(" & e.Item.Cells(0).Text & ",0); return false; ")
            Else
                view.Attributes.Add("onclick", "popupProjectDetail(" & e.Item.Cells(0).Text & ",1); return false; ")
 
            End If
        End If
    End Sub

    Sub SortCommand_OnClick(ByVal Source As Object, ByVal E As DataGridSortCommandEventArgs)
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Cmd.Connection = Conn
        Dim strSQL As String
        ' Dim prjName As String
        Conn.Open()
        Dim search As Integer
        If dd_prj_name.SelectedValue <> "" Then
            search = dd_prj_name.SelectedValue
	            
            If search = -1 Then
                strSQL = "SELECT * FROM changeRequest inner join projectMaster on changeRequest.chgProjId=projectMaster.projId left join paymentmaster on paymentmaster.crId = changerequest.chgId order by changeRequest.InsertedOn desc"
            Else
                strSQL = "SELECT * FROM changeRequest inner join projectMaster on changeRequest.chgProjId=projectMaster.projId left join paymentmaster on paymentmaster.crId = changerequest.chgId WHERE changeRequest.chgProjId=" & search & " AND projectMaster.projId =" & search & " ORDER BY changeRequest.InsertedOn DESC"
            End If
   
            Cmd.CommandText = strSQL
            da = New SqlDataAdapter(strSQL, Conn)
            ds = New DataSet()
	    
            da.Fill(ds)
            Dim dv As DataView
            Dim numberDiv As Integer
            numberDiv = Convert.ToInt16(HV1.Value)
        

            dv = ds.Tables(0).DefaultView

            If ((numberDiv Mod 2) = 0) Then
                dv.Sort = E.SortExpression + " " + "DESC"
            Else
                dv.Sort = E.SortExpression + " " + "ASC"
            End If
            numberDiv += 1
            HV1.Value = numberDiv.ToString()
            MyDataGrid.DataSource = dv
            MyDataGrid.DataBind()
            
        End If
    End Sub
     Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs)
        BindGrid()
    End Sub
</script>

<body>
    <form id="Form1" runat="server" method="post">
        <input type="hidden" id="empId" runat="server" size="20" />
    
       <table cellpadding="4" width="100%" border="1" style="border-collapse: collapse;border-color: #E8E8E8">
       <ucl:adminMenu ID="adminMenu1" runat="server" />
        </table>
        <table cellpadding="4" width="100%" border="0" style="border-collapse: collapse;border-color: #E8E8E8">
            <tr>
                <td height="12" width="100%" colspan="2" style="background-color:#edf2e6">
                    <b><font face="Verdana" color="#A2921E">Change Request</font></b></td>
            </tr>
            </table>
            <table cellpadding="4" width="100%" border="0" style="border-collapse: collapse;border-color: #E8E8E8">
            <tr>
                <td align="left" bgcolor="#edf2e6" width="40%">
                    <b><font color="#A2921E" face="Verdana" size="2">&nbsp;</font> <font face="Verdana" size="2" color="#A2921E">Project Name :&nbsp;</font></b>
                    <asp:DropDownList ID="dd_prj_name" runat="server" CssClass="cssdata" AutoPostBack="True" OnSelectedIndexChanged="dd_prj_name_SelectedIndexChanged"></asp:DropDownList>
                    <u><font color="#FF0000"></font></u>
                </td>
             
                <td align="left" bgcolor="#edf2e6" width="20%">
                  <b><font color="#A2921E" face="Verdana" size="2">&nbsp;</font><font face="Verdana" size="2" color="#A2921E">Status :&nbsp;</font></b>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="cssdata" AutoPostBack="True"  OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                         </asp:DropDownList><u><font color="#FF0000"><b> </b></font></u>
                </td>
                <td colspan="2" bgcolor="#edf2e6" width="10%">
                    <asp:Button ID="btnAdd" runat="server" Text="Add New Request" Style="font-family: Verdana;
                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" /></td>
            </tr>
        </table>
       <table cellpadding="4" width="100%" border="0" style="border-collapse: collapse;border-color: #E8E8E8">
        <tr>
            <td align="left" bgcolor="#edf2e6" width="15%"><b><font color="#A2921E" face="Verdana" size="2">&nbsp;</font><font face="Verdana"
                        size="2" color="#A2921E">From :&nbsp;</font></b><input type="text" id="txtFromDate" runat="server" size="10" name="txtFromDate" onkeyup="this.value = this.value.replace(/[^\/]/g, '');"
                                    onclick="popupCalender('txtFromDate')" onkeypress="return false;"/>
                       
              
              <b><font color="#A2921E" face="Verdana" size="2">&nbsp;</font><font face="Verdana"
                        size="2" color="#A2921E">To :&nbsp;</font></b><input type="text" id="txtToDate" runat="server" size="10" name="txtToDate" onkeyup="this.value = this.value.replace(/[^\/]/g, '');"
                                    onclick="popupCalender('txtToDate')" onkeypress="return false;"/>&nbsp;&nbsp;&nbsp;
                                       <asp:Button ID="btnSearchby" runat="server" Text="Search" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" onClick="btnSearch_Click"/>
        </td>
        </tr>
        </table>
        <br />
        <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" 
            bordercolor="#EAEAEA" width="100%" id="AutoNumber1" height="1">
            <%If dd_prj_name.SelectedValue <> -1 Then%>
            <tr>
                <td width="18%" height="22" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer Name:</font></td>
                <td width="28%" height="22" colspan="3" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_name)%>
                    </font>
                </td>
                <td width="22%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer E-mail</font></td>
                <td width="44%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_email)%>
                    </font>
                </td>
            </tr>
            <tr>
                <td width="18%" height="22" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer Company :</font></td>
                <td width="28%" height="22" colspan="3" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(comp_name)%>
                    </font>
                </td>
                <td width="22%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;Project Name :</font></td>
                <td width="44%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_prjname)%>
                    </font>
                </td>
            </tr>
            <tr>
                <td width="18%" height="22" valign="top">
                    <font face="Verdana" size="2">&nbsp;Customer Address :</font></td>
                <td width="28%" height="22" colspan="3" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(cust_add)%>
                    </font>
                </td>
                <td width="22%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;Registration Date :</font></td>
                <td width="44%" height="22" colspan="2" valign="top">
                    <font face="Verdana" size="2">&nbsp;<%Response.Write(Day(cust_regdate) & "-" & Left(MonthName(Month(cust_regdate)), 3) & "-" & Year(cust_regdate))%>
                    </font>
                </td>
            </tr>
        </table>
        <%End If%>
           <asp:DataGrid ID="MyDataGrid" runat="server" BorderColor="" Font-Size="10pt" Font-Name="Verdana"
                BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False" FooterStyle-HorizontalAlign="Right"
                HeaderStyle-Font-Size="10pt" AllowSorting="True" HeaderStyle-BackColor="#edf2e6"
                ForeColor="#A2921E" Font-Bold="True" OnSortCommand="SortCommand_OnClick" CellPadding="0"
                Width="100%" OnItemCommand="ItemCommand" OnItemDataBound="ItemDataBound">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <Columns>
                    <asp:BoundColumn Visible="true" ItemStyle-Width="8%" DataField="chgId" SortExpression="chgId"
                        HeaderText="CR_Id"></asp:BoundColumn>
                    <asp:BoundColumn DataField="chgProjId" HeaderText="projID" Visible="false"></asp:BoundColumn>
                     <asp:BoundColumn  DataField="paymentCRID" HeaderText="projID" Visible="false"></asp:BoundColumn>
                    <asp:BoundColumn DataField="chgDate" SortExpression="chgDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yy}">
                        <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="projName" HeaderText="Project Name">
                        <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                    </asp:BoundColumn>
                   <%-- <asp:BoundColumn DataField="chgTitle" HeaderText="Title">
                        <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                    </asp:BoundColumn>--%>
                     <asp:HyperLinkColumn HeaderText="Title" HeaderStyle-Width="15%" DataNavigateUrlField="chgProjId" DataNavigateUrlFormatString="paymentSummary.aspx?projId={0}"
                    ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" DataTextFormatString="{0:##,###,###}" DataTextField="chgTitle" />
                    <asp:BoundColumn DataField="chgApproved" HeaderText="Status">
                        <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="chgEstTime" HeaderText="Estimated Time (Days)">
                        <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="chgEstCost" HeaderText="Estimated Cost">
                        <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="chgcompDate" HeaderText="Completion Date" DataFormatString="{0:dd-MMM-yy}">
                        <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="middle"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="">
                        <ItemStyle Width="12%" Wrap="false" HorizontalAlign="center" VerticalAlign="middle">
                        </ItemStyle>
                        <ItemTemplate>
                            <asp:Button ID="view" runat="server" CommandName="view" Text="View" align="right"
                                Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                                background-color: #C5D5AE" Font-Bold="true" Width="75"></asp:Button><br />
                            
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="">
                        <ItemStyle Width="13%" Wrap="false" HorizontalAlign="center" VerticalAlign="middle">
                        </ItemStyle>
                        <ItemTemplate>
                            <asp:Button ID="delete" runat="server" CommandName="delete" Text="Delete" align="right"
                                Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                                background-color: #C5D5AE" Font-Bold="true" Width="75"></asp:Button>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            <asp:HiddenField ID="HV1" runat="server"></asp:HiddenField>
    </form>
</body>
</html>
