/**********************************************************
 * 
 * AjaxEngine脚本资源类
 * -------------------------------------------------------
 * 作者:Houfeng(侯锋)
 * Email:Houzhanfeng@gmail.com
 * 
 *********************************************************/

using System;
using System.Text;

namespace Houfeng.Web.Ajax
{
    internal class Resources1
    {
        private Resources1()
        {
        }
        private static Resources1 _r;
        public static Resources1  Get()
        {
            if(_r ==null )
                _r =new Resources1();
            return _r ;
        }

        public string AjaxPublicScript
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("function  ____GetXmlHttp() {");
                sb.AppendLine("var xmlHttp;");
                sb.AppendLine("try {");
                sb.AppendLine("return new ActiveXObject( \"Msxml2.XMLHTTP.4.0\" );");
                sb.AppendLine("} catch( ex ) {}");
                sb.AppendLine("try {");
                sb.AppendLine("return new ActiveXObject( \"MSXML2.XMLHTTP\" );");
                sb.AppendLine("} catch( ex ){}");
                sb.AppendLine("try {");
                sb.AppendLine("return new ActiveXObject( \"Microsoft.XMLHTTP\" );");
                sb.AppendLine("} catch( ex ){}");
                sb.AppendLine("try {");
                sb.AppendLine("return new XMLHttpRequest();");
                sb.AppendLine("} catch( ex ){}");
                sb.AppendLine("return null;");
                sb.AppendLine("} ");
                sb.AppendLine("function  ____CallbackServerFunction(funname,pars,asyn,callbackfun)");
                sb.AppendLine("{");
                sb.AppendLine("var rs=null;");
                sb.AppendLine("try");
                sb.AppendLine("{");
                sb.AppendLine("funname=escape(funname);");
                sb.AppendLine("pars =escape(pars);");
                sb.AppendLine("var ____url=window.location.href.split('#')[0].split('?')[0];");
                sb.AppendLine("if(____url.indexOf('?')>-1)");
                sb.AppendLine("____url=____url+\"&temp=\"+(new Date()).getUTCMilliseconds();");
                sb.AppendLine("else ");
                sb.AppendLine("____url=____url+\"?temp=\"+(new Date()).getUTCMilliseconds();");
                sb.AppendLine("var xmlHttp=____GetXmlHttp();");
                sb.AppendLine("var content= null;");
                sb.AppendLine("if(pars!=null && pars!=\"\")");
                sb.AppendLine("content = \"Action=CallbackServerFunction&FunctionName=\"+funname+\"&Parameters=\"+pars;");
                sb.AppendLine("else ");
                sb.AppendLine("content = \"Action=CallbackServerFunction&FunctionName=\"+funname;");
                sb.AppendLine("xmlHttp.onreadystatechange=function(){");
                sb.AppendLine("if(xmlHttp.readyState == 4 ){");
                sb.AppendLine("if(xmlHttp.status == 200) {");
                sb.AppendLine("rs=unescape(xmlHttp.responseText);");
                sb.AppendLine("if(callbackfun!=null)");
                sb.AppendLine("callbackfun(rs);");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("try");
                sb.AppendLine("{");
                sb.AppendLine("xmlHttp.open(\"POST\",____url, asyn);");
                sb.AppendLine("}");
                sb.AppendLine("catch(ex1)");
                sb.AppendLine("{");
                sb.AppendLine("xmlHttp.Open(\"POST\",____url, asyn);");
                sb.AppendLine("}");
                sb.AppendLine("xmlHttp.setRequestHeader(\"Content-Length\",content.length);");
                sb.AppendLine("xmlHttp.setRequestHeader(\"CONTENT-TYPE\",\"application/x-www-form-urlencoded\");");
                sb.AppendLine("try");
                sb.AppendLine("{");
                sb.AppendLine("xmlHttp.send(content);");
                sb.AppendLine("}");
                sb.AppendLine("catch(ex2)");
                sb.AppendLine("{");
                sb.AppendLine("xmlHttp.Send(content);");
                sb.AppendLine("}");
                //
                sb.AppendLine("if(callbackfun==null && rs==null){");
                sb.AppendLine("rs=unescape(xmlHttp.responseText);");
                sb.AppendLine("}");
                //              
                sb.AppendLine("}");
                sb.AppendLine("catch(ex)");
                sb.AppendLine("{");
                sb.AppendLine("alert(\"Error:\"+ex);");
                sb.AppendLine("}");
                sb.AppendLine("return rs ;");
                sb.AppendLine("} ");
                return sb.ToString();
            }
        }

        public string AjaxEventCallBackScript
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("if(typeof(HTMLElement)!=\"undefined\" && !window.opera)");
                sb.AppendLine("{");
                sb.AppendLine("HTMLElement.prototype.__defineGetter__(\"outerHTML\",function(){");
                sb.AppendLine("var a=this.attributes, str=\"<\"+this.tagName, i=0;for(;i<a.length;i++)");
                sb.AppendLine("if(a[i].specified)");
                sb.AppendLine("str+=\" \"+a[i].name+'=\"'+a[i].value+'\"';");
                sb.AppendLine("if(!this.canHaveChildren)");
                sb.AppendLine("return str+\" />\";");
                sb.AppendLine("return str+\">\"+this.innerHTML+\"</\"+this.tagName+\">\";");
                sb.AppendLine("});");
                sb.AppendLine("HTMLElement.prototype.__defineSetter__(\"outerHTML\",function(s){");
                sb.AppendLine("var r = this.ownerDocument.createRange();");
                sb.AppendLine("r.setStartBefore(this);");
                sb.AppendLine("var df = r.createContextualFragment(s);");
                sb.AppendLine("this.parentNode.replaceChild(df, this);");
                sb.AppendLine("return s;");
                sb.AppendLine("});");
                sb.AppendLine("HTMLElement.prototype.__defineGetter__(\"canHaveChildren\",function(){");
                sb.AppendLine("return !/^(area|base|basefont|col|frame|hr|img|br|input|isindex|link|meta|param)$/.test(this.tagName.toLowerCase());");
                sb.AppendLine("});");
                sb.AppendLine("}");
                sb.AppendLine("var theForm = document.forms[0];");
                sb.AppendLine("if (!theForm) {");
                sb.AppendLine("theForm = document.getElementsByTagName('form')[0];");
                sb.AppendLine("}");
                sb.AppendLine("function __doPostBack(eventTarget, eventArgument) {");
                sb.AppendLine("theForm.__EVENTTARGET.value = eventTarget;");
                sb.AppendLine("theForm.__EVENTARGUMENT.value = eventArgument;");
                sb.AppendLine("var eventTargetObj=document.getElementById(eventTarget.replace(new RegExp(\"\\\$\",\"gm\"),\"_\"));");
                sb.AppendLine("if(eventTargetObj!=null && eventTargetObj.attributes['notAjax']!=null){");
                sb.AppendLine("if (!theForm.onsubmit || (theForm.onsubmit() != false)) {");
                sb.AppendLine("theForm.__NOTAJAX.value='notajax';");
                sb.AppendLine("theForm.submit();");
                sb.AppendLine("return false ;");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("else{ theForm.__NOTAJAX.value=''; }");
                sb.AppendLine("var eles = theForm.elements;");
                sb.AppendLine("var serializeData ={}");
                sb.AppendLine("for(var i = 0 ; i<eles.length ; i++ ){");
                sb.AppendLine("var tag = eles[i].tagName.toLowerCase();");
                sb.AppendLine("var type = eles[i].type;");
                sb.AppendLine("if ((tag == \"input\" && (type == \"text\" || type == \"hidden\")) ||tag == \"textarea\" || tag == \"select\"){");
                sb.AppendLine("serializeData[eles[i].name] = eles[i].value;");
                sb.AppendLine("}");
                sb.AppendLine("else if(type ==\"checkbox\" || type==\"radio\"){");
                sb.AppendLine("if(type==\"checkbox\"){");
                sb.AppendLine("if (!serializeData[eles[i].name] && eles[i].checked)");
                sb.AppendLine("serializeData[eles[i].name] = eles[i].value;");
                sb.AppendLine("else if (serializeData[eles[i].name]&&eles[i].checked)");
                sb.AppendLine("serializeData[eles[i].name] += \"-\"+eles[i].value;");
                sb.AppendLine("else if(serializeData[eles[i].name] && serializeData[eles[i].name] !=\"\"){}");
                sb.AppendLine("else");
                sb.AppendLine("serializeData[eles[i].name] = \"\";");
                sb.AppendLine("}");
                sb.AppendLine("else if(eles[i].type == \"radio\"){");
                sb.AppendLine("if(eles[i].checked)");
                sb.AppendLine("{   serializeData[eles[i].name] = eles[i].value;}");
                sb.AppendLine("else{");
                sb.AppendLine("if(!serializeData[eles[i].name])");
                sb.AppendLine("serializeData[eles[i].name] = \"\";");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("var serializeDataString=\"____AjaxSubmitForm=true\";");
                sb.AppendLine("for(p in serializeData){");
                sb.AppendLine("if(p!=\"__VIEWSTATE\")");
                sb.AppendLine("serializeDataString+=\"&\"+escape(p)+\"=\"+escape(serializeData[p]);");
                sb.AppendLine("else{");
                sb.AppendLine("serializeDataString+=\"&\"+(p)+\"=\"+(serializeData[p]);");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("____CallbackServerFunction(\"\",\"\",true,function(rs){");
                sb.AppendLine("var renderData=____jsonStringToObject(rs.substring(rs.indexOf(\"--BEGIN--\")+9,rs.indexOf(\"--END--\")));");
                sb.AppendLine("var count=renderData.length;");
                sb.AppendLine("for(i=0;i<count;i++){");
                sb.AppendLine("if(renderData[i][\"type\"]==\"control\"){");
                sb.AppendLine("var ct=document.getElementById(renderData[i][\"id\"]);");
                sb.AppendLine("if(ct!=null ){");
                sb.AppendLine("if(renderData[i][\"context\"]!=\"\")");
                sb.AppendLine("ct.outerHTML=renderData[i][\"context\"];");
                sb.AppendLine("else");
                sb.AppendLine("ct.style.display='none';");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("else if(renderData[i][\"type\"]==\"message\"){");
                sb.AppendLine("alert(renderData[i][\"context\"]);");
                sb.AppendLine("}");
                sb.AppendLine("else if(renderData[i][\"type\"]==\"script\"){");
                sb.AppendLine("eval(renderData[i][\"context\"]);");
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine("return false ;");
                sb.AppendLine("},serializeDataString);");
                sb.AppendLine("return ;");
                sb.AppendLine("}");
                sb.AppendLine("");
                return sb.ToString();
            }
        }
    }
}
