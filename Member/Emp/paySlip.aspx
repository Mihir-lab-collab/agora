<%@ Page Language="vb" AutoEventWireup="false" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
        .style1 {
            height: 25px;
        }

        .style3 {
            height: 25px;
            width: 4%;
        }

        .style4 {
            width: 4%;
        }
    </style>
</head>
<body>

    <script language="javascript">
        function popupDisp() {
            pop = window.open('popupPaySlip.aspx', 'popupwindow', 'scrollbars=yes,toolbar=no,menubar=no,width=800,height=530,left=100,top=100');
            pop.focus()
        }

    </script>

    <script runat="server">
        Dim conpayment As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim conempdetail As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim empID As String
        Dim ddate, dMonth, ddate1
        Dim date1 As DateTime
        Dim intmonth As Integer
        Dim strDate
        Dim flgsalgenerated As Integer = 0
        Dim gf As New generalFunction
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            gf.checkEmpLogin()
            empID = Session("dynoEmpIdSession")
            'dDate1 = "1-" & MonthName(Month(dDate)) & "-" & Year(dDate)
		
            If Not IsPostBack Then
                'session.add("strDate",DateAdd("m",-1,Now))
                Dim intOffMonthset = -1
                If (Now.Day < 10) Then
                    intOffMonthset = -2
                End If
                
                lbtnnext.Visible = False
                Session.Add("currentslipdate", DateAdd("m", intOffMonthset, Now))
                ddate = Session("currentslipdate")
                Session.Add("LastUpdateDate", DateAdd("m", intOffMonthset, Now))
                dMonth = Month(ddate)
		
                intmonth = Convert.ToInt32(dMonth)
		
                calculatepayslip()
                YourPackage()
          
            Else
                
                
                
            End If
            If Request("logout") = "true" Then
                Session.Abandon()
                Response.Redirect("/emp/empLoginProcess.asp")
            End If
		
		
        End Sub
        
        Private Sub YourPackage()
            Dim strSQlyourpackage As String
            Dim strSQlyourpackage1 As String
            strSQlyourpackage = "select convert(varchar(3),DateName( month , DateAdd( month , empPayMonth , 0 ) -1)) + '-' +  Convert(varchar(5),empPayYear)  EffectiveFrom,(employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta) + (select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid) as totalctc , empPayBonus  from employeepaymaster where empid=" & Convert.ToInt32(empID)
            strSQlyourpackage1 = "select  E.empid, E.AnnualtotalCTC,E.totalgross from (select (employeemaster.empname) as empname,CONVERT(CHAR(11),employeemaster.empJoiningDate) as empJdate,(employeepaymaster.emppaybasic)as emppaybasic,(employeepaymaster.emppaymonth) as emppayMonth,(employeepaymaster.emppayyear) as emppayYear,employeemaster.empid,      ((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualtotalCTC,           floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as totalctc,         floor( (select Case when ispf=1 then (employeepaymaster.emppaybasic * 100/40)  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as totalgross,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic * 100/40))-((employeepaymaster.emppaybasic * 12/100)) -(employeepaymaster.empPT) else ( ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) -(employeepaymaster.empPT) ) end  from employeemaster where empid=employeepaymaster.empid))as totalnet   from employeepaymaster RIGHT JOIN employeemaster ON employeepaymaster.empid=employeemaster.empid where employeemaster.empleavingdate is NULL OR employeemaster.empleavingdate >' " & DateTime.Now & "')E where E.empid = " & Convert.ToInt32(empID)


            conpayment.Open()
            conempdetail.Open()
           
           
            Dim cmdyourpackage As SqlCommand = New SqlCommand(strSQlyourpackage, conpayment)
            Dim cmdyourpackage1 As SqlCommand = New SqlCommand(strSQlyourpackage1, conempdetail)
           
           
           
            Dim drpayment As SqlDataReader
          
            drpayment = cmdyourpackage.ExecuteReader
            If drpayment.Read() Then
               
                lblEffectiveFrom.Text = drpayment("EffectiveFrom").ToString()
                lblAnnualBonus.Text = Format(drpayment("empPayBonus"), "0,00")
            End If
          
            
            
            Dim drpayment1 As SqlDataReader
          
            drpayment1 = cmdyourpackage1.ExecuteReader
            If drpayment1.Read() Then
               
                lblAnnualCTC.Text = Format(drpayment1("AnnualtotalCTC"), "0,00")
                lblMonthlyGross.Text = Format(drpayment1("totalgross"), "0,00")
                
            End If
           
            drpayment1.Close()
            drpayment.Close()
            conpayment.Close()
            conempdetail.Close()
            
        End Sub
        
        Private Sub calculatepayslip()
            ddate = Session("currentslipdate")
            dMonth = Month(ddate)
            intmonth = Convert.ToInt32(dMonth)
            Dim intYear = Convert.ToInt32(Year(ddate))
	
            Dim strSqlEmpDetail As String
            Dim strSQlpayment
            conpayment.Open()
            conempdetail.Open()
            ddate1 = ddate
            ddate1 = "1-" & MonthName(Month(ddate1)) & "-" & Year(ddate1)
            strSQlpayment = "select top 1 * from employeePayProcessDetail,employeePayProcess where " & _
             "employeePayProcess.payId = employeePayProcessDetail.payID and " & _
            "month(employeePayProcess.paydate) =" & intmonth & " and " & _
            "Year(employeePayProcess.paydate) =" & intYear & _
            " and employeePayProcessDetail.empId =" & _
            Convert.ToInt32(empID) & " order by employeePayProcessDetail.empPayID "

            strSqlEmpDetail = "select e.empname,e.empjoiningdate,s.skilldesc from employeemaster as e inner join 			skillmaster as s on e.skillid=s.skillid where empid=" & Convert.ToInt32(empID)
            
            
            Dim cmdpayment As SqlCommand = New SqlCommand(strSQlpayment, conpayment)
            Dim cmdempDetail As SqlCommand = New SqlCommand(strSqlEmpDetail, conempdetail)
            Dim drpayment As SqlDataReader
            Dim drempdetail As SqlDataReader
            drempdetail = cmdempDetail.ExecuteReader
            If drempdetail.Read() Then
                lblname.Text = drempdetail("empname") & " ( " & empID.ToString() & " )"
                lbldesignation.Text = drempdetail("skilldesc").ToString()
                lblemployedsince.Text = Format(drempdetail("empJoiningDate"), "dd-MMM-yyyy")
                Session("pemployedsince") = lblemployedsince.Text
                Session("pdesignation") = lbldesignation.Text
            End If
            drpayment = cmdpayment.ExecuteReader
            If drpayment.Read() Then
                flgsalgenerated = 1
                If Len(Trim(drpayment("paybasic"))) > 3 Then
                    lblbasicsal.Text = Format(drpayment("paybasic"), "0,00")
                Else
                    lblbasicsal.Text = drpayment("paybasic")
                End If
                If Len(Trim(drpayment("payhra"))) > 3 Then
                    lblhra.Text = Format(drpayment("payhra"), "0,00")
                Else
                    lblhra.Text = drpayment("payhra")
                End If
                If Len(Trim(drpayment("payconveyance"))) > 3 Then
                    lblta.Text = Format(drpayment("payconveyance"), "0,00")
                Else
                    lblta.Text = drpayment("payconveyance")
                End If
                If Len(Trim(drpayment("paylta"))) > 3 Then
                    lbllta.Text = Format(drpayment("paylta"), "0,00")
                Else
                    lbllta.Text = drpayment("paylta")
                End If
                If Len(Trim(drpayment("payfood"))) > 3 Then
                    lblfoodallow.Text = Format(drpayment("payfood"), "0,00")
                Else
                    lblfoodallow.Text = drpayment("payfood")
                End If
                If Len(Trim(drpayment("paymedical"))) > 3 Then
                    lblmedical.Text = Format(drpayment("paymedical"), "0,00")
                Else
                    lblmedical.Text = drpayment("paymedical")
                End If
                If Len(Trim(drpayment("payspecial"))) > 3 Then
                    lblspecialallow.Text = Format(drpayment("payspecial"), "0,00")
                Else
                    lblspecialallow.Text = drpayment("payspecial")
                End If
                If Len(Trim(drpayment("payadvance"))) > 3 Then
                    lbladvancea.Text = Format(drpayment("payadvance"), "0,00")
                Else
                    lbladvancea.Text = drpayment("payadvance")
                End If
                If Len(Trim(drpayment("paybonus"))) > 3 Then
                    lblbonus.Text = Format(drpayment("paybonus"), "0,00")
                Else
                    lblbonus.Text = drpayment("paybonus")
                End If
                If Len(Trim(drpayment("payDeduction"))) > 3 Then
                    lblincometax.Text = Format(drpayment("payDeduction"), "0,00")
                Else
                    lblincometax.Text = drpayment("payDeduction")
                End If
                lblproffesiontax.Text = Format(drpayment("paypt"), "0,00")
                If Len(drpayment("payepf")) > 3 Then
                    lblepf.Text = Format(drpayment("payepf"), "0,00")
                Else
                    lblepf.Text = drpayment("payepf")
                End If
                If Len(Trim(drpayment("payloaninstl"))) > 3 Then
				
                    lblloanrepayment.Text = Format(drpayment("payloaninstl"), "0,00")
                Else
                    lblloanrepayment.Text = drpayment("payloaninstl")
                End If
				
                If (drpayment("payleave")) = 0 Then
                
                    lblleavededuction.Text = "0"
				
                Else
                    lblleavededuction.Text = Format(Math.Round((Convert.ToDouble(lblbasicsal.Text) + Convert.ToDouble(lblhra.Text) + Convert.ToDouble(lblta.Text) + Convert.ToDouble(lblmedical.Text) + Convert.ToDouble(lbllta.Text) + Convert.ToDouble(lblfoodallow.Text) + Convert.ToDouble(lblspecialallow.Text)) / (Day(DateAdd("d", -1, DateAdd("m", 1, ddate1)))) * (drpayment("payLeave"))), "0,00")

                End If
			
                If Len(Trim(drpayment("payAddition"))) > 3 Then
                    lbladvanceab.Text = Format(drpayment("payAddition"), "0,00")
                Else
                    lbladvanceab.Text = drpayment("payAddition")
                End If
                If Len(Trim(drpayment("payothers"))) > 3 Then
                    lblothers.Text = Format(drpayment("payotherDeduction"), "0,00")
                Else
                    lblothers.Text = drpayment("payotherDeduction")
                End If
				
                'lbldayspresent.text=drpayment("payleavedays").tostring()
                lbldayspresent.Text = Day(DateAdd("d", -1, DateAdd("m", 1, ddate1))) - drpayment("payleave").ToString()
                lbldaysabsent.Text = drpayment("payleave").ToString()
                lblnoofdays.Text = Day(DateAdd("d", -1, DateAdd("m", 1, ddate1)))
				
				
                lbltotaldeduction.Text = Format(Convert.ToDouble(lbladvancea.Text) + Convert.ToDouble(lblincometax.Text) + Convert.ToDouble(lblproffesiontax.Text) + Convert.ToDouble(lblepf.Text) + Convert.ToDouble(lblloanrepayment.Text) + Convert.ToDouble(lblleavededuction.Text) + Convert.ToDouble(lblothers.Text), "0,00")


                If lblepf.Text <> "0" Then
                    lblgrosssal.Text = Format(Convert.ToDouble(lblbasicsal.Text) + Convert.ToDouble(lblhra.Text) + Convert.ToDouble(lblta.Text) + Convert.ToDouble(lblmedical.Text) + Convert.ToDouble(lbllta.Text) + Convert.ToDouble(lblfoodallow.Text) + Convert.ToDouble(lblspecialallow.Text), "0,00") 'Format(Convert.ToDouble(lblbasicsal.Text) * 100 / 40, "0,00")
                Else
                    
                    lblgrosssal.Text = Format(Convert.ToDouble(lblbasicsal.Text) + Convert.ToDouble(lblhra.Text) + Convert.ToDouble(lblta.Text) + Convert.ToDouble(lblmedical.Text) + Convert.ToDouble(lbllta.Text) + Convert.ToDouble(lblfoodallow.Text) + Convert.ToDouble(lblspecialallow.Text), "0,00")
                End If
                
				
                
                
                lblab.Text = Format((lblgrosssal.Text - lbltotaldeduction.Text), "0,00")
				
                lblnetpayable.Text = Format((Convert.ToDouble(lblgrosssal.Text) - Convert.ToDouble(lbltotaldeduction.Text)) + Convert.ToDouble(lblbonus.Text) + Convert.ToDouble(lbladvanceab.Text) + Convert.ToDouble(lblloan.Text) + Convert.ToDouble(lblpaidleaves.Text), "0,00")
				
                Session("pempname") = drempdetail("empname")
                Session("pbasic") = lblbasicsal.Text
                Session("phra") = lblhra.Text
                Session("pta") = lblta.Text
                Session("pmedical") = lblmedical.Text
                Session("plta") = lbllta.Text
                Session("pfoodallow") = lblfoodallow.Text
                Session("pspecialallow") = lblspecialallow.Text
                Session("padvancea") = lbladvancea.Text
                Session("plblbonus") = lblbonus.Text
                Session("pprofessiontax") = lblproffesiontax.Text
                Session("pepf") = lblepf.Text
                Session("ppf") = lblincometax.Text
                Session("ploan") = lblloan.Text
                Session("ptotaldeduction") = lbltotaldeduction.Text
                Session("pnetpayable") = lblnetpayable.Text
                Session("pgrosssal") = lblgrosssal.Text
                Session("pab") = lblab.Text
                Session("pdayspresent") = lbldayspresent.Text
                Session("pdaysabsent") = lbldaysabsent.Text
                Session("pnoofdays") = lblnoofdays.Text
                Session("ploanpay") = lblloanrepayment.Text
                Session("pleavededuction") = lblleavededuction.Text
                Session("pother") = lblothers.Text
                Session("padvanceab") = lbladvanceab.Text
				
            Else
                flgsalgenerated = 0
                lblbasicsal.Text = "0"
                lblhra.Text = "0"
                lbllta.Text = "0"
                lblta.Text = "0"
                lblfoodallow.Text = "0"
                lblmedical.Text = "0"
                lblspecialallow.Text = "0"
                lbladvancea.Text = "0"
                lblbonus.Text = "0"
                lblincometax.Text = "0"
                lblproffesiontax.Text = "0"
                lblepf.Text = "0"
                lblloan.Text = "0"
                lblothersab.Text = "0"
                lbldayspresent.Text = "0"
                lbldaysabsent.Text = "0"
                lblnoofdays.Text = Day(DateAdd("d", -1, DateAdd("m", 1, ddate1)))
                Session("ab") = "0"
                lbltotaldeduction.Text = "0"
                lblgrosssal.Text = "0"
                lblnetpayable.Text = "0"
                lblloanrepayment.Text = "0"
                lblleavededuction.Text = "0"
                lblothers.Text = "0"
                lbladvanceab.Text = "0"
				
					
            End If
            drpayment.Close()
            drempdetail.Close()
            conempdetail.Close()
            conpayment.Close()
        End Sub
    

        Sub lbtnpre_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Session("currentslipdate") = DateAdd("m", -1, Session("currentslipdate"))
            calculatepayslip()
            lbtnnext.Visible = True
        End Sub

        Sub lbtnnext_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
         
            Session("currentslipdate") = DateAdd("m", 1, Session("currentslipdate"))
            
            Dim intOffMonthset = -1
            If (Now.Day < 10) Then
                intOffMonthset = -2
            End If
                
            Dim CurrentDate = DateAdd("m", intOffMonthset, Now)
            
            Dim LastUpdateDate = Convert.ToDateTime(Session("LastUpdateDate"))
           
            Dim SessionDate = Convert.ToDateTime(Session("currentslipdate"))
           
            
            If CurrentDate.CompareTo(SessionDate) < 0 Then
                Session("currentslipdate") = Session("LastUpdateDate")
                 SessionDate = Convert.ToDateTime(Session("currentslipdate"))
            End If
            
               
            
            If SessionDate.Date = CurrentDate.Date Then
                ddate = Session("currentslipdate")
                dMonth = Month(ddate)
                lbtnnext.Visible = False
                Session("LastUpdateDate") = Session("currentslipdate")
            End If
           
           
            calculatepayslip()
        End Sub

    </script>

    <form id="Form1" runat="server">



        <table id="Table3" cellspacing="0" cellpadding="2" width="100%" align="center" border="0">
            <tr valign="top">
                <td>
                    <EMPHEADER:empHeader ID="Empheader" runat="server"></EMPHEADER:empHeader>
                    <br>
                    <uc1:empMenuBar ID="EmpMenuBar" runat="server"></uc1:empMenuBar>
                </td>
            </tr>
        </table>



        <br>
        <% If Now.Day >= 10 Then%>
        <table align="center" cellpadding="4" width="70%" border="1" style="border-collapse: collapse" bordercolor="#E8E8E8" bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8">
            <tr>
                <td width="25%" bgcolor="#edf2e6" colspan="2">
                    <font face="Verdana" color="#a2921e" size="2"><b>Your Current Package
                        <font face="Verdana" color="#a2921e" size="2"><b></td>
            </tr>

            <tr>
                <td class="style3">
                    <font face="Verdana" color="#a2921e" size="2"><b>Effective From
                </td>

                <td width="25%" class="style1">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblEffectiveFrom" runat="server">April 2012</asp:Label></td>



            </tr>

            <tr>
                <td class="style4">
                    <font face="Verdana" color="#a2921e" size="2"><b>Annual CTC
                </td>

                <td width="25%">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        Rs. <asp:Label ID="lblAnnualCTC" runat="server"  >0</asp:Label></td>

            </tr>


            <tr>
                <td class="style3">
                    <font face="Verdana" color="#a2921e" size="2"><b>Annual Bonus
                </td>

                <td width="25%" class="style1">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        Rs. <asp:Label ID="lblAnnualBonus" runat="server" Text ="0"></asp:Label></td>



            </tr>

            <tr>
                <td class="style3">
                    <font face="Verdana" color="#a2921e" size="2"><b>Monthly Gross
                </td>

                <td width="25%" class="style1">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        Rs. <asp:Label ID="lblMonthlyGross" runat="server">0</asp:Label></td>



            </tr>
        </table>


        <br />
        <br />

        <%End If%>
        <table id="tblsalaryslip" align="center" cellpadding="4" width="70%" border="1" style="border-collapse: collapse"
            bordercolor="#E8E8E8" bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8">
            <tr>
                <td width="25%" bgcolor="#edf2e6" colspan="4">
                    <font face="Verdana" color="#a2921e" size="2">
                        <img src="../images/dynamic_logo.gif" align="right" valign="top">
                        <b>INTELGAIN TECHNOLOGIES PVT. LTD.
                            <br>
                            B-203, Sanpada Station Complex, Navi Mumbai 400 705
                            <br>
                        </b>
                        <br>
                        <tr>
                            <td colspan="4">
                                <br>
                            </td>
                            <tr></td>
            </tr>



            <tr>
                <td width="25%" bgcolor="#edf2e6" colspan="4">
                    <font face="Verdana" color="#a2921e" size="2"><b>SALARY SLIP FOR THE MONTH
                        <asp:LinkButton ID="lbtnpre" runat="server" OnClick="lbtnpre_click"> <font face="Verdana" color="#a2921e" size="2"><b><< </b></font></asp:LinkButton>
                        <%= MonthName(dMonth) & " " & Year(ddate)%>
                        <asp:LinkButton ID="lbtnnext" runat="server" OnClick="lbtnnext_Click"><b><font face="Verdana" color="#a2921e" size="2"> >> </b></asp:LinkButton>
                    </b></td>
            </tr>
            <% If flgsalgenerated = 1 Then%>
            <tr>
                <td width="25%" colspan="4">
                    <font face="Verdana" color="#a2921e" size="2"><b>Employee Details
                </td>
                <tr>
                    </td>
                </tr>
            <tr>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Name
                </td>
                </b></font>
                <td width="25%">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblname" runat="server"></asp:Label></td>
                </b></font> </td>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>No Of Days
                </td>
                </b></font>
                <td width="25%">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblnoofdays" runat="server">0</asp:Label></td>
                </b></font></td>
            </tr>
            <tr>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Employed Since
                </td>
                </b></font>
                <td width="25%">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblemployedsince" runat="server"></asp:Label></td>
                </b></font> </td>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Days Present
                </td>
                </b></font>
                <td width="25%">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lbldayspresent" runat="server">0</asp:Label></td>
                </b></font></td>
            </tr>
            <tr>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Designation
                </td>
                </b></font>
                <td width="25%">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lbldesignation" runat="server"></asp:Label></td>
                </b></font></td>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Days Absent
                </td>
                </b></font>
                <td width="25%">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lbldaysabsent" runat="server">0</asp:Label></td>
                </b></font> </td>
            </tr>
        </table>
        <table align="center" width="70%" border="1" cellpadding="2" cellspacing="0" bordercolorlight="#000000"
            bordercolordark="#FFFFFF">
            <tr>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="20%" align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>Receivable</b></td>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="10%" align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>Rs.</b></font>
                </td>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="10%" align="center">
                    <font face="verdana" size="2" color="#a2921e"><b>Deductions</b></font>
                </td>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="10%" align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>Rs.</b></font>
                </td>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="10%" align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>Addition</b></font>
                </td>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="10%" align="center">
                    <font face="verdana" size="2" color="#a2921e"><b>Rs.</b></td>
            </tr>
            <tr>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Basic Salary</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblbasicsal" runat="server">0</asp:Label></td>
                </b></font>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Advance</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lbladvancea" runat="server">0</asp:Label></td>
                </b></font>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>(A-B)</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblab" runat="server">0</asp:Label></td>
                </b></font>
            </tr>
            <tr>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>HRA</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblhra" runat="server">0</asp:Label></td>
                </b></font>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Income Tax</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblincometax" runat="server">0</asp:Label></td>
                </b></font>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Bonus</b></font>
                </td>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblbonus" runat="server">0</asp:Label></td>
                </b></font>
            </tr>
            <tr>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>TA</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblta" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Profession Tax</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblproffesiontax" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Advance</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lbladvanceab" runat="server">0</asp:Label></td>
                </b></font></td>
            </tr>
            <tr>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Medical</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblmedical" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>EPF</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblepf" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Loan</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblloan" runat="server">0</asp:Label></td>
                </b></font></td>
            </tr>
            <tr>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>LTA</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lbllta" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Loan Payment</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblloanrepayment" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Paid Leaves</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblpaidleaves" runat="server">0</asp:Label></td>
                </b></font></td>
            </tr>
            <tr>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Food Allow.</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblfoodallow" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Leave Deduction</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblleavededuction" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Others</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblothersab" runat="server">0</asp:Label></td>
                </b></font></td>
            </tr>
            <tr>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Special Allow.</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblspecialallow" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>
                    <font face="Verdana" color="#a2921e" size="2"><b>Others</b></font>
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblothers" runat="server">0</asp:Label></td>
                </b></font></td>
                <td>-
                </td>
                <td align="right">
                    <font face="Verdana" color="#a2921e" size="2"><b>
                        <asp:Label ID="lblblank" runat="server">-</asp:Label></td>
                </b></font></td>
            </tr>
            <tr>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="30%" nowrap="nowrap" align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>Gross Salary (A) </b></td>
                <td align="right" bgcolor="#C5D5AE" bordercolorlight="#000000" nowrap="nowrap" width="10%"
                    align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>
                        <asp:Label ID="lblgrosssal" runat="server">0</asp:Label>
                    </b></font>
                </td>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="25%" nowrap="nowrap" align="center">
                    <font face="verdana" size="2" color="#a2921e"><b>Total Deduction (B)</b></font>
                </td>
                <td align="right" bgcolor="#C5D5AE" bordercolorlight="#000000" nowrap="nowrap" width="10%"
                    align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>
                        <asp:Label ID="lbltotaldeduction" runat="server">0</asp:Label>
                    </b></font>
                </td>
                <td bgcolor="#C5D5AE" bordercolorlight="#000000" width="35%" nowrap="nowrap" align="center">
                    <font face="Verdana" size="2" color="#a2921e"><b>Net Payable</b></font>
                </td>
                <td align="right" bgcolor="#C5D5AE" bordercolorlight="#000000" nowrap="nowrap" width="10%"
                    align="center">
                    <font face="verdana" size="2" color="#a2921e"><b>
                        <asp:Label ID="lblnetpayable" runat="server">0</asp:Label>
                    </b></td>
            </tr>
        </table>
        <br>
        <table align="center">
            <tr>
                <td>
                    <input type="button" align="right" size="15" value="Print" onclick="popupDisp()"
                        id="btnPrint" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                        runat="server" />
                </td>
            </tr>
        </table>
        <% Else%>
        </table>
        <br>
        <table align="center" width="70%" border="1" cellpadding="2" cellspacing="0" bordercolorlight="#000000"
            bordercolordark="#FFFFFF">
            <tr>
                <td width="25%" bgcolor="#edf2e6" colspan="4" align="center">
                    <font face="Verdana" color="#a2921e" size="2"><b>SALARY SLIP FOR THIS MONTH IS NOT
                    GENERATED
                </td>
                <tr>
                    <%End If%>
    </form>
</body>
<html>
