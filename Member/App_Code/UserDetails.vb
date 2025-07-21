Imports Microsoft.VisualBasic
<Serializable()>
Public Class UserDetails
    Private intEmpID As Integer
    Public Property EmployeeID() As Integer
        Get
            Return intEmpID
        End Get
        Set(value As Integer)
            intEmpID = value
        End Set
    End Property

    Private strUserType As String
    Public Property UserType() As String
        Get
            Return strUserType
        End Get
        Set(value As String)
            strUserType = value
        End Set
    End Property
    Private lbnIsAdmin As Boolean
    Public Property IsAdmin() As Boolean
        Get
            Return lbnIsAdmin
        End Get
        Set(value As Boolean)
            lbnIsAdmin = value
        End Set
    End Property

    Private intSkillID As Integer
    Public Property SkillID() As Integer
        Get
            Return intSkillID
        End Get
        Set(value As Integer)
            intSkillID = value
        End Set
    End Property

    Private strEmpName As String
    Public Property Name() As String
        Get
            Return strEmpName
        End Get
        Set(value As String)
            strEmpName = value
        End Set
    End Property

    Private strEmpAddress As String
    Public Property Address() As String
        Get
            Return strEmpAddress
        End Get
        Set(value As String)
            strEmpAddress = value
        End Set
    End Property

    Private strEmpContact As String
    Public Property Contact() As String
        Get
            Return strEmpContact
        End Get
        Set(value As String)
            strEmpContact = value
        End Set
    End Property


    Private strEmpJoiningDate As String
    Public Property JoiningDate() As String
        Get
            Return strEmpJoiningDate
        End Get
        Set(value As String)
            strEmpJoiningDate = value
        End Set
    End Property

    Private strEmpLeavingDate As String
    Public Property LeavingDate() As String
        Get
            Return strEmpLeavingDate
        End Get
        Set(value As String)
            strEmpLeavingDate = value
        End Set
    End Property

    Private strEmpProbationPeriod As String
    Public Property ProbationPeriod() As String
        Get
            Return strEmpProbationPeriod
        End Get
        Set(value As String)
            strEmpProbationPeriod = value
        End Set
    End Property

    Private strEmpNotes As String
    Public Property Notes() As String
        Get
            Return strEmpNotes
        End Get
        Set(value As String)
            strEmpNotes = value
        End Set
    End Property

    Private strEmpEmail As String
    Public Property EmailID() As String
        Get
            Return strEmpEmail
        End Get
        Set(value As String)
            strEmpEmail = value
        End Set
    End Property

    Private strEmpAccountNo As String
    Public Property AccountNo() As String
        Get
            Return strEmpAccountNo
        End Get
        Set(value As String)
            strEmpAccountNo = value
        End Set
    End Property

    Private strEmpBDate As String
    Public Property BDate() As String
        Get
            Return strEmpBDate
        End Get
        Set(value As String)
            strEmpBDate = value
        End Set
    End Property

    Private strEmpADate As String
    Public Property ADate() As String
        Get
            Return strEmpADate
        End Get
        Set(value As String)
            strEmpADate = value
        End Set
    End Property

    Private strEmpPrevEmployer As String
    Public Property PreviousEmployer() As String
        Get
            Return strEmpPrevEmployer
        End Get
        Set(value As String)
            strEmpPrevEmployer = value
        End Set
    End Property

    Private intEmpExperience As Integer
    Public Property Experience() As Integer
        Get
            Return intEmpExperience
        End Get
        Set(value As Integer)
            intEmpExperience = value
        End Set
    End Property

    Private strEmpInsertedOn As String
    Public Property InsertedOn() As String
        Get
            Return strEmpInsertedOn
        End Get
        Set(value As String)
            strEmpInsertedOn = value
        End Set
    End Property

    Private strEmpInsertedBy As String
    Public Property InsertedBy() As String
        Get
            Return strEmpInsertedBy
        End Get
        Set(value As String)
            strEmpInsertedBy = value
        End Set
    End Property

    Private strEmpInsertedIP As String
    Public Property InsertedIP() As String
        Get
            Return strEmpInsertedIP
        End Get
        Set(value As String)
            strEmpInsertedIP = value
        End Set
    End Property

    Private strEmpLocationID As String
    Public Property LocationID() As String
        Get
            Return strEmpLocationID
        End Get
        Set(value As String)
            strEmpLocationID = value
        End Set
    End Property
    Private strProfileID As String
    Public Property ProfileID() As String
        Get
            Return strProfileID
        End Get
        Set(value As String)
            strProfileID = value
        End Set
    End Property
    Private strProfileLocationID As String
    Public Property ProfileLocationID() As Integer
        Get
            Return strProfileLocationID
        End Get
        Set(value As Integer)
            strProfileLocationID = value
        End Set
    End Property
    Private blnIsModuleAdmin As Boolean
    Public Property IsModuleAdmin() As Boolean
        Get
            Return blnIsModuleAdmin
        End Get
        Set(value As Boolean)
            blnIsModuleAdmin = value
        End Set
    End Property


    'SecurityLevel To Be deleted
    Private intSecurityLevel As Integer
    Public Property SecurityLevel() As Integer
        Get
            Return intSecurityLevel
        End Get
        Set(value As Integer)
            intSecurityLevel = value
        End Set
    End Property
End Class
