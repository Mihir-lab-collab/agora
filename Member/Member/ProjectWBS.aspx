<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectWBS.aspx.cs" Inherits="Member_ProjectWBS" ValidateRequest="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="http://code.jquery.com/jquery-1.9.1.js" type="text/javascript"></script>--%>
    <%--   Angular Links--%>
    <%--<link href="css/ng-grid.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/ng-grid/2.0.11/ng-grid.min.js"></script>

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.6.0/moment.min.js"></script>
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.4.4/underscore-min.js"></script>


    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css">

    <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>--%>

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/console.js"></script>
    <script type="text/javascript" src="js/ProjWBS.js"></script>

    <style type="text/css">
        .buttondel {
            background-image: url('images/delete.png');
        }

        .center {
            text-align: center !important;
            margin: 0 auto;
            width: auto;
        }


        .modalBackground {
            background: #827F7F;
            opacity: 0.8;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 12%;
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

        .k-grid th.k-header,
        .k-grid-header {
            background: #252e34;
        }

            .k-grid th.k-header,
            .k-grid th.k-header .k-link {
                color: white;
            }

        /*rows*/

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

        .DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }
        /*for history grid [e]*/


        .ngViewport.ng-scope {
            height: auto !important;
        }

        /*css for table WBS*/


        .WBSLIST {
            border-collapse: collapse;
        }

        .WBSLIST, .row, .hd {
            border: 1px solid #d4cece;
        }

        table.addwbs-list tr th {
            text-align: left;
        }

        table.addwbs-list tr th, .addwbs-list tr td {
            padding: 2px 2px !important;
        }
    </style>

    <script type="text/javascript">
        var isSubmitted = false;
        function preventMultipleSubmissions() {
            if (!isSubmitted) {
                isSubmitted = true;
                SaveWBS();
                return true;
            }
            else {
                return false;
            }
            return true;
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content_wrap">
        <div class="gride_table" id="ProjectWBS" runat="server">
            <div class="box_border">
                <div class="grid_head">
                    <span style="font-size: medium; font-weight: bold">Project MS / WBS -  </span> <asp:Label ID="lblProjName" Font-Size="Medium" Font-Bold="true" runat="server"></asp:Label> 
                    <span style="font-size: medium; font-weight: bold ;float:right"> <input type="button" onclick="openEditPopUP()" class="small_button white_button" value="Add WBS " id="btnWBS" runat="server" style="margin-left:5px;"/> </span>&nbsp;
                    <span style="font-size: medium; font-weight: bold ;float:right"> <input type="button" onclick="OpenPopUp()" class="small_button white_button" value="Add Timesheet" id="btnWBSAdd" runat="server" /> </span>
                   
                </div>
                <div id="gridProjectMilestone"></div>
            </div>
            <br />

            <div class="box_border">
                <br />
                <div style="text-align: start">
                  <%--  <input type="button" onclick="openEditPopUP()" class="small_button white_button" value="Add WBS " id="btnWBS" runat="server" />--%>
                    <asp:CheckBox ID="chkShowAllTasks" text="Show Completed WBS Timesheets" runat="server" style="margin-left:5px;"/>
                </div>
                <br />
                <div id="gridProject"></div>
                <br />
                <br />
             <%--  <div style="text-align: start">
                   <input type="button" onclick="OpenPopUp()" class="small_button white_button" value="Add Timesheet" id="btnWBSAdd" runat="server" />
                    
                </div>--%>
              
                <br />
                <div id="gridProjectWBSDetail"></div>

                <div class="result"></div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                <asp:Button ID="btnDeleteWBSTimeId" runat="server" Text="Button" OnClick="btnDeleteWBSTimeId_Click" Style="display: none;" />

            </div>
            <br />
            <br />
        </div>

        <div align="center" class="modalPopup" id="dvall" runat="server">

            <b>Please Select A Project to Add.</b>

        </div>
        <div class="modalBackground" id="dvbg" runat="server" style="display: none"></div>

        <%-- TIMESHEET STARTS--%>
        <div id="divAddPopupOverlay" runat="server"></div>

        <%-- <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; width: 382px; height: 461px; min-width: 370px; min-height: 50px; top: 50% !important; left: 50% !important; margin-top: -230px; margin-left: -191px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable"> --%>
       <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; width: 1288px; max-width:100%; height: 235px; min-width: 0px; min-height: 50px; top: 50% !important; left: 50% !important; margin-top: -230px; margin-left: -600px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
           <div>
                <div class="popup_head">
                    <h3>Timesheet</h3>
                    <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()" alt="Close" />
                    <div class="clear">
                    </div>
                </div>

                

                <table align="left" width="100%" border="1" cellpadding="2" cellspacing="0">
                        <tr>
                            <td style="width: 10%" align="center">
                                <font face="Verdana" size="2"><b>WBS: &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></b> </font>
                            </td>
                           
                     <td width="10%" align="center">
                                <font face="Verdana" size="2"><b>Name:
                            
                             &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></b></font>
                            </td>
                         
                         <td width="10%" align="center">
                                <font face="Verdana" size="2"><b>Module:&nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></b></font>
                            </td>
                         
   <td style="width: 25%" align="center">
                                <font face="Verdana" size="2"><b>Start Date:</b>&nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></font>
                            </td>
                         
   <td style="width: 25%" align="center">
                                <font face="Verdana" size="2"><b>End Date:</b>&nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></font>
                            </td>
                         
   <td style="width: 25%" align="center">
                                <font face="Verdana" size="2"><b>Comment:</b>&nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></font>
                            </td>


                            <td style="width: 15%" align="center">
                                <font face="verdana" size="2">&nbsp;</font>
                            </td>
                       
            </tr>
                        <tr>
                            <td align="center">
                                 <input id="txtWBS" type="text" name="txtWBS" style="width: 170px" runat="server" />
                            <span id="lblerrmsgTimesheet" style="color: Red;"></span>

                            <input id="txtWBSName" type="text" name="txtWBSName" style="width: 170px; display: none" runat="server" />
                            </td>

                            <td align="center">
                              <input id="txtName" type="text" name="txtName" style="width: 170px" runat="server" />
                              <span id="lblerrmsgName" style="color: Red;"></span>
                            </td>

                            <td align="center">
                               
                            <input id="txtModule" type="text" name="txtModule" style="width: 170px" runat="server" />
                            <span id="lblerrmsgModule" style="color: Red;"></span>

                            </td>
                            

                     <td align="center">
                               <input   type="text" id="txtSDate" class="changeDate" onkeyup="dateInput(this)" onkeypress="return false;" name="txtSDate" style="width: 190px" runat="server" />
                            <span id="lblerrmsgstartDate" style="color: Red;"></span>
                            </td>

                            <td align="center">
                               <input type="text" id="txtEDate" class="changeDate" onkeyup="dateInput(this)" onkeypress="return false;" name="txtEDate" style="width: 190px" runat="server" />
                            <span id="lblerrmsgendDate" style="color: Red;"></span>
                            </td>
                         
                      <td align="center">
                              <textarea id="txtComment" runat="server" class="k-textbox" name="txtComment" style="width: 200px; height: 128px; float: left;margin-left:15px"></textarea>
                            </td>
               
                  <td align="center">
                              <asp:Button ID="btnSaveWBS" runat="server" Text="Save" CausesValidation="true" Style="width: 40px; height: 20px;" ValidationGroup="validate" CssClass="small_button white_button" OnClick="btnSaveWBS_Click" OnClientClick="javascript:return CheckInsert()" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Button  Text="Cancel" OnClientClick="closeAddPopUP()" runat="server" style="width: 40px; height: 20px;" CssClass="small_button white_button" id="btnCancel" />
                           <%-- <input type="button" value="Cancel" onclclick="closeAddPopUP()" runat="server" style="width: 40px; height: 20px;" CssClass="small_button white_button" id="btnCancel" />--%>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <span id="lblerrormsgDate" style="color: Red;"></span>
                            </td>
                        </tr>
                    </table>
                <div>&nbsp;</div>
            </div>
        </div>

        <%--TIMESHEET ENDS--%>

        <%-- WBS STARTS--%>
        <div id="divAddPopupOverlayWBS" runat="server"></div>

        <div class="k-widget k-windowAdd" id="divAddPopupWBS" style="display: none; padding-top: 10px; padding-right: 10px; width: 1210px; height: 605px; min-width: 370px; min-height: 50px; margin-left: -500px; margin-top: -265px; top: 50% !important; left: 50%  !important; left: 260px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
            <div>
                <div class="popup_head">
                    <h3>Add WBS Details</h3>
                    <img src="Images/delete_ic.png" class="close-button" onclick="closeEditPopUP()"
                        alt="Close" />
                    <div class="clear">
                    </div>
                </div>
                <div style="padding: 0 24px;">
                    <div>
                        <input type="button" value="Add" runat="server" style="width: 80px; height: 25px;" id="btnAdd" class="addrow" />
                        <%--<button type="button" id="btn_asp_save" name="btn_wbs_save" runat="server" onclick="SaveWBS();" value="Save" style="width: 80px; height: 25px;">Save</button>--%>
                        <input type="button" id="btn_asp_save" name="btn_wbs_save" runat="server" onclick="SaveWBS();" value="Save" style="width: 80px; height: 25px;" />
                        <input type="button" value="Cancel" onclick="closeEditPopUP()" runat="server" style="width: 80px; height: 25px;" id="btnClose" />
                        <span id="lblerrmsgDate" style="color: Red;"></span>

                    </div>
                    <br />

                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="WBSLIST addwbs-list" align="center" style="margin: 0px auto;" id="testtab">

                        <tr style="height: 40px; background-color: #252e34; color: #fff;">
                            <th style="text-align: left; color: #fff;" class="hd">Milestone &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></th>
                            <th style="text-align: left; color: #fff;" class="hd">WBS   &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></th>
                            <th style="text-align: left; color: #fff;" class="hd">Start Date   &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></th>
                            <th style="text-align: left; color: #fff;" class="hd">End Date   &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></th>
                            <th style="text-align: left; color: #fff;" width="100" class="hd">Planned Hours   &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></th>
                            <th style="text-align: left; color: #fff;" class="hd">
                                <label><b>Assign To</b></label>
                                &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></th>
                            <th style="text-align: left; color: #fff;" class="hd">Status  &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span></th>
                            <th style="text-align: left; color: #fff;" class="hd">Remark </th>

                        </tr>

                        <tr>
                            <td align="center" class="row">
                                <input id="txtMilestone" type="text" name="txtMilestone" style="width: 120px" runat="server" />
                                &nbsp;&nbsp;&nbsp;
                            <span id="lblerrmsgMilestone" style="color: Red;"></span>
                            </td>

                            <td align="center" class="row">
                                <input id="txteditWBS" type="text" name="txteditWBS" style="width: 120px" class="k-textbox" runat="server" />
                                &nbsp;&nbsp;&nbsp;
                            <span id="lblerrmsgWBS" style="color: Red;"></span>
                            </td>


                            <td class="row">
                                <input id="txtWBSSDate" onkeyup="return false" class="date" onkeypress="return false" name="txtWBSSDate" style="width: 180px" runat="server" />
                                &nbsp;&nbsp;&nbsp;
                            <span id="lblerrmsgSDate" style="color: Red;"></span>
                            </td>

                            <td class="row">
                                <input id="txtWBSEDate" onkeyup="return false" class="date" onkeypress="return false" name="txtWBSEDate" style="width: 180px" runat="server" />
                                &nbsp;&nbsp;&nbsp;
                            <span id="lblerrmsgEDate" style="color: Red;"></span>
                            </td>


                            <td class="row">
                                <div style="white-space: nowrap;">
                                    <input id="txteditPlannedHours" type="text" name="txteditPlannedHours" value="9" style="width: 50px" class="k-textbox inputHours" runat="server" />

                                    <select id="dropDownMin" class="pannedhrs">
                                        <option value="0">00</option>
                                        <option value="1">30</option>

                                    </select>
                                </div>
                                &nbsp;&nbsp;&nbsp;<span id="lblerrmsgtxteditPlannedHours" style="color: Red;"></span>


                            </td>

                            <td class="row">
                                <input id="txtassignto" multiple="multiple" data-placeholder="select development team" name="txtassignto" style="width: 150px" class="k-textbox" runat="server" />
                                &nbsp;&nbsp;&nbsp;
                            <span id="lblerrmsgEmployeeSelect" style="color: Red;"></span>
                            </td>

                            <td align="center" class="row">
                                <input id="txtStatus" type="text" name="txtStatus" style="width: 100px" validationmessage="Please Enter Status" runat="server" />

                                &nbsp;&nbsp;&nbsp;
                            <span id="lblerrmsgStatus" style="color: Red;"></span>
                            </td>
                            <td align="center" class="row">
                                <textarea id="txtremark" rows="4" cols="50" style="width: 150px" validationmessage="Please Enter Remark" runat="server"></textarea>

                            </td>
                            <td align="center" class="row">
                                <input type="button" id="btnDelete" class="buttondel" style="width: 21px; height: 21px; display: none" onclick="removeRow()" />
                            </td>

                        </tr>

                    </table>
                    <style>
                        .pannedhrs {
                            padding: 2px .3em;
                            height: 20px;
                            vertical-align: middle;
                            border-radius: 3px;
                            -mo-border-radius: 3px;
                            -webkit-border-radius: 3px;
                            font-size: 12px;
                            line-height: 1.6em;
                        }
                    </style>


                    <div>&nbsp;</div>
                </div>
            </div>
        </div>

        <%-- WBS ENDS--%>

        <%--EDIT WBS STARTS--%>

        <div id="divEditPopupOverlayWBS" runat="server"></div>

        <div class="k-widget k-windowAdd" id="divEditPopupWBS" style="display: none; padding-top: 10px; padding-right: 10px; width: 800px; height: 325px; min-width: 370px; min-height: 50px; top: 25% !important; left: 30%  !important; left: 260px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
            <div>
                <div class="popup_head">
                    <h3>Edit WBS Details</h3>
                    <img src="Images/delete_ic.png" class="close-button" onclick="closePopUP()"
                        alt="Close" />
                    <div class="clear">
                    </div>
                </div>



                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">

                    <tr>
                        <th>Milestone</th>
                        <td align="center">
                            <input id="txtEditMilestone" type="text" name="txtEditMilestone" style="width: 250px" runat="server" />


                            &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                            <span id="lblEditerrmsgMilestone" style="color: Red;"></span>
                        </td>
                        <th>WBS:</th>
                        <td align="center">
                            <input id="txtEditeditWBS" type="text" name="txtEditeditWBS" style="width: 250px" class="k-textbox" runat="server" />


                            &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                            <span id="lblEditerrmsgWBS" style="color: Red;"></span>
                        </td>
                    </tr>
                    <tr>
                        <th>Start Date: 
                        </th>
                        <td>
                            <input id="txtEditWBSSDate" onkeyup="return false" class="date" onkeypress="return false" name="txtEditWBSSDate" style="width: 250px" runat="server" />
                            &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                            <span id="lblEditerrmsgSDate" style="color: Red;"></span>
                        </td>
                        <th>End Date:
                        </th>
                        <td>
                            <input id="txtEditWBSEDate" onkeyup="return false" class="date" onkeypress="return false" name="txtEditWBSEDate" style="width: 250px" runat="server" />
                            &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                            <span id="lblEditerrmsgEDate" style="color: Red;"></span>
                        </td>
                    </tr>

                    <tr>
                        <th>
                            <label><b>Assign To</b></label>
                        </th>
                        <td>

                            <input id="txtEditassignto" multiple="multiple" data-placeholder="select development team" name="txtEditassignto" style="width: 240px" class="k-textbox" runat="server" />
                            &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                            <span id="lblEditerrmsgEmployeeSelect" style="color: Red;"></span>
                        </td>
                        <th>Status</th>
                        <td align="center">
                            <input id="txtEditStatus" type="text" name="txtEditStatus" style="width: 250px" validationmessage="Please Enter Status" runat="server" />

                            &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                            <span id="lblEditerrmsgStatus" style="color: Red;"></span>
                        </td>
                    </tr>
                    <tr>


                        <th>Remark</th>
                        <td align="center">
                            <textarea id="txtEditremark" rows="4" cols="50" style="width: 250px" validationmessage="Please Enter Remark" runat="server"></textarea>
                            <span id="lblEditerrmsgDate" style="color: Red;"></span>

                        </td>
                        <%-- <th></th>
                         <td>
                             <span id="lblEditerrmsgDate" style="color: Red;"></span>
                        </td>--%>

                        <th>Status</th>
                        <td align="center">
                            <input id="txtPlannedHours" type="text" name="txtPlannedHours" style="width: 50px" class="k-textbox  inputHours" runat="server" />
                            &nbsp;&nbsp;&nbsp;
                            <span id="lblerrmsgtxtPlannedHours" style="color: Red;"></span>
                            <select id="dropEditDownMin">
                                <option value="00">00</option>
                                <option value="30">30</option>

                            </select>


                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="4">
                            <asp:Button ID="btnUpdate" runat="server" CausesValidation="true" OnClick="btnUpdate_Click" OnClientClick="javascript:return CheckInsert();" Text="Update" Style="width: 80px; height: 25px;" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" value="Cancel" onclick="closePopUP()" runat="server" style="width: 80px; height: 25px;" id="Button2" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <%--EDIT WBS ENDS--%>

        <asp:HiddenField runat="server" ID="hdProjId" />
        <asp:HiddenField ID="hdprojectMileId" runat="server" />
        <asp:HiddenField ID="hdnMin" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnEmpName" runat="server" />
        <asp:HiddenField ID="hdprojectWBSID" runat="server" />
        <asp:HiddenField ID="hdAssignTo" runat="server" />
        <asp:HiddenField ID="hdWBSHours" runat="server" />
        <asp:HiddenField ID="hdWBSActualHours" runat="server" />
        <asp:HiddenField ID="hdStartTime" runat="server" />
        <asp:HiddenField ID="hdEndTime" runat="server" />
        <asp:HiddenField ID="hdcheckStime" runat="server" />
        <asp:HiddenField ID="hdCheckEtime" runat="server" />
        <asp:HiddenField ID="hdStatusId" runat="server" />
        <asp:HiddenField ID="hdCheckWBSSaveValue" runat="server" />
        <%--AP--%>
        <asp:HiddenField ID="hdnSTime" runat="server" />
        <asp:HiddenField ID="hdnETime" runat="server" />
        <asp:HiddenField ID="hdnWBSID" runat="server" />
        <asp:HiddenField ID="hdnWBSName" runat="server" />
        <asp:HiddenField ID="hdnCount" runat="server" />
        <asp:HiddenField ID="hdnTab" runat="server" />
        <asp:HiddenField ID="hdntest" runat="server" />
        <asp:HiddenField ID="hdnarray" runat="server" />
        <asp:HiddenField ID="hdnCheckExists" runat="server" />
        <asp:HiddenField ID="hdnAdmin" runat="server" />
        <asp:HiddenField ID="hdnGetCurrentDate" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnEmpId" runat="server" />
        <asp:HiddenField ID="hdnProjWBSTimesheetId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnGetProfileAccess" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnModuleID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnModuleName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSelectedEmpName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnWBS" runat="server" ClientIDMode="Static" />
    </div>
</asp:Content>
