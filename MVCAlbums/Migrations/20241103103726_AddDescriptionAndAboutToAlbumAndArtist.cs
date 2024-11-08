using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAlbums.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndAboutToAlbumAndArtist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Artist",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Album",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "Artist");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Album");
        }
    }
}
