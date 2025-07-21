<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="TDSEmployee.aspx.cs" Inherits="Member_TdsEmployee" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


 <%--   <script src="//kendo.cdn.telerik.com/2016.2.714/js/jquery.min.js"></script>--%> <%-- Commented for resolving Menu design issue--%>
    <script src="//kendo.cdn.telerik.com/2016.2.714/js/kendo.all.min.js"></script>

    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css"/>
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css"/>
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css"/>
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css"/>
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css"/>

  
    <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>

    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/TdsEmployee.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function ()
        {

            CheckExistingTdsEmp();

        });

    </script>

    <style type="text/css">
        .demo-section {
            overflow: auto;
        }

        .tdPadding {
            padding: 4px;
        }

        .metrotable {
            width: 100%;
            border-collapse: collapse;
        }

            .metrotable > thead > tr > th {
                font-size: 1.3em;
                padding-top: 0;
                padding-bottom: 5px;
            }
    </style>

    <style type="text/css">
        .modalBackground {
            background: #827F7F;
            opacity: 0.8;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 20%;
        }

        .modalPopup {
            position: fixed;
            top: 46%;
            left: 45%;
            background-color: #ffffff;
            border-color: black;
            border-style: solid;
            border-width: 2px;
            height: 30px;
            padding-left: 10px;
            padding-top: 10px;
            width: 300px;
            z-index: 100001;
        }



        /*headers*/

        /*.k-grid th.k-header,
        .k-grid-header {
            background: #252e34;
        }*/

        /*.k-grid th.k-header,*/
        .k-grid th.k-header .k-link {
            color: black;
        }

        /*rows*/

        /*.k-grid-content > table > tbody > tr {
            background: #cbc8c8;
            padding: 4px !important;
            margin: 0 !important;
            white-space: pre-wrap;
            
        }

        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
           
        }*/

        /*selection*/

        /*.k-grid table tr.k-state-selected {
            background: #f99;
            color: #fff;
        }*/
        /*for history grid [s]*/
        /*.k-alt {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            
        }*/

        /*.DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }*/
        /*for history grid [e]*/

    </style>

    <style type="text/css">
        .red {
            color: red;
            text-align: center;
            font-size: medium;
        }

        .black {
            color: black;
            text-align: center;
        }

        .font {
            font: medium;
        }
    </style>

    <style type="text/css">
        .pdf-page {
            margin: 0 auto;
            box-sizing: border-box;
            box-shadow: 0 5px 10px 0 rgba(0,0,0,.3);
            background-color: #fff;
            color: #333;
            position: relative;
            font-size: small;
        }

        .pdf-header {
            position: absolute;
            top: .5in;
            height: .6in;
            left: .5in;
            right: .5in;
            border-bottom: 1px solid #e5e5e5;
        }

        .invoice-number {
            padding-top: .17in;
            float: right;
        }

        .pdf-footer {
            position: absolute;
            bottom: .5in;
            height: .6in;
            left: .5in;
            right: .5in;
            padding-top: 10px;
            border-top: 1px solid #e5e5e5;
            text-align: left;
            color: #787878;
            font-size: 12px;
        }

        .pdf-body {
            position: absolute;
            top: 3.7in;
            bottom: 1.2in;
            left: .5in;
            right: .5in;
        }

        .size-a4 {
            width: 8.3in;
            height: 11.7in;
        }

        .size-letter {
            width: 8.5in;
            height: 11in;
            font-size: medium;
        }

        .size-executive {
            width: 7.25in;
            height: 10.5in;
        }

        .company-logo {
            font-size: 30px;
            font-weight: bold;
            color: #3aabf0;
        }

        .for {
            position: absolute;
            top: 1.5in;
            left: .5in;
            width: 2.5in;
        }

        .from {
            position: absolute;
            top: 1.5in;
            right: .5in;
            width: 2.5in;
        }

            .from p, .for p {
                color: #787878;
            }

        .signature {
            padding-top: .2in;
        }



        .errorClass { border:  1px solid red; }

        .k-grouping-row .k-icon {
        display: none !important;
        }
        
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                     <asp:Label ID="lblInvestmentDeclaration" Text="Investment Declaration" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <input type="button" id="btnGetTds" value="Submit Declaration Form" onclick="GetTDS()" class="small_button white_button open" style="display: none" />
                    </div>
                    <div class="clear"></div>

                </div>
                <div class="a_popbox" id="divAddPopup" style="display: none;">
                    <div class="popup_wrap" style="width: 900px; top: 1%; left: 25%; margin-top: 1%; height: auto; max-height: 500px; min-height: auto; overflow: auto;">
                        <div class="popup_head">
                            <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
                            <div class="clear">
                            </div>
                            <div align="center">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="Span1" style="font-size: large; font-weight: 100">Investment Declaration</span>
                                        </td>
                                    </tr>

                                </table>
                            </div>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <input type="button" id="btnSaveChanges" value="SUBMIT" onclick="SaveChangesTDS()" class="small_button white_button open" style="width: 80px; height: 25px; display: none;" />
                                        </td>
                                        <td> </td>
                                        <td>
                                            <input type="button" id="btnExportToPdf" value="PDF" class="small_button white_button open" onclick="getPDF('.pdf-page')" style="width: 80px; height: 25px;"/></td>
                                   <td>
                                       <div id="dvMsg" style=" color:red; width:190px; padding:3px; display:none;" >
                           * Please Enter Father Name 
                        </div>  
                                    <div id="dvErr" style=" color:red; width:190px; padding:3px; display:none;" >
                          * Please Enter PAN No. 
                        </div></td>
                                         </tr>
                                </table>
                            </div>
                        </div>

                        <div class="pdf-page size-a4">
                            <br />
                            <div id="dv1stTable" style="display:none"; align="center">

                                <h1 align="center" style="color: black">Employee Investment Declaration form</h1>

                                <table class="metrotable" border="1" style="height: 20px; width: 763px;">
                                    <tr>
                                        <td class="tdPadding">Financial Year:</td>
                                        <td class="tdPadding">
                                            <label id="lblFinancialYear" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdPadding">Name of the Employee:</td>
                                        <td class="tdPadding">
                                            <label id="lblEmpName1"></label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdPadding">Father Name:</td>
                                        <td class="tdPadding">
                                            <label id="lblFatherName" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdPadding">PAN:</td>
                                        <td class="tdPadding">
                                            <label id="lblPanNo" />
                                        </td>
                                    </tr>
                                  <tr class="declarationCheck">
                                      <td class="tdPadding" colspan="2">
                                        <!-- Pure CSS Tick Box -->
                                        <div style="
                                            display: inline-block;
                                            width: 18px;
                                            height: 18px;
                                            border: 2px solid black;
                                            position: relative;
                                            margin-right: 8px;
                                            vertical-align: middle;
                                          ">
                                          <div style="
                                              position: absolute;
                                              top: 2px;
                                              left: 4px;
                                              width: 6px;
                                              height: 12px;
                                              border-right: 2px solid green;
                                              border-bottom: 2px solid green;
                                              transform: rotate(45deg);
                                            ">
                                          </div>
                                        </div>

                                        <!-- Your original text -->
                                        <label style="vertical-align: middle;">
                                          I understand the applicable tax regimes under the Income Tax Act and acknowledge my responsibility for ensuring accurate TDS declarations.
                                        </label>
                                      </td>
                                   </tr>

                                   <tr>
                                      <td class="tdPadding" colspan="2">
                                        <div id="tickNew" style="display: inline-block; width: 18px; height: 18px; border: 2px solid black; position: relative; margin-right: 8px; vertical-align: middle;"></div>
                                        <label for="RadioButton1">New Regime</label>

                                        <div id="tickOld" style="display: inline-block; width: 18px; height: 18px; border: 2px solid black; position: relative; margin-right: 8px; vertical-align: middle;"></div>
                                        <label for="RadioButton2">Old Regime</label>
                                    </td>

                                </tr>

                                  <tr>
                                    <td class="tdPadding" colspan="2">
                                        <small>Note: If New tax regime selected then rebate of HRA,PT,Chapter VI-A Deductions,will  not be applicable. 
                                            If Old tax regime selected then all the rebate will be applicable as per old taxation rule.</small>
                                    </td>
                                </tr>

                                </table>

                            </div>

                            <div id="dv1stDumyDisplayTable" style="display: block" align="center">
                            <h1 align="center" style="color: black">Employee Investment Declaration form</h1>
                            <table border="1" style="height: 20px; width: 764px; border-collapse: collapse; border: 1px solid black">
                                <tr>
                                    <td class="tdPadding">Financial Year:</td>
                                    <td>
                                        <input type="text" id="txtFinancialYear" style="width: 97%;" readonly="readonly"/></td>
                                </tr>
                                <tr>
                                    <td class="tdPadding">Name of the Employee:</td>
                                    <td>
                                        <input type="text" id="txtEmpName" style="width: 97%;" runat="server" readonly="readonly"/></td>
                                </tr>
                                <tr>
                                    <td class="tdPadding">Father Name:</td>
                                    <td>
                                    <asp:TextBox ID="txtFatherName" runat="server" style="width: 97%;" ClientIDMode="Static"></asp:TextBox> </td> 
                                
                                  </tr>
                                <tr>
                                    <td class="tdPadding">PAN:</td>
                                    <td>
                                        <asp:TextBox ID="txtPanNo" onblur="fnValidatePAN(this);" runat="server" style="width: 97%;"></asp:TextBox>  </td>
                        
                                   
                                </tr>
                               <tr>
                                    <td class="tdPadding declarationCheck" colspan="2">
                                        <asp:CheckBox ID="chkDec" runat="server" />
                                        <label>
                                            I understand the applicable tax regimes under the Income Tax Act and acknowledge my responsibility for ensuring accurate TDS declarations.
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdPadding" colspan="2">
                                        <asp:RadioButton ID="rdNewRegime" name="Regime" runat="server" GroupName="Regime" Text="New Regime" />
                                        <asp:RadioButton ID="rdOldRegime" name="Regime" runat="server" GroupName="Regime" Text="Old Regime" />
                                    </td>
                                </tr>

                                 <tr>
                                    <td class="tdPadding" colspan="2">
                                        <small>Note: If New tax regime selected then rebate of HRA,PT,Chapter VI-A Deductions,will  not be applicable. 
                                            If Old tax regime selected then all the rebate will be applicable as per old taxation rule.</small>
                                    </td>
                                </tr>
                            </table>
                        </div>

                            <br />
                            <%--<h1 align="center" style="color: black">Details of Incomes,Investments and other [Per Annum]</h1>--%>
                         
                            <div id="gridProject" align="center"></div>

                             <div id="dvTemp" style="display:none;" align="center">
                                <div class="demo-section k-content">

                                    <table id="Temp" class="metrotable" border="1" style="height: 20px; width: 763px;">
                                        <thead>
                                            <tr>
                                                <th>Description</th>
                                                <th>Amount</th>
                                                <th>Comment</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td colspan="3"></td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>

                                <script id="template" type="text/x-kendo-template">
                                 <tr>
                                        <td class="tdPadding" style="width: 50%;">#= TdsName #</td>
                                        <td class="tdPadding" style="width: 15%; text-align: right;">#= kendo.toString(Amount,"n") #</td>
                                        <td class="tdPadding" style="width: 35%; text-align: left;">#= Comment #</td>
                                 </tr>                                                       
                                </script>

                            </div>
                            <br />
                            <%--  <br />

                            <h1 align="center" style="color: black">U/C VIA  [Per Annum]</h1>
 
                             <div id="gridSecondList"></div>

                            <div id="dvTemp1" style="display:none" align="center">
                                <div class="demo-section k-content" >

                                    <table id="Temp1" class="metrotable" border="1" style="height: 20px; width: 763px;">
                                        <thead>
                                            <tr>
                                                <th>Description</th>
                                                <th>Amount</th>
                                                <th>Comment</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td colspan="3"></td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </div>

                                <script id="template1" type="text/x-kendo-template">
                <tr>
                    <td class="tdPadding">#= TdsName #</td>
                    <td class="tdPadding" align="right">#= kendo.toString(Amount,"n") #</td>
                    <td class="tdPadding"align="center">#= Comment #</td>
                </tr>
                                </script>
                            </div>--%>

                            <table class="signature" border="0" style="width: 183%;">
                                <tr>
                                    <td class="tdPadding">Date of signing the form:</td>
                                    <td>Signature of the Employee: ________________</td>

                                </tr>
                                <tr>
                                    <td>_ </td>
                                   <td>_ </td>

                                </tr>
                            </table>                            
                        </div>                       
                    </div>                    
                </div>

                <div id="gridFilledTds"></div>

            </div>
        </div>
        <asp:HiddenField ID="hdnEmpId" runat="server" />
         <asp:HiddenField ID="hdnTdsId" runat="server" /> 
        <asp:HiddenField ID="hdnRegime" runat="server" />
    </div>
</asp:Content>

