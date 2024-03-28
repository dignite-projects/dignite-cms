using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Dignite.Cms;

[Dependency(ReplaceServices = true)]
public class CmsBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Dignite";
}
