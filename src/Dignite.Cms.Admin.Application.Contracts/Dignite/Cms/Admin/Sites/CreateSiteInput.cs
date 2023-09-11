using System;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class CreateSiteInput : CreateOrUpdateSiteInputBase
    {
        public CreateSiteInput():base()
        {
            IsActive= true;
            Regions.Add(new CreateOrUpdateRegionInput(true,"en"));
        }
    }
}
