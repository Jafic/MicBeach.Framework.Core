using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Security.Authentication.Cookie
{
    /// <summary>
    /// Cookie存储模式
    /// </summary>
    public enum CookieStorageModel
    {
        Default = 110,
        InMemory = 120,
        Distributed = 130
    }
}
