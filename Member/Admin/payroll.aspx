<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Drawing" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pay Summary</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

    <script language="VB" runat="server">
        Dim objCommon As New clsCommon()
        Dim SortField As String
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim EmpName, empPayMonth, empPayYear As String
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            gf.checkEmpLogin()
            If Not IsPostBack Then
                
                If SortField = "" Then
                    SortField = "empid"
                End If
                
                'Session("dynoAdminSession") = 1
                If Session("dynoAdminSession") = 1 Then
                    Session.Add("dynoMonthsession", "0")
                    Dim userDetail As New UserDetails
                    userDetail = Session("DynoEmpSessionObject")
                    
                    If userDetail.ProfileID <> "" Then
                        hdLocationId.Value = objCommon.GetLocationAcess(userDetail.ProfileID).ToString()
                        'hdHasAllLocationAcess.Value = (hdLocationId.Value.Equals("0") ? "1" : "0")
                    End If
                    Call BindLocation()
                    Call bindgrid()
                    
                End If
                
            End If
        End Sub

        Sub bindgrid()
            If hdLocationId.Value <> "0" Then
                Dim location As String = objCommon.GetLocationName(Convert.ToInt32(hdLocationId.Value))
                Session("DivLocation") = location
                'dlLocation.SelectedItem.ToString()
                Session("DivLocationID") = Convert.ToInt32(hdLocationId.Value)
                'dlLocation.SelectedValue
            Else
                Session("DivLocation") = dlLocation.SelectedItem.ToString()
                Session("DivLocationID") = dlLocation.SelectedValue
            End If
            
            'If hdnStatus.Value = "Inactive" Then
            'Dim conpay As New SqlConnection
            'Dim cmdpay As New SqlCommand
            'Dim dstpay As New DataSet
            'Dim dadpay As New SqlDataAdapter(cmdpay)
            'Dim sql As String
                
            'conn.Open()
            'cmdpay.Connection = conn
            'cmdpay.CommandType = CommandType.Text
               
            'sql = "select max(employeemaster.empname) as empname,CONVERT(CHAR(11),employeemaster.empJoiningDate) as empJdate,employeemaster.empExperince,employeepaymaster.empid,max(employeepaymaster.emppaybasic)as emppaybasic,max(employeepaymaster.emppaymonth) as emppayMonth,max(employeepaymaster.emppayyear) as emppayYear,((((max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)+max(employeepaymaster.emppaybasic)*12/100))*12 )+ max(employeepaymaster.empPayBonus)) as Annualtotalctc, (max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)+max(employeepaymaster.emppaybasic)*12/100) as totalctc,(max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)) as totalgross,(max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)-max(employeepaymaster.emppaybasic)*12/100-max(employeepaymaster.empPT+employeepaymaster.empInsurance)) as totalnet from employeepaymaster, employeemaster where employeepaymaster.empid=employeemaster.empid and employeemaster.empleavingdate is not Null  "

            'If hdLocationId.Value <> "0" Then
            '    sql = sql + " and employeemaster.LocationFKID = '" & hdLocationId.Value & "'"
            'End If
            'sql = sql + " group by employeepaymaster.empid,employeemaster.empJoiningDate,employeemaster.empExperince  having max(employeepaymaster.emppaybasic) < 70000 "

            'cmdpay.CommandText = sql
                    
            'dadpay.Fill(dstpay)
            'DGrdpayroll.DataSource = dstpay
            'DGrdpayroll.DataBind()
            'conpay.Close()
            'cmdpay.Dispose()
            'Else
                
            Dim Cmd As New SqlCommand
            Dim ds As New DataSet

            Dim sql As String = ""
            Dim strDate As String = Format(DateAdd("d", -Day(Now) + 1, DateAdd("m", -1, Now())), "dd-MMM-yy")

            conn.Open()
            Cmd.CommandText = CommandType.Text
            'sql = "select (employeemaster.empname) as empname,CONVERT(CHAR(11),employeemaster.empJoiningDate) as empJdate,employeemaster.empExperince,(employeepaymaster.emppaybasic)as emppaybasic,(employeepaymaster.emppaymonth) as emppayMonth,(employeepaymaster.emppayyear) as emppayYear,employeemaster.empid,      ((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualtotalCTC,           floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as totalctc,         floor( (select Case when ispf=1 then (employeepaymaster.emppaybasic * 100/40)  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as totalgross,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic * 100/40))-((employeepaymaster.emppaybasic * 12/100)) -(employeepaymaster.empPT+employeepaymaster.empInsurance) else ( ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) -(employeepaymaster.empPT+employeepaymaster.empInsurance) ) end  from employeemaster where empid=employeepaymaster.empid))as totalnet   from employeepaymaster RIGHT JOIN employeemaster ON employeepaymaster.empid=employeemaster.empid where (employeemaster.empleavingdate is NULL OR employeemaster.empleavingdate >'" & strDate & "')  "

            sql = "select (employeemaster.empname) as empname,CONVERT(CHAR(11),employeemaster.empJoiningDate) as empJdate," +
                "employeemaster.empExperince,(employeepaymaster.emppaybasic)as emppaybasic,(employeepaymaster.emppaymonth) " +
                "as emppayMonth,(employeepaymaster.emppayyear) as emppayYear,employeemaster.empid, " +
                "((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+" +
                "(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+" +
                "(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end " +
                "from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualtotalCTC," +
                "Round((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+" +
                "(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+" +
                "(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then " +
                "employeepaymaster.empPayBasic*12/100 else 0 end from employeemaster where empid=employeepaymaster.empid)) " +
                "else ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+" +
                "(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+" +
                "(employeepaymaster.emppaylta)) end from employeemaster where empid=employeepaymaster.empid),0) as totalctc, " +
                "Round((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+" +
                "(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+" +
                "(employeepaymaster.emppaylta),0) as totalGross, Round((employeepaymaster.emppaybasic)+" +
                "(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+" +
                "(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta) - " +
                "(empPT + empInsurance + (select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end from " +
                "employeemaster where empid=employeepaymaster.empid)),0) as totalNet from employeepaymaster RIGHT JOIN " +
                "employeemaster ON employeepaymaster.empid=employeemaster.empid WHERE "
            
            If hdnStatus.Value = "Inactive" Then
                sql = sql + " employeemaster.empleavingdate IS NOT NULL "
            Else
                sql = sql + " (employeemaster.empleavingdate is NULL OR employeemaster.empleavingdate > '" & strDate & "')"
            End If
            
            If hdLocationId.Value <> "0" Then
                sql = sql + "  AND employeemaster.LocationFKID = '" & hdLocationId.Value & "'"
            End If
            
            Dim da As SqlDataAdapter = New SqlDataAdapter(sql, conn)
	
            da.Fill(ds)

            DGrdpayroll.DataSource = ds
            DGrdpayroll.DataBind()
            conn.Close()
            Cmd.Dispose()

            'End If
        End Sub

        Sub dgrdpayroll_Sort(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
            SortField = e.SortExpression
            bindgrid()
        End Sub
		
        Sub dgrdPayroll_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
            Dim strY As String
            If e.Item.Cells(3).Text = Trim(Str(1)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "JAN - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(2)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "FEB - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(3)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "MAR - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(4)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "APR - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(5)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "MAY - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(6)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "JUN - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(7)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "JUL - " & strY
                'End If
            ElseIf e.Item.Cells(3).Text = Trim(Str(8)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "AUG - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(9)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "SEP - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(10)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "OCT - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(11)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "NOV - " & strY
            End If
            If e.Item.Cells(3).Text = Trim(Str(12)) Then
                strY = (e.Item.Cells(4).Text)
                e.Item.Cells(3).Text = "DEC - " & strY
            End If
            ' End if
	  
        End Sub


        Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)

            If e.CommandName = "detail" Then
                Dim intId As String = e.Item.Cells(0).Text
                Dim strname As String = e.Item.Cells(1).Text
                Dim intbasic As String = e.Item.Cells(8).Text

    
                hdnid.Value = intId
                hdname.Value = strname
                hdbasic.Value = intbasic

    
                Dim sp As String
                sp = "<Script language=JavaScript>"
                sp += "var Id=document.forms[0].hdnid.value;"
                sp += "var ename=document.forms[0].hdname.value;"
                sp += "var ebasic=document.forms[0].hdbasic.value;"
                sp += "window.open('payrolldetails.aspx?empId='+ Id +'&empname='+ ename + '&empPayBasic='+ ebasic,"
                sp += "'popupwindow',"
                sp += "'width=1000,height=1000,left=0,top=0,scrollbars=yes,menubar=no,addressbar=no,toolbar=no,status=no,resizable=yes'); "
                sp += "</" + "script>"
                ClientScript.RegisterStartupScript(Me.GetType, "script123", sp)

            End If
	
        End Sub

        Function getRevDate(ByVal monthStr As String, ByVal yearStr As String) As String
            Dim strDate As String = ""
            If IsNumeric(monthStr) Then
                strDate = Left(MonthName(monthStr), 3) & " " & yearStr
            End If
            Return strDate
        End Function

        Sub dlLocation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            hdLocationId.Value = dlLocation.SelectedValue
            Call bindgrid()
        End Sub

        Sub BindLocation()

            Dim dtEmployeeLocation As DataTable = New DataTable()
            dtEmployeeLocation = objCommon.EmployeeLocationList()
            If (hdLocationId.Value.Equals("0")) Then
       
                dlLocation.DataSource = dtEmployeeLocation
                dlLocation.DataTextField = "Name"
                dlLocation.DataValueField = "LocationID"
                dlLocation.DataBind()
                dlLocation.SelectedValue = dtEmployeeLocation.Select("Name='Mumbai'").FirstOrDefault()("LocationID").ToString()
                hdLocationId.Value = dlLocation.SelectedValue
                dlLocation.Visible = True
                lblLocation.Visible = True
            Else
                lblLocationId.Visible = True
                lblLocation.Visible = True
                dlLocation.Visible = False
                Dim location As String = objCommon.GetLocationName(Convert.ToInt32(hdLocationId.Value))
                lblLocationId.Text = location
            End If
        End Sub
        
        Public Function CalculateTotalExp(strd2 As String, intPreviousExpInMonths As Integer, blnAdd As [Boolean]) As String
            If String.IsNullOrEmpty(strd2.Trim()) Then
                Return ""
            End If
            ' compute & return the difference of two dates,
            ' returning years, months & days
            ' d1 should be the larger (newest) of the two dates
            ' we want d1 to be the larger (newest) date
            ' flip if we need to
            Dim d1 As DateTime = DateTime.Now

            Dim strDate As String() = strd2.Split(New String() {" ", "-", "/"}, StringSplitOptions.None)



            Dim d2 As New DateTime(Convert.ToInt16(strDate(2)), Convert.ToInt16(strDate(1)), Convert.ToInt16(strDate(0)), 0, 0, 0)

            If d1 < d2 Then
                Dim d3 As DateTime = d2
                d2 = d1
                d1 = d3
            End If
            Dim intMonth, intDays, intYears As Integer
            ' compute difference in total months
            intMonth = intPreviousExpInMonths
            If blnAdd Then
                intMonth = 12 * (d1.Year - d2.Year) + (d1.Month - d2.Month) + intPreviousExpInMonths
            End If
            ' based upon the 'days',
            ' adjust months & compute actual days difference
            If d1.Day < d2.Day Then
                ' intMonth--;
                intDays = DateTime.DaysInMonth(d2.Year, d2.Month) - d2.Day + d1.Day
            Else
                intDays = d1.Day - d2.Day
            End If
            ' compute years & actual months
            intYears = intMonth / 12
            intMonth = Math.Abs(intMonth - (intYears * 12))

            Dim strExp As String = String.Empty

            If intYears = 0 AndAlso intMonth <> 0 Then
                If intMonth > 1 Then
                    strExp = intMonth.ToString() & " Months"
                Else
                    strExp = intMonth.ToString() & " Month"
                End If
            ElseIf intYears <> 0 AndAlso intMonth = 0 Then
                If intYears > 1 Then
                    strExp = intYears.ToString() & " Years"
                Else
                    strExp = intYears.ToString() & " Year"
                End If
            ElseIf intYears <> 0 AndAlso intMonth <> 0 Then
                If intYears > 1 AndAlso intMonth > 1 Then
                    strExp = intYears.ToString() & " Years " & intMonth.ToString() & " Months"
                ElseIf intYears = 1 AndAlso intMonth > 0 Then
                    strExp = intYears.ToString() & " Year " & intMonth.ToString() & " Months"
                ElseIf intYears > 1 AndAlso intMonth = 1 Then
                    strExp = intYears.ToString() & " Years " & intMonth.ToString() & " Month"
                End If
            End If
            Return strExp
        End Function
        
        Protected Sub lnkActive_Click(sender As Object, e As EventArgs)
            hdnStatus.Value = "active"
            lnkActive.ForeColor = Drawing.Color.Black
            lnkInactive.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
            lnkPayProcess.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
            Call bindgrid()
        End Sub

        Protected Sub lnkInactive_Click(sender As Object, e As EventArgs)
            hdnStatus.Value = "Inactive"
            lnkActive.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
            lnkInactive.ForeColor = Drawing.Color.Black
            lnkPayProcess.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
            
            Call bindgrid()
        End Sub

        Protected Sub lnkPayProcess_Click(sender As Object, e As EventArgs)
            lnkActive.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
            lnkInactive.ForeColor = System.Drawing.ColorTranslator.FromHtml("#A2921E")
            Response.Redirect("payProcess.aspx")
        End Sub
    </script>

</head>
<body>
    <ucl:adminMenu ID="adminMenu" runat="server" />

    <form id="Form1" method="post" runat="server">
        <table cellpadding="4" width="100%" border="1" style="border-collapse: collapse"
            bordercolor="#E8E8E8" bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8">
            <tr>
                <td align="right" colspan="4" bgcolor="#edf2e6">
                    <b><font face="Verdana" size="2">
                    <p align="left">
                        <asp:LinkButton ID="lnkActive" runat="server" OnClick="lnkActive_Click" Text="Active" ForeColor="#A2921E"></asp:LinkButton>| 
                    <asp:LinkButton ID="lnkInactive" runat="server" OnClick="lnkInactive_Click" Text="Inactive" ForeColor="#A2921E"></asp:LinkButton>|
                    <a href="javascript: payDisp();" id="lnlAddNewRev" style="color: #A2921E">Add New Revision</a>|
                        <asp:LinkButton ID="lnkPayProcess" runat="server" OnClick="lnkPayProcess_Click" Text="Pay Process" ForeColor="#A2921E"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <div style="text-align: right; background-color: #edf2e6; color: #A2921E; font-family: Verdana; font-size: 14;">
            <b>
                <asp:Label ID="lblLocation" Text="Location:" runat="server" Visible="false" /></b>
            <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="b_dropdown" OnSelectedIndexChanged="dlLocation_SelectedIndexChanged" Visible="false" AppendDataBoundItems="true">
                <asp:ListItem Text="All" Value="0"></asp:ListItem>
            </asp:DropDownList>
            <b><font face="Verdana" color="#a2921e" size="2">  <asp:Label ID="lblLocationId" runat="server" Visible="false"/></font></b>
        </div>
        <div></div>
        <%  Dim i As Integer
            i = 3
        %>
        <asp:DataGrid ID="DGrdpayroll" runat="server" AllowSorting="True" Width="100%" BackColor="White"
            BorderColor="Black" ShowFooter="True" CellPadding="2" Font-Name="Verdana"
            Font-Size="10pt" HeaderStyle-ForeColor="#A2921E" HeaderStyle-BackColor="#edf2e6"
            HeaderStyle-Font-Size="11pt" HeaderStyle-Font-Bold="True" MaintainState="true"
            FooterStyle-HorizontalAlign="Right" AutoGenerateColumns="False" OnSortCommand="DGrdpayroll_Sort"
            OnItemCommand="ItemCommand" OnItemDataBound="dgrdPayroll_ItemDataBound" Font-Names="Verdana">
            <AlternatingItemStyle BackColor="#edf2e6" />
            <Columns>
                <asp:TemplateColumn HeaderText="Sr Nn">
                    <ItemTemplate>
                        <%# Container.ItemIndex+1%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Employee ID" DataField="empid" />
                <asp:BoundColumn HeaderText="Employee Name" DataField="empname" />
                <asp:BoundColumn HeaderText="Joining Date" DataField="empJdate" />
                <asp:TemplateColumn HeaderText="Total Experience">
                    <ItemTemplate>
                        <%# CalculateTotalExp(Convert.ToDateTime(Container.DataItem("empJdate").ToString()).ToString("dd-MM-yyyy"), Container.DataItem("empExperince").ToString(), True)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Revision Month">
                    <ItemTemplate>
                        <%#getRevDate(Container.DataItem("empPayMonth").ToString(), Container.DataItem("empPayYear").ToString())%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Annual CTC (INR)" DataField="AnnualtotalCTC" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="CTC (INR)" DataField="totalCTC" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Gross Salary (INR)" DataField="totalgross" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Net Salary (INR)" DataField="totalnet" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <input type="button" width="90" value="Detail" onclick="getDetail(<%#Container.dataitem("empId")%>, '<%#Container.dataitem("empName")%>    ')"
                            style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>

            <FooterStyle HorizontalAlign="Right"></FooterStyle>

            <HeaderStyle BackColor="#EDF2E6" Font-Bold="True" Font-Size="11pt" ForeColor="#A2921E"></HeaderStyle>
        </asp:DataGrid>
        <input type="hidden" id="detail" name="detail">
        <input type="hidden" id="hdnid" runat="server">
        <input type="hidden" id="hdname" runat="server">
        <input type="hidden" id="hdbasic" runat="server">
        <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnStatus" runat="server" Value="" />
    </form>
</body>
<script src="../JSController/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="../JSController/ScrollableGridPlugin.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
 /*   $(document).ready(function () {
        $('#<%=DGrdpayroll.ClientID %>').Scrollable({
            ScrollHeight: 900,
            IsInUpdatePanel: true
        });
    });*/
    function getDetail(empId, empName)
    {
        pop = window.open('payrolldetails.aspx?empId=' + empId + '&empName=' + empName, 'popupwindow', 'width=1000,height=580,left=0,top=0,scrollbars=yes,menubar=no,addressbar=no,toolbar=no,status=no,resizable=yes');
        pop.focus()
    }
  
    function payDisp()
    {
        pop = window.open('addpayroll.aspx','popupwindow','scrollbars=yes,toolbar=no,menubar=no,width=945,height=550,left=20,top=10');
        pop.focus();       
    }
</script>

</html>
