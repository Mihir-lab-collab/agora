<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Notes.aspx.cs" Inherits="Member_Notes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://angular-ui.github.com/ng-grid/css/ng-grid.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/ng-grid/2.0.11/ng-grid.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.6.0/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.4.4/underscore-min.js"></script>
    <script type="text/javascript" src="https://cdn.kendostatic.com/2012.2.710/js/cultures/kendo.culture.en-GB.min.js"></script>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>


    <script type="text/javascript" src="../js/console.js"></script>
    <script src="js/Notes.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
                    
            $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" });
            $('[id$="txtFromTo"]').kendoDatePicker({ format: "dd/MM/yyyy" });
       
            var CurrentDate = new Date();
            var toDt = new String(CurrentDate.getDate());
            toDt = toDt.length > 1 ? toDt : '0' + toDt;
            var toMonth = new String(CurrentDate.getMonth() + 1);
            toMonth = toMonth.length > 1 ? toMonth : '0' + toMonth;
            var toYear = CurrentDate.getFullYear();
            var toDate = toDt + "/" + toMonth + "/" + toYear;


            var fromDt = new Date();
            fromDt.setMonth(fromDt.getMonth() - 1)
            var frmdt = new String(fromDt.getDate());
            frmdt = frmdt.length > 1 ? frmdt : '0' + frmdt;
            var fromMonth = fromDt.getMonth() + 1;
            fromMonth = fromMonth.length > 1 ? fromMonth : '0' + fromMonth;
            var fromYear = fromDt.getFullYear();
            var fromDate = frmdt + "/" + fromMonth + "/" + fromYear;
                  
            var noteTypeId = <%=Session["NoteType"].ToString()%>;   

             var refID= <%=Session["RefID"].ToString()%>; 
             
            GetNotes(fromDate, toDate, noteTypeId,refID,"");

        });

        function Search() {
   
            var fromDate = $('[id$="txtFromDate"]').val();
           

            var toDate = $('[id$="txtFromTo"]').val();        
           
                      
            var Reference = $('[id$="txtTeference"]').val();    

            //if(fromDate=="")
            //{
            //    var fromDt = new Date();
            //    fromDt.setMonth(fromDt.getMonth() - 1)
            //    var frmdt = new String(fromDt.getDate());
            //    frmdt = frmdt.length > 1 ? frmdt : '0' + frmdt;
            //    var fromMonth = fromDt.getMonth() + 1;
            //    fromMonth = fromMonth.length > 1 ? fromMonth : '0' + fromMonth;
            //    var fromYear = fromDt.getFullYear();
            //    fromDate = frmdt + "/" + fromMonth + "/" + fromYear;
            //}

            //if(toDate=="")
            //{
            //    var CurrentDate = new Date();
            //    var toDt = new String(CurrentDate.getDate());
            //    toDt = toDt.length > 1 ? toDt : '0' + toDt;
            //    var toMonth = new String(CurrentDate.getMonth() + 1);
            //    toMonth = toMonth.length > 1 ? toMonth : '0' + toMonth;
            //    var toYear = CurrentDate.getFullYear();
            //    toDate = toDt + "/" + toMonth + "/" + toYear;
            //}

                   
            var noteTypeId = <%=Session["NoteType"].ToString()%>;
            var refID= <%=Session["RefID"].ToString()%>;    
            GetNotes(fromDate, toDate,noteTypeId,refID,Reference);

        }

    </script>

    <style type="text/css">
         .displayNone { display: none; }

        .k-edit-form-container {
            width: 100%;
        }

        #details-container {
            padding: 10px;
        }

            #details-container h2 {
                margin: 0;
            }

            #details-container em {
                color: #8c8c8c;
            }

            #details-container dt {
                margin: 0;
                display: inline;
            }

        .ForeColor {
            color: red;
        }

        .k-textbox {
            width: 11.8em;
        }

        #tickets {
        }

            #tickets h3 {
                font-weight: normal;
                font-size: 1.4em;
                border-bottom: 1px solid #ccc;
            }

            #tickets ul {
                list-style-type: none;
                margin: 0;
                padding: 0;
            }

            #tickets li {
                margin: 10px 0 0 0;
            }

        label {
            display: inline-block;
            width: 90px;
            text-align: right;
        }

        .required {
            font-weight: bold;
        }

        .accept, .status {
            padding-left: 90px;
        }

        .valid {
            color: green;
        }

        .invalid {
            color: red;
        }

        span.k-tooltip {
            margin-left: 6px;
        }

        .note.error span {
            background: transparent url(../images/error.png) 0px 0px no-repeat;
        }

        .note.check span {
            background: transparent url(../images/check.png) 0px 0px no-repeat;
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
            background: #eceaea;
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
            background: #eceaea;
            color: black;
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


        .buttondel {
            background-image: url('images/delete.png');
        }

        /*.buttoncopy{
            background-color:gray;
        }*/
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

        input.ng-invalid-required {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }

        input.ng-invalid-pattern {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }

        .validdate {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }

        .k-textbox .k-icon {
            margin: -8px 0 0 -17px;
        }

        .center {
            text-align: center !important;
            margin: 0 auto;
            width: auto;
        }

        .text span.k-tooltip {
            display: table-caption;
            padding: 0 10px 0 0;
        }
    </style>
    <%--<style type="text/css">
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
    </style>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblNotes" Text="Notes" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                  
                    <div>

                        <table width="100%">
                            <tr>
                                 <td align="right">Reference:
                                                <input type="text" id="txtTeference"  runat="server" style="width: 125px" />
                                   
                                </td>

                               <td align="right">From Date:
                                                <input type="text" id="txtFromDate"  runat="server" onkeypress="return false;" style="width: 125px" />
                                  
                                </td>
                                <td align="center">To Date:
                                                <input type="text" id="txtFromTo" runat="server" onkeypress="return false;" style="width: 125px" />
                                       
                                </td>
                                <td align="left">
                                    <span id="Span1" runat="server" onclick="Search();" class="small_button white_button open">Search</span>
                                    &nbsp;&nbsp;&nbsp;
                                     <span id="clear" runat="server" onclick="Clear();" class="small_button white_button open">Clear</span>
                                    
                                </td>
                                
                                <td align="right">
                                    <span id="SpanAddNote" runat="server" onclick="ShowAddPopup();"  class="small_button white_button open" >Add Note</span>
                                </td>
                            </tr>
                        
                        </table>
                    </div>
                    <div class="clear"></div>
                </div>
                <div id="gridNote"></div>
              
            </div>
        </div>
    </div>
    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; width: 615px; min-height: 50px; top: 17%; left: 625px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Note</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()" alt="Close" />
                <div class="clear">
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>

                    <th><span style="color: red">*</span> Reference:</th>
                    <td align="center">

                        <asp:DropDownList runat="server" ID="ddlReference" Style="width: 200px" onchange="return FillProfileData();">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredReference"
                            ControlToValidate="ddlReference" InitialValue="0" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>Notes:</th>
                    <td align="center" >
                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="150px" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span style="color: red; float: left;">Fields marked with * are mandatory</span><span class="clear"></span>
                    </td>
                    <th></th>
                    <td colspan="2">
                        <div id="Div1" runat="server" style="text-align: right">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" CausesValidation="true" ValidationGroup="validate" OnClick="btnSave_Click" CssClass="small_button white_button" ClientIDMode="Static" UseSubmitBehavior="false" />

                            <input type="button" class="small_button white_button open" id="btnCancel" value="CANCEL"   onclick="closeAddPopUP()"/>
                       
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <input type="hidden" id="noteTypeId"  runat="server"/>

</asp:Content>

