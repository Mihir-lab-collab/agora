
$(document).ready(function () {
    $('[id$="txtEmployeeID"]').keypress(function (e) {
        if ($('.red').length != 0) {
            $('.red').html('');
        }
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            $('[id$="txtEmployeeID"]').after('<div class="red" style="color:red">Only digits are  allowed</div>');
            return false;
        }
    });

    $('[id$="SearchYear"]').keypress(function (e) {
        if ($('.red').length != 0) {
            $('.red').html('');
        }
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            $('[id$="SearchYear"]').after('<div class="red" style="color:red">Only digits are  allowed</div>');
            return false;
        }
    });
});

function Search() {

    if ($('.red').length != 0) {
        $('.red').html('');
    }

    var d = new Date();
    var currmonth = d.getMonth() + 1;
    var currday = d.getDate();
    var curryear = d.getFullYear();

    var output = d.getFullYear() + '/' +
        (('' + currmonth).length < 2 ? '0' : '') + currmonth + '/' +
        (('' + currday).length < 2 ? '0' : '') + currday;


    var empid = $('[id$="txtEmployeeID"]').val();
    var month = $('[id$="ddlMonth"]').val();
    var inputyear = $('[id$="SearchYear"]').val();

    $('[id*="hdnEmpID"]').val(empid);
    $('[id$="hdnMonth"]').val(month);
    $('[id$="hdnYear"]').val(inputyear);

    if(empid=="")
    {
        $('[id$="txtEmployeeID"]').after('<div class="red" style="color:red">Please Enter Employeed ID</div>');
        return false;
    }
    else if (!empid.match(/^[0-9]{4}$/)) {
        $('[id$="txtEmployeeID"]').after('<div class="red" style="color:red">Enter only digit for Employee ID</div>');
        return false;

    }
    else if (month == 0)
    {
        $('[id$="ddlMonth"]').after('<div class="red" style="color:red">Please Select Month</div>');
        return false;
    }
    
    else if (inputyear=="")
    {
        $('[id$="SearchYear"]').after('<div class="red" style="color:red">Please Enter Year</div>');
        return false;
    }
    else if (inputyear.length < 4 || inputyear.length > 4)
    {
        $('[id$="SearchYear"]').after('<div class="red" style="color:red">Year should be of 4 digit</div>');
            return false;
    }
    else if (!inputyear.match(/^[0-9]{4}$/))
    {
        $('[id$="SearchYear"]').after('<div class="red" style="color:red">Enter only digit in Year</div>');
        return false;

    }
    else if (inputyear.charAt(0) == 0) {
        $('[id$="SearchYear"]').after('<div class="red" style="color:red">First Digit of year should not be Zero</div>');
        return false;
    }
    else if (month!="" && inputyear!= "")
    {
        if (inputyear <= curryear) {
            if (currmonth > month && curryear <= inputyear) {

              
                return true;
            }
            else if (currmonth < month && curryear == inputyear) {
            
                $('[id$="ddlMonth"]').after('<div class="red" style="color:red">Can not generate pay slip for month  greater than current month</div>');
                return false;
            }
            else if (currmonth == month && curryear == inputyear) {

                $('[id$="ddlMonth"]').after('<div class="red" style="color:red">Can not generate pay slip for current month and year</div>');
                return false;
            }
        }
        else {
           
            $('[id$="SearchYear"]').after('<div class="red" style="color:red">Year should be less than current year</div>');
            return false;
        }
    }
}
