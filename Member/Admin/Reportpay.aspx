

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
<TITLE>Pay Repot</TITLE>
<SCRIPT LANGUAGE="JavaScript">


function pick()
{
  if (window.opener && !window.opener.closed)

 symbol=window.opener.document.Form1.hdname.value;
 
 return symbol;
}



</SCRIPT>
</HEAD>

<BODY>
<TABLE BORDER="1" CELLSPACING="0" CELLPADDING="5" >
<TR BorderColor="#A2921E" BGCOLOR="#EDF2E6"><TD><B>EmpName</B></TD><TD><B>EmpId</B></TD><TD><B>Basic</B></TD><TD><B>Hra</B></TD><TD><B>Conveyance</B></TD><TD><B>Medical</B></TD><TD><B>Food</B></TD><TD><B>Special</B></TD><TD><B>Lta</B></TD><TD><B>EPf</B></TD><TD><B>Pf</B></TD><TD><B>At</B></TD><TD><B>Pt</B></TD><TD><B>Loan</B></TD><TD><B>Advance</B></TD><TD><B>Leave</B></TD><TD><B>Deduction</B></TD><TD><B>Bonus</B></TD><TD><B>Addition</B></TD><TD><B>Remarks</B></TD><TD><B>Ctc</B></TD><TD><B>Gross</B></TD><TD><B>Net</B></TD><TD><B>TotalDed</B></TD></TR>
<TR>
<TD><A HREF="javascript:pick('symbol')" >Empname</A> 

</TD>
<TD></TD></TR>

</TABLE>
</BODY>
</html>
