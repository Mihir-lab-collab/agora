<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Member_Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error Page</title>
   <style>

body, html {
  height: 100%;
  margin: 0;
}

.bg {
  /* The image used */
  background-image: url(Images/ErrorImages/Error_pic.jpg);

  /* Full height */
  width:100%; height:50%;
  object-fit: cover; 

  /* Center and scale the image nicely */
  
  background-position: center;
  background-repeat: no-repeat;
  
}
 .imgg{
    
    min-width:100%;
    height:400px;
    width:auto;
    position:center;
    top:-100%; bottom:-100%;
    left:-100%; right:-100%;
    margin:auto;
}


.column:hover{ background: url(Images/ErrorImages/selected.gif) no-repeat scroll center bottom transparent;}
   
.column {border-right: 0px solid #DDDDDD;width:350px; margin-left:50%}
.column h3 {color: #333333; font-family: 'Lucida Grande','Lucida Sans Unicode',Tahoma,Arial,san-serif;font-size:1.4em; text-align: center;}
.column h3 a {color: #323232;cursor: pointer;text-decoration: none;}

.swtab {background: url(Images/rp_error.png) no-repeat 123px center transparent;
	display: block;
    height: 50px;
    padding-left: 60px;
    padding-top: 30px;

}
.report a { background:url(Images/report.png) no-repeat 80px center;
display: block;
    height: 50px;
    padding-left: 60px;
    padding-top: 30px;

}
.report a:hover, .swtab:hover{ color:#878787;}
</style>
</head>
<body>
    <div><img class="imgg" src="Images/Error_pic.jpg" alt=""></div>
    <form id="form1" runat="server">
        <div style=" display:block"  align="center">
            <asp:Label runat="server" ID="lblError"></asp:Label>
        </div>
        <div style="width:70%; display:block"  align="center">
               <table align="center"><tr><td>
           
           	<div class="column selected">
                   <h3 class="search"><asp:LinkButton id="btnBack" rel="search-area"  OnClientClick="JavaScript:window.history.back(1);return false;" class="swtab" runat="server">Back</asp:LinkButton></h3>
            	
            </div>
               </td>
            

                                     </tr></table></div>
    </form>
</body>
</html>
