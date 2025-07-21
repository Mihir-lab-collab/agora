

function SendObjRequest(_objName, _methodName, _parameters, _doneCallBack, _callBackArgs) {
    CallJSONPostData(GetStringifyObject(NewRequestObj(_objName, _methodName, _parameters)), function (data) {
        _doneCallBack(data.RetObj, _callBackArgs);
    });
}
//function SendObjRequest(_objName, _methodName, _parameters, _doneCallBack, _callBackArgs, _errorFunction) {
//    CallJSONPostData(GetStringifyObject(NewRequestObj(_objName, _methodName, _parameters)), function (data) {
//       _doneCallBack(data.RetObj, _callBackArgs);

//        //return (data.RetObj, _callBackArgs)
//    }, function (data) {
//        _errorFunction(data, _callBackArgs);
//    });
//}

function NewRequestObj(_objName, _fnName, _parameters) {
    var ReqObj = new Object();
    ReqObj.ObjNm = _objName;
    ReqObj.FnNm = _fnName;
    ReqObj.Params = _parameters;
    return ReqObj;
}
function GetStringifyObject(_obj) {
    return JSON.stringify(_obj);
}
function CallJSONPostData(_stringifyData, _successFn, _errorFn) {
    CallJSON("POST", _stringifyData, _successFn, _errorFn);
}
function CallJSON(_method, _stringifyData, _successFn, _errorFn) {
    $.ajax({
        async: true,
        type: _method,
        url: "/Agora_dev/Member/Handler.aspx/BindGrid",
        data: "{ _reqObj :" + _stringifyData + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var retCode = jQuery.parseJSON(data.d);
            if (retCode.StatusStr == "OK") {
                if (_successFn != undefined) {
                    _successFn(retCode);
                }
            }
            else {
                if (_errorFn == undefined) {
                    alert(retCode.Msg);
                }
                else {
                    _errorFn(retCode);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (_errorFn != undefined) {
                alert(_errorFn);
                //jAlert('error', 'An unexpected error has occured. Please contact your System Administrator.', 'Error');
            }
        }
    });
}
