using System;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class CreateSiteInput : CreateOrUpdateSiteInputBase
    {
        public CreateSiteInput():base()
        {
            IsActive= true;
            Host = "https://";
        }
    }
}
