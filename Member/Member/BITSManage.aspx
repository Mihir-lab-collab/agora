<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BITSManage.aspx.cs" Inherits="Member_BITSManage" MasterPageFile="~/Member/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <%--<script src="js/Myprojectlist.js" type="text/javascript"> </script>--%>
    <script type="text/javascript" language="javascript">
        function CancelPopUP() {
            CancelAddPopUP();
            return false;
        }
        function CheckInsert() {
            document.getElementById("<%=hfProjectStatus.ClientID%>").value = $("#drpstatus").val();
            document.getElementById("<%=hfprojstatusDate.ClientID%>").value = $("#txtStartDate").val();
            document.getElementById("<%=hfprojcompletion.ClientID%>").value = $("#txtcomplete").val();
            document.getElementById("<%=hfRemarks.ClientID%>").value = $("#txtremark").val();
            document.getElementById("<%=hdnProjectId.ClientID%>").value = $("#lblprojid").html();
            closeAddPopUP();
        }


        function ShowIframe() {
            openLoading();
            document.getElementById("BICron").src = "http://localhost:51174//Crons/cron.aspx?m=BI";
            return false;

        }
        function openLoading() {
            $('#divLoading').css('display', '');
            $('#divAddLoadingOverlay').removeClass("k-overlayDisplaynone");
            $('#divAddLoadingOverlay').addClass('overlyload');
            $('#divAddLoadingOverlay').css('display', '');
            setInterval("closeLoading()", 4000);
        }
        function closeLoading() {
            $('#divLoading').css('display', 'none');
            $('#divAddLoadingOverlay').removeClass("overlyload").addClass("k-overlayDisplaynone");
            $('#divAddLoadingOverlay').css('display', 'none');
            window.location.reload();
        }
        var GridReport = "#grdProjectDetails";




    </script>

    <script type="text/javascript" src="js/BITS.js"></script>


    <style type="text/css">
        .Detailslbl {
            width: 300px;
        }
    </style>

    <style type="text/css">
        /*headers*/

        .k-grid th.k-header,
        .k-grid-header {
            background: #252e34;
        }

            .k-grid th.k-header,
            .k-grid th.k-header .k-link {
                color: white;
            }

        /*rows*/
        /* .k-grid-content {
    height: 100%;
    max-height: 500px;
    overflow-y: auto;
    position: relative;
    width: 100%;
}*/

        .k-grid-content > table > tbody > tr {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }


        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }

        /*selection*/

        .k-grid table tr.k-state-selected {
            background: #f99;
            color: #fff;
        }
        /*for history grid [s]*/
        .k-alt {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*for history grid [e]*/

        /*for displaying text right aligned*/
        .k-grid .ra,
        .k-numerictextbox .k-input {
            text-align: right;
        }
        /*end*/

        /*for wrapping column header text*/
        .k-grid .k-grid-header .k-header .k-link {
            height: auto;
        }

        .k-grid .k-grid-header .k-header {
            white-space: normal;
        }
        /*end*/
        .tbreakup .k-grid-header-wrap table {
            width: 100% !important;
        }
        .tbreakup .k-grid table {
            width: 100% !important;
        }
        /*added to display color code for project health*/
        .btnColor {
            width: 20px;
            height: 20px;
            border-radius: 100px;
            float: left;
            margin-left: 12px;
        }

        .red {
            background-color: red;
        }

        .green {
            background-color: green;
        }

        .yellow {
            background-color: yellow;
        }

        .blue {
            background-color: blue;
        }
        .orange {
            background-color: orange;
        }
        /*end added to display color code for project health*/


         div.k-windowAdd
         { 
             display:block;

         }
        .overlyload 
        {
            background: rgba(0, 0, 0, 0.5);
            position: fixed;
            top: 0;
            left: 0;
            z-index: 100000000;
            width: 100%;
            height: 100%;
            background-color: #000;
            filter: alpha(opacity=50);
            opacity: .5; 
        }
        #paymentrecd {
        margin-left: calc(50% - 50vw);
        width: 100vw;
        }

        .line-in-middle { position:relative } 
         .line-in-middle:before{
             bottom:0;
             left:50%;
             width:2px;
             height:18px;
             position:absolute;
             content:"";
             border-left:2px dotted #000;
	}
         .container {
display: block;
position: relative;
padding-left: 35px;
margin-bottom: 12px;
cursor: pointer;
font-size: 22px;
-webkit-user-select: none;
-moz-user-select: none;
-ms-user-select: none;
user-select: none;
}

/*/ Hide the browser's default checkbox /*/
.container input {
position: absolute;
opacity: 0;
cursor: pointer;
height: 0;
width: 0;
}

/*/ Create a custom checkbox /*/
.checkmark {
position: absolute;
top: 0;
left: 0;
height: 25px;
width: 25px;
background-color: #eee;
}

/*/ On mouse-over, add a grey background color /*/
.container:hover input ~ .checkmark {
background-color: #ccc;
}

/*/ When the checkbox is checked, add a blue background /*/
.container input:checked ~ .checkmark {
background-color: #2196F3;
}

/*/ Create the checkmark/indicator (hidden when not checked) /*/
.checkmark:after {
content: "";
position: absolute;
display: none;
}

/*/ Show the checkmark when checked /*/
.container input:checked ~ .checkmark:after {
display: block;
}

/*/ Style the checkmark/indicator /*/
.container .checkmark:after {
left: 9px;
top: 5px;
width: 5px;
height: 10px;
border: solid white;
border-width: 0 3px 3px 0;
-webkit-transform: rotate(45deg);
-ms-transform: rotate(45deg);
transform: rotate(45deg);
}
.list { display:inline-block;}
#results, #events {
  margin-top: 20px;
}

#console > div {
  margin-bottom: 10px;
}

</style>

    <!-- vnw 06-jan-2022 start-->
    <style type="text/css">
        #modalContainer {
	position:absolute;
	width:100%;
	height:100%;
	top:0px;
	left:0px;
}
        .ModalPopUp {
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            background-image: url('../Member/images/CPP_bg.png');
            z-index: 99999999;
        }
.ModalLoad 
{
	background: url('../Member/images/CPP_load.gif') no-repeat; width: 60px; height: 60px; margin:0 auto;  
}
    </style>
    <!-- vnw 06-jan-2022 end --->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table tbreakup">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td align="left" width="38%">
                                <asp:Label ID="lblProjects" Text="BITS" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                             <td id="tdOverHead" width="12%">
                                <input type="checkbox" id="chkIsOverHeads" onclick="fnshowOverHeads(this.checked);" checked="checked"/>
                                <span class="active-set" id="Span1" style="font-size: small; text-align: left;">Include Overheads</span> &nbsp&nbsp
                             </td>
                            <td id="tdTNM" width ="6%">
                                <input type="checkbox" id="chkTNM"  checked="checked"/>
                                <span class="active-set" id="Spantnm" style="font-size: small; text-align: left;">T&M</span> &nbsp&nbsp
                             </td>
                            <td id="tdFixedCost">
                                <input type="checkbox" id="chkFixedCost"  checked="checked"/>
                                <span class="active-set" id="Spanfc" style="font-size: small; text-align: left;">Fixed Cost</span> &nbsp&nbsp
                             </td>
                            <td align="right">
                                 <iframe id = "BICron" frameborder = "0" height = "0px" width = "0px"></iframe>
                                <%--<asp:ImageButton ID="imgRefresh" runat="server" OnClientClick = "return ShowIframe()" ImageUrl="~/Member/images/refresh-icon.png" Width="50px" Height="40px"/> --%>
                                <%--<asp:ImageButton ID="imgRefresh" runat="server" OnClick="imgRefresh_Click" OnClientClick="openLoading();" ImageUrl="~/Member/images/refresh-icon.png" Width="50px" Height="40px"/>--%>

                            </td>
                        </tr>
                    </table>
                  <div id="checkBoxList" style="display:block;text-align:center;"></div>
                    <div id="results">
                </div>
                    <!-- added by vw 06-jan-2023 start-->
                    <div id="divModalPopUp" class="ModalPopUp" runat="server">
                            <div class="ModalLoad"></div>
                        </div>
                    <!-- added by vw 06-jan-2023 end-->

                <div id="divAddLoadingOverlay" style="display:none;" class="overlyload"></div>
    <div class="k-widget k-windowAdd" id="divLoading" style="display: none; padding:10px; width:auto; height:auto;  z-index: 999999999; opacity: 1; transform: scale(1); left:50%; top:30%;" data-role="draggable">
    <div>
             <img src="../Member/images/loading.gif" alt=""/>
    </div>
    </div>
                <div id="grdProjectDetails" style="overflow: auto;"></div>
                <div id="divAddPopupOverlay" runat="server"></div>
                <div id="divOverlay"></div>
                
                <!--1st grid-->
                

                <!--2nd Grid -->
                <%--<div id="details" class="k-widget k-windowAdd" style="display: none; padding-left: 10px; padding-right: 10px; padding-top: 12px; padding-bottom: 10px; min-width: 600px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">--%>
                <div class="a_popbox" id="details" style="display: none;">
                    <div class="popup_wrap" style="width: 900px; top: 1%; left: 25%; margin-top: 1%; height: auto; max-height:500px; min-height:auto; overflow: auto;">
                        <div class="popup_head">
                            <h3>Project Details</h3>
                            <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
                            <div class="clear">
                            </div>
                        </div>
                        <%-- Section 1- Project details--%>
                        <table cellpadding="0" cellspacing="0" border="1" width="100%" class="manage_form1">
                            <tr>
                                <th>Project Name
                                </th>
                                <td width="35%">
                                    <label id="lblProjectName" class="Detailslbl" />
                                </td>
                                <th>PM
                                </th>
                                <td>
                                    <label id="lblPM" class="Detailslbl" />
                                </td>
                            </tr>
                            <tr>
                                <th>Duration
                                </th>
                                <td>
                                    <label id="lblDuration" class="Detailslbl" />
                                </td>
                                <th>Budgeted Hours
                                </th>
                                <td>
                                    <label id="lblBudgetedHrs" class="Detailslbl" />
                                </td>
                            </tr>
                            <tr id="trBudgetdCost">
                                 <th>Budgeted Cost
                                </th>
                                <td>
                                    <label id="lblBudgetedCost" class="Detailslbl" />
                                </td>
                                 <th>Total Hours Consumed
                                </th>
                                <td>
                                    <label id="lblstrUnApprovedHours" class="Detailslbl" />
                                </td>
                            </tr>
                              <tr>
                                <th id="thPayRec">Payment Received
                                </th>
                                <td id="tdPayRec">
                                    <label id="lblPaymentRec" class="Detailslbl" />
                                </td>
                                 <th>Actual Cost
                                </th>
                                <td>
                                    <label id="lblActualCost" class="Detailslbl" />
                                </td>
                            </tr>
                             <tr>
                                <th>Report Date</th>           
                                <td><label id="lblRptdate" class="Detailslbl" /></td>
                                 <th>Status
                                </th>
                                <td>
                                    <label id="lblStatus" class="Detailslbl" />
                                </td>
                            </tr>
                            <tr>
                                <th>Allocated Development Team</th>           
                                <td><label id="lblDevelopmentTeam" class="Detailslbl" /></td>
                                 <th></th>
                                <td></td>
                            </tr>
                            <%--<tr id="trBudgetdCost">
                                 <th>Budgeted Cost
                                </th>
                                <td>
                                    <label id="lblBudgetedCost" class="Detailslbl" />
                                </td>
                                 <th>Actual Cost
                                </th>
                                <td>
                                    <label id="lblActualCost" class="Detailslbl" />
                                </td>
                            </tr>--%>
                           <%-- <tr>
                                <th id="thPayRec">Payment Received
                                </th>
                                <td id="tdPayRec">
                                    <label id="lblPaymentRec" class="Detailslbl" />
                                </td>
                                <th>Status
                                </th>
                                <td>
                                    <label id="lblStatus" class="Detailslbl" />
                                </td>
                            </tr>--%>
                             <tr>
                                <%--<th></th>
                                <td>
                                    <%--<label id="lblStatus" class="Detailslbl" />
                                </td>--%>
                                <%--<th>Report Date</th>           
                                <td><label id="lblRptdate" class="Detailslbl" /></td>--%>
                                 <td colspan="2"> <button id="btnStatus" type="button" onclick="openAddPopUP()" style="width:100px;height:30px;" >Status</button></td>
                             
                                 <th colspan="2">
                              <div style="float:right;"  id="showdiv">
                            <div id="Block1">
                            <div style="width:385px;"><span style="width:120px;display: inline-block;"><b>Received:</b>&nbsp;&nbsp;&nbsp;</span><div id="paymentrecd1" style="background-color:Orange;color:red;width:50px;margin: 4px 0 0;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label id="lblPaymentRec" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcost1" style="background-color:green;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCost1" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent OH:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcostOH1" style="background-color:blue;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCostOH1" class="Detailslbl" /></span></div>
                            </div>
                            <div id="Block2">
                                <div style="width:385px;"><span style="width:120px;display: inline-block;"><b>Received:</b>&nbsp;&nbsp;&nbsp;</span><div id="paymentrecd2" style="background-color:Orange;color:red;width:50px;margin: 4px 0 0;display:inline-block;" class="line-in-middle">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label id="lblPaymentRec" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcost2" style="background-color:green;color:white;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCost2" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent OH:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcostOH2" style="background-color:blue;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCostOH2" class="Detailslbl" /></span></div>
                             </div>
                             <div id="Block3">
                                <div style="width:385px;"><span style="width:120px;display: inline-block;"><b>Received:</b>&nbsp;&nbsp;&nbsp;</span><div id="paymentrecd3" style="background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label id="lblPaymentRec" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcost3" style="background-color:green;color:white;display:inline-block;width:50px;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCost3" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent OH:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcostOH3" style="background-color:blue;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCostOH3" class="Detailslbl" /></span></div>
                            </div>
                            <div id="Block4">
                            <div style="width:385px;"><span style="width:120px;display: inline-block;"><b>Received:</b>&nbsp;&nbsp;&nbsp;</span><div id="paymentrecd4" style="background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;" class="line-in-middle">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label id="lblPaymentRec" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcost4" style="background-color:green;color:white;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCost4" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent OH:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcostOH4" style="background-color:blue;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCostOH4" class="Detailslbl" /></span></div>
                            </div>
                            <div id="Block5">
                            <div style="width:385px;"><span style="width:120px;display: inline-block;"><b>Received:</b>&nbsp;&nbsp;&nbsp;</span><div id="paymentrecd5" style="background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;width:50px;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label id="lblPaymentRec" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcost5" style="background-color:green;color:white;display:inline-block;width:50px;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCost5" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent OH:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcostOH5" style="background-color:blue;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCostOH5" class="Detailslbl" /></span></div>
                            </div>
                            <div id="Block6">
                            <div style="width:385px;"><span style="width:120px;display: inline-block;"><b>Received:</b>&nbsp;&nbsp;&nbsp;</span><div id="paymentrecd6" style="background-color:Orange;color:red;margin: 4px 0 0;display:inline-block;width:50px;" class="line-in-middle">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label id="lblPaymentRec" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcost6" style="background-color:green;color:white;display:inline-block;width:50px;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCost6" class="Detailslbl" /></span></div><div style="width:385px;margin-top:4px;"><span style="width:120px;display: inline-block;"><b>Spent OH:</b>&nbsp;&nbsp;&nbsp;</span><div id="actualcostOH6" style="background-color:blue;display:inline-block;">&nbsp;</div><span style="display:inline-block;width:50px;">&nbsp;&nbsp;<label for="lblActualCostOH6" class="Detailslbl" /></span></div>
                            </div>
                            </div>
                                 </th>
                                
                            </tr>
                            
                            </table>
                                                      
                               
                            <%--<div style="width:100px;float:right;margin-right:285px;margin-top:-30px;display:none;" id="showdiv">--%>
                            
                            <div class="clear">
                        </div>
                        <%-- Section 2- Timesheet Breakup - Work wise --%>
                        <div class="popup_head">
                            <h3>Timesheet Breakup - Work Wise</h3>
                            <div class="clear">
                            </div>
                        </div>
                        <div id="grdTSBreakup" style="align-content: center;"></div>
                        <div class="clear">
                        </div>
                        <%-- Section 3- Timesheet Details - Month & Year wise --%>
                        <div class="popup_head">
                            <h3>Timesheet</h3>
                            <div class="clear">
                            </div>
                        </div>
                        <div id="grdTSDetails" style="align-content: center;"></div>
                    </div>
                </div>

                <!-- 3rd Grid-->
                <%--<div id="TSdetails" class="k-widget k-windowAdd" style="display: none; padding-left: 10px; padding-right: 10px; padding-top: 12px; padding-bottom: 10px; min-width: 600px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">--%>
                <div class="a_popbox" id="TSdetails" style="display: none;">
                    <div class="popup_wrap" style="width: 500px; top: 10%; left: 40%; margin-top: 5%; min-height:auto; height:auto; max-height:400px; overflow: auto; align-self: center;">
                        <div class="popup_head">
                            <h3>Timesheet Module Details</h3>
                            <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closeTSPopUP()" />
                            <div class="clear">
                            </div>
                        </div>

                        <%-- Section 1- TSMoule details--%>
                        <div id="grdTSModuleDetails" style="align-content: center; overflow:hidden;"></div>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

            <div class="a_popbox" id="divStatusPopUP" style="display: none;">
    <div class="k-widget k-windowAdd" id="divAddPopup" padding-top: 10px; padding-right: 10px; min-width: 120px; min-height: 80px; top: 10%; left: 500px; z-index: 10003; margin-top: 5%; opacity: 1; transform: scale(1);" data-role="draggable" >
        <div>
            <div class="popup_head">
                <h3>Update Status</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="javascript: return CancelPopUP();"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

          <asp:UpdatePanel ID="pnlUpdateStatus" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form project-tbl">
                       <tr style="display:none">
                            <td colspan="4">
                                <asp:Label ID="lblprojid" runat="server" Style="text-align: left; width: 100%;" ClientIDMode="Static" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>Project</th>
                            <td align="left">
                                <asp:Label ID="lblProjectname1" runat="server" ClientIDMode="Static" Style="text-align: left; width: 100%;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>Last Status</th>
                            <td align="left">
                                <asp:Label ID="lblStatus1" runat="server" ClientIDMode="Static" Style="text-align: left; width: 100%;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>Current Status</th>
                            <td align="left">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <th>Date</th>
                                        <td>
                                            <input id  ="txtStartDate" name="txtStartDate" onkeyup="dateInput(this)" onkeypress="dateInput(this)" style="width: 230px" class="k-textbox" />
                                            <span style="color: Red;">*</span>
                                            <span id="lblmsgdate" style="color: Red;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Project Status Type</th>
                                        <td>
                                            <input id="drpstatus" type="text" style="width: 230px" />
                                            <span style="color: Red;">*</span>
                                            <span id="lblmsgstatus" style="color: Red;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Completed</th>
                                        <td>
                                            <input id="txtcomplete" style="width: 230px" class="k-textbox" onkeypress="return isNumberKeyOrDelete(event);"/>
                                            <span style="color: Red;">*</span>
                                            <span id="lblmsgcomplete" style="color: Red;"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Remark</th>
                                        <td>

                                           <%-- <input id="txtremark" style="width: 230px" class="k-textbox" />--%>
                                           <%--  <asp:TextBox runat="server" ID="txtremark" TextMode="MultiLine" Height="57px" Width="230px"></asp:TextBox>--%>
                                            <textarea cols="60" rows="5" id="txtremark" name="message"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <td>
                                            <asp:Label ID="errormessage" runat="server" ClientIDMode="Static"></asp:Label></td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <th></th>
                            <td>
                                <asp:LinkButton ID="lnkSaveProjectStatus" runat="server" CssClass="small_button red_button open" OnClientClick="CheckInsert()" OnClick="lnkSaveProjectStatus_Click" ><span>Save</span></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;
                                <input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="CancelAddPopUP();" />
                            </td>
                        </tr>
                    </table>
                    <br />
                  <div class="popup_head" style="overflow:hidden; padding-bottom:10px; color:#000"> <h3> Project Status</h3></div>
<div id="GridProjectStaus"></div>    
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </div>
        <script type="text/x-kendo-template" id="popup-editor">
   
       
    </script>
    <asp:HiddenField ID="hfModuleID" runat="server" />
    <asp:HiddenField ID="hfProjectStatus" runat="server" />
    
    <asp:HiddenField ID="hfprojcompletion" runat="server" />
    <asp:HiddenField ID="hfRemarks" runat="server" />

        <asp:HiddenField ID="hfprojstatusDate" runat="server" />
    <input type="hidden" id="hdnProjectId" runat="server" />
    <input type="hidden" id="hdnTSYear" runat="server" />
    <input type="hidden" id="hdnTSMonth" runat="server" />
    <input type="hidden" id="hdnAdmin" runat="server" value="0"/>
    <asp:HiddenField ID="TypeVal" runat="server" Value="0" />
   
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>
    </label>

   
    </div>
</asp:Content>

