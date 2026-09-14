using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Web.Helpers
{
    public static class HelpersUrl
    {
		public static string AbsoluteContent(this UrlHelper urlHelper, string contentPath)
		{
			// Build a URI for the requested path
			var url = new Uri(HttpContext.Current.Request.Url, urlHelper.Content(contentPath));
			// Return the absolute UrI
			return url.AbsoluteUri;
		}
	}
}