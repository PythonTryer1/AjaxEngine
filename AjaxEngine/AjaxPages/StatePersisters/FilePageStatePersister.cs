using System;
using System.IO;
using System.Web.UI;


namespace AjaxEngine.AjaxPages.StatePersisters
{
    [Serializable]
    public class FilePageStatePersister : AjaxPageStatePersister
    {
        public FilePageStatePersister(Page page) : base(page) { }
        public override void Load()
        {
            if (!File.Exists(ViewStateFilePath))
            {
                base.ViewState = null;
                base.ControlState = null;
            }
            else
            {
                StreamReader sr = File.OpenText(ViewStateFilePath);
                string viewStateString = sr.ReadToEnd();
                sr.Close();
                LosFormatter los = new LosFormatter();
                Pair pair = (Pair)los.Deserialize(viewStateString);
                base.ViewState = pair.First;
                base.ControlState = pair.Second;
            }
        }
        public override void Save()
        {
            Pair pair = new Pair();
            if (base.ViewState != null)
                pair.First = base.ViewState;
            if (base.ControlState != null)
                pair.Second = base.ControlState;
            //
            LosFormatter los = new LosFormatter();
            StringWriter writer = new StringWriter();
            los.Serialize(writer, pair);
            StreamWriter sw = File.CreateText(ViewStateFilePath);
            sw.Write(writer.ToString());
            sw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public string ViewStateFilePath
        {
            get
            {
                string folderName = Path.Combine(Page.Request.PhysicalApplicationPath, "__Houfeng_AjaxEngine_ViewState");
                if (!Directory.Exists(folderName))
                    Directory.CreateDirectory(folderName);
                //
                string fileName = this.GetUniqueKey() + ".vs";
                return Path.Combine(folderName, fileName);
            }
        }
    }
}
