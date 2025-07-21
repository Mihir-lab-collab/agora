<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Milestone.aspx.cs" Inherits="Member_Milestone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/datepicker1.css" rel="stylesheet" />


    <link href="css/ng-grid.css" rel="stylesheet" />
    <%--<script type="text/javascript" src="js/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="js/1.3.7_angular.js"></script>
    <script type="text/javascript" src="js/ng-grid.js"></script>
    <script type="text/javascript" src="js/ng-grid-flexible-height.js"></script>--%>

    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js" type="text/javascript"></script>
    <%--<link rel="stylesheet" type="text/css" href="http://angular-ui.github.com/ng-grid/css/ng-grid.css" />--%>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/ng-grid/2.0.11/ng-grid.min.js"></script>

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.6.0/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.4.4/underscore-min.js"></script>

    <script type="text/javascript" src="js/console.js"></script>
    <script type="text/javascript" src="js/Milestone.js"></script>


    <style type="text/css">
        .gridStyle {
            border: 1px solid rgb(212,212,212);
            width: 890px;
            height: 600px;
            padding: 0,0,0,15px;
            background-color: #e9eae8;
            text-align: center;
        }

        .buttondel {
            background-image: url('images/delete.png');
        }

        /*=========Popup css===========*/
        #popupbtn {
            padding: 10px;
            background: #267E8A;
            cursor: pointer;
            color: #FCFCFC;
            margin: 200px 0px 0px 200px;
        }


        .popup-overlay {
            width: 100%;
            height: 100%;
            position: fixed;
            display: none;
            background: rgba(0, 0, 0, .85);
            top: 0;
            left: 100%;
            opacity: 0.6;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
            -webkit-transition: opacity .2s ease-out;
            -moz-transition: opacity .2s ease-out;
            -ms-transition: opacity .2s ease-out;
            -o-transition: opacity .2s ease-out;
            transition: opacity .2s ease-out;
            z-index: 9998;
        }

        .overlay .popup-overlay {
            left: 0;
            display: block;
            background: 10px solid rgba(0, 0, 0, .3);
        }

        .popup {
            position: fixed;
            top: 10%;
            left: 50%;
            z-index: 9999;
            display: none;
            padding: 10px;
            -webkit-transition: opacity .2s ease-out;
            -moz-transition: opacity .2s ease-out;
            -ms-transition: opacity .2s ease-out;
            -o-transition: opacity .2s ease-out;
            transition: opacity .2s ease-out;
        }

            .popup .popup-warp {
                background: #fff; /*opacity: 0; min-height: 150px; width: 500px; margin-left:-260px; */
                padding: 10px;
                position: relative;
                border: 1px solid #e9e9e9;
                border-radius: 20px;
                -webkit-border-radius: 20px;
                -moz-border-radius: 20px;
                behavior: url(pie/PIE.htc);
                border: 10px solid rgba(0, 0, 0, .3);
                -webkit-background-clip: padding-box; /* for Safari */
                background-clip: padding-box;
            }

            .popup.visible, .popup.transitioning {
                z-index: 9999;
                display: block;
            }

                .popup.visible .popup-warp {
                    opacity: 1;
                    -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                    -webkit-transition: opacity .2s ease-out;
                    -moz-transition: opacity .2s ease-out;
                    -ms-transition: opacity .2s ease-out;
                    -o-transition: opacity .2s ease-out;
                    transition: opacity .2s ease-out;
                }

            .popup .popup-exit {
                background: url(../Member/images/close-icon.png) no-repeat top;
                height: 18px;
                width: 18px;
                overflow: hidden;
                text-indent: -9999px;
                position: absolute;
                right: 10px;
                cursor: pointer;
            }

                .popup .popup-exit:hover {
                    background-position: bottom;
                }

            .popup h2 {
                font-size: 16px;
                font-weight: 600;
                margin-bottom: 5px;
                color: #292929;
            }

            .popup p {
                font-weight: 400;
            }

        .importlist {
            padding: 15px 10px;
            overflow: auto;
        }

            .importlist li {
                float: left;
                padding: 10px 0px;
                min-width: 120px;
                text-align: center;
            }

                .importlist li > .radiodiv {
                    overflow: auto;
                    display: inline-block;
                    *display: inline;
                    zoom: 1;
                }

                    .importlist li > .radiodiv input {
                        margin: 2px 3px 0 0;
                        float: left;
                    }

                    .importlist li > .radiodiv label {
                        display: inline-block;
                        float: left;
                    }

        .popup .popup-warp > .note {
            font-size: 11px;
            margin-top: 20px;
        }

        .popup .popup-warp > .botbtndiv {
            text-align: center;
            margin: 20px 0 10px;
        }

            .popup .popup-warp > .botbtndiv .btn {
                margin: 0 5px;
            }


        .modalBackground {
            background: #827F7F;
            opacity: 0.8;
            position: fixed;
            width: 100%;
            height: 100%;
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
    </style>

    <script type="text/javascript">
        var GridMileStone = "#gridMileStone";

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <input type="hidden" id="hdnProjID" runat="server" clientidmode="Static" />
    <input type="hidden" id="hdnExRate" runat="server" clientidmode="Static" />

    <div class="content_wrap" style="padding: 0 0 15px 0;">
        <div class="gride_table" id="MilestoneID" runat="server">
            <div class="box_border">
                <div class="grid_head">
                    <span style="font-size: medium; font-weight: bold">Project Milestone</span>
                </div>
                <div ng-app="myApp" ng-controller="milestoneCtrl">
                    <div>
                        <table class="manage_form" width="60%">
                            <tr>
                                <td>Project:</td>
                                <td>
                                    <asp:Label ID="lblProject" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>Start Date:</td>
                                <td>
                                    <asp:Label ID="lblStartDate" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>End Date:</td>
                                <td>
                                    <label>{{ EndDate }}</label>
                            </tr>

                            <tr>
                                <td>Duration:</td>
                                <td>

                                    <asp:Label ID="lblDuration" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>Initial Project Cost:</td>
                                <td>
                                    <asp:Label ID="lblInitProjectCost" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>Budgeted Cost:</td>
                                <td>
                                    <asp:Label ID="lblCost" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>Total Invoiced:</td>
                                <td>
                                    <asp:Label ID="lblTotalInvoiced" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>Total Recieved:</td>
                                <td>
                                    <asp:Label ID="lblTotalRecieved" runat="server"></asp:Label></td>
                            </tr>
                            <%--  <tr>
                            <td>Balance Amount:</td>
                            <td>
                           
                               <label> {{ totalBalance }}</label>
                            </td>
                        </tr>--%>
                            <tr>
                                <td>Estimated Hours:</td>
                                <td>

                                    <label>{{ totalEstHr }}</label>
                            </tr>
                        </table>
                    </div>
                    <br />

                    <div ng-form="milestone_form">
                        <div style="text-align: start">
                            &nbsp;&nbsp;&nbsp;
        <input type="button" id="btnSave" value="Save" ng-click="insertMileStone();" ng-disabled="isProcessing" runat="server" style="width: 80px; height: 25px;" />
                            &nbsp;&nbsp;&nbsp;
        <input type="button" id="btnAdd" value="Add New" ng-click="Add()" runat="server" onclientclick="ShowLoading();" style="width: 80px; height: 25px;" />
                        </div>
                        <br />
                        <div id="NgGridMilestone" class="gridStyle" ng-grid="gridMileStone" style="width: 1200px;"></div>

                        <div class="result"></div>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                </div>
                <asp:HiddenField ID="hdnEmpId" ClientIDMode="Static" runat="server" />


            </div>
        </div>
        <div align="center" class="modalPopup" id="dvall" runat="server">

            <b>Please Select A Project to View Milestones.</b>

        </div>
        <div class="modalBackground" id="dvbg" runat="server"></div>
    </div>
</asp:Content>

