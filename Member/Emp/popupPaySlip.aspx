<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title></title>
</head>
<body onload="window.print()">

	<script runat="server">
		Dim dmonth, ddate
		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			ddate = Session("currentslipdate")
			dmonth = Month(ddate)
			lblname.Text = Session("pempname") & " ( " & Session("dynoempidsession") & " )"
			lblbasicsal.Text = Session("pbasic")
			lblhra.Text = Session("phra")
			lblta.Text = Session("pta")
			lblmedical.Text = Session("pmedical")
			lbllta.Text = Session("plta")
			lblfoodallow.Text = Session("pfoodallow")
			lblspecialallow.Text = Session("pspecialallow")
			lbladvancea.Text = Session("padvancea")
			lblbonus.Text = Session("plblbonus")
			lblproffesiontax.Text = Session("pprofessiontax")
			lblincometax.Text = Session("ppf")
			lblepf.Text = Session("pepf")
			lblloan.Text = Session("ploan")
			lbltotaldeduction.Text = Session("ptotaldeduction")
			lblnetpayable.Text = Session("pnetpayable")
			lblgrosssal.Text = Session("pgrosssal")
			lblemployedsince.Text = Session("pemployedsince")
			lbldesignation.Text = Session("pdesignation")
			lblloanrepayment.Text = Session("ploanpay")
			lbldayspresent.Text = Session("pdayspresent")
			lbldaysabsent.Text = Session("pdaysabsent")
			lblnoofdays.Text = Session("pnoofdays")
			lblab.Text = Session("pab")
			lblleavededuction.Text = Session("pleavededuction")
			lblothers.Text = Session("pother")
			lbladvanceab.Text = Session("padvanceab")
		End Sub
	</script>

	<table id="tblslip" align="center" style="vertical-align: top" width="100%" border="1"
		cellpadding="2" cellspacing="0">
		<tr>
			<td width="25%" colspan="6" valign="top">
				<p>
					<font face="Verdana" size="2">
						<img src="../images/dynologo.gif" alt="" align="right" />
						<b>INTELGAIN TECHNOLOGIES PVT. LTD.
							<br />
							B-203, Sanpada Station Complex, Navi Mumbai 400 705 </b></font>
				</p>
				<table width="100%" border="0">
					<tr>
						<td bgcolor="#000000">
							<font face="Verdana" size="2"><b><font color="#FFFFFF">SALARY SLIP FOR THE MONTH OF
								&nbsp; </font></b></font>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
							&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <font face="Verdana"
								size="2" color="#FFFFFF"><b>
									<%= MonthName(dMonth)&" "&Year(dDate) %>
								</b></font>
						</td>
					</tr>
				</table>
				<b></b><font face="Verdana" size="2"></font><font face="Verdana" size="2"><b>Employee
					Details &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
					&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
					&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
					&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Date :
					<%=DateTime.Now.ToString("dd-MMM-yyyy")%>
				</b></font>
				<table width="100%">
					<tr>
						<td height="12">
							<table align="left" border="1" width="50%" cellspacing="0" cellpadding="0">
								<tr>
									<td width="50%">
										<font face="Verdana" size="2">Name</font></td>
									<td colspan="2">
										<font face="Verdana" size="2">
											<asp:Label ID="lblname" runat="server"></asp:Label></font></td>
								</tr>
								<tr>
									<td width="50%">
										<font face="Verdana" size="2">Designation </font>
									</td>
									<td colspan="2">
										<font face="Verdana" size="2">
											<asp:Label ID="lbldesignation" runat="server"></asp:Label></font></td>
								</tr>
								<tr>
									<td width="50%">
										<font face="Verdana" size="2">Employed Since</font></td>
									<td colspan="2">
										<font face="Verdana" size="2">
											<asp:Label ID="lblemployedsince" runat="server"></asp:Label>
										</font>
									</td>
								</tr>
							</table>
							<table align="right" border="1" width="30%" cellspacing="0" cellpadding="0">
								<tr>
									<td>
										<font face="Verdana" size="2">No Of Days</font></td>
									<td align="right">
										<font face="Verdana" size="2">
											<asp:Label ID="lblnoofdays" runat="server">0</asp:Label></font></td>
								</tr>
								<tr>
									<td width="25%">
										<font face="Verdana" size="2">Days Present</font></td>
									<td width="25%" align="right">
										<font face="Verdana" size="2">
											<asp:Label ID="lbldayspresent" runat="server">0</asp:Label>
										</font>
									</td>
								</tr>
								<tr>
									<td width="25%">
										<font face="Verdana" size="2">Days Absent </font>
									</td>
									<td width="25%" align="right">
										<font face="Verdana" size="2">
											<asp:Label ID="lbldaysabsent" runat="server">0</asp:Label></font></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<b>
					<br />
					<font face="Verdana" size="2"><u><i>Break up of Salary</i></u></font></b></td>
		</tr>
		<tr>
			<td align="center" style="background-color: #000000; width: 30%">
				<font face="Verdana" size="2" color="#ffffff"><b>Receivable</b></font></td>
			<td align="center" style="background-color: #000000; width: 25%">
				<font face="Verdana" size="2" color="#ffffff"><b>Rs.</b></font></td>
			<td align="center" style="background-color: #000000; width: 25%">
				<font face="verdana" size="2" color="#ffffff"><b>Deductions</b></font></td>
			<td align="center" style="background-color: #000000; width: 25%">
				<font face="Verdana" size="2" color="#ffffff"><b>Rs.</b></font></td>
			<td align="center" style="background-color: #000000; width: 25%">
				<font face="Verdana" size="2" color="#ffffff"><b>Addition</b></font></td>
			<td align="center" style="background-color: #000000; width: 25%">
				<font face="verdana" size="2" color="#ffffff"><b>Rs.</b></font></td>
		</tr>
		<tr>
			<td>
				<font face="Verdana" size="2">Basic Salary</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblbasicsal" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Advance</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lbladvancea" runat="server">0</asp:Label></font></td>
			<td>
				<font face="Verdana" size="2">(A-B)</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblab" runat="server">0</asp:Label></font></td>
		</tr>
		<tr>
			<td>
				<font face="Verdana" size="2">HRA</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblhra" runat="server">0</asp:Label></font></td>
			<td>
				<font face="Verdana" size="2">Income Tax</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblincometax" runat="server">0</asp:Label></font></td>
			<td>
				<font face="Verdana" size="2">Bonus</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblbonus" runat="server">0</asp:Label></font></td>
		</tr>
		<tr>
			<td>
				<font face="Verdana" size="2">TA</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblta" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Profession Tax</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblproffesiontax" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Advance</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lbladvanceab" runat="server">0</asp:Label>
				</font>
			</td>
		</tr>
		<tr>
			<td>
				<font face="Verdana" size="2">Medical</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblmedical" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">EPF</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblepf" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Loan</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblloan" runat="server">0</asp:Label>
				</font>
			</td>
		</tr>
		<tr>
			<td>
				<font face="Verdana" size="2">LTA</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lbllta" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Loan Payment</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblloanrepayment" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Paid Leaves</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblpaidleaves" runat="server">0</asp:Label>
				</font>
			</td>
		</tr>
		<tr>
			<td>
				<font face="Verdana" size="2">Food Allow.</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblfoodallow" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Leave Deduction</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblleavededuction" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Others</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblothersab" runat="server">0</asp:Label>
				</font>
			</td>
		</tr>
		<tr>
			<td>
				<font face="Verdana" size="2">Special Allow.</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblspecialallow" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				<font face="Verdana" size="2">Others</font></td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblothers" runat="server">0</asp:Label>
				</font>
			</td>
			<td>
				-</td>
			<td align="right">
				<font face="Verdana" size="2">
					<asp:Label ID="lblblank" runat="server">-</asp:Label>
				</font>
			</td>
		</tr>
		<tr>
			<td style="background-color: #000000; width: 30%" align="center" nowrap="nowrap"="nowrap="nowrap"">
				<font face="Verdana" size="2" color="#ffffff"><b>Gross Salary (A) </b></font>
			</td>
			<td style="background-color: #000000; width: 10%" nowrap="nowrap"="nowrap="nowrap"" align="center">
				<font face="Verdana" size="2" color="#ffffff"><b>
					<asp:Label ID="lblgrosssal" runat="server">0</asp:Label></b></font></td>
			<td nowrap="nowrap"="nowrap="nowrap"" align="center" style="background-color: #000000; width: 25%">
				<font face="verdana" size="2" color="#ffffff"><b>Total Deduction (B)</b></font></td>
			<td nowrap="nowrap"="nowrap="nowrap"" align="center" style="background-color: #000000; width: 10%">
				<font face="Verdana" size="2" color="#ffffff"><b>
					<asp:Label ID="lbltotaldeduction" runat="server">0</asp:Label></b></font></td>
			<td style="background-color: #000000; width: 35%" nowrap="nowrap"="nowrap="nowrap"" align="center">
				<font face="Verdana" size="2" color="#ffffff"><b>Net Payable</b></font></td>
			<td style="background-color: #000000; width: 10%" nowrap="nowrap"="nowrap="nowrap"" align="center">
				<font face="verdana" size="2" color="#ffffff"><b>
					<asp:Label ID="lblnetpayable" runat="server">0</asp:Label></b></font></td>
		</tr>
		<tr>
			<td colspan="6">
				<font face="Verdana" size="2"><b>Note : </b>(This is a Computer Generated Statement
					and does not require any signature)</font>
			</td>
		</tr>
	</table>
</body>
</html>
