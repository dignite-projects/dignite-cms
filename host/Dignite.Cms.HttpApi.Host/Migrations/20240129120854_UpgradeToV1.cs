using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dignite.Cms.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeToV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CmsEntries_SectionId_CreatorId_PublishTime_Status",
                table: "CmsEntries");

            migrationBuilder.DropIndex(
                name: "IX_CmsEntries_SectionId_Culture_PublishTime_Status",
                table: "CmsEntries");

            migrationBuilder.DropIndex(
                name: "IX_CmsEntries_SectionId_Culture_Slug",
                table: "CmsEntries");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CmsFields");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "CmsFields");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CmsFieldGroups");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "CmsFieldGroups");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CmsEntryTypes");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "CmsEntryTypes");

            migrationBuilder.DropColumn(
                name: "Revision_IsActive",
                table: "CmsEntries");

            migrationBuilder.DropColumn(
                name: "Revision_Version",
                table: "CmsEntries");

            migrationBuilder.RenameColumn(
                name: "HostUrl",
                table: "CmsSites",
                newName: "Host");

            migrationBuilder.RenameColumn(
                name: "Cultures",
                table: "CmsSites",
                newName: "Languages");

            migrationBuilder.RenameColumn(
                name: "FormName",
                table: "CmsFields",
                newName: "FormControlName");

            migrationBuilder.RenameColumn(
                name: "Revision_Notes",
                table: "CmsEntries",
                newName: "VersionNotes");

            migrationBuilder.RenameColumn(
                name: "Revision_InitialId",
                table: "CmsEntries",
                newName: "InitialVersionId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CmsFields",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivatedVersion",
                table: "CmsEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntries_CreatorId_SectionId_PublishTime_Status",
                table: "CmsEntries",
                columns: new[] { "CreatorId", "SectionId", "PublishTime", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntries_Culture_SectionId_PublishTime_Status",
                table: "CmsEntries",
                columns: new[] { "Culture", "SectionId", "PublishTime", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntries_Culture_SectionId_Slug",
                table: "CmsEntries",
                columns: new[] { "Culture", "SectionId", "Slug" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntries_SectionId",
                table: "CmsEntries",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CmsEntries_CreatorId_SectionId_PublishTime_Status",
                table: "CmsEntries");

            migrationBuilder.DropIndex(
                name: "IX_CmsEntries_Culture_SectionId_PublishTime_Status",
                table: "CmsEntries");

            migrationBuilder.DropIndex(
                name: "IX_CmsEntries_Culture_SectionId_Slug",
                table: "CmsEntries");

            migrationBuilder.DropIndex(
                name: "IX_CmsEntries_SectionId",
                table: "CmsEntries");

            migrationBuilder.DropColumn(
                name: "IsActivatedVersion",
                table: "CmsEntries");

            migrationBuilder.RenameColumn(
                name: "Languages",
                table: "CmsSites",
                newName: "Cultures");

            migrationBuilder.RenameColumn(
                name: "Host",
                table: "CmsSites",
                newName: "HostUrl");

            migrationBuilder.RenameColumn(
                name: "FormControlName",
                table: "CmsFields",
                newName: "FormName");

            migrationBuilder.RenameColumn(
                name: "VersionNotes",
                table: "CmsEntries",
                newName: "Revision_Notes");

            migrationBuilder.RenameColumn(
                name: "InitialVersionId",
                table: "CmsEntries",
                newName: "Revision_InitialId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CmsFields",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CmsFields",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "CmsFields",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CmsFieldGroups",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "CmsFieldGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CmsEntryTypes",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "CmsEntryTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Revision_IsActive",
                table: "CmsEntries",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Revision_Version",
                table: "CmsEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntries_SectionId_CreatorId_PublishTime_Status",
                table: "CmsEntries",
                columns: new[] { "SectionId", "CreatorId", "PublishTime", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntries_SectionId_Culture_PublishTime_Status",
                table: "CmsEntries",
                columns: new[] { "SectionId", "Culture", "PublishTime", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsEntries_SectionId_Culture_Slug",
                table: "CmsEntries",
                columns: new[] { "SectionId", "Culture", "Slug" });
        }
    }
}
