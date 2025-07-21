<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
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
 
        Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

            If Not IsPostBack Then
                If Session("dynoAdminSession") = 1 Then
                    Session.Add("dynoflgsession", "0")
                    Session.Add("dynoReportsession", "0")
                    Session.Add("dynoprocessSession", "0")
                    Session.Add("dynoprocesscountSession", "0")
                    Session.Add("dynoprocessbasicSession", "0")

		
                    Call bindgrid()
		
                End If
	
            End If

        End Sub


        Sub bindgrid()
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
 
            Dim da As SqlDataAdapter = New SqlDataAdapter("select max(employeemaster.empname) as empname,employeepaymaster.empid,max(employeepaymaster.emppaybasic)as basic,max(employeepaymaster.empPayHra)as Hra,max(employeepaymaster.emppayConveyance)as Conveyance,max(employeepaymaster.empPayMedical)as Medical,max(employeepaymaster.empPayFood)as Food,max(employeepaymaster.empPaySpecial)as Special,max(employeepaymaster.empPayPF)as PF,max(employeepaymaster.empPayEPF)as EPF,max(employeepaymaster.empPayLTA)as LTA,max(employeepaymaster.empPayPT)as PT,(max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)+max(employeepaymaster.emppaypf)+max(employeepaymaster.emppayepf)) as totalctc,(max(employeepaymaster.emppaybasic)+max(employeepaymaster.emppayhra)+max(employeepaymaster.emppayconveyance)+max(employeepaymaster.emppaymedical)+max(employeepaymaster.emppayfood)+max(employeepaymaster.emppayspecial)+max(employeepaymaster.emppaylta)+max(employeepaymaster.emppaypf)) as totalgross,0 as TotalDeduction,0 as totalnet from employeepaymaster,employeemaster where employeepaymaster.empid=employeemaster.empid and employeemaster.empleavingdate is Null group by employeepaymaster.empid having max(employeepaymaster.emppaybasic) < 70000 ", conn)

            Dim ds As New DataSet()

            da.Fill(ds)
  
            dgrdprocess.DataSource = ds
            Session("dynoProcessCountsession") = (ds.Tables(0).Rows.Count)

            hdnid.Value = Session("dynoProcessCountsession")

            dgrdprocess.DataBind()


        End Sub


        Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            'response.Redirect ("../admin/payProcess.aspx")

            Dim Id As String

            Id = "datagrid.dgrdprocess"

            'Id ="Table3"
            ' Response.Redirect('postForm2.aspx?dgd='+ Id )

            Dim sp As String
            sp = "<Script language=JavaScript>"
            sp += "var Id='dgrdprocess';"
            sp += "window.open('postForm2.aspx?dgd='+ Id ,"
            sp += "'popupwindow',"
            sp += "'width=745,left=0,top=0,scrollbars=yes,menubar=no,addressbar=no,toolbar=no,status=no,resizable=yes'); "
            sp += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "script123", sp)
        End Sub

        Sub btncalculation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            'DIM BTNPRINT AS new button

            ' btnPrint.Attributes.Add("Onclick", "javascript: getPrint();")

    
  
            Dim dtt As DataGridItem
            Dim intcnt As Integer
            intcnt = Session("dynoProcessCountsession")

            Dim valueempname As New ArrayList
            Dim valueempid As New ArrayList
            Dim valuebasic As New ArrayList
            Dim valuehra As New ArrayList
            Dim valueconveyance As New ArrayList
            Dim valuemedical As New ArrayList
            Dim valuefood As New ArrayList
            Dim valuespecial As New ArrayList
            Dim valuelta As New ArrayList
            Dim valueepf As New ArrayList
            Dim valuepf As New ArrayList
            Dim valueat As New ArrayList
            Dim valuept As New ArrayList
            Dim valueloan As New ArrayList
            Dim valueadvance As New ArrayList
            Dim valueleave As New ArrayList
            Dim valuededuction As New ArrayList
            Dim valuebonus As New ArrayList
            Dim valueaddition As New ArrayList
            Dim valueremarks As New ArrayList
            Dim valuectc As New ArrayList
            Dim valuegross As New ArrayList
            Dim valuenetsalary As New ArrayList
            Dim valuetotaldeduction As New ArrayList

            'dim Tbreport as string
            'Tbreport=request.form("valueempname(0)")



            'Dim coll As NameValueCollection

            Dim dt As New System.Data.DataTable

            dt.Columns.Add("EmpName")
            dt.Columns.Add("EmpId")
            dt.Columns.Add("Basic")
            dt.Columns.Add("HRA")
            dt.Columns.Add("Conveyance")
            dt.Columns.Add("Medical")
            dt.Columns.Add("Food")
            dt.Columns.Add("Special")
            dt.Columns.Add("LTA")
            dt.Columns.Add("EPF")
            dt.Columns.Add("PF")
            dt.Columns.Add("AT")
            dt.Columns.Add("PT")
            dt.Columns.Add("Loan")
            dt.Columns.Add("Advance")
            dt.Columns.Add("Leave")
            dt.Columns.Add("Deduction")
            dt.Columns.Add("Bonus")
            dt.Columns.Add("Addition")
            dt.Columns.Add("Remarks")
            dt.Columns.Add("CTC")
            dt.Columns.Add("Gross")
            dt.Columns.Add("NetSalary")
            dt.Columns.Add("TotalDeduction")

            For Each dtt In dgrdprocess.Items
  		
                Dim ttempname As New Label
                Dim ttempid As New Label
                Dim ttbasic As New TextBox
                Dim ttHra As New TextBox
                Dim ttConveyance As New TextBox
                Dim ttMedical As New TextBox
                Dim ttFood As New TextBox
                Dim ttSpecial As New TextBox
                Dim ttLTA As New TextBox
                Dim ttEPF As New TextBox
                Dim ttPF As New TextBox
                Dim ttAT As New TextBox
                Dim ttPT As New TextBox
                Dim ttLoan As New TextBox
                Dim ttAdvance As New TextBox
                Dim ttLeave As New TextBox
                Dim ttDeduction As New TextBox
                Dim ttBonus As New TextBox
                Dim ttAddition As New TextBox
                Dim ttRemarks As New TextBox
                Dim ttCTC As New TextBox
                Dim ttGross As New TextBox
                Dim ttNetSalary As New TextBox
                Dim ttTotalDeduction As New TextBox

   
                Dim dr As Data.DataRow

                'ttempname = CType(dtt.Cells(0).FindControl("txtempname"), Textbox)
                'ttempid = CType(dtt.Cells(1).FindControl("txtempid"), Textbox)
                ttempname = CType(dtt.Cells(0).FindControl("txtempname"), Label)
                ttempid = CType(dtt.Cells(1).FindControl("txtempid"), Label)
                ttbasic = CType(dtt.Cells(2).FindControl("Basic"), TextBox)
                ttHra = CType(dtt.Cells(3).FindControl("HRA"), TextBox)
                ttConveyance = CType(dtt.Cells(4).FindControl("Conveyance"), TextBox)
                ttMedical = CType(dtt.Cells(5).FindControl("Medical"), TextBox)
                ttFood = CType(dtt.Cells(6).FindControl("Food"), TextBox)
                ttSpecial = CType(dtt.Cells(7).FindControl("Special"), TextBox)
                ttLTA = CType(dtt.Cells(8).FindControl("LTA"), TextBox)
                ttPF = CType(dtt.Cells(9).FindControl("EPF"), TextBox)
                ttEPF = CType(dtt.Cells(10).FindControl("PF"), TextBox)
                ttAT = CType(dtt.Cells(11).FindControl("AT"), TextBox)
                ttPT = CType(dtt.Cells(12).FindControl("PT"), TextBox)
                ttLoan = CType(dtt.Cells(13).FindControl("LOAN"), TextBox)
                ttAdvance = CType(dtt.Cells(14).FindControl("ADVANCE"), TextBox)
                ttLeave = CType(dtt.Cells(15).FindControl("LEAVE"), TextBox)
                ttDeduction = CType(dtt.Cells(16).FindControl("DOthers"), TextBox)
                ttBonus = CType(dtt.Cells(17).FindControl("Bonus"), TextBox)
                ttAddition = CType(dtt.Cells(18).FindControl("AOthers"), TextBox)
                ttRemarks = CType(dtt.Cells(19).FindControl("Remarks"), TextBox)

                ttCTC = CType(dtt.Cells(20).FindControl("CTC"), TextBox)
                ttGross = CType(dtt.Cells(21).FindControl("Gross"), TextBox)
                ttNetSalary = CType(dtt.Cells(22).FindControl("NetSalary"), TextBox)
                ttTotalDeduction = CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox)


                'response.write(CType(dtt.Cells(1).FindControl("empid"), Textbox).text)
                ' response.end

                hdid.Value = ttempid.Text
                hdname.Value = ttempname.Text
                Session("dynoprocessbasicSession") = ttbasic.Text
                hdbasic.Value = Session("dynoprocessbasicSession")

                hdhra.Value = ttHra.Text
                hdconveyance.Value = ttConveyance.Text
                hdmedical.Value = ttMedical.Text
                hdfood.Value = ttFood.Text
                hdspecial.Value = ttSpecial.Text
                hdlta.Value = ttLTA.Text
                hdpf.Value = ttPF.Text
                hdepf.Value = ttEPF.Text
                hdat.Value = ttAT.Text
                hdpt.Value = ttPT.Text
                hdloan.Value = ttLoan.Text
                hdadvance.Value = ttAdvance.Text
                hdleave.Value = ttLeave.Text
                hddeduction.Value = ttDeduction.Text
                hdbonus.Value = ttBonus.Text
                hdaddition.Value = ttAddition.Text
                hdremarks.Value = ttRemarks.Text
      
                valueempname.Add(hdname.Value)
                valueempid.Add(hdid.Value)
                valuebasic.Add(hdbasic.Value)
                valuehra.Add(hdhra.Value)
                valueconveyance.Add(hdconveyance.Value)
                valuemedical.Add(hdmedical.Value)
                valuefood.Add(hdfood.Value)
                valuespecial.Add(hdspecial.Value)
                valuelta.Add(hdspecial.Value)
                valueepf.Add(hdpf.Value)
                valueat.Add(hdat.Value)
                valuept.Add(hdpt.Value)
                valueloan.Add(hdloan.Value)
                valueadvance.Add(hdadvance.Value)
                valueleave.Add(hdleave.Value)
                valuededuction.Add(hddeduction.Value)
                valuebonus.Add(hdbonus.Value)
                valueaddition.Add(hdaddition.Value)
                valueremarks.Add(hdremarks.Value)
        
          
                CType(dtt.Cells(20).FindControl("CTC"), TextBox).Text = 0
                CType(dtt.Cells(21).FindControl("Gross"), TextBox).Text = 0
                CType(dtt.Cells(22).FindControl("NetSalary"), TextBox).Text = 0
                CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text = 0
		  
                CType(dtt.Cells(20).FindControl("CTC"), TextBox).Text = CInt(ttbasic.Text) + CInt(ttHra.Text) + CInt(ttConveyance.Text) + CInt(ttMedical.Text) + CInt(ttFood.Text) + CInt(ttSpecial.Text) + CInt(ttLTA.Text) + CInt(ttPF.Text) + CInt(ttEPF.Text)

                CType(dtt.Cells(21).FindControl("Gross"), TextBox).Text = CInt(ttbasic.Text) + CInt(ttHra.Text) + CInt(ttConveyance.Text) + CInt(ttMedical.Text) + CInt(ttFood.Text) + CInt(ttSpecial.Text) + CInt(ttLTA.Text) + CInt(ttPF.Text)

		
                CType(dtt.Cells(22).FindControl("NetSalary"), TextBox).Text = CType(dtt.Cells(20).FindControl("CTC"), TextBox).Text - CInt(ttEPF.Text) - CInt(ttPF.Text) - Val(ttPT.Text) - Val(ttAT.Text) - Val(ttLoan.Text) - Val(ttAdvance.Text) - Val(ttLeave.Text) - Val(ttDeduction.Text) + Val(ttBonus.Text) + Val(ttAddition.Text)

         
		 
                CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text = CInt(ttPF.Text) + Val(ttPT.Text) + Val(ttAT.Text) + Val(ttLoan.Text) + Val(ttAdvance.Text) + Val(ttLeave.Text) + Val(ttDeduction.Text)


                hdctc.Value = CType(dtt.Cells(20).FindControl("CTC"), TextBox).Text
                hdgross.Value = CType(dtt.Cells(21).FindControl("Gross"), TextBox).Text
                hdnet.Value = CType(dtt.Cells(22).FindControl("NetSalary"), TextBox).Text
                hdtotaldeduction.Value = CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text

                valuectc.Add(hdctc.Value)
                valuegross.Add(hdgross.Value)
                valuenetsalary.Add(hdnet.Value)
                valuetotaldeduction.Add(hdtotaldeduction.Value)

			

                dr = dt.NewRow

                'i=(dt.rows.count)
                'response.write(i & "<br>")
		
                dr("EmpName") = ttempname.Text
                dr("EmpId") = CInt(ttempid.Text)
                dr("Basic") = CInt(ttbasic.Text)
                dr("Hra") = CInt(ttHra.Text)
                dr("Conveyance") = CInt(ttConveyance.Text)
                dr("Medical") = CInt(ttMedical.Text)
                dr("Food") = CInt(ttFood.Text)
                dr("Special") = CInt(ttSpecial.Text)
                dr("LTA") = CInt(ttLTA.Text)
                dr("Pf") = CInt(ttPF.Text)
                dr("Epf") = CInt(ttEPF.Text)
                dr("AT") = Val(ttAT.Text)
                dr("PT") = Val(ttPT.Text)
                dr("Loan") = Val(ttLoan.Text)
                dr("Advance") = Val(ttAdvance.Text)
                dr("Leave") = Val(ttLeave.Text)
                dr("Deduction") = Val(ttDeduction.Text)
                dr("Bonus") = Val(ttBonus.Text)
                dr("Addition") = Val(ttAddition.Text)
                dr("Remarks") = ttRemarks.Text
                dr("CTC") = CType(dtt.Cells(20).FindControl("CTC"), TextBox).Text
                dr("Gross") = CType(dtt.Cells(21).FindControl("Gross"), TextBox).Text
                dr("NetSalary") = CType(dtt.Cells(22).FindControl("NetSalary"), TextBox).Text
                dr("TotalDeduction") = CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text
        
                dt.Rows.Add(dr)

                Session("dynoReportsession") = dt

                'response.write(session.item("dynoReportsession"))
                'response.end

        
                'DGrdReport.DataSource = dt
                'DGrdReport.DataBind()

          

                'Response.Write(hdNetSalary.value & "<br>")
                'response.end

                ' Response.Write(hdbasic.value & "<br>")
                ' Response.Write(ttbasic.Text & "<br>")
                ' Response.Write(ttHra.Text & "<br>")
                '           Response.Write(ttConveyance.Text & "<br>")
                '           Response.Write(ttMedical.Text & "<br>")
                '           Response.Write(ttfood.Text & "<br>")
                '           Response.Write(ttSpecial.Text & "<br>")
                '           Response.Write(ttlta.Text & "<br>")
                '           Response.Write(ttpf.Text & "<br>")
                '           Response.Write(ttepf.Text & "<br>")
                '           Response.Write(ttat.Text & "<br>")
                '           Response.Write(ttpt.Text & "<br>")
                '           Response.Write(ttloan.Text & "<br>")
                '           Response.Write(ttadvance.Text & "<br>")
                '           Response.Write(ttleave.Text & "<br>")
                '           Response.Write(ttdeduction.Text & "<br>")
                '           Response.Write(ttbonus.Text & "<br>")
                '           Response.Write(ttaddition.Text & "<br>")
                '           Response.Write(ttremarks.Text & "<br>")
                '           Response.Write(ttctc.Text & "<br>")
                '           Response.Write(ttgross.Text & "<br>")
                '           Response.Write(ttNetSalary.Text & "<br>")

            
                ' Next

                'response.write("test") 
                'response.write(ctype(dtt.Cells(22).FindControl("NetSalary"),textbox).text)
            Next

       

            ' DGrdReport.DataSource = dt
            'DGrdReport.DataBind()

         
		

            ' document.postForm.action="postForm2.aspx";
            'document.postForm.submit();

            'btncalculation.Attributes.Add("Onclick", "javascript: getcalculation();")

            'Dim webbrowser  
            'webbrowser.Navigate("javascript:document.form.submit();")


            'Dim sp As String
            'sp += "<Script language=JavaScript>"
            'sp +="alert('hi');"
            'sp += "javascript:document.form.submit();"
            'sp += "</" + "script>"
            'RegisterStartupScript("script123", sp)

        End Sub



    </script>

</head>
<body>
    </form>
    <form id="postForm" name="postForm" method="post" runat="server">
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
                        <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Width="90px"
                            Text="Cancel" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                            background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btncalculation" OnClick="btncalculation_Click" runat="server" Width="90px"
                            Text="Calculate" Style="font-family: Verdana; font-size: 8pt; color: #A2921E;
                            font-weight: bold; background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </tr>
            <tr>
                <td align="left" bgcolor="#c5d5ae" colspan="4">
                    <b><font face="Verdana" color="#a2921e" size="2"></font></b>
                    <asp:Label ID="LBLprocess" runat="server" Width="100%" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp<-------------------Deduction----------------><-Addition->"> 
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
                        autopostback="true" HeaderStyle-Font-Size="10pt" HeaderStyle-ForeColor="#A2921E"
                        HeaderStyle-Font-Bold="True" AllowSorting="True" HeaderStyle-BackColor="#C5D5AE"
                        CellPadding="0">
                        <ItemStyle BackColor="#FFFFEE"></ItemStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Emp
                                    <br>
                                    Name</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtempname" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("empname")%>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Emp<br>
                                    Id</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtEmpId" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("empId")%>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Bas<br>
                                    ic</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="basic" runat="server" Text='<%#  container.dataitem("Basic")%>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    HRA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="HRA" runat="server" Text='<%# container.dataitem("Hra")%>'
                                        DataFormatString="{0:##,###,###}">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Con<br>
                                    veya</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="Conveyance" runat="server" Text='<%# container.dataitem("Conveyance") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Med<br>
                                    ical</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="Medical" runat="server" Text='<%# container.dataitem("Medical") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Food</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="Food" runat="server" Text='<%# container.dataitem("Food") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Spe<br>
                                    ical</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="Special" runat="server" Text='<%# container.dataitem("Special") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    LTA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="LTA" runat="server" Text='<%# container.dataitem("LTA") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    EPF</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="40px" ID="EPF" runat="server" Text='<%# container.dataitem("EPF") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="DodgerBlue">PF</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#66ccff" Width="40px" ID="PF" runat="server" Text='<%# container.dataitem("PF") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="DodgerBlue">Adv<br>
                                        Tax</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#66ccff" Width="40px" ID="AT" runat="server" DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="DodgerBlue">PT</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#66ccff" Width="40px" ID="PT" runat="server" Text='<%# container.dataitem("PT") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="DodgerBlue">Loan</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#66ccff" Width="40px" ID="Loan" runat="server" DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="DodgerBlue">Adv<br>
                                        ance</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#66ccff" Width="40px" ID="Advance" runat="server" DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="DodgerBlue">Leav</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#66ccff" Width="40px" ID="Leave" runat="server" DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="DodgerBlue">Ded<br>
                                        ucti</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="#66ccff" Width="40px" ID="DOthers" runat="server" DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="Black">Bonu</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="Silver" Width="40px" ID="Bonus" runat="server" DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="Black">Add<br>
                                        itio</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox BackColor="Silver" Width="40px" ID="AOthers" runat="server" DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="Red">CTC</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="true" BackColor="red" Width="50px" ID="CTC" runat="server"
                                        Text='<%# container.dataitem("TotalCTC") %>' DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="Red">Gross</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="true" BackColor="red" Width="50px" ID="Gross" runat="server"
                                        Text='<%# container.dataitem("TotalGross") %>' DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="Red">Net<br>
                                        Sal</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="true" BackColor="red" Width="40px" ID="NetSalary" runat="server"
                                        Text='<%# container.dataitem("TotalNet") %>' DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <font color="Red">Tot<br>
                                        dedu</font></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="true" BackColor="red" Width="40px" ID="TotalDeduction" runat="server"
                                        Text='<%# container.dataitem("TotalDeduction") %>' DataFormatString="{0:##,###,###}"
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Remark</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="60px" ID="Remarks" runat="server">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <input type="hidden" id="hdtbreport" runat="server">
        <input type="hidden" id="hdnid" runat="server">
        <input type="hidden" id="hdid" name="hdid" runat="server">
        <input type="hidden" id="hdname" name="hdname" runat="server">
        <input type="hidden" id="hdbasic" name="hdbasic" runat="server">
        <input type="hidden" id="hdhra" name="hdhra" runat="server">
        <input type="hidden" id="hdconveyance" name="hdconveyance" runat="server">
        <input type="hidden" id="hdmedical" name="hdmedical" runat="server">
        <input type="hidden" id="hdfood" name="hdfood" runat="server">
        <input type="hidden" id="hdspecial" name="hdspecial" runat="server">
        <input type="hidden" id="hdlta" name="hdlta" runat="server">
        <input type="hidden" id="hdpf" name="hdpf" runat="server">
        <input type="hidden" id="hdepf" name="hdepf" runat="server">
        <input type="hidden" id="hdat" name="hdat" runat="server">
        <input type="hidden" id="hdpt" name="hdpt" runat="server">
        <input type="hidden" id="hdloan" name="hdloan" runat="server">
        <input type="hidden" id="hdadvance" name="hdadvance" runat="server">
        <input type="hidden" id="hdleave" name="hdleave" runat="server">
        <input type="hidden" id="hddeduction" name="hddeduction" runat="server">
        <input type="hidden" id="hdbonus" name="hdbonus" runat="server">
        <input type="hidden" id="hdaddition" name="hdaddition" runat="server">
        <input type="hidden" id="hdremarks" name="hdremarks" runat="server">
        <input type="hidden" id="hdctc" name="hdctc" runat="server">
        <input type="hidden" id="hdgross" name="hdgross" runat="server">
        <input type="hidden" id="hdnet" name="hdnet" runat="server">
        <input type="hidden" id="hdtotaldeduction" name="hdtotaldeduction" runat="server">
    </form>
</body>
</html>
