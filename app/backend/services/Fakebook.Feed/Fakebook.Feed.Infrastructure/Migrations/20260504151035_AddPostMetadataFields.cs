using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakebook.Feed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPostMetadataFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "feeling",
                table: "posts",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "posts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "feeling",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "location",
                table: "posts");
        }
    }
}
