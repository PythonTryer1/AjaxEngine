/*
* Author:Houfeng
* Email:admin@xhou.net
*/
(function (env) {
    if (!env.jQuery) {
        throw "jQuery not found.";
    }
    var owner = env.AjaxEngine = env.AjaxEngine || {};
    owner.onRequestBegin = owner.onRequestBegin || env.onRequestBegin;
    owner.onRequestEnd = owner.onRequestEnd || env.onRequestEnd;
    owner.wrapUrl = owner.wrapUrl || function (url) {
        var app = this;
        if (url.indexOf('?') > -1)
            url += "&__t=" + Math.random();
        else
            url += "?__t=" + Math.random();
        return url;
    };
    owner.isArray = function (obj) {
        var v1 = Object.prototype.toString.call(obj) === '[object Array]';
        var v2 = obj instanceof Array;
        var v3 = (obj.length instanceof Array) && obj[0];
        var v3 = (obj.length instanceof Array) && (typeof obj.splice === 'function');
        return v1 || v2 || v3 || v4;
    };
    owner.stringToJson = owner.stringToJson || function (str) {
        if (env.JSON && env.JSON.parse) {
            return env.JSON.parse(str);
        }
        return (new Function("return " + str + ";"))();
    };
    owner.jsonToString = owner.jsonToString || function (obj) {
        if (env.JSON && env.JSON.stringify) {
            return env.JSON.stringify(obj);
        }
        var THIS = this;
        switch (typeof (obj)) {
            case 'string':
                return '"' + obj.replace(/(["\\])/g, '\\$1') + '"';
            case 'array':
                return '[' + obj.map(THIS.jsonToString).join(',') + ']';
            case 'object':
                if (owner.isArray(obj)) {
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
                    for (var p in obj) {
                        string.push(THIS.jsonToString(p) + ':' + THIS.jsonToString(obj[p]));
                    }
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
    owner.ajax = owner.ajax || env.jQuery.ajax;
    owner.callService = owner.callService || function (url, data, callback) {
        if (owner.onRequestBegin)
            owner.onRequestBegin();
        var formData = [];
        for (var name in data) {
            var value = (typeof data[name] === "string") ? data[name] : owner.jsonToString(data[name]);
            formData.push({ "name": name, "value": value });
        }
        var reutrnResult = null;
        owner.ajax({
            type: "post",
            url: owner.wrapUrl(url),
            async: callback != null,
            cache: false,
            data: formData,
            dataType: "json",
            success: function (result, status) {
                if (owner.onRequestEnd) {
                    owner.onRequestEnd();
                }
                reutrnResult = result;
                if (callback) callback(reutrnResult);
            }
        });
        return reutrnResult;
    };
}(this));
