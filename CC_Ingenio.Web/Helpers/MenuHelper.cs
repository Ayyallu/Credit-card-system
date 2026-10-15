using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.Helpers
{
    public static class MenuHelper
    {
        public static MvcHtmlString Menu(this HtmlHelper Helper, List<UserEntity> Privileges)
        {
            StringBuilder sb = new StringBuilder();
            List<String> lsHeaders = new List<String>();

            foreach (UserEntity ue in Privileges)
            {
                if (String.IsNullOrEmpty(ue.ParentEntityCode)) //Main Headers
                {
                    while (lsHeaders.Count != 0) //clear all headers when a new main header starts
                    {
                        sb.Append("</ul></li>");
                        lsHeaders.RemoveAt(lsHeaders.Count - 1);
                    }

                    TagBuilder tbA = new TagBuilder("a");
                    tbA.AddCssClass("arrow");
                    tbA.MergeAttribute("href", "#");
                    TagBuilder tbI = new TagBuilder("i");
                    tbI.AddCssClass(ue.DisplayForm);
                    tbA.InnerHtml = tbI.ToString(TagRenderMode.Normal) + ResourceUtils.GetResourceString(ue.EntityCode);
                    lsHeaders.Add(ue.EntityCode);
                    sb.Append("<li>" + tbA.ToString(TagRenderMode.Normal) + "<ul>");
                }
                else
                {                         
                    //Delete last header in same level from header list when new header is opened
                    while (lsHeaders.Count != 0)
                    {
                        if (lsHeaders[lsHeaders.Count - 1] == ue.ParentEntityCode)
                            break;
                        sb.Append("</ul></li>");
                        lsHeaders.RemoveAt(lsHeaders.Count - 1);
                    }
                        
                    if (String.IsNullOrEmpty(ue.DisplayForm)) //Sub header
                    {
                        TagBuilder tbA = new TagBuilder("a");
                        tbA.AddCssClass("arrow");
                        tbA.MergeAttribute("href", "#");
                        tbA.InnerHtml = ResourceUtils.GetResourceString(ue.EntityCode);
                        lsHeaders.Add(ue.EntityCode);
                        sb.Append("<li>" + tbA.ToString(TagRenderMode.Normal) + "<ul>");
                    }
                    else//items
                    {
                        TagBuilder tbLi = new TagBuilder("li");
                        TagBuilder tbA = new TagBuilder("a");
                        tbA.MergeAttribute("href", ue.DisplayForm);
                        tbA.InnerHtml = ResourceUtils.GetResourceString(ue.EntityCode);
                        tbLi.InnerHtml = tbA.ToString(TagRenderMode.Normal);
                        sb.Append(tbLi.ToString(TagRenderMode.Normal));
                    }
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
                sb.Append("</ul></li>");

            //while (lsHeaders.Count != 1) //clear all headers when a new main header starts
            //{
            //    sb.Append("</ul></li>");
            //    lsHeaders.RemoveAt(lsHeaders.Count - 1);
            //}

            return new MvcHtmlString(sb.ToString());
        }
    }    
}