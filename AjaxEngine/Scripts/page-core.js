/// <reference path="jquery.js" />

/*
* Author:Houfeng
* Email:admin@xhou.net
*/

if (navigator.userAgent.indexOf("Gecko/") > -1) {
    HTMLElement.prototype.__defineGetter__("outerHTML", function () {
        var a = this.attributes, str = "<" + this.tagName, i = 0; for (; i < a.length; i++)
            if (a[i].specified)
                str += " " + a[i].name + '="' + a[i].value + '"';
        if (!this.canHaveChildren)
            return str + " />";
        return str + ">" + this.innerHTML + "</" + this.tagName + ">";
    });
    HTMLElement.prototype.__defineSetter__("outerHTML", function (s) {
        var r = this.ownerDocument.createRange();
        r.setStartBefore(this);
        var df = r.createContextualFragment(s);
        this.parentNode.replaceChild(df, this);
        return s;
    });
    HTMLElement.prototype.__defineGetter__("canHaveChildren", function () {
        return !/^(area|base|basefont|col|frame|hr|img|br|input|isindex|link|meta|param)$/.test(this.tagName.toLowerCase());
    });
}

//---
window.AjaxEngine = window.AjaxEngine || {};
(function (owner) {
    owner.onRequestBegin =owner.onRequestBegin || window.onRequestBegin;
    owner.onRequestEnd = owner.onRequestEnd||window.onRequestEnd;
    owner.wrapUrl =owner.wrapUr || function (url) {
        var app = this;
        if (url.indexOf('?') > -1)
            url += "&__t=" + Math.random();
        else
            url += "?__t=" + Math.random();
        return url;
    };
    owner.stringToJson =owner.stringToJson || function (str) {
        var jsonObject = (new Function("return " + str + ";"))();
        return jsonObject;
    };
    owner.jsonToString =owner.jsonToString|| function (obj) {
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
    owner.$ = owner.$|| function (id) {
        return document.getElementById(id);
    };
    owner.ajax =owner.ajax || $.ajax;
    owner.serializeData = owner.serializeData || function () {
        var formData = $(theForm).serializeArray();
        theForm.__EVENTTARGET.value = "";
        theForm.__EVENTARGUMENT.value = "";
        return formData;
    };
    owner.callServer = owner.callServer || function (data, callback) {
        if (owner.onRequestBegin)
            owner.onRequestBegin();
        var formData = owner.serializeData();
        formData["AjaxRequest"] = "true";
        formData.push({ "name": "AjaxRequest", "value": "true" });
        for (var name in data) {
            var value = (typeof data[name] === "string") ? data[name] : owner.jsonToString(data[name]);
            formData.push({ "name": name, "value": value });
        }
        //--
        var reutrnResult = null;
        owner.ajax({
            type: "post",
            url: owner.wrapUrl(location.href.indexOf('?')>-1?location.href:theForm.action),
            async: callback != null,
            cache: false,
            data: formData,
            dataType: "json",
            success: function (result) {
                reutrnResult = owner.processResult(result);
                if (callback)
                    callback(reutrnResult);
            }
        });
        return reutrnResult;
    };
    owner.processResult =  owner.processResult || function (msgList) {
        var returnResult = null;
        if (!msgList) return returnResult;
        for (var i in msgList) {
            var msg = msgList[i];
            if (msg.Type === 0) {
                returnResult = msg.Context;
            }
            else if (msg.Type === 1) {
                var element = AjaxEngine.$(msg.Id);
                if (element && element.outerHTML)
                    element.outerHTML = msg.Context;
            }
            else if (msg.Type === 2) {
                //window.console.log(msg.Context);
                eval(msg.Context);
            }
        }
        if (owner.onRequestEnd)
            owner.onRequestEnd();
        return returnResult;
    };
    owner.doPostBack =  owner.doPostBack || function (eventTarget, eventArgument) {
        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
            theForm.__EVENTTARGET.value = eventTarget;
            theForm.__EVENTARGUMENT.value = eventArgument;
            var target = owner.$(eventTarget);
            if (target && target.getAttribute && target.getAttribute("notAjax") == "notAjax") {
                theForm.submit();
            }
            else {
                owner.callServer({}, function (result) { });
            }
        }
    };
    //
    if (__ControlAjaxEnabled)
        __doPostBack = owner.doPostBack;
} (window.AjaxEngine));
