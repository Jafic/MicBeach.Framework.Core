using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Utility
{
    public class CookieItem
    {
        public string Key
        { get; set; }

        public string Value
        { get; set; }

        public CookieOptions Options
        {
            get;set;
        }
    }
}
