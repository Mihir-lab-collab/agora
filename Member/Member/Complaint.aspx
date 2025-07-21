<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Complaint.aspx.cs" Inherits="Complaint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 308px;
        }

        .auto-style2 {
            width: 220px;
        }

        .auto-style3 {
            width: 203px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>

    <script src="../Member/js/complaint.js" type="text/javascript"></script>
    
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
    </style>
        
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblComplaint" Text="Complaints" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                </div>
                <div id="grid"></div>
            </div>
        </div>
    </div>

  
    <script type="text/x-kendo-template" id="popup-editor">
   <table cellpadding="0" cellspacing="0" border="0" width="600px" class="manage_form">
               <tr id="trConfig" class="manage_bg">
        </tr>
        
                    <th>Project Name:</th>
                    <td align="center">                      
                        <input id="EditprojName" disabled data-bind="value:projName" />
                        
                    </td>
                </tr>
        <tr>
                    <th>Complaint Date:</th>
                    <td align="center">
                        <input id="EditcompDate " disabled data-bind="value:compDate"  style="width: 103px" />                               
                         
                    </td>
                </tr>
         <tr>
                    <th>Category:</th>
                    <td align="center">
                        <input id="Editcategory" disabled data-bind="value:compCategory"  />
                       
                    </td>
                </tr>
        <tr>
                    <th>Complaint Title</th>
                    <td align="center">
                        <input id="Edittitle" disabled data-bind="value:compTitle" />
                        
                    </td>
                </tr>
         <tr>
                    <th>Compalint Description:</th>
                    <td align="center">
                        <textarea id="EditcompDesc" disabled rows="15" data-bind="value:compDesc"  style="width: 300px"/> 
                       
                    </td>
                </tr>

               <tr>
                    <th>Feedback:</th>
                    <td align="center">
                        <textarea id="EditFeedback"  data-bind="value:compFeedback" rows="10" style="width: 300px;"/> 
                       
                    </td>
                </tr
              <tr>
                    <th>Resolve:</th>
                    <td align="center">
                        <input id="Editresolve"  data-bind="value:compResolved"  />
                         
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

    <asp:HiddenField ID="hdncompId" runat="server" Value="0" />


</div>

</div>

</div>

</asp:Content>

