<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empattdetails1.aspx.vb" Inherits="admin_empattdetails1" %>
<%@ Register Src="../controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Attendance details</title>
    
   


       <link rel="stylesheet" href="../css/style.css" type="text/css" />

    <script language="javascript" type="text/javascript">
//Open popup function
   function Popup(attDate)
   {
     window.open('empAttendanceDetail.aspx?strDate=' + attDate,'Report','scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=600,height=600,left=220,top=110');
     return false;
   }
//End open poup
function doAttandance(empid,strdate,strUpdate)
{
 
 var a=strdate.split(' ')
 strdate=a[1]
 a=strdate.split('-')

 var mon=getnummonth(a[1])

if (strUpdate==null)
{
 strdate=a[0] + '-' + mon+ '-'+a[2]
     window.open("EditAttendance.aspx?id=" + empid + "&currDate="+strdate,"pop",'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=400,height=300,left=220,top=110')
    //alert(empid);
    
}
else
{
     strdate=a[0] + '-' + mon+ '-'+a[2]
     window.open("EditAttendance.aspx?id=" + empid + "&currDate="+strdate +"&strUpdate="+strUpdate,"pop",'scrollbars=yes,ststus=no,toolbar=no,menubar=no,location=right,resizable=yes,width=400,height=300,left=220,top=110')
}
     
}


function getnummonth(strmonth)
{
    var num;
   if(strmonth=="1")
  {
        num="Jan"
        return  num;
  }
  else if(strmonth=="2")
  {
     num="Feb"
    return  num;
  }
  else if(strmonth=="3")
  {
    num="Mar"
    return  num;
  }
  else if(strmonth=="4")
  {
  num="Apr"
    return  num;
  }
  else if(strmonth=="5")
  {
  num="May"
    return  num;
  }
  else if(strmonth=="6")
  {
  num="Jun"
    return  num;
  }
  else if(strmonth=="7")
  {
  num="Jul"
    return  num;
    }
    else if(strmonth=="8")
  {
  num="Aug"
    return  num;
  }
  else if(strmonth=="9")
  {
  num="Sep"
    return  num;
  }
  else if(strmonth=="10")
  {
  num="Oct"
    return  num;
  }
  else if(strmonth=="11")
  {
  num="Nov"
    return  num;
    }
    else if(strmonth=="12")
  {
  num="Dec"
    return  num;
  }
 }

	</script>
</head>
<body>
<uc1:adminMenu ID="AdminMenu1" runat="server" />
    <form id="form1" runat="server">
    <table width="100%">
    <tr style="background-color: #C5D5AE">
    <td align="left" >
								<%
								    Dim strDate As Object
								    Dim nextMonth As Object
								    Dim prevMonth As Object
								    strDate = Request.QueryString("strDate")
									If Not IsDate(strDate) Or Month(strDate) = Month(Date.Today) Then
										strDate = Now()
									End If
								    nextMonth = FormatDateTime((DateAdd("m", 1, "1-" & MonthName(Month(strDate)) & "-" & Year(strDate))), 2)
								    prevMonth = FormatDateTime((DateAdd("m", -1, "1-" & MonthName(Month(strDate)) & "-" & Year(strDate))), 2)
								%>
								<div style="float:left;padding-left:10px">
								<a href="empattdetails.aspx?strDate=<% =prevMonth%>" style="text-decoration: none"><font
									color="#A2921E"><b><<</b></font></a>&nbsp; <font color="#A2921E"></font><font face="Verdana"
										size="2" color="#A2921E"><b>
										
											<%=Left(MonthName(Month(strDate)), 3) & " " & Year(strDate)%>
											
										</b></font><font color="#A2921E"></font><a href="empattdetails.aspx?strDate=<%=nextMonth%>"
											style="text-decoration: none"><font color="#A2921E"><b>&nbsp;>></b></font></a><font color="#A2921E"></font>
											</div>
											<div style="float:left;padding-left:20px"><a style="text-decoration:none" href="javascript:void(0);" onclick="return Popup('<%=strDate %>');"><font color="#A2921E"></font><font face="Verdana"
										size="2" color="#A2921E"><b>Attendance Report</b></font></a></div>
											</td>
     </tr>
     <tr>
     <td>
    
         <asp:GridView ID="grdatt" runat="server"  AutoGenerateColumns="False"    Width="3000px" CssClass="manage">
       
             <Columns>
                 <asp:TemplateField HeaderText="EmpID" >
                 <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("EmpID") %>' style="white-space:nowrap;font-family:Verdana;font-size:12px"></asp:Label>
                 </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Name" HeaderStyle-Width="5%">
                      <ItemTemplate>
                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                     </ItemTemplate>
               <ItemStyle CssClass="widthname" Wrap="false" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 1" >
                      <ItemTemplate >
                         <asp:Label ID="Label3" runat="server" Text='<%# Bind("aa1") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                          <asp:CheckBox ID="chk3" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                      <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 2">
                     <ItemTemplate>
                         <asp:Label ID="Label4" runat="server" Text='<%# Bind("aa2") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                          <asp:CheckBox ID="chk4" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                  
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 3">
                     <ItemTemplate>
                         <asp:Label ID="Label5" runat="server" Text='<%# Bind("aa3") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk5" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 4">
                       <ItemTemplate>
                                   <asp:Label ID="Label6" runat="server" Text='<%# Bind("aa4") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                                   <asp:CheckBox ID="chk6" runat="server" Text=""  ToolTip='<%# Bind("empname") %>'/>
                       </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 5">
                       <ItemTemplate>
                         <asp:Label ID="Label7" runat="server" Text='<%# Bind("aa5") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk7" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 6">
                       <ItemTemplate>
                         <asp:Label ID="Label8" runat="server" Text='<%# Bind("aa6") %>'    style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk8" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 7">
                       <ItemTemplate>
                         <asp:Label ID="Label9" runat="server" Text='<%# Bind("aa7") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"> </asp:Label>
                         <asp:CheckBox ID="chk9" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 8">
                       <ItemTemplate>
                         <asp:Label ID="Label10" runat="server" Text='<%# Bind("aa8") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk10" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 9">
                       <ItemTemplate>
                         <asp:Label ID="Label11" runat="server" Text='<%# Bind("aa9") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk11" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 10">
                       <ItemTemplate>
                         <asp:Label ID="Label12" runat="server" Text='<%# Bind("aa10") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk12" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 11">
                       <ItemTemplate>
                         <asp:Label ID="Label13" runat="server" Text='<%# Bind("aa11") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk13" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 12">
                       <ItemTemplate>
                         <asp:Label ID="Label14" runat="server" Text='<%# Bind("aa12") %>'  ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk14" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 13">
                       <ItemTemplate>
                         <asp:Label ID="Label15" runat="server" Text='<%# Bind("aa13") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk15" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 14">
                       <ItemTemplate>
                         <asp:Label ID="Label16" runat="server" Text='<%# Bind("aa14") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk16" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 15">
                       <ItemTemplate>
                         <asp:Label ID="Label17" runat="server" Text='<%# Bind("aa15") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk17" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
               <%--  <asp:TemplateField HeaderText="EmpID" >
                 <ItemTemplate>
                <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                 </ItemTemplate>
                 </asp:TemplateField>--%>
                 <asp:TemplateField HeaderText="Date 16">
                       <ItemTemplate>
                         <asp:Label ID="Label18" runat="server" Text='<%# Bind("aa16") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk18" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 17">
                       <ItemTemplate>
                         <asp:Label ID="Label19" runat="server" Text='<%# Bind("aa17") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk19" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 18">
                       <ItemTemplate>
                         <asp:Label ID="Label20" runat="server" Text='<%# Bind("aa18") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk20" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 19">
                       <ItemTemplate>
                         <asp:Label ID="Label21" runat="server" Text='<%# Bind("aa19") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk21" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 20">
                       <ItemTemplate>
                         <asp:Label ID="Label22" runat="server" Text='<%# Bind("aa20") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk22" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 21">
                       <ItemTemplate>
                         <asp:Label ID="Label23" runat="server" Text='<%# Bind("aa21") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk23" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 22">
                       <ItemTemplate>
                         <asp:Label ID="Label24" runat="server" Text='<%# Bind("aa22") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk24" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 23">
                       <ItemTemplate>
                         <asp:Label ID="Label25" runat="server" Text='<%# Bind("aa23") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk25" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 24">
                       <ItemTemplate>
                         <asp:Label ID="Label26" runat="server" Text='<%# Bind("aa24") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk26" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 25">
                       <ItemTemplate>
                         <asp:Label ID="Label27" runat="server" Text='<%# Bind("aa25") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk27" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 26">
                       <ItemTemplate>
                         <asp:Label ID="Label28" runat="server" Text='<%# Bind("aa26") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk28" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 27">
                       <ItemTemplate>
                         <asp:Label ID="Label29" runat="server" Text='<%# Bind("aa27") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk29" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 28">
                       <ItemTemplate>
                         <asp:Label ID="Label30" runat="server" Text='<%# Bind("aa28") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk30" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 29">
                       <ItemTemplate>
                         <asp:Label ID="Label31" runat="server" Text='<%# Bind("aa29") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk31" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 30">
                       <ItemTemplate>
                         <asp:Label ID="Label32" runat="server" Text='<%# Bind("aa30") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk32" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Date 31">
                       <ItemTemplate>
                         <asp:Label ID="Label33" runat="server" Text='<%# Bind("aa31") %>' ToolTip='<%# Bind("empname") %>' style="white-space:nowrap;"></asp:Label>
                         <asp:CheckBox ID="chk33" runat="server" Text="" ToolTip='<%# Bind("empname") %>'/>
                     </ItemTemplate>
                     <ItemStyle CssClass="grdattwidth" />
                 </asp:TemplateField>
                                 
                 
             </Columns>
             <HeaderStyle CssClass="hrdstyl" />
        </asp:GridView>

     </td>
     </tr>
   
    </table>
    </form>
</body>
</html>
