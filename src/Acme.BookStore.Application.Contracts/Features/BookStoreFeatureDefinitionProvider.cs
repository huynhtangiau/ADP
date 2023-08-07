using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Features;

namespace Acme.BookStore.Features
{
    public class BookStoreFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var myGroup = context.AddGroup(BookStoreFeatures.GroupName);

            myGroup.AddFeature(BookStoreFeatures.Books.SmartSearch, defaultValue: "false");
        }
    }
}
