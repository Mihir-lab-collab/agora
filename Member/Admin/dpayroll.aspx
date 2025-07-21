<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pay Process</title>

    <script language="javascript">

  function calAmount()

  {
  var Id;
  Id =document.getElementById("hdnid").value;
   
     var intbasic=0;
     var tbasic=0;
     var inthra=0;
     var intconveyance=0;
     var intmedical=0;
     var intfood=0;
     var intspecial=0;
     var intlta=0;
     var intpf=0;
     var intepf=0;
     var intat=0;
     var intpt=0;
     var intloan=0;
     var intadvance=0;
     var intleave=0;
     var intdeduction=0;
     var intbonus=0;
     var intothers=0;
     var totalctc=0;
	 var intctc=0;
	 var intgrosstotal=0;
     var intnettotal=0; 

   for(i = 0; i < Id; i++) 
	{
	  intbasic =Number(document.Form1.basic[i].value);
	  inthra   =Number(document.Form1.Hra[i].value);
	  intconveyance =Number(document.Form1.Conveyance[i].value);
      intmedical =Number(document.Form1.Medical[i].value);
      intfood =Number(document.Form1.Food[i].value);
      intspecial =Number(document.Form1.Special[i].value);
      intlta =Number(document.Form1.Lta[i].value);
      intpf =Number(document.Form1.Pf[i].value);
      intepf =Number(document.Form1.EPf[i].value);
      intat =Number(document.Form1.AT[i].value);
      intpt =Number(document.Form1.PT[i].value);
      intloan =Number(document.Form1.LOAN[i].value);
      intadvance =Number(document.Form1.Advance[i].value);
      intleave =Number(document.Form1.Leave[i].value);
      intdeduction =Number(document.Form1.Dothers[i].value);
      intbonus =Number(document.Form1.Bonus[i].value);
      intothers =Number(document.Form1.Aothers[i].value);


      intctc   =intbasic + inthra + intconveyance + intmedical + intfood + intspecial + intlta + intpf + intepf;
      document.Form1.CTC[i].value =intctc;

      intgrosstotal = intbasic + inthra + intconveyance + intmedical + intfood + intspecial + intlta + intpf;
	  
  	  document.Form1.GROSS[i].value =intgrosstotal;

      intnettotal =intctc - intepf - intpf - intpt - intat - intloan - intadvance - intleave - intdeduction + intbonus + intothers
	  document.Form1.NetSalary[i].value=intnettotal;
	}
}
    </script>

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

        Sub btnconfirmation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
 
            Dim daConfirm As SqlDataAdapter = New SqlDataAdapter("select ", conn)

            Dim dsConfirm As New DataSet()

            daConfirm.Fill(dsConfirm)
  
            dgrdprocess.DataSource = dsConfirm
            Session("dynoProcessCountsession") = (dsConfirm.Tables(0).Rows.Count)

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
                    Dim txtHra As New TextBox
                    Dim txtConveyance As New TextBox
                    Dim txtMedical As New TextBox
                    Dim txtFood As New TextBox
                    Dim txtSpecial As New TextBox
                    Dim txtLTA As New TextBox
                    Dim txtPF As New TextBox
                    Dim txtEPF As New TextBox
                    Dim txtAT As New TextBox
                    Dim txtPT As New TextBox
                    Dim txtLoan As New TextBox
                    Dim txtAdvance As New TextBox
                    Dim txtLeave As New TextBox
                    Dim txtDeduction As New TextBox
                    Dim txtBonus As New TextBox
                    Dim txtAddition As New TextBox
                    Dim txtRemarks As New TextBox

 

                    txtEmpName = CType(dgi.FindControl("txtEmpName"), TextBox)
                    txtEmpId = CType(dgi.FindControl("txtempId"), TextBox)
                    txtBasic = CType(dgi.Cells(2).FindControl("basic"), HtmlInputText)
                    Response.Write(txtBasic.Value)
                    Response.End()
                    txtHra = CType(dgi.FindControl("HRA"), TextBox)
                    txtConveyance = CType(dgi.FindControl("Conveyance"), TextBox)
                    txtMedical = CType(dgi.FindControl("Medical"), TextBox)
                    txtFood = CType(dgi.FindControl("Food"), TextBox)
                    txtSpecial = CType(dgi.FindControl("Special"), TextBox)
                    txtLTA = CType(dgi.FindControl("LTA"), TextBox)
                    txtPF = CType(dgi.FindControl("PF"), TextBox)
                    txtEPF = CType(dgi.FindControl("EPF"), TextBox)
                    txtAT = CType(dgi.FindControl("AT"), TextBox)
                    txtPT = CType(dgi.FindControl("PT"), TextBox)
                    txtLoan = CType(dgi.FindControl("LOAN"), TextBox)
                    txtAdvance = CType(dgi.FindControl("ADVANCE"), TextBox)
                    txtLeave = CType(dgi.FindControl("LEAVE"), TextBox)
                    txtDeduction = CType(dgi.FindControl("DOthers"), TextBox)
                    txtBonus = CType(dgi.FindControl("Bonus"), TextBox)
                    txtAddition = CType(dgi.FindControl("AOthers"), TextBox)
                    txtRemarks = CType(dgi.FindControl("Remarks"), TextBox)

  
                    Dim strEmpName As String
                    Dim strEmpId As String
                    Dim strBasic As String = 0
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
                    'strBasic = txtBasic.text
                    strHra = txtHra.Text
                    strConveyance = txtConveyance.Text
                    strMedical = txtMedical.Text
                    strFood = txtFood.Text
                    strSpecial = txtSpecial.Text
                    strLTA = txtLTA.Text
                    strPF = txtPF.Text
                    strEPF = txtEPF.Text
                    strAT = txtAT.Text
                    strPT = txtPT.Text
                    strLoan = txtLoan.Text
                    strAdvance = txtAdvance.Text
                    strLeave = txtLeave.Text
                    strDeduction = txtDeduction.Text
                    strBonus = txtBonus.Text
                    strAddition = txtAddition.Text
                    strRemarks = txtRemarks.Text

	
                    strSql = "insert into employeepayprocessdetail (payid,empid,PayBasic,PayHra,PayConveyance," & _
                    "PayMedical,PayFood,PaySpecial,payLTA,PayPF,PayEPF,PayAT,PayPT,payLoanInstl,payBonus,payAdvance," & _
                    "payLeave,payDeduction,payOthers,payremark) Values (" & strpayid & "," & strEmpId & ",'" & _
                    strBasic & "','" & strHra & "','" & strConveyance & "','" & strMedical & "','" & strFood & "','" & _
                    strSpecial & "','" & strLTA & "','" & strPF & "','" & strEPF & "','" & strAT & "','" & strPT & _
                    "','" & strLoan & "','" & strBonus & "','" & strAdvance & "','" & strLeave & "','" & _
                    strDeduction & "','" & strAddition & "','" & strRemarks & "')"


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
        <table id="Table3" cellspacing="0" cellpadding="0" width="100%" align="left" height="100%">
            <tr valign="top" height="10%" bgcolor="#EDF2E6">
                <td colspan="2" height="20">
                    <p align="left">
                        <font face="Arial" color="#A2921E"><b><span style="font-size: 14pt">Pay Comment</span></b></font><br>
                </td>
            </tr>
            <tr valign="top" height="10%">
                <td width="100%" colspan="2" align="left" bgcolor="#edf2e6">
                    <textarea id="PayNotes" runat="server" rows="2" cols="70" name="Textarea1"></textarea>
                </td>
                <tr width="100%" height="10%" colspan="2" align="left" bgcolor="#EDF2E6">
                    <td colspan="2" height="40">
                        <font face="Arial" color="#A2921E"></font>
                        <asp:Button ID="btnadd" OnClick="btnadd_Click" runat="server" Width="90px" Text="Submit"
                            Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                            background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Width="90px"
                            Text="Cancel" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                            background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnconfirmation" OnClick="btnconfirmation_Click" runat="server" Width="90px"
                            Text="Confirmation" Style="font-family: Verdana; font-size: 8pt; color: #A2921E;
                            font-weight: bold; background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                    <input onkeypress="return numbersonly(event)" id="basic" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("basic")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    HRA</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Hra" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Hra")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    CONVEY</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Conveyance" onblur="javascript:return calAmount();"
                                        style="width: 60px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Conveyance")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    MEDICAL</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Medical" onblur="javascript:return calAmount();"
                                        style="width: 65px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Medical")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    FOOD</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Food" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Food")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    SPECIAL</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Special" onblur="javascript:return calAmount();"
                                        style="width: 61px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Special")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    LTA</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Lta" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Lta")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    PF</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Pf" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("Pf")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    EPF</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="EPf" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("EPf")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    AT</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="AT" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    PT</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="PT" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("PT")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    LOAN</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="LOAN" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    ADVANCE</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Advance" onblur="javascript:return calAmount();"
                                        style="width: 68px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    LEAVE</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Leave" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    DEDUCTION</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Dothers" onblur="javascript:return calAmount();"
                                        style="width: 83px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    BONUS</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Bonus" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value="0"
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    ADDITION</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="Aothers" onblur="javascript:return calAmount();"
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
                                    <input onkeypress="return numbersonly(event)" id="CTC" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("TotalCTC")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    GROSS</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="GROSS" onblur="javascript:return calAmount();"
                                        style="width: 54px; height: 22px" type="text" maxlength="10" size="3" value='<%#  container.dataitem("TotalGROSS")%>'
                                        dataformatstring="{0:##,###,###}">
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    NET SALARY</HeaderTemplate>
                                <ItemTemplate>
                                    <input onkeypress="return numbersonly(event)" id="NetSalary" onblur="javascript:return calAmount();"
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
        <input type="hidden" id="hdnid" runat="server">
    </form>
</body>
</html>
