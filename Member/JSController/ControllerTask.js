var Controller = {
    GetIncidentList: function (getParamerters, doneCallback, callbackArgs) {
        var data = [{ id: 1, name: "Jane Doe", age: 28 },
                    { id: 2, name: "Shon Doe", age: 22 },
                    { id: 3, name: "Sag Doe", age: 25 }]
        return data;
    },

    GetTaskManagerList: function (getParamerters, doneCallback, callbackArgs) {
        var data = [{ bugs_bug_id: 1, bugs_bug_name: "Jane Doe", bugs_date_assigned: 28, bugs_bug_lastmodified: 22,
                     projectName: "zzz", projectModule: "xxx", priorities_priority_desc: "cc", bugs_assigned_to: "xx", statuses_status: "cc"},
                     { bugs_bug_id: 1, bugs_bug_name: "Jane Doe", bugs_date_assigned: 28, bugs_bug_lastmodified: 22,
                     projectName: "zzz", projectModule: "ttt", priorities_priority_desc: "rr", bugs_assigned_to: "ee", statuses_status: "ww"
                     }]


        return data;
    },
    getTaskList: function (getParamerters, doneCallback, callbackArgs) {
        SendObjRequest("Contact", "GetSingleSearchResult", getParamerters, function (data)
        {
            return data;
        }
        , callbackArgs);
    },
}