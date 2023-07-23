using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Localization;

namespace Acme.BookStore.Localization
{
    public static class LocailizationHelper
    {
        public static string L(this string name)
        {
            return LocalizableString.Create<BookStoreResource>(name).ResourceName;
        }
    }
}
