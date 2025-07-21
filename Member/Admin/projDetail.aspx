<%@ Page Language="VB" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link href="../Javascript/dhtmlxcombo.css" rel="stylesheet" type="text/css" />
<script src="../Javascript/dhtmlxcommon.js" type="text/javascript"></script>
<script src="../Javascript/dhtmlxcombo.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js"> </script>
<script language="javascript" type="text/javascript">
    function moveItem(lstCodeReview, lstAddCodeReview) {
        var selectedValue = "";
        var selectedName = "";
        for (i = 0; i < lstCodeReview.length; i++) {
            if (lstCodeReview[i].selected == true) {
                var TOLength = lstAddCodeReview.length;
                lstAddCodeReview[lstAddCodeReview.length] = new Option(lstCodeReview[i].text, lstCodeReview[i].value);
                lstAddCodeReview.options[TOLength].style.backgroundColor = lstCodeReview.options[i].style.backgroundColor;

            }
        }
        for (i = lstCodeReview.options.length - 1; i >= 0; i--) {
            if (lstCodeReview.options[i].selected)
                lstCodeReview.remove(i);
        }
    }

    function moveteam(lstteam, lstAddTeam) {
        var selectedValue = "";
        var selectedName = "";
        for (i = 0; i < lstteam.length; i++) {
            if (lstteam[i].selected == true) {
                var TOLength = lstAddTeam.length;
                lstAddTeam[lstAddTeam.length] = new Option(lstteam[i].text, lstteam[i].value);
            }
        }
        for (i = lstteam.options.length - 1; i >= 0; i--) {
            if (lstteam.options[i].selected)
                lstteam.remove(i);
        }

    }

    function checkKey() {

        var key = event.keyCode;
        if (key == 32) {
            return false;
        }

    }
</script>
<script language="javascript" type="text/javascript">
    function removeitem(lstCodeReview, lstAddCodeReview) {
        var selectedValue = "";
        var selectedName = "";
        for (i = 0; i < lstCodeReview.length; i++) {
            if (lstCodeReview[i].selected == true) {
                var TOLength = lstAddCodeReview.length
                lstAddCodeReview[lstAddCodeReview.length] = new Option(lstCodeReview[i].text, lstCodeReview[i].value);
                lstAddCodeReview.options[TOLength].style.backgroundColor = lstCodeReview.options[i].style.backgroundColor;

            }
        }
        for (i = lstCodeReview.options.length - 1; i >= 0; i--) {
            if (lstCodeReview.options[i].selected)
                lstCodeReview.remove(i);
        }

    }

    function removeTeam(lstteam, lstAddTeam) {
        var selectedValue = "";
        var selectedName = "";
        for (i = 0; i < lstteam.length; i++) {
            if (lstteam[i].selected == true) {
                var TOLength = lstAddTeam.length;
                lstAddTeam[lstAddTeam.length] = new Option(lstteam[i].text, lstteam[i].value);
            }
        }
        for (i = lstteam.options.length - 1; i >= 0; i--) {
            if (lstteam.options[i].selected)
                lstteam.remove(i);
        }
    }


    function fillTextBox() {
        var msg = "";
        var pName = document.getElementById("projName");
        if (pName.value == "") {
            document.getElementById("errorOnEmptyPrjTitle").style.display = "block"
            msg += "F";
        }
        else {
            document.getElementById("errorOnEmptyPrjTitle").style.display = "none"
            msg += "T";
        }
        var pDate = document.getElementById("projStartDate");
        if (pDate.value == "") {
            document.getElementById("errorOnEmptyStartDate").style.display = "block"
            msg += "F";
        }
        else {
            document.getElementById("errorOnEmptyStartDate").style.display = "none"
            msg += "T";
        }
        if (document.getElementById("errorProjName").style.display == "none") {
            var lineage = '';
            var i;
            var team = '';
            var j;
            for (var i = 0; i < document.getElementById('lstAddCodeReview').length; i++) {
                if (lineage == '') {
                    lineage = document.getElementById('lstAddCodeReview').options[i].value;
                }
                else {
                    lineage = lineage + ',' + document.getElementById('lstAddCodeReview').options[i].value;
                }
            }
            for (var j = 0; j < document.getElementById('lstAddTeam').length; j++) {
                if (team == '') {
                    team = document.getElementById('lstAddTeam').options[j].value;
                }
                else {
                    team = team + ',' + document.getElementById('lstAddTeam').options[j].value;
                }
            }

            document.getElementById('hiddenlineage').value = lineage;
            document.getElementById('hiddenlineage').value = lineage;
            document.getElementById('hiddenTeam').value = team;
            document.getElementById('hiddenTeam').value = team;
            msg += "T";

        }
        else {
            msg += "F";


        }

        var Fc = msg.split("F").length - 1;

        if (Fc > 0) {
            return false;
        }
        else {
            return true;

        }

    }
</script>
<script language="VB" runat="server">
    Dim SortField As String
    Dim listItems As String
    Dim empCode As String
    Dim sql As String
    Dim prosql As String
    Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
    Dim gf As New generalFunction
    
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        gf.checkEmpLogin()
        conn.Open()
        If Not IsPostBack() Then
            loadData()
        End If
              
    End Sub
    Function boolean_to_int(ByVal flag As Boolean) As Integer
        If (flag = True) Then
            boolean_to_int = 1
        Else
            boolean_to_int = 0
        End If
    End Function
    
    'New Functionlity by neeta t.'
    Public Function validateMultipleEmailsCommaSeparated() As Boolean
        If (txtEmail.Text <> "") Then
            Dim emilid As String = txtEmail.Text()
            Dim strArr() As String
            Dim flag As Boolean
            Dim i As Integer
         
            Dim separator As String() = New String() {","}
            strArr = emilid.Split(separator, StringSplitOptions.RemoveEmptyEntries)
            If (strArr.Length <= 10) Then
              
                For i = 0 To strArr.Length - 1
                    If Not (validateEmail(strArr(i))) Then
                        flag = False
                        Exit For
                    Else
                        flag = True
                    End If
                Next
                Return flag
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Total Mail id Exceeded than 10')", True)
                Return False
            End If
            
        Else
            Return True
        End If
        
       
    End Function
    Public Function trim(ByRef stringToTrim As String) As String
        Dim rplstr As String = "/^\s+|\s+$/g"
        Return Regex.Replace(stringToTrim, rplstr, "")
        ' Return stringToTrim.Replace(rplstr, "")
    End Function
    Public Function validateEmail(ByRef strmail As String) As Boolean
        strmail = trim(strmail)
        ' Dim regex As String = "/^[a-z0-9_\+-]+(\.[a-z0-9_\+-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.([a-z]{2,4})$/"
        Dim regex As String = "^(([^<>()[\]\\.,;:\s@\""]+" & "(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@" & "((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}" & "\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+" & "[a-zA-Z]{2,}))$"
       
        If RegularExpressions.Regex.IsMatch(strmail, regex) Then
            Return True
        Else
            Return False
        End If
    End Function
    'end of New Functionlity'
    Sub addBT_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
        If (projName.Value <> "") Then
           
            If (projManager.SelectedValue = 0) Then
                lblProjManager.Text = "Please Select Project Manager"
                Return
            Else
                lblProjManager.Text = ""
            End If
            
            If (projCustName.SelectedIndex.ToString() = 0) Then
                lblcust.Text = "Please select customer"
                
            ElseIf Not (validateMultipleEmailsCommaSeparated()) Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AlertMessageBox", "alert('Invalid email address')", True)
            Else
                
                lblcust.Text = ""
                Dim Rdr As SqlDataReader
                Dim projid As Integer
                Dim moduleid As Integer
                Dim moduleid1 As Integer
                Dim moduleid2 As Integer
                Dim moduleid3 As Integer
                Dim moduleid4 As Integer
                Dim moduleid5 As Integer
             
            
                If IsPostBack Then
                    If conn.State = ConnectionState.Closed Then
                        conn.Open()
                    End If
                    
                    Dim projProcDate, projLastPaymentDate As String
                    projProcDate = "1-" & MonthName(projProcMonth.Value) & "-" & projProcYear.Value
                    projLastPaymentDate = "1-" & MonthName(projLastPaymentMonth.Value) & "-" & projLastPaymentYear.Value
                    If hiddenlineage.Value = "" Then
                        hiddenlineage.Value = "0"
                    End If
			
                    If projCode.Value = "" Then
                        sql = "INSERT INTO projectMaster(projName,custId," & _
                        "projManager,projStartDate,projDuration,currId,currExRate," & _
                        "projCost,projProcMonth,lastPaymentMonth,projDesc,codeRevteam,allowTSEmployee,OtherEmailId) VALUES('" & _
                        projName.Value & "','" & projCustName.Value & "','" & _
                        projManager.SelectedValue & "','" & projStartDate.Text & _
                        "','" & projDuration.Value & "','" & projCurrency.Value & "','" & _
                        projExchangeRate.Value & "','" & projCost.Value & "','" & _
                        projProcDate & "','" & projLastPaymentDate & "', '" & _
                        projDesc.Value & "','" & hiddenlineage.Value & "'," & boolean_to_int(chkEmpTimeSheet.Checked) & " ,'" & txtEmail.Text & "')"
								
                    Else
                        sql = "UPDATE projectMaster SET projName='" & projName.Value & _
                        "', custId='" & projCustName.Value & "',projManager='" & _
                        projManager.SelectedValue & "',projStartDate='" & projStartDate.Text & _
                        "',projDuration='" & projDuration.Value & "',currId='" & _
                        projCurrency.Value & "',currExRate='" & projExchangeRate.Value & _
                        "',projCost='" & projCost.Value & "',projProcMonth='" & projProcDate & _
                        "',lastPaymentMonth='" & projLastPaymentDate & "', projDesc='" & _
                        projDesc.Value & "', codeRevteam = '" & hiddenlineage.Value & "',allowTSEmployee=" & boolean_to_int(chkEmpTimeSheet.Checked) & " ,OtherEmailId='" & txtEmail.Text & "'  WHERE projId=" & projCode.Value
                     
								
                        If IsDate(projActCompDate.Value) Then
                            sql = sql & vbCrLf & "UPDATE projectMaster SET projActComp='" & _
                            projActCompDate.Value & "' WHERE projId=" & projCode.Value
                        Else
                            sql = sql & vbCrLf & "UPDATE projectMaster SET projActComp=" & _
                            "NULL WHERE projId=" & projCode.Value
                        End If
                    End If
           
                    Dim Cmd As New SqlCommand(sql, conn)
                    Cmd.ExecuteNonQuery()
                    Cmd.Dispose()
                    If projCode.Value = "" Then
                        prosql = "SELECT MAX(projId)as projid from projectMaster"
                        Dim Cmd1 As New SqlCommand(prosql, conn)
                        Rdr = Cmd1.ExecuteReader()
                        If Rdr.Read Then
                            projid = Rdr("projId")
                            Rdr.Close()
                            Cmd1.Dispose()
                            prosql = "SELECT MAX(moduleId) as moduleId from projectModuleMaster"
                            Cmd1 = New SqlCommand(prosql, conn)
                            Rdr = Cmd1.ExecuteReader()
                            If Rdr.Read Then
                                If Not IsDBNull(Rdr("moduleId")) Then
                                    moduleid = Rdr("moduleId")
                                Else
                                    moduleid = 0
                                End If
                        
                                Rdr.Close()
                                Cmd1.Dispose()
                                moduleid1 = moduleid + 1
                                moduleid2 = moduleid + 2
                                moduleid3 = moduleid + 3
                                moduleid4 = moduleid + 4
                                moduleid5 = moduleid + 5
                        
                                prosql = "INSERT INTO projectModuleMaster(projId,moduleName,moduleRefId,ProjectModuleTypeID)" & _
                                "VALUES(" & projid & ",'System Analysis'," & 1 & ",2)"
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "UPDATE projectModuleMaster SET modulerefid=" & moduleid1 & _
                                          " WHERE moduleid=" & moduleid1
                        
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "INSERT INTO projectModuleMaster(projId,moduleName,moduleRefId,ProjectModuleTypeID)" & _
                                "VALUES(" & projid & ",'Database Design'," & 2 & ",1)"
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "UPDATE projectModuleMaster SET modulerefid=" & moduleid2 & _
                                          " WHERE moduleid=" & moduleid2
                        
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "INSERT INTO projectModuleMaster(projId,moduleName,moduleRefId,ProjectModuleTypeID)" & _
                                "VALUES(" & projid & ",'Web Design'," & 3 & ",3)"
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "UPDATE projectModuleMaster SET modulerefid=" & moduleid3 & _
                                          " WHERE moduleid=" & moduleid3
                        
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "INSERT INTO projectModuleMaster(projId,moduleName,moduleRefId,ProjectModuleTypeID)" & _
                                "VALUES(" & projid & ",'Programming'," & 4 & ",1)"
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "UPDATE projectModuleMaster SET modulerefid=" & moduleid4 & _
                                         " WHERE moduleid=" & moduleid4
                                Cmd1 = New SqlCommand(prosql, conn)
                                Cmd1.ExecuteNonQuery()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "INSERT INTO projectModuleMaster(projId,moduleName,moduleRefId,ProjectModuleTypeID)" & _
                                "VALUES(" & projid & ",'Testing and QC'," & 5 & ",6)"
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "UPDATE projectModuleMaster SET modulerefid=" & moduleid5 & _
                                         " WHERE moduleid=" & moduleid5
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                        
                                prosql = "Insert into projectStatus values(" & projid & ",getdate(),0,'',1)"
                                Cmd1 = New SqlCommand(prosql, conn)
                                Rdr = Cmd1.ExecuteReader()
                                Cmd1.Dispose()
                                Rdr.Close()
                            End If
                        End If
				
                        If Request.QueryString("projid") = "" Then
                            Dim cmdmbr As SqlCommand
                            Dim strCRT As String
                            strCRT = "select max(projid) as projid from projectMaster"
                            cmdmbr = New SqlCommand(strCRT, conn)
                            Rdr = cmdmbr.ExecuteReader()
                            If Rdr.Read Then
                                projid = Rdr("projId")
                                Rdr.Close()
                                Dim str As String
                                Dim strsqlTeam As String
                                Dim arrCRTeam As Array
                                str = hiddenTeam.Value().ToString()
                                If Not String.IsNullOrEmpty(str) Then    ' add by satya 15 may 2012
                                    arrCRTeam = str.Split(",")
                                    If arrCRTeam.Length > -1 Then
                                        Dim CmdTeam As SqlCommand
                                        Dim j As Integer
                                        For j = 0 To arrCRTeam.Length - 1
                                            Try
                                                strsqlTeam = "INSERT INTO projectMember (projid,empid) VALUES('" & projid & "','" & arrCRTeam(j) & "')"
                                                CmdTeam = New SqlCommand(strsqlTeam, conn)
                                                CmdTeam.ExecuteNonQuery()
                                            Catch ex As Exception
                                            End Try
                                        Next
                                    End If
                                End If
                                conn.Close()
                            End If
                        End If
                    End If
			
                    If Request.QueryString("projid") <> "" Then
                        Dim memberid As Integer
                        memberid = Request.QueryString("projid")
                        Dim strdelmember As String
                        strdelmember = "DELETE FROM projectMember WHERE projid=" & memberid
				
                        Dim Cmd1 As New SqlCommand
                        Cmd1 = New SqlCommand(strdelmember, conn)
                        Cmd1.ExecuteNonQuery()
                        conn.Close()
                        Dim strteam As String
                        Dim strsqlteam As String
                        Dim strarrayteam As Array
                        strteam = hiddenTeam.Value().ToString()
                        If Not String.IsNullOrEmpty(strteam) Then   ' add by satya 15 may 2012
                            strarrayteam = strteam.Split(",")
                            Dim Cmdteam As SqlCommand
                            Dim j As Integer
                            For j = 0 To strarrayteam.Length - 1
                                strsqlteam = "INSERT INTO projectMember(projid,empid) VALUES('" & memberid & "','" & strarrayteam(j) & "')"
                                Cmdteam = New SqlCommand(strsqlteam, conn)
                                conn.Open()
                                Cmdteam.ExecuteNonQuery()
                                conn.Close()
                            Next
                        End If
                    End If
                End If
                Response.Redirect("default.aspx")
            End If
        End If
        
    End Sub

    Sub loadData()
        Dim Rdr As SqlDataReader
        If Request.QueryString("projId") <> "" Then
            sql = "select allowTSEmployee from projectMaster where projid =" & Request.QueryString("projId")
            Dim cmdchecked As New SqlCommand(sql, conn)
            Rdr = cmdchecked.ExecuteReader
            Dim a As Integer
            If Rdr.Read() Then
                a = Rdr("allowTSEmployee")
                If a = -1 Then
                    chkEmpTimeSheet.Checked = True
                Else
                    chkEmpTimeSheet.Checked = False
                End If
					
            End If
            Rdr.Close()
        End If

        sql = "select '' as CustCompany,'0' as custID union select  custName  + ' ('+ custCompany +') ' as custCompany ,custID from customerMaster order by custCompany"
        Dim Cmd1 As New SqlCommand(sql, conn)
        Rdr = Cmd1.ExecuteReader()
        projCustName.DataSource = Rdr
        projCustName.DataTextField = "custCompany"
        projCustName.DataValueField = "custId"
        projCustName.DataBind()
        Rdr.Close()
	
        sql = "SELECT * FROM employeeMaster WHERE empLeavingDate IS NULL ORDER BY empName"
        Cmd1.CommandText = sql
        Rdr = Cmd1.ExecuteReader()
        projManager.DataSource = Rdr
        projManager.DataTextField = "empName"
        projManager.DataValueField = "empId"
        projManager.DataBind()
        Rdr.Close()

        sql = "SELECT * FROM currencyMaster ORDER BY currId"
        Cmd1.CommandText = sql
        Rdr = Cmd1.ExecuteReader()
        projCurrency.DataSource = Rdr
        projCurrency.DataTextField = "currSymbol"
        projCurrency.DataValueField = "currId"
        projCurrency.DataBind()
        Rdr.Close()
	
        Dim i As Integer
        For i = 1 To 12
            projLastPaymentMonth.Items.Add(New ListItem(Left(MonthName(i), 3), i))
            projProcMonth.Items.Add(New ListItem(Left(MonthName(i), 3), i))
        Next

        projLastPaymentMonth.SelectedIndex = Month(Now()) - 1
        projProcMonth.SelectedIndex = Month(Now()) - 1

        i = Year(Now())
        Do While i > 1998
            projProcYear.Items.Add(i)
            i = i - 1
        Loop
    
        For i = 2003 To Year(Now()) + 6
            projLastPaymentYear.Items.Add(New ListItem(i, i))
        Next
        projLastPaymentYear.Value = Year(Now())
   	
        Dim projId As String = Request.QueryString("projId")
        If projId <> "" Then
            sql = "select * from projectMaster where projId=" & projId & " ORDER BY projName"
            Dim Cmd As New SqlCommand(sql, conn)
            Rdr = Cmd.ExecuteReader()
            If Rdr.Read() Then
                projName.Value = Rdr("projName")
                projCode.Value = Rdr("projId")
                projCustName.Value = Rdr("custId")
                projManager.SelectedValue = Rdr("projManager")
                projDuration.Value = Rdr("projDuration")
                ' projDuration.Items.FindByValue(Rdr("projDuration")).Selected = True
                projStartDate.Text = Format(Rdr("projStartDate"), "dd-MMM-yyyy")
                If IsDate(Rdr("projActComp")) Then
                    projActCompDate.Value = Format(Rdr("projActComp"), "dd-MMM-yyyy")
                End If
                projCost.Value = Rdr("projCost")
                projCurrency.Value = Rdr("currid")
                projExchangeRate.Value = Rdr("currExRate")
                projDesc.Value = Rdr("projDesc")
                projProcMonth.Value = Month(Rdr("projProcMonth"))
                projProcYear.Value = Format(Rdr("projProcMonth"), "yyyy")
                txtEmail.Text = Rdr("OtherEmailId").ToString()
            End If
            Rdr.Close()
        End If

        If Request.QueryString("projid") = "" Then
            Dim strCoder As String = "select empname,empid from employeemaster where empLeavingDate is null ORDER BY empName"
            Cmd1.CommandText = strCoder
            Rdr = Cmd1.ExecuteReader()
            lstCodeReview.DataSource = Rdr
            lstCodeReview.DataTextField = "empname"
            lstCodeReview.DataValueField = "empid"
            lstCodeReview.DataBind()
            Rdr.Close()
	
            sql = "select empname,empid from employeemaster where empLeavingDate is null  ORDER BY empName"
            Cmd1.CommandText = sql
            Rdr = Cmd1.ExecuteReader()
            lstteam.DataSource = Rdr
            lstteam.DataTextField = "empName"
            lstteam.DataValueField = "empId"
            lstteam.DataBind()
            Rdr.Close()
        Else
            sql = "select empid,empname from employeemaster where empid not in(select empid from projectMember where projid=" & Request.QueryString("projid") & ") and employeeMaster.empLeavingDate is null  ORDER BY empName"

            Cmd1.CommandText = sql
            Rdr = Cmd1.ExecuteReader()
            lstteam.DataSource = Rdr
            lstteam.DataTextField = "empName"
            lstteam.DataValueField = "empId"
            lstteam.DataBind()
            Rdr.Close()

            '--- to show data in list box
            If Request.QueryString("projid") <> "" Then
                Dim strsqlrun As String
                strsqlrun = "select coderevteam from projectMaster WHERE projid=" & Request.QueryString("projid")
		
                Cmd1.CommandText = strsqlrun
                Rdr = Cmd1.ExecuteReader()
                If (Rdr.Read) Then
                    hidshow.Text = Rdr("coderevteam").ToString()
                    If hidshow.Text <> "" Or IsDBNull(hidshow.Text) Then
                        sql = "select empid,empname from employeemaster where empid not in( " & hidshow.Text & _
                        " ) and employeeMaster.empLeavingDate is null  ORDER BY empName"
                        Rdr.Close()
	
                        Cmd1.CommandText = sql
                        Rdr = Cmd1.ExecuteReader()
                        lstCodeReview.DataSource = Rdr
                        lstCodeReview.DataTextField = "empName"
                        lstCodeReview.DataValueField = "empId"
                        lstCodeReview.DataBind()
	
                    Else
        
                        hidshow.Text = "0"
                        sql = "select empid,empname from employeemaster where empid not in( " & hidshow.Text & " ) and employeeMaster.empLeavingDate is null  ORDER BY empName"
                        Rdr.Close()
                        Cmd1.CommandText = sql
                        Rdr = Cmd1.ExecuteReader()
                        lstCodeReview.DataSource = Rdr
                        lstCodeReview.DataTextField = "empName"
                        lstCodeReview.DataValueField = "empId"
                        lstCodeReview.DataBind()
                        Rdr.Close()
                    End If
                End If
                Rdr.Close()
		
	
                sql = "select empid,empname from employeemaster where empid  in(" & hidshow.Text & " ) and employeeMaster.empLeavingDate is null  ORDER BY empName"
                Cmd1.CommandText = sql
                Rdr = Cmd1.ExecuteReader()
                lstAddCodeReview.DataSource = Rdr
                lstAddCodeReview.DataTextField = "empName"
                lstAddCodeReview.DataValueField = "empId"
                lstAddCodeReview.DataBind()
                Rdr.Close()


                sql = "select empid,empname from employeemaster where empid  in(select empid from projectMember where projid=" & Request.QueryString("projid") & ") and employeeMaster.empLeavingDate is null  ORDER BY empName"
                Cmd1.CommandText = sql
                Rdr = Cmd1.ExecuteReader()
                lstAddTeam.DataSource = Rdr
                lstAddTeam.DataTextField = "empName"
                lstAddTeam.DataValueField = "empId"
                lstAddTeam.DataBind()

            End If
        End If
    End Sub
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dyno Admin Control</title>
    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />
    <link rel="stylesheet" href="../Includes/calendar.css" type="text/css" />
    <script language="JavaScript" src="/includes/CalendarControl.js" type="text/javascript">
    </script>
</head>
<body>
    <ucl:adminMenu ID="adminMenu" runat="server" />
    <form runat="server" id="projForm">
        <table cellpadding="4" width="100%" border="1" style="border-collapse: collapse; border-color: #E8E8E8">
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Project Title</b></font>
                </td>
                <td width="33%">
                    <table>
                        <tr>
                            <td>
                                <input type="hidden" id="projCode" runat="server" width="0" size="20" />
                                <input type="text" id="projName" runat="server" size="20" onblur="callAjax()" />
                                <div id="errorOnEmptyPrjTitle" style="display: none; font-size: small;">
                                    <font color="red">Please enter Project Title</font>
                                </div>
                            </td>
                            <td>
                                <div id="errorProjName" style="display: none; font-size: small;">
                                    <font color="red">Poject Title already Exist</font>
                                </div>
                            </td>
                        </tr>
                    </table>
                    &nbsp;
                </td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Actual Completion</b></font>
                </td>
                <td width="33%">
                    <input type="text" id="projActCompDate" runat="server" size="15" name="projActCompDate"
                        onclick="popupCalender('projActCompDate')" />
                </td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Customer</b></font>
                </td>
                <td width="33%">
                    <select size="1" id="projCustName" runat="server" name="projCustName">
                    </select>
                    <br />
                    <asp:Label ID="lblcust" runat="server" Text="" ForeColor="Red" Font-Size="Small"></asp:Label>
                </td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Project Cost</b></font>
                </td>
                <td width="33%">
                    <input type="text" id="projCost" runat="server" size="10" name="projCost" />
                </td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Project Manager</b></font>
                </td>
                <td width="33%">
                    <%--<select size="1" id="projManager" runat="server" name="projManager">
                    <option value="0">Select</option>
                </select>--%>
                    <asp:DropDownList ID="projManager" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Label ID="lblProjManager" runat="server" Text="" ForeColor="Red" Font-Size="Small"></asp:Label>
                </td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Payment Currency</b></font>
                </td>
                <td width="33%">
                    <select size="1" id="projCurrency" runat="server" name="projCurrency">
                    </select>
                </td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Project Duration</b></font>
                </td>
                <td width="33%">
                    <select size="1" id="projDuration" runat="server" name="projDuration">
                        <option selected value="0.25">1 week</option>
                        <option value="0.5">2 week</option>
                        <option value="0.75">3 week</option>
                        <option value="1.25">4 week</option>
                        <option value="1.5">5 week</option>
                        <option value="1.75">6 week</option>
                        <option value="1">1 Month</option>
                        <option value="2">2 Month</option>
                        <option value="3">3 Month</option>
                        <option value="4">4 Month</option>
                        <option value="5">5 Month</option>
                        <option value="6">6 Month</option>
                        <option value="7">7 Month</option>
                        <option value="8">8 Month</option>
                        <option value="9">9 Month</option>
                        <option value="10">10 Month</option>
                        <option value="11">11 Month</option>
                        <option value="12">12 Month</option>
                        <option value="13">13 Month</option>
                        <option value="14">14 Month</option>
                        <option value="15">15 Month</option>
                        <option value="16">16 Month</option>
                        <option value="17">17 Month</option>
                        <option value="18">18 Month</option>
                    </select>
                </td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Exchange Rate (In Rupee)</b></font>
                </td>
                <td width="33%">
                    <input type="text" id="projExchangeRate" runat="server" size="5" name="projExchangeRate" />
                </td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Start Date</b></font>
                </td>
                <td width="33%">
                    <asp:TextBox ID="projStartDate" runat="server" size="14" onclick="popupCalender('projStartDate')"
                        onkeypress="return false;"></asp:TextBox>
                    <div id="errorOnEmptyStartDate" style="display: none; font-size: small;">
                        <font color="red">Please enter Start Date</font>
                    </div>
                </td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Last Payment Month</b></font>
                </td>
                <td width="33%">
                    <select size="1" id="projLastPaymentMonth" runat="server" name="projLastPaymentMonth">
                    </select>
                    <select size="1" id="projLastPaymentYear" runat="server" name="projLastPaymentYear">
                    </select>
                </td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Procurement Month</b></font>
                </td>
                <td width="33%">
                    <select size="1" id="projProcMonth" runat="server" name="projProcMonth">
                    </select>
                    <select size="1" id="projProcYear" runat="server" name="projProcYear">
                    </select>
                </td>
                <td bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Development Team</b></font>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstteam" runat="server" SelectionMode="Multiple" onDblClick="moveteam(lstteam,lstAddTeam);"></asp:ListBox>
                            </td>
                            <td>
                                <input type="button" id="btnTadd" runat="server" value="Add" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                                    font-bold="true" onclick="moveteam(lstteam, lstAddTeam);" />
                                <br>
                                <input type="button" id="btnTremove" runat="server" value="Remove" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                                    font-bold="true" onclick="removeTeam(lstAddTeam, lstteam);" />
                            </td>
                            <td>
                                <asp:ListBox ID="lstAddTeam" size="4" runat="server" SelectionMode="Multiple" onDblClick="removeTeam(lstAddTeam,lstteam);"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Project Description</b></font>
                </td>
                <td width="33%">
                    <textarea id="projDesc" runat="server" rows="5" cols="50" name="projDesc" />
                </td>
                <td bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Code Review Team</b></font>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstCodeReview" runat="server" SelectionMode="Multiple" onDblClick="moveItem(lstCodeReview,lstAddCodeReview);"></asp:ListBox>
                            </td>
                            <td>
                                <input type="button" id="btnadd" runat="server" value="Add" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                                    font-bold="true" onclick="moveItem(lstCodeReview, lstAddCodeReview);" />
                                <br>
                                <input type="button" id="btnremove" runat="server" value="Remove" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                                    font-bold="true" onclick="removeitem(lstAddCodeReview, lstCodeReview);" />
                            </td>
                            <td>
                                <asp:ListBox ID="lstAddCodeReview" runat="server" SelectionMode="Multiple" onDblClick="moveItem(lstAddCodeReview,lstCodeReview);"></asp:ListBox>
                            </td>
                        </tr>
                        <input id="hiddenTeam" type="hidden" runat="server" />
                        <input id="hiddenlineage" type="hidden" runat="server" />
                        <div style="display: none">
                            <asp:Label ID="hidshow" runat="server"></asp:Label>
                        </div>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Employee TimeSheet</b></font>
                </td>
                <td>
                    <asp:CheckBox ID="chkEmpTimeSheet" runat="server" />
                </td>

                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Other EmailId</b></font>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="308px" Height="86px" TextMode="MultiLine"
                        onkeypress="javascript:return checkKey()"></asp:TextBox><br />
                    <span id="msg" style="font: Verdana; font-size: 9pt; color: #A2921E">Please input a comma separated list of email addresses.</span>
                    <%--<asp:RegularExpressionValidator ID="emailAddressesListValidator" Display="Dynamic" Font-Names="Verdhana" Font-Size="9pt"

                    ControlToValidate="txtEmail" runat="server" ForeColor="Red" ErrorMessage="Invalid List. Please input a comma separated list of email addresses."
                    ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*$"> 
                </asp:RegularExpressionValidator>--%>
                </td>
            </tr>
            <tr align="center">
                <td colspan="4" bgcolor="#edf2e6">
                    <asp:Button ID="addBT" runat="server" Text="Save" BackColor=" #C5D5AE" Font-Size="8pt"
                        ForeColor=" #A2921E" Font-Bold="true" OnClick="addBT_OnClick" OnClientClick="javascript:return fillTextBox();" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<script type="text/javascript">
    function ajaxFunction() {
        var xmlHttp;
        try {
            xmlHttp = new XMLHttpRequest();
        }
        catch (e) {    // Internet Explorer
            try {
                xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e) {
                    alert("Your browser does not support AJAX!");
                    return false;
                }
            }
        }
        return xmlHttp;
    }

    function callAjax() {
        var xmlHttp = ajaxFunction();
        var pName = document.getElementById("projName").value;
        var pprojCode = document.getElementById("projCode").value;
        if (pName != "") {
            xmlHttp.open("GET", "ajaxGet.aspx?no=" + Math.random() + "&pojectName=" + pName + "&pprojCode=" + pprojCode, true);

            xmlHttp.onreadystatechange = function () {
                if (xmlHttp.readyState == 4) {
                    var str = xmlHttp.responseText;
                    if (str != "") {

                        if (str == "False") {
                            document.getElementById("errorProjName").style.display = "none";
                        }
                        else {

                            document.getElementById("errorProjName").style.display = "";
                        }
                    }
                }
            }
            xmlHttp.send(null);
        }
        else {
            document.getElementById("errorProjName").style.display = "none"
        }
    }
    var z = dhtmlXComboFromSelect("projCustName");
    z.enableFilteringMode(true);
</script>
