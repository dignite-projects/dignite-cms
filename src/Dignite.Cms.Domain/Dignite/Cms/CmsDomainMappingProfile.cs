using AutoMapper;
using Dignite.Cms.Entries;

namespace Dignite.Cms
{
    public class CmsDomainMappingProfile : Profile
    {
        public CmsDomainMappingProfile()
        {
            CreateMap<Entry, EntryEto>();
        }
    }
}
