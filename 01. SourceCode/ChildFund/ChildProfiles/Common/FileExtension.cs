﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace ChildProfiles
{
    public static class FileExtension
    {
        /// <summary>
        /// Add versioned to file javascript
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static MvcHtmlString IncludeVersionedJs(this HtmlHelper helper, string filename)
        {
            string version = GetVersion(helper, filename);
            return MvcHtmlString.Create("<script type='text/javascript' src='" + filename + version + "'></script>");
        }

        /// <summary>
        /// Add versioned to file css
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static MvcHtmlString IncludeVersionedCss(this HtmlHelper helper, string filename)
        {
            string version = GetVersion(helper, filename);
            return MvcHtmlString.Create("<link href='" + filename + version + "' rel='stylesheet' />");
        }

        private static string GetVersion(this HtmlHelper helper, string filename)
        {
            var context = helper.ViewContext.RequestContext.HttpContext;

            if (context.Cache[filename] == null)
            {
                var physicalPath = context.Server.MapPath(filename);
                var version = $"?v={new FileInfo(physicalPath).LastWriteTime.ToString("MMddHHmmss")}";
                context.Cache.Add(filename, version, null,
                  DateTime.Now.AddMinutes(5), TimeSpan.Zero,
                  CacheItemPriority.Normal, null);
                return version;
            }
            else
            {
                return context.Cache[filename] as string;
            }
        }
    }
}