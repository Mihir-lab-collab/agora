<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Member_Default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detail template</title>
    <meta charset="utf-8">
    <link href="Styles/styles/examples-offline.css" rel="stylesheet">
    <link href="styles_1/kendo.common.min.css" rel="stylesheet">
    <link href="styles_1/kendo.rtl.min.css" rel="stylesheet">
    <link href="styles_1/kendo.default.min.css" rel="stylesheet">
    <link href="styles_1/kendo.dataviz.min.css" rel="stylesheet">
    <link href="styles_1/kendo.dataviz.default.min.css" rel="stylesheet">
    <script src="js/jquery.min.js"></script>
    <script src="js/kendo.all.min.js"></script>
    <script src="js/console.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a class="offline-button" href="../index.html">Back</a>
        <div id="example">
            <div id="grid">
            </div>
            <script type="text/x-kendo-template" id="template">
                <div class="tabstrip">
                    <ul>
                        <li class="k-state-active">
                           Ingredients
                        </li>
                        <li>
                            Details
                        </li>
                    </ul>
                    <div>
                        <div class="orders"></div>
                    </div>
                    <div>
                        <div class='employee-details'>
                            <ul>
                                <li><label>Recipiename:</label>#= Recipiename #</li>
                                <li><label>preparationtime:</label>#= preparationtime #</li>
                                <li><label>cookingtime:</label>#= cookingtime #</li>
                                <li><label>by:</label>#= by #</li>
                            </ul>
                        </div>
                    </div>
                </div>

            </script>
            <script>
                $(document).ready(function () {
                    var ctkGridData = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: "WebService.asmx/GetRecipie",
                                contentType: "application/json; charset=utf-8",
                                type: "GET"

                            }
                        },
                        schema: {
                            data: "d",
                            total: function (response) { // For grid item count botttom right of grid
                                return $(response.d).length;
                            }
                        },
                        pageSize: 10
                    }); //----> End Data store <----//



                    var element = $("#grid").kendoGrid({
                        dataSource: ctkGridData,
                        height: 550,
                        sortable: true,
                        pageable: false,
                        detailTemplate: kendo.template($("#template").html()),
                        detailInit: detailInit,
                        dataBound: function () {
                            this.expandRow(this.tbody.find("tr.k-master-row").first());
                        },
                        columns: [
                                                        {
                                                            field: "Name",
                                                            title: "Project",
                                                            width: "120px"
                                                        },
                            {
                                field: "PM",
                                title: "PM",
                                width: "120px"
                            },
                            //{
                            //    field: "cookingtime",
                            //    width: "120px"
                            //},
                            //{
                            //    field: "by",
                            //    width: "120px"
                            //}
                        ]
                    });
                });

                function detailInit(e) {
                    var detailRow = e.detailRow;
                    options:
                        {
                            RecipieID: 1
                        }
                    var ctkGridData_1 = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: "WebService.asmx/GetIngredients",
                                contentType: "application/json; charset=utf-8",
                                type: "POST"
                            },
                            parameterMap: function (options, operation) {
                                switch (operation) {
                                    case "read":
                                        return JSON.stringify(options);
                                        break;
                                }
                            }
                        },
                        schema: {
                            data: "d",
                            model: {
                                id: "RecipieID",
                                fields: {
                                    Recipie_ID: { type: "string" },
                                    Ing_ID: { type: "string" },
                                    Name: { type: "string" },
                                    Qty: { type: "string" }
                                }
                            },
                            total: function (response) { // For grid item count botttom right of grid
                                return $(response.d).length;
                            }
                        },
                        serverPaging: true,
                        serverSorting: true,
                        serverFiltering: true,
                        pageSize: 7,
                        filter: { field: "Recipie_ID", operator: "eq", value: e.data.Recipie_ID }
                    }); //----> End Data store <----//

                    detailRow.find(".tabstrip").kendoTabStrip({
                        animation: {
                            open: { effects: "fadeIn" }
                        }
                    });

                    detailRow.find(".orders").kendoGrid({
                        dataSource: ctkGridData_1,
                        scrollable: false,
                        sortable: true,
                        pageable: true,
                        columns: [
                            { field: "Ing_ID", title: "Ing_ID", width: "70px" },
                            { field: "Name", title: "Name", width: "110px" },
                            { field: "Qty", title: "Qty" }
                        ]
                    });
                }
            </script>
            <style>
                .k-detail-cell .k-tabstrip .k-content
                {
                    padding: 0.2em;
                }
                .employee-details ul
                {
                    list-style: none;
                    font-style: italic;
                    margin: 15px;
                    padding: 0;
                }
                .employee-details ul li
                {
                    margin: 0;
                    line-height: 1.7em;
                }
                
                .employee-details label
                {
                    display: inline-block;
                    width: 90px;
                    padding-right: 10px;
                    text-align: right;
                    font-style: normal;
                    font-weight: bold;
                }
            </style>
        </div>
    </div>
    </form>
</body>
</html>

