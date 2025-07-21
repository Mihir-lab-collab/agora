Imports System.Web
Imports System.Data
Imports CommonFunctionLib
Imports generalFunction
Imports System.Data.SqlClient
Imports System

Public Class UserSession
    Public Shared objGeneralFunction As New generalFunction()
    Public Shared objDB As New DBFunc()
    Shared Status As String = ""
    Public Function CreateUserSessions(dr As DataRow, EmpData As Object) As [Boolean]
        Dim userdetails As New UserDetails()
        userdetails.EmployeeID = dr("empId")
        userdetails.UserType = dr("UserType")
        userdetails.IsAdmin = dr("IsAdmin")
        userdetails.SkillID = dr("skillID")
        userdetails.Name = dr("empName")
        userdetails.Address = dr("empAddress")
        userdetails.Contact = dr("empContact")
        userdetails.JoiningDate = dr("empJoiningDate")
        If IsDBNull(dr("empLeavingDate")) = False Then
            userdetails.LeavingDate = dr("empLeavingDate")
        End If
        userdetails.ProbationPeriod = dr("empProbationPeriod")
        If IsDBNull(dr("empNotes")) = False Then
            userdetails.Notes = dr("empNotes")
        End If
        userdetails.EmailID = dr("empEmail")
        If IsDBNull(dr("empAccountNo")) = False Then
            userdetails.AccountNo = dr("empAccountNo")
        End If
        If IsDBNull(dr("empBDate")) = False Then
            userdetails.BDate = dr("empBDate")
        End If
        If IsDBNull(dr("empADate")) = False Then
            userdetails.ADate = dr("empADate")
        End If
        If IsDBNull(dr("empPrevEmployer")) = False Then
            userdetails.PreviousEmployer = dr("empPrevEmployer")
        End If
        userdetails.InsertedOn = dr("InsertedOn")
        userdetails.InsertedBy = dr("InsertedBy")
        userdetails.InsertedIP = dr("InsertedIP")
        userdetails.LocationID = dr("LocationFKID")
        userdetails.ProfileID = Convert.ToString(dr("ProfileID"))
        'If IsDBNull(dr("LocationID")) = False Then
        '    userdetails.ProfileLocationID = dr("LocationID")
        'End If
        HttpContext.Current.Session("DynoEmpSessionObject") = userdetails
        HttpContext.Current.Session("MemberSession") = userdetails

        'To Be deleted
        userdetails.SecurityLevel = dr("SecurityLevel")
        ''''''''''''''''''''''''''''''

        Return True
    End Function

    Public Function CreateUserSessions(UM As UserMaster) As [Boolean]
        Dim userdetails As New UserDetails()
        userdetails.EmployeeID = UM.EmployeeID
        userdetails.UserType = UM.UserType
        userdetails.IsAdmin = UM.IsAdmin
        userdetails.SkillID = UM.SkillID
        userdetails.Name = UM.Name
        userdetails.Address = UM.Address
        userdetails.Contact = UM.Contact
        userdetails.JoiningDate = UM.JoiningDate
        userdetails.LeavingDate = UM.LeavingDate
        userdetails.ProbationPeriod = UM.ProbationPeriod
        userdetails.Notes = UM.Notes
        userdetails.EmailID = UM.EmailID
        userdetails.AccountNo = UM.AccountNo
        userdetails.BDate = UM.BDate
        userdetails.ADate = UM.ADate
        userdetails.PreviousEmployer = UM.PreviousEmployer
        userdetails.LocationID = UM.LocationID
        userdetails.ProfileID = Convert.ToString(UM.ProfileID)

        HttpContext.Current.Session("DynoEmpSessionObject") = userdetails
        HttpContext.Current.Session("MemberSession") = userdetails

        'To Be deleted
        userdetails.SecurityLevel = 1
        ''''''''''''''''''''''''''''''

        Return True
    End Function
End Class

