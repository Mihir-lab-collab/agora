<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pay Process</title>

    <script language="VB" runat="server">
        Dim conn As New SqlConnection
        Dim Cmd As New SqlCommand()
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSQL As String
        Dim flg As Integer
        Public inttest As Integer
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            gf.checkEmpLogin()
            If Not IsPostBack Then
                If Session("dynoAdminSession") = 1 Then
                    Session.Add("dynoflgsession", "0")
                    Session.Add("dynoprocessSession", "0")
                    Session.Add("dynoprocesscountSession", "0")
                    Call bindgrid()
                End If
            End If
        End Sub

        Sub bindgrid()
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
            Dim da As SqlDataAdapter = New SqlDataAdapter("select max(employeemaster.empname) as empname,employeepaymaster.empid,max(employeepaymaster.emppaybasic)as basic,max(employeepaymaster.empPayHra)as Hra,max(employeepaymaster.emppayConveyance)as Conveyance,max(employeepaymaster.empPayMedical)as Medical,max(employeepaymaster.empPayFood)as Food,max(employeepaymaster.empPaySpecial)as Special,max(employeepaymaster.empPayPF)as PF,max(employeepaymaster.empPayEPF)as EPF,max(employeepaymaster.empPayLTA)as LTA,max(employeepaymaster.empPayPT)as PT,(max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)+max(employeepaymaster.emppaypf)+max(employeepaymaster.emppayepf)) as totalctc,(max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)+max(employeepaymaster.emppaypf)) as totalgross,(max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)+max(employeepaymaster.emppaypf)+max(employeepaymaster.emppayepf)-max(employeepaymaster.emppaypf)-max(employeepaymaster.emppaypt)-max(employeepaymaster.emppayepf)) as totalnet from employeepaymaster,employeemaster where employeepaymaster.empid=employeemaster.empid and employeemaster.empleavingdate is Null group by employeepaymaster.empid having max(employeepaymaster.emppaybasic) < 70000 ", conn)
            Dim ds As New DataSet()
            da.Fill(ds)
            dgrdprocess.DataSource = ds
            Session("dynoProcessCountsession") = (ds.Tables(0).Rows.Count)
            hdnid.Value = Session("dynoProcessCountsession")
            dgrdprocess.DataBind()
        End Sub

        Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        End Sub

        Sub dgrdProcess_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        End Sub

        Sub btnadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim str As String
            Dim strSql As String
            Dim insertsql As String
            Dim ddate As String
            Dim psql As String
            Dim strpayid As String
	 
            ddate = DateTime.Now
            str = PayNotes.Value
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
            strSql = "select * from employeePayProcess where month(payDate) ='" & Month(ddate) & "'"

            Dim cmdcheck As SqlCommand = New SqlCommand(strSql, conn)
            Dim drcheck As SqlDataReader
            drcheck = cmdcheck.ExecuteReader

                
            If (drcheck.Read) Then
                Dim strcheck As String = drcheck("payDate")
                Session.Item("dynoProcesssession") = strcheck
	
            End If
         
            If (Session.Item("dynoProcesssession")) <> "0" Then
                Dim tp As String = ""
                tp += "<Script language=JavaScript>"
                tp += " alert('Salary Generation For The Month is Already Done '); "
                tp += "</" + "script>"
                ClientScript.RegisterStartupScript(Me.GetType, "script123", tp)
            End If
	
            If Session.Item("dynoProcesssession") = "0" Then

                insertsql = "insert into employeePayProcess (payComment,payDate) Values('" & Trim(Replace(PayNotes.Value, "'", "''")) & "' ,getdate())"

                conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                conn.Open()
                Dim paddcmd As SqlCommand = New SqlCommand(insertsql, conn)
 
                paddcmd.Connection = conn
                paddcmd.CommandText = insertsql
                paddcmd.ExecuteNonQuery()
           
				  
                psql = "select * from employeePayProcess where payDate ='" & ddate & "'"
                Dim cmd As SqlCommand = New SqlCommand(psql, conn)
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader

              
                If (dr.Read) Then
                    Dim str1 As Integer = dr("payID")
                    Session.Item("dynoflgsession") = str1
                End If
			
                strpayid = (Session.Item("dynoflgsession"))
                dr.Close()


      

                Dim dgi As DataGridItem
                For Each dgi In dgrdprocess.Items

                    Dim txtEmpName As New TextBox
                    Dim txtEmpId As New TextBox
                    Dim txtBasic As New HtmlInputText
                    Dim txtHra As New HtmlInputText
                    Dim txtConveyance As New HtmlInputText
                    Dim txtMedical As New HtmlInputText
                    Dim txtFood As New HtmlInputText
                    Dim txtSpecial As New HtmlInputText
                    Dim txtLTA As New HtmlInputText
                    Dim txtPF As New HtmlInputText
                    Dim txtEPF As New HtmlInputText
                    Dim txtAT As New HtmlInputText
                    Dim txtPT As New HtmlInputText
                    Dim txtLoan As New HtmlInputText
                    Dim txtAdvance As New HtmlInputText
                    Dim txtLeave As New HtmlInputText
                    Dim txtDeduction As New HtmlInputText
                    Dim txtBonus As New HtmlInputText
                    Dim txtAddition As New HtmlInputText
                    Dim txtRemarks As New TextBox




                    txtEmpName = CType(dgi.FindControl("txtEmpName"), TextBox)
                    txtEmpId = CType(dgi.FindControl("txtempId"), TextBox)
                    txtBasic = CType(dgi.Cells(2).FindControl("basic"), HtmlInputText)

                    txtHra = CType(dgi.FindControl("HRA"), HtmlInputText)
                    txtConveyance = CType(dgi.FindControl("Conveyance"), HtmlInputText)
                    txtMedical = CType(dgi.FindControl("Medical"), HtmlInputText)
                    txtFood = CType(dgi.FindControl("Food"), HtmlInputText)
                    txtSpecial = CType(dgi.FindControl("Special"), HtmlInputText)
                    txtLTA = CType(dgi.FindControl("LTA"), HtmlInputText)
                    txtPF = CType(dgi.FindControl("PF"), HtmlInputText)
                    txtEPF = CType(dgi.FindControl("EPF"), HtmlInputText)
                    txtAT = CType(dgi.FindControl("AT"), HtmlInputText)
                    txtPT = CType(dgi.FindControl("PT"), HtmlInputText)
                    txtLoan = CType(dgi.FindControl("LOAN"), HtmlInputText)
                    txtAdvance = CType(dgi.FindControl("ADVANCE"), HtmlInputText)
                    txtLeave = CType(dgi.FindControl("LEAVE"), HtmlInputText)
                    txtDeduction = CType(dgi.FindControl("DOthers"), HtmlInputText)
                    txtBonus = CType(dgi.FindControl("Bonus"), HtmlInputText)
                    txtAddition = CType(dgi.FindControl("AOthers"), HtmlInputText)
                    txtRemarks = CType(dgi.FindControl("Remarks"), TextBox)

  
                    Dim strEmpName As String
                    Dim strEmpId As String
                    Dim strBasic As String
                    Dim strHra As String
                    Dim strConveyance As String
                    Dim strMedical As String
                    Dim strFood As String
                    Dim strSpecial As String
                    Dim strLTA As String
                    Dim strPF As String
                    Dim strEPF As String
                    Dim strAT As String
                    Dim strPT As String
                    Dim strLoan As String
                    Dim strAdvance As String
                    Dim strLeave As String
                    Dim strDeduction As String
                    Dim strBonus As String
                    Dim strAddition As String
                    Dim strRemarks As String

                    strEmpName = txtEmpName.Text
                    strEmpId = txtEmpId.Text
                    strBasic = txtBasic.Value
                    strHra = txtHra.Value
                    strConveyance = txtConveyance.Value
                    strMedical = txtMedical.Value
                    strFood = txtFood.Value
                    strSpecial = txtSpecial.Value
                    strLTA = txtLTA.Value
                    strPF = txtPF.Value
                    strEPF = txtEPF.Value
                    strAT = txtAT.Value
                    strPT = txtPT.Value
                    strLoan = txtLoan.Value
                    strAdvance = txtAdvance.Value
                    strLeave = txtLeave.Value
                    strDeduction = txtDeduction.Value
                    strBonus = txtBonus.Value
                    strAddition = txtAddition.Value
                    strRemarks = txtRemarks.Text

	
                    strSql = "insert into employeepayprocessdetail (payid,empid,PayBasic,PayHra,PayConveyance,PayMedical,PayFood,PaySpecial,payLTA,PayPF,PayEPF,PayAT,PayPT,payLoanInstl,payBonus,payAdvance,payLeave,payDeduction,payOthers,payremark) Values (" & strpayid & "," & strEmpId & ",'" & strBasic & "','" & strHra & "','" & strConveyance & "','" & strMedical & "','" & strFood & "','" & strSpecial & "','" & strLTA & "','" & strPF & "','" & strEPF & "','" & strAT & "','" & strPT & "','" & strLoan & "','" & strBonus & "','" & strAdvance & "','" & strLeave & "','" & strDeduction & "','" & strAddition & "','" & strRemarks & "')"


                    conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                    conn.Open()
                    Dim addcmd As SqlCommand = New SqlCommand(strSql, conn)
            
                    addcmd.Connection = conn
                    addcmd.CommandText = strSql
                    addcmd.ExecuteNonQuery()

                Next

                Dim tp As String = ""
                tp += "<Script language=JavaScript>"
                tp += " alert('Record Saved '); "
                tp += "</" + "script>"
                ClientScript.RegisterStartupScript(Me.GetType, "script123", tp)

            End If
        End Sub



    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table id="Table3" cellspacing="0" cellpadding="0" width="100%" align="left">
            <tr valign="top" bgcolor="#EDF2E6">
                <td colspan="2">
                    <font face="Arial" color="#A2921E"><b><span style="font-size: 14pt">Pay Comment</span></b></font><br />
                </td>
            </tr>
            <tr valign="top">
                <td width="100%" colspan="2" align="left" bgcolor="#edf2e6">
                    <textarea id="PayNotes" runat="server" rows="2" cols="70" name="Textarea1"></textarea>
                </td>
                <td colspan="2">
                    <font face="Arial" color="#A2921E"></font>
                    <asp:Button ID="btnadd" OnClick="btnadd_Click" runat="server" Width="90px" Text="Submit"
                        Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                        background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Width="90px"
                        Text="Cancel" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                        background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            </tr>
            <tr>
                <td align="left" bgcolor="#c5d5ae" colspan="4">
                    <b><font face="Verdana" color="#a2921e" size="2"></font></b>
                    <asp:Label ID="LBLprocess" runat="server" Width="100%" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<-----------------------------------------DEDUCTION-------------------------------><---ADDITION---->"> 
                    </asp:Label>
                </td>
            </tr>
            <tr>
            </tr>
            <tr height="60%" valign="top">
                <td height="100%">
                    <asp:DataGrid ID="dgrdprocess" CssClass="text" DataKeyField="empId" runat="server"
                        BorderColor="" Font-Size="10pt" Font-Name="Verdana" BackColor="White" Font-Names="Verdana"
                        AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" Width="100%"
                        HeaderStyle-Font-Size="10pt" HeaderStyle-ForeColor="#A2921E" HeaderStyle-Font-Bold="True"
                        AllowSorting="True" HeaderStyle-BackColor="#C5D5AE" CellPadding="0" OnItemDataBound="dgrdProcess_ItemDataBound"
                        PageSize="25" AllowPaging="True">
                        <ItemStyle BackColor="#FFFFEE"></ItemStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    EMPNAME</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E" ID="txtempname"
                                        ReadOnly="true" runat="server" Text='<%# container.dataitem("empname")%>'> 

                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    EMPID</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E" Width="60px"
                                        ID="txtEmpId" ReadOnly="true" runat="server" Text='<%# container.dataitem("empId")%>'>  
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    BASIC</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="basic" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("basic")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    HRA</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Hra" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Hra")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    CONVEY</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Conveyance" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 60px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Conveyance")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    MEDICAL</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Medical" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 65px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Medical")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    FOOD</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Food" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Food")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    SPECIAL</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Special" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 61px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Special")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    LTA</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Lta" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Lta")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    PF</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Pf" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Pf")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    EPF</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="EPf" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("EPf")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    AT</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="AT" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    PT</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="PT" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("PT")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    LOAN</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="LOAN" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    ADVANCE</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Advance" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 68px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    LEAVE</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Leave" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    DEDUCTION</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Dothers" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 83px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    BONUS</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Bonus" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    ADDITION</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Aothers" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 70px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    REMARKS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="100px" ID="Remarks" runat="server">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    CTC</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="CTC" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("TotalCTC")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    GROSS</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="GROSS" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("TotalGROSS")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    NET SALARY</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="NetSalary" runat="server" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("TotalNet")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle Mode="Numericpages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <input type="hidden" id="hdnid" runat="server" />
        <input type="hidden" id="bas" runat="server" />
    </form>
</body>
</html>
