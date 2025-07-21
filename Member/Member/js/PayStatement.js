function CloseMailBox() {
    $('#divMail').css('display', 'none');
    $('#divAddPopupOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");


}
function ValidateEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);
};

function sendmail() {

    var To = $('[id$="txtTo"]').val();
    var Cc = $('[id$="txtCc"]').val();
    var Bcc = $('[id$="txtBcc"]').val();
    var Subject = $('[id$="txtSubject"]').val();
    var editor = $('[id$="txtEmail"]').data("kendoEditor");
    var MsgBody = editor.value();

    if ($('.red').length != 0) {
        $('.red').html('');
    }

    if (To == "") {
        $('[id$="txtTo"]').after('<div class="red" style="color:red">Please enter recipient email id</div>');
        return false;
    }

    else if (To != "") {

        if (!ValidateEmail($('[id$="txtTo"]').val())) {
            $('[id$="txtTo"]').after('<div class="red" style="color:red">Please enter valid email id</div>');
            return false;
        }
        else {
            $.ajax({

                type: "POST",
                url: "PayStatement.aspx/SendMail",
                contentType: "application/json;charset=utf-8",
                data: "{To:'" + To + "',Cc:'" + Cc + "',Bcc:'" + Bcc + "',Subject:'" + Subject + "',MsgBody:'" + MsgBody + "'}",
                cache: false,
                async: false,
                dataType: "json",
                success: function (msg) {
                    var message = msg.d;
                    if (message != 'Mail Sent')
                        alert("Mail sent Failed.");
                    else
                        alert("Mail sent successfully.");
                    $('#divMail').css('display', 'none');
                    $('body').css('cursor', 'default');

                },
                error: function (msg) {
                    alert("Mail sent failed."
                          + msg.responseText);
                }
            });
        }

    }
}

function BindMail() {
    $('[id$="dvAttachnemt"]').text("");
    $.ajax(
           {
               type: "POST",
               url: "PayStatement.aspx/GetMailInfo",
               contentType: "application/json;charset=utf-8",
               data: "",
               cache: false,
               async: false,
               dataType: "json",
               success: function (msg) {
                   var message = jQuery.parseJSON(msg.d);
                   var ccName = message.ccName;
                   var filePath = message.filePath;
                   var description = message.description;
                   var subject = message.subject;
                   $('[id$="txtTo"]').val("");
                   $('[id$="txtCc"]').val(ccName);
                   $('[id$="txtSubject"]').val(subject);
                   var editor = $('[id$="txtEmail"]').data("kendoEditor");
                   editor.value(description);
                   var name = message.fileName;
                   var Anchor = $('<a target="_blank" href="../Common/Download.aspx?m=Pay&f=' + name + '">' + name + '</a>');
                   $('[id$="dvAttachnemt"]').append(Anchor);
                   $('#divMail').css('display', '');
                   $('#divAddPopupOverlay').addClass('k-overlay');
                   $('body').css('cursor', 'default');
               },
               error: function (msg) {
                   alert("The call to the server side failed."
                         + msg.responseText);
               }
           }
        );
}