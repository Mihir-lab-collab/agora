

function CallJSON(_method,url ,data) {
    var retCode = "";
    $.ajax({
        async: true,
        type: "FilterProject",
        url: "Filter.aspx",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            retCode = jQuery.parseJSON(data.d);
            
            
        }
    });
    alert('1' + retCode);
    return retCode;
}

