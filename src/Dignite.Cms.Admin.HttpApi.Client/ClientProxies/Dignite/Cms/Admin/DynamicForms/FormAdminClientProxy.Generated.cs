// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using Dignite.Cms.Admin.DynamicForms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

// ReSharper disable once CheckNamespace
namespace Dignite.Cms.Admin.DynamicForms;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IFormAdminAppService), typeof(FormAdminClientProxy))]
public partial class FormAdminClientProxy : ClientProxyBase<IFormAdminAppService>, IFormAdminAppService
{
    public virtual async Task<ListResultDto<FormControlDto>> GetFormControlsAsync()
    {
        return await RequestAsync<ListResultDto<FormControlDto>>(nameof(GetFormControlsAsync));
    }
}
