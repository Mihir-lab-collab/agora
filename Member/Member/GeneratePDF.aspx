<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GeneratePDF.aspx.cs" Inherits="Member_GeneratePDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript">
        function closeWindow() {
            Window.close();
        }
    </script>

</head>
<body >
     
<div style="width:1000px; margin:0 auto;">
   <table style="line-height:15px;" width="100%">
      <thead>
         <tr>
            <td width="50%" valign="bottom">logo</td>
            <td width="50%" align="right"><strong>Tele :</strong>  91 22 62596100/06<br /><strong>Email :</strong> contactus@intelegain.com<br /><strong>Website :</strong> www.intelegain.com </td>
         </tr>
      </thead>
   </table>
   <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse;" width="100%">
      <tbody>
         <tr>
            <td  style="line-height:20px; align:left;border-right:#9d9d9d 1px solid;" ><strong>{CustName}<br />{CustCompany}<br />{CustAddress}<br />{CustEmail}</strong></td>
           <td></td>
            <td align="right" valign="top">
            <table width="100%" border="1" cellpadding="2" cellspacing="0" frame="box" style="border-collapse:collapse; align:right;">
                  <tbody>
                     <tr align="right">
                        <td align="left" colspan="2" style="font-size:15px;border-bottom:#000 1px solid;"><b>INVOICE</b></td>
                     </tr>
                     <tr>
                        <td>Invoice No:</td>
                        <td>{InvoiceNo}</td>
                     </tr>
                     <tr>
                        <td>Invoice Date:</td>
                        <td>{InvoiceDate}</td>
                     </tr>
                  </tbody>
               </table>
            </td>
         </tr>
      </tbody>
   </table>
   <table border="1" bordercolor="#000" cellpadding="2" cellspacing="0" style="border-collapse:collapse;" width="100%">
      <thead style="border:none;">
         <tr valign="top">
            <th align="left"><b>Sr. No.</b></th>
            <th align="left" colspan="3"><b>Product / Service Description</b></th>
            <th><b>Price</b></th>
            <th align="right"><b>Amount</b></th>
         </tr>
      </thead>
      <tbody>
      <tr valign="top">
      <td colspan="6" valign="top">
          <table valign="top" width="100%" border="0" cellpadding="0" cellspacing="0" style="border-bottom:none;">
              <tbody>
         <Repeater>
             <tr valign="top"> 
             <td colspan="2">{SrNo}</td> 
             <td align="left" colspan="5">{Description}</td> 
             <td align="right" colspan="2">{Rate}</td> 
             <td align="right" colspan="2">{Price}</td> 

             </tr>

         </Repeater>
          </tbody>
 </table>
         </td>
         </tr>
         <tr align="right">
            <td colspan="5">Current Invoice Total</td>
            <td>{CurSymbol}{TotalInvoice}</td>
         </tr>
         <tr align="right">
            <td colspan="5">{ServiceTaxTitle}</td>
            <td>{CurSymbol}{Tax}</td>
         </tr>
         <tr align="right">
            <td colspan="5"><strong>Total Due </strong></td>
            <td><strong>{CurSymbol}{GrandTotal}</strong></td>
         </tr>
         <tr align="left">
            <td colspan="6"><strong>{CurSymbol}{CostinWords} </strong></td>
         </tr>
      </tbody>
   </table>
    <table>
        <tr>
           <%--<td><strong>{TDSCheck}</strong></td>--%>
           <td style="border:#000 1px solid;border-radius:2px;" width="100%">{TDSCheck}</td>
       </tr>
    </table>
   <table border="0" cellpadding="0" cellspacing="0" width="100%">
      <tbody>
         <tr><td>PAN No. ABC12345<br />Service Tax No. 12345222</td>
         </tr>
      </tbody>
   </table>
   <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse;margin-bottom:5px;" width="100%">
      <thead>
         <tr>
            <th align="left">Courier cheque at: </th>
         </tr>
      </thead>
      <tbody>
         <tr>
            <td>{CompanyAddress}</td>
         </tr>
      </tbody>
   </table>
   <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse;margin:0;" width="100%">
      <tbody>
         <tr>
            <td valign="top" align="left"><b>Payment Options</b><br />
               <table border="1" width="100%" cellspacing="0" cellpadding="10">
                  <tbody>
                     <tr>
                        <td style="border:#000 1px solid;border-radius:2px;" width="100%">
                            <b>Cheque: </b>Intelegain Technologies Pvt Ltd <br />
                            <b>Bank Wire Transfer: </b>Beneficiary Account Name: Intelegain Technologies Pvt. Ltd. 
                            <br />Beneficiary Account Number: 0990317119 
                             <br />Receiving Bank Name: Citibank N.A   
                            <br />Receiving Bank Swiftbic: CITIINBX   
                             <br />IFSC Code: CITI0100000  
                            <br />ABA Routing no: 021000089  
                             <br />Receiving Bank Address: 
                            <br />Citibank N.A  
                            <br />Mahatma Phule Bhavan  
                            <br />Palm Beach Road Junction <br />Sector-17, Vashi, New Mumbai – 400705, India 
                        </td>
                     </tr>
                  </tbody>
               </table>
            </td>
         </tr>
      </tbody>
   </table>
</div>
</body>
</html>
