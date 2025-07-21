<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="GetMonthlyRevenue.aspx.cs" Inherits="_Default" EnableEventValidation="false"  %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
     <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
      <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css" />

     <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
     <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>

    <script type="text/javascript" src="../js/console.js"></script>


     <%--Bellow links are for kendo controls (do not change sequence)--%>
   <%-- <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>--%>

             

    <script type="text/javascript" language="javascript">

       $(document).ready(function () {
            var PrevDate = new Date();
           // PrevDate.setDate(PrevDate.getDate() - 30);
            PrevDate.setFullYear(PrevDate.getFullYear() - 1);
            var todayDate = kendo.toString((new Date()), 'dd/MM/yyyy');

  

            $('[id$="txtFromDate"]').kendoDatePicker({ format: "dd/MM/yyyy" }).width(150);
            $('[id$="txtTODate"]').kendoDatePicker({ format: "dd/MM/yyyy" }).width(150);

            $("#txtTODate").data("kendoDatePicker").value(todayDate);
            $("#txtFromDate").data("kendoDatePicker").value(PrevDate);


        });
       
        //function ShowIframe()
        //{
        //    openLoading();
        //    document.getElementById("BICron").src = "http://localhost:51174//Crons/cron.aspx?m=BI";
        //    return false;
         
        //}
        //function openLoading()
        //{
        //    $('#divLoading').css('display', '');
        //    $('#divAddLoadingOverlay').removeClass("k-overlayDisplaynone");
        //    $('#divAddLoadingOverlay').addClass('overlyload');
        //    $('#divAddLoadingOverlay').css('display', '');
        //   setInterval("closeLoading()", 4000);
        // }
        //function closeLoading()
        //{
        //    $('#divLoading').css('display', 'none');
        //    $('#divAddLoadingOverlay').removeClass("overlyload").addClass("k-overlayDisplaynone");
        //    $('#divAddLoadingOverlay').css('display', 'none');
        //    window.location.reload();
        //}

        //var GridReport = "#bindTableData";
    </script>
  

    <style type="text/css">
        /*headers*/

        /*#exceldownload {
            background-color: forestgreen;
            border: none;
            text-decoration: none;
            display: inline-block;
            color: white;
            padding: 8px 8px 8px 8px;
            border-radius: 4px;
        }*/

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
            height: 40px;
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
             height: 30px;
        }
        /*end*/
        .tbreakup .k-grid-header-wrap table {
            width: 100% !important;
        }
        .tbreakup .k-grid table {
            width: 100% !important;
            border-spacing: 1px;
        }
        /*added to display color code for project health*/
        .btnColor {
            width: 20px;
            height: 20px;
            border-radius: 100px;
            float: left;
            margin-left: 12px;
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


         div.k-windowAdd
         { 
             display:block;

         }
        .overlyload 
        {
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
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        
<script type="text/javascript">

    

  

   

    function GetOverDues() {  
        //alert("Clicked");
        fromDate = $('#txtFromDate').val();
        toDate = $('#txtTODate').val();

        var obj = {};
        obj.FromDate = $.trim($("[id*=txtFromDate]").val());
        obj.ToDate = $.trim($("[id*=txtTODate]").val());

        //console.log(fromDate, toDate, obj);
        //alert(fromDate)
        //alert(toDate)
        var isDateField = [];
        var colnumName = [];
        var tabledata = [];

        var d = new Date();
        var month = d.getMonth()+1;
        var day = d.getDate();
        var exceldate = d.getFullYear() + '/' + (month<10 ? '0' : '') + month + '/' + (day<10 ? '0' : '') + day;
        $.ajax({  
            type: "POST",  
            url: 'GetMonthlyRevenue.aspx/GetMonthlyRevenue',  
            //data: { FromDate: fromDate, ToDate: toDate },  
            data: JSON.stringify(obj),  
            contentType: "application/json; charset=utf-8",  
            dataType: "json",  
            success: function (response) {
                var data = response.d;
                $('#grdProjectDetails').empty();
                $('#grdProjectDetails').html(response.d);
                //alert(response.d);

                $("#dtAmountTable").kendoGrid({
                   toolbar: ["excel"],
                    excel: {
                        fileName: "MonthlyRevenue.xlsx",
                        allPages: true
                    },
                    sortable: true,
                    filterable: true,
                });
 
            },            
            failure: function (jqXHR, textStatus, errorThrown) {                  
                alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText); // Display error message  
            }  
        });


        function generateGrid(response) {
            var obj = jQuery.parseJSON(response)
            var model = generateModel(obj);
            

            for (var key in obj.Table1) {
                    for (var key1 in obj.Table1[key]) {
                        //console.log(obj.Table1[key][key1])
                        colnumName.push(obj.Table1[key][key1])
                    }
            }


        var columns = generateColumns(colnumName);

        var dataSource = new kendo.data.DataSource({  
        transport: transport,  
        pageSize: 30,  
            schema: {
                model: model
            }
        });     

        var grid = $("#grdProjectDetails").kendoGrid({
            dataSource: {
                data: obj.Table,
            //transport:{
            //    read: function (options) {
            //        options.success(obj.Table);
            //    }
            //},
            //pageSize: 5,
            schema: {
                model: model
            }
            },
            columns: columns,
            pageable: true,
            editable:true
        });
        }
                
        function generateColumns(response){
            var columnNames = response;
            return columnNames.map(function(name){
              return { field: name, format: (isDateField[name] ? "{0:D}" : "") };
            })
        }

        function generateModel(response) {

        var sampleDataItem = response.Table[0];

        var model = {};
        var fields = {};
        for (var property in sampleDataItem) {
          if(property.indexOf("ID") !== -1){
            model["id"] = property;
          }
          var propType = typeof sampleDataItem[property];

          if (propType === "number" ) {
            fields[property] = {
              type: "number",
              validation: {
                required: true
              }
            };
            if(model.id === property){
              fields[property].editable = false;
              fields[property].validation.required = false;
            }
          } else if (propType === "boolean") {
            fields[property] = {
              type: "boolean"
            };
          } else if (propType === "string") {
            var parsedDate = kendo.parseDate(sampleDataItem[property]);
            if (parsedDate) {
              fields[property] = {
                type: "date",
                validation: {
                  required: true
                }
              };
              isDateField[property] = true;
            } else {
              fields[property] = {
                validation: {
                  required: true
                }
              };
            }
          } else {
            fields[property] = {
              validation: {
                required: true
              }
            };
          }
        }

        model.fields = fields;
        //console.log(model)
        return model;
      }


    } 


     function GetExcelReport() {

          var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE "); 
            var obj = {};
            obj.FromDate = $.trim($("[id*=txtFromDate]").val());
            obj.ToDate = $.trim($("[id*=txtTODate]").val());

          $.ajax({  
            type: "POST",  
            url: 'GetMonthlyRevenue.aspx/GetMonthlyRevenue',  
            //data: { FromDate: fromDate, ToDate: toDate },  
            data: JSON.stringify(obj),  
            contentType: "application/json; charset=utf-8",  
            dataType: "json",  
            success: function (response) {
                tab_text = response.d;
                 if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
                {
                    txtArea1.document.open("txt/html","replace");
                    txtArea1.document.write(tab_text);
                    txtArea1.document.close();
                    txtArea1.focus(); 
                    sa=txtArea1.document.execCommand("SaveAs",true,"MonthlyRevenue.xlsx");
                }  
                else                 //other browser not tested on IE 11
                    sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));
                return (sa);
            },            
            failure: function (jqXHR, textStatus, errorThrown) {                  
                alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText); // Display error message  
            }  
        });

         //console.log(tab_text)
        //tab_text= tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
        //tab_text= tab_text.replace(/<img[^>]*>/gi,""); // remove if u want images in your table
        //tab_text= tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

       

         

        


        //var obj1 = {};
        //obj1.FromDate = $.trim($("[id*=txtFromDate]").val());
        //obj1.ToDate = $.trim($("[id*=txtTODate]").val());
        ////console.log(JSON.stringify(obj1))
        //$.ajax({
        //    type: "POST",
        //    url: 'AmountOverDue.aspx/ExportToExcel',
        //    data: JSON.stringify(obj1),
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (response) {
        //        alert("File Downloaded !!!")
        //    },
        //    failure: function (jqXHR, textStatus, errorThrown) {
        //        alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText); // Display error message  
        //    }

        //});
        //window.location = "ExportToExcel?FromDate=" + obj.FromDate + "&ToDate=" + obj.ToDate;
    }

     $(function () {
        $('#txtFromDate').kendoDatePicker({ format: "dd/MM/yyyy" });
        $('#txtTODate').kendoDatePicker({ format: "dd/MM/yyyy" });
    });
    
   
    $("#export").click(function (e) {
            // trigger export of the products grid
            $("#dtAmountTable").data("kendoGrid").saveAsExcel();
            // wait for both exports to finish
            $.when.apply(null, promises)
             .then(function(dtAmountTableWorkbook) {

              // create a new workbook using the sheets of the products and orders workbooks
              var sheets = [
                dtAmountTableWorkbook.sheets[0]
              ];

              sheets[0].title = "MonthlyRevenue";

              var workbook = new kendo.ooxml.Workbook({
                sheets: sheets
              });

              // save the new workbook,b
              kendo.saveAs({
                dataURI: workbook.toDataURL(),
                fileName: "MonthlyRevenue.xlsx"
              })
            });
          });



</script>
   
    <div class="content_wrap">
        <div class="gride_table tbreakup">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td align="left" width="38%">
                                <asp:Label ID="lblProjects" Text="Monthly Revenue" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                             <td id="tdOverHead">

                                    <asp:Label ID="Label1" Text="From Month" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <input type="text" id="txtFromDate" onkeypress="return false;" style="width: 140px" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblPaidTo" Text="To Month" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                                    <input type="text" id="txtTODate" onkeypress="return false;" style="width: 140px" />
                                    &nbsp;&nbsp;&nbsp;
                                    <input type="button" value="Search" onclick="GetOverDues();" runat="server" class="small_button white_button" />
    

                                <%--<input type="checkbox" id="chkIsOverHeads" onclick="fnshowOverHeads(this.checked);" style="display:none" />
                                <span class="active-set" id="Span1" style="font-size: small; text-align: left;display:none">Include Overheads</span> &nbsp&nbsp--%>
                            </td>

                            <td align="right">
                                 <%--<iframe id = "BICron" frameborder = "0" height = "0px" width = "0px"></iframe>--%>
                                <%--<asp:ImageButton ID="imgRefresh" runat="server" OnClientClick = "return ShowIframe()" style="display:none"  ImageUrl="~/Member/images/refresh-icon.png" Width="50px" Height="40px"/> --%>
                                <%--<asp:ImageButton ID="imgRefresh" runat="server" OnClick="imgRefresh_Click" OnClientClick="openLoading();" ImageUrl="~/Member/images/refresh-icon.png" Width="50px" Height="40px"/>--%>
                                <%--<button class="btn-button" type="submit"  id="exceldownload" onclick="DownloadExcelFile(); return false"><i class="fa fa-file-excel-o"></i> Excel</button>--%>
                                <%--<input type="button" value="Excel" onclick="GetExcelReport();" runat="server" class="small_button white_button" />--%>

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




        </div>
    </div>
    </div>
    


    

</asp:Content>