<%@ Page Language="VB" AutoEventWireup="false" CodeFile="codeRevDetails.aspx.vb" Inherits="emp_codeRevDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title> 
     <style type="text/css">

div.sample_attach1
{
  width: 400px;
  border: 1px solid black;
  font-family: Arial;
  background: #C5D5AE;
  padding: 0px 5px;
  font-weight: 900;
  color: RED;
}
input.btn{
   
   font-family: Verdana;
   font-size:84%;
   font-weight:bold;
   color: #A2921E;
   border: 1px solid black;
   border:1px solid;
   background:#C5D5AE;
   border-top-color:#696;
   border-left-color:#696;
   border-right-color:#363;
   border-bottom-color:#363;
   
   }

</style>

    <script language="javascript" type="text/javascript" src="../includes/dropdownhh.js"></script>
</head>


<body>
    <form id="form1" runat="server">
    <table width="100%" runat="server" align="center" >
        <tr style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE">
            <td axis="center">Rating Details</td>
        </tr>
        <tr>
            <td>
                <table Border="4">
                    <asp:DataGrid runat="server"  ID="dgrddetails" BorderColor="Black" Font-Size="10pt" Font-Name="Verdana"
			                         BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"	HeaderStyle-Font-Size="10pt" HeaderStyle-BackColor="LightGray" CellPadding="2"
			                          Width="100%"  >
                     <itemstyle ForeColor="#000000" BackColor="#FFFFEE" VerticalAlign="Top" Width="100%"></itemstyle>
                      <headerstyle Font-Bold="True" ForeColor="#a2921e" BackColor="#C5D5AE" Width="100%"></headerstyle>
                        <Columns>
                        <asp:TemplateColumn>
                          <ItemTemplate>
                          <tr border="1">
                              <td width="100%" border="1"   align="left"   valign="top"> <font style="font-size: 10pt; color: Black; font-family: Arial">
                                  <table cellpadding="0" cellspacing="0" width="100%" valign="Top" border="1">
                                   <tr>
										<td  style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Date</b><font></td>
										
										<td style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>Comment</b></font></td>
										<td style="background:#C5D5AE;width:7%"  align="center"   valign="top"><b> <font style="font-size: 10pt; color: #A2921E; font-family: Arial"><b>FeedBack</b></font></td>
										<td></td>
								   </tr>
									<tr>
                                        <td width="15%" nowrap="nowrap"><%#Container.DataItem("dd")%></td>
                                        <td align="left" style="Display:none"><%#Container.DataItem("coderevid")%></td>
                                        <td width="35%" align="left"><%#Container.DataItem("comments")%></td>
										<td width="40%" align="left">
											<asp:TextBox TextMode="MultiLine" runat="server" ID="txtFeedbakHistory"  Columns="50" Rows ="10" style="border-style:solid; border-width:0; font-family: Verdana;  font-size: 10pt; color:#808080" readonly="true" ></asp:TextBox></font> 
										</td>
                                         <td >
					                                <div id="src_floppy" >
                                                               <img src="../images/givefd.gif" alt="" />
                                                     </div>
										</td>
									</tr>
									
									 <tr id="trajax" runat="server">
				  <td>
					
                                                            <div class="sample_attach1" id="src_floppy_child">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td style="width: 250px">
                                                                            <b>Give Feedback:new</b></td>
                                                                        <td width="3px">
                                                                            <img src="../images/close.gif" style="cursor: pointer;" onclick="javascript:document.getElementById('src_floppy_child').style.visibility = 'hidden'" />
                                                                        </td>
                                                                    </tr>
                                                                   <tr>
                                                                        <td style="width: 250px" colspan="2">
                                                                            <asp:TextBox TextMode="MultiLine" runat="server" ID="txtFeedbackComment666"  Columns="45" Rows ="10" ></asp:TextBox>
                                                                            <input type="button" value="Submit" onclick="javascript:call_Ajax()" class="btn" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <script type="text/javascript">
                                                    at_attach("src_floppy", "src_floppy_child", "click", "x", "pointer");
                                                            </script>
											</td></tr></table>
				
				</td>
				</tr>
                                  
                                  
                                  </table>
                              
                              
                              
                              </td></font>
                          
                          
                          
                          </tr>
                          
                          
                          
                          </ItemTemplate>
                        
                        
                        </asp:TemplateColumn>
                          
                        </Columns>
                    </asp:DataGrid></table>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        
    </table>
        
    
    </form>
</body>
</html>
<script>

function ajaxFunction()
{  
  var xmlHttp;
  try
    {  
	  xmlHttp=new XMLHttpRequest();
	}
  catch (e)
    {    // Internet Explorer
	   try
      	{   
		   xmlHttp=new ActiveXObject("Msxml2.XMLHTTP"); 
		}
    	catch (e)
      	{    
			  try
        		{ 
					xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
				}
     			catch (e)
        		{   
				 	alert("Your browser does not support AJAX!");   
					return false;
				}
	 	}  
	}
return xmlHttp;
}
function call_Ajax()
{  
	var xmlHttp = ajaxFunction() ;
    var Val = document.getElementById("dgrddetails_ctl02_txtFeedbackComment666").value;
xmlHttp.open("GET","feedback.aspx?empID=<%=Session("dynoEmpIdSession")%>&ID="+Val+"&revId=<%=request.querystring("revId")%>&no1="+Math.random(),true);

    xmlHttp.onreadystatechange=function()
      {
      if(xmlHttp.readyState==4)
        {
			
         var str =  xmlHttp.responseText;
        	if (str != "") 
			{
				
				if(str=="true")
				{
					document.getElementById('src_floppy_child').style.visibility = 'hidden';
					window.location.href = window.location.href ;
				}
				else
				{
					alert('Enter FeedBack');
				}
			}
		}
      }

    xmlHttp.send(null);  
}
</script>
