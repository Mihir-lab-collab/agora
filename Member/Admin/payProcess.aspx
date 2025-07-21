<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pay Process</title>
    <script language="javascript" type="text/javascript">
        function payDisp(dDate) {
            pop = window.open('coveringLetter.aspx?dDate=' + dDate, 'popupwindow', 'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=945,height=650,left=20,top=10');
            pop.focus()
        }

        function SelectAll(spanChk) {
            var oItem = spanChk.children;

            var theBox = (spanChk.type == "checkbox") ?
                spanChk : spanChk.children.item[0];

            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)

                if (elm[i].type == "checkbox" &&
                  elm[i].id != theBox.id) {
                    //elm[i].click();
                    if (elm[i].checked != xState)
                        elm[i].click();
                    //elm[i].checked=xState;
                }
        }
    </script>

    <script language="VB" runat="server">
        Dim Cmd As New SqlCommand()
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSQL As String
        Dim flg As Integer
        Public inttest As Integer
        Dim intctcdisplay As Integer
        Dim intgrossdisplay As Integer
        Dim intnetdisplay As Integer
        Dim itmctc As String
        Dim ctcitem As Integer
        Dim itmgross As String
        Dim grossitem As Integer
        Dim itmnet As String
        Dim netitem As Integer
        Dim flgPrev As Integer
        Dim strmonthPrev As DateTime
        Dim dDate, dMonth, dYear, payDate As String
        Dim ctcsum, grosssum, netsum As Double
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            spanLocation.InnerHtml= Session("DivLocation").ToString().Trim()
            spanLocation1.InnerHtml= Session("DivLocation").ToString().Trim()

            gf.checkEmpLogin()
            dDate = Request("salDate")
            If Not IsDate(dDate) Then
                dDate = DateAdd("m", -1, Now)
            End If
            dDate = "1-" & MonthName(Month(dDate)) & "-" & Year(dDate)
		
            dMonth = Month(dDate)
            dYear = Year(dDate)
            Session("sessionDDate") = dDate
            If Not IsPostBack Then
                Call cbindgrid()
            End If
        End Sub
        Sub cbindgrid()
            Dim conn As New SqlConnection
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
            hdmonth.Value = MonthName(dMonth)
            hdyear.Value = dYear
		
            Session.Item("dynoProcesssessiondays") = Day(DateAdd("d", -1, DateAdd("m", 1, dDate)))
		
            LBLprocess.Text = "Salary Generated for the Month of " & hdmonth.Value & " " & hdyear.Value
            LBLdays.Text = "Total No Of Days " & Session.Item("dynoProcesssessiondays")
            strSQL = "select * from employeePayProcess where month(payDate) ='" & Month(dDate) & "' AND Year(payDate)=" & Year(dDate)
            Session.Item("dynoProcesssessionmonth") = hdmonth.Value
            Session.Item("dynoProcesssessionyear") = hdyear.Value
            Dim cmdcheck As SqlCommand = New SqlCommand(strSQL, conn)
            Dim drcheck As SqlDataReader
            drcheck = cmdcheck.ExecuteReader
            If (drcheck.Read) Then
                payDate = drcheck("payDate")
                PNotes.Text = drcheck("payComment")
                strSQL = "select (employeemaster.empname) as empname,(employeemaster.empAccountNo) as empAccountNo ,(employeepayprocessdetail.empid) as empid, DateAdd(month, empProbationperiod, empJoiningDate) as empConfirmDate , (employeepayprocessdetail.paybasic) as basic,(employeepayprocessdetail.PayHra) as Hra,(employeepayprocessdetail.payConveyance) as Conveyance,(employeepayprocessdetail.PayMedical) as Medical,(employeepayprocessdetail.PayFood) as Food,(employeepayprocessdetail.PaySpecial) as Special,(employeepayprocessdetail.PayPF) as PF,(employeepayprocessdetail.PayEPF) as EPF, (employeePayprocessdetail.payLeaveDays) as PresentDays,(employeepayprocessdetail.PayLTA) as LTA, (employeepayprocessdetail.PayPT) as PT,(employeepayprocessdetail.PayInsurance) as Insurance,(employeepayprocessdetail.PayAT) as AT, (employeepayprocessdetail.Paydeduction) as deduction,(employeepayprocessdetail.payOtherDeduction) as otherdeduction,(employeepayprocessdetail.PayLeave) as Leave, (employeepayprocessdetail.PayAdvance) as Advance,(employeepayprocessdetail.Paybonus) as Bonus, (employeepayprocessdetail.Payaddition) as Addition,(employeepayprocessdetail.PayloanInstl) as Loan, (employeepayprocessdetail.PayOthers) as Others,(employeepayprocessdetail.Payremark) as RemarkS,((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+ (employeepayprocessdetail.payconveyance)+(employeepayprocessdetail.paymedical)+ (employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+ (employeepayprocessdetail.paylta)+ + (employeepayprocessdetail.payPF))  as Ctc, floor(( select Case when EMP.ispf=1 then  (employeepayprocessdetail.paybasic) * 100/40 else  (employeepayprocessdetail.paybasic+employeepayprocessdetail.payhra+ employeepayprocessdetail.payconveyance+employeepayprocessdetail.paymedical+employeepayprocessdetail.payfood+employeepayprocessdetail.payspecial+employeepayprocessdetail.paylta ) end from employeepaymaster EMP, employeeMaster EP where EMP.empId = ep.empid and   EP.empid=employeepayprocessdetail.empid)) as gross,((employeepayprocessdetail.payBonus)+(employeepayprocessdetail.payAddition)) as TotalAddition, ((employeepayprocessdetail.payPf)+(employeepayprocessdetail.payPT)+(employeepayprocessdetail.payInsurance)+(employeepayprocessdetail.payAT)+(employeepayprocessdetail.payLoanInstl)+(employeepayprocessdetail.payAdvance)+ ((employeepayprocessdetail.payLeave) * (((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+(employeepayprocessdetail.paylta))/" & Session.Item("dynoProcesssessiondays") & "))+(employeepayprocessdetail.payDeduction)+(employeepayprocessdetail.payOtherDeduction)) as TotalDeduction,  floor(( select Case when EMP.ispf=1 then ((employeepayprocessdetail.paybasic) * 100/40 )-((employeepayprocessdetail.paybasic) * 12/100)-(employeepayprocessdetail.payPT)-(employeepayprocessdetail.payInsurance) else  (employeepayprocessdetail.paybasic+employeepayprocessdetail.payhra+ employeepayprocessdetail.payconveyance+employeepayprocessdetail.paymedical+employeepayprocessdetail.payfood+employeepayprocessdetail.payspecial+employeepayprocessdetail.paylta )-(employeepayprocessdetail.payPT)-(employeepayprocessdetail.payInsurance) end from employeepaymaster EMP, employeeMaster EP where EMP.empId = ep.empid and   EP.empid=employeepayprocessdetail.empid))as netSALARYOld ,floor(( select Case when EMP.ispf=1 then  (employeepayprocessdetail.paybasic) * 100/40 else  (employeepayprocessdetail.paybasic+employeepayprocessdetail.payhra+ employeepayprocessdetail.payconveyance+employeepayprocessdetail.paymedical+employeepayprocessdetail.payfood+employeepayprocessdetail.payspecial+employeepayprocessdetail.paylta ) end from employeepaymaster EMP, employeeMaster EP where EMP.empId = ep.empid and   EP.empid=employeepayprocessdetail.empid) + (((employeepayprocessdetail.payBonus)+(employeepayprocessdetail.payAddition)))-(((employeepayprocessdetail.payPf)+(employeepayprocessdetail.payPT)+(employeepayprocessdetail.payInsurance)+(employeepayprocessdetail.payAT)+(employeepayprocessdetail.payLoanInstl)+(employeepayprocessdetail.payAdvance)+ ((employeepayprocessdetail.payLeave) * (((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+(employeepayprocessdetail.paylta))/" & Session.Item("dynoProcesssessiondays") & "))+(employeepayprocessdetail.payDeduction)+(employeepayprocessdetail.payOtherDeduction))))  as netSALARY from employeepayprocessdetail,employeemaster where employeepayprocessdetail.empid=employeemaster.empid  and employeemaster.LocationFKID = ' " & Session("DivLocationID") & "'            and employeepayprocessdetail.payid= " & drcheck("payId")

strSQL = "select empname as empname,empAccountNo as empAccountNo , employeepayprocessdetail.empid as empid, DateAdd(month, empProbationperiod, empJoiningDate) as empConfirmDate , paybasic as basic,PayHra as Hra,payConveyance as Conveyance,PayMedical as Medical,PayFood as Food,PaySpecial as Special,PayPF as PF,PayEPF as EPF, payLeaveDays as PresentDays,PayLTA as LTA, PayPT as PT, PayInsurance as Insurance,PayAT as AT, ( Paydeduction) as deduction,( payOtherDeduction) as otherdeduction,( PayLeave) as Leave, ( PayAdvance) as Advance,( Paybonus) as Bonus, ( Payaddition) as Addition,( PayloanInstl) as Loan, ( PayOthers) as Others,( Payremark) as RemarkS,payLeave,(paybasic+payhra+ payconveyance+paymedical+payfood+payspecial+paylta+payPF) as Ctc,(paybasic+ payhra+  payconveyance+ paymedical+ payfood+ payspecial+ paylta) as gross,(payBonus+payAddition) as TotalAddition, Round((payPf+ payPT+payInsurance+payAT+payLoanInstl+payAdvance+ (payLeave*(paybasic+payhra+payconveyance+paymedical+payfood+ payspecial+paylta)/31)+payDeduction+payOtherDeduction),0) as TotalDeduction, Round(paybasic+ payhra+  payconveyance+ paymedical+ payfood+ payspecial+ paylta+payBonus+payAddition-(payPf+payPT+payInsurance+payAT+payLoanInstl+payAdvance+ (payLeave *(paybasic+payhra+payconveyance+paymedical+payfood+payspecial+paylta)/31)+payDeduction+payOtherDeduction),0) as netSALARY from employeepayprocessdetail,employeemaster where  employeepayprocessdetail.empid=employeemaster.empid and employeemaster.LocationFKID = '" & Session("DivLocationID") & "'            and employeepayprocessdetail.payid= " & drcheck("payId")

'Response.Write(strSQL)
                
                Session("payid") = drcheck("payId")
                drcheck.Close()
                Dim dadummy As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)
                Dim dsdummy As New DataSet()
                Dim objDT As System.Data.DataTable
                objDT = New System.Data.DataTable("dtdummy")
                objDT.Columns.Add("SerialNo", GetType(Integer))
                objDT.Columns("SerialNo").AutoIncrement = True
                objDT.Columns("SerialNo").AutoIncrementSeed = 1
                objDT.Columns("SerialNo").AutoIncrementStep = 1
                dadummy.Fill(dsdummy)
                dadummy.Fill(objDT)
                dgrdDummy.DataSource = dsdummy
                dgrdDummy.DataBind()
            Else
                Call bindgrid()
            End If
        End Sub

        Sub bindgrid()
            Dim conn As New SqlConnection
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
            strSQL = "SELECT empname, employeemaster.empid, DateAdd(month, empProbationperiod, empJoiningDate) as empConfirmDate,  emppaybasic as basic,emppayHra as Hra,emppayConveyance AS Conveyance, empPayMedical as Medical, empPayFood as Food, empPaySpecial as Special,(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid) AS PF,(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid) AS EPF, 0 as PresentDays, 0 as TotalDays, empPayLTA as LTA, empPT as PT,empInsurance as Insurance, 0 as AT, 0 as Loan, 0 as advance, 0 as deduction,0 as otherdeduction, 0 as leave, 0 as bonus, 0 as addition, (emppaybasic+emppayhra+emppayconveyance+emppaymedical+emppayfood+emppayspecial+emppaylta+emppaybasic*12/100) as totalctc, (emppaybasic+emppayhra+emppayconveyance+emppaymedical+emppayfood+emppayspecial+emppaylta) AS totalgross, 0 as TotalAddition, 0 as TotalDeduction, 0 as totalnet  FROM employeePayMaster RIGHT JOIN employeeMaster on employeePayMaster.empId=employeeMaster.empId WHERE (empLeavingDate IS NULL OR empLeavingDate>'" & dDate & "') and employeemaster.LocationFKID = ' " & Session("DivLocationID") & "' ORDER BY employeeMaster.empId"
	       
            Dim da As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)
            Dim ds As New DataSet()
            da.Fill(ds)
            dgrdprocess.DataSource = ds
            dgrdprocess.DataBind()
        End Sub

        Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Response.Redirect("../admin/Payroll.aspx")
        End Sub

        Sub btndelete_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim conn As New SqlConnection
            Dim payId As String = ""
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()

            strSQL = "select * from employeePayProcess where month(payDate) ='" & Month(dDate) & "' AND Year(payDate)=" & Year(dDate)
            Dim cmdcheck1 As SqlCommand = New SqlCommand(strSQL, conn)
            Dim drcheck1 As SqlDataReader
            drcheck1 = cmdcheck1.ExecuteReader
            If (drcheck1.Read) Then
                payId = drcheck1("payId")
            End If
            drcheck1.Close()
		
            If IsNumeric(payId) Then
                strSQL = " delete from employeepayprocessdetail where  payid = " & payId & ";" & _
                "delete from employeepayprocess where  payid = " & payId
                Dim deletecmd As SqlCommand = New SqlCommand(strSQL, conn)
                deletecmd.ExecuteNonQuery()
                deletecmd.Connection.Close()
                Response.Redirect("../admin/payProcess.aspx")
            End If
        End Sub

        Sub btncalculation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                
          
                Dim dtt As DataGridItem
                Dim intcnt As Integer
                ' Dim i As Integer
                Dim dt As New System.Data.DataTable
                Dim intGbasic, intGctc, intGgross, IntGnet As Integer
                Session.Add("dynoProcesssessionCtc", "0")
                Session("dynoReportNotessession") = PayNotes.Value
                intcnt = Session("dynoProcessCountsession")
                intGbasic = 0
                intGctc = 0
                intGgross = 0
                IntGnet = 0
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
                dt.Columns.Add("Insurance")
                dt.Columns.Add("Loan")
                dt.Columns.Add("Advance")
                dt.Columns.Add("IncomeTax Deduction")
                dt.Columns.Add("Other Deduction")
                dt.Columns.Add("Leave")
                dt.Columns.Add("PresentDays")
                dt.Columns.Add("TotalDays")
                dt.Columns.Add("Bonus")
                dt.Columns.Add("Addition")
                dt.Columns.Add("TotalAddition")
                dt.Columns.Add("TotalDeduction")
                dt.Columns.Add("CTC")
                dt.Columns.Add("Gross")
                dt.Columns.Add("NetSalary")
                dt.Columns.Add("Remarks")
                dt.Columns.Add("confirmDate")
                For Each dtt In dgrdprocess.Items
                    Dim chkselect As New CheckBox
                    chkselect = CType(dtt.Cells(0).FindControl("chkselect"), CheckBox)
                    If chkselect.Checked = True Then
                        Dim ttempname As New Label
                        Dim ttempid As New Label
                        Dim ttConfirmDate As New Label
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
                        Dim ttInsurance As New TextBox
                        Dim ttLoan As New TextBox
                        Dim ttAdvance As New TextBox
                        Dim ttDeduction As New TextBox
                        Dim ttLeave As New TextBox
                        Dim ttPresentDays As New TextBox
                        Dim ttTotalDays As New TextBox
                        Dim ttBonus As New TextBox
                        Dim ttAddition As New TextBox
                        Dim ttTotalAddition As New TextBox
                        Dim ttTotalDeduction As New TextBox
                        Dim ttCTC As New TextBox
                        Dim ttGross As New TextBox
                        Dim ttNetSalary As New TextBox
                        Dim ttRemarks As New TextBox
                        Dim ttotherDeduction As New TextBox
                        Dim strGross As String
                        Dim dr As Data.DataRow
                        ttempname = CType(dtt.Cells(0).FindControl("txtempname"), Label)
                        ttempid = CType(dtt.Cells(1).FindControl("txtempid"), Label)
                        ttConfirmDate = CType(dtt.Cells(1).FindControl("txtConfirmDate"), Label)
                        ttbasic = CType(dtt.Cells(2).FindControl("Basic"), TextBox)
                        ttHra = CType(dtt.Cells(3).FindControl("HRA"), TextBox)
                        ttConveyance = CType(dtt.Cells(4).FindControl("Conveyance"), TextBox)
                        ttMedical = CType(dtt.Cells(5).FindControl("Medical"), TextBox)
                        ttFood = CType(dtt.Cells(6).FindControl("Food"), TextBox)
                        ttSpecial = CType(dtt.Cells(7).FindControl("Special"), TextBox)
                        ttLTA = CType(dtt.Cells(8).FindControl("LTA"), TextBox)
                        ttEPF = CType(dtt.Cells(9).FindControl("EPF"), TextBox)
                        ttPF = CType(dtt.Cells(10).FindControl("PF"), TextBox)
                        ttAT = CType(dtt.Cells(11).FindControl("AT"), TextBox)
                        ttPT = CType(dtt.Cells(12).FindControl("PT"), TextBox)
                        ttInsurance = CType(dtt.Cells(28).FindControl("Insurance"), TextBox)
                        ttLoan = CType(dtt.Cells(13).FindControl("LOAN"), TextBox)
                        ttAdvance = CType(dtt.Cells(14).FindControl("ADVANCE"), TextBox)
                        ttDeduction = CType(dtt.Cells(15).FindControl("DOthers"), TextBox)
                        ttotherDeduction = CType(dtt.Cells(16).FindControl("Otherdeduction"), TextBox)
                        ttLeave = CType(dtt.Cells(17).FindControl("LEAVE"), TextBox)
                        ttPresentDays = CType(dtt.Cells(18).FindControl("PresentDays"), TextBox)
                        ttTotalDays = CType(dtt.Cells(19).FindControl("TotalDays"), TextBox)
                        ttBonus = CType(dtt.Cells(20).FindControl("Bonus"), TextBox)
                        ttAddition = CType(dtt.Cells(21).FindControl("AOthers"), TextBox)
                        ttTotalAddition = CType(dtt.Cells(22).FindControl("TotalAddition"), TextBox)
                        ttTotalDeduction = CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox)
                        ttCTC = CType(dtt.Cells(24).FindControl("CTC"), TextBox)
                        ttGross = CType(dtt.Cells(25).FindControl("Gross"), TextBox)
                        ttNetSalary = CType(dtt.Cells(26).FindControl("NetSalary"), TextBox)
                        ttRemarks = CType(dtt.Cells(27).FindControl("Remarks"), TextBox)
                        Dim strPt As String = String.Empty
                        CType(dtt.Cells(22).FindControl("TotalAddition"), TextBox).Text = 0
                        CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text = 0
                        CType(dtt.Cells(24).FindControl("CTC"), TextBox).Text = 0
                        CType(dtt.Cells(25).FindControl("Gross"), TextBox).Text = 0
                        CType(dtt.Cells(26).FindControl("NetSalary"), TextBox).Text = 0
                        CType(dtt.Cells(22).FindControl("TotalAddition"), TextBox).Text = Val(ttBonus.Text) + Val(ttAddition.Text)
                        CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text = Val(ttPF.Text) + Val(ttPT.Text) + Val(ttInsurance.Text) + Val(ttAT.Text) + Val(ttLoan.Text) + Val(ttAdvance.Text) + Val(ttLeave.Text) + Val(ttDeduction.Text) + Val(ttotherDeduction.Text)
                        CType(dtt.Cells(24).FindControl("CTC"), TextBox).Text = Val(ttbasic.Text) + Val(ttHra.Text) + Val(ttConveyance.Text) + Val(ttMedical.Text) + Val(ttFood.Text) + Val(ttSpecial.Text) + Val(ttLTA.Text) + Val(ttPF.Text)
                        ' CType(dtt.Cells(25).FindControl("Gross"), TextBox).Text = Val(ttbasic.Text) + Val(ttHra.Text) + Val(ttConveyance.Text) + Val(ttMedical.Text) + Val(ttFood.Text) + Val(ttSpecial.Text) + Val(ttLTA.Text)
                        If Val(ttPF.Text) = 0 Then
                            'CType(dtt.Cells(25).FindControl("Gross"), TextBox).Text = Math.Floor(Val(ttbasic.Text) * 100 / 55)
                            CType(dtt.Cells(25).FindControl("Gross"), TextBox).Text = Val(ttbasic.Text) + Val(ttHra.Text) + Val(ttConveyance.Text) + Val(ttMedical.Text) + Val(ttFood.Text) + Val(ttSpecial.Text) + Val(ttLTA.Text)
                        Else
                        
                            CType(dtt.Cells(25).FindControl("Gross"), TextBox).Text = Val(ttbasic.Text) * 100 / 40
                        End If
                    
                        CType(dtt.Cells(26).FindControl("NetSalary"), TextBox).Text = CType(dtt.Cells(24).FindControl("Gross"), TextBox).Text - Val(ttEPF.Text) - Val(strPt) - Val(ttInsurance.Text) - Val(ttAT.Text) - Val(ttLoan.Text) - Val(ttAdvance.Text) - Val(ttLeave.Text) - Val(ttotherDeduction.Text) - Val(ttDeduction.Text) + Val(ttBonus.Text) + Val(ttAddition.Text)
	                
                        Dim rddays As SqlDataReader
                        Dim cmddays As New SqlCommand()
                        Dim conn As New SqlConnection
	               
                        strSQL = "SELECT Day(empLeavingDate) as totPresents FROM employeeMaster WHERE Month(empLeavingDate)= " & Month(dDate) & _
                        " AND Year(empLeavingDate)=" & Year(dDate) & " AND empId=" & Trim(ttempid.Text)
                        conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                        conn.Open()
                        cmddays.Connection = conn
                        cmddays = New SqlCommand(strSQL, conn)
                        rddays = cmddays.ExecuteReader()
                        Dim empTotDays As Integer = 0
                        If (rddays.Read()) Then
                            empTotDays = rddays("totPresents")
                        End If
                        rddays.Close()

                        strSQL = "SELECT count(*) as totPresents FROM empATT WHERE attstatus = 'WL' AND MONTH(ATTDATE)= " & Month(dDate) & _
                        " AND Year(attDate)=" & Year(dDate) & " AND empId=" & Trim(ttempid.Text)
                        conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                        conn.Open()
                        cmddays.Connection = conn
                        cmddays = New SqlCommand(strSQL, conn)
                        rddays = cmddays.ExecuteReader()
                        Dim empTotLeaves As Integer = 0
                        If (rddays.Read()) Then
                            empTotLeaves = rddays("totPresents")
                        End If
                        rddays.Close()
						
                        Dim rddayslatejoin As SqlDataReader
                        Dim cmddayslatejoin As New SqlCommand()
                        Dim connlatejoin As New SqlConnection
                        Dim strsqllatejoin As String
                        strsqllatejoin = "SELECT Day(empJoiningDate) as totPresents FROM employeeMaster WHERE Month(empJoiningDate)= " & Month(dDate) & _
                        " AND Year(empJoiningDate)=" & Year(dDate) & " AND empId=" & Trim(ttempid.Text)
                        connlatejoin = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                        connlatejoin.Open()
                        cmddayslatejoin.Connection = connlatejoin
                        cmddayslatejoin = New SqlCommand(strsqllatejoin, connlatejoin)
                        rddayslatejoin = cmddayslatejoin.ExecuteReader()
                        If (rddayslatejoin.Read()) Then
                            empTotDays = rddayslatejoin("totPresents") - 1
                            empTotDays = Convert.ToInt32(Session.Item("dynoProcesssessiondays")) - empTotDays
                        End If
                        rddayslatejoin.Close()
                        rddayslatejoin.Close()

                        If empTotDays > 0 Then
                            empTotLeaves = empTotLeaves + (Session.Item("dynoProcesssessiondays") - empTotDays)
                        End If
		
                        CType(dtt.Cells(17).FindControl("LEAVE"), TextBox).Text = empTotLeaves
                        CType(dtt.Cells(18).FindControl("PresentDays"), TextBox).Text = Session.Item("dynoProcesssessiondays") - empTotLeaves
                        ttLeave.Text = empTotLeaves
                        ttPF.Text = Val(ttPF.Text) - (Math.Round(Val(ttPF.Text) / Val(Session.Item("dynoProcesssessiondays")) * empTotLeaves))
                        ttEPF.Text = Val(ttEPF.Text) - (Math.Round(Val(ttEPF.Text) / Val(Session.Item("dynoProcesssessiondays")) * empTotLeaves))
                        CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text = CInt(ttPF.Text) + Val(ttPT.Text) + Val(ttInsurance.Text) + Val(ttAT.Text) + Val(ttLoan.Text) + Val(ttAdvance.Text) + Val(ttDeduction.Text) + Val(ttotherDeduction.Text)
			
                        Dim leavededuction As Integer
                        leavededuction = Math.Round(Val(ttGross.Text) / Val(Session.Item("dynoProcesssessiondays")) * empTotLeaves)
                        ' leavededuction = 500
                        ttTotalDeduction.Text = Math.Round(CType(dtt.Cells(23).FindControl("TotalDeduction"), TextBox).Text + leavededuction)
                        CType(dtt.Cells(24).FindControl("CTC"), TextBox).Text = Val(ttbasic.Text) + Val(ttHra.Text) + Val(ttConveyance.Text) + Val(ttMedical.Text) + Val(ttFood.Text) + Val(ttSpecial.Text) + Val(ttLTA.Text) + CInt(ttPF.Text)
                        strGross = CType(dtt.Cells(25).FindControl("gross"), TextBox).Text - leavededuction
                        Dim strTotalPt As String
                    
                        strTotalPt = Val(Val(ttBonus.Text) + Val(ttAddition.Text)) + Val(strGross)
                        If (Val(strTotalPt) <= CInt(2500)) Then
                            strPt = 0
	              
                        ElseIf ((CInt(2501) <= Val(strTotalPt)) And (Val(strTotalPt) <= CInt(3500))) Then
                            strPt = 60
	            
                        ElseIf ((CInt(3501) <= Val(strTotalPt)) And (Val(strTotalPt) <= CInt(5000))) Then
                            strPt = 120
	            
	                
                        ElseIf ((CInt(5001) <= Val(strTotalPt)) And (Val(strTotalPt) <= CInt(10000))) Then
                            strPt = 175
	            
                        ElseIf (CInt(10001) <= Val(strTotalPt)) Then
                            If Month(DateAdd("m", -1, Now)) = 2 Then
                                strPt = 300
                            Else
                                strPt = 200
                            End If
                        End If
	              
	               
	                
                        ttPT.Text = Val(strPt)
                        CType(dtt.Cells(26).FindControl("NetSalary"), TextBox).Text = CType(dtt.Cells(25).FindControl("gross"), TextBox).Text - CInt(ttEPF.Text) - Val(strPt) - Val(ttInsurance.Text) - Val(ttAT.Text) - Val(ttLoan.Text) - Val(ttAdvance.Text) - leavededuction - Val(ttDeduction.Text) - Val(ttotherDeduction.Text) + Val(ttBonus.Text) + Val(ttAddition.Text)
                  
                        dr = dt.NewRow
                        dr("EmpName") = ttempname.Text
                        dr("EmpId") = CInt(ttempid.Text)
                        dr("Basic") = Val(ttbasic.Text)
                        dr("Hra") = Val(ttHra.Text)
                        dr("Conveyance") = Val(ttConveyance.Text)
                        dr("Medical") = Val(ttMedical.Text)
                        dr("Food") = Val(ttFood.Text)
                        dr("Special") = Val(ttSpecial.Text)
                        dr("LTA") = Val(ttLTA.Text)
                        dr("Pf") = Val(ttPF.Text)
                        dr("Epf") = Val(ttEPF.Text)
                        dr("AT") = Val(ttAT.Text)
                        dr("PT") = Val(ttPT.Text)
                        dr("Insurance") = Val(ttInsurance.Text)
                        dr("Loan") = Val(ttLoan.Text)
                        dr("Advance") = Val(ttAdvance.Text)
                        dr("IncomeTax Deduction") = Val(ttDeduction.Text)
                        dr("Other Deduction") = Val(ttotherDeduction.Text)
                        dr("Leave") = Val(ttLeave.Text)
                        dr("PresentDays") = Val(ttPresentDays.Text)
                        dr("TotalDays") = Val(ttTotalDays.Text)
                        dr("Bonus") = Val(ttBonus.Text)
                        dr("Addition") = Val(ttAddition.Text)
                        dr("TotalAddition") = CType(dtt.Cells(22).FindControl("TotalAddition"), TextBox).Text
                        'dr("TotalDeduction") = ctype(dtt.Cells(22).FindControl("TotalDeduction"),textbox).text
                        dr("totalDeduction") = Val(ttTotalDeduction.Text)
                        dr("CTC") = CType(dtt.Cells(24).FindControl("CTC"), TextBox).Text
                   
                        'dr("Gross") = CType(dtt.Cells(25).FindControl("Gross"), TextBox).Text
                        'dr("NetSalary") = CType(dtt.Cells(26).FindControl("NetSalary"), TextBox).Text
                    
                    
                    
                        If Val(ttPF.Text) <> 0 Then
                            dr("NetSalary") = (Val(ttbasic.Text) * 100 / 40) - (Val(ttbasic.Text) * 12 / 100) - (Val(ttPT.Text)) - (Val(ttInsurance.Text))
                            dr("Gross") = Val(ttbasic.Text) * 100 / 40
                        Else
                            dr("NetSalary") = Val(ttbasic.Text) + Val(ttHra.Text) + Val(ttConveyance.Text) + Val(ttMedical.Text) + Val(ttFood.Text) + Val(ttSpecial.Text) + Val(ttLTA.Text) - (Val(ttPT.Text)) - (Val(ttInsurance.Text))
                            dr("Gross") = Val(ttbasic.Text) + Val(ttHra.Text) + Val(ttConveyance.Text) + Val(ttMedical.Text) + Val(ttFood.Text) + Val(ttSpecial.Text) + Val(ttLTA.Text)
                        End If
                     
                        dr("Remarks") = ttRemarks.Text
                        intGctc = intGctc + CType(dtt.Cells(24).FindControl("CTC"), TextBox).Text
                        intGgross = intGgross + CType(dtt.Cells(25).FindControl("Gross"), TextBox).Text
                        IntGnet = IntGnet + CType(dtt.Cells(26).FindControl("NetSalary"), TextBox).Text
                        dt.Rows.Add(dr)
                        Session("dynoReportsession") = dt
                        Session("dynoflgsession") = "1"
                        Response.Write(CType(dtt.Cells(26).FindControl("NetSalary"), TextBox).Text)
                    End If
                Next
	     
                Session.Item("dynoprocessbasicSession") = intGbasic
                Session.Item("dynoProcesssessionCtc") = intGctc
                Session.Item("dynoprocessGrossSession") = intGgross
                Session.Item("dynoprocessNetSession") = IntGnet
                Response.Redirect("/admin/PostForm2.aspx")
                
            Catch ex As Exception
                       Response.Write(ex.Message)   
            End Try
        End Sub
	

        Sub dgrdDummy_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim tctc As Label
                tctc = e.Item.FindControl("CTC")
                Dim cellValue As String = (tctc.Text) & ""
		
                'DataBinder.Eval(e.Item.DataItem, "CTC")
		
                If cellValue <> "" Then
                    ctcsum += Convert.ToDouble(cellValue)
                End If


                Dim tgross As Label
                tgross = e.Item.FindControl("Gross")
                Dim cellValue1 As String = (tgross.Text) & ""


                'DataBinder.Eval(e.Item.DataItem, "GROSS")
                If cellValue1 <> "" Then
                    grosssum += Convert.ToDouble(cellValue1)
                End If

                Dim tnetsalary As Label
                tnetsalary = e.Item.FindControl("NetSalary")
                Dim cellValue2 As String = (tnetsalary.Text) & ""
                'DataBinder.Eval(e.Item.DataItem, "NETSALARY")
                If cellValue2 <> "" Then
  	   
                    netsum += Convert.ToDouble(cellValue2)
		 	
			
                End If

            End If
            Session("NetAmount") = Format(netsum, "0,00")
            lblGrandTotal.Text = "Total:"
            LBLGrandCtc.Text = Format(ctcsum, "0,00")
            LBLGrandGross.Text = Format(grosssum, "0,00")
            LBLGrandNet.Text = Format(netsum, "0,00")
            Session("LBLGrandNet") = LBLGrandNet.Text
	
        End Sub

        Dim sum As Double
        Dim sum_price, sun_net As Double
        Sub DisplayTotal(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim cellValue As String = DataBinder.Eval(e.Item.DataItem, "totalctc") & ""
                If cellValue <> "" Then
                    sum += Convert.ToDouble(cellValue)
                End If

                cellValue = DataBinder.Eval(e.Item.DataItem, "totalgross") & ""
                If cellValue <> "" Then
                    sum_price += Convert.ToDouble(cellValue)
                End If

                cellValue = DataBinder.Eval(e.Item.DataItem, "totalnet") & ""
                If cellValue <> "" Then
                    sun_net += Convert.ToDouble(cellValue)
                End If

            ElseIf e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(0).Text = "<b>Grand Total</b>"
                e.Item.Cells(22).Text = "<b>" & Format(sum, "0,00") & "</b>"
                e.Item.Cells(23).Text = "<b>" & Format(sum_price, "0,00") & "</b>"
                e.Item.Cells(24).Text = "<b>" & Format(sun_net, "0,00") & "</b>"
            End If
        End Sub

        Function getBasic(ByVal basic As Integer, ByVal pf As Integer) As Integer
            If IsNumeric(basic) <> IsNumeric(0) Then
                If IsNumeric(basic) And IsNumeric(pf) Then
                    basic = (basic / (basic * 0.12)) * pf
                End If
            End If
            getBasic = Math.Round(basic)
        End Function
    </script>

</head>
<body>
    <ucl:adminMenu ID="adminMenu" runat="server" />
    <form id="postForm" name="postForm" method="post" runat="server">
        <table id="Table3" cellspacing="0" cellpadding="0" style="width: 100%;">
            <%
                If Not IsDate(payDate) Then
            %>
            <tr valign="top" style="background-color: #EDF2E6">
                <td height="30">
                    <p align="left">
                        <font face="Verdana" color="#A2921E"><b><span style="font-size: 14pt">Pay Comment</span></b></font>
                        <br />
                    </p>
                 
                </td>
            </tr>
            <tr valign="top" style="height: 10%">
                <td align="left" bgcolor="#edf2e6">
                    <textarea id="PayNotes" runat="server" rows="2" cols="70" name="Textarea1" style="font-weight: bold; font-size: 11pt; color: black; font-family: Verdana"></textarea>
                </td>
            </tr>
            <tr style="height: 10%; background-color: #EDF2E6" align="left">
                <td height="40%">
                    <font face="Verdana" color="#A2921E"></font>
                    <asp:Button ID="btncalculation" OnClick="btncalculation_Click" runat="server" Width="90px"
                        Text="Continue" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"></asp:Button>
                    <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Width="90px"
                        Text="Cancel" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"></asp:Button>
                     <div style="text-align:right  ; font-family:Verdana ;"   > <b>Location: <span id="spanLocation" runat ="server">Mumbai</span> </b></div>
                </td>
            </tr>
            <tr>
                <td height="10%" align="left" bgcolor="#c5d5ae">
                    <b><font face="Verdana" color="#a2921e" size="4"></font></b>
                    <asp:Label Font-Size="11" Font-Name="Verdana" Font-Bold="True" ID="LBLprocess" runat="server"
                        Text=" ">
                    </asp:Label>
                    <asp:Label Font-Size="11" Font-Name="Verdana" Font-Bold="True" ID="LBLdays" runat="server"
                        Text=" ">
                    </asp:Label> </td>
            </tr>

            
            <tr style="height: 60%" valign="top">
                <td height="100%">
                    <asp:DataGrid ID="dgrdprocess" CssClass="text" DataKeyField="empId" runat="server"
                        BorderColor="" Font-Size="10pt" Font-Name="Verdana" BackColor="White" Font-Names="Verdana"
                        AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" Width="100%"
                        autopostback="true" HeaderStyle-Font-Size="10pt" HeaderStyle-ForeColor="#A2921E"
                        HeaderStyle-Font-Bold="True" AllowSorting="True" HeaderStyle-BackColor="#C5D5AE"
                        CellPadding="0" OnItemDataBound="DisplayTotal">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Select">
                                <HeaderTemplate>
                                    Select All
                             <input id="chkAll" onclick="SelectAll(this);" runat="server" type="checkbox" checked="true" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Sr Nn">
                                <ItemTemplate>
                                    <%# Container.ItemIndex+1%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                                <HeaderTemplate>
                                    EmpName
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtempname" readonly="true" runat="server" Width="90px" Text='<%# container.dataitem("empname")%>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                                <HeaderTemplate>
                                    EmpId
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtEmpId" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("empId")%>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                                <HeaderTemplate>
                                    Confirmation Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtConfirmDate" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("empConfirmDate")%>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                                <HeaderTemplate>
                                    Basic
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="basic" runat="server" Text='<%#container.dataitem("Basic")%>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                                <HeaderTemplate>
                                    HRA
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="HRA" runat="server" Text='<%# container.dataitem("Hra")%>'
                                        DataFormatString="{0:##,###,###}">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Conveya
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Conveyance" runat="server" Text='<%# container.dataitem("Conveyance") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Medical
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Medical" runat="server" Text='<%# container.dataitem("Medical") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                                <HeaderTemplate>
                                    Food
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Food" runat="server" Text='<%# container.dataitem("Food") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Speical
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Special" runat="server" Text='<%# container.dataitem("Special") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    LTA
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="LTA" runat="server" Text='<%# container.dataitem("LTA") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    EPF
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="EPF" runat="server" Text='<%# container.dataitem("EPF") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    PF
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="PF" runat="server" Text='<%# container.dataitem("PF") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    AdvTax
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="AT" runat="server" Text='<%# container.dataitem("AT") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    PT
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="PT" runat="server" Text='<%# container.dataitem("PT") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Loan
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Loan" runat="server" Text='<%# container.dataitem("Loan") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Advance
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Advance" runat="server" Text='<%# container.dataitem("Advance") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    IncomeTax Deductions
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="DOthers" runat="server" Text='<%# container.dataitem("Deduction") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    other Deductions
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Otherdeduction" runat="server" Text='<%# container.dataitem("otherDeduction") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Leave
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="leave" runat="server" Text='<%# container.dataitem("Leave") %>'
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Present
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="PresentDays" runat="server" Text='<%# container.dataitem("PresentDays") %>'
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Total days
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="totaldays" runat="server" Text='<%# container.dataitem("TotalDays") %>'
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Bonus
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Bonus" runat="server" Text='<%# container.dataitem("Bonus") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Additions
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="AOthers" runat="server" Text='<%# container.dataitem("Addition") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Total-Addition
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="True" Width="50px" ID="TotalAddition" runat="server" Text='<%# container.dataitem("TotalAddition") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Total-Deduction
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="True" Width="50px" ID="TotalDeduction" runat="server" Text='<%# container.dataitem("TotalDeduction") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF"></ItemStyle>
                                <HeaderTemplate>
                                    CTC
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="True" Width="50px" ID="CTC" runat="server" Text='<%# container.dataitem("totalctc") %>'
                                        TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF"></ItemStyle>
                                <HeaderTemplate>
                                    Gross
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="True" Width="40px" ID="Gross" runat="server" Text='<%# container.dataitem("totalgross") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="false">
                                <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    NetSalary
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ReadOnly="True" Width="50px" ID="NetSalary" runat="server" Text='<%# container.dataitem("totalnet") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                                <FooterStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%=LBLGrandNet.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Remark
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox Width="60px" ID="Remarks" runat="server">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                                  <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Insurance
                                </HeaderTemplate>
                                 <ItemTemplate>
                                    <asp:TextBox Width="50px" ID="Insurance" runat="server" Text='<%# container.dataitem("Insurance") %>'
                                        DataFormatString="{0:##,###,###}" TextMode="singleline">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <%
            Else
            %>
            <tr bgcolor="#C5D5AE">
                <td style="height: 25px">
                    <a href="javascript: payDisp('<%=dDate%>');"><font face="Verdana" size="2" color="#A2921E">
                        <b>Bank Statment</b></font></a>
                </td>
            </tr>
            <tr bgcolor="#EDF2E6">
                <td>
                    <p align="left">
                        <font face="Verdana" color="#A2921E"><b><span style="font-size: 12pt">Pay Comment</span></b></font>
                        </p>
                      <div style="text-align:right  ; font-family:Verdana ;"   > <b>Location: <span id="spanLocation1" runat ="server">Mumbai</span> </b></div> 
                </td>
            </tr>
            <tr>
                <td bgcolor="#edf2e6">
                    <asp:Label Font-Name="Verdana" ID="PNotes" Font-Size="13px" Font-Bold="True" readonly="true"
                        runat="server" rows="2" cols="70" BgColor="#edf2e6"></asp:Label>
                </td>
            </tr>
            <tr>
                <td bgcolor="#edf2e6">
                    <asp:Button ID="btndelete" OnClick="btndelete_Click" runat="server" Width="90px"
                        Text="Delete" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"></asp:Button>
                </td>
            </tr>
            <tr>
                <td style ="width :100%">
                    <asp:DataGrid ID="dgrdDummy" CssClass="text" runat="server" BorderColor="" Font-Size="10pt" 
                        Font-Name="Verdana" BackColor="White" Font-Names="Verdana" AutoGenerateColumns="false"
                        ShowFooter="true" Width="1200px" autopostback="true" HeaderStyle-Font-Size="10pt"
                        HeaderStyle-ForeColor="#A2921E" HeaderStyle-Font-Bold="True" AllowSorting="True"
                        HeaderStyle-BackColor="#C5D5AE" CellPadding="2" CellSpacing="0" OnItemDataBound="dgrdDummy_ItemDataBound">
                        <ItemStyle HorizontalAlign="center" BackColor="#FFFFEE"></ItemStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                <HeaderTemplate>
                                    Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtempname" readonly="true" runat="server" Width="90px" Text='<%# container.dataitem("Empname")%>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="left" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%= lblGrandTotal.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="ID" DataField="Empid" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Basic" DataField="Basic" DataFormatString="{0:##,###,###}" />
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Cal Basic
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#getBasic(container.dataitem("Basic"), container.dataitem("EPF"))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="HRA" DataField="Hra" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Conv" DataField="Conveyance"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Med" DataField="Medical" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Food" DataField="Food" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Spec" DataField="Special" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="LTA" DataField="Lta" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="EPF" DataField="Epf" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="PF" DataField="Pf" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="AT" DataField="AT" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="PT" DataField="PT" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Insurance" DataField="Insurance" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Loan" DataField="Loan" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Adv" DataField="Advance" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Dedu" DataField="Deduction"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText=" Other Dedu" DataField="otherDeduction"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Leav" DataField="Leave" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Pres" DataField="PresentDays"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Bonu" DataField="Bonus" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Addi" DataField="Addition"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="TotA" DataField="TotalAddition"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="TotD" DataField="TotalDeduction"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <HeaderTemplate>
                                    CTC
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="CTC" readonly="true" runat="server" Width="70px" Text='<%# DataBinder.Eval(container.dataitem,"CTC","{0:##,###,###}")  %>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%=LBLGrandCtc.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <HeaderTemplate>
                                    Gross
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Gross" readonly="true" runat="server" Width="60px" Text='<%# DataBinder.eval(container.dataitem,"Gross","{0:##,###,###}") %>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%= LBLGrandGross.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <HeaderTemplate>
                                    Net
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="NetSalary" readonly="true" runat="server" Width="70px" Text='<%# databinder.eval(container.dataitem,"NetSalary","{0:##,###,###}") %>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%=LBLGrandNet.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Remark
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="remark" runat="server" Width="10px" Text=' <%#container.dataitem("Remarks") %>'>   </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tblTotal" cellspacing="0" cellpadding="0" width="1550px" border="0" runat="server">
                        <tr>
                            <td valign="middle" align="left" width="1018px" colspan="1" rowspan="1">
                                <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="True" font-Name="Verdana"
                                    Font-Size="10" Visible="false"></asp:Label></td>
                            <td valign="middle" align="right" width="58.4px" colspan="1" rowspan="1">
                                <asp:Label ID="LBLGrandCtc" runat="server" Font-Bold="True" font-Name="Verdana" Font-Size="10"
                                    Visible="false"></asp:Label></td>
                            <td valign="middle" align="left" width="33.2px" colspan="1" rowspan="1">
                                <asp:Label ID="LBLGrandGross" runat="server" Font-Bold="True" font-Name="Verdana"
                                    Font-Size="10" Visible="false"></asp:Label></td>
                            <td valign="middle" align="left" width="98.4px" colspan="1" rowspan="1">
                                <asp:Label ID="LBLGrandNet" runat="server" Font-Bold="True" font-Name="Verdana" Font-Size="10"
                                    Visible="false"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%
            End If
            %>
        </table>
        <input type="hidden" id="hdnid" runat="server" />
        <input type="hidden" id="hdmonth" runat="server" />
        <input type="hidden" id="hdyear" runat="server" />
    </form>
</body>
</html>
