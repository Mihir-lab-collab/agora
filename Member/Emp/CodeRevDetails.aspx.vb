Imports System.Data
Imports System.Data.SqlClient

Partial Class emp_codeRevDetails
    Inherits Authentication
    Dim gf As New generalFunction
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
 protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    
   
    
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        If Not Page.IsPostBack Then
            fillgrid()
        End If

    End Sub

    Sub fillgrid()
        Dim strsqldetails As String
        Dim dsdetails As DataSet
        Dim dadetails As SqlDataAdapter
       ' strsqldetails = "select tblcodeRevReport.comments,convert(CHAR(19), tblEmpFeedback.fbdate,7) as dd,tblcodeRevReport.coderevid from  tblEmpFeedback,tblcodeRevReport where tblcodeRevReport.coderevid=" & Request.QueryString("revId")
		strsqldetails="select convert(CHAR(19),ratedate,7) as dd,comments,coderevid from  tblcodeRevReport where coderevid=" & Request.QueryString("revId")
        strConn.Open()
        dadetails = New SqlDataAdapter(strsqldetails, strConn)
        dsdetails = New DataSet()
        dadetails.Fill(dsdetails)
        dgrddetails.DataSource = dsdetails
        dgrddetails.DataBind()
        strConn.Close()
    End Sub

   Protected Sub dgrddetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrddetails.ItemDataBound
       session("strEmp")=""
	
		If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
           
			
	    Dim fldresolution as String
		Dim strSQLhistory as String
		Dim cmdhistory as sqlcommand
	
		Dim comm as String
		Dim empname as String
			Dim strsqlcomment As String
			       Dim dtrFB As SqlDataReader
			Dim i as integer=Request.QueryString("revId")
            strsqlcomment = "select tblEmpFeedback.FeedBack,employeemaster.empname,convert(varchar(10),tblEmpFeedback.fbdate,7) as datefb  from tblEmpFeedback,employeemaster where tblEmpFeedback.empid=employeemaster.empid and tblEmpFeedback.coderevid=" & i
	        Dim cmd As SqlCommand = New SqlCommand(strsqlcomment, strConn)
            dtrFB = cmd.ExecuteReader()
            while dtrFB.read  
					If strsqlcomment = ""  Then 
							fldresolution =  " - " & dtrFB("empname") & " - " &  dtrFB("datefb") & " - " & dtrFB("FeedBack")
					Else
							fldresolution =  fldresolution & vbcrlf & " - " &  dtrFB("empname") & " - " & dtrFB("datefb")& " - " & dtrFB("FeedBack")
					End If
 
		   End While
session("strEmp")=fldresolution
		   Dim txt as textBox
		   	
		   txt=e.item.FindControl("txtFeedbakHistory")
			txt.Text=session("strEmp")
		   'txtFeedbakHistory.Value= session("strEmp")
		
		 strConn.close
        End If
    End Sub
End Class
