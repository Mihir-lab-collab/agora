<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="~/Member/Home.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <style type="text/css">
        .popup_head {
        }
            .popup_head h3 {
                padding-left: 0px;
            }

        #btn_AcceptReview {
            pointer-events: none;
        }

        .projectlist {
            width: 80%;
            border: 1px solid #ccc;
            border-collapse: inherit !important;
        }

            .projectlist tbody th {
                padding: 6px 10px;
            }

            .projectlist tbody td {
                padding: 6px 10px;
            }

                .projectlist tbody td.expandview {
                    padding: 0;
                }

            .projectlist tbody table {
                width: 97%;
                border: 1px solid #ccc;
            }

            .projectlist tbody td.expandview > div > div {
                padding: 10px 0;
            }

        .tooltip-inner {
            text-align: left;
            max-width: 215px;
            font-size: larger;
        }

        .tool-top-hover span {
            width: 50%;
            float: left;
        }

            .tool-top-hover span:nth-child(even) {
                color: #d70a0a;
            }

            .tool-top-hover span:nth-child(odd) {
                color: #fff;
                width: 45%;
            }


        .hidden {
            display: none;
        }

        .input {
            margin: 20px 0;
        }

        .tool-top-hover {
            display: none;
            position: relative;
        }

        .tool-tip {
            position: relative;
            text-align: left;
        }

            .tool-tip:hover > .tool-top-hover {
                display: block;
                position: absolute;
                left: 20%;
                top: 0;
                z-index: 999;
                background: rgba(247, 171, 36, 0.88);
                padding: 10px;
                border-radius: 8px;
            }

                .tool-tip:hover > .tool-top-hover:after {
                    width: 20px;
                    height: 20px;
                    border-top: 10px solid transparent;
                    border-bottom: 10px solid transparent;
                    border-right: 10px solid blue;
                    position: absolute;
                    left: -20px;
                    top: 50%;
                    height: 10px;
                    content "";
                }
    </style>

    <style type="text/css">
        #pdf-example {
            border: 1px solid black;
           
        }
    </style>

    <style type="text/css">
      #canvas_container {
          width: 1200px;
          height: 620px;          
          overflow: hidden;
          margin-left:10px;
          background:#ededed;
        text-align: center;
        border: solid 3px; 
      }
        #pdf_renderer {
            /*width:612px;
            height:600px;*/
            padding-top:10px;
            padding-bottom:10px;
        }
 
      /*#canvas_container {*/
        /*background: #333;*/
        /*background:#ededed;
        text-align: center;
        border: solid 3px;        
      }*/      
      .center
      {
        position: absolute;
        top: 15%;
        left: 20%;        
        margin-top: -30px;
        margin-left: -50px;
        /*top: 50%;
        left: 50%;        
        margin-top: -50px;
        margin-left: -50px;*/
        width: 100px;
        height: 100px;
       }        ​
       .fontFamily
        {
         font-family: "Times New Roman", Times, serif;
        }
        .customePrevLeft {
        left: 531px;
         top: 15px !important;
         background: url(../images/button-previous.png) no-repeat;
        }

        /*#pdf_renderer,#navigation_controls
        {
        font-size:10pt;
        font-family:Arial,sans-serif;
        }*/
        .btns {
            background-color:black;
            color:white;
            border-radius:5px;
            border-color:white;
            font-size:small;
            text-align:center;
            width:90px;
            margin:5px;            
        }
        .dynamic-scroller {
            width:615px !important;
            max-height:792px !important;
            overflow-y:auto !important;
}
        .pdnRight {
            padding-left: 16px !important;
        }
  </style>
 
   
    <%--<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.0.943/pdf.worker.min.js"></script>   
    <script src="../Member/js/pdf.min.js" type="text/javascript"></script>   
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.6.347/pdf_viewer.min.css" integrity="sha512-5cOE2Zw/F4SlIUHR/xLTyFLSAR0ezXsra+8azx47gJyQCilATjazEE2hLQmMY7xeAv/RxxZhs8w8zEL7dTsvnA==" crossorigin="anonymous" referrerpolicy="no-referrer" />--%>

    <script src="../Member/js/pdf.min.js" type="text/javascript"></script> 
    
    <script type="text/javascript">
        $(function () {
            Get_ReviewList();//Call By Shubh
            $('[id*=gvParentGrid] tr').each(function () {
                var toolTip = $(this).attr("title");
                $(this).find("td").each(function () {
                    $(this).simpletip({
                        content: toolTip
                    });
                });
                $(this).removeAttr("title");
            });
        });
        //Start By Shubh
        function Open_ReviewPopup() {
            $('#div_ReviewPopup').css('display', '');
        }
        function Get_ReviewList() {
            $.ajax({
                type: "POST",
                url: "Home.aspx/Get_ReviewList",
                contentType: "application/json;charset=utf-8",
                data: "{'Mode':'" + 'Get_ReviewListByEmpCode' + "'}",
                dataType: "json",
                async: true,
                success: function (msg) {
                    var data = msg.d;
                    if (data.length > 0) {
                        $('#lbl_Reviews').html(data);
                        Open_ReviewPopup();
                    }
                },
                error: function (x, e) {
                    alert("The call to the server side failed. " + x.responseText);
                }
            });
        }
        //End By Shubh

        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "images/minus_icon.png";
            } else {
                div.style.display = "none";
                img.src = "images/plus_icon.png";

            }
        }

        function OnlyNumericEntry(e, evnt) {
            var evt = e.which || window.event || evnt.which;
            if (evt) {
                var keyCode = evt.charCode || evt.keyCode || evnt.keyCode;
                if ((keyCode == 32) || (keyCode >= 47 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 122)) {
                    if (evt.preventDefault) {
                        evt.preventDefault();
                        Findlength(e.id, e);
                        validateTime(e.id);
                    }

                    return true;
                }
                else {
                    var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?";
                    for (var i = 0; i < $('#' + e.id).val.length; i++) {
                        if (iChars.indexOf($('#' + e.id).val().charAt(i)) != -1) {
                            $('#' + e.id).css("background-color", "red");
                            return false;
                        }
                    }
                    if ((keyCode >= 65 && keyCode <= 90)) {
                        $('#' + e.id).css("background-color", "red");
                        return false;
                    }
                }
            }
        }
        function Findlength(id, e) {
            var getlen = document.getElementById(id).value.length;
            if (getlen == '2') {
                validateTime(id);
                while (e.nextSibling) {
                    var e = e.nextSibling;
                    if (e.nodeType === 1 && e.tagName.toLowerCase() == "input") {
                        e.focus();
                        break;
                    }
                }
                if (e.nextSibling == null && $('#' + id).attr('class') != 'clsvalidateOutmnt') {
                    $('.clsvalidateOuthrs').focus();
                }
                else if ($('#' + id).attr('class') == 'clsvalidateOutmnt') {
                    $('.clscheck').focus();
                }
                return false;
            }
        }

        function validateTime(id) {
            var x = parseInt(document.getElementById(id).value);
            var Inhrs, Inmnt, Outhrs, Outmnt = false;
            if ($('#' + id).attr('class') == 'clsvalidateInhrs') {
                if (x >= 1 && x <= 24) {
                    Inhrs = true;
                    $('#' + id).css("background-color", "white");
                }
                else {
                    $('#' + id).css("background-color", "red");
                }
            }
            else if ($('#' + id).attr('class') == 'clsvalidateINmnt') {
                var inHrs = id.replace('txtCheckInMinute', 'txtCheckInHour');
                if (parseInt(document.getElementById(inHrs).value) < 24) {
                    if (x >= 0 && x <= 60) {
                        Inmnt = true;
                        if (parseInt(document.getElementById(inHrs).value) != 24 && x == 60) {
                            $('#' + id).val("00");
                            if (parseInt(document.getElementById(inHrs).value) >= 1 && parseInt(document.getElementById(inHrs).value) <= 23)
                                document.getElementById(inHrs).value = parseInt(document.getElementById(inHrs).value) + 1;
                        }

                        $('#' + id).css("background-color", "white");
                    }
                    else {
                        $('#' + id).css("background-color", "red");
                    }
                }
                else
                    $('#' + id).val("00");
            }
            else if ($('#' + id).attr('class') == 'clsvalidateOuthrs') {
                if (x >= 1 && x <= 24) {
                    Outhrs = true;
                    $('#' + id).css("background-color", "white");
                }
                else {
                    $('#' + id).css("background-color", "red");
                }
            }
            else if ($('#' + id).attr('class') == 'clsvalidateOutmnt') {
                var inHrs = id.replace('txtCheckOutMinute', 'txtCheckOutHour');
                if (parseInt(document.getElementById(inHrs).value) < 24) {
                    if (x >= 0 && x <= 60) {
                        Outmnt = true;
                        if (x == 60) {
                            $('#' + id).val("00");
                            if (parseInt(document.getElementById(inHrs).value) >= 1 && parseInt(document.getElementById(inHrs).value) <= 23)
                                document.getElementById(inHrs).value = parseInt(document.getElementById(inHrs).value) + 1;
                        }
                        $('#' + id).css("background-color", "white");
                    }
                    else {
                        $('#' + id).css("background-color", "red");
                    }
                }
                else
                    $('#' + id).val("00");
            }
        }

        function isSpclChar(str) {
            var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?";
            for (var i = 0; i < str.length; i++) {
                if (iChars.indexOf(str.charAt(i)) != -1) {
                    alert("special characters are not allowed.\n");
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        function Validatedate(id) {
            var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?";

            var dtBlock = $(id).closest(".clsdateblock");
            var Inhrs = $(dtBlock).find('.clsvalidateInhrs').val();
            var INmnt = $(dtBlock).find('.clsvalidateINmnt').val();
            var Outhrs = $(dtBlock).find('.clsvalidateOuthrs').val();
            var Outmnt = $(dtBlock).find('.clsvalidateOutmnt').val();

            if (Inhrs == "" || INmnt == "") {
                alert("Please enter In hours & In Time.");
                return false;
            }

            if ((isSpclChar(Inhrs) && isSpclChar(INmnt))) {
                if (Inhrs == "00" && INmnt == "00") {
                    alert("Please enter correct inputs.\n");
                    return false;
                }

                for (var i = 0; i < Inhrs.length; i++) {
                    if (iChars.indexOf(Inhrs.charAt(i)) != -1) {
                        alert("Special characters are not allowed.\n");
                        return false;
                    }
                }
                for (var i = 0; i < INmnt.length; i++) {
                    if (iChars.indexOf(INmnt.charAt(i)) != -1) {
                        alert("special characters are not allowed.\n");
                        return false;
                    }
                }

                if (Inhrs.length == 1)
                    $(dtBlock).find('.clsvalidateInhrs').val('0' + Inhrs)
                if (INmnt.length == 1)
                    $(dtBlock).find('.clsvalidateINmnt').val('0' + INmnt)
            }

            if (Outhrs != "" && Outmnt != "") {
                if (Outhrs == "00" && Outmnt == "00") {
                    alert("Please enter correct inputs.\n");
                    return false;
                }
                for (var i = 0; i < Outhrs.length; i++) {
                    if (iChars.indexOf(Outhrs.charAt(i)) != -1) {
                        alert("Special characters not allowed.\n");
                        return false;
                    }
                }
                for (var i = 0; i < Outmnt.length; i++) {
                    if (iChars.indexOf(Outmnt.charAt(i)) != -1) {
                        alert("Special characters not allowed.\n");
                        return false;
                    }
                }

                if ((isSpclChar(Outhrs) && isSpclChar(Outmnt))) {
                    if (Outhrs.length == 1)
                        $(dtBlock).find('.clsvalidateOuthrs').val('0' + Outhrs)
                    if (Outmnt.length == 1)
                        $(dtBlock).find('.clsvalidateOutmnt').val('0' + Outmnt)
                }
            }
            if (parseInt(Inhrs) > 23 || parseInt(INmnt) > 59 || parseInt(Outhrs) > 23 || parseInt(Outmnt) > 59) {
                alert('Please enter correct inputs.\n');
                return false;
            }

            if (isNaN(parseInt(Inhrs)) || isNaN(parseInt(INmnt))) {
                alert('characters not allowed.\n');
                return false;
            }
            if (Outhrs != "" || iOutmnt != "") {
                if (isNaN(parseInt(Outhrs)) || isNaN(parseInt(Outmnt))) {
                    alert('characters not allowed.\n');
                    return false;
                }

                var dt = new Date();
                var d1 = new Date(parseInt(dt.getYear()), (parseInt(dt.getMonth())), parseInt(dt.getDate()), parseInt(Inhrs), parseInt(INmnt), parseInt(00));
                var d2 = new Date(parseInt(dt.getYear()), (parseInt(dt.getMonth())), parseInt(dt.getDate()), parseInt(Outhrs, 10), parseInt(Outmnt), parseInt(00));

                if (d1 > d2) {
                    alert("Out time should be greater than In time.")
                    return false;
                }
            }
        }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="scriptmanagerHome" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanelHome" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <div class="content_wrap">
                <div class="gride_table">
                    <div class="grid_head" style="display: flex; justify-content: space-between; align-items: center;">
    <h2 style="margin: 0;">Dashboard</h2>
    <asp:Button ID="btnGoToEmployeeRemoteAtt" runat="server" Text="Remote Attendance" OnClick="btnGoToEmployeeRemoteAtt_Click" CssClass="small_button red_button open" />
</div>


                    <div class="accord">
                        <div id="divAlert" runat="server">
                            <div class="grid_head_b" style="color: #ff0131; font-size: 14px;">
                                <b>Alerts</b>
                            </div>

                            <div id="Div1" runat="server" style="font-size: 12px; margin-top: 10px; color: #ff0131;">Incomplete Timesheet:</div>                          
                            <asp:GridView ID="TSGrid" runat="server" Width="80%" CssClass="manage_gridb" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTSDate" runat="server" Text='<%# Eval("TSDate", "{0:dd-MMM-yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hours">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTSHour" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("AttTSHour").ToString())? "00:00" : Eval("AttTSHour")) %>' Style="color: #ff0131;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />

                            <div class="clear"></div>
                        </div>



                        <div id="divBirthday" runat="server">
                            <div class="grid_head_b" style="color: #ff0131; font-size: 14px;">
                                <b>Best Wishes: <%=DateTime.Now.ToString("dd-MMM-yyyy")%></b>
                            </div>
                            <div>
                                <div id="Bday" runat="server" style="font-size: 18px; margin-top: 10px;"></div>
                            </div>
                            <div class="clear"></div>
                        </div>

                        <div id="divAppraisal" runat="server">
                            <div class="grid_head_b" style="color: #ff0131; font-size: 14px;">
                                <b>Please Initiate the Performance Review:
                                    <asp:HyperLink href="EmployeeAppraisalInitiation.aspx" onclick='ShowAddPopup();' runat="server">Click Here!</asp:HyperLink></b>
                                <%--<span id="spn" runat="server" onclick="ShowAddPopup();" class="small_button white_button open">Add New</span>--%>
                            </div>
                            <div>
                                <div id="Div2" runat="server" style="font-size: 18px; margin-top: 10px;"></div>
                            </div>
                            <div class="clear"></div>
                        </div>

                        <div id="divEmpAppraisal" runat="server">
                            <div class="grid_head_b" style="color: #ff0131; font-size: 14px;">
                                <b>Please Review Yourself:
                                    <asp:HyperLink href="EmpSelfAppraisal.aspx" runat="server">Click Here!</asp:HyperLink></b>
                                <%--<span id="spn" runat="server" onclick="ShowAddPopup();" class="small_button white_button open">Add New</span>--%>
                            </div>
                            <div>
                                <div id="AppDiv" runat="server" style="font-size: 18px; margin-top: 10px;"></div>
                            </div>
                            <div class="clear"></div>
                        </div>

                        <%--  <div id="divMgrAppraisal" runat="server">
                            <div class="grid_head_b" style="color: #ff0131; font-size: 14px;">
                                <b>Please Appraise Employees: <asp:HyperLink href="EmpManagerAppraisal.aspx"  runat="server">Click Here!</asp:HyperLink></b>
                            </div>
                            <div>
                                <div id="AppDiv1" runat="server" style="font-size: 18px; margin-top: 10px;"></div>
                            </div>
                            <div class="clear"></div>
                        </div>--%>

                        <ajax:Accordion ID="accordionHome"
                            runat="Server"
                            HeaderCssClass="accord_head "
                            HeaderSelectedCssClass="accord_head_a"
                            ContentCssClass="accord_cont" FadeTransitions="false" RequireOpenedPane="false">
                            <Panes>
                                <ajax:AccordionPane runat="server" ID="AccordionProjectHealthAlert">
                                    <Header>
                                        <asp:Label ID="ProjectHealthAlert" runat="server" Style="color: #ff0131;"></asp:Label>
                                    </Header>
                                    <Content>
                                        <div id="ProjectHours" runat="server">
                                            <div id="ProjctHoursDetails" runat="server" style="font-size: 14px; margin-top: 10px; color: #ff0131;" class="input">The following projects have hours that have touched or exceeded 50% of the current milestone hours</div>
                                            <asp:GridView ID="gvParentGrid" runat="server" DataKeyNames="ProjID"
                                                AutoGenerateColumns="false" OnRowDataBound="gvUserInfo_RowDataBound" GridLines="None" BorderStyle="Solid" BorderWidth="1px" BorderColor="#2d2d2d" CssClass="projectlist">
                                                <HeaderStyle BackColor="#df5015" Font-Bold="true" ForeColor="White" />
                                                <RowStyle BackColor="#E1E1E1" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <HeaderStyle BackColor="#2d2d2d" Font-Bold="true" ForeColor="White" />
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <a href="JavaScript:divexpandcollapse('div<%# Eval("ProjID") %>');">
                                                                <img id="imgdiv<%# Eval("ProjID") %>" width="18px" border="0" src="images/plus_icon.png" />
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ProjID" ItemStyle-CssClass="hidden"
                                                        HeaderStyle-CssClass="hidden" />
                                                    <asp:BoundField DataField="ProjectManager" ItemStyle-CssClass="hidden"
                                                        HeaderStyle-CssClass="hidden" />
                                                    <asp:BoundField DataField="AccountManager" ItemStyle-CssClass="hidden"
                                                        HeaderStyle-CssClass="hidden" />
                                                    <%--   <asp:BoundField DataField="ProjID" HeaderText="Project Name" HeaderStyle-HorizontalAlign="Left" />--%>
                                                    <%--  <asp:BoundField DataField="projName" HeaderText="Project Name" HeaderStyle-HorizontalAlign="Left" />--%>
                                                    <asp:TemplateField HeaderText="Project Name" InsertVisible="False"
                                                        ShowHeader="False">
                                                        <ItemTemplate>
                                                            <div class="tool-tip">
                                                                <asp:HyperLink runat="server" ID="link1" class="tooltips" NavigateUrl='<%# Eval("ProjID", "~/Member/Milestone.aspx?projid={0}") %>'
                                                                    Text='<%# Eval("projName") %>' />
                                                                <div class="tool-top-hover">
                                                                    <span runat="server" id="PM"></span><span runat="server" id="PMName"></span>
                                                                    <span runat="server" id="AM"></span><span runat="server" id="AMName"></span>
                                                                    <span runat="server" id="BA"></span><span runat="server" id="BAName"></span>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="EstHours" HeaderText="Estimated Hours" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="ActualHours" HeaderText="Actual Hours" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td colspan="100%" class="expandview">
                                                                    <div id="div<%# Eval("ProjID") %>" style="display: none; position: relative; left: 15px; overflow: auto">
                                                                        <asp:GridView ID="gvChildGrid" runat="server" AutoGenerateColumns="false" BorderStyle="Double" BorderColor="#2d2d2d" GridLines="None">
                                                                            <HeaderStyle BackColor="#2d2d2d" Font-Bold="true" ForeColor="White" />
                                                                            <RowStyle BackColor="#E1E1E1" />
                                                                            <AlternatingRowStyle BackColor="White" />
                                                                            <HeaderStyle BackColor="#2d2d2d" Font-Bold="true" ForeColor="White" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Name" HeaderText="Milestone Name" HeaderStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="EstHours" HeaderText="Budgeted Hours" HeaderStyle-HorizontalAlign="Left" />

                                                                                <asp:BoundField DataField="ActualHours" HeaderText="Completed MS Hours" HeaderStyle-HorizontalAlign="Left" />
                                                                                <%--<asp:BoundField DataField="ComputedMSHours" HeaderText="MS Health" HeaderStyle-HorizontalAlign="Left" />--%>

                                                                                <asp:TemplateField HeaderText="MS Health" HeaderStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <%# Eval("ComputedMSHours") %>%
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                <asp:Label ID="lblEmptyMessage" Text="" runat="server" />
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <br />
                                        </div>
                                    </Content>
                                </ajax:AccordionPane>
                                <ajax:AccordionPane runat="server" ID="AccordionPendingProjectStatusAlert">
                                    <Header>
                                        <asp:Label ID="ProjectStatusAlert" runat="server" Style="color: #ff0131;"></asp:Label>
                                    </Header>
                                    <Content>
                                        <div id="ProjectStatus" runat="server">
                                            <div id="IncompeteProjectStatus" runat="server" style="font-size: 14px; margin-top: 10px; color: #ff0131;" class="input">List of projects whose status is not updated in a week:</div>
                                            <asp:GridView ID="IncompeteStatus" runat="server" Width="40%" CssClass="manage_gridb" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Project Name" HeaderStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("projName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date" HeaderStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblStatusDate" runat="server" Text='<%# Eval("ProjectStautusDate") %>' style="color: #ff0131;"/>--%>
                                                            <asp:Label ID="lblStatusDate" runat="server" Text='<%# (String.IsNullOrEmpty(Convert.ToString( Eval("ProjectStautusDate")))? "Status is never updated" :Eval("ProjectStautusDate", "{0:dd-MMM-yyyy}")) %>' Style="color: #ff0131;" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <br />
                                        </div>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane runat="server" ID="AccordionPane2">
                                    <Header>
                                        <asp:Label ID="lblEvents" runat="server"></asp:Label>
                                    </Header>
                                    <Content>
                                        <asp:GridView ID="grdCIP" runat="server" Width="100%" CssClass="manage_gridb" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Event Date" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEventDate" runat="server" Text='<%# Eval("EventDateTime").ToString() %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lbl" runat="server" Text="No CIP Sessions for this year."></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane ID="Panel5" runat="server">
                                    <Header>Calendar</Header>
                                    <Content>
                                        <asp:Label ID="lblAttendance" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>

                                        <div style="float: right; color: red; font-weight: bold;">
                                            Time should be in 24 hrs.
                                        </div>
                                        <table id="tableHeader" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="lbtnpre" runat="server" OnClick="lbtnpre_Click" OnClientClick="ShowLoading();"><b><< </b></asp:LinkButton>
                                                    <asp:Label ID="lblMonthYear" Font-Size="Large" runat="server"></asp:Label>
                                                    <asp:LinkButton ID="lbtnnext" runat="server" OnClick="lbtnnext_Click" OnClientClick="ShowLoading();"><b>>> </b></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>


                                        <asp:DataList ID="DataCalendar" runat="server" DataSourceID="MyCalendar" Style="width: 1008px; margin: auto;" RepeatColumns="7" RepeatDirection="Horizontal">
                                            <HeaderTemplate>
                                                <div style="text-align: center; background-color: GrayText;">
                                                    <table>
                                                        <tr align="center">
                                                            <td>
                                                                <asp:Label ID="Label6" Text="Sunday" runat="server" Width="174px" ForeColor="White" Font-Size="Large"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label0" Text="Monday" runat="server" Width="174px" ForeColor="White" Font-Size="Large"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label1" Text="Tuesday" runat="server" Width="174px" ForeColor="White" Font-Size="Large"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label2" Text="Wednesday" runat="server" Width="174px" ForeColor="White" Font-Size="Large"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label3" Text="Thursday" runat="server" Width="174px" ForeColor="White" Font-Size="Large"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label4" Text="Friday" runat="server" Width="174px" ForeColor="White" Font-Size="Large"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label5" Text="Saturday" runat="server" Width="174px" ForeColor="White" Font-Size="Large"></asp:Label>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="Panel2" CssClass="clsdateblock" runat="server" Width="175px" Height="135px" BorderWidth="1px" BackColor='<%#  System.Drawing.Color.FromName(Convert.ToString((Eval("Color")))) %>'>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <%-- <asp:LinkButton ID="AddTimesheet" runat="server" OnClick="AddTimesheet">Add Timesheet</asp:LinkButton>--%>
                                                                            <asp:Label ID="TextBox1" Text="IN" runat="server" Width="60" MaxLength="2" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>'></asp:Label>
                                                                            <asp:TextBox ID="txtCheckInHour" Text='<%# Convert.ToString(Eval("InHour")) %>' CssClass="clsvalidateInhrs" runat="server" Style="text-align: center;" Width="18" MaxLength="2" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>' onkeyup="OnlyNumericEntry(this,event);" Enabled='<%# Convert.ToBoolean(Eval("SaveVisible")) %>'></asp:TextBox>
                                                                            <asp:TextBox ID="txtCheckInMinute" Text='<%# Convert.ToString(Eval("InMinute")) %>' CssClass="clsvalidateINmnt" runat="server" Style="text-align: center;" Width="18" MaxLength="2" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>' onkeyup="OnlyNumericEntry(this,event);" Enabled='<%# Convert.ToBoolean(Eval("SaveVisible")) %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="TextBox2" Text="OUT" runat="server" Width="60" MaxLength="3" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>'></asp:Label>
                                                                            <asp:TextBox ID="txtCheckOutHour" Text='<%# Convert.ToString(Eval("OutHour")) %>' CssClass="clsvalidateOuthrs" runat="server" Style="text-align: center;" Width="18" MaxLength="2" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>' onkeyup="OnlyNumericEntry(this,event);" Enabled='<%# Convert.ToBoolean(Eval("SaveVisible")) %>'></asp:TextBox>
                                                                            <asp:TextBox ID="txtCheckOutMinute" Text='<%# Convert.ToString(Eval("OutMinute")) %>' CssClass="clsvalidateOutmnt" runat="server" Style="text-align: center;" Width="18" MaxLength="2" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>' onkeyup="OnlyNumericEntry(this,event);" Enabled='<%# Convert.ToBoolean(Eval("SaveVisible")) %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label8" Text="IN OFFICE" runat="server" Width="60" MaxLength="3" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>'></asp:Label>
                                                                            <asp:TextBox ID="TextBox3" Text='<%# Convert.ToString(Eval("TotWorkHour")) %>' CssClass="clsvalidateOuthrs" runat="server" Width="53" MaxLength="2" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>' onkeyup="OnlyNumericEntry(this,event);" Enabled='<%# Convert.ToBoolean(Eval("SaveVisible")) %>'></asp:TextBox>
                                                                            <%--<asp:TextBox ID="TextBox4" Text='<%# Convert.ToString(Eval("brkMinute")) %>' CssClass="clsvalidateOutmnt" runat="server" Width="15" MaxLength="2"  Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>' onkeyup="OnlyNumericEntry(this,event);" Enabled='<%# Convert.ToBoolean(Eval("SaveVisible")) %>'></asp:TextBox>--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblDay" runat="server" Text='<%# Eval("Day") %>' Font-Size="XX-Large" Visible='<%# Convert.ToBoolean(Eval("Visible")) %>' />
                                                            </td>

                                                        </tr>

                                                        <tr>
                                                            <td align="left">
                                                                <asp:LinkButton ID="btnSave" Text="Check-In" runat="server" CssClass="clscheck" OnClientClick="return Validatedate(this);" Visible='<%# Convert.ToBoolean(Eval("SaveVisible")) %>' OnClick="btnSave_Click"></asp:LinkButton>
                                                                <asp:Label ID="Label9" Text="BREAK" runat="server" Style="margin-left: 2px;" Width="60" MaxLength="3" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>'></asp:Label>
                                                                <asp:TextBox ID="TextBox4" Text='<%# Convert.ToString(Eval("brkHour")) %>' Style="margin-top: -2px;" CssClass="clsvalidateOuthrs" runat="server" Width="53" MaxLength="2" Visible='<%# !Convert.ToBoolean(Eval("btnHoliday")) %>' onkeyup="OnlyNumericEntry(this,event);" Enabled='<%# Convert.ToBoolean(Eval("SaveVisible")) %>'></asp:TextBox>
                                                            </td>

                                                            <td align="center">
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Font-Size="Medium" ForeColor="Red" />
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblTimesheetHours" runat="server" Text='<%# Eval("WorkingHours") %>' Font-Size="Small" ForeColor="Black" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div id="spnholiday" style="padding: 10px; color: red; text-align: center; font-weight: bold; padding-top: 0px; padding-bottom: 0px;">
                                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("HolidayLabel").ToString() %>'></asp:Label>
                                                    </div>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <asp:ObjectDataSource ID="MyCalendar" runat="server" SelectMethod="GetDays" TypeName="clsCalendar" OnSelecting="MyCalendar_Selecting">
                                            <SelectParameters>
                                                <asp:Parameter Name="startdate" Type="DateTime" />
                                                <asp:Parameter Name="intUserID" Type="Int32" />
                                                <asp:Parameter Name="intUserLocation" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane runat="server" ID="Panel1" Enabled="true">
                                    <Header> News</Header>
                                    <Content>
                                        <asp:GridView ID="gvNews" runat="server" Width="100%" CssClass="manage_gridb" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Notice On">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNewsDate" runat="server" Text='<%# DateTime.Parse(Eval("NoticeDate").ToString()).ToString("dd-MMM-yyyy") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNewsDesc" runat="server" Text='<%#Eval("notice_descr") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lbl" runat="server" Text="No News for now."></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane runat="server" ID="Panel2">
                                    <Header> Upcoming Occasions</Header>
                                    <Content>
                                        <asp:GridView ID="grdupcoming" runat="server" Width="100%" CssClass="manage_gridb" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField HeaderText="Name" DataField="empName" />
                                                <asp:BoundField HeaderText="Date" DataField="Date" DataFormatString="{0:dd-MMM}" />
                                                <asp:BoundField HeaderText="Occasion" DataField="Event" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lbl" runat="server" Text="No upcoming event for this week."></asp:Label>
                                            </EmptyDataTemplate>

                                        </asp:GridView>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane runat="server" ID="AccordionPane1">
                                    <Header>
                                        <asp:Label ID="lblHolidays" runat="server"></asp:Label>
                                    </Header>
                                    <Content>
                                        <asp:GridView ID="gvHolidayView" runat="server" Width="100%" CssClass="manage_gridb" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Occasion Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHolidayDate" runat="server" Text='<%# DateTime.Parse(Eval("HolidayDate").ToString()).ToString("dd-MMM-yyy") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Occasion">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOccasion" runat="server" Text='<%#Eval("Narration") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lbl" runat="server" Text="No holidays for this year."></asp:Label>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane ID="Panel3" runat="server">
                                    <Header>HR Manual</Header>
                                    <Content>                                        
                                        <table id="hrManual" width="100%" style="border:1px solid #dddddd">
                                            <tr style="background:url(../images/minus_icon.png) 5px 9px no-repeat #ededed;height:35px;text-align:left;">
                                                <th style="padding-left:15px;">Last Updated</th>
                                                <th style="padding-left:15px;">HR Policy</th>                                                
                                            </tr>
                                            <tr style="border:1px solid #dddddd;">
                                                <td style="border:1px solid #dddddd;font-weight: 900;padding-left:15px;"> <asp:Label ID="lblHr_Manual" runat="server" ForeColor="black" ></asp:Label></td>
                                                <td>
                                                    <table>
                                                    <tr>
                                                        <td style="font-weight:bold;color:black;padding-left:15px"><label id="Hrmanual" onclick="openPdf(this)" >View</label></td>
                                                        <td><img src="images/pdf_icon.png" id="Hrmanual1" alt="view pdf" onclick = "openPdf(this)" style="width:50px;height:50px;"/></td>
                                                    </tr>
                                                    </table>
                                                     <%--<input type="hidden" id="hdn_Hrmanual" value="../ManualsDocument/HR%20Manual%20-%202018.pdf"/>  --%>  
                                                    <input type="hidden" id="hdn_Hrmanual" runat="server" value=""/>  
                                                 </td>
                                            </tr>                                                                                     
                                        </table>                                        
                                        <%--&nbsp; The HR manual is a comprehensive guide to employees about the terms and conditions of their service with Intelgain Technologies &nbsp;--%>
                                       <span style="float: right;display:none">
                                           <a href="DownloadFile.aspx?Filename=HR Manual - 2018.pdf&Folder=~/ManualsDocument/" id="FileDownload" runat="server">Read...</a>
                                           &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane ID="Panel4" runat="server">
                                    <Header>Mediclaim Policy File </Header>
                                    <Content>
                                        <table id="hrMediclaim" width="100%" style="border:1px solid #dddddd">
                                            <tr style="background:url(../images/minus_icon.png) 5px 9px no-repeat #ededed;height:35px;text-align:left;padding-right:5px;">
                                                <th style="padding-left:15px;">Last Updated</th>
                                                <th style="padding-left:15px;">HR Mediclaim</th>                                                
                                            </tr>
                                            <tr style="border:1px solid #dddddd">
                                                <td style="border:1px solid #dddddd;font-weight: 900;padding-left:15px;"> <asp:Label ID="lblHr_Mediclame" runat="server" ForeColor="black" ></asp:Label></td>                                                
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td><td style="font-weight:bold;color:black;padding-left:15px"><label id="Mediclaim" onclick = "openPdf(this)" >View</label></td></td>
                                                            <td><img src="images/pdf_icon.png" id="Mediclaim1" alt="view pdf" onclick = "openPdf(this)" style="width:50px;height:50px;"/></td>
                                                        </tr>
                                                    </table>
                                                    <%--<input type="hidden" id="hdn_Mediclaim" value="../ManualsDocument/HEALTH%20INSURANCE%20SCHEME.pdf"/>--%>
                                                    <input type="hidden" id="hdn_Mediclaim" runat="server" value=""/>
                                                </td>                                               
                                            </tr>                                   
                                        </table>
                                        <%--&nbsp; Details about Mediclaim policy &nbsp;--%>
                                       <span style="float: right;display:none;">
                                           <a href="DownloadFile.aspx?Filename=HEALTH INSURANCE SCHEME.pdf&Folder=~/ManualsDocument/" id="A1" runat="server">Read...</a>
                                           &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane ID="Panel6" runat="server">
                                    <Header>Anti Sexual Harrasment Policy</Header>
                                    <Content>
                                        <table id="hrHarrasmentPolicy" width="100%" style="border:1px solid #dddddd">
                                            <tr style ="background:url(../images/minus_icon.png) 5px 9px no-repeat #ededed;height:35px;text-align:left;padding-right:5px;">
                                                <th style="padding-left:15px;">Last Updated</th>
                                                <th style="padding-left:15px;">Anti Sexual Harrasment Policy</th>                                                
                                            </tr>
                                            <tr style="border:1px solid #dddddd">
                                                <td style="border:1px solid #dddddd;font-weight: 900;padding-left:15px;"> <asp:Label ID="lbl_AntiASHP" runat="server" ForeColor="black" ></asp:Label></td>
                                                 <td>
                                                    <table>
                                                        <tr>
                                                            <td><td style="font-weight:bold;color:black;padding-left:15px"><label id="ASHP" onclick = "openPdf(this)" >View</label></td></td>
                                                            <td><img src="images/pdf_icon.png" id="ASHP1" alt="view pdf" onclick = "openPdf(this)" style="width:50px;height:50px;"/></td>
                                                        </tr>
                                                    </table>
                                                     <%--<input type="hidden" id="hdn_ASHP" value="../ManualsDocument/Intelegain%20ASHP%20final.pdf"/>--%>
                                                     <input type="hidden" id="hdn_ASHP" runat="server" value=""/>
                                                </td>                                                                                                
                                            </tr>
                                        </table>
                                        <%--&nbsp; Details about Anti sexual harrasment policy &nbsp;--%>
                                       <span style="float: right;display:none;">
                                           <a href="DownloadFile.aspx?Filename=Intelegain ASHP final.pdf&Folder=~/ManualsDocument/" id="A2" runat="server">Read...</a>
                                           &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    </Content>
                                </ajax:AccordionPane>

                                <ajax:AccordionPane ID="Panel7" runat="server">
                                    <Header>Device Setup</Header>
                                    <Content>
                                        <table id="AndroidAPK" width="100%" style="border:1px solid #dddddd">
                                            <tr style ="background:url(../images/minus_icon.png) 5px 9px no-repeat #ededed;height:35px;text-align:left;padding-right:5px;">
                                                <th style="padding-left:24px;">Android</th>
                                                <%--<th style="padding-left:15px;">Download APK</th>  --%>                                              
                                            </tr>
                                            <tr style="border:1px solid #dddddd">
                                                <td style="border:1px solid #dddddd;font-weight: 900;padding-left:15px;"> 
                                                    <img src="AndroidAPK/AgoraApkQR.png?v2" id="ApkScanCode" alt="scan and download APk"  style="width:100px;height:100px;"/>
                                                    <br />
                                                    <asp:Label ID="lblApkVersion" runat="server" ForeColor="black" CssClass="pdnRight"></asp:Label>
                                                </td>
                                                 <%--<td>
                                                    <table>
                                                        <tr>
                                                            <td><td style="font-weight:bold;color:black;padding-left:15px">
                                                                <label id="lblApkName" ></label>
                                                                <asp:Label ID="lblApkName" runat="server" ForeColor="black" ></asp:Label>
                                                                </td>
                                                            </td>
                                                            <td>
                                                                <img src="AndroidAPK/APKqrcode.png" id="ApkScanCode2" alt="scan and download APk"  style="width:100px;height:100px;"/>                                                                
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" id="hdnScanAPk" runat="server" value=""/>
                                                </td>--%>                                                                                                
                                            </tr>
                                        </table>
                                        <%--&nbsp; Details about Anti sexual harrasment policy &nbsp;--%>
                                       <span style="float: right;display:none;">
                                           <a href="DownloadFile.aspx?Filename=Intelegain ASHP final.pdf&Folder=~/ManualsDocument/" id="A3" runat="server">Read...</a>
                                           &nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    </Content>
                                </ajax:AccordionPane>
                            </Panes>
                        </ajax:Accordion>
                    </div>                    
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="a_popbox" id="div_ReviewPopup" style="display: none;">
        <div class="k-widget k-windowAdd a_popbox" style="background-color: white; padding-top: 10px; padding-right: 10px; min-width: 400px; border-color: black; border-width: thin; min-height: 150px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);">
            <div>
                <div class="popup_head">
                    <h3>Review</h3>
                    <div class="clear"></div>
                </div>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                    <tr>
                        <td align="center">
                            <span id="lbl_Reviews"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_AcceptReview" runat="server" Text="Please Accept" CssClass="small_button red_button open" OnClick="Btn_AcceptReview_Click" />
                        </td>
                    </tr>
                </table>
                <div>&nbsp;</div>
            </div>
        </div>
    </div> 
    
    <!-- VW PDf Div display middle of screen Start -->
   
                      <div id="my_pdf_viewer" style="display:none;" class="center">               
                        <div id="canvas_container" style="overflow:auto !important">                        
                             <div id="navigation_controls" style="padding-top:15px;padding-bottom:5px;text-align:center;padding-right:8px"; class="fontFamily">
                                    <button id="go_previous" style="background-color:black;color:white;border-radius:5px;font-size:small;text-align:center;width:90px; margin:5px;font-family:'Times New Roman', Times, serif" > Previous </button> 
                                    <input id="current_page"  value="1"  class="item" type="text" style="background-color:black;color:white;border-radius:5px;text-align:center;width:90px;margin:5px;font-family:'Times New Roman', Times, serif"/>
                                    <button id="go_next" style="background-color:black;color:white;border-radius:5px;font-size:small;text-align:center;width:90px;margin:5px;font-family:'Times New Roman', Times, serif"> Next </button>
                                    <button id="close" onclick="ClosePDF()" style="background-color:black;color:white;border-radius:5px;font-size:small;text-align:center;width:90px;margin:5px;font-family:'Times New Roman', Times, serif"> Close </button>
                                    <button id="zoom_in" class="font-button plus" style="background-color:black;color:white;border-radius:5px;font-size:small;text-align:center;width:30px;margin:5px;font-family:'Times New Roman', Times, serif"> + </button>
                                    <button id="zoom_out" class="font-button minus" style="background-color:black;color:white;border-radius:5px;font-size:small;text-align:center;width:30px;margin:5px;font-family:'Times New Roman', Times, serif"> - </button>                      
                            </div> 
                            <%--<canvas id="pdf_renderer" style="border:1px solid;margin-right:25px;position:center;height:550px;width:750px;padding-bottom:1px;margin-bottom:20px"></canvas>--%>                    
                            <canvas id="pdf_renderer"></canvas>
                        </div>
                </div>

                    <%--<div id="my_pdf_viewer" style="display:none;" class="center">
                        <div id="navigation_controls" style="display:flex;padding-top:15px;padding-bottom:5px;text-align:center;padding-left:290px;width:924px;background-color:black !important;" class="fontFamily">
                            <button id="go_previous" class="btns">Previous</button>
                            <input id="current_page" value="1" type="text" style="border-radius:5px;border-color:white;height: 10px !important;margin-top: 3px;width:56px;" readonly />
                            <button id="go_next" class="btns">Next</button>
                            <button id="close" onclick="ClosePDF()" class="btns">Close</button>
                            <button id="zoom_in" class="btns">+</button>
                            <button id="zoom_out" class="btns">-</button>
                        </div>
                        <div id="canvas_container">
                            <canvas id="pdf_renderer"></canvas>
                        </div>
                    </div>--%>

                     <script type="text/javascript">

                         var PolicyclickID = "";
                         var url = "";
                         function openPdf(elem) {                            

                             var pageNoValue = document.getElementById('current_page').value;
                             document.getElementById('current_page').value = "Page no. " + pageNoValue;
                             

                             PolicyclickID = $(elem).attr("id");
                             
                             $("#my_pdf_viewer").css("display", "block");
                             $('.nbs-flexisel-nav-right').css('display', 'none');
                             $('.nbs-flexisel-nav-left').css('display', 'none');

                             if (PolicyclickID == "Hrmanual" || PolicyclickID == "Hrmanual1") {
                                 //url = $("#hdn_Hrmanual").val();
                                 url = $("#ctl00_ContentPlaceHolder1_Panel3_content_hdn_Hrmanual").val();                                 
                                 
                             }
                             if (PolicyclickID == "Mediclaim" || PolicyclickID == "Mediclaim1") {
                                 //url = $("#hdn_Mediclaim").val();
                                 url = $("#ctl00_ContentPlaceHolder1_Panel4_content_hdn_Mediclaim").val();                                
                             } 
                             if (PolicyclickID == "ASHP" || PolicyclickID == "ASHP1") {
                             /*url = $("#hdn_ASHP").val();*/
                                 url = $("#ctl00_ContentPlaceHolder1_Panel6_content_hdn_ASHP").val();                                 
                             }

                             var myState = {
                                 pdf: null,
                                 currentPage: 1,
                                 zoom: 1
                             }

                    <%--This Url used to display PDF https://code.tutsplus.com/tutorials/how-to-create-a-pdf-viewer-in-javascript--cms-32505--%>
                             pdfjsLib.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/2.0.943/pdf.worker.js`;
                             pdfjsLib.getDocument(url).then((pdf) => {

                                 myState.pdf = pdf;
                                 render();

                             });

                             function render() {                                 
                                 myState.pdf.getPage(myState.currentPage).then((page) => {

                                     var canvas = document.getElementById("pdf_renderer");
                                     var ctx = canvas.getContext('2d');

                                     var viewport = page.getViewport(myState.zoom);

                                     canvas.width = viewport.width;
                                     canvas.height = viewport.height;

                                     page.render({
                                         canvasContext: ctx,
                                         viewport: viewport
                                     });
                                 });
                             }

                             document.getElementById('go_previous').addEventListener('click', (e) => {
                                 e.preventDefault();
                                 if (myState.pdf == null || myState.currentPage == 1)
                                     return;
                                 myState.currentPage -= 1;
                                 document.getElementById("current_page").value = "Page no. " + myState.currentPage;
                                 render();
                             });

                             document.getElementById('go_next').addEventListener('click', (e) => {
                                 e.preventDefault();
                                 if (myState.pdf == null || myState.currentPage > myState.pdf._pdfInfo.numPages)
                                     return;
                                 myState.currentPage += 1;
                                 document.getElementById("current_page").value = "Page no. " + myState.currentPage;
                                 render();
                             });

                             document.getElementById('current_page').addEventListener('keypress', (e) => {
                                 e.preventDefault();
                                 if (myState.pdf == null) return;

                                 // Get key code
                                 var code = (e.keyCode ? e.keyCode : e.which);

                                 // If key code matches that of the Enter key
                                 if (code == 13) {
                                     var desiredPage = document.getElementById('current_page').valueAsNumber;

                                     if (desiredPage >= 1 && desiredPage <= myState.pdf._pdfInfo.numPages) {
                                         myState.currentPage = desiredPage;
                                         document.getElementById("current_page").value = desiredPage;
                                         render();
                                     }
                                 }
                             });

                             document.getElementById('zoom_in').addEventListener('click', (e) => {
                                 e.preventDefault();
                                 if (myState.pdf == null) return;                                 
                                 myState.zoom += 0.5;                                 
                                 render();
                             });

                             document.getElementById('zoom_out').addEventListener('click', (e) => {
                                 e.preventDefault();
                                 if (myState.pdf == null) return;
                                 myState.zoom -= 0.5;
                                 render();
                             });

                         }
                         function ClosePDF(e) {
                             e.preventDefault();                             
                             $("#my_pdf_viewer").css("display", "none");
                             $('.nbs-flexisel-nav-right').css('display', 'block');
                             $('.nbs-flexisel-nav-left').css('display', 'block');
                         }
                         $('body').on('contextmenu', 'canvas', function (e) { return false; });
                     </script>                    
   
                    <!-- VW PDF Div display middle of Screen End  -->

</asp:Content>


