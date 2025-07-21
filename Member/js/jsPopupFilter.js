jQuery(function ($) {

    $("a.topopup").click(function () {


        var link = $(this);
        var title = link.attr('title');

        var Empid = $("#ctl00_ContentPlaceHolder1_hdnUserID").val();

        $.ajax(
                        {
                            type: "POST",
                            url: "Filter.aspx/FilterData",
                            contentType: "application/json;charset=utf-8",
                            data: "{ 'strEmpID': '"+Empid+"', 'strColumn': '"+title+"' }",
                            dataType: "json",
                            success: function (msg) {
                                loading(title,jQuery.parseJSON(msg.d));
                            },
                            error: function (x, e) {
                                alert("The call to the server side failed. "
                                      + x.responseText);
                            }
                        }
            );


      // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup(); // function show popup
        }, 500); // .5 second
        return false;
    });

    /* event for close the popup */
    $("div.close").hover(
                    function () {
                        $('span.ecs_tooltip').show();
                    },
                    function () {
                        $('span.ecs_tooltip').hide();
                    }
                );

    $("#toPopup .close").click(function () {
        alert('@#');
        disablePopup();  // function close pop up
    });

    $(this).keyup(function (event) {
        if (event.which == 27) { // 27 is 'Ecs' in the keyboard
            disablePopup();  // function close pop up
        }
    });


    /************** start: functions. **************/
    function loading(title, listArray) {
        var listStr = "<div><input type='checkbox' class='chkFilterAll' checked='checked' />ALL</div>";
        for (var i = 0; i < listArray.length; i++) {
            listStr += "<div class='checkboxListWrap'><input type='checkbox' checked='checked' />" + listArray[i] + "</div>";
        }
        $("#popup_content #ColName").html(title);
        $("#popup_content #checkList").html(listStr);
        $(".chkFilterAll").change(function () {
            if ($(this).prop("checked")) {
                $(".checkboxListWrap input").each(function () {
                    $(this).prop('checked', true);
                });
            }
            else
            {
                $(".checkboxListWrap input").each(function () {
                    $(this).prop('checked', false);
                });
            }
        });
        $(".checkboxListWrap").change(function () {
            $(".chkFilterAll").each(function () {
                $(this).prop('checked', false);
            });
        });
        $("div.loader").show();

    }
    function closeloading() {
        $("div.loader").fadeOut('normal');
    }

    var popupStatus = 0; // set value

    function loadPopup() {
        if (popupStatus == 0) { // if value is 0, show popup
            closeloading(); // fadeout loading
            $("#toPopup").fadeIn(0500); // fadein popup div
            $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
            $("#backgroundPopup").fadeIn(0001);
            popupStatus = 1; // and set value to 1
        }
    }

    function disablePopup() {
        if (popupStatus == 1) { // if value is 1, close popup
            $("#toPopup").fadeOut("normal");
            $("#backgroundPopup").fadeOut("normal");
            popupStatus = 0;  // and set value to 0
        }
    }
    /************** end: functions. **************/
}); // jQuery End