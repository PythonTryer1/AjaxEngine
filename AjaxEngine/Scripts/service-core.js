/// <reference path="jquery.js" />

/*
* Author:Houfeng
* Email:admin@xhou.net
*/
window.AjaxEngine = window.AjaxEngine || {};
(function (owner) {
    owner.onRequestBegin =owner.onRequestBegin || window.onRequestBegin;
    owner.onRequestEnd = owner.onRequestEnd||window.onRequestEnd;
    owner.wrapUrl = owner.wrapUrl || function (url) {
        var app = this;
        if (url.indexOf('?') > -1)
            url += "&__t=" + Math.random();
        else
            url += "?__t=" + Math.random();
        return url;
    };
    owner.stringToJson = owner.stringToJson || function (str) {
        var jsonObject = (new Function("return " + str + ";"))();
        return jsonObject;
    };
    owner.jsonToString =  owner.jsonToString || function (obj) {
        var THIS = this;
        switch (typeof (obj)) {
            case 'string':
                return '"' + obj.replace(/(["\\])/g, '\\$1') + '"';
            case 'array':
                return '[' + obj.map(THIS.jsonToString).join(',') + ']';
            case 'object':
                if (obj instanceof Array) {
                    var strArr = [];
                    var len = obj.length;
                    for (var i = 0; i < len; i++) {
                        strArr.push(THIS.jsonToString(obj[i]));
                    }
                    return '[' + strArr.join(',') + ']';
                } else if (obj == null || obj == undefined) {
                    return 'null';
                } else {
                    var string = [];
                    for (var p in obj)
                        string.push(THIS.jsonToString(p) + ':' + THIS.jsonToString(obj[p]));
                    return '{' + string.join(',') + '}';
                }
            case 'number':
                return obj;
            case 'boolean':
                return obj;
            case false:
                return obj;
        }
    };
    owner.ajax = owner.ajax || $.ajax;
    owner.callService = owner.callService || function (url, data, callback) {
        if (owner.onRequestBegin)
            owner.onRequestBegin();
        var formData = [];
        for (var name in data) {
            var value = (typeof data[name] === "string") ? data[name] : owner.jsonToString(data[name]);
            formData.push({ "name": name, "value": value });
        }
        //alert(owner.jsonToString(formData));
        //--
        var reutrnResult = null;
        owner.ajax({
            type: "post",
            url: owner.wrapUrl(url),
            async: callback != null,
            cache: false,
            data: formData,
            dataType: "json",
            success: function (result) {
                if (owner.onRequestEnd)
                    owner.onRequestEnd();
                reutrnResult = result;
                if (callback)
                    callback(reutrnResult);
            }
        });
        return reutrnResult;
    };
} (window.AjaxEngine));
