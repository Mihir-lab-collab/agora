Imports System.Collections.Generic
Imports System.Web
Imports System.Data.SqlClient
Imports System.Data
Imports CommonFunctionLib
Imports System.Web.UI.WebControls
Imports System.Globalization
Imports System
Imports System.Configuration

''' <summary>
''' Summary description for clsCommon
''' </summary>
''' 

Public Class clsCommon
    Public objDBFunction As New DBFunc()
    Public onjGeneral As New generalFunction()

    ' TODO: Add constructor logic here
    '
    Public Sub New()
    End Sub



    'updated by satish on 19/07/2013
    'updated by satish on 19/07/2013
    Public Function Employee(ByVal strLocation As String, ByVal strName As String, ByVal strAddress As String, ByVal strContact As String, ByVal intSkill As Integer, ByVal strNotes As String, ByVal strJoiningDate As String, ByVal strProbationPeriod As String, ByVal strEmail As String, ByVal strAccountno As String, ByVal strBDate As String, ByVal strADate As String, ByVal strPrevEmployer As String, ByVal intexperince As Integer, ByVal strInsertedOn As String, ByVal intInsertedBy As Integer, ByVal strInsertedIP As String, ByRef intEmpID As Integer, ByVal strType As String, ByVal strLeavingDate As String, ByVal blnTester As Boolean, ByVal strResume As String, ByVal strPhoto As Byte(), ByVal primarySkillId As String) As DataTable
        'Try
        If strType = "Insert" Then
            'strInsertedOn = ChangeDatePattern(strInsertedOn, "yyyy-MM-dd hh:mm:ss")
            'strJoiningDate = ChangeDatePattern(strJoiningDate, "yyyy-MM-dd hh:mm:ss")
            strBDate = ChangeDatePattern(strBDate, "yyyy-MM-dd hh:mm:ss")
            strADate = ChangeDatePattern(strADate, "yyyy-MM-dd hh:mm:ss")

            'strJoiningDate = Convert.ToDateTime(strJoiningDate).ToString("dd/MM/yyyy")



            If (strJoiningDate <> "Null") Then
                strJoiningDate = "'" + strJoiningDate + "'"
            End If

            If (strBDate <> "Null") Then
                strBDate = "'" + strBDate + "'"
            End If

            If (strADate <> "Null") Then
                strADate = "'" + strADate + "'"
            End If

            Dim strConnString As String = ConfigurationManager.ConnectionStrings("conString").ConnectionString
            Dim dtEmployee As DataTable = New DataTable()
            Using connection As New SqlConnection(strConnString)
                connection.Open()

                Dim command As SqlCommand = connection.CreateCommand()
                Dim transaction As SqlTransaction

                ' Start a local transaction
                transaction = connection.BeginTransaction("A")

                ' Must assign both transaction object and connection 
                ' to Command object for a pending local transaction.
                command.Connection = connection
                command.Transaction = transaction

                Try
                    command.CommandText = "DECLARE @itemID int; INSERT INTO USERMASTER(UserType,SecurityLevel,IsAdmin,InsertedOn,InsertedBy,InsertedIP) values('e',5,0,convert(date,'" + strInsertedOn + "',103)," + intInsertedBy.ToString() + ",'" + strInsertedIP + "') SET @itemID = (select @@identity) " & _
                            "INSERT INTO employeeMaster(empid,empName,empAddress,empContact,skillId,empNotes,empJoiningDate,empLeavingDate,empProbationPeriod,empEmail,empAccountNo,empBDate,empADate,empPrevEmployer,empExperince,IsTester,InsertedOn,InsertedBy,InsertedIP, LocationFKID,Resume,PrimarySkillId,photo) VALUES(@itemID,'" + strName + "','" + strAddress + "','" + strContact + "'," + intSkill.ToString() + ",'" + strNotes + "',convert(date," + strJoiningDate + ",103)," + strLeavingDate + "," + strProbationPeriod + ",'" + strEmail + "','" + strAccountno + "'," + strBDate + "," + strADate + ",'" + strPrevEmployer + "'," + intexperince.ToString() + ",'" + blnTester.ToString() + "',convert(date,'" + strInsertedOn + "',103)," + intInsertedBy.ToString() + ",'" + strInsertedIP + "','" + strLocation + "', '" + strResume + "', '" + primarySkillId + "',  @Image )  Select @itemID "
                    command.Parameters.Add(New SqlParameter("@Image", SqlDbType.Image))
                    If strPhoto Is Nothing Then
                        command.Parameters("@Image").Value = DBNull.Value
                    Else
                        command.Parameters("@Image").Value = strPhoto
                    End If
                    dtEmployee.Load(command.ExecuteReader())


                    intEmpID = Convert.ToInt16(dtEmployee.Rows(0)(0).ToString())
                    ' Attempt to commit the transaction.
                    transaction.Commit()
                    connection.Close()
                    'intEmpID = Convert.ToInt16(dtEmployee.Rows(0)(0).ToString())
                Catch ex As Exception
                    transaction.Rollback()
                End Try

            End Using
            Return dtEmployee
        ElseIf strType = "Update" Then
            Dim sql As String
            Dim strConnString As String = ConfigurationManager.ConnectionStrings("conString").ConnectionString
            Using connection As New SqlConnection(strConnString)
                connection.Open()

                Dim intActive As Integer = 1
                If (strLeavingDate <> "Null") Then
                    intActive = 0
                End If

                sql = "SET DATEFORMAT DMY; UPDATE employeeMaster SET  skillid =" + intSkill.ToString() + ", empName = '" + strName + "' , empAddress = '" + strAddress + "' , empContact = '" + strContact + "' , empJoiningDate = " + strJoiningDate + ", empLeavingDate =" + strLeavingDate + ", empProbationPeriod = '" + strProbationPeriod + "' , empNotes = '" + strNotes + "' , empEmail = '" + strEmail + "' , empAccountNo = '" + strAccountno + "' , empBDate = " + strBDate + ", empADate =" + strADate + " , empPrevEmployer = '" + strPrevEmployer + "' , empExperince = " + intexperince.ToString() + " , ModifiedOn = '" + strInsertedOn + "' , ModifiedBy = " + intInsertedBy.ToString() + " , ModifiedIP = '" + strInsertedIP + "', IsTester ='" + blnTester.ToString() + "' , LocationFKID ='" + strLocation + "',Resume ='" + strResume.ToString() + "' ,IsActive=" + intActive.ToString() + " ,primaryskillid=" + primarySkillId + " WHERE empid = " + intEmpID.ToString()
                Dim command As SqlCommand = New SqlCommand()
                command.Connection = connection

                If strPhoto Is Nothing Then

                Else
                    sql = sql + " UPDATE employeeMaster SET photo = @Image WHERE empid = " + intEmpID.ToString()
                    command.Parameters.Add(New SqlParameter("@Image", SqlDbType.Image))
                    command.Parameters("@Image").Value = strPhoto
                End If
                command.CommandText = sql
                'command.CommandType = CommandType.Text
                '//sql = "SET DATEFORMAT DMY; UPDATE employeeMaster SET skillid =" + intSkill.ToString() + ", empName = '" + strName + "' , empAddress = '" + strAddress + "' , empContact = '" + strContact + "' , empJoiningDate = " + strJoiningDate + ", empLeavingDate =" + strLeavingDate + ", empProbationPeriod = '" + strProbationPeriod + "' , empNotes = '" + strNotes + "' , empEmail = '" + strEmail + "' , empAccountNo = '" + strAccountno + "' , empBDate = " + strBDate + ", empADate =" + strADate + " , empPrevEmployer = '" + strPrevEmployer + "' , empExperince = " + intexperince.ToString() + " , ModifiedOn = '" + strInsertedOn + "' , ModifiedBy = " + intInsertedBy.ToString() + " , ModifiedIP = '" + strInsertedIP + "', IsTester ='" + blnTester.ToString() + "' , LocationFKID ='" + strLocation + "',IsActive ='" + intActive.ToString() + "',Resume=" + strResume.ToString() + " WHERE empid = " + intEmpID.ToString()
                'sql = "SET DATEFORMAT DMY; UPDATE employeeMaster SET  empJoiningDate = '" + strJoiningDate + "'  WHERE empid = " + intEmpID.ToString()
                'objDBFunction.ExecuteSQLRtnDT(sql)
                command.ExecuteReader()
                connection.Close()
                Return New DataTable()
            End Using
        End If
        'Catch ex As Exception
        '    Return New DataTable()
        'End Try
    End Function





    'Written By Rajeev - 07/05/2013
    Public Function EmployeeLocationList() As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select LocationID,Name from Location  order by Name")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    'Written By Rajeev - 07/05/2013
    Public Function SearchByLocation(strvalue As String, strLeavingDate As String, strLocation As String) As DataTable
        Try
            '    If strLocation = "0" Then
            '        If strLeavingDate = "ALL" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empJoiningDate like '%" + strvalue + "%' ) order by empid desc")
            '        ElseIf strLeavingDate = "Left" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%'  ) and empLeavingDate is not NULL order by empid desc")
            '        ElseIf strLeavingDate = "Current" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' ) and empLeavingDate is NULL order by empid desc")
            '        End If
            '    Else
            '        If strLeavingDate = "ALL" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empJoiningDate like '%" + strvalue + "%'  ) and LocationFKID = '" + strLocation + "' order by empid desc")
            '        ElseIf strLeavingDate = "Left" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' ) and LocationFKID = '" + strLocation + "' and empLeavingDate is not NULL order by empid desc")
            '        ElseIf strLeavingDate = "Current" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' ) and LocationFKID = '" + strLocation + "' and empLeavingDate is NULL order by empid desc")
            '        End If
            '    End If

            Return objDBFunction.ExecuteSQLRtnDT("EXEC sp_SearchByLocation  '" + strLocation + "','" + strLeavingDate + "','" + strvalue + "'")

        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    'Written By Rajeev - 07/05/2013
    Public Function SearchByNameEmployeeID(strvalue As String, strLeavingDate As String, strLocation As String) As DataTable
        Try
            '    If strLocation = "0" Then
            '        If strLeavingDate = "ALL" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where ( skillMaster.skillDesc like '%" + strvalue + "%' or  empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empJoiningDate like '%" + strvalue + "%' or empid like '%" + strvalue + "%')  order by empid desc")
            '        ElseIf strLeavingDate = "Left" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel,skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (skillMaster.skillDesc like '%" + strvalue + "%' or empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empid like '%" + strvalue + "%')  and empLeavingDate is not NULL order by empid desc")
            '        ElseIf strLeavingDate = "Current" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (skillMaster.skillDesc like '%" + strvalue + "%' or empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empid like '%" + strvalue + "%')  and empLeavingDate is NULL order by empid desc")
            '        End If
            '    Else
            '        If strLeavingDate = "ALL" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (skillMaster.skillDesc like '%" + strvalue + "%' or empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empJoiningDate like '%" + strvalue + "%' or empid like '%" + strvalue + "%') and LocationFKID = '" + strLocation + "'  order by empid desc")
            '        ElseIf strLeavingDate = "Left" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (skillMaster.skillDesc like '%" + strvalue + "%' or empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empid like '%" + strvalue + "%') and LocationFKID = '" + strLocation + "'  and empLeavingDate is not NULL order by empid desc")
            '        ElseIf strLeavingDate = "Current" Then
            '            Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (skillMaster.skillDesc like '%" + strvalue + "%' or empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empid like '%" + strvalue + "%') and LocationFKID = '" + strLocation + "' and empLeavingDate is NULL order by empid desc")
            '        End If

            '    End If
            Return objDBFunction.ExecuteSQLRtnDT("EXEC sp_SearchByNameEmployeeID  '" + strLocation + "','" + strLeavingDate + "','" + strvalue + "'")

        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function






    Public Function ChangeDatePattern(strDate As String, strPattern As String) As String
        Try
            Dim dt As DateTime = DateTime.Parse(strDate.Replace("'", ""))
            Dim reformatted As String = dt.ToString(strPattern, CultureInfo.InvariantCulture)
            Return reformatted
        Catch ex As Exception
            Return strDate
        End Try
    End Function
    ''Written By Rajeev - 09/05/2013
    'Public Function Location(intLocationID As Integer, strName As String, strModifiedOn As String, intModifiedBy As Integer, strModifiedIP As String, strType As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(13) {}
    '        param(0) = intLocationID
    '        param(1) = strName
    '        param(2) = strModifiedOn
    '        param(3) = intModifiedBy
    '        param(4) = strModifiedIP
    '        param(5) = strType
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_Location", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function
    'Written By Rajeev - 10/05/2013
    Public Function HoildayList(LocationID As String) As DataTable
        Try
            Dim strSQL As String
            If Month(Now()) > 10 Then
                strSQL = "SELECT * FROM holidayMaster WHERE holidayDate BETWEEN '1-Jan-" & Year(Now()).ToString() & _
               "' AND '31-Mar-" & (Year(Now()) + 1).ToString() & "' And LocationID='" & LocationID & "' ORDER BY holidayDate"
            Else
                strSQL = "SELECT * FROM holidayMaster WHERE holidayDate BETWEEN '1-Jan-" & Year(Now()).ToString() & _
               "' AND '31-Dec-" & Year(Now()).ToString() & "' And LocationID='" & LocationID & "' ORDER BY holidayDate"
            End If

            Return objDBFunction.ExecuteSQLRtnDT(strSQL)
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    'Written By Rajeev - 10/05/2013
    Public Function EmployeeSkillList(SecurityLevel As Integer) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select * from skillMaster where SecurityLevel <= '" & SecurityLevel & "' order by SkillDesc")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    'Written By Rajeev - 14/05/2013
    Public Function SecurityLevel(intEmpID As Integer) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("select  UM.Usermasterid,  SecurityLevel = Case When UM.SecurityLevel >  SM.SecurityLevel then UM.SecurityLevel else SM.SecurityLevel End  from  UserMaster UM inner join  employeemaster EM  on EM.Empid = UM.Usermasterid inner join SkillMaster SM on  SM.SkillId = EM.skillid and EM.Empid =" + Convert.ToString(intEmpID))
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function



    'Public Function Attribute(intCategoryID As Integer, intAttributeID As Integer, strName As String, strDefaultName As String, strType As String, strInsertDate As String, intInsertedBy As Integer, _
    '    strInsertedIP As String, intSortOrder As Integer, strMore As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(10) {}
    '        param(0) = intCategoryID
    '        param(1) = intAttributeID
    '        param(2) = strName
    '        param(3) = strDefaultName
    '        param(4) = strType
    '        param(5) = strInsertDate
    '        param(6) = intInsertedBy
    '        param(7) = strInsertedIP
    '        param(8) = intSortOrder
    '        param(9) = strMore
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_Attribute", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    'Public Function Item(ItemID As Integer, BrandID As Integer, intCategoryOrItemId As Integer, intItemInvoiceID As Integer, intSupplierId As Integer, strDescription As String, fltPrice As Single, _
    ' strExpiryDate As String, intQuantity As Integer, strSerialNo As String, strInsertedOn As String, intInsertedBy As Integer, strInsertedIP As String, _
    ' intSortOrder As Integer, strMore As String) As DataTable
    '    'Try


    '    Dim param As Object() = New Object(14) {}
    '    param(0) = ItemID
    '    param(1) = BrandID
    '    param(2) = intCategoryOrItemId
    '    param(3) = intItemInvoiceID
    '    param(4) = strDescription
    '    param(5) = fltPrice
    '    param(6) = strExpiryDate
    '    param(7) = intQuantity
    '    param(8) = strSerialNo
    '    param(9) = strInsertedOn
    '    param(10) = intInsertedBy
    '    param(11) = strInsertedIP
    '    param(12) = intSortOrder
    '    param(13) = strMore
    '    Return objDBFunction.ExecuteProcedureRtnDT("SP_Item", param)
    '    'Catch ex As Exception
    '    'Return New DataTable()
    '    'End Try
    'End Function


    'Public Function ItemAttribute(intItemID As Integer, intAttributeID As Integer, strValue As String, strInsertedOn As String, intInsertedBy As Integer, strInsertedIP As String, _
    '    intSortOrder As Integer, strMore As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(8) {}
    '        param(0) = intItemID
    '        param(1) = intAttributeID
    '        param(2) = strValue
    '        param(3) = strInsertedOn
    '        param(4) = intInsertedBy
    '        param(5) = strInsertedIP
    '        param(6) = intSortOrder
    '        param(7) = strMore
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_ItemAttribute", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    'Public Function Brand(intBrandId As Integer, strName As String, strDescription As String, strModifiedOn As String, intModifiedBy As Integer, strModifiedIP As String, _
    '    intSortOrder As Integer, strType As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(8) {}
    '        param(0) = intBrandId
    '        param(1) = strName
    '        param(2) = strDescription
    '        param(3) = strModifiedOn
    '        param(4) = intModifiedBy
    '        param(5) = strModifiedIP
    '        param(6) = intSortOrder
    '        param(7) = strType
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_Brand", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    'Public Function Supplier(intSuppleirID As Integer, strName As String, strAddress As String, strCity As String, strState As String, strCountry As String, _
    '   strEmail As String, strMobile As String, strModifiedOn As String, intModifiedBy As Integer, strModifiedIP As String, intSortOrder As Integer, strType As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(13) {}
    '        param(0) = intSuppleirID
    '        param(1) = strName
    '        param(2) = strAddress
    '        param(3) = strCity
    '        param(4) = strState
    '        param(5) = strCountry
    '        param(6) = strEmail
    '        param(7) = strMobile
    '        param(8) = strModifiedOn
    '        param(9) = intModifiedBy
    '        param(10) = strModifiedIP
    '        param(11) = intSortOrder
    '        param(12) = strType
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_Supplier", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    'Public Function Category(intCategoryID As Integer, strName As String, strModifiedOn As String, intModifiedBy As Integer, strModifiedIP As String, intSortOrder As Integer, strType As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(6) {}
    '        param(0) = intCategoryID
    '        param(1) = strName
    '        param(2) = strModifiedOn
    '        param(3) = intModifiedBy
    '        param(4) = strModifiedIP
    '        param(5) = intSortOrder
    '        param(6) = strType
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_Category", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    'Public Function MiscAgora(strMore As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(0) {}
    '        param(0) = strMore
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_MiscAgora", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    Public Function VerifyUser(intEmpID As Integer) As DataTable
        Try
            'changed by alok on 23 August
            'Return objDBFunction.ExecuteSQLRtnDT("Select UserMaster.UserType, UserMaster.SecurityLevel, UserMaster.IsAdmin, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid and UserMaster.UserMasterID =" + Convert.ToString(intEmpID))
            'Return objDBFunction.ExecuteSQLRtnDT("Select UserMaster.UserType, UserMaster.SecurityLevel, profile.IsAdmin, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid inner join profile on profile.ProfileId=employeeMaster.ProfileID and UserMaster.UserMasterID =" + Convert.ToString(intEmpID))
            Return objDBFunction.ExecuteSQLRtnDT("Declare @ProfileId varchar(50) select @ProfileId=ProfileId from profile where profile.ProfileId=(select top(1)ProfileID from employeeMaster where empid =" + Convert.ToString(intEmpID) + ") if(@ProfileId!='') begin Select UserMaster.UserType, UserMaster.SecurityLevel, profile.*, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid inner join profile on profile.ProfileId=employeeMaster.ProfileID and UserMaster.UserMasterID =" + Convert.ToString(intEmpID) + " end else begin Select UserMaster.UserType, UserMaster.SecurityLevel, 0 as IsAdmin, employeeMaster.* from UserMaster inner join employeeMaster on UserMaster.UserMasterID = employeeMaster.empid and UserMaster.UserMasterID =" + Convert.ToString(intEmpID) + " end ")

        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Sub UpdateUserAndEmployeeMaster(intEmpID As Integer, strInsertedIP As String)
        Try
            objDBFunction.ExecuteSQL("Update UserMaster Set ModifiedOn ='" + DateTime.Now.ToString() + "', ModifiedBy =" + intEmpID.ToString() + ", ModifiedIP ='" + strInsertedIP + "' where UserMasterID =" + intEmpID.ToString())
            objDBFunction.ExecuteSQL("Update employeeMaster Set ModifiedOn ='" + DateTime.Now.ToString() + "', ModifiedBy =" + intEmpID.ToString() + ", ModifiedIP ='" + strInsertedIP + "' where empid =" + intEmpID.ToString())
        Catch ex As Exception
        End Try
    End Sub

    Public Function GetMainMenuBarItem(intEmpID As Integer) As DataTable
        Try
            Dim param As Object() = New Object(2) {}
            param(0) = intEmpID
            Return objDBFunction.ExecuteProcedureRtnDT("SP_GetModuleByProfile", param)
            'Return objDBFunction.ExecuteSQLRtnDT("Select * from Module where (ModuleID in (Select ModuleID from dbo.ProfileModule where ProfileID in (Select B.ProfileID from dbo.UserMaster join dbo.EmployeeMaster as A on UserMasterID = A.EmpID Left join dbo.Profile as B on B.ProfileID = A.ProfileID where UserMasterID = " + Convert.ToString(intEmpID) + ")) or IsGenric = 1) and IsMenuVisible = 1 and [Type] = 'e'  and ModuleId_Parent is NUll order by SortOrder desc")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function GetSubMenuItem(intEmpID As Integer, intParentID As Integer) As DataTable
        Try
            Dim param As Object() = New Object(2) {}
            param(0) = intEmpID
            param(1) = intParentID
            Return objDBFunction.ExecuteProcedureRtnDT("SP_GetModuleByProfile", param)
            'Return objDBFunction.ExecuteSQLRtnDT("Select * from Module where (ModuleID in (Select ModuleID from dbo.ProfileModule where ProfileID in (Select B.ProfileID from dbo.UserMaster join dbo.EmployeeMaster as A on UserMasterID = A.EmpID Left join dbo.Profile as B on B.ProfileID = A.ProfileID where UserMasterID = " + Convert.ToString(intEmpID) + ")) or IsGenric = 1) and IsMenuVisible = 1 and [Type] = 'e'  and ModuleId_Parent = " + Convert.ToString(intParentID) + " order by SortOrder desc")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function CheckPermissionForUpdates(intSecurityLevel As Integer, intModuleID As Integer) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select * from Module where SecurityLevelUpdate =" + Convert.ToString(intSecurityLevel) + "and ModuleID =" + Convert.ToString(intModuleID))
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function CheckPermissionForAdd(intSecurityLevel As Integer, intModuleID As Integer) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select * from Module where SecurityLevelAdd =" + Convert.ToString(intSecurityLevel) + "and ModuleID =" + Convert.ToString(intModuleID))
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    'Public Function EmployeeDetails(strLeavingDate As String, intEmpId As Integer) As DataTable
    '    Try
    '        If strLeavingDate = "ALL" Then
    '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid order by empid desc")
    '        ElseIf strLeavingDate = "Left" Then
    '            Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empLeavingDate is not NULL order by empid desc ")
    '        ElseIf strLeavingDate = "Current" Then
    '            Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empLeavingDate is NULL order by empid desc")
    '        ElseIf strLeavingDate = "Select" Then
    '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empid=" + intEmpId.ToString() + "order by empid desc")
    '        End If
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try

    'End Function

    'Public Function SearchByName(strvalue As String, strLeavingDate As String) As DataTable
    '    Try
    '        If strLeavingDate = "ALL" Then
    '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel,  skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empJoiningDate like '%" + strvalue + "%') order by empid desc")
    '        ElseIf strLeavingDate = "Left" Then
    '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel,  skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%') and empLeavingDate is not NULL order by empid desc")
    '        ElseIf strLeavingDate = "Current" Then
    '            Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel,  skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%') and empLeavingDate is NULL order by empid desc")
    '        End If

    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    'Public Function Employee(strName As String, strAddress As String, strContact As String, intSkill As Integer, strNotes As String, strJoiningDate As String, strProbationPeriod As String, strEmail As String, strAccountno As String, strBDate As String, strADate As String, strPrevEmployer As String, intexperince As Integer, strInsertedOn As String, intInsertedBy As Integer, strInsertedIP As String, ByRef intEmpID As Integer, strType As String, strLeavingDate As String, blnTester As Boolean) As DataTable
    '    Try
    '        If strType = "Insert" Then
    '            Dim dtEmployee As DataTable = New DataTable()
    '            Dim mastersql As String
    '            mastersql = "DECLARE @itemID int; Insert into UserMaster(UserType,SecurityLevel,IsAdmin,InsertedOn,InsertedBy,InsertedIP) values('e',5,0,'" + strInsertedOn + "'," + intInsertedBy.ToString() + ",'" + strInsertedIP + "') SET @itemID = (select @@identity) SELECT @itemID AS UserMasterID"
    '            Dim dt As System.Data.DataTable
    '            dt = objDBFunction.ExecuteSQLRtnDT(mastersql)
    '            intEmpID = Convert.ToInt16(dt.Rows(0)(0).ToString())
    '            Try
    '                Dim sql As String
    '                sql = "INSERT INTO employeeMaster(empid,empName,empAddress,empContact,skillId,empNotes,empJoiningDate,empLeavingDate,empProbationPeriod,empEmail,empAccountNo,empBDate,empADate,empPrevEmployer,empExperince,IsTester,InsertedOn,InsertedBy,InsertedIP) VALUES(" + dt.Rows(0)(0).ToString() + ",'" + strName + "','" + strAddress + "','" + strContact + "'," + intSkill.ToString() + ",'" + strNotes + "','" + strJoiningDate + "'," + strLeavingDate + "," + strProbationPeriod + ",'" + strEmail + "','" + strAccountno + "'," + strBDate + "," + strADate + ",'" + strPrevEmployer + "'," + intexperince.ToString() + ",'" + blnTester.ToString() + "','" + strInsertedOn + "'," + intInsertedBy.ToString() + ",'" + strInsertedIP + "')"
    '                dtEmployee = objDBFunction.ExecuteSQLRtnDT(sql)
    '            Catch ex As Exception
    '                objDBFunction.ExecuteSQLRtnDT("Delete from UserMaster where UserMasterID =" + intEmpID.ToString())
    '            End Try
    '            Return dtEmployee
    '        ElseIf strType = "Update" Then
    '            Dim sql As String
    '            sql = "UPDATE employeeMaster SET skillid =" + intSkill.ToString() + ", empName = '" + strName + "' , empAddress = '" + strAddress + "' , empContact = '" + strContact + "' , empJoiningDate = '" + strJoiningDate + "', empLeavingDate =" + strLeavingDate + ", empProbationPeriod = '" + strProbationPeriod + "' , empNotes = '" + strNotes + "' , empEmail = '" + strEmail + "' , empAccountNo = '" + strAccountno + "' , empBDate = " + strBDate + ", empADate =" + strADate + " , empPrevEmployer = '" + strPrevEmployer + "' , empExperince = " + intexperince.ToString() + " , ModifiedOn = '" + strInsertedOn + "' , ModifiedBy = " + intInsertedBy.ToString() + " , ModifiedIP = '" + strInsertedIP + "', IsTester ='" + blnTester.ToString() + "'WHERE empid = " + intEmpID.ToString()
    '            objDBFunction.ExecuteSQL(sql)
    '            Return New DataTable()
    '        End If
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    Public Function EmployeeQualificationList(intEmpID As Integer) As DataTable
        Try
            If intEmpID = 0 Then
                Return objDBFunction.ExecuteSQLRtnDT("Select * from empQualificationMaster order by qualificationDesc")
            ElseIf intEmpID > 0 Then
                Return objDBFunction.ExecuteSQLRtnDT("Select empQualificationMaster.qualificationDesc as 'QualDesc' from empQualificationMaster inner join empQualification on empQualificationMaster.qualificationId = empQualification.qualificationId where empQualification.empid =" + intEmpID.ToString() + " order by qualificationDesc")
            End If
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function EmployeeSkillList(strSkillID As String, strMore As String) As DataTable
        Try
            If (strMore = "ALL") Then
                Return objDBFunction.ExecuteSQLRtnDT("Select * from skillMaster order by SkillDesc")
            ElseIf (strMore = "One") Then
                Return objDBFunction.ExecuteSQLRtnDT("Select SkillDesc from skillMaster where SkillId =" + strSkillID)
            End If
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    'Public Function HoildayList() As DataTable
    '    Try
    '        Dim strSQL As String
    '        If Month(Now()) > 10 Then
    '            strSQL = "SELECT * FROM holidayMaster WHERE holidayDate BETWEEN '1-Jan-" & Year(Now()).ToString() & _
    '           "' AND '31-Mar-" & (Year(Now()) + 1).ToString() & "' ORDER BY holidayDate"
    '        Else
    '            strSQL = "SELECT * FROM holidayMaster WHERE holidayDate BETWEEN '1-Jan-" & Year(Now()).ToString() & _
    '           "' AND '31-Dec-" & Year(Now()).ToString() & "' ORDER BY holidayDate"
    '        End If

    '        Return objDBFunction.ExecuteSQLRtnDT(strSQL)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    Public Function NewsList() As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("SELECT TOP 1 notice_descr,date FROM notice ORDER BY noticeid DESC")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function GetQualificationId(strQualificationName As String) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select qualificationId from empQualificationMaster where qualificationdesc ='" + strQualificationName + "'")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Sub EmployeeQualifcation(intEmployeeId As Integer, intQualificationId As Integer, strMore As String)
        Try
            If strMore = "Insert" Then
                objDBFunction.ExecuteSQL("Insert Into empQualification(empId, qualificationId) values(" + intEmployeeId.ToString() + "," + intQualificationId.ToString() + ")")
            ElseIf strMore = "Delete" Then
                objDBFunction.ExecuteSQL("Delete from empQualification where empId =" + intEmployeeId.ToString() + "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ManageEmpSecondarySkills(employeeId As Integer, techskillId As String, mode As String)
        Try
            If mode = "Insert" Then
                objDBFunction.ExecuteSQL("Insert Into EmpSecondarySkills(EmpId, TechSkillId) values(" + employeeId.ToString() + "," + techskillId + ")")
            ElseIf mode = "Delete" Then
                objDBFunction.ExecuteSQL("Delete from EmpSecondarySkills where EmpId =" + employeeId.ToString() + "")
            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Function GetSkillId(strSkillName As String) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select skillid from skillMaster where skilldesc ='" + strSkillName + "'")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Sub AddRow(dtTempTable As DataTable)
        Dim dr1 As DataRow = dtTempTable.NewRow()
        dr1("Type") = "e"
        dr1("Name") = "Temp"
        dr1("Menu") = "Temp"
        dr1("IsMenuVisible") = "False"
        dr1("SecurityLevelView") = 10
        dr1("SecurityLevelAdd") = 10
        dr1("SecurityLevelUpdate") = 10
        dr1("SortOrder") = 10
        dr1("InsertedOn") = DateTime.Now.ToString()
        dr1("InsertedBy") = 1000
        dr1("InsertedIP") = "10:0:0:1"
        dtTempTable.Rows.Add(dr1)
    End Sub
    Public Function IsExistsModuleName(strModuleName As String, strModuleID As String) As Boolean
        Dim dt As DataTable
        dt = objDBFunction.ExecuteSQLRtnDT("Select * from Module where Name = '" + strModuleName.ToString() + "' and ModuleID <> '" + strModuleID + "'")
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Modules(strMore As String, strModuleID As String, strModuleParentID As String, strUserType As String, strMenuName As String, _
                           strDisplayName As String, strPageUrl As String, strIsGenric As String, strIsVisible As String, _
                           strSortOrder As String, strDate As String, strIP As String, strBY As String) As DataTable
        Try
            If strMore = "Select" Then

                Dim dtTempTable As DataTable = objDBFunction.ExecuteSQLRtnDT("Select A.*,B.Name as 'Module_ParentName' from Module A left outer join Module B on A.ModuleID_Parent = B.ModuleID")
                'AddRow(dtTempTable)
                'AddRow(dtTempTable)

                Return dtTempTable
            ElseIf strMore = "Insert" Then

                Dim strquery As String = "Insert into Module(ModuleID_Parent,[Type],Name,Menu,EntryPage,IsGenric,IsMenuVisible," _
                                                     & "SortOrder,InsertedOn,InsertedBy,InsertedIP) values(" + strModuleParentID + ",'" + strUserType + "','" + strMenuName + "','" + strDisplayName + "'," + strPageUrl + ",'" + strIsGenric + "','" + strIsVisible + "'," + strSortOrder + ",'" + strDate + "'," + strBY + ",'" + strIP + "')"

                Return objDBFunction.ExecuteSQLRtnDT(strquery)
            ElseIf strMore = "Update" Then

                Dim strquery As String = "UPDATE Module set ModuleID_Parent = " + strModuleParentID + "," _
                                                                        & "Type = '" + strUserType + "'," _
                                                                        & "Name = '" + strMenuName + "'," _
                                                                        & "Menu = '" + strDisplayName + "'," _
                                                                        & "EntryPage = " + strPageUrl + "," _
                                                                        & "IsGenric = '" + strIsGenric + "'," _
                                                                        & "IsMenuVisible = '" + strIsVisible + "'," _
                                                                        & "SortOrder = " + strSortOrder + "," _
                                                                        & "ModifiedOn = getDate()," _
                                                                        & "ModifiedBy = " + strBY + "," _
                                                                        & "ModifiedIP = '" + strIP + "'" _
                                                                        & "where ModuleID = " + strModuleID
                Return objDBFunction.ExecuteSQLRtnDT(strquery)
            ElseIf strMore = "SelectModID" Then
                Return objDBFunction.ExecuteSQLRtnDT("Select A.*,B.Name as 'Module_ParentName' from Module A left outer join Module B on A.ModuleID_Parent = B.ModuleID where A.ModuleID = " + strModuleID)
            End If
        Catch
            Return New DataTable()
        End Try
        Return New DataTable()
    End Function

    Public Function GetParentModuleNames() As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select ModuleID,Name from Module where ModuleID_Parent is Null order by Name")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Sub UpdatePassword(strEmail As String, intEmpId As Integer, strPassword As String)
        Try
            Dim sql As String
            sql = "UPDATE employeeMaster SET empPassword ='" + strPassword + "' WHERE empid = " + intEmpId.ToString() + " and empEmail = '" + strEmail + "'"
            objDBFunction.ExecuteSQL(sql)
        Catch ex As Exception

        End Try
    End Sub

    Public Function Package(strEmpID As String, strMore As String, strMonth As String, strYear As String) As DataTable
        Try
            Dim strQuery As String = ""
            If strMore = "Pack1" Then
                strQuery = "select convert(varchar(3),DateName( month , DateAdd( month , empPayMonth , 0 ) - 1 )) + '-' +  Convert(varchar(5),empPayYear)  EffectiveFrom,(employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta) + (select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid) as totalctc , empPayBonus,convert(varchar,empPayBonus)+' '+'('+case when IsPerformanceBased=0 then 'Fixed)' else 'Performance Based)' end as empPayBonus1  from employeepaymaster where empid=" & strEmpID
            ElseIf strMore = "Pack2" Then
                strQuery = "select  E.empid, E.AnnualtotalCTC,E.totalgross from (select (employeemaster.empname) as empname,CONVERT(CHAR(11),employeemaster.empJoiningDate) as empJdate,(employeepaymaster.emppaybasic)as emppaybasic,(employeepaymaster.emppaymonth) as emppayMonth,(employeepaymaster.emppayyear) as emppayYear,employeemaster.empid,      ((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualtotalCTC,           floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as totalctc,         floor( (select Case when ispf=1 then (employeepaymaster.emppaybasic * 100/40)  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as totalgross,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic * 100/40))-((employeepaymaster.emppaybasic * 12/100)) -(employeepaymaster.empPT) else ( ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) -(employeepaymaster.empPT) ) end  from employeemaster where empid=employeepaymaster.empid))as totalnet   from employeepaymaster RIGHT JOIN employeemaster ON employeepaymaster.empid=employeemaster.empid where employeemaster.empleavingdate is NULL OR employeemaster.empleavingdate >' " & DateTime.Now & "')E where E.empid = " & strEmpID
            ElseIf strMore = "MonYearPay" Then
                strQuery = "select top 1 * from employeePayProcessDetail,employeePayProcess where " + "employeePayProcess.payId = employeePayProcessDetail.payID and month(employeePayProcess.paydate) =" + strMonth + " and Year(employeePayProcess.paydate) =" + strYear + " and employeePayProcessDetail.empId =" + strEmpID + " order by employeePayProcessDetail.empPayID"
            ElseIf strMore = "Details" Then
                strQuery = "select e.empname,e.empjoiningdate,s.skilldesc from employeemaster as e inner join skillmaster as s on e.skillid=s.skillid where empid=" + strEmpID
            End If
            Return objDBFunction.ExecuteSQLRtnDT(strQuery)
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Sub GetMultiRowHeader(e As GridViewRowEventArgs, GetCels As SortedList)

        If e.Row.RowType = DataControlRowType.Header Then
            Dim row As GridViewRow = Nothing
            Dim enumCels As IDictionaryEnumerator = GetCels.GetEnumerator()

            row = New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)
            While enumCels.MoveNext()

                Dim count As String() = enumCels.Value.ToString().Split(Convert.ToChar(","))
                Dim Cell As TableHeaderCell = Nothing
                Cell = New TableHeaderCell()
                Cell.RowSpan = Convert.ToInt16(count(2).ToString())
                Cell.ColumnSpan = Convert.ToInt16(count(1).ToString())
                Cell.Controls.Add(New LiteralControl(count(0).ToString()))
                Cell.HorizontalAlign = HorizontalAlign.Center
                ' Cell.Attributes.Add("text-align", "center");
                Cell.Style.Add("text-align", "center")
                row.Cells.Add(Cell)

            End While

            e.Row.Parent.Controls.AddAt(0, row)
        End If
    End Sub

    'Public Function ItemInvoiceDetails(userid As String, strmodifiedip As String, iteminvoiceid As String, supplierid As String, invoiceno As String, purchasedate As String, note As String, strmore As String) As DataTable
    '    Dim dttemptable As DataTable = New DataTable()
    '    ' try
    '    Dim strquery As String
    '    strquery = String.Empty

    '    If strmore = "insert" Then
    '        strquery = "set dateformat dmy; insert into iteminvoicedetails (supplierid,invoiceno,purchasedate,note,insertedby,insertedon,insertedip) values('" + supplierid + "','" + invoiceno + "',convert(date,'" + Convert.ToDateTime(purchasedate).ToString() + "'),'" + note + "'," + userid + ",convert(date,'" + DateTime.Now.ToString() + "'),'" + strmodifiedip + "')"
    '        objDBFunction.ExecuteSQLRtnDT(strquery)
    '        dttemptable = objDBFunction.ExecuteSQLRtnDT("select iteminvoiceid from iteminvoicedetails where invoiceno='" + invoiceno + "'")
    '    ElseIf strmore = "select" Then
    '        dttemptable = objDBFunction.ExecuteSQLRtnDT("select iteminvoiceid from iteminvoicedetails where invoiceno='" + invoiceno + "'")
    '    ElseIf strmore = "selectall" Then
    '        dttemptable = objDBFunction.ExecuteSQLRtnDT("select iteminvoicedetails.iteminvoiceid,iteminvoicedetails.invoiceno,(select sum(price) from item where item.iteminvoiceid=iteminvoicedetails.iteminvoiceid) as totalprice ,iteminvoicedetails.purchasedate,supplier.name as supplier,iteminvoicedetails.note from iteminvoicedetails inner join supplier on iteminvoicedetails.supplierid=supplier.supplierid")
    '    ElseIf strmore = "categorywise" Then
    '        ''for reading purpose.
    '        Dim categoryid As String
    '        categoryid = supplierid

    '        dttemptable = objDBFunction.ExecuteSQLRtnDT("select iid.iteminvoiceid,iid.invoiceno,iid.purchasedate,iid.note,i.itemid,i.categoryid,i.brandid,i.price,i.description,i.quantity,i.expirydate,i.serialno,s.supplierid,s.name as supplier,c.categoryid,c.name as category ,b.brandid, b.name as brand " +
    '                                                    " from iteminvoicedetails iid inner join item i on iid.iteminvoiceid =i.iteminvoiceid inner join  supplier s on iid.supplierid =s.supplierid inner join brand b on i.brandid =b.brandid inner join category c on i.categoryid =c.categoryid   where c.categoryid ='" + categoryid + "' ")
    '    ElseIf strmore = "selectiteminvoiceid" Then
    '        dttemptable = objDBFunction.ExecuteSQLRtnDT("select iid.iteminvoiceid,iid.invoiceno,iid.purchasedate,iid.note,i.itemid,i.price,i.description,i.quantity,i.expirydate,i.serialno,s.supplierid,s.name as supplier,c.categoryid,c.name as category ,b.brandid, b.name as brand,ia.itemattributeid,ia.value,a.attributeid,a.name,a.defaultvalue, a.type " +
    '                                                    " from iteminvoicedetails iid full join item i on iid.iteminvoiceid =i.iteminvoiceid full join  supplier s on iid.supplierid =s.supplierid full join brand b on i.brandid =b.brandid full join category c on i.categoryid =c.categoryid  full join itemattribute ia on ia.itemid =i.itemid full join attribute a on a.categoryid =c.categoryid where iid.iteminvoiceid ='" + iteminvoiceid + "'")
    '    ElseIf strmore = "update" Then
    '        objDBFunction.ExecuteSQLRtnDT("set dateformat dmy; update iteminvoicedetails set supplierid='" + supplierid + "',invoiceno= '" + invoiceno + "',purchasedate='" + purchasedate + "',note='" + note + "',modifiedon ='" + Now.ToString() + "',modifiedby=" + userid + ",modifiedip='" + strmodifiedip + "' where iteminvoiceid='" + iteminvoiceid + "'")
    '    End If

    '    ' dttemptable = objdbfunction.executesqlrtndt(strquery)
    '    Return dttemptable
    '    'catch ex as exception
    '    'return dttemptable
    '    ' end try
    'End Function

    'Public Function ItemInvoiceDetails(UserID As String, strModifiedIP As String, ItemInvoiceID As String, SupplierID As String, InvoiceNo As String, PurchaseDate As String, Note As String, vatPercentage As Decimal, strMore As String) As DataTable
    '    Dim dtTempTable As DataTable = New DataTable()
    '    Try

    '        Dim param As Object() = New Object(10) {}
    '        If SupplierID = "" Then
    '            param(0) = DBNull.Value
    '        Else
    '            param(0) = Convert.ToInt32(SupplierID)
    '        End If

    '        param(1) = InvoiceNo

    '        If ItemInvoiceID = "" Then
    '            param(2) = DBNull.Value
    '        Else
    '            param(2) = Convert.ToInt32(ItemInvoiceID)
    '        End If

    '        If PurchaseDate = "" Then
    '            param(3) = DBNull.Value
    '        Else
    '            param(3) = PurchaseDate
    '        End If

    '        param(4) = Note

    '        If UserID = "" Then
    '            param(5) = DBNull.Value
    '        Else
    '            param(5) = Convert.ToInt32(UserID)
    '        End If

    '        param(6) = vatPercentage
    '        param(7) = DateTime.Now

    '        If UserID = "" Then
    '            param(8) = DBNull.Value
    '        Else
    '            param(8) = Convert.ToInt32(UserID)
    '        End If
    '        param(9) = strModifiedIP   'insertedip
    '        param(10) = strMore
    '        dtTempTable = objDBFunction.ExecuteProcedureRtnDT("SP_InvoiceDetails", param)

    '        Return dtTempTable
    '    Catch ex As Exception
    '        Return dtTempTable
    '    End Try
    'End Function
    Public Function GetTaskDetails(strEmpID As String) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("SELECT * from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] as projectName , [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function GetDataForFilter(strEmpID As String, strColName As String) As DataTable
        Try
            Dim dtTask As DataTable = New DataTable()
            If (strColName = "Task") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(bugs_bug_name) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            ElseIf (strColName = "No") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(bugs_bug_id) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            ElseIf (strColName = "Status") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(statuses_status) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            ElseIf (strColName = "Projects") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(projects_project_name) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            ElseIf (strColName = "Priority") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(priorities_priority_desc) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            ElseIf (strColName = "Assigned To") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(bugs_assigned_to) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            ElseIf (strColName = "Last Modified") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(bugs_date_resolved) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            ElseIf (strColName = "Posted On") Then
                dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(bugs_date_assigned) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            End If

            Return dtTask
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
#Region "Rita 25June"

    Public Function GetTaskDetailsFilter(strEmpID As String, strProjName As String, strProjModule As String, strPriority As String, strAssignedTo As String, strStatus As String, strTaskID As String, strFrom As String, strTo As String) As DataTable
        Try
            If (strProjName = "--- ALL ---" Or strProjName = "--- SELECT ---") Then
                strProjName = "temp.projects_project_name"
            Else
                strProjName = "'" + strProjName + "'"
            End If

            If (strProjModule = "--- ALL ---" Or strProjModule = "--- SELECT ---") Then
                strProjModule = "temp.projectModule"
            Else
                strProjModule = "'" + strProjModule + "'"
            End If

            If (strPriority = "--- ALL ---" Or strPriority = "--- SELECT ---") Then
                strPriority = "temp.priorities_priority_desc"
            Else
                strPriority = "'" + strPriority + "'"
            End If

            If (strStatus = "--- ALL ---" Or strStatus = "--- SELECT ---") Then
                strStatus = "temp.statuses_status"
            Else
                strStatus = "'" + strStatus + "'"
            End If

            If (strAssignedTo = "--- ALL ---" Or strAssignedTo = "--- SELECT ---") Then
                strAssignedTo = "temp.bugs_assigned_to"
            Else
                strAssignedTo = "'" + strAssignedTo + "'"
            End If

            If (strTaskID = "") Then
                strTaskID = "temp.bugs_bug_id"
            Else
                strTaskID = "'" + strTaskID + "'"
            End If

            strFrom = ChangeDatePattern(strFrom, "MM/dd/yyyy")
            If (strFrom = "") Then
                strFrom = "temp.bugs_bug_lastmodified"
            Else
                strFrom = "'" + strFrom + "'"
            End If

            strTo = ChangeDatePattern(strTo, "MM/dd/yyyy")
            If (strTo = "") Then
                strTo = "temp.bugs_bug_lastmodified"
            Else
                strTo = "'" + strTo + "'"
            End If

            Return objDBFunction.ExecuteSQLRtnDT("SELECT * from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] as projectName , [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ") AND temp.projects_project_name =" + strProjName + " AND temp.projectModule = " + strProjModule + " AND temp.statuses_status = " + strStatus + " AND temp.priorities_priority_desc = " + strPriority + " AND temp.bugs_assigned_to = " + strAssignedTo + " AND temp.bugs_bug_id = " + strTaskID + " AND (temp.bugs_bug_lastmodified >= " + strFrom + " AND temp.bugs_bug_lastmodified <= " + strTo + ")")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function


    Public Function GetDataForProjects(strEmpID As String) As DataTable
        Try
            Dim dtTask As DataTable = New DataTable()
            dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(projId),projName from projectMaster where custId=" + strEmpID)
            Return dtTask
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    Public Function GetDataForStatus(strEmpID As String) As DataTable
        Try
            Dim dtTask As DataTable = New DataTable()
            dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(statuses_status),bugs_status_id from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            Return dtTask
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    Public Function GetDataForPriority(strEmpID As String) As DataTable
        Try
            Dim dtTask As DataTable = New DataTable()
            dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(priorities_priority_desc),bugs_priority_id from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectMAster].[projName] + '/' + [projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            Return dtTask
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    Public Function GetDataForModule(strEmpID As String) As DataTable
        Try
            Dim dtTask As DataTable = New DataTable()
            dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT Distinct(projectModule) from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            Return dtTask
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    Public Function GetDataForAssignedTo(strEmpID As String) As DataTable
        Try
            Dim dtTask As DataTable = New DataTable()
            dtTask = objDBFunction.ExecuteSQLRtnDT("SELECT distinct(bugs_assigned_to),asemployees_employee_id from ( SELECT empName as bugs_assigned_to, [bugs].[bug_id] as bugs_bug_id,[bugs].[bug_name] as bugs_bug_name,[bugs].[priority_id] as bugs_priority_id, [projectModuleMaster].[projid] as bugs_project_id,[bugs].[status_id] as bugs_status_id,bugStatuses.SortOrder,[bugs].[date_resolved] as bugs_date_resolved,[bugs].[date_assigned]as bugs_date_assigned, [projectMAster].[projid] as projects_project_id, [projectMAster].[projName] as projects_project_name, [bugpriorities].[priority_id] as priorities_priority_id, [bugpriorities].[priority_desc] as priorities_priority_desc, empid asemployees_employee_id, empName as employees_employee_name,[bugstatuses].[status_id] as statuses_status_id, [bugstatuses].[status] as statuses_status,CONVERT(VARCHAR(10), bug_lastModified, 101) +' '+CONVERT(VARCHAR(8),bug_lastModified, 108) as bugs_bug_lastmodified ,[projectModuleMaster].[moduleName] as projectModule from bugs, projectMaster, bugPriorities, Bugstatuses, employeeMasterView, projectModuleMaster where [projectModuleMaster].[moduleId]=[bugs].[moduleID] and[employeeMAsterView].[empid] = [bugs].[assigned_to] and [projectMAster].[projId] in (SELECT projId from projectmoduleMaster where projectmoduleMaster.moduleId=bugs.moduleId )  and [bugpriorities].[priority_id]=bugs.[priority_id] and ( [employeeMAsterView].[empId]=bugs.[assigned_to]  or [employeeMAsterView].[empId] = cast(substring(bugs.[assigned_by],3,4) as int) ) and [bugstatuses].[status_id]=bugs.[status_id] AND (bugs.[status_id]<>5)  AND (bugs.[status_id]<>5) ) as temp  where temp.bugs_project_id in (SELECT projid from projectMember where empid=" + strEmpID + ")")
            Return dtTask
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

#End Region
    Public Function GetData(strSql As String) As DataTable
        Return objDBFunction.ExecuteSQLRtnDT(strSql)
    End Function


#Region "By Satish"
    'Public Sub Attendance(empID As String, attDate As String, attStatus As String, attInTime As String, attOutTime As String, attComment As String, attIP As String, adminID As String, entryDate As String, strMore As String, workingHour As String)
    '    Try
    '        Dim strquery As String = String.Empty

    '        strquery = "Select * from empAtt where attDate ='" + attDate + "' and empID = " + empID
    '        Dim dtAtt As DataTable = objDBFunction.ExecuteSQLRtnDT(strquery)
    '        If (dtAtt.Rows.Count > 0) Then
    '            If (strMore = "ALL") Then
    '                strquery = "UPDATE empAtt Set attInTime=" + attInTime + ", attOutTime=" + attOutTime + ", attIP='" + attIP + "',adminID=" + adminID + ", entryDate='" + entryDate + "' where empid=" + empID + " and attDate='" + attDate + "'"
    '            ElseIf (strMore = "IN") Then
    '                strquery = "UPDATE empAtt Set attInTime=" + attInTime + ", attIP='" + attIP + "', entryDate='" + entryDate + "' where empid=" + empID + " and attDate='" + attDate + "'"
    '            ElseIf (strMore = "OUT") Then
    '                strquery = "UPDATE empAtt Set attOutTime=" + attOutTime + ", attIP='" + attIP + "', entryDate='" + entryDate + "' where empid=" + empID + " and attDate='" + attDate + "'"
    '            End If
    '            objDBFunction.ExecuteSQL(strquery)
    '            Return
    '        End If
    '        strquery = "Insert into empAtt(empID,attDate,attStatus,attInTime,attOutTime,attIP,adminID,entryDate) values(" + empID + ",'" + attDate + "','" + attStatus + "'," + attInTime + "," + attOutTime + ",'" + attIP + "'," + adminID + ",'" + entryDate + "')"
    '        objDBFunction.ExecuteSQL(strquery)
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Sub SaveRoles(RoleID As String, RoleName As String, strModifiedOn As String, intModifiedBy As Integer, strModifiedIP As String)
        Try
            Dim strquery As String = String.Empty

            strquery = "Select * from Roles where RoleID =" + RoleID
            Dim dtAtt As DataTable = objDBFunction.ExecuteSQLRtnDT(strquery)
            If (dtAtt.Rows.Count > 0) Then
                strquery = "UPDATE Roles Set RoleName='" + RoleName + "',ModifiedOn='" + strModifiedOn + "',ModifiedBy=" + intModifiedBy + ",ModifiedIP='" + strModifiedIP

            Else
                strquery = "Insert into Roles(RoleName,insertedOn,InsertedBy,InsertedIP) values('" + RoleName + "','" + strModifiedOn + "'," + intModifiedBy + ",'" + strModifiedIP + "')"

            End If
            objDBFunction.ExecuteSQL(strquery)

            Return
        Catch ex As Exception

        End Try
    End Sub


    Public Function GetRoles(ByVal RoleID As Integer) As DataTable
        Try

            Dim param As Object() = New Object(6) {}
            param(0) = RoleID
            'param(1) = RoleName
            'param(2) = UserMId
            'param(3) = InsertedOn
            'param(4) = InsertedBy
            'param(5) = InsertedIP
            param(6) = "select"
            Return objDBFunction.ExecuteProcedureRtnDT("SP_Supplier", param)
            'Dim strquery As String = String.Empty
            'If RoleID = 0 Then
            '    strquery = "Select * from Roles"

            '    'Else
            '    '    strquery = "Delete  from Roles where RoleID=" + RoleID
            '    '    Return objDBFunction.ExecuteSQL(strquery)
            'End If
            'Return objDBFunction.ExecuteSQLRtnDT(strquery)

        Catch ex As Exception
            Return New DataTable
        End Try
    End Function

    Public Function GetRolesDelete(ByVal RoleID As String) As Integer
        Try
            Dim strquery As String = String.Empty
            'If RoleID != 0 Then
            strquery = "Delete  from Roles where RoleID=" + RoleID
            objDBFunction.ExecuteSQL(strquery)
            Return 1
            ' End If

        Catch ex As Exception
            Return 0
        End Try
    End Function


    'Public Function GetAttendenceData(ByVal Startdate As String, ByVal Enddate As String, ByVal Userid As String) As DataTable
    '    Try
    '        Dim param As Object() = New Object(3) {}
    '        param(0) = DateTime.Parse(Startdate).ToString("yyyy-MM-dd")
    '        param(1) = DateTime.Parse(Enddate).ToString("yyyy-MM-dd")
    '        param(2) = Userid
    '        Return objDBFunction.ExecuteProcedureRtnDT("SP_GetAttendence", param)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function


    'Public Function UpcomingEvents() As DataTable
    '    Try
    '        Dim strquer As String = "select empname,empBDate,'BirthDay' as occation,empLeavingDate from employeemaster where " +
    '                                " convert(datetime, cast(year(getdate()) as varchar)+'/'+cast(month(empBdate) as varchar)+'/' +cast(day(empBdate) as varchar)) " +
    '                                " BETWEEN getdate() and dateadd(D,7,getdate()) and empLeavingdate is null union all  select empname,empADate,'Marriage Anniversary' " +
    '                                " as occation,empLeavingDate from employeemaster where convert(datetime, cast(year(getdate()) as varchar) " +
    '                                " +'/'+cast(month(empAdate) as varchar)+'/'+cast(day(empAdate) as varchar)) " +
    '                                " BETWEEN getdate() and dateadd(D,7,getdate()) and empLeavingdate is null "
    '        Return objDBFunction.ExecuteSQLRtnDT(strquer)
    '    Catch ex As Exception
    '        Return New DataTable()
    '    End Try
    'End Function

    'Public Function GetBdayList() As String
    '    Try
    '        Dim strquer As String = "select empname, 'Birthday'  as [event]  from employeemaster where day(empBDate) = day(getdate()) and " +
    '         " Month(empBDate) = Month(getdate()) " +
    '        " AND empBDate <> '1-jan-1900'  and empLeavingdate is null "

    '        Dim dTable As DataTable = objDBFunction.ExecuteSQLRtnDT(strquer)

    '        strquer = String.Empty
    '        If dTable.Rows.Count > 0 Then
    '            strquer = "<div style='float:Left; font-weight:bold;' >&nbsp;Happy Birthday: </div><div style='float:Left;font-weight:bold;'>" '<marquee scrollamount='2' behavior='alternate' direction='right' width='100%'>"
    '            For i As Integer = 0 To dTable.Rows.Count - 1
    '                strquer += "<span <span style='color:#E69503;' >" & dTable.Rows(i)("empname").ToString() & "</span> ,"
    '            Next
    '            strquer = strquer.TrimEnd(",")
    '            strquer += "</div><br/><br/>"
    '        End If
    '        Return strquer


    '    Catch ex As Exception
    '        Return String.Empty
    '    End Try
    'End Function

    'Public Function GetMAnnyversaryList() As String
    '    Try
    '        'Dim strquer As String = "select empname,'Marriage Anniversary'  as [event] from employeemaster where day(empADate) = day(getdate()) and " +
    '        '                            " month(empADate)  =month(getdate()) and empADate <> '1-jan-1900' and empLeavingdate is null "

    '        Dim strquer As String = "select empname from employeemaster where day(empADate) = day(getdate()) " +
    '                                   " and month(empADate)  =month(getdate()) and empADate <> '1-jan-1900' " +
    '                                   " and empLeavingdate is null "

    '        Dim dTable As DataTable = objDBFunction.ExecuteSQLRtnDT(strquer)

    '        strquer = String.Empty
    '        If dTable.Rows.Count > 0 Then
    '            strquer = "<div style='float:Left; font-weight:bold;'>&nbsp;Happy Marriage Anniversary: </div><div style='float:Left;font-weight:bold;'>"
    '            For i As Integer = 0 To dTable.Rows.Count - 1
    '                strquer += "<span style='color:#E69503;margin-right:5px;' >" & dTable.Rows(i)("empname").ToString() & "</span> ,"
    '            Next
    '            strquer = strquer.TrimEnd(",")
    '            strquer += "</div>"

    '        End If
    '        Return strquer

    '    Catch ex As Exception
    '        Return String.Empty
    '    End Try
    'End Function

    Public Function GetEncriptFileName(ByVal strval As String) As String
        Try
            Dim strquer As String = onjGeneral.EnryptString(strval)
            Return strquer

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Function GetDecriptFileName(ByVal strval As String) As String
        Try
            Dim strquer As String = onjGeneral.DecryptString(strval)
            Return strquer

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Function HRManualFileName() As String
        Try
            Dim strquer As String = GetEncriptFileName(ConfigurationManager.AppSettings("Hrmanualpath").ToString())
            Return strquer

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Function MediclaimFileName() As String
        Try
            Dim strquer As String = GetEncriptFileName(ConfigurationManager.AppSettings("Mediclaimpath").ToString())
            Return strquer

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Function AntiHarasmentFileName() As String
        Try
            Dim strquer As String = GetEncriptFileName(ConfigurationManager.AppSettings("Antiharrasmentpath").ToString())
            Return strquer

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Function CheckEmailID(ByVal EmailId As String) As Boolean
        Try
            Dim strquer As String = "select COUNT(*) as [C] from  employeeMaster where empEmail='" + EmailId + "' and empLeavingDate is null"
            Dim dt As DataTable
            dt = objDBFunction.ExecuteSQLRtnDT(strquer)
            If (dt.Rows(0)("C") > 0) Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region


    'End Class  Return New DataTable()
    '        End Try
    '    End Function

    Public Function EmployeeDetails(strLeavingDate As String, intEmpId As Integer) As DataTable
        'Try
        '    If strLeavingDate = "ALL" Then
        '        Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid order by empid desc")
        '    ElseIf strLeavingDate = "Left" Then
        '        Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empLeavingDate is not NULL order by empid desc ")
        '    ElseIf strLeavingDate = "Current" Then
        '        Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empLeavingDate is NULL order by empid desc")
        '    ElseIf strLeavingDate = "Select" Then
        '        Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empid=" + intEmpId.ToString() + "order by empid desc")
        '    End If
        'Catch ex As Exception
        '    Return New DataTable()
        'End Try

        Try
            If strLeavingDate = "ALL" Then
                'Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid order by empid desc")
                Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.*, ((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualCTC, floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as CTC, floor( (select Case when ispf=1 then (employeepaymaster.emppaybasic * 100/40)  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as Gross,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic * 100/40))-((employeepaymaster.emppaybasic * 12/100)) -(employeepaymaster.empPT) -(employeepaymaster.empInsurance) else ( ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) -(employeepaymaster.empPT)-(employeepaymaster.empInsurance)  ) end  from employeemaster where empid=employeepaymaster.empid))as Net From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid left JOIN  employeepaymaster ON employeepaymaster.empid=employeeMaster.empid order by employeeMaster.empid desc")
            ElseIf strLeavingDate = "Left" Then
                'Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empLeavingDate is not NULL order by empid desc ")
                Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.*,((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualCTC,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as CTC, floor( (select Case when ispf=1 then (employeepaymaster.emppaybasic * 100/40)  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as Gross,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic * 100/40))-((employeepaymaster.emppaybasic * 12/100)) -(employeepaymaster.empPT) -(employeepaymaster.empInsurance) else ( ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) -(employeepaymaster.empPT)-(employeepaymaster.empInsurance)  ) end  from employeemaster where empid=employeepaymaster.empid))as Net From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid left JOIN  employeepaymaster ON employeepaymaster.empid=employeeMaster.empid where empLeavingDate is not NULL order by employeeMaster.empid desc ")
            ElseIf strLeavingDate = "Current" Then
                'Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empLeavingDate is NULL order by empid desc")
                Return objDBFunction.ExecuteSQLRtnDT("SELECT  skillMaster.SecurityLevel,skillMaster.skillDesc as 'Skill',employeeMaster.* ,((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualCTC, floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as CTC, floor( (select Case when ispf=1 then (employeepaymaster.emppaybasic * 100/40)  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as Gross, floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic * 100/40))-((employeepaymaster.emppaybasic * 12/100)) -(employeepaymaster.empPT) -(employeepaymaster.empInsurance) else ( ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) -(employeepaymaster.empPT)-(employeepaymaster.empInsurance)  ) end  from employeemaster where empid=employeepaymaster.empid))as Net From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid  left JOIN  employeepaymaster ON employeepaymaster.empid=employeeMaster.empid where empLeavingDate is NULL order by employeemaster.empid desc")
            ElseIf strLeavingDate = "Select" Then
                'Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where empid=" + intEmpId.ToString() + "order by empid desc")
                Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel, skillMaster.skillDesc as 'Skill', employeeMaster.*,((((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))*12 )+ employeepaymaster.empPayBonus) as AnnualCTC,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster where empid=employeepaymaster.empid))  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as CTC,floor( (select Case when ispf=1 then (employeepaymaster.emppaybasic * 100/40)  else  ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) end  from employeemaster where empid=employeepaymaster.empid)) as Gross,floor((select Case when ispf=1 then ((employeepaymaster.emppaybasic * 100/40))-((employeepaymaster.emppaybasic * 12/100)) -(employeepaymaster.empPT) -(employeepaymaster.empInsurance) else ( ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)) -(employeepaymaster.empPT)-(employeepaymaster.empInsurance)  ) end  from employeemaster where empid=employeepaymaster.empid))as Net From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid left JOIN  employeepaymaster ON employeepaymaster.empid=employeeMaster.empid  where employeeMaster.empid=" + intEmpId.ToString() + " order by employeeMaster.empid desc")
            End If
        Catch ex As Exception
            Return New DataTable()
        End Try

    End Function
    Public Function SearchByName(ByVal strvalue As String, ByVal strLeavingDate As String) As DataTable
        Try
            If strLeavingDate = "ALL" Then
                Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel,'' as Net,'' as AnnualCTC,'' as CTC,'' as Gross, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%' or empJoiningDate like '%" + strvalue + "%') order by empid desc")
            ElseIf strLeavingDate = "Left" Then
                Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel,'' as Net,'' as AnnualCTC,'' as CTC,'' as Gross, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%') and empLeavingDate is not NULL order by empid desc")
            ElseIf strLeavingDate = "Current" Then
                Return objDBFunction.ExecuteSQLRtnDT("SELECT skillMaster.SecurityLevel,'' as Net,'' as AnnualCTC,'' as CTC,'' as Gross, skillMaster.skillDesc as 'Skill', employeeMaster.* From skillMaster Inner Join employeeMaster on skillMaster.skillid = employeeMaster.skillid where (empName like '%" + strvalue + "%' or empContact like '%" + strvalue + "%' or empEmail like '%" + strvalue + "%' or empNotes like '%" + strvalue + "%' or empAddress like '%" + strvalue + "%') and empLeavingDate is NULL order by empid desc")
            End If
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    'Public Function Employee(strName As String, strAddress As String, strContact As String, intSkill As Integer, strNotes As String, strJoiningDate As String, strProbationPeriod As String, strEmail As String, strAccountno As String, strBDate As String, strADate As String, strPrevEmployer As String, intexperince As Integer, strInsertedOn As String, intInsertedBy As Integer, strInsertedIP As String, ByRef intEmpID As Integer, strType As String, strLeavingDate As String, blnTester As Boolean) As DataTable
    '    Try
    '        If strType = "Insert" Then
    '            Dim dtEmployee As DataTable = New DataTable()
    '            Dim mastersql As String
    '            mastersql = "DECLARE @itemID int; Insert into UserMaster(UserType,SecurityLevel,IsAdmin,InsertedOn,InsertedBy,InsertedIP) values('e',5,0,'" + strInsertedOn + "'," + intInsertedBy.ToString() + ",'" + strInsertedIP + "') SET @itemID = (select @@identity) SELECT @itemID AS UserMasterID"
    '            Dim dt As System.Data.DataTable
    '            dt = objDBFunction.ExecuteSQLRtnDT(mastersql)
    '            intEmpID = Convert.ToInt16(dt.Rows(0)(0).ToString())
    '            Try
    '                Dim sql As String
    '                sql = "INSERT INTO employeeMaster(empid,empName,empAddress,empContact,skillId,empNotes,empJoiningDate,empLeavingDate,empProbationPeriod,empEmail,empAccountNo,empBDate,empADate,empPrevEmployer,empExperince,IsTester,InsertedOn,InsertedBy,InsertedIP) VALUES(" + dt.Rows(0)(0).ToString() + ",'" + strName + "','" + strAddress + "','" + strContact + "'," + intSkill.ToString() + ",'" + strNotes + "','" + strJoiningDate + "'," + strLeavingDate + "," + strProbationPeriod + ",'" + strEmail + "','" + strAccountno + "'," + strBDate + "," + strADate + ",'" + strPrevEmployer + "'," + intexperince.ToString() + ",'" + blnTester.ToString() + "','" + strInsertedOn + "'," + intInsertedBy.ToString() + ",'" + strInsertedIP + "')"
    '                dtEmployee = objDBFunction.ExecuteSQLRtnDT(sql)
    '            Catch ex As Exception
    '                objDBFunction.ExecuteSQLRtnDT("Delete from UserMaster where UserMasterID =" + intEmpID.ToString())
    '            End Try
    '            Return dtEmployee
    '        ElseIf strType = "Update" Then
    '            Dim sql As String
    '            sql = "UPDATE employeeMaster SET skillid =" + intSkill.ToString() + ", empName = '" + strName + "' , empAddress = '" + strAddress + "' , empContact = '" + strContact + "' , empJoiningDate = '" + strJoiningDate + "', empLeavingDate =" + strLeavingDate + ", empProbationPeriod = '" + strProbationPeriod + "' , empNotes = '" + strNotes + "' , empEmail = '



    Public Function GetemployeeNameByProjId(ByVal ProjId As String) As DataTable
        Try
            ' Return objDBFunction.ExecuteSQLRtnDT(" select empname from employeemaster where empid in(select empid from projectMember where projId= " + ProjId)
            Return objDBFunction.ExecuteSQLRtnDT("select empname from employeemaster where empid in(select empid from projectMember where projId=131)")
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    'Written By supriya 
    Public Function GetEmployeePhoto(ByVal intEmpId As Integer) As Byte()

        Dim strConnString As String = ConfigurationManager.ConnectionStrings("conString").ConnectionString
        Using connection As New SqlConnection(strConnString)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            command = New SqlCommand("Select Photo from employeemaster where empid=" + intEmpId.ToString(), connection)
            Dim imageB As Object = command.ExecuteScalar()
            If imageB IsNot DBNull.Value Then
                Dim imageData As Byte() = DirectCast(command.ExecuteScalar(), Byte())
                Return imageData
            Else
                Return Nothing
            End If
        End Using

        'Return objDBFunction.ExecuteSQLRtnDT("Select Photo from employeemaster where empid=" + intEmpId.ToString())


    End Function
    Public Function GetExistsClient(ByVal strEmail As String, ByVal intEmpId As Integer) As Integer
        Try
            Dim strConnString As String = ConfigurationManager.ConnectionStrings("conString").ConnectionString
            Using connection As New SqlConnection(strConnString)
                connection.Open()
                Dim dtEmployee As DataTable = New DataTable()
                Dim command As SqlCommand = connection.CreateCommand()
                command.Connection = connection
                command.CommandText = "select top 1 empid from employeemaster where empEmail = '" + strEmail.Trim() + "' and empid != " + intEmpId.ToString

                Dim Empid As Integer = command.ExecuteScalar()

                connection.Close()
                Return Empid

            End Using
            'Return objDBFunction.ExecuteSQLRtnDT("select empEmail from employeemaster where empEmail = '" + strEmail.Trim() + "'")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetLocationAcess(ByVal profileId As String) As Integer
        Try
            Dim strConnString As String = ConfigurationManager.ConnectionStrings("conString").ConnectionString
            Using connection As New SqlConnection(strConnString)
                connection.Open()
                Dim command As SqlCommand = connection.CreateCommand()
                command.Connection = connection
                command.CommandText = "select LocationId from Profile where ProfileId = " + profileId

                Dim locationId As Integer = command.ExecuteScalar()
                connection.Close()
                Return locationId

            End Using

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetLocationName(ByVal locationId As Integer) As String
        Try
            Dim strConnString As String = ConfigurationManager.ConnectionStrings("conString").ConnectionString
            Using connection As New SqlConnection(strConnString)
                connection.Open()
                Dim command As SqlCommand = connection.CreateCommand()
                command.Connection = connection
                command.CommandText = "select Name from location  where LocationId = " + locationId.ToString

                Dim location As String = command.ExecuteScalar()
                connection.Close()
                Return location

            End Using

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetCompanydetails(intEmpId As Integer) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("select CompanyName, CompanyAddress,CompanyLogo from employeeMaster emp inner join Location L on emp.LocationFKID = L.LocationID where empid=" + intEmpId.ToString)
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function GetTechSkills(employeeId As Integer) As DataTable
        Try
            If employeeId = 0 Then
                Return objDBFunction.ExecuteSQLRtnDT("Select * from techSkills")
            Else
                Return objDBFunction.ExecuteSQLRtnDT("select T.techId from techSkills T join dbo.EmpSecondarySkills S on T.techId=S.TechSkillId where S.EmpId = " + employeeId.ToString())
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    'Section Start Project Module
    Public Function GetProjectName(ByVal strEmpID As String) As DataTable
        Try

            Dim param As Object() = New Object(1) {}
            param(0) = strEmpID
            Return objDBFunction.ExecuteProcedureRtnDT("sp_GetProjects", param)
            ' Return objDBFunction.ExecuteSQLRtnDT("select projectMaster.projid, projectMaster.projName from projectMaster join projectMember on projectMaster.projid = projectMember.projid where projectMember.empID = " + strEmpID)
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function GetModuleName(ByVal strProjectID As String) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("select moduleid,moduleName from projectModuleMaster where projId=" + strProjectID)
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Function GetModuleNameByID(ByVal strModuleID As String) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("select moduleid,moduleName,moduleDescription,moduleEstimate from projectModuleMaster where moduleId=" + strModuleID)
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

    Public Sub UpdateModule(ByVal strModuleTypeID As Integer, ByVal strModuleName As String, ByVal strModuleDesc As String, ByVal intModuleEst As Integer, ByVal intProjId As Integer)
        Try
            Dim str As String = "UPDATE projectModuleMaster SET ProjectModuleTypeID='" + strModuleTypeID.ToString() +
                "', moduleName ='" + strModuleName + "', moduleDescription='" + strModuleDesc + "', moduleEstimate=" +
                intModuleEst.ToString() + ", ModifiedOn=getDate() WHERE moduleId = " + Convert.ToString(intProjId)

            objDBFunction.ExecuteSQL(str)
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message)
        End Try

    End Sub
    Public Sub UpdateModuleRefID(ByVal intModuleId As Integer)
        Try
            objDBFunction.ExecuteSQL("Update projectModuleMaster set moduleRefId=(select top 1 moduleid from projectModuleMaster order by InsertedOn desc)  WHERE moduleId = " + intModuleId.ToString())
        Catch ex As Exception

        End Try
    End Sub

    Public Function GetModuleID() As String
        Dim ModID As String = ""
        Dim db As New DataTable()
        Dim strQuery As String = "select top 1 moduleid from projectModuleMaster order by InsertedOn desc"
        db = objDBFunction.ExecuteSQLRtnDT(strQuery)
        If db.Rows.Count > 0 Then
            ModID = db.Rows(0).Item("moduleid").ToString()
        End If
        Return ModID
    End Function

    Public Sub InsertModuleDetails(ByVal intModuleType As Integer, ByVal intProjId As Integer, ByVal intModuleRefId As Integer, ByVal intModuleEstimate As Integer, ByVal strModName As String, ByVal strModDesc As String)
        Try
            objDBFunction.ExecuteSQL("Insert Into projectModuleMaster(ProjectModuleTypeID,projid,moduleRefId,moduleEstimate,moduleName,moduleDescription) values(" + intModuleType.ToString() + "," + intProjId.ToString() + "," + intModuleRefId.ToString() + "," + intModuleEstimate.ToString() + ",'" + strModName + "','" + strModDesc + "')")

        Catch ex As Exception

        End Try
    End Sub

    ' To be deleted
    Public Function checkModName(ByVal intProjId As Integer, ByVal intRefProjId As String, ByVal strModName As String) As Boolean
        Dim exists As Boolean = False
        Dim db As New DataTable()
        Dim strQuery As String

        If (intRefProjId <> 0) Then
            strQuery = "select moduleName from projectModuleMaster where projID=" + intProjId.ToString() +
                " AND moduleRefId=" + intRefProjId + " AND moduleName='" + strModName + "'"
        Else
            strQuery = "select moduleName from projectModuleMaster where projID=" + intProjId.ToString() +
                " AND moduleName='" + strModName + "'"
        End If

        db = objDBFunction.ExecuteSQLRtnDT(strQuery)
        If db.Rows.Count > 0 Then
            exists = True
        End If
        Return exists
    End Function
    Public Function GetAlertCC(intProjectId As Integer) As DataTable
        Try
            Return objDBFunction.ExecuteSQLRtnDT("Select AlertCC from ProjectMaster where projId =" + Convert.ToString(intProjectId))
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function
    'Section End Project Module

End Class

