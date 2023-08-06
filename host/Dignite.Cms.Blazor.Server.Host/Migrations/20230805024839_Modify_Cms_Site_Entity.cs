using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dignite.Cms.Blazor.Server.Host.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCmsSiteEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUrl",
                table: "CmsSites");

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "CmsSites",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Host",
                table: "CmsSites");

            migrationBuilder.AddColumn<string>(
                name: "BaseUrl",
                table: "CmsSites",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
