<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>

<script runat="server">
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        gf.checkEmpLogin()
    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<meta http-equiv="Content-Language" content="en-us">
		<title>Intelgain Technologies Pvt Ltd</title>
	    <style>
<!--
 li.MsoNormal
	{mso-style-parent:"";
	margin-bottom:.0001pt;
	punctuation-wrap:simple;
	text-autospace:none;
	font-size:10.0pt;
	font-family:"Times New Roman";
	margin-left:0in; margin-right:0in; margin-top:0in}
-->
        </style>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div align="center">
              <center>
			<table id="Table3" cellSpacing="0" cellPadding="2" width="100%" valign=top
				border="0" style="border-collapse: collapse" bordercolor="#111111">
				<tr>
					<td><EMPHEADER:EMPHEADER id="Empheader" runat="server">
                    </EMPHEADER:EMPHEADER>
                    <font face="Verdana" color="#A2921E" size="2"><BR>
						</font>
						<uc1:empMenuBar id="EmpMenuBar" runat="server">
                    </uc1:empMenuBar>
                    <BR></td>
				</tr>
				<tr>
					<td bgcolor="#C5D5AE">
                    <font face="Verdana" color="#a2921e" size="2"><b>Office 
                    Timings</b></font></td>
				</tr>
				<tr>
					<td valign="top">
                    <p class="MsoNormal"><b>
                    <span style="font-family: Verdana; color: #A2921E">
                    <font size="2">Weekdays: 9:30 AM TO 6:30 PM<br>
                    Saturdays: 9:30 AM TO 4:30 PM<br>
                    Weekly Off: All Sundays + 2<sup>nd</sup> and 4<sup>th</sup> 
                    Saturdays</font></span></b><BR><BR></td>
				</tr>
				<tr>
					<td bgcolor="#C5D5AE">
                    <font face="Verdana" color="#a2921e" size="2"><b>Leave Rules</b></font></td>
				</tr>
				<tr>
					<td align="justify" valign="top">
                    <p class="MsoNormal" style="text-indent: -9.0pt; margin-left: 9.0pt">
                    <b><font size="2">
                    <span style="font-family: Verdana; color: #A2921E">I.</span></font><span style="font-style: normal; font-variant: normal; font-weight: normal; font-family: Verdana; color: #A2921E"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </font></span>
                    <span style="font-size: 12.0pt; font-family: Verdana; color: #A2921E">
                    <font size="2">
                    <span style="font-family: Verdana; color: #A2921E">Casual 
                    leave: 7 days in a calendar year</span></font></span></b></p>
                    <ul style="margin-top: 0in; margin-bottom: 0in" type="disc">
                      <li class="MsoNormal" style="text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      An employee shall be entitled to 7 days casual leave in a 
                      calendar year, subject to the condition that an 
                      application for availing the same is made in advance. 
                      Failure to seek prior permission shall result in 
                      considering the same as unauthorized absence (without pay) 
                      for such day / days. However on sufficient reasons being 
                      given by the employee for not having sought prior 
                      permission leave applied for may be sanctioned by the 
                      competent authority subject to admissibility.</span></font></li>
                      <li class="MsoNormal" style="text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Casual leave should not normally be granted for more than 
                      2 days at any one time, except under special 
                      circumstances, which is to be approved by director / 
                      Competent Authority.</span></font></li>
                      <li class="MsoNormal" style="text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Sundays / public holidays / weekly off / compensatory off 
                      are&nbsp; permitted&nbsp; to be prefixed&nbsp; or suffixed to casual 
                      leave and Sundays /&nbsp; holidays&nbsp; falling during a period of 
                      casual leave&nbsp; are not counted as part of casual leave
                      </span></font></li>
                      <li class="MsoNormal" style="text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Casual leave cannot be combined with any other kind of 
                      leave.</span></font></li>
                      <li class="MsoNormal" style="text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      An employee joining middle of a year may be granted casual 
                      leave proportionately.</span></font></li>
                      <li class="MsoNormal" style="text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Unutilized casual leave will lapse at end of the year.</span></font></li>
                    </ul>
                    <p class="MsoNormal" style="text-indent: -9.0pt; margin-left: 9.0pt">
                    <b><font size="2">
                    <span style="font-family: Verdana; color: #A2921E">II.</span></font><span style="font-style: normal; font-variant: normal; font-weight: normal; font-family: Verdana; color: #A2921E"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </font></span>
                    <span style="font-size: 12.0pt; font-family: Verdana; color: #A2921E">
                    <font size="2">
                    <span style="font-family: Verdana; color: #A2921E">Sick 
                    leave: 5 days per year with full pay</span></font></span></b><span style="font-family: Verdana; color: #A2921E"><font size="2">&nbsp;</font></span></p>
                    <ul style="margin-top: 0in; margin-bottom: 0in" type="disc">
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      An employee shall be entitled to 5 days sick leave each 
                      calendar year with full pay. &nbsp;</span></font></li>
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Sick leaves for more than 2 days will be granted only on 
                      production of medical certificate from doctor/authorized 
                      medical officer.</span></font></li>
                      <li class="MsoNormal" style="text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Unutilized sick leave will lapse at end of the year.</span></font></li>
                    </ul>
                    <p class="MsoNormal"><b>
                    <span style="font-family: Verdana; color: #A2921E">
                    <font size="2">&nbsp;</font></span><font size="2"><span style="font-family: Verdana; color: #A2921E">III.</span></font><span style="font-style: normal; font-variant: normal; font-weight: normal; font-family: Verdana; color: #A2921E"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </font></span>
                    <span style="font-size: 12.0pt; font-family: Verdana; color: #A2921E">
                    <font size="2">
                    <span style="font-family: Verdana; color: #A2921E">Earned 
                    Leave: 20 days per year from second year of service.</span></font></span></b></p>
                    <ul>
                      <li>
                      <p class="MsoNormal">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">An Employee shall be entitled 20 days 
                      earned leave per year from second year of employment. EL 
                      may be accumulated upto 90 days in the entire service 
                      period excess, if any, shall lapse.</font></span></li>
                      <li>
                      <p class="MsoNormal">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">An employee may be granted earned leave not 
                      more than 20 days at a time.</font></span></li>
                      <li>
                      <p class="MsoNormal">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">Earned leave may be allowed to be combined 
                      with any other leave except casual leave.</font></span></li>
                      <li>
                      <p class="MsoNormal">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">Application for E.L shall be submitted at 
                      least seven days in advance and shall not be availed, 
                      until &amp; unless it is sanctioned.</font></span></li>
                      <li>
                      <p class="MsoNormal">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">Leave applied for may be refused by the 
                      leave sanctioning authority, wholly or in part thereof, 
                      looking to the exigencies of work &amp; no compensation would 
                      be admissible in such eventuality.</font></span></li>
                      <li>
                      <p class="MsoNormal">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">An employee on E.L. may be recalled from 
                      leave in exigencies of work, even prior to the expiry of 
                      leave, without entitling the employee to any kind of 
                      compensation.</font></span></li>
                    </ul>
                    <p class="MsoNormal" style="text-indent: -9.0pt; margin-left: 9.0pt">
                    <b><font size="2">
                    <span style="font-family: Verdana; color: #A2921E">IV.</span></font><span style="font-style: normal; font-variant: normal; font-weight: normal; font-family: Verdana; color: #A2921E"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </font></span>
                    <span style="font-size: 12.0pt; font-family: Verdana; color: #A2921E">
                    <font size="2">
                    <span style="font-family: Verdana; color: #A2921E">
                    Extraordinary leave:&nbsp; leave without pay.</span></font></span></b></p>
                    <p class="MsoNormal" style="text-align: justify; margin-left: .5in">
                    <span style="font-family: Verdana; color: #A2921E">
                    <font size="2">Under very special circumstances, when no 
                    other leave is admissible extraordinary leave may be 
                    sanctioned subject to the condition that:</font></span></p>
                    <ul>
                      <li>
                      <p class="MsoNormal" style="text-align: justify; margin-left: .5in">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">No leave salary shall be for this period.</font></span></li>
                      <li>
                      <p class="MsoNormal" style="text-align: justify; margin-left: .5in">
                      <span style="font-family: Verdana; color: #A2921E">
                      <font size="2">Extra-ordinary leave upto one month on any 
                      one occasion shall be sanctioned by the leave sanctioning 
                      authority, beyond which will require the approval of 
                      director.</font></span></li>
                    </ul>
                    <p class="MsoNormal" style="text-indent: -9.0pt; margin-left: 9.0pt">
                    <b><font size="2">
                    <span style="font-family: Verdana; color: #A2921E">V.</span></font><span style="font-style: normal; font-variant: normal; font-weight: normal; font-family: Verdana; color: #A2921E"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </font></span>
                    <span style="font-size: 12.0pt; font-family: Verdana; color: #A2921E">
                    <font size="2">
                    <span style="font-family: Verdana; color: #A2921E">General 
                    procedure for availing leave:</span></font></span></b></p>
                    <ul style="margin-top: 0in; margin-bottom: 0in" type="disc">
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      No employee shall be entitling for any leave during first 
                      two months of employment.</span></font></li>
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Leave of any kind cannot be claimed as matter of right.</span></font></li>
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      An employee who desires to obtain leave of absence other 
                      than casual Leave shall apply in writing to the 
                      departmental head. Generally such application for leave 
                      shall be made not less than 7 days before the date from 
                      which the leave is to commence except in urgent cases or 
                      unforeseen circumstances, including illness when it is not 
                      possible to do so.</span></font></li>
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      An employee desiring extension of leave should send the 
                      application sufficiently in advance.</span></font></li>
<li class="MsoNormal" style="color: black; text-align: justify">
          <font color="#A2921E"><span style="font-family: Verdana">Any leave 
          taken on a saturday and the following monday will include the sunday 
          as a leave as well. This is liable for all types of leaves</span></font></li>
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      An employee who overstays his leave (except under 
                      circumstances beyond his control) without proper&nbsp; 
                      permission shall not be paid for the period he overstays 
                      and shall further render&nbsp; himself liable to such 
                      disciplinary action as the competent authority may think 
                      fit to impose.</span></font></li>
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      An employee shall before preceding on leave intimate to 
                      the sanctioning authority his address while on leave.</span></font></li>
                      <li class="MsoNormal" style="color: black; text-align: justify">
                      <font color="#A2921E"><span style="font-family: Verdana">
                      Board of Directors has right to change any above rule 
                      without any prior notice.</span></font></li>
                    </ul>
&nbsp;</td>
				</tr>
			</table>
		      </center>
            </div>
		</form>
	</body>
</html>