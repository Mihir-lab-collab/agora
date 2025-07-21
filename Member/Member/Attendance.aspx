<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance.aspx.cs" Inherits="Member_Attendance" MasterPageFile="~/Member/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <%--<script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="js/Attendance.js"></script>
      <script type="text/javascript" src="../Javascript/jquery-ui.min.js"></script>

    <%--<script src="../js/jquery-1.9.0.js" type="text/javascript"></script>--%>

    <style type="text/css">


    .fixedHeader
    {

 
    position: sticky;
    /*z-index:50;*/
    left:0;

  

    }



    </style>

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

        .k-grid-content > table > tbody > tr 
        {
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
        .manage_grid tbody td, .manage_grid_a tbody td {
            width: 25px;
        }

        /*pagination here css*/
        .pagination-centered {
            background: #fff;
            text-align: center !important;
        }

            .pagination-centered td table {
                background: #fff;
                width: 12%;
                margin: 0 auto;
                display: block;
            }

                .pagination-centered td table tbody tr td {
                    width: auto !important;
                }

                    .pagination-centered td table tbody tr td span {
                        min-width: auto !important;
                    }

                .pagination-centered:hover, .pagination-centered td table tbody tr td, .manage_grid_a tbody tr.pagination-centered:hover td {
                    background: #fff !important;
                }

        .manage_grid_a tbody tr:hover td {
            background-color: #EBEBEB !important;
        }

        .manage_grid_a tbody td span {
            display: inline-block;
            min-width: 14px;
        }

        span.k-datepicker {
            width: 148px !important;
        }

        span.k-datepicker {
            background: none !important;
        }

        .k-select {
            margin-top: 3px;
        }

        .k-icon {
            margin: 1px 2px;
        }

        .manage_grid_a thead tr th {
            background: #cbc8c8;
            background-image: none;
            border: 1px solid #dddddf;
            padding: 6px 10px;
            text-align: left;
        }

        .manage_grid tbody td, .manage_grid_a tbody td, .manage_grid_a thead tr th {
            text-align: center;
        }
        .manage_grid tbody, .manage_grid_a tbody {
            height:420px !important;
        }

        .manage_grid_a tbody tr td:nth-child(1) {
            text-align: left !important;
            background: #eee;
            min-width: 127px !important;
        }

        .manage_grid_a thead tr th:nth-child(1) {
            text-align: left !important;
            background: #cbc8c8;
            min-width: 100px !important;
        }

        input[type="checkbox"] {
            margin-left: 6px;
            margin-top: 3px;
            float: none;
        }

        table.scroll {
            border-spacing: 100;
        }

            table.scroll tbody, table.scroll thead {
                display: block;
            }

        thead tr th {
            height: 30px;
            line-height: 30px;
        }
        tbody
        {
              
                 position: sticky; 
                 left: 0;
                
                  z-index: 20; 
        }
        thead{
        }
  
}
        
        table.scroll tbody 
            {
            height: 420px !important;
             
             /*    position: sticky; 
                 left: 0;
                overflow-x:hidden;
                  z-index: 20; */
       
        } 

        tbody td:last-child, thead th:last-child {
            border-right: none;
        }

        /*table fixed css*/
        .manage_grid_a thead tr th {
            padding: 6px 6px;
        }

        .manage_grid tbody td, .manage_grid_a tbody td {
            padding: 6px 6px;
        }

        .manage_grid_a tbody td, .manage_grid_a thead tr th {
            min-width: 100px;
        }

        div.k-windowAdd {
            /*display: block;*/
            margin-top: 14%;
            position: absolute;

        }

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

        .k-button {
            width: 48%;
            text-transform: capitalize;
            border: #c5c5c5 1px solid;
            background: #f6f6f6;
            font-weight: normal;
        }

            .k-button:hover {
                background-image: none;
                border: #c5c5c5 1px solid;
                background-color: #BEBCBC;
            }

        .clear_btn {
            width: auto;
            padding: 0 15px;
            border: #c5c5c5 1px solid;
            background: #f6f6f6;
            font-size: 12px;
        }
        


/*        #mystyle{
            display:block;
        }*/
    

        /*td.locked, th.locked {
position:relative;   
left:expression((this.parentElement.parentElement.parentElement.parentElement.scrollLeft-2)+'px');
}*/

        .GRScroll {
         position: relative;
  width: 5em;
  left: 20.5em;
  top: auto;
  border-top-width: 1px;
  /*only relevant for first row*/
  margin-top: -1px; 
  display:block;
  height:100%;
  /*overflow:hidden; */  
        }
      

        /*Following css for First column freeze*/
.myGrid{
width:100%;
background-color:#fff;
margin: 5px 0 10px 0;
border: solid 1px #525252;
border-collapse:collapse;
}

.myGrid td{
padding: 2px;
border:solid 1px #c1c1c1;
color:#717171;
}

.myGrid th{
padding: 4px 2px;
color: #fff;
background-color: #424242;
border-left:solid 1px #525252;
font-size:0.9em;
}

.myGrid .alt{
background-color: #EFEFEF;
}

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblLocation" Text="Attendance" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                </div>
                <div class="grid_head">
                    <table id="tblsalaryslip" runat="server" width="100%">
                        <tr>
                            <td>
                                <font face="Verdana" size="2"><b>
                                    <asp:LinkButton ID="prevMonth" Text="<<" CommandArgument="prev"
                                        runat="server" OnClick="PagerButtonClick" CausesValidation="False" />
                                    <asp:Label ID="lblMonth" runat="server"></asp:Label>
                                    <asp:LinkButton ID="nextMonth" Text=">>" CommandArgument="next" runat="server" OnClick="PagerButtonClick"
                                        CausesValidation="False" />
                                </b><b><a style="text-decoration: none" href="javascript:void(0);" onclick="return PopupHours();">Hours Report</a></b></font>
                            </td>
                        </tr>
                    </table>
                </div>
              
                
              <%--  //Added for frezze column//--%>

<%--                    <div>
    <asp:GridView ID="grdatt" runat="server" Style="width: 100%; border-collapse: collapse;">
    </asp:GridView>
</div>
<div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <link href="GridviewScroll/gridviewScroll.css" rel="stylesheet" type="text/css" />
    <script src="GridviewScroll/gridviewScroll.js" type="text/javascript"></script>
    <script src="GridviewScroll/gridviewScroll.min.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        table
        {
            border: 1px solid #ccc;
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
        }
        table th, table td
        {
            padding: 5px;
            border-color: #ccc;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#grdatt').gridviewScroll({
                width: 600,
                height: 300,
                freezesize: 2, // Freeze Number of Columns.
                headerrowcount: 1, //Freeze Number of Rows with Header.
                arrowsize: 30,
                varrowtopimg: "Images/arrowvt.png",
                varrowbottomimg: "Images/arrowvb.png",
                harrowleftimg: "Images/arrowhl.png",
                harrowrightimg: "Images/arrowhr.png"
            });
        });
    </script>
</div>--%>
               
                
                
                
                
                
                <div id="gridContainer" style="width:100%; overflow:auto;border-collapse:collapse;margin: auto; float:left;" >
               <asp:GridView ID="grdatt" runat="server"  AutoGenerateColumns="False" CssClass="manage_grid_a scroll" OnRowDataBound="grdatt_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="EmpID">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("EmpID") %>' Style="white-space: nowrap; font-family: Verdana; font-size: 12px"></asp:Label>
                                    <%--<asp:Label ID="lbl_locationFk" runat="server" Text='<%# Bind("LocationFKID") %>'  Visible="false"></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="fixedHeader" ItemStyle-CssClass="fixedHeader" HeaderStyle-Width="5%"><%--ItemStyle-CssClass="GRScroll"--%>
                                <HeaderTemplate>Name <span class="k-icon k-filter" id="display" onclick="ClosedivFilter(this);" style="cursor: pointer; float: right; clear: right; margin-top: 7px;"></span></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton  ID="Label2" runat="server" OnClick="lnk_EmpName_Click" OnClientClick="ShowLoading();" Text='<%# Bind("empname") %>' CommandArgument='<%# Bind("empname") %>' ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="1">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn3" runat="server" Value='<%# Bind("aa1") %>' />
                                    <asp:Label ID="LabelH3" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label3" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk3" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="2">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn4" runat="server" Value='<%# Bind("aa2") %>' />
                                    <asp:Label ID="LabelH4" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label4" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk4" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="3">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn5" runat="server" Value='<%# Bind("aa3") %>' />
                                    <asp:Label ID="LabelH5" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label5" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk5" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="4">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn6" runat="server" Value='<%# Bind("aa4") %>' />
                                    <asp:Label ID="LabelH6" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label6" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk6" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="5">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn7" runat="server" Value='<%# Bind("aa5") %>' />
                                    <asp:Label ID="LabelH7" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label7" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk7" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="6">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn8" runat="server" Value='<%# Bind("aa6") %>' />
                                    <asp:Label ID="LabelH8" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label8" runat="server" Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk8" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="7">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn9" runat="server" Value='<%# Bind("aa7") %>' />
                                    <asp:Label ID="LabelH9" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label9" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"> </asp:Label>
                                    <asp:CheckBox ID="chk9" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="8">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn10" runat="server" Value='<%# Bind("aa8") %>' />
                                    <asp:Label ID="LabelH10" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label10" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk10" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="9">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn11" runat="server" Value='<%# Bind("aa9") %>' />
                                    <asp:Label ID="LabelH11" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label11" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk11" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="10">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn12" runat="server" Value='<%# Bind("aa10") %>' />
                                    <asp:Label ID="LabelH12" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label12" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk12" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="11">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn13" runat="server" Value='<%# Bind("aa11") %>' />
                                    <asp:Label ID="LabelH13" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label13" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk13" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="12">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn14" runat="server" Value='<%# Bind("aa12") %>' />
                                    <asp:Label ID="LabelH14" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label14" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk14" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="13">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn15" runat="server" Value='<%# Bind("aa13") %>' />
                                    <asp:Label ID="LabelH15" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label15" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk15" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="14">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn16" runat="server" Value='<%# Bind("aa14") %>' />
                                    <asp:Label ID="LabelH16" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label16" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk16" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="15">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn17" runat="server" Value='<%# Bind("aa15") %>' />
                                    <asp:Label ID="LabelH17" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label17" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk17" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="16">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn18" runat="server" Value='<%# Bind("aa16") %>' />
                                    <asp:Label ID="LabelH18" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label18" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk18" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="17">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn19" runat="server" Value='<%# Bind("aa17") %>' />
                                    <asp:Label ID="LabelH19" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label19" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk19" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="18">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn20" runat="server" Value='<%# Bind("aa18") %>' />
                                    <asp:Label ID="LabelH20" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label20" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk20" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="19">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn21" runat="server" Value='<%# Bind("aa19") %>' />
                                    <asp:Label ID="LabelH21" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label21" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk21" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="20">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn22" runat="server" Value='<%# Bind("aa20") %>' />
                                    <asp:Label ID="LabelH22" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label22" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk22" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="21">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn23" runat="server" Value='<%# Bind("aa21") %>' />
                                    <asp:Label ID="LabelH23" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label23" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk23" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="22">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn24" runat="server" Value='<%# Bind("aa22") %>' />
                                    <asp:Label ID="LabelH24" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label24" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk24" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="23">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn25" runat="server" Value='<%# Bind("aa23") %>' />
                                    <asp:Label ID="LabelH25" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label25" runat="server" Text='<%# Bind("aa23") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk25" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="24">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn26" runat="server" Value='<%# Bind("aa24") %>' />
                                    <asp:Label ID="LabelH26" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label26" runat="server" Text='<%# Bind("aa24") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk26" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="25">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn27" runat="server" Value='<%# Bind("aa25") %>' />
                                    <asp:Label ID="LabelH27" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label27" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk27" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="26">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn28" runat="server" Value='<%# Bind("aa26") %>' />
                                    <asp:Label ID="LabelH28" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label28" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk28" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="27">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn29" runat="server" Value='<%# Bind("aa27") %>' />
                                    <asp:Label ID="LabelH29" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label29" runat="server" Text='<%# Bind("aa27") %>' ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk29" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="28">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn30" runat="server" Value='<%# Bind("aa28") %>' />
                                    <asp:Label ID="LabelH30" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label30" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk30" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="29">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn31" runat="server" Value='<%# Bind("aa29") %>' />
                                    <asp:Label ID="LabelH31" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label31" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk31" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="30">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn32" runat="server" Value='<%# Bind("aa30") %>' />
                                    <asp:Label ID="LabelH32" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label32" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk32" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="31">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hdn33" runat="server" Value='<%# Bind("aa31") %>' />
                                    <asp:Label ID="LabelH33" runat="server" ToolTip='<%# Bind("empname") %>' Style="white-space: nowrap;"></asp:Label>
                                    <asp:Label ID="Label33" runat="server" ToolTip='<%# Bind("empname") %>'
                                        Style="white-space: nowrap;"></asp:Label>
                                    <asp:CheckBox ID="chk33" runat="server" Text="" ToolTip='<%# Bind("empname") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="grdattwidth" />
                            </asp:TemplateField>
                        </Columns>
                        
                       
                        <HeaderStyle CssClass="hrdstyl" />
                        <RowStyle CssClass="torhight" />
                    </asp:GridView>
                    <div id="DivGridv" runat="server" style="padding-left: 15px; font-size: 18px; margin: 15px;" visible="false">No Record Found  
                        <asp:Button ID="btnNodataFound" runat="server" Text="Clear Filter" CssClass="small_button red_button k-button open clear_btn" OnClientClick="javascript:return DateCompaire()" OnClick="btnNodataFound_Click" />
                    </div>
                </div>
                 <PagerStyle CssClass="pagination pagination-left" HorizontalAlign="Center" BorderStyle="None" />
                <!-- Added By Trupti -->
                <div class="k-widget" id="divShowCalendarEmp" style="display: none; padding-top: 10px; padding-right: 1px; min-width: 270px; 
min-height: 50px; top: 0%; left: 10%; z-index: 10003; opacity: 1; transform: scale(1); max-width: 1252px; width:80%; height: 890px; position: absolute;" data-role="draggable">
                    <img src="Images/delete_ic.png" class="close-button" runat="server" onclick="closedivShowCalendarEmp();" alt="Close" />
                    &nbsp; &nbsp;
                    <asp:Label ID="lbl_empname" runat="server" Text=""></asp:Label>


                    <%--<ajax:AccordionPane ID="Panel5" runat="server">
                                    <Header>Calendar</Header>
                                    <Content>--%>
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

                    <div class="table-responsive" style="overflow:auto;">
                    <asp:DataList ID="DataCalendar" runat="server" DataSourceID="MyCalendar" Style="width: 1008px; margin: auto;" RepeatColumns="7" RepeatDirection="Horizontal">
                        <HeaderTemplate>
                            <div style="text-align: center; background-color: GrayText;">
                                <table>
                                    <tr align="center">
                                        <td>
                                            <asp:Label ID="Label6" Text="Sunday" runat="server" Width="175px" ForeColor="White" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label0" Text="Monday" runat="server" Width="175px" ForeColor="White" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" Text="Tuesday" runat="server" Width="175px" ForeColor="White" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" Text="Wednesday" runat="server" Width="175px" ForeColor="White" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" Text="Thursday" runat="server" Width="175px" ForeColor="White" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" Text="Friday" runat="server" Width="175px" ForeColor="White" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" Text="Saturday" runat="server" Width="175px" ForeColor="White" Font-Size="Large"></asp:Label>
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
                                 
                                <div id="spnholiday" style="padding: 10px; color: red; text-align: center; font-weight: bold; padding-top :0px; padding-bottom : 0px;">
                                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("HolidayLabel").ToString() %>'></asp:Label>
                                </div>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:DataList>
                        </div>
                    <asp:ObjectDataSource ID="MyCalendar" runat="server" SelectMethod="GetDays" TypeName="clsCalendar" OnSelecting="MyCalendar_Selecting">
                        <SelectParameters>
                            <asp:Parameter Name="startdate" Type="DateTime" />
                            <asp:Parameter Name="intUserID" Type="Int32" />
                            <asp:Parameter Name="intUserLocation" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <%-- </Content>
                                </ajax:AccordionPane>--%>
                </div>
                <!--end  -->
            </div>
        </div>
    </div>

    <div id="divAddPopupOverlay"></div>

    <div class="k-widget k-windowAdd popup_div" id="divAddPopup" style="display: none; padding: 10px; min-width: 90px; min-height: 50px; top: 30%; left: 456px; z-index: 99999; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Edit Attendance</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Emp Name</th>
                    <td align="center">
                        <asp:TextBox ID="txtEmpName" runat="server" Style="width: 300px;" onkeypress="return false;" onkeyup="return false" class="k-textbox" />
                    </td>
                </tr>
                <tr>
                    <th>Leave/Present Date From</th>
                    <td align="center">
                        <asp:TextBox ID="txtFromDate" runat="server" onkeyup="return false" Style="border-radius: 0; width: 148px !important;" onkeypress="return false" />
                    </td>
                </tr>
                <tr>
                    <th>To</th>
                    <td align="center">
                        <asp:TextBox ID="txtToDate" runat="server" onkeyup="return false" Style="border-radius: 0; width: 148px !important;" onkeypress="return false" />
                    </td>
                </tr>
                <tr>
                    <th>Status</th>
                    <td align="center">
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false" CssClass="c_dropdown" Visible="true" AppendDataBoundItems="true" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSave" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return DateCompaire()" OnClick="lnkSave_Click" />
                        <asp:HiddenField ID="hdStatusUpdate" runat="server" />
                        <asp:HiddenField ID="hdEmpid" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnTime" runat="server" />
                    </td>
                </tr>
            </table>
            <div>&nbsp;</div>
        </div>
    </div>

    <div class="k-widget k-windowAdd popup_div" id="divLogPopup" style="display: none; padding: 10px; min-width: 90px; min-height: 50px; top: 40%; left: 456px; z-index: 99999; opacity: 1; transform: scale(1); width: 600px;" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Attendance Log</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeLogPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <div id="divAttLodDatils">
            </div>
            <div>&nbsp;</div>
        </div>
    </div>

    <div class="k-widget k-windowAdd " id="divAction" style="display: none; padding: 10px; min-width: 90px; min-height: 50px; z-index: 10000; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Action</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeActionPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <div id="actionLink">
            </div>

        </div>
    </div>

    <div class="k-widget k-windowAdd" id="DivPopupHours" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div>
                    <div class="popup_head">
                        <h3>Hours Report </h3>
                        <img src="Images/delete_ic.png" class="close-button" onclick="closePopupHours()"
                            alt="Close" />
                        <div class="clear">
                        </div>
                    </div>
                    <div class="">
                        <table id="Table1" runat="server" width="100%">
                            <tr>
                                <td>
                                    <font face="Verdana" size="2"><b>
                                        <asp:LinkButton ID="LinkButton1" Text="<<" CommandArgument="prevH"
                                            runat="server" OnClick="PagerHButtonClick" CausesValidation="False" OnClientClick="openLoading();" />

                                        <asp:Label ID="lblMonthHR" runat="server"></asp:Label>

                                        <asp:LinkButton ID="LinkButton2" Text=">>" CommandArgument="nextH" runat="server" OnClick="PagerHButtonClick"
                                            CausesValidation="False" OnClientClick="openLoading();" />
                                    </b></font>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="padding: 5px; overflow-y: auto; height: 300px;">
                        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="true" CssClass="manage_grid_a manage " Width="856px">
                        </asp:GridView>
                    </div>
                    <div>&nbsp;</div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divAddLoadingOverlay" style="display: none;" class="overlyload"></div>
    <div class="k-widget k-windowAdd" id="divLoading" style="display: none; padding: 10px; width: auto; height: auto; z-index: 999999999; opacity: 1; transform: scale(1); left: 50%; top: 30%;" data-role="draggable">
        <div>

            <img src="images/loading.gif" alt="" />



        </div>
    </div>

    <div>
        <div class="k-widget k-windowAdd" id="divFilter" style="display: none; padding: 10px 5px; border-radius: 0 0 5px 5px; width: auto; height: auto; opacity: 1; left: 247px; top: 244px; transform: scale(1); background: #ebebeb;">
            <div class="k-filter-help-text" style="margin-bottom: 8px;">Show items with value that:</div>
            <asp:TextBox ID="txtfilter" runat="server"></asp:TextBox>
            <div class="clear">
            </div>
            <br />
            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="small_button red_button k-button open" OnClientClick="javascript:return checkFilter()" OnClick="btnFilter_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="small_button red_button k-button open" OnClientClick="javascript:return DateCompaire()" OnClick="btnClear_Click" />
        </div>

    </div>

    <script type="text/javascript">
        var $table = $('table.scroll'),
            $bodyCells = $table.find('tbody tr:first').children(),
            colWidth;
        // Adjust the width of thead cells when window resizes
        $(window).resize(function () {
            // Get the tbody columns width array
            colWidth = $bodyCells.map(function () {
                return $(this).width();
            }).get();
            // Set the width of thead columns
            $table.find('thead tr').children().each(function (i, v) {
                $(v).width(colWidth[i]);
            });
        }).resize(); // Trigger resize handler




        function ShowCalendarDiv() {

            $('#divShowCalendarEmp').css('display', '');
            $('#divShowCalendarEmp').addClass('k-overlay');
            $('.black_cover').css('display', 'block');
        }

        function closedivShowCalendarEmp() {
            $('.black_cover').css('display', 'none');
            $('#divShowCalendarEmp').css('display', 'none');
            $('#divShowCalendarEmp').removeClass("k-overlay").addClass("k-overlayDisplaynone");
        }

    </script>


    <asp:HiddenField ID="hdnMonthNo" runat="server" />
    <asp:HiddenField ID="hdnYear" runat="server" />
    <asp:HiddenField ID="hdnMonthNoHR" runat="server" />
    <asp:HiddenField ID="hdnYearHR" runat="server" />
    <asp:HiddenField ID="hdnFilter" runat="server" Value="Nofilter" />

    
    <%--<script language="javascript" type="text/javascript">
         $(document).ready(function () {
            //here clone our gridview first
            var tab = $("#<%=grdatt.ClientID%>").clone(true);
            //here again for freeze
            var tabFreeze = $("#<%=grdatt.ClientID%>").clone(true);
            //set width (for scroll)
            var totalWidth = $("#<%=grdatt.ClientID%>").outerWidth();
            var firstColWidth = $("#<%=grdatt.ClientID%> th:first-child").outerWidth();
            tabFreeze.width(firstColWidth);
            tab.width(totalWidth - firstColWidth);
            //here make 2 table 1 for freeze column and 2 for all ramain column
            tabFreeze.find("th:gt(0)").remove();
            tabFreeze.find("td:not(:first-child)").remove();
            tab.find("th:first-child").remove();
            tab.find("td:first-child").remove();
            //create a container for these 2 table and make second table scrolable

            var container = $('<table border="0" cellpadding="0" cellspacing="0"><tr><td valign="top"><div id="FCol"></div></td><td valign="top"><div id="Col" style="width:1200px; overflow:auto;"></div></td></tr></table>')
 
            $("#FCol", container).html($(tabFreeze));
            $("#Col", container).html($(tab));
            //clear all html
            $("#gridContainer").html('');
            $("#gridContainer").append(container);
        });

    --%>
 



    



    

<%--      //function scrolling() {
      //  var Col = $("#Col");
      //  $("#FCol").scroll(function () {

      //      Col.prop("scrollTop", this.scrollTop)
      //          .prop("scrollLeft", this.scrollLeft);
      //  });

      //      }
    
      //});
--%>

        
   <%-- </script>--%>
  <%--  <script type="text/javascript">
    $(document).ready(function () {
            $('#grdatt').gridviewScroll({
                
                freezesize: 1, 
                headerrowcount: 1,
                arrowsize: 30,
         });
        });--%>
    </script>
</asp:Content>
 