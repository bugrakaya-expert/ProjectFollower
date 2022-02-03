using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFollower.Extensions
{
    public class CookieExtensions
    {
        public static IHttpContextAccessor _hc;

        #region Filters

        public static class Filters
        {
            public static void SetFilters(bool archived)
            {
                _hc.HttpContext.Response.Cookies.Append("kj6ght", archived.ToString());
            }
            public static class GetFilters
            {
                static bool archived = Convert.ToBoolean(_hc.HttpContext.Request.Cookies["_kj6ght"]);
                public static bool Archived()
                {
                    return archived;
                }
            }
        }
        #endregion Filters
    }
}
