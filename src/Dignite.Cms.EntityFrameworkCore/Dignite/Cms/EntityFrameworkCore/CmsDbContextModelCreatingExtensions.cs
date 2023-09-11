using Dignite.Abp.FieldCustomizing.EntityFrameworkCore;
using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Dignite.FileExplorer.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Encodings.Web;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.CmsKit.EntityFrameworkCore;

namespace Dignite.Cms.EntityFrameworkCore;

public static class CmsDbContextModelCreatingExtensions
{
    public static void ConfigureCms(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureCmsKit();
        builder.ConfigureFileExplorer();

        builder.Entity<Site>(site =>
        {
            //Configure table & schema name
            site.ToTable(CmsDbProperties.DbTablePrefix + "Sites", CmsDbProperties.DbSchema);

            site.ConfigureByConvention();

            //Properties
            site.Property(s => s.DisplayName).IsRequired().HasMaxLength(SiteConsts.MaxDisplayNameLength);
            site.Property(s => s.Name).IsRequired().HasMaxLength(SiteConsts.MaxNameLength);
            site.Property(s => s.HostUrl).IsRequired().HasMaxLength(SiteConsts.MaxHostUrlLength);
            site.Property(s => s.Regions).HasConversion(
                config => JsonSerializer.Serialize(config, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    }),
                jsonData => JsonSerializer.Deserialize<ICollection<SiteRegion>>(jsonData,new JsonSerializerOptions())
                );

            //Indexs
            site.HasIndex(s => s.Name);
        });

        builder.Entity<Section>(section =>
        {
            //Configure table & schema name
            section.ToTable(CmsDbProperties.DbTablePrefix + "Sections", CmsDbProperties.DbSchema);

            section.ConfigureByConvention();

            //Properties
            section.Property(s => s.DisplayName).IsRequired().HasMaxLength(SectionConsts.MaxDisplayNameLength);
            section.Property(s => s.Name).IsRequired().HasMaxLength(SectionConsts.MaxNameLength);
            section.Property(r => r.Route).IsRequired().HasMaxLength(SectionConsts.MaxPageRouteLength);
            section.Property(r => r.Template).IsRequired().HasMaxLength(SectionConsts.MaxPagetemplateLength);

            //Relations
            section.HasMany(s => s.EntryTypes).WithOne().HasForeignKey(et => et.SectionId);
            section.HasMany(s => s.Entries).WithOne().HasForeignKey(e => e.SectionId);
            section.HasOne(s => s.Site).WithMany().HasForeignKey(s => s.SiteId);

            //Indexs
            section.HasIndex(s => s.SiteId);
        });

        builder.Entity<EntryType>(entryType =>
        {
            //Configure table & schema name
            entryType.ToTable(CmsDbProperties.DbTablePrefix + "EntryTypes", CmsDbProperties.DbSchema);

            entryType.ConfigureByConvention();

            //Properties
            entryType.Property(et => et.DisplayName).IsRequired().HasMaxLength(EntryTypeConsts.MaxDisplayNameLength);
            entryType.Property(et => et.Name).IsRequired().HasMaxLength(EntryTypeConsts.MaxNameLength);
            entryType.Property(et => et.FieldTabs).HasConversion(
                config => JsonSerializer.Serialize(config, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                }),
                jsonData => JsonSerializer.Deserialize<IList<EntryFieldTab>>(jsonData, new JsonSerializerOptions())
                );

            //
            entryType.HasIndex(et=>et.SectionId);
        });

        builder.Entity<FieldGroup>(fieldGroup =>
        {
            //Configure table & schema name
            fieldGroup.ToTable(CmsDbProperties.DbTablePrefix + "FieldGroups", CmsDbProperties.DbSchema);

            fieldGroup.ConfigureByConvention();

            //Properties
            fieldGroup.Property(fg => fg.Name).IsRequired().HasMaxLength(FieldGroupConsts.MaxNameLength);
            fieldGroup.HasMany(fg => fg.Fields).WithOne().HasForeignKey(e => e.GroupId).OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Field>(field =>
        {
            //Configure table & schema name
            field.ToTable(CmsDbProperties.DbTablePrefix + "Fields", CmsDbProperties.DbSchema);

            field.ConfigureByConvention();
            field.ConfigureCustomizableFieldDefinitions();
        });

        builder.Entity<Entry>(entry =>
        {
            //Configure table & schema name
            entry.ToTable(CmsDbProperties.DbTablePrefix + "Entries", CmsDbProperties.DbSchema);

            entry.ConfigureByConvention();
            entry.ConfigureObjectCustomizedFields();

            entry.Property(e => e.Region).IsRequired().HasMaxLength(SiteConsts.MaxRegionLength);
            entry.Property(e => e.Title).IsRequired().HasMaxLength(EntryConsts.MaxTitleLength);
            entry.Property(e => e.Slug).IsRequired().HasMaxLength(EntryConsts.MaxSlugLength);
            entry.OwnsOne(
                    e => e.Revision,
                    e =>
                    {
                        e.Property(r => r.InitialId).IsRequired().HasColumnName("Revision_InitialId");
                        e.Property(r => r.Version).IsRequired().HasColumnName("Revision_Version");
                        e.Property(r => r.IsActive).IsRequired().HasColumnName("Revision_IsActive");
                        e.Property(r => r.Notes).HasMaxLength(EntryConsts.MaxRevisionNotesLength).HasColumnName("Revision_Notes");
                    });

            //Indexes
            entry.HasIndex(e => new { e.SectionId, e.Region, e.PublishTime, e.Status });
            entry.HasIndex(e => new { e.SectionId, e.CreatorId, e.PublishTime, e.Status });
            entry.HasIndex(e => new { e.SectionId, e.Region, e.Slug });
        });

        builder.TryConfigureObjectExtensions<CmsDbContext>();
    }
}
