using MicBeach.DataValidation.Mvc;
using MicBeach.Web.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site.Cms.Config
{
    public static class MvcConfig
    {
        public static void Init()
        {
            MvcDataValidation.AddCustomDataAnnotationsModelValidatorProvider(ServiceProviderConfig.ServiceProvider);//添加自定义数据验证
            MvcDataValidation.AddCustomMetadataProvider(ServiceProviderConfig.ServiceProvider);
        }
    }
}
