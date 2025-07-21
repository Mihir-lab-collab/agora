<%@ Page Language="VB" Debug="TRUE" EnableViewState="true" %>
<%@ Register Src="../controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Web.Mail" %>
<%@ Import Namespace="System.IO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD html 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml"  >
<head>
<title>Add Pay Details</title>

<script language="JavaScript" type="text/javascript">
    function payClose() {
        window.close();
    }
    window.onload = ChageParentStyle;
    function ChageParentStyle()
    {
        window.opener.document.getElementById('lnkActive').style.color = "#A2921E";
        window.opener.document.getElementById('lnkInactive').style.color = "#A2921E";
        window.opener.document.getElementById('lnlAddNewRev').style.color = "Black";                  
        return false;
    }
</script>
</head>
<script language="VB" runat="server">
    Dim strsql As String
    Dim gf As New generalFunction
    Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
    Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        gf.checkEmpLogin()
        If Not IsPostBack Then
            Dim empId As Integer
            Dim empName As String
            Dim sql As String
            empId = Request.QueryString("empId")
            empName = Request.QueryString("empName")
            Dim str, strempid, strempname As String
            sql = "select empid,empname + '(' +(str(empid))+')' as empname from employeemaster where " & _
            "employeemaster.empleavingdate is Null "
            conn.Open()
            Dim Cmd As New SqlCommand(sql, conn)

            Dim Rdr As SqlDataReader
            Rdr = Cmd.ExecuteReader()
        
            While Rdr.Read
                strempname = Rdr("empName")
                strempid = Rdr("empId")
                str = strempname & "(" & strempid & ")"
                ddlcategory.Items.Add(New ListItem(Rdr.GetString(1), Rdr.GetValue(0)))
            End While
            ddlcategory.Items.Insert(0, "Select")
            Cmd.Dispose()
            conn.Close()
			 
            If (Request.QueryString("empid") <> 0) Then
                sql = "select * from EmployeeMaster where empId=" & Request.QueryString("empid")
                conn.Open()
                Dim ds As DataSet
                Dim adp As New SqlDataAdapter(sql, conn)
                ds = New DataSet()
                adp.Fill(ds)
                ddlcategory.DataSource = ds
                ddlcategory.DataTextField = "empName"
                ddlcategory.DataValueField = "empId"
                ddlcategory.DataBind()
                conn.Close()
            End If
            loadData()
        End If
    End Sub
	
    Sub BTclose_OnClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim sql As String
        Dim empId As Integer
        empId = ddlcategory.SelectedItem.Value
        sql = "delete from  employeePayMaster where  empId = " & empId
        conn.Open()
        Dim sqldeletecommad As New SqlCommand(sql, conn)
        sqldeletecommad.ExecuteNonQuery()
        conn.Close()
		
        Dim strsql As String
        Dim sqlCheck As String
        Dim flag As String
        
        Dim intPayMonth, intPayYear, intPayBasic, intPayHra, intPayConveyance, intPayMedical As Integer
        Dim intPayFood, intPaySpecial, intPayLta, intPayPt, intBonus, boolChecked, perfChecked, intPayInsurance As Integer
        intPayMonth = payMonth.Value
        intPayYear = payYear.Value
        intPayBasic = paybasic.Value
        intPayHra = payhra.Value
        intPayConveyance = payConveyance.Value
        intPayMedical = payMedical.Value
        intPayFood = payfood.Value
        intPaySpecial = paySpecial.Value
        intPayLta = payLta.Value
        intPayInsurance = PayInsurance.Value
        intPayPt = payPT.Value
        intBonus = payBonus.Value
        boolChecked = chkperBased.Checked
        
        If boolChecked = True Then
            perfChecked = 1
        Else
            perfChecked = 0

        End If
        flag = 0
        sqlCheck = "select * from  employeepaymaster where  empId = " & empId & " and empPayMonth = " & _
        intPayMonth & " and  empPayYear = " & intPayYear
        conn.Open()
        Dim cmdcheck As SqlCommand = New SqlCommand(sqlCheck, conn)
        Dim drCheck As SqlDataReader
        drCheck = cmdcheck.ExecuteReader()
        If drCheck.Read() Then
            flag = 1
        End If
        drCheck.Close()
        conn.Close()
        If flag = 0 Then
            strsql = "INSERT INTO employeepayMaster (empid,empPayMonth,empPayYear,empPayBasic,empPayHra, " & _
            "empPayConveyance,empPayMedical,empPayFood,empPaySpecial,empPayLTA,empPT, EmpInsurance,IsPerformanceBased,empPayBonus,IsPF,ModifiedBy) Values (" & _
            empId & "," & intPayMonth & "," & intPayYear & ", " & intPayBasic & "," & _
            intPayHra & " ," & intPayConveyance & _
            "," & intPayMedical & " ," & intPayFood & "," & intPaySpecial & "," & intPayLta & "," & intPayPt & "," & intPayInsurance & "," & perfChecked & "," & intBonus & ", '" & chkPF.Checked & "'," & Session("dynoempIdsession") & ")"
            conn.Open()
            Dim addcmd As SqlCommand = New SqlCommand(strsql, conn)
            addcmd.Connection = conn
            addcmd.CommandText = strsql
            addcmd.ExecuteNonQuery()
            conn.Close()
            Dim sp1 As String
            sp1 = "<Script language=JavaScript>"
            sp1 += " alert('Record Saved '); "
            sp1 += " opener.location.href = opener.location.href; "
            sp1 += " window.close() ; "
            sp1 += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType, "script123", sp1)
        Else
            Dim sp1 As String
            sp1 = "<Script language=JavaScript>"
            sp1 += " alert('Already revision is present for employee for month " + MonthName(Month(CStr(intPayMonth) & "/1" & "/2006")) + " '); "
            sp1 += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType, "script123", sp1)
			
        End If
    End Sub

    Sub loadData()
        Dim i As Integer
        For i = 1 To 12
            payMonth.Items.Add(New ListItem(Left(MonthName(i), 3), i))
        Next
        payMonth.SelectedIndex = Month(Now()) - 1
        i = Year(Now()) - 1
        Do While i < Year(Now()) + 2
            payYear.Items.Add(i)
            i = i + 1
        Loop
        payYear.Value = Year(Now())
    End Sub
    
    Sub clear()
        Dim yr As String
        yr = "select Year(getdate())"
        payYear.Value = yr
        paybasic.Value = 0
        payhra.Value = 0
        payConveyance.Value = 0
        payMedical.Value = 0
        payfood.Value = 0
        paySpecial.Value = 0
        payLta.Value = 0
        payPT.Value = 0
    End Sub
  
    Sub ddlcategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim search As String
        search = ddlcategory.SelectedItem.Value
        Session.Item("dynoempIdsession") = search
        Dim Cmd As New SqlCommand
        Dim ds As New DataSet
        conn.Open()
        Cmd.CommandText = CommandType.Text
        Dim intHra, intbasic, intConveyance, intMedical, intFood, intSpecial, intLTA, intPT As Integer
        strsql = "select max(p.empid) as empId,max(p.empPaybasic) as basic,max(p.empPayHra) as HRA,max(p.empPayConveyance) as Conveyance,max(p.empPayMedical) as Medical,max(p.empPayFood) as Food,max(p.empPaySpecial) as Special,max(p.empPaybasic)*12/100 as PF,max(p.empPaybasic)*12/100 as EPF,max(p.empPayLta) as LTA,max(p.empPT) as PT  , MAX(p.empPayBonus) as BONUS from employeepaymaster  as p,employeemaster as e where p.empid='" & search & "'"
        Dim Cmdsql As New SqlCommand(strsql, conn)
        Dim Rdrsql As SqlDataReader
        Rdrsql = Cmdsql.ExecuteReader()
        If Rdrsql.Read() Then
            If IsDBNull(Rdrsql("Basic")) Then
                clear()
                Session.Item("dynobasicsession") = 0
            Else
                intbasic = Rdrsql("Basic")
                paybasic.Value = intbasic
                Session.Item("dynobasicsession") = intbasic
            End If
  	      
            If IsDBNull(Rdrsql("HRA")) Then
                Session.Item("dynoHra") = 0
            Else
                intHra = Rdrsql("HRA")
                payhra.Value = intHra
                Session.Item("dynoHra") = intHra
            End If
         
            If IsDBNull(Rdrsql("Conveyance")) Then
                Session.Item("dynoConveyance") = 0
            Else
                intConveyance = Rdrsql("Conveyance")
                payConveyance.Value = intConveyance
                Session.Item("dynoConveyance") = intConveyance
            End If

            If IsDBNull(Rdrsql("Medical")) Then
                Session.Item("dynoMedical") = 0
            Else
                intMedical = Rdrsql("Medical")
                payMedical.Value = intMedical
                Session.Item("dynoMedical") = intMedical
            End If

            If IsDBNull(Rdrsql("Food")) Then
                Session.Item("dynoFood") = 0
            Else
                intFood = Rdrsql("Food")
                payfood.Value = intFood
                Session.Item("dynoFood") = intFood
            End If

            If IsDBNull(Rdrsql("Special")) Then
                Session.Item("dynoSpecial") = 0
            Else
                intSpecial = Rdrsql("Special")
                paySpecial.Value = intSpecial
                Session.Item("dynoSpecial") = intSpecial
            End If
         
    
            If IsDBNull(Rdrsql("LTA")) Then
                Session.Item("dynoLTA") = 0
            Else
                intLTA = Rdrsql("LTA")
                payLta.Value = intLTA
                Session.Item("dynoLTA") = intLTA
            End If
        
            If IsDBNull(Rdrsql("PT")) Then
                Session.Item("dynoPT") = 0
            Else
                intPT = Rdrsql("PT")
                payPT.Value = intPT
                Session.Item("dynoPT") = intPT
            End If

        End If
        conn.Close()
    End Sub
</script>
<body>
<form runat="server" id="projForm" onsubmit="return validateForm()">
    <table cellpadding="4" width="100%" border="1" cellspacing="1">
        <tr>
            <td style="background-color: #C5D5AE" colspan="4">
                <font face="Verdana" color="#a2921e"><b>Employee Salary Details</b></font></td>
        </tr>
        <tr>
            <td style="background-color: #edf2e6" nowrap="nowrap"="nowrap="nowrap"" width="150" valign="top">
                <font face="Verdana" color="#a2921e" size="2"><b>Employee Name </b></font>
            </td>
            <td valign="top">
                <font face="verdana" size="2"><b>
                    <asp:DropDownList ID="ddlcategory" runat="server" CssClass="cssData" AutoPostBack="true"
                        EnableViewState="true" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged"
                        Width="200">
                    </asp:DropDownList>
                </b></font>
            </td>
            <td valign="top" bgcolor="#edf2e6" align="left">
                <font face="Verdana" color="#a2921e" size="2"><b>Gross Salary</b></font>
            </td>
            <td valign="top" align="right" nowrap="nowrap"="nowrap="nowrap"">
                <input type="text" id="txtGross" name="txtGross" size="20" onkeypress="return numericCharacters(event);" />
                <input type="button" name="calculate" onclick="fillvalueWithCheckGross()" value="calculate" />
            </td>
        </tr>
        <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Effective From</b></font></td>
            <td width="25%">
                <select size="1" id="payMonth" runat="server" name="payMonth" width="100" tabindex="1">
                </select>
                <select size="1" id="payYear" runat="server" name="payYear" tabindex="2">
                </select>
                &nbsp;</td>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>L T A</b></font></td>
            <td width="25%">
                <input type="text" id="payLta" value="0" runat="server" onkeypress="return numericCharacters(event);"
                    onblur="calculate1();" width="0" size="20" /></td>
        </tr>
        <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Basic Salary</b></font></td>
            <td width="25%">
                <input name="text" type="text" id="paybasic" tabindex="3" onkeypress="return numericCharacters(event);"
                    onblur="calculate1();" value="0" size="20" width="0" runat="server" />
                &nbsp;</td>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Food Allowance</b></font></td>
            <td width="25%">
                <input type="text" id="payfood" value="0" runat="server" onkeypress="return numericCharacters(event);"
                    onblur="calculate1();" width="0" size="20" />
            </td>
        </tr>
        <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>House Rent Allowance </b></font>
            </td>
            <td width="25%">
                <input type="text" id="payhra" value="0" runat="server" onkeypress="return numericCharacters(event);"
                    onblur="calculate1();" width="0" size="20" tabindex="4" />
            </td>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Special Allowance </b></font>
            </td>
            <td width="25%">
                <input type="text" id="paySpecial" value="0" runat="server" onkeypress="return numericCharacters(event);"
                    onblur="calculate1();" width="0" size="20" />
            </td>
        </tr>
        <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Conveyance</b></font></td>
            <td width="25%">
                <input type="text" id="payConveyance" value="0" onkeypress="return numericCharacters(event);"
                    onblur="calculate1();" runat="server" width="0" size="20" tabindex="5" />
            </td>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Gross</b></font></td>
            <td width="25%" bgcolor="#edf2e6">
                <input type="text" id="Gross" value="0" style="background-color: #edf2e6" readonly="readonly" />
            </td>
        </tr>
        <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Medical Allowance</b></font></td>
            <td width="25%">
                <input type="text" id="payMedical" value="0" runat="server" onkeypress="return numericCharacters(event);"
                    onblur="calculate1();" width="0" size="20" tabindex="6" />
            </td>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>CTC</b></font></td>
            <td width="25%" bgcolor="#edf2e6">
                <input type="text" id="CTC" value="0" style="background-color: #edf2e6" readonly="readonly" />
            </td>
        </tr>
        <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Provident Fund</b></font></td>
            <td width="25%" bgcolor="#edf2e6">
                    <asp:CheckBox ID="chkPF" runat="server" onclick="addPF();"/>
                </td>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Net Salary</b></font></td>
            <td width="25%" bgcolor="#edf2e6">
                <input type="text" id="Net" value="0" style="background-color: #edf2e6" readonly="readonly" />
            </td>
        </tr>
        <tr>
            <td bgcolor="#C5D5AE" colspan="4">
                <font face="Verdana" color="#a2921e"><b>Deductions</b></font></td>
        </tr>
        <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Professional Tax </b></font>
            </td>
            <td width="25%">
                <input type="text" id="payPT" onkeypress="return numericCharacters(event);" value="0"  onblur="calculate1();" 
                    runat="server" width="0" size="20" tabindex="7" />
            </td>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Insurance Premium </b></font>
            </td>
            <td width="25%">
                <input type="text" id="PayInsurance" onkeypress="return numericCharacters(event);" value="0" onblur="calculate1();" 
                    runat="server" width="0" size="20" tabindex="7" />
            </td>

        </tr>
            <tr>
            <td bgcolor="#C5D5AE" colspan="4">
                <font face="Verdana" color="#a2921e"><b>Annual</b></font></td>
        </tr>
            <tr>
            <td width="25%" bgcolor="#edf2e6">
                <font face="Verdana" color="#a2921e" size="2"><b>Bonus </b></font>
            </td>
            <td width="25%">
                <input type="text" id="payBonus" onkeypress="return numericCharacters(event);" value="0"
                    runat="server" width="0" size="20" tabindex="7" />
            </td>
                <td width="25%" bgcolor="#edf2e6" colspan="2" >
                    <asp:CheckBox ID="chkperBased" runat="server" Text="Performance Based"  />
            </td>
               
        </tr>
        <tr>
            <td colspan="4" bgcolor="#edf2e6" align="center">
                <input type="button" id="BTclose" runat="server" value="SAVE-CLOSE" width="80" style="font-family: Verdana;
                    font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                    font-bold="true" onserverclick="BTclose_OnClick" name="BTclose" />
            </td>
        </tr>
    </table>
        <asp:HiddenField ID="hdnPF" runat="server" />
        <asp:HiddenField ID="hdnBasic" runat="server" />
        <asp:HiddenField ID="hdnSpecialAll" runat="server" />

        <asp:HiddenField ID="hdnGross" runat="server" />
        <asp:HiddenField ID="hdnCTC" runat="server" />
        <asp:HiddenField ID="hdnNet" runat="server" />
</form>
</body>
</html>

<script language="javascript" type="text/javascript">

function validateForm() {
    var txtGross = document.forms["projForm"]["txtGross"].value;
    var Gross = document.forms["projForm"]["Gross"].value;
    var CTC = document.forms["projForm"]["CTC"].value;
    var Net = document.forms["projForm"]["Net"].value;

    if (txtGross == null || txtGross == "" || Gross == null || Gross == "" || Gross == 0 || CTC == null || CTC == "" || CTC == 0 || Net == null || Net == "" || Net == 0) {
        alert("Input fields must be filled before submit.");
        return false;
    }
}
function addPF() {
    var chk = document.getElementById("<%=chkPF.ClientID %>");
    var Gross = document.getElementById("txtGross").value;
    var Gross1 = document.getElementById("Gross").value;
    var PF;
    if (Gross != "" && Gross1 != 0 && Gross >= 12000) {
        if (chk.checked == true) {

            fillvalue40();

        }
        else {
            fillvalue55();
        }
    }
    else {
        chk.checked = false;
    }


}

function fillvalueWithCheckGross() {
    var chk = document.getElementById("<%=chkPF.ClientID %>");

    if ((document.getElementById("txtGross").value != "") && (parseInt(document.getElementById("txtGross").value) > 0)) {

        if ((document.getElementById("txtGross").value != "") && (parseInt(document.getElementById("txtGross").value) >= 12000)) {
            if (chk.checked == false)
                fillvalue55();
            else
                fillvalue40();
        }
        else {
            fillvalueBasicUpto6500();
        }
    }
}



function fillvalueBasicUpto6500() {
    var Basic = 0, HRA = 0, Conv = 0, Medical = 0, LTA = 0, Food = 0, Special = 0, PTax = 0, gross = 0, balance = 0;

    gross = parseInt(document.getElementById("txtGross").value);
    balance = gross;
    Basic = gross * .55;
    if (Basic <= 6600 && gross > 6600) {
        Basic = 6600;
    }
    else {
        Basic = gross;
    }
    document.getElementById("paybasic").value = Basic;

    balance = balance - Basic

    Conv = 800;
    if (Conv >= balance) {
        Conv = balance;
    }
    document.getElementById("payConveyance").value = Conv;

    balance = balance - Conv;

    Medical = 1500;

    if (balance < Medical) {
        Medical = balance;
    }
    document.getElementById("payMedical").value = Medical;

    balance = balance - Medical;




    HRA = Basic * 0.5;
    if (HRA <= balance) {
        HRA = Basic * 0.5; ;
    }
    else {
        HRA = balance;
    }
    document.getElementById("payhra").value = HRA;
    balance = balance - HRA;
    if (balance > 0) {
        if (gross > 15000) {
            Food = 2000;
        }
        else if (gross < 15000) {
            Food = 1000;
        }
        if (balance < Food) {
            Food = balance;
        }
    }
    document.getElementById("payfood").value = Food;
    balance = balance - Food

    if (balance > 0) {
        if (gross > 15000) {
            LTA = 2000;
        }
        else if (gross < 15000) {
            LTA = 1000;
        }
        if (balance < LTA) {
            LTA = balance;
        }
    }
    balance = balance - LTA

    document.getElementById("payLta").value = LTA;
    Special = gross - (Basic + HRA + Conv + LTA + Food + Medical);
    document.getElementById("paySpecial").value = Special;
    if (gross <= 2000) {
        PTax = 0;
    }
    else if ((gross > 2000) && (gross <= 2500)) {
        PTax = 30;
    }
    else if ((gross > 2500) && (gross <= 3000)) {
        PTax = 60;
    }
    else if ((gross > 3000) && (gross <= 5000)) {
        PTax = 120;
    }
    else if ((gross > 5000) && (gross <= 10000)) {
        PTax = 175;
    }
    else {
        PTax = 200;
    }
    document.getElementById("payPT").value = PTax;

    document.getElementById("Gross").value = gross;
    document.getElementById("CTC").value = gross;
    document.getElementById("Net").value = gross - PTax - parseInt(document.getElementById("PayInsurance").value);

}

function fillvalueUpto12000() {
    var Basic = 0, HRA = 0, Conv = 0, Medical = 0, LTA = 0, Food = 0, Special = 0, PTax = 0, gross = 0;
    gross = parseInt(document.getElementById("txtGross").value);
    Basic = gross;

    document.getElementById("paybasic").value = Basic;
    document.getElementById("payhra").value = HRA;
    document.getElementById("payConveyance").value = Conv;
    document.getElementById("payMedical").value = Medical;
    document.getElementById("payfood").value = Food;
    document.getElementById("paySpecial").value = Special;
    document.getElementById("payLta").value = LTA;
    if (gross <= 2000) {
        PTax = 0;
    }
    else if ((gross > 2000) && (gross <= 2500)) {
        PTax = 30;
    }
    else if ((gross > 2500) && (gross <= 3000)) {
        PTax = 60;
    }
    else if ((gross > 3000) && (gross <= 5000)) {
        PTax = 120;
    }
    else if ((gross > 5000) && (gross <= 10000)) {
        PTax = 175;
    }
    else {
        PTax = 200;
    }
    document.getElementById("payPT").value = PTax;
    document.getElementById("Gross").value = gross;
    document.getElementById("CTC").value = gross;
    document.getElementById("Net").value = gross - PTax - parseInt(document.getElementById("PayInsurance").value);

}
function fillvalue40() {
    var Basic, HRA, Conv, Medical = 0, LTA = 0, Food = 0, Special, PTax = 0, gross, BasicPercent = 0.40;
    gross = parseInt(document.getElementById("txtGross").value);
    var chk = document.getElementById("<%=chkPF.ClientID %>");
    balance = gross;

    Basic = gross * BasicPercent;
    document.getElementById("hdnBasic").value = Basic;



    document.getElementById("hdnPF").value = Basic * 0.12; //calculating PF


    balance = balance - Basic;
    document.getElementById("paybasic").value = Basic;
    HRA = gross * BasicPercent * 0.5;
    document.getElementById("payhra").value = HRA;
    balance = balance - HRA;


    Conv = 800;
    document.getElementById("payConveyance").value = Conv;
    balance = balance - Conv;

    Medical = 1500;
    if (balance < Medical) {
        Medical = balance;
    }
    document.getElementById("payMedical").value = Medical;

    balance = balance - Medical;
    if (balance > 0) {
        if (gross > 15000) {
            Food = 2000;
        }
        else if (gross < 15000) {
            Food = 1000;
        }
        if (balance < Food) {
            Food = balance;
        }
    }
    document.getElementById("payfood").value = Food;
    balance = balance - Food

    if (balance > 0) {
        if (gross > 15000) {
            LTA = 2000;
        }
        else if (gross < 15000) {
            LTA = 1000;
        }
        if (balance < LTA) {
            LTA = balance;
        }
    }
    balance = balance - LTA

    document.getElementById("payLta").value = LTA;
    Special = gross - (Basic + HRA + Conv + LTA + Food + Medical);
    document.getElementById("paySpecial").value = Special;
    document.getElementById("hdnSpecialAll").value = Special;
    if (gross <= 2000) {
        PTax = 0;
    }
    else if ((gross > 2000) && (gross <= 2500)) {
        PTax = 30;
    }
    else if ((gross > 2500) && (gross <= 3000)) {
        PTax = 60;
    }
    else if ((gross > 3000) && (gross <= 5000)) {
        PTax = 120;
    }
    else if ((gross > 5000) && (gross <= 10000)) {
        PTax = 175;
    }
    else {
        PTax = 200;
    }
    document.getElementById("payPT").value = PTax;

    document.getElementById("Gross").value = gross;
    document.getElementById("CTC").value = gross + ((Basic * 12) / 100);
    document.getElementById("Net").value = gross - ((Basic * 12) / 100) - PTax - parseInt(document.getElementById("PayInsurance").value);
    document.getElementById("paySpecial").value = Special;//  + ((Basic * 12) / 100);

}


function fillvalue55() {
    var Basic, HRA, Conv, Medical = 0, LTA = 0, Food = 0, Special, PTax = 0, gross, BasicPercent = 0.55;
    gross = parseInt(document.getElementById("txtGross").value);
    var chk = document.getElementById("<%=chkPF.ClientID %>");

    balance = gross;
    Basic = gross * BasicPercent;


    document.getElementById("hdnBasic").value = Math.round(Basic);
    document.getElementById("hdnPF").value = Math.round(Basic * 0.12); //calculating PF
    balance = balance - Basic;
    document.getElementById("paybasic").value = Math.round(Basic);
    HRA = gross * BasicPercent * 0.5;

    document.getElementById("payhra").value = Math.round(HRA);
    balance = balance - HRA;
    Conv = 800;
    document.getElementById("payConveyance").value = Conv;
    balance = balance - Conv;

    Medical = 1500;
    if (balance < Medical) {
        Medical = balance;
    }
    document.getElementById("payMedical").value = Math.round(Medical);

    balance = balance - Medical;
    if (balance > 0) {
        if (gross > 15000) {
            Food = 2000;
        }
        else if (gross < 15000) {
            Food = 1000;
        }
        if (balance < Food) {
            Food = balance;
        }
    }
    document.getElementById("payfood").value = Math.round(Food);
    balance = balance - Food

    if (balance > 0) {
        if (gross > 15000) {
            LTA = 2000;
        }
        else if (gross < 15000) {
            LTA = 1000;
        }
        if (balance < LTA) {
            LTA = balance;
        }
    }
    balance = balance - LTA
    document.getElementById("payLta").value = Math.round(LTA);
    Special = gross - (Basic + HRA + Conv + LTA + Food + Medical);
    document.getElementById("paySpecial").value = Math.round(Special);
    document.getElementById("hdnSpecialAll").value = Math.round(Special);

    if (gross <= 2000) {
        PTax = 0;
    }
    else if ((gross > 2000) && (gross <= 2500)) {
        PTax = 30;
    }
    else if ((gross > 2500) && (gross <= 3000)) {
        PTax = 60;
    }
    else if ((gross > 3000) && (gross <= 5000)) {
        PTax = 120;
    }
    else if ((gross > 5000) && (gross <= 10000)) {
        PTax = 175;
    }
    else {
        PTax = 200;
    }
    document.getElementById("payPT").value = PTax;
    document.getElementById("Gross").value = gross;
    document.getElementById("CTC").value = parseInt(document.getElementById("txtGross").value);
    document.getElementById("Net").value = parseInt(document.getElementById("txtGross").value) - parseInt(document.getElementById("payPT").value) - parseInt(document.getElementById("PayInsurance").value)

}

function fillvalue() {
    var Basic, HRA, Conv, Medical = 0, LTA = 0, Food = 0, Special, PTax = 0, gross, BasicPercent = 0.40;
    gross = parseInt(document.getElementById("txtGross").value);
    window.alert(gross); //-----------------test
    var chk = document.getElementById("<%=chkPF.ClientID %>");
    balance = gross;
    Basic = gross * BasicPercent;
    window.alert(Basic); //-----------------test
    document.getElementById("hdnBasic").value = Math.round(Basic);
    document.getElementById("hdnPF").value = Basic * 0.12; //calculating PF
    balance = balance - Basic;
    document.getElementById("paybasic").value = Math.round(Basic);
    HRA = gross * BasicPercent * 0.5;
    document.getElementById("payhra").value = Math.round(HRA);
    balance = balance - HRA;

    Conv = 800;
    document.getElementById("payConveyance").value = Conv;
    balance = balance - Conv;

    Medical = 1500;
    if (balance < Medical) {
        Medical = balance;
    }
    document.getElementById("payMedical").value = Math.round(Medical);
    balance = balance - Medical;
    if (balance > 0) {
        if (gross > 15000) {
            Food = 2000;
        }
        else if (gross < 15000) {
            Food = 1000;
        }
        if (balance < Food) {
            Food = balance;
        }
    }
    document.getElementById("payfood").value = Math.round(Food);
    balance = balance - Food

    if (balance > 0) {
        if (gross > 15000) {
            LTA = 2000;
        }
        else if (gross < 15000) {
            LTA = 1000;
        }
        if (balance < LTA) {
            LTA = balance;
        }
    }
    balance = balance - LTA
    document.getElementById("payLta").value = Math.round(LTA);
    Special = gross - (Basic + HRA + Conv + LTA + Food + Medical);
    document.getElementById("paySpecial").value = Math.round(Special);
    document.getElementById("hdnSpecialAll").value = Math.round(Special);

    if (gross <= 2000) {
        PTax = 0;
    }
    else if ((gross > 2000) && (gross <= 2500)) {
        PTax = 30;
    }
    else if ((gross > 2500) && (gross <= 3000)) {
        PTax = 60;
    }
    else if ((gross > 3000) && (gross <= 5000)) {
        PTax = 120;
    }
    else if ((gross > 5000) && (gross <= 10000)) {
        PTax = 175;
    }
    else {
        PTax = 200;
    }
    document.getElementById("payPT").value = PTax;

    calculate1();
    calcGrossCTCNet();
}

function calcGrossCTCNet() {
    window.alert(parseInt(document.getElementById("txtGross").value));
    var chk = document.getElementById("<%=chkPF.ClientID %>");
    if (chk.checked == false) {

        document.getElementById("CTC").value = parseInt(document.getElementById("txtGross").value);
        document.getElementById("Net").value = parseInt(document.getElementById("txtGross").value) - parseInt(document.getElementById("payPT").value) - parseInt(document.getElementById("PayInsurance").value)
    }
}

function calcGrossCTCNet() {

    var chk = document.getElementById("<%=chkPF.ClientID %>");
    if (chk.checked == false) {
        document.getElementById("CTC").value = parseInt(document.getElementById("txtGross").value);
        document.getElementById("NET").value = parseInt(document.getElementById("txtGross").value) - parseInt(document.getElementById("payPT").value) - parseInt(document.getElementById("PayInsurance").value)
    }
}

function calculate1() {
    var Basic = 0, HRA = 0, Conv = 0, Medical = 0, LTA = 0, Food = 0, Special = 0, PTax = 0, gross = 0, CTC = 0, Net;
    var chk = document.getElementById("<%=chkPF.ClientID %>");
    if (document.getElementById("paybasic").value != "") {
        Basic = parseInt(document.getElementById("paybasic").value);
        document.getElementById("hdnBasic").value = Basic
        document.getElementById("hdnPF").value = Basic * 0.12;
    }

    if (document.getElementById("payConveyance").value != "")
        Conv = parseInt(document.getElementById("payConveyance").value);
    if (document.getElementById("payhra").value != "")
        HRA = parseInt(document.getElementById("payhra").value);
    if (document.getElementById("payMedical").value != "")
        Medical = parseInt(document.getElementById("payMedical").value);
    if (document.getElementById("payfood").value != "")
        Food = parseInt(document.getElementById("payfood").value);
    if (document.getElementById("payLta").value != "")
        LTA = parseInt(document.getElementById("payLta").value);
    if (document.getElementById("paySpecial").value != "")
        Special = parseInt(document.getElementById("paySpecial").value);
    if (document.getElementById("payPT").value != "")
        PTax = parseInt(document.getElementById("payPT").value);
    gross = Basic + Conv + HRA + Medical + Food + LTA + Special;
    document.getElementById("Gross").value = gross;

    if (chk.checked == true) {
        CTC = (Basic * 12) / 100 + gross
    }
    else
    {
        CTC=gross
    }

    document.getElementById("CTC").value = CTC;

    Net = gross - PTax - parseInt(document.getElementById("PayInsurance").value);

    if (chk.checked == true) {
        Net = Net - (Basic * 12) / 100 ;
    }
    document.getElementById("Net").value = Net;
    document.getElementById("hdnGROSS").value = gross;
    document.getElementById("hdnCTC").value = CTC;
    document.getElementById("hdnNet").value = Net;
    addPF(); 
}

function numericCharacters(event)
{
    if (document.all) {
        if (event.keyCode >= 48 && event.keyCode <= 57) {
            return true;
        }
        else if (event.keyCode == 46) {
            return true;
        }
        else {
            return false;
        }
    }
    if ((!document.all) && (document.getElementById)) {
        if (event.which >= 48 && event.which <= 57) {
            return true;
        }
        else if (event.which == 0 || event.which == 8) {
            return true;
        }
        else if (event.which == 46) {
            return true;
        }
        else {
            return false;
        }
    }
}

</script>

