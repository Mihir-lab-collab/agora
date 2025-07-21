<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Technologykeyword.aspx.cs" Inherits="Member_Technologykeyword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Member/js/kendo.web.min.js" type="text/javascript"></script>
    <link href="../styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../js/cultures/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/Technologykeyword.js" type="text/javascript"></script>
      <style type="text/css">
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
    </style>
    <style type="text/css">
        .k-textbox {
            width: 11.8em;
        }

        #tickets {
            /*width: 510px;
                    height: 323px;
                    margin: 30px auto;
                    padding: 10px 20px 20px 170px;
                    background: url('../../content/web/validator/ticketsOnline.png') transparent no-repeat 0 0;*/
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

        .k-grid-content > table > tbody > tr {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }


        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0!important;
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
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*for history grid [e]*/
    </style>
     <script type="text/javascript" language="javascript">
        
         $(document).ready(function () {

             //var gridResult = $("#gridtechnologykeyword").kendoGrid({
             //    dataSource: { data: $('[id$=hdntechdata]').val() },
             //    scrollable: true,
             //    sortable: true,
             //    filterable: true,
             //    pageable: {
             //        input: true,
             //        numeric: false
             //    },
             //    columns: [
             //        {
             //            field: "techId",
             //            title: "techId"
             //        },
             //        {
             //            field: "techName",
             //            title: "techName"
             //        },
             //        {
             //            field: "subTechName"
             //        },
            
             //    ]
             //});
           //  change event


             //$('[id$=searchbox]').keyup(function () {
             //    var val = $('[id$=searchbox]').val();
             //    $("#gridtechnologykeyword").data("kendoGrid").dataSource.filter({
             //        logic: "or",
             //        filters: [
             //            {
             //                field: "techId",
             //                operator: "contains",
             //                value: val
             //            },
             //            {
             //                field: "techName",
             //                operator: "contains",
             //                value: val
             //            }
             //        ]
             //    });
             //});



            //$("#searchbox").keyup(function () {
            //    //$filter = new Array();
            //    //$x = $(this).val();
            //    //if ($x) {
            //    //    $filter.push({ field: "techId", operator: "contains", value: $x });
            //    //    $filter.push({ field: "techName", operator: "contains", value: $x });

            //    //}

            //    //gridResult.datasource.filter($filter);

            //    //$("#gridtechnologykeyword").data("kendoGrid").dataSource.filter({
            //    //    logic: "or",
            //    //    filters: [
            //    //        {
            //    //            field: "techId",
            //    //            operator: "contains",
            //    //            value: val
            //    //        },
            //    //        {
            //    //            field: "techName",
            //    //            operator: "contains",
            //    //            value: val
            //    //        },
            //    //        {
            //    //            field: "subTechName",
            //    //            operator: "contains",
            //    //            value:val
            //    //        },
                        
            //    //    ]
            //    //});

            //});
         });

       
 
         function GetDataOnInsert(Buttonid) {

             var techname = $("#txttechName").val();
             var subtechname = $("#txtsubTechName").val();
          

             var techSpan = $("#lblerrmsgtechName");
             var SubtechSpan = $("#lblerrmsgsubTechName");

             if (techname == "") {
                 techSpan.html("Please enter Technology Name.");
                 return false;
             }
             else {
                 techSpan.html("");
             }
             //if (subtechname == "") {
             //    SubtechSpan.html("Please enter Subtechnology name.");
             //    return false;
             //}
             //else {
             //    SubtechSpan.html("");
             //}
             if (techname != "" && subtechname != "") {
                 document.getElementById("<%=hfTechname.ClientID%>").value = techname;
                return true;
            }

         }

         function searchgrid() {
             var val = $('[id$=searchbox]').val();        
             var data = $("#gridtechnologykeyword").data("kendoGrid").dataSource;

             data.filter([
     {
         "logic": "or",
         "filters": [
             {
                 "field": "techId",
                 "operator": "eq",
                 "value": val
             },
             {
                 "field": "techName",
                 "operator": "eq",
                 "value": val
             }
         ]
     },
             ]);




             //$("#gridtechnologykeyword").data("kendoGrid").dataSource.filter({
             //    logic: "or",
             //    filters: [
             //        {
             //            field: "techId",
             //            operator: "contains",
             //            value: val
             //        },
             //        {
             //            field: "techName",
             //            operator: "contains",
             //            value: val
             //        }
             //    ]
             //});
         }
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                 <div class="grid_head">
                    <table width="100%">
                        <tr>
                           <%-- <td align="left">

                                <asp:Label ID="lblCusomerModule" Text="Task Manager" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>--%>
                            <td align="right">
                                <table>
                                    <tr>
                                        <%--    <td align="left">
                                            <span id="spnTerminated" style="font-size: small;">
                                                <asp:CheckBox ID="CheckBox1" runat="server" Width="200px" Text="Include Terminated Tasks"  AutoPostBack="true" />
                                            </span>
                                        </td>--%>

                                        <td>
                                        <input type="text" id="searchbox" name="searchbox" />
                                        </td>
                                        <td style="width:100px;">
                                            <input type="button" id="btnsearch" value="Search" class="small_button white_button open" onclick="GetTechnologyDetails(searchbox.value);"
                                        </td>
                                        <td style="width:10px;"></td>
                                       
                                        <td>
                                            <span id="spn" runat="server" onclick="ShowAddPopup()" class="small_button white_button open">Add New</span>
                                        </td>
                                        <td>
                                            <span id="PrevMonth" onclick="GetReport()" class="small_button white_button open" runat="server" style="display: none">Report</span>
                                            <%--<span id="PrevMonth" onclick="GetReport()" class="small_button white_button open" style="display:block;">Report</span>--%> <%--added by pravin--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
               <!--- -->
                <div id="gridtechnologykeyword"></div>
            </div>
        </div>
    </div>


    <!-- for Add new record popup -->

        <div id="divAddPopupOverlay" runat="server"></div>
    <%--<div class="a_popbox" id="divAddPopup" style="display: none;">--%>
    <%-- <div class="popup_wrap" style="width: 600px; top: 10%; left: 30%; overflow-y: auto; overflow-x: hidden; position: fixed">--%>
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 656px; z-index: 10003; opacity: 1; transform: scale(1); text-align:center" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add New</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
              <tr>
                  <th>Technology Name</th>
                  <td>
                      <input id="txttechName" type="text" style="width:200px"  onkeypress="return isChar(event,this);" class="k-textbox" />
                       <span style="color: Red;">*</span>
                        <span id="lblerrmsgtechName" style="color: Red;"></span>
                  </td>
              </tr>
                <%-- <tr>
                  <th>Sub Technology</th>
                  <td>
                      <input id="txtsubTechName" type="text" style="width:300px; height:100px;"  onkeypress="return isChar(event,this);" class="k-textbox" />
                       <span style="color: Red;">*</span>
                        <span id="lblerrmsgsubTechName" style="color: Red;"></span>
                  </td>
              </tr>     --%>          
                <tr>
                    <th></th>
                    <td>
                        <asp:LinkButton ID="lnkSaveNew" runat="server" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveNew_Click"><span>Save</span></asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>




     <script type="text/x-kendo-template" id="popup-editor">
          <div id="details-container">
           <table width="100%" cellpadding="0" cellspacing="0" class="manage_form">
            <tr id="trtechId" Style="display:none;"><td>#=techId#</td></tr>
          <tr id="trtech" class="manage_bg">
        <th>
                <label>Technology ID</label>
         </th>
         <td>#=techId#</td>
         </tr>
           <tr>
                    <th>Technology Name</th>
                    <td align="center">
                        <input id="EdittechName"  data-bind="value:techName"  style="width: 200px;height:25px" />                              
                    </td>
                </tr>
       <%--  <tr>
                    <th>SubTechnology</th>
                    <td align="center">
                        <input id="EditsubTechName"  data-bind="value:subTechName"  style="width: 300px;height:100px;" />                              
                    </td>
                </tr>--%>
         </table>
         </div>
         </script>
        <asp:HiddenField ID="hdnTechId" runat="server" />
     <asp:HiddenField ID="hdntechdata" runat="server" />
      <asp:HiddenField ID="hfTechname" runat="server" />
    <asp:HiddenField ID="hfsubTechname" runat="server" />
        <input type="hidden" id="hdn" class="hdnval" runat="server" />

</asp:Content>

