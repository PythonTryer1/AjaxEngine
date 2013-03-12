using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace AjaxEngine.AjaxHandlers
{
    public interface IAjaxHandler : IHttpHandler, IServiceEntity, IRequiresSessionState
    {
    }
}
