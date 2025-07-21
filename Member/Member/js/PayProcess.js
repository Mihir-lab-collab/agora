$(document).ready(function () {
    kendo.culture('en-IN');
    GetPayProcessDetails();
});

function fncallGetPayProcessData(id) {
    if (id.toString().indexOf('lnkPrev') > 0) {
        var prevmonth = parseInt($('[id$="hdnMonth"]').val()) - 1;
        GetPayProcessDetails(prevmonth);
    }
    else {
        var nxtmonth = parseInt($('[id$="hdnMonth"]').val()) + 1;
        GetPayProcessDetails(nxtmonth);
    }


}
//////////////////////////////     PayProcess Details ///////////////////////////
function GetPayProcessDetails(month) {
    var monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
    ];

    //var LocationId = $('[id$="hdLocationId"]').val();
    var EmpId = 0;

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth(); //January is 0!
    var year = today.getFullYear();

    if (month == undefined) {
        mm = mm;
    }
    else {
        mm = month;
    }



    var d1 = new Date(year, mm - 1, 1)
    $('[id$="lblmonthyear"]').html(monthNames[d1.getMonth()] + ' ' + d1.getFullYear());
    $('[id$="hdnMonth"]').val(mm);
 
    $.ajax({
        type: "POST",
        url: "PayProcess.aspx/BindPayrProcessDetails",
        contentType: "application/json;charset=utf-8",
        //data: "{'EmpId':'" + EmpId + "','year':'" + year + "','locationId':'" + LocationId + "','month':'" + parseInt(mm) + "'}",
        data: "{'EmpId':'" + EmpId + "','year':'" + d1.getFullYear() + "','month':'" + parseInt(d1.getMonth() + 1) + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetPayProcessData(jQuery.parseJSON(msg.d));
            $('[id$="lblPayComment"]').html('');
            //$('[id$="txtPayComment"]').html('');
            if ((msg.d) != null && (msg.d) != "[]") {
                $('[id$="divBtnStmt"]').show();
                $('[id$="hdnPayID"]').val(eval(msg.d)[0].PayId);
                //alert((eval(msg.d)[0]).Comment);
                $('[id$="lblPayComment"]').html((eval(msg.d)[0]).Comment);
                //$('[id$="txtPayComment"]').html((eval(msg.d)[0]).Comment);
            }
            else {
                $('[id$="hdnPayID"]').val('');
                $('[id$="divBtnStmt"]').hide();
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed. "
                  + x.responseText);
        }
    });
}

function GetPayProcessData(Tdata) {
    kendo.culture("en-IN");
     $("#grdPayProcess").kendoGrid({
        height: 500,
        dataSource: {
            data: Tdata,
            schema: {
                model: {
                    fields: {
                        EmpID: {
                            type: "number",
                            editable: false
                        },
                        EmpName: {
                            type: "string"
                        },
                        Basic: {
                            type: "number"
                        },
                        HRA: {
                            type: "number"
                        },
                        Conveyance: {
                            type: "number"
                        },
                        Medical: {
                            type: "number"
                        },
                        Food: {
                            type: "number"
                        },
                        Special: {
                            type: "number"
                        },
                        PF: {
                            type: "number"
                        },
                        LTA: {
                            type: "number"
                        },
                        PT: {
                            type: "number"
                        },
                        Insurance: {
                            type: "number"
                        },
                        Loan: {
                            type: "number"
                        },
                        Advance: {
                            type: "number"
                        },
                        Leaves: {
                            type: "number"
                        },
                        Presents: {
                            type: "number"
                        },
                        Tax: {
                            type: "number"
                        },
                        Deduction: {
                            type: "number"
                        },
                        Bonus: {
                            type: "number"
                        },
                        Addition: {
                            type: "number"
                        },
                        Remark: {
                            type: "string"
                        },
                        CTC: {
                            type: "number"
                        },
                        Gross: {
                            type: "number"
                        },
                        Net: {
                            type: "number"
                        },
                        TotalAddition: {
                            type: "number"
                        },
                        TotalDeduction: {
                            type: "number"
                        },
                        Comment: {
                            type: "string"
                        },
                         CalcBasic: {
                            type: "number"
                         },
                         CalcGross: {
                             type: "number"
                         },
                    }
                }
            },
            aggregate: [
        { field: "CTC", aggregate: "sum" },
        { field: "Gross", aggregate: "sum" },
        { field: "CalcGross", aggregate: "sum" },
        { field: "Net", aggregate: "sum" },
            ],
            //pageSize: 100,
        },
        scrollable: true,
        sortable: true,
        //pageable: {
        //    input: true,
        //    numeric: false
        //},
        columns: [
      {
          field: "EmpName",
          title: "Name",
          width: "100px",
          attributes: {
              style: "background-color: lightgray;"
          },
          //locked: true
          //lockable: false
      }, {
          field: "EmpID",
          title: "ID",
          width: "50px",
          attributes: {
              style: "background-color: lightgray;"
          },
          //locked: true
      },  {
          field: "Gross",
          title: "Gross",
          width: "80px",
          template: '<div class="ra">#= kendo.toString(Gross,"C0") #</div>',
          footerTemplate: 'Sum of Gross: #=  kendo.toString(sum,"C0") # ',
          attributes: {
              style: "background-color: lightgray;"
          },
      },
       {
           field: "CalcGross",
           title: "Calculated Gross",
           width: "80px",
           template: '<div class="ra">#= kendo.toString(CalcGross,"n0") #</div>',
           footerTemplate: 'Sum of Calculated Gross: #=  kendo.toString(sum,"C0") # ',
           attributes: {
               style: "background-color: lightgray;"
           },
       }, {
          field: "Net",
          title: "Net Salary",
          width: "80px",
          template: '<div class="ra">#= kendo.toString(Net,"n0") #</div>',
          footerTemplate: 'Sum of Net Salary: #= kendo.toString(sum,"n0") # ',
          attributes: {
              style: "background-color: lightgray;"
          },
      },
      {
          field: "CTC",
          title: "CTC",
          width: "80px",
          culture: "en-IN", format: "{0:c}",
          template: '<div class="ra">#= kendo.format("{0:c0}",CTC) #</div>',
          footerTemplate: 'Sum of CTC: #=  kendo.format("{0:c0}",sum) # ',
          attributes: {
              style: "background-color: lightgray;"
          },
          //template: '<div class="ra">#= kendo.toString(CTC,"n0") #</div>',
          //footerTemplate: 'Sum of CTC: #=  kendo.toString(sum,"n0") # ',

      },
        {
            field: "TotalAddition",
            title: "Total Addition",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(TotalAddition,"n0") #</div>',
            attributes: {
                style: "background-color: lightpink;"
            },
        },
        {
            field: "TotalDeduction",
            title: "Total Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(TotalDeduction,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        },

        {
            field: "Basic",
            title: "Basic",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Basic,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "CalcBasic",
            title: "Calculated Basic",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(CalcBasic,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "HRA",
            title: "HRA",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(HRA,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Conveyance",
            title: "Conveyance",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Conveyance,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "Medical",
            title: "Medical",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Medical,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Food",
            title: "Food",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Food,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "Special",
            title: "Special",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Special,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        }, {
            field: "LTA",
            title: "LTA",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(LTA,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "Days",
            title: "Days",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Days,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },
        {
            field: "Leaves",
            title: "Leaves",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Leaves,"n0") #</div>',
            attributes: {
                style: "background-color: lightgray;"
            },
        },

        {
             field: "Presents",
             title: "Present",
             width: "80px",
             template: '<div class="ra">#= kendo.toString(Presents,"n0") #</div>',
             attributes: {
                 style: "background-color: lightgray;"
             },
         },
        {
            title: "Deductions",
            columns: [
                {
                    field: "PT",
                    title: "PT",
                    width: "80px",
                    template: '<div class="ra">#= kendo.toString(PT,"n0") #</div>',
                    attributes: {
                        style: "background-color: lightblue;"
                    },
                }, {
                    field: "Insurance",
                    title: "Insurance",
                    width: "80px",
                    template: '<div class="ra">#= kendo.toString(Insurance,"n0") #</div>',
                    attributes: {
                        style: "background-color: lightblue;"
                    },
                }, {
                    field: "PF",
                    title: "PF",
                    width: "80px",
                    template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>',
                    attributes: {
                        style: "background-color: lightblue;"
                    },
                },
                        {
                            field: "Ded_Leave",
                            title: "Leaves Deduction",
                            width: "80px",
                            template: '<div class="ra">#= kendo.toString(Ded_Leave,"n0") #</div>',
                            attributes: {
                                style: "background-color: lightblue;"
                            },
                        },
        {
            field: "Loan",
            title: "Loan Installment",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Loan,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Advance",
            title: "Advance",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Advance,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Tax",
            title: "Income Tax Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Tax,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }, {
            field: "Deduction",
            title: "Other Deduction",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Deduction,"n0") #</div>',
            attributes: {
                style: "background-color: lightblue;"
            },
        }
            ]
        },
        {
            title: "Additions",
            columns: [
        {
            field: "Bonus",
            title: "Bonus",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Bonus,"n0") #</div>',
            attributes: {
                style: "background-color: lightpink;"
            },
        }, {
            field: "Addition",
            title: "Other Addition",
            width: "80px",
            template: '<div class="ra">#= kendo.toString(Addition,"n0") #</div>',
            attributes: {
                style: "background-color: lightpink;"
            },
        }]
        },
         {
             field: "Remark",
             title: "Remark",
             width: "80px",
             attributes: {
                 style: "background-color: lightgreen;"
             },
         }
        //{
        //    field: "Remark",
        //    title: "Remark",
        //    width: "100px",
        //}, {
        //    field: "TotalAddition",
        //    title: "Total Addition",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(TotalAddition,"n0") #</div>'
        //}, {
        //    field: "TotalDeduction",
        //    title: "Total Deduction",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(TotalDeduction,"n0") #</div>'
        //}, {
        //    field: "Basic",
        //    title: "Basic",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Basic,"n0") #</div>'
        //}, {
        //    field: "HRA",
        //    title: "HRA",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(HRA,"n0") #</div>'
        //}, {
        //    field: "Conveyance",
        //    title: "Conveyance",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Conveyance,"n0") #</div>'
        //},
        //{
        //    field: "Medical",
        //    title: "Medical",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Medical,"n0") #</div>'
        //}, {
        //    field: "Food",
        //    title: "Food",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Food,"n0") #</div>'
        //}, {
        //    field: "Special",
        //    title: "Special",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Special,"n0") #</div>'
        //}, {
        //    field: "LTA",
        //    title: "LTA",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(LTA,"n0") #</div>'
        //}, {
        //    field: "PF",
        //    title: "EPF",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>'
        //}, {
        //    field: "PF",
        //    title: "PF",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(PF,"n0") #</div>'
        //}, {
        //    field: "PT",
        //    title: "PT",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(PT,"n0") #</div>'
        //}, {
        //    field: "Insurance",
        //    title: "Insurance",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Insurance,"n0") #</div>'
        //},
        //{
        //    field: "Loan",
        //    title: "Loan",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Loan,"n0") #</div>'
        //}, {
        //    field: "Advance",
        //    title: "Advance",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Advance,"n0") #</div>'
        //}, {
        //    field: "Deduction",
        //    title: "Deduction",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Deduction,"n0") #</div>'
        //},
        //{
        //    field: "Leaves",
        //    title: "Leaves",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Leaves,"n0") #</div>'
        //}, {
        //    field: "Presents",
        //    title: "Presents",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Presents,"n0") #</div>'
        //}, {
        //    field: "Bonus",
        //    title: "Bonus",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Bonus,"n0") #</div>'
        //}, {
        //    field: "Addition",
        //    title: "Addition",
        //    width: "80px",
        //    template: '<div class="ra">#= kendo.toString(Addition,"n0") #</div>'
        //},
        ],

        filterable: {
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    contains: "Contains",
                    eq: "Is equal to"
                }
            }
        },
        cancel: function (e) {
            e.preventDefault()
            ClosingRateWindow(e);
        },
     });
     var footer = $('#grdPayProcess').find('.k-grid-footer');
     var Header = $('#grdPayProcess').find('.k-grid-header');
     footer.insertAfter(Header);
}


function CheckPayProcess(id, name, curBugID) {
    $.ajax(
       {
           type: "POST",
           url: "PayAdd.aspx/IFExistsPayProcess",
           contentType: "application/json;charset=utf-8",
           data: "{'bugFileId':'" + id + "','bugFileName':'" + name + "'}",
           dataType: "json",
           cache: false,
           async: false,
           success: function (msg) {
               var detailObj = jQuery.parseJSON(msg.d)
               $.each(detailObj, function (key, output) {
                   if (output == "false") {
                       alert('You just added this file.Wait for 1 minute');
                       return false;
                   }
                   else if (output == "true") {
                       alert('File deleted successfully.');
                       GetAttachment(curBugID);
                       return false;
                   }

               });
           },
           error: function (x, e) {
               alert("The call to the server side failed. "
                     + x.responseText);
           }
       }
);


}