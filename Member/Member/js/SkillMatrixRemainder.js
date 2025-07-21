$(document).ready(function () {
    //onLoad();

    ////ddlRemainder
    $('[id$="ddlRemainder"]').change(function () {
        RestCookieTime();
    });
});


setInterval(CheckCookieTime, 60000);

/// <reference path="../Admin.master" />
function CheckCookieTime() {
    var EmpID = $('[id$="hdnEmpID"]').val();
    
    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/CheckCookieTime",
        contentType: "application/json;charset=utf-8",
        data: "{EmpID:'" + 0 + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            if (msg.d != "0") {
                //$('[id$="divMatter"]').val(msg.d);
                $('#divOverlay').css('display', '');
                $(".overlay").fadeIn();
                $('#divRemainderPopup').css('display', "block").addClass("animated");
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed1. "
                  + x.responseText);           
        }
    });
}


/////////////////////
function RestCookieTime() {
    var RTime = $('[id$="ddlRemainder"]').val()
    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/RestCookieTime",
        contentType: "application/json;charset=utf-8",
        data: "{RTime:'" + RTime + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            $('#divRemainderPopup').slideUp("slow");
            //runEffect();
            $(".overlay").fadeOut(600);
            //$('#divOverlay').css('display', 'none');
        },
        error: function (x, e) {
            alert("The call to the server side failed2. "
                  + x.responseText);
        }
    });
}



function runEffect() {
    // get effect type from
    var selectedEffect = "explode";// $("#effectTypes").val();

    // most effect types need no options passed by default
    var options = {};
    // some effects have required parameters
    if (selectedEffect === "scale") {
        options = { percent: 0 };
    } else if (selectedEffect === "size") {
        options = { to: { width: 200, height: 60 } };
    }

    // run the effect
    $("#divRemainderPopup").hide(selectedEffect, options, 1000, callback);
};

// callback function to bring a hidden box back
function callback() {
    setTimeout(function () {
        $("#divRemainderPopup").removeAttr("style").hide().fadeIn();
    }, 1000);
};


//------------------------------------------------------TO HIGHLIGHT SKILLS----------------------------------------//
function GetSkillByID() {
    var skillID = $('[id$="hdnSkillID"]').val();
    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/GetSkillByID",
        contentType: "application/json;charset=utf-8",
        data: "{skillID:'" + skillID + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            data = data.d;
            $('[id$="lblcategory"]').text(data.Category);
            $('[id$="lblSkill"]').text(data.SkillName);
            $('[id$="txtYear"]').val(0);

        },
        error: function (x, e) {
            alert("The call to the server side failed1. "
                  + x.responseText);
        }
    });
}

function SaveSkillHighlight() {
    var experience = 0;
    var empID = $('[id$="hdnLoginID"]').val();
    var skillID = $('[id$="hdnSkillID"]').val();
    var year = $('[id$="txtYear"]').val();
    var mnth = $('[id$="ddlMnth"]').val();
    var level = $('[id$="ddlLevel"]').val();
    if (year == "")
        year = 0;
    experience = parseInt(year) * 12 + parseInt(mnth);

    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/SaveSkillHighlight",
        contentType: "application/json;charset=utf-8",
        data: "{empID:'" + empID + "',skillID:'" + skillID + "',experience:'" + experience + "',level:'" + level + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            if (msg.d != "0") {
                //alert("saved successfully");
                //$('[id$="divSkilldata"]').html('');
                //$('[id$="divSkilldata"]').html(msg.d);
                $('#' + skillID).remove();
                ClosePopUp();
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed1. "
                  + x.responseText);
        }
    });
}

function SaveSkillByChk(id)
{
    $('[id$="hdnSkillID"]').val(id);
    $('[id$="ddlMnth"]').val(0);
    $('[id$="txtYear"]').val(0);
    SaveSkillHighlight();
}

function RightSidePaging() {
    var pageNum = $('[id$="hdnPageNum"]').val();
    //var skillID = $('[id$="hdnSkillID"]').val();    

    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/RightSidePaging",
        contentType: "application/json;charset=utf-8",
        data: "{pageNum:'" + parseInt(pageNum) + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            if (msg.d != "0") {
                $('[id$="divContainer"]').html(msg.d);
                pageNum = parseInt(pageNum) + 1;
                $('[id$="hdnPageNum"]').val(pageNum);
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed1. "
                  + x.responseText);
        }
    });
}

function LeftSidePaging() {
    var pageNum = $('[id$="hdnPageNum"]').val();
    //var skillID = $('[id$="hdnSkillID"]').val();    
    if (pageNum == "1")
        return;
    
    $.ajax({
        type: "POST",
        url: "SkillMatrix.aspx/LeftSidePaging",
        contentType: "application/json;charset=utf-8",
        data: "{pageNum:'" + parseInt(pageNum) + "'}",
        dataType: "json",
        async: false,
        success: function (msg) {
            if (msg.d != "0") {
                $('[id$="divContainer"]').html(msg.d);
                pageNum = parseInt(pageNum) - 1;
                $('[id$="hdnPageNum"]').val(pageNum);
            }
        },
        error: function (x, e) {
            alert("The call to the server side failed1. "
                  + x.responseText);
        }
    });
}

function OpenPopup(id) {
    //alert(id);
    $('[id$="hdnSkillID"]').val(id);
    $('#divOpenPopup').css('display', '');
    $('#divAddPopupOverlay').css('display', '');
    GetSkillByID();
}
function ClosePopUp() {
    $('#divAddPopupOverlay').css('display', 'none');

    $('#divOpenPopup').slideUp("slow");
    //$(".divAddPopupOverlay").fadeOut(600);
}