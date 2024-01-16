using System;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class CreateSiteInput : CreateOrUpdateSiteInputBase
    {
        public CreateSiteInput():base()
        {
            IsActive= true;
            HostUrl = "https://";
            Cultures.Add(new CreateOrUpdateCultureInput(true,"en"));
        }
    }
}
