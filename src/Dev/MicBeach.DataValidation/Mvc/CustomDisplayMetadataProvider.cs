using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.DataValidation.Mvc
{
    public class CustomDisplayMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            DisplayText customDisplay = DisplayManager.GetDisplay(context.Key.ContainerType, context.Key.Name);
            if (customDisplay != null && !string.IsNullOrWhiteSpace(customDisplay.DisplayName))
            {
                context.DisplayMetadata.DisplayName = () => customDisplay.DisplayName;
            }
        }
    }
}
