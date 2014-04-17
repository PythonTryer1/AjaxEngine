/**********************************************************
 * 
 * 添加了Ajax能力的页面基类 
 * -------------------------------------------------------
 * 作者:Houfeng(侯锋)
 * Email:Houzhanfeng@gmail.com
 * 
 *********************************************************/

using System;
using System.Web.UI;
using StatePersisters = AjaxEngine.AjaxPages.StatePersisters;

namespace AjaxEngine.AjaxPages
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AjaxPageBase : System.Web.UI.Page, IAjaxPage
    {
        public AjaxPageEngine PageEngine { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PageEngine = new AjaxPageEngine(this);
        }
        /// <summary>
        /// 引发页面的Load事件
        /// </summary>
        /// <param name="e">包含事件数据的 EventArgs 对象</param>
        protected override void OnLoad(EventArgs e)
        {
            this.PageEngine.ProcessAjaxRequest();
            base.OnLoad(e);
        }
        /// <summary>
        /// 页面呈现方法
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            this.PageEngine.AjaxRender();
        }
        /// <summary>
        /// 验证是否Render在HtmlForm中（请不要重载此方法）
        /// </summary>
        /// <param name="control">控件</param>
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        /// <summary>
        /// 是否验证事件
        /// </summary>
        public override bool EnableEventValidation
        {
            get
            {
                return false;
            }
            set
            {
                base.EnableEventValidation = false;
            }
        }
        /// <summary>
        /// 提供操作视图状态的基本功能（一般不用重载）
        /// </summary>
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                return new StatePersisters.HiddenFieldPageStatePersister(this);
            }
        }

    }
}