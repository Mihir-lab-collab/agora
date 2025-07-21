<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditAttendance.aspx.vb" Inherits="admin_EditAttendance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
 <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />
<head runat="server">

    <title>Edit Attendance</title>

</head>
 <script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript">
    </script>

<script language="javascript" type="text/javascript">
function GetTime()
{
    var dt = new Date;
    var curr_hour=dt.getHours();
    if(curr_hour<10)
    {
    curr_hour="0"+curr_hour;
    }
    var curr_min=dt.getMinutes();
    //alert(curr_hour+":"+curr_min);
    var hdnTime=document.getElementById("hdnTime")
    hdnTime.value="09"+":"+"30"+":00";
}
function CompDate(adate,bdate)
{
	a = adate.split('-');
	b = bdate.split('-');
	var sDate = new Date(a[2],parseInt(getnummonth(a[1]))-1,a[0]);
	var eDate = new Date(b[2],parseInt(getnummonth(b[1]))-1,b[0]);

	if (sDate <= eDate )
	{
	
		return true; 
	}
	else
	{
	    alert("From date must be less than or equal To date !");
		return false;
	}
}

function DateCompaire()
{
 return CompDate(document.getElementById('txtdatefrom').value,document.getElementById('txtdate').value);
}

function getnummonth(strmonth)
{
    var num;
   if(strmonth=="Jan")
  {
        num="01"
        return  num;
  }
  else if(strmonth=="Feb")
  {
     num="02"
    return  num;
  }
  else if(strmonth=="Mar")
  {
    num="03"
    return  num;
  }
  else if(strmonth=="Apr")
  {
  num="04"
    return  num;
  }
  else if(strmonth=="May")
  {
  num="05"
    return  num;
  }
  else if(strmonth=="Jun")
  {
  num="06"
    return  num;
  }
  else if(strmonth=="Jul")
  {
  num="07"
    return  num;
    }
    else if(strmonth=="Aug")
  {
  num="08"
    return  num;
  }
  else if(strmonth=="Sep")
  {
  num="09"
    return  num;
  }
  else if(strmonth=="Oct")
  {
  num="10"
    return  num;
  }
  else if(strmonth=="Nov")
  {
  num="11"
    return  num;
    }
    else if(strmonth=="Dec")
  {
  num="12"
    return  num;
  }
 }
</script>
<body onload="GetTime()">
    <form id="form1" runat="server">
    <table style="vertical-align:top;width:100%" align="center" >
    <tr>
    <td align="center" colspan="2" style="background-color:#C5D5AE;"> 
      <font face="Verdana" size="2" color="#A2921E"><b>Edit Attendance</b></font>
      </td>
    </tr>
    <tr>
    <td align="right" style="background-color:#FFFFEE;"> <font face="Verdana" size="2" color="#A2921E"><b>Emp Name</b></font></td>
            <td valign="top" style="background-color:#FFFFEE;">
                <asp:Label ID="lblempname" runat="server" Font-Bold="True" Font-Names="Verdana" 
                    Font-Size="Small"></asp:Label>
                </td>
       </tr>

       <tr>
    <td align="right" style="background-color:#FFFFEE;"> <font face="Verdana" size="2" color="#A2921E"><b> Leave/Present Date From:</b></font></td>
            <td valign="top" style="background-color:#FFFFEE;">
                <asp:TextBox ID="txtdateFrom" runat="server" onclick="popupCalender('txtdateFrom')" onkeypress="return false;"></asp:TextBox></td>
       </tr>


        <tr>
    <td align="right" style="background-color:#FFFFEE;"> <font face="Verdana" size="2" color="#A2921E"><b> To:</b></font></td>
            <td valign="top" style="background-color:#FFFFEE;">
                <asp:TextBox ID="txtdate" runat="server" onclick="popupCalender('txtdate')" onkeypress="return false;"></asp:TextBox></td>
       </tr>

        <tr>
    <td align="right"  style="background-color:#FFFFEE;"> <font face="Verdana" size="2" color="#A2921E"><b>Status</b></font></td>
            <td valign="top"  style="background-color:#FFFFEE;">
                <asp:DropDownList ID="dropattstatus" runat="server">
                </asp:DropDownList></td>
       </tr>
        <tr>
    <td align="right" style="background-color:#FFFFEE;">&nbsp;</td>
            <td valign="top"  style="background-color:#FFFFEE;">
                &nbsp;</td>
       </tr>
       <tr>
       <td align="center" colspan="2" style="background-color:#C5D5AE;">
           <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return DateCompaire()"/>
       </td>
       </tr>
        </table>
        <input type="hidden" runat="server" id="hdnTime" />
    </form>
</body>
</html>
