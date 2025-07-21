
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.string" %>
<%@import Namespace="System.Data.SqlClient"%>
<%@ Page Language="VB" %>

      
<%
	  Dim custid as integer=request.querystring("custId")
	  Dim strSQL as String ="select custname,custemail,custpassword,custid from customermaster  where custId =" & custid


    Dim strCustid As String = String.Empty
    Dim strCustname As String = String.Empty
    Dim strCustpassword As String = String.Empty
    Dim strcustemail As String = String.Empty


		Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
		Dim conn As SqlConnection = New SqlConnection( dsn1)
            conn.Open()
            Dim objcmd As SqlCommand = New SqlCommand(strsql, conn)
            Dim objdatareader As SqlDataReader
            objdatareader = objcmd.ExecuteReader
            If objdatareader.Read() Then
				
					strCustid = objdatareader("custid")
					strCustPassword = objdatareader("custPassword")
					strCustname = objdatareader("Custname")
                    strCustemail = objdatareader("Custemail")
		       
            end if

            objdatareader.Close()
            conn.Close()		
%>

     <html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Hello</title>
</head>


<body>

<table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111" width="711">
  <tr>
    <td width="711" colspan="2"><font face="Arial" size="2">Hello  &nbsp;<%=strCustname%> ,</font></td>
  </tr>
  <tr>
    <td width="711" colspan="2">&nbsp;</td>
  </tr>
  <tr>
    <td width="711" colspan="2"><font face="Arial" size="2">Thank you for choosing to work 
    with Dynamic Web Technologies. </font></td>
  </tr>
  <tr>
    <td width="711" colspan="2">&nbsp;</td>
  </tr>
  <tr>
    <td width="711" colspan="2">Account Details</td>
  </tr>
  <tr>
    <td width="177" nowrap="nowrap"="nowrap="nowrap""><b>Login Id</b></td>
    <td width="534" nowrap="nowrap"="nowrap="nowrap""><%=strCustid%></td>
  </tr>
  <tr>
    <td width="177"  nowrap="nowrap"="nowrap="nowrap""><b>Password</b></td>
    <td width="534" nowrap="nowrap"="nowrap="nowrap""><%=strCustpassword%></td>
  </tr>
  <tr>
    <td width="177" nowrap="nowrap"="nowrap="nowrap""><b>Registered Email Id</b></td>
    <td width="534" nowrap="nowrap"="nowrap="nowrap""><%=strcustemail%></td>
  </tr>
  <tr>
    <td width="711" colspan="2">&nbsp;</td>
  </tr>
  <tr>
    <td width="711" colspan="2"><font face="Arial" size="2">You will now have access to the 
    following systems as a customer of Dynamic Web;</font></td>
  </tr>
  <tr>
    <td width="711" colspan="2">&nbsp;</td>
  </tr>
  <tr>
    <td width="711" colspan="2"><b><font face="Arial" size="2">1. Project Reports<br/>
    </font></b><font face="Arial" size="2">Project progress reports are sent 
    through the system which contains a brief on the status along with the 
    project plan indicating % completion. As a customer archives of various 
    project reports can be viewed of those sent every week<br/>
&nbsp;</font></td>
  </tr>
  <tr>
    <td width="711" colspan="2"><b><font face="Arial" size="2">2. Bug Management<br/>
    </font></b><font face="Arial" size="2">This is an interactive tool to enable 
    you to add any bugs relevant to your project/system that arise out of the 
    development performed by our team. Once you post a bug, the concerned team 
    is notified through email and they begin work on it. Once the bug is 
    rectified, the customer can test and terminate it once accepted.<br/>
&nbsp;</font></td>
  </tr>
  <tr>
    <td width="711" colspan="2"><b><font face="Arial" size="2">3. Change Request<br/>
    </font></b><font face="Arial" size="2">During the course of the project if 
    there are major functional changes that need to be incorporated, this 
    section needs to be used to intimate our&nbsp; team about the changes. This 
    will of course be discussed. The tool however is a documented proof of the 
    changes requested and the same is approved and mutually agreed to maintain 
    transparency during the course of the project. <br/>
&nbsp;</font></td>
  </tr>
  <tr>
    <td width="711" colspan="2"><b><font face="Arial" size="2">4. Customer Feedback System<br/>
    </font></b><font face="Arial" size="2">As a customer, you can at any point 
    of time during the course of the project can give your feedback about the 
    work performed by our team. Usually this feedback is taken once the project 
    is completed.<br/>
&nbsp;</font></td>
  </tr>
  <tr>
    <td width="711" colspan="2"><b><font face="Arial" size="2">5. Complaints<br/>
    </font></b><font face="Arial" size="2">As a customer, you can send in your 
    complaints to us and we will try and act upon it to provide the best 
    resolution possible. These complaints can be of any kind related to your 
    project.<br/>
&nbsp;</font></td>
  </tr>
  <tr>
    <td width="711" colspan="2"><b><font face="Arial" size="2">6. Making Credit Card 
    payments<br/>
    </font></b><font face="Arial" size="2">In case you are making payments using 
    the credit card (based on payment terms agreed), you can use this system to 
    make those payments. If there is a payment due, you will see an invoice and 
    you can follow that and make your payment. <br/>
&nbsp;</font></td>
  </tr>

</table>
<p>&nbsp;</p>
<p dir="ltr">&nbsp;</p>

</body>

</html>