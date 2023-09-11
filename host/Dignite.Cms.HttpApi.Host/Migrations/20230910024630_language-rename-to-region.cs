using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dignite.Cms.Migrations
{
    /// <inheritdoc />
    public partial class languagerenametoregion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "CmsSites");

            migrationBuilder.RenameColumn(
                name: "Languages",
                table: "CmsSites",
                newName: "Regions");

            migrationBuilder.RenameColumn(
                name: "Host",
                table: "CmsSites",
                newName: "HostUrl");

            migrationBuilder.RenameColumn(
                name: "EntryPage_Template",
                table: "CmsSections",
                newName: "Template");

            migrationBuilder.RenameColumn(
                name: "EntryPage_Route",
                table: "CmsSections",
                newName: "Route");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "CmsEntries",
                newName: "Region");

            migrationBuilder.RenameIndex(
                name: "IX_CmsEntries_SectionId_Language_Slug",
                table: "CmsEntries",
                newName: "IX_CmsEntries_SectionId_Region_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_CmsEntries_SectionId_Language_PublishTime_Status",
                table: "CmsEntries",
                newName: "IX_CmsEntries_SectionId_Region_PublishTime_Status");

            migrationBuilder.AlterColumn<string>(
                name: "Template",
                table: "CmsSections",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                table: "CmsSections",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Regions",
                table: "CmsSites",
                newName: "Languages");

            migrationBuilder.RenameColumn(
                name: "HostUrl",
                table: "CmsSites",
                newName: "Host");

            migrationBuilder.RenameColumn(
                name: "Template",
                table: "CmsSections",
                newName: "EntryPage_Template");

            migrationBuilder.RenameColumn(
                name: "Route",
                table: "CmsSections",
                newName: "EntryPage_Route");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "CmsEntries",
                newName: "Language");

            migrationBuilder.RenameIndex(
                name: "IX_CmsEntries_SectionId_Region_Slug",
                table: "CmsEntries",
                newName: "IX_CmsEntries_SectionId_Language_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_CmsEntries_SectionId_Region_PublishTime_Status",
                table: "CmsEntries",
                newName: "IX_CmsEntries_SectionId_Language_PublishTime_Status");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "CmsSites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "EntryPage_Template",
                table: "CmsSections",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "EntryPage_Route",
                table: "CmsSections",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
