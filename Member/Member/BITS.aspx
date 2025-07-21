<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BITS.aspx.cs" Inherits="Member_BITS" MasterPageFile="~/Member/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
  
    <script type="text/javascript" language="javascript">
        function ShowIframe()
        {
            openLoading();
            document.getElementById("BICron").src = "http://localhost:51174//Crons/cron.aspx?m=BI";
            return false;
         
        }
        function openLoading()
        {
            $('#divLoading').css('display', '');
            $('#divAddLoadingOverlay').removeClass("k-overlayDisplaynone");
            $('#divAddLoadingOverlay').addClass('overlyload');
            $('#divAddLoadingOverlay').css('display', '');
           setInterval("closeLoading()", 4000);
         }
        function closeLoading()
        {
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
            margin-left: 25px;
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


          div.k-windowAdd{ display:block;}
        .overlyload {
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

    
  /*progress[value]::-webkit-progress-value::before {
  content: '80%';
  position: absolute;
  right: 0;
  top: -125%;
}*/

  progress {
  color: red;
  font-size: .6em;
  line-height: 1.5em;
  text-indent: .5em;
  width: 15em;
  height: 1.8em;
  border: 1px solid #0063a6;
  background:red;
}
progress {
background-color: #000;
color:#000;
}
progress::-webkit-progress-bar {
background-color: #000;
color:#000;
}
progress::-webkit-progress-value {
background-color: #000;
color:#000;
} 
progress::-moz-progress-bar {
background-color: #000;
color:#000;
}
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table tbreakup">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblProjects" Text="BITS" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                             <td align="right">
     <iframe id = "BICron" frameborder = "0" height = "0px" width = "0px"></iframe>
   <asp:ImageButton ID="imgRefresh" runat="server" OnClientClick = "return ShowIframe()" ImageUrl="~/Member/images/refresh-icon.png" Width="50px" Height="40px"/>
<%--<asp:ImageButton ID="imgRefresh" runat="server" OnClick="imgRefresh_Click" OnClientClick="openLoading();" ImageUrl="~/Member/images/refresh-icon.png" Width="50px" Height="40px"/>--%>
                             </td>
                        </tr>
                    </table>
                </div>
    <div id="divAddLoadingOverlay" style="display:none;" class="overlyload"></div>
    <div class="k-widget k-windowAdd" id="divLoading" style="display: none; padding:10px; width:auto; height:auto;  z-index: 999999999; opacity: 1; transform: scale(1); left:50%; top:30%;" data-role="draggable">
    <div>
             <img src="../Member/images/loading.gif" alt=""/>
    </div>
    </div>
                <!--1st grid-->
                
                <div id="grdProjectDetails" style="overflow: auto;"></div>
                <div id="divAddPopupOverlay" runat="server"></div>
                <div id="divOverlay"></div>
              
                

               <!--2nd Grid -->
                <%--<div id="details" class="k-widget k-windowAdd" style="display: none; padding-left: 10px; padding-right: 10px; padding-top: 12px; padding-bottom: 10px; min-width: 600px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">--%>
                <div class="a_popbox" id="details" style="display: none;">
                    <div class="popup_wrap" style="width: 900px; top: 1%; left: 25%; margin-top: 1%; height: auto; max-height: 500px; min-height: auto; overflow: auto;">
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
                            <tr>
                                <th>Status
                                </th>
                                <td>
                                    <label id="lblStatus" class="Detailslbl" />
                                </td>
                                <th>Report Date</th>           
                                <td><label id="lblRptdate" class="Detailslbl" /></td>
                            </tr>

                        </table>
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
                    <div class="popup_wrap" style="width: 500px; top: 10%; left: 40%; margin-top: 5%; min-height: auto; height: auto; max-height: 400px; overflow: auto; align-self: center;">
                        <div class="popup_head">
                            <h3>Timesheet Module Details</h3>
                            <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closeTSPopUP()" />
                            <div class="clear">
                            </div>
                        </div>

                        <%-- Section 1- TSMoule details--%>
                        <div id="grdTSModuleDetails" style="align-content: center; overflow: hidden;"></div>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <input type="hidden" id="hdnProjectId" runat="server" />
    <input type="hidden" id="hdnTSYear" runat="server" />
    <input type="hidden" id="hdnTSMonth" runat="server" />
    <input type="hidden" id="hdnAdmin" runat="server" value="0" />
</asp:Content>


