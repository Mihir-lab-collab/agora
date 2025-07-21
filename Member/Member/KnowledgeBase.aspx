<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="KnowledgeBase.aspx.cs" Inherits="Member_KnowledgeBase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>

    <script src="../Member/js/KnowledgeBase.js" type="text/javascript"></script>
    <%--  <script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>--%>

    <script type="text/javascript">
        $(document).ready(function () {

        });

        var grid = "#grid";
        function GetDataOnInsert() {
            var kbDate = $("#txtkbDate").val();
            $('[id$=hdnkbDate]').val(kbDate);

            var kbTitle = $("#txtkbTitle").val();
            $('[id$=hdnkbTitle]').val(kbTitle);

            var kbDescrptn = $("#txtkbDescrptn").val();
            $('[id$=hdnkbDescrptn]').val(kbDescrptn);

            var kbComments = $("#txtkbComments").val();
            $('[id$=hdnkbComments]').val(kbComments);

            var projId = $("#EditprojName").val();
            $('[id$=hdnprojId]').val(projId);

            var Url = $("#txtUrl").val();
            $('[id$=hdnUrl]').val(Url);

            var techId = $("#txttechName").val();
            $('[id$=hdntechId]').val(techId);

            var subtechName = $("#txttags").val();
            subtechName = replaceall(subtechName, ',', '$');
            $('[id$=hdnsubtechName]').val(subtechName);

            var empNameSpan = $("#lblempName");
            var kbTitleSpan = $("#lblkbTitle");
            var kbDescrptnSpan = $("#lblkbDescrptn");
            var techNameSpan = $("#lbltechName");
            var urlSpan = $("#lblUrl");
            urlSpan.html("");

            if (kbTitle == "") {
                kbTitleSpan.html("Please enter Title.");
                return false;
            }
            else {
                kbTitleSpan.html("");
            }
            if (kbDescrptn == "") {
                kbDescrptnSpan.html("Please enter Desciption.");
                return false;
            }
            else {
                kbDescrptnSpan.html("");
            }

            if (techId == "") {
                techNameSpan.html("Please Select Technology Name.");
                return false;
            }
            else {
                techNameSpan.html("");
            }

            if ($('[id$=txtUrl]').val() != "") {
                if (check_it('Add') == false) {
                    return false;
                }
            }

            if (kbTitle != "" && kbDescrptn != "" && techId != "") {
                return true;
            }

        }


        function check_it(str) {
            // var tomatch = /http:\/\/[A-Za-z0-9\.-]{3,}\.[A-Za-z]{3}/
            // var tomatch = /(ftp|http|https|www):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
            var tomatch = /^(?:([A-Za-z]+):)?(\/{0,3})([0-9.\-A-Za-z]+)(?::(\d+))?(?:\/([^?#]*))?(?:\?([^#]*))?(?:#(.*))?$/
            var testemail = '';
            var urlspan = '';
            if (str == 'Add') {
                testemail = $('[id$=txtUrl]').val();
                urlspan = $("#lblUrl");
            }
            else if (str == 'Edit') {
                testemail = $('[id$=EditUrl]').val();
                urlspan = $("#lbleditUrl");
            }
            if (!tomatch.test(testemail)) {
                urlspan.html("Please enter valid Url.");
                if (str == 'Add')
                    $('[id$=txtUrl]').val('');
                else
                    $('[id$=EditUrl]').val('');
                return false;
            }
            else {
                urlspan.html("");
                return true;

            }
        }

    </script>


    <script type="text/javascript">
        $(document).ready(function () {
            $(document).on('click', '.DivShowEditor', function (e) {
                var NextDiv = $(this).next();
                var TxtObj = $(this).parent().find('textarea');
                var Text = TxtObj.val();
                TxtObj.kendoEditor();
                $(this).hide();
                NextDiv.show();
            });
            $(document).on('click', '.DivHideEditor', function (e) {
                var PrevDiv = $(this).prev();
                var TxtObj = $(this).parent().find('textarea');
                var Text = TxtObj.val();
                var parentTable = TxtObj.parents('table:first');
                TxtObj.removeProp('data-role');
                TxtObj.removeProp('autocomplete');
                TxtObj.insertAfter($(this));
                TxtObj.show();
                var ss = htmlDecode(Text);
                TxtObj.val(ss);
                parentTable.remove();
                $(this).hide();
                PrevDiv.show();
            });
        });
        function htmlDecode(value) {
            return $('<div/>').html(value).text();
        }
        function callEditor() {
            var value = $('[id$="txtValue"]');
            var value1 = $('[id$="txtValue1"]');

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

            .k-grid tbody .k-button, .k-ie8 .k-grid tbody button.k-button {
              min-width: 0px; 
            }
        /*rows*/

        .k-grid-content > table > tbody > tr {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: nowrap;
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

        .ViewData {
            background-image: url('images/zoom.png');
            min-width: 10px;
            width: 20px;
            height: 17px;
            background-size: 20px;
            display: inline-block;
        }
        /*article*/
        .article    { margin:10px auto; padding:0 100px; border:none;}
        .article td {
            padding: 10px !important;
            border-bottom: none;
            border-right: none;
        }
        .article p  { font-size:14px; line-height:1.625em; padding:0.5em 0 1em;}
        .article p b { color:#373737;}
        .k-windowAdd {
            width: 100%;
            max-width: 1024px;
            min-width: 1000px;
            left: 20% !important;
            top:10% !important;
        }

        .article h1 { float:left; width:94%;
            text-align: left;
            color: #111;  font-size: 30px;  line-height: 36px; margin-bottom:-10px;
        }
        .article .comment_link a       { background:url('images/comment_icon.png') right 9px no-repeat; float:right; text-indent:-9999px; width:34px; height:36px; text-align:right; display:inline-block; }
        .article .comment_link a:hover { background:url('images/comment_hover.png') right 9px no-repeat;}
        
        .article h1:hover {color: #252e34;}
        .article td b, .comment_box b { color:#f57b19;}
        .article td b:hover { color:#f57619;}
        #getcmmnthistory    { padding:0 10px 0 50px; overflow:hidden;} 

        .kB_leftpanel       { width:720px; margin-right:25px; float:left;}
        .kB_rightpanel      { width:250px; float:right; margin-top:47px;}

        .kbdetails_main        { height:612px; overflow-y:scroll; }
        .comment               { position:relative; margin-bottom:15px;  border:#ddd 1px solid; text-align: left; background:#f6f6f6; border-radius: 3px; padding:12px;   }
        .comment::after        { width: 0; content:""; height: 0; border-bottom: 18px solid transparent; border-right:23px solid #ddd; position:absolute; left:-24px; top:15px; z-index:100000;}
          .red_button:hover     { color:#fff;}  
          #lblUrl a             { word-break:break-all;}  
    </style>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <span style="font-size: medium; font-weight: bold">Knowledge Base</span>
                            </td>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>

                                            <input type="text" id="searchbox" name="searchbox" />

                                        </td>
                                        <td style="width: 100px;">
                                            <input type="button" id="btnsearch" value="Search" class="small_button white_button open" onclick="GetKnowledgeBaseDetails(searchbox.value);" />
                                        </td>
                                        <td>
                                            <span id="spn" runat="server" onclick="ShowAddPopup();" class="small_button white_button open">Add New</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="grid"></div>
            </div>
        </div>
    </div>
  
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 100px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add New</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" width="100%" class="manage_form ">
                <tr>
                    <th>Project Name :
                    </th>
                    <td>
                        <input id="EditprojName" />
                    </td>
                </tr>
                <tr>
                    <th>Employee Name :
                    </th>
                    <td>
                        <asp:Label ID="txtkbempName" runat="server" Style="width: 300px; height: 30px;" class="k-textbox" Text="Label"></asp:Label>

                        <span id="empSpn" style="color: Red;">*</span>
                        <span id="lblempName" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Title :
                    </th>
                    <td>
                        <input id="txtkbTitle" type="text" style="width: 300px;" class="k-textbox" />

                        <span id="lblkbTitle" style="color: Red;">*</span>
                    </td>
                </tr>
                <tr>
                    <th>Description : </th>
                    <td id="val">
                        <div style="width: 520px;">
                            <div class="DivShowEditor small_button red_button">HTML</div>
                            <div class="DivHideEditor small_button red_button" style="display: none;">HTML</div>
                            <textarea id="txtkbDescrptn" runat="server" style="width: 500px; height: 100px;" rows="20" cols="70" class="k-textbox"></textarea>

                            <span id="lblkbDescrptn" style="color: Red;">*</span>
                        </div>

                    </td>
                </tr>
                <tr>
                    <th>TechName :
                    </th>
                    <td>
                        <input id="txttechName" type="text" rows="5" style="width: 300px;" class="k-textbox"></input>

                        <span id="lbltechName" style="color: Red;">*</span>
                    </td>
                </tr>
                <tr>
                    <th>Tags :
                    </th>
                    <td>
                        <input id="txttags" style="width: 300px;"></input><br />
                        <span style="color: orange">*Seperated By Commas</span>

                    </td>
                </tr>
                <tr>
                    <th>Upload File :
                    </th>
                    <td>
                        <input id="txtkbFile" type="file" rows="10" style="width: 300px;" class="k-textbox"></input>
                    </td>
                </tr>
                <tr>
                    <th>Url : 
                    </th>
                    <td>
                        <input id="txtUrl" style="width: 300px;"></input>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:LinkButton ID="lnkKnowledge" runat="server" Text="Save" OnClientClick="javascript:return GetDataOnInsert();" CssClass="small_button red_button open" OnClick="lnkKnowledgen_Click"></asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div id="KBDetails" class="k-widget k-windowAdd" style="display: none; padding-left: 10px; padding-right: 10px; padding-top: 12px; padding-bottom: 40px; min-width: 600px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

        <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
        <div class="kbdetails_main">
              <div class="kB_leftpanel">
                <table cellpadding="0" cellspacing="0" border="0" width="90%" class="manage_form article">

                    <tr>
                        <td colspan="4" align="center" class="comment_link" >
                            <h1>
                                <label id="lblTitle" />
                               
                            </h1>
                            <a href="#" onclick="toggle_visibility('getcmmnthistory');" >   </a>
                        </td>
                    </tr>
                    <tr style="border-bottom:#eee 1px solid; padding-bottom:12px; margin-bottom:20px;">
                        <td style="display:inline-block; min-width:20%;">
                            Posted by: &nbsp;
                            <b><label id="lblName" /></b>
                        </td>
                        <td style="display:inline">
                            On 
                            <b><label id="lblDate" /></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="100px" width="85%" class="align" style="vertical-align: top; ">
                            <b style="display: block; color:#373737; ">Description :</b>
                            <p id="lblDescrptn" />
                        </td>
               
                    </tr>
                    <tr style="display: none;">
                        <td>
                            <label id="lblId" />
                        </td>
                    </tr>
                </table>

                <div id="getcmmnthistory" style="display:none" >
                    <div id="history" class="comment_box">
                    </div>
                </div>
                    <div style="width: 520px; margin-top: 20px; padding:0 60px;">
          <%--  <div class="DivShowEditor small_button red_button">HTML</div>
            <div class="DivHideEditor small_button red_button" style="display: none; margin-left: 20px;">HTML</div>--%>
            <textarea id="Textarea1" runat="server" style="width: 500px; height: 60px; padding:10px;"  rows="15" cols="70" class="k-textbox"></textarea>
            <span style="color: Red;">*</span>
            <span id="lblComment" style="color: Red;"></span>
            <asp:LinkButton ID="lnkHistory" runat="server" Text="post" CssClass="small_button red_button open" OnClick="lnkHistory_Click" Style="margin-top: 18px;"></asp:LinkButton>

        </div>
                </div>

              <div class="kB_rightpanel">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form article">

                    <tr>
                        <td colspan="2" style="display: block;" height="70px">
                            <div>
                                <span>Uploaded File : </span>
                                <div id="attachment">
                                </div>
                        </td>
                        <td style="display: block;" colspan="2" height="43px">
                            <div>
                                <span>Uploaded Url : </span>
                                <label id="lblUrl"></label>
                            </div>
                        </td>
                    </tr>
                </table>
            
        </div>

        <div class="clear"></div>
        </div>
        

      

    </div>

    <script type="text/x-kendo-template" id="popup-editor">
   <table cellpadding="0" cellspacing="0" border="0" width="600px" class="manage_form">
               <tr id="trConfig" class="manage_bg">
        </tr>
                    <th>Title:</th>
                    <td align="center">                      
                        <input id="EditTitle"  data-bind="value:kbTitle" style="width: 300px;" />
                    </td>
                </tr>
            <tr>
                               <th>Description :
                    </th>
                    <td id="val">
                        <div style="width: 520px;">
                            <div class="DivShowEditor small_button red_button">HTML</div>
                            <div class="DivHideEditor small_button red_button" style="display: none;">HTML</div>
                            <textarea id="EditkbDescrptn" data-bind="value:kbDescrptn" runat="server" style="width: 500px; height: 100px;" rows="20" cols="70"  class="k-textbox"></textarea>                           
                        </div>
                    </td>
                </tr>        
         <tr>
                    <th>View :</th>
                    <td align="center">
            <div id="attachment"></div>
                 
                </tr>
             <tr>
                    <th>URL :</th>
                    <td align="center">
            <div id="attachUrl">
                    </div>           
                    </td>
                </tr>
            <tr>
                    <th>New URL:</th>
                    <td align="center">
                      <input type="text" id="EditUrl"  data-bind="value:Url"  onchange="return check_it('Edit');" style="width: 300px"/> 
                        <span style="color: Red;">*</span>
                        <span id="lbleditUrl" style="color: Red;"></span>                        
                    </td>
                </tr>         
        <tr>
                    <th>Add File</th>
                    <td align="center">
                        <input id="EditkbFile" type="file" />
                    </td>
                </tr>
      
           <tr>
                    <th>techId :</th>
                    <td align="center">
                        <input id="EdittechId"  style="width: 300px"/> 
                    </td>
                </tr>
           <tr>
                    <th>Tags :
                    </th>
                    <td>
                        <input id="Edittags" style="width: 300px;" data-bind="value:subtechName"></input>
                    </td>
                </tr> 
      
               <tr>
                <th></th>
                <td>
                    <div id="tdUpdate"></div>
                </td>
            </tr>

            </table>
    </script>

    <asp:HiddenField ID="hdnkbId" runat="server" />
    <asp:HiddenField ID="hdnempName" runat="server" />
    <asp:HiddenField ID="hdnkbTitle" runat="server" />
    <asp:HiddenField ID="hdnkbDescrptn" runat="server" />
    <asp:HiddenField ID="hdnkbComments" runat="server" />
    <asp:HiddenField ID="hdnkbFile" runat="server" />
    <asp:HiddenField ID="hdnkbDate" runat="server" />
    <asp:HiddenField ID="hdnempId" runat="server" />
    <asp:HiddenField ID="hdntechId" runat="server" />
    <asp:HiddenField ID="hdnsubtechName" runat="server" />
    <asp:HiddenField ID="hdnprojId" runat="server" />
    <asp:HiddenField ID="hdnUrl" runat="server" />
    <asp:HiddenField ID="hdnempNameHistory" runat="server" />
    <asp:HiddenField ID="hdncmmnthistory" runat="server" />

     <asp:HiddenField ID="hdn" runat="server" />
    
    </div>
</asp:Content>

