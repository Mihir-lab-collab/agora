<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="SkillMatrix.aspx.cs" Inherits="Member_SkillMatrix" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Member/controls/EmployeeSkill.ascx" TagName="ucEmpSkill" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>--%>
    <%--<link href="css/draggableCss.css" rel="stylesheet" />--%>
    <%--<script src="js/DraggbleJs.js"></script>--%>   

    <%--Bellow links are for Kendo --%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/SkillMatrix.js" type="text/javascript"></script>


    <script type="text/javascript">

        function call() {
            alert("Saved Successfully");
        }
        function CheckSkillOnInsert(Buttonid) {
            var skill = $('[id$="txtSkillName"]').val();
            var errSkill = $("#lblerrmsgSkill");

            if (skill == "") {
                errSkill.html("Please enter Skill.");
                $("#lblerrmsgSkill").css('display', '');
                return false;
            }
        }
        function GetDataOnInsert(Buttonid) {
            //var skill = $('[id$="txtSkillName"]').val();
            var category = $('[id$="txtCategoryName"]').val();
            //var Time = $('#timepicker').val(); 

            var errHDate = $("#lblerrmsgdate");
            //var errSkill = $("#lblerrmsgSkill");
            if (category == "") {
                errHDate.html("Please enter category.");
                $("#lblerrmsgdate").css('display', '');
                return false;
            }
            //if (skill == "") {
            //    errSkill.html("Please enter Skill.");
            //    return false;
            //}
            <%-- else
         {
                errHDate.html("");
            }

            var errNarration = $("#lblerrmsgnarration");
            if (narration == "") {
                errNarration.html("Please Enter Description.");
                return false;
            }
            else {
                errNarration.html("");
            }

            if (EventDate != "") {
                document.getElementById("<%=hdnEventDate.ClientID%>").value = EventDate;                 
            }
            if (narration != "") {
                document.getElementById("<%=hdnDescription.ClientID%>").value = narration;                 
            }
            document.getElementById("<%=hdnTime.ClientID%>").value = Time;
         --%>
        }
    </script>

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
    </style>

 
 <%--<script>
     $(function () {
         $("#divEmpSkill").dialog();
     });
  </script>
    --%>
<%--  <script type="text/javascript"> // Div draggable
      $(function () {
          $('body').on('mousedown', '#divEmpSkill', function () {
              $(this).addClass('draggable').parents().on('mousemove', function (e) {
                  $('.draggable').offset({
                      top: e.pageY - $('.draggable').outerHeight() / 2,
                      left: e.pageX - $('.draggable').outerWidth() / 2
                  }).on('mouseup', function () {
                      $(this).removeClass('draggable');
                  });
              });
              e.preventDefault();
          }).on('mouseup', function () {
              $('.draggable').removeClass('draggable');
          });
      });
  </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="temp"></div>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblHoliday" Text="Skill Matrix" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblLocation" Text="" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>Enter Skills:
                                                <asp:TextBox ID="txtSearchSkill" runat="server" Text=""></asp:TextBox>
                                                &nbsp;&nbsp;&nbsp;
                                                <span id="spnSearch" runat="server" onclick="SearchSkills();" class="small_button white_button open">Search</span>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <span id="Span1" runat="server" onclick="ShowCategoryPopup();" class="small_button white_button open">Add Category</span>&nbsp;&nbsp;&nbsp;
                                                <span id="spn" runat="server" onclick="ShowSkillPopup();" class="small_button white_button open">Add Skill</span>&nbsp;&nbsp;&nbsp;
                                                <span id="Span2" runat="server" onclick="ShowEmpSkill();" class="small_button white_button open">View Employee Skill</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="gridEvents"></div>
            </div>
        </div>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divCategeoryPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 400px; border-color: black; border-width: thin; min-height: 150px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Category</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeCategoryPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Category:</th>
                    <td align="center">
                        <input id="txtCategoryName" runat="server" name="txtCategoryName" style="width: 200px;" validationmessage="Please enter Category" class="k-textbox" />
                        &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                        <span id="lblerrmsgdate" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="btnSaveCategory" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="btnSaveCategory_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancelCategory" onclick="closeCategoryPopUP();" />
                    </td>
                </tr>

            </table>
            <div>&nbsp;</div>
        </div>
    </div>

    <div class="k-widget k-windowAdd" id="divSkillPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 400px; border-color: black; border-width: thin; min-height: 195px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Skill</h3>
                <span id="spnMsg" style="color:red; padding:14px 22% 0; display:inline-block;">saved successfully</span>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeSkillPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                        <tr>
                            <th>Select category:</th>
                            <td align="center">
                                <%--<select id="ddlCategory" runat="server" name="ddlCategory" >
                        </select>--%>
                                <asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <th>Skill Name:</th>
                            <td align="center">
                                <input id="txtSkillName" runat="server" name="txtSkillName" style="width: 200px;" validationmessage="Please enter skill name" class="k-textbox" />
                                &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                                <span id="lblerrmsgSkill" style="color: Red;"></span>
                            </td>
                        </tr>

                        <tr>
                            <th></th>
                            <td>
                                <asp:Button ID="btnSaveMore" runat="server" Text="Save & AddMore" CssClass="small_button red_button open" OnClientClick="javascript:return CheckSkillOnInsert(this.id);" OnClick="btnSaveSkill_Click" />
                                &nbsp;&nbsp;&nbsp;<asp:Button ID="btnSaveSkill" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return CheckSkillOnInsert(this.id);" OnClick="btnSaveSkill_Click" />
                                &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancelSkill" onclick="closeSkillPopUP();" />
                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>&nbsp;</div>
        </div>
    </div>

    <div class="k-widget k-windowAdd"  id="divEmpSkill" style="display: none;  padding-top: 10px; padding-right: 10px; min-width: 800px; border-color: black; border-width: thin; min-height: 510px; top: 10%; left: 350px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Employee Skill</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeEmpSkill()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <div>
                <UC:ucEmpSkill ID="ucEmpSkills" runat="server"></UC:ucEmpSkill>
            </div>
        </div>
    </div>

    <div class="k-widget k-windowAdd" id="divEmpDetail" style="display: none; padding-top: 10px; padding-right: 10px; width: 700px; min-width: 400px; border-color: black; border-width: thin; min-height: 310px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Employee Detail</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeEmpDetail()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <div>
                <b>Category :
                    <label id="lblCategory"></label>
                </b>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <b>Skill :
                    <label id="lblSkill"></label>
                </b>
                <br />
            </div>
            <div id="gridDetail_"></div>
        </div>
    </div>

    <asp:HiddenField ID="hdnSkillID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnLoginID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCategoryID" runat="server" />
    <asp:HiddenField ID="hdnEmpID" runat="server" />
    <%--<asp:HiddenField ID="hdnTime" runat="server" />--%>
</asp:Content>




