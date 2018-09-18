using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskMasterCore.Web.Models.ViewModels;

namespace TaskMasterCore.Web.HtmlHelpers
{
    public static class ListHelper
    {
        public static HtmlString CreateList(this IHtmlHelper html, IList<RequestNav> list)
        {
            if (list != null)
            {
                string result = "<ul>";
                foreach (var item in list.Where(o => o.ParentId == null))
                {
                    result += "<li>";

                    result += GetForm(item.Id, item.Name);
                    result += GetChilds(item.Id, list);

                    result += "</li>";
                }
                result += "</ul>";
                return new HtmlString(result);
            }
            else return new HtmlString(string.Empty);
            
        }

        private static string GetChilds(int parentId, IList<RequestNav> list)
        {
            if (!list.Any(o => o.ParentId == parentId)) return string.Empty;

            string result = "<ul>";

            foreach (var item in list.Where(o => o.ParentId == parentId))
            {
                result += "<li>";

                result += GetForm(item.Id, item.Name);
                result += GetChilds(item.Id, list);

                result += "</li>";
            }

            result += "</ul>";

            return result;
        }

        private static string GetForm(int id, string name)
        {
            return string.Format(@"<a class""link"" href=""/Request/Index/{0}"">{1}</a>", id, name);//item.Name;
            //return string.Format(@"<form action =""/Request/Edit"" method =""post""><input type=""hidden"" name=""id"" value=""{0}""><input type=""submit"" value=""{1}""/></form>", id, name);
        }
    }
}
