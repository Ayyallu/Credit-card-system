using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.Helpers
{
    public static class BasicButtonsHelper
    {
        public static MvcHtmlString BasicButtons(this HtmlHelper Helper, List<string> Privileges)
        {
            TagBuilder tbDiv = new TagBuilder("div");
            tbDiv.AddCssClass("rowbtn");
            TagBuilder tbSpanNewStarting = new TagBuilder("span");
            TagBuilder tbButtonNew = new TagBuilder("input");
            TagBuilder tbSpanSaveStarting = new TagBuilder("span");
            TagBuilder tbButtonSave = new TagBuilder("input");
            TagBuilder tbSpanSubmitStarting = new TagBuilder("span");
            TagBuilder tbButtonSubmit = new TagBuilder("input");
            TagBuilder tbSpanDeleteStarting = new TagBuilder("span");
            TagBuilder tbButtonDelete = new TagBuilder("input");
            TagBuilder tbSpanAuthorizeStarting = new TagBuilder("span");
            TagBuilder tbButtonAuthorize = new TagBuilder("input");
            TagBuilder tbSpanResetStarting = new TagBuilder("span");
            TagBuilder tbButtonReset = new TagBuilder("input");


            if (Privileges.Contains("Add"))
            {
                tbButtonNew.MergeAttribute("id", "New");
                tbButtonNew.MergeAttribute("name", "Command");
                tbButtonNew.MergeAttribute("value", Resources.New);
                tbButtonNew.MergeAttribute("class", "Greenbutton");
                tbButtonNew.MergeAttribute("type", "button");
                tbSpanNewStarting.InnerHtml += tbButtonNew.ToString(TagRenderMode.SelfClosing);
                tbDiv.InnerHtml += tbSpanNewStarting.ToString(TagRenderMode.Normal);
            }
            if (Privileges.Contains("Add") )
            {
                tbButtonSave.MergeAttribute("id", "Save");
                tbButtonSave.MergeAttribute("name", "Command");
                tbButtonSave.MergeAttribute("value", Resources.Save);
                tbButtonSave.MergeAttribute("class", "Greenbutton");
                tbButtonSave.MergeAttribute("type", "submit");
                tbSpanSaveStarting.InnerHtml += tbButtonSave.ToString(TagRenderMode.SelfClosing);
                tbDiv.InnerHtml += tbSpanSaveStarting.ToString(TagRenderMode.Normal);

            }

            if ( Privileges.Contains("Update"))
            {            
                tbButtonSubmit.MergeAttribute("id", "Submit");
                tbButtonSubmit.MergeAttribute("name", "Command");
                tbButtonSubmit.MergeAttribute("value", Resources.Submit);
                tbButtonSubmit.MergeAttribute("class", "Greenbutton");
                tbButtonSubmit.MergeAttribute("type", "submit");
                tbSpanSubmitStarting.InnerHtml += tbButtonSubmit.ToString(TagRenderMode.SelfClosing);
                tbDiv.InnerHtml += tbSpanSubmitStarting.ToString(TagRenderMode.Normal);
            }

            if (Privileges.Contains("Delete"))
            {
                tbButtonDelete.MergeAttribute("id", "Delete");
                tbButtonDelete.MergeAttribute("name", "Command");
                tbButtonDelete.MergeAttribute("value", Resources.Delete);
                tbButtonDelete.MergeAttribute("class", "Greenbutton");
                tbButtonDelete.MergeAttribute("type", "submit");
                tbSpanDeleteStarting.InnerHtml += tbButtonDelete.ToString(TagRenderMode.SelfClosing);
                tbDiv.InnerHtml += tbSpanDeleteStarting.ToString(TagRenderMode.Normal);
            }

            if (Privileges.Contains("Authorize"))
            {
                tbButtonAuthorize.MergeAttribute("id", "Authorize");
                tbButtonAuthorize.MergeAttribute("value", Resources.Authorize);
                tbButtonAuthorize.MergeAttribute("class", "Greenbutton");
                tbButtonAuthorize.MergeAttribute("type", "button");
                tbSpanAuthorizeStarting.InnerHtml += tbButtonAuthorize.ToString(TagRenderMode.SelfClosing);
                tbDiv.InnerHtml += tbSpanAuthorizeStarting.ToString(TagRenderMode.Normal);
            }

            if (!Privileges.Contains("Authorize"))
            {
                tbButtonReset.MergeAttribute("id", "Reset");
                tbButtonReset.MergeAttribute("value", Resources.Cancel);
                tbButtonReset.MergeAttribute("class", "Greenbutton");
                tbButtonReset.MergeAttribute("type", "reset");
                tbSpanResetStarting.InnerHtml += tbButtonReset.ToString(TagRenderMode.SelfClosing);
                tbDiv.InnerHtml += tbSpanResetStarting.ToString(TagRenderMode.Normal);
            }

            //tbDiv.InnerHtml += tbSpanNewStarting.ToString(TagRenderMode.Normal);
            //tbDiv.InnerHtml += tbSpanSaveStarting.ToString(TagRenderMode.Normal);
            //tbDiv.InnerHtml += tbSpanSubmitStarting.ToString(TagRenderMode.Normal);
            //tbDiv.InnerHtml += tbSpanDeleteStarting.ToString(TagRenderMode.Normal);
            //tbDiv.InnerHtml += tbSpanAuthorizeStarting.ToString(TagRenderMode.Normal);
            //tbDiv.InnerHtml += tbSpanResetStarting.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(tbDiv.ToString(TagRenderMode.Normal));
        }
    }
}